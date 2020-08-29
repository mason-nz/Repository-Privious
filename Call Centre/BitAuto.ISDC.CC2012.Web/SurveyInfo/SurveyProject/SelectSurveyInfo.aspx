<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectSurveyInfo.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.SurveyInfo.SurveyProject.SelectSurveyInfo" %>

<script type="text/javascript">
    //查询试卷信息
    function SearchEm() {
        LoadingAnimation('divQueryEmployeeList');
        var surveyName = escapeStr($.trim($("#txtSurveyNamePop").val()));
        var bgId = $.trim($("[id$='sltUserGroupPop']").val());
        var scId = $.trim($("#sltSurveyCategoryPop").val());
        var createUserId = $("[id$='sltCreateUserPop']").val();
        var pody = {
            SurveyName: surveyName,
            BGID: bgId,
            SCID: scId,
            CreateUserID: createUserId,
            R: Math.random()
        }
        $('#divQueryEmployeeList').load('/SurveyInfo/SurveyProject/SelectSurveyInfo.aspx #divQueryEmployeeList > *', pody);
    }

    //选择操作
    function SelectSurvey(siid, name) {
        $('#popupLayer_' + 'SelectSurveyInfo').data('SIID', siid);
        $('#popupLayer_' + 'SelectSurveyInfo').data('Name', name);
        $.closePopupLayer('SelectSurveyInfo', true);
    }

    //所属业务组改变时，重新加载分类
    function UserGroupChangedPop() {
        $("[id$='sltSurveyCategoryPop']").find("option").remove();
        var bgId = $("[id$='sltUserGroupPop']").val();
        $.post("/AjaxServers/SurveyInfo/CommonHandler.ashx", { Action: "GetSurveyCategory", BGID: bgId }, function (data) {
            if (data) {
                $("[id$='sltSurveyCategoryPop']").append("<option value='-1'>请选择</option>");
                var jsonData = $.evalJSON(data);
                $.each(jsonData, function (i, item) {
                    $("[id$='sltSurveyCategoryPop']").append("<option value='" + item.SCID + "'>" + item.Name + "</option>");
                });
            }
            else {
                $("[id$='sltSurveyCategoryPop']").append("<option value='-1'>请选择</option>");
            }
        });
    }
    //分页操作
    function ShowDataByPost1(pody) {
        LoadingAnimation('divQueryEmployeeList');
        $('#divQueryEmployeeList').load('/SurveyInfo/SurveyProject/SelectSurveyInfo.aspx #divQueryEmployeeList > *', pody);
    }
    $(document).ready(function () {
        enterSearch(SearchEm);
        UserGroupChangedPop();
    }); 
</script>
<div class="pop pb15 openwindow" style="background: #FFF; width: 650px;">
    <div class="title bold">
        <h2>
            问卷选择</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SelectSurveyInfo',false);">
        </a></span>
    </div>
    <div class="more" id="closebox" style="float: right;" onclick="javascript:$.closePopupLayer('SelectSurveyInfo',false);">
    </div>
    <div id='divQuerySurvey'>
        <div class="infor_li renyuan_cx">
            <ul class="clearfix  outTable">
                <li style="width: 275px;">
                    <label style="width: 75px;">
                        所属分组：</label>
                    <select id="sltUserGroupPop" runat="server" style="width: 190px;" onchange="UserGroupChangedPop()">
                    </select>
                </li>
                <li id="liSurveyCategoryPop" style="width: 230px;">
                    <label style="width: 75px;">
                        分类：</label>
                    <select id="sltSurveyCategoryPop" style="width: 120px;">
                    </select>
                </li>
                <li style="width: 275px;">
                    <label style="width: 75px;">
                        问卷：</label>
                    <input type="text" id="txtSurveyNamePop" class="w190" />
                </li>
                <li style="width: 230px;">
                    <label style="width: 75px;">
                        创建人：</label>
                    <select id="sltCreateUserPop" runat="server" style="width: 120px;">
                    </select>
                </li>
                <li class="btn">
                    <input type="button" value="查询" class="btnSave bold" onclick="javascript:SearchEm();" />
                </li>
            </ul>
        </div>
        <div id="divQueryEmployeeList">
            <table width="100%" cellspacing="0" cellpadding="0" id="tableQueryEmployee" class="tableList mt10 mb15">
                <tr>
                    <th style="width: 10%;">
                        选择
                    </th>
                    <th style="width: 35%;">
                        调查名称
                    </th>
                    <th style="width: 15%;">
                        所属分组
                    </th>
                    <th style="width: 15%;">
                        分类
                    </th>
                    <th style="width: 10%;">
                        创建人
                    </th>
                    <th style="width: 20%;">
                        创建时间
                    </th>
                </tr>
                <tbody>
                    <asp:repeater id="rptSurvey" runat="server">
                    <ItemTemplate>
                    <tr>
                        <td>
                            <a class="linkBlue" onclick="SelectSurvey('<%# Eval("SIID") %>','<%# Eval("Name").ToString()%>');" name='<%# Eval("Name").ToString()%>' id='<%# Eval("SIID") %>' style=" cursor:pointer;">选择</a>
                        </td>
                        <td>
                            <%#Eval("Name")%>
                        </td>
                        <td>
                            <%# Eval("GroupName").ToString()%>
                        </td>                        
                        <td >
                            <%# Eval("CategoryName")%>
                        </td>
                        <td >
                            <%# ShowCreateUserName(Eval("CreateUserID").ToString())%>
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                        </td>
                    </tr>                   
                    </ItemTemplate>
                    </asp:repeater>
                </tbody>
            </table>
            <div class="it_page" style="text-align: right;">
                <asp:literal runat="server" id="litPagerDown"></asp:literal>
                &nbsp;&nbsp;&nbsp;
            </div>
        </div>
    </div>
</div>
