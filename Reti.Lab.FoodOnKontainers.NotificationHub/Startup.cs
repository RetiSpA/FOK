using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reti.Lab.FoodOnKontainers.Middleware;
using Reti.Lab.FoodOnKontainers.Middleware.Extensions;
using Reti.Lab.FoodOnKontainers.NotificationHub.Events;
using Reti.Lab.FoodOnKontainers.NotificationHub.PushNotification;
using Reti.Lab.FoodOnKontainers.NotificationHub.Settings;

namespace Reti.Lab.FoodOnKontainers.NotificationHub
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

            var notificationConfigSection = Configuration.GetSection("NotificationHubConfig");

            var notifications = new Notifications(notificationConfigSection["SubscriptionId"], notificationConfigSection["Name"]);

            services.AddSingleton<Notifications>(notifications);
            services.AddSingleton<INotificationEventManager, NotificationEventsManager>();
            services.Configure<NotificationHubConfig>(Configuration.GetSection("NotificationHubConfig"));

            

            LogMiddleware.AddRabbitMQConfiguration(services, Configuration);

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

            app.UseMvc();
        }
    }
}
