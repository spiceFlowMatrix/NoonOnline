using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trainning24.BL.ViewModels
{
    public class NotificationContent
    {
        private Dictionary<string, object[]> _customItems = new Dictionary<string, object[]>();

        private Dictionary<long, int> _userBadges = new Dictionary<long, int>();

        public string Alert { get; set; }

        protected int? Badge { get; set; }

        public NotificationContent AddCustom(string key, params object[] values)
        {
            if (values == null) return this;
            List<object> items;
            if (_customItems.ContainsKey(key))
                items = _customItems[key].ToList();
            else
                items = new List<object>();
            foreach (var item in values)
                items.Add(item);
            _customItems[key] = items.ToArray();
            return this;
        }

        public int? GetBadge(long? userId)
        {
            int? badge = null;
            if (userId.HasValue)
                badge = _userBadges.ContainsKey(userId.Value) ? (int?)_userBadges[userId.Value] : null;
            if (!badge.HasValue && this.Badge.HasValue)
                badge = this.Badge.Value;
            return badge;
        }

        public NotificationContent WithBadge(int? value)
        {
            this.Badge = value;
            return this;
        }

        public NotificationContent WithBadge(int? value, long userId)
        {
            if (value.HasValue)
                _userBadges[userId] = value.Value;
            else if (_userBadges.ContainsKey(userId))
                _userBadges.Remove(userId);
            return this;
        }

        public Dictionary<string, object[]> GetCustom()
        {
            var items = new Dictionary<string, object[]>();

            foreach (var item in _customItems)
                items.Add(item.Key, item.Value);

            return items;
        }

        public static NotificationContent CreateInstance(string alert)
        {
            return new NotificationContent
            {
                Alert = alert
            };
        }

        public JObject ToJObject()
        {
            //long? userId
            JObject json = new JObject();

            json["alert"] = this.Alert;
            //int? badge = GetBadge(userId);
            //if (badge.HasValue)
            //    json["badge"] = badge.Value;

            foreach (var item in _customItems)
            {
                if (item.Value.Length > 0)
                {
                    if (item.Value.Length == 1)
                        json[item.Key] = new JValue(item.Value.First());
                    else
                        json[item.Key] = new JArray(item.Value);
                }
            }

            return json;
        }

        public override string ToString()
        {
            return ToJObject().ToString();
        }
    }
}
