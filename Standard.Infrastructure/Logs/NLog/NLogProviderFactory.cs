using Standard.Infrastructure.Logs.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Logs.NLog
{
    public class NLogProviderFactory : ILogProviderFactory
    {
        public NLogProviderFactory()
        {
        }

        public ILogProvider Create(string name)
        {
            return new NLogProvider(name);
        }
    }
}
