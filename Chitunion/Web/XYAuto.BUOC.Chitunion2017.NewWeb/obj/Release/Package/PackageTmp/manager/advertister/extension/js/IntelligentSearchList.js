/*
* Written by:     wangc
* function:       智能搜索
* Created Date:   2017-12-19
* Modified Date:
*/
$(function(){
	function TableList(){
		this.init();
	}
	var queryCondition = new Object();
	TableList.prototype = {
		constructor: TableList,
		init:function(){
			var _self = this;
			$('#searchList').off('click').on('click',function(){
				var upData = _self.getParams();
				queryCondition = upData;
				_self.searchList(upData,'#list_table tbody');
			})
			_self.getStatusDictInfo();

			$('#searchList').click();
			_self.operate();
			
		},
		autocomplete:function(){//推广名称模糊查询

		},
		getStatusDictInfo:function(){//状态枚举
			setAjax({
				url:public_url+'/api/DictInfo/GetDictInfoByTypeID',
				type:'get',
				data:{
					typeID:100
				}
			},function(data){
				if(data.Status == 0){
					var str = '';
					data.Result.forEach(function(el){
						str += '<option value='+el.DictId+'>'+el.DictName+'</option>';
					})
					$('#extendStatus').find('option:first').after(str);
				}
			})
		},
		searchList:function(upData,temp){//查询列表
			var _self = this;
			$('#list_table tbody').html('');
			setAjax({
				url:public_url+'/api/SmartSearch/GetSmartSearchList',
				// url:'json/GetSmartSearchList.json',
				type:'get',
				data:upData
			},function(data){
				if(data.Status == 0){
					if(data.Result.TotalCount > 0){
						$('#no_data').addClass('hide');
						$(temp).html(ejs.render($('#listTemp').html(),{list:data.Result.List}));
						_self.operate();
						var h = $(document).height();
		    			// $('#navLeft').css('height',h+'px');
						//分页
						if(data.Result.TotalCount > 20){
							$("#pageContainer").show();
						}else{
							$("#pageContainer").hide();
						}
						$("#pageContainer").pagination(data.Result.TotalCount,{
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            current_page:upData.PageIndex,
                            callback: function (curPage, jg) {
                                var upData_pageIndex = queryCondition;
                                upData_pageIndex.PageIndex = curPage;
                                setAjax({
                                    url:public_url+'/api/SmartSearch/GetSmartSearchList',
									// url:'json/GetSmartSearchList.json',
                                    type: 'get',
                                    selector:temp,
                                    data: upData_pageIndex
                                }, function (data_pageIndex) {
                                    // 渲染数据
                                    $(temp).html(ejs.render($('#listTemp').html(),{list:data_pageIndex.Result.List}));
                                    _self.operate();
                                    $(document).scrollTop(0);
                                })
                            }
                        });
					}else{
						$('#no_data').removeClass('hide');
						$('#pageContainer').hide();
					}
				}
			})
		},
		operate:function(){//操作
		    
			/*添加智能匹配推广*/
			$('#add_extend').off('click').on('click',function(){
				window.location = '/static/advertister/sort_list.html';
			})
			/*查看*/
			$('.viewDetailInfo').off('click').on('click',function(){
				var RecID = $(this).attr('RecID');
				window.open('viewOrderDetail.html?RecID='+RecID);
			})

			$('#list_table tr').off('click').on('click',function(){
				var $this = $(this);
				var RecID = $this.find('.viewDetailInfo').attr('RecID');
				window.open('viewOrderDetail.html?RecID='+RecID);
			})
		},
		getParams:function(){//获取参数
			return {
				PageIndex : 1,
				PageSize : 20,
				Name : $.trim($('#extendName').val()),
				Status : $('#extendStatus option:checked').attr('value')-0
			}
		}
	}
	new TableList();
})