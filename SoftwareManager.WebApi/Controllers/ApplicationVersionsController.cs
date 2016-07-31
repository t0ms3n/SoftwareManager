using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using SoftwareManager.BLL.Exceptions;
using SoftwareManager.BLL.Contracts.Models;
using SoftwareManager.BLL.Contracts.Services;
using SoftwareManager.WebApi.Extensions;
using SoftwareManager.WebApi.Helpers;
using SoftwareManager.WebApi.HttpActionResults;

namespace SoftwareManager.WebApi.Controllers
{
    [ODataRoutePrefix("ApplicationVersions")]
    public class ApplicationVersionsController : ODataBaseController
    {
        private readonly IApplicationVersionService _applicationVersionService;

        public ApplicationVersionsController(IApplicationVersionService applicationVersionService)
        {
            _applicationVersionService = applicationVersionService;
        }

        [HttpGet]
        [EnableQuery]
        [ODataRoute("")]
        public IHttpActionResult GetApplicationVersions(ODataQueryOptions<ApplicationVersion> queryOptions)
        {
            string[] membersToExpand = ODataQueryOptionsExtractor.GetNavigationPropertiesToExpand(queryOptions.SelectExpand);
            var query = _applicationVersionService.FindApplicationVersions(membersToExpand);
            return Ok(query);
        }

        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select | AllowedQueryOptions.Expand)]
        [ODataRoute("({key})")]
        public IHttpActionResult GetApplicationVersion([FromODataUri] int key, ODataQueryOptions<ApplicationVersion> queryOptions)
        {
            var applicationVersion = _applicationVersionService.FindApplicationVersion(f => f.Id == key);

            if (!applicationVersion.Any())
            {
                return NotFound();
            }

            string[] membersToExpand = ODataQueryOptionsExtractor.GetNavigationPropertiesToExpand(queryOptions.SelectExpand);
            var query = _applicationVersionService.FindApplicationVersion(f => f.Id == key, membersToExpand);

            return Ok(SingleResult.Create(query));
        }

        [HttpGet]
        [ODataRoute("({key})/id")]
        [ODataRoute("({key})/versionNumber")]
        [ODataRoute("({key})/applicationId")]
        public IHttpActionResult GetApplicationVersionProperty([FromODataUri] int key)
        {
            var applicationVersion = _applicationVersionService.FindApplicationVersion(q => q.Id == key).FirstOrDefault();
            return GetObjectProperty(applicationVersion);
        }

        [HttpGet]
        //[ODataRoute("Set({key})/Collection")]
        public IHttpActionResult GetApplicationVersionCollection([FromODataUri] int key)
        {
            var collectionToGet = Url.Request.RequestUri.Segments.Last().FirstLetterToUpper();
            var applicationVersion = _applicationVersionService.FindApplicationVersions(f => f.Id == key, collectionToGet).FirstOrDefault();
            return GetObjectCollection(applicationVersion, collectionToGet);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(ApplicationVersion applicationVersion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _applicationVersionService.CreateApplicationVersion(applicationVersion);
            }
            catch (ModelValidationException modelValidationException)
            {

                return new BadRequestWithModelErrorResult(modelValidationException.ModelErrors);
            }

            return Created(applicationVersion);
        }


        //public async Task<IHttpActionResult> Put([FromODataUri] int key, ApplicationVersion applicationVersion)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        await _applicationVersionService.UpdateApplicationVersion(key, applicationVersion);
        //    }
        //    catch (EntityNotFoundException)
        //    {
        //        return NotFound();
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ApplicationVersion> applicationVersion, ODataQueryOptions<ApplicationVersion> options)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentApplicationVersion = await _applicationVersionService.GetApplicationVersionAsync(key);
            if (currentApplicationVersion == null)
            {
                return NotFound();
            }

            if (options.IfMatch == null
               || !options.IfMatch.ApplyTo(_applicationVersionService.FindApplicationVersion(f => f.Id == key)).Any())
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }

            //Pass Delta to BLL and patch there?
            applicationVersion.Patch(currentApplicationVersion);

            try
            {
                // Update
                await _applicationVersionService.UpdateApplicationVersion(key, currentApplicationVersion);
            }
            catch (ModelValidationException modelValidationException)
            {

                return new BadRequestWithModelErrorResult(modelValidationException.ModelErrors);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            try
            {
                await _applicationVersionService.DeleteApplicationVersion(key);
            }
            catch (ItemNotFoundException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
