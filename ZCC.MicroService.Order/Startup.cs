using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Order.MicroService.Common;
using Order.MicroService.IService;
using Order.MicroService.Service;
using Order.MicroService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZCC.MicroService.Order
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpReports().AddHttpTransport();
            services.AddControllers();

            //AddCors方法的调用会把CORS服务加到应用的服务容器中（service container）;
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            });

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        options.Authority = "https://localhost:6500";//ids4的地址--专门获取公钥
            //        options.ApiName = "MicroService";
            //        options.RequireHttpsMetadata = false;
            //    });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ZCC.MicroService.Order", Version = "v1" });
            });

            #region 注册服务
            services.AddTransient<OrderDBContext>();
            services.AddTransient<IAccountService, AccountServiceImpl>();
            services.AddTransient<IStorageService, StorageServiceImpl>();
            // 必须使用该方式注册，服务熔断机制才能生效
            services.AddSingleton<IOrderService, OrderServiceImpl>();
            #endregion
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseHttpReports();

            //app.UseCors(MyAllowSpecificOrigins);    //这个代码会把CORS策略通过CORS中间件应用到这个应用的所有终端(endpoints);即把跨域作用到整个应用

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZCC.MicroService.Order v1"));


            app.UseRouting();
            //添加鉴权管道
           // app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //注册Consul
            this.Configuration.ConsulRegistry();
        }
    }
}
