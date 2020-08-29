/**
 * Created by jiangqian on 2017/3/13.
 */

// 获取当前时间
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
}

var publish = {
    isAdd:null,     //是否为新增, 0--新增，1--编辑 ,2--四大刊例后添加，3--手动输入媒体名称添加
    userID:null,
    userType:null,
    MediaType:null, //媒体类型 14001-微信 14002-APP 14003-微博 14004-视频 14005-直播
    MediaID:null,//媒体ID
    MediaName:null,
    MediaNumber:null,
    PubID:null,
    BeginTime:null, //开始时间
    EndTime:null,   //结束时间
    PurchaseDiscount:null,  //采购折扣
    SaleDiscount:null,  //销售折扣
    Prices:[],  //价格
    AppStatus:null,
    imgErr:'<img src="/images/icon21.png" /> ',
    init:function(){
        //加载页头页尾
        //获取上个页面的信息通过url;
        this.userType = CTLogin.Category;//用户类型：媒体主——29002，AE——29001，运营。媒体主没有header的列表
        this.isAdd = GetQueryString('isAdd')!=null&&GetQueryString('isAdd')!='undefined'?parseFloat(GetQueryString('isAdd')):null;//是否新增
        this.MediaID = GetQueryString('MediaID')!=null&&GetQueryString('MediaID')!='undefined'?parseFloat(GetQueryString('MediaID')):null;//媒体ID
        this.PubID = GetQueryString('PubID')!=null&&GetQueryString('PubID')!='undefined'?parseFloat(GetQueryString('PubID')):null;//刊例ID
        //console.log(this.isAdd,this.MediaID,this.MediaType,this.MediaName,this.MediaNumber,this.PubID);
        var reg01,reg02,reg03;
        if((this.isAdd==0||this.isAdd==2)&&(this.MediaID!=null)){
            reg01 = true;
        }else{
            reg01 = false;
        }
        if((this.isAdd==1)&&(this.PubID!=null)){
            reg02 = true;
        }else{
            reg02 = false;
        }
        if(this.isAdd==3){
            reg03 = true;
        }else{
            reg03 = false;
        }
        if(reg01||reg02||reg03){
            this.updateView();
        }else{
            layer.alert('页面错误,跳转到首页',{closeBtn: 0},function(){
                window.location.href = "/index.html";
            });
        }
    },
    updateView:function(){ //更新页面视图
        if(this.MediaType==14002){
            if(this.isAdd==0){
                this.AppAddPublish(false);
            }else if(this.isAdd==1){
                this.AppEditPublish();
            }else if(this.isAdd==3){
                this.AppAddPublish(true);
            }
        }else{
            if(this.isAdd==0){
                this.addPublish(false);
            }else if(this.isAdd==1){
                this.editPublish();
            }else if(this.isAdd==2){
                this.addPublish(true);
            }
        }
    },
    publishCom:function(MediaType,callback){//公共样式设置 分模块 不包括APP刊例
        if(MediaType==14002){
            if(publish.userType==29002){
                $('#isAE').hide();
            }
            callback();
        }else{
            var Number = '平台账号：',
                Name = '昵称：';
            $('table td').css('position','relative');
            var tableMedia = null;
            if(MediaType==14001){
                //微信刊例
                tableMedia = 'table#wechatPublish';
                Number = '微信号：';
                Name = '微信名称:';
                var w = window.innerWidth;
                var h = window.innerHeight;
                $('#imgDemo').css({'width':w,'height':h,'background':'rgba(0,0,0,0.5)'});
                $('a[href^= "showDemo"]').click(function(e){
                    e.preventDefault();
                    $('#imgDemo').show();
                    if($(this).attr('href')=="showDemo01"){
                        $('#imgNow').attr('src','/PublishManager/img/single.png');
                    }else if($(this).attr('href')=="showDemo02"){
                        $('#imgNow').attr('src','/PublishManager/img/more.png');
                    }else if($(this).attr('href')=="showDemo03"){
                        $('#imgNow').attr('src','/PublishManager/img/more.png');
                    }else if($(this).attr('href')=="showDemo04"){
                        $('#imgNow').attr('src','/PublishManager/img/more.png');
                    }

                });
                $('#imgDemo').click(function(){
                    $('#imgDemo').hide();
                });
            }else if(MediaType==14003){
                //微博刊例
                tableMedia = 'table#blogPublish';
                Number = '微博号：';
            }else if(MediaType==14004){
                //视频刊例
                tableMedia = 'table#videoPublish';
            }else if(MediaType==14005){
                //直播刊例
                tableMedia = 'table#livevideoPublish';
            }
            $('#Number').html(Number);
            $('#Name').html(Name);
            if(publish.userType==29002){
                $('#isAE').hide();
            }
            callback(tableMedia);
        }
    },
    addPublish:function(isAddLast){//添加刊例 不包括APP刊例
        this.publishCom(this.MediaType,callFun);
        function callFun(table){
            //隐藏 添加第二步横条
            if(isAddLast){
                $('.install #isAddLast').hide();
            }
            setAjax(
                {
                    url:'/api/Media/GetMediaDetail',
                    type:'GET',
                    data:{MediaType:publish.MediaType,MediaID:publish.MediaID}
                },
                function(msg){
                    if(msg.Status==0){
                        var data = msg.Result.MediaInfo;
                        if(data) {
                            publish.MediaName = data.Name;
                            publish.MediaNumber = data.Number;
                            $('#MediaNumber').html(publish.MediaNumber);
                            $('#MediaName').html(publish.MediaName);
                            publish.submitFun(table);
                        }else{
                            layer.alert('页面错误，跳转至首页',{closeBtn:0},function(){
                                window.location.href='/index.html';
                            });
                        }
                    }else{
                        layer.alert('页面错误，跳转至首页',{closeBtn:0},function(){
                            window.location.href='/index.html';
                        });
                    }

                }
            );
        }
    },
    editPublish:function(){//编辑刊例 不包括APP刊例
        this.publishCom(this.MediaType,callFun);
        function callFun(table){
            // 隐藏 添加第二步横条
            $('.install #isAddLast').hide();
            $('#pub-title').html('编辑刊例');
            //渲染动态数
            setAjax({
                    type:'get',
                    url:'/api/Periodication/GetPublishInfoBymediaTypeAndPubID',
                    data:{PubID:publish.PubID,MediaType:publish.MediaType},
                    dataType:'json'
                },
                function(obj){//获取信息，渲染到页面
                    if(obj.Status==0&&obj.Result.MediaID!=0){//刊例信息列表计算
                        var result = obj.Result;

                        //价格
                        var price = [];
                        $.each(result.Detail,function(v){//
                            var priceData = result.Detail[v].SecondDescrit;
                            $.each(priceData,function(p){
                                price.push({Combdimension:priceData[p].Combdimension,Price:parseInt(priceData[p].Price)});
                            })
                        });
                        $(table+' input').each(function(i){
                            var input = $(this);
                            $.each(price,function(j){
                               if(publish.MediaType == 14005){
                                    if(price[j].Combdimension.split('-')[0] == input.data('add')){
                                        if(price[j].Price!=0){
                                            input.val(price[j].Price);
                                        }
                                    }
                               }else{
                                    if(price[j].Combdimension == input.data('add')){
                                        if(price[j].Price!=0){
                                            input.val(price[j].Price);
                                        }
                                    }
                               }
                            });
                        });
                        //时间
                        publish.EndTime = getDate(result.EndTime);
                        publish.BeginTime = getDate(result.BeginTime);
                        //折扣
                        $('input#PurchaseDiscount').val((result.PurchaseDiscount*100).toFixed(2));
                        $('input#SaleDiscount').val((result.SaleDiscount*100).toFixed(2));
                        //周期
                        $('input#BeginTime').val(publish.BeginTime);
                        $('input#EndTime').val(publish.EndTime);
                        publish.MediaName = result.Name;
                        publish.MediaNumber = result.Number;
                        publish.MediaID = result.MediaID;
                        $('#MediaNumber').html(publish.MediaNumber);
                        $('#MediaName').html(publish.MediaName);
                        //输入表单验证,提交请求数据
                        publish.submitFun(table);
                    }else{
                        layer.alert('页面错误，跳转至首页',{closeBtn:0},function(){
                            window.location.href='/index.html';
                        });
                    }
                }
            )
        }
    },
    testReg:function(){
        //正则匹配
        //价格表单验证失去焦点时，判断参数是否为  大于0 的整数——请填写大于0的整数
        //var input = table+' input';
        //var errorMsg = '';
        //var priceReg = false,discountReg = false,timeReg = false;
        //$(table).on('blur','input',function(){
        //    var val = $(this).val();
        //    if(val!==""){
        //        var reg  = new RegExp("^[0-9]*[1-9][0-9]*$");
        //        if(reg.test(val)){
        //            priceReg = true;
        //            $('div#tableErr').hide();
        //        }else{
        //            errorMsg = '请填写大于0的整数';
        //            $('div#tableErr').html('价格为大于0的整数').show();
        //            $(this).focus();
        //            priceReg = false;
        //        }
        //    }else{
        //        priceReg = false;
        //    }
        //});
        //$('input[name="Discount"]').focus(function(){//判断价格是否为空
        //    var totalVal = '';
        //    $(input).each(function(i){
        //        totalVal+=$(this).val();
        //    });
        //
        //    if(totalVal==""){
        //        $(input).eq(0).focus();
        //        priceReg = false;
        //        $('div#tableErr').html('价格为必填项，至少填写一个').show();
        //
        //    }else{
        //        priceReg = true;
        //        $('div#tableErr').hide();
        //    }
        //});
        ////折扣验证表单失去焦点时，判断参数是否为  0——100的两位小数
        //$('input[name="Discount"]').blur(function(){
        //    var reg =/^([1-9]\d?(\.\d{1,2})?|0\.\d{1,2}|100)$/;//**********************************************************
        //    var val = $(this).val();
        //    if(val==""){
        //        $('li#discountErr').show().html('折扣不能为空');
        //        $(this).focus();
        //        discountReg = false;
        //    }else{
        //        if(reg.test(val)){
        //            discountReg = true;
        //            $('li#discountErr').hide();
        //        }else{
        //            errorMsg = '请填写0-100的数';
        //            $('li#discountErr').show().html('请填写0-100的数');
        //            $(this).focus();
        //            discountReg = false;
        //        }
        //    }
        //});
        var nowdate = new Date();
        $('#BeginTime').off('click').on('click',function(){
            laydate({
                // fixed:true,
                elem:'#BeginTime',
                choose:function(date){
                    if($('#EndTime').val() && date>$('#EndTime').val()){
                        $('#BeginTime').val('');
                        $('#timeErr').html(publish.imgErr+'起始时间不能大于结束时间！').show();
                    }else{
                        $('#timeErr').html('').hide();
                    }
                }
            })
        });
        $('#EndTime').off('click').on('click',function(){
            laydate({
                // fixed:true,
                elem:'#EndTime',
                choose:function(date){
                    console.log(date < new Date().format('yyyy-MM-dd'));
                    if(new Date().format('yyyy-MM-dd')&&date<=new Date().format('yyyy-MM-dd')){
                        $('#EndTime').val('');
                        $('#timeErr').html(publish.imgErr+'结束时间不能小于今天！').show();
                    }else{
                        $('#timeErr').html('').hide();
                    }
                }
            })
        });
    },
    clickReg:function(table){
        var input = table+' input';
        var priceReg = false,PurchaseDiscountReg = false,SaleDiscountReg = false,timeReg = false;
        var totalVal = '';
        $(input).each(function(i){
            var val = $(this).val();
            totalVal+=val;
        });
        if(totalVal==""){
            priceReg = false;
            $('div#tableErr').html(publish.imgErr+'价格不能为空，至少填写一项').show();
        }else{
            priceReg = true;
            $('div#tableErr').hide();
        }
        $(input).each(function(i){
            var val = $(this).val();
            totalVal+=val;
            if(val!==""){
                var reg  = new RegExp("^[0-9]*[1-9][0-9]*$");
                if(reg.test(val)){
                    priceReg = true;

                    $('div#tableErr').hide();
                }else{
                    priceReg = false;
                    $('div#tableErr').show().html(publish.imgErr+'价格为大于0的整数');
                }
            }else{
                //priceReg = true;
                //$('div#tableErr').hide();
            }
        });
        //折扣验证
        function discount(idName,ErrName){
            var reg =/^([1-9]\d?(\.\d{1,2})?|0\.\d{1,2}|100.00|100|100.0|0|0.00|0.0)$/;
            var val = $('input#'+idName).val();
            var RegName=false;
            if(val==""){
                RegName = false;
                $('li#'+ErrName).show().html(publish.imgErr+'折扣不能为空');
            }else{
                if(reg.test(val)){
                    RegName = true;
                }else{
                    RegName = false;
                    $('li#'+ErrName).show().html(publish.imgErr+'折扣不能大于100或小于0，最多两位小数');
                }
            }
            return RegName
        }
        if(publish.userType==29002){
            SaleDiscountReg = discount('SaleDiscount','discountErr');
            PurchaseDiscountReg = true;
            if(SaleDiscountReg){
                $('li#discountErr').hide();
            }
        }else{
            SaleDiscountReg = discount('SaleDiscount','discountErr');
            PurchaseDiscountReg = discount('PurchaseDiscount','discountErr');
            if(SaleDiscountReg&&PurchaseDiscountReg){
                $('li#discountErr').hide();
            }
        }
        //时间验证————
        var BeginTime = $('input[name="BeginTime"]').val();
        var EndTime = $('input[name="EndTime"]').val();
        if(BeginTime==""||EndTime==""){
            timeReg = false;
            $('#timeErr').html(publish.imgErr+'请填写刊例执行周期').show();
        }else{
            BeginTime = new Date(BeginTime);
            EndTime = new Date(EndTime);
            if(BeginTime<=EndTime){
                timeReg = true;
                $('#timeErr').hide();
            }else{
                timeReg = false;
            }
        }
        return priceReg&&PurchaseDiscountReg&&SaleDiscountReg&&timeReg;
    },
    submitFun:function(table){
        publish.testReg();
        var input = table+' input';
        $('#submit').click(function(e){//点击时候验证所填内容的正确与否
            e.preventDefault();
            if(publish.clickReg(table)){//发送请求
                publish.Price = [];
                publish.BeginTime = $('input#BeginTime').val()+' 00:00:00';
                publish.EndTime = $('input#EndTime').val()+' 00:00:00';
                publish.PurchaseDiscount =$('input#PurchaseDiscount').val()? parseFloat($('input#PurchaseDiscount').val())/100:null;
                publish.SaleDiscount = parseFloat($('input#SaleDiscount').val())/100;
                $(input).each(function(i){
                    if($(this).val()&&$(this).val().length){
                        publish.Prices.push($(this).data('add')+'-'+$(this).val());
                    }
                });
                var addData = { "Publish":{
                    "PubID": publish.PubID,
                    "MediaType": publish.MediaType,
                    "MediaID": publish.MediaID,
                    "MediaName":publish.MediaName,
                    "BeginTime": publish.BeginTime,
                    "EndTime": publish.EndTime,
                    "PurchaseDiscount": publish.PurchaseDiscount,
                    "SaleDiscount": publish.SaleDiscount
                },
                    "Prices":publish.Prices};
                setAjax({
                        type:'POST',
                        url:'/api/Publish/ModifyPublish',
                        data:addData
                    },
                    function(result){
                        if(result.Status==0){
                            var href = '';
                            if(publish.MediaType==14001){
                                href = '/PublishManager/pricelist-wechat.html'
                            }else if(publish.MediaType==14003){
                                href = '/PublishManager/pricelist-sina.html';
                            }else if(publish.MediaType==14004){
                                href = '/PublishManager/pricelist-video.html';
                            }else if(publish.MediaType==14005){

                                href = '/PublishManager/pricelist-zhibo.html';
                            }
                            window.location.href=href;
                        }else{
                            layer.alert('操作失败，请重新操作');
                        }
                    }
                )
            }
        });
    },
    AppReg:function(){
        //验证是否通过
        var nameReg = false,PurchaseDiscountReg = false,SaleDiscountReg=false,timeReg = false;
        if(publish.isAdd==3){
            if($('#MediaName input').val()==""){
                $('#NameErr').html(publish.imgErr+'请输入媒体名称');
                nameReg = false;
            }else{
                nameReg = true;
                if($('#MediaName input').val().length<=50){
                    $('#NameErr').html("");
                    nameReg = true;
                }else{
                    $('#NameErr').html(publish.imgErr+'媒体名称在50个汉字以内');
                    nameReg = false;
                }
            }
        }else{
            nameReg = true;
        }
        //折扣验证
        function discount(idName,ErrName){
            var reg =/^([1-9]\d?(\.\d{1,2})?|0\.\d{1,2}|100.00|100|100.0|0.00|0.0|0)$/;
            var val = $('input#'+idName).val();
            var RegName=false;
            if(val==""){
                RegName = false;
                $('li#'+ErrName).show().html(publish.imgErr+'折扣不能为空');
            }else{
                if(reg.test(val)){
                    RegName = true;
                    $('li#'+ErrName).hide();
                }else{
                    RegName = false;
                    $('li#'+ErrName).show().html(publish.imgErr+'折扣不能大于100或小于0，最多两位小数');
                }
            }
            return RegName
        }
        if(publish.userType==29002){
            SaleDiscountReg = discount('SaleDiscount','SaleDiscountErr');
            PurchaseDiscountReg = true;
        }else{
            SaleDiscountReg = discount('SaleDiscount','SaleDiscountErr');
            PurchaseDiscountReg = discount('PurchaseDiscount','PurchaseDiscountErr');
        }

        var BeginTime = $('input[name="BeginTime"]').val();
        var EndTime = $('input[name="EndTime"]').val();
        if(BeginTime==""||EndTime==""){
            timeReg = false;
            $('#timeErr').html(publish.imgErr+'请填写刊例执行周期').show();
        }else{
            BeginTime = new Date(BeginTime);
            EndTime = new Date(EndTime);
            if(BeginTime<=EndTime){
                timeReg = true;
                $('#timeErr').hide();
            }else{
                timeReg = false;
                $('#timeErr').html(publish.imgErr+'开始时间应小于结束时间').show();
            }
        }
        return nameReg&&PurchaseDiscountReg&&SaleDiscountReg&&timeReg;
    },
    AppKeywords:function(){
        $('#MediaName input').keyup(function(e) {
            if (e.keyCode === 13 || e.keyCode === 32 || e.keyCode === 8) {
                var val = $(this).val();
                if (val !== "") {
                    setAjax({
                            url: '/api/Media/GetMediaPairs',
                            type: 'get',
                            data: {MediaType: 14002, Name: val}
                        },
                        function (msg) {
                            if (msg.Status == 0) {
                                var list = msg.Result;
                                $('#searchName li').remove();
                                if (list && list.length > 0) {
                                    $.each(list, function (i) {
                                        $('#searchName').show();
                                        $('#searchName').append('<li>' + list[i].Name + '</li>');
                                    });
                                    $('#searchName li').css({'cursor': 'pointer', 'width': '100%','box-sizing':'border-box', padding: '0 5px','margin':'0'});
                                    $('#searchName').on('mouseover', 'li', function () {
                                        $(this).css({'background': "#d9dee0"});
                                    });
                                    $('#searchName').on('mouseout', 'li', function () {
                                        $(this).css({background: "#fff"});
                                    });
                                    $('#searchName').on('mousedown', 'li', function () {
                                        $(this).css({"background":"#d9dee0"});
                                        $('#MediaName input').val($(this).html());
                                        $('#searchName').hide();
                                    });
                                }
                            }
                        }
                    )
                }else{
                    $('#searchName li').remove();
                    $('#searchName').hide();
                }
            }
        });
        $('#MediaName input').bind('blur',function(){
            $('#searchName').hide();
        });
        $('#MediaName input').bind('focus',function(){
            if($('#searchName li')&&$('#searchName li').length>0)
                $('#searchName').show();
        });
    },
    AppSetName:function(){
        setAjax(
            {
                url:'/api/Media/GetMediaDetail',
                type:'GET',
                data:{MediaType:publish.MediaType,MediaID:publish.MediaID}
            },
            function(msg){
                if(msg.Status==0){
                    var data = msg.Result.MediaInfo;
                    if(data) {
                        publish.MediaName = data.Name;
                        publish.MediaNumber = data.Number;
                        $('#MediaName').html(publish.MediaName);
                        $('#MediaName').prev().find('span').hide();
                    }
                }else{
                    layer.alert('页面错误，跳转至首页',{closeBtn:0},function(){
                        window.location.href='/index.html';
                    });
                }
            }
        );
    },
    AppAddPublish:function(isInputName){
        this.publishCom(this.MediaType,callFun);
        function callFun(){
            if(isInputName){//不带媒体ID号，通过媒体名称，创刊例。创建成功继续添加广告位；
                // 若已存在广告位为0，提示该刊例广告位为0，跳转到添加广告位页面；
                //若已存在，广告位不为0，则跳转到广告位列表页面；
                //若媒体名称不存在，提示媒体名称错误；
                publish.AppKeywords();
            }else{//带媒体ID号及媒体名称添加APP刊例，为固定了。有两个页面但传输数据相同,添加成功后均需返回参数，媒体名称、刊例类型、刊例执行期;
                publish.AppSetName();
            }
            publish.AppSubmit();
        }
    },
    AppEditPublish:function(){
        this.publishCom(this.MediaType,callFun);
        function callFun(list){
            $('#submit').html('保存');
            //1、ajax请求 数据渲染;
            //提交成功后返回列表；
            setAjax({
                    type:'get',
                    url:'/api/Periodication/GetPublishInfoBymediaTypeAndPubID',
                    data:{PubID:publish.PubID,MediaType:publish.MediaType},
                    dataType:'json'
                },
                function(obj){//获取信息，渲染到页面
                    if(obj.Status==0&&obj.Result.MediaID!=0){//刊例信息列表计算
                        var result = obj.Result;
                        //时间
                        publish.EndTime = getDate(result.EndTime);
                        publish.BeginTime = getDate(result.BeginTime);
                        publish.MediaID = result.MediaID;
                        publish.MediaName = result.Name;
                        //折扣
                        $('input#PurchaseDiscount').val((result.PurchaseDiscount*100).toFixed(2));
                        $('input#SaleDiscount').val((result.SaleDiscount*100).toFixed(2));
                        //周期
                        $('input#BeginTime').val(publish.BeginTime);
                        $('input#EndTime').val(publish.EndTime);
                        $('#MediaName').html(result.Name);
                        $('#MediaName').prev().find('span').hide();
                        //输入表单验证,提交请求数据
                        publish.AppSubmit();
                    }else{
                        layer.alert('页面错误，跳转至首页',{closeBtn:0},function(){
                            window.location.href='/index.html';
                        });
                    }
                }
            )
        }
    },
    AppSubmit:function(){
        publish.testReg();
        $('#submit').click(function(e){
            e.preventDefault();
            if(publish.AppReg()){
                publish.BeginTime = $('input#BeginTime').val()+' 00:00:00';
                publish.EndTime = $('input#EndTime').val()+' 00:00:00';
                if($('input#PurchaseDiscount').val()!=""){
                    publish.PurchaseDiscount = parseFloat($('input#PurchaseDiscount').val())/100;
                }
                publish.SaleDiscount = parseFloat($('input#SaleDiscount').val())/100;
                if(publish.isAdd==3){
                    publish.MediaName = $('input[name="MediaName"]').val();
                }
                var AppData ={
                    Publish:{
                        PubID:publish.PubID,
                        MediaType:publish.MediaType,
                        MediaID:publish.MediaID,
                        MediaName:publish.MediaName,
                        BeginTime:publish.BeginTime,
                        EndTime:publish.EndTime,
                        PurchaseDiscount:publish.PurchaseDiscount,
                        SaleDiscount:publish.SaleDiscount
                    },
                    Prices:null
                } ;
                setAjax({
                        url:'/api/Publish/ModifyPublish',
                        type:'POST',
                        data:AppData
                    },
                    function(msg){
                        if(msg.Status==0){
                            //是否为编辑还是添加
                            publish.PubID = msg.Result.PubID;
                            if(publish.isAdd==1){
                                //跳转到——媒体刊例列表-APP页面
                                window.location.href = '/PublishManager/auditPublishAPP.html?MediaID='+publish.MediaID;
                            }else{
                                //继续添加广告位
                                window.location.href = '/PublishManager/addEditAdSpace.html?PubID='+publish.PubID;
                            }
                        }else if(msg.Status==1){
                            publish.PubID = msg.Result.PubID;
                            $('#submit').attr('disabled',true);
                            layer.confirm('您已创建刊例，是否跳转添加广告位？',{btn:['是','否']},
                                function(index){
                                    layer.close(index);
                                    window.location.href="/PublishManager/addEditAdSpace.html?PubID="+publish.PubID;//
                                },
                                function(){
                                    $('#MediaName input').focus();
                                }
                            );

                        }else if(msg.Status==2){
                            publish.PubID = msg.Result.PubID;
                            publish.MediaID = msg.Result.MediaID;
                            layer.confirm('该媒体已创建刊例，是否跳转至广告位列表？',{btn:['是','否']},
                                function(index){
                                    layer.close(index);
                                    window.location.href="/PublishManager/auditPublishAPP.html?MediaID="+publish.MediaID;
                                },
                                function(){
                                    $('#MediaName input').focus();
                                }
                            );
                        }else if(msg.Status==3){
                            layer.alert('媒体名称错误，添加失败',{closeBtn:0},function(index){
                                layer.close(index);
                                $('#MediaName input').focus();
                            });

                        }else if(msg.Status==4){
                            layer.alert('添加失败，请重新添加',{closeBtn:0},function(){
                                layer.close(index);
                                $('#MediaName input').focus();
                            });
                        }
                    }
                );
            }
        })
    }
};

