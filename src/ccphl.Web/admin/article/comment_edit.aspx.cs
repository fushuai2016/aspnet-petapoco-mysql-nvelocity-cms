using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ccphl.Common;
using Model;
using ccphl.DBUtility;

namespace ccphl.Web.admin.article
{
    public partial class comment_edit : Web.UI.ManagePage
    {
        //页面名称
        public const string Name = "文章评论";
        //列表页面名称
        public const string ListName = "comment_list.aspx";
        //编辑页面名称
        public const string EditName = "comment_edit.aspx";
        protected int channelid;
        protected int siteid;
        private int id = 0;
        protected int columntype;

        protected comment model = new comment();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.channelid = DTRequest.GetQueryInt("channelid");
            this.siteid = DTRequest.GetQueryInt("siteid");
            this.id = DTRequest.GetQueryInt("id");
            this.columntype = DTRequest.GetQueryInt("columntype");
            if (channelid == 0 || siteid == 0)
            {
                JscriptMsg("参数不正确！", "", "Error");
                return;
            }
            if (id == 0)
            {
                JscriptMsg("传输参数不正确！", "", "Error");
                return;
            }
            if (!db.IsExists<comment>(this.id))
            {
                JscriptMsg("记录不存在或已删除！", "", "Error");
                return;
            }
            if (!Page.IsPostBack)
            {
                ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.Reply); //检查权限
                ShowInfo(this.id);
            }
        }

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            model = db.GetModel<comment>(_id);
            txtReContent.Text = Utils.ToTxt(model.ReplyText);
            rblIsLock.SelectedValue = model.Status.ToString();
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.Reply); //检查权限

            model = db.GetModel<comment>(this.id);
            model.IsReply = 1;
            model.ReplyText = Utils.ToHtml(txtReContent.Text);
            model.IsReply =(sbyte)Utils.StrToInt(rblIsLock.SelectedValue,0);
            model.ReplyTime = DateTime.Now;
            model.Update();
            AddAdminLog(DTEnums.ActionEnum.Reply.ToString(), "回复评论ID:" + model.CommentID); //记录日志
            JscriptMsg("评论回复成功！", ListName + "?siteid=" + siteid + "&channelid=" + channelid + "&columntype=" + this.columntype, "Success");
        }
    }
}