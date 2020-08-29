<%@ Page Title="" Language="C#" MasterPageFile="~/Controls/Top.Master" AutoEventWireup="true"
    CodeBehind="ExamProjectList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ExamObject.ExamProjectList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
    <title>考试项目管理</title>
    <link rel="stylesheet" type="text/css" href="../../css/GooCalendar.css" />
    <script type="text/javascript" src="../../js/GooCalendar.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="searchTj" id="SearchCon" style="width: 100%;">
            <ul class="clearfix">
                <li>
                    <label>
                        项目名称：</label>
                    <input type="text" name="ObjName" id="ObjName" class="w200" />
                </li>
                <li>
                    <label>
                        考试时间：</label>
                    <input type="text" value="" id="calenStar1" class="w95" />-<input type="text" value=""
                        id="calenEnd1" class="w95" />
                </li>
                <li>
                    <label>
                        创建人：</label>
                    <select id="CreateUser" class="w200" style='width: 206px;'>
                        <option value="-1">请选择</option>
                        <asp:Repeater runat="server" ID="Rpt_CreateUser">
                            <ItemTemplate>
                                <option value='<%#Eval("UserID") %>'>
                                    <%#Eval("TrueName")%></option>
                            </ItemTemplate>
                        </asp:Repeater>
                    </select>
                </li>
            </ul>
            <ul style="clear: both; width: 98%;">
                <li>
                    <label>
                        所属分组：</label>
                    <select class="w200" style='width: 206px;' id="selArea">                       
                    </select>
                </li>
                <li>
                    <label>
                        状态：</label>
                    <span>
                        <input type="checkbox" name="State" value='1' style="border: none;" />
                        <em onclick='emChkIsChoose(this);'>未开始</em> </span><span>
                            <input type="checkbox" name="State" value='2' style="border: none;" />
                            <em onclick='emChkIsChoose(this);'>进行中</em> </span><span>
                                <input type="checkbox" name="State" value='3' style="border: none;" />
                                <em onclick='emChkIsChoose(this);'>已完成</em> </span></li>
                <li style="width: 350px;">
                    <label>
                        分类：</label>
                    <asp:Repeater runat="server" ID="Rpt_Cate">
                        <ItemTemplate>
                            <input type="checkbox" name="ExObjCate" style="border: none;" value='<%#Eval("ECID") %>' /><em
                                onclick='emChkIsChoose(this);'><%#Eval("Name") %></em>
                        </ItemTemplate>
                    </asp:Repeater>
                </li>
                <li class="btnsearch" style="margin-top: 5px; clear: none; width: 100px;">
                    <input class="cx" type="button" name="Search" value="查询" onclick="fnSearch()" style="float: right;" /></li>
            </ul>
        </div>
        <div id="ajaxTable">
        </div>
        <input type="hidden" id="pageHiddenMain" />
    </div>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
        getUserGroup();
            //敲回车键执行方法
            enterSearch(fnSearch);
            InitWdatePicker(2, ["calenStar1", "calenEnd1"]);
            //填充空格，以使checkbox对齐，按照3个字对齐，少一个字填充3个&nbsp;
            $("em").each(function () {
                var em_val = $.trim($(this).html());
                var add_nbsp = "";
                for (var i = em_val.length; i < 3; i++) {
                    add_nbsp += "&nbsp;&nbsp;&nbsp;";
                }
                $(this).html(em_val + add_nbsp);
            });

            fnSearch();
        });

        function fnSearch() {
            LoadingAnimation('ajaxTable');
            var pody = params();
            $('#ajaxTable').load('ExamProjectListAjax.aspx #ajaxTable', pody);
        }
        //分页操作
        function ShowDataByPost1(pody) {
            LoadingAnimation('ajaxTable');
            $('#ajaxTable').load('ExamProjectListAjax.aspx #ajaxTable', pody + '&random=' + Math.random());
        }

        function params() {
            var ObjName = $.trim($("#ObjName").val());
            var Group = "";
            var StartTime = $.trim($("#calenStar1").val());
            var EndTime = $.trim($("#calenEnd1").val());

            if (StartTime != '') {
                StartTime += ' 00:00:01';
            }
            if (EndTime != '') {
                EndTime += ' 23:59:59';
            }

            var CreateUserID = $("#CreateUser option:selected").val();
            var Cate = getCheckBoxVal('ExObjCate'); //得到checkbox的value值
            var State = getCheckBoxVal('State');
            var IsMakeUp = "";
            var currentPageNum = $("#pageHiddenMain").val();

            var selBGIDS = $("#selArea").val();

            var pody = 'ObjName=' + encodeURI(ObjName) + '&Group=' + encodeURI(Group)
                        + '&StartTime=' + StartTime + '&EndTime=' + EndTime
                        + '&CreateUserID=' + CreateUserID + '&Cate=' + Cate
                        + '&State=' + State + '&IsMakeUp=' + IsMakeUp
                        + '&page=' + currentPageNum
                        + '&BGIDS=' +selBGIDS 
                        + '&random=' + Math.round(Math.random() * 10000);
            return pody;
        }
        //添加试卷
        function OpenAddPageInfo() {
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/ExamOnline/ExamObject/ExamProjectEdit.aspx');
            }
            catch (e) {
                window.open('/ExamOnline/ExamObject/ExamProjectEdit.aspx', '', 'height=700,width=1050,left=200,toolbar=no,menubar=no,scrollbars=yes,resizable=no,location=no,status=no')
            }
        }
        function OpenAddPageInfo2(id) {
            try {
                var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/ExamOnline/ExamObject/ExamProjectEdit.aspx?id=' + id));
            }
            catch (e) {
                window.open('/ExamOnline/ExamObject/ExamProjectEdit.aspx?id=' + id, '', 'height=700,width=1050,left=200,toolbar=no,menubar=no,scrollbars=yes,resizable=no,location=no,status=no');
            }
        }
        function OpenAddPageInfo_(Str) {
            try {
                var dd = window.external.MethodScript("/browsercontrol/newpage?url=<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>" + escape(Str))
            }
            catch (e) {
                window.open(Str, '', 'height=700,width=1050,left=200,toolbar=no,menubar=no,scrollbars=yes,resizable=no,location=no,status=no');
            }
            
        }

        //加载分组
        function getUserGroup() {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", ShowSelfGroup: false, r: Math.random() }, null, function (data) {
                $("#selArea").append("<option value='-2'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#selArea").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }


        //删除考试项目
        function delExamInfo(thisObj, EIID, State) {

            var msg = "";
            if (State == 1)
            { msg = "确定要设为已完成吗?"; }
            else {
                msg = "确定删除考试项目吗？";
            }
            $.jConfirm(msg, function (r) {
                if (r) {
                    AjaxPostAsync('ExamInfoDel.ashx', { 'EIID': EIID, 'State': State },
                    function () { },
                    function (data) {
                        if (data == "success") {
                            //$.jAlert("操作成功！");
                            $.jPopMsgLayer("操作成功！");
                            
                            fnSearch();
                            if (State == -1)
                            { $(thisObj).parent().parent().remove(); }
                        }
                        else {
                            $.jAlert("操作失败。");
                        }
                    });
                }
            });
        }

    </script>
    <!--日历控件js-->
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
        var property3 = {
            divId: "calen2", //日历控件最外层DIV的ID
            needTime: false, //是否需要显示精确到秒的时间选择器，即输出时间中是否需要精确到小时：分：秒 默认为FALSE可不填
            //yearRange: [1970, 2030], //可选年份的范围,数组第一个为开始年份，第二个为结束年份,如[1970,2030],可不填
            week: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'], //数组，设定了周日至周六的显示格式,可不填
            month: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'], //数组，设定了12个月份的显示格式,可不填
            format: "yyyy-MM-dd"
            /*设定日期的输出格式,可不填*/
        };
        var property = {
            divId: "demo",
            needTime: true,
            fixid: "fff"
            /*决定了日历的显示方式，默认不填时为点击INPUT后出现，但如果填了此项，日历控件将始终显示在一个id为其值（如"fff"）的DIV中（且此DIV必须存在）*/
        };
        $(document).ready(function () {
            //$.createGooCalendar("calenStar1", property3);
            //$.createGooCalendar("calenEnd1", property3);
            //canva2 = $.createGooCalendar("calen2", property2);
            //canva2.setDate({ year: 2008, month: 11, day: 22, hour: 14, minute: 52, second: 45 });
        });
    </script>
</asp:Content>
