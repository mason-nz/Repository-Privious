<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectProjectPop.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ProjectManage.PopPanel.SelectProjectPop" EnableViewState="false" %>

<%--
<link href="../../Css/base.css" rel="stylesheet" type="text/css" />
<link href="../../Css/style.css" rel="stylesheet" type="text/css" />
<script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
<script src="../../Js/common.js" type="text/javascript"></script>--%>
<script src="../../Js/jquery.autocomplete.min.js" type="text/javascript"></script>
<link href="../../Css/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {
        //初始化“项目名称”条件，自动查询
        $('#txtProjectName').autocomplete('/AjaxServers/ProjectManage/ProjectHandler.ashx', {
            minChars: 1,
            //width: showWidth,
            scrollHeight: 300,
            autoFill: false,
            delay: 400,
            matchSubset: false, //是否启用缓存
            cacheLength: 1, //缓存的长度.即缓存多少条记录.设成1为不缓存.Default: 10
            matchContains: false,
            extraParams: { Action: 'GetProejctNameByAutoComplete',
                ProjectName: function () { return $("#txtProjectName").val(); },
                BGID: function () { return $("#selPGBG_Group").val(); },
                PCatageID: function () { return $("#selPGBG_catalog").val(); },
                r: Math.random()
            },
            parse: function (data) {
                if (data != "") {
                    return $.map(eval(data), function (row) {
                        return {
                            data: row,
                            value: row.ProjectID,    //此处无需把全部列列出来，只是两个关键列
                            result: data.ProjectName
                        }
                    });
                }
                else {
                    $("#txtProjectName").removeAttr('ProjectID');
                }
            },
            formatItem: function (data, i, n, value) {
                return data.ProjectName;
            },
            formatResult: function (data, value) {
                return data.ProjectID;
            }
        }).result(function (event, data, formatted) {
            if (data && data != "") {
                $(this).val(data.ProjectName);
                $(this).attr('ProjectID', data.ProjectID);
            }
        });

        ProjectPop_getUserGroup();
        ProjectSelectMemberSearch();
    });


    //加载登陆人业务组
    function ProjectPop_getUserGroup() {
        AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", r: Math.random() }, null, function (data) {
            $("#selPGBG_Group").append("<option value='-1'>请选所属分组</option>");
            var jsonData = $.evalJSON(data);
            if (jsonData != "") {
                for (var i = 0; i < jsonData.length; i++) {
                    $("#selPGBG_Group").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                }
            }
        });
    }
    //根据选择的分组绑定对应的分类
    function selPGBG_GroupChange() {
        $("#selPGBG_catalog").children().remove();
        $("#selPGBG_catalog").append("<option value='-1'>请选择分类</option>");
        if ($("#selPGBG_Group").val() != "-1") {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: $("#selPGBG_Group").val(), TypeId: "2", r: Math.random() }, null, function (data) {
                var jsonData = $.evalJSON(data);
                for (var i = 0; i < jsonData.length; i++) {
                    $("#selPGBG_catalog").append("<option value='" + jsonData[i].SCID + "'>" + jsonData[i].Name + "</option>");
                }
            });
        }
    }

    //查询
    function ProjectSelectMemberSearch() {
        var pody = _params();
        var podyStr = JsonObjToParStr(pody);

        LoadingAnimation("ajaxTableSelectProjectPop");
        $("#ajaxTableSelectProjectPop").load("/AjaxServers/ProjectManage/SelectProjectPop.aspx? .bit_table > *", podyStr);
    }


    //获取参数
    function _params() {

        var name = encodeURIComponent($.trim($("#txtProjectName").val()));
        if (name == '' || ($('#txtProjectName').attr('ProjectID') == null)) { $("#txtProjectName").removeAttr('ProjectID'); }
        var projectID = '';
        if ($('#txtProjectName').attr('ProjectID')) {
            projectID = $('#txtProjectName').attr('ProjectID');
        }

        var group = "";
        if ($("#selPGBG_Group").val() != "-1" && $("#selPGBG_Group").val() != undefined) {
            group = $("#selPGBG_Group").val();
        }
        var category = "";
        if ($("#selPGBG_catalog").val() && $("#selPGBG_catalog").val() != "-1" && $("#selPGBG_catalog").val() != "") {
            category = $("#selPGBG_catalog").val();
        }


        var pody = {
            name: name,
            projectID: projectID,
            group: group,
            category: category,
            rt: Math.random()
        };

        return pody;
    }

    //分页操作
    function ShowDataByPost100(pody) {

        LoadingAnimation("ajaxTableSelectProjectPop");
        $('#ajaxTableSelectProjectPop').load('/AjaxServers/ProjectManage/SelectProjectPop.aspx?r=' + Math.random() + ' .bit_table > *', pody);
    }

    function SaveSelectedProjectSelPop(parameters) {

        var dids = "";
        var dNames = "";
        var tCheckBoxs = $("#divProjectAutoCallPop .Table2 input[name=ckACP]:checked");

        tCheckBoxs.each(function (idx, ck) {
            var this$ = $(ck);
            dids += "," + this$.attr("did");
            dNames += "," + $.trim(this$.closest('td').next().text());
        }
        );
        if (dids.length > 1) {
            dids = dids.substr(1);
        }
        if (dNames.length > 1) {
            dNames = dNames.substr(1);
        }
        $.closePopupLayer('OpenProjectSelectAutoCall', true, { dids: dids, dNames: dNames });
    }

    function ckAllProjectPop(eve) {
        var $this = $(eve);
        var ckChecked = $this.is(":checked");
        if (ckChecked) {
            $("#divProjectAutoCallPop .Table2 input[name=ckACP]").attr("checked", 'true');
        } else {
            $("#divProjectAutoCallPop .Table2 input[name=ckACP]").removeAttr("checked");
        }
    }

</script>
<div class="pop pb15 openwindow" style="width: 750px;" id="divProjectAutoCallPop">
    <div class="title bold">
        <h2 style="cursor: auto;">
            选择项目</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('OpenProjectSelectAutoCall',false);">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li class="name1" style="width: 520px;">
            <select id="selPGBG_Group" onchange="javascript:selPGBG_GroupChange()" class="w90"
                style="width: 120px; *width: 100px; cursor: pointer; width: 100px\9">
            </select>
            <select id="selPGBG_catalog" class="w120" style="cursor: pointer; margin-right: 20px;">
            </select>
            <input type="text" name="txtProjectName" id="txtProjectName" class="w190" style="height: 26px;
                vertical-align: baseline;" />
        </li>
        <li class="btn" style="width: 150px; margin-top: 5px;">
            <input name="" type="button" value="查 询" onclick="javascript:ProjectSelectMemberSearch();"
                class="btnSave bold" />
            <a id="a1" href="javascript:void(0);" onclick="javascript:$.closePopupLayer('OpenProjectSelectAutoCall',true,{dids:'',dNames:''});">
                清空已选项</a> </li>
    </ul>
    <div id="ajaxTableSelectProjectPop" class="Table2">
    </div>
    <div class="btn">
        <input type="button" value="提交" class="subtim" onclick="javascript:SaveSelectedProjectSelPop();" />
        <input type="button" value="取消" class="btnattach" onclick="javascript:$.closePopupLayer('OpenProjectSelectAutoCall',false);" />
    </div>
</div>
