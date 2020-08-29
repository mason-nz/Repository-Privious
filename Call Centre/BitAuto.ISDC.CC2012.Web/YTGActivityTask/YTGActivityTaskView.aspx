<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="YTGActivityTaskView.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.YTGActivityTask.YTGActivityTaskView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">
    <title>邀约处理页</title>
    <link href="../css/base.css" type="text/css" rel="stylesheet" />
    <link href="../css/style.css" type="text/css" rel="stylesheet" />
    <link href="../css/CTIPopup.css" type="text/css" rel="stylesheet" />
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" charset="utf-8" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="../Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="/Js/controlParams.js" type="text/javascript"></script>
     <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <link href="../../Css/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
    <script src="../../Js/jquery.autocomplete.min.js" type="text/javascript"></script>
 
    <script type="text/javascript">
        $(function () {
            var isSuccess = "<%=isSuccess%>";
 
          
            //如果失败，显示失败原因
            if (isSuccess =="0") {
                $("#li_fail").css("display", "");
            }
            else {
                $("#li_fail").css("display", "none");
            }
            //加载电话历史记录
            //  GetCallRecordORGIHistory();
            //绑定任务处理历史
            BindHistory();
            //绑定话务记录
            BindCallReocrd();
 
        });
 

        function OpenDemandID(url) {
            url += "&R=" + Math.random();
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + escape(url));
            }
            catch (e) {
                window.open(url);
            }
        }
      
        //隐藏/显示div容器
        function divShowHideEvent(divId, obj) {
            if ($(obj).attr("class") == "toggle") {
                $("#" + divId).show("slow");
                $(obj).attr("class", "toggle hide");
            }
            else {
                $("#" + divId).hide("slow");
                $(obj).attr("class", "toggle");
            }
        }
        //绑定话务记录
        function BindCallReocrd() {
            LoadingAnimation("divCallRecordList");
            $("#divCallRecordList").load("../YTGActivityTask/AjaxServers/YTGActivityTaskCallRecordList.aspx", { TaskID: '<%=TaskID %>', r: Math.random() });
        }
        //绑定历史记录
        function BindHistory() {
            LoadingAnimation("divTaskLog");
            $("#divTaskLog").load("../YTGActivityTask/AjaxServers/YTGActivityTaskOperationLog.aspx", { TaskID: '<%=TaskID %>', r: Math.random() });
        }

        function ViewActivityInfo() {
            $.openPopupLayer({
                name: "ViewActivityInfoAjaxPopup",
                parameters: {},
                url: "/YTGActivityTask/AjaxServers/YTGActivityTaskActivityInfo.aspx?ActivityID=" + <%=model.ActivityID%> + "&r=" + Math.random()
            });

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
 
    <div class="w980">
        <div class="taskT">
            邀约处理<span></span>
        </div>
        <div class="baseInfo">
            <div class="title contact" style="clear: both;">
                基本信息<a href="javascript:void(0)" onclick="divShowHideEvent('divContactRecord',this)"
                    class="toggle hide"></a>
            </div>
            <div id="divContactRecord">
                <ul class="clearfix ">
                    <li>
                        <label>
                            项目名称：</label>
                        <span id="spanProjectName">
                            <%=GetProjectName()%></span> </li>
                    <li>
                        <label>
                            任务ID：</label>
                        <span id="spanTaskID">
                            <%=TaskID%></span> </li>
                    <li>
                        <label>
                            <span class="redColor">*</span>姓名：</label>
                        <span id="spanUserName">
                             <%=model.UserName%> 
                        </span></li>
                    <li>
                        <label>
                            <span class="redColor">*</span>性别：</label>
                        <span id="spanSex"  runat="server"></span>
                    </li>
                    <li>
                        <label>
                            电话：</label>
                        <span id="spanTel">
                            <%=model.Tel%>
                        </span>
                    </li>
                    <li>
                        <label>
                            下单地区：</label>
                        <span id="spanXiDanArea"  runat="server">
                             
                        </span></li>
                    <li>
                        <label>
                            关联活动主题：</label>
                        <span>
                            <a href="javascript:void(0)" onclick="javascript:ViewActivityInfo()" >
                                <%=GetActivityName()%>
                            </a> 
                        </span></li>
                    <li>
                        <label>
                            下单日期：</label>
                        <span id="spanXiDanDate"  runat="server">
                             
                        </span></li>
                    <li>
                        <label>
                            下单车型：</label>
                        <span>
                         <%=GetOrderCarSerialName() %></span> </li>
                    <li>
                        <label>
                            <span class="redColor">*</span>试驾地区：</label>
                        <span id="spanShiJiaArea"  runat="server">
                             
                        </span></li>
                    <li>
                        <label>
                            <span class="redColor" id="spCar">*</span>意向车型：</label>
                        <span id="spanYiXiangCheXing"  runat="server">
 
                        </span></li>
                    <li>
                        <label>
                            <span class="redColor" id="Span4">*</span>预计购车时间：</label>
                        <span id="spanPredictBuyTime"  runat="server"> 
          
                        </span></li>
                    <li>
                        <label>
                            <span class="redColor">*</span>是否成功：</label>
                        <span id="spanIsSuccess"  runat="server"></span>
                    </li>
                    <li>
                        <label>
                            <span class="redColor">*</span>是否接通：</label>
                        <span id="spanIsConnected"  runat="server"></span>
                    </li>
                    <li id="li_fail" style="display: none">
                        <label>
                            <span class="redColor">*</span>失败原因：
                        </label>
                        <span id="spanFailReason"  runat="server"></span>
                    </li>
                    <li class="gdjl">
                        <label>
                            备注：</label>
                        <span id="spanRemark" runat="server">
                            
                        </span></li>
                </ul>
            </div>
            <div class="cont_cx khxx CustInfoArea">
                <div class="title contact" style="clear: both;">
                    记录历史<a class="toggle hide" onclick="divShowHideEvent('infoBlock1',this)" href="javascript:void(0)"></a>
                </div>
                <div id="infoBlock1">
                    <ul class="infoBlock firstPart">
                        <li style="width: 900px; height: auto;">
                            <div style="">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;操作记录</div>
                            <div id="divTaskLog" class="fullRow cont_cxjg" style="margin-left: 78px;">
                            </div>
                        </li>
                        <li style="width: 900px; height: auto;">
                            <div style="">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;通话记录</div>
                            <div id="divCallRecordList" class="fullRow cont_cxjg" style="margin-left: 78px;">
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <br />
 
        </div>
    </div>
    </form>
</body>
</html>
