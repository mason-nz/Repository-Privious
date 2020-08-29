<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TagSelectPopNew.aspx.cs"
    EnableViewState="false" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.TagSelectPopNew" %>

<%@ Register Src="UCTag/TagLayout.ascx" TagName="tagSelect" TagPrefix="uc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>选择</title>
   <%-- <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="/Scripts/common.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <link href="/css/css.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />--%>
    <script type="text/javascript">


        $(function () {
            var choose$ = $('.choose');

            //            if (nameT) {
            //                choose$.empty().append('<li class="lichoose" name="imgDelTag"  did="' + valT + '">' + nameT + '&nbsp;<img src="../Css/img/xz_close.png"></li>');
            //            }


            $('.Contentbox').find('li').click(function (eve) {
                var this$ = $(this);
                var divid = $.trim(this$.closest('div').attr('did'));
                if (divid != '') {
                    divid = divid.substr(divid.lastIndexOf('_') + 1) - 1;
                }
                var txt = $.trim(this$.closest('.Tab2').find('ul[did=ulTagC]>li:eq(' + divid + ')').text()) + "--" + $.trim(this$.text());
                choose$.empty().append('<li class="lichoose" name="imgDelTag"  did="' + this$.attr('did') + '">' + txt + '&nbsp;<img src="/images/xz_close.png"></li>');
                $.closePopupLayer('TagSelectPopNew', true, { name: txt, val: this$.attr('did') });
            });

            $('.choose').click(function (eve) {
                if (eve.target.tagName == "LI") {
                    choose$.empty();
                }
            });
        });
        //TagSelectPopNew
        function SaveValue() {
            var li$ = $('.choose').find('li');
            var name = $.trim(li$.text());
            var val = li$.attr("did");
            if (!val) {
                alert("请选择标签后再保存");
                return;
            }
            $.closePopupLayer('TagSelectPopNew', true, { name: name, val: val });
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="popup  w820">
        <div class="title bold">
            选择标签<a href="#" onclick="javascript:$.closePopupLayer('TagSelectPopNew',false);"
                class="right"><img src="/images/c_btn.png" border="0" /></a>
        </div>
        <div class="xz_bq">
            <div class="bq1" style="border-bottom-width: 0px;">
                <span>已选择标签</span>
                <ul class="choose">
                    <%--<li class="lichoose" name="imgDelTag"  tagid="96">后台商机总量少&nbsp;<img src="../Css/img/xz_close.png"></li>--%>
                    <%--<li>后台商机总量少&nbsp;<img src="/images/xz_close.png"></li>--%>
                </ul>
            </div>
            <uc3:tagSelect ID="tagSelect1" runat="server" />
        </div>
        <div class="btnPop">
            <input name="" type="button" value="保 存" class="btnSave bold" onclick="SaveValue();" />
            <input name="" type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('TagSelectPopNew',false);" /></div>
    </div>
    </form>
</body>
</html>
