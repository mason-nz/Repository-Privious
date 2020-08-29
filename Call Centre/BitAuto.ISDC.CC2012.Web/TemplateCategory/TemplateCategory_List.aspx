<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="TemplateCategory_List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateCategory.TemplateCategory_List"
    Title="信息模板列表" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <div class="search" id="SearchCon">
        <ul class="clearfix">
            <li>
                <label>
                    模版类型：</label>
                <input type="radio" name="TemType" id="typeMail" value="2" onclick="typeChange()"
                    checked="checked" /><em onclick="emChkIsChoose(this)">邮件</em>
                <input type="radio" name="TemType" value="1" onclick="typeChange()" /><em onclick="emChkIsChoose(this)">
                    短信</em> </li>
            <li style="margin-right: 0px;">
                <label>
                    模版分类：</label>
                <select id="TypeSelect_1" onchange="loadTemplateCate(getSelectVal('TypeSelect_1', 'value'),'TypeSelect_2')"
                    class="w125">
                </select>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <select id="TypeSelect_2" onchange="hiddenInput()" class="w125">
                    <option value="-1">请选择</option>
                </select>
            </li>
            <li class="btnsearch" style="padding-top: 3px; margin-left: 5px;">
                <input type="button" name="Search" value="查询" onclick="search()" class="searchBtn bold" /></li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <input type="button" value="删除" onclick="delTemplate()" class="newBtn" style="*margin-top: 3px;" />
        <input type="button" onclick="addTemplate_()" value="添加模板" class="newBtn" style="*margin-top: 3px;" />&nbsp;&nbsp;
    </div>
    <div id="TemplateTable" style="width: 99%;">
        <table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="100%">
            <tr class="bold">
                <th style="width: 4%;">
                    <input type="checkbox" id="checkboxCon" onclick="checkboxCon()" />
                </th>
                <th style="width: 9%;">
                    模板类型
                </th>
                <th style="width: 15%;">
                    模板标题
                </th>
                <th style="width: 14%;">
                    模板分类
                </th>
                <th style="width: 15%;">
                    创建时间
                </th>
                <th style="width: 20%;">
                    邮件接收人
                </th>
                <th style="width: 14%;">
                    创建人
                </th>
                <th style="width: 9%;">
                    操作
                </th>
            </tr>
            <asp:Repeater runat="server" ID="Rpt_TempList">
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" name="checkName" value="<%#Eval("RecID")%>" />
                        </td>
                        <td>
                            <%#Eval("Type").ToString()=="1"?"短信":"邮件"%>
                        </td>
                        <td>
                            <%#Eval("Title")%>
                        </td>
                        <td>
                            <%#Eval("Name")%>
                        </td>
                        <td>
                            <%#Eval("CreateTimeB")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("EmailServers")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("CreateName")%>
                        </td>
                        <td>
                            <a onclick="updateTemplateSup(<%#Eval("RecID")%>)" style="cursor: pointer;">编辑 </a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        <div class="pages" style="float: right; width: 100%; text-align: right; margin-top: 10px;">
            <table style="width: 100%;">
                <tr>
                    <td style="text-align: left;">
                        &nbsp;&nbsp;
                    </td>
                    <td style="text-align: right;">
                        <p>
                            <asp:Literal runat="server" ID="litPage"></asp:Literal>
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        <div style="clear: both;">
            <input type="hidden" id="hiddenPage" />
        </div>
    </div>
    <script type="text/javascript">
        //弹出层-添加模板
        function addTemplate_() {
            $.openPopupLayer({
                name: "addTemplate",
                parameters: {},
                url: "TemplateCategory_Add.aspx",
                beforeClose: function (e, data) {
                }
            });
        }

        //弹出层修改模板
        function updateTemplateSup(RecID) {
            $.openPopupLayer({
                name: "updateTemplate",
                parameters: { "TemplateID": RecID },
                url: "TemplateCategory_Update.aspx",
                beforeClose: function (e, data) {
                }
            });
        }
        function checkboxCon() {
            if ($("#checkboxCon").attr("checked")) {
                $('[name=checkName]:checkbox').attr("checked", true);
            }
            else {
                $('[name=checkName]:checkbox').attr("checked", false);
            }
        }

        function delTemplate() {
            var checkedS = $(":checkbox[name^='checkName'][checked=true]");
            if (checkedS.length == 0) {
                $.jAlert('请至少选择一个删除项！');
                return;
            }
            else { }

            $.jConfirm('是否确定删除选中模版？', function (r) {
                if (r) {
                    delTemplate2();
                }
            });
        }
        function delTemplate2() {
            var checkedS = $(":checkbox[name^='checkName'][checked=true]");
            var templateIDs = "";
            checkedS.each(function () {
                templateIDs += $(this).val() + ",";
            });
            podyTemplate = { TemplateIDs: templateIDs };
            AjaxPost('delTemplate.ashx', podyTemplate, null, function (data) {
                if (data == "您没有执行此操作的权限") {
                    $.jAlert(data);
                    return;
                }
                else {
                    LoadingAnimation('TemplateTable');
                    search();
                }
            });
        }
        //分页操作
        function ShowDataByPost1(pody) {
            $('#TemplateTable').load('TemplateCategory_List.aspx #TemplateTable', pody);
            $('#hiddenPage').val(pody);
        }



        $(document).ready(function () {
            //敲回车键执行方法
            enterSearch(search);

            typeChange();
            search();
        });
        //加载下拉框
        function loadTemplateCate(lTPID, lTsub) {
            //alert($(":checkbox[name^='type2'][checked=true]"));
            //alert();

            var type = $("input[name='TemType']:checked").val();
            $('#' + lTsub + ' option').remove();
            $('#' + lTsub).append('<option value="-1">请选择</option>');
            if (lTPID != '-1') {
                //alert(type +'-'+ lTPID);
                AjaxPostAsync("getTemplateCategroys.ashx", { Type: type, PID: lTPID }, null, function (data) {
                    if (data != '') {
                        var jsonData = $.evalJSON(data);
                        $.each(jsonData.root, function (idx, item) {
                            $('#' + lTsub).append("<option value='" + item.RecID + "'>" + item.Name + "</option>")
                        });
                        if (lTsub == "TypeSelect_2") {
                            $('#TypeSelect_2').css('display', 'inline');
                        }
                        $('#hiddenFile').val("-1");
                    }
                    else {//没有二级 
                        //$('#hiddenFile').val($('#TypeSelect_1').val());
                        //$('#TypeSelect_2').css('display', 'none');
                    }
                });
            }
            else {
                //$('#TypeSelect_2').css('display', 'none');
            }
        }

        //切换type
        function typeChange() {
            loadTemplateCate(0, "TypeSelect_1");
            $('#TypeSelect_2 option').remove();
            $('#TypeSelect_2').append('<option value="-1">请选择</option>');
        }
        //获取select选中的value
        function getSelectVal(gSselectID, gStype) {
            //元素验证            
            if (gStype == 'value') {
                return $('#' + gSselectID).val();
            }
        }

        //搜索
        function search() {
            LoadingAnimation('TemplateTable');
            $('#TemplateTable').load('TemplateCategory_List.aspx #TemplateTable', params(), function () {
            //如果查询的是短信类型，则隐藏table列：邮件联系人
                if ($(":radio[name='TemType']:checked").val() == "1") {
                    $("#TemplateTable table tr:eq(0) th:eq(5)").hide();
                    $("#TemplateTable table tbody tr").each(function () {
                        $(this).find("td:eq(5)").hide();
                    });
                }
            });


        }
        function params() {
            var type_ = $(':radio[name="TemType"]:checked').val();
            var class1_ = $.trim($("#TypeSelect_1").val());
            var class2_ = $.trim($("#TypeSelect_2").val());
            var pody = 'type=' + type_ + '&class1=' + class1_ + '&class2=' + class2_;
            return pody + '&r=' + Math.random();
        }
    </script>
</asp:Content>
