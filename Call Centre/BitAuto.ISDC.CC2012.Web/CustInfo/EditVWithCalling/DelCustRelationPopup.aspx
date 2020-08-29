<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DelCustRelationPopup.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling.DelCustRelationPopup" %>

<script type="text/javascript">
    var delCustRelationInfoHelper = (function () {
        var closePopup = function (effectiveAction) {//关闭本窗口。参数：关闭前是否有[有效改变]
            $.closePopupLayer('<%= PopupName%>', effectiveAction);
        },

        submit = function () {
            var descriptionWhenDelete = $.trim($('#tfDescriptionWhenDelete').val());

            if (descriptionWhenDelete == '') {
                $.jAlert('请填写删除理由！');
                return;
            }

            $.post('/CustInfo/Handler.ashx?callback=?', {
                Action: 'AddDeleteCustRelationInfo',
                TID: '<%= this.TID %>',
                CustID: '<%= this.CustID %>',
                DelRelationCustIDs: '<%= this.DelRelationCustIDs %>',
                DescriptionWhenDelete: encodeURIComponent(descriptionWhenDelete)
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
<div class="pop pb15 openwindow" style="width:">
    <div class="title bold">
        <h2>
            填写删除客户的3个问题</h2>
        <span><a href="javascript:void(0)" onclick="javascript:delCustRelationInfoHelper.closePopup(false);">
        </a></span>
    </div>
    <ul class="clearfix ">
        <li class="nowidth">
            <label>
                3个问题：</label>
            <textarea id="tfDescriptionWhenDelete" name="DescriptionWhenDelete" rows="5" class="k400"
                runat="server"></textarea>
        </li>
    </ul>
    <div class="btn" style="margin: 20px 10px 10px 0px;">
        <input type="button" value="确定" class="btnSave bold" onclick="javascript:delCustRelationInfoHelper.submit();" />
        <input type="button" value="取消" class="btnSave bold" onclick="javascript:delCustRelationInfoHelper.closePopup(false);" />
    </div>
</div>
</form>
