using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Reti.Lab.FoodOnKontainers.Events.DTO.Delivery;
using Reti.Lab.FoodOnKontainers.NotificationHub.PushNotification;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Reti.Lab.FoodOnKontainers.NotificationHub.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private NotificationHubClient hub;


        public NotificationController(Notifications notifications)
        {
            hub = notifications.Hub;

        }


        [HttpPost("send")]
        public async Task<IActionResult> send([FromBody] DeliveryReadyEvent deliverReady)
        {

            string[] userTag = new string[1];
            userTag[0] = "rider:" + deliverReady.ryderId;


            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;

            switch (deliverReady.pns.ToLower())
            {
                case "apns":
                    // iOS
                    var alert = "{\"aps\":{\"alert\":\"New Delivery is ready\"}}";
                    outcome = await hub.SendAppleNativeNotificationAsync(alert, userTag);
                    break;
                case "fcm":
                    // Android
                    var notif = "{ \"data\" : {\"message\":\"New Delivery is ready\",\"deliveryId\":\"" + deliverReady.deliveryId + "\"}}";
                    outcome = await hub.SendFcmNativeNotificationAsync(notif, userTag);
                    break;
            }

            if (outcome != null)
            {
                if (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown)))
                {
                    return BadRequest();
                }
            }

            return Ok();
        }
    }
}
