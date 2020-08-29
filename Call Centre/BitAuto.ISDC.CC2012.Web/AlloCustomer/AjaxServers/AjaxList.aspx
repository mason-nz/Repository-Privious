<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AlloCustomer.AjaxServers.List" %>

<script type="text/javascript">
    (function () {
        var oAllSelect = document.getElementById("ckbAllSelect");
        var aSelect = document.getElementsByName("chkSelect");
        for (var i = 0; i < aSelect.length; i++) {
            aSelect[i].onclick = function () {
                if (this.checked) {
                    var isAllCheck = true;
                    for (var i = 0; i < aSelect.length; i++) {
                        if (!aSelect[i].checked) {
                            isAllCheck = false;
                            break;
                        }
                    }
                    if (isAllCheck) oAllSelect.checked = true;
                }
                else {
                    oAllSelect.checked = false;
                }
            };
        }
        oAllSelect.onclick = function () {
            if (this.checked) {
                for (var i = 0; i < aSelect.length; i++) {
                    aSelect[i].checked = true;
                }
            }
            else {

                for (var i = 0; i < aSelect.length; i++) {
                    aSelect[i].checked = false;
                }

            }
        }
    })();
</script>
<div class="bit_table">
    <table cellpadding="0" cellspacing="0" class="tableList" width="99%" id="tableList">
        <tr>
            <th style="width: 5%">
                <input id="ckbAllSelect" type="checkbox" name="selItem" />
            </th>
            <th>
                客户名称
            </th>
            <th>
                需求总数量
            </th>
            <th>
                集客中需求量
            </th>
            <th>
                负责客服
            </th>
            <th>
                负责销售
            </th>
            <th>
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                        <td>                       
                           <input name="chkSelect"  type="checkbox" value='<%#Eval("CustID") %>' KeFuId='<%#Eval("KeFuUserId").ToString()%>'/>
                        </td>
                        <td>
                        <%#Eval("CustName")%> &nbsp;
                        </td>
                        <td>
                            <%#Eval("CountNum")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("Countjkz")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("KeFuName")%>&nbsp;
                        </td>
                        <td>
                            <%#Eval("SaleName")%>&nbsp;
                        </td> 
                        <td>
                       <%#getOperLink(Eval("CustID").ToString())%>&nbsp;
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <div class="pageTurn mr10" style="margin-top: 10px;">
        <p>
            <asp:literal runat="server" id="litPagerDown"></asp:literal>
        </p>
    </div>
</div>
