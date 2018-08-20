using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using demo10prompts.Bots;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace demo10prompts
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(HostingEnvironment.ContentRootPath)
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables();
            var configuration = builder.Build();
            services.AddSingleton(configuration);

            services.AddMvc();
            services.AddBot<SimpleBot>((options) =>
           {

               IStorage dataStore = new MemoryStorage();
               options.Middleware.Add(new ConversationState<Dictionary<string,object>>(dataStore));
               options.CredentialProvider = new ConfigurationCredentialProvider(configuration);
           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
                        
            app.UseMvc();
            app.UseBotFramework();
        }
    }
}
