using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using SoftwareManager.BLL.Contracts.Models;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.BLL.Exceptions;
using SoftwareManager.BLL.Models;
using SoftwareManager.WebApi.Extensions;
using SoftwareManager.WebApi.Helpers;
using SoftwareManager.WebApi.HttpActionResults;

namespace SoftwareManager.WebApi.Controllers
{
    [ODataRoutePrefix("ApplicationManagers")]

    public class ApplicationManagersController : ODataBaseController
    {
        private readonly IApplicationManagerService _applicationManagerService;

        public ApplicationManagersController(IApplicationManagerService applicationManagerService)
        {
            _applicationManagerService = applicationManagerService;
        }

        [HttpGet]
        [EnableQuery]
        [ODataRoute("")]
        public IHttpActionResult GetApplicationManagers(ODataQueryOptions<ApplicationManager> queryOptions)
        {
            string[] membersToExpand = ODataQueryOptionsExtractor.GetNavigationPropertiesToExpand(queryOptions.SelectExpand);
            var query = _applicationManagerService.FindApplicationManagers(membersToExpand);
            return Ok(query);
        }

        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select | AllowedQueryOptions.Expand)]
        [ODataRoute("({key})")]
        public IHttpActionResult GetApplicationManager([FromODataUri] int key, ODataQueryOptions<ApplicationManager> queryOptions)
        {
            var applicationManager = _applicationManagerService.FindApplicationManager(q => q.Id == key);

            if (!applicationManager.Any())
            {
                return NotFound();
            }

            string[] membersToExpand = ODataQueryOptionsExtractor.GetNavigationPropertiesToExpand(queryOptions.SelectExpand);
            var query = _applicationManagerService.FindApplicationManager(f => f.Id == key, membersToExpand);

            return Ok(SingleResult.Create(query));
        }

        [HttpGet]
        [ODataRoute("({key})/id")]
        [ODataRoute("({key})/isActive")]
        [ODataRoute("({key})/isAdmin")]
        [ODataRoute("({key})/name")]
        [ODataRoute("({key})/loginName")]
        public IHttpActionResult GetApplicationManagerProperty([FromODataUri] int key)
        {
            var applicationManager = _applicationManagerService.FindApplicationManager(f => f.Id == key).FirstOrDefault();
            return GetObjectProperty(applicationManager);
        }

        [HttpGet]
        [ODataRoute("({key})/createdApplicationVersions")]
        [ODataRoute("({key})/createdApplications")]
        [ODataRoute("({key})/mangerOfApplications")]
        public IHttpActionResult GetApplicationManagerCollection([FromODataUri] int key)
        {
            var collectionToGet = Url.Request.RequestUri.Segments.Last().FirstLetterToUpper();
            var applicationManager = _applicationManagerService.FindApplicationManagers(collectionToGet).FirstOrDefault(w => w.Id == key);

            return GetObjectCollection(applicationManager, collectionToGet);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(ApplicationManager applicationManager)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _applicationManagerService.CreateApplicationManager(applicationManager);
            }
            catch (ModelValidationException modelValidationException)
            {

                return new BadRequestWithModelErrorResult(modelValidationException.ModelErrors);
            }


            return Created(applicationManager);
        }



        //public async Task<IHttpActionResult> Put([FromODataUri] int key, ApplicationManager applicationManager)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        await _applicationManagerService.UpdateApplicationManager(key, applicationManager);
        //    }
        //    catch (EntityNotFoundException)
        //    {
        //        return NotFound();
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ApplicationManager> applicationManager, ODataQueryOptions<ApplicationManager> options)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentApplicationManager = await _applicationManagerService.GetApplicationManagerAsync(key);
            if (currentApplicationManager == null)
            {
                return NotFound();
            }

            if (options.IfMatch == null
                || !options.IfMatch.ApplyTo(_applicationManagerService.FindApplicationManager(f => f.Id == key)).Any())
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }

            //Pass Delta to BLL and patch there?
            applicationManager.Patch(currentApplicationManager);


            try
            {
                // Update           
                await _applicationManagerService.UpdateApplicationManager(key, currentApplicationManager);
            }
            catch (ModelValidationException modelValidationException)
            {

                return new BadRequestWithModelErrorResult(modelValidationException.ModelErrors);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key, ODataQueryOptions<ApplicationManager> options)
        {
            if (options.IfMatch == null
               || !options.IfMatch.ApplyTo(_applicationManagerService.FindApplicationManager(f => f.Id == key)).Any())
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }

            try
            {
                await _applicationManagerService.DeleteApplicationManager(key);
            }
            catch (ItemNotFoundException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
