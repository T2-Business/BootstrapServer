using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace T2BootstrapServers.Tokenization.Model
{
    public interface IAuthContainerModel
    {
  
        int ExpireDays { get; set; }
        string  Issuer { get; set; }
        Claim[] Claims { get; set; }
    }
}
