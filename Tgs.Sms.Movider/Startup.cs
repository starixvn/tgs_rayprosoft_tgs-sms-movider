using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tgs.Sms.Movider.Core;
using Tgs.Sms.Movider.MessageQueue;

namespace Tgs.Sms.Movider
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRabbitMessageQueue(queueConfig =>
                {
                    queueConfig
                        .ConfigQueueOptions((options, sp) =>
                        {
                            var config = sp.GetRequiredService<IConfiguration>();

                            config.GetSection("Sms").Bind(options);

                            options.OnSetupQueueAndExchange = channel =>
                            {
                                channel.QueueDeclare(
                                    "tgs.sms.movider",
                                    true,
                                    false,
                                    false,
                                    null);

                                channel.QueueBind(
                                    "tgs.sms.movider",
                                    "amq.direct",
                                    "tgs.sms.movider",
                                    null);
                            };
                        })
                        .AddHandler<MoviderReceiveHandler>("tgs.sms.movider", false);
                });

            services
                .AddSingleton<MoviderSerializer>()
                .AddMoviderRepositories(
                (sp, dbContextBuilder) =>
                {
                    var config = sp.GetRequiredService<IConfiguration>();

                    dbContextBuilder.UseNpgsql(config.GetConnectionString("Tgs"));
                });

            services.AddMoviderSms((options,sp)=>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                config.GetSection("MoviderSms").Bind(options);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });            
        }
    }
}