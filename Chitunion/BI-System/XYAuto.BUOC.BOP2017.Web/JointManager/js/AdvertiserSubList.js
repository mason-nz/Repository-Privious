$(function () {
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

    var xyConfig = {
        url : {
            'SelectZhyAdvertiserList' : public_url + '/api/ZhyInfo/SelectZhyAdvertiserList'//客户管理的列表
        }
    }

    function AdvertiserSubList(user) {
        this.Rendering(user)
    }

    AdvertiserSubList.prototype = {
        constructor: AdvertiserSubList,
        Rendering: function (user) {//渲染
            var _this = this;
            var url = xyConfig.url.SelectZhyAdvertiserList;
            //var url = 'json/AdvertiserSubList.json';
            $('.but_query').off('click').on('click', function () {
                setAjax({
                    url: url,
                    type: 'get',
                    data: _this.parameter(1)
                }, function (data) {
                    $('#Rendering').html(ejs.render($(user).html(), data));
                    _this.operation();
                    // 如果数据为0显示图片
                    if (data.Result.TotalCount != 0) {
                        $('#pageContainer').show()
                        //分页
                        $("#pageContainer").pagination(
                            data.Result.TotalCount,
                            {
                                items_per_page: 20, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    setAjax({
                                        url: url,
                                        type: 'get',
                                        data: _this.parameter(currPage)
                                    }, function (data) {
                                        $('#Rendering').html(ejs.render($(user).html(), data));
                                        _this.operation();
                                    })
                                }
                            });
                    } else {
                        $('#img').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                        $('#pageContainer').hide()
                    }
                })
            })
            $('.but_query').click()
        },
        parameter: function (PageIndex) {//参数
            var phone = $('#phone').val();

            return {
                Mobile: phone,
                PageIndex: PageIndex,
                pageSize: 20
            }
        },
        operation: function () {

            $('.GetInto_detail').off('click').on('click',function(){
                var that = $(this);
                var UserId = that.parents('tr').attr('UserId');
                window.open('/JointManager/AdvertiserDetail.html?UserId='+UserId);
            })

        }
    }
    var user = '#Operate';//广告运营
    if (CTLogin.RoleIDs == 'SYS005RL00019') {
        user = '#Supertube';
    }else if(CTLogin.RoleIDs == 'SYS005RL00021'){
        user = '#Operate';//广告运营
    }
    var advertisersubList = new AdvertiserSubList(user);
})