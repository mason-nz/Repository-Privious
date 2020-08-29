<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SavePopup.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.EditVWithCalling.SavePopup" %>

<script type="text/javascript">
    var additionalStatusHelper = (function () {
        var closePopup = function (effectiveAction, cData) {//关闭本窗口。参数：关闭前是否有[有效改变]
            $.closePopupLayer('<%= PopupName%>', effectiveAction, cData);
        },

        submit = function () {
            var additionalStatus = $('input[name="AdditionalStatus"]:checked').val();
            var G_description = additionalStatus == 'AS_G' ? $.trim($('#tf_G_description').val()) : '';
            if (G_description.length > 30) { alert('描述字数不可大于30'); return; }

            closePopup(true, { AdditionalStatus: additionalStatus, G_description: G_description });
        };

        return {
            closePopup: closePopup,
            submit: submit
        }
    })();
</script>
<script type="text/javascript">
    $(function () {
        $('input[name="AdditionalStatus"][value="<%=this.AdditionalStatus%>"]').attr('checked', true);
        $('#tf_G_description').val('<%=this.AdditionalStatusDescription%>');

        if (($('input[name="AdditionalStatus"]:checked').val() || '') == '') { $('#rb_A').attr('checked', true); }
    });
</script>
<form>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            标注附加信息</h2>
        <span><a href="javascript:void(0)" onclick="javascript:additionalStatusHelper.closePopup(false);">
        </a></span>
    </div>
    <ul class="clearfix ">
        <li class="nowidth" style="padding-top: 3px;">
            <label style="float:none;cursor:pointer" for="rb_A"><input type="radio" id="rb_A" name="AdditionalStatus" value="AS_A" runat="server" />
            <span>A 接通—信息核实</span></label> </label></li>
        <li class="nowidth" style="padding-top: 3px;">
            <label style="float:none;cursor:pointer" for="rb_B"><input type="radio" id="rb_B" name="AdditionalStatus" value="AS_B" runat="server" />
            <span>B 接通—拒绝核实</span></label> </label></li>
        <li class="nowidth" style="padding-top: 3px;">
            <label style="float:none;cursor:pointer" for="rb_C"><input type="radio" id="rb_C" name="AdditionalStatus" value="AS_C" runat="server" />
            <span>C 接通—部分核实</span> </label></li>
        <li class="nowidth" style="padding-top: 3px;">
            <label style="float:none;cursor:pointer" for="rb_D"><input type="radio" id="rb_D" name="AdditionalStatus" value="AS_D" runat="server" />
            <span>D 未接通—通话中、无人接听、无法接通</span> </label></li>
        <li class="nowidth" style="padding-top: 3px;">
            <label style="float:none;cursor:pointer" for="rb_E"><input type="radio" id="rb_E" name="AdditionalStatus" value="AS_E" runat="server" />
            <span>E 未接通—关机、停机</span> </label></li>
        <li class="nowidth" style="padding-top: 3px;">
            <label style="float:none;cursor:pointer" for="rb_F"><input type="radio" id="rb_F" name="AdditionalStatus" value="AS_F" runat="server" />
            <span>F 错误信息—公司关闭、仅作售后、汽车装饰、二手车、配件销售、重型机车（大卡/大货/大客）</span> </label></li>
        <li class="nowidth" style="padding-top: 3px;">
            <label style="float:none;cursor:pointer" for="rb_G"><input type="radio" id="rb_G" name="AdditionalStatus" value="AS_G" runat="server" />
            <span>G 其它：</span></label>
            <input id="tf_G_description" name="G_description" runat="server" style="width: 200px;" />
        </li>
        <li class="nowidth" style="padding-top: 3px;">
            <label style="float:none;cursor:pointer" for="rb_H"><input type="radio" id="rb_H" name="AdditionalStatus" value="AS_H" runat="server" />
            <span>H 未接通-电话错误、无电话、忙音、空号</span></label> </li>
        <li class="nowidth" style="padding-top: 3px;">
            <label style="float:none;cursor:pointer" for="rb_I"><input type="radio" id="rb_I" name="AdditionalStatus" value="AS_I" runat="server" />
            <span>I 查询完电话的</span></label> </li>
        <li class="nowidth" style="padding-top: 3px;">
            <label style="float:none;cursor:pointer" for="rb_J"><input type="radio" id="rb_J" name="AdditionalStatus" value="AS_J" runat="server" />
            <span>J 重复信息暂存</span></label> </li>
    </ul>
    <div class="btn" style="margin: 20px 10px 10px 0px;">
        <input type="button" value="确定" class="btnSave bold" onclick="javascript:additionalStatusHelper.submit();" />
        <input type="button" value="取消" class="btnSave bold" onclick="javascript:additionalStatusHelper.closePopup(false);" />
    </div>
</div>
</form>
