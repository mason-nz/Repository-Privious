<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuestionAnsweredDetails.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib.QuestionAnsweredDetails" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
        $(function () {

            $('#tableReturnVisitCust tr:even').addClass('color_hui'); //设置列表行样式
            $("#tableReturnVisitCust th").css({ "line-height": "18px" });
            SetTableStyle('tableReturnVisitCust');
        });

        //查询之后，回调函数
        function LoadDivSuccess(data) {
            $('#tableReturnVisitCust tr:even').addClass('color_hui'); //设置列表行样式
            SetTableStyle('tableReturnVisitCust');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

<div id="divList" class="bit_table">
        <!--列表开始-->
        <div class="faqList">
            <asp:Repeater runat="server" ID="Rt_Question">
                <ItemTemplate>
                    <ul>
                        <li class="bt"  style="  position:relative;"><b>Q：</b>                
                            <span><%#Eval("CONTENT") %></span> 
                            <a <%#Eval("Status") == null || Eval("Status").ToString() == "0" ? "style='position:absolute; top:0px; right:80px;display:block;float:right'":"style='display:none'"%>  onclick="DeleteClick('<%#Eval("id")%>')">删除</a></li>
 
                        <li <%#Eval("Status") == null || Eval("Status").ToString() == "0" ? "style='display:none'":"style='display:block'"%> ><b>A：</b> <span>
                            <%#Eval("answerContent")%></span> </li>
                        <li <%#Eval("Status") == null || Eval("Status").ToString() == "0"  ? "style='display:none'":"style='display:block'"%> class="lb">
                            <span style=" vertical-align:bottom;display:block; float:right;">
                                解答人：<%#Eval("TrueName")%> 
                            </span>
                            <span style=" vertical-align:middle;display:block; float:right;">
                                解答时间：<%#Convert.ToDateTime(Eval("LastModifyDate").ToString()).ToString("yyyy-MM-dd") == "1900-01-01" ? "" : Convert.ToDateTime(Eval("LastModifyDate").ToString()).ToString("yyyy-MM-dd hh:mm:ss")%>
                            </span>  &nbsp;
        
                        </li>
                    </ul>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!--分页-->
        <div class="pageTurn mr10">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
        <input type="hidden" id="hidRecordCount"  value="<%=RecordCount %>"/>
        <input type="hidden" id="hidNotAnswer" value="<%=NotAnswerCount%>"/>
    </div>
      </form>
</body>
</html>
