﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="site_list.aspx.cs" Inherits="ccphl.Web.admin.settings.site_list" %>
<%@ Import namespace="ccphl.Common" %>
<%@ Import Namespace="Model" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>站点管理</title>
<script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
<script type="text/javascript" src="../js/layout.js"></script>
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
</head>

<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <span>站点管理</span>
</div>
<!--/导航栏-->

<!--工具栏-->
<div class="toolbar-wrap">
  <div id="floatHead" class="toolbar">
    <div class="l-list">
      <ul class="icon-list">
        <li><a class="add" href="site_edit.aspx?action=<%=DTEnums.ActionEnum.Add %>&mid=<%=ModuleID %>"><i></i><span>新增</span></a></li>
        <li><asp:LinkButton ID="btnSave" runat="server" CssClass="save" onclick="btnSave_Click"><i></i><span>保存</span></asp:LinkButton></li>
        <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
        <li><asp:LinkButton ID="btnDelete" runat="server" CssClass="del" OnClientClick="return ExePostBack('btnDelete','本操作会删除本导航及下属子导航，是否继续？');" onclick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>
      </ul>
    </div>
  </div>
</div>
<!--/工具栏-->

<!--列表-->
<asp:Repeater ID="rptList" runat="server" 
    onitemdatabound="rptList_ItemDataBound" >
<HeaderTemplate>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
  <tr>
    <th width="8%">选择</th>
    <th align="left" width="12%">站点ID</th>
    <th align="left">站点名称</th>
    <th width="8%">站点代码</th>
    <th width="8%">状态</th>
    <th align="left" width="12%">排序</th>
    <th width="12%">操作</th>
  </tr>
</HeaderTemplate>
<ItemTemplate>
  <tr>
    <td align="center">
      <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" Enabled='false' style="vertical-align:middle;" />
      <asp:HiddenField ID="hidId" Value='<%#((system_site)Container.DataItem).SiteID%>' runat="server" />
      <asp:HiddenField ID="hidLayer" Value='<%#((system_site)Container.DataItem).SiteLayer%>' runat="server" />
    </td>
    <td style="white-space:nowrap;word-break:break-all;overflow:hidden;"><%#((system_site)Container.DataItem).SiteID%></td>
    <td style="white-space:nowrap;word-break:break-all;overflow:hidden;">
      <asp:Literal ID="LitFirst" runat="server"></asp:Literal>
      <a href="site_edit.aspx?action=<%#DTEnums.ActionEnum.Edit %>&mid=<%=ModuleID %>&id=<%#((system_site)Container.DataItem).SiteID%>"><%#((system_site)Container.DataItem).SiteName%></a>
      <%#string.IsNullOrEmpty(((system_site)Container.DataItem).DomainName) ? "" : "(域名：" + ((system_site)Container.DataItem).DomainName + ")"%>
    </td>
    <td align="center"><%#((system_site)Container.DataItem).SiteCode%></td>
    <td align="center"><%#((system_site)Container.DataItem).Status == 1 ? "启用" : "禁用"%></td>
    <td><asp:TextBox ID="txtSortId" runat="server" Text='<%#((system_site)Container.DataItem).OrderBy%>' CssClass="sort" onkeydown="return checkNumber(event);" /></td>
    <td align="center" style="white-space:nowrap;word-break:break-all;overflow:hidden;">
      <a href="site_edit.aspx?action=<%#DTEnums.ActionEnum.Add %>&mid=<%=ModuleID %>&id=<%#((system_site)Container.DataItem).SiteID%>">添加子站点</a>
      <a href="site_edit.aspx?action=<%#DTEnums.ActionEnum.Edit %>&mid=<%=ModuleID %>&id=<%#((system_site)Container.DataItem).SiteID%>">修改</a>
    </td>
  </tr>
</ItemTemplate>
<FooterTemplate>
  <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"8\">暂无记录</td></tr>" : ""%>
</table>
</FooterTemplate>
</asp:Repeater>
<!--/列表-->
</form>
</body>
</html>
