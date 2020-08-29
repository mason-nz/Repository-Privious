<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCQualityStandardFiveLevelEdit.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard.UCQualityStandardFiveLevelEdit" %>
<script type="text/javascript">
    //录音质检保存
    function SaveQualityStandar() {
        Save("/AjaxServers/QualityStandard/ScoringManage/ScoringManage_CC.ashx");
    }
    //录音质检提交
    function SubQualityStandar() {
     if (checkisallchecked()) {
        Submit("/AjaxServers/QualityStandard/ScoringManage/ScoringManage_CC.ashx");
         }
        else {
            $.jAlert("存在未评分的项目，所以不能提交！");
        }
    }
    //对话质检保存
    function SaveQualityStandarIM() {
        Save("/AjaxServers/QualityStandard/ScoringManage/ScoringManage_IM.ashx");
    }
    //对话质检提交
    function SubQualityStandarIM() {
        if (checkisallchecked()) {
            Submit("/AjaxServers/QualityStandard/ScoringManage/ScoringManage_IM.ashx");
        }
        else {
            $.jAlert("存在未评分的项目，所以不能提交！");
        }
    }
    function checkisallchecked() {
        var hasdeadchecked = false;
        $("input[type='checkbox'][isdead='yes']").each(function () {
            if ($(this).attr("checked") != undefined) {
                if ($(this).attr("checked")) {
                    hasdeadchecked = true;
                }
            }
        });
        if (hasdeadchecked) {
            return true;
        }

        var allchoosed = true;
        $(".standardItemTable .pingfentr ul").each(function () {
            var choosed = false;
            $(this).find(" li").each(function () {
                if ($(this).css("display") != "none" && $($(this).find(" span")[0]).hasClass("current")) {
                    choosed = true;
                    return false;
                }
            });
            if (!choosed) {
                allchoosed = false;
                return false;
            }
        });
        return allchoosed;
    }


    //保存
    function Save(url) {
        var json = {
            action: "save",
            tableEndName: '<%=tableEndName %>',
            data: JSON.stringify(GetQualityStandarObj())
        };
        //        console.log(json);
        //        return false;
        //保存
        AjaxPost(url, json,
            function () {
                $.blockUI({ message: '正在处理，请等待...' });
            },
            function (data) {
                $.unblockUI();
                if (data.split('_')[0] == "success") {
                    $('#hdQs_RID').val(data.split('_')[1])
                    //$.jAlert("保存成功！");
                    $.jPopMsgLayer("保存成功！", function () {
                        //刷新父页面
                        execOpenerSearch();
                    });
                }
                else {
                    $.jAlert(data);
                }
            });
    }
    //提交
    function Submit(url) {
        var json = {
            action: "subsave",
            tableEndName: '<%=tableEndName %>',
            data: JSON.stringify(GetQualityStandarObj())
        };
        //        console.log(json);
        //        return false;
        //提交
        AjaxPost(url, json,
            function () {
                $.blockUI({ message: '正在处理，请等待...' });
            },
            function (data) {
                $.unblockUI();
                if (data.split('_')[0] == "success") {
                    //                    $.jAlert("提交成功！", function () {
                    //                        closePageExecOpenerSearch();
                    //                    });
                    $.jPopMsgLayer("提交成功！", function () {
                        closePageExecOpenerSearch();
                    });

                }
                else {
                    $.jAlert(data, function () {
                        closePageExecOpenerSearch();
                    });
                }
            });
    }

    //取页面评分信息
    function GetQualityStandarObj() {
        //扣分项
        var QS_ResultDetailList = new Array(); //问题列表   choosedstandardscore  choosedstandardmark
        $("input[name='choosedstandardmark']").each(function () {
            if ($(this).attr("sidname") != "") {
                var resultDetail = { QS_RDID: escape("0"),
                    ScoreType: escape($(this).attr("cbtype")),
                    QS_RTID: escape('<%=QS_RTID%>'),
                    QS_RID: escape($('#hdQs_RID').val()),
                    QS_CID: escape($(this).attr("cidname")),
                    QS_IID: escape($(this).attr("iidname")),
                    QS_SID: escape($(this).attr("sidname")),
                    QS_MID: 0,
                    QS_MID_End: -2,
                    Type: escape("1"),
                    ScoreDeadID: escape(""),
                    ScoreDeadID_End: escape(""),
                    Remark: escape($(this).val())
                };
                QS_ResultDetailList.push(resultDetail);
            }
        });
        //致命项
        $("input[type='checkbox'][isdead='yes']").each(function () {
            if ($(this).attr("checked") != undefined) {
                if ($(this).attr("checked")) {
                    var resultDetail = { QS_RDID: escape("0"),
                        ScoreType: escape("3"),
                        QS_RTID: escape('<%=QS_RTID%>'),
                        QS_RID: escape($('#hdQs_RID').val()),
                        QS_CID: escape(""),
                        QS_IID: escape(""),
                        QS_SID: escape(""),
                        QS_MID: escape(""),
                        QS_MID_End: escape(""),
                        Type: escape("2"),
                        ScoreDeadID: escape($(this).attr("value")),
                        ScoreDeadID_End: escape($(this).attr("value")),
                        Remark: escape($($(this).parent().parent().next().children()[0]).val())
                    };
                    QS_ResultDetailList.push(resultDetail);
                }
            }
        });

        //评分信息
        var QS_ResultoObj = {
            QS_RID: escape($('#hdQs_RID').val()),
            CallID: escape('<%=CallID%>'),
            CSID: escape('<%=CSID%>'),
            QS_RTID: escape('<%=QS_RTID%>'),
            ScoreType: escape('<%=ScoreType%>'),
            NoDeadNum: escape('<%=NoDeadMIdNum%>'),
            DeadNum: escape('<%=DeadMIdNum%>'),
            QS_ResultDetailList: QS_ResultDetailList,
            QualityAppraisal: escape($.trim($('#txtQualityInfo').val()))
        };

        return QS_ResultoObj
    }
    //load
    $(document).ready(function () {

        $("td[name='firsttd']").each(function () {
            var l_h = $(this).height(); //获取左侧的td的高度
            var l_h = Math.ceil(l_h) + 9;
            $($(this).next().children()[0]).css("height", l_h); //给右侧嵌套table添加属性（高）
        })
        initPingfen();
    });
    //初始化五级质检第一列汉子信息
    function initPingfen() {
        // standardItemTable  standardtr  pingfentr
        $(".standardItemTable").each(function () {
            var pingfentr = $(this).find(" .pingfentr");
            $(pingfentr).find(" li").each(function () {
                $(this).hide();
            });
            var remarksid = $(this).find(" input[name='choosedstandardmark']").attr("sidname");
            $(this).find(" .standardtr").each(function () {
                var sid = $(this).find(" td:eq(0) input[type=hidden]").attr("sidname");
                var ischeckstandard = false;
                if (remarksid == sid) {
                    ischeckstandard = true;
                }
                var textobj = $.trim($(this).find(" td:eq(0)").text());
                $(pingfentr).find(" li").each(function () {
                    if ($(this).html().indexOf(textobj) > 0) {
                        $(this).show();
                        if (ischeckstandard) {
                            if (!$(this).find(" span").hasClass("current")) {
                                $(this).find(" span").addClass("current");
                            }
                        }
                    }
                });

            });
        });
    }
    //控制显示
    function divShowHideEvent(divId, obj) {
        if ($(obj).hasClass("hide")) {
            //隐藏的
            $(obj).parent().siblings().show();
            $(obj).attr("class", "toggle2");
        }
        else {
            $(obj).parent().siblings().hide();
            $(obj).attr("class", "toggle2 hide");
        }
    }

    function choosestatus(thisObj, val) {
        //清楚所有选择
        $(thisObj).parent().parent().find(" span").each(function () {
            if ($(this).hasClass("current")) {
                $(this).removeClass("current");
            }
        });
        //选中指定项
        $(thisObj).addClass("current");
        changescorevalue(thisObj, val);
    }
    function changescorevalue(choosedObj, val) {
        var $thisTableObj = $(choosedObj).parent().parent().parent().parent().parent();
        var scoreval = $thisTableObj.find(" input:[name='standardscore" + val + "']").val();
        $thisTableObj.find(" input:[name='choosedstandardscore']").val(scoreval + "分").attr("sidname", $thisTableObj.find(" input:[name='standardscore" + val + "']").attr("sidname"));
        //alert($thisTableObj.find(" input:[name='choosedstandardscore']").val(scoreval + "分").attr("sidname"));  //choosedstandardmark
        $thisTableObj.find(" input:[name='choosedstandardmark']").attr("sidname", $thisTableObj.find(" input:[name='standardscore" + val + "']").attr("sidname"));
    }

    function checkmarklenght(obj) {
        var thisVal = $.trim($(obj).val());
        if (thisVal.length > 50) {
            $(obj).val(thisVal.substring(0, 50));
        }
        $(obj).css("border", "#CCC 1px solid");
    }
    function checkmarklenghtdown(obj) {
        var thisVal = $.trim($(obj).val());
        if (thisVal.length > 50) {
            $(obj).css("border", "#f00 1px solid");
        }
        else {
            $(obj).css("border", "#CCC 1px solid");
        }
    }

    //	 $(function () {

    //            $("#table td ul.pfbz_choose li a").hover(function () {
    //                var thisClassName = $(this).attr("class");
    //                //将此组评分按钮背景色全部重置为未选中时的灰色背景图片
    //                $(this).parent().parent().find(" a").css({ "background-image": "url(/Css/img/input-unchecked.png)", "background-repeat": "no-repeat" });
    //                //已经被选中，则背景色变成淡蓝色图片
    //                if (thisClassName == "current") {
    //                    $(this).css({ "background-image": "url(/Css/img/input-checked-hover.png)", "background-repeat": "no-repeat" });
    //                }
    //                //没有被选中，则背景色是淡灰色图片
    //                else {
    //                    $(this).css({ "background-image": "url(/Css/img/input-unchecked-hover.png)", "background-repeat": "no-repeat" });
    //                }
    //            }
    //            , function () {
    //                var thisClassName = $(this).attr("class");
    //                //将此组评分按钮背景色全部重置为未选中时的灰色背景图片
    //                $(this).parent().parent().find(" a").css({ "background-image": "url(/Css/img/input-unchecked.png)", "background-repeat": "no-repeat" });
    //                //将已经被选中，则背景色变成深蓝色图片
    //                $(this).parent().parent().find(" a[class='current']").css({ "background-image": "url(/Css/img/input-unchecked.png)", "background-repeat": "no-repeat" });
    //            });
    //        });
