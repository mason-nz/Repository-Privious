<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCKLOptionLogList.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib.UCKLOptionLogList" %>
<script type="text/javascript">
    function divShowHideEvent(divId, obj) {
        if ($(obj).attr("class") == "toggle") {
            $("#tableList").show();
            $(obj).attr("class", "toggle hide");
        }
        else {
            $("#tableList").hide();
            $(obj).attr("class", "toggle");
        }
    }
</script>
<style type="text/css">
    .title .toggle { background: url("/css/img/slidebuttons.png")  scroll right 0px transparent;  display: block; height: 12px; position: relative;width: 10px;float:right;top:6px;right:10px; *margin-top:-28px;}
    .title  .hide { background: url("/css/img/slidebuttons.png")  scroll right -16px transparent;  display: block; height: 12px; position: relative;width: 10px;float:right;top:6px;right:10px; *margin-top:-28px;}

    </style>
<div class="czRecord">
    <div class="title bold">
        操作记录<a href="javascript:void(0)" onclick="divShowHideEvent('divBaseInfo',this)" class="toggle"></a></div>
    <table width="100%" border="1" class="tableList RocordL" id="tableList" style="display: none">
        <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
            <th>
                操作人
            </th>
            <th>
                动作
            </th>
            <th>
                操作时间
            </th>
            <th>
                备注
            </th>
        </tr>
        <asp:Repeater ID="repeaterTableList" runat="server">
            <ItemTemplate>
                <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                    <td>
                        <%#BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(Eval("CreateUserID").ToString()))%>
                    </td>
                    <td>
                        <%#BitAuto.ISDC.CC2012.BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.EnumOptStatus), Convert.ToInt32(Eval("OptStatus")))%>
                    </td>
                    <td>
                        <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                    </td>
                    <td>
                        <%#Eval("Remark").ToString()%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</div>
