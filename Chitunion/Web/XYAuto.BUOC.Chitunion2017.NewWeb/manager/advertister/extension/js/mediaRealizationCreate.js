/*
* Written by:     yangyakun
* function:       媒体智投添加
* Created Date:   2017-12-20
*/
$(function(){
   
    //判断是否登陆  以及权限
    var gourl=encodeURI(public_url+'/manager/advertister/extension/mediaRealizationCreate.html');
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

    var spaceArr = [];
    var arr = [];
    var num = 0;
	//上传图片
	uploadImg('uploadFile')
	function uploadImg(id) {
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
                // 'fileTypeDesc': '支持格式:jpg,jpeg,png',
                //     'fileTypeExts': '*.jpg;*.jpeg;*.png;',
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
                        console.log(json);
                        var fileCount = $('#'+id).parents('ul').find('.chooseName').length;
                        //如果未上传过
                        if( !fileCount){
                            $('#'+id).parent('li').after('<li><span class="chooseName">'+json.FileName.substr(0,10)+'</span><i class="delIt">×</i>'+
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
                }
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

    //删除已上传物料
    $('.delIt').off('click').on('click',function(){
        $(this).parent('li').remove();
    })
  //创建媒体智投推广
	$('.button_gradient').click(function(){//点击提交进行验证
		var val_name = $.trim($('.generalize_name').val()),//推广名称
			val_intro = $.trim($('.generalize_intro').val()),//其它说明
			val_budget = $.trim($('.generalize_budget').val()),//推广预算
			val_start = $.trim($('#startDate').val()),//推广时间
			val_end = $.trim($('#endDate').val()),
            materialurl = '',
            AreaList = [],
            CarList = [],
			Buseiness = new Object();
            if($('#material').find('.chooseName').length){
                materialurl = $('#material').find('a').attr('href');
            }
            console.log($('#city_choice').find('.chooseName').length)
            for(var i = 0;i<$('#city_choice').find('.chooseName').length;i++){
                var ProvinceID = $('#city_choice').find('.chooseName').eq(i).attr('ProvinceID'),
                    CityID = $('#city_choice').find('.chooseName').eq(i).attr('CityID');
                    AreaList.push({
                        ProvinceID : ProvinceID,
                        CityID : CityID
                    })
            }
            for(var i = 0;i<$('#car_choice').find('.chooseName').length;i++){
                var MakeID = $('#car_choice').find('.chooseName').eq(i).attr('makeID'),
                    ModelID = $('#car_choice').find('.chooseName').eq(i).attr('ModelID');
                    CarList.push({
                        MakeID : MakeID,
                        ModelID : ModelID
                    })
            }
            console.log(CarList)
			Buseiness = {
				Name : val_name,
                AreaList :AreaList, 
                CarList :CarList,
				Remark : val_intro,
				BudgetPrice : val_budget,
				BeginTime : val_start,
				EndTime : val_end,
                Materialurl : materialurl
			};
            console.log(Buseiness)
			if(val_name.length<=0){//判断推广名称
				$('.notes_name').show()
				$('.notes_name').html('请输入推广名称！')
			}else if(val_name.length>20){
		      $('.notes_name').html('推广名称最多可输入20个汉字！')
			}else{
				$('.notes_name').hide()
			}
            if($('#checkArea').find('.chooseName').length){

            }
			if(val_intro.length<0){//判断其它说明
			
			}else if(val_intro.length>2000){
				$('.notes_intro').show()
				$('.notes_intro').html('推广简介最多可输入2000个汉字！')
			}else{
				$('.notes_intro').hide()
			}
            // if(materialurl==""){
            //     $('.notes_material').show()
            //     $('.notes_material').html('请上传物料')
            // }
			if(val_budget==''){
				$('.notes_budget').show()
				$('.notes_budget').html('请输入推广预算')
			}else if(val_budget.length>8){
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
            }else{
                $('.notes_time').hide();
            }
			if(val_name.length && val_intro.length<=2000 && val_budget && val_start && val_end){
				//调接口传输json				
				setAjax({
		        		url:public_url+'/api/MediaPromotion/AddMediaPromotionInfo',
		        		// url:'./json/mediaRealizationCreate.json',
		        		type:'post',
		        		data:Buseiness
		        		},function(data){
		        			if(data.Status == 0){
		        				layer.msg('保存成功',{time:2000});
                                window.location=public_url+'/manager/advertister/extension/mediaRealizationList.html';
		        			}else{
		        				layer.msg(data.Message,{time:2000})
		        			}
		        		})
			}
	})

	//点击选择车型出现弹层  推广车型 
	$('#choseType').off('click').on('click',function(){
	   // 弹层
      	$.openPopupLayer({
            name: "motorcycletype",
            url: "motorcycletype.html",
            error: function (dd) {
                    alert(dd.Status);
                },
            success: function () {
               	// 关闭选择分类弹窗
                $('#closebt').off('click').on('click', function () {
                    $.closePopupLayer('motorcycletype')
                        })
                $('#car_choice').on('click','.delIt',function(){
                        $(this).parent('li').remove()
                    })
                    var val = this.value,
                        strBrand = '',
                        strSub = '',
                        searchStr = '';
                        var num = 0;
                	setAjax({
                        url:public_url+'/api/CarSerial/QueryBrand',
	                    // url : './json/brand.json',
	                    type:'get'
		                },function(data){
		                    if(data.Status == 0){
                                for(var i = 0;i<data.Result.length;i++){
                                    strBrand+='<li class="brand" style="width:100%"><a href="#'+(Number(data.Result[i].MasterBrandID))+'">'+data.Result[i].MasterBrandName+'</li>'
                                    strSub += '<li id="'+(Number(data.Result[i].MasterBrandID))+'"><ul>'
                                    for(var j = 0;j<data.Result[i].carBrandList.length;j++){
                                        strSub+='<li style="width:100%;border-top:1px solid #ccc;"><div class="sub_brand"><input type="checkbox" class="all city" MakeID="'+data.Result[i].carBrandList[j].BrandId+'" >'+data.Result[i].carBrandList[j].BrandName+'</div><div class="sub_model">'// 右侧的列表
                                        for(var k = 0;k<data.Result[i].carBrandList[j].carSerialList.length;k++){
                                            strSub+='<div style="float:left;padding:1% 5% 1% 0;"><input type="checkbox" class="carSerial city" ModelID="'+data.Result[i].carBrandList[j].carSerialList[k].CarSerialId +'">' + data.Result[i].carBrandList[j].carSerialList[k].ShowName + '</div>'
                                        }
                                        strSub+='</div></li>'
                                    }
                                    strSub += '</ul></li>'
                                }
                            }

                            $('#checkBrand').html(strBrand)
                            $('.series').html(strSub)
                              for(var i = 0;i<$('#car_choice li').length;i++){
                                for(var j = 0;j<$('.series .sub_model input').length;j++){
                                    if($('#car_choice li').eq(i).find('span').attr('modelid')==$($('.series .sub_model input')[j]).attr('modelid')){
                                        $($('.series .sub_model input')[j]).prop('checked',true)
                                    }
                                }
                            }
                            //当子品牌被选中的时候，车型也被选中
                            $('.all').change(function () {
                                var checked = $(this).prop('checked')
                                // 当子品牌被选中的时候，下面的车型也就被选中
                                    if (checked) {
                                        $(this).parent('.sub_brand').next('.sub_model').find('.carSerial').prop('checked', true)
                                    }else{
                                        $(this).parent('.sub_brand').next('.sub_model').find('.carSerial').prop('checked', false)
                                    }
                            })
                            // 当车型全部选中的时候，那个子品牌被选中
                            $('.sub_model').each(function(){
                                var check = $(this).find('.carSerial')
                                var value = $(this).parent('div').text()
                                check.change(function(){
                                    var bool = isAllChecked(check)
                                    var allCheck = $(this).parents('.sub_model').prev().find('.all')
                                    if (bool) {
                                        allCheck.prop('checked', true)
                                    } else {
                                        allCheck.prop('checked', false)
                                    }
                                })
                            })
                            //搜索框
                            $('.search_inpBrand').keyup(function () {
                                var val = $(this).val()
                                if (val != '') {
                                    $('.show').show()
                                    $('#checkBrand li').each(function(){
                                        if ($(this).text().indexOf(val) != -1) {
                                            var id = $(this).find('a').attr('href')
                                            searchStr += '<p><a href="'+id+'">'+$(this).text()+'</a></p>'
                                        }
                                    })
                                    $('.show').html(searchStr)
                                    searchStr = ''
                                }else{
                                    $('.show').hide()
                                }
                            })
                            //提交
                            $('#submitMessage').click(function(){
                               var str=''
                                if ($('.series .sub_model input:checked').length>0&&$('.series .sub_model input:checked').length <= 20) {
                                    for(var i = 0;i<$('.series .sub_model input:checked').length;i++){
                                        var makeid = $($('.series .sub_model input:checked')[i]).parents('.sub_model').prev().find('input').attr('makeid')  ,
                                            modelid = $($('.series .sub_model input:checked')[i]).attr('modelid'),
                                            makeidName =$($('.series .sub_model input:checked')[i]).parents('.sub_model').prev().text(),
                                            modelidName =$($('.series .sub_model input:checked')[i]).parent('div').text();  
                                            str+='<li><span class="chooseName" MakeId="'+makeid+'" ModelId="'+modelid+'">'+makeidName+'-'+modelidName+'</span><i class="delIt">×</i></li>'
                                        }
                                        $('#car_choice').html(str)
                                        }
                                if ($('.series .sub_model input:checked').length > 20) {
                                    layer.msg('您最多可选择20个车型',{'time':2000})
                                } else if ($('.series .sub_model input:checked').length == 0){
                                    layer.msg('请选择车型',{'time':2000})
                                }else{
                                    $.closePopupLayer('motorcycletype')
                                }
                             })




                            function isAllChecked(check) {
                                var allChecked = true
                                check.each(function () {
                                    if (!$(this).prop('checked')) {
                                        allChecked = false
                                    }
                                })
                                return allChecked;
                            }


		                  }
                        )
            }
        })		
	})
	//点击选择城市出现弹层  推广城市
	$('#choseCity').off('click').on('click',function(){
        //弹层
        $.openPopupLayer({
            name: "cityLayer",
            url: "cityLayer.html",
            error: function (dd) {
                    alert(dd.Status);
                },
            success: function () {
                // 关闭选择分类弹窗
                $('#closebt').off('click').on('click', function () {
                    $.closePopupLayer('cityLayer')
                        })
                    city()
                    //点击删除
                    $('#city_choice').on('click','.delIt',function(){
                        $(this).parent('li').remove()

                    })
                    for(var i = 0;i<$('#city_choice li').length;i++){
                        for(var j = 0;j<$('.series .sub_model input').length;j++){
                            if($('#city_choice li').eq(i).find('span').attr('cityid')==$($('.series .sub_model input')[j]).attr('CityID')){
                                if($($('.series .sub_model input')[j]).attr('CityID')==0){
                                    $($($('.series .sub_model input')[j]).parents('.sub_model').prev('.sub_brand').find('input').prop('checked',true))
                                    $('.city').prop('disabled', true)
                                }
                                $($('.series .sub_model input')[j]).attr('checked','checked')
                            }
                        }
                    }

                    
            }
        })
    })
    //城市
    function city () {
            // 渲染城市
            var data = JSonData.masterArea
            var arrData = [0];
            var letter = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
            var searchStr = '';
            var strBrand = '<li class="brand"><a href="#0">全国</a></li>',
                strAll = '<li id="0"><div class="sub_brand"><input type="checkbox" class="all" ProvinceID="0">全国</div><div class="sub_model"><ol><li><input type="checkbox" class="whole check" CityID="0">全国</li></ol></div></li>';
            // 根据字母的顺序进行排列
            for (var j = 0; j < letter.length; j++) {
                for (var i in data) {
                    var strSub = '<li id="'+(Number(i)+1)+'"><div class="sub_brand"><input type="checkbox" class="all city" ProvinceID="' + data[i].id + '">' // 右侧的列表
                    if (letter[j] == data[i].szm) {
                        strBrand += '<li class="brand" style="width:100%"><a href="#'+(Number(i)+1)+'">' + data[i].name + '</li>'; // 左侧的列表 
                        strSub += data[i].name + '</div><div class="sub_model"><ol>';
                        if (data[i].subArea) {// 渲染城市列表
                            var subArea = data[i].subArea;
                            for (var k=0;k<subArea.length;k++) {
                                strSub += '<li><input type="checkbox" class="city check" CityID="' + subArea[k].id + '">' + subArea[k].name + '</li>'
                            }
                            strSub += '</ol></div></li>'
                        }
                        strAll += strSub
                        // console.log(subArea[k].id,data[i].id)

                    }
                }
            }
            $('.list').html(strBrand)
            $('.series').html(strAll)
            $.each(spaceArr,function(i,v){
                if (v == "全国") {
                    $('input').prop('checked', false)
                }
            })
            $('.all').change(function () {
                var value = $(this).parent().text()
                var checked = $(this).prop('checked')
                // 选择全国禁用其它的选项
                if (value == '全国') {
                    if (checked) {
                        $('.whole').prop('checked', true)
                        $('.city').prop('disabled', true)
                        $('.city').prop('checked', false)
                    } else {
                        $('.whole').prop('checked', false)
                        $('.city').attr('disabled', false)
                    }
                } else { // 当省份被选中的时候，下面的城市也就被选中
                    if (checked) {
                        $(this).parents('li').find('.sub_model').find('.check').prop('checked', true)
                    } else {
                        $(this).parents('li').find('.sub_model').find('.check').prop('checked', false)
                    }
                }
            })
            // 当城市全部选中的时候，那个省份被选中
            $('.sub_model').each(function(){
                var check = $(this).find('.check')
                var value = $(this).text()
                check.change(function(){
                    var bool = isAllChecked(check)
                    var allCheck = $(this).parents('li').find('.all')
                    if (bool) {
                        allCheck.prop('checked', true)
                    } else {
                        allCheck.prop('checked', false)
                    }
                    if (value == '全国') {
                        var able = $(this).prop('checked')
                        $('.city').attr('disabled', able)
                    } 
                })
            })
            $('.search_inpBrand').keyup(function () {
                var val = $(this).val()
                if (val != '') {
                    $('.show').show()
                    $('.list li').each(function(){
                        if ($(this).text().indexOf(val) != -1) {
                            var id = $(this).find('a').attr('href')
                            searchStr += '<p><a href="'+id+'">'+$(this).text()+'</a></p>'
                            // window.location.href = id
                        }
                    })
                    // console.log(searchStr)
                    $('.show').html(searchStr)
                    searchStr = ''
                }else{
                    $('.show').hide()
                }
            })

            //提交
            $('#submitMessage').click(function(){
                var str=''
                if ($('.series .sub_model input:checked').length>0&&$('.series .sub_model input:checked').length<=20) {
                    for(var i = 0;i<$('.series .sub_model input:checked').length;i++){
                        var ProvinceID = $($('.series .sub_model input:checked')[i]).parents('.sub_model').prev().find('input').attr('ProvinceID')  ,
                            CityID = $($('.series .sub_model input:checked')[i]).attr('CityID'),
                            city =$($('.series .sub_model input:checked')[i]).parent('li').text(),
                            provice =$($('.series .sub_model input:checked')[i]).parents('.sub_model').prev().text();  
                            str+='<li><span class="chooseName" ProvinceID="'+ProvinceID+'" CityID="'+CityID+'">'+provice+'-'+city+'</span><i class="delIt">×</i></li>'
                        }
                        $('#city_choice').html(str)
                        }
                if ($('.series .sub_model input:checked').length > 20) {
                    layer.msg('您最多可选择20个城市',{'time':2000})
                    return false;
                } else if ($('.series .sub_model input:checked').length == 0){
                    layer.msg('请选择城市',{'time':2000})
                }else{
                    $.closePopupLayer('cityLayer')
                }
            })
            function isAllChecked(check) {
                var allChecked = true
                check.each(function () {
                    if (!$(this).prop('checked')) {
                        allChecked = false
                    }
                })
                return allChecked;
            }
            function mouseOver(lis, ipt, ul) {
                for (var i = 0; i < lis.length; i++) {
                    lis[i].onmouseover = function () {
                        this.style.background = 'skyblue';
                    }
                    lis[i].onmouseout = function () {
                        this.style.background = '';
                    }
                    lis[i].onclick = function () {
                        ipt.value = this.innerHTML;
                        ul.style.display = 'none';
                    }
                }
            }
    }



})