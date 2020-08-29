/// <reference path="../jquery.1.11.3.min.js" />
/// <reference path="../Common_chitu.js" />

//Auth:lixiong
//Date:2017-04-12
//Description:

(function ($) {
    $.recommend = {};
    $.extend($.recommend, {
        bussinesstype: 14001,
        categoryId: 20005,
        ajaxSearchUrl: '/api/recommend/query',
        ajaxPostDeleteUrl: "/api/recommend/delete",
        ajaxPostUpdateUrl: "/api/recommend/update",
        ajaxPostPublishUrl: "/api/recommend/Publish",
        ajaxSearchData: {
            pageIndex: 1,
            pageSize: 20,
            bussinesstype: 0,
            categoryId: -1,
            mediaName: ""
        },
        ajaxPostPublishData: {
            categoryId: -1,
            bussinesstype: 0
        },
        ajaxPostUpdateData: {
            Id: 0,
            SortNumber: 0,
            ImageUrl: "",
            VideoUrl: ""
        },
        init: function (options) {

            //初始化参数
            $.recommend.ajaxSearchData.categoryId = 20005;
            $.recommend.ajaxSearchData.bussinesstype = 14001;

            $.extend($.moduleApply, options);

            $.recommend.ajaxFunction.ajaxSearchRecommendList();

        },
        ajaxFunction: {
            ajaxSearchRecommendList: function () {
                //查询列表

                //var set = {
                //    url:"/api/Publish/query",
                //    data:{
                //        pageIndex:1,
                //        pageSize : 20,
                //        bussinesstype:14001,
                //        CategoryID:-1,
                //        FansCount:'',
                //        Price:'',
                //        CoverageArea:'',
                //        OrderRemark:-1,
                //        IsAuth:-1,
                //        LevelType:-1,
                //        FansSex:-2,
                //        orderBy:-1,
                //        OrderByReference:-1
                //    }
                //}

                setAjax({
                    url: $.recommend.ajaxSearchUrl,
                    type: 'get',
                    data: $.recommend.ajaxSearchData,
                    function(data) {
                        //TODO  :处理逻辑
                        console.log("success", data);
                    }
                });
                //$.ajax({
                //    type: "GET",
                //    url: $.recommend.ajaxSearchUrl,
                //    data: $.recommend.ajaxSearchData,
                //    dataType: "json",
                //    success: function (data) {
                //        //TODO  :处理逻辑
                //        console.log("success", data);
                //    },
                //    error: function (msg) {
                //        console.log(msg);
                //    }
                //});
            }
            , ajaxPostPublish: function () {
                //发布操操作

                $.recommend.ajaxPostPublishData.bussinesstype = $.recommend.bussinesstype;
                $.recommend.ajaxPostPublishData.categoryId = 2005;

                $.ajax({
                    type: "POST",
                    url: $.recommend.ajaxPostPublishUrl,
                    data: $.recommend.ajaxPostPublishData,
                    dataType: "json",
                    success: function (data) {
                        //TODO  :处理逻辑
                        console.log("success", data);
                    },
                    error: function (msg) {
                        console.log(msg);
                    }
                });
            }
            , ajaxPostDelete: function (selectRecId) {
                //删除
                $.ajax({
                    type: "POST",
                    url: $.recommend.ajaxPostDeleteUrl,
                    data: { recId: selectRecId },
                    dataType: "json",
                    success: function (data) {
                        //TODO  :处理逻辑
                        console.log("success", data);
                    },
                    error: function (msg) {
                        console.log(msg);
                    }
                });
            }
            , ajaxPostUpdate: function () {
                //修改
                $.ajax({
                    type: "POST",
                    url: $.recommend.ajaxPostUpdateUrl,
                    data: ajaxPostUpdateData,
                    dataType: "json",
                    success: function (data) {
                        //TODO  :处理逻辑
                        console.log("success", data);
                    },
                    error: function (msg) {
                        console.log(msg);
                    }
                });
            }
        }
    });
})(jQuery);

$(document).ready(function () {
    $.recommend.init();
});
