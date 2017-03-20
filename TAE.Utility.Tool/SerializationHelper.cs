using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TAE.Utility.Tool
{
    [Serializable]
    public class SerializationHelper
    {
        private SerializationHelper() { }

        #region ===================XmlSerializer=====================
        /// <summary>
        /// 序列化,使用标准的XmlSerialize,优先考虑
        /// 不能序列化IDictionary接口
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static void XmlSerialize(object obj, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        /// <summary>
        /// 反序列化,使用标准的XmlSerialize,优先考虑
        /// 不能序列化IDictionary接口
        /// </summary>
        /// <param name="type">对象</param>
        /// <param name="filename">文件路径</param>
        /// <returns>type类型的对象实例</returns>
        public static object XmlDeserializeFromFile(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }
        public static string XmlSerialize(object obj, Encoding encoding)
        {
            if (obj == null)
                return null;
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            StreamWriter sWriter = new StreamWriter(stream, encoding);
            XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
            xsn.Add(String.Empty, String.Empty);
            serializer.Serialize(sWriter, obj, xsn);
            string str = encoding.GetString(stream.ToArray());
            stream.Close();
            return str;
        }
        public static string XmlSerialize(object obj)
        {
            if (obj == null)
                return null;
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            StringWriter sWriter = new StringWriter();
            serializer.Serialize(sWriter, obj);
            return sWriter.ToString();
        }
        public static object XmlDeserialize(Type type, string xmlStr)
        {
            if (xmlStr == null || xmlStr == "")
                return null;
            XmlSerializer serializer = new XmlSerializer(type);
            StringReader sReader = new StringReader(xmlStr);
            return serializer.Deserialize(sReader);
        }
        #endregion
        /// <summary>
        /// 将C#数据实体转化为JSON数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">要转化的数据实体</param>
        /// <returns>JSON格式字符串</returns>
        public static string Jsonserializa<T>(T obj)
        {
            return Jsonserializa(obj, Encoding.UTF8);
        }
        /// <summary>
        /// 将C#数据实体转化为JSON数据
        /// </summary>
        /// <param name="obj">要转化的数据实体</param>
        /// <param name="encoding"></param>
        /// <returns>JSON格式字符串</returns>
        public static string Jsonserializa(object obj, Encoding encoding)
        {
            if (obj == null)
                return null;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms, encoding);
            string resultStr = sr.ReadToEnd();
            sr.Close();
            ms.Close();
            return resultStr;
        }



    }
}
