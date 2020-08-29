/*
* Written by:     wangcan
* function:       广告模板审核、修正
* Created Date:   2017-06-06
* Modified Date:
*/
/*
    1、媒体名称右上角显示，在接口调用之后，单独写
    2、起投天数>0，<180
    3、定向城市：城市、组名不能重复，修改时，已选城市不能删除
    4、审核和修正、成功后将当前一条数据隐藏，需要判断页面上显示上的数据，是否需要重新调接口数据。
        当所有的数据都审核/修正完成，把右侧所有的table移除。重新加载新数据

*/

$(function(){
    //记录“下一页”的点击次数，以便给图片加上唯一的ID值
    var clickCount = 0;
    //获取url参数
    var userData = GetRequest();

    //初始化数据,只有点击行审核，左边才有数据。
    if(userData.isMany == 0){
        getLeftData();
    }else{
        $('#only_right').show();
    }
    getRightData();
    /*
    1、鼠标移入城市组名称时，对应城市显示
    2、对比标红
    3、操作广告样式
    */
    function compareAndSign(){
        //鼠标移入城市组名称时，对应城市显示
        $('.mp_left').find('table').on('mouseover','.GroupName',function(){
            if($(this).next('.add_city').find('ul li').length){
                $(this).next('.add_city').show();
            }
        }).on('mouseout','.GroupName',function(){
            $(this).next('.add_city').hide();
        })
        $('.mp_right').find('table .AdSaleAreaGroup').each(function(){
            $(this).on('mouseover',function(){
                if($(this).find('.add_city ul li').length){
                    $(this).find('.add_city').show();
                }
            }).on('mouseout',function(){
                $(this).find('.add_city').hide();
            })
        });
        //对比标红
        if($('.mp_left').find('.emplate_table table').length != 0){
            var leftData = JSON.parse($('.mp_left .emplate_table').attr('result'));
            $('.mp_right').find('table').each(function(){
                var _this = $(this);
                var curData = JSON.parse(_this.attr('result'));
                //广告样式，多出的标红
                for(var i=0;i<curData.AdTempStyle.length;i++){
                    var notEqualNum = 0;
                    for(var j=0;j<leftData.AdTempStyle.length;j++){
                        if(curData.AdTempStyle[i].AdStyle == leftData.AdTempStyle[j].AdStyle){
                            notEqualNum++;
                        }
                    }
                    if(notEqualNum == 0){
                        _this.find('.AdTempStyle .show').children().each(function(){
                            if($.trim($(this).html()) == curData.AdTempStyle[i].AdStyle){
                                $(this).addClass('yellow')
                            }
                        });
                    }
                }
                //轮播数，多出的标红
                if(curData.CarouselCount > leftData.CarouselCount){
                    _this.find('.CarouselCount .show').addClass('yellow');
                }
                //售卖平台，多出的标红
                if((1 & leftData.SellingPlatform) <= 0){
                    _this.find('.SellingPlatform .show div').each(function(){
                        if($.trim($(this).text()) == 'Android'){
                            $(this).addClass('yellow');
                        }
                    });
                //原来选择的售卖平台，不可修改，为disable状态
                }else{
                    _this.find('.SellingPlatform .hide span').each(function(){
                        if($(this).find('input').attr('value') == 'Android' && $(this).find('input').attr('checked')){
                            $(this).find('input').attr('disabled','disabled');
                        }
                    });
                }
                if((2 & leftData.SellingPlatform) <= 0){
                    _this.find('.SellingPlatform .show div').each(function(){
                        if($.trim($(this).text()) == 'iOS'){
                            $(this).addClass('yellow');
                        }
                    });
                }else{
                    _this.find('.SellingPlatform .hide span').each(function(){
                        if($(this).find('input').attr('value') == 'iOS' && $(this).find('input').attr('checked')){
                            $(this).find('input').attr('disabled','disabled');
                        }
                    });
                }
                if((4 & leftData.SellingPlatform) <= 0){
                    _this.find('.SellingPlatform .show div').each(function(){
                        if($(this).text() == 'Android & iOS'){
                            $(this).addClass('yellow');
                        }
                    });
                }else{
                    _this.find('.SellingPlatform .hide span').each(function(){
                        if($(this).find('input').attr('value') == 'Android & iOS' && $(this).find('input').attr('checked')){
                            $(this).find('input').attr('disabled','disabled');
                        }
                    });
                }
                if((8 & leftData.SellingPlatform) <= 0){
                    _this.find('.SellingPlatform .show div').each(function(){
                        if($(this).text() == 'PAD'){
                            $(this).addClass('yellow');
                        }
                    });
                }else{
                    _this.find('.SellingPlatform .hide span').each(function(){
                        if($(this).find('input').attr('value') == 'PAD' && $(this).find('input').attr('checked')){
                            $(this).find('input').attr('disabled','disabled');
                        }
                    });
                }
                if((16 & leftData.SellingPlatform) <= 0){
                    _this.find('.SellingPlatform .show div').each(function(){
                        if($(this).text() == 'Gphone'){
                            $(this).addClass('yellow');
                        }
                    });
                }else{
                    _this.find('.SellingPlatform .hide span').each(function(){
                        if($(this).find('input').attr('value') == 'Gphone' && $(this).find('input').attr('checked')){
                            $(this).find('input').attr('disabled','disabled');
                        }
                    });
                }
                if((32 & leftData.SellingPlatform) <= 0){
                    _this.find('.SellingPlatform .show div').each(function(){
                        if($(this).text() == 'IPhone'){
                            $(this).addClass('yellow');
                        }
                    });
                }else{
                    _this.find('.SellingPlatform .hide span').each(function(){
                        if($(this).find('input').attr('value') == 'IPhone' && $(this).find('input').attr('checked')){
                            $(this).find('input').attr('disabled','disabled');
                        }
                    });
                }

                //售卖方式，多出的标红，售卖方式，只可增加，不能删除原来的
                //如果左侧为1，说明为CPD,则右侧CPM标红,CPD为不可修改状态，如果左侧为2，说明为CPM,则右侧CPD标红，CPM为不可修改状态，如果左侧为4，则右侧不需要标红
                switch(leftData.SellingMode){
                    case 1:
                        _this.find('.SellingMode .show div').each(function(){
                            if($(this).html() == 'CPM'){
                                $(this).addClass('yellow');
                            }
                        })
                        _this.find('.SellingMode .hide input').each(function(){
                            if($(this).attr('value') == 'CPM' && $(this).attr('checked')){
                                $(this).attr('disabled','disabled');
                            }
                        });
                        break;
                    case 2:
                        _this.find('.SellingMode .show div').each(function(){
                            if($(this).html() == 'CPD' ){
                                $(this).addClass('yellow');
                            }
                        })
                        _this.find('.SellingMode .hide input').each(function(){
                            if($(this).attr('value') == 'CPD' && $(this).attr('checked')){
                                $(this).attr('disabled','disabled');
                            }
                        });
                        break;
                    case 3:
                        _this.find('.SellingMode .hide input').each(function(){
                            if($(this).attr('value') == 'CPD' && $(this).attr('checked')){
                                $(this).attr('disabled','disabled');
                            }
                            if($(this).attr('value') == 'CPM' && $(this).attr('checked')){
                                $(this).attr('disabled','disabled');
                            }
                        });
                        break;
                    default:
                        break;
                }


                //售卖区域，城市组名称不同，标红。名称相同，但城市组中有Ispublic=0的城市，，也需要标红
                for(var i=0;i<curData.AdSaleAreaGroup.length;i++){
                    //城市名称相同的个数，城市组中含Ispublic=0的城市个数（如果>0,则城市组标红）
                    var EqualCityNameNum = 0,Ispublic0 = 0,cityName="",cityID=-2;
                    for(var j=0;j<leftData.AdSaleAreaGroup.length;j++){
                        //当前城市组名称与左侧城市组名称不同的数量
                        if(curData.AdSaleAreaGroup[i].GroupName == leftData.AdSaleAreaGroup[j].GroupName){
                            EqualCityNameNum++;
                        }
                    }
                    if(EqualCityNameNum == 0){
                        _this.find('.AdSaleAreaGroup').each(function(){
                            if($(this).find('.GroupName').text() == curData.AdSaleAreaGroup[i].GroupName){
                                $(this).find('.GroupName').addClass('yellow');
                            }
                        });
                    }
                    for(var k=0;k<curData.AdSaleAreaGroup[i].DetailArea.length;k++){
                        if(curData.AdSaleAreaGroup[i].DetailArea[k].IsPublic == 0){
                            cityID = curData.AdSaleAreaGroup[i].GroupId;
                            Ispublic0++;
                        }
                    }
                    //名称相同，如果内含ISpublic=0的城市，城市组标红，由于城市组名称可能重复，所以只能判断ID
                    if(Ispublic0 > 0){
                        _this.find('.AdSaleAreaGroup').each(function(){
                            if(JSON.parse($(this).attr('AdSaleAreaGroup')).GroupId == cityID){
                                $(this).find('.GroupName').addClass('yellow');
                            }
                        });
                    }


                }
                //广告展示逻辑，素材说明、备注、与左侧不同则标红
                if($.trim(curData.AdDisplay) != $.trim(leftData.AdDisplay)){
                    _this.find('.AdDisplay .show').children().addClass('yellow');
                }
                if($.trim(curData.AdDescription) != $.trim(leftData.AdDescription)){
                    _this.find('.AdDescription .show').children().addClass('yellow');
                }
                if($.trim(curData.Remarks) != $.trim(leftData.Remarks)){
                    _this.find('.Remarks .show').children().addClass('yellow');
                }
                if(curData.AdDisplayLength != leftData.AdDisplayLength){
                    _this.find('.AdDisplayLength .show').addClass('yellow');
                }
            });
        }
        //打开编辑页后，点击取消，隐藏.hide，显示.show,不做其他操作
        $('.cancel').off('click').on('click',function(){
            $(this).parents('.adv').find('.hide').hide().end().find('.show').show();
            auditTemp();
        })
    }



    //左侧模板渲染，若从批量审核进来，左侧肯定没有；若点击行“审核”，可能有，可能没有。但是只选择一个也可以批量审核，呵呵
    //还有一种情况，点击修正后自动审核通过或点击审核通过后，需要重新调左侧数据，ID值取当前页面未审核的RecId
    function getLeftData(NextTemplateID){
        $.ajax({
            url:'/api/Template/GetParentInfo',
            type:'get',
            data:{
                AdTempId:NextTemplateID || userData.AdTempId
            },
            async: false,
            dataType: 'json',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success:function(data){
                if(data.Status == 0){
                    $('.mp_left .emplate_table').html(ejs.render($('#parentTemp').html(), {data: data.Result}));
                    var returnData = data.Result;
                    $('.mp_left .emplate_table').attr('result',JSON.stringify(returnData));
                }else{
                    $('.mp_left .emplate_table').html('');
                    $('#only_right').show();
                }
            }
        })
    }
    //右侧模板渲染
    function getRightData(NextTemplateID,NextAdBaseTempId){
        var upData = null;
        //点行审核：传AdTempId，不传AdTempIdList
        if(userData.isMany == 0){
            upData = {
                pageIndex:1,
                pageSize:12,
                BaseMediaId:userData.BaseMediaId,
                AdTempId:userData.AdTempId
            }
        //点批量审核，传AdTempId=-2，AdTempIdList = 获取到的值
        }else{
            upData = {
                pageIndex:1,
                pageSize:12,
                BaseMediaId:userData.BaseMediaId,
                AdTempId:-2,
                AdTempIdList:userData.AdTempId
            }
        }
        //如果是动态调下一条，肯定是调行审核
        if(NextTemplateID && NextAdBaseTempId){
            upData = {
                pageIndex:1,
                pageSize:12,
                BaseMediaId:NextAdBaseTempId,
                AdTempId:NextTemplateID
            }
        }
        $.ajax({
            url:'/api/Template/list',
            type:'get',
            data:upData,
            async: false,
            dataType: 'json',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success:function(data){
                if(data.Status == 0 && data.Result){
                    if(!$('.order_r .mediaName').find('span').html() && data.Result.MediaInfo){
                        $('.order_r .mediaName').find('span').html(data.Result.MediaInfo.Name);
                        $('.order_r .mediaName').find('img').attr('src',data.Result.MediaInfo.HeadIconURL);
                        $('.order_r .mediaName').find('span').html(data.Result.BaseMediaName).end().find('img').attr('src',data.Result.BaseMediaLogoUrl);
                    }
                    $('.mp_right .swiper-wrapper').html(ejs.render($('#childTemp').html(), {data: data.Result.List}));
                    for(var l=0;l<data.Result.List.length;l++){
                        var returnData = data.Result.List[l];
                        $('.mp_right').find('table').eq(''+l+'').attr('result',JSON.stringify(returnData));
                    }
                    //把总数目绑在标签上
                    $('.swiper-wrapper').attr('TotleCount',data.Result.TotleCount);
                    modifyModule();
                    auditTemp();
                    compareAndSign();
                    if(data.Result.TotleCount <= 3){
                        $('.arrow_l').hide();
                        $('.arrow_r').hide();
                    }else{

                    }
                    var swiper = new Swiper('.swiper-container', {
                        nextButton: '.arrow_r',
                        prevButton: '.arrow_l',
                        slidesPerView: 3,
                        centeredSlides: false,
                        paginationClickable: true,
                        spaceBetween: 0
                    });
                    //高度判断
                    changeHeight();
                }
            }
        })

    }
    //当点击下一项时，判断下一页还有几列，若还有4列，length=3,以此类推
    document.querySelector('.arrow_r').addEventListener('click',function(){
        /*获取当前页面后还有几个表格,

        如果<3,通过数据总数（totalCount）和页面上显示的表格总数(displayNum)进行比较
        if(displayNum<totalCount) 就请求数据，
        否则不请求

        请求时，需要传pageIndex,后台每次返回pageSize=20条数据，则共有（Math.ceil(totalCount/pageSize)）页
        当前页面上数据的个数是displayNum,则已显示（displayNum/pageSize）页数据

        传pageIndex = displayNum/pageSize + 1
        */
        getData();
        changeHeight();
    })

    //审核模板
    //审核通过，审核通过或驳回时，判断还有几个.adv，如果有三个，请求数据，再次渲染
    //当全部审核完成时，返回数据会有Result:{NextTemplateID:1}，需要重新调数据渲染页面
    function auditTemp(){
        $('.adv').off('click').on('click','.pass',function(){
            var _this = $(this);
            setAjax({
                url:'/api/Template/AuditTemplate',
                type:'post',
                data:{
                    "TemplateID": _this.parents('.adv').find('.mediaInfo').attr('RecID'),
                    "RejectReason":"",
                    "OpType":48002//48002通过，48003驳回
                }
            },function(data){
                if(data.Status == 0){
                    _this.parents('.adv').hide();
                    layer.msg(data.Message,{time:1000});
                    if(data.Result.NextAdTempId){
                        var NextAdTempId = data.Result.NextAdTempId;
                        var NextAdBaseTempId = data.Result.NextAdBaseTempId;
                    }
                    //在这里需要动态的调一下数据
                    getData();
                    //如果NextAdBaseTempId=0，说明没有数据了
                    if($('.swiper-wrapper').find('table:visible').length == 0 && NextAdBaseTempId != 0){
                        $('#only_right').hide();
                        //如果数据都审核完了，remove所有数据，设置localstorage，以便在getData时可以获取对应的数据
                        $('.swiper-wrapper').find('.adv').remove();
                        window.location.href='/publishmanager/advTempAudit.html?AdTempId='+NextAdTempId+'&BaseMediaId='+NextAdBaseTempId+'&isMany=0';
                    }else{
                        if($('.swiper-wrapper').find('table:visible').length == 0 && NextAdBaseTempId == 0){
                            layer.msg('没有待审核的数据了',{time:1000},function () {
                                window.location = '/publishmanager/advTempAuditList.html';
                            });
                        }else if($('.swiper-wrapper').find('table:visible').length != 0){
                            var curRecID = $('.swiper-wrapper').find('table:visible').eq(0).find('.mediaInfo').attr('recid');
                            getLeftData(curRecID);
                        }
                    }
                }else{
                    layer.msg(data.Message);
                }
            })
        }).on('click','.reject_btn',function(){
            var _this = $(this);
            $.openPopupLayer({
                name: "popLayerDemo",
                url: "adTempReject.html",
                success: function () {
                    /*弹层关闭*/
                    $('.closeIt').each(function(){
                        $(this).off('click').on('click',function(event){
                            event.preventDefault();
                            $.closePopupLayer('popLayerDemo');
                        })
                    });
                    $('#allowBtn').off('click').on('click',function(event){
                        var val = $.trim($('#rejCon').val());
                        if(val){
                            setAjax({
                                url:'/api/Template/AuditTemplate',
                                type:'post',
                                data:{
                                    "TemplateID": _this.parents('.adv').find('.mediaInfo').attr('RecID'),
                                    "RejectReason":val,
                                    "OpType":48003//48002通过，48003驳回
                                }
                            },function(data){
                                if(data.Status == 0){
                                    _this.parents('.adv').hide();
                                    $.closePopupLayer('popLayerDemo');
                                    if(data.Result.NextAdTempId){
                                        var NextAdTempId = data.Result.NextAdTempId;
                                        var NextAdBaseTempId = data.Result.NextAdBaseTempId;
                                    }
                                    layer.msg(data.Message,{time:1000})
                                    //在这里需要动态的调一下数据
                                    getData();
                                    //如果NextAdBaseTempId=0，说明没有数据了
                                    if($('.swiper-wrapper').find('table:visible').length == 0 && NextAdBaseTempId != 0){
                                        $('#only_right').hide();
                                        //如果数据都审核完了，remove所有数据，设置localstorage，以便在getData时可以获取对应的数据
                                        $('.swiper-wrapper').find('.adv').remove();
                                        window.location.href='/publishmanager/advTempAudit.html?AdTempId='+NextAdTempId+'&BaseMediaId='+NextAdBaseTempId+'&isMany=0';
                                    }else{
                                        if($('.swiper-wrapper').find('table:visible').length == 0 && NextAdBaseTempId == 0){
                                            layer.msg('没有待审核的数据了',{time:1000},function () {
                                                window.location = '/publishmanager/advTempAuditList.html';
                                            });
                                        }
                                    }
                                }
                            })
                        }else{
                            layer.msg('请输入驳回原因',{time:2000})
                        }
                    })
                }
            })
        })
    }

    //修正模板
    function modifyModule(){
        $('.modify').off('click').on('click',function(){
            $(this).parents('.adv').find('.show').hide().end().find('.hide').show();
            changeHeight();
            //广告样式操作：添加、删除
            $('.addAdvStyle').off('click').on('click',function(){
                $(this).parents('.styleEach').after(''+
                    '<div class="styleEach">'+
                        '<div class="in_left">'+
                            '<input name="Username" type="text"  value="" AdStyleId="-2" style="width:140px;" maxlength="10">'+
                            '<span class="delStyle"><img src="../ImagesNew/close_h1.png"></span>'+
                        '</div>'+
                        '<div class="button_add sure" style="width:50px;margin-left:15px;float: left;padding: 0;cursor:pointer;">确定</div>'+
                        '<div class="button_add addAdvStyle" style="width:50px;margin-left:15px;float: left;padding: 0;cursor:pointer;display:none;">添加</div>'+
                        '<div class="clear"></div>'+
                    '</div>');
                $(this).hide();
                //添加新的样式后，显示所有的删除按钮
                $(this).parents('.AdTempStyle').find('.delStyle').show();
                changeHeight();
            });
            $('.AdTempStyle .hide').off('click').on('click','.delStyle',function(){
                var length = $(this).parents('.AdTempStyle').find('input').length;
                var hide = $(this).parents('.hide');
                //判断，如果删除的时候只有两个样式，那么删除后，另一个不可删，只有添加了新的，才显示删除按钮
                if(length == 2){
                    $(this).parents('.AdTempStyle').find('.delStyle').hide();
                    hide.find('input:first').parents('.in_left').next().show().css('color','red');
                }
                //还有一个判断，如果默认显示一个样式，删除后怎么添加，添加按钮在哪？还是默认不让删除嘞？
                //渲染的时候，判断有几个样式，如果只有一个，删除按钮隐藏，当添加了新的样式之后，删除显示
                //如果不是新加的样式，点击删除之后，此行删除，找到最后一个显示的"添加"，显示。
                if($(this).parents('.in_left').next().html() != '确定'){
                    $(this).parents('.styleEach').remove();
                //如果是新加的样式，点击删除之后，此行删除，同时，找到最后一个显示的样式， 后面的“添加”显示
                }else{

                    $(this).parents('.styleEach').remove();
                    if(length == 1){
                        hide.find('.addAdvStyle:last').show();
                    }
                    //存在一种情况，没有点击确定，添加不能显示.或者，新添加的都删除了，那么判断“确定”的数量，如果是0，说明都删除了
                    if(hide.find('.sure:last').css('display') == 'none' || hide.find('.sure').length == 0){
                        hide.find('.addAdvStyle:last').show();
                    }
                }
                //如果只有一个input去删除，显示最后添加按钮
                if(length == 1){
                    hide.find('.addAdvStyle:last').show().css('color','red')
                }
                changeHeight();
            }).on('click','.sure',function(){
                if($(this).prev().find('input').val()){
                    $(this).hide().next().show();
                    $('.addAdvStyle').off('click').on('click',function(){
                        $(this).parents('.styleEach').after(''+
                            '<div class="styleEach">'+
                                '<div class="in_left">'+
                                    '<input name="Username" type="text"  value="" AdStyleId="-2" style="width:140px;" maxlength="10">'+
                                    '<span class="delStyle"><img src="../ImagesNew/close_h1.png"></span>'+
                                '</div>'+
                                '<div class="button_add sure" style="width:50px;margin-left:15px;float: left;padding: 0;cursor:pointer;">确定</div>'+
                                '<div class="button_add addAdvStyle" style="width:50px;margin-left:15px;float: left;padding: 0;cursor:pointer;display:none;">添加</div>'+
                                '<div class="clear"></div>'+
                            '</div>');
                        $(this).hide();
                        //添加新的样式后，显示所有的删除按钮
                        $(this).parents('.AdTempStyle').find('.delStyle').show();
                    });
                }
                changeHeight();
            })


            //轮播数和起投天次只能为数字
            $(this).parents('.adv').find('.CarouselCount input').on("input",function(){
                replaceAndSetPos(this,/[^0-9]/g,'');
            })
            $(this).parents('.adv').find('.AdDisplayLength input').on("input",function(){
                replaceAndSetPos(this,/[^0-9]/g,'');
            })
            //图片相关操作：删除、上传

            $('.adv').off('click').on('click','.delPicture',function(){
                var _this = $(this);
                //原来的图例图片变成背景图，原来的删除图变成添加图
                _this.parents('.ad_map').find('.disPicture').attr('src','../ImagesNew/icon65.png');
                _this.removeClass('delPicture').addClass('addPicture').attr({'src':'../ImagesNew/icon63.png','title':'添加'});
                //1、已上传的图片，显示“删除”，此时，上传图片按钮层级需要低一些，而删除按钮层级高、
                //当删除后，层级置为默认0，将上传按钮层级置高。
                //2、显示“+”的，为默认层级即可
                _this.parents('span').css('z-index',0);
                _this.parents('span').prev().css('z-index',100)
            })
            //上传图片

            for(var i=0;i<50;i++){
                for(var j=0;j<3;j++){
                    uploadImg('imgDefault'+i+j);
                    uploadImg('imgAdd'+i+j);

                }
            }
            //对于新渲染的数据，上传图片时，设置了另外的ID，需要重新写上传图片
            for(var i=0;i<3;i++){
                for(var j=0;j<3;j++){
                    for(var q=0;q<50;q++){
                        uploadImg('imgDefault'+i+j+q);
                        uploadImg('imgAdd'+i+j+q);
                    }

                }
            }
            //DIV模拟的文本域进行修改时，动态调整高度
            $('.textarea').each(function(){
                $(this).off('keyup').on('keyup',function(){
                    changeHeight();
                })
            });

            //更新入库
            $('.updata').off('click').on('click',function(){
                var _this = $(this);
                //验证轮播数和示例图
                if(dataVerification(_this) == false){
                    return
                }
                //获取所有修改后的信息
                //广告样式，只要新增的名称
                var AdTempStyle = [];
                _this.parents('.adv').find('.AdTempStyle .styleEach').each(function(){
                    if($(this).find('input') && $.trim($(this).find('input').val())){
                        AdTempStyle.push({
                            "BaseMediaID": -2,
                            "AdTemplateID":-2,
                            "AdStyleId": $(this).find('input').attr('AdStyleId')-0,
                            "IsPublic": 0,//是否是公共模板内容
                            "AdStyle": $.trim($(this).find('input').val())
                        });
                    }
                });

                if(AdTempStyle.length == 0 && _this.parents('.adv').find('.AdTempStyle .styleEach').length == 1){
                    layer.msg('请添加广告样式');
                    return false;
                }
                //售卖平台
                var SellingPlatform = 1;
                _this.parents('.adv').find('.SellingPlatform input:checked').each(function(){
                    SellingPlatform  = SellingPlatform | ($(this).attr('plNum'));
                })

                //售卖方式
                var SellingMode = 1;
                _this.parents('.adv').find('.SellingMode input:checked').each(function(){
                    SellingMode  = SellingMode | ($(this).attr('plNum'));
                })
                //城市
                var AdSaleAreaGroup = [];
                //传值的时候判断，如果originalSaleAreaGroup=AdSaleAreaGroup，说明没有修改，不传值，如果修改了，获取AdSaleAreaGroup里所有IsPublic=0的城市，传给后台
                //将originalSaleAreaGroup和AdSaleAreaGroup中的id值分别放到一个数组中，
                //先排序，再转成字符串进行==比较，如果相等，说明没修改，不需要给后台传这个对象，如果不等，需要将ispublc=0的数据传给后台
                function sortNumber(a,b){
                    return a - b
                }
                _this.parents('.adv').find('.AdSaleAreaGroup .cityList').each(function(){
                    if($(this).find('.city_name').html() == '全国' || $(this).find('.city_name').html() == '其他城市'){
                        return;
                    }
                    //如果没有originalSaleAreaGroup，说明是新增的城市组，直接传
                    if(!$(this).attr('originalSaleAreaGroup')){
                        AdSaleAreaGroup.push(JSON.parse($(this).attr('AdSaleAreaGroup')));
                        return;
                    }
                    //如果不是新增的：
                    //获取修改前和修改后的城市组信息，进行对比
                    var beforeGroup = JSON.parse($(this).attr('originalSaleAreaGroup'));
                    var afterGroup = JSON.parse($(this).attr('AdSaleAreaGroup'));
                    //判断如果是全国或其他城市，不添加到向后台传的AdSaleAreaGroup数组中
                    if(beforeGroup.GroupType == 1){
                        var beforeGroupIds = [],afterGroupIds = [];
                        for(var k=0;k<beforeGroup.DetailArea.length;k++){
                            beforeGroupIds.push(beforeGroup.DetailArea[k].CityId);
                        }
                        beforeGroupIds = beforeGroupIds.sort(sortNumber);
                        for(var j=0;j<afterGroup.DetailArea.length;j++){
                            afterGroupIds.push(afterGroup.DetailArea[j].CityId);
                        }
                        afterGroupIds = afterGroupIds.sort(sortNumber);
                        //修改后逻辑，不管修改前和修改后是否相等，都传Ispublic=0的城市
                        var obj = {
                            "GroupId": afterGroup.GroupId,
                            "GroupType":afterGroup.GroupType,
                            "IsPublic":0,
                            "GroupName":afterGroup.GroupName,
                            "DetailArea": []
                        };
                        //如果删除了，DetailArea传空,否则找到所有Ispublic=0的数据，加入区域中
                        if(afterGroupIds){
                            //找到所有Ispublic=0的数据，加入区域中
                            for(var l = 0;l<afterGroup.DetailArea.length;l++){
                                if(afterGroup.DetailArea[l].IsPublic == 0){
                                    obj.DetailArea.push(afterGroup.DetailArea[l]);
                                }
                            }
                        }
                        AdSaleAreaGroup.push(obj);
                    }
                })
                //示例图
                var AdLegendURL = [];
                _this.parents('.adv').find('.hide .ad_map>img').each(function(){
                    if($(this).attr('src') != '../ImagesNew/icon65.png'){
                        AdLegendURL.push($(this).attr('src'));
                    }
                });
                function delHtmlTag(str)
                {
                    return str.replace(/<[^>]+>/g,"");//去掉所有的html标记
                }
                var obj ={
                    "BusinessType":15000,
                    "OperateType": 2,
                    "Temp": {
                        "IsModified":true,//是否是运营修正操作
                        "TemplateId":_this.parents('.adv').find('.mediaInfo').attr('RecID')-0,//模板ID
                        "BaseAdId":_this.parents('.adv').find('.mediaInfo').attr('BaseAdId')-0,//父类模板id
                        "BaseMediaId":_this.parents('.adv').find('.mediaInfo').attr('BaseMediaId')-0,//媒体id
                        "AdTemplateName":$.trim(_this.parents('.adv').find('.AdTemplateName td:first').html()),   //广告名称
                        "OriginalFile": _this.parents('.adv').find('.OriginalFile').attr('OriginalFile'),//刊例原件
                        "AdForm": _this.parents('.adv').find('.AdForm').attr('AdForm')-0,//广告形式
                        "CarouselCount":$.trim(_this.parents('.adv').find('.CarouselCount input').val())-0,//轮播数
                        "SellingPlatform":SellingPlatform-0,//售卖平台
                        "SellingMode": SellingMode-0,//售卖方式
                        "AdLegendURL": AdLegendURL.join(','),//示例图，以,分隔的字符串
                        "AdDisplay": delHtmlTag($.trim(_this.parents('.adv').find('.AdDisplay .textarea').html())),//广告展示逻辑
                        "AdDescription": delHtmlTag($.trim(_this.parents('.adv').find('.AdDescription .textarea').html())),//广告模板说明、描述
                        "Remarks":delHtmlTag($.trim(_this.parents('.adv').find('.Remarks .textarea').html())),//备注
                        "AdDisplayLength": $.trim(_this.parents('.adv').find('.AdDisplayLength input').val())-0,//起投天数/次
                        "AdTempStyle":AdTempStyle,//广告样式
                        "AdSaleAreaGroup": AdSaleAreaGroup
                    }
                }
                setAjax({
                    url:'/api/Template/curd',
                    type:'post',
                    data:obj
                },function(data){
                    if(data.Status == 0){
                       //点击“更新入库相当于审核通过”
                       setAjax({
                            url:'/api/Template/AuditTemplate',
                            type:'post',
                            data:{
                                "TemplateID": _this.parents('.adv').find('.mediaInfo').attr('RecID'),
                                "RejectReason":"",
                                "OpType":48002//48002通过，48003驳回
                            }
                        },function(data){
                            if(data.Status == 0){
                                if(data.Result.NextAdTempId){
                                    var NextAdTempId = data.Result.NextAdTempId;
                                    var NextAdBaseTempId = data.Result.NextAdBaseTempId;
                                }
                                _this.parents('.adv').hide();
                                layer.msg(data.Message,{time:1000});
                                //在这里需要动态的调一下数据
                                getData();
                                //如果NextAdBaseTempId=0，说明没有数据了
                                if($('.swiper-wrapper').find('table:visible').length == 0 && NextAdBaseTempId != 0){
                                    $('#only_right').hide();
                                    //如果数据都审核完了，remove所有数据，设置localstorage，以便在getData时可以获取对应的数据
                                    $('.swiper-wrapper').find('.adv').remove();
                                    window.location.href='/publishmanager/advTempAudit.html?AdTempId='+NextAdTempId+'&BaseMediaId='+NextAdBaseTempId+'&isMany=0';

                                }else{
                                    if($('.swiper-wrapper').find('table:visible').length == 0 && NextAdBaseTempId == 0){
                                        layer.msg('没有待审核的数据了',{time:1000},function () {
                                            window.ltion = '/publishmanager/advTempAuditList.html';
                                        });
                                    }else if($('.swiper-wrapper').find('table:visible').length != 0){
                                        var curRecID = $('.swiper-wrapper').find('table:visible').eq(0).find('.mediaInfo').attr('recid');
                                        getLeftData(curRecID);
                                    }
                                }
                            }else{
                                layer.msg(data.Message);
                            }
                        })
                    }else{
                        layer.msg(data.Message);
                    }
                });

            });

        });
    }



    //添加城市
    $('.addCity').off('click').on('click',function(){
        var _this = $(this);
        $.openPopupLayer({
            name: "popLayerDemo",
            url: "chooseCity.html",
            success: function () {
                //城市组信息
                var AdSaleAreaGroup = {
                    GroupId: -2,
                    GroupType:1,
                    IsPublic:0,
                    GroupName: "",
                    DetailArea: []
                };
                /*弹层关闭*/
                $('.close').each(function(){
                    $(this).off('click').on('click',function(event){
                        event.preventDefault();
                        $.closePopupLayer('popLayerDemo');
                    })
                });
                //获取其他城市组的城市
                var sibilingCityInfo = [];
                _this.parents('td').find('.cityList:visible').each(function(){
                    if($(this).find('.city_name').html() == '全国' || $(this).find('.city_name').html() == '其他城市'){
                        return;
                    }else{
                        var DetailArea = JSON.parse($(this).attr('AdsaleareaGroup')).DetailArea;
                        for(var i=0;i<DetailArea.length;i++){
                            sibilingCityInfo.push(DetailArea[i].CityName)
                        }

                    }
                });
                //其他城市组有的城市，不能再添加，所以默认不显示
                for(var j=0;j<sibilingCityInfo.length;j++){
                    $('.add_sell_l .num_name').each(function(){
                        if($.trim($(this).text()) == sibilingCityInfo[j]){
                            $(this).parents('.sort_list').remove();
                        }
                    });
                }
                //获取其他城市组的城市,已选的城市，不能再选
                var sibilingCityInfo = [];
                //获取其他城市组的城市组名称，已设置的城市组名称，不能再设置
                var sibilingGroupName = [];
                _this.parents('td').find('.cityList:visible').each(function(){
                    if($(this).find('.city_name').html() == '全国' || $(this).find('.city_name').html() == '其他城市'){
                        return;
                    }else{
                        var DetailArea = JSON.parse($(this).attr('AdsaleareaGroup')).DetailArea;
                        sibilingGroupName.push(JSON.parse($(this).attr('AdsaleareaGroup')).GroupName);
                        for(var i=0;i<DetailArea.length;i++){
                            sibilingCityInfo.push(DetailArea[i].CityName)
                        }

                    }
                });
                sibilingGroupName = sibilingGroupName.join(',');

                $('.keep').off('click').on('click',function(event){
                    event.preventDefault();
                    //每次新增的时候重置集合 新添加的城市
                    DetailArea = [];
                    // //点击保存操作
                    //城市组命名部分
                    var val = $.trim($("#name").val());
                    //城市组命名的判断
                    if(val){
                        if(sibilingGroupName.indexOf(val) != -1){
                            layer.msg('城市组名称不能重复');
                            return;
                        }else{
                            var reg_location = /^([\u4e00-\u9fa5]|[0-9]|[a-z]|[A-Z]|\-|\_|\/){1,6}$/;
                            if(!reg_location.test(val)){
                                layer.msg("请输入汉字、数字、字母，特殊符号：“-”“_”“/”组成的名字，长度不超过6",{time:2000});
                                return
                            }
                            if($('.selectdCity ul li').length < 1){
                                layer.msg('请选择要添加到此城市组的城市',{time:2000})
                                return;
                            }
                            $('.selectdCity ul li').each(function(index,item){
                                AdSaleAreaGroup.DetailArea.push({
                                    ProvinceId: -2,
                                    ProvinceName: "",
                                    IsPublic:0,
                                    CityId: $(this).attr("city_id"),
                                    CityName: $.trim($(this).html())
                                })
                            })
                            //设置城市组名称
                            AdSaleAreaGroup.GroupName = val;
                            //
                            var b = JSON.stringify(AdSaleAreaGroup).split('').join('');
                            //在页面中插入城市组名
                            _this.parents('.adv').find('.AdSaleAreaGroup .cityList:last').after('<div class="cityList ad_close1" AdSaleAreaGroup='+(b)+' originalSaleAreaGroup="" style="margin-right: 5px;">'+'<span class="city_name" style="cursor:pointer">'+val+'</span>'+'&nbsp;<span class="delCity" style="cursor:pointer"><img src="/ImagesNew/close_h1.png"></span>'+'</div>');
                            //如果加入城市组之后，只有此城市和全国，则显示其他城市
                            if(_this.parents('.adv').find('.AdSaleAreaGroup .cityList:visible').length == 2){
                                _this.parents('.adv').find('.AdSaleAreaGroup .cityList').each(function(){
                                    if($(this).children('span:first').html() == '其他城市'){
                                        $(this).show();
                                        return false;
                                    }
                                });
                            }
                            // 将数据保存
                            $.closePopupLayer('popLayerDemo');
                        }
                    }else{
                        layer.msg('请输入自定义城市组的名称');
                    }
                    //点击再次编辑
                    operateCity();
                })
            }
        })
        changeHeight();
    })
    operateCity();
    //编辑、删除城市
    function operateCity(){
        $(".city_name").off('click').on("click",function(){
            var that = $(this);
            if($.trim(that.html()) == '全国' || $.trim(that.html()) == '其他城市'){
                return
            }
            $.openPopupLayer({
                name: "popLayerDemo",
                url: "chooseCity.html",
                success: function () {
                /*弹层关闭*/
                $('.close').each(function(){
                    $(this).off('click').on('click',function(event){
                        event.preventDefault();
                        $.closePopupLayer('popLayerDemo');
                    })
                });
                //弹层打开后，把城市组已选的城市信息显示出来
                var grounpInfo = JSON.parse(that.parents('.cityList').attr('AdsaleareaGroup'));

                //所有已选城市ID
                var otherCityId = [];
                for(var k=0;k<grounpInfo.DetailArea.length;k++){
                    otherCityId.push(grounpInfo.DetailArea[k].CityId)
                }
                otherCityId = otherCityId.join(',');
                //把已选择的城市名称显示出来
                $('#name').attr('GroupId',grounpInfo.GroupId).val(grounpInfo.GroupName);
                if(grounpInfo.GroupId != -2){
                    $('#name').attr('disabled','disabled');
                }
                //怎么判断此城市组是默认渲染的还是后来添加的：默认渲染的城市组有ID
                for(var i=0;i<grounpInfo.DetailArea.length;i++){
                    if(otherCityId && otherCityId.indexOf(grounpInfo.DetailArea[i].CityId) != -1){
                        if(grounpInfo.DetailArea[i].IsPublic == 0){
                            $('.selectdCity ul').append('<li class="num_name"  title="点击移除" IsPublic=0 city_id="'+grounpInfo.DetailArea[i].CityId+'">'+grounpInfo.DetailArea[i].CityName+'</li>')
                        }else{
                            $('.selectdCity ul').append('<li class="num_name" IsPublic=1 city_id="'+grounpInfo.DetailArea[i].CityId+'">'+grounpInfo.DetailArea[i].CityName+'</li>')
                        }
                    }
                    //判断所选城市是否可删除

                    $('.add_sell_l .num_name').each(function(){
                        if($.trim($(this).text()) == grounpInfo.DetailArea[i].CityName){
                            $(this).addClass('red');
                        }
                    });
                }
                //获取其他城市组的城市,已选的城市，不能再选
                var sibilingCityInfo = [];
                //获取其他城市组的城市组名称，已设置的城市组名称，不能再设置
                var sibilingGroupName = [];
                that.parents('.cityList').siblings('.cityList:visible').each(function(){
                    if($(this).find('.city_name').html() == '全国' ||  $(this).find('.city_name').html() == '其他城市'){
                        return
                    }else{
                        var DetailArea = JSON.parse($(this).attr('AdsaleareaGroup')).DetailArea;
                        sibilingGroupName.push(JSON.parse($(this).attr('AdsaleareaGroup')).GroupName);
                        for(var i=0;i<DetailArea.length;i++){
                            sibilingCityInfo.push(DetailArea[i].CityName)
                        }
                    }
                });
                sibilingGroupName = sibilingGroupName.join(',');
                //其他城市组有的城市，不能再添加，所以默认不显示
                for(var j=0;j<sibilingCityInfo.length;j++){
                    $('.add_sell_l .num_name').each(function(){
                        if($.trim($(this).text()) == sibilingCityInfo[j]){
                            $(this).parents('.sort_list').remove();
                        }
                    });
                }
                //编辑点击保存时的操作
                $('.keep').off('click').on('click',function(){
                    event.preventDefault();
                    //城市组命名部分
                    var val = $("#name").val();
                    //城市组命名的判断
                    if(!val){
                        layer.msg('请输入自定义城市组的名称');
                        return;
                    }else {
                        if(sibilingGroupName.indexOf(val) != -1){
                            layer.msg('城市组名称不能重复');
                            return;
                        }else{
                            var reg_location = /^([\u4e00-\u9fa5]|[0-9]|[a-z]|[A-Z]|\-|\_|\/){1,6}$/;
                            if(!reg_location.test(val)){
                                layer.msg("请输入汉字、数字、字母，特殊符号：“-”“_”“/”组成的名字，长度不超过6",{time:2000});
                                return
                            }
                            //修改后已选的城市数组    修改后 除了页面一开始渲染的时候就有的城市，还添加的城市
                            var selectdCity = [];
                            //页面一开始渲染的时候就有的城市，长度
                            var originalCityLen = grounpInfo.DetailArea.length;
                            $('.selectdCity ul li').each(function(index,item){
                                //如果已选城市没有变动，则不修改已选城市数据
                                var obj = {
                                    ProvinceId: -2,
                                    ProvinceName:"",
                                    CityId:$(this).attr('city_id'),
                                    IsPublic:$(this).attr('IsPublic'),
                                    CityName: $.trim($(this).html())
                                }
                                selectdCity.push(obj);

                            })
                            //无论是否有其他后来选择的城市，都加selectedOtherInfo属性，
                            //传值的时候判断，如果originalSaleAreaGroup=selectedOtherInfo，说明没有修改，不传值，如果修改了，获取selectedOtherInfo里所有IsPublic=0的城市，传给后台
                            //存储所有已选城市信息
                            var GroupInfo = {
                                "GroupId": grounpInfo.GroupId,
                                "GroupType":1,
                                "IsPublic":0,
                                "GroupName": val,
                                "DetailArea": selectdCity
                            }
                            that.parents('.cityList').attr('AdSaleAreaGroup',JSON.stringify(GroupInfo));
                            //在页面中修改城市组名
                            that.html(val);
                            $.closePopupLayer('popLayerDemo');
                            }
                        }
                    })
                }
            })
        })
        //删除后，将selectedOtherInfo里DetailArea置为空数组，后台要空数组
        $('.delCity').off('click').on('click',function(){
            var AdSaleAreaGroup = JSON.parse($(this).parents('.cityList').attr('AdSaleAreaGroup'));
            var afterDelInfo = {
                "GroupId": AdSaleAreaGroup.GroupId,
                "GroupType":AdSaleAreaGroup.GroupType,
                "IsPublic":AdSaleAreaGroup.IsPublic,
                "GroupName": AdSaleAreaGroup.GroupName,
                "DetailArea":[]
            }
            //删除之前获取父亲
            var par = $(this).parents('.adv');
            //删除，直接remove.
            $(this).parents('.cityList').remove();

            //如果删除后只剩下全国和其他城市
            if(par.find('.AdSaleAreaGroup .cityList:visible').length == 2){
                par.find('.AdSaleAreaGroup .cityList').each(function(){
                    if($(this).children('span').html() == '其他城市'){
                        $(this).hide();
                    }
                });
            }
        })
    }


    //当点击更新入库时，需要验证：1、轮播数在1-20.且大于左侧，2、必须要上传图例（>1）3、起投天次在1-365
    function dataVerification(event){
        var flag = true;
        //轮播数（当左侧有数据时，进行验证）
        if($('.mp_left').find('.emplate_table table').length != 0){
            var leftLun = $('.mp_left').find('table .CarouselCount');
            var val = $.trim(event.parents('.adv').find('.CarouselCount input').val());
            if(val < 1 || val > 20){
                layer.msg('轮播数应在1-20之间');
                flag = false;
            }
            if( val< leftLun.attr('CarouselCount')){
                layer.msg('轮播数不得小于'+leftLun.attr('CarouselCount'),{time:2000});
                flag = false;
            }
        }else{
            var curLun = $.trim(event.parents('.adv').find('.CarouselCount input').val());
            if(curLun < 1 || curLun > 20){
                layer.msg('轮播数应在1-20之间');
                flag = false;
            }
        }
        //如果已经验证轮播数不符合要求，只提示轮播数错误信息，不再继续验证示例图
        if(flag == false){
            return false;
        }

        //起投天次
        var dateLen = $.trim(event.parents('.adv').find('.AdDisplayLength input').val());
        if( dateLen && (dateLen < 1 || dateLen >365)){
            layer.msg('起投天/次应在1-365之间');
            flag = false;
        }
        return flag;
    }
    //获取数据
    function getData(NextAdBaseTempId,NextAdBaseTempId) {

        //此渲染必须是同步的
        /*获取当前页面后还有几个表格,

        如果<3,通过数据总数（totalCount）和页面上显示的表格总数(displayNum)进行比较
        if(displayNum<totalCount) 就请求数据，
        否则不请求

        请求时，需要传pageIndex,后台每次返回pageSize=20条数据，则共有（Math.ceil(totalCount/pageSize)）页
        当前页面上数据的个数是displayNum,则已显示（displayNum/pageSize）页数据

        传pageIndex = displayNum/pageSize + 1
        */
        //一共渲染的表格个数（即页面上显示的表格总数，包含隐藏的表格）
        var displayNum = $('.mp_right .adv').length;
        //获取总数据个数
        var totalLength = $('.swiper-wrapper').attr('TotleCount');
        //当前页面后隐藏的表格个数
        var length = $('.swiper-slide-next').nextAll('.adv').length;
        var upData = null;
        //点行审核：传AdTempId，不传AdTempIdList
        if(userData.isMany == 0){
            upData = {
                pageIndex:displayNum/12 + 1,
                pageSize:12,
                BaseMediaId:userData.BaseMediaId,
                AdTempId:userData.AdTempId
            }
        //点批量审核，传AdTempId=-2，AdTempIdList = 获取到的值
        }else{
            upData = {
                pageIndex:displayNum/12 + 1,
                pageSize:12,
                BaseMediaId:userData.BaseMediaId,
                AdTempId:-2,
                AdTempIdList:userData.AdTempId
            }
        }
        if(NextAdBaseTempId && NextAdBaseTempId){
            upData = {
                pageIndex:displayNum/12 + 1,
                pageSize:12,
                BaseMediaId:NextAdBaseTempId,
                AdTempId:NextAdBaseTempId
            }
        }
        if(length < 4 && length >0 && displayNum < totalLength){
            clickCount++;
            $.ajax({
                url:'/api/Template/list',
                type:'get',
                data:upData,
                async: false,
                dataType: 'json',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                success:function(data){
                    if(data.Status == 0){
                        $('.mp_right .swiper-wrapper .swiper-slide:last').after(ejs.render($('#Temp').html(), {data: data.Result.List,count:clickCount}));
                        var len = displayNum;
                        for(var l=0;l<data.Result.List.length;l++){
                            $('.mp_right').find('table').eq(''+len+'').attr('result',JSON.stringify(data.Result.List[l]));
                            len++;
                        }
                        $('.swiper-wrapper').attr('TotleCount',data.Result.TotleCount);
                        compareAndSign();
                        modifyModule();
                        auditTemp();
                        var swiper = new Swiper('.swiper-container', {
                            nextButton: '.arrow_r',
                            prevButton: '.arrow_l',
                            slidesPerView: 3,
                            centeredSlides: false,
                            paginationClickable: true,
                            spaceBetween: 0
                        });
                        for(var i=0;i<50;i++){
                            for(var j=0;j<3;j++){
                                uploadImg('imgDefault'+i+j);
                                uploadImg('imgAdd'+i+j);

                            }
                        }
                        //对于新渲染的数据，上传图片时，设置了另外的ID，需要重新写上传图片
                        for(var i=0;i<3;i++){
                            for(var j=0;j<3;j++){
                                for(var q=0;q<50;q++){
                                    uploadImg('imgDefault'+i+j+q);
                                    uploadImg('imgAdd'+i+j+q);
                                }

                            }
                        }
                    }
                }
            })
        }
    }

    // 只能输入数字
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
    /*只能输入数字，结束*/
    //上传图片
    function uploadImg(id,img) {
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
                // 'buttonImage':'/images/icon63.png',
                'buttonText': '',
                // 'buttonClass': 'allBtn_file',
                'width': 60,
                'height': 60,
                'fileTypeDesc': '支持格式:xls,xlsx,jpg,jpeg,png,gif',
                'fileTypeExts': '*.xls;*.xlsx;*.jpg;*.jpeg;*.png;*.gif;',
                fileSizeLimit:'10MB',
                'fileCount':1,
                'queueSizeLimit': 1,
                "queueID":'imgShow',
                'scriptAccess': 'always',
                'IsGenSmallImage':1,//生成缩略图
                'overrideEvents' : [ 'onDialogClose'],
                'formData': { Action: 'BatchImport', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')), IsGenSmallImage: 1 },
                // 'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
                'onQueueComplete': function (event, data) {
                    //enableConfirmBtn();
                },
                'onQueueFull': function () {
                    alert('您最多只能上传1个文件！');
                    return false;
                },
                //检测FLASH失败调用
                'onFallback':function(){
                    alert('您未安装FLASH控件，无法上传图片！');
                    return false;
                },
                //上传成功后返回的信息
                'onUploadSuccess': function (file, data, response) {
                    if (response == true) {
                        var json = $.evalJSON(data);
                        $('#'+id).prev().attr('src',json.Msg.split('|')[0]);
                        $('#'+id).next().find('img').removeClass('addPicture').addClass('delPicture').attr({'src':'../ImagesNew/icon64.png','title':'删除'}).css('z-index',100);
                        $('#'+id).css('z-index',0);
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
    function changeHeight(){
        //获取所有table里每行高度最高的行高，当前行的所有同级行，高度都设为最高高度
        for(var i=0;i<15;i++){
            var heightArr = [];
            $('table').each(function(){
                var h = $(this).find('tr:eq('+i+')')[0].offsetHeight;
                heightArr.push(h);
            });
            var maxHeight = heightArr[0];
            for(var k=0;k<heightArr.length;k++){
                if(heightArr[k]>maxHeight){
                    maxHeight = heightArr[k]
                }
            }
            $('table').each(function(){
                $(this).find('tr:eq('+i+')').height(maxHeight);
            });
        }
    }
    // 获取url？后面的参数
    function GetRequest() {
        var url = location.search; //获取url中"?"符后的字串
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
            }
        }
        return theRequest;
    }
})
// 通过url获取文件名称
function getFileName(o){
    if(o == null) return '';
    var pos=o.lastIndexOf("/");
    var str = o.substring(pos+1);
    var pos1 = str.indexOf('$');
    return str.substr(0,pos1);
}
