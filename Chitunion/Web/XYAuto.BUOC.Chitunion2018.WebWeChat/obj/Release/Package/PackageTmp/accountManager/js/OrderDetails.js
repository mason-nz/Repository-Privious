$(function () {

    // 判断无网络
    wx.ready(function () {
        wx.getNetworkType({
            success: function (res) {
                var networkType = res.networkType; // 返回网络类型2g，3g，4g，wifi
                $('.netOk').show().siblings('.no_data').remove();
            },
            fail:function () {
                $('.no_data').show().siblings('.netOk').remove();
                return false;
            }
        });
    })
    var app=new Vue({
        el:'#app',
        data:{
            // 订单号
            OrderId:'',
            // 订单名称
            OrderName:'',
            // 计费规则
            CPCUnitPrice:'',
            // 领取时间
            ReceiveTime:'',
            // 专属链接
            OrderUrl:'',
            // 订单收入
            List:[],
            // 汇总
            Extend:{}
        },
        created(){
            this.OrderDetails()
            // this.share()
        },
        methods:{
            //刷新
            shuaxin(){
                window.location.reload()
            },
            // 订单详情渲染
            OrderDetails(){
                var _this=this
                $.ajax({
                    url: public_url+'/api/Task/GetOrderInfo',
                    type: 'get',
                    dataType: 'json',
                    data: {
                        orderid:GetRequest().OrderId
                    },
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    success: function (data) {
                        if (data.Status == 0) {
                                // 订单号
                            _this.OrderId=data.Result.OrderId;
                                // 订单名称
                            _this.OrderName=data.Result.OrderName;
                                // 计费规则
                            _this.CPCUnitPrice = removePoint0(formatMoney(data.Result.CPCUnitPrice * 10, 1, ''), 1) + '毛/有效阅读';
                                // 领取时间
                            _this.ReceiveTime=data.Result.ReceiveTime;
                                // 专属链接
                            _this.OrderUrl=data.Result.OrderUrl;
                            // 订单收入
                            _this.List=data.Result.List
                            // 汇总
                            _this.Extend=data.Result.Extend
                            // 分享
                            var option = {
                                title: data.Result.TaskName, // 分享标题
                                desc: data.Result.Synopsis, // 分享描述
                                link: data.Result.OrderUrl, // 分享链接，该链接域名或路径必须与当前页面对应的公众号JS安全域名一致
                                imgUrl: data.Result.ImgUrl, // 分享图标
                                type: '', // 分享类型,music、video或link，不填默认为link
                                dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
                                success: function () {
                                    // 用户确认分享后执行的回调函数
                                    // _this.shareOK(datac.TaskId, data.Result.OrderUrl)
                                    alert('分享成功')
                                },
                                cancel: function () {
                                    // 用户取消分享后执行的回调函数
                                }
                            }
                            wx.ready(function () {
                                //隐藏所有传播类和复制链接菜单
                                wx.hideMenuItems({
                                    menuList: ['menuItem:copyUrl','menuItem:share:appMessage','menuItem:share:timeline','menuItem:share:qq','menuItem:share:weiboApp','menuItem:share:QZone','menuItem:share:facebook']  // 要隐藏的菜单项，只能隐藏“传播类”和“保护类”按钮，所有menu项见附录3
                                });
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
            // // 分享
            // share(){
            //     var _this=this;
            //     $.ajax({
            //         url:public_url+'/api/Task/GetOrderUrl',
            //         type: 'get',
            //         dataType: 'json',
            //         data: {
            //             TaskId:datac.TaskId
            //         },
            //         xhrFields: {
            //             withCredentials: true
            //         },
            //         crossDomain: true,
            //         success: function (data) {
            //             if(data.Status==0){
            //
            //
            //             }else {
            //                 layer.open({
            //                     content: data.Message
            //                     , skin: 'msg'
            //                     , time: 2 //2秒后自动关闭
            //                 });
            //             }
            //         }
            //     })
            //
            // },
            // shareOK(TaskId,OrderUrl){
            //     $.ajax({
            //         url:public_url+'/api/Task/SubmitOrder',
            //         type: 'get',
            //         dataType: 'json',
            //         data: {
            //             TaskId:TaskId,
            //             OrderUrl:OrderUrl
            //         },
            //         xhrFields: {
            //             withCredentials: true
            //         },
            //         crossDomain: true,
            //         success:function (data) {
            //             if (data.Status==0){
            //
            //             }else {
            //                 layer.open({
            //                     content: data.Message
            //                     , skin: 'msg'
            //                     , time: 2 //2秒后自动关闭
            //                 });
            //             }
            //         }
            //     })
            // }
        },
        filters: {
            Date: function (value) {
                if (!value) return ''
                value = new Date(Date.parse(value.replace(/-/g,  "/"))).Format('yyyy-MM-dd')
                return value
            }
        }
    })
})