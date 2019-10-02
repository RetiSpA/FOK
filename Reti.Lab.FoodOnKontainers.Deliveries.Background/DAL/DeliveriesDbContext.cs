using Microsoft.EntityFrameworkCore;
using Reti.Lab.FoodOnKontainers.Deliveries.Background.Models;

namespace Reti.Lab.FoodOnKontainers.Deliveries.Background.DAL
{
    public partial class DeliveriesDbContext : DbContext
    {
        public DeliveriesDbContext()
        {
        }

        public DeliveriesDbContext(DbContextOptions<DeliveriesDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Delivery> Delivery { get; set; }
        public virtual DbSet<Rider> Rider { get; set; }
        public virtual DbSet<Status> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.Property(e => e.DeliveryAddress)
                    .IsRequired()
                    .HasColumnName("Delivery_Address")
                    .HasMaxLength(100);

                entity.Property(e => e.DeliveryPosition)
                    .IsRequired()
                    .HasColumnName("Delivery_Position");

                entity.Property(e => e.DeliveryDate).HasColumnName("Delivery_Date");

                entity.Property(e => e.DeliveryName)
                    .IsRequired()
                    .HasColumnName("Delivery_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.IdOrder).HasColumnName("Id_Order");

                entity.Property(e => e.IdRestaurant).HasColumnName("Id_Restaurant");

                entity.Property(e => e.IdRider).HasColumnName("Id_Rider");

                entity.Property(e => e.IdStatus).HasColumnName("Id_Status");

                entity.Property(e => e.PickUpAddress)
                    .IsRequired()
                    .HasColumnName("PickUp_Address")
                    .HasMaxLength(100);

                entity.Property(e => e.PickUpPosition)
                    .IsRequired()
                    .HasColumnName("PickUp_Position");

                entity.Property(e => e.PickUpDate).HasColumnName("PickUp_Date");

                entity.Property(e => e.RestaurantName)
                    .IsRequired()
                    .HasColumnName("Restaurant_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.TakeChargeDate).HasColumnName("Take_Charge_Date");

                entity.Property(e => e.DeliveryRequestedDate).HasColumnName("Delivery_Requested_Date");

                entity.HasOne(d => d.IdRiderNavigation)
                    .WithMany(p => p.Delivery)
                    .HasForeignKey(d => d.IdRider)
                    .HasConstraintName("FK_Delivery_Rider");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.Delivery)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Delivery_Status");
            });

            modelBuilder.Entity<Rider>(entity =>
            {
                entity.HasKey(e => e.IdRider);

                entity.Property(e => e.IdRider).HasColumnName("Id_Rider");

                entity.Property(e => e.AverageRating)
                    .HasColumnName("Average_Rating")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.RiderName)
                    .IsRequired()
                    .HasColumnName("Rider_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.StartingPointAddress)
                    .HasColumnName("Starting_Point_Address")
                    .HasMaxLength(100);

                entity.Property(e => e.StartingPoint).HasColumnName("Starting_Point");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
