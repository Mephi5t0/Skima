﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Models.Tokens.Repository;
using Models.Users.Repository;

namespace API.Auth
{
    public class Authenticator : IAuthenticator
    {
        private readonly UserRepository userRepository;
        private readonly TokenRepository tokenRepository;

        public Authenticator(UserRepository userRepository, TokenRepository tokenRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
        }

        public async Task<string> AuthenticateAsync(string email, string password, CancellationToken cancellationToken)
        {
            var identity = await GetIdentity(email, password);

            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return await Task.FromResult(encodedJwt);
        }

        public static string HashPassword(string password)
        {
            using (var md5 = MD5.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = md5.ComputeHash(passwordBytes);
                var hash = BitConverter.ToString(hashBytes);
                return hash;
            }
        }

        private async Task<IReadOnlyCollection<Claim>> GetIdentity(string email, string password)
        {
            List<Claim> claims = null;
            var user = await userRepository.GetByEMailAsync(email);
            var refreshToken = GenerateRefreshToken();

            if (user != null)
            {
                var passwordHash = Authenticator.HashPassword(password);
                if (passwordHash == user.PasswordHash)
                {
                    claims = new List<Claim>
                    {
                        new Claim("userId", user.Id),
                        new Claim("refreshToken", refreshToken)
                    };
                    
                    await tokenRepository.SaveRefreshTokenAsync(user.Id, refreshToken);
                }
            }

            return claims;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}