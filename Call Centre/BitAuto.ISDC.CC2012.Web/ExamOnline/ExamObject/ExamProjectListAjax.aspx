<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamProjectListAjax.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ExamOnline.ExamObject.ExamProjectListAjax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>异步加载考试项目列表</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="bit_table" id="ajaxTable">
        <div class="optionBtn mt10" style="cursor: auto;">
            <div class="new">
                <a target="_blank" style="background: url(/css/img/addExamObj.gif) scroll 0 0; float: right;"
                    href="/ExamOnline/ExamObject/ExamProjectEdit.aspx"></a>
            </div>
        </div>
        <table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
            <tr class="bold" style="text-align: center;">
                <th width="30%">
                    项目名称
                </th>
                <th width="10%">
                    参考人数
                </th>
                <th width="15%">
                    创建人
                </th>
                <th width="20%">
                    考试时间
                </th>
                <th width="10%">
                    状态
                </th>
                <th width="15%">
                    操作
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("Name").ToString()%>
                            &nbsp;
                        </td>
                        <td>
                            <%#Eval("JoinNum")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("UserName")%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("ExamStartTime").ToString(),"yyyy-MM-dd")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("State")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Contral")%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <br />
        <!--分页-->
        <div class="pages1" style="text-align: right; margin-bottom: 5px; clear: both;">
            <table style="width: 100%;">
                <tr>
                    <td style="text-align: right;">
                        <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
                        <input type="hidden" id="currentPageNum" value='<%=currentPageNum %>' />
                    </td>
                </tr>
            </table>
            <script type="text/javascript">
                $(document).ready(function () {
                    $("a[name='modfiyRole']").click(function (e) {
                        //alert(e);
                        e.preventDefault();
                        ModfiyRole($(this).attr("useridstr"),
                     $(this).attr("userName"), $(this).attr("dataright"), $(this).attr("agentNum"));
                    });

                    $("#pageHiddenMain").val($('#currentPageNum').val());
                });

                function ModfiyRole(userId, userNames, dataright, agentNum) {
                    $.openPopupLayer({
                        name: "updateUsersRigth",
                        parameters: { "userNames": escape(userNames), userIDs: userId, single: 'true', dataright: dataright, agentNum: agentNum },
                        url: "UpdateUsersRight.aspx",
                        beforeClose: function (e, data) {
                            if (e) {
                                search();
                            }
                        }
                    });
                }

                function checkboxCon() {
                    if ($("#checkboxCon").attr("checked")) {
                        $('[name=UserID]:checkbox').attr("checked", true);
                    }
                    else {
                        $('[name=UserID]:checkbox').attr("checked", false);
                    }
                }
            </script>
            <script language="javascript" type="text/javascript">


                function getCheckUserID() {
                    var checkUsers = $(":checkbox[name^='UserID'][checked=true]");
                    var length = checkUsers.length;
                    var userIDs = "";
                    for (var i = 0; i < length; i++) {
                        userIDs += checkUsers.eq(i).val() + ",";
                    }
                    if (userIDs.length > 0) {
                        userIDs = userIDs.substring(0, userIDs.length - 1);
                    }
                    return userIDs;
                }
            </script>
        </div>
    </div>
    </form>
</body>
</html>
