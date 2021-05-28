using System;
using Amazon;
using Amazon.CloudWatchLogs;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.AwsCloudWatch;

namespace Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            Log.Information("Starting web host");

            try
            {
                CreateHostBuilder(args).Build().Run();
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
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog((context, services, configuration) =>
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
                })
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                });
    }
}
