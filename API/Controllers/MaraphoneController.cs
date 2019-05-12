using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Mvc;
using Models.Converters.Maraphone;
using Models.Maraphone;
using Models.Maraphone.Repository;

namespace API.Controllers
{
    using Client = global::Client.Models;
    
    [Route("v1/maraphones")]
    public class MaraphoneController : Controller
    {
        private readonly MaraphoneRepository maraphoneRepository;

        public MaraphoneController(MaraphoneRepository maraphoneRepository)
        {
            this.maraphoneRepository = maraphoneRepository;
        }
        
        [HttpGet(Name = "GetMaraphone")]
        public async Task<IActionResult> GetAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                var error = ServiceErrorResponses.InvalidRouteParameter("maraphoneId");
                return this.BadRequest(error);
            }

            var modelMaraphone = await maraphoneRepository.GetAsync(id, cancellationToken);

            if (modelMaraphone == null)
            {
                var message = "Maraphone with id " + id + " not found.";
                var error = ServiceErrorResponses.NoSuchObject("Maraphone", message);
                return this.NotFound(error);
            }
            var clientMaraphone = MaraphoneConverter.Convert(modelMaraphone);

            return Ok(clientMaraphone);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Client.Maraphone.MaraphoneBuildInfo buildInfo,
            CancellationToken cancellationToken)
        {
            if (buildInfo == null)
            {
                var error = ServiceErrorResponses.InvalidRouteParameter("maraphoneId");
                return this.BadRequest(error);
            }
            var userId = User.FindFirstValue("userId");
            
            if (userId == null)
            {
                var error = ServiceErrorResponses.InvalidClaims("userId");
                return this.BadRequest(error);
            }

            var maraphoneCreationInfo = new MaraphoneCreationInfo(buildInfo, userId);
            
            var modelMaraphone = await maraphoneRepository.CreateAsync(maraphoneCreationInfo, cancellationToken);
            var clientMaraphone = MaraphoneConverter.Convert(modelMaraphone);

            return CreatedAtRoute("GetMaraphone", clientMaraphone);
        }
    }
}