<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamList" %>
    <div class="bit_table" id="divList">
        <asp:Repeater ID="repeaterTableList" runat="server">
            <ItemTemplate>
                <div class="zskList">
                    <div class="bt">
                        <img src="../images/unread.png" alt="" id="img<%#Eval("eiEPID") %>" />&nbsp;<b><span style="color:#0088CC"><%#Eval("epName").ToString()%></span></b>
                        <em>考试结束时间：<%#Eval("eiEndTime").ToString()%></em><%#showOperBtn(Eval("eiEndTime").ToString(), Eval("ExamType").ToString(), Eval("eiEIID").ToString(), Eval("eiEPID").ToString())%></div>
                    <p class="grayColor">
                        <%#Eval("epExamDesc")%></p>
                    <input type="hidden" value="<%#IsEndTimeOver(Eval("eiEndTime").ToString(), Eval("ExamType").ToString(), Eval("eiEIID").ToString()) %>" />
                </div>
            </ItemTemplate>
        </asp:Repeater> 
        <!--分页-->
        <div class="pageTurn mr10">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
        <input type="hidden" runat="server" id="hidTestOver" />
        <input type="hidden" runat="server" id="hidNoTest" />
    </div>
    <script type="text/javascript">
        function ViewExamPaper(epidStr, objectIDStr, comeStr, examTypeStr) {
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/ExamOnline/ExamScoreManagement/MarkExamPaper.aspx?epid=' + epidStr + '&eiid=' + objectIDStr + '&come=' + comeStr + '&type=' + examTypeStr));
            }
            catch (e) {
                window.open('/ExamOnline/ExamScoreManagement/MarkExamPaper.aspx?epid=' + epidStr + '&eiid=' + objectIDStr + '&come=' + comeStr + '&type=' + examTypeStr, '', 'height=900,width=1050,left=200,toolbar=no,menubar=no,scrollbars=yes,resizable=no,location=no,status=no');
            }
        }
        function StartExam(objectID, examType) {
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/ExamOnline/TakingAnExam.aspx?eiid=' + objectID + '&type=' + examType));
            }
            catch (e) {
                window.open("TakingAnExam.aspx?eiid=" + objectID + "&type=" + examType);
            }
           
        }
    </script>