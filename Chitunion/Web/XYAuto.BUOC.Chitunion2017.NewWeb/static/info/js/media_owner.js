
var timeDate = 2500;
//抢任务ajax
var robAjax = {
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
    //列表模板
	listModule: function(TaskId, TaskName, Synopsis, TaskAmount, TakeCount, RuleCount, MaterialUrl, ImgUrl, CPCPrice, numGapClass, type){
		var html = '';
		if(type == 2){
			var synopsisClass = 'synopsis_trigger',
				TaskType = '192002',
				imgHei = 'task_img_wrap2',
				unitTxt = '有效点击';
		} else {
			var synopsisClass = '',
				TaskType = '192001',
				imgHei = '',
				unitTxt = '有效阅读';
		}
		html = '<div class="task fixedClear" id="task_'+TaskId+'" TaskType="'+TaskType+'" TaskId="'+TaskId+'">'
            +'<div class="exclusiveinfo" ></div>'
        	+'<div class="task_tit article">'
                +'<a href="'+MaterialUrl+'" target="_blank" class="no_c">'
                    +'<div class="task_img_wrap '+imgHei+'">'
                        +'<img src="'+ImgUrl+'" alt="">'
                    +'</div>'
                    +'<div class="task_news">'
                        +'<p class="task_tie slh p1">'+TaskName+'</p>'
                        +'<p class="task_tie slh p2 '+synopsisClass+'">'+Synopsis+'</p>'
                    +'</div>'
                +'</a>'
            +'</div>'
            +'<div class="task_info">'
                +'<div><span>佣金：</span><span class="f_red">'+CPCPrice+'</span>元/'+unitTxt+'</div>'
            	+'<div><span>最高可赚：</span><span class="f_red">'+TaskAmount+'</span>元</div>'
            +'</div>'
			+'<button class="task_operation '+numGapClass+'"></button>'
        +'</div>';
        return html;
	},
	//生成分享链接
	toGetShareModuleFenf: function(tit, purl, ourl){//tit:title, purl:pasteurl, ourl: order url
		var lastIcon = '<li class="js-social-share" title="复制" id="CopysubmitMessage"><img src="../../images/dis/l_copy_30px.png"/><p>复制</p></li>';
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
				+lastIcon
				+'</ul>'
			+'<div class="wx_code_wrap">'
				+'<span class="wx_code_tri" id=""></span>'
				+'<img class="wx_code_share" src="' + purl + '"/>'
			+'</div>'
		+'</div>';
		return strModule;
	},
	toGetShareModuleTiep: function(titMain, titSub, downImg){
		// var lastIcon = '<li class="js-social-share" title="下载" class="download_img_btn"><a class="download_img_a"><img class="download_img" src="'+downImg+'"/><img src="../../images/dis/l_download_30px.png"/><p>下载</p></a></li>';
		// var strModule = '<img class="close_x" src="../../images/close_x.png"/><div class="share_icon_wrap share_icon_tiep_wrap fixedClear">'
		// 	+'<p class="share_tit share_tiep_pic_wrap"><img class="" src="'+downImg+'"/></p>'
		// 	+'<ul class="js-social-share-params fixedClear" data-title="" data-summary="' + tit + '" data-message="' + tit + '" data-url="' + ourl + '" data-picture="http://www.jqsite.com/assets/img/logo.png">'+lastIcon+'</ul>'
		// +'</div>';
		var lastIcon = '<li class="js-social-share" title="下载" class="download_img_btn"><a class="download_img_a"><img class="download_img" src="'+downImg+'"/><img src="../../images/dis/l_download_30px.png"/><p>下载</p></a></li>';
            var strModule = '<img class="close_x" src="../../images/close_x.png"/><div class="share_icon_wrap share_icon_tiep_wrap fixedClear">'
            +'<ul class="js-social-share-params fixedClear">'
                +lastIcon
            +'</ul>'
            +'<div class="task_tit article">'
				+'<p class="task_tie slh p1">'+titMain+'</p>'
            +'</div>'
            +'<div class="task_info">'
                +titSub
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
	//点击
	toRob: function(){
		console.log($('.task_operation').length+'+task_operation.length');
		$('.task_operation').off('click').on('click', function () {
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
	                        robAjax.toAjaxData(that, TaskType, TaskId);
	                        layer.close(index);
	                    });
	                    return false
	                } else {
	                    robAjax.toAjaxData(that, TaskType, TaskId);
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
	//Receive请求
	toAjaxData: function(that, TaskType, TaskId){
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
	            console.log(data)
	            var data1 = data;
	        	if(TaskType == '192001'){//内容分发
	    			var shareTypeTxt = '正在生成专属链接';
	        	} else if(TaskType == '192002'){
	    			var shareTypeTxt = '正在生成专属图片';
				}
				var shareLoadingHtml = '<div class="loading-wrap"><img src="../../images/loading.gif"/><p>'+shareTypeTxt+'</p></div>';
	            that.parents('.task').find($('.exclusiveinfo')).css({'display': 'block'}).html(shareLoadingHtml);
	            setTimeout(function () {
	                if (data.Status == 0 || data.Status == 1004) {//0：未领取	1004：已领取
                        var str = '',
	                    	dataOrderUrl = data.Result.OrderUrl,
	                    	dataPasterUrl = data.Result.PasterUrl,//下载图片
	                    	dataShareTempQrImage = '',//分享
	                    	dataTitle = data.Result.Title;
	                    if(TaskType == '192001'){
	                    	var pasterUrl = dataPasterUrl;//内容分发二维码
	                    	str = robAjax.toGetShareModuleFenf(dataTitle, pasterUrl, dataOrderUrl);
							that.parents('.task').find($('.exclusiveinfo')).css({'display': 'block'}).html(str);
							
							//复制粘贴
							robAjax.toCopyUrl('#CopysubmitMessage', dataOrderUrl);
							//微信滑过出现二维码
							robAjax.loadImage($('.wx_code_share'), pasterUrl);
							var timeOutIndex;
							$('.wx_code_share').error(function () {//image 元素遇到错误
								console.log('图片加载失败...');
								if (timeOutIndex) {
									clearTimeout(timeOutIndex);
								}
								timeOutIndex = setTimeout(function () {
									robAjax.loadImage($('.wx_code_share'), pasterUrl);
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
	                    } else if(TaskType == '192002'){//贴片广告,下载图片
		                    if(data.Result.ShareTempQrImage){
		                    	dataShareTempQrImage = public_url + data.Result.ShareTempQrImage;
	                    		var pasterUrl = dataShareTempQrImage;
								var dataDownloadImg = dataPasterUrl;
								
								var titMain = that.parents('.task').find('.task_news p').eq(0).html(),
									titSub = that.parents('.task').find('.task_info').html();
	                    		str = robAjax.toGetShareModuleTiep(titMain, titSub, dataDownloadImg);
		                    }
                        	that.parents('.task').find($('.exclusiveinfo')).css({'display': 'block'}).html(str);
		                    //接下来进行事件绑定
							var aBtn = that.parents('.task').find($(".download_img_a"));
							if (robAjax.browserIsIe()) {
							    //是ie等,绑定事件
							    aBtn.on("click", function() {
							    	var imgSrc = $(this).find(".download_img").attr("src");
							    	//调用创建iframe的函数
							    	window.open(imgSrc);
							    });
							} else {
							    aBtn.each(function(i,v){
									//支持download,添加属性.
									var imgSrc = $(v).find(".download_img").attr("src");
									$(v).attr("download",imgSrc);
									$(v).attr("href",imgSrc);
							    })
							}
	                    }
                        that.parents('.task').addClass('task_operation_get');
                        that.removeClass('task_operation_yes').addClass('task_operation_no');
                        robAjax.share();
                        
	                } else if (data.Status == '1001') {
	                	that.parents('.task').find($('.exclusiveinfo')).css({'display': 'none'}).html('');
	                    layer.msg('任务信息不存在');
	                } else if (data.Status == '1002' || data.Status == '1003') {
	                	that.parents('.task').find($('.exclusiveinfo')).css({'display': 'none'}).html('');
	                    layer.msg('该任务已结束，不可领取');
	                } else if (data.Status == '1004') {
//	                	that.parents('.task').find($('.exclusiveinfo')).css({'display': 'none'}).html('');
//	                    layer.msg('您已领取过该任务');
	                } else if (data.Status == '1005') {
	                	that.parents('.task').find($('.exclusiveinfo')).css({'display': 'none'}).html('');
	                    layer.msg('同一个用户1小时之内，只能领取60个不同的任务');
	                } else {
	                	that.parents('.task').find($('.exclusiveinfo')).css({'display': 'none'}).html('');
	                }
	            },timeDate)
	
	        }
	    })
	},
	//列表append
	toAppendList: function(dom, timer, html){
		console.log(dom.html()+'++dom.html()')
		if(dom.html() == ''){
			console.log('列表为空')
			if(timer){
				clearTimeout(timer);
			}
			timer = setTimeout(function(){
				dom.html(html);
				robAjax.toRob();
			},50)
		} else {
			console.log('列表加载')
			robAjax.toRob();
			clearTimeout(timer);
		}
	},
        
	//下载图片
	//判断是否为Trident内核浏览器(IE等)函数
	browserIsIe: function () {
	    if (!!window.ActiveXObject || "ActiveXObject" in window){
	        return true;
	    }
	    else{
	        return false;
	    }
	}
};
//内容分发
setAjax({
    url: public_url + '/api/task/GetDistrbuteList',
    type: 'get',
    data: {
        PageIndex: 1,
        PageSize: 8,
        Category: '-2'
    }
}, function (data) {//请求成功回调函数
	if(data.Status == 0){
		if(data.Result.List.length){
			var dataList = data.Result.List,
				dataLen = dataList.length,
				listHtml1 = '';
			for(var i = 0; i < dataLen; i++){
				var TaskName =  dataList[i].TaskName ? dataList[i].TaskName : '--';
				var Synopsis =  dataList[i].Synopsis ? dataList[i].Synopsis : '--';
				var numGapClass = ((dataList[i].RuleCount - dataList[i].TakeCount) == 0) ? 'task_operation_no' : 'task_operation_yes';
				listHtml1 += robAjax.listModule(dataList[i].TaskId, dataList[i].TaskName, dataList[i].Synopsis, dataList[i].TaskAmount, dataList[i].TakeCount, dataList[i].RuleCount, dataList[i].MaterialUrl, dataList[i].ImgUrl, dataList[i].CPCPrice, numGapClass, 1);
			}
			
			setTimeout(function(){
				$('#dis_list1').html(listHtml1);
				robAjax.toRob();
			},200)
		}	
	}
})
//贴片广告
setAjax({
    url: public_url + '/api/task/GetCoverImageList',
    type: 'get',
    data: {
        PageIndex: 1,
        PageSize: 8
    }
}, function (data) {
	if(data.Status == 0){
	    if(data.Result.List.length){
	    	var dataList = data.Result.List,
				dataLen = dataList.length,
				listHtml2 = '';
			for(var i = 0; i < dataLen; i++){
				var TaskName =  dataList[i].TaskName == '无' ? '--' : dataList[i].TaskName ;
				var Synopsis =  dataList[i].Synopsis == '无' ? '--' : dataList[i].Synopsis;
				var numGapClass = ((dataList[i].RuleCount - dataList[i].TakeCount) == 0) ? 'task_operation_no' : 'task_operation_yes';
				listHtml2 += robAjax.listModule(dataList[i].TaskId, dataList[i].TaskName, dataList[i].Synopsis, dataList[i].TaskAmount, dataList[i].TakeCount, dataList[i].RuleCount, dataList[i].MaterialUrl, dataList[i].ImgUrl, dataList[i].CPCPrice, numGapClass, 2);
			}
			
			setTimeout(function(){
				$('#dis_list2').html(listHtml2);
				robAjax.toRob();
			},200)
		}
		
	}
})

