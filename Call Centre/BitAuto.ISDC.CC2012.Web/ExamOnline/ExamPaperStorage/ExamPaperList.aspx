<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    Title="试卷库列表" CodeBehind="ExamPaperList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ExamPaperStorage.ExamPaperList" %>

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

            //试卷名称
            var PaperName = $("#txtPaperName").val();

            //分类
            var ECIDStr = "";
            var list = $(":input[name='paperCatage'][checked=true]");
            $(list).each(function () {
                ECIDStr = ECIDStr + $(this).val() + ",";
            });
            ECIDStr = ECIDStr.substring(0, ECIDStr.lastIndexOf(','));

            var ECID = $("#selCatage").val();

            //创建日期
            var BeginTime = $("#tfBeginTime").val();
            var EndTime = $("#tfEndTime").val();

            //状态
            var State = "";
            list = $(":input[name='paperState'][checked=true]");
            $(list).each(function () {
                State = State + $(this).val() + ",";
            });
            State = State.substring(0, State.lastIndexOf(','));

            //创建人
            var CreateUser = $("[id$=selCreateUser]").val();
            var bgidT = $('#<%=ddlBGIDs.ClientID %>').val();
            bgidT = bgidT == "-1" ? "" : bgidT;
            var pody = {
                PaperName: escape(PaperName),
                Catage: escape(ECID),
                BeginTime: escape(BeginTime),
                EndTime: escape(EndTime),
                State: escape(State),
                CreateUser: escape(CreateUser),
                bgid: escape(bgidT),
                R: Math.random()
            }
            return pody;
        }
        //查询
        function search() {

            //加载查询结果
            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/AjaxServers/ExamOnline/ExamPaperStorage/ExamPaperList.aspx", GetPody());
        }

        //加载分组
        function getUserGroup() {
            AjaxPostAsync("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetUserGroupByLoginUserID", ShowSelfGroup: false, r: Math.random() }, null, function (data) {
                $("#<%=ddlBGIDs.ClientID %>").append("<option value='-2'>请选所属分组</option>");
                var jsonData = $.evalJSON(data);
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        $("#<%=ddlBGIDs.ClientID %>").append("<option value=" + jsonData[i].BGID + ">" + jsonData[i].Name + "</option>");
                    }
                }
            });
        }


    </script>
    <style type="text/css">
        .ddlT {
            *width: 206px;
            width: 206px;            
        }
    </style>
    <form id="Form1" runat="server">
    <div class="searchTj" style="width: 100%;">
        <ul>
            <li>
                <label>
                    试卷名称：</label>
                <input type="text" id="txtPaperName" class="w200" />
            </li>
            <li>
                <label>
                    创建人：</label>
                <select id="selCreateUser" runat="server" class="w200" style='width: 206px;'>
                </select>
            </li>
            <li>
                <label>
                    状态：</label>
                <asp:Repeater ID="RptState" runat="server">
                    <ItemTemplate>
                        <input type="checkbox" name="paperState" style="border: none;" value='<%#Eval("value") %>' />
                        <em onclick="emChkIsChoose(this)">
                            <%#Eval("Name") %></em>
                    </ItemTemplate>
                </asp:Repeater>
            </li>
        </ul>
        <ul style="clear: both; width: 90%;">
            <li>
                <label>
                    所属分组：</label><span>
                        <asp:DropDownList runat="server" ID="ddlBGIDs" CssClass="ddlT" Style="border: #CCC 1px solid;">
                        </asp:DropDownList>
                    </span></li>
            <li>
                <label>
                    分类：</label>
                <select id="selCatage" class="w200" style='width: 206px;'>
                    <option value="-1">请选择</option>
                    <asp:Repeater ID="RptCatage" runat="server">
                        <ItemTemplate>
                            <option value="<%#Eval("ECID") %>">
                                <%#Eval("Name") %></option>
                        </ItemTemplate>
                    </asp:Repeater>
                </select>
            </li>
            <li>
                <label>
                    创建日期：</label>
                <input type="text" name="BeginTime" id="tfBeginTime" class="w95" />-<input type="text"
                    name="EndTime" id="tfEndTime" class="w95" />
            </li>
            <li class="btnsearch" style="float: right; clear: none; margin-top: 5px; width: 111px;
                text-align: left;">
                <input type="button" value="查 询" class="cx" onclick="search()" style="margin-left: 45px;"
                    name="" />
            </li>
        </ul>
    </div>
    </form>
    <div class="optionBtn clearfix">
        <%if (AddPaperButton)
          { %>
        <div class="new">
            <a href="/ExamOnline/ExamPaperStorage/ExamPaperEdit.aspx" style="background: url(/css/img/addPaper.png) scroll 0 0;"
                target="_blank"></a>
        </div>
        <%} %>
        <%if (AddQueryButton)
          {%>
        <div class="new">
            <a href="/KnowledgeLib/KnowledgeEdit.aspx" style="background: url(/css/img/addQustion.png) scroll 0 0;"
                target="_blank"></a>
        </div>
        <%}%>
    </div>
    <div class="bit_table" width="99%" cellspacing="0" cellpadding="0" id="ajaxTable">
    </div>
</asp:Content>
