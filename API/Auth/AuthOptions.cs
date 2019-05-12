using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace API.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "Skima";
        public const string AUDIENCE = "https://www.skima.cf:5061/";
        public const int LIFETIME = 30;
        
        const string KEY = "Qsd2a!faT#Q@RQRS14fw@Fas@v!^FA@rAS21[QSXZhr@qfe";

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}