using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Application
{
    public class Config
    {
        public static ApiResource Resource { get; } = new ApiResource("ApiScope", "API");

        public static IEnumerable<ApiResource> GetApiResources()
        {
            yield return Resource;
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "api-client",
                    ClientName = "Api Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    //RedirectUris           = { "http://localhost:5002/signin-oidc" },
                    // PostLogoutRedirectUris = { "/api/Account/Logout" },
                    // LogoutUri = "/api/Account/Logout",
                    AllowedScopes = new List<string>(){ Resource.Name},
                    // AllowedCorsOrigins = {"null", Program.HomeUrl},
                    AccessTokenLifetime = 60 * 10
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "xishuai",
                    Password = "123",
                    Claims = new List<Claim>
                    {
                        new Claim("name", "xishuai"),
                        new Claim("website", "http://xishuai.cnblogs.com")
                    }
                }
            };
        }
    }
}
