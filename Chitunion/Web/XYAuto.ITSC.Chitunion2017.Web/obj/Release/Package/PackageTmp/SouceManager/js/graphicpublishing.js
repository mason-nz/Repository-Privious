//获取Url上的参数
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {

            theRequest[strs[i].split("=")[0]] = strs[i].split("=")[1];
        }
    }
    return theRequest;
};
$(function () {
    var zhixing;
    function GraphicPublishing() {
        // 图文渲染
        this.graphicrendering();
        // 同步信息
        this.synchronousgraphictext();
    }

    GraphicPublishing.prototype = {
        constructor: GraphicPublishing,
        // 图文渲染
        graphicrendering: function () {
            setAjax({
                url: '/api/WeChatEditor/SelectGroupInfoByGroupID',
                type: 'get',
                data: {
                    GroupID: GetRequest().GroupID
                }
            }, function (data) {
                $('#Graphic_rendering').html(ejs.render($('#graphicrendering').html(), data));
            })
        },
        // 同步信息
        synchronousgraphictext: function () {
            var _this = this;
            setAjax({
                url: '/api/WeChatEditor/SelectWxStatusInfoByGroupID',
                type: 'get',
                data: {
                    GroupID: GetRequest().GroupID,
                    InputType: 0
                }
            }, function (data) {

                $('#graphics').html(ejs.render($('#graphics_add').html(), data));
                _this.operation(_this);
                if(data.Result.ReturnType==1){
                    $('.mt20').click();
                    zhixing=true;
                }
            })
        },
        // 操作
        operation: function (_this) {
            var _this=_this;
            // 选中状态按钮变红
            $('.onebox').off('change').on('change', function () {
                if ($("input[name='checkbox']:checked").length) {
                    $('.mt20 a').attr('class', 'but_data')
                } else {
                    $('.mt20 a').attr('class', 'but_data_h')
                }
            });
            // 同步
            $('#mt20').off('click').on('click', function () {
                var arr=[];
                $('.onebox').each(function () {
                   if($(this).prop('checked')==true){
                       arr.push($(this).attr('ArticleID')-0);
                   }
                })
                if($(this).find('a').attr('class')=='but_data'){
                    setAjax({
                        url: '/api/Article/UploadWeixinArticleGroup',
                        type: 'post',
                        data: {
                            GroupID: GetRequest().GroupID,
                            UploadWxIDs:arr
                        }

                    },function (data) {
                        if(data.Status!=0){
                            layer.msg(data.Message);
                            return false;
                        }
                        // 给url添加参数
                        // function ChangeParam(name, value) {
                        //     var url = window.location.href;
                        //     var newUrl = "";
                        //     var reg = new RegExp("(^|)" + name + "=([^&]*)(|$)");
                        //     var tmp = name + "=" + value;
                        //     if (url.match(reg) != null) {
                        //         newUrl = url.replace(eval(reg), tmp);
                        //     }
                        //     else {
                        //         if (url.match("[\?]")) {
                        //             newUrl = url + "&" + tmp;
                        //         }
                        //         else {
                        //             newUrl = url + "?" + tmp;
                        //         }
                        //     }
                        //     location.href = newUrl;
                        // }
                        // ChangeParam('a',1);
                        // $('#tongbu').html('同步中');
                        setAjax({
                            url: '/api/WeChatEditor/SelectWxStatusInfoByGroupID',
                            type: 'get',
                            data: {
                                GroupID: GetRequest().GroupID,
                                InputType: 1
                            }
                        }, function (data) {
                            $('#graphics').html(ejs.render($('#graphics_add').html(), data));
                            // _this.operation(_this);
                            // $('#tongbu').html('同步中');
                        })
                        // 计时器
                        var a=setInterval(function () {
                            setAjax({
                                url: '/api/WeChatEditor/SelectWxStatusInfoByGroupID',
                                type: 'get',
                                data: {
                                    GroupID: GetRequest().GroupID,
                                    InputType: 1
                                }
                            }, function (data) {
                                $('#graphics').html(ejs.render($('#graphics_add').html(), data));
                                console.log(1);
                                // // $('#tongbu').html('同步中');
                                // if(data.Result.ReturnType==0){
                                //     // 同步信息
                                //     _this.synchronousgraphictext();
                                //     // $('#tongbu').html('立即同步');
                                // }
                                if(data.Result.ReturnType==1&&data.Result.IsComplete==1){
                                    // 同步信息
                                    // _this.synchronousgraphictext();
                                    _this.operation(_this);
                                    clearInterval(a);
                                    // $('#tongbu').html('立即同步');
                                }
                            })
                        },3000);

                    });
                }
                // if(GetRequest().a==1||zhixing){
                //     // 计时器
                //     var b=setTimeout(function () {
                //         setAjax({
                //             url: '/api/WeChatEditor/SelectWxStatusInfoByGroupID',
                //             type: 'get',
                //             data: {
                //                 GroupID: GetRequest().GroupID,
                //                 InputType: 0
                //             }
                //         }, function (data) {
                //             $('#graphics').html(ejs.render($('#graphics_add').html(), data));
                //             if(data.Result.ReturnType==0){
                //                 // 同步信息
                //                 _this.synchronousgraphictext();
                //             }
                //         })
                //     },3000)
                // }
            });
            // // 增加授权账号
            // $('#Addauthorization').off('submit').on('submit', function () {
            //     $('#form2').attr("action", ' /wx/WeChat.ashx?m=oauth');
            // })
            // 有账号
            $('#form3').html('<input type="hidden" name="GroupID" value="'+GetRequest().GroupID+'"> <span class="red fb">!</span> 未授权素材管理权限，<input type="submit" style="cursor: pointer;color: #f77777;background: #fff;border: 0;font-size: 14px;height: 19px;margin-top: -3px;" class="red" id="Authorized" value="马上授权"><input type="hidden" name="Type" value="38001">')
            // 无账号
            $('#form1').html('<span class="red fb" >!</span> 无授权账号，<input type="submit" style="cursor: pointer;color: #f77777;background: #fff;border: 0;font-size: 14px;height: 19px;margin-top: -3px;" id="Unauthorized"  class="red" value="马上授权"><input type="hidden" name="GroupID" value="'+GetRequest().GroupID+'"><input type="hidden" name="Type" value="38001">')
            // 关闭
            $('.close').off('click').on('click',function () {
                $('.situation').hide();
            })
        }
    }
    new GraphicPublishing()
})