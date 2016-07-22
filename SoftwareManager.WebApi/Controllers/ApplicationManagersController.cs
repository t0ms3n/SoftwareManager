using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using SoftwareManager.BLL.Exceptions;
using SoftwareManager.BLL.Services;
using SoftwareManager.Entities;
using SoftwareManager.WebApi.Validators;

namespace SoftwareManager.WebApi.Controllers
{
    [ODataRoutePrefix("ApplicationManagers")]

    public class ApplicationManagersController : ODataController
    {
        private readonly IApplicationManagerService _applicationManagerService;

        public ApplicationManagersController(IApplicationManagerService applicationManagerService)
        {
            _applicationManagerService = applicationManagerService;
        }

        [HttpGet]
        [EnableQuery]
        [ODataRoute("")]
        public IQueryable<ApplicationManager> GetApplicationManagers()
        {
            return _applicationManagerService.FindApplicationManagers();
        }

        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select | AllowedQueryOptions.Expand)]
        [ODataRoute("({key})")]
        public IHttpActionResult GetApplicationManager([FromODataUri] int key)
        {
            var applicationManager = _applicationManagerService.FindApplicationManager(key);

            if (!applicationManager.Any())
            {
                return NotFound();
            }

            return Ok(SingleResult.Create(applicationManager));
        }

        [HttpGet]
        //[ODataRoute("({key})/id")]
        //[ODataRoute("({key})/isActive")]
        //[ODataRoute("({key})/isAdmin")]
        //[ODataRoute("({key})/name")]
        //[ODataRoute("({key})/loginName")]
        public IHttpActionResult GetApplicationManagerProperty([FromODataUri] int key)
        {
            var applicationManager = _applicationManagerService.FindApplicationManager(key).FirstOrDefault();
            if (applicationManager == null)
            {
                return NotFound();
            }

            var propertyToGet = Url.Request.RequestUri.Segments.Last();

            if (!applicationManager.HasProperty(propertyToGet))
            {
                return NotFound();
            }

            var propertyValue = applicationManager.GetValue(propertyToGet);

            if (propertyValue == null)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }

            return this.CreateOkHttpActionResult(propertyValue);
        }

        [HttpGet]
        //[ODataRoute("Set({key})/Collection")]
        public IHttpActionResult GetApplicationManagerCollection([FromODataUri] int key)
        {
            var collectionToGet = Url.Request.RequestUri.Segments.Last();

            var applicationManager = _applicationManagerService.FindApplicationManagers(collectionToGet).FirstOrDefault(w => w.Id == key);

            if (applicationManager == null)
            {
                return NotFound();
            }

            var collectionPropertyValue = applicationManager.GetValue(collectionToGet);
            return this.CreateOkHttpActionResult(collectionPropertyValue);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(ApplicationManager applicationManager)
        {
            var validator = new ApplicationManagerValidator();
            var validationResult = validator.Validate(applicationManager);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
            }

            await _applicationManagerService.CreateApplicationManager(applicationManager);

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
                || !options.IfMatch.ApplyTo(_applicationManagerService.FindApplicationManager(key)).Any())
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }          

            applicationManager.Patch(currentApplicationManager);

            var validator = new ApplicationManagerValidator();
            var validationResult = validator.Validate(currentApplicationManager);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Erneuter Abruf der Entity ... Patch in BLL?
            await _applicationManagerService.UpdateApplicationManager(key, currentApplicationManager);

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key, ODataQueryOptions<ApplicationManager> options)
        {
            if (options.IfMatch == null
               || !options.IfMatch.ApplyTo(_applicationManagerService.FindApplicationManager(key)).Any())
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }

            try
            {
                await _applicationManagerService.DeleteApplicationManager(key);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
