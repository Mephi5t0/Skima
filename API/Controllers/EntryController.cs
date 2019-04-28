using System.Threading;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Mvc;
using Models.Converters.Entry;
using Models.Entries.Repository;

namespace API.Controllers
{
    using Client = global::Client.Models;

    public class EntryController : Controller
    {
        private readonly EntryRepository entryRepository;

        public EntryController(EntryRepository entryRepository)
        {
            this.entryRepository = entryRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Client.Entry.EntryCreationInfo entryCreationInfo,
            CancellationToken cancellationToken)
        {
            if (entryCreationInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("Entry");
                return this.BadRequest(error);
            }

            var modelEntry = EntryConverter.Convert(entryCreationInfo);
            var createdEntry = await entryRepository.CreateAsync(modelEntry, cancellationToken);

            return Ok(createdEntry);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromRoute] string id,
            CancellationToken cancellationToken)
        {
            if (id == null)
            {
                var error = ServiceErrorResponses.InvalidRouteParameter("EntryId");
                return this.BadRequest(error);
            }

            var entry = await entryRepository.GetAsync(id);
            var clientEntry = EntryConverter.ConvertToClientModel(entry);

            return Ok(clientEntry);
        }
    }
}