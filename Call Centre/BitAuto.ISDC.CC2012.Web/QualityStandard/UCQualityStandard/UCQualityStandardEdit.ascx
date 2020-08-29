<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCQualityStandardEdit.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard.UCQualityStandardEdit" %>
<script type="text/javascript">
    //录音质检保存
    function SaveQualityStandar() {
        Save("/AjaxServers/QualityStandard/ScoringManage/ScoringManage_CC.ashx");
    }
    //录音质检提交
    function SubQualityStandar() {
        Submit("/AjaxServers/QualityStandard/ScoringManage/ScoringManage_CC.ashx");
    }
    //对话质检保存
    function SaveQualityStandarIM() {
        Save("/AjaxServers/QualityStandard/ScoringManage/ScoringManage_IM.ashx");
    }
    //对话质检提交
    function SubQualityStandarIM() {
        Submit("/AjaxServers/QualityStandard/ScoringManage/ScoringManage_IM.ashx");
    }

    //保存
    function Save(url) {
        var json = {
            action: "save",
            tableEndName: '<%=tableEndName %>',
            data: JSON.stringify(GetQualityStandarObj())
        };
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
                    $.jPopMsgLayer("保存成功！");
                    //刷新父页面
                    execOpenerSearch();
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
        var QS_ResultDetailList = new Array(); //问题列表
        $("input[type='checkbox'][isdead='no']").each(function () {
            if ($(this).attr("checked") != undefined) {
                if ($(this).attr("checked")) {
                    var resultDetail = { QS_RDID: escape("0"),
                        ScoreType: escape($(this).attr("cbtype")),
                        QS_RTID: escape('<%=QS_RTID%>'),
                        QS_RID: escape($('#hdQs_RID').val()),
                        QS_CID: escape($(this).attr("cidname")),
                        QS_IID: escape($(this).attr("iidname")),
                        QS_SID: escape($(this).attr("sidname")),
                        QS_MID: escape($(this).attr("value")),
                        QS_MID_End: escape($(this).attr("value")),
                        Type: escape("1"),
                        ScoreDeadID: escape(""),
                        ScoreDeadID_End: escape(""),
                        Remark: escape($($(this).parent().parent().next().children()[0]).val())
                    };
                    QS_ResultDetailList.push(resultDetail);
                }
            }
        })
        //致命项
        $("input[type='checkbox'][isdead='yes']").each(function () {
            if ($(this).attr("checked") != undefined) {
                if ($(this).attr("checked")) {
                    var resultDetail = { QS_RDID: escape("0"),
                        ScoreType: escape("1"),
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
    //load
    $(document).ready(function () {

        $("td[name='firsttd']").each(function () {
            var l_h = $(this).height(); //获取左侧的td的高度
            var l_h = Math.ceil(l_h) + 9;
            $($(this).next().children()[0]).css("height", l_h); //给右侧嵌套table添加属性（高）
        })
    });
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
</script>
<div class="pfb" style="margin-top: 10px;">
    <input type="hidden" id="hdQs_RID" value='<%=QS_RID%>' />
    <input type="text" id="hdrealQs_RID" visible="false" runat="server" />
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
                            <%#GetNum(Container.ItemIndex + 1,"2")%><%#Eval("ItemName")%><span style='<%=ScoreTypeFlag %>'>（<%#Eval("Score")%>分）</span></p>
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
                                            <table cellspacing="0" cellpadding="0" width="100%">
                                                <asp:Repeater ID="rp_Marking" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td width="58%" class="zdq">
                                                                <%#Eval("MarkingItemName")%>
                                                                <span style='<%=ScoreTypeFlag %>'>（<%#Eval("Score")%>分）</span>
                                                            </td>
                                                            <td width="12%">
                                                                <label>
                                                                    <input id="cbMarking" <%#IsExistResultDetail(Eval("QS_MID").ToString(),"1")%> sidname="<%#Eval("QS_SID")%>"
                                                                        iidname="<%#Eval("QS_IID")%>" cidname="<%#Eval("QS_CID")%>" cbtype="<%#Eval("ScoreType")%>"
                                                                        isdead="no" type="checkbox" value="<%#Eval("QS_MID")%>" class="dx" /></label>
                                                            </td>
                                                            <td width="30%" class="borderR">
                                                                <input type="text" id="MarkingRemark" value="<%#GetMarkedRemeak(Eval("QS_MID").ToString(),"1")%>"
                                                                    class="wsr" runat="server" />
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
</div>
