<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlloServicePop.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AlloCustomer.AlloServicePop" %>

<link href="/Css/base.css" type="text/css" rel="stylesheet" />
<link href="/Css/style.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
    $('#tableList tr:even').addClass('color_hui'); //设置列表行样式

    $(document).ready(function () {
        enterSearch(PersonSearch);
        //PersonSearch();
    });

    //查询操作
    function PersonSearch() {
        var pody = "BGID=" + escape($("#ddlBussiGroup").val()) + "&Name=" + escape($('#txtPopUserName').val()) + "&random=" + Math.random();
        $('#divPersonList').load('/AlloCustomer/AlloServicePop.aspx #divPersonList > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#divPersonList tr:even').addClass('color_hui'); //设置列表行样式
    }
    //选择操作
    function SelectPerson(name,userid) {

        $('#popupLayer_' + 'AlloServicePop').data('name', name);
        $('#popupLayer_' + 'AlloServicePop').data('userid', userid);
        $.closePopupLayer('AlloServicePop', true);

    }
    //分页操作
    function ShowDataByPost4(pody) {
        $('#divPersonList').load('/AlloCustomer/AlloServicePop.aspx #divPersonList > *', pody + '&random=' + Math.random(), LoadDivSuccess);
    }
    (function () {

        document.getElementById("popAClear").onclick = function () {
            $('#popupLayer_' + 'AlloServicePop').data('userid', '');
            $('#popupLayer_' + 'AlloServicePop').data('name', '');
            $.closePopupLayer('AlloServicePop', true);
        }

    })();
</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            选择客服</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AlloServicePop',false);">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li style="width: 200px; float: left;">客服姓名：
            <input type="text" name="txtPopUserName" id="txtPopUserName" runat="server" class="w180"
                style="width: 120px;" /></li>
        <li style="width: 200px; float: left;" class="name1" id="popGroup">所在分组： <span>
            <select id="ddlBussiGroup" runat="server" class="w180" style="width: 120px;">
            </select></span></li>
        <li class="btn" style="width: 160px">
            <input name="" type="button" value="查 询" onclick="javascript:PersonSearch();" class="btnSave bold" />&nbsp;&nbsp;<a
                id='popAClear' style="cursor: pointer">清空已选项</a></li>
    </ul>
    <div class="Table2" id="divPersonList">
        <table width="98%" border="1" cellpadding="0" cellspacing="0" class="Table2List"
            id="tableList">
            <tbody>
                <tr class="bold">
                    <th width="10%">
                        操作
                    </th>
                    <th width="20%">
                        客服姓名
                    </th>
                    <th width="10%">
                        角色
                    </th>
                    <th width="30%">
                        所在分组
                    </th>
                </tr>
                <asp:repeater id="repterPersonlist" runat="server">
                    <ItemTemplate>
                      <tr>
                        <td> 
                            <a href="javascript:SelectPerson('<%# Eval("TrueName") %>','<%# Eval("UserID") %>')">选择</a>
                        </td>
                        <td class="l">
                            <%#Eval("TrueName").ToString()%>&nbsp;
                        </td>     
                        <td class="l"> 
                            <%#Eval("RoleName")%>&nbsp;
                        </td>           
                        <td class="l">                            
                            <%#Eval("groupName")%>&nbsp;
                        </td>
                      </tr>
                    </ItemTemplate>
                  </asp:repeater>
            </tbody>
        </table>
        <div class="pageTurn mr10">
            <p>
                <asp:literal runat="server" id="litPagerDown1"></asp:literal>
            </p>
        </div>
    </div>
</div>
