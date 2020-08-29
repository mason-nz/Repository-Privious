<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlayRecordListAjax.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CTI.PlayRecordAjax" %>

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

//    function closePopupLayerPlayRecordingSound() {
//        try {
//            $('#divRecordPlugin').find('object')[0].controls.stop(); //停止播放
//            $.closePopupLayer('PlayRecordLayer', false);
//        } catch (e) {
//            $.closePopupLayer('PlayRecordLayer', false);
//        }
//    }
</script>
<div id="divRecordPlugin"><a>WAV</a></div>
    
