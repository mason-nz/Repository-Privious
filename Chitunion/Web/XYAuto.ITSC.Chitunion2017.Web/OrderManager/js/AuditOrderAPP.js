/**
 * Created by chengj on 2017/3/3.
 */
 $(function () {
    /*公共方法，获取媒体类型和订单ID等信息*/
    function GetRequest() {
        var url = location.search; //获取url中"?"符后的字串
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
            }
        }
        return theRequest;
    }

    // 判断标志
    var flag = false;
    var orderData = GetRequest();
    /*获取订单号和媒体类型*/
    var orderid = orderData.orderID;
    var MediaType = orderData.MediaType;
    //通过订单ID查询订单列表
    getOrderList(orderid);
    /*检测是否设置排期*/
    schedulNum();
    //表单验证
    $("input:text[name='textUsername']").blur(function () {
        checkUsername();
    })
    $("#BeginData").click(function () {
        laydate({
            elem: '#BeginData',
            fixed: false,
            istime: true,
            isclear: true,
            format: 'YYYY-MM-DD',
            choose: function (date) {
                checkTime();
                if(date > $("#EndData").val()){
                    layer.alert('起始时间不能大于结束时间');
                    $("#BeginData").val("");
                }else{
                    if(!compare(date,$("#EndData").val())){
                        layer.alert('周期不能大于半年');
                        $("#BeginData").val("");
                    }
                }
                /*审核时点击执行周期清空选定的排期信息*/
                clearSchedule();
                GetOrderPrice();
            }
        }
        )
    })
    $("#EndData").click(function () {
        laydate({
            elem: '#EndData',
            fixed: false,
            isclear: true,
            format: 'YYYY-MM-DD',
            choose: function (date) {
                checkTime();
                if (date < $("#BeginData").val()) {
                    layer.alert('结束时间不能小于开始时间');
                    $("#EndData").val("");
                }else{
                    if(!compare($("#BeginData").val(),date)){
                        layer.alert('周期不能大于半年');
                        $("#EndData").val("");
                    }
                }
                clearSchedule();
                GetOrderPrice();
            }
        }
        )
    })
    $(".textArea").blur(function () {
        checkExplain();
    })

    $("#agreementBox").change(function () {
        checkAgreementBox();
    })
    /*点击删除某一项数据*/
    $("#tableList").on("click", ".delBtn", function () {
        var tablistLength = $("#tableList").find(".tableList").length;
        // console.log(tablistLength);
        if(tablistLength <= 1){
            layer.confirm('不能删除项目中的最后一个媒体', {
                time: 0 //不自动关闭
                , btn: ["关闭"]
            });
        }else{
            //信息框-例2
            var thisTable = $(this)
            layer.confirm('是否删除此项？', {
                time: 0 //不自动关闭
                , btn: ['删除', '取消']
                , yes: function (index) {
                    var MediaID = thisTable.parent().parent().attr("MediaID");
                    var PublishDetailID = thisTable.parent().parent().attr("PublishDetailID");
                    Operate_ShoppingCart(14002, MediaID, PublishDetailID)
                    thisTable.parent().parent().remove();
                    GetMeidaNum();
                    var orderList = GetMeidaNum();
                    if (orderList == 0) {
                        $("#nextBtn").remove();
                    }
                    GetOrderPrice();
                    checkCPMCPD();
                    layer.close(index);
                    layer.msg('操作成功', {time: 400});
                }
            });
        }

    })

    /*全选*/
    $('#tableList').on('change', '#allbox', function () {
        if ($(this).prop('checked')) {
            $('.onebox').prop('checked', true);
        } else {
            $('.onebox').prop('checked', false);
        }
    });
    $('#tableList').on('change', '.onebox', function () {
        if ($('.onebox').length == $("input[name='checkbox']:checked").length) {
            $('#allbox').prop('checked', true);
        } else {
            $('#allbox').prop('checked', false);
        }
    });
    GetMeidaNum()
    /*设置排期*/
    $('#tableList').on('click', '.setData', function () {
        /*绑定this*/
        var that = $(this);
        if (timeInterval() == false) {
            layer.confirm('执行周期不能为空', {
                time: 0 //不自动关闭
                , btn: ["关闭"]
            });
        } else {
            $.openPopupLayer({
                name: "popLayerDemo",
                url: "/OrderManager/schedulePopupAPP.html?r=" + Math.random(),
                success: function () {
                    $("#selectNum").text("广告位数量：1个");
                    //获取开始日期和结束日期
                    var beginDay = $("#BeginData").val();
                    var endDay = $("#EndData").val();
                    //调用设置排期方法，显示对应月份日期
                    setSchedule(beginDay, endDay);
                    /*页面加载完成进行的数据操作*/
                    function getDataArr() {
                        var dataA = [];
                        var selectList = $(".select");
                        for (var i = 0; i < selectList.length; i++) {
                            dataA.push($(selectList[i]).parent().attr("data"));
                        }
                        return dataA;
                    }

                    /*二次点击设置排期时显示已设置的排期*/
                    displaySchedule(that);
                    /*弹窗出现后显示投放对应天数*/
                    var dataArr = getDataArr();
                    $(".ml20").text("投放天数:" + dataArr.length + "天");
                    var orderList = $(".choseLi")
                    for (var i = 0; i < orderList.length; i++) {
                        $(orderList[i]).click(function () {
                            if ($(this).children().attr("class") === "select") {
                                $(this).children().removeClass("select")
                            } else {
                                $(this).html("<span>" + $(this).text() + "</span>")
                                $(this).children().addClass("select")
                            }
                            var dataArr = getDataArr();
                            $(".ml20").text("投放天数:" + dataArr.length + "天");
                        })
                    }
                    /*获取被选中的标签*/
                    $(".getData").click(function () {
                        var deliveryDays = $(".ml20").text().substr(5, 1);
                        // console.log(deliveryDays);
                        if (deliveryDays == 0) {
                            /*显示弹窗，提示用户投放天数不能为空*/
                            layer.confirm('投放天数不能为空', {
                                time: 0 //不自动关闭
                                , btn: ["关闭"]
                            });
                        } else {
                            /*关闭弹层*/
                            $.closePopupLayer("popLayerDemo");
                            var dataArr = getDataArr();
                            /*返回选中的日期*/
                            /*处理返回的日期数组 若为单日，返回单日，若为日期区间，转换为开始-结束的形式*/
                            var pushdata = outPutDate(dataArr);
                            /**所有已选广告位的当前行 数组*/
                            var arr = that.parents("tr");
                            for (var i = 0; i < arr.length; i++) {
                                var MediaID = $(arr[i]).attr("MediaID");
                                var tempData = ADSchedule(MediaID, pushdata);
                                var ADScheduleInfos = JSON.stringify(tempData);
                                $(arr[i]).attr("ADScheduleInfos", ADScheduleInfos);
                            }
                            /*获取并设置总天数*/
                            var bannerData = dataArr.length;
                            for (var i = 0; i < arr.length; i++) {
                                var thisArr = arr[i];
                                $(thisArr).children().eq(9).attr("ADLaunchDays", bannerData);
                                $(thisArr).attr("ADLaunchDays", bannerData);
                                $(thisArr).children().eq(9).text(bannerData);
                                /*计算金额*/
                                var thePrice = $(thisArr).children().eq(8).attr("Price")
                                var theAmount = $(thisArr).children().eq(10);
                                var price = (thePrice * bannerData).toFixed(2);
                                theAmount.attr("AdjustPrice", price);
                                var formatPrice = formatMoney(price);
                                theAmount.children().val(price);
                            }
                            // $(".tempData").removeClass("tempData");
                            GetOrderPrice();
                            schedulNum()
                        }

                    })
                    $(".but_keep").click(function () {
                        $.closePopupLayer("popLayerDemo");
                    })
                    $("#closebt").click(function () {
                        $.closePopupLayer("popLayerDemo");
                    })
                    //BeginPlayDays
                    /*点击提交 获取日期数组 并设置主页面天数*/
                },
                error: function (dd) {
                    alert(dd.status);
                }
            });

        }
    });
    $('#tableList').on('click', '.setAllData', function () {
        if (timeInterval() == false) {
            layer.confirm('执行周期不能为空', {
                    time: 0 //不自动关闭
                    , btn: ["关闭"]
                });
        } else {
            var selectNum = $("input[name='checkbox']:checked").length;
            if (selectNum == 0) {
                layer.confirm('请选择广告位', {
                        time: 0 //不自动关闭
                        , btn: ["关闭"]
                    });
            } else {
                /*打开弹窗*/
                $.openPopupLayer({
                    name: "popLayerDemo",
                    url: "/OrderManager/schedulePopupAPP.html?r=" + Math.random(),
                    success: function () {
                        $("#selectNum").text("广告位数量：" + selectNum + "个");
                            //获取开始日期和结束日期
                            var beginDay = $("#BeginData").val();
                            var endDay = $("#EndData").val();
                            //调用设置排期方法，显示对应月份日期
                            setSchedule(beginDay, endDay);
                            /*获取所有选择的日期，存放在Li的data属性上*/
                            function getDataArr() {
                                var dataA = [];
                                var selectList = $(".select");
                                for (var i = 0; i < selectList.length; i++) {
                                    dataA.push($(selectList[i]).parent().attr("data"));
                                }
                                return dataA;
                            }

                            // /*二次点击设置排期时显示已设置的排期*/
                            // displaySchedule(that);
                            /*所有已选日期数组*/
                            var dataArr = getDataArr();
                            $(".ml20").text("投放天数:" + dataArr.length + "天");
                            var orderList = $(".choseLi");
                            /*设置所有日期的点击事件，改变背景颜色，同时修改投放天数*/
                            for (var i = 0; i < orderList.length; i++) {
                                $(orderList[i]).click(function () {
                                    if ($(this).children().attr("class") === "select") {
                                        $(this).children().removeClass("select")
                                    } else {
                                        $(this).html("<span>" + $(this).text() + "</span>")
                                        $(this).children().addClass("select")
                                    }
                                    var dataArr = getDataArr();
                                    $(".ml20").text("投放天数:" + dataArr.length + "天");
                                })
                            }
                            /*获取被选中的标签*/
                            $(".getData").click(function () {
                                var deliveryDays = $(".ml20").text().substr(5, 1);
                                if (deliveryDays == 0) {
                                    /*显示弹窗，提示用户投放天数/千次不能为空*/
                                    layer.confirm('投放天数/千次不能为空', {
                                        time: 0 //不自动关闭
                                        , btn: ["关闭"]
                                    });
                                } else {
                                    /*关闭弹层*/
                                    $.closePopupLayer("popLayerDemo");
                                    var dataArr = getDataArr();
                                    /*返回选中的日期*/
                                    /*处理返回的日期数组 若为单日，返回单日，若为日期区间，转换为开始-结束的形式*/
                                    var pushdata = outPutDate(dataArr);
                                    /**所有已选广告位的当前行 数组*/
                                    var arr = $("input[name='checkbox']:checked").parents("tr");
                                    for (var i = 0; i < arr.length; i++) {
                                        var MediaID = $(arr[i]).attr("MediaID")
                                        var tempData = ADSchedule(MediaID, pushdata);
                                        var ADScheduleInfos = JSON.stringify(tempData);
                                        $(arr[i]).attr("ADScheduleInfos", ADScheduleInfos)
                                    }
                                    /*获取并设置总天数*/
                                    var bannerData = dataArr.length;
                                    // var arr = $("input[name='checkbox']:checked").parent().parent();
                                    // console.log(arr);
                                    for (var i = 0; i < arr.length; i++) {
                                        var thisArr = arr[i];
                                        $(thisArr).children().eq(9).attr("ADLaunchDays", bannerData);
                                        $(thisArr).attr("ADLaunchDays", bannerData);
                                        $(thisArr).children().eq(9).text(bannerData);
                                        /*计算金额*/
                                        var thePrice = $(thisArr).children().eq(8).attr("Price")
                                        var theAmount = $(thisArr).children().eq(10)
                                        var price = (thePrice * bannerData).toFixed(2)
                                        theAmount.attr("AdjustPrice", price);
                                        var formatPrice = formatMoney(price);
                                        theAmount.children().val(price);
                                    };
                                    GetOrderPrice();
                                    schedulNum()
                                }

                            })
                            $(".but_keep").click(function () {
                                $.closePopupLayer("popLayerDemo");
                            })
                            $("#closebt").click(function () {
                                $.closePopupLayer("popLayerDemo");
                            })

                        },
                        error: function (dd) {
                            alert(dd.status);
                        }

                    })
            }
        }
    });
    /*点击保存 跳转到购物车*/
    $(".getShopCar").click(function () {
        var data = getupShoppingCart(2, MediaType, orderid)
        upShoppingCart(data, MediaType, orderid)
    })

    // 参数保存用于点击审核通过不通过的click事件
    var enable = {
        MediaType: MediaType,
        orderid: orderid
    }

    /*  //上传图片的插件begin*/

    /**
     * @desc  JQuery扩展，将json字符串转换为对象，需要引用类库JQuery
     * @param   json字符串
     * @return 返回object,array,string等对象
     * @Add=Masj, Date: 2009-12-07
     */
     jQuery.extend(
     {
        evalJSON: function (strJson) {
            if ($.trim(strJson) == '')
                return '';
            else
                return eval("(" + strJson + ")");
        }
    });
    function getCookie(name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
        if (arr = document.cookie.match(reg))
            return unescape(arr[2]);
        else
            return null;
    }

    function escapeStr(str) {
        return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
    }

    function postTest() {

        $.ajax({
            type: "post",
            url: "/api/Authorize/CheckRight",
            contentType: 'application/json',
            data: JSON.stringify({ModuleIDs: "Jim,dafd,a,asd,fasd"}),
            success: function (data, status) {
            }
        });
    }

    function disableConfirmBtn() {
        $('#btnConfirm').attr('disabled', 'disabled');
    }

    function enableConfirmBtn() {
        $('#btnConfirm').removeAttr('disabled');
    }

    var uploadSuccess = true;
    $(document).ready(function () {

        $('#divPopLayer').unbind('click').bind('click', function () {

            $.openPopupLayer({
                name: "popLayerDemo",
                url: "/appSchedulePopup.html?r=" + Math.random(),
                error: function (dd) {
                    alert(dd.status);
                }
            });

        });

        $("#uploadify").uploadify({
            'buttonText': '+ 上传附件',
            'buttonClass': 'but_upload',
            'swf': '/Js/uploadify.swf?_=' + Math.random(),
            'uploader': '/AjaxServers/UploadFile.ashx',
            'auto': true,
            'multi': false,
            'width': 200,
            'height': 35,
            'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
            'fileTypeDesc': '支持格式:jpg,jpeg,png,zip',
            'fileTypeExts': '*.jpg;*.jpeg;*.png;*.zip;',
            'fileSizeLimit':'5MB',
            'queueSizeLimit': 1,
            'scriptAccess': 'always',
            'onQueueComplete': function (event, data) {
                //enableConfirmBtn();

            },
            'onQueueFull': function () {
                alert('您最多只能上传1个文件！');
                return false;
            },
            'onUploadSuccess': function (file, data, response) {
                $("#uploadify-queue").css("display", "none")

                if (response == true) {
                    var json = $.evalJSON(data);
                    // console.log(json);
                    $("#imgName").text(json.FileName)
                    $('#imgUploadFile').attr('UploadFileURL',json.Msg);
                    $("#imgDownURL").attr("href",json.Msg)
                    //var jsonData = $.evalJSON(data);

                    //                    if (jsonData.Result == false) {
                    //                        $.jAlert(jsonData.Msg);
                    //                    }

                    //$('#SpanMsg').hide();

                    //                            if (jsonData.FailCount > 0) {
                    //                                //$("#hidData").val(jsonData.ErrorData);
                    //                                $("#formExport").submit();
                    //                            }
                    //                            $.jAlert(jsonData.Msg);
                }
            },
            'onProgress': function (event, queueID, fileObj, data) {

            },
            'onUploadError': function (event, queueID, fileObj, errorObj) {
                //enableConfirmBtn();
            }
        });
    });
    /*上传图片插件End*/


    /*时间转换函数*/
    function ConverToDates(DateStr) {
        var myDate = new Date(DateStr);
        var strDate = myDate.getFullYear() + "/" + (myDate.getMonth() + 1) + "/" + myDate.getDate();
        return strDate;
    }

    /*计算当前日期是星期几*/
    function getWeek(strDate) {
        var week = new Date(strDate).getDay();
        return week;
    }

    /*验证是否填写需求名称*/
    function checkUsername() {
        var requireName = $("input:text[name='textUsername']").val()
        if (requireName === "") {
            $(".nameMsg").text("请填写需求名称");
            return false;
        } else if (requireName.length < 3 || requireName.length > 25) {
            $(".nameMsg").text("字符长度为3-25");
            return false;
        } else {
            $(".nameMsg").text("");
            return true;
        }
        ;
    }

    /*验证是否选择时间区间*/
    function checkTime() {
        var requireTime = $("input[name='beginTime']").val()
        var requireETime = $("input[name='endTime']").val()
        if (requireTime === "" || requireETime === "") {
            $(".timeMsg").text("请选择执行时间")
            return false;
        } else {
            $(".timeMsg").text(" ");
            return true;
        }
        ;
    }

    /*验证是否填写详情*/
    function checkExplain() {
        var requireExplain = $(".textArea").val()
        if (requireExplain === "") {
            $(".explainMsg").text("请填写需求说明")
            return false;
        } else {
            $(".explainMsg").text(" ")
            return true;
        }
        ;
    };
    /*验证否选中同意协议按钮*/
    function checkAgreementBox() {
        if ($("#agreementBox").prop('checked')) {
            $("#nextBtn").css("background-color", "#FF9100");
            return true;
        } else {
            $("#nextBtn").css("background-color", "#666");
            return false
        }
    }

    /*验证排期天数是否为0*/
    function schedulNum() {
        /*获取选择了广告位的媒体个数*/
        var ADLaunchDays = $("tr[ADLaunchDays]");
        for (var i = 0; i < ADLaunchDays.length; i++) {
            var theADLaunchDays = $(ADLaunchDays[i]).attr("ADLaunchDays");
            if (theADLaunchDays != 0) {
                return true;
            } else {
                return false;
            }
        }
    }

    /*验证是否是正整数*/
    function isInteger(obj) {
        return Math.floor(obj) === obj
    }

    /*验证是否存在CPMCPD*/
    function checkCPMCPD() {
        var cpmlist = $(".CPM");
        var cpdlist = $(".CPD");
        // console.log(cpdlist);
        if (cpmlist.length == 0) {
            $("#CPM").remove();
        }
        if (cpdlist.length == 0) {
            $("#CPD").remove();
        }

    }

    /*验证是否选择时间区间*/
    function timeInterval() {
        var BeginDataArr = $("input[name='beginTime']").val().split("-");
        var BeginData = "";
        for (var i = 0; i < BeginDataArr.length; i++) {
            BeginData = BeginData + BeginDataArr[i];
        }
        var EndDataArr = $("input[name='endTime']").val().split("-");
        var EndData = "";
        for (var i = 0; i < EndDataArr.length; i++) {
            EndData = EndData + EndDataArr[i];
        }
        if (BeginDataArr == 0) {
            return false;
        }
        else if (EndData - BeginData >= 0) {
            return true;
        } else {
            return false;
        }
    }


    /*1.页面加载完成  根据返回的信息查询订单列表*/
    function getOrderList(orderid) {
        setAjax({
            type: "get",
            url: "/api/ADOrderInfo/GetByOrderID_ADOrderInfo",
            data: {
                orderid: orderid,
            }
                // url:"php/GetPublishBasicInfoByID.php",
            }, function (data) {
                if (data.Result.ADOrderInfo.Status != '16002') {
                    alert("该项目已审核！系统自动跳转至审核列表");
                    window.location = "/OrderManager/ListOfProject.html";
                }
                // console.log("订单信息查询成功了");
                /*渲染页面*/
                if (data.Result) {
                    /*若非AE或广告主为空，不显示广告主，否则，
                    若有广告主的企业名称，显示其企业名称，若无，显示登录账号*/
                    if(CTLogin.RoleIDs != "SYS001RL00005" && CTLogin.RoleIDs != "SYS001RL00001" && CTLogin.RoleIDs != "SYS001RL00004" || data.Result.ADOrderInfo.CustomerID == ""){
                        $(".advertiser").remove();
                    }else{
                        $(".advertiser .advertiserName").attr("CustomerID",data.Result.ADOrderInfo.CustomerID);
                        if(data.Result.ADOrderInfo.CustomerName != ""){
                            $(".advertiser .advertiserName").text(data.Result.ADOrderInfo.CustomerName);
                        }else{
                            $(".advertiser .advertiserName").text(data.Result.ADOrderInfo.CreatorUserName);
                        }
                    }
                    $("input[name='textUsername']").val(data.Result.ADOrderInfo.OrderName);
                    $("#BeginData").val(data.Result.ADOrderInfo.BeginTime.substr(0, 10));
                    $("#EndData").val(data.Result.ADOrderInfo.EndTime.substr(0, 10));
                    $(".textArea").val(data.Result.ADOrderInfo.Note);
                    var imgSrc = data.Result.ADOrderInfo.UploadFileURL;
                    $("#imgName").text(data.Result.ADOrderInfo.UploadFileName);
                    $("#imgUploadFile").attr("UploadFileURL", imgSrc);
                    $("#imgDownURL").attr("href", imgSrc);
                    //    1.获取模板中的页面结构
                    var appTemplate = $("#appOrderTemplate").html();
                    var html = ejs.render(appTemplate, {data: data});
                    $("#appTable>table").append(html);
                    $("#buttonSwitch1").html(ejs.render($("#buttonSwitch2").html(), {data: data.Result.ADOrderInfo.Status}))
                    var tableList = $(".tableList");
                    var ADScheduleInfosArr = [];
                    for (var j = 0; j < data.Result.SubADInfos.length; j++) {
                        for (var i = 0; i < data.Result.SubADInfos[j].APPDetails.length; i++) {
                            var ADScheduleInfos = data.Result.SubADInfos[j].APPDetails[i].ADScheduleInfos;
                            ADScheduleInfosArr.push(ADScheduleInfos);
                        }
                    }
                    var tableList = $(".tableList");
                    var dataarr=[];
                    for (var i = 0; i < ADScheduleInfosArr.length; i++) {
                        var thisADSInfo = ADScheduleInfosArr[i];
                        for (var k = 0; k < thisADSInfo.length; k++) {
                            thisMediaID = thisADSInfo[k].MediaID;
                            // console.log(thisMediaID);
                            var ADScheduleInfos = thisADSInfo;
                            dataarr.push(ADScheduleInfos)
                        }
                    }
                    Array.prototype.unique1 = function(){
                        var res = [this[0]];
                        for(var i = 1; i < this.length; i++){
                            var repeat = false;
                            for(var j = 0; j < res.length; j++){
                                if(this[i] == res[j]){
                                    repeat = true;
                                    break;
                                }
                            }
                            if(!repeat){
                                res.push(this[i]);
                            }
                        }
                        return res;
                    }
                    dataarr=dataarr.unique1()
                    $(".tableList.CPD").each(function (i) {
                        $(this).attr("ADScheduleInfos",JSON.stringify(dataarr[i]))
                    })
                    var OriginalPriceList = $("td[OriginalPrice]");
                    for (var i = 0; i < OriginalPriceList.length; i++) {
                        var thisTd = OriginalPriceList[i];
                        var thePrice = $(thisTd).attr("OriginalPrice") - 0;
                        var theDiscount = $(thisTd).attr("SaleDiscount") - 0;
                        var OPrice = thePrice * theDiscount;
                        var price = OPrice.toFixed(2);
                        $(thisTd).attr("price", price);
                        $(thisTd).text(formatMoney(price));
                        if ($(thisTd).parent().attr("ADLaunchIDs") == "11002") {
                            $(thisTd).text(formatMoney(price) + "/CPM");
                        } else {
                            $(thisTd).text(formatMoney(price)+ "/天/轮播");
                        }
                    }


                }
                GetMeidaNum()
                GetOrderPrice();
                checkCPMCPD();
                /*验证金额格式*/
                function checkAccount(){
                    var a;
                    var allAccount = $(".accountTest");
                    var reg = /^(0|[1-9][0-9]{0,9})(\.[0-9]{1,2})?$/;
                    allAccount.each(function(){
                        if (reg.test($(this).val()) == false) {
                            layer.confirm('请输入正确的金额', {
                                time: 0 //不自动关闭
                                , btn: ["关闭"]
                            });
                            a = false;
                            return false;
                        }else{
                            return true;
                        }
                    });
                    return a===undefined?true:false;
                }
                /*验证投放天数非0*/
                function dateIsNull(){
                    var a;
                    var dateList = $('.CPD td[adlaunchdays]');
                    dateList.each(function(){
                        if($(this).text().trim() == "" || $(this).text().trim() == 0){
                            layer.confirm('投放天数/千次不能为空', {
                                time: 0 //不自动关闭
                                , btn: ["关闭"]
                            });
                            a = false;
                            return false
                        }
                    });
                    return a===undefined?true:false;
                }
                // 判断cpm广告位的投放次数是否为空或零
                function dataListIsNull() {
                    var b;
                    var dataListM = $('.CPM td[adlaunchdays]');
                    dataListM.each(function(){

                        if(!($(this).children().val()!=""&&($(this).children().val()!="0"))){
                            layer.confirm('投放天数/千次不能为空', {
                                time: 0 //不自动关闭
                                , btn: ["关闭"]
                            });
                            b = false;
                            return false
                        }
                    })
                    return b===undefined?true:false;
                }

                /*点击是否通过*/
                $("#through").on("click", function () {
                    checkAccount();
                    dataListIsNull();
                    checkUsername();
                    checkTime();
                    checkExplain();
                    checkAgreementBox();
                    // 获取表格里的信息
                    if (checkUsername() == true && checkTime() == true && checkExplain() == true && checkAccount() == true) {
                        if (schedulNum() == true && dateIsNull() == true && dataListIsNull() ==true) {
                            var data = getupShoppingCart(2, enable.MediaType, enable.orderid)
                            YesupOrder(data, enable.orderid);
                        } else {
                            layer.confirm('投放天数/千次不能为空', {
                                time: 0 //不自动关闭
                                , btn: ["关闭"]
                            });
                        }

                    }

                })

                $("#notThrough").unbind("click").bind("click", function () {
                    checkAccount();
                    dataListIsNull();
                    if(checkAccount() == false){
                        layer.confirm('请输入正确的金额', {
                            time: 0 //不自动关闭
                            , btn: ["关闭"]
                        });

                    }else{
                      if(dateIsNull() == false || dataListIsNull() ==false){
                          layer.confirm('投放天数/千次不能为空', {
                                time: 0 //不自动关闭
                                , btn: ["关闭"]
                            });

                      }else if(checkAccount()==true){
                        $.openPopupLayer({
                            name: "AuditRejected",
                            url: "/OrderManager/AuditRejected.html?r=" + Math.random(),
                            success: function () {
                                $("#RejectedMsg").on("keyup", function () {
                                    var rejectMsg = $(this).val();
                                    //判断驳回原因是否符合要求,要求字符长度小于200
                                    if (rejectMsg.length > 200) {
                                        $(this).val(rejectMsg.substr(0, 200));
                                    }
                                });
                                $(".button").click(function () {
                                    var data = getupShoppingCart(2, enable.MediaType, enable.orderid);
                                    // console.log(data);
                                    var rejectMsg = $("#RejectedMsg").val();
                                    if (rejectMsg == "") {
                                        layer.confirm('请填写驳回原因', {
                                            time: 0 //不自动关闭
                                            , btn: ["确定"]
                                        })
                                    } else {
                                        // console.log(data);
                                        NoupOrder(data, enable.orderid, rejectMsg);

                                        $.closePopupLayer("AuditRejected");
                                    }

                                })
                                $("#no").click(function () {
                                    $.closePopupLayer('AuditRejected')
                                })
                            },
                            error: function (dd) {
                                alert(dd.status);
                            }
                        });
                      }
                    }

                });
            }, function () {
                // console.log("订单信息查询失败了");
            }
            )
    }

    /*2.获取媒体总个数的函数*/
    function GetMeidaNum() {
        var trList = $("tr[IsSelected='true']").length;
        $("#tableNum").text(trList);
        return trList;
    }

    /*3.获取总价格*/
    /*获取价格*/
    function GetOrderPrice() {
        var AmountList = $("td[AdjustPrice]");
        var OrderPrice = 0;
        for (var i = 0; i < AmountList.length; i++) {
            var price = $(AmountList[i]).attr("AdjustPrice")
            if (price == "") {
                price = 0;
            }
            OrderPrice = (price - 0) + OrderPrice;
        }
        OrderPrice = formatMoney(OrderPrice,2,"");
        $("#OrderPrice").text(OrderPrice);
        var revisedPriceList = $("input[name='Username']")
        for (var i = 0; i < revisedPriceList.length; i++) {
            $(revisedPriceList[i]).change(function () {
                var thisprice = $(this).val() - 0;
                if (isInteger(thisprice)==false) {
                    layer.confirm('投放天数/千次不能为空', {
                            time: 0 //不自动关闭
                            , btn: ["关闭"]
                        });
                    $(this).val("")
                    $(this).parent().parent().children().eq(10).children().val("");
                }
                else {
                    if (thisprice == "") {
                        $(this).parent().parent().children().eq(10).children().val("");
                        $(this).parent().parent().children().eq(10).children().attr("value", "");
                        $(this).parent().parent().children().eq(10).attr("AdjustPrice", "");
                        $(this).parent().parent().children().eq(9).attr("ADLaunchDays", "");
                    } else {
                        var onePrice = $(this).parent().parent().children().eq(8).attr("Price") - 0;
                        var price = (thisprice * onePrice).toFixed(2)
                        $(this).parent().parent().children().eq(9).attr("ADLaunchDays", thisprice);
                        $(this).parent().parent().attr("ADLaunchDays", thisprice);
                        $(this).parent().parent().children().eq(10).attr("AdjustPrice", price);
                        $(this).parent().parent().children().eq(10).children().val(price);
                    }
                    var AmountList = $("td[AdjustPrice]");
                    var OrderPrice = 0;
                    for (var i = 0; i < AmountList.length; i++) {
                        var price = $(AmountList[i]).attr("AdjustPrice")
                        OrderPrice = (price - 0) + OrderPrice;
                    }
                    OrderPrice = formatMoney(OrderPrice,2,"");
                    $("#OrderPrice").text(OrderPrice);
                    schedulNum()
                }
            })
        }
        var revisedPriceList = $("input[name='CPDUsername']")
        for (var i = 0; i < revisedPriceList.length; i++) {
            $(revisedPriceList[i]).change(function () {
                if (/^(0|[1-9][0-9]{0,9})(\.[0-9]{1,2})?$/.test($(this).val()) == false) {
                      layer.confirm('请输入正确的金额', {
                          time: 0 //不自动关闭
                          , btn: ["关闭"]
                      });
                      $(this).val("");
                } else {
                    var thisprice = $(this).val() - 0;
                    if (thisprice == "" && thisprice != 0) {
                        $(this).parent().parent().children().eq(10).children().val("");
                        $(this).parent().parent().children().eq(10).attr("AdjustPrice", "");
                    } else {
                            // var price = formatMoney(thisprice);
                            $(this).parent().parent().children().eq(10).attr("AdjustPrice", thisprice.toFixed(2));
                            $(this).parent().parent().children().eq(10).children().val(thisprice);
                        }
                        var AmountList = $("td[AdjustPrice]");
                        var OrderPrice = 0;
                        for (var i = 0; i < AmountList.length; i++) {
                            var price = $(AmountList[i]).attr("AdjustPrice")
                            OrderPrice = (price - 0) + OrderPrice;
                        }
                        OrderPrice = formatMoney(OrderPrice,2,"");
                        $("#OrderPrice").text(OrderPrice);
                        schedulNum()
                    }
                })
        }

        var revisedPriceList = $("input[name='CPMUsername']")
        for (var i = 0; i < revisedPriceList.length; i++) {
            $(revisedPriceList[i]).change(function () {
                if (/^(0|[1-9][0-9]{0,9})(\.[0-9]{1,2})?$/.test($(this).val()) == false) {
                    layer.confirm('请输入正确的金额', {
                        time: 0 //不自动关闭
                        , btn: ["关闭"]
                    });
                    $(this).val("");
                } else {
                    var thisprice = $(this).val() - 0;
                    if (thisprice == "" && thisprice != 0) {
                        $(this).parent().parent().children().eq(10).children().val("");
                        $(this).parent().parent().children().eq(10).attr("AdjustPrice", "");
                    } else {
                            // var price = formatMoney(thisprice);
                            $(this).parent().parent().children().eq(10).attr("AdjustPrice", thisprice.toFixed(2));
                            $(this).parent().parent().children().eq(10).children().val(thisprice);
                        }
                        var AmountList = $("td[AdjustPrice]");
                        var OrderPrice = 0;
                        for (var i = 0; i < AmountList.length; i++) {
                            var price = $(AmountList[i]).attr("AdjustPrice")
                            OrderPrice = (price - 0) + OrderPrice;
                        }
                        OrderPrice = formatMoney(OrderPrice,2,"");
                        $("#OrderPrice").text(OrderPrice);
                        schedulNum()
                    }
                })
        }

        return OrderPrice;
    }

    /*4.删除提交信息*/
    function Operate_ShoppingCart(MediaType, MediaID, PublishDetailID) {

        setAjax({
            type: "post",
            url: "/api/ShoppingCart/Operate_ShoppingCart",
            data: {
                "OptType": 2,
                "MediaType": MediaType,
                "IDs": [{
                    "MediaID": MediaID,
                    "PublishDetailID": PublishDetailID,
                    "pub": ""
                }]
            }
        },
        function (data) {
        }
        , function () {
            alert("请求失败")
        })
    }

    /*2.获取订单补充信息*/
    function getADOrderInfo(MediaType, orderid) {
        var requireName = $("input:text[name='textUsername']").val()
        var requireExplain = $(".textArea").val()
        var UploadFileURL = $('#imgUploadFile').attr('UploadFileURL')
        // console.log(UploadFileURL);
        var requireTime = $("input[name='beginTime']").val()
        var requireETime = $("input[name='endTime']").val()
        var CustomerID = $(".advertiser .advertiserName").attr("CustomerID") || "gt86ZRCRjng%3d";
        var ADOrderInfo = {
            "OrderID": orderid,
            "MediaType": MediaType,
            "OrderName": requireName,
            "Status": 16002,
            "BeginTime": requireTime,
            "EndTime": requireETime,
            "Note": requireExplain,
            "UploadFileURL": UploadFileURL,
            "CustomerID": CustomerID
        }
        return ADOrderInfo;
    }

    /*2.上传信息*/

    function getupShoppingCart(optType, MediaType, orderid) {
        var getTr = $("tr[IsSelected='true']")
        var ADDetails = []
        // /*遍历*/
        // console.log("订单数目是" + getTr.length);
        for (var j = 0; j < getTr.length; j++) {
            var MediaID = $(getTr[j]).attr("MediaID");
            var PublishDetailID = $(getTr[j]).attr("PublishDetailID");
            var ADLaunchDays = $(getTr[j]).children().eq(9).attr("ADLaunchDays");
            var AdjustPrice = $(getTr[j]).children().eq(10).attr("AdjustPrice");
            var AdjustDiscount = $(getTr[j]).children().eq(10).attr("AdjustDiscount");

            /*排期数组*/
            // var ADSchedule = $(getTr[j]).children().eq(9).attr("ADSchedule");
            var tempObjADSchedule = $(getTr[j]).attr("ADScheduleInfos")
            if (tempObjADSchedule === "") {
                var ObjStory = {
                    "MediaType": MediaType,
                    "MediaID": MediaID,
                    "PubDetailID": PublishDetailID,
                    "AdjustPrice": AdjustPrice,
                    "AdjustDiscount": AdjustDiscount,
                    "ADLaunchDays": ADLaunchDays,
                    "ADScheduleInfos": ""
                }
            } else {
                var ADScheduleInfos = JSON.parse(tempObjADSchedule);
                var ObjStory = {
                    "MediaType": 14002,
                    "MediaID": MediaID,
                    "PubDetailID": PublishDetailID,
                    "AdjustPrice": AdjustPrice,
                    "AdjustDiscount": AdjustDiscount,
                    "ADLaunchDays": ADLaunchDays,
                    "ADScheduleInfos": ADScheduleInfos
                }
            }

            ADDetails[j] = ObjStory
        }
        var ADOrderInfo = getADOrderInfo(MediaType, orderid);
        var getupShoppingCart = {
            "optType": optType,
            "ADOrderInfo": ADOrderInfo,
            "ADDetails": ADDetails
        }
        // // var getOrderInfoObj = new ObjStory(MediaID, PublishDetail,OrderPrice);//声明对象
        // var OrderInfoJsonData = JSON.stringify(getOrderInfo);
        return getupShoppingCart;
    }

    /*判断时间间隔是否大于半年*/
    function compare(begin,end){
        var b_year = begin.split('-')[0];
        var b_month = begin.split('-')[1];
        var b_day = begin.split('-')[2];

        var e_year = end.split('-')[0];
        var e_month = end.split('-')[1];
        var e_day = end.split('-')[2];
        if(e_year- b_year>=1){
            return false;
        }else if(e_month - b_month >6){
            return false;
        }else{
            if(e_month - b_month == 6){
                if(e_day >= b_day){
                    return false
                }else{
                    return true
                }
            }else{
                return true;
            }
        }
    }
    /*3.审核通过上传订单信息  */
    function YesupOrder(pushData, orderid) {
        pushData.ADOrderInfo.Status = 16003;
        //console.log(pushData);
            setAjax({
                type: "post",
                url: "/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
                // url:"php/GetPublishBasicInfoByID.php",
                data: pushData
            }, function (data) {
                var rejectMsg = null;
                Review_ADOrderInfo(orderid, 27001, rejectMsg)
            }, function () {
                // console.log("提交订单信息第二步失败了");
            })
        }

        /*4.审核不通过上传订单信息*/
        function NoupOrder(pushData, orderid, rejectMsg) {
            //console.log(pushData);
            // console.log(getADDetailsInfo());
            setAjax({
                type: "post",
                url: "/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
                // url:"php/GetPublishBasicInfoByID.php",
                data: pushData
            }, function (data) {
                Review_ADOrderInfo(orderid, 27002, rejectMsg);
            }, function () {
                // console.log("提交订单信息第二步失败了");
            })
        }

        /*8. 上传订单是否通过按钮*/
        function Review_ADOrderInfo(orderid, optType, rejectMsg) {
            setAjax({
                type: "get",
                url: "/api/ADOrderInfo/Review_ADOrderInfo",
                data: {
                    orderid: orderid,
                    optType: optType,
                    rejectMsg: rejectMsg
                }
            }, function (data) {
                if (rejectMsg == null) {
                    window.location.href = "/OrderManager/ListOfProject.html?state=16003";
                } else {
                    window.location.href = "/OrderManager/ListOfProject.html?state=16006";
                }
            }, function () {
                // console.log("调用审核订单接口失败了");
            })
        }

        /*5.点击保存订单跳转到购物车*/
    //获取选择的排期
    function ADSchedule(MediaID, pushdata) {
        var ADInfosArr = [];
        for (var j = 0; j < pushdata.length; j++) {
            var arr = {};
            arr = {
                "ADDetailID": 0,
                "OrderID": "",
                "SubOrderID": "",
                "MediaID": MediaID,
                "PubID": 0,
                "BeginData": pushdata[j].S,
                "EndData": pushdata[j].E,
                "CreateTime": "0001-01-01T00:00:00",
                "CreateUserID": 0
            }
            ADInfosArr[j] = arr;
        }
        return ADInfosArr
    }

    /*审核订单 保存订单并继续添加账号*/
    function upShoppingCart(data, MediaType, orderid) {
        // console.log(data);
        if (data.ADDetails.length != 0) {
            setAjax({
                type: "post",
                url: "/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
                    // url:"php/GetPublishBasicInfoByID.php",
                    data: data
                }, function (data) {
                    window.location.href = "/OrderManager/app_list_check.html?MediaType=" + MediaType + "&orderID=" + orderid;
                }, function () {

                })
        } else {
            setAjax({
                type: "post",
                url: "/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
                    // url:"php/GetPublishBasicInfoByID.php",
                    data: {
                        "optType": 2,
                        "ADOrderInfo": {
                            "OrderID": orderid,
                            "MediaType": MediaType,
                            "Status": 16001
                        },
                        "ADDetails": []
                    }
                }, function (data) {
                    window.location = "/OrderManager/app_list.html"
                }, function () {

                })
        }
    }

    /*6.如果审核时修改了开始时间和结束时间  所设置的排期信息将清空*/
    function clearSchedule() {
        var trList = $(".tableList");
        for (var i = 0; i < trList.length; i++) {
            var thisTr = trList[i];
            if ($(thisTr).attr("ADLaunchIDs") == "11001") {
                $(thisTr).attr("ADScheduleInfos", "")
                $(thisTr).attr("ADScheduleInfos", "")
                $(thisTr).children().eq(9).text("");
                $(thisTr).children().eq(9).attr("ADLaunchDays", "");
                $(thisTr).children().eq(10).attr("AdjustPrice", "");
                $(thisTr).children().eq(10).children().val("");
                $(thisTr).children().eq(10).children().attr("value", "");
            } else {
                $(thisTr).attr("ADScheduleInfos", "")
                $(thisTr).children().eq(9).children().val("");
                $(thisTr).children().eq(9).children().attr("value", "");
                $(thisTr).children().eq(9).attr("ADLaunchDays", "");
                $(thisTr).children().eq(10).attr("AdjustPrice", "");
                $(thisTr).children().eq(10).children().val("");
                $(thisTr).children().eq(10).children().attr("value", "");
            }
        }
    }

    /*封装输出连续日期的JS函数*/
    function outPutDate(datestr) {
        Date.prototype.addDate = function (days) {
                var a = this; //new Date(dd);
                a = a.valueOf();
                a = a + days * 24 * 60 * 60 * 1000
                a = new Date(a);
                return a;
            }
            var result = new Array();
            var array = {};

            for (var i = 0; i < datestr.length; i++) {

                var d = new Date(datestr[i].replace(/-/g, "/"));
                var isEnd = (i == (datestr.length - 1) ? true : false);

                if (isEnd == false) {
                    var dNext = new Date(datestr[i + 1].replace(/-/g, "/"));
                    if ((d.addDate(1) - dNext) == 0) {
                        if (!array.S) {
                            array.S = datestr[i];
                        }
                        array.E = datestr[i + 1];
                        continue;
                    }
                    else {
                        if (array.S) {
                            array.E = datestr[i];
                        }
                        else {
                            array.S = datestr[i];
                            array.E = datestr[i];
                        }
                        result.push(array);
                        array = {};
                        continue;
                    }
                }
                else {
                    if (!array.S) {
                        array.S = datestr[i];
                    }
                    if (!array.E) {
                        array.E = datestr[i];
                    }
                    result.push(array);
                    array = {};
                }
            }
            return result;
        }
    })
