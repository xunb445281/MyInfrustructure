using NLog;
using Standard.Infrastructure.Logs.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Logs.NLog
{
    public class NLogProvider : ILogProvider
    {
        private readonly ILogger logger;

        public string LogName
        {
            get;
        }

        public NLogProvider(string logName)
        {
            this.LogName = logName;
            this.logger = LogManager.GetLogger(logName);
        }

        private LogLevel ConvertTo(Microsoft.Extensions.Logging.LogLevel level)
        {
            LogLevel trace;
            switch (level)
            {
                case  Microsoft.Extensions.Logging.LogLevel.Trace:
                    {
                        trace = LogLevel.Trace;
                        break;
                    }
                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    {
                        trace = LogLevel.Debug;
                        break;
                    }
                case Microsoft.Extensions.Logging.LogLevel.Information:
                    {
                        trace = LogLevel.Info;
                        break;
                    }
                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    {
                        trace = LogLevel.Warn;
                        break;
                    }
                case Microsoft.Extensions.Logging.LogLevel.Error:
                    {
                        trace = LogLevel.Error;
                        break;
                    }
                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    {
                        trace = LogLevel.Fatal;
                        break;
                    }
                default:
                    {
                        trace = LogLevel.Off;
                        break;
                    }
            }
            return trace;
        }

        public void WriteLog(Microsoft.Extensions.Logging.LogLevel level, string message)
        {
            this.logger.Log(this.ConvertTo(level), message);
        }
    }
}
