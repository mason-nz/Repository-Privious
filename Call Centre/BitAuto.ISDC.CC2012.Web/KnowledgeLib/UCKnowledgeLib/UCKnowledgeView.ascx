<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCKnowledgeView.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib.UCKnowledgeView" %>
<%@ Import Namespace="System.IO" %>
<script src="../../Js/jquery.media.js" type="text/javascript"></script>
<%--<script src="http://github.com/malsup/media/raw/master/jquery.media.js?v0.92" type="text/javascript"></script>--%>
<script src="../../Js/jquery.metadata.js" type="text/javascript"></script>
<script type="text/javascript">

    var pdfOverIframeStr='<iframe id="pdfOverIframe" style="left: 200px; top: 200px; width: 1000px; height: 800px; position: absolute; z-index: 1;"></iframe>';
    $(document).ready(function () {

        setTimeout(function () {
            var fileUrlT = "<%=FileUrl %>";
            if ('<%=IsFileExists %>'=='1') {
                if ('<%=IsMedia %>'=='1') {
                            $('#pdfReader').attr('href', fileUrlT);
                            $.fn.media.mapFormat(fileUrlT.substr(fileUrlT.length-3), 'winmedia');
                            $('#pdfReader').media({ width: 800, height: 63, autoplay: true });
                    $('#pdfReader').css({ "margin-left": "50px" });
                } else {
                    if (fileUrlT.substr(fileUrlT.length - 3) == "pdf") {
                        //if (CheckReader.hasGeneric() || CheckReader.hasReader() || CheckReader.hasReaderActiveX()) {
                            $('#pdfReader').media({
                                width: 910,
                                height: 500,
                                src:fileUrlT 
                            });
//                        } else {
//                            $('#pdfReader').html("你浏览器不支持Reader，请下载插件 <a href='http://get.adobe.com/cn/reader/' target='_blank'>Adobe Reader</a>");
//                        }
                    } else {
                         $('#pdfReader').hide();
                    }
                }
            } else {
                $('#pdfReader').hide();
            }
        }, 100);

        var isHaveFiles = '<%=IsHaveFiles %>'; //是否有附件
        if (isHaveFiles == "") {
            $("#labFiles").text("");
        }      

        $('#aDownload').click(function () {
            if (<%=IsFileExists %>) {
                 try {
                var thsSpan$ = $('#donCunt');
                var countNow = parseInt(thsSpan$.text());

                $.post("../AjaxServers/KnowledgeLib/KnowledgeLibList.ashx", { Action: 'AddKLClickAndDownloadCount', r: Math.random(), type: 1, KLID: <%=KID %> }, function () {
                    countNow++;
                    thsSpan$.text(countNow);
                });
            } catch (e) {

            }
                  RefreshPDF();       
            }
        });

        //添加收藏
        $('#addFavorite').click(function() {
        $('body').prepend(pdfOverIframeStr);
            $.post("/KnowledgeLib/Personalization/PersonalizationHandler.ashx", { Action: "addcollection", CollectRefId: <%=KID %>,CollectType:0, r: Math.random() }, function (data) {
            if (data == "success") {
                        $.jPopMsgLayer("收藏成功！", function () {
                        $('#pdfOverIframe').remove();
                        RefreshPDF();
                        });
                $.closePopupLayer('AddNewQuestionAjaxPopup');
            }
            else {
                $.jAlert(data, function () {
                        $('#pdfOverIframe').remove();
                        RefreshPDF();
                        });
            }
        });
        });

        //提问
        $('#aQuestion').click(function() {
             $.openPopupLayer({
                    name: "AddNewQuestionAjaxPopup",
                    parameters: {},
                    url: "/AjaxServers/KnowledgeLib/AddQuestion.aspx?KLType=0&KLID=<%=KID %>&r=" + Math.random(),
                    success:function(){
                     $('body').prepend(pdfOverIframeStr);
                    },
                    afterClose:function(){
                      $('#pdfOverIframe').remove();
                    }
                });

            RefreshPDF();
        });

        
         var hdIDs = '<%=hdAbs.ClientID %>';
        var thdValue = $('#' + hdIDs).val();

        var fontNum = 70;
        if(thdValue.length>fontNum) {
            $('#pabs').html(thdValue.substr(0,fontNum) + '....');
            $('#arro').show();
        } else {
            $('#pabs').html(thdValue);
            $('#arro').hide();
        }
        
        
        $('#arro').click(function(eve) {
            $('.pNone').toggle();
            var ths$ = $(this);
            if (ths$.hasClass('dnowup')) {
                $(this).removeClass("dnowup").addClass("updnow");
                 
                    $('#pabs').html(thdValue);
            } else {
                 $(this).removeClass("updnow").addClass("dnowup");
                 if(thdValue.length>fontNum) {
                        $('#pabs').html(thdValue.substr(0,fontNum) + '....');
                    } else {
                        $('#pabs').html(thdValue);
                    }
            }
        });

    });

    function RefreshPDF() {
         if ($.browser.msie) {
                    var hrefT = '<%= FileUrl %>';
                    if (hrefT.substr(hrefT.length - 3) == "pdf") {
                        $('#pdfReader iframe').attr("src", hrefT);
                    }
                }
    }
    function ChangWidth() {
        var MaxWidth = 910;
        $("[id$='txtContext'] table").each(function (i, v) {

            if (Number($(this).width()) > MaxWidth) {
                $(this).css("width", "")
                $(this).attr("width", MaxWidth);
            }
            var countWidth = 0;
            var xWidth = 0;
            $(this).find("td").each(function (ti, tv) {
                countWidth = countWidth + Number($(this).width());
            }
            );

            xWidth = countWidth - MaxWidth;
            if (xWidth > 0) {
                //TD宽度总数大于最大宽度
                var m = (countWidth - MaxWidth) / countWidth; //比例

                $(this).find("td").each(function (ti, tv) {
                    $(this).css("width", "");
                    $(this).attr("width", $(this).attr("width") * m);
                });
            }

        }
         );

    }

    var CheckReader = {
        createAXO: function (type) {
            var ax;
            try {
                ax = new ActiveXObject(type);
            } catch (e) {
                ax = null;
            }
            return ax;

        },
        //Tests specifically for Adobe Reader (aka Acrobat) in Internet Explorer
        hasReaderActiveX: function () {
            var axObj = null;
            if (window.ActiveXObject) {
                axObj = CheckReader.createAXO("AcroPDF.PDF");
                //If "AcroPDF.PDF" didn't work, try "PDF.PdfCtrl"
                if (!axObj) { axObj = CheckReader.createAXO("PDF.PdfCtrl"); }
                //If either "AcroPDF.PDF" or "PDF.PdfCtrl" are found, return true
                if (axObj !== null) { return true; }
            }
            //If you got to this point, there's no ActiveXObject for PDFs
            return false;
        },
        //Tests specifically for Adobe Reader (aka Adobe Acrobat) in non-IE browsers
        hasReader: function () {
            var i,
            n = navigator.plugins,
            count = n.length,
            regx = /Adobe Reader|Adobe PDF|Acrobat/gi;
            for (i = 0; i < count; i++) {
                if (regx.test(n[i].name)) {
                    return true;
                }
            }
            return false;
        },
        //Detects unbranded PDF support
        hasGeneric: function () {
            var plugin = navigator.mimeTypes["application/pdf"];
            return (plugin && plugin.enabledPlugin);
        }
    };
