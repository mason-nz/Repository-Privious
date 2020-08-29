<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WOrderReturnVisit.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl.WOrderDealControl.WOrderReturnVisit" %>
<%@ Register Src="WOrderProcessList.ascx" TagName="WOrderProcessList" TagPrefix="uc1" %>
<script type="text/javascript">
    //获取回访填写的数据
    function Read() {
        var isReturnVist = $.trim($("input:checked[name='radReturnVist']").val()); //是否回访
        var txtAnswer = $.trim($("#txtAnswer").val()); //回访内容
        //校验
        if (txtAnswer.length > 1000) {
            $.jAlert("回复内容长度不能超过1000个字符！", function () { $("#txtAnswer").focus(); });
            return;
        }
        //构建
        var data = new Object();
        data.IsReturnVisit = isReturnVist; //是否回访
        data.ProcessContent = txtAnswer; //回复
        data.CallIds = $.trim($("#hid_callids").val()); //电话
        //默认值，必须设置
        data.WorkOrderStatus = "<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderStatus.Processed %>"; //已处理
        data.Recevicer = []; //接收人
        data.ExtendRecev = []; //抄送人
        data.imgData = []; //附件
        return data;
    }
    //提交
    function ReturnVisitSubmit() {
        var data = Read();
        if (data == null || data == undefined || data == "") {
            return;
        }
        //保存数据
        var pody = {
            Action: "SaveProcess",
            OrderID: "<%=OrderId %>",
            JsonData: escape(JSON.stringify(data)), //json数据
            R: Math.random()
        };
        //保存
        AjaxPost("/WOrderV2/Handler/WorderDealHandler.ashx", pody, null, function (data) {
            var jsonData = $.evalJSON(data);
            if (jsonData.success) {
                //$.jPopMsgLayer
                $.jAlert("工单" + jsonData.result + "成功，关闭当前页面！", function () { closePageExecOpenerSearch("btnsearch"); });
            }
            else {
                $.jAlert(jsonData.message, function () { });
            }
        });
    }
</script>
<div class="content" style="padding-top: 0px; padding-bottom: 0px;">
    <div class="titles bd ft14">
        处理记录</div>
    <table id="<%=TableHtmlId %>" border="0" cellspacing="0" cellpadding="0" class="xm_View_bs xm_View_hf">
        <tr>
            <th>
                <ul>
                    <li>
                        <label>
                            <span class="redColor">*</span>是否回访：</label><span class="hf_con"><label><input name="radReturnVist"
                                type="radio" value="1" class="dx" />是</label><label class="yes"><input checked="checked"
                                    name="radReturnVist" type="radio" value="0" class="dx" />否</label></span></li>
                    <li>
                        <label>
                            回复：</label>
                    <span class="hf_con" style=" margin-left:0px; padding-left:10px;">
                            <textarea id="txtAnswer" style="height:120px;" ></textarea>
                        </span></li>
                    <li class="btn"><span>
                        <input name="submit" type="button" value="提交" onclick="ReturnVisitSubmit()" /></span></li>
                </ul>
                <div class="clearfix">
                </div>
            </th>
        </tr>
        <!--处理历史记录-->
        <uc1:WOrderProcessList ID="ucWOrderProcessList" runat="server" />
    </table>
</div>
