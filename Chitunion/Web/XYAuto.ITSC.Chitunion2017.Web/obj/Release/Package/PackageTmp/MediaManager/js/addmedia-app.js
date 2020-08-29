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



// 相关资质上传
function uploadImg(id,img,imgerr,bigImg) {
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

    $(document).ready(function (){
        $('#'+id).uploadify({
            'auto': true,
            'multi': false,
            'swf': '/Js/uploadify.swf?_=' + Math.random(),
            'uploader': '/AjaxServers/UploadFile.ashx',
            // 'buttonImage':'/images/icon63.png',
            'buttonText': ' ',
            // 'buttonClass': 'allBtn_file',
            'width': 75,
            'height': 73,
            'fileTypeDesc': '支持格式:xls,jpg,jpeg,png.gif',
            'fileTypeExts': '*.xls;*.jpg;*.jpeg;*.png;*.gif;',
            fileSizeLimit:'2MB',
            'fileCount':1,
            'queueSizeLimit': 1,
            queueID:'imgShow',
            'scriptAccess': 'always',
            'overrideEvents' : [ 'onDialogClose'],
            'formData': { Action: 'BatchImport', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')), IsGenSmallImage: 1 },
            'onQueueComplete': function (event, data) {
                //enableConfirmBtn();
            },
            'onQueueFull': function () {
                alert('您最多只能上传1个文件！');
                return false;
            },
            //检测FLASH失败调用
            'onFallback':function(){
                $('#'+imgerr).html('您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。');
            },
            //上传成功后返回的信息
            'onUploadSuccess': function (file, data, response) {
                if (response == true) {
                    var json = $.evalJSON(data);

                    var imgUrlsTotal = json.Msg.split('|');
                    var smallImgUrl = imgUrlsTotal[1];
                    var bigImgUrl = imgUrlsTotal[0];

                    //小图
                    $('#'+img).attr('src',smallImgUrl);
                    //大图

                    $('#'+bigImg).find('img').attr("src",bigImgUrl);

                    //上传成功的提示信息
                    $("#"+imgerr).html('上传成功！').css('color',"#666");
                    //上传成功后警告信息隐藏
                    $("#"+imgerr).next().css("display","none");

                    //鼠标放过显示大图
                    $('#'+id).mousemove(function () {
                        $('#'+bigImg).show();
                    }).mouseout(function () {
                        $('#'+bigImg).hide();
                    })
                    $('.allBtn_file').hide();
                    //AE下的最后一个上传附件  显示名称
                    // if(id == 'lastFileSrc'){
                    //     $('.lastFileUrl').show();
                    //     $('.lastFileName').text(json.FileName);
                    //     $('.lastFileUrl').attr('href',"" + json.Msg.split('|')[0]);
                    //     $('#lastFileSrc').parent().next().hide();
                    // }
                }
            },
            'onProgress': function (event, queueID, fileObj, data) { },
            'onUploadError': function (event, queueID, fileObj, errorObj) {},
            'onSelectError':function(file, errorCode, errorMsg){
                if (errorCode == SWFUpload.UPLOAD_ERROR.FILE_CANCELLED
                    || errorCode == SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED) {
                    return false;
                }
                switch(errorCode) {
                    case -100:
                        $('#'+imgerr).html('<img src="/images/icon21.png">上传图片数量超过1个').css('color',"red");
                        break;
                    case -110:
                        $('#'+imgerr).html('<img src="/images/icon21.png">上传图片大小应小于2MB').css('color',"red");
                        break;
                    case -120:

                        break;
                    case -130:
                        $('#'+imgerr).html('<img src="/images/icon21.png">上传图片类型不正确').css('color',"red");
                        break;
                }
            }
        });
    });
};

$(function () {
    var i=0;
    if(GetRequest().MediaId==undefined){
        document.body.innerHTML='';
        layer.msg('媒体id不能为空',{time:1300},function () {
            window.location='/mediamanager/mediaapp.html';
            i=1;
        });
    };
    if(GetRequest().OperateType==1){

        if(GetRequest().appName==undefined){
            document.body.innerHTML='';
            layer.msg('媒体名称不能为空',{time:1300},function () {
                window.location='/mediamanager/mediaapp.html';
                i=1;
            });
        };
    }
    if(GetRequest().BaseMediaId==undefined){
        document.body.innerHTML='';
        layer.msg('媒体id不能为空',{time:1300},function () {
            window.location='/mediamanager/mediaapp.html';
            i=1;
        })
    };
    if(i){
        document.body.innerHTML='';
    }

    // 添加app
    function addMediaApp() {
        // app名称
        this.appName()
        // 媒体Logo
        this.medialog();
        // 选择分类
        this.Selectclassify();
        // 覆盖地区
        this.Coveredarea();
        // 所在地区
        this.locationarea();
        // app日活
        this.Dailyliving();
        // 下单备注
        this.Ordermemo();
        // 相关资质
        this.relevantattribute();
        // 提交
        this.Submit();
        // 点击提交并添加广告
        this.ConfirmAdd();

        // 编辑基本信息
        if(GetRequest().OperateType==2||GetRequest().Rendering==1){
            this.Editbasic();
        }
        // 编辑相关资质
        if(CTLogin.RoleIDs=='SYS001RL00003'){
            this.Editorialqualifi();
        }
    }

    addMediaApp.prototype = {
        constructor: addMediaApp,
        /*============公共方法start============*/
        // 获取url？后面的参数
        GetRequest: GetRequest,
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
                    'uploader': '/AjaxServers/UploadFile.ashx',
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
                            $('#medialogo_Prompt').hide();
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

        // 编辑
        // 基本信息
        Editbasic:function () {
            var _this=this;
            setAjax({
                url: '/api/media/GetInfo?v=1_1',
                type: 'get',
                data: {
                    businesstype: 14002,
                    MediaId: GetRequest().MediaId,
                    baseMediaId: GetRequest().BaseMediaId
                }
            }, function (data) {
                // data.Result = {
                //     "CoverageArea": [
                //         {
                //             "ProvinceId": 10,
                //             "ProvinceName": "河南省",
                //             "CityId": 1001,
                //             "CityName": "郑州市"
                //         },
                //         {
                //             "ProvinceId": 11,
                //             "ProvinceName": "安徽省",
                //             "CityId": -2,
                //             "CityName": ""
                //         }
                //     ],
                //     "CommonlyClass": [
                //         {
                //             "CategoryId": 22026,
                //             "CategoryName": "test_categroy",
                //             "SortNumber":1
                //         },
                //         {
                //             "CategoryId": 22014,
                //             "CategoryName": "test_categroy_1",
                //             "SortNumber":0
                //         }
                //     ],
                //     "ProvinceId": 10,
                //     "ProvinceName": "河南省",
                //     "CityId": 1001,
                //     "CityName": "郑州市",
                //     "DailyLive": 10088,
                //     "Remark": "ddddddd",
                //     "OrderRemark": [
                //         {
                //             "Id": 40001,
                //             "Name": "d_name",
                //             "Descript": "描述"
                //         },
                //         {
                //             "Id": 40003,
                //             "Name": "d_name",
                //             "Descript": "描述"
                //         },
                //         {
                //             "Id": 40005,
                //             "Name": "d_name",
                //             "Descript": "描述"
                //         },
                //         {
                //             "Id": 40009,
                //             "Name": "d_name",
                //             "Descript": "描述"
                //         }
                //     ],
                //     "MediaID": 18,
                //     "Number": null,
                //     "Name": "app_test",
                //     "HeadIconURL": "/sss/image"
                // };

                if (data.Result ? data.Result : null){
                    // APP名称
                    $('#app_name').html(data.Result.Name);
                    // 媒体Logo
                    $('#headimgUploadFile').attr('src',data.Result.HeadIconURL);
                    $('#headBigImg').attr('src',data.Result.HeadIconURL);
                    // 常见分类
                    _this.myAjax({
                        url: '/api/DictInfo/GetDictInfoByTypeID',
                        type: 'get',
                        data: {
                            typeID:52
                        }
                    }, function (dat) {

                        var arr = [];
                        console.log(data.Result.CommonlyClass);
                        data.Result.CommonlyClass ? data.Result.CommonlyClass : data.Result.CommonlyClass = [];
                        for (var i = 0; i < data.Result.CommonlyClass.length; i++) {
                            for (var j = 0; j < dat.Result.length; j++) {
                                if (data.Result.CommonlyClass[i].CategoryId == dat.Result[j].DictId) {
                                    arr.push({name: dat.Result[j].DictName, id: dat.Result[j].DictId, sub: j})
                                }
                            }
                        }
                        if (arr.length > 0) {
                            var a = '<li class="ins_a">&nbsp;</li>';
                            var b = 0;
                            for (var i = 0; i < arr.length; i++) {
                                a += '<li name=' + arr[i].sub + ' dictid=' + arr[i].id + ' mainclass="0"><div class="classification">' + arr[i].name + '<span style="display: none"><img src="/ImagesNew/icon50.png"/></span><em class="emclass" style="display:none;position: absolute;bottom: -3px;top:3px;right: 0; z-index: 0;"><img src="/ImagesNew/icon85.png"></em></div></li>';
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

                                $('.emclass').hide().parents('li').attr('mainclass', '0');
                                $('.emclass').eq(0).show().parents('li').attr('mainclass', '1');
                                $('.classification').off('click').on('click', function () {
                                    $('.emclass').hide().parents('li').attr('mainclass', '0');
                                    $(this).find('.emclass').show().parents('li').attr('mainclass', '1');
                                });
                            });
                            $('.emclass').hide().parents('li').attr('mainclass', '-0');
                            $('.emclass').eq(0).show().parents('li').attr('mainclass', '1');
                            $('.classification').off('click').on('click', function () {
                                $('.emclass').hide().parents('li').attr('mainclass', '0');
                                $(this).find('.emclass').show().parents('li').attr('mainclass', '1');
                            });

                        }

                    })
                    // 覆盖地区
                    data.Result.CoverageArea?data.Result.CoverageArea:[];
                    var Covered='';
                    if(data.Result.CoverageArea.length>0){
                            $('#Covered').html('<li class="ins_a">&nbsp;</li><div class="clear"></div>')

                    }
                    for (var i=0;i<data.Result.CoverageArea.length;i++){
                        // 渲染
                        $('#Covered li:last').after('<li style="margin-bottom: 2px" provinceid=' + data.Result.CoverageArea[i].ProvinceId + ' province=' + data.Result.CoverageArea[i].ProvinceName + ' cityid=' + data.Result.CoverageArea[i].CityId + ' city='+data.Result.CoverageArea[i].CityName +'><div class="classification">' + data.Result.CoverageArea[i].ProvinceName +' '+(data.Result.CoverageArea[i].CityName?data.Result.CoverageArea[i].CityName:'')+'<span style="display: none"><img src="/ImagesNew/icon50.png"/></span></div></li>');
                    };
                    // 鼠标经过分类显示和隐藏关闭
                    $('#Covered li').off('mouseover').on('mouseover', function () {
                        $(this).find('span').show();
                    }).off('mouseout').on('mouseout', function () {
                        $(this).find('span').hide();
                    }).find('span').off('click').on('click', function () {//点击span关闭
                        // 移除当前li
                        $(this).parent().parent().remove();
                        // 判断是否还有选择的分类，根据span判断
                        if ($('#Covered li span').length == 0) {
                            $('#Covered').html('');
                        }
                    })
                    // 所在地区
                    // 省
                    if (data.Result.ProvinceId!=-2 ? data.Result.ProvinceId : '' != '') {
                        $('#locationArea1 option').each(function () {
                            if ($(this).attr('value') == data.Result.ProvinceId) {
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
                    if (data.Result.CityId ? data.Result.CityId : '' != '') {
                        $('#locationArea2 option').each(function () {
                            if ($(this).attr('value') == data.Result.CityId) {
                                console.log($(this));
                                $(this).prop('selected', true)
                            }
                        })
                    }

                    // app日活
                    $('#Daily').val(data.Result.DailyLive);
                    // 媒体介绍
                    $('#introduce textarea').val(data.Result.Remark)
                    // 下单备注
                    if (data.Result.OrderRemark ? data.Result.OrderRemark : '' != '') {
                        setAjax({
                            url: '/api/DictInfo/GetDictInfoByTypeID',
                            type: 'get',
                            data: {
                                typeID: 40
                            }
                        }, function (data1) {
                            var a = '';
                            var p=[]
                            for (var i=0;i<data.Result.OrderRemark.length;i++){
                               p.push(data.Result.OrderRemark[i].Id)
                            }
                            console.log(p);
                            $(data1.Result).each(function () {
                                var b = ' ';
                                if (p.indexOf($(this)[0].DictId)!=-1) {
                                    b = 'checked';
                                }
                                a += '<span><input DictId="' + $(this)[0].DictId + '" ' + b + '  type="checkbox" > ' + $(this)[0].DictName + '</span>';
                            })
                            a += '<textarea name="" id="Other"  cols="" rows="5" style="width:140px;height:20px;resize:none;display: none;float:right;margin-right: -30px;"></textarea>'
                            $('.answer').html(a);
                            if($('.answer span input:last').prop('checked')==true){
                                for (var i=0;i<data.Result.OrderRemark.length;i++){
                                    if(data.Result.OrderRemark[i].Id==40009){
                                        $('#Other').show().html(data.Result.OrderRemark[i].Descript)
                                    }
                                }
                            }else {
                                $('#Other').hide();

                            }
                            // 显示隐藏其他文本域
                            $('.answer span:last input').off('click').on('click',function () {
                                if($(this).prop('checked')==true){
                                    $(this).parent().next().show();
                                }else {
                                    $(this).parent().next().hide();
                                }
                            })
                        })
                    }
                }
            })
        },


        // 资质信息
        Editorialqualifi:function () {

            setAjax({
                url:'/api/Media/GetAppQualification?v=1_1',
                type:'get',
                data:{
                    MediaType:14002,
                    MediaID:GetRequest().MediaId
                }
            },function (data) {
                if(GetRequest().OperateType==1){
                    if(data.Result.MediaRelationsName==null){
                        if(data.Result.OperatingTypeName=='个人'){
                            data.Result.MediaRelationsName='自有'
                        }
                        if(data.Result.OperatingTypeName=='企业'){
                            data.Result.MediaRelationsName='代理'
                        }
                    }
                }
                if (data.Result ? data.Result : null){
                    if(data.Result.MediaRelationsName=='代理'){
                        // 企业名称
                        $('#enterprise').val(data.Result.EnterpriseName);
                        // 营业执照
                        if(data.Result.BusinessLicense){
                            $('#headimgUploadFile_Businesslicense').attr('src',data.Result.BusinessLicense);
                            $('#bigImgshow_Businesslicense img').attr('src',data.Result.BusinessLicense);
                        }
                        // 代理合同
                        if(data.Result.Q1){
                            $('#headimgUploadFile_homepage').attr('src',data.Result.Q1);
                            $('#bigImgshow_homepage img').attr('src',data.Result.Q1);
                        }
                        if(data.Result.Q2){
                            $('#headimgUploadFile_Shadowe').attr('src',data.Result.Q2);
                            $('#bigImgshow_Shadowe img').attr('src',data.Result.Q2);
                        }

                        if(!data.Result.CanEdit){
                            // 移除示例
                            $('#Businesslicense_Sample').remove()
                        }

                    }
                    if(data.Result.MediaRelationsName=='自有'){
                        if(data.Result.OperatingTypeName=='企业'){
                            // 选中自有不选中代理
                            $('#Mediarelations_agent div').attr('class','default_ifi');
                            $('#Mediarelations_have div').attr('class','current_ifi');
                            // 选中企业不选中个人
                            $('#Operatortype_personal div').attr('class','default_ifi').css('color', 'rgb(102, 102, 102)');
                            $('#Operatortype_enterprise div').attr('class','current_ifi');
                            // 显示企业隐藏合同
                            $('#Certificate').hide();
                            $('#enterprise_information').show();
                            $('#Mediarelations_have div').click();
                            // 企业名称
                            $('#enterprise').val(data.Result.EnterpriseName);
                            // 营业执照
                            if(data.Result.BusinessLicense){
                                $('#headimgUploadFile_Businesslicense').attr('src',data.Result.BusinessLicense);
                                $('#bigImgshow_Businesslicense img').attr('src',data.Result.BusinessLicense);
                            }

                            if(!data.Result.CanEdit){
                                // 移除示例
                                $('#Businesslicense_Sample').remove()
                            }
                        }
                        if(data.Result.OperatingTypeName=='个人'){
                            // 选中自有不选中代理
                            $('#Mediarelations_agent div').attr('class','default_ifi');
                            $('#Mediarelations_have div').attr('class','current_ifi');
                            // 选中个人不选中企业
                            $('#Operatortype_personal div').attr('class','current_ifi').css('color', 'rgb(102, 102, 102)');
                            $('#Operatortype_enterprise div').attr('class','default_ifi');
                            // 显示个人隐藏企业
                            $('#enterprise_information').hide();
                            $('#personal_information').show();
                            $('#Mediarelations_have div').click();
                            // 真实姓名
                            $('#Realname').val(data.Result.EnterpriseName);
                            // 手持身份证
                            if(data.Result.Q1){
                                $('#headimgUploadFile_Positive').attr('src',data.Result.Q1);
                                $('#bigImgshow_Positive img').attr('src',data.Result.Q1);
                            }
                        }
                    };

                    // 鼠标移入显示大图
                    function showImg() {
                        $("#Businesslicense").each(function () {
                            $(this).mousemove(function () {
                                if ($.trim($(this).find('img:eq(0)').attr("src")) != "/ImagesNew/add2.png") {
                                    $(this).find('span:eq(0)').show();
                                }
                            }).mouseout(function () {
                                $(this).find('span:eq(0)').hide();
                            })
                        })
                    }
                    showImg();
                    console.log((!data.Result.CanEdit));
                    console.log(data.Result.OperatingTypeName == '个人');
                    if((!data.Result.CanEdit)&&data.Result.OperatingTypeName=='个人'){
                        console.log(1);
                        var Mediarelations=$('#Mediarelations').height();
                        var h3=$('#Mediarelations').prev('h3').height();
                        var Relevantqualifications=$('#Relevantqualifications').innerHeight()
                        var Certificate=$('#Certificate').outerHeight();
                        var grey=$('.grey').parent().outerHeight();
                        var zuizhong=Relevantqualifications-Certificate-grey;
                        if(data.Result.MediaRelationsName=='代理'){
                            $('#zedang').css({
                                background:'rgba(0,0,0,0)',
                                height:zuizhong-Mediarelations-h3+'px',
                                marginTop:Mediarelations+h3+10+'px',
                                width:'100%',
                                position:'absolute',
                                left:0,
                                top:0,
                                'z-index':10
                            }).show();
                        }else {
                            $('#zedang').css({
                                background:'rgba(0,0,0,0)',
                                height:'100%',
                                // marginTop:Mediarelations+h3+10+'px',
                                width:'100%',
                                position:'absolute',
                                left:0,
                                top:0,
                                'z-index':10
                            }).show();
                        }

                    }else if((!data.Result.CanEdit)&&data.Result.OperatingTypeName!='个人'){
                        console.log(2);
                        var Mediarelations=$('#Mediarelations').height();
                        var h3=$('#Mediarelations').prev('h3').height();
                        var Relevantqualifications=$('#Relevantqualifications').innerHeight();
                        var Certificate=$('#Certificate').outerHeight();
                        var grey=$('.grey').parent().outerHeight();
                        var zuizhong=Relevantqualifications-Certificate-grey;
                        if(data.Result.MediaRelationsName=='代理'){
                            $('#zedang').css({
                                background:'rgba(0,0,0,0)',
                                height:zuizhong-Mediarelations-h3+'px',
                                marginTop:Mediarelations+h3+10+'px',
                                width:'100%',
                                position:'absolute',
                                left:0,
                                top:0,
                                'z-index':10
                            }).show();
                        }else {
                            $('#zedang').css({
                                background:'rgba(0,0,0,0)',
                                height:zuizhong-Mediarelations-h3+'px',
                                marginTop:Mediarelations+h3+10+'px',
                                width:'100%',
                                position:'absolute',
                                left:0,
                                top:0,
                                'z-index':10
                            }).show();
                        }
                    }
                }
            })
        },

        // APP名称
        appName:function () {
            $('#app_name').html(decodeURIComponent(GetRequest().appName));
            // 媒体介绍
            $('#introduce textarea').off('keyup').on('keyup',function () {
                $('#introduce_Prompt').hide();
            })
        },
        // 媒体Logo
        medialog: function () {
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
                    url: "Selectclassify-app.html",
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
                                    a += '<li name=' + $(this).attr('name') + ' dictid=' + $(this).attr('dictid') + ' mainclass="0"><div class="classification">' + $(this).html() + '<span style="display: none;z-index: 2;"><img src="/ImagesNew/icon50.png"/></span><em class="emclass" style="display:none;position: absolute;bottom: -3px;top:3px;right: 0; z-index: 0;"><img src="/ImagesNew/icon85.png"></em></div></li>';
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
                                $('.emclass').hide().parents('li').attr('mainclass', '0');
                                $('.emclass').eq(0).show().parents('li').attr('mainclass', '1');
                                $('.classification').off('click').on('click', function () {
                                    $('.emclass').hide().parents('li').attr('mainclass', '0');
                                    $(this).find('.emclass').show().parents('li').attr('mainclass', '1');
                                });
                            });
                            $('.emclass').hide().parents('li').attr('mainclass', '0');
                            $('.emclass').eq(0).show().parents('li').attr('mainclass', '1');
                            $('.classification').off('click').on('click', function () {
                                $('.emclass').hide().parents('li').attr('mainclass', '0');
                                $(this).find('.emclass').show().parents('li').attr('mainclass', '1');
                            });
                        })

                    }
                });

                $('#common_Prompt').hide();
            });
        },
        // 覆盖地区
        Coveredarea: function () {
            var _this = this;
            // 省/直辖市
            $(_this.JSonData.masterArea).each(function (i) {
                $('#locationArea-Covered1').append('<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>')
            })
            // 城市
            $('#locationArea-Covered1').off('change').on('change', function () {
                var City1 = '', City2 = '<option value="-2">城市</option>';
                if($(this).val()!=0){
                    $($(_this.JSonData.masterArea)[$('#locationArea-Covered1 option:checked').attr('i')].subArea).each(function (i) {
                        City1 += '<option i=' + i + ' value=' + this.id + '>' + this.name + '</option>'
                    })
                }
                $('#locationArea-Covered2').html(City2 + City1)
            });
            // 点击添加
            $('#Coveredadd').off('click').on('click', function () {
                // 每次点击清空提示
                $('#Contain-Prompt').html('');
                // 获取
                var Provinceid = $('#locationArea-Covered1 option:checked').val();
                var Province = $('#locationArea-Covered1 option:checked').html();
                var cityid = $('#locationArea-Covered2 option:checked').val();
                var city = $('#locationArea-Covered2 option:checked').html();
                // 判断直接选择省/直辖市
                if($('#locationArea-Covered1').val()=='-2'){
                    return false;
                }
                // 判断全国的时候不能加别的省
                if($('#Covered li:last').attr('province')=='全国'){
                    $('#Contain-Prompt').html('所选区域不能有包含关系');
                    return false;
                }
                // 判断添加别的省不能填全国
                if($('#Covered li').length!=0){
                    if($('#Covered li:eq(1)')!='全国'){
                        if(Province=='全国'){
                            $('#Contain-Prompt').html('所选区域不能有包含关系');
                            return false;
                        }
                    }
                }

                // 判断只选省份不选市
                if($('#locationArea-Covered2 option').eq(0).prop('selected')==true){
                    city=' ';
                }
                // 判断不能存在包含关系
                var Contain=false;
                $('#Covered li').each(function () {
                    // 判断省但不包含市
                    if($(this).attr('province')==Province&&city==' '){
                        Contain=true;
                        $('#Contain-Prompt').html('所选区域不能有包含关系');
                    }
                    // 判断省包含市
                    if($(this).attr('province')==Province&&city!=' '){
                        if($(this).attr('province')==Province&&$(this).attr('city')==city){
                            Contain=true;
                            $('#Contain-Prompt').html('所选区域不能有包含关系');
                        }
                        if($(this).attr('province')==Province&&$(this).attr('city')==''){
                            if($(this).attr('province')==Province){
                                Contain=true;
                                $('#Contain-Prompt').html('所选区域不能有包含关系');
                            }
                        }
                    }
                })
                if(Contain){
                    return false;
                }
                // 初始渲染
                if ($('#Covered li').attr('class') != 'ins_a') {
                    $('#Covered').html('<li class="ins_a">&nbsp;</li><div class="clear"></div>')
                }
                // 渲染
                $('#Covered li:last').after('<li style="margin-bottom: 2px" provinceid=' + Provinceid + ' province=' + Province + ' cityid=' + cityid + ' city='+city +'><div class="classification">' + Province +' '+city+'<span style="display: none"><img src="/ImagesNew/icon50.png"/></span></div></li>');
                // 清空
                $('#locationArea-Covered1 option').eq(0).prop('selected', true);
                $('#locationArea-Covered2').html('<option value="-2">城市</option>');
                // 鼠标经过分类显示和隐藏关闭
                $('#Covered li').off('mouseover').on('mouseover', function () {
                    $(this).find('span').show();
                }).off('mouseout').on('mouseout', function () {
                    $(this).find('span').hide();
                }).find('span').off('click').on('click', function () {//点击span关闭
                    // 移除当前li
                    $(this).parent().parent().remove();
                    // 判断是否还有选择的分类，根据span判断
                    if ($('#Covered li span').length == 0) {
                        $('#Covered').html('');
                    }
                })
            });
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
        // app日活
        Dailyliving: function () {
            var _this=this;
            $('#Daily').off('keyup').on('keyup',function () {
                if(!_this.executeRegular($(this).val(),/^\+?[1-9]\d*$/)){
                    $(this).val('');
                    $('#Daily-Prompt').html('请输入大于零的正整数')
                }else {
                    $('#Daily-Prompt').html('')
                }
            });
        },
        // 下单备注
        Ordermemo: function () {
            setAjax({
                url: '/api/DictInfo/GetDictInfoByTypeID',
                type: 'get',
                data: {
                    typeID: 40
                }
            }, function (data) {
                var a = ''
                $(data.Result).each(function () {
                    a += '<span><input DictId="' + $(this)[0].DictId + '" type="checkbox" > ' + $(this)[0].DictName + '</span>';
                })
                a += '<textarea name="" id="Other"  cols="" rows="5" style="width:140px;height:20px;resize:none;display: none;float: right;margin-right: -30px"></textarea>'
                $('.answer').html(a);

                // 显示隐藏其他文本域
                $('.answer span:last input').off('click').on('click',function () {
                    if($(this).prop('checked')==true){
                        $(this).parent().next().show();
                    }else {
                        $(this).parent().next().hide();
                    }
                })
            })
        },
        // 相关资质
        relevantattribute: function () {
            // 媒体关系
            // 点击代理
            $('#Mediarelations_agent').off('click').on('click',function () {
                // 移除个人的点击事件
                $('#Operatortype_personal').unbind('click');
                // 显示代理合同
                $('#Certificate').show();
                // 代理有样式 自有没样式
                $(this).find('div').attr('class','current_ifi');
                $('#Mediarelations_have').find('div').attr('class','default_ifi');

                // 运营者类型
                // 个人
                $('#Operatortype_personal').find('div').attr('class','default_ifi').css('color','#aaa');
                $('#personal_information').hide();
                // 企业
                $('#Operatortype_enterprise').find('div').attr('class','current_ifi');
                $('#enterprise_information').show();
            });
            // 点击自有
            $('#Mediarelations_have').off('click').on('click',function () {
                // 隐藏代理合同
                $('#Certificate').hide();
                // 代理没有样式 自有样式
                $(this).find('div').attr('class','current_ifi');
                $('#Mediarelations_agent').find('div').attr('class','default_ifi');
                $('#Operatortype_personal').find('div').css('color','#666');
                // 运营者类型
                // 点击个人
                $('#Operatortype_personal').off('click').on('click',function () {
                    // 企业没有样式 个人样式
                    $(this).find('div').attr('class','current_ifi');
                    $('#Operatortype_enterprise').find('div').attr('class','default_ifi');
                    // 个人显示,企业隐藏
                    $('#enterprise_information').hide();
                    $('#personal_information').show();
                });
                // 点击企业
                $('#Operatortype_enterprise').off('click').on('click',function () {
                    // 企业有样式 个人没样式
                    $(this).find('div').attr('class','current_ifi');
                    $('#Operatortype_personal').find('div').attr('class','default_ifi');
                    // 企业显示,个人隐藏
                    $('#enterprise_information').show();
                    $('#personal_information').hide();
                })
            });
            // 营业执照
            uploadImg('headUploadify_Businesslicense','headimgUploadFile_Businesslicense','headimgErr','bigImgshow_Businesslicense');
            $('#Businesslicense_Sample').mousemove(function () {
                $('#Businesslicense_img').show();
            }).mouseout(function () {
                $('#Businesslicense_img').hide();
            });
            // 代理合同证明
            // 首页
            uploadImg('headUploadify_homepage','headimgUploadFile_homepage','headimgErr','bigImgshow_homepage');
            // 尾页
            uploadImg('headUploadify_Shadowe','headimgUploadFile_Shadowe','headimgErr','bigImgshow_Shadowe');
            // 个人身份证图片
            // 正面
            uploadImg('headUploadify_Positive','headimgUploadFile_Positive','headimgErr','bigImgshow_Positive');
            // 反面
            uploadImg('headUploadify_opposite','headimgUploadFile_opposite','headimgErr','bigImgshow_opposite');
        },
        // 获取参数
        Getparameters: function () {
            /*==========start基本信息必填===========*/
            // app名称
            var app_name=$('#app_name').html();
            // 媒体Logo
            var LogoUrl=$('#headBigImg').attr('src');
            // 常见分类
            var Commonclass=[];
            $('#ification [dictid]').each(function () {
                Commonclass.push({
                    "CategoryId":$(this).attr('dictid')-0,
                    "SortNumber":$(this).attr('mainclass')
                });
            });
            // APP日活
            var Daily=$('#Daily').val();
            // 媒体介绍
            var introduce=$('#introduce textarea').val();
            /*==========end基本信息必填===========*/

            /*==========start基本信息非必填===========*/
            // 覆盖地区
            var Covered=[];
            $('#Covered [provinceid]').each(function () {
                Covered.push({
                    "ProvinceId": $(this).attr('provinceid')-0,
                    "CityId": $(this).attr('cityid')-0
                })
            });
            // 所在地区
            var ProvinceID=$('#locationArea1 option:selected').val();
            var CityID=$('#locationArea2 option:selected').val();
            // 下单备注
            var OrderRemark=[];
            $('.answer span input').each(function () {
                if($(this).prop('checked')==true){
                    OrderRemark.push({
                        "Id":$(this).attr('dictid')-0,
                        "Descript":$(this).attr('dictid')==40009?$('#Other').val():''
                    })
                }

            })
            /*==========end基本信息非必填===========*/
            var obj={
                "BusinessType": 14002,
                "operateType": GetRequest().OperateType?GetRequest().OperateType:1,
                "App": {
                    "MediaId":GetRequest().MediaId,
                    "BaseMediaId":GetRequest().BaseMediaId,
                    "Name": app_name,
                    "HeadIconURL": LogoUrl,
                    "OrderRemark": OrderRemark,
                    "CoverageArea": Covered,
                    "CommonlyClass": Commonclass,
                    "ProvinceId": ProvinceID,
                    "Cityid":CityID,
                    "Remark":introduce,
                    "DailyLive":Daily,
                    "Qualification":{}
                }
            };
            /*========strat相关资质=========*/
            // 媒体主提交相关资质
            if(CTLogin.RoleIDs=="SYS001RL00003") {
                // 媒体关系
                var Mediarelations = $('#Mediarelations .current_ifi').attr('MediaRelations') - 0;
                // 运营类型
                var Operationtype = $('#Operatortype .current_ifi').attr('OperatingType') - 0;
                // 企业名称
                var enterprise = $('#enterprise').val();
                // 营业执照
                var bigImgshow_Businesslicense = $('#bigImgshow_Businesslicense img').attr('src');
                // 代理合同证明
                var bigImgshow_homepage = $('#bigImgshow_homepage img').attr('src');//首页
                var bigImgshow_Shadowe = $('#bigImgshow_Shadowe img').attr('src');//尾页
                // 真实姓名
                var Realname = $('#Realname').val();
                // 身份证
                var bigImgshow_Positive = $('#bigImgshow_Positive img').attr('src');//正面

                // 判断相关资质对应传的值
                // 代理
                if (Mediarelations == 50001) {
                    // 企业名称
                    obj.App.Qualification.EnterpriseName = enterprise;
                    // 营业执照
                    obj.App.Qualification.BusinessLicense = bigImgshow_Businesslicense;
                    // 代理合同证明
                    obj.App.Qualification.AgentContractFrontURL = bigImgshow_homepage;
                    obj.App.Qualification.AgentContractBackURL = bigImgshow_Shadowe;
                }
                ;
                // 自有
                if (Mediarelations == 50002) {
                    // 企业
                    if (Operationtype == 1001) {
                        // 企业名称
                        obj.App.Qualification.EnterpriseName = enterprise;
                        // 营业执照
                        obj.App.Qualification.BusinessLicense = bigImgshow_Businesslicense;
                    }
                    // 个人
                    if (Operationtype == 1002) {
                        // 真实姓名
                        obj.App.Qualification.EnterpriseName=Realname;
                        // 身份证图片
                        obj.App.Qualification.IDCardFrontURL = bigImgshow_Positive;
                    }
                }
                ;
                // 媒体关系
                obj.App.Qualification.MediaRelations = Mediarelations;
                // 运营类型
                obj.App.Qualification.OperatingType = Operationtype;
            }
            /*========end相关资质=========*/
            return obj;
        },
        // 判断是否填写的参数（基本信息）
        referto: function () {
            // 初始不显示
            $('#medialogo_Prompt').hide();
            $('#common_Prompt').hide();
            $('#Daily-Prompt').hide();
            $('#introduce_Prompt').hide();
            var i=0;
            // 判断是否上传媒体logo
            if($('#headBigImg').attr('src')==''){
                $('#medialogo_Prompt').show();
                i++;
            }
            // 判断是否选择常见分类
            if($('#ification [dictid]').length<=0){
                $('#common_Prompt').show();
                i++;
            }
            // 判断添加app日活
            if($('#Daily').val()==''){
                $('#Daily-Prompt').show();
                i++;
            }
            // 判断填写媒体介绍
            if($('#introduce textarea').val()==''){
                $('#introduce_Prompt').show();
                i++;
            }
            // 判断下单备注
            if($('.answer span:last input').prop('checked')==true&&$('.answer textarea').val()==''){
                layer.msg('请输入下单备注!',{time:1000});
                i++;
            }
            if(i>0){
                return false;
            }else {
                return true;
            }
        },
        // 判断是否填写的参数（相关资质）
        intelligence: function () {
            // 初始不显示
            $('#enterprise_Prompt').hide();
            $('#Businesslicense_Prompt').hide();
            $('#Certificate_Prompt').hide();
            $('#enterprise_Prompt').hide();
            $('#Businesslicense_Prompt').hide();
            $('#Realname_Prompt').hide();
            $('#idcardpicture_Prompt').hide();
            // 媒体关系
            var Mediarelations=$('#Mediarelations .current_ifi').attr('MediaRelations')-0;
            // 运营类型
            var Operationtype=$('#Operatortype .current_ifi').attr('OperatingType')-0;
            // 标识
            var i=0;
            // 代理
            if(Mediarelations==50001){
                // 判断企业名称
                if($('#enterprise').val()==''){
                    $('#enterprise_Prompt').show();
                    i++;
                }
                // 判断营业执照
                if($('#bigImgshow_Businesslicense img').attr('src')==''){

                    $('#Businesslicense_Prompt').show();
                    i++;
                }
                // 判断代理合同证明
                if($('#bigImgshow_homepage img').attr('src')==''||$('#bigImgshow_Shadowe img').attr('src')==''){
                    $('#Certificate_Prompt').show();
                    i++;
                }
            }
            // 自有
            if(Mediarelations==50002){
                // 企业
                if(Operationtype==1001){
                    // 判断企业名称
                    if($('#enterprise').val()==''){
                        $('#enterprise_Prompt').show();
                        i++;
                    }
                    // 判断营业执照
                    if($('#bigImgshow_Businesslicense img').attr('src')==''){
                        $('#Businesslicense_Prompt').show();
                        i++;
                    }
                }
                // 个人
                if(Operationtype==1002){
                    // 真实姓名
                    if($('#Realname').val()==''){
                        $('#Realname_Prompt').show();
                        i++;
                    }
                    // 身份证图片
                    if($('#bigImgshow_Positive img').attr('src')==''){
                        $('#idcardpicture_Prompt').show();
                        i++;
                    }
                }
            }

            if(i>0){
                return false;
            }else {
                return true;
            }
        },
        //点击提交
        Submit: function () {
            var _this=this;
            $('#Submit').off('click').on('click',function () {
                // 标识
                var i=0;
                // 判断基本信息填写
                if(!_this.referto()){
                    i++;
                };
                // 判断相关资质填写
                // 媒体主
                if(CTLogin.RoleIDs=="SYS001RL00003") {
                    if (!_this.intelligence()) {
                        i++;
                    }
                }
                if(i>0){
                    return false;
                };

                // 添加媒体
                setAjax({
                    url:'/api/media/curd?v=1_1',
                    type:'POST',
                    data:_this.Getparameters()
                },function (data) {
                    if(data.Status==0){
                        layer.msg('提交成功',{time:1000},function () {
                            window.location='/mediamanager/mediaAPP.html';
                        })
                    }else {
                        layer.msg(data.Message);
                    }
                })
            })
        },
        // 点击提交并添加广告
        ConfirmAdd: function () {
            var _this=this;
            $('#ConfirmAdd').off('click').on('click',function () {
                // 标识
                var i=0;
                // 判断基本信息填写
                if(!_this.referto()){
                    i++;
                };
                // 判断相关资质填写
                // 媒体主
                if(CTLogin.RoleIDs=="SYS001RL00003") {
                    if (!_this.intelligence()) {
                        i++;
                    }
                }
                if(i>0){
                    return false;
                };
                // 添加媒体
                setAjax({
                    url:'/api/media/curd?v=1_1',
                    type:'POST',
                    data:_this.Getparameters()
                },function (data) {
                    if(data.Status==0){
                        layer.msg('提交成功',{time:1000},function () {
                            setAjax({
                                url: '/api/media/GetInfo?v=1_1',
                                type: 'get',
                                data: {
                                    businesstype: 14002,
                                    MediaId: GetRequest().MediaId,
                                    baseMediaId: GetRequest().BaseMediaId
                                }
                            }, function (data) {
                                // data.Result = {
                                //     "CoverageArea": [
                                //         {
                                //             "ProvinceId": 10,
                                //             "ProvinceName": "河南省",
                                //             "CityId": 1001,
                                //             "CityName": "郑州市"
                                //         },
                                //         {
                                //             "ProvinceId": 11,
                                //             "ProvinceName": "安徽省",
                                //             "CityId": -2,
                                //             "CityName": ""
                                //         }
                                //     ],
                                //     "CommonlyClass": [
                                //         {
                                //             "CategoryId": 22026,
                                //             "CategoryName": "test_categroy",
                                //             "SortNumber":1
                                //         },
                                //         {
                                //             "CategoryId": 22014,
                                //             "CategoryName": "test_categroy_1",
                                //             "SortNumber":0
                                //         }
                                //     ],
                                //     "ProvinceId": 10,
                                //     "ProvinceName": "河南省",
                                //     "CityId": 1001,
                                //     "CityName": "郑州市",
                                //     "DailyLive": 10088,
                                //     "Remark": "ddddddd",
                                //     "OrderRemark": [
                                //         {
                                //             "Id": 40001,
                                //             "Name": "d_name",
                                //             "Descript": "描述"
                                //         },
                                //         {
                                //             "Id": 40003,
                                //             "Name": "d_name",
                                //             "Descript": "描述"
                                //         },
                                //         {
                                //             "Id": 40005,
                                //             "Name": "d_name",
                                //             "Descript": "描述"
                                //         },
                                //         {
                                //             "Id": 40009,
                                //             "Name": "d_name",
                                //             "Descript": "描述"
                                //         }
                                //     ],
                                //     "MediaID": 18,
                                //     "Number": null,
                                //     "Name": "app_test",
                                //     "HeadIconURL": "/sss/image"
                                // };
                                if (data.Result.AdTemplateId > 0) {
                                    window.location = '/PublishManager/addPublishForApp.html?MediaID=' + data.Result.MediaID + '&PubID=0&TemplateID=' + data.Result.AdTemplateId
                                } else {
                                    window.location = '/PublishManager/add_template.html?BaseMediaID=' + data.Result.BaseMediaId?data.Result.BaseMediaId:-2 + '&AppName=' + $.trim($('.mt10 input').val())+'&MediaID=' + data.Result.MediaID
                                }
                            })
                        })
                    }else {
                        layer.msg(data.Message);
                    }
                })
            })
        }
    }

    var addmediaapp = new addMediaApp();
})