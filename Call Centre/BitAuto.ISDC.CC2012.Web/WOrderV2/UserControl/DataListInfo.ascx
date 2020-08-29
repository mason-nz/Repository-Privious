<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataListInfo.ascx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.UserControl.DataListInfo" %>
<script type="text/javascript">
    $(document).ready(function () {
        SetMenu();
        var iskhhf = Common.Params.ModuleSource == "<%=(int)BitAuto.ISDC.CC2012.Entities.ModuleSourceEnum.M03_客户回访 %>";
        var ishmc = Common.Params.CallSource == "<%=(int)BitAuto.ISDC.CC2012.Entities.CallSourceEnum.C01_呼入 %>" &&
                           Common.Params.DataSource == "<%=(int)BitAuto.ISDC.CC2012.Entities.WorkOrderDataSource.HMCLine %>";
        if (iskhhf) {
            //客户回访只显示客户信息
            $('#li_khhf').nextAll().hide();
            $("#r_bor_left").css("width", "0px");
            $("#r_bor_right").css("width", "0px");
            //加载客户信息
            changetab($("a[theid='客户']")[0], 'item');
        }
        else if (ishmc) {
            $('#li_khhf').nextAll().show();
            $("#r_bor_left").css("width", "3px");
            $("#r_bor_right").css("width", "3px");
            //默认加载订单记录
            changetab($("a[theid='惠买车']")[0], 'item');
        }
        else {
            $('#li_khhf').nextAll().show();
            $("#r_bor_left").css("width", "3px");
            $("#r_bor_right").css("width", "3px");
            //默认加载订单记录
            changetab($("a[theid='订单']")[0], 'item');
        }
    });

    function SetMenu() {
        $('ul.mail_nav li div').hover(
            function () {
                $(this).children('div.nav_selectSub21').show();
            },
            function () {
                $(this).children('div.nav_selectSub21').hide();
            }
        );

        $('ul.mail_nav li div').click(function () {
            $(this).children('div.nav_selectSub21').hide();
        });

        $(".rightcon div[name='tabclassdiv']").hover(function () {
            if (!$(this).hasClass("righttabdivon")) {
                $(this).addClass("righttabdivhover");
            }
        }, function () {
            $(this).removeClass("righttabdivhover");
        });

        $(".mail_nav a").hover(function () {
            var parentObj = $(this).parent().parent().parent().find(" div[name='tabclassdiv']")
            if (!parentObj.hasClass("righttabdivon")) {
                parentObj.addClass("righttabdivhover");
            }
        }, function () {
            var parentObj = $(this).parent().parent().parent().find(" div[name='tabclassdiv']")
            parentObj.removeClass("righttabdivhover");
        });
    }

    function tabchange(thisobj, triggerfireevent) {
        $(".rightcon .mail_nav div[name='tabclasslink']").each(function () {
            if ($(this).hasClass("righttablinkon")) {
                $(this).removeClass("righttablinkon");
            }
            if (!$(this).hasClass("righttablinkout")) {
                $(this).addClass("righttablinkout");
            }
        });
        $(".rightcon .mail_nav div[name='tabclassdiv']").each(function () {
            if ($(this).hasClass("righttabdivon")) {
                $(this).removeClass("righttabdivon");
            }
            if (!$(this).hasClass("righttabdivout")) {
                $(this).addClass("righttabdivout");
            }
        });
        if ($(thisobj).attr("name") == "tabclasslink") {
            if ($(thisobj).hasClass("righttablinkout")) {
                $(thisobj).removeClass("righttablinkout");
            }
            $(thisobj).addClass("righttablinkon");
        }
        else if ($(thisobj).attr("name") == "tabclassdiv") {
            if ($(thisobj).hasClass("righttabdivout")) {
                $(thisobj).removeClass("righttabdivout");
            }
            $(thisobj).addClass("righttabdivon");
            if (triggerfireevent) {
                clearitemselectedclass();
                var firstLink = $(thisobj).next().find(" a:first");
                firstLink.trigger('click');
            }
        }
    }

    function clearitemselectedclass() {
        $(".rightcon .cui_selectBox21 a").each(function () {
            if ($(this).hasClass("righttabdivitemselected")) {
                $(this).removeClass("righttabdivitemselected");
            }
        });
    }

    function changetab(thisobj, name) {
        if (name == "tab") {
            tabchange(thisobj, true);
        }
        else {
            tabchange($(thisobj).parent().parent().parent().find(" div[name='tabclassdiv']"), false);
            clearitemselectedclass();
            $(thisobj).addClass("righttabdivitemselected");

            //容器的url
            var IframeContainer = "/WOrderV2/AjaxServers/IframeContainer/IframeContainer.aspx?Url=";
            //用户输入
            var phone = $.trim($("#inp_phone").val());
            var url = $(thisobj).attr("thehref");
            if ($(thisobj).attr("theid") == "客户") {
                url = url + Common.Params.CRMCustID;
                IframeContainer += escape(url);
                $("#div_content").load(IframeContainer + "&height=817&r=" + Math.random());
            }
            else if ($(thisobj).attr("theid") == "订单") {
                $("#div_content").load(url + "&tel=" + phone + "&keyid=" + keyid + "&r=" + Math.random());
            }
            else if ($(thisobj).attr("theid") == "通用") {
                $("#div_content").load(url + "&PhoneNums=" + phone + "&r=" + Math.random());
            }
            else if ($(thisobj).attr("theid") == "易湃") {
                $("#div_content").load(url + "&tel=" + phone + "&r=" + Math.random());
            }
            else if ($(thisobj).attr("theid") == "惠买车") {
                var obj = new Object();
                obj.businessType = 'huimaiche';
                obj.YPFanXianURL = '<%=YPFanXianURL%>';
                obj.TaskURL = '<%=HuiMaiChe_InBound_Url%>';
                if (obj.TaskURL.indexOf('?') < 0) {
                    obj.TaskURL += '?';
                }
                obj.TaskURL += ('&phone=' + phone);
                obj.EPEmbedCC_APPID = '<%=EPEmbedCC_APPID %>';
                OtherBusinessLoginByIframe(obj, $('#div_content'), IframeContainer);
            }
        }
    }

    //加载订单
    var keyid = Math.ceil(Math.random() * 100000);
