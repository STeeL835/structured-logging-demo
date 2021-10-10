using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace StructuredLoggingDemo.WebApi.Utils
{
    public static class LoggerExtensions
    {
        /// <summary>
        /// A convenience method for adding properties to scope without message (with target-typed <see langword="new"/> in C# 9.0)
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="props"></param>
        /// <returns></returns>
        public static IDisposable BeginScopeWithProps(this ILogger logger, Dictionary<string, object> props)
        {
            return logger.BeginScope(props);
        }
    }
}