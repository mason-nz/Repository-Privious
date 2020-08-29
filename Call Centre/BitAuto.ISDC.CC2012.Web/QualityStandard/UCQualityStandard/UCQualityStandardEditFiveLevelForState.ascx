<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCQualityStandardEditFiveLevelForState.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard.UCQualityStandardEditFiveLevelForState" %>

<script type="text/javascript">
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
    //复审-录音
    function SubQualityStandar() {
            return SubState("/AjaxServers/QualityStandard/ScoringManage/ScoringManage_CC.ashx");
    }
    //复审-对话
    function SubQualityStandarIM() {
            return SubState("/AjaxServers/QualityStandard/ScoringManage/ScoringManage_IM.ashx");
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


    function SubState(url) {
        var flag = false;
        var json = {
            action: "substate",
            data: JSON.stringify(GetQualityStandarObj())
        };
        //审核
        //console.log(json);
        //return false;
        AjaxPostAsync(url, json, null,
            function (data) {
                if (data.split('_')[0] == "success") {
                    flag = true;

                }
                else {
                    flag = false;
                    $.jAlert(data);
                }
            });
        return flag;
    }

    //取页面评分信息
    function GetQualityStandarObj() {
        //扣分项
        var QS_ResultDetailList = new Array(); //问题列表
        if ('<%=ScoreType%>' == "3") {
            $("input[type='text'][name='choosedstandardmark']").each(function () {
                 var resultDetail = { QS_RDID: escape($(this).attr("rdidname")),
                    ScoreType: escape($(this).attr("cbtype")),
                    QS_RTID: escape('<%=QS_RTID%>'),
                    QS_RID: escape($('#hdQs_RID').val()),
                    QS_CID: escape($(this).attr("cidname")),
                    QS_IID: escape($(this).attr("iidname")),
                    QS_SID: escape($(this).attr("sidname")),
                    QS_MID: escape("0"),
                    QS_MID_End: escape("0"),
                    Type: escape("1"),
                    ScoreDeadID: escape(""),
                    ScoreDeadID_End: escape(""),
                    Remark: escape($(this).val())
                };
                QS_ResultDetailList.push(resultDetail);
            });
        }
        else {
            $("input[type='checkbox'][isdead='no']").each(function () {

                var QS_MIDValue = -2;
                var QS_MID_EndValue = -2;
                if ($(this).attr("rdidname") != "" && $(this).attr("rdidname") != "0") {
                    QS_MIDValue = $(this).attr("value");
                }
                if ($(this).attr("checked") != undefined) {

                    if ($(this).attr("checked")) {
                        QS_MID_EndValue = $(this).attr("value");
                    }
                }
                var resultDetail = { QS_RDID: escape($(this).attr("rdidname")),
                    ScoreType: escape($(this).attr("cbtype")),
                    QS_RTID: escape('<%=QS_RTID%>'),
                    QS_RID: escape($('#hdQs_RID').val()),
                    QS_CID: escape($(this).attr("cidname")),
                    QS_IID: escape($(this).attr("iidname")),
                    QS_SID: escape($(this).attr("sidname")),
                    QS_MID: escape(QS_MIDValue),
                    QS_MID_End: escape(QS_MID_EndValue),
                    Type: escape("1"),
                    ScoreDeadID: escape(""),
                    ScoreDeadID_End: escape(""),
                    Remark: escape("")
                };
                QS_ResultDetailList.push(resultDetail);
            });
        }
        //致命项
        $("input[type='checkbox'][isdead='yes']").each(function () {
            var ScoreDeadIDValue = -2;
            var ScoreDeadID_EndValue = -2;
            if ($(this).attr("rdidname") != "" && $(this).attr("rdidname") != "0") {
                ScoreDeadIDValue = $(this).attr("value");
            }
            if ($(this).attr("checked") != undefined) {

                if ($(this).attr("checked")) {
                    ScoreDeadID_EndValue = $(this).attr("value");
                }
            }

            var resultDetail = { QS_RDID: escape($(this).attr("rdidname")),
                ScoreType: escape("1"),
                QS_RTID: escape('<%=QS_RTID%>'),
                QS_RID: escape($('#hdQs_RID').val()),
                QS_CID: escape(""),
                QS_IID: escape(""),
                QS_SID: escape(""),
                QS_MID: escape(""),
                QS_MID_End: escape(""),
                Type: escape("2"),
                ScoreDeadID: escape(ScoreDeadIDValue),
                ScoreDeadID_End: escape(ScoreDeadID_EndValue),
                Remark: escape("")

            };
            QS_ResultDetailList.push(resultDetail);
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
                var standardtitle =  $.trim($(this).find(" td:eq(0)").text())
                $(pingfentr).find(" li").each(function () {
                    if ($(this).html().indexOf(standardtitle) > 0) {
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

    $(function () {
        initPingfen();
    });

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
</script>
<div class="pfb">
    <input type="hidden" id="hdQs_RID" value='<%=QS_RID%>' />
    <asp:Repeater ID="rp_Category" runat="server" OnItemDataBound="rp_Category_ItemDataBound">
        <ItemTemplate>
            <div class="lybase fwgf">
                <div class="title">
                    <%#GetNum(Container.ItemIndex + 1,"1")%><%#Eval("Name")%><span style='<%=ScoreTypeFlag %>'>（<%#Eval("Score")%>分）</span>
                    <a class="toggle2" onclick="divShowHideEvent('baseInfo',this)" href="javascript:void(0)"
                        style="*margin-top: -25px;"></a>
                    <asp:Label ID="lblQS_CID" runat="server" Visible="false" Text='<%#Eval("QS_CID")%>'></asp:Label>
                </div>
                <asp:Repeater ID="rp_Item" runat="server" OnItemDataBound="rp_Item_ItemDataBound">
                    <ItemTemplate>
                        <p>
                            <%#GetNum(Container.ItemIndex + 1,"2")%><%#Eval("ItemName")%><span style='<%=ScoreTypeFlag %>'>（<%#Eval("Score")%>分）</span>
                        </p>
                        <asp:Label ID="lblQS_IID" runat="server" Visible="false" Text='<%#Eval("QS_IID")%>'></asp:Label>
                        <table class="standardItemTable" border="1" cellspacing="0" cellpadding="0" width="100%">
                            <asp:Repeater ID="rp_Standard" runat="server">
                                <ItemTemplate>
                                    <tr class="standardtr">
                                        <td width="8%" class="bdlnone bdblue">
                                            <%#GetFiveLevelStandardName(Eval("SkillLevel").ToString())%>
                                            <input type="hidden" sidname='<%#Eval("QS_SID")%>' name="standardscore<%#Eval("SkillLevel").ToString()%>" value="<%#Eval("Score").ToString()%>" />
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
                                    评分：
                                </td>
                                <td width="36%" style='text-align: left;'>
                                    <ul class="pfbz_chooseedit">
                                        <li><span href="javascript:void(0)" onclick='javascript:choosestatus(this,5);'>优秀</span></li>
                                        <li><span href="javascript:void(0)" onclick='javascript:choosestatus(this,4);'>良好</span></li>
                                        <li><span href="javascript:void(0)" onclick='javascript:choosestatus(this,3);'>合格</span></li>
                                        <li><span href="javascript:void(0)" onclick='javascript:choosestatus(this,2);'>较差</span></li>
                                        <li><span href="javascript:void(0)" onclick='javascript:choosestatus(this,1);'>很差</span></li>
                                    </ul>
                                </td> 
                                <td width="36%" style='text-align: left;'>  
                                    <input sidname='<%#Eval("QS_SID")%>' iidname='<%#Eval("QS_IID")%>' 
                                        cidname='<%#Eval("QS_CID")%>' cbtype='<%#Eval("ScoreType")%>' rdidname='<%#Eval("QS_RDID")%>' 
                                        choosedstandardscore="" type="text" name="choosedstandardmark" value="<%#GetStandardRemarkBySID(Eval("QS_SID").ToString()) %>" class="w400" />
                                    <input type="text" name="choosedstandardscore"  value="" style=" border:0px ; width:30px; display:none;"/>
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
            <%=GetNum("1")%>致命项 <a class="toggle2" onclick="divShowHideEvent('baseInfo',this)"
                href="javascript:void(0)" style="*margin-top: -25px;"></a>
        </div>
        <table width="100%" cellspacing="0" cellpadding="0" border="1" style="margin-top: 10px;">
            <tbody>
                <asp:Repeater ID="rp_Dead" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td width="58%" class="bdlnone zdq bd">
                                <%#GetNum(Container.ItemIndex + 1,"2")%><%#Eval("DeadItemName")%>
                            </td>
                            <td width="15%">
                                <label>
                                </label>
                            </td>
                            <td width="4%">
                                <label>
                                    <input id="cbDead" name="" type="checkbox" <%#IsExistResultDetail(Eval("QS_DAID").ToString(),"2")%>
                                        isdead="yes" rdidname="<%#IsExistResultDetailForMarking(Eval("QS_DAID").ToString(),"2")%>"
                                        value="<%#Eval("QS_DAID")%>" class="dx" /></label>
                            </td>
                            <td width="20%">
                                <div class="suggestion">
                                    <%#GetMarkedRemeak(Eval("QS_DAID").ToString(),"2")%></div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    <div class="lybase fwgf" style="<%=haveQulity%>">
        <div class="title">
            <%=GetNum("2")%>质检评价</div>
        <div class="pj">
            <textarea rows="" cols="" name="" id="txtQualityInfo" disabled="disabled"><%=txtQualityInfo%></textarea>
        </div>
    </div>
</div>