<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ads_edit.aspx.cs" Inherits="ccphl.Web.admin.ads.ads_edit" ValidateRequest="false" %>
<%@ Import namespace="ccphl.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>编辑<%=Name%></title>
<script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
<script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
<script type="text/javascript" src="../../scripts/swfupload/swfupload.js"></script>
<script type="text/javascript" src="../../scripts/swfupload/swfupload.queue.js"></script>
<script type="text/javascript" src="../../scripts/swfupload/swfupload.handlers.js"></script>
<script type="text/javascript" src="../js/layout.js"></script>
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {
        //初始化表单验证
        $("#form1").initValidform();
         //初始化上传控件
        $(".upload-img").each(function () {
            $(this).InitSWFUpload({ sendurl: "../../tools/upload_ajax.ashx", flashurl: "../../scripts/swfupload/swfupload.swf",siteid:<%=siteConfig.SiteID %>,thumbnail:true, filetypes: "*.jpg;*.jpge;*.png;*.gif;" });
        });
        
    });
</script>
</head>

<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="<%=ListName%>" class="back"><i></i><span>返回列表页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="<%=ListName%>"><span><%=Name%></span></a>
  <i class="arrow"></i>
  <span>编辑<%=Name%></span>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div class="content-tab-wrap">
  <div id="floatHead" class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a href="javascript:;" onclick="tabs(this);" class="selected">编辑<%=Name%></a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content">
   <dl>
    <dt>所属位置</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="ddlParentId" runat="server" datatype="*" sucmsg=" "></asp:DropDownList>
      </div>
    </dd>
  </dl>
 <dl>
    <dt>是否禁用</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="cbStatus" runat="server" />
      </div>
      <span class="Validform_checktip">*禁用后广告无效。</span>
    </dd>
  </dl>
  <dl>
    <dt>广告名称</dt>
    <dd><asp:TextBox ID="txtTitle" runat="server" CssClass="input normal" datatype="*1-100" sucmsg=" "></asp:TextBox> <span class="Validform_checktip">*广告名称，100字符内</span></dd>
  </dl>
 <dl>
    <dt>封面图片</dt>
    <dd>
      <asp:TextBox ID="thumb" runat="server" CssClass="input normal upload-path" />
      <div class="upload-box upload-img"></div>
    </dd>
  </dl>
  <dl>
    <dt>URL链接</dt>
    <dd>
      <asp:TextBox ID="txtLinkUrl" runat="server" maxlength="255"  CssClass="input normal" />
      <span class="Validform_checktip">广告图片链接地址</span>
    </dd>
  </dl>
    <dl>
    <dt>排序数字</dt>
    <dd>
      <asp:TextBox ID="txtSortId" runat="server" CssClass="input small" datatype="n" sucmsg=" ">99</asp:TextBox>
      <span class="Validform_checktip">*数字，越小越向前</span>
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
