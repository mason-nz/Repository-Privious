<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="ExamScoreList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ExamOnline.ExamScoreManagement.ExamScoreList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head1" runat="server">
    <title>考试成绩管理</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            getUserGroup();
            //敲回车键执行方法
            enterSearch(search);
            InitWdatePicker(2, ["tfBeginTime", "tfEndTime"]);
            search();
        });
        function GetPody() {
            //项目名称
            var ExamProjectName = $("#txtExamProjectName").val();
            //试卷名称
            var PaperName = $("#txtPaperName").val();
            //分类
            var Catage = "";
            var list = $(":input[name='paperCatage'][checked=true]");
            $(list).each(function () {
                Catage = Catage + $(this).val() + ",";
            });
            Catage = Catage.substring(0, Catage.lastIndexOf(','));
            //考试时间
            var BeginTime = $("#tfBeginTime").val();
            var EndTime = $("#tfEndTime").val();
            //考生姓名
            var ExamPerson = $("#txtExamPerson").val();
            //所属分组
            var BGID = $("#EGroup").val();

            var pody = {
                ExamProjectName: escape(ExamProjectName),
                PaperName: escape(PaperName),
                Catage: escape(Catage),
                BeginTime: escape(BeginTime),
                EndTime: escape(EndTime),
                ExamPerson: escape(ExamPerson),
                BGID: BGID,
                R: Math.random()
            }
            return pody;
        }
        //查询
        function search() {
            //加载查询结果
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/ExamOnline/ExamScoreManage/ExamScoreList.aspx", GetPody());
        }

        function Export() {
            //项目名称
            var ExamProjectName = $("#txtExamProjectName").val();
            //试卷名称
            var PaperName = $("#txtPaperName").val();
            //分类
            var Catage = "";
            var list = $(":input[name='paperCatage'][checked=true]");
            $(list).each(function () {
                Catage = Catage + $(this).val() + ",";
            });
            Catage = Catage.substring(0, Catage.lastIndexOf(','));
            //考试时间
            var BeginTime = $("#tfBeginTime").val();
            var EndTime = $("#tfEndTime").val();
            //考生姓名
            var ExamPerson = $("#txtExamPerson").val();
            //所属分组
            var BGID = $("#EGroup").val();

            $("#formExport [name='ExamProjectName']").val(escapeStr(ExamProjectName));
            $("#formExport [name='PaperName']").val(escapeStr(PaperName));
            $("#formExport [name='Catage']").val(escapeStr(Catage));
            $("#formExport [name='BeginTime']").val(escapeStr(BeginTime));
            $("#formExport [name='EndTime']").val(escapeStr(EndTime));
            $("#formExport [name='ExamPerson']").val(escapeStr(ExamPerson));
            $("#formExport [name='BGID']").val(escapeStr(BGID));
            $("#formExport").submit();
        }
        function ViewExamScore(eiid, Type, come, ExamPersonID, Epid) {
            window.open("/ExamOnline/ExamScoreManagement/MarkExamPaper.aspx?eiid=" + eiid + "&type=" + Type + "&come=" + come + "&ExamPersonID=" + ExamPersonID + "&epid=" + Epid + "");
        }

        //加载分组
        function getUserGroup() {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", ShowSelfGroup: false, r: Math.random() }, null, function (data) {
                $("#EGroup").append("<option value='-2'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#EGroup").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
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
    </script>
    <div class="searchTj" style="width: 100%;">
        <ul class="clear">
            <li>
                <label>
                    项目名称：</label>
                <input type="text" id="txtExamProjectName" class="w200" />
            </li>
            <li>
                <label>
                    考试时间：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" class="w95" />-<input type="text"
                    name="EndTime" id="tfEndTime" class="w95" />
            </li>
            <li style="width: 380px;">
                <label>
                    分类：</label>
                <asp:Repeater ID="RptCatage" runat="server">
                    <ItemTemplate>
                        <span>
                            <input type="checkbox" name="paperCatage" style="border: none;" value='<%#Eval("ECID") %>' /><em
                                onclick="emChkIsChoose(this)"><%#Eval("Name") %></em></span>
                    </ItemTemplate>
                </asp:Repeater>
            </li>
        </ul>
        <ul style="clear: both;">
            <li>
                <label>
                    试卷名称：</label>
                <input type="text" id="txtPaperName" class="w200" />
            </li>
            <li>
                <label>
                    考生姓名：</label>
                <input type="text" id="txtExamPerson" class="w200" />
            </li>
            <li>
                <label>
                    所属分组：
                </label>
                <select id="EGroup" class="w200" style='width: 206px;'>
                </select>
            </li>
            <li class="btnsearch" style="float: left; clear: none; margin-top: 5px; width: 150px;">
                <input type="button" value="查 询" id="btnSearch" class="cx" style=" margin-left:20px;" onclick="search()" />
            </li>  
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <%if (ExportButton)
          { %><input name="" type="button" value="导出" onclick="Export()" class="newBtn" /><%} %>
    </div>
    <div class="bit_table" width="99%" cellspacing="0" cellpadding="0" id="ajaxTable">
    </div>
    <form id="formExport" action="ExamScoreExport.aspx">
    <input type="hidden" name='ExamProjectName' value="" />
    <input type="hidden" name='PaperName' value="" />
    <input type="hidden" name='Catage' value="" />
    <input type="hidden" name='BeginTime' value="" />
    <input type="hidden" name='EndTime' value="" />
    <input type="hidden" name='ExamPerson' value="" />
    <input type="hidden" name='BGID' value="" />
    </form>
</asp:Content>
