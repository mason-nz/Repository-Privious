<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CallRecordList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo.CallRecordList" %>

<!--客户回访-话务记录 强斐 2016-8-17-->
<%@ Register Src="../../Controls/AjaxPager.ascx" TagName="AjaxPager" TagPrefix="uc1" %>
<script type="text/javascript">
    $(document).ready(function () {
        //呼入
        $("[name='playMav']").live("click", function (e) {
            e.preventDefault();
            var url = $(this).attr("adurl");
            openPlayAudioPopup(url);
        });
    });

    function openPlayAudioPopup(audioURL) {
        $.openPopupLayer({
            name: "PlayAudioAjaxPopup",
            parameters: { AudioURL: audioURL },
            popupMethod: 'Post',
            url: "/AjaxServers/TrafficManage/PlayAudioLayer.aspx"
        });
    }
</script>
<form id="form1" runat="server">
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr class="back" onmouseout="this.className='back'">
        <th name='Col_Contact' style="width: 10%">
            联系人
        </th>
        <th name='Col_ANI' style="width: 12%;">
            主叫号码
        </th>
        <th name='Col_PhoneNum' style="width: 12%;">
            被叫号码
        </th>
        <th name='Col_BeginTime' style="width: 20%;">
            通话时间
        </th>
        <th name='Col_Status' style="width: 7%;">
            呼叫类型
        </th>
        <th name='Col_AgentName' style="width: 8%;">
            坐席
        </th>
        <th name='Col_AgentNum' style="width: 8%;">
            工号
        </th>
        <th name='Col_TaskID' style="width: 15%;">
            任务ID
        </th>
        <th style="width: 8%;">
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr>   
                <td name='Col_Contact'>
                    <%# Eval("Contact")%>&nbsp;
                </td>               
                <td name='Col_ANI'>
                    <%# Eval("ANI")%>&nbsp;
                </td> 
                <td name='Col_PhoneNum'>
                    <%# Eval("PhoneNum")%>&nbsp;
                </td> 
                <td name='Col_BeginTime'>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("BeginTime").ToString())%>&nbsp;
                </td> 
                <td name='Col_Status'>
                    <%#BitAuto.ISDC.CC2012.BLL.Util.GetCallStatus(Eval("CallStatus").ToString())%>&nbsp;
                </td>                 
                 <td name='Col_AgentName'>
                    <%# Eval("AgentName")%>&nbsp;
                </td>  
                  <td name='Col_AgentNum'>
                    <%# Eval("AgentNum")%>&nbsp;
                </td>                                                                  
                <td name='Col_TaskID'>
                 <%#GetViewUrl(Eval("TaskID").ToString(),Eval("BGID").ToString(),Eval("SCID").ToString()) %>&nbsp;
                </td>
                <td >                
                 <%# Eval("AudioURL").ToString().Trim() == "" ? "" : "<a href='javascript:void(0);' onclick='javascript:ADTTool.PlayRecord(\"" + Eval("AudioURL").ToString() + "\");' title='播放录音' ><img src='../../Images/callTel.png' /></a>"%>&nbsp;
                </td> 
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="color_hui">
                <td name='Col_Contact'>
                    <%# Eval("Contact")%>&nbsp;
                </td>               
                <td name='Col_ANI'>
                    <%# Eval("ANI")%>&nbsp;
                </td> 
                <td name='Col_PhoneNum'>
                    <%# Eval("PhoneNum")%>&nbsp;
                </td> 
                <td name='Col_BeginTime'>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("BeginTime").ToString())%>&nbsp;
                </td> 
                <td name='Col_Status'>
                    <%#BitAuto.ISDC.CC2012.BLL.Util.GetCallStatus(Eval("CallStatus").ToString())%>&nbsp;
                </td>                 
                 <td name='Col_AgentName'>
                    <%# Eval("AgentName")%>&nbsp;
                </td>  
                  <td name='Col_AgentNum'>
                    <%# Eval("AgentNum")%>&nbsp;
                </td>                                                                  
                <td name='Col_TaskID'>
                  <%#GetViewUrl(Eval("TaskID").ToString(),Eval("BGID").ToString(),Eval("SCID").ToString()) %>&nbsp;
                </td>
                <td >                
                 <%# Eval("AudioURL").ToString().Trim() == "" ? "" : "<a href='javascript:void(0);' onclick='javascript:ADTTool.PlayRecord(\"" + Eval("AudioURL").ToString() + "\");' title='播放录音' ><img src='../../Images/callTel.png' /></a>"%>&nbsp;
                </td> 
            </tr>
        </AlternatingItemTemplate>
    </asp:repeater>
</table>
<!--分页-->
<div class="pages" style="float: right">
    <uc1:AjaxPager ID="AjaxPager_CallRecord" runat="server" PageSize="5" />
</div>
</form>
