<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CallList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TrafficManage.CallList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>简易话务列表</title>
    <style type="text/css">
        table.tableList tr th
        {
            font-weight: bold;
            background: #EBF2F6;
            height: 38px;
            padding-top: 5px;
        }
        table.tableList tr td, table.tableList tr th
        {
            line-height: 28px;
            text-align: left;
            border-top: none;
            border-bottom: #ddd 1px solid;
            padding-left: 15px;
        }
        table.tableList tr.hover, .tableList table tr.hover
        {
            background: #F3F3F3;
        }
    </style>
</head>
<body>
    <div style="overflow: scroll; height: 100%;">
        <table cellpadding="0" cellspacing="0" class="tableList">
            <tr>
                <th>
                    RecID
                </th>
                <th>
                    录音流水号
                </th>
                <th>
                    话务号
                </th>
                <th>
                    坐席分机号
                </th>
                <th>
                    主叫
                </th>
                <th>
                    被叫
                </th>
                <th>
                    电话状态
                </th>
                <th>
                    接入号码
                </th>
                <th>
                    呼出类型
                </th>
                <th>
                    所属技能组
                </th>
                <th>
                    初始化时间
                </th>
                <th>
                    振铃开始时间
                </th>
                <th>
                    接通时间
                </th>
                <th>
                    坐席挂断时间
                </th>
                <th>
                    客户挂断时间
                </th>
                <th>
                    事后处理开始时间
                </th>
                <th>
                    事后处理时长
                </th>
                <th>
                    转接开始时间
                </th>
                <th>
                    转接恢复时间
                </th>
                <th>
                    录音总时长
                </th>
                <th>
                    录音地址url
                </th>
                <th>
                    创建时间
                </th>
            </tr>
            <asp:Repeater ID="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                            <%#Eval("RecID")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("SessionID")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("CallID")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("ExtensionNum")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("PhoneNum")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("ANI")%>&nbsp;
                        </td>
                        <td>
                            <%#BitAuto.ISDC.CC2012.BLL.Util.GetCallStatus(Eval("CallStatus").ToString()) %>&nbsp;
                        </td>
                        <td>
                            <%#Eval("SwitchINNum")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("OutBoundType").ToString() == "1" ? "页面呼出" : Eval("OutBoundType").ToString() == "2" ? "客户端呼出" : "无" %>&nbsp;
                        </td>
                        <td>
                            <%#Eval("SkillGroup")%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("InitiatedTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("RingingTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EstablishedTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("AgentReleaseTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CustomerReleaseTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("AfterWorkBeginTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("AfterWorkTime")%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("ConsultTime").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("ReconnectCall").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("TallTime")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("AudioURL")%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString())%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <br />
    <span style="font-size: 14px; font-weight: bold; float: right;">总数量：<label runat="server"
        id="lbCount"></label>条&nbsp;&nbsp;&nbsp;</span>
</body>
</html>
