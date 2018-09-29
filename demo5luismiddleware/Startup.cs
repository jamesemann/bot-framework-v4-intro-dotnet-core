using demo5luismiddleware.Bots;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Bot.Builder.AI.Luis;

public class Startup
{
    // Inject the IHostingEnvironment into constructor
    public Startup(IHostingEnvironment env)
    {
        // Set the root path
        ContentRootPath = env.ContentRootPath;
    }

    // Track the root path so that it can be used to setup the app configuration
    public string ContentRootPath { get; private set; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Set up the service configuration
        var builder = new ConfigurationBuilder()
            .SetBasePath(ContentRootPath)
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();
        var configuration = builder.Build();
        services.AddSingleton(configuration);

        services.AddSingleton(sp =>
        {
            // get these values from luis.ai
            // i've left the endpoint in so you have an example of an url that works
            // because all luis client libs seem to vary 
            var luisApp = new LuisApplication(
                applicationId: "",
                endpointKey: "", 
                endpoint:"https://westus.api.cognitive.microsoft.com");

            var luisPredictionOptions = new LuisPredictionOptions
            {
                IncludeAllIntents = true,
            };

            return new LuisRecognizer(
                application: luisApp,
                predictionOptions: luisPredictionOptions,
                includeApiResults: true);
        });

        // Add your SimpleBot to your application
        services.AddBot<SimpleBot>(options =>
        {
            options.CredentialProvider = new ConfigurationCredentialProvider(configuration);
        });

    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseStaticFiles();

        // Tell your application to use Bot Framework
        app.UseBotFramework();
    }
}