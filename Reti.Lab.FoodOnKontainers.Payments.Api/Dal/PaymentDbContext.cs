using Microsoft.EntityFrameworkCore;
using Reti.Lab.FoodOnKontainers.Payments.Api.Models;

namespace Reti.Lab.FoodOnKontainers.Payments.Api.Dal
{
    public partial class PaymentDbContext : DbContext
    {
        public PaymentDbContext()
        {
        }

        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PaySystem> PaySystem { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<Receipt> Receipt { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=FOK_Payment;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<PaySystem>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");
            });
        }
    }
}
