<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewCustBaseInfo.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ExternalTask.CustBaseInfo.UCCustBaseInfo.ViewCustBaseInfo" %>
 <%@ Register Src="~/CustCategory/BuyCarInfoView.ascx" TagName="BuyCarInfoView" TagPrefix="uc1" %>
 <ul class="clearfix">
        	<li><label>客户ID：</label><span><%=CustID%></span></li>
            <li>&nbsp;</li>
            <li><label>姓名：</label><span><%=CustName%></span></li>
            <li><label>地区：</label><span><%=PlaceStr%></span></li>
            <li><label>性别：</label><span><%=Sex%></span></li>
            <li><label>分属大区：</label><span><%=AreaStr%></span></li>
            <li><label>电话：</label><span><%=Tels%></span></li>
            <li><label>地址：</label><span class="exceed"><%=Address%></span></li>
            <li><label>邮箱：</label><span><%=Email %></span></li>
            <li><label>数据来源：</label><span><%=DataSourceStr %></span></li>
            <li><label>客户分类：</label><span><%=CustCategoryStr%></span></li>
        </ul>
        <div class="line"></div> 
<asp:PlaceHolder ID="PlaceHolderCustCategory" runat="server"></asp:PlaceHolder>
  <div class="line"></div> 
        <ul class="clearfix">
        	<li><label>创建人：</label><span><%=CreateUserName%></span></li>
            <li><label>创建时间：</label><span><%=CreateTime %></span></li>
            <li><label>修改人：</label><span><%=ModifyUserName%></span></li>
            <li><label>修改时间：</label><span><%=ModifyTime%></span></li>
        </ul> 