<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StopPopup.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling.StopPopup" %>

<script type="text/javascript">
    var stopCheckInfoHelper = (function () {
        var closePopup = function (effectiveAction) {//关闭本窗口。参数：关闭前是否有[有效改变]
            $.closePopupLayer('<%= PopupName%>', effectiveAction);
        },

        submit = function () {
            var descriptionWhenStop = $.trim($('#tfDescriptionWhenStop').val());

            if (descriptionWhenStop == '') {
                $.jAlert('请填写删除理由！');
                return;
            }

            $.post('/CustInfo/Handler.ashx?callback=?', {
                Action: 'stopcheckinfo',
                TID: '<%= this.TID %>',
                DescriptionWhenStop: encodeURIComponent(descriptionWhenStop)
            }, function (jd, textStatus, xhr) {
                if (textStatus != 'success') { $.jAlert('请求错误'); }
                else if (jd.success) {
                    closePopup(true);
                }
                else {
                    $.jAlert('错误: ' + jd.message);
                }
            }, 'json');
        };

        return {
            closePopup: closePopup,
            submit: submit
        }
    })();
</script>
<form>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            填写停用客户信息理由</h2>
        <span><a href="javascript:void(0)" onclick="javascript:stopCheckInfoHelper.closePopup(false);">
        </a></span>
    </div>
    <ul class="clearfix ">
        <li class="nowidth">
            <label>
                停用理由：</label>
            <textarea id="tfDescriptionWhenStop" name="DescriptionWhenStop" rows="5" class="k400"
                runat="server"></textarea>
        </li>
    </ul>
    <div class="btn" style="margin: 20px 10px 10px 0px;">
        <input type="button" value="确定" class="btnSave bold" onclick="javascript:stopCheckInfoHelper.submit();" />
        <input type="button" value="取消" class="btnSave bold" onclick="javascript:stopCheckInfoHelper.closePopup(false);" />
    </div>
</div>
</form>
