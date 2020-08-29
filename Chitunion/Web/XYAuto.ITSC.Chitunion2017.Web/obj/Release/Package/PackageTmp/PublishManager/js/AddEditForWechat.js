/**
 * Created by fengb on 2017/4/22.
 * 1.区分媒体主和AE添加刊例的内容不一样
 * 2.上传图片显示缩略图以及鼠标滑过显示大图
 * 3.添加广告位的弹层  以及对应的显示到页面中
 * 4.上传文件
 * 5.金额折扣计算 验证input
 * 6.单图文多图文实例
 * 7.验证input
 * 8.删除广告位数据 至少保留一个
 * 9.刊例列表里面有一个添加刊例 还有操作中有编辑刊例   媒体列表里面操作还有一个添加刊例
 *
 * 媒体主003和AE005角色有添加刊例   区别在于媒体主没有折扣 没有最后的上传附件  而AE都有
 *
 *     MediaID=16632&entrance=1   入口1  媒体列表  传mediaID 可以不用穿媒体名称  添加媒体的同时也能添加刊例
 *
 *     entrance=2                 入口2  刊例列表  没有mediaID传媒体名称也就是公众号账号
 *     MediaID=16632&entrance=2&increase=1    入口3  从每一行添加刊例的时候   都需要把相应的媒体ID传过去
 *     PubID=1&isAdd=1&entrance=2&MediaID=2     编辑
 *     PubID=1&isAdd=2&entrance=2             复制
 *
 *
 */


