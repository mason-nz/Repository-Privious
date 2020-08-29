<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueueList.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.QueueList"  
    MasterPageFile="~/Controls/Top.Master" Title="排队中" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <script type="text/javascript">

        setInterval(loadData, 3000);

        function loadData() {
            $('#divcontent').load("../AjaxServers/QueueManage/QueueList.aspx");
        };

        $(function () {
            loadData();

        });
        
    </script>
    <div class="content">
        <!--列表开始-->
        <div class="cxList" style="margin-top: 20px;" id="divcontent">
            <%--<table border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <th width="24%">
                        经销商名称
                    </th>
                    <th width="13%">
                        地理位置
                    </th>
                    <th width="8%">
                        所属城市群
                    </th>
                    <th width="8%">
                        连接时间
                    </th>
                    <th width="10%">
                        上次最后消息时间
                    </th>
                    <th width="10%">
                        上次访问时间
                    </th>
                    <th width="7%">
                        访问次数
                    </th>
                    <th width="20%">
                        最近访问页面
                    </th>
                </tr>
                <tr>
                    <td class="cName">
                        张家口庞大信诚汽车销售服务有限公司
                    </td>
                    <td>
                        江苏省 南京市 南京市
                    </td>
                    <td>
                        石家庄区
                    </td>
                    <td>
                        13:11:06
                    </td>
                    <td>
                        13:21:06
                    </td>
                    <td>
                        2014-9-10
                    </td>
                    <td>
                        10
                    </td>
                    <td class="cName">
                        什么是汽车通？
                    </td>
                </tr>
                <tr>
                    <td class="cName">
                        张家口庞大信诚汽车销售服务有限公司
                    </td>
                    <td>
                        江苏省 南京市
                    </td>
                    <td>
                        石家庄区
                    </td>
                    <td>
                        13:11:06
                    </td>
                    <td>
                        13:21:06
                    </td>
                    <td>
                        2014-9-10
                    </td>
                    <td>
                        10
                    </td>
                    <td class="cName">
                        什么是汽车通？
                    </td>
                </tr>
                <tr>
                    <td class="cName">
                        张家口庞大信诚汽车销售服务有限公司
                    </td>
                    <td>
                        江苏省 南京市
                    </td>
                    <td>
                        石家庄区
                    </td>
                    <td>
                        13:11:06
                    </td>
                    <td>
                        13:21:06
                    </td>
                    <td>
                        2014-9-10
                    </td>
                    <td>
                        10
                    </td>
                    <td class="cName">
                        什么是汽车通？
                    </td>
                </tr>
                <tr>
                    <td class="cName">
                        张家口庞大信诚汽车销售服务有限公司
                    </td>
                    <td>
                        江苏省 南京市
                    </td>
                    <td>
                        石家庄区
                    </td>
                    <td>
                        13:11:06
                    </td>
                    <td>
                        13:21:06
                    </td>
                    <td>
                        2014-9-10
                    </td>
                    <td>
                        10
                    </td>
                    <td class="cName">
                        什么是汽车通？
                    </td>
                </tr>
                <tr>
                    <td class="cName">
                        张家口庞大信诚汽车销售服务有限公司
                    </td>
                    <td>
                        江苏省 南京市
                    </td>
                    <td>
                        石家庄区
                    </td>
                    <td>
                        13:11:06
                    </td>
                    <td>
                        13:21:06
                    </td>
                    <td>
                        2014-9-10
                    </td>
                    <td>
                        10
                    </td>
                    <td class="cName">
                        什么是汽车通？
                    </td>
                </tr>
                <tr>
                    <td class="cName">
                        张家口庞大信诚汽车销售服务有限公司
                    </td>
                    <td>
                        江苏省 南京市
                    </td>
                    <td>
                        石家庄区
                    </td>
                    <td>
                        13:11:06
                    </td>
                    <td>
                        13:21:06
                    </td>
                    <td>
                        2014-9-10
                    </td>
                    <td>
                        10
                    </td>
                    <td class="cName">
                        什么是汽车通？
                    </td>
                </tr>
                <tr>
                    <td class="cName">
                        张家口庞大信诚汽车销售服务有限公司
                    </td>
                    <td>
                        江苏省 南京市
                    </td>
                    <td>
                        石家庄区
                    </td>
                    <td>
                        13:11:06
                    </td>
                    <td>
                        13:21:06
                    </td>
                    <td>
                        2014-9-10
                    </td>
                    <td>
                        10
                    </td>
                    <td class="cName">
                        什么是汽车通？
                    </td>
                </tr>
                <tr>
                    <td class="cName">
                        张家口庞大信诚汽车销售服务有限公司
                    </td>
                    <td>
                        江苏省 南京市
                    </td>
                    <td>
                        石家庄区
                    </td>
                    <td>
                        13:11:06
                    </td>
                    <td>
                        13:21:06
                    </td>
                    <td>
                        2014-9-10
                    </td>
                    <td>
                        10
                    </td>
                    <td class="cName">
                        什么是汽车通？
                    </td>
                </tr>
                <tr>
                    <td class="cName">
                        张家口庞大信诚汽车销售服务有限公司
                    </td>
                    <td>
                        江苏省 南京市
                    </td>
                    <td>
                        石家庄区
                    </td>
                    <td>
                        13:11:06
                    </td>
                    <td>
                        13:21:06
                    </td>
                    <td>
                        2014-9-10
                    </td>
                    <td>
                        10
                    </td>
                    <td class="cName">
                        什么是汽车通？
                    </td>
                </tr>
                <tr>
                    <td class="cName">
                        张家口庞大信诚汽车销售服务有限公司
                    </td>
                    <td>
                        江苏省 南京市
                    </td>
                    <td>
                        石家庄区
                    </td>
                    <td>
                        13:11:06
                    </td>
                    <td>
                        13:21:06
                    </td>
                    <td>
                        2014-9-10
                    </td>
                    <td>
                        10
                    </td>
                    <td class="cName">
                        什么是汽车通？
                    </td>
                </tr>
            </table>--%>
        </div>
        <!--列表结束-->
        <!--分页开始-->
        <%--<div class="pagesnew">
            <span class="pre">上一页</span> <a href="#" class="active">1</a> <a href="#">2</a>
            <a href="#">3</a> <a href="#">4</a> <a href="#">5</a> <span>...</span> <span><a href="#"
                class="next">下一页</a></span> <span>共50页</span> <span>到第</span><input type="text" value="" /><span>页</span>
            <span class="qd_go"><a href="#">GO</a></span>
        </div>--%>
        <!--分页结束-->
        <div class="clearfix">
        </div>        
    </div>
</asp:Content>
