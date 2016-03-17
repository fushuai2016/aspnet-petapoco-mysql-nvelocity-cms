using System;
using System.Collections.Generic;
using System.Configuration;
using ccphl.DBUtility;
using System.Text;
using Model;
using ccphl.Common;

namespace ccphl.NVelocity
{
    public class BLLHelper
    {
        /// <summary>
        /// 重写的通用数据库接口
        /// </summary>
        MySqlUtility db = null;
        /// <summary>
        /// 原生数据库接口
        /// </summary>
        MySqlHelper db2 = null;
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public BLLHelper()
        {
            if (db == null)
                db = new MySqlUtility();
            if (db2 == null)
                db2 = MySqlHelper.GetInstance();
          
        }

       


    }
}