using System.Collections.Generic;
using demo7dialogs.Bots;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    // Inject the IHostingEnvironment into constructor
    public Startup(IHostingEnvironment env)
    {
        // Set the root path
        ContentRootPath = env.ContentRootPath;
    }

    // Track the root path so that it can be used to setup the app configuration
    public string ContentRootPath { get; }

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
        services.AddBot<SimpleBotComponents>(options =>
        {
            options.CredentialProvider = new ConfigurationCredentialProvider(configuration);
            options.Middleware.Add(new ConversationState<Dictionary<string, object>>(new MemoryStorage()));
        });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseStaticFiles();

        // Tell your application to use Bot Framework
        app.UseBotFramework();
    }
}