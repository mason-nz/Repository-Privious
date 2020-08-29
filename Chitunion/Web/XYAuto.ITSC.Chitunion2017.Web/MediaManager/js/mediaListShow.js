/**
 * Created by liushuai on 2017/2/23.
 */
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
// 行业分类
if (CTLogin.RoleIDs == "SYS001RL00001" || CTLogin.RoleIDs == "SYS001RL00004") {
    function IndustryCategory(i) {
        setAjax({
            url: '/api/DictInfo/GetDictInfoByTypeID',
            type: 'get',
            data: {
                'TypeID': i
            },
        }, function (data) {
            var str = '';
            var res = data.Result;
            str += '<option DictID="-2">不限</option>';
            for (var i = 0; i < res.length; i++) {
                str += ' <option DictID="' + res[i].DictId + '">' + res[i].DictName + '</option>'
            }
            $('#industry').html(str);

                if(GetRequest().categoryId!=undefined){
                    var a=true;
                    $("#industry option").each(function () {
                        if($(this).attr('dictid')==GetRequest().categoryId){
                            $(this).attr("selected","selected");
                            $('#search').click();
                            a=false;
                        }
                    })
                    if(a){
                        alert('行业分类不存在');
                    }
                }

        })
    }

    function Authentication() {
        setAjax({
            url: '/api/DictInfo/GetDictInfoByTypeID',
            type: 'get',
            data: {
                'TypeID': 30
            },
        }, function (data) {
            var str = '';
            var res = data.Result;
            str += '<option value="-2">不限</option>';
            for (var i = 0; i < res.length; i++) {
                str += ' <option value="' + res[i].DictId + '">' + res[i].DictName + '</option>'
            }
            $('#certified').html(str);
        })
    }
}
/*初始化页面加载*/
$(function () {
    if (CTLogin.IsLogin) {
        $('#loginName').text(CTLogin.RealName);
    } else {
        $('#loginName').val('未登录');
    }

    var typeid = '26';
    var mediaType = $('.media_add_btn').attr('type-id');
    if (mediaType == '14005')
        typeid = '34';
    setAjax({
        url: '/api/DictInfo/GetDictInfoByTypeID',
        type: 'get',
        data: {TypeID: typeid}
    }, function (data) {
        var str = '';
        var res = data.Result;
        str += '<option DictID="-2">不限</option>';
        for (var i = 0; i < res.length; i++) {
            str += ' <option DictID="' + res[i].DictId + '">' + res[i].DictName + '</option>'
        }
        $('#liveNum').html(str);
        $('#videoName').html(str);
    });

    $('#createDate').off('click').on('click', function () {
        laydate({
            fixed: false,
            elem: '#createDate',
            choose: function (date) {
                if (date > $('#createDate1').val() && $('#createDate1').val()) {
                    layer.alert('起始时间不能大于结束时间！');
                    $('#createDate').val('')
                }
            }
        });
    });
    $('#createDate1').off('click').on('click', function () {
        laydate({
            fixed: false,
            elem: '#createDate1',
            choose: function (date) {
                if (date < $('#createDate').val() && $('#createDate').val()) {
                    layer.alert('结束时间不能小于起始时间！');
                    $('#createDate1').val('')
                }
            }
        });
    });

});

var chooseNum = function () {
    var num = 1;
    var mediaType = $('.media_add_btn').attr('type-id');
    switch (mediaType) {
        case'14001':
            num = 1;
            break;
        case'14002':
            num = 5;
            break;
        case'14003':
            num = 2;
            break;
        case'14004':
            num = 3;
            break;
        case'14005':
            num = 4;
            break;
    }
    judgeAuthority(num);
};

/**
 * Written by:     liushuai
 * function: 微信公众号列表查询
 * Created Date:   2017-02-24
 * Modified Date:   2017-02-26
 * obj:查询条件以一个对象格式传入
 * */

