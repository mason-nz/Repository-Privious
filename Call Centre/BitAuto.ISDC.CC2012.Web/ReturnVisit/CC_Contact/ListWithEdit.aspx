<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListWithEdit.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ReturnVisit.CC_Contact.ListWithEdit" %>

<!--其他任务，客户核实，客户回访 公用控件-->
<script type="text/javascript" language="javascript">
    //增删改查逻辑实现
     var contactInfoList_CC_Helper = (function(){
        var refresh = function (setFirstPage){
            <%=this.AjaxPager_Contact.RefreshFunctionName %>("", setFirstPage);
        },
        
        addNewContactInfo = function(custType) {//新增 
            $.openPopupLayer({
                name: "AddContactInfo",
                width: 550,
                url: "/ReturnVisit/CC_Contact/Add.aspx",
                parameters: {
                    PopupName: 'AddContactInfo',
                    CustID:'<%=this.CustID %>',
                    ID: 0,//代表是新增
                    CustType:custType
                },
                afterClose: function(e) {
                    if(e){
                        contactInfoList_CC_Helper.refresh();                        
                    }
                }
            });
        },
        
        editContactInfo = function(id) {//编辑
          var custType=$('select[id$=selCustType]').val();
            $.openPopupLayer({
                name: "EditContactInfo",
                width: 550,
                url: "/ReturnVisit/CC_Contact/Add.aspx",
                parameters: {
                    PopupName: 'EditContactInfo',
                    ID: id,
                    CustID:'<%=this.CustID %>',
                    CustType:custType
                },
                afterClose: function(e) {
                    if(e){
                        contactInfoList_CC_Helper.refresh();                        
                    }
                }
            });
        },
        
        delContactInfo = function(id) {//删除
            $.jConfirm('客户联系人可能存在上下及关系，确定要删除吗？', function(result) {
                if (result) {              
                    $.getJSON("/ReturnVisit/CC_Contact/Handler.ashx?callback=?", {
                         Action: 'DeleteContact',
                         ID: id
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
<input id="ContractIDForCRM" type="hidden" />
<div class="Table2" style="margin: 0; padding: 0; clear: none">
    <table style="width: 100%" border="0" class="Table2List" cellspacing="0" cellpadding="0"
        class="cxjg">
        <tr>
            <th width="10%">
                联系人
            </th>
            <th width="10%">
                职务
            </th>
            <th width="25%">
                办公电话
            </th>
            <th width="25%">
                移动电话
            </th>
            <th width="20%">
                Email
            </th>
            <th width="10%">
                操作
            </th>
        </tr>
        <asp:repeater id="repeater_Contact" runat="server">
        <ItemTemplate>
            <tr>
                <td><%#Eval("CName").ToString()%>&nbsp;</td>    
                <td><%#Eval("Title").ToString()%>&nbsp;</td>
                <td class="l">
                    <span style="float:left;">
                    <%#Eval("OfficeTel").ToString().Trim()%></span>
                    <%if (TaskType == "客户回访")
                      { %>
                            <!--座机打电话--跳转-->
                            <a
                            <%# ShowPhoneCallImg(Eval("OfficeTel").ToString()) %>
                            <%# GetAddWOrderUrl(Eval("ID").ToString(),Eval("OfficeTel").ToString(),Eval("CName").ToString(), Eval("Sex").ToString())%>>
                            <img alt="打电话" src="/images/phone.gif" border="0" style="margin-top:5px; vertical-align:middle"/>
                            </a>
                    <%}
                      else if (TaskType == "其他任务" || TaskType == "客户核实")
                      {%>
                            <!--座机打电话--执行-->
                            <a
                            <%# ShowPhoneCallImg(Eval("OfficeTel").ToString()) %>
                            href="javascript:void(0);" onclick="javascript:CallOutForCRM('<%#Eval("OfficeTel").ToString() %>','<%=FirstMemberCode %>','<%=FirstMemberName %>','<%=CustID %>','<%#Eval("CName").ToString() %>','<%#Eval("Sex").ToString() %>');">
                            <img alt="打电话" src="/images/phone.gif" border="0" style="margin-top:5px; vertical-align:middle"/>
                            </a>
                    <%}%>         
                </td>
                <td class="l">
                    <span style="float:left;">
                    <%#Eval("Phone").ToString().Trim()%>
                    </span>

                    <!--手机发短信-->
                    <a
                    <%#ShowSmSSendImg(Eval("Phone").ToString()) %>
                    href="javascript:void(0);" onclick="javascript:SendSmSForCRM('<%#Eval("Phone").ToString() %>','<%=FirstMemberCode %>','<%=FirstMemberName %>','<%=CustID %>','<%#Eval("CName").ToString() %>','<%#Eval("Sex").ToString() %>');">
                    <img alt='发送短信' src='/images/sms.png' border='0' style='float:right;padding-top:5px;vertical-align:middle' />
                    </a>

                    <%if (TaskType == "客户回访")
                      { %>                            
                            <!--手机打电话--跳转-->
                            <a
                            <%# ShowPhoneCallImg(Eval("Phone").ToString()) %>
                            <%# GetAddWOrderUrl(Eval("ID").ToString(),Eval("Phone").ToString(),Eval("CName").ToString(), Eval("Sex").ToString())%>>
                            <img alt="打电话" src="/images/phone.gif" border="0" style=" margin-top:5px;vertical-align:middle " />
                            </a>
                    <%}
                      else if (TaskType == "其他任务" || TaskType == "客户核实")
                      {%>
                            <!--手机打电话--执行-->
                            <a
                            <%# ShowPhoneCallImg(Eval("Phone").ToString()) %>
                            href="javascript:void(0);" onclick="javascript:CallOutForCRM('<%#Eval("Phone").ToString() %>','<%=FirstMemberCode %>','<%=FirstMemberName %>','<%=CustID %>','<%#Eval("CName").ToString() %>','<%#Eval("Sex").ToString() %>');">
                            <img alt="打电话" src="/images/phone.gif" border="0" style=" margin-top:5px;vertical-align:middle " />
                            </a>
                    <%}%>
                </td>
                <td style="word-wrap:break-word;text-align:left;padding:0 10px;width:180px;word-break:break-all;">
                    <%#Eval("Email").ToString()%>&nbsp;
                </td>
                <td>                    
                    <a href='javascript:void(0);' onclick='contactInfoList_CC_Helper.editContactInfo("<%#Eval("ID").ToString()%>");'>编辑</a>
                    <%# BitAuto.ISDC.CC2012.BLL.ProjectTask_ReturnVisit.Instance.CCAddCRMContractLogForRVIsHave(Convert.ToInt32(Eval("ID").ToString()))== true ?
                        "<a href='javascript:void(0);' onclick='javascript:contactInfoList_CC_Helper.delContactInfo(" + Eval("ID") + ");'>删除</a>" : ""%>
                </td>
            </tr>               
        </ItemTemplate>       
    </asp:repeater>
    </table>
</div>
<div class="pages" style="float: right">
    <uc:AjaxPager ID="AjaxPager_Contact" runat="server" />
</div>
</form>
