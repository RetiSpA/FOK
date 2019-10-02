using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reti.Lab.FoodOnKontainers.Middleware;
using Reti.Lab.FoodOnKontainers.Middleware.Dto;
using Reti.Lab.FoodOnKontainers.Middleware.Extensions;
using Reti.Lab.FoodOnKontainers.Reviews.Api.Events;
using Reti.Lab.FoodOnKontainers.Reviews.Api.Models;
using Reti.Lab.FoodOnKontainers.Reviews.Api.Services;

namespace Reti.Lab.FoodOnKontainers.Reviews.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ReviewsDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("FokReviewsDB"));
            });

            //registering feedbackservice service
            services.AddScoped<IReviewService, ReviewService>();
            services.AddSingleton<ILogService, LogService>();
            //registering bus event manager
            services.AddSingleton<IReviewEventsManager, ReviewEventsManager>();

            LogMiddleware.AddRabbitMQConfiguration(services, Configuration);

            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.DateTimeItFormatter();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (app.ApplicationServices.GetRequiredService<RabbitMQConfigurations>().Enabled)
            {
                app.UseMiddleware<LogMiddleware>();
               
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowCredentials();
            });
            app.UseMvc();

            app.ApplicationServices.GetService<IReviewEventsManager>();
        }
    }
}
