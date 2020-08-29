<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JsJDKTest.aspx.cs" Inherits="XYAuto.BUOC.Chitunion2017.WebAPIWeChat.JsJDKTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type ="text/javascript" src="Scripts/jquery-1.10.2.min.js"></script>
      <script type ="text/javascript" src="https://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script type="text/javascript">
        $(function () {
            var APPID = "";
            $.ajax({
                type: "get",
                url: "http://wxtest-ct.qichedaquan.com/api/Weixin/WeixinJSSDK/GetInfo",
                data: "url=" + window.location.href,
                dataType: "JSON",
                success: function (data) {
                    APPID = data.Result.AppId;
                    wx.config({
                        debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                        appId: APPID, // 必填，公众号的唯一标识
                        timestamp: data.Result.Timestamp, // 必填，生成签名的时间戳
                        nonceStr: data.Result.NonceStr, // 必填，生成签名的随机串
                        signature: data.Result.Signature,// 必填，签名
                        jsApiList: [
                                'checkJsApi',
                                'onMenuShareTimeline',
                                'onMenuShareAppMessage',
                                'onMenuShareQQ',
                                'onMenuShareWeibo',
                                'hideMenuItems',
                                'showMenuItems',
                                'hideAllNonBaseMenuItem',
                                'showAllNonBaseMenuItem',
                                'translateVoice',
                                'startRecord',
                                'stopRecord',
                                'onRecordEnd',
                                'playVoice',
                                'pauseVoice',
                                'stopVoice',
                                'uploadVoice',
                                'downloadVoice',
                                'chooseImage',
                                'previewImage',
                                'uploadImage',
                                'downloadImage',
                                'getNetworkType',
                                'openLocation',
                                'getLocation',
                                'hideOptionMenu',
                                'showOptionMenu',
                                'closeWindow',
                                'scanQRCode',
                                'chooseWXPay',
                                'openProductSpecificView',
                                'addCard',
                                'chooseCard',
                                'openCard'
                        ] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2。详见：http://mp.weixin.qq.com/wiki/7/aaa137b55fb2e0456bf8dd9148dd613f.html
                    });

                    wx.error(function (res) {
                        console.log(res);
                        alert('验证失败');
                    });

                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
      
    <div>

    </div>

          
    </form>
</body>
</html>
