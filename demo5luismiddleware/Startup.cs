using demo5luismiddleware.Bots;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.Ai.LUIS;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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


        // Add your SimpleBot to your application
        services.AddBot<SimpleBot>(options =>
        {
            options.CredentialProvider = new ConfigurationCredentialProvider(configuration);

            // Add LUIS recognizer as middleware
            // fill in these values
            options.Middleware.Add(
                new LuisRecognizerMiddleware(
                    new LuisModel(
                        "",
                        "",
                        new Uri(""))));
        });


    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseStaticFiles();

        // Tell your application to use Bot Framework
        app.UseBotFramework();
    }
}