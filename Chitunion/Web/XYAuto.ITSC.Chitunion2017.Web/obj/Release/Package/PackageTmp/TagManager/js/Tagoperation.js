// 获取url？后面的参数
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}

function DelimitAndSelected() {
    /*右侧textarea聚焦后，添加class，以便添加关键词时放置*/
    $('.mark_right').off('focus').on('focus', 'textarea', function () {
        $('.mark_right').find('textarea,input').removeClass('onFocus');
        $(this).addClass('onFocus');
    })
    $('.mark_right').off('focus').on('focus', 'input', function () {
        $('.mark_right').find('textarea,input').removeClass('onFocus');
        $(this).addClass('onFocus');
    })
    /*点击左侧关键词，对应右侧光标处增加一个*/
    $('.keyWords').off('click').on('click', 'li', function () {
        var addText = $.trim($(this).text()) + ';';
        var oldText = $('.mark_right').find('.onFocus').val().replace(/；/g, ';');
        var textArr = oldText && oldText.split(';');
        if (!contains(textArr, $.trim($(this).text()))) {
            $('.mark_right').find('.onFocus').val(oldText + addText);
        }

    })

    function contains(arr, obj) {
        var i = arr && arr.length;
        while (i--) {
            if (arr[i] === obj) {
                return true;
            }
        }
        return false;
    }


    // 改变子IP的marginleft
    $('.getLeft').each(function(){
        //父IP的宽度
        var l =  $(this).parents('.detail_mark').find('.sumLeft').outerWidth(true);
        //此子IP及其旗下标签总共的高度
        var h = $(this).parent('div').outerHeight(true);
        $(this).css('marginLeft',l+'px');
        // $(this).css('marginBottom',h-50+'px')
    })
    $('.getTotalLeft').each(function(){
        var l = $(this).parent().find('.getLeft').outerWidth(true);
        $(this).css('marginLeft',l+'px')
    })
    $('.Differencesunzi .defaultInput').each(function(){
        var l = $(this).parent('div').find('.getLeft').outerWidth(true);
        $(this).css('marginLeft',l+'px');
    })

    //切换：关键词和原文
    $('.switch').off('click').on('click', 'span', function () {
        $(this).addClass('current').siblings('span').removeClass('current');
        var idx = $(this).index();
        if (idx == 0) {
            $('.mark_left').find('.keyWords').show().end().find('.article').hide();
        } else {
            $('.mark_left').find('.keyWords').hide().end().find('.article').show();
        }
    });

    $('.detail_mark li').off('click').on('click', function () {// 点击选中，再点取消选中
        var _this = $(this);
        if ($(this).attr('class') != 'hasChecked') {
            // 判断分类和ip不能选择5个以上
            if ($(this).parents('ul').attr('id') == 'classification') {
                if ($('#classification li.hasChecked').length >= 5) {
                    layer.msg('最多选择5个分类');
                    return false;
                }
            }
            if ($(this).parents('ul').attr('id') == 'ip') {
                if ($('#ip li.hasChecked').length >= 5) {
                    layer.msg('最多可选择5个IP');
                    return false;
                }
            }

            if ($(this).attr('class').indexOf('Unselected') == -1) {// 如果是父ip点击没有效果
                $(this).attr('class', 'hasChecked')
            }

            if ($(this).attr('name') == 'selected') {//判断是否是ip的
                var IpId=[]
                $.ajax({
                    url: 'http://www.chitunion.com/api/LabelTask/SelectSonIpInfo',
                    type: 'get',
                    data: {
                        IpId: $(this).attr('dictid')
                    },
                    dataType: 'json',
                    xhrFields: {
                        withCredentials: true
                    },
                    async:false,
                    crossDomain: true,
                    success: function (data) {
                        if(data.Status==0){
                            IpId=data.Result;
                        }else {
                            layer.msg(data.Message)
                        }

                    }
                });
                var zIpId='';
                for(var i=0;i<IpId.length;i++){
                    zIpId+='<li DictId="'+IpId[i].DictId+'" name="q" class=""><span>'+IpId[i].DictName+'</span></li>'
                }
                $('.sonAdd').append('<div class="DifferenceIp">\n' +
                    '                <li style="margin-bottom: 20px;" dictid='+$(this).attr("dictid")+' class="sumLeft Unselected">\n' +
                    $(this).clone(true).html()+
                    '                    <i class="add_ear"></i>\n' +
                    '                </li>\n' +zIpId+
                    '                <div class="clear"></div>\n' +
                    '<div class="add_sunzi"></div>' +
                    '            </div>')
                $('.markPerson').hide();
                DelimitAndSelected();
            }
            if($(this).attr('name')=='q'){
                if($(this).siblings('.add_sunzi').children('div').attr('class')!='Differencesunzi'){
                    $(this).siblings('.add_sunzi').append('<div class="Differencesunzi">\n<div>' +
                        '                        <li class="getLeft Unselected" dictid='+$(this).attr("dictid")+'>\n' +
                        $(this).clone(true).html()+
                        '                        </li>\n' +
                        '                        <input type="text" name="" style="width:300px;">\n' +
                        '                        </div><div class="clear"></div>\n' +
                        '                    </div>');
                }else {
                    $(this).siblings('.add_sunzi').children('div').children('div:last').before('<div>' +
                        '                        <li class="getLeft Unselected" dictid='+$(this).attr("dictid")+'>\n' +
                        $(this).clone(true).html()+
                        '                        </li>\n' +
                        '                        <input type="text" name="" style="width:300px;">\n' +
                        '                        </div>');
                }

                $('.markPerson').hide()
                DelimitAndSelected();
            }
        } else {
            if ($(this).attr('name') == 'selected') {//判断是否是ip的

                $('.DifferenceIp').each(function () {
                    var $this = $(this);
                    if($.trim($(this).find('.sumLeft span').text()) == $.trim(_this.find('span').text())){
                        if($(this).find('li[name=q].hasChecked').length!=0){
                            $(this).find('.add_sunzi .Differencesunzi').each(function () {
                                var flag=false;
                                $(this).find('li[name=labelli]').each(function () {
                                    if($(this).attr('class')=='hasChecked'){
                                        flag=true;
                                    }
                                })
                                if(flag||$.trim($(this).find('input').val())!=''){
                                    layer.confirm('取消后IP与标签不可恢复，确认取消吗', {
                                        time: 0 //不自动关闭
                                        , btn: ['确认', '取消']
                                        , yes: function (index) {
                                            $this.remove();
                                            _this.attr('class', '');
                                            layer.msg('删除成功', {time: 2000});
                                            layer.close(index);
                                        }
                                    });
                                    return false;
                                }else {
                                    $this.remove();
                                    _this.attr('class', '');
                                }
                            })
                        }else {
                            $this.remove();
                            _this.attr('class', '');
                        }

                        $(this).find('li[name=q]').each(function () {
                            var _this = $(this);
                            if($(this).attr('class')=='hasChecked'){
                                $(this).find('.Differencesunzi').each(function () {
                                    var $this = $(this);
                                    var flag=false;
                                    $(this).find('li[name=labelli]').each(function () {
                                        if($(this).attr('class')=='hasChecked'){
                                            flag=true;
                                        }
                                    })
                                    if(flag||$.trim($(this).find('input').val())!=''){
                                        layer.confirm('取消后IP与标签不可恢复，确认取消吗', {
                                            time: 0 //不自动关闭
                                            , btn: ['确认', '取消']
                                            , yes: function (index) {
                                                $this.remove();
                                                _this.attr('class', '');
                                                layer.msg('删除成功', {time: 2000});
                                                layer.close(index);
                                            }
                                        });
                                        return false;
                                    }else {
                                        $this.remove();
                                        _this.attr('class', '');
                                    }
                                })
                            }
                        })
                    }
                })
            } else {


                if($(this).attr('name') == 'q'){
                    // console.log($(this));
                    // console.log($(this).parent('.DifferenceIp'));
                    $('.DifferenceIp').each(function () {
                        var $this = $(this);
                        if($.trim($(this).find('.sumLeft span').text()) == $.trim(_this.find('span').text())){
                            // console.log($(this).find('li[name=q]'));
                            $(this).find('li[name=q]').each(function () {
                                var _this = $(this);
                                // console.log(_this);
                                if($(this).attr('class')=='hasChecked'){
                                    // console.log(1);
                                    $(this).find('.Differencesunzi').each(function () {
                                        var $this = $(this);
                                        var flag=false;
                                        $(this).find('li[name=labelli]').each(function () {
                                            if($(this).attr('class')=='hasChecked'){
                                                flag=true;
                                            }
                                        })
                                        if(flag||$.trim($(this).find('input').val())!=''){
                                            layer.confirm('取消后IP与标签不可恢复，确认取消吗', {
                                                time: 0 //不自动关闭
                                                , btn: ['确认', '取消']
                                                , yes: function (index) {
                                                    $this.remove();
                                                    _this.attr('class', '');
                                                    layer.msg('删除成功', {time: 2000});
                                                    layer.close(index);
                                                }
                                            });
                                            return false;
                                        }else {
                                            $this.remove();
                                            _this.attr('class', '');
                                        }
                                    })
                                }
                            })
                        }
                    })
                    $(this).parent('.DifferenceIp').find('.Differencesunzi').children('div').each(function () {
                        var $this = $(this);
                        // console.log($(this));
                        // console.log($.trim($this.find('.getLeft span').text()));
                        // console.log($.trim(_this.find('span').text()));
                        if($.trim($this.find('.getLeft span').text())==$.trim(_this.find('span').text())){
                            if($this.find('li[name=labelli].hasChecked').length!=0||$.trim($this.find('input').val())!=''){
                                layer.confirm('取消后子IP与标签不可恢复，确认取消吗', {
                                    time: 0 //不自动关闭
                                    , btn: ['确认', '取消']
                                    , yes: function (index) {
                                        $this.remove();
                                        _this.attr('class', '');
                                        layer.msg('删除成功', {time: 2000});
                                        layer.close(index);
                                    }
                                });
                                return false;
                            }else {
                                $this.remove();
                                _this.attr('class', '');
                                return false;
                            }
                        }
                    })
                }else {
                    $(this).attr('class', '');
                }
            }
        }

    }).off('mouseover').on('mouseover', function () {// 鼠标移入显示名称
        $(this).find('.markPerson').show();
    }).off('mouseout').on('mouseout', function () {// 鼠标移出隐藏名称
        $(this).find('.markPerson').hide();
    })

    /*设置打标签人显示的位置*/
    $('.markPerson').each(function () {
        var curW = $(this).width(),
            curH = $(this).height();
        $(this).css('right', -curW - 13);
        $(this).css('top', -curH / 2 + 8);
        $(this).find('img').css('top', curH / 2);
    });

    // input
    $('input').attr('placeholder','请输入标签文字，标签之间用 ";" 分隔')
}
