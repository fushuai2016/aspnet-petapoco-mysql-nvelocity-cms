﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="article_edit.aspx.cs" Inherits="ccphl.Web.admin.article.article_edit" ValidateRequest="false" EnableEventValidation="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>编辑信息</title>
<script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
<script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
<script type="text/javascript" src="../../scripts/datepicker/WdatePicker.js"></script>
<script type="text/javascript" src="../../scripts/swfupload/swfupload.js"></script>
<script type="text/javascript" src="../../scripts/swfupload/swfupload.queue.js"></script>
<script type="text/javascript" src="../../scripts/swfupload/swfupload.handlers.js"></script>
<script type="text/javascript" charset="utf-8" src="../../editor/kindeditor-min.js"></script>
<script type="text/javascript" charset="utf-8" src="../../editor/lang/zh_CN.js"></script>
<script type="text/javascript" src="../js/layout.js"></script>
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {
        //初始化表单验证
        $("#form1").initValidform();
        //计算用户组价格
        $("#field_control_sell_price").change(function () {
            var sprice = $(this).val();
            if (sprice > 0) {
                $(".groupprice").each(function () {
                    var num = $(this).attr("discount") * sprice / 100;
                    $(this).val(ForDight(num, 2));
                });
            }
        });
        //初始化编辑器
        var editor = KindEditor.create('.editor', {
            width: '98%',
            height: '350px',
            resizeType: 1,
            uploadJson: '../../tools/upload_ajax.ashx?action=EditorFile&IsWater=0&siteid=' + $("#sid").val(),
            fileManagerJson: '../../tools/upload_ajax.ashx?action=ManagerFile&siteid=' + $("#sid").val(),
            allowFileManager: true
        });
        var editorMini = KindEditor.create('.editor-mini', {
            width: '98%',
            height: '250px',
            resizeType: 1,
            uploadJson: '../../tools/upload_ajax.ashx?action=EditorFile&IsWater=0&siteid=' + $("#sid").val(),
            items: [
				'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
				'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
				'insertunorderedlist', '|', 'emoticons', 'image', 'link']
        });
        //初始化上传控件
        $(".upload-cos").each(function () {
            $(this).InitSWFUpload({ sendurl: "../../tools/upload_ajax.ashx", siteid: $("#sid").val(), thumbnail: true, flashurl: "../../scripts/swfupload/swfupload.swf" });
        });
        //初始化上传控件
        $(".upload-img").each(function () {
            $(this).InitSWFUpload({ sendurl: "../../tools/upload_ajax.ashx", siteid: $("#sid").val(), thumbnail: true, flashurl: "../../scripts/swfupload/swfupload.swf" });
        });
        $(".upload-album").each(function () {
            $(this).InitSWFUpload({ btntext: "批量上传", btnwidth: 66, single: false, water: false, siteid: $("#sid").val(), thumbnail: true, filesize: "8096", sendurl: "../../tools/upload_ajax.ashx", flashurl: "../../scripts/swfupload/swfupload.swf", filetypes: "*.jpg;*.jpge;*.png;*.gif;" });
        });
        $(".attach-btn").click(function () {
            showAttachDialog();
        });
        //设置封面图片的样式
        $(".photo-list ul li .img-box img").each(function () {
            if ($(this).attr("src") == $("#hidFocusPhoto").val()) {
                $(this).parent().addClass("selected");
            }
        });
    });
    //创建附件窗口
    function showAttachDialog(obj) {
        var objNum = arguments.length;
        var attachDialog = $.dialog({
            id: 'attachDialogId',
            lock: true,
            max: false,
            min: false,
            title: "上传附件",
            content: 'url:dialog/dialog_attach.aspx?siteid='+$("#sid").val()+"&channelid="+$("#chid").val(),
            width: 500,
            height: 180
        });
        //如果是修改状态，将对象传进去
        if (objNum == 1) {
            attachDialog.data = obj;
        }
    }
    //删除附件节点
    function delAttachNode(obj) {
        $(obj).parent().remove();
    }
