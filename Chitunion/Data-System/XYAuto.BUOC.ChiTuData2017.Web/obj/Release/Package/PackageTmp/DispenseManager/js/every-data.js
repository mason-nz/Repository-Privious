/*
	fengb  9.8
 */


$(function () {

    var DistributeType = GetQueryString('DistributeType')!=null&&GetQueryString('DistributeType')!='undefined'?GetQueryString('DistributeType'):null;
    var MaterielId = GetQueryString('MaterielId')!=null&&GetQueryString('MaterielId')!='undefined'?GetQueryString('MaterielId'):null;
    
    var xyConfig = {
    	url : {
    		'GetDailyQuery' : public_url + '/api/Materiel/GetDailyQuery',
            'GetInfo' : public_url + '/api/Materiel/GetInfo',
            'Export' : public_url + '/api/Materiel/Export'
    	},
	    Pub_url: {
	        "QueryBrand" : public_url + '/api/Materiel/GetDailyQuery',
	        "QuerySerialList" : public_url + '/api/CarSerial/QuerySerialList',
	        "GetDictInfoByTypeId": public_url + '/api/DictInfo/GetDictInfoByTypeId'
	    }
	}
 

    function NewDespense() {
        this.geteverydataBase();
        this.queryparameters();

        $('#searchBtn').click();

        var voidDateRange = [];
        var timeObj = {
            'S': new Date().format('yyyy-MM-dd'),
            'E': '2020-09-15'
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
                    MaterielId : MaterielId
                }
                _this.requestdata(obj);
            })
        },
        // 请求数据
        requestdata: function (obj) {
        	
            var _this = this;
            setAjax({
                url: xyConfig.url.GetDailyQuery,
                type: 'get',
                data: obj,
            }, function (data) {

                var Result = data.Result;
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
                                    url: xyConfig.url.GetDailyQuery,
                                    type: 'get',
                                    data: obj
                                }, function (data) {
                                    var Result = data.Result;
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
           //wechatnews.triggleChannel();

        },
        // 操作
        operation: function (obj) {

        	var _this = this;
        	obj.BusinessType = 2;

        	//分发明细
        	$('.ad_table .DispenseList>tr:even').off('click').on('click',function(){
        		var that = $(this);
        		var MaterielId = that.attr('MaterielId');
                var DistributeId = that.attr('DistributeId');
                var CurDate = that.attr('Date');

                window.open('/DispenseManager/MaterialDatail.html?DistributeType='+ DistributeType +'&MaterielId='+MaterielId + '&DistributeId='+DistributeId);
        	}).on('mouseover',function(){
        		var that = $(this);
        		that.css({'cursor':'pointer'});
        	})


           //下载报表   二次改版的时候不区分日报表和渠道信息
           $('.downloading').off('click').on('click',function(){
                /*if(DistributeType == 73001){//全网域  区分日报表和渠道信息

                    var _width = document.documentElement.clientWidth;
                    var _height = document.documentElement.clientHeight;
                    $('.channel_box').show();
                    $('.channel_box .layer').show();
                    var layer_height = $('.channel_box .layer').height();
                    var _left = (_width-256)/2;
                    var _top = (_height-layer_height)/2;
                    $('.channel_box').css({'width':_width,'height':_height,'position':'fixed','left':0,'top':0,'background':'rgba(0,0,0,0.7)','zIndex':1000});
                    $('.channel_box .layer').css({'position':'absolute','left':_left,'top':_top});

                    //关闭
                    $('#closebt1').off('click').on('click',function(){
                        $('.channel_box').hide();
                        $('.channel_box .layer').hide();
                    })
                    //取消
                    $('#cancleMessage').off('click').on('click',function(){
                        $('.channel_box').hide();
                        $('.channel_box .layer').hide();
                    })

                    //确定
                    $('#alredayChoose').off('click').on('click',function(){
                        var that = $(this);
                        var isChecked = that.parents('.layer_con').find('input[name=theReason]').is(':checked');
                        if(!isChecked){
                            layer.msg('请选择',{'time':1000});
                        }else{
                            var ExportType = $('.layer_con').find('input[name=theReason]:checked').attr('ExportType');
                            obj.ExportType = ExportType;
                            setAjax({
                                url: xyConfig.url.Export,
                                type: 'get',
                                data: obj
                            }, function (data) {
                                var Result = data.Result;
                                if(data.Status == 0){
                                    window.location = Result.Url;
                                    $('.channel_box').hide();
                                    $('.channel_box .layer').hide();
                                    $('.layer_con').find('input[name=theReason]:checked').attr('checked',false);
                                }else{
                                    layer.msg(data.Message,{'time':1000});
                                }
                            })
                        }
                    })
                }else if(DistributeType == 73002){//经纪人*/
                    obj.ExportType = 2;
                    setAjax({
                        url: xyConfig.url.Export,
                        type: 'get',
                        data: obj
                    }, function (data) {
                        var Result = data.Result;
                        if(data.Status == 0){
                            window.location = Result.Url;
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })
                //}
           })


           //所有数据为-1的转换为---
            $('.ad_table td').each(function(){
                var that = $(this);
                if(that.text() == -1){
                    that.text('—');
                }
            })

        },
        //根据角色显示渠道  操作渠道
        triggleChannel : function(){

            if(DistributeType == 73001){//全网域
                $('.increase_add').off('click').on('click',function(event){
                    event.stopPropagation();
                    var that = $(this);
                    var tit = that.attr('triTit');
                    if(tit == 'on'){
                        that.parents('tr').next('tr').show();
                        that.attr('triTit','off');
                        that.find('img').attr('src','/ImagesNew/reduce.png');
                    }else if(tit == 'off'){
                        that.parents('tr').next('tr').hide();
                        that.attr('triTit','on');
                        that.find('img').attr('src','/ImagesNew/addCh.png');
                    }
                })
            }else if(DistributeType == 73002){//经纪人
                $('.increase_add').hide();
            }

            

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


/* 3.将当前时间格式变为2017-04-21*/
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
