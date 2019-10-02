using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Reti.Lab.FoodOnKontainers.Middleware;
using Reti.Lab.FoodOnKontainers.Middleware.Dto;
using Reti.Lab.FoodOnKontainers.Middleware.Extensions;
using Reti.Lab.FoodOnKontainers.Payments.Api.BackgroundService;
using Reti.Lab.FoodOnKontainers.Payments.Api.Dal;
using Reti.Lab.FoodOnKontainers.Payments.Api.Dto;
using Reti.Lab.FoodOnKontainers.Payments.Api.Payment;

namespace Reti.Lab.FoodOnKontainers.Payments.Api
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
            // configure DI for application services
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddSingleton<ILogService, LogService>();
            services.AddHostedService<ReceiptService>();

            services.AddDbContext<PaymentDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("FokPaymentDB"));
            });

            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.DateTimeItFormatter();

            var rabbitMQConfigurations = new RabbitMQConfigurations();
            new ConfigureFromConfigurationOptions<RabbitMQConfigurations>(
                Configuration.GetSection("RabbitMQConfigurations"))
                    .Configure(rabbitMQConfigurations);

            services.AddSingleton(rabbitMQConfigurations);

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            Mapper.Initialize(cfg => cfg.AddProfile<MappingProfile>());
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
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
