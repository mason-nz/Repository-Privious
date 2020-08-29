/**
 * Created by chengj on 2017/3/3.
 */
$(function () {
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
    /*获取解码后的用户ID*/
    function GetUserId() {
        var url = location.search; //获取url中"?"符后的字串
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = strs[i].split("=")[1];
            }
        }
        return theRequest;
    }
    var orderData = GetRequest();
    /*获取订单号*/
    var orderid = orderData.orderID;
    var MediaType = orderData.MediaType;
    /*获取媒体类型ID*/
    if (orderid) {
        getOrderList(orderid);
    } else {
        GetInfo_ShoppingCart(14002);
    }
    //表单验证
    $("#beginTime").click(function () {
        laydate({
                elem: '#beginTime',
                fixed: false,
                istime: true,
                isclear: true,
                format: 'YYYY-MM-DD',
                choose: function () {
                    checkTime();
                    var beginData = $("#beginTime").val();
                    var myDate = new Date();
                    var onDay = myDate.toLocaleDateString();
                    var dataResult = comptime(beginData, onDay)
                    if (dataResult === 102) {
                        layer.alert('开始日期至少要大于今天');
                        $("#beginTime").val("");
                    }else{
                        if(!compare(date,$('#endTime').val())){
                            layer.alert('周期不能大于半年！');
                            $("#beginTime").val("");
                        }
                    }

                }
            }
        )
    });
    $("#endTime").click(function () {
        laydate({
                elem: '#endTime',
                fixed: false,
                isclear: true,
                format: 'YYYY-MM-DD',
                choose: function () {
                    checkTime();
                    var BeginTime =   $("#beginTime").val();
                    var endTime =  $("#endTime").val();
                    var dataResult = comptime(BeginTime, endTime)
                    if (dataResult === 201) {
                        layer.alert('结束时间不能小于开始时间');
                        $("#endTime").val("");
                    }else{
                          if(!compare($('#beginTime').val(),date)){
                            layer.alert('周期不能大于半年！');
                            $("#endTime").val("");
                        }
                    }
                }
            }
        )
    });

    $(".textArea").blur(function () {
        /*需求说明验证*/
        checkExplain();
    })

    $("#agreementBox").change(function () {
        /*是否同意协议*/
        checkAgreementBox();
    });
    /*当鼠标点击需求名称文本框时，后面的提示文字消失*/
    $("#text").off('click').on('click',function(){
        $(this).siblings('.nameMsg').hide();
    });
    /*需求名称聚焦时，验证需求名称是否符合要求*/
    $("#text").blur(function () {
        checkUsername()
    });

    //点击提交表单信息
    $(".button").click(function () {
        //检查表单验证
        checkUsername();
        checkTime();
        checkExplain();
        checkAgreementBox();
        checkAttachment();
        // 获取表格里的信息
        if (checkUsername() == true && checkTime() == true && checkAgreementBox() == true&&checkExplain()==true&& checkAttachment()==true) {
            var userID = GetUserId().userID;
            var pushData= getADDetailsInfo(orderid,userID);
            upOrder(pushData);
        }
    })

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
    /*读取cookie*/
    function getCookie(name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
        if (arr = document.cookie.match(reg))
            return unescape(arr[2]);
        else
            return null;
    }
    /*URL编码*/
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
        /*文件上传*/
        $('#divPopLayer').unbind('click').bind('click', function () {
            $.openPopupLayer({
                name: "popLayerDemo",
                url: "/appSchedulePopup.html?r=" + Math.random(),
                error: function (dd) {
                    alert(dd.status);
                }
            });
        });
        $('#uploadify').uploadify({
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
                $("#ImgUp").css("display","block");
                $("#uploadify-queue").css("display", "none");
                if (response == true) {
                    var json = $.evalJSON(data);
                    // console.log(json);
                    $("#imgName").text(json.FileName);
                    $('#imgUploadFile').attr('UploadFileURL',json.Msg);
                    $("#imgDownURL").attr("href",json.Msg);
                    checkAttachment();
                }
            },
            'onProgress': function (event, queueID, fileObj, data) { },
            'onUploadError': function (event, queueID, fileObj, errorObj) {
                //enableConfirmBtn();
            },
            'onSelectError':function(file, errorCode, errorMsg){

            }
        });
    });
    /*点击“赤兔联盟协议”，查看协议内容*/
    $('#agreement').off('click').on('click',function (e) {
        $.openPopupLayer({
            name: "Agreement",
            url: "Agreement.html",
            error: function (dd) {
                alert(dd.status);
            },
            success: function () {
                $('#agreementClose').off('click').on('click',function () {
                    $.closePopupLayer('Agreement');
                })
            }
        });
    });


