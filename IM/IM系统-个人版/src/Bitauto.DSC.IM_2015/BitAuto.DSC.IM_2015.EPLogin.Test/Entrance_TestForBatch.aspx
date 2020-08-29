<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Entrance_TestForBatch.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.EPLogin.Test.Entrance_TestForBatch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>易湃入口-测试页面</title>
    <style type="text/css">
        #TextArea1
        {
            height: 264px;
            width: 357px;
        }
    </style>
    <script src="js/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="js/common.js" type="text/javascript"></script>
    <script src="js/json2.js" type="text/javascript"></script>
    <script src="js/Enum/Area2.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        //提交表单操作

        $(document).ready(function () {
            BindProvince("ddlProvince");
        });
        function SubmitForm() {
            var membercodestr = document.getElementById("txtMemberCode").value;
            var useridstr = document.getElementById("txtUserID").value;
            if (membercodestr == "") {
                alert("请选择有经销商的省份然后查询。");
            }
            else if (useridstr == "") {
                alert("UserID不能空。");
            }
            else {
                var membercodearry = membercodestr.split(',');
                var useridarry = useridstr.split(',');
                for (var i = 0; i < membercodearry.length; i++) {
                    var pody = { txtUserID: useridarry[i], txtMemberCode: membercodearry[i], txtKey: 'tcim!@#$', action: 'produce' };
                    AjaxPostAsync('Handler1.ashx', pody, null,
             function (msg) {
                 document.getElementById("hidTitle" + (i + 1)).value = document.title;
                 document.getElementById("loginkey" + (i + 1)).value = msg;
                 document.getElementById("hidUrl" + (i + 1)).value = window.location;
                 document.getElementById("form" + (i + 2)).submit();
             });

                }
            }
        }
        function selectprovince() {

        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        UserId：<input id="txtUserID" type="text" runat="server" /><br />
        请选择经销商省份：<select id="ddlProvince"></select><input type="button" id="selectprovince"
            value="选择" onclick="var provinceid = document.getElementById('ddlProvince').value;var pody = { provinceid: provinceid };AjaxPostAsync('Handler1.ashx', pody, null,function (msg) {document.getElementById('txtMemberCode').value = msg; });" />
        经销商编号：<input id="txtMemberCode" type="text" runat="server" />
        <a href="javascript:void(0);" target="_self" onclick="javascript:SubmitForm();">经销商聊天入口</a>
    </div>
    </form>
    <form id="form2" action="http://imep.sys1.bitauto.com/onlineservice.aspx" method="post"
    target="_blank">
    <input type="hidden" id="hidTitle1" name="PageTitle" />
    <input type="hidden" id="loginkey1" name="loginkey" />
    <input type="hidden" id="hidUrl1" name="SourceUrl" />
    </form>
    <form id="form3" action="http://imep.sys1.bitauto.com/onlineservice.aspx" method="post"
    target="_blank">
    <input type="hidden" id="hidTitle2" name="PageTitle" />
    <input type="hidden" id="loginkey2" name="loginkey" />
    <input type="hidden" id="hidUrl2" name="SourceUrl" />
    </form>
    <form id="form4" action="http://imep.sys1.bitauto.com/onlineservice.aspx" method="post"
    target="_blank">
    <input type="hidden" id="hidTitle3" name="PageTitle" />
    <input type="hidden" id="loginkey3" name="loginkey" />
    <input type="hidden" id="hidUrl3" name="SourceUrl" />
    </form>
    <form id="form5" action="http://imep.sys1.bitauto.com/onlineservice.aspx" method="post"
    target="_blank">
    <input type="hidden" id="hidTitle4" name="PageTitle" />
    <input type="hidden" id="loginkey4" name="loginkey" />
    <input type="hidden" id="hidUrl4" name="SourceUrl" />
    </form>
    <form id="form6" action="http://imep.sys1.bitauto.com/onlineservice.aspx" method="post"
    target="_blank">
    <input type="hidden" id="hidTitle5" name="PageTitle" />
    <input type="hidden" id="loginkey5" name="loginkey" />
    <input type="hidden" id="hidUrl5" name="SourceUrl" />
    </form>
    <form id="form7" action="http://imep.sys1.bitauto.com/onlineservice.aspx" method="post"
    target="_blank">
    <input type="hidden" id="hidTitle6" name="PageTitle" />
    <input type="hidden" id="loginkey6" name="loginkey" />
    <input type="hidden" id="hidUrl6" name="SourceUrl" />
    </form>
    <form id="form8" action="http://imep.sys1.bitauto.com/onlineservice.aspx" method="post"
    target="_blank">
    <input type="hidden" id="hidTitle7" name="PageTitle" />
    <input type="hidden" id="loginkey7" name="loginkey" />
    <input type="hidden" id="hidUrl7" name="SourceUrl" />
    </form>
    <form id="form9" action="http://imep.sys1.bitauto.com/onlineservice.aspx" method="post"
    target="_blank">
    <input type="hidden" id="hidTitle8" name="PageTitle" />
    <input type="hidden" id="loginkey8" name="loginkey" />
    <input type="hidden" id="hidUrl8" name="SourceUrl" />
    </form>
    <form id="form10" action="http://imep.sys1.bitauto.com/onlineservice.aspx" method="post"
    target="_blank">
    <input type="hidden" id="hidTitle9" name="PageTitle" />
    <input type="hidden" id="loginkey9" name="loginkey" />
    <input type="hidden" id="hidUrl9" name="SourceUrl" />
    </form>
    <form id="form11" action="http://imep.sys1.bitauto.com/onlineservice.aspx" method="post"
    target="_blank">
    <input type="hidden" id="hidTitle10" name="PageTitle" />
    <input type="hidden" id="loginkey10" name="loginkey" />
    <input type="hidden" id="hidUrl10" name="SourceUrl" />
    </form>
</body>
<script>
    
</script>
</html>
