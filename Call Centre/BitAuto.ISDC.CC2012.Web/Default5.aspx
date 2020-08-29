<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="Default5.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.Default5" %>

<%@ Register src="CustInfo/DetailV/UCCstMember.ascx" tagname="UCCstMember" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
    <script type="text/javascript" src="/js/bit.dropdownlist.js"></script>
    <script type="text/javascript">
    
    
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
    主品牌to子品牌to车型:
    <select id="producer1">
    </select>
    <select id="master4">
    </select>
    <select id="serial4"></select>
    <select id="cartype4"></select>
    <br />
    <br />
    <br />
    <br />
    <br />
    
    主品牌to子品牌to车型:
    <select id="Select1" style="width: 100px;">
    </select><select id="Select2" style="width: 100px;"></select><select id="Select3" style="width: 100px;" onmouseover="javascript:FixWidth(this);"></select>
    <a target="_blank"></a>
    <script>
//        var options = {
//            container: { master: "master4", serial: "serial4", cartype: "cartype4" },
//            include: { serial: "1", cartype: "1" },
//            datatype: 0//,
//            //binddefvalue: {}
//        };
//        //主品牌to子品牌to车型
//        new BindSelect(options).BindList();

        var options2 = {
            container: { master: "Select1", serial: "Select2", cartype: "Select3" },
            include: { serial: "1", cartype: "1" },
            datatype: 0,
            binddefvalue: { master: "111", serial: "2167", cartype: "100114" }
        };
        new BindSelect(options2).BindList();
    </script>

    </form>
    <uc1:UCCstMember ID="UCCstMember1" runat="server" />
</asp:Content>
