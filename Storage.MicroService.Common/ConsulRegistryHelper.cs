using Consul;
using System;
using Microsoft.Extensions.Configuration;

namespace Storage.MicroService.Common
{
    public static class ConsulRegistryHelper
    {
        public static void ConsulRegistry(this IConfiguration configuration)
        {
            try
            {
                // 读取appsettings.json的配置信息
                string ip = configuration["ip"];
                string port = configuration["port"];
                string weight = configuration["weight"];
                string consulAddress = configuration["ConsulAddress"];
                string consulCenter = configuration["ConsulCenter"];

                // 创建连接consul客户端对象
                ConsulClient client = new ConsulClient(c =>
                {
                    c.Address = new Uri(consulAddress);
                    c.Datacenter = consulCenter;
                });

                // 注册服务
                client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = "StorageService " + Guid.NewGuid(),//--唯一的
                    Name = "StorageService",//分组---根据Service
                    Address = ip,
                    Port = int.Parse(port),
                    Tags = new string[] { weight.ToString() },//额外标签信息
                    Check = new AgentServiceCheck()
                    {
                        // 间隔多久发起回调的时间
                        Interval = TimeSpan.FromSeconds(12),
                        // 回调使用Http请求地址
                        HTTP = $"http://{ip}:{port}/Api/Health/Index",
                        // 超过5秒没有响应，上报不健康
                        Timeout = TimeSpan.FromSeconds(50),
                        // 剔除不健康服务的时间
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(20)
                    }//配置心跳规格参数
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