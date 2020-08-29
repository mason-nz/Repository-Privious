<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LabelConfig.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.SeniorManage.LabelConfig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {           
            search2();
        });

        
    </script>
</head>
<div class="popup openwindow">
	<div class="title ft14"><h2>标签配置管理</h2><span><a href="#" class="right" onclick="javascript:$.closePopupLayer('LabelConfigPop',false);"><img src="../images/c_btn.png" border="0"/></a></span></div>
    <div class="content">
    	<div class="search">
        <ul>
        <li style="width:300px;"><label>所属分组：</label><select id="selGroup" runat="server" class="w100" style="width:210px;"><option></option></select></li>        
        </ul>
        <div class="clearfix"></div>
        </div>
        <div id="divConfig">
        <%--<table cellspacing="0" cellpadding="0"  class="fzList">
          <tr>
            <th width="10%"></th>
            <th width="50%">标签</th>
            <th width="30%">操作</th>
          </tr>
          <asp:repeater id="repeaterConfig" runat="server">
                        <ItemTemplate>
                            <tr style="cursor:pointer" class="" onclick="">
                                <td>
                                <%#Eval("isBelong").ToString() == "0" ? "<input name='' type='checkbox' checked value='' ltid='" + Eval("LTID").ToString() + "' />" : "<input name='' type='checkbox' value='' />"%>
                                </td>
                                <td class="cName"><%#Eval("Name") %></td>
                                <td>                                    
                                    <%#Eval("LTID").ToString().Trim() != MinLTID.ToString() ? "<a href=\"javascript:void(0)\" onclick=\"MoveUpOrDown('" + Eval("LTID").ToString() + "','up')\">上移</a>" : "<span style=\"color:#666;\">上移</span>"%>
                                    <%#Eval("LTID").ToString().Trim() != MaxLTID.ToString() ? "<a href=\"javascript:void(0)\" onclick=\"MoveUpOrDown('" + Eval("LTID").ToString() + "','down')\">下移</a>" : "<span style=\"color:#666;\">下移</span>"%>
                                </td>                                
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
        </table>--%>
        </div>
        <div class="clearfix"></div>
        <div class="btn"><input type="button" onclick="Save2DB()"  value="保存" class="save w60"/>&nbsp;&nbsp;&nbsp;&nbsp;<input type="button"  value="关闭" onclick="javascript:$.closePopupLayer('LabelConfigPop',false);" class="cancel w60 gray" /></div>
    </div>
</div>
</html>
