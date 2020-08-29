/**
 * Created by fengb on 2017/6/5.
 */

$(function () {

    var RoleIDs = CTLogin.RoleIDs;
    var config = {};

    var PubID = GetQueryString('PubID')!=null&&GetQueryString('PubID')!='undefined'?parseFloat  (GetQueryString('PubID')):null;
    var MediaID = GetQueryString('MediaID')!=null&&GetQueryString('MediaID')!='undefined'?parseFloat  (GetQueryString('MediaID')):null;
    var TemplateID = GetQueryString('TemplateID')!=null&&GetQueryString('TemplateID')!='undefined'?parseFloat  (GetQueryString('TemplateID')):null;

    var isCPD = false;

    //媒体ID是必填项  刊例ID和模板ID不确定的时候传0

    var purchaseDiscounting = null;//采购折扣
    var salesDiscount = null;//销售折扣

    var allPriceObj = {};//价格对象    每个对象对应的key才是一个相应的数组
    var cur_objId = {};//广告价格的Id

    var bigArr = [];//存有所有ID的大数租

    var CPMBArr = [];//CPM的大数组

    var addPublishApp = {
        constructor : addPublishApp,
        init : function () {
            //查看广告详情
            //var url = 'js/addApp.json';
            var url = '/api/Publish/GetADDetail?v=1_1';
             setAjax({
                 url : url,
                 type : 'get',
                 data : {
                     MediaType : 14002,
                     MediaID : MediaID,
                     PubID : PubID,
                     TemplateID : TemplateID
                 }
             },function (data) {
                 if(data.Status == 0){
                     var data = data.Result;
                     if(data == null){
                         layer.alert('传入的参数有误');
                         window.location = '/publishmanager/advertisinglist_app.html';
                     }else{
                         //获取媒体名称和媒体logo给全局变量config
                         config.MediaName = data.TemplateList[0].MediaName;
                         config.MediaLogo = data.TemplateList[0].MediaLogo;
                         config.TemplateList = data.TemplateList;//模板数组
                         config.PublishBasicInfo = data.PublishBasicInfo;//刊例信息
                         config.BaseMediaID = data.BaseMediaID;//用于修改模板的时候

                         for(var i = 0;i<config.TemplateList.length;i++){
                             bigArr.push({'pid':[]});
                             CPMBArr.push({'isdiffer':[]});
                         }
                         addPublishApp.showTemplate(config);
                     }
                 }
             })
        },
        showTemplate : function (data) {//渲染数据
            $('.order_r').html(ejs.render($('#AddTemplate').html(), data));
            addPublishApp.getInitFun();
        },
        getInitFun : function () {//初始化调用
            addPublishApp.allUploadInit();
            addPublishApp.leftListTab();
            addPublishApp.checkOfDate();
            addPublishApp.checkOfPrice();
            addPublishApp.LinksToJump();

            $(".price_data").find("input[name=Username]").each(function(){
                $(this).on("input",function(){
                    replaceAndSetPos(this,/[^0-9]/g,'');
                })
            });
        },
        LinksToJump : function () {//链接跳转
            //新增模板
            var BaseMediaID = config.BaseMediaID;//基表的ID
            $('.addTemplate').on('click',function () {
                var MediaName = $(this).attr('MediaName');
                $(this).attr('href','/PublishManager/add_template.html?BaseMediaID='+BaseMediaID+'&MediaID='+MediaID+'&AppName='+MediaName);
            })
        },
        allUploadInit : function () {//上传附件
            config.TemplateList.forEach(function (item,j) {
                uploadFile('KanLiManuscript'+j);
            })
        },
        leftListTab : function () {//模板菜单切换

            //初始化  模板ID为0 的情况下默认渲染第一个
            if(TemplateID == 0){

                var idx = 0;
                publicChange(idx);

                allPriceObj[idx] = [];//价格对象
                cur_objId[idx] = [];//价格对象的对应的ID

            }else{//知道具体的模板的情况下 选中具体的左侧菜单和右侧的内容

                var TemplateIDArr = [];
                $('#menuTemplates li').each(function () {
                    var that = $(this);
                    var TemplateID = that.attr('TemplateID')*1;
                    TemplateIDArr.push(TemplateID);
                })
                var idx = TemplateIDArr.indexOf(TemplateID);


                allPriceObj[idx] = [];//价格对象
                cur_objId[idx] = [];//价格对象的对应的ID


                var rightContent = $('.ad_right').find('.right_content').eq(idx);

                publicChange(idx);

                var url = '/api/Publish/GetPubDateList?v=1_1';
                setAjax({
                    url : url,
                    type : 'get',
                    data : {
                        'TemplateID' : TemplateID,
                        'MediaID' : MediaID
                    }
                },function (data) {
                    if(data.Result != ''){
                        $('.notOneData').show();
                        $('.notOneDataSelect').parent().show();
                        var str = '';
                        data.Result.forEach(function (item) {
                            //console.log(item);
                            var BeginTime = item.BeginTime;
                            var EndTime = item.EndTime;
                            str += '<option FileName='+item.FileName+' PubFileUrl='+item.PubFileUrl+'>' + BeginTime + "至" +  EndTime +'</option>';
                        })
                        rightContent.find('.add_ad ul .notOneDataSelect').append(str);
                    }else{
                        $('.notOneData').hide();
                        $('.notOneDataSelect').parent().hide();
                    }
                })

            }

            //切换的时候
            $('#menuTemplates>li').off('click').on('click',function (e) {
                e.stopPropagation();
                e.preventDefault();

                var idx = $(this).index();
                var rightContent = $('.ad_right').find('.right_content').eq(idx);

                publicChange(idx);

                var curTemplateID = $(this).attr('TemplateID');
                rightContent.find('.add_ad ul .notOneDataSelect').html('<option>请选择</option>');

                var url = '/api/Publish/GetPubDateList?v=1_1';
                setAjax({
                    url : url,
                    type : 'get',
                    data : {
                        'TemplateID' : curTemplateID,
                        'MediaID' : MediaID
                    }
                },function (data) {
                    if(data.Result != ''){
                        $('.notOneData').show();
                        $('.notOneDataSelect').parent().show();
                        var str = '';
                        data.Result.forEach(function (item) {
                            var BeginTime = item.BeginTime;
                            var EndTime = item.EndTime;
                            str += '<option FileName='+item.FileName+' PubFileUrl='+item.PubFileUrl+'>' + BeginTime + "至" +  EndTime +'</option>';
                        })
                        rightContent.find('.add_ad ul .notOneDataSelect').append(str);
                    }else{
                        $('.notOneData').hide();
                        $('.notOneDataSelect').parent().hide();
                    }
                })

            })

            //删除模板菜单和模板右侧大块内容
            $('#menuTemplates .DeleteRejected').each(function () {
                $(this).off('click').on('click',function (e) {
                    e.stopPropagation();
                    e.preventDefault();
                    var that = $(this);
                    var idx = that.parents('li').index();//索引从0开始
                    var isChecked = that.parents('li').attr('class');//是否为选中状态
                    var TemplateID = that.parents('li').attr('TemplateID');
                    var url = '/api/Template/ToDeleteAppTemplate';

                    if(isChecked == '' || isChecked == undefined){//不选中的情况
                        layer.confirm('确认要删除模板吗', {
                            btn: ['确认','取消'] //按钮
                        }, function(){
                            layer.closeAll();
                            setAjax({
                                url : url,
                                type : 'get',
                                data : {
                                    TemplateID : TemplateID
                                }
                            },function (data) {
                                if(data.Status == 0){
                                    //layer.msg(data.Message,{time:1000});
                                    that.parents('li').remove();
                                    $('.ad_right').find('.right_content').eq(idx).remove();
                                }else{
                                    layer.msg(data.Message,{time:1000});
                                }
                            })
                        })
                    }else{//选中的情况
                        layer.confirm('确认要删除模板吗', {
                            btn: ['确认','取消'] //按钮
                        }, function(){
                            layer.closeAll();
                            setAjax({
                                url : url,
                                type : 'get',
                                data : {
                                    TemplateID : TemplateID
                                }
                            },function (data) {
                                if(data.Status == 0){
                                    //layer.msg(data.Message,{time:1000});
                                    that.parents('li').remove();
                                    $('.ad_right').find('.right_content').eq(idx).remove();
                                    $('#menuTemplates>li').eq(0).addClass('current').siblings().removeClass('current');
                                    $('.ad_right').find('.right_content').eq(0).show().siblings().hide();
                                }else{
                                    layer.msg(data.Message,{time:1000});
                                }
                            })
                        })
                    }
                })
            })


            //当切换或者默认选中的时候  对应的样式或者右侧的大box 发生相应的变化或者改变
            function publicChange(idx) {

                var idx = idx;
                $('#menuTemplates>li').eq(idx).addClass('current').siblings().removeClass('current');
                $('.ad_right').find('.right_content').eq(idx).show().siblings().hide();

                var rightContent = $('.ad_right').find('.right_content').eq(idx);
                var holidays = JSON.parse(rightContent.find('.holidays').attr('holidays'));
                var CPDCPM = [];
                if(holidays == null){
                    CPDCPM = [];
                }else{
                    holidays.forEach(function (item) {
                        CPDCPM.push(item.Name);
                    })
                }

                addPublishApp.addPricePopup(CPDCPM,idx);
                var BaseMediaID = config.BaseMediaID;//基表的ID

                // 查看模板  修改模板
                var AuditStatusName = rightContent.attr('AuditStatusName');
                if(AuditStatusName == '待审核'){
                    //待审核没有修改模板的入口
                    rightContent.find('.EditTemplate').hide();
                    rightContent.find('.ViewDetails').on('click',function () {
                        var TemplateID = rightContent.attr('TemplateID');
                        $(this).attr('href','/publishmanager/advTempDetail.html?AdTempId='+TemplateID);
                    })

                    //待审核和驳回不能添加广告
                    rightContent.find('.add_ad').hide();
                    rightContent.find('.noData_img').hide();
                    rightContent.find('.noReviewTem').show();

                }else if(AuditStatusName == '已通过'){
                    rightContent.find('.ViewDetails').on('click',function () {
                        var TemplateID = rightContent.attr('TemplateID');
                        $(this).attr('href','/publishmanager/advTempDetail.html?AdTempId='+TemplateID+'&AdBaseTempId='+TemplateID);
                    })
                    //找到已通过的左侧菜单
                    var curMenu = $('#menuTemplates>li').eq(idx).find('span:eq(0)').text();
                    console.log(curMenu);
                    //已通过的修改模板   要掉接口提示
                    rightContent.find('.EditTemplate').on('click',function () {
                        var TemplateID = rightContent.attr('TemplateID');
                        var that = $(this);
                        /*//获取左侧所有的菜单
                        var TemplateNameArr = [];
                        $('#menuTemplates>li').each(function () {
                            var that = $(this);
                            var val = $.trim(that.find('span:eq(0)').text());
                            TemplateNameArr.push(val);
                        })
                        TemplateNameArr.splice(idx,1);//把左侧选中的当前项删除*/
                        //console.log(TemplateNameArr);
                        /*if(TemplateNameArr.indexOf(curMenu) != -1){
                            layer.msg('您已对该模板进行了修改，请勿重复操作',{time:2000});
                        }else{
                            $(this).attr('target','_blank');
                            $(this).attr('href','/publishmanager/edit_template.html?BaseMediaID='+BaseMediaID+'&MediaID='+MediaID +'&AdBaseTempId='+TemplateID+'&PubID=0');
                        }*/
                        setAjax({
                            url : '/api/Template/CheckCanAddModifyTemplate',
                            type : 'get',
                            data : {
                                'baseTemplateID' : TemplateID
                            }
                        },function(data){
                            if(data.Status == 0){
                                if(data.Result == true){
                                    window.location = '/publishmanager/edit_template.html?BaseMediaID='+BaseMediaID+'&MediaID='+MediaID +'&AdBaseTempId='+TemplateID+'&PubID=0';
                                    //that.attr('target','_blank');
                                    //that.attr('href','/publishmanager/edit_template.html?BaseMediaID='+BaseMediaID+'&MediaID='+MediaID +'&AdBaseTempId='+TemplateID+'&PubID=0');
                                }else{
                                    layer.msg('您已对该模板进行了修改，请勿重复操作',{time:1000});
                                }
                            }else{
                                layer.msg(data.Result,{'time':1000});
                            }
                        })
                    })


                }else {//已驳回
                    rightContent.find('.ViewDetails').on('click',function () {
                        var TemplateID = rightContent.attr('TemplateID');
                        $(this).attr('href','/publishmanager/advTempDetail.html?AdTempId='+TemplateID);
                    })
                    //已驳回修改模板
                    rightContent.find('.EditTemplate').on('click',function () {
                        var TemplateID = rightContent.attr('TemplateID');
                        $(this).attr('target','_blank');
                        $(this).attr('href','/publishmanager/edit_template.html?BaseMediaID='+BaseMediaID+'&MediaID='+MediaID+'&TemplateID='+TemplateID+'&PubID=0');
                    })

                    //待审核和驳回不能添加广告
                    rightContent.find('.add_ad').hide();
                    rightContent.find('.noData_img').hide();
                    rightContent.find('.noReviewTem').show();

                }
            }
        },
        checkOfDate : function () {//日期验证
            config.TemplateList.forEach(function (item,j) {
                var startTime = 'startTime'+j;
                var endTime = 'endTime'+j;

                $('#'+ startTime).off("click").on("click", function () {
                    laydate({
                        elem: '#'+ startTime,
                        fixed: false,
                        choose: function (date) {
                            if(date>$(".install").find('#'+ endTime).val() && $(".install").find('#'+ endTime).val()){
                                layer.alert('起始时间不能大于结束时间！');
                                $(".install").find('#'+ startTime).val('');
                            }
                            $('.hasTheDate').val('请选择');
                            $('.uploadShow').hide();
                        }
                    });
                });
                $('#'+ endTime).off("click").on("click", function () {
                    laydate({
                        elem: '#'+ endTime,
                        fixed: false,
                        min : laydate.now(),
                        choose: function (date) {
                            if(date<$(".install").find('#'+ startTime).val() && $(".install").find('#'+ startTime).val()){
                                layer.alert('结束时间不能小于起始时间！');
                                $(".install").find('#'+ endTime).val('');
                            }
                            $('.hasTheDate').val('请选择');
                            $('.uploadShow').hide();
                        }
                    });
                });

                //已有有效期选择自动更新到前面的日期框里面
                $('.hasTheDate').on('change',function () {
                    var that = $(this);
                    var txt = that.find('option:checked').val();
                    var FileName = that.find('option:checked').attr('FileName');
                    var PubFileUrl = that.find('option:checked').attr('PubFileUrl');
                    if(txt != '请选择'){
                        var beginDate = txt.split('至')[0];
                        var endDate = txt.split('至')[1];
                        $(this).parent().siblings('.hanDate').find('#'+ startTime).val(beginDate);
                        $(this).parent().siblings('.hanDate').find('#'+ endTime).val(endDate);
                        $(this).parents('.add_ad').find('.uploadShow').show();
                        $(this).parents('.add_ad').find('.uploadShow .uploadName').text(FileName);
                        $(this).parents('.add_ad').find('.uploadShow .uploadFile').attr('href',PubFileUrl);
                        //$(this).parents('.add_ad').find('.uploadify').attr('buttonText','重新上传');
                    }else{
                        $(this).parent().siblings('.hanDate').find('#'+ startTime).val('');
                        $(this).parent().siblings('.hanDate').find('#'+ endTime).val('');
                        $(this).parents('.add_ad').find('.uploadShow').hide();
                        $(this).parents('.add_ad').find('.uploadShow .uploadName').text('');
                        $(this).parents('.add_ad').find('.uploadShow .uploadFile').attr('href','javascript:;');
                    }
                })

            })
        },
        checkOfPrice : function () {//折扣验证

            var reg = /^(\d{1,2}(\.\d{1,3})?|100|100\.0|100\.00)$/;//折扣验证
            purchaseDiscounting = $('.purchaseDiscounting').val();//采购折扣
            salesDiscount = $('.salesDiscount').val();//销售折扣

            if(purchaseDiscounting == undefined && salesDiscount == undefined){//媒体主角色下 木有折扣
                purchaseDiscounting = 100;//默认传100 也就是1
                salesDiscount = 100;
            }else{//AE角色   折扣存在的情况

                //采购折扣验证
                $(".purchaseDiscounting").on("blur",function () {
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

                // 销售折扣验证
                $(".salesDiscount").on("blur",function () {
                    sales = $(this).val()*1;
                    purchase = $('.purchaseDiscounting').val()*1;
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
            }
        },
        addPricePopup : function (CPDCPM,idxIndex) {//添加广告的弹层  区分节假日

            var rightContent = $('.ad_right').find('.right_content').eq(idxIndex);

            if(rightContent.find('.difHolidays').prop('checked')){
                isCPD = true;
            }else{
                isCPD = false;
            }

            rightContent.find('.difHolidays').on('change',function(){
                if(rightContent.find('.difHolidays').prop('checked')){
                    isCPD = true;
                }else{
                    isCPD = false;
                }
            })


            if(CPDCPM.indexOf('CPD') != -1){//有CPM的话则正常显示
                $('.difHolidays').removeAttr('disabled');
                $('.difHolidays').next().css('color','');

                $('.difHolidays').on('click',function () {
                    if($(this).prop('checked')){
                        isCPD = true;
                    }else{
                        isCPD = false;
                    }
                })
            }else{//木有的话则不能点击
                $('.difHolidays').attr('disabled',true);
                $('.difHolidays').next().css('color','#ccc');
                isCPD = false;
            }

            $('.purchaseDiscounting').on('change',function () {
                purchaseDiscounting = $.trim($(this).val())*1/100;//采购折扣
                changeAutoPrice('.ad_table',purchaseDiscounting,salesDiscount,'isCPD');
            })

            $('.salesDiscount').on('change',function () {
                salesDiscount = $.trim($(this).val())*1/100;//销售折扣
                changeAutoPrice('.ad_table',purchaseDiscounting,salesDiscount,'isCPD');
            })

            config.TemplateList.forEach(function (item,j) {
                var addAdvertisingPrice = 'addAdvertisingPrice'+j;
                //添加广告价格
                $('.'+ addAdvertisingPrice).off('click').on('click',function (event) {
                    event.preventDefault();
                    var that = $(this);
                    var reg = /^(\d{1,2}(\.\d{1,3})?|100|100\.0|100\.00)$/;//折扣验证
                    var idx = that.attr('class').split(' ')[1].substr(19,20);
                    var startTimeTxt = 'startTime'+ idx;
                    var endTimeTxt = 'endTime'+ idx;
                    var startTime = $('#'+startTimeTxt).val();//开始日期
                    var endTime = $('#'+endTimeTxt).val();//结束日期

                    //当前的元素   大盒子
                    var cur_content = $('.ad_right .right_content').eq(idx);

                    $('#'+startTimeTxt).on('change',function () {
                        startTime = $(this).val();
                    })
                    $('#'+endTimeTxt).on('change',function () {
                        endTime = $(this).val();
                    })

                    //console.log(!reg.test(salesDiscount*1000/10))
                    //console.log(!reg.test(purchaseDiscounting*1000/10))


                    if(RoleIDs == 'SYS001RL00003'){//媒体主
                        if(startTime == ''){
                            $('.errorTit').show();
                            $('.errorTit').find('span').text('请输入价格有效期');
                        }else if(endTime == ''){
                            $('.errorTit').show();
                            $('.errorTit').find('span').text('请输入价格有效期');
                        }else {
                            $('.errorTit').hide();
                            openPopup();
                        }
                    }else if(RoleIDs == 'SYS001RL00005'){//AE
                        if(startTime == ''){
                            $('.errorTit').show();
                            $('.errorTit').find('span').text('请输入价格有效期');
                        }else if(endTime == ''){
                            $('.errorTit').show();
                            $('.errorTit').find('span').text('请输入价格有效期');
                        }else if(purchaseDiscounting == ''){
                            $('.errorTit').show();
                            $('.errorTit').find('span').text('请输入采购折扣');
                        }else if(salesDiscount == ''){
                            $('.errorTit').show();
                            $('.errorTit').find('span').text('请输入销售折扣');
                        }else if(salesDiscount < purchaseDiscounting){
                            $('.errorTit').show();
                            $('.errorTit').find('span').text('销售折扣不能小于采购折扣');
                        }else if(!reg.test(salesDiscount*1000/10) || !reg.test(purchaseDiscounting*1000/10)){
                            $('.errorTit').show();
                            $('.errorTit').find('span').text('请输入0〜100之间的整数或小数(保留三位)');
                        }else{
                            $('.errorTit').hide();
                            openPopup();
                        }
                    }

                    //出现弹层
                    function openPopup() {
                        var newUrl = "/PublishManager/addPublishForApp.html?MediaID="+MediaID+"&PubID="+PubID+"&TemplateID="+TemplateID+"&numberIdx="+idx+"&isCPD="+isCPD ;
                        history.pushState('','',newUrl);
                        $.openPopupLayer({
                            name : "popLayerDemo",
                            url : "./publishApp-popup.html?numberIdx="+idx,
                            success : function () {
                                //输入框正确格式验证  只能输入正整数
                                $(".add_ad").find("input[name=Username]").each(function(){
                                    $(this).on("input",function(){
                                        replaceAndSetPos(this,/[^0-9]/g,'');
                                    })
                                });

                                //弹层里面的一系列价格的计算  layer_con3
                                if(isCPD == true){// 工作日节假日
                                    changeAutoPrice('.layer_con3',purchaseDiscounting,salesDiscount,'isCPD');
                                }else{  //只有工作日的情况
                                    changeAutoPrice('.layer_con3',purchaseDiscounting,salesDiscount,'isCPM');
                                }

                                //点击保存的时候的一系列的验证
                                $('#safeADPrice').off('click').on('click',function (event) {
                                    event.preventDefault();
                                    //广告形式
                                    var AdStyle = $('input:radio[name=AdStyle]:checked').val();
                                    var AdStyleTxt = $('input:radio[name=AdStyle]:checked').next().text();
                                    //轮播的id和值是一样的
                                    var CarouselCount = $('input:radio[name=CarouselCount]:checked').val();
                                    //售卖平台
                                    var SellingPlatform = $('input:radio[name=platform]:checked').val();
                                    var SellingPlatformTxt = $('input:radio[name=platform]:checked').next().text();

                                    //售卖方式
                                    var SellingMode = $('input:radio[name=saleStyle]:checked').val();
                                    var SellingModeTxt = $('input:radio[name=saleStyle]:checked').next().text();

                                    //售卖区域
                                    var AdGroup = $('input:radio[name=saleArea]:checked').val();
                                    var AdGroupTxt = $('input:radio[name=saleArea]:checked').next().text();

                                    var PubPrice = $('.layer .PubPrice').val();//刊例价
                                    var SalePrice = $('.layer .SalePrice').attr('money');//销售价
                                    var PubPriceHoliday = $('.layer .PubPrice_Holiday').val();//节假日刊例价
                                    var SalePriceHoliday = $('.layer .SalePrice_Holiday').attr('money');//节假日日销售价
                                    var clickRate = $('.layer .clickRate').val();//点击量
                                    var quantityRate = $('.layer .quantityRate').val();//曝光量

                                    //针对每输入一次的弹层里面的数据都自带一个属于他们的ID  ID是由所有的广告单元字符串拼接到一起的
                                    var PriceID = AdStyle + CarouselCount + SellingPlatform + SellingMode + AdGroup ;

                                    //区分节假日 一些列的验证
                                    if(isCPD == true){

                                        if(AdStyle == undefined){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请选择广告形式');
                                        }else if(CarouselCount == undefined){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请选择轮播数');
                                        }else if(SellingPlatform == undefined){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请选择售卖平台');
                                        }else if(SellingMode == undefined){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请选择售卖方式');
                                        }else if(AdGroup == undefined){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请选择售卖区域');
                                        }else if(PubPrice == ''){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请输入工作日刊例价格');
                                        }else if(PubPriceHoliday == ''){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请输入节假日刊例价格');
                                        }else if(SalePrice == ''){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请输入工作日销售价格');
                                        }else if(SalePriceHoliday == ''){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请输入节假日销售价格');
                                        }else if(bigArr[idx]['pid'].indexOf(PriceID) != -1){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('不能重复添加广告单元');
                                        }else{
                                            $('.errorMeg').hide();
                                            var priceObj = {
                                                'isCPD' : true,
                                                'PriceID' : PriceID,
                                                'AdStyleList' : {
                                                    'ID' : AdStyle,
                                                    'Name' : AdStyleTxt
                                                },
                                                'CarouselCount' : CarouselCount,
                                                'SellingPlatformList' : {
                                                    'ID' : SellingPlatform,
                                                    'Name' : SellingPlatformTxt
                                                },
                                                'SellingModeList' : {
                                                    'ID' : SellingMode,
                                                    'Name' : SellingModeTxt
                                                },
                                                'AdGroupList' : {
                                                    'ID' : AdGroup,
                                                    'Name' : AdGroupTxt
                                                },
                                                'PubPrice' : PubPrice,
                                                'PubPrice_Holiday' : PubPriceHoliday,
                                                'SalePrice' : SalePrice,
                                                'SalePrice_Holiday' : SalePriceHoliday,
                                                'clickRate' : clickRate,
                                                'quantityRate' : quantityRate
                                            }

                                            bigArr[idx]['pid'].push(priceObj.PriceID);//不能重复的ID
                                            CPMBArr[idx]['isdiffer'].push(SellingMode);//存在有CPM

                                            $.closePopupLayer('popLayerDemo');
                                            //当里面什么都没有的时候直接在tbody里面追加   当里面有的时候每次在最后一个tr的后面追加
                                            var trIdx = cur_content.find('.price_data tbody tr').length;
                                            if(trIdx == 0){
                                                cur_content.find('.price_data>img').hide();
                                                cur_content.find('.price_data table').show();
                                                cur_content.find('.price_data table tbody').html(ejs.render($('#TableISCPD').html(),{data:priceObj}));
                                            }else{
                                                cur_content.find('.price_data>img').hide();
                                                cur_content.find('.price_data table').show();
                                                cur_content.find('.price_data table tbody tr:last').after(ejs.render($('#TableISCPD').html(),{data:priceObj}));
                                            }
                                            cur_content.find('.keep').show();
                                            //渲染后表格里面的价钱计算
                                            changeAutoPrice('.ad_table',purchaseDiscounting,salesDiscount,'isCPD');
                                        }

                                    }else{

                                        if(AdStyle == undefined){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请选择广告形式');
                                        }else if(CarouselCount == undefined){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请选择轮播数');
                                        }else if(SellingPlatform == undefined){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请选择售卖平台');
                                        }else if(SellingMode == undefined){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请选择售卖方式');
                                        }else if(AdGroup == undefined){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请选择售卖区域');
                                        }else if(PubPrice == ''){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请输入刊例价格');
                                        }else if(SalePrice == ''){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('请输入销售价格');
                                        }else if(bigArr[idx]['pid'].indexOf(PriceID) != -1){
                                            $('.layer_con3 .errorMeg').show();
                                            $('.layer_con3 .errorMeg').find('span').text('不能重复添加广告单元');
                                        }else{
                                            $('.layer_con3 .errorMeg').hide();
                                            var priceObj = {
                                                'isCPD' : false,
                                                'PriceID' : PriceID,
                                                'AdStyleList' : {
                                                    'ID' : AdStyle,
                                                    'Name' : AdStyleTxt
                                                },
                                                'CarouselCount' : CarouselCount,
                                                'SellingPlatformList' : {
                                                    'ID' : SellingPlatform,
                                                    'Name' : SellingPlatformTxt
                                                },
                                                'SellingModeList' : {
                                                    'ID' : SellingMode,
                                                    'Name' : SellingModeTxt
                                                },
                                                'AdGroupList' : {
                                                    'ID' : AdGroup,
                                                    'Name' : AdGroupTxt
                                                },
                                                'PubPrice' : PubPrice,
                                                'SalePrice' : SalePrice,
                                                'clickRate' : clickRate,
                                                'quantityRate' : quantityRate,
                                                'PubPrice_Holiday' : '',
                                                'SalePrice_Holiday' : ''
                                            }

                                            bigArr[idx]['pid'].push(priceObj.PriceID);//不能重复的ID
                                            CPMBArr[idx]['isdiffer'].push(SellingMode);//存在有CPM

                                            console.log(priceObj)

                                            $.closePopupLayer('popLayerDemo');
                                            //当里面什么都没有的时候直接在tbody里面追加   当里面有的时候每次在最后一个tr的后面追加
                                            var trIdx = cur_content.find('.price_data tbody tr').length;
                                            if(trIdx == 0){
                                                cur_content.find('.price_data>img').hide();
                                                cur_content.find('.price_data table').show();
                                                cur_content.find('.price_data table tbody').html(ejs.render($('#TableISCPD').html(),{data:priceObj}));
                                            }else{
                                                cur_content.find('.price_data>img').hide();
                                                cur_content.find('.price_data table').show();
                                                cur_content.find('.price_data table tbody tr:last').after(ejs.render($('#TableISCPD').html(),{data:priceObj}));
                                            }
                                            cur_content.find('.keep').show();
                                            //渲染后表格里面的价钱计算
                                            changeAutoPrice('.ad_table',purchaseDiscounting,salesDiscount,'isCPM');
                                        }
                                    }

                                    //渲染完数据之后  如果手动修改的话则对应的不区分的文案变成红边的input框
                                    cur_content.find('.difHolidays').on('change',function(){
                                        console.log('变');
                                        var that = $(this);

                                        console.log(CPMBArr[idx]['isdiffer']);

                                        //则不能让它勾选  并且提示用户
                                        if(isCon(CPMBArr[idx]['isdiffer'],'11002')){//存在CPM
                                            layer.msg('售卖方式中有CPM，不可区分节假日',{'time':1000});
                                            that.prop('checked',false);
                                        }

                                        if(that.prop('checked')){
                                            isCPD = true;
                                            cur_content.find('.no_differ').each(function () {
                                                var that = $(this);
                                                that.find('span').hide();
                                                that.find('input').show();
                                            })
                                            //渲染后表格里面的价钱计算
                                            changeAutoPrice('.ad_table',purchaseDiscounting,salesDiscount,'isCPD');
                                        }else{
                                            isCPD = false;
                                            cur_content.find('.no_differ').each(function () {
                                                var that = $(this);
                                                that.find('span').show();
                                                that.find('input').hide();
                                            })
                                            //渲染后表格里面的价钱计算
                                            changeAutoPrice('.ad_table',purchaseDiscounting,salesDiscount,'isCPM');
                                        }
                                    })

                                    //添加完弹层之后列表数据还是可以更改的 那就当更改的时候从新赋值给每一行的变量
                                    var curTable = $('.ad_right .right_content').eq(idx).find('.price_data');

                                    //点击量更改
                                    curTable.find('table .clickRate').on('change',function () {
                                        var that = $(this);
                                        that.parents('tr').attr('ClickCount',that.val());
                                    })
                                    //曝光量
                                    curTable.find('table .quantityRate').on('change',function () {
                                        var that = $(this);
                                        that.parents('tr').attr('ExposureCount',that.val());
                                    })

                                    addPublishApp.deletePriceData(CPMBArr[idx]['isdiffer'],idx);//删除操作
                                    addPublishApp.safaPriceAllData(idxIndex);//保存数据
                                })

                                //关闭弹层
                                $('.closePopup').off('click').on('click',function (event) {
                                    event.preventDefault();
                                    $.closePopupLayer('popLayerDemo');
                                })
                            }
                        })
                    }
                })
            })
        },
        deletePriceData : function(newArr,curIndex){//删除数据
            $('.delete_data').each(function(){
                var that = $(this);
                that.off('click').on('click',function(event){
                    event.preventDefault();
                    var idx = that.parents('tbody tr').index();
                    var len = that.parents('tbody').find('tr').length;
                    if(len <= 1){
                        layer.msg('至少要保留一个广告位',{time:1000});
                    }else{
                        layer.confirm('确认要删除数据吗', {
                            btn: ['确认','取消'] //按钮
                        }, function(){
                            layer.closeAll();
                            newArr.splice(idx,1);
                            //console.log(newArr);
                            bigArr[curIndex]['pid'].splice(idx,1);
                            //console.log(bigArr[curIndex]['pid']);
                            that.parents('tr').remove();
                        })
                    }
                })
            })
        },
        safaPriceAllData : function(idxIndex){//保存价格的数据  将渲染到页面中的tab每一行的数据传给后台

            var cur = $('.ad_right .right_content').eq(idxIndex);
            var startTimeIdx = 'startTime' + idxIndex;
            var endTimeIdx = 'endTime' + idxIndex;

            cur.find('.ConfirmAndSave').off('click').on('click',function () {
                //var PubID = 0;
                //var MediaType = 14002;
                //var MediaID = cur.attr('MediaID')*1;
                var TemplateID = cur.attr('TemplateID')*1;
                var BeginTime = cur.find('#'+ startTimeIdx).val();
                var EndTime = cur.find('#'+ endTimeIdx).val();
                var PurchaseDiscount = purchaseDiscounting;// 折扣为全局变量
                var SaleDiscount = salesDiscount;
                var filePath = cur.find('.uploadFile').attr('href');//上传

                var tr_len = cur.find('table tbody tr').length;
                var requireLen = cur.find('table tbody tr .require_span').length;

                var HasHoliday;//是否区分节假日
                var requireArr = [];//勾选过后节假日的价格验证

                cur.find('.difHolidays').on('change',function() {
                    var that = $(this);
                    if (that.prop('checked')) {
                        HasHoliday = true;
                    }else{
                        HasHoliday = false;
                    }
                })
                if(cur.find('.difHolidays').prop('checked')){
                    HasHoliday = true;
                }else{
                    HasHoliday = false;
                }

                if(RoleIDs == 'SYS001RL00003'){//媒体主  只有销售价格  没有上传附件
                    if(HasHoliday == true){//区分 是框框
                        cur.find('.require_input').each(function () {
                            requireArr.push($(this).val());
                        })
                        for(var i = requireArr.length;i >= 0;i--){
                            if(requireArr[i] == '' || requireArr[i] == '0'){
                                requireArr.splice(i,1);
                            }
                        }
                        if(BeginTime == '' || EndTime == ''){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写价格有效期');
                        }else if(PurchaseDiscount == ''){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写采购折扣');
                        }else if(SaleDiscount == ''){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写销售折扣');
                        }else if(requireArr.length < tr_len * 2 ){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写大于0的价格');
                        }else{
                            cur.find('.errorMeg').hide();
                            setAjaxSenData(PurchaseDiscount*1/100,SaleDiscount*1/100);
                        }
                    }else{//不区分节假日  有文字
                        cur.find('.require_input').each(function () {
                            requireArr.push($(this).val());
                        })
                        for(var i = requireArr.length;i >= 0;i--){
                            if(requireArr[i] == '' || requireArr[i] == '0'){
                                requireArr.splice(i,1);
                            }
                        }
                        if(BeginTime == '' || EndTime == ''){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写价格有效期');
                        }else if(PurchaseDiscount == ''){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写采购折扣');
                        }else if(SaleDiscount == ''){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写销售折扣');
                        }else if(requireArr.length < tr_len * 2 -requireLen){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写大于0的价格');
                        }else{
                            cur.find('.errorMeg').hide();
                            setAjaxSenData(PurchaseDiscount*1/100,SaleDiscount*1/100);
                        }
                    }
                }else if(RoleIDs == 'SYS001RL00005'){//AE
                    if(HasHoliday == true){//区分 是框框
                        cur.find('.require_input').each(function () {
                            requireArr.push($(this).val());
                        })
                        for(var i = requireArr.length;i >= 0;i--){
                            if(requireArr[i] == '' || requireArr[i] == '0'){
                                requireArr.splice(i,1);
                            }
                        }
                        if(BeginTime == '' || EndTime == ''){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写价格有效期');
                        }else if(PurchaseDiscount == ''){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写采购折扣');
                        }else if(SaleDiscount == ''){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写销售折扣');
                        }else if(filePath == 'javascript:;') {
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请上传附件');
                        }else if(requireArr.length < tr_len * 4 ){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写大于0的价格');
                        }else{
                            cur.find('.errorMeg').hide();
                            setAjaxSenData(PurchaseDiscount*1,SaleDiscount*1);
                        }
                    }else{//不区分节假日  有文字
                        cur.find('.require_input').each(function () {
                            requireArr.push($(this).val());
                        })
                        for(var i = requireArr.length;i >= 0;i--){
                            if(requireArr[i] == '' || requireArr[i] == '0'){
                                requireArr.splice(i,1);
                            }
                        }
                        if(BeginTime == '' || EndTime == ''){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写价格有效期');
                        }else if(PurchaseDiscount == ''){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写采购折扣');
                        }else if(SaleDiscount == ''){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写销售折扣');
                        }else if(filePath == 'javascript:;') {
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请上传附件');
                        }else if(requireArr.length < tr_len * 4 -requireLen){
                            cur.find('.errorMeg').show();
                            cur.find('.errorMeg span').text('请填写大于0的价格');
                        }else{
                            cur.find('.errorMeg').hide();
                            setAjaxSenData(PurchaseDiscount*1,SaleDiscount*1);
                        }
                    }
                }

                function setAjaxSenData(PurchaseDis,SaleDis) {
                    var sendData = {
                        'Publish' : {},
                        'PriceList' : []
                    }
                    //基本信息
                    sendData['Publish'] = {
                        "MediaType": 14002,
                        "PubID": PubID,
                        "TemplateID": TemplateID,
                        "MediaID": MediaID,
                        "BeginTime": BeginTime,
                        "EndTime": EndTime,
                        "PurchaseDiscount": PurchaseDis,
                        "SaleDiscount": SaleDis,
                        "ImgUrl": filePath,
                        "HasHoliday": HasHoliday
                    }

                    if(HasHoliday == true){
                        //价格数组
                        cur.find('.price_data table tbody tr').each(function () {
                            var that = $(this);
                            var obj = {
                                'RecID' : 0,
                                'CarouselNumber' : that.attr('CarouselNumber')*1,
                                'SaleType' : that.attr('SaleType')*1,
                                'SalePlatform' : that.attr('SalePlatform')*1,
                                'ADStyle' : that.attr('ADStyle')*1,
                                'SaleArea' : that.attr('SaleArea')*1,
                                'ClickCount' : that.attr('ClickCount')*1,
                                'ExposureCount' : that.attr('ExposureCount')*1,
                                'PubPrice' : that.attr('PubPrice')*1,
                                'SalePrice' : that.attr('SalePrice')*1,
                                'PubPrice_Holiday' : that.attr('PubPrice_Holiday')*1,
                                'SalePrice_Holiday' : that.attr('SalePrice_Holiday')*1
                            }
                            sendData['PriceList'].push(obj);
                        })
                        var url = '/api/Publish/ModifyPublish?v=1_1';
                        setAjax({
                            url : url,
                            type : 'post',
                            data : {
                                'Publish' : sendData.Publish,
                                'PriceList' : sendData.PriceList
                            }
                        },function (data) {
                            if(data.Status == 0){
                                layer.msg(data.Message,{time:1000});
                                window.location = '/publishmanager/advertisinglist_app.html';
                            }else{
                                layer.msg(data.Message,{time:1000});
                            }
                        })

                    }else{//那tr上面的关于节假日的价格去掉

                        //价格数组
                        cur.find('.price_data table tbody tr').each(function () {
                            var that = $(this);
                            var obj = {
                                'RecID' : 0,
                                'CarouselNumber' : that.attr('CarouselNumber')*1,
                                'SaleType' : that.attr('SaleType')*1,
                                'SalePlatform' : that.attr('SalePlatform')*1,
                                'ADStyle' : that.attr('ADStyle')*1,
                                'SaleArea' : that.attr('SaleArea')*1,
                                'ClickCount' : that.attr('ClickCount')*1,
                                'ExposureCount' : that.attr('ExposureCount')*1,
                                'PubPrice' : that.attr('PubPrice')*1,
                                'SalePrice' : that.attr('SalePrice')*1,
                                'PubPrice_Holiday' : 0,
                                'SalePrice_Holiday' : 0
                            }
                            sendData['PriceList'].push(obj);
                        })
                        var url = '/api/Publish/ModifyPublish?v=1_1';
                        setAjax({
                            url : url,
                            type : 'post',
                            data : {
                                'Publish' : sendData.Publish,
                                'PriceList' : sendData.PriceList
                            }
                        },function (data) {
                            if(data.Status == 0){
                                layer.msg(data.Message,{time:1000});
                                window.location = '/publishmanager/advertisinglist_app.html';
                            }else{
                                layer.msg(data.Message,{time:1000});
                            }
                        })
                    }
                }
            })
        }
    }

    addPublishApp.init();


    //价格变化
    function changeAutoPrice(id,purchaseDiscounting,salesDiscount,variable){

        var elem = id;
        var purchaseDis = purchaseDiscounting;
        var salesDis = salesDiscount;
        var variable = variable;

        if(elem == '.layer_con3'){
            if(variable == 'isCPD'){
                var pub_price = 0;//公共变量 刊例价格 方便后面的比较
                var pub_priceHoliday = 0;//公共变量  刊例的节假日价格


                $(elem).find('.PubPrice').on('change',function(){
                    var that = $(this);
                    var money = that.val()*1;
                    that.parents('ul').next().find('.SalePrice').val(formatMoney(money*salesDis,2,''));
                    that.parents('ul').next().find('.SalePrice').attr('money',(money*salesDis).toFixed(2));
                    pub_price = money;
                })

                //当输入节假日刊例价格
                $(elem).find('.PubPrice_Holiday').on('change',function () {
                    var money = $(this).val()*1;
                    $(this).parents('ul').next().find('.SalePrice_Holiday').val(formatMoney(money*salesDis,2,''));
                    $(this).parents('ul').next().find('.SalePrice_Holiday').attr('money',(money*salesDis).toFixed(2));
                    pub_priceHoliday = money;
                })


                if(RoleIDs == 'SYS001RL00003'){//媒体主
                    $(elem).find('.SalePrice').on('change',function () {
                        var money = $(this).val()*1;
                        $(this).attr('money',money);
                    })
                    $(elem).find('.SalePrice_Holiday').on('change',function () {
                        var money = $(this).val()*1;
                        $(this).attr('money',money);//赋值给tr
                    })
                }else if(RoleIDs == 'SYS001RL00005'){//AE
                    $(elem).find('.SalePrice').on('change',function () {
                        var money = $(this).val()*1;
                        if(money < pub_price*purchaseDis){
                            layer.alert("销售价不能低于采购价");
                            $(this).val('');
                            $(this).attr('money','');
                        }else{
                            $(this).attr('money',money);//赋值给tr
                        }
                    })
                    $(elem).find('.SalePrice_Holiday').on('change',function () {
                        var money = $(this).val()*1;
                        if(money < pub_priceHoliday*purchaseDis){
                            layer.alert("销售价不能低于采购价");
                            $(this).val('');
                            $(this).attr('money','');
                        }else{
                            $(this).attr('money',money);//赋值给tr
                        }
                    })
                }


            }else{

                var pub_price = 0;//公共变量 刊例价格 方便后面的比较
                //当输入工作日刊例价格
                $(elem).find('.PubPrice').on('change',function () {
                    var money = $(this).val()*1;
                    $(this).parents('ul').next().find('.SalePrice').val(formatMoney(money*salesDis,2,''));
                    $(this).parents('ul').next().find('.SalePrice').attr('money',(money*salesDis).toFixed(2));
                    pub_price = money;
                })
                //手动修改销售价格的时候   不能低于刊例价格乘以采购折扣

                if(RoleIDs == 'SYS001RL00003'){//媒体主
                    $(elem).find('.SalePrice').on('change',function () {
                        var money = $(this).val()*1;
                        $(this).attr('money',money);
                    })
                }else if(RoleIDs == 'SYS001RL00005'){//AE
                    $(elem).find('.SalePrice').on('change',function () {
                        var money = $(this).val()*1;
                        if(money < pub_price*purchaseDis){
                            layer.alert("销售价不能低于采购价");
                            $(this).val('');
                            $(this).attr('money','');
                        }else{
                            $(this).attr('money',money);//赋值给tr
                        }
                    })
                }

            }

        }else if(elem == '.ad_table'){

            if(variable == 'isCPD'){

                var pub_price = 0;//公共变量 刊例价格 方便后面的比较
                var pub_priceHoliday = 0;//公共变量  刊例的节假日价格
                $(elem).find('.PubPrice').on('change',function () {
                    var money = $(this).val()*1;
                    $(this).parents('tr').find('.SalePrice').val(formatMoney(money*salesDis,2,''));
                    $(this).parents('tr').find('.SalePrice').attr('money',(money*salesDis).toFixed(2));
                    $(this).parents('tr').attr('PubPrice',money);//赋值给tr
                    $(this).parents('tr').attr('SalePrice',(money*salesDis).toFixed(2));//赋值给tr
                    pub_price = money;
                })

                $(elem).find('.PubPrice_Holiday').on('change',function () {
                    var money = $(this).val()*1;
                    $(this).parents('tr').find('.SalePrice_Holiday').val(formatMoney(money*salesDis,2,''));
                    $(this).parents('tr').find('.SalePrice_Holiday').attr('money',(money*salesDis).toFixed(2));
                    $(this).parents('tr').attr('PubPrice_Holiday',money);//赋值给tr
                    $(this).parents('tr').attr('SalePrice_Holiday',(money*salesDis).toFixed(2));//赋值给tr
                    pub_priceHoliday = money;
                })

                if(RoleIDs == 'SYS001RL00003'){//媒体主
                    $(elem).find('.SalePrice').on('change',function () {
                        var money = $(this).val()*1;
                        $(this).parents('tr').attr('SalePrice',money);//赋值给tr
                    })
                    $(elem).find('.SalePrice_Holiday').on('change',function () {
                        var money = $(this).val()*1;
                        $(this).parents('tr').attr('SalePrice_Holiday',money);//赋值给tr
                    })
                }else if(RoleIDs == 'SYS001RL00005'){//AE
                    $(elem).find('.SalePrice').on('change',function () {
                        var money = $(this).val()*1;
                        if(money < pub_price*purchaseDis){
                            layer.alert("销售价不能低于采购价");
                            $(this).val('');
                        }else{
                            $(this).parents('tr').attr('SalePrice',money);//赋值给tr
                        }
                    })
                    $(elem).find('.SalePrice_Holiday').on('change',function () {
                        var money = $(this).val()*1;
                        if(money < pub_priceHoliday*purchaseDis){
                            layer.alert("销售价不能低于采购价");
                            $(this).val('');
                        }else{
                            $(this).parents('tr').attr('SalePrice_Holiday',money);//赋值给tr
                        }
                    })
                }

            }else{

                var pub_price = 0;//公共变量 刊例价格 方便后面的比较
                $(elem).find('.PubPrice').on('change',function () {
                    var money = $(this).val()*1;
                    $(this).parents('tr').find('.SalePrice').val(formatMoney(money*salesDis,2,''));
                    $(this).parents('tr').find('.SalePrice').attr('money',(money*salesDis).toFixed(2));
                    $(this).parents('tr').attr('PubPrice',money);//赋值给tr
                    $(this).parents('tr').attr('SalePrice',(money*salesDis).toFixed(2));//赋值给tr
                    pub_price = money;
                })

                if(RoleIDs == 'SYS001RL00003'){//媒体主
                    $(elem).find('.SalePrice').on('change',function () {
                        var money = $(this).val()*1;
                        $(this).parents('tr').attr('SalePrice',money);//赋值给tr
                    })
                }else if(RoleIDs == 'SYS001RL00005'){//AE
                    $(elem).find('.SalePrice').on('change',function () {
                        var money = $(this).val()*1;
                        if(money < pub_price*purchaseDis){
                            layer.alert("销售价不能低于采购价");
                            $(this).val('');
                        }else{
                            $(this).parents('tr').attr('SalePrice',money);//赋值给tr
                        }
                    })
                }
            }
        }
    }

    //获取url 地址参数方法
    function GetQueryString(name) {
        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if(r!=null)return unescape(r[2]); return null;
    }

    //刊例原稿  只有AE会有
    function uploadFile(id) {
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
        };
        function escapeStr(str) {
            return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
        };


        $('#'+id).uploadify({
            'buttonText': '上传附件',
            'buttonClass': 'button_add',
            'swf': '/Js/uploadify.swf?_=' + Math.random(),
            'uploader': '/AjaxServers/UploadFile.ashx',
            'auto': true,
            'multi': false,
            'width': 80,
            'height': 18,
            /*'formData': { Action: 'BatchImport', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')), IsGenSmallImage: 1 }, 存在缩略图大图的格式 */
            'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
            'fileTypeDesc': '支持格式:xls,xlsx,jpg,jpeg,png.gif,zip,pdf,ppt,pptx',
            'fileTypeExts': '*.xls;*.xlsx;*.jpg;*.jpeg;*.png;*.gif;*.zip;*.pdf;*.ppt;*.pptx;',
            'fileSizeLimit':'10MB',
            'queueSizeLimit': 1,
            'scriptAccess': 'always',
            'onQueueComplete': function (event, data) {},
            'fileCount':1,
            queueID:'imgShow',
            'scriptAccess': 'always',
            'overrideEvents' : [ 'onDialogClose'],
            'onQueueComplete': function (event, data) {},
            'onQueueFull': function () {
                layer.alert('您最多只能上传1个文件！');
                return false;
            },
            'onUploadSuccess': function (file, data, response) {
                if (response == true) {
                    var json = $.evalJSON(data);
                    var uploadShow = $("#"+id).parent().next();
                    console.log(json);
                    uploadShow.show();
                    uploadShow.find(".uploadName").text(json.FileName);
                    uploadShow.find(".uploadFile").attr("href","" + json.Msg);
                }
            },
            'onProgress': function (event, queueID, fileObj, data) {},
            'onUploadError': function (event, queueID, fileObj, errorObj) {},
            'onSelectError':function(file, errorCode, errorMsg){}
        });

    };

    //数组去重
    Array.prototype.unique = function(){
        var res = [];
        var json = {};
        for(var i = 0; i < this.length; i++){
          if(!json[this[i]]){
           res.push(this[i]);
           json[this[i]] = 1;
          }
        }
        return res;
    }

    //判断一个值是否在数组存在
    function isCon(arr, val){
        for(var i=0; i<arr.length; i++){
            if(arr[i] == val)
                return true;
        }
        return false;
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

})
