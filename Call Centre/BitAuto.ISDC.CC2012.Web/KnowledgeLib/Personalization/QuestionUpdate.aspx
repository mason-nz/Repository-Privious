<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuestionUpdate.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization.QuestionUpdate" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>问题解答</title>
    <script src="/Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Js/common.js" type="text/javascript"></script>
    <script src="/Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="/Js/json2.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <link href="/Css/base.css" rel="stylesheet" type="text/css" />
    <link href="/Css/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        //展开收缩
        function divShowHideEvent(divId, obj) {
            if ($("#" + divId).css("display") == "none") {
                $("#" + divId).slideDown("slow");
                $(obj).attr("class", "toggle");
            }
            else {
                $("#" + divId).slideUp("slow");
                $(obj).attr("class", "toggle hide");
            }
        }
        function saveData(action) {
            if ($.trim($("#txtAnswer").val()) == '') {
                $.jAlert("请填写问题解答内容！");
                return;
            }
            else {
                $.post("/KnowledgeLib/Personalization/PersonalizationHandler.ashx", { Action: action, QuestionId: $("#hidKLRId").val(), QuestionDetails: $("#txtAnswer").val(), r: Math.random() }, function (data) {
                    if (data == "提交成功") {
                        //$.jAlert("提交成功！");
                        $.jPopMsgLayer("提交成功！");
                        
                        $("#txtAnswer").val("");
                        $("#baseInfo").load("/KnowledgeLib/Personalization/QuestionUpdate.aspx #baseInfo > *", {Action:"<%=Action%>",id:"<%=Id%>"});
                    }
                    else {
                        $.jAlert(data);
                    }
                });
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            <span>问题解答</span>
        </div>
        <div class="baseInfo">
          <ul class="clearfix  outTable" style="padding-top: 10px; width:950px;">
            <li style="width: 950px; float: left; clear:both;"><span class="redColor" style="vertical-align: top">*</span><b>问题标题：</b><span>
                <%=txtTilte%></span>
            </li>
            <li style="width: 950px; float: left;clear:both;"><span class="redColor" style="vertical-align: top">*</span><b>问题分类：</b><span>
                 <%=txtType%> </span>
            </li>
            <li style="width: 950px; float: left;clear:both;"><span class="redColor" style="vertical-align: top">*</span><b>问题内容：</b><span>
                 <%=txtContent%></span>
            </li>
            <li style="width: 950px; float: left;clear:both;"><span style="vertical-align: top"><span class="redColor" style="vertical-align: top">*</span><b>问题解答：</b></span>
                <span>
                    <textarea id="txtAnswer" maxlength="2000" name="" rows="5" runat="server" style="width: 860px;"></textarea>
                </span>
            </li>
            <li class="btnsearch" style="width:900px;clear:both; margin-top:15px; text-align:right; padding-right:60px;">
                <input name="" type="button" value="提  交" onclick="javascript:saveData('<%=Action%>')" />
            </li>
          </ul>
          <input type="hidden" id="hidKLRId" runat="server"/>
        </div>
    
    <div class="baseInfo">
        <div class="title">
                    操作记录<a href="javascript:void(0)" onclick="divShowHideEvent('baseInfo',this)" class="toggle"></a>
         </div>
        <div id="baseInfo">
            <table style=" width:960px; margin-left:20px;" border="0" cellspacing="0" cellpadding="0" class="tableList" id="tableReturnVisitCust">
            <thead>
                <tr class="color_hui" >
                    <th width="11%">
                        <strong>操作人</strong>
                    </th>
                    <th width="11%">
                        <strong>动作</strong>
                    </th>
                    <th width="11%">
                        <strong>状态</strong>
                    </th>
                    <th width="16%">
                        <strong>操作时间</strong>
                    </th>
                    <th width="51%">
                        <strong>备注</strong>
                    </th>
                </tr>
            </thead>
            <tbody id="AjexTableBody">
            <asp:repeater id="repeaterList" runat="server">
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <%# Eval("TrueName")%>&nbsp;
                        </td>
                        <td align="center">
                            <%# Eval("ACTION")%>&nbsp;
                        </td>
                        <td align="center">
                            <%# Eval("STATUS")%>&nbsp;
                        </td>
                        <td align="center">
                            <%# Eval("CreateDate")%>&nbsp;
                        </td>
                        <td align="center">
                            <%# Eval("Comments")%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
             </asp:repeater>
             </tbody>
        </table>
        </div>
    </div>
    </div>
    </form>
</body>
</html>