<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="role_edit.aspx.cs" Inherits="ccphl.Web.admin.manager.role_edit" ValidateRequest="false" %>
<%@ Import namespace="ccphl.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>编辑<%=Name%></title>
<script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
<script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
<script type="text/javascript" src="../js/layout.js"></script>
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {
        //初始化表单验证
        $("#form1").initValidform();
        //是否启用权限
        if ($("#ddlRoleType").find("option:selected").attr("value") == 1) {
            $(".border-table").find("input[type='checkbox']").prop("disabled", true);
        }
        $("#ddlRoleType").change(function () {
            if ($(this).find("option:selected").attr("value") == 1) {
                $(".border-table").find("input[type='checkbox']").prop("checked", false);
                $(".border-table").find("input[type='checkbox']").prop("disabled", true);
            } else {
                $(".border-table").find("input[type='checkbox']").prop("disabled", false);
            }
        });
        //权限全选
        $("input[name='checkAll']").click(function () {
            if ($(this).prop("checked") == true) {
                $(this).parent().siblings("td").find("input[type='checkbox']").prop("checked", true);
            } else {
                $(this).parent().siblings("td").find("input[type='checkbox']").prop("checked", false);
            }
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
  <a href="manager_list.aspx"><span>管理员</span></a>
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
        <li><a href="javascript:;" onclick="tabs(this);" class="selected">编辑<%=Name%>信息</a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content">
  <dl>
    <dt>角色名称</dt>
    <dd><asp:TextBox ID="txtRoleName" runat="server" CssClass="input normal" datatype="*1-100" sucmsg=" "></asp:TextBox> <span class="Validform_checktip">*角色中文名称，100字符内</span></dd>
  </dl>  
   <dl>
    <dt>是否启用</dt>
    <dd>
      <div class="rule-single-checkbox">
          <asp:CheckBox ID="cbIsLock" runat="server" Checked="True" />
      </div>
      <span class="Validform_checktip">*不启用则无法使用该角色权限</span>
    </dd>
  </dl> 
  <dl>
    <dt>系统权限</dt>
    <dd>
      <table border="0" cellspacing="0" cellpadding="0" class="border-table" width="98%">
        <thead>
          <tr>
            <th width="30%">导航名称</th>
            <th>权限分配</th>
            <th width="10%">全选</th>
          </tr>
        </thead>
        <tbody>
          <asp:Repeater ID="rptList" runat="server" onitemdatabound="rptList_ItemDataBound">
          <ItemTemplate>
          <tr>
            <td style="white-space:nowrap;word-break:break-all;overflow:hidden;">
              <asp:HiddenField ID="hidId" Value='<%#Eval("ModuleID") %>' runat="server" />
              <asp:HiddenField ID="hidName" Value='<%#Eval("ModuleCode") %>' runat="server" />
              <asp:HiddenField ID="hidActionType" Value='<%#Eval("ButtonAuthority") %>' runat="server" />
              <asp:HiddenField ID="hidLayer" Value='<%#Eval("ModuleLayer") %>' runat="server" />
              <asp:Literal ID="LitFirst" runat="server"></asp:Literal>
              <%#Eval("ModuleName")%>
            </td>
            <td>
              <asp:CheckBoxList ID="cblActionType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="cbllist"></asp:CheckBoxList>
            </td>
            <td align="center"><input name="checkAll" type="checkbox" /></td>
          </tr>
          </ItemTemplate>
          </asp:Repeater>
        </tbody>
      </table>
    </dd>
  </dl>
  <dl>
    <dt>网站权限</dt>
    <dd>*此权限需要勾选系统权限中的<b style=" color:Red;">内容管理</b>权限</dd>
    <dd>
      <table border="0" cellspacing="0" cellpadding="0" class="border-table" width="98%">
        <thead>
          <tr>
            <th width="30%">频道名称</th>
            <th>权限分配</th>
            <th width="10%">全选</th>
          </tr>
        </thead>
        <tbody>
          <asp:Repeater ID="rptList2" runat="server" onitemdatabound="rptList2_ItemDataBound">
          <ItemTemplate>
          <tr>
            <td style="white-space:nowrap;word-break:break-all;overflow:hidden;">
              <asp:HiddenField ID="hidId" Value='<%#Eval("Id") %>' runat="server" />
              <asp:HiddenField ID="hidColumnId" Value='<%#Eval("ColumnId") %>' runat="server" />
              <asp:HiddenField ID="hidActionType" Value='<%#Eval("Authority") %>' runat="server" />
              <asp:HiddenField ID="hidLayer" Value='<%#Eval("Layer") %>' runat="server" />
              <asp:Literal ID="LitFirst" runat="server"></asp:Literal>
              <%#Eval("Name")%>
            </td>
            <td>
              <asp:CheckBoxList ID="cblActionType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="cbllist"></asp:CheckBoxList>
            </td>
            <td align="center"><input name="checkAll" type="checkbox" /></td>
          </tr>
          </ItemTemplate>
          </asp:Repeater>
        </tbody>
      </table>
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
