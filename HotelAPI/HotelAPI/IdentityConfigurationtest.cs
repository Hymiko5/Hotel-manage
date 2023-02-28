using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace HotelAPI
{
    public class IdentityConfiguration
    {
        public static List<TestUser> testUsers = new List<TestUser> { new TestUser
            {
                SubjectId = "1144",
                Username = "mukesh",
                Password = "mukesh",
                Claims =
                {
                    new Claim(JwtClaimTypes.Name, "Mukesh Murugan"),
                    new Claim(JwtClaimTypes.GivenName, "Mukesh"),
                    new Claim(JwtClaimTypes.FamilyName, "Murugan"),
                    new Claim(JwtClaimTypes.WebSite, "http://codewithmukesh.com"),
                }
            }
        };

        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
            new ApiScope("hotelApi.read"),
            new ApiScope("hotelApi.write")
        };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("hotelApi")
            {
                Scopes = new List<string>{ "hotelApi.read", "hotelApi.write" },
                ApiSecrets = new List<Secret>
                {
                    new Secret("supersecret".Sha256())
                }
            }
        };

        public static IEnumerable<Client> Clients => new Client[]
        {
            new Client
            {
                ClientId = "cwm.client",
                ClientName = "Client Creadentials Client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AccessTokenType = AccessTokenType.Reference,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "hotelApi.read" }
            }
        };
    }
}
