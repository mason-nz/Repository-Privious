<%@ Page Title="对话监控" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="ConversationMonitor.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.QueueManage.ConversationMonitor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Scripts/Enum/Area.js" type="text/javascript"></script>
    <script src="../Scripts/Enum/Area2.js" type="text/javascript"></script>
    <script type="text/javascript">
        var _interval;
        var _csid;
        var _searchstarttime;
        var _membername;
        var islock;
        function BindBeginEndtime() {
            $("#txtStartTime").val('<%=DateTime.Now.ToString("yyyy-MM-dd 00:00:00") %>');
            $("#txtEndTime").val('<%=DateTime.Now.ToString("yyyy-MM-dd 23:59:59") %>');
        }
        $(function () {
            //BindSelDistrictData();
            //            $('#txtStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); }, dateFmt: 'yyyy-MM-dd HH:mm:ss'}); });
            //            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}', dateFmt: 'yyyy-MM-dd HH:mm:ss' }); });
            $('#txtStartTime').bind('click focus', function () { WdatePicker({ isShowClear: true, readOnly: false, startDate: '%y-%M-%d 00:00:00', dateFmt: 'yyyy-MM-dd HH:mm:ss', maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ isShowClear: true, readOnly: false, startDate: '%y-%M-%d 23:59:59', dateFmt: 'yyyy-MM-dd HH:mm:ss', minDate: '#F{$dp.$D(\'txtStartTime\')}' }); });

            if ($("#txtStartTime").val() == "" && $("#txtEndTime").val() == "") {
                BindBeginEndtime();
            }
            BindProvince('ddlSearchVisitorProvince'); //绑定会员省份

            $("[id$=ddlSearchVisitorProvince]").change(function () {
                BindCity('ddlSearchVisitorProvince', 'ddlSearchVisitorCity');
            });
            search();
            setInterval("search()", 1000 * 20);
        });
        //查询
        function search() {
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg, function () {
                    return false;
                });
            }
            else {
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);

                LoadingAnimation("ajaxChatingList");
                $('#ajaxChatingList').load("/AjaxServers/QueueManage/MonitorData.aspx", podyStr, function () {
                    var $tabs = $("#Tab1 .Contentbox >div");
                    var $hidcsidinput = $("#ajaxChatingList .hidcsidinput");
                    $tabs.each(function () {
                        var csid = $(this).attr("name").substring(0, $(this).attr("name").indexOf(","));
                        var hasval = false;
                        if (csid != "") {
                            $hidcsidinput.each(function () {
                                if (csid == $(this).val()) {
                                    hasval = true;
                                }
                            });
                        }
                        if (!hasval) {
                            var classname = $.trim($(this).attr("class"));
                            switch (classname) {
                                case "online h50": $(this).removeClass("online h50").addClass("offline").css("background-color", "#fcf2ca");
                                    clearSetInterval();
                                    break;
                                case "queue": $(this).removeClass("queue").addClass("offline");
                                    break;
                                default: break;
                            }
                        }
                    });
                });
            }
        }
        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";
            var txtVisitorName = $.trim($("#txtVisitorName").val());
            if (txtVisitorName.length > 50) {
                msg += "访客姓名字数太多<br/>";
                $("#txtVisitorName").val('');
            }
            var txtAgentName = $.trim($("#txtAgentName").val());
            if (txtAgentName.length > 50) {
                msg += "客服名称字数太多<br/>";
                $("#txtAgentName").val('');
            }
            var beginTime = $.trim($("#txtStartTime").val());
            var endTime = $.trim($("#txtEndTime").val());
            if (beginTime != "") {
                if (!beginTime.isDateTime()) {
                    msg += "最后对话时间(起始)格式不正确<br/>";
                    $("#txtStartTime").val('');
                }
            }

            if (endTime != "") {
                if (!endTime.isDateTime()) {
                    msg += "最后对话时间(终止)格式不正确<br/>";
                    $("#txtEndTime").val('');
                }
            }

            if (beginTime != "" && endTime != "") {
                if (beginTime > endTime) {
                    msg += "最后对话时间中结束时间不能大于开始时间<br/>";
                    $("#txtStartTime").val('');
                    $("#txtEndTime").val('');
                }
            }
            return msg;
        }

        //获取参数
        function _params() {
            var txtVisitorName = encodeURIComponent($.trim($('#txtVisitorName').val()));
            var selProvinceID = encodeURIComponent($.trim($('#ddlSearchVisitorProvince').val()));
            var selCityID = encodeURIComponent($.trim($('#ddlSearchVisitorCity').val()));
            var selSourceType = encodeURIComponent($.trim($('#<%=selSourceType.ClientID%>').val()));

            var txtStartTime = encodeURIComponent($.trim($('#txtStartTime').val()));
            var txtEndTime = encodeURIComponent($.trim($('#txtEndTime').val()));
            var txtAgentName = encodeURIComponent($.trim($('#txtAgentName').val()));
            var selBusinessGroup = encodeURIComponent($.trim($('#<%=selBusinessGroup.ClientID%>').val()));

            var pody = {
                VisitorName: txtVisitorName,
                ProvinceID: selProvinceID,
                CityID: selCityID,
                SourceTypeValue: selSourceType,

                QueryStarttime: txtStartTime,
                QueryEndTime: txtEndTime,
                UserName: txtAgentName,
                BusinessGroup: selBusinessGroup,
                r: Math.random()  //随机数
            }

            return pody;
        }
        //获取大区数据
        function BindSelDistrictData() {
            $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'getdistrictdata', r: Math.random() }, function (data) {
                $("#selDistrict").html("");
                $("#selDistrict").append("<option value='-1'>请选择</option>");
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    if (jsonData != "") {
                        $.each(jsonData.root, function (idx, item) {
                            $("#selDistrict").append("<option value='" + item.value + "'>" + item.name + "</option>");
                        });

                        $("#ajaxTable tr:eq(0)").click();
                    }
                }
            });
        }
        //根据大区获取城市群数据
        function BindSelectChange() {
            //            var pid = encodeURIComponent($("#selDistrict").val());
            //            $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'getcitydata', District: encodeURIComponent(pid), r: Math.random() }, function (data) {
            //                $("#selCityGroup").html("");
            //                $("#selCityGroup").append("<option value='-1'>请选择</option>");
            //                if (data != "") {
            //                    var jsonData = $.evalJSON(data);
            //                    if (jsonData != "") {
            //                        $.each(jsonData.root, function (idx, item) {
            //                            $("#selCityGroup").append("<option value='" + item.value + "'>" + item.name + "</option>");
            //                        });
            //                    }
            //                }
            //            });
        }
    </script>
    <script type="text/javascript">

        //        var _interval;
        //        var _csid;
        //        var _searchstarttime;
        //        var _membername;

        //开始监控某个会话
        function startMonitor(csid, searchtime, membername) {
            var hasMonitored = 0;
            $("#Tab1 .Contentbox >div").each(function () {
                if ($(this).attr("name").indexOf(csid + ",") != -1) {
                    hasMonitored = 1;     //说明已经对该会话添加监控
                }
            });
            //此对话没有被监控
            if (hasMonitored == 0) {
                //3:关闭已开启的定时器
                clearSetInterval();

                //注意：此处要更新正在监控的tab的那么属性
                var $nowonlinediv;
                //1:修改正在监控选项卡的属性      
                $("#Tab1 .Contentbox >div").each(function () {
                    if ($(this).hasClass("online h50")) {
                        $nowonlinediv = $(this); //保留当前监控tab对象
                        $(this).removeClass("online h50").addClass("queue");
                    }
                    $(this).css("background-color", "#FFFFFF");
                });
                if ($nowonlinediv != undefined) {
                    //2.更新当前监控会话tab的name属性信息
                    $nowonlinediv.attr("name", (_csid + "," + _searchstarttime));
                }

                //3.初始化新的监控会话的数据查询参数
                _csid = csid;
                _membername = membername;
                _searchstarttime = '1900-01-01 01:01:01';
                //4.添加新的监控会话的tab
                var str1 = "<div name=\"" + csid + "," + _searchstarttime + "\" class=\"online h50\"><i></i><h4>" + membername + "</h4><span class=\"close\"><a href=\"#\" >关闭</a></span></div>";
                $("#Tab1 .Contentbox").prepend(str1);
                //4:隐藏已有的监控内容显示div，添加要监控内容显示div
                //messagedivcontenor_contenor
                $("#messagedivcontenor_contenor >div").each(function () {
                    $(this).hide();
                });
                $("#messagedivcontenor_contenor").append("<div class=\"scroll_gd\" id=\"messagedivcontenor" + csid + "\" style=\" margin:0px; height:450px;\"></div><div id=\"finishdivcontenner" + csid + "\" class=\"scroll_gd\" style=\"margin: 0px; height: 37px; width: 100%; float: left;text-align: right; overflow: hidden;border-top:1px solid #00769c;\"><span class=\"right btn\" style=\"padding: 6px 0px; width:100%;\"><label class=\"lblastinfotime\" style=\" float:left; line-height:25px;margin-left:20px;\">最后消息时间：</label><input class=\"w60 gray\" style='margin-right:20px' type=\"button\" value=\"结束\" onclick=\"finishMonitor(" + csid + ")\" /></span></div>");

                //4:开启定时器
                setIntervalUp();
            }
            //此对话已被监控
            else if (hasMonitored == 1) {
                //判断此会话是不是当前监控的会话
                //1.不是当前监控会话
                if (!$("#Tab1 .Contentbox >div[name^='" + csid + ",']").hasClass("online h50")) {
                    //1.将该会话的tab设置为当前监控状态,其他处于 online状态的tab设置为 queue状态
                    var $nowonlinediv;
                    $("#Tab1 .Contentbox >div").each(function () {
                        if ($(this).hasClass("online h50")) {
                            $nowonlinediv = $(this); //保留当前监控tab对象
                            $(this).removeClass("online h50").addClass("queue");
                        }
                        $(this).css("background-color", "#FFFFFF");
                    });
                    $("#Tab1 .Contentbox >div[name^='" + csid + ",']").removeClass("queue").addClass("online h50");

                    //2.将该会话的内容显示div显示出来，其他会话的内容显示div隐藏掉
                    $("#messagedivcontenor_contenor >div").each(function () {
                        $(this).hide();
                    });
                    $("#messagedivcontenor" + csid).show(); //.find(" div").css("display", "block"); ;
                    $("#finishdivcontenner" + csid).show();
                    if ($nowonlinediv != undefined) {
                        //.更新当前监控会话tab的name属性信息
                        $nowonlinediv.attr("name", (_csid + "," + _searchstarttime));
                    }
                    //3.更新查询数据的参数
                    _csid = csid;
                    var divname = $("#Tab1 .Contentbox >div[name^='" + csid + ",']").attr("name");
                    _searchstarttime = divname.substr(divname.indexOf(",") + 1);
                }
                //2.是当前监控对话——那就神马都不用做了
            }

        }

        function setIntervalUp() {
            islock = false;
            _interval = setInterval("ajaxGetMessage()", 1000 * 2);
        }
        function clearSetInterval() {
            clearInterval(_interval);
            islock = true;
        }

        function ajaxGetMessage() {
            if (!islock) {
                islock = true;
                $.getJSON("/AjaxServers/LayerDataHandler.ashx", { Action: 'getnewmessagesdata', CSID: _csid, QueryStarttime: _searchstarttime, r: Math.random() }, function (json, textStatus) {/*参数json指test.json文件的内容*/
                    if (json != null) {
                        $.each(json, function (index, val) {/*用jquery $.each()遍历输出json文件内容*/
                            _searchstarttime = val.lastdate;
                            $("#messagedivcontenor" + _csid).append("<div class=\"dh1\"><div class=\"title\">"
                                + val.newName
                                + "</div><div class=\"dhc\">"
                                + val.Content
                                + "</div></div>");
                            $("#finishdivcontenner" + _csid + " .lblastinfotime").text("最后消息时间：" + val.lastdate.substr(11, 8));
                        });
                    }
                    islock = false;
                });
            }

        }

        function getDataTimeNow() {
            var nowstr = new Date();
            var datenow = nowstr.getFullYear() + "-"
                     + ((nowstr.getMonth() + 1) < 10 ? "0" : "") + (nowstr.getMonth() + 1) + "-"
                     + (nowstr.getDate() < 10 ? "0" : "") + nowstr.getDate()
                     + " "
                     + (nowstr.getHours() < 10 ? "0" : "") + nowstr.getHours() + ":"
                     + (nowstr.getMinutes() < 10 ? "0" : "") + nowstr.getMinutes() + ":"
                     + (nowstr.getSeconds() < 10 ? "0" : "") + nowstr.getSeconds();
            return datenow;

        }
        $(function () {
            $("#Tab1 .Contentbox >div").live("click", function () {
                setOnline(this);
            });

            $("#Tab1 .close").live("click", function (event) {
                droptab(this);
                event.preventDefault();
                event.stopPropagation();
                return false;
            });
        })
        function finishMonitor(csid) {
            $("#Tab1 .Contentbox >div[name^='" + csid + ",'] .close").click();
        }
        //切换tab选项卡事件
        function setOnline(obj) {
            //1.获取对象的class属性已判断其状态
            var objClassName = $.trim($(obj).attr("class"));
            switch (objClassName) {
                case "online h50": break;
                case "queue":
                    clearSetInterval();
                    //1.将当前对象的状态设置为当前监控状态(tab和内容显示的div状态都得改变)
                    var $nowonlinediv;
                    var namevalues = $.trim($(obj).attr("name")).split(',');

                    $("#Tab1 .Contentbox >div").each(function () {
                        if ($(this).hasClass("online h50")) {
                            $nowonlinediv = $(this); //保留当前监控tab对象
                            $(this).removeClass("online h50").addClass("queue");

                        }
                        $(this).css("background-color", "#FFFFFF");
                    });
                    $(obj).removeClass("queue").addClass("online h50");

                    $("#messagedivcontenor_contenor >div").each(function () {
                        $(this).hide();
                    });
                    $("#messagedivcontenor" + namevalues[0]).show(); //.find(" div").css("display", "block"); ;
                    $("#finishdivcontenner" + namevalues[0]).show();
                    //  alert("old: " + _searchstarttime + "  ,  new: " + namevalues[1]);
                    //2.更新当前监控会话tab的name属性信息
                    if ($nowonlinediv == null || $nowonlinediv == "") {
                        $("#Tab1 .Contentbox >div[name^='" + _csid + ",']").attr("name", (_csid + "," + _searchstarttime));
                    }
                    else {
                        $nowonlinediv.attr("name", (_csid + "," + _searchstarttime));
                    }
                    //2.更新查询数据的参数
                    if (namevalues.length > 1) {
                        _csid = namevalues[0];
                        _searchstarttime = namevalues[1];
                    }
                    else {
                        $.jAlert("页面脚本出现异常，请重新打开页面！");
                    }
                    setIntervalUp();
                    break;
                case "offline":
                    //1.将当前对象的状态设置为当前监控状态(tab和内容显示的div状态都得改变)
                    var $nowonlinediv;
                    var namevalues = $.trim($(obj).attr("name")).split(',');
                    $("#Tab1 .Contentbox >div").each(function () {
                        if ($(this).hasClass("online h50")) {
                            $nowonlinediv = $(this); //保留当前监控tab对象
                            $(this).removeClass("online h50").addClass("queue");
                        }
                        $(this).css("background-color", "#FFFFFF");
                    });
                    $(obj).css("background-color", "#fcf2ca");
                    $("#messagedivcontenor_contenor >div").each(function () {
                        $(this).hide();
                    });
                    $("#messagedivcontenor" + namevalues[0]).show();
                    $("#finishdivcontenner" + namevalues[0]).show();
                    //2.更新当前监控会话tab的name属性信息
                    if ($nowonlinediv == null || $nowonlinediv == "") {
                        $("#Tab1 .Contentbox >div[name^='" + _csid + ",']").attr("name", (_csid + "," + _searchstarttime));
                    }
                    else {
                        $nowonlinediv.attr("name", (_csid + "," + _searchstarttime));
                    }
                    //2.更新查询数据的参数
                    if (namevalues.length > 1) {
                        _csid = namevalues[0];
                        _searchstarttime = namevalues[1];
                    }
                    else {
                        $.jAlert("页面脚本出现异常，请重新打开页面！");
                    }
                    break;
            }
        }
        //删除监控tab 选项卡事件
        function droptab(obj) {
            var $newobj = $(obj).parent();
            //1.获取对象的class属性已判断其状态
            var objClassName = $.trim($newobj.attr("class"));
            var namevalues = $.trim($newobj.attr("name")).split(',');
            switch (objClassName) {
                case "online h50":
                    //1.让最顶端的queue状态tab变为online状态，并显示相应的内容展示div
                    var $firstqueue = $("#Tab1 .Contentbox div[class*='queue']");
                    var newonlinetabnamevalues = $.trim($firstqueue.eq(0).attr("name")).split(',');
                    if ($firstqueue.length > 0) {
                        $firstqueue.eq(0).removeClass("queue").addClass("online h50");
                        $("#messagedivcontenor" + newonlinetabnamevalues[0]).show().find(" div").css("display", "block"); ;
                        $("#finishdivcontenner" + newonlinetabnamevalues[0]).show();
                        //2.删除当前监控会话的div和tab
                        $("#messagedivcontenor" + namevalues[0]).remove();
                        $("#finishdivcontenner" + namevalues[0]).remove();
                        $newobj.remove();
                        //3. 更新查询数据的参数值
                        if (newonlinetabnamevalues.length > 1) {
                            _csid = newonlinetabnamevalues[0];
                            _searchstarttime = newonlinetabnamevalues[1];
                        }
                        else {
                            $.jAlert("页面脚本出现异常，请重新打开页面！");
                        }
                    }
                    //没有状态为queue的tab存在了，所以直接删除
                    else {
                        //直接删掉此tab以及其对应的会话内容div
                        $("#messagedivcontenor" + namevalues[0]).remove();
                        $("#finishdivcontenner" + namevalues[0]).remove();
                        $newobj.remove();
                        //关闭已开启的定时器
                        clearSetInterval();
                    }
                    break;
                case "queue":
                case "offline":
                    //直接删掉此tab以及其对应的会话内容div
                    $("#messagedivcontenor" + namevalues[0]).remove();
                    $("#finishdivcontenner" + namevalues[0]).remove();
                    $newobj.remove();
                    break;
                default: break;
            }
        }
    </script>
    <!--内容开始-->
    <div class="content">
        <!--查询开始-->
        <div class="searchTj">
            <ul>
                <li>
                    <label>
                        访客姓名：</label><input name="" id="txtVisitorName" type="text" class="w240" /></li>
                <li>
                    <label>
                        地区：</label>
                    <select id="ddlSearchVisitorProvince" class="w240" style="width: 120px" name="ddlSearchVisitorProvince"
                        class="kProvince">
                    </select>
                    <select id="ddlSearchVisitorCity" class="w240" style="width: 120px" name="ddlSearchVisitorCity">
                        <option>城市</option>
                    </select></li>
                <li>
                    <label>
                        访客来源：</label>
                    <select class="w240" id="selSourceType" runat="server">
                    </select></li>
            </ul>
            <div class="clearfix">
            </div>
            <ul>
                <li>
                    <label>
                        会话开始时间：</label>
                    <input name="" type="text" id="txtStartTime" class="w240" style="width: 108px;" />
                    -
                    <input name="" type="text" id="txtEndTime" class="w240" style="width: 108px;" />
                </li>
                <li>
                    <label>
                        客服名称：</label>
                    <input name="" type="text" id="txtAgentName" class="w240" />
                </li>
                <li>
                    <label>
                        所属分组：</label>
                    <select class="w240" id="selBusinessGroup" runat="server">
                    </select></li>
                <li style="width: 80px;">
                    <div class="tjBtn">
                        <input type="button" value="查询" onclick="javascript:search()" class="w60" /></div>
                </li>
            </ul>
            <div class="clearfix">
            </div>
        </div>
        <!--查询结束-->
        <!--列表开始-->
        <div class="cxList dh_gl" style="margin: 0px 26px 0px 25px; overflow-x: hidden; background-color: #F2F2F2;
            border: 1px solid #E6E6E6">
            <table id="freezedivTable" border="0" cellspacing="0" cellpadding="0">
                <thead>
                    <tr>
                        <th width="10%">
                            访客姓名
                        </th>
                        <th width="12%">
                            地区
                        </th>
                        <th width="10%">
                            客服名称
                        </th>
                        <th width="8%">
                            工号
                        </th>
                        <th width="11%">
                            所属分组
                        </th>
                        <th width="10%">
                            会话开始时间
                        </th>
                        <th width="10%">
                            最后消息时间
                        </th>
                        <th width="10%">
                            访客来源
                        </th>
                        <th width="12%">
                            会话发起页面
                        </th>
                        <th width="7%">
                            操作
                        </th>
                    </tr>
                </thead>
                <tbody id="ajaxChatingList">
                </tbody>
            </table>
        </div>
        <!--列表结束-->
        <div class="clearfix">
        </div>
        <div class="jk_dh_start">
            <!--对话列表左开始-->
            <div class="left dh_list">
                <!--对话列表-->
                <div class="dh_info">
                    <div id="Tab1">
                        <div class="Contentbox Contentbox_dh">
                        </div>
                    </div>
                </div>
                <!--对话列表-->
            </div>
            <!--对话列表左结束-->
            <!--对话中开始-->
            <div class="left dh_window">
                <div class="dialogue" id="messagedivcontenor_contenor">
                    <div class="scroll_gd" id="messagedivcontenor">
                    </div>
                </div>
            </div>
            <!--对话中结束-->
        </div>
        <div class="clearfix">
        </div>
    </div>
    <!--内容结束-->
</asp:Content>
