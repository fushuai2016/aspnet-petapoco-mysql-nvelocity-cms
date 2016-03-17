using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ccphl.Common
{
    public partial class XmlConfig<T> where T : new()
    {
        private static object lockHelper = new object();

        /// <summary>
        ///  读取配置文件
        /// </summary>
        public T loadConfig(string configFilePath)
        {
            return (T)SerializationHelper.Load(typeof(T), configFilePath);
        }

        /// <summary>
        /// 写入配置文件
        /// </summary>
        public T saveConifg(T model, string configFilePath)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(model, configFilePath);
            }
            return model;
        }
        /// <summary>
        ///  读取List配置文件
        /// </summary>
        public List<T> loadListConfig(string configFilePath)
        {
            return (List<T>)SerializationHelper.Load(typeof(List<T>), configFilePath);
        }

        /// <summary>
        /// 写入List配置文件
        /// </summary>
        public List<T> saveListConifg(List<T> list, string configFilePath)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(list, configFilePath);
            }
            return list;
        }
        /// <summary>
        ///  读取图片配置文件
        /// </summary>
        public List<T> loadImgListConfig()
        {
            List<T> list = CacheHelper.GetList<T>(DTKeys.CACHE_IMG_CONFIG);
            if (list==null)
            {
                CacheHelper.Insert(DTKeys.CACHE_IMG_CONFIG, loadListConfig(Utils.GetXmlMapPath(DTKeys.FILE_IMG_XML_CONFING)),
                    Utils.GetXmlMapPath(DTKeys.FILE_IMG_XML_CONFING));
                list = CacheHelper.GetList<T>(DTKeys.CACHE_IMG_CONFIG);
            }
            return list;
        }
        /// <summary>
        ///  保存图片配置文件
        /// </summary>
        public List<T> saveImgListConifg(List<T> list)
        {
            return saveListConifg(list, Utils.GetXmlMapPath(DTKeys.FILE_IMG_XML_CONFING));
        }
        /// <summary>
        ///  读取系统配置文件
        /// </summary>
        public T loadSystemConfig()
        {
            T model = CacheHelper.Get<T>(DTKeys.CACHE_SYSTEM_CONFIG);
            if (model == null)
            {
                CacheHelper.Insert(DTKeys.CACHE_SYSTEM_CONFIG, loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SYSTEM_XML_CONFING)),
                    Utils.GetXmlMapPath(DTKeys.FILE_SYSTEM_XML_CONFING));
                model = CacheHelper.Get<T>(DTKeys.CACHE_SYSTEM_CONFIG);
            }
            return model;
        }

        /// <summary>
        ///  保存系统配置文件
        /// </summary>
        public T saveSystemConifg(T model)
        {
            return saveConifg(model, Utils.GetXmlMapPath(DTKeys.FILE_SYSTEM_XML_CONFING));
        }

        /// <summary>
        ///  读取订单配置文件
        /// </summary>
        public T loadOrderConfig()
        {
            T model = CacheHelper.Get<T>(DTKeys.CACHE_ORDER_CONFIG);
            if (model == null)
            {
                CacheHelper.Insert(DTKeys.CACHE_ORDER_CONFIG, loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_ORDER_XML_CONFING)),
                    Utils.GetXmlMapPath(DTKeys.FILE_ORDER_XML_CONFING));
                model = CacheHelper.Get<T>(DTKeys.CACHE_ORDER_CONFIG);
            }
            return model;
        }

        /// <summary>
        ///  保存订单配置文件
        /// </summary>
        public T saveOrderConifg(T model)
        {
            return saveConifg(model, Utils.GetXmlMapPath(DTKeys.FILE_ORDER_XML_CONFING));
        }
    }
}
