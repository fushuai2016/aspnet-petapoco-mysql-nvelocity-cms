using System;
using System.Collections.Generic;
using System.Text;

namespace ccphl.Entity
{
    /// <summary>
    /// 图片裁剪比例实体类
    /// </summary>
    [Serializable]
    public class ImageConfig
    {
        public ImageConfig()
        { }

        private string _imgname = "";
        private string _imgcode ="";
        private int _imgwidthshare= 0;
        private int _imgheightshare = 0;
        private int _imgstatus = 1;

        /// <summary>
        /// 图片类型名称
        /// </summary>
        public string imgname
        {
            get { return _imgname; }
            set { _imgname = value; }
        }
        /// <summary>
        /// 图片类型代码
        /// </summary>
        public string imgcode
        {
            get { return _imgcode; }
            set { _imgcode = value; }
        }
        /// <summary>
        /// 图片类型宽度占比
        /// </summary>
        public int imgwidthshare
        {
            get { return _imgwidthshare; }
            set { _imgwidthshare = value; }
        }
        /// <summary>
        /// 图片类型高度占比
        /// </summary>
        public int imgheightshare
        {
            get { return _imgheightshare; }
            set { _imgheightshare = value; }
        }
        /// <summary>
        /// 图片类型状态
        /// </summary>
        public int imgstatus
        {
            get { return _imgstatus; }
            set { _imgstatus = value; }
        }
    }
}
