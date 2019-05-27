using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Errors;
using Client.Models.Maraphone.Task;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Converters.Maraphone;
using Models.Maraphone.Task;
using Models.Maraphone.Task.Repository;
using ContentResult = Client.Models.Maraphone.Task.ContentResult;

namespace API.Controllers
{
    using Client = global::Client.Models;
    
    [Route("v1/content")]
    [Authorize]
    public class ContentController : Controller
    {
        private readonly ContentRepository contentRepository;

        public ContentController(ContentRepository contentRepository)
        {
            this.contentRepository = contentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Client.Maraphone.Task.ContentBuildInfo contentBuildInfo,
            CancellationToken cancellationToken)
        {
            if (contentBuildInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("ContentBuildInfo");
                return this.BadRequest(error);
            }

            var contentType = HttpContext.Request.Headers["Skima-Data-Type"].FirstOrDefault();

            if (contentType == null)
            {
                return this.BadRequest("Content not specified");
            }

            var contentCreationInfo = new ContentCreationInfo(contentType, contentBuildInfo.Data);
            var document = await contentRepository.CreateAsync(contentCreationInfo, cancellationToken);
            var result = new ContentResult
            {
                Id = document.Id
            };
            
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                var error = ServiceErrorResponses.InvalidRouteParameter("DocumentId");
                return this.BadRequest(error);
            }

            var modelContent = await contentRepository.GetAsync(id);
            if (modelContent == null)
            {
                var error = ServiceErrorResponses.NoSuchObject("Content", "Not content with such id " + id);
                return this.NotFound(error);
            }
            
            var clientContent = ContentConverter.Convert(modelContent);

            return Ok(clientContent);
        }
    }
}