﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="column_list.aspx.cs" Inherits="ccphl.Web.admin.article.column_list" %>
<%@ Import namespace="ccphl.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>内容类别</title>
<script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
<script type="text/javascript" src="../js/layout.js"></script>
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
</head>

<body class="mainbody">
<form id="form1" runat="server">
<asp:HiddenField ID="chid"  runat="server"/>
<asp:HiddenField ID="sid"  runat="server"/>
<!--导航栏-->
<div class="location">
  <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <span><%=Name %></span>
</div>
<!--/导航栏-->

<!--工具栏-->
<div class="toolbar-wrap">
  <div id="floatHead" class="toolbar">
    <div class="l-list">
      <ul class="icon-list">
        <li><a class="add" href="<%=EditName%>?action=<%=DTEnums.ActionEnum.Add %>&mid=<%=ModuleID%>&siteid=<%=this.siteid %>&channelid=<%=this.channelid %>&columntype=<%=this.columntype %>"><i></i><span>新增</span></a></li>
        <li><asp:LinkButton ID="btnSave" runat="server" CssClass="save" onclick="btnSave_Click"><i></i><span>保存</span></asp:LinkButton></li>
        <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
        <li><asp:LinkButton ID="btnDelete" runat="server" CssClass="del" OnClientClick="return ExePostBack('btnDelete','是否继续？');" onclick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>
      </ul>
    </div>
  </div>
</div>
<!--/工具栏-->

<!--列表-->
<asp:Repeater ID="rptList" runat="server" onitemdatabound="rptList_ItemDataBound">
<HeaderTemplate>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
  <tr>
    <th width="6%">选择</th>
    <th align="left" width="6%">编号</th>
    <th align="left">栏目名称</th>
    <th align="left" width="12%">调用别名</th>
    <th align="left" width="6%">状态</th>
    <th align="left" width="12%">排序</th>
    <th width="12%">操作</th>
  </tr>
</HeaderTemplate>
<ItemTemplate>
  <tr>
    <td align="center">
      <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" style="vertical-align:middle;" />
      <asp:HiddenField ID="hidId" Value='<%#Eval("ColumnID")%>' runat="server" />
      <asp:HiddenField ID="hidLayer" Value='<%#Eval("ColumnLayer") %>' runat="server" />
    </td>
    <td><%#Eval("ColumnID")%></td>
    
    <td>
      <asp:Literal ID="LitFirst" runat="server"></asp:Literal>
      <a href="<%=EditName%>?action=<%#DTEnums.ActionEnum.Edit %>&mid=<%=ModuleID%>&siteid=<%#this.siteid %>&channelid=<%#this.channelid %>&columntype=<%=this.columntype %>&id=<%#Eval("ColumnID")%>"><%#Eval("ColumnName")%></a>
    </td>
    <td><%#Eval("ColumnCode")%></td>
    <td><%#Eval("Status").ToString()=="0"?"禁用":"启用" %></td>
    <td><asp:TextBox ID="txtSortId" runat="server" Text='<%#Eval("OrderBy")%>' CssClass="sort" onkeydown="return checkNumber(event);" /></td>
    <td align="center">
      <a href="<%=EditName%>?action=<%#DTEnums.ActionEnum.Add %>&mid=<%=ModuleID%>&siteid=<%#this.siteid %>&channelid=<%#this.channelid %>&columntype=<%=this.columntype %>&id=<%#Eval("ColumnID")%>">添加子类</a>
      <a href="<%=EditName%>?action=<%#DTEnums.ActionEnum.Edit %>&mid=<%=ModuleID%>&siteid=<%#this.siteid %>&channelid=<%#this.channelid %>&columntype=<%=this.columntype %>&id=<%#Eval("ColumnID")%>">修改</a>
    </td>
  </tr>
</ItemTemplate>
<FooterTemplate>
  <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"7\">暂无记录</td></tr>" : ""%>
</table>
</FooterTemplate>
</asp:Repeater>
<!--/列表-->
</form>
</body>
</html>
