using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Http.BatchFormatters;

namespace StructuredLoggingDemo.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
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
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((_, configuration) => ConfigureSerilog(configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        private static LoggerConfiguration ConfigureSerilog(LoggerConfiguration config)
        {
            return config
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)

                    .Enrich.FromLogContext()

                    .WriteTo.Console(
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} | {Message:lj}{NewLine}{Exception}")
                    .WriteTo.DurableHttpUsingTimeRolledBuffers("http://localhost:9001",
                        period: TimeSpan.FromSeconds(2),
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        batchFormatter: new ArrayBatchFormatter(),
                        bufferFileShared: true, // bookmark still locked
                        bufferPathFormat: "logs/logBuffer-{Date}.json");
        }
    }
}
