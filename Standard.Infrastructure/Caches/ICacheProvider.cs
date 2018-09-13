using System;
using System.Collections.Generic;

namespace Standard.Infrastructure.Caches
{
    public interface ICacheProvider
    {
        void Add<T>(string key, T data);

        void Add<T>(string key, T data, int cacheTime);

        void Add<T>(string key, T data, DateTime expiresAt);

        T Get<T>(string key);

        bool HasKey(string key);

        void Remove(string key);

        void RemoveAll();

        void Replace<T>(string key, T data);

        void Replace<T>(string key, T data, int cacheTime);

        void Replace<T>(string key, T data, DateTime expiresAt);

        IList<string> SearchKeys(string pattern);

        void Set<T>(string key, T data);

        void Set<T>(string key, T data, int cacheTime);

        void Set<T>(string key, T data, DateTime expiresAt);
    }
}