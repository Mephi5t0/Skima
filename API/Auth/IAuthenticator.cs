using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace API.Auth
{
    public interface IAuthenticator
    {
        Task<string> AuthenticateAsync(string login, string password, CancellationToken cancellationToken);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}