using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reti.Lab.FoodOnKontainers.Basket.Api.Basket;
using Reti.Lab.FoodOnKontainers.Basket.Api.Basket.Repository;
using Reti.Lab.FoodOnKontainers.Basket.Api.Events;
using Reti.Lab.FoodOnKontainers.Basket.Api.Settings;
using Reti.Lab.FoodOnKontainers.Middleware;
using Reti.Lab.FoodOnKontainers.Middleware.Dto;
using Reti.Lab.FoodOnKontainers.Middleware.Extensions;
using StackExchange.Redis;

namespace Reti.Lab.FoodOnKontainers.Basket.Api
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
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.DateTimeItFormatter();
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = Configuration.GetSection("RedisConfig").GetValue<string>("connection");               
            });
                        
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IBasketRepository, RedisBasketRepository>();            
            services.Configure<RedisConfig>(Configuration.GetSection("RedisConfig"));            
            services.AddSingleton<ConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(Configuration.GetSection("RedisConfig").GetValue<string>("connection"), true);
                configuration.ResolveDns = true;
                return ConnectionMultiplexer.Connect(configuration);
            });
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BasketEventsManager>();

            LogMiddleware.AddRabbitMQConfiguration(services, Configuration);
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
        }
    }
}
