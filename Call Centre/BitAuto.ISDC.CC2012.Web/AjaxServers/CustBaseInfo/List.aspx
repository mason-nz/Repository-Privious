<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo.CustBaseInfoList" %>

<script type="text/javascript">
    //添加工单
    function AddWOrderV2(obj, tels, urls) {
        var co = $(obj);
        var arraytel = tels.split(',');
        var arrayurl = urls.split(',');
        var content = $('<div class="open_tell" stype="position:absolute;"></div>');
        var ul = $('<ul class="list"></ul>').width(80);
        var i;
        for (i = 0; i < arraytel.length; i++) {
            var v = arraytel[i];
            var ulObj = $('<li>').append(
                $('<a>').html(v)
                .attr('href', '/WOrderV2/AddWOrderInfo.aspx?' + arrayurl[i])
                .attr('target', '_blank')
                .css('cursor', 'pointer'));
            ul.append(ulObj);
        }
        content.append(ul).appendTo($('body'));
        var height = 25 * (i + 1) + 29;
        var top = (co.offset().top - height) + 100;
        var left = co.offset().left - 21;
        content.css({ left: left, top: top })
        .hover(
        function () {

        },
        function () {
            $(this).remove();
        });
    }
</script>
<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th>
            客户ID
        </th>
        <th>
            客户姓名
        </th>
        <th>
            性别
        </th>
        <th>
            联系电话
        </th>
        <th>
            地区
        </th>
        <th>
            客户分类
        </th>
        <th>
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">                     
                <td>
                    <%#Eval("CustID") %>&nbsp;
                </td>
                <td>
                    <a href='../TaskManager/CustInformation.aspx?CustID=<%#Eval("CustID")%>' target="_blank" class="linkBlue">
                        <%#Eval("CustName")%>&nbsp;
                    </a>
                </td>
                <td>
                    <%#Eval("SexName")%>&nbsp;
                </td>
                <td>
                    <%#Eval("Tels")%>&nbsp;
                </td>
                <td>
                    <%#Eval("AreaName")%>&nbsp;
                </td>
                <td>
                    <%#Eval("CustCategoryName")%>&nbsp;
                </td>
                <td>
                    <%#GetOperLink(Eval("Tels").ToString())%>&nbsp;
                </td>
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<br />
<!--分页-->
<div class="pages1" style="text-align: right;">
    <uc:AjaxPager ID="AjaxPager_Custs" runat="server" ContentElementId="ajaxTable" />
</div>
