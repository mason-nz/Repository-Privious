/**
 * Created by fengb on 2017/7/24.
 */

$(function () {

    var orderID = GetQueryString('orderID')!=null&&GetQueryString('orderID')!='undefined'?GetQueryString('orderID'):null;

    var config = {};
    var DetailOfOrder = {
        init : function () {

            //根据项目号查看项目的接口  取信息
            var url = 'http://www.chitunion.com/api/ADOrderInfo/IntelligenceADOrderInfoQuery?v=1_1';
            setAjax({
                url : url,
                type : 'get',
                data : {
                    orderid : orderID
                }
            },function (data) {
                var Result = data.Result;
                if(data.Status == 0){
                    $('.install_box').html(ejs.render($('#wisdom-project').html(),Result));
                    
                    //计算每一个地区的价格的总计
                    Result.AreaInfos.forEach(function(item,i){
                        var PublishDetails = item.PublishDetails;
                        var SalePrice = 0;//销售
                        var OriginalReferencePrice = 0;//原创
                        var CostReferencePrice = 0;//成本
                        var FinalCostPrice = 0;//最终成本价
                        PublishDetails.forEach(function(each){
                            SalePrice += each.SalePrice;
                            CostReferencePrice += each.CostReferencePrice;
                            FinalCostPrice += each.FinalCostPrice;
                            if(each.EnableOriginPrice == true){
                                OriginalReferencePrice += each.OriginalReferencePrice;
                            }else{
                                OriginalReferencePrice += 0;
                            }
                        })
                        var cur_tab = $('.to_b_list_box').find('.to_b_list').eq(i);
                        cur_tab.find('.SalePrice').text(formatMoney(SalePrice,2));
                        cur_tab.find('.CostReferencePrice').text(formatMoney(CostReferencePrice,2));
                        cur_tab.find('.FinalCostPrice').text(formatMoney(FinalCostPrice,2));
                        cur_tab.find('.OriginalReferencePrice').text(formatMoney(OriginalReferencePrice,2));
                    })

                    DetailOfOrder.ModifyAmount();
                    DetailOfOrder.returnStateText(Result.ADOrderInfo.Status);
                    DetailOfOrder.operatingBtn();
                    DetailOfOrder.LayoutChanges();
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
        },
        returnStateText : function(status) {
            switch (status) {
                case 16001:
                    $('.orderStatus').text('草稿');
                    break;
                case 16002:
                    $('.orderStatus').text('待审核');
                    break;
                case 16003:
                    $('.orderStatus').text('待执行');
                    break;
                case 16004:
                    $('.orderStatus').text('执行中');
                    break;
                case 16005:
                    $('.orderStatus').text('已取消');
                    break;
                case 16006:
                    $('.orderStatus').text('已驳回');
                    break;
                case 16007:
                    $('.orderStatus').text('已删除');
                    break;
                case 16008:
                    $('.orderStatus').text('执行完毕');
                    break;
                case 16009:
                    $('.orderStatus').text('已完成');
                    break;
                default:
                    break;
            }
        },
        ModifyAmount : function() {// 计算广告位个数

            var SalePrice = 0;//销售
            var OriginalReferencePrice = 0;//原创
            var CostReferencePrice = 0;//成本
            var FinalCostPrice = 0;//最终成本价

            //媒体个数
            var allPositionCount = $('.to_b_list_box .Tr').length;
            $('.allPositionCount').text(allPositionCount);

            $('.to_b_list_box .Tr').each(function () {
                var that = $(this);
                var SalesVal = $(this).attr('SalePrice')*1;
                var OriginVal = $(this).attr('OriginalReferencePrice')*1;
                var coastVal = $(this).attr('CostReferencePrice')*1;
                var FinalVal = $(this).attr('FinalCostPrice')*1;
                SalePrice += SalesVal;
                OriginalReferencePrice += OriginVal;
                CostReferencePrice += coastVal;
                FinalCostPrice += FinalVal;
            })
            //销售参考价总计
            $('.SalePriceTotal').text(formatMoney(SalePrice+OriginalReferencePrice,2));
            //成本参考价总计
            $('.CostReferencePriceTotal').text(formatMoney(CostReferencePrice+OriginalReferencePrice,2));
        },
        operatingBtn : function(){//驳回情况下    可以确认再提及改状态为待审核
            
            $('.react_reason .but_add').off('click').on('click',function () {
                setAjax({
                    url : 'http://www.chitunion.com/api/ADOrderInfo/UpdateStatus_ADOrderInfo',
                    type : 'get',
                    data : {
                        'orderid' : orderID,
                        'status' : 16002
                    }
                },function (data) {
                    if(data.Status == 0){
                        window.location = '/ordermanager/listofproject.html';
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                })
            })
        },
        LayoutChanges : function () {//展开收起
            var flag = true;

            $('#layoutChanges').off('click').on('click',function () {
                var that = $(this);
                if(flag == true) {
                    that.parents('.demand').find('ul').hide();
                    that.find('img').attr('src','../images/collection08.png');
                    flag = false;
                }else{
                    that.parents('.demand').find('ul').show();
                    that.find('img').attr('src','../images/collection09.png');
                    flag = true;
                }
            })
        }
    };

    DetailOfOrder.init();

})


//获取url 地址参数方法
function GetQueryString(name) {
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if(r!=null)return r[2]; return null;
}


