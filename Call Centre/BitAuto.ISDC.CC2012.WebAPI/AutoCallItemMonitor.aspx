<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AutoCallItemMonitor.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.WebAPI.AutoCallItemMonitor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/Css/CC/base.css" rel="stylesheet" type="text/css" />
    <link href="Content/Css/CC/style.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="Scripts/common.js" type="text/javascript"></script>
    <script src="Scripts/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
</head>
<body>
    <div id="head">
        <div class=" logo" style="text-align: center;">
            易车自动外呼监视
        </div>
        <div class="right">
            <a style="line-height: 32px;" class="button profile">
                <img src="/Content/Css/CC/img/huser.png"></a> 您好，<%=strName%>&nbsp; |&nbsp; <a href="/LoginOut.aspx"
                    target="_self">注销</a>
        </div>
        <div class="menu left">
            <ul id="ulTopMenu">
            </ul>
        </div>
    </div>
    <div class="left" id="content" style="margin-left: 10px;">
        <div class="rC left">
            <div class="content">
                <div class="search clearfix" style="margin-top: 25px; margin-bottom: 10px;">
                    <ul class="clear">
                        <li>
                            <label>
                                电话号码：</label>
                            <input type="text" id="txtName" class="w190" />
                        </li>
                        <li>
                            <label>
                                项目ID：</label>
                            <input type="text" id="txtProd" class="w190" />
                        </li>
                        <li>
                            <label style="font-weight: bolder;">
                                外呼状态：</label>
                            <span>
                                <input type="checkbox" id="chkStatusUnfinished" value="0" name="acChkStatus" /><em
                                    onclick='emChkIsChoose(this);'>未开始&nbsp;</em></span> <span>
                                        <input type="checkbox" id="chkStatusUnused" value="1" name="acChkStatus" /><em onclick='emChkIsChoose(this);'>已锁定&nbsp;</em></span>
                            <span>
                                <input type="checkbox" id="chkStatusUsed" value="2" name="acChkStatus" /><em onclick='emChkIsChoose(this);'>外呼中</em></span>
                            <span>
                                <input type="checkbox" id="Checkbox1" value="3" name="acChkStatus" /><em onclick='emChkIsChoose(this);'>已结束</em></span>
                            <span>
                                <input type="checkbox" id="Checkbox2" value="4" name="acChkStatus" /><em onclick='emChkIsChoose(this);'>已处理</em></span>
                        </li>
                        <li class="btnsearch">
                            <input style="float: right" name="" type="button" value="查 询" onclick="javascript:search();" />
                        </li>
                    </ul>
                </div>
                <div class="optionBtn clearfix">
                </div>
                <div id="ajaxTable">
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function JsonObjToParStr(json) {
            var tmps = [];
            for (var key in json) {
                tmps.push(key + '=' + escape(json[key]));
            }
            return tmps.join('&');
        }

        //查询
        function search() {
            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServices/AutoCallItems.aspx .bit_table > *", podyStr);
        }


        //获取参数
        function _params() {

            var name = encodeURIComponent($.trim($("#txtName").val()));
            var pro = encodeURIComponent($.trim($("#txtProd").val()));

            var acStatus = $(":checkbox[name='acChkStatus']:checked").map(function () {
                return $(this).val();
            }).get().join(',');

            var pody = {
                phone: name,
                ac: acStatus,
                pro: pro,
                r: Math.random()
            };

            return pody;
        }

        //分页操作
        function ShowDataByPost100(pody) {

            LoadingAnimation("ajaxTable");
            $('#ajaxTable').load('/AjaxServices/AutoCallItems.aspx?r=' + Math.random() + ' .bit_table > *', pody);
        }


        $(function () {
            search();
            enterSearch(search);
        });

    </script>
</body>
</html>
