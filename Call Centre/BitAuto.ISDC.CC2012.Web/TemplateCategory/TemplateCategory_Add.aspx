<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateCategory_Add.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TemplateCategory.TemplateCategory_Add" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script language="javascript" type="text/javascript">
    function openSelectBrandPopup() {
        $.openPopupLayer({
            name: "SelectEmployeePopup",
            parameters: { UserIDs: $.trim($("#PersonsGetEmailIN").val()) },
            url: "../AjaxServers/GetEmployeeList.aspx",
            beforeClose: function (e, data) {
                if ($.trim($("#PersonsGetEmail").html()) != "") {
                    $("#PersonsGetEmail").show();
                }
                else {
                    $("#PersonsGetEmail").hide();
                }
            }
        }
         );
    }
</script>
<script type="text/javascript" language="javascript">
    //预加载
    $(document).ready(function () {
        loadTemplateCateAdd(0, "TypeSelect_1Add");
    });

    //加载下拉框
    function loadTemplateCateAdd(lTPID, lTsub) {
        $('#TypeSelect_2Add').css('display', 'none');
        var type = $(':radio[name="typeAdd"]:checked').val();
        $('#' + lTsub + ' option').remove();
        $('#' + lTsub).append('<option value="-1">请选择</option>');
        if (lTPID != '-1') {
            AjaxPostAsync("getTemplateCategroys.ashx", { Type: type, PID: lTPID }, null, function (data) {
                if (data != '') {
                    var jsonData = $.evalJSON(data);
                    $.each(jsonData.root, function (idx, item) {
                        $('#' + lTsub).append("<option value='" + item.RecID + "'>" + item.Name + "</option>")
                    });
                    if (lTsub == "TypeSelect_2Add") {
                        $('#TypeSelect_2Add').css('display', 'inline');
                    }
                }
                else {//没有二级  
                    $('#TypeSelect_2Add').css('display', 'none');
                }
            });
        }
        else {
            $('#TypeSelect_2Add').css('display', 'none');
        }
    }

    //切换type
    function typeChangeAdd() {
        loadTemplateCateAdd(0, "TypeSelect_1Add");
        if ($(':radio[name="typeAdd"]:checked').val() == "2") {
            //邮件
            $('#sepcialLi').css("display", "inline");
        }
        else {
            //短信
            $('#sepcialLi').css("display", "none");
        }
    }
    function hiddenInput() {
        $('#hiddenFileAdd').val($('#TypeSelect_2Add').val());
    }
    //获取select选中的value
    function getSelectVal(gSselectID, gStype) {
        //元素验证            
        if (gStype == 'value') {
            return $('#' + gSselectID).val();
        }
    }    
</script>
<div class="pop pb15 openwindow" style="width: 650px; height: 350px;">
    <div class="title bold">
        <h2>
            添加模板</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('addTemplate',false);">
        </a></span>
    </div>
    <ul class="clearfix ft14" id="addTemplatePage">
        <li>
            <label>
                请选择模板类型<span class="redColor">*</span>：</label>
            <span>
                <input type="radio" name="typeAdd" id="typeMailAdd" value="2" onclick="typeChangeAdd()"
                    checked="checked" /><em onclick="emChkIsChoose(this);typeChangeAdd()">邮件</em>
                <input type="radio" name="typeAdd" value="1" onclick="typeChangeAdd()" /><em onclick="emChkIsChoose(this);typeChangeAdd()">短信</em>
            </span></li>
        <li>
            <label>
                分类选择<span class="redColor">*</span>：</label>
            <span style="display: block; float: left; width: 320px;">
                <select id="TypeSelect_1Add" onchange="loadTemplateCateAdd(getSelectVal('TypeSelect_1Add', 'value'),'TypeSelect_2Add')"
                    class="w125">
                </select>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <select style="display: none" id="TypeSelect_2Add" onchange="hiddenInput()" class="w125">
                </select>
            </span></li>
        <li id="sepcialLi">
            <label>
                邮件接收人<span class="redColor">*</span>：</label>
            <span><span id="PersonsGetEmail" style="display: none; background: #ccc;
                border: 1px solid #EEE; float: left; height: 20px; line-height: 20px;"></span><span
                    style="float: left;">
                    <input type="hidden" name="PersonsGetEmailIN" id="PersonsGetEmailIN" />
                    <input type="button" name="getPersion" onclick="javascript:openSelectBrandPopup();"
                        class="btnChoose" value="点击选择" />
                </span></span></li>
        <li>
            <label>
                模板标题<span class="redColor">*</span>：</label>
            <span>
                <input type="text" name="TemplateTile" id="TemplateTile" style="width: 200px; height: 21px;" />
            </span></li>
        <li style="width: 555px">
            <label>
                模板文本<span class="redColor">*</span>：</label>
            <span>
                <textarea name="TemplateCon" id="TemplateCon" style="width: 340px; height: 100px;"
                    rows="3"></textarea>
            </span></li>
    </ul>
    <div class="btn">
        <input type="button" value="保 存" onclick="addTemplate()" class="btnSave bold" />
        <input type="button" value="取 消" onclick="javascript:$.closePopupLayer('addTemplate',false);"
            class="btnCannel bold" />
    </div>
    <input type="text" name="hiddenFileAdd" id="hiddenFileAdd" value="-1" runat="server"
        style="display: none;" />
    <script language="javascript" type="text/javascript">
        function addTemplate() {
            //验证数据1
            var type = $(":radio[name^='typeAdd'][checked=true]").val();
            var tcID = $("#TypeSelect_1Add").val();
            if ($("#TypeSelect_2Add").is(":visible")) {
                tcID = $("#TypeSelect_2Add").val();
                if (tcID == "-1") {
                    $.jAlert("请选择模板分类！");
                    return;
                }
            }
            else if (tcID == "-1") {
                $.jAlert("请选择模板分类！");
                return;
            }
            var title = $.trim($("#TemplateTile").val());
            var con = $.trim($("#TemplateCon").val());
            var PersonsGetEmailIN = $.trim($("#PersonsGetEmailIN").val());
            var PersionNames = $.trim($("#PersonsGetEmail").text());

            if (title == "") {
                $.jAlert("请输入模板标题！");
                return;
            }
            if (con == "") {
                $.jAlert("请输入文本内容！");
                return;
            }
            if (type == "1") {//短信 
            }
            else if (type == "2") {
                if (PersonsGetEmailIN == "") {
                    $.jAlert("请填写邮件接收人！");
                    return;
                }
            }
            //异步添加
            podyTemplate = { Type: type, TCID: tcID, Title: encodeURI(title), Con: encodeURI(con), Persion: PersonsGetEmailIN, PersionName: PersionNames };
            AjaxPost('addTemplate.ashx', podyTemplate, null, function (data) {
                //验证返回信息
                if (data == 'success') {
                    //添加完局部刷新页面
                    window.location.reload(); 
                    //search();
                }
                else if (data == 'repeated') {
                    $.jAlert('已存在此名称的模板，请勿重复添加！');
                }

            });
            $.closePopupLayer('addTemplate', false);
        }
        function checkData() {
        }
    </script>
</div>