</script>
<div class="rightcon">
    <div class="r_bor_left" id="r_bor_left">
    </div>
    <!--右内容开始-->
    <div class="r_bor_center">
        <!--导航开始-->
        <ul class="mail_nav">
            <li id="li_khhf" style="display: none">
                <div class="cui_selectBox21 call_center current">
                    <div style="text-align: center;" name="tabclassdiv" class="cui_selectTitle21" style="background-image: none;
                        cursor: default;">
                        <span style="margin-right: 20px; display: block; margin-top: -2px;">客户信息</span></div>
                    <div style="display: none">
                        <p>
                            <a href="javascript:void(0)" thehref="<%=WebUrl %>/ReturnVisit/CustInfoShow.aspx?custid="
                                theid="客户" onclick="changetab(this,'item');" rel="">客户信息</a></p>
                    </div>
                </div>
            </li>
            <%--     <li>
                <div class="cui_selectBox21 call_center">
                    <div name="tabclasslink" class="cui_selectTitle21NoBackImg righttablinkout" onclick="changetab(this,'tab');">
                        <span>FAQ</span></div>
                </div>
            </li>--%>
            <li id="li_hjzx" style="display: none">
                <div class="cui_selectBox21 call_center current">
                    <div style="text-align: center;" name="tabclassdiv" class="cui_selectTitle21 righttabdivout"
                        onclick="changetab(this,'tab');">
                        <span style="margin-right: 20px; display: block; margin-top: -2px;">呼叫中心</span></div>
                    <div class="nav_selectSub21">
                        <p>
                            <a href="javascript:void(0)" thehref="/WOrderV2/AjaxServers/AddOrderTab/OrderInfoListForHB.aspx?Notext=false"
                                theid="订单" onclick="changetab(this,'item');" rel="">订单记录</a></p>
                        <p>
                            <a href="javascript:void(0)" thehref="/WOrderV2/AjaxServers/AddOrderTab/TaskHistoryRecord.aspx?do=1"
                                theid="通用" onclick="changetab(this,'item');" rel="">业务记录</a></p>
                        <p>
                            <a href="javascript:void(0)" thehref="/WOrderV2/AjaxServers/AddOrderTab/CallRecordORIG.aspx?do=1"
                                theid="通用" onclick="changetab(this,'item');" rel="">话务记录</a></p>
                        <p>
                            <a href="javascript:void(0)" thehref="/WOrderV2/AjaxServers/AddOrderTab/SMSSendHistoryList.aspx?do=1"
                                theid="通用" onclick="changetab(this,'item');" rel="">短信记录</a></p>
                        <p>
                            <a href="javascript:void(0)" thehref="/WOrderV2/AjaxServers/AddOrderTab/BlackWhiteListOperLogList.aspx?do=1"
                                theid="通用" onclick="changetab(this,'item');" rel="">免打扰记录</a></p>
                    </div>
                </div>
            </li>
            <li id="li_yp" style="display: none">
                <div class="cui_selectBox21 call_center">
                    <div style="text-align: center;" name="tabclassdiv" class="cui_selectTitle21 righttabdivout"
                        onclick="changetab(this,'tab');">
                        <span style="margin-right: 20px; display: block; margin-top: -2px;">易湃</span></div>
                    <div class="nav_selectSub21">
                        <p>
                            <a href="javascript:void(0)" thehref="/WOrderV2/AjaxServers/OrderYPTab/DealerInfo.aspx?areaorderinfo=text_content"
                                theid="易湃" onclick="changetab(this,'item');" rel="">经销商查询</a></p>
                    </div>
                </div>
            </li>
            <li id="li_huimaiche" style="display: none">
                <div class="cui_selectBox21 call_center">
                    <div style="text-align: center;" name="tabclassdiv" class="cui_selectTitle21 righttabdivout"
                        onclick="changetab(this,'tab');">
                        <span style="margin-right: 20px; display: block; margin-top: -2px;">惠买车</span></div>
                    <div class="nav_selectSub21">
                        <p>
                            <a href="javascript:void(0)" thehref="" theid="惠买车" onclick="changetab(this,'item');"
                                rel="">惠买车</a></p>
                    </div>
                </div>
            </li>
            <%--<li><a href="javascript:void(0)" class="ershouche">二手车</a></li>
            <li><a href="javascript:void(0)" class="huimaiche">惠买车</a></li>
            <li><a href="javascript:void(0)" class="yichehui">易车惠</a></li>--%>
        </ul>
        <!--导航结束-->
        <div class="clearfix">
        </div>
        <!--内容开始-->
        <div id="div_content">
        </div>
        <!--内容结束-->
    </div>
    <!--右内容结束-->
    <div class="r_bor_right" id="r_bor_right">
    </div>
</div>
