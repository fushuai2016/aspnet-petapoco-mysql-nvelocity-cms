using System;
using System.Collections;
using System.Web;
using System.IO;
using System.Drawing;
using System.Net;
using System.Configuration;
using ccphl.Common;
using ccphl.Entity;
using Model;
using ccphl.DBUtility;
using System.Threading;
using System.Collections.Generic;

namespace ccphl.Web.UI
{
    public class UpLoad
    {
        static private SystemConfig sysConfig;
        XmlConfig<SystemConfig> xc = new XmlConfig<SystemConfig>();
        XmlConfig<ImageConfig> xc2 = new XmlConfig<ImageConfig>();
        static private List<ImageConfig> imgList;
        public UpLoad()
        {
            sysConfig = xc.loadSystemConfig();
            imgList = xc2.loadImgListConfig();
        }
        /// <summary>
        /// 文件上传方法
        /// </summary>
        /// <param name="postedFile">文件流</param>
        /// <param name="siteid">站点id</param>
        /// <param name="isThumbnail">是否生成缩略图</param>
        /// <param name="isimgs">是否生成多图</param>
        /// <returns>上传后文件信息</returns>
        public string fileSaveAs(HttpPostedFile postedFile, int siteid,bool isThumbnail=true,bool isimgs=true,bool iswater=true)
        {
            try
            {
                MySqlUtility db = new MySqlUtility();
                system_site site = db.GetModel<system_site>(siteid);
                //if (site.SiteID == 0)
                //{
                //    return "{\"status\": 0, \"msg\": \"文件上传站点不存在！\"}";
                //}
                string fileExt = Utils.GetFileExt(postedFile.FileName); //文件扩展名，不含“.”
                int fileSize = postedFile.ContentLength; //获得文件大小，以字节为单位
                string fileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\") + 1); //取得原文件名
                string random = Utils.GetRamCode();//时间戳
                string newFileName = random + "." + fileExt; //随机生成新的文件名
                string newThumbnailFileName = random + "_" + sysConfig.thumbnailsuffix + "." + fileExt; //随机生成缩略图文件名
                string upLoadPath = GetUpLoadPath(site); //上传目录相对路径
                string fullUpLoadPath = Utils.GetMapPath(upLoadPath); //上传目录的物理路径
                string newFilePath = upLoadPath + newFileName; //上传后的路径
                string newThumbnailPath = upLoadPath + newThumbnailFileName; //上传后的缩略图路径
                //非图片类型
                if (!IsImage(fileExt))
                {
                    if (!File.Exists(fullUpLoadPath + fileName))
                    {
                        newFileName =fileName;
                    }
                    else
                    {
                        newFileName = Utils.Number(2) + "_" + fileName;
                    }
                    newFilePath = upLoadPath + newFileName; //上传后的路径
                }

                //检查文件扩展名是否合法
                if (!CheckFileExt(fileExt))
                {
                    return "{\"status\": 0, \"msg\": \"不允许上传" + fileExt + "类型的文件！\"}";
                }
                //检查文件大小是否合法
                if (!CheckFileSize(fileExt, fileSize))
                {
                    return "{\"status\": 0, \"msg\": \"文件超过限制的大小啦！\"}";
                }
                //检查上传的物理路径是否存在，不存在则创建
                if (!Directory.Exists(fullUpLoadPath))
                {
                    Directory.CreateDirectory(fullUpLoadPath);
                }

                //保存文件
                postedFile.SaveAs(fullUpLoadPath + newFileName);
                //如果是图片，检查图片是否超出最大尺寸，是则裁剪
                if (IsImage(fileExt) && (sysConfig.imgmaxheight > 0 ||sysConfig.imgmaxwidth > 0))
                {
                    Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newFileName,
                        sysConfig.imgmaxwidth, sysConfig.imgmaxheight);
                }
                //如果是图片，检查是否需要生成缩略图，是则生成
                if (IsImage(fileExt) && isThumbnail && sysConfig.thumbnailwidth > 0 && sysConfig.thumbnailheight > 0)
                {
                    Thumbnail.MakeThumbnailImage(fullUpLoadPath + newFileName, fullUpLoadPath + newThumbnailFileName,
                        sysConfig.thumbnailwidth,sysConfig.thumbnailheight, "Cut");
                }
                //如果是图片，检查是否需要压缩图片
                if (IsImage(fileExt)&&sysConfig.imgzip==1)
                {
                    Thumbnail.Compression(fullUpLoadPath + newFileName, fullUpLoadPath + newFileName);
                }
                //如果是图片，检查是否需要打水印
                if (IsWaterMark(fileExt) && iswater && site.WaterType > 0)
                {
                    switch (site.WaterType)
                    {
                        case 1:
                            WaterMark.AddImageSignText(newFilePath, newFilePath,
                                site.WaterText, site.WaterPos??0,
                                80, site.WaterFont, site.WaterFontSize??0);
                            break;
                        case 2:
                            WaterMark.AddImageSignPic(newFilePath, newFilePath,
                                site.WaterImgUrl, site.WaterPos??0,
                                80, site.WaterTransparency??0);
                            break;
                        default:
                            break;
                    }
                }
                //如果是图片，检查是否需要生成多种尺寸的图片
                if (IsImage(fileExt) && isimgs && sysConfig.isimgs==1)
                {
                    MyImages m = new MyImages();
                    m.iswater = iswater;
                    m.site = site;
                    m.random = random;
                    m.fullUpLoadPath = fullUpLoadPath;
                    m.fileExt = fileExt;
                    m.newFileName = newFileName;
                    //多线程生成
                    Thread picThread = new Thread(new ThreadStart(m.CreateImages));
                    picThread.Name = "ccphl_create_images";
                    picThread.Start();
                }
                //处理完毕，返回JOSN格式的文件信息
                return "{\"status\": 1, \"msg\": \"上传文件成功！\", \"name\": \""
                    + fileName + "\", \"path\": \"" + newFilePath + "\", \"thumb\": \""
                    + newThumbnailPath + "\", \"size\": " + fileSize + ", \"ext\": \"" + fileExt + "\"}";
            }
            catch
            {
                return "{\"status\": 0, \"msg\": \"上传过程中发生意外错误！\"}";
            }
        }

