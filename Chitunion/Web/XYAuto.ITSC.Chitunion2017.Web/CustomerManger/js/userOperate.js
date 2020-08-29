/*
* 查询列表
*
* */
$(function () {

    var SelectData = {

        init : function () {
            //省市下拉调用公共的方法
            BindProvince('ddlProvince');

            //账号来源切换
            $('select[name=source]').on('change',function () {
                var that = $(this);
                var val = that.find('option:checked').val();

                if(val != '赤兔平台'){
                    $('select[name=accountSource]').hide();
                }else{
                    $('select[name=accountSource]').show();
                }
            })

            //查询按钮
            $('#select_data').on('click',function () {
                var userName = $.trim($('input[name=userName]').val());//用户名
                var mobile = $.trim($('input[name=mobile]').val());//手机号
                var trueName = $.trim($('input[name=trueName]').val());//公司名称
                var source = $.trim($('select[name=source]').find('option:checked').val());//库存或者赤兔
                var accountSource = $.trim($('select[name=accountSource]').find('option:checked').val());//账号来源

                var status = $.trim($('select[name=status]').find('option:checked').val());//账号状态
                var type = $.trim($('select[name=type]').find('option:checked').val());//账号类型
                var beginDate = $('#beginDate').val();//开始日期
                var endDate = $('#endDate').val();//结束日期

                var ProvinceID = $('select[name=ddlProvince]').find('option:checked').val();
                var CityID = $('select[name=ddlCity]').find('option:checked').val();

                if(source == '赤兔平台'){
                    source = accountSource;
                }else{
                    source = source;
                }

                var obj = {
                    UserName : userName,
                    Mobile : mobile,
                    TrueName : trueName,
                    UserSource : source,
                    Status : status,
                    UserType : type,
                    ProvinceID : ProvinceID,
                    CityID : CityID,
                    StarDate : beginDate,
                    EndDate : endDate,
                    PageIndex : 1,
                    PageSize : 20
                }
                console.log(obj);
                SelectData.sendRequest(obj,obj.PageIndex);
            })
        },
        sendRequest : function (obj,PageIndex) {

            var url = 'http://www.chitunion.com/api/UserInfo/SelectAdveristerList';
            setAjax({
                url : url,
                type : 'get',
                data : obj
            },function (data) {
                var Result = data.Result;
                var TotalCount = data.Result.TotalCount;
                if(data.Status == 0){
                    if(Result.TotalCount > 0){
                        $('#list_data').show();
                        $('#list_data').html(ejs.render($("#ListTemplate").html(),Result));
                        $('#No_Data').hide();

                        //分页
                        $("#pageContainer").pagination(TotalCount, {
                            items_per_page: 20, //每页显示多少条记录（默认为10条）
                            callback: function (currPage) {
                                obj.PageIndex = currPage;
                                setAjax({
                                    url : 'http://www.chitunion.com/api/UserInfo/SelectAdveristerList',
                                    type : 'get',
                                    data : obj
                                },function (data) {
                                    var Result = data.Result;
                                    $('#list_data').html(ejs.render($("#ListTemplate").html(),Result));
                                })
                            }
                        })
                        //操作
                        SelectData.operation();
                    }else{
                        $('#No_Data').show();
                        $('#list_data').hide();
                        $('#pageContainer').hide();
                    }
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
        },
        createPageController : function (TotalCount,pageNum) {//分页显示
            $("#pageContainer").pagination(TotalCount, {
                current_page: (pageNum ? pageNum : 1),
                items_per_page: 20, //每页显示多少条记录（默认为10条）
                callback: function (currPage) {
                    //SelectData.sendRequest(obj,currPage);
                }
            })
        },
        operation : function () {//操作

            var isChecked = $('#selectAll').prop('checked');

            //全选
            $('#selectAll').on('change',function () {
                var that = $(this);
                if(that.prop('checked') == true){
                    $('#list_data input[name=PitchUp]').prop('checked',true);
                }else{
                    $('#list_data input[name=PitchUp]').prop('checked',false);
                }
            })

            //单选
            $('.onecheck').off('click').on('click',function () {
                if($('#list_data input[name=PitchUp]').length == $('.onecheck:checked').length){
                    $('#selectAll').prop('checked',true);
                }else{
                    $('#selectAll').prop('checked',false);
                }
            })


            //启用
            $('.start_using').on('click',function () {
                var UserIDList = [];
                if($('.box input[type=checkbox]:checked').length < 1){
                    layer.msg('请选择要启用的用户',{'time':1000});
                }else{
                    $('.box input[name=PitchUp]:checked').each(function(){
                        var that = $(this);
                        UserIDList.push(that.attr('UserID'));
                    })
                    setAjax({
                        url : 'http://www.chitunion.com/api/UserInfo/UpdateUserStatusInfo',
                        type : 'post',
                        data : {
                            'UserIDList' : UserIDList,
                            'Status' : 0
                        }
                    },function (data) {
                        if(data.Status == 0){
                            layer.msg(data.Message,{'time':1000});
                            $('#select_data').click();
                            $('#selectAll').prop('checked',false);
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })
                }
            })

            //禁用
            $('.forbidden').on('click',function () {
                var UserIDList = [];
                if($('.box input[type=checkbox]:checked').length < 1){
                    layer.msg('请选择要禁用的用户',{'time':1000});
                }else{
                    $('.box input[name=PitchUp]:checked').each(function(){
                        var that = $(this);
                        UserIDList.push(that.attr('UserID'));
                    })
                    setAjax({
                        url : 'http://www.chitunion.com/api/UserInfo/UpdateUserStatusInfo',
                        type : 'post',
                        data : {
                            'UserIDList' : UserIDList,
                            'Status' : 1
                        }
                    },function (data) {
                        if(data.Status == 0){
                            layer.msg(data.Message,{'time':1000});
                            $('#select_data').click();
                            $('#selectAll').prop('checked',false);
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })
                }
            })

            //重置密码
            $('.reset_password').on('click',function () {
                var UserIDList = [];
                if($('.box input[type=checkbox]:checked').length < 1){
                    layer.msg('请选择要重置密码的用户',{'time':1000});
                }else{
                    $('.box input[name=PitchUp]:checked').each(function(){
                        var that = $(this);
                        UserIDList.push(that.attr('UserID'));
                    })
                    setAjax({
                        url : 'http://www.chitunion.com/api/UserInfo/UpdateUserPwdInfo',
                        type : 'post',
                        data : {
                            'UserIDList' : UserIDList
                        }
                    },function (data) {
                        if(data.Status == 0){
                            layer.msg(data.Message,{'time':1000});
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })
                }
            })

            //新建广告主
            $('.media_add_btn').on('click',function(){
                window.open('http://j.chitunion.com/userInfo/user/getInfoAndDetail/29001/0');
            })

            //编辑
            $('.edit_each').on('click',function(){
                var category = 29001;
                var UserID = $(this).attr('UserID');
                window.location = 'http://j.chitunion.com/userInfo/user/getInfoAndDetail/'+category+'/'+UserID ;
            })

        }
    }

    SelectData.init();//初始化渲染
    $('#select_data').click();
})