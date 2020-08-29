/**
 * Created by chengj on 2017/3/8.
 */
$(function () {
    // 判断标志
    var flag = false;
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
    var orderData = GetRequest();

    //118新增的字段将其 在查询接口中取出  然后提交修改的时候给接口传过去
    var CRMCustomerID = '';
    var CustomerText = '';


    /*获取订单号*/
    var orderid = orderData.orderID;
    var MediaType = orderData.MediaType;
    /*查询订单列表*/
    getOrderList(orderid);

    /*验证是否填写需求名称*/
    $("input:text[name='Username']").blur(function () {
        checkUsername();
    })

    /*点击执行时间选择框，弹出日历插件*/
    $("#BeginData").click(function () {
        laydate({
                elem: '#BeginData',
                fixed: false,
                istime: true,
                isclear: true,
                format: 'YYYY-MM-DD hh:mm:ss',
                choose: function () {
                    checkTime();
                    var beginData = $("#BeginData").val();
                    var myDate = new Date();
                    var onDay = myDate.toLocaleDateString();
                    var dataResult = comptime(beginData, onDay)
                    if (dataResult === 102) {
                        layer.confirm('开始日期至少要大于今天', {
                            time: 0 //不自动关闭
                            , btn: ["关闭"]
                        });
                        $("#BeginData").val("");
                        $("#EndData").val("");
                    }

                }
            }
        )
    })

    /*验证是否填写需求说明*/
    $(".textArea").blur(function () {
        checkExplain();
    })

    /*点击保存并继续添加账号，获取页面数据，将数据提交到购物车*/
    $(".getShopCar").click(function () {
        var data = getADDetailsInfo(MediaType, orderid)
        upShoppingCart(data, MediaType, orderid)
    })

    /*点击删除某一项数据*/
    $("#tableList").on("click", ".delBtn", function () {
        var tablistLength = $("#tableList").find(".tableList").length;
        if(tablistLength <= 1){
            layer.confirm('不能删除项目中的最后一个媒体', {
                time: 0 //不自动关闭
                , btn: ["关闭"]
            });
        }else{
            //信息框-例2
            var thisTable = $(this);
            layer.confirm('是否删除此项？', {
                time: 0 //不自动关闭
                , btn: ['删除', '取消']
                , yes: function (index) {
                    var MediaID = thisTable.parent().parent().attr("MediaID");
                    var PublishDetailID = thisTable.parent().parent().attr("PublishDetailID");
                    Operate_ShoppingCart(MediaType, MediaID, PublishDetailID)
                    thisTable.parent().parent().remove();
                    GetMeidaNum();
                    GetOrderPrice()
                    layer.close(index);
                    layer.msg('操作成功', {time: 400});
                }
            });
        }

    })
    /*选择广告，获取刊例详细信息*/
    $("#tableList").on("click", ".select_banner", function () {
        /*点击时发起ajax请求 参数是媒体类型 和媒体类型ID  获取到相应的刊例信息*/
        /*获取到这一行的媒体ID begin*/
        var MediaID = $(this).parent().parent().attr("MediaID");
        $(this).parent().parent().addClass("location");
        if (MediaType == "14004" || MediaType == "14005") {
            $.openPopupLayer({
                name: "popLayerDemo",
                url: "/OrderManager/schedulePopupVedioSelf.html?r=" + Math.random(),
                success: function () {
                    /*渲染对应媒体ID的刊例信息*/
                    GetPublishBasicInfoByID(MediaType, MediaID);
                    /*点击提交按钮 获取被选中的广告信息*/
                    $(".button").click(function () {
                        var ADPosition1 = $("input[type='radio']:checked").parent().parent().attr("ADPosition1");
                        var ADDetailID = $("input[type='radio']:checked").parent().parent().attr("ADDetailID");
                        var Price = $("input[type='radio']:checked").parent().parent().attr("Price");
                        var ADTypeName = ADPosition1;
                        var PublishDetailID = ADDetailID
                        if (ADTypeName != "undefined-undefined-undefined") {
                            $(".location").children().eq(1).text(ADTypeName)
                            $(".location").attr("ADTypeName", ADTypeName)
                            $(".location").attr("PublishDetailID", PublishDetailID)
                            $(".location").attr("AdjustPrice", Price)
                            $(".location").children().eq(2).text(formatMoney(Price))
                            $(".location").children().eq(3).children().val(formatMoney(Price))
                            $(".location").children().eq(2).attr("AdjustPrice", Price)
                        }
                        GetOrderPrice();
                        $(".location").removeClass("location")
                        $.closePopupLayer("popLayerDemo");
                    })
                    $(".but_keep").click(function () {
                        $.closePopupLayer("popLayerDemo");
                        $(".location").removeClass("location")

                    })
                    $("#closebt").click(function () {
                        $.closePopupLayer("popLayerDemo");
                        $(".location").removeClass("location")
                    })
                },
                error: function (dd) {
                    alert(dd.status);
                }
            });
        }
        else if (MediaType == "14003") {
            $.openPopupLayer({
                name: "popLayerDemo",
                url: "/OrderManager/schedulePopupVedioWeibo.html?r=" + Math.random(),
                success: function () {
                    /*渲染对应媒体ID的刊例信息*/
                    GetPublishBasicInfoByID(MediaType, MediaID);
                    /*点击提交按钮 获取被选中的广告信息*/
                    $(".button").click(function () {
                        var ADPosition2 = $("input[type='radio']:checked").parent().parent().attr("ADPosition2");
                        var ADPosition1 = $("input[type='radio']:checked").parent().parent().attr("ADPosition1");
                        var ADDetailID = $("input[type='radio']:checked").parent().parent().attr("ADDetailID");
                        var Price = $("input[type='radio']:checked").parent().parent().attr("Price");
                        var ADTypeName = ADPosition1 + "/" + ADPosition2;
                        var PublishDetailID = ADDetailID
                        if (ADTypeName != "undefined-undefined-undefined") {
                            $(".location").children().eq(1).text(ADTypeName)
                            $(".location").attr("ADTypeName", ADTypeName)
                            $(".location").attr("PublishDetailID", PublishDetailID)
                            $(".location").attr("AdjustPrice", Price)
                            $(".location").children().eq(2).text(formatMoney(Price))
                            $(".location").children().eq(3).children().val(formatMoney(Price))
                            $(".location").children().eq(2).attr("AdjustPrice", Price)
                        }
                        GetOrderPrice();
                        $(".location").removeClass("location")
                        $.closePopupLayer("popLayerDemo");
                    })
                    $(".but_keep").click(function () {
                        $.closePopupLayer("popLayerDemo");
                        $(".location").removeClass("location")

                    })
                    $("#closebt").click(function () {
                        $.closePopupLayer("popLayerDemo");
                        $(".location").removeClass("location")
                    })
                },
                error: function (dd) {
                    alert(dd.status);
                }
            });
        }
        else {
            $.openPopupLayer({
                name: "popLayerDemo",
                url: "/OrderManager/schedulePopupSelf.html?r=" + Math.random(),
                success: function () {
                    /*渲染对应媒体ID的刊例信息*/
                    GetPublishBasicInfoByID(MediaType, MediaID);
                    /*点击提交按钮 获取被选中的广告信息*/
                    $(".button").click(function () {
                        var ADPosition2 = $("input[type='radio']:checked").parent().parent().attr("ADPosition2");
                        var ADPosition3 = $("input[type='radio']:checked").parent().parent().attr("ADPosition3");
                        var ADPosition1 = $("input[type='radio']:checked").parent().parent().attr("ADPosition1");
                        var ADDetailID = $("input[type='radio']:checked").parent().parent().attr("ADDetailID");
                        var Price = $("input[type='radio']:checked").parent().parent().attr("Price");
                        var ADTypeName = ADPosition2 + "/" + ADPosition3 + "/" + ADPosition1;
                        var PublishDetailID = ADDetailID
                        if (ADTypeName != "undefined-undefined-undefined") {
                            $(".location").children().eq(1).text(ADTypeName)
                            $(".location").attr("ADTypeName", ADTypeName)
                            $(".location").attr("PublishDetailID", PublishDetailID)
                            $(".location").attr("AdjustPrice", Price)
                            $(".location").children().eq(2).text(formatMoney(Price))
                            $(".location").children().eq(3).children().val(formatMoney(Price))
                            $(".location").children().eq(2).attr("AdjustPrice", Price)
                        }
                        GetOrderPrice();
                        $(".location").removeClass("location")
                        $.closePopupLayer("popLayerDemo");
                    })
                    $(".but_keep").click(function () {
                        $.closePopupLayer("popLayerDemo");
                        $(".location").removeClass("location")

                    })
                    $("#closebt").click(function () {
                        $.closePopupLayer("popLayerDemo");
                        $(".location").removeClass("location")
                    })
                },
                error: function (dd) {
                    alert(dd.status);
                }
            });
        }
        /*点击时发起ajax请求 参数是媒体类型 和媒体类型ID  获取到相应的刊例信息*/
        /*获取到这一行的媒体ID begin*/
    })
    
    /*点击审核通过，将修改后的订单信息提交到后台，并将审核通过的状态提交到后台*/
    $("#through").on("click", function () {
        dataListIsNull();
        if(flag){
            flag = false;
            return;
        }
        flag = false;
        //检查表单验证
        checkUsername();
        checkTime();
        checkExplain();
        checkAttachment();
        // 获取表格里的信息
        if (checkUsername() == true && checkTime() == true && checkExplain() == true && checkAttachment() == true) {
            var data = getADDetailsInfo(MediaType, orderid);
            YesupOrder(data, orderid);
        }

    })

    /*点击审核不通过，将修改后的订单信息提交到后台，并将审核不通过的状态提交到后台*/
    $("#notThrough").unbind("click").bind("click", function () {
        dataListIsNull();
        if(flag){
            flag = false;
            return;
        }
        flag = false;
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
                /*点击提交按钮 获取被选中的广告信息*/
                $(".button").click(function () {

                    var data = getADDetailsInfo(MediaType, orderid)
                    var rejectMsg = $("#RejectedMsg").val();
                    if (rejectMsg == "") {
                        layer.confirm('请填写驳回原因', {
                            time: 0 //不自动关闭
                            , btn: "确定"
                        })
                    } else {
                        NoupOrder(data, orderid, rejectMsg);
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

    });
})

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
        if (data.Result.ADOrderInfo.Status != 16002) {
            alert("该项目已审核！系统自动跳转至审核列表");
            window.location = "/OrderManager/ListOfProject.html";
        }
        /*渲染页面*/
        if (data.Result === null) {
            // console.log("购物车没有数据");
        }
        else {
            // console.log("请求的类型是APP");
            //    1.获取模板中的页面结构
            /*计算价格*/
            /*若非AE或广告主为空，不显示广告主，否则，
            若有广告主的企业名称，显示其企业名称，若无，显示登录账号*/
            if(CTLogin.RoleIDs != "SYS001RL00005" && CTLogin.RoleIDs != "SYS001RL00001" && CTLogin.RoleIDs != "SYS001RL00004"|| data.Result.ADOrderInfo.CustomerID == ""){
                $(".advertiser").remove();
            }else{
                $(".advertiser .advertiserName").attr("CustomerID",data.Result.ADOrderInfo.CustomerID);
                if(data.Result.ADOrderInfo.CustomerName != ""){
                    $(".advertiser .advertiserName").text(data.Result.ADOrderInfo.CustomerName);
                }else{
                    $(".advertiser .advertiserName").text(data.Result.ADOrderInfo.CreatorUserName);
                }
            }
            CRMCustomerID = data.Result.ADOrderInfo.CRMCustomerID;
            CustomerText = data.Result.ADOrderInfo.CustomerText;
            //    1.获取模板中的页面结构
            $("input[name='Username']").val(data.Result.ADOrderInfo.OrderName)
            $("input[name='beginTime']").val(data.Result.ADOrderInfo.BeginTime)
            $("input[name='endTime']").val(data.Result.ADOrderInfo.EndTime)
            $(".textArea").val(data.Result.ADOrderInfo.Note)
            // var imgSrc = data.Result.ADOrderInfo.UploadFileURL;
            $("#imgName").text(data.Result.ADOrderInfo.UploadFileName)
            var imgSrc = data.Result.ADOrderInfo.UploadFileURL;
            $("#imgUploadFile").attr("UploadFileURL", imgSrc)
            $("#imgDownURL").attr("href", imgSrc)
            $(".downImg").attr("href", imgSrc)
            var mediaTemplate = $("#mediaTemplate").html();
            var html = ejs.render(mediaTemplate, {data: data})
            $("#mediaTable>table").append(html);
        }
        GetOrderPrice();
        GetMeidaNum();
        /*验证成交价格格式*/
        var tablePriceList = $(".tableList .accountTest")
        for (var i = 0; i < tablePriceList.length; i++) {
            $(tablePriceList[i]).change(function () {
                if (/^(0|[1-9][0-9]{0,9})(\.[0-9]{1,2})?$/.test($(this).val()) == false) {
                      layer.confirm('请输入正确的金额', {
                          time: 0 //不自动关闭
                          , btn: ["关闭"]
                      });
                      $(this).val("");
                } 
            })
        }
    }, function () {
        // console.log("订单信息查询失败了");
    })

}

