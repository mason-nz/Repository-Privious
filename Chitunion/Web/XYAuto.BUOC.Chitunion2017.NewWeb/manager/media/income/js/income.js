/*
* Written by:     fengb
* function:       媒体主收入管理
* Created Date:   2017-12-21
* Modified Date:  2018-02-10
* 收益列表--点击任务名称时，在新窗口打开订单详情页
*/

$(function () {

    // var urlArrs = ['/api/order/GetIncomeList','/api/order/GetWithdrawalsList'];
    var urlArrs = ['/api/IncomeManage/GetIncomeDetailModelList','/api/order/GetWithdrawalsList'],
        cashLimit = 50;

    function AccountMange() {
        // 时间控件
        $('#StartDate').off('click').on('click', function () {
            laydate({
                fixed: false,
                elem: '#StartDate',
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
                choose: function (date) {
                    if (date < $('#StartDate').val() && $('#StartDate').val()) {
                        layer.msg('结束时间不能小于起始时间！',{'time':1000});
                        $('#EndDate').val('');
                    }
                }
            });
        });
        this.init();
        $('#TabSelect li').eq(0).click();
    }
   
    AccountMange.prototype = {
        constructor: AccountMange,
        init : function(){
            var _this = this;
            //基本信息
            _this.getincome();
            //支付状态枚举
            _this.getPayStatus();
            //明细切换
            $('#TabSelect li').off('click').on('click',function(){
                var that = $(this);
                var idx = that.index();
                that.addClass('selected').siblings().removeClass('selected');
                _this.queryparameters(idx);
                if(idx == 0){
                    $('#OrderType').parent().show();
                    $('#PayType').parent().hide();
                }else if(idx == 1){
                    $('#OrderType').parent().hide();
                    $('#PayType').parent().show();
                }
                //_this.enumeration(idx);//枚举
            })
        },
        getPayStatus:function(){
            setAjax({
                url:public_url+'/api/DictInfo/GetDictInfoByTypeID',
                type:'get',
                data:{
                    typeID:195
                }
            },function(data){
                if(data.Status == 0){
                    var str = '<option value="-2">全部</option>';
                    data.Result.forEach(function(item){
                        str += '<option value='+item.DictId+'>'+item.DictName+'</option>';
                    })
                    $('#PayType').html(str);
                }else{
                    layer.msg(data.Message,{time:2000});
                }
            })
        },
        queryparameters: function (idx) {// 获取查询参数
            var _this = this;
            // 搜索
            $('#searchBtn').off('click').on('click', function () {
                var StartDate = $('#StartDate').val();
                var EndDate = $('#EndDate').val();
                var OrderType = $('#OrderType option:checked').val();//订单状态
                var PayType = $('#PayType option:checked').val();//支付状态
                var obj = new Object();
                if(idx == 0){
                    obj = {
                        PageIndex : 1,
                        PageSize : 20,
                        IncomeBeginTime : StartDate,
                        IncomeEndTime : EndDate,
                        CategoryID:OrderType
                    }
                }else{
                    obj = {
                        PageIndex : 1,
                        PageSize : 20,
                        StartDate : StartDate,
                        EndDate : EndDate,
                        OrderType : OrderType,
                        PayType : PayType
                    }
                }
                _this.requestdata(obj,idx);
            })
            $('#searchBtn').click();
        },
        requestdata: function (obj,idx) {// 请求数据
            var _this = this;
            $.ajax({
                url: public_url + urlArrs[idx],
                type: 'get',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                data: obj,
                beforeSend: function(){
                    $('.listLoading').html('<img src="/images/loading.gif" style="display: block;margin: 30px auto;">');
                },
                success : function (data) {
                    if(data.Status == 0){
                        $('.listLoading').html('');
                        var Result = data.Result;
                        _this.renderdata(Result, idx);
                        if(idx == 0){
                            if(Result.TotalCount > 0) {
                                $("#pageContainer").show();
                                $('.extension').html('').hide();
                                _this.createPageController(obj, Result,idx);
                            }else{
                                $('.extension').hide();
                                $("#pageContainer").hide();
                                $('.listLoading').html('<img src="/images/no_data.png" style="display: block;margin: 30px auto;">');
                            }
                        }else{
                            if(Result.TotleCount > 0) {
                                $("#pageContainer").show();
                                var TotalMoney;
                                TotalMoney = removePoint0(formatMoney(Result.Extend.TotalMoney,2,''),2);
                                $('.extension').html('累计提现金额：<span class="TotalMoney">'+TotalMoney+'</span>元').show();
                                _this.createPageController(obj, Result,idx);
                            }else{
                                $('.extension').hide();
                                $("#pageContainer").hide();
                                $('.listLoading').html('<img src="/images/no_data.png" style="display: block;margin: 30px auto;">');
                            }
                        }
                    }else{
                        layer.msg(data.Message,{'time':2000});
                    }
                }
            })
        },
        createPageController : function(obj, Result,idx){
            var _this = this;
            var counts = Result.TotleCount || Result.TotalCount;
            $("#pageContainer").pagination(counts, {
                current_page: (obj.PageIndex ? obj.PageIndex : 1),
                items_per_page: 20, 
                callback: function (currPage) {
                    var obj1 = obj;
                    obj1.PageIndex = currPage;
                    _this.requestdata(obj1,idx);
                    $('.main_table tbody').html('');
                }
            });
        },
        renderdata: function (data,idx) {// 渲染数据
            var _this = this;
            $('.main_table').html(ejs.render($('#personal-order' + idx).html(), data));
            $('.pay_status').off('mouseover').on('mouseover',function(){
                if($(this).find('div').length){
                    $(this).addClass('pointer').find('div').show().end().find('.trangle').show();
                }
            }).off('mouseout').on('mouseout',function(){
                if($(this).find('div').length){
                    $(this).removeClass('pointer').find('div').hide().end().find('.trangle').hide();
                }
            })
        },
        enumeration : function(idx){//枚举信息
            var arr = [192001,192002];
            //枚举信息
            $.ajax({
                url: public_url + '/api/DictInfo/GetDictInfoByTypeID',
                type: 'get',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                data: {
                    typeID : arr[idx]
                },
                success : function (data) {
                    if(data.Status == 0){
                        var Result = data.Result;
                        var str = '<option DictId="-2">全部</option>';
                        for(var i = 0;i <= Result.length - 1;i ++){
                            str += "<option DictId="+ Result[i].DictId+">"+Result[i].DictName+"</option>";
                        }
                        $('#OrderType').html(str);
                    }
                }
            })
        },
        getincome :function(){//查询账户基本信息、点击提现验证
            var _this = this,
                userInfo = {
                    Status : 0,//认证状态，默认未认证
                    Type : 1001,//用户类型，默认是企业
                    Mobile:'',//手机号
                    CanWithdrawalsMoney:0,//可提现金额，默认0
                    AccountName : '',//支付宝账号，默认空

                };//用户信息，主要是可提现金额、提现账号、手机号、
            //查询收入详情
            $.ajax({
                //url: './json/GetIncomeInfo.json',
                url : public_url + '/api/order/GetIncomeInfo',
                type: 'get',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                async : false,
                success : function (data) {
                    if(data.Status == 0){
                        var Result = data.Result;
                        userInfo.CanWithdrawalsMoney = data.Result.CanWithdrawalsMoney;//保存可提现金额
                        $('.Personal_massage').html(ejs.render($('#get-income').html(), Result));
                        _this.vertifiUserInfo(userInfo);//验证用户信息是否完善

                    }else{
                        layer.msg(data.Message,{'time':2000});
                    }
                }
            })
        },
        vertifiUserInfo:function(userInfo){//新增：验证用户手机号、支付宝账号、认证信息是否完善，对应在页面显示
            var _this = this;
            setAjax({
                url:public_url+'/api/UserMange/QueryUserBasicInfo',
                type:'get',
                data:{
                    UserID:CTLogin.UserID
                }
            },function(data){
                if(data.Status == 0){
                    if(data.Result.AuthenticationInfo){
                        if(data.Result.AuthenticationInfo.Status){
                            userInfo.Status = data.Result.AuthenticationInfo.Status;//保存认证状态
                        }
                        if(data.Result.AuthenticationInfo.Type){
                            userInfo.Type = data.Result.AuthenticationInfo.Type;//保存用户类型
                        }
                    }
                    userInfo.Mobile = data.Result.BasicInfo.Mobile;//保存手机号
                    if(data.Result.BankAccountInfo && data.Result.BankAccountInfo.AccountName){
                        userInfo.AccountName = data.Result.BankAccountInfo.AccountName;//保存支付宝账号
                    }
                    if( !userInfo.Mobile ){
                        $('.forPhone').attr('src','/images/icon16.png').parent().addClass('pointer');
                        $('.forPhone').parent().off('click').on('click',function(){
                            window.location = '/manager/advertister/account/accountmanage.html';
                        })
                    }
                    if(userInfo.Status != 2){
                        $('.forAuth').attr('src','/images/icon16.png').parent().addClass('pointer');
                        $('.forAuth').parent().off('click').on('click',function(){
                            if(!userInfo.Mobile){
                                layer.msg('请先绑定手机号！',{time:2000},function(){
                                    window.location = '/manager/advertister/account/accountmanage.html';
                                })
                            }else{
                                window.location = '/manager/advertister/account/accountmanage.html?selected=1';
                            }
                            
                        })
                    }
                    if( !userInfo.AccountName ){
                        $('.forPay').attr('src','/images/icon16.png').parent().addClass('pointer');
                        $('.forPay').parent().off('click').on('click',function(){
                            if(!userInfo.Mobile){
                                layer.msg('请先绑定手机号！',{time:2000},function(){
                                    window.location = '/manager/advertister/account/accountmanage.html';
                                })
                            }else{
                                window.location = '/manager/advertister/account/accountmanage.html?selected=2';
                            }
                        })
                    }

                }else{
                    layer.msg(data.Message,{time:2000})
                }
            })
            //提现
            //增加验证是否有手机号--- 验证用户是否认证----验证用户是否有提现账号---验证用户可提现金额是否>=50---打开弹层
            $('#Withdrawals').off('click').on('click',function(){

                if( !userInfo.Mobile ){
                    layer.msg('请先绑定手机号！',{time:2000},function(){
                        window.location = '/manager/advertister/account/accountmanage.html';
                    }) 
                    return
                }
                switch(userInfo.Status){
                    case 0://未认证 
                        layer.msg('请先补充认证信息！',{time:2000},function(){
                            window.location = '/manager/advertister/account/accountmanage.html?selected=1';
                        })  
                        break;
                    case 1://待审核 
                        layer.msg('认证信息正在审核中，审核通过才可提现！',{time:2000});
                        break; 
                    case 2://认证成功--验证提现账号
                        if( !userInfo.AccountName ){
                            layer.msg('请到账号管理中完善提现账号！',{time:2000},function(){
                                window.location = '/manager/advertister/account/accountmanage.html?selected=2';
                            })
                        }else{
                            if( userInfo.CanWithdrawalsMoney < cashLimit ){
                                layer.msg('可提现金额不足'+cashLimit+'元，无法提现！',{'time':2000});
                            }else{
                                _this.judgeTruth(userInfo);//后台验证是否打开弹层
                            }
                        } 
                        break;
                    case 3://认证未通过
                        layer.msg('认证信息审核未通过，请修改认证信息！',{time:2000},function(){
                            window.location = '/manager/advertister/account/accountmanage.html?selected=1';
                        });  
                        break;
                    default:
                        break;  
                }
            })
        },
        judgeTruth:function(userInfo){
            var _this = this;
            setAjax({
                url:public_url+'/api/order/VerifyWithdrawalsClick',
                type:'post',
                data:{}
            },function(data){
                if(data.Status == 0){
                    _this.operatePopup(userInfo);//弹层操作
                }else{
                   if(data.Status == 1011){
                        layer.msg('请先补充认证信息！',{time:2000},function(){
                            window.location = '/manager/advertister/account/accountmanage.html?selected=1';
                        }) 
                   }else if(data.Status == 1012){
                        layer.msg('认证信息审核未通过，请修改认证信息！',{time:2000},function(){
                            window.location = '/manager/advertister/account/accountmanage.html?selected=1';
                        }); 
                   }else if(data.Status == 1013){
                        layer.msg('请到账号管理中完善提现账号！',{time:2000},function(){
                            window.location = '/manager/advertister/account/accountmanage.html?selected=2';
                        }) 
                   }else if(data.Status == 1015){
                        layer.msg('可提现金额不足'+cashLimit+'元，无法提现！',{time:2000});
                   }else if(data.Status == 1016){
                        layer.msg('每天只能提现1次，请明天再试！',{time:2000});
                   }else if(data.Status == 1017){
                        layer.msg('您有正在支付中的提现申请，请在提现完成后再申请！',{time:2000});
                   }else{
                        layer.msg('提交失败，请稍后重试',{time:2000});
                   }
                }
            })
            
        },
        operatePopup:function(userInfo){//提现弹层操作
            var _this = this;
            $.openPopupLayer({
                name: "layerIncome",
                url: "layerIncome.html",
                error: function (dd) {
                    console.log(dd.status);
                },
                success: function () {
                    var WithdrawalsPrice = 0.00;
                    $('#vertifyCode').click();
                    // Type : 1001,//用户类型，默认是企业
                    //显示
                    $('#AccountName').html(userInfo.AccountName);
                    $('#canGetMoney').html(userInfo.CanWithdrawalsMoney);
                    $('#cashAccount').attr('placeholder','可提现金额'+userInfo.CanWithdrawalsMoney+'元');
                    $('#mobile_number').html(userInfo.Mobile.substr(0,3)+'****'+userInfo.Mobile.substr(7,10));

                    //提现金额-验证：用户输入的提现金额需小于等于系统提示的可提现金额；当大于时，提现文本框失去焦点时，可提现金额标红显示
                    //失去焦点时：验证金额是否小于提现金额、类型不同，显示不同。个人显示个税。企业显示发票
                    $('#cashAccount').off('blur').on('blur',function(){
                        WithdrawalsPrice = parseFloat($.trim($(this).val()));//用户输入的提现金额
                        if( !WithdrawalsPrice ){
                            $('.moneyInfo').removeClass('hide').find('.red').html('请输入提现金额');
                            $('.forPerson').addClass('hide');
                            return
                        }else if(WithdrawalsPrice < cashLimit){
                            $('.moneyInfo').removeClass('hide').find('.red').html('提现金额不足'+cashLimit+'元，无法提现！');
                            $('.forPerson').addClass('hide');
                            return
                        }else if(WithdrawalsPrice > userInfo.CanWithdrawalsMoney){
                            $('.moneyInfo').removeClass('hide').find('.red').html('最多可提现金额为'+userInfo.CanWithdrawalsMoney+'元');
                            $('.forPerson').addClass('hide');
                            return
                        }else if( ! /^d*(?:.d{0,2})?$/.test(WithdrawalsPrice) && !/^\d{1,8}$/.test(WithdrawalsPrice)){
                            $('.moneyInfo').removeClass('hide').find('.red').html('请输入正确的金额格式');
                            $('.forPerson').addClass('hide');
                            return
                        }else{
                            $('.moneyInfo').addClass('hide').find('.red').html('');
                            //如果是个人，调接口查询显示个税。如果是企业，显示“发票等”
                            if(userInfo.Type == '1002'){//个人
                                setAjax({
                                    url:public_url+'/api/order/WithdrawalsPriceCalc',
                                    type:'post',
                                    data:{
                                        WithdrawalsPrice:WithdrawalsPrice
                                    }
                                },function(data){
                                    if(data.Status == 0){
                                        if(data.Result.IndividualTaxPeice){
                                            //显示个税和实际付款
                                            $('#TaxPeice').html(data.Result.IndividualTaxPeice);
                                            $('#PracticalPrice').html(data.Result.PracticalPrice);
                                            $('.forPerson').removeClass('hide');
                                        }else{
                                            $('.forPerson').addClass('hide');
                                        }
                                        //当扣税金额大于提现金额时，提示用户“可提现金额不足，无法提现！”
                                        if(data.Result.IndividualTaxPeice > WithdrawalsPrice){
                                            $('.tipInfo').html('可提现金额不足，无法提现！').removeClass('hide');
                                        }else{
                                            $('.tipInfo').addClass('hide');
                                        }
                                    }else{
                                        layer.msg(data.Message,{time:2000})
                                    }
                                })
                            }else{
                                $('.forForm').removeClass('hide');
                            }
                        }

                    })

                    //获取手机号验证码
                    $('#getCode').off('click').on('click',function(){
                        var _self = $(this),
                            html = _self.html();
                        if(html != '获取验证码'){
                            return
                        }
                        //发送手机验证码
                        setAjax({
                            url:public_url+'/AjaxServers/LoginManager.ashx',
                            type:'post',
                            data:{
                                action : 'sendmobilemsg_withdraw',
                                mobile:userInfo.Mobile,
                                checkCode:$.trim($('#codeForVertify').val())                                    
                            }
                        },function(data){
                            switch(data){
                                case 0:
                                    console.log('短信已发送');
                                    $('.tipInfo_final').html('').hide();
                                    var time = 60;
                                    _self.html(time+'秒后可重新获取');
                                    var t = setInterval(function(){
                                        time -- ;
                                        if(time > 0){
                                            _self.html(time+'秒后可重新获取');
                                        }else{
                                            _self.html('获取验证码');
                                            clearInterval(t);
                                        }
                                    },1000)
                                    break;
                                case -9:
                                    $('.tipInfo_final').html('<img src="/images/icon7.png"> 验证码不正确').show();
                                    $('#vertifyCode').click();
                                    break;
                                case -10:
                                    $('.tipInfo_final').html('<img src="/images/icon7.png"> 验证码获取频繁，请稍后再试').show();
                                    $('#vertifyCode').click();
                                    break;
                                case -11:
                                    $('.tipInfo_final').html('<img src="/images/icon7.png"> 发送短信失败').show();
                                    $('#vertifyCode').click();
                                    break;
                            }
                        })
                    })
                    //点击提现，需先验证手机验证码，再调接口提交
                    $('#sure_submmit').off('click').on('click',function(){
                        if($('#sure_submmit').css('backgroundColor') == 'rgb(228, 230, 236)'){
                            return
                        }
                        //验证：提现金额非空、验证码非空、手机验证码非空
                        var PhoneCode = $.trim($('#codeText').val()),//手机验证码
                            checkCode = $.trim($('#codeForVertify').val());//验证码

                        if( !WithdrawalsPrice ){
                            $('.tipInfo_final').html('<img src="/images/icon7.png">请输入提现金额').show();
                            return
                        }else if(WithdrawalsPrice<cashLimit){
                            $('.tipInfo_final').html('<img src="/images/icon7.png">提现金额不足'+cashLimit+'元，无法提现').show();
                            return
                        }
                        if( !checkCode ){
                           $('.tipInfo_final').html('<img src="/images/icon7.png">请输入验证码').show();
                            return 
                        }
                        if( !PhoneCode ){
                           $('.tipInfo_final').html('<img src="/images/icon7.png">请输入手机验证码').show();
                            return 
                        }
                        $.post(
                            public_url+'/AjaxServers/LoginManager.ashx',{
                            action:'checksmscode',
                            mobile:userInfo.Mobile,
                            mobileCheckCode:PhoneCode
                        },function(data){
                              var dataInfo = $.evalJSON(data);
                              if(dataInfo.result == 0){
                                   $('.tipInfo_final').html('').hide(); 
                                   //说明验证码和手机验证码OK，调接口
                                   $.ajax({
                                        url:public_url+'/api/order/Withdrawals',
                                        type:"post",
                                        data:{
                                            WithdrawalsPrice:WithdrawalsPrice
                                        },
                                        async: false,
                                        xhrFields: {
                                            withCredentials: true
                                        },
                                        crossDomain: true,
                                        beforeSend:function(){
                                            $('#sure_submmit').css({'backgroundColor':'rgb(228, 230, 236)','color':'#888'})
                                        },
                                        success:function(data){
                                            if(data.Status == 0){
                                                $('.tipInfo_final').html('平台在1-5个工作日内结算完毕，请注意查收！').show();
                                                setTimeout(function(){
                                                    $('#closebt').click();
                                                    _this.getincome();//重新渲染提现数据
                                                    $('#TabSelect').find('li').eq(1).click();
                                                },2000)
                                            }else if(data.Status == 1012){
                                                layer.msg('请先补充认证信息',{time:2000},function(){
                                                    $.closePopupLayer('layerIncome');
                                                })
                                            }else if(data.Status == 1013){
                                                layer.msg('请先补充提现账号',{time:2000},function(){
                                                    $.closePopupLayer('layerIncome');
                                                })
                                            }else if(data.Status == 1014){
                                                $('.tipInfo_final').html('请确认您的手机号').show();
                                                $('#vertifyCode').click();
                                            }else if(data.Status == 1015){
                                                layer.msg('您的可提现余额不足，无法提现',{time:2000},function(){
                                                    $.closePopupLayer('layerIncome');
                                                })
                                            }else if(data.Status == 1016){
                                                layer.msg('每天只能提现1次，请明天再试！',{time:2000},function(){
                                                    $.closePopupLayer('layerIncome');
                                                })
                                            }else if(data.Status == 3020){
                                                $('.tipInfo_final').html('提现金额不足'+cashLimit+'元，无法提现').show();
                                                $('#vertifyCode').click();
                                            }else{
                                                layer.msg('提交失败，请稍后重试',{time:2000});
                                            }
                                        }
                                    })
                              }else{
                                  $('.tipInfo_final').html('<img src="/images/icon7.png"> '+dataInfo.msg).show();
                              }
                        });
                    })

                    $('#closebt').off('click').on('click',function(){
                        $.closePopupLayer('layerIncome');
                    })
                    $('#vertifyCode').off('click').on('click',function(){
                        $(this).attr('src','/CheckCode.aspx?r='+Math.random());
                    })
                }
            })
        }
    }
    var AccountMange = new AccountMange();
})