using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using SoftwareManager.BLL.Models;

namespace SoftwareManager.WebApi.HttpActionResults
{
    public class BadRequestWithModelErrorResult : IHttpActionResult
    {
        private readonly IList<ModelError> _errors;

        public BadRequestWithModelErrorResult(IList<ModelError> errors )
        {
            this._errors = errors;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response.Content = new ObjectContent<IList<ModelError>>(_errors, new JsonMediaTypeFormatter());
            return Task.FromResult(response);
        }
    }
}