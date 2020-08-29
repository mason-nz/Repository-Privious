<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="Default4.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.Default4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" type="text/css" href="/css/GooCalendar.css"/>
<script  type="text/javascript" src="/js/GooCalendar.js"></script>
<script type="text/javascript">
    var property2 = {
        divId: "calen1", //日历控件最外层DIV的ID
        needTime: true, //是否需要显示精确到秒的时间选择器，即输出时间中是否需要精确到小时：分：秒 默认为FALSE可不填
        yearRange: [1970, 2030], //可选年份的范围,数组第一个为开始年份，第二个为结束年份,如[1970,2030],可不填
        //week: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'], //数组，设定了周日至周六的显示格式,可不填
        //month: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'], //数组，设定了12个月份的显示格式,可不填
        format: "yyyy-MM-dd hh:mm:ss"
        /*设定日期的输出格式,可不填*/
    };
    var property3 = {
        divId: "calen2", //日历控件最外层DIV的ID
        needTime: false, //是否需要显示精确到秒的时间选择器，即输出时间中是否需要精确到小时：分：秒 默认为FALSE可不填
        //yearRange: [1970, 2030], //可选年份的范围,数组第一个为开始年份，第二个为结束年份,如[1970,2030],可不填
        week: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'], //数组，设定了周日至周六的显示格式,可不填
        month: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'], //数组，设定了12个月份的显示格式,可不填
        format: "yyyy-MM-dd"
        /*设定日期的输出格式,可不填*/
    };
    var property = {
        divId: "demo",
        needTime: true,
        fixid: "fff"
        /*决定了日历的显示方式，默认不填时为点击INPUT后出现，但如果填了此项，日历控件将始终显示在一个id为其值（如"fff"）的DIV中（且此DIV必须存在）*/
    };
    $(document).ready(function () {
        $.createGooCalendar("calen", property2);
        $.createGooCalendar("cale2", property3);
        //canva2 = $.createGooCalendar("calen2", property2);
        //canva2.setDate({ year: 2008, month: 11, day: 22, hour: 14, minute: 52, second: 45 });
    });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    444
    <form name="form1" action="Default5.aspx" method="post" target="_blank">
    <input type="text" id="q1" name="q1" />
    <input type="submit" />
    </form>
    <br/>
    精确到时分秒：<input type="text" value="" id="calen" />
    精确到日：<input type="text" value="" id="cale2" />
</asp:Content>
