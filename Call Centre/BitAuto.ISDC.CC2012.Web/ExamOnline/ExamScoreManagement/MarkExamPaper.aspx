<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarkExamPaper.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ExamOnline.ExamScoreManagement.MarkExamPaper" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=ViewType%></title>
    <link href="../../Css/base.css" type="text/css" rel="stylesheet" />
    <link href="../../css/style.css" type="text/css" rel="stylesheet" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../../Js/common.js" type="text/javascript"></script>
    <script src="../../Js/json2.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $("span[name='selectItem']").each(function () {

                var id = $(this).attr("cid");
                var selected = $(this).attr("par");
                var retVal = GetSelect(id, selected);
                $(this).text(retVal);

            });
            $("input[type='text']").each(function () {
                $(this).keyup(function (event) {
                    if (((event.keyCode >= 48) && (event.keyCode <= 57)) || ((event.keyCode >= 96) && (event.keyCode <= 106)) || (event.keyCode == 190 || event.keyCode == 110)) {
                    } else {
                        $(this).attr("value", "");
                    }
                });
            });

            $("img").each(function () {
                var img = $(this);
                img.hide();
                var KLQID = $(this).attr("id");
                var KLQIDText = "";
                $("span[name='selectItem'][cid='" + KLQID + "']").each(function () {
                    var selected = $(this).text();
                    if (KLQIDText == "" && selected != undefined && selected != "") {
                        KLQIDText = selected;
                    }
                    else if (KLQIDText != "" && selected != "") {
                        if (selected == KLQIDText) {
                            img.show();

                            $('#' + KLQID).attr("src", "../../Images/right.png");
                        }
                        else {
                            img.show();

                            $('#' + KLQID).attr("src", "../../Images/wrong.png");
                        }
                    }

                });

            });

        });
        function GetSelect(KLQID, selected) {
            var returnstr = "";
            if (selected != "") {
                var str = ",";
                if (selected.indexOf(str) > 0) {
                    //debugger
                    var myselected = new Array()
                    myselected = selected.split(",");

                    $("input[type='checkbox'][name='" + KLQID + "']").each(function () {

                        //debugger
                        for (var i = 0; i < myselected.length; i++) {
                            if ($(this).val() == myselected[i]) {
                                returnstr += $(this).attr("valuea");
                            }
                        }
                    });
                }
                else {
                    $("input[name='" + KLQID + "']").each(function () {

                        if ($(this).val() == selected) {
                            returnstr = $(this).attr("valuea");
                        }
                    });
                }
            }
            return returnstr;
        }

        function check() {


            var subscore = "";
            var flag = true;
            $("input[type='text']").each(function () {
                //debugger
                if ($(this).val() == "") {
                    flag = false;
                    alert("请给每一个主观题打分");
                    return flag;
                }
                else {
                    var KLQID = $(this).attr("id")
                    var BQID = $(this).attr("name");
                    var SmallScore = $(this).val();
                    var maxfen = $(this).attr("maxvalue");

                    if (parseInt(SmallScore) > parseInt(maxfen)) {
                        alert("每题得分不能大于该小题分值！");
                        flag = false;
                        return flag;
                    }
                }
            });
            return flag
        }

        function subinfo() {

            if ($('#hdsubhave').val() != "") {
                $.jAlert("正在处理中不能重复提交！");
            }
            else {
                if (check()) {

                    if ($.jConfirm("确认要提交吗？", function (r) {
                        if (r) {

                            $('#hdsubhave').val("1");
                            var subscore = "";
                            var EOLID = '<%=EOLID%>';
                            $("input[type='text']").each(function () {
                                var KLQID = $(this).attr("id")
                                var BQID = $(this).attr("name");
                                var SmallScore = $(this).val();
                                subscore += BQID + ":" + KLQID + ":" + SmallScore + ",";
                            });
                            if (subscore != "") {
                                subscore = subscore.substring(0, subscore.length - 1);
                            }
                            params = {
                                Action: "SubScore",
                                subscore: encodeURIComponent(subscore),
                                EOLID: encodeURIComponent(EOLID)

                            }
                            AjaxPost("../../AjaxServers/ExamOnline/ExamScoreManage/ExamScoreManagement.ashx", params, null,
                    function (data) {
                        $('#hdsubhave').val("");
                        if (data == 'success') {
                            //$.jAlert("提交成功！", function () { closePage(); });
                            $.jPopMsgLayer("提交成功！", function () { closePage(); });
                        }
                        else {
                            $.jAlert(data);
                        }
                    });
                        }
                    }));
                }
            }
        }
    </script>
