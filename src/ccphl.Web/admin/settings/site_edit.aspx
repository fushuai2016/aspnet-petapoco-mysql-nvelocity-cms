<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="site_edit.aspx.cs" Inherits="ccphl.Web.admin.settings.site_edit" ValidateRequest="false" EnableEventValidation="false"%>
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
<script type="text/javascript" src="../js/pinyin.js"></script>
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {
        //初始化表单验证
        $("#form1").initValidform();
        //初始化上传控件
        $(".upload-img").each(function () {
            $(this).InitSWFUpload({ sendurl: "../../tools/upload_ajax.ashx", flashurl: "../../scripts/swfupload/swfupload.swf",siteid:0 });
        });
    });
    function selectChange(obj, id) {
        var dir = $('#' + id).val();
        $('#indextemplate').html('');
        $('#indextemplate').html('<option value="">请选择主页模板</option>');
        $('#listtemplate').html('');
        $('#listtemplate').html('<option value="">请选择列表模板</option>');
        $('#showtemplate').html('');
        $('#showtemplate').html('<option value="">请选择详细模板</option>');
        $.ajax({
            type: "post",
            url: "../../tools/admin_ajax.ashx?action=template_files&dir=" + dir,
            dataType: "json",
            success: function (data, textStatus) {
                if (data.status == 1 && data.total > 0) {
                    $.each(data.filelist, function (l, item) {
                        if (item.value.replace(".html", "").endWith("index")) {
                            $('#indextemplate').append('<option value="' + item.value + '">' + item.value + '</option>');
                        }
                        else if (item.value.replace(".html", "").endWith("list")) {
                            $('#listtemplate').append('<option value="' + item.value + '">' + item.value + '</option>');
                        }
                        else if (item.value.replace(".html", "").endWith("show")) {
                            $('#showtemplate').append('<option value="' + item.value + '">' + item.value + '</option>');
                        }
                    });
                } else {
                }
                $(".rule-single-select").ruleSingleSelect();
            }
        });
    }
    String.prototype.endWith = function (str) {
        if (str == null || str == "" || this.length == 0 || str.length > this.length)
            return false;
        if (this.substring(this.length - str.length) == str)
            return true;
        else
            return false;
        return true;
    }
    function change2py(cn, eninput) {
        eninput.value = CC2PY(cn).replace(/\s+/g, "");
    }
</script>
</head>

