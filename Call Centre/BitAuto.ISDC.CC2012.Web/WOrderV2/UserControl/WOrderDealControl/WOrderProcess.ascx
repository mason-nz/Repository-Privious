<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WOrderProcess.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl.WOrderDealControl.WOrderProcess" %>
<%@ Register Src="WOrderProcessList.ascx" TagName="WOrderProcessList" TagPrefix="uc1" %>
<%--上传附件专用--%>
<link href="/Css/workorder/wo-uploadify.css" rel="stylesheet" type="text/css" />
<script src="/Js/wo-jquery.uploadify.js" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        //是否隐藏或显示接收人和抄送人
        ShowReceviceIMG();
        //接收人
        var contr1IMG = new ReceiverCopyUserSelectControl();
        contr1IMG.InitialEvent("txtRecevicerIMG", 4);
        //抄送人
        var contr2IMG = new ReceiverCopyUserSelectControl();
        contr2IMG.InitialEvent("txtExtendRecevIMG");
        //上传控件初始化
        UploadifyControl.Init("uploadDiv", "上传附件", "uploadpos", "<%=(int)BitAuto.ISDC.CC2012.BLL.Util.ProjectTypePath.WorkOrder %>");
    });

    //是否隐藏或显示接收人和抄送人
    function ShowReceviceIMG() {
        var PendingStatusImg = '<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderStatus.Pending %>'; //待审核
        var UntreatedStatusImg = '<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderStatus.Untreated %>'; //待处理
        var processingStatusImg = '<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderStatus.Processing %>'; //处理中
        var ProcessedStatusImg = '<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderStatus.Processed %>'; //已处理
        var CompletedStatusImg = '<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderStatus.Completed %>'; //已完成
        var ClosedStatusImg = '<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderStatus.Closed %>'; //已关闭

        var status = $("#selWorkOrderStatusIMG").val();
        if (status == ProcessedStatusImg || status == ClosedStatusImg) {
            //工单状态修改为“已处理”和“已关闭”时，隐藏接收人和抄送人
            $("#receviceLIIMG,#extendReceLIIMG").hide();
        }
        else {
            $("#receviceLIIMG,#extendReceLIIMG").show();
        }
    }

    //获取处理用户填写的数据
    function Read() {
        var PendingStatusImg = '<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderStatus.Pending %>'; //待审核
        var UntreatedStatusImg = '<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderStatus.Untreated %>'; //待处理
        var processingStatusImg = '<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderStatus.Processing %>'; //处理中
        var ProcessedStatusImg = '<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderStatus.Processed %>'; //已处理
        var CompletedStatusImg = '<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderStatus.Completed %>'; //已完成
        var ClosedStatusImg = '<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderStatus.Closed %>'; //已关闭

        var OrderStatus = $.trim($("#selWorkOrderStatusIMG").val()); //工单状态
        var txtAnswer = $.trim($("#txtRetrunAnswerIMG").val()); //回复记录
        var txtRecevicer = $.trim($("#txtRecevicerIMG").attr("userids")); //接收人
        var txtExtendRecev = $.trim($("#txtExtendRecevIMG").attr("userids")); //抄送人
        //校验
        if (txtAnswer.length > 1000) {
            $.jAlert("回复内容长度不能超过1000个字符！", function () { $("#txtRetrunAnswerIMG").focus(); });
            return;
        }
        if (!(OrderStatus == ProcessedStatusImg || OrderStatus == ClosedStatusImg)) {
            if (txtRecevicer == "") {
                $.jAlert("接收人不能为空！", function () { $("#txtRecevicerIMG").focus(); });
                return;
            }
        }
        //构建
        var data = new Object();
        var recevicer = $.evalJSON($.trim($("#txtRecevicerIMG").attr("useridusercodejson"))) || [];
        var extendRecev = $.evalJSON($.trim($("#txtExtendRecevIMG").attr("useridusercodejson"))) || [];

        data.WorkOrderStatus = $.trim(OrderStatus); //状态
        data.ProcessContent = $.trim(txtAnswer); //回复
        data.Recevicer = (OrderStatus == ProcessedStatusImg || OrderStatus == ClosedStatusImg) ? [] : recevicer; //接收人
        data.ExtendRecev = (OrderStatus == ProcessedStatusImg || OrderStatus == ClosedStatusImg) ? [] : extendRecev; //抄送人
        data.imgData = UploadifyControl.GetUploadifyArr(); //附件
        data.CallIds = $.trim($("#hid_callids").val()); //电话
        //默认值，必须设置
        data.IsReturnVisit = -1;
        return data;
    }

    //提交
    function ProcessSubmit() {
        var data = Read();
        if (data == null || data == undefined || data == "") {
            return;
        }
        //保存数据
        var pody = {
            Action: "SaveProcess",
            OrderID: "<%=OrderId %>",
            RightData: "<%=Right%>",
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
    <!--处理记录-->
    <div class="titles bd ft14">
        处理记录</div>
    <table id="<%=TableHtmlId %>" border="0" cellspacing="0" cellpadding="0" class="xm_View_bs xm_View_sh">
        <tr>
            <th>
                <ul class="sh_con">
                    <li>
                        <label>
                            <span class="redColor">*</span>工单状态：</label><span class="hf_con">
                                <select id="selWorkOrderStatusIMG" name="" class="w265" onchange="ShowReceviceIMG()">
                                    <%
                                        foreach (int key in DictStauts.Keys)
                                        {%>
                                    <option value='<%=key%>'>
                                        <%=DictStauts[key]%>
                                    </option>
                                    <%} %>
                                </select></span></li>
                    <li id="receviceLIIMG">
                        <label>
                            <span class="redColor">*</span>接收人：</label><span class="hf_con"><input type="text"
                                id="txtRecevicerIMG" value="" class="w265" readonly="readonly" /></span></li>
                    <li id="extendReceLIIMG">
                        <label>
                            抄送人：</label><span class="hf_con"><input id="txtExtendRecevIMG" type="text" value=""
                                class="w265" readonly="readonly" /></span></li>
                    <li style="clear: both; width:900px">
                        <label>
                            回复：</label>
                        <span class="hf_con" style=" margin-left:0px; padding-left:10px;">
                            <textarea id="txtRetrunAnswerIMG" style="height:120px; " ></textarea></span> <span class="add_attach" style="padding-top: 20px"/>
                                <span id="uploadDiv"></span>
                                <span id="div_FileContent" class="uploadify-filecontent">
                                </span></span></li>
                    <li class="btn" style="clear: both;">
                        <input name="submit" type="button" value="提交" onclick="ProcessSubmit()" /></li>
                </ul>
                <div class="clearfix">
                </div>
            </th>
        </tr>
        <!--处理历史记录-->
        <uc1:WOrderProcessList ID="ucWOrderProcessList" runat="server" />
    </table>
    <div class="clearfix">
    </div>
    <!--处理记录-->
</div>
