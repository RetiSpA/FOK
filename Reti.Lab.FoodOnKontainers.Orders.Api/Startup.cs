using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reti.Lab.FoodOnKontainers.Middleware;
using Reti.Lab.FoodOnKontainers.Middleware.Dto;
using Reti.Lab.FoodOnKontainers.Middleware.Extensions;
using Reti.Lab.FoodOnKontainers.Orders.Api.DAL;
using Reti.Lab.FoodOnKontainers.Orders.Api.Events;
using Reti.Lab.FoodOnKontainers.Orders.Api.Services;

namespace Reti.Lab.FoodOnKontainers.Orders.Api
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
            services.AddDbContext<OrdersDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("FokOrdersDB"), x => x.UseNetTopologySuite());
            });

            services.AddScoped<IOrdersService, OrdersService>();
            services.AddSingleton<ILogService, LogService>();
            services.AddSingleton<IOrdersEventManager, OrdersEventManager>();

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

            if(app.ApplicationServices.GetRequiredService<RabbitMQConfigurations>().Enabled)
            {
                app.UseMiddleware<LogMiddleware>();
                app.ApplicationServices.GetService<IOrdersEventManager>();
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
