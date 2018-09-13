using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Logs
{
    public interface ILog
    {
        void Debug(string message);

        void Error(string message);

        void Info(string message);

        void Warn(string message);
    }
}
