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
                    $('#'+img).attr('src', smallImgUrl);
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
var objData;
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
    //         }
    //     ],
    //     "CommonlyClass": [
    //         {
    //             "CategoryId": 111,
    //             "CategoryName": "test_categroy",
    //             "SortNumber":1
    //         },
    //         {
    //             "CategoryId": 1121,
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
    //             "Id": 10,
    //             "Name": "d_name",
    //             "Descript": "描述"
    //         }
    //     ],
    //     "MediaID": 18,
    //     "Number": null,
    //     "Name": "app_test",
    //     "HeadIconURL": "/sss/image"
    // };
    objData=data;
    if(data.Result?data.Result:null){
        $('#essentia').html(ejs.render($('#essentialinformation').html(), data));
    }
})
$(function () {
    var i=0;
    if(GetRequest().OperateType==1){
        if(GetRequest().appName==undefined){
            document.body.innerHTML='';
            layer.msg('媒体名称不能为空',{time:1300},function () {
                window.location='/mediamanager/mediaapp.html';
                i=1;
            });
        };
    }
    if(GetRequest().MediaId==undefined){
        document.body.innerHTML='';
        layer.msg('媒体id不能为空',{time:1300},function () {
            window.location='/mediamanager/mediaapp.html';
            i=1;
        })
    };

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
        // 相关资质
        this.relevantattribute();
        // 提交
        this.Submit();
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
        // ajax请求
        myAjax: setAjax,
        /*============公共方法end============*/
        // 相关资质
        relevantattribute: function () {
            // 媒体关系
            // 点击代理
            $('#Mediarelations_agent').off('click').on('click', function () {
                // 移除个人的点击事件
                $('#Operatortype_personal').unbind('click');
                // 显示代理合同
                $('#Certificate').show();
                // 代理有样式 自有没样式
                $(this).find('div').attr('class', 'current_ifi');
                $('#Mediarelations_have').find('div').attr('class', 'default_ifi');

                // 运营者类型
                // 个人
                $('#Operatortype_personal').find('div').attr('class', 'default_ifi').css('color', '#aaa');
                $('#personal_information').hide();
                // 企业
                $('#Operatortype_enterprise').find('div').attr('class', 'current_ifi');
                $('#enterprise_information').show();
            });
            // 点击自有
            $('#Mediarelations_have').off('click').on('click', function () {
                // 隐藏代理合同
                $('#Certificate').hide();
                // 代理没有样式 自有样式
                $(this).find('div').attr('class', 'current_ifi');
                $('#Mediarelations_agent').find('div').attr('class', 'default_ifi');
                $('#Operatortype_personal').find('div').css('color', '#666');
                // 运营者类型
                // 点击个人
                $('#Operatortype_personal').off('click').on('click', function () {
                    // 企业没有样式 个人样式
                    $(this).find('div').attr('class', 'current_ifi');
                    $('#Operatortype_enterprise').find('div').attr('class', 'default_ifi');
                    // 个人显示,企业隐藏
                    $('#enterprise_information').hide();
                    $('#personal_information').show();
                });
                // 点击企业
                $('#Operatortype_enterprise').off('click').on('click', function () {
                    // 企业有样式 个人没样式
                    $(this).find('div').attr('class', 'current_ifi');
                    $('#Operatortype_personal').find('div').attr('class', 'default_ifi');
                    // 企业显示,个人隐藏
                    $('#enterprise_information').show();
                    $('#personal_information').hide();
                })
            });
            // 营业执照
            uploadImg('headUploadify_Businesslicense', 'headimgUploadFile_Businesslicense', 'headimgErr', 'bigImgshow_Businesslicense');
            $('#Businesslicense_Sample').mousemove(function () {
                $('#Businesslicense_img').show();
            }).mouseout(function () {
                $('#Businesslicense_img').hide();
            });
            // 代理合同证明
            // 首页
            uploadImg('headUploadify_homepage', 'headimgUploadFile_homepage', 'headimgErr', 'bigImgshow_homepage');
            // 尾页
            uploadImg('headUploadify_Shadowe', 'headimgUploadFile_Shadowe', 'headimgErr', 'bigImgshow_Shadowe');
            // 个人身份证图片
            // 正面
            uploadImg('headUploadify_Positive', 'headimgUploadFile_Positive', 'headimgErr', 'bigImgshow_Positive');
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
                // data= {
                //     "Status": 0,
                //     "Message": "成功",
                //     "Result": {
                //         "MediaRelations":1,
                //         "MediaRelationsName":"代理",
                //         "OperatingType":1,
                //         'enterprise': 111,
                //         "OperatingTypeName":"企业",
                //         "EnterpriseName":"企业名称",
                //         "BusinessLicense":"营业执照",
                //         "BusinessLicense":"/UploadFiles/2017/6/6/17/alert_logo$dca0e8f7-6a66-4e67-9f29-cd69257bca01_sl.gif",
                //         "Q1":"资质1",
                //         "Q2":"资质2",
                //         "CanEdit":false
                //     }
                // };
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
                        $('#headimgUploadFile_Businesslicense').attr('src',data.Result.BusinessLicense);
                        $('#bigImgshow_Businesslicense img').attr('src',data.Result.BusinessLicense);
                        // 代理合同
                        if(data.Result.Q1!=null){
                            $('#headimgUploadFile_homepage').attr('src',data.Result.Q1);
                            $('#bigImgshow_homepage img').attr('src',data.Result.Q1);
                        }
                        if(data.Result.Q2!=null){
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
                            $('#headimgUploadFile_Businesslicense').attr('src',data.Result.BusinessLicense);
                            $('#bigImgshow_Businesslicense img').attr('src',data.Result.BusinessLicense);
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
                            if(data.Result.Q1!=null){
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
        // 获取参数
        Getparameters: function () {
            /*==========start基本信息必填===========*/
            // app名称
            var app_name = objData.Result.Name;
            // 媒体Logo
            var LogoUrl = objData.Result.HeadIconURL;
            // 常见分类
            var Commonclass = objData.Result.CommonlyClass;
            // APP日活
            var Daily = objData.Result.DailyLive;
            // 媒体介绍
            var introduce = objData.Result.Remark;
            /*==========end基本信息必填===========*/

            /*==========start基本信息非必填===========*/
            // 覆盖地区
            var Covered = objData.Result.CityId;
            // 所在地区
            var ProvinceID =  objData.Result.ProvinceId;
            var CityID = objData.Result.CityId;
            // 下单备注
            var OrderRemark = objData.Result.OrderRemark;
            /*==========end基本信息非必填===========*/
            var obj = {
                "BusinessType": 14002,
                "operateType": GetRequest().OperateType?GetRequest().OperateType:1,
                "App": {
                    "Name": app_name,
                    "HeadIconURL": LogoUrl,
                    "OrderRemark": OrderRemark,
                    "CoverageArea": Covered,
                    "CommonlyClass": Commonclass,
                    "ProvinceId": ProvinceID,
                    "Cityid": CityID,
                    "Remark": introduce,
                    "DailyLive": Daily,
                    "Qualification": {}
                }
            };
            /*========strat相关资质=========*/
            // 媒体主提交相关资质
            if (CTLogin.RoleIDs == "SYS001RL00003") {
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
                var bigImgshow_opposite = $('#bigImgshow_opposite img').attr('src');//正面

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
            var Mediarelations = $('#Mediarelations .current_ifi').attr('MediaRelations') - 0;
            // 运营类型
            var Operationtype = $('#Operatortype .current_ifi').attr('OperatingType') - 0;
            // 标识
            var i = 0;
            // 代理
            if (Mediarelations == 50001) {
                // 判断企业名称
                if ($('#enterprise').val() == '') {
                    $('#enterprise_Prompt').show();
                    i++;
                }
                // 判断营业执照
                if ($('#bigImgshow_Businesslicense img').attr('src') == '') {
                    $('#Businesslicense_Prompt').show();
                    i++;
                }
                // 判断代理合同证明
                if ($('#bigImgshow_homepage img').attr('src') == '' || $('#bigImgshow_Shadowe img').attr('src') == '') {
                    $('#Certificate_Prompt').show();
                    i++;
                }
            }
            // 自有
            if (Mediarelations == 50002) {
                // 企业
                if (Operationtype == 1001) {
                    // 判断企业名称
                    if ($('#enterprise').val() == '') {
                        $('#enterprise_Prompt').show();
                        i++;
                    }
                    // 判断营业执照
                    if ($('#bigImgshow_Businesslicense img').attr('src') == '') {
                        $('#Businesslicense_Prompt').show();
                        i++;
                    }
                }
                // 个人
                if (Operationtype == 1002) {
                    // 真实姓名
                    if ($('#Realname').val() == '') {
                        $('#Realname_Prompt').show();
                        i++;
                    }
                    // 身份证图片
                    if ($('#bigImgshow_Positive img').attr('src') == '') {
                        $('#idcardpicture_Prompt').show();
                        i++;
                    }
                }
            }

            if (i > 0) {
                return false;
            } else {
                return true;
            }
        },
        //点击提交
        Submit: function () {
            var _this = this;
            $('#Submit').off('click').on('click', function () {
                // 标识
                var i = 0;
                // 判断相关资质填写
                // 媒体主
                if (CTLogin.RoleIDs == "SYS001RL00003") {
                    if (!_this.intelligence()) {
                        i++;
                    }
                }
                if (i > 0) {
                    return false;
                }
                ;
                // 添加媒体
                setAjax({
                    url: '/api/media/curd?v=1_1',
                    type: 'POST',
                    data: _this.Getparameters()
                }, function (data) {
                    if (data.Status == 0) {
                        layer.msg('提交成功', {time: 1000}, function () {
                            window.location = '/mediamanager/mediaAPP.html';
                        })
                    } else {
                        layer.msg(data.Message);
                    }
                })
            })
        }
    }

    var addmediaapp = new addMediaApp();
})