<%@ Page Language="C#" Title="易团购外呼录音页" AutoEventWireup="true" CodeBehind="YTGActivityTaskCallRecordList.aspx.cs" 
Inherits="BitAuto.ISDC.CC2012.Web.YTGActivityTask.AjaxServers.YTGActivityTaskCallRecordList" %>

<script type="text/javascript">
    function openPlayAudioPopup(audioURL) {
        $.openPopupLayer({
            name: "PlayAudioAjaxPopup",
            parameters: { AudioURL: audioURL },
            popupMethod: 'Post',
            url: "/AjaxServers/AudioManager/PlayAudioLayer.aspx"
        });
    }
</script>
<div class="bit_table">
    <table style="width: 100%" border="0" cellspacing="0" cellpadding="0" class="tableList"
        style="margin-bottom: 40px;">
        <tr>
            <th width="18%">
                通话开始时间
            </th>
            <th width="18%">
                通话结束时间
            </th>
            <th width="10%">
                操作人
            </th>
            <th width="10%">
                播放录音
            </th>
        </tr>
        <asp:repeater id="repeater_Contact" runat="server">
        <ItemTemplate>
            <tr>
                
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("BeginTime").ToString())%>&nbsp;
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EndTime").ToString())%>&nbsp;
                </td>
                <td>
                   <%#BitAuto.ISDC.CC2012.BLL.Util.GetNameInHRLimitEID(Convert.ToInt32(Eval("CreateUserID").ToString()))%>               
                </td>
                   <td >                
                 <%# Eval("AudioURL").ToString().Trim() == "" ? "" : "<a href='javascript:void(0);' onclick='javascript:ADTTool.PlayRecord(\"" + Eval("AudioURL").ToString() + "\");' title='播放录音' ><img src='../../Images/callTel.png' /></a>"%>&nbsp;

                </td> 
                     
            </tr>               
        </ItemTemplate>
    </asp:repeater>
    </table>
</div>
