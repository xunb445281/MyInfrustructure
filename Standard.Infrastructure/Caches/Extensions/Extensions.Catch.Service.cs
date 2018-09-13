using Microsoft.Extensions.DependencyInjection;
using Standard.Infrastructure.Caches.Redis;
using System;
using System.Runtime.CompilerServices;

namespace Standard.Infrastructure.Caches.Extensions
{
    public static class Extensions
    {
        public static void AddRedis(this IServiceCollection services)
        {
            services.AddSingleton<ICacheProvider, RedisCacheProvider>();
        }
    }
}