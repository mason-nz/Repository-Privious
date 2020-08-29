<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComSenManageList.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.SeniorManage.ComSenManageList" %>

    <!--列表开始-->
<table border="0" cellspacing="0" cellpadding="0">
    <thead>
        <th colspan="7" style="background: #F9F9F9">
            <div class="btn btn2 right">
                <input type="button" value="标签管理" onclick="LabelEditPop()" class="save w80 gray" />
                <input type="button" value="新增常用语" onclick="AddConSentencePop()" class="cancel w85 gray" /></div>
        </th>
    </thead>
    <tr>
        <th width="65%">
            常用语
        </th>
        <th width="15%">
            标签
        </th>
        <th width="15%">
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
                        <ItemTemplate>
                            <tr style="cursor:pointer" class="" onclick="">
                                <td class="cName" name="csName">
                                    <%#Eval("Name") %>&nbsp;
                                </td>
                                <td name="ltName">
                                    <%#Eval("ltName")%>&nbsp;
                                </td>
                                <td>
                                    <a href="javascript:void(0)" csid="<%#Eval("CSID")%>" ltid="<%#Eval("LTID")%>" onclick="EditConSentencePop(this)">修改</a> 
                                    <a href="javascript:void(0)" csid="<%#Eval("CSID")%>" ltid="<%#Eval("LTID")%>" onclick="DeleteConSentence(this)">删除</a> 
                                    <%#Eval("CSID").ToString().Trim() != MinCSID.ToString() ? "<a href=\"javascript:void(0)\" onclick=\"MoveUpOrDown('" + Eval("CSID").ToString() + "','up')\">上移</a>" : "<span style=\"color:#666;\">上移</span>"%>
                                    <%#Eval("CSID").ToString().Trim() != MaxCSID.ToString() ? "<a href=\"javascript:void(0)\" onclick=\"MoveUpOrDown('" + Eval("CSID").ToString() + "','down')\">下移</a>" : "<span style=\"color:#666;\">下移</span>"%>
                                </td>                                
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
</table>
    <!--列表结束-->
<!--分页开始-->
<div class="pagesnew" style="float: right; margin: 10px;" id="itPage">
<%--<p class="pageP">
            每页显示条数 <a href="#" name="apageSize" v='20'>20</a>&nbsp;&nbsp; <a href="#" name="apageSize"
                v='50'>50</a>&nbsp;&nbsp; <a href="#" name="apageSize" v='100'>100</a>
        </p>--%>
        <p>
    <asp:literal runat="server" id="litPagerDown"></asp:literal>
    </p>
</div>
