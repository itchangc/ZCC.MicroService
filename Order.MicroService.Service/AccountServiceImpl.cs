using Polly;
using Order.MicroService.Common;
using System;
using System.Net.Http;
using Polly.Timeout;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Consul;
using Order.MicroService.IService;

namespace Order.MicroService.Service
{
    public class AccountServiceImpl : IAccountService
    {
        private ILogger<AccountServiceImpl> _logger;
        private AsyncPolicy<bool> _asyncPolicy;
        public AccountServiceImpl(ILogger<AccountServiceImpl> logger)
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

            _asyncPolicy = Policy<bool>
                .Handle<Exception>()
                .FallbackAsync(AccountServiceFallback(), (x) =>
                {
                    _logger.LogInformation($"AccountService进行了服务降级 -- {x.Exception.Message}");
                    return Task.CompletedTask;
                })
                .WrapAsync(circuitBreakerPolicy)
                .WrapAsync(timeout);
        }

        public bool AccountServiceFallback()
        {
            return false;
        }
        /// <summary>
        /// 异步
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public async Task<bool> DeductMoney1(long? userId, decimal? money)
        {
            return await _asyncPolicy.ExecuteAsync(() => {

                // 获取随机负载到服务实例
                AgentService agentService = ConsulBalanceHelper.ChooseService("AccountService");
                string url = $"http://{agentService.Address}:{agentService.Port}/api/account/decrease/{userId}/{money}";
                ApiResponse apiResponse = HttpHelper.HttpRequest<ApiResponse>(url, HttpMethod.Get, null);
                return Task.FromResult(apiResponse.Success);
            });
        }
        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public bool DeductMoney(long? userId, decimal? money)
        {
            // string url = $"http://localhost:9000/api/account/decrease/{userId}/{money}";
            // 获取随机负载到服务实例
            AgentService agentService = ConsulBalanceHelper.ChooseService("AccountService");
            string url = $"http://{agentService.Address}:{agentService.Port}/api/account/decrease/{userId}/{money}";
            ApiResponse apiResponse = HttpHelper.HttpRequest<ApiResponse>(url, HttpMethod.Get, null);
            return apiResponse.Success;

        }
    }
}