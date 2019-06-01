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
using Models.Tokens.Repository;

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

            return NoContent();
        }

        [HttpPatch]
        public async Task<IActionResult> Refresh()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Substring(7);

            var principal = authenticator.GetPrincipalFromExpiredToken(token);
            var userId = principal.Claims.First(claim => claim.Type == "userId").Value;
            var refreshToken = principal.Claims.First(claim => claim.Type == "refreshToken").Value;

            if (userId == null)
            {
                var error = ServiceErrorResponses.InvalidClaims("userId");
                return BadRequest(error);
            }

            var refreshTokenExist = await tokenRepository.IsRefreshTokenExistAsync(userId, refreshToken);

            if (!refreshTokenExist)
            {
                return BadRequest("Invalid refresh token");
            }

            var newRefreshToken = authenticator.GenerateRefreshToken();
            var claims = new List<Claim>
            {
                new Claim("userId", userId),
                new Claim("refreshToken", newRefreshToken)
            };
            var newJwtToken = GenerateToken(claims);
            await tokenRepository.RemoveRefreshTokenAsync(userId, refreshToken);
            await tokenRepository.SaveRefreshTokenAsync(userId, newRefreshToken);

            return new ObjectResult(new
            {
                token = newJwtToken
            });
        }

        private static string GenerateToken(IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;
            
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
