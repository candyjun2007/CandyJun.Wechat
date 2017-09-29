using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.Containers;
using Microsoft.Extensions.Options;
using CandyJun.Wechat.Utilities;

namespace CandyJun.Wechat
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            //Senparc.Weixin.SDK 配置
            builder.AddJsonFile("SenparcWeixin.json", optional: true);

            Configuration = builder.Build();
            //提供网站根目录
            Server.AppDomainAppPath = env.ContentRootPath;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.Configure<SenparcWeixinSetting>(Configuration.GetSection("SenparcWeixinSetting"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            #region 微信相关

            //注册微信
            AccessTokenContainer.Register(senparcWeixinSetting.Value.WeixinAppId, senparcWeixinSetting.Value.WeixinAppSecret);

            //Senparc.Weixin SDK 配置
            Senparc.Weixin.Config.DefaultSenparcWeixinSetting = senparcWeixinSetting.Value;

            #endregion
        }
    }
}
