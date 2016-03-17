<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="region_edit.aspx.cs" Inherits="ccphl.Web.admin.ccphl.region_edit"
    ValidateRequest="false" %>

<%@ Import Namespace="ccphl.Common" %>
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
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
    $(function () {
        //初始化表单验证
        //$("#form1").initValidform();
        //初始化上传控件
        $(".upload-img").each(function () {
            $(this).InitSWFUpload({ sendurl: "../../tools/upload_ajax.ashx", flashurl: "../../scripts/swfupload/swfupload.swf",siteid:<%=siteConfig.SiteID %>,thumbnail:true });
        });
    });
    
    </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
    <!--导航栏-->
    <div class="location">
        <a href="<%=ListName%>" class="back"><i></i><span>返回列表页</span></a> <a href="../center.aspx"
            class="home"><i></i><span>首页</span></a> <i class="arrow"></i><a href="<%=ListName%>">
                <span>
                    <%=Name%>列表</span></a> <i class="arrow"></i><span>编辑<%=Name%></span>
    </div>
    <div class="line10">
    </div>
    <!--/导航栏-->
    <!--内容-->
    <div class="content-tab-wrap">
        <div id="floatHead" class="content-tab">
            <div class="content-tab-ul-wrap">
                <ul>
                    <li><a href="javascript:;" onclick="tabs(this);" class="selected">
                        <%=Name%>基本信息</a></li>
                </ul>
            </div>
        </div>
    </div>
    <div class="tab-content">
        <input type="hidden" id="hidparentid" runat="server"/>
        <dl style=" display:none;">
            <dt>上级站点</dt>
            <dd>
                <div class="rule-single-select">
                    <asp:DropDownList ID="ddlParentId" runat="server">
                    </asp:DropDownList>
                </div>
            </dd>
        </dl>
         <dl >
            <dt>上级站点</dt>
            <dd>
                <asp:TextBox ID="parentid" runat="server" CssClass="input normal" datatype="*2-50"
                    sucmsg=" " />
            </dd>
        </dl>
        <dl>
            <dt>站点名称</dt>
            <dd>
                <asp:TextBox ID="regionname" runat="server" CssClass="input normal" datatype="*2-50"
                    sucmsg=" " />
                <span class="Validform_checktip">*</span>
            </dd>
        </dl>
        <dl>
            <dt>站点代码</dt>
            <dd>
                <asp:TextBox ID="regioncode" runat="server" CssClass="input normal" datatype="n2-20"
                    errormsg="请填写正确的代码" sucmsg=" "></asp:TextBox>
                <span class="Validform_checktip">站点代码，只允许数字</span>
            </dd>
        </dl>
        <dl>
            <dt>站点地址</dt>
            <dd>
                <asp:TextBox ID="regionurl" runat="server" CssClass="input normal" datatype="url"
                    errormsg=" " sucmsg=" "></asp:TextBox>
                <span class="Validform_checktip">站点的链接地址</span>
            </dd>
        </dl>
        <dl>
            <dt>是否开通</dt>
            <dd>
                <div class="rule-single-checkbox">
                    <asp:CheckBox ID="cbIsOpen" runat="server" />
                </div>
            </dd>
        </dl>
        <dl>
            <dt>是否禁用</dt>
            <dd>
                <div class="rule-single-checkbox">
                    <asp:CheckBox ID="cbIsLock" runat="server" />
                </div>
            </dd>
        </dl>
        <dl>
            <dt>机构代码</dt>
            <dd>
                <asp:TextBox ID="departmentid" runat="server" CssClass="input normal" datatype="n" />
                <span class="Validform_checktip"></span>
            </dd>
        </dl>
        <dl>
            <dt>党组织代码</dt>
            <dd>
                <asp:TextBox ID="organcode" runat="server" CssClass="input normal" datatype="n0-20" />
                <span class="Validform_checktip"></span>
            </dd>
        </dl>
        <dl>
            <dt>图标</dt>
            <dd>
                <asp:TextBox ID="icon" runat="server" CssClass="input normal" datatype="*0-20" />
                <span class="Validform_checktip"></span>
            </dd>
        </dl>
        <dl>
            <dt>图片封面</dt>
            <dd>
                <asp:TextBox ID="imgurl" runat="server" CssClass="input normal upload-path" />
                <div class="upload-box upload-img">
                </div>
            </dd>
        </dl>
        <dl>
            <dt>地图经度</dt>
            <dd>
                <asp:TextBox ID="longitude" runat="server" CssClass="input normal" datatype="n0-20" />
                <span class="Validform_checktip"></span>
            </dd>
        </dl>
        <dl>
            <dt>地图纬度</dt>
            <dd>
                <asp:TextBox ID="latitude" runat="server" CssClass="input normal" datatype="n0-20" />
                <span class="Validform_checktip"></span>
            </dd>
        </dl>
        <dl>
            <dt>天气地区代码</dt>
            <dd>
                <asp:TextBox ID="weathercode" runat="server" CssClass="input normal" datatype="*0-20" />
                <span class="Validform_checktip"></span>
            </dd>
        </dl>
        <dl>
            <dt>客服端IP地址</dt>
            <dd>
                <asp:TextBox ID="clientip" runat="server" CssClass="input normal" datatype="/^\s*$|^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$/" />
                <span class="Validform_checktip"></span>
            </dd>
        </dl>
        <dl>
            <dt>远程教育地址</dt>
            <dd>
                <asp:TextBox ID="DistanceEducationUrl" runat="server" CssClass="input normal" datatype="url" />
                <span class="Validform_checktip"></span>
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
            <dt>备注</dt>
            <dd>
                <asp:TextBox ID="remark" runat="server" CssClass="input normal" datatype="*0-200"
                    TextMode="MultiLine" />
                <span class="Validform_checktip"></span>
            </dd>
        </dl>
    </div>
    <!--工具栏-->
    <div class="page-footer">
        <div class="btn-list">
            <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" OnClick="btnSubmit_Click" />
            <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript:history.back(-1);" />
        </div>
        <div class="clear">
        </div>
    </div>
    <!--/工具栏-->
    </form>
</body>
</html>
