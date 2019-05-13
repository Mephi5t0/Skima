using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using API.Auth;
using API.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Converters.Users;
using Models.Users;
using Models.Users.Repository;

namespace API.Controllers
{
    using Client = global::Client.Models;
    
    [Route("v1/users")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserRepository userRepository;

        public UserController(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Client.Users.UserRegistrationInfo registrationInfo,
            CancellationToken cancellationToken)
        {
            if (registrationInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("UserRegistrationInfo");
                return this.BadRequest(error);
            }

            User result;
            var creationInfo = new UserCreationInfo(Authenticator.HashPassword(registrationInfo.Password), registrationInfo.FirstName,
                registrationInfo.LastName, registrationInfo.Email, registrationInfo.Phone);

            try
            {
                result = await userRepository.CreateAsync(creationInfo, cancellationToken);
            }
            catch (UserDuplicationException)
            {
                var error = ServiceErrorResponses.ConflictLogin(creationInfo.Email);
                return this.Conflict(error);
            }

            var clientUser = UserConverter.Convert(result);

            return CreatedAtRoute("GetUser", clientUser);
        }

        [HttpGet(Name = "GetUser")]
        public async Task<IActionResult> GetAsync([FromRoute] string id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                var error = ServiceErrorResponses.InvalidRouteParameter("userId");
                return this.BadRequest(error);
            }

            var modelUser = await userRepository.GetByIdAsync(id);
            
            if (modelUser == null)
            {
                var error = ServiceErrorResponses.NoSuchObject("User", "User not found");
                return NotFound(error);
            }
            var clientUserInfo = UserConverter.ConvertToUserInfo(modelUser);

            return Ok(clientUserInfo);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateAsync([FromBody] Client.Users.UserPatchInfo patchInfo, CancellationToken cancellationToken)
        {
            if (patchInfo == null)
            {
                var error = ServiceErrorResponses.BodyIsMissing("UserPatchInfo");
                return this.BadRequest(error);
            }
            
            var userId = User.FindFirstValue("userId");
            
            if (userId == null)
            {
                var error = ServiceErrorResponses.InvalidClaims("userId");
                return this.BadRequest(error);
            }

            User modelUser;
            
            try
            {
                modelUser = await userRepository.PatchAsync(patchInfo, userId, cancellationToken);
            }
            catch (UserNotFoundException e)
            {
                var error = ServiceErrorResponses.NoSuchObject("User", e.Message);
                return this.NotFound(error);
            }

            var clientUserInfo = UserConverter.ConvertToUserInfo(modelUser);

            return Ok(clientUserInfo);
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync()
        {
            var userId = User.FindFirstValue("userId");
            
            if (userId == null)
            {
                var error = ServiceErrorResponses.InvalidClaims("userId");
                return this.BadRequest(error);
            }

            await userRepository.RemoveAsync(userId);

            return NoContent();
        }    
    }
}