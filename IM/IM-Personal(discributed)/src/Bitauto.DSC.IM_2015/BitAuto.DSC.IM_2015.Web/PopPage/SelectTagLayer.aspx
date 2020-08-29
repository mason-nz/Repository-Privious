<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectTagLayer.aspx.cs"
    Inherits="BitAuto.DSC.IM_2015.Web.PopPage.SelectTagLayer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/css.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.css?r=23232323" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        $(function () {
            BusiTypeChange();
        });

        function SelectTabID(liID) {
            //样式
            $(liID).siblings().attr("class", ""); //其他的取消选中
            $(liID).attr("class", "current"); //选中
            //数据
            var tagid = $(liID).attr("tagid");
            var tagname = $(liID).attr("tagname");
            var parenttagname = $(liID).attr("parentname");
            var busiTypeId = $("select[id$='ddlBussyType']").val();
            var busiName = $("select[id$='ddlBussyType']").find("option:selected").text();
            var returnName = busiName + '--' + parenttagname + '--' + tagname;
            var data = { tagid: tagid, tagname: returnName, busiTypeId: busiTypeId };
            $.closePopupLayer('TagSelectPopNew', true, data);
        }
        function setCCTab(name, cursel, n) {
            for (i = 1; i <= n; i++) {
                var menu = document.getElementById(name + i);
                var con = document.getElementById("con_" + name + "_" + i);
                menu.className = i == cursel ? "hover" : "";
                con.style.display = i == cursel ? "block" : "none";
            }
        }

        function SelectClear() {
            var data = { tagid: '0', tagname: '', busiTypeId: '0' };
            $.closePopupLayer('TagSelectPopNew', true, data);
        }

        function BusiTypeChange() {
            var busiType = $("select[id$='ddlBussyType']").val();
            var pody = { action: 'getTabInfo', BusiTypeID: escape(busiType), r: Math.random() };
            LoadingAnimation("divleft");
            LoadingAnimation("Lit_right");
            AjaxPost("/AjaxServers/LayerDataHandler.ashx", pody, null,
             function (msg) {
                 if (msg != "") {
                     var jsonData = $.evalJSON(msg);
                     var fristA = new Array();
                     $.each(jsonData, function (i) {
                         var obj = jsonData[i];
                         if (obj["PID"] == "0") {
                             fristA.push(obj);
                         }
                     });
                     var htmlStr = "<ul>";
                     var oneSelected = '<%=ParentID %>';
                     for (var i = 0; i < fristA.length; i++) {
                         var selectStyle = "";
                         var recidOne = fristA[i]["RecID"];
                         if (recidOne == oneSelected)//已选中
                         {
                             selectStyle = " class='hover'";
                         }
                         var n = i + 1;
                         htmlStr += "<li " + selectStyle + " id=\"cctabone" + n + "\" onclick=\"setCCTab('cctabone'," + n + "," + fristA.length + ")\" >" + fristA[i]["TagName"] + "</li>";
                     }
                     htmlStr += "</ul>";
                     $("#divleft").html(htmlStr);

                     var secondHtmlStr = "<ul>";
                     var TagId = '<%=TagId %>';
                     for (var i = 0; i < fristA.length; i++) {
                         var fristTag = fristA[i];
                         var n = i + 1;
                         var secondA = new Array();
                         $.each(jsonData, function (i) {
                             var obj = jsonData[i];
                             if (obj["PID"] == fristTag["RecID"]) {
                                 secondA.push(obj);
                             }
                         });
                         if (fristTag["RecID"] == oneSelected) {
                             secondHtmlStr += "<div class=\"hover\" id=\"con_cctabone_" + n + "\"  style='display: block;'>"
                         }
                         else {
                             secondHtmlStr += "<div id=\"con_cctabone_" + n + "\"  style='display: none;'>"
                         }
                         secondHtmlStr += "<ul>";
                         for (var j = 0; j < secondA.length; j++) {
                             var selectStyle = "";
                             var recid = secondA[j].RecID;
                             if (TagId != "" && recid == TagId)//已选中
                             {
                                 selectStyle = " class=\"current\"";
                             }
                             secondHtmlStr += " <li " + selectStyle + " onclick=\"javascript:SelectTabID($(this))\" tagid=\"" + secondA[j]["RecID"] + "\" parentname=\"" + fristTag["TagName"] + "\" tagname=\"" + secondA[j]["TagName"] + "\"><a href='#'>" + secondA[j]["TagName"] + "</a></li>";
                         }
                         secondHtmlStr += "</ul>";
                         secondHtmlStr += "</div>";
                     }
                     secondHtmlStr += "</ul>";
                     $("#divright").html(secondHtmlStr);

                     var reqbusiType = '<%=BusiTypeId %>';
                     if (busiType != reqbusiType) {
                         //默认选中第一个
                         $(".Menubox_left ul li").first().attr("class", "hover");
                         $(".Contentbox_right div").first().attr("class", "hover").css("display", "block");
                     }
                 }
             });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pop_new w900">
        <div class="title">
            <h2>
                标签选择</h2>
            <a href="#" onclick="javascript:$.closePopupLayer('TagSelectPopNew',false);"></a>
        </div>
        <div class="search">
            <label>
                业务类型</label>
            <span>
                <select class="w180" onchange="BusiTypeChange()" id="ddlBussyType" runat="server">
                </select>
            </span>
        </div>
        <div id="divContent" class="biaoqian_xz">
            <div class="Menubox_left" id="divleft">
                <asp:Literal ID="Lit_left" runat="server"></asp:Literal>
            </div>
            <div class="Contentbox_right" id="divright">
                <asp:Literal ID="Lit_right" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="clearfix">
        </div>
        <div class="option_button  btn">
            <input name="" type="button" onclick="javascript:$.closePopupLayer('TagSelectPopNew',false);"
                value="取消" /><span style="position: relative; *top: -4px;"><a href="#" onclick="SelectClear()">清空已选项</a></span>
        </div>
        <div class="clearfix">
        </div>
    </div>
    </form>
</body>
</html>
