using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using moore.Data.ViewModels;
using moore.Models;
using System.Security.Claims;

namespace moore.Data
{
    public class Datainit
    {
        //initilization and data seeding class 
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new mooreContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<mooreContext>>()))
            {
                // Look for any Orders first if yes dont do it if not seed the data.
                
                if (context.Order.Any())
                {
                    return;   // DB has been seeded
                }
                // order data seeding 5 items seeded to database 
                context.Order.AddRange(
                    new Order
                    {

                        //UserId = 1,
                        OrderDate = DateTime.Parse("2035-2-12"),
                        OrderTotal = 268.83

                    },

                    new Order
                    {

                        OrderDate = DateTime.Parse("2031-3-13"),
                        OrderTotal = 3400.00
                        //UserId = 1,
                    },

                    new Order
                    {

                        OrderDate = DateTime.Parse("2024-2-23"),
                        OrderTotal = 5260.12
                        // UserId = 2,
                    },

                    new Order
                    {

                        OrderDate = DateTime.Parse("2031-4-15"),
                        OrderTotal = 4110.41
                        // UserId = 3,
                    },
                     new Order
                     {

                         OrderDate = DateTime.Parse("2041-4-15"),
                         OrderTotal = 1211.551
                         // UserId = 3,
                     }
                );

               // same as order 5 elements or items to be seeded to product db 

                if (context.Product.Any())
                {
                    return;   // DB has been seeded
                }
                context.Product.AddRange(
                    new Product
                    {

                        Name ="Visual studio",
                        Description = "Host only ( use it on your own device)",
                        Price = 22 ,
                        Discount = 5 ,
                        DateCreated = DateTime.Parse("2023-4-15"),
                        Quantity = 5

                    }, new Product
                    {

                        Name = "Microsoft Office",
                        Description = "Host only ( use it on your own device)",
                        Price = 22,
                        Discount = 5,
                        DateCreated = DateTime.Parse("2034-4-15"),
                        Quantity = 5

                    }, new Product
                    {

                        Name = "SharpShooter",
                        Description = "Host only ( use it on your own device)",
                        Price = 22,
                        Discount = 5,
                        DateCreated = DateTime.Parse("2055-4-15"),
                        Quantity = 5

                    }, new Product
                    {

                        Name = "Visual studio",
                        Description = "Cloud & Host (can be used on our cloud servers)",
                        Price = 22,
                        Discount = 5,
                        DateCreated = DateTime.Parse("2044-4-15"),
                        Quantity = 5

                    }, new Product
                    {

                        Name = "Microsoft Office",
                        Description = "Cloud & Host (can be used on our cloud servers)",
                        Price = 22,
                        Discount = 5,
                        DateCreated = DateTime.Parse("2055-4-15"),
                        Quantity = 5

                    }



                    );
                context.SaveChanges();

                // same as order 5 elements or items to be seeded to orderdetail db 

                if (context.OrderDetail.Any())
                {
                    return;   // DB has been seeded
                }
                context.OrderDetail.AddRange(
                    new OrderDetail
                    {
                        Quantity = 1,
                        Price = 12,
                        OrderID  = context.Order.FirstOrDefault(o => o.OrderDate == DateTime.Parse("2035-2-12")).OrderID,
                        ProductID = context.Product.FirstOrDefault(p => p.Name == "Microsoft Office" && p.Description.Contains("Cloud & Host (can be used on our cloud servers)")).ProductId
                    },
                    new OrderDetail
                    {
                        Quantity = 2,
                        Price = 13,
                        OrderID = context.Order.FirstOrDefault(o => o.OrderDate == DateTime.Parse("2031-3-13")).OrderID,
                        ProductID = context.Product.FirstOrDefault(p => p.Name == "SharpShooter" && p.Description.Contains("Host only ( use it on your own device)")).ProductId
                    },
                     new OrderDetail
                     {
                         Quantity = 4,
                         Price = 12,
                         OrderID = context.Order.FirstOrDefault(o => o.OrderDate == DateTime.Parse("2024-2-23")).OrderID,
                         ProductID = context.Product.FirstOrDefault(p => p.Name == "Microsoft Office" && p.Description.Contains("Host only ( use it on your own device)")).ProductId


                     },
                  new OrderDetail
                  {

                      Quantity = 1,
                      Price = 12,
                      OrderID = context.Order.FirstOrDefault(o => o.OrderDate == DateTime.Parse("2031-4-15")).OrderID,
                      ProductID = context.Product.FirstOrDefault(p => p.Name == "Visual studio" && p.Description.Contains("Host only ( use it on your own device)")).ProductId

                  },
                       new OrderDetail
                       {

                           Quantity = 2,
                           Price = 12,
                           OrderID = context.Order.FirstOrDefault(o => o.OrderDate == DateTime.Parse("2041-4-15")).OrderID,
                           ProductID = context.Product.FirstOrDefault(p => p.Name == "Visual studio" && p.Description.Contains("Cloud & Host (can be used on our cloud servers)")).ProductId

                       }

                    ) ;
                //save the changes 
               context.SaveChanges();
            }
        }
        //This code is used to seed users and roles in an application using Identity Framework. It creates four roles (admin and user) and creates several users with various claims and roles. 
        public static async Task SeedingUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
            {
                using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
                {

                //Roles
                //the role manager and checking if the admin and user roles exist; if they don't, it creates them
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    if (!await roleManager.RoleExistsAsync(UserRole.Admin))
                        await roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
                    if (!await roleManager.RoleExistsAsync(UserRole.User))
                        await roleManager.CreateAsync(new IdentityRole(UserRole.User));


                //Users
                //user manager and creates 6 users with unique emails, user names, and claims so 2 admins 3 users or 4 users .
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    string adminUserEmail = "admin@mooore.com";
                    var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                // Create a new ApplicationUser object with the values from the registration form
                // to pass the values or seed the data
                if (adminUser == null)
                    {
                        var newAdminUser = new ApplicationUser
                        {
                            FirstName = "Admin",
                            LastName = "User",
                            UserName = "admin-user",
                            Email = adminUserEmail,
                            Address = "safkasffa",
                            PhoneNumber = "123123",
                            EmailConfirmed = true,
                            TwoFactorEnabled = false,
                            PhoneNumberConfirmed = false,
                            LockoutEnabled = false,
                            AccessFailedCount = 0,
                        };
                    //add the password
                        await userManager.CreateAsync(newAdminUser, "1qaz!QAZ");
                    //pass the role as admin
                        await userManager.AddToRoleAsync(newAdminUser, UserRole.Admin);
                    }
                
                string adminUserEmail1 = "admin1@mooore.com";
                var adminUser1 = await userManager.FindByEmailAsync(adminUserEmail1);

                if (adminUser1 == null)
                {
                    var newAdminUser1 = new ApplicationUser
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        UserName = "admin-user1",
                        Email = adminUserEmail1,
                        Address = "safkasffa",
                        PhoneNumber = "123123",
                        EmailConfirmed = true,
                        TwoFactorEnabled = false,
                        PhoneNumberConfirmed = false,
                        LockoutEnabled = false,
                        AccessFailedCount = 0,
                    };
                    await userManager.CreateAsync(newAdminUser1, "1qaz!QAZ");
                    await userManager.AddToRoleAsync(newAdminUser1, UserRole.Admin);
                }

                //----------------
                string appUserEmail = "user@mooore.com";
                    var appUser = await userManager.FindByEmailAsync(appUserEmail);

                    if (appUser == null)
                    {
                    

                        var newAppUser = new ApplicationUser
                        {
                            
                            FirstName = "Application-user ",
                            LastName = appUserEmail,
                            UserName = "app-user",
                            Email = appUserEmail,
                            Address = "safkasffa",
                            PhoneNumber = "123123",
                            EmailConfirmed = true,
                            TwoFactorEnabled = false,
                            PhoneNumberConfirmed = false,
                            LockoutEnabled = false,
                            AccessFailedCount = 0,
                            IsSubscriptionPaid= true
                        };
                        await userManager.CreateAsync(newAppUser, "1qaz!QAZ");
                        await userManager.AddToRoleAsync(newAppUser, UserRole.User);
                    await userManager.AddClaimAsync(newAppUser, new Claim("SubscriptionType", newAppUser.IsSubscriptionPaid ? "Paid" : "Unpaid"));
                

                }
                //----------------
                string appUserEmail1 = "user1@mooore.com";
                var appUser1 = await userManager.FindByEmailAsync(appUserEmail1);

                if (appUser1 == null)
                {


                    var newAppUser = new ApplicationUser
                    {

                        FirstName = "Application ",
                        LastName = "User",
                        UserName = "app-user1",
                        Email = appUserEmail1,
                        Address = "safkasffa",
                        PhoneNumber = "123123",
                        EmailConfirmed = true,
                        TwoFactorEnabled = false,
                        PhoneNumberConfirmed = false,
                        LockoutEnabled = false,
                        AccessFailedCount = 0,
                        IsSubscriptionPaid = false
                    };
                    await userManager.CreateAsync(newAppUser, "1qaz!QAZ");
                    await userManager.AddToRoleAsync(newAppUser, UserRole.User);
                    await userManager.AddClaimAsync(newAppUser, new Claim("SubscriptionType", newAppUser.IsSubscriptionPaid ? "Paid" : "Unpaid"));


                }
                //----------------
                string appUserEmail2 = "user2@mooore.com";
                var appUser2 = await userManager.FindByEmailAsync(appUserEmail2);

                if (appUser2 == null)
                {


                    var newAppUser = new ApplicationUser
                    {

                        FirstName = "Application ",
                        LastName = "User",
                        UserName = "app-user2",
                        Email = appUserEmail2,
                        Address = "safkasffa",
                        PhoneNumber = "123123",
                        EmailConfirmed = true,
                        TwoFactorEnabled = false,
                        PhoneNumberConfirmed = false,
                        LockoutEnabled = false,
                        AccessFailedCount = 0,
                        IsSubscriptionPaid = true
                    };
                    await userManager.CreateAsync(newAppUser, "1qaz!QAZ");
                    await userManager.AddToRoleAsync(newAppUser, UserRole.User);
                    await userManager.AddClaimAsync(newAppUser, new Claim("SubscriptionType", newAppUser.IsSubscriptionPaid ? "Paid" : "Unpaid"));


                }
                //----------------
                string appUserEmail3 = "user3@mooore.com";
                var appUser3 = await userManager.FindByEmailAsync(appUserEmail3);

                if (appUser3 == null)
                {


                    var newAppUser = new ApplicationUser
                    {

                        FirstName = "Application ",
                        LastName = "User",
                        UserName = "app-user3",
                        Email = appUserEmail3,
                        Address = "safkasffa",
                        PhoneNumber = "123123",
                        EmailConfirmed = true,
                        TwoFactorEnabled = false,
                        PhoneNumberConfirmed = false,
                        LockoutEnabled = false,
                        AccessFailedCount = 0,
                        IsSubscriptionPaid = false
                    };
                    await userManager.CreateAsync(newAppUser, "1qaz!QAZ");
                    await userManager.AddToRoleAsync(newAppUser, UserRole.User);
                    await userManager.AddClaimAsync(newAppUser, new Claim("SubscriptionType", newAppUser.IsSubscriptionPaid ? "Paid" : "Unpaid"));


                }


            }
               

            }
       
    }
}