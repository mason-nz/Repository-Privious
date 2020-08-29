<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WhiteDataList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.BlackWhite.AjaxService.WhiteDataList" %>

<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th style="width: 10%;">
            电话号码
        </th>
        <th style="width: 10%;">
            有效期
        </th>
        <th style="width: 10%;">
            类型
        </th>
        <th style="width: 25%;">
            对应业务
        </th>
        <th style="width: 20%;">
            原因
        </th>
        <th style="width: 8%;">
            添加人
        </th>
        <th style="width: 10%;">
            添加时间
        </th>
        <th style="width: 7%;">
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
        <ItemTemplate>
            <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                <td>
                   <%# Eval("PhoneNum")%>&nbsp; 
                </td>
                <td>
                    <%# ( Eval("ExpiryDate")== null || string.IsNullOrEmpty(Eval("ExpiryDate").ToString())) ? "" : Convert.ToDateTime(Eval("ExpiryDate").ToString()).ToString("yyyy-MM-dd")%>&nbsp;
                </td>  
                <td>
                    <%# Eval("NewCallType")%>&nbsp;
                </td>                 
                <td class="td_cdidstocdidsname">
                    <%# Eval("CDIDS")%>&nbsp;
                </td> 
                <td>
                    <%# Eval("Reason")%>&nbsp;
                </td> 
                <td>
                    <%#Eval("TrueName")%>&nbsp;
                </td> 
                <td>
                   <%# ( Eval("CreateDate")== null || string.IsNullOrEmpty(Eval("CreateDate").ToString())) ? "" : Convert.ToDateTime(Eval("CreateDate").ToString()).ToString("yyyy-MM-dd")%>&nbsp;
                </td>  
                <td >                
                  <a href="javascript:void(0)" onclick='javascript:OpenBWEditLayer(<%#Eval("RecId")%>);'>编辑</a>&nbsp;
                  <a href="javascript:void(0)" onclick='javascript:DeleteData(<%#Eval("RecId")%>);'>删除</a> 
                </td> 
            </tr>
        </ItemTemplate>
    </asp:repeater>
</table>
<!--分页-->
<div class="pageTurn mr10">
    <p>
        <asp:literal runat="server" id="litPagerDown"></asp:literal>
    </p>
</div>
<input type="hidden" id="pageHidden" value='<%=getCurrentPage() %>' />
<script type="text/javascript">
    //修改指定数据
    function OpenBWEditLayer(recid) {
        $.openPopupLayer({
            name: "UpdateBlackDataAjaxPopup",
            parameters: { RecId: recid, r: Math.random() },
            url: "/BlackWhite/AddWhiteData.aspx",
            beforeClose: function (e, data) {
                if (e) {
                    var page = '<%=PageIndex %>';
                    if (recid == "") {
                        //新增，刷新到第一页
                        page = 1;
                    }
                    search(page);
                }
            }
        });
    }
</script>

 