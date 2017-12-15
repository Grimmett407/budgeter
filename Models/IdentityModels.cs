using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using budgeter.Models.CodeFirst;

namespace budgeter.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }

        public ApplicationUser()
        {
            this.BankAccounts = new HashSet<BankAccount>();
            this.Households = new HashSet<Household>();
            this.Transactions = new HashSet<Transaction>();
            this.Budgets = new HashSet<Budget>();
            this.Invites = new HashSet<Invite>();
        }

        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Household> Households { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Invite> Invites { get; set; }
    }
}