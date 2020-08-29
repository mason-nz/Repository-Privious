<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SwitchPage.aspx.cs" Inherits="XYAuto.BUOC.Chitunion2017.WebAPIWeChat.SwitchPage1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--<asp:Label ID="Label1" runat="server" Text="" ></asp:Label>--%>
            <asp:Label ID="MsgTitle" runat="server" Text="ffffff"></asp:Label>
            <asp:TextBox ID="TextBox1" runat="server" Text="xiaom"></asp:TextBox>
           <input runat="server"  type="hidden" id="usertoken"  value="555"/>
        </div>
    </form>

</body>
<script>
    //var cookie = {
    //    set: function (key, val) {//设置cookie方法
    //        document.cookie = key + "=" + val + ";"  //设置cookie
    //    },
    //    get: function (key) {//获取cookie方法
    //        /*获取cookie参数*/
    //        var getCookie = document.cookie.replace(/[ ]/g, "");  //获取cookie，并且将获得的cookie格式化，去掉空格字符
    //        var arrCookie = getCookie.split(";")  //将获得的cookie以"分号"为标识 将cookie保存到arrCookie的数组中
    //        var tips;  //声明变量tips
    //        for (var i = 0; i < arrCookie.length; i++) {   //使用for循环查找cookie中的tips变量
    //            var arr = arrCookie[i].split("=");   //将单条cookie用"等号"为标识，将单条cookie保存为arr数组
    //            if (key == arr[0]) {  //匹配变量名称，其中arr[0]是指的cookie名称，如果该条变量为tips则执行判断语句中的赋值操作
    //                tips = arr[1];   //将cookie的值赋给变量tips
    //                break;   //终止for循环遍历
    //            }
    //        }
    //        return tips;
    //    }
    //}
    //document.getElementById("MsgTitle").textContent = cookie.get('xy_usertoken');
    //document.getElementById("MsgTitle").textContent = cookie.get('xy_usertoken');
    //var usertoken = cookie.get('xy_usertoken');
    //document.writeln("获取token：" + usertoken)
<%--    var user = <% XYAuto.ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.GetUserInfoByToken(MsgTitle.Text);%>;--%>
    document.write(JSON.stringify(document.cookie));

    function getCookie(name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
        if (arr = document.cookie.match(reg))
            return unescape(arr[2]);
        else
            return null;
    }
    var token = document.writeln("获取：" + getCookie("xy_usertoken"));
    document.getElementById("usertoken").value = token;
<%--    var user = <% XYAuto.ITSC.Chitunion2017.BLL.UserManage.UserManage.Instance.GetUserInfoByToken(usertoken.Value);%>;   
    document.writeln("奶奶熊："+usertoken.Value);--%>
</script>
</html>
