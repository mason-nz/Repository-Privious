<%@ Page Title="客服统计" Language="C#" AutoEventWireup="true" CodeBehind="TrailList.aspx.cs"
    MasterPageFile="~/Controls/Top.Master" Inherits="BitAuto.DSC.IM_2015.Web.TrailManager.TrailList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#txtStartTime').bind('click focus', function () {
                var r = $('#txtEndTime').val().match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
                if (r != null)
                    var d = new Date(r[1], r[3] - 1, r[4]);
                if (r != null && d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4])
                    WdatePicker({ minDate: '#F{$dp.$DV(\'' + d.getFullYear() + '-' + (d.getMonth() + 1) + '-01\',{M:-1});}', maxDate: '#F{$dp.$D(\'txtEndTime\');}', onpicked: function () { document.getElementById("txtEndTime").focus(); } });
                else
                    WdatePicker({ maxDate: '#F{$dp.$D(\'txtEndTime\');}', onpicked: function () { document.getElementById("txtEndTime").focus(); } });
            });

            $('#txtEndTime').bind('click focus', function () {
                var r = $('#txtStartTime').val().match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
                if (r != null)
                    var d = new Date(r[1], r[3] - 1, r[4]);
                if (r != null && d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4])
                    WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '#F{$dp.$DV(\'' + d.getFullYear() + '-' + (d.getMonth() + 1) + '-01\',{M:2,d:-1});}' });
                else
                    WdatePicker({ minDate: '#F{$dp.$D(\'txtStartTime\')}' });
            });
          
            var data = new Date();

            $('#txtStartTime').val(data.getFullYear() + "-" + (data.getMonth() + 1) + "-" + data.getDate());
            $('#txtEndTime').val(data.getFullYear() + "-" + (data.getMonth() + 1) + "-" + data.getDate());


        });

        //查询数据
        function ShowDataByPost1(podyStr) {
            LoadingAnimation("ajaxMessageInfo");
            $('#ajaxMessageInfo').load("/AjaxServers/TrailManager/TrailList.aspx", podyStr, function () { });
        }


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
                $('#ajaxMessageInfo').load("/AjaxServers/TrailManager/TrailList.aspx", podyStr);
            }
        }
        //验证数据格式
        function judgeIsSuccess() {
            var msg = "";
            var txtName = $.trim($("#txtName").val());
            if (txtName.length > 50) {
                msg += "客服字数太多<br/>";
                $("#txtJxsName").val('');
            }

            var beginTime = $.trim($("#txtStartTime").val());
            var endTime = $.trim($("#txtEndTime").val());


            if (beginTime != "") {
                if (!beginTime.isDate()) {
                    msg += "格式不正确<br/>";
                    $("#txtStartTime").val('');
                }
            }

            if (endTime != "") {
                if (!endTime.isDate()) {
                    msg += "时间格式不正确<br/>";
                    $("#txtEndTime").val('');
                }
            }

            if (beginTime != "" && endTime != "") {
                if (beginTime > endTime) {
                    msg += "时间中结束时间不能大于开始时间<br/>";
                    $("#txtStartTime").val('');
                    $("#txtEndTime").val('');
                }
            }
            if (beginTime != "" && endTime != "") {
                if ((parseInt(endTime.substr(5, 2)) - parseInt(beginTime.substr(5, 2)) > 1) && (endTime.substr(0, 4) == beginTime.substr(0, 4))) {
                    msg += "时间间隔不可以超过一个月<br/>";
                    $("#txtStartTime").val('');
                    $("#txtEndTime").val('');
                }

                if (endTime.substr(5, 2) != "01" && beginTime.substr(5, 2) != "12" && (parseInt(endTime.substr(0, 4)) != parseInt(beginTime.substr(0, 4)))) {
                    msg += "时间间隔不可以超过一个月<br/>";
                    $("#txtStartTime").val('');
                    $("#txtEndTime").val('');
                }
            }

            return msg;
        }

        //获取参数
        function _params() {
            var txtName = encodeURIComponent($.trim($('#txtName').val()));
            var groupID = encodeURIComponent($.trim($('#<%=ddGroup.ClientID%>').val()));
            var txtCode = encodeURIComponent($.trim($('#txtCode').val()));
            var txtStartTime = encodeURIComponent($.trim($('#txtStartTime').val()));
            var txtEndTime = encodeURIComponent($.trim($('#txtEndTime').val()));
            var sourceType = $.trim($('#<%=ddlSourceType.ClientID%>').val());

            var pody = {
                UserName: txtName,
                Code: txtCode,
                QueryStarttime: txtStartTime,
                QueryEndTime: txtEndTime,
                SourceType: sourceType,
                GroupId: groupID,
                r: Math.random()  //随机数
            }

            return pody;
        }
        //导出
        function ExportData() {
            var podyStr = JsonObjToParStr(_params());
            window.location = "/AjaxServers/TrailManager/TrailList.aspx?export=1&" + podyStr;
        }
    </script>
    <!--内容开始-->
    <div class="content">
        <!--查询开始-->
        <div class="searchTj">
            <ul>
                <li>
                    <label>
                        访客来源：</label>
                    <select class="w240" id="ddlSourceType" runat="server">
                    </select>
                </li>
                <li>
                    <label>
                        客服：</label><input name="" id="txtName" type="text" class="w240" /></li>
                <li>
                    <label>
                        工号：</label><input name="" type="text" id="txtCode" class="w240" />
                </li>
                <li>
                    <label>
                        时间：</label><input name="" id="txtStartTime" type="text" class="w240" style="width: 108px;" />
                    -
                    <input name="" id="txtEndTime" type="text" class="w240" style="width: 108px;" /></li>
                <li>
                    <label>
                        所属分组：</label>
                    <select class="w240" id="ddGroup" runat="server">
                        <option value='-1'>请选择</option>
                    </select>
                </li>
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
