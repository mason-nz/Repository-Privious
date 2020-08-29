/*
* Written by:     wangc
* function:       媒体主个人中心
* Created Date:   2017-12-19
* Modified Date:
*/
/*
微博列表，粉丝数显示规则？

*/
$(function(){
	function TableList(){
		this.init();
	}
	TableList.prototype = {
		constructor: TableList,
		init:function(){
			var _self = this;
			//如用户未登录跳转至登录页；如已登录不是媒体主角色，跳转至广告主个人中心页面
			if( CTLogin.IsLogin ){
				if(CTLogin.Category == '29001'){
					window.location = '/manager/advertister/personal/PersonCenter.html';
				}
			}else{
				window.location = '/Login.aspx';
			}
			$('.wx_type').off('click').on('click','li',function(){
				var _this = $(this),
		            index = _this.index();
					upData = {
                    	PageIndex:1,
                    	PageSize:20
                    },
					requestUrl = public_url+'/api/media/GetBindingsWxList';
					// requestUrl = 'json/1.json',
					temp = '#listTemp';
		        _this.addClass('selected').siblings().removeClass('selected');
				if(index == 1){
					temp = '#listTemp1';
					requestUrl = 'json/2.json';
					// requestUrl = public_url+'/api/media/GetBindingsWbList';
				}
				_self.QueryStatistInfo();
				_self.searchList(upData,'.media_table',temp,requestUrl);
			})
			$('.wx_type li').eq(0).click();
			_self.operate();
		},
		QueryStatistInfo:function(){
			setAjax({
				// url:'json/StatistInfo.json',
				url:public_url+'/api/media/GetStatistInfo',
				type:'get',
				data:{}
			},function(data){
				if(data.Status == 0){
					var returnData = data.Result;
					if(CTLogin.RegisterType == '199002'){//赤兔联盟微信服务号注册
						if(CTLogin.WX_Nickname){
							$('#user_name').html(CTLogin.WX_Nickname.substr(0,15));
						}
						if(CTLogin.WX_HeadimgUrl){
							$('.person_img').attr('src',CTLogin.WX_HeadimgUrl);
						}
					}else{//非微信注册
						if(returnData.UserHeadImg){
							$('.person_img').attr('src',returnData.UserHeadImg);
						}
						if(returnData.UserName){
							$('#user_name').html(returnData.UserName.substr(0,15));
						}
					}
					//累计收益
					if(returnData.EarningsPrice){
						$('#EarningsPrice').html(removePoint0(formatMoney(returnData.EarningsPrice,2,''),2)).addClass('pointer');
					}
					//订单数量
					if(returnData.OrderCount){
						$('#OrderCount').html(returnData.OrderCount).addClass('pointer');
					}
				}
			})
		},
		searchList:function(upData,container,temp,requestUrl){//查询列表:数据，容器，模板，接口url
			var _self = this;
			setAjax({
				url:requestUrl,
				type:'get',
				data:upData
			},function(data){
				if(data.Status == 0){
					console.log(data);
					if(temp == '#listTemp'){
						$('#media_num').html(data.Result.TotleCount);
					}
					if(data.Result.TotleCount > 0){
						$('.extension').show();
						$('#no_data').addClass('hide');
						$(container).html(ejs.render($(temp).html(),{list:data.Result.List}));
						_self.operate();
						//分页:数据超过20条才显示分页
						if(data.Result.TotleCount > 20){
							$("#pageContainer").show();
						}else{
							$("#pageContainer").hide();
						}
						$("#pageContainer").pagination(data.Result.TotleCount,{
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            current_page:upData.PageIndex,
                            callback: function (curPage, jg) {
                                var upData_pageIndex = {
                                	PageIndex:1,
                                	PageSize:20
                                }
                                upData_pageIndex.PageIndex = curPage;
                                setAjax({
									url:requestUrl,
                                    type: 'get',
                                    data: upData_pageIndex
                                }, function (data_pageIndex) {
                                    // 渲染数据
                                    $(container).html(ejs.render($(temp).html(),{list:data_pageIndex.Result.List}));
                                    _self.operate();
                                })
                            }
                        });
					}else{
						$(container).html(ejs.render($(temp).html(),{list:data.Result.List}));
						$('#no_data').removeClass('hide');
						$("#pageContainer").hide();
						_self.operate();
					}
				}
			})
		},
		operate:function(){//操作
			var _self = this;
			/*一些跳转*/
			$('#perfect_Info').off('click').on('click',function(){
				window.location = '/manager/advertister/account/accountManage.html'
			})
			//累计收益
			$('#EarningsPrice').off('click').on('click',function(){
				if($.trim($(this).html()) != '0.00'){
					window.location = '/manager/media/income/income.html'
				}
			})
			$('.add_authorization').off('click').on('click',function(){
				//调接口页面跳转
				$('#form').attr("action", 'http://www.chitunion.com/wx/WeChat.ashx?m=oauth').submit();
			})
			/*抢任务--订单数量>0时跳转*/
			$('#OrderCount').off('click').on('click',function(){
				if($.trim($(this).html()) != 0){
					window.location = '/static/media/contentDistribution.html';
				}
			})
			/*报价信息修改*/
			$('.price_layer').off('click').on('click',function(){
				var _this = $(this),
					MediaId = _this.attr('MediaId'),
					priceInfo = new Object();
				setAjax({
					url:public_url+'/api/media/GetBindingsWxInfo',
					// url:'json/3.json',
					type:'get',
					data:{
						MediaId:MediaId
					}
				},function(data){
					if(data.Status == 0){
						priceInfo = data.Result;
						console.log(priceInfo);
						//调接口渲染
						$.openPopupLayer({
				            name: "Layer",
				            url: "priceInfoLayer.html",
				            error: function (dd) {
				                alert(dd.status);
				            },
				            success: function () {
				            	_self.popupOperate(priceInfo,MediaId);
				            }
				        })
					}
				})
			})
			
		},
		popupOperate:function(priceInfo,MediaId){//弹层渲染及操作
			var _self = this;
			//城市选择
			_self.getCityInfo(priceInfo.OverlayArea);

			if( priceInfo.HeadImg ){
				$('#headImg').attr('src',priceInfo.HeadImg);
			}else{
				$('#headImg').attr('src','/images/default_headimg.png');
			}

			if( priceInfo.NickName ){
				$('#mediaName').html(priceInfo.NickName);
			}else{
				$('#mediaName').html('—');
			}
			if( priceInfo.WxNumber ){
				$('#mediaNumber').html(priceInfo.WxNumber);
			}else{
				$('#mediaNumber').html('—');
			}
			
			if( priceInfo.FansCount ){
				$('#fansCount').val(priceInfo.FansCount);
			}
			if( priceInfo.FansFemalePer ){
				$('.girl').val(priceInfo.FansFemalePer*100);
				$('.boy').val(priceInfo.FansMalePer*100);
			}
			if( priceInfo.First && priceInfo.First.Forward ){
				$('#price0').val(priceInfo.First.Forward);
			}
			if( priceInfo.First && priceInfo.First.OriginalPublish ){
				$('#price1').val(priceInfo.First.OriginalPublish);
			}
			if( priceInfo.Second && priceInfo.Second.Forward ){
				$('#price2').val(priceInfo.Second.Forward);
			}
			if( priceInfo.Second && priceInfo.Second.OriginalPublish ){
				$('#price3').val(priceInfo.Second.OriginalPublish);
			}
			if( priceInfo.Third && priceInfo.Third.Forward ){
				$('#price4').val(priceInfo.Third.Forward);
			}
			if( priceInfo.Third && priceInfo.Third.OriginalPublish ){
				$('#price5').val(priceInfo.Third.OriginalPublish);
			}
			if( priceInfo.Fourth && priceInfo.Fourth.Forward ){
				$('#price6').val(priceInfo.Fourth.Forward);
			}
			if( priceInfo.Fourth && priceInfo.Fourth.OriginalPublish ){
				$('#price7').val(priceInfo.Fourth.OriginalPublish);
			}
        	$('#closebt').off('click').on('click',function(){
        		$.closePopupLayer('Layer');
        	})

        	//操作
        	//粉丝数、粉丝性别比例、必须为数字
        	_self.limitNumber('#fansCount');
        	_self.limitNumber('.boy');
        	_self.limitNumber('.girl');
        	_self.limitNumber('#price0');
        	_self.limitNumber('#price1');
        	_self.limitNumber('#price2');
        	_self.limitNumber('#price3');
        	_self.limitNumber('#price4');
        	_self.limitNumber('#price5');
        	_self.limitNumber('#price6');
        	_self.limitNumber('#price7');

        	var mediaType = $('.wx_type .selected').attr('value');
    		if(mediaType == '14001'){//微信下调行业分类枚举
    			_self.getCategoryInfo(47,priceInfo.CategoryId);
    		}
        	
        	$('#submitMessage').off('click').on('click',function(){
        		var modify_priceInfo = _self.getParams(MediaId);
        		var $tip = $('.keep .tip');
        		if( !modify_priceInfo.FansCount ){//只能输入数字；最大为int类型的最大值，最小值为1
        			$tip.html('请输入粉丝数');
        			return
        		}
        		if( !modify_priceInfo.FansMalePer || !modify_priceInfo.FansFemalePer ){
        			$tip.html('请输入粉丝性别比例');
        			return
        		}
        		console.log(modify_priceInfo.FansMalePer + modify_priceInfo.FansFemalePer)
        		if( modify_priceInfo.FansMalePer + modify_priceInfo.FansFemalePer > 1){
        			$tip.html('粉丝性别比例不正确，请重新输入');
        			return
        		}
        		if(modify_priceInfo.OverlayArea.ProvinceID == '-2'){
        			$tip.html('请选择省份');
        			return
        		}

        		if( !modify_priceInfo.CategoryId ){
        			$tip.html('请选择行业分类');
        			return
        		}
        		if( modify_priceInfo.DeliveryPrices.length == 0){
        			$tip.html('至少选择一个价格');
        			return
        		}
        		var errorInfo = [];
        		modify_priceInfo.DeliveryPrices.forEach(function(item){
        			if((parseInt(item.Price)+'').length >8){
        				$tip.html('请输入正确的价格');
        				errorInfo.push(1);
        			}
        		})
        		if(errorInfo.length > 0){
        			return
        		}
        		$.closePopupLayer('Layer');
        		$('.wx_type li').eq(0).click();
        		setAjax({
        			url:public_url+'/api/media/updateWxOffer',
        			// url:'json/1.json',
        			type:'post',
        			data:modify_priceInfo
        		},function(data){
        			if(data.Status == 0){
        				layer.msg('成功',{time:1000});
        				$.closePopupLayer('Layer');
        				$('.wx_type li').eq(0).click();
        			}else{
        				layer.msg(data.Message);
        			}
        		})

        	})
		},
		getCityInfo:function(info){
			$(JSonData.masterArea).each(function (i) {
		        $('#province_select').append('<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>')
		    })
		    $('#province_select').off('change').on('change', function () {
		        var City1 = '', City2 = '<option value="-2">城市</option>';
		        if($('#province_select option:checked').attr('value') != '-2'){
			        $($(JSonData.masterArea)[$('#province_select option:checked').attr('i')].subArea).each(function (i) {
			            City1 += '<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>'
			        })
		    	}
		        $('#city_select').html(City2 + City1);
		    })
		    if(info && info.ProvinceId != 0){//说明是区域
		    	$('input[name=area]').eq(1).prop('checked',true);
		    	$('#province_select option').each(function(i,item){
		    		if($(item).attr('value') == info.ProvinceId){
		    			$(item).prop('selected',true);
		    			$('#province_select').change();
		    			$('#city_select option').each(function(j,single){
		    				if($(single).attr('value') == info.CityId){
		    					$(single).prop('selected',true);
		    				}
		    			})
		    		}
		    	})
		    }else{
		    	$('input[name=area]').eq(0).prop('checked',true);
		    }
		},
		getCategoryInfo:function(typeID,CategoryId){//根据枚举显示行业分类并选中已有的
			//调分类枚举接口
			setAjax({
				url:public_url+'/api/DictInfo/GetDictInfoByTypeID',
				type:'get',
				data:{
					typeID:typeID
				}
			},function(data){
				if(data.Status == 0){
					var str = '';
					data.Result.forEach(function(el){
						str += '<li><input type="radio" name="category" value='+el.DictId+'>'+el.DictName+'</li>';
					})
					$('#category_list').html(str);
					$('#category_list').find('input').each(function(i,item){
						if($(item).attr('value') == CategoryId){
							$(item).prop('checked',true);
						}
					})
				}
			})
		},
		getParams:function(MediaId){//获取微信报价信息修改参数
			var ProvinceID = 0,
				CityID = -2,
				DeliveryPricesList = [];
			$('#deleverPrice').find('input').each(function(i,item){
				var _this = $(item),
					parLi = _this.parent('li'),
					price = $.trim(_this.val())-0,
					ADPosition2 = $.trim(_this.attr('adPosition'))-0,
					ADPosition1 = $.trim(parLi.attr('value'))-0;
				if( price ){
					DeliveryPricesList.push({
						Price:parseFloat(price),
						ADPosition1:ADPosition1,
						ADPosition2:ADPosition2
					})
				}
			})
			if($('input[name=area]:checked').attr('value') == '0'){//全国
				ProvinceID = 0;
				CityID = -2;
			}else{
				ProvinceID = $('#province_select option:checked').attr('value')-0;
				CityID = $('#city_select option:checked').attr('value')-0;
			}
			return {
			    MediaId:MediaId-0,
			    FansCount:$.trim($('#fansCount').val())-0,
			    CategoryId:$('#category_list input:checked').attr('value')-0,
			    FansMalePer:parseFloat($.trim($('.boy').val())/100),
			    FansFemalePer:parseFloat($.trim($('.girl').val())/100),
			    OverlayArea:{
			        ProvinceID:ProvinceID-0,
			        CityID:CityID-0
			    },
			    DeliveryPrices:DeliveryPricesList
			}
		},
		limitNumber:function(ele){//限制只能输入数字
			//获取光标位置
		    function getCursorPos(obj) {
		        var CaretPos = 0;
		        // IE Support
		        if (document.selection) {
		            obj.focus (); //获取光标位置函数
		            var Sel = document.selection.createRange ();
		            Sel.moveStart ('character', -obj.value.length);
		            CaretPos = Sel.text.length;
		        }
		        // Firefox/Safari/Chrome/Opera support
		        else if (obj.selectionStart || obj.selectionStart == '0')
		            CaretPos = obj.selectionEnd;
		        return (CaretPos);
		    };
		    //定位光标
		    function setCursorPos(obj,pos){
		        if (obj.setSelectionRange) { //Firefox/Safari/Chrome/Opera
		            obj.focus(); //
		            obj.setSelectionRange(pos,pos);
		        } else if (obj.createTextRange) { // IE
		            var range = obj.createTextRange();
		            range.collapse(true);
		            range.moveEnd('character', pos);
		            range.moveStart('character', pos);
		            range.select();
		        }
		    };
		    function replaceAndSetPos(obj,pattern,text){
		        if ($(obj).val() == "" || $(obj).val() == null) {
		            return;
		        }
		        var pos=getCursorPos(obj);//保存原始光标位置
		        var temp=$(obj).val(); //保存原始值
		        obj.value=temp.replace(pattern,text);//替换掉非法值
		        //截掉超过长度限制的字串（此方法要求已设定元素的maxlength属性值）
		        var max_length = obj.getAttribute? parseInt(obj.getAttribute("maxlength")) : "";
		        if( obj.value.length > max_length){
		            var str1 = obj.value.substring( 0,pos-1 );
		            var str2 = obj.value.substring( pos,max_length+1 );
		            obj.value = str1 + str2;
		        }
		        pos=pos-(temp.length-obj.value.length);//当前光标位置
		        setCursorPos(obj,pos);//设置光标
		        //el.onkeydown = null;
		    };
		    $(ele).on("input",function(){replaceAndSetPos(this,/[^0-9]/g,'');});
		    $(ele).on("keyup",function(){replaceAndSetPos(this,/[^0-9]/g,'');})
		}
	}
	new TableList();
})