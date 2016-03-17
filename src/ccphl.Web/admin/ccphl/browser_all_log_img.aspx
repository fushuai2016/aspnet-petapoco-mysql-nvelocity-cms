<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="browser_all_log_img.aspx.cs" Inherits="ccphl.Web.admin.ccphl.browser_all_log_img" %>
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

    <!--列表-->
    <div class="line10"></div>
     <div class="line20">
    </div>
     <div class="nlist-2">
      <ul id="container" style="min-width: 310px; min-height:800px; margin: 0 auto; padding:10px 10px 10px 10px;">
      </ul>
    </div>
    <div class="line20"></div>
    </form>
    <script type="text/javascript">
        $(function () {
            chart0();
        });
        
        function chart0() {
           
            var url = "/tools/ccphl_ajax.ashx?action=get_browserinfo_all&type=all_browserinfo&r=" + Math.random();
            $.getJSON(url, function (data) {
                if (data.status == 1) {
                    setdata(data, data.xlist);
                }
            });
       }
       function setdata(data,categories) {
           var chart = {
               chart: {
                   type: 'column',
                   renderTo: 'container'
               },
               title: {
                   text: '各区县《为民服务站》使用情况统计'
               },
               xAxis: {
                   categories: categories
               },
               yAxis: {
                   min: 0,
                   title: {
                       text: '累计访问量'
                   },
                   stackLabels: {
                       enabled: true,
                       style: {
                           fontWeight: 'bold',
                           color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                       }
                   }
               },
               legend: {
                   align: 'right',
                   x: -70,
                   verticalAlign: 'top',
                   y: 20,
                   floating: true,
                   backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColorSolid) || 'white',
                   borderColor: '#CCC',
                   borderWidth: 1,
                   shadow: false
               },
               plotOptions: {
                   column: {
                       stacking: 'normal',
                       dataLabels: {
                           enabled: false,
                           color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white'
                       }
                   }
               },
               series: []
           };
           $.each(data.list, function (i, d) {
               var num = [];
               $.each(d.data, function (k, m) {
                   num.push([m]);
               });
               chart.series.push({ name: d.name, data: num });
           });
           var c = new Highcharts.Chart(chart);
       }
    </script>
    <script src="../../scripts/highcharts/js/highcharts.js" type="text/javascript"></script>
    <script src="../../scripts/highcharts/js/modules/exporting.src.js" type="text/javascript"></script>
    <script src="../../scripts/highcharts/js/themes/grid.js" type="text/javascript"></script>
</body>
</html>
