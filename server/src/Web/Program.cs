using System;
using System.Text.Json;
using Amazon;
using Amazon.CloudWatchLogs;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.AwsCloudWatch;
using Web;
using Web.Filters;
using Web.Services.Authentication;
using Microsoft.AspNetCore.SignalR;
using Web.Data;
using Web.Hubs;
using Web.Services.Antiforgery;
using Web.Services.Hashing;
using Web.Services.Geocoding;
using Web.Data.EF;
using Web.Services;
using Web.Services.Billing;
using Web.Services.Cache;
using Web.Services.DateTimeServices;
using Web.Services.Events;
using Web.Services.Mail;
using Web.Services.Storage;
using Microsoft.Extensions.Configuration;
using Autofac;
using Web.Services.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;

Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

Log.Information("Starting web host");

try
{
    var appBuilder = WebApplication.CreateBuilder(args);

    appBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

    appBuilder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console();

        if (context.HostingEnvironment.IsProduction())
        {
            var awsSettings = services.GetRequiredService<AwsSettings>();

            configuration
                .WriteTo.AmazonCloudWatch(
                    logGroup: "grubhub",
                    logStreamPrefix: "web",
                    restrictedToMinimumLevel: LogEventLevel.Warning,
                    createLogGroup: true,
                    logGroupRetentionPolicy: LogGroupRetentionPolicy.OneWeek,
                    appendUniqueInstanceGuid: false,
                    appendHostName: false,
                    cloudWatchClient: new AmazonCloudWatchLogsClient(
                        awsSettings.AccessKeyId,
                        awsSettings.SecretAccessKey,
                        RegionEndpoint.GetBySystemName(awsSettings.Region)));
        }
    });

    appBuilder.Services.AddHealthChecks();

    var settings = appBuilder.Configuration.Get<Settings>();

    appBuilder.Services.AddSingleton(settings.App);
    appBuilder.Services.AddSingleton(settings.Geocoding);
    appBuilder.Services.AddSingleton(settings.Database);
    appBuilder.Services.AddSingleton(settings.Stripe);
    appBuilder.Services.AddSingleton(settings.Mail);
    appBuilder.Services.AddSingleton(settings.Aws);
    appBuilder.Services.AddSingleton(settings.Cache);
    appBuilder.Services.AddSingleton(settings.Cap);

    appBuilder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder
                .WithOrigins(settings.App.CorsOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });

    appBuilder.Services
        .AddControllers(options =>
        {
            options.Filters.Add<ExceptionFilter>();

            if (appBuilder.Environment.IsProduction())
            {
                options.Filters.Add<AntiforgeryFilter>();
            }
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        });

    appBuilder.Services.AddAntiforgery(options =>
    {
        options.HeaderName = "X-XSRF-TOKEN"; // as expected from the client
        options.Cookie.Name = "csrf_token"; // set automatically by asp.net as http only
    });

    appBuilder.Services.AddHttpContextAccessor();

    appBuilder.Services.AddAuth(appBuilder.Environment);

    appBuilder.Services.AddMediatR();

    appBuilder.Services.AddSignalR();
    appBuilder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

    appBuilder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

    appBuilder.Services.AddEntityFramework(settings.Database);

    appBuilder.Services.AddDapper();

    appBuilder.Services.AddDateTimeProvider();

    appBuilder.Services.AddStripe(settings.Stripe);

    appBuilder.Services.AddCap(settings);

    appBuilder.Services.AddGeocoding(settings.Geocoding);

    appBuilder.Services.AddHashing();

    appBuilder.Services.AddMail();

    appBuilder.Services.AddImageStorage();

    appBuilder.Services.AddDistributedCache(settings.Cache);

    appBuilder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.AddValidators();
    });

    var app = appBuilder.Build();

    app.UseForwardedHeaders(new ForwardedHeadersOptions()
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    app.UseCors();

    app.UseSerilogRequestLogging();

    app.UseRouting();

    app.UseCookiePolicy(
        new CookiePolicyOptions()
        {
            Secure = app.Environment.IsProduction()
                ? CookieSecurePolicy.Always
                : CookieSecurePolicy.None,
        });

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHub<OrderHub>("/hubs/orders");
    app.MapHealthChecks("/");

    await app.RunAsync();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
