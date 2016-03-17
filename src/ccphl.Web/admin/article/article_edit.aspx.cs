using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Linq;
using ccphl.Common;
using ccphl.DBUtility;
using Model;
using ccphl.Web.UI;

namespace ccphl.Web.admin.article
{
    public partial class article_edit : Web.UI.ManagePage
    {
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;
        public int siteid;
        public int channelid;
        public int columnid;
        protected int columntype;
        //页面名称
        public const string Name = "频道内容";
        //列表页面名称
        public const string ListName = "article_list.aspx";
        //编辑页面名称
        public const string EditName = "article_edit.aspx";
        //页面初始化事件
        protected void Page_Init(object sernder, EventArgs e)
        {
            this.siteid = DTRequest.GetQueryInt("siteid");
            this.channelid = DTRequest.GetQueryInt("channelid");
            this.columnid = DTRequest.GetQueryInt("columnid");
            this.columntype = DTRequest.GetQueryInt("columntype");
        }

        //页面加载事件
        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");

            if (siteid == 0 || channelid == 0)
            {
                JscriptMsg("频道参数不正确！", "", "Error");
                return;
            }
            this.chid.Value = this.channelid.ToString();
            this.sid.Value = this.siteid.ToString();
            this.div_txtCallIndex.Visible = false;
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryInt("id");
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "", "Error");
                    return;
                }
                if (!db.IsExists<Model.article>(this.id))
                {
                    JscriptMsg("记录不存在或已删除！", "", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.View); //检查权限
                var ch = db.GetModel<Model.channel>(this.channelid);
                if (ch.IsAlbums==1)
                {
                    div_albums_container.Visible = true;
                }
                if (ch.IsAttach == 1)
                {
                    div_attach_container.Visible = true;
                }
                TreeBind(this.siteid,this.channelid); //绑定类别
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
            }
        }

        #region 绑定类别=================================
        private void TreeBind(int site_id, int channel_id)
        {
            this.ddlCategoryId.Items.Clear();
            this.ddlCategoryId.Items.Add(new ListItem("请选择所属栏目", ""));
            var cols = GetColumnTreeList2(site_id, channel_id);
            foreach (var s in cols)
            {
                string Id = s.ColumnID.ToString();
                int ClassLayer = s.ColumnLayer ?? 1;
                string Title = s.ColumnName;

                if (ClassLayer == 1)
                {
                    this.ddlCategoryId.Items.Add(new ListItem(Title, Id));
                }
                else
                {
                    Title = "├ " + Title;
                    Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                    this.ddlCategoryId.Items.Add(new ListItem(Title, Id));
                }
            }
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            
            Model.article model = db.GetModel<Model.article>(_id);
            Model.article_content content=db.GetModel<Model.article_content>(model.ContentID);
            ddlCategoryId.SelectedValue = model.ColumnID.ToString();
            txtCallIndex.Text = model.ArticleCode;
            txtTitle.Text = model.ArticleName;
            txtLinkUrl.Text = model.LinkOutUrl;
          
            txtImgUrl.Text = model.ThumbPath;

            field_control_source.Text = model.ArticleSource;
            field_control_author.Text = model.ArticleAuthor;

            txtSeoTitle.Text = model.SEO_Title;
            txtSeoKeywords.Text = model.SEO_Keywords;
            txtSeoDescription.Text = model.SEO_Description;
            txtKeywords.Text = model.Keywords;
            txtZhaiyao.Text = model.Description;
            txtContent.Value = content.Text;
            txtSortId.Text = model.OrderBy.ToString();
            txtClick.Text = model.Click.ToString();
            rblStatus.SelectedValue = model.Status.ToString();
            txtAddTime.Text = (model.AddTime??DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss");
            if (model.IsCom == 1)
            {
                cblItem.Items[0].Selected = true;
            }
            if (model.IsTop == 1)
            {
                cblItem.Items[1].Selected = true;
            }
            if (model.IsRed == 1)
            {
                cblItem.Items[2].Selected = true;
            }
            if (model.IsHot == 1)
            {
                cblItem.Items[3].Selected = true;
            }
            if (model.IsPost == 1)
            {
                cblItem.Items[4].Selected = true;
            }
            
            
            hidFocusPhoto.Value = model.ThumbPath; //封面图片
            //组图
            var imglist = db.GetModelList<article_image>(" ArticleID="+model.ArticleID);
            rptAlbumList.DataSource = imglist;
            rptAlbumList.DataBind();
            //绑定内容附件
            var attlist = db.GetModelList<article_attach>(" ArticleID=" + model.ArticleID);
            rptAttachList.DataSource = attlist;
            rptAttachList.DataBind();
           
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = false;
            Model.article model = new Model.article();

            model.GUID = Guid.NewGuid().ToString();
            model.SiteID = this.siteid;
            model.ChannelID = this.channelid;
            model.ColumnID = Utils.StrToInt(ddlCategoryId.SelectedValue, 0);
            model.ArticleCode = txtCallIndex.Text.Trim();
            model.ArticleName = txtTitle.Text.Trim();
            model.LinkOutUrl = txtLinkUrl.Text.Trim();
            model.ThumbPath = txtImgUrl.Text;
            model.ArticleSource = field_control_source.Text.Trim() == "" ? siteConfig.SiteName : field_control_source.Text.Trim();
            model.ArticleAuthor = field_control_author.Text.Trim() == "" ? GetAdminInfo().UserName : field_control_author.Text.Trim();
            model.SEO_Title = txtSeoTitle.Text.Trim();
            model.SEO_Keywords = txtSeoKeywords.Text.Trim();
            model.SEO_Description = txtSeoDescription.Text.Trim();
            model.Keywords = txtKeywords.Text.Trim();
            //内容摘要提取内容前255个字符
            if (string.IsNullOrEmpty(txtZhaiyao.Text.Trim()))
            {
                model.Description = Utils.DropHTML(txtContent.Value, 255);
            }
            else
            {
                model.Description = Utils.DropHTML(txtZhaiyao.Text, 255);
            }
            model.OrderBy = Utils.StrToInt(txtSortId.Text.Trim(), 99);
            model.Click = int.Parse(txtClick.Text.Trim());
            model.Status = (sbyte)Utils.StrToInt(rblStatus.SelectedValue, 0);
            model.IsCom = 0;
            model.IsTop = 0;
            model.IsRed = 0;
            model.IsHot = 0;
            model.IsPost = 0;
            if (cblItem.Items[0].Selected == true)
            {
                model.IsCom = 1;
            }
            if (cblItem.Items[1].Selected == true)
            {
                model.IsTop = 1;
            }
            if (cblItem.Items[2].Selected == true)
            {
                model.IsRed = 1;
            }
            if (cblItem.Items[3].Selected == true)
            {
                model.IsHot = 1;
            }
            if (cblItem.Items[4].Selected == true)
            {
                model.IsPost = 1;
            }
            model.IsDelete = 0;

            model.AddUserID = GetAdminInfo().UserID; //管理员发布
            model.AddTime = Utils.StrToDateTime(txtAddTime.Text.Trim());

            #region 保存文章内容====================
            article_content content = new article_content();
            content.SiteID = this.siteid;
            content.ChannelID = this.channelid;
            content.GUID = Guid.NewGuid().ToString();
            content.Text = txtContent.Value;

            long contentid = db.Insert<article_content>(content) ;
            model.ContentID = contentid;
            #endregion

            long aid = db.Insert<Model.article>(model);
            
            #region 保存相册====================
            //检查是否有自定义图片
            if (txtImgUrl.Text.Trim() == "")
            {
                model.ThumbPath = hidFocusPhoto.Value;
            }
            string[] albumArr = Request.Form.GetValues("hid_photo_name");
            string[] remarkArr = Request.Form.GetValues("hid_photo_remark");
            if (albumArr != null && albumArr.Length > 0)
            {
                for (int i = 0; i < albumArr.Length; i++)
                {
                    string[] imgArr = albumArr[i].Split('|');
                    if (imgArr.Length == 3)
                    {
                        article_image img = new article_image();
                        if (!string.IsNullOrEmpty(remarkArr[i]))
                        {
                            img.GUID = Guid.NewGuid().ToString();
                            img.SiteID = this.siteid;
                            img.ChannelID = this.channelid;
                            img.ArticleID = aid;
                            img.Description = remarkArr[i];
                            img.ThumbnailUrl = imgArr[1];
                            img.ImageUrl = imgArr[2];
                            img.IsSingle = 0;
                            img.AddTime = DateTime.Now;
                        }
                        else
                        {
                            img.GUID = Guid.NewGuid().ToString();
                            img.SiteID = this.siteid;
                            img.ChannelID = this.channelid;
                            img.ArticleID = aid;
                            img.Description = "";
                            img.ThumbnailUrl = imgArr[1];
                            img.ImageUrl = imgArr[2];
                            img.IsSingle = 0;
                            img.AddTime = DateTime.Now;
                        }
                        img.Insert();
                    }
                }
            }
            #endregion

            #region 保存附件====================
            //保存附件
            string[] attachFileNameArr = Request.Form.GetValues("hid_attach_filename");
            string[] attachFilePathArr = Request.Form.GetValues("hid_attach_filepath");
            string[] attachFileSizeArr = Request.Form.GetValues("hid_attach_filesize");
            string[] attachPointArr = Request.Form.GetValues("txt_attach_point");
            if (attachFileNameArr != null && attachFilePathArr != null && attachFileSizeArr != null && attachPointArr != null
                && attachFileNameArr.Length > 0 && attachFilePathArr.Length > 0 && attachFileSizeArr.Length > 0 && attachPointArr.Length > 0)
            {
               
                for (int i = 0; i < attachFileNameArr.Length; i++)
                {
                    article_attach att = new article_attach();
                    int fileSize = Utils.StrToInt(attachFileSizeArr[i], 0);
                    string fileExt = Utils.GetFileExt(attachFilePathArr[i]);
                    int _point = Utils.StrToInt(attachPointArr[i], 0);
                    att.GUID = Guid.NewGuid().ToString();
                    att.SiteID = this.siteid;
                    att.ChannelID = this.channelid;
                    att.ArticleID = aid;
                    att.AddTime = DateTime.Now;
                    att.FileName = attachFileNameArr[i];
                    att.FilePath = attachFilePathArr[i];
                    att.FileSize = fileSize;
                    att.FileExt = fileExt;
                    att.Point = _point;
                    att.Click = 0;
                    att.Insert();
                }
            }
            #endregion

            if (aid > 0)
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加频道内容:" + model.ArticleName); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = false;
            Model.article model = db.GetModel<Model.article>(_id);
            var imglist = db.GetModelList<article_image>(" ArticleID="+_id);//图片相册列表
            var attlist = db.GetModelList<article_attach>(" ArticleID=" + _id);//附件列表

            model.SiteID = this.siteid;
            model.ChannelID = this.channelid;
            model.ColumnID = Utils.StrToInt(ddlCategoryId.SelectedValue, 0);
            model.ArticleCode = txtCallIndex.Text.Trim();
            model.ArticleName = txtTitle.Text.Trim();
            model.LinkOutUrl = txtLinkUrl.Text.Trim();
            model.ThumbPath = txtImgUrl.Text;
            model.ArticleSource = field_control_source.Text.Trim() == "" ? siteConfig.SiteName : field_control_source.Text.Trim();
            model.ArticleAuthor = field_control_author.Text.Trim() == "" ? GetAdminInfo().UserName : field_control_author.Text.Trim();
            model.SEO_Title = txtSeoTitle.Text.Trim();
            model.SEO_Keywords = txtSeoKeywords.Text.Trim();
            model.SEO_Description = txtSeoDescription.Text.Trim();
            model.Keywords = txtKeywords.Text.Trim();
            //内容摘要提取内容前255个字符
            if (string.IsNullOrEmpty(txtZhaiyao.Text.Trim()))
            {
                model.Description = Utils.DropHTML(txtContent.Value, 255);
            }
            else
            {
                model.Description = Utils.DropHTML(txtZhaiyao.Text, 255);
            }
            model.OrderBy = Utils.StrToInt(txtSortId.Text.Trim(), 99);
            model.Click = int.Parse(txtClick.Text.Trim());
            model.Status = (sbyte)Utils.StrToInt(rblStatus.SelectedValue, 0);
            model.IsCom = 0;
            model.IsTop = 0;
            model.IsRed = 0;
            model.IsHot = 0;
            model.IsPost = 0;
            if (cblItem.Items[0].Selected == true)
            {
                model.IsCom = 1;
            }
            if (cblItem.Items[1].Selected == true)
            {
                model.IsTop = 1;
            }
            if (cblItem.Items[2].Selected == true)
            {
                model.IsRed = 1;
            }
            if (cblItem.Items[3].Selected == true)
            {
                model.IsHot = 1;
            }
            if (cblItem.Items[4].Selected == true)
            {
                model.IsPost = 1;
            }
           
            model.UpdateUserID = GetAdminInfo().UserID; //管理员发布
            model.UpdateTime = DateTime.Now; //获得当前登录用户名

            #region 保存文章内容====================

            article_content content =db.GetModel<article_content>(model.ContentID);
            content.Text = txtContent.Value;
            content.Update();

            #endregion

            #region 保存相册====================
            //检查是否有自定义图片
            if (txtImgUrl.Text.Trim() == "")
            {
                model.ThumbPath = hidFocusPhoto.Value;
            }
            string[] albumArr = Request.Form.GetValues("hid_photo_name");
            string[] remarkArr = Request.Form.GetValues("hid_photo_remark");
            if (albumArr != null)
            {
                for (int i = 0; i < albumArr.Length; i++)
                {
                    string[] imgArr = albumArr[i].Split('|');
                    int img_id = Utils.StrToInt(imgArr[0], 0);
                    if (imgArr.Length == 3)
                    { 
                        article_image img=new article_image();
                        if (db.IsExists<article_image>(img_id))
                        {
                            img = db.GetModel<article_image>(img_id);
                            imglist=imglist.Where(p => p.ImageID != img_id).ToList();
                        }
                        if (!string.IsNullOrEmpty(remarkArr[i]))
                        {
                            if (img.ImageID == 0)
                            {
                                img.GUID = Guid.NewGuid().ToString();
                                img.SiteID = this.siteid;
                                img.ChannelID = this.channelid;
                                img.IsSingle = 0;
                                img.AddTime = DateTime.Now;
                            }
                            img.ArticleID = model.ArticleID;
                            img.Description = remarkArr[i];
                            img.ThumbnailUrl = imgArr[1];
                            img.ImageUrl = imgArr[2];
                            if (img.ImageID == 0)
                            {
                                img.Insert();
                            }
                            else
                            {
                                img.Update();
                            }
                        }
                        else
                        {
                            if (img.ImageID == 0)
                            {
                                img.GUID = Guid.NewGuid().ToString();
                                img.SiteID = this.siteid;
                                img.ChannelID = this.channelid;
                                img.IsSingle = 0;
                                img.AddTime = DateTime.Now;
                            }
                            img.ArticleID = model.ArticleID;
                            img.Description = "";
                            img.ThumbnailUrl = imgArr[1];
                            img.ImageUrl = imgArr[2];
                            if (img.ImageID == 0)
                            {
                                img.Insert();
                            }
                            else
                            {
                                img.Update();
                            }
                        }
                    }
                }
            }
            //删除已删除的图片
            foreach (var im in imglist)
            {
                Utils.DeleteFile(im.ImageUrl);
                Utils.DeleteFile(im.ThumbnailUrl);
                db.Delete<article_image>(im.ImageID);
            }
            #endregion

            #region 保存附件====================
           
            string[] attachIdArr = Request.Form.GetValues("hid_attach_id");
            string[] attachFileNameArr = Request.Form.GetValues("hid_attach_filename");
            string[] attachFilePathArr = Request.Form.GetValues("hid_attach_filepath");
            string[] attachFileSizeArr = Request.Form.GetValues("hid_attach_filesize");
            string[] attachPointArr = Request.Form.GetValues("txt_attach_point");
            if (attachIdArr != null && attachFileNameArr != null && attachFilePathArr != null && attachFileSizeArr != null && attachPointArr != null
                && attachIdArr.Length > 0 && attachFileNameArr.Length > 0 && attachFilePathArr.Length > 0 && attachFileSizeArr.Length > 0 && attachPointArr.Length > 0)
            {
                for (int i = 0; i < attachFileNameArr.Length; i++)
                {
                    article_attach att = new article_attach();
                    int attachId = Utils.StrToInt(attachIdArr[i], 0);
                    int fileSize = Utils.StrToInt(attachFileSizeArr[i], 0);
                    string fileExt = Utils.GetFileExt(attachFilePathArr[i]);
                    int _point = Utils.StrToInt(attachPointArr[i], 0);
                    if (db.IsExists<Model.article_attach>(attachId))
                    {
                        att = db.GetModel<article_attach>(attachId);
                        attlist = attlist.Where(p => p.AttachID != attachId).ToList();
                        attlist.Remove(att);
                        att.FileName = attachFileNameArr[i];
                        att.FilePath = attachFilePathArr[i];
                        att.FileSize = fileSize;
                        att.FileExt = fileExt;
                        att.Point = _point;
                        att.Update();
                    }
                    else
                    {
                        att.GUID = Guid.NewGuid().ToString();
                        att.SiteID = this.siteid;
                        att.ChannelID = this.channelid;
                        att.ArticleID = model.ArticleID;
                        att.AddTime = DateTime.Now;
                        att.FileName = attachFileNameArr[i];
                        att.FilePath = attachFilePathArr[i];
                        att.FileSize = fileSize;
                        att.FileExt = fileExt;
                        att.Point = _point;
                        att.Click = 0;
                        att.Insert();
                    }
                }
            }
            //删除已删除的附件
            foreach (var at in attlist)
            {
                Utils.DeleteFile(at.FilePath);
                db.Delete<article_attach>(at.AttachID);
            }
            #endregion

            if (model.Update()>0)
            {
                Books b = new Books();
                b.Id = (int)model.ArticleID;
                b.Title = model.ArticleName;
                b.Content = model.Description;
                b.Link = "/news/"+model.GUID+".htm";
                b.Thumbnail = model.ThumbPath;
                b.AddTime = model.AddTime.ToString();
                LuceneHelper.luceneHelper.Mod(b);
                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改频道内容:" + model.ArticleName); //记录日志
                result = true;
            }
            return result;
        }
        #endregion

        
        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.Edit); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                JscriptMsg("修改信息成功！", ListName + "?siteid=" + siteid + "&channelid=" + channelid + "&columnid=" + columnid + "&columntype=" + columntype, "Success");
            }
            else //添加
            {
                ChkAdminWebLevel(this.siteid, this.channelid, this.columntype, (long)DTEnums.ActionEnum.Add); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                JscriptMsg("添加信息成功！", ListName + "?siteid=" + siteid + "&channelid=" + channelid + "&columnid=" + columnid + "&columntype=" + columntype, "Success");
            }
        }
    }
}