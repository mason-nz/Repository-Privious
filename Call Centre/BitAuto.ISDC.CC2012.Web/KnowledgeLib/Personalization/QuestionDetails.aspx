<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuestionDetails.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.Personalization.QuestionDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>问题详细信息</title>
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
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            <span>问题解答</span>
        </div>
        <div class="baseInfo">
          <ul class="clearfix  outTable" style="padding-top: 10px;">
            <li style="width: 370px; float: left; clear:both;"><span class="redColor" style="vertical-align: top">*</span><b>问题标题：</b><span>
                <%=txtTilte%></span>
            </li>
            <li style="width: 370px; float: left;clear:both;"><span class="redColor" style="vertical-align: top">*</span><b>问题分类： </b><span>
                 <%=txtType%> </span>
            </li>
            <li style="width: 370px; float: left;clear:both;"><span class="redColor" style="vertical-align: top">*</span><b>问题内容： </b><span>
                 <%=txtContent%></span>
            </li>
            <li style="width: 370px; float: left;clear:both;"><span class="redColor" style="vertical-align: top">*</span><b>问题解答： </b><span>
                 <%=txtAnswer%></span>
            </li>
          </ul>
        </div>
    
    <div class="baseInfo">
        <div class="title">
                    操作记录<a href="javascript:void(0)" onclick="divShowHideEvent('baseInfo',this)" class="toggle"></a>
         </div>
        <div id="baseInfo">
            <table  style=" width:960px; margin-left:20px;"  border="0" cellspacing="0" cellpadding="0" class="tableList" id="tableReturnVisitCust">
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
