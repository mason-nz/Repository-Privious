<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectAgents.aspx.cs" Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.PopPages.SelectAgents"
    EnableViewState="false" %>

<script type="text/javascript">

    //查询操作
    function PersonSearch() {
        var pody = "BGID=" + escape($('#<%=ddlBussiGroup.ClientID %>').val()) +
        "&TrueName=" + escape($('#txtUserName').val()) +
        "&random=" + Math.random();
        $('#divPersonList').Mask();
        $('#divPersonList').load('/AjaxServers/PopPages/SelectAgents.aspx #divPersonList > *', pody, LoadDivSuccess);
    }
    //查询之后，回调函数
    function LoadDivSuccess(data) {
        // $('#divPersonList tr:even').addClass('color_hui'); //设置列表行样式
        //        unMaskPage();
        $('#divPersonList').UnMask();
    }
    //选择操作
    function SelectPerson(number, id) {
        $.closePopupLayer('SelectAgents', true, { agentid: id, number: number });
        return false;
    }
    //分页操作
    function ShowDataByPost2(pody) {
        //MaskPage();
        $('#divPersonList').Mask();
        $('#divPersonList').load('/AjaxServers/PopPages/SelectAgents.aspx #divPersonList > *', pody, LoadDivSuccess);
    }

//    $(function() {
//        $(window.document.body).BQPop({ left: 100, top: 180 });
//        $('#BQMain').show();
//    });
  
</script>
<div class="popup" style="position: relative;">
    <div class="title ft14">
        用户转移<a href="#" class="right"><img src="../../Images/c_btn.png" border="0" onclick="javascript:$.closePopupLayer('SelectAgents',false);" /></a></div>
    <div class="content">
        <div class="search">
            <ul>
                <li>
                    <label>
                        所属分组：</label>
                    <select id="ddlBussiGroup" runat="server" datatextfield="name" datavaluefield="value"
                        class="w100">
                    </select>
                </li>
                <li>
                    <label>
                        姓名：</label><input type="text" name="txtUserName" id="txtUserName" runat="server"
                            class="w200" /></li>
                <%--<li><label>姓名：</label><input type="text" value=""  class="w200"/></li>--%>
                <li style="width: 160px;" class="btn">
                    <input type="button" value="查询" onclick="javascript:PersonSearch();" class="w60" />
                    <%--<a href="#">清空已选项</a>--%></li>
            </ul>
            <div class="clearfix">
            </div>
        </div>
        <div id="divPersonList">
            <table cellspacing="0" cellpadding="0" class="fzList">
                <tr>
                    <th width="25%">
                        所属分组
                    </th>
                    <th width="20%">
                        姓名
                    </th>
                    <th width="20%">
                        工号
                    </th>
                    <th width="20%">
                        状态
                    </th>
                    <th width="15%">
                        操作
                    </th>
                </tr>
                <asp:repeater id="repterPersonlist" runat="server">
                    <ItemTemplate>
                      <tr>
                        
                          <td class="l">                            
                            <%#Eval("BGName")%>&nbsp;
                        </td>             
                        <td class="l">                            
                            <%#Eval("AgentName")%>&nbsp;
                        </td>
                        <td class="l"> 
                            <%#Eval("AgentNumber")%>&nbsp;
                        </td>
                        <td>在线</td>
                        <td>                             
                            <em style="cursor: pointer;font-style:normal;color: blue;" onclick="SelectPerson('<%# Eval("AgentNumber") %>','<%# Eval("AgentID") %>');">选择</em>                            
                        </td>
                      </tr>
                    </ItemTemplate>
                  </asp:repeater>
            </table>
            <!--分页开始-->
            <div class="pagesnew" style="margin: 10px 0 0 0;">
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
                <%--<span class="pre">上一页</span> <a href="#" class="active">1</a> <a href="#">2</a>
            <a href="#">3</a> <a href="#">4</a> <a href="#">5</a> <span>...</span> <span><a href="#"
                class="next">下一页</a></span> <span>共50页</span> <span>到第</span><input type="text" value="" /><span>页</span>
            <span class="qd_go"><a href="#">GO</a></span>--%>
            </div>
        </div>
        <!--分页结束-->
        <div class="clearfix">
        </div>
    </div>
</div>
