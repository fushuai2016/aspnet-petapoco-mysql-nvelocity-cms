using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using ccphl.Entity;

namespace ccphl.Web.admin.settings
{
    public partial class sys_config : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "系统参数";
        //列表页面名称
        public const string ListName = "sys_config.aspx";
        //编辑页面名称
        public const string EditName = "sys_config.aspx";
        string defaultpassword = "0|0|0|0"; //默认显示密码
        XmlConfig<SystemConfig> xc = new XmlConfig<SystemConfig>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ChkAdminLevel((long)DTEnums.ActionEnum.View); //检查权限
                ShowInfo();
            }
        }

        #region 赋值操作=================================
        private void ShowInfo()
        {
            SystemConfig model = xc.loadSystemConfig();

            webpath.Text = model.webpath;
            webmanagepath.Text = model.webmanagepath;
            htmlpath.Text = model.htmlpath;
            staticextension.Text = model.staticextension;
            if (model.staticstatus == 1)
            {
                staticstatus.Checked = true;
            }
            else
            {
                staticstatus.Checked = false;
            }
            if (model.memberstatus == 1)
            {
                memberstatus.Checked = true;
            }
            else
            {
                memberstatus.Checked = false;
            }
            if (model.commentstatus == 1)
            {
                commentstatus.Checked = true;
            }
            else
            {
                commentstatus.Checked = false;
            }

            smsapiurl.Text=model.smsapiurl;
            smsusername.Text = model.smsusername;
            if (!string.IsNullOrEmpty(model.smspassword))
            {
                smspassword.Attributes["value"] = defaultpassword;
            }

            emailsmtp.Text = model.emailsmtp;
            emailport.Text = model.emailport.ToString();
            emailfrom.Text = model.emailfrom;
            emailusername.Text = model.emailusername;
            if (!string.IsNullOrEmpty(model.emailpassword))
            {
                emailpassword.Attributes["value"] = defaultpassword;
            }
            emailnickname.Text = model.emailnickname;

            filepath.Text = model.filepath;
            filesave.SelectedValue = model.filesave.ToString();
            fileextension.Text = model.fileextension;
            attachsize.Text = model.attachsize.ToString();
            imgsize.Text = model.imgsize.ToString();
            imgmaxheight.Text = model.imgmaxheight.ToString();
            imgmaxwidth.Text = model.imgmaxwidth.ToString();
            thumbnailheight.Text = model.thumbnailheight.ToString();
            thumbnailwidth.Text = model.thumbnailwidth.ToString();
            if (model.imgzip == 1)
            {
                imgzip.Checked = true;
            }
            else
            {
                imgzip.Checked = false;
            }
            if (model.isimgs == 1)
            {
                isimgs.Checked = true;
            }
            else
            {
                isimgs.Checked = false;
            }
            imgtype.SelectedValue = model.imgtype.ToString();
            thumbnailsuffix.Text = model.thumbnailsuffix;
        }
        #endregion

        #region 获取短信数量=================================
        private string GetSmsCount()
        {
            //string code = string.Empty;
            //int count = new BLL.sms_message().GetAccountQuantity(out code);
            //if (code == "115")
            //{
            //    return "查询出错：请完善账户信息";
            //}
            //else if (code != "100")
            //{
            //    return "错误代码：" + code;
            //}
            //return count + " 条";
            return "";
        }
        #endregion

        /// <summary>
        /// 保存配置信息
        /// </summary>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ChkAdminLevel((long)DTEnums.ActionEnum.Edit); //检查权限

            SystemConfig model = xc.loadSystemConfig();
            try
            {
                model.webpath = webpath.Text;
                model.webmanagepath = webmanagepath.Text;
                model.htmlpath=htmlpath.Text;
                model.staticextension = staticextension.Text;
                if (memberstatus.Checked == true)
                {
                    model.memberstatus = 1;
                }
                else
                {
                    model.memberstatus = 0;
                }
                if (commentstatus.Checked == true)
                {
                    model.commentstatus = 1;
                }
                else
                {
                    model.commentstatus = 0;
                }
                if (staticstatus.Checked == true)
                {
                    model.staticstatus = 1;
                }
                else
                {
                    model.staticstatus = 0;
                }
                if (memberstatus.Checked == true)
                {
                    model.memberstatus = 1;
                }
                else
                {
                    model.memberstatus = 0;
                }
                if (commentstatus.Checked == true)
                {
                    model.commentstatus = 1;
                }
                else
                {
                    model.commentstatus = 0;
                }

                model.smsapiurl = smsapiurl.Text;
                model.smsusername = smsusername.Text;
                //判断密码是否更改
                if (smspassword.Text.Trim() != "" && smspassword.Text.Trim() != defaultpassword)
                {
                    model.smspassword = Utils.MD5(smspassword.Text.Trim());
                }

                model.emailsmtp = emailsmtp.Text;
                model.emailport = Utils.StrToInt(emailport.Text.Trim(), 25);
                model.emailfrom = emailfrom.Text;
                model.emailusername = emailusername.Text;
                //判断密码是否更改
                if (emailpassword.Text.Trim() != defaultpassword)
                {
                    model.emailpassword = DESEncrypt.Encrypt(emailpassword.Text);
                }
                model.emailnickname = emailnickname.Text;

                model.filepath = filepath.Text;
                model.filesave = Utils.StrToInt(filesave.SelectedValue, 2);
                model.fileextension = fileextension.Text;
                model.attachsize = Utils.StrToInt(attachsize.Text.Trim(), 0);
                model.imgsize = Utils.StrToInt(imgsize.Text.Trim(), 0);
                model.imgmaxheight = Utils.StrToInt(imgmaxheight.Text.Trim(), 0);
                model.imgmaxwidth = Utils.StrToInt(imgmaxwidth.Text.Trim(), 0);
                model.thumbnailheight = Utils.StrToInt(thumbnailheight.Text.Trim(), 0);
                model.thumbnailwidth = Utils.StrToInt(thumbnailwidth.Text.Trim(), 0);
                if (imgzip.Checked == true)
                {
                    model.imgzip = 1;
                }
                else
                {
                    model.imgzip = 0;
                }
                if (isimgs.Checked == true)
                {
                    model.isimgs = 1;
                }
                else
                {
                    model.isimgs = 0;
                }
                model.imgtype =  Utils.StrToInt(imgtype.SelectedValue,0);
                model.thumbnailsuffix = thumbnailsuffix.Text;

                xc.saveSystemConifg(model);

                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改"+Name+"信息"); //记录日志
                JscriptMsg("修改"+Name+"成功！", EditName, "Success");
            }
            catch
            {
                JscriptMsg("文件写入失败，请检查是否有权限！", "", "Error");
            }
        }

    }
}