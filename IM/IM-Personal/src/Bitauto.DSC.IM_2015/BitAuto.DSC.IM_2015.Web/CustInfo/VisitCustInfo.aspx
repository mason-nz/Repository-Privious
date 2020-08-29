<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisitCustInfo.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.CustInfo.VisitCustInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<%--<script src="/Scripts/jquery-1.4.1.js" language="javascript" type="text/javascript"></script>
<script src="/Scripts/jquery-1.4.1.min.js" language="javascript" type="text/javascript"></script>
<script type="text/javascript" charset="utf-8" src="/Scripts/jquery.jmpopups-0.5.1.pack.js"></script>--%>
<script type="text/javascript" charset="utf-8" src="/Scripts/Enum/Area2.js"></script>
<%--<script type="text/javascript" charset="utf-8" src="/Scripts/common.js"></script>--%>
<script type="text/javascript">
    
    //默认加载访客信息
    Load(1);


    function edit(isonly) {
        if (isonly == "1") {
            $("input[id$='username']").attr("disabled", "disabled");
            $("input[id$='radMan']").attr("disabled", "disabled");
            $("input[id$='radWoman']").attr("disabled", "disabled");
            $("select[id$='selProvince']").attr("disabled", "disabled");
            $("select[id$='selCity']").attr("disabled", "disabled");
            $("input[id$='tel']").attr("disabled", "disabled");
            $("textarea[id$='Remark']").attr("disabled", "disabled");
        }
        else if (isonly == "0") {
            $("input[id$='username']").removeAttr("disabled");
            $("input[id$='radMan']").removeAttr("disabled");
            $("input[id$='radWoman']").removeAttr("disabled");
            $("select[id$='selProvince']").removeAttr("disabled");
            $("select[id$='selCity']").removeAttr("disabled");
            $("input[id$='tel']").removeAttr("disabled");
            $("textarea[id$='Remark']").removeAttr("disabled");
        }
    }
    function editsave() {
        //save
        var op = $("#id_editsave").attr("class");
        if (op == "edit") {
            edit("0");
            $("#id_editsave").attr("class", "save");
        }
        else if (op == "save") {
            var result = save();
            if (result == "") {
                edit("1");
                $("#id_editsave").attr("class", "edit");
            }
            else
            {
               $.jAlert(result);
            }
        }
    }
    function Load(loadtype) {
        LoadingAnimation("VisitServiceDiv");
        if (loadtype == 1) {
            $("#two2").attr("class", "");
            $("#two1").attr("class", "hover");
            var pody = 'loginid=<%=loginid%>&CSID=<%=RequestCSID %>&VisitID=<%=RequestVisitID%>&r=' + Math.random();
            $('#VisitServiceDiv').empty();
            $('#VisitServiceDiv').load('CustInfo/VisitCust.aspx', pody, null);
            $("#id_editsave").css("display","");
            $("#id_editsave").attr("class", "edit");
        }
        else if (loadtype == 2) {
            $("#two2").attr("class", "hover");
            $("#two1").attr("class", "");
            var pody = 'loginid=<%=loginid%>&VisitID=<%=RequestVisitID%>&r=' + Math.random();
            $('#VisitServiceDiv').empty();
            $('#VisitServiceDiv').load('CustInfo/ServiceInfo.aspx', pody, null);
            $("#id_editsave").css("display","none");
        }
    }
</script>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hdcustid" runat="server" />
    <div class="kh_box">
        <div class="menubox">
            <ul class="kh_fwxx">
                <li id="two1" onclick="Load(1)" class="hover" style="background: none;">访客信息<span>|</span></li>
                <li id="two2" onclick="Load(2)" style="background: none;">服务信息<span></span></li>
                <div class="edit" id="id_editsave" onclick="editsave()">
                </div>
            </ul>
        </div>
        <div class="contentbox" id="VisitServiceDiv">
        </div>
    </div>
    </form>
</body>
</html>
