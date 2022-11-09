using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace MY.IDP.Settings
{
    public class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser()
                {
                    SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                    Username = "123",
                    Password = "123",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "MohammadJavad"),
                        new Claim("family_name", "Tavakoli"),
                        new Claim("address", "Main street"),
                        new Claim("role", "PayingUser"),
                        new Claim("subscriptionlevel", "PayingUser"),
                        new Claim("country", "ir")
                    }
                },
                new TestUser()
                {
                    SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                    Username = "321",
                    Password = "123",

                    Claims = new List<Claim>
                    {
                        new Claim("given_name", "User 2"),
                        new Claim("family_name", "Test"),
                        new Claim("address", "Big Street 2"),
                        new Claim("role", "FreeUser"),
                        new Claim("subscriptionlevel", "FeeUser"),
                        new Claim("country", "be")
                    }
                }
            };
        }

        //scope
        public static IEnumerable<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource(
                    name: "roles",
                    userClaims: new List<string>() { "role" },
                    displayName: "Your role(s)"),
                new IdentityResource(
                    name: "country",
                    userClaims: new List<string>() { "country" },
                    displayName: "The country you're living in"),
                new IdentityResource(
                    name: "subscriptionlevel",
                    userClaims: new List<string>() { "subscriptionlevel" },
                    displayName: "Your subscription level")
            };
        }
        // api-related resources (scopes)

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("imagegalleryapi", "ERP BACKEND", new List<string> { "role" })
                {
                    Scopes = new List<string>()
                    {
                        "imagegalleryapi.access"
                    },
                    ApiSecrets = { new Secret("APISecret".Sha256()) }
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope(name: "imagegalleryapi.access", displayName: "Access API Backend")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "Image Gallery",
                    //Refrence Token
                    AccessTokenType = AccessTokenType.Reference,
                    ClientId = "imagegalleryclient",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:5076/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:5076/signout-callback-oidc"
                    },
                    AllowedCorsOrigins = { "https://localhost:5076" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "roles",
                        "imagegalleryapi.access",
                        "country",
                        "subscriptionlevel"
                    },

                    AuthorizationCodeLifetime = 300,
                    AccessTokenLifetime = 3600,

                    AllowOfflineAccess = true,
                    // AbsoluteRefreshTokenLifetime = 2592000 , //Defaults to 2592000 seconds / 30 days
                    // RefreshTokenExpiration = TokenExpiration.Sliding,
                    UpdateAccessTokenClaimsOnRefresh = true,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RequireConsent = true,
                    RequirePkce = false,
                }
            };
        }
    }
}