</script>
<input type="hidden" id="hdQs_RID" value='<%=QS_RID%>' />
<input type="text" id="hdrealQs_RID" visible="false" runat="server" />
<asp:Repeater ID="rp_Category" runat="server" OnItemDataBound="rp_Category_ItemDataBound">
    <ItemTemplate>
        <div class="lybase fwgf">
            <div class="title">
                <%#GetNum(Container.ItemIndex + 1,"1")%><%#Eval("Name")%><span style='<%=ScoreTypeFlag %>'>（<%#Eval("Score")%>分）</span>
                <a class="toggle2" onclick="javascript:divShowHideEvent('baseInfo',this)" href="javascript:void(0)"
                    style="*margin-top: -25px;"></a>
                <asp:Label ID="lblQS_CID" runat="server" Visible="false" Text='<%#Eval("QS_CID")%>'></asp:Label>
            </div>
            <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_Item_ItemDataBound">
                <ItemTemplate>
                    <p>
                        <%#GetNum(Container.ItemIndex + 1,"2")%><%#Eval("ItemName")%><span style='<%=ScoreTypeFlag %>'>（<%#Eval("Score")%>分）</span></p>
                    <asp:Label ID="lblQS_IID" runat="server" Visible="false" Text='<%#Eval("QS_IID")%>'></asp:Label>
                    <table class="standardItemTable" border="1" cellspacing="0" cellpadding="0" width="100%">
                        <asp:Repeater ID="rp_Standard" runat="server">
                            <ItemTemplate>
                                <tr class="standardtr">
                                    <td width="8%" class="bdlnone bdblue">
                                        <%#GetFiveLevelStandardName(Eval("SkillLevel").ToString())%>
                                        <input type="hidden" sidname='<%#Eval("QS_SID")%>' name="standardscore<%#Eval("SkillLevel").ToString()%>"
                                            value="<%#Eval("Score").ToString()%>" />
                                    </td>
                                    <td width="36%" style='text-align: left;'>
                                        <%#Eval("ScoringStandardName")%>
                                    </td>
                                    <td width="36%" style='text-align: left;'>
                                        <%#Eval("SExplanation")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr class="pingfentr">
                            <td width="8%" class="bdlnone bdblue">
                                评分
                            </td>
                            <td width="36%" style='text-align: left;'>
                                <ul class="pfbz_chooseedit">
                                    <li class="editfl"><span href="javascript:void(0)" onclick='javascript:choosestatus(this,5);'>优秀</span></li>
                                    <li class="editfl"><span href="javascript:void(0)" onclick='javascript:choosestatus(this,4);'>良好</span></li>
                                    <li class="editfl"><span href="javascript:void(0)" onclick='javascript:choosestatus(this,3);'>合格</span></li>
                                    <li class="editfl"><span href="javascript:void(0)" onclick='javascript:choosestatus(this,2);'>较差</span></li>
                                    <li class="editfl"><span href="javascript:void(0)" onclick='javascript:choosestatus(this,1);'>很差</span></li>
                                </ul>
                            </td>
                            <td width="36%" style='text-align: left;'>
                                <input title="此处输入长度限制为50个字符" sidname='<%#Eval("QS_SID")%>' iidname='<%#Eval("QS_IID")%>'
                                    cidname='<%#Eval("QS_CID")%>' cbtype='<%#Eval("ScoreType")%>' choosedstandardscore=""
                                    type="text" name="choosedstandardmark" onkeydown="checkmarklenghtdown(this);"
                                    onkeyup="checkmarklenght(this);" value="<%#GetStandardRemarkBySID(Eval("QS_SID").ToString()) %>"
                                    class="w400" />
                                <input type="text" name="choosedstandardscore" value="" style="border: 0px; width: 30px;
                                    display: none;" />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </ItemTemplate>
