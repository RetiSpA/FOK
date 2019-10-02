using Microsoft.EntityFrameworkCore;
using Reti.Lab.FoodOnKontainers.Events.DTO.Restaurants;
using Reti.Lab.FoodOnKontainers.Restaurants.Api.DAL;
using Reti.Lab.FoodOnKontainers.Restaurants.Api.Events;
using Reti.Lab.FoodOnKontainers.Restaurants.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Restaurants.Api.Services
{
    public interface IRestaurantMenuService
    {
        Task<List<RestaurantsMenu>> GetRestaurantMenu(int idRestaurant);
        Task AddMenuItem(RestaurantsMenu newMenuItem);

        Task UpdateMenuItem(RestaurantsMenu menuItem);

        Task DeleteMenuItem(int menuItemId);

    }

    public class RestaurantMenuService : IRestaurantMenuService
    {
        private RestaurantsDbContext restaurantsDbContext;
        private IRestaurantsEventsManager restaurantEventManager;

        public RestaurantMenuService(RestaurantsDbContext restaurantsDbContext, IRestaurantsEventsManager restaurantEventManager)
        {
            this.restaurantsDbContext = restaurantsDbContext;
            this.restaurantEventManager = restaurantEventManager;
        }

        public async Task AddMenuItem(RestaurantsMenu newMenuItem)
        {
            restaurantsDbContext.RestaurantsMenu.Add(newMenuItem);
            await restaurantsDbContext.SaveChangesAsync();
        }

        public async Task DeleteMenuItem(int menuItemId)
        {
            var menuItemToDel = restaurantsDbContext.RestaurantsMenu.Single(rm => rm.Id == menuItemId);

            restaurantsDbContext.RestaurantsMenu.Remove(menuItemToDel);
            await restaurantsDbContext.SaveChangesAsync();


            var productAvailabilityChanged = new ProductAvailabilityChangedEvent
            {
                 itemId = menuItemId,
                 available = false,
                 restaurantId = menuItemToDel.IdRestaurant
            };
            restaurantEventManager.ProductAvailabilityChanged(productAvailabilityChanged);
        }

        public async Task<List<RestaurantsMenu>> GetRestaurantMenu(int idRestaurant)
        {

            List<RestaurantsMenu> restaurantMenu = await restaurantsDbContext.RestaurantsMenu.Where(rm => rm.IdRestaurant == idRestaurant).ToListAsync();
            return restaurantMenu;
        }

        public async Task UpdateMenuItem(RestaurantsMenu menuItem)
        {
            var currentMenuItem = restaurantsDbContext.RestaurantsMenu.Single(mi => mi.Id == menuItem.Id);
            decimal? currentMenuItemPrice = currentMenuItem.Price;

            currentMenuItem.Description = menuItem.Description;
            currentMenuItem.IdDishType = menuItem.IdDishType;
            currentMenuItem.IdRestaurant = menuItem.IdRestaurant;
            currentMenuItem.Name = menuItem.Name;
            currentMenuItem.Price = menuItem.Price;
            currentMenuItem.Promo = menuItem.Promo;

            await restaurantsDbContext.SaveChangesAsync();

            if (menuItem.Price.Value != currentMenuItemPrice.Value)
            {
                var priceChangedEvent = new PriceChangedEvent
                {
                    itemId = menuItem.Id,
                    restaurantId = menuItem.IdRestaurant,
                    newPrice = menuItem.Price.Value
                    
                };

                restaurantEventManager.ProductPriceChanged(priceChangedEvent);
            }
        }
    }
}
