<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SurveyMappingList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.SurveyInfo.SurveyMapping.SurveyMappingList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            //            $(".hrefExport").each(function () {
            //                $(this).attr("href", $(this).attr("href") + "&Browser=" + GetBrowserName())
            //            });
        });

        function exportdata(_siid, _projectid, _typeid) {
            $.openPopupLayer({
                name: "ExportFilterPanel",
                parameters: { SIID: _siid, ProjectID: _projectid, typeID: _typeid },
                url: "ExportFilter.aspx",
                afterClose: function (e, data) {
                }
            }
                     );
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="bit_table">
        <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
            <tr>
                <th>
                    问卷名称
                </th>
                <th>
                    关联项目
                </th>
                <th>
                    状态
                </th>
                <th>
                    填写数量
                </th>
                <th>
                    操作
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <a target='_blank' href='../../SurveyInfo/SurveyInfoView.aspx?SIID=<%#Eval("SIID")%>'
                                class="linkBlue">
                                <%#Eval("SName")%>&nbsp; </a>
                        </td>
                        <td>
                            <%#Eval("PName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("PStatus")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("CountAnswer")%>&nbsp;
                        </td>
                        <td>
                            <%#getOperator(Eval("PStatus").ToString(), Eval("SIID").ToString(), Eval("ProjectID").ToString(),Eval("Source").ToString())%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <!--分页-->
        <div class="pageTurn mr10">
            <p>
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </p>
        </div>
    </div>
    </form>
</body>
</html>
