using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace TAE.Core.Config
{
    using TAE.Core.Config;
    using TAE.Utility.Tool;
    public class CachedConfigContext : ConfigContext
    {
        public static CachedConfigContext Current = new CachedConfigContext();
        /// <summary>
        /// 重写基类取配置文件,加入缓存
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public override T Get<T>(string index = null)
        {
            var fileName = this.GetConfigFileName<T>(index);
            var key = "ConfigFile_" + fileName;
            var context = Caching.Get(key);
            if (context != null)
                return (T)context;
            var value = base.Get<T>(index);
            Caching.Set(key, value, new CacheDependency(ConfigService.GetFilePath(fileName)));
            return value;
        }
        /// <summary>
        /// 获取缓存配置
        /// </summary>
        public CacheConfig CacheConfig
        {
            get
            {
                return this.Get<CacheConfig>();
            }
        }
        public DaoConfig DaoConfig
        {
            get
            {
                return this.Get<DaoConfig>();
            }
        }
    }
}