//广告位添加和编辑的页面——页面调用adSpace.init();
var adSpace = {
    isAdd:0, //0--新增，1-编辑
    MediaID:null,
    MediaType:14002,
    MediaName:null,
    ADDetail:1,//广告位ID 编辑时候有值
    ADDetailID:null,
    PubID:null,//刊例ID
    AdPosition:null,//广告位位置
    AdForm:null,//广告位形式
    DisplayLength:null,//时长
    CanClick:null,//是否可点击
    CarouselCount:null,//轮播数
    PlayPosition:null,//位置
    DailyExposureCount:null,//日常曝光数
    CPM:null,//是否为CPM方式
    CarouselPlay:null,//是否轮播
    DailyClickCount:null,//日常点击数
    CPM2:null,//是否为CPM方式
    CarouselPlay2:null,//是否轮播
    ThrMonitor:[],//第三方检测
    SysPlatform:[],//系统平台
    Style:null,//	样式
    IsDispatching:null,//	是否配送
    ADShow:null,//广告位展示
    ADRemark:null,//广告位说明
    AcceptBusinessIDs:[],//接受行业ID
    NotAcceptBusinessIDs:[],//不接受行业ID
    IsCarousel:null,//是否轮播
    SaleType:null,//售卖方式
    Price:null,//价格
    BeginPlayDays:null,//起投天数
    imgErr:publish.imgErr,
    init:function(){
        //获取页面数据渲染到页面---PubID为必传项，如果有传ADDetailID为编辑页面，没传为新增页面
        this.PubID = GetQueryString('PubID')!=null&& GetQueryString('PubID')!='undefined'?parseInt(GetQueryString('PubID')):null;
        this.ADDetailID = GetQueryString('ADDetailID')!=null&& GetQueryString('ADDetailID')!='undefined'?parseInt( GetQueryString('ADDetailID')):null;
        if(GetQueryString('ADDetailID')){
            this.isAdd = 1;
        }else{
            this.isAdd = 0;
        }
        if(this.PubID){
            this.updateView();
        }else{
            layer.alert('页面错误,跳转到首页',{closeBtn: 0},function(){
                window.location.href = "/index.html";
            })
        }
    },
    updateView:function(){
        //公共部分渲染——媒体名称、刊例执行期——通过媒体类型和刊例ID查询信息
        setAjax(
            {
                url:'/api/Periodication/GetPublishInfoBymediaTypeAndPubID',
                type:'GET',
                data:{PubID:adSpace.PubID,MediaType:14002}
            },
            function(msg){
                if(msg.Status==0){
                    var data = msg.Result;
                    $('#MediaName').html(data.Name);
                    $('#BeginTime').html(getDate(data.BeginTime));
                    $('#EndTime').html(getDate(data.EndTime));
                    adSpace.MediaID = data.MediaID;
                    if(msg.Result.MediaID!=null&&msg.Result.Name!=null&&msg.Result.Number!=null){
                        //渲染到页面
                        adSpace.upLoadImg();
                        if(adSpace.isAdd==0){
                            adSpace.addFun();
                        }else if(adSpace.isAdd==1){
                            adSpace.editFun();
                        }
                    }else{
                        layer.alert('页面错误，跳转至首页',{closeBtn:0},function(){
                            window.location.href='/index.html';
                        });
                    }
                }else{
                    layer.alert('页面错误，跳转至首页',{closeBtn:0},function(){
                        window.location.href='/index.html';
                    });
                }
            }
        );
    },
    testReg:function(){//-------------------页面输入内容时验证
        //验证轮播次数，勾选和不勾选
        $('input[name="CarouselCount"]').blur(function(){
            var val = $('input[name="CarouselCount"]').val();
            if(val==""){
                $('input[name="CarouselPlay"]').attr('checked',false);
                $('input[name="CarouselPlay2"]').attr('checked',false);
                $('input[name="IsCarousel"]').attr('checked',false);
            }else{
                $('input[name="CarouselPlay"]').attr('checked',true);
                $('input[name="CarouselPlay2"]').attr('checked',true);
                $('input[name="IsCarousel"]').attr('checked',true);
            }
        });
        //可点击不可点击逻辑
        if($('input[name="CanClick"]:checked').val()=='true'){
            $('#showCanClick').show();
        }else{
            $('#showCanClick').hide();
        }
        $('input[name="CanClick"]').click(function(){
            if($(this).val()=='true'){
                $('#showCanClick').show();
            }else{
                $('#showCanClick').hide();
            }
        });
        $('input[name="SaleType"]').click(function(){
            if($(this).val()==11001){
                $('#isCPD').show();
                $('input[name="IsCarousel"]').parent().show();
            }else{
                $('#isCPD').hide();
                $('input[name="IsCarousel"]').parent().hide();
            }
        });
        //图片放大镜效果
        $('#imgShow').mousemove(function(){
            if($.trim($(this).attr("src"))!= "/ImagesNew/uploadimg.png"){
                $('#BigImg').css("display","inline");
            }
        }).mouseout(function () {
            $('#BigImg').css("display","none");
        });

        //添加接受行业和不接受行业
        var closeImg = '<a  class="removeIndustry"><img src="/images/icon13.png" width="16" height="16"  onMouseOver="this.src=\'/images/icon14.png\'" onMouseOut="this.src=\'/images/icon13.png\'"></a>';
        function showIndustry(callBack){
            setAjax({
                    url:'/api/DictInfo/GetDictInfoByTypeID',
                    type:'GET',
                    data:{typeID:2}
                },
                function(msg){
                    var html = '';
                    $.each(msg.Result,function(i){
                        html+='<li style="width:250px"><input name="" type="checkbox" value='+msg.Result[i].DictId+'> <span>'+msg.Result[i].DictName+'</span></li>';
                    });
                    $('#industry').html(html);
                    callBack();
                }
            )
        }
        $('#AcceptBusinessIDs').click(function(e){
            e.preventDefault();
            $.openPopupLayer({
                name:'popLayerDemo',
                url:'choseIndustry.html?r='+Math.random(),
                success:function(dd){
                    showIndustry(accept);
                    function accept(){
                        $('#closebt').click(function(){
                            $.closePopupLayer('popLayerDemo');

                        });
                        $('#backNow').click(function(e){
                            e.preventDefault();
                            $.closePopupLayer('popLayerDemo');
                        });
                        if(adSpace.NotAcceptBusinessIDs.length>0){
                            $.each(adSpace.NotAcceptBusinessIDs,function(i){
                                $('#industry input').each(function(j){
                                    if($(this).val()==adSpace.NotAcceptBusinessIDs[i]){
                                        $(this).attr('disabled',true);
                                    }
                                })
                            })
                        }
                        if(adSpace.AcceptBusinessIDs.length>0){
                            $.each(adSpace.AcceptBusinessIDs,function(i){
                                $('#industry input').each(function(j){
                                    if($(this).val()==adSpace.AcceptBusinessIDs[i]){
                                        $(this).attr('checked',true);
                                    }
                                })
                            })
                        }
                        $('#industrySave').click(function(e){
                            e.preventDefault();
                            adSpace.AcceptBusinessIDs.length=0;
                            $('#addIndustry').html('');
                            $('#industry input:checked').each(function(j){
                                adSpace.AcceptBusinessIDs.push($(this).val());
                                $('#addIndustry').append('<li data-industry="'+$(this).val()+'">'+$(this).next().html()+closeImg);
                            });
                            $.closePopupLayer('popLayerDemo');
                        });
                    }
                }
            });
        });
        $('#NotAcceptBusinessIDs').click(function(e){
            e.preventDefault();
            $.openPopupLayer({
                name:'popLayerDemo',
                url:'choseIndustry.html?r='+Math.random(),
                success:function(){
                    showIndustry(notAccept);
                    function notAccept(){
                        $('#industry li').ready(function(){
                            $('#closebt').click(function(){
                                $.closePopupLayer('popLayerDemo');
                            });
                            $('#backNow').click(function(e){
                                e.preventDefault();
                                $.closePopupLayer('popLayerDemo');
                            });
                            if(adSpace.AcceptBusinessIDs.length>0){
                                $.each(adSpace.AcceptBusinessIDs,function(i){
                                    $('#industry input').each(function(j){
                                        if($(this).val()==adSpace.AcceptBusinessIDs[i]){
                                            $(this).attr('disabled',true);
                                        }
                                    })
                                })
                            }
                            if(adSpace.NotAcceptBusinessIDs.length>0){
                                $.each(adSpace.NotAcceptBusinessIDs,function(i){
                                    $('#industry input').each(function(j){
                                        if($(this).val()==adSpace.NotAcceptBusinessIDs[i]){
                                            $(this).attr('checked',true);
                                        }
                                    })
                                })
                            }
                            $('#industrySave').click(function(e){
                                e.preventDefault();
                                adSpace.NotAcceptBusinessIDs.length=0;
                                $('#addUnIndustry').html('');
                                $('#industry input:checked').each(function(j){
                                    adSpace.NotAcceptBusinessIDs.push($(this).val());
                                    $('#addUnIndustry').append('<li data-industry="'+$(this).val()+'">'+$(this).next().html()+closeImg);
                                });
                                $.closePopupLayer('popLayerDemo');
                            });
                        });
                    }
                }
            });
        });
        $('#addIndustry').on('click','.removeIndustry',function(){
            var val = $(this).parent().data('industry');
            $(this).parent().remove();
            $.each(adSpace.AcceptBusinessIDs,function(i){
                if(adSpace.AcceptBusinessIDs[i]==val){
                    adSpace.AcceptBusinessIDs.splice(i,1);
                }
            });
        });
        $('#addUnIndustry').on('click','.removeIndustry',function(){
            var val = $(this).parent().data('industry');
            $(this).parent().remove();
            $.each(adSpace.NotAcceptBusinessIDs,function(i){
                if(adSpace.NotAcceptBusinessIDs[i]==val){
                    adSpace.NotAcceptBusinessIDs.splice(i,1);
                }
            });
        });
        //新增和编辑的区别
        if(adSpace.isAdd==1) {
            var industry = [];
            setAjax({
                    url:'/api/DictInfo/GetDictInfoByTypeID',
                    type:'GET',
                    data:{typeID:2}
                },
                function(msg){
                    industry = industry.concat(msg.Result);
                    if (adSpace.AcceptBusinessIDs.length > 0) {
                        $.each(adSpace.AcceptBusinessIDs, function (i) {
                            $.each(industry, function(j) {
                                if (industry[j].DictId==adSpace.AcceptBusinessIDs[i]) {
                                    $('#addIndustry').append('<li data-industry="' + industry[j].DictId + '">' + industry[j].DictName + closeImg + '</li>');
                                }
                            })
                        })
                    }
                    if (adSpace.NotAcceptBusinessIDs.length > 0) {
                        $.each(adSpace.NotAcceptBusinessIDs, function (i) {
                            $.each(industry, function (j) {
                                if (industry[j].DictId == adSpace.NotAcceptBusinessIDs[i]) {
                                    $('#addUnIndustry').append('<li data-industry="' + industry[j].DictId + '">' + industry[j].DictName + closeImg + '</li>');
                                }
                            })
                        })
                    }
                }
            );
        }else{
            //提示请上传图片

        }
    },
    clickReg:function(){
        var AdPositionReg , //必填，50个汉字
            AdLegendURLReg,
            AdFormReg , //必填，50个汉字
            DisplayLengthReg , //非必填，数字类型，小于300
            CarouselCountReg , //非必填，数字类型，小于20，不为空时，日均曝光量、日均点击量、售卖方式中的轮播复选框均被选中；为空时，三个复选框也均取消选中
            SysPlatformReg ,//默认都选中；至少选择一个
            PlayPositionReg ,//非必填，小于10个汉字
            DailyExposureCountReg ,//非必填，数字类型
            DailyClickCountReg,//非必填。数字类型
            StyleReg,//非必填，50个汉字
            PriceReg,//价格，必填，数字
            BeginPlayDaysReg;//非必填，数字类型
        if($('#imgShow').attr('src')==""){
            $('#imgErr').html(adSpace.imgErr+'请上传图片');
            AdLegendURLReg = false;
        }else{
            AdLegendURLReg = true;
        }
        function val(inputName){
            return $('input[name='+inputName+']').val();
        }
        function li(inputName){
            var liName = $('input[name='+inputName+']').parent().next('li');
            return liName;
        }
        var regChart = /^[1-9]\d[\u4e00-\u9fa5]*$/;
        function chartReg(inputName){
            var val = $('input[name='+inputName+']').val();
            var li =  $('input[name='+inputName+']').parent().next('li');
            var reg;
            if(val==""){
                reg = false;
                $(li).html(adSpace.imgErr+'请输入内容');
            }else{
                if(val.length<=50){
                    reg = true;
                    $(li).html('');
                }else{
                    reg = false;
                    $(li).html(adSpace.imgErr+'请输入50个以内的文字');
                }
            }
            return reg;
        }
        AdFormReg = chartReg('AdForm');
        AdPositionReg = chartReg('AdPosition');

        if(val("Style")==""||(val("Style").length>0&&val("Style").length<=100)){
            StyleReg = true;
            $(li("Style")).html('');
        }else{
            StyleReg = false;
            $(li("Style")).html(adSpace.imgErr+"请输入50个以内的文字");
        }
        if(val("PlayPosition")==""||val("PlayPosition").length<=20){
            PlayPositionReg = true;
            $(li("PlayPosition")).html('');
        }else{
            PlayPositionReg = false;
            $(li("PlayPosition")).html(adSpace.imgErr+"请输入10个以内的文字");
        }
        var regNum = /^[0-9]*[1-9][0-9]*$/;
        function numReg(inputName,err){
            var val = $('input[name='+inputName+']').val();
            var li =  $('input[name='+inputName+']').parent().next('li');
            var reg;
            if(err){
                if(val==""||val==0){
                    reg = true;
                    $('#'+err).html('');
                }else{
                    if(regNum.test(val)){
                        reg = true;
                        $('#'+err).html('');
                    }else{
                        reg = false;
                        $('#'+err).html(adSpace.imgErr+'请输入数字');
                    }
                }
                return reg;
            }else{
                if(val==""||val==0){
                    reg = true;
                    $(li).html('');
                }else{
                    if(regNum.test(val)){
                        reg = true;
                        $(li).html('');
                    }else{
                        reg = false;
                        $(li).html(adSpace.imgErr+'请输入数字');
                    }
                }
                return reg;
            }
        }
        if(numReg('DisplayLength')){
            if(val('DisplayLength')<300){
                DisplayLengthReg = true;
                $(li('DisplayLength')).html('');
            }else{
                DisplayLengthReg = false;
                $(li('DisplayLength')).html(adSpace.imgErr+'请输入小于300的数字');
            }
        }
        if(numReg('CarouselCount')){
            CarouselCountReg = true;
            if(val('CarouselCount')<20){
                CarouselCountReg = true;
                $(li('CarouselCount')).html('');
            }else{
                CarouselCountReg = false;
                $(li('CarouselCount')).html(adSpace.imgErr+'请输入小于20的数字');
            }
        }else{
            CarouselCountReg = false;
            $(li('CarouselCountReg')).html(adSpace.imgErr+'请输入小于20的数字');
        }
        DailyExposureCountReg = numReg('DailyExposureCount','DailyExposureCountErr');
        DailyClickCountReg = numReg('DailyClickCount','DailyClickCountErr');
        BeginPlayDaysReg = numReg("BeginPlayDays");
        if($('input[name="SysPlatform01"]:checked').val()==12001||$('input[name="SysPlatform02"]:checked').val()==12002){
            SysPlatformReg = true;
            $('input[name="SysPlatform02"]').parent().next().hide();
        }else{
            SysPlatformReg = false;
            $('input[name="SysPlatform02"]').parent().next().html(adSpace.imgErr+'请选择系统平台');
        }
        if(val('Price')==""){
            PriceReg = false;
            $('#priceErr').html(adSpace.imgErr+'请输入价格');
        }else{
            if(regNum.test(val('Price'))){
                PriceReg = true;
                $('#priceErr').html('');
            }else{
                PriceReg = false;
                $('#priceErr').html(adSpace.imgErr+'请输入价格');
            }
        }
        var totalReg = AdLegendURLReg&&AdPositionReg&&AdFormReg&&StyleReg&&PlayPositionReg&&DisplayLengthReg&&CarouselCountReg&&DailyExposureCountReg&&DailyClickCountReg&&BeginPlayDaysReg&&SysPlatformReg&&PriceReg;
        return  totalReg;
    },
    submitFun:function(){
        $('#submit').click(function(e){
            e.preventDefault();
            if(adSpace.clickReg()){
                //获取页面数据拼接成对象；
                //提交成功回调函数；
                adSpace.AdLegendURL = $('#imgShow').attr('src');
                adSpace.AdPosition = $('input[name="AdPosition"]').val();
                adSpace.AdForm = $('input[name="AdForm"]').val();
                adSpace.DisplayLength =  $('input[name="DisplayLength"]').val()?parseInt($('input[name="DisplayLength"]').val()):null;
                adSpace.CanClick = $('input[name="CanClick"]:checked').val();
                adSpace.CarouselCount =$('input[name="CarouselCount"]').val()?parseInt($('input[name="CarouselCount"]').val()):null;
                adSpace.PlayPosition = $('input[name="PlayPosition"]').val();
                adSpace.DailyExposureCount = $('input[name="DailyExposureCount"]').val()?parseInt($('input[name="DailyExposureCount"]').val()):null;
                adSpace.CPM = $('input[name="CPM"]:checked').val()=='true'?true:false;
                adSpace.CarouselPlay = $('input[name="CarouselPlay"]:checked').val()=='true'?true:false;
                adSpace.CPM2 = $('input[name="CPM2"]:checked').val()=='true'?true:false;
                adSpace.CarouselPlay2 = $('input[name="CarouselPlay2"]:checked').val()=='true'?true:false;
                adSpace.DailyClickCount = $('input[name="DailyClickCount"]').val()?parseInt($('input[name="DailyClickCount"]').val()):null;
                isNull('ThrMonitor01',adSpace.ThrMonitor);
                isNull('ThrMonitor02',adSpace.ThrMonitor);
                isNull('SysPlatform01',adSpace.SysPlatform);
                isNull('SysPlatform02',adSpace.SysPlatform);
                function isNull(inputName,adSpaceName){
                    var val = $('input[name='+inputName+']:checked').val();
                    if(val){
                        adSpaceName.push(val);
                    }
                }
                adSpace.Style =  $('input[name="Style"]').val();
                adSpace.IsDispatching= $('input[name="IsDispatching"]:checked').val();
                adSpace.ADShow =  $('textarea[name="ADShow"]').val();
                adSpace.ADRemark =  $('textarea[name="ADRemark"]').val();
                adSpace.IsCarousel = $('input[name="IsCarousel"]').attr('checked')?true:false;
                adSpace.SaleType = parseInt($('input[name="SaleType"]:checked').val());
                adSpace.Price = parseFloat($('input[name="Price"]').val());
                adSpace.BeginPlayDays = $('input[name="BeginPlayDays"]').val()?parseInt($('input[name="BeginPlayDays"]').val()):null;
                var data ={
                    ADDetailID:adSpace.ADDetailID,
                    PubID:adSpace.PubID,
                    AdLegendURL:adSpace.AdLegendURL,
                    AdPosition:adSpace.AdPosition,
                    AdForm:adSpace.AdForm,
                    DisplayLength:adSpace.DisplayLength,
                    CanClick:adSpace.CanClick,
                    CarouselCount: adSpace.CarouselCount,
                    PlayPosition:adSpace.PlayPosition,
                    DailyExposureCount:adSpace.DailyExposureCount,
                    CPM:adSpace.CPM,
                    CarouselPlay:adSpace.CarouselPlay,
                    DailyClickCount:adSpace.DailyClickCount,
                    CPM2:adSpace.CPM2,
                    CarouselPlay2:adSpace.CarouselPlay2,
                    ThrMonitor:adSpace.ThrMonitor,
                    SysPlatform:adSpace.SysPlatform,
                    Style:adSpace.Style,
                    IsDispatching:adSpace.IsDispatching,
                    ADShow:adSpace.ADShow,
                    ADRemark:adSpace.ADRemark,
                    AcceptBusinessIDs:adSpace.AcceptBusinessIDs,
                    NotAcceptBusinessIDs:adSpace.NotAcceptBusinessIDs,
                    IsCarousel:adSpace.IsCarousel,
                    SaleType:adSpace.SaleType,
                    Price:adSpace.Price,
                    BeginPlayDays:adSpace.BeginPlayDays
                };

                setAjax({url:'/api/Publish/ModifyADPosition',type:'post',data:data},
                    function(msg){
                        if(msg.Status==0){
                            window.location.href = '/PublishManager/auditPublishAPP.html?MediaID='+adSpace.MediaID;
                        }else{
                            layer.alert('操作失败，请重新编辑');
                        }
                    }
                );
            }
        });
        $('#cancel').click(function(e){
            e.preventDefault();
            window.location.href = '/PublishManager/auditPublishAPP.html?MediaID='+adSpace.MediaID;
        })
    },
    addFun:function(){
        //新增广告位时，初始化页面
        $('input[name="CanClick"]').each(function(){
            if($(this).val()=='true'){
                $(this).attr('checked',true);
            }
        });
        $('input[name="SysPlatform01"]').attr('checked',true);
        $('input[name="SysPlatform02"]').attr('checked',true);
        $('input[name="SaleType"]').each(function(){
            if($(this).val()==11001){
                $(this).attr('checked',true);
            }
        });
        $('input[name="IsDispatching"]').each(function(){
            if($(this).val()=='false'){
                $(this).attr('checked',true);
            }
        });
        adSpace.testReg();
        this.submitFun();
    },
    editFun:function(){
        //根据广告位ID请求数据渲染到页面
        //渲染成功后，回调函数调用编辑方法
        setAjax({
                url:'/api/Periodication/GetAppPublishAdvInfoByAdvID',
                type:'GET',
                data:{ADDetailID:adSpace.ADDetailID}
            },
            function(msg){
                if(msg.Status==0){
                    var data = msg.Result;
                    $('#imgShow').attr('src',data.AdLegendURL);
                    $('#BigImg img').attr('src',data.AdLegendURL);
                    $('input[name="AdPosition"]').val(data.AdPosition);
                    $('input[name="AdForm"]').val(data.AdForm);
                    $('input[name="Style"]').val(data.Style);
                    $('input[name="DisplayLength"]').val(hasCount(data.DisplayLength));
                    function radioVal(inputName,dataName){
                        $('input[name='+inputName+']').each(function(i){
                            if(dataName!=null){
                                if($(this).val()==(""+dataName)){
                                    $(this).attr('checked',true);
                                }else{
                                    $(this).attr('checked',false);
                                }
                            }
                        });
                    }
                    radioVal('CanClick',data.CanClick);
                    radioVal('SaleType',data.SaleType);
                    if( $('input[name="SaleType"]:checked').val()==11001){
                        $('#isCPD').show();
                        $('input[name="IsCarousel"]').parent().show();
                    }else{
                        $('#isCPD').hide();
                        $('input[name="IsCarousel"]').parent().hide();
                    }
                    radioVal('IsDispatching',data.IsDispatching);
                    $('input[name="CarouselCount"]').val(hasCount(data.CarouselCount));
                    $('input[name="PlayPosition"]').val(data.PlayPosition);
                    $('input[name="DailyExposureCount"]').val(hasCount(data.DailyClickCount));
                    $('input[name="CPM"]').attr('checked',data.CPM);
                    $('input[name="CarouselPlay"]').attr('checked',data.CarouselPlay);
                    $('input[name="CPM2"]').attr('checked',data.CPM2);
                    $('input[name="CarouselPlay2"]').attr('checked',data.CarouselPlay2);
                    $('input[name="DailyClickCount"]').val(hasCount(data.DailyClickCount));
                    function hasCount(dataName){
                        if(dataName==null&&dataName==0){
                            return '';
                        }else{
                            return dataName;
                        }
                    }
                    function checkboxReturn(inputName,dataName){
                        if(dataName&&dataName.length>0){
                            $.each(dataName,function(i){
                                if($('input[name='+inputName+']').val()==dataName[i]){
                                    $('input[name='+inputName+']').attr('checked',true);
                                }
                            });
                        }
                    }
                    checkboxReturn('SysPlatform01',JSON.parse(data.SysPlatform));
                    checkboxReturn('SysPlatform02',JSON.parse(data.SysPlatform));
                    checkboxReturn('ThrMonitor01',JSON.parse(data.ThrMonitor));
                    checkboxReturn('ThrMonitor02',JSON.parse(data.ThrMonitor));
                    $('textarea[name="ADShow"]').val(data.ADShow);
                    $('textarea[name="ADRemark"]').val(data.ADRemark);
                    if(data.AcceptBusinessIDs){
                        adSpace.AcceptBusinessIDs = adSpace.AcceptBusinessIDs.concat(JSON.parse(data.AcceptBusinessIDs));
                    }
                    if(data.NotAcceptBusinessIDs){
                        adSpace.NotAcceptBusinessIDs = adSpace.NotAcceptBusinessIDs.concat(JSON.parse(data.NotAcceptBusinessIDs));
                    }
                    $('input[name="IsCarousel"]').attr('checked',data.IsCarousel);
                    $('input[name="Price"]').val(hasCount(data.Price));
                    $('input[name="BeginPlayDays"]').val(hasCount(data.BeginPlayDays));
                    adSpace.testReg();
                    adSpace.submitFun();
                }else{
                    layer.alert('页面错误，跳转至首页',{closeBtn:0},function(){
                        window.location.href='/index.html';
                    });
                }
            });
    },
    upLoadImg:function(){
        /*上传图片*/
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
        if(adSpace.isAdd!=1){
            $('#imgErr').html('请选择图片：大小2MB内，格式jpg/jpeg/png').removeClass('red');
        }
        $('#file_upload').uploadify({
            'auto':true,//是否自动上传
            'multi': false,
            'swf'      : '/js/uploadify.swf?_=' + Math.random(),//flash
            'uploader' : '/AjaxServers/UploadFile.ashx',//上传处理程序
            'buttonImage':'/images/icon20.png',
            'width':27,
            height:25,//按钮高
            'fileTypeDesc':'请选择jpg/jpeg/png格式的图片',
            'fileTypeExts':'*.jpg;*.jpeg;*.gif;*.png',
            'fileCount':1,
            fileSizeLimit:'2MB',
            queueSizeLimit : 1,
            queueID:'imgShow',
            'scriptAccess': 'always',
            'overrideEvents' : [ 'onDialogClose'],
            'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
            'onSelect' : function(file) {
                $('#imgShow').attr('src',file.data);
            },
            'onSelectError':function(file, errorCode, errorMsg){
                if (errorCode == SWFUpload.UPLOAD_ERROR.FILE_CANCELLED
                    || errorCode == SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED) {
                    return false;
                }
                switch(errorCode) {
                    case -100:
                        $('#imgErr').html(adSpace.imgErr+'上传图片数量超过1个').addClass('red');
                        break;
                    case -110:
                        $('#imgErr').html(adSpace.imgErr+'上传图片大小应小于2MB').addClass('red');
                        break;
                    case -130:
                        $('#imgErr').html(adSpace.imgErr+'上传图片类型不正确').addClass('red');
                        break;
                }
            },
            //检测FLASH失败调用
            'onFallback':function(){
                $('li#imgErr').html(adSpace.imgErr+'您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。').addClass('red');
            },
            //上传到服务器，服务器返回相应信息到data里
            'onUploadSuccess':function(file,data,response){
                if (response) {
                    data = JSON.parse(data);
                    $('#imgShow').attr('src', '' +data.Msg);
                    $('#BigImg img').attr('src','' +data.Msg);
                    $('#imgErr').html('上传成功！').removeClass('red');
                    adSpace.AdLegendURL = data.Msg;
                }
            },
            'onUploadError': function (event, queueID, fileObj, errorObj) {
            }
        });
    }
};

//获取url 地址参数方法
function GetQueryString(name) {
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if(r!=null)return unescape(r[2]); return null;
}
//获取时间方法
function getDate(data){
    if(data){
        var Time = data.split(' ')[0];
        //var y = Time[0];
        //var m = Time[1]<10?'0'+Time[1]:Time[1];
        //var d = Time[2]<10?'0'+Time[2]:Time[2];
        //Time = y+'-'+m+'-'+d;
        return Time;
    }else{
        return null;
    }
}
