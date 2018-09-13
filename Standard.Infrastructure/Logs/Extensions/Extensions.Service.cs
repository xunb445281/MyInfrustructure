using Microsoft.Extensions.DependencyInjection;
using Standard.Infrastructure.Logs.Abstracts;
using Standard.Infrastructure.Logs.NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Infrastructure.Logs.Extensions
{
    public static class Extensions
    {
        public static void AddNLog(this IServiceCollection services)
        {
            services.AddScoped<ILogProviderFactory, NLogProviderFactory>();
            services.AddScoped<ILog, Log>();
        }
    }
}
