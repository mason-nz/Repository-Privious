<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AjaxPager.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.Controls.AjaxPager" %>
共
<%=TotalCount%>
项 &nbsp;<asp:Literal ID="FirstPage" runat="server"></asp:Literal>
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
        //加载查询结果
        LoadingAnimation('<%=ContentElementId%>');
        $.post("<%=Url%>"+ '?' + queryString, "<%=RequestData%>", function(data){
            $('#<%=ContentElementId%>').html(data);

            //如果存在这个formatStandardShortDate方法(将table中时间格式修改为标准时间格式)，执行
            if($.isFunction(window.formatStandardShortDate))
            {
                formatStandardShortDate();
            }
        });
    }
    function <%=RefreshFunctionName %>(suffix, setFirstPage){
        if(suffix){if(suffix.length>0 && suffix[0]!='&'){suffix = '&'+suffix;}}
        else{suffix = '';}            
        var currentP = "<%=CurrentPage%>";
        if(setFirstPage){currentP = "1";}
        <%=FunctionName %>("pageSize=<%=PageSize%>&page=" + currentP + suffix);
    }
    //载入时的动画. eleId为容器ID
    function LoadingAnimation(eleId) {
        $('#' + eleId).html('<div style="width:100%; height:40px;padding-top:15px;"><div class="blue-loading" style="width:50%;float:left;background-position:right;"></div><div style="float:left;padding:20px 0px 0px 10px;">正在加载中...</div></div>');
    }
</script>
