using FinanceManagement.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.DAL.Data
{
    public class FinanceDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<FinancialTransaction> Transactions { get; set; }
        public DbSet<PlaidTransaction> PlaidTransactions { get; set; }

        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Budgets)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Expenses)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Transactions)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Budget>()
                .HasMany(b => b.Categories)
                .WithOne(c => c.Budget)
                .HasForeignKey(c => c.BudgetId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Expenses)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId);
        }
    }
}
