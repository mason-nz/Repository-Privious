<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CityManageList.aspx.cs"
    Inherits="BitAuto.DSC.IM_DMS2014.Web.AjaxServers.SeniorManage.CityManageList" %>

<script type="text/javascript">
    $(document).ready(function () {
        //定义选择控件动作
        DefineCheckBox();
    });
    //定义选择控件动作
    function DefineCheckBox() {
        var ckb_all = document.getElementById("ckb_all");
        var ckb_row = document.getElementsByName("ckb_row");
        for (var i = 0; i < ckb_row.length; i++) {
            ckb_row[i].onclick = function () {
                if (this.checked) {
                    var isAllCheck = true;
                    for (var i = 0; i < ckb_row.length; i++) {
                        if (!ckb_row[i].checked) {
                            isAllCheck = false;
                            break;
                        }
                    }
                    if (isAllCheck)
                        ckb_all.checked = true;
                }
                else {
                    ckb_all.checked = false;
                }
            };
        }
        ckb_all.onclick = function () {
            for (var i = 0; i < ckb_row.length; i++) {
                ckb_row[i].checked = this.checked;
            }
        }
    }
</script>
<table border="0" cellspacing="0" cellpadding="0">
    <tr>
        <th width="10%">
            <input id="ckb_all" type="checkbox" value="" />
        </th>
        <th width="30%">
            所属大区
        </th>
        <th width="30%">
            城市群
        </th>
        <th width="25%">
            所属客服
        </th>
    </tr>
    <asp:repeater id="rpt_citygroup" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <input name="ckb_row" type="checkbox" value="<%#Eval("CityGroup")%>" />
                </td>
                <td>
                    <%#Eval("DistrictName")%>
                    <input type="hidden" value="<%#Eval("District")%>" id="<%#Eval("CityGroup")%>_District"/>
                </td>
                <td>
                    <%#Eval("CityGroupName")%>
                    <input type="hidden" value="<%#Eval("CityGroup")%>" id="<%#Eval("CityGroup")%>_CityGroup"/>
                </td>
                <td>
                    <%#Eval("kefu_nm_list")%>
                    <input type="hidden" value="<%#Eval("kefu_id_list")%>" id="<%#Eval("CityGroup")%>_kefu_id_list"/>
                </td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
    <tr>
        <td colspan="5">
            <div class="pagesnew" style="float: right; margin: 10px;" id="itPage">
                <p>
                    <asp:literal runat="server" id="litPagerDown"></asp:literal>
                </p>
            </div>
        </td>
    </tr>
</table>
