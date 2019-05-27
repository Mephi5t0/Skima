using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using API.Auth;
using API.Errors;
using Client.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.Converters.Users;
using Models.Tokens.Repository;
using Models.Users;
using Models.Users.Repository;

namespace API.Controllers
{
    using Client = global::Client.Models;

    [Route("v1/auth/tokens")]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly TokenRepository tokenRepository;

        private readonly IAuthenticator authenticator;

        public AuthenticationController(TokenRepository tokenRepository,
            IAuthenticator authenticator)
        {
            this.tokenRepository = tokenRepository;
            this.authenticator = authenticator;
        }

        [HttpPost]
        public async Task<IActionResult> Token([FromBody] Client.Auth.TokenCreationInfo tokenCreationInfo,
            CancellationToken cancellationToken)
        {
            string encodedJwt;

            try
            {
                encodedJwt = await authenticator.AuthenticateAsync(tokenCreationInfo.Email, tokenCreationInfo.Password,
                    cancellationToken);
            }
            catch
            {
                return BadRequest("Invalid login or password");
            }

            var authResult = new AuthResult
            {
                Token = encodedJwt
            };
                
            return Ok(authResult);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Token()
        {
            var refreshToken = User.FindFirstValue("refreshToken");
            
            if (refreshToken == null)
            {
                return BadRequest("Refresh token was expected, but received null");
            }

            await tokenRepository.RemoveRefreshTokenAsync(refreshToken);

            return Ok("Refresh token removed successfully");
        }

        [HttpPatch]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = User.FindFirstValue("refreshToken");
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Substring(7);
            
            var principal = authenticator.GetPrincipalFromExpiredToken(token);
            var userId = principal.Claims.First(claim => claim.Type == "userId").ToString();
            
            if (userId == null)
            {
                var error = ServiceErrorResponses.InvalidClaims("userId");
                return BadRequest(error);
            }
            
            var savedRefreshToken = await tokenRepository.GetRefreshTokenAsync(userId);
            if (savedRefreshToken != refreshToken)
            {
                return BadRequest("Invalid refresh token");
            }

            var newJwtToken = GenerateToken(principal.Claims);
            var newRefreshToken = authenticator.GenerateRefreshToken();
            await tokenRepository.RemoveRefreshTokenAsync(userId, refreshToken);
            await tokenRepository.SaveRefreshTokenAsync(userId, newRefreshToken);

            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }

        private static string GenerateToken(IEnumerable<Claim> claims)
        {
            var jwt = new JwtSecurityToken(AuthOptions.ISSUER,
                AuthOptions.AUDIENCE,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(AuthOptions.LIFETIME),
                new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
