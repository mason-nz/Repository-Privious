<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupManageAdd.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.GroupManage.GroupManageAdd" %>

<div class="pop pb15 popuser openwindow">
    <script type="text/javascript">
        $(document).ready(function () {
            //设置每一个选项
            $("input[name='ckb_call']").change(function () {
                SetAllSelectChecked();
            });
            //全选按钮设置
            $("#ckb_all").change(function () {
                $("input[name='ckb_call']").attr("checked", $("#ckb_all").attr("checked"));
            });
            BindCall();
            BindLine();
            //设置显示与否
            SetDivShow();
            //添加业务选择事件
            $("input[cname='ckb_btype']").change(function () {
                SetDivShow();
            });
        });
        //外显赋值
        function BindCall() {
            var cdid = '<%=CDID %>';
            if (cdid != "") {
                $("#call_" + cdid).attr("checked", true);
            }
        }
        //业务线赋值
        function BindLine() {
            var lineids = '<%=LineIDs %>';
            if (lineids != "") {
                $(lineids.split(',')).each(function (i, v) {
                    $("#line_" + v).attr("checked", true);
                });
                SetAllSelectChecked();
            }
        }
        //设置全选状态
        function SetAllSelectChecked() {
            if ($("input[name='ckb_call']").length == $("input[name='ckb_call']:checked").length && $("input[name='ckb_call']").length != 0) {
                $("#ckb_all").attr("checked", true);
            }
            else {
                $("#ckb_all").attr("checked", false);
            }
        }
        //设置显示与否
        function SetDivShow() {
            var BusinessType = GetMutilSelectValues("cname", "ckb_btype");

            if (BusinessType == "1") {
                $("#div_cc").css("display", "block");
                $("#div_im").css("display", "none");
            }
            else if (BusinessType == "2") {
                $("#div_cc").css("display", "none");
                $("#div_im").css("display", "block");
            }
            else if (BusinessType == "1,2") {
                $("#div_cc").css("display", "block");
                $("#div_im").css("display", "block");
            }
            else {
                $("#div_cc").css("display", "none");
                $("#div_im").css("display", "none");
            }
        }
    </script>
    <script type="text/javascript">
        //长度限制
        function LimitLength(txt, len) {
            var value = $.trim($(txt).val());
            if (value != "" && value.length > len) {
                value = value.substring(0, 15);
                $(txt).val(value);
            }
        }
        //获取多选值
        function GetMutilSelectValues(key, value) {
            var ids = $("input[" + key + "='" + value + "']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(',');
            return ids;
        }
        //保存按钮
        function SaveData() {
            var Name = $.trim($("#txtBgName").val());
            var AreaID = $.trim($('input:radio[name="rdo_AreaID"]:checked').val());
            var BusinessType = GetMutilSelectValues("cname", "ckb_btype");
            var CDID = $.trim($('input:radio[name="radio_call"]:checked').val());
            var LineIDs = GetMutilSelectValues("name", "ckb_call");

            var param = {
                Action: "DisposeBusinessGroup",
                BGID: '<%=BGID %>',
                Name: encodeURIComponent(Name),
                RegionID: AreaID,
                CDID: encodeURIComponent(CDID),
                BusinessType: BusinessType == "1,2" ? "3" : BusinessType,
                LineIDs: LineIDs,
                r: Math.random()
            }

            //非空校验
            if (Name == '') {
                $.jAlert("请添写分组名称", function () { $("#txtBgName").focus(); });
                return;
            }
            if (AreaID == undefined || AreaID == "") {
                $.jAlert("请选择所属区域");
                return;
            }
            if (BusinessType == undefined || BusinessType == "") {
                $.jAlert("请选择业务分类");
                return;
            }
            if (BusinessType == "1" || BusinessType == "1,2") {
                if (CDID == undefined || CDID == "") {
                    $.jAlert("请选择外显400号码");
                    return;
                }
            }
            if (BusinessType == "2" || BusinessType == "1,2") {
                if (LineIDs == undefined || LineIDs == "") {
                    $.jAlert("请选择负责业务线");
                    return;
                }
            }

            //名称重复校验
            var pody = {
                Action: "CheckGroupNameNotUse",
                BGID: '<%=BGID %>',
                Name: encodeURIComponent(Name),
                r: Math.random()
            }

            AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", pody, null, function (data) {
                if (data == "ok") {
                    SaveData2(param);
                }
                else {
                    $.jAlert("名称重复，请重新输入", function () { $("#txtBgName").focus(); });
                }
            });
        }
        //保存数据
        function SaveData2(param) {
            AjaxPostAsync("/AjaxServers/ZuoxiManage/Handler.ashx", param, null, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "yes") {
                    $.jPopMsgLayer("保存成功", function () { $.closePopupLayer('GroupManageAdd', true); });
                }
                else {
                    $.jAlert(jsonData.msg);
                }
            });
        }
    </script>
    <div class="title bold">
        <h2>
            <%=TitleString %>
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('GroupManageAdd',false);">
        </a></span>
    </div>
    <div id="page1" class="moveC clearfix" style="margin-top: 5px;">
        <ul class="clearfix" style="padding-bottom: 10px;">
            <li>
                <label>
                    <span class="redColor">*</span>分组名称：
                </label>
                <span class="gh">
                    <input type="text" id="txtBgName" value='' class="w125" style="width: 200px;" runat="server"
                        onkeyup="LimitLength(this,15);" onafterpaste="LimitLength(this,15);" />
                </span></li>
            <li>
                <label>
                    <span class="redColor">*</span> 所属区域：
                </label>
                <span class="w400">
                    <label style="width: 150px;">
                        <input type="radio" class="dx" value="1" name="rdo_AreaID" runat="server" id="rdo_areaid_1" />北京
                    </label>
                    <label style="width: 150px;">
                        <input type="radio" class="dx" value="2" name="rdo_AreaID" runat="server" id="rdo_areaid_2" />西安
                    </label>
                </span></li>
            <li>
                <label>
                    <span class="redColor">*</span> 业务分类：
                </label>
                <span class="w400">
                    <label style="width: 150px;">
                        <input type="checkbox" class="dx" value="1" cname="ckb_btype" id="ckb_businesstype_1"
                            runat="server" />热线
                    </label>
                    <label style="width: 150px;">
                        <input type="checkbox" class="dx" value="2" cname="ckb_btype" id="ckb_businesstype_2"
                            runat="server" />在线
                    </label>
                </span></li>
        </ul>
        <div class="moveC clearfix" style="padding-bottom: 10px;" id="div_cc">
            <ul class="clearfix">
                <li style="width: 600px;">
                    <label>
                        <span class="redColor">*</span>外显400号：
                    </label>
                    <span class="w400" style="width: 480px;">
                        <asp:repeater runat="server" id="repeater_call">
                            <ItemTemplate>
                                <label style="width: 230px; line-height: 30px;">
                                    <input id="call_<%#Eval("CDID")%>" name="radio_call" value="<%#Eval("CDID") %>" type="radio" class="dx" />
                                    <%#Eval("CallNum")%>
                                </label>
                            </ItemTemplate>
                        </asp:repeater>
                    </span></li>
            </ul>
        </div>
        <div class="moveC clearfix" style="padding-bottom: 10px;" id="div_im">
            <ul class="clearfix">
                <li style="width: 550px;">
                    <label>
                        <span class="redColor">*</span>负责业务线：
                    </label>
                    <span class="w400" style="width: 400px;">
                        <label style="width: 650px;">
                            <input id="ckb_all" type="checkbox" class="dx" />
                            全选
                        </label>
                        <asp:repeater runat="server" id="repeater_line">
                            <ItemTemplate>
                                <label style="width: 120px; line-height: 30px;">
                                    <input id="line_<%#Eval("RecID") %>" name="ckb_call" value="<%#Eval("RecID") %>" type="checkbox" class="dx" />
                                    <%#Eval("Name")%>
                                </label>
                            </ItemTemplate>
                        </asp:repeater>
                    </span></li>
            </ul>
        </div>
        <div class="btn mt20">
            <input id="btnSave_Page1" type="button" onclick="SaveData();" value="保 存" class="btnSave bold" />&nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" value="取 消" class="btnCannel bold" onclick="javascript:$.closePopupLayer('GroupManageAdd',false);" />
        </div>
    </div>
</div>
