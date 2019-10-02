using Reti.Lab.FoodOnKontainers.Restaurants.Api.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Extensions;
using Reti.Lab.FoodOnKontainers.Restaurants.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.Services
{

    public interface IRestaurantService
    {
        Task<List<Models.Restaurants>> GetRestaurants();

        Task<Models.Restaurants> GetRestaurantDetail(int idRestaurant);

        Task AddRestaurant(Models.Restaurants restaurant);

        Task UpdateRestaurant(Models.Restaurants restaurant);

        Task DisableRestaurant(int idRestaurant);

        Task UpdateRestaurantAverageRating(int idRestaurant, decimal newRating);
       
    }


    public class RestaurantService : IRestaurantService
    {
        private RestaurantsDbContext restaurantsDbContext;
        public RestaurantService(RestaurantsDbContext restaurantsDbContext)
        {
            this.restaurantsDbContext = restaurantsDbContext;
        }

        public async Task AddRestaurant(Models.Restaurants restaurant)
        {

            this.restaurantsDbContext.Restaurants.Add(restaurant);
            await this.restaurantsDbContext.SaveChangesAsync();

        }

        public async Task DisableRestaurant(int idRestaurant)
        {
            var restaurantToDisable = restaurantsDbContext.Restaurants.Single(r => r.Id == idRestaurant);
            restaurantToDisable.Enabled = false;
            await restaurantsDbContext.SaveChangesAsync();
        }

        public async Task<Models.Restaurants> GetRestaurantDetail(int idRestaurant)
        {
            var restaurantDetail = await restaurantsDbContext.Restaurants
                 .Include(rm => rm.IdRestaurantTypeNavigation)
                 .Include(rm => rm.RestaurantsMenu)
                 .ThenInclude(rm => rm.IdDishTypeNavigation)
                 .SingleAsync(rs => rs.Id == idRestaurant);

            return restaurantDetail;
        }

        public async Task<List<Models.Restaurants>> GetRestaurants()
        {
            return await this.restaurantsDbContext.Restaurants
                .Include(rm => rm.IdRestaurantTypeNavigation)
                .ToListAsync();
        }

        public async Task UpdateRestaurant(Models.Restaurants restaurant)
        {
            var updatedRestaurant = this.restaurantsDbContext.Restaurants.Attach(restaurant);
            updatedRestaurant.State = EntityState.Modified;

            await this.restaurantsDbContext.SaveChangesAsync();
        }

        public async Task UpdateRestaurantAverageRating(int idRestaurant, decimal newAverageRating)
        {
            var Restaurant = this.restaurantsDbContext.Restaurants.Single(rs => rs.Id == idRestaurant);
            Restaurant.AverageRating = newAverageRating;
             
            await this.restaurantsDbContext.SaveChangesAsync();
        }
    }
}
