using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZCC.MicroService.Identity4
{
    /// <summary>
    /// 创建IdentityServer4 配置类
    /// 1、配置作用域变量
    /// 2、配置访问客户端账户变量
    /// 3、定义
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// 定义作用域变量
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("gatewayScope"),
            new ApiScope("scope2")
        };

        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId="app_test",
                AllowedGrantTypes=GrantTypes.ClientCredentials,
                ClientSecrets={
                    new Secret("123456".Sha256())
                },
                AllowedScopes=new List<string>{
                    "gatewayScope"
                }
            }
        };

        /// <summary>
        /// 定义ApiResource   
        /// 这里的资源（Resources）指的就是管理的API
        /// </summary>
        /// <returns>多个ApiResource</returns>
        public static IEnumerable<ApiResource> ApiResources =>
        new[]
        {
            new ApiResource("MicroService", "用户获取API")
            {
                Scopes={ "gatewayScope" }//4.x必须写的
            }
        };
    }

}

