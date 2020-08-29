<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddFreProblem.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.SeniorManage.AddFreProblem" %>

<script type="text/javascript">
    //保存
    function SaveData() {
        var txt_title = $.trim($("#txt_title").val());
        var txt_url = $.trim($("#txt_url").val());
        var txt_Remark = $.trim($("#txt_Remark").val());
        var sourceTyps=$(":checked[name='sourceTypeAdd']").map(function(){return $(this).val() }).get().join(",");
        if (txt_title == "") {
            $.jAlert("标题不能为空！", function () { $("#txt_title").focus(); });
            return;
        }
        if (!isURL(txt_url)) {
            $.jAlert("链接格式不正确！", function () { $("#txt_url").focus(); });
            return;
        }
           if (sourceTyps == "") {
            $.jAlert("应用业务线不能为空！", function () { $("#txtRemark").focus(); });
            return;
        }

        //提交入库
        var pody = {
            Action: '<%=Action %>',
            Title: encodeURIComponent(txt_title),
            Url: txt_url,
            Remark: txt_Remark,
            SourceTyps: sourceTyps,
            RecID: <%=RecID %>,
            r: Math.random
        };
        AjaxPostAsync("/AjaxServers/SeniorManage/FreProblemHandler.ashx", pody, function () { }, function (data) {
            var jsonData = $.evalJSON(data);
            if (jsonData.result == "success") {
                $.closePopupLayer('AddFreProblem', true);
            }
            else {
                $.jAlert("保存失败：" + jsonData.msg);
            }
        });
    }
    function len(s) {// 获取字符串的字节长度
        s = String(s);
        return s.length + (s.match(/[^\x00-\xff]/g) || "").length; // 加上匹配到的全角字符长度
    }
    function limit(obj, limit) {
        var val = $.trim(obj.value);
        if (val == "")
            obj.value = val;
        if (len(val) > limit) {
            val = val.substring(0, limit);
            while (len(val) > limit) {
                val = val.substring(0, val.length - 1);
            };
            obj.value = val;
        }
    }
    function isURL(url) {
        var re = new RegExp("^(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&amp;%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&amp;%\$#\=~_\-]+))*$");
        if (re.test(url)) {
            return true;
        } else {
            return false;
        }
    }
       function GetSourceTypeAdd() {
        var SourceTypes=','+'<%=SourceTypes %>'+',';
            AjaxPost("/AjaxServers/SeniorManage/FreProblemHandler.ashx", { Action: 'GetSourceType' }, function () { }, function (data) {
                if (data.result) {
                    $("#divCheck").append($("#tmpAdd").tmpl(data));
                    $("input[name='sourceTypeAdd']").each(function(){
                    if(SourceTypes.indexOf(","+$(this).val()+',',0)!=-1)
                    $(this).attr("checked","checked");
                    });
                     
                if ($("[name='sourceTypeAdd']:checked").length == $("[name='sourceTypeAdd']").length) {
                         $("#sourceTypeAdd").attr("checked", "checked")
                      }
                }
            });
        }
    $(document).ready(function(){
         GetSourceTypeAdd();
    });

    
   
</script>
<div class="popup openwindow">
    <div class="title ft14">
        <h2>
            <%=ActionName%>常见问题
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AddFreProblem',false);"
            class="right">
            <img src="/images/c_btn.png" border="0" />
        </a></span>
    </div>
    <div class="content">
        <ul id="ulSearchAdd">
            <li>
                <label>
                    <em>*</em>标题：
                </label>
                <span>
                    <input id="txt_title" maxlength="20" type="text" value="<%=title %>" class="w240"
                        style="width: 240px;" /><em>（20个汉字）</em> </span></li>
            <li>
                <label>
                    <em>*</em>链接：
                </label>
                <span>
                    <textarea id="txt_url" cols="" rows="" style="height: 50px;"><%=url %></textarea>
                </span></li>
            <li>
                <label>
                    备注：
                </label>
                <span>
                    <textarea id="txt_Remark" cols="" rows="" style="height: 30px;"><%=Remark%></textarea>
                </span></li>
            <li>
                <label>
                    应用业务线：
                </label>
                <div id="divCheck" style=" width:450px; float:left; clear:none;">
                    <span style="width:100px">
                        <label>
                            <input type="checkbox" value="-1" onclick="CheckAll('sourceTypeAdd')" id="sourceTypeAdd" /><em>全部</em></label></span>
                </div>
            </li>
        </ul>
        <div class="clearfix">
        </div>
        <div class="btn">
            <input type="button" value="保存" class="save w60" onclick="SaveData()" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" value="关闭" class="cancel w60 gray" onclick="javascript:$.closePopupLayer('AddFreProblem',false);" /></div>
    </div>
</div>
<script id="tmpAdd" type="text/x-jquery-tmpl"> 
       {{each json}}
             <span style="width:100px;">
                    <label>
                        <input name="sourceTypeAdd" type="checkbox"  onclick="Check('sourceTypeAdd',this)" value="${SourceTypeValue}"/><em>${SourceTypeName}</em></label></span> </li>
       {{/each}}
</script>
