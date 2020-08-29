<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoreUrlConfig.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.SeniorManage.MoreUrlConfig" %>

<script type="text/javascript">
    function allsave() {
        var flag = true;
        var ServerTimeList = new Array();
        $("#trList tr:gt(0)").each(function () {
            var sourcetype = $(this).find("td:eq(0)").attr("sourcetype");
            var MoreURL = $(this).find("td:eq(1) input[type='text'][name='MoreURL']").val();
            var sourcetypename = $(this).find("td:eq(0)").text();
            if (checkURL(MoreURL, sourcetypename, $(this))) {
                var MoreURL = MoreURL.replace("&", "&amp;"); 
                var servertimeModel = {
                    SourceTypeName: escape(sourcetypename),
                    SourceType: escape(sourcetype),
                    MoreURL: escape(MoreURL)
                };
                ServerTimeList.push(servertimeModel);
            }
            else {
                flag = false;
                return false;
            }
        });
        if (flag) {
            var ServerTimeStr = JSON.stringify(ServerTimeList);
            var pody = {
                MoreURLStr: escape(ServerTimeStr),
                Action: escape('savemoreurl')
            };
            AjaxPostAsync("/AjaxServers/SeniorManage/BussinessConfigDeal.ashx", pody, function () { }, function (data) {
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "success") {
                    $.jAlert("保存成功");
                }
                else {
                    $.jAlert("保存失败：" + jsonData.msg);
                }
            });
        }
    }
    function checkURL(URL, sourctypename,control) {
        var str = URL;
        //下面的代码中应用了转义字符"\"输出一个字符"/"
        var Expression = /http(s)?:\/\/([\w-]+\.)+[\w-]+(\/[\w- .\/?%&=]*)?/;
        var objExp = new RegExp(Expression);
        if (objExp.test(str) == true) {
            return true;
        } else {
            $.jAlert(sourctypename + "链接输入不正确", function () { $(control).find("td:eq(1) input[type='text'][name='MoreURL']").focus(); });
            return false;
        }
    }
    
    
   
</script>
<div class="popup openwindow">
    <div class="title ft14">
        <h2>
            业务线“更多”URL配置
        </h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('AddFreProblem',false);"
            class="right">
            <img src="/images/c_btn.png" border="0" />
        </a></span>
    </div>
    <div class="content">
        <!--查询开始-->
        <div class="searchTj">
            <div class="clearfix">
            </div>
        </div>
        <!--查询结束-->
        <div class="dc">
        </div>
        <!--列表开始-->
        <div id="ajaxMessageInfo" class="cxList" style="margin-top: 8px; height: auto;">
            <table border="0" cellspacing="0" cellpadding="0">
                <tbody id="trList">
                    <tr>
                        <th width="25%">
                            业务线
                        </th>
                        <th width="50%">
                            链接
                        </th>
                    </tr>
                    <asp:repeater id="repeaterTableList" runat="server">
                        <ItemTemplate>
                            <tr style="cursor: pointer" class="" onclick="">
                                <td sourcetype="<%#((BitAuto.DSC.IM_2015.BLL.MoreURlModelClss)Container.DataItem).SourceType%>">
                                    <%#((BitAuto.DSC.IM_2015.BLL.MoreURlModelClss)Container.DataItem).SourceTypeName%>
                                </td>
                                <td>
                                    <input type="text" name="MoreURL" value='<%#((BitAuto.DSC.IM_2015.BLL.MoreURlModelClss)Container.DataItem).MoreURL%>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
                </tbody>
            </table>
        </div>
        <div class="clearfix">
        </div>
        <div class="btn">
            <input type="button" value="保存" class="save w60" onclick="allsave()" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" value="关闭" class="cancel w60 gray" onclick="javascript:$.closePopupLayer('AddFreProblem',false);" /></div>
    </div>
</div>