/*获取价格*/
function GetOrderPrice() {
    var AdjustPriceList = $("tr[AdjustPrice]");
    var OrderPrice = 0;
    for (var i = 0; i < AdjustPriceList.length; i++) {
        var price = $(AdjustPriceList[i]).attr("AdjustPrice")
        OrderPrice = (price - 0) + OrderPrice;
    }
    OrderPrice = formatMoney(OrderPrice,2,"");
    $("#OrderPrice").text(OrderPrice);

    var revisedPriceList = $("input[name='revisedPrice']")
    for (var i = 0; i < revisedPriceList.length; i++) {
        $(revisedPriceList[i]).change(function () {
            $(this).parent().parent().attr("AdjustPrice", $(this).val());
            $(this).attr("AdjustPrice", $(this).val());
            var OrderPrice = 0;
            for (var i = 0; i < revisedPriceList.length; i++) {
                var price = $(revisedPriceList[i]).val();
                OrderPrice = (price - 0) + OrderPrice;
                $("#OrderPrice").text(formatMoney(OrderPrice,2,""));
            }
        })
    }
    return OrderPrice;

}
/*获取媒体个数*/
function GetMeidaNum() {
    var trList = $(".tableList").length;
    $("#tableNum").text(trList);
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
/*5.选择广告  获取刊例详细信息*/
function GetPublishBasicInfoByID(MediaType, MediaID) {
    // alert("传入获取媒体详细信息的ID是" + PubIDOrMediaID)

    setAjax(
        {
            type: "get",
            url: "/api/Periodication/GetPublishBasicInfoByID",
            // url:"php/GetPublishBasicInfoByID.php",
            data: {
                PubIDOrMediaID: MediaID,
                MediaType: MediaType
            }
        },
        function (data) {
            var table_html = $("#pubBasicInfo").html()
            var html = ejs.render(table_html, {data: data})
            $("#tableInfo>table").empty().append(html);
        }
    )
}
/*5.点击保存订单跳转到购物车*/
function getupShoppingCart(MediaType) {
    var getTr = $(".tableList")
    var IDs = [];
    for (var j = 0; j < getTr.length; j++) {
        var MediaID = $(getTr[j]).attr("MediaID");
        var PublishDetailID = $(getTr[j]).attr("PublishDetailID");
        IDs[j] = {
            "MediaID": MediaID,
            "PublishDetailID": PublishDetailID
        }
    }
    var getupShoppingCart = {
        "OptType": 2,
        "MediaType": MediaType,
        "IDs": IDs
    }
    return getupShoppingCart;
}

function upShoppingCart(data, MediaType, orderid) {
    if (data.ADDetails.length != 0) {
        setAjax({
            type: "post",
            url: "/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
            data: data
        }, function (data) {
            if (MediaType == 14001) {
                window.location.href = "/OrderManager/wx_list_check.html?orderID=" + orderid;
            } else if (MediaType == 14003) {
                window.location.href = "/OrderManager/wb_list_check.html?orderID=" + orderid;
            } else if (MediaType == 14004) {
                window.location.href = "/OrderManager/sp_list_check.html?orderID=" + orderid;
            } else {
                window.location.href = "/OrderManager/zb_list_check.html?orderID=" + orderid;
            }
        })
    } else {
        setAjax({
            type: "post",
            url: "/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
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
            if (MediaType == 14003) {
                window.location = '/OrderManager/wb_list.html';
            }
            if (MediaType == 14001) {
                window.location = '/OrderManager/wx_list.html';
            }
            if (MediaType == 14004) {
                window.location = '/OrderManager/sp_list.html';
            }
            if (MediaType == 14005) {
                window.location = '/OrderManager/zb_list.html';
            }
        })
    }
}

/*6.获取订单补充信息*/
function getADOrderInfo(MediaType, orderid) {
    var requireName = $("input:text[name='Username']").val()
    var requireExplain = $(".textArea").val()
    var UploadFileURL = $('#imgUploadFile').attr('UploadFileURL')
    var beginData = $("#BeginData").val();
    var CustomerID = $(".advertiser .advertiserName").attr("CustomerID") || "gt86ZRCRjng%3d";
    ADOrderInfo = {
        "OrderID": orderid,
        "MediaType": MediaType,
        "OrderName": requireName,
        "Status": 16002,
        "BeginTime": beginData,
        "EndTime": beginData,
        "Note": requireExplain,
        "UploadFileURL": UploadFileURL,
        "CustomerID": CustomerID,
        'CRMCustomerID' :CRMCustomerID,
        'CustomerText' :CustomerText
    }
    return ADOrderInfo;
}
/*7上传信息*/
function getADDetailsInfo(MediaType, orderid) {
    //获取补充信息
    var ADOrderInfo = getADOrderInfo(MediaType, orderid);
    var newResult = {};
    newResult.optType = "2";
    newResult.ADOrderInfo = ADOrderInfo;
    newResult.ADDetails = [];
    /*数据调整*/
    var tableList = $(".tableList");
    for (var i = 0; i < tableList.length; i++) {
        var thisList = tableList[i];
        var obj = {};
        obj.MediaType = $(thisList).attr("MediaType");
        obj.MediaID = $(thisList).attr("MediaID");
        obj.PubDetailID = $(thisList).attr("PublishDetailID");
        obj.AdjustPrice = $(thisList).attr("AdjustPrice");
        ;
        obj.AdjustDiscount = $(thisList).attr("AdjustDiscount");
        obj.ADLaunchDays = 0;
        obj.ADScheduleInfos = [];
        newResult.ADDetails.push(obj);// = obj;
    }
    return newResult;
}


function YesupOrder(pushData, orderid) {
    pushData.ADOrderInfo.Status = 16003;
    setAjax({
        type: "post",
        url: "/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
        data: pushData
    }, function (data) {
        var rejectMsg = null;
        Review_ADOrderInfo(orderid, 27001, rejectMsg)
    }, function () {
        //console.log("提交订单信息第二步失败了");
    })
}

function NoupOrder(pushData, orderid, rejectMsg) {
    setAjax({
        type: "post",
        url: "/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
        data: pushData
    }, function (data) {
        Review_ADOrderInfo(orderid, 27002, rejectMsg);
    }, function () {
        // console.log("提交订单信息第二步失败了");
    })
}


/*8. 上传订单状态（通过/驳回）*/
function Review_ADOrderInfo(orderid, optType, rejectMsg) {
    setAjax({
        type: "get",
        url: "/api/ADOrderInfo/Review_ADOrderInfo",
        data: {
            orderid: orderid,
            optType: optType,
            rejectMsg: rejectMsg
        }
    }, function () {
        if (rejectMsg == null) {
            window.location.href = "/OrderManager/ListOfProject.html?state=16003";
        } else {
            window.location.href = "/OrderManager/ListOfProject.html?state=16006";
        }
    }, function () {
        // console.log("调用审核订单接口失败了");
    })
}
/*验证否上传附件*/
function checkAttachment() {
    var imgName = $("#imgName").text()
    if (imgName === "") {
        $(".imgMsg").text("请上传附件")
        return false;
    } else {
        $(".imgMsg").text(" ")
        return true;
    }
    ;
}


function comptime(beginTime, endTime) {
    var beginTimes = beginTime.substr(0, 10).split('-');
    var endTimes = endTime.substr(0, 10).split('/');

    beginTime = beginTimes[1] + '-' + beginTimes[2] + '-' + beginTimes[0] + ' ' + beginTime.substring(10, 19);
    endTime = endTimes[1] + '-' + endTimes[2] + '-' + endTimes[0] + ' ' + endTime.substring(10, 19);
    var a = (Date.parse(endTime) - Date.parse(beginTime)) / 3600 / 1000;
    if (a < 0) {
        return 201;
    } else if (a > 0) {
        return 102;
    } else if (a == 0) {
        return 101;
    } else {
        return 'exception'
    }
}
// 判断cpm广告位的投放次数是否为空或零
function dataListIsNull() {
    var dataListM = $('.tableList .accountTest');
    dataListM.each(function(){
        if(!($(this).val()!="")){
            layer.confirm('成交价格不能为空', {
                time: 0 //不自动关闭
                , btn: ["关闭"]
            });
            flag = true;
            return;
        }
    })
}
/*验证需求名称是否为空，字符串长度3-15*/
function checkUsername() {
    var requireName = $("input:text[name='Username']").val()
    if (requireName === "") {
        $(".nameMsg").text("请填写需求名称")
        return false;
    } else if (requireName.length < 3 || requireName.length > 25) {
        $(".nameMsg").text("字符长度为3-25")
        return false;
    } else {
        $(".nameMsg").text("")
        return true;
    }
    ;
}
/*验证金额是否正确*/
function checkAccount(){
    var curAccount = $(".accountTest").val();
    var reg = /^(([1-9]+[0-9]*\.?)|([0]{1}\.{1}))[0-9]*$/;
    if (reg.test(curAccount) == false) {
        layer.confirm('请输入正确的金额', {
            time: 0 //不自动关闭
            , btn: ["关闭"]
        });
        curAccount = "";
        return false;
    }
}
function getCursorPos(obj) {
    var CaretPos = 0;
    // IE Support
    if (document.selection) {
        obj.focus (); //获取光标位置函数
        var Sel = document.selection.createRange ();
        Sel.moveStart ('character', -obj.value.length);
        CaretPos = Sel.text.length;
    }
    // Firefox/Safari/Chrome/Opera support
    else if (obj.selectionStart || obj.selectionStart == '0')
        CaretPos = obj.selectionEnd;
    return (CaretPos);
};
//定位光标
function setCursorPos(obj,pos){

    if (obj.setSelectionRange) { //Firefox/Safari/Chrome/Opera
        obj.focus(); //
        obj.setSelectionRange(pos,pos);
    } else if (obj.createTextRange) { // IE
        var range = obj.createTextRange();
        range.collapse(true);
        range.moveEnd('character', pos);
        range.moveStart('character', pos);
        range.select();
    }
};
//替换后定位光标在原处,可以这样调用onkeyup=replaceAndSetPos(this,/[^/d]/g,'');
function replaceAndSetPos(obj,pattern,text){
    if ($(obj).val() == "" || $(obj).val() == null) {
        return;
    }
    var pos=getCursorPos(obj);//保存原始光标位置
    var temp=$(obj).val(); //保存原始值
    obj.value=temp.replace(pattern,text);//替换掉非法值
    //截掉超过长度限制的字串（此方法要求已设定元素的maxlength属性值）
    var max_length = obj.getAttribute? parseInt(obj.getAttribute("maxlength")) : "";
    if( obj.value.length > max_length){
            var str1 = obj.value.substring( 0,pos-1 );
        var str2 = obj.value.substring( pos,max_length+1 );
        obj.value = str1 + str2;
    }
    pos=pos-(temp.length-obj.value.length);//当前光标位置
    setCursorPos(obj,pos);//设置光标
    //el.onkeydown = null;
};

/*验证执行时间是否为空*/
function checkTime() {
    var requireTime = $("input[name='beginTime']").val()
    if (requireTime === "") {
        $(".timeMsg").text("请选择执行时间")
        return false;
    } else {
        $(".timeMsg").text(" ")
        return true;
    }
    ;
}
/*验证是否填写需求说明*/
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

/*上传图片的插件begin*/

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
            $("#ImgUp").css("display", "block")
            if (response == true) {
                var json = $.evalJSON(data);
                // console.log(json);
                $("#imgName").text(json.FileName)
                $('#imgUploadFile').attr('UploadFileURL',json.Msg);
                $("#imgDownURL").attr("href",json.Msg)
                checkAttachment()
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
