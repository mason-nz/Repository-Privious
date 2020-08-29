/*配置js和css文件，只需要修改版本号即可*/
var IncludeFiles = {
    js: [
        { name: 'jquery', src: "/Scripts/jquery-1.7.1.min.js", version: "1.0", isLoading: true },
        { name: 'jmpopups', src: "/Scripts/jquery.jmpopups-0.5.1.pack.js", version: "1.0", isLoading: true },
        { name: 'echarts', src: "/Scripts/echarts.min.js", version: "3.3.2", isLoading: true },
        { name: 'custom', src: "/Scripts/echarts_custom.js", version: "1.0", isLoading: true },

        { name: 'animateNumber', src: "/Scripts/jquery.animateNumber.min.js", version: "1.0", isLoading: false },
        { name: 'swiper', src: "/Scripts/swiper-3.4.0.jquery.min.js", version: "3.4.0", isLoading: false },
        { name: 'swiper.animate', src: "/Scripts/swiper.animate1.0.2.min.js", version: "1.0.2", isLoading: false }
    ],

    css: [
        { name: 'style', src: "/Css/style.css", version: "1.0.5", isLoading: true },
        { name: 'swiperCSS', src: "/Css/swiper-3.4.0.min.css", version: "3.4.0", isLoading: false },
        { name: 'swiper.animate', src: "/Css/animate.min.css", version: "1.0.2", isLoading: false }
    ]
};
/*加载CSS文件*/
for (var i = 0; i < IncludeFiles.css.length; i++) {
    if (IncludeFiles.css[i].isLoading) {
        document.writeln("<link type=\"text/css\" href=\"" + IncludeFiles.css[i].src + "?version=" + IncludeFiles.css[i].version + "\" rel=\"Stylesheet\">");
    }
}
/*加载JS文件*/
for (var j = 0; j < IncludeFiles.js.length; j++) {
    if (IncludeFiles.js[j].isLoading) {
        document.writeln("<script type='text/javascript' src='" + IncludeFiles.js[j].src + "?version=" + IncludeFiles.js[j].version + "'></script>");
    }
}




/*加载文件公告方法*/
function LoadJS(key) {
    for (var i = 0; i < IncludeFiles.js.length; i++) {
        if (IncludeFiles.js[i].name == key) {
            document.writeln("<script type='text/javascript' src='" + IncludeFiles.js[i].src + "?version=" + IncludeFiles.js[i].version + "'></script>");
            break;        
        }
    }
}
/*加载文件公告方法*/
function LoadCSS(key) {
    for (var i = 0; i < IncludeFiles.css.length; i++) {
        if (IncludeFiles.css[i].name == key) {
            document.writeln("<link type=\"text/css\" href=\"" + IncludeFiles.css[i].src + "?version=" + IncludeFiles.css[i].version + "\" rel=\"Stylesheet\">");
            break;
        }
    }
}