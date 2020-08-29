<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateCategory_Update.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateCategory.TemplateCategory_Update" %>
<script type="text/javascript" language="javascript">
    $(document).ready(function () {
        var type = <%=type %>;
        var pID= <%=pID %>;
        var level = <%=level %>;
        var tcID = <%=TcID %>;
        var userIDs= <%=ToUserIDs %>;
        var userNames= <%=ToUserNames %>;
        if (type == 1) {
            $("#typeMesUp").attr("checked",true);
            $('#sepcialLi').css("display", "none");
        }
        else
        {
            //$("#typeMail").attr("checked",true);
            $('#sepcialLi').css("display", "inline");
        }
        typeChangeUp();
        if(level == 1){//分类只有一级
            var oSelect = $('#TypeSelect_1Up option'); 
            var optionLength = oSelect.length;
            for(var i=0;i<optionLength;i++){
                if(oSelect.eq(i).val()==tcID){
                    oSelect.eq(i).attr("selected",true);
                    break;
                }
            }
        }
        else{
            //加载二级分类
            loadTemplateCate(pID,'TypeSelect_2Up')
            //选中一级分类
            var oSelect = $('#TypeSelect_1Up option');
            var optionLength = oSelect.length;
            for(var i=0;i<optionLength;i++){
                if(oSelect.eq(i).val() == pID){
                    oSelect.eq(i).attr("selected",true); 
                }
            }
            //选中二级分类
            var oSelect = $('#TypeSelect_2Up option');
            var optionLength = oSelect.length;
            for(var i=0;i<optionLength;i++){
                if(oSelect.eq(i).val()==tcID){
                    oSelect.eq(i).attr("selected",true); 
                    break;
                }
            }
        }
        $("#PersonsGetEmail").html(userNames);
        $("#PersonsGetEmailIN").val(userIDs);
        $('#hiddenFileUp').val(tcID);      
    });
    //加载下拉框
    function loadTemplateCate(lTPID, lTsub) {
        var type = $('input[name="TemTypeUp"]:checked').val();

        $('#' + lTsub + ' option').remove();
        $('#' + lTsub).append('<option value="-1">请选择</option>');

        $('#TypeSelect_2Up').css('display', 'none');
        if (lTPID != '-1') {
            AjaxPostAsync("getTemplateCategroys.ashx", { Type: type, PID: lTPID }, null, function (data) {
                if (data != '') {//alert(data);
                    var jsonData = $.evalJSON(data);
                    $.each(jsonData.root, function (idx, item) {
                        $('#' + lTsub).append("<option value='" + item.RecID + "'>" + item.Name + "</option>")
                    });
                    if (lTsub == "TypeSelect_2Up") {
                        $('#TypeSelect_2Up').css('display', 'inline');
                    }
                    $('#hiddenFile').val("-1");
                }
                else {//没有二级 
                    $('#hiddenFileUp').val($('#TypeSelect_1Up').val());
                    $('#TypeSelect_2Up').css('display', 'none');
                }
            });
        }
        else {
            $('#hiddenFileUp').val("-1");
            $('#TypeSelect_2Up').css('display', 'none');
        }
    }

    //切换type
    function typeChangeUp() {
        $('#TypeSelect_2Up').css('display', 'none');
        $('#hiddenFileUp').val("-1");
        loadTemplateCate(0, "TypeSelect_1Up");
        if ($('input[name="TemTypeUp"]:checked').val() == "2") {
            //邮件
            $('#sepcialLi').css("display", "inline");
        }
        else {
            //短信
            $('#sepcialLi').css("display", "none");
        }
    }

    //获取select选中的value
    function getSelectVal(gSselectID, gStype) {
        //元素验证
            
        if (gStype == 'value') {
            return $('#'+gSselectID).val();
        }
    }
    
    function openSelectBrandPopup() {
        $.openPopupLayer({
            name: "SelectEmployeePopup",
            parameters: {UserIDs: $.trim($("#PersonsGetEmailIN").val())},
            url: "../AjaxServers/GetEmployeeList.aspx",
            beforeClose: function (e, data) {
                //alert('关闭事件');
            }
        }
        );
    }
    //异步修改
    function updateTemplate() {
        //验证数据
        
        var type = $("input[name='TemTypeUp']:checked").val();
        
        var tcID = $("#hiddenFileUp").val();
        var title = $.trim($("#TemplateTile").val());
        var con = $.trim($("#TemplateCon").val());
        var PersonsGetEmailIN = $.trim($("#PersonsGetEmailIN").val());
        var PersionNames = $.trim($("#PersonsGetEmail").text()); 
        if (tcID == "-1") {
            $.jAlert("请选择模板分类！");
            return;
        }
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
        //异步修改        
        podyTemplate = { Type: type,RecID:<%=recID%>, TCID: tcID, Title: escape(title), Con: escape(con), Persion: PersonsGetEmailIN, PersionName: escape(PersionNames) };
        AjaxPost('updateTemplate.ashx', podyTemplate, null, function (data) {
                //验证返回信息
                if (data == 'success') {
                    //添加完局部刷新页面
                    search();
                }
                else if (data == 'repeated') {
                    $('#addTemplatePage ')
                    $.jAlert('已存在此名称的模板，请勿重复添加！');
                }
                $.closePopupLayer('updateTemplate', true);
        });
        
        //局部重载页面
        //location.reload(true);
    }
    function hiddenInput() {
        $('#hiddenFileUp').val($('#TypeSelect_2Up').val());
    }
