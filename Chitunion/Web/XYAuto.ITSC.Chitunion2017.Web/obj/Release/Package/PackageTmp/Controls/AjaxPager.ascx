<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AjaxPager.ascx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.Controls.AjaxPager" %>

共 <%=TotalCount%> 项
<asp:Literal ID="FirstPage" runat="server"></asp:Literal>
<asp:Literal ID="PrePage" runat="server"></asp:Literal>
<span class="down">
    <asp:Literal ID="List" runat="server"></asp:Literal>
</span>
<asp:Literal ID="NextPage" runat="server"></asp:Literal>
<asp:Literal ID="LastPage" runat="server"></asp:Literal>

<script type="text/javascript" language="javascript">
    function <%=FunctionName %>(suffix){
        var suffixObj={};
        if(suffix){suffixObj=jQuery.unserialise(suffix);}
        var queryStrObj = jQuery.unserialise("<%=QueryString%>");
        var queryString = jQuery.param(jQuery.extend({}, queryStrObj, suffixObj));
        AjaxPost("<%=Url%>"+ '?' + queryString, "<%=RequestData%>", null, function(data){
            $('#<%=ContentElementId%>').html(data);
        });
    }
    function <%=RefreshFunctionName %>(suffix, setFirstPage){
        if(suffix){if(suffix.length>0 && suffix[0]!='&'){suffix = '&'+suffix;}}
        else{suffix = '';}            
        var currentP = "<%=CurrentPage%>";
        if(setFirstPage){currentP = "1";}
        <%=FunctionName %>("pageSize=<%=PageSize%>&page=" + currentP + suffix);
    }
</script>