<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamProjectView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ExamOnline.ExamObject.ExamProjectView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>考试项目查看</title>
    <script type="text/javascript" src="../../Js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="../../Js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="../Js/common.js" language="javascript" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">

        $(document).ready(function () {
            $("#aExport").click(function (e) {
                e.preventDefault();

                var eiid = $("#hidEIID").val();
                if (eiid == "") {
                    $.jAlert("没有找到项目ID信息，不能导出成绩");
                }
                else {
                    $("#formExport").submit();
                }
            });
        });
      
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            查看考试项目</div>
        <div class="addzs">
            <ul class="clearfix" style="display: block; float: none;">
                <li>
                    <label>
                        考试项目名称：</label>&nbsp<span id="txtTitle" runat="server"><%=model.Name%></span></li>
                <li>
                    <label>
                        所属分组：</label>&nbsp<span id="Span14" runat="server"><%=GetBusinessGroupNameById(model.BGID.Value)%></span></li>
                <li>
                    <label>
                        项目分类：</label>&nbsp<span id="Span1" runat="server"><%=GetCatageName(model.ECID.ToString())%></span></li>
                <li>
                    <label>
                        项目说明：</label>&nbsp<span id="Span2" runat="server"><%=model.Description %></span></li>
                <li>
                    <label>
                        考试范围：</label>&nbsp<span id="Span3" runat="server"><%=model.BusinessGroup%></span></li>
                <li>
                    <label>
                        考试试卷：</label>&nbsp<span id="Span4" runat="server"> <a href="/ExamOnline/ExamPaperStorage/ExamPaperView.aspx?epid=<%=model.EPID%>">
                            <%=examPaper.Name %>
                        </a></span></li>
                <li>
                    <label>
                        考生姓名：</label>&nbsp<span id="Span5" runat="server">
                            <%=EmployeeNames%></span></li>
                <li>
                    <label>
                        考试时间：</label>&nbsp<span id="Span6" runat="server">
                            <%=model.ExamStartTime%>
                            到
                            <%=model.ExamEndTime%>
                        </span></li>
                <li>
                    <label>
                        是否补考：</label>&nbsp<span id="Span7" runat="server"><% =(model.IsMakeUp==1?"是":"否") %></span></li>
                <%if (model.IsMakeUp == 1)
                  { %>
                <li>
                    <label>
                        补考试卷：</label>&nbsp<span id="Span8" runat="server"> <a href="/ExamOnline/ExamPaperStorage/ExamPaperView.aspx?epid=<%=examPaper_Make.EPID%>">
                            <%=examPaper_Make.Name%>
                        </a></span></li>
                <li>
                    <label>
                        考生姓名：</label>&nbsp<span id="Span9" runat="server"><%=EmployeeNames_Make%></span></li>
                <li>
                    <label>
                        补考时间：</label>&nbsp<span id="Span10" runat="server">
                            <%=examInfo_Make.MakeUpExamStartTime.ToString() %>
                            到
                            <%=examInfo_Make.MakeupExamEndTime.ToString()%>
                        </span></li>
                <%} %>
                <li>
                    <label>
                        成绩：</label>&nbsp<span id="Span11"> <a href="#" id="aExport">导出考试成绩</a> </span>
                </li>
                <li>
                    <label>
                        创建人：</label>&nbsp<span id="Span12" runat="server"><%= BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(model.CreaetUserID)) %></span></li>
                <li>
                    <label>
                        创建时间：</label>&nbsp<span id="Span13" runat="server"><%= model.CreateTime.ToString() %></span></li>
                <li class="attach"></li>
            </ul>
        </div>
    </div>
    </form>
    <%--用于导出的Form--%>
    <form id="formExport" action="/ExamOnline/ExamObject/DataExport.aspx" method="post">
    <input type="hidden" id="hidEIID" name="hidEIID" value="" runat="server" />
    <input type="hidden" id="hidEName" name="hidEName" value="" runat="server" />
    </form>
</body>
</html>
