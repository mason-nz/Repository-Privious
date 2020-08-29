$(function () {
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
    }


    var Escape = decodeURIComponent(GetRequest().WxNumber);
    // if (GetRequest().IsAuditPass == undefined) {
    //     if (GetRequest().WxID > 0) {
    //         $.ajax({
    //             url: '/api/Media/CheckCanAdd?v=1_1',
    //             type: 'get',
    //             data: {WxID: GetRequest().WxID},
    //             dataType: 'json',
    //             async: false,
    //             xhrFields: {
    //                 withCredentials: true
    //             },
    //             crossDomain: true,
    //             success: function (data) {
    //                 if (!data.Result) {
    //                     layer.alert('该账号已添加，请勿重复操作', function () {
    //                         window.location = '/mediamanager/mediawechatlist_new.html';
    //                     })
    //                 }
    //             }
    //         })
    //
    //     }
    //     ;
    //     if (GetRequest().WxID == 0) {
    //         layer.alert('账号不存在，请确认url是否正确', function () {
    //             window.location = '/mediamanager/mediawechatlist_new.html';
    //         })
    //     }
    //     ;
    //     if (GetRequest().WxID == -1) {
    //         layer.alert('请确认系统生成的验证码是否与文章里的一致', function () {
    //             window.location = '/mediamanager/mediawechatlist_new.html';
    //         })
    //     }
    // }
    if (GetRequest().AuthType == 38001) {
        $('#Authorprompt').show();
    }
    var nan = '', nv = '',fsfbqyjt=0;

    //添加或编辑媒体
    function Addeditmedia() {
        var _this = this;
        // 渲染

        this.addandedit(_this);
        if (CTLogin.RoleIDs == "SYS001RL00001" || CTLogin.RoleIDs == "SYS001RL00004") {
            $('#Submit3').hide()
        }
    }

    Addeditmedia.prototype = {
        constructor: Addeditmedia,
        /*============公共方法start============*/
        // 获取url？后面的参数
        GetRequest: function () {
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
        },
        // 执行正则表达式
        executeRegular: function (val, regular) {
            return regular.test(val);
        },
        // 上传图片
        uploadImg: function (id, img, imgerr, bigImg) {
            jQuery.extend(
                {
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

            // 鼠标移入显示大图
            function showImg() {
                $(".uploadBigimg").each(function () {
                    $(this).mousemove(function () {
                        if ($.trim($(this).attr("src")) != "") {
                            $(this).next().css("display", "inline");
                        }
                    }).mouseout(function () {
                        $(this).next().css("display", "none");
                    })
                })
            }

            // function postTest() {
            //     // setAjax({
            //     //     url: '/api/Authorize/GetMenuInfo',
            //     //     type: 'get'
            //     // }, null);
            //     $.ajax({
            //         type: "get",
            //         url: "/api/Authorize/GetMenuInfo",
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

                $('#' + id).uploadify({
                    'auto': true,
                    'multi': false,
                    'swf': '/Js/uploadify.swf?_=' + Math.random(),
                    'uploader': 'http://www.chitunion.com/AjaxServers/UploadFile.ashx',
                    'buttonImage': '/images/icon20.png',
                    'width': 27,
                    'height': 25,
                    'fileTypeDesc': '支持格式:xls,jpg,jpeg,png.gif',
                    'fileTypeExts': '*.xls;*.jpg;*.jpeg;*.png;*.gif',
                    fileSizeLimit: '2MB',
                    'fileCount': 1,
                    'queueSizeLimit': 1,
                    queueID: 'imgShow',
                    'scriptAccess': 'always',
                    'overrideEvents': ['onDialogClose'],
                    'formData': {
                        Action: 'BatchImport',
                        LoginCookiesContent: escapeStr(getCookie('ct-uinfo')),
                        IsGenSmallImage: 1
                    },
                    'onQueueComplete': function (event, data) {
                        //enableConfirmBtn();
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
                            var json = $.evalJSON(data);
                            console.log(json);//文件数据
                            //小图

                            $('#' + img).attr('src', json.Msg.split('|')[1]);
                            //大图

                            $("#" + bigImg).attr("src", json.Msg.split('|')[0]);
                            //上传成功的提示信息
                            $("#" + imgerr).html('上传成功！').css('color', "#666");
                            //上传成功后警告信息隐藏
                            $("#" + imgerr).next().css("display", "none");
                            showImg();
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
                                $('#' + imgerr).html('<img src="/images/icon21.png">上传图片大小应小于2MB').css('color', "red");
                                break;
                            case -120:

                                break;
                            case -130:
                                $('#' + imgerr).html('<img src="/images/icon21.png">上传图片类型不正确').css('color', "red");
                                break;
                        }
                    }
                });
            });
        },
        // ajax请求
        myAjax: setAjax,
        // 省级联动
        JSonData: JSonData,
        /*============公共方法end============*/
        //添加或编辑

        addandedit: function (_this) {
            var _this = _this;
            // 渲染个人信息

            this.renderInfo(_this);

            // 区域媒体
            this.quyumeiti(_this);
            // 覆盖地区
            this.fgdq();
            // 粉丝数
            this.Numberoffans()
            // 粉丝数截图
            this.fansscreenshot()
            // 选择分类
            this.Selectclassify()
            //粉丝数分布区域
            this.fansdistri();
            // 分布区域截图
            this.fansdistriscreenshot();
            // 粉丝性别比例
            this.fansex();
            // 性别比例截图
            this.genderscreenshot();
            // 所在区域
            this.locationarea();

            // 微信号
            this.microsignal();
            // 微信名称
            this.WeChatName();
            // 头像
            this.Headportrait();
            // 二维码
            this.QRcode();

            // 下单备注
            this.Ordermemo();
            // 代理授权资质
            this.authorization();
            // 营业执照
            this.Businesslicense();
        },
        // 渲染个人信息

        renderInfo: function (_this) {
            this.information(_this)
        },
        // 返回个人信息

        information: function (_this) {
            var _this = _this;
            var operatetype;
            if (_this.GetRequest().AuthType == undefined) {
                operatetype = 2
            } else {
                operatetype = 1
            }
            if (_this.GetRequest().AuthType == 38003 || _this.GetRequest().wxae == 1) {
                $('#headerinfo').hide();
                $('#Microsignalfill').show()
                $('#Headportrait').show()
                $('#QRcode').show()
                $('#wechatname').show()
                $('#Prompt').hide()
                $('#mediaLevel').show()
                $('#weChatConfirm').show()
                $('#sign').show();
                $('#PromptFans').html('');
                if (_this.GetRequest().WxNumber != undefined) {
                    $('#Microsignalfilling1').val(Escape)
                        .prop('disabled', true)
                }
                $('#Submit3').show();
            }
            if (_this.GetRequest().AuthType == 38004 || _this.GetRequest().OAuthType == 38004){
                $('#QRcode').show()
            }
            if (_this.GetRequest().WxID == 0) {
                $('#headerinfo').hide();
                $('#Microsignalfill').show()
                $('#wechatname').show()
                $('#Headportrait').show()
            }
            var candata = {
                businesstype: 14001,
                recId: _this.GetRequest().WxID,
                operateType: operatetype
            }


            candata.IsAuditPass = _this.GetRequest().IsAuditPass == 43002 ? true : false;
            // ae不显示
            if (CTLogin.RoleIDs != "SYS001RL00003") {
                $('#fansscreenshotnumber').hide();
                $('#fansscreenshot').hide();
                $('#genderscreen').hide();
            }
            $('#Ordermemo').show();
            if (_this.GetRequest().AuthType == 38004 || _this.GetRequest().OAuthType == 38004) {
                // 资质信息
                $('#authorization').show();
                $('#Enterprisename').show();
                $('#Businesslicense').show();
            }
            if (_this.GetRequest().AuditStatus == 43004) {
                candata.IsAuditPass = true;
                candata.recId = _this.GetRequest().WxID - 0;
                candata.operateType = 2;
            }
            _this.myAjax({
                url: 'http://www.chitunion.com/api/media/GetInfo?v=1_1',
                type: 'get',
                data: candata
            }, function (data) {


                // 编辑
                if (operatetype == 2) {
                    if(CTLogin.RoleIDs=='SYS001RL00003'){
                        if(data.Result.AuditStatus!=43003){
                            layer.msg('该微信账号当前不可编辑',{time:1000},function () {
                                window.location="/mediamanager/mediawechatlist_new.html"
                            });
                            // return  false;
                        }
                    }

                    /*======个人信息开始======*/
                    // 判断个人信息
                    if (data.Result.Name == '') {
                        data.Result.Name = '暂无'
                    }
                    var Number = data.Result.Number;
                    // if (Number == '') {
                    //     Number = data.Result.MediaID
                    // }
                    if (Number == '') {
                        Number = '暂无'
                    }
                    $('.wechat_name').html('<div>微信名称：<span>' + data.Result.Name + '</span></div><div>微信帐号：<span>' + Number + '</span></div>')
                    $('.frame_n img').attr('src', data.Result.HeadIconURL);
                    /*=======粉丝数======*/
                    $('#Fans').val(data.Result.FansCount);
                    nan = data.Result.FansMalePer;
                    nv = data.Result.FansFemalePer;
                    console.log(nan, nv);
                    // 粉丝数截图
                    if (data.Result.FansCountURL ? data.Result.FansCountURL : '' != '') {

                        $('#headimgUploadFile').attr('src', data.Result.FansCountURL);
                        $('#headBigImg').attr('src', data.Result.FansCountURL);
                        $('#headimgUploadFile').off('mouseover').on('mouseover', function () {
                            $('#headBigImg').parent().show();
                        })
                        $('#headimgUploadFile').off('mouseout').on('mouseout', function () {
                            $('#headBigImg').parent().hide();
                        })
                    } else {
                        if (CTLogin.RoleIDs == "SYS001RL00003") {
                            $('#fansscreenshotnumber').hide();
                            $('#Fans').off('keyup').on('keyup', function () {
                                $('#fansscreenshotnumber').show();
                                if ($('#Fans').val() == data.Result.FansCount) {
                                    $('#fansscreenshotnumber').hide();
                                }
                            })
                        }
                    }
                    /*=======粉丝比例========*/
                    $('#male').val(data.Result.FansMalePer == 0 ? '' : data.Result.FansMalePer);
                    if (data.Result.FansMalePer > 0) {
                        $('#genderscreen').show()
                    }
                    $('#female').val(data.Result.FansFemalePer == 0 ? '' : data.Result.FansFemalePer);
                    if (data.Result.FansFemalePer > 0) {
                        $('#genderscreen').show()
                    }
                    // 粉丝比例截图
                    if (data.Result.FansSexScaleUrl ? data.Result.FansSexScaleUrl : '' != '') {
                        $('#headimgUploadFile3').attr('src', data.Result.FansSexScaleUrl);
                        $('#headBigImg3').attr('src', data.Result.FansSexScaleUrl);
                        $('#headimgUploadFile3').off('mouseover').on('mouseover', function () {
                            $('#headBigImg3').parent().show();
                        }).off('mouseout').on('mouseout', function () {
                            $('#headBigImg3').parent().hide();
                        })
                    } else {
                        $('#genderscreen').hide();
                    }
                    // 常见分类
                    _this.myAjax({
                        url: '/api/DictInfo/GetDictInfoByTypeID',
                        type: 'get',
                        data: {
                            typeID: 47
                        }
                    }, function (dat) {

                        var arr = [];
                        console.log(data.Result.CommonlyClass);
                        data.Result.CommonlyClass ? data.Result.CommonlyClass : data.Result.CommonlyClass = [];
                        for (var i = 0; i < data.Result.CommonlyClass.length; i++) {
                            for (var j = 0; j < dat.Result.length; j++) {
                                if (data.Result.CommonlyClass[i] == dat.Result[j].DictId) {
                                    arr.push({name: dat.Result[j].DictName, id: dat.Result[j].DictId, sub: j})
                                }
                            }
                        }
                        if (arr.length > 0) {
                            var a = '<li class="ins_a">&nbsp;</li>';
                            var b = 0;
                            for (var i = 0; i < arr.length; i++) {
                                a += '<li name=' + arr[i].sub + ' dictid=' + arr[i].id + ' mainclass="-0"><div class="classification">' + arr[i].name + '<span style="display: none"><img src="/ImagesNew/icon50.png"/></span><em class="emclass" style="display:none;position: absolute;bottom: 0;right: 0; z-index: 0;"><img src="/ImagesNew/icon85.png"></em></div></li>';
                                b++;
                            }
                            a += '<div class="clear"></div>';
                            // 判断a是否进入循环，否的话，a就为空
                            if (b < 1) {
                                a = '';
                            }
                            $('#ification').html(a);
                            // 鼠标经过分类显示和隐藏关闭
                            $('#ification li').off('mouseover').on('mouseover', function () {
                                $(this).find('span').show();
                            }).off('mouseout').on('mouseout', function () {
                                $(this).find('span').hide();
                            }).find('span').off('click').on('click', function () {//点击span关闭
                                // 让弹窗里的分类的选中移除
                                $('.familiar ul li').eq($(this).parent().parent().attr('name')).attr('class', '');
                                // 移除当前li
                                $(this).parent().parent().remove();
                                // 判断是否还有选择的分类，根据span判断
                                if ($('#ification li span').length == 0) {
                                    $('#ification').html('');
                                }
                                //设置主分类

                                $('.emclass').hide().parents('li').attr('mainclass', '-0');
                                $('.emclass').eq(0).show().parents('li').attr('mainclass', '-1');
                                $('.classification').off('click').on('click', function () {
                                    $('.emclass').hide().parents('li').attr('mainclass', '-0');
                                    $(this).find('.emclass').show().parents('li').attr('mainclass', '-1');
                                });
                            });
                            $('.emclass').hide().parents('li').attr('mainclass', '-0');
                            $('.emclass').eq(0).show().parents('li').attr('mainclass', '-1');
                            $('.classification').off('click').on('click', function () {
                                $('.emclass').hide().parents('li').attr('mainclass', '-0');
                                $(this).find('.emclass').show().parents('li').attr('mainclass', '-1');
                            });

                        }

                    }, false)

                    // 粉丝分布区域

                    if (data.Result.FansArea != null) {
                        if (data.Result.FansArea.length > 0) {
                            // 初始渲染
                            if ($('#Renfansdistri li').attr('class') != 'ins_a') {
                                $('#Renfansdistri').html('<li class="ins_a">&nbsp;</li>')
                            }
                            // 渲染
                            $(data.Result.FansArea).each(function () {
                                $('#Renfansdistri').append('<li provinceid=' + $(this)[0].ProvinceID + ' province=' + $(this)[0].ProvinceName + ' provinceper=' + $(this)[0].UserScale + '><div class="classification">' + $(this)[0].ProvinceName + ' ' + $(this)[0].UserScale + '%<span style="display: none"><img src="/ImagesNew/icon50.png"/></span></div></li>')
                            })
                            $('#Renfansdistri').append('<div class="clear"></div>')
                            // 显示粉丝数截图
                            $('#fansscreenshot').show();
                            // 鼠标经过分类显示和隐藏关闭
                            $('#Renfansdistri li').off('mouseover').on('mouseover', function () {
                                $(this).find('span').show();
                            }).off('mouseout').on('mouseout', function () {
                                $(this).find('span').hide();
                            }).find('span').off('click').on('click', function () {//点击span关闭
                                // 移除当前li
                                $(this).parent().parent().remove();
                                // 判断是否还有选择的分类，根据span判断
                                if ($('#Renfansdistri li span').length == 0) {
                                    $('#Renfansdistri').html('');
                                    // 判断隐藏粉丝数截图
                                    $('#fansscreenshot').hide();
                                }
                            })
                        }
                        ;
                    }

                    // 分布区域截图
                    if (data.Result.FansAreaShotUrl ? data.Result.FansAreaShotUrl : '' != '') {
                        $('#headimgUploadFile2').attr('src', data.Result.FansAreaShotUrl);
                        $('#headBigImg2').attr('src', data.Result.FansAreaShotUrl);
                        $('#headimgUploadFile2').off('mouseover').on('mouseover', function () {
                            $('#headBigImg2').parent().show();
                        }).off('mouseout').on('mouseout', function () {
                            $('#headBigImg2').parent().hide();
                        })
                    } else {
                        $('#fansscreenshot').hide();
                    }
                    // 所在地区
                    // 省
                    if (data.Result.ProvinceID != -2 ? data.Result.ProvinceID : '' != '') {
                        $('#locationArea1 option').each(function () {
                            if ($(this).attr('value') == data.Result.ProvinceID) {
                                $(this).prop('selected', true);
                                $('#locationArea1').change();
                            }
                        })
                    }
                    // 城市
                    $('#locationArea1').off('change').on('change', function () {
                        var City1 = '', City2 = '<option value="-2">城市</option>'
                        $($(_this.JSonData.masterArea)[$('#locationArea1 option:checked').attr('i')].subArea).each(function (i) {
                            City1 += '<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>'
                        })
                        $('#locationArea2').html(City2 + City1)
                    });
                    // 城市
                    if (data.Result.CityID ? data.Result.CityID : '' != '') {
                        $('#locationArea2 option').each(function () {
                            if ($(this).attr('value') == data.Result.CityID) {
                                console.log($(this));
                                $(this).prop('selected', true)
                            }
                        })
                    }
                    // 微信号
                    if (data.Result.Number != '') {
                        $('#Microsignalfilling1').val(data.Result.Number).prop('disabled', true)
                    }
                    // 微信名称
                    if (data.Result.Name != '') {
                        $('#wechatname1').val(data.Result.Name)
                    }
                    // 头像
                    if (data.Result.HeadIconURL ? data.Result.HeadIconURL : '' != '') {
                        $('#headimgUploadFile-Headportrait').attr('src', data.Result.HeadIconURL);
                        $('#headBigImg-Headportrait').attr('src', data.Result.HeadIconURL);
                        $('#headimgUploadFile-Headportrait').off('mouseover').on('mouseover', function () {
                            $('#headBigImg-Headportrait').parent().show();
                        }).off('mouseout').on('mouseout', function () {
                            $('#headBigImg-Headportrait').parent().hide();
                        })
                    }
                    ;
                    // 二维码
                    if (data.Result.TwoCodeURL ? data.Result.TwoCodeURL : '' != '') {
                        $('#headimgUploadFile-QRcode').attr('src', data.Result.TwoCodeURL);
                        $('#headBigImg-QRcode').attr('src', data.Result.TwoCodeURL);
                        $('#headimgUploadFile-QRcode').off('mouseover').on('mouseover', function () {
                            $('#headBigImg-QRcode').parent().show();
                        }).off('mouseout').on('mouseout', function () {
                            $('#headBigImg-QRcode').parent().hide();
                        })
                    }
                    ;
                    // 媒体级别
                    console.log(typeof (data.Result.LevelType - 0));
                    if ((typeof (data.Result.LevelType - 0)) == "number") {
                        $('#mediaLevel input').each(function () {
                            if ($(this).val() == data.Result.LevelType) {
                                $(this).prop('checked', true)
                            }
                        })
                    }
                    //微信认证
                    if (data.Result.IsAuth) {
                        $('#weChatConfirm input').eq(0).prop('checked', true)
                    } else {
                        $('#weChatConfirm input').eq(1).prop('checked', true)
                    }

                    // 描述／签名
                    if (data.Result.Sign ? data.Result.Sign : '' != '') {
                        $('.sign').val(data.Result.Sign)
                    }
                    ;
                    // 下单备注
                    if (!data.Result.IsAreaMedia) {
                        // 下单备注
                        if (data.Result.OrderRemark ? data.Result.OrderRemark : '' != '') {
                            var a = [];
                            var c = data.Result.OrderRemark;
                            for (var i = 0; i < data.Result.OrderRemark.length; i++) {
                                a.push(data.Result.OrderRemark[i].Id)
                            }
                            data.Result.OrderRemark = a.join(',');
                            setAjax({
                                url: 'http://www.chitunion.com/api/DictInfo/GetDictInfoByTypeID',
                                type: 'get',
                                data: {
                                    typeID: 40
                                }
                            }, function (data1) {
                                var a = '';
                                $(data1.Result).each(function () {
                                    var b = ' ';
                                    // var i=data.Result.OrderRemark.split(',');
                                    // var r=$(this)[0].DictId;
                                    // console.log(i+''.indexOf(r));
                                    // console.log($(this)[0].DictId);
                                    // console.log((data.Result.OrderRemark).split(',').indexOf($(this)[0].DictId));
                                    // console.log((data.Result.OrderRemark).split(','));
                                    if ((data.Result.OrderRemark).split(',').indexOf($(this)[0].DictId + '') != -1) {
                                        b = 'checked';
                                    }
                                    a += '<span><input DictId="' + $(this)[0].DictId + '" ' + b + '  type="checkbox" > ' + $(this)[0].DictName + '</span>';
                                })
                                a += '<textarea name="" id="Other"  cols="" rows="5" style="width:150px;height:20px;resize:none;display: none;float: right;margin-right: 10px"></textarea>'
                                $('.answer').html(a);
                                if (data.Result.OrderRemark.split(',')[data.Result.OrderRemark.split(',').length - 1] == 40009) {
                                    $('#Other').show().html(c[c.length - 1].Descript);
                                }
                                ;
                                // 显示隐藏其他文本域
                                $('.answer span:last input').off('click').on('click', function () {
                                    if ($(this).prop('checked') == true) {
                                        $(this).parent().next().show();
                                    } else {
                                        $(this).parent().next().hide();
                                    }
                                });
                            })
                        }
                    } else {
                        // 下单备注
                        if (data.Result.OrderRemark ? data.Result.OrderRemark : '' != '') {
                            var a = [];
                            var c = data.Result.OrderRemark;
                            for (var i = 0; i < data.Result.OrderRemark.length; i++) {
                                a.push(data.Result.OrderRemark[i].Id)
                            }
                            data.Result.OrderRemark = a.join(',');
                            setAjax({
                                url: 'http://www.chitunion.com/api/DictInfo/GetDictInfoByTypeID',
                                type: 'get',
                                data: {
                                    typeID: 60
                                }
                            }, function (data1) {
                                var a = '';
                                $(data1.Result).each(function () {
                                    var b = ' ';
                                    // var i=data.Result.OrderRemark.split(',');
                                    // var r=$(this)[0].DictId;
                                    // console.log(i+''.indexOf(r));
                                    // console.log($(this)[0].DictId);
                                    // console.log((data.Result.OrderRemark).split(',').indexOf($(this)[0].DictId));
                                    // console.log((data.Result.OrderRemark).split(','));
                                    if ((data.Result.OrderRemark).split(',').indexOf($(this)[0].DictId + '') != -1) {
                                        b = 'checked';
                                    }
                                    a += ' <span><input DictId="' + $(this)[0].DictId + '" ' + b + '  type="checkbox" > ' + $(this)[0].DictName + '</span>';
                                })
                                $('.answer1').html(a);
                            })
                        }
                    }
                    // 区域媒体
                    if (data.Result.IsAreaMedia) {
                        $('#quyumeiti input').eq(0).prop('checked', true);
                        $('#quyumeiti input').eq(1).prop('checked', false);
                        $('#xiadanbeizhu1').hide();
                        $('#xiadanbeizhu2').show();
                        $('#meisifbqy').hide();
                        $('#fgdq').show();
                    } else {
                        $('#quyumeiti input').eq(0).prop('checked', false);
                        $('#quyumeiti input').eq(1).prop('checked', true);
                        $('#xiadanbeizhu1').show();
                        $('#xiadanbeizhu2').hide();
                        $('#meisifbqy').show();
                        $('#fgdq').hide();
                    }

                    // 覆盖地区
                    // 省
                    if (data.Result.AreaMedia.length > 0) {
                        if (data.Result.AreaMedia[0].ProvinceId != -2 ? data.Result.AreaMedia[0].ProvinceId : '' != '') {
                            $('#fgdq_1 option').each(function () {
                                if ($(this).attr('value') == data.Result.AreaMedia[0].ProvinceId) {
                                    $(this).prop('selected', true);
                                    $('#fgdq_1').change();
                                }
                            })
                        }
                        // 城市
                        $('#fgdq_1').off('change').on('change', function () {
                            var City1 = '', City2 = '<option value="-2">城市</option>'
                            $($(_this.JSonData.masterArea)[$('#fgdq_1 option:checked').attr('i')].subArea).each(function (i) {
                                City1 += '<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>'
                            })
                            $('#fgdq_2').html(City2 + City1);
                            // if($('#fgdq_1 option:checked').html()=='北京'||$('#fgdq_1 option:checked').html()=='上海'||$('#fgdq_1 option:checked').html()=='天津'||$('#fgdq_1 option:checked').html()=='重庆'){
                            //     $('#fgdq_2').hide();
                            // }else {
                            //     $('#fgdq_2').show();
                            // }
                        });
                        // 城市
                        if (data.Result.AreaMedia[0].CityId ? data.Result.AreaMedia[0].CityId : '' != '') {
                            $('#fgdq_2 option').each(function () {
                                if ($(this).attr('value') == data.Result.AreaMedia[0].CityId) {
                                    console.log($(this));
                                    $(this).prop('selected', true)
                                }
                            })
                        }
                    }

                    if (CTLogin.RoleIDs == 'SYS001RL00004' || CTLogin.RoleIDs == 'SYS001RL00001') {
                        // 资质信息
                        $('#authorization').hide();
                        $('#Enterprisename').hide();
                        $('#Businesslicense').hide();
                    }
                    if (CTLogin.RoleIDs == "SYS001RL00005" || CTLogin.RoleIDs == "SYS001RL00001" || CTLogin.RoleIDs == "SYS001RL00004" || GetRequest().AuditStatus == 43004 || GetRequest().AuthType == 38002 || GetRequest().AuthType == 38004) {
                        $('#fansscreenshot').hide();
                        $('#genderscreen').hide();
                    }

                    _this.myAjax({
                        url: 'http://www.chitunion.com/api/media/GetQualification?v=1_1',
                        type: 'get',
                        data: {
                            MediaId: candata.recId,
                            IsInsert: false
                        }
                    }, function (data) {
                        if (data.Result != null) {
                            // 授权资质
                            if (data.Result.QualificationOne ? data.Result.QualificationOne : '' != '') {
                                $('#headimgUploadFile-authorization').attr('src', data.Result.QualificationOne);
                                $('#headBigImg-authorization').attr('src', data.Result.QualificationOne);

                                $('#headimgUploadFile-authorization').off('mouseover').on('mouseover', function () {
                                    $('#headBigImg-authorization').parent().show();
                                }).off('mouseout').on('mouseout', function () {
                                    $('#headBigImg-authorization').parent().hide();
                                })
                            }
                            if (data.Result.QualificationTwo ? data.Result.QualificationTwo : '' != '') {
                                $('#headimgUploadFile-authorization1').attr('src', data.Result.QualificationTwo);
                                $('#headBigImg-authorization1').attr('src', data.Result.QualificationTwo);

                                $('#headimgUploadFile-authorization1').off('mouseover').on('mouseover', function () {
                                    $('#headBigImg-authorization1').parent().show();
                                }).off('mouseout').on('mouseout', function () {
                                    $('#headBigImg-authorization1').parent().hide();
                                })
                            }
                            // 企业名称
                            if (data.Result.EnterpriseName ? data.Result.EnterpriseName : '' != '') {
                                $('#Enterprisename1').val(data.Result.EnterpriseName)
                            }
                            // 营业执照
                            if (data.Result.BusinessLicense ? data.Result.BusinessLicense : '' != '') {
                                $('#headimgUploadFile-Businesslicense').attr('src', data.Result.BusinessLicense);
                                $('#headBigImg-Businesslicense').attr('src', data.Result.BusinessLicense);

                                $('#headimgUploadFile-Businesslicense').off('mouseover').on('mouseover', function () {
                                    $('#headBigImg-Businesslicense').parent().show();
                                }).off('mouseout').on('mouseout', function () {
                                    $('#headBigImg-Businesslicense').parent().hide();
                                })

                            }
                            ;
                        }

                    })
                }
                // 添加
                if (operatetype == 1) {
                    if (_this.GetRequest().AuthType == 38004) {
                        if (data.Result.Sign == null) {
                            $('#sign').show()
                        } else {
                            $('#sign textarea').val(data.Result.Sign)
                        }
                    }

                    // 判断个人信息
                    if (data.Result.Name == '') {
                        data.Result.Name = '暂无'
                    }
                    var Number = data.Result.Number;
                    // if (Number == '') {
                    //     Number = data.Result.MediaID
                    // }
                    if (Number == '') {
                        Number = '暂无'
                    }
                    $('.wechat_name').html('<div>微信名称：<span>' + data.Result.Name + '</span></div><div>微信帐号：<span>' + Number + '</span></div>');
                    /*=======粉丝数======*/
                    $('#Fans').val(data.Result.FansCount);

                    // 粉丝数截图
                    if (data.Result.FansCountURL ? data.Result.FansCountURL : '' != '') {
                        $('#headimgUploadFile').attr('src', data.Result.FansCountURL);
                        $('#headBigImg').attr('src', data.Result.FansCountURL);
                        $('#headimgUploadFile').off('mouseover').on('mouseover', function () {
                            $('#headBigImg').parent().show();
                        })
                        $('#headimgUploadFile').off('mouseout').on('mouseout', function () {
                            $('#headBigImg').parent().hide();
                        })

                    } else {
                        if (CTLogin.RoleIDs == "SYS001RL00003") {
                            $('#fansscreenshotnumber').hide();
                            $('#Fans').off('keyup').on('keyup', function () {
                                $('#fansscreenshotnumber').show();
                                if ($('#Fans').val() == data.Result.FansCount) {
                                    $('#fansscreenshotnumber').hide();
                                }
                            })
                        }
                    }

                    /*=======粉丝比例========*/
                    $('#male').val(data.Result.FansMalePer == 0 ? '' : data.Result.FansMalePer);
                    nan = data.Result.FansMalePer;
                    nv = data.Result.FansFemalePer;

                    if (data.Result.FansMalePer > 0) {
                        $('#genderscreen').show()
                    }
                    $('#female').val(data.Result.FansFemalePer == 0 ? '' : data.Result.FansFemalePer);
                    if (data.Result.FansFemalePer > 0) {
                        $('#genderscreen').show()
                    }
                    // 粉丝比例截图
                    if (data.Result.FansSexScaleUrl ? data.Result.FansSexScaleUrl : '' != '') {
                        $('#headimgUploadFile3').attr('src', data.Result.FansSexScaleUrl);
                        $('#headBigImg3').attr('src', data.Result.FansSexScaleUrl);
                        $('#headimgUploadFile3').off('mouseover').on('mouseover', function () {
                            $('#headBigImg3').parent().show();
                        }).off('mouseout').on('mouseout', function () {
                            $('#headBigImg3').parent().hide();
                        })
                    }
                    // 常见分类
                    _this.myAjax({
                        url: 'http://www.chitunion.com/api/DictInfo/GetDictInfoByTypeID',
                        type: 'get',
                        data: {
                            typeID: 47
                        }
                    }, function (dat) {

                        var arr = [];
                        console.log(data.Result.CommonlyClass);
                        data.Result.CommonlyClass ? data.Result.CommonlyClass : data.Result.CommonlyClass = [];
                        for (var i = 0; i < data.Result.CommonlyClass.length; i++) {
                            for (var j = 0; j < dat.Result.length; j++) {
                                if (data.Result.CommonlyClass[i] == dat.Result[j].DictId) {
                                    arr.push({name: dat.Result[j].DictName, id: dat.Result[j].DictId, sub: j})
                                }
                            }
                        }
                        if (arr.length > 0) {
                            var a = '<li class="ins_a">&nbsp;</li>';
                            var b = 0;
                            for (var i = 0; i < arr.length; i++) {
                                a += '<li name=' + arr[i].sub + ' dictid=' + arr[i].id + ' mainclass="-0"><div class="classification">' + arr[i].name + '<span style="display: none"><img src="/ImagesNew/icon50.png"/></span><em class="emclass" style="display:none;position: absolute;bottom: 0;right: 0; z-index: 0;"><img src="/ImagesNew/icon85.png"></em></div></li>';
                                b++;
                            }
                            a += '<div class="clear"></div>';
                            // 判断a是否进入循环，否的话，a就为空
                            if (b < 1) {
                                a = '';
                            }
                            $('#ification').html(a);
                            // 鼠标经过分类显示和隐藏关闭
                            $('#ification li').off('mouseover').on('mouseover', function () {
                                $(this).find('span').show();
                            }).off('mouseout').on('mouseout', function () {
                                $(this).find('span').hide();
                            }).find('span').off('click').on('click', function () {//点击span关闭
                                // 让弹窗里的分类的选中移除
                                $('.familiar ul li').eq($(this).parent().parent().attr('name')).attr('class', '');
                                // 移除当前li
                                $(this).parent().parent().remove();
                                // 判断是否还有选择的分类，根据span判断
                                if ($('#ification li span').length == 0) {
                                    $('#ification').html('');
                                }
                                //设置主分类

                                $('.emclass').hide().parents('li').attr('mainclass', '-0');
                                $('.emclass').eq(0).show().parents('li').attr('mainclass', '-1');
                                $('.classification').off('click').on('click', function () {
                                    $('.emclass').hide().parents('li').attr('mainclass', '-0');
                                    $(this).find('.emclass').show().parents('li').attr('mainclass', '-1');
                                });
                            });
                            $('.emclass').hide().parents('li').attr('mainclass', '-0');
                            $('.emclass').eq(0).show().parents('li').attr('mainclass', '-1');
                            $('.classification').off('click').on('click', function () {
                                $('.emclass').hide().parents('li').attr('mainclass', '-0');
                                $(this).find('.emclass').show().parents('li').attr('mainclass', '-1');
                            });

                        }

                    }, false)

                    // 粉丝分布区域

                    if (data.Result.FansArea != null) {
                        if (data.Result.FansArea.length > 0) {
                            // 初始渲染
                            if ($('#Renfansdistri li').attr('class') != 'ins_a') {
                                $('#Renfansdistri').html('<li class="ins_a">&nbsp;</li>')
                            }
                            // 渲染
                            $(data.Result.FansArea).each(function () {
                                $('#Renfansdistri').append('<li provinceid=' + $(this)[0].ProvinceID + ' province=' + $(this)[0].ProvinceName + ' provinceper=' + $(this)[0].UserScale + '><div class="classification">' + $(this)[0].ProvinceName + ' ' + $(this)[0].UserScale + '%<span style="display: none"><img src="/ImagesNew/icon50.png"/></span></div></li>')
                            })
                            $('#Renfansdistri').append('<div class="clear"></div>')
                            // 显示粉丝数截图
                            $('#fansscreenshot').show();
                            // 鼠标经过分类显示和隐藏关闭
                            $('#Renfansdistri li').off('mouseover').on('mouseover', function () {
                                $(this).find('span').show();
                            }).off('mouseout').on('mouseout', function () {
                                $(this).find('span').hide();
                            }).find('span').off('click').on('click', function () {//点击span关闭

                                // 移除当前li
                                $(this).parent().parent().remove();

                                if (GetRequest().AuditStatus == 43004 || GetRequest().AuthType == 38002 || GetRequest().AuthType == 38004) {
                                    $('#fansscreenshot').show();
                                }

                                // 判断是否还有选择的分类，根据span判断
                                if ($('#Renfansdistri li span').length == 0) {
                                    $('#Renfansdistri').html('');
                                    // 判断隐藏粉丝数截图
                                    $('#fansscreenshot').hide();
                                }
                            })
                        }
                        ;
                    }

                    // 分布区域截图
                    if (data.Result.FansAreaShotUrl ? data.Result.FansAreaShotUrl : '' != '') {
                        $('#headimgUploadFile2').attr('src', data.Result.FansAreaShotUrl);
                        $('#headBigImg2').attr('src', data.Result.FansAreaShotUrl);
                        $('#headimgUploadFile2').off('mouseover').on('mouseover', function () {
                            $('#headBigImg2').parent().show();
                        }).off('mouseout').on('mouseout', function () {
                            $('#headBigImg2').parent().hide();
                        })
                    }
                    // 所在地区
                    // 省
                    if (data.Result.ProvinceID != -2 ? data.Result.ProvinceID : '' != '') {
                        $('#locationArea1 option').each(function () {
                            if ($(this).attr('value') == data.Result.ProvinceID) {
                                $(this).prop('selected', true);
                                $('#locationArea1').change();
                            }
                        })
                    }
                    // 城市
                    $('#locationArea1').off('change').on('change', function () {
                        var City1 = '', City2 = '<option value="-2">城市</option>'
                        $($(_this.JSonData.masterArea)[$('#locationArea1 option:checked').attr('i')].subArea).each(function (i) {
                            City1 += '<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>'
                        })
                        $('#locationArea2').html(City2 + City1)
                    });
                    // 城市
                    if (data.Result.CityID ? data.Result.CityID : '' != '') {
                        $('#locationArea2 option').each(function () {
                            if ($(this).attr('value') == data.Result.CityID) {
                                console.log($(this));
                                $(this).prop('selected', true)
                            }
                        })
                    }
                    // 微信号
                    if (data.Result.Number != '') {
                        $('#Microsignalfilling1').val(data.Result.Number).prop('disabled', true)
                    }
                    // 微信名称
                    if (data.Result.Name != '') {
                        $('#wechatname1').val(data.Result.Name)
                    }
                    // 头像
                    if (data.Result.HeadIconURL ? data.Result.HeadIconURL : '' != '') {
                        $('#headimgUploadFile-Headportrait').attr('src', data.Result.HeadIconURL);
                        $('#headBigImg-Headportrait').attr('src', data.Result.HeadIconURL);
                        $('#headimgUploadFile-Headportrait').off('mouseover').on('mouseover', function () {
                            $('#headBigImg-Headportrait').parent().show();
                        }).off('mouseout').on('mouseout', function () {
                            $('#headBigImg-Headportrait').parent().hide();
                        })
                    }
                    ;
                    // 二维码
                    if (data.Result.TwoCodeURL ? data.Result.TwoCodeURL : '' != '') {
                        $('#headimgUploadFile-QRcode').attr('src', data.Result.TwoCodeURL);
                        $('#headBigImg-QRcode').attr('src', data.Result.TwoCodeURL);
                        $('#headimgUploadFile-QRcode').off('mouseover').on('mouseover', function () {
                            $('#headBigImg-QRcode').parent().show();
                        }).off('mouseout').on('mouseout', function () {
                            $('#headBigImg-QRcode').parent().hide();
                        })
                    }
                    ;
                    // 媒体级别
                    if ((typeof (data.Result.LevelType - 0)) == "number") {

                        $('#mediaLevel input').each(function () {
                            if ($(this).val() == data.Result.LevelType) {
                                $(this).prop('checked', true)
                            }
                        })
                    }
                    //微信认证
                    if (data.Result.IsAuth) {
                        $('#weChatConfirm input').eq(0).prop('checked', true)
                    } else {
                        $('#weChatConfirm input').eq(1).prop('checked', true)
                    }
                    // 下单备注
                    if (!data.Result.IsAreaMedia) {
                        if (data.Result.OrderRemark ? data.Result.OrderRemark : '' != '') {
                            var a = [];
                            var c = data.Result.OrderRemark;
                            for (var i = 0; i < data.Result.OrderRemark.length; i++) {
                                a.push(data.Result.OrderRemark[i].Id)
                            }
                            data.Result.OrderRemark = a.join(',');
                            setAjax({
                                url: 'http://www.chitunion.com/api/DictInfo/GetDictInfoByTypeID',
                                type: 'get',
                                data: {
                                    typeID: 40
                                }
                            }, function (data1) {
                                var a = '';
                                $(data1.Result).each(function () {
                                    var b = ' ';
                                    // var i=data.Result.OrderRemark.split(',');
                                    // var r=$(this)[0].DictId;
                                    // console.log(i+''.indexOf(r));
                                    // console.log($(this)[0].DictId);
                                    // console.log((data.Result.OrderRemark).split(',').indexOf($(this)[0].DictId));
                                    // console.log((data.Result.OrderRemark).split(','));
                                    if ((data.Result.OrderRemark).split(',').indexOf($(this)[0].DictId + '') != -1) {
                                        b = 'checked';
                                    }
                                    a += '<span><input DictId="' + $(this)[0].DictId + '" ' + b + '  type="checkbox" > ' + $(this)[0].DictName + '</span>';
                                })
                                a += '<textarea name="" id="Other"  cols="" rows="5" style="width:150px;height:20px;resize:none;display: none;float: right;margin-right: 10px"></textarea>'
                                $('.answer').html(a);
                                if (data.Result.OrderRemark.split(',')[data.Result.OrderRemark.split(',').length - 1] == 40009) {
                                    $('#Other').show().html(c[c.length - 1].Descript);
                                }
                                ;
                                // 显示隐藏其他文本域
                                $('.answer span:last input').off('click').on('click', function () {
                                    if ($(this).prop('checked') == true) {
                                        $(this).parent().next().show();
                                    } else {
                                        $(this).parent().next().hide();
                                    }
                                });
                            })
                        }
                    } else {
                        // 下单备注
                        if (data.Result.OrderRemark ? data.Result.OrderRemark : '' != '') {
                            var a = [];
                            var c = data.Result.OrderRemark;
                            for (var i = 0; i < data.Result.OrderRemark.length; i++) {
                                a.push(data.Result.OrderRemark[i].Id)
                            }
                            data.Result.OrderRemark = a.join(',');
                            setAjax({
                                url: 'http://www.chitunion.com/api/DictInfo/GetDictInfoByTypeID',
                                type: 'get',
                                data: {
                                    typeID: 60
                                }
                            }, function (data1) {
                                var a = '';
                                $(data1.Result).each(function () {
                                    var b = ' ';
                                    // var i=data.Result.OrderRemark.split(',');
                                    // var r=$(this)[0].DictId;
                                    // console.log(i+''.indexOf(r));
                                    // console.log($(this)[0].DictId);
                                    // console.log((data.Result.OrderRemark).split(',').indexOf($(this)[0].DictId));
                                    // console.log((data.Result.OrderRemark).split(','));
                                    if ((data.Result.OrderRemark).split(',').indexOf($(this)[0].DictId + '') != -1) {
                                        b = 'checked';
                                    }
                                    a += ' <span><input DictId="' + $(this)[0].DictId + '" ' + b + '  type="checkbox" > ' + $(this)[0].DictName + '</span>';
                                })
                                $('.answer1').html(a);
                            })
                        }
                    }
                    // 描述／签名
                    if (data.Result.Sign ? data.Result.Sign : '' != '') {
                        $('.sign').val(data.Result.Sign)
                    }
                    ;
                    // 区域媒体
                    if (data.Result.IsAreaMedia) {
                        $('#quyumeiti input').eq(0).prop('checked', true);
                        $('#quyumeiti input').eq(1).prop('checked', false);
                        $('#xiadanbeizhu1').hide();
                        $('#xiadanbeizhu2').show();
                        $('#meisifbqy').hide();
                        $('#fgdq').show();
                    } else {
                        $('#quyumeiti input').eq(0).prop('checked', false);
                        $('#quyumeiti input').eq(1).prop('checked', true);
                        $('#xiadanbeizhu1').show();
                        $('#xiadanbeizhu2').hide();
                        $('#meisifbqy').show();
                        $('#fgdq').hide();
                    }

                    // 覆盖地区
                    // 省
                    if(data.Result.AreaMedia){
                        if (data.Result.AreaMedia.length > 0) {
                            if (data.Result.AreaMedia[0].ProvinceId != -2 ? data.Result.AreaMedia[0].ProvinceId : '' != '') {
                                $('#fgdq_1 option').each(function () {
                                    if ($(this).attr('value') == data.Result.AreaMedia[0].ProvinceId) {
                                        $(this).prop('selected', true);
                                        $('#fgdq_1').change();
                                    }
                                })
                            }
                            // 城市
                            $('#fgdq_1').off('change').on('change', function () {
                                var City1 = '', City2 = '<option value="-2">城市</option>'
                                $($(_this.JSonData.masterArea)[$('#fgdq_1 option:checked').attr('i')].subArea).each(function (i) {
                                    City1 += '<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>'
                                })
                                $('#fgdq_2').html(City2 + City1);
                                // if($('#fgdq_1 option:checked').html()=='北京'||$('#fgdq_1 option:checked').html()=='上海'||$('#fgdq_1 option:checked').html()=='天津'||$('#fgdq_1 option:checked').html()=='重庆'){
                                //     $('#fgdq_2').hide();
                                // }else {
                                //     $('#fgdq_2').show();
                                // }
                            });
                            // 城市
                            if (data.Result.AreaMedia[0].CityId ? data.Result.AreaMedia[0].CityId : '' != '') {
                                $('#fgdq_2 option').each(function () {
                                    if ($(this).attr('value') == data.Result.AreaMedia[0].CityId) {
                                        console.log($(this));
                                        $(this).prop('selected', true)
                                    }
                                })
                            }
                        }
                    }

                    if (CTLogin.RoleIDs == "SYS001RL00005" || CTLogin.RoleIDs == "SYS001RL00001" || CTLogin.RoleIDs == "SYS001RL00004" || GetRequest().AuditStatus == 43004 || GetRequest().AuthType == 38002 || GetRequest().AuthType == 38004) {
                        $('#fansscreenshot').hide();
                        $('#genderscreen').hide();
                    }
                    _this.myAjax({
                        url: 'http://www.chitunion.com/api/media/GetQualification?v=1_1',
                        type: 'get',
                        data: {
                            MediaId: candata.recId,
                            IsInsert: true
                        }
                    }, function (data) {
                        if (data.Result != null) {

                            // 授权资质
                            // if (data.Result.QualificationOne ? data.Result.QualificationOne : '' != '') {
                            //     $('#headimgUploadFile-authorization').attr('src', data.Result.QualificationOne);
                            //     $('#headBigImg-authorization').attr('src', data.Result.QualificationOne);
                            //
                            //     $('#headimgUploadFile-authorization').off('mouseover').on('mouseover', function () {
                            //         $('#headBigImg-authorization').parent().show();
                            //     }).off('mouseout').on('mouseout', function () {
                            //         $('#headBigImg-authorization').parent().hide();
                            //     })
                            // }
                            // if (data.Result.QualificationTwo ? data.Result.QualificationTwo : '' != '') {
                            //     $('#headimgUploadFile-authorization1').attr('src', data.Result.BusinessLicense);
                            //     $('#headBigImg-authorization1').attr('src', data.Result.QualificationTwo);
                            //
                            //     $('#headimgUploadFile-authorization1').off('mouseover').on('mouseover', function () {
                            //         $('#headBigImg-authorization1').parent().show();
                            //     }).off('mouseout').on('mouseout', function () {
                            //         $('#headBigImg-authorization1').parent().hide();
                            //     })
                            // }
                            // 企业名称
                            if (data.Result.EnterpriseName ? data.Result.EnterpriseName : '' != '') {
                                $('#Enterprisename1').val(data.Result.EnterpriseName)
                                if(data.Result.AuditStatus==2){
                                    $('#Enterprisename1').prop('disabled',true);
                                }
                            }
                            // 营业执照
                            if (data.Result.BusinessLicense ? data.Result.BusinessLicense : '' != '') {
                                $('#headimgUploadFile-Businesslicense').attr('src', data.Result.BusinessLicense);
                                $('#headBigImg-Businesslicense').attr('src', data.Result.BusinessLicense);

                                $('#headimgUploadFile-Businesslicense').off('mouseover').on('mouseover', function () {
                                    $('#headBigImg-Businesslicense').parent().show();
                                }).off('mouseout').on('mouseout', function () {
                                    $('#headBigImg-Businesslicense').parent().hide();
                                })

                                if(data.Result.AuditStatus==2){
                                    $('#headUploadify-Businesslicense').hide();
                                }

                            }
                            ;
                        }
                    })
                    $('.frame_n img').attr('src', data.Result.HeadIconURL)
                    if (data.Result.IsExist) {
                        $('#Publicnumber').show().find('a').off('click').on('click', function () {
                            window.location = '/PublishManager/addEditPublishForWeiChat.html?MediaID=' + GetRequest().WxID + '&entrance=1';
                        }).end().nextAll().remove()
                    }
                    // 微信号
                    if (data.Result.Number != '') {
                        $('#Microsignalfilling1').val(data.Result.Number).prop('disabled', true)
                    }
                    // 微信名称
                    if (data.Result.Name != '') {
                        $('#wechatname1').val(data.Result.Name).prop('disabled', true)
                    }
                    // 头像
                    if (data.Result.HeadIconURL ? data.Result.HeadIconURL : '' != '') {
                        $('#headimgUploadFile-Headportrait').attr('src', data.Result.HeadIconURL)
                            .siblings('.set').remove();
                        $('#headBigImg-Headportrait').attr('src', data.Result.HeadIconURL);
                        $('#headimgUploadFile-Headportrait').off('mouseover').on('mouseover', function () {
                            $('#headBigImg-Headportrait').parent().show();
                        }).off('mouseout').on('mouseout', function () {
                            $('#headBigImg-Headportrait').parent().hide();
                        })
                    }
                    ;
                    // 二维码
                    if (data.Result.TwoCodeURL ? data.Result.TwoCodeURL : '' != '') {
                        $('#headimgUploadFile-QRcode').attr('src', data.Result.TwoCodeURL).siblings('.set').remove();
                        $('#headBigImg-QRcode').attr('src', data.Result.TwoCodeURL);
                        $('#headimgUploadFile-QRcode').off('mouseover').on('mouseover', function () {
                            $('#headBigImg-QRcode').parent().show();
                        }).off('mouseout').on('mouseout', function () {
                            $('#headBigImg-QRcode').parent().hide();
                        })
                    }
                    ;
                    // 当id为0是
                    if (_this.GetRequest().WxID == 0) {
                        console.log(1);
                        $('#Microsignalfilling1').val(Escape);
                        $('#wechatname1').prop('disabled', false)
                    }
                }
                // 禁用粉丝数
                if (_this.GetRequest().AuthType == 38001) {
                    if (data.Result.FansCount != 0) {
                        $('#Fans').val(data.Result.FansCount)
                        // .prop('disabled', true);
                    }
                    if (data.Result.FansCount < 500) {
                        $('#PromptFans').html('<span style="color: red"> 粉丝数小于500，不能被添加</span>')
                    }
                    if (data.Result.FansCount == 0) {
                        $('#Fans').val(data.Result.FansCount ? data.Result.FansCount : '')
                    } else {
                        $('#fansscreenshotnumber').hide()
                    }

                }
                // 点击提交
                _this.ClickSubmit(_this, data);
                // 提交并创建刊例
                _this.Createandsubmit(_this, data)
            })
        },
        // 微信号
        microsignal: function () {
            // 当失去焦点时，提示
            $('#Microsignalfilling1').off('blur').on('blur', function () {
                if ($(this).val() == '') {
                    $('#Microsignalfilling2').html('<span style="color: red"> 请输入微信号</span>')
                }
            }).off('focus').on('focus', function () {
                $('#Microsignalfilling2').html('')
            })
        },
        // 微信名称
        WeChatName: function () {
            // 当失去焦点时，提示
            $('#wechatname1').off('blur').on('blur', function () {
                if ($(this).val() == '') {
                    $('#wechatname2').html('<span style="color: red"> 请输入微信名称</span>')
                }
            }).off('focus').on('focus', function () {
                $('#wechatname2').html('')
            })
        },
        // 区域媒体
        quyumeiti: function () {
            $('#quyumeiti input').off('click').on('click', function () {
                if ($('#quyumeiti input').eq(0).prop('checked') == true) {
                    $('#meisifbqy').hide();
                    $('#fgdq').show();
                    $('#xiadanbeizhu1').hide();
                    $('#xiadanbeizhu2').show();
                } else {
                    $('#fgdq').hide();
                    $('#meisifbqy').show();
                    $('#xiadanbeizhu2').hide();
                    $('#xiadanbeizhu1').show();
                }
            })
        },
        // 覆盖地区
        fgdq: function () {
            var _this = this;
            // 省/直辖市
            $(_this.JSonData.masterArea).each(function (i) {
                $('#fgdq_1').append('<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>')
            })
            // 城市
            $('#fgdq_1').off('change').on('change', function () {
                var City1 = '', City2 = '<option value="-2">城市</option>';
                $($(_this.JSonData.masterArea)[$('#fgdq_1 option:checked').attr('i')].subArea).each(function (i) {
                    City1 += '<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>'
                })
                $('#fgdq_2').html(City2 + City1);
                // if($('#fgdq_1 option:checked').html()=='北京'||$('#fgdq_1 option:checked').html()=='上海'||$('#fgdq_1 option:checked').html()=='天津'||$('#fgdq_1 option:checked').html()=='重庆'){
                //     $('#fgdq_2').hide();
                // }else {
                //     $('#fgdq_2').show();
                // }
            })
        },
        // 头像
        Headportrait: function () {
            // 上传
            this.uploadImg("headUploadify-Headportrait", "headimgUploadFile-Headportrait", "headimgErr-Headportrait", "headBigImg-Headportrait");
        },
        // 二位码
        QRcode: function () {
            // 上传
            this.uploadImg("headUploadify-QRcode", "headimgUploadFile-QRcode", "headimgErr-QRcode", "headBigImg-QRcode");
        },
        //粉丝数判断
        Numberoffans: function () {
            var _this = this;
            // 键盘按下
            $('#Fans').off('keyup').on('keyup', function () {
                if (!_this.executeRegular($(this).val()[0], /^\+?[1-9]\d*$/)) {
                    $(this).val('')
                }
                if (!_this.executeRegular($(this).val()[$(this).val().length - 1], /^\+?[0-9]\d*$/)) {
                    $(this).val($(this).val().substring(0, $(this).val().length - 1))
                }
            }).off('keydown').on('keydown', function () {//键盘抬起
                // 自执行键盘按下
                $('#Fans').keyup();
            }).off('blur').on('blur', function () {//失去焦点
                if ($(this).val() == '' || $(this).val() - 0 + '' == 'NaN') {
                    $('#PromptFans').html('<span style="color: red"> 请输入粉丝数</span>')
                    $(this).val('')
                    return
                }

                if ($(this).val() <= 500) {
                    $('#PromptFans').html('<span style="color: red"> 粉丝数小于500，不能被添加</span>')
                    return
                }
                if (_this.executeRegular($(this).val()[0], /^\+?[1-9]\d*$/)) {
                    $('#PromptFans').html('')
                }
            }).off('focus').on('focus', function () {//获取焦点
                // 删除提示
                $('#PromptFans').html('')
            })
        },
        // 粉丝数截图
        fansscreenshot: function () {
            // 上传
            this.uploadImg("headUploadify", "headimgUploadFile", "headimgErr", "headBigImg");
        },
        // 选择分类
        Selectclassify: function () {
            var _this = this
            // 点击选择分类
            $('#selectclassify').off('click').on('click', function () {

                // 获取渲染后的分类的下标，存入数组
                var arr = [];
                $('#ification span').each(function () {
                    arr.push($(this).parent().parent().attr('name'))
                })
                // 弹窗
                $.openPopupLayer({
                    name: "selectclassify",
                    url: "Selectclassify.html",
                    error: function (dd) {
                        alert(dd.Status);
                    },
                    success: function () {
                        // 关闭选择分类弹窗
                        $('.close').off('click').on('click', function () {
                            $.closePopupLayer('selectclassify')
                        })
                        // 让渲染后的分类，在弹层中的分类选中
                        for (var i = 0; i < arr.length; i++) {
                            $('.familiar ul li').eq(arr[i]).attr('class', 'active')
                        }
                        // 选中或取消某个分类
                        $('.familiar ul li').each(function () {
                            $(this).off('click').on('click', function () {
                                if ($(this).attr('class') == 'active') {
                                    $(this).attr('class', '')
                                } else {
                                    $(this).attr('class', 'active')
                                }
                            })
                        })
                        // 点击提交
                        $('#keepbutton').off('click').on('click', function () {
                            // 提示
                            if ($('.familiar ul li.active').length > 5) {
                                $('#Promptfamiliar').html('<span style="color: red">最多只选5个</span>')
                                return
                            }
                            if ($('.familiar ul li.active').length < 1) {
                                $('#Promptfamiliar').html('<span style="color: red">请至少选择一个常见分类</span>')
                                return
                            }

                            var a = '<li class="ins_a">&nbsp;</li>';
                            var b = 0;
                            $('.familiar ul li').each(function () {
                                if ($(this).attr('class') == 'active') {
                                    a += '<li name=' + $(this).attr('name') + ' dictid=' + $(this).attr('dictid') + ' mainclass="-0"><div class="classification">' + $(this).html() + '<span style="display: none;z-index: 2;"><img src="/ImagesNew/icon50.png"/></span><em class="emclass" style="display:none;position: absolute;bottom: 0;right: 0; z-index: 0;"><img src="/ImagesNew/icon85.png"></em></div></li>';
                                    b++;
                                }
                            })
                            a += '<div class="clear"></div>';
                            // 判断a是否进入循环，否的话，a就为空
                            if (b < 1) {
                                a = '';
                            }
                            $('#ification').html(a);
                            // 关闭弹窗
                            $.closePopupLayer('selectclassify')
                            // 鼠标经过分类显示和隐藏关闭
                            $('#ification li').off('mouseover').on('mouseover', function () {
                                $(this).find('span').show();
                            }).off('mouseout').on('mouseout', function () {
                                $(this).find('span').hide();
                            }).find('span').off('click').on('click', function () {//点击span关闭
                                // 让弹窗里的分类的选中移除
                                $('.familiar ul li').eq($(this).parent().parent().attr('name')).attr('class', '');
                                // 移除当前li
                                $(this).parent().parent().remove();
                                // 判断是否还有选择的分类，根据span判断
                                if ($('#ification li span').length == 0) {
                                    $('#ification').html('');
                                }
                                //设置主分类
                                $('.emclass').hide().parents('li').attr('mainclass', '-0');
                                $('.emclass').eq(0).show().parents('li').attr('mainclass', '-1');
                                $('.classification').off('click').on('click', function () {
                                    $('.emclass').hide().parents('li').attr('mainclass', '-0');
                                    $(this).find('.emclass').show().parents('li').attr('mainclass', '-1');
                                });
                            });
                            $('.emclass').hide().parents('li').attr('mainclass', '-0');
                            $('.emclass').eq(0).show().parents('li').attr('mainclass', '-1');
                            $('.classification').off('click').on('click', function () {
                                $('.emclass').hide().parents('li').attr('mainclass', '-0');
                                $(this).find('.emclass').show().parents('li').attr('mainclass', '-1');
                            });
                        })

                    }
                })
            })
        },
        // 粉丝数分布区域
        fansdistri: function () {
            var _this = this;
            // 省级渲染到粉丝数分布区域上
            $(_this.JSonData.masterArea).each(function () {
                $('#fansdistribution').append('<option value=' + this.id + '>' + this.name + '</option>')
            })
            // 添加
            $('#addfansdistri').off('click').on('click', function () {
                fsfbqyjt=1;
                // 选中省/直辖市
                if ($('#fansdistribution option:checked').val() == -2) {
                    $('#Promptfansdistri').html('<span style="color: red">请选择区域</span>');
                    return
                }
                // 正则匹配百分比
                if (!_this.executeRegular($('#perfansdistri').val() - 0, /^(0|[1-9][0-9]{0,9})(\.[0-9]{1,2})?$/)) {
                    $('#Promptfansdistri').html('<span style="color: red">请输入整数或小数，小数保留两位</span>');
                    $('#perfansdistri').val('');
                    return
                }
                // 粉丝数分布区域不能为空
                if ($('#perfansdistri').val() == '') {
                    $('#Promptfansdistri').html('<span style="color: red">请输入百分比数</span>');
                    return
                }
                // 粉丝数分布区域不能大于100
                if ($('#perfansdistri').val() - 0 > 100) {
                    $('#Promptfansdistri').html('<span style="color: red">百分比不能大于100</span>');
                    $('#perfansdistri').val('');
                    return
                }
                // 获取
                var Provinceid = $('#fansdistribution option:checked').val();
                var Province = $('#fansdistribution option:checked').html()
                var Provinceper = $('#perfansdistri').val();
                // 判断是否重复添加
                var a = false;
                $('#Renfansdistri li').each(function () {
                    if ($(this).attr('provinceid') == Provinceid) {
                        a = true
                    }
                });
                if (a) {
                    $('#Promptfansdistri').html('<span style="color: red">该区域已添加</span>');
                    return
                }
                // 判断渲染的数据相加不能大于100
                var nub = 0;
                $('#Renfansdistri li').each(function () {
                    if ($(this).attr('Provinceper') - 0 > 0) {
                        nub += ($(this).attr('Provinceper') - 0)
                    }
                })
                if (Provinceper - 0 + nub > 100) {
                    $('#Promptfansdistri').html('<span style="color: red">所选区域的总和的百分比不能大于100</span>');
                    return
                }
                // 初始渲染
                if ($('#Renfansdistri li').attr('class') != 'ins_a') {
                    $('#Renfansdistri').html('<li class="ins_a">&nbsp;</li><div class="clear"></div>')
                }
                // 渲染
                $('#Renfansdistri li:last').after('<li provinceid=' + Provinceid + ' province=' + Province + ' provinceper=' + Provinceper + '><div class="classification">' + Province + ' ' + Provinceper + '%<span style="display: none"><img src="/ImagesNew/icon50.png"/></span></div></li>')
                if (CTLogin.RoleIDs != "SYS001RL00003") {
                    $('#fansscreenshot').hide();
                } else {
                    // 显示粉丝数截图
                    $('#fansscreenshot').show();
                }
                // 清空
                $('#perfansdistri').val('')
                $('#Promptfansdistri').html('')
                $('#fansdistribution option').eq(0).prop('selected', true);
                // 鼠标经过分类显示和隐藏关闭
                $('#Renfansdistri li').off('mouseover').on('mouseover', function () {
                    $(this).find('span').show();
                }).off('mouseout').on('mouseout', function () {
                    $(this).find('span').hide();
                }).find('span').off('click').on('click', function () {//点击span关闭
                    // 移除当前li
                    $(this).parent().parent().remove();
                    // 判断是否还有选择的分类，根据span判断
                    if ($('#Renfansdistri li span').length == 0) {
                        $('#Renfansdistri').html('');
                        // 判断隐藏粉丝数截图
                        $('#fansscreenshot').hide();
                    }
                })
            })

        },
        //分布区域截图
        fansdistriscreenshot: function () {
            // 上传
            this.uploadImg("headUploadify2", "headimgUploadFile2", "headimgErr2", "headBigImg2");
        },
        // 粉丝性别比例
        fansex: function () {
            var _this = this
            var arr = [];
            arr.length = 2;
            // 男性失去焦点
            $('#male').off('blur').on('blur', function () {
                // 当数据没有时隐藏
                if ($('#female').val() == '' && $('#male').val() == '') {

                    $('#genderscreen').hide();
                }
                // 数据为空时
                if ($(this).val() == '') {
                    return
                }

                if (!_this.executeRegular($(this).val(), /^(0|[1-9][0-9]{0,9})(\.[0-9]{1,2})?$/)) {
                    $('#Promptfansex').html('<span style="color: red"> 只能输入数字和小数点，支持两位小数</span>')
                    $(this).val('')
                    arr[0] = true
                    return
                }
                // 判断男性比例不能大于100
                if ($(this).val() - 0 > 100) {
                    $('#Promptfansex').html('<span style="color: red"> 男女比例之和不能大于100</span>')
                    $(this).val('')
                    arr[0] = true
                    return
                }
                // 判断男女比例相加是否大于>100
                if (($('#female').val() - 0 + ($('#male').val() - 0)) > 100) {
                    $('#Promptfansex').html('<span style="color: red"> 男女比例之和不能大于100</span>')
                    return
                }
                if (CTLogin.RoleIDs != "SYS001RL00003") {
                    // 显示性别比例截图
                    $('#genderscreen').hide();
                } else {
                    $('#genderscreen').show();
                    if (nan == $('#male').val() && nv == $('#female').val()) {
                        // 显示性别比例截图
                        $('#genderscreen').hide();
                    }
                }
                if (GetRequest().AuthType == 38001 || GetRequest().AuthType == 38002 || GetRequest().AuthType == 38004) {
                    if (nan == $('#male').val() && nv == $('#female').val()) {
                        // 显示性别比例截图
                        $('#genderscreen').hide();
                    }
                }
                // 清空数据
                $('#Promptfansex').html('')
            })
            if (arr[0]) {
                return
            }
            // 女性失去焦点
            $('#female').off('blur').on('blur', function () {
                // 当数据没有时隐藏
                if ($('#female').val() == '' && $('#male').val() == '') {
                    $('#genderscreen').hide();
                }
                // 数据为空时
                if ($(this).val() == '') {
                    return
                }

                if (!_this.executeRegular($(this).val(), /^(0|[1-9][0-9]{0,9})(\.[0-9]{1,2})?$/)) {
                    $('#Promptfansex').html('<span style="color: red"> 只能输入数字和小数点，支持两位小数</span>')
                    $(this).val('')
                    arr[1] = true
                    return
                }
                // 判断女性比例不能大于100
                if ($(this).val() - 0 > 100) {
                    $('#Promptfansex').html('<span style="color: red"> 男女比例之和不能大于100</span>')
                    $(this).val('')
                    arr[0] = true
                    return
                }
                // 判断男女比例相加是否大于>100
                if (($('#female').val() - 0 + ($('#male').val() - 0)) > 100) {
                    $('#Promptfansex').html('<span style="color: red"> 男女比例之和不能大于100</span>')
                    return
                }
                if (CTLogin.RoleIDs != "SYS001RL00003") {
                    // 显示性别比例截图
                    $('#genderscreen').hide();
                } else {
                    $('#genderscreen').show();
                    if (nan == $('#male').val() && nv == $('#female').val()) {
                        // 显示性别比例截图
                        $('#genderscreen').hide();
                    }
                }
                if (GetRequest().AuthType == 38001 || GetRequest().AuthType == 38002 || GetRequest().AuthType == 38004) {
                    if (nan == $('#male').val() && nv == $('#female').val()) {
                        // 显示性别比例截图
                        $('#genderscreen').hide();
                    }
                }
                // 清空数据
                $('#Promptfansex').html('')
            })
            if (arr[1]) {
                return
            }

        },
        // 性别比例截图
        genderscreenshot: function () {
            // 上传
            this.uploadImg("headUploadify3", "headimgUploadFile3", "headimgErr3", "headBigImg3");
            // 点击示例
            console.log($('.Screenshotlayer img').next());
            $('.Screenshotlayer').off('click').on('click', function (e) {
                $(this).find('span:eq(1)').next().css({
                    'position': 'fixed',
                    'left': '50%',
                    'top': '50%',
                    'z-index': 100,
                    'display': 'block'
                });
                $(this).find('span:eq(1)').next().next().css({
                    'position': 'fixed',
                    'left': '0',
                    'top': '0',
                    'width': "100%",
                    'height': "100%",
                    'background-color': 'rgba(0,0,0,.5)',
                    'z-index': 99,
                    'display': 'block'
                })
            });
            $('.Screenshotlayer img').next().off('click').on('click', function (e) {
                console.log($(this));
                $(this).css({'display': 'none'})
                $('.Screenshotlayer').find('img').css('display', 'none');
                return false
            })
        },
        // 所在地区
        locationarea: function () {
            var _this = this;
            // 省/直辖市
            $(_this.JSonData.masterArea).each(function (i) {
                $('#locationArea1').append('<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>')
            })
            // 城市
            $('#locationArea1').off('change').on('change', function () {
                var City1 = '', City2 = '<option value="-2">城市</option>';
                $($(_this.JSonData.masterArea)[$('#locationArea1 option:checked').attr('i')].subArea).each(function (i) {
                    City1 += '<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>'
                })
                $('#locationArea2').html(City2 + City1)
            })
        },
        // 下单备注
        Ordermemo: function () {
            setAjax({
                url: 'http://www.chitunion.com/api/DictInfo/GetDictInfoByTypeID',
                type: 'get',
                data: {
                    typeID: 40
                }
            }, function (data) {
                var a = ''
                $(data.Result).each(function () {
                    a += '<span><input DictId="' + $(this)[0].DictId + '" type="checkbox" > ' + $(this)[0].DictName + '</span>';
                })
                a += '<textarea name="" id="Other"  cols="" rows="5" style="width:150px;height:20px;resize:none;display: none;float: right;margin-right: 10px"></textarea>'
                $('.answer').html(a);
                // 显示隐藏其他文本域
                $('.answer span:last input').off('click').on('click', function () {
                    if ($(this).prop('checked') == true) {
                        $(this).parent().next().show();
                    } else {
                        $(this).parent().next().hide();
                    }
                })
            });
            setAjax({
                url: 'http://www.chitunion.com/api/DictInfo/GetDictInfoByTypeID',
                type: 'get',
                data: {
                    typeID: 60
                }
            }, function (data) {
                var a = ''
                $(data.Result).each(function () {
                    a += ' <span><input DictId="' + $(this)[0].DictId + '" type="checkbox" > ' + $(this)[0].DictName + '</span>';
                });
                $('.answer1').html(a);
            })
        },
        // 代理授权资质
        authorization: function () {
            // 上传
            this.uploadImg("headUploadify-authorization", "headimgUploadFile-authorization", "headimgErr-authorization", "headBigImg-authorization");
            // 上传
            this.uploadImg("headUploadify-authorization1", "headimgUploadFile-authorization1", "headimgErr-authorization1", "headBigImg-authorization1");
        },
        // 营业执照
        Businesslicense: function () {
            // 上传
            this.uploadImg("headUploadify-Businesslicense", "headimgUploadFile-Businesslicense", "headimgErr-Businesslicense", "headBigImg-Businesslicense");
        },
        // 提交
        ClickSubmit: function (_this, data) {
            var _this = _this;
            var operatetype;
            if (_this.GetRequest().AuthType == undefined) {
                operatetype = 2
            } else {
                operatetype = 1
            }
            // 点击提交
            $('#Submit1').off('click').on('click', function () {
                // 点击提交还原
                $('#fansscreen').html('');
                $('#Selectclass').html('');
                $('#fansshot').html('');
                $('#genscreen').html('');
                $('#Microsignalfilling2').html('');
                $('#wechatname2').html('');
                $('#wechatname2').html('');
                $('#headimgUploadFile-Headportrait2').html('');
                $('#headimgUploadFile-QRcode2').html('');
                $('#mediaLevelinfo').html('');
                $('#weChatConfirminfo').html('');
                $('#signinfo').html('');
                $('#fgdq_3').html('');
                $('#Ordermemo3').html('');
                // $('.grey').html('<img src="/ImagesNew/icon21.png"/> 提示：审核时间通常为1到3个工作日，审核通过即可生效。')

                // 存入错误信息
                var arr = []
                // 判断粉丝数后面是否错误提示
                if ($('#PromptFans span').html() == undefined ? '' : $('#PromptFans span').html() != '') {
                    arr.push($('#PromptFans span').html())
                }
                // 判断粉丝数是否填写
                if (!$('#Fans').val()) {
                    arr.push('请输入粉丝数')
                    $('#PromptFans').html('<span style="color: red"> 请输入粉丝数</span>')
                }

                if ($('#Fans').val() <= 500) {
                    arr.push('粉丝数小于500，不能被添加')
                    $('#PromptFans').html('<span style="color: red"> 粉丝数小于500，不能被添加</span>')
                }
                if (!(_this.GetRequest().AuthType == 38001)) {
                    // 判断粉丝数截图是否有数据
                    if (CTLogin.RoleIDs == "SYS001RL00003" && $('#fansscreenshotnumber').css('display') != "none") {
                        console.log(1);
                        if ($('#headimgUploadFile').attr('src') == '') {
                            arr.push('请上传粉丝数截图');
                            $('#fansscreen').html('请上传粉丝数截图');
                        }
                    }
                }
                // 判断是否有常见分类
                if ($('#ification li').length == 0) {
                    arr.push('请至少选择一个常见分类')
                    $('#Selectclass').html('请至少选择一个常见分类');
                }
                // 判断粉丝分布区域下是否有数据
                // if ($('#Renfansdistri li').length > 0) {
                //     if (CTLogin.RoleIDs == "SYS001RL00003") {
                //         if ($('#headimgUploadFile2').attr('src') == '' && $('#fansscreenshot').css('display') == 'block') {
                //             arr.push('请上传粉丝分布区域截图');
                //             $('#fansshot').html('请上传粉丝分布区域截图');
                //         }
                //     }
                // }
                if ($('#quyumeiti input').eq(1).prop('checked') == true) {
                    // 判断粉丝分布区域下是否有数据
                    if ($('#Renfansdistri li').length > 0) {
                        if (CTLogin.RoleIDs == "SYS001RL00003") {
                            if(fsfbqyjt){
                            if ($('#headimgUploadFile2').attr('src') == '') {
                                arr.push('请上传粉丝分布区域截图');
                                $('#fansshot').html('请上传粉丝分布区域截图');
                            }
                            }
                        }
                    }
                } else {
                    if ($('#fgdq_1 option:checked').attr("value") == -2) {
                        arr.push('请选择覆盖地区');
                        $('#fgdq_3').html('请选择覆盖地区');
                    }

                    // 下单备注
                    var OrderRemark = [];
                    $('#Ordermemo2 span input').each(function () {
                        if ($(this).prop('checked') == true) {
                            OrderRemark.push($(this).attr('dictid'))
                        }
                    })
                    if (OrderRemark.length == 0) {
                        arr.push('请选择下单备注');
                        $('#Ordermemo3').html('请选择下单备注');
                    }

                }
                // 判断粉丝性别比例是否有数据
                if ($('#male').val() || $('#female').val()) {
                    if (CTLogin.RoleIDs == "SYS001RL00003") {
                        if ($('#male').val() != 0 || $('#female').val() != 0) {
                            if (nan != $('#male').val() || nv != $('#female').val()) {
                                if ($('#headimgUploadFile3').attr('src') == '') {
                                    arr.push('请上传粉丝男女比例截图');
                                    $('#genscreen').html('请上传粉丝男女比例截图');
                                }
                            }
                        }
                    }
                }

                // 判断下单备注
                if ($('.answer span:last input').prop('checked') == true && $('.answer textarea').val() == '') {
                    layer.msg('请输入下单备注!', {time: 1000});
                    arr.push('请输入下单备注!');
                }
                // 资质信息

                if (_this.GetRequest().AuthType == 38004 || _this.GetRequest().OAuthType == 38004) {
                    if (_this.GetRequest().q != 1) {
                        if ($('#headBigImg-authorization').attr('src') == '' && $('#headBigImg-authorization1').attr('src') == '') {
                            arr.push('请上传代理授权资质');
                            $('#authorization-genscreen').html('请上传代理授权资质');
                        }
                        ;
                        if ($('#Enterprisename1').val() == '') {
                            arr.push('请输入企业名称');
                            $('#Enterprisename2').html('请输入企业名称');
                        }
                        ;
                        if ($('#headBigImg-Businesslicense').attr('src') == '') {
                            arr.push('请上传营业执照');
                            $('#Businesslicense-genscreen').html('请上传营业执照');
                        }
                        if ($('#sign textarea').val() == '') {
                            arr.push('请输入描述／签名');
                            $('#signinfo').html('请输入描述／签名');
                        }
                    }

                    // 判断二维码
                    if ($('#headimgUploadFile-QRcode').attr('src') == '') {
                        arr.push('请上传二维码')
                        $('#headimgUploadFile-QRcode2').html('<span style="color: red"> 请上传二维码</span>')
                    }
                }
                // AE
                if (_this.GetRequest().AuthType == 38003 || _this.GetRequest().wxae == 1) {
                    // 判断微信号
                    if ($('#Microsignalfilling1').val() == '') {
                        arr.push('请输入微信号')
                        $('#Microsignalfilling2').html('<span style="color: red"> 请输入微信号</span>')
                    }
                    // 判断微信名称
                    if ($('#wechatname1').val() == '') {
                        arr.push('请输入微信名称')
                        $('#wechatname2').html('<span style="color: red"> 请输入微信名称</span>')
                    }
                    // 判断头像
                    if ($('#headimgUploadFile-Headportrait').attr('src') == '') {
                        arr.push('请上传头像')
                        $('#headimgUploadFile-Headportrait2').html('<span style="color: red"> 请上传头像</span>')
                    }
                    // 判断二维码
                    if ($('#headimgUploadFile-QRcode').attr('src') == '') {
                        arr.push('请上传二维码')
                        $('#headimgUploadFile-QRcode2').html('<span style="color: red"> 请上传二维码</span>')
                    }
                    // 判断媒体级别
                    var mediaLevel = 0;
                    $('#mediaLevel input').each(function () {
                        if ($(this).prop('checked') == false) {
                            mediaLevel++;
                        }
                    })
                    if (mediaLevel > 1) {
                        arr.push('请选择媒体级别');
                        $('#mediaLevelinfo').html("请选择媒体级别")
                    }
                    // 判断微信认证
                    var weChatConfirm = 0;
                    $('#weChatConfirm input').each(function () {
                        if ($(this).prop('checked') == false) {
                            weChatConfirm++;
                        }
                    })
                    if (weChatConfirm > 1) {
                        arr.push('请选择微信认证');
                        $('#weChatConfirminfo').html("请选择微信认证")
                    }
                    // 判断描述／签名
                    if ($.trim($('.sign').val()) == '') {
                        arr.push('请填写描述／签名');
                        $('#signinfo').html("请填写描述／签名")
                    }

                }
                if (_this.GetRequest().WxID == 0) {
                    // 判断微信号
                    if ($('#Microsignalfilling1').val() == '') {
                        arr.push('请输入微信号')
                        $('#Microsignalfilling2').html('<span style="color: red"> 请输入微信号</span>')
                    }
                    // 判断微信名称
                    if ($('#wechatname1').val() == '') {
                        arr.push('请输入微信名称')
                        $('#wechatname2').html('<span style="color: red"> 请输入微信名称</span>')
                    }
                    // 判断头像
                    if ($('#headimgUploadFile-Headportrait').attr('src') == '') {
                        arr.push('请上传头像')
                        $('#headimgUploadFile-Headportrait2').html('<span style="color: red"> 请上传头像</span>')
                    }
                }

                // 判断是否选择主分类
                var classa = true;
                $('#ification [mainclass]').each(function () {
                    if ($(this).attr('mainclass') == '-1') {
                        classa = false;
                        return false;
                    }
                })
                if ($('#ification li').length != 0) {
                    if (classa) {
                        layer.msg('请设置一个主分类');
                        return false;
                    }
                }

                // 判断是否修改
                // 粉丝数
                var Fans = $('#Fans').val();
                // 粉丝数截图
                var headimgUploadFile = $('#headBigImg').attr("src");
                // 常见分类
                var arrification = []
                $('#ification li[name]').each(function () {
                    arrification.push($(this).attr('dictid') - 0)
                })
                // 粉丝分布区域
                var arrRenfansdistri = []
                $('#Renfansdistri li[provinceid]').each(function () {
                    arrRenfansdistri.push($(this).attr('provinceid') + '-' + $(this).attr('provinceper'))
                })
                // 分布区域截图
                var headimgUploadFile2 = $('#headBigImg2').attr("src")
                // 粉丝性别比例
                var male = $('#male').val()
                var female = $('#female').val()
                // 性别比例截图
                var headimgUploadFile3 = $('#headBigImg3').attr("src")
                // 所在地区
                var locationArea1 = $('#locationArea1 option:checked').attr("value")
                var locationArea2 = $('#locationArea2 option:checked').attr("value")


                var Contrast = {
                    "FansCount": Fans - 0,
                    "FansMalePer": male - 0,
                    "FansFemalePer": female - 0,
                    "CommonlyClass": arrification.length ? arrification : null,
                    "FansArea": arrRenfansdistri.length ? arrRenfansdistri : null,
                    "FansAreaShotUrl": headimgUploadFile2 == '' ? null : headimgUploadFile2,
                    "FansSexScaleUrl": headimgUploadFile3 == '' ? null : headimgUploadFile3,
                    "IsAreaMedia":$('#quyumeiti input').eq(0).prop('checked'),
                };
                // 覆盖地区
                var fgdq_1 = $('#fgdq_1 option:checked').attr("value");
                var fgdq_2 = $('#fgdq_2 option:checked').attr("value");
                // 覆盖地区
                Contrast.AreaMedia = [{
                    "CityId": fgdq_2,
                    "CityName": $('#fgdq_2 option:checked').html() == '城市' ? '' : $('#fgdq_2 option:checked').html(),
                    "ProvinceId": fgdq_1,
                    "ProvinceName": $('#fgdq_1 option:checked').html() == '省/直辖市' ? '' : $('#fgdq_1 option:checked').html()
                }];
                if ($('#quyumeiti input').eq(1).prop('checked') == true) {
                    // 下单备注1
                    var OrderRemark = [];
                    $('#Ordermemo li:last span input').each(function () {
                        if ($(this).prop('checked') == true) {
                            OrderRemark.push($(this).attr('dictid'))
                        }
                    })
                        var OrderRemark1 = [];
                        for (var i = 0; i < OrderRemark.length; i++) {
                            if (OrderRemark[i] == 40009) {
                                OrderRemark1.push({
                                    "Descript": $('#Other').val(),
                                    "Id": OrderRemark[i] - 0
                                })
                            } else {
                                OrderRemark1.push({
                                    "Descript": '',
                                    "Id": OrderRemark[i] - 0

                                })
                            }
                        }
                        Contrast.OrderRemark = OrderRemark1;
                } else {
                    // 下单备注2
                    var OrderRemark_1 = [];
                    $('#Ordermemo2 span input').each(function () {
                        if ($(this).prop('checked') == true) {
                            OrderRemark_1.push($(this).attr('dictid'))
                        }
                    })
                        var OrderRemark_2 = [];
                        for (var i = 0; i < OrderRemark_1.length; i++) {
                            if (OrderRemark_1[i] == 40009) {
                                OrderRemark_2.push({
                                    "Descript": $('#Other').val(),
                                    "Id": OrderRemark_1[i] - 0
                                })
                            } else {
                                OrderRemark_2.push({
                                    "Descript": '',
                                    "Id": OrderRemark_1[i] - 0
                                })
                            }
                        }
                        Contrast.OrderRemark = OrderRemark_2;

                }

                if (CTLogin.RoleIDs == 'SYS001RL00005' || CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00004') {
                    // 微信认证
                    $('#weChatConfirm input').each(function () {
                        if ($(this).prop('checked') == true) {
                            Contrast.IsAuth = $(this).attr('data-id') ? true : false;
                        }
                    })
                }
                var FansAreaData = [];
                if (data.Result.FansArea != null) {
                    for (var i = 0; i < data.Result.FansArea.length; i++) {
                        console.log(data.Result.FansArea);
                        console.log(data.Result.FansArea.ProvinceID);
                        FansAreaData.push(data.Result.FansArea[i].ProvinceID + '-' + data.Result.FansArea[i].UserScale)
                    }
                }
                var arrOrder=[];
                data.Result.OrderRemark?data.Result.OrderRemark:data.Result.OrderRemark=[];
                for(var i=0;i<data.Result.OrderRemark.length;i++){
                    arrOrder.push({
                        "Descript":data.Result.OrderRemark[i].Descript,
                        "Id":data.Result.OrderRemark[i].Id
                    })
                }
                var ContrastData = {
                    "FansCount": data.Result.FansCount,
                    "FansMalePer": data.Result.FansMalePer,
                    "FansFemalePer": data.Result.FansFemalePer,
                    "CommonlyClass": data.Result.CommonlyClass,
                    "FansArea": FansAreaData.length ? FansAreaData : null,
                    "FansAreaShotUrl": data.Result.FansAreaShotUrl,
                    "FansSexScaleUrl": data.Result.FansSexScaleUrl,
                    "IsAreaMedia":data.Result.IsAreaMedia,
                    "OrderRemark":arrOrder,
                    "AreaMedia":data.Result.AreaMedia?data.Result.AreaMedia:[]
                }
                if (CTLogin.RoleIDs == 'SYS001RL00005' || CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00004') {
                    // 微信号
                    Contrast.Number = $('#Microsignalfilling1').val();
                    // 微信名称
                    Contrast.Name = $('#wechatname1').val();
                    // 头像
                    Contrast.HeadIconURL = $('#headBigImg-Headportrait').attr('src');
                    // 二维码
                    Contrast.TwoCodeURL = $('#headBigImg-QRcode').attr('src');
                    // 媒体级别
                    $('#mediaLevel input').each(function () {
                        if ($(this).prop('checked') == true) {
                            Contrast.LevelType = $(this).attr('value')
                        }
                    })
                    // 微信认证
                    $('#weChatConfirm input').each(function () {
                        if ($(this).prop('checked') == true) {
                            Contrast.IsAuth = $(this).attr('data-id') ? true : false;
                        }
                    })

                    // 描述签名
                    Contrast.Sign = $('.sign').val();
                    ContrastData.Number = data.Result.Number;
                    ContrastData.Name = data.Result.Name;
                    ContrastData.HeadIconURL = data.Result.HeadIconURL;
                    ContrastData.TwoCodeURL = data.Result.TwoCodeURL;
                    ContrastData.LevelType = data.Result.LevelType;
                    ContrastData.IsAuth = data.Result.IsAuth;
                    ContrastData.Sign = data.Result.Sign;
                    if (JSON.stringify(Contrast) == JSON.stringify(ContrastData)) {
                        if (operatetype == 2) {
                            layer.msg('您未对媒体信息进行修改，不能提交审核');
                            return false
                        }

                    }
                } else {
                    if (_this.GetRequest().WxID == 0) {
                        // 微信号
                        Contrast.Number = $('#Microsignalfilling1').val();
                        // 微信名称
                        Contrast.Name = $('#wechatname1').val();
                        // 头像
                        Contrast.HeadIconURL = $('#headBigImg-Headportrait').attr('src');
                        ContrastData.Number = data.Result.Number;
                        ContrastData.Name = data.Result.Name;
                        ContrastData.HeadIconURL = data.Result.HeadIconURL;

                    }
                    if (JSON.stringify(Contrast) == JSON.stringify(ContrastData)) {
                        if (operatetype == 2) {
                            layer.msg('您未对媒体信息进行修改，不能提交审核');
                            return false
                        }
                    }
                }
                console.log(JSON.stringify(_this.Getparameters()));
                // return false;

                if (arr.length == 0) {
                    console.log(1);
                    console.log(_this.Getparameters());
                    setAjax({
                        url: 'http://www.chitunion.com/api/media/curd?v=1_1',
                        type: 'POST',
                        data: _this.Getparameters()
                    }, function (data) {
                        console.log(data);
                        console.log(data.Status);
                        if (data.Status == 0) {
                            layer.msg('提交成功');
                            window.location = '/MediaManager/mediawechatlist_new.html'
                        } else {
                            layer.msg(data.Message)
                        }

                    })

                } else if (arr.length == 1 && arr[0] == " 提示：授权的数据不能修改，系统会自动更新") {
                    console.log(1);
                    console.log(_this.Getparameters());
                    setAjax({
                        url: 'http://www.chitunion.com/api/media/curd?v=1_1',
                        type: 'POST',
                        data: _this.Getparameters()
                    }, function (data) {

                        console.log(data);
                        console.log(data.Status);
                        if (data.Status == 0) {
                            layer.msg('提交成功')
                            window.location = '/MediaManager/mediawechatlist_new.html'
                        } else {
                            layer.msg(data.Message)
                        }

                    })


                } else {
                    // arr=arr.join('/')
                    // $('.grey').html('<img src="/ImagesNew/icon21.png"/> 提示：'+arr)
                    console.log(arr);
                    console.log(2);
                    return
                }
            })

        },
        // 提交并创建刊例
        Createandsubmit: function (_this, data) {
            var _this = _this;
            var operatetype;
            if (_this.GetRequest().AuthType == undefined) {
                operatetype = 2
            } else {
                operatetype = 1
            }
            // 点击提交
            $('#Submit3').off('click').on('click', function () {
                // 点击提交还原
                $('#fansscreen').html('');
                $('#Selectclass').html('');
                $('#fansshot').html('');
                $('#genscreen').html('');
                $('#Microsignalfilling2').html('');
                $('#wechatname2').html('');
                $('#wechatname2').html('');
                $('#headimgUploadFile-Headportrait2').html('');
                $('#headimgUploadFile-QRcode2').html('');
                $('#mediaLevelinfo').html('');
                $('#weChatConfirminfo').html('');
                $('#signinfo').html('');
                // $('.grey').html('<img src="/ImagesNew/icon21.png"/> 提示：审核时间通常为1到3个工作日，审核通过即可生效。')
                // 存入错误信息
                var arr = [];
                // 判断下单备注
                if ($('.answer span:last input').prop('checked') == true && $('.answer textarea').val() == '') {
                    layer.msg('请输入下单备注!', {time: 1000});
                    arr.push('请输入下单备注!');
                }
                // 判断粉丝数后面是否错误提示
                if ($('#PromptFans span').html() == undefined ? '' : $('#PromptFans span').html() != '') {
                    arr.push($('#PromptFans span').html())
                }
                // 判断粉丝数是否填写
                if (!$('#Fans').val()) {
                    arr.push('请输入粉丝数')
                    $('#PromptFans').html('<span style="color: red"> 请输入粉丝数</span>')
                }

                if ($('#Fans').val() <= 500) {
                    arr.push('粉丝数小于500，不能被添加')
                    $('#PromptFans').html('<span style="color: red"> 粉丝数小于500，不能被添加</span>')
                }
                if (!(_this.GetRequest().AuthType == 38001)) {
                    if (CTLogin.RoleIDs == "SYS001RL00003") {
                        // 判断粉丝数截图是否有数据
                        if ($('#headimgUploadFile').attr('src') == '') {
                            arr.push('请上传粉丝数截图');
                            $('#fansscreen').html('请上传粉丝数截图');
                        }
                    }
                }
                // 判断是否有常见分类
                if ($('#ification li').length == 0) {
                    arr.push('请至少选择一个常见分类')
                    $('#Selectclass').html('请至少选择一个常见分类');
                }
                // 判断粉丝分布区域下是否有数据
                if ($('#Renfansdistri li').length > 0) {
                    if (CTLogin.RoleIDs == "SYS001RL00003") {
                        console.log(fsfbqyjt);
                        if(fsfbqyjt){
                            if ($('#headimgUploadFile2').attr('src') == '') {
                                arr.push('请上传粉丝分布区域截图');
                                $('#fansshot').html('请上传粉丝分布区域截图');
                            }
                        }
                    }
                }
                // 判断粉丝性别比例是否有数据
                if ($('#male').val() || $('#female').val()) {
                    if (CTLogin.RoleIDs == "SYS001RL00003") {
                        if ($('#male').val() != 0 && $('#female').val() != 0) {
                            if ($('#headimgUploadFile3').attr('src') == '') {
                                arr.push('请上传粉丝男女比例截图');
                                $('#genscreen').html('请上传粉丝男女比例截图');
                            }
                        }
                    }
                }


                // 资质信息
                if (_this.GetRequest().AuthType == 38004 || _this.GetRequest().OAuthType == 38004) {
                    if (_this.GetRequest().q != 1) {
                        if ($('#headBigImg-authorization').attr('src') == '' && $('#headBigImg-authorization1').attr('src') == '') {
                            arr.push('请上传代理授权资质');
                            $('#authorization-genscreen').html('请上传代理授权资质');
                        }
                        ;
                        if ($('#Enterprisename1').val() == '') {
                            arr.push('请输入企业名称');
                            $('#Enterprisename2').html('请输入企业名称');
                        }
                        ;
                        if ($('#headBigImg-Businesslicense').attr('src') == '') {
                            arr.push('请上传营业执照');
                            $('#Businesslicense-genscreen').html('请上传营业执照');
                        }
                        ;
                        if ($('#sign textarea').val() == '') {
                            arr.push('请输入描述／签名');
                            $('#signinfo').html('请输入描述／签名');
                        }
                    }
                }
                // AE
                if (_this.GetRequest().AuthType == 38003 || _this.GetRequest().wxae == 1) {
                    // 判断微信号
                    if ($('#Microsignalfilling1').val() == '') {
                        arr.push('请输入微信号')
                        $('#Microsignalfilling2').html('<span style="color: red"> 请输入微信号</span>')
                    }
                    // 判断微信名称
                    if ($('#wechatname1').val() == '') {
                        arr.push('请输入微信名称')
                        $('#wechatname2').html('<span style="color: red"> 请输入微信名称</span>')
                    }
                    // 判断头像
                    if ($('#headimgUploadFile-Headportrait').attr('src') == '') {
                        arr.push('请上传头像')
                        $('#headimgUploadFile-Headportrait2').html('<span style="color: red"> 请上传头像</span>')
                    }
                    // 判断二维码
                    if ($('#headimgUploadFile-QRcode').attr('src') == '') {
                        arr.push('请上传二维码')
                        $('#headimgUploadFile-QRcode2').html('<span style="color: red"> 请上传二维码</span>')
                    }
                    // 判断媒体级别
                    var mediaLevel = 0;
                    $('#mediaLevel input').each(function () {
                        if ($(this).prop('checked') == false) {
                            mediaLevel++;
                        }
                    })
                    if (mediaLevel > 1) {
                        arr.push('请选择媒体级别');
                        $('#mediaLevelinfo').html("请选择媒体级别")
                    }
                    // 判断微信认证
                    var weChatConfirm = 0;
                    $('#weChatConfirm input').each(function () {
                        if ($(this).prop('checked') == false) {
                            weChatConfirm++;
                        }
                    })
                    if (weChatConfirm > 1) {
                        arr.push('请选择微信认证');
                        $('#weChatConfirminfo').html("请选择微信认证")
                    }
                    // 判断描述／签名
                    if ($.trim($('.sign').val()) == '') {
                        arr.push('请填写描述／签名');
                        $('#signinfo').html("请填写描述／签名")
                    }

                }

                if (_this.GetRequest().WxID == 0) {
                    // 判断微信号
                    if ($('#Microsignalfilling1').val() == '') {
                        arr.push('请输入微信号')
                        $('#Microsignalfilling2').html('<span style="color: red"> 请输入微信号</span>')
                    }
                    // 判断微信名称
                    if ($('#wechatname1').val() == '') {
                        arr.push('请输入微信名称')
                        $('#wechatname2').html('<span style="color: red"> 请输入微信名称</span>')
                    }
                    // 判断头像
                    if ($('#headimgUploadFile-Headportrait').attr('src') == '') {
                        arr.push('请上传头像')
                        $('#headimgUploadFile-Headportrait2').html('<span style="color: red"> 请上传头像</span>')
                    }
                }

                // 判断是否选择主分类
                var classa = true;
                $('#ification [mainclass]').each(function () {
                    if ($(this).attr('mainclass') == '-1') {
                        classa = false;
                        return false;
                    }
                })
                if ($('#ification li').length != 0) {
                    if (classa) {
                        layer.msg('请设置一个主分类');
                        return false;
                    }
                }

                // 判断是否修改
                // 粉丝数
                var Fans = $('#Fans').val();
                // 粉丝数截图
                var headimgUploadFile = $('#headBigImg').attr("src");
                // 常见分类
                var arrification = []
                $('#ification li[name]').each(function () {
                    arrification.push($(this).attr('dictid'))
                })
                // 粉丝分布区域
                var arrRenfansdistri = []
                $('#Renfansdistri li[provinceid]').each(function () {
                    arrRenfansdistri.push($(this).attr('provinceid') + '-' + $(this).attr('provinceper'))
                })
                console.log(arrRenfansdistri);
                // 分布区域截图
                var headimgUploadFile2 = $('#headBigImg2').attr("src")
                // 粉丝性别比例
                var male = $('#male').val()
                var female = $('#female').val()
                // 性别比例截图
                var headimgUploadFile3 = $('#headBigImg3').attr("src")
                // 所在地区
                var locationArea1 = $('#locationArea1 option:checked').attr("value")
                var locationArea2 = $('#locationArea2 option:checked').attr("value")
                var Contrast = {
                    "FansCount": Fans - 0,
                    "FansMalePer": male - 0,
                    "FansFemalePer": female - 0,
                    "CommonlyClass": arrification.length ? arrification : null,
                    "FansArea": arrRenfansdistri.length ? arrRenfansdistri : null,
                    "FansAreaShotUrl": headimgUploadFile2 == '' ? null : headimgUploadFile2,
                    "FansSexScaleUrl": headimgUploadFile3 == '' ? null : headimgUploadFile3
                }
                var FansAreaData = [];
                if (data.Result.FansArea != null) {
                    for (var i = 0; i < data.Result.FansArea.length; i++) {
                        FansAreaData.push(data.Result.FansArea.ProvinceID + '-' + data.Result.FansArea.UserScale)
                    }
                }
                var ContrastData = {
                    "FansCount": data.Result.FansCount,
                    "FansMalePer": data.Result.FansMalePer,
                    "FansFemalePer": data.Result.FansFemalePer,
                    "CommonlyClass": data.Result.CommonlyClass,
                    "FansArea": FansAreaData.length ? FansAreaData : null,
                    "FansAreaShotUrl": data.Result.FansAreaShotUrl,
                    "FansSexScaleUrl": data.Result.FansSexScaleUrl
                }
                if (CTLogin.RoleIDs == 'SYS001RL00005') {
                    // 微信号
                    Contrast.Number = $('#Microsignalfilling1').val();
                    // 微信名称
                    Contrast.Name = $('#wechatname1').val();
                    // 头像
                    Contrast.HeadIconURL = $('#headBigImg-Headportrait').attr('src');
                    // 二维码
                    Contrast.TwoCodeURL = $('#headBigImg-QRcode').attr('src');
                    // 媒体级别
                    $('#mediaLevel input').each(function () {
                        if ($(this).prop('checked') == true) {
                            Contrast.LevelType = $(this).attr('value')
                        }
                    })
                    // 微信认证
                    $('#weChatConfirm input').each(function () {
                        if ($(this).prop('checked') == true) {
                            Contrast.IsAuth = $(this).attr('data-id') ? true : false;
                        }
                    })
                    // 描述签名
                    Contrast.Sign = $('.sign').val();

                    ContrastData.Number = data.Result.Number;
                    ContrastData.Name = data.Result.Name;
                    ContrastData.HeadIconURL = data.Result.HeadIconURL;
                    ContrastData.TwoCodeURL = data.Result.TwoCodeURL;
                    ContrastData.LevelType = data.Result.LevelType;
                    ContrastData.IsAuth = data.Result.IsAuth;
                    ContrastData.Sign = data.Result.Sign;
                    if (JSON.stringify(Contrast) == JSON.stringify(ContrastData)) {
                        if (operatetype == 2) {
                            layer.msg('您未对媒体信息进行修改，不能提交审核');
                            return false
                        }
                    }
                } else {
                    if (_this.GetRequest().WxID == 0) {
                        // 微信号
                        Contrast.Number = $('#Microsignalfilling1').val();
                        // 微信名称
                        Contrast.Name = $('#wechatname1').val();
                        // 头像
                        Contrast.HeadIconURL = $('#headBigImg-Headportrait').attr('src');
                        ContrastData.Number = data.Result.Number;
                        ContrastData.Name = data.Result.Name;
                        ContrastData.HeadIconURL = data.Result.HeadIconURL;
                    }
                    if (JSON.stringify(Contrast) == JSON.stringify(ContrastData)) {
                        if (operatetype == 2) {
                            layer.msg('您未对媒体信息进行修改，不能提交审核');
                            return false
                        }
                    }
                }
                ;

                if (arr.length == 0) {

                    console.log(1);
                    console.log(_this.Getparameters());
                    setAjax({
                        url: 'http://www.chitunion.com/api/media/curd?v=1_1',
                        type: 'POST',
                        data: _this.Getparameters()
                    }, function (data) {
                        if (data.Status == 0) {
                            layer.msg('提交成功');
                            if (_this.GetRequest().OAuthType != undefined) {
                                window.location = '/PublishManager/addEditPublishForWeiChat.html?MediaID=' + _this.GetRequest().WxID + '&entrance=1';
                            } else {
                                window.location = '/PublishManager/addEditPublishForWeiChat.html?MediaID=' + data.Result.MediaId + '&entrance=1';
                            }

                        }
                    })

                } else if (arr.length == 1 && arr[0] == " 提示：授权的数据不能修改，系统会自动更新") {
                    console.log(1);
                    console.log(_this.Getparameters());
                    setAjax({
                        url: 'http://www.chitunion.com/api/media/curd?v=1_1',
                        type: 'POST',
                        data: _this.Getparameters()
                    }, function (data) {
                        if (data.Status == 0) {
                            layer.msg('提交成功')
                            if (_this.GetRequest().OAuthType != undefined) {
                                window.location = '/PublishManager/addEditPublishForWeiChat.html?MediaID=' + _this.GetRequest().WxID + '&entrance=1';
                            } else {
                                window.location = '/PublishManager/addEditPublishForWeiChat.html?MediaID=' + data.Result.MediaId + '&entrance=1';
                            }
                        }
                    })

                } else {
                    // arr=arr.join('/')
                    // $('.grey').html('<img src="/ImagesNew/icon21.png"/> 提示：'+arr)
                    console.log(arr);
                    console.log(2);
                    return
                }
            })
        },
        // 获取参数
        Getparameters: function () {
            var _this = this
            // 微信名称
            var wechat = $('.wechat_name span').eq(0).html()
            // 微信号
            var wechatnumber = $('.wechat_name span').eq(1).html()
            // 头像
            var Headportrait = $('.frame_n  img').attr('src')
            // 粉丝数
            var fans = $('#Fans').val();
            // 粉丝数截图
            var headimgUploadFile = $('#headBigImg').attr("src");
            // 常见分类
            var arrification = []
            $('#ification li[name]').each(function () {
                arrification.push($(this).attr('dictid') + $.trim($(this).attr('mainclass')))
            })
            arrification = arrification.join(',')
            // 粉丝分布区域
            var arrRenfansdistri = []
            $('#Renfansdistri li[provinceid]').each(function () {
                arrRenfansdistri.push($(this).attr('provinceid') + '-' + $(this).attr('provinceper'))
            })
            arrRenfansdistri = arrRenfansdistri.join(',')
            // 分布区域截图
            var headimgUploadFile2 = '';
            if (arrRenfansdistri != '') {
                var headimgUploadFile2 = $('#headBigImg2').attr("src");
            }
            // 粉丝性别比例
            var male = $('#male').val()
            var female = $('#female').val()
            // 性别比例截图
            var headimgUploadFile3 = $('#headBigImg3').attr("src")
            // 所在地区
            var locationArea1 = $('#locationArea1 option:checked').attr("value")
            var locationArea2 = $('#locationArea2 option:checked').attr("value")
            // 覆盖地区
            var fgdq_1 = $('#fgdq_1 option:checked').attr("value");
            var fgdq_2 = $('#fgdq_2 option:checked').attr("value");
            // 参数
            var operatetype, oauthtype;
            if (_this.GetRequest().OAuthType != undefined) {
                operatetype = 2

                oauthtype = _this.GetRequest().OAuthType;
            } else {
                operatetype = 1

                oauthtype = _this.GetRequest().AuthType
            }
            var obj =
                {
                    "businesstype": 14001,
                    "OperateType": operatetype,
                    "WeiXin": {
                        businesstype: 14001,
                        Number: wechatnumber,
                        Name: wechat,
                        HeadIconURL: Headportrait,
                        FansCount: fans - 0,
                        CommonlyClass: arrification,
                        AuthType: oauthtype - 0,
                    }
                }
            if (CTLogin.RoleIDs == "SYS001RL00003") {
                obj.WeiXin.FansCountUrl = headimgUploadFile
            }
            if (operatetype == 2) {
                obj.WeiXin.MediaId = _this.GetRequest().WxID - 0;
            }
            // 判断分布区域截图是否为空
            if (headimgUploadFile2 != '') {
                obj.WeiXin.FansArea = arrRenfansdistri;
                obj.WeiXin.FansAreaShotUrl = headimgUploadFile2;
            }
            if (CTLogin.RoleIDs == 'SYS001RL00001' || CTLogin.RoleIDs == 'SYS001RL00004' || CTLogin.RoleIDs == 'SYS001RL00005') {
                obj.WeiXin.FansArea = arrRenfansdistri;
            }
            // 判断性别比例截图是否为空
            if (headimgUploadFile3 != '') {
                if (male != '' && female != '') {
                    obj.WeiXin.FansMalePer = male;
                    obj.WeiXin.FansFemalePer = female;
                    obj.WeiXin.FansSexScaleUrl = headimgUploadFile3;
                }
                if (male != '') {
                    obj.WeiXin.FansMalePer = male;
                    obj.WeiXin.FansSexScaleUrl = headimgUploadFile3;
                }
                if (female != '') {
                    obj.WeiXin.FansFemalePer = female;
                    obj.WeiXin.FansSexScaleUrl = headimgUploadFile3;
                }
            }
            if (GetRequest().AuthType == 38001 || GetRequest().AuthType == 38002 || GetRequest().AuthType == 38004) {
                obj.WeiXin.FansMalePer = male;
                obj.WeiXin.FansFemalePer = female;

            }
            if (CTLogin.RoleIDs == "SYS001RL00005" || CTLogin.RoleIDs == "SYS001RL00001" || CTLogin.RoleIDs == "SYS001RL00004") {
                obj.WeiXin.FansMalePer = $.trim(male);
                obj.WeiXin.FansFemalePer = $.trim(female);
                obj.WeiXin.FansArea = arrRenfansdistri;
            }
            // 判断所在城市是否所选

            if (locationArea1 != -2 && locationArea2 != -2) {
                obj.WeiXin.ProvinceID = locationArea1
                obj.WeiXin.CityID = locationArea2
            }
            // ae手工过来的
            if (_this.GetRequest().AuthType == 38003 || _this.GetRequest().wxae == 1) {
                // 微信号
                obj.WeiXin.Number = $('#Microsignalfilling1').val();
                // 微信名称
                obj.WeiXin.Name = $('#wechatname1').val();
                // 头像
                obj.WeiXin.HeadIconURL = $('#headBigImg-Headportrait').attr('src');
                // 二维码
                obj.WeiXin.TwoCodeURL = $('#headBigImg-QRcode').attr('src');
                // 媒体级别
                $('#mediaLevel input').each(function () {
                    if ($(this).prop('checked') == true) {
                        obj.WeiXin.LevelType = $(this).attr('value')
                    }
                })
                // 微信认证
                $('#weChatConfirm input').each(function () {
                    if ($(this).prop('checked') == true) {
                        obj.WeiXin.IsAuth = $(this).attr('data-id') ? true : false;
                    }
                })
                // 描述签名
                obj.WeiXin.Sign = $('.sign').val();
            }
            ;
            if(_this.GetRequest().AuthType == 38004 || _this.GetRequest().OAuthType == 38004){
                obj.WeiXin.TwoCodeURL = $('#headBigImg-QRcode').attr('src');
            }
            if (_this.GetRequest().WxID == 0) {
                // 微信号
                obj.WeiXin.Number = $('#Microsignalfilling1').val();
                // 微信名称
                obj.WeiXin.Name = $('#wechatname1').val();
                // 头像
                obj.WeiXin.HeadIconURL = $('#headBigImg-Headportrait').attr('src');
            }
            // 下单备注
            // var OrderRemark = [];
            // $('#Ordermemo li:last span input').each(function () {
            //     if ($(this).prop('checked') == true) {
            //         OrderRemark.push($(this).attr('dictid'))
            //     }
            // })
            // if (OrderRemark.length > 0) {
            //     var OrderRemark1=[];
            //     for(var i=0;i<OrderRemark.length;i++){
            //         if(OrderRemark[i]==40009){
            //             OrderRemark1.push({
            //                 "Id": OrderRemark[i]-0,
            //                 "Descript": $('#Other').val()
            //         })
            //         }else {
            //             OrderRemark1.push({
            //                 "Id": OrderRemark[i]-0,
            //                 "Descript": ''
            //             })
            //         }
            //     }
            //     obj.WeiXin.OrderRemark = OrderRemark1;
            // }

            if ($('#quyumeiti input').eq(1).prop('checked') == true) {
                // 下单备注1
                var OrderRemark = [];
                $('#Ordermemo li:last span input').each(function () {
                    if ($(this).prop('checked') == true) {
                        OrderRemark.push($(this).attr('dictid'))
                    }
                })
                if (OrderRemark.length > 0) {
                    var OrderRemark1 = [];
                    for (var i = 0; i < OrderRemark.length; i++) {
                        if (OrderRemark[i] == 40009) {
                            OrderRemark1.push({
                                "Id": OrderRemark[i] - 0,
                                "Descript": $('#Other').val()
                            })
                        } else {
                            OrderRemark1.push({
                                "Id": OrderRemark[i] - 0,
                                "Descript": ''
                            })
                        }
                    }
                    obj.WeiXin.OrderRemark = OrderRemark1;
                }
                obj.WeiXin.IsAreaMedia = false;
            } else {
                obj.WeiXin.IsAreaMedia = true;
                // 下单备注2
                var OrderRemark_1 = [];
                $('#Ordermemo2 span input').each(function () {
                    if ($(this).prop('checked') == true) {
                        OrderRemark_1.push($(this).attr('dictid'))
                    }
                })
                if (OrderRemark_1.length > 0) {
                    var OrderRemark_2 = [];
                    for (var i = 0; i < OrderRemark_1.length; i++) {
                        if (OrderRemark_1[i] == 40009) {
                            OrderRemark_2.push({
                                "Id": OrderRemark_1[i] - 0,
                                "Descript": $('#Other').val()
                            })
                        } else {
                            OrderRemark_2.push({
                                "Id": OrderRemark_1[i] - 0,
                                "Descript": ''
                            })
                        }
                    }
                    obj.WeiXin.OrderRemark = OrderRemark_2;
                }
                ;

                // 覆盖地区
                obj.WeiXin.AreaMedia = [{
                    "CityId": fgdq_2,
                    "CityName": $('#fgdq_2 option:checked').html() == '城市' ? '' : $('#fgdq_2 option:checked').html(),
                    "ProvinceId": fgdq_1,
                    "ProvinceName": $('#fgdq_1 option:checked').html() == '省/直辖市' ? '' : $('#fgdq_1 option:checked').html()
                }]

            }
            // 资质信息
            if (_this.GetRequest().AuthType == 38004 || _this.GetRequest().OAuthType == 38004) {

                // 描述／签名
                obj.WeiXin.Sign = $('#sign textarea').val();
                if (_this.GetRequest().q != 1) {
                    // obj.WeiXin.(未知)
                    // 代理授权资质
                    obj.WeiXin.QualificationOne = $('#headBigImg-authorization').attr('src');
                    obj.WeiXin.QualificationTwo = $('#headBigImg-authorization1').attr('src');
                    // obj.WeiXin.(未知)
                    // 企业名称
                    obj.WeiXin.EnterpriseName = $('#Enterprisename1').val()
                    // obj.WeiXin.(未知)
                    // 营业执照
                    obj.WeiXin.BusinessLicense = $('#headBigImg-Businesslicense').attr('src')
                    // obj.WeiXin.(未知)
                }
            }
            if (_this.GetRequest().WxID == 0) {
                // 微信号
                obj.WeiXin.Number = $('#Microsignalfilling1').val();
                // 微信名称
                obj.WeiXin.Name = $('#wechatname1').val();
                // 头像
                obj.WeiXin.HeadIconURL = $('#headBigImg-Headportrait').attr('src');
            }
            if (operatetype == 1) {
                obj.WeiXin.MediaId = _this.GetRequest().WxID - 0;
            }

            if (_this.GetRequest().AuditStatus == 43004) {
                obj.OperateType = 2
                obj.WeiXin.MediaId = _this.GetRequest().WxID - 0;
            }
            console.log(obj);
            return obj
        }
    }
    var addeditmedia = new Addeditmedia()


});