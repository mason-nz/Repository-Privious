$(function () {
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

    function AdvertiserSubList(user) {
        this.Rendering(user)
    }

    AdvertiserSubList.prototype = {
        constructor: AdvertiserSubList,
        Rendering: function (user) {//渲染
            var _this = this;
            var url = 'http://www.chitunion.com/api/ZhyInfo/SelectZhyAdvertiserList';
            $('.but_query').off('click').on('click', function () {
                setAjax({
                    url: url,
                    type: 'get',
                    data: _this.parameter(1)
                }, function (data) {
                    console.log(data);
                    $('#Rendering').html(ejs.render($(user).html(), data));
                    _this.operation();
                    // 如果数据为0显示图片
                    if (data.Result.TotalCount != 0) {
                        $('#pageContainer').show()
                        //分页
                        $("#pageContainer").pagination(
                            data.Result.TotalCount,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    setAjax({
                                        url: url,
                                        type: 'get',
                                        data: _this.parameter(currPage)
                                    }, function (data) {
                                        $('#Rendering').html(ejs.render($(user).html(), data));
                                        _this.operation();
                                    })
                                }
                            });
                    } else {
                        $('#img').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                        $('#pageContainer').hide()
                    }
                })
            })
            $('.but_query').click()
        },
        parameter: function (PageIndex) {//参数
            var phone = $('#phone').val();

            return {
                Mobile: phone,
                PageIndex: PageIndex,
                pageSize: 20
            }
        },
        operation: function () {
            // 点击账号绑定
            $('.Accountbinding').off('click').on('click', function () {
                var _this = $(this);
                var Username = $(this).attr('Username');
                var Corporatename = $(this).attr('Corporatename');
                var phone = $(this).attr('phone');
                var UserId = $(this).attr('UserId');
                $.openPopupLayer({
                    name: "Accountbinding",
                    url: "Accountbinding.html",
                    error: function (dd) {
                        alert(dd.status);
                    },
                    success: function (data) {
                        console.log(UserId);
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
                                layer.msg('请选择绑定QQ');
                                return false;
                            }
                            // var ao = $('#ao option:selected').val()
                            // if (ao == '-2') {
                            //     layer.msg('请选择关联广告运营');
                            //     return false;
                            // }
                            setAjax({
                                url: 'http://www.chitunion.com/api/ZhyInfo/BingdingOptIdAndAccIdToAdvId',
                                type: 'post',
                                data: {
                                    // OperaterId: ao,
                                    AdvertiserId: UserId,
                                    AccountId: bindingQQ
                                }
                            }, function (data) {
                                if (data.Status == 0) {
                                    layer.msg('成功');
                                    $.closePopupLayer('Accountbinding');
                                    $('.but_query').click()
                                } else {
                                    layer.msg(data.Message);
                                }
                            })
                        })
                    }
                });
            })
            // 点击解除绑定
            $('.Unbound').off('click').on('click', function () {
                var UserId = $(this).attr('UserId');
                layer.confirm('您确定要将此账户解绑吗？', {
                        time: 0,//不自动关闭
                        btn: ['确认', '取消'] //按钮
                        , yes: function (index) {
                            setAjax({
                                url: 'http://www.chitunion.com/api/ZhyInfo/DeleteAdvsiterByAdvId',
                                type: 'post',
                                data: {
                                    AdvertiserId: UserId
                                }
                            }, function (data) {
                                if (data.Status == 0) {
                                    layer.close(index);
                                    $('.but_query').click();
                                } else {
                                    layer.msg(data.Message)
                                }
                            })
                        }
                    }
                );
            })
            // 修改运营
            $('.Modifyoperation').off('click').on('click', function () {
                var Username = $(this).attr('Username');
                var Corporatename = $(this).attr('Corporatename');
                var phone = $(this).attr('phone');
                var UserId = $(this).attr('UserId');
                $.openPopupLayer({
                    name: "Modifyoperation",
                    url: "Modifyoperation.html",
                    error: function (dd) {
                        alert(dd.status);
                    },
                    success: function (data) {
                        console.log(UserId);
                        $('#Username').html(Username);
                        $('#Corporatename').html(Corporatename);
                        $('#phone1').html(phone);

                        $('#closebt').off('click').on('click', function () {
                            $.closePopupLayer('Modifyoperation')
                        })
                        // 点击确认
                        $('#submitMessage').off('click').on('click', function () {
                            var ao = $('#ao option:selected').val()
                            if (ao == '-2') {
                                layer.msg('请选择广点通管理员');
                                return false;
                            }
                            setAjax({
                                url: 'http://www.chitunion.com/api/ZhyInfo/BingdingOptIdToAdvsId',
                                type: 'post',
                                data: {
                                    OperaterId: ao,
                                    OperateType:1,
                                    AdvertiserIds: [UserId]
                                }
                            }, function (data) {
                                if (data.Status == 0) {
                                    layer.msg('成功');
                                    $.closePopupLayer('Modifyoperation');
                                    $('.but_query').click()
                                } else {
                                    layer.msg(data.Message);
                                }
                            })
                        })
                    }
                });
            })
        }
    }
    var user = '#Operate';
    if (CTLogin.RoleIDs == 'SYS001RL00001'||CTLogin.RoleIDs == 'SYS001RL00004') {
        user = '#Supertube';
    }
    var advertisersubList = new AdvertiserSubList(user);
})