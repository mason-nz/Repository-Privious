<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeAgentExclusiveList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.EmployeeAgentExclusive.EmployeeAgentExclusiveList" %>

<div class="bit_table" id="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
        <tr class="bold">
            <th width="5%">
                <input type="checkbox" id="checkboxCon" onclick="checkboxAll()" />
            </th>
            <th width="10%">
                姓名
            </th>
            <th width="5%">
                工号
            </th>
            <th width="15%">
                所属分组
            </th>
            <th width="5%">
                专属客服
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'"> 
                        <td align="center" style="cursor:pointer;" onclick="CheckBoxRelated(this)">
                            <input type="checkbox" name="UserID"  value="<%#Eval("UserID")%>" onclick="CheckBox(this)" />&nbsp;
                        </td>                 
                        <td align="center">
                            <%#Eval("TrueName").ToString()%>&nbsp;
                        </td>                
                        <td align="center">                            
                            <label id="NumLab_<%#Eval("UserID")%>" ><%#Eval("AgentNum")%> </label>&nbsp;
                            <input class = "MustNum" id="NumInput_<%#Eval("UserID")%>" type="text" value="<%#Eval("AgentNum")%>" style=" display:none; width:100px;" />
                        </td>
                        <td>
                            <%# GetGroupNameByBGID(Eval("BGID").ToString())%>&nbsp;
                        </td>
                        <td>
                             <%#Eval("IsExclusive").ToString()=="1"?"已设置":""%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <!--分页-->
    <div class="pages1" style="text-align: right; margin-bottom: 5px; clear: both; margin-top: 10px;">
        <table style="width: 99%;">
            <tr>
                <td style="text-align: right;">
                    <asp:literal runat="server" id="litPagerDown"></asp:literal>
                </td>
            </tr>
        </table>
    </div>
    <input type="hidden" id="pageHidden" value='<%=getCurrentPage() %>' />
</div>
<script>
    //全选
    function checkboxAll() {

        if ($("#checkboxCon").attr("checked")) {
            $('[name=UserID]:checkbox').attr("checked", true);
        }
        else {
            $('[name=UserID]:checkbox').attr("checked", false);
        }
    }

    function CheckBoxRelated(e) {

        //$(this).find("input[type=checkbox]").attr("checked", true);
        var nodes = e.childNodes;
        for (var i = 0; i < nodes.length; i++) {
            if (nodes[i].type == "checkbox") {
                nodes[i].checked = !nodes[i].checked;
            }

        }
    }

    function CheckBox(e) {

        //event.stopPropagation();
        e.checked = !e.checked;
    }

</script>
