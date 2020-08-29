<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamPaperList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ExamPaperStorage.ExamPaperList" %>

<script type="text/javascript">

    function DelPage(epid) {
        $.jConfirm("确定要删除此试卷吗？", function (isOk) {
            if (isOk) {
                //删除试卷

                var pody = { action: escape('DelPager'), epid: escape(epid) };
                AjaxPost('/AjaxServers/ExamOnline/ExamPaperStorage/ExamPaper.ashx', pody, null,
                             function (data) {
                                 if (data == "success") {
                                     LoadingAnimation("ajaxTable");
                                     $("#ajaxTable").load("/AjaxServers/ExamOnline/ExamPaperStorage/ExamPaperList.aspx", GetPody())
                                 }
                                 else {
                                     $.jAlert(data);
                                 }
                             });
            }
        });
    }

    $(document).ready(function () {

        $("a[name='delPager']").click(function (e) { e.preventDefault(); DelPage($(this).attr("epid")); });

    });
    
</script>
<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th>
            分类
        </th>
        <th>
            试卷名称
        </th>
        <th>
            所属分组
        </th>
        <th>
            创建时间
        </th>
        <th>
            创建人
        </th>
        <th>
            状态
        </th>
        <th>
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>                 
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">                      
                        <td>                           
                          <%# Eval("ECName")%>&nbsp;
                        </td>
                        <td>
                                 <%#Eval("Name")%>&nbsp;
                        </td>
                        <td>
                                 <%#Eval("BGName")%>&nbsp;
                        </td>                           
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                        </td>
                        <td>
                            <%#getCreateUserName(Eval("CreaetUserID").ToString())%>&nbsp;
                        </td>
                        <td>
                           <%# GetStataByID(Eval("Status").ToString())%>&nbsp;
                        </td>
                        <td>
                           <%# GetButtonHtml(Eval("Status").ToString(),Eval("EPID").ToString()) %>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
</table>
<br />
<!--分页-->
<div class="pages1" style="text-align: right;">
    <uc:AjaxPager ID="AjaxPager_Custs" runat="server" ContentElementId="ajaxTable" />
</div>