</asp:Repeater>
<div class="lybase fwgf" style="<%=haveDead%>">
    <div class="title">
        <%=GetNum("1")%>致命项<a class="toggle2" onclick="divShowHideEvent('baseInfo',this)"
            href="javascript:void(0)" style="*margin-top: -25px;"></a>
    </div>
    <table width="100%" cellspacing="0" cellpadding="0" border="1" style="margin-top: 10px;">
        <tbody>
            <asp:Repeater ID="rp_Dead" runat="server">
                <ItemTemplate>
                    <tr>
                        <td class="bdlnone zdq bd itemTitle">
                            <%#GetNum(Container.ItemIndex + 1,"2")%><%#Eval("DeadItemName")%>
                        </td>
                        <td width="8%">
                            <label>
                                <input id="cbDead" name="" type="checkbox" <%#IsExistResultDetail(Eval("QS_DAID").ToString(),"2")%>
                                    isdead="yes" value="<%#Eval("QS_DAID")%>" class="dx" /></label>
                        </td>
                        <td width="20%" class="borderR">
                            <input type="text" id="DeadRemark" value="<%#GetMarkedRemeak(Eval("QS_DAID").ToString(),"2")%>"
                                class="wsr" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</div>
<div class="lybase fwgf" style="<%=haveQulity%>">
    <div class="title">
        <%=GetNum("2")%>质检评价 <a class="toggle2" onclick="divShowHideEvent('baseInfo',this)"
            href="javascript:void(0)" style="*margin-top: -25px;"></a>
    </div>
    <div class="pj">
        <textarea rows="" cols="" name="" id="txtQualityInfo"><%=txtQualityInfo%></textarea>
    </div>
</div>
