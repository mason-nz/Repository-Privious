<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCCstMemberUCount.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.DetailV.UCCstMemberUCount" %>
 <script type="text/javascript">
        $(document).ready(
            function () {
                loadCSTAccountInfo<%=CstMemberID() %>();
            }
        );
        function loadCSTAccountInfo<%=CstMemberID() %>() {
            var where = "";
            var cstMemberID = "<%=CstMemberID() %>";
            where += '&CSTMemberID=' + escape(cstMemberID);
            LoadingAnimation('cstAccountInfo_' + cstMemberID);
            $("#cstAccountInfo_" + cstMemberID).load("../../AjaxServers/CSTMember/CSTAccountInfo.aspx?r=" + Math.random(), where, AccountSuccessLoaded);
        }
        function AccountSuccessLoaded() {
        }
  </script>
  
        <div class="spliter">
        </div>
     <ul>
                    <li>
                        <label style="width: 110px;">
                            车商币余额：</label><%=UCount%></li>
                    <li>
                        <label style="width: 110px;">
                            累计充值车商币：</label><%=UbTotalAmount%></li>
                    <li>
                        <label style="width: 110px;">
                            累计消费车商币：</label><%=UbTotalExpend%></li>
                    <li>
                        <label style="width: 110px;">
                            最后充值时间：</label><%=lastAddUbTime%></li>
                    <li>
                        <label style="width: 110px;">
                            有效截止日：</label><%=activeTime%></li>
                    <li>
                        <label style="width: 110px;">
                            开通时间：</label><%=syncTime%></li>
                </ul>
  <div class="khinfo">
                <h3 style="background-color: #e1e1e1; margin: 5px 0 5px 0; padding: 5px;">
                    车商通产品开通情况</h3>
                <div>
                    <iframe width="100%" height="260" frameborder="0" scrolling="no" src="http://ma.ucar.cn/webservice/dealerinfo.aspx?tvaid=<%=CstMemberID() %>&key=<%=productOpenKey %>">
                    </iframe>
                </div>
            </div>
            <div class="khinfo" style="background-image: none">
                <h3 style="background-color: #e1e1e1; margin: 5px 0 5px 0; padding: 5px;">
                    车商通账号</h3>
                <div id="cstAccountInfo_<%=CstMemberID() %>">
                </div>
            </div>