</script>
</head>
<body class="mainbody">
<form id="form1" runat="server">
<asp:HiddenField ID="chid"  runat="server"/>
<asp:HiddenField ID="sid"  runat="server"/>
<!--导航栏-->
<div class="location">
  <a href="<%=ListName %>?siteid=<%=this.siteid %>&channelid=<%=this.channelid %>&columnid=<%=this.columnid %>" class="back"><i></i><span>返回列表页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="<%=ListName %>?siteid=<%=this.siteid %>&channelid=<%=this.channelid %>&columnid=<%=this.columnid %>"><span>内容管理</span></a>
  <i class="arrow"></i>
  <span>编辑信息</span>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div class="content-tab-wrap">
  <div id="floatHead" class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a href="javascript:;" onclick="tabs(this);" class="selected">基本信息</a></li>
        <li id="field_tab_item" runat="server" visible="false"><a href="javascript:;" onclick="tabs(this);">扩展选项</a></li>
        <li><a href="javascript:;" onclick="tabs(this);">详细描述</a></li>
        <li><a href="javascript:;" onclick="tabs(this);">SEO选项</a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content">
  <dl>
    <dt>所属栏目</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="ddlCategoryId" runat="server" datatype="*" sucmsg=" "></asp:DropDownList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>显示状态</dt>
    <dd>
      <div class="rule-multi-radio">
        <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="1" Selected="True">显示</asp:ListItem>
        <asp:ListItem Value="0">不显示</asp:ListItem>
        </asp:RadioButtonList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>推荐类型</dt>
    <dd>
      <div class="rule-multi-checkbox">
        <asp:CheckBoxList ID="cblItem" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="1">允许评论</asp:ListItem>
        <asp:ListItem Value="1">置顶</asp:ListItem>
        <asp:ListItem Value="1">推荐</asp:ListItem>
        <asp:ListItem Value="1">热门</asp:ListItem>
        <asp:ListItem Value="1">推送</asp:ListItem>
        </asp:CheckBoxList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>内容标题</dt>
    <dd>
      <asp:TextBox ID="txtTitle" runat="server" CssClass="input normal" datatype="*2-100" sucmsg=" " />
      <span class="Validform_checktip">*标题最多100个字符</span>
    </dd>
  </dl>
  <dl ID="div_sub_title" runat="server" visible="false">
    <dt><asp:Label ID="div_sub_title_title" runat="server" Text="副标题" /></dt>
    <dd>
      <asp:TextBox ID="field_control_sub_title" runat="server" CssClass="input normal" datatype="*0-255" sucmsg=" " />
      <asp:Label ID="div_sub_title_tip" runat="server" CssClass="Validform_checktip" />
    </dd>
  </dl>
  <dl>
    <dt>封面图片</dt>
    <dd>
      <asp:TextBox ID="txtImgUrl" runat="server" CssClass="input normal upload-path" />
      <div class="upload-box upload-img"></div>
    </dd>
  </dl>

  <dl>
    <dt>排序数字</dt>
    <dd>
      <asp:TextBox ID="txtSortId" runat="server" CssClass="input small" datatype="n" sucmsg=" ">99</asp:TextBox>
      <span class="Validform_checktip">*数字，越小越向前</span>
    </dd>
  </dl>
  <dl>
    <dt>浏览次数</dt>
    <dd>
      <asp:TextBox ID="txtClick" runat="server" CssClass="input small" datatype="n" sucmsg=" ">0</asp:TextBox>
      <span class="Validform_checktip">点击浏览该信息自动+1</span>
    </dd>
  </dl>
  <dl>
    <dt>发布时间</dt>
    <dd>
      <div class="input-date">
        <asp:TextBox ID="txtAddTime" runat="server" CssClass="input date" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}\s{1}(\d{1,2}:){2}\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" " />
        <i>日期</i>
      </div>
      <span class="Validform_checktip">不选择默认当前发布时间</span>
    </dd>
  </dl>
  <dl ID="div_albums_container" runat="server" visible="false">
    <dt>图片相册</dt>
    <dd>
      <div class="upload-box upload-album"></div>
      <input type="hidden" name="hidFocusPhoto" id="hidFocusPhoto" runat="server" class="focus-photo" />
      <div class="photo-list">
        <ul>
          <asp:Repeater ID="rptAlbumList" runat="server">
            <ItemTemplate>
            <li>
              <input type="hidden" name="hid_photo_name" value="<%#Eval("ImageID")%>|<%#Eval("ThumbnailUrl")%>|<%#Eval("ImageUrl")%>" />
              <input type="hidden" name="hid_photo_remark" value="<%#Eval("Description")%>" />
              <div class="img-box" onclick="setFocusImg(this);">
                <img src="<%#Eval("ThumbnailUrl")%>" bigsrc="<%#Eval("ImageUrl")%>" />
                <span class="remark"><i><%#Eval("Description").ToString() == "" ? "暂无描述..." : Eval("remark").ToString()%></i></span>
              </div>
              <a href="javascript:;" onclick="setRemark(this);">描述</a>
              <a href="javascript:;" onclick="delImg(this);">删除</a>
            </li>
            </ItemTemplate>
          </asp:Repeater>
        </ul>
      </div>
    </dd>
  </dl>
  <dl ID="div_attach_container" runat="server" visible="false">
    <dt>上传附件</dt>
    <dd>
      <a class="icon-btn add attach-btn"><span>添加附件</span></a>
      <div id="showAttachList" class="attach-list">
        <ul>
          <asp:Repeater ID="rptAttachList" runat="server">
            <ItemTemplate>
              <li>
                <input name="hid_attach_id" type="hidden" value="<%#Eval("AttachID")%>" />
                <input name="hid_attach_filename" type="hidden" value="<%#Eval("FileName")%>" />
                <input name="hid_attach_filepath" type="hidden" value="<%#Eval("FilePath")%>" />
                <input name="hid_attach_filesize" type="hidden" value="<%#Eval("FileSize")%>" />
                <i class="icon"></i>
                <a href="javascript:;" onclick="delAttachNode(this);" class="del" title="删除附件"></a>
                <a href="javascript:;" onclick="showAttachDialog(this);" class="edit" title="更新附件"></a>
                <div class="title"><%#Eval("FileName")%></div>
                <div class="info">类型：<span class="ext"><%#Eval("FileExt")%></span> 大小：<span class="size"><%#(Convert.ToInt32(Eval("FileSize")) / 1024) > 1024 ? Convert.ToDouble((Convert.ToDouble(Eval("FileSize")) / 1048576f)).ToString("0.0") + "MB" : (Convert.ToInt32(Eval("FileSize")) / 1024) + "KB"%></span> 下载：<span class="down"><%#Eval("Click")%></span>次</div>
                <div class="btns">下载积分：<input type="text" name="txt_attach_point" onkeydown="return checkNumber(event);" value="<%#Eval("Point")%>" /></div>
              </li>
            </ItemTemplate>
          </asp:Repeater>
        </ul>
      </div>
    </dd>
  </dl>
