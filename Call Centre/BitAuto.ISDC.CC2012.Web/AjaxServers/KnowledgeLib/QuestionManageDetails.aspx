<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuestionManageDetails.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib.QuestionManageDetails" %>


<script type="text/javascript">
    $(function () {

        $('#tableReturnVisitCust tr:even').addClass('color_hui'); //设置列表行样式
        $("#tableReturnVisitCust th").css({ "line-height": "18px" });
        SetTableStyle('tableReturnVisitCust');
    });

    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#tableReturnVisitCust tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableReturnVisitCust');
    }
  
</script>
<div id="divList" class="bit_table">
    <table width="99%" border="0" cellspacing="0" cellpadding="0" class="tableList" id="tableReturnVisitCust">
    <thead>
        <tr class="color_hui" >
            <th width="24%">
                <strong>标题</strong>
            </th>
            <th width="8%">
                <strong>分类</strong>
            </th>
            <th width="8%">
                <strong>提问人</strong>
            </th>
            <th width="14%">
                <strong>提问时间</strong>
            </th>
            <th width="8%">
                <strong>解答人</strong>
            </th>
            <th width="14%">
                <strong>解答时间</strong>
            </th>
            <th width="8%">
                <strong>状态</strong>
            </th>
              <th width="16%">
                <strong>操作</strong>
            </th>
        </tr>
        </thead>
        <tbody id="AjexTableBody">
        <asp:repeater id="repeaterList" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="center">
                        <%# Eval("Title")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("KCName")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("CreateUserName")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("NewCreateDate")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("AnswerUserName")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("NewLastModifyDate")%>&nbsp;
                    </td>
                    <td align="center">
                        <%# Eval("NewStatus")%>&nbsp;
                    </td>
                    <td align="center">
                    <span  <%#Eval("Status") != null && Eval("Status").ToString() == "1"  ? "":"style='display:none'"%>>
                    <a href="../../KnowledgeLib/Personalization/QuestionDetails.aspx?id=<%#Eval("Id")%>" target="_blank">查看</a>&nbsp;
                    <a href="../../KnowledgeLib/Personalization/QuestionUpdate.aspx?Action=updatequestion&id=<%#Eval("Id")%>" target="_blank">修改</a>&nbsp;
                    </span>
                    <span <%#Eval("Status") == null || Eval("Status").ToString() == "0"  ? "":"style='display:none'"%>>
                    <a  href="../../KnowledgeLib/Personalization/QuestionUpdate.aspx?Action=answerquestion&id=<%#Eval("Id")%>" target="_blank">解答</a>&nbsp;
                    <a  onclick="DeleteClick('<%#Eval("Id")%>')">删除</a>
                     </span>
                    </td> 
                </tr>
            </ItemTemplate>
         </asp:repeater>
         </tbody>
    </table>
<!--分页-->
<div class="pages1" style="text-align: right;">
    <uc:AjaxPager ID="AjaxPager" runat="server" ContentElementId="ajaxTable" /> &nbsp;   &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
</div>
</div>
