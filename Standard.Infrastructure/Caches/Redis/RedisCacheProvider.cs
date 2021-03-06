using StackExchange.Redis;
using Standard.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Standard.Infrastructure.Caches.Redis
{
    public class RedisCacheProvider : ICacheProvider
    {

        ConnectionMultiplexer redisManager;
        RedisOptions options;


        public RedisCacheProvider(RedisOptions options)
        {
            this.options = options;
            string connectStr = $"{this.options.Host}:{this.options.Port},password={this.options.Password}";
            redisManager = ConnectionMultiplexer.Connect(connectStr);

        }

        /// <summary>
        /// 记录异常
        /// </summary>
        private void LogError(Exception ex)
        {
            //Log.GetLog().Error($"redis异常，原因:{ex.ToWarning().Message}");
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private string Serialize<T>(T data)
        {
            string value = string.Empty;

            if (data is string)
                value = data.ToString();
            else
                value = JsonHelper.ToJson(data);
            return value;
        }

        #region 键值

        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasKey(string key)
        {
            try
            {
                return redisManager.GetDatabase().KeyExists(key);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            try
            {
                redisManager.GetDatabase().KeyDelete(key);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keys"></param>
        public void Remove(List<string> keys)
        {
            try
            {
                var db = redisManager.GetDatabase();
                foreach (var key in keys)
                {
                    db.KeyDelete(key);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            try
            {
                string json = redisManager.GetDatabase().StringGet(key);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return default(T);
                }
                return JsonHelper.FromJson<T>(json);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return default(T);
        }

        /// <summary>
        /// 异步获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                string json = await redisManager.GetDatabase().StringGetAsync(key);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return default(T);
                }
                return JsonHelper.FromJson<T>(json);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return default(T);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            try
            {
                return redisManager.GetDatabase().StringGet(key);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return "";
        }


        /// <summary>
        /// 异步获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string key)
        {
            try
            {
                return await redisManager.GetDatabase().StringGetAsync(key);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return "";
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void Set<T>(string key, T data)
        {
            Set<T>(key, data, 0);
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expiresAt"></param>
        public void Set<T>(string key, T data, DateTime expiresAt)
        {
            TimeSpan span = expiresAt - DateTime.Now;
            Set<T>(key, data, span.TotalSeconds);
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public void Set<T>(string key, T data, double cacheTime)
        {
            try
            {
                string value = Serialize<T>(data);

                if (cacheTime > 0)
                {
                    var span = TimeSpan.FromSeconds(cacheTime);
                    redisManager.GetDatabase().StringSet(key, value, span);
                }
                else
                {
                    redisManager.GetDatabase().StringSet(key, value);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

        }

        /// <summary>
        /// 异步设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        /// <returns></returns>
        public async Task SetAsync<T>(string key, T data, double cacheTime = 0)
        {
            try
            {
                string value = Serialize<T>(data);

                if (cacheTime > 0)
                {
                    var span = TimeSpan.FromSeconds(cacheTime);
                    await redisManager.GetDatabase().StringSetAsync(key, value, span);
                }
                else
                {
                    await redisManager.GetDatabase().StringSetAsync(key, value);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        #endregion

        #region 集合

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string> ListGet(string key)
        {
            try
            {
                return redisManager.GetDatabase().ListRange(key).ToStringArray().ToList();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return new List<string>();
        }

        /// <summary>
        /// 获取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> ListGet<T>(string key)
        {
            List<T> list = new List<T>();
            try
            {
                var values = redisManager.GetDatabase().ListRange(key).ToStringArray().ToList();

                foreach (var value in values)
                {
                    list.Add(JsonHelper.FromJson<T>(value));
                }
                return list;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return new List<T>();
        }

        /// <summary>
        /// 添加到集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void ListAdd<T>(string key, T value)
        {
            List<T> list = new List<T>();
            list.Add(value);
            ListAdd<T>(key, list);
        }

        /// <summary>
        /// 添加到集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public void ListAdd<T>(string key, List<T> values)
        {
            try
            {
                var db = redisManager.GetDatabase();
                foreach (var value in values)
                {
                    var result = Serialize<T>(value);
                    db.ListLeftPush(key, result);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        /// <summary>
        /// 设置集合值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void ListSet<T>(string key, int index, T value)
        {
            try
            {
                var result = Serialize<T>(value);
                redisManager.GetDatabase().ListSetByIndex(key, index, result);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public void ListRemove<T>(string key, T value)
        {
            try
            {
                var result = Serialize<T>(value);
                redisManager.GetDatabase().ListRemove(key, result);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        #endregion

        #region 哈希表

        /// <summary>
        /// 获取哈希列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IDictionary<string, string> HashGet(string key)
        {
            try
            {
                return redisManager.GetDatabase().HashGetAll(key).ToStringDictionary();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// 获取哈希列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T HashGet<T>(string key, string name)
        {
            try
            {
                var result = redisManager.GetDatabase().HashGet(key, name).ToString();
                if (string.IsNullOrWhiteSpace(result))
                {
                    return default(T);
                }
                return JsonHelper.FromJson<T>(result);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return default(T);
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string HashGet(string key, string name)
        {
            try
            {
                var result = redisManager.GetDatabase().HashGet(key, name).ToString();
                return result;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return "";
        }

        /// <summary>
        /// 设置哈希列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void HashSet<T>(string key, string name, T value)
        {
            try
            {
                var result = Serialize<T>(value);
                redisManager.GetDatabase().HashSet(key, name, result);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }


        #endregion

        public void Dispose()
        {
            if (redisManager != null)
                redisManager.Close();
        }

        #region 锁
        /// <summary>
        /// 获取锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        /// <returns></returns>
        public bool TakeLock(string key, string data, double cacheTime)
        {
            try
            {
                if (cacheTime > 0)
                {
                    var span = TimeSpan.FromSeconds(cacheTime);
                    return redisManager.GetDatabase().LockTake(key, data, span);
                }
                else
                {
                    return redisManager.GetDatabase().StringSet(key, data);
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return false;
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool LockRelease(string key, string data)
        {
            try
            {
                return redisManager.GetDatabase().LockRelease(key, data);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return false;
        }
        #endregion

    }
}