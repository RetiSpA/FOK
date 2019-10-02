using Microsoft.EntityFrameworkCore;
using Reti.Lab.FoodOnKontainers.Reviews.Api.DTO;
using Reti.Lab.FoodOnKontainers.Reviews.Api.Events;
using Reti.Lab.FoodOnKontainers.Reviews.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.Reviews.Api.Services
{

    public interface IReviewService
    {
        Task<List<RestaurantReviewDto>> GetRestaurantReviews(int idRestaurant);

        Task<List<RiderReviewDto>> GetRiderReviews(int idRyder);

        Task<RestaurantReviewDto> GetRestaurantReviewByOrder(int idOrder);

        Task<RiderReviewDto> GetRiderReviewByOrder(int idRyder);

        Task<RestaurantReviewDto> GetRestaurantReviewByUser(int idUser, int IdRestaurant);

        Task<List<RestaurantReviewDto>> GetRestaurantsReviewByUser(int idUser);

        Task<List<RiderReviewDto>> GetRidersReviewByUser(int idUser);

        Task<RiderReviewDto> GetRiderReviewByUser(int idUser, int idRider);

        Task AddRiderReview(RiderReviewDto ryderReview);

        Task AddRestaurantReview(RestaurantReviewDto restaurantReview);


    }
    public class ReviewService : IReviewService
    {

        private ReviewsDBContext reviewsDBContext;
        private IReviewEventsManager reviewEventsManager;

        public ReviewService(ReviewsDBContext reviewsDBContext, IReviewEventsManager reviewEventsManager)
        {
            this.reviewsDBContext = reviewsDBContext;
            this.reviewEventsManager = reviewEventsManager;
        }

        public async Task AddRestaurantReview(RestaurantReviewDto restaurantReviewDto)
        {
            var restaurantReview = new RestaurantReview
            {
                IdOrder = restaurantReviewDto.IdOrder,
                IdRestaurant = restaurantReviewDto.IdRestaurant,
                IdUser = restaurantReviewDto.IdUser,
                Rating = restaurantReviewDto.Rating,
                Review = restaurantReviewDto.Review,
                RestaurantName = restaurantReviewDto.RestaurantName,
                UserName = restaurantReviewDto.UserName,
            };

            await reviewsDBContext.RestaurantsReviews.AddAsync(restaurantReview);
            await reviewsDBContext.SaveChangesAsync();
            await reviewEventsManager.RestaurantRatingChanged(restaurantReviewDto.IdRestaurant, restaurantReviewDto.Rating);

        }

        public async Task AddRiderReview(RiderReviewDto riderReviewDto)
        {
            var ryderReview = new RiderReview
            {
                IdOrder = riderReviewDto.IdOrder,
                UserName = riderReviewDto.UserName,
                Review = riderReviewDto.Review,
                Rating = riderReviewDto.Rating,
                IdUser = riderReviewDto.IdUser,
                IdRyder = riderReviewDto.IdRider,
                RyderName = riderReviewDto.RiderName,
            };

            await reviewsDBContext.RidersReviews.AddAsync(ryderReview);
            await reviewsDBContext.SaveChangesAsync();
            await reviewEventsManager.RiderRatingChanged(riderReviewDto.IdRider, riderReviewDto.Rating);
           
        }

        public async Task<RestaurantReviewDto> GetRestaurantReviewByOrder(int idOrder)
        {
            var restaurantReviewsByOrder = await reviewsDBContext.RestaurantsReviews.SingleAsync(rst => rst.IdOrder == idOrder);

            return ToRestaurantReviewDtO(restaurantReviewsByOrder);
        }

        public async Task<List<RestaurantReviewDto>> GetRestaurantsReviewByUser(int idUser)
        {
            var restaurantReviews = await reviewsDBContext.RestaurantsReviews.Where(rw => rw.IdUser == idUser)
                .Select(rw => ToRestaurantReviewDtO(rw))
                .ToListAsync();

            return restaurantReviews;
        }

        public async Task<RestaurantReviewDto> GetRestaurantReviewByUser(int idUser, int IdRestaurant)
        {
            var restaurantReview = await reviewsDBContext.RestaurantsReviews.FirstOrDefaultAsync(rw => rw.IdUser == idUser && rw.IdRestaurant == IdRestaurant);
            return ToRestaurantReviewDtO(restaurantReview);
        }



        public async Task<List<RestaurantReviewDto>> GetRestaurantReviews(int idRestaurant)
        {
            var restaurantReviews = await reviewsDBContext.RestaurantsReviews.Where(rw => rw.IdRestaurant == idRestaurant)
                 .Select(rw => ToRestaurantReviewDtO(rw))
                 .ToListAsync();
            return restaurantReviews;
        }



        public async Task<RiderReviewDto> GetRiderReviewByOrder(int idOrder)
        {
            var riderReview = await this.reviewsDBContext.RidersReviews.FirstOrDefaultAsync(rw => rw.IdOrder == idOrder);
            return ToRiderReviewDto(riderReview);
        }

        public async Task<List<RiderReviewDto>> GetRidersReviewByUser(int idUser)
        {
            var riderReviews = await reviewsDBContext.RidersReviews.Where(rw => rw.IdUser == idUser)
                              .Select(rw => ToRiderReviewDto(rw))
                              .ToListAsync();
            return riderReviews;
        }

        public async Task<RiderReviewDto> GetRiderReviewByUser(int idUser, int idRider)
        {
            var riderReview = await reviewsDBContext.RidersReviews.FirstOrDefaultAsync(rw => rw.IdUser == idUser && rw.IdRyder == idRider);
            return ToRiderReviewDto(riderReview);
        }

        public async Task<List<RiderReviewDto>> GetRiderReviews(int idRyder)
        {

            var riderReviews = await reviewsDBContext.RidersReviews.Where(rw => rw.IdRyder == idRyder)
                               .Select(rw => ToRiderReviewDto(rw))
                               .ToListAsync();
            return riderReviews;
        }

        private RestaurantReviewDto ToRestaurantReviewDtO(RestaurantReview rw)
        {
            return new RestaurantReviewDto
            {
                IdUser = rw.IdUser,
                UserName = rw.UserName,
                IdOrder = rw.IdOrder,
                IdRestaurant = rw.IdRestaurant,
                Rating = rw.Rating,
                RestaurantName = rw.RestaurantName,
                Review = rw.Review
            };
        }

        private RiderReviewDto ToRiderReviewDto(RiderReview rw)
        {
            return new RiderReviewDto
            {
                IdOrder = rw.IdOrder,
                IdRider = rw.IdRyder,
                IdUser = rw.IdUser,
                Rating = rw.Rating,
                Review = rw.Review,
                RiderName = rw.RyderName,
                UserName = rw.UserName
            };
        }

      
    }
}
