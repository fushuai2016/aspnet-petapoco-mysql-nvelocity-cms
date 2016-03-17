<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="channel_list.aspx.cs" Inherits="ccphl.Web.admin.channel.channel_list" %>
<%@ Import namespace="ccphl.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title><%=Name%>列表</title>
<script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
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
  <span><%=Name%>列表</span>
</div>
<!--/导航栏-->

<!--工具栏-->
<div class="toolbar-wrap">
  <div id="floatHead" class="toolbar">
    <div class="l-list">
      <ul class="icon-list">
        <li><a class="add" href="<%=EditName%>?action=<%=DTEnums.ActionEnum.Add %>&mid=<%=ModuleID%>"><i></i><span>新增</span></a></li>
        <li><asp:LinkButton ID="btnSave" runat="server" CssClass="save" onclick="btnSave_Click"><i></i><span>保存</span></asp:LinkButton></li>
        <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
        <li><asp:LinkButton ID="btnDelete" runat="server" CssClass="del" OnClientClick="return ExePostBack('btnDelete','如果该频道下还存在文章则无法删除，是否继续？');" onclick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>
      </ul>
      <div class="menu-list">
        <div class="rule-single-select">
          <asp:DropDownList ID="ddlCategoryId" runat="server" AutoPostBack="True" onselectedindexchanged="ddlCategoryId_SelectedIndexChanged"></asp:DropDownList>
        </div>
      </div>
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
    <th width="6%">选择</th>
    <th align="left" width="18%">频道名称</th>
    <th align="left" width="20%">所属站点</th>
    <th align="left" width="12%">分页数量</th>
    <th align="left" width="12%">系统频道</th>
    <th align="left" width="6%">状态</th>
    <th align="left" width="12%">排序</th>
    <th width="10%">操作</th>
  </tr>
</HeaderTemplate>
<ItemTemplate>
  <tr>
    <td align="center"><asp:CheckBox ID="chkId" CssClass="checkall" runat="server" Enabled='<%#Eval("IsSystem").ToString()=="0"?true:false %>' style="vertical-align:middle;" /><asp:HiddenField ID="hidId" Value='<%#Eval("ChannelID")%>' runat="server" /></td>
    <td><%#Eval("ChannelName")%></td>
    <td><%#GetSiteName(int.Parse(Eval("SiteID").ToString()))%></td>
    <td><%#Eval("PageSize")%></td>
    <td><%#Eval("IsSystem").ToString()=="0"?"否":"是" %></td>
    <td><%#Eval("Status").ToString()=="0"?"禁用":"启用" %></td>
    <td><asp:TextBox ID="txtSortId" runat="server" Text='<%#Eval("OrderBy")%>' CssClass="sort" onkeydown="return checkNumber(event);" /></td>
    <td align="center"><a href="<%=EditName%>?action=<%#DTEnums.ActionEnum.Edit %>&mid=<%=ModuleID %>&id=<%#Eval("ChannelID")%>">修改</a></td>
  </tr>
</ItemTemplate>
<FooterTemplate>
  <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"8\">暂无记录</td></tr>" : ""%>
</table>
</FooterTemplate>
</asp:Repeater>
<!--/列表-->

<!--内容底部-->
<div class="line20"></div>
<div class="pagelist">
  <div class="l-btns">
    <span>显示</span><asp:TextBox ID="txtPageNum" runat="server" CssClass="pagenum" onkeydown="return checkNumber(event);" ontextchanged="txtPageNum_TextChanged" AutoPostBack="True"></asp:TextBox><span>条/页</span>
  </div>
  <div id="PageContent" runat="server" class="default"></div>
</div>
<!--/内容底部-->
</form>
</body>
</html>
