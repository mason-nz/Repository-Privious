/**
 * Created by luq on 2017/3/2.
 */
$(function(){
    var ListDetail = new ComponentSection({});
    var SearchId = window.location.search;
    var searchid = Array.prototype.slice.call(SearchId).splice(12).join("");
    ListDetail.addSelfState({
        "SearchId":SearchId
    });
    /*获取订单的基本信息，以及子订单的信息*/
    ListDetail.getDate = function(){
        var url = "/api/ADOrderInfo/GetByOrderID_ADOrderInfo"+ListDetail.State.SearchId;
        setAjax({
            url:url,
            type:"get"
        },function(data){
            if(data.Status == 0){
                ListDetail.setState({
                    "ADOrderInfo":data.Result.ADOrderInfo,
                    "SubADInfos":data.Result.SubADInfos,
                    "MediaType":data.Result.ADOrderInfo.MediaType.toString()
                },function(){
                    ListDetail.ReturnAllMoney();//计算总金额
                    ListDetail.GetCPDAndCPM();//CPD和CPM
                });
                ListDetail.ReturnStateText(data.Result.ADOrderInfo.Status);
            }else {
                layer.alert("数据加载失败，请稍后重试~");
            }
            ListDetail.createBaseInfo();
        },function(error){
            layer.alert("数据加载失败，请稍后重试~");
        });
    };



    /*根据返回数据判断订单所处阶段状态，根据状态码，得到文字信息。*/
    ListDetail.ReturnStateText = function(status){
        switch (status){
            case 16001:
                ListDetail.setState({
                    "Status":"草稿"
                });
                break;
            case 16002:
                ListDetail.setState({
                    "Status":"待审核"
                });
                break;
            case 16003:
                ListDetail.setState({
                    "Status":"待执行"
                });
                break;
            case 16004:
                ListDetail.setState({
                    "Status":"执行中"
                });
                break;
            case 16005:
                ListDetail.setState({
                    "Status":"已取消"
                });
                break;
            case 16006:
                ListDetail.setState({
                    "Status":"已驳回"
                });
                break;
            case 16007:
                ListDetail.setState({
                    "Status":"已删除"
                });
                break;
            case 16008:
                ListDetail.setState({
                    "Status":"执行完毕"
                });
                break;
            case 16009:
                ListDetail.setState({
                    "Status":"订单完成"
                });
                break;
            default:break;
        }
    };
    //根据获取数据分为app和自媒体
    ListDetail.GetCPDAndCPM = function(){
        //根据广告为信息，将app的分成cpd和cpm
        var CPD = [];
        var CPM = [];
        if(ListDetail.State.MediaType == "14002"){
            ListDetail.State.SubADInfos.forEach(function(item){
                if(item.APPDetails){
                    item.APPDetails.forEach(function(data){
                        if(data.CPDCPM == "11001"){
                            CPD.push(data);
                        }else if(data.CPDCPM == "11002"){
                            CPM.push(data);
                        }
                    });
                }
            });

            ListDetail.State.ADOrderInfo.CPDArray = CPD;//将CPD和CPM存入主订单里面
            ListDetail.State.ADOrderInfo.CPMArray = CPM;
            ListDetail.createADList();

        }else{//自媒体
            ListDetail.createADList();
        }
    };


    // 计算总额
    ListDetail.ReturnAllMoney = function(){
        var AllMoney= 0;
        if(ListDetail.State.MediaType == "14002"){//app
            ListDetail.State.SubADInfos.forEach(function(item){
                if(item.APPDetails){
                    item.APPDetails.forEach(function(data){
                        AllMoney += data.AdjustPrice;
                    });
                }
            });
        }else{//自媒体
            ListDetail.State.SubADInfos.forEach(function(item){
                item.SelfDetails.forEach(function(data){
                  AllMoney += data.AdjustPrice;
                })
            })
        }
        ListDetail.setState({
            "AllMoney":AllMoney
        });
    };

    /*创建订单基本信息*/
    ListDetail.createBaseInfo = function(){
        ListDetail.Render("#OrderInfoTemplate","#OrderInfo");
    };
    /*创建广告位列表*/
    ListDetail.createADList = function(){
        switch (ListDetail.State.MediaType){
            case "14001":
                ListDetail.Render("#ADInfoTemplate-other","#ADInfo");
                break;
            case "14002":
                ListDetail.Render("#ADInfoTemplate-app","#ADInfo");
                break;
            case "14003":
                ListDetail.Render("#ADInfoTemplate-other","#ADInfo");
                break;
            case "14004":
                ListDetail.Render("#ADInfoTemplate-other","#ADInfo");
                break;
            case "14005":
                ListDetail.Render("#ADInfoTemplate-other","#ADInfo");
                break;
            default:break;
        }
        ListDetail.openDatePoup();
    };

    //查看排期
    ListDetail.openDatePoup = function(){


        $(".selectDate").each(function(){
            $(this).on("click",function(e){
                var that = $(this);
                e.preventDefault();
                $.openPopupLayer({
                    name: "popLayerDemo",
                    url: "/OrderManager/detailDatePoup.html",
                    success:function(){

                        //查看排期接口
                        var addId = that.parents("tr").attr("addDetailId");
                        setAjax({
                            url : "/api/ADOrderInfo/GetByDetailID_CPDScheduleInfo?addetailinfoid="+addId,
                            type : "get"
                        },function(data){
                            if(data.Status == 0){
                                var result = data.Result;
                                that.parents("tr").attr("adscheduleinfos",JSON.stringify(result));


                                //获取开始日期和结束日期
                                var beginDay = $(".appointmentTime").find(".startTime").text().substr(0,10);
                                // console.log("开始时间是"+beginDay);
                                var endDay = $(".appointmentTime").find(".endTime").text().substr(0,10);
                                // console.log("结束时间是"+endDay);

                                //调用设置排期方法，显示所有日期
                                setSchedule(beginDay,endDay);
                                var dateArr = getAll(beginDay,endDay);
                                displaySchedule(that);

                                var selectList = $(".select");
                                $(".ml20").text("投放天数:" + selectList.length + "天");
                                ListDetail.closeDatePoup();


                            }else{
                                layer.alert(data.Message);
                            }
                        })
                    }
                })
            })
        })
    };

    //关闭排期弹窗
    ListDetail.closeDatePoup = function(){
        $(".but_keep").on("click",function(e){
            e.preventDefault();
            $.closePopupLayer('popLayerDemo');
        })
        $("#closebt").on("click",function(e){
            e.preventDefault();
            $.closePopupLayer('popLayerDemo');
        })
    };

    ListDetail.getDate();
});

/*数组去重
function arrRepeat(arr) {
    var result = [];
    for (var i = 0; i < arr.length; i++) {
        if (result.indexOf(arr[i]) == -1) {
            result.push(arr[i])
        }
    }
    return result;
};*/
