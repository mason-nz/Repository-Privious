<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnswerList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage.AnswerList" %>

<!--已接来电查询 强斐 2016-8-16-->
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
<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th name='Col_TaskID' style="width: 80px;">
            任务ID
        </th>
        <th name='Col_AgentName' style="width: 40px;">
            坐席
        </th>
        <th name='Col_AgentNum' style="width: 40px;">
            工号
        </th>
        <th name='Col_ANI' style="width: 80px;">
            主叫号码
        </th>
        <th name='Col_PhoneNum' style="width: 50px;">
            被叫号码
        </th>
        <th name='Col_BeginTime' style="width: 125px;">
            开始时间
        </th>
        <th name='Col_EndTime' style="width: 125px;">
            结束时间
        </th>
        <th name='Col_TallTime' style="width: 50px;">
            时长(秒)
        </th>
        <th name='Col_业务线' style="width: 65px;">
            业务线
        </th>
        <th name='Col_技能组' style="width: 85px;">
            技能组
        </th>
        <th name='Col_Score' style="width: *">
            满意度
        </th>
        <th style="width: 30px;">
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                <td name='Col_TaskID'>
                  <%#GetViewUrl(Eval("TaskID").ToString(),Eval("BGID").ToString(),Eval("SCID").ToString()) %>&nbsp;
                </td>
                <td name='Col_AgentName'>
                    <%# Eval("AgentName")%>&nbsp;
                </td>  
                <td name='Col_AgentNum'>
                    <%# Eval("AgentNum")%>&nbsp;
                </td>                 
                <td name='Col_ANI'>
                    <%#BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo.CustBaseInfoHelper.GetLinkToCustByTel(Eval("ANI").ToString())%>&nbsp;
                </td> 
                <td name='Col_PhoneNum'>
                    <%# Eval("PhoneNum")%>&nbsp;
                </td> 
                <td name='Col_BeginTime'>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("BeginTime").ToString())%>&nbsp;
                </td> 
                <td name='Col_EndTime'>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EndTime").ToString())%>&nbsp;
                </td> 
                <td name='Col_TallTime'>
                    <%# Eval("TallTime")%>&nbsp;
                </td> 
                <td name='Col_业务线'>
                    <%# Eval("SwitchINNum_Name")%>&nbsp;
                </td> 
                <td name='Col_技能组'>
                    <%# Eval("SkillGroup_Name")%>&nbsp;
                </td> 
                <td name='Col_Score'>
                    <%# Eval("Score")%>&nbsp;
                </td>
                <td >                
                 <%# Eval("AudioURL").ToString().Trim() == "" ? "" : "<a href='javascript:void(0);' onclick='javascript:ADTTool.PlayRecord(\"" + Eval("AudioURL").ToString() + "\");' title='播放录音' ><img src='../../Images/callTel.png' /></a>"%>&nbsp;
                </td> 
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<!--分页-->
<div class="pageTurn mr10">
    <p>
        <asp:literal runat="server" id="litPagerDown"></asp:literal>
    </p>
</div>
