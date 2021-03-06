using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Standard.Infrastructure.Caches
{
    public interface ICacheProvider : IDisposable
    {
        #region 键值

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool HasKey(string key);

        /// <summary>
        /// 删除key
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// 删除keys
        /// </summary>
        /// <param name="keys"></param>
        void Remove(List<string> keys);

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 异步获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);

        /// <summary>
        /// 异步获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetAsync(string key);

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        void Set<T>(string key, T data);

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime">秒数</param>
        void Set<T>(string key, T data, double cacheTime);

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expiresAt"></param>
        void Set<T>(string key, T data, DateTime expiresAt);

        /// <summary>
        /// 异步设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T data, double cacheTime = 0);

        #endregion

        #region 集合

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        List<string> ListGet(string key);

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        List<T> ListGet<T>(string key);

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void ListAdd<T>(string key, T value);

        /// <summary>
        /// 添加集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        void ListAdd<T>(string key, List<T> values);

        /// <summary>
        /// 设置集合
        /// </summary>
        /// <param name="listId"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        void ListSet<T>(string key, int index, T value);

        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void ListRemove<T>(string key, T value);

        #endregion

        #region 哈希列表

        /// <summary>
        /// 获取哈希列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IDictionary<string, string> HashGet(string key);

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        T HashGet<T>(string key, string name);


        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        string HashGet(string key, string name);

        /// <summary>
        /// 设置哈希列值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void HashSet<T>(string key, string name, T value);

        #endregion

        #region 锁


        /// <summary>
        /// 获取锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        /// <returns></returns>
        bool TakeLock(string key, string data, double cacheTime);

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        bool LockRelease(string key, string data);

        #endregion

    }
}