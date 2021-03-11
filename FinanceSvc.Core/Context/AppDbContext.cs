using FinanceSvc.Core.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.Context;
using Shared.Entities;
using System.Reflection;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Context
{
    public class AppDbContext : ApplicationDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.UseOpenIddict();//Comment in other services other than Auth
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Ignore(typeof(User));
            modelBuilder.Ignore(typeof(Role));
            modelBuilder.Ignore(typeof(UserClaim));
            modelBuilder.Ignore(typeof(UserRole));
            modelBuilder.Ignore(typeof(UserLogin));
            modelBuilder.Ignore(typeof(RoleClaim));
            modelBuilder.Ignore(typeof(UserToken));
        }

        public DbSet<TestModel> TestModels { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<SchoolClass> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<FeeGroup> FeeGroups { get; set; }
        public DbSet<FeeComponent> FeeComponents { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<AccountClass> AccountClasses { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoicePayment> InvoicePayments { get; set; }
        public DbSet<SessionSetup> SessionSetups { get; set; }


        public async Task AddSampleData()
        {
            TestModels.Add(new TestModel { TenantId = 1, Name = "Frank" });
            TestModels.Add(new TestModel { TenantId = 1, Name = "John" });
            TestModels.Add(new TestModel { TenantId = 2, Name = "Sani" });
            TestModels.Add(new TestModel { TenantId = 2, Name = "Chisom" });

            await SaveChangesAsync();
        }
    }
}
