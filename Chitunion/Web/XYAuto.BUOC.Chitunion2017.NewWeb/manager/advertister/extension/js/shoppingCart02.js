/*
* Written by:     wangc
* function:       选号车
* Created Date:   2017-12-20
* Modified Date:
*/
$(function(){
	function TableList(){
		this.init();
	}
	TableList.prototype = {
		constructor:TableList,
		init:function(){
			var _self = this;
			//如果未登录或不是广告主，跳转到登录页面
			if(CTLogin.IsLogin){
	            if( CTLogin.Category != '29001'){
	                layer.confirm('您不是广告主，请登录！', {
	                    btn: ['登录'] //按钮
	                }, function(){
	                    var url = encodeURI(public_url+'/manager/advertister/extension/shoppingCart02.html');
	                    window.location = '/Exit.aspx?gourl='+url;
	                });
	            }else{
					_self.searchList();
					_self.operate();
					_self.uploadFile('uploadFile');
	            }
	        }else{
	            layer.confirm('您尚未登录，请登录！', {
	                btn: ['登录'] //按钮
	            }, function(){
	                var url = encodeURI(public_url+'/manager/advertister/extension/shoppingCart02.html');
	                window.location = '/Login.aspx?gourl='+url;
	            });
	        }
			
		},
		searchList:function(){
			var _self = this;
			//推广目的枚举
			setAjax({
				url:public_url+'/api/DictInfo/GetDictInfoByTypeID',
				// url:'json/purpose.json',
				type:'get',
				data:{
					typeID : 97
				}
			},function(data){
				if(data.Status == 0){
					var str = '';
					data.Result.forEach(function(item,i){
						str +='<span><input type="checkbox" name="purpose" dict='+Math.pow(2,i)+' />'+  item.DictName+'</span>'
					})
					$('.chooseList').html(str);
				}
			})
			//获取各个媒体类型下的购物车数据
			setAjax({
				// url:'json/shop1.json',
				url:public_url+'/api/MediaMatching/GetCartMediaList',
				type:'get',
				data:{}
			},function(data){
				if(data.Status == 0){
     				$('#wechat_num').html(data.Result.CartWeiXin.TotalCount);
     				$('#weibo_num').html(data.Result.CartWeiBo.TotalCount);
     				$('#app_num').html(data.Result.CartApp.TotalCount);
     				$('.total_num').html(data.Result.CartWeiXin.TotalCount+data.Result.CartWeiBo.TotalCount+data.Result.CartApp.TotalCount);
					$('#table_wechat').html(ejs.render($('#weixin_list').html(),{list:data.Result.CartWeiXin.List}));
					$('#table_weibo').html(ejs.render($('#weibo_list').html(),{list:data.Result.CartWeiBo.List}));
					$('#table_app').html(ejs.render($('#app_list').html(),{list:data.Result.CartApp.List}));
					// $('#navLeft').css('height',$('.contaner_body').height());
					_self.operate();
				}else{
					layer.msg(data.Message,{time:1000});
				}
			})
		},
		operate:function(){
			var _self = this;
			//推广预算:只能是数字
			_self.limitNumber('#budget');

			//推广时间选择
			$('#beginTime').off('click').on('click', function () {
                laydate({
                    fixed: false,
                    elem: '#beginTime',
                    min:laydate.now(+3),
                    istoday:false,
                	isclear: true,
                    choose: function (date) {
                        if (date > $('#endTime').val() && $('#endTime').val()) {
                            layer.alert('结束日期必须大于开始日期！');
                            $('#beginTime').val('')
                        }
                        $('#beginTime').parents('ul').find('.tipInfo').html('').hide();
                    }
                });
            });
            $('#endTime').off('click').on('click', function () {
                laydate({
                    fixed: false,
                    elem: '#endTime',
                    min:laydate.now(+3),
                	isclear: true,
                    istoday:false,
                    choose: function (date) {
                        if (date < $('#beginTime').val() && $('#beginTime').val()) {
                            layer.alert('结束日期必须大于开始日期！');
                            $('#endTime').val('')
                        }
                        $('#beginTime').parents('ul').find('.tipInfo').html('').hide();
                    }
                });
            });

			//切换媒体类型
			$('.wx_type').off('click').on('click','li',function(){
		        var _this = $(this),
		            index = _this.index();
		        _this.addClass('selected').siblings().removeClass('selected');
		        $('.rightMain').find('.table').hide().end().find('.table').eq(''+index+'').show();
		    	// $('#navLeft').css('height',$('.right_body').height());
		    })
		    //切换收起展开
		    $('.scale').off('click').on('click',function(){
		        var curImg = $(this).find('img').attr('src');
		        if(curImg == '/images/arrow_02.png'){//收起
		            $(this).html('<span>查看</span> <img src="/images/arrow_01.png">')
		            $('.rightMain .wx_form').addClass('hide');
		        }else{
		            $(this).html('<span>收起</span> <img src="/images/arrow_02.png">')
		            $('.rightMain .wx_form').removeClass('hide');
		        }
		        // $('#navLeft').css('height',$('.right_body').height()+48);
		        $('.wx_type li').eq(0).click();
		        
		    })
		    //微信二维码显示
			$('.noBigPic').off('mouseover').on('mouseover',function(){
		        $(this).removeClass('noBigPic');
		    }).off('mouseout').on('mouseout',function(){
		        $(this).addClass('noBigPic');
		    })

		    //删除已上传物料
		    $('.delIt').off('click').on('click',function(){
		    	$(this).parent('li').remove();
		    })

		    //用户输入时，提示信息消失
		    $('#promoteName').off('focus').on('focus',function(){
		    	$(this).parents('ul').find('.tipInfo').html('').hide();
		    })
		    $('#promoteDemand').off('focus').on('focus',function(){
		    	$(this).parents('ul').find('.tipInfo').html('').hide();
		    })
		    $('#budget').off('focus').on('focus',function(){
		    	$(this).parents('ul').find('.tipInfo').html('').hide();
		    })
		    $('.chooseList').off('change').on('change','input',function(){
		    	$('.chooseList').parents('ul').find('.tipInfo').html('').hide();
		    })

		    $('.help').off('click').on('click',function(){
		    	$.openPopupLayer({
		            name: "Layer",
		            url: "demandLayer.html",
		            error: function (dd) {
		                alert(dd.status);
		            },
		            success: function () {
		            	$('#closebt').off('click').on('click',function(){
		            		$.closePopupLayer('Layer');
		            	})
		            }
		        })
		    })


		    //提交订单
		    $('#submmit_order').off('click').on('click',function(){
		    	var finalData = _self.formValidation();
		    	if(finalData.errorInfo.length == 0){
		    		setAjax({
		    			// url:'json/success.json',
		    			url:public_url+'/api/SmartSearch/AddSmartSearchInfo',
		    			type:'post',//后期改成post
		    			data:finalData.upData
		    		},function(data){
		    			if(data.Status == 0){
		    				window.location = 'shoppingCart03.html?RecID='+data.Result.ReturnID;
		    			}else{
		    				layer.msg(data.Message);
		    			}
		    		})	
		    	}
		    })
		},
		formValidation:function(){//验证
			var _self = this;
			var upData = _self.getFormInfo();
			var $nameTip = $('#promoteName').parents('ul').find('.tipInfo'),
				$demand = $('#promoteDemand').parents('ul').find('.tipInfo'),
				$purpose = $('.chooseList').parents('ul').find('.tipInfo'),
				$budget = $('#budget').parents('ul').find('.tipInfo'),
				$time = $('#beginTime').parents('ul').find('.tipInfo'),
				errorInfo = [];

			if( !upData.Name ){
				$nameTip.html('<img src="/images/icon7.png"> 请输入推广名称').show();
				errorInfo.push(1);
			}else if( upData.Name.length > 20){
				$nameTip.html('<img src="/images/icon7.png"> 推广名称最多可输入20个汉字').show();
				errorInfo.push(1);
			}else{
				 // 判断推广计划名称在该广告主该类推广计划下是否存在，如存在提示“推广名称不能重复！
			}

			if( !upData.Demand ){
				$demand.html('<img src="/images/icon7.png"> 请输入推广需求').show();
				errorInfo.push(1);
			}else if(upData.Demand.length < 20 ){
				$demand.html('<img src="/images/icon7.png"> 推广需求最少需输入20个汉字').show();
				errorInfo.push(1);
			}else if(upData.Demand.length > 2000 ){
				$demand.html('<img src="/images/icon7.png"> 推广需求最多可输入2000个汉字').show();
				errorInfo.push(1);
			}

			if($('.chooseList').find('input:checked').length == 0){
				$purpose.html('<img src="/images/icon7.png"> 请选择推广目的').show();
				errorInfo.push(1);
			}
			if( !upData.BudgetPrice ){
				$budget.html('<img src="/images/icon7.png"> 请输入推广预算').show();
				errorInfo.push(1);
			}else if( upData.BudgetPrice < 3000 ){
				$budget.html('<img src="/images/icon7.png"> 推广预算最低为3000元').show();
				errorInfo.push(1);
			}else if( upData.BudgetPrice.length > 8){
				$budget.html('<img src="/images/icon7.png"> 推广金额格式不正确').show();
				errorInfo.push(1);
			}

			if( !$('.total_num').html() ){
				layer.msg('媒体/账号丢失，请重新选择账号/媒体',{time:1000})
				errorInfo.push(1);
			}

			if( !upData.BeginTime || !upData.EndTime ){
				$time.html('<img src="/images/icon7.png"> 请选择推广时间').show();
				errorInfo.push(1);
			}
			return {
				errorInfo:errorInfo,
				upData:upData
			}
		},
		getFormInfo:function(){//获取信息
			var Purposes = 0,
				MaterialUrl = '',
				MaterialUrlName = '';
			if($('.chooseList').find('input:checked').length){
				$('.chooseList').find('input:checked').each(function(i,item){
					Purposes = Purposes | $(item).attr('dict');
				})
			}
			if($('.chooseName').length){
				MaterialUrl = $('.downLoad').attr('href');
				MaterialUrlName = $.trim($('.chooseName').html());
			}
			return{
				Name : $.trim($('#promoteName').val()),//推广名称
				Demand : $.trim($('#promoteDemand').val()),//推广需求
				Purposes : Purposes,//推广目的
				BudgetPrice : $.trim($('#budget').val()),//推广预算
				BeginTime : $.trim($('#beginTime').val()),//开始时间
				EndTime : $.trim($('#endTime').val()),//结束时间
				MaterialUrl : MaterialUrl,//物料路径，非必填
				MaterialUrlName:MaterialUrlName
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
		},
		uploadFile:function(id) {
			var _self = this;
	        jQuery.extend({
	            evalJSON: function (strJson) {
	                if ($.trim(strJson) == '')
	                    return '';
	                else
	                    return eval("(" + strJson + ")");
	            }
	        });
	        function getCookie(name) {
	            var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
	            if (arr = document.cookie.match(reg))
	                return unescape(arr[2]);
	            else
	                return null;
	        };
	        function escapeStr(str) {
	            return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
	        };
	        $('#'+id).uploadify({
	        	"queueID": "imgShow", 
	            'buttonText': '上传',
	            'buttonClass': 'but_upload',
	            'swf': '/Js/uploadify.swf?_=' + Math.random(),
	            'uploader': public_url+'/AjaxServers/UploadFile.ashx',
	            'auto': true,
	            'multi': false,
	            'width': 80,
	            'height': 20,
	            'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-ouinfo')) },
	            'fileTypeDesc': '支持格式:zip,rar',
                'fileTypeExts': '*.zip;*.rar;',
	            'fileSizeLimit':'5MB',
	            'queueSizeLimit': 1,
	            'scriptAccess': 'always',
	            'onQueueComplete': function (event, data) {
	                //enableConfirmBtn();
	            },
	            'onQueueFull': function () {
	                layer.alert('您最多只能上传1个文件！');
	                return false;
	            },
	            'onUploadSuccess': function (file, data, response) {
	                if (response == true) {
	                    var json = $.evalJSON(data);
	                    var fileCount = $('#'+id).parents('ul').find('.chooseName').length;
	                    //如果未上传过
	                    if( !fileCount){
		                    $('#'+id).parent('li').after('<li><span class="chooseName">'+json.FileName.substr(0,20)+'</span><i class="delIt">×</i>'+
	                    '<a href='+json.Msg+' class="downLoad"></a></li>');
		                    //删除已上传物料
						    $('.delIt').off('click').on('click',function(){
						    	$(this).parent('li').remove();
						    })
	                    }else{
	                    	$('#'+id).parent('li').next().find('.chooseName').html(json.FileName).find('a').attr('src',json.Msg);
	                    }
	                }
	            },
	            'onProgress': function (event, queueID, fileObj, data) {},
	            'onUploadError': function (event, queueID, fileObj, errorObj) {
	                console.log(errorObj);
	                //enableConfirmBtn();
	            },
	            'onSelectError':function(file, errorCode, errorMsg){
	                console.log(errorCode);
	                if(errorCode == '-110'){
                    	layer.msg('文件大小需小于5M',{time:2000});
                    }
	            }
	        });
	    }
	}
	new TableList();
})