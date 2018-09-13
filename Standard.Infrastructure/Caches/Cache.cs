using Standard.Infrastructure.Helpers;
using Standard.Infrastructure.Iocs;
using System;

namespace Standard.Infrastructure.Caches
{
    public class Cache
    {
        public Cache()
        {
        }

        public static ICacheProvider GetCache()
        {
            return Ioc.Container.GetService(typeof(ICacheProvider)) as ICacheProvider;
        }
    }
}