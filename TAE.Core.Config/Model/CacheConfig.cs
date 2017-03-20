using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TAE.Core.Config
{
    [Serializable]
    public class CacheConfig : ConfigFileBase
    {
        public CacheConfig()
        {
        }
        /// <summary>
        /// 配置缓存实体
        /// </summary>
        public CacheConfigItem[] CacheConfigItems { get; set; }
        /// <summary>
        /// 缓存提供者项
        /// </summary>
        public CacheProviderItem[] CacheProviderItems { get; set; }

    }
    /// <summary>
    /// 缓存提供者项
    /// </summary>
    public class CacheProviderItem : ConfigNodeBase
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
    }
    /// <summary>
    /// 配置缓存项
    /// </summary>
    public class CacheConfigItem : ConfigNodeBase
    {
        [XmlAttribute(AttributeName = "keyRegex")]
        public string KeyRegex { get; set; }

        [XmlAttribute(AttributeName = "moduleRegex")]
        public string ModuleRegex { get; set; }

        [XmlAttribute(AttributeName = "providerName")]
        public string ProviderName { get; set; }

        [XmlAttribute(AttributeName = "minitus")]
        public int Minitus { get; set; }

        [XmlAttribute(AttributeName = "priority")]
        public int Priority { get; set; }

        [XmlAttribute(AttributeName = "isAbsoluteExpiration")]
        public bool IsAbsoluteExpiration { get; set; }

        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; }
    }
}
