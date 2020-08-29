/**
 * Created by fengb on 2017/8/28.
 */

$(function () {

    var GroupId = GetQueryString('GroupId')!=null&&GetQueryString('GroupId')!='undefined'?GetQueryString('GroupId'):null;
    var isSee = GetQueryString('isSee')!=null&&GetQueryString('isSee')!='undefined'?GetQueryString('isSee'):null;

    var config = {};

    var head_editor = UE.getEditor('head-editor');
    var foot_editor = UE.getEditor('foot-editor');

    var DeleteList = [];//删除的作废的腰部文章ID

    //清洗主管只能查看  不能操作
    if(CTLogin.RoleIDs == 'SYS001RL00012'){
        $('.keep').hide();
        $('.operation_btn').hide();
    }else if(CTLogin.RoleIDs == 'SYS001RL00013'){
        if(isSee == 1){
            $('.keep').hide();
            $('.operation_btn').hide();
        }
    }

    /*window.onbeforeunload = function (e) {
        e = e || window.event;
          // 兼容IE8和Firefox 4之前的版本
        if (e) {
           e.returnValue = '关闭提示';
        }
        // Chrome, Safari, Firefox 4+, Opera 12+ , IE 9+
        return '关闭提示';
    };*/


    var MaterialClean = {
        init : function () {

            UE.getEditor('head-editor');
            UE.getEditor('foot-editor'); 

            var url = 'http://www.chitunion.com/api/Materiel/GetCleanInfo';
            if(isSee == null){//清洗
                setAjax({
                    url : url,
                    type : 'get',
                    data : {
                        GroupId : GroupId,
                        isSee : false
                    }
                },function (data) {
                    var Result = data.Result;
                    if(data.Status == 0){
                        var Head = Result.Head;
                        var Body = Result.Body;

                        config.Head = Head;
                        config.Body = Body;

                        //默认绑定属性  
                        $('.head').attr('ArticleId',Head.ArticleId);  
                        $('.head').attr('Title',Head.Title);
                        $('.head').attr('Abstract',Head.Abstract);
                        $('.head').attr('Content',Head.Content);

                        //默认渲染
                        $('.head .head-tit').val(Head.Title);//标题
                        $('.head .Category').text(Head.Category);//分类

                        var ResourceName = '';//来源啊
                        if(Head.Resource == 1){
                            ResourceName = '微信'
                        }else if(Head.Resource == 2){
                            ResourceName = '汽车之家'
                        }else if(Head.Resource == 3){
                            ResourceName = '今日头条'
                        }else if(Head.Resource == 4){
                            ResourceName = '网易汽车 '
                        }else if(Head.Resource == 5){
                            ResourceName = '行圆新闻后台'
                        }else if(Head.Resource == 6){
                            ResourceName = '搜狐'
                        }

                        $('.head .source').text(ResourceName);//来源
                        $('.head .HeadImg').attr("src",Head.HeadImg);//图片
                        $('.head .Abstract').val(Head.Abstract);//文章摘要
                        head_editor.ready(function() {
                            head_editor.setContent(Head.Content);
                        });   


                        //修改头部的基本信息
                        $('.head .head-tit').on('change',function(){
                            var that = $(this);
                            $('.head').attr('Title',that.val());
                        })
                        $('.head .Abstract').on('change',function(){
                            var that = $(this);
                            $('.head').attr('Abstract',that.val());
                        })
                        head_editor.addListener("contentChange",function(){
                            $('.head').attr('Content',head_editor.getContent());
                        });


                        $('.foot .foot-tip').html(ejs.render($('#material-clean').html(),{data:Body}));

                        //默认加载第一个腰部的文章信息
                        setTimeout(function() {
                            $('.foot .foot-tip li').eq(0).click();
                        },100);

                        MaterialClean.changeEachTxt();//切换腰部
                        MaterialClean.changeYaoInfor();//修改腰部的当前的信息
                        MaterialClean.operation();//
                        MaterialClean.SubmitInfo();
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                })

            }else if(isSee == 1){//查看
                setAjax({
                    url : url,
                    type : 'get',
                    data : {
                        GroupId : GroupId,
                        isSee : true
                    }
                },function (data) {
                    var Result = data.Result;
                    if(data.Status == 0){
                        var Head = Result.Head;
                        var Body = Result.Body;

                        config.Head = Head;
                        config.Body = Body;

                        //默认绑定属性  
                        $('.head').attr('ArticleId',Head.ArticleId);  
                        $('.head').attr('Title',Head.Title);
                        $('.head').attr('Abstract',Head.Abstract);
                        $('.head').attr('Content',Head.Content);

                        //默认渲染
                        $('.head .head-tit').val(Head.Title);//标题
                        $('.head .Category').text(Head.Category);//分类

                        var ResourceName = '';//来源啊
                        if(Head.Resource == 1){
                            ResourceName = '微信'
                        }else if(Head.Resource == 2){
                            ResourceName = '汽车之家'
                        }else if(Head.Resource == 3){
                            ResourceName = '今日头条'
                        }else if(Head.Resource == 4){
                            ResourceName = '网易汽车 '
                        }else if(Head.Resource == 5){
                            ResourceName = '行圆新闻后台'
                        }else if(Head.Resource == 6){
                            ResourceName = '搜狐'
                        }

                        $('.head .source').text(ResourceName);//来源
                        $('.head .HeadImg').attr("src",Head.HeadImg);//图片
                        $('.head .Abstract').val(Head.Abstract);//文章摘要
                        head_editor.ready(function() {
                            head_editor.setContent(Head.Content);
                        });   


                        //修改头部的基本信息
                        $('.head .head-tit').on('change',function(){
                            var that = $(this);
                            $('.head').attr('Title',that.val());
                        })
                        $('.head .Abstract').on('change',function(){
                            var that = $(this);
                            $('.head').attr('Abstract',that.val());
                        })
                        head_editor.addListener("contentChange",function(){
                            $('.head').attr('Content',head_editor.getContent());
                        });


                        $('.foot .foot-tip').html(ejs.render($('#material-clean').html(),{data:Body}));

                        //默认加载第一个腰部的文章信息
                        setTimeout(function() {
                            $('.foot .foot-tip li').eq(0).click();
                        },100);

                        MaterialClean.changeEachTxt();//切换腰部
                        MaterialClean.changeYaoInfor();//修改腰部的当前的信息
                        MaterialClean.operation();//
                        MaterialClean.SubmitInfo();
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                })
            }            

        },
        changeEachTxt : function () {//切换腰部

            $('.foot .foot-tip li').on('click',function (i) {
                var that = $(this);
                var idx = that.index();
                that.addClass('selected').siblings().removeClass('selected');

                var Title = that.attr('Title');
                var Abstract = that.attr('Abstract');
                var HeadImg = that.attr('HeadImg');
                var Content = that.attr('Content');
                var SerialName = that.attr('SerialName');
                var BrandName = that.attr('BrandName');

                $('.foot .head-tit').val(Title);//标题
                $('.foot .BrandName').text(BrandName +'-'+ SerialName);//车型
                $('.foot .Abstract').val(Abstract);//文章摘要
                $('.foot .HeadImg').attr("src",HeadImg);//图片
                foot_editor.ready(function() {
                    foot_editor.setContent(Content);
                });

            })
        },
        changeYaoInfor : function(){
            //修改腰部信息当前的基本信息
            $('.foot .head-tit').on('change',function(){
                var that = $(this);
                $('.foot .foot-tip li.selected').attr('Title',that.val());
            })
            $('.foot .Abstract').on('change',function(){
                var that = $(this);
                $('.foot .foot-tip li.selected').attr('Abstract',that.val());
            })
            foot_editor.addListener("contentChange",function(){
               $('.foot .foot-tip li.selected').attr('Content',foot_editor.getContent());
            });

            MaterialClean.changeEachTxt();//切换腰部
        },
        operation : function(){//操作   编辑和删除

            //删除
            $('.DeleteBtn').off('click').on('click',function(){
                var len = $('.foot-tip').find('li').length;
                var ArticleId = $('.foot .foot-tip li.selected').attr('ArticleId')*1;
            
                if(len <= 1){
                    layer.msg('至少要保留一篇文章',{'time':1000});
                }else{
                    layer.confirm('确认要删除这篇文章吗', {
                        btn: ['确认','取消'] //按钮
                    }, function(){
                        layer.closeAll();
                        $('.foot .foot-tip li.selected').remove();
                        $('.foot .foot-tip li').eq(0).addClass('selected').siblings().removeClass('selected');
                        DeleteList.push(ArticleId);
                        MaterialClean.SubmitInfo();
                        //默认加载第一个腰部的文章信息
                        setTimeout(function() {
                            $('.foot .foot-tip li').eq(0).click();
                        },100);    
                    })
                }
            })


            //编辑出现弹层哦
            $('.EditBtn').off('click').on('click',function(){
                var _width = document.documentElement.clientWidth;
                var _height = document.documentElement.clientHeight;
                
                $('.channel_box').show();
                $('.channel_box .layer').show();
                $('.channel_box .BrandName').text(config.Body[0].BrandName +'-'+ config.Body[0].SerialName);//弹层中的车型
                var layer_height = $('.channel_box .layer').height();
                var _left = (_width-760)/2;
                var _top = (_height-layer_height)/2;
                $('.channel_box').css({'width':_width,'height':_height,'position':'fixed','left':0,'top':0,'background':'rgba(0,0,0,0.7)','zIndex':1000});
                $('.channel_box .layer').css({'position':'absolute','left':_left,'top':_top});

                //关闭
                $('#closebt1').off('click').on('click',function(){
                    $('.channel_box').hide();
                    $('.channel_box .layer').hide();
                })
                //取消
                $('#cancleMessage').off('click').on('click',function(){
                    $('.channel_box').hide();
                    $('.channel_box .layer').hide();
                })

                
                setTimeout(function() {
                    $('#select_adposition').click();
                },50);  


                // 时间控件
                $('#StartDate').off('click').on('click', function () {
                    laydate({
                        fixed: false,
                        elem: '#StartDate',
                        choose: function (date) {
                            if (date > $('#EndDate').val() && $('#EndDate').val()) {
                                layer.alert('起始时间不能大于结束时间！',{'time':1000});
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


                var CopyrightState = 1;//是否原创 默认是
                //查询
                $('#select_adposition').on('click',function () {
                    var ArticleIds = [];
                    var Resource = $('#Resource option:checked').val();
                    var CarSerialId = config.Body[0].SerialId;//车型ID
                    var StartDate = $('#StartDate').val();
                    var EndDate = $('#EndDate').val();
                    
                    $('.CopyrightState').on('change',function(){
                        var that = $(this);
                        if(that.attr('isOrFalse') == '是'){
                            CopyrightState = 1;
                        }else{
                            CopyrightState = 2;
                        }
                    })
                        
                    $('.foot-tip li').each(function () {
                        ArticleIds.push($(this).attr('ArticleId'));
                    });
                    ArticleIds = ArticleIds.join(',');

                    var obj = {
                        ArticleIds : ArticleIds,
                        //Resource : 2,
                        //CarSerialId : 2952,  
                        Resource : Resource,
                        CarSerialId : CarSerialId,
                        StartDate : StartDate,
                        EndDate : EndDate,
                        CopyrightState : CopyrightState,
                        OrderBy : 1001,
                        PageSize : 10,
                        PageIndex : 1
                    }
                    
                    MaterialClean.GetArticleList(obj);


                    // 发布日期的顺序排列
                    $('.position_table .PublishTime').on('click',function(){
                        var img = $(this).find('img').attr('src');
                        $(this).addClass('yellow').siblings().removeClass('yellow');
                        if(img && img=='/images/icon16_c.png'){//默认的图片
                            $(this).find('img').attr('src','/images/icon16_b.png');
                            obj.OrderBy = 1001;
                            MaterialClean.GetArticleList(obj);
                        }else if(img && img=='/images/icon16_a.png'){
                            $(this).find('img').attr('src','/images/icon16_b.png');
                            obj.OrderBy = 1001;
                            MaterialClean.GetArticleList(obj);
                        }else if(img && img=='/images/icon16_b.png'){
                            $(this).find('img').attr('src','/images/icon16_a.png');
                            obj.OrderBy = 1002;
                           MaterialClean.GetArticleList(obj);
                        }
                    });


                    // 阅读数的顺序排列
                    $('.position_table .ReadNum').on('click',function(){
                        var img = $(this).find('img').attr('src');
                        $(this).addClass('yellow');
                        if(img && img=='/images/icon16_c.png'){//默认的图片
                            $(this).find('img').attr('src','/images/icon16_b.png');
                            obj.OrderBy = 2001;
                            MaterialClean.GetArticleList(obj);
                        }else if(img && img=='/images/icon16_a.png'){
                            $(this).find('img').attr('src','/images/icon16_b.png');
                            obj.OrderBy = 2001;
                            MaterialClean.GetArticleList(obj);
                        }else if(img && img=='/images/icon16_b.png'){
                            $(this).find('img').attr('src','/images/icon16_a.png');
                            obj.OrderBy = 2002;
                           MaterialClean.GetArticleList(obj);
                        }
                    });


                    // 点赞数的顺序排列
                    $('.position_table .LikeNum').on('click',function(){
                        var img = $(this).find('img').attr('src');
                        $(this).addClass('yellow');
                        if(img && img=='/images/icon16_c.png'){//默认的图片
                            $(this).find('img').attr('src','/images/icon16_b.png');
                            obj.OrderBy = 3001;
                            MaterialClean.GetArticleList(obj);
                        }else if(img && img=='/images/icon16_a.png'){ 
                            $(this).find('img').attr('src','/images/icon16_b.png');
                            obj.OrderBy = 3001;
                            MaterialClean.GetArticleList(obj);
                        }else if(img && img=='/images/icon16_b.png'){
                            $(this).find('img').attr('src','/images/icon16_a.png');
                            obj.OrderBy = 3002;
                           MaterialClean.GetArticleList(obj);
                        }
                    });


                    // 评论数的顺序排列
                    $('.position_table .ComNum').on('click',function(){
                        var img = $(this).find('img').attr('src');
                        $(this).addClass('yellow');
                        if(img && img=='/images/icon16_c.png'){//默认的图片
                            $(this).find('img').attr('src','/images/icon16_b.png');
                            obj.OrderBy = 4001;
                            MaterialClean.GetArticleList(obj);
                        }else if(img && img=='/images/icon16_a.png'){ 
                            $(this).find('img').attr('src','/images/icon16_b.png');
                            obj.OrderBy = 4001;
                            MaterialClean.GetArticleList(obj);
                        }else if(img && img=='/images/icon16_b.png'){
                            $(this).find('img').attr('src','/images/icon16_a.png');
                            obj.OrderBy = 4002;
                           MaterialClean.GetArticleList(obj);
                        }
                    });


                })
            })   
        },
        GetArticleList : function(obj){//列表的 渲染方法

            var url = 'http://www.chitunion.com/api/Materiel/GetArticleList';
            setAjax({
                url : url,
                type : 'get',
                data : obj
            },function (data) {
                var Result = data.Result;
                if(data.Status == 0){
                    if(Result.TotleCount != 0) {
                        $('.position_table table').show();
                        $('#ChoosePositionContent').html(ejs.render($('#seleted-article').html(),Result));
                        MaterialClean.ChooseArticle();
                        MaterialClean.SubmitInfo();
                        //分页
                        $("#pageContainer").pagination(
                            Result.TotleCount,
                            {
                                items_per_page: 10, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    obj.PageIndex = currPage;
                                    setAjax({
                                        url: 'http://www.chitunion.com/api/Materiel/GetArticleList',
                                        type: 'get',
                                        data: obj
                                    }, function (data) {
                                        var Result = data.Result;
                                        $('#ChoosePositionContent').html(ejs.render($('#seleted-article').html(),Result));
                                        MaterialClean.ChooseArticle();
                                        MaterialClean.SubmitInfo();
                                    })
                                }
                            });
                    }else{
                        $('.position_table table').hide();
                        $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                    }
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })

        },
        ChooseArticle : function(){//选择文章对应进行渲染哦
            //选择
            $('.choose_article').off('click').on('click',function(){
                var that = $(this);
                var ArticleId = that.parents('tr').attr('ArticleId');

                var url = 'http://www.chitunion.com/api/Materiel/GetArticleInfo';
                setAjax({
                    url : url,
                    type : 'get',
                    data : {
                        ArticleId : ArticleId
                    }
                },function(data){
                    if(data.Status == 0){
                        var Result = data.Result;
                        $('.foot .foot-tip li.selected').attr('Title',Result.Title);
                        $('.foot .foot-tip li.selected').attr('Abstract',Result.Abstract);
                        $('.foot .foot-tip li.selected').attr('Content',Result.Content);
                        $('.foot .foot-tip li.selected').attr('HeadImg',Result.HeadImg);
                        $('.foot .foot-tip li.selected').attr('ArticleId',Result.ArticleId);

                        $('.channel_box').hide();
                        $('.channel_box .layer').hide();

                        //默认加载第一个腰部的文章信息
                        setTimeout(function() {
                            $('.foot .foot-tip li.selected').click();
                        },100);
                        MaterialClean.SubmitInfo();
                    }else{
                        layer.msg(data.message,{'time':1000});
                    }
                })

            })

        },
        SubmitInfo : function(){//提交

            //提交
            $('#SubmitInfo').off('click').on('click',function(){
                //头的基本信息
                var head_tit = $('.head .head-tit').val();
                var head_con = UE.getEditor('head-editor').hasContents();// true or false
                var head_Abstract = $('.head .Abstract').val();

                var YaoTitle = [];
                var YaoContent = [];
                var YaoAbstract = [];

                //腰的基本信息
                $('.foot-tip li').each(function(){
                    var that = $(this);
                    YaoTitle.push(that.attr('Title'));
                    YaoContent.push(that.attr('Content'));
                    YaoAbstract.push(that.attr('Abstract'));
                })

                for(var i = YaoTitle.length;i >= 0;i--){
                    if(YaoTitle[i] == ''){
                        YaoTitle.splice(i,1);
                    }
                }   
                for(var i = YaoContent.length;i >= 0;i--){
                    if(YaoContent[i] == ''){
                        YaoContent.splice(i,1);
                    }
                }
                for(var i = YaoAbstract.length;i >= 0;i--){
                    if(YaoAbstract[i] == ''){
                        YaoAbstract.splice(i,1);
                    }
                }

            
                var foot_tit = $('.foot .head-tit').val();
                var foot_con = UE.getEditor('foot-editor').hasContents();// true or false
                var foot_Abstract = $('.foot .Abstract').val();

                
                if(head_tit == '' || head_con == false || head_Abstract == ''){
                    layer.msg('请补充头部信息',{'time':1000});
                }else if(YaoTitle.length < $('.foot-tip li').length || YaoContent.length < $('.foot-tip li').length || YaoAbstract.length < $('.foot-tip li').length){
                    layer.msg('请补充腰部信息',{'time':1000});
                }else{

                    var Head = {
                        ArticleId : $('.head').attr('ArticleId'),
                        Title : $('.head').attr('Title'),
                        Content : $('.head').attr('Content'),
                        Abstract : $('.head').attr('Abstract')
                    }

                    var Body = [];
                    $('.foot-tip li').each(function(){
                        var that = $(this);
                        var obj = {
                            ArticleId : that.attr('ArticleId'),
                            Title : that.attr('Title'),
                            Content : that.attr('Content'),
                            Abstract : that.attr('Abstract')
                        }
                        Body.push(obj);
                    })
                    setAjax({
                        url : 'http://www.chitunion.com/api/Materiel/SubmitInfo',
                        type : 'post',
                        data : {
                            GroupId : GroupId,
                            Head : Head,
                            Body : Body,
                            DeleteList : DeleteList
                        }
                    },function(data){
                        var Result = data.Result;
                        if(data.Status == 0){
                            if(Result > 0){
                                window.location = '/Datamanager/material_clean.html?GroupId=' + Result;
                            }else{
                                layer.msg('暂时没有下一条待清洗的数据了',{'time':1000});
                                window.location = '/DataManager/task_list.html';
                            }
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })
                }
            })


            //作废
            $('#ToAbandoned').off('click').on('click',function(){
                setAjax({
                    url : 'http://www.chitunion.com/api/Materiel/ToAbandoned',
                    type : 'post',
                    data : {
                        GroupId : GroupId,
                    }
                },function(data){
                    var Result = data.Result;
                    if(data.Status == 0){
                        if(Result > 0){
                            window.location = '/Datamanager/material_clean.html?GroupId=' + Result;
                        }else{
                            layer.msg('暂时没有下一条待作废的数据了',{'time':1000});
                            window.location = '/DataManager/task_list.html';
                        }
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                })
            })

        }

    };

    MaterialClean.init();
})


//获取url 地址参数方法
function GetQueryString(name) {
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if(r!=null)return r[2]; return null;
}


