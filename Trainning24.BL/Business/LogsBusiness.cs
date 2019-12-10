using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Logs;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class LogsBusiness
    {
        private readonly EFLogsRepository EFLogsRepository;



        public async Task<object> PullMessagesAsync(string projectId, string subscriptionId, bool acknowledge)
        {
            try
            {
                SubscriptionName subscriptionName = new SubscriptionName(projectId, subscriptionId);
                SubscriberClient subscriber = await SubscriberClient.CreateAsync(subscriptionName);
                await subscriber.StartAsync(
                    async (PubsubMessage message, CancellationToken cancel) =>
                    {
                        string text = Encoding.UTF8.GetString(message.Data.ToArray());
                        await Console.Out.WriteLineAsync($"Message {message.MessageId}: {text}");
                        return acknowledge ? SubscriberClient.Reply.Ack : SubscriberClient.Reply.Nack;
                    });
                await Task.Delay(3000);
                await subscriber.StopAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return 0;
        }

        public LogsBusiness(
            EFLogsRepository EFLogsRepository
        )
        {
            this.EFLogsRepository = EFLogsRepository;
        }

        public Logs Create(AddLogsModel AddLogsModel)
        {
            Logs Logs = new Logs
            {
                Data = AddLogsModel.data,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = 0
            };

            EFLogsRepository.Insert(Logs);
            return Logs;
        }

        public List<Logs> LogsList()
        {
            List<Logs> LogsList = new List<Logs>();
            LogsList = EFLogsRepository.GetAll();
            return LogsList;
        }

        public Logs getLogsById(long id)
        {
            return EFLogsRepository.GetById(b => b.Id == id && b.IsDeleted != true);
        }

        public Logs Delete(int Id, int DeleterId)
        {
            Logs Logs = new Logs();
            Logs.Id = Id;
            Logs.DeleterUserId = DeleterId;
            int i = EFLogsRepository.Delete(Logs);
            Logs deletedLogs = EFLogsRepository.GetById(b => b.Id == Id);
            return deletedLogs;
        }
    }
}
