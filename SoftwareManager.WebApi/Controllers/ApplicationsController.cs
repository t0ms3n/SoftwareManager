using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    [ODataRoutePrefix("Applications")]
    public class ApplicationsController : ODataController
    {
        private readonly IApplicationService _applicationService;      

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [EnableQuery]
        [ODataRoute("")]
        public IHttpActionResult GetApplications()
        {
            return Ok(_applicationService.FindApplications());
        }

        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select | AllowedQueryOptions.Expand)]
        [ODataRoute("({key})")]
        public IHttpActionResult GetApplication([FromODataUri] int key)
        {
            var application = _applicationService.FindApplication(key);

            if (!application.Any())
            {
                return NotFound();
            }

            return Ok(SingleResult.Create(application));
        }

        [HttpGet]
        //[ODataRoute("({key})/id")]
        //[ODataRoute("({key})/applicationIdentifier")]
        //[ODataRoute("({key})/name")]
        public IHttpActionResult GetApplicationProperty([FromODataUri] int key)
        {
            var application = _applicationService.FindApplication(key).FirstOrDefault();

            if (application == null)
            {
                return NotFound();
            }

            var propertyToGet = Url.Request.RequestUri.Segments.Last();

            if (!application.HasProperty(propertyToGet))
            {
                return NotFound();
            }

            var propertyValue = application.GetValue(propertyToGet);

            if (propertyValue == null)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }

            return this.CreateOkHttpActionResult(propertyValue);
        }

        [HttpGet]
        //[ODataRoute("({key})/applicationVersions")]
        //[ODataRoute("({key})/applicationApplicationManagers")]
        public IHttpActionResult GetApplicationCollection([FromODataUri] int key)
        {
            var collectionToGet = Url.Request.RequestUri.Segments.Last();

            var application = _applicationService.FindApplications(collectionToGet).FirstOrDefault(w => w.Id == key);

            if (application == null)
            {
                return NotFound();
            }

            var collectionPropertyValue = application.GetValue(collectionToGet);
            return this.CreateOkHttpActionResult(collectionPropertyValue);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(Application application)
        {
            ValidateModel(application);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            await _applicationService.CreateApplication(application);

            return Created(application);
        }


        //public async Task<IHttpActionResult> Put([FromODataUri] int key, Application application)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        await _applicationService.UpdateApplication(key, application);
        //    }
        //    catch (EntityNotFoundException)
        //    {
        //        return NotFound();
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Application> application, ODataQueryOptions<Application> options)
        {
            if (options.IfMatch == null
               || !options.IfMatch.ApplyTo(_applicationService.FindApplication(key)).Any())
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }

            var currentApplication = await _applicationService.GetApplicationAsync(key);
            if (currentApplication == null)
            {
                return NotFound();
            }

            application.Patch(currentApplication);

            ValidateModel(currentApplication);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Erneuter Abruf der Entity ... Patch in BLL?
            await _applicationService.UpdateApplication(key, currentApplication);

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key, ODataQueryOptions<Application> options)
        {
            if (options.IfMatch == null
               || !options.IfMatch.ApplyTo(_applicationService.FindApplication(key)).Any())
            {
                return StatusCode(HttpStatusCode.PreconditionFailed);
            }

            try
            {
                await _applicationService.DeleteApplication(key);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        //[ODataRoute("({key})/applicationApplicationManagers/$ref")]
        public async Task<IHttpActionResult> CreateApplicationManagerAssociation([FromODataUri] int key,
            [FromBody] Uri link)
        {
            int keyOfManagerToAdd = Request.GetKeyValue<int>(link);
            try
            {
                await _applicationService.CreateApplicationManagerAssociation(key, keyOfManagerToAdd);
            }
            catch (EntityNotFoundException)
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
        //[ODataRoute("({key})/applicationApplicationManagers({relatedKey})/$ref")]
        public async Task<IHttpActionResult> UpdateApplicationManagerAssociation([FromODataUri] int key, [FromODataUri] int relatedKey,
            [FromBody] Uri link)
        {
            int keyOfManagerToAdd = Request.GetKeyValue<int>(link);
            try
            {
                await _applicationService.UpdateApplicationManagerAssociation(key, relatedKey, keyOfManagerToAdd);
            }
            catch (EntityNotFoundException)
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
        //[ODataRoute("({key})/applicationApplicationManagers({relatedKey})/$ref")]
        public async Task<IHttpActionResult> DeleteApplicationManagerAssociation([FromODataUri] int key, [FromODataUri] int relatedKey)
        {
            try
            {
                await _applicationService.DeleteApplicationManagerAssociation(key, relatedKey);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        private void ValidateModel(Application item)
        {
            var validator = new ApplicationValidator();
            var validationResult = validator.Validate(item);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
        }
    }
}
