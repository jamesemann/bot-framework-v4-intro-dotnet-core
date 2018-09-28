using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dispatch
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Env.ContentRootPath)
                .AddJsonFile("appsettings.json");

            var configuration = configurationBuilder
                .Build();


            services.AddBot<Bot>((options) => {
                options.CredentialProvider = new ConfigurationCredentialProvider(configuration);

                var dispatcherConfiguration = Config.GetDispatchConfiguration(this.Configuration).Dispatcher;
                
                var luisModel = new LuisModel(dispatcherConfiguration.Id, dispatcherConfiguration.Key, dispatcherConfiguration.Url);
                var luisOptions = new LuisRequest { Verbose = true };

                options.Middleware.Add(new LuisRecognizerMiddleware(luisModel, luisOptions: luisOptions));
            });

            services.AddMvc();
        }

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
