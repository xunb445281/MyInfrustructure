using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Logs.Abstracts
{
    public interface ILogProviderFactory
    {
        ILogProvider Create(string name);
    }
}
