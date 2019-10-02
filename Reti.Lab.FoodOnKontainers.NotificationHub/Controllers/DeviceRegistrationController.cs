using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using Microsoft.Extensions.Options;
using Reti.Lab.FoodOnKontainers.NotificationHub.DTO;
using Reti.Lab.FoodOnKontainers.NotificationHub.PushNotification;
using Reti.Lab.FoodOnKontainers.NotificationHub.Settings;

namespace Reti.Lab.FoodOnKontainers.NotificationHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceRegistrationController : ControllerBase
    {

        private NotificationHubClient hub;
      
        public DeviceRegistrationController(Notifications notifications)
        {
                
            hub = notifications.Hub;
        }


        [HttpPost("NewDevice/{handle}")]
        public async Task<IActionResult> RegisterDevice(string handle)
        {
            string newRegistrationId = null;

            if (handle != null)
            {
                var registrations = await hub.GetRegistrationsByChannelAsync(handle, 100);

                foreach (RegistrationDescription registration in registrations)
                {
                    if (newRegistrationId == null)
                    {
                        newRegistrationId = registration.RegistrationId;
                    }
                    else
                    {
                        await hub.DeleteRegistrationAsync(registration);
                    }
                }
            }

            if (newRegistrationId == null)
                newRegistrationId = await hub.CreateRegistrationIdAsync();

            return Ok(newRegistrationId);
        }


        [HttpPut("NewUpdateDevice/{id}")]
        public async Task<IActionResult> RegisterDevice(string id, DeviceRegistration deviceUpdate)
        {
            RegistrationDescription registration = null;
            switch (deviceUpdate.Platform)
            {
                case "mpns":
                    registration = new MpnsRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "wns":
                    registration = new WindowsRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "apns":
                    registration = new AppleRegistrationDescription(deviceUpdate.Handle);
                    break;
                case "fcm":
                    registration = new FcmRegistrationDescription(deviceUpdate.Handle);
                    break;
                default:
                    return BadRequest();
            }

            registration.RegistrationId = id;
           
            // add check if user is allowed to add these tags
            registration.Tags = new HashSet<string>(deviceUpdate.Tags);
            registration.Tags.Add("rider:" + deviceUpdate.riderName);

            try
            {
                await hub.CreateOrUpdateRegistrationAsync(registration);
            }
            catch (MessagingException e)
            {
                ReturnGoneIfHubResponseIsGone(e);
            }

            return Ok(registration);
        }

        private static void ReturnGoneIfHubResponseIsGone(MessagingException e)
        {
            var webex = e.InnerException as WebException;
            if (webex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = (HttpWebResponse)webex.Response;
                if (response.StatusCode == HttpStatusCode.Gone)
                    throw new HttpRequestException(HttpStatusCode.Gone.ToString());
            }
        }

    }
}