/*
* Written by:     wangc
* function:       提现列表
* Created Date:   2018-02-5
* Modified Date:
*/
$(function(){
	var vm = new Vue({
		el: "#dataList",
		data: {
			mescroll: null,
			list: []
		},
		mounted: function() {
			//创建MeScroll对象,down可以不用配置,因为内部已默认开启下拉刷新,重置列表数据为第一页
			//解析: 下拉回调默认调用mescroll.resetUpScroll(); 而resetUpScroll会将page.num=1,再执行up.callback,从而实现刷新列表数据为第一页;
			var self = this;
			self.mescroll = new MeScroll("mescroll", { //请至少在vue的mounted生命周期初始化mescroll,以确保您配置的id能够被找到
				up: {
					callback: self.upCallback, //上拉回调
					//以下参数可删除,不配置
					noMoreSize: 4, //如果列表已无数据,可设置列表的总数量要大于半页才显示无更多数据;避免列表数据过少(比如只有一条数据),显示无更多数据会不好看; 默认5
					isBounce: false, //此处禁止ios回弹,解析(务必认真阅读,特别是最后一点): http://www.mescroll.com/qa.html#q10
					page:{size:20}, //可配置每页8条数据,默认10
					empty:{ //配置列表无任何数据的提示
						warpId:"dataList",
						icon: "../images/no_data.png", 
					  	tip : "您还没有提现哦" , 
					  	btntext : "分享赚大钱" , 
					  	btnClick : function() {
					  		window.location = public_url+'/moneyManager/make_money.html';
					  	} 
					},
					//vue的案例请勿配置clearId和clearEmptyId,否则列表的数据模板会被清空
					//vue的案例请勿配置clearId和clearEmptyId,否则列表的数据模板会被清空
					//clearId: "dataList",
					//clearEmptyId: "dataList"
				}
			});
			
		},
		methods: {
			//上拉回调 page = {num:1, size:10}; num:当前页 ,默认从1开始; size:每页数据条数,默认10
			upCallback: function(page) {
				//联网加载数据
				var self = this;
				getListDataFromNet(page.num, page.size, function(curPageData) {
					//curPageData=[]; //打开本行注释,可演示列表无任何数据empty的配置
					
					//如果是第一页需手动制空列表 (代替clearId和clearEmptyId的配置)
					if(page.num == 1) self.list = [];
					
					//更新列表数据
					self.list = self.list.concat(curPageData);
					
					//联网成功的回调,隐藏下拉刷新和上拉加载的状态;
					//mescroll会根据传的参数,自动判断列表如果无任何数据,则提示空;列表无下一页数据,则提示无更多数据;
				
					//方法一(推荐): 后台接口有返回列表的总页数 totalPage
					//self.mescroll.endByPage(curPageData.length, totalPage); //必传参数(当前页的数据个数, 总页数)
					
					//方法二(推荐): 后台接口有返回列表的总数据量 totalSize
					//self.mescroll.endBySize(curPageData.length, totalSize); //必传参数(当前页的数据个数, 总数据量)
					
					//方法三(推荐): 您有其他方式知道是否有下一页 hasNext
					//self.mescroll.endSuccess(curPageData.length, hasNext); //必传参数(当前页的数据个数, 是否有下一页true/false)
					
					//方法四 (不推荐),会存在一个小问题:比如列表共有20条数据,每页加载10条,共2页.如果只根据当前页的数据个数判断,则需翻到第三页才会知道无更多数据,如果传了hasNext,则翻到第二页即可显示无更多数据.
					self.mescroll.endSuccess(curPageData.length);
				
				}, function() {
					//联网失败的回调,隐藏下拉刷新和上拉加载的状态;
					self.mescroll.endErr();
				});
			},
			toDetail:function(RecId){
				window.location = public_pre+'/cashManager/cashDetail.html?WithdrawalsId='+RecId;
			}
		},
	});
	
	/*联网加载列表数据 */
	function getListDataFromNet(pageNum,pageSize,successCallback,errorCallback) {
    	$.ajax({
			url:public_url+'/api/Withdrawals/GetWithdrawalsList',
			type:'get',
			xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
			data:{
				PageIndex:pageNum,
				PageSize:pageSize,
				r:Math.random()
			},
			success:function(response){
				if(response.Status == 0){
					var listData = response.Result.List;
        			successCallback && successCallback(listData);//成功回调
				}else{
					layer.open({
						content:response,
						time:2,
						skin: 'msg'
					})
				}
			},
			error:function(response) {
				errorCallback && errorCallback()//失败回调
			}
		})
	}
})