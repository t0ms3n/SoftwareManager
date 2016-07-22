
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
using Mapster;

namespace SoftwareManager.WebApi.Controllers
{
    [ODataRoutePrefix("ApplicationVersions")]
    public class ApplicationVersionsController : ODataController
    {
        private readonly IApplicationVersionService _applicationVersionService;

        public ApplicationVersionsController(IApplicationVersionService applicationVersionService)
        {
            _applicationVersionService = applicationVersionService;
        }

        [HttpGet]
        [EnableQuery]
        [ODataRoute("")]
        public IQueryable<ApplicationVersion> GetApplicationVersions()
        {
            return _applicationVersionService.FindApplicationVersions();
        }

        [HttpGet]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.Select | AllowedQueryOptions.Expand)]
        [ODataRoute("({key})")]
        public IHttpActionResult GetApplicationVersion([FromODataUri] int key)
        {
            var applicationVersion = _applicationVersionService.FindApplicationVersion(key);

            if (!applicationVersion.Any())
            {
                return NotFound();
            }

            return Ok(SingleResult.Create(applicationVersion));
        }

        [HttpGet]
        [ODataRoute("({key})/id")]
        [ODataRoute("({key})/application")]
        public IHttpActionResult GetApplicationVersionProperty([FromODataUri] int key)
        {
            var applicationVersion = _applicationVersionService.FindApplicationVersion(key).FirstOrDefault();

            if (applicationVersion == null)
            {
                return NotFound();
            }

            var propertyToGet = Url.Request.RequestUri.Segments.Last();

            if (!applicationVersion.HasProperty(propertyToGet))
            {
                return NotFound();
            }

            var propertyValue = applicationVersion.GetValue(propertyToGet);

            if (propertyValue == null)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }

            return this.CreateOkHttpActionResult(propertyValue);
        }

        [HttpGet]
        //[ODataRoute("Set({key})/Collection")]
        public IHttpActionResult GetApplicationVersionCollection([FromODataUri] int key)
        {
            var collectionToGet = Url.Request.RequestUri.Segments.Last();

            var applicationVersion = _applicationVersionService.FindApplicationVersions(collectionToGet).FirstOrDefault(w => w.Id == key);

            if (applicationVersion == null)
            {
                return NotFound();
            }

            var collectionPropertyValue = applicationVersion.GetValue(collectionToGet);
            return this.CreateOkHttpActionResult(collectionPropertyValue);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Post(ApplicationVersion applicationVersion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _applicationVersionService.CreateApplicationVersion(applicationVersion);

            return Created(applicationVersion);
        }


        public async Task<IHttpActionResult> Put([FromODataUri] int key, ApplicationVersion applicationVersion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _applicationVersionService.UpdateApplicationVersion(key, applicationVersion);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ApplicationVersion> applicationVersion)
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

            // applicationVersion.Patch(currentApplicationVersion);
            // Erneuter Abruf der Entity ... Patch in BLL?
            await _applicationVersionService.UpdateApplicationVersion(key, currentApplicationVersion);

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            try
            {
                await _applicationVersionService.DeleteApplicationVersion(key);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
