using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Core.Config
{
    public class FileConfigService : IConfigService
    {
        private readonly string configFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetConfig(string name)
        {
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);
            var configPath = GetFilePath(name);
            if (!File.Exists(configPath))
                return null;
            else
                return File.ReadAllText(configPath);
        }
        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Content"></param>
        public void SaveConfig(string name, string Content)
        {
            var configPath = GetFilePath(name);
            File.WriteAllText(configPath, Content);
        }
        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetFilePath(string FileName)
        {
            var configPath = string.Format(@"{0}\{1}.xml", configFolder, FileName);
            return configPath;
        }
    }
}
