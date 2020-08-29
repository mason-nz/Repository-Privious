/*
* Written by:     wangcan
* function:       智投
* Created Date:   2017-07-24
* Modified Date:  2017-08-21 增加导出媒体
*/
$(function(){
	//checkedInfo是为了页面显示存储数据，selectInfo是为了调后台接口存储数据,config是为了导出媒体
	var checkedInfo = [],selectInfo = [];
	var config = {};
	function WisdomDelivery(){
		this.chooseOption();
		this.intelligentDelivery();
		this.submmit();
	}
	WisdomDelivery.prototype = {
		constructor:WisdomDelivery,
		chooseOption:function(){//选择城市、推广目的，车型，品牌、投放日期等选择
			var _self = this;
			$('#chooseCity').off('click').on('click',function(){
				if(getSelectCityInfo(checkedInfo).cityNum >= 20){
					layer.msg('最多可选择20个城市');
					return
				}
		        $.openPopupLayer({
		            name: "popLayerDemo",
		            url: "chooseCityForOrder.html",
		            success: function () {
		            	// 城市信息显示
		            	$(JSonData.masterArea).each(function (i) {
				            $('#province').append('<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>')
				        })
					    $('#province').off('change').on('change', function () {
					        var City1 = '', City2 = '<option value="-2">城市</option>';
					        $($(JSonData.masterArea)[$('#province option:checked').attr('i')].subArea).each(function (i) {
					            City1 += '<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>'
					        })
					        $('#city').html(City2 + City1);
					        var province = $(this).find('option:checked').html();
					        if(province == '北京' || province == '上海' || province == '重庆' || province == '天津'){
					        	$('#city').find('option:eq(0)').prop('selected',true);
					        	$('.selectCity').hide();
					        }else{
					        	$('#city').show();
					        	$('.selectCity').show();
					        }
					    })
					    //限制推广预算和媒体数量只能是数字
					    $('.promotionBudget').on("input",function(){
			                replaceAndSetPos(this,/[^0-9]/g,'');
			            })
			            $('.mediaCount').on("input",function(){
			                replaceAndSetPos(this,/[^0-9]/g,'');
			            })
		                /*弹层关闭*/
		                $('#closebt').each(function(){
		                    $(this).off('click').on('click',function(event){
		                        event.preventDefault();
		                        $.closePopupLayer('popLayerDemo');
		                    })
		                });
		                /*提交*/
		                $('#submitMessage').off('click').on('click',function(event){
		                	event.preventDefault();
		                    var obj = {
					            "ProvinceID": $('#province').val(),//必填
					            "ProvinceName":$('#province option:checked').html(),//必填
					            "CityID":$('#city').val(),//非必填（如果没有选择，则为-2）
					            "CityName": $('#city option:checked').html()=='城市'?'':$('#city option:checked').html(),//非必填
					            "Budget":$('.promotionBudget').val()-0,//必填
					            "MediaCount":$('.mediaCount').val()-0,//必填
					            "OriginContain":$('.isOriginal').prop('checked') == true ? 1:0//是否包含仅原创
					    	};
					    	var tipsInfo = [];
					    	if(obj.ProvinceID == -2){
					    		$('#tipsInfo').html('<img src="../images/icon21.png"> 请选择城市');
					    		tipsInfo.push('请选择城市');
					    		return;
					    	}
					    	if(obj.Budget == "" || obj.Budget == 0){
					    		$('#tipsInfo').html('<img src="../images/icon21.png"> 请输入推广预算');
					    		tipsInfo.push('请输入推广预算');
					    		return;
					    	}
					    	if(obj.MediaCount == "" || obj.MediaCount == 0){
					    		$('#tipsInfo').html('<img src="../images/icon21.png"> 请输入媒体数量');
					    		tipsInfo.push('请输入媒体数量');
					    		return;
					    	}
					    	if(isChooseCity(obj.ProvinceID,obj.CityID,checkedInfo) == false){
					    		$('#tipsInfo').html('<img src="../images/icon21.png"> 城市不能重复');
					    		tipsInfo.push('城市不能重复');
					    		return;
					    	}
					    	//判断媒体数量总和，如果小于100，可添加，否则提示
					    	if(getSelectCityInfo(checkedInfo).mediaNum-(-obj.MediaCount) > 100){
					    		$('#tipsInfo').html('<img src="../images/icon21.png"> 媒体数量累加不得超过100');
					    		tipsInfo.push('媒体数量累加不得超过100');
					    		return;
					    	}
					    	//如果验证通过
					    	if(tipsInfo.length == 0){
					    		checkedInfo.push(obj)
					    	}else{
					    		return
					    	}
					    	$.closePopupLayer('popLayerDemo');
					    	var str = "";
					    	for(var i=0;i<checkedInfo.length;i++){
					    		//选择省市，如果只选择省，显示省份，如果选择了市，显示市名称
					    		var checkedCity = "",mediaInfo = "";
					    		if(checkedInfo[i].CityID == -2){
					    			checkedCity = checkedInfo[i].ProvinceName;
					    		}else{
					    			checkedCity = checkedInfo[i].ProvinceName+'-'+checkedInfo[i].CityName;
					    		}
					    		//媒体数量显示，如果含仅原创勾选，显示（含仅原创），否则显示不含仅原创
					    		if(checkedInfo[i].OriginContain){
					    			mediaInfo = checkedInfo[i].MediaCount+'个媒体（含仅原创）'
					    		}else{
					    			mediaInfo = checkedInfo[i].MediaCount+'个媒体（不含仅原创）'
					    		}
					    		str += '<tr CityID='+checkedInfo[i].CityID+' ProvinceID='+checkedInfo[i].ProvinceID+'>'+
		                                '<td>'+checkedCity+'</td>'+
		                                '<td>'+checkedInfo[i].Budget+'</td>'+
		                                '<td>'+mediaInfo+'</td>'+
		                                '<td>'+
		                                    '<a href="javascript:void(0);" class="city_delete">'+
		                                        '<img src="../ImagesNew/delete.png" title="删除"/>'+
		                                    '</a>'+
		                                '</td>'+
		                            '</tr>';
					    	}
					    	$('.checedCity tbody').html(str);
					    	$('.checedCity').show();
					    	//更新预算总计
					    	$('#totalBudget').html(formatMoney(getSelectCityInfo(checkedInfo).Budget));
					    	$('.chooseCity').find('.hidden_tip').hide();
					    	//删除城市
							$('.city_delete').off('click').on('click',function(){
								var _this = $(this);
								var CityID = _this.parents('tr').attr('CityID'),
									ProvinceID = _this.parents('tr').attr('ProvinceID'),
									index = 0;
								checkedInfo.forEach(function(value,idx){
									if(value.CityID == CityID && value.ProvinceID == ProvinceID){
										index = idx;
									}
								})
								checkedInfo.splice(index,1);
								if(_this.parents('tbody').find('tr').length == 1){
									_this.parents('tr').remove();
									$('.checedCity').hide();
								}
								_this.parents('tr').remove();

								$('#totalBudget').html(formatMoney(getSelectCityInfo(checkedInfo).Budget));
								
							})
		                })
		            }
		        })
		    })
			/*当用户进行选择时，不显示提示信息*/
			//推广目的
			$('.promotionPurpose').off('change').on('change','input',function(){
				$('.promotionPurpose').find('.hidden_tip').hide();
			})
			//车系品牌显示
			$.ajax({
				url:'http://www.chitunion.com/api/CarSerial/QueryBrand',
				type:'get',
				async: false,
		        dataType: 'json',
		        xhrFields: {
		            withCredentials: true
		        },
		        crossDomain: true,
		        data: {},
		        success:function(data){
		        	if(data.Status == 0){
						var str = '<option MasterId="-2">请选择品牌</option>';
						for(var i=0;i<data.Result.length;i++){
							str += '<option MasterId='+data.Result[i].MasterId+'>'+data.Result[i].Name+'</option>'
						}
						$('.brand').html(str);

						$('.brand').off('change').on('change',function(){
							$('.brands').find('.hidden_tip').hide();
							var MasterBrandId = $(this).find('option:checked').attr('MasterId');
							if(MasterBrandId == -2){
								$('.series').html('<option BrandId="-2">请选择子品牌</option>');
								$('.Models').html('<option CarSerialId="-2">请选择车型</option>');
							}else{
								$.ajax({
									url:'http://www.chitunion.com/api/CarSerial/QueryBrand',
									type:'get',
									async: false,
							        dataType: 'json',
							        xhrFields: {
							            withCredentials: true
							        },
							        crossDomain: true,
							        data: {
							        	MasterBrandId:MasterBrandId
							        },
							        success:function(data){
							        	if(data.Status == 0){
											var str1 = '<option BrandId="-2">请选择子品牌</option>';
											for(var j=0;j<data.Result.length;j++){
												str1 += '<option BrandId='+data.Result[j].BrandId+'>'+data.Result[j].Name+'</option>'
											}
											$('.series').html(str1);
											$('.Models').html('<option MasterId="-2">请选择车型</option>');
											$('.series').off('change').on('change',function(){
												$('.brands').find('.hidden_tip').hide();
												var BrandId = $(this).find('option:checked').attr('BrandId');
												if(BrandId == -2){
													$('.Models').html('<option MasterId="-2">请选择车型</option>');
												}else{
													$.ajax({
														url:'http://www.chitunion.com/api/CarSerial/QuerySerialList',
														type:'get',
														async: false,
												        dataType: 'json',
												        xhrFields: {
												            withCredentials: true
												        },
												        crossDomain: true,
												        data: {
												        	BrandId:BrandId
												        },
												        success:function(data){
												        	var str2 = '<option CarSerialId="-2">请选择车型</option>';
															for(var k=0;k<data.Result.length;k++){
																str2 += '<option BrandId='+data.Result[k].BrandId+' CarSerialId='+data.Result[k].CarSerialId+'>'+data.Result[k].ShowName+'</option>'
															}
															$('.Models').html(str2);
															$('.Models').off('change').on('change',function(){
																$('.brands').find('.hidden_tip').hide();
															})
												        }
													})
												}
											})
							        	}
							        }
								});
							}
						})
		        	}
		        }
			});
			//集客入口
			$('.exit').off('change').on('change','input',function(){
				$('.exit').find('.hidden_tip').hide();
			})
			//投放日期
			$('#deleverDay').off('click').on('click', function () {
		        laydate({
		            fixed: false,
		            elem: '#deleverDay',
		            istoday : false,
		            min:laydate.now(+20),
		            choose: function (date) {
		                $('.deleverDay').find('.hidden_tip').hide();
		            }
		        });
		    });
			//验证城市未选择
			function isChooseCity(ProvinceID,CityID,checkedInfo) {
				var flag = true;
				checkedInfo.forEach(function(value,index){
		    		if(value.ProvinceID == ProvinceID && value.CityID == CityID){
		    			flag = false;
		    		}
		    	})
		    	return flag
			}
		    //获取已选择的媒体数量之和，推广预算之和，城市数量之和。
		    function getSelectCityInfo(checkedInfo){
		    	var returnObj = {
		    		mediaNum : 0,
		    		Budget:0,
		    		cityNum : 0
		    	};
		    	checkedInfo.forEach(function(value,index){
		    		returnObj.mediaNum += value.MediaCount;
		    		returnObj.Budget += value.Budget;
		    		returnObj.cityNum ++ ;
		    	})
		    	return returnObj;
		    }
		    // 只能输入数字 
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
		    //替换后定位光标在原处,可以这样调用onkeyup=replaceAndSetPos(this,/[^/d]/g,'');
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
		},
		intelligentDelivery:function(){//智能投放
			var _self = this;
			//智能投放
			$('#intelligentDelivery').off('click').on('click',function(){
				//获取所有选择的信息
				var curSelectInfo = 
				{
					AreaInfo: [],//城市信息
					BudgetTotal : getSelectCityInfo(checkedInfo).Budget,//预算总计
					OrderRemark : [],//推广目的
					MasterBrand : $('.brands .brand').val(),
					CarBrand : $('.brands .series').val(),
					CarSerial : $('.brands .Models').val(),//品牌或车型
					MasterID: $('.brands .brand option:checked').attr('masterid'),
					BrandID:$('.brands .series option:checked').attr('brandid'),
					SerialID: $('.brands .Models option:checked').attr('carserialid'),
					LaunchTime : $('#deleverDay').val(),//投放日期
					JKEntrance : $('.exit').find('input:checked').attr('value')//集客入口

				}
				$('.promotionPurpose').find('input:checked').each(function(index,item){
					curSelectInfo.OrderRemark.push($(item).attr('value'));
				});

				var deleverError = [];
				if( curSelectInfo.BudgetTotal == 0 ){
					$('.chooseCity').find('.hidden_tip').show();
					deleverError.push('请选择城市');
				}
				if( curSelectInfo.OrderRemark.length == 0){
					$('.promotionPurpose').find('.hidden_tip').show();
					deleverError.push('请选择推广目的');
				}
				if( curSelectInfo.MasterBrand == '请选择品牌' || curSelectInfo.CarBrand == '请选择子品牌' || curSelectInfo.CarSerial == '请选择车型'){
					$('.brands').find('.hidden_tip').show();
					deleverError.push('请选择品牌或车型');
				}
				if( !curSelectInfo.LaunchTime){
					$('.deleverDay').find('.hidden_tip').show();
					deleverError.push('请选择预计投放日期');
				}
				if( !curSelectInfo.JKEntrance ){
					$('.exit').find('.hidden_tip').show();
					deleverError.push('请选择集客入口');
				}
				if(deleverError.length == 0){
					curSelectInfo.OrderRemark = curSelectInfo.OrderRemark.join(',');//推广目的为,拼接的字符串
					selectInfo = curSelectInfo;
					checkedInfo.forEach(function(v,j){
						var AreaInfoObj = {
							ProvinceID : v.ProvinceID,
							CityID : v.CityID,
							Budget : v.Budget,
							MediaCount : v.MediaCount,
							OriginContain : v.OriginContain == 1?true:false
						}
						selectInfo.AreaInfo.push(AreaInfoObj);
					})
		            $('.orderInfo').html('');
					_self.getAllCount();
		            _self.getInitalData(checkedInfo,selectInfo);
				}
			})
			//获取已选择的媒体数量之和，推广预算之和，城市数量之和。
		    function getSelectCityInfo(checkedInfo){
		    	var returnObj = {
		    		mediaNum : 0,
		    		Budget:0,
		    		cityNum : 0
		    	};
		    	checkedInfo.forEach(function(value,index){
		    		returnObj.mediaNum += value.MediaCount;
		    		returnObj.Budget += value.Budget;
		    		returnObj.cityNum ++ ;
		    	})
		    	return returnObj;
		    }
		},
		getInitalData:function(checkedInfo,selectInfo){//智投返回结果
			var _self = this;
			setAjax({
				url:'http://www.chitunion.com/api/ADOrderInfo/IntelligenceRecommend?v=1_1',
				type:'post',
				data:selectInfo
			},function(data){
				if(data.Status == 0){
					//每次获取新的数据后，都将config重新设置成返回数据
					config = data.Result;
					//调接口
					$('.orderInfo').html(ejs.render($('#orderInfo').html(), {list: data.Result}));
					//当媒体显示数量小于之前选择的数量时，提示用户媒体数量不足
					var littleMediaCityName = [];
					$('.orderInfo').find('.recommendCity').each(function(i,item){
						checkedInfo.forEach(function(v,j){
							if(v.ProvinceID == $(item).attr('ProvinceID') && v.CityID == $(item).attr('CityID')){
								if($(item).find('.mediaInfo').length < v.MediaCount){
									littleMediaCityName.push(v.CityName == ''?v.ProvinceName:v.CityName);
									$(item).hide();
								}
							}
						})
					});
					if(littleMediaCityName.length == 0){
						$('.errorInfo .see_now').hide();
					}else{
	                    $('.errorInfo .see_now').show();
						$('.errorInfo .see_now').html(littleMediaCityName.join('、')+'媒体数量不足，是否查看现有媒体！<a href="javascript:;" style="color:#FF4F4F;">立即查看</a>');
					}
					//当城市下没有对应的媒体时，提示用户没有匹配的媒体
					var noMediaCityName = [];
	                checkedInfo.forEach(function(v,k){
	                	var curCityName = v.CityName?v.CityName:v.ProvinceName;
	                    noMediaCityName.push(curCityName);
	                    if(curCityName == '北京' || curCityName=='上海' || curCityName=='天津' || curCityName == '重庆'){
	                    	for(var n=0;n<data.Result.length;n++){
	                        	if(data.Result[n].ProvinceName == curCityName){
	                        		for(var l=0;l<noMediaCityName.length;l++){
										if(noMediaCityName[l] == curCityName){
											noMediaCityName.splice(l,1);
										}
									}
	                        	}
	                    	}
	                    }else{
	                        for(var n=0;n<data.Result.length;n++){
	                           //如果城市名称和省份名称都存在，说明此城市存在数据，把这个城市从没有数据的媒体城市中移除。否则，不存在数据，需要将城市信息显示在页面上。
								if(data.Result[n].ProvinceName == v.ProvinceName && data.Result[n].CityName == v.CityName){
									for(var l=0;l<noMediaCityName.length;l++){
										if(noMediaCityName[l] == curCityName){
											noMediaCityName.splice(l,1);
										}
									}
								}
	                    	}
	                    }
	                })	

	                if(noMediaCityName.length == 0){
						$('.errorInfo .no_media').hide();
					}else{
	                    $('.errorInfo .no_media').show();
						$('.errorInfo .no_media').html(noMediaCityName.join('、')+'没有匹配的媒体，请重新选择区域！ <a href="javascript:;" style="color:#FF4F4F;">重新选择</a>');
					}
					if(noMediaCityName.length == 0 && littleMediaCityName.length == 0){
						$('.errorInfo .reChoose,.errorInfo .see_now').hide();
					}else{
						$('.goToTop').show();
					}
					if($('.mediaInfo:visible').length == 0){
						$('.bottom').hide();
					}else{
						$('.bottom').show();
					}
					_self.getAllCount();
					//错误提示信息，点击操作：查看、重新选择
					$('.reChoose').off('click').on('click','a',function(){
						$('html,body').animate({
							scrollTop: '0'
						},100)
	                    $('.errorInfo .reChoose,.errorInfo .see_now').hide();
					})
					$('.see_now').off('click').on('click','a',function(){
						$('.recommendCity').show();
	                    $('.errorInfo .reChoose,.errorInfo .see_now').hide();
						_self.getAllCount();
						if($('.mediaInfo:visible').length == 0){
							$('.bottom').hide();
						}else{
							$('.bottom').show();
						}
					})


					//全选、单选、原创参考价选择
					$('.twoChecked').off('change').on('change',function(){
						$(this).parents('table').find('.onecheck').prop('checked',$(this).prop('checked'));
						$(this).parents('table').find('.onecheck').parents('.mediaInfo').attr('isSelected',$(this).prop('checked')==true?1:0);
						$(this).parents('.recommendCity').attr('isSelected',$(this).prop('checked')==true?1:0);
						_self.getAllCount();
					})
					$('.onecheck').off('change').on('change',function(){
						var _this = $(this);
						//单选影响全选
						if(_this.parents('.recommendCity').find('.onecheck').length == _this.parents('.recommendCity').find('.onecheck:checked').length){
							_this.parents('.recommendCity').find('.twoChecked').prop('checked','checked');
							_this.parents('.recommendCity').attr('isselected',1);
						}else{
							_this.parents('.recommendCity').find('.twoChecked').prop('checked',false);
							if(_this.parents('.recommendCity').find('.onecheck:checked').length == 0){
								_this.parents('.recommendCity').attr('isselected',0);
							}else{
								_this.parents('.recommendCity').attr('isselected',1);
							}
						}
						//单选变化时，对应改变tr上绑定的isSelected属性
						if(_this.prop('checked')){
							_this.parents('.mediaInfo').attr('isSelected',1);
						}else{
							_this.parents('.mediaInfo').attr('isSelected',0);
						}
						_self.getAllCount();
					})
					$('.priceChoose').off('change').on('change',function(){
						var _this = $(this);
						if(_this.prop('checked')){
							_this.parents('tr').attr('EnableOriginPrice',true);
							_this.parent('p').css('color','#666');
							var originalCount = 0;
							_this.parents('tbody').find('tr').each(function(){
								if($(this).attr('EnableOriginPrice') == 'true'){
									originalCount += $(this).attr('OriginaReferencePrice')-0;
								}
							});
							_this.parents('tbody').find('.priceAccount .originalCount').html(formatMoney(originalCount));
						}else{
							_this.parents('tr').attr('EnableOriginPrice',false);
							_this.parent('p').css('color','#D1D1D1');
							var originalCount = 0;
							_this.parents('tbody').find('tr').each(function(){
								if($(this).attr('EnableOriginPrice') == 'true'){
									originalCount += $(this).attr('OriginaReferencePrice')-0;
								}
							});
							_this.parents('tbody').find('.priceAccount .originalCount').html(formatMoney(originalCount));
						}
						_self.getAllCount();
					})
	                //导出媒体
					$('#outPut').off('click').on('click',function(){
						if($('.orderInfo .mediaInfo[isSelected=1]:visible').length < 1){
							layer.msg('请选择媒体');
							return
						}
						// console.log(selectInfo,'这是点击导出媒体后的selectInfo');
						var upData = {
							MasterBrand : selectInfo.MasterBrand,
							CarBrand : selectInfo.CarBrand,
							CarSerial : selectInfo.CarSerial,
							BudgetTotal : selectInfo.BudgetTotal,
							LaunchTime : selectInfo.LaunchTime,
							JKEntrance : selectInfo.JKEntrance == 1 ? true : false,
							AreaInfo:[]
						}
						var curTime = new Date();
						var tommorow20 = new Date((+curTime)+20*24*3600*1000).format('yyyy-MM-dd');
						if(upData.LaunchTime.substr(0,10) < tommorow20){
							layer.msg('预计投放日期需为20天以后的日期，请修改！');
							$('.deleverDay').find('.hidden_tip').html('<img src="../images/icon21.png"> 预计投放日期需为20天以后的日期，请修改！').show();
							return
						}
						// console.log(config,'这是点击导出媒体后的config')
						config.forEach(function(v){
							//通过城市ID循环判断，
							var AreaInfoSingle = {
								ProvinceName: v.ProvinceName,
								CityName:v.CityName == null ? '':v.CityName,
								PublishDetails:[]
							}
							v.PublishDetails.forEach(function(item,j){
								//通过PublishDetailID广告位ID比较，如果在已选的数据中存在，说明需要传给后台，如果没有选择，不传
								var count = 0;
								var compareData = _self.getAllInfo(selectInfo).ADDetails;
								// console.log(compareData,'这是对比数据');
								compareData.forEach(function(single){
									single.PublishDetails.forEach(function(oneObj){
										if(item.PublishDetailID == oneObj.PublishDetailID){
											count++
										}
									})
								});
								//如果没有选中，不传
								if(!count){
									return;
								}
								var PublishDetailsSingle = {
									MediaName : item.MediaName,
									MediaNumber : item.MediaNumber,
									ADPosition : item.ADPosition,
									CreateType : item.CreateType,
									FansCount : item.FansCount,
									ADLaunchDays : 1,//投入次数，目前写死1
									CostReferencePrice : item.CostReferencePrice,
									OriginalReferencePrice : item.OriginalReferencePrice
								}
								compareData.forEach(function(single){
									single.PublishDetails.forEach(function(oneObj){
										if(item.PublishDetailID == oneObj.PublishDetailID){
											if( oneObj.EnableOriginPrice == 'false'){
												PublishDetailsSingle.OriginalReferencePrice = 0
											}
										}
									})
								});
								AreaInfoSingle.PublishDetails.push(PublishDetailsSingle);
							})
							//如果有对应媒体，再push进去，避免出现数据为空的情况
							if(AreaInfoSingle.PublishDetails.length){
								upData.AreaInfo.push(AreaInfoSingle);
							}
						})
						// console.log(upData,'这是点击导出媒体后,需要向后台传的数据');
						setAjax({
							url:'http://www.chitunion.com/api/ADOrderInfo/IntelligenceRecommendExport?v=1_1',
							type:'post',
							data:upData
						},function(data){
							if(data.Status == 0){
								window.location = decodeURIComponent(data.Result);
							}
							
						})
					})
				}else{
					if(data.Status == -1 && data.Message == '没有数据'){
						$('.bottom').hide();
						//当城市下没有对应的媒体时，提示用户没有匹配的媒体
						var noMediaCityName = [];
						checkedInfo.forEach(function(v,k){
							if(v.CityName){
								noMediaCityName.push(v.CityName);
							}else{
								noMediaCityName.push(v.ProvinceName);
							}
						})
	                    $('.errorInfo .reChoose').show();
						$('.errorInfo .no_media').html(noMediaCityName.join('、')+'没有匹配的媒体，请重新选择区域！ <a href="javascript:;" style="color:#FF4F4F;">重新选择</a>');
						//错误提示信息，点击操作：查看、重新选择
						$('.reChoose').off('click').on('click','a',function(){
							$('html,body').animate({
								scrollTop: '0'
							},100)
	                        $('.errorInfo .reChoose,.errorInfo .see_now').hide();
						})
					}
				}
			})
		},
		submmit:function(){//确认投放
			var _self = this;
			$('#nextBtn').off('click').on('click',function(){
				if($('.orderInfo .mediaInfo[isSelected=1]:visible').length > 0){
					var upData = _self.getAllInfo(selectInfo);
		            setAjax({
						url:'http://www.chitunion.com/api/ADOrderInfo/IntelligenceADOrderInfoCrud?v=1_1',
						type:'post',
						data:upData
					},function(data){
						if(data.Status == 0){
							window.location = '/OrderManager/shopcartForWisdomDelivery02.html?orderID='+data.Message;
						}else{
							layer.msg(data.Message);
						}
					})

				}else{
					layer.msg('请选择媒体');
				}
			})
		},
		getAllCount:function(){//获取媒体信息：媒体个数、销售参考价、成本参考价、原创参考价
			//媒体数目
			var mediaNumber = $('.orderInfo .mediaInfo[isSelected=1]:visible').length;
			$('.bottom').find('.tableNum').html(mediaNumber);
			//销售价、成本价、原创价
			var SalePrice = 0,CostReferencePrice = 0,OriginaReferencePrice = 0;
			$('.orderInfo').find('.mediaInfo[isSelected=1]:visible').each(function(i,item) {
				SalePrice += ($(item).attr('SalePrice')-0);
				CostReferencePrice += ($(item).attr('CostReferencePrice')-0);
				if($(item).find('.priceChoose').prop('checked')){
					OriginaReferencePrice += ($(item).attr('OriginaReferencePrice')-0);
					SalePrice += ($(item).attr('OriginaReferencePrice')-0);
					CostReferencePrice += ($(item).attr('OriginaReferencePrice')-0);
				}
			});
			$('.SalePrice').html(formatMoney(SalePrice));
			$('.CostReferencePrice').html(formatMoney(CostReferencePrice));
			$('.OriginaReferencePrice').html(formatMoney(OriginaReferencePrice));
		},
		getAllInfo:function(selectInfo){//获取页面上的信息
	        var upData = {
	            "optType" : 1,
	            "ADOrderInfo" : {
	                "OrderID" : "",
	                "OrderName" : "",
	                "Status" : 16001,
	                "CustomerID" : "gt86ZRCRjng%3d",
	                "CRMCustomerID":"",
	                "CustomerText" : "",
	                "MarketingPolices" : "",//营销政策
	                "UploadFileURL" : "",
	                "LaunchTime" : selectInfo.LaunchTime,
	                "BudgetTotal" : selectInfo.BudgetTotal,
	                "OrderRemark" :selectInfo.OrderRemark,
	                "MasterID" : selectInfo.MasterID,
	                "BrandID" : selectInfo.BrandID,
	                "SerialID" : selectInfo.SerialID,
	                "MasterName" : selectInfo.MasterBrand,
	                "BrandName" : selectInfo.CarBrand,
	                "SerialName" : selectInfo.CarSerial,
	                "JKEntrance" : selectInfo.JKEntrance==1?true:false
	            },
	            "ADDetails" : []
	        }
	        $('.recommendCity[isSelected=1]:visible').each(function(i,single){
	            var _single = $(single);
	            var ADDetailsObj = {
	                ProvinceID : _single.attr('ProvinceID'),
	                CityID : _single.attr('CityID'),
	                Budget : 0,
	                MediaCount : 0,
	                OriginContain : true,
	                PublishDetails:[]
	            };
	            selectInfo.AreaInfo.forEach(function(v,k){
	                if(ADDetailsObj.ProvinceID == v.ProvinceID && ADDetailsObj.CityID == v.CityID){
	                    ADDetailsObj.Budget = v.Budget;
	                    ADDetailsObj.MediaCount = _single.find('.mediaInfo').length;
	                    ADDetailsObj.OriginContain = v.OriginContain==1?true:false;
	                }
	            })
	            $(single).find('.mediaInfo[isSelected=1]:visible').each(function(j,item){
	                var _item = $(item);
	                var PublishDetailsObj = {
	                    PublishDetailID : _item.attr('PublishDetailID'),
	                    MediaType : _item.attr('MediaType'),
	                    MediaID : _item.attr('MediaID'),
	                    AdjustPrice : 0,
	                    EnableOriginPrice : _item.attr('EnableOriginPrice'),
	                    CostReferencePrice : _item.attr('CostReferencePrice'),
	                    CostPrice : 0,
	                    FinalCostPrice : 0,
	                    ChannelID : 0,
	                    LaunchTime : selectInfo.LaunchTime
	                }
	                ADDetailsObj.PublishDetails.push(PublishDetailsObj);
	            })
	            upData.ADDetails.push(ADDetailsObj);
	        });
	        // console.log(upData);
	        return upData;
		}
	}
	new WisdomDelivery();
    /* 将当前时间格式变为2017-04-21*/
	Date.prototype.format = function(fmt) {
	    var o = {
	        "M+" : this.getMonth()+1,                 //月份
	        "d+" : this.getDate(),                    //日
	        "h+" : this.getHours(),                   //小时
	        "m+" : this.getMinutes(),                 //分
	        "s+" : this.getSeconds(),                 //秒
	        "q+" : Math.floor((this.getMonth()+3)/3), //季度
	        "S"  : this.getMilliseconds()             //毫秒
	    };
	    if(/(y+)/.test(fmt)) {
	        fmt=fmt.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));
	    }
	    for(var k in o) {
	        if(new RegExp("("+ k +")").test(fmt)){
	            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));
	        }
	    }
	    return fmt;
	}
})