</script>
<style type="text/css">
    .pNone
    {
        display: none;
    }
</style>
<div class="article">
    <h2 id="txtTitle" runat="server" style="font-family: 宋体;">
    </h2>
    <div class="fbt">
        <span>
            <asp:Label runat="server" ID="lbCreateTime"></asp:Label></span> <span>分类:<asp:Label
                runat="server" ID="lbC"></asp:Label></span> <span style="margin-right: 5px;">&nbsp;阅读:<asp:Label
                    runat="server" ID="lbClickCount"></asp:Label>次</span> <span>&nbsp;下载:&nbsp;<em id="donCunt"><asp:Label
                        runat="server" ID="lbDownCount"></asp:Label></em>次&nbsp;</span>
        <span><a href="javascript:void(0);" id="addFavorite">收藏&nbsp;</a>
            <img src="/Images/sc.png" /></span> <span><a href="javascript:void(0);" id="aQuestion">
                提问</a>
                <img src="/Images/tw.png" /></span>
        <% if (IsFileExists == 1)
           { %>
        <span><a href='<%=BindDownLoadA %>' target="_self" id="aDownload">下载
            <img src="/Images/xz.png" /></a> </span>
        <% } %>
    </div>
    <div class="content" style="position: relative;">
        <p style="overflow: hidden; display: inline;" id="pabs">
        </p>
        <span class='dnowup' id='arro' style="top: 12px; position: absolute; right: -8px;
            display: none;">&nbsp; </span>
    </div>
    <div id="txtContext" runat="server">
    </div>
    <div class="attach">
        <asp:Repeater runat="server" ID="rpfileList">
            <ItemTemplate>
                <span><a href='<%=BindDownLoadA %>' target="_blank">
                    <%#Eval("Filename")%><%#Eval("ExtendName")%></a></span>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="content" style="overflow: auto;">
        <a class="textArea" id="pdfReader" >
        </a>
    </div>   
    <asp:HiddenField runat="server" ID="hdAbs" />
</div>
