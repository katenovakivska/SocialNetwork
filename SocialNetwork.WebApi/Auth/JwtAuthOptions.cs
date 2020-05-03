using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SocialNetwork.PL.Auth
{
    public class JwtAuthOptions
    {
        private const string SECURITY_KEY = "super_secure_key_of_auction_application";

        public const int LIFETIME_MINUTES = 60;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY));
        }
    }
}
