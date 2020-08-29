/*
	fengb  9.8
 */


$(function () {

    var DistributeType = GetQueryString('DistributeType')!=null&&GetQueryString('DistributeType')!='undefined'?GetQueryString('DistributeType'):null;
    var MaterielId = GetQueryString('MaterielId')!=null&&GetQueryString('MaterielId')!='undefined'?GetQueryString('MaterielId'):null;
    var DistributeId = GetQueryString('DistributeId')!=null&&GetQueryString('DistributeId')!='undefined'?GetQueryString('DistributeId'):null;
    var CurDate = GetQueryString('CurDate')!=null&&GetQueryString('CurDate')!='undefined'?GetQueryString('CurDate'):null;

    var config = {};
    var xyConfig = {
    	url : {
    		'GetDetailsQuery' : public_url + '/api/Materiel/GetDetailsQuery',
            'GetInfo' : public_url + '/api/Materiel/GetInfo',
            'Export' : public_url + '/api/Materiel/Export'
    	},
	    Pub_url: {
	        "QueryBrand" : public_url + '/api/CarSerial/QueryBrand',
	        "QuerySerialList" : public_url + '/api/CarSerial/QuerySerialList',
	        "GetDictInfoByTypeId": public_url + '/api/DictInfo/GetDictInfoByTypeId'
	    }
	}
 

    function NewDespense() {

        //默认加载初始化时间
        $('#StartDate').val(CurDate);
        $('#EndDate').val(CurDate);
        
        this.geteverydataBase();
        this.queryparameters();

        $('#searchBtn').click();

        var voidDateRange = [];
        var timeObj = {
            'S': new Date().format('yyyy-MM-dd'),
            'E': '2027-09-15'
        }
        voidDateRange.push(timeObj);

        // 时间控件
        $('#StartDate').off('click').on('click', function () {
            laydate({
                fixed: false,
                elem: '#StartDate',
                voidDateRange : voidDateRange,
                choose: function (date) {
                    if (date > $('#EndDate').val() && $('#EndDate').val()) {
                        layer.msg('起始时间不能大于结束时间！',{'time':1000});
                        $('#StartDate').val('');
                    }
                }
            });
        });
        $('#EndDate').off('click').on('click', function () {
            laydate({
                fixed: false,
                elem: '#EndDate',
                voidDateRange : voidDateRange,
                choose: function (date) {
                    if (date < $('#StartDate').val() && $('#StartDate').val()) {
                        layer.msg('结束时间不能小于起始时间！',{'time':1000});
                        $('#EndDate').val('');
                    }
                }
            });
        });
    }

    NewDespense.prototype = {
        constructor: NewDespense,
        geteverydataBase : function(){
            setAjax({
                url : xyConfig.url.GetInfo,
                type : 'get',
                data : {
                    MaterielId : MaterielId,
                    DistributeType : DistributeType
                }
            },function(data){
                var Result = data.Result;
                if(data.Status == 0){
                    $('.react_reason').html(ejs.render($('#Dispense-Base').html(), Result));
                }else{
                    layer.msg(data.message,{'time':1000});
                }
            })
        },
        // 获取查询参数
        queryparameters: function () {
            var _this = this;
           
            // 搜索
            $('#searchBtn').off('click').on('click', function () {
               //开始时间		
               var StartDate = $('#StartDate').val();
               //结束时间
               var EndDate = $('#EndDate').val();
             
                //初始化的所有的变量
                var obj = {};

                //对象
                obj = {
                    PageSize : 20,
                    PageIndex : 1,
                    StartDate : StartDate,
                    EndDate : EndDate,
                    DistributeType : DistributeType,
                    MaterielId : MaterielId,
                    DistributeId : DistributeId
                }
                _this.requestdata(obj);
            })
        },
        // 请求数据
        requestdata: function (obj) {
        	
            var _this = this;
            setAjax({
                url: xyConfig.url.GetDetailsQuery,
                type: 'get',
                data: obj,
            }, function (data) {

                var Result = data.Result;
                config = Result;
                // 渲染数据
                _this.renderdata(_this, data);
                _this.operation(obj);

                // 如果数据为0显示图片
                if (data.Result.TotalCount != 0) {
                    //分页
                    $("#pageContainer").pagination(
                        data.Result.TotalCount,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                obj.PageIndex = currPage;
                                setAjax({
                                    url: xyConfig.url.GetDetailsQuery,
                                    type: 'get',
                                    data: obj
                                }, function (data) {
                                    var Result = data.Result;
                                    config = Result;
                                    // 渲染数据
                                    _this.renderdata(_this, data);
                                    _this.operation(obj);
                                })
                            }
                        });
                }else{
                    $('#pageContainer').html('<img src="/ImagesNew/no_data.png" style="display: block;margin: 70px auto;">');
                }
            })

        },
        // 渲染数据
        renderdata: function (_this, data) {

           $('.DispenseList').html(ejs.render($('#Dispense-List').html(), data));
            
            //合并单元格
           if(config.TotalCount != 0) {
                config.List.forEach(function(every,i){
                    var len = every.Item.length;
                    every.Item.forEach(function(item,j){
                        if(j == 0){
                            var str =   '<td>'+ item.ConentTypeName +'：'+ item.Title +'</td>'+
                                        '<td>'+ item.ArticleTypeName +'</td>'+
                                        '<td class="PV">'+ item.PV +'</td>'+
                                        '<td class="UV">'+ item.UV +'</td>'+
                                        '<td>'+ item.ClickPV +'</td>'+
                                        '<td>'+ item.ClickUV +'</td>'+
                                        '<td>'+ item.ReadNumber +'</td>'+
                                        '<td class="LikeNumber">'+ item.LikeNumber +'</td>'+
                                        '<td class="ForwardNumber">'+ item.ForwardNumber +'</td>';

                            $('.RowSpan').eq(i).after(str);
                        }
                        if(item.FootStatistics){
                            len += item.FootStatistics.length;
                        }else{
                            len += 1;
                        }
                    })
                    $('.RowSpan').eq(i).attr('rowspan',len);
                })
           }
           
           if(DistributeType == 73001){//全网域
                $('.PV').hide();
                $('.UV').hide();
                $('.LikeNumber').hide();
                $('.ForwardNumber').hide();
           }

           //所有数据为-1的转换为---
            $('.ad_table td').each(function(){
                var that = $(this);
                if(that.text() == -1){
                    that.text('—');
                }
            })
        },
        // 操作
        operation: function (obj) {

        	var _this = this;
        	obj.BusinessType = 3;

           //下载报表
           $('.downloading').off('click').on('click',function(){
                setAjax({
                    url: xyConfig.url.Export,
                    type: 'get',
                    data: obj
                }, function (data) {
                    var Result = data.Result;
                    if(data.Status == 0){
                        window.open(Result.Url);
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                })
           })
        }
    }


    var wechatnews = new NewDespense();

})