<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="<%=ListName%>" class="back"><i></i><span>返回列表页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="<%=ListName%>"><span><%=Name%>列表</span></a>
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
        <li><a href="javascript:;" onclick="tabs(this);" class="selected"><%=Name%>基本信息</a></li>
        <li><a href="javascript:;" onclick="tabs(this);">模板设置</a></li>
        <li><a href="javascript:;" onclick="tabs(this);">水印设置</a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content">
  <dl>
    <dt>上级站点</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="ddlParentId" runat="server"></asp:DropDownList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>网站名称</dt>
    <dd>
      <asp:TextBox ID="webname" runat="server" CssClass="input normal" datatype="*2-255" sucmsg=" " />
      <span class="Validform_checktip">*任意字符，控制在255个字符内</span>
    </dd>
  </dl>
  <dl>
    <dt>网站代码</dt>
    <dd>
      <asp:TextBox ID="webcode" runat="server" CssClass="input normal" datatype="/^[a-zA-Z\_]{2,50}$/" errormsg="请填写正确的代码" sucmsg=" "></asp:TextBox>
      <span class="Validform_checktip">站点代码，只允许字母、下划线</span>
    </dd>
  </dl>
  <dl>
    <dt>是否禁用</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="cbIsLock" runat="server" />
      </div>
      <span class="Validform_checktip">*禁用后站点无法访问。</span>
    </dd>
  </dl>
 <dl>
    <dt>网站域名</dt>
    <dd>
      <asp:TextBox ID="weburl" runat="server" CssClass="input normal" datatype="url" sucmsg=" " />
      <span class="Validform_checktip">*以“http://”开头</span>
    </dd>
  </dl>
  <dl>
    <dt>网站LOGO</dt>
    <dd>
      <asp:TextBox ID="weblogo" runat="server" CssClass="input normal upload-path" />
      <div class="upload-box upload-img"></div>
    </dd>
  </dl>
  <dl>
    <dt>公司名称</dt>
    <dd>
      <asp:TextBox ID="webcompany" runat="server" CssClass="input normal" datatype="*0-50"/>
    </dd>
  </dl>
  <dl>
    <dt>通讯地址</dt>
    <dd>
      <asp:TextBox ID="webaddress" runat="server" CssClass="input normal" datatype="*0-256" TextMode="MultiLine"/>
    </dd>
  </dl>
  <dl>
    <dt>联系电话</dt>
    <dd>
      <asp:TextBox ID="webtel" runat="server" CssClass="input normal" datatype="*0-50" />
      <span class="Validform_checktip">*非必填，区号+电话号码</span>
    </dd>
  </dl>
  <dl>
    <dt>传真号码</dt>
    <dd>
      <asp:TextBox ID="webfax" runat="server" CssClass="input normal" datatype="*0-50"/>
      <span class="Validform_checktip">*非必填，区号+传真号码</span>
    </dd>
  </dl>
  <dl>
    <dt>管理员邮箱</dt>
    <dd>
      <asp:TextBox ID="webmail" runat="server" CssClass="input normal" datatype="*0-50"/>
    </dd>
  </dl>
  <dl>
    <dt>网站备案号</dt>
    <dd>
      <asp:TextBox ID="webicp" runat="server" CssClass="input normal" datatype="*0-50"/>
    </dd>
  </dl>
  <dl>
    <dt>首页标题(SEO)</dt>
    <dd>
      <asp:TextBox ID="webtitle" runat="server" CssClass="input normal" datatype="*0-50"/>
      <span class="Validform_checktip">*自定义的首页标题</span>
    </dd>
  </dl>
  <dl>
    <dt>页面关健词(SEO)</dt>
    <dd>
      <asp:TextBox ID="webkeyword" runat="server" CssClass="input normal" datatype="*0-200" TextMode="MultiLine"/>
      <span class="Validform_checktip">页面关键词(keyword)</span>
    </dd>
  </dl>
  <dl>
    <dt>页面描述(SEO)</dt>
    <dd>
      <asp:TextBox ID="webdescription" runat="server" CssClass="input normal" datatype="*0-512" TextMode="MultiLine"/>
      <span class="Validform_checktip">页面描述(description)</span>
    </dd>
  </dl>
  <dl>
    <dt>网站版权信息</dt>
    <dd>
      <asp:TextBox ID="webcopyright" runat="server" CssClass="input" TextMode="MultiLine" />
      <span class="Validform_checktip">支持HTML</span>
    </dd>
  </dl>
  <dl>
    <dt>排序数字</dt>
    <dd>
      <asp:TextBox ID="txtSortId" runat="server" CssClass="input small" datatype="n" sucmsg=" ">99</asp:TextBox>
      <span class="Validform_checktip">*数字，越小越向前</span>
    </dd>
  </dl>
  <dl ID="div_channel_container" runat="server" visible="false">
    <dt>网站频道</dt>
    <dd>
      <div class="rule-multi-porp">
          <asp:CheckBoxList ID="cblChannel" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:CheckBoxList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>网站关闭原因</dt>
    <dd>
      <asp:TextBox ID="webclosereason" runat="server" CssClass="input" TextMode="MultiLine" />
      <span class="Validform_checktip">支持HTML</span>
    </dd>
  </dl>
  <dl>
    <dt>网站统计代码</dt>
    <dd>
      <asp:TextBox ID="webcountcode" runat="server" CssClass="input" TextMode="MultiLine" />
      <span class="Validform_checktip">支持HTML</span>
    </dd>
  </dl>
</div>

