<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sys_config.aspx.cs" Inherits="ccphl.Web.admin.settings.sys_config" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title><%=Name %>设置</title>
<script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
<script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
<script type="text/javascript" src="../../scripts/swfupload/swfupload.js"></script>
<script type="text/javascript" src="../../scripts/swfupload/swfupload.handlers.js"></script>
<script type="text/javascript" src="../js/layout.js"></script>
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {
        //初始化表单验证
        $("#form1").initValidform();
        //初始化上传控件
        $(".upload-img").each(function () {
            $(this).InitSWFUpload({ sendurl: "../../tools/upload_ajax.ashx", flashurl: "../../scripts/swfupload/swfupload.swf" });
        });
    });
</script>
</head>

<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <span><%=Name %>设置</span>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div class="content-tab-wrap">
  <div id="floatHead" class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a href="javascript:;" onclick="tabs(this);"  class="selected">功能权限设置</a></li>
        <li><a href="javascript:;" onclick="tabs(this);">短信平台设置</a></li>
        <li><a href="javascript:;" onclick="tabs(this);">邮件发送设置</a></li>
        <li><a href="javascript:;" onclick="tabs(this);">文件上传设置</a></li>
      </ul>
    </div>
  </div>
</div>

<!--功能权限设置-->
<div class="tab-content">
  <dl>
    <dt>网站安装目录</dt>
    <dd>
      <asp:TextBox ID="webpath" runat="server" CssClass="input txt" datatype="*1-100" sucmsg=" " />
      <span class="Validform_checktip">*根目录输入“/”，其它输入“/目录名/”</span>
    </dd>
  </dl>
  <dl>
    <dt>后台管理目录</dt>
    <dd>
      <asp:TextBox ID="webmanagepath" runat="server" CssClass="input txt" datatype="*1-100" sucmsg=" " />
      <span class="Validform_checktip">*默认admin，其它请输入目录名，否则无法进入后台</span>
    </dd>
  </dl>
  <dl>
    <dt>静态html存放目录</dt>
    <dd>
      <asp:TextBox ID="htmlpath" runat="server" CssClass="input txt" datatype="*1-100" sucmsg=" " />
      <span class="Validform_checktip">*创建于站点根目录下</span>
    </dd>
  </dl>
  <dl>
    <dt>是否开启静态html生成</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="staticstatus" runat="server" />
      </div>
      <span class="Validform_checktip">*开启后自动生成静态html</span>
    </dd>
  </dl>
  <dl>
    <dt>静态URL后缀</dt>
    <dd>
      <asp:TextBox ID="staticextension" runat="server" CssClass="input small" datatype="*1-100" sucmsg=" " />
      <span class="Validform_checktip">*扩展名，不包括“.”，访问或生成时将会替换此值，如：aspx、html</span>
    </dd>
  </dl>
  <dl>
    <dt>是否开启会员功能</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="memberstatus" runat="server" />
      </div>
      <span class="Validform_checktip">*开启后才能使用商城系统</span>
    </dd>
  </dl>
  <dl>
    <dt>是否开启评论功能</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="commentstatus" runat="server" />
      </div>
      <span class="Validform_checktip">*开启后才能评论</span>
    </dd>
  </dl>
</div>
<!--/功能权限设置-->

<!--手机短信设置-->
<div class="tab-content" style="display:none">
  <dl>
    <dt>短信平台API地址</dt>
    <dd>
      <asp:TextBox ID="smsapiurl" runat="server" CssClass="input normal" datatype="*0-256" TextMode="MultiLine"/>
    </dd>
  </dl>
  <dl>
    <dt>平台登录账户</dt>
    <dd>
      <asp:TextBox ID="smsusername" runat="server" CssClass="input txt" />
      <span class="Validform_checktip">*短信平台注册的用户名</span>
    </dd>
  </dl>
  <dl>
    <dt>平台登录密码</dt>
    <dd>
      <asp:TextBox ID="smspassword" runat="server" CssClass="input txt" TextMode="Password" />
      <span class="Validform_checktip">*短信平台注册的密码</span>
    </dd>
  </dl>
  <dl>
    <dt>短信平台说明</dt>
    <dd>
      请不要使用系统默认账户test，因为它是公用的测试账号；<br />
      请在短信平台修改账户资料中绑定签名方可使用短信功能；<br />
    </dd>
  </dl>
</div>
<!--/手机短信设置-->

