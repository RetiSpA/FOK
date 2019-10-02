using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Options;
using Reti.Lab.FoodOnKontainers.NotificationHub.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reti.Lab.FoodOnKontainers.NotificationHub.PushNotification
{
    public class Notifications
    {
        public NotificationHubClient Hub { get; set; }
        private NotificationHubConfig notificationHubConfig;

        public Notifications(IOptions<NotificationHubConfig> notificationHubConfig)
        {
            this.notificationHubConfig = notificationHubConfig.Value;
            Hub = NotificationHubClient.CreateClientFromConnectionString(this.notificationHubConfig.SubscriptionId,this.notificationHubConfig.Name);
        }

        public Notifications(string connectionString, string name)
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, name);
        }
    }
}
