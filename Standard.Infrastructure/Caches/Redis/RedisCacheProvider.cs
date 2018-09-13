using ServiceStack.Caching;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;

namespace Standard.Infrastructure.Caches.Redis
{
    public class RedisCacheProvider : ICacheProvider
    {
        private RedisManagerPool redisManager;

        private readonly string connectString = "{0}@{1}:{2}";

        public RedisCacheProvider()
        {
            RedisPoolConfig redisPoolConfig = new RedisPoolConfig();
            this.redisManager = new RedisManagerPool(String.Format(this.connectString, (object)"zhangh", "128.0.100.118", "6999"));
        }

        public void Add<T>(string key, T data)
        {
            using (IRedisClient client = this.redisManager.GetClient())
            {
                client.Add<T>(key, data);
            }
        }

        public void Add<T>(string key, T data, int cacheTime)
        {
            DateTime now = DateTime.Now;
            this.Add<T>(key, data, now.AddMinutes((double)cacheTime));
        }

        public void Add<T>(string key, T data, DateTime expiresAt)
        {
            using (IRedisClient client = this.redisManager.GetClient())
            {
                client.Add<T>(key, data, expiresAt);
            }
        }

        public T Get<T>(string key)
        {
            T t;
            using (IRedisClient client = this.redisManager.GetClient())
            {
                t = client.Get<T>(key);
            }
            return t;
        }

        public bool HasKey(string key)
        {
            bool flag;
            using (IRedisClient client = this.redisManager.GetClient())
            {
                flag = client.ContainsKey(key);
            }
            return flag;
        }

        public void Remove(string key)
        {
            using (IRedisClient client = this.redisManager.GetClient())
            {
                client.Remove(key);
            }
        }

        public void RemoveAll()
        {
            using (IRedisClient client = this.redisManager.GetClient())
            {
                client.FlushDb();
            }
        }

        public void Replace<T>(string key, T data)
        {
            using (IRedisClient client = this.redisManager.GetClient())
            {
                client.Replace<T>(key, data);
            }
        }

        public void Replace<T>(string key, T data, int cacheTime)
        {
            DateTime now = DateTime.Now;
            this.Replace<T>(key, data, now.AddMinutes((double)cacheTime));
        }

        public void Replace<T>(string key, T data, DateTime expiresAt)
        {
            using (IRedisClient client = this.redisManager.GetClient())
            {
                client.Replace<T>(key, data, expiresAt);
            }
        }

        public IList<string> SearchKeys(string pattern)
        {
            IList<string> strs;
            using (IRedisClient client = this.redisManager.GetClient())
            {
                strs = client.SearchKeys(pattern);
            }
            return strs;
        }

        public void Set<T>(string key, T data)
        {
            using (IRedisClient client = this.redisManager.GetClient())
            {
                client.Set<T>(key, data);
            }
        }

        public void Set<T>(string key, T data, int cacheTime)
        {
            DateTime now = DateTime.Now;
            this.Set<T>(key, data, now.AddMinutes((double)cacheTime));
        }

        public void Set<T>(string key, T data, DateTime expiresAt)
        {
            using (IRedisClient client = this.redisManager.GetClient())
            {
                client.Set<T>(key, data, expiresAt);
            }
        }
    }
}