// 判断时间间隔是否大于半年
function compare(begin,end){
    var b_year = begin.split('-')[0];
    var b_month = begin.split('-')[1];
    var b_day = begin.split('-')[2];

    var e_year = end.split('-')[0];
    var e_month = end.split('-')[1];
    var e_day = end.split('-')[2];

    if(e_year- b_year>=1){
        return false;
    }else if(e_month - b_month >=6){
        return false;
    }else{
        if(e_month - b_month== 5){
            if(e_day == b_day){
                return false
            }else{
                return true
            }
        }else{
            return true;
        }
    }
}

/*验证是否选择时间区间*/
function timeInterval() {
    var BeginDataArr =  $("input[name='beginTime']").val().split("-");
    var BeginData = "";
    for (var i = 0; i < BeginDataArr.length; i++) {
        BeginData = BeginData + BeginDataArr[i]
    }
    var EndDataArr =  $("input[name='endTime']").val().split("-");
    var EndData = "";
    for (var i = 0; i < EndDataArr.length; i++) {
        EndData = EndData + EndDataArr[i]
    }
    if (BeginDataArr == 0) {
        return false;
    }
    else if (EndData - BeginData > 0) {
        return true;
    } else {
        return false;
    }
}

/*验证执行时间非空*/
function checkTime() {
    var requireTime = $("input[name='beginTime']").val();
    var requireETime = $("input[name='endTime']").val();
    if (requireTime === ""||requireETime==="") {
        $(".timeMsg").text("请选择执行时间")
        return false;
    } else {
        $(".timeMsg").text(" ")
        return true;
    };
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
/*验证需求名称*/
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
/*验证需求说明*/
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
/*验证是否上传附件*/
function checkAttachment() {
    var imgName = $("#imgName").text()
    if (imgName === "") {
        $(".imgMsg").text("请上传附件")
        return false;
    } else {
        $(".imgMsg").text(" ")
        return true;
    };
}
/*根据是否接受协议，改变“提交需求”的背景颜色*/
function checkAgreementBox() {
    if($("#agreementBox").prop('checked')){
        $("#nextBtn").css("background-color","#FF9100")
        return true;
    }else{
        $("#nextBtn").css("background-color","#666")
        return false
    }
}

/*验证是否存在CPMCPD*/
function checkCPMCPD() {
    var cpmlist = $(".CPM");
    var cpdlist = $(".CPD");
    if (cpmlist.length == 0) {
        $("#CPM").remove()
    }
    if (cpdlist.length == 0) {
        $("#CPD").remove()
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
        }, function (data) {
            // console.log("订单信息查询成功了");
            /*渲染页面*/
            if (data.Result === null) {
                // console.log("购物车没有数据");
            }
            else {
                /*显示广告主名称(非AE下移除节点)*/
                if(CTLogin.RoleIDs != "SYS001RL00005" && CTLogin.RoleIDs != "SYS001RL00004" && CTLogin.RoleIDs != "SYS001RL00001"){
                    $(".advertiser").remove();
                }else{
                    var curAdvName;
                    if(data.Result.ADOrderInfo.CustomerName != ""){
                        curAdvName = data.Result.ADOrderInfo.CustomerName;
                    }else{
                        curAdvName = data.Result.ADOrderInfo.CustomerUserName;
                    }
                    $(".advName").text(curAdvName);
                }
                // console.log("请求的类型是APP");
                //    1.获取模板中的页面结构
                $('.CustomerID').val(data.Result.ADOrderInfo.CustomerID);
                $("#beginTime").val(data.Result.ADOrderInfo.BeginTime.substr(0,10))
                $("#endTime").val(data.Result.ADOrderInfo.EndTime.substr(0,10))
                var appTemplate = $("#appTemplate").html();
                var html = ejs.render(appTemplate, {data: data})
                $("#appTable>table").append(html);
                var strResult=JSON.stringify(data.Result)
                /*计算价格*/
                var AdjustPriceList=$("td[AdjustPrice]");
                for(var i=0;i<AdjustPriceList.length;i++){
                    var thisTd=AdjustPriceList[i];
                    var thePrice=$(thisTd).attr("OriginalPrice")-0;
                    var theDiscount=$(thisTd).attr("SaleDiscount")-0;
                    var ADLaunchDays=$(thisTd).attr("ADLaunchDays")-0;
                    var OPrice=thePrice*theDiscount;
                    var AdjustPrice=thePrice*theDiscount*ADLaunchDays;
                    var price=OPrice.toFixed(2);
                    var AdjustPrice=AdjustPrice.toFixed(2);
                    $(thisTd).attr("AdjustPrice",AdjustPrice);
                    $(thisTd).text(formatMoney(AdjustPrice));

                    $(thisTd).parent().children().eq(7).text(formatMoney(price)+"/天/轮播");
                    if($(thisTd).parent().attr("ADLaunchIDs")=="11002"){
                        $(thisTd).parent().children().eq(7).text(formatMoney(price)+"/CPM");
                    }
                }

                $("#appTable").attr("result",strResult);
                GetOrderPrice();
            }
        checkCPMCPD()
        }, function () {
            // console.log("订单信息查询失败了");
        }
    )
}
/*获取价格*/
function GetOrderPrice() {
    var AmountList = $("td[AdjustPrice]");
    var OrderPrice = 0;
    for (var i = 0; i < AmountList.length; i++) {
        var price = $(AmountList[i]).attr("AdjustPrice")
        OrderPrice = (price - 0) + OrderPrice;
    }
    OrderPrice = formatMoney(OrderPrice);
    $("#OrderPrice").text(OrderPrice.substr(1));
    return OrderPrice;
}
/*2.获取订单补充信息*/
function getADOrderInfo(orderid,userID) {
    var requireName = $("input:text[name='Username']").val()
    var requireExplain = $(".textArea").val()
    var UploadFileURL = $('#imgUploadFile').attr('UploadFileURL')
    // console.log(UploadFileURL);
    var requireTime = $("input[name='beginTime']").val()
    var requireETime = $("input[name='endTime']").val()
    var ADOrderInfo = {
        "OrderID": orderid,
        "MediaType": 14002,
        "OrderName": requireName,
        "Status": 16002,
        "BeginTime":requireTime,
        "EndTime": requireETime,
        "Note": requireExplain,
        "UploadFileURL": UploadFileURL,
        "CustomerID": userID
    }
    return ADOrderInfo;
}
/*2.获取广告位信息*/
function getADDetailsInfo(orderid,userID) {
    //获取补充信息
    var ADOrderInfo = getADOrderInfo(orderid,userID);
    var strResult=$("#appTable").attr("result")
    var objResult=JSON.parse(strResult);
    var newResult = {};
    newResult.optType = "2";
    newResult.ADOrderInfo = ADOrderInfo;
    newResult.ADDetails = [];
    for (var j = 0; j < objResult.SubADInfos.length; j++) {
        for (var i = 0; i < objResult.SubADInfos[j].APPDetails.length; i++) {
            obj={};
            obj.MediaType = objResult.SubADInfos[j].APPDetails[i].MediaType;
            obj.MediaID = objResult.SubADInfos[j].APPDetails[i].MediaID;
            obj.PubDetailID = objResult.SubADInfos[j].APPDetails[i].PublishDetailID;
            obj.AdjustPrice =objResult.SubADInfos[j].APPDetails[i].AdjustPrice;
            obj.AdjustDiscount =objResult.SubADInfos[j].APPDetails[i].AdjustDiscount;
            obj.ADLaunchDays = objResult.SubADInfos[j].APPDetails[i].ADLaunchDays;
            obj.ADScheduleInfos = objResult.SubADInfos[j].APPDetails[i].ADScheduleInfos;
            newResult.ADDetails.push(obj);
        }
    }
    return newResult;
}
/*上传订单信息*/
function upOrder(pushData) {
    setAjax({
        type: "post",
        url: "/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
        data:pushData
    }, function (data) {
        if(data.Status == 0){
           /*提交完成页面跳转至*/
            var orderid = GetRequest().orderID;
            /*获取媒体类型ID*/
            var MediaType = GetRequest().MediaType;
            window.location = "/OrderManager/requirement03.html?MediaType="+MediaType+"&orderID="+orderid; 
        }else{
            // console.log(data.Message);
        }
        
    }, function () {
        // console.log("提交订单信息第二步失败了");
    })
}
});