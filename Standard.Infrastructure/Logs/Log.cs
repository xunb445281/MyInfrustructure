using Standard.Infrastructure.Helpers;
using Standard.Infrastructure.Iocs;
using Standard.Infrastructure.Logs.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Logs
{
    public class Log : ILog
    {
        private ILogProvider logProvider;

        public Log(ILogProvider logProvider)
        {
            this.logProvider = logProvider;
        }

        public static ILog GetLog(string name = "NLog")
        {
            ILogProviderFactory service = Ioc.Container.GetService(typeof(ILogProviderFactory)) as ILogProviderFactory;
            return new Log(service.Create(name));
        }

        public void Debug(string message)
        {
            this.logProvider.WriteLog(Microsoft.Extensions.Logging.LogLevel.Debug, message);
        }

        public void Error(string message)
        {
            this.logProvider.WriteLog(Microsoft.Extensions.Logging.LogLevel.Error, message);
        }


        public void Info(string message)
        {
            this.logProvider.WriteLog(Microsoft.Extensions.Logging.LogLevel.Information, message);
        }

        public void Warn(string message)
        {
            this.logProvider.WriteLog(Microsoft.Extensions.Logging.LogLevel.Warning, message);
        }
    }
}
