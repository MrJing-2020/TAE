using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAE.Core.Config
{
    public interface IConfigService
    {
        /// <summary>
        /// 获取配置文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetConfig(string name);
        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Content"></param>
        void SaveConfig(string name, string Content);
        /// <summary>
        /// 获取文件地址
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetFilePath(string name);
    }
}
