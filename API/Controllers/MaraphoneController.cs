using System.Threading;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Mvc;
using Models.Converters.Activities;
using Models.Converters.Maraphone;
using Models.Maraphone;
using Models.Maraphone.Repository;

namespace API.Controllers
{
    using Client = global::Client.Models;
    
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
            var clientMaraphone = MaraphoneConverter.ConvertToClientModel(modelMaraphone);

            return Ok(clientMaraphone);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Client.Maraphone.MaraphoneCreationInfo creationInfo,
            CancellationToken cancellationToken)
        {
            if (creationInfo == null)
            {
                var error = ServiceErrorResponses.InvalidRouteParameter("maraphoneId");
                return this.BadRequest(error);
            }

            var modelMaraphone = await maraphoneRepository.CreateAsync(creationInfo, cancellationToken);
            var clientMaraphone = MaraphoneConverter.ConvertToClientModel(modelMaraphone);

            return CreatedAtRoute("GetMaraphone", clientMaraphone);
        }
    }
}