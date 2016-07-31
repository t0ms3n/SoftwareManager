using SoftwareManager.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData.Batch;

namespace SoftwareManager.WebApi.Handlers
{
    /// <summary>
    /// Custom batch handler specialized to execute batch changeset in OData $batch requests with transactions.
    /// The requests will be executed in the order they arrive, that means that the client is responsible for
    /// correctly ordering the operations to satisfy referential constraints.
    /// </summary>
    public class EntityFrameworkBatchHandler : DefaultODataBatchHandler
    {
        public EntityFrameworkBatchHandler(HttpServer httpServer)
            : base(httpServer)
        {
        }

        /// <summary>
        /// Executes the batch request and associates a <see cref="ISoftwareManagerUoW"/>instance with all the requests of 
        /// a single changeset and wraps the execution of the whole changeset within a transaction.
        /// </summary>
        /// <param name="requests">The <see cref="ODataBatchRequestItem"/> instances of this batch request.</param>
        /// <param name="cancellation">The <see cref="CancellationToken"/> associated with the request.</param>
        /// <returns>The list of responses associated with the batch request.</returns>
        public override async Task<IList<ODataBatchResponseItem>> ExecuteRequestMessagesAsync(
            IEnumerable<ODataBatchRequestItem> requests,
            CancellationToken cancellation)
        {
            if (requests == null)
            {
                throw new ArgumentNullException("requests");
            }

            IList<ODataBatchResponseItem> responses = new List<ODataBatchResponseItem>();
            try
            {
                foreach (ODataBatchRequestItem request in requests)
                {
                    OperationRequestItem operation = request as OperationRequestItem;
                    if (operation != null)
                    {
                        responses.Add(await request.SendRequestAsync(Invoker, cancellation));
                    }
                    else
                    {
                        await ExecuteChangeSet((ChangeSetRequestItem)request, responses, cancellation);
                    }
                }
            }
            catch
            {
                foreach (ODataBatchResponseItem response in responses)
                {
                    if (response != null)
                    {
                        response.Dispose();
                    }
                }
                throw;
            }

            return responses;
        }

        private async Task ExecuteChangeSet(
            ChangeSetRequestItem changeSet,
            IList<ODataBatchResponseItem> responses,
            CancellationToken cancellation)
        {
            var uow = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ISoftwareManagerUoW)) as
                ISoftwareManagerUoW;

            using (var transaction = uow.Begin())
            {
                var changeSetResponse = (ChangeSetResponseItem)await changeSet.SendRequestAsync(Invoker, cancellation);
                responses.Add(changeSetResponse);

                if (changeSetResponse.Responses.All(r => r.IsSuccessStatusCode))
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
        }
    }
}
