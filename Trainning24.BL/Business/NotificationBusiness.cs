using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PushSharp.Core;
using PushSharp.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class NotificationBusiness
    {
        public void SendNotification(List<UserSessions> devices, NotificationContent content)
        {
            // Configuration FCM (use this section for FCM)
            var config = new GcmConfiguration("AAAAvNmd35s:APA91bGtLARteUoqTVc5y9l2dOv5eMmGFKL584d-I38tm00ucwsW3AYuk0_OxkXXucb2_87Ca7euhFxx_aCEMSYaHtQK_cL6mMZ4mXa8Sl3uoHPa4TNCThjaFh0rWg8S1psqnw5WRZ8iEp1r-dc4MwYEfuaflh2zfw");
            config.GcmUrl = "https://fcm.googleapis.com/fcm/send";
            var provider = "FCM";
            // Create a new broker
            var gcmBroker = new GcmServiceBroker(config);
            // Wire up events
            gcmBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {
                aggregateEx.Handle(ex =>
                {
                    // See what kind of exception it was to further diagnose
                    if (ex is GcmNotificationException notificationException)
                    {
                        // Deal with the failed notification
                        var gcmNotification = notificationException.Notification;
                        var description = notificationException.Description;
                        Console.WriteLine($"{provider} Notification Failed: ID={gcmNotification.MessageId}, Desc={description}");
                    }
                    else if (ex is GcmMulticastResultException multicastException)
                    {
                        foreach (var succeededNotification in multicastException.Succeeded)
                        {
                            Console.WriteLine($"{provider} Notification Succeeded: ID={succeededNotification.MessageId}");
                        }
                        foreach (var failedKvp in multicastException.Failed)
                        {
                            var n = failedKvp.Key;
                            var e = failedKvp.Value;

                            Console.WriteLine($"{provider} Notification Failed: ID={n.MessageId}, Desc={e}");
                        }
                    }
                    else if (ex is DeviceSubscriptionExpiredException expiredException)
                    {

                        var oldId = expiredException.OldSubscriptionId;
                        var newId = expiredException.NewSubscriptionId;

                        Console.WriteLine($"Device RegistrationId Expired: {oldId}");

                        if (!string.IsNullOrWhiteSpace(newId))
                        {
                            // If this value isn't null, our subscription changed and we should update our database
                            Console.WriteLine($"Device RegistrationId Changed To: {newId}");
                        }
                    }
                    else if (ex is RetryAfterException retryException)
                    {
                        // If you get rate limited, you should stop sending messages until after the RetryAfterUtc date
                        Console.WriteLine($"{provider} Rate Limited, don't send more until after {retryException.RetryAfterUtc}");
                    }
                    else
                    {
                        Console.WriteLine("{provider} Notification Failed for some unknown reason");
                    }
                    // Mark it as handled
                    return true;
                });
            };

            gcmBroker.OnNotificationSucceeded += (notification) =>
            {
                Console.WriteLine("{provider} Notification Sent!");
            };
            // Start the broker
            gcmBroker.Start();
            //foreach (var regId in MY_REGISTRATION_IDS)
            //{
            //    // Queue a notification to send
            //    //gcmBroker.QueueNotification(new GcmNotification
            //    //{
            //    //    RegistrationIds = new List<string> {
            //    //    regId
            //    //},
            //    //    Data = JObject.Parse("{ \"somekey\" : \"somevalue\" }")
            //    //});
            //}
            devices.GroupBy(g => g.DeviceToken)
                .Select(s => s.First()).ToList().ForEach(e =>
                {
                    switch (e.DeviceType)
                    {
                        case "Android":
                            {
                                var androidNotification = new GcmNotification
                                {
                                    To = e.DeviceToken,
                                    CollapseKey = "NONE",
                                    Data = content.ToJObject()
                                };
                                gcmBroker.QueueNotification(androidNotification);
                                break;
                            }
                    }
                });
            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            gcmBroker.Stop();
        }
    }
}
