using System;
using System.Collections.Generic;
using System.Text;
using T2BootstrapServers.Tokenization.Model;

namespace T2BootstrapServers.Tokenization
{
   public interface IAuthService
    {
        string GenerateToken(IAuthContainerModel model);
        
    }
}
