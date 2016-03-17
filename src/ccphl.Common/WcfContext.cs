using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.IO;

namespace ClassLibrary
{

    /// <summary>
    /// 动态调用WCF的工具类库
    /// </summary>
    public class WcfContext
    {
        /// <summary>
        /// wcf通讯协议标准绑定类型枚举
        /// </summary>
        public enum BindingEnum
        {
            /// <summary>
            /// 基本绑定
            /// </summary>
            BasicHttpBinding,
            /// <summary>
            /// TCP绑定
            /// </summary>
            NetTcpBinding,
            /// <summary>
            /// 对等网绑定
            /// </summary>
            NetPeerTcpBinding,
            /// <summary>
            /// IPC绑定
            /// </summary>
            NetNamedPipeBinding,
            /// <summary>
            /// web绑定
            /// </summary>
            WSHttpBinding,
            /// <summary>
            /// web联邦绑定
            /// </summary>
            WSFederationHttpBinding,
            /// <summary>
            /// web双向绑定
            /// </summary>
            WSDualHttpBinding

        }

        #region Wcf服务工厂
        /// <summary>
        /// 创建wcf 服务
        /// </summary>
        /// <typeparam name="T">服务接口</typeparam>
        /// <param name="url">wcf服务地址</param>
        /// <returns></returns>
        public static T CreateWCFServiceByURL<T>(string url)
        {
            return CreateWCFServiceByURL<T>(url, BindingEnum.WSHttpBinding);
        }

        /// <summary>
        /// 创建wcf 服务
        /// </summary>
        /// <typeparam name="T">服务接口</typeparam>
        /// <param name="url">wcf服务地址</param>
        /// <param name="bing">通讯协议类型</param>
        /// <returns></returns>
        public static T CreateWCFServiceByURL<T>(string url, BindingEnum bing)
        {
            if (string.IsNullOrEmpty(url)) throw new NotSupportedException("this url isn`t Null or Empty!");
            EndpointAddress address = new EndpointAddress(url);
            Binding binding = CreateBinding(bing);
            ChannelFactory<T> factory = new ChannelFactory<T>(binding, address);
            return factory.CreateChannel();
        }
        #endregion

        #region 创建传输协议
        /// <summary>
        /// 创建传输协议
        /// </summary>
        /// <param name="binding">传输协议名称</param>
        /// <returns></returns>
        private static Binding CreateBinding(BindingEnum binding)
        {
            Binding bindinginstance = null;
            if (binding == BindingEnum.BasicHttpBinding)
            {
                BasicHttpBinding ws = new BasicHttpBinding();
                ws.MaxBufferSize = 2147483647;
                ws.MaxBufferPoolSize = 2147483647;
                ws.MaxReceivedMessageSize = 2147483647;
                ws.ReaderQuotas.MaxStringContentLength = 2147483647;
                ws.CloseTimeout = new TimeSpan(0, 10, 0);
                ws.OpenTimeout = new TimeSpan(0, 10, 0);
                ws.ReceiveTimeout = new TimeSpan(0, 10, 0);
                ws.SendTimeout = new TimeSpan(0, 10, 0);

                bindinginstance = ws;
            }
            else if (binding == BindingEnum.NetNamedPipeBinding)
            {
                NetNamedPipeBinding ws = new NetNamedPipeBinding();
                ws.MaxReceivedMessageSize = 65535000;
                bindinginstance = ws;
            }
            else if (binding == BindingEnum.NetPeerTcpBinding)
            {
                NetPeerTcpBinding ws = new NetPeerTcpBinding();
                ws.MaxReceivedMessageSize = 65535000;
                bindinginstance = ws;
            }
            else if (binding == BindingEnum.NetTcpBinding)
            {
                NetTcpBinding ws = new NetTcpBinding();
                ws.MaxReceivedMessageSize = 65535000;
                ws.Security.Mode = SecurityMode.None;
                bindinginstance = ws;
            }
            else if (binding == BindingEnum.WSDualHttpBinding)
            {
                WSDualHttpBinding ws = new WSDualHttpBinding();
                ws.MaxReceivedMessageSize = 65535000;

                bindinginstance = ws;
            }
            else if (binding == BindingEnum.WSFederationHttpBinding)
            {
                WSFederationHttpBinding ws = new WSFederationHttpBinding();
                ws.MaxReceivedMessageSize = 65535000;
                bindinginstance = ws;
            }
            else if (binding == BindingEnum.WSHttpBinding)
            {
                WSHttpBinding ws = new WSHttpBinding(SecurityMode.None);
                ws.MaxReceivedMessageSize = 65535000;
                ws.Security.Message.ClientCredentialType = System.ServiceModel.MessageCredentialType.Windows;
                ws.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows;
                bindinginstance = ws;
            }
            return bindinginstance;

        }
        #endregion

        

        #region wcf上传文件
        ///// <summary>
        ///// 上传文件
        ///// </summary>
        ///// <param name="userID">验证用的</param>
        ///// <param name="Extension">扩展名</param>
        ///// <param name="shuzhu">这是文件的byte数组数据</param>
        ///// <param name="DBMD5">对数据检验</param>
        ///// <param name="oldPhotoName">上传重复数据时可以删除之前的文件</param>
        ///// <returns></returns>
        //public string FilesUpLoad(string userID, string Extension, byte[] shuzhu, string DBMD5, string oldPhotoName)
        //{
        //    string fallies = "";
        //    string msg = "";
        //    if (UNIC.Common.Public.CheckDBMD5(userID, DBMD5, "DBPassWord", out msg))
        //    {
        //        try
        //        {

        //            if (!Public.getXmlElementValue("NotPass").ToString().Trim().Contains(Extension))
        //            {
        //                string temps = System.Web.HttpContext.Current.Server.MapPath("Files");
        //                try
        //                {

        //                    if (!string.IsNullOrEmpty(oldPhotoName))
        //                    {
        //                        if (File.Exists(temps + "\\" + oldPhotoName))
        //                        {
        //                            //判断旧图片是否存在，存在就删除
        //                            File.Delete(temps + "\\" + oldPhotoName);
        //                        }
        //                    }
        //                }
        //                catch
        //                {


        //                }
        //                if (string.IsNullOrEmpty(oldPhotoName.Trim()))
        //                {
        //                    Random rnd = new Random();
        //                    oldPhotoName = DateTime.Now.ToString("yyyyMMddHHmmss") + rnd.Next(1000, 9999);
        //                }
        //                fallies = temps + "\\" + oldPhotoName + "." + Extension;
        //                if (!Directory.Exists(temps))
        //                    Directory.CreateDirectory(temps);

        //                File.WriteAllBytes(fallies, shuzhu);
        //                fallies = "Files/" + oldPhotoName + "." + Extension;
        //            }
        //        }
        //        catch
        //        {
        //            fallies = "";

        //        }
        //    }
        //    return fallies;
        //}
        #endregion
    }
}



