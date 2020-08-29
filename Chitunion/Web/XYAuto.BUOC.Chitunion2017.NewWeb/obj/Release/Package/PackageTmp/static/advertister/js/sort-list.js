/*
* Written by:     fengb
* function:       微信列表
* Created Date:   2017-12-19
*/

$(function () {

    //type类型 默认为微信
    var type = GetQueryString('type')!=null&&GetQueryString('type')!='undefined'?GetQueryString('type'):0;
    //关键字 默认为空
    var keyword = GetQueryString('keyword')!=null&&GetQueryString('keyword')!='undefined'?GetQueryString('keyword'):'';
    keyword = decodeURI(keyword);

    var ListType = ['wx_list','wb_list','app_list'];
    var obj = {
        ListType: ListType[type],
        keyword: keyword,
        PageIndex: 1,
        PageSize: 20,
        CategoryID: -1,
        MaxFansCount: -1,
        MinFansCount: -1,
        MaxPrice: -1,
        MinPrice: -1,
        ProvinceID: -1,
        CityID: -1,
        ADPositionID: -1,
        ReferenceType: -1,
        SortField: -1,
        SortIndex: -1,
        r: Math.random()
    }


    console.log(CTLogin);//返回登陆Json对象

    function WxList() {
        this.Condition();//条件
    	this.InitMaterial();//初始化

        $('#search-input').val(keyword);
        $('#MediaChange li').eq(type).click();
    }
   
    WxList.prototype = {
        constructor: WxList,
        InitMaterial : function(){//切换媒体类型
            var _this = this;

            $('#MediaChange li').off('click').on('click',function(){
                var that = $(this);
                var idx = that.index();
                var _img = that.find('img');
                that.addClass('selected').siblings().removeClass('selected');
                _this.InitNoSort();
                if(idx == 0){//微信
                    $('#City').parents('.wx_wrap').show();
                    $('#FansCount').parents('.wx_wrap').show();
                    $('#Price').parents('.wx_wrap').show();
                    $('#ADPositionID').show();
                    $('#ReferenceType').hide();
                    _img.attr('src','/images/icons/wx.png');
                    $('#MediaChange li').eq(1).find('img').attr('src','/images/icons/wb1.png');
                    $('#MediaChange li').eq(2).find('img').attr('src','/images/icons/app1.png');
                }else if(idx == 1){//微博
                    $('#City').parents('.wx_wrap').hide();
                    $('#FansCount').parents('.wx_wrap').show();
                    $('#Price').parents('.wx_wrap').show();
                    $('#ADPositionID').hide();
                    $('#ReferenceType').show();
                    _img.attr('src','/images/icons/wb.png');
                    $('#MediaChange li').eq(0).find('img').attr('src','/images/icons/wx1.png');
                    $('#MediaChange li').eq(2).find('img').attr('src','/images/icons/app1.png');
                }else if(idx == 2){//app
                    $('#City').parents('.wx_wrap').hide();
                    $('#FansCount').parents('.wx_wrap').hide();
                    $('#Price').parents('.wx_wrap').hide();
                    _img.attr('src','/images/icons/app.png');
                    $('#MediaChange li').eq(0).find('img').attr('src','/images/icons/wx1.png');
                    $('#MediaChange li').eq(1).find('img').attr('src','/images/icons/wb1.png');
                }
                _this.InitParams(idx);
            })  
        },
        InitParams: function (idx) {//参数
            var _this = this;
            //常见分类
            _this.CommonSort (idx);
            //展开购物车
            _this.LauchShopCar();
            //初始化不限
            _this.InitNoSort();

            //搜索
            $('#submit').off('click').on('click',function(){
                keyword = $.trim($('#search-input').val());//关键字
                CategoryID = $('#CategoryID .active').attr('DictId')*1;//行业分类
                MaxFansCount = $('#FansCount .active').attr('maxId')*1;//最大粉丝数
                MinFansCount = $('#FansCount .active').attr('minId')*1;//最小粉丝数
                MaxPrice = $('#Price .active').attr('maxId')*1;//最大价格
                MinPrice = $('#Price .active').attr('minId')*1;//最小价格
                ProvinceID = $('#City .active').attr('ProvinceID')*1;//省份ID
                CityID = $('#City .active').attr('CityID')*1;//城市ID

                if(keyword != ''){
                    obj = {
                        ListType : ListType[idx],
                        keyword : keyword,
                        PageIndex : 1,
                        PageSize : 20,
                        CategoryID : -1,
                        MaxFansCount : -1,
                        MinFansCount : -1,
                        MaxPrice : -1,
                        MinPrice : -1,
                        ProvinceID : -1,
                        CityID : -1,
                        ADPositionID : -1,
                        ReferenceType : -1,
                        SortField : -1,
                        SortIndex : -1
                    }
                    var str = '';
                    str += '<li order="0">关键字：<span>' + keyword +'</span><i class="close">×</i></li>';
                    str += '<a class="delete_all">清空条件</a>';
                    $('#CheckedOption').html(str);
                    _this.DeleteSingleOption();//删除条件加载数据
                    _this.InitNoSort();
                    $('.wx_already_wrap').show();
                }else{
                    obj = {
                        ListType : ListType[idx],
                        keyword : '',
                        PageIndex : 1,
                        PageSize : 20,
                        CategoryID : CategoryID,
                        MaxFansCount : MaxFansCount,
                        MinFansCount : MinFansCount,
                        MaxPrice : MaxPrice,
                        MinPrice : MinPrice,
                        ProvinceID : ProvinceID,
                        CityID : CityID,
                        ADPositionID : -1,
                        ReferenceType : -1,
                        SortField : -1,
                        SortIndex : -1
                    }
                    $('.wx_already_wrap').hide();
                    $('#CheckedOption').html('');
                }
                _this.RequestParam(obj);//请求参数
                //条件切换样式更改
                _this.ClickSortOptions('.ChangeOptions',idx);
            })
            $('#submit').click();
            
            //输入回车
            $('#search-input').on('keypress',function(event){
                if(event.keyCode == "13") {//keyCode=13是回车键
                    $('#submit').click();
                }                
            })

            $('input[name=fan]').eq(0).on('keypress',function(event){
                if(event.keyCode == "13") {//keyCode=13是回车键
                    $('#FanSure').click();
                }  
            })
            $('input[name=fan]').eq(1).on('keypress',function(event){
                if(event.keyCode == "13") {//keyCode=13是回车键
                    $('#FanSure').click();
                }  
            })
            
            //粉丝数量确定
            $('#FanSure').off('click').on('click',function(){
                var that = $(this);
                var par = that.parents('.price-set');
                var input1 = par.find('input').eq(0).val() * 1;
                var input2 = par.find('input').eq(1).val() * 1;

                var sort = that.attr('sort')*1;
                var _html = $('#CheckedOption').html();
                var input_name = '';
                var put1 = par.find('input').eq(0).val() * 1;
                var put2 = par.find('input').eq(1).val() * 1;
               
                if(input1 == 0 && input2 == 0){//都为0
                    layer.msg('请输入合法的粉丝数量',{'time':2000});
                    return;
                }else{
                    if(input1 == 0){//第一个为空
                        input1 = -1;
                        input_name = '<=' + put2;
                    }else if(input2 == 0){//第二个为空
                        input2 = -1;
                        input_name = '>=' + put1;
                    }else{
                        if(input1 > input2){
                            layer.msg('请输入合法的粉丝数量',{'time':2000});
                            return;
                        }else{
                            input_name = put1 + '-' + put2;
                        }
                    }
                    $('.wx_already_wrap').show();
                    $('#FansCount').attr('minId',input1);
                    $('#FansCount').attr('maxId',input2);
                    $('#FansCount li').find('a').removeClass('active');

                    _this.BlueSureBtn();
                }
              
                obj.MinFansCount = input1;
                obj.MaxFansCount = input2;
                _this.RequestParam(obj);
            })

            
            $('input[name=price]').eq(0).on('keypress',function(event){
                if(event.keyCode == "13") {//keyCode=13是回车键
                    $('#PriceSure').click();
                }  
            })
            $('input[name=price]').eq(1).on('keypress',function(event){
                if(event.keyCode == "13") {//keyCode=13是回车键
                    $('#PriceSure').click();
                }  
            })

            //价格确定
            $('#PriceSure').off('click').on('click',function(){
                var that = $(this);
                var par = that.parents('.price-set');
                var input1 = par.find('input').eq(0).val() * 1;
                var input2 = par.find('input').eq(1).val() * 1;
                var sort = that.attr('sort')*1;
                var ADPositionID = $('#ADPositionID option:checked').attr('DictId');//图文ID
                var ReferenceType = $('#ReferenceType option:checked').attr('DictId');//直发 转发

                var _html = $('#CheckedOption').html();
                var input_name = '';
                var put1 = par.find('input').eq(0).val() * 1;
                var put2 = par.find('input').eq(1).val() * 1;

                if(input1 == 0 && input2 == 0){//都为0
                    layer.msg('请输入合法的价格',{'time':2000});
                    return;
                }else{
                    if(input1 == 0){//第一个为空
                        input1 = -1;
                        input_name = '<=' + put2;
                    }else if(input2 == 0){//第二个为空
                        input2 = -1;
                        input_name = '>=' + put1;
                    }else{
                        if(input1 > input2){
                            layer.msg('请输入合法的价格',{'time':2000});
                            return;
                        }else{
                            input_name = put1 + '-' + put2;
                        }
                    }

                    $('.wx_already_wrap').show();
                    $('#Price').attr('minId',input1);
                    $('#Price').attr('maxId',input2);
                    $('#Price li').find('a').removeClass('active');

                    _this.BlueSureBtn();
                }
                obj.MinPrice = input1;
                obj.MaxPrice = input2;
                obj.ADPositionID = ADPositionID;
                obj.ReferenceType = ReferenceType;
                _this.RequestParam(obj);//请求参数
            })

            //区域确定
            $('#CitySure').off('click').on('click',function(){
                var that = $(this);
                var par = that.parents('.price-set');
                var select1 = $('#ProvinceID option:checked').val();
                var select2 = $('#CityID option:checked').val();
                var sort = that.attr('sort')*1;
               
                var name1 = $('#ProvinceID option:checked').text();
                var name2 = $('#CityID option:checked').text();
                
                if(select1 == -1 && select2 == -1){
                    layer.msg('请选择省份城市',{'time':2000});
                    return;
                }else{
                    $('.wx_already_wrap').show();
                    $('#City').attr('ProvinceID',name1);
                    $('#City').attr('CityID',name2);
                    $('#City li').find('a').removeClass('active');
                    _this.BlueSureBtn();
                }
                
                obj.ProvinceID = select1;
                obj.CityID = select2;
                _this.RequestParam(obj);//请求参数
            })
        },
        Condition : function(){//枚举和条件初始化
            var _this = this;

            //常见分类展开
            $('.wx_ext').off('click').on('click',function(){
                var that = $(this);
                var imgSrc = that.find('img').attr('src');
                if(imgSrc == '/images/arrow_01.png'){
                    that.parents('.wx_wrap').css('height','auto');
                    that.html('收起 <img src="/images/arrow_02.png">');
                }else{
                    that.parents('.wx_wrap').css('height','36px');
                    that.html('展开 <img src="/images/arrow_01.png">')
                }
            })
            //输入框只能为正整数
            $('.input').on("input",function(){
                var that = $(this);
                var val = that.val();
                replaceAndSetPos(this,/[^0-9]/g,'');
                if(val <= 0){
                    that.val('');
                }
            })

            //广告位置渲染  
            $.ajax({
                url: public_url + '/api/DictInfo/GetDictInfoByTypeID',
                type: 'get',
                xhrFields: {
                    withCredentials: true
                },
                cache: false, 
                crossDomain: true,
                data: {
                    typeID : 6,
                    r: Math.random()
                },
                success : function (data) {
                    if(data.Status == 0){
                        var Result = data.Result;
                        var str = '';
                        for(var i = 0;i <= Result.length - 1;i ++){
                            str += "<option DictId="+ Result[i].DictId+">"+Result[i].DictName+"</option>";
                        }
                        $('#ADPositionID').html(str);
                    }else{
                        layer.msg(data.Message,{'time':2000});
                    }
                }
            })

            //省份城市下拉框初始化数据
            BindProvince('ProvinceID');
            $('#ProvinceID').off('change').on('change',function(){
                BindCity('ProvinceID','CityID');
            })

            _this.GetCartCount();
        },
        CommonSort : function(idx){//不同媒体类型的常见分类不同
            var _this = this;
            //常见分类 
            var enumeration = [47,91,52];
            //常见分类
            $.ajax({
                url: public_url + '/api/DictInfo/GetDictInfoByTypeID',
                type: 'get',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                data: {
                    typeID: enumeration[idx],
                    r: Math.random() 
                },
                async : false,
                success : function (data) {
                    if(data.Status == 0){
                        var Result = data.Result;
                        var str = '<li><a href="javascript:;" class="active" DictId="-1">不限</a></li>';
                        for(var i = 0;i <= Result.length - 1;i ++){
                            str += "<li><a href='javascript:;' DictId="+ Result[i].DictId+">"+Result[i].DictName+"</a></li>"; 
                        }
                        $('#CategoryID').html(str);
                    }else{
                        layer.msg(data.Message,{'time':2000});
                    }
                }
            })
        },
        RequestParam: function (obj) {//请求参数
            var _this = this;
            $.ajax({
                //url: './json/GetMediaMatchingList.json',
                url :  public_url + '/api/MediaMatching/GetMediaMatchingList',
                type: 'get',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                data: obj,
                beforeSend: function(){
                    $('.ListDetail').html('<img src="/images/loading.gif" style="display: block;margin: 140px auto;">');
                },
                success : function (data) {
                    if(data.Status == 0){
                        $('.ListDetail').html('');
                        var idx = ListType.indexOf(obj.ListType);
                        var ResultArr = ['WeiXinList','WeiBoList','AppList'];
                        var Result = data.Result[ResultArr[idx]];
                        if(Result.TotalCount > 0) {
                            $('.tottom_tips').show();
                            //判断用户是否登录
                            if(CTLogin.IsLogin != true){
                                obj.PageIndex = 1;
                                $('#pageContainer').hide(); 
                                $('.tottom_tips .tit').html('当前最多显示前1页搜索结果，若需更多请<a href="javascript:;" class="getToUs">联络我们</a> ！');
                            }else{
                                //有条件是1页  没有条件是15页
                                if(obj.keyword != '' ||  obj.CategoryID != -1 || obj.MaxFansCount != -1 || obj.MinFansCount != -1 || obj.MaxPrice != -1 || obj.MinPrice != -1 
                                    || obj.ProvinceID != -1 || obj.CityID != -1){
                                    $('#pageContainer').hide();
                                    $('.tottom_tips .tit').html('当前最多显示前1页搜索结果，若需更多请<a href="javascript:;" class="getToUs">联络我们</a> ！');
                                }else{
                                    $('#pageContainer').show();
                                    $('.tottom_tips .tit').html('当前最多显示前15页搜索结果，若需更多请<a href="javascript:;" class="getToUs">联络我们</a> ！');
                                }
                            }
                            _this.Renderdata(ResultArr[idx],Result,Result.TotalCount);//
                            _this.CreatePageController(obj, Result);//分页
                        }else{
                            $('.tottom_tips').hide();
                            $('#pageContainer').hide();
                            $('.ListDetail').html('<img src="/images/no_data.png" style="display: block;margin: 140px auto;">');
                        }
                    }else{
                        layer.msg(data.Message,{'time':2000});
                    }
                }
            })
        },
        CreatePageController : function(obj, Result){
            var _this = this;
            var counts = Result.TotalCount;
            if(counts >= 300){
                counts = 300;
            }
            $("#pageContainer").pagination(counts, {
                current_page: (obj.PageIndex ? obj.PageIndex : 1),
                items_per_page: 20, 
                callback: function (currPage) {
                    var obj1 = obj;
                    obj1.PageIndex = currPage;
                    _this.RequestParam(obj1);
                }
            });
        },
        Renderdata : function(elem,Result,total){//渲染数据
            var _this = this;
            $('.ListDetail').html(ejs.render($('#'+elem).html(), Result));
            _this.GetToShopCarOperation();
            $('.tottom_tips .getToUs').off('click').on('click',function(){
                window.open('/static/info/aboutus.html') ;
            })
            $('.wx_form .wechat_list3 .TotalCount').html(total);
            $('.wx_form .wechat_list4 .TotalCount').html(total);
        },
        ClickSortOptions : function(elem,index){//点击各种条件的一系列动作
            var _this = this;
            $(elem).find('li').off('click').on('click',function(){
                var that = $(this);
                var SortArr = ['关键字','常见分类','粉丝数量','价格','覆盖地区'];
                var TitArr = [];
                var link = that.find('a');
                var ulElem = that.parent('ul').attr('id');
                link.addClass('active').parent('li').siblings('li').find('a').removeClass('active');

                //加载数据
                if(ulElem == 'CategoryID'){//常见分类
                    var CategoryID = $('#'+ulElem).find('.active').attr('DictId')*1;
                    obj.CategoryID = CategoryID;
                    _this.RequestParam(obj);
                }else if(ulElem == 'FansCount'){//粉丝数量                
                    var MaxFansCount = $('#'+ulElem).find('.active').attr('maxId')*1;
                    var MinFansCount = $('#'+ulElem).find('.active').attr('minId')*1;
                    obj.MaxFansCount = MaxFansCount;
                    obj.MinFansCount = MinFansCount;
                    _this.RequestParam(obj);
                    $('#FansCount').attr('minId','-1');
                    $('#FansCount').attr('maxId','-1');
                    $('#FanSure').prev().val('');
                    $('#FanSure').prev().prev().val('');
                }else if(ulElem == 'Price'){//价格
                    var MaxPrice = $('#'+ulElem).find('.active').attr('maxId')*1;
                    var MinPrice = $('#'+ulElem).find('.active').attr('minId')*1;
                    obj.MaxPrice = MaxPrice;
                    obj.MinPrice = MinPrice;
                    _this.RequestParam(obj);
                    $('#Price').attr('minId','-1');
                    $('#Price').attr('maxId','-1');
                    $('#PriceSure').prev().val('');
                    $('#PriceSure').prev().prev().val('');
                }else if(ulElem == 'City'){//覆盖地区
                    var ProvinceID = $('#'+ulElem).find('.active').attr('ProvinceID')*1;    
                    var CityID = $('#'+ulElem).find('.active').attr('CityID')*1;
                    obj.ProvinceID = ProvinceID;
                    obj.CityID = CityID;
                    _this.RequestParam(obj);
                    $('#City').attr('ProvinceID','-1');
                    $('#City').attr('CityID','-1');
                    $('#ProvinceID option:first').prop('selected',true);
                    $('#CityID option:first').prop('selected',true);
                }

                _this.BlueSureBtn();
               _this.DeleteSingleOption();//删除条件

            })
        },
        BlueSureBtn : function(){//点击确定按钮
            var _this = this;

            var SortArr = ['关键字','常见分类','粉丝数量','价格','覆盖地区'];
            var TitArr = [];
            var CategoryTit = $('#CategoryID .active').text();
            var FansTiT = '';
            var PriceTiT = '';
            var CityTit = '';
            var keywords = $.trim($('#search-input').val());


            var minFance = $('#FansCount').attr('minId') * 1;
            var maxFance = $('#FansCount').attr('maxId') * 1;
            var minPrice = $('#Price').attr('minId') * 1;
            var maxPrice = $('#Price').attr('maxId') * 1;

            var ProvinceID = $('#City').attr('ProvinceID');
            var CityID = $('#City').attr('CityID');

            //粉丝
            if(minFance == -1 && maxFance == -1){
                FansTiT = $('#FansCount .active').text();
            }else{
                if(minFance == -1){
                    FansTiT = '<=' + maxFance;
                }else if(maxFance == -1){
                    FansTiT = '>=' + minFance;
                }else{
                    FansTiT = minFance + '-' + maxFance;
                }                  
            }   

            //价格
            if(minPrice == -1 && maxPrice == -1){
                PriceTiT = $('#Price .active').text();
            }else{
                if(minPrice == -1){
                    PriceTiT = '<=' + maxPrice;
                }else if(maxPrice == -1){
                    PriceTiT = '>=' + minPrice;
                }else{
                    PriceTiT = minPrice + '-' + maxPrice;
                }       
            }  


            //区域
            if(ProvinceID == '-1' && CityID == '-1'){
                CityTit = $('#City .active').text();
            }else{
                if(CityID == '请选择城市'){
                    CityTit = ProvinceID;
                }else{
                    CityTit = ProvinceID + '-' + CityID;
                }
            }


            TitArr.push(keywords);
            TitArr.push(CategoryTit);
            TitArr.push(FansTiT);
            TitArr.push(PriceTiT);
            TitArr.push(CityTit);

            var str = '';
            for(var i = 0 ;i<= TitArr.length - 1;i ++ ){
                if(TitArr[i] != '不限'){
                    str += "<li order="+i+">" + SortArr[i]  + "：" + "<span>" + TitArr[i] +"</span><i class='close'>×</i></li>";
                }
            }
            str += '<a class="delete_all">清空条件</a>';
            $('#CheckedOption').html(str);


            if(keywords == ''){//没有关键字的时候  清除关键字这一选项
                $('#CheckedOption li').each(function(){
                    var that = $(this);
                    var order = that.attr('order') * 1;
                    if(order == 0){
                        that.remove();
                    }
                })
            }

            //当全部选择不限的时候 隐藏已选择栏目
            if(isAllEqual(TitArr)){
                $('.wx_already_wrap').hide();
            }else{
                $('.wx_already_wrap').show();
            }

            _this.DeleteSingleOption();//删除条件加载数据

        },
        InitNoSort : function(){//条件初始化为不限
            $('#CategoryID li:eq(0)').find('a').addClass('active').parent('li').siblings('li').find('a').removeClass('active');
            $('#FansCount li:eq(0)').find('a').addClass('active').parent('li').siblings('li').find('a').removeClass('active');
            $('#Price li:eq(0)').find('a').addClass('active').parent('li').siblings('li').find('a').removeClass('active');
            $('#City li:eq(0)').find('a').addClass('active').parent('li').siblings('li').find('a').removeClass('active');
            $('.wx_already_wrap').hide();
        },
        GetToShopCarOperation: function () {//渲染完一系列操作
            var _this = this;
            //二维码
            $('.viewBigpic').off('mouseover').on('mouseover',function(){
                $(this).parent().removeClass('noBigPic');
            }).off('mouseout').on('mouseout',function(){
                $(this).parent().addClass('noBigPic');
            })

            //单选多选
            $('.wx_form input[type=checkbox]').off('click').on('click',function(){
                var that = $(this);
                var type = that.attr('name');
                var single = that.parents('.wx_table2');
                var len = $('.wx_form').find('.wx_table2').length;
                if(type == 'check_all'){//全选时
                    if(that.prop('checked') == true){
                        $('.wx_form input[name=check_single]').prop('checked',true);
                    }else{
                        $('.wx_form input[name=check_single]').prop('checked',false);    
                    }
                }else if(type == 'check_single'){//单选时
                    if(len == $('.wx_form input[name=check_single]:checked').length){
                        $('.wx_form input[name=check_all]').prop('checked',true);
                    }else{
                        $('.wx_form input[name=check_all]').prop('checked',false);
                    }
                }
            })

            //加入购物车
            $('.wx_form .AddToCar').off('click').on('click',function(){
                var that = $(this);
                var type = that.attr('name');
                var arr = [];
                var isalreday = that.attr('alreday');

                if(CTLogin.IsLogin != true || CTLogin.Category != 29001){
                    layer.confirm('您不是广告主，请登录！', {
                        btn: ['登录'] //按钮
                    }, function(){
                        var url = encodeURI('/static/advertister/sort_list.html');
                        window.location = '/Exit.aspx?gourl='+url;
                    })
                }else{
                    var index_load = layer.load(0, {shade: false});
                    if(type == 'total'){//多选
                        $('.wx_form .one_check').each(function(){
                            var cur = $(this);
                            if(cur.prop('checked') == true){
                                obj = {
                                    MediaType : cur.parents('.wx_table2').attr('MediaType')*1,
                                    MediaID : cur.parents('.wx_table2').attr('MediaID')*1
                                }
                                arr.push(obj);
                                cur.parents('.wx_table2').find('.AddToCar').attr('alreday',1);
                            }
                        })
                    }else if(type == 'single'){//单选
                        var single = that.parents('.wx_table2');
                        var obj = {
                            MediaType : single.attr('MediaType')*1,
                            MediaID : single.attr('MediaID')*1
                        }
                        arr.push(obj);
                        that.attr('alreday',1);
                    }
                    layer.close(index_load);
                    _this.AddToShopCar(arr);
                }
            })
        },
        AddToShopCar : function(arr){//添加购物车
            var _this = this;
            if(arr.length > 0){
                $.ajax({
                    url:  public_url + '/api/MediaMatching/AddCartInfo',
                    type: 'post',
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    data: {
                        CartInfoList : arr,
                        r: Math.random()
                    },
                    async : false,
                    success : function (data) {
                        if(data.Status == 0){
                            //收起购物车
                            $('#quick_links_pop').animate({left:'280px'});
                            _this.GetCartCount();
                            _this.GetCarListData();//加载购物车数据
                        }else{
                            layer.msg(data.Message,{'time':2000});
                        }
                    }
                })
            }            
        },
        LauchShopCar : function(){//展开购物车
            var _this = this;
            //展开的时候加载数据
            $('#shopCart').off('click').on('click',function(){
                var $pop = $('#quick_links_pop');
                if($pop.css('left') == '280px'){
                    $('.mui-mbar-tabs,.quick_links_wrap').css('width','320px');
                    $pop.animate({left:0});
                    $('.big_box').not('.mui-mbar-tabs').off('click').on('click',function(){
                        $('.mui-mbar-tabs,.quick_links_wrap').css('width','0');
                        $pop.animate({left:'280px'});
                    })
                    _this.GetCarListData();//加载购物车数据
                }else{
                    $('.mui-mbar-tabs,.quick_links_wrap').css('width','0');
                    $pop.animate({left:'280px'});
                }
            })
        },
        GetCarListData : function(){//加载购物车数据
            var _this = this;

            if(CTLogin.Category == 29001){
                //详细数据
                $.ajax({
                    url: public_url + '/api/MediaMatching/GetCartInfoList',
                    type: 'get',
                    xhrFields: {
                        withCredentials: true,
                        r: Math.random()
                    },
                    crossDomain: true,
                    success : function (data) {
                        if(data.Status == 0){
                            var Result = data.Result;
                            if(Result.length > 0){
                                $('.car_list').html(ejs.render($('#detail-shopcar').html(), {CartList:Result} ));                            
                            }else{
                                $('.car_list').html('<img src="/images/shop_car.png" style="display: block;margin: 140px auto;">');
                            }
                            $('#shopCart .cart_num').html(Result.length);
                            _this.GetShopCarNum(Result);
                            _this.GetOperation();
                        }else{
                            layer.msg(data.Message,{'time':2000});
                        }
                    }
                })
            }
        },
        GetShopCarNum : function(Result){//获取购物车一系列数量
            var _this = this;
            var wx_total = 0;
            var ap_total = 0;
            var wb_total = 0;
            Result.forEach(function(item){
                if(item.MediaType == 14001){
                    wx_total = wx_total + 1;
                }else if(item.MediaType == 14002){
                    ap_total = ap_total + 1;
                }else if(item.MediaType == 14003){
                    wb_total = wb_total + 1;
                }
            })

            if(wx_total > 0){
                $('.wx_total').prev('img').attr('src','/images/s_wx.png');
            }else{
                $('.wx_total').prev('img').attr('src','/images/s_wx1.png');
            }
            if(wb_total > 0){
                $('.wb_total').prev('img').attr('src','/images/s_wb.png');
            }else{
                $('.wb_total').prev('img').attr('src','/images/s_wb1.png');
            }
            if(ap_total > 0){
                $('.ap_total').prev('img').attr('src','/images/s_app.png');
            }else{
                $('.ap_total').prev('img').attr('src','/images/s_app1.png');
            }

            $('.wx_total').html(wx_total);
            $('.ap_total').html(ap_total);
            $('.wb_total').html(wb_total);

            _this.GetCartCount();
        },
        GetOperation : function(){//购物车一系列动作
            var _this = this;
            //删除
            $('.delete_car').off('click').on('click',function(){
                var that = $(this);
                var MediaID = that.parents('.cart_shop').attr('MediaID');
                var MediaType = that.parents('.cart_shop').attr('MediaType');
                var CartInfoList = [
                    {
                        MediaID : MediaID,
                        MediaType : MediaType
                    }
                ];
                $.ajax({
                    url: public_url + '/api/MediaMatching/DelCartInfo',
                    type: 'post',
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    data : {
                        CartInfoList : CartInfoList,
                        r: Math.random()
                    },
                    success : function (data) {
                        if(data.Status == 0){
                            var Result = data.Result;
                            _this.GetCarListData();
                        }else{
                            layer.msg(data.Message,{'time':2000});
                        }
                    }
                })
            })

            //全部删除
            $('#delete_all').off('click').on('click',function(){
                var that = $(this);      
                var CartInfoList = [];          
                $('.car_list .cart_shop').each(function(){
                    var that = $(this);
                    var MediaID = that.attr('MediaID');
                    var MediaType = that.attr('MediaType');
                    var obj = {
                        MediaID : MediaID,
                        MediaType : MediaType
                    }
                    CartInfoList.push(obj);
                })
                if(CartInfoList.length > 0){
                    $.ajax({
                        url: public_url + '/api/MediaMatching/DelCartInfo',
                        type: 'post',
                        xhrFields: {
                            withCredentials: true
                        },
                        crossDomain: true,
                        data : {
                            CartInfoList : CartInfoList,
                            r: Math.random()
                        },
                        success : function (data) {
                            if(data.Status == 0){
                                var Result = data.Result;
                                _this.GetCarListData();
                            }else{
                                layer.msg(data.Message,{'time':2000});
                            }
                        }
                    })
                }
            })

            //导出数据
            $('#CartExportExcel').off('click').on('click',function(){
                var that = $(this);
                var len = $('.car_list .cart_shop').length;
                if(len > 0){
                    var _url = public_url + '/api/MediaExport/CartExportExcel.aspx';
                    that.attr('href',_url);
                }                
            })

            //立即推广
            $('.go_detail').off('click').on('click',function(){
                var that = $(this);
                var _url = "/manager/advertister/extension/shoppingCart01.html";
                var GetCartCount = $('#shopCart .cart_num').html() * 1;
                if(GetCartCount > 0){
                    that.attr('href',_url);
                }else{
                    layer.msg('选号车为空，无法推广！',{time:2000});
                }
            })
        },
        GetCartCount : function(){//获取购物车总数
            if(CTLogin.Category == 29001){
                $.ajax({
                    url: public_url + '/api/MediaMatching/GetCartCount',
                    type: 'get',
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    success : function (data) {
                        if(data.Status == 0){
                            var Result = data.Result;
                            var CartCount = Result.CartCount;
                            $('.mui-mbar-tabs .cart_num').html(CartCount + '个');
                            $('#shopCart .cart_num').html(CartCount);
                        }else{
                            layer.msg(data.Message,{'time':2000});
                        }
                    }
                })
            }
        },
        DeleteSingleOption : function(){//单个条件删除

            var idx = $('#MediaChange .selected').index();
            var _this = this;
            //单个条件删除  从新加载数据
            $('#CheckedOption .close').off('click').on('click',function(){
                var that = $(this);
                var order = that.parents('li').attr('order')*1;
                var _li = $('.ChangeOptions').eq(order-1).find('li').eq(0);
                _li.find('a').addClass('active').parent('li').siblings('li').find('a').removeClass('active');
                that.parents('li').remove();

                if(order == 0){//关键字
                    obj.keyword = '';
                    _this.RequestParam(obj);
                    $('#search-input').val('');
                }else if(order == 1){//分类
                    obj.CategoryID = -1;
                    _this.RequestParam(obj);
                }else if(order == 2){//粉丝数量
                    obj.MinFansCount = -1;
                    obj.MaxFansCount = -1;
                    _this.RequestParam(obj);
                    $('#FanSure').prev().val('');
                    $('#FanSure').prev().prev().val('');
                    $('#FansCount').attr('minId','-1');
                    $('#FansCount').attr('maxId','-1');
                }else if(order == 3){//价格
                    obj.MinPrice = -1;
                    obj.MaxPrice = -1;
                    obj.ADPositionID = -1;
                    obj.ReferenceType = -1;
                    _this.RequestParam(obj);
                    $('#Price').attr('minId','-1');
                    $('#Price').attr('maxId','-1');
                    $('#PriceSure').prev().val('');
                    $('#PriceSure').prev().prev().val('');
                }else{//地区
                    obj.ProvinceID = -1;
                    obj.CityID = -1;
                    _this.RequestParam(obj);
                    $('#City').attr('ProvinceID','-1');
                    $('#City').attr('CityID','-1');
                    $('#ProvinceID option:first').prop('selected',true);
                    $('#CityID option:first').prop('selected',true);
                }
                var len = $('#CheckedOption li').length;
                if(len < 1){
                    $('.wx_already_wrap').hide();
                }else{
                    $('.wx_already_wrap').show();
                }
            })
            //清空全部条件 从新加载数据
            $('#CheckedOption .delete_all').off('click').on('click',function(e){
                e.preventDefault();
                _this.InitNoSort();
                obj = {
                    ListType : ListType[idx],
                    keyword : '',
                    PageIndex : 1,
                    PageSize : 20,
                    CategoryID : -1,
                    MaxFansCount : -1,
                    MinFansCount : -1,
                    MaxPrice : -1,
                    MinPrice : -1,
                    ProvinceID : -1,
                    CityID : -1,
                    ADPositionID : -1,
                    ReferenceType : -1,
                    SortField : -1,
                    SortIndex : -1
                }
                $('#CheckedOption').html('');
                $('#search-input').val('');
                $('#FanSure').prev().val('');
                $('#FanSure').prev().prev().val('');
                $('#PriceSure').prev().val('');
                $('#PriceSure').prev().prev().val('');
                $('#ProvinceID option:first').prop('selected',true);
                $('#CityID option:first').prop('selected',true);
                $('#FansCount').attr('minId','-1');
                $('#FansCount').attr('maxId','-1');
                $('#Price').attr('minId','-1');
                $('#Price').attr('maxId','-1');
                $('#City').attr('ProvinceID','-1');
                $('#City').attr('CityID','-1');
                _this.RequestParam(obj);

                var len = $('#CheckedOption li').length;
                if(len < 1){
                    $('.wx_already_wrap').hide();
                }else{
                    $('.wx_already_wrap').show();
                }
            })

            $('#CheckedOption li').each(function(){
                var len = $('#CheckedOption li').length;
                if(len < 1){
                    $('.wx_already_wrap').hide();
                }else{
                    $('.wx_already_wrap').show();
                }
            })
        }   
    }

    var WxList = new WxList();

    //判断一个数组的内容是否都一样
    function isAllEqual(array){
        if(array.length>0){
           return !array.some(function(value,index){
             return value !== array[0];
           });   
        }else{
            return true;
        }
    }
})