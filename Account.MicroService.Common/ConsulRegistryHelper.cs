using Consul;
using Microsoft.Extensions.Configuration;
using System;

namespace Account.MicroService.Common
{
    public static class ConsulRegistryHelper
    {
        public static void ConsulRegistry(this IConfiguration configuration)
        {
            try
            {
                string ip = configuration["ip"];
                string port = configuration["port"];
                string weight = configuration["weight"];
                string consulAddress = configuration["ConsulAddress"];
                string consulCenter = configuration["ConsulCenter"];

                ConsulClient client = new ConsulClient(c =>
                {
                    c.Address = new Uri(consulAddress);
                    c.Datacenter = consulCenter;
                });

                client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = "AccountService " + Guid.NewGuid(),//--唯一的
                    Name = "AccountService",//分组---根据Service
                    Address = ip,
                    Port = int.Parse(port),
                    Tags = new string[] { weight.ToString() },//额外标签信息
                    Check = new AgentServiceCheck() // 添加心跳检查
                    {
                        // 间隔多久发起回调的时间
                        Interval = TimeSpan.FromSeconds(12),
                        // 回调使用Http请求地址
                        HTTP = $"http://{ip}:{port}/Api/Health/Index",
                        // 超过5秒没有响应，上报不健康
                        Timeout = TimeSpan.FromSeconds(50),
                        // 剔除不健康服务的时间
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20)
                    }//配置心跳
                });
                Console.WriteLine($"{ip}:{port}--weight:{weight}"); //命令行参数获取
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Consul注册：{ex.Message}");
            }
        }
    }
}

