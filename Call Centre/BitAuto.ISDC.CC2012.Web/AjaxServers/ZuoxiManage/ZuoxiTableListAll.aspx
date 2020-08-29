<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZuoxiTableListAll.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ZuoxiManage.ZuoxiTableListAll" %>


<div class="bit_table" id="bit_table">
    <div class="optionBtn clearfix">
 
     </div>
        <table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
        <tr class="bold">
            <th width="5%">
                <input type="checkbox" id="checkboxCon" onclick="checkboxCon()" />
            </th>
            <th width="10%">
                姓名
            </th>
            <th width="5%">
                工号
            </th>
            <th width="10%">
                角色
            </th>
            <th width="10%">
                所属业务
            </th>
            <th width="15%">
                所属分组
            </th>
            <th width="10%">
                所属区域
            </th>
            <th width="*">
                管辖分组
            </th>
            <th width="5%">
                操作
            </th>
        </tr>
        <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'"> 
                        <td align="center">
                            <input type="checkbox" name="UserID" value="<%#Eval("UserID")%>"/>&nbsp;
                        </td>                 
                        <td align="center">
                            <%#Eval("trueName").ToString()%>&nbsp;
                        </td>                
                        <td align="center">                            
                            <label id="NumLab_<%#Eval("UserID")%>" ><%#Eval("AgentNum")%></label>
                            <input class = "MustNum" id="NumInput_<%#Eval("UserID")%>" type="text" value="<%#Eval("AgentNum")%>" style=" display:none; width:100px;" />
                        </td>
                        <td align="center" id="Role_<%#Eval("UserID")%>"> 
                            <%#Eval("RoleName")%>&nbsp;
                        </td>
                        <td align="center">
                            <%#GetBusinessType(Eval("BusinessType").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# GetGroupNameByBGID(Eval("BGID").ToString())%>&nbsp;
                        </td>
                        <td>
                            <%# Eval("RegionID").ToString() == "1" ? "北京" : Eval("RegionID").ToString()=="2"?"西安":""%> &nbsp;
                        </td>
                        <td>
                            <%#GetUserGroupNamesStr(int.Parse(Eval("UserID").ToString()))%>&nbsp;
                        </td>
                        <td>
                            <a href="javascript:void(0);" name="modfiyRole" useridstr='<%#Eval("UserID")%>' userName='<%#Eval("trueName").ToString()%>' dataright='<%#Eval("RightType").ToString()%>'  agentNum='<%#Eval("AgentNum")%>' >设置</a>
                        </td>        
                    </tr>
                </ItemTemplate>
            </asp:repeater>
    </table>
    <!--分页-->
    <div class="pages1" style="text-align: right; margin-bottom: 5px; clear: both; margin-top: 10px;">
        <table style="width: 99%;">
            <tr>
                <td style="text-align: right;">
                    <asp:literal runat="server" id="litPagerDown"></asp:literal>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            $(document).ready(function () {
                $("a[name='modfiyRole']").click(function (e) {
                    ModfiyRole($(this).attr("useridstr"), $(this).attr("userName"), $(this).attr("agentNum"));
                });


                $(".MustNum").keyup(function (event) {
                    if (((event.keyCode >= 48) && (event.keyCode <= 57)) || ((event.keyCode >= 96) && (event.keyCode <= 106)) || (event.keyCode == 190 || event.keyCode == 110)) {
                    } else {
                        $(this).attr("value", "");
                    }
                });
            });
            //弹出设置窗口 强斐 2015-4-8
            function ModfiyRole(UserID, UserName, AgentNum) {
                $.openPopupLayer({
                    name: "EmployeeAgentOption",
                    parameters: { "UserName": escape(UserName), UserID: UserID, AgentNum: AgentNum, r: Math.random() },
                    url: "EmployeeAgentOption.aspx",
                    beforeClose: function (e, data) {
                        if (e) {
                            var page = '<%=PageIndex %>';
                            search(page);
                        }
                    }
                });
            }
            //选择
            function checkboxCon() {
                if ($("#checkboxCon").attr("checked")) {
                    $('[name=UserID]:checkbox').attr("checked", true);
                }
                else {
                    $('[name=UserID]:checkbox').attr("checked", false);
                }
            }
            //区域管理
            function AreaManage() {
                $.openPopupLayer({
                    name: "AreaManageUI",
                    parameters: {},
                    url: "/ZuoxiManage/AreaManageUI.aspx",
                    beforeClose: function (e, data) {
                        if (e) {
                            window.location.reload();
                        }
                    }
                });
            }
            //批量修改
            function updateUsersRigth() {
                var userNames = "";
                var userIDs = "";

                //选择个数校验
                var checkUsers = $(":checkbox[name^='UserID'][checked=true]");
                var length = checkUsers.length;
                if (length == 0) {
                    $.jAlert("至少选择一个修改项！");
                    return;
                }

                //区域校验
                var region_Beijing = false;
                var region_Xian = false;

                for (var i = 0; i < length; i++) {
                    //第二列取name值
                    var name = $(checkUsers.parent("td").parent("tr").eq(i)).find("td").eq(1).html();
                    userNames += $.trim(name) + ";";
                    //取id值
                    userIDs += checkUsers.eq(i).val() + ",";
                    //第七列取区域值
                    var userRegin = $(checkUsers.parent("td").parent("tr").eq(i)).find("td").eq(6).text();
                    if ($.trim(userRegin) == "北京") {
                        region_Beijing = true;
                    }
                    else if ($.trim(userRegin) == "西安") {
                        region_Xian = true;
                    }
                }
                //存在两个区域的人员
                if (region_Beijing && region_Xian) {
                    $.jAlert("要修改的坐席必须属于同一区域！");
                    return;
                }
                //去掉尾‘,’
                if (userIDs.length > 0) {
                    userNames = userNames.substring(0, userNames.length - 1)
                    userIDs = userIDs.substring(0, userIDs.length - 1);
                }
                //设置区域
                var theRegion = -2;
                if (region_Beijing) {
                    theRegion = 1;
                }
                else if (region_Xian) {
                    theRegion = 2;
                }
                //打开弹层
                $.openPopupLayer({
                    name: "EmployeeAgentMutilOption",
                    parameters: { UserNames: escape(userNames), UserIDs: userIDs, AreaID: theRegion, r: Math.random() },
                    url: "EmployeeAgentMutilOption.aspx",
                    beforeClose: function (e, data) {
                        if (e) {
                            var page = '<%=PageIndex %>';
                            search(page);
                        }
                    }
                });
            }
        </script>
    </div>
    <input type="hidden" id="pageHidden" value='<%=getCurrentPage() %>' />
</div>
