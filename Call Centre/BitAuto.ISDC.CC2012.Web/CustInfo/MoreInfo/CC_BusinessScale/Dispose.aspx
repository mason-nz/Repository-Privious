<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dispose.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_BusinessScale.Dispose" %>

<script type="text/javascript">

    function SaveData() {
        var txtMonthStock = $.trim($('#sltMonthStock').val());
        var txtMonthSales = $.trim($('#sltMonthSales').val());
        var txtMonthTrade = $.trim($('#sltMonthTrade').val());
        if (txtMonthSales == '-1' && txtMonthStock == "-1" && txtMonthTrade == "-1") {
            $.jAlert("请至少选择一项");
            return;
        }
        var url = "/CustInfo/MoreInfo/CC_BusinessScale/Handler.ashx?Action=<%=Action %>";
        var postBody = "&TID=" + escape("<%=TID %>")
                        + "&RecID=" + escape("<%=RecID %>")
                        + "&MonthStock=" + escapeStr(txtMonthStock)
                        + "&MonthSales=" + escapeStr(txtMonthSales)
                        + "&MonthTrade=" + escapeStr(txtMonthTrade);
        AjaxPost(url, postBody, null, SuccessPost);
        function SuccessPost(data) {
            if (data == 'success') {
                $.jPopMsgLayer('操作成功！', function () {
                    loadTabList(5, 1);
                    Close('DisposeBussinessScale', true);
                });
            }
            else {
                $.jAlert(data);
            }
        }
    }
            
</script>
<form id="AddScaleInfo">
<!--弹窗1联系人信息-->
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            二手车规模信息</h2>
        <span><a href="javascript:void(0)" onclick="javascript:Close('DisposeBussinessScale');">
        </a></span>
    </div>
    <ul class="clearfix ">
        <li>
            <label>
                月库存数量：</label><select id="sltMonthStock" name="MonthStock" runat="server" style="width: 150px">
                </select>
        </li>
        <li>
            <label>
                月置换数量：</label><select id="sltMonthSales" name="MonthSales" runat="server" style="width: 150px">
                </select>
        </li>
        <li>
            <label>
                月交易数量：</label><select id="sltMonthTrade" name="MonthTrade" runat="server" style="width: 150px">
                </select>
        </li>
    </ul>
    <div class="btn" style="margin: 20px 10px 10px 0px;">
        <input type="button" value="保存" class="btnSave bold" onclick="javascript:SaveData();" />
        <input type="button" value="退出" class="btnSave bold" onclick="javascript:Close('DisposeBussinessScale');" />
    </div>
</div>
</form>
