/*
* Written by:     yangyakun
* function:       媒体变现
* Created Date:   2017-12-15
*/
$(function () {
    var timeDate = 2500;
    var shareLoadingHtml = '<div class="loading-wrap"><img src="../../images/loading.gif"/><p>正在生成专属链接</p></div>';
    //内容分发  切片广告
    function contentDistribution(obj) {
        this.Rendering(obj)
    }
    contentDistribution.prototype = {
        constructor: contentDistribution,//保存用于创建当前对象的函数。指向contentDistribution
        Rendering: function (obj) {//渲染
            var SceneID;//分类ID
            var _this = this;
            //点击内容分发——分类
            $('.layout_nav').on('click', 'li', function () {
            	var $this = $(this);
                $this.find('a').addClass('actived').parent('li').siblings().find('a').removeClass('actived');
                SceneID = $this.attr('sceneid');
                setAjax({
                    url: public_url + '/api/task/GetDistrbuteList',
                    type: 'get',
                    data: {
                        PageIndex: 1,
                        PageSize: 20,
                        Category: SceneID
                    }
                }, function (data) {//请求成功回调函数
                    $('.list' + obj.val).html(ejs.render($('#list' + obj.val).html(), {data: data.Result}));
                    _this.operation();
                    // 如果数据为0显示图片
//                  console.log(data.Result.TotleCount+'+data.Result.TotleCount');
                    if (data.Result.TotleCount != 0) {
                        if (data.Result.TotleCount > 20) {
                            $('#pageContainer').show()
                        } else {
                            $('#pageContainer').hide()
                        }
                        //分页
                        $("#pageContainer").pagination(
                            data.Result.TotleCount,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    setAjax({
                                        url: public_url + '/api/task/GetDistrbuteList',
                                        type: 'get',
                                        data: {
                                            PageIndex: currPage,
                                            PageSize: 20,
                                            Category: SceneID
                                        }
                                    }, function (data) {
                                        $('.list' + obj.val).html(ejs.render($('#list' + obj.val).html(), {data: data.Result}));
                                        _this.operation();
                                    })
                                }
                            });
                    } else {
                        $('.list' + obj.val).html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                        $('#pageContainer').hide()
                    }
                })
            })
			//列表请求
            setAjax({
                url: obj.url,
                type: 'get',
                data: _this.getParams(obj.val, 1)
            }, function (data) {//请求成功回调
                $('.list' + obj.val).html(ejs.render($('#list' + obj.val).html(), {data: data.Result}));
                _this.operation();
                // 如果数据为0显示图片
                //console.log(data.Result.TotleCount)
                if (data.Result.TotleCount != 0) {
                    if (data.Result.TotleCount > 20) {
                        $('#pageContainer').show()
                    } else {
                        $('#pageContainer').hide()
                    }
                    //分页
                    $("#pageContainer").pagination(
                        data.Result.TotleCount,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                setAjax({
                                    url: obj.url,
                                    type: 'get',
                                    data: _this.getParams(obj.val, currPage)
                                }, function (data) {
                                    $('.list' + obj.val).html(ejs.render($('#list' + obj.val).html(), {data: data.Result}));
                                    _this.operation();
                                })
                            }
                        });
                } else {
                    $('.list' + obj.val).html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                    $('#pageContainer').hide()
                }
            })

        },
        operation: function () {
            var _this = this;
            //内容分发点  击头图或文章标题跳转至文章详情页
            $('.article').off('click').on('click', function () {
                var TaskId = $(this).parents('.task').attr('TaskId');
                // window.location()//跳转至文章详情页
            })
			//点击抢任务
			var clickFlag = 0;
            $('.task_operation').off('click').on('click', function () {
            	clickFlag ++;
                var gourl = public_url + '/static/media/contentDistribution.html',
                	that = $(this),
            		TaskType = that.parents('.task').attr('TaskType'),
                    TaskId = that.parents('.task').attr('TaskId'),
                    OrderId = that.parents('.task').attr('OrderId'),
                    flag = false;
                //判断是否登陆
                if (CTLogin.IsLogin) {
                    if (CTLogin.Category == 29002) {
	                	$.ajax({//请求判断抢任务数量是否上限
                            url: public_url + '/api/task/VerifyEffective',
                            // url : './json/Receive.json',
                            type: "post",
                            dataType: "json",
                            cache: true,
                            xhrFields: {
                                withCredentials: true
                            },
                            crossDomain: true,
                            data: {
                            },
                            async:false,
                            success: function (data) {
                                if(data.Status == 0){
                                    flag = false;
                                }else {
                                    flag = true;
                                }
                            }
                        })
                        if (flag){//分享已上线
                            layer.confirm('当日分享次数已达上限，是否无偿分享？', function(index){
                                _this.toAjaxData(that, _this, TaskType, TaskId, clickFlag);
                                layer.close(index);
                            });
                            return false
                        } else {
                            _this.toAjaxData(that, _this, TaskType, TaskId, clickFlag);
                        }
                    } else {
                        layer.confirm('您不是媒体主,请登录！', {
                            btn: ['登录'] //按钮
                        }, function () {
                            window.location = '/Exit.aspx?type=2&gourl=' + gourl;
                        });
                    }
                } else {
                    layer.confirm('您尚未登录,请登录！', {
                        btn: ['登录'] //按钮
                    }, function () {
                        window.location = '/login.aspx?type=2&gourl=' + gourl;
                    });
                }
            })
        },
        share: function () {//分享 
            $('body').on('click', '.js-social-share', function () {
                var $btn = $(this);
                var type = $btn.data('share');
                var params = $btn.parents('.js-social-share-params').data();
                var url = '';
                switch (type) {
                    case 'weibo':
                        url = weibo(params);
                        window.open(url);
                        break;
                    case 'qzone':
                        url = qzone(params);
                        window.open(url);
                        break;
                    case 'qq':
                        url = qq(params);
                        window.open(url);
                        break;
                    case 'weixin':
                        weixin($btn, params);
                        break;
                }
            });
			 
            //关闭遮罩层
		    $('.close_x').off('click').on('click', function(){
		    	$(this).parent().hide().html('');
		    })
		    
            function weixin($btn, params) {
                // $btn.on('mouseover',function(){
                //     $btn.next().css('display','block')
                // }).on('mouseout',function(){
                //     $btn.next().css('display','none')
                // })
            }

            function weibo(params) {
                var query = {};
                query.url = params.url;
                query.title = params.message;
                query.pic = params.picture;
                return 'http://service.weibo.com/share/share.php?' + buildUrlQuery(query);
            }

            function qzone(params) {
                var query = {};
                query.url = params.url;
                query.title = params.title;
                query.summary = params.summary;
                query.desc = params.message;
                query.pics = params.picture;
                return 'http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?' + buildUrlQuery(query);
            }

            function qq(params) {
                var query = {};

                query.url = params.url;
                query.title = params.title;
                query.summary = params.summary;
                query.desc = params.message;
                query.pics = params.picture;

                return 'http://connect.qq.com/widget/shareqq/index.html?' + buildUrlQuery(query);
            }

            function buildUrlQuery(query) {
                var queryItems = [];
                for (var q in query) {
                    queryItems.push(q + '=' + encodeURIComponent(query[q] || ''))
                }
                return queryItems.join('&');
            }
            
        },
        getParams: function (val, i) {//获取参数
            if (val == 1) {
                return {
                    PageIndex: i,
                    PageSize: 20
                }
            } else if (val == 2) {
                return {
                    PageIndex: i,
                    PageSize: 20
                }
            }

        },
        //生成分享链接
        toGetShareModule: function(tit, purl, ourl){//tit:title, purl:pasteurl, ourl: order url
        	var strModule = '<img class="close_x" src="../../images/close_x.png"/><div class="share_icon_wrap fixedClear">'
    			+'<p class="share_tit">点击下列图标即刻分享</p>'
    			+'<ul class="js-social-share-params fixedClear" data-title="" data-summary="' + tit + '" data-message="' + tit + '" data-url="' + ourl + '" data-picture="http://www.jqsite.com/assets/img/logo.png">'
    				+'<li class="wx_share_wrap js-social-share weixin" data-cmd="weixin" title="分享到微信" data-share="weixin" data-qrcode-url="http://www.jqsite.com/notes/1602289217.html">'
    					+'<img src="../../images/dis/l_wx_30px.png"/>'
    					+'<p>微信</p>'
    				+'</li>'
    				+'<li class="js-social-share" data-cmd="tsina" title="分享到新浪微博" data-share="weibo">'
    					+'<img src="../../images/dis/l_xl_30px.png"/>'
    					+'<p>新浪</p>'
    				+'</li>'
    				+'<li class="js-social-share" data-cmd="qq" title="分享到QQ" data-share="qq">'
    					+'<img src="../../images/dis/l_qq_30px.png"/>'
    					+'<p>QQ</p>'
    				+'</li>'
    				+'<li class="js-social-share" data-cmd="qzone" title="分享到QQ空间" data-share="qzone">'
    					+'<img src="../../images/dis/l_qzone_30px.png"/>'
    					+'<p>QQ空间</p>'
    				+'</li>'
    				+'<li class="js-social-share" title="复制" id="CopysubmitMessage">'
    					+'<img src="../../images/dis/l_copy_30px.png"/>'
    					+'<p>复制</p>'
    				+'</li>'
    			+'</ul>'
    			+'<div class="wx_code_wrap">'
    				+'<span class="wx_code_tri" id=""></span>'
    				+'<img class="wx_code_share" src="' + purl + '"/>'
    			+'</div>'
    		+'</div>';
    		return strModule;
        },
        //加载二维码图片	
		loadImage: function(imgObj, url) {
			imgObj.attr('src', url);
		},
		//粘贴
		toCopyUrl: function(dom, getOrderUrl){
			console.log(getOrderUrl+'+getOrderUrl')
			var clipboard = new Clipboard(dom, {
                text: function () {
                    return getOrderUrl;
                }
            });
            clipboard.on('success', function (e) {
                layer.msg('复制成功!', {'time': 2000});
                e.clearSelection();
            });
            clipboard.on('error', function (e) {
                console.error('Action:', e.action);
                console.error('Trigger:', e.trigger);
            });
		},
        //领取任务Receive回调
        receiveCallbackFn: function(data, that, _this, TaskType, clickFlag){
        	var OrderId = data.Result.OrderId;
        	that.parents('.task').find($('.exclusiveinfo')).css({'display': 'block'}).html(shareLoadingHtml);
            setTimeout(function () {
                if (data.Status == 0 || data.Status == 1004) {//0：未领取，1004：已领取
                    var str = '',
                    	dataOrderUrl = data.Result.OrderUrl,
                    	dataPasterUrl = data.Result.PasterUrl,
                    	dataTitle = data.Result.Title;
                    if(TaskType == '192001'){//内容分发
                    	var pasterUrl = dataPasterUrl;
                    } else if(TaskType == '192002'){//贴片广告
                    	var pasterUrl = public_url +  dataPasterUrl;
                    }
                    str = _this.toGetShareModule(dataTitle, pasterUrl, dataOrderUrl);
                    that.parents('.task').find($('.exclusiveinfo')).css({'display': 'block'}).html(str);
                    that.parents('.task').addClass('task_operation_get');
                    that.removeClass('task_operation_yes').addClass('task_operation_no');
                    _this.share();
                    //复制粘贴
                    _this.toCopyUrl('#CopysubmitMessage', dataOrderUrl);
                    //微信滑过出现二维码
                    _this.loadImage($('.wx_code_share'), pasterUrl);
					var timeOutIndex;
					$('.wx_code_share').error(function () {//image 元素遇到错误
						console.log('图片加载失败...');
						if (timeOutIndex) {
							clearTimeout(timeOutIndex);
						}
						timeOutIndex = setTimeout(function () {
							_this.loadImage($('.wx_code_share'), pasterUrl);
						}, 1000);
					}).load(function () {//加载图片
						console.log("图片加载完成！");
						clearTimeout(timeOutIndex);
					});
                    that.parents('.task').find($('.weixin')).on('mouseover', function () {
                        $(this).parent().siblings('.wx_code_wrap').css({'display': 'block', 'z-index': 10000})
                    }).on('mouseout', function () {
                        $(this).parent().siblings('.wx_code_wrap').css('display', 'none')
                    })
                    if(clickFlag == 1){
                    	if (TaskType == '192002' && data.Status == 0) {//贴片广告，第一次领取
	                    	$.openPopupLayer({
	                            name: "wechatofficialaccount",
	                            url: "wechatofficialaccount.html",
	                            error: function (dd) {
	                                alert(dd.status);
	                            },
	                            success: function (data) {
	                                $('#closebt').off('click').on('click', function () {
	                                    $.closePopupLayer('wechatofficialaccount')
	                                })
	                                // 点击生成订单
	                                $('#submitMessage').off('click').on('click', function () {
	                                    window.location = public_url + '/manager/media/order/patch-detail.html?OrderId=' + OrderId//跳转至切片广告订单详情页
	                                })
	
	                            }
	                        });
	                        
	                    }
                    }
                    
                } else if (data.Status == '1001') {
                	that.parents('.task').find($('.exclusiveinfo')).css({'display': 'none'}).html('');
                    layer.msg('任务信息不存在');
                } else if (data.Status == '1002' || data.Status == '1003') {
                	that.parents('.task').find($('.exclusiveinfo')).css({'display': 'none'}).html('');
                    layer.msg('该任务已结束，不可领取');
                } else if (data.Status == '1005') {
                	that.parents('.task').find($('.exclusiveinfo')).css({'display': 'none'}).html('');
                    layer.msg('同一个用户1小时之内，只能领取60个不同的任务');
                } else {
                	that.parents('.task').find($('.exclusiveinfo')).css({'display': 'none'}).html('');
                }
            },timeDate)
        },
        //Ajax  /api/task/Receive
        toAjaxData: function(that, _this, TaskType, TaskId, clickFlag){
        	$.ajax({
                url: public_url + '/api/task/Receive',//抢任务
                // url : './json/Receive.json',
                type: "post",
                dataType: "json",
                cache: true,
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                data: {
                    TaskType: TaskType,
                    TaskId: TaskId
                },
                success: function (data) {
                    setTimeout(function(){
                    	_this.receiveCallbackFn(data, that, _this, TaskType, clickFlag);
                    }, 40)

                }
            })
        }
    }

    function rob() {
        var _this = this;
        //内容分发点  击头图或文章标题跳转至文章详情页
        $('.article').off('click').on('click', function () {
            var TaskId = $(this).parents('.task').attr('TaskId');
            // window.location()//跳转至文章详情页
        })

        $('.task_operation').off('click').on('click', function () {//抢任务
            //判断是否登陆
            console.log(CTLogin+'+rob')
            var that = $(this);
            if (CTLogin.Category == 29002 && CTLogin.IsLogin == true) {
                var TaskType = that.parents('.task').attr('TaskType'),
                    TaskId = that.parents('.task').attr('TaskId');
                $.ajax({
                    url: public_url + '/api/task/Receive',
                    // url : './json/Receive.json',
                    type: "post",
                    dataType: "json",
                    cache: true,
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    data: {
                        TaskType: TaskType,
                        TaskId: TaskId
                    },
                    success: function (data) {
	                    setTimeout(function(){
	                    	_this.receiveCallbackFn(data, that, _this, TaskType, clickFlag);
	                    }, 50)
                    }
                    
                })
            }

        })
    }

    //点击切换内容分发和切片广告
    $('.nav li a').off('click').on('click', function () {
    	var $this = $(this);
        $this.addClass('active').parent('li').siblings().children('a').removeClass('active');
        var ind = $this.parent('li').index(),
            val = $this.parent('li').val(),
            url = '',
            str = '';

        switch (ind) {
            case 0 ://内容分发
                setAjax({
                    //url: 'http://op.chitunion.com/api/SceneInfo/GetAllScene',
                    url : public_url + '/api/task/GetCategoryList',
                    type: 'get'
                }, function (data) {
                    if(data.Status == 0){
                        var Result = data.Result;
                        str = "<li SceneID='-2'><a href='javascript:void(0)' class='actived'>全部</a></li>";
                        for (var i = 0; i < Result.length; i++) {
                            str += '<li SceneID=' + Result[i].CategoryId + '><a href="javascript:void(0)">' + Result[i].CategoryName + '</a></li>';
                        }
                        $('.scene').html(str);
                        $('.scene li:last').css("border", 0);
                        //当大于第二行的时候显示分类
                        var len = $('.layout_nav .scene').find('li').length;
                        if(len <= 30){
                            $('.layout_nav ul').css('width','1200px');
                            $('#trigger').css('visibility','hidden');
                            $('.scene li').eq(15).css("border", 0);
                            $('.scene li').eq(31).css("border", 0);
                        }else{
                            $('.layout_nav ul').css('width','1118px');
                            $('#trigger').css('visibility','visible');
                            $('.scene li').eq(14).css("border", 0);
                            $('.scene li').eq(29).css("border", 0);
                        }
                        if(len < 15){
                            $('.layout_nav').css('height','48px');
                        }else if(len > 15 && len < 30){
                            $('.layout_nav').css('height','78px');
                        }else{
                            $('.layout_nav').css('height','78px');
                        }

                        //分类
                        $('#trigger').off('click').on('click',function(){
                            var that = $(this);
                            var imgSrc = that.find('img').attr('src');
                            if(imgSrc == '/images/arrow_01.png'){
                                that.parents('.layout_nav').css('height','auto');
                                that.html('收起 <img src="/images/arrow_02.png">');
                            }else{
                                that.parents('.layout_nav').css('height','78px');
                                that.html('展开 <img src="/images/arrow_01.png">');
                            }
                        }).on('mouseover',function(){
                            var that = $(this);
                            that.addClass('link');
                        }).on('mouseout',function(){
                            var that = $(this);
                            that.removeClass('link');
                        })
                    }                    
                })
                url = public_url + '/api/task/GetDistrbuteList'
                $('.content_distribution').show()
                $('.section_advertising').hide()
                break;
            case 1 ://贴片广告
                url = public_url + '/api/task/GetCoverImageList'
                // url = './json/GetCoverImageList.json'
                $('.content_distribution').hide()
                $('.section_advertising').show()
                $('#trigger').html('展开 <img src="/images/arrow_01.png">');
                break;
        }
        var obj = {
            url: url,
            val: val
        }
        new contentDistribution(obj);
    })
    //默认加载内容分发
    $('.nav li a:first').click();
    if (GetUserId().rob == '1') {//1时，切换到贴片广告
        setTimeout(function () {
            $('.nav li').eq(1).find('a').click()
        }, 50)
        $('.nav li').eq(1).find('a').addClass('active').parent('li').siblings().children('a').removeClass('active');
        $('.content_distribution').hide()
        $('.section_advertising').show()
        var _this = $(this)
        setAjax({
            url: public_url + '/api/task/GetCoverImageList',
            type: 'get',
            data: {
                PageIndex: 1,
                PageSize: 20
            }
        }, function (data) {
            console.log(data);
            $('.list2').html(ejs.render($('#list2').html(), {data: data.Result}));
            rob();
            // 如果数据为0显示图片
            console.log(data.Result.TotleCount)
            if (data.Result.TotleCount != 0) {
                if (data.Result.TotleCount > 20) {
                    $('#pageContainer').show()
                } else {
                    $('#pageContainer').hide()
                }
                //分页
                $("#pageContainer").pagination(
                    data.Result.TotleCount,
                    {
                        items_per_page: 20, //每页显示多少条记录（默认为20条）
                        callback: function (currPage, jg) {
                            setAjax({
                                url: public_url + '/api/task/GetCoverImageList',
                                type: 'get',
                                data: {
                                    PageIndex: currPage,
                                    PageSize: 20
                                }
                            }, function (data) {
                                $('.list2').html(ejs.render($('#list2').html(), {data: data.Result}));
                                rob();
                            })
                        }
                    });
            } else {
                $('.list2').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                $('#pageContainer').hide()
            }
        })
    }

    /*获取url的参数*/
    function GetUserId() {
        var url = location.search; //获取url中"?"符后的字串
        var theRequest = {};
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = strs[i].split("=")[1];
            }
        }
        return theRequest;
    }
})