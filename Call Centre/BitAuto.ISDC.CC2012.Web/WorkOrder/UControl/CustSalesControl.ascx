<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustSalesControl.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WorkOrder.UControl.CustSalesControl" %>
<label>
    接收人：</label><span><input name="ReceiverName" id="txtReceiverName" type="text" class="w250"
        readonly="readonly" onclick="uc_selectSales('')" /><input name="ReceiverID" id="hidReceiverID"
            type="hidden" value="" /></span><span><input name="IsSales" id="chkIsSales" type="checkbox"
                value="true" class="dx" onclick="uc_chkSelectSales()" /><em onclick="emChkIsChoose(this);uc_chkSelectSales();">营销顾问</em></span>
<script type="text/javascript">
    //选择营销顾问
    function uc_chkSelectSales() {

        if (!$("#chkIsSales").is(":checked")) {
            $("#hidReceiverID").val("");
            $("#txtReceiverName").val("");
        }
        else {
            uc_popSelectUser("<%=crmCustID %>");
        }
    }

    //弹出选择接收人
    function uc_selectSales(crmCustID) {
        $.openPopupLayer({
            name: "SelectCustUserPopup",
            parameters: { CrmCustID: crmCustID },
            url: "/WorkOrder/AjaxServers/SelectCustUserPoper.aspx", afterClose: function (e, data) {
                if (e) {
                    var userid = data.UserID;
                    var username = data.UserName;

                    $("#hidReceiverID").val(userid);
                    $("#txtReceiverName").val(username);

                    if (crmCustID == "") {
                        //说明是直接点击的接收人文本框，需要清空营销顾问复选框
                        $("#chkIsSales").attr("checked", false);
                    }
                }
                else {
                    //说明没有选择，需要清空营销顾问复选框
                    $("#chkIsSales").attr("checked", false);
                }
            }
        });
    }

    function uc_popSelectUser(crmCustID) {
        $.post("/WorkOrder/AjaxServers/WorkOrderOperHandler.ashx", { Action: 'GetCustUser', CrmCustID: crmCustID }, function (data) {
            var jsonData = $.evalJSON(data);

            if (jsonData.totalcount == 0) {
                $.jAlert("该客户下没有负责销售，请选择其他接收人！", function () {
                    $("#chkIsSales").attr("checked", false);
                    return false;
                });
            }
            else if (jsonData.totalcount == 1) {
                var userid = jsonData.userinfo.userid;
                var username = jsonData.userinfo.username;

                $("#hidReceiverID").val(userid);
                $("#txtReceiverName").val(username);
            }
            else {
                uc_selectSales(crmCustID);
            }
        });
    }

</script>
