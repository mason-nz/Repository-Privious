$(function () {
    // 判断无网络
    wx.ready(function () {
        wx.getNetworkType({
            success: function (res) {
                var networkType = res.networkType; // 返回网络类型2g，3g，4g，wifi
                $('.netOk').show().siblings('.no_data').remove();
            },
            fail: function () {
                $('.no_data').show().siblings('.netOk').remove();
                return false;
            }
        });
    })
    // console.log(GetRequest().MaterialUrl);
    // var data = JSON.parse(GetRequest().data)
    var app = new Vue({
        el: '#app',
        data: {
            // MaterialUrl: 'http://newscdn.chitunion.com/ct_m/20180123/74744.html',
            MaterialUrl: '',
            TaskId: GetRequest().TaskId
        },
        created() {
            this.share()
        },
        methods: {
            // 分享
            share() {
                var _this = this;
                $.ajax({
                    url: public_url + '/api/Task/GetOrderUrl',
                    type: 'get',
                    dataType: 'json',
                    data: {
                        // TaskId: data.TaskId
                        TaskId: GetRequest().TaskId
                    },
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    success: function (data) {
                        if (data.Status == 0) {
                            _this.MaterialUrl=data.Result.OrderUrl
                            // _this.MaterialUrl='http://newscdn.chitunion.com/ct_m/20180123/74744.html?utm_source=chitu&utm_term=bgL0IP0mHg'
                            var option = {
                                title: data.Result.TaskName, // 分享标题
                                desc: data.Result.Synopsis, // 分享描述
                                link: data.Result.OrderUrl, // 分享链接，该链接域名或路径必须与当前页面对应的公众号JS安全域名一致
                                // link:'http://newscdn.chitunion.com/ct_m/20180123/74744.html',
                                imgUrl: data.Result.ImgUrl, // 分享图标
                                type: '', // 分享类型,music、video或link，不填默认为link
                                dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                                success: function () {
                                    // 用户确认分享后执行的回调函数
                                    _this.shareOK(GetRequest().TaskId, data.Result.OrderUrl)
                                },
                                cancel: function () {
                                    // 用户取消分享后执行的回调函数
                                }
                            }
                            wx.ready(function () {
                                wx.onMenuShareTimeline(option);
                                wx.onMenuShareAppMessage(option);
                                wx.onMenuShareQQ(option);
                                wx.onMenuShareWeibo(option);
                                wx.onMenuShareQZone(option);
                            })
                        } else {
                            layer.open({
                                content: data.Message
                                , skin: 'msg'
                                , time: 2 //2秒后自动关闭
                            });
                        }
                    }
                })

            },
            shareOK(TaskId, OrderUrl) {
                $.ajax({
                    url: public_url + '/api/Task/SubmitOrder',
                    type: 'get',
                    dataType: 'json',
                    data: {
                        TaskId: TaskId,
                        OrderUrl: OrderUrl
                    },
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    success: function (data) {
                        if (data.Status == 0) {

                        } else {
                            layer.open({
                                content: data.Message
                                , skin: 'msg'
                                , time: 2 //2秒后自动关闭
                            });
                        }
                    }
                })
            }
        }
    })
})