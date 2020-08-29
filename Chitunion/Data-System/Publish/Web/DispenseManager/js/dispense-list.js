/*
	fengb  9.8
 */


$(function () {

    var obj = {};//查询条件
    var xyConfig = {
    	url : {
    		'GetDistributeList' : public_url + '/api/Materiel/GetDistributeList',
            'Export' : public_url + '/api/Materiel/Export'
    	},
	    Pub_url: {
	        "QueryBrand" : public_url + '/api/CarSerial/QueryBrand',
	        "QuerySerialList" : public_url + '/api/CarSerial/QuerySerialList',
	        "GetDictInfoByTypeId": public_url + '/api/DictInfo/GetDictInfoByTypeId',
            'GetBaseLable' : public_url + '/api/DictInfo/GetBaseLable?typeId=6001',
            'GetChildrenLable' : public_url + '/api/DictInfo/GetChildrenLable',
            'GetSceneList_jsonp' : 'http://op.chitunion.com/api/JSONP/GetSceneList_jsonp'//场景跨域jsonp处理
	    }
	}

 
    function NewDespense() {

        //默认设置快捷选项的  开始结束属性
        //var Yesterday = getthedate(new Date(),-1);
        //$('#DispenseTime option').eq(0).attr('StartDate',Yesterday);
        //$('#DispenseTime option').eq(0).attr('EndDate',Yesterday);
        //最近7天
        var LastSevenStart = getthedate(new Date(),-7);
        var LastSevenEnd = getthedate(new Date(),-1);
        $('#DispenseTime option').eq(0).attr('StartDate',LastSevenStart);
        $('#DispenseTime option').eq(0).attr('EndDate',LastSevenEnd);
        //最近30天
        var LastStart = getthedate(new Date(),-30);
        var LastEnd = getthedate(new Date(),-1);
        $('#DispenseTime option').eq(1).attr('StartDate',LastStart);
        $('#DispenseTime option').eq(1).attr('EndDate',LastEnd);

    	this.getBasicType();//获取枚举
    	this.selectMotorcycle();//车型
        this.queryparameters();//查询

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
                    }else{
                        $('#DispenseTime .selected').removeClass('selected');
                    }
                }
            });
        });
        $('#EndDate').off('click').on('click', function () {
            var LastTirty = getthedate($('#StartDate').val(),90);
            laydate({
                fixed: false,
                elem: '#EndDate',
                voidDateRange : voidDateRange,
                choose: function (date) {
                    if (date < $('#StartDate').val() && $('#StartDate').val()) {
                        layer.msg('结束时间不能小于起始时间！',{'time':1000});
                        $('#EndDate').val('');
                    }else if(date > LastTirty){
                        layer.msg('最多支持查询90天的数据哦!',{'time':1000});
                        $('#EndDate').val('');
                    }else{
                        $('#DispenseTime .selected').removeClass('selected');
                    }
                }
            });
        });

        //模拟 ul
        $('.SimulationUl li').on('mouseover',function(){
            var that = $(this);
            that.addClass('selected').siblings().removeClass('selected');
        }).off('click').on('click',function(){
            var that = $(this);
            var val = that.text();
            that.parents('li').find('input').val(val);
            that.parent('.SimulationUl').hide();
        })

        //点击三角的时候  模拟UI出来 选中的时候消失
        $('.Choose_Ul').off('click').on('click',function(event){
            event.stopPropagation();
            var that = $(this); 
            that.next('.SimulationUl').show();
        })

        $(document.body).not('#AssembleUser').on('click',function(){
            $('.SimulationUl').hide();
        })
    }

    

    NewDespense.prototype = {
        constructor: NewDespense,
        // 获取查询参数
        queryparameters: function () {
            var _this = this;
           
            // 搜索
            $('#searchBtn').off('click').on('click', function () {
               //开始时间		
               var StartDate = $('#StartDate').val();
               //结束时间
               var EndDate = $('#EndDate').val();

               if(StartDate == '' && EndDate == ''){
                    StartDate = $('#DispenseTime option:checked').attr('StartDate');
                    EndDate = $('#DispenseTime option:checked').attr('EndDate');
               }

               //业务类型
               var DistributeType = $('#DistributeType option:checked').attr('DistributeType');
               //组装操作人
               var AssembleUser = $.trim($('#AssembleUser').val());
               //分发操作人
               var DistributeUser = $.trim($('#DistributeUser').val());
               if(DistributeUser == '经纪人'){
                    DistributeUser = 'QingNiaoAgent';
               }

               //标题 URL模糊搜索
               var MaterielName = $.trim($('#MaterielName').val());
               var CarSerialId = $('.Models option:checked').attr('CarSerialId')*1;//车型ID
               var BrandId = $('.series option:checked').attr('BrandId')*1;//子品牌ID

               //车型模糊搜索
               var CarSerialName = $.trim($('#CarSerialName').val());
               //渠道ID
               var ChannelId = $('#ChannelId option:checked').attr('ChannelId');
               //IP
               var Ip = $('#IP option:checked').attr('TitleId');
               //子IP
               var ChildIp = $('#ChildIp option:checked').attr('ChildIp');
                
                //针对下载数据的查询条件
                var ExpChannelName = $('#ChannelId option:checked').text();
                var brandName = $('.brand option:checked').text();
                var seriesName = $('.series option:checked').text();
                var ModelsName =  $('.Models option:checked').text();
                var ExpIpName = $('#IP option:checked').text();
                var ExpChildIpName = $('#ChildIp option:checked').text();

                if(ExpChannelName == '请选择'){
                    ExpChannelName = '--';
                }
                if(brandName == '请选择品牌'){
                    brandName = '--';
                }
                if(seriesName == '请选择子品牌'){
                    seriesName = '--';
                }
                if(ModelsName == '请选择车型'){
                    ModelsName = '--';
                }
                if(ExpIpName == '请选择IP'){
                    ExpIpName = '--';
                }
                if(ExpChildIpName == '请选择子IP'){
                    ExpChildIpName = '--';
                }

                var ExpCarSerialName = brandName + ',' + seriesName + ',' + ModelsName + ',' + CarSerialName;
                
                //对象
                obj = {
                    PageSize : 20,
                    PageIndex : 1,
                    StartDate : StartDate,
                    EndDate : EndDate,
                    DistributeType : DistributeType,
                    AssembleUser : AssembleUser,
                    DistributeUser : DistributeUser,
                    MaterielName : MaterielName,
                    CarSerialId : CarSerialId,
                    BrandId : BrandId,
                    CarSerialName : CarSerialName,
                    ChannelId : ChannelId,
                    Ip : Ip,
                    ChildIp : ChildIp,
                    ExpChannelName : ExpChannelName,
                    ExpCarSerialName : ExpCarSerialName,
                    ExpIpName : ExpIpName,
                    ExpChildIpName : ExpChildIpName
                }
                _this.requestdata(obj);


                // 发布日期的顺序排列
                $('.HeadOrderBy a').off('click').on('click',function(){
                    var that = $(this);
                    var _class = that.attr('tit');
                    var img = that.find('img').attr('src');
                    that.addClass('yellow');
                    that.parents('th').siblings().find('a').removeClass('yellow');
                    that.parents('th').siblings().find('img').attr('src','/ImagesNew/icon16_c.png');
                    
                    if(img && img == '/ImagesNew/icon16_c.png'){//默认的图片
                        that.find('img').attr('src','/ImagesNew/icon16_a.png');//升序
                        if(_class == 'PV'){
                            obj.OrderBy = 1001;
                        }else if(_class == 'UV'){
                            obj.OrderBy = 1011;
                        }else if(_class == 'OnLineAvgTimeFormt'){
                            obj.OrderBy = 1021;
                        }else if(_class == 'JumpProportion'){
                            obj.OrderBy = 1031;
                        }else if(_class == 'BrowsePageAvg'){
                            obj.OrderBy = 1041;
                        }else if(_class == 'InquiryNumber'){
                            obj.OrderBy = 1051;
                        }else if(_class == 'SessionNumber'){
                            obj.OrderBy = 1061;
                        }else if(_class == 'TelConnectNumber'){
                            obj.OrderBy = 1071;
                        }else if(_class == 'ForwardNumber'){
                            obj.OrderBy = 1081;
                        }
                        _this.requestdata(obj);
                    }else if(img && img=='/ImagesNew/icon16_a.png'){
                        that.find('img').attr('src','/ImagesNew/icon16_b.png');//降序
                        if(_class == 'PV'){
                            obj.OrderBy = 1002;
                        }else if(_class == 'UV'){
                            obj.OrderBy = 1012;
                        }else if(_class == 'OnLineAvgTimeFormt'){
                            obj.OrderBy = 1022;
                        }else if(_class == 'JumpProportion'){
                            obj.OrderBy = 1032;
                        }else if(_class == 'BrowsePageAvg'){
                            obj.OrderBy = 1042;
                        }else if(_class == 'InquiryNumber'){
                            obj.OrderBy = 1052;
                        }else if(_class == 'SessionNumber'){
                            obj.OrderBy = 1062;
                        }else if(_class == 'TelConnectNumber'){
                            obj.OrderBy = 1072;
                        }else if(_class == 'ForwardNumber'){
                            obj.OrderBy = 1082;
                        }
                        _this.requestdata(obj);
                    }else if(img && img=='/ImagesNew/icon16_b.png'){
                        that.find('img').attr('src','/ImagesNew/icon16_a.png');
                        if(_class == 'PV'){
                            obj.OrderBy = 1001;
                        }else if(_class == 'UV'){
                            obj.OrderBy = 1011;
                        }else if(_class == 'OnLineAvgTimeFormt'){
                            obj.OrderBy = 1021;
                        }else if(_class == 'JumpProportion'){
                            obj.OrderBy = 1031;
                        }else if(_class == 'BrowsePageAvg'){
                            obj.OrderBy = 1041;
                        }else if(_class == 'InquiryNumber'){
                            obj.OrderBy = 1051;
                        }else if(_class == 'SessionNumber'){
                            obj.OrderBy = 1061;
                        }else if(_class == 'TelConnectNumber'){
                            obj.OrderBy = 1071;
                        }else if(_class == 'ForwardNumber'){
                            obj.OrderBy = 1081;
                        }
                       _this.requestdata(obj);
                    }
                });

            })
        },
        // 请求数据
        requestdata: function (obj) {
            $('.DispenseList').html('');
            $('#pageContainer').html('');
            var _this = this;
            $.ajax({
                url: xyConfig.url.GetDistributeList,
                type: 'get',
                data: obj,
                beforeSend: function(){
                    $('#listLoading').html('<img src="/ImagesNew/icon_loading.gif" style="display: block;margin: 70px auto;">');
                },
                success : function (data) {
                    $('#listLoading').html('');
                    var Result = data.Result;
                    _this.renderdata(_this, data, obj);
                    
                    if(data.Result.TotalCount > 0) {
                        _this.createPageController(_this, obj, data.Result);
                    }else{
                        $('#pageContainer').html('<img src="/ImagesNew/no_data.png" style="display: block;margin: 70px auto;">');
                    }
                }
            })
        },
        createPageController : function(_this, obj, Result){
            var counts = Result.TotalCount;
            $("#pageContainer").pagination(counts, {
                current_page: (obj.PageIndex ? obj.PageIndex : 1),
                items_per_page: 20, 
                callback: function (currPage) {
                    var obj1 = obj;
                    obj1.PageIndex = currPage;
                    _this.requestdata(obj1);
                    $('.DispenseList').html('');
                    $('#pageContainer').html('');
                }
            });
        },
        // 渲染数据
        renderdata: function (_this, data,obj) {  

           $('.DispenseList').html(ejs.render($('#Dispense-List').html(), data));
           var ConfigData = data.Result;
           if(ConfigData.TotalCount != 0){
                $('.TotalBar .TotalMateriel').text(ConfigData.Extend.TotalMateriel);
                $('.TotalBar .InquiryNumber').text(ConfigData.Extend.TotalInquiryNumber);
                $('.TotalBar .SessionNumber').text(ConfigData.Extend.TotalSessionNumber);
                $('.TotalBar .TelConnectNumber').text(ConfigData.Extend.TotalTelConnectNumber);
                $('.TotalBar .TotalDistribute').text(ConfigData.Extend.TotalDistribute);
                $('.TotalBar .TotalForwardNumber').text(ConfigData.Extend.TotalForwardNumber);
                $('.TotalBar .TotalClue').text(ConfigData.Extend.TotalClue);
           }else{
                $('.TotalBar .TotalMateriel').text(0);
                $('.TotalBar .InquiryNumber').text(0);
                $('.TotalBar .SessionNumber').text(0);
                $('.TotalBar .TelConnectNumber').text(0);
                $('.TotalBar .TotalDistribute').text(0);
                $('.TotalBar .TotalForwardNumber').text(0);
                $('.TotalBar .TotalClue').text(0);
           }
           _this.operation(obj);
        },
        // 操作
        operation: function (obj) {

        	var _this = this;
        	obj.BusinessType = 1;

            //所有数据为-1的转换为---
            $('.ad_table td').each(function(){
                var that = $(this);
                if(that.text() == -1){
                    that.text('—');
                }
            })

        	//分发明细
        	$('.ad_table .DispenseList tr').off('click').on('click',function(){
        		var that = $(this);
        		var MaterielId = that.attr('MaterielId');
                var DistributeType = that.attr('DistributeType');
        		
        		window.open('/DispenseManager/EveryData.html?DistributeType='+ DistributeType +'&MaterielId='+MaterielId);
        	}).on('mouseover',function(){
        		var that = $(this);
        		that.css({'cursor':'pointer'});
        	})


           //下载报表
           $('.downloading').off('click').on('click',function(){
                $.ajax({
                    url: xyConfig.url.Export,
                    type: 'get',
                    data: obj,
                    beforeSend: function(){
                        layer.msg('获取中请稍后', {
                            icon: 16,
                            shade: 0.01
                        })
                    },
                    success : function(data){
                        var Result = data.Result;
                        if(data.Status == 0){
                            window.open(Result.Url);
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    }
                })
           })

        },
        selectMotorcycle : function(){//查询车型   和IP子IP
            $.ajax({
                url:xyConfig.Pub_url.QueryBrand,
                type:'get',
                async: false,
                dataType: 'json',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                success:function(data){
                    if(data.Status == 0){
                        var str = '<option MasterId="-2">请选择品牌</option>';
                        for(var i=0;i<data.Result.length;i++){
                            str += '<option MasterId='+data.Result[i].MasterId+'>'+data.Result[i].Name+'</option>'
                        }
                        $('.brand').html(str);

                        $('.brand').off('change').on('change',function(){
                            $('.brands').find('.hidden_tip').hide();
                            var MasterBrandId = $(this).find('option:checked').attr('MasterId');
                            if(MasterBrandId == -2){
                                $('.series').html('<option BrandId="-2">请选择子品牌</option>');
                                $('.Models').html('<option CarSerialId="-2">请选择车型</option>');
                            }else{
                                $.ajax({
                                    url:xyConfig.Pub_url.QueryBrand,
                                    type:'get',
                                    async: false,
                                    dataType: 'json',
                                    xhrFields: {
                                        withCredentials: true
                                    },
                                    crossDomain: true,
                                    data: {
                                        MasterBrandId:MasterBrandId
                                    },
                                    success:function(data){
                                        if(data.Status == 0){
                                            var str1 = '<option BrandId="-2">请选择子品牌</option>';
                                            for(var j=0;j<data.Result.length;j++){
                                                str1 += '<option BrandId='+data.Result[j].BrandId+'>'+data.Result[j].Name+'</option>'
                                            }
                                            $('.series').html(str1);
                                            $('.Models').html('<option MasterId="-2">请选择车型</option>');
                                            $('.series').off('change').on('change',function(){
                                                $('.brands').find('.hidden_tip').hide();
                                                var BrandId = $(this).find('option:checked').attr('BrandId');
                                                if(BrandId == -2){
                                                    $('.Models').html('<option MasterId="-2">请选择车型</option>');
                                                }else{
                                                    $.ajax({
                                                        url:xyConfig.Pub_url.QuerySerialList,
                                                        type:'get',
                                                        async: false,
                                                        dataType: 'json',
                                                        xhrFields: {
                                                            withCredentials: true
                                                        },
                                                        crossDomain: true,
                                                        data: {
                                                            BrandId:BrandId
                                                        },
                                                        success:function(data){
                                                            var str2 = '<option CarSerialId="-2">请选择车型</option>';
                                                            for(var k=0;k<data.Result.length;k++){
                                                                str2 += '<option BrandId='+data.Result[k].BrandId+' CarSerialId='+data.Result[k].CarSerialId+'>'+data.Result[k].ShowName+'</option>'
                                                            }
                                                            $('.Models').html(str2);
                                                            $('.Models').off('change').on('change',function(){
                                                                $('.brands').find('.hidden_tip').hide();
                                                            })
                                                        }
                                                    })
                                                }
                                            })
                                        }
                                    }
                                });
                            }
                        })
                    }
                }
            });


            //查询IP  子IP
            $.ajax({
                url:xyConfig.Pub_url.GetBaseLable,
                type:'get',
                async: false,
                dataType: 'json',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                success:function(data){
                    if(data.Status == 0){
                        var str = '<option TitleId="-2">请选择IP</option>';
                        for(var i=0;i<data.Result.length;i++){
                            str += '<option TitleId='+data.Result[i].TitleId+'>'+data.Result[i].Name+'</option>'
                        }
                        $('#IP').html(str);

                        $('#IP').off('change').on('change',function(){
                            var TitleId = $(this).find('option:checked').attr('TitleId');
                            if(TitleId == -2){
                                $('#IP').html('<option TitleId="-2">请选择IP</option>');
                                $('#ChildIp').html('<option ChildIp="-2">请选择子IP</option>');
                            }else{
                                $.ajax({
                                    url:xyConfig.Pub_url.GetChildrenLable,
                                    type:'get',
                                    async: false,
                                    dataType: 'json',
                                    xhrFields: {
                                        withCredentials: true
                                    },
                                    crossDomain: true,
                                    data: {
                                        titleId:TitleId
                                    },
                                    success:function(data){
                                        if(data.Status == 0){
                                            var str = '<option ChildIp="-2">请选择子IP</option>';
                                            for(var i=0;i<data.Result.length;i++){
                                                str += '<option ChildIp='+data.Result[i].TitleId+'>'+data.Result[i].Name+'</option>'
                                            }
                                            $('#ChildIp').html(str);
                                        }
                                    }
                                });
                            }
                        })
                    }
                }
            });
        },
        getBasicType : function(){//获取基本的枚举信息
        	//场景/渠道
        	setAjax({
        		url : xyConfig.Pub_url.GetSceneList_jsonp,
        		type : 'get',
        		dataType:'jsonp',  
                jsonp:'callback'
        	},function(data){
                var Result = JSON.parse(data);
    			var str = '<option ChannelId="-2">请选择</option>';
                for(var i=0;i<Result.length;i++){
                    str += '<option ChannelId='+Result[i].SceneID+'>'+Result[i].SceneName+'</option>'
                }
                $('#ChannelId').html(str);
        	});

        }
    }

    var wechatnews = new NewDespense();
})

/* 1.获取  昨天  最近七天  最近三十天*/
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
