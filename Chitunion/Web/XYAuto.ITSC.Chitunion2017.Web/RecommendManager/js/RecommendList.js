/// <reference path="../..jquery.1.11.3.min.js" />
/// <reference path="../../js/Common_chitu.js" />
/// <reference path="../../js/ejs.min.js" />
/// <reference path="../../js/layer/layer.js" />

//Auth:lixiong
//Date:2017-04-12
//Description:

"use strict";

(function ($) {
    $.recommend = {
        businesstype: 14001
    };
    $.extend($.recommend, {
        //businesstype: -1,
        categoryId: -2,
        btnSearch: $("#btnSearch"),
        btnSelectMedia: $("#selectMedia"),
        btnPublishOperate: $("#publishOperate"),
        InputSearchKw: $("#searchKw"),
        rendContent: $("#contentList"),
        pageContainer: $("#pageContainer"),
        selectTable: $("#tab_list .tab_menu"),
        currentDomain: "",
        ajaxSearchUrl: '/api/recommend/query',
        ajaxPostDeleteUrl: "/api/recommend/deleteInfo",
        ajaxPostUpdateUrl: "/api/recommend/update",
        ajaxPostPublishUrl: "/api/recommend/Publish",
        ajaxSearchData: {
            pageIndex: 1,
            pageSize: 20,
            businesstype: 0,
            categoryId: -2,
            mediaName: ""
        },
        ajaxPostPublishData: {
            categoryId: -1,
            businesstype: 0
        },
        ajaxPostUpdateData: {
            RecID: 0,
            SortNumber: 0,
            ImageUrl: "",
            VideoUrl: ""
        },
        constBusinesstype: {
            weixin: 14001,
            app: 14002,
            weibo: 14003,
            video: 14004,
            Broadcast: 14005
        },
        ajaxSuccessTips: {
            publishSuccessMsg: "发布成功",
            uploadImageErrorMsg: "请上传图片",
            uploadVideoErrorMsg: "请上传视频"
        },
        init: function (options) {
            //初始化参数
            $.extend($.recommend, options);

            //取到分类第一栏
            //$.recommend.selectTable.find('.selected').data('catid');

            //分类附加到程序
            $.recommend.ajaxSearchData.businesstype = $.recommend.businesstype;
            if ($.recommend.businesstype !== $.recommend.businesstype.app) {
                $.recommend.ajaxSearchData.categoryId = $.recommend.selectTable.find('.selected').data('catid');
            }

            //初始化查询
            $.recommend.ajaxFunction.ajaxSearchRecommendList();

            //点击查询按钮
            $.recommend.btnSearch.on("click", function () {
                //if ($.trim($.recommend.InputSearchKw.val()).length) {
                $.recommend.ajaxSearchData.pageIndex = 1;
                $.recommend.ajaxSearchData.mediaName = $.recommend.InputSearchKw.val();
                $.recommend.ajaxFunction.ajaxSearchRecommendList();
                //}
            });

            //选择分类查询
            $.recommend.selectTable.delegate("li", "click", function () {
                var self = $(this);
                self.addClass('selected').siblings().removeClass('selected');

                $.recommend.ajaxSearchData.pageIndex = 1;
                $.recommend.ajaxSearchData.mediaName = $.trim($.recommend.InputSearchKw.val());
                $.recommend.ajaxSearchData.categoryId = self.data('catid');
                $.recommend.ajaxFunction.ajaxSearchRecommendList();
            });

            //发布操作
            $.recommend.btnPublishOperate.on("click", function () {
                $.recommend.ajaxFunction.ajaxPostPublish();
            });

            //选择媒体添加
            $.recommend.btnSelectMedia.on("click", function () {
                var categoryId = $.recommend.selectTable.find(".selected").data('catid');

                switch ($.recommend.businesstype) {
                    case $.recommend.constBusinesstype.app:
                        window.location.href = "/publishmanager/advertisinglist_app.html";
                        break;
                    case $.recommend.constBusinesstype.weixin:
                        window.location.href = "/MediaManager/mediaWeChatList_new.html?categoryId=" + categoryId;
                        break;
                    case $.recommend.constBusinesstype.video:
                        window.location.href = "/MediaManager/mediaVideoList.html?categoryId=" + categoryId;
                        break;
                    case $.recommend.constBusinesstype.weibo:
                        window.location.href = "/MediaManager/mediaBlogList.html?categoryId=" + categoryId;
                        break;
                    case $.recommend.constBusinesstype.Broadcast:
                        window.location.href = "/MediaManager/mediaLiveList.html?categoryId=" + categoryId;
                        break;
                    default:
                        return "#";
                }
            });

            $.recommend.ajaxFunction.LoadImage();
        },
        ajaxFunction: {
            ajaxSearchRecommendList: function () {
                //查询列表
                var set = {
                    //selector: '.list',
                    url: $.recommend.ajaxSearchUrl,
                    type: "get",
                    data: $.recommend.ajaxSearchData
                }

                setAjax(set,
                    function (data) {
                        //TODO  :处理逻辑
                        if (data.Status === 0) {
                            $(set.selector).hide();
                            if (data.Result.TotleCount > 0) {
                                var html = $.recommend.ajaxFunction.ajaxRenderView(data.Result.List);
                                $.recommend.rendContent.html(html);
                                //隐藏loading

                                //分页处理
                                $.recommend.ajaxFunction.ajaxPageContainer(data.Result.TotleCount);
                            } else {
                                $.recommend.rendContent.html('');
                                $.recommend.pageContainer.html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                            }
                        }
                    }
                );
            }
            , ajaxPageContainer: function (counts) {
                //分页
                $('#pageContainer').pagination(
                  counts,
                    {
                        items_per_page: $.recommend.ajaxSearchData.pageSize,
                        callback: function (currPage) {
                            $.recommend.ajaxSearchData.pageIndex = currPage;
                            setAjax({
                                url: $.recommend.ajaxSearchUrl,
                                type: "get",
                                data: $.recommend.ajaxSearchData
                            }, function (data) {
                                if (data.Status === 0) {
                                    if (data.Result.TotleCount > 0) {
                                        var html = $.recommend.ajaxFunction.ajaxRenderView(data.Result.List);
                                        $.recommend.rendContent.html(html);
                                        //$.recommend.pageContainer.html('');
                                    } else {
                                        $.recommend.rendContent.html('');
                                        $.recommend.pageContainer.html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">');
                                    }
                                }
                            }
                            );
                        }
                    }
                );
            }
            , ajaxPostPublish: function () {
                //发布操操作

                $.recommend.ajaxPostPublishData.businesstype = $.recommend.ajaxSearchData.businesstype;
                $.recommend.ajaxPostPublishData.categoryId = $.recommend.ajaxSearchData.categoryId;

                $.ajax({
                    type: "POST",
                    url: $.recommend.ajaxPostPublishUrl,
                    data: $.recommend.ajaxPostPublishData,
                    xhrFields: { withCredentials: true },
                    crossDomain: true,
                    dataType: "json",
                    success: function (data) {
                        //TODO  :处理逻辑
                        if (data.Status === 0) {
                            var index = layer.alert($.recommend.ajaxSuccessTips.publishSuccessMsg, { closeBtn: 0 }, function () {
                                window.open("/index.html");
                                layer.close(index);
                            });
                        } else {
                            layer.alert(data.Message, { closeBtn: 0 });
                        }
                    },
                    error: function (msg) {
                        console.log(msg);
                    }
                });
            }
            , ajaxPostDelete: function (selectRecId) {
                //删除
                var index = layer.confirm('是否要删除？', { btn: ['是', '否'] }, function () {
                    $.ajax({
                        type: "POST",
                        url: $.recommend.ajaxPostDeleteUrl,
                        data: { RecID: selectRecId },
                        xhrFields: { withCredentials: true },
                        crossDomain: true,
                        dataType: "json",
                        success: function (data) {
                            //TODO  :处理逻辑
                            layer.close(index);
                            if (data.Status === 0) {
                                $("#tr_" + selectRecId).remove();
                            } else {
                                layer.alert(data.Message, { closeBtn: 0 });
                            }
                        },
                        error: function (msg) {
                            layer.alert(msg, { closeBtn: 0 });
                        }
                    });
                });
            }
            , ajaxPostUpdate: function (recId, operateType) {
                var updateSortNum = Number($("#editShowInput_" + recId).val()) || -1;
                if (recId <= 0 || isNaN(updateSortNum)) {
                    layer.alert("请输入正确的数字类型", { closeBtn: 0 });
                    return;
                }
                $.recommend.ajaxPostUpdateData.SortNumber = updateSortNum;
                $.recommend.ajaxPostUpdateData.RecID = recId;

                if ($.recommend.businesstype === $.recommend.businesstype.video) {
                    //视频有图片
                    //取到参数视频和图片
                    if (!$.recommend.ajaxPostUpdateData.ImageUrl.length || !$.recommend.ajaxPostUpdateData.VideoUrl.length) {
                        layer.alert($.recommend.ajaxSuccessTips.uploadImageErrorMsg + '&nbsp;或&nbsp;' +
                            $.recommend.ajaxSuccessTips.uploadVideoErrorMsg
                            , { closeBtn: 0 });
                        return;
                    }
                }
                var index = layer.load();
                //修改
                $.ajax({
                    type: "POST",
                    url: $.recommend.ajaxPostUpdateUrl,
                    data: $.recommend.ajaxPostUpdateData,
                    xhrFields: { withCredentials: true },
                    crossDomain: true,
                    dataType: "json",
                    success: function (data) {
                        //TODO  :处理逻辑
                        layer.close(index);
                        if (data.Status === 0) {
                            if (operateType === 1) {
                                $.closePopupLayer('closedown1');
                            }
                            $.recommend.ajaxFunction.ajaxSearchRecommendList();
                        } else {
                            layer.alert(data.Message, { closeBtn: 0 });
                        }
                    },
                    error: function (msg) {
                        layer.alert(msg, { closeBtn: 0 });
                    }
                });
            }
            , UpdateBefore: function (recId) {
                //展示编辑文本框，隐藏展示
                $("#clickEdit_" + recId).hide();
                $("#editLable_" + recId).hide();
                $("#clickEdit2_" + recId).show();
                $("#editShowInput_" + recId).show();
            }
            , UpdateCancel: function (recId) {
                $("#clickEdit_" + recId).show();
                $("#editLable_" + recId).show();
                $("#clickEdit2_" + recId).hide();
                $("#editShowInput_" + recId).hide();
            }
            , LoadImage: function () {
                /*图例弹层*/

                $("#contentList").delegate("[name='loadImageAdd']", "click", function () {
                    var url = $(this).attr('picurl');
                    $.openPopupLayer({
                        name: 'popLayerDemo',
                        url: '/OrderManager/layer.html',
                        error: function (dd) {
                            alert(dd.status);
                        },
                        success: function () {
                            $('.layer_con2 img').attr('src', url);
                            $('.layer_con2').html('<img src="' + url + '" width="350" height="420">');

                            $('#popupLayerScreenLocker').click(function () {
                                $.closePopupLayer('popLayerDemo');
                            });
                        }
                    });
                });
            }, UpdateImageVideo: function (recId, imageUrl, videoUrl, name) {
                //媒体为视频的时候，有编辑图片和视频内容地址
                //点击编辑按钮、在视频/图片一栏会出现 图片编辑按钮
                //点击图片编辑按钮、弹出层，选择文件上传
                //上传成功回写到页面存储

                $.openPopupLayer({
                    name: "closedown1", //用于关闭弹层
                    url: "uploadFile.html", // 弹层路径
                    error: function (dd) {
                        alert(dd.status);
                    },
                    success: function () {
                        //显示弹出窗后的逻辑在这写

                        $.recommend.ajaxFunction.UpdateImageVideoLoad(imageUrl, videoUrl, name);

                        $.recommend.ajaxFunction.ajaxUploadFile('#UploadifyImage');
                        $.recommend.ajaxFunction.ajaxUploadFile('#UploadifyVideo');
                        $('.keep .button').off('click').on("click", function () {
                            //确认按钮、这里不提交postUpdate
                            //console.log($.recommend.ajaxPostUpdateData);

                            if (!$.recommend.ajaxPostUpdateData.ImageUrl.length || !$.recommend.ajaxPostUpdateData.VideoUrl.length) {
                                layer.alert($.recommend.ajaxSuccessTips.uploadImageErrorMsg + '&nbsp;或&nbsp;' +
                                    $.recommend.ajaxSuccessTips.uploadVideoErrorMsg
                                    , { closeBtn: 0 });
                                return;
                            }
                            //提交更新文件
                            $.recommend.ajaxFunction.ajaxPostUpdate(recId);

                            $.closePopupLayer('closedown1'); //关闭弹窗
                        });
                        $('#closebt').off('click').on('click', function () {
                            $.closePopupLayer('closedown1');
                        });
                        $('.keep .but_keep').off('click').on('click', function () {
                            $.closePopupLayer('closedown1');
                        });
                    }
                });
            }
            , UpdateImageVideoLoad: function (imageUrl, videoUrl, name) {
                //修改的时候默认展示信息
                //imageUrl = "/UploadFiles/2017/4/14/20/84481_20130116142820494200_1$0a4bd208-28d6-4d17-80e1-3c8e8b704b0b.jpg
                imageUrl = imageUrl.toLowerCase() === 'null' ? '' : imageUrl;
                videoUrl = videoUrl.toLowerCase() === 'null' ? '' : videoUrl;
                var imageFileName = imageUrl.substr(imageUrl.lastIndexOf('/') + 1);
                var videoFileName = videoUrl.substr(videoUrl.lastIndexOf('/') + 1);

                $("#Upname").html(name);

                $(".fileBox1").show();
                $("#FileNameImage").text(imageFileName);
                $.recommend.ajaxPostUpdateData.ImageUrl = imageUrl;
                $("#downloadFileImage").attr("href", imageUrl.length > 0 ?
                    $.recommend.ajaxFunction.funcUitl.urlDomain(imageUrl) : 'javascript:;');

                $(".fileBox2").show();
                $("#FileNameVideo").text(videoFileName);
                $.recommend.ajaxPostUpdateData.VideoUrl = videoUrl;
                $("#downloadFileVideo").attr("href", videoUrl.length > 0 ?
                    $.recommend.ajaxFunction.funcUitl.urlDomain(videoUrl) : 'javascript:;');
            }
            , ajaxUploadFile: function (uploadifyDoc) {
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

                var jpg, jpeg;
                if (uploadifyDoc === "#UploadifyImage") {
                    jpg = '支持格式:jpg,jpeg,png';
                    jpeg = '*.jpg;*.jpeg;*.png;';
                }
                if (uploadifyDoc === "#UploadifyVideo") {
                    jpg = '支持格式:mp4';
                    jpeg = '*.MP4;';
                }
                $(uploadifyDoc).uploadify({
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
                    'fileSizeLimit': '11111MB',
                    'scriptAccess': 'always',
                    'onQueueComplete': function (event, data) {
                        //enableConfirmBtn();
                    },
                    'onQueueFull': function () {
                        layer.alert('您最多只能上传1个文件！');
                        return false;
                    },
                    'onUploadSuccess': function (file, data, response) {
                        //console.log(data);
                        if (response == true) {
                            var json = $.evalJSON(data);
                            if (uploadifyDoc === '#UploadifyImage') {
                                $(".fileBox1").show();
                                $("#FileNameImage").text(json.FileName);
                                $.recommend.ajaxPostUpdateData.ImageUrl = $.recommend.currentDomain + json.Msg;
                                //$("#downloadFileImage").attr("href", "" + json.Msg);
                            }
                            if (uploadifyDoc === '#UploadifyVideo') {
                                $(".fileBox2").show();
                                $("#FileNameVideo").text(json.FileName);
                                $.recommend.ajaxPostUpdateData.VideoUrl = $.recommend.currentDomain + json.Msg;
                                //$("#downloadFileVideo").attr("href", "" + json.Msg);
                            }
                        }
                    }
                });
            }
            , funcUitl: {
                urlDomain: function (url) {
                    if (url == null || url.toLowerCase() === 'null') {
                        return 'null';
                    }
                    if (!url.length) {
                        return url;
                    }

                    if (url.indexOf('http://') > -1 || url.indexOf('https://') > -1) {
                        return url;
                    } else {
                        if (url.indexOf($.recommend.currentDomain) > -1) {
                            return url;
                        } else {
                            return $.recommend.currentDomain + url;
                        }
                    }
                }
            }
            , ajaxRenderView: function (data) {
                var arrPush = new Array();
                if (data.length === 0) {
                    return "";
                }
                var count = data.length;
                switch ($.recommend.businesstype) {
                    case 14001://weixin
                        for (var i = 0; i < count; i++) {
                            var item = data[i];
                            arrPush.push('<tr id=tr_' + item.RecID + '>');
                            arrPush.push("               <td>");
                            arrPush.push('                  <div class="portrait">');
                            arrPush.push('                       <img src="' + $.recommend.ajaxFunction.funcUitl.urlDomain(item.HeadIconURL) + '"></div>');
                            arrPush.push('                   <div class="public">');
                            arrPush.push("                       <h2>" + item.Name + "</h2>");
                            arrPush.push("                   </div>");
                            arrPush.push('                   <div class="clear"></div>');
                            arrPush.push("               </td>");
                            arrPush.push("               <td>" + item.FansCount + "</td>");
                            arrPush.push("               <td>" + item.UpdateCount + "</td>");
                            arrPush.push("               <td>" + item.AveragePointCount + "</td>");
                            arrPush.push("               <td>" + item.MaxinumReading + "</td>");
                            arrPush.push("               <td>" + item.ReferReadCount + "</td>");
                            //arrPush.push("               <td>" + item.SortNumber + "</td>");
                            arrPush.push("               <td><lable id='editLable_" + item.RecID + "'>" + item.SortNumber + "</lable>");
                            arrPush.push('                   <input  id="editShowInput_' + item.RecID + '"  type="text" style="width: 60px; height: 29px; line-height: 22px; font-size: 12px;display:none" value="' + item.SortNumber + '"></td>');
                            arrPush.push('<td><div id=clickEdit_' + item.RecID + '>' +
                                '<a href="javascript:$.recommend.ajaxFunction.UpdateBefore(' + item.RecID + ');">编辑</a>' +
                                '&nbsp;<a href="javascript:$.recommend.ajaxFunction.ajaxPostDelete(' + item.RecID + ');">删除</a>' +
                                '</div>' +
                                '<div style="display:none" id=clickEdit2_' + item.RecID + '>' +
                                 '<a href="javascript:$.recommend.ajaxFunction.ajaxPostUpdate(' + item.RecID + ');">更新</a>' +
                                  '&nbsp;<a href="javascript:$.recommend.ajaxFunction.UpdateCancel(' + item.RecID + ');">取消</a>' +
                                '</div>' +
                                '</td>');

                            //arrPush.push('               <td><a id="clickEdit_' + item.RecID + '" href="javascript:$.recommend.ajaxFunction.UpdateBefore(' + item.RecID + ');">编辑</a> ' +
                            //    '<a id="clickEdit2_' + item.RecID + '" style="display:none" href="javascript:$.recommend.ajaxFunction.ajaxPostUpdate(' + item.RecID + ');">更新</a> ' +
                            //    '&nbsp;<a href="javascript:$.recommend.ajaxFunction.ajaxPostDelete(' + item.RecID + ')">删除</a></td>');
                            arrPush.push("           </tr>");
                        }
                        break;
                    case 14002://app
                        for (var j = 0; j < count; j++) {
                            var itemj = data[j];
                            arrPush.push('<tr id=tr_' + itemj.RecID + '>');
                            arrPush.push("               <td>");
                            arrPush.push('                  <div class="portrait">');
                            arrPush.push('                       <img src="' + $.recommend.ajaxFunction.funcUitl.urlDomain(itemj.HeadIconURL) + '"></div>');
                            arrPush.push('                   <div class="public">');
                            arrPush.push("                       <h2>" + itemj.Name + "</h2>");
                            arrPush.push("                   </div>");
                            arrPush.push('                   <div class="clear"></div>');
                            arrPush.push("               </td>");
                            arrPush.push("               <td>" + itemj.AdPosition + "</td>");
                            arrPush.push("               <td>" + itemj.AdForm + "</br>" +
                                "<div><a href='javascript:;' name='loadImageAdd' class='blue3 picDemo' picurl=" + $.recommend.ajaxFunction.funcUitl.urlDomain(itemj.AdLegendURL) + ">查看图例</a></div>" +
                                "</td>");
                            //arrPush.push("               <td>" + itemj.AveragePointCount + "</td>");
                            arrPush.push("               <td><lable id='editLable_" + itemj.RecID + "'>" + itemj.SortNumber + "</lable>");
                            arrPush.push('                   <input  id="editShowInput_' + itemj.RecID + '"  type="text" style="width: 60px; height: 29px; line-height: 22px; font-size: 12px;display:none" value="' + itemj.SortNumber + '"></td>');
                            arrPush.push('<td><div id=clickEdit_' + itemj.RecID + '>' +
                                '<a href="javascript:$.recommend.ajaxFunction.UpdateBefore(' + itemj.RecID + ');">编辑</a>' +
                                '&nbsp;<a href="javascript:$.recommend.ajaxFunction.ajaxPostDelete(' + itemj.RecID + ');">删除</a>' +
                                '</div>' +
                                '<div style="display:none" id=clickEdit2_' + itemj.RecID + '>' +
                                 '<a href="javascript:$.recommend.ajaxFunction.ajaxPostUpdate(' + itemj.RecID + ');">更新</a>' +
                                  '&nbsp;<a href="javascript:$.recommend.ajaxFunction.UpdateCancel(' + itemj.RecID + ');">取消</a>' +
                                '</div>' +
                                '</td>');
                            arrPush.push("           </tr>");
                        }

                        break;
                    case 14003://weibo
                        for (var k = 0; k < count; k++) {
                            var itemk = data[k];
                            arrPush.push('<tr id=tr_' + itemk.RecID + '>');
                            arrPush.push("               <td>");
                            arrPush.push('                  <div class="portrait">');
                            arrPush.push('                       <img src="' + $.recommend.ajaxFunction.funcUitl.urlDomain(itemk.HeadIconURL) + '"></div>');
                            arrPush.push('                   <div class="public">');
                            arrPush.push("                       <h2>" + itemk.Name + "</h2>");
                            arrPush.push("                   </div>");
                            arrPush.push('                   <div class="clear"></div>');
                            arrPush.push("               </td>");
                            arrPush.push("               <td>" + itemk.FansCount + "</td>");
                            arrPush.push("               <td>" + itemk.AverageCommentCount + "</td>");
                            arrPush.push("               <td>" + itemk.AverageForwardCount + "</td>");
                            arrPush.push("               <td>" + itemk.AveragePointCount + "</td>");
                            arrPush.push("               <td><lable id='editLable_" + itemk.RecID + "'>" + itemk.SortNumber + "</lable>");
                            arrPush.push('                   <input  id="editShowInput_' + itemk.RecID + '"  type="text" style="width: 60px; height: 29px; line-height: 22px; font-size: 12px;display:none" value="' + itemk.SortNumber + '"></td>');
                            arrPush.push('<td><div id=clickEdit_' + itemk.RecID + '>' +
                                '<a href="javascript:$.recommend.ajaxFunction.UpdateBefore(' + itemk.RecID + ');">编辑</a>' +
                                '&nbsp;<a href="javascript:$.recommend.ajaxFunction.ajaxPostDelete(' + itemk.RecID + ');">删除</a>' +
                                '</div>' +
                                '<div style="display:none" id=clickEdit2_' + itemk.RecID + '>' +
                                 '<a href="javascript:$.recommend.ajaxFunction.ajaxPostUpdate(' + itemk.RecID + ');">更新</a>' +
                                  '&nbsp;<a href="javascript:$.recommend.ajaxFunction.UpdateCancel(' + itemk.RecID + ');">取消</a>' +
                                '</div>' +
                                '</td>');
                            arrPush.push("           </tr>");
                        }

                        break;
                    case 14004://视频

                        for (var m = 0; m < count; m++) {
                            var itemm = data[m];
                            arrPush.push('<tr id=tr_' + itemm.RecID + '>');
                            arrPush.push("               <td>");
                            arrPush.push('                  <div class="portrait">');
                            arrPush.push('                       <img src="' + $.recommend.ajaxFunction.funcUitl.urlDomain(itemm.HeadIconURL) + '"></div>');
                            arrPush.push('                   <div class="public">');
                            arrPush.push("                       <h2>" + itemm.Name + "</h2>");
                            arrPush.push("                   </div>");
                            arrPush.push('                   <div class="clear"></div>');
                            arrPush.push("               </td>");
                            arrPush.push("               <td>" + itemm.FansCount + "</td>");
                            arrPush.push("               <td>" + itemm.AverageCommentCount + "</td>");
                            arrPush.push("               <td>" + itemm.AveragePointCount + "</td>");// <video src=""></video>

                            arrPush.push("               <td>  ");
                            arrPush.push("                <div class='video_bf'>" +
                                "<video class='video-js vjs-default-skin vjs-big-play-centered' controls preload='none' width='100px' height='100px' poster='" + $.recommend.ajaxFunction.funcUitl.urlDomain(itemm.ImageUrl) + "' data-setup='{}'>" +
                                "  <source src='" + $.recommend.ajaxFunction.funcUitl.urlDomain(itemm.VideoUrl) + "' type='video/mp4'>" +
                                "  <p class='vjs-no-js'>To view this video please enable JavaScript, and consider upgrading to a web browser that <a href='http://videojs.com/html5-video-support/' target='_blank'>supports HTML5 video</a></p>" +
                                " </video>  " +
                                "<a id='btnEditImage_" + itemm.RecID + "' href='javascript:$.recommend.ajaxFunction.UpdateImageVideo(" + itemm.RecID + ",\"" + encodeURIComponent(itemm.ImageUrl) + "\",\"" + encodeURIComponent(itemm.VideoUrl) + "\",\"" + itemm.Name + "\");'>编辑文件</a>" +
                                "</td>  ");
                            //arrPush.push("               <td><div class='portrait'><img src=\"" + $.recommend.ajaxFunction.funcUitl.urlDomain(itemm.ImageUrl) + "\" /></div>" +
                            //    "<br/><div class='portrait'><video style='width:46px;height=46px' src='" + $.recommend.ajaxFunction.funcUitl.urlDomain(itemm.VideoUrl) + "' controls=\"controls\"></video></div>&nbsp;" +
                            //    "<a id='btnEditImage_" + itemm.RecID + "' style='display:none' href='javascript:$.recommend.ajaxFunction.UpdateImageVideo(" + itemm.RecID + ",\"" + encodeURIComponent(itemm.ImageUrl) + "\",\"" + encodeURIComponent(itemm.VideoUrl) + "\");'>编辑文件</a></td>");
                            arrPush.push("               <td><lable id='editLable_" + itemm.RecID + "'>" + itemm.SortNumber + "</lable>");
                            arrPush.push('                   <input  id="editShowInput_' + itemm.RecID + '"  type="text" style="width: 60px; height: 29px; line-height: 22px; font-size: 12px;display:none" value="' + itemm.SortNumber + '"></td>');
                            arrPush.push('<td><div id=clickEdit_' + itemm.RecID + '>' +
                                '<a href="javascript:$.recommend.ajaxFunction.UpdateBefore(' + itemm.RecID + ');">编辑</a>' +
                                '&nbsp;<a href="javascript:$.recommend.ajaxFunction.ajaxPostDelete(' + itemm.RecID + ');">删除</a>' +
                                '</div>' +
                                '<div style="display:none" id=clickEdit2_' + itemm.RecID + '>' +
                                 '<a href="javascript:$.recommend.ajaxFunction.ajaxPostUpdate(' + itemm.RecID + ');">更新</a>' +
                                  '&nbsp;<a href="javascript:$.recommend.ajaxFunction.UpdateCancel(' + itemm.RecID + ');">取消</a>' +
                                '</div>' +
                                '</td>');
                            arrPush.push("           </tr>");
                        }

                        break;
                    case 14005://直播

                        for (var z = 0; z < count; z++) {
                            var itemz = data[z];
                            arrPush.push('<tr id=tr_' + itemz.RecID + '>');
                            arrPush.push("               <td>");
                            arrPush.push('                  <div class="portrait">');
                            arrPush.push('                       <img src="' + $.recommend.ajaxFunction.funcUitl.urlDomain(itemz.HeadIconURL) + '"></div>');
                            arrPush.push('                   <div class="public">');
                            arrPush.push("                       <h2>" + itemz.Name + "</h2>");
                            arrPush.push("                   </div>");
                            arrPush.push('                   <div class="clear"></div>');
                            arrPush.push("               </td>");
                            arrPush.push("               <td>" + itemz.Number + "</td>");
                            arrPush.push("               <td>" + itemz.Sex + "</td>");
                            arrPush.push("               <td>" + itemz.FansCount + "</td>");
                            arrPush.push("               <td>" + itemz.CumulateReward + "</td>");
                            arrPush.push("               <td><lable id='editLable_" + itemz.RecID + "'>" + itemz.SortNumber + "</lable>");
                            arrPush.push('                   <input  id="editShowInput_' + itemz.RecID + '"  type="text" style="width: 60px; height: 29px; line-height: 22px; font-size: 12px;display:none" value="' + itemz.SortNumber + '"></td>');
                            arrPush.push('<td><div id=clickEdit_' + itemz.RecID + '>' +
                                '<a href="javascript:$.recommend.ajaxFunction.UpdateBefore(' + itemz.RecID + ');">编辑</a>' +
                                '&nbsp;<a href="javascript:$.recommend.ajaxFunction.ajaxPostDelete(' + itemz.RecID + ');">删除</a>' +
                                '</div>' +
                                '<div style="display:none" id=clickEdit2_' + itemz.RecID + '>' +
                                 '<a href="javascript:$.recommend.ajaxFunction.ajaxPostUpdate(' + itemz.RecID + ');">更新</a>' +
                                  '&nbsp;<a href="javascript:$.recommend.ajaxFunction.UpdateCancel(' + itemz.RecID + ');">取消</a>' +
                                '</div>' +
                                '</td>');
                            arrPush.push("           </tr>");
                        }

                        break;
                    default:
                        return "";
                }

                return arrPush.join('');
            }
        }
    });
})(jQuery);