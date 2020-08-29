<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KnowledgeCategorySelect.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.KnowledgeCategorySelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function updateCategory() {
            var KnowledgeCategory = $("#KCIDhid").val();
            var KLID = '<%=RequestKLID %>';
            if (KnowledgeCategory == "") {
                $.jAlert("请选择分类！");
            }
            else {
                var pody = { KCID: KnowledgeCategory, KLID: KLID, Action: 'updateknowledgecategory' };
                AjaxPost('../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx', pody, null, function (data) {
                    if (data != '') {

                        if (data == 'success') {
                            $.jPopMsgLayer("转移成功！", function () { 
                                window.location.reload();
                            }); 
                            //$.closePopupLayer('KnowledgeCategorySelectAjaxPopup');
                        }
                        else {
                            $.jAlert("转移失败！");
                        }
                    }

                });
            }

        }

        function selectclick(KCID, AID) {
            $("#KCIDhid").val("");
            $("#KCIDhid").val(KCID);
            $("a").removeClass("cur");
            //$("a").addClass("cur");
            $("#" + AID).addClass("cur");
            //$(this).addClass("cur");

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pop pb15 popv">
        <div class="title bold">
            <h2>
                转移至</h2>
            <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('KnowledgeCategorySelectAjaxPopup');">
            </a></span>
        </div>
        <asp:Repeater ID="repeaterTableList" runat="server" OnItemDataBound="repeaterTableList_ItemDataBound">
            <ItemTemplate>
                <div class="moveC clearfix">
                    <div class="bt">
                        <%#Eval("Name")%>：</div>
                    <asp:Repeater ID="repeatersonson" OnItemDataBound="repeatersonson_ItemDataBound"
                        runat="server">
                        <ItemTemplate>
                            <div class="moveout " <%# Container.ItemIndex>0? "style='clear: both; margin-left: 65px;'":""%>>
                                <div class="bt2">
                                    <%#Eval("Name")%>&nbsp;|</div>
                                <asp:Repeater ID="repeatersonsonson" runat="server">
                                    <ItemTemplate>
                                        <div>
                                            <a href="javascript:void(0)" id="<%#Eval("KCID")%>" onclick="selectclick('<%#Eval("KCID")%>',this.id)">
                                                <%#Eval("Name")%></a></div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div class="moveout" style="clear: both; margin-left: 65px;" id="dvson" runat="server">
                        <asp:Repeater ID="repeaterLast" runat="server">
                            <ItemTemplate>
                                <div>
                                    <a href="javascript:void(0)" id="<%#Eval("KCID")%>" onclick="selectclick('<%#Eval("KCID")%>',this.id)">
                                        <%#Eval("Name")%></a></div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="moveout ">
                        <asp:Repeater ID="repeaterson" runat="server">
                            <ItemTemplate>
                                <div>
                                    <a href="javascript:void(0)" id="<%#Eval("KCID")%>" onclick="selectclick('<%#Eval("KCID")%>',this.id)">
                                        <%#Eval("Name")%></a></div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <div class="Table2">
            <div class="btn" style="margin: 20px 10px 10px 0px;">
                <input type="hidden" id="KCIDhid" />
                <input type="button" name="" value="确 定" onclick="javascript:updateCategory();" class="btnSave bold" />&nbsp;&nbsp;
                <input type="button" name="" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('KnowledgeCategorySelectAjaxPopup');" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
