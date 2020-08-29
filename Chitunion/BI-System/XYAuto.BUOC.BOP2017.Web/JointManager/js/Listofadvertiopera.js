$(function () {

    var xyConfig = {
        url : {
            'SelectZhyOperaterList' : public_url + '/api/ZhyInfo/SelectZhyOperaterList',//列表数据渲染
            'SelectOptAdvertiserList' : public_url + '/api/ZhyInfo/SelectOptAdvertiserList',//
            'BingdingOptIdToAdvsId' : public_url + '/api/ZhyInfo/BingdingOptIdToAdvsId'
        }
    } 

    function Listofadvertiopera() {
        this.Rendering();
    }
    Listofadvertiopera.prototype={
        constructor: Listofadvertiopera,
        Rendering:function () {
            var _this=this;
            var url =  xyConfig.url.SelectZhyOperaterList;
            //var url = 'json/Listofadvertiopera.json';
            $('#but_query').off('click').on('click',function () {
                setAjax({
                    url: url,
                    type: "GET",
                    data: _this.parameter(1)
                }, function (data) {
                    $('table').html(ejs.render($('#Listofadvertiopera').html(), data));
                    _this.operation()
                    if (data.Result.TotalCount != 0) {
                        var counts = data.Result.TotalCount;
                        //分页
                        $("#pageContainer").pagination(
                            counts,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    // ajax请求
                                    setAjax({
                                        url: url,
                                        type: "GET",
                                        data: _this.parameter(currPage)
                                    }, function (data) {
                                        $('table').html(ejs.render($('#Listofadvertiopera').html(), data));
                                        _this.operation()
                                    })
                                }
                            });

                    } else {
                        $('#pageContainer').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                })
            })
            $('#but_query').click()
        },
        parameter:function (PageIndex) {
           var Cellphone=$('#Cellphone').val();
           var Username=$('#Username').val();
           return {
               UserName:Username,
               Mobile:Cellphone,
               PageIndex:PageIndex,
               pageSize:20
           }
        },
        operation:function () {
            //关联广告主
            $('.Associatedadvertiser').off('click').on('click',function () {
                var UserId=$(this).attr('UserId');
                var UserName=$(this).attr('UserName');
                $.openPopupLayer({
                    name: "Associatedadvertiser",
                    url: "Associatedadvertiser.html",
                    error: function (dd) {
                        alert(dd.status);
                    },
                    success: function (data) {
                        $('#gld').html('关联到"'+UserName+'"');
                        $('#closebt').off('click').on('click',function () {
                            $.closePopupLayer('Associatedadvertiser')
                        })
                        // 右侧
                        $.ajax({
                            url : xyConfig.url.SelectOptAdvertiserList,
                            type: 'get',
                            data: {
                                OperaterId:UserId
                            },
                            dataType: 'json',
                            xhrFields: {
                                withCredentials: true
                            },
                            crossDomain: true,
                            async: false,
                            success: function (data) {
                                if(data.Status == 0){
                                    data.Result.AdvertiserInfo.forEach(function (e) {
                                        $('#tar-city').append('<option value='+e.UserId+'>'+e.UserName+'-'+e.Mobile+'</option>')
                                    })
                                }
                            }
                        });
                        //弹层确定
                        $('#submitMessage').off('click').on('click',function () {
                            var arr=[];
                            $('#tar-city option').each(function () {
                                arr.push($(this).val())
                            })
                            setAjax({
                                url : xyConfig.url.BingdingOptIdToAdvsId,
                                type:'post',
                                data:{
                                    "OperaterId":UserId,
                                    "OperateType":2,
                                    "AdvertiserIds":arr
                                }
                            },function (data) {
                               if(data.Status==0){
                                   $.closePopupLayer('Associatedadvertiser');
                                   $('#but_query').click()
                               } else {
                                   layer.msg(data.Message)
                               }
                            })
                        })
                    }
                });
            })

            $('#xinjianyonghu').off('click').on('click',function () {
                window.location='http://www.chitunion.com/UserInfoManage/InsideAddUser.aspx'
            })
        }
    }
    var listofadvertiopera=new Listofadvertiopera()
})