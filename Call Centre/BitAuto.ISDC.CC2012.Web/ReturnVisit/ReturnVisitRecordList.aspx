<%@ Page Language="C#" Title="回访记录列表页" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="ReturnVisitRecordList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ReturnVisit.ReturnVisitRecordList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
    <link rel="stylesheet" type="text/css" href="../../css/GooCalendar.css" />
    <script type="text/javascript" src="../Js/Enum/Area2.js"></script>
    <script type="text/javascript" src="../js/GooCalendar.js"></script>
    <script type="text/javascript">
        var property2 = {
            divId: "calen1", //日历控件最外层DIV的ID
            needTime: true, //是否需要显示精确到秒的时间选择器，即输出时间中是否需要精确到小时：分：秒 默认为FALSE可不填
            yearRange: [1970, 2030], //可选年份的范围,数组第一个为开始年份，第二个为结束年份,如[1970,2030],可不填
            //week: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'], //数组，设定了周日至周六的显示格式,可不填
            //month: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'], //数组，设定了12个月份的显示格式,可不填
            format: "yyyy-MM-dd hh:mm:ss"
            /*设定日期的输出格式,可不填*/
        };
        $(document).ready(function () {

            //            $.createGooCalendar('txtStartTime', property2);
            //            $.createGooCalendar('txtEndTime', property2);


            //敲回车键执行方法
            enterSearch(search);

            BindProvince('<%=ddlSearchProvince.ClientID%>'); //绑定省份
            $("[id$=ddlSearchProvince]").change(function () {
                BindCity('<%=ddlSearchProvince.ClientID%>', '<%=ddlSearchCity.ClientID%>');
                BindCounty('<%=ddlSearchProvince.ClientID%>', '<%=ddlSearchCity.ClientID%>', '<%=ddlSearchCounty.ClientID%>');
            });
            $("[id$=ddlSearchCity]").change(function () {
                BindCounty('<%=ddlSearchProvince.ClientID%>', '<%=ddlSearchCity.ClientID%>', '<%=ddlSearchCounty.ClientID%>');
            });
            //全选/全不选
            $("#ckbAllSelect").live("click", function () {
                $(":checkbox[name='chkSelect']").attr("checked", $(this).attr("checked"));
            });
            search();

            $('#txtStartTime').bind('click focus', function () { WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', startDate: "%y-%M-%d 00:00:00", maxDate: '#F{$dp.$D(\'txtEndTime\')}', onpicked: function () { document.getElementById("txtEndTime").focus(); } }); });
            $('#txtEndTime').bind('click focus', function () { WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', startDate: "%y-%M-%d 23:59:59", minDate: '#F{$dp.$D(\'txtStartTime\')}' }); });


        });
        function search() {
            var pagesize = $("#hidSelectPageSize").val();
            var url = '../AjaxServers/ReturnVisit/ReturnVisitRecordList.aspx?pagesize=' + pagesize;
            var txtSearchCustName = $.trim($('#' + '<%=txtSearchCustName.ClientID%>').val());
            var txtCustID = $.trim($('#' + '<%=txtCustID.ClientID%>').val());
            var ddlSearchProvince = $.trim($('#' + '<%=ddlSearchProvince.ClientID%>').val());
            var ddlSearchCity = $.trim($('#' + '<%=ddlSearchCity.ClientID%>').val());
            var ddlSearchCounty = $.trim($('#' + '<%=ddlSearchCounty.ClientID%>').val());
            var selCustType = $.trim($('#selCustType').val());
            var txtSearchProjectName = $.trim($("input[id$='txtSearchProjectName']").val());

            var TypeID = 0;
            if ($("[id$='chkTypeNew']").attr("checked") == true) {
                TypeID = Number(TypeID) + Number($("[id$='chkTypeNew']").val());
            }
            if ($("[id$='chkTypeSnd']").attr("checked") == true) {
                TypeID = Number(TypeID) + Number($("[id$='chkTypeSnd']").val());
            }
            if ($("[id$='chkTypeYK']").attr("checked") == true) {
                TypeID = Number(TypeID) + Number($("[id$='chkTypeYK']").val());
            }

            var txtStartTime = $.trim($("input[id$='txtStartTime']").val());
            var txtEndTime = $.trim($("input[id$='txtEndTime']").val());

            var selVisitType = $.trim($('#' + '<%=selVisitType.ClientID%>').val());
            var SelectUserid = $.trim($("input[id$='hidSelectUserid']").val());

            if (txtStartTime != "" && (!txtStartTime.isDateTime())) {
                $.jAlert("访问时间开始时间格式不正确！");
            }
            else if (txtEndTime != "" && (!txtEndTime.isDateTime())) {
                $.jAlert("访问时间结束时间格式不正确！");
            }
            else {
                url += '&CustName=' + escape(txtSearchCustName);
                url += '&CustID=' + escape(txtCustID);
                url += '&Province=' + escape(ddlSearchProvince);
                url += '&City=' + escape(ddlSearchCity);
                url += '&County=' + escape(ddlSearchCounty);
                url += '&CustType=' + escape(selCustType);
                url += '&StartTime=' + escape(txtStartTime);
                url += '&EndTime=' + escape(txtEndTime);
                url += '&TypeID=' + escape(TypeID);
                url += '&VisitType=' + escape(selVisitType);
                url += '&VisitUserid=' + escape(SelectUserid);
                url += '&ProjectName=' + escapeStr(txtSearchProjectName);
                LoadingAnimation("divContent");
                $('#divContent').load(url + "&" + Math.random());
            }
        }

        //弹出集采项目名
        function openAjaxProjectNameSearchAjaxPopup() {
            $.openPopupLayer({
                name: "ProjectNameSelectAjaxPopup",
                parameters: {},
                url: "/AjaxServers/Base/SelectProjectName.aspx",
                beforeClose: function (b, cData) {

                    var ProjectName = $('#popupLayer_' + 'ProjectNameSelectAjaxPopup').data('ProjectName');
                    //$('#txtSearchProjectName').val(ProjectName);
                    $("input[id$='txtSearchProjectName']").val(ProjectName);
                }
            });
        }

        //选择访问人
        function SelectVisitPerson() {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    $('#hidSelectUserid').val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                    $('#txtVisitPerson').val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                }
            });
        }
        //点击文字，选中复选框
        function emChkIsChoose(othis) {
            var $checkbox = $(othis).prev();
            if ($checkbox.is(":checked")) {
                $checkbox.removeAttr("checked");
            }
            else {
                $checkbox.attr("checked", "checked");
            }
        }

        function Export() {
            var url = '../AjaxServers/ReturnVisit/ExportReturnVistRecord.aspx?Export=true';
            var txtSearchCustName = $.trim($('#' + '<%=txtSearchCustName.ClientID%>').val());
            var txtCustID = $.trim($('#' + '<%=txtCustID.ClientID%>').val());
            var ddlSearchProvince = $.trim($('#' + '<%=ddlSearchProvince.ClientID%>').val());
            var ddlSearchCity = $.trim($('#' + '<%=ddlSearchCity.ClientID%>').val());
            var ddlSearchCounty = $.trim($('#' + '<%=ddlSearchCounty.ClientID%>').val());
            var selCustType = $.trim($('#selCustType').val());
            var txtSearchProjectName = $.trim($("input[id$='txtSearchProjectName']").val());

            var TypeID = 0;
            if ($("[id$='chkTypeNew']").attr("checked") == true) {
                TypeID = Number(TypeID) + Number($("[id$='chkTypeNew']").val());
            }
            if ($("[id$='chkTypeSnd']").attr("checked") == true) {
                TypeID = Number(TypeID) + Number($("[id$='chkTypeSnd']").val());
            }
            if ($("[id$='chkTypeYK']").attr("checked") == true) {
                TypeID = Number(TypeID) + Number($("[id$='chkTypeYK']").val());
            }

            var txtStartTime = $.trim($("input[id$='txtStartTime']").val());
            var txtEndTime = $.trim($("input[id$='txtEndTime']").val());

            var selVisitType = $.trim($('#' + '<%=selVisitType.ClientID%>').val());
            var SelectUserid = $.trim($("input[id$='hidSelectUserid']").val());

            if (txtStartTime != "" && (!txtStartTime.isDateTime())) {
                $.jAlert("访问时间开始时间格式不正确！");
            }
            else if (txtEndTime != "" && (!txtEndTime.isDateTime())) {
                $.jAlert("访问时间结束时间格式不正确！");
            }
            else {
                url += '&CustName=' + escape(txtSearchCustName);
                url += '&CustID=' + escape(txtCustID);
                url += '&Province=' + escape(ddlSearchProvince);
                url += '&City=' + escape(ddlSearchCity);
                url += '&County=' + escape(ddlSearchCounty);
                url += '&CustType=' + escape(selCustType);
                url += '&StartTime=' + escape(txtStartTime);
                url += '&EndTime=' + escape(txtEndTime);
                url += '&TypeID=' + escape(TypeID);
                url += '&VisitType=' + escape(selVisitType);
                url += '&VisitUserid=' + escape(SelectUserid);
                url += '&ProjectName=' + escapeStr(txtSearchProjectName);
            }

            window.location = url;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="search clearfix">
        <!--主体内容部分star-->
        <ul class="clear" id="ulNormal">
            <li>
                <label>
                    客户名称：</label><input type="text" maxlength="100" class="w190" name="txtSearchCustName"
                        id="txtSearchCustName" runat="server" /></li>
            <li>
                <label style="width: 61px">
                    客户ID：</label><input type="text" style="width: 267px" class="w125" name="txtCustID"
                        maxlength="20" id="txtCustID" runat="server" /></li>
            <li>
                <label style="width: 60px">
                    客户地区：</label>
                <select id="ddlSearchProvince" style="width: 80px" name="ddlSearchProvince" class="kProvince"
                    runat="server">
                </select>
                <select id="ddlSearchCity" onchange="javascript:BindCounty('<%=ddlSearchProvince.ClientID%>','<%=ddlSearchCity.ClientID%>','<%=ddlSearchCounty.ClientID%>');"
                    style="width: 63px" name="ddlSearchCity" runat="server">
                </select>
                <select id="ddlSearchCounty" name="ddlSearchCounty" class="kArea" style="width: 63px;"
                    runat="server">
                </select></li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    客户类型：</label>
                <select id="selCustType" style="width: 195px">
                    <option value="-2" selected="selected">请选择</option>
                    <option value="20001">厂商</option>
                    <option value="20002">集团</option>
                    <option value="20003">4s</option>
                    <option value="20004">特许经销商</option>
                    <option value="20005">综合店</option>
                    <option value="20007">汽车服务商</option>
                    <option value="20011">经纪公司</option>
                    <option value="20010">个人</option>
                    <option value="20012">交易市场</option>
                    <option value="20006">其它</option>
                    <option value="20013">车易达</option>
                    <option value="20014">二手车中心</option>
                </select>
            </li>
            <li>
                <label style="width: 60px">
                    集采项目：</label><input type="text" style="width: 267px" onclick="openAjaxProjectNameSearchAjaxPopup();"
                        name="txtSearchProjectName" id="txtSearchProjectName" runat="server" class="w125" /></li>
            <li>
                <label style="width: 60px">
                    所属业务：</label>
                <span style="width: 50px; text-align: left;">
                    <input id="chkTypeNew" name='carType' type="checkbox" value="1" /><em onclick="emChkIsChoose(this)">新车</em></span>
                <span style="width: 90px; text-align: left;">
                    <input id="chkTypeSnd" type="checkbox" name='carType' value="2" /><em onclick="emChkIsChoose(this)">二手车</em></span>
                <span style="width: 60px; text-align: left;">
                    <input id="chkTypeYK" type="checkbox" name='carType' value="4" /><em onclick="emChkIsChoose(this)">易卡</em></span>
            </li>
        </ul>
        <ul class="clear">
            <li>
                <label>
                    访问分类：</label>
                <select id="selVisitType" style="width: 195px" runat="server">
                    <option value="-2" selected="selected">请选择</option>
                </select>
            </li>
            <li>
                <label style="width: 60px">
                    访问时间：</label>
                <input type="text" id="txtStartTime" class="w90" style="width: 120px;" />
                <span style="padding-right: 3px;">至</span>
                <input type="text" id="txtEndTime" class="w90" style="width: 120px;" />
            </li>
            <li>
                <label style="width: 60px">
                    访问人：</label><input type="text" maxlength="50" onclick="SelectVisitPerson()" class="w90"
                        id="txtVisitPerson" /></li><li class="btnsearch">
                            <input name="" type="button" value="查 询" onclick="javascript:search()" />
                        </li>
        </ul>
    </div>
    <input type="hidden" id="hidSelectUserid" /><input type="hidden" id="hidSelectPageSize"
        value="" />
    <% if (IsExport)
       {%>
    <div class="optionBtn clearfix">
        <input type="button" class="newBtn" onclick="Export()" value="导出" name="" />
    </div>
    <%} %>
    <div id="divContent">
    </div>
</asp:Content>
