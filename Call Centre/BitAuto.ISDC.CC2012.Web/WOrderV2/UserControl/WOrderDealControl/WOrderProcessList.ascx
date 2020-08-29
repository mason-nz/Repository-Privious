<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WOrderProcessList.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl.WOrderDealControl.WOrderProcessList" %>
<script type="text/javascript">
    $(function () {
        InitTableData();
    });
    //时间转换
    function timeStamp2String(time) {
        var datetime = new Date();
        time = parseInt(time.replace(/\D/igm, ""));
        datetime.setTime(time);
        var year = datetime.getFullYear();
        var month = datetime.getMonth() + 1 < 10 ? "0" + (datetime.getMonth() + 1) : datetime.getMonth() + 1;
        var date = datetime.getDate() < 10 ? "0" + datetime.getDate() : datetime.getDate();
        var hour = datetime.getHours() < 10 ? "0" + datetime.getHours() : datetime.getHours();
        var minute = datetime.getMinutes() < 10 ? "0" + datetime.getMinutes() : datetime.getMinutes();
        var second = datetime.getSeconds() < 10 ? "0" + datetime.getSeconds() : datetime.getSeconds();
        return year + "-" + month + "-" + date + " " + hour + ":" + minute + ":" + second;
    }
    //查询工单处理记录
    function InitTableData() {
        var url = '/WOrderV2/Handler/WorderDealHandler.ashx';
        var tableID = '<%=TableHtmlId %>';
        var orderID = '<%=OrderId %>';
        AjaxPostAsync(url, {
            Action: "GetWOrderProcessByOrderID",
            OrderID: orderID,
            r: Math.random()
        }, null, function (returnValue) {
            var retValue = $.evalJSON(returnValue);
            var tabData = $.evalJSON(retValue.message);
            var tabHtml = '';
            for (var key in tabData) {
                //开始循环
                tabHtml += '<tr>';
                tabHtml += '<th colspan="4" class="gongdanjilu">';
                tabHtml += '<ul>';
                tabHtml += '<li>' + tabData[key].CreateUserName + '【' + tabData[key].CreateUserDeptName + '】</li>';
                tabHtml += '<li class="status"><span>' + tabData[key].StatusStr + '</span></li>';
                tabHtml += '<li class="time"><span>' + timeStamp2String(tabData[key].CreateTime) + '</span></li>';
                tabHtml += '<li class="attach w800"><pre>' + tabData[key].ProcessContent + "</pre>";
                tabHtml += '<span class="imgData"  data=' + tabData[key].RecID + '></span></li>';
                tabHtml += '</ul>';
                tabHtml += '</th>';
                tabHtml += '</tr>';
                //结束循环
            }
            $("#" + tableID).append(tabHtml);
            InitExtendData();
        });
    }
    //初始化《附件》和《录音》
    function InitExtendData() {
        var Tableid = "<%=TableHtmlId %>";
        // 绑定附件图片 begin
        AjaxPostAsync('/WOrderV2/Handler/WorderDealHandler.ashx', {
            Action: "GetAttachmentProcessListByOrderID",
            OrderID: '<%=OrderId%>',
            StoragePathType: '<%=(int)BitAuto.ISDC.CC2012.BLL.Util.ProjectTypePath.WorkOrder%>',
            r: Math.random()
        }, null, function (returnValue) {
            var retValue = $.evalJSON(returnValue);
            var retMsg = $.evalJSON(retValue.message);
            var spanList = $("#" + Tableid + " .gongdanjilu  span[data]");
            for (var i = 0; i < spanList.length; i++) {
                for (var j = 0; j < retMsg.length; j++) {
                    if ($(spanList[i]).attr("data") == retMsg[j].RelatedID) {
                        $(spanList[i]).append(' <img src="' + retMsg[j].SmallFilePath + '" alt="' + retMsg[j].FileName + '" data="' + retMsg[j].FilePath + '" height="90" title="' + retMsg[j].FileName + '" width="130" />');
                    }
                }
                BindSlider($("#" + Tableid + " .gongdanjilu span[data='" + $(spanList[i]).attr("data") + "']").children("img"), "data");
            }
        });
        // 绑定附件图片 end

        // 绑定收件人 begin
        AjaxPostAsync('/WOrderV2/Handler/WorderDealHandler.ashx', {
            Action: "GetReceiverPeopleByOrderID",
            OrderID: '<%=OrderId%>',
            r: Math.random()
        }, null, function (returnValue) {
            var retValue = $.evalJSON(returnValue);
            var retMsg = $.evalJSON(retValue.message);
            var spanList = $("#" + Tableid + " .gongdanjilu  span[data]");

            for (var i = 0; i < spanList.length; i++) {
                var recever = "->";
                for (var j = 0; j < retMsg.length; j++) {                  
                    if ($(spanList[i]).attr("data") == retMsg[j].ReceiverID) {
                        recever += retMsg[j].UserName + "，";                     
                    }
                }
                    if (recever != "->") {
                        recever = recever.substring(0, recever.length - 1);                     
                        $(spanList[i]).parent().siblings().eq(0).append(recever);
                    }

             
            }
        });
        // 绑定收件人 end

        // 绑定处理话务 begin
        AjaxPostAsync('/WOrderV2/Handler/WorderDealHandler.ashx', {
            Action: "GetCallReportByOrderID",
            OrderID: '<%=OrderId%>',
            r: Math.random()
        }, null, function (returnValue) {
            var retValue = $.evalJSON(returnValue);
            var retMsg = $.evalJSON(retValue.message);
            var spanList = $("#" + Tableid + " .gongdanjilu  span[data]");
            for (var i = 0; i < spanList.length; i++) {
                for (var j = 0; j < retMsg.length; j++) {
                    if ($(spanList[i]).attr("data") == retMsg[j].ReceiverID) {
                        $(spanList[i]).before(' <a href="javascript:void(0);" onclick="ADTTool.PlayRecord(&quot;' + retMsg[j].AudioURL + '&quot; , &quot;/WOrderV2/PopLayer/PlayRecord.aspx&quot;);"  title="' + retMsg[j].DataID + '"><img src="/Images/callTel.png" alt="' + retMsg[j].DataID + '"/></a>');
                    }
                }
            }
        });
        // 绑定处理话务 end
    }
</script>
<!--工单处理记录-->
