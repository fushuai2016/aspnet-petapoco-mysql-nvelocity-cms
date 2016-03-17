<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="down_edit.aspx.cs" Inherits="ccphl.Web.admin.ccphl.down_edit" ValidateRequest="false" %>
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
            $(this).InitSWFUpload({ sendurl: "http://localhost:12688/Upload/add?appkey=24lb844dt4zr22tttv4p", flashurl: "../../scripts/swfupload/swfupload.swf",siteid:<%=siteConfig.SiteID %>,thumbnail:true, filetypes: "*.jpg;*.jpeg;*.png;*.gif;" });
        });
//        $(".upload-img").each(function () {
//            $(this).InitSWFUpload({ sendurl: "../../tools/upload_ajax.ashx", flashurl: "../../scripts/swfupload/swfupload.swf",siteid:<%=siteConfig.SiteID %>,thumbnail:true, filetypes: "*.jpg;*.jpge;*.png;*.gif;" });
//        });
         $(".upload-file").each(function () {
            $(this).InitSWFUpload({ sendurl: "../../tools/upload_ajax.ashx", flashurl: "../../scripts/swfupload/swfupload.swf",siteid:<%=siteConfig.SiteID %>, filetypes: "*.jpg;*.jpeg;*.png;*.gif;*.rar;*.zip;*.doc;*.xls",filesize: "204800"});
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
    <dt>文件名称</dt>
    <dd>
      <asp:TextBox ID="txtfile" runat="server" CssClass="input normal " />
    </dd>
  </dl>
 <dl>
    <dt>封面图片</dt>
    <dd>
      <asp:TextBox ID="thumb" runat="server" CssClass="input normal upload-path" />
      <div class="upload-box upload-img"></div>
    </dd>
  </dl>
  <dl>
    <dt>上传文件</dt>
    <dd>
    <asp:TextBox ID="txtpath" runat="server" CssClass="input normal upload-path" datatype="*" sucmsg=" "></asp:TextBox> 
    <div class="upload-box upload-file"></div>
    <input type="hidden" id="filename" runat="server" class="upload-name"/>
    <input type="hidden" id="filesize" runat="server" class="upload-size"/>
    <input type="hidden" id="fileext" runat="server" class="upload-ext"/>
    <span class="Validform_checktip">*选择文件上传</span></dd>
  </dl> 
   <dl>
    <dt>文件描述</dt>
    <dd>
      <asp:TextBox ID="txtRemark" runat="server" CssClass="input" TextMode="MultiLine"></asp:TextBox>
      <span class="Validform_checktip">非必填，可为空</span>
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
