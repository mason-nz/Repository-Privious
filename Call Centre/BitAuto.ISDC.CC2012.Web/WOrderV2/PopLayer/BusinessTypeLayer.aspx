<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusinessTypeLayer.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer.BusinessTypeLayer" EnableViewState="false" %>

<script type="text/javascript">

    var tp = {};
    tp.SearchGroup = function () {

        var status = "";
        if ($('#ckAvalibile01').is(':checked')) {
            status += $('#ckAvalibile01').attr("value") + ",";
        }

        if ($('#ckAvalibile11').is(':checked')) {
            status += $('#ckAvalibile11').attr("value") + ",";
        }

        if (status.length > 0) {
            status = status.substring(0, status.length - 1);
        }
        var pody = "status=" + status + "&r=" + Math.random();

        $('#divList').load('PopLayer/BusinessTypeLayer.aspx #divList > *', pody, function () { });

    };

    function addNewRow() {
        ;
        var count = $('#tabList tr').length;
        var tr = [];
        var cId = "";

        tr.push('<tr dataid="-1" datasort=' + count + ' datastatus="1" >');

        tr.push('<td><span class="tagname"></span> <span class="inputname" ><input type="text" class="vInput w150"  /></span></td>');
        tr.push('<td class="">在用</td>');
        tr.push('<td>');
        tr.push('<span class="tagname" style="display: none;"><a href="javascript:void(0);" onclick="tp.upTr(this);">上移</a><a href="javascript:void(0);" onclick="tp.downTr(this);">下移</a></span> <span class="inputname" ></span>');
        tr.push('</td>');

        tr.push('<td><span class="tagname" style="display: none;"><a href="javascript:void(0);" onclick="tp.EditTr(this);">编辑</a><a href="javascript:void(0);" onclick="tp.delTr(this);"> 删除</a> <a href="javascript:void(0);" onclick="tp.stopTr(this);" class="stopT">停用</a> </span>');
        tr.push('<span class="inputname" ><a href="javascript:void(0);" onclick="tp.saveTr(this);">保存</a> <a href="javascript:void(0);" onclick="tp.cancelTr(this);">取消</a></span>');
        tr.push('</td>');

        tr.push('</tr>');
        tr.push('');
        $('#tabList').append(tr.join(' '));
        $('#divList').scrollTop($('#divList')[0].scrollHeight);
    }
    tp.SaveAll = function () {
        $('#tabList tr').each(function () {
            if ($(this).attr("dataid")) {
                tp.saveTr($(this).children()[0]);
            }
        });

    }

    //1 显示编辑框，隐藏文本；2 隐藏编辑、删除按钮，显示保存、取消按钮
    tp.EditTr = function (obj) {
        var tr$ = $(obj).closest("tr");
        tr$.find('.inputname').show().siblings().hide();
    };
    tp.cancelTr = function (obj) {
        var tr$ = $(obj).closest("tr");
        var dataid = $.trim(tr$.attr("dataid"));
        if (dataid == "-1") {
            tr$.remove();
            return;
        }
        tr$.find('.tagname').show().siblings().hide();
    };
    tp.saveTr = function (obj) {
        ;
        var MaxLent = 15;

        var this$ = $(obj);
        var tr$ = $(obj).closest("tr");
        var trindex = tr$.index();

        var dataid = tr$.attr('dataid');
        var currDataSort = tr$.attr("datasort");
        var cn = tr$.find('.vInput').val();

        if (cn.length == 0) {
            $.jAlert("第" + trindex + "行内容不能为空！");
            return;
        }
        if (cn.length > MaxLent) {
            $.jAlert("第" + trindex + "行内容长度不能超过:" + MaxLent + "个字！");
            return;
        }

        var body = {};

        if (dataid == "-1") {
            body = { action: "InsertNewTag", currid: dataid, tagname: cn, currsort: currDataSort, r: Math.random() };
        } else {
            body = { action: "ChangeName", currid: tr$.attr('dataid'), tagname: cn, currsort: currDataSort, r: Math.random() };
        }
        ;
        tp.SendReqest(body, function (data) {
            if (dataid == "-1") {
                tr$.attr("dataid", data);
            }
            tr$.find('td:eq(0) .tagname').html(cn);
            tr$.find('.tagname').show().siblings().hide();
        });
    };
    tp.delTr = function (obj) {
        $.jConfirm("你确定要删除吗？", function (tc) {
            if (tc) {
                var this$ = $(obj);
                var tr$ = $(obj).closest("tr");
                var body = { action: "DeleTag", currid: tr$.attr('dataid'), r: Math.random() };
                tp.SendReqest(body, function () {
                    tr$.remove();
                    // tr$.find('td:eq(1)').html(GetStatus("-1", "1"));
                    // tr$.find('td:eq(2)').html(" ");
                    // tr$.find('td:eq(3)').html(" ");
                });
            }
        });

    };

    tp.stopTr = function (obj) {
        var this$ = $(obj);
        var tr$ = $(obj).closest("tr");
        var sta = tr$.attr("datastatus");
        if (sta == "1") {
            sta = "0";
        }
        else {
            sta = "1";
        }

        var body = { action: "ChangeStatus", currid: tr$.attr('dataid'), status: sta, r: Math.random() };
        tp.SendReqest(body, function () {
            tr$.attr("datastatus", sta);
            this$.html(GetStatus(sta, "2"));
            tr$.find('td:eq(1)').html(GetStatus(sta, "1"));

            tp.SearchGroup();
        });


    };
    tp.upTr = function (obj) {

        var currTr = $(obj).closest("tr"); //当前行
        var nextTr = $(currTr).prev(); //上一行

        var currDataId = currTr.attr("dataId"); //数据id
        var currDataSort = currTr.attr("datasort");
        var nextDataId = nextTr.attr("dataId"); //数据id
        var nextDataSort = nextTr.attr("datasort");

        if (!currDataId || !nextDataId) {
            $.jAlert("已经是第一行了");
            return;
        }
        currTr.hide(200, function () { currTr.insertBefore(nextTr).show(500); });
        currTr.attr("datasort", nextDataSort);
        nextTr.attr("datasort", currDataSort);
        //  var body = { action: "ChangeOrder", currid: currDataId, currsort: currDataSort, nextid: nextDataId, nextsort: nextDataSort, r: Math.random() };
        //  tp.SendReqest(body, function () {
        //      currTr.hide(200, function () { currTr.insertBefore(nextTr).show(500); });
        //      currTr.attr("datasort", nextDataSort);
        //      nextTr.attr("datasort", currDataSort);
        //  });
    };
    tp.downTr = function (obj) {

        var currTr = $(obj).closest("tr"); //当前行
        var nextTr = $(currTr).next(); //上一行

        var currDataId = currTr.attr("dataId"); //数据id
        var currDataSort = currTr.attr("datasort");
        var nextDataId = nextTr.attr("dataId"); //数据id
        var nextDataSort = nextTr.attr("datasort");

        if (!currDataId || !nextDataId) {
            $.jAlert("已经是最后一行了");
            return;
        }
        currTr.hide(200, function () { currTr.insertAfter(nextTr).show(500); });
        currTr.attr("datasort", nextDataSort);
        nextTr.attr("datasort", currDataSort);
        // var body = { action: "ChangeOrder", currid: currDataId, currsort: currDataSort, nextid: nextDataId, nextsort: nextDataSort, r: Math.random() };
        // tp.SendReqest(body, function () {
        //     currTr.hide(200, function () { currTr.insertAfter(nextTr).show(500); });
        //     currTr.attr("datasort", nextDataSort);
        //     nextTr.attr("datasort", currDataSort);
        // });
    };

    tp.SendReqest = function (body, callback) {

        AjaxPostAsync("/WOrderV2/Handler/BusinessTypeHandler.ashx", body, null, function (data) {
            if (data) {
                data = JSON.parse(data);
                if (data.Success == true) {
                    if (callback) {
                        callback(data.Data);
                    }
                } else {
                    $.jAlert(data.Message);
                }
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

    function GetStatus(status, type) {
        var str = "";
        if (type == "1") {
            if (status == "1") {
                str = "在用";
            }
            else if (status == "0") {
                str = "停用";
            } else if (status == "-1") {
                str = "已删除";
            }
        }
        else if (type = "2") {
            if (status == "1") {
                str = "停用";
            }
            else if (status == "0") {
                str = "启用";
            }
        }

        return str;

    }
    $(function () {
        tp.SearchGroup();

    });
</script>
<div class="pop pb15 openwindow" style="width: 600px;">
    <div class="title bold">
        <h2>
            业务类型管理</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('BusinessTypeLayer',false);">
        </a></span>
    </div>
    <ul class="clearfix  outTable" style="width: 98%;">
        <li style="width: 230px; float: left;" class="name1">
            <label>
                状态：</label>
            <input type="checkbox" name="chkStatus" id="ckAvalibile01" value="1" checked="checked"
                onclick="javascript:tp.SearchGroup();" />在用
            <input type="checkbox" name="chkStatus" id="ckAvalibile11" value="0" onclick="javascript:tp.SearchGroup();" />停用
        </li>
    </ul>
    <div id="divList" class="Table2" style="height: 260px; overflow-y: auto; overflow-x: hidden;">
        <table id="tabList" border="0" cellpadding="0" cellspacing="0" class="Table2List"
            width="98%">
            <tr>
                <th width="35%">
                    业务类型
                </th>
                <th width="15%">
                    状态
                </th>
                <th width="25%">
                    顺序
                </th>
                <th width="25%">
                    操作
                </th>
            </tr>
            <asp:repeater id="rpt" runat="server">
                    <ItemTemplate>
                        <tr dataid="<%#Eval("RecID")%> " datasort="<%#Container.ItemIndex+1%>" datastatus="<%#Eval("Status")%>">
                            <td>
                                <span class="tagname">
                                    <%#Eval("BusiTypeName")%></span> <span class="inputname" style="display: none;">
                                        <input type="text" class="vInput w150" value="<%#Eval("BusiTypeName")%>" />
                                    </span>
                            </td>
                            <td>
                                <%#GetZYTitle(Eval("Status").ToString(),"1")%>
                            </td>
                            <td>
                                <span class="tagname"><a href="javascript:void(0);" onclick="tp.upTr(this);">上移</a>
                                    <a href="javascript:void(0);" onclick="tp.downTr(this);">下移</a> </span><span class="inputname"
                                        style="display: none;"></span>
                            </td>
                            <td>
                                <span class="tagname"><a href="javascript:void(0);" onclick="tp.EditTr(this);">编辑</a>
                                    <a href="javascript:void(0);" onclick="tp.delTr(this);">
                                        删除</a> <a href="javascript:void(0);"
                                            onclick="tp.stopTr(this);" class="stopT">
                                            <%#GetZYTitle(Eval("Status").ToString(),"2")%></a> </span><span class="inputname"
                                                style="display: none;"><a href="javascript:void(0);" onclick="tp.saveTr(this);">保存</a>
                                                <a href="javascript:void(0);" onclick="tp.cancelTr(this);">取消</a></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
        </table>
    </div>
    <div class="clearfix">
    </div>
    <div style="height: 10px;">
    </div>
    <div class="btn">
        <input name="" type="button" onclick="addNewRow()" value="新增" />
        <input name="" type="button" value="保存全部" onclick="tp.SaveAll()" />
        <%-- <input type="button" class="btnSave bold" onclick="javascript:$.closePopupLayer('BusinessTypeLayer',false);"
            value="关闭" />--%>
    </div>
    <div class="clearfix">
    </div>
</div>
