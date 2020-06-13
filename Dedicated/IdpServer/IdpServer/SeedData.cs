using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdpServer.ConfigModels;
using IdpServer.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdpServer
{
    public class SeedData
    {
        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            var database = serviceProvider.GetRequiredService<ApplicationDbContext>().Database;

            //if (!await database.CanConnectAsync())
            //{
            //    throw new Exception("Unable to connect with database.");
            //}

            await database.MigrateAsync();
            await serviceProvider.GetRequiredService<ConfigurationDbContext>().Database.MigrateAsync();
            await serviceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();
            await AddClients(serviceProvider);
        }

        private static async Task AddClients(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<ConfigurationDbContext>();

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var spaClientExists = await dbContext.Clients.AnyAsync(x => x.ClientId == "internal::js");

            if (!spaClientExists)
            {
                var spaUrl = serviceProvider.GetRequiredService<IOptions<AppConfig>>().Value.ClientUrls.Angular;

                var spaClient = new Client
                {
                    ClientId = "internal::js",
                    ClientName = "JS Clients",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    AllowedCorsOrigins = { $"{spaUrl}" },
                    RedirectUris = { $"{spaUrl}/signin-callback", $"{spaUrl}/assets/oidc/silent-renew.html" },
                    PostLogoutRedirectUris = { $"{spaUrl}" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "all"
                    }
                };

                await dbContext.Clients.AddAsync(spaClient.ToEntity());

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
