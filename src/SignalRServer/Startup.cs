using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalRServer.Benchmark;
using SignalRServer.Feedback;
using SignalRServer.Handlers;
using SignalRServer.Hubs;
using SignalRServer.Models;
using SignalRServer.Storage;

namespace SignalRServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(origin => true);
                //.WithOrigins("http://localhost:8000");
            }));

            // 注入 SignalR 服务
            var signalrBuilder = services.AddSignalR(config =>
            {
                // 是否向客户端发送详细的错误消息。详细的错误消息包括来自服务器上引发异常的详细信息。
                config.EnableDetailedErrors = true;
                // 客户端如果在30s内没有向服务器发送任何消息，那么服务器端则会认为客户端已经断开连接了，建议值为 KeepAliveInterval 值的两倍 
                config.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
                // 服务器未在15s内向客户端发送消息，在15s的时候服务器会自动ping客户端，保持连接打开的状态
                config.KeepAliveInterval = TimeSpan.FromSeconds(15);

            });

            /*-------------------MessagePack 协议-----------------------*/


            /*
             * https://docs.microsoft.com/zh-cn/aspnet/core/signalr/messagepackhubprotocol?view=aspnetcore-3.1
             * 
             * 1. MessagePack是一种快速、精简的二进制序列化格式。 当性能和带宽需要考虑时，它很有用，因为它会创建比JSON更小的消息。 
             *    在查看网络跟踪和日志时，不能读取二进制消息，除非这些字节是通过 MessagePack 分析器传递的。 SignalR提供对 MessagePack 格式的内置支持，并为客户端和服务器提供要使用的 Api。
             *    小的整数会被编码成一个字节，短的字符串仅仅只需要比它的长度多一字节的大小。
             * 
             * 2. 前端启用 MesagePack 协议支持
             *    yarn add @microsoft/signalr-protocol-msgpack
             *     const connection = new signalR.HubConnectionBuilder()
             *              .withUrl("/chathub")
             *              .withHubProtocol(new signalR.protocols.msgpack.MessagePackHubProtocol())
             *              .build();
             */

            //signalrBuilder.AddMessagePackProtocol(
            //// 自定义 MessagePack 如何设置数据的格式，FormatterResolvers 属性用于配置 MessagePack 序列化选项
            ////options =>
            ////{
            ////    options.FormatterResolvers = new List<MessagePack.IFormatterResolver>()
            ////    {
            ////        MessagePack.Resolvers.StandardResolver.Instance
            ////    };
            ////}
            //);

            // 注入请求统计后台服务
            services.AddHostedService<HostedCounterService>();

            // 注入消息反馈监听服务
            services.AddHostedService<FeedbackMonitorService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();

            //允许所有跨域，cors是在ConfigureServices方法中配置的跨域策略名称
            //注意：UseCors必须放在UseRouting和UseEndpoints之间
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ProxyHub>("/signalr");
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModele());
        }
    }

    public class AutofacModele : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 客户端信息存储 -- 临时
            //context.Services.AddSingleton<ConnectionCounter>();
            builder.RegisterType<ConnectionCounter>().SingleInstance();

            // 客户端信息存储 -- 临时
            //context.Services.AddSingleton<ClientStorage>(); 
            builder.RegisterType<ClientStorage>().SingleInstance();

            // 消息历史记录
            //context.Services.AddSingleton<MessageHistory>();
            builder.RegisterType<MessageHistory>().SingleInstance();

            // 批量注入命令处理函数
            var assembly = Assembly.GetExecutingAssembly();
            var handlers = assembly.GetTypes().Where(p => p.IsClass && typeof(ICommandHandler).IsAssignableFrom(p)).ToList();
            handlers.ForEach(t =>
            {
                var att = t.GetCustomAttribute<InjectNamedAttribute>();
                if (att != null)
                {
                    builder.RegisterType(t).Named<ICommandHandler>(att.Named).InstancePerLifetimeScope();
                }
            });
        }
    }
}
