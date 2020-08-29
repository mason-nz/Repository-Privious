<%@ Page Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AlloCustomer.List"
    Title="客户分配" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="search clearfix">
        <ul class="clear">
            <li>
                <label>
                    客户名称：</label>
                <input type="text" id="txtCustName" class="w190" />
            </li>
            <li>
                <label>
                    客户地区：</label>
                <select id="ddlSearchProvince" style="width: 81px" name="ddlSearchProvince" class="kProvince">
                </select>
                <select id="ddlSearchCity" style="width: 81px" name="ddlSearchCity" onchange="javascript:BindCounty('ddlSearchProvince','ddlSearchCity','ddlSearchCounty');">
                </select>
                <select id="ddlSearchCounty" name="ddlSearchCounty" class="kArea" style="width: 81px;">
                </select>
            </li>
            <li>
                <label>
                    所属大区：</label>
                <select id="selArea" class="w160" runat="server">
                </select>
            </li>
            <li>
                <label>
                    负责客服：</label>
                <input type="text" id="txtKeFuName" class="w190" readonly="true" onclick="selectServicePop()" />
                <input type="checkbox" id="chkIsKeFu" onclick='clearKeFu()' value="-1" style="vertical-align: middle;" />
                <em style="vertical-align: middle;" onclick="emChkIsChoose(this);clearKeFu()">无客服
                </em></li>
            <li class="btnsearch" style="width: 552px;margin-right: 0px">
                <input style="float: right" name="" id="btnSearch" type="button" value="查 询" onclick="javascript:search()" />
            </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (right_export)
          { %>
        <input type="button" id="btnExport" value="导出" class="newBtn" onclick="ExportData()" />
        <%} %>
        <%if (right_withdraw)
          { %>
        <input type="button" id="btnWithDraw" value="收回" class="newBtn" onclick="operRecede()" />
        <%} %>
        <%if (right_allocation)
          { %>
        <input type="button" id="btnAllocation" value="分配" class="newBtn" onclick="operAssign()" />
        <%} %>
    </div>
    <div id="ajaxTable">
    </div>
    <script src="/Js/Enum/Area2.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);

            BindProvince('ddlSearchProvince'); //绑定省份
            $("[id$=ddlSearchProvince]").change(function () {
                BindCity('ddlSearchProvince', 'ddlSearchCity');
                BindCounty('ddlSearchProvince', 'ddlSearchCity', 'ddlSearchCounty');
            });
            $("[id$=ddlSearchCity]").change(function () {
                BindCounty('ddlSearchProvince', 'ddlSearchCity', 'ddlSearchCounty.ClientID');
            });

            search();
        });

        //查询
        function search() {
            var pody = _params();
            var podyStr = JsonObjToParStr(pody);

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AlloCustomer/AjaxServers/AjaxList.aspx", podyStr);
        }
        //获取参数
        function _params() {

            var cName = encodeURIComponent($.trim($("#txtCustName").val()));
            var pid = encodeURIComponent($.trim($("#ddlSearchProvince").val()));
            var ctid = encodeURIComponent($.trim($("#ddlSearchCity").val()));
            var cnid = encodeURIComponent($.trim($("#ddlSearchCounty").val()));
            var area = encodeURIComponent($.trim($("[id$='selArea']").val()));
            var kef = encodeURIComponent($.trim($("#txtKeFuName").val()));
            if (kef == "" && $("#chkIsKeFu").attr("checked") == true) {
                kef = "-1";
            }
            var pody = {
                CustName: cName,
                Province: pid,
                City: ctid,
                County: cnid,
                Area: area,
                KeFuName: kef,
                r: Math.random()
            }

            return pody;
        }

        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation("tableList");
            $('#ajaxTable').load('/AlloCustomer/AjaxServers/AjaxList.aspx', pody + "&r=" + Math.random());
        }

    </script>
    <script type="text/javascript">
        function clearKeFu() {
            var chkKeFu = document.getElementById('chkIsKeFu');
            if (chkKeFu.checked) {
                $("#txtKeFuName").val('');
            }
        }

        //选择负责客服
        function selectServicePop(afterFn) {

            $.openPopupLayer({
                name: "AlloServicePop",
                url: "/AlloCustomer/AlloServicePop.aspx",
                success: function () {
                    if (afterFn) { $("#popAClear").hide(); }
                },
                beforeClose: function (e) {
                    if (!e) return;

                    var userid = $('#popupLayer_' + 'AlloServicePop').data('userid');
                    if (afterFn) {
                        afterFn(userid);
                    }
                    else {
                        $("#txtKeFuName").val($('#popupLayer_' + 'AlloServicePop').data('name'));
                        if ($("#txtKeFuName").val() != "") {
                            $("#chkIsKeFu").attr("checked", false);
                        }

                    }
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(search);
                }
            });
        }

        //分配操作
        function operAssign(custid) {
            var custIDs = custid ? custid : getCustIDs();
            if (custIDs == "") {
                $.jAlert("请选择客户！");
                return;
            }

            selectServicePop(function (userid) {
                $.post("/AlloCustomer/AjaxServers/Handler.ashx", { Action: "assignTask", UserID: userid, CustIDs: custIDs, r: Math.random() }, function (data) {
                    var jsonData = eval("(" + data + ")");
                    if (jsonData.result == "true") {
//                        $.jAlert("客户分配成功", function () {
//                            search();
//                        });
                        $.jPopMsgLayer("客户分配成功", function () {
                            search();
                        });
                    }
                    else {
                        $.jAlert(jsonData.msg);
                    }
                });
            });

        }
        //收回操作
        function operRecede(custid) {
            var custIDs = custid ? custid : getCustIDs();
            if (custIDs == "") {
                $.jAlert("请选择客户！");
                return;
            }

            $.jConfirm("是否确定收回被分配的客服？", function (r) {
                if (!r) return;

                $.post("/AlloCustomer/AjaxServers/Handler.ashx", { Action: "recedeTask", CustIDs: custIDs, r: Math.random() }, function (data) {
                    var jsonData = eval("(" + data + ")");
                    if (jsonData.result == "true") {
//                        $.jAlert("客户收回成功", function () {
//                            search();
//                        });
                        $.jPopMsgLayer("客户收回成功", function () {
                            search();
                        });
                        
                    }
                    else {
                        $.jAlert(jsonData.msg);
                    }
                });

            });
        }

        //获取TaskIDs
        function getCustIDs() {
            var custIDs = "";
            var aSelect = document.getElementsByName("chkSelect");
            for (var i = 0; i < aSelect.length; i++) {
                if (aSelect[i].checked) {
                    custIDs += aSelect[i].value + ',';
                }
            }
            custIDs ? custIDs = custIDs.substring(0, custIDs.length - 1) : "";
            return custIDs;
        }

        //导出
        function ExportData() {
            var podyStr = JsonObjToParStr(_params());
            window.location = "/AlloCustomer/Export.aspx?Browser=" + GetBrowserName() + podyStr;
        }
    </script>
</asp:Content>
