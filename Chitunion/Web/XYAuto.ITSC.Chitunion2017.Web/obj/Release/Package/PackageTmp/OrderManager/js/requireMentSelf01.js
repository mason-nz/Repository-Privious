
$(function () {
    // 获取url？后面的参数
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
    /*请求购物车的数据  */
    var orderData = GetRequest();
    var orderid = orderData.orderID;
    var MediaType = orderData.MediaType;


    if (!orderid) {
        GetInfo_ShoppingCart(MediaType);
    } else {
        getOrderList(orderid);
    }
    var userData = GetUserId();
    if(userData.userID){
        var userID = userData.userID;
    }else if($('.CustomerID').val()){
        var userID = $('.CustomerID').val();
    }else{
        var userID = "gt86ZRCRjng%3d";
    }
    SelectAd()
    /*点击删除某一项数据*/
    $("#tableList").on("click", ".delBtn", function () {
        //信息框-例2
        var thisTable = $(this)
        layer.confirm('是否删除此项？', {
            time: 0 //不自动关闭
            , btn: ['删除', '取消']
            , yes: function (index) {
                var MediaID = thisTable.parent().parent().attr("MediaID");
                var PublishDetailID = thisTable.parent().parent().attr("PublishDetailID");
                Operate_ShoppingCart(MediaType, MediaID, PublishDetailID)
                thisTable.parent().parent().remove();
                GetMeidaNum();
                var orderList = GetMeidaNum()
                if (orderList == 0) {
                    $("#nextBtn").remove()
                }
                GetOrderPrice()
                SelectAd()
                layer.close(index);
                layer.msg('操作成功', {time: 400});
            }
        });

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
                        var thePrice = $("input[type='radio']:checked").parent().parent().attr("Price") - 0;
                        var ADTypeName = ADPosition1;
                        var PublishDetailID = ADDetailID
                        if (ADTypeName != "undefined-undefined-undefined") {
                            var Price = formatMoney(thePrice,2);
                            // console.log(Price);
                            $(".location").children().eq(1).text(ADTypeName);
                            $(".location").attr("ADTypeName", ADTypeName);
                            $(".location").attr("PublishDetailID", PublishDetailID);
                            $(".location").attr("AdjustPrice", thePrice.toFixed(2));
                            $(".location").children().eq(2).text(Price);
                            $(".location").children().eq(2).attr("AdjustPrice", thePrice.toFixed(2));
                        }
                        SelectAd()
                        GetOrderPrice();
                        $(".location").removeClass("location")
                        $.closePopupLayer("popLayerDemo");
                    })
                    $(".but_keep").click(function () {
                        $.closePopupLayer("popLayerDemo");
                        $(".location").removeClass("location")
        //    获取被选中的按钮
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
                        var thePrice = $("input[type='radio']:checked").parent().parent().attr("Price") - 0;
                        var ADTypeName = ADPosition1 + "-" + ADPosition2;
                        var PublishDetailID = ADDetailID
                        if (ADTypeName != "undefined-undefined-undefined") {
                            var Price = formatMoney(thePrice,2);
                            // console.log(Price);
                            $(".location").children().eq(1).text(ADTypeName);
                            $(".location").attr("ADTypeName", ADTypeName);
                            $(".location").attr("PublishDetailID", PublishDetailID);
                            $(".location").attr("AdjustPrice", thePrice.toFixed(2));
                            $(".location").children().eq(2).text(Price);
                            $(".location").children().eq(2).attr("AdjustPrice", thePrice.toFixed(2));
                        }
                        SelectAd()
                        GetOrderPrice();
                        $(".location").removeClass("location")
                        $.closePopupLayer("popLayerDemo");
                    })
                    $(".but_keep").click(function () {
                        $.closePopupLayer("popLayerDemo");
                        $(".location").removeClass("location")

                        //    获取被选中的按钮
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
                        var thePrice = $("input[type='radio']:checked").parent().parent().attr("Price") - 0;
                        var ADTypeName = ADPosition2 + "-" + ADPosition3 + "-" + ADPosition1;
                        var PublishDetailID = ADDetailID

                        if (ADTypeName != "undefined-undefined-undefined") {
                            var Price = formatMoney(thePrice,2);
                            // console.log(Price);
                            $(".location").children().eq(1).text(ADTypeName);
                            $(".location").attr("ADTypeName", ADTypeName);
                            $(".location").attr("PublishDetailID", PublishDetailID);
                            $(".location").attr("AdjustPrice", thePrice.toFixed(2));
                            $(".location").children().eq(2).text(Price);
                            $(".location").children().eq(2).attr("AdjustPrice", thePrice.toFixed(2));
                        }
                        SelectAd()
                        GetOrderPrice();
                        $(".location").removeClass("location")
                        $.closePopupLayer("popLayerDemo");
                    })
                    $(".but_keep").click(function () {
                        $.closePopupLayer("popLayerDemo");
                        $(".location").removeClass("location")
                    //    获取被选中的按钮
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
    //保存并继续添加账号
    $(".getShopCar").click(function () {
        // V1,.1.8下，AE和运营广告主名称权限相同
        if(CTLogin.RoleIDs == "SYS001RL00005" || CTLogin.RoleIDs == "SYS001RL00001" || CTLogin.RoleIDs == "SYS001RL00004" || CTLogin.RoleIDs == "SYS001RL00008"){
            if($(".advertiser input").val() == ""){
                var data = getOrderInfo();
                upShoppingCart(data);
            }else{
                if(checkAdvName($('#adMatser option:checked').attr('value')) == true){
                    var data = getOrderInfo();
                    upShoppingCart(data);
                }
            }
        }else{
            var data = getOrderInfo();
            upShoppingCart(data);
        }
        
    })
    $("#nextBtn").click(function () {
        if (SelectAd() === true) {
            var curUserID,CustomerName;
            if (!orderid) {
                if(CTLogin.RoleIDs == "SYS001RL00005" || CTLogin.RoleIDs == "SYS001RL00001" || CTLogin.RoleIDs == "SYS001RL00004" || CTLogin.RoleIDs == "SYS001RL00008"){
                    if(checkAdvName($('#adMatser option:checked').attr('value')) == true){
                        var data = getOrderInfo();
                        AddOrUpdate_ADOrderInfo(MediaType, data,"");
                    }
                }else{
                    var data = getOrderInfo(MediaType,"",userID);
                    AddOrUpdate_ADOrderInfo(MediaType, data,"");
                }
            } else {
                if(CTLogin.RoleIDs == "SYS001RL00005" || CTLogin.RoleIDs == "SYS001RL00001" || CTLogin.RoleIDs == "SYS001RL00004" || CTLogin.RoleIDs == "SYS001RL00008"){
                    if(checkAdvName($('#adMatser option:checked').attr('value')) == true){
                        var data = getOrderInfo();
                        AddOrUpdate_ADOrderInfo(MediaType, data, orderid);
                    }
                }else{
                    var data = getOrderInfo();
                    AddOrUpdate_ADOrderInfo(MediaType, data, orderid);
                }
            }
        } 
    })

    /*获取当前广告主ID*/
    function getUserID(){
        var userID = $(".advertiser").attr("userID");
        if(userID == undefined){
            return "gt86ZRCRjng%3d"
        }else{
            return userID
        }
    }

    /*验证广告主填写是否正确，若正确，将对应的userID绑定到标签上，若不正确，显示错误提示信息*/
    function checkAdvName(IsAEAuth){
        var flag = true;
        var advName = $(".advertiser input").val();
        if(advName != ""){
            //如果是销售角色，需验证广告主名称是否为空，不需调验证是否存在。但如果用户选择了库里有的广告主名称，需要把对应的ID返给后台
            /*获取当前广告主名称之后，进行模糊查询，当前广告主是否存在*/
            $.ajax({
                type: "get",
                url: "http://www.chitunion.com/api/ADOrderInfo/GetADMaster?v=1_1",
                data: {
                    NameOrMobile: advName,
                    IsAEAuth:IsAEAuth
                },
                dataType: 'json',
                async: false,
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                success:function(data){
                    /*count为广告主名称不匹配的数量*/
                    var count = 0;
                    if(data.Result == null){
                        $(".advertiser .advMsg").html("<img src='../images/icon21.png'> 广告主不存在，请重新输入！");
                        $("#nextBtn").css("background-color", "#666");
                        flag = false;
                    }else{
                        for(var i=0;i<data.Result.length;i++){
                            if(data.Result[i].Mobile != advName){
                                count++;
                            }
                            if(data.Result[i].TrueName != advName){
                                count++;
                            }

                        }
                        /*由于查询时会匹配数据中的真实名称和手机号，所以若广告主名称不匹配的数量 = 总查询的数据总量时，说明都不匹配*/
                        if(count == data.Result.length * 2){
                            $(".advertiser .advMsg").html("<img src='../images/icon21.png'> 广告主不存在，请重新输入！");
                            flag = false;
                            $("#nextBtn").css("background-color", "#666");
                        }else{
                            /*匹配时再次调用接口，把当前广告主名称对应的userID绑定到页面上*/
                            $.ajax({
                                type: "get",
                                url: "http://www.chitunion.com/api/ADOrderInfo/GetADMaster?v=1_1",
                                data: {
                                    NameOrMobile: advName,
                                    IsAEAuth:IsAEAuth
                                },
                                dataType: 'json',
                                async: false,
                                xhrFields: {
                                    withCredentials: true
                                },
                                crossDomain: true,
                                success:function(data){
                                    for(var i=0;i<data.Result.length;i++){
                                        for(j in data.Result[i]){
                                            if(advName == data.Result[i][j]){
                                                $(".advertiser").attr("userID",data.Result[i].UserID);
                                            }
                                        }
                                    }
                                }
                            });
                            $(".advertiser .advMsg").html("");
                            flag = true;
                        }
                    }
                }
            });
        }else{
            $(".advertiser .advMsg").html("<img src='../images/icon21.png'> 广告主名称不能为空");
            flag = false;
        }
        return flag;
    }
    /*验证广告主非空*/
    function AdvNameIsNull(){
        var advName = $(".advertiser input").val();
        if(advName == ""){
            $(".advertiser .nameMsg").text("广告主不能为空，请输入!");
            $("#nextBtn").css("background-color", "#666");
            return false;
        }
    }
    /*1.从购物车获取数据并进行模糊查询*/
    function GetInfo_ShoppingCart(MediaType) {
        setAjax(
            {
                type: "get",
                url: "http://www.chitunion.com/api/ShoppingCart/GetInfo_ShoppingCart",
                data: {
                    MediaType: MediaType,
                }
            },
            function (data) {

                if (data.Result === null) {
                    GetMeidaNum()
                    var orderList = GetMeidaNum()
                    if (orderList == 0) {
                        $("#nextBtn").remove()
                    }
                }
                else {
                    if (data.Result.MediaType != 14002) {
                        // console.log("请求的数据是自媒体");
                        /*如果不是AE，不显示广告主------V1.1.8下，如果是销售角色，也显示广告主名称*/
                        if(CTLogin.RoleIDs != "SYS001RL00005" && CTLogin.RoleIDs != "SYS001RL00001" && CTLogin.RoleIDs != "SYS001RL00004" && CTLogin.RoleIDs != "SYS001RL00008"){
                            $(".advertiser").remove();
                        }else{
                            /*判断是代客户下单进入还是一级菜单立即投放进入*/
                            if(userData.flag == 0){
                                // console.log("AE代客户下单");
                                setAjax(
                                    {
                                        type: "get",
                                        url: "http://www.chitunion.com/api/UserInfo/GetUserInfoByUserID",
                                        data:{
                                            userid: GetUserId().userID
                                        }
                                    },
                                    function(data){
                                        if(data.Status == 0){
                                            if(data.Result.TrueName != ""){
                                                $(".advertiser input").val(data.Result.TrueName);
                                            }else{
                                                $(".advertiser input").val(data.Result.UserName);
                                            }
                                            $(".advertiser input").attr("disabled","disabled");
                                        }else{
                                            alert(data.Message);
                                        }
                                    }
                                );
                                
                            }else{
                                // console.log("一级菜单进入");
                                $(".advertiser input").on("keyup",function(){
                                    var nameOrMobile = $(this).val();
                                    if(nameOrMobile.length >0 && nameOrMobile != " "){
                                        GetADMaster(nameOrMobile,$('#adMatser option:checked').attr('value'));
                                    }

                                });
                                $(".advertiser input").off("focus").on("focus",function(){
                                    var nameOrMobile = $.trim($(this).val());
                                    if(nameOrMobile == ""){
                                        setAjax({
                                            type: "get",
                                            url: "http://www.chitunion.com/api/ADOrderInfo/GetADMaster?v=1_1",
                                            data: {
                                                NameOrMobile: nameOrMobile,
                                                IsAEAuth:0
                                            }
                                        },
                                        function (data) {
                                            var availableAdv = [];
                                            $(".advertiser input").autocomplete({
                                                source: availableAdv
                                            })
                                        },
                                        function(){}
                                    )}
                                })
                                $('#adMatser').off('change').on('change',function(){
                                    $('.advertiser input').val('');
                                })
                            }
                        }
                        //    1.获取模板中的页面结构
                        var mediaTemplate = $("#mediaTemplate").html();
                        var html = ejs.render(mediaTemplate, {data: data})
                        $("#mediaTable>table").append(html);
                        GetMeidaNum()
                        GetOrderPrice()
                        SelectAd()


                    }
                }

            },
            function () {
                // console.log("请求购物车数据失败了");
            }
        )
    }

    /*模糊查询广告主名称，传入参数为查询方式：0为查询自己创的和授权的，1为只查询授权的*/
    function GetADMaster(nameOrMobile,IsAEAuth){
        setAjax(
            {
                type: "get",
                url: "http://www.chitunion.com/api/ADOrderInfo/GetADMaster?v=1_1",
                data: {
                    NameOrMobile: nameOrMobile,
                    IsAEAuth:IsAEAuth
                }
            },
            function (data) {
                var availableAdv = [];
                if(data.Status == 0){
                    for(var i=0;i<data.Result.length;i++){
                        if(data.Result[i].Mobile.toLowerCase().indexOf(nameOrMobile.toLowerCase()) != -1){
                            if(availableAdv.length<10){
                                availableAdv.push(data.Result[i].Mobile);
                            }
                        }
                        if(data.Result[i].TrueName.toLowerCase().indexOf(nameOrMobile.toLowerCase()) != -1){
                            if(availableAdv.length<10){
                                availableAdv.push(data.Result[i].TrueName);
                            }
                        }
                    }
                }
                $(".advertiser input").autocomplete({
                    source: availableAdv
                })
            },
            function(){}
        )
    }
    /*获取选择广告位的媒体个数*/
    function SelectAd() {
        /*获取选择了广告位的媒体个数*/
        var price = GetOrderPrice()
        var trList = $(".tableList");
        var noChoseNum = 0;
        for (var i = 0; i < trList.length; i++) {
            var thisTr = trList[i];
            var PublishDetailID = $(thisTr).attr("PublishDetailID")
            if ($(thisTr).attr("PublishDetailID") == "0" || $(thisTr).attr("PublishDetailID") == "-2") {
                noChoseNum++;
            }
        }
        if (noChoseNum == 0 && price != 0.00) {
            $("#nextBtn").css("background-color", "#FF9100")
            return true;
        } else {
            $("#nextBtn").css("background-color", "#666");
        }
    }
    /*2.获取媒体总个数的函数*/
    function GetMeidaNum() {
        var trList = $("tr[IsSelected='true']").length;
        $("#tableNum").text(trList);
        return trList;
    }
    /*3.获取总价格*/
    function GetOrderPrice() {
        var AmountList = $("td[AdjustPrice]");
        var OrderPrice = 0;
        for (var i = 0; i < AmountList.length; i++) {
            var price = $(AmountList[i]).attr("AdjustPrice")
            OrderPrice = (price - 0) + OrderPrice;
        }
        if (OrderPrice == 0.00 || OrderPrice == 0) {
            $("#OrderPrice").text("0.00");
        } else {
            // OrderPrice = OrderPrice.toFixed(2)
            OrderPrice = formatMoney(OrderPrice,2,"");
            $("#OrderPrice").text(OrderPrice);
        }
        return OrderPrice;
    }
    /*4.删除提交信息*/
    function Operate_ShoppingCart(MediaType, MediaID, PublishDetailID) {

        setAjax({
                type: "post",
                url: "http://www.chitunion.com/api/ShoppingCart/Operate_ShoppingCart",
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
        setAjax(
            {
                type: "get",
                url: "http://www.chitunion.com/api/Periodication/GetPublishBasicInfoByID",
                data: {
                    PubIDOrMediaID: MediaID,
                    MediaType: MediaType
                }
            },
            function (data) {
                var table_html = $("#pubBasicInfo").html()
                var html = ejs.render(table_html, {data: data})
                $("#tableInfo>table").empty().append(html);
            },
            function () {
            }
        )
    }
    /*通过当前的URL的userID，设置flag标识，以此来判断系统角色（是否为AE），并返回在url后传递的参数：用户ID和flag标识*/
    function userID1(userID){
        if(userID != "gt86ZRCRjng%3d"){
            if(GetUserId().flag){
                return '&userID='+ userID + '&flag=' + GetUserId().flag;
            }else{
                return '&userID='+ userID + '&flag=0';
            }
        }else{
            return '&userID=gt86ZRCRjng%3d&flag=1'
        }
    }
    
    /*6.点击保存订单跳转到购物车*/
    function upShoppingCart(data) {
        var MediaType = GetRequest().MediaType;
        var orderid = GetRequest().orderID?GetRequest().orderID:"";
        if(data != undefined && data.ADDetails.length != 0){
            setAjax({
            type: "post",
            url: "http://www.chitunion.com/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
            data: data
            }, function (data) {        
                if(data.Status == 0){
                    var curUserID = $(".advertiser").attr("userID");
                    var nextOrderID = orderid ? orderid : data.Message;
                     if (MediaType == 14001) {
                        if(curUserID){
                            window.location = "/OrderManager/wx_list_check.html?MediaType=" + MediaType + "&orderID=" + nextOrderID +"&OrderState=1" + "&userID=" + curUserID +"&flag="+GetUserId().flag;
                        }else{
                            window.location = "/OrderManager/wx_list_check.html?MediaType=" + MediaType + "&orderID=" + nextOrderID +"&OrderState=1" + userID1(userID);

                        }
                    } else if (MediaType == 14003) {
                        if(curUserID){
                            window.location = "/OrderManager/wb_list_check.html?MediaType=" + MediaType + "&orderID=" + nextOrderID +"&OrderState=1" + "&userID=" + curUserID +"&flag="+GetUserId().flag;
                        }else{
                            window.location = "/OrderManager/wb_list_check.html?MediaType=" + MediaType + "&orderID=" + nextOrderID +"&OrderState=1" + userID1(userID);

                        }
                    } else if (MediaType == 14004) {
                        if(curUserID){
                            window.location = "/OrderManager/sp_list_check.html?MediaType=" + MediaType + "&orderID=" + nextOrderID +"&OrderState=1" + "&userID=" + curUserID +"&flag="+GetUserId().flag;
                        }else{
                            window.location = "/OrderManager/sp_list_check.html?MediaType=" + MediaType + "&orderID=" + nextOrderID +"&OrderState=1" + userID1(userID);

                        }
                    } else {
                        if(curUserID){
                            window.location = "/OrderManager/zb_list_check.html?MediaType=" + MediaType + "&orderID=" + nextOrderID +"&OrderState=1" + "&userID=" + curUserID +"&flag="+GetUserId().flag;
                        }else{
                            window.location = "/OrderManager/zb_list_check.html?MediaType=" + MediaType + "&orderID=" + nextOrderID +"&OrderState=1" + userID1(userID);

                        }
                    }
                }else{
                    layer.msg(data.Message);
                }
            }, function () {
            })
        }else {
            // 修改订单
            setAjax({
                url:'http://www.chitunion.com/api/ADOrderInfo/AddOrUpdate_ADOrderInfo',
                type:'post',
                data:{
                    "optType" : 2,
                    "ADOrderInfo" : {
                        "OrderID" : orderid,
                        "MediaType" : MediaType,
                        "Status" : 16001//CT20170324663
                    },
                    "ADDetails" : []
                }
            },function (data) {
                if(MediaType==14003){
                window.location='/OrderManager/wb_list.html';
                }
                if(MediaType==14001){
                    window.location = '/OrderManager/wx_list.html';
                }
                if(MediaType==14004){
                    window.location='/OrderManager/sp_list.html';
                }
                if(MediaType==14005){
                    window.location='/OrderManager/zb_list.html';
                }
            })
        }
    }


    /*6.点下一步 提交订单数据  媒体ID，发布形式描述， 价格
     * 封装获取订单信息函数*/
    function getOrderInfo() {
        var getTr = $(".tableList");
        var ADDetails = []
        /*遍历*/
        for (var j = 0; j < getTr.length; j++) {
            var MediaID = $(getTr[j]).attr("MediaID");
            var PublishDetailID = $(getTr[j]).attr("PublishDetailID");
            var AdjustPrice = $(getTr[j]).attr("AdjustPrice") || "0.00";
            var ADScheduleInfos = [];
            var OrderPrice = $("#OrderPrice")
            var ObjStory = {
                "MediaType": MediaType,
                "MediaID": MediaID,
                "PubDetailID": PublishDetailID,
                "AdjustPrice": AdjustPrice,
                "AdjustDiscount": 0,
                "ADLaunchDays": 0,
                "ADScheduleInfos": ADScheduleInfos
            }
            ADDetails[j] = ObjStory
        }
        //如果未保存成草稿，只获取数据。如果保存成草稿后点击下一步，判断是否有媒体信息
        var getOrderInfo = {
            "optType": 1,
            "ADOrderInfo": {
                "OrderID": GetRequest().orderID?GetRequest().orderID:'',
                "MediaType":GetRequest().MediaType,
                "OrderName": "",
                "Status": 16001,
                "BeginTime": "1990-01-01",
                "EndTime": "1990-01-01",
                "Note": "",
                "UploadFileURL": "",
                "CustomerID":"gt86ZRCRjng%3d",
                "CRMCustomerID" : "",
                "CustomerText" : ""
            },
            "ADDetails": ADDetails
        }

        if($.trim($('.advertiser').attr('userid')) != ""){
            //赤兔
            if($('#adMatser option:checked').attr('value') == 0){
                getOrderInfo.ADOrderInfo.CustomerID = $.trim($('.advertiser').attr('userid'));
            //CRM
            }else{
                getOrderInfo.ADOrderInfo.CRMCustomerID = $.trim($('.advertiser').attr('userid'));
            }
        }
        getOrderInfo.ADOrderInfo.CustomerText = $.trim($(".advertiser input").val());
        return getOrderInfo;
    }
    /*调用API 发起AJAX请求 提交订单数据*/
    function AddOrUpdate_ADOrderInfo(MediaType, data, orderid) {
        setAjax(
            {
                type: "post",
                url: "http://www.chitunion.com/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
                data: data
            },
            function (data) {
                if(data.Status != 0){
                    alert(data.Message);
                }else{
                    var curUserID = $(".advertiser").attr("userID");
                    var curOrderID = GetRequest().orderID?GetRequest().orderID:"";
                    if( !curOrderID){
                        curOrderID = data.Message;
                    }
                    if(curUserID != "" && curUserID != undefined){
                        window.location= "/OrderManager/requirementSelf02.html?MediaType=" + MediaType + "&orderID=" + curOrderID + "&OrderState=1" + "&userID=" + curUserID + "&flag="+GetUserId().flag;
                    }else{
                        window.location= "/OrderManager/requirementSelf02.html?MediaType=" + MediaType + "&orderID=" + curOrderID + "&OrderState=1" + userID1(userID);
                    }
                   
                }
            },
            function () {
                alert("数据提交失败了");
            }
        )
    }


    /*1.根据订单号读取数据列表*/
    function getOrderList(orderid) {
        setAjax({
            type: "get",
            url: "http://www.chitunion.com/api/ADOrderInfo/GetByOrderID_ADOrderInfo",
            data: {
                orderid: orderid,
            }
        }, function (data) {
            /*渲染页面*/
            if (data.Result === null) {
                // console.log("购物车没有数据");
            }
            else {
                /*从后台数据中获取当前的广告主名称*/
                var curAdvName;
                if(data.Result.ADOrderInfo.CustomerName != ""){
                    curAdvName = data.Result.ADOrderInfo.CustomerName;
                }else{
                    curAdvName = data.Result.ADOrderInfo.CustomerUserName;
                }
                if(data.Result.ADOrderInfo.CRMCustomerID){
                    curAdvName = data.Result.ADOrderInfo.CustomerText;
                }
                // console.log("请求的类型是APP");
                /*如果不是AE、运营，不显示广告主----V1.1.8下，增加角色销售，也需要显示广告主名称*/
                if(CTLogin.RoleIDs != "SYS001RL00005" && CTLogin.RoleIDs != "SYS001RL00001" && CTLogin.RoleIDs != "SYS001RL00004" && CTLogin.RoleIDs != "SYS001RL00008"){
                    $(".advertiser").remove();
                }else{
                    /*判断是代客户下单进入还是一级菜单立即投放进入*/
                    // console.log(GetUserId().userID);
                    if(userData.flag == 0){
                        // console.log("AE代客户下单");
                        setAjax(
                            {
                                type: "get",
                                url: "http://www.chitunion.com/api/UserInfo/GetUserInfoByUserID",
                                data:{
                                    userid: GetUserId().userID
                                }
                            },
                            function(data){
                                if(data.Status == 0){
                                    if(data.Result.TrueName != ""){
                                        $(".advertiser input").val(data.Result.TrueName);
                                    }else{
                                        $(".advertiser input").val(data.Result.UserName);
                                    }
                                    $(".advertiser input").attr("disabled","disabled");
                                }else{
                                    alert(data.Message);
                                }
                            }
                        );
                        
                    }else{
                        // console.log("一级菜单进入");
                        $(".advertiser input").val(curAdvName);
                        $(".advertiser input").on("keyup",function(){
                            var nameOrMobile = $(this).val();
                            if(nameOrMobile.length >0 && nameOrMobile != " "){
                                 /*if(CTLogin.RoleIDs == "SYS001RL00005"){
                                    GetADMaster(nameOrMobile,1);
                                }else{
                                    GetADMaster(nameOrMobile,0);
                                }*/
                                // V1.1.8下，AE和运营广告主名称权限相同
                                // GetADMaster(nameOrMobile,0);
                                GetADMaster(nameOrMobile,$('#adMatser option:checked').attr('value'));
                            }
                        });
                        $(".advertiser input").off("focus").on("focus",function(){
                            var nameOrMobile = $.trim($(this).val());
                            if(nameOrMobile == ""){
                                setAjax({
                                    type: "get",
                                    url: "http://www.chitunion.com/api/ADOrderInfo/GetADMaster?v=1_1",
                                    data: {
                                        NameOrMobile: nameOrMobile,
                                        // V1.1.8下，AE和运营广告主名称权限相同
                                        IsAEAuth:0
                                    }
                                },
                                function (data) {
                                    var availableAdv = [];
                                    $(".advertiser input").autocomplete({
                                        source: availableAdv
                                    })
                                })
                            }

                        })
                    }
                }

                //    1.获取模板中的页面结构
                var mediaOrderTemplate = $("#mediaOrderTemplate").html();
                var html = ejs.render(mediaOrderTemplate, {data: data})
                $('.CustomerID').val(data.Result.ADOrderInfo.CustomerID);
                $("#mediaTable>table").append(html);
                var AdjustPriceList = $("td[AdjustPrice]");
                for (var i = 0; i < AdjustPriceList.length; i++) {
                    var thisTd = AdjustPriceList[i];
                    if ($(thisTd).attr("OriginalPrice") != "-1") {
                        var thePrice = $(thisTd).attr("OriginalPrice") ;
                        var theDiscount = $(thisTd).attr("SaleDiscount");
                        var AdjustPrice =  thePrice * theDiscount;
                       //var price = AdjustPrice.toFixed(2);
                        var  price=formatMoney(AdjustPrice,2)
                        $(thisTd).attr("AdjustPrice", AdjustPrice);
                        if(price != "¥0.00"){
                            $(thisTd).text(price);
                        }else{
                            $(thisTd).text("--");
                            $(thisTd).prev().text("--");
                        }
                    }
                }
            }
            GetMeidaNum()
            GetOrderPrice()
            SelectAd()
        }, function () {
            // console.log("订单信息查询失败了");
        })

    }
    /*2.根据订单号修改订单，上传数据*/
})
