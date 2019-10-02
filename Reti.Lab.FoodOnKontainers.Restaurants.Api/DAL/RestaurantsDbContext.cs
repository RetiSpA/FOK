using Microsoft.EntityFrameworkCore;
using Reti.Lab.FoodOnKontainers.Restaurants.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.DAL
{
    public class RestaurantsDbContext : DbContext
    {
        public RestaurantsDbContext()
        {
        }

        public RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options)
            : base(options)
        {
           
        }

        public DbSet<DishTypes> DishTypes { get; set; }
        public DbSet<MenuPhoto> MenuPhoto { get; set; }
        public DbSet<RestaurantTypes> RestaurantTypes { get; set; }
        public DbSet<Models.Restaurants> Restaurants { get; set; }
        public DbSet<RestaurantsMenu> RestaurantsMenu { get; set; }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<DishTypes>(entity =>
            {
                entity.ToTable("Dish_Types");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MenuPhoto>(entity =>
            {
                entity.HasKey(e => e.IdMenu);

                entity.ToTable("Menu_Photo");

                entity.Property(e => e.IdMenu)
                    .HasColumnName("Id_Menu")
                    .ValueGeneratedNever();

                entity.Property(e => e.Photo).IsRequired();

                entity.HasOne(d => d.IdMenuNavigation)
                    .WithOne(p => p.MenuPhoto)
                    .HasForeignKey<MenuPhoto>(d => d.IdMenu)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Menu_Photo_Restaurants_Menu");
            });

            modelBuilder.Entity<RestaurantTypes>(entity =>
            {
                entity.ToTable("Restaurant_Types");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Models.Restaurants>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasColumnName("Position");

                entity.Property(e => e.AverageRating)
                    .HasColumnName("Average_Rating")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Enabled)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IdRestaurantType).HasColumnName("Id_Restaurant_Type");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdRestaurantTypeNavigation)
                    .WithMany(p => p.Restaurants)
                    .HasForeignKey(d => d.IdRestaurantType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurants_Restaurant_Types");
            });

            modelBuilder.Entity<RestaurantsMenu>(entity =>
            {
                entity.ToTable("Restaurants_Menu");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.IdDishType).HasColumnName("Id_Dish_Type");

                entity.Property(e => e.IdRestaurant).HasColumnName("Id_Restaurant");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Promo).HasColumnType("decimal(3, 2)");

                entity.HasOne(d => d.IdDishTypeNavigation)
                    .WithMany(p => p.RestaurantsMenu)
                    .HasForeignKey(d => d.IdDishType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurants_Menu_Dish_Types");

                entity.HasOne(d => d.IdRestaurantNavigation)
                    .WithMany(p => p.RestaurantsMenu)
                    .HasForeignKey(d => d.IdRestaurant)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Restaurants_Menu_Restaurants");
            });
        }
    }
}
