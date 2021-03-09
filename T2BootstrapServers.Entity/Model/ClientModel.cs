using System;
using System.Collections.Generic;
using System.Text;

namespace T2BootstrapServer.Entity.Model
{
    public class ClientModel
    {
        public ClientModel()
        {
            UsedHostList = new List<string>();
        }
        public string AccountId { get; set; } = "*";
        public string AccountCode { get; set; } = "*";
        public string TimeZone { get; set; } = "*";
        public string OSType { get; set; } = "*";
        public string CountryCode { get; set; } = "*";
        public string UserName { get; set; } = "*";
        public string UserId { get; set; } = "*";
        public List<string> UsedHostList { get; set; }
    }
}
