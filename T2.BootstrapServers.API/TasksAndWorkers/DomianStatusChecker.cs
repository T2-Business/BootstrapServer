using log4net;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using T2BootstrapServer.Entity;
using T2BootstrapServer.Entity.Model;

namespace T2BootstrapServer.API
{
    public class DomainStatusManager : IHostedService, IDisposable
    {
        #region Fields 
        ConcurrentBag<CachedHost> hostList = new ConcurrentBag<CachedHost>();
       
        static CancellationTokenSource consumertoken = new CancellationTokenSource();
        static ActionBlock<DateTimeOffset> task;
        private readonly List<HostSetting> _domainList;
        private readonly ILogger<DomainStatusManager> _logger;
        private readonly AppSettings _setting;
        private readonly IMemoryCache cache;
        #endregion


        #region Constructor 
        public DomainStatusManager(ILogger<DomainStatusManager> logger, IOptions<List<HostSetting>> options, IOptions<AppSettings> setting, IMemoryCache memoryCache)
        {
            _logger = logger;
            _domainList = options.Value;
            _setting = setting.Value;
            cache = memoryCache;
        } 
        #endregion

        private async Task CheckHostStatusAsync()
        {
            _logger.LogInformation($"DomainStatusChecker-CheckHostStatus ,  Start check the servers status ");
            List<Task> tasks = new List<Task>();
            hostList.Clear();
            foreach (var item in _domainList)
            { 
                tasks.Add(CheckServer(item)); 
            }

            await Task.WhenAll(tasks);
             
            InsertIntoCache();
        }

        private void InsertIntoCache()
        {
            cache.Remove(Variables.CacheKey);
            cache.Set(Variables.CacheKey, hostList.ToList());
        }

        private async Task CheckServer(HostSetting hostSettings)
        {
            foreach (var hostUrl in hostSettings.ServerUrl)
            {
                try
                {

                    using (HttpClient httpClient = new HttpClient())
                    {
                        _logger.LogInformation($"DomainStatusChecker-CheckHostStatus ,  Start check the [ {hostUrl} ] status ");
                        if (!string.IsNullOrEmpty(hostUrl))
                        {
                            httpClient.BaseAddress = new Uri(hostUrl);
                            httpClient.DefaultRequestHeaders.Accept.Clear();
                            var Response = await httpClient.GetAsync(_setting.HostCheckStatusUrl);
                            if (Response.IsSuccessStatusCode)
                            {

                                StoreCompletedHost(hostSettings, hostUrl, true);
                                _logger.LogInformation($"DomainStatusChecker-CheckHostStatus ,  Server [ {hostUrl} ] is Live ");
                            }
                            else
                            {
                                StoreCompletedHost(hostSettings, hostUrl, false);
                                _logger.LogInformation($"DomainStatusChecker-CheckHostStatus ,  Server [ {hostUrl} ] is Down  ");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    StoreCompletedHost(hostSettings, hostUrl, false);
                    _logger.LogError(ex, $"DomainStatusChecker-CheckHostStatus ,  Start check the [ {hostUrl} ] status ");
                }
            }
        }


        private void StoreCompletedHost(HostSetting hostSettings, string hostUrl, bool status)
        {

            hostList.Add(
            new CachedHost()
            {
                AccountId = hostSettings.AccountId,
                AccountCode = hostSettings.AccountCode,
                CountryCode = hostSettings.CountryCode,
                MaxRequestPerSecond = hostSettings.MaxRequestPerSecond,
                OSType = hostSettings.OSType,
                TimeZone = hostSettings.TimeZone,
                UserId = hostSettings.UserId,
                UserName = hostSettings.UserName,
                ServerUrl = hostUrl,
                IsActive = status

            });
        }


        #region HostedService Methods

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            int consumerDalyTime = Convert.ToInt32(_setting.RefreshHostInterval_Minute);
            task = SustainableTaskManager.SustainableTask(async now => await CheckHostStatusAsync(), consumertoken.Token, consumerDalyTime);
            task.Post(DateTimeOffset.Now);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            task.Complete();
            return Task.CompletedTask;
        } 
        #endregion

         

        #region Dispose
        public void Dispose(bool status)
        {

        }
        public void Dispose()
        {

        } 
        #endregion
    }
}
