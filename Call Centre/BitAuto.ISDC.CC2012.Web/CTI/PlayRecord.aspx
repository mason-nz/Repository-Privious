<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlayRecord.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CTI.PlayRecord" %>

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
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            播放录音</h2>
        <span><a href="javascript:void(0)" onclick="javascript:closePopupLayerPlayRecordingSound();">
        </a></span>
    </div>
    <div id="divRecordPlugin">
        <a>WAV</a>
    </div>
    <div class="btn" style="margin: 20px 10px 10px 0px;">
        <!--<input type="button" name="" value="提 交" onclick="javascript:SaveSelectBrand();"
            class="btnSave bold" />&nbsp;&nbsp;-->
        <input type="button" name="" value="关 闭" class="btnCannel bold" onclick="javascript:closePopupLayerPlayRecordingSound();" />
    </div>
