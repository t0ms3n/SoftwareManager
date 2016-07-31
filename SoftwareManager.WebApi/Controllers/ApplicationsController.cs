using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using SoftwareManager.BLL.Contracts.Models;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.BLL.Exceptions;
using SoftwareManager.WebApi.Extensions;
using SoftwareManager.WebApi.Helpers;
using SoftwareManager.WebApi.HttpActionResults;

namespace SoftwareManager.WebApi.Controllers
{
    [ODataRoutePrefix("Applications")]
    public class ApplicationsController : ODataBaseController
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [EnableQuery]
        [ODataRoute("")]
        public IHttpActionResult GetApplications(ODataQueryOptions<Application> queryOptions)
        {
            string[] membersToExpand = ODataQueryOptionsExtractor.GetNavigationPropertiesToExpand(queryOptions.SelectExpand);
            var query = _applicationService.FindApplications(membersToExpand);
            return Ok(query);
        }

        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select | AllowedQueryOptions.Expand)]
        [ODataRoute("({key})")]
        public IHttpActionResult GetApplication([FromODataUri] int key, ODataQueryOptions<Application> queryOptions)
        {
            var application = _applicationService.FindApplication(q => q.Id == key);

            if (!application.Any())
            {
                return NotFound();
            }

            string[] membersToExpand = ODataQueryOptionsExtractor.GetNavigationPropertiesToExpand(queryOptions.SelectExpand);
            var query = _applicationService.FindApplication(f => f.Id == key, membersToExpand);

            return Ok(SingleResult.Create(query));
        }

        [HttpGet]
        [ODataRoute("({key})/id")]
        [ODataRoute("({key})/identifier")]
        [ODataRoute("({key})/name")]
        public IHttpActionResult GetApplicationProperty([FromODataUri] int key)
        {
            var application = _applicationService.FindApplication(q => q.Id == key).FirstOrDefault();
            return GetObjectProperty(application);
        }

        [HttpGet]
        [ODataRoute("({key})/applicationVersions")]
        [ODataRoute("({key})/applicationApplicationManagers")]
        public IHttpActionResult GetApplicationCollection([FromODataUri] int key)
        {
            var collectionToGet = Url.Request.RequestUri.Segments.Last().FirstLetterToUpper();
            var application = _applicationService.FindApplication(f => f.Id == key, collectionToGet).FirstOrDefault();
            return GetObjectCollection(application, collectionToGet);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(Application application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _applicationService.CreateApplication(application);
            }
            catch (ModelValidationException modelValidationException)
            {

                return new BadRequestWithModelErrorResult(modelValidationException.ModelErrors);
            }

            return Created(application);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Application> application, ODataQueryOptions<Application> options)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (options.IfMatch == null
               || !options.IfMatch.ApplyTo(_applicationService.FindApplication(f => f.Id == key)).Any())
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }

            var currentApplication = await _applicationService.GetApplicationAsync(key);
            if (currentApplication == null)
            {
                return NotFound();
            }

            //Pass Delta to BLL and patch there?
            application.Patch(currentApplication);

            try
            {
                // Update
                await _applicationService.UpdateApplication(key, currentApplication);
            }
            catch (ModelValidationException modelValidationException)
            {

                return new BadRequestWithModelErrorResult(modelValidationException.ModelErrors);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key, ODataQueryOptions<Application> options)
        {
            // Delete only if it was not changed?
            if (options.IfMatch == null
               || !options.IfMatch.ApplyTo(_applicationService.FindApplication(f => f.Id == key)).Any())
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }

            try
            {
                await _applicationService.DeleteApplication(key);
            }
            catch (ItemNotFoundException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [ODataRoute("({key})/applicationApplicationManagers/$ref")]
        public async Task<IHttpActionResult> CreateApplicationManagerAssociation([FromODataUri] int key,
            [FromBody] Uri link)
        {
            int keyOfManagerToAdd = Request.GetKeyValue<int>(link);
            try
            {
                await _applicationService.CreateApplicationManagerAssociation(key, keyOfManagerToAdd);
            }
            catch (ItemNotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut]
        [ODataRoute("({key})/applicationApplicationManagers({relatedKey})/$ref")]
        public async Task<IHttpActionResult> UpdateApplicationManagerAssociation([FromODataUri] int key, [FromODataUri] int relatedKey,
            [FromBody] Uri link)
        {
            int keyOfManagerToAdd = Request.GetKeyValue<int>(link);
            try
            {
                await _applicationService.UpdateApplicationManagerAssociation(key, relatedKey, keyOfManagerToAdd);
            }
            catch (ItemNotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //DELETE odata/Applications('key')/ApplicationApplicationManagers/$ref?$id={'relatedUriWithRelatedKey'}
        [HttpDelete]
        [ODataRoute("({key})/applicationApplicationManagers({relatedKey})/$ref")]
        public async Task<IHttpActionResult> DeleteApplicationManagerAssociation([FromODataUri] int key, [FromODataUri] int relatedKey)
        {
            try
            {
                await _applicationService.DeleteApplicationManagerAssociation(key, relatedKey);
            }
            catch (ItemNotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
