<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    Title="个人用户列表" CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustBaseInfo.CustBaseInfoList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Js/jquery.uploadify.v2.1.4.min.js"></script>
    <script type="text/javascript" src="../Js/swfobject.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //初始化日历控件，前面的日期不能大于后面的日期
            //敲回车键执行方法
            enterSearch(search);

            BindProvince('ddlProvince'); //绑定省份
            triggerProvince();

            //初始化联系电话
            var tel = "";
            tel = '<%=RequestCustTel %>';
            if (tel != "") {
                $("#txtCustTel").val(tel);
            }

            //search();
        });
        function triggerProvince() {//选中省份
            BindCity('ddlProvince', 'ddlCity');
            BindCounty('ddlProvince', 'ddlCity', 'ddlCounty');
        }

        function triggerCity() {//选中城市
            BindCounty('ddlProvince', 'ddlCity', 'ddlCounty');
            //若城市列表中，没有数据，则添加属性noCounty，值为1，否则不添加属性
            if ($('#ddlCounty option').size() == 1)
            { $('#ddlCounty').attr('noCounty', '1'); }
            else
            { $('#ddlCounty').removeAttr('noCounty'); }
        }
        function GetPody() {
            var custName = $.trim($("#txtCustName").val());
            var sex = $("input[name='sex']").map(function () {
                if ($(this).attr("checked")) {
                    return $(this).val();
                }
            }).get().join(','); ;
            var custTel = $("#txtCustTel").val();
            var province = $("#ddlProvince").val();
            var city = $("#ddlCity").val();
            var county = $("#ddlCounty").val();
            var pody = {
                CustName: encodeURIComponent(custName),
                Sexs: encodeURIComponent(sex),
                CustTel: encodeURIComponent(custTel),
                ProvinceID: encodeURIComponent(province),
                CityID: encodeURIComponent(city),
                CountyID: encodeURIComponent(county),
                R: Math.random()
            }
            return pody;
        }
        //校验
        function judge() {
            var pody = GetPody();
            if (pody.CustTel == "" && pody.CustName == "") {
                $.jAlert("请填写客户姓名或者联系电话后查询！", function () {
                    $("#txtCustName").focus();
                });
                return false;
            }
            return true;
        }
        //查询
        function search() {
            if (judge()) {
                //加载查询结果
                LoadingAnimation("ajaxTable");
                //*添加监控
                var monitorPageTime = new Date().getTime(); //监控页面加载耗时_开始时间
                var podyStr = GetPody();
                $("#ajaxTable").load("../AjaxServers/CustBaseInfo/List.aspx?r=" + Math.random(), podyStr, function () {
                    StatAjaxPageTime(monitorPageTime, "/AjaxServers/CustBaseInfo/List.aspx?" + JsonObjToParStr(podyStr));
                });
            }
        }
        function OpenAddCustBaseInfo() {
            var root = "<%=ExitAddress %>";
            var url = "/WOrderV2/AddWOrderInfo.aspx?<%=Param %>";
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + url);
            }
            catch (e) {
                window.open(url, '', 'height=900,width=1050,left=200,toolbar=no,menubar=no,scrollbars=yes,resizable=no,location=no,status=no');
            }
        }
        function openUploadExcelInfoAjaxPopup() {
            $.openPopupLayer({
                name: "UploadUserAjaxPopup",
                parameters: {},
                url: "../DataImport/Main.aspx"
            });
        }
    </script>
    <script type="text/javascript">
        function ExportExcel() {
            if (judge()) {
                $("#hidIsOkOrCancel").val("0");
                $.openPopupLayer({
                    name: "DisposeSetPoper",
                    parameters: {},
                    url: "/CustBaseInfo/CustomerFields.aspx",
                    afterClose: function (e, data) {
                        if ($("#hidIsOkOrCancel").val() == "1") { //是否是点击了确定后关闭的
                            var ids = $("#hidFieldsCustomer").val();
                            //导出
                            var pody = GetPody();
                            $("#formExport [name='CustName']").val(escapeStr(pody.CustName));
                            $("#formExport [name='Sexs']").val(escapeStr(pody.Sexs));
                            $("#formExport [name='CallTime']").val(escapeStr(pody.CallTime));
                            $("#formExport [name='CustTel']").val(escapeStr(pody.CustTel));
                            //经销商名称
                            $("#formExport [name='DealerName']").val(escapeStr(pody.DealerName));
                            $("#formExport [name='BeginTime']").val(escapeStr(pody.BeginTime));
                            $("#formExport [name='EndTime']").val(escapeStr(pody.EndTime));
                            $("#formExport [name='ProvinceID']").val(escapeStr(pody.ProvinceID));
                            $("#formExport [name='CityID']").val(escapeStr(pody.CityID));
                            $("#formExport [name='CountyID']").val(escapeStr(pody.CountyID));
                            $("#formExport [name='AreaIDs']").val(escapeStr(pody.AreaIDs));
                            $("#formExport [name='DataSources']").val(escapeStr(pody.DataSources));
                            $("#formExport [name='CityScopes']").val(escapeStr(pody.CityScopes));
                            $("#formExport [name='ConsultTypes']").val(escapeStr(pody.ConsultTypes));
                            $("#formExport [name='AgentNum']").val(escapeStr(pody.AgentNum));
                            $("#formExport [name='field']").val(escapeStr(ids));
                            $("#formExport").submit();
                        }
                    }
                });
            }
        }
        //选择坐席名称
        function SelectVisitPerson() {
            $.openPopupLayer({
                name: "AssignTaskAjaxPopupForSelect",
                url: "/AjaxServers/ReturnVisit/CustAssignUserList.aspx",
                beforeClose: function (e) {
                    $("[id$='txtSearchTrueName']").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('name'));
                    $("#txtAgentNum").val($('#popupLayer_' + 'AssignTaskAjaxPopupForSelect').data('userid'));
                },
                afterClose: function () {
                    //敲回车键执行方法
                    enterSearch(search);
                }
            });
        }
    </script>
    <div class="searchTj" style="width: 100%;">
        <ul style="width: 98%;">
            <li>
                <label>
                    客户姓名：</label>
                <input type="text" id="txtCustName" class="w190" />
            </li>
            <li>
                <label>
                    客户地区：</label><select id="ddlProvince" class="w90" style="width: 70px" onchange="triggerProvince()"></select><select
                        id="ddlCity" class="w90" style="width: 66px" onchange="triggerCity()"></select><select
                            id="ddlCounty" class="w90" style="width: 70px"></select></li>
            <li style="width: 200px; *width: 200px;">
                <label>
                    客户性别：</label>
                <input type="checkbox" style="border: none;" name="sex" value="1" /><em onclick="emChkIsChoose(this);">先生</em><input
                    type="checkbox" style="border: none;" name="sex" style="margin-left: 20px;" value="2" /><em
                        onclick="emChkIsChoose(this);">女士</em></li>
        </ul>
        <ul style="width: 98%;">
            <li>
                <label>
                    联系电话：</label>
                <input id="txtCustTel" class="w190" />
            </li>
            <li style="width: 300px; display: none">
                <label>
                    坐席名称：</label>
                <div class="coupon-box02" style="float: left;">
                    <input type="text" id="txtSearchTrueName" class="text02" readonly="readonly" />
                    <b onclick="SelectVisitPerson()"><a href="javascript:void(0)">选择</a></b>
                    <input type="hidden" id="txtAgentNum" class="w100" /></div>
            </li>
            <li class="btnsearch" style="clear: none; margin-top: 5px; width: 100px">
                <input type="button" value="查 询" id="btnsearch" class="cx" onclick="search()" name="" /></li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (DataImportButton)
          {%>
        <input name="" type="button" value="导入Excel" style="display: none" onclick="openUploadExcelInfoAjaxPopup()"
            class="newBtn mr10" />
        <%}%>
        <%if (DataExportButton)
          {%>
        <input name="" type="button" value="导出客户信息" style="display: none" onclick="javascript:ExportExcel()"
            class="newBtn" style="*margin-top: 3px;" />
        <%}%>
        <%if (DelButtonAuth)
          { %><input name="" type="button" value="删 除" class="newBtn" style="display: none"
              onclick="DeleteCustBaseInfo()" /><%} %>
        <input name="" type="button" value="新 增" class="newBtn" onclick="OpenAddCustBaseInfo()" />
    </div>
    <div class="bit_table" width="99%" cellspacing="0" cellpadding="0" id="ajaxTable">
    </div>
    <input type="hidden" id="hidFieldsCustomer" value="" />
    <input type="hidden" id="hidIsOkOrCancel" value="0" />
    <form id="formExport" action="CustExcelExport.aspx">
    <input type="hidden" name='CustName' value="" />
    <input type="hidden" name='Sexs' value="" />
    <input type="hidden" name='CallTime' value="" />
    <input type="hidden" name='CustTel' value="" />
    <input type="hidden" name='DealerName' value="" />
    <input type="hidden" name='BeginTime' value="" />
    <input type="hidden" name='EndTime' value="" />
    <input type="hidden" name='ProvinceID' value="" />
    <input type="hidden" name='CityID' value="" />
    <input type="hidden" name='CountyID' value="" />
    <input type="hidden" name='AreaIDs' value="" />
    <input type="hidden" name='DataSources' value="" />
    <input type="hidden" name='CityScopes' value="" />
    <input type="hidden" name='ConsultTypes' value="" />
    <input type="hidden" name='AgentNum' value="" />
    <input type="hidden" name='field' value="" />
    </form>
</asp:Content>
