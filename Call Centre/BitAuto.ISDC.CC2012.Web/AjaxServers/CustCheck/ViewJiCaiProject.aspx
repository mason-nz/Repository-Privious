<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewJiCaiProject.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CustCheck.ViewJiCaiProject" %>

<div class="pop pb15 openwindow" id="popfowwardtask">
    <div class="title bold">
        <h2>
            集采项目</h2>
        <span><a href="javascript:void(0);" onclick="javascript:viewJiCaiProject.close();"
            class="right"></a></span>
    </div>
    <fieldset class="tb">
        <div class="cont_cxjg" id="viewJiCaiProject_ProjectList">
            <table cellspacing="0" cellpadding="0" border="0" width="100%" class="cxjg">
                <thead>
                    <tr>
                        <th>
                            集采项目名称
                        </th>
                        <th width="10%">
                            项目类型
                        </th>
                        <th width="7%">
                            批次号
                        </th>
                        <th width="10%">
                            是否锁定
                        </th>
                        <th width="10%">
                            参加时间
                        </th>
                        <th width="11%">
                            客户联系人
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <asp:repeater id="rptProjectList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="text-align:left; padding-left:4px"><%# Eval("ProjectName") %></td>
                            <td><%# Eval("ProjectCategory").ToString()=="3"?"区域集采":"厂商集采" %></td>
                            <td><%# Eval("BatchNum") %></td>
                            <td><%# Eval("Status").ToString() == "1" ? "是" : "否" %></td>
                            <td>
                                <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>
                            </td>
                            <td><%# Eval("ContactName") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
                </tbody>
            </table>
            <div class="clear">
            </div>
        </div>
    </fieldset>
</div>
