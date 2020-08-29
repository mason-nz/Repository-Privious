<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddFreProblem.aspx.cs"
    Inherits="BitAuto.DSC.IM_DMS2014.Web.SeniorManage.AddFreProblem" %>

<script type="text/javascript">
    //保存
    function SaveData() {
        var txt_title = $.trim($("#txt_title").val());
        var txt_url = $.trim($("#txt_url").val());
        if (txt_title == "") {
            $.jAlert("标题不能为空！", function () { $("#txt_title").focus(); });
            return;
        }
        if (!isURL(txt_url)) {
            $.jAlert("链接格式不正确！", function () { $("#txt_url").focus(); });
            return;
        }
        //提交入库
        var pody = {
            Action: '<%=Action %>',
            Title: encodeURIComponent(txt_title),
            Url: txt_url,
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
        <ul>
            <li>
                <label>
                    <em>*</em>标题：
                </label>
                <span>
                    <input id="txt_title" type="text" value="<%=title %>" class="w240" style="width: 240px;"
                        onkeyup="limit(this,40)" onfocus="limit(this,40)" onafterpaste="limit(this,40)" /><em>（20个汉字）</em>
                </span></li>
            <li>
                <label>
                    <em>*</em>链接：
                </label>
                <span>
                    <textarea id="txt_url" cols="" rows="" onkeyup="limit(this,300)" onafterpaste="limit(this,300)"
                        onfocus="limit(this,300)"><%=url %></textarea>
                </span></li>
        </ul>
        <div class="clearfix">
        </div>
        <div class="btn">
            <input type="button" value="保存" class="save w60" onclick="SaveData()" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" value="关闭" class="cancel w60 gray" onclick="javascript:$.closePopupLayer('AddFreProblem',false);" /></div>
    </div>
</div>
