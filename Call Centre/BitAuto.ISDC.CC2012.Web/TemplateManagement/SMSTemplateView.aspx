<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SMSTemplateView.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.SMSTemplateView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>查看短信模板</title>
</head>
<body>
    <div class="pop pb15 editbq">
        <div class="title bold">
            查看短信模板<span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ViewSMSTemplate',false);">
            </a></span>
        </div>
        <ul id="tagEditUL" class="clearfix ">
            <li>
                <label>
                    所属分组：</label><span><%=BGID%></span>&nbsp;&nbsp;<span><%=SCID%></span></li>
            <li>
                <label>
                    模板标题：</label><span><%=smstitle%></span></li>
            <li>
                <label>
                    模板内容：</label><span style="width:320px; float:left; clear: none;"><%=smscontent%></span></li>
        </ul>
        <div class="clearfix">
        </div>
        <div class="btn">
            <input type="button" class="btnCannel bold" onclick="javascript:$.closePopupLayer('ViewSMSTemplate',false);"
                value="关 闭" name="" /></div>
    </div>
</body>
</html>
