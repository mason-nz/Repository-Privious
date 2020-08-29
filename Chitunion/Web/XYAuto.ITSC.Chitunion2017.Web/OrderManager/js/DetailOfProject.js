/**
 * Created by fengb on 2017/4/27.
 */

$(function () {

    var orderID = GetQueryString('orderID')!=null&&GetQueryString('orderID')!='undefined'?GetQueryString('orderID'):null;

    //获取url 地址参数方法
    function GetQueryString(name) {
        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if(r!=null)return r[2]; return null;
    }

    var config = {};
    var DetailOfOrder = {
        constructor : DetailOfOrder,
        init : function () {
            //根据项目号查看项目的接口  取信息
            var url = '/api/ADOrderInfo/GetByOrderID_ADOrderInfo?v=1_1';
            //var url = 'js/listDetail.json';
            setAjax({
                url : url,
                type : 'get',
                data : {
                    orderid : orderID
                }
            },function (data) {
                config = data.Result;
                var Status = config.ADOrderInfo.Status;
                config.Status = Status;
                DetailOfOrder.showTemplate(config,'#DetailOfProjectForWeChat');
                DetailOfOrder.ModifyAmount();
                //DetailOfOrder.initMoney();
                DetailOfOrder.returnStateText(config.ADOrderInfo.Status);
            })
        },
        showTemplate : function (data,id) {
            var _this = this;
            //->首先把页面中模板的innerHTML获取到
            var str=$(id).html();
            //->然后把str和data交给EJS解析处理，得到我们最终想要的字符串
            var result = ejs.render(str, {
                data: data
            });
            //->最后把获取的HTML放入到MENU
            $(".install_box").html(result);
            lookDetailHolidays();//鼠标悬浮效果 以及链接
            DetailOfOrder.operatingBtn();

        },
        returnStateText : function(status) {
            switch (status) {
                case 16001:
                    $('.orderStatus').text('草稿');
                    //$('.subOrderStatus').text('草稿');
                    break;
                case 16002:
                    $('.orderStatus').text('待审核');
                    //$('.subOrderStatus').text('待审核');
                    break;
                case 16003:
                    $('.orderStatus').text('待执行');
                    //$('.subOrderStatus').text('待执行');
                    break;
                case 16004:
                    $('.orderStatus').text('执行中');
                    //$('.subOrderStatus').text('执行中');
                    break;
                case 16005:
                    $('.orderStatus').text('已取消');
                    //$('.subOrderStatus').text('已取消');
                    break;
                case 16006:
                    $('.orderStatus').text('已驳回');
                    //$('.subOrderStatus').text('已驳回');
                    break;
                case 16007:
                    $('.orderStatus').text('已删除');
                    //$('.subOrderStatus').text('已删除');
                    break;
                case 16008:
                    $('.orderStatus').text('执行完毕');
                    //$('.subOrderStatus').text('执行完毕');
                    break;
                case 16009:
                    $('.orderStatus').text('已完成');
                    //$('.subOrderStatus').text('已完成');
                    break;
                default:
                    break;
            }
        },
        ModifyAmount : function() {// 计算广告位个数
            var AllMoney = 0;
            var allPositionCount = 0;
            var SelfDetailsLen = 0;
            var APPDetailsLen = 0;

            //总价
            /*$('.to_b_list .totalPrices').each(function () {
                var val = $(this).parent().attr('lastMoney')*1;
                AllMoney += val;
            })*/
            $('.allMoney').text(formatMoney(config.ADOrderInfo.TotalAmount,2));

            //广告位个数
            config.SubADInfos.forEach(function (items) {
                if(items.SelfDetails != null){
                    SelfDetailsLen += items.SelfDetails.length;  
                }
                if(items.APPDetails != null){
                    APPDetailsLen += items.APPDetails.length;  
                }
            })
            allPositionCount = SelfDetailsLen + APPDetailsLen;
            $('.allPositionCount').text(allPositionCount);

        },
        initMoney : function(){//计算appCPD的 初始化总额
            config.SubADInfos.forEach(function (items,j) {
                if(items.APPDetails != null){
                    items.APPDetails.forEach(function (item,i) {
                        if(item.CPDCPM == '11001'){//CPD
                            var allDays = 0;
                            var Holidays = 0;
                            var workingDays = 0;
                            item.ADSchedules.forEach(function (each,e) {
                                allDays += each.AllDays;
                                Holidays += each.Holidays;
                                workingDays = allDays-Holidays;
                            })

                            var cur = $('.to_b_list').find('.cpdApp').eq(i);
                            var originalprice = cur.attr('originalprice')*1;
                            var priceHoliday = cur.attr('priceholiday')*1;
                            //赋给初始化变量  到每一行里面
                            cur.attr('allDays',allDays);//总时间
                            cur.attr('Holidays',Holidays);//假期时间
                            cur.attr('workingDays',workingDays);//工作日时间
                            //每一行的总价  然后赋值给tr
                            var total = Holidays*priceHoliday + workingDays*originalprice;
                            cur.find('.totalPrices').val(total);
                            cur.find('.totalPrice').attr('totalprice',total);
                            cur.attr('AdjustPrice',total);
                            //已失效已过期的情况下不算总额   那就默认渲染
                            cur.find('.init_total').html(total);
                            DetailOfOrder.ModifyAmount();
                            //只有一个排期的时候 初始化把删除按钮隐藏
                            if(item.ADSchedules.length == 1){
                                cur.find('.app_delete').hide();
                            }
                        }
                    })
                }
            })
        },
        operatingBtn : function(){//状态后面的操作
            //审核
            $('.audit_project').on('click',function(){
                var SubId = $(this).attr("OrderID");
                var url = "/OrderManager/AuditProject.html?orderID=" + SubId;
                window.open(url);
            })
            //删除
            $('.delete_project').on('click',function(){
                var SubId = $(this).attr("OrderID");
                layer.confirm('确认删除该项目？', {
                    btn: ['确认','取消'] //按钮
                }, function(){
                    var url = "/api/ADOrderInfo/UpdateStatus_ADOrderInfo?orderid=" + SubId + "&status=16007";
                    layer.closeAll();
                    setAjax({
                        url : url,
                        type : 'get'
                    },function (data) {
                        if(data.Status == 0){
                            window.close();
                        }
                    })
                })
            })
        }
    };

    DetailOfOrder.init();

    //鼠标悬浮查看节假日  以及各种链接
    function lookDetailHolidays() {

        $('.look_holidays').each(function () {
            var that = $(this);
            that.off('mouseenter').on('mouseenter',function (e) {
                e.preventDefault();
                e.stopPropagation();
                var detailDays = that.next().html();
                that.parents('.appTime').css('position','relative');
                that.parents('.appTime').find('.SuspensionElements').show();
                that.parents('.appTime').find('.SuspensionElements').html(detailDays);
            })
            that.off('mouseleave').on('mouseleave',function (e) {
                e.preventDefault();
                e.stopPropagation();
                that.parents('.appTime').find('.SuspensionElements').hide();
            })
        })

        //无用的tr链接
        $('.uselessTr').each(function () {
            var that = $(this);
            var HasOtherPublish = that.attr('HasOtherPublish')*1;
            var expired = that.attr('expired')*1;
            var MediaType = that.attr('MediaType')*1;
            var MediaID = that.attr('MediaID')*1;
            var TemplateID = that.attr('TemplateID')*1;

            if(MediaType == 14001){//微信
                if(HasOtherPublish == 1){//跳转到详情页
                    that.find('.content a').eq(0).on('click',function () {
                        $(this).attr('target','_blank');
                        $(this).attr('href','/OrderManager/wx_detail.html?MediaType='+MediaType+'&MediaID='+MediaID);
                    })
                }else if(HasOtherPublish == 0){//提示
                    that.find('.content a').eq(0).on('click',function () {
                        alert('很抱歉，该广告已下架或被删除');
                    })
                }else if(expired == -1){//已失效
                    that.find('.content a').eq(0).on('click',function () {
                        alert('很抱歉，该广告已下架或被删除');
                    })
                }
            }else{//app
                if(HasOtherPublish == 1){//跳转到详情页
                    that.find('.content a').eq(0).on('click',function () {
                        $(this).attr('target','_blank');
                        $(this).attr('href','/OrderManager/app_detail.html?MediaID='+MediaID+'&TemplateID='+TemplateID);
                    })
                }else if(HasOtherPublish == 0){//提示
                    that.find('.content a').eq(0).on('click',function () {
                        alert('很抱歉，该广告已下架或被删除');
                    })
                }else if(expired == -1){//已失效
                    that.find('.content a').eq(0).on('click',function () {
                        alert('很抱歉，该广告已下架或被删除');
                    })
                }
            }
        })

        //有用的tr链接
        $('.usefulTr').each(function () {
            var that = $(this);
            var MediaType = that.attr('MediaType')*1;
            var MediaID = that.attr('MediaID')*1;
            var TemplateID = that.attr('TemplateID')*1;
            if(MediaType == 14001){
                that.find('.content a').eq(0).on('click',function () {
                    $(this).attr('target','_blank');
                    $(this).attr('href','/OrderManager/wx_detail.html?MediaType='+MediaType+'&MediaID='+MediaID);
                })
            }else{
                that.find('.content a').eq(0).on('click',function () {
                    $(this).attr('target','_blank');
                    $(this).attr('href','/OrderManager/app_detail.html?MediaID='+MediaID+'&TemplateID='+TemplateID);
                })
            }
        })

    };

})



