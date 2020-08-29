/**
 * Created by fengb on 2017/9/29.
 */

$(function () {

    var UserId = GetQueryString('UserId')!=null&&GetQueryString('UserId')!='undefined'?GetQueryString('UserId'):null;

    var xyConfig = {
        url : {
            'SelectZhyAdvertiserInfo' : public_url + '/api/ZhyInfo/SelectZhyAdvertiserInfo',
            'BingdingOptIdAndAccIdToAdvId' : public_url + '/api/ZhyInfo/BingdingOptIdAndAccIdToAdvId',
            'DeleteAdvsiterByAdvId' : public_url + '/api/ZhyInfo/DeleteAdvsiterByAdvId',
            'BingdingOptIdToAdvsId' : public_url + '/api/ZhyInfo/BingdingOptIdToAdvsId'
        }
    }

    var DetailOfAdvertiser = {
        init : function () {
            var url = xyConfig.url.SelectZhyAdvertiserInfo;
            //var url = 'json/SelectZhyAdvertiserInfo.json';
            setAjax({
                url : url,
                type : 'get',
                data : {
                    UserId : UserId
                }
            },function (data) {
                var Result = data.Result;
                if(data.Status == 0){
                   $('.install_box').html(ejs.render($('#Advertiser-detail').html(),Result));
                    DetailOfAdvertiser.operation();
                    if(Result.AccountId == 0){
                        $('.customer_second li').each(function(){
                            var that = $(this);
                            that.find('strong').text('一');
                        })
                    }
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
        },
        operation : function(){//操作

            if(CTLogin.RoleIDs == 'SYS005RL00019'){//超管
                $('.operation_btn').show();
            }else if(CTLogin.RoleIDs == 'SYS005RL00021'){//广告运营
                $('.operation_btn').hide();
            }

            

            // 点击账号绑定
            $('.operation_btn .Accountbinding').off('click').on('click', function () {
                var _this = $(this);
                var Username = $(this).attr('Username');
                var Corporatename = $(this).attr('Corporatename');
                if(Corporatename == 'null'){
                    Corporatename = '一';
                }
                var phone = $(this).attr('phone');
                var UserId = $(this).attr('UserId');
                $.openPopupLayer({
                    name: "Accountbinding",
                    url: "Accountbinding.html",
                    error: function (dd) {
                        alert(dd.status);
                    },
                    success: function (data) {
                        $('#Username').html(Username);
                        $('#Corporatename').html(Corporatename);
                        $('#phone1').html(phone);

                        $('#closebt').off('click').on('click', function () {
                            $.closePopupLayer('Accountbinding')
                        })
                        // 点击确认
                        $('#submitMessage').off('click').on('click', function () {
                            var bindingQQ = $('#bindingQQ option:selected').val()
                            if (bindingQQ == '-2') {
                                layer.msg('请选择绑定子客/广告主');
                                return false;
                            }
                            setAjax({
                                url : xyConfig.url.BingdingOptIdAndAccIdToAdvId,
                                type: 'post',
                                data: {
                                    AdvertiserId: UserId,
                                    AccountId: bindingQQ.split('-')[0]
                                }
                            }, function (data) {
                                if (data.Status == 0) {
                                    $.closePopupLayer('Accountbinding');
                                    DetailOfAdvertiser.init();
                                } else {
                                    layer.msg(data.Message);
                                }
                            })
                        })
                    }
                });
            });
            
            // 修改绑定
            $('.operation_btn .Modifyoperation').off('click').on('click', function () {
                var Username = $(this).attr('Username');
                var Corporatename = $(this).attr('Corporatename');
                if(Corporatename == 'null'){
                    Corporatename = '一';
                }
                var phone = $(this).attr('phone');
                var UserId = $(this).attr('UserId');
                $.openPopupLayer({
                    name: "Modifyoperation",
                    url: "Modifyoperation.html",
                    error: function (dd) {
                        alert(dd.status);
                    },
                    success: function (data) {
                        $('#Username').html(Username);
                        $('#Corporatename').html(Corporatename);
                        $('#phone1').html(phone);

                        $('#closebt').off('click').on('click', function () {
                            $.closePopupLayer('Modifyoperation')
                        })
                        // 点击确认
                        $('#submitMessage').off('click').on('click', function () {
                            var ao = $('#bindingQQ option:selected').val()
                            if (ao == '-2') {
                                layer.msg('请选择绑定子客/广告主');
                                return false;
                            }
                            setAjax({
                                url : xyConfig.url.BingdingOptIdAndAccIdToAdvId,
                                type: 'post',
                                data: {
                                    AdvertiserId: UserId,
                                    AccountId: ao.split('-')[0]
                                }
                            }, function (data) {
                                if (data.Status == 0) {
                                    $.closePopupLayer('Modifyoperation');
                                    DetailOfAdvertiser.init();
                                } else {
                                    layer.msg(data.Message);
                                }
                            })
                        })
                    }
                });
            });

            // 点击解除绑定
            $('.operation_btn .Unbound').off('click').on('click', function () {
                var UserId = $(this).attr('AccountId');
                layer.confirm('您确定要将此账户解绑吗？', {
                        time: 0,//不自动关闭
                        btn: ['确认', '取消'] //按钮
                        , yes: function (index) {
                            setAjax({
                                url : xyConfig.url.DeleteAdvsiterByAdvId,
                                type: 'post',
                                data: {
                                    AdvertiserId: UserId
                                }
                            }, function (data) {
                                if (data.Status == 0) {
                                    layer.close(index);
                                    DetailOfAdvertiser.init();
                                } else {
                                    layer.msg(data.Message)
                                }
                            })
                        }
                    }
                );
            });

        }        
    };

    DetailOfAdvertiser.init();

})


//获取url 地址参数方法
function GetQueryString(name) {
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if(r!=null)return r[2]; return null;
}


