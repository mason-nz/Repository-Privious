<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MessageOnline.aspx.cs"
    Inherits="BitAuto.DSC.IM2014.Server.Web.MessageOnline" %>

<%@ Register Src="Controls/Top.ascx" TagName="Top" TagPrefix="uc1" %>
<%@ Register Src="Controls/Bottom.ascx" TagName="Bottom" TagPrefix="uc2" %>
<%@ Register Src="Controls/ControlManage.ascx" TagName="ControlManage" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>在线留言</title>
    <script src="/js/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/jquery.jmpopups-0.5.1.pack.js"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='Scripts/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script type="text/javascript">
        loadJS("common");
    </script>
    <script type="text/javascript">
        //        $(function () {
        //            $("#mb1").append("<li><a href='DialogueOnline.aspx'>在线对话</a></li>").append("<li><a href='#' class='cur'>在线留言</a></li>");
        //        });

        $(document).ready(function () {
            TextBind();
        });
        //分页
        function ShowDataByPost1(pody) {
            var url = 'MessageOnline.aspx #mycontent';
            $("#mycontent").load(url, pody, function () {
                TextBind();
            });
        }
        //把前端输入的内容，原封不动显示出来，控件的text属性将不解释html字符，html()将解释html字符
        function TextBind() {
            $("td[name='UserName']").each(function () {
                if ($(this).attr("mst") != "") {
                    $(this).text(decodeURIComponent($(this).attr("mst")));
                }
                else {
                    $(this).html("&nbsp;");
                }
            });
            $("td[name='ContentName']").each(function () {
                $(this).text(decodeURIComponent($(this).attr("mst")));
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="crm">
        <div class="head">
            <uc1:Top ID="Top1" runat="server" />
            <div class="mainmenu_bottom">
                <ul class="" id="mb1">
                    <uc3:ControlManage ID="ControlManageSet1" runat="server" />
                </ul>
                <div class="clearfix">
                </div>
            </div>
        </div>
        <div class="content" id="mycontent">
            <!--列表开始-->
            <div class="cxList" style="margin-top: 8px; height: auto;">
                <table border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <th width="10%">
                            访客ID
                        </th>
                        <th width="10%">
                            类型
                        </th>
                        <th width="10%">
                            邮箱
                        </th>
                        <th width="8%">
                            姓名
                        </th>
                        <th width="10%">
                            手机
                        </th>
                        <th width="12%">
                            时间
                        </th>
                        <th width="40%">
                            留言内容
                        </th>
                    </tr>
                    <asp:Repeater ID="repeaterTableList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("UserID").ToString().Substring(Eval("UserID").ToString().LastIndexOf("-") + 1, 12)%>&nbsp;
                                </td>
                                <td>
                                    <%#getType(Eval("TypeID").ToString())%>&nbsp;
                                </td>
                                <td>
                                    <%#Eval("Email")%>&nbsp;
                                </td>
                                <td name="UserName" mst="<%#GetValue(Eval("UserName").ToString())%>">
                                </td>
                                <td>
                                    <%#Eval("Phone")%>&nbsp;
                                </td>
                                <td>
                                    <%#Eval("CreateTime")%>&nbsp;
                                </td>
                                <td name="ContentName" mst="<%#GetValue(Eval("Content").ToString())%>">
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div class="it_page" style="float: right; margin: 10px;" id="itPage">
                <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
            </div>
            <!--列表结束-->
            <div class="clearfix">
            </div>
        </div>
        <uc2:Bottom ID="Bottom1" runat="server" />
    </div>
    </form>
</body>
</html>
