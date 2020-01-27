namespace DTA.Web.Migrations
{
    using DTA.Web.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security.Claims;

    internal sealed class Configuration : DbMigrationsConfiguration<DTA.Web.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DTA.Web.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            // role (Const.getRoles() return string[] whit all roles)

            //Step 1 Create the user.
            var passwordHasher = new PasswordHasher();
            var user = new ApplicationUser()
            {
                UserName = "admin@admin.net",
                Email = "admin@admin.net",
                SecurityStamp = "b427bc63-bb63-4daf-803a-eeb4debc18ba", 
                PasswordHash = passwordHasher.HashPassword("Admin1234")
                
            }; 

            //Step 2 Create and add the new Role.
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleToChoose = context.Roles.Where(x => x.Name == "Admin").FirstOrDefault();
            if (roleToChoose == null)
            {
                roleToChoose = new IdentityRole("Admin");
                context.Roles.Add(roleToChoose);
            }
            //Step 3 Create a role for a user
            var role = new IdentityUserRole();
            role.RoleId = roleToChoose.Id;
            role.UserId = user.Id;

            //Step 4 Add the role row and add the user to DB)
            user.Roles.Add(role);
            context.Users.Add(user);
        }
    }
}
