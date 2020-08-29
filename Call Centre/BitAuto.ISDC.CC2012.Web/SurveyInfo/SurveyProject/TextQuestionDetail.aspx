<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TextQuestionDetail.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyProject.TextQuestionDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>调查结果详情</title>
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../../Js/common.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script src="../../Js/json2.js" type="text/javascript"></script>
    <link href="../../css/base.css" type="text/css" rel="stylesheet" />
    <link href="../../css/style.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $("#chkFilterNull").bind("change", function () {
                search(1);
            });
            //search();
        });

        //查询
        function search(page) {
            var pody = _params(page);
            LoadingAnimation("tableList");
            $(".bit_table").load("TextQuestionDetail.aspx .bit_table > *", pody);
        }
        //获取参数
        function _params(page) { 
            var filterNull = "";
            if ($("#chkFilterNull").is(":checked")) {
                filterNull = $("#chkFilterNull").val();
            }

            var answerContent = encodeURIComponent($.trim($("#txtAnswerContent").val()));

            var pody = {
                SPIID: '<%=RequestSPIID %>',
                SQID: '<%=RequestSQID %>',
                SOID: '<%=RequestSOID %>',
                FilterNull: filterNull,
                AnswerContent: answerContent,
                page: page,
                r: Math.random()
            };

            return pody;
        }

        //分页操作
        function ShowDataByPost1(page) {
            var pody = _params(page.substr(page.length - 1, 1));
            LoadingAnimation("tableList");
            $('.bit_table').load('TextQuestionDetail.aspx .bit_table > *', pody);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            调查问卷调查结果</div>
        <br />
        <div class="search zsk">
            <ul>
                <li style="font-size: 13px; padding: 0 0 10px 58px;">问题：<%=questionStr %>
                </li>
            </ul>
            <ul>
                <li style="padding-left: 58px">
                    <input type="checkbox" name="chkFilterNull" id="chkFilterNull" value="1" />过滤空选项
                    <span class="keywords">
                        <input style="" type="text" value="" class="w220" name="txtAnswerContent" id="txtAnswerContent" /></span>
                    <span class="btnsearch">
                        <input name="" type="button" onclick="search(1)" value="搜 索" id="btnsearch" /></span>
                </li>
            </ul>
        </div>
        <div class="bit_table">
            <br />
            <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList"
                style="padding-left: 10px">
                <tr>
                    <th style="width: 8%">
                        姓名
                    </th>
                    <th>
                        提交时间
                    </th>
                    <th>
                        答案文本
                    </th>
                    <th>
                        查看问卷
                    </th>
                </tr>
                <asp:Repeater ID="repeaterTableList" runat="server">
                    <ItemTemplate>
                        <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                            <td>
                                <%#getCreateUserName(Eval("CreateUserID").ToString())%>&nbsp;
                            </td>
                            <td>
                               <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                            </td>
                            <td>
                                <%#getContent(Eval("AnswerContent").ToString())%>&nbsp;
                            </td>
                            <td>
                                <%if (right_view)
                                  {%>
                                <a target="_blank" href="../PersonalSurveyInfoView.aspx?SPIID=<%#Eval("SPIID").ToString()%>&UserID=<%#Eval("CreateUserID").ToString() %>">
                                    查看问卷</a>&nbsp;
                                <%} %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <br />
            <!--分页-->
            <div class="pageTurn mr10">
                <p>
                    <asp:Literal runat="server" ID="litPagerDown"></asp:Literal>
                </p>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
