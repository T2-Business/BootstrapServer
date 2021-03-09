using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using T2BootstrapServer.API.Helper;
using T2BootstrapServer.Entity;
using T2BootstrapServer.Entity.Model;
using T2BootstrapServers.Tokenization;
using T2BootstrapServers.Tokenization.Model;

namespace T2BootstrapServer.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EnvironmentController : ControllerBase
    {
        #region Fields
        private readonly ILogger<EnvironmentController> _logger;
        private readonly AppSettings _setting;
        private readonly List<HostSetting> configurationHostList;
        IMemoryCache _cache;
        #endregion

        #region Constructor
        public EnvironmentController(ILogger<EnvironmentController> logger, IMemoryCache cache, IOptions<AppSettings> setting, IOptions<List<HostSetting>> hostList)
        {

            _logger = logger;
            _setting = setting.Value;
            _cache = cache;
            configurationHostList = hostList.Value;
        }
        #endregion

        [HttpPost]
        public IEnumerable<string> Check(ClientModel model)
        {
            IList<CachedHost> _cachedDomainList = _cache.Get<List<CachedHost>>(Variables.CacheKey);

            if (_cachedDomainList != null && _cachedDomainList.Any())
            {
                // now we will get all the Active Host  because we  the cache is full and the domain are checked for the availability 
                
				var predicate = PredicateBuilder.True<CachedHost>();
                predicate = predicate.And(a => a.AccountCode.Equals(model.AccountCode, StringComparison.OrdinalIgnoreCase) || a.AccountCode == "*");
                predicate = predicate.And(a => a.AccountId.Equals(model.AccountId, StringComparison.OrdinalIgnoreCase) || a.AccountId == "*");
                predicate = predicate.And(a => a.CountryCode.Equals(model.CountryCode, StringComparison.OrdinalIgnoreCase) || a.CountryCode == "*");
                predicate = predicate.And(a => a.OSType.Equals(model.OSType, StringComparison.OrdinalIgnoreCase) || a.OSType == "*");
                predicate = predicate.And(a => a.TimeZone.Equals(model.TimeZone, StringComparison.OrdinalIgnoreCase) || a.TimeZone == "*");
                predicate = predicate.And(a => a.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase) || a.UserName == "*");
                predicate = predicate.And(a => a.UserId.Equals(model.UserId, StringComparison.OrdinalIgnoreCase) || a.UserId == "*");
                if (model.UsedHostList.Any())
                {
                    predicate = predicate.And(a => !model.UsedHostList.Contains(a.ServerUrl));
                }
                return _cachedDomainList.AsQueryable().Where(predicate).Select(a => a.ServerUrl);
            }
            else
            {
                // the cache is not filled yet and the host is not checked for the availability so we will get the url from the Configuration 
                return GetAllHostUrl(model);

            }

        }



        #region Helpers

        private List<string> GetAllHostUrl(ClientModel model)
        {
            var predicate = PredicateBuilder.True<HostSetting>();
            predicate = predicate.And(a => a.AccountCode.Equals(model.AccountCode, StringComparison.OrdinalIgnoreCase) || a.AccountCode == "*");
            predicate = predicate.And(a => a.AccountId.Equals(model.AccountId, StringComparison.OrdinalIgnoreCase) || a.AccountId == "*");
            predicate = predicate.And(a => a.CountryCode.Equals(model.CountryCode, StringComparison.OrdinalIgnoreCase) || a.CountryCode == "*");
            predicate = predicate.And(a => a.OSType.Equals(model.OSType, StringComparison.OrdinalIgnoreCase) || a.OSType == "*");
            predicate = predicate.And(a => a.TimeZone.Equals(model.TimeZone, StringComparison.OrdinalIgnoreCase) || a.TimeZone == "*");
            predicate = predicate.And(a => a.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase) || a.UserName == "*");
            predicate = predicate.And(a => a.UserId.Equals(model.UserId, StringComparison.OrdinalIgnoreCase) || a.UserId == "*"); 
            var hosts = configurationHostList.AsQueryable().Where(predicate).Select(a => a.ServerUrl).ToList();
            return hosts.SelectMany(x => x).ToList();

        }
        #endregion
    }
}
