using System;
using System.Text;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;
using NVelocity.Runtime;
using Commons.Collections;
using System.Web;
using System.IO;

namespace ccphl.NVelocity
{
    /// <summary>
    /// NVelocity模板工具类 VelocityHelper
    /// </summary>
    public class VelocityHelper
    {
        private VelocityEngine velocity = null;
        private IContext context = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="templatDir"></param>
        public VelocityHelper(string templatDir)
        {
            Init(templatDir);
        }
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public VelocityHelper() { }
        /// <summary>
        /// 初始话NVelocity模块 虚拟地址
        /// </summary>
        /// <param name="templatDir"></param>
        public void Init(string templatDir)
        {
            //创建VelocityEngine实例对象
            velocity = new VelocityEngine();

            //使用设置初始化VelocityEngine
            ExtendedProperties props = new ExtendedProperties();
            props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, HttpContext.Current.Server.MapPath(templatDir));
            props.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
            props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_CACHE, true); //是否缓存
            props.AddProperty("file.resource.loader.modificationCheckInterval", (Int64)300); //缓存时间(秒)
            velocity.Init(props);

            //为模板变量赋值
            context = new VelocityContext();
        }
        /// <summary>
        /// 初始话NVelocity模块 完整物理地址
        /// </summary>
        /// <param name="templatDir"></param>
        public void Init2(string templatDir)
        {
            //创建VelocityEngine实例对象
            velocity = new VelocityEngine();

            //使用设置初始化VelocityEngine
            ExtendedProperties props = new ExtendedProperties();
            props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, templatDir);
            props.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
            props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_CACHE, true); //是否缓存
            props.AddProperty("file.resource.loader.modificationCheckInterval", (Int64)300); //缓存时间(秒)
            velocity.Init(props);

            //为模板变量赋值
            context = new VelocityContext();
        }
        /// <summary>
        /// 给模板变量赋值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(string key, object value)
        {
            if (context == null)
                context = new VelocityContext();
            context.Put(key, value);
        }
        /// <summary>
        /// 显示模板
        /// </summary>
        /// <param name="templatFileName"></param>
        public void Display(string templatFileName)
        {
            Put("sqlhelper",new BLLHelper());
            //从文件中读取模板
            Template template = velocity.GetTemplate(templatFileName);
            //合并模板
            StringWriter writer = new StringWriter();
            template.Merge(context, writer);
            //输出
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(writer.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 根据模板生成静态页面 虚拟路径
        /// </summary>
        /// <param name="templatFileName"></param>
        /// <param name="htmlpath"></param>
        public void CreateHtml(string templatFileName, string htmlpath)
        {
            Put("sqlhelper",new BLLHelper());
            //从文件中读取模板
            Template template = velocity.GetTemplate(templatFileName);
            //合并模板
            StringWriter writer = new StringWriter();
            template.Merge(context, writer);
            using (StreamWriter write2 = new StreamWriter(HttpContext.Current.Server.MapPath(htmlpath), false, Encoding.UTF8, 200))
            {
                write2.Write(writer);
                write2.Flush();
                write2.Close();
            }
        }
        /// <summary>
        /// 根据模板生成静态页面 完整物理路径
        /// </summary>
        /// <param name="templatFileName"></param>
        /// <param name="htmlpath"></param>
        public void CreateHtml2(string templatFileName, string htmlpath)
        {
            Put("sqlhelper", new BLLHelper());
            //从文件中读取模板
            Template template = velocity.GetTemplate(templatFileName);
            //合并模板
            StringWriter writer = new StringWriter();
            template.Merge(context, writer);
            using (System.IO.FileStream fileStream = new System.IO.FileStream(htmlpath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite))
            {
                using (StreamWriter write2 = new StreamWriter(fileStream, Encoding.UTF8, 200))
                {
                    write2.Write(writer);
                    write2.Flush();
                    write2.Close();
                }
            }
        }
       
    }
}
