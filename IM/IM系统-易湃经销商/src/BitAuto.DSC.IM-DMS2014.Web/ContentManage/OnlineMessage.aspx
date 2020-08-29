<%@ Page Title="在线留言" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="OnlineMessage.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.ContentManage.OnlineMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            BindSelDistrictData();
            $('#txtStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () {document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}' }); });
        });

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
                    }
                }
            });
        }

        function BindSelectChange() {
            var pid = $("#selDistrict").val();
            $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'getcitydata', District: pid, r: Math.random() }, function (data) {
                $("#selCity").html("");
                $("#selCity").append("<option value='-1'>请选择</option>");
                if (data != "") {
                    var jsonData = $.evalJSON(data);
                    if (jsonData != "") {
                        $.each(jsonData.root, function (idx, item) {
                            $("#selCity").append("<option value='" + item.value + "'>" + item.name + "</option>");
                        });
                    }
                }
            });
        }
        
    </script>
    <script type="text/javascript">
        $(function () {
            search();
        })
        //查询
        function search() {
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg);
                return false;
            }
            else {
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);

                LoadingAnimation("ajaxMessageInfo");
                $('#ajaxMessageInfo').load("/AjaxServers/ContentManage/OnlineMessageData.aspx", podyStr);
            }
        }
        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";
            var txtjxsName = $.trim($("#txtJxsName").val());
            if (txtjxsName.length > 50) {
                msg += "经销商名称字数太多<br/>";
                $("#txtJxsName").val('');
            }
            var txtAgentName = $.trim($("#txtAgentName").val());
            if (txtAgentName.length > 20) {
                msg += "操作人名称字数太多<br/>";
                $("#txtAgentName").val('');
            }
            var beginTime = $.trim($("#txtStartTime").val());
            var endTime = $.trim($("#txtEndTime").val());

            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "留言时间格式不正确<br/>";
                    $("#txtStartTime").val('');
                }
            }

            if (endTime != "") {
                if (!endTime.isDate()) {
                    msg += "留言时间格式不正确<br/>";
                    $("#txtEndTime").val('');
                }
            }

            if (beginTime != "" && endTime != "") {
                if (beginTime > endTime) {
                    msg += "留言时间中结束时间不能大于开始时间<br/>";
                    $("#txtStartTime").val('');
                    $("#txtEndTime").val('');
                }
            }

            return msg;
        }

        //获取参数
        function _params() {
            var txtJxsName = encodeURIComponent($.trim($('#txtJxsName').val()));
            var selDistict = encodeURIComponent($.trim($('#selDistrict').val()));
            var selCity = encodeURIComponent($.trim($('#selCity').val()));
            var txtAgentName = encodeURIComponent($.trim($('#txtAgentName').val()));
            var selMessageState = encodeURIComponent($.trim($('#selMessageState').val()));
            var txtStartTime = encodeURIComponent($.trim($('#txtStartTime').val()));
            var txtEndTime = encodeURIComponent($.trim($('#txtEndTime').val()));


            var pody = {
                MemberName: txtJxsName,
                District: selDistict,
                CityGroup: selCity,
                LastModifyUserName: txtAgentName,
                QueryStarttime: txtStartTime,
                QueryEndTime: txtEndTime,
                MessageState: selMessageState,
                r: Math.random()  //随机数
            }

            return pody;
        }


        function changeMessageState(obj) {
            var selValue = $(obj).val();
            var firstValue = $(obj).find(" option").eq(0).val();
            if (firstValue != selValue) {
                $.jConfirm("您确认要执行此操作吗？", function (isOk) {
                    if (isOk) {

                        var selRecID = $(obj).attr("name");
                        $.get("/AjaxServers/LayerDataHandler.ashx", { Action: 'addremarkinfo', RemarkRecID: encodeURIComponent(selRecID), MessageStatus: encodeURIComponent(selValue), r: Math.random() }, function (data) {
                            if (data == "success") {
                                $.jAlert("状态修改成功！");

                                reloadpagedata();     //--------------------------------------------------------------------------
                            }
                            else {
                                $.jAlert("操作出现异常，请稍后再试！");
                            }
                        });
                    }
                    else {
                        $(obj).find(" option").removeAttr("selected").eq(0).attr("selected", "selected");
                    }
                });
            }

        }
        function OpenAddRemarkLayer(RecID, CType) {

            $.openPopupLayer({
                name: "AddRemarkLayerAjaxPopup",
                parameters: {},
                url: "/AjaxServers/ContentManage/AddRemarkForm.aspx?RecID=" + RecID + "&CType=" + CType + "&r=" + Math.random()
            });

        };

        function OpenContentDetailLayer(RecID) {
            $.openPopupLayer({
                name: "ContentDetailLayerAjaxPopup",
                parameters: {},
                url: "/AjaxServers/ContentManage/ContentsDetailForm.aspx?RecID=" + RecID + "&r=" + Math.random()
            });
        }
        //导出
        function ExportData() {
            var podyStr = JsonObjToParStr(_params());
            window.location = "/AjaxServers/ContentManage/OnlineMessageExport.aspx?" + podyStr;
        }

        //分页操作 
        function ShowDataByPost1(pody) {
            LoadingAnimation("ajaxMessageInfo");
            $('#ajaxMessageInfo').load("/AjaxServers/ContentManage/OnlineMessageData.aspx", pody);
        }
    </script>
    <script type="text/javascript">
        function reloadpagedata() {
            var page = $("#ajaxMessageInfo #itPage a[class='active']").text();
            var msg = judgeIsSuccess();
            if (msg != "") {
                $.jAlert(msg);
                return false;
            }
            else {
                var pody = _params();
                var podyStr = JsonObjToParStr(pody);

                LoadingAnimation("ajaxMessageInfo");
                $('#ajaxMessageInfo').load("/AjaxServers/ContentManage/OnlineMessageData.aspx?page=" + page + "&", podyStr);
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
                        经销商名称：</label><input name="" id="txtJxsName" type="text" class="w240" /></li>
                <li>
                    <label>
                        所属大区：</label><select class="w240" id="selDistrict" onchange="javascript:BindSelectChange()"><option
                            value='-1'>请选择</option>
                        </select></li>
                <li>
                    <label>
                        所属城市群：</label><select class="w240" id="selCity"><option value='-1'>请选择</option>
                        </select></li>
                <li>
                    <label>
                        操作人：</label><input name="" type="text" id="txtAgentName" class="w240" />
                </li>
                <li>
                    <label>
                        状态：</label>
                    <select class="w240" id="selMessageState">
                        <option value='-1'>请选择</option>
                        <option value='1'>新建</option>
                        <option value='2'>处理中</option>
                        <option value='3'>已完成</option>
                    </select>
                </li>
                <li>
                    <label>
                        时间：</label><input name="" id="txtStartTime" type="text" class="w240" style="width: 108px;" />
                    -
                    <input name="" id="txtEndTime" type="text" class="w240" style="width: 108px;" /></li>
                <li style="width: 80px;">
                    <div class="tjBtn">
                        <input type="button" value="查询" onclick="search()" class="w60" /></div>
                </li>
            </ul>
            <div class="clearfix">
            </div>
        </div>
        <!--查询结束-->
        <div class="dc">
        </div>
        <!--列表开始-->
        <div id="ajaxMessageInfo" class="cxList" style="margin-top: 8px; height: auto;">
        </div>
        <!--列表结束-->
        <div class="clearfix">
        </div>
    </div>
    <!--内容结束-->
</asp:Content>
