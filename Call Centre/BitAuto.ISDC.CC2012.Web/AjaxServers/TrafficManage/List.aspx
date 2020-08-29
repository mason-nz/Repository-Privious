<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage.List" %>

<script type="text/javascript">
    $(document).ready(function () {
        //呼出
        $("#liANI").hide();
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
        <th>
            任务ID
        </th>
        <th>
            任务分类
        </th>
        <th>
            坐席
        </th>
        <th>
            工号
        </th>
        <th>
            被叫号码
        </th>
        <th>
            开始时间
        </th>
        <th>
            结束时间
        </th>
        <th>
            时长(秒)
        </th>
        <th>
            呼叫类别
        </th>
        <th>
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                <td name='Col_TaskID'>                
                  <%#GetViewUrl(Eval("TaskID").ToString(),Eval("BGID").ToString(),Eval("SCID").ToString()) %>&nbsp;
                </td>
                <td name='Col_TaskTypeName'>
                    <%# Eval("TaskTypeName")%>&nbsp;
                </td> 
                 <td name='Col_AgentName'>
                    <%# Eval("AgentName")%>&nbsp;
                </td>  
                  <td name='Col_AgentNum'>
                    <%# Eval("AgentNum")%>&nbsp;
                </td>      
                <td name='Col_PhoneNum'>
                    <%#BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo.CustBaseInfoHelper.GetLinkToCustByTel(Eval("PhoneNum").ToString())%>&nbsp;
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
                <td>
                     <%#Eval("OutBoundType").ToString() == "1" ? "页面" : Eval("OutBoundType").ToString() == "2" ? "客户端" : Eval("OutBoundType").ToString() == "4" ? "自动" : "&nbsp;--&nbsp; "%>&nbsp;
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
