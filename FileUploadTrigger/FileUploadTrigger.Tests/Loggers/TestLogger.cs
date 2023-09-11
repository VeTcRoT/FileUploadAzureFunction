using Microsoft.Extensions.Logging.Abstractions.Internal;
using Microsoft.Extensions.Logging;

namespace FileUploadTrigger.Tests.Loggers
{
    public class TestLogger<T> : ILogger<T>
    {
        private readonly List<string> logMessages = new List<string>();

        public IEnumerable<string> LogMessages => logMessages;

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            logMessages.Add(message);
        }
    }
}
