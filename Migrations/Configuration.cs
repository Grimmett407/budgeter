namespace budgeter.Migrations
{
    using budgeter.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<budgeter.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(budgeter.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));


            //Admin
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                roleManager.Create(new IdentityRole { Name = "Member" });
            }

            if (!context.Users.Any(u => u.Email == "cgrimmett407@gmail.com"))
            {
                userManager.Create(new ApplicationUser         //Creating new user for the application using required fields
                {
                    UserName = "cgrimmett407@gmail.com",
                    DisplayName = "Christian Grimmett",
                    Email = "cgrimmett407@gmail.com",
                    FirstName = "Christian",
                    LastName = "Grimmett",
                }, "Chris407!!");
            }

            //Members
            if (!context.Users.Any(u => u.Email == "ewatkins@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ewatkins@coderfoundry.com",
                    DisplayName = "E. Watkins",
                    Email = "ewatkins@coderfoundry.com",
                    FirstName = "Eric",
                    LastName = "Watkins",
                }, "password1!");
            }

            var adminId = userManager.FindByEmail("cgrimmett407@gmail.com").Id;
            userManager.AddToRole(adminId, "Admin");

            var moderatorId = userManager.FindByEmail("ewatkins@coderfoundry.com").Id;
            userManager.AddToRole(moderatorId, "Member");

            //Dropdown Items for Types
            context.Types.AddOrUpdate(t => t.Id,
               new budgeter.Models.CodeFirst.Type() { Id = 1, Name = "Income" },
               new budgeter.Models.CodeFirst.Type() { Id = 2, Name = "Expense" }
               );

            //Dropdown Items for categories
            context.Categories.AddOrUpdate(c => c.Id,
                new budgeter.Models.CodeFirst.Category() { Id = 1, Name = "Rent/Utilities" },
                new budgeter.Models.CodeFirst.Category() { Id = 2, Name = "Food" },
                new budgeter.Models.CodeFirst.Category() { Id = 3, Name = "Entertainment" },
                new budgeter.Models.CodeFirst.Category() { Id = 4, Name = "Travel" },
                new budgeter.Models.CodeFirst.Category() { Id = 5, Name = "Fuel" },
                new budgeter.Models.CodeFirst.Category() { Id = 6, Name = "Miscellaneous" },
                new budgeter.Models.CodeFirst.Category() { Id = 7, Name = "Deposit" }

            );



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
