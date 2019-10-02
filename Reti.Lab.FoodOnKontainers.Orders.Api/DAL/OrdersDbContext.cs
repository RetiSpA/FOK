using Microsoft.EntityFrameworkCore;
using Reti.Lab.FoodOnKontainers.Orders.Api.Models;

namespace Reti.Lab.FoodOnKontainers.Orders.Api.DAL
{
    public partial class OrdersDbContext : DbContext
    {
        public OrdersDbContext()
        {
        }

        public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Status> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.CreateDate).HasColumnName("Create_Date");

                entity.Property(e => e.DeliveryAddress)
                    .IsRequired()
                    .HasColumnName("Delivery_Address")
                    .HasMaxLength(100);

                entity.Property(e => e.DeliveryPosition)
                    .IsRequired()
                    .HasColumnName("Delivery_Position");

                entity.Property(e => e.IdRestaurant).HasColumnName("Id_Restaurant");

                entity.Property(e => e.IdStatus).HasColumnName("Id_Status");

                entity.Property(e => e.IdUser).HasColumnName("Id_User");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RestaurantAddress)
                    .HasColumnName("Restaurant_Address")
                    .HasMaxLength(100);

                entity.Property(e => e.RestaurantPosition)
                    .HasColumnName("Restaurant_Position");

                entity.Property(e => e.RestaurantName)
                    .HasColumnName("Restaurant_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .HasColumnName("User_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.DeliveryRequestedDate).HasColumnName("Delivery_Requested_Date");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Status");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => new { e.IdOrder, e.IdMenuItem });

                entity.ToTable("Order_Item");

                entity.Property(e => e.IdOrder).HasColumnName("Id_Order");

                entity.Property(e => e.IdMenuItem).HasColumnName("Id_Menu_Item");

                entity.Property(e => e.MenuItemName)
                    .IsRequired()
                    .HasColumnName("Menu_Item_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdOrderNavigation)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.IdOrder)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Item_Order");
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
