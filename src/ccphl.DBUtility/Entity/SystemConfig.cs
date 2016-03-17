using System;
using System.Collections.Generic;
using System.Text;

namespace ccphl.Entity
{
    /// <summary>
    /// 系统配置实体类
    /// </summary>
    [Serializable]
    public class SystemConfig
    {
        public SystemConfig()
        { }

        private string _webpath = "";
        private string _webmanagepath = "";
        private string _htmlpath = "";
        private int _staticstatus = 0;
        private string _staticextension = "";
        private int _memberstatus = 0;
        private int _commentstatus = 0;

        private string _smsapiurl = "";
        private string _smsusername = "";
        private string _smspassword = "";

        private string _emailsmtp = "";
        private int _emailport = 25;
        private string _emailfrom = "";
        private string _emailusername = "";
        private string _emailpassword = "";
        private string _emailnickname = "";

        private string _filepath = "";
        private int _filesave = 1;
        private string _fileextension = "";
        private int _attachsize = 0;
        private int _imgzip = 0;
        private int _imgtype = 0;
        private int _imgsize = 0;
        private int _isimgs = 0;
        private int _imgmaxheight = 0;
        private int _imgmaxwidth = 0;
        private int _thumbnailheight = 0;
        private int _thumbnailwidth = 0;
        private string _thumbnailsuffix = "";

        #region 功能权限设置==================================
        /// <summary>
        /// 网站安装目录
        /// </summary>
        public string webpath
        {
            get { return _webpath; }
            set { _webpath = value; }
        }
        /// <summary>
        /// 网站管理目录
        /// </summary>
        public string webmanagepath
        {
            get { return _webmanagepath; }
            set { _webmanagepath = value; }
        }
        /// <summary>
        /// 静态文件存放文件夹名称
        /// </summary>
        public string  htmlpath
        {
            get { return _htmlpath; }
            set { _htmlpath = value; }
        }
        /// <summary>
        /// 是否开启生成静态
        /// </summary>
        public int staticstatus
        {
            get { return _staticstatus; }
            set { _staticstatus = value; }
        }
        /// <summary>
        /// 生成静态扩展名
        /// </summary>
        public string staticextension
        {
            get { return _staticextension; }
            set { _staticextension = value; }
        }
        /// <summary>
        /// 开启会员功能
        /// </summary>
        public int memberstatus
        {
            get { return _memberstatus; }
            set { _memberstatus = value; }
        }
        /// <summary>
        /// 开启评论审核
        /// </summary>
        public int commentstatus
        {
            get { return _commentstatus; }
            set { _commentstatus = value; }
        }

        #endregion

        #region 短信平台设置==================================
        /// <summary>
        /// 短信API地址
        /// </summary>
        public string smsapiurl
        {
            get { return _smsapiurl; }
            set { _smsapiurl = value; }
        }
        /// <summary>
        /// 短信平台登录账户名
        /// </summary>
        public string smsusername
        {
            get { return _smsusername; }
            set { _smsusername = value; }
        }
        /// <summary>
        /// 短信平台登录密码
        /// </summary>
        public string smspassword
        {
            get { return _smspassword; }
            set { _smspassword = value; }
        }
        #endregion

        #region 邮件发送设置==================================
        /// <summary>
        /// STMP服务器
        /// </summary>
        public string emailsmtp
        {
            get { return _emailsmtp; }
            set { _emailsmtp = value; }
        }
        /// <summary>
        /// SMTP端口
        /// </summary>
        public int emailport
        {
            get { return _emailport; }
            set { _emailport = value; }
        }
        /// <summary>
        /// 发件人地址
        /// </summary>
        public string emailfrom
        {
            get { return _emailfrom; }
            set { _emailfrom = value; }
        }
        /// <summary>
        /// 邮箱账号
        /// </summary>
        public string emailusername
        {
            get { return _emailusername; }
            set { _emailusername = value; }
        }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string emailpassword
        {
            get { return _emailpassword; }
            set { _emailpassword = value; }
        }
        /// <summary>
        /// 发件人昵称
        /// </summary>
        public string emailnickname
        {
            get { return _emailnickname; }
            set { _emailnickname = value; }
        }
        #endregion

        #region 文件上传设置==================================
        /// <summary>
        /// 附件上传目录
        /// </summary>
        public string filepath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }
        /// <summary>
        /// 附件保存方式
        /// </summary>
        public int filesave
        {
            get { return _filesave; }
            set { _filesave = value; }
        }
        /// <summary>
        /// 附件上传类型
        /// </summary>
        public string fileextension
        {
            get { return _fileextension; }
            set { _fileextension = value; }
        }
        /// <summary>
        /// 文件上传大小
        /// </summary>
        public int attachsize
        {
            get { return _attachsize; }
            set { _attachsize = value; }
        }
        /// <summary>
        /// 是否开启图片压缩 0 关闭 1开启
        /// </summary>
        public int imgzip
        {
            get { return _imgzip; }
            set { _imgzip = value; }
        }
        /// <summary>
        /// 是否开启多图片生成 0 关闭 1开启
        /// </summary>
        public int isimgs
        {
            get { return _isimgs; }
            set { _isimgs = value; }
        }
        /// <summary>
        /// 图片裁剪方式 0 填充 1剪切
        /// </summary>
        public int imgtype
        {
            get { return _imgtype; }
            set { _imgtype = value; }
        }
        /// <summary>
        /// 图片上传大小
        /// </summary>
        public int imgsize
        {
            get { return _imgsize; }
            set { _imgsize = value; }
        }
        /// <summary>
        /// 图片最大高度(像素)
        /// </summary>
        public int imgmaxheight
        {
            get { return _imgmaxheight; }
            set { _imgmaxheight = value; }
        }
        /// <summary>
        /// 图片最大宽度(像素)
        /// </summary>
        public int imgmaxwidth
        {
            get { return _imgmaxwidth; }
            set { _imgmaxwidth = value; }
        }
        /// <summary>
        /// 生成缩略图高度(像素)
        /// </summary>
        public int thumbnailheight
        {
            get { return _thumbnailheight; }
            set { _thumbnailheight = value; }
        }
        /// <summary>
        /// 生成缩略图宽度(像素)
        /// </summary>
        public int thumbnailwidth
        {
            get { return _thumbnailwidth; }
            set { _thumbnailwidth = value; }
        }
        /// <summary>
        /// 生成缩略图后缀
        /// </summary>
        public string thumbnailsuffix
        {
            get { return _thumbnailsuffix; }
            set { _thumbnailsuffix = value; }
        }
        #endregion

    }
}
