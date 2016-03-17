<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="column_edit.aspx.cs" Inherits="ccphl.Web.admin.article.column_edit" ValidateRequest="false" %>
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

        $("#btnVarAdd").click(function () {
            varHtml = createVarHtml();
            $("#tr_box").append(varHtml);

        });
       $(".upload-img").each(function () {
            $(this).InitSWFUpload({ sendurl: "../../tools/upload_ajax.ashx", flashurl: "../../scripts/swfupload/swfupload.swf",siteid:<%=this.siteid %>,thumbnail:true, filetypes: "*.jpg;*.jpge;*.png;*.gif;" });
        });
        
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
<asp:HiddenField ID="chid"  runat="server"/>
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
        <li><a href="javascript:;" onclick="tabs(this);">SEO选项</a></li>
        
      </ul>
    </div>
  </div>
</div>

<div class="tab-content">
  <dl>
    <dt>所属父栏目</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="ddlParentId" runat="server"></asp:DropDownList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>栏目名称</dt>
    <dd><asp:TextBox ID="txtTitle" runat="server" onBlur="change2cn(this.value, this.form.txtCallIndex)" CssClass="input normal" datatype="*1-100" sucmsg=" "></asp:TextBox> <span class="Validform_checktip">*类别中文名称，100字符内</span></dd>
  </dl>
  <dl>
    <dt>调用别名</dt>
    <dd>
      <asp:TextBox ID="txtCallIndex" runat="server" CssClass="input normal" datatype="/^[a-zA-Z0-9\-\_]{2,50}$/" errormsg="请填写正确的别名" sucmsg=" "></asp:TextBox>
      <span class="Validform_checktip">填写栏目名称对应英文名称，如：帮助中心可以命名为 “help或者helpcenter”</span>
    </dd>
  </dl>
   <dl id="content_txtThumb" runat="server">
    <dt>封面图片</dt>
    <dd>
      <asp:TextBox ID="txtThumb" runat="server" CssClass="input normal upload-path" />
      <div class="upload-box upload-img"></div>
    </dd>
  </dl>
  <dl ID="div_url" runat="server" visible="false">
    <dt>URL链接</dt>
    <dd>
      <asp:TextBox ID="txtLinkUrl" runat="server" maxlength="255"   CssClass="input normal" />
      <span class="Validform_checktip">*请填写完整的url，如：“http://www.baidu.com”填写后直接跳转到该网址</span>
    </dd>
  </dl>
  <dl>
    <dt>是否禁用</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="cbStatus" runat="server" />
      </div>
      <span class="Validform_checktip">*禁用后频道无效。</span>
    </dd>
  </dl>
  <dl ID="div_isedit_container" runat="server" visible="false">
    <dt>开启编辑功能</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="cbIsEdit" runat="server" />
      </div>
      <span class="Validform_checktip">*开启编辑功能后才能在后台编辑新闻内容</span>
    </dd>
  </dl>
  <dl ID="div_isalbums_container" runat="server" visible="false">
    <dt>开启组图功能</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="cbIsAlbums" runat="server" />
      </div>
      <span class="Validform_checktip">*开启组图功能后可上传多张图片</span>
    </dd>
  </dl>
  <dl ID="div_isattach_container" runat="server" visible="false">
    <dt>开启附件功能</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="cbIsAttach" runat="server" />
      </div>
      <span class="Validform_checktip">*开启附件功能后可上传多个附件。</span>
    </dd>
  </dl>
  <dl id="content_cblPageType" runat="server">
    <dt>页面类型</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="cblPageType" ClientIDMode="Static" runat="server"  sucmsg=" "></asp:DropDownList>
        <span class="Validform_checktip">*页面类型</span>
      </div>
    </dd>
  </dl>
  <dl id="content_indextemplate" runat="server">
    <dt>首页显示模板</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="indextemplate" ClientIDMode="Static" runat="server"  sucmsg=" "></asp:DropDownList>
        <span class="Validform_checktip">*首页模板，以字符index结尾（不包括扩展名）</span>
      </div>
    </dd>
  </dl>
   <dl id="content_listtemplate" runat="server">
    <dt>列表显示模板</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="listtemplate" ClientIDMode="Static" runat="server"  sucmsg=" "></asp:DropDownList>
        <span class="Validform_checktip">*列表模板，以字符list结尾（不包括扩展名）</span>
      </div>
    </dd>
  </dl>
   <dl id="content_showtemplate" runat="server">
    <dt>详细显示模板</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="showtemplate" runat="server" ClientIDMode="Static"></asp:DropDownList>
        <span class="Validform_checktip">*详细模板，以字符show结尾（不包括扩展名）</span>
      </div>
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
    <dt>关健词</dt>
    <dd>
      <asp:TextBox ID="txtkeywords" runat="server" CssClass="input normal" datatype="*0-200" TextMode="MultiLine"/>
      <span class="Validform_checktip">页面关键词(keyword)</span>
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

<div class="tab-content" style="display:none">
 <dl>
    <dt>SEO标题</dt>
    <dd>
      <asp:TextBox ID="txtSeoTitle" runat="server" maxlength="255"  CssClass="input normal" datatype="s0-100" sucmsg=" " />
      <span class="Validform_checktip">255个字符以内</span>
    </dd>
  </dl>
  <dl>
    <dt>SEO关健字</dt>
    <dd>
      <asp:TextBox ID="txtSeoKeywords" runat="server" CssClass="input" TextMode="MultiLine"></asp:TextBox>
      <span class="Validform_checktip">以“,”逗号区分开，255个字符以内</span>
    </dd>
  </dl>
  <dl>
    <dt>SEO描述</dt>
    <dd>
      <asp:TextBox ID="txtSeoDescription" runat="server" CssClass="input" TextMode="MultiLine"></asp:TextBox>
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
