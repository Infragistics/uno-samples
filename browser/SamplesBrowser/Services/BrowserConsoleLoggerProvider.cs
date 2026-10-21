using System;
using Microsoft.Extensions.Logging;

namespace SamplesBrowser.Services;

/// <summary>Writes log entries to the browser console (Console.WriteLine maps to console.log there).</summary>
public sealed class BrowserConsoleLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new Logger(categoryName);

    public void Dispose()
    {
    }

    private sealed class Logger(string category) : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            var message = formatter(state, exception);
            if (exception != null)
            {
                message += Environment.NewLine + exception;
            }
            Console.WriteLine($"{Prefix(logLevel)}: {category}: {message}");
        }

        private static string Prefix(LogLevel level) => level switch
        {
            LogLevel.Critical => "crit",
            LogLevel.Error => "fail",
            LogLevel.Warning => "warn",
            LogLevel.Information => "info",
            LogLevel.Debug => "dbug",
            _ => "trce",
        };
    }
}
