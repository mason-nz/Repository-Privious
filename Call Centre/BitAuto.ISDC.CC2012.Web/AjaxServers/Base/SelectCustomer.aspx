<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectCustomer.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.Base.SelectCustomer" %>

<script type="text/javascript">
    BindProvince('dllSelectCustomerProvince'); //绑定省份 
    if ('<%=RequestProvinceID %>' != '-1') {
        $('#dllSelectCustomerProvince').val('<%=RequestProvinceID %>').change();
    }


    //查询操作
    function SelectCustomerSearch() {
        var pody = 'type=<%=RequestType %>' +
             '&CustID=' + escapeStr($('#txtSelectCustID').val()) +
             '&custname=' + escapeStr($('#txtSelectCustName').val()) +
             '&provinceid=' + escapeStr($('#dllSelectCustomerProvince').val()) +
             '&cityid=' + escapeStr($('#dllSelectCustomerCity').val()) +
             '&page=1&search=yes';
        if ('<%=RequestType %>' == '2') {
            pody += '&CustType=<%=RequestCustType %>';
        }
        //$.jAlert(pody)
        $('#divSelectCustomerList').load('/AjaxServers/Base/SelectCustomer.aspx #divSelectCustomerList > *', pody, LoadDivSuccess);
    }
    //分页操作
    function ShowDataByPost1(pody) {
        $('#divSelectCustomerList').load('/AjaxServers/Base/SelectCustomer.aspx #divSelectCustomerList > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        $('#tableCustomer tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableCustomer');
    }
    //提交操作
    function SelectCustomerSubmit(custid, custname) {
        var url = '/AjaxServers/Base/CustManager.ashx?Action=GetCustPidsAndBrandIDs';
        AjaxPost(url, { CustID: custid }, null, function (data) {
            var s = $.evalJSON(data);
            var data = { CustID: custid, CustName: custname,
                CustPid: '', CustPidName: '',
                BrandIDs: '', BrandNames: '',
                Pid: '', PidName: ''
            };
            if (s.GetInfo == 'yes') {
                data.CustPid = s.CustPid; //所属厂商
                data.CustPidName = s.CustPidName;
                data.BrandIDs = s.BrandIDs; //主营品牌
                data.BrandNames = s.BrandNames;
                data.Pid = s.Pid;
                data.PidName = s.PidName;
            }
            $.closePopupLayer('SelectCustomerPopup', true, data);
        });
        //    }
    }

    //全部取消操作
    function AllCancel(tableid) {
        $('#' + tableid + ' :radio[checked=true]').attr('checked', '');
    }
</script>
<script type="text/javascript">
    $(function () {
        //敲回车键执行方法
        enterSearch(SelectCustomerSearch); 
    });
</script>
<div class="pop pb15 openwindow" style="width: 660px">
    <div class="title bold">
        <h2>
            <em id="spanTitle" runat="server" style="color: #FFFFFF;">选择所属产商</em></h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SelectCustomerPopup',false);">
        </a></span>
    </div>
    <ul class="clearfix  outTable">
        <li style="width: 300px;">
            <label>
                客户编号：</label>
            <input type="text" id="txtSelectCustID" name="txtSelectCustID" class="k200" />
        </li>
        <li style="width: 300px;">
            <label>
                客户名称：</label>
            <input type="text" id="txtSelectCustName" name="txtSelectCustName" class="k200" />
        </li>
        <li style="width: 400px;">
            <label style="width: 85px; padding-left: 35px;">
                客户所在地：</label>
            <select id="dllSelectCustomerProvince" name="dllSelectCustomerProvince" onchange="BindCity('dllSelectCustomerProvince','dllSelectCustomerCity');"
                class="area" runat="server" style="width: 100px;">
            </select>
            <select id="dllSelectCustomerCity" name="dllSelectCustomerCity" runat="server" style="width: 100px;">
                <option value="-1">市/县</option>
            </select>
        </li>
        <li class="btn">
            <input type="button" class="button" value="查询" onclick="javascript:SelectCustomerSearch();" />
        </li>
    </ul>
    <div class="Table2" id="divSelectCustomerList">
        <table width="98%" border="1" cellpadding="0" cellspacing="0" class="Table2List"
            id="tableCustomer">
            <tbody>
                <tr class="bold">
                    <th width="5%">
                    </th>
                    <th width="13%">
                        客户编号
                    </th>
                    <th width="37%">
                        客户名称
                    </th>
                    <th width="30%">
                        客户所在地
                    </th>
                </tr>
                <asp:repeater id="repterCustomerList" runat="server">
                                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">
                                        <td>
                                        <a href="javascript:SelectCustomerSubmit('<%# Eval("CustID")%>','<%# Eval("CustName")%>');"  id='<%# Eval("CustID") %>'>选择</a>
                                            </td>
                                        <td class="l">
                                            <%# Eval("CustID")%>
                                        </td>
                                        <td class="l">
                                            <%# Eval("CustName")%>
                                        </td>                                       
                                        <td class="l">
                                            <%#BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(Eval("ProvinceID").ToString())%>-<%#BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(Eval("CityID").ToString())%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:repeater>
            </tbody>
        </table>
        <div class="pageTurn mr10">  
            <p>
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
            </p>
        </div> 
    </div>
</div>
