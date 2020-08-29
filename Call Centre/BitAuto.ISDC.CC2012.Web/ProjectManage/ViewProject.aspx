<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewProject.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.ViewProject" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>项目查看</title>
    <link type="text/css" href="/css/base.css" rel="stylesheet" />
    <link type="text/css" href="/css/style.css" rel="stylesheet" />
    <script src="/Js/jquery-1.4.4.min.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("CTITool");
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(LoadLogInfo, 100);
        });

        function LoadLogInfo() {
            var ProjectID = '<%=ProjectID %>';
            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load('ProjectLog.aspx?r=' + Math.random() + "&ProjectID=" + ProjectID, null);
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load('ProjectLog.aspx?r=' + Math.random(), pody);
        }
    </script>
</head>
<body>
    <div class="w980">
        <div class="taskT">
            项目查看</div>
        <div class="content">
            <div class="titles bd ft14">
                基础信息</div>
            <div class="lineS">
            </div>
            <table border="0" cellspacing="0" cellpadding="0" class="xm_View_bs">
                <tr>
                    <th width="15%">
                        项目信息：
                    </th>
                    <td width="30%">
                        <asp:Literal ID="Literal_NM" runat="server"></asp:Literal>
                    </td>
                    <th width="15%">
                        项目分类：
                    </th>
                    <td width="30%">
                        <asp:Literal ID="Literal_Type" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <th>
                        项目状态：
                    </th>
                    <td>
                        <asp:Literal ID="Literal_Status" runat="server"></asp:Literal>
                    </td>
                    <th>
                        任务数据：
                    </th>
                    <td>
                        <asp:Literal ID="Literal_Total" runat="server"></asp:Literal>
                        （总计）/
                        <asp:Literal ID="Literal_End" runat="server"></asp:Literal>
                        （已提交）
                    </td>
                </tr>
                <tr>
                    <th>
                        项目说明：
                    </th>
                    <td colspan="3">
                        <div class="bzh">
                            <asp:Literal ID="Literal_Desc" runat="server"></asp:Literal>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="clearfix">
            </div>
            <!--问卷信息-->
            <% if (ShowSurvery)
               { %>
            <div class="titles bd ft14">
                问卷信息
            </div>
            <table border="0" cellspacing="0" cellpadding="0" class="xm_View_bs">
                <asp:Repeater ID="Repeater_Survery" runat="server">
                    <ItemTemplate>
                        <tr>
                            <th width="15%">
                                调查问卷：
                            </th>
                            <td width="30%">
                                <a href="/SurveyInfo/SurveyInfoView.aspx?SIID=<%#Eval("SIID")  %>" target="_blank">
                                    <%#Eval("SurveyName") %></a>
                            </td>
                            <th width="15%">
                                调查周期：
                            </th>
                            <td width="30%">
                                <%#BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToDateTime(Eval("BeginDate")).ToString("yyyy年MM月dd日") %>
                                —
                                <%#BitAuto.ISDC.CC2012.Entities.CommonFunction.ObjectToDateTime(Eval("EndDate")).ToString("yyyy年MM月dd日")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="clearfix">
            </div>
            <%} %>
            <!--问卷信息-->
            <!--外呼信息-->
            <div class="titles bd ft14">
                外呼信息</div>
            <table border="0" cellspacing="0" cellpadding="0" class="xm_View_bs">
                <asp:Literal ID="Literal_OutCall" runat="server"></asp:Literal>
            </table>
            <div class="clearfix">
            </div>
            <!--自动外呼信息-->
            <!--操作信息-->
            <div class="titles bd ft14">
                操作信息</div>
            <div class="cxList kp_ht" id="ajaxTable">
            </div>
            <!--操作信息-->
        </div>
    </div>
</body>
</html>