</div>

<div id="field_tab_content" runat="server" visible="false" class="tab-content" style="display:none"></div>

<div class="tab-content" style="display:none">
  <dl ID="div_txtCallIndex" runat="server">
    <dt>调用别名</dt>
    <dd>
      <asp:TextBox ID="txtCallIndex" runat="server" CssClass="input normal" datatype="/^\s*$|^[a-zA-Z0-9\-\_]{2,50}$/" sucmsg=" "></asp:TextBox>
      <span class="Validform_checktip">*别名访问，非必填，不可重复</span>
    </dd>
  </dl>
  <dl ID="div_url" runat="server" visible="false">
    <dt>URL链接</dt>
    <dd>
      <asp:TextBox ID="txtLinkUrl" runat="server" maxlength="255"  CssClass="input normal" />
      <span class="Validform_checktip">填写完整的http链接，填写后直接跳转到该网址</span>
    </dd>
  </dl>
  <dl ID="div_source" runat="server" >
    <dt><asp:Label ID="div_source_title" runat="server" Text="信息来源" /></dt>
    <dd>
      <asp:TextBox ID="field_control_source" runat="server" CssClass="input normal"></asp:TextBox>
      <asp:Label ID="div_source_tip" runat="server" CssClass="Validform_checktip" />
    </dd>
  </dl>
  <dl ID="div_author" runat="server">
    <dt><asp:Label ID="div_author_title" runat="server" Text="文章作者" /></dt>
    <dd>
      <asp:TextBox ID="field_control_author" runat="server" CssClass="input normal" datatype="s0-50" sucmsg=" "></asp:TextBox>
      <asp:Label ID="div_author_tip" runat="server" CssClass="Validform_checktip" />
    </dd>
  </dl>
  <dl>
    <dt>关键词</dt>
    <dd>
      <asp:TextBox ID="txtKeywords" runat="server" CssClass="input" TextMode="MultiLine" datatype="*0-100" sucmsg=" "></asp:TextBox>
      <span class="Validform_checktip">英文逗号隔开</span>
    </dd>
  </dl>
  <dl>
    <dt>内容摘要</dt>
    <dd>
      <asp:TextBox ID="txtZhaiyao" runat="server" CssClass="input" TextMode="MultiLine" datatype="*0-255" sucmsg=" "></asp:TextBox>
      <span class="Validform_checktip">不填写则自动截取内容前255字符</span>
    </dd>
  </dl>
  <dl>
    <dt>内容描述</dt>
    <dd>
      <textarea id="txtContent" class="editor" style="visibility:hidden;" runat="server"></textarea>
    </dd>
  </dl>
</div>

<div class="tab-content" style="display:none">
  <dl>
    <dt>SEO标题</dt>
    <dd>
      <asp:TextBox ID="txtSeoTitle" runat="server" maxlength="255"  CssClass="input normal" datatype="*0-100" sucmsg=" " />
      <span class="Validform_checktip">255个字符以内</span>
    </dd>
  </dl>
  <dl>
    <dt>SEO关健字</dt>
    <dd>
      <asp:TextBox ID="txtSeoKeywords" runat="server" CssClass="input" TextMode="MultiLine" datatype="*0-255" sucmsg=" "></asp:TextBox>
      <span class="Validform_checktip">以“,”逗号区分开，255个字符以内</span>
    </dd>
  </dl>
  <dl>
    <dt>SEO描述</dt>
    <dd>
      <asp:TextBox ID="txtSeoDescription" runat="server" CssClass="input" TextMode="MultiLine" datatype="*0-255" sucmsg=" "></asp:TextBox>
      <span class="Validform_checktip">255个字符以内</span>
    </dd>
  </dl>
</div>
<!--/内容-->

<!--工具栏-->
<div class="page-footer">
  <div class="btn-list">
    <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" onclick="btnSubmit_Click" />
    <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript:history.back(-1);" />
  </div>
  <div class="clear"></div>
</div>
<!--/工具栏-->
</form>
</body>
</html>
