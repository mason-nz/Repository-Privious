<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TagManagementPop.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.TagManageMentPop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    
</head>
<body>
    <form id="form1" runat="server">
  <div class="pop pb15 openwindow" style="width: 600px">
        <div class="title bold">
            <h2>
                标签管理</h2>
            <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ModfiyBusinessGroup',false);">
            </a></span>
        </div>
        <div class="infor_li renyuan_cx">
            <ul class="clearfix  outTable">
                <li style="width: 260px;">
                    <label>
                        所属区域：
                    </label>
                    <select id="sltRegion" class="defselect" style="width: 100px;">
                        <option value="-1">请选择</option>
                        <asp:Repeater runat="server" ID="rptRegion">
                            <ItemTemplate>
                                <option value="<%#Eval("Value") %>">
                                    <%#Eval("Name")%></option>
                            </ItemTemplate>
                        </asp:Repeater>
                    </select>
                </li>
                <li style="width: 160px;">
                    <%--<label style="width: 50px;">
                        状态：
                    </label>
                    <input type="checkbox" name="chkStatus" value="0" />在用
                    <input type="checkbox" name="chkStatus" value="1" />停用 --%>
                    <input type="text" id="txtSearch"/>
                    </li>
                <li class="btn">
                    <input type="button" value="查询" class="btnSave bold" onclick="javascript:SearchGroup();" />
                    <input type="button" value="新增" class="btnSave bold" onclick="javascript:SearchGroup();" />
                </li>
            </ul>
        </div>
        <div class="Table2" id="divList" style="margin: 10px 0px 0px 20px; width: 550px;
            text-align: center">
            <table border="1" cellpadding="0" cellspacing="0" class="Table2List" id="tableQueryGroup"
                width="98%">
                <tbody id="trList">
                    <tr class="bold">
                        <th style="color: Black; font-weight: bold; width: 20%">
                            所属分类
                        </th>
                        <th style="color: Black; font-weight: bold; width: 10%">
                            标签名称
                        </th>
                        <th style="color: Black; font-weight: bold; width: 30%">
                            状态
                        </th>
                        <th style="color: Black; font-weight: bold; width: 20%">
                            操作
                        </th>
                        <th style="color: Black; font-weight: bold; width: 10%">
                            顺序
                        </th>
                        
                    </tr>
                    <asp:Repeater ID="rptGroup" runat="server" OnItemDataBound="rptSelectBind">
                        <ItemTemplate>
                            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                                <td>
                                    <em>
                                        <%#Eval("Name")%></em>
                                    <input type="text" class="w120" value='<%#Eval("Name") %>' style="display: none;
                                        width: 80%;" />&nbsp;
                                    <input type="hidden" value='<%#Eval("BGID") %>' />
                                </td>
                                <td>
                                    <%# GetUserCountByGroup(Eval("BGID").ToString())%>
                                </td>
                                <td>
                                    <em value="<%#Eval("RegionID")%>">
                                        <%#Eval("RegionID").ToString()=="1"?"北京":"西安"%></em>
                                    <select style="display: none; width: 80%;">
                                        <asp:Repeater ID="rptSelectArea" runat="server">
                                            <ItemTemplate>
                                                <option value="<%#Eval("Value")%>">
                                                    <%#Eval("Name")%></option>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </select>
                                </td>
                                <td>
                                    <em value="<%#Eval("CDID")%>">
                                        <%#Eval("CallNum")%></em>
                                    <select style="display: none; width: 80%;">
                                        <asp:Repeater ID="rptSelect" runat="server">
                                            <ItemTemplate>
                                                <option value="<%#Eval("CDID")%>">
                                                    <%#Eval("CallNum")%></option>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </select>
                         
                                </td>
                                <td>
                                    <em>
                                        <%#Eval("Status").ToString()=="0"?"在用":"停用"%></em>
                                </td>
                                <td>
                                    <a href="javascript:void(0);" onclick="groupSave(this)" style="display: none" class='operateafter'>
                                        保存</a> <a href="javascript:void(0);" onclick="groupCancel(this)" style="display: none"
                                            class='operateafter'>取消</a> <a href="javascript:void(0)" onclick="showEditText(this)"
                                                class='operatebefore'>修改</a>
                                    <%#Eval("Status").ToString() == "0" ? "<a href='javascript:void(0)' " + (CanStop(Eval("BGID")) > 0 ? "onclick='alertInfo()' " : " onclick='changeStatus(this)' ") + " class='operatebefore'>停用</a>" : "<a href='javascript:void(0)'  onclick='changeStatus(this)' class='operatebefore'>启用</a>"%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        <ul class="clearfix" style="text-align: center;" id="btnSumbit">
            <li>
                <div class="btn" style="width: auto">
                    <input type="button" value="新增" onclick="addNewRow()" class="btnSave bold" />&nbsp;&nbsp;&nbsp;
                    <input type="button" class="btnSave bold" onclick="javascript:$.closePopupLayer('ModfiyBusinessGroup',false);"
                        class="btnChoose" value="关闭页面" />
                </div>
            </li>
        </ul>
    </div>
    </form>
</body>
</html>