$(function () {

    var config = {
        RoleIDs : CTLogin.RoleIDs,
        Publish : {
            "PubID": '',
            "ADName": "",
            "MediaType": 14001,
            "MediaID": '',
            "MediaName": "",
            'MediaNumber':'',
            "BeginTime": "",
            "EndTime": "",
            "PurchaseDiscount": '',
            "SaleDiscount": '',
            "ImgUrl":''
        },
        Details : [
            {
                "ADPosition1": 6002,
                "ADPosition2": 8001,
                "ADPosition3": -2,
                "Price": '',
                "SalePrice": '',
                'SmallImgUrls':["/images/icon65.png","/images/icon65.png","/images/icon65.png"],
                "ImgUrls": ['','','','']
            },
            {
                "ADPosition1": 6003,
                "ADPosition2": 8001,
                "ADPosition3": -2,
                "Price": '',
                "SalePrice": '',
                "SmallImgUrls": ["/images/icon65.png","/images/icon65.png","/images/icon65.png"],
                'ImgUrls':['','','','']
            },
            {
                "ADPosition1": 6001,
                "ADPosition2": 8001,
                "ADPosition3": -2,
                "Price": '',
                "SalePrice": '',
                "SmallImgUrls": ["/images/icon65.png","/images/icon65.png","/images/icon65.png"],
                'ImgUrls':['','','','']
            }
        ]
    };

    var PubID = GetQueryString('PubID')!=null&&GetQueryString('PubID')!='undefined'?parseFloat  (GetQueryString('PubID')):null;
    var MediaID = GetQueryString('MediaID')!=null&&GetQueryString('MediaID')!='undefined'?parseFloat(GetQueryString('MediaID')):null;
    var isAdd = GetQueryString('isAdd')!=null&&GetQueryString('isAdd')!='undefined'?parseFloat(GetQueryString('isAdd')):null;
    var entrance = GetQueryString('entrance')!=null&&GetQueryString('entrance')!='undefined'?parseFloat(GetQueryString('entrance')):null;//入口 ！！！！！！！！！！
    var increase = GetQueryString('increase')!=null&&GetQueryString('increase')!='undefined'?parseFloat(GetQueryString('increase')):null;//入口1的区分

    var MediaName = '';

    //获取原有的广告位信息数组
    var alredayPositionArr = [];

    function  AddEditPublish() {
        this.init();
    }

    //初始化
    AddEditPublish.prototype.init = function () {
        var _this = this;
        if(entrance == 1){//从媒体列表进来的
            //根据媒体ID查询 媒体的基本信息   微信名称 账号 图像
            var url = '/api/media/GetItem?v=1_1';
            setAjax({
                url : url,
                type : 'get',
                data : {
                    mediaId : MediaID,
                    businesstype : 14001
                }
            },function (data) {
                if(data.Status == 0){
                    config.MediaName = data.Result.Name;//微信名称
                    config.MediaNumber = data.Result.Number;//微信账号
                    config.HeadIconURL = data.Result.HeadIconURL;//只有从媒体中过来会有这个图像字段
                    config.entrance = entrance;//入口字段
                    _this.getImgTextNum();//图文文字和数字对应
                    _this.showTemplate(config,"#AddTemplate");//渲染数据
                    $('.ADName').attr('value',config.MediaName);
                }
            })
        }else{//当为2的时候从广告列表进来  保留公众号账号的元素   内容模糊查询从接口读出来

            if(increase == 1){//添加广告   从每一行进来带过来媒体ID
                console.log('从每一行点击进来添加广告');
                var url = '/api/media/GetItem?v=1_1';
                setAjax({
                    url : url,
                    type : 'get',
                    data : {
                        mediaId : MediaID,
                        businesstype : 14001
                    }
                },function (data) {
                    if(data.Status == 0){
                        config.MediaName = data.Result.Name;//微信名称
                        config.MediaNumber = data.Result.Number;//微信账号
                        config.ADName = data.Result.ADName;//广告名称
                        config.increase = increase;//入口2的区分字段
                        _this.getImgTextNum();//图文文字和数字对应
                        _this.showTemplate(config,"#AddTemplate");//渲染数据
                        $('.ADName').attr('value',config.MediaName);
                        MediaName = MediaID;//将媒体ID赋值给全局变量
                    }
                })
            }else if(isAdd != null) {//编辑  复制
                if(MediaID != null){
                    console.log('编辑，传PubID');
                    _this.isAddPublish();
                    config.isAudit = true;//是否编辑的情况下做的标识
                    MediaName = MediaID;//将媒体ID赋值给全局变量
                }else{
                    console.log('复制相当于新增不传PubID');
                    _this.isAddPublish();
                    config.isAudit = true;//是否编辑的情况下做的标识
                    MediaName = MediaID;//将媒体ID赋值给全局变量
                }
            }else{
                console.log('直接点击添加广告,新增不传PubID');
                _this.getImgTextNum();//图文文字和数字对应
                _this.showTemplate(config,"#AddTemplate");//渲染数据
            }
        }
    }

    //验证input
    AddEditPublish.prototype.checkOfInput = function () {
        //input不能为空
        $('input[name=Username]').each(function () {
            $(this).on('blur',function () {
                var txt = $(this).val();
                if($.trim(txt) ==''){
                    $(this).parents('ul').find('.errorMes').show();
                }else{
                    $(this).parents('ul').find('.errorMes').hide();
                }
            })
        });

        //开始日期
        $(".install").find("#startTime").off("click").on("click", function () {
            laydate({
                elem: "#startTime",
                fixed: false,
                choose: function (date) {
                    if(date>$(".install").find("#endTime").val() && $(".install").find("#endTime").val()){
                        layer.alert('起始时间不能大于结束时间！');
                        $(".install").find("#startTime").val('');
                    }
                }
            });
        });

        //结束日期
        $(".install").find("#endTime").off("click").on("click", function () {
            laydate({
                elem: "#endTime",
                fixed: false,
                min : laydate.now(),
                choose: function (date) {
                    if(date<$(".install").find("#startTime").val() && $(".install").find("#startTime").val()){
                        layer.alert('结束时间不能小于起始时间！');
                        $(".install").find("#endTime").val('');
                    }
                }
            });
        });
    }

    //验证价格
    AddEditPublish.prototype.PriceVerification = function () {
        //var reg = /^(0|[1-9][0-9]{0,9})(\.[0-9]{1,2})?$/;
        var reg = /^(\d{1,2}(\.\d{1,3})?|100|100\.0|100\.00)$/;//折扣验证
        var purchase = null;//采购折扣
        var sales = null;//销售折扣
        var periodication = 0;//刊例价格
        var total = 0;//销售价格

        purchase = $('.purchase').val();
        sales = $('.sales').val();

        //判断是否有折扣的情况下    比如有的角色有折扣  有的角色没有折扣
        if(sales == undefined){//没有折扣的情况下    销售价格就是刊例价格
             $('.periodicationPrice').each(function(index) {
                 $(this).on('change',function () {
                    periodication = $(this).val();
                    $(this).parent().next().find('.retailPrice').val(periodication);//设置销售价格
                    //将价格存起来放入tr里面
                    $(this).parents('tr').attr('price',periodication);//刊例价格
                    $(this).parents('tr').attr('saleprice',periodication);//销售价格

                    //从对应的tr里面取出价格 然后存入Details对应里面的属性
                    for(var i = 0;i <= config.Details.length -1;i++){
                        var price1 = $('table').find('tr').eq(i+1).attr('price');
                        var saleprice1 = $('table').find('tr').eq(i+1).attr('saleprice');
                        config.Details[i].Price = price1;
                        config.Details[i].SalePrice = saleprice1;
                    }
                })
            })


            //销售价格手动改变的时候不能低于采购价
            $('.retailPrice').each(function () {
                $(this).on('change',function () {
                    total = $(this).parents('tr').attr('price')*1;
                    var val = $(this).val()*1;
                    if(val < total){
                        alert("销售价不能低于采购价");
                        $(this).val('');
                        $(this).parents('tr').attr('saleprice','');
                    }else{
                        $(this).parents('tr').attr('saleprice',val);
                        //从对应的tr里面取出价格 然后存入Details对应里面的属性
                        for(var i = 0;i <= config.Details.length -1;i++){
                            var price1 = $('table').find('tr').eq(i+1).attr('price');
                            var saleprice1 = $('table').find('tr').eq(i+1).attr('saleprice');
                            config.Details[i].Price = price1;
                            config.Details[i].SalePrice = saleprice1;
                        }
                    }
                })
            })
        }else{//当存在刊例价格的时候     分为新增和编辑
            purchase = $('.purchase').val();
            sales = $('.sales').val();
            //获取采购折扣   手动更改折扣的情况下
            $(".purchase").on("blur",function () {
                purchase = $(this).val();
                var txt = $(this).attr('txt');
                var val = $.trim($(this).val());
                if(val == ''){
                    var str = "<img src=../images/icon21.png><span>"+txt+"不能为空</span>";
                    $(this).parents('ul').find('.errorMes').show();
                    $(this).parents('ul').find('.errorMes').html(str);
                }else if(!reg.test(val)){
                    var str = '<img src="../images/icon21.png"><span>请输入0〜100之间的整数或小数(保留三位)</span>';
                    $(this).parents('ul').find('.errorMes').show();
                    $(this).parents('ul').find('.errorMes').html(str);
                }else{
                    $(this).parents('ul').find('.errorMes').hide();
                }
            });
            // 获取销售折扣
            $(".sales").on("blur",function () {
                sales = $(this).val()*1;
                purchase = $('.purchase').val()*1;
                var txt = $(this).attr('txt');
                var val = $.trim($(this).val());
                if(val == ''){
                    var str = "<img src=../images/icon21.png><span>"+txt+"不能为空</span>";
                    $(this).parents('ul').find('.errorMes').show();
                    $(this).parents('ul').find('.errorMes').html(str);
                }else if(!reg.test(val)){
                    var str = '<img src="../images/icon21.png"><span>请输入0〜100之间的整数或小数(保留三位)</span>';
                    $(this).parents('ul').find('.errorMes').show();
                    $(this).parents('ul').find('.errorMes').html(str);
                }else if(sales < purchase){
                    var str = "<img src=../images/icon21.png><span>销售折扣不能小于采购折扣</span>";
                    $(this).parents('ul').find('.errorMes').css('display','block');
                    $(this).parents('ul').find('.errorMes').html(str);
                }else{
                    $(this).parents('ul').find('.errorMes').hide();
                }
            });

            $('.periodicationPrice').each(function () {
                $(this).on('change',function () {
                    periodication = $(this).val();
                    total = periodication*sales/100;//获取销售价格
                    $(this).parent().next().find('.retailPrice').val(total);//设置销售价格

                    //将价格存起来放入tr里面
                    $(this).parents('tr').attr('price',periodication);
                    $(this).parents('tr').attr('saleprice',total);

                    //从对应的tr里面取出价格 然后存入Details对应里面的属性
                    for(var i = 0;i <= config.Details.length -1;i++){
                        var price1 = $('table').find('tr').eq(i+1).attr('price');
                        var saleprice1 = $('table').find('tr').eq(i+1).attr('saleprice');
                        config.Details[i].Price = price1;
                        config.Details[i].SalePrice = saleprice1;
                    }
                })
            })


            //销售价格手动改变的时候不能低于采购价  采购价等于刊例价乘以采购折扣
            $('.retailPrice').each(function () {
                $(this).on('change',function () {
                    total = $(this).parents('tr').attr('price')*purchase/100;
                    var val = $(this).val()*1;
                    if(val <= total){
                        alert("销售价不能低于采购价");
                        $(this).val('');
                        $(this).parents('tr').attr('saleprice','');
                    }else{
                        $(this).parents('tr').attr('saleprice',val);
                        //从对应的tr里面取出价格 然后存入Details对应里面的属性
                        for(var i = 0;i <= config.Details.length -1;i++){
                            var price1 = $('table').find('tr').eq(i+1).attr('price');
                            var saleprice1 = $('table').find('tr').eq(i+1).attr('saleprice');
                            config.Details[i].Price = price1;
                            config.Details[i].SalePrice = saleprice1;
                        }
                    }
                })
            })
        }


        //刊例价格 和销售价格只能输入正整数
        $(".install").find("input[name=quotationRate]").each(function(){
            $(this).on("input",function(){
                replaceAndSetPos(this,/[^0-9]/g,'');
            })
        });

        //采购折扣 和销售折扣 验证 0-100的正实数 blur的时候不能为空
        function discountVerification(event) {
            $(event).on("blur",function () {
                var txt = $(event).attr('txt');
                var val = $.trim($(event).val());
                if(val == ''){
                    var str = "<img src=../images/icon21.png><span>"+txt+"不能为空</span>";
                    $(this).parents('ul').find('.errorMes').show();
                    $(this).parents('ul').find('.errorMes').html(str);
                }else if(!reg.test(val)){
                    var str = '<img src="../images/icon21.png"><span>请输入0〜100之间的整数或小数(保留三位)</span>';
                    $(this).parents('ul').find('.errorMes').show();
                    $(this).parents('ul').find('.errorMes').html(str);
                }else{
                    $(this).parents('ul').find('.errorMes').hide();
                }
            })
        };
        //discountVerification('.purchase');
        //discountVerification('.sales');

    }

    //添加广告位  弹层
    AddEditPublish.prototype.addAdvertisingPosition = function () {
        var _this = this;
        $('#addPositionPopup').on('click',function (event) {
            event.preventDefault();
            $.openPopupLayer({
                name: "popLayerDemo",
                url: "./Advertising-position.html",
                success: function () {
                    //图片上传
                    uploadImg("Uploadify_popup","UploadFile_popup","","bigImg_popup");
                    uploadImg("Uploadify_popup1","UploadFile_popup1","","bigImg_popup1");
                    uploadImg("Uploadify_popup2","UploadFile_popup2","","bigImg_popup2");

                    //刊例价格 和销售价格只能输入正整数
                    $(".ad_add").find("input[name=Username]").each(function(){
                        $(this).on("input",function(){
                            replaceAndSetPos(this,/[^0-9]/g,'');
                        })
                    });

                    /*输入刊例价格  销售价格自动输入*/
                    $('.kanliPrice').on('change',function () {
                        var price = $(this).val();
                        $('.xiaoshouPrice').val(price);
                    });

                    var newPosition = '';//广告位信息数组
                    var AdPositionText = '';
                    var PuStyleText = '';

                    $('select[name=AdPosition]').on('change',function () {
                        newPosition = '';
                        AdPositionText = $(this).find("option:selected").text();
                        newPosition +=  AdPositionText ;
                        newPosition += '-'+PuStyleText;
                    });
                    $('select[name=PuStyle]').on('change',function () {
                        newPosition = '';
                        PuStyleText = $(this).find("option:selected").text();
                        newPosition +=  AdPositionText ;
                        newPosition += '-'+PuStyleText;
                    });

                    //点击添加的时候
                    $("#addDetailMessage").off('click').on('click',function (event) {
                        event.preventDefault();

                        var AdPosition = $('select[name=AdPosition]').val();//广告形式  默认的
                        var PuStyle = $('select[name=PuStyle]').val();//发布形式
                        var kanliPrice = $('.kanliPrice').val();//刊例价格
                        var xiaoshouPrice = $('.xiaoshouPrice').val();//销售价格
                        var ADPosition1 = '';
                        var ADPosition2 = '';
                        var ADPosition3 = -2;
                        //获取原有的广告位信息数组  从全局变量里面取出来
                        var ADPositionText1 = '';
                        var ADPositionText2 = '';

                        var MediaName = $('#MediaName').val();//广告主名称  又为媒体名称
                        var ADName = $('.ADName').val();//刊例名称
                        var BeginTime = $('#startTime').val();//开始日期
                        var EndTime = $('#endTime').val();//结束日期
                        //采购折扣
                        var purchase = ($('.purchase').val() == undefined) ? 100 : $('.purchase').val();
                        //销售折扣
                        var sales = ($('.sales').val() == undefined ) ? 100 : $('.sales').val();

                        var lastFileUrl = $('.lastFileUrl').attr('href');//最后一个上传的路径
                        var lastFileName = $('.lastFileName').text();//最后一个上传的文件名称

                        //上传图片路径
                        var SmallImgUrls = [];//小图
                        var ImgUrls = [];//大图
                        $('.tc .ad_add_pic').each(function () {
                            var _src1 = $(this).find('img').eq(0).attr('src');//小图
                            SmallImgUrls.push(_src1);
                            var _src2 = $(this).find('img').eq(1).attr('src');//大图
                            ImgUrls.push(_src2);
                        })

                        //判断是否是编辑情况下
                        if(config.isAudit == true){
                            //判断广告形式 和发布形式的字段
                            if(AdPosition == '单图文'){
                                ADPositionText1 = '单图文';
                                ADPosition1 = 6001;
                            }else if(AdPosition == '多图文头条'){
                                ADPositionText1 = '多图文头条';
                                ADPosition1 = 6002;
                            }else if(AdPosition == '多图文第二条'){
                                ADPositionText1 = '多图文第二条';
                                ADPosition1 = 6003;
                            }else if(AdPosition == '多图文3-N条'){
                                ADPositionText1 = '多图文3-N条';
                                ADPosition1 = 6004;
                            };

                            if(PuStyle == '直发'){
                                ADPositionText2 = '直发';
                                ADPosition2 = 8001;
                            }else if(PuStyle == '原创+发布'){
                                ADPositionText2 = '原创+发布';
                                ADPosition2 = 8002;
                            }else if(PuStyle == '贴片广告'){
                                ADPositionText2 = '贴片广告';
                                ADPosition2 = 8003;
                            };
                        }else{
                            //判断广告形式 和发布形式的字段
                            if(AdPosition == '单图文'){
                                ADPosition1 = 6001;
                            }else if(AdPosition == '多图文头条'){
                                ADPosition1 = 6002;
                            }else if(AdPosition == '多图文第二条'){
                                ADPosition1 = 6003;
                            }else if(AdPosition == '多图文3-N条'){
                                ADPosition1 = 6004;
                            };

                            if(PuStyle == '直发'){
                                ADPosition2 = 8001;
                            }else if(PuStyle == '原创+发布'){
                                ADPosition2 = 8002;
                            }else if(PuStyle == '贴片广告'){
                                ADPosition2 = 8003;
                            };
                        }

                        var str = '';
                        if(AdPosition == '广告形式'){
                            str += '<img src="../images/icon21.png"><span>请选择广告形式</span>';
                        }else if(PuStyle == '发布形式'){
                            str += '<img src=../images/icon21.png><span>请选择发布形式</span>';
                        }else if(kanliPrice == ''){
                            str += '<img src=../images/icon21.png><span>请填写刊例价格</span>';
                        }else if(xiaoshouPrice == ''){
                            str += '<img src=../images/icon21.png><span>请填写销售价格</span>';
                        }else if(alredayPositionArr.indexOf(newPosition) > -1){
                            str += '<img src=../images/icon21.png><span>该广告位已经存在</span>';
                        }else{
                            var obj = {ADPosition1:ADPosition1,ADPosition2:ADPosition2,ADPosition3:ADPosition3,Price:kanliPrice,SalePrice:xiaoshouPrice*sales/100,SmallImgUrls:SmallImgUrls,ImgUrls:ImgUrls,ADPositionText1:ADPositionText1,ADPositionText2:ADPositionText2};

                            config.Publish = {
                                "PubID": '',//新增没有刊例ID
                                "ADName": ADName,
                                "MediaType": 14001,
                                "MediaID": MediaID,
                                'MediaNumber' : '',//多加的为了渲染不为空
                                "MediaName": MediaName,
                                "BeginTime": BeginTime,
                                "EndTime": EndTime,
                                "PurchaseDiscount": purchase,
                                "SaleDiscount": sales,
                                "ImgUrl" : lastFileUrl + '|' +lastFileName//最后一个上传的路径
                            }

                            //在上传之后  渲染 还要保留之前存的数据和金钱
                            var imgFileArr = {};//小图
                            var bigImgUrls = {};//大图
                            $('table tbody>tr').each(function (i) {
                                imgFileArr[i]=[];
                                bigImgUrls[i]=[];
                                $(this).find('td').eq(1).find('.ad_map').each(function () {
                                    var _src1 = $(this).find('img').eq(0).attr('src');
                                    imgFileArr[i].push(_src1);
                                    var _src2 = $(this).find('img').eq(1).attr('src');
                                    bigImgUrls[i].push(_src2);
                                })
                            })

                            for(var v = 0;v < config.Details.length;v++){
                                config.Details[v].SmallImgUrls = imgFileArr[v];//小图
                                config.Details[v].ImgUrls = bigImgUrls[v];//大图
                            }
                            config.Details.push(obj);//添加到初始化变量里面
                            console.log(config);
                            $.closePopupLayer('popLayerDemo');//弹层关闭
                            _this.getImgTextNum();//图文形式对应
                            _this.showTemplate(config,"#AddTemplate");//渲染数据 并一系列操作
                        };

                        $('.errorTip').html(str);
                    });

                    /*弹层关闭*/
                    $('.closePopup').each(function(){
                        $(this).off('click').on('click',function(event){
                            event.preventDefault();
                            $.closePopupLayer('popLayerDemo');
                        })
                    });
                }
            })
        })
    }


    //渲染模板  传data数据
    AddEditPublish.prototype.showTemplate = function(data,id){
        var _this = this;
        //->首先把页面中模板的innerHTML获取到
        var str=$(id).html();
        //->然后把str和data交给EJS解析处理，得到我们最终想要的字符串
        var result = ejs.render(str, {
            data: data
        });

        //->最后把获取的HTML放入到MENU
        $(".order_r").html(result);

        AllImgInitShow();//所有的图片初始化的问题
        uploadImg("lastFileSrc","","","");//最后一个上传

        _this.PriceVerification();//验证价格和折扣
        _this.checkOfInput();//验证input不能为空
        _this.addAdvertisingPosition();//添加广告位的弹层
        _this.safeAllData();//保存数据
        _this.deleteAdpositionData();//删除广告位数据
        _this.giveExample();//多图文单图文实例

        //鼠标放过显示大图
        $('.ad_map').mousemove(function () {
            var that = $(this);
            var _src = that.find('.bigPhoto').find('img').attr('src');
            if(_src != ''){
                that.find('.bigPhoto').show();
            }
        }).mouseout(function () {
            var that = $(this);
            that.find('.bigPhoto').hide();
        })


        /*关于相同广告位不能重复的问题*/
        $('table tbody>tr').each(function (i) {
            var that = $(this);
            var text = $.trim(that.find('td').eq(0).text());
            alredayPositionArr.push(text);
        })
        /* 将广告位数组去重 alredayPositionArr*/
        var result = [];
        for (var i = 0; i < alredayPositionArr.length; i++) {
            if (result.indexOf(alredayPositionArr[i]) == -1) {
                result.push(alredayPositionArr[i]);
            }
        }

        //公众号账号  模糊查询
        $('#MediaName').off('focus').on('focus',function () {
            var val = $.trim($(this).val());
            if(val == ''){
                setAjax({
                    url:"/api/ADOrderInfo/QueryWeChat_NumerOrName?v=1_1",
                    type:'get',
                    data:{
                        NumberORName : val,
                        AuditStatus : 43002
                    },
                    dataType:'json'
                },function(data){
                    if(data.Status != 0){
                        var PubliceMessage = [];
                        $('#MediaName').autocomplete({
                            source: PubliceMessage
                        })
                    }
                })
            }
        })

        var NewPubliceMessage = [];
        $('#MediaName').off('keyup').on('keyup',function () {
            var val = $.trim($(this).val());
            if(val.length > 0){
                setAjax({
                    url:"/api/ADOrderInfo/QueryWeChat_NumerOrName?v=1_1",
                    type:'get',
                    data:{
                        NumberORName : val,
                        AuditStatus : 43002
                    },
                    dataType:'json'
                },function(data){
                    if(data.Status == 0){
                        var Result = data.Result;
                        var MediaIdArr = [];
                        var PubliceMessage = [];//每一行的信息
                        Result.forEach(function (item) {
                            var message = item.Number;
                            PubliceMessage.push(message);
                            MediaIdArr.push(item.MediaID);
                        })
                        NewPubliceMessage = PubliceMessage;
                        $('#MediaName').autocomplete({
                            source: PubliceMessage,
                            select : function (event,ui) {
                                var txt = ui.item.value;
                                var idx = PubliceMessage.indexOf(txt);
                                MediaName = MediaIdArr[idx];
                                $(this).parent().next().hide();
                                $(this).parents('ul').next().find('.ADName').val(txt.split(' ')[0]);
                            }
                        })
                    }
                })
            }
        })


        //在离开的时候需要验证是否是有效的公众号
        $('#MediaName').off('blur').on('blur',function () {
            var val = $.trim($(this).val());
            if(NewPubliceMessage.indexOf(val) == -1){
                $(this).parent().next().show();
                $(this).parent().next().html('<img src="../images/icon21.png">请输入有效的公众号');
            }
        })

    }

    //单图文多图文  文字和数字一一对应关系
    AddEditPublish.prototype.getImgTextNum = function () {
        var adArr = ['单图文','多图文头条','多图文第二条','多图文3-N条'];//广告形式
        var geStyArr = ['直发','原创+发布','贴片广告'];//发布形式

        var adArrNum = [6001,6002,6003,6004];
        var geStyArrNum = [8001,8002,8003];

        var totalText = [];
        var totalNum = [];
        var styleIndex = '';

        //广告形式 发布形式文字
        for(var i=0;i<=adArr.length-1;i++){
            for(var j=0;j<=geStyArr.length-1;j++){
                totalText.push(adArr[i] + '-' + geStyArr[j]);
            }
        }

        //广告形式 发布形式对应的数字
        for(var i=0;i<=adArrNum.length-1;i++){
            for(var j=0;j<=geStyArrNum.length-1;j++){
                totalNum.push(adArrNum[i] + '-' + geStyArrNum[j]);
            }
        }

        config.Details.forEach(function(item) {
            styleIndex =  totalNum.indexOf(item.ADPosition1 + '-' + item.ADPosition2);
            item.allPositionStyle = totalText[styleIndex];
        })
    }

    //保存数据
    AddEditPublish.prototype.safeAllData = function () {

        //保存所有的信息
        $('#safeAllData').off('click').on('click',function () {

            //$('#safeAllData').css('background','#ccc');
            //$('#safeAllData').attr('disabled',true);

            var priceArr = [];//存放价格数组
            var tr_len = $('.tbody_item').find('tr').length;//tr长度
            var MediaNameText = $('#MediaName').val();//广告主名称  又为媒体名称

            //公众号的提示
            var MediaNameTit = $('#MediaName').parent().next().css('display') == 'block';

            var ADName = $('.ADName').val();//刊例名称
            var BeginTime = $('#startTime').val();//开始日期
            var EndTime = $('#endTime').val();//结束日期
            //采购折扣
            var purchase = ($('.purchase').val() == undefined) ? 100 : $('.purchase').val();
            //销售折扣
            var sales = ($('.sales').val() == undefined ) ? 100 : $('.sales').val();

            var lastFileSrc = $('.lastFileUrl').attr('href');//最后一个上传的路径

            var imgFileArr = {};//大图
            var smallFileArr = {};//小图

            $('tbody tr').each(function (i) {
                imgFileArr[i] = [];
                smallFileArr[i] = [];
                //图片路径
                $(this).find('td').eq(1).find('.ad_map').each(function () {
                    //大图
                    var _src1 = $(this).find('img').eq(1).attr('src');
                    imgFileArr[i].push(_src1);
                    //小图
                    var _src2 = $(this).find('img').eq(0).attr('src');
                    if(_src2 == "/images/icon65.png"){
                        _src2 = '';
                    }
                    smallFileArr[i].push(_src2);
                })
                //价格
                $(this).find('.periodicationPrice').each(function () {
                    priceArr.push($(this).parents('tr').attr('price'));
                })
                $(this).find('.retailPrice').each(function () {
                    priceArr.push($(this).parents('tr').attr('saleprice'));
                })
            })

            for(var v = 0;v < config.Details.length;v++){
                config.Details[v].SmallImgUrls = smallFileArr[v];
                config.Details[v].ImgUrls = imgFileArr[v];
            }

            //将数组中如果有空的价格  去掉
            for(var i = priceArr.length ;i >= 0;i--){
                if(priceArr[i] == ''){
                    priceArr.splice(i,1);
                }
            }

            if(MediaNameTit == true ){
                layer.msg('请输入有效的公众号',{'time':1000});
            }else if(MediaNameText == ''){
                $('#MediaName').parent().next().show();
            }else if(ADName == ''){
                $('.ADName').parent().next().show();
            }else if(BeginTime == '' || EndTime == ''){
                $('#startTime').parent().next().show();
            }else if(purchase == ''){
                var txt = $('.purchase').attr('txt');
                var str = "<img src=../images/icon21.png><span>"+txt+"不能为空</span>";
                $('.purchase').parents('ul').find('.errorMes').show();
                $('.purchase').parents('ul').find('.errorMes').html(str);
            }else if(sales == ''){
                var txt = $('.sales').attr('txt');
                var str = "<img src=../images/icon21.png><span>"+txt+"不能为空</span>";
                $('.sales').parents('ul').find('.errorMes').show();
                $('.sales').parents('ul').find('.errorMes').html(str);
            }else if(priceArr.length < tr_len*2  ){
                $('.periodicationPrice').parents('table').next().find('.errorMes').show();
            }else if(lastFileSrc == 'javascript:;'){
                $('.periodicationPrice').parents('table').next().find('.errorMes').hide();//价格隐藏
                $('#lastFileSrc').parent().next().show();
            }else {
                $('.periodicationPrice').parents('table').next().find('.errorMes').hide();//价格隐藏

                if(isAdd == null){//在添加的情况下  分入口一  和入口二 参数不一致
                    if(entrance == 1){//有媒体ID  媒体name可以不传
                        console.log("添加1");
                        config.Publish = {
                            "PubID": '',//新增没有刊例ID
                            "ADName": ADName,
                            "MediaType": 14001,
                            "MediaID": MediaID,
                            "MediaName": '',
                            "BeginTime": BeginTime,
                            "EndTime": EndTime,
                            "PurchaseDiscount": purchase/100,
                            "SaleDiscount": sales/100,
                            "ImgUrl" : lastFileSrc//最后一个上传的路径
                        }
                        //将不用传的参数动态从对象中删除
                        config.Details.forEach(function (item) {
                             item.ADPositionText1 = undefined;
                             item.ADPositionText2 = undefined;
                             item.ADPosition1Code = undefined;
                             item.ADPosition2Code = undefined;
                             item.Combdimension = undefined;
                             item.ImgUrl1 = undefined;
                             item.ImgUrl2 = undefined;
                             item.ImgUrl3 = undefined;
                             item['ImgUrl1-sl'] = undefined;
                             item['ImgUrl2-sl'] = undefined;
                             item['ImgUrl3-sl'] = undefined;
                             item.OldPrice = undefined;
                             item.OldSalePrice = undefined;
                             item.RecID = undefined;
                             item.SmallImgUrls = undefined;
                             item.allPositionStyle = undefined;
                        })
                        setRequest(config.Publish,config.Details);
                    }else if(entrance = 2){//entrance = 2的时候  传媒体name
                        console.log("添加2");
                        config.Publish = {
                            "PubID": '',//新增没有刊例ID
                            "ADName": ADName,
                            "MediaType": 14001,
                            "MediaID": MediaName,
                            "MediaName": MediaNameText,
                            "BeginTime": BeginTime,
                            "EndTime": EndTime,
                            "PurchaseDiscount": purchase/100,
                            "SaleDiscount": sales/100,
                            "ImgUrl" : lastFileSrc//最后一个上传的路径
                        }
                        //将不用传的参数动态从对象中删除
                        config.Details.forEach(function (item) {
                            item.ADPositionText1 = undefined;
                            item.ADPositionText2 = undefined;
                            item.ADPosition1Code = undefined;
                            item.ADPosition2Code = undefined;
                            item.Combdimension = undefined;
                            item.ImgUrl1 = undefined;
                            item.ImgUrl2 = undefined;
                            item.ImgUrl3 = undefined;
                            item['ImgUrl1-sl'] = undefined;
                            item['ImgUrl2-sl'] = undefined;
                            item['ImgUrl3-sl'] = undefined;
                            item.OldPrice = undefined;
                            item.OldSalePrice = undefined;
                            item.RecID = undefined;
                            item.SmallImgUrls = undefined;
                            item.allPositionStyle = undefined;
                        })
                        setRequest(config.Publish,config.Details);
                    }
                }else{//有isAdd的时候    分为1和2的时候
                    if(isAdd  == 1){//编辑
                        console.log("编辑");
                        config.increase = undefined;
                        config.Publish = {
                            "PubID": PubID,
                            "ADName": ADName,
                            "MediaType": 14001,
                            "MediaID": MediaName,
                            "MediaName": '',
                            "BeginTime": BeginTime,
                            "EndTime": EndTime,
                            "PurchaseDiscount": purchase/100,
                            "SaleDiscount": sales/100,
                            "ImgUrl" : lastFileSrc//最后一个上传的路径
                        }
                        //将不用传的参数动态从对象中删除
                        config.Details.forEach(function (item) {
                            item.ADPositionText1 = undefined;
                            item.ADPositionText2 = undefined;
                            item.ADPosition1Code = undefined;
                            item.ADPosition2Code = undefined;
                            item.Combdimension = undefined;
                            item.ImgUrl1 = undefined;
                            item.ImgUrl2 = undefined;
                            item.ImgUrl3 = undefined;
                            item['ImgUrl1-sl'] = undefined;
                            item['ImgUrl2-sl'] = undefined;
                            item['ImgUrl3-sl'] = undefined;
                            item.OldPrice = undefined;
                            item.OldSalePrice = undefined;
                            item.RecID = undefined;
                            item.SmallImgUrls = undefined;
                            item.allPositionStyle = undefined;
                        })
                        //从接口拿出来数据  要和直接渲染的进行比较  看是否进行更改   如果不能更改则不能保存
                        var alredayObj = {
                            RoleIDs : CTLogin.RoleIDs
                        };

                        setAjax({
                            url : '/api/Periodication/SelectWXPublishByIDAndType',
                            type : 'get',
                            data : {
                                PubID : PubID,
                                MediaType : 14001
                            }
                        },function (data) {
                            var Result = data.Result;
                            //从接口获取出来的  然后转化为统一的对象 方便进行比较
                            alredayObj.Publish = {
                                "PubID": PubID,
                                "ADName": Result.ADName,
                                "MediaType": 14001,
                                "MediaID": Result.MediaID,//从接口获取出来
                                "MediaName": '',//名称
                                "BeginTime": Result.BeginTime.substr(0,10),//开始日期
                                "EndTime": Result.EndTime.substr(0,10),//结束日期
                                "PurchaseDiscount": Result.PurchaseDiscount,//采购折扣
                                "SaleDiscount": Result.SaleDiscount,//销售折扣
                                "ImgUrl" : Result.SingleFile//最后一个上传的路径
                            }
                            //媒体主的时候没有上传附件  所以要将接口的字段去掉
                            if(config.RoleIDs == 'SYS001RL00003'){
                                alredayObj.Publish.ImgUrl = undefined;
                            }

                            alredayObj.Details = Result.Detail;//广告位的信息
                            alredayObj.Details.forEach(function (item) {
                                var ImgUrls = [];
                                ImgUrls.push(item['ImgUrl1']);
                                ImgUrls.push(item['ImgUrl2']);
                                ImgUrls.push(item['ImgUrl3']);
                                item.ImgUrls = ImgUrls;//为了渲染数据所以得取出来
                                item.ADPosition1 = item.ADPosition1Code;
                                item.ADPosition2 = item.ADPosition2Code;
                                item.ADPosition3 = -2;
                                item.ADPositionText1 = undefined;
                                item.ADPositionText2 = undefined;
                                item.ADPosition1Code = undefined;
                                item.ADPosition2Code = undefined;
                                item.Combdimension = undefined;
                                item.ImgUrl1 = undefined;
                                item.ImgUrl2 = undefined;
                                item.ImgUrl3 = undefined;
                                item['ImgUrl1-sl'] = undefined;
                                item['ImgUrl2-sl'] = undefined;
                                item['ImgUrl3-sl'] = undefined;
                                item.OldPrice = undefined;
                                item.OldSalePrice = undefined;
                                item.RecID = undefined;
                                item.SmallImgUrls = undefined;
                                item.allPositionStyle = undefined;
                            })
                            alredayObj.isAudit = true;
                            alredayObj.increase = undefined;

                            //当不是AE的情况下  没有折扣 没有上传附件
                            if(config.RoleIDs != 'SYS001RL00005'){
                                config.Publish.ImgUrl = undefined;
                            }
                            if(alredayObj.RoleIDs != 'SYS001RL00005'){
                                alredayObj.Publish.ImgUrl = undefined;
                                alredayObj.Publish.PurchaseDiscount = 1;
                                alredayObj.Publish.SaleDiscount = 1;
                            }

                            //如果没有进行任何更改的话  则不能保存不能走下一步 并且提示
                            if(JSON.stringify(alredayObj) == JSON.stringify(config)){
                                layer.msg('您未对广告进行修改，不能提交',{'time':1000});
                            }else{
                                //调用修改的接口
                                setRequest(config.Publish,config.Details);
                            }
                        })
                    }else{//2的时候  复制
                        console.log("复制");
                        config.Publish = {
                            "PubID": '',//复制相当于新增
                            "ADName": ADName,
                            "MediaType": 14001,
                            "MediaID": MediaName,
                            "MediaName": '',
                            "BeginTime": BeginTime,
                            "EndTime": EndTime,
                            "PurchaseDiscount": purchase/100,
                            "SaleDiscount": sales/100,
                            "ImgUrl" : lastFileSrc//最后一个上传的路径
                        }
                        //将不用传的参数动态从对象中删除
                        config.Details.forEach(function (item) {
                            item.ADPositionText1 = undefined;
                            item.ADPositionText2 = undefined;
                            item.ADPosition1Code = undefined;
                            item.ADPosition2Code = undefined;
                            item.Combdimension = undefined;
                            item.ImgUrl1 = undefined;
                            item.ImgUrl2 = undefined;
                            item.ImgUrl3 = undefined;
                            item['ImgUrl1-sl'] = undefined;
                            item['ImgUrl2-sl'] = undefined;
                            item['ImgUrl3-sl'] = undefined;
                            item.OldPrice = undefined;
                            item.OldSalePrice = undefined;
                            item.RecID = undefined;
                            item.SmallImgUrls = undefined;
                            item.allPositionStyle = undefined;
                        })
                        setRequest(config.Publish,config.Details);
                    }
                }
            }
        })

        //添加修改的接口
        function setRequest(Publish,Details) {
            var url = '/api/Publish/ModifyPublish?v=1_1';
            setAjax({
                url:url,
                type:'post',
                data: {
                    Publish : config.Publish,
                    Details : config.Details
                }
            },function(data){
                if(data.Status == 0){
                    window.location = '/publishmanager/pricelist-wechatnew.html';
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            });
        }

    }

    //删除广告位数据
    AddEditPublish.prototype.deleteAdpositionData = function () {
        var _this = this;
        $('.deletePosition').each(function () {
            $(this).on('click',function (event) {
                event.preventDefault();
                var that = $(this).parents('tr');
                var len = $('.table > table tbody').find('tr').length;
                console.log(len);
                var idx = that.index();
                if(len <= 1){
                    layer.msg('至少保留一个广告位',{'time':1000});
                }else{
                    layer.confirm('确认要删除数据吗', {
                        btn: ['确认','取消'] //按钮
                    }, function(){
                        that.remove();
                        config.Details.splice(idx,1);//将它不只是在页面中删除  还从数据结构中删除
                        var positionArr = [];
                        /*关于相同广告位不能重复的问题*/
                        $('table tbody>tr').each(function (i) {
                            var that = $(this);
                            var text = that.find('td').eq(0).text();
                            positionArr.push(text);
                        })
                        alredayPositionArr = positionArr;
                        layer.closeAll();
                    })
                }
            })
        })
    }

    //如果是编辑的情况下  渲染数据
    AddEditPublish.prototype.isAddPublish = function () {

        //先查询出来详细数据   然后修改之后再从新保存
        var _this = this;
        var url = '/api/Periodication/SelectWXPublishByIDAndType';
        setAjax({
            url : url,
            type : 'get',
            data : {
                PubID : PubID,
                MediaType : 14001
            }
        },function (data) {
            var Result = data.Result;
            config.isAudit = true;//是否编辑的情况下做的标识
            config.Publish = {
                "PubID": PubID,
                "ADName": Result.ADName,//广告名称
                "MediaType": 14001,
                "MediaID": Result.MediaID,//从接口获取出来
                "MediaName": Result.Name,//名称
                'MediaNumber': Result.Number,//公众号账号
                "BeginTime": Result.BeginTime,//开始日期
                "EndTime": Result.EndTime,//结束日期
                "PurchaseDiscount": formatMoney(Result.PurchaseDiscount*100,2,''),//采购折扣
                "SaleDiscount": formatMoney(Result.SaleDiscount*100,2,''),//销售折扣
                "ImgUrl":Result.SingleFile + '|' + Result.SingleFileName
            }
            config.Details = Result.Detail;//广告位的信息
            config.Details.forEach(function (item) {
                var ImgUrls = [];
                var SmallImgUrls = [];
                ImgUrls.push(item['ImgUrl1']);
                ImgUrls.push(item['ImgUrl2']);
                ImgUrls.push(item['ImgUrl3']);
                SmallImgUrls.push(item['ImgUrl1-sl']);
                SmallImgUrls.push(item['ImgUrl2-sl']);
                SmallImgUrls.push(item['ImgUrl3-sl']);
                item.ImgUrls = ImgUrls;//为了渲染数据所以得取出来
                item.SmallImgUrls = SmallImgUrls;

                //渲染到页面中的汉字
                item.ADPositionText1 = item.ADPosition1;
                item.ADPositionText2 = item.ADPosition2;
                //从查看接口取出来的字段属性和   要传增加的接口的属性不一样  所以转一下
                item.ADPosition1 = item.ADPosition1Code;
                item.ADPosition2 = item.ADPosition2Code;
                item.ADPosition3 = -2;
            })

            MediaName = Result.MediaID;//赋给全局变量
            _this.getImgTextNum();//图文形式对应
            _this.showTemplate(config,"#AddTemplate");
            $('#MediaName').attr('disabled',true);//在复制和编辑的时候不让修改微信名称
        })
    }

    //单图文  多图文示例
    AddEditPublish.prototype.giveExample = function () {
        var w = window.innerWidth;
        var h = window.innerHeight;
        $('#imgDemo').css({'width':w,'height':h,'background':'rgba(0,0,0,0.5)'});

        $('a[href^= "showDemo"]').click(function(e){
            e.preventDefault();
            $('#imgDemo').show();
            if($(this).attr('href')=="showDemo01"){
                $('#imgNow').attr('src','/ImagesNew/single.png');
            }else if($(this).attr('href')=="showDemo02"){
                $('#imgNow').attr('src','/ImagesNew/more.png');
            }else if($(this).attr('href')=="showDemo03"){
                $('#imgNow').attr('src','/ImagesNew/more.png');
            }else if($(this).attr('href')=="showDemo04"){
                $('#imgNow').attr('src','/ImagesNew/more.png');
            }
        });
        $('#imgDemo').click(function(){
            $('#imgDemo').hide();
        });
    }

    new AddEditPublish();

    //上传图片  缩略图
    function AllImgInitShow() {
        /*  第一个参数是 点击上传传的ID   第二个参数是小图  第三个参数是提示  第四个参数是大图 */
        config.Details.forEach(function (item,j) {
            for(var i=0;i<=2;i++){
                var z = i + j * 3;
                uploadImg("headUploadify"+z,"headimgUploadFile"+z,"headimgErr"+z,"bigImgshow"+z,'');
            }
        })
    }

    /*  第一个参数是 点击上传传的ID
        第二个参数是小图
        第三个参数是提示
        第四个参数是大图 */

    function uploadImg(id,img,imgerr,bigImg) {
        jQuery.extend({
            evalJSON: function (strJson) {
                if ($.trim(strJson) == '')
                    return '';
                else
                    return eval("(" + strJson + ")");
            }
        });
        function getCookie(name) {
            var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
            if (arr = document.cookie.match(reg))
                return unescape(arr[2]);
            else
                return null;
        }
        function escapeStr(str) {
            return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
        }

        $(document).ready(function (){
            $('#'+id).uploadify({
                'auto': true,
                'multi': false,
                'swf': '/Js/uploadify.swf?_=' + Math.random(),
                'uploader': '/AjaxServers/UploadFile.ashx',
                'buttonText': '+',
                'buttonClass': 'allBtn_file',
                'width': 60,
                'height': 60,
                'fileTypeDesc': '支持格式:xls,xlsx,jpg,jpeg,png.gif,zip,pdf,ppt,pptx,mp4',
                'fileTypeExts': '*.xls;*.xlsx;*.jpg;*.jpeg;*.png;*.gif;*.zip;*.pdf;*.ppt;*.pptx;*.mp4',
                'fileSizeLimit':'5MB',
                'fileCount':1,
                'queueSizeLimit': 1,
                queueID:'imgShow',
                'scriptAccess': 'always',
                'overrideEvents' : [ 'onDialogClose'],
                'formData': { Action: 'BatchImport', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')), IsGenSmallImage: 1 },
                'onQueueComplete': function (event, data) {},
                'onQueueFull': function () {
                    alert('您最多只能上传1个文件！');
                    return false;
                },
                //检测FLASH失败调用
                'onFallback':function(){
                    $('#'+imgerr).html('您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。');
                },
                //上传成功后返回的信息
                'onUploadSuccess': function (file, data, response) {
                    if (response == true) {
                        var json = $.evalJSON(data);
                        var imgUrlsTotal = json.Msg.split('|');
                        var smallImgUrl = imgUrlsTotal[1];
                        var bigImgUrl = imgUrlsTotal[0];

                        //小图
                        $('#'+img).attr('src',"" + smallImgUrl);
                        //大图
                        $('#'+bigImg).find('img').attr("src","" +bigImgUrl);
                        //上传成功的提示信息
                        $("#"+imgerr).html('上传成功！').css('color',"#666");
                        //上传成功后警告信息隐藏
                        $("#"+imgerr).next().css("display","none");
                        //鼠标放过显示大图
                        $('#'+id).mousemove(function () {
                            $('#'+bigImg).show();
                        }).mouseout(function () {
                            $('#'+bigImg).hide();
                        })
                        //AE下的最后一个上传附件  显示名称
                        if(id == 'lastFileSrc'){
                            $('.lastFileUrl').show();
                            $('.lastFileName').text(json.FileName);
                            $('.lastFileUrl').attr('href',"" + json.Msg.split('|')[0]);
                            $('#lastFileSrc').parent().next().hide();
                        }
                    }
                },
                'onProgress': function (event, queueID, fileObj, data) { },
                'onUploadError': function (event, queueID, fileObj, errorObj) {},
                'onSelectError':function(file, errorCode, errorMsg){
                    if (errorCode == SWFUpload.UPLOAD_ERROR.FILE_CANCELLED
                        || errorCode == SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED) {
                        return false;
                    }
                    switch(errorCode) {
                        case -100:
                            $('#'+imgerr).html('<img src="/images/icon21.png">上传图片数量超过1个').css('color',"red");
                            break;
                        case -110:
                            $('#'+imgerr).html('<img src="/images/icon21.png">上传图片大小应小于2MB').css('color',"red");
                            break;
                        case -120:

                            break;
                        case -130:
                            $('#'+imgerr).html('<img src="/images/icon21.png">上传图片类型不正确').css('color',"red");
                            break;
                    }
                }
            });
        });
    }

    //----------------只能输入数字 Start
    /*调用    '/限制的正则表达式/g'   必须以/开头，/g结尾
     *  onkeyup="replaceAndSetPos(this,/[^0-9]/g,'')" oninput ="replaceAndSetPos(this,/[^0-9]/g,'')"
     */
    //获取光标位置
    function getCursorPos(obj) {
        var CaretPos = 0;
        // IE Support
        if (document.selection) {
            obj.focus (); //获取光标位置函数
            var Sel = document.selection.createRange ();
            Sel.moveStart ('character', -obj.value.length);
            CaretPos = Sel.text.length;
        }
        // Firefox/Safari/Chrome/Opera support
        else if (obj.selectionStart || obj.selectionStart == '0')
            CaretPos = obj.selectionEnd;
        return (CaretPos);
    };
    //定位光标
    function setCursorPos(obj,pos){

        if (obj.setSelectionRange) { //Firefox/Safari/Chrome/Opera
            obj.focus(); //
            obj.setSelectionRange(pos,pos);
        } else if (obj.createTextRange) { // IE
            var range = obj.createTextRange();
            range.collapse(true);
            range.moveEnd('character', pos);
            range.moveStart('character', pos);
            range.select();
        }
    };
    //替换后定位光标在原处,可以这样调用onkeyup=replaceAndSetPos(this,/[^/d]/g,'');
    function replaceAndSetPos(obj,pattern,text){
        if ($(obj).val() == "" || $(obj).val() == null) {
            return;
        }
        var pos=getCursorPos(obj);//保存原始光标位置
        var temp=$(obj).val(); //保存原始值
        obj.value=temp.replace(pattern,text);//替换掉非法值
        //截掉超过长度限制的字串（此方法要求已设定元素的maxlength属性值）
        var max_length = obj.getAttribute? parseInt(obj.getAttribute("maxlength")) : "";
        if( obj.value.length > max_length){
            var str1 = obj.value.substring( 0,pos-1 );
            var str2 = obj.value.substring( pos,max_length+1 );
            obj.value = str1 + str2;
        }
        pos=pos-(temp.length-obj.value.length);//当前光标位置
        setCursorPos(obj,pos);//设置光标
        //el.onkeydown = null;
    };
    //-----------------只能输入数字 end


    //获取url 地址参数方法
    function GetQueryString(name) {
        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if(r!=null)return unescape(r[2]); return null;
    }

})
