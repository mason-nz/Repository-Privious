/*
* Written by:     fengbo
* function:       选择分类
* Created Date:   2018-01-22
* Modified Date:   2018-03-05
*/
$(function () {

    function Makemoney() {

        //菜单的数量因为是动态的 所以初始化先默认1 然后获取分类再进行更新
        this.len = 1;

        //导航菜单 每个菜单对应一个mescroll对象
        this.mescrollArr = new Array(this.len);

        //当前菜单下标
        this.curNavIndex = 0;

        this.sort();
    }
                
    //分类导航
    Makemoney.prototype.sort = function () {

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
                    var html = '';//dom盒子

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
                            if(IsSkip){
                                sortArr = notSelectedSort;
                            }else{
                                window.location = public_pre + '/moneyManager/sort.html';
                            }
                        }else{
                            sortArr = isSelectedSort;
                        }

                        //console.log(sortArr);

                        for (var i = 0; i <= sortArr.length - 1 ; i++) {
                            if (i == 0) {
                                str += '<li SceneID=' + sortArr[i].SceneID + ' class="selected" i=' + i + '><a href="javascript:;">' + sortArr[i].SceneName + '</a></li>';
                            } else {
                                str += '<li SceneID=' + sortArr[i].SceneID + ' i=' + i + '><a href="javascript:;">' + sortArr[i].SceneName + '</a></li>';
                            }

                            html += '<div id=mescroll' + [i] + ' class="swiper-slide mescroll">' +
                                        '<div id=dataList' + [i] + ' class="data-list"></div>' +
                                    '</div>';
                        }
                        str += '<li i=-1  ><a href="javascript:;">推荐</a></li>';
                        $('#nav').html(str);
                        $('#swiperWrapper').html(html);

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
                            //跳转页面
                            window.location = public_pre + '/moneyManager/sort.html';
                        })

                        _this.init(len);//调取列表方法

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

    //列表初始化
    Makemoney.prototype.init = function (len) {

        var _this = this;

        //更改菜单的数量
        _this.len = len;

        //当前菜单下标
        var idx = _this.curNavIndex;

        //初始化首页
        _this.mescrollArr[idx] = _this.initMescroll(idx);

        //初始化轮播
        var swiper = new Swiper('#swiper', {
            onTransitionEnd: function (swiper) {
                var i = swiper.activeIndex;//轮播切换完毕的事件
                _this.changePage(i);
            }
        });

        //菜单点击事件
        $("#nav li").off('click').on('click', function () {
            var that = $(this);
            var i = that.attr('i') * 1;
            swiper.slideTo(i);//以轮播的方式切换列表
        })

    }

    //创建MeScroll对象 内部已默认开启下拉刷新,自动执行up.callback,刷新列表数据
    Makemoney.prototype.initMescroll = function (index) {

        var _this = this;

        var mescroll = new MeScroll("mescroll" + index, {
            //上拉加载的配置项
            up: {
                callback: function (page) {//上拉回调,此处可简写;callback: _this.getListData(index)
                    _this.getListData(index, page);
                },
                clearEmptyId: "dataList" + index,
                isBounce: false, //此处禁止ios回弹,解析(务必认真阅读,特别是最后一点): http://www.mescroll.com/qa.html#q10
                noMoreSize: 4, //如果列表已无数据,可设置列表的总数量要大于半页才显示无更多数据;避免列表数据过少(比如只有一条数据),显示无更多数据会不好看; 默认5
                empty: {
                    icon: "../images/no_data.png", //图标,默认null
                    tip: "这里没有数据哦" //提示
                },
                toTop: { //配置回到顶部按钮
                    src: "../images/mescroll-totop.png", //默认滚动到1000px显示,可配置offset修改
                },
                auto: true
                //warpId: "swiperWrapper"//让上拉进度装到upscrollWarp里面
            }
        });

        var imgH = $('.task_banner').height() + 50;
        $('.swiper-container').css({'top':imgH});

        return mescroll;
    }


    //联网加载列表数据 
    Makemoney.prototype.getListData = function (index, page) {
        var _this = this;

        //记录当前联网的nav下标,防止快速切换时,联网回来curNavIndex已经改变的情况;
        var dataIndex = index;

        _this.getListDataFromNet(page.num, page.size, function (pageData, total) {

            //联网成功的回调,隐藏下拉刷新和上拉加载的状态;
            _this.mescrollArr[dataIndex].endByPage(pageData.length, total);

            //设置列表数据
            _this.setListData(pageData, dataIndex);

        }, function () {

            //联网失败的回调,隐藏下拉刷新和上拉加载的状态;
            _this.mescrollArr[dataIndex].endErr();

        });

    }

    //填充数据 pageData 当前页的数据 index 数据属于哪个nav
    Makemoney.prototype.setListData = function (List, index) {

        var listDom = $('#dataList' + index);
        var str = '';

        for (var i = 0; i <= List.length - 1; i++) {
            //未转发
            if (List[i].IsForward == 0) {
                str += '<div><dl MaterialUrl=' + List[i].MaterialUrl + ' TaskId=' + List[i].TaskId + ' >' +
								'<dt>' +
									'<h2>' + List[i].TaskName + '</h2>' +
									'<p>' +
										'<span>' + removePoint0(formatMoney(List[i].CPCPrice * 10,1,''),1) + '毛/有效阅读</span>' +
										'<span>最高可赚&nbsp' + formatMoney(List[i].TotalPrice, 2, '') + '元</span>' +
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
										'<span>已赚&nbsp' + formatMoney(List[i].TotalAmount, 2, '') + '元</span>' +
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

    //请求进入的参数
    Makemoney.prototype.getListDataFromNet = function (PageIndex, PageSize, successCallback, errorCallback) {

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
                    PageIndex: PageIndex,
                    PageSize: PageSize,
                    SceneID: $('#nav li.selected').attr('SceneID') * 1,
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
                            window.location = MaterialUrl;
                            setTimeout(function () {
                                that.removeClass('selected');
                            }, 50)
                        })

                        
                        $('body').bind('touchmove', function(e) { 

                            var imgH = $('.task_banner').height();
                            var scTop = $('.swiper-slide-active').scrollTop();
                            var cTop = imgH - scTop;

                            if(scTop >= imgH){
                                //$('.scrollx').addClass('nav-sticky');
                                $('.scrollx').css({'position':'fixed','top':0});
                                $('#swiper').css({'top':'0'});
                                $('.task_banner').hide();
                            }else{
                                $('.scrollx').css({'position':'fixed','top':cTop});
                                //$('.scrollx').addClass('nav-sticky');
                                $('#swiper').css({'top': cTop + 50});
                                $('.task_banner').show();
                            }
                        });

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
        }, 500)
    }

    //左右切换
    Makemoney.prototype.changePage = function (i) {
        var _this = this;

        var curNavIndex = _this.curNavIndex;

        if (curNavIndex != i) {
            //更改列表条件
            var curNavDom;//当前菜单项

            $("#nav li").each(function (n, dom) {
                if (dom.getAttribute('i') == i) {
                    dom.classList.add("selected");
                    curNavDom = dom;
                } else {
                    dom.classList.remove("selected");
                }
            });

            //菜单项居中动画
            var scrollxContent = document.getElementById("scrollxContent");

            //当前位置
            var star = scrollxContent.scrollLeft;

            //居中
            var end = curNavDom.offsetLeft + curNavDom.clientWidth / 2 - document.body.clientWidth / 2;

            _this.mescrollArr[curNavIndex].getStep(star, end, function (step, timer) {
                scrollxContent.scrollLeft = step; //从当前位置逐渐移动到中间位置,默认时长300ms
            });

            //隐藏当前回到顶部按钮
            _this.mescrollArr[curNavIndex].hideTopBtn();

            //取出菜单所对应的mescroll对象,如果未初始化则初始化
            if (_this.mescrollArr[i] == null) {

                _this.mescrollArr[i] = _this.initMescroll(i);

            } else {

                //检查是否需要显示回到到顶按钮
                var curMescroll = _this.mescrollArr[i];
                var curScrollTop = curMescroll.getScrollTop();

                if (curScrollTop >= curMescroll.optUp.toTop.offset) {

                    curMescroll.showTopBtn();

                } else {

                    curMescroll.hideTopBtn();

                }

            }
            //更新全局的标记
            _this.curNavIndex = i;
        }

    }
    new Makemoney();

})