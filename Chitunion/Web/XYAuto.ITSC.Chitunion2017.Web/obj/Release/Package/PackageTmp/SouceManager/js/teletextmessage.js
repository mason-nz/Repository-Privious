$(function () {
    function TeletextMessage() {
        this.Graphicrendering();
    }
    TeletextMessage.prototype={
        constructor: TeletextMessage,
        // 图文渲染
        Graphicrendering:function () {
            var _this=this;
            $('.but_query').off('click').on('click',function () {
                setAjax({
                    url:'/api/Article/GetWeixinArticleGroupList',
                    type:'get',
                    data:_this.parameter(1)
                },function (data) {

                    $('#content').html(ejs.render($('#teletextmessage').html(), data));
                    $('#Total').html('图文消息(共'+data.Result.TotalCount+'条)');
                    // 如果数据为0显示图片
                    if (data.Result.TotalCount != 0) {
                        //分页
                        $("#pageContainer").pagination(
                            data.Result.TotalCount,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    setAjax({
                                        url:'/api/Article/GetWeixinArticleGroupList',
                                        type:'get',
                                        data:_this.parameter(currPage)
                                    }, function (data) {
                                        $('#content').html(ejs.render($('#teletextmessage').html(), data));
                                        $('#Total').html('图文消息(共'+data.Result.TotalCount+'条)');
                                        _this.operation()
                                    })
                                }
                            });
                        $('#pageContainer').show();
                    } else {
                        $('.pic_display').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">').css('min-height','255px');
                        $('.teletext_message').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">').css('min-height','300px');
                        $('#pageContainer').hide();
                    };
                    _this.operation()

                })
            })
            $('.but_query').click();

        },
        // 参数
        parameter:function (i) {
            var abstract=$('#abstract').val();
            return {
                Key:abstract,
                PageSize:20,
                PageIndex:i
            }
        },
        // 操作
        operation:function () {
            // 瀑布流
            $(function(){
                $('#brand-waterfall').waterfall();
            });
            // 切换
            $('#Gongge').off('click').on('click',function () {
                $('.teletext_message').show();
                $('.pic_display').hide();
                $('#Gongge img').attr('src','/ImagesNew/icon69.png');
                $('#Gonggelist img').attr('src','/ImagesNew/icon71.png');
            })
            $('#Gonggelist').off('click').on('click',function () {
                $('.teletext_message').hide();
                $('.pic_display').show();
                $('#Gongge img').attr('src','/ImagesNew/icon68.png');
                $('#Gonggelist img').attr('src','/ImagesNew/icon70.png');
            });
            // 点击删除
            $('.delete_off').off('click').on('click',function () {
                var _this=$(this);
                if ($(this).attr('SyncCount')>0){
                    layer.msg('不可删除');
                    return false;
                }
                layer.confirm('删除后不会影响已同步的图文消息，确定删除该素材？', {
                    time: 0 //不自动关闭
                    , btn: ['确认', '取消']
                    , yes: function (index) {
                        setAjax({
                            url:'/api/Article/BatchDelete',
                            type:'post',
                            data:{
                                GroupID:_this.attr("groupid")-0
                            }
                        },function (data) {
                            if(data.Status==0){
                                layer.msg('删除成功',{time:1000});
                                $('.but_query').click();
                            }
                        })
                        layer.close(index);
                        // layer.msg('操作成功', {time: 400});
                    }
                });

            });
            // 点击导入文章
            $('#importarticles').off('click').on('click',function () {
                $.openPopupLayer({
                    name: "clickApply",
                    url: "importarticles.html",
                    error: function (dd) {
                        alert(dd.status);
                    },
                    success: function () {
                        // 点击关闭
                        $('.close').off('click').on('click',function () {
                            $.closePopupLayer('clickApply')
                        });
                        // 点击确认
                        $('.but_data').off('click').on('click',function () {
                            if ($('#obj1-1').val().indexOf('https://mp.weixin.qq.com/') == -1) {
                                layer.msg('请输入正确的url地址', {time: 600});
                                return false;
                            }
                            if ($.trim($('#obj1-1').val()) == '') {
                                return false
                            }
                           setAjax({
                               url:'/api/WeChatEditor/ImportWxArticle',
                               type:'get',
                               data:{
                                   // ImportType:53001,
                                   ImportUrl:$.trim($('#obj1-1').val())
                               }
                           },function (data) {
                               if(data.Status==0){
                                   layer.msg('导入成功');
                                   $.closePopupLayer('clickApply');
                                   $('.but_query').click();
                               }else {
                                   layer.msg('导入失败');
                               }
                           })
                        })
                    }
                })
            });
            // 经过显示弹出
            $('.mobileshell').off('mouseover').on('mouseover',function () {
                $(this).find('.budge').show().find('ul').eq(0).off('click').on('click',function () {
                    var _this=$(this);
                    console.log('qqq');
                    console.log(_this);
                    console.log(_this.attr('name'));
                    // pc端
                    setAjax({
                        url:'/api/Article/GetArticleView',
                        type:'get',
                        data:{
                            OptType:1,
                            ArticleID:_this.attr('name')
                        }
                    },function (data) {
                        window.open(data.Result)
                    })
                }).end().eq(1).off('click').on('click',function () {
                    var _this=$(this);
                    // 二位码
                    setAjax({
                        url:'/api/Article/GetArticleView',
                        type:'get',
                        data:{
                            OptType:2,
                            ArticleID:_this.attr('name')
                        }
                    },function (data) {
                        _this.parent().hide().next().show().off('click').on('click',function () {
                            $(this).hide()
                        }).find('img').attr('src',data.Result)
                    });

                })
            });
            $('.mobileshell').off('mouseout').on('mouseout',function () {
                $(this).find('.budge').hide();
            });

            $('.viev').off("click").on('click',function () {
                var _this=$(this);
                // pc端
                setAjax({
                    url:'/api/Article/GetArticleView',
                    type:'get',
                    data:{
                        OptType:1,
                        ArticleID:_this.attr('name')
                    }
                },function (data) {
                    window.open(data.Result)
                })
            });
            
            // 编辑
            $('.compile').off("click").on('click',function () {
                if ($(this).attr('SyncCount')>0){
                    layer.msg('不可编辑');
                    return false;
                }
                window.location='/SouceManager/editor.html?GroupID='+$(this).attr('GroupID')
            })
            // 选中后不切换
            if($('#Gongge img').attr('src')=='/ImagesNew/icon69.png'){
                $('#Gongge').click();
                return false;
            }else {
                $('#Gonggelist').click()
            }
        }
    }
    new TeletextMessage();
})