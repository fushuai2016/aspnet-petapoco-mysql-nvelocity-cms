<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="role_list.aspx.cs" Inherits="ccphl.Web.admin.manager.role_list" %>
<%@ Import namespace="ccphl.Common" %>
<%@ Import Namespace="Model" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title><%=Name%>列表</title>
<script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
<script type="text/javascript" src="../js/layout.js"></script>
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<link  href="../../css/pagination.css" rel="stylesheet" type="text/css" />
</head>

<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="manager_list.aspx"><span>管理员</span></a>
  <i class="arrow"></i>
  <span><%=Name%>列表</span>
</div>
<!--/导航栏-->

<!--工具栏-->
<div class="toolbar-wrap">
  <div id="floatHead" class="toolbar">
    <div class="l-list">
      <ul class="icon-list">
        <li><a class="add" href="<%=EditName%>?action=<%=DTEnums.ActionEnum.Add %>&mid=<%=ModuleID %>"><i></i><span>新增</span></a></li>
        <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
        <li><asp:LinkButton ID="btnDelete" runat="server" CssClass="del" OnClientClick="return ExePostBack('btnDelete','本操作会删除角色及相关权限，是否继续？');" onclick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>
      </ul>
    </div>
    <div class="r-list">
      <asp:TextBox ID="txtKeywords" runat="server" CssClass="keyword" />
      <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn-search" onclick="btnSearch_Click">查询</asp:LinkButton>
    </div>
  </div>
</div>
<!--/工具栏-->

<!--列表-->
<asp:Repeater ID="rptList" runat="server">
<HeaderTemplate>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
  <tr>
    <th width="8%">选择</th>
    <th align="left">角色名称</th>
    <th width="12%" align="left">角色状态</th>
    <th width="12%">操作</th>
  </tr>
</HeaderTemplate>
<ItemTemplate>
  <tr>
    <td align="center">
      <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" Enabled='true' style="vertical-align:middle;" />
      <asp:HiddenField ID="hidId" Value='<%#Eval("RoleID")%>' runat="server" />
    </td>
    <td><a href="role_edit.aspx?action=<%#DTEnums.ActionEnum.Edit %>&mid=<%=ModuleID %>&id=<%#Eval("RoleID")%>"><%#Eval("RoleName")%></a></td>
    <td><%#((system_role)Container.DataItem).Status ==1?"启用":"禁用"%></td>
    <td align="center"><a href="<%=EditName%>?action=<%#DTEnums.ActionEnum.Edit %>&mid=<%=ModuleID %>&id=<%#Eval("RoleID")%>">修改</a></td>
  </tr>
</ItemTemplate>
<FooterTemplate>
  <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"4\">暂无记录</td></tr>" : ""%>
</table>
</FooterTemplate>
</asp:Repeater>
<!--/列表-->
</form>
</body>
</html>
