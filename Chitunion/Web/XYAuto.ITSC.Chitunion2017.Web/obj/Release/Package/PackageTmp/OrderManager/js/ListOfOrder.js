/**
 * Created by luq on 2017/2/28.
 */
$(function(){
    var ProjectList = new ComponentSection({});
    ProjectList.addSelfState({
        "SelectItems":["待执行","执行中","已完成","已取消"],
        "SearchOptions":{
            "OrderNum":"",
            "DemandDescribe":"",
            "StartTime":"",
            "EndTime":"",
            "MediaType":"0",
            "Creater":"",
            "CustomerID": "",
            "OrderSource":'0',
            'IsCRM':'0',
            'SubOrderNum':''
        },
        "CTLogin":CTLogin,
        "OrderState":"16003"
    });
    /****************交互事件，逻辑内容*****************/


    /*状态栏选项点击切换状态*/
    ProjectList.createSelectItem = function(){
        ProjectList.Render("#SelectItemsTemplate","#SelectItems",function(){
            $("#SelectItems").find("li").off("click").on("click",function(event){
                var event = window.event || event;
                ProjectList.SelectItemsClickHandle(event);//切换数据
            });
        });
    };


    /*切换不同状态对应不同的列表数据*/
    ProjectList.SelectItemsClickHandle = function(event){

        var target = event.srcElement || event.target;

        /*清空select里面携带的参数和默认显示的option*/
        /*ProjectList.State.SearchOptions.MediaType = 0;
        $("#SearchSection").find("select[data-type='MediaType']").find("option")[0].selected=true;*/

        $(target).parent().find(".selected").removeClass("selected");
        $(target).addClass("selected");
        switch (target.innerHTML){
            case "草稿":
                if(ProjectList.State.OrderState != "16001"){
                    ProjectList.setState({
                        "OrderState":"16001"
                    },function(){
                        ProjectList.getData(1);
                    });
                }
                break;
            case "待审核":
                if(ProjectList.State.OrderState != "16002"){
                    ProjectList.setState({
                        "OrderState":"16002"
                    },function(){
                        ProjectList.getData(1);
                    });
                }
                break;
            case "待执行":
                if(ProjectList.State.OrderState != "16003"){
                    ProjectList.setState({
                        "OrderState":"16003"
                    },function(){
                        ProjectList.getData(1);
                    });
                }
                break;
            case "执行中":
                if(ProjectList.State.OrderState != "16004"){
                    ProjectList.setState({
                        "OrderState":"16004"
                    },function(){
                        ProjectList.getData(1);
                    });
                }
                break;
            case "执行完毕":
                if(ProjectList.State.OrderState != "16008"){
                    ProjectList.setState({
                        "OrderState":"16008"
                    },function(){
                        ProjectList.getData(1);
                    });
                }
                break;
            case "已完成":
                if(ProjectList.State.OrderState != "16009"){
                    ProjectList.setState({
                        "OrderState":"16009"
                    },function(){
                        ProjectList.getData(1);
                    });
                }
                break;
            case "已取消":
                if(ProjectList.State.OrderState != "16005"){
                    ProjectList.setState({
                        "OrderState":"16005"
                    },function(){
                        ProjectList.getData(1);
                    });
                }
                break;
            case "已驳回":
                if(ProjectList.State.OrderState != "16006"){
                    ProjectList.setState({
                        "OrderState":"16006"
                    },function(){
                        ProjectList.getData(1);
                    });
                }
                break;
            default:break;
        }
    };


    /*根据搜索输入框输入事件将输入value存入this.State.SearchOptions里面*/
    ProjectList.SearchInputHandle = function(event){
        var target = event.srcElement || event.target;
        switch ($(target).attr("data-type")){
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
            case "SubOrderNum":
                this.State.SearchOptions.SubOrderNum = target.value;
                break;   
            default:break;
        }
    };

    

    /*列表操作栏按钮所对应的方法*/
    ProjectList.HandleOfListButton = function(){
        $("#List").find("a").each(function(){
            switch ($(this).text()){
                case "取消订单":
                    ProjectList.ChangeStatusHandle($(this),16005);
                    break;
                case "删除":
                    ProjectList.ChangeStatusHandle($(this),16007);
                    break;
                case "执行":
                    ProjectList.ChangeStatusHandle($(this),16004);
                    break;
                case "上传数据":
                    //微信 app的上传数据进入详情页里面   视频直播的进上传数据页面
                    var SubId = $(this).attr("data-id");
                    var mediaType = $(this).parent().parent().attr("data-type");
                    var Ordersource = $(this).parent().parent().attr("OrderSource");
                    if(Ordersource == '代客下单-智投'){
                        var url = "/OrderManager/WisdomDetailOrder.html?suborderid="+SubId+'&isSelected=2';
                        $(this).attr("href", url).attr("target", "_blank");
                    }else{
                        var url = "/OrderManager/new-detailoforder.html?suborderid="+SubId;
                        $(this).attr("href", url).attr("target", "_blank");
                    }

                    switch(mediaType){
                        case "14003":
                        case "14004":
                        case "14005":
                            var url = "/OrderManager/my_uploadData.html?suborderid="+SubId;
                            $(this).attr("href",url).attr("target","_blank");
                            break;
                    }
                    break;
                case "执行完毕":
                    ProjectList.ChangeStatusHandle($(this),16008);
                    break;
                case "已完成":
                    ProjectList.ChangeStatusHandle($(this),16009);
                    break;
                case "查看":
                    var SubId = $(this).attr("data-id");
                    var mediaType = $(this).parent().parent().attr("data-type");
                    var Ordersource = $(this).parent().parent().attr("OrderSource");

                    if(Ordersource == '代客下单-智投'){
                        var url = "/OrderManager/WisdomDetailOrder.html?suborderid="+SubId+'&isSelected=1';
                        $(this).attr("href", url).attr("target", "_blank");
                    }else{
                        var url = "/OrderManager/new-detailoforder.html?suborderid="+SubId;
                        $(this).attr("href", url).attr("target", "_blank");
                    }
                    switch(mediaType){
                        case "14003":
                        case "14004":
                        case "14005":
                            var url = "/OrderManager/ListDetaileOfOrder.html?suborderid="+SubId;
                            $(this).attr("href",url).attr("target","_blank");
                            break;
                    }
                default:break;
            }
        });
    };


    /*根据之订单号修改子订单状态（上边方法的公用函数）*/
    ProjectList.ChangeStatusHandle = function(DOMTarget,status){
         DOMTarget.off("click").on("click",function(event){
            event.preventDefault();
            event.returnValue=false;
            var id = DOMTarget.attr("data-id");

            var url = "/api/ADOrderInfo/CancelOrDelete_SubADInfo?"+"suborderid="+id+"&status="+status;
            var tipInfo = "";
            switch(status){
                case 16004://执行订单
                    tipInfo = "确认执行此订单？";
                    TipsInfo(url,tipInfo);
                break;
                case 16008://执行完毕
                    tipInfo = "确认将此订单标记为执行完毕？";
                    TipsInfo(url,tipInfo);
                break;
                case 16009://订单完成
                    tipInfo = "确认将此订单标记为已完成？";
                    TipsInfo(url,tipInfo);
                break;
                case 16005://取消订单
                    tipInfo = "确认取消此订单？";
                    TipsInfo(url,tipInfo);
                break;
                case 16007://删除订单
                    tipInfo = "确认删除此订单？";
                    TipsInfo(url,tipInfo);
                break;
                default:break;
            }
        });
        // 提示框函数
        function TipsInfo(url,tipInfo){
            layer.open({
                type: 1,
                offset: 'c' ,//具体配置参考：offset参数项
                content: '<div style="padding: 20px 50px;">'+tipInfo+'</div>',
                btn: ["确定","取消"],
                btnAlign: 'c', //按钮居中
                yes: function(){
                    layer.closeAll();
                    setAjax({
                        url:url,
                        type:"get"
                    },function(data){
                        if(data.Status == 0){
                            layer.closeAll();
                            ProjectList.getData(1);
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    },function(err){
                        layer.open({
                            type:1,
                            offset:"c",
                            content: '<div style="padding: 20px 50px;">网络异常，请稍后重试~</div>',
                            btn: "确定",
                            btnAlign: 'c',
                            yes:function(){
                                layer.closeAll();
                            }
                        });
                    });
                }
            });
        }
    }
    /***********************************************/


    /*从后台获取数据（ajax）并且即使进行列表的渲染以及更改页码。*/
    ProjectList.getData = function(pageIndex){
        
        var url = "http://www.chitunion.com/api/ADOrderInfo/SelectOrderInfo?v=1_1";
        setAjax({
            url:url,
            type:"get",
            data:{
                "OrderType" : 1,
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
        },function(data){
            if(data.Status == 0){
                ProjectList.setState({
                    "OrderList":data.Result.OrderList,
                    "TotalCount":data.Result.TotalCount
                },function(){
                    ProjectList.createList();
                    if(ProjectList.State.OrderList.length > 0){
                        $("#No_Data").html("");
                        ProjectList.createPageController(pageIndex);
                    }else{
                        $("#pageContainer").html("");
                        ProjectList.createNoDataInfo();
                    }
                });
            }else{
                layer.msg(data.Message,{'time':1000});
            }
            
        },function(err){});
    };

    
    /*输入搜索条件  然后查询数据    根据value赋值给ProjectList.State.SearchOptions*/
    ProjectList.createSearchSection = function(fn){
        ProjectList.Render("#SearchSectionTemplate","#SearchSection",function(){

            /*input*/
            $("#SearchSection").find("input").off("input").on("input",function(event){
                var event = window.event || event;
                ProjectList.SearchInputHandle(event);
            });


            /*select有些特殊，不能直接用ejs赋值value,所以需要手动绑定一下数据。*/
            $("#SearchSection").find("select[data-type='MediaType']").val(ProjectList.State.SearchOptions.MediaType);
            $("#SearchSection").find("select[data-type='MediaType']").off("change").on("change",function(event){
                var event = window.event || event;
                var target = event.srcElement || event.target;
                ProjectList.State.SearchOptions.MediaType = target.value;
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
                if(val.length > 0&&val!=" "){
                    setAjax({
                        url:"/api/ADOrderInfo/GetADMaster",
                        type:'get',
                        data:{
                            NameOrMobile:val,
                            IsAEAuth:0
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
                                /*ProjectList.State.SearchOptions.CustomerID = UserID;*///CustomerID
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
                                    console.log(txt);
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
                        url:"/api/ADOrderInfo/GetADMaster",
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
            $("#SearchSection").find("#startTime").off("click").on("click",function(){
                laydate({
                    elem:"#startTime",
                    fixed:false,
                    choose:function(date){
                        if(date>$("#SearchSection").find("#endTime").val() && $("#SearchSection").find("#endTime").val()){
                            layer.alert('起始时间不能大于结束时间！');
                            $("#SearchSection").find("#startTime").val('')
                        }else{
                           ProjectList.State.SearchOptions.StartTime = date;
                        }
                    }
                });
            });
            $("#SearchSection").find("#endTime").off("click").on("click",function(){
                laydate({
                    elem:"#endTime",
                    fixed:false,
                    choose:function(date){
                        if(date<$("#SearchSection").find("#startTime").val() && $("#SearchSection").find("#startTime").val()){
                            layer.alert('结束时间不能小于起始时间！');
                            $("#SearchSection").find("#endTime").val('')
                        }else{
                            ProjectList.State.SearchOptions.EndTime = date;
                        }
                    }
                });
            });

           

            /*点击查询按钮渲染数据*/
            $("#SearchSection").find(".but_add").off("click").on("click", function () {

                var val = $.trim($("#AdventerMaster").val());
                var CustomerID = ProjectList.State.SearchOptions.CustomerID;

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

            if(fn){
                fn();
            }
        });
    };
    

    /*根据不同的角色创建列表*/
    ProjectList.createList = function(){
        switch (ProjectList.State.CTLogin.RoleIDs){
            case "SYS001RL00001"://假设为AE
                ProjectList.Render("#ListTemplate-1","#List",function(){
                    ProjectList.HandleOfListButton();
                });
                break;
            case "SYS001RL00005"://超管
                ProjectList.Render("#ListTemplate-1","#List",function(){
                    ProjectList.HandleOfListButton();
                });
                break;
            case "SYS001RL00002"://假设为广告主
             ProjectList.Render("#ListTemplate-2","#List",function(){
                    ProjectList.HandleOfListButton();
                });
                break;
            case "SYS001RL00004"://假设为运营
             ProjectList.Render("#ListTemplate-1","#List",function(){
                    ProjectList.HandleOfListButton();
                });
                break;
            case "SYS001RL00003"://媒体
             ProjectList.Render("#ListTemplate-4","#List",function(){
                    ProjectList.HandleOfListButton();
                });
            break;
            case "SYS001RL00008"://销售
             ProjectList.Render("#ListTemplate-3","#List",function(){
                    ProjectList.HandleOfListButton();
                });
            break;
            default:break;
        }
        judgeAuthority();
    };


    /*创建页码*/
    ProjectList.createPageController = function(pageIndex){
        var counts = ProjectList.State.TotalCount;
        $("#pageContainer").pagination(counts,{
            current_page: (pageIndex ? pageIndex : 1),
            items_per_page: 20, //每页显示多少条记录（默认为10条）
            callback: function (currPage) {
                // console.log(currPage); //当前页面，默认从1开始
                ProjectList.getData(currPage);
            }
        });
    };


    /*创建无数据时的提示信息*/
    ProjectList.createNoDataInfo = function(){
        ProjectList.Render("#No_DataTemplate","#No_Data");
    };


    /*函数执行*/
    ProjectList.createSelectItem();
    ProjectList.createSearchSection();
    ProjectList.getData(1);
});
