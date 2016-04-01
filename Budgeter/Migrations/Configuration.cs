namespace CF_Budgeter.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using CF_Budgeter.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<CF_Budgeter.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CF_Budgeter.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

            var uStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(uStore);

            if (userManager.FindByEmail("austin.torres@colorado.edu") != null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "austin.torres@colorado.edu",
                    Email = "austin.torres@colorado.edu",
                    FirstName = "Austin",
                    LastName = "Torres",

                }, "TA1234ta!!");
            }

            var userId = userManager.FindByEmail("austin.torres@colorado.edu").Id;
            userManager.AddToRole(userId, "Admin");
            {
                userManager.AddToRole(userId, "Admin");
            }
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
