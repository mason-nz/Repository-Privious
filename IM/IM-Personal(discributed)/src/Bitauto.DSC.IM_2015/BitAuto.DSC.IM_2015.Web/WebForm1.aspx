<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
<div id="aa" >
    </div>
    <%--<form id="form1" runat="server">
    
    </form>--%>
</body>
<script src="Scripts/jquery-1.4.1.min.js"></script>
<script type="text/javascript">
   var str = "http://wasmip.baidu.com.cn/mip/km/archives/km_archives_main/kmArchivesMain.do?method=view&fdId%1059192测试22222http://www.baidu.comwww.baidu.com"; 
   var re=/(http:\/\/)?[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*/gi; var arry1; var arry2=[]; var obstr=str; while(arry1=re.exec(str)){ arry2.push(arry1[0]); obstr=obstr.replace(arry1[0],'######'); } var subos='' for(var i=0;i<arry2.length;i++){ subos=''+arry2[i]+''; obstr=obstr.replace('######',subos); } alert(obstr);     
</script>
</html>
