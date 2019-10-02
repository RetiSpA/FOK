using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Reti.Lab.FoodOnKontainers.Reviews.Api.Models
{
    public partial class ReviewsDBContext : DbContext
    {
        public ReviewsDBContext()
        {
        }

        public ReviewsDBContext(DbContextOptions<ReviewsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RestaurantReview> RestaurantsReviews { get; set; }
        public virtual DbSet<RiderReview> RidersReviews { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<RestaurantReview>(entity =>
            {
                entity.HasKey(e => e.IdOrder);

                entity.ToTable("Restaurants_Reviews");

                entity.Property(e => e.IdOrder)
                    .HasColumnName("id_order")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdRestaurant).HasColumnName("id_restaurant");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.RestaurantName)
                    .IsRequired()
                    .HasColumnName("restaurant_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Review)
                    .IsRequired()
                    .HasColumnName("review");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RiderReview>(entity =>
            {
                entity.HasKey(e => e.IdOrder)
                    .HasName("PK_Ryder_Reviews");

                entity.ToTable("Ryders_Reviews");

                entity.Property(e => e.IdOrder)
                    .HasColumnName("id_order")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdRyder).HasColumnName("id_ryder");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.Review)
                    .IsRequired()
                    .HasColumnName("review");

                entity.Property(e => e.RyderName)
                    .IsRequired()
                    .HasColumnName("ryder_name")
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasMaxLength(50);
            });
        }
    }
}
