using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Http.BatchFormatters;

namespace StructuredLoggingDemo.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = ConfigureSerilog(new LoggerConfiguration()).CreateLogger();

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
                .UseSerilog((context, configuration) => ConfigureSerilog(configuration))
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
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} | {Message:lj}{NewLine}{Exception}")
                .WriteTo.DurableHttpUsingTimeRolledBuffers("http://localhost:9001",
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    batchFormatter: new ArrayBatchFormatter(),
                    bufferPathFormat: "logs/logBuffer-{Date}.json");
        }
    }
}
