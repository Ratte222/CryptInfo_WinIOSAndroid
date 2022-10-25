using AuxiliaryLib.Extensions;
using MauiCryptApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Helpers
{
    public class MockLoggerForSynchronize : ILogger<Backuper_Core.Services.BackupJob>
    {
        public readonly List<LogModel> _logs;

        public MockLoggerForSynchronize(List<LogModel> logs)
        {
            _logs = logs;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _logs.Add(new LogModel() { Level = logLevel.ToString(), Description = formatter.Invoke(state, exception) + " \r\n " + exception.Serialize() });
        }
    }
}