</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>编辑模板-<%=Template.Title %></h2>
        <span ><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('updateTemplate',false);"></a></span>
    </div>
    <ul class="clearfix ft14">
        <li>
            <label>请选择模板类型<span class="redColor">*</span>：</label>
            <span>
                <input type="radio" name="TemTypeUp" id="typeMailUp" value="2" onclick="typeChangeUp()" checked="checked" />邮件
                <input type="radio" name="TemTypeUp" id="typeMesUp" value="1" onclick="typeChangeUp()" />短信
            </span>
        </li>
        <li>
            <label>请选择分类<span class="redColor">*</span>：</label>
            <span style=" display:block; float:left; width:320px;">
                <select id="TypeSelect_1Up" onchange="loadTemplateCate(getSelectVal('TypeSelect_1Up', 'value'),'TypeSelect_2Up')"  class="w125">                    
                </select>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <select  id="TypeSelect_2Up" onchange="hiddenInput()"  class="w125">                    
                </select>
            </span>
        </li>
        <li id="sepcialLi">
            <label>邮件接收人<span class="redColor">*</span>：</label>
            <span id="PersonsGetEmail" style=" background:#ccc; display:inline-block; border:1px solid #EEE; float:left; height:20px; line-height:20px;" ></span>
                <span style=" float:left;">
                <input type="hidden" style=" display:none;" name="PersonsGetEmailIN" id="PersonsGetEmailIN"/>
                <input type="button" class="btnChoose" name="getPersion" onclick="javascript:openSelectBrandPopup();" value="点击选择" />
            </span>
        </li>
        <li>
            <label>模板标题<span class="redColor">*</span>：</label>
            <span><input name="" id="TemplateTile" type="text"  class="w190" value="<%=Template.Title %>"/></span>
        </li>
        <li>
            <label>请输入文本<span class="redColor">*</span>:</label>
            <span><textarea name="" id="TemplateCon" cols="" rows="" ><%=Template.Content %></textarea></span>
        </li>
    </ul>
   <div class="btn"> 
       <input name="" type="button" value="保 存" onclick="updateTemplate()" class="btnSave bold"/> 
       <input name="" type="button" value="取 消" class="btnCannel bold"onclick="javascript:$.closePopupLayer('updateTemplate',false);"/>
   </div>
   <input type="text" name="hiddenFileUp" id="hiddenFileUp" runat="server" style=" display:none;" />
</div>
