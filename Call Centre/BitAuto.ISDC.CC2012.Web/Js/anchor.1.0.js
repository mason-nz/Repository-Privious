$.fn.zxxAnchor = function (options) {
    var defaults = {
        ieFreshFix: true,
        anchorSmooth: true,
        anchortag: "anchor",
        animateTime: 1000
    };
    var sets = $.extend({}, defaults, options || {});

    if (sets.ieFreshFix) {
        var url = window.location.toString();
        var id = url.split("#")[1];
        if (id && $("#" + id).size() > 0) {
            var t = $("#" + id).offset().top;
            $(window).scrollTop(t);
        }
    }

    $(this).each(function () {
        $(this).mouseup(function () {
            if ($(this).size() > 0 && $(this).attr(sets.anchortag) != undefined) {
                var aim = $(this).attr(sets.anchortag).replace(/#/g, "");
                if ($("#" + aim).size() > 0) {
                    var pos = $("#" + aim).offset().top;
                    if (sets.anchorSmooth) {

                        $("html,body").animate({ scrollTop: pos }, sets.animateTime);
                    } else {
                        $(window).scrollTop(pos);
                    }
                    return false;
                }
            }
        });
    });
};
