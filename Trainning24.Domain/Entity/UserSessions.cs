using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Domain.Entity
{
    public class UserSessions : EntityBase
    {
        public long UserId { get; set; }
        [MaxLength]
        public string AccessToken { get; set; }
        [MaxLength]
        public string DeviceToken { get; set; }
        [StringLength(10)]
        public string DeviceType { get; set; }
        [StringLength(20)]
        public string Version { get; set; }
    }
}
