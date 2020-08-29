$(function () {

    function PictureList() {
        // 触发搜索
        this.search();
        // 本地上传
        this.localupload()
    };
    PictureList.prototype={
        constructor: PictureList,
        // 上传图片
        uploadImg: function (id, img, imgerr, bigImg) {

            jQuery.extend({
                    evalJSON: function (strJson) {
                        if ($.trim(strJson) == '')
                            return '';
                        else
                            return eval("(" + strJson + ")");
                    }
                });
            function getCookie(name) {
                var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
                if (arr = document.cookie.match(reg))
                    return unescape(arr[2]);
                else
                    return null;
            }

            function escapeStr(str) {
                return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
            }

            // function postTest() {
            //     // setAjax({
            //     //     url: ' /api/Authorize/GetMenuInfo',
            //     //     type: 'get'
            //     // }, null);
            //     $.ajax({
            //         type: "get",
            //         url: " /api/Authorize/GetMenuInfo",
            //         //contentType: 'application/json',
            //         dataType: 'json',
            //         xhrFields: {
            //             withCredentials: true
            //         },
            //         crossDomain: true,
            //         //data: JSON.stringify({ ModuleIDs: "Jim,dafd,a,asd,fasd" }),
            //         success: function (data, status) { }
            //     });
            // }
            //
            // function disableConfirmBtn() { $('#btnConfirm').attr('disabled', 'disabled'); }
            // function enableConfirmBtn() { $('#btnConfirm').removeAttr('disabled'); }
            //
            // var uploadSuccess = true;
            $(document).ready(function () {

                // $('#divPopLayer').unbind('click').bind('click', function () {
                //     $.openPopupLayer({
                //         name: "popLayerDemo",
                //         url: "/弹出层页面.html?r=" + Math.random(),
                //         error: function (dd) { alert(dd.status); }
                //     });
                //
                // });
                var arr=[];
                // $('#' + id).off('click').on('click',function () {
                //     arr=[];
                // })

                $('#' + id).uploadify({
                    'auto': true,
                    'multi': true,
                    'swf': '/Js/uploadify.swf?_=' + Math.random(),
                    'uploader': '/AjaxServers/UploadFile.ashx',
                    'buttonText': '本地上传',
                    // 'buttonClass': 'button_upload',
                    'width': 80,
                    'height': 25,
                    'fileTypeDesc': '支持格式:xls,jpg,jpeg,png.gif',
                    'fileTypeExts': '*.jpg;*.jpeg;*.png;*.gif',
                    fileSizeLimit: '5MB',
                    // 'fileCount': 5,
                    'queueSizeLimit': 999,
                    queueID: 'imgShow',
                    'scriptAccess': 'always',
                    'overrideEvents': ['onDialogClose'],
                    'formData': {
                        Action: 'BatchImport',
                        LoginCookiesContent: escapeStr(getCookie('ct-uinfo')),
                        IsGenSmallImage: 0
                    },
                    onUploadStart:function () {
                        // arr=[];
                    },
                    'onQueueComplete': function (event, data) {
                        //enableConfirmBtn();
                        $.ajax({
                            url: '/api/WeChatEditor/InsertPictrues',
                            type: 'post',
                            data: {
                                PicUrls: arr
                            },
                            dataType: 'json',
                            xhrFields: {
                                withCredentials: true
                            },
                            crossDomain: true,
                            async:false,
                            success: function (data) {
                                // console.log(data);
                                $('#Upload_zhong').hide();
                                arr=[]
                            }
                        })
                        layer.msg('上传成功',{time:800},function () {
                            $('.but_query').click();
                        });

                    },
                    'onQueueFull': function () {
                        alert('您最多只能上传1个文件！');
                        return false;
                    },
                    //检测FLASH失败调用
                    'onFallback': function () {
                        $('#' + imgerr).html('您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。');
                    },
                    //上传成功后返回的信息
                    'onUploadSuccess': function (file, data, response) {
                        if (response == true) {
                            $('#medialogo_Prompt').hide();
                            var json = $.evalJSON(data);
                            // console.log(json);//文件数据
                            arr.push(json.Msg);
                            $('#Upload_zhong').show();
                            // $.ajax({
                            //     url: ' /api/WeChatEditor/InsertPictrues',
                            //     type: 'post',
                            //     data: {
                            //         PicUrls: json.Msg
                            //     },
                            //     dataType: 'json',
                            //     xhrFields: {
                            //         withCredentials: true
                            //     },
                            //     crossDomain: true,
                            //     async:false,
                            //     success: function (data) {
                            //         // console.log(data);
                            //     }
                            // })
                        }
                    },
                    'onProgress': function (event, queueID, fileObj, data) {
                    },
                    'onUploadError': function (event, queueID, fileObj, errorObj) {
                        //enableConfirmBtn();
                    },
                    'onSelectError': function (file, errorCode, errorMsg) {
                        //if(this.queueData.filesErrored>0){alert(this.queueData.errorMsg);}
                        if (errorCode == SWFUpload.UPLOAD_ERROR.FILE_CANCELLED
                            || errorCode == SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED) {
                            return false;
                        }
                        switch (errorCode) {
                            case -100:
                                $('#' + imgerr).html('<img src="/images/icon21.png">上传图片数量超过1个').css('color', "red");
                                break;
                            case -110:
                                $('#' + imgerr).html('<img src="/images/icon21.png">上传图片大小应小于5MB').css('color', "red");
                                break;
                            case -120:

                                break;
                            case -130:
                                $('#' + imgerr).html('<img src="/images/icon21.png">上传图片类型不正确').css('color', "red");
                                break;
                        }
                    },
                });
            });
        },
        // 搜索
        search:function () {
            var _this=this;
            // 点击搜索
            $('.but_query').off('click').on('click',function () {
                setAjax({
                    url:'/api/WeChatEditor/SelectPictrues',
                    type:'get',
                    data:_this.parameter(1)
                },function (data) {
                    console.log(data);
                    $('.pic_display').html(ejs.render($('#picture').html(), data));
                    // 如果数据为0显示图片
                    if (data.Result.TotalCount != 0) {
                        //分页
                        $("#pageContainer").pagination(
                            data.Result.TotalCount,
                            {
                                items_per_page: 30, //每页显示多少条记录（默认为20条）
                                callback: function (currPage, jg) {
                                    setAjax({
                                        url:'/api/WeChatEditor/SelectPictrues',
                                        type:'get',
                                        data:_this.parameter(currPage)
                                    }, function (data) {
                                        $('.pic_display').html(ejs.render($('#picture').html(), data));
                                        _this.operation()
                                    })
                                }
                            });
                        $('#pageContainer').show();
                    } else {
                        $('.pic_display').html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        $('#pageContainer').hide();
                    };

                    _this.operation()
                });
                $('#allbox').prop('checked', false);
            });
            $('.but_query').click();
        },
        // 参数
        parameter:function (currPage) {
            var content=$('#content').val();
            return {
                PicName:content,
                PageSize:30,
                PageIndex:currPage,
                WxID:-1
            }
        },
        // 操作
        operation:function () {
            /*全选*/
            $('.picture_ma').on('change', '#allbox', function () {
                if ($(this).prop('checked')) {
                    $('.onebox').prop('checked', true);
                } else {
                    $('.onebox').prop('checked', false);
                }
            });
            $('.picture_ma').on('change', '.onebox', function () {
                if ($('.onebox').length == $("input[name='checkbox']:checked").length) {
                    $('#allbox').prop('checked', true);
                } else {
                    $('#allbox').prop('checked', false);
                }
            });
            // 删除
            $('#delete').off('click').on('click',function () {
                if($("input[name='checkbox']:checked").length>0){
                    var arr=[];
                    for (var i=0;i<$("input[name='checkbox']:checked").length;i++){
                        arr.push($($("input[name='checkbox']:checked")[i]).attr('PicID')-0)
                    }
                    layer.confirm('确定删除吗？', {
                        time: 0 //不自动关闭
                        , btn: ['确定', '取消']
                        , yes: function (index) {
                            setAjax({
                                url:'/api/WeChatEditor/DeletePictruesByPicIDs',
                                type:'post',
                                data:{
                                    PicIDs:arr
                                }
                            },function (data) {
                                if(data.Status==0){
                                    layer.close(index);
                                    layer.msg('删除成功', {time: 400});
                                    $('.but_query').click();
                                }else {
                                    layer.close(index);
                                    layer.msg('删除失败', {time: 400});
                                }

                            })

                        }
                    });
                }else {
                    layer.msg('请选择要删除的图片')
                }

            })
            // 删除提示
            $('#deleteshow').mousemove(function () {
                $(this).find('span').show()
            }).mouseout(function () {
                $(this).find('span').hide()
            });
            // 编辑
            $('.edit_span').off('click').on('click',function () {
                var flat=$(this);
                console.log($(this).parent().hide().next().show());
                $(this).parent().hide().next().show().find('input').focus().off('blur').on('blur',function () {
                    var _this=$(this);
                    var PicName=$(this).val();
                    var PicID=$(this).attr('PicID')-0;
                    setAjax({
                        url:'/api/WeChatEditor/UpdatePictruesByPicID',
                        type:'post',
                        data:{
                            PicID:PicID,
                            PicName:PicName
                        }
                    },function (data) {
                        console.log(data);
                        if (data.Status==0){
                            flat.html($.trim(PicName)).parent().show();
                            _this.val($.trim(PicName)).parent().hide();
                        }
                    })
                });
            })
        },
        // 本地上传
        localupload:function () {
            this.uploadImg('upload');
        }
    }
    var pictrelist=new PictureList();
})