function getthedate(dd,dadd){
	//可以加上错误处理
	var a = new Date(dd)
	a = a.valueOf()
	a = a + dadd * 24 * 60 * 60 * 1000
	a = new Date(a);
	var m = a.getMonth() + 1;
	if(m.toString().length == 1){
	    m='0'+m;
	}
	var d = a.getDate();
	if(d.toString().length == 1){
	    d='0'+d;
	}
	return a.getFullYear() + "-" + m + "-" + d;
}

//获取url 地址参数方法
function GetQueryString(name) {
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if(r!=null)return r[2]; return null;
}


/* 2.将当前时间格式变为2017-04-21*/
Date.prototype.format = function(fmt) {
    var o = {
        "M+" : this.getMonth()+1,                 //月份
        "d+" : this.getDate(),                    //日
        "h+" : this.getHours(),                   //小时
        "m+" : this.getMinutes(),                 //分
        "s+" : this.getSeconds(),                 //秒
        "q+" : Math.floor((this.getMonth()+3)/3), //季度
        "S"  : this.getMilliseconds()             //毫秒
    };
    if(/(y+)/.test(fmt)) {
        fmt=fmt.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));
    }
    for(var k in o) {
        if(new RegExp("("+ k +")").test(fmt)){
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));
        }
    }
    return fmt;
};
