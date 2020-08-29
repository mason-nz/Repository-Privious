<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmotionForm.aspx.cs" Inherits="BitAuto.DSC.IM_DMS2014.Web.EmotionForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/css.css" rel="stylesheet" type="text/css" />
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/emotionstyle.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        //        function closeFrame() {
        //            window.top.window.document.getElementById("bq_listSH").click();
        //        };
        function setEmotionTab(name, cursel, n) {
            for (i = 1; i <= n; i++) {
                var menu = document.getElementById(name + i);
                var con = document.getElementById("con_" + name + "_" + i);
                menu.className = i == cursel ? "hover" : "";
                con.style.display = i == cursel ? "block" : "none";
            }
            getEmotionData(cursel);
        };

        $(function () {
            $(".emotionImgs").live("click", function () {
                var type = "<%=ReplyBoxID%>";
                switch (type) {
                    case "AgentChat":
                        $(window.top.window.document).find(".dialogue:visible .ask_t").append('<img src="' + $(this).attr("src") + '"/>');
                        break;
                    default:
                        $(window.top.window.document.getElementById("Smessage")).append('<img src="' + $(this).attr("src") + '" />');
                        break;

                }
                window.parent.changeImgLayerState();
            });
            //setEmotionTab('one', 2, 3);
        });
        function getEmotionData(ecategory) {

            if ($("#con_one_" + ecategory + " >ul >li").length <= 0) {
                $.get("/AjaxServers/LayerDataHandlerBefore.ashx", { Action: 'getemotioninfobyecategory', ECategory: ecategory }, function (data) {
                    if (data != "") {
                        var jsonData = $.evalJSON(data);
                        if (jsonData != "") {
                            var temphtml = "";
                            $.each(jsonData.root, function (idx, item) {
                                temphtml += '<li><img class="emotionImgs" title="' + item.EText + '" src="' + item.EUrl + '" /> </li>';
                            });
                            switch (ecategory) {
                                case 1: $("#con_one_1 >ul").html(temphtml); break;
                                case 2: $("#con_one_2 >ul").html(temphtml); break;
                                case 3: $("#con_one_3 >ul").html(temphtml); break;
                            }

                        }
                    }
                });
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="yc_Emotiondiv">
        <div class="imgContentbox">
            <!--易小车Start-->
            <div id="con_one_2" style="display: none;">
                <ul>
                </ul>
            </div>
            <!--易小车End-->
            <!--易小妹Start-->
            <div id="con_one_3" style="display: none;">
                <ul>
                </ul>
            </div>
            <!--易小妹End-->
            <!--其它Start-->
            <div id="con_one_1" style="display: none;">
                <ul>
                </ul>
            </div>
            <!--其它End-->
        </div>
        <div class="menuContentbox">
            <ul>
                <li id="one2" onclick="setEmotionTab('one',2,3)">易小车</li>
                <li id="one3" onclick="setEmotionTab('one',3,3)">易小妹</li>
                <li id="one1" onclick="setEmotionTab('one',1,3)">其它</li>
            </ul>
        </div>
    </div>
    </form>
</body>
</html>
