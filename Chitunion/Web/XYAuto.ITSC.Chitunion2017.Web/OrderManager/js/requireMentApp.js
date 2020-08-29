/**
 * Created by chengj on 2017/3/3.
 */
/*请求购物车的数据  */
$(function () {
    /*获取订单号和媒体类型*/
    var userData = GetUserId();

    var orderid = userData.orderID;
    var MediaType = userData.MediaType;

    /*若订单号存在，根据主订单号查询订单信息，若订单号不存在，根据媒体类型（即14002）从购物车中获取信息*/
    if (orderid) {
        getOrderList(orderid);
    } else {
        GetInfo_ShoppingCart(14002);
    }

    var userID;
    if(userData.userID != "gt86ZRCRjng%3d"){
        userID = userData.userID;
    }else if($('.CustomerID').val()){
        userID = $('.CustomerID').val();
    }else{
        userID = "gt86ZRCRjng%3d";
    }

    /*根据广告位选择个数判断“下一步”颜色*/
    schedulNum();

    /*删除某一项的点击事件*/
    $("#tableList").on("click", ".delBtn", function () {
        /*弹窗提示*/
        var thisTable = $(this);
        layer.confirm('是否删除此项？', {
            time: 0 //不自动关闭
            , btn: ['删除', '取消']
            , yes: function (index) {
                /*获取媒体ID和刊例广告位ID*/
                var MediaID = thisTable.parents("tr").attr("MediaID");
                var PublishDetailID = thisTable.parents("tr").attr("PublishDetailID");
                /*通过媒体类型和媒体ID，广告位ID删除当行商品*/
                Operate_ShoppingCart(MediaType, MediaID, PublishDetailID);
                /*将商品行移除*/
                thisTable.parents("tr").remove();
                /*获取媒体个数并显示*/
                GetMeidaNum();
                /*若媒体个数为0，移除“下一步”按钮*/
                var orderList = GetMeidaNum();
                if (orderList == 0) {
                    $("#nextBtn").remove();
                }
                /*验证是否存在CPM和CPD，若不存在，移除对应标题行*/
                checkCPMCPD();
                /*显示订单总价格*/
                GetOrderPrice();
                /*判断投放天数和总价格是否为0，若为0，“下一步”置灰*/
                schedulNum();
                layer.close(index);
                layer.msg('操作成功', {time: 400});
            }
        });

    })

    /*全选 全不选按钮*/
    $('#tableList').on('change', '#allbox', function () {
        if ($(this).prop('checked')) {
            $('.onebox').prop('checked', true);
        } else {
            $('.onebox').prop('checked', false);
        }
    });

    /*单选按钮*/
    $('#tableList').on('change', '.onebox', function () {
        if ($('.onebox').length == $("input[name='checkbox']:checked").length) {
            $('#allbox').prop('checked', true);
        } else {
            $('#allbox').prop('checked', false);
        }
    });

    /*设置排期*/
    $('#tableList').on('click', '.setData', function () {
        /*绑定this*/
        var that = $(this);
        /*若执行周期不为空，显示选择排期弹窗页面，否则，显示提示信息*/
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
                    /*获取开始日期和结束日期*/
                    var beginDay = $("#BeginData").val();
                    var endDay = $("#EndData").val();
                    /*调用设置排期方法，显示对应月份日期*/
                    setSchedule(beginDay,endDay);

                    /*二次点击设置排期时显示已设置的排期，并计算投放天数*/
                    displaySchedule(that);
                    var dataArr = getDataArr();
                    $(".ml20").text("投放天数:" + dataArr.length + "天");
                    /*排期显示后，日期的点击事件*/
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
                        /*获取投放天数的第一位数字，判断是否是0*/
                        var deliveryDays = $(".ml20").text().substr(5,1);
                        if(deliveryDays == 0){
                            layer.confirm('投放天数不能为0', {
                                time: 0 //不自动关闭
                                , btn: ["关闭"]
                            });
                        }else{
                            /*关闭弹层*/
                            $.closePopupLayer("popLayerDemo");
                            var dataArr = getDataArr();
                            /*返回选中的日期*/
                            /*处理返回的日期数组 若为单日，返回单日，若为日期区间，转换为开始-结束的形式*/
                            var pushdata = outPutDate(dataArr);
                            /*获取所有已选广告位的当前行 数组，给tr加上排期属性*/
                            var arr = that.parents("tr");
                            for (var i = 0; i < arr.length; i++) {
                                var MediaID = $(arr[i]).attr("MediaID")
                                var tempData = ADSchedule(MediaID, pushdata);
                                var ADScheduleInfos = JSON.stringify(tempData);
                                $(arr[i]).attr("ADScheduleInfos", ADScheduleInfos)
                            }
                            /*获取并设置总天数*/
                            var bannerData = dataArr.length;
                            for (var i = 0; i < arr.length; i++) {
                                var thisArr = arr[i];
                                $(thisArr).children().eq(9).attr("ADLaunchDays", bannerData);
                                $(thisArr).attr("ADLaunchDays", bannerData);
                                $(thisArr).children().eq(9).text(bannerData);
                                /*获取并显示每行金额*/
                                var thePrice = $(thisArr).children().eq(8).attr("Price");
                                var theAmount = $(thisArr).children().eq(10);
                                var price = (thePrice * bannerData).toFixed(2);
                                theAmount.attr("AdjustPrice", price);
                                var formatPrice = formatMoney(price);
                                theAmount.text(formatPrice);
                            }
                            GetOrderPrice();
                            schedulNum();
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
            });

        }
        schedulNum();
    });

    $('#tableList').on('click', '.setAllData',function(){
        if (timeInterval() == false) {
            layer.confirm('执行周期不能为空', {
                time: 0 //不自动关闭
                , btn: ["关闭"]
            });
        } else{
            var selectNum = $("input[name='checkbox']:checked").length;
            if(selectNum == 0){
                layer.confirm('请选择广告位', {
                    time: 0 //不自动关闭
                    , btn: ["关闭"]
                });
            }else{
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
                        setSchedule(beginDay,endDay);
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
                            var deliveryDays = $(".ml20").text().substr(5,1);
                            if(deliveryDays == 0){
                                layer.confirm('投放天数不能为0', {
                                    time: 0 //不自动关闭
                                    , btn: ["关闭"]
                                });
                            }else{
                                /*关闭弹层*/
                                $.closePopupLayer("popLayerDemo");
                                var dataArr = getDataArr();
                                /*返回选中的日期*/
                                /*处理返回的日期数组 若为单日，返回单日，若为日期区间，转换为开始-结束的形式*/
                                var pushdata = outPutDate(dataArr);
                                /*所有已选广告位的当前行 数组*/
                                var arr = $("input[name='checkbox']:checked").parents("tr");
                                for (var i = 0; i < arr.length; i++) {
                                    var MediaID = $(arr[i]).attr("MediaID")
                                    var tempData = ADSchedule(MediaID, pushdata);
                                    var ADScheduleInfos = JSON.stringify(tempData);
                                    $(arr[i]).attr("ADScheduleInfos", ADScheduleInfos)
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
                                    var theAmount = $(thisArr).children().eq(10)
                                    var price = (thePrice * bannerData).toFixed(2)
                                    theAmount.attr("AdjustPrice", price);
                                    var formatPrice = formatMoney(price);
                                    theAmount.text(formatPrice);
                                }
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
        schedulNum();
    });

    $("#BeginData").click(function () {
        laydate({
            elem: '#BeginData',
            isclear: true,
            format: 'YYYY-MM-DD',
            min:laydate.now(+1),
            choose: function (date) {
                checkTime();
                if ($.trim($("#EndData").val()) && date > $("#EndData").val() ) {
                    layer.alert('开始日期不能大于结束日期');
                    $("#BeginData").val("");
                }else{
                    if(!compare(date,$('#EndData').val())){
                        layer.alert('周期不能大于半年！');
                        $("#BeginData").val("");
                    }
                }
                clearSchedule();
                GetOrderPrice();
            }
        });
    });
    $("#EndData").click(function () {
        laydate({
                elem: '#EndData',
                isclear: true,
                format: 'YYYY-MM-DD',
                min:laydate.now(+1),
                choose: function (date) {
                    timeInterval();
                    checkTime();
                    if ( $.trim($("#BeginData").val()) && date< $("#BeginData").val()) {
                        layer.alert('结束时间不能小于开始时间');
                        $("#EndData").val("");
                    }else{
                        if(!compare($('#BeginData').val(),date)){
                            layer.alert('周期不能大于半年！');
                            $("#EndData").val("");
                        }
                    }
                    clearSchedule();
                    GetOrderPrice();
                }
            }
        )

    })
    /*点击继续添加广告位*/
    $(".getShopCar").click(function () {
        if (!orderid) {
            if(CTLogin.RoleIDs == "SYS001RL00005"){
                if($(".advertiser input").val() == ""){
                    var curUserID = getUserID();
                    var data = getOrderInfo(MediaType, curUserID);
                    upShoppingCart(data,MediaType);
                }else{
                    if(checkAdvName(1) == true){
                        var curUserID = getUserID();
                        var data = getOrderInfo(MediaType, curUserID);
                        upShoppingCart(data,MediaType);
                    }
                }
            }else if(CTLogin.RoleIDs == "SYS001RL00001" || CTLogin.RoleIDs == "SYS001RL00004"){
                if($(".advertiser input").val() == ""){
                    var curUserID = getUserID();
                    var data = getOrderInfo(MediaType, curUserID);
                    upShoppingCart(data,MediaType);
                }else{
                    if(checkAdvName(0) == true){
                        var curUserID = getUserID();
                        var data = getOrderInfo(MediaType, curUserID);
                        upShoppingCart(data,MediaType);
                    }
                }
            }else{
                var data = getOrderInfo(MediaType,userID);
                upShoppingCart(data,MediaType);
            }

        } else {
            if(CTLogin.RoleIDs == "SYS001RL00005"){
                if($(".advertiser input").val() == ""){
                    var curUserID = getUserID();
                    var data = getupShoppingCart(2,MediaType, orderid,curUserID);
                    upShoppingCart(data,MediaType,orderid);
                }else{
                    if(checkAdvName(1) == true){
                        var curUserID = getUserID();
                        var data = getupShoppingCart(2,MediaType, orderid,curUserID);
                        upShoppingCart(data,MediaType,orderid);
                    }
                }
            }else if(CTLogin.RoleIDs == "SYS001RL00001" || CTLogin.RoleIDs == "SYS001RL00004"){
                if($(".advertiser input").val() == ""){
                    var curUserID = getUserID();
                    var data = getupShoppingCart(2,MediaType, orderid,curUserID);
                    upShoppingCart(data,MediaType,orderid);
                }else{
                    if(checkAdvName(0) == true){
                        var curUserID = getUserID();
                        var data = getupShoppingCart(2,MediaType, orderid,curUserID);
                        upShoppingCart(data,MediaType,orderid);
                    }
                }
            }else{
                var data = getupShoppingCart(2,MediaType, orderid,userID);
                upShoppingCart(data,MediaType,orderid);
            }

        }
    })
    
    /*点击下一步提交订单信息*/
    $(".orderBtn").click(function () {

        /*验证是否选择排期 验证投放天数是否为空*/
        if(CPMDateIsNull()==true){
            if(timeInterval()==true){
                if (!orderid) {
                    if(CTLogin.RoleIDs == "SYS001RL00005"){
                        if(AdvNameIsNull() != false && checkAdvName(1) == true){
                            var curUserID = getUserID();
                            var data = getOrderInfo(14002,curUserID);
                            AddOrUpdate_ADOrderInfo(MediaType, data);
                        }
                    }else if(CTLogin.RoleIDs == "SYS001RL00001" || CTLogin.RoleIDs == "SYS001RL00004"){
                        if(AdvNameIsNull() != false && checkAdvName(0) == true){
                            var curUserID = getUserID();
                            var data = getOrderInfo(14002,curUserID);
                            AddOrUpdate_ADOrderInfo(MediaType, data);
                        }
                    }else{
                        var data = getOrderInfo(14002,userID);
                        AddOrUpdate_ADOrderInfo(MediaType, data);
                    }
                } else {
                    if(CTLogin.RoleIDs == "SYS001RL00005"){
                        if(AdvNameIsNull() != false && checkAdvName(1) == true){
                            var curUserID = getUserID();
                            var data = getADDetailsInfo(orderid,curUserID);
                            upOrder(data);
                        }
                    }else if(CTLogin.RoleIDs == "SYS001RL00001" || CTLogin.RoleIDs == "SYS001RL00004"){
                        if(AdvNameIsNull() != false && checkAdvName(0) == true){
                            var curUserID = getUserID();
                            var data = getADDetailsInfo(orderid,curUserID);
                            upOrder(data);
                        }
                    }else{
                        var data = getADDetailsInfo(orderid,userID);
                        upOrder(data);
                    }

                }
            }else{
                layer.confirm('请选择执行周期', {
                    time: 0 //不自动关闭
                    , btn: ["关闭"]
                });
            }
        }else{
            layer.confirm('投放天数/千次不能为空', {
                time: 0 //不自动关闭
                , btn: ["关闭"]
            });
        }

    })

    /*根据url获取订单号和媒体类型，用户ID等信息（编码）*/
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
    /*根据URL获取用户ID（不进行编码）*/
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

    /*当前用户是AE时，通过标签属性获取当前广告主ID*/
    function getUserID(){
        var userID = $(".advertiser").attr("userID");
        if(userID == undefined){
            return "gt86ZRCRjng%3d"
        }else{
            return userID
        }
    }
    /*通过当前的URL的userID，设置flag标识，以此来判断系统角色（是否为AE），并返回在url后传递的参数：用户ID和flag标识*/
    function userID1(userID){
        /*若当前用户ID是默认值，有两种可能跳转到当前页面
         * 1、第一次点击一级菜单（微信/APP）跳转而来
         * 2、在下单第一步没有填写广告主名称，二次/多次点击一级菜单跳转而来
         * 这两种情况下，都是点击一级菜单跳转而来，此时flag = 1
         * 若当前用户ID不是默认值，有两种可能
         * 1、AE点击代客户下单跳转而来，携带userID
         * 2、点击一级菜单跳转过来后，填写了有效的广告主名称，使得用户ID不是默认值
         * 这两种情况下flag是不同的，所以需要判断当前URL上是否有flag值，
         若有flag值，当前页面跳转时携带的flag就是url上的flag值，
         若没有，则flag = 0(因为当填写了有效的广告主名称之后，再增加广告位，重新进入下单页面时，flag肯定是存在且是1)
        */
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
    
    /*验证广告主名称非空（仅AE点击下一步时需验证）*/
    function AdvNameIsNull(){
        var advName = $(".advertiser input").val();
        if(advName == ""){
            $(".advertiser .nameMsg").text("广告主不能为空，请输入!");
            return false;
        }
    }

    /*验证执行时间非空*/
    function checkTime() {
        var requireTime = $("input[name='beginTime']").val();
        if (requireTime === "") {
            $(".timeMsg").text("请选择执行时间");
            return false;
        } else {
            $(".timeMsg").text(" ")
            return true;
        }
    }

    /*验证广告主填写是否正确，若正确，将对应的userID绑定到标签上，若不正确，显示错误提示信息*/
    function checkAdvName(IsAEAuth){
        var flag = false;
        if($(".advertiser input").val() != ""){
            var advName = $(".advertiser input").val();
            /*获取当前广告主名称之后，进行模糊查询，当前广告主是否存在*/
            $.ajax({
                type: "get",
                url: "/api/ADOrderInfo/GetADMaster",
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
                        $(".advertiser .nameMsg").text("广告主不存在，请重新输入！");
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
                            $(".advertiser .nameMsg").text("广告主不存在，请重新输入！");
                            flag = false;
                            $("#nextBtn").css("background-color", "#666");
                        }else{
                            /*匹配时再次调用接口，把当前广告主名称对应的userID绑定到页面上*/
                            $.ajax({
                                type: "get",
                                url: "/api/ADOrderInfo/GetADMaster",
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
                            $(".advertiser .nameMsg").text("");
                            flag = true;
                        }
                    }

                    // }
                }
            });
        }
        return flag;
    }

    /*验证是否存在CPMCPD，若不存在，移除对应标题行*/
    function checkCPMCPD() {
        var cpmlist = $(".CPM");
        var cpdlist = $(".CPD");
        if (cpmlist.length == 0) {
            $("#CPM").remove();
        }
        if (cpdlist.length == 0) {
            $("#CPD").remove();
        }
    }

    /*验证是否选择执行周期*/
    function timeInterval() {
        var BeginDataArr = $("#BeginData").val().split("-");
        var BeginData = "";
        for (var i = 0; i < BeginDataArr.length; i++) {
            BeginData = BeginData + BeginDataArr[i]
        }
        var EndDataArr = $("#EndData").val().split("-");
        var EndData = "";
        for (var i = 0; i < EndDataArr.length; i++) {
            EndData = EndData + EndDataArr[i]
        }
        if (BeginDataArr == 0) {
            return false;
        }else if (EndData - BeginData >= 0) {
            return true;
        }else{
            return false;
        }
    }

    /* 判断时间间隔是否大于半年*/
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

    /*验证是否选择排期*/
    /*投放天数非0验证*/
    function schedulNum() {
        /*获取选择了广告位的媒体，若投放天数不为0,“下一步”，正常。否则，置灰*/
        var ADLaunchDays = $("tr[ADLaunchDays]");
        var noChoseNum=0;
        for(var i=0;i<ADLaunchDays.length;i++){
            var thisTr=ADLaunchDays[i];
            if($(thisTr).attr("ADLaunchDays")=="0"){
                noChoseNum++;
            }
        }
        if(noChoseNum == 0 || noChoseNum == ""){
            $("#nextBtn").css("background-color", "#FF9100")
            return true;
        }
        else{
            $("#nextBtn").css("background-color", "#666");
        }
    }
    /*验证投放天数是否为空*/
    function CPMDateIsNull(){
        var revisedPriceList = $(".tableList input[name='Username']");
        var ADLaunchDays = $(".CPD td[ADLaunchDays]");
        var noDateNum = 0;
        for (var i = 0; i < revisedPriceList.length; i++) {
            var Num = $(revisedPriceList[i]).val();
            if(Num == "" || Num == 0 ){
                noDateNum++;
            }
        }
        for(var i = 0; i < ADLaunchDays.length; i++){
            var number = $(ADLaunchDays[i]).text();
            if(number == "" || number == 0 ){
                noDateNum++;
            }
        }
        if(noDateNum == 0){
            $("#nextBtn").css("background-color", "#FF9100");
            return true;
        }else{
            $("#nextBtn").css("background-color", "#666");
        }
    }

    /*清空CPD投放天数和金额以及之前设置好的排期*/
    function clearSchedule() {
        var trList = $(".tableList");
        for (var i = 0; i < trList.length; i++) {
            var thisTr = trList[i];
            if ($(thisTr).attr("ADLaunchIDs") == "11001") {
                $(thisTr).attr("ADScheduleInfos", "");
                $(thisTr).children().eq(9).text("");
                $(thisTr).children().eq(9).attr("ADLaunchDays", "");
                $(thisTr).children().eq(10).text("");
                $(thisTr).children().eq(10).attr("AdjustPrice", "");
            } else {
                $(thisTr).attr("ADScheduleInfos", "")
                $(thisTr).children().eq(9).children().val("");
                $(thisTr).children().eq(9).children().attr("value", "");
                $(thisTr).children().eq(9).attr("ADLaunchDays", "");
                $(thisTr).children().eq(10).attr("AdjustPrice", "");
                $(thisTr).children().eq(10).text("");
            }
        }
    }

    /*1.从购物车获取数据*/
    function GetInfo_ShoppingCart(MediaType) {
        setAjax(
            {
                type: "get",
                url: "/api/ShoppingCart/GetInfo_ShoppingCart",
                data: {
                    MediaType: MediaType,
                }
            },
            function (data) {
                if (data.Result === null) {
                    // console.log("购物车没有数据");
                    var orderList = GetMeidaNum()
                    if (orderList == 0) {
                        $("#nextBtn").remove()
                    }
                }
                else {
                    if (data.Result.MediaType === 14002) {
                        /*如果不是AE，不显示广告主*/
                        if(CTLogin.RoleIDs != "SYS001RL00005" && CTLogin.RoleIDs != "SYS001RL00001" &&CTLogin.RoleIDs != "SYS001RL00004"){
                            $(".advertiser").remove();
                        }else{
                            /*判断是代客户下单进入还是一级菜单立即投放进入*/
                            if(userData.flag == 0){
                                // console.log("AE代客户下单");
                                setAjax(
                                    {
                                        type: "get",
                                        url: "/api/UserInfo/GetUserInfoByUserID",
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
                                        if(CTLogin.RoleIDs == "SYS001RL00005"){
                                            GetADMaster(nameOrMobile,1);
                                        }else{
                                            GetADMaster(nameOrMobile,0);
                                        }
                                        
                                    }

                                });
                                $(".advertiser input").off("focus").on("focus",function(){
                                    var nameOrMobile = $.trim($(this).val());
                                    if(nameOrMobile == ""){
                                        if(CTLogin.RoleIDs == "SYS001RL00005"){
                                            setAjax(
                                                {
                                                    type: "get",
                                                    url: "/api/ADOrderInfo/GetADMaster",
                                                    data: {
                                                        NameOrMobile: nameOrMobile,
                                                        IsAEAuth:1
                                                    }
                                                },
                                                function (data) {
                                                    var availableAdv = [];
                                                    $(".advertiser input").autocomplete({
                                                        source: availableAdv
                                                    })
                                                },
                                                function(){}
                                            )
                                        }else{
                                            setAjax(
                                                {
                                                    type: "get",
                                                    url: "/api/ADOrderInfo/GetADMaster",
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
                                            )
                                        }
                                    }

                                })
                            }
                        }
                         //1.获取模板中的页面结构
                        var appTemplate = $("#appTemplate").html();
                        var html = ejs.render(appTemplate, {data: data});
                        $("#appTable>table").append(html);
                        var orderList = GetMeidaNum();
                        if (orderList == 0) {
                            $("#tableNum").text("--");
                            $("#nextBtn").remove();
                        }
                        checkCPMCPD();
                        GetOrderPrice();
                        schedulNum();
                    }
                    else {
                        // console.log("请求的数据是自媒体");
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
                url: "/api/ADOrderInfo/GetADMaster",
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
    /*2.获取媒体总个数，将媒体个数显示在页面上*/
    function GetMeidaNum() {
        var trList = $("tr[IsSelected='true']").length;
        $("#tableNum").text(trList);
        return trList;
    }
    /*3.获取总价格*/
    function GetOrderPrice() {
        var AmountList = $("td[AdjustPrice]");
        var OrderPrice = 0;
        /*循环获取各媒体的价格*/
        for (var i = 0; i < AmountList.length; i++) {
            var price = $(AmountList[i]).attr("AdjustPrice");
            OrderPrice = (price - 0) + OrderPrice;
        }
        /*将格式化的价格显示在页面上*/
        var formatPrice = formatMoney(OrderPrice,2,"");
        $("#OrderPrice").text(formatPrice);
        /*当修改价格时，总价对应改变*/
        var revisedPriceList = $("#tableList input[name='Username']");
        for (var i = 0; i < revisedPriceList.length; i++) {
            $(revisedPriceList[i]).on('blur',function () {
                var thisCount = $(this).val();
                /*若投放天数为空，则金额为空，并设置tr的属性值*/
                if (thisCount == "") {
                    $(this).parents('.tableList').children().eq(10).text("");
                    $(this).parents('.tableList').children().eq(10).attr("AdjustPrice", "");
                    $(this).parents('.tableList').children().eq(9).attr("ADLaunchDays", "");
                    $(this).parents('.tableList').attr("ADLaunchDays", '');
                } else {
                    /*获取单价，计算并显示总价，并给对应标签绑上属性值*/
                    var onePrice = $(this).parents("tr").children().eq(8).attr("Price") - 0;
                    var price = (thisCount * onePrice).toFixed(2);
                    $(this).parents('.tableList').children().eq(9).attr("ADLaunchDays", thisCount);
                    $(this).parents('.tableList').attr("ADLaunchDays", thisCount);
                    $(this).parents('.tableList').children().eq(10).attr("AdjustPrice", price);
                    $(this).parents('.tableList').children().eq(10).text(formatMoney(price));
                }
                /*修改价格后再次循环获取价格，显示价格*/
                var AmountList = $("td[AdjustPrice]");
                var OrderPrice = 0;
                for (var i = 0; i < AmountList.length; i++) {
                    var price = $(AmountList[i]).attr("AdjustPrice")
                    OrderPrice = (price - 0) + OrderPrice;
                }
                OrderPrice = formatMoney(OrderPrice,2,"");
                $("#OrderPrice").text(OrderPrice);
                schedulNum();
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
                /*返回状态信息，若Status为0表示删除成功*/
            }
            , function () {
                alert("请求失败");
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
    /*6.点下一步 提交订单数据  媒体ID，发布形式描述， 价格 。中途没有跳转至购物车，没有购物车生成的订单号
     * 封装获取订单信息函数*/
    function getOrderInfo(MediaType,userID) {
        var getOrderInfo = getupShoppingCart(1, MediaType,"",userID)
        return getOrderInfo;
    }
    /*获取页面的元素信息 */
    function getupShoppingCart(optType, MediaType, orderid,userID) {
        var getTr = $("tr[IsSelected='true']")
        var ADDetails = []
        for (var j = 0; j < getTr.length; j++) {
            var ADLaunchDays = $(getTr[j]).attr("ADLaunchDays");
            var MediaID = $(getTr[j]).attr("MediaID");
            var PublishDetailID = $(getTr[j]).attr("PublishDetailID");
            if (!orderid) {
                var OriginalPrice = $(getTr[j]).children().eq(8).attr("Price");
            } else {
                var OriginalPrice = $(getTr[j]).children().eq(8).attr("OriginalPrice");
            }
            var tempObjADSchedule = $(getTr[j]).attr("ADScheduleInfos")
            if (tempObjADSchedule === "") {

                var ObjStory = {
                    "MediaType": MediaType,
                    "MediaID": MediaID,
                    "PubDetailID": PublishDetailID,
                    "OriginalPrice": OriginalPrice,
                    "AdjustDiscount": 0,
                    "ADLaunchDays": ADLaunchDays || 0,
                    "ADScheduleInfos": []
                }
            } else {
                var ADScheduleInfos = JSON.parse(tempObjADSchedule);
                var ObjStory = {
                    "MediaType": MediaType,
                    "MediaID": MediaID,
                    "PubDetailID": PublishDetailID,
                    "OriginalPrice": OriginalPrice,
                    "AdjustDiscount": 0,
                    "ADLaunchDays": ADLaunchDays,
                    "ADScheduleInfos": ADScheduleInfos
                }
            }
            ADDetails[j] = ObjStory
        }
        var BeginTime,EndTime;
        if($("#BeginData").val() == " " || $("#BeginData").val() == ""){
            BeginTime = "1990-01-01";
        }else{
            BeginTime = $("#BeginData").val();
        }
        if($("#EndData").val() == " " || $("#EndData").val() == ""){
            EndTime = "1990-01-01";
        }else{
            EndTime = $("#EndData").val();
        }
        var getupShoppingCart = {
            "optType": optType,
            "ADOrderInfo": {
                "OrderID": orderid,
                "MediaType": MediaType,
                "OrderName": "",
                "Status": 16001,
                "BeginTime": BeginTime,
                "EndTime": EndTime,
                "Note": "",
                "UploadFileURL": "",
                "CustomerID": userID
            },
            "ADDetails": ADDetails
        }
        return getupShoppingCart;
    }

    function upShoppingCart(data, MediaType,orderID) {
        if(data != undefined && data.ADDetails.length != 0) {
            setAjax({
                type: "post",
                url: "/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
                data: data
            }, function (data) {
                if(data.Status != 0){
                    alert(data.Message);
                }else{
                    var msg = data.Message;
                    var curUserID = $(".advertiser").attr("userID");
                    if(!orderid){
                        if(curUserID != "" && curUserID != undefined){
                            window.location= "/OrderManager/app_list_check.html?MediaType=" + MediaType + "&orderID=" + msg + "&OrderState=1" + "&userID=" + curUserID +"&flag="+GetUserId().flag;
                        }else{
                            window.location= "/OrderManager/app_list_check.html?MediaType=" + MediaType + "&orderID=" + msg + "&OrderState=1" + userID1(userID);
                        }
                    }else{
                        if(curUserID != "" && curUserID != undefined){
                            window.location= "/OrderManager/app_list_check.html?MediaType=" + MediaType + "&orderID=" + orderID + "&OrderState=1" + "&userID=" + curUserID +"&flag="+GetUserId().flag;
                        }else{
                            window.location= "/OrderManager/app_list_check.html?MediaType=" + MediaType + "&orderID=" + orderID + "&OrderState=1" + userID1(userID);
                        }
                    }

                }
            }, function () {

            })
        }else {
            // 修改订单
            setAjax({
                url:'/api/ADOrderInfo/AddOrUpdate_ADOrderInfo',
                type:'post',
                data:{
                    "optType" : 2,
                    "ADOrderInfo" : {
                        "OrderID" : orderID,
                        "MediaType" : MediaType,
                        "Status" : 16001
                    },
                    "ADDetails" : []
                }
            },function (data) {
                window.location='/OrderManager/app_list.html';
            })
        }
    }


    /*7.调用API 发起AJAX请求 提交订单数据*/
    function AddOrUpdate_ADOrderInfo(MediaType, data) {
        setAjax(
            {
                type: "post",
                url: "/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
                data: data
            },
            function (data) {
                var curUserID = $(".advertiser").attr("userID");
                if(!orderid){
                    var msg = data.Message;
                    if(curUserID != "" && curUserID != undefined){
                        window.location= "/OrderManager/requirementAPP02.html?MediaType=" + MediaType + "&orderID=" + msg + "&OrderState=1" + "&userID=" + curUserID +"&flag="+GetUserId().flag;
                    }else{
                        window.location= "/OrderManager/requirementAPP02.html?MediaType=" + MediaType + "&orderID=" + msg + "&OrderState=1" + userID1(userID);
                    }
                }else{
                    if(curUserID != "" && curUserID != undefined){
                        window.location= "/OrderManager/requirementAPP02.html?MediaType=" + MediaType + "&orderID=" + orderid + "&OrderState=1" + "&userID=" + curUserID +"&flag="+GetUserId().flag;
                    }else{
                        window.location= "/OrderManager/requirementAPP02.html?MediaType=" + MediaType + "&orderID=" + orderid + "&OrderState=1" + userID1(userID);
                    }
                }
            },
            function () {
                alert("数据提交失败了");
            }
        )
    }


    /*1.根据主订单号获取数据（查看主订单接口GetByOrderID_ADOrderInfo）
    *（1）显示订单数据
    *（2）若为AE代客户下单，显示广告主名称
        若为一级菜单直接下单，需要用户输入广告主名称，支持模糊匹配，
        并在二次进入时，显示之前输入的广告主名称（可编辑）
    */
    function getOrderList(orderid) {
        setAjax({
                type: "get",
                url: "/api/ADOrderInfo/GetByOrderID_ADOrderInfo",
                data: {
                    orderid: orderid,
                }
            }, function (data) {
                // console.log("订单信息查询成功了");
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
                    /*如果不是AE，不显示广告主，如果是AE，显示广告主名称，并在需要时进行模糊查询*/
                    if(CTLogin.RoleIDs != "SYS001RL00005" && CTLogin.RoleIDs != "SYS001RL00001" &&CTLogin.RoleIDs != "SYS001RL00004"){
                        $(".advertiser").remove();
                    }else{
                        /*判断是代客户下单进入还是一级菜单立即投放进入*/
                        if(userData.flag == 0){
                            // console.log("AE代客户下单");
                            /*通过url中的userID,发送请求，返回用户的真实名称/手机号，显示在页面上*/
                            setAjax(
                                {
                                    type: "get",
                                    url: "/api/UserInfo/GetUserInfoByUserID",
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
                            /*当广告主名称文本框输入时，进行模糊查询，联想输入*/
                            $(".advertiser input").on("keyup",function(){
                                var nameOrMobile = $(this).val();
                                if(nameOrMobile.length >0 && nameOrMobile != " "){
                                    if(CTLogin.RoleIDs == "SYS001RL00005"){
                                        GetADMaster(nameOrMobile,1);
                                    }else{
                                        GetADMaster(nameOrMobile,0);
                                    }
                                    
                                }

                            })
                            $(".advertiser input").off("focus").on("focus",function(){
                                var nameOrMobile = $.trim($(this).val());
                                if(nameOrMobile == ""){
                                    if(CTLogin.RoleIDs == "SYS001RL00005"){
                                        setAjax(
                                            {
                                                type: "get",
                                                url: "/api/ADOrderInfo/GetADMaster",
                                                data: {
                                                    NameOrMobile: nameOrMobile,
                                                    IsAEAuth:1
                                                }
                                            },
                                            function (data) {
                                                var availableAdv = [];
                                                $(".advertiser input").autocomplete({
                                                    source: availableAdv
                                                })
                                            },
                                            function(){}
                                        )
                                    }else{
                                        setAjax(
                                            {
                                                type: "get",
                                                url: "/api/ADOrderInfo/GetADMaster",
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
                                        )
                                    }
                                }

                            })

                        }
                    }
                    /*数据渲染*/
                    if(data.Result.ADOrderInfo.BeginTime.substr(0, 10) == "1900-01-01" || data.Result.ADOrderInfo.BeginTime.substr(0, 10) == "1990-01-01" ||
                        data.Result.ADOrderInfo.EndTime.substr(0, 10) == "1900-01-01" || data.Result.ADOrderInfo.EndTime.substr(0, 10) == "1990-01-01"){
                        $("#BeginData").val(" ");
                        $("#EndData").val(" ");
                    }else{
                        $("#BeginData").val(data.Result.ADOrderInfo.BeginTime.substr(0, 10));
                        $("#EndData").val(data.Result.ADOrderInfo.EndTime.substr(0, 10));
                    }
                    $(".textArea").val(data.Result.ADOrderInfo.Note);
                    var imgSrc = data.Result.ADOrderInfo.UploadFileURL;
                    $("#imgName").text(data.Result.ADOrderInfo.UploadFileName);
                    $("#imgUploadFile").attr("UploadFileURL", imgSrc);
                    $("#imgDownURL").attr("href", imgSrc);
                    // console.log("请求的类型是APP");
                    //1.获取模板中的页面结构
                    var appTemplate = $("#appOrderTemplate").html();
                    var html = ejs.render(appTemplate, {data: data})
                    $("#appTable>table").append(html);
                    $('.CustomerID').val(data.Result.ADOrderInfo.CustomerID);
                    /*将从后台获取的排期信息存储在数组中*/
                    var ADScheduleInfosArr = [];
                    for (var j = 0; j < data.Result.SubADInfos.length; j++) {
                        for (var i = 0; i < data.Result.SubADInfos[j].APPDetails.length; i++) {
                            var ADScheduleInfos = data.Result.SubADInfos[j].APPDetails[i].ADScheduleInfos;
                            ADScheduleInfosArr.push(ADScheduleInfos)
                        }
                    }
                    // console.log(ADScheduleInfosArr);
                    /*将排期信息绑定到对应tr的属性上*/
                    var tableList = $(".tableList");
                    for (var i = 0; i < ADScheduleInfosArr.length; i++) {
                        var thisADSInfo = ADScheduleInfosArr[i];
                        for (var k = 0; k < thisADSInfo.length; k++) {
                            thisMediaID = thisADSInfo[k].MediaID;
                            var ADScheduleInfos = JSON.stringify(thisADSInfo);
                            for (var j = 0; j < tableList.length; j++) {
                                var thisTr = tableList[j];
                                var thisListID = $(thisTr).attr("MediaID")
                                var thisListADLaunchID = $(thisTr).attr("ADLaunchIDs")
                                if (thisMediaID == thisListID && thisListADLaunchID == 11001) {
                                    $(thisTr).attr("ADScheduleInfos", ADScheduleInfos)
                                }
                            }
                        }
                    }
                    var OriginalPriceList = $("td[OriginalPrice]");
                    for (var i = 0; i < OriginalPriceList.length; i++) {
                        var thisTd = OriginalPriceList[i];
                        var thePrice = $(thisTd).attr("OriginalPrice") - 0;
                        var theDiscount = $(thisTd).attr("SaleDiscount") - 0;
                        var OPrice = thePrice * theDiscount;
                        var price = OPrice.toFixed(2);
                        $(thisTd).attr("price", price);
                        if ($(thisTd).parent().attr("ADLaunchIDs") == "11002") {
                            $(thisTd).text(formatMoney(price)+"元/CPM");
                            /*设置CPM投放天数和金额为0时不显示*/
                            var curDateDom = $(thisTd).next().find("input");
                            var curDate = curDateDom.val();
                            // console.log(curDate);
                            if(curDate == 0){
                                $(thisTd).next().find("input").val(" ");
                            }
                        } else {
                            $(thisTd).text(formatMoney(price)+"元/天/轮播");
                            /*设置CPD投放天数和金额，若为0，则不显示*/
                            var curDate = $(thisTd).next().text();
                            if(curDate == 0){
                                $(thisTd).next().text(" ");
                            }
                        }
                        /*设置从后台获取数据后金额的显示格式*/
                        var curAmount = $(thisTd).next().next().text();
                        if(curAmount == 0){
                            $(thisTd).next().next().text(" ");
                        }else{
                            $(thisTd).next().next().text(formatMoney(curAmount));
                        }
                    }
                }
                GetMeidaNum();
                GetOrderPrice();
                checkCPMCPD();
                schedulNum();

            }, function () {
                // console.log("订单信息查询失败了");
            }
        )
    }
    /*6.上传信息===中途有跳转至购物车，有订单号 此次提交为修改订单*/
    function getADDetailsInfo(orderid,userID) {
        var newResult = getupShoppingCart(2, 14002, orderid,userID);
        // console.log("有订单号提交的数据" + newResult);
        return newResult;
    }
    function upOrder(data,MediaType) {
        setAjax({
            type: "post",
            url: "/api/ADOrderInfo/AddOrUpdate_ADOrderInfo",
            data: data
        }, function (data) {
            if(data.Status == 0){
                var orderData = GetRequest();
                /*获取订单号*/
                var orderid = orderData.orderID;
                var MediaType = orderData.MediaType;
                if(GetUserId().userID){
                    var userID = GetUserId().userID;
                }else{
                    var userID = '';
                }
                var cpdlist = $(".CPD");

                var curUserID = $(".advertiser").attr("userID");
                if (cpdlist.length == 0) {
                    if(!orderid){
                        var msg = data.Message;
                        if(curUserID != "" && curUserID != undefined){
                            window.location = "/OrderManager/requirementAPP02.html?MediaType=" + MediaType + "&orderID=" + msg+ "&userID=" + curUserID;
                        }else{
                            window.location= "/OrderManager/requirementAPP02.html?MediaType=" + MediaType + "&orderID=" + msg + userID1(userID);
                        }
                    }else{
                        if(curUserID != "" && curUserID != undefined){
                            window.location = "/OrderManager/requirementAPP02.html?MediaType=" + MediaType + "&orderID=" + orderid+ "&userID=" + curUserID;
                        }else{
                            window.location= "/OrderManager/requirementAPP02.html?MediaType=" + MediaType + "&orderID=" + orderid + userID1(userID);
                        }
                    }
                    
                } else {
                    if (schedulNum() == true) {
                        if(!orderid){
                            var msg = data.Message;
                            if(curUserID != "" && curUserID != undefined){
                                window.location = "/OrderManager/requirementAPP02.html?MediaType=" + MediaType + "&orderID=" + msg+ "&userID=" + curUserID;
                            }else{
                                window.location= "/OrderManager/requirementAPP02.html?MediaType=" + MediaType + "&orderID=" + msg + userID1(userID);
                            }
                        }else{
                            if(curUserID != "" && curUserID != undefined){
                                window.location = "/OrderManager/requirementAPP02.html?MediaType=" + MediaType + "&orderID=" + orderid+ "&userID=" + curUserID;
                            }else{
                                window.location= "/OrderManager/requirementAPP02.html?MediaType=" + MediaType + "&orderID=" + orderid + userID1(userID);
                            }
                        }
                    } else {
                        layer.confirm('投放天数不能为0', {
                            time: 0 //不自动关闭
                            , btn: ["关闭"]
                        });
                    }
                }
            }else{
                // console.log(data.Message);
            }

        }, function () {
            // console.log("提交订单信息第二步失败了");
        })
    }


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
    
    /*封装输出连续日期的JS函数*/
    //var datestr = ['2017-03-10', '2017-03-17', '2017-03-18'];
    //var datestr = ['2017-03-10', '2017-03-11', '2017-03-18'];
    //var datestr = ['2017-03-10'];
    // var datestr = ['2017-03-30', '2017-03-31', '2017-04-01', '2017-03-19', '2017-03-21', '2017-03-22', '2017-03-24'];
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