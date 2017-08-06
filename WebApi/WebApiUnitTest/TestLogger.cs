using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiUnitTest
{
    public class TestLogger<T> : ILogger<T>
    {
        ConsoleLogger logger = new ConsoleLogger("consoleLogger", (string s, LogLevel logLevel) => { return true; }, true);

        public IDisposable BeginScope<TState>(TState state)
        {
            return logger.BeginScope<TState>(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}
