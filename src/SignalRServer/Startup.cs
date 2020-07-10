using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SignalRServer.Benchmark;
using SignalRServer.Hubs;

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
                .WithOrigins("http://localhost:8000");
            }));

            // 注入 SignalR 服务
            var signalrBuilder = services.AddSignalR(config =>
            {
                // 是否向客户端发送详细的错误消息。详细的错误消息包括来自服务器上引发异常的详细信息。
                config.EnableDetailedErrors = true;
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

            services.AddSingleton<ConnectionCounter>();
            services.AddHostedService<HostedCounterService>();

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
                endpoints.MapHub<ChatHub>("/hubs/chathub");
                endpoints.MapControllers();
            });
        }
    }
}
