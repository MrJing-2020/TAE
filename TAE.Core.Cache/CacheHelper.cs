using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace TAE.Core.Cache
{
    /// <summary>
    /// 缓存帮助类
    /// </summary>
    public class CacheHelper
    {
        public static object Get(string key)
        {
            var cacheConfig = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);
            return cacheConfig.CacheProvider.Get(key);
        }
        public static void Set(string key, object value)
        {
            var cacheConfig = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);
            cacheConfig.CacheProvider.Set(key, value, cacheConfig.CacheConfigItem.Minitus, cacheConfig.CacheConfigItem.IsAbsoluteExpiration, null);
        }

        public static void Set(string key, object value, int minitus)
        {
            var cacheConfig = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);
            cacheConfig.CacheProvider.Set(key, value, minitus, cacheConfig.CacheConfigItem.IsAbsoluteExpiration, null);
        }

        public static void Remove(string key)
        {
            var cacheConfig = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);
            cacheConfig.CacheProvider.Remove(key);
        }
        public static void Clear(string keyRegex = ".*", string moduleRegex = ".*")
        {
            if (!Regex.IsMatch(CacheConfigContext.ModuleName, moduleRegex, RegexOptions.IgnoreCase))
                return;
            foreach (var cacheProiders in CacheConfigContext.CacheProvider.Values)
                cacheProiders.Clear(keyRegex);
        }
        /// <summary>
        /// 如果缓存里没有,则取数据然后缓存起来
        /// 调用GetItem方法（如果HttpContext.Current.Items中缓存有数据，则直接取HttpContext.Current.Items中的数据）
        /// 否则取 HttpRuntime.Cache中的数据，然后缓存到HttpContext.Current.Items中
        /// </summary>
        /// <typeparam name="F"></typeparam>
        /// <param name="key"></param>
        /// <param name="getRealData"></param>
        /// <returns></returns>
        public static F Get<F>(string key, Func<F> getRealData)
        {
            var getDataFromCache = new Func<F>(() =>
            {
                F data = default(F);
                try
                {
                    var cacheData = Get(key);
                    if (cacheData == null)
                    {
                        data = getRealData();
                        if (data != null)
                            Set(key, data);
                    }
                    else
                    {
                        data = (F)cacheData;
                    }
                }
                catch (Exception)
                {
                    return getRealData();
                }
                return data;
            });
            return GetItem<F>(key, getDataFromCache);
        }
        public static F Get<F>(string key, int id, Func<int, F> getRealData)
        {
            return Get<F, int>(key, id, getRealData);
        }
        public static F Get<F>(string key, string id, Func<string, F> getRealData)
        {
            return Get<F, string>(key, id, getRealData);
        }
        public static F Get<F>(string key, string branchKey, Func<F> getRealData)
        {
            return Get<F, string>(key, branchKey, id => getRealData());
        }
        public static F Get<F, T>(string key, T id, Func<T, F> getRealData)
        {
            key = string.Format("{0}_{1}", key, id);
            var getDataFromCache = new Func<F>(() =>
            {
                F data = default(F);
                var cacheData = Get(key);
                if (cacheData == null)
                {
                    data = getRealData(id);
                    if (data != null)
                        Set(key, data);
                }
                else
                {
                    data = (F)cacheData;
                }
                return data;
            });
            return GetItem<F>(key, getDataFromCache);
        }

        #region 以下几个方法从HttpContext.Items缓存页面数据，适合页面生命周期，页面载入后就被移除，而非HttpRuntime.Cache在整个应用程序都有效

        /// <summary>
        /// 获取HttpContext.Items缓存的数据
        /// </summary>
        /// <typeparam name="F"></typeparam>
        /// <param name="name"></param>
        /// <param name="getRealData">待返回类型的委托</param>
        /// <returns></returns>
        public static F GetItem<F>(string name, Func<F> getRealData)
        {
            if (HttpContext.Current == null)
                return getRealData();
            var httpContextItems = HttpContext.Current.Items;
            if (httpContextItems.Contains(name))
            {
                return (F)httpContextItems[name];
            }
            else
            {
                //var data = getRealData();
                //if (data != null)
                //    httpContextItems[name] = data;
                httpContextItems[name] = getRealData();
                return (F)httpContextItems[name];
            }
        }
        public static F GetItem<F>() where F : new()
        {
            return GetItem<F>(typeof(F).ToString(), () => new F());
        }
        public static F GetItem<F>(Func<F> getRealData)
        {
            return GetItem<F>(typeof(F).ToString(), getRealData);
        }

        #endregion
    }
}
