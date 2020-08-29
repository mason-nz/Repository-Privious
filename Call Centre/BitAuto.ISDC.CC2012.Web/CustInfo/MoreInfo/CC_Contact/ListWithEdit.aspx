<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListWithEdit.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CC_Contact.ListWithEdit" %>
<!--功能废弃 强斐 2016-8-3-->
<script type="text/javascript" language="javascript">

     var contactInfoList_CC_Helper = (function(){
        var refresh = function (setFirstPage){
            <%=this.AjaxPager_Contact.RefreshFunctionName %>("", setFirstPage);
        },
        
        addNewContactInfo = function(custType) {//新增 
            $.openPopupLayer({
                name: "AddContactInfo",
                width: 550,
                url: "/CustInfo/MoreInfo/CC_Contact/Add.aspx",
                parameters: {
                    PopupName: 'AddContactInfo',
                    TID: '<%= this.TID %>',
                    CustID:'<%=this.CustID %>',
                    ID: 0,//代表是新增
                    CustType:custType
                },
                afterClose: function(effectiveAction) {
                    if (effectiveAction) { contactInfoList_CC_Helper.refresh(); }
                }
            });
        },
        
        editContactInfo = function(id) {//编辑
          var custType=$('select[id$=selCustType]').val();
            $.openPopupLayer({
                name: "EditContactInfo",
                width: 550,
                url: "/CustInfo/MoreInfo/CC_Contact/Add.aspx",
                parameters: {
                    PopupName: 'EditContactInfo',
                    TID: '<%= TID %>',
                    ID: id,
                    CustID:'<%=this.CustID %>',
                    CustType:custType
                },
                afterClose: function(effectiveAction) {
                    if (effectiveAction) { contactInfoList_CC_Helper.refresh(); }
                }
            });
        },
        
        delContactInfo = function(id) {//删除
            $.jConfirm('客户联系人可能存在上下及关系，确定要删除吗？', function(result) {
                if (result) {              
                    $.getJSON("/CustInfo/MoreInfo/CC_Contact/Handler.ashx?callback=?", {
                         Action: 'DeleteContact',
                         ContactID: id
                    }, function(jd, textStatus, xhr) {
                        if (textStatus != 'success') { $.jAlert('请求错误'); }
                        else if (jd.success) {
                            contactInfoList_CC_Helper.refresh();
                        }
                        else {
                            $.jAlert('错误: ' + jd.message);
                        }
                    });
                }
            });
        };
        
        return {
            refresh: refresh,
            addNewContactInfo: addNewContactInfo,
            editContactInfo: editContactInfo,
            delContactInfo: delContactInfo
        };
    })();
</script>
<form id="form1" runat="server">
<input id="TelName" type="hidden" />
<% 
    if (IsShowDispose.ToLower() != "no")
    { %>
<h2>
    <span>客户联系人</span><a href="#this" onclick="javascript:if(contactInfoList_CC_Helper){contactInfoList_CC_Helper.addNewContactInfo($('select[id$=selCustType]').val());}">[添加联系人]</a>
</h2>
<%}%>
<table style="width: 100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr>
        <th width="10%">
            联系人
        </th>
        <th width="10%">
            职务
        </th>
        <th width="20%">
            办公电话
        </th>
        <th width="20%">
            移动电话
        </th>
        <th width="20%">
            Email
        </th>
        <th width="10%">
            负责会员
        </th>
        <% 
    if (IsShowDispose.ToLower() != "no")
    { %>
        <th width="10%">
            操作
        </th>
        <%}%>
    </tr>
    <asp:repeater id="repeater_Contact" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("CName").ToString()%></td><%--/ <%# Eval("Sex").ToString().Trim()=="1" ? "先生":"女士"%>--%>        
                <td><%#Eval("Title").ToString()%></td>
                <td class="l">
                    <span style="float:left;"><%#Eval("OfficeTel").ToString().Trim()%></span>
                    <a style="float:right;" href="javascript:void(0);" onclick="功能废弃">
                    <img alt="打电话" src="/images/phone.gif" border="0" style=" margin-top:5px;"/></a>
                </td>
                <td class="l">
                    <span style="float:left;"><%#Eval("Phone").ToString().Trim()%></span>
                    <a style="float:right;" href="javascript:void(0);" onclick="功能废弃">
                    <img alt="打电话" src="/images/phone.gif" border="0" style=" margin-top:5px;" /></a>                    
                </td>
                <td ><%#Eval("Email").ToString()%></td>
                <td><%#ShowManageMember(Eval("ID").ToString())%></td>
                 <% 
                     if (IsShowDispose.ToLower() != "no")
                     { %>
                <td>
                    <%--<a href='javascript:ViewContactInfo(<%#Eval("ID") %>);' >查看</a>--%>
                    <%# true == true
                        ? "<a href='#this' onclick='javascript:contactInfoList_CC_Helper.editContactInfo(" + Eval("ID") + ");'>编辑</a>"
                        : "编辑"
                    %>
                   <%# BitAuto.ISDC.CC2012.Web.WebUtil.Converter.String2Int(Eval("OriginalContactID").ToString(), -2) <= 0
                        ? "<a href='#this' onclick='javascript:contactInfoList_CC_Helper.delContactInfo(" + Eval("ID") + ");'>删除</a>"
                        : "&nbsp;删除"
                    %>
                </td>
                <%} %>
            </tr>               
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="color_hui">
                <td class><%#Eval("CName").ToString()%></td><%--/ <%# Eval("Sex").ToString().Trim() == "1" ? "先生" : "女士"%>--%>
                <td class><%#Eval("Title").ToString()%></td>
                <td class="l">
                    <span style="float:left;"><%#Eval("OfficeTel").ToString()%></span>
                    <a style="float:right;"href="javascript:void(0);" onclick="功能废弃">
                        <img alt="打电话" src="/images/phone.gif" border="0" style=" margin-top:5px;" /></a>
                    
                </td>
                <td class="l">
                    <span style="float:left;"><%#Eval("Phone").ToString()%></span>
                    <a style="float:right;"href="javascript:void(0);" onclick="功能废弃">
                        <img alt="打电话" src="/images/phone.gif" border="0" style=" margin-top:5px;" /></a>
                </td>
                <td ><%#Eval("Email").ToString()%></td>
                <td><%#ShowManageMember(Eval("ID").ToString())%></td>
                <% 
                    if (IsShowDispose.ToLower() != "no")
                    { %>
                <td>
                    <%--<a href='javascript:ViewContactInfo(<%#Eval("ID") %>);' >查看</a>--%>
                    <%# true == true
                        ? "<a href='#this' onclick='javascript:contactInfoList_CC_Helper.editContactInfo(" + Eval("ID") + ");'>编辑</a>"
                        : "编辑"
                    %>
                   <%# BitAuto.ISDC.CC2012.Web.WebUtil.Converter.String2Int(Eval("OriginalContactID").ToString(), -2) <= 0
                        ? "<a href='#this' onclick='javascript:contactInfoList_CC_Helper.delContactInfo(" + Eval("ID") + ");'>删除</a>"
                        : "&nbsp;删除"
                    %>
                </td>
                <%} %>
            </tr>
        </AlternatingItemTemplate>
    </asp:repeater>
</table>
<div class="pages" style="float:right">
    <uc:AjaxPager ID="AjaxPager_Contact" runat="server" PageSize="5"/>
</div>
</form>