<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="CustTagPop.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage.CustTagPop" %>

<script type="text/javascript">
    function saveCustTagChange() {
        var checkedTagIds = "";
        var checkedTagNum = 0;
        $.each($(".kh_classfied :checkbox[name='ckCustTag']"), function () {
            if ($(this).attr("checked") == true) {
                checkedTagIds += "," + $(this).val();
                checkedTagNum++;
            }
        });
        if (checkedTagNum > 2) {
            $.jAlert("最多只能选两个标签");
            return false;
        }
        else {
            if (checkedTagIds.length > 0) {
                checkedTagIds = checkedTagIds.substr(1);
            }
            $.post("/AjaxServers/ProjectManage/CustTagHandler.ashx", { Action: "saveCustTagChange", TagIds: checkedTagIds, CustId: '<%=CustID%>', UserId: '<%=userid%>', r: Math.random() }, function (data) {
                if (data = "success") {
                    $.jAlert("保存成功", function () {
                        $.closePopupLayer('CustTagPopLayer', false);
                        search(_tagid);
                    });
                }
                else {
                    $.jAlert(data);
                }
            });
        }
    }
</script>
<!--客户分类-->
<div class="w300">
    <div class="openwindow title ft14 taskT">
        <h2 style="color: White;">
            客户分类
        </h2>
        <span class="right"><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AssignmentTask',false);">
        </a></span>
    </div>
    <table cellpadding="0" cellspacing="0" class="kh_classfied">
        <tr>
            <th width="20%" style=" padding-right:4px;">
                <input name="" type="checkbox" value="" id="ckCheckAllTag" />
            </th>
            <th width="80%">
                分类名称
            </th>
        </tr>
        <asp:repeater id="rp_CustTags" runat="server">
              <ItemTemplate>
                <tr>
                    <td>
                        <input name="ckCustTag" type="checkbox" <%#(Eval("HasThisTag") != null && Eval("HasThisTag").ToString() == "1") ? "checked=true":"" %>   value='<%#Eval("TagID")%>' />&nbsp;
                    </td>
                    <td class="cName">
                        <%#Eval("TagName")%>&nbsp;
                    </td>
                </tr>
              </ItemTemplate>
        </asp:repeater>
    </table>   
    <div class="btn">
        <input type="button" name="" value="保 存" class="btnSave bold" onclick="javascript:saveCustTagChange()"/>&nbsp;&nbsp;
        <input type="button" name="" value="取 消" onclick="javascript:$.closePopupLayer('CustTagPopLayer',false);"
            class="btnCannel bold" />
    </div>
</div>