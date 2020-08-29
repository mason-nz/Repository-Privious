<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditModuleInfo.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.AjaxServers.SysManager.EditModuleInfo" %>
<script>
    //提交数据 
    function PostPaly(Close) {
        if (Close) {
            var url = "../AjaxServers/SysManager/ModuleInfoManager.ashx";

            var ModuleName = $('#txtModuleName');
            var ModuleIntro = $('#<%=txtModuleIntro.ClientID %>');
            var ModuleURL = $('#txtModuleURL');
            var Links = $('#txtLinks');
            var ModuleID = $('#txtModuleID');
            var OrderNum = $('#txtOrderNum');
            var DomainCode = $('#selectDomainCode');

            if (trim(ModuleID.val()).length == 0) {
                alert('请填写模块编号！');
                return false;
            }
            if (trim(ModuleName.val()).length == 0) {
                alert('请填写模块名称！');
                return false;
            }
            if (OrderNum.val().search("^-?\\d+$") != 0) {
                alert("请输入一个整数!");
                return false;
            }

            if (trim(ModuleName.val()).length > 100) {
                alert('模块名称必须在100个字符以内！');
                return false;
            }
            if (trim(ModuleIntro.val()).length > 500) {
                alert('模块描述必须在500个字符以内！');
                return false;
            }
            if (trim(ModuleURL.val()).length > 200) {
                alert('模块URL必须在200个字符以内！');
                return false;
            }
            var postBody = "ShowType=<%=ShowType %>&Pid=<%=Pid  %>&SysID=<%=SysID %>"
		                + "&ModuleName=" + escape(trim(ModuleName.val()))
		                + "&ModuleID=" + escape(trim(ModuleID.val()))
		                + "&URL=" + escape(trim(ModuleURL.val()))
		                + "&Links=" + escape(trim(Links.val()))
		                + "&OrderNum=" + escape(trim(OrderNum.val()))
		                + "&Intro=" + escape(trim(ModuleIntro.val()))
                        + "&Blank=" + escape(trim($("#selBlank").val()))
		                + "&DomainCode=" + escape(trim(DomainCode.val()))
                        ; //构造要携带的数据       
            AjaxPost(url, postBody, null, SuccessPost);
        }
    }
    function SuccessPost(data) {
        var jsonData = $.evalJSON(data);
        if ('<%=ShowType %>' == 'add') {
            var str = $.trim(jsonData.add);
            if (str == 'yes') {
                alert('添加成功！');
                Close('ModuleAjaxPopup');
            }
            else if (str == 'ModuleNameExist') {
                alert("模块编号已存在");
            }
            else if (str == 'urlExist') {
                alert("链接地址已在别的模块被使用");
            }
            else if (str == 'ExistModuleName') {
                alert("您输入的模块名称已经存在，请重新输入！");
            }
            else if (str == 'UrlOutOfLength') {
                alert("模块URL必须在200个字符以内！");
            }
            else if (str == 'no') {
                alert('添加失败！');
            }

        }
        else if ('<%=ShowType %>' == 'edit') {
            var str = $.trim(jsonData.updata);
            if (str == 'yes') {
                alert('修改成功！');
                Close('ModuleAjaxPopup');
            }
            else if (str == 'urlExist') {
                alert("链接地址已在别的模块被使用");
            }
            else if (str == 'ExistModuleName') {
                alert("您输入的模块名称已经存在，请重新输入！");
            }
            else if (str == 'UrlOutOfLength') {
                alert("模块URL必须在200个字符以内！");
            }
            else {
                alert('修改失败！');
            }
        }
    }   	
</script>
<form id="form1" runat="server">
    <div>
        <div class="openwindow">
            <div class="close">
                <a href="javascript:Close('ModuleAjaxPopup');">关闭</a></div>
            <h2>
                <span>模块<asp:Literal runat="server" ID="litTitle"></asp:Literal></span></h2>
            <fieldset>
                <ol>
                    <li class="nowidth">
                        <label>模块名称：</label> <input type="text" name="txtModuleName" runat="server" id="txtModuleName"       maxlength="50" />
                    </li>
                     <li class="nowidth">
                        <label>模块编号：</label><input type="text" name="txtModuleID" id="txtModuleID" runat="server"        maxlength="40" />
                    </li>
                    <li class="nowidth">
                        <label>
                            模块描述：</label><asp:TextBox style="width: 400px;" runat="server" MaxLength="500" TextMode="MultiLine" Rows="6" ID="txtModuleIntro"></asp:TextBox>
                    </li>
                    <li class="nowidth">
                        <label>
                            包含链接：</label><asp:TextBox style="width: 400px;" runat="server" MaxLength="500" TextMode="MultiLine" Rows="6" ID="txtLinks"></asp:TextBox>
                    </li>
                    <li class="nowidth">
                        <label>
                            系统域名：</label><select id="selectDomainCode" name="selectDomainCode" runat="server" ></select>
                    </li>
                    <li class="nowidth">
                        <label>
                            URL：</label><input type="text" id="txtModuleURL" runat="server" maxlength="200" style="width: 400px" />
                    </li> 
                   
                    <li class="nowidth">
                        <label>
                            排序字段：</label> <input type="text" id="txtOrderNum" runat="server" maxlength="50" style="width: 400px"
                                    value="0" />

                    </li>
                    <li class="nowidth">
                    <label>
                            打开方式：</label> <select runat="server" id="selBlank" >
                            <option value="0" selected="selected">原页面打开</option>
                            <option value="1" >新页面打开</option>
                            </select>
                    </li>
                </ol>
            </fieldset> 
            <fieldset class="submits">
                <input type="button" value="保存" onclick="PostPaly(true);" class="button" />
                <input type="button" onclick="Close('ModuleAjaxPopup');" value="退出" class="button" />
            </fieldset>
        </div>
    </div>
</form>