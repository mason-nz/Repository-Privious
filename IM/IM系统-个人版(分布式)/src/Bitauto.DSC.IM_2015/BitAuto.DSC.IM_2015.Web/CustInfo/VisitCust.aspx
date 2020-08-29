<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisitCust.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.CustInfo.VisitCust" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery.blockUI.js"></script>
</head>
<script>
    //加载地区
    //省份城市方法
    var CustBaseInfoHelper = (function () {
        var triggerProvince = function () {//选中省份
            BindCity('<%=selProvince.ClientID%>', '<%=selCity.ClientID%>');
        }
        return {
            TriggerProvince: triggerProvince
        }
    })();
    BindProvince('<%=selProvince.ClientID%>'); //绑定省份
    $("select[id$='selProvince']").val('<%=ProvinceID%>');
    CustBaseInfoHelper.TriggerProvince();
    $("select[id$='selCity']").val('<%=CityID%>');

    function save() {
        var loginidstr = '<%=LoginID %>';
        if (loginidstr == '' || loginidstr == '0' || loginidstr == '-1') {
            error = "访客标识不存在";
        }
        else {
            //用户名
            var usernamevalue = $("input[id$='username']").val();
            //省
            var selProvince = $("select[id$='selProvince']").val();
            //市
            var selCity = $("select[id$='selCity']").val();
            //性别
            var Sex = $("input[name='sex']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            //电话
            var tel = $("input[id$='tel']").val();
            //备注
            var Remark = $("textarea[id$='Remark']").val();
            var VisitID = '<%=VisitID %>';
            //VisitID = '1';
            var error = "";
            if (Len(usernamevalue) > 50) {
                error = "姓名超长！";
            }
            else if (Len(tel) > 11) {
                error = "电话超长！";
            }
            else if (Len(tel) > 0 && !isTelOrMobile(tel)) {
                error = "电话格式错误！";
            }
            else if (Len(Remark) > 300) {
                error = "备注超长！";
            }
            else {
                MaskPage();
                var pody = { action: 'save', usernamevalue: escape(usernamevalue), selProvince: escape(selProvince), selCity: escape(selCity), Sex: escape(Sex), tel: escape(tel), Remark: escape(Remark), VisitID: escape(VisitID) };
                AjaxPost('AjaxServers/CustInfo/VisitCustInfoHandler.ashx', pody, null,
                 function (msg) {
                     UnMaskPage();
                     error = msg;
                     if (msg == '') {
                         var csid = '<%=RequestCSID %>'
                         top.ChangeWYTabName(csid, usernamevalue);
                     }
                 });

            }
        }
        return error;

    }
    
</script>
<body>
    <form id="form1" runat="server">
    <div id="con_two_1" class="hover">
        <table border="0" cellspacing="0" cellpadding="0" width="100%">
            <tr>
                <th width="40%">
                    姓名：
                </th>
                <td width="60%">
                    <input type="text" id="username" runat="server" disabled="disabled" />
                </td>
            </tr>
            <tr>
                <th>
                    性别：
                </th>
                <td>
                    <label>
                        <input name="sex" id="radMan" type="radio" disabled="disabled" runat="server" value="1" />先生</label><label><input
                            name="sex" type="radio" id="radWoman" disabled="disabled" runat="server" value="0" />女士</label>
                </td>
            </tr>
            <tr>
                <th>
                    地区：
                </th>
                <td>
                    <select id="selProvince" runat="server" onchange="javascript:CustBaseInfoHelper.TriggerProvince()"
                        disabled="disabled">
                    </select><select id="selCity" runat="server" disabled="disabled"></select>
                </td>
            </tr>
            <tr>
                <th>
                    电话：
                </th>
                <td>
                    <input type="text" id="tel" runat="server" disabled="disabled" />
                </td>
            </tr>
            <tr>
                <th>
                    备注：
                </th>
                <td>
                    <textarea type="text" id="Remark" runat="server" rows="5" disabled="disabled"></textarea>
                </td>
            </tr>
            <tr>
                <th>
                    访客来源：
                </th>
                <td>
                    <%=sourctypeinfo %>
                </td>
            </tr>
            <tr>
                <th>
                    发起页面：
                </th>
                <td>
                    <a href='<%=TitleURL%>' target="_blank">
                        <%=URLTitle%></a>
                </td>
            </tr>
            <tr>
                <th>
                    会话开始时间：
                </th>
                <td>
                    <%=ConvertsTime %>
                </td>
            </tr>
            <tr>
                <th>
                    应答时间：
                </th>
                <td>
                    <%=AgentSTime%>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
