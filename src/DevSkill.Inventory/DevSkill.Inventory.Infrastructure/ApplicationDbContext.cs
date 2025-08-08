using System.Reflection.Emit;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
        ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole,
        ApplicationUserLogin, ApplicationRoleClaim,
        ApplicationUserToken>
    {

        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _migrationAssembly = migrationAssembly;
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString, (x) => x.MigrationsAssembly(_migrationAssembly));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Product>()
                .HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Product>()
                .HasOne(p => p.Unit)
                .WithMany()
                .HasForeignKey(p => p.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany()
                .HasForeignKey(s => s.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Sale>()
                .HasOne(s => s.SaleType)
                .WithMany()
                .HasForeignKey(s => s.SalesTypeId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<SaleItem>()
                .HasOne(si => si.Sale)
                .WithMany(s => s.Items)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PaymentItem>()
                .HasOne(si => si.Sale)
                .WithMany(s => s.PaymentItems)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SaleItem>()
                .HasOne(si => si.Product)
                .WithMany()
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);



            builder.Entity<Customer>()
                .HasMany(c => c.Sales)
                .WithOne(s => s.Customer)
                .HasForeignKey(s => s.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CustomerLedger>()
              .HasOne(x => x.Customer)
              .WithMany() 
              .HasForeignKey(x => x.CustomerId)
              .OnDelete(DeleteBehavior.Cascade);


           builder.Entity<ApplicationUser>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);


            //builder.Entity<SalesReturn>()
            //    .HasOne(r => r.Customer)
            //    .WithMany()
            //    .HasForeignKey(r => r.CustomerId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<SalesReturnItem>()
            //    .HasOne(i => i.Product)
            //    .WithMany()
            //    .HasForeignKey(i => i.ProductId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<SalesReturnItem>()
            //    .HasOne(i => i.SalesReturn)
            //    .WithMany(r => r.Items)
            //    .HasForeignKey(i => i.SalesReturnId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<MoneyReceipt>()
            //    .HasOne(r => r.Customer)
            //    .WithMany()
            //    .HasForeignKey(r => r.CustomerId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<MoneyReceiptDetail>()
            //    .HasOne(d => d.MoneyReceipt)
            //    .WithMany(r => r.Details)
            //    .HasForeignKey(d => d.MoneyReceiptId);

            //builder.Entity<DebitVoucherDetail>()
            //    .HasOne(d => d.DebitVoucher)
            //    .WithMany(r => r.Details)
            //    .HasForeignKey(d => d.DebitVoucherId);

            builder.Entity<SaleType>().HasData(
                new SaleType
                {
                    Id = Guid.Parse("485A4002-097F-4864-A193-9017DC5230C5"),
                    PriceName = "MRP"
                },
                new SaleType
                {
                    Id = Guid.Parse("FF7CC7D7-2694-4512-A425-3E9CCE91CAA5"),
                    PriceName = "Wholesale"
                }
            );

            builder.Entity<AccountType>().HasData(
              new AccountType
              {
                  Id = Guid.Parse("4123C6EB-3D4B-4E5E-8FE3-CC6A8859320E"),
                  Name = "Bank",
                  IsActive = true
              },
              new AccountType
              {
                  Id = Guid.Parse("2FEDFFD2-F8B5-41F8-BD12-E593EC86E89E"),
                  Name = "Cash",
                  IsActive = true
              },
              new AccountType
              {
                  Id = Guid.Parse("4973B853-D6E2-42B3-ACBF-8195D611B470"),
                  Name = "Mobile",
                  IsActive = true
              }
           );
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ImageResizeQueue> ImageResizeQueues { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<SaleType> SaleTypes { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<CashAccount> CashAccounts { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<PaymentItem> PaymentItems { get; set; }
        public DbSet<BalanceTransfer> BalanceTransfers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<MobileAccount> MobileAccounts { get; set; }
        public DbSet<CustomerLedger> CustomerLedgers { get; set; }
        //public DbSet<SalesReturn> SalesReturns { get; set; }
        //public DbSet<SalesReturnItem> SalesReturnsItem { get; set; }

        public DbSet<CompanyProfile> CompanyProfiles { get; set; }
        //public DbSet<DebitVoucher> DebitVouchers { get; set; }
        //public DbSet<MoneyReceipt> MoneyReceipts { get; set; }

    }
}
