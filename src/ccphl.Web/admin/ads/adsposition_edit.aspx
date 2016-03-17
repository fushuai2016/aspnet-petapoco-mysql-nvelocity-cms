<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adsposition_edit.aspx.cs" Inherits="ccphl.Web.admin.ads.adsposition_edit" ValidateRequest="false" %>
<%@ Import namespace="ccphl.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>

<title>编辑类别</title>
<script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
<script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
<script type="text/javascript" charset="utf-8" src="../../editor/kindeditor-min.js"></script>
<script type="text/javascript" charset="utf-8" src="../../editor/lang/zh_CN.js"></script>
<script type='text/javascript' src="../../scripts/swfupload/swfupload.js"></script>
<script type="text/javascript" src="../../scripts/swfupload/swfupload.handlers.js"></script>
<script type="text/javascript" src="../js/layout.js"></script>
<script type="text/javascript" src="../js/pinyin.js"></script>
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {
        //初始化表单验证
        $("#form1").initValidform();

    });

    function change2cn(en, cninput) {
        cninput.value = getSpell(en, "");
    }
    function change2py(cn, eninput) {
        eninput.value = CC2PY(cn).replace(/\s+/g, "");
    }
   
</script>
</head>

<body class="mainbody">
<form id="form1" runat="server">
<asp:HiddenField ID="sid"  runat="server"/>
<!--导航栏-->
<div class="location">
  <a href="<%=ListName%>" class="back"><i></i><span>返回列表页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="<%=ListName %>"><span><%=Name%>列表</span></a>
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
        <li><a href="javascript:;" onclick="tabs(this);" class="selected">基本信息</a></li>
        
      </ul>
    </div>
  </div>
</div>

<div class="tab-content">
  <dl>
    <dt>所属父位置</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="ddlParentId" runat="server"></asp:DropDownList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>广告位置名称</dt>
    <dd><asp:TextBox ID="txtTitle" runat="server" onBlur="change2cn(this.value, this.form.txtCallIndex)" CssClass="input normal" datatype="*1-100" sucmsg=" "></asp:TextBox> <span class="Validform_checktip">*类别中文名称，100字符内</span></dd>
  </dl>
  <dl>
    <dt>调用别名</dt>
    <dd>
      <asp:TextBox ID="txtCallIndex" runat="server" CssClass="input normal" datatype="/^[a-zA-Z0-9\-\_]{2,50}$/" errormsg="请填写正确的别名" sucmsg=" "></asp:TextBox>
      <span class="Validform_checktip">广告位置的调用别名，只允许字母、数字、下划线</span>
    </dd>
  </dl>
  <dl>
    <dt>是否禁用</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="cbStatus" runat="server" />
      </div>
      <span class="Validform_checktip">*禁用后广告位置无效。</span>
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
    <dt>描述</dt>
    <dd>
      <asp:TextBox ID="txtdescription" runat="server" CssClass="input normal" datatype="*0-200" TextMode="MultiLine"/>
      <span class="Validform_checktip">页面描述(description)</span>
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
