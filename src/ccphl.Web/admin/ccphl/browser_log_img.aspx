<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="browser_log_img.aspx.cs" Inherits="ccphl.Web.admin.ccphl.browser_log_img" %>
<%@ Import Namespace="ccphl.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>管理日志列表</title>
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>

    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
</head>

<body class="mainbody">
    <form id="form1" runat="server">
    <!--导航栏-->
    <div class="location">
        <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
        <a href="../center.aspx" class="home"><i></i><span>首页</span></a> <i class="arrow">
        </i><span><%=Name %></span>
    </div>
    <!--/导航栏-->

    <!--工具栏-->
    <div class="toolbar-wrap">
        <div id="floatHead" class="toolbar">
            <div class="l-list">
                <ul class="icon-list">
                    <li>
                       
                    </li>                    
                </ul>
            </div>
            <div class="r-list" style="border: #ddd solid 1px;">
                <asp:LinkButton ID="lbtnViewTxt" runat="server" CssClass="txt-view" onclick="lbtnViewTxt_Click" ToolTip="文字列表视图" />
            </div>
        </div>
    </div>
    <!--/工具栏-->

    <!--列表-->
    <div class="line10"></div>

    <div class="nlist-2">
      <ul id="container" style="min-width: 310px; height: 400px;max-width:650px; margin: 0 auto; padding:10px 10px 10px 10px;">
      </ul>
    </div>
    <div class="line20"></div>
    <div class="line20">
    </div>
     <div class="nlist-2">
      <ul id="container3" style="min-width: 310px; height: 400px; margin: 0 auto; max-width:650px; padding:10px 10px 10px 10px;">
      </ul>
    </div>
    <div class="line20"></div>
    <div class="line20">
    </div>
     <div class="nlist-2">
      <ul id="container2" style="min-width: 310px; height: 400px; margin: 0 auto;max-width:650px; padding:10px 10px 10px 10px;">
      </ul>
    </div>
    <div class="line20"></div>
     <div class="line20">
    </div>
     <div class="nlist-2">
      <ul id="container4" style="min-width: 310px; height: 800px; margin: 0 auto; padding:10px 10px 10px 10px;">
      </ul>
    </div>
    <div class="line20"></div>
    </form>
    <script type="text/javascript">
        $(function () {
            chart0();
            chart1();
            chart2();
            chart3();
        });
        function chart0() {
            var chart = new Highcharts.Chart({
                chart: {
                    renderTo: 'container',
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false

                },
                title: {
                    text: '浏览器使用分布'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            },
                            connectorColor: 'silver'
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: '浏览器占比'
                }]
            });
            var url = "/tools/ccphl_ajax.ashx?action=get_browserinfo&type=all_browserinfo&r="+Math.random();
            $.getJSON(url, function (data) {
                if (data.status == 1) {
                    browsers = [],
                    //迭代，把异步获取的数据放到数组中
                    $.each(data.list, function (i, d) {
                        browsers.push([d.name, d.share]);
                    });
                    chart.series[0].setData(browsers);
                }
            });
        }
        function chart1() {
            var chart1 = new Highcharts.Chart({
                chart: {
                    renderTo: 'container2',
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false

                },
                title: {
                    text: '操作系统使用分布'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            },
                            connectorColor: 'silver'
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: '操作系统占比'
                }]
            });
            var url = "/tools/ccphl_ajax.ashx?action=get_browserinfo&type=all_platforms&r=" + Math.random();
            $.getJSON(url, function (data) {
                if (data.status == 1) {
                    browsers = [],
                    //迭代，把异步获取的数据放到数组中
                    $.each(data.list, function (i, d) {
                        browsers.push([d.name, d.share]);
                    });
                    chart1.series[0].setData(browsers);
                }
            });
        }
        function chart2() {
            var chart2 = new Highcharts.Chart({
                chart: {
                    renderTo: 'container3',
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false

                },
                title: {
                    text: 'IE各版本使用分布'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            },
                            connectorColor: 'silver'
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: '占比'
                }]
            });
            var url = "/tools/ccphl_ajax.ashx?action=get_browserinfo&type=ie_ver&r=" + Math.random();
            $.getJSON(url, function (data) {
                if (data.status == 1) {
                    browsers = [],
                    $.each(data.list, function (i, d) {
                        browsers.push([d.name, d.share]);
                    });
                    chart2.series[0].setData(browsers);
                }
            });
        }
        function chart3() {
            var chart3={
                chart: {
                    type: 'column',
                    renderTo: 'container4',
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false

                },
                title: {
                    text: '操作系统——浏览器使用统计'
                },
                xAxis: {
                    categories: ['Windows XP', 'Windows 7', 'Windows 8', '其它操作系统']
                },

                yAxis: {
                    allowDecimals: false,
                    min: 0,
                    title: {
                        text: '用户数量'
                    }
                },

                tooltip: {
                    formatter: function () {
                        return '<b>' + this.x + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>' +
                    '总数: ' + this.point.stackTotal;
                    }
                },
                plotOptions: {
                    column: {
                        stacking: 'normal'
                    }
                },
                series: []
            };
            var url = "/tools/ccphl_ajax.ashx?action=get_browserinfo&type=platforms_browser&r=" + Math.random();
            $.getJSON(url, function (data) {
                if (data.status == 1) {
                    $.each(data.list, function (i, d) {
                        var num = [];
                        $.each(d.data, function (k, m) {
                            num.push([m]);
                        });
                        chart3.series.push({ name: d.name, data: num, stack: d.stack });
                    });
                    var chart = new Highcharts.Chart(chart3);
                }
            });
        }
    </script>
    <script src="../../scripts/highcharts/js/highcharts.js" type="text/javascript"></script>
    <script src="../../scripts/highcharts/js/modules/exporting.src.js" type="text/javascript"></script>
    <script src="../../scripts/highcharts/js/themes/grid.js" type="text/javascript"></script>
</body>
</html>
