using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Converters.Entry;
using Models.Entries;
using Models.Entries.Repository;

namespace API.Controllers
{
    using Client = global::Client.Models;

    [Route("v1/entries")]
    [Authorize]
    public class EntryController : Controller
    {
        private readonly EntryRepository entryRepository;

        public EntryController(EntryRepository entryRepository)
        {
            this.entryRepository = entryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Client.Entry.EntryBuildInfo entryBuildInfo,
            CancellationToken cancellationToken)
        {
            if (entryBuildInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("Entry");
                return this.BadRequest(error);
            }
            var userId = User.FindFirstValue("userId");
            
            if (userId == null)
            {
                var error = ServiceErrorResponses.InvalidClaims("userId");
                return this.BadRequest(error);
            }

            var entryCreationInfo = new EntryCreationInfo(userId, entryBuildInfo.ActivityId);
            Entry modelEntry;
            try
            {
                modelEntry = await entryRepository.CreateAsync(entryCreationInfo, cancellationToken);
            }
            catch (EntryDuplicationException e)
            {
                return BadRequest(e.Message);
            }
            
            var clientEntry = EntryConverter.Convert(modelEntry);
            
            return CreatedAtRoute(
                "GetEntry", 
                new { id = clientEntry.Id },
                clientEntry);
        }

        [HttpGet("{id}", Name = "GetEntry")]
        public async Task<IActionResult> GetAsync([FromRoute] string id,
            CancellationToken cancellationToken)
        {
            if (id == null)
            {
                var error = ServiceErrorResponses.InvalidRouteParameter("EntryId");
                return this.BadRequest(error);
            }

            var entry = await entryRepository.GetAsync(id);
            var clientEntry = EntryConverter.Convert(entry);

            return Ok(clientEntry);
        }
    }
}