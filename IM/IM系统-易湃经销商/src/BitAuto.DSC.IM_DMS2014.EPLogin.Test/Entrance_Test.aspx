<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Entrance_Test.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.EPLogin.Test.Entrance_Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>易湃入口-测试页面</title>
    <style type="text/css">
        #TextArea1
        {
            height: 264px;
            width: 357px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        //提交表单操作
        function SubmitForm() {
            document.getElementById("hidTitle").value = document.title;
            document.getElementById("hidUrl").value = window.location;
            document.getElementById("form2").submit();
        }

        function check() {
            if (document.getElementById("radioType2").checked) {
                document.getElementById("txtJson").disabled = 'disabled';
                document.getElementById("txtUserID").removeAttribute('disabled');
                document.getElementById("txtMemberCode").removeAttribute('disabled');
            }
            else {
                document.getElementById("txtJson").removeAttribute('disabled');
                document.getElementById("txtUserID").disabled = 'disabled';
                document.getElementById("txtMemberCode").disabled = 'disabled';
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    转换前Json内容：<br />
    UserId——对应表EPVisitLog.LoginID【易湃登录账号主键ID】<br />
    UserName——对应表EPVisitLog.ContractName【易湃登录账号名称】<br />
    Mobile——对应表EPVisitLog.ContractPhone【易湃登录账号，联系人手机号码】<br />
    Email——对应表EPVisitLog.ContractEmail【易湃登录账号，联系人Email】<br />
    Post——对应表EPVisitLog.ContractJob【易湃登录账号，联系人职务】<br />
    DealerId——对应表EPVisitLog.MemberCode【易湃登录账号，所属易湃会员编号】<br />
    DateTimeFormat——【易湃登录账号，时间戳，格式yyyyMMddHHmmss】<br />
    AppId——对应表EPVisitLog.VisitRefer【易湃登录账号，所属应用模块ID】<br />
    生成加密内容方式：
    <label for="radioType1" onclick="check();" style='color:Red;' >
    <input id="radioType1" type="radio" value='1' name="radioType" checked="true" runat="server" />手动(仅需修改textarea内容)</label>
     <label for="radioType2" onclick="check();"style='color:Red;'>             
     <input id="radioType2" type="radio" value='0' name="radioType"  runat="server" />自动(仅需修改UserId、经销商编号输入框内容)</label><br />
     <textarea id="txtJson" rows="10" cols="60" runat="server"></textarea><br />
     UserId：<input id="txtUserID" type="text" disabled="disabled" runat="server"/><br />
     经销商编号：<input id="txtMemberCode" type="text" disabled="disabled" runat="server"/>
    <br />
    加密key：<input id="txtKey" type="text" value="xxxx@#$&"  runat="server"/>
    <asp:Button ID="btnChanged" runat="server"
        Text="转换" onclick="btnChanged_Click" /><br />
    转换后内容：
    <textarea id="txtJsonStr" rows="8" cols="60" runat="server"></textarea>
    <br />
    注：下面“经销商聊天入口”链接，需要Post3个参数，分别为：<br />
    PageTitle：当前页面title<br />
    loginkey：易湃加密后的jsonkey<br />
    SourceUrl：易湃真正来源url（由于易湃用中转页调整到聊天入口，因此Request.UrlReferrer获取不到真正的地址）<br />
    <a style="margin-left: 600px;" href="javascript:void(0);" target="_self" onclick="javascript:SubmitForm();">经销商聊天入口</a>
    </div>
    </form>

    <form id="form2" action="http://imep.sys1.bitauto.com/onlineservice.aspx" method="post" target="_blank" >
    <input type="hidden" id="hidTitle" name="PageTitle" />
    <input type="hidden" id="loginkey" name="loginkey" runat="server" />
    <input type="hidden" id="hidUrl" name="SourceUrl" />
    </form>
</body>
 <script type="text/javascript" language="javascript">
     check();
 </script>
</html>
