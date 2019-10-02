using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reti.Lab.FoodOnKontainers.Middleware.Extensions;

namespace Reti.Lab.FoodOnKontainers.Gateway.HttpAggregator
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.DateTimeItFormatter();

            var urls = Configuration.GetSection("urls");
            services.Configure<UrlsConfig>(urls);

            services.AddHttpClient<IBasketService, BasketService>();
            services.AddHttpClient<IOrderService, OrderService>();
            services.AddHttpClient<IPaymentService, PaymentService>();
            services.AddHttpClient<IUserService, UserService>();
            services.AddHttpClient<IRestaurantService, RestaurantService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
