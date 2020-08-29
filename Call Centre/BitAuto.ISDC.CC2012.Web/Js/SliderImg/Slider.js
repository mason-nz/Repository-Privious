/**
* lxm 2016-07-28
实现功能：点击页面小图，显示大图，支持左右切换，鼠标悬停

*/
; $(function ($, window, document, undefined) {

    SliderImg = function (options) {
        /*
        options = {
        auto: true,
        time: 3000,
        event: 'hover' | 'click',
        mode: 'slide | fade',
        controller: $(),
        activeControllerCls: 'className',
        exchangeEnd: $.noop
        }
        */

        "use strict"; //stirct mode not support by IE9-

        var divcover = "div_cover"; //遮盖层
        var divloading = "div_loading"; //加载中
        var options = options || {};

        createDiv(options.imgs); //创建元素

        var container = $("#" + options.containeriId);
        if (!container) return;

        var divcover = "div_cover"; //遮盖层
        var currentIndex = options.currentIndex,
            cls = options.activeControllerCls,
            delay = options.delay,
            isAuto = options.auto,
            controller = $("#" + options.banController),
            event = options.event,
            interval,
            slidesWrapper = container.children().first(),
            slides = slidesWrapper.children(),
            length = slides.length,
            childWidth = container.width(),
            totalWidth = childWidth * slides.length,
            imgs = options.imgs, //
            containeriId = options.containeriId, //
            imgController = options.imgController, //
            banController = options.banController;

        //prev frame
        this.prev = function () {
            prev();
        }

        //next frame
        this.next = function () {
            next();
        }

        this.close = function () {
            close();

        }

        var prev = function () {
            stop();

            currentIndex == 0 ? triggerPlay(length - 2) : triggerPlay(currentIndex - 2);

            isAuto && autoPlay();
        }

        //next frame
        var next = function () {
            stop();

            currentIndex == length - 1 ? triggerPlay(-1) : triggerPlay(currentIndex);

            isAuto && autoPlay();
        }

        var close = function () {
            container.remove();
            $("#" + divcover).remove();

            if (options.afterClose) {
                options.afterClose();
            }
        }

        function init() {
            var controlItem = controller.children();

            mode();

            event == 'hover' ? controlItem.mouseover(function () {
                stop();
                var index = $(this).index();

                play(index, options.mode);
            }).mouseout(function () {
                isAuto && autoPlay();
            }) : controlItem.click(function () {
                stop();
                var index = $(this).index();

                play(index, options.mode);
                isAuto && autoPlay();
            });

            isAuto && autoPlay();
        }

        //animate mode
        function mode() {

            var wrapper = container.children().first();

            options.mode == 'slide' ? wrapper.width(totalWidth) : wrapper.children().css({
                //'position': 'absolute',
                //'left': 0,
                //'top': 0
            })
                .eq(currentIndex).siblings().hide();
        }

        //auto play
        function autoPlay() {
            interval = setInterval(function () {
                triggerPlay(currentIndex);
            }, options.time);
        }

        //trigger play
        function triggerPlay(cIndex) {
            var index;

            (cIndex == length - 1) ? index = 0 : index = cIndex + 1;
            play(index, options.mode);
        }

        //play
        function play(index, mode) {

            slidesWrapper.stop(true, true);
            slides.stop(true, true);

            mode == 'slide' ? (function () {
                if (index > currentIndex) {
                    slidesWrapper.animate({
                        left: '-=' + Math.abs(index - currentIndex) * childWidth + 'px'
                    }, delay);
                } else if (index < currentIndex) {
                    slidesWrapper.animate({
                        left: '+=' + Math.abs(index - currentIndex) * childWidth + 'px'
                    }, delay);
                } else {
                    return;
                }
            })() : (function () {
                if (slidesWrapper.children(':visible').index() == index) return;
                slidesWrapper.children().fadeOut(delay).eq(index).fadeIn(delay);
            })();

            try {
                controller.children('.' + cls).removeClass(cls);
                controller.children().eq(index).addClass(cls);
            } catch (e) { }

            currentIndex = index;

            options.exchangeEnd && typeof options.exchangeEnd == 'function' && options.exchangeEnd.call(this, currentIndex);
        }

        //stop
        function stop() {
            clearInterval(interval);
        }

        function createDiv(imgs) {

            var html_img = "";
            var html_nav = "";
            var url = "";

            var windowHeight = $(window).height() - 3;
            var windowWidth = $(window).width() - 3;

            for (var i = 0; i < imgs.length; i++) {
                url = imgs[i].url;
                //style=\" width:" + width + "px; height:" + height + "px \"
                //html_img += " <li><img alt=\"\"   src=\"" + url + "\" style='display:none;' /></li>";
                //html_img += " <li><div style='background:url(" + url + ") no-repeat center;top:0px;left:0px; width:" + windowWidth + "px;height:" + windowHeight + "px; '></div></li>";
                html_img += " <li><img style='clear:both;' alt=\"\"   src=\"" + url + "\"  /></li>";
                html_nav += "  <li><a>" + i + "</a></li>";
            }

            var html = "";
            html += "<div id='" + divcover + "' class='coverlayer' style=\"top:0px;left:0px; width:" + windowWidth + "px;height:" + windowHeight + "px; \" ></div>";
            html += "<div id='" + divloading + "' class='loading' style=\"top:0px;left:0px; width:" + windowWidth + "px;height:" + windowHeight + "px; \" ></div>";
            html += "<div id=\"banner_tabs\" class=\"flexslider\" style=\" top:0px; width:" + windowWidth + "px;height:" + windowHeight + "px; \" >";
            html += " <ul id='slides' class=\"slides\" >" + html_img + "</ul>";
            html += " <ul class=\"flex-direction-nav\">";
            html += "   <li><a class=\"flex-prev\" href=\"javascript:;\">Previous</a></li>";
            html += "   <li><a class=\"flex-next\" href=\"javascript:;\">Next</a></li>";
            html += "   </ul>";
            html += " <ol id=\"bannerCtrl\" class=\"flex-control-nav flex-control-paging\">" + html_nav + "</ol>";
            html += " <div class=\"close\"><a /></div>";
            html += "</div>";

            $(document.body).append(html);

        }

        function autoHeight(containeriId) {
            var windowHeight = $(window).height() - 3;
            var windowWidth = $(window).width() - 3;

            var width = 0;
            var height = 0;
            var top = 0;
            $("#" + containeriId + " img").each(function () {
                width = this.width;
                if (width > windowWidth - 20) {
                    width = windowWidth - 20;
                }
                height = this.height;
                if (height > windowHeight - 20) {
                    height = windowHeight - 20;
                }
                top = ($(this).parent().height() - height) / 2

                $(this).css({ "width": width, "height": height, "top": top, "display": "inline-block" });
            });
        }
        function autoHeight_one(img) {

            var windowHeight = $(window).height() - 3;
            var windowWidth = $(window).width() - 3;

            var width = 0;
            var height = 0;
            var top = 0;

            width = img.width;
            if (width > windowWidth - 20) {
                width = windowWidth - 20;
            }
            height = img.height;
            if (height > windowHeight - 20) {
                height = windowHeight - 20;
            }
            top = ($(img).parent().height() - height) / 2; //居中

            $(img).css({ "width": width, "height": height, "top": top, "display": "inline-block" });


        }
        var t_img = 0; // 定时器
        //判断图片是否加载完成，cid容器，callback回调
        function isImgLoad(cid, callback) {

            var isLoad = true; // 控制变量
            // 注意我的图片类名都是cover，因为我只需要处理cover。其它图片可以不管。
            // 查找所有封面图，迭代处理
            var n = 0;
            $("#" + cid + " img").each(function () {

                n++;
                // 找到为0就将isLoad设为false，并退出each
                if (this.height == 0) {
                    isLoad = false;
                }
                else {
                    //调整高度
                    var resize = $(this).attr("resize"); //是否调整过
                    if (resize == undefined || resize == null) {
                        resize = "1";
                    }
                    if (resize != "0") {

                        if (this.height > 50 || resize == "3" || t_img == 10) {
                            autoHeight_one(this); //自适应
                            $(this).attr("resize", "0");
                        }
                        else {
                            resize = parseInt(resize) + 1;
                            $(this).attr("resize", resize);
                            isLoad = false;
                        }
                    }
                }
            });
            // 为true，没有发现为0的。加载完毕
            if (isLoad || t_img > 10) {

                clearTimeout(t_img); // 清除定时器
                // 回调函数
                if (callback != null) {
                    callback();
                }
                // 为false，因为找到了没有加载完成的图，将调用定时器递归
            } else {
                t_img = setTimeout(function () {
                    isImgLoad(cid, callback); // 递归扫描
                }, 500); // 500毫秒就扫描一次
            }
        }

        function addEvent() {
            $("#" + options.containeriId + " .flex-prev").click(function () {
                prev();
            });
            $("#" + options.containeriId + " .flex-next").click(function () {
                next();
            });
            $("#" + options.containeriId + " .close").click(function () {
                close();
            });

        }

        //事件
        addEvent();

        //调整图片
        setTimeout(function () {
            $("#" + divloading).remove(); isImgLoad(options.imgController, function () { })
        }, 500);



        //init
        init();


        //默认显示
        // play(options.currentIndex, options.mode);
    };

} (jQuery, window, document));


//绑定Slider      例如：  BindSlider(".bzh2 img","src");
function BindSlider(selct, att) {

    var att = att || "src";

    $(selct).click(function () {
        var imgs = [];
        //获取所有图片
        $(selct).each(function () {
            imgs.push({ url: $(this).attr(att) });
        });
        //索引
        var index = $(this).index();

        //
        var bannerSlider = new SliderImg({
            time: 5000,
            delay: 400,
            event: 'hover',
            auto: false,
            mode: 'fade',
            currentIndex: index,
            activeControllerCls: 'active',
            imgs: imgs,
            containeriId: "banner_tabs",
            imgController: "slides",
            banController: "bannerCtrl",
            afterClose: null

        });
    });


}



