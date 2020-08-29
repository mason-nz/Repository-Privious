/*
* Written by:     fengbo
* function:       认证信息
* Created Date:   2018-03-05
* Modified Date:   2018-03-05
*/

$(function () {
    var variable_sfz = '';
    var variable_yyzz = '';
    var app = new Vue({
        el: '#app',
        data: {
            //顶部提示
            header_hide: 'header_hide',
            header_text: '信息审核中，不可修改',
            header_click: 1,
            showTitle: false,
            //类型颜色
            typecolourblue: 'blue',
            typecolour: '',
            // 个人系列
            personal: true,
            // 真实姓名
            RealName: '',
            oldRealName: '',
            // 身份证号
            IdNumber: '',
            oldIdNumber: '',
            photoimg: '',
            preUploadPhotoimg: '',
            isUploadStateSfz: false,//是否点了上传图片
            isUploadStateYyzz: false,//是否点了上传图片
            idCardPhotoimg: '',
            sfz: '请上传',
            // 企业系列
            enterprise: false,
            typeInfo: '',
            // 公司名称
            CorporateName: '',
            oldCorporateName: '',
            // 营业执照
            BLicenceURL: '',
            preUploadBLicenceURL: '',
            yyzz: '请上传',
            //是否禁止输入
            IsbaseOnInput: false,
            //按钮是否点击
            IsClickBtn: false,
            isUploadYyzz: false,//营业执照上传
            isUploadSfz: true,//身份证
            //监测输入框是否已经填写
            isRealName: false,
            isIdNumber: false,
            isphotoimg: false,
            isCorporateName: false,
            isBLicenceURL: false,
            rejectStatus: '',//驳回状态
            alreadyUploadYyzz: false,
            alreadyUploadSfz: false
        },
        created:function() {
            var _this = this;
            _this.useriformation();
            _this.keyupDocument();
            //_this.IDCardUploading();
            _this.ClickBLicenceURL();
        },
        methods: {
            //点击上传图片弹框——关闭
            toClickPopClose:function(){
                $('#js_upload_sfz_wrap,#js_upload_yyzz_wrap').hide();
            },
            // 键盘按下
            keyupDocument:function(){
                var _this = this;
                // console.log(_this.isRealName, _this.isIdNumber, _this.isUploadSfz);
                if(_this.typecolourblue == 'blue'){
                    if(_this.rejectStatus == 3){//被拒，修改一个就可以保存
                        if(_this.RealName != '' && _this.IdNumber != '' && $('#photoimg').attr('src') != ''){
                            if(_this.isRealName || _this.isIdNumber || _this.isUploadSfz){
                                $('#PreserVation').attr('class','btn_bg');
                            } else {
                                $('#PreserVation').attr('class','Thegrey');
                            }
                        } else {
                            $('#PreserVation').attr('class','Thegrey');
                        }
                    } else {
                        if(_this.isRealName && _this.isIdNumber && _this.isUploadSfz){
                            $('#PreserVation').attr('class','btn_bg');
                        }else{
                            $('#PreserVation').attr('class','Thegrey');
                        }
                    }
                } else {
                    // console.log(_this.isCorporateName,_this.isUploadYyzz)
                    if(_this.rejectStatus == 3){//被拒，修改一个就可以保存
                        if(_this.CorporateName != '' && $('#BLicenceURL').attr('src') != ''){
                            if(_this.isCorporateName || _this.isUploadYyzz){
                                $('#PreserVation').attr('class','btn_bg');
                            } else {
                                $('#PreserVation').attr('class','Thegrey');
                            }
                        } else {
                            $('#PreserVation').attr('class','Thegrey');
                        }
                    } else  {
                        if(_this.isCorporateName && _this.isUploadYyzz){
                            $('#PreserVation').attr('class','btn_bg');
                        }else{
                            $('#PreserVation').attr('class','Thegrey');
                        }
                    }
                }
            },
            //获取认证基本信息
            useriformation:function(){
                var _this = this;
                $.ajax({
                    //url: 'json/person.json',
                    url: public_url + '/api/UserManage/QueryUserDetailInfo',
                    type: 'get',
                    dataType: 'json',
                    data: {},
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    success: function (data) {
                        if (data.Status == 0) {
                            var Result = data.Result;
                            //未认证  填写信息
                            if (Result == null || Result.Status == 0) {//0：未认证 1：待审核 2：已认证 3：认证未通过
                                _this.showTitle = false;
                            } else {
                                _this.rejectStatus = Result.Status;//认证状态
                                _this.showTitle = true;
                                _this.IsbaseOnInput = false;

                                //个人
                                if(Result.Type == 1002){
                                    _this.typecolourblue = 'blue';
                                    _this.typecolour = '';
                                    _this.personal = true;
                                    _this.enterprise = false;

                                    _this.RealName = Result.TrueName;
                                    _this.oldRealName = Result.TrueName;
                                    _this.IdNumber = Result.IdentityNo;
                                    _this.oldIdNumber = Result.IdentityNo;
                                    _this.idCardPhotoimg = Result.IDCardFrontURL;
                                    //_this.IDCardUploading();
                                    if(_this.idCardPhotoimg != ''){
                                        $('#imgUploadFile').attr('src', _this.idCardPhotoimg)
                                    }
                                }else if(Result.Type == 1001){//企业
                                    _this.typecolourblue = '';
                                    _this.typecolour = 'blue';
                                    _this.personal = false;
                                    _this.enterprise = true;

                                    _this.CorporateName = Result.TrueName;
                                    _this.oldCorporateName = Result.TrueName;
                                    _this.BLicenceURL = Result.BLicenceURL;
                                    _this.ClickBLicenceURL();
                                    if(_this.BLicenceURL != ''){
                                        $('#imgBLicenceURL').attr('src', _this.BLicenceURL)
                                    }
                                }

                                //待审核  渲染信息 不可修改
                                //已认证  渲染信息 不可修改
                                if(Result.Status == 1 || Result.Status == 2){
                                    _this.IsbaseOnInput = true;
                                    if(Result.Status == 2){
                                        _this.header_text = Result.Reason;
                                        _this.showTitle = false;
                                    }
                                }else if(Result.Status == 3){//认证未通过 渲染信息 可以修改
                                    _this.IsbaseOnInput = false;
                                    _this.header_text = Result.Reason;
                                }
                            }
                        }else{//提示信息
                            layer.open({
                                content: data.Message,
                                skin: 'msg',
                                time: 2 //2秒后自动关闭
                            });
                        }
                    }
                })
            },
            // 点击个人 个人和企业按钮
            personalClick:function() {
                var _this = this;
                var colour = _this.typecolourblue;
                _this.typecolourblue = _this.typecolour;
                _this.typecolour = colour;

                _this.personal = true;
                _this.enterprise = false;
                //_this.IDCardUploading();
                _this.keyupDocument();
                if(this.personal){
                    $('#PreserVation').attr('data-type', 'sfz');
                }
            },
            // 点击企业 个人和企业按钮
            enterpriseClick:function() {
                var _this = this;
                var colour = this.typecolour;
                this.typecolour = this.typecolourblue;
                this.typecolourblue = colour;

                this.personal = false;
                this.enterprise = true;
                _this.ClickBLicenceURL();
                _this.keyupDocument();
                if(this.enterprise){
                    $('#PreserVation').attr('data-type', 'yyzz');
                }
            },
            // 判断身份证合法
            checkIdentity:function(identity) {
                var reg = /^[1-9]{1}[0-9]{14}$|^[1-9]{1}[0-9]{16}([0-9]|[xX])$/;
                if (reg.test(identity)) {
                    return true;
                } else {
                    return false;
                }
            },
            //点击上传照片
            toUploadPicSfz:function(){
                var _this = this;
                if(_this.rejectStatus == '' || _this.rejectStatus == 3){//没有照片或者被拒
                    $('#js_upload_sfz_wrap').show();
                }
            },
            //点击上传照片
            toUploadPicYyzz:function(){
                var _this = this;
                if(_this.rejectStatus == '' || _this.rejectStatus == 3){//没有照片或者被拒
                    $('#js_upload_yyzz_wrap').show();
                }
            },
            // 点击手持身份证
            IDCardUploading:function() {
                var _this = this;
                if(this.personal){
                    $('#PreserVation').attr('data-type', 'sfz');
                } else {
                    $('#PreserVation').attr('data-type', 'yyzz');
                }
                $(document).ready(function () {
                    // if(_this.idCardPhotoimg == '' || _this.rejectStatus == 3){//身份证为空或者被驳回
                        jQuery(function () {
                            var $ = jQuery,
                                $list = $('#thelist'),
                                $btn = $('.ctlBtn'),
                                state = 'pending',
                                uploader1,
                                imgSrcDisplay = '',
                                imgSrcId;
                            if(_this.idCardPhotoimg != '' || _this.preUploadPhotoimg != ''){
                                _this.sfz = '';
                                imgSrcDisplay = 'inline-block';
                                imgSrcId = _this.idCardPhotoimg;
                                if(_this.preUploadPhotoimg != ''){
                                    imgSrcId = _this.preUploadPhotoimg;
                                }
                                var imgWrap = '<span id="sfz_sc">'+_this.sfz+'</span><img id="photoimg" style="display: '+imgSrcDisplay+' ;" src="'+imgSrcId+'" alt="">';
                            } else {
                                _this.sfz = '请上传';
                                imgSrcDisplay = 'none';
                                imgSrcId = _this.idCardPhotoimg;
                                var imgWrap = '<span id="sfz_sc">'+_this.sfz+'</span><img id="photoimg" style="display: '+imgSrcDisplay+' ;" src="'+imgSrcId+'" alt="">';
                            }

                            uploader1 = WebUploader.create({
                                //auto:true, //[默认值：false] 设置为 true 后，不需要手动调用上传，有文件选择即开始上传。
                                //runtimeOrder: 'flash',//指定运行时启动顺序。默认会想尝试 html5 是否支持，如果支持则使用 html5, 否则则使用 flash
                                // 不压缩image
                                resize: false,
                                // swf文件路径
                                swf: '/js/uploader.swf',
                                // 文件接收服务端。
                                server: '/AjaxServers/UploadFile.ashx',
                                // 选择文件的按钮。可选。
                                // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                                pick: {
                                    id: '#picker',
                                    multiple: false,
                                    innerHTML: imgWrap
                                },
                                formData: { Action: "BatchImport", CarType: "", LoginCookiesContent: escapeStr(getCookie("ct-wxinfo")), IsGenSmallImage: 1 },
                                fileVal: 'Filedata',
                                // fileNumLimit: 1,
                                duplicate :true,
                                accept: {
                                    title: 'Images',
                                    extensions: 'jpg,jpeg,bmp,png',
                                    mimeTypes: 'image/*'
                                },
                                thumb: {
                                    // 图片质量，只有type为`image/jpeg`的时候才有效。
                                    quality: 100,
                                    // 是否允许放大，如果想要生成小图的时候不失真，此选项应该设置为false.
                                    allowMagnify: false,
                                    // 为空的话则保留原有图片格式。否则强制转换成指定的类型。
                                    type: ''
                                }
                            });

                            uploader1.addButton({
                                id: '#js_upload_sfz_btn',
                                multiple: false,
                                innerHTML: '立即上传'
                            });
                            var  ratio = window.devicePixelRatio || 1;
                            // 缩略图大小
                            var thumbnailWidth = 110 * ratio;
                            var thumbnailHeight = 110 * ratio;

                            // 当有文件添加进来的时候
                            uploader1.on('fileQueued', function (file) {

                                variable_sfz = '';
                                // 预览
                                uploader1.makeThumb( file, function(error,ret) {
                                    // console.log(ret);
                                    if (error) {
                                        layer.open({
                                            content: '不能预览'
                                            , skin: 'msg'
                                            , time: 2 //2秒后自动关闭
                                        });
                                    } else {
                                        _this.preUploadPhotoimg = ret;
                                        $('#sfz_sc').text('');
                                        $('#photoimg').show().attr('src',ret);
                                        // $('.webuploader-pick').css('z-index','2');
                                        $('#js_upload_sfz_wrap').hide();
                                        _this.isUploadSfz = true;
                                        _this.isUploadStateSfz = true;
                                        $('#photoimg').on('click',function () {
                                            $('#js_preview_image').attr('src', ret);
                                            $('#js_preview_image_wrap').show();
                                            event.stopPropagation();
                                        })
                                        $('#js_preview_image_wrap').on('click', function(){
                                            $(this).hide();
                                        })
                                    }
                                },thumbnailWidth, thumbnailHeight);
                            });

                            // 文件上传过程中创建进度条实时显示。
                            uploader1.on('uploadProgress', function (file, percentage) {
                                var $li = $('#' + file.id),
                                    $percent = $li.find('.progress .progress-bar');

                                // 避免重复创建
                                if (!$percent.length) {
                                    $percent = $('<div class="progress progress-striped active">' +
                                        '<div class="progress-bar" role="progressbar" style="width: 0%">' +
                                        '</div>' +
                                        '</div>').appendTo($li).find('.progress-bar');
                                }

                                $li.find('p.state').text('上传中');
                                $percent.css('width', percentage * 100 + '%');
                            });

                            uploader1.on('uploadSuccess', function (file, json) {
                                var array = json.Msg.split("|");
                                for (var i = 0; i < array.length; i++) {
                                    if (i > 0) {
                                        $("#imgUploadFile").after($("<img>").attr("src", "" + array[i]));
                                    }
                                    else {
                                        $("#imgUploadFile").attr("src", "" + array[i]);
                                    }
                                }
                                if(json.Status==1){
                                    // alert(json.Status+"+json.Statusjson.Statusjson.Status")
                                    variable_sfz=array[array.length-1];
                                    _this.alreadyUploadSfz = true;
                                    _this.Preservation();
                                }else {
                                    layer.open({
                                        content: array[array.length-1]
                                        , skin: 'msg'
                                        , time: 2 //2秒后自动关闭
                                    });
                                    variable_sfz=0
                                }
                            });

                            uploader1.on('uploadError', function (file) {
                                // $('#' + file.id).find('p.state').text('上传出错');
                                layer.open({
                                    content: '上传出错'
                                    , skin: 'msg'
                                    , time: 2 //2秒后自动关闭
                                });
                                variable_sfz=1;
                            });

                            uploader1.on('uploadComplete', function (file) {
                                $('#' + file.id).find('.progress').fadeOut();
                            });

                            uploader1.on('all', function (type) {
                                if (type === 'startUpload') {
                                    state = 'uploading';
                                } else if (type === 'stopUpload') {
                                    state = 'paused';
                                } else if (type === 'uploadFinished') {
                                    state = 'done';
                                }
                                if (state === 'uploading') {
                                    // $btn.text('暂停上传');
                                } else {
                                    // $btn.text('开始上传');
                                }
                            });

                            console.log($('[data-type=sfz]').text())
                            $('[data-type=sfz]').off('click').on('click', function () {
                                if($(this).attr('class') == "Thegrey"){
                                    return false;
                                }
                                if(_this.isUploadStateSfz){
                                    console.log('上传图片');
                                    if (state === 'uploading') {
                                        // uploader.stop();
                                    } else {
                                        if(_this.alreadyUploadSfz){
                                            _this.Preservation();
                                            return false;
                                        } else {
                                            uploader1.upload();
                                        }
                                    }
                                    if(variable_sfz == 1){
                                        layer.open({
                                            content: '上传出错'
                                            , skin: 'msg'
                                            , time: 2 //2秒后自动关闭
                                        });
                                    }
                                } else {
                                    console.log('点击保存身份证');
                                    _this.Preservation();
                                    return false;
                                }
                            });
                            $('#sfz_sc').on('click', function(){
                                $('#js_upload_sfz_wrap').show();
                            })
                            $('#photoimg').on('click',function () {
                                var src = $(this).attr('src');
                                if(src != ''){
                                    $('#js_preview_image').attr('src', src);
                                    $('#js_preview_image_wrap').show();
                                    event.stopPropagation();
                                }
                            })
                            $('#js_preview_image_wrap').on('click', function(){
                                $(this).hide();
                            })

                        });

                    // }
                });
            },
            // 点击营业执照
            ClickBLicenceURL:function(){
                var _this = this;
                if(this.enterprise){
                    $('#PreserVation').attr('data-type', 'yyzz');
                } else {
                    $('#PreserVation').attr('data-type', 'sfz');
                }
                $(document).ready(function () {
                    jQuery(function () {
                        var $ = jQuery,
                            $list = $('#thelist'),
                            state = 'pending',
                            uploader2,
                            imgSrcDisplay = '',
                            imgSrcId = '';
                        if(_this.BLicenceURL != '' || _this.preUploadBLicenceURL != ''){
                            _this.yyzz_sc = '';
                            imgSrcDisplay = 'inline-block';
                            imgSrcId = _this.BLicenceURL;
                            if(_this.preUploadBLicenceURL != ''){
                                imgSrcId = _this.preUploadBLicenceURL;
                            }
                            var imgWrap = '<span id="yyzz_sc">'+_this.yyzz_sc+'</span><img style="display: '+imgSrcDisplay+';" id="BLicenceURL" src="'+imgSrcId+'" alt="">';
                        } else {
                            _this.yyzz_sc = '请上传';
                            imgSrcDisplay = 'none';
                            imgSrcId = _this.BLicenceURL;
                            var imgWrap = '<span id="yyzz_sc">'+_this.yyzz_sc+'</span><img style="display: '+imgSrcDisplay+';" id="BLicenceURL" src="'+imgSrcId+'" alt="">';
                        }
                        uploader2 = WebUploader.create({
                            //auto:true, //[默认值：false] 设置为 true 后，不需要手动调用上传，有文件选择即开始上传。
                            //runtimeOrder: 'flash',//指定运行时启动顺序。默认会想尝试 html5 是否支持，如果支持则使用 html5, 否则则使用 flash
                            // 不压缩image
                            resize: false,
                            // swf文件路径
                            swf: '/js/uploader.swf',
                            // 文件接收服务端。
                            server: '/AjaxServers/UploadFile.ashx',
                            // 选择文件的按钮。可选。
                            // 内部根据当前运行是创建，可能是input元素，也可能是flash.
                            pick: {
                                id: '#js_licence_picker',
                                multiple: false,
                                innerHTML: imgWrap
                            },
                            formData: { Action: "BatchImport", CarType: "", LoginCookiesContent: escapeStr(getCookie("ct-wxinfo")), IsGenSmallImage: 1 },
                            fileVal: 'Filedata',
                            // fileNumLimit: 1,
                            duplicate :true,
                            accept: {
                                title: 'Images',
                                extensions: 'jpg,jpeg,bmp,png',
                                mimeTypes: 'image/*'
                            },
                            thumb: {
                                // 图片质量，只有type为`image/jpeg`的时候才有效。
                                quality: 100,
                                // 是否允许放大，如果想要生成小图的时候不失真，此选项应该设置为false.
                                allowMagnify: false,
                                // 为空的话则保留原有图片格式。否则强制转换成指定的类型。
                                type: ''
                            }
                        });
                        uploader2.addButton({
                            id: '#js_upload_yyzz_btn',
                            multiple: false,
                            innerHTML: '立即上传'
                        });

                        var  ratio = window.devicePixelRatio || 1;
                        // 缩略图大小
                        var thumbnailWidth = 110 * ratio;
                        var thumbnailHeight = 110 * ratio;

                        // 当有文件添加进来的时候
                        uploader2.on('fileQueued', function (file) {
                            variable_sfz = '';
                            // 预览
                            uploader2.makeThumb( file, function(error,ret) {
                                if (error) {
                                    layer.open({
                                        content: '不能预览'
                                        , skin: 'msg'
                                        , time: 2 //2秒后自动关闭
                                    });
                                } else {
                                    _this.preUploadBLicenceURL = ret;
                                    $('#yyzz_sc').text('');
                                    $('#BLicenceURL').show().attr('src', ret);
                                    // $('.webuploader-pick').css('z-index','2');
                                    $('#js_upload_yyzz_wrap').hide();
                                    _this.isUploadYyzz = true;
                                    _this.isUploadStateYyzz = true;
                                    $('#BLicenceURL').on('click',function () {
                                        $('#js_preview_image').attr('src', ret);
                                        $('#js_preview_image_wrap').show();
                                        event.stopPropagation();
                                    })
                                    $('#js_preview_image_wrap').on('click', function(){
                                        $(this).hide();
                                    })
                                }
                            },thumbnailWidth, thumbnailHeight);
                        });

                        // 文件上传过程中创建进度条实时显示。
                        uploader2.on('uploadProgress', function (file, percentage) {
                            var $li = $('#' + file.id),
                                $percent = $li.find('.progress .progress-bar');

                            // 避免重复创建
                            if (!$percent.length) {
                                $percent = $('<div class="progress progress-striped active">' +
                                    '<div class="progress-bar" role="progressbar" style="width: 0%">' +
                                    '</div>' +
                                    '</div>').appendTo($li).find('.progress-bar');
                            }

                            $li.find('p.state').text('上传中');
                            $percent.css('width', percentage * 100 + '%');
                        });

                        uploader2.on('uploadSuccess', function (file, json) {
                            // alert(json+'+json');
                            var array = json.Msg.split("|");
                            for (var i = 0; i < array.length; i++) {
                                if (i > 0) {
                                    $("#imgBLicenceURL").after($("<img>").attr("src", "" + array[i]));
                                }
                                else {
                                    $("#imgBLicenceURL").attr("src", "" + array[i]);
                                }
                            }
                            if(json.Status==1){//图片上传完成
                                variable_sfz=array[array.length-1];
                                _this.alreadyUploadYyzz = true;
                                _this.Preservation();
                            }else {
                                layer.open({
                                    content: array[array.length-1]
                                    , skin: 'msg'
                                    , time: 2 //2秒后自动关闭
                                });
                                variable_sfz=0
                            }
                        });

                        uploader2.on('uploadError', function (file) {
                            // $('#' + file.id).find('p.state').text('上传出错');
                            layer.open({
                                content: '上传出错'
                                , skin: 'msg'
                                , time: 2 //2秒后自动关闭
                            });
                            variable_sfz=1;
                        });

                        uploader2.on('uploadComplete', function (file) {
                            $('#' + file.id).find('.progress').fadeOut();
                        });

                        uploader2.on('all', function (type) {
                            if (type === 'startUpload') {
                                state = 'uploading';
                            } else if (type === 'stopUpload') {
                                state = 'paused';
                            } else if (type === 'uploadFinished') {
                                state = 'done';
                            }
                            if (state === 'uploading') {
                                // $btn.text('暂停上传');
                            } else {
                                // $btn.text('开始上传');
                            }
                        });
                        console.log($('[data-type=yyzz]').text())
                        $('[data-type=yyzz]').off('click').on('click', function () {
                            console.log(222)
                            if($(this).attr('class') == "Thegrey"){
                                return false;
                            }
                            if(_this.isUploadStateYyzz){
                                console.log('企业上传图片');
                                if (state === 'uploading') {
                                    // uploader.stop();
                                } else {
                                    if(_this.alreadyUploadYyzz){
                                        _this.Preservation();
                                        return false;
                                    } else {
                                        uploader2.upload();
                                    }
                                }
                                if(variable_sfz == 1){
                                    layer.open({
                                        content: '上传出错'
                                        , skin: 'msg'
                                        , time: 2 //2秒后自动关闭
                                    });
                                }
                            } else {
                                console.log('企业点击保存');
                                _this.Preservation();
                                return false;
                            }
                        })
                        $('#yyzz_sc').on('click', function(){
                            $('#js_upload_yyzz_wrap').show();
                        })
                        $('#BLicenceURL').on('click',function () {
                            var src = $(this).attr('src');
                            if(src != ''){
                                $('#js_preview_image').attr('src', src);
                                $('#js_preview_image_wrap').show();
                                event.stopPropagation();
                            }
                        })
                        $('#js_preview_image_wrap').on('click', function(){
                            $(this).hide();
                        })
                    })
                });
            },
            toSaveAjax:function(obj){
                $.ajax({
                    url:public_url+'/api/UserManage/SaveUserDetail',
                    type:'post',
                    dataType:'json',
                    data:obj,
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    success:function (data) {
                        if (data.Status==0){
                            layer.closeAll()
                            layer.open({
                                content: "提交成功！"
                                , skin: 'msg'
                                , time: 2 //2秒后自动关闭
                                ,success: function(elem){
                                    window.location=public_pre+"/userManager/userInfo.html"
                                }
                            });
                        }else {
                            layer.closeAll()
                            layer.open({
                                content: data.Message
                                , skin: 'msg'
                                , time: 2 //2秒后自动关闭
                            });
                        }
                    }
                })
            },
            // 保存
            Preservation:function() {
                // 置灰不点击
                if($('#PreserVation').attr('class') == "Thegrey"){
                    return false;
                }

                var obj = {};
                var _this = this;
                // 个人
                if (this.personal && !this.enterprise) {
                    // 真实姓名
                    if (!this.RealName) {
                        layer.open({
                            content: '真实姓名不能为空'
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                        });
                        return false
                    }
                    // 身份证号
                    if (!this.IdNumber) {
                        layer.open({
                            content: '身份证号不能为空'
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                        });
                        return false
                    }
                    if (!this.checkIdentity(this.IdNumber)) {
                        layer.open({
                            content: '请输入正确的身份证号码'
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                        });
                        return false
                    }
                    // 手持身份证
                    if (!this.IdNumber) {
                        layer.open({
                            content: '手持身份证不能为空'
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                        });
                        return false
                    }
                    // //身份证照片
                    // if($('#imgUploadFile').attr('src') != ''){
                    //     _this.isUploadSfz = true;
                    // }
                    // if (!_this.isUploadSfz) {
                    //     layer.open({
                    //         content: '请上传身份证照片'
                    //         , skin: 'msg'
                    //         , time: 2 //2秒后自动关闭
                    //     });
                    //     return false
                    // }
                } else {//企业
                    // 企业名称
                    if (!this.CorporateName) {
                        layer.open({
                            content: '企业名称不能为空'
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                        });
                        return false
                    }
                    // 营业执照
                    if($('#imgBLicenceURL').attr('src') != ''){
                        _this.isUploadYyzz = true;
                    }
                    if (!_this.isUploadYyzz) {
                        layer.open({
                            content: '营业执照不能为空'
                            , skin: 'msg'
                            , time: 2 //2秒后自动关闭
                        });
                        return false
                    }
                }

                if(this.personal && !this.enterprise){
                    //个人
                    obj = {
                        Type: 1002,
                        TrueName: _this.RealName,//个人姓名
                        IdentityNo: _this.IdNumber//身份证号码
                        //IDCardFrontURL: $('#imgUploadFile').attr('src')//身份证正面URL
                    };
                    // if($('#imgUploadFile').attr('src') != ''){
                        layer.open({
                            content: '是否保存？<br />保存后审核通过不可修改'
                            ,btn: ['确认保存', '取消'],
                            yes: function(index){//点确定按钮触发的回调函数，返回一个参数为当前层的索引
                                layer.close(index);
                                layer.open({
                                    type: 2,
                                    shadeClose:false
                                });
                                // 保存接口
                                _this.toSaveAjax(obj)
                            },no:function(index){                   //点取消按钮触发的回调函数
                                layer.close(index)
                            }
                        })
                    // }
                }else {
                    //企业
                    obj = {
                        Type: 1001,
                        TrueName: _this.CorporateName,//公司名称
                        BLicenceURL:$('#imgBLicenceURL').attr('src')//营业执照
                    }
                    if($('#imgBLicenceURL').attr('src') != ''){
                        layer.open({
                            content: '是否保存？<br />保存后审核通过不可修改'
                            ,btn: ['确认保存', '取消'],
                            yes: function(index){//点确定按钮触发的回调函数，返回一个参数为当前层的索引
                                layer.close(index);
                                layer.open({
                                    type: 2,
                                    shadeClose:false
                                });
                                // 保存接口
                                _this.toSaveAjax(obj)
                            },no:function(index){                   //点取消按钮触发的回调函数
                                layer.close(index)
                            }
                        })
                    }
                }
            }
        },
        watch: {
            RealName:function(newVal) {
                var _this = this;
                if(newVal != _this.oldRealName){
                    this.isRealName = true;
                }else{
                    this.isRealName = false;
                }
                if(newVal == ''){
                    this.isRealName = false;
                }
                _this.keyupDocument()
            },
            IdNumber:function(newVal){
                var _this = this;
                if(newVal != _this.oldIdNumber){
                    this.isIdNumber = true;
                }else{
                    this.isIdNumber = false;
                }
                if(newVal == ''){
                    this.isIdNumber = false;
                }
                _this.keyupDocument()
            },
            photoimg:function(newVal){
                if(newVal != ''){
                    this.isphotoimg = true;
                }else{
                    this.isphotoimg = false;
                }
                if(newVal == ''){
                    this.isphotoimg = false;
                }
                _this.keyupDocument()
            },
            CorporateName:function(newVal){
                var _this = this;
                if(newVal != _this.oldCorporateName){
                    this.isCorporateName = true;
                }else{
                    this.isCorporateName = false;
                }
                if(newVal == ''){
                    this.isCorporateName = false;
                }
                _this.keyupDocument()
            },
            BLicenceURL:function(newVal){
                if(newVal != ''){
                    this.isBLicenceURL = true;
                }else{
                    this.isBLicenceURL = false;
                }
                if(newVal == ''){
                    this.isBLicenceURL = false;
                }
                this.keyupDocument()
            },
            isUploadSfz:function(newVal){
                if(newVal){
                    this.keyupDocument()
                }
            },
            isUploadYyzz:function(newVal){
                if(newVal){
                    this.keyupDocument()
                }
            }
        }
    })


    /**
     * @desc  JQuery扩展，将json字符串转换为对象，需要引用类库JQuery
     * @param   json字符串
     * @return 返回object,array,string等对象
     * @Add=Masj, Date: 2009-12-07
     */
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

})
