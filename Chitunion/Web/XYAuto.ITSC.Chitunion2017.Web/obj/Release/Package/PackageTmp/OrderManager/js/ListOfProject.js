/**
* Created by luq on 2017/2/27.
*/
$(function () {
    var ProjectList = new ComponentSection({});
    var state = window.location.search.substring(1).split("=")[1];

    ProjectList.addSelfState({
        "SelectItems": ["草稿", "待审核", "待执行", "执行中", "执行完毕", "已完成", "已取消", "已驳回"],
        "SearchOptions": {
            "OrderNum": "",
            "DemandDescribe": "",
            "StartTime": "",
            "EndTime": "",
            "MediaType": "0",
            "Creater": "",
            "CustomerID": "",
            "OrderSource":'0',
            'IsCRM':'0',
            'SubOrderNum':''
        },
        "CTLogin": CTLogin,
        "OrderState": "16001"
    });
    /****************交互事件，逻辑内容*****************/

    $('.but_query').on('click',function () {
        window.open('/OrderManager/shopcartForWisdomDelivery01.html');
    })

    /*从审核页面过来根据审核的状态自动选择列表页面对应的状态以及数据*/
    ProjectList.shenHeToCur = function(){
        switch(state){
            case "16002" :
                if (ProjectList.State.OrderState != "16002") {
                    ProjectList.setState({
                        "OrderState": state
                    }, function () {
                        ProjectList.getData(1);
                    });
                }
                $(".selected").removeClass("selected");
                $("#SelectItems").find("li").eq(1).addClass("selected");
            break;
            case "16003" :
                if (ProjectList.State.OrderState != "16003") {
                    ProjectList.setState({
                        "OrderState": state
                    }, function () {
                        ProjectList.getData(1);
                    });
                }
                $(".selected").removeClass("selected");
                $("#SelectItems").find("li").eq(2).addClass("selected");
            break;
            case "16004" :
                if (ProjectList.State.OrderState != "16004") {
                    ProjectList.setState({
                        "OrderState": state
                    }, function () {
                        ProjectList.getData(1);
                    });
                }
                $(".selected").removeClass("selected");
                $("#SelectItems").find("li").eq(3).addClass("selected");
            break;
            case "16005" :
                if (ProjectList.State.OrderState != "16005") {
                    ProjectList.setState({
                        "OrderState": state
                    }, function () {
                        ProjectList.getData(1);
                    });
                }
                $(".selected").removeClass("selected");
                $("#SelectItems").find("li").eq(6).addClass("selected");
            break;
            case "16006" :
                if (ProjectList.State.OrderState != "16006") {
                    ProjectList.setState({
                        "OrderState": state
                    }, function () {
                        ProjectList.getData(1);
                    });
                }
                $(".selected").removeClass("selected");
                $("#SelectItems").find("li").eq(7).addClass("selected");
            break;
            default: break;
        }
    };



    /*状态栏选项点击切换状态*/
    ProjectList.createSelectItem = function () {

        $("#SelectItems").find("li").off("click").on("click", function (event) {
                var event = window.event || event;
                var target = event.srcElement || event.target;
                ProjectList.SelectItemsClickHandle(event);//切换数据
                ProjectList.HandleOfListButton();//列表操作栏按钮所对应的方法
        });
        ProjectList.shenHeToCur();
    };

    
    /*切换不同状态对应不同的列表数据*/
    ProjectList.SelectItemsClickHandle = function (event) {

        var target = event.srcElement || event.target;
        /*清空select里面携带的参数和默认显示的option*/
        /*ProjectList.State.SearchOptions.MediaType = 0;
        $("#SearchSection").find("select[data-type='MediaType']").find("option")[0].selected=true;*/
        $(target).parent().find(".selected").removeClass("selected");
        $(target).addClass("selected");
        switch (target.innerHTML) {
            case "草稿":
                if (ProjectList.State.OrderState != "16001") {
                    ProjectList.setState({
                        "OrderState": "16001"
                    }, function () {
                        ProjectList.getData(1);
                    });
                }
                $("#List").attr("type",target.innerHTML);
                break;
            case "待审核":
                if (ProjectList.State.OrderState != "16002") {
                    ProjectList.setState({
                        "OrderState": "16002"
                    }, function () {
                        ProjectList.getData(1);
                    });
                }
                $("#List").attr("type",target.innerHTML);
                break;
            case "待执行":
                if (ProjectList.State.OrderState != "16003") {
                    ProjectList.setState({
                        "OrderState": "16003"
                    }, function () {
                        ProjectList.getData(1);
                    });
                }
                $("#List").attr("type",target.innerHTML);
                break;
            case "执行中":
                if (ProjectList.State.OrderState != "16004") {
                    ProjectList.setState({
                        "OrderState": "16004"
                    }, function () {
                        ProjectList.getData(1);
                    });
                }
                $("#List").attr("type",target.innerHTML);
                break;
            case "执行完毕":
                if (ProjectList.State.OrderState != "16008") {
                    ProjectList.setState({
                        "OrderState": "16008"
                    }, function () {
                        ProjectList.getData(1);
                    });
                }
                $("#List").attr("type",target.innerHTML);
                break;
            case "已完成":
                if (ProjectList.State.OrderState != "16009") {
                    ProjectList.setState({
                        "OrderState": "16009"
                    }, function () {
                        ProjectList.getData(1);
                    });
                }
                $("#List").attr("type",target.innerHTML);
                break;
            case "已取消":
                if (ProjectList.State.OrderState != "16005") {
                    ProjectList.setState({
                        "OrderState": "16005"
                    }, function () {
                        ProjectList.getData(1);
                    });
                }
                $("#List").attr("type",target.innerHTML);
                break;
            case "已驳回":
                if (ProjectList.State.OrderState != "16006") {
                    ProjectList.setState({
                        "OrderState": "16006"
                    }, function () {
                        ProjectList.getData(1);
                    });
                }
                $("#List").attr("type",target.innerHTML);
                break;
            default: break;
        }
    };


    /*根据搜索输入框输入事件将输入value存入this.State.SearchOptions里面*/
    ProjectList.SearchInputHandle = function (event) {
        var target = event.srcElement || event.target;
        switch ($(target).attr("data-type")) {
            case "OrderNum":
                this.State.SearchOptions.OrderNum = target.value;
                break;
            case "DemandDescribe":
                this.State.SearchOptions.DemandDescribe = target.value;
                break;
            case "StartTime":
                //this.State.SearchOptions.StartTime = target.value;
                break;
            case "EndTime":
                //this.State.SearchOptions.EndTime = target.value;
                break;
            case "Creater":
                this.State.SearchOptions.Creater = target.value;
                break;
            default: break;
        }
    };


    /*列表操作栏按钮所对应的方法*/
    ProjectList.HandleOfListButton = function () {

        var state = window.location.search.substring(1).split("=")[1];
        $("#List").find("a").each(function () {
            switch ($(this).text()) {
                case "二维码":
                    var SubId = $(this).attr("data-id");
                    var url = "/OrderManager/QRcode.html?OrderID="+SubId;
                    $(this).attr("href", url).attr("target", "_blank");
                    break;
                case "删除":
                    $(this).off("click").on("click", function (event) {
                        event.preventDefault();
                        event.returnValue=false;
                        var SubId = $(this).attr("data-id");
                        layer.confirm('确认删除此订单？', {
                            btn: ['确认','取消'] //按钮
                        }, function(){
                            var url = "/api/ADOrderInfo/UpdateStatus_ADOrderInfo?orderid=" + SubId + "&status=16007";
                            layer.closeAll();
                            setAjax({
                                url : url,
                                type : 'get'
                            },function (data) {
                                if(data.Status == 0){
                                    ProjectList.getData(1);
                                }
                            })
                        })
                    });
                    break;
                case "查看":
                    var SubId = $(this).attr("data-id");
                    var Ordersource = $(this).parents('tr').attr("OrderSource");
                    var mediaType = $(this).parent().parent().attr("data-type");

                    if(Ordersource == '代客下单-智投'){
                        var url = "/OrderManager/WisdomDetailProject.html?orderID=" + SubId;
                        $(this).attr("href", url).attr("target", "_blank");
                    }else{
                        var url = "/OrderManager/new-detailofproject.html?orderID=" + SubId;
                        $(this).attr("href", url).attr("target", "_blank");
                    }
                    switch (mediaType) {
                        case "14003":
                        case "14004":
                        case "14005":
                            var url = "./ListDetailOfProject.html?orderid=" + SubId;
                            $(this).attr("href", url).attr("target", "_blank");
                        break;
                    }
                    break;
                case "审核":
                    var SubId = $(this).attr("data-id");
                    var Ordersource = $(this).parents('tr').attr("OrderSource");
                    var mediaType = $(this).parent().parent().attr("data-type");

                    if(Ordersource == '代客下单-智投'){
                        var url = "/OrderManager/WisdomAuditProject.html?orderID=" + SubId;
                        $(this).attr("href", url).attr("target", "_self");
                    }else{
                        var url = "/OrderManager/AuditProject.html?orderID=" + SubId;
                        $(this).attr("href", url).attr("target", "_self");
                    }
                    switch (mediaType) {
                        case "14003":
                        case "14004":
                        case "14005":
                            var url = "/OrderManager/AuditOrderSelf.html?MediaType=" + 14003 + "&orderID=" + SubId;
                            $(this).attr("href", url).attr("target", "_self");
                            break;
                    }
                    break;
                case "编辑":
                    var SubId = $(this).attr("data-id"); //根据id
                    var Ordersource = $(this).parents('tr').attr("OrderSource");
                    var mediaType = $(this).parent().parent().attr("data-type");

                    switch (mediaType) {
                        case "14001":
                        case "14003":
                        case "14004":
                        case "14005":
                            var url = "/OrderManager/requirementSelf01.html?MediaType=" + 14001 + "&orderID=" + SubId+"&OrderState="+1;
                            $(this).attr("href", url).attr("target", "_self");
                            break;
                        case "14002":
                            var url = "/OrderManager/requirementApp.html?MediaType=" + 14002 + "&orderID=" + SubId+"&OrderState="+1;
                            $(this).attr("href", url).attr("target", "_self");
                            break;
                    }

                    if(Ordersource == '代客下单-智投' && mediaType == '14001'){
                        var url = "/OrderManager/editWisdomDelivery01.html?orderID=" + SubId;
                        $(this).attr("href", url).attr("target", "_self");
                    }
                    
                    break;
                default: break;
            }
        });
    };
    

    /*从后台获取数据（ajax）并且及时进行列表的渲染以及更改页码。*/
    ProjectList.getData = function (pageIndex) {

        var url = "http://www.chitunion.com/api/ADOrderInfo/SelectOrderInfo?v=1_1";
        setAjax({
            url: url,
            type: "get",
            data:{
                "OrderType" : 0,
                "OrderNum" : ProjectList.State.SearchOptions.OrderNum,
                "OrderState" : ProjectList.State.OrderState,
                "DemandDescribe" : ProjectList.State.SearchOptions.DemandDescribe,
                "StartTime" : ProjectList.State.SearchOptions.StartTime,
                "EndTime" : ProjectList.State.SearchOptions.EndTime,
                "MediaType" : ProjectList.State.SearchOptions.MediaType,
                "Creater" : ProjectList.State.SearchOptions.Creater,
                "pagesize" : 20,
                "PageIndex" : pageIndex,
                "CustomerID" : ProjectList.State.SearchOptions.CustomerID,
                'OrderSource':ProjectList.State.SearchOptions.OrderSource,
                'IsCRM':ProjectList.State.SearchOptions.IsCRM,
                'SubOrderNum':ProjectList.State.SearchOptions.SubOrderNum
            }
        }, function (data) {
            if (data.Status == 0) {
                ProjectList.setState({
                    "OrderList": data.Result.OrderList,
                    "TotalCount": data.Result.TotalCount
                }, function () {
                    ProjectList.createList();
                    if (ProjectList.State.OrderList.length > 0) {
                        $("#No_Data").html("");
                        ProjectList.createPageController(pageIndex);
                    } else {
                        $("#pageContainer").html("");
                        ProjectList.createNoDataInfo();
                    }
                });
            } else {
                layer.msg(data.Message,{'time':1000});
            }
        }, function (err) { });
    };



    /*输入搜索条件  然后查询数据    根据value赋值给ProjectList.State.SearchOptions*/
    ProjectList.createSearchSection = function (fn) {
        ProjectList.Render("#SearchSectionTemplate", "#SearchSection", function () {

            /*input*/
            $("#SearchSection").find("input").off("input").on("input", function (event) {
                var event = window.event || event;
                ProjectList.SearchInputHandle(event);//将value绑定到ProjectList.State.SearchOptions上
            });

            /*select有些特殊，不能直接用ejs赋值value,所以需要手动绑定一下数据。*/
            //项目类型
            $("#SearchSection").find("select[data-type='OrderSource']").val(ProjectList.State.SearchOptions.OrderSource);
            $("#SearchSection").find("select[data-type='OrderSource']").off("change").on("change", function (event) {
                var event = window.event || event;
                var target = event.srcElement || event.target;
                ProjectList.State.SearchOptions.OrderSource = target.value;
            });

            //媒体类型
            $("#SearchSection").find("select[data-type='MediaType']").val(ProjectList.State.SearchOptions.MediaType);
            $("#SearchSection").find("select[data-type='MediaType']").off("change").on("change", function (event) {
                var event = window.event || event;
                var target = event.srcElement || event.target;
                ProjectList.State.SearchOptions.MediaType = target.value;
            });

            //是否关联CRM
            $("#SearchSection").find("select[data-type='IsCRM']").val(ProjectList.State.SearchOptions.IsCRM);
            $("#SearchSection").find("select[data-type='IsCRM']").off("change").on("change", function (event) {
                var event = window.event || event;
                var target = event.srcElement || event.target;
                ProjectList.State.SearchOptions.IsCRM = target.value;
            });


            var AUserName = [];
            var ATrueName = [];
            var AMobile = [];
            var AUserId = [];

            /*广告主模糊查询*/
            $("#SearchSection").find("input[data-type=CustomerID]").off("keyup").on('keyup',function(){

                AUserName = [];
                ATrueName = [];
                AMobile = [];
                AUserId = [];

                var val = $.trim($(this).val());
                if(val.length > 0 && val!=" "){
                    setAjax({
                        url:"/api/ADOrderInfo/GetADMaster?v=1_1",
                        type:'get',
                        data:{
                            NameOrMobile:val,
                            IsAEAuth:1
                        },
                        dataType:'json'
                    },function(data){
                        
                        if(data.Status == 0){
                            var availableTags = [];
                            var UserID = "";

                            for(var i = 0;i<data.Result.length;i++){
                                UserID = data.Result[i].UserID;
                                if(data.Result[i].UserName.toUpperCase().indexOf(val)!=-1 || data.Result[i].UserName.toLowerCase().indexOf(val)!=-1 || data.Result[i].Mobile.toUpperCase().indexOf(val)!=-1 || data.Result[i].Mobile.toLowerCase().indexOf(val)!=-1 || data.Result[i].TrueName.toUpperCase().indexOf(val)!=-1 || data.Result[i].TrueName.toLowerCase().indexOf(val)!=-1){

                                    availableTags.push(data.Result[i].UserName);
                                    availableTags.push(data.Result[i].TrueName);
                                    availableTags.push(data.Result[i].Mobile);
                                    //availableTags.push(data.Result[i].UserID);
                                    
                                    AUserName.push(data.Result[i].UserName);
                                    ATrueName.push(data.Result[i].TrueName);
                                    AMobile.push(data.Result[i].Mobile);
                                    AUserId.push(data.Result[i].UserID);
                                }
                                //ProjectList.State.SearchOptions.CustomerID = UserID;//CustomerID

                            }
                            /*将availableTags去重*/
                            var result = [];
                            for (var i = 0; i < availableTags.length; i++) {
                                if (result.indexOf(availableTags[i]) == -1) {
                                    result.push(availableTags[i]);
                                }
                            }
                            $("#SearchSection").find("input[data-type=CustomerID]").autocomplete({
                                source: result,
                                select : function (event,ui) {
                                    var txt = ui.item.value;
                                    var a = ATrueName.indexOf(txt);
                                    var b = AUserName.indexOf(txt);
                                    var c = AMobile.indexOf(txt);
                                    /*在模糊查询中选定一项确切的数据后获取userId*/
                                    if(a != -1){
                                        ProjectList.State.SearchOptions.CustomerID = AUserId[a];
                                        console.log(AUserId[a]);
                                    }else if(b != -1){
                                        ProjectList.State.SearchOptions.CustomerID = AUserId[b];
                                        console.log(AUserId[b]);
                                    }else if(c != -1){
                                        ProjectList.State.SearchOptions.CustomerID = AUserId[c];
                                        console.log(AUserId[c]);
                                    }
                                }
                            })

                        }else{
                            ProjectList.State.SearchOptions.CustomerID = "";
                        }
                    })
                }
            });


            $("#SearchSection").find("input[data-type=CustomerID]").off("focus").on('focus',function(){

                var val = $.trim($(this).val());
                if(val == ""){
                    setAjax({
                        url:"/api/ADOrderInfo/GetADMaster?v=1_1",
                        type:'get',
                        data:{
                            NameOrMobile:val,
                            IsAEAuth:0
                        },
                        dataType:'json'
                    },function(data){

                        if(data.Status != 0){
                            var availableTags = [];
                            $("#SearchSection").find("input[data-type=CustomerID]").autocomplete({
                                source: availableTags
                            })
                            ProjectList.State.SearchOptions.CustomerID = "";
                        }

                    })
                }
            });


            /*日期控件绑定数据*/
            $("#SearchSection").find("#startTime").off("click").on("click", function () {
                laydate({
                    elem: "#startTime",
                    fixed: false,
                    choose: function (date) {
                        if(date>$("#SearchSection").find("#endTime").val() && $("#SearchSection").find("#endTime").val()){
                            layer.alert('起始时间不能大于结束时间！');
                            $("#SearchSection").find("#startTime").val('')
                        }
                        ProjectList.State.SearchOptions.StartTime = date;
                    }
                });
            });
            $("#SearchSection").find("#endTime").off("click").on("click", function () {
                laydate({
                    elem: "#endTime",
                    fixed: false,
                    choose: function (date) {
                        if(date<$("#SearchSection").find("#startTime").val() && $("#SearchSection").find("#startTime").val()){
                            layer.alert('结束时间不能小于起始时间！');
                            $("#SearchSection").find("#endTime").val('')
                        }
                        ProjectList.State.SearchOptions.EndTime = date;
                    }
                });
            });

            /*点击查询按钮渲染数据*/
            $("#SearchSection").find(".but_add").off("click").on("click", function () {
                
                var val = $.trim($("#AdventerMaster").val());
                var CustomerID = ProjectList.State.SearchOptions.CustomerID;

                console.log(val,CustomerID);
                console.log(ATrueName);

                var a = ATrueName.indexOf(val);
                var b = AUserName.indexOf(val);
                var c = AMobile.indexOf(val);

                if(val == ""){
                    CustomerID = "";
                    ProjectList.getData(1);
                }else{
                    ProjectList.getData(1);
                    if(a == -1 && b == -1 && c == -1){
                        layer.alert("请输入有效的广告主");
                        $("#AdventerMaster").val("");
                        ProjectList.State.SearchOptions.CustomerID = "";
                        ProjectList.getData(1);
                    }else{
                        ProjectList.getData(1);
                    }
                }

            });


            if (fn) {
                fn();
            }
        });
    };
    

    /*根据不同的角色创建列表*/
    ProjectList.createList = function () {
        switch (ProjectList.State.CTLogin.RoleIDs) {
            case "SYS001RL00001": //超级管理员
                ProjectList.Render("#ListTemplate-1", "#List", function () {
                    ProjectList.HandleOfListButton();
                });
                break;
            case "SYS001RL00004": //假设为运营
                ProjectList.Render("#ListTemplate-1", "#List", function () {
                    ProjectList.HandleOfListButton();
                });
                break;
            case "SYS001RL00005": //假设为AE
                ProjectList.Render("#ListTemplate-1", "#List", function () {
                    ProjectList.HandleOfListButton();
                });
                break;
            case "SYS001RL00002": //假设为广告主
                ProjectList.Render("#ListTemplate-2", "#List", function () {
                    ProjectList.HandleOfListButton();
                });
                $('.menu_nav .but_query').hide();
                break;
            case "SYS001RL00008": //假设为销售
                ProjectList.Render("#ListTemplate-3", "#List", function () {
                    ProjectList.HandleOfListButton();
                });
                break;
            default: break;
        }
        judgeAuthority();//根据角色判断功能权限
        ProjectList.CrmNumADD();
        ProjectList.FinalReport();
    };


    ProjectList.FinalReport = function(){//结案报告
        $('.final_report').on('click',function (event) {
            var that = $(this);
            var SubId = that.attr("data-id");
            setAjax({
                url : 'http://www.chitunion.com/api/ADOrderInfo/SelectFinalReportUrlByOrderID?v=1_1',
                type : 'get',
                data : {
                    OrderID : SubId
                }
            },function (data) {
                var Result = data.Result;
                if(data.Status == 0){
                    //that.attr('href','http://www.chitunion.com'+Result);
                    window.open('http://www.chitunion.com'+Result);
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
        })
    }


    
    /*创建页码*/
    ProjectList.createPageController = function (pageIndex) {
        var counts = ProjectList.State.TotalCount;
        $("#pageContainer").pagination(counts, {
            current_page: (pageIndex ? pageIndex : 1),
            items_per_page: 20, //每页显示多少条记录（默认为10条）
            callback: function (currPage) {
                // console.log(currPage); //当前页面，默认从1开始
                ProjectList.getData(currPage);
            }
        });
    };
    

    /*创建无数据时的提示信息*/
    ProjectList.createNoDataInfo = function () {
        ProjectList.Render("#No_DataTemplate", "#No_Data");
    };


    /*关闭弹层*/
    ProjectList.closeDatePoup = function(){
        $(".button").on("click",function(e){
            e.preventDefault();
            e.returnValue=false;
            $.closePopupLayer('popLayerDemo');
        })
        $("#closebt").on("click",function(e){
            e.preventDefault();
            e.returnValue=false;
            $.closePopupLayer('popLayerDemo');
        })
    };

    //CRM合同编号   销售AE运营超管都能维护
    ProjectList.CrmNumADD = function(){

        var flag = false;

        $('#List tr td').on('mouseenter',function(){
            var that = $(this);
            that.find('.CrmNum').removeAttr('disabled');
            //that.find('.CrmNum').css({'border':'1px solid #ccc'});
        }).on('mouseleave',function(){
            var that = $(this);
            that.find('.CrmNum').css({'border':'none'});
            //that.find('.CrmNum').attr('disabled',true);
        }).on('click',function(){
            var that = $(this);
            var cur = that.find('.CrmNum');
            var OrderNum = that.attr('OrderNum');
            cur.css({'border':'1px solid #ccc'});
        })

       
        $('#List tr td .CrmNum').on('blur',function(){
            var that = $(this);
            var txt = $.trim(that.val());
            var OrderNum = that.parent().attr('OrderNum');
            var len = txt.length;
            var reg = /^([a-z]|[A-Z]|[0-9]|\s)+$/;

            if(txt != ''){
                if(len > 15){
                    layer.msg('不能超过15个字符哦',{'time':1000});
                    that.css({'border':'1px solid #ccc'});
                }else if(!reg.test(txt)){
                    layer.msg('请输入字母或者数字',{'time':1000});
                    that.css({'border':'1px solid #ccc'});
                }else{
                    that.css({'border':'none'});
                    that.attr('disabled',true);
                    flag = true;
                    senToNxt(OrderNum,txt,that);
                }
            }else{
                that.css({'border':'none'});
                that.attr('disabled',true);
                flag = true;
                senToNxt(OrderNum,txt,that);
            }
        })


        //新增CRM合同编号 接口
        function senToNxt(OrderNum,CrmNum,ele){
            if(flag == true){
                setAjax({
                    url : 'http://www.chitunion.com/api/ADOrderInfo/UpdateCrmNum?v=1_1',
                    type : 'post',
                    data : {
                        'OrderNum' : OrderNum,
                        'CrmNum' : CrmNum
                    }
                },function(data){
                    if(data.Status == 0){
                        $(ele).css({'border':'none'});
                        $(ele).attr('disabled','disabled');
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                })
            }
        }
        
    };


    /*函数执行*/
    ProjectList.createSelectItem();
    ProjectList.createSearchSection();
    ProjectList.getData(1);
    
});


/*数组去重
function ArrNoRepeat(arr) {
    var result = [];
    for (var i = 0; i < arr.length; i++) {
        if (result.indexOf(arr[i]) == -1) {
            result.push(arr[i]);
        }
    }
    return result;
};*/