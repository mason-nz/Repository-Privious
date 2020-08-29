/*
* Written by:     fengbo
* function:       订单列表
* Created Date:   2018-01-24
* Modified Date:
*/
$(function(){

	function GetList(){
		
		//导航菜单 每个菜单对应一个mescroll对象
		this.mescrollArr = new Array(2);

		//当前菜单下标
		this.curNavIndex = 0;

		this.init();
	}


	GetList.prototype.init = function(){

		var _this = this;

		var idx = _this.curNavIndex;
		
		//初始化首页
		_this.mescrollArr[idx] = _this.initMescroll(idx);

		//初始化轮播
		var swiper = new Swiper('#swiper2', {
	        onTransitionEnd: function(swiper){
	        	var i = swiper.activeIndex;//轮播切换完毕的事件
	        	_this.changePage(i);
		    }
	    });
	    		
		//菜单点击事件
		$("#nav li").off('click').on('click',function(){
			var that = $(this);
			var i = that.attr('i') * 1;
			swiper.slideTo(i);//以轮播的方式切换列表
		})

	}


	GetList.prototype.initMescroll = function(index){

		var _this = this;

		var mescroll = new MeScroll("mescroll"+index, {
			//上拉加载的配置项
			up: {
				callback:function(page){//上拉回调,此处可简写;callback: _this.getListData(index)
					_this.getListData(index,page);
				},
				isBounce: false, //此处禁止ios回弹,解析(务必认真阅读,特别是最后一点): http://www.mescroll.com/qa.html#q10
				noMoreSize: 4, //如果列表已无数据,可设置列表的总数量要大于半页才显示无更多数据;避免列表数据过少(比如只有一条数据),显示无更多数据会不好看; 默认5
				empty: {
					icon: "../images/no_data.png", //图标,默认null
					tip: "这里没有数据哦", //提示
					//btntext: "去逛逛 >", //按钮,默认""
					//btnClick: function(){//点击按钮的回调,默认null
						//alert("点击了按钮,具体逻辑自行实现");
					//} 
				},
				clearEmptyId: "dataList"+index,
				toTop:{ //配置回到顶部按钮
					src : "../images/mescroll-totop.png", //默认滚动到1000px显示,可配置offset修改
				}
			}
		});

		return mescroll;
	}


	GetList.prototype.getListData = function(index,page){
		var _this = this;

		//记录当前联网的nav下标,防止快速切换时,联网回来curNavIndex已经改变的情况;
		var dataIndex = index; 

		_this.getListDataFromNet(page.num, page.size, function(pageData,total){

			//联网成功的回调,隐藏下拉刷新和上拉加载的状态;
			_this.mescrollArr[dataIndex].endByPage(pageData.length,total);
			
			//设置列表数据
			_this.setListData(pageData,dataIndex);

		}, function(){

			//联网失败的回调,隐藏下拉刷新和上拉加载的状态;
            _this.mescrollArr[dataIndex].endErr();

		});
	}


	//填充数据 pageData 当前页的数据 index 数据属于哪个nav
	GetList.prototype.setListData = function(List,index){

		var _this = this;
		var listDom = $('#dataList' + index);
		var str = '';

		for(var i = 0; i< List.length; i++){

            str  += '<ul TaskID='+ List[i].TaskID +' OrderId='+ List[i].OrderId +'>' + 
						'<li>' +
							'<h2>'+ List[i].OrderName +'</h2>' +
						'</li>' +
						'<li>' + 
							'<span>' + List[i].CreateTime.substr(0,16) + '</span>' + 
							'<span>已赚'+ formatMoney(List[i].TotalAmount,2,'') +'元</span>' +
						'</li>' +  
					'</ul>';
        }

		listDom.append(str);
		_this.operat();//操作
	}


	//请求进入的参数
	GetList.prototype.getListDataFromNet = function(PageIndex,PageSize,successCallback,errorCallback){

		//延时一秒,模拟联网
        setTimeout(function () {
        	$.ajax({
                url: public_url + '/api/Task/GetOrderByStatus',
                type: 'get',
                data: {
                	PageIndex: PageIndex,
                	PageSize: PageSize,
                	status: $('#nav li.selected').attr('status') * 1,
                	r: Math.random()
                },
                success: function(data){
                	if(data.Status == 0){
                		var TaskInfo = data.Result.List;
                		var TotalCount = data.Result.TotalCount;
                		//回调
                		successCallback(TaskInfo,TotalCount);
                		
                	}else{
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


	//左右切换
	GetList.prototype.changePage = function(i){
		var _this = this;

		var curNavIndex = _this.curNavIndex;

		if(curNavIndex != i) {
			
			//更改列表条件
			var curNavDom;//当前菜单项

			$("#nav li").each(function(n,dom){
				if(dom.getAttribute('i') == i) {
					dom.classList.add("selected");
					curNavDom = dom;
				} else{
					dom.classList.remove("selected");
				}
			});

			//菜单项居中动画
			var scrollxContent = document.getElementById("scrollxContent");

			//当前位置
			var star = scrollxContent.scrollLeft;

			//居中
			var end = curNavDom.offsetLeft + curNavDom.clientWidth/2 - document.body.clientWidth/2; 

			_this.mescrollArr[curNavIndex].getStep(star, end, function(step,timer){
				scrollxContent.scrollLeft = step; //从当前位置逐渐移动到中间位置,默认时长300ms
			});

			//隐藏当前回到顶部按钮
			_this.mescrollArr[curNavIndex].hideTopBtn();

			//取出菜单所对应的mescroll对象,如果未初始化则初始化
			if(_this.mescrollArr[i] == null){

				_this.mescrollArr[i]= _this.initMescroll(i);

			}else{

				//检查是否需要显示回到到顶按钮
				var curMescroll = _this.mescrollArr[i];
				var curScrollTop = curMescroll.getScrollTop();

				if(curScrollTop >= curMescroll.optUp.toTop.offset){
					curMescroll.showTopBtn();
				}else{
					curMescroll.hideTopBtn();
				}

			}
			//更新全局的标记
			_this.curNavIndex = i;
		}
	}


	//操作
	GetList.prototype.operat = function(){
		$('.order_list .lists ul').off('click').on('click',function(){
			var that = $(this);
			var OrderId = that.attr('OrderId') * 1;
			var TaskId = that.attr('TaskID') * 1;
			var data = JSON.stringify({"OrderId":OrderId + "","TaskId":TaskId});
			
			window.location = public_pre + '/accountManager/OrderDetails.html?OrderId=' + OrderId;
		})
	}

	new GetList();
})