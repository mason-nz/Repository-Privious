<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlayRecord.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer.PlayRecord" %>

<script src="/Js/jquery.media.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        PlayRecordingSound('<%=RequestRecordURL %>');
    });

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
<div class="pop_new w600 openwindow">
    <div class="title bold">
        <h2>
            播放录音 <span><a href="javascript:void(0)" onclick="javascript:closePopupLayerPlayRecordingSound();">
            </a></span>
        </h2>
    </div>
    <div id="divRecordPlugin" style="font-size: 12px; margin-bottom: 15px; overflow: hidden;">
        <a>WAV</a>
    </div>
    <div class="clearfix">
    </div>
    <div class="option_button btn">
        <input type="button" name="" value="关 闭" onclick="javascript:closePopupLayerPlayRecordingSound();" />
    </div>
    <div class="clearfix">
    </div>
</div>
