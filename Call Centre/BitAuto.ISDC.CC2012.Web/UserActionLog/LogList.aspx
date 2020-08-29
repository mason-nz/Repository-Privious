<%@ Page Language="C#" Title="操作日志" AutoEventWireup="true" MasterPageFile="~/Controls/Top.Master"
    CodeBehind="LogList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.UserActionLog.LogList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
  <script type="text/javascript" src="../Js/swfobject.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            $('#tfStartTime').bind('click focus', function () { WdatePicker({ maxDate: '#F{$dp.$D(\'tfEndTime\')}', onpicked: function () { document.getElementById("tfEndTime").focus(); } }); });
            $('#tfEndTime').bind('click focus', function () { WdatePicker({ minDate: '#F{$dp.$D(\'tfStartTime\')}' }); });
         
            //敲回车键执行方法
            enterSearch(search);

            search();
            
        });

        //查询
        function search() {
            var startTime = $.trim($("#tfStartTime").val());
            var endTime = $.trim($("#tfEndTime").val());
            if (startTime != "") {
                if (!startTime.isDate()) {
                    $.jAlert("时间格式不正确！");
                    return false;
                }
            }
            if (endTime != "") {
                if (!endTime.isDate()) {
                    $.jAlert("时间格式不正确！");
                    return false;
                }
            }
            var userName = $.trim($("#userName").val());

            var loginInfo = $.trim($("#tfLoginInfo").val());

            var pody = {
                StartTime: escapeStr(startTime),
                EndTime: escapeStr(endTime),
                usrName: escape(userName),
                LoginInfo: escapeStr(loginInfo)
            };

            LoadingAnimation("ajaxTable");
            $("#ajaxTable").load("/UserActionLog/UserActionLogList.aspx", pody);
        }

        //分页操作
        function ShowDataByPost1(pody) {
            $('.bit_table').load('/UserActionLog/UserActionLogList.aspx .bit_table > *', pody, null);
        }

        //导出4s和非4s数据
        function ExprotDataBy4sAndNot4sPopLayer() {
            $.openPopupLayer({
                name: "ExprotDataBy4sAndNot4s",
                //parameters: { TypeId: "2" },
                url: "/UserActionLog/ExprotDataLayer.aspx",
                beforeClose: function (e, data) {
                    //window.location.reload();
                }
            }); 
        }
    </script>
    <div class="search">
        <ul class="clearfix" style=" padding-top:2px;">
            <li>
                <label>
                    时间：</label>
              <input type="text" name="StartTime" id="tfStartTime" class="w90"  
                    style="width: 85px;"   />
                至
                  <input type="text" name="EndTime" id="tfEndTime"  class="w90"  style="width: 85px;"
                      />
            </li>
            <li>
                <label>
                    操作人：</label>
                <input style="width: 120px;" type="text" id="userName" value="" class="w190" class="text" />
            </li>
            <li>
                <label>
                    描述：</label>
                <input type="text" name="LoginInfo" id="tfLoginInfo" style="width: 120px;"
                    class="w190" />
            </li>
            <li class="btnsearch" style="width:100px;">
                <input type="button" value="查 询" class="searchBtn bold" onclick="search()" name=""  />
                
                </li>
        </ul>
    </div>
    <div class="optionBtn clearfix">
        <input type="button" onclick="ExprotDataBy4sAndNot4sPopLayer()" class="newBtn" value="导出" />        
    </div>
    <div id="ajaxTable">
    </div>
    <input type="hidden" id="hidFieldsCustomer" value="" />
    <input type="hidden" id="hidIsOkOrCancel" value="0" />
</asp:Content>
