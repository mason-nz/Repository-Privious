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
$(function () {
    function PresentManagement() {
        this.init()
    }
    PresentManagement.prototype={
        constructor:PresentManagement,
        // 获取条件
        ObtainConditions:function (i) {
            // 认证状态
            var OrderStatus=$('.tab_menu .selected').attr('value');
            // 用户名
            var UserName=$('#u_name').val();
            // 手机号
            var iphone=$('#iphone').val();
            // 账号来源
            var AccountSource = $('#AccountSource option:selected').val();
            // 账号状态
            var AccountStatus = $('#AccountStatus option:selected').val();
            // 开始时间
            var CreateTime = $('#CreateTime').val();
            // 结束时间
            var endTime = $('#endTime').val();
            var obj={
                UserName:UserName,
                Mobile:iphone,
                Source:AccountSource,
                Status:AccountStatus,
                ApproveStatus:OrderStatus,
                ListType:"g_user",
                BeginTime:CreateTime,
                EndTime:endTime,
                PageSize:20
            }
            if(i!=undefined){
                obj.PageIndex=i
            }
            return obj
        },
        init:function () {
            var _this=this;
            // 点击查询
            $('#Inquiry').off('click').on('click',function () {
                _this.query(_this.ObtainConditions(1),_this)
            })
            // 切换
            $('.tab_menu li').off('click').on('click',function () {
                $(this).attr('class','selected').siblings().attr('class','');
                $('#allbox').prop('checked',false)
                $('#Inquiry').click();
            })
            if(GetRequest().finish==1){
                $('.tab_menu li').eq(1).click();
            }else {
                $('#Inquiry').click();
            }
            //下载数据
            $('#ExportData').off('click').on('click', function () {
                var parameter = _this.ObtainConditions(1);
                var that = $(this);
                var _url = public_url + '/api/ExcelOperation/ExportResources.aspx?Export=advertisementuser&UserName=' + parameter.UserName + '&ListType=' + parameter.ListType + '&AttentionStatus=' + parameter.AttentionStatus + '&Mobile=' + parameter.Mobile + '&RegisterFrom=' + parameter.RegisterFrom + '&RegisterType=' + parameter.RegisterType + '&Status=' + parameter.Status + '&ApproveStatus=' + parameter.ApproveStatus + '&BeginTime=' + parameter.BeginTime + '&EndTime=' + parameter.EndTime;
                that.attr('href', _url);
            })
        },
        query:function (parameter,_this) {
            setAjax({
                url:public_url+'/api/MediaUser/GetMediaUserList',
                // url:'json/advertisementUserList.json',
                type:'get',
                data:parameter
            },function (data) {
                if(data.Status==0){
                    $('.channel').html(ejs.render($('#channel').html(), data));
                    $('#Cumulative').html(ejs.render($('.Cumulative').html()));
                    _this.operation()
                    if (data.Result.TotalCount != 0) {
                        var counts = data.Result.TotalCount;
                        //分页
                        $("#pageContainer").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    parameter.PageIndex=currPage
                                    // ajax请求
                                    setAjax({
                                        url: public_url+'/api/MediaUser/GetMediaUserList',
                                        type: "GET",
                                        data: parameter
                                    }, function (data) {
                                        $('.channel').html(ejs.render($('#channel').html(), data));
                                        $('#Cumulative').html(ejs.render($('.Cumulative').html()));
                                        _this.operation()
                                    })
                                }
                            });

                    } else {
                        $('#pageContainer').html('<img src="/ImagesNew/no_data.png" style="display: block;margin: 70px auto;">')
                    }

                }else {
                    layer.msg(data.Message)
                }
            })
           
        },
        operation:function () {
            /*全选 全不选按钮*/
            $('#tableList').on('change', '#allbox', function () {
                if ($(this).prop('checked')) {
                    $('.onebox').prop('checked', true);
                } else {
                    $('.onebox').prop('checked', false);
                }
            });

            /*单选按钮*/
            $('#tableList').on('change', '.onebox', function () {
                if ($('.onebox').length == $("input[name='checkbox']:checked").length) {
                    $('#allbox').prop('checked', true);
                } else {
                    $('#allbox').prop('checked', false);
                }
            });
            // 启用
            $('#Enable').off('click').on('click',function () {
                var UserID=[];
                $('.onebox').each(function () {
                    if($(this).prop('checked')){
                        UserID.push($(this).attr('userid'))
                    }
                });
                if(UserID.length==0){
                    layer.msg("请选择广告主！")
                    return false;
                }
                setAjax({
                    url:public_url+'/api/MediaUser/UserEnableOrDisable',
                    type:'post',
                    data:{
                        UserIDList:UserID,
                        Status:0,
                        ListType:'g_user'
                    }
                },function (data) {
                    if(data.Status==0){
                        layer.msg('成功',{time:1000},function () {
                            location.reload()
                        })
                    }else {
                        layer.msg(data.Message)
                    }
                })
            })
            // 禁用
            $('#Disable').off('click').on('click',function () {
                var UserID=[];
                $('.onebox').each(function () {
                    if($(this).prop('checked')){
                        UserID.push($(this).attr('userid'))
                    }
                });
                if(UserID.length==0){
                    layer.msg("请选择广告主！")
                    return false;
                }
                setAjax({
                    url:public_url+'/api/MediaUser/UserEnableOrDisable',
                    type:'post',
                    data:{
                        UserIDList:UserID,
                        Status:1,
                        ListType:'g_user'
                    }
                },function (data) {
                    if(data.Status==0){
                        layer.msg('成功',{time:1000},function () {
                            location.reload()
                        })
                    }else {
                        layer.msg(data.Message)
                    }
                })
            })
            // 重置密码
            $('#reset_password').off('click').on('click',function () {
                var UserID=[];
                $('.onebox').each(function () {
                    if($(this).prop('checked')){
                        UserID.push($(this).attr('userid'))
                    }
                });
                if(UserID.length==0){
                    layer.msg("请选择广告主！")
                    return false;
                }
                setAjax({
                    url:public_url+'/api/MediaUser/UserResetPassword',
                    type:'post',
                    data:{
                        UserIDList:UserID,
                        ListType:'g_user'
                    }
                },function (data) {
                    if(data.Status==0){
                        layer.msg('密码已重置为123.abc，请通知用户用新密码登录',{time:1000},function () {
                            location.reload()
                        })
                    }else {
                        layer.msg(data.Message)
                    }
                })
            })
            $('.fail').on('mouseover',function () {
                $(this).next().show()
            }).on('mouseout',function () {
                $(this).next().hide()
            })
            var Prompt_script_h=$('.Prompt_script').parent('td').outerHeight();
            $('.Prompt_script').css('top',Prompt_script_h-1+'px')
        }
    }
    new PresentManagement()
})