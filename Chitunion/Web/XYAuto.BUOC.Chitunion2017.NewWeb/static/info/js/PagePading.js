(function ($) {
    //分页
    $.fn.NewPage = function (options) {
        var setting = {
            pageDIV: "#ucPager",
            TotalPages: 0,
            TotalRecords: 0,
            PageSize: 0,
            handlefun: "",
            argument: ""
        }
        if (options) {
            $.extend(setting, options);
        };
        var PageIndex = 1;
        var mTotalPages = 0;
        var mTotalRecords = 0;
        function init() {
            var strs = "";
            CalculateTotalPages();
            strs += RenderPrevious();
            strs += RenderMiddleRange();
            strs += RenderNext();
            mTotalRecords = parseInt(setting.TotalRecords);
            //var pagestr = "<p class=\"page\"> "
            var pagestr = '';
            if (mTotalPages > 1) {


                pagestr += '<p class="num-info left">总数<em>' + mTotalRecords + '</em>条，共<em>' + mTotalPages + '</em>页</p>';

                pagestr += '<p class="right">' + strs + '<input class="page-inp mr4" type="text"><a class="page-go btngoPage downpage" href="javascript:;">go</a></p>';

                //pagestr += strs + " <input class='page-inp' type='text' /> <a class=\"downpage btngoPage ml5\" href=\"javascript:;\" style=\"margin-right: 5px;\">go</a> </p><p class=\"num-info\">总数 " + mTotalRecords + " 条</p><p class=\"num-info ml10\">共&nbsp;" + mTotalPages + "&nbsp;页</p>";
            }
            // alert(pagestr);
            $(setting.pageDIV).html(pagestr);
        }
        function CalculateTotalPages() {
            if (setting.TotalPages == 0) {
                if (setting.TotalRecords == 0) {
                    mTotalPages = 0;
                }
                mTotalPages = parseInt(setting.TotalRecords / setting.PageSize);
                if ((setting.TotalRecords % setting.PageSize) > 0) {
                    mTotalPages++;
                }
            } else {
                mTotalPages = setting.TotalPages;
            }
        }
        function RenderPrevious() {
            var str = "";
            if (mTotalPages > 1) {

                if (PageIndex == 1) {
                    str += "  <a  class=\"uppage\" style=\"margin-right: 5px;\" data='-1'> 上一页 </a> "
                } else {
                    str += " <a  class=\"uppage\" style=\"margin-right: 5px;\" data='" + (parseInt(PageIndex) - 1) + "'> 上一页</a> "
                }
            }
            return str;
        }
        function RenderNext() {
            var str = "";
            if (mTotalPages > 1) {
                if (PageIndex != mTotalPages) {
                    str += "  <a class=\"downpage\" style=\"margin-right: 5px;\" data=\"" + (parseInt(PageIndex) + 1) + "\">下一页</a> ";
                } else {
                    str += "  <a class=\"downpage\" style=\"margin-right: 5px;\" data=\"-1\">下一页</a> "
                }
            }

            return str;
        }

        function RenderMiddleRange() {
            var str = "";
            var lastIndex = mTotalPages - 1;
            if (mTotalPages > 1) {
                if (mTotalPages > 3) {
                    if (PageIndex < 3) {
                        for (var i = 1; i < 4; i++) {
                            if (i == PageIndex) {
                                str += " <a class='selected' data=\"" + i + "\"> " + i + " </a> "
                            } else {
                                str += " <a  href='javascript:void(0);' data=\"" + i + "\">" + i + "</a> "
                            }
                        }
                        if (mTotalPages > 4) {
                            str += " <a href='javascript:void(0);' data=\"-1\">...</a> ";
                        }
                        str += " <a class=\"\" href='javascript:void(0);' data=\"" + mTotalPages + "\">" + mTotalPages + "</a> ";
                    } else if (PageIndex > parseInt(mTotalPages) - 2 && PageIndex <= mTotalPages) {
                        str += " <a class=\"\" href='javascript:void(0);' data=\"1\">1</a> ";
                        if (mTotalPages > 4) {
                            str += " <a href='javascript:void(0);' data=\"-1\">...</a> ";
                        }
                        for (var j = parseInt(mTotalPages) - 2; j <= parseInt(mTotalPages) ; j++) {
                            if (j == PageIndex) {
                                str += " <a  href='javascript:void(0);' class='selected'  data=\"" + j + "\">" + j + "</a> "
                            } else {
                                str += " <a  href='javascript:void(0);' data=\"" + j + "\">" + j + "</a> "
                            }
                        }
                    } else {
                        str += " <a href='javascript:void(0);' data=\"-1\">...</a> ";
                        for (var k = parseInt(PageIndex) - 1; k < parseInt(PageIndex) + 2; k++) {
                            if (k == PageIndex) {
                                str += " <a class='selected' href='javascript:void(0);' data=\"" + k + "\">" + k + "</a> ";
                            }
                            else {
                                str += " <a class=\"\" href='javascript:void(0);' data=\"" + k + "\">" + k + "</a> ";
                            }
                        }
                        str += " <a href='javascript:void(0);' data=\"-1\">...</a> ";

                    }

                } else if (mTotalPages > 0) {
                    for (var m = 1; m <= parseInt(mTotalPages) ; m++) {
                        if (m == PageIndex) {
                            str += " <a  href='javascript:void(0);' class='selected' data=\"" + m + "\">" + m + "</a> "
                        } else {
                            str += " <a  href='javascript:void(0);' data=\"" + m + "\">" + m + "</a> "
                        }
                    }
                }
            }
            return str;
        }

        function pageClick() {

            $(setting.pageDIV).find("a").click(function () {

                var currIndex = 0;
                if ($(this).hasClass("btngoPage")) {

                    currIndex = Number($(setting.pageDIV).find(".page-inp").val());

                    if (currIndex > mTotalPages) {
                        currIndex = mTotalPages;
                    }
                } else {
                    currIndex = $(this).attr("data");
                }
                if (currIndex > 0) {
                    PageIndex = currIndex;
                }
                eval(setting.handlefun)(PageIndex, setting.PageSize, setting.argument);
                init();
                pageClick();
            });

        }
        init();
        pageClick();
    };

    $.SelectUl = function (options) {
        var opt = { cid: 0, timeout: 100, "showid": "", "hideid": "", over: function () { }, out: function () { } };
        if (options) { $.extend(opt, options) }
        $(opt.showid).mouseover(function () {
            if (opt.cid > 0) { clearTimeout(opt.cid) }; $(opt.hideid).show(); opt.over()
        }).mouseout(function () { opt.cid = setTimeout(function () { $(opt.hideid).hide(); opt.out() }, opt.timeout) });
        $(opt.hideid).mouseover(function () {
            if (opt.cid > 0) { clearTimeout(opt.cid) } var $this = $(this); $this.show(); opt.over(); return false
        }).mouseout(function () { var $this = $(this); opt.cid = setTimeout(function () { $this.hide(); opt.out() }, opt.timeout); return false })
    };

    $.SelectClick = function (options) {
        var opt = { "showid": "", "hideid": "", click: function () { } };
        if (options) {
            $.extend(opt, options);
        }
        $(opt.showid).click(function () {
            $(opt.hideid).show();
            opt.click();
            return false;
        })
    }

})(jQuery);
