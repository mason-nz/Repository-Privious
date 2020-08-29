<%@ Page Title="添加备注页" Language="C#" AutoEventWireup="true" CodeBehind="AddRemarkForm.aspx.cs" 
Inherits="BitAuto.DSC.IM_2015.Web.AjaxServers.ContentManage.AddRemarkForm" %>

    <script type="text/javascript">

        function saveRemarkInfo() {
            $("#saveremarinfBT").attr("disabled", "disabled");
            var msg = judgeRemarkInfoIsSuccess();
            if (msg != "") {
                $.jAlert(msg);
                $("#saveremarinfBT").removeAttr("disabled");
                return false;
            }
            var hidRecID = encodeURIComponent($("#hidRecID").val());
            var taRemarkContents = encodeURIComponent($("#<%=taRemarkContents.ClientID%>").val());

            $.post("/AjaxServers/LayerDataHandler.ashx", { Action: 'addremarkinfo', RemarkRecID: hidRecID, RemarkDetails: taRemarkContents, r: Math.random() }, function (data) {
                if (data == "success") {
                    $.jAlert("操作成功！");
                    $.closePopupLayer('AddRemarkLayerAjaxPopup', true);
                    reloadpagedata();  //--------------------------------------------------------------------------------------
                }
                else {
                    $.jAlert(data);
                }
            });

        }
        //验证数据格式
        function judgeRemarkInfoIsSuccess() {
            var msg = "";
            var taRemarkContents = $("#<%=taRemarkContents.ClientID%>").val();
            if (taRemarkContents == "") {
                msg = "请填写备注内容<br/>";
            }
            if (taRemarkContents.length > 1000) {
                msg = "备注内容长度不能超过1000个字符<br/>";
            }

            return msg;
        }
        function closeAddRemarkLayerAjaxPopup() {
            $.closePopupLayer('AddRemarkLayerAjaxPopup');
        }
    </script>
<div class="online_kf online_kf2" style=" background-color:#E4E4E4;width:400px;">
    <div class="title_kf">
 
        备注<span><a href="#" onclick="closeAddRemarkLayerAjaxPopup()"><img src="/Images/c_btn.png" border="0" alt="关闭" /></a></span></div>
    <div class="content_kf content_kf_ms" style=" height:200px;">
        <!--左开始-->
        <div class="left_c">
            <div class="answer answer2" style=" height:90%">

                <table border="0" cellspacing="0" cellpadding="0" class="msg_lv" width="100%" style=" margin:0px;">
                    <tr>
                        <th>
                            提交人：
                        </th>
                        <td>
                            <input disabled="disabled" type="text" value="" class="w180" style=" width:250px;" id="txtAddRemarkUser"  runat="server"/> 
                            <input type="hidden" value="<%=RecID%>"  id="hidRecID" />        
                        </td>
                    </tr>                      
                    <tr>
                        <th style="vertical-align: top;">
                            <span class="red">*</span>备注：
                        </th>
                        <td>
                            <textarea name="" id="taRemarkContents" style=" width:244px; height:70px;"  runat="server"></textarea>
                        </td>
                    </tr>
                </table>
                <div class="btn submit">
                    <input type="button" value="保存" id="saveremarinfBT" runat="server" onclick="javascript:saveRemarkInfo()" class="w80" />
                </div>
            </div>
        </div>
        <!--左结束-->
    </div>
    <div class="clearfix">
    </div>
</div>
