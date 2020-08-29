/*
* Written by:     yangyk
* function:       创建内容分发20
* Created Date:   2017-12-20
*/
$(function(){
	
    /* 新增内容分发*/
    //判断是否登陆  以及权限
    var gourl = encodeURI(public_url+'/manager/advertister/extension/distributeGeneralizeCreate.html');
        if(CTLogin.IsLogin){
            if(CTLogin.Category==29001){ 
                
            }else{
                layer.msg('您不是广告主,请登录!',{time:2000},function(){
                    window.location = '/Exit.aspx?gourl='+gourl;
                })
            }
        }else{
            layer.msg('您尚未登录,请登录!',{time:2000},function(){
                    window.location = '/login.aspx?gourl='+gourl;
                })
        }
	//上传图片
	uploadImg('uploadImg')
 	function uploadImg(id){
            /**
            * @desc  JQuery扩展，将json字符串转换为对象，需要引用类库JQuery
            * @param   json字符串
            * @return 返回object,array,string等对象
            * @Add=Masj, Date: 2009-12-07
            */
            jQuery.extend(
             {
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
            }
            function escapeStr(str) {
                return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
            }

            $(document).ready(function () {
                $('#'+id).uploadify({
                    "buttonText": "",
                    "buttonClass": "but_upload",
                    "swf": "/Js/uploadify.swf?_=" + Math.random(),
                    "uploader": "/AjaxServers/UploadFile.ashx",
                    "auto": true,
                    "multi": false,
                    "width": 260,
                    "height": 104,
                    "formData": { Action: "BatchImport", CarType: "", LoginCookiesContent: escapeStr(getCookie("ct-ouinfo")), IsGenSmallImage: 1 },
                    "fileTypeDesc": "支持格式:,jpg,png",
                    "fileTypeExts": "*.jpg;*.png;",
                    "queueSizeLimit": 1,
                    "fileSizeLimit": "5MB",
                    "scriptAccess": "always",
                    //"overrideEvents": ["onDialogClose"],
                    "queueID": "imgShow",
                    "onUploadStart": function (file) {
                        //console.log(file);
                        if ("*.jpg;*.png;".indexOf(file.type) <= 0) {
                            alert("推广图片格式不正确!");
                            this.cancelUpload(file.id);
                            $("#" + file.id).remove();
                        }
                    },
                    "onQueueComplete": function (event, data) {
                        //enableConfirmBtn();
                    },
                    "onQueueFull": function () {
                        alert("您最多只能上传1个文件！");
                        return false;
                    },
                    'onFallback':function(){
                        console.log('您未安装FLASH控件，无法上传图片！');
                        return false;
                    },
                    "onUploadSuccess": function (file, data, response) {
                        $('#uploadify').css({'position':'absolute','top':'-50px',"left":'-10px'})
                        if (response == true) {
                            var json = $.evalJSON(data);
                            console.log(json);
                            var array = json.Msg.split("|");
                            for (var i = 0; i < array.length; i++) {
                                if (i > 0) {
                                    $("#imgUploadFile").after($("<img>").attr("src", "" + array[i]));
                                }
                                else {
                                    $("#imgUploadFile").attr("src", "" + array[i]);
                                }
                            }
                            $('.uploadFile>img').hide()
                        }
                    },
                    "onProgress": function (event, queueID, fileObj, data) { },
                    "onUploadError": function (event, queueID, fileObj, errorObj) {
                        //enableConfirmBtn();
                    }
                });
            });
 	}
    //推广预算只能是数字
    limitNumber('.generalize_budget')
    //限制数字
    function limitNumber (ele){//限制只能输入数字
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
    //推广时间
    $('#startDate').off('click').on('click', function () {
        laydate({
            fixed: false,
            elem: '#startDate',
            min:laydate.now(+3),
                istoday:false,
                choose: function (date) {
                if (date > $('#endDate').val() && $('#endDate').val()) {
                    layer.alert('结束日期必须大于开始日期！');
                    $('#startDate').val('')
                }else if($('#startDate').val()==''){
                     $('.notes_time').html('请选择推广时间！')
                }
            }
        });
    });
    $('#endDate').off('click').on('click', function () {
        laydate({
            fixed: false,
            elem: '#endDate',
            min:laydate.now(+3),
            choose: function (date) {
                if (date < $('#startDate').val() && $('#startDate').val()) {
                    layer.alert('结束日期必须大于开始日期！');
                    $('#endDate').val('')
                }else if($('#endDate').val()==''){
                    $('.notes_time').html('请选择推广时间！')
                }
            }
        });
    });
	$('.button_gradient').click(function(){//点击提交进行验证
		var val_name = $.trim($('.generalize_name').val()),//推广名称
			val_url = $.trim($('.generalize_link').val()),//推广链接
			val_intro = $.trim($('.generalize_intro').val()),//推广简介
			val_pic =$('#imgUploadFile').attr('src'),//推广图片
			val_platform = $('.generalize_platform').find('input:checked').length,//推广平台
			BillingMode = $('.billing_model').find('input:checked').attr('value')-0,//计费模式
			val_budget = $.trim($('.generalize_budget').val()),//推广预算
			val_start = $.trim($('#startDate').val()),//推广时间
			val_end = $.trim($('#endDate').val()),
			platform = 0,
			Buseiness = new Object();
			if($('.generalize_platform').find('input:checked').length){
				$('.generalize_platform').find('input:checked').each(function(i,item){
					platform = platform | $(item).attr('id');
				})
			}
			Buseiness = {
				Name : val_name,
				Link : val_url,
				Synopsis : val_intro,
				BudgetPrice : val_budget,
				BeginTime : val_start,
				EndTime : val_end,
				ImgUrl : val_pic,
				platform :platform,
				BillingMode :BillingMode	
			};
			console.log(Buseiness)
			if(val_name.length<=0){//判断推广名称
				$('.notes_name').show()
				$('.notes_name').html('请输入推广名称！')
			}else if(val_name.length>20){
				$('.notes_name').show()
				$('.notes_name').html('推广名称最多可输入20个汉字！')
			}else{
				$('.notes_name').hide()
			}
			if(val_url.length<=0){//判断推广链接
				$('.notes_url').show()
				$('.notes_url').html('请输入推广链接！')
			}else if(!val_url.match(/(https|http):\/\/.+/) || val_url.length>255){
				$('.notes_url').show()
				$('.notes_url').html('推广链接格式不正确！')
			}else{
				$('.notes_url').hide()
			}
			if(val_intro.length<=0){//判断推广简介
				$('.notes_intro').show()
				$('.notes_intro').html('请输入推广简介！')
			}else if(val_intro.length>2000){
				$('.notes_intro').show()
				$('.notes_intro').html('推广简介最多可输入2000个汉字！')
			}else if(val_intro.length<20){
				$('.notes_intro').show()
				$('.notes_intro').html('推广简介最少需输入20个汉字！')
			}else{
				$('.notes_intro').hide()
			}
            if(val_pic==""){
                $('.notes_pic').show()
                $('.notes_pic').html('请上传图片！')
            }else{
                $('.notes_pic').hide()
            }
			if($('.generalize_platform input[type=checkbox]:checked').length<=0){//判断推广平台
				$('.notes_platform').show()
				$('.notes_platform').html('请选择推广平台！')
			}else{
				$('.notes_platform').hide()
			}
			if($('.billing_model input[type=radio]:checked').length<=0){//判断计费模式
				$('.notes_billing').show()
				$('.notes_billing').html('请选择计费模式！')
			}else{
				$('.notes_billing').hide()
			}
			if(val_budget==''){//判断推广预算
				$('.notes_budget').show()
				$('.notes_budget').html('请输入推广预算')
			}else if(val_budget*1>=1.7976931348623157E+308){
				$('.notes_budget').show()
				$('.notes_budget').html('推广预算格式不正确')
			}
			else if((val_budget*1<3000)){
				$('.notes_budget').show()
				$('.notes_budget').html('推广预算最低为3000')
			}else{
				$('.notes_budget').hide()
			}
			if(!val_start && !val_end){
                $('.notes_time').show()
                $('.notes_time').html('请输入推广时间')
            }
			if(val_name.length && val_url.length && val_intro.length && $('.generalize_platform input[type=checkbox]:checked').length && $('.billing_model input[type=radio]:checked').length && val_budget && val_start && val_end ){
				//调接口传输json				
				setAjax({
		        		url:public_url+'/api/ContentDistribute/AddContentDistributeInfo',
		        		// url:'./json/distributeGenralizeadd.json',
		        		type:'post',
		        		data:Buseiness
		        		},function(data){
		        			if(data.Status == 0){
		        				layer.msg('保存成功',{time:2000});
		        				window.location=public_url+'/manager/advertister/extension/distributeGeneralizelist.html';
		        			}else{
		        				layer.msg(data.Message,{time:2000})
		        			}
		        		})
			}		
	})
})