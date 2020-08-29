<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyOnline.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.QuestionnaireSurvey.SurveyOnline" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function openpage(_spiid) {
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/SurveyInfo/QuestionnaireSurvey/TakingAnSurvey.aspx?SPIID=' + _spiid));

            } catch (e) {
                window.open("TakingAnSurvey.aspx?SPIID=" + _spiid);
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="bit_table" id="divList">
        <asp:Repeater ID="repeaterTableList" runat="server" OnItemDataBound="repeaterTableList_ItemDataBound">
            <ItemTemplate>
                <div class="zskList">
                    <div class="bt">
                        <img src="../../images/unread.png" alt="" id="img<%#Eval("SPIID") %>" />&nbsp;<b><span
                            style="color: #0088CC"><%#getTitle(Eval("Name").ToString())%></span></b> <em>调查结束时间：
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("SurveyEndTime").ToString())%>
                            </em><%#showOperBtn(Eval("SPIID").ToString())%></div>
                    <p>
                        <%#getContent(Eval("Description").ToString())%>
                    </p>
                    <input type="hidden" name="ajaxPage_isSubmit" value="<%#surveyIsSubmit(Eval("SPIID").ToString()) %>" />
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <!--分页-->
        <div class="pageTurn mr10">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
        <input type="hidden" runat="server" id="hidSurveyAll" />
        <input type="hidden" runat="server" id="hidSurveying" />
        <input type="hidden" runat="server" id="hidImgID" />
    </div>
    </form>
</body>
</html>
