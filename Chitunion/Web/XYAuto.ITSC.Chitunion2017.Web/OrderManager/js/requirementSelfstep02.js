/**
 * Created by chengj on 2017/3/3.
 */
$(function () {
    /*请求购物车的数据  */
    var orderData = GetRequest();
    var orderid = orderData.orderID;
    var MediaType = orderData.MediaType;
    //查询订单列表
    getOrderList(orderid)
    //表单验证
    $("input:text[name='Username']").blur(function () {
        checkUsername();
    })

    $("#beginTime").click(function () {
        laydate({
                elem: '#beginTime',
                fixed: false,
                istime: true,
                isclear: true,
                format: 'YYYY-MM-DD hh:mm:ss',
                choose: function () {
                    checkTime();
                    var beginData = $("input[name='beginTime']").val();
                    var myDate = new Date();
                    var onDay = myDate.toLocaleDateString();
                    // console.log(onDay);
                    // console.log(beginData);
                    var dataResult = comptime(beginData, onDay)
                    if (dataResult === 102) {
                        layer.confirm('开始日期至少要大于今天', {
                            time: 0 //不自动关闭
                            , btn: ["关闭"]
                        });
                        $("input[name='beginTime']").val("");
                        $("input[name='endTime']").val("");
                    }

                }
            }
        )
    })
    $("#text").on('focus',function(){
        $(this).parents('ul').find('.nameMsg').text('');
    }).on('blur',function(){
        checkUsername();
    });

    $(".textArea").on('focus',function(){
        $(this).parents('ul').find('.explainMsg').text('');
    }).on('blur',function () {
        checkExplain();
    })

    $("#agreementBox").change(function () {
        checkAgreementBox();
    })

    //点击提交表单信息
    $(".button").click(function () {
        //检查表单验证
        checkUsername();
        checkTime();
        checkExplain();
        checkAgreementBox()
        // V1.1.8下AE/运营/超管/销售角色时 上传物料可以为空，非必填项，而广告主下，为必填项
        if(CTLogin.RoleIDs == 'SYS001RL00002' && checkAttachment()==false){
            return
        }
        
        //获取表格里的信息w
        if (checkUsername() == true && checkTime() == true && checkAgreementBox()==true&&checkExplain() == true) {
            var data = getADDetailsInfo();
            console.log(data);
            upOrder(data,MediaType,orderid);
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
            url: "http://www.chitunion.com/api/Authorize/CheckRight",
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
            'uploader': 'http://www.chitunion.com/AjaxServers/UploadFile.ashx',
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
                    $('#imgUploadFile').attr('UploadFileURL', 'http://www.chitunion.com' + json.Msg);
                    $("#imgDownURL").attr("href", 'http://www.chitunion.com' + json.Msg)
                    // V1.1.8下AE/运营/超管/销售角色时 上传物料可以为空，非必填项，而广告主下，为必填项
                    if(CTLogin.RoleIDs == 'SYS001RL00002'){
                        checkAttachment()
                    }
                    //$.jAlert(jsonData.Msg);
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
    $('#agreement').off('click').on('click',function (e) {
        $.openPopupLayer({
            name: "Agreement",
            url: "Agreement.html",
            error: function (dd) {
                alert(dd.status);
            },
            success: function () {
                $('#agreementClose').off('click').on('click',function () {
                    $.closePopupLayer('Agreement')
                })
            }
        })
    })
})

function checkUsername() {
    var requireName = $("input:text[name='Username']").val()
    var usernameRegex = /^\w{3,25}$/;
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
function checkAgreementBox() {
    if ($("#agreementBox").prop('checked')) {
        $("#nextBtn").css("background-color", "#FF9100")
        return true;
    } else {
        $("#nextBtn").css("background-color", "#666")
        return false
    }
}
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


/*验证是否选择时间区间*/
function timeInterval() {
    var BeginDataArr = $("input[name='beginTime']").val().split("-");
    var BeginData = "";
    for (var i = 0; i < BeginDataArr.length; i++) {
        BeginData = BeginData + BeginDataArr[i]
    }
    var EndDataArr = $("input[name='endTime']").val().split("-");
    var EndData = "";
    for (var i = 0; i < EndDataArr.length; i++) {
        EndData = EndData + EndDataArr[i]
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
        url: "http://www.chitunion.com/api/ADOrderInfo/GetByOrderID_ADOrderInfo",
        data: {
            orderid: orderid,
        }
    }, function (data) {
        result = data.Result;
        /*渲染页面*/
        if (data.Result === null) {
            // console.log("购物车没有数据");
        }
        else {
            /*显示广告主名称(非AE下移除节点)*/
            if(CTLogin.RoleIDs != "SYS001RL00005" && CTLogin.RoleIDs != "SYS001RL00004" && CTLogin.RoleIDs != "SYS001RL00001" &&  CTLogin.RoleIDs != "SYS001RL00008"){
                $(".advertiser").remove();
            }else{
                var curAdvName;
                // 媒体主取CustomerUserName  AE超管销售取CustomerName
                if(CTLogin.RoleIDs == 'SYS001RL00003'){
                    curAdvName = data.Result.ADOrderInfo.CustomerUserName;
                }else{
                    curAdvName = data.Result.ADOrderInfo.CustomerName;
                }
                if(data.Result.ADOrderInfo.CRMCustomerID){
                    curAdvName = data.Result.ADOrderInfo.CustomerText;
                }
                $(".advName").text(curAdvName);
            }
            //    1.获取模板中的页面结构
            var mediaTemplate = $("#mediaTemplate").html();
            var html = ejs.render(mediaTemplate, {data: data});

            $('.CustomerID').val(data.Result.ADOrderInfo.CustomerID);
            $('.advertiser').attr('CustomerID',data.Result.ADOrderInfo.CustomerID);
            $('.advertiser').attr('CRMCustomerID',data.Result.ADOrderInfo.CRMCustomerID);
            $("#mediaTable>table").append(html);
            var strResult = JSON.stringify(data.Result)
            $("#mediaTable").attr("result", strResult)
            /*计算价格*/
            var AdjustPriceList=$("td[AdjustPrice]");
            for(var i=0;i<AdjustPriceList.length;i++){
                var thisTd=AdjustPriceList[i];
                var thePrice=$(thisTd).attr("OriginalPrice")-0;
                var theDiscount=$(thisTd).attr("SaleDiscount")-0;
                var AdjustPrice=thePrice*theDiscount;
                var price=AdjustPrice.toFixed(2);
                $(thisTd).attr("AdjustPrice",price);
                $(thisTd).text(formatMoney(price));
            }
            GetOrderPrice();
        }
    }, function () {
        // console.log("订单信息查询失败了");
    })

}
/*获取价格*/
function GetOrderPrice() {
    var AdjustPriceList = $("td[AdjustPrice]");
    var OrderPrice = 0;
    for (var i = 0; i < AdjustPriceList.length; i++) {
        var price = $(AdjustPriceList[i]).attr("AdjustPrice")
        OrderPrice = (price - 0) + OrderPrice;
    }
    OrderPrice = formatMoney(OrderPrice,2,"");
    $("#OrderPrice").text(OrderPrice);
    return OrderPrice;
}
/*2.获取订单补充信息*/
function getADOrderInfo() {
    ADOrderInfo = {
        "OrderID": GetRequest().orderID,
        "MediaType": GetRequest().MediaType,
        "OrderName": $.trim($("input:text[name='Username']").val()),
        "Status": 16002,
        "BeginTime": $("input[name='beginTime']").val(),
        "EndTime": $("input[name='beginTime']").val(),
        "Note": $.trim($(".textArea").val()),
        "UploadFileURL": $('#imgUploadFile').attr('UploadFileURL'),
        "CustomerID": $('.advertiser').attr('CustomerID'),
        "CRMCustomerID":$('.advertiser').attr('CRMCustomerID'),
        "CustomerText" :$.trim($('.advertiser').find('.advName').html())
    }
    return ADOrderInfo;
}
/*2.上传信息*/
function getADDetailsInfo() {
    //获取补充信息
    var ADOrderInfo = getADOrderInfo();
    var strResult = $("#mediaTable").attr("result")
    // console.log(strResult);
    var objResult = JSON.parse(strResult);
    var newResult = {};
    newResult.optType = "2";
    newResult.ADOrderInfo = ADOrderInfo;
    var ADDetailsArr = [];
    for (var j = 0; j < objResult.SubADInfos.length; j++) {
        for (var i = 0; i < objResult.SubADInfos[j].SelfDetails.length; i++) {
            var obj = {};
            obj.MediaType = objResult.SubADInfos[j].SelfDetails[i].MediaType;
            obj.MediaID = objResult.SubADInfos[j].SelfDetails[i].MediaID;
            obj.PubDetailID = objResult.SubADInfos[j].SelfDetails[i].PublishDetailID;
            obj.AdjustPrice = objResult.SubADInfos[j].SelfDetails[i].AdjustPrice;
            obj.AdjustDiscount = objResult.SubADInfos[j].SelfDetails[i].AdjustDiscount;
            obj.ADLaunchDays = objResult.SubADInfos[j].SelfDetails[i].ADLaunchDays;
            obj.ADScheduleInfos = [];
            ADDetailsArr.push(obj);
        }
    }
    newResult.ADDetails = ADDetailsArr;
    return newResult;
}
function upOrder(data,MediaType,orderid) {
    setAjax({
        type: "post",
        url: "http://www.chitunion.com/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
        data: data
    }, function (data) {
        if(data.Status == 0){
            window.location = "/OrderManager/requirement03.html?MediaType=" + MediaType + "&orderID=" + orderid;
        }else{
            layer.msg(data.Message);
        }
    }, function () {
        // console.log("提交订单信息第二步失败了");
    })
}
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