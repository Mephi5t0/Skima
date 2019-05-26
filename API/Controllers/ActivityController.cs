using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using API.Errors;
using Client.Models.Activity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Activity;
using Models.Activity.Repository;
using Models.Converters.Activities;
using Models.Maraphone.Repository;

namespace API.Controllers
{
    using Client = global::Client.Models.Activity;
    
    [Route("v1/activities")]
    [Authorize]
    public class ActivityController : Controller
    {
        private readonly ActivityRepository activityRepository;
        private readonly MaraphoneRepository maraphoneRepository;


        public ActivityController(ActivityRepository activityRepository, MaraphoneRepository maraphoneRepository)
        {
            this.activityRepository = activityRepository;
            this.maraphoneRepository = maraphoneRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Client.ActivityBuildInfo buildInfo,
            CancellationToken cancellationToken)
        {
            if (buildInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("ActivityCreationInfo");
                return this.BadRequest(error);
            }
            var userId = User.FindFirstValue("userId");
            
            if (userId == null)
            {
                var error = ServiceErrorResponses.InvalidClaims("userId");
                return this.BadRequest(error);
            }
            
            var maraphoneId = buildInfo.MaraphoneId;
            var maraphone = await maraphoneRepository.GetAsync(maraphoneId, cancellationToken);
            if (maraphone == null)
            {
                var error = ServiceErrorResponses.NoSuchObject("Maraphone", "Maraphone not found");
                return this.NotFound(error);
            }
            
            var duration = maraphone.Duration;
            var endAt = buildInfo.StartAt + duration;
            
            var activityCreationInfo = new ActivityCreationInfo(buildInfo.MaraphoneId, buildInfo.Tags,
                userId, buildInfo.Experts, buildInfo.StartAt, endAt);

            var modelActivity = await activityRepository.CreateAsync(activityCreationInfo, endAt, cancellationToken);
            var clientActivity = ActivityConverter.Convert(modelActivity);

            return Created("https://skima.cf/v1/activities", clientActivity);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                var error = ServiceErrorResponses.InvalidRouteParameter("activityId");
                return this.BadRequest(error);
            }

            var modelActivity = await activityRepository.GetByIdAsync(id);

            if (modelActivity == null)
            {
                var message = "Activity with id " + id + " not found.";
                var error = ServiceErrorResponses.NoSuchObject("Activity", message);
                return this.NotFound(error);
            }
            var clientActivity = ActivityConverter.Convert(modelActivity);

            return Ok(clientActivity);
        }

        [HttpGet]
        public async Task<IActionResult> SearchAsync([FromQuery] Client.ActivityInfoSearchQuery infoSearchQuery, CancellationToken cancellationToken)
        {
            if (infoSearchQuery == null)
            {
                var error = ServiceErrorResponses.InvalidQuery("ActivityInfoSearchQuery");
                return this.BadRequest(error);
            }

            var modelQuery = ActivitySearchInfoQueryConverter.Convert(infoSearchQuery);
            var modelActivities = await activityRepository.SearchAsync(modelQuery, cancellationToken);
            var clientActivities = modelActivities.Select(ActivityConverter.Convert).ToList();
            var clientActivityList = new ActivityList
            {
                Activities = clientActivities
            };

            return Ok(clientActivityList);
        }
    }
}