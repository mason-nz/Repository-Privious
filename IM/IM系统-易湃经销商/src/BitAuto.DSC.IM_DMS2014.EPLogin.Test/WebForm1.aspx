<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.EPLogin.Test.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<script src="js/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="js/common.js" type="text/javascript"></script>
<script src="js/json2.js" type="text/javascript"></script>
<script src="js/Enum/Area2.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    function sub() {
        document.getElementById("form2").submit();
    }
</script>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
    <form id="form2" action="http://127.0.0.1:63122/api/attend/ApprovalApply" method="post"
    target="_self">
    <%--    ApplyType=11&CardTimeStartTime=2016-10-24 19:21&Content=1111112222&EmployeeNumber=5913&ApplyType=&ApplyTypeName=&CardTimeEndTime=2016-10-24 19:21&StartTime=&EndTime=&ApplyCertify=&TransferLetter=&BabyBirthday=&SelectTime=
    string EmployeeNumber, string ApplyType, string ApplyTypeName, string CardTimeStartTime, string CardTimeEndTime, string StartTime, string EndTime, string ApplyCertify, string TransferLetter, string BabyBirthday, string SelectTime, string Content--%>

    <%--string EmployeeNumber, string ApplyType, string ApplyTypeName, string CardTimeStartTime, string CardTimeEndTime, string StartTime, string EndTime, string ApplyCertify, string TransferLetter, string BabyBirthday, string SelectTime, string Content--%>

    <input type="hidden" id="aaaa" name="JsonStr" value="[{employeeNumber:5913,applyGeneral:400188,applyGeneralID:11,result:False,common:不同意}]" />
    
    
    
    
  
    </form>
    <input type="button" value="dadfasd" onclick="sub()" />
</body>
</html>