function searchList(obj) {
    var sysID = CTLogin.RoleIDs;
    var temp = '#tmpl1';//默认渲染第一个模板
    var obj1=obj.MediaType;
    setAjax({
            url: '/api/Media/GetMediaList',
            type: 'get',
            data: obj
        },
        function (data) {
            var status = data.Status;
            if (status == 0) {
                var arr = data.Result.List;
                var counts = data.Result.TotalCount;
                /*判断如果是AE与媒体住都不显示来源跟录入人*/
                if (sysID != 'SYS001RL00005' && sysID != 'SYS001RL00003') {
                    temp = '#tmpl1';
                    $('.origin').css('display', 'block');
                    $('.createPeo').css('display', 'block');
                } else {
                    temp = '#tmpl2';
                    $('.origin').css('display', 'none');
                    $('.createPeo').css('display', 'none');
                }

                if (arr.length != 0) {
                    var str = $(temp).html();
                    var html = ejs.render(str, {list: arr});
                    $('.list_table').html(html);
                    $('#pageContainer').show();
                    var mediaID = $('.details').attr('media-id');

                    $('.transAPP').attr('href', '/PublishManager/auditPublishAPP.html?MediaID=' + mediaID)
                } else {
                    if (temp == '#tmpl1') {
                        var str = $('#tmpl3').html();
                        var html = ejs.render(str, {list: arr});
                        $('.list_table').html(html);
                        var mediaID = $('.details').attr('media-id');
                        $('.transAPP').attr('href', '/PublishManager/auditPublishAPP.html?MediaID=' + mediaID)
                    } else {
                        var str = $('#tmpl4').html();
                        var html = ejs.render(str, {list: arr});
                        $('.list_table').html(html);
                    }
                    $('#pageContainer').hide()

                }
                // 当为管理员和运营时隐藏录用人和录用日期
                if (sysID == "SYS001RL00001" || sysID == "SYS001RL00004") {
                    $('.createPeo').css('display', 'none');
                    $('.createDtae').css('display', 'none');
                }
                console.log(obj);
                if (obj1 == 14003) {
                    Recommend_sina()
                }
                if (obj1 == 14001) {
                    Recommend_wechat()
                }
                if (obj1 == 14004) {
                    Recommend_vedio()
                }
                if (obj1 == 14005) {
                    Recommend_zhibo()
                }
                chooseNum();
                $('#pageContainer').pagination(
                    counts,
                    {
                        items_per_page: 20,
                        callback: function (currPage, jg) {
                            obj.PageIndex=currPage
                            setAjax({
                                    url: '/api/Media/GetMediaList',
                                    type: "get",
                                    data: obj
                                },
                                function (data) {
                                    var status = data.Status;
                                    if (status == 0) {
                                        var arr = data.Result.List;
                                        var counts = data.Result.TotalCount;
                                        if (sysID != 'SYS001RL00005') {
                                            temp = '#tmpl1';
                                            $('.origin').css('display', 'block');
                                            $('.createPeo').css('display', 'block');
                                        } else {
                                            temp = '#tmpl2';
                                            $('.origin').css('display', 'none');
                                            $('.createPeo').css('display', 'none');
                                        }
                                        if (arr.length != 0) {
                                            var str = $(temp).html();
                                            var html = ejs.render(str, {list: arr});
                                            $('.list_table').html(html);
                                            $('#pageContainer').show();
                                            var mediaID = $('.details').attr('media-id');

                                            $('.transAPP').attr('href', '/PublishManager/auditPublishAPP.html?MediaID=' + mediaID)
                                        } else {
                                            if (temp == '#tmpl1') {
                                                var str = $('#tmpl3').html();
                                                var html = ejs.render(str, {list: arr});
                                                $('.list_table').html(html);
                                                var mediaID = $('.details').attr('media-id');

                                                $('.transAPP').attr('href', '/PublishManager/auditPublishAPP.html?MediaID=' + mediaID)
                                            } else {
                                                var str = $('#tmpl4').html();
                                                var html = ejs.render(str, {list: arr});
                                                $('.list_table').html(html);
                                            }
                                            $('#pageContainer').hide()
                                        }
                                        // 当为管理员和运营时隐藏录用人和录用日期
                                        if (sysID == "SYS001RL00001" || sysID == "SYS001RL00004") {
                                            $('.createPeo').css('display', 'none');
                                            $('.createDtae').css('display', 'none');
                                        }
                                        chooseNum();
                                        console.log(obj);
                                        if (obj1 == 14003) {
                                            Recommend_sina()
                                        }
                                        if (obj1 == 14001) {
                                            Recommend_wechat()
                                        }
                                        if (obj1 == 14004) {
                                            Recommend_vedio()
                                        }
                                        if (obj1 == 14005) {
                                            Recommend_zhibo()
                                        }
                                    }

                                }
                            );
                        }
                    }
                );

                /*   var NewPage = new PaginationController({
                 WrapContainer: "#pageContainer",
                 DisabledClassName: ".disabled",//不可点按钮的class名，可选
                 CurrentClassName: ".current",//选中状态按钮class名，可选
                 EnableClickClassName: ".EnableClick",//可点击按钮class名，可选
                 NormalTextClassName: ".NormalText",//普通文本class名，可选，
                 MaxPage: counts,
                 PageItemCount: 20,
                 ControllerCount: 5,
                 CallBack: function (currentPageIndex, callback) {
                 setAjax({
                 url: '/api/Media/GetMediaList?PageIndex=' + currentPageIndex,
                 type: "get",
                 data: obj
                 },
                 function (data) {
                 var status = data.Status;
                 if (status == 0) {
                 var arr = data.Result.List;
                 var counts = data.Result.TotalCount;
                 if (sysID != 'SYS001RL00005') {
                 temp = '#tmpl1';
                 $('.origin').css('display', 'block');
                 $('.createPeo').css('display', 'block');
                 } else {
                 temp = '#tmpl2';
                 $('.origin').css('display', 'none');
                 $('.createPeo').css('display', 'none');
                 }
                 if (arr.length != 0) {
                 var str = $(temp).html();
                 var html = ejs.render(str, {list: arr});
                 $('.list_table').html(html);
                 $('#pageContainer').show();
                 var mediaID=$('.details').attr('media-id');

                 $('.transAPP').attr('href','/PublishManager/auditPublishAPP.html?MediaID='+mediaID)
                 } else {
                 if(temp=='#tmpl1'){
                 var str = $('#tmpl3').html();
                 var html = ejs.render(str, {list: arr});
                 $('.list_table').html(html);
                 var mediaID=$('.details').attr('media-id');

                 $('.transAPP').attr('href','/PublishManager/auditPublishAPP.html?MediaID='+mediaID)
                 }else{
                 var str = $('#tmpl4').html();
                 var html = ejs.render(str, {list: arr});
                 $('.list_table').html(html);
                 }
                 $('#pageContainer').hide()
                 }
                 var curMediaType = $('.but_add').attr('type-id').slice(4);
                 chooseNum();
                 }
                 }
                 );
                 callback(true);
                 }
                 });
                 NewPage.createPageItemFu(1)*/
            }
        },
        function (err) {

        }
    );
}


