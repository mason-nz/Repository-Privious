$(function () {
    function ToExamine() {
        this.right();
    }
    ToExamine.prototype = {
        constructor: ToExamine,
        left: function (_this) {
            var _this=_this;
             setAjax({
		    	url:'json/mediaInfo.json',
		    	type:'get',
		    	data:{
		    		TaskID:GetRequest().TaskID
		    	}
		    },function(data){
		    	var result = {};
		    	if(data.Status == 0){
		    		result = data;
				    $.ajax({
                        url: 'http://www.chitunion.com/api/LabelTask/SelectLableInfo',//分类:65001，场景，65002：IP :65003
                        // url:'json/labelInfo.json',
                        type: 'get',
                        dataType: 'json',
                        async: false,
                        xhrFields: {
                            withCredentials: true
                        },
                        crossDomain: true,
                        data: {
                            LableType: 65001
                        },
                        success: function (data) {
                            if (data.Status == 0) {
                                result.Result.CategoryInfo = data.Result;
                            } else {
                                layer.msg(data.Message);
                            }
                        }
                    })
                    $.ajax({
                        url: 'http://www.chitunion.com/api/LabelTask/SelectLableInfo',// 分类:65001，场景，65002：IP :65003
                        // url:'json/labelInfo.json',
                        type: 'get',
                        dataType: 'json',
                        async: false,
                        xhrFields: {
                            withCredentials: true
                        },
                        crossDomain: true,
                        data: {
                            LableType: 65002
                        },
                        success: function (data) {
                            if (data.Status == 0) {
                                result.Result.SceneInfo = data.Result;
                            } else {
                                layer.msg(data.Message);
                            }
                        }
                    })
                    $.ajax({
                        url: 'http://www.chitunion.com/api/LabelTask/SelectLableInfo',//分类:65001，场景，65002：IP :65003
                        // url:'json/labelInfo.json',
                        type: 'get',
                        dataType: 'json',
                        async: false,
                        xhrFields: {
                            withCredentials: true
                        },
                        crossDomain: true,
                        data: {
                            LableType: 65003
                        },
                        success: function (data) {
                            if (data.Status == 0) {
                                result.Result.IPInfo = data.Result;
                            } else {
                                layer.msg(data.Message);
                            }
                        }
                    })
				    result.Result.CustomLableInfo=[];
				    result.Result.CustomSceneInfo = [];
		               
		                DelimitAndSelected();
		                _this.Submit();
		        }
		    })
		},
	    right: function () {
            var _this=this;
		    setAjax({
		    	url:'http://www.chitunion.com/api/LabelTask/SelectMediaOrArticleInfo',
		    	type:'get',
		    	data:{
		    		TaskID:GetRequest().TaskID
		    	}
		    },function(data){
		    	var result = {};
		    	if(data.Status == 0){
		    		result = data;
				    $.ajax({
                        url: 'http://www.chitunion.com/api/LabelTask/SelectLableInfo',//分类:65001，场景，65002：IP :65003
                        // url:'json/labelInfo.json',
                        type: 'get',
                        dataType: 'json',
                        async: false,
                        xhrFields: {
                            withCredentials: true
                        },
                        crossDomain: true,
                        data: {
                            LableType: 65001
                        },
                        success: function (data) {
                            if (data.Status == 0) {
                                result.Result.CategoryInfo = data.Result;
                            } else {
                                layer.msg(data.Message);
                            }
                        }
                    })
                    $.ajax({
                        url: 'http://www.chitunion.com/api/LabelTask/SelectLableInfo',// 分类:65001，场景，65002：IP :65003
                        // url:'json/labelInfo.json',
                        type: 'get',
                        dataType: 'json',
                        async: false,
                        xhrFields: {
                            withCredentials: true
                        },
                        crossDomain: true,
                        data: {
                            LableType: 65002
                        },
                        success: function (data) {
                            if (data.Status == 0) {
                                result.Result.SceneInfo = data.Result;
                            } else {
                                layer.msg(data.Message);
                            }
                        }
                    })
                    $.ajax({
                        url: 'http://www.chitunion.com/api/LabelTask/SelectLableInfo',//分类:65001，场景，65002：IP :65003
                        // url:'json/labelInfo.json',
                        type: 'get',
                        dataType: 'json',
                        async: false,
                        xhrFields: {
                            withCredentials: true
                        },
                        crossDomain: true,
                        data: {
                            LableType: 65003
                        },
                        success: function (data) {
                            if (data.Status == 0) {
                                result.Result.IPInfo = data.Result;
                            } else {
                                layer.msg(data.Message);
                            }
                        }
                    })
				    result.Result.CustomSceneInfo = [];
				    $('#right_z').html(ejs.render($('#right').html(), result));
				    var mediaType;
				    switch(result.Result.MediaType){
				    	case 14001:
				    		mediaType = '[微信]';
				    		break;
				    	case 14002:
				    		mediaType = '[APP]';
				    		break;
				    	case 14003:
				    		mediaType = '[微博]';
				    		break;
				    	case 14004:
				    		mediaType = '[视频]';
				    		break;
				    	case 14005:
				    		mediaType = '[直播]';
				    		break;
                        case 14006:
                            mediaType = '[头条]';
                            break;
				    }
				    $('.container .mark_right').before('<ul style="margin: 15px 0 0 0">'+
                            '<li class="ins_a" style="width: 123px;">'+
                                '<div class="frame_n"><img src='+result.Result.HeadImg+' width="80" height="80"/></div>'+
                            '</li>'+
                            '<li class="wechat_name">'+
                                '<div>'+
                                    result.Result.MediaName
                                +'</div>'+
                                '<div>'+
                                    result.Result.MediaOrArticle+mediaType
                                +'</div>'+
                            '</li>'+
                            '<div class="clear"></div>'+
                        '</ul>')
		            DelimitAndSelected();
                    $('.getLeft').each(function(){
                        //父IP的宽度
                        var l =  $(this).parents('.detail_mark').find('.sumLeft').outerWidth(true);
                        //此子IP及其旗下标签总共的高度
                        var h = $(this).parent('div').outerHeight(true);
                        $(this).css('marginLeft',l+'px');
                        $(this).css('marginBottom',h-50+'px')
                    })
                	_this.Submit();
			   	}
		    })
	 	},
        parameter: function () {//参数


            var classification = []; // 分类
            $('#classification li.hasChecked').each(function () {
                classification.push({
                    "DictId": $(this).attr('dictid') - 0,
                    "DictName": $.trim($(this).find('span').text())
                })
            })

            var OldScene = []; // 原始场景
            $('#oldscene li.hasChecked').each(function () {
                OldScene.push({
                    "DictId": $(this).attr('dictid') - 0,
                    "DictName": $.trim($(this).find('span').text())
                })
            })

            var NewScene1 = []; // 新场景1
            $('#newscene1 li.hasChecked').each(function () {
                NewScene1.push({
                    "DictName": $.trim($(this).find('span').text())
                })
            })
            var NewScene2 = [];//新场景2
            var newscene2str=$('#newscene2').val().replace(/；/g,';')
            $(newscene2str.split(';')).each(function (i, e) {
                if($.trim(e)!=''){
                    NewScene2.push({
                        "DictName": $.trim(e)
                    })
                }
            })
            var NewScene = NewScene1.concat(NewScene2);// 新场景1和2合并

            var ip = [];// ip
            $('.DifferenceIp').each(function () {
                var SonIP=[];
                $(this).find('.Differencesunzi').children('div.clear').siblings('div').each(function () {
                    var CustomLableInfo=[];
                    $($(this).find('input').val().replace(/；/g, ';').split(';')).each(function (i,e) {
                        if ($.trim(e) != '') {
                            CustomLableInfo.push({
                                "DictName": $.trim(e)
                            })
                        }
                    })
                    $(this).find('li[name="labelli"].hasChecked').each(function () {
                        CustomLableInfo.push({
                            "DictName": $(this).find('span').text()
                        })
                    })
                    SonIP.push({
                        "DictId": $(this).find('.getLeft').attr('dictid'),
                        "DictName": $(this).find('.getLeft span').text(),
                        "CustomLableInfo":CustomLableInfo
                    })
                })
                ip.push({
                    "DictId": $(this).find('.sumLeft').attr('dictid'),
                    "DictName": $(this).find('.sumLeft span').text(),
                    "SonIP":SonIP
                })
            })


            return {
                "TaskID": GetRequest().TaskID,
                "CategoryInfo": classification,
                "SceneInfo": OldScene,
                "CustomSceneInfo": NewScene,
                "IPInfo": ip
            }
        }, //获取参数
        judge: function () {//判断

            /*===========判断数量============*/
            // 分类
            var classification = $('#classification li.hasChecked').length;
            // console.log(classification, '分类数量');
            if (classification <= 0) {
                layer.msg('分类不能为空');
                return false;
            }
            // 场景
            var oldscene = $('#oldscene li.hasChecked').length - 0;
            var newscene1 = $('#newscene1 li.hasChecked').length - 0;
            var newscene2 = [];
            $($('#newscene2').val().replace(/；/g, ';').split(';')).each(function (i, e) {
                if ($.trim(e) != '') {
                    newscene2.push(e)
                }
            })
            newscene2 = newscene2.length - 0;
            var scene = oldscene + newscene1 + newscene2;
            // console.log(scene, '场景数量');
            if (scene === 0) {
                layer.msg('请至少选择或填写一个场景');
                return false;
            }
            if (scene > 100) {
                layer.msg('最多选择和填写一百个场景');
                return false;
            }

            // ip
            var ip = [];// ip
            $('.DifferenceIp').each(function () {
                var SonIP=[];
                $(this).find('.Differencesunzi').children('div.clear').siblings('div').each(function () {
                    var CustomLableInfo=[];
                    $($(this).find('input').val().replace(/；/g, ';').split(';')).each(function (i,e) {
                        if ($.trim(e) != '') {
                            CustomLableInfo.push({
                                "DictName": $.trim(e)
                            })
                        }
                    })
                    $(this).find('li[name="labelli"].hasChecked').each(function () {
                        CustomLableInfo.push({
                            "DictName": $(this).find('span').text()
                        })
                    })
                    SonIP.push({
                        "DictId": $(this).find('.getLeft').attr('dictid'),
                        "DictName": $(this).find('.getLeft span').text(),
                        "CustomLableInfo":CustomLableInfo
                    })
                })
                ip.push({
                    "DictId": $(this).find('.sumLeft').attr('dictid'),
                    "DictName": $(this).find('.sumLeft span').text(),
                    "SonIP":SonIP
                })
            })
            var flag=true;
            $(ip).each(function (i,e) {
                if(e.SonIP==undefined||e.SonIP.length==0){
                    flag=false;
                    layer.msg('请至少选择一个子IP');
                    return false;
                }
                var a=false;
                $(e.SonIP).each(function (i,e) {
                    if(e.CustomLableInfo==undefined||e.CustomLableInfo.length==0){
                        // console.log(e.CustomLableInfo);
                        // console.log(e.CustomLableInfo == undefined);
                        flag=false;
                        a=true
                        layer.msg('请至少填写一个标签');
                        return false;
                    }
                    if(e.CustomLableInfo.length>100){
                        flag=false;
                        layer.msg('最多填写一百个标签');
                        a=true
                        return false;
                    }
                })
                if(a){
                    return false
                }
            })
            if(ip.length==0){
                layer.msg('请至少选择一个IP');
                return false;
            }
            if(!flag){
                return false
            }
            return true;
        }, //判断数量
        repetition: function () {

            // 场景
            var scene = [];
            $('#oldscene li.hasChecked').each(function () {
                scene.push($.trim($(this).find('span').text()))
            });
            $('#newscene1 li.hasChecked').each(function () {
                scene.push($.trim($(this).find('span').text()))
            })
            $($('#newscene2').val().replace(/；/g, ';').split(';')).each(function (i, e) {
                if ($.trim(e) != '') {
                    scene.push($.trim(e))
                }
            })
            // console.log(scene, '场景的文本');
            var scene_cf = false;

            var newArr1 = [];
            for (var i = 0, len = scene.length; i < len; i++) {
                if (newArr1.indexOf(scene[i]) == -1) {
                    newArr1.push(scene[i]);
                }
            }
            newArr1.forEach(function (single, k) {
                var count = 0;
                scene.forEach(function (item, j) {
                    if (single == item) {
                        count++;
                    }
                })
                if (count > 1) {
                    scene_cf = true;
                }
            })
            if (scene_cf) {
                layer.msg('场景不能填写重复内容');
                return false;
            }

            // ip
            // var sonAdd = [];
            // $('.sonAdd textarea').each(function () {
            //     var a = [];
            //     $($(this).val().replace(/；/g, ';').split(';')).each(function (i, e) {
            //         if ($.trim(e) != '') {
            //             a.push($.trim(e))
            //         }
            //     })
            //     if ($(this).parent().siblings('.son_ip').find('span').text()) {
            //         a.push($.trim($(this).parent().siblings('.son_ip').find('span').text()));
            //     }
            //     sonAdd.push(a);
            // })
            // // console.log(sonAdd, 'ip的文本数组');
            // var sonAdd_cf = false;
            //
            // function getRepeatNum(arr) {
            //     var newArr = [];
            //     for (var i = 0, len = arr.length; i < len; i++) {
            //         if (newArr.indexOf(arr[i]) == -1) {
            //             newArr.push(arr[i]);
            //         }
            //     }
            //     newArr.forEach(function (single, k) {
            //         var count = 0;
            //         arr.forEach(function (item, j) {
            //             if (single == item) {
            //                 count++;
            //             }
            //         })
            //         if (count > 1) {
            //             sonAdd_cf = true;
            //         }
            //     })
            // }
            //
            // for (var j = 0; j < sonAdd.length; j++) {
            //     getRepeatNum(sonAdd[j]);
            // }
            // // console.log(sonAdd_cf);
            // if (sonAdd_cf) {
            //     layer.msg('子IP不能填写重复内容');
            //     return false;
            // }

            var ip = [];// ip
            $('.DifferenceIp').each(function () {
                var SonIP=[];
                $(this).find('.Differencesunzi').children('div.clear').siblings('div').each(function () {
                    var CustomLableInfo=[];
                    $($(this).find('input').val().replace(/；/g, ';').split(';')).each(function (i,e) {
                        if ($.trim(e) != '') {
                            CustomLableInfo.push({
                                "DictName": $.trim(e)
                            })
                        }
                    })
                    $(this).find('li[name="labelli"].hasChecked').each(function () {
                        CustomLableInfo.push({
                            "DictName": $(this).find('span').text()
                        })
                    })
                    SonIP.push({
                        "DictId": $(this).find('.getLeft').attr('dictid'),
                        "DictName": $(this).find('.getLeft span').text(),
                        "CustomLableInfo":CustomLableInfo
                    })
                })
                ip.push({
                    "DictId": $(this).find('.sumLeft').attr('dictid'),
                    "DictName": $(this).find('.sumLeft span').text(),
                    "SonIP":SonIP
                })
            })
            var sonadd=true;
            $(ip).each(function (i,e) {
                var a=true;
                $(e.SonIP).each(function (i,e) {
                    var b=true;
                    var v=[]
                    $(e.CustomLableInfo).each(function (i,e) {
                        v.push(JSON.stringify(e))
                    })
                    var ary = v;
                    // console.log(ary);
                    var s = ary.join(",")+",";
                    for(var i=0;i<ary.length;i++) {
                        if(s.replace(ary[i]+",","").indexOf(ary[i]+",")>-1) {
                            b=false;
                            break;
                        }
                    }
                    if(!b){
                        a=false
                        return false;
                    }
                })
                if(!a){
                    sonadd=false
                    return false;
                }
            })
            if(!sonadd){
                layer.msg('标签不能填写重复内容');
                return false;
            }
            return true;
        }, // 判断重复
	    Submit:function () {
            var _this=this;
            $('#Submit').off('click').on('click',function () {
                if(!_this.judge()){
                    return false;
                }
                if(!_this.repetition()){
                    return false;
                }
                if($('#Submit').css('background-color') != "rgb(255, 79, 79)"){
                    return false
                }
                // console.log(_this.parameter())
                // return false;
                $.ajax({
                    url:'http://www.chitunion.com/api/LabelTask/InserLableTakeInfo',
                    type:'post',
                    data: _this.parameter(),
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    dataType:"json",
                    beforeSend: function () {
                        $('#Submit').css('background-color','rgb(102, 102, 102)');
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
                            layer.msg('成功',{time:1000}, function () {
                                window.location = '/TagManager/TagManageList.html';
                            })
                        }else{
                            layer.msg(data.Message);
                            $('#Submit').css('background-color','#FF4F4F');
                        }
                    }
                })
            })
	    }
	}

    new ToExamine()
})