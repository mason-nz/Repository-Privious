<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlayRecordList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CTI.PlayRecordList" %>

<script src="/Js/jquery.media.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        //PlayRecordingSound('<%=RequestRecordURL %>');

        var param = {
            RecordURL: '<%=RequestRecordURL %>',
            R: Math.random()
        }
        $("#divRecordPlugin2").load("/CTI/PlayRecordListAjax.aspx", param);
    });

    function Play(obj) {
        try {
            $('#divRecordPlugin').find('object')[0].controls.stop(); //停止播放
        } catch (e) {
        $.jAlert("当前浏览器不能停止播放录音,出错："+ e);
        }
        var url = $(obj).attr("recordurl");
        //$("#divRecordPlugin").empty();
//        $("#divRecordPlugin").html("");
//        $("#divRecordPlugin").append("<a>WAV</a>");

        var param = {
            RecordURL: url,            
            R: Math.random()
        }
        $("#divRecordPlugin2").load("/CTI/PlayRecordListAjax.aspx", param);
        //PlayRecordingSound(url);
    }
    function PlayRecordingSound(path) {
        if (path == '')
            $('#divRecordPlugin').html('参数无效');
        else {
            $('#divRecordPlugin a').attr('href', path).html(path);
            $.fn.media.mapFormat('mp3', 'winmedia');
            $('#divRecordPlugin a').media({ width: 600, height: 62, autoplay: true });
        }
    }

    function closePopupLayerPlayRecordingSound() {
        try {
            $('#divRecordPlugin').find('object')[0].controls.stop(); //停止播放
            $.closePopupLayer('PlayRecordLayer', false);
        } catch (e) {
            $.closePopupLayer('PlayRecordLayer', false);
        }
    }
</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            播放录音</h2>
        <span><a href="javascript:void(0)" onclick="javascript:closePopupLayerPlayRecordingSound();">
        </a></span>
    </div>
    <p id="divRecordPlugin2">
        <%--<a>WAV</a>--%>
    </p>
    <div class="bit_table">
        <table cellpadding="0" cellspacing="0" class="tableList" width="99%">
            <tr>
                <th width="20%">
                    坐席
                </th>
                <th width="15%">
                    工号
                </th>
                <th width="25%">
                    录音时间
                </th>
                <th width="20%">
                    通话时长
                </th>
                <th width="10%">
                    操作
                </th>
            </tr>
            <asp:repeater id="rptRecordUrlList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>
                           <%#Eval("TrueName")%>
                        </td>
                         <td>
                        <%#Eval("AgentNum")%>&nbsp;
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EstablishedTime").ToString())%>&nbsp;
                        </td>
                        <td>
                        <%#Eval("TallTime")%>&nbsp;
                        </td>                        
                        <td>
                           <a href='javascript:void(0);' recordurl="<%#Eval("AudioURL")%>" onclick='Play(this);' title='播放录音' ><img src='../Images/callTel.png' /></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
        </table>
    </div>
    <div class="btn" style="margin: 20px 10px 10px 0px;">       
        <input type="button" name="" value="关 闭" class="btnCannel bold" onclick="javascript:closePopupLayerPlayRecordingSound();" />
    </div>
