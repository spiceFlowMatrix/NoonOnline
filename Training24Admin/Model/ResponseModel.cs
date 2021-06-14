using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Training24Admin.Model
{
    public class PaginationResponse
    {
        public int response_code { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int totalcount { get; set; }
        public Object data { get; set; }
    }

    public class SuccessResponse
    {
        public int response_code { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public Object data { get; set; }
    }

    public class UnsuccessResponse
    {
        public int response_code { get; set; }
        public string message { get; set; }
        public string status { get; set; }        
    }

    public class Response
    {        
        public int response_code { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public Object data { get; set; }
    }
    public class AthResponse
    {
        public int response_code { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string token { get; set; }
        //public Object data { get; set; }
    }


    public class AuthLoginResponse
    {
        public int response_code { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string userid { get; set; }
    }


    public class ResponseNotificationModel
    {
        public int response_code { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public Object data { get; set; }
        string playerName { get; set; }
        string teamName { get; set; }
    }
    public class ResponseTimeZoneModel {
        public int response_code { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public DateTime server_current_time { get; set; }
        public string server_time_zone { get; set; }
        public DateTime event_time { get; set; }
        public DateTime event_converted_time { get; set; }
        public bool isDaylight { get; set; }
        public string dayLightStartDate { get; set; }
        public string dayLightEndDate { get; set; }
        public TimeSpan dayLightDifference { get; set; }
    }

    public class DiscussionFileResponse
    {
        public int response_code { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public long file_id { get; set; }
    }
}