<!--功能权限设置-->
<div class="tab-content" style="display:none">
  <dl>
    <dt>模板存放目录</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="template" ClientIDMode="Static" runat="server" datatype="*" sucmsg=" "></asp:DropDownList>
        <span class="Validform_checktip">*目录“~/template”下的文件夹，模板文件统一存放于此目录下</span>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>首页显示模板</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="indextemplate" ClientIDMode="Static" runat="server" sucmsg=" "></asp:DropDownList>
        <span class="Validform_checktip">*首页模板，以字符index结尾（不包括扩展名）</span>
      </div>
    </dd>
  </dl>
   <dl>
    <dt>列表显示模板</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="listtemplate" ClientIDMode="Static" runat="server"  sucmsg=" "></asp:DropDownList>
        <span class="Validform_checktip">*列表模板，以字符list结尾（不包括扩展名）</span>
      </div>
    </dd>
  </dl>
   <dl>
    <dt>详细显示模板</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="showtemplate" runat="server" ClientIDMode="Static" ></asp:DropDownList>
        <span class="Validform_checktip">*详细模板，以字符show结尾（不包括扩展名）</span>
      </div>
    </dd>
  </dl>
</div>
<!--/功能权限设置-->

<!--上传配置-->
<div class="tab-content" style="display:none">
  <dl>
    <dt>图片水印类型</dt>
    <dd>
      <div class="rule-multi-radio">
        <asp:RadioButtonList ID="watermarktype" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="0">关闭水印</asp:ListItem>
        <asp:ListItem Value="1">文字水印</asp:ListItem>
        <asp:ListItem Value="2">图片水印</asp:ListItem>
        </asp:RadioButtonList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>图片水印位置</dt>
    <dd>
      <div class="rule-multi-radio">
        <asp:RadioButtonList ID="watermarkposition" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="1">左上</asp:ListItem>
        <asp:ListItem Value="2">中上</asp:ListItem>
        <asp:ListItem Value="3">右上</asp:ListItem>
        <asp:ListItem Value="4">左中</asp:ListItem>
        <asp:ListItem Value="5">居中</asp:ListItem>
        <asp:ListItem Value="6">右中</asp:ListItem>
        <asp:ListItem Value="7">左下</asp:ListItem>
        <asp:ListItem Value="8">中下</asp:ListItem>
        <asp:ListItem Value="9">右下</asp:ListItem>
        </asp:RadioButtonList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>图片水印文件</dt>
    <dd>
      <asp:TextBox ID="watermarkpic" runat="server" CssClass="input normal upload-path" />
      <div class="upload-box upload-img"></div>
      <span class="Validform_checktip">*如图片不存在将使用文字水印</span>
    </dd>
  </dl>
  <dl>
    <dt>水印透明度</dt>
    <dd>
      <asp:TextBox ID="watermarktransparency" runat="server" CssClass="input small" datatype="n" sucmsg=" " />
      <span class="Validform_checktip">*取值范围1--10 (10为不透明)</span>
    </dd>
  </dl>
  <dl>
    <dt>水印文字</dt>
    <dd>
      <asp:TextBox ID="watermarktext" runat="server" CssClass="input txt" datatype="s2-100" sucmsg=" " />
      <span class="Validform_checktip">*文字水印的内容</span>
    </dd>
  </dl>
  <dl>
    <dt>文字字体</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="watermarkfont" runat="server">
          	<asp:ListItem Value="Arial">Arial</asp:ListItem>
	        <asp:ListItem Value="Arial Black">Arial Black</asp:ListItem>
	        <asp:ListItem Value="Batang">Batang</asp:ListItem>
	        <asp:ListItem Value="BatangChe">BatangChe</asp:ListItem>
	        <asp:ListItem Value="Comic Sans MS">Comic Sans MS</asp:ListItem>
	        <asp:ListItem Value="Courier New">Courier New</asp:ListItem>
	        <asp:ListItem Value="Dotum">Dotum</asp:ListItem>
	        <asp:ListItem Value="DotumChe">DotumChe</asp:ListItem>
	        <asp:ListItem Value="Estrangelo Edessa">Estrangelo Edessa</asp:ListItem>
	        <asp:ListItem Value="Franklin Gothic Medium">Franklin Gothic Medium</asp:ListItem>
	        <asp:ListItem Value="Gautami">Gautami</asp:ListItem>
	        <asp:ListItem Value="Georgia">Georgia</asp:ListItem>
	        <asp:ListItem Value="Gulim">Gulim</asp:ListItem>
	        <asp:ListItem Value="GulimChe">GulimChe</asp:ListItem>
	        <asp:ListItem Value="Gungsuh">Gungsuh</asp:ListItem>
	        <asp:ListItem Value="GungsuhChe">GungsuhChe</asp:ListItem>
	        <asp:ListItem Value="Impact">Impact</asp:ListItem>
	        <asp:ListItem Value="Latha">Latha</asp:ListItem>
	        <asp:ListItem Value="Lucida Console">Lucida Console</asp:ListItem>
	        <asp:ListItem Value="Lucida Sans Unicode">Lucida Sans Unicode</asp:ListItem>
	        <asp:ListItem Value="Mangal">Mangal</asp:ListItem>
	        <asp:ListItem Value="Marlett">Marlett</asp:ListItem>
	        <asp:ListItem Value="Microsoft Sans Serif">Microsoft Sans Serif</asp:ListItem>
	        <asp:ListItem Value="MingLiU">MingLiU</asp:ListItem>
	        <asp:ListItem Value="MS Gothic">MS Gothic</asp:ListItem>
	        <asp:ListItem Value="MS Mincho">MS Mincho</asp:ListItem>
	        <asp:ListItem Value="MS PGothic">MS PGothic</asp:ListItem>
	        <asp:ListItem Value="MS PMincho">MS PMincho</asp:ListItem>
	        <asp:ListItem Value="MS UI Gothic">MS UI Gothic</asp:ListItem>
	        <asp:ListItem Value="MV Boli">MV Boli</asp:ListItem>
	        <asp:ListItem Value="Palatino Linotype">Palatino Linotype</asp:ListItem>
	        <asp:ListItem Value="PMingLiU">PMingLiU</asp:ListItem>
	        <asp:ListItem Value="Raavi">Raavi</asp:ListItem>
	        <asp:ListItem Value="Shruti">Shruti</asp:ListItem>
	        <asp:ListItem Value="Sylfaen">Sylfaen</asp:ListItem>
	        <asp:ListItem Value="Symbol">Symbol</asp:ListItem>
	        <asp:ListItem Value="Tahoma" Selected="True">Tahoma</asp:ListItem>
	        <asp:ListItem Value="Times New Roman">Times New Roman</asp:ListItem>
	        <asp:ListItem Value="Trebuchet MS">Trebuchet MS</asp:ListItem>
	        <asp:ListItem Value="Tunga">Tunga</asp:ListItem>
	        <asp:ListItem Value="Verdana">Verdana</asp:ListItem>
	        <asp:ListItem Value="Webdings">Webdings</asp:ListItem>
	        <asp:ListItem Value="Wingdings">Wingdings</asp:ListItem>
	        <asp:ListItem Value="仿宋_GB2312">仿宋_GB2312</asp:ListItem>
	        <asp:ListItem Value="宋体">宋体</asp:ListItem>
	        <asp:ListItem Value="新宋体">新宋体</asp:ListItem>
	        <asp:ListItem Value="楷体_GB2312">楷体_GB2312</asp:ListItem>
	        <asp:ListItem Value="黑体">黑体</asp:ListItem>
        </asp:DropDownList>
      </div>
      <asp:TextBox ID="watermarkfontsize" runat="server" CssClass="input small" datatype="n" sucmsg=" " /> px
      <span class="Validform_checktip">*文字水印的字体和大小 </span>
    </dd>
  </dl>
</div>
<!--/上传配置-->

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
