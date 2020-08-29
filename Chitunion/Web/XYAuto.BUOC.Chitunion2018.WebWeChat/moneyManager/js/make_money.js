/*
* Written by:     fengbo
* function:       选择分类
* Created Date:   2018-01-22
* Modified Date:   2018-03-08
*/
$(function () {

    var SS = window.sessionStorage;

    function MakeMoney(){
        this.LastPageSize = 10;
        this.mescroll = null;
        this.init();
        this.sort();
        this.scrollView();
        this.tit(); //已下线
    }


    MakeMoney.prototype.scrollView = function(){
        window.onload = function(){
            //解决加号在安卓情况下滚动不显示的问题
            $('#mescroll').on('scroll', function(e) {
                var that = $(this);
                var _class = $('#navContent').attr('class').split(' ');
                var len = _class.length;
                var imgh = $('.task_banner').height(); //178.625
                var scrh = that.scrollTop();
                if(scrh >= imgh){
                    $('#more').css({
                        'position':'fixed'
                    })
                }else{
                    $('#more').css({
                        'position':'absolute'
                    })
                }
                SS.setItem('scrollAction',1);
                //如果PageSize 返回的时候更改过的话 那么在下次下拉刷新的话要重置10
                if(SS.getItem('offsetTop') == null && SS.getItem('PageSize') == null){
                    SS.setItem('PageSize',10);
                }
            })
            var mySwiper = new Swiper ('.swiper-container',{
                loop: false,
                autoplay: 6000,//可选选项，自动滑动
                pagination: '.swiper-pagination'// 如果需要分页器
            })
        }
    }


    //每人每天首次加载弹窗
    MakeMoney.prototype.tit = function(){

        var Result = 0;
        $.ajax({
            url: public_url + '/api/Task/GetUserDayOrderCount',
            type: 'get',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (data) {
                if(data.Status == 0){
                    Result = data.Result;
                    //Result = 5;
                    var each_UserID = null;
                    $.ajax({
                        url: public_url + '/api/Task/GetUnionAndUserId',
                        type: 'get',
                        xhrFields: {
                            withCredentials: true
                        },
                        crossDomain: true,
                        async: false,
                        success: function (data) {
                            if(data.Status == 0){
                                each_UserID = data.Result.UserID;
                            }
                        }
                    })

                    //当前日期
                    var tody = (new Date()).Format("yyyy-MM-dd");
                    var UserID = LS.get('UserID');

                    console.log(UserID , each_UserID);

                    if(Result < 5){
                        if(tody != LS.get('date') || each_UserID != LS.get('UserID')){
                            LS.set('date',tody);
                            LS.set('UserID',each_UserID);
                            $('.shareBox').show();
                            $('.shareImg').show();
                            $('#shareImg').off('click').on('click',function(){
                                $('.shareBox').hide();
                                $('.shareImg').hide();
                            })
                        }
                    }else{
                        $('.shareBox').hide();
                        $('.shareImg').hide();
                    }
                }
            }
        })
    }


    //初始化mescroll对象
    MakeMoney.prototype.init = function(){

        var _this = this;

        //mescroll对象
        _this.mescroll = new MeScroll("mescroll", {
            up: {
                callback: function (page, mescroll) {
                    var TaskId = $('#dataList div:last').find('dl').attr('TaskId');//任务ID
                    if(SS.getItem('PageSize')){
                        _this.LastPageSize = SS.getItem('PageSize');
                    }
                    if(page.num == 1){
                        TaskId = 0;
                    }else{
                        TaskId = TaskId?TaskId:0;
                    }
                    _this.getListData(page,TaskId,_this.LastPageSize);
                },
                isBounce: false, //此处禁止ios回弹,解析(务必认真阅读,特别是最后一点): http://www.mescroll.com/qa.html#q10
                clearEmptyId: "dataList", //相当于同时设置了clearId和empty.warpId; 简化写法;
                warpId: "upscrollWarp", //让上拉进度装到upscrollWarp里面
                noMoreSize: 4, //如果列表已无数据,可设置列表的总数量要大于半页才显示无更多数据;避免列表数据过少(比如只有一条数据),显示无更多数据会不好看; 默认5
                empty: {
                    icon: "../images/no_data.png", //图标,默认null
                    tip: "这里没有数据哦" //提示
                },
                toTop: { //配置回到顶部按钮
                    src: "../images/mescroll-totop.png", //默认滚动到1000px显示,可配置offset修改
                },
                offset: 10
            }
        })


        var navWarp = $("#navWarp");
        if(_this.mescroll.os.ios){
            //ios的悬停效果,通过给navWarp添加nav-sticky样式来实现
            navWarp.addClass("nav-sticky");
        }else{
            //android和pc端悬停效果,通过监听mescroll的scroll事件,控制navContent是否为fixed定位来实现
            var navHeight = navWarp.get(0).offsetHeight ; //固定高度占位,避免悬浮时列表抖动
            navWarp.css({'height':navHeight + 'px'});
            var navContent = $("#navContent");
            _this.mescroll.optUp.onScroll = function(mescroll, y, isUp){
                //console.log("up --> onScroll 列表当前滚动的距离 y = " + y + ", 是否向上滑动 isUp = " + isUp);
                if(y >= navWarp.get(0).offsetTop){
                    navContent.addClass("nav-fixed");
                }else{
                    navContent.removeClass("nav-fixed");
                }
            }
        }
    }


    //初始化分类
    MakeMoney.prototype.sort = function(){
        var _this = this;

        $.ajax({
            url: public_url + '/api/Task/GetSceneInfoByUserId',
            //url: 'json/GetSceneInfoByUserId.json',
            type: 'get',
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            data: {
                r: Math.random()
            },
            async: false,
            success: function (data) {
                if (data.Status == 0) {
                    var Result = data.Result.CategoryList;
                    var IsSkip = data.Result.IsSkip;
                    var len = Result.length;
                    var str = '';//导航内容

                    var sortArr = [];
                    var isSelectedSort = [];
                    var notSelectedSort = [];

                    isSelectedSort.push({
                        IsSelected: 1,
                        SceneID: 0,
                        SceneName: '推荐'
                    })

                    /*
                        1. 判断是否一个分类都木有选中  如果有选中的  则表明已经选择过 则把选择过的过滤为SortArr渲染
                        2. 判断是否一个分类都木有选中  如果都木有选中  则判断是否为跳过  如果不是跳过则回去分类如果是跳过则将45个分类直接渲染 SortArr
                    */

                    //是否选择分类
                    if (Result) {

                        //过滤
                        for (var i = 0; i <= len - 1 ; i++) {
                            var obj = {
                                IsSelected: Result[i].IsSelected,
                                SceneID: Result[i].SceneID,
                                SceneName: Result[i].SceneName
                            }
                            if(Result[i].IsSelected == 1){
                                isSelectedSort.push(obj);
                            }else{
                                notSelectedSort.push(obj);
                            }
                        }

                        //木有选择过的分类
                        if(notSelectedSort.length == len){
                            // if(IsSkip){
                            //     sortArr = notSelectedSort;
                            // }else{
                            //     window.location = public_pre + '/moneyManager/sort.html';
                            // }
                            sortArr = notSelectedSort;
                        }else{
                            sortArr = isSelectedSort;
                        }


                        for (var i = 0; i <= sortArr.length - 1 ; i++) {
                            if (i == 0) {
                                str += '<li SceneID=' + sortArr[i].SceneID + ' class="selected" i=' + i + '><a href="javascript:;">' + sortArr[i].SceneName + '</a></li>';
                            } else {
                                str += '<li SceneID=' + sortArr[i].SceneID + ' i=' + i + '><a href="javascript:;">' + sortArr[i].SceneName + '</a></li>';
                            }
                        }
                        str += '<li i=-1  ><a href="javascript:;">推荐</a></li>';
                        $('#navContent').append(str);

                        _this.changeSort();

                    }
                } else {
                    layer.open({
                        content: data.Message,
                        skin: 'msg',
                        time: 2 //2秒后自动关闭
                    });
                }
            }
        })
    }


    //切换分类
    MakeMoney.prototype.changeSort = function(){

        var _this = this;
        var navWarp = $("#navWarp");
        //设置nav的宽度
        var len = $("#navContent li").length;
        var w = $("#navContent li").width();

        //固定具体的分类
        if(SS.getItem('SceneID') != null){
            var cur_id = SS.getItem('SceneID');
            $('#navContent li').each(function(){
                var that = $(this);
                var idx = that.index();
                var SceneID = that.attr('SceneID');
                var scrollWidth =  w * (idx - 1 );//左右滚动的距离
                if(cur_id == SceneID){
                    $('#navContent li').eq(idx).addClass('selected').siblings().removeClass('selected');
                    $('#navContent').animate({'scrollLeft': scrollWidth });
                }
            })
        }

        //点击切换分类
        $("#navContent li").off('click').on('click',function(){
            var that = $(this);
            var idx = that.index();
            var SceneID = that.attr('SceneID');
            var moveWidth = - w * idx;//左右移动的距离
            SS.setItem('SceneID',SceneID);
            SS.removeItem("offsetTop");
            that.addClass('selected').siblings().removeClass('selected');
            var minHight = _this.mescroll.getClientHeight() - navWarp.get(0).offsetHeight;
            $("#upscrollWarp").css({'minHeight': minHight });
            //重置列表数据
            _this.mescroll.resetUpScroll();
        })

        //跳转分类
        $('#more').off('click').on('click', function () {
            //绑定百度统计trackEvent事件逻辑
            var name = $(this).attr('baidu_track_name');
            var action = $(this).attr('baidu_track_action');
            var label = $(this).attr('baidu_track_label');
            var val = $(this).attr('baidu_track_val');
            if (val == null) {
                window._hmt && window._hmt.push(['_trackEvent', name, action, label == null ? '-' : label]);
            }
            else {
                window._hmt && window._hmt.push(['_trackEvent', name, action, label == null ? '-' : label, val]);
            }
            if(GetRequest().channel){
                window.location = public_pre + '/moneyManager/sort.html?channel=' + GetRequest().channel;
            }else{
                window.location = public_pre + '/moneyManager/sort.html';
            }
        })
    }

    //调取接口
    MakeMoney.prototype.getListData = function(page,TaskId,LastPageSize){

        var _this = this;
        //联网加载数据
        _this.getListDataFromNet(page.num, page.size, TaskId, LastPageSize, function(curPageData){
            _this.mescroll.endSuccess(curPageData.length);
            //设置列表数据
            _this.setListData(curPageData, page.num);
        }, function(){
            //联网失败的回调,隐藏下拉刷新和上拉加载的状态;
            _this.mescroll.endErr();
        });
    }

    //渲染数据
    MakeMoney.prototype.setListData = function (List, firstPage) {

        var listDom = $("#dataList");
        var str = '';
        //手工置顶文章
        if (firstPage == 1) {
           // str += '<div><dl MaterialUrl=http://wxnews.xingyuanwanli.com/ct_m/20180417/193675.html>' +
           //                 '<dt>' +
           //                     '<h2>5万块的7座SUV 比路虎霸气 不买宏光就选它！</h2>' +
           //                     '<p>' +
           //                         '<img src="/images/select.png"><span>1毛/有效阅读</span>' +
           //                     '</p>' +
           //                 '</dt>' +
           //                 '<dd>' +
           //                     '<img src=http://imgcdn.chitunion.com/group4/M00/DB/DD/Qw0DAFrwC5GANEusAAAyni5wN7E769.JPG class="head_img">' +
           //                 '</dd>' +
           //         '</dl></div>';
           // str += '<div><dl MaterialUrl=http://wxnews.xingyuanwanli.com/ct_m/20180328/176101.html>' +
           //                 '<dt>' +
           //                     '<h2>10万块豪华SUV 配置超百万 2.0T还买H6是不是傻？</h2>' +
           //                     '<p>' +
           //                         '<img src="/images/select.png"><span>1毛/有效阅读</span>' +
           //                     '</p>' +
           //                 '</dt>' +
           //                 '<dd>' +
           //                     '<img src=http://imgcdn.chitunion.com/group4/M00/DC/06/Qw0DAFrwDVqAQKqfAAAwG3yXuRg845.JPG class="head_img">' +
           //                 '</dd>' +
           //         '</dl></div>';
           str += '<div><dl MaterialUrl=http://wxnews.xingyuanwanli.com/ct_m/20180421/195551.html>' +
                           '<dt>' +
                               '<h2>想买H6，那不妨来看看这几款性价比超高的SUV</h2>' +
                               '<p>' +
                                   '<img src="/images/select.png"><span>1毛/有效阅读</span>' +
                               '</p>' +
                           '</dt>' +
                           '<dd>' +
                               '<img src=http://imgcdn.chitunion.com/group4/M00/DC/08/Qw0DAFrwDWaAMCGsAABKVsT5r6w501.JPG class="head_img">' +
                           '</dd>' +
                   '</dl></div>';
        }
        for (var i = 0; i <= List.length - 1; i++) {
            //未转发
            if (List[i].IsForward == 0) {
                //精选的样式 <img src="/images/select.png">
                str += '<div><dl MaterialUrl=' + List[i].MaterialUrl + ' TaskId=' + List[i].TaskId + ' >' +
                            '<dt>' +
                                '<h2>' + List[i].TaskName + '</h2>' +
                                '<p>' +
                                    '<span>' + removePoint0(formatMoney(List[i].CPCPrice * 10,1,''),1) + '毛/有效阅读</span>' +
                                '</p>' +
                            '</dt>' +
                            '<dd>' +
                                '<img src=' + List[i].ImgUrl + ' class="head_img">' +
                            '</dd>' +
                    '</dl></div>';
            } else {//已转发
                str += '<div><dl MaterialUrl=' + List[i].MaterialUrl + ' TaskId=' + List[i].TaskId + '>' +
                                '<dt>' +
                                    '<h2>' + List[i].TaskName + '</h2>' +
                                    '<p>' +
                                        '<span>' + removePoint0(formatMoney(List[i].CPCPrice * 10,1,''),1) + '毛/有效阅读</span>' +
                                    '</p>' +
                                '</dt>' +
                                '<dd>' +
                                    '<img src=' + List[i].ImgUrl + ' alt="" class="head_img">' +
                                    '<img src="../images/icon1.png" alt="" class="icon_img">' +
                                '</dd>' +
                        '</dl></div>';
            }
        }

        listDom.append(str);
    }


    //传参数
    MakeMoney.prototype.getListDataFromNet = function(pageNum,pageSize,TaskId,LastPageSize,successCallback,errorCallback){

        var _this = this;
        //延时一秒,模拟联网
        setTimeout(function () {
            $.ajax({
                //url: 'json/GetTaskListByUserId.json',
                url: public_url + '/api/Task/GetTaskListByUserId',
                type: 'get',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                data: {
                    PageSize: LastPageSize,
                    SceneID: $('#navContent li.selected').attr('SceneID') * 1,
                    PageIndex: TaskId,
                    r: Math.random()
                },
                success: function (data) {
                    if (data.Status == 0) {
                        var TaskInfo = data.Result.TaskInfo;
                        var TotalCount = data.Result.TotalCount;
                        //回调
                        successCallback(TaskInfo, TotalCount);
                        //进入详情页面
                        $('.data-list div').off('click').on('click', function () {
                            var that = $(this);
                            var MaterialUrl = that.find('dl').attr('MaterialUrl');
                            that.addClass('selected').siblings().removeClass('selected');
                            MaterialUrl = MaterialUrl + '?utm_source=chitu';
                            if(GetRequest().channel){
                                MaterialUrl += '&channel=' + GetRequest().channel;
                            }
                            //详情页面加随机数
                            MaterialUrl += '&r=' + Math.random();
                            window.location = MaterialUrl;
                            var len = that.index() + 1;//当前文章的长度
                            var sTop = $('#mescroll').scrollTop();
                            if(len <= 10){
                                len = 10;
                            }
                            SS.setItem("offsetTop",sTop);
                            SS.setItem('PageSize',len);
                            setTimeout(function () {
                                that.removeClass('selected');
                            }, 50)
                        })

                        //判断是否为直接刷新 还是浏览完一个文章以后 返回然后从新刷新
                        if(SS.getItem('offsetTop') != null && SS.getItem('PageSize') != null){
                            var cur_top = SS.getItem('offsetTop');
                            $('#mescroll').scrollTop(cur_top);
                        }else{
                            if(SS.getItem('scrollAction') == null){
                                $('#mescroll').scrollTop(0);
                            }
                        }
                        setTimeout(function(){
                            SS.removeItem("offsetTop");
                            SS.removeItem('PageSize');
                        },2000)
                    } else {
                        layer.open({
                            content: data.Message,
                            skin: 'msg',
                            time: 2 //2秒后自动关闭
                        });
                    }
                },
                error: errorCallback
            })
        },500)
    }


    //微信初始化
    MakeMoney.prototype.WxReday = function(){
        wx.ready(function () {
            //隐藏所有传播类和复制链接菜单
            wx.hideMenuItems({
                menuList: ['menuItem:copyUrl','menuItem:share:appMessage','menuItem:share:timeline','menuItem:share:qq','menuItem:share:weiboApp','menuItem:share:QZone','menuItem:share:facebook']  // 要隐藏的菜单项，只能隐藏“传播类”和“保护类”按钮，所有menu项见附录3
            });
        })
    }

    new MakeMoney();


    /* 1.获取  昨天  最近七天  最近三十天*/
    function getthedate(dd,dadd){
        //可以加上错误处理
        var a = new Date(dd)
        a = a.valueOf()
        a = a + dadd * 24 * 60 * 60 * 1000
        a = new Date(a);
        var m = a.getMonth() + 1;
        if(m.toString().length == 1){
            m='0'+m;
        }
        var d = a.getDate();
        if(d.toString().length == 1){
            d='0'+d;
        }
        return a.getFullYear() + "-" + m + "-" + d;
    }

})
