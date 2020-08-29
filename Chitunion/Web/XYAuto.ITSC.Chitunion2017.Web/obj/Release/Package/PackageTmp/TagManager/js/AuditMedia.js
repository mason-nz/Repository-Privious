$(function () {
    function ToExamine() {
        this.right();
    }

    ToExamine.prototype = {
        constructor: ToExamine,
        left: function (_this) {
            var _this=_this;
            setAjax({
                url: 'http://www.chitunion.com/api/LabelTask/SelectMediaOrArticleLable',
                type: 'GET',
                data: {
                    TaskID: GetRequest().TaskID,
                    SelectType:1
                }
            }, function (data) {

                $('#left_z').html(ejs.render($('#left').html(), data));
                // 点击新摘要
                $('#newsummary').off('click').on('click',function () {
                   setAjax({
                       url:'http://www.chitunion.com/api/LabelTask/GetSummaryKeyWord',
                       type: 'get',
                       data: {
                           articleID:$('#newsummary').attr('articleID')-0,
                           summarySize:$('.TaskCount').eq(0).val()-0,
                           keyWordSize:$('.TaskCount').eq(1).val()-0
                       }
                   },function (data) {
                       if(data.Status==0){
                           $('.summary').html(data.Result.Summary)
                       }else {
                           layer.msg(data.Message)
                       }

                   })
                });
                // 点击新关键词
                $('#newKeyword').off('click').on('click',function () {
                    setAjax({
                        url:'http://www.chitunion.com/api/LabelTask/GetSummaryKeyWord',
                        type: 'get',
                        data: {
                            articleID:$('#newsummary').attr('articleID')-0,
                            summarySize:$('.TaskCount').eq(0).val()-0,
                            keyWordSize:$('.TaskCount').eq(1).val()-0
                        }
                    },function (data) {
                        if(data.Status==0){
                            var Keyword=''
                            $(data.Result.KeyWord.split(',')).each(function (i,e) {
                                Keyword+='<li>'+e+'</li>';
                            })
                            $('#Keyword').html(Keyword);
                        }else {
                            layer.msg(data.Message)
                        }

                    })
                });
                $('#newsummary').click()
                $('#newKeyword').click()

                /*=====关键字和摘要范围======*/
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

                $('.TaskCount').off('blur').on('blur',function(){
                    var _this = $(this);
                    var val = _this.val();
                    if(val){
                        if(val<1){
                            _this.val('1');
                        }else if( val >50){
                            _this.val('50');
                        }
                    }
                }).off('input').on('input',function(){
                    replaceAndSetPos(this,/[^0-9]/g,'');
                })
                /*=====关键字和摘要范围======*/


                DelimitAndSelected();
                _this.Submit();
            })
        },
        right: function () {
            var _this=this;
            setAjax({
                url: 'http://www.chitunion.com/api/LabelTask/SelectMediaOrArticleLable',
                // url:'json/info.json',
                type: 'GET',
                data: {
                    TaskID: GetRequest().TaskID,
                    SelectType:GetRequest().SelectType
                }
            }, function (data) {
                var result = {};
                if (data.Status == 0) {
                    result = data;
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
                            '<li class="ins_a"  style="width: 123px;">'+
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
                        '</ul>');
                    // _this.left(_this);
                     DelimitAndSelected();
                    // 改变子IP的marginleft
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
                "OptType":GetRequest().SelectType==1?2:1,
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
               // console.log(_this.parameter());
               $.ajax({
                   url:'http://www.chitunion.com/api/LabelTask/ExmainLableInfo',//  json/info.json
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
                               window.location = '/TagManager/TagAuditList.html';
                           })
                       }else{
                           layer.msg(data.Message);
                           $('#Submit').css('background-color','#FF4F4F');
                       }
                   }
               })
           });

           // 取消
            $('#cancel').off('click').on('click',function () {
                setAjax({
                    url:'http://www.chitunion.com/api/LabelTask/LabelTaskAuditCancel',
                    type:'get',
                    data:{
                        taskID:GetRequest().TaskID
                    }
                },function (data) {
                    if(data.Status==0){
                        window.location='/TagManager/TagAuditList.html'
                    }else {
                        layer.msg(data.Message)
                    }
                })
            })
        }
    }

    new ToExamine()
})