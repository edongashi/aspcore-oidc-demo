﻿using System;
using System.Threading.Tasks;
using AuthServer.Data;
using OpenIddict.Core;
using OpenIddict.Models;
using Utils.Initialization;

namespace AuthServer.Infrastructure.Initialization
{
    public class ClientInitializer : IStartupService
    {
        private static string Combine(string address, string path)
        {
            if (path.StartsWith("/"))
            {
                if (address.EndsWith("/"))
                {
                    return address + path.Substring(1);
                }

                return address + path;
            }

            if (address.EndsWith("/"))
            {
                return address + path;
            }

            return address + "/" + path;
        }

        private readonly IAddressResolver addressResolver;
        private readonly ApplicationDbContext context;
        private readonly OpenIddictApplicationManager<OpenIddictApplication> manager;

        public ClientInitializer(
            IAddressResolver addressResolver,
            ApplicationDbContext context,
            OpenIddictApplicationManager<OpenIddictApplication> manager)
        {
            this.addressResolver = addressResolver;
            this.context = context;
            this.manager = manager;
        }

        public async Task InitializeAsync()
        {
            await context.Database.EnsureCreatedAsync();
            //if (await manager.FindByClientIdAsync("react") == null)
            //{
            //    var descriptor = new OpenIddictApplicationDescriptor
            //    {
            //        ClientId = "react",
            //        ClientSecret = "react_secret",
            //        DisplayName = "SPA client application",
            //        PostLogoutRedirectUris = { new Uri("http://localhost:3000/signout-callback-oidc") },
            //        RedirectUris = { new Uri("http://localhost:3000/signin-oidc") },
            //        Permissions =
            //        {
            //            OpenIddictConstants.Permissions.Endpoints.Authorization,
            //            OpenIddictConstants.Permissions.Endpoints.Logout,
            //            OpenIddictConstants.Permissions.GrantTypes.Implicit
            //        }
            //    };

            //    await manager.CreateAsync(descriptor);
            //}

            //if (await manager.FindByClientIdAsync("mvc") == null)
            //{
            //    var descriptor = new OpenIddictApplicationDescriptor
            //    {
            //        ClientId = "mvc",
            //        ClientSecret = "mvc_secret",
            //        DisplayName = "MVC client application",
            //        PostLogoutRedirectUris = { new Uri("http://localhost:3002/signout-callback-oidc") },
            //        RedirectUris = { new Uri("http://localhost:3002/signin-oidc") },
            //        Permissions =
            //            {
            //                OpenIddictConstants.Permissions.Endpoints.Authorization,
            //                OpenIddictConstants.Permissions.Endpoints.Logout,
            //                OpenIddictConstants.Permissions.Endpoints.Token,
            //                OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
            //                OpenIddictConstants.Permissions.GrantTypes.RefreshToken
            //            }
            //    };

            //    await manager.CreateAsync(descriptor);
            //}

            //if (await manager.FindByClientIdAsync("resource") == null)
            //{
            //    var descriptor = new OpenIddictApplicationDescriptor
            //    {
            //        ClientId = "resource",
            //        ClientSecret = "resource_secret",
            //        Permissions =
            //        {
            //            OpenIddictConstants.Permissions.Endpoints.Introspection
            //        }
            //    };

            //    await manager.CreateAsync(descriptor);
            //}

            if (await manager.FindByClientIdAsync("swagger") == null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = "swagger",
                    DisplayName = "Swagger",
                    RedirectUris = { new Uri(Combine(addressResolver.Resolve(), "/swagger/oauth2-redirect.html")) },
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Implicit
                    }
                };

                await manager.CreateAsync(descriptor);
            }
        }
    }
}
