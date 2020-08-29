<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AreaManageUI.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ZuoxiManage.AreaManageUI" %>

<script type="text/javascript">
    //修改用户
    function ModifyAreaUser(ul) {
        var regionid = $("#" + ul).attr("name");
        var yxuser = "";
        $("#" + ul + " li").each(function (i, e) {
            yxuser += $(e).attr("userid") + ",";
        });
        if (yxuser.length > 0) {
            yxuser = yxuser.substring(0, yxuser.length - 1);
        }
        $.openPopupLayer({
            name: "SelectEmployeePopup",
            parameters: { UserIDs: yxuser },
            url: "/ExamOnline/ExamObject/GetEmployeeList.aspx",
            beforeClose: function (e, data) {
                if (e) {
                    if (data != "") {
                        var pody = 'action=modify&regionid=' + regionid + '&data=' + escape(data) + '&r=' + Math.random();
                        $('#divBrandList').load('/ZuoxiManage/AreaManageUI.aspx #divBrandList > *', pody, function () { });
                    }
                }
            }
        });
    }
</script>
<div class="pop pb15 popuser openwindow" style="line-height: 35px;" id="divBrandList">
    <div class="title bold">
        <h2>
            区域管理设置</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AreaManageUI',false);">
        </a></span>
    </div>
    <div class="popT bold pt5">
        <span style="display: inline-block; width: 100px; text-align: right; margin-right: 2px;">
            <span style="color: Red">*</span>区域： </span>北京
    </div>
    <div class="popT bold pt5">
        <span style="display: inline-block; width: 100px; text-align: right; float: left;"><span
            style="color: Red">* </span>负责员工： </span><span style="font-weight: normal;">
                <ul class="clearfix deful" style="padding: 0px; width: 400px; float: left" id="bjul"
                    name="<%=Bjid %>">
                    <%=htmlBj%>
                </ul>
                <a href="javascript:void(0);" onclick="ModifyAreaUser('bjul')" style="vertical-align: top">
                    修改</a> </span>
    </div>
    <div class="line" style="float: left">
    </div>
    <div class="popT bold pt5">
        <span style="display: inline-block; width: 100px; text-align: right; margin-right: 2px;">
            <span style="color: Red">*</span>区域： </span>西安
    </div>
    <div class="popT bold pt5">
        <span style="display: inline-block; width: 100px; text-align: right; float: left;"><span
            style="color: Red">* </span>负责员工： </span><span style="font-weight: normal;">
                <ul class="clearfix deful" style="padding: 0px; width: 400px; float: left" id="xaul"
                    name="<%=Xaid %>">
                    <%=htmlXa %>
                </ul>
                <a href="javascript:void(0);" onclick="ModifyAreaUser('xaul')" style="vertical-align: top">
                    修改</a> </span>
    </div>
    <div class="btn" style="margin-top: 5px;">
        <input type="button" onclick="javascript:$.closePopupLayer('AreaManageUI',true);"
            value="关闭" class="btnSave bold" />
    </div>
</div>
