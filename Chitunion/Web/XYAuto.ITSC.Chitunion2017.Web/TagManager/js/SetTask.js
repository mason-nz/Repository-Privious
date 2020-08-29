/*
* Written by:     wangcan
* function:       设置任务
* Created Date:   2017-08-08
* Modified Date:
*/
/*
1、


*/
$(function(){
	//项目名称：失去焦点时，判断是否为空
	$('#Name').off('blur').on('blur',function(){
		var val = $.trim($(this).val());
		if(!val){
			$('#Name').parents('ul').find('.tipInfo').show();
		}else{
			$('#Name').parents('ul').find('.tipInfo').hide();
		}
	})
	/*任务数量：就是指一项目中最大的任务数量；数值必须>0且 <=10000否则不可输入；

	当未输入时值时，点保存条件按钮，系统提示“本次任务数据量不能为空”；

	当小于1时，则显示为1,大于10000时，则显示为100000
	*/
	$('#TaskCount').off('blur').on('blur',function(){
		var _this = $(this);
		var val = _this.val();
		if(val){
			$('#TaskCount').parents('ul').find('.tipInfo').hide();
			if(val<1){
				_this.val('1');
			}else if( val >10000){
				_this.val('10000');
			}
		}
	}).off('input').on('input',function(){
		replaceAndSetPos(this,/[^0-9]/g,'')
	})


	//文件上传
	uploadFile('uploadify');
	$('#submit').off('click').on('click',function(){
		var upData = {
			Name : $.trim($('#Name').val()),
			ProjectType : $('#projectType input:checked').attr('value')-0,
			TaskCount : $.trim($('#TaskCount').val())-0,
			UploadFileURL : $('#uploadify').parents('ul').next().find('#imgDownURL').attr("href")
		}
		var tipInfo = [];
		if( !upData.Name ){
			$('#Name').parents('ul').find('.tipInfo').show();
			tipInfo.push('名称不能为空');
		}
		if( upData.UploadFileURL == 'javascript:;' ){
			$('#imgUploadFile').parents('ul').find('.tipInfo').show();
			tipInfo.push('未上传文件');
		}
		if( !upData.TaskCount ){
			$('#TaskCount').parents('ul').find('.tipInfo').show();
			tipInfo.push('任务数量不能为空');
		}
		if(tipInfo.length == 0 && $('#submit').css('background-color') == 'rgb(255, 79, 79)'){
            $.ajax({
                url:'http://www.chitunion.com/api/LabelTask/LabelProjectCreate',//  json/info.json
                type:'post',
                data: upData,
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                dataType:"json",
                beforeSend: function () {
                    $('#submit').css('background-color','rgb(102, 102, 102)');
                    layer.msg('保存中', {
                        icon: 16,
                        shade: 0.01,
                        time: 2000 //2秒关闭（如果不配置，默认是3秒）
                    }, function(){
                        //do something
                    });
                },
                success: function (data){
                    if(data.Status == 0){
                        layer.msg('保存成功', {
                            time: 1000 //2秒关闭（如果不配置，默认是3秒）
                        }, function(){
                           window.location = '/TagManager/TaskManageList.html';
                        });
                    }else{
                        layer.msg(data.Message);
                        $('#submit').css('background-color','#FF4F4F');
                    }
                }
            })
			// setAjax({
			// 	url:'/api/LabelTask/LabelProjectCreate',//  json/info.json
			// 	type:'post',
			// 	data:upData
			// },function(data){
			// 	if(data.Status == 0){
			// 		$('#hideTips').html('保存成功').show();
			// 		setTimeout(function(){window.location = '/TagManager/TaskManageList.html';},1000);
			// 	}else{
   //                  $('#hideTips').html(data.Message).show();
   //                  setTimeout(function(){$('#hideTips').hide();},1000);
			// 	}
			// })
		}
	})
	/*上传附件*/
    function uploadFile(id) {
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
            'buttonText': '+ 上传账号信息',
            'buttonClass': 'but_upload',
            'swf': '/Js/uploadify.swf?_=' + Math.random(),
            'uploader': 'http://www.chitunion.com/AjaxServers/UploadFile.ashx',
            'auto': true,
            'multi': false,
            'width': 200,
            'height': 35,
            'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
            'fileTypeDesc': '支持格式:xlsx',
            'fileTypeExts': '*.xlsx',
            'fileSizeLimit':'10MB',
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
                    $('#'+id).parents('ul').next().show();
                    $('#'+id).parents('ul').next().find('#imgName').text(json.FileName);
                    $('#'+id).parents('ul').next().find('#imgDownURL').attr("href","http://www.chitunion.com" + json.Msg);
                    $('.imgMsg').html('上传成功！');
                    $('#imgUploadFile').parents('ul').find('.tipInfo').hide();
                    $.ajax({
                    	url:'http://www.chitunion.com/api/LabelTask/LabelStatistics',//   json/LabelStatistics.json
                    	type:'get',
                    	dataType: 'json',
		                async: false,
		                xhrFields: {
		                    withCredentials: true
		                },
		                crossDomain: true,
                    	data:{
                    		projectType : $('#projectType input:checked').attr('value'),
                    		uploadFileURL : "http://www.chitunion.com" + json.Msg
                    	},
                    	beforeSend: function () {
					        
				      	},	
                    	success:function(data){
                            if(data.Status == 0){
                    		  $('#StatisticResult').show().find('#count').html(formatMoney(data.Result,0,""));
                            }else{
                                layer.msg(data.Message);
                            }
                    	}
                    })
                }
            },
            'onProgress': function (event, queueID, fileObj, data) {
		        
            },
            'onUploadError': function (event, queueID, fileObj, errorObj) {
                console.log(errorObj);
                //enableConfirmBtn();
            },
            'onSelectError':function(file, errorCode, errorMsg){
                console.log(errorCode);
            }
        });

    };

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
})