        #region 私有方法
        /// <summary>
        /// 多图生成类
        /// </summary>
        private class MyImages
        {
            public bool iswater;
            /// <summary>
            /// 图片随机名称
            /// </summary>
            public string random;
            /// <summary>
            /// 图片扩展名
            /// </summary>
            public string fileExt;
            /// <summary>
            /// 图片上传路径
            /// </summary>
            public string fullUpLoadPath;
            /// <summary>
            /// 原图名称
            /// </summary>
            public string newFileName;
            /// <summary>
            /// 站点信息
            /// </summary>
            public system_site site;

            private object lockThis = new object();//线程锁  
            public void CreateImages()
            {
                lock (lockThis)
                {
                    foreach (var img in imgList)
                    {
                        if (img.imgstatus == 1)
                        {
                            string filename = random + "_" + img.imgcode + "." + fileExt;//生成图片名称
                            string filepath = fullUpLoadPath + filename;//生成图片完整物理路径
                            if (Thumbnail.Crop(fullUpLoadPath + newFileName, filepath, img.imgwidthshare, img.imgheightshare, sysConfig.imgtype) && site.WaterType > 0&&iswater)
                            {
                                switch (site.WaterType)
                                {
                                    case 1:
                                        WaterMark.AddImageSignText(filepath, filepath,
                                            site.WaterText, site.WaterPos ?? 0,
                                            80, site.WaterFont, site.WaterFontSize ?? 0);
                                        break;
                                    case 2:
                                        WaterMark.AddImageSignPic(filepath, filepath,
                                            site.WaterImgUrl, site.WaterPos ?? 0,
                                            80, site.WaterTransparency ?? 0);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }  
        /// <summary>
        /// 返回上传目录相对路径
        /// </summary>
        /// <param name="fileName">上传文件名</param>
        private string GetUpLoadPath(system_site site)
        {
            if (site.SiteID == 0)
                site.SiteCode = "default";
            string path = sysConfig.webpath + sysConfig.filepath + "/" + site.SiteCode + "/"; //站点目录+上传目录+站点代码
            switch (sysConfig.filesave)
            {
                case 1: //按年月日每天一个文件夹
                    path += DateTime.Now.ToString("yyyyMMdd");
                    break;
                case 2: //按年月日每天一个文件夹
                    path += DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd");
                    break;
                default: //按年/月/日存入不同的文件夹
                    path += DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd");
                    break;
            }
            return path + "/";
        }

        /// <summary>
        /// 是否需要打水印
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsWaterMark(string _fileExt)
        {
            //判断是否可以打水印的图片类型
            ArrayList al = new ArrayList();
            al.Add("bmp");
            al.Add("jpeg");
            al.Add("jpg");
            al.Add("png");
            if (al.Contains(_fileExt.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否为图片文件
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsImage(string _fileExt)
        {
            ArrayList al = new ArrayList();
            al.Add("bmp");
            al.Add("jpeg");
            al.Add("jpg");
            al.Add("gif");
            al.Add("png");
            if (al.Contains(_fileExt.ToLower()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查是否为合法的上传文件
        /// </summary>
        private bool CheckFileExt(string _fileExt)
        {
            //检查危险文件
            string[] excExt = { "asp", "aspx", "php", "jsp", "htm", "html" };
            for (int i = 0; i < excExt.Length; i++)
            {
                if (excExt[i].ToLower() == _fileExt.ToLower())
                {
                    return false;
                }
            }
            //检查合法文件
            string[] allowExt = sysConfig.fileextension.Split(',');
            for (int i = 0; i < allowExt.Length; i++)
            {
                if (allowExt[i].ToLower() == _fileExt.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查文件大小是否合法
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <param name="_fileSize">文件大小(B)</param>
        private bool CheckFileSize(string _fileExt, int _fileSize)
        {
            //判断是否为图片文件
            if (IsImage(_fileExt))
            {
                if (sysConfig.imgsize > 0 && _fileSize > sysConfig.imgsize * 1024)
                {
                    return false;
                }
            }
            else
            {
                if (sysConfig.attachsize > 0 && _fileSize > sysConfig.attachsize * 1024)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

    }
}
