using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Core.Config
{
    using TAE.Utility.Tool;
    public class ConfigContext
    {
        public IConfigService ConfigService { get; set; }

        public ConfigContext() : this(new FileConfigService()) { }

        public ConfigContext(IConfigService pageContentConfigService)
        {
            this.ConfigService = pageContentConfigService;
        }

        /// <summary>
        /// 获取T类型对应xml配置文件，获取到后序列号成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual T Get<T>(string index = null) where T : ConfigFileBase, new()
        {
            var result = new T();
            this.VilidateClusteredByIndex(result, index);
            result = this.GetConfigFile<T>(index);
            return result;
        }

        private T GetConfigFile<T>(string index = null) where T : ConfigFileBase, new()
        {
            var result = new T();
            var fileName = this.GetConfigFileName<T>(index);
            var content = this.ConfigService.GetConfig(fileName);
            if (content == null)
            {
                this.ConfigService.SaveConfig(fileName, string.Empty);
            }
            else if (!string.IsNullOrEmpty(content))
            {
                try
                {
                    result = (T)SerializationHelper.XmlDeserialize(typeof(T), content);
                }
                catch
                {
                    result = new T();
                }
            }
            return result;
        }

        public virtual void VilidateClusteredByIndex<T>(T configFile, string index) where T : ConfigFileBase
        {

        }

        /// <summary>
        /// 获取配置文件名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual string GetConfigFileName<T>(string index = null)
        {
            var fileName = typeof(T).Name;
            if (!string.IsNullOrEmpty(index))
                fileName = string.Format("{0}_{1}", fileName, index);
            return fileName;
        }
    }
}
