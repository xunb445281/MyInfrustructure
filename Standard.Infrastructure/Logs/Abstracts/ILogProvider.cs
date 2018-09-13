using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Logs.Abstracts
{
    public interface ILogProvider
    {
        string LogName
        {
            get;
        }

        void WriteLog(LogLevel level, string message);
    }
}
