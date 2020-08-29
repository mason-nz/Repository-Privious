<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BlackDataList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.BlackWhite.AjaxService.BlackDataList" %>

<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th style="width: 9%;">
            电话号码
        </th>
        <th style="width: 9%;">
            有效期
        </th>
        <th style="width: 8%;">
            类型
        </th>
        <th style="width: 24%;">
            对应业务
        </th>
        <th style="width: 14%;">
            原因
        </th>
        <th style="width: 8%;">
            添加人
        </th>
        <th style="width: 9%;">
            添加时间
        </th>
        <th style="width: 8%;">
            状态
        </th>
        <th style="width: 10%;">
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
                    <%# Eval("CallOutNDType") == null ? "" : GetCallOutNDTypeName(Eval("CallOutNDType").ToString())%>&nbsp;
                </td> 
                <td>
                    <%#Eval("TrueName")%>&nbsp;
                </td> 
                <td>
                   <%# ( Eval("CreateDate")== null || string.IsNullOrEmpty(Eval("CreateDate").ToString())) ? "" : Convert.ToDateTime(Eval("CreateDate").ToString()).ToString("yyyy-MM-dd")%>&nbsp;
                </td>  
                <td class="nodistubstatus">
                    <%#Eval("Status") == null ? "" : (Eval("Status").ToString() == "0" ? ((Eval("ExpiryDate") != null && !string.IsNullOrEmpty(Eval("ExpiryDate").ToString())) && Convert.ToDateTime(Eval("ExpiryDate").ToString()).ToString("yyyy-MM-dd").CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) < 0 ? "已过期" : "正常") : (Eval("Status").ToString() == "1" ? "已过期" : ""))%>&nbsp;
                </td>
                <td >      
                  <a href='javascript:void(0);' onclick="javascript:PlayAudio('<%#Eval("CallID")%>');" title="播放录音"  <%#Eval("CallID") == null || string.IsNullOrEmpty(Eval("CallID").ToString()) ? "style=\'visibility:hidden;\'" : "style=\'visibility:visible;\'"%> ><img style=" vertical-align:text-bottom;" src='../../Images/callTel.png' /></a>&nbsp;        
                  <a href="javascript:void(0)" onclick='javascript:OpenBWEditLayer(<%#Eval("RecId")%>,<%#Eval("PhoneNum")%>);'>编辑</a>&nbsp;
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
    function OpenBWEditLayer(recid,phoneNum) {
        $.openPopupLayer({
            name: "UpdateBlackDataAjaxPopup",
            parameters: { RecId: recid, PhoneNumber: phoneNum, r: Math.random() },
            url: "/BlackWhite/NoDisturbLayer.aspx",
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
