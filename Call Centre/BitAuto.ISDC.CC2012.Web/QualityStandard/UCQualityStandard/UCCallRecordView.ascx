<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCallRecordView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard.UCCallRecordView" %>
<style type="text/css">
    #lbtnPlayRecording
    {
        height: 70px;
        margin-left: 20px;
        margin-top: 10px;
    }
</style>
<script src="/Js/jquery.media.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        PlayRecordingSound('<%=FileUrl %>');
    });

    function PlayRecordingSound(path) {
        $('#lbtnPlayRecording').attr('href', path);
        $.fn.media.mapFormat('mp3', 'winmedia');
        $('#lbtnPlayRecording').media({ width: 960, height: 70, autoplay: false });
    }
</script>
<input type="hidden" value='<%=FileUrl %>' id="HideFIleUrl" />
<div class="title" style="padding: 0px;">
    <div class="record">
        <a id="lbtnPlayRecording"></a>
    </div>
</div>
<!--录音基本信息-->
<div class="lybase">
    <div class="title">
        录音基本信息<a class="toggle2" onclick="divShowHideEvent('baseInfo',this)" href="javascript:void(0)"
            style="*margin-top: -25px;"></a> </a>
    </div>
    <table border="1" cellspacing="0" cellpadding="0" width="100%" style="margin-top: 10px;">
        <tr>
            <td width="40%" class="bdlnone">
                坐席：<span id="spUserName" runat="server"></span>
            </td>
            <td width="40%">
                录音时间：<span id="spBeginTime" runat="server"></span>
            </td>
            <td width="20%" rowspan="5" class="score" id="tdScore" runat="server" visible="false">
            </td>
        </tr>
        <tr>
            <td class="bdlnone">
                任务ID：<span id="spTaskID" runat="server"></span>
            </td>
            <td>
                通话时长：<span><span id="spTimeLong" runat="server"></span>秒</span>
            </td>
        </tr>
        <tr>
            <td class="bdlnone">
                任务分类：<span id="spSCName" runat="server"></span>
            </td>
            <td>
                录音类型：<span id="spCallType" runat="server"></span>
            </td>
        </tr>
        <tr>
            <td class="bdlnone">
                CallID：<span id="spCallID" runat="server"></span>
            </td>
            <td>
                流水号：<span id="spLiuShui" runat="server"></span>
            </td>
        </tr>
        <tr>
            <td class="bdlnone">
                满意度：<span id="spIVRScore" runat="server"></span>
            </td>
            <td>
            </td>
        </tr>
    </table>
</div>
