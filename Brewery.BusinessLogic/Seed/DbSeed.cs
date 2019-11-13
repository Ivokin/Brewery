using Brewery.Core.Constants;
using Brewery.Infrastructure.Entities;
using Brewery.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Brewery.BusinessLogic.Service
{
    public class DbSeed
    {
        public static void Seed(IServiceProvider services)
        {
            using (var serviceScope = services.CreateScope())
            {
                ApplicationDbContext context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                UserManager<IdentityUser> userManager = serviceScope.ServiceProvider.GetService<UserManager<IdentityUser>>();

                IdentityResult roleResult;
                if (roleManager == null || userManager == null)
                {
                    throw new ArgumentNullException();
                }

                var roleCheck = roleManager.RoleExistsAsync(AccountConstants.AdminRole);
                if (!roleCheck.Result)
                {
                    roleResult = roleManager.CreateAsync(new IdentityRole(AccountConstants.AdminRole)).Result;
                }

                roleCheck = roleManager.RoleExistsAsync(AccountConstants.Employee);
                if (!roleCheck.Result)
                {
                    roleResult = roleManager.CreateAsync(new IdentityRole(AccountConstants.Employee)).Result;
                }

                IdentityUser user = userManager.FindByEmailAsync(AccountConstants.AdminEmail).Result;
                if (user != null)
                {
                    userManager.AddToRoleAsync(user, AccountConstants.AdminRole).Wait();
                }

                if (context != null && context.Resources != null && !context.Resources.Any())
                {
                    ICollection<Resource> newResource = new Collection<Resource>()
                        {
                            new Resource()
                            {
                                Name = "Hops",
                                AmountInStock = 10,
                                Link = "https://en.wikipedia.org/wiki/Hops",
                                Unit = UnitConstants.Kg
                            },
                             new Resource()
                             {
                                Name = "Barley",
                                AmountInStock  = 20,
                                Link = "https://en.wikipedia.org/wiki/Barley",
                                Unit = UnitConstants.Kg
                             },
                             new Resource()
                             {
                                Name = "Water",
                                AmountInStock  = 30,
                                Link = "https://en.wikipedia.org/wiki/Water",
                                Unit = UnitConstants.Litre
                             },
                             new Resource()
                             {
                                Name = "Yeast",
                                AmountInStock  = 40,
                                Link = "https://en.wikipedia.org/wiki/Yeast",
                                Unit = UnitConstants.Kg
                        }
                    };
                    context.Resources.AddRange(newResource);
                }

                context.SaveChanges();
            }
        }
    }
}
