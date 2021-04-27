using Consul;
using Microsoft.Extensions.Logging;
using Order.MicroService.Common;
using Order.MicroService.IService;
using Polly;
using Polly.Timeout;
using Polly.Wrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Order.MicroService.Service
{
    public class StorageServiceImpl : IStorageService
    {
        private AsyncPolicyWrap<bool> _policyWrap;
        private ILogger<AccountServiceImpl> _logger;
        public StorageServiceImpl(ILogger<AccountServiceImpl> logger)
        {
            _logger = logger;
            // 调用超时策略
            var timeout = Polly.Policy
                  // 超过一秒钟，就设定超时
                  .TimeoutAsync(1, TimeoutStrategy.Pessimistic, (context, ts, task) =>
                  {
                      _logger.LogInformation("AccountServiceImpl调用超时");
                      return Task.CompletedTask;
                  });
            // 调用熔断策略
            var circuitBreakerPolicy = Polly.Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                  exceptionsAllowedBeforeBreaking: 5,             // 连续5次异常
                  durationOfBreak: TimeSpan.FromMilliseconds(10),       // 断开10秒钟
                  onBreak: (exception, breakDelay) =>
                  {
                      //熔断以后，需要处理的动作；  记录日志；
                      _logger.LogInformation($"AccountServiceImpl服务出现=========>熔断");
                      _logger.LogInformation($"熔断: {breakDelay.TotalMilliseconds } ms, 异常: " + exception.Message);
                  },
                  onReset: () => //// 熔断器关闭时
                  {
                      _logger.LogInformation($"AccountServiceImpl服务熔断器关闭了");
                  },
                  onHalfOpen: () => // 熔断时间结束时，从断开状态进入半开状态
                  {
                      _logger.LogInformation($"AccountServiceImpl服务熔断时间到，进入半开状态");
                  });

            _policyWrap = Policy<bool>
                .Handle<Exception>()
                .FallbackAsync(StorageServiceFallback(), (x) =>
                {
                    _logger.LogInformation($"AccountService进行了服务降级 -- {x.Exception.Message}");
                    return Task.CompletedTask;
                })
                .WrapAsync(circuitBreakerPolicy)
                .WrapAsync(timeout);
        }

        /// <summary>
        /// 提供的降级方法
        /// </summary>
        /// <returns></returns>
        public bool StorageServiceFallback()
        {
            return false;
        }
        /// <summary>
        /// 走异步方式
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<bool> DecreaseStorage1(long? productId, int? count)
        {
            return await _policyWrap.ExecuteAsync(() => {
                AgentService agentService = ConsulBalanceHelper.ChooseService("StorageService");
                string url = $"http://{agentService.Address}:{agentService.Port}/api/storage/decrease/{productId}/{count}";
                ApiResponse result = HttpHelper.HttpRequest<ApiResponse>(url, HttpMethod.Get, null);
                return Task.FromResult(result.Success);
            });
        }
        /// <summary>
        /// 走同步方式
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool DecreaseStorage(long? productId, int? count)
        {
            // string url = $"http://192.168.3.199:5202/api/storage/decrease/{productId}/{count}";
            // 获取随机负载到服务实例
            AgentService agentService = ConsulBalanceHelper.ChooseService("StorageService");
            string url = $"http://{agentService.Address}:{agentService.Port}/api/storage/decrease/{productId}/{count}";
            ApiResponse result = HttpHelper.HttpRequest<ApiResponse>(url, HttpMethod.Get, null);
            return result.Success;
        }
    }
}
