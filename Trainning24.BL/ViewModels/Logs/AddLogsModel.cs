using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Logs
{
    public class AddLogsModel
    {
        public string data { get; set; }
    }


    public class NotifiactionPayload
    {
        public Message message { get; set; }
        public string subscription { get; set; }
    }

    public class Message
    {
        public Attribute attributes { get; set; }
        public string data { get; set; }
        public string messageId { get; set; }
        public string message_id { get; set; }
        public string publishTime { get; set; }
        public string publish_time { get; set; }
    }

    public class Attribute
    {
        public string bucketId { get; set; }
        public string eventTime { get; set; }
        public string eventType { get; set; }
        public string notificationConfig { get; set; }
        public string objectGeneration { get; set; }
        public string objectId { get; set; }
        public string payloadFormat { get; set; }
    }

    public class DecodedData
    {
        public string kind { get; set; }
        public string id { get; set; }
        public string selfLink { get; set; }
        public string name { get; set; }
        public string bucket { get; set; }
        public string generation { get; set; }
        public string metageneration { get; set; }
        public string contentType { get; set; }
        public string timeCreated { get; set; }
        public string updated { get; set; }
        public string storageClass { get; set; }
        public string timeStorageClassUpdated { get; set; }
        public string size { get; set; }
        public string md5Hash { get; set; }
        public string mediaLink { get; set; }
        public string crc32c { get; set; }
        public string etag { get; set; }
    }

}

