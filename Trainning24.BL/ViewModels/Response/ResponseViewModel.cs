using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Response
{
    public  class ResponseViewModel
    {
    }

    public class NotificationResponse
    {
        public bool success { get; set; }
        public int status { get; set; }
        public string message { get; set; }
    }
}
