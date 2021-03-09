using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2BootstrapServer.Entity
{
    public class AppSettings
    {
        

        public int RefreshHostInterval_Minute { get; set; }
        public string HostCheckStatusUrl { get; set; }
        public string SecretKey { get; set; }
        public int JwtVlidaity_day { get; set; }
        public string Issuer { get; set; }
        public List<string> Clients { get; set; }

    }








}