<!--邮件发送设置-->
<div class="tab-content" style="display:none">
  <dl>
    <dt>SMTP服务器</dt>
    <dd>
      <asp:TextBox ID="emailsmtp" runat="server" CssClass="input normal" datatype="*0-100" sucmsg=" " />
      <span class="Validform_checktip">*发送邮件的SMTP服务器地址</span>
    </dd>
  </dl>
  <dl>
    <dt>SMTP端口</dt>
    <dd>
      <asp:TextBox ID="emailport" runat="server" CssClass="input small" datatype="n" sucmsg=" " />
      <span class="Validform_checktip">*SMTP服务器的端口，大部分服务商都支持25端口</span>
    </dd>
  </dl>
  <dl>
    <dt>发件人地址</dt>
    <dd>
      <asp:TextBox ID="emailfrom" runat="server" CssClass="input normal" datatype="e" sucmsg=" " />
      <span class="Validform_checktip">*发件人的邮箱地址</span>
    </dd>
  </dl>
  <dl>
    <dt>邮箱账号</dt>
    <dd>
      <asp:TextBox ID="emailusername" runat="server" CssClass="input normal" datatype="*0-100" sucmsg=" " />
      <span class="Validform_checktip">*</span>
    </dd>
  </dl>
  <dl>
    <dt>邮箱密码</dt>
    <dd>
      <asp:TextBox ID="emailpassword" runat="server" CssClass="input normal" datatype="*0-100" sucmsg=" " TextMode="Password" />
      <span class="Validform_checktip">*</span>
    </dd>
  </dl>
  <dl>
    <dt>发件人昵称</dt>
    <dd>
      <asp:TextBox ID="emailnickname" runat="server" CssClass="input normal" datatype="*0-50" sucmsg=" " />
      <span class="Validform_checktip">*对方收到邮件时显示的昵称</span>
    </dd>
  </dl>
</div>
<!--/邮件发送设置-->

<!--上传配置-->
<div class="tab-content" style="display:none">
  <dl>
    <dt>文件上传目录</dt>
    <dd>
      <asp:TextBox ID="filepath" runat="server" CssClass="input txt" datatype="*2-100" sucmsg=" " />
      <span class="Validform_checktip">*文件保存的目录名，自动创建根目录下</span>
    </dd>
  </dl>
  <dl>
    <dt>文件上传类型</dt>
    <dd>
      <asp:TextBox ID="fileextension" runat="server" CssClass="input normal" datatype="*1-255" TextMode="MultiLine" sucmsg=" " />
      <span class="Validform_checktip">*以英文的逗号分隔开，如：“zip,rar”</span>
    </dd>
  </dl>
  <dl>
    <dt>文件保存方式</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="filesave" runat="server" datatype="*" sucmsg=" ">
          <asp:ListItem Value="1">按年月日每天一个目录</asp:ListItem>
          <asp:ListItem Value="2">按年月/日/存入不同目录</asp:ListItem>
          <asp:ListItem Value="3">按年/月/日/存入不同目录</asp:ListItem>
        </asp:DropDownList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>附件上传大小</dt>
    <dd>
      <asp:TextBox ID="attachsize" runat="server" CssClass="input small" datatype="n" sucmsg=" " /> KB
      <span class="Validform_checktip">*超过设定的文件大小不予上传，0不限制</span>
    </dd>
  </dl>
  <dl>
    <dt>图片上传大小</dt>
    <dd>
      <asp:TextBox ID="imgsize" runat="server" CssClass="input small" datatype="n" sucmsg=" " /> KB
      <span class="Validform_checktip">*超过设定的图片大小不予上传，0不限制</span>
    </dd>
  </dl>
  <dl>
    <dt>图片最大尺寸</dt>
    <dd>
      <asp:TextBox ID="imgmaxheight" runat="server" CssClass="input small" datatype="n" sucmsg=" " /> ×
      <asp:TextBox ID="imgmaxwidth" runat="server" CssClass="input small" datatype="n" sucmsg=" " /> px
      <span class="Validform_checktip">*左边高度，右边宽度，超出自动裁剪，0为不受限制</span>
    </dd>
  </dl>
  <dl>
    <dt>图片裁剪类型</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="imgtype" runat="server" datatype="*" sucmsg=" ">
          <asp:ListItem Value="0">填充</asp:ListItem>
          <asp:ListItem Value="1">剪切</asp:ListItem>
        </asp:DropDownList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>是否开启图片压缩</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="imgzip" runat="server" />
      </div>
      <span class="Validform_checktip">*开启后，上传的原图将经过无损压缩(不会改变分辨率)</span>
    </dd>
  </dl>
  <dl>
    <dt>是否开启多图生成</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="isimgs" runat="server" />
      </div>
      <span class="Validform_checktip">*开启后，上传的图片将根据图片生成配置种类生成对应比例的图片</span>
    </dd>
  </dl>
  <dl>
    <dt>生成缩略图尺寸</dt>
    <dd>
      <asp:TextBox ID="thumbnailheight" runat="server" CssClass="input small" datatype="n" sucmsg=" " /> ×
      <asp:TextBox ID="thumbnailwidth" runat="server" CssClass="input small" datatype="n" sucmsg=" " /> px
      <span class="Validform_checktip">*左边高度，右边宽度，0为不生成缩略图</span>
    </dd>
  </dl>
  <dl>
    <dt>生成缩略图后缀</dt>
    <dd>
      <asp:TextBox ID="thumbnailsuffix" runat="server" CssClass="input small" datatype="*0-5" sucmsg=" " /> 
      <span class="Validform_checktip">*缩略图后缀</span>
    </dd>
  </dl>
</div>
<!--/上传配置-->

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