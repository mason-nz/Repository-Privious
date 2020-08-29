<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TagManagementPop.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.TemplateManagement.TagManagementPop"
    EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <%--    <link href="/css/base.css" type="text/css" rel="stylesheet" />
    <link href="/css/style.css" type="text/css" rel="stylesheet" />
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../../Js/common.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        var tp = {};
        tp.SearchGroup = function () {
            var cid = $('#sltRegion').val();
            if (cid == -1) {
                cid = 0;
            }
            var status = $('#ckAvalibile0').is(':checked') ? "0" : "";
            status = $('#ckAvalibile1').is(':checked') ? (status == "0" ? "0,1" : "1") : (status == "0" ? "0" : "");
            var pody = 'bgid=' + tp.BGID + '&c=' + cid + '&r=' + Math.random() + "&s=" + status;

            $('#divList').load('../AjaxServers/TemplateManagement/TagManagementPop.aspx #divList > *', pody, function () { });
            //            $('#divList').load('TagManagementPop.aspx #divList > *', pody, function () { });
        };

        function addNewRow() {
            var tr = [];
            var cId = $('#sltRegion').val();
            cId = cId == -1 ? 0 : cId;
            var cName = cId == 0 ? "" : $.trim($('#sltRegion').find('option:selected').text());

            tr.push('<tr class="back" onmouseover="this.className=\'hover\'" onmouseout="this.className=\'back\'" did="-1" cid="');
            tr.push(cId + '"  >');
            tr.push('<td><em>');
            tr.push(cName);
            tr.push('</em></td>');
            tr.push('<td><span class="tagname"></span> <span class="inputname" ><input type="text" class="vInput"  /></span></td>');
            tr.push('<td class="">在用</td>');
            tr.push('<td><span class="tagname" style="display: none;"><a href="javascript:void(0);" onclick="tp.EditTr(this);">编辑</a><a href="javascript:void(0);" onclick="tp.delTr(this);"> 删除</a> <a href="javascript:void(0);" onclick="tp.stopTr(this);" class="stopT">停用</a> </span>');
            tr.push('<span class="inputname" ><a href="javascript:void(0);" onclick="tp.saveTr(this);">保存</a> <a href="javascript:void(0);" onclick="tp.cancelTr(this);">取消</a></span>');
            tr.push('</td>');
            tr.push('<td>');
            tr.push('<span class="tagname" style="display: none;"><a href="javascript:void(0);" onclick="tp.upTr(this);">上移</a><a href="javascript:void(0);" onclick="tp.downTr(this);">下移</a></span> <span class="inputname" ></span>');
            tr.push('</td>');
            tr.push('</tr>');
            tr.push('');
            $('#trList').append(tr.join(' '));
        }


        //1 显示编辑框，隐藏文本；2 隐藏编辑、删除按钮，显示保存、取消按钮
        tp.EditTr = function (obj) {
            var tr$ = $(obj).closest("tr");
            tr$.find('.inputname').show().siblings().hide();
        };
        tp.cancelTr = function (obj) {
            var tr$ = $(obj).closest("tr");
            var did = $.trim(tr$.attr("did"));
            if (did == "-1") {
                tr$.remove();
                return;
            }
            tr$.find('.tagname').show().siblings().hide();
        };
        tp.saveTr = function (obj) {
            var this$ = $(obj);
            var tr$ = $(obj).closest("tr");
            var MaxLent = 10; // $('#sltRegion').val() == -1 ? 4 : 10;
            var cn = tr$.find('.vInput').val();
            if (cn.length == 0) {
                $.jAlert("内容不能为空.");
                return;
            }
            if (cn.length > MaxLent) {
                $.jAlert("内容长度最大为:" + MaxLent);
                return;
            }
            var body = {};
            var did = tr$.attr('did');
            if (did == "-1") {
                body = { action: "InsertNewTag", BGID: tp.BGID, TagID: did, pid: $.trim(tr$.attr('cid')), tagName: cn, r: Math.random() };
            } else {
                body = { action: "ChangeName", TagID: tr$.attr('did'), tagName: cn, r: Math.random() };
            }

            tp.SendReqest(body, function (data) {
                if (did == "-1") {
                    tr$.attr("did", data);
                }
                tr$.find('td:eq(1) .tagname').html(cn);
                tr$.find('.tagname').show().siblings().hide();
            });
        };
        tp.delTr = function (obj) {
            $.jConfirm("你确定要删除吗？", function (tc) {
                if (tc) {
                    var this$ = $(obj);
                    var tr$ = $(obj).closest("tr");
                    var body = { action: "DeleTag", TagID: tr$.attr('did'), r: Math.random() };
                    tp.SendReqest(body, function () {
                        this$.closest('td').prev().html("删除");
                        this$.html('').next().html('');
                    });
                }
            });

        };

        tp.stopTr = function (obj) {
            var this$ = $(obj);
            var tr$ = $(obj).closest("tr");
            var sta = 1;
            if ($.trim(this$.html()) == "停用") {
                sta = 1;
            } else if ($.trim(this$.html()) == "启用") {
                sta = 0;
            } else {
                sta = -1; //删除
            }
            var body = { action: "ChangeValidate", TagID: tr$.attr('did'), status: sta, r: Math.random() };
            tp.SendReqest(body, function () {
                if (sta == 1) {
                    this$.html("启用");
                    this$.closest('td').prev().html("停用");
                } else {
                    this$.html("停用");
                    this$.closest('td').prev().html("在用");

                }
            });
        };
        tp.upTr = function (obj) {
            var status = $('#ckAvalibile0').is(':checked') ? "0" : "";
            status = $('#ckAvalibile1').is(':checked') ? (status == "0" ? "0,1" : "1") : (status == "0" ? "0" : "");
            var tr$ = $(obj).closest("tr");
            if (!tr$.prev().hasClass('back')) {
                return;
            }
            var tid = tr$.attr('did');

            if (tid == null) {
                $.jAlert("参数错误，未找到ID");
                return;
            }

            var body = { action: "ChangeOrder", TagID: tid, isUp: 1, status: status, r: Math.random() };
            tp.SendReqest(body, function () {
                //tr$.slideUp(1000).insertBefore(tr$.prev()).slideDown(1000);
                tr$.hide(200, function () { tr$.insertBefore(tr$.prev()).show(500); });
            });
        };
        tp.downTr = function (obj) {
            var status = $('#ckAvalibile0').is(':checked') ? "0" : "";
            status = $('#ckAvalibile1').is(':checked') ? (status == "0" ? "0,1" : "1") : (status == "0" ? "0" : "");
            var tr$ = $(obj).closest("tr");
            if (tr$.next().length == 0) {
                return;
            }
            var tid = tr$.attr('did');

            if (tid == null) {
                $.jAlert("参数错误，未找到ID");
                return;
            }
            var body = { action: "ChangeOrder", TagID: tid, isUp: 0, status: status, r: Math.random() };
            tp.SendReqest(body, function () {
                //                tr$.hide(300, function () { tr$.insertAfter(tr$.next()).show(500); }); //.slideDown(1000);
                tr$.next().hide(100, function () {
                    tr$.insertAfter(tr$.next()); tr$.prev().show(500);
                });

            });
        };
        tp.SendReqest = function (body, callback) {

            if (body.TagID == null) {
                $.jAlert("参数错误，未找到ID");
                return;
            }
            AjaxPostAsync("../AjaxServers/TemplateManagement/TagEdit.ashx", body, null, function (data) {
                if (data == "ok" || tp.isNum(data)) {
                    if (callback) {
                        callback(data);
                    }
                } else {
                    $.jAlert(data);
                }

            });
        }

        tp.isNum = function (s) {
            var pattern = /^[0-9]*$/;
            if (pattern.test(s)) {
                return true;
            }
            return false;
        };


        $(function () {
            $('#sltRegion').change(tp.SearchGroup);
        });
        tp.BGID = '<%=BGID %>';
        top.UserId = '<%=userID %>';
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pop pb15 openwindow" style="width: 600px">
        <div class="title bold">
            <h2>
                标签管理</h2>
            <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('EditWorkTag2',false);">
            </a></span>
        </div>
        <div class="infor_li renyuan_cx">
            <ul class="clearfix  outTable">
                <li style="width: 220px;">
                    <label style="width: 60px;" for="sltRegion">
                        标签分类：
                    </label>
                    <select id="sltRegion" class="defselect" style="width: 150px;">
                        <option value="-1">请选择</option>
                        <asp:Repeater runat="server" ID="rptCatalog">
                            <ItemTemplate>
                                <option value="<%#Eval("id") %>">
                                    <%#Eval("TagName")%></option>
                            </ItemTemplate>
                        </asp:Repeater>
                    </select>
                </li>
                <li style="width: 180px;">
                    <label style="width: 60px;">
                        状态：
                    </label>
                    <span>
                    <input type="checkbox" name="chkStatus" id="ckAvalibile0" value="0" checked="checked"
                        onclick="javascript:tp.SearchGroup();" /><em>在用</em> </span>
                    <span> <em><input type="checkbox" name="chkStatus" id="ckAvalibile1" value="1" onclick="javascript:tp.SearchGroup();" />停用</em></span>
                </li>
                <li class="btn">
                    <input type="button" value="查询" class="btnSave bold" onclick="javascript:tp.SearchGroup();" />
                </li>
            </ul>
        </div>
        <div class="Table2" id="divList" style="margin: 10px 0px 0px 20px; width: 580px;
            text-align: center">
            <table border="1" cellpadding="0" cellspacing="0" class="Table2List" id="tableQueryGroup"
                width="98%">
                <tbody id="trList">
                    <tr class="bold">
                        <th style="color: Black; font-weight: bold; width: 30%">
                            所属分类
                        </th>
                        <th style="color: Black; font-weight: bold; width: 30%">
                            标签名称
                        </th>
                        <th style="color: Black; font-weight: bold; width: 10%">
                            状态
                        </th>
                        <th style="color: Black; font-weight: bold; width: 20%">
                            操作
                        </th>
                        <th style="color: Black; font-weight: bold; width: 10%">
                            顺序
                        </th>
                    </tr>
                    <asp:Repeater ID="rptTags" runat="server">
                        <ItemTemplate>
                            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'"
                                did="<%#Eval("TagID")%> ">
                                <td>
                                    <em>
                                        <%#Eval("pName")%>
                                </td>
                                <td>
                                    <span class="tagname">
                                        <%#Eval("TagName")%></span> <span class="inputname" style="display: none;">
                                            <input type="text" class="vInput" value="<%#Eval("TagName")%>" />
                                        </span>
                                </td>
                                <td class="">
                                    <%#Eval("IsUsed")%>
                                </td>
                                <td>
                                    <span class="tagname"><a href="javascript:void(0);" onclick="tp.EditTr(this);">编辑</a>
                                        <a href="javascript:void(0);" onclick="tp.delTr(this);">
                                            <%#GetDelTitle(Eval("Status").ToString())%></a> <a href="javascript:void(0);" onclick="tp.stopTr(this);"
                                                class="stopT">
                                                <%#GetZYTitle(Eval("Status").ToString())%></a> </span><span class="inputname" style="display: none;">
                                                    <a href="javascript:void(0);" onclick="tp.saveTr(this);">保存</a> <a href="javascript:void(0);"
                                                        onclick="tp.cancelTr(this);">取消</a></span>
                                </td>
                                <td>
                                    <span class="tagname"><a href="javascript:void(0);" onclick="tp.upTr(this);">上移</a>
                                        <a href="javascript:void(0);" onclick="tp.downTr(this);">下移</a></span> <span class="inputname"
                                            style="display: none;"></span>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        <ul class="clearfix" style="text-align: center;" id="btnSumbit">
            <li>
                <div class="btn" style="width: auto">
                    <input type="button" value="新增" onclick="addNewRow()" class="btnSave bold" />&nbsp;&nbsp;&nbsp;
                    <input type="button" class="btnSave bold" onclick="javascript:$.closePopupLayer('EditWorkTag2',false);"
                        class="btnChoose" value="关闭页面" />
                </div>
            </li>
        </ul>
    </div>
    </form>
</body>
</html>