// 微博推荐到首页
function Recommend_sina() {
    $('.recommend').off('click').on('click', function () {
        var _this = $(this)
        setAjax({
            url: '/api/recommend/add',
            type: 'POST',
            data: {
                MediaId: _this.attr('name'),
                BusinessType: 14003
            }
        }, function (data) {
            if (data.Status != 0) {
                alert(data.Message)
                return
            }
            _this.hide();
        })
    })
}

function Recommend_wechat() {
    $('.recommend').off('click').on('click',function () {
        var _this=$(this)
        setAjax({
            url:'/api/recommend/add',
            type:'POST',
            data: {
                MediaId:_this.attr('name'),
                BusinessType:14001
            }
        },function (data) {
            if(data.Status!=0){
                alert(data.Message)
                return
            }
            _this.hide();
        })
    })
}

function Recommend_vedio() {
    $('.recommend').off('click').on('click', function () {
        var _this = $(this)
        $.openPopupLayer({
            name: "closedown1",
            url: "/MediaManager/supplement.html",
            error: function (dd) {
                alert(dd.status);
            },
            success: function () {
                $('#Upname').html(_this.attr('data-name'))
                function uploadFile(UploadifyDoc) {
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
                    };
                    function escapeStr(str) {
                        return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
                    };
                    if (UploadifyDoc == "#UploadifyDoc2") {
                        var jpg='支持格式:jpg,jpeg,png';
                        var jpeg='*.jpg;*.jpeg;*.png;'
                    }
                    if (UploadifyDoc == "#UploadifyDoc1") {
                        var jpg='支持格式:mp4';
                        var jpeg='*.mp4;'
                    }
                    $(UploadifyDoc).uploadify({
                        'buttonText': '+ 上传附件',
                        'buttonClass': 'but_upload',
                        'swf': '/Js/uploadify.swf?_=' + Math.random(),
                        'uploader': '/AjaxServers/UploadFile.ashx',
                        'auto': true,
                        'multi': false,
                        'width': 200,
                        'height': 35,
                        'formData': {
                            Action: 'BatchImport',
                            CarType: '',
                            LoginCookiesContent: escapeStr(getCookie('ct-uinfo'))
                        },
                        'fileTypeDesc': jpg,
                        'fileTypeExts': jpeg,
                        'queueSizeLimit': 2,
                        'fileSizeLimit': '1GB',
                        'scriptAccess': 'always',
                        'onQueueComplete': function (event, data) {
                            //enableConfirmBtn();
                        },
                        'onQueueFull': function () {
                            layer.alert('您最多只能上传1个文件！');
                            return false;
                        },
                        'onUploadSuccess': function (file, data, response) {
                            if (response == true) {
                                var json = $.evalJSON(data);
                                if (UploadifyDoc == '#UploadifyDoc2') {
                                    $(".fileBox1").show();
                                    $("#FileName1").text(json.FileName);
                                    $("#downloadFile1").attr("href", json.Msg);
                                }
                                if (UploadifyDoc == '#UploadifyDoc1') {
                                    $(".fileBox2").show();
                                    $("#FileName2").text(json.FileName);
                                    $("#downloadFile2").attr("href", json.Msg);
                                }
                            }
                        },
                        'onProgress': function (event, queueID, fileObj, data) {
                        },
                        'onUploadError': function (event, queueID, fileObj, errorObj) {
                            //enableConfirmBtn();
                        },
                        'onSelectError': function (file, errorCode, errorMsg) {
                            console.log(errorCode);
                            /*if (errorCode == SWFUpload.UPLOAD_ERROR.FILE_CANCELLED
                             || errorCode == SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED) {
                             return;
                             }
                             switch(errorCode) {
                             case -100:
                             $('#'+imgerr).html('上传图片数量超过1个');
                             break;
                             case -110:
                             $('#'+imgerr).html('上传图片大小应小于2MB');
                             break;
                             case -120:

                             break;
                             case -130:
                             $('#'+imgerr).html('上传图片类型不正确');
                             break;
                             }*/
                        }
                    });

                };
                uploadFile('#UploadifyDoc2');
                uploadFile('#UploadifyDoc1');
                $('.keep .button').off('click').on('click', function () {
                    var flag1=1,flag2=1;
                    if($("#FileName1").text()==''){
                        $('#xianshi1').text('图片不能为空').css('display','block');
                        flag1=0;
                    }
                    if($("#FileName2").text()==''){
                        $('#xianshi2').text('视频不能为空').css('display','block');
                        flag2=0;
                    }
                    if(flag1==0||flag2==0){
                        return
                    }
                    setAjax({
                        url:'/api/recommend/add',
                        type:'POST',
                        data:{
                            BusinessType:14004,
                            MediaId:_this.attr('name'),
                            ImageUrl:$('#downloadFile1').attr('href'),
                            VideoUrl:$('#downloadFile2').attr('href')
                        }
                    },function (data) {
                        if(data.Status!=0){
                            alert(data.Message)
                            return
                        }
                        _this.hide();
                        $.closePopupLayer('closedown1');
                    })
                })
                $('#closebt').off('click').on('click', function () {
                    $.closePopupLayer('closedown1');
                })
                $('.keep .but_keep').off('click').on('click', function () {
                    $.closePopupLayer('closedown1');
                })
            }
        })

    })
}

function Recommend_zhibo() {
    $('.recommend').off('click').on('click',function () {
        var _this=$(this)
        setAjax({
            url:'/api/recommend/add',
            type:'POST',
            data: {
                MediaId:_this.attr('name'),
                BusinessType:14005
            }
        },function (data) {
            if(data.Status!=0){
                alert(data.Message)
                return
            }
            _this.hide();
        })
    })
}





