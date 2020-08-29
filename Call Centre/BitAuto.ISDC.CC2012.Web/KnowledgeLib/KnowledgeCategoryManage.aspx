<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KnowledgeCategoryManage.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.KnowledgeCategoryManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(function () {
            laySelKCIDChange(0);
            $("#layKnowledgekeywords").focus(function () {
                if ($("#layKnowledgekeywords").val() == "请输入关键字查询") {
                    $("#layKnowledgekeywords").val('');
                }
                $("#layKnowledgekeywords").css({ 'color': '#000', 'background': '#FFFFCC' });
            });

            $("#layKnowledgekeywords").blur(function () {
                if ($.trim($("#layKnowledgekeywords").val()) == "") {
                    $("#layKnowledgekeywords").val('请输入关键字查询');
                }
                $("#layKnowledgekeywords").css({ 'background': 'url("../CSS/img/inputbg01.jpg") repeat-x scroll 0 0 transparent', 'color': '#CCCCCC' }); //background: 'url("../img/inputbg01.jpg")
            });
        });
        //上移或下移
        function SortNumUpOrDown(sortid, type) {
            var inpsortnum_v = $("#inpsortnum").val();
            $.get("../AjaxServers/KnowledgeLib/KnowledgeCategoryHandler.ashx",
            {
                Action: 'sortnumupordown',
                SortId: sortid,
                SortType: type,
                SortInfo: inpsortnum_v,
                r: Math.random()
            },
            function (data) {
                laySearchData();
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pop pb15 openwindow" style="width: 600px">
        <div class="title bold">
            <h2>
                分类管理</h2>
            <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('addCategory',false);">
            </a></span>
        </div>
        <ul class="clearfix ft14" id="addTemplatePage" style="margin: 0px; padding: 0px;
            margin-right: 25px; margin-left: 20px;">
            <li style="width: 150px; margin: 0px; padding: 0px; ">
                <span style="vertical-align:middle">分类：</span>
                <span>
                    <select id="layerSelKCID1" class="w100" onchange="laySelKCIDChange(1)" style="vertical-align:middle">
                        <option value='-1'>请选择</option>
                    </select>
                </span>      
             </li>
             <li style="width: 220px; margin: 0px; padding: 0px; ">
                <span class="keywords">
                    <input style="vertical-align:middle" type="text" value="请输入关键字查询" class="w200" name="layKnowledgekeywords" id="layKnowledgekeywords" />
                </span>
             </li>
             <li style="width: 180px; margin: 0px; padding: 0px; ">
                <span class="btnsearch">
                    <input type="button" onclick="laySearchData();" value="搜 索" />
                </span>
                <span class="btnsearch">
                    <input type="button" value="新 增" onclick="layAddNewRow()" />
                </span>
              </li>
        </ul>
        <div class="Table2" id="divQueryCategoryList" style="margin-left: 20px; width: 550px;
            text-align: center">
            <table border="1" cellpadding="0" cellspacing="0" class="Table2List" id="tableQueryCategory"
                width="100%">
                <tbody id="trList">
                    <tr class="bold">
                        <th style="color: Black; font-weight: bold; width: 20%">
                            所属分类
                        </th>
                        <th style="color: Black; font-weight: bold; width: 20%">
                            分类名称
                        </th>
                        <th style="color: Black; font-weight: bold; width: 10%">
                            状态
                        </th>
                        <th style="color: Black; font-weight: bold; width: 30%">
                            操作
                        </th>
                        <th style="color: Black; font-weight: bold; width: 20%">
                            顺序
                        </th>
                    </tr>
                </tbody>
            </table>
        </div>
        <ul class="clearfix" style="text-align: center;" id="btnSumbit">
            <li>
                <input type="button" onclick="javascript:$.closePopupLayer('addCategory',false);"
                    class="btnChoose" value="关闭页面" /></li>
        </ul>
        <input type="hidden" value="" id="inpsortnum" />
    </div>
    </form>
</body>
</html>