</head>
<body>
    <input type="hidden" id="hdsubhave" value="" />
    <div class="w980">
        <div class="taskT">
            <%=ViewType%></div>
        <div class="examBt1">
            <b style="font-family: 宋体; font-size: 20px">
                <%=ExamperName%></b></div>
        <div class="examBt2">
            考生姓名：<%=username%></div>
        <div class="fenshu" id="fenshu" runat="server">
            <p>
                <%=SumScore%></p>
        </div>
        <div class="addzs">
            <asp:Repeater ID="repeaterTableList" runat="server" OnItemDataBound="repeaterTableList_ItemDataBound">
                <ItemTemplate>
                    <div class="title bold examT">
                        <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"-")%><%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.Name.ToString()%>：<span><%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.BQDesc%></span><span>（共<%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.QuestionCount.ToString()%>题，每题<%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.EachQuestionScore.ToString()%>分，共<%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.QuestionCount * ((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.EachQuestionScore%>分）</span></div>
                    <asp:Label ID="lblAskCategory" runat="server" Visible="false" Text="<%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.AskCategory%>"></asp:Label>
                    <asp:Label ID="lblBQID" runat="server" Visible="false" Text="<%#((BitAuto.ISDC.CC2012.Entities.ExamBigQuestioninfo)(Container.DataItem)).ExamBigQuestion.BQID%>"></asp:Label>
                    <asp:Repeater ID="repeaterRadio" OnItemDataBound="repeaterRadio_ItemDataBound" runat="server"
                        Visible="false">
                        <ItemTemplate>
                            <div class="st">
                                <ul>
                                    <li class="xzt">
                                        <label>
                                            <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"1")%></label><span class="sjbt"><%#Eval("Ask")%><span
                                                class="bold">（<span class="bold" name="selectItem" cid='<%#Eval("KLQID")%>' par='<%# GetSelectedByID(Eval("BQID").ToString(),Eval("KLQID").ToString())%>'>
                                                </span>）</span><img id="<%#Eval("KLQID")%>" src="" /></span>
                                        <ul class="clearfix w800">
                                            <asp:Repeater ID="repeatersonradio" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <input name="<%#Eval("KLQID")%>" type="radio" bqid="<%#Eval("BQID")%>" valuea="<%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"A")%>"
                                                            value="<%#Eval("KLAOID")%>" class="dt" /><%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"a")%><span><%#Eval("Answer")%></span></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                        <div class="title bold conrect">
                                            正确答案：<span name="selectItem" cid='<%#Eval("KLQID")%>' par='<%# GetRightByID(Eval("KLQID").ToString())%>'></span></div>
                                    </li>
                                </ul>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="repeaterCheckbox" OnItemDataBound="repeaterCheckbox_ItemDataBound"
                        runat="server" Visible="false">
                        <ItemTemplate>
                            <div class="st">
                                <ul>
                                    <li class="xzt">
                                        <label>
                                            <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"1")%></label><span class="sjbt"><%#Eval("Ask")%><span
                                                class="bold">（ <span class="bold" name="selectItem" cid='<%#Eval("KLQID")%>' par='<%# GetSelectedByID(Eval("BQID").ToString(),Eval("KLQID").ToString())%>'>
                                                </span>）</span><img id="<%#Eval("KLQID")%>" src="" /></span>
                                        <ul class="clearfix w800">
                                            <asp:Repeater ID="repeatersonCheckbox" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <input name="<%#Eval("KLQID")%>" type="checkbox" valuea="<%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"A")%>"
                                                            bqid="<%#Eval("BQID")%>" value="<%#Eval("KLAOID")%>" class="dt" /><%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"a")%><span><%#Eval("Answer")%></span></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                        <div class="title bold conrect">
                                            正确答案：<span name="selectItem" cid='<%#Eval("KLQID")%>' par='<%# GetRightByID(Eval("KLQID").ToString())%>'></span></div>
                                    </li>
                                </ul>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="repeaterask" Visible="false" runat="server">
                        <ItemTemplate>
                            <div class="faq examt">
                                <ul class="clearfix">
                                    <li>
                                        <label>
                                            <%#ConvertStrForNeed((Container.ItemIndex + 1).ToString(),"1")%></label><span class="sjbt"><%#Eval("Ask")%></span>
                                    </li>
                                    <li class="answer2">
                                        <label>
                                            提交的答案：</label><span class="tjda"><%# GetSelectedByID(Eval("BQID").ToString(),Eval("KLQID").ToString())%></span></li>
                                    <li class="answer2 blueColor">
                                        <label>
                                            得分：</label><span><%if (come == "3" && IsMarking != "1")%><%{%>
                                                <input type="text" id="<%#Eval("KLQID").ToString()%>" name="<%#Eval("BQID")%>" value=""
                                                    class="w90" maxvalue="<%#GetMaxFen(Eval("BQID").ToString())%>" /><%}%><%else
                                                               {%><%# Getfenshu(Eval("BQID").ToString(), Eval("KLQID").ToString())%><%} %>分</li>
                                    <li class="answer2 blueColor">
                                        <label>
                                            参考答案：</label><span class="tjda"><%# GetRightByIDWenda(Eval("KLQID").ToString())%></span></li>
                                </ul>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="btn" style="margin: 20px auto;">
        <%if (come == "3" && IsMarking != "1")%><%{%>
        <input type="button" onclick="subinfo()" name="" value="提 交" />&nbsp;&nbsp;
        <input type="button" name="" value="取 消" onclick="closePage()" />&nbsp;&nbsp;
        <%}
          else
          {%>
        <input type="button" name="" value="关 闭" onclick="closePage()" />&nbsp;&nbsp;
        <%}%>
    </div>
</body>
</html>
