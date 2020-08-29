<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExprotDataLayer.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.UserActionLog.ExprotDataLayer" %>

<link href="/css/monthpicker.css" type="text/css" rel="stylesheet" />
<style type="text/css">
    .message
    {
        border: 1px solid #999;
        font-weight: bold;
        background: #ffe;
        height: 100px;
        overflow: auto;
    }
    .javascript
    {
        border: 1px solid #999;
    }
    pre
    {
        font-family: Verdana;
        width: 500px;
        font-size: 12px;
        padding: 6px;
    }
</style>
<script type="text/javascript" src="/Js/monthpicker.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#divMonthpicker").monthpicker('<%=DateTime.Now.ToString("yyyy-MM") %>', MonthpickerCallBack);
        SetMonthpickerPara('<%=DateTime.Now.Year %>', '<%=DateTime.Now.Month %>');
    });

    function MonthpickerCallBack(data, $e) {
        SetMonthpickerPara(data["year"], data["month"]);
    }

    function SetMonthpickerPara(year, month) {
        //$("#divMonthpicker").data('year', year)
        //$("#divMonthpicker").data('month', month);
        $('#hidYear').val(year);
        $('#hidMonth').val(month);
    }

    function ExprotDataBy4sAndNot4s() {
        var year = $('#hidYear').val();
        var month = $('#hidMonth').val();
        if (year == 'undefined' || month == 'undefined') {
            $.jAlert('参数有误！'); return;
        }
    }
</script>
<div style="width: 1200px" class="pop pb15 openwindow">
    <form id="form1" action="ExprotData.aspx" method="post">
    <div class="title bold">
        <h2 style="cursor: auto;">
            导出4s和非4s数据</h2>
        <span><a onclick="javascript:$.closePopupLayer('ExprotDataBy4sAndNot4s',false);"
            href="javascript:void(0)"></a></span>
    </div>
    <%--    <ul id="addTemplatePage" class="clearfix ft14">
        <li style="width: 259px">
            <label>
                所属业务组：</label>
           
        </li>
    </ul>--%>
    <div id="divMonthpicker" class="MonthPicker" style="clear: both;">
    </div>
    <input type="hidden" id="hidYear" name="year" value="" />
    <input type="hidden" id="hidMonth" name="month" value="" />
    <input type="hidden" id="hidAction" name="action" value="exprotdataby4sandnot4s" />
    <ul id="btnSumbit" style="text-align: center;" class="clearfix">
        <li>
            <input type="submit" style="margin-right: 18px" value="导出" class="btnChoose" onclick="ExprotDataBy4sAndNot4s();" />
            <input type="button" value="关闭页面" class="btnChoose" onclick="javascript:$.closePopupLayer('ExprotDataBy4sAndNot4s',false);" /></li>
    </ul>
    </form>
</div>
