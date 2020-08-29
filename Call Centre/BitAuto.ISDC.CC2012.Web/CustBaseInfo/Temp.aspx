<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Temp.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.CustBaseInfo.Temp" %>

<%@ Register Src="CustContactRecord.ascx" TagName="CustContactRecord" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="../Js/jquery-1.4.4.min.js"></script>
    <script type="text/javascript" src="../Js/common.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //选择
            $("input[name='mytype']").click(function () { ShowPage($(this).attr("typename")); });

            //添加
            $("#btnAdd").click(function () { AddInfo(); });

            //导出
            $("#btnExport").click(function () { ExportExcel(); });

            //
           

        });

      

        function ExportExcel() {

            var field = "CustBasicInfo.CustName,CustBasicInfo.CustID,CustBasicInfo.Sex,CustBasicInfo.ProvinceID,CustBasicInfo.CityID,CustBasicInfo.CountyID,CustBasicInfo.AreaID,dbo.GetTelByCustID(CustBasicInfo.CustID) as Phone,dbo.GetEmailByCustID(CustBasicInfo.CustID) AS Email,CustBasicInfo.Address,CustBasicInfo.DataSource,CustBasicInfo.CustCategoryID,BuyCarInfo.Age,BuyCarInfo.Vocation,BuyCarInfo.IDCard,BuyCarInfo.Marriage,BuyCarInfo.Income,BuyCarInfo.CarBrandId,BuyCarInfo.CarSerialId,BuyCarInfo.IsAttestation,BuyCarInfo.DriveAge,BuyCarInfo.UserName,BuyCarInfo.CarNo,BuyCarInfo.Remark,DealerInfo.Name,DealerInfo.CityScope,DealerInfo.MemberType,DealerInfo.CarType,dbo.GetBrandListByCustID( CustBasicInfo.CustID) as BrandID,DealerInfo.MemberCode,DealerInfo.MemberStatus,DealerInfo.Remark,CustBasicInfo.CreateUserID,CustBasicInfo.CreateTime,CustBasicInfo.CallTime";
            var pody = { where: '', field: field, t: "cust" };
            AjaxPost('/AjaxServers/ExcelOperate/ExcelExport.ashx', pody,
             null,
             function (data) { alert("返回值：" + data); });
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="Checkbox11" type="radio" value="checkbox" name="mytype" typename="type1" />新车
        <input id="Checkbox12" type="radio" value="checkbox" name="mytype" typename="type2" />二手车
        <input id="Checkbox13" type="radio" value="checkbox" name="mytype" typename="type3" />个人反馈
        <input id="Checkbox14" type="radio" value="checkbox" name="mytype" typename="type4" />活动
        <input id="Checkbox15" type="radio" value="checkbox" name="mytype" typename="type5" />个人用车
        <input id="Checkbox16" type="radio" value="checkbox" name="mytype" typename="type6" />个人其他
        <input id="Checkbox17" type="radio" value="checkbox" name="mytype" typename="type7" />经销商合作
        <input id="Checkbox18" type="radio" value="checkbox" name="mytype" typename="type7" />经销商反馈
        <input id="Checkbox19" type="radio" value="checkbox" name="mytype" typename="type7" />经销商其它
        <br />
        <input type="button" id="btnAdd" value="添加" />
        <input type="button" id="btnExport" value="导出" />
        <br />
        <br />
        <hr />
        <br />
        <uc1:CustContactRecord ID="CustContactRecord1" runat="server" />
    </div>

    <br />
    <ul class="clearfix ft14">
                <li>
                    <label>
                        咨询类型：</label><select class="w125"></select><select class="w125"></select></li>
                <li>
                    <label>
                        推荐经销商:</label><input type="text" class="w190" /></li>
                <li>
                    <label>
                        购车预算:</label><input type="text" class="w190" /></li>
                <li>
                    <label>
                        推荐活动:</label><input type="text" class="w190" /></li>
                <li>
                    <label>
                        购车时间：</label><select class="w125"></select></li>
                <li>
                    <label>
                        新购/置换:</label><input type="radio" name="new" />新购<input type="radio" name="new" style="margin-left: 50px;" />置换</li>
                <li>
                    <label>
                        来电记录:</label><textarea rows="5" cols="40"></textarea></li>
                <li>
                    <label>
                        来电时间:</label><span>2012-07-28 17：25:30</span></li>
            </ul>
    </form>
</body>
</html>
