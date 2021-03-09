using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2BootstrapServer.API.Filter
{
    public class CustomAttribute : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            throw new NotImplementedException();
        }
    }
}


 