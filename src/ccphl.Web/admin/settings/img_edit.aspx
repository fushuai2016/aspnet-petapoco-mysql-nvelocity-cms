<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="img_edit.aspx.cs" Inherits="ccphl.Web.admin.settings.img_edit" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>编辑<%=Name %>信息</title>
<script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
<script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
<script type="text/javascript" src="../js/layout.js"></script>
<script type="text/javascript" src="../js/pinyin.js"></script>
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {
        //初始化表单验证
        $("#form1").initValidform();
        //添加按钮(点击绑定)
        $("#itemAddButton").click(function () {
            showChannelDialog();
        });
    });

    //删除一行
    function delItemTr(obj) {
        $.dialog.confirm("您确定要删除这个页面吗？", function () {
            $(obj).parent().parent().remove(); //删除节点
        });
    }
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
        <li><a href="javascript:;" onclick="tabs(this);" class="selected"><%=Name %>基本信息</a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content">
    <dl>
    <dt>图片类型名称</dt>
    <dd>
      <asp:TextBox ID="txtName" runat="server" CssClass="input normal" datatype="*2-50" errormsg="请填写正确的名称！" sucmsg=" "></asp:TextBox>
      <span class="Validform_checktip">*图片名称，2-50字符。</span>
    </dd>
  </dl>
  <dl>
    <dt>图片类型代码</dt>
    <dd><asp:TextBox ID="txtCode" runat="server"  CssClass="input normal"  datatype="/^[1-9]{2,5}$/" sucmsg=" "></asp:TextBox> <span class="Validform_checktip">*图片代码，只允许数字，一般填写图片宽高比的组合数字，如16:9，填169！</span></dd>
  </dl>
 <dl>
    <dt>是否启用</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="cbIsLock" runat="server" />
      </div>
      <span class="Validform_checktip">*禁用后不生成此类型图片。</span>
    </dd>
  </dl>

  <dl>
    <dt>图片宽度占比</dt>
    <dd>
      <asp:TextBox ID="txtwidth" runat="server" CssClass="input small" datatype="n" sucmsg=" ">0</asp:TextBox>
      <span class="Validform_checktip">*生成图片宽占比，如宽高比为16:9的图片，宽占比为16</span>
    </dd>
  </dl>
  <dl>
    <dt>图片高度占比</dt>
    <dd>
      <asp:TextBox ID="txtheight" runat="server" CssClass="input small" datatype="n" sucmsg=" ">0</asp:TextBox>
      <span class="Validform_checktip">**生成图片高占比，如宽高比为16:9的图片，高占比为9</span>
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
