using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace T2BootstrapServers.Tokenization.Model
{
    public class JWTContainerModel : IAuthContainerModel
    {
        #region Members
        public int ExpireDays { get; set; } = 1800; 
  
 

        public Claim[] Claims { get; set; }
        public string Issuer { get; set; }

        #endregion
    }
}
