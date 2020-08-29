<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderInfoListForHB.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.AjaxServers.AddOrderTab.OrderInfoListForHB" %>

<div class="search" id="search_div">
    <ul>
        <li>
            <label>
                手机号码：</label><span><input name="" id="txtphone" type="text" class="w200"></span></li>
        <li class="button">
            <input name="" type="button" value="查询" onclick="searchForHB(true)"></li>
    </ul>
</div>
<div class="clearfix">
</div>
<div class="search_list_bt" style="position: relative;">
    <table border="1" cellspacing="0" cellpadding="0" class="bt_guding" style="position: absolute;">
        <tr class="bold">
            <th style="width: 8%;">
                姓名
            </th>
            <th style="width: 12%;">
                下单号码
            </th>
            <th style="width: 10%;">
                下单品牌
            </th>
            <th style="width: 10%;">
                下单车型
            </th>
            <th style="width: 18%;">
                下单时间
            </th>
            <th style="width: 8%;">
                来源
            </th>
            <th style="width: 10%;">
                订单ID
            </th>
            <th style="">
                备注
            </th>
            <th style="width: 8%;">
                操作
            </th>
        </tr>
    </table>
</div>
<div class="search_list search_list_order" id="bit_table3">
</div>
<script type="text/javascript">
    $(document).ready(function () {
        var tel = '<%=Tel %>';
        var Notext = "<%=Notext %>";
        $("#txtphone").val(tel);
        searchForHB(false);
        if (Notext == "true") {
            //不显示查询条件
            $("#search_div").css("display", "none");
        }
    });
    //从hbase库查询订单
    function searchForHB(isalert) {
        var tel = $.trim($("#txtphone").val());
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
            LoadingAnimation("bit_table3", "search_list_bt_loading");
            $("#bit_table3").load("/WOrderV2/AjaxServers/AddOrderTab/OrderInfoListForHB_AjaxList.aspx?tel=" + tel + "&keyid=" + keyid + "&r=" + Math.random(), null);
        }
        else {
            if (isalert) {
                $.jAlert(msg, function () { $("#txtphone").focus(); });
            }
            return;
        }
    }
    //分页
    function ShowDataByPost1010(pody) {
        LoadingAnimation("bit_table3", "search_list_bt_loading");
        $("#bit_table3").load('/WOrderV2/AjaxServers/AddOrderTab/OrderInfoListForHB_AjaxList.aspx?r=' + Math.random(), pody);
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
