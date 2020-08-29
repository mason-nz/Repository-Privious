$(function () {
    $('.but_query').off('click').on('click', function () {
        $('#allbox').prop('checked',false);
        function parameter(currPage) {
            var content = $('#content').val();
            var Status = 0;
            return {
                PageIndex: currPage,
                PageSize: 20,
                Status: Status,
                TitleOrAbstract: content
            }
        };
        setAjax({
            url: '/api/WeChatEditor/SelectArticleGroupListByIDList',
            type: 'get',
            data: parameter(1)
        }, function (data) {
            $('.pic_display').html('<div class="pic_table"> <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#ddd" id="table"> </table> </div> <!--分页--> ')
            // 如果数据为0显示图片
            if (data.Result.TotalCount != 0) {
                //分页
                $("#pageContainer").pagination(
                    data.Result.TotalCount,
                    {
                        items_per_page: 20, //每页显示多少条记录（默认为20条）
                        callback: function (currPage, jg) {
                            setAjax({
                                url: '/api/WeChatEditor/SelectArticleGroupListByIDList',
                                type: 'get',
                                data: parameter(currPage)
                            }, function (data) {
                                $('.pic_display').html('<div class="pic_table"> <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#ddd" id="table"> </table> </div>')
                                $('#table').html(ejs.render($('#synchronous_history').html(), data));
                                operation()
                            })
                        }
                    });
                $('#pageContainer').show();
            } else {
                $('.pic_display').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                $('#pageContainer').hide();
            }
            ;
            $('#table').html(ejs.render($('#synchronous_history').html(), data));
            operation()
        })
    });
    $('.but_query').click();
    
    function operation() {
        $('.Open_list').off('click').on('click',function () {
            if($(this).find('.Open_zhankai').attr('class')=='Open_zhankai'){
                console.log($(this));
                $(this).html('<img class="Open_shouqi" src="/ImagesNew/syn_07.png"><div class="open_s">收起<em><img src="/ImagesNew/icon90.png"></em></div>')
                $(this).parents('tr').find('.show_hide').show();
                return false;
            }
            if($(this).find('.Open_shouqi').attr('class')=='Open_shouqi'){
                $(this).parents('tr').find('.show_hide').hide();
                $(this).html('<img class="Open_zhankai" src="/ImagesNew/syn_06.png"><div class="open_s">展开<em><img src="/ImagesNew/icon90.png"></em></div>');
                return false;
            }

        })
    };


    $('#allbox').off('change').on('change',function () {
        if($('#allbox').prop('checked')==false){
            function parameter(currPage) {
                var content = $('#content').val();
                var Status = 0;
                return {
                    PageIndex: currPage,
                    PageSize: 20,
                    Status: Status,
                    TitleOrAbstract: content
                }
            };
            setAjax({
                url: '/api/WeChatEditor/SelectArticleGroupListByIDList',
                type: 'get',
                data: parameter(1)
            }, function (data) {
                $('.pic_display').html('<div class="pic_table"> <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#ddd" id="table"> </table> </div> <!--分页--> ')

                // 如果数据为0显示图片
                if (data.Result.TotalCount != 0) {
                    //分页
                    $("#pageContainer").pagination(
                        data.Result.TotalCount,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                setAjax({
                                    url: '/api/WeChatEditor/SelectArticleGroupListByIDList',
                                    type: 'get',
                                    data: parameter(currPage)
                                }, function (data) {
                                    $('.pic_display').html('<div class="pic_table"> <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#ddd" id="table"> </table> </div> <!--分页--> ')
                                    $('#table').html(ejs.render($('#synchronous_history').html(), data));
                                    operation()
                                })
                            }
                        });
                    $('#pageContainer').show();
                } else {
                    $('.pic_display').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    $('#pageContainer').hide();
                }
                ;
                $('#table').html(ejs.render($('#synchronous_history').html(), data));
                operation()
            })
        }else {
            function parameter(currPage) {
                var content = $('#content').val();
                var Status = 1;
                return {
                    PageIndex: currPage,
                    PageSize: 20,
                    Status: Status,
                    TitleOrAbstract: content
                }
            };
            setAjax({
                url: '/api/WeChatEditor/SelectArticleGroupListByIDList',
                type: 'get',
                data: parameter(1)
            }, function (data) {
                $('.pic_display').html('<div class="pic_table"> <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#ddd" id="table"> </table> </div> <!--分页--> ')
                // 如果数据为0显示图片
                if (data.Result.TotalCount != 0) {
                    //分页
                    $("#pageContainer").pagination(
                        data.Result.TotalCount,
                        {
                            items_per_page: 20, //每页显示多少条记录（默认为20条）
                            callback: function (currPage, jg) {
                                setAjax({
                                    url: '/api/WeChatEditor/SelectArticleGroupListByIDList',
                                    type: 'get',
                                    data: parameter(currPage)
                                }, function (data) {
                                    $('.pic_display').html('<div class="pic_table"> <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#ddd" id="table"> </table> </div> <!--分页-->')
                                    $('#table').html(ejs.render($('#synchronous_history').html(), data));
                                    operation()
                                })
                            }
                        });
                    $('#pageContainer').show();
                } else {
                    $('.pic_display').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    $('#pageContainer').hide();
                }
                ;
                $('#table').html(ejs.render($('#synchronous_history').html(), data));
                operation()
            })
        }
    })
})