<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditScoreTable.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.ScoreTableManage.EditScoreTable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>评分表</title>
    <link href="../../Css/base.css" rel="stylesheet" type="text/css" />
    <link href="../../Css/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../../Js/common.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../../Js/jfunk-0.0.1.js" type="text/javascript"></script>
    <script src="../../Js/EditScoreTable.js?d=20140623" type="text/javascript"></script>
    <script src="../../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../../Js/json2.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript">

//        function setRegion() {
//            var chkRegion = document.getElementsByName('chkRegionID');
//            var regionID = "";
//            for (var k = 0; k < chkRegion.length; k++) {
//                if (chkRegion[k].checked) {
//                    regionID += chkRegion[k].value + ',';
//                }
//            }
//            EditScoreT.scoreObj.RegionID = regionID.substring(0, regionID.lastIndexOf(',')); //设置使用分组
//        }

        $(function () {
            var NoRight = '<%=NoRight %>';
            if (NoRight == "1") {
                $.jAlert("你没有权限新增和编辑评分表");
                return;
            }

            //编辑
            var QS_RTID = '<%=QS_RTID %>';
            var JsonStr = $("[id$='hidJsonStr']").val();
            if (QS_RTID != "" && JsonStr != "") {
                //编辑
                var Status = '<%=Status %>';
                var StatusInUse = '<%=StatusInUse %>';

                if (Status != '10001') {//状态：未完成
                    if (Status == '10003' && StatusInUse == '0') {
                        //状态：已完成,使用状态：未使用 可编辑 
                        EditScoreT.scoreObj = $.evalJSON(decodeURIComponent(JsonStr));
                    }
                    else {
                        $.jAlert("评分表当前状态不允许编辑");
                    }
                }
                else {
                    EditScoreT.scoreObj = $.evalJSON(decodeURIComponent(JsonStr));
                }
            }

            //初始化
            EditScoreT.init();

            //五个按钮事件
            $("#btnCategory").click(function () { EditScoreT.OpenCategory(); });
            $("#btnItem").click(function () {
                if ($(EditScoreT.scoreObj.Catage).length > 0) {
                    EditScoreT.OpenItem();
                }
                else {
                    $.jAlert("请先添加分类");
                }
            });
            $("#btnStandard").click(function () {

                if ($(EditScoreT.scoreObj.Catage).length > 0 && $(EditScoreT.scoreObj.Catage[0].Item).length > 0) {
                    EditScoreT.OpenStandard();
                }
                else {
                    $.jAlert("请先添加项目");
                }
            });
            $("#btnDead").click(function () { EditScoreT.OpenDead(); });
            $("#btnAppraisal").click(function () { EditScoreT.AddAppraisal(); });

            //评分分类对话框事件
            $("#divCategoryPopup .add a").live("click", function (e) {
                if (document.getElementById("spanTotalScore")) {
                    GetCanUseScore();
                    if ($("#spanCanUseScore").text() == "0" || $.trim($("#spanCanUseScore").text()).indexOf("-") == 0) {
                        $("#divCategoryPopup .add").each(function () {
                            $(this).find(" a").attr("title", "可用分值小于等于0时，不能继续增加质检类别");
                        });
                        return false;
                    }
                    else {
                        $("#divCategoryPopup .add").each(function () {
                            $(this).find(" a").attr("title", "");
                        });
                    }
                }
                e.preventDefault();
                EditScoreT.AddCategory(this);
                layResize();
            });
            //解决遮罩层不能遮罩住页面的问题
            function layResize() {
                if (document.getElementById("popupLayerScreenLocker")) {
                    $('#popupLayerScreenLocker').height($(document).height() * 1 + "px");
                }
            };
            $("#divCategoryPopup .delete a").live("click", function (e) {
                e.preventDefault();
                EditScoreT.DeleteCategory(this);
            });

            //质检项目对话框事件
            $("#divItemPopup .add a").live("click", function (e) {
                if (document.getElementById("spanCanUseScore")) {
                    if ($("#spanCanUseScore").text() == "0" || $.trim($("#spanCanUseScore").text()).indexOf("-") == 0) {
                        $("#divItemPopup .add").each(function () {
                            $(this).find(" a").attr("title", "可用分值小于等于0时，不能继续增加质检类别");
                        });
                        return false;
                    }
                    else {
                        $("#divItemPopup .add").each(function () {
                            $(this).find(" a").attr("title", "");
                        });
                    }
                }
                e.preventDefault();
                EditScoreT.AddItem(this);
                layResize();
            });
            $("#divItemPopup .delete a").live("click", function (e) {
                e.preventDefault();
                EditScoreT.DeleteItem(this);
            });
            $("#divItemPopup #itemSelCatagory").live("change", function () {
                EditScoreT.ShowItemHtmlByCID($(this).val());
                //用于五级质检使用
                if (document.getElementById("spanTotalScore") && EditScoreT.scoreObj.ScoreType == "3") {
                    GetCanUseScore();
                }
            });

            //质检标准对话框事件
            $("#divstandardPopup .SLi .add a").live("click", function (e) { e.preventDefault(); EditScoreT.AddStandard(this); });
            $("#divstandardPopup .SLi .delete a").live("click", function (e) { e.preventDefault(); EditScoreT.DeleteStandard(this); });
            $("#divstandardPopup #standardSelCatagory").live("change", function () {
                EditScoreT.BindItemByCID($(this).val());
                EditScoreT.ShowStandardHtmlByCIDIID($("#divstandardPopup #standardSelCatagory").val(),
             $("#divstandardPopup #standardSelItem").val());
            });
            $("#divstandardPopup #standardSelItem").live("change", function () {
                EditScoreT.ShowStandardHtmlByCIDIID($("#divstandardPopup #standardSelCatagory").val(), $(this).val());
            });
            //扣分项
            $("#divstandardPopup .MLi .add a").live("click", function (e) { e.preventDefault(); EditScoreT.AddMarking(this); });
            $("#divstandardPopup .MLi .delete a").live("click", function (e) { e.preventDefault(); EditScoreT.DeleteMarking(this); });

            //致命项
            $("#divDeadPopup .add a").live("click", function (e) { e.preventDefault(); EditScoreT.AddDead(this); });
            $("#divDeadPopup .delete a").live("click", function (e) { e.preventDefault(); EditScoreT.DeleteDead(this); });

            //选择评分表类别
            $("[name='ScoreType']").click(function () {
                selectScoreType($(this).val());
            });

            //选择适用区域 add lxw 13.6.19
            //            var chkRegion = document.getElementsByName('chkRegionID');
            //            for (var k = 0; k < chkRegion.length; k++) {
            //                chkRegion[k].onclick = setRegion;
            //            }

            //评分表名称，输入就保存到Json中
            $("#txtName").keyup(function () { EditScoreT.scoreObj.Name = $.trim($(this).val()); });
            //评分表描述，输入就保存到Json中
            $("#txtDesc").keyup(function () { EditScoreT.scoreObj.Description = $.trim($(this).val()); });

            $("#txtDeadItemNum").keyup(function (event) {
                if (((event.keyCode >= 48) && (event.keyCode <= 57)) || ((event.keyCode >= 96) && (event.keyCode <= 106)) || (event.keyCode == 190 || event.keyCode == 110)) {
                    EditScoreT.scoreObj.DeadItemNum = $.trim($(this).val());
                } else {
                    $('#txtDeadItemNum').attr("value", "0");
                }
            });

            $("#txtNoDeadItemNum").keyup(function (event) {
                if (((event.keyCode >= 48) && (event.keyCode <= 57)) || ((event.keyCode >= 96) && (event.keyCode <= 106)) || (event.keyCode == 190 || event.keyCode == 110)) {
                    EditScoreT.scoreObj.NoDeadItemNum = $.trim($(this).val());
                } else {
                    $('#txtNoDeadItemNum').attr("value", "0");
                }
            });

            //保存
            $("#btnSave").click(function () {
                if (checkParam()) {
                    SaveInfo("Save");
                }
            });
            $("#btnPreView").click(function () {
                if (checkParam()) {
                    SaveInfo("PreView");
                }
            });
            $("#btnSubmit").click(function () {
                if (checkParam()) {
                    SaveInfo("Submit");
                }
            });

        });

        function checkParam() {
            if ($.trim($("#txtName").val()) == "") {
                $.jAlert("请填写“评分表名称”项");
                return false;
            }
//            if ($("input:checkbox[name='chkRegionID']:checked").length == 0) {
//                $.jAlert("请选择“适用区域”项");
//                return false;
//            }
            if ($.trim($("#txtDesc").val()) == "") {
                $.jAlert("请填写“评分表说明”项");
                return false;
            }
            if ($("#div1").html() == "" && $("#div2").html() == "" && $("#div3").html() == "") {
                $.jAlert("请设置“评分表设置”项");
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        //选择评分表类型
        function selectScoreType(v) {
            if (v != "1" && v != "2" && v != "3") { alert("评分表类型值不正确"); return; }
            EditScoreT.scoreObj.ScoreType = v; //设置类型值
//            if (v == "1" || v == "3") {
//                $("#btnDead").show();
//                $(".DeadItemDiv").hide();
//            }
//            else {
//                $("#btnDead").hide();
//                $(".DeadItemDiv").show();
//            }

            EditScoreT.BindHTMLbyOjb(); //重新绑定HTML
        }

        function SaveInfo(ActionType) {
            EditScoreT.SaveInfo(ActionType);
//            if (ActionType != "PreView") {
//                execOpenerSearch();
//            }
        } 
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
           <%if (string.IsNullOrEmpty(QS_RTID))
             {%>添加评分表
           <%}
             else
             {%>编辑评分表
           <%} %>
           </div>
        <!--添加评分表-->
        <div class="addexam clearfix">
            <ul>
                <li>
                    <label>
                        <span class="redColor">*</span>评分表名称：</label><span><input type="text" id="txtName"
                            value="" class="w260"/></span></li>
                <li>
                    <label><span class="redColor">*</span>评分表类型：</label>
                    <span  style="float: left;">
                        <input name="ScoreType" type="radio" value="1" checked="checked" />
                        <em onclick="emChkIsChoose(this);selectScoreType('1');" class="dx">评分型</em>&nbsp;
                        <input name="ScoreType" type="radio" value="2" />
                        <em class="dx" onclick="emChkIsChoose(this);selectScoreType('2');">合格型</em>
                        <input name="ScoreType" type="radio" value="3" checked="checked" />
                        <em onclick="emChkIsChoose(this);selectScoreType('3');" class="dx">五级评分型</em>&nbsp;
                    </span>
                    <span style="float: left; display: none;" class="DeadItemDiv">
                        <label>致命项数:&nbsp;&nbsp;</label><input type="text" class="w50" value="" id="txtDeadItemNum"/>
                    </span>
                    <span style="float: left; display: none;" class="DeadItemDiv">
                        <label style="width: 80px;">非致命项数:&nbsp;&nbsp;</label><input type="text" class="w50" value="" id="txtNoDeadItemNum"/>
                    </span>
                </li>
             <%--   <li>
                    <label>
                        <span class="redColor">*</span>适用区域：</label><span style="float: left;">
                            <input type="checkbox" value="1" name="chkRegionID" /><em onclick="emChkIsChoose(this);setRegion();">北京</em>
                        </span><span style="float: left;">
                            <input type="checkbox" value="2" name="chkRegionID" /><em onclick="emChkIsChoose(this);setRegion();">西安</em>
                        </span></li>--%>
                <li>
                    <label>
                        <span class="redColor">*</span>评分表说明：</label><span><textarea id="txtDesc" name="" cols="" rows=""></textarea></span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>评分表设置：</label><span class="btn" style="text-align: left; width: 598px; margin-left:0px;">
 
                                <input type="button" id="btnCategory" value="质检类别" style="margin-right: 5px;"/>
                                <input type="button" id="btnItem" value="质检项目" style="margin-right: 5px;"/>
                                <input type="button" id="btnStandard" value="质检标准" style="margin-right: 5px;"/>
                                <input type="button" id="btnDead" value="致命项" style="margin-right: 5px;"/>
                                <input type="button" id="btnAppraisal" value="质检评价" style="margin-right: 5px;"/>
                        </span></li>
            </ul>
            <!--添加评分表-->
        </div>
        <div class="pfb" id="div1">
        </div>
        <div class="pfb" id="div2">
        </div>
        <div class="pfb" id="div3">
        </div>
        <div class="btn" style="margin: 20px auto">
            <input type="button" id="btnPreView" name="" value="预览"/>&nbsp;&nbsp;
            <input type="button" id="btnSave" name="" value="保存"/>&nbsp;&nbsp;
            <input type="button" id="btnSubmit" name="" value="提交"/>
        </div>
    </div>
    <input type="hidden" value="" runat="server" id="hidJsonStr" />
    </form>
</body>
</html>
