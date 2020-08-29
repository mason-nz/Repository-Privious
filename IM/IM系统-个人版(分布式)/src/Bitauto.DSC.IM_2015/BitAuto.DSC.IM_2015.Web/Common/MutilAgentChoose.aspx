<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MutilAgentChoose.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.Common.MutilAgentChoose" %>

<script type="text/javascript">
    //查询
    function SearchAgent() {
        LoadingAnimation("AgentList");
        var pody = {
            BGID: $("#sel_group").val(),
            UserName: $.trim($("#txt_agent").val()),
            CityGroups: $.trim($("#hidden_citygroups").val()),
            //缓存数据
            Select: '<%=Select %>',
            ChooseUsers: '<%=ChooseUsers %>',
            r: Math.random()
        };
        var podyStr = JsonObjToParStr(pody);
        $("#AgentList").load("/Common/MutilAgentChoose.aspx #AgentList > *", podyStr);
    }
    //分页操作
    function ShowDataByPost11(podyStr) {
        LoadingAnimation("AgentList");
        $("#AgentList").load("/Common/MutilAgentChoose.aspx #AgentList > *", podyStr);
    }
    //清空
    function ClearSelect() {
        var pody = {
            userid: "",
            name: ""
        };
        $.closePopupLayer('ShowMutilAgentChoose', true, pody);
    }
    //单选
    function ChooseOneAgent(userid, name) {
        var pody = {
            userid: userid,
            name: name
        };
        $.closePopupLayer('ShowMutilAgentChoose', true, pody);
    }
    //多选
    function ChooseMoreAgent(userid, bgname, truename, agentnum, istip) {
        if ($("#tr_" + userid).length != 0) {
            if (istip) {
                $.jAlert("该客服已添加！");
            }
            return;
        }

        var html = "<tr id=\"tr_" + userid + "\">";
        html += "<td>" + bgname + "</td>";
        html += "<td>" + truename + "</td>";
        html += "<td>" + agentnum + "</td>";
        html += "<td class=\"close\" onclick=\"CancelSelected('" + userid + "')\">取消";
        html += "<input type=\"hidden\" name=\"hidden_selected_userid\" value=\"" + userid + "\" username=\"" + truename + "\"/>";
        html += "</td></tr>";
        $("#selectedList_table").append(html);
    }
    //移除
    function CancelSelected(userid) {
        if ($("#tr_" + userid).length != 0) {
            $("#tr_" + userid).remove();
        }
    }
    //获取多选选择的内容
    function GetMutilSelectedUserids() {
        var selects = document.getElementsByName("hidden_selected_userid");
        var userids = "";
        var usernames = "";
        for (var i = 0; i < selects.length; i++) {
            userids += selects[i].value + ",";
            usernames += $(selects[i]).attr("username") + ",";
        }
        if (userids.length > 1) {
            userids = userids.substr(0, userids.length - 1);
        }
        if (usernames.length > 1) {
            usernames = usernames.substr(0, usernames.length - 1);
        }
        return { userids: userids, usernames: usernames }
    }
    //确定
    function ConfirmChoose() {
        var para = GetMutilSelectedUserids();
        if (para.userids.length == 0) {
            $.jAlert("请至少选择一个客服！");
            return;
        }
        $.closePopupLayer('ShowMutilAgentChoose', true, para);
    }
    //选择本页所有人员
    function ChoosePageAll() {
        var tr_hidden_data = document.getElementsByName("tr_hidden_data");
        for (var i = 0; i < tr_hidden_data.length; i++) {
            var v = tr_hidden_data[i].value.split(',');
            ChooseMoreAgent(v[0], v[1], v[2], v[3], false);
        }
    }
    //回车查询
    function EnterPressToSerach() {
        var e = window.event;
        if (e.keyCode == 13) {
            SearchAgent();
        }
    }
