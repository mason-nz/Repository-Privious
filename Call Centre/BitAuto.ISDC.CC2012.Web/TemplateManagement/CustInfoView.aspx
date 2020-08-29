<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustInfoView.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.CustInfoView" %>

<style>
    #Ul2
    {
        margin: 10px 20px;
        clear: both;
        content: ".";
        display: block;
        font-size: 0;
        height: 160px;
        line-height: 0;
    }
    #Ul2 li
    {
        width: 270px;
        line-height: 30px;
    }
    #Ul3
    {
        margin: 10px 20px;
        clear: both;
        content: ".";
        display: block;
        font-size: 0;
        height: 150px;
        line-height: 0;
    }
    #Ul3 li
    {
        width: 270px;
        line-height: 30px;
    }
    .line
    {
        background: url("/css/img/line.jpg") repeat-x scroll 0 0 transparent;
        height: 1px;
        margin: 0 auto;
    }
</style>
<%if (liWidth == string.Empty)
  { %>
<ul id="Ul2" class="ui-sortable">
    <li class="" style="cursor: default;">
        <label>
            客户ID：</label>客户ID</li>
    <li class="" style="cursor: default;">
        <label>
            客户名称：</label>客户名称</li>
    <li class="" style="cursor: default;">
        <label>
            客户类别：</label>客户类别</li>
    <li class="" style="cursor: default;">
        <label>
            主营品牌：</label>主营品牌</li>
    <li class="" style="cursor: default;">
        <label>
            客户地区：</label>客户地区</li>
    <li class="" style="cursor: default;">
        <label>
            注册地址：</label>注册地址</li>
    <li class="" style="cursor: default;">
        <label>
            联系人：</label>联系人</li>
    <li class="" style="cursor: default;">
        <label>
            联系电话：</label>联系电话</li>
    <li class="" style="cursor: default;">
        <label>
            邮编：</label>邮编</li>
    <li class="" style="cursor: default;">
        <label>
            &nbsp;</label></li>
</ul>
<div class="line">
</div>
<ul id="Ul3" class="ui-sortable">
    <li class="" style="cursor: default;">
        <label>
            会员ID：</label>会员ID</li>
    <li class="" style="cursor: default;">
        <label>
            会员名称：</label>会员名称</li>
    <li class="" style="cursor: default;">
        <label>
            会员类型：</label>会员类型</li>
    <li class="" style="cursor: default;">
        <label>
            主营品牌：</label>主营品牌</li>
    <li class="" style="cursor: default;">
        <label>
            会员地区：</label>会员地区</li>
    <li class="" style="cursor: default;">
        <label>
            会员电话：</label>会员电话</li>
    <li class="" style="cursor: default;">
        <label>
            会员邮编：</label>会员邮编</li>
    <li class="" style="cursor: default;">
        <label>
            销售地址：</label>销售地址</li>
</ul>
<div class="line">
</div>
<div style="margin: 0; padding: 0; clear: none" class="Table2">
    <table cellpadding="0" border="0" cellspacing="0" class="Table2List" style="width: 100%;
        margin-left: 0px;">
        <tbody>
            <tr>
                <th width="10%">
                    联系人
                </th>
                <th width="10%">
                    职务
                </th>
                <th width="25%">
                    办公电话
                </th>
                <th width="25%">
                    移动电话
                </th>
                <th width="20%">
                    Email
                </th>
                <th width="10%">
                    操作
                </th>
            </tr>
        </tbody>
    </table>
</div>
<%}
  else
  {%>
<ul id="Ul2" class="ui-sortable">
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            客户ID：</label>客户ID</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            客户名称：</label>客户名称</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            客户类别：</label>客户类别</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            主营品牌：</label>主营品牌</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            客户地区：</label>客户地区</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            注册地址：</label>注册地址</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            联系人：</label>联系人</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            联系电话：</label>联系电话</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            邮编：</label>邮编</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            &nbsp;</label></li>
</ul>
<div class="line">
</div>
<ul id="Ul3" class="ui-sortable">
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            会员ID：</label>会员ID</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            会员名称：</label>会员名称</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            会员类型：</label>会员类型</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            主营品牌：</label>主营品牌</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            会员地区：</label>会员地区</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            会员电话：</label>会员电话</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            会员邮编：</label>会员邮编</li>
    <li class="" style="cursor: default; width: <%=liWidth%>">
        <label>
            销售地址：</label>销售地址</li>
</ul>
<div style="margin: 0; padding: 0; text-align: center;" class="Table2">
    <table cellpadding="0" border="0" cellspacing="0" class="Table2List" style="width: 70%;
        margin-left: 100px">
        <tbody>
            <tr>
                <th width="10%">
                    联系人
                </th>
                <th width="10%">
                    职务
                </th>
                <th width="25%">
                    办公电话
                </th>
                <th width="25%">
                    移动电话
                </th>
                <th width="20%">
                    Email
                </th>
                <th width="10%">
                    操作
                </th>
            </tr>
        </tbody>
    </table>
</div>
<%} %>