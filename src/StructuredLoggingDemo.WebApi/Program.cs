using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Web;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Http.BatchFormatters;

namespace StructuredLoggingDemo.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if NLOG
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Info("Starting web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                logger.Fatal(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
#elif SERILOG
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger()
                .ForContext<Program>();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
#endif
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
#if NLOG
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog()
#elif SERILOG
                .UseSerilog((_, configuration) => ConfigureSerilog(configuration))
#endif
                ;


#if SERILOG
        private static LoggerConfiguration ConfigureSerilog(LoggerConfiguration config)
        {
            return config
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)

                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Logger", "Serilog")

                    .WriteTo.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} | {Message:lj}{NewLine}{Exception}")
                    .WriteTo.DurableHttpUsingTimeRolledBuffers("http://localhost:9001",
                        period: TimeSpan.FromSeconds(2),
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        batchFormatter: new ArrayBatchFormatter(),
                        bufferFileShared: true, // bookmark still locked
                        bufferPathFormat: "logs/logBuffer-{Date}.json");
        }
#endif
    }
}
