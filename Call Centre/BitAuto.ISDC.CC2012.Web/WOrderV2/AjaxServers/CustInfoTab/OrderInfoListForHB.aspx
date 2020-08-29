<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderInfoListForHB.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.CustInfoTab.OrderInfoListForHB" %>

<div class="search_list2" id="bit_table3">
</div>
<script type="text/javascript">
    $(document).ready(function () {
        searchForHB(false);
    });
    //从hbase库查询订单
    function searchForHB(isalert) {
        var tel = '<%=Tel %>';
        var keyid = '<%=keyID %>';
        var isok = true;
        var msg = "";

        if (tel) {
            var tels = tel.split(",");
            for (var i = 0; i < tels.length; i++) {
                if (!isTelOrMobile(tels[i])) {
                    isok = false;
                    msg = "手机或电话号码输入不正确！";
                    break;
                }
            }
        }
        //查询订单
        if (isok) {
            //加载查询结果
            LoadingAnimation("bit_table3");
            $("#bit_table3").load("/WOrderV2/AjaxServers/CustInfoTab/OrderInfoListForHB_AjaxList.aspx?tel=" + tel + "&keyid=" + keyid + "&r=" + Math.random(), null);
        }
        else {
            if (isalert) {
                $.jAlert(msg, function () { });
            }
            return;
        }
    }
    //分页
    function ShowDataByPost1010(pody) {
        LoadingAnimation("bit_table3");
        $("#bit_table3").load('/WOrderV2/AjaxServers/CustInfoTab/OrderInfoListForHB_AjaxList.aspx?r=' + Math.random(), pody);
    }
    window.onunload = function () {
        var pody = {
            keyid: '<%=keyID %>',
            r: Math.random()
        };
        AjaxPostAsync("/WOrderV2/AjaxServers/OrderInfoListForHBForClose.ashx", pody, null, function (data) {
        });
    }
</script>
