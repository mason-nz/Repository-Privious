<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCQualityStandardEditForState.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard.UCQualityStandardEditForState" %>
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

    function SubState(url) {
        var flag = false;
        var json = {
            action: "substate",
            data: JSON.stringify(GetQualityStandarObj())
        };
        //审核
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
        })
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
        })

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
    $(document).ready(function () {
        $("td[name='firsttd']").each(function () {
            var l_h = $(this).height(); //获取左侧的td的高度
            var l_h = Math.ceil(l_h) + 9;
            $($(this).next().children()[0]).css("height", l_h); //给右侧嵌套table添加属性（高）
        })
    });
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
                        <table border="1" cellspacing="0" cellpadding="0" width="100%">
                            <asp:Repeater ID="rp_Standard" runat="server" OnItemDataBound="rp_Standard_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td width="36%" class="bdlnone zdq" name="firsttd">
                                            <%#GetNum(Container.ItemIndex + 1,"3")%><%#Eval("ScoringStandardName")%>
                                            <span>（<%#Eval("ScoreType").ToString()=="1"?Eval("Score")+"分":(Eval("IsIsDead").ToString()=="1"?"致命":"非致命")%>）</span>
                                            <asp:Label ID="lblQS_SID" runat="server" Visible="false" Text='<%#Eval("QS_SID")%>'></asp:Label>
                                        </td>
                                        <td width="64%" class="qrb">
                                            <table style="border: 0; cellspacing: 0;cellpadding:0; width: 100%">
                                                <asp:Repeater ID="rp_Marking" runat="server" OnItemDataBound="rp_Marking_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td class="zdq" style="width:58%">
                                                                <%#Eval("MarkingItemName")%>
                                                                <asp:Label ID="lblMarkID" Visible="false" Text='<%#Eval("QS_MID")%>' runat="server"></asp:Label>
                                                                <asp:Label ID="lblScoreType" runat="server" Visible="false" Text='<%#Eval("ScoreType")%>'></asp:Label>
                                                                <asp:Label ID="lblStandarID" Visible="false" Text='<%#Eval("QS_SID")%>' runat="server"></asp:Label>
                                                                <span style='<%=ScoreTypeFlag %>'>（<%#Eval("Score")%>分）</span>&nbsp;
                                                            </td>
                                                            <td style="width:12%">
                                                                <label>
                                                                    <asp:Label ID="lblMarkingScore" Visible="false" Text='<%#Eval("Score")%>' runat="server"></asp:Label>
                                                                </label>&nbsp;
                                                            </td>
                                                            <td style="width:12%">
                                                                <label>
                                                                    <input id="cbMarking" sidname="<%#Eval("QS_SID")%>" <%#IsExistResultDetail(Eval("QS_MID").ToString(),"1")%>
                                                                        rdidname="<%#IsExistResultDetailForMarking(Eval("QS_MID").ToString(),"1")%>"
                                                                        iidname="<%#Eval("QS_IID")%>" cidname="<%#Eval("QS_CID")%>" cbtype="<%#Eval("ScoreType")%>"
                                                                        isdead="no" type="checkbox" value="<%#Eval("QS_MID")%>" class="dx" /></label>&nbsp;
                                                            </td>
                                                            <td style="width:18%; border-right:0px;word-wrap: break-word; word-break: break-all;"> 
                                                                <%--<div class="suggestion">--%>
                                                                    <asp:Label ID="lblMarkingRemark" Visible="false" runat="server"></asp:Label>
                                                                   <%-- </div>--%>&nbsp;
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
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
