using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BCS.Core.Utilities
{
    public static class CacheHelper
    {
        //内存缓存
        private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());


        /// <summary>
        /// 验证缓存项是否存在
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>返回结果（true:存在）</returns>
        public static bool Exists(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            return _cache.TryGetValue(key, out _);
        }


        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <returns>返回结果（true:存在）</returns>
        public static bool Set(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _cache.Set(key, value);

            return Exists(key);
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长</param>
        /// <param name="isSliding">是否滑动过期（如果在过期时间内有操作，则以当前时间点延长过期时间）</param>
        /// <returns>返回结果（true:存在）</returns>
        public static bool Set(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _cache.Set(key, value,
             isSliding
                 ? new MemoryCacheEntryOptions().SetSlidingExpiration(expiresIn)
                 : new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiresIn));


            return Exists(key);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            _cache.Remove(key);

        }

        /// <summary>
        /// 删除匹配到的缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public static void RemoveCacheRegex(string key)
        {
            IList<string> list = SearchCacheRegex(key);
            foreach (var s in list)
            {
                Remove(s);
            }
        }

        /// <summary>
        /// 批量删除缓存
        /// </summary>
        /// <returns></returns>
        public static void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            keys.ToList().ForEach(item => _cache.Remove(item));

        }

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        /// <returns></returns>
        public static void RemoveCacheAll()
        {
            var tmp = GetCacheKeys();
            foreach (var s in tmp)
            {
                Remove(s);
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>返回对应类型</returns>
        public static T Get<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return _cache.Get(key) as T;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>返回object</returns>
        public static object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            return _cache.Get(key);
        }

        /// <summary>
        /// 搜索匹配到的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IList<string> SearchCacheRegex(string key)
        {
            var cacheKeys = GetCacheKeys();
            var tmp = cacheKeys.Where(k => Regex.IsMatch(k, key)).ToList();
            return tmp.AsReadOnly();
        }

        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns>返回所有的缓存键列表</returns>
        public static List<string> GetCacheKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _cache.GetType().GetField("_entries", flags).GetValue(_cache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }

        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <param name="keys">缓存Key集合</param>
        /// <returns>返回字典</returns>
        public static IDictionary<string, object> GetAll(IEnumerable<string> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));

            var dict = new Dictionary<string, object>();
            keys.ToList().ForEach(item => dict.Add(item, _cache.Get(item)));
            return dict;
        }
    }
}