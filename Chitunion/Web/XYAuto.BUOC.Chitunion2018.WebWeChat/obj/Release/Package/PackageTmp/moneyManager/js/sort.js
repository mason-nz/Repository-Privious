/*
* Written by:     fengbo
* function:       选择分类
* Created Date:   2018-01-20
* Modified Date:
*/

$(function(){


	var lenArr = [];//判断个数

	var sort = new Vue({
		el: '#sort',
		data: {
			sortArr: [],//分类数组
			isHover: false
		},
		created: function(){
	        this.GetSceneInfo();
	        this.WxReday();
	    },
		methods: {
			//获取分类列表 
			GetSceneInfo : function(){
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
					success: function(data){
						if(data.Status == 0){
							var Result = data.Result.CategoryList.splice(1);
							var IsSkip = data.Result.IsSkip;
							var isSelectedSort = [];
							var notSelectedSort = [];

							var arrIds = [];

							//过滤
	                        for (var i = 0; i <= Result.length - 1 ; i++) {
                        		var obj = {
	                                IsSelected: Result[i].IsSelected,
	                                SceneID: Result[i].SceneID * 1,
	                                SceneName: Result[i].SceneName
	                            }
	                            if(Result[i].IsSelected == 1){
	                                isSelectedSort.push(obj);
	                                arrIds.push(Result[i].SceneID);
	                            }else{
	                                notSelectedSort.push(obj);
	                            } 
	                        }


	                        arrIds = arrIds.sort(sortNumber);//从小到大排序
	                        console.log(arrIds);

	                        // if(notSelectedSort.length == Result.length){
	                        //     if(IsSkip){
	                        //         $('#skip_btn').hide();
	                        //     }
	                        // }else{
	                        //     $('#skip_btn').hide();
	                        // }

	                       
							_this.sortArr = Result;
							//渲染以后
							_this.$nextTick(function () {
								$('#sort li').on('click',function(){
								 	var that = $(this);
								 	var _class = that.attr('class');
								 	
								 	if(_class == ''){
								 		that.addClass('selected');
								 	}else{
								 		that.removeClass('selected');
								 	}
								 	
								 	var i = 0;//控制输入小于5个分类
								 	lenArr = [];//每次都要重置

									$('#sort li').each(function(){
										var that = $(this);
										var _class = that.attr('class');
										var SceneID = that.attr('SceneID') * 1;
										if(_class == 'selected'){
											i ++;
											lenArr.push(SceneID);
										}								 		
									})

									lenArr = lenArr.sort(sortNumber);//从小到大排序

									if(i >= 5){
										$('#sort_btn').attr('class','sort_btn');
										//[1,2,3] != [1,2,3]  必须的先进行转换才可以比较
										if(JSON.stringify(arrIds) != JSON.stringify(lenArr)){
											$('#sort_btn').attr('class','sort_btn');
										}else{
											$('#sort_btn').attr('class','grey_btn');
										}
									}else{
										$('#sort_btn').attr('class','grey_btn');
									}
								})	
							})
						}else{
							layer.open({
							    content: data.Message,
							    skin: 'msg',
							    time: 2 //2秒后自动关闭
							});
						}
					}
				})
			},
			//保存分类
			UpdateUserScene : function(){
				var _this = this;
				var len = $('#sort ul li.selected').length;
				var SceneInfo = [];

				if($('#sort_btn').attr('class') == 'grey_btn'){
					return 
				}

				if(len >= 5){
					$('#sort ul li').each(function(i,v){
						var that = $(v);
						var _class = that.attr('class');
						if(_class == 'selected'){
							var obj = {
								SceneID : that.attr('SceneID'),
								SceneName : $.trim(that.text())
							}
							SceneInfo.push(obj);
						}
					})
					var data = {
						SceneInfo: SceneInfo,
						r: Math.random()
					}
					$.ajax({
						url: public_url + '/api/Task/UpdateUserScene',
						type: 'post',
						xhrFields: {
					        withCredentials: true
					    },
					    crossDomain: true,
						data: JSON.stringify(data),
						success: function(data){
							if(data.Status == 0){
								if(GetRequest().channel){
					                window.location = public_pre + '/moneyManager/make_money.html?channel=' + GetRequest().channel;
					            }else{
					                window.location = public_pre + '/moneyManager/make_money.html';
					            }
							}else{
								layer.open({
								    content: data.Message,
								    skin: 'msg',
								    time: 2 //2秒后自动关闭
								});
							}
						}
					})
				}else{
					layer.open({
					    content: '请至少选择5个分类',
					    skin: 'msg',
					    time: 2 //2秒后自动关闭
					});
				}
			},
			//跳过分类
			SkipBtn: function(){
				$.ajax({
					url: public_url + '/api/Task/ToSkip',
					type: 'post',
					data: {
						r: Math.random() 
					},
					success: function(data){
						if(data.Status == 0){
							window.location = public_pre + '/moneyManager/make_money.html';
						}else{
							layer.open({
							    content: data.Message,
							    skin: 'msg',
							    time: 2 //2秒后自动关闭
							});
						}
					}
				})
			},
			//微信初始化
			WxReday: function(){
				wx.ready(function () {
				    //隐藏所有传播类和复制链接菜单
				    wx.hideMenuItems({
				        menuList: ['menuItem:copyUrl','menuItem:share:appMessage','menuItem:share:timeline','menuItem:share:qq','menuItem:share:weiboApp','menuItem:share:QZone','menuItem:share:facebook']  // 要隐藏的菜单项，只能隐藏“传播类”和“保护类”按钮，所有menu项见附录3
				    });
				})
			}
			//点击切换分类
			// ChangeSort : function(val,index){
			// 	var _this = this;
			// 	_this.isHover = index;
			// }
		}
	})

	function sortNumber(a,b){
		return a - b;
	}

})



