using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Standard.Infrastructure.Caches.Redis;
using System;
using System.Runtime.CompilerServices;

namespace Standard.Infrastructure.Caches.Extensions
{
    /// <summary>
    /// ע��redis��ط���
    /// </summary>
    public static class Extensions
    {
        public static void AddRedis(this IServiceCollection services,Action<RedisOptions> action)
        {
            services.Configure(action);
            RedisOptions options = new RedisOptions();
            action(options);
            services.AddSingleton(options);
            services.AddSingleton<ICacheProvider, RedisCacheProvider>();
        }
    }
}