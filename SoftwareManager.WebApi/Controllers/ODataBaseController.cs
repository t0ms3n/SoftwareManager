using System.Linq;
using System.Web.Http;
using System.Web.OData;
using SoftwareManager.WebApi.Extensions;

namespace SoftwareManager.WebApi.Controllers
{
    public class ODataBaseController : ODataController
    {
        protected IHttpActionResult GetObjectCollection(object item, string collectionToGet)
        {
            if (item == null)
            {
                return NotFound();
            }

            var collectionPropertyValue = item.GetValue(collectionToGet);
            return this.CreateOkHttpActionResult(collectionPropertyValue);
        }

        protected IHttpActionResult GetObjectProperty(object item)
        {
            if (item == null)
            {
                return NotFound();
            }

            var propertyToGet = Url.Request.RequestUri.Segments.Last().FirstLetterToUpper();

            if (!item.HasProperty(propertyToGet))
            {
                return NotFound();
            }

            var propertyValue = item.GetValue(propertyToGet);

            if (propertyValue == null)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }

            return this.CreateOkHttpActionResult(propertyValue);
        }
    }
}