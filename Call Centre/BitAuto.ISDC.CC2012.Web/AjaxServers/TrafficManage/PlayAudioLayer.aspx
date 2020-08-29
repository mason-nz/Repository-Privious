<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlayAudioLayer.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TrafficManage.PlayAudioLayer" %>

<script language="javascript" type="text/javascript" src="/js/jquery.media.js"></script>
<script type="text/javascript">
    PlayRecordingSound();
    function PlayRecordingSound() {
        $.fn.media.mapFormat('wav', 'winmedia');
        $.fn.media.mapFormat('mp3', 'winmedia');
        $('a.media').media({
            width: 400,
            height: 65,
            autoplay: true
        });
    }
</script>
<div class="pop pb15 openwindow" style="width: 500px;">
    <div class="title bold">
        <h2>
            播放录音</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('PlayAudioAjaxPopup');">
        </a></span>
    </div>
    <fieldset>
        <a class="media" href='<%=RequestAudioURL %>' id="lbtnAudio">
            <%=RequestAudioURL %></a>
    </fieldset>
    <div class="btn">
        <input type="button" class="button" value="取消" onclick="javascript:$.closePopupLayer('PlayAudioAjaxPopup',false);" />
    </div>
</div>
