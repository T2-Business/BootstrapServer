using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using T2BootstrapServer.Entity;
using T2BootstrapServers.Tokenization;
using T2BootstrapServers.Tokenization.Model;

namespace T2BootstrapServer.API.Controllers
{
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<EnvironmentController> _logger;
        private readonly AppSettings _setting;
        IAuthService _authService;
        public TokenController(ILogger<EnvironmentController> logger  ,IOptions<AppSettings> setting, IAuthService authService)
        {
            _logger = logger; 
            _setting = setting.Value;
            _authService = authService;
        }

        [HttpGet("{clientId}")]

        public IActionResult GeneratToken(string clientId)
        {
            if (!_setting.Clients.Contains(clientId))
            {
              return   BadRequest("invalid Client Id");
            }

          string token = _authService.GenerateToken(new JWTContainerModel() {
             ExpireDays =  _setting.JwtVlidaity_day,
              Issuer=_setting.Issuer
            });
            
       

            return Ok(token);
             
        }
    }
}