</script>
<div class="popup openwindow">
    <div class="title ft14">
        <h2>
            <%=DefineTitle %>
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('ShowMutilAgentChoose',false);"
            class="right">
            <img src="/images/c_btn.png" border="0" />
        </a></span>
    </div>
    <div class="content">
        <div class="search">
            <ul>
                <li>
                    <label>
                        所属分组：
                    </label>
                    <select class="w100" id="sel_group">
                        <option value="-1">请选择</option>
                        <asp:repeater id="rpt_group" runat="server">
                        <ItemTemplate>
                            <option value="<%#Eval("BGID") %>">
                                <%#Eval("Name")%>
                            </option>
                        </ItemTemplate>
                    </asp:repeater>
                    </select>
                </li>
                <li>
                    <label>
                        姓名：
                    </label>
                    <input type="text" value="" class="w200" id="txt_agent" onkeypress="EnterPressToSerach();" />
                </li>
                <li style="width: 160px;" class="btn">
                    <input type="button" value="查询" class="w60" onclick="SearchAgent()" />
                    <%if (Select == "multiple")
                      {%>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <%}
                      else
                      {%><a href="javascript:void(0)" onclick="ClearSelect()">清空已选项</a>
                    <%} %>
                    <input type="hidden" id="hidden_citygroups" value="<%=CityGroups %>" />
                </li>
            </ul>
            <div class="clearfix">
            </div>
        </div>
        <div id="AgentList">
            <table cellspacing="0" cellpadding="0" class="fzList">
                <tr>
                    <th width="40%">
                        所属分组
                    </th>
                    <th width="15%">
                        姓名
                    </th>
                    <th width="15%">
                        工号
                    </th>
                    <th width="25%">
                        操作
                    </th>
                </tr>
                <asp:repeater id="rpt_agent" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="hidden" name="tr_hidden_data" value="<%#Eval("UserID")%>,<%#Eval("BGName")%>,<%#Eval("TrueName")%>,<%#Eval("AgentNum")%>" />
                                <%#Eval("BGName")%>
                            </td>
                            <td>
                                <%#Eval("TrueName")%>
                            </td>
                            <td>
                                <%#Eval("AgentNum")%>
                            </td>
                            <td>
                            <%if (Select == "multiple")
                              {%>
                                <a href="javascript:void(0)" onclick="ChooseMoreAgent('<%#Eval("UserID")%>','<%#Eval("BGName")%>','<%#Eval("TrueName")%>','<%#Eval("AgentNum")%>',true)">选择</a>
                              <%}
                              else
                              {%>
                                <a href="javascript:void(0)" onclick="ChooseOneAgent('<%#Eval("UserID")%>','<%#Eval("TrueName")%>')">选择</a>
                              <%} %>                                
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
            </table>
            <div class="pagesnew" style="float: right; margin: 10px;" id="itPage">
                <p>
                    <asp:literal runat="server" id="litPagerDown"></asp:literal>
                </p>
            </div>
        </div>
        <div class="clearfix">
        </div>
        <%if (Select == "multiple")
          { %>
        <div>
            <span style="color: #666;">已选择人员列表</span> <span style="color: #666;">&nbsp;&nbsp;(</span>
            <a href="javascript:void(0)" onclick="ChoosePageAll()">添加本页所有人员</a> <span style="color: #666;">
                )</span>
        </div>
        <div id="selectedList">
            <table id="selectedList_table" cellspacing="0" cellpadding="0" class="fzList" style="margin-top: 20px;">
                <tr>
                    <th width="40%">
                        所属分组
                    </th>
                    <th width="15%">
                        姓名
                    </th>
                    <th width="15%">
                        工号
                    </th>
                    <th width="25%">
                        操作
                    </th>
                </tr>
            </table>
        </div>
        <div class="clearfix">
        </div>
        <div class="btn">
            <input type="button" value="确定" class="save w60" onclick="ConfirmChoose()" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" value="关闭" class="cancel w60 gray" onclick="javascript:$.closePopupLayer('ShowMutilAgentChoose',false);" />
        </div>
        <%} %>
    </div>
</div>
