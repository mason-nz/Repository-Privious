$(function () {

    $('#pc_content_body').html('<div id="top">\n' +
        '        <div class="box">\n' +
        '            <div class="left"></div>\n' +
        '            <ul class="right pr">\n' +
        '                <li>\n' +
        '                    <a href="javascript:void(0)">\n' +
        '                        <img src="data:image/png;base64,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">\n' +
        '                        <span>广告主</span>\n' +
        '                    </a>\n' +
        '                </li>\n' +
        '                <li>\n' +
        '                    <img src="data:image/png;base64,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">\n' +
        '                </li>\n' +
        '                <li>\n' +
        '                    <a href="javascript:void(0)">\n' +
        '                        <img src="data:image/png;base64,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">\n' +
        '                        <span>媒体主</span>\n' +
        '                    </a>\n' +
        '                </li>\n' +
        '            </ul>\n' +
        '            <div class="clear"></div>\n' +
        '        </div>\n' +
        '    </div>\n' +
        '    <div class="topBar">\n' +
        '        <div class="topBox">\n' +
        '            <a href="#" class="topLogo"></a>\n' +
        '            <ul id="topBoxlist">\n' +
        '                <li><a href="#">首页</a></li>\n' +
        '                <li><a href="#" class="active">智能匹配</a></li>\n' +
        '                <li><a href="#">内容分发</a></li>\n' +
        '                <li><a href="#">媒体智投</a></li>\n' +
        '                <li><a href="#">媒体变现</a></li>\n' +
        '                <li><a href="#">客户服务</a></li>\n' +
        '                <div class="clear"></div>\n' +
        '            </ul>\n' +
        '            <div class="clear"></div>\n' +
        '        </div>\n' +
        '    </div>\n' +
        '    <div class="clear"></div>')
    var public_url='http://op1.chitunion.com'
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

    //    点击查看全部
    $('.pc_content>p>span').off('click').on('click', function () {
        $(this).parent().hide();
        $('.pc_content>div').css('max-height', '100%')
    })
    //    计算腰部图片
    var img_w = $('.pc_Waist_a>img').width();
    $('.pc_Waist_a>img').each(function () {
        $(this).css('height', img_w * 2 / 3 + 'px')
        $(this).next().css('height', img_w * 2 / 3 + 'px')
    })

    // 点赞
    $.ajax({
        url: public_url+'/api/ApiNL/GetLikeCount',
        type: 'get',
        dataType: 'json',
        data: {
            MaterieUrl: window.location.href
        },
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: function (data) {
            if (data.Status == 0) {
                if(data.Result.ClickType==1){
                    $('.pc_Fabulous img').eq(0).attr('src', 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAYAAAGWB6gOAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyNpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTQwIDc5LjE2MDQ1MSwgMjAxNy8wNS8wNi0wMTowODoyMSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIChNYWNpbnRvc2gpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjJCOUVCQTdERDk2MDExRTc5QzFGODRDOERFNzc3ODlFIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjJCOUVCQTdFRDk2MDExRTc5QzFGODRDOERFNzc3ODlFIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MkI5RUJBN0JEOTYwMTFFNzlDMUY4NEM4REU3Nzc4OUUiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MkI5RUJBN0NEOTYwMTFFNzlDMUY4NEM4REU3Nzc4OUUiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7P5q3sAAAEbUlEQVR42mL8//8/AzpgAhGcgfMZoDQXiM0IVQlXDhRkZGLAAlhgssiCAAHEiG4RE8hQqCX/4bZ+X58IU/YfxXCgSkkMgwECCGwozKFQ7SAKJJAI8wQ2N4LsSPj1558OkOaHuxnNAWDAxsJ0+c/ffx+ATEEmJMfoALHavovPGn78/vvi1YfvB3lDFgqB5AACiBFbiOIKInSHrwfiQJRoQAZABy8HUgEYcQUDX9YmJAIdHHHn+acJQG48wr9ANwHxf46AeWz/0QBQjAskj2wSBxY3/8HqJmQA9MQv5PSjDKR+A2nTlx++7//3//93IFsRZ9xhMY0BIIDggYlLITTccAKQPpAaJgYiADRpgWz8D2Rj1UPQoK8//kgAbfwHCXYGUICwYVUIjRMGYPiD6APQuGD49+//pf8EAFAdI1QfhovsoTQvIyODLhG+5iLkNUZCJlx//KEV6OWvWA36++//RyTuP1yGfPnx+4pR3vomjGwCAzzBC0SBFDMQ/wDGDihQOYGYG0qzQi3+DsTPga75jdMgUKqEYrADQZZDMdb0gwwAAoio3E0MYMFlA01SNtTCr9CwYiArZYOj7///M6A0AywS95FtELjmYGTUB7E/fv11l2yDvq1LvANjp0850kIwsHEE9FfkbKAowQdKsB5QB2zDl2n/A8PBF0iJEMqwwEx9D0kfiovAMfLl+29nRkZGIz4uVrxe/vPvHwsrMxPWMGKFVhIsQMPYCYXd/RefF5Ic/diAfs66aVQxCD0PkmXQmduvG4DUT7wG/fz99+Prj9+v4zPIrWb7HFiFhi0d/QWmYiMg/QxalBimeWgoZHprBsiIcFtwsrHIgCq8aVuu5X3/9fcDvgT5DYhvgCIOWjp+m7XjxkMgBuUzdqhakPgb5CIWm0H/oaUfDPyA4veECjVKYw0FAAQY1UpIagEWYrxPoICgyAEw+2DmMFHTd8DqNRnWZgFhYKprJNUMqjnow5dfNsxMjHPQGq3u0Oqavg76/P23pQAP20F08Yv33s4nVCdQ1UHAeNcB5onbvJysx9DN+vjt10mHiq3rofmf/ESNZBkfkIoC4jvAYN+DJM4DpGYAcTQ4m+Jo1fFxsukD1b7EUTiHY9SIBEII1FwDFX/TgXj3x9XxnSC7P62JLwTFEMwxeMsTRqy9EXCLEIg3AD3JSoqDuJBbtJ++/QLxud5//ilMjTT3/svPPdBim2gHMaK2L8B85j9//1MlE8zbdasF2salXbYnIUeerll85gZ6/TwgDgL2N667126PA1WSsP7agDpo2+nHVefvvn0KrbVJKof+H776Iu/bzz/3gAnwcP3Ss1NBYsopKydP2nQ17OGrL+uBLZ6XxDoE2Kb9XD7/lEt838FDoPYIrtDBWttDKztW6MAKN3Tg4CO0bcICFeOGDjKwQosIZqjnGJEwrE3zB9pzeQPKsEDH/MVXueIqGH9DDXiDXn9CE+M7pBCGOYYJyVGwnApz0B+o3v+EWhMsFCaLf/g6zcOigQYAHwBMLjwmBb0AAAAASUVORK5CYII=')
                        .parent().attr('class', 'gradrent')
                        .siblings('hr').css('border-top', '1px solid #fff')
                }else if(data.Result.ClickType==2){
                    $('.pc_Fabulous img').eq(0).attr('src', 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAYAAAGWB6gOAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyNpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTQwIDc5LjE2MDQ1MSwgMjAxNy8wNS8wNi0wMTowODoyMSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIChNYWNpbnRvc2gpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjJCOUVCQTc5RDk2MDExRTc5QzFGODRDOERFNzc3ODlFIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjJCOUVCQTdBRDk2MDExRTc5QzFGODRDOERFNzc3ODlFIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MkI5RUJBNzdEOTYwMTFFNzlDMUY4NEM4REU3Nzc4OUUiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MkI5RUJBNzhEOTYwMTFFNzlDMUY4NEM4REU3Nzc4OUUiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz47Zb/UAAAEqklEQVR42mL8//8/AzpggdJwGc7A+YxMDFgAC0wWSAkB8Xtk7SDwDsYACCBGdIuYgNpglvzHsOD7+sR/TEiGg41gQXYPiAYIILChUHNg2qKBVCsQnwfiQJAcC5Ikul/l//3//xZICyMbrQakfgMV3wfxm5afjzx89cUzsJORdN4G4gcgJzx7+21L+6oLm3a3eB4CSQAEECO2EMUVbsgOXwOknKDhCA1/oElA/J8jYB7nf0wAlkd2Eysu65jwuQXoDDa4IiBHDkj9OHj5eTOIf+n+u+n7Lz1vgrkZPcQZ788L3yIhyOUFYuOy7r9i0srwN59+HEAWxIg7dAAMEgaAAIIHJi6FIEUEPAdRAwsvYFiC6LfQcAWx//7HAf79+38KTR+G32CxxYcvmBkZGUx//v5bQHSc4APsrMz9OA36/P33aViof//19zYhw4Dhw42RRaGBzQSN079AzAzEYmG2SjoLi+x3QdImwy+uoPmS0ITxA2Q32FJgYLOgWfIPiQ0y7Pmqw/deALECUPEDYNiAkt83IPsHcqyhlyS4oh9UdjwCYmmoi39hjQBC6YgYAPIaQAARlbuJASzofiU3ZTMRcLICtDw8By2NnwKxIV4XgcILVP7++fuvkpGR8TIzE+MWLOoFoYYyIdcU6AmSC0R8+vZb6sOXn+b4XPrhy69MfMUWMzjx/PvP9PP3P7zJQoCHbSpJ5R8xZSPFBr1YGk2d3M/DwZqN16BHr7+cmbzl6jJCBgHDkhFX9P8G+lsH5GpQfpqw4YrB13UJJ5gYGTlgCkwLNjiUBOkZAJMGY2zvgU3IiRXZoO9AfBOa60Fp5DZ30ALD96vidnOwMsuAFBgqizxM6D94DSr/iZRMC6ruQEXILRDn288/87jYWZJJziIg74Kyxdcff25AympGPbLqUZiX9XLW+gObFT/8mnbFEcz9BAq2J8DwAkXES5w1C7UKNiYGKgGAAKNaCUktwIKt5CQ1eCgBMPtg5rDgsSgASIEaXqC2ZyxQ40eouD6QApWPHqBGK5H2vv/x+68hMKM9JKQQV9yDMth6IAalQF9g/qgDlcPA3FsDpC8AcToJjgHXGEDHPABmVFNyHcSBzAEaxA6qYYA1Cycl0cPKwjR7z4Wn3OQ4iBGb2L9/WMWJdxAzk76DruQOYPQzUVISUTcXMTPZHOjwVhs0DgIBPUXhdlyhhM9B/5F6WP+hleEPajgImMB9QE10Uhz037dxl+nxG6861h17kC6fuKId5B61tFUzHCq2mj9/920HIUuBlc1PHOLfDlx+ngNrPREsqaEFFUgxJzS3gZruX6GtamZoe04A1N2pjTTULgzQ7eZkY5bF0pY7KRm7NBkpkzBCQx0Uyh+A+COwTPuNXjDichDBKgeIQc00HlAZMynD0jLVXWMBeoi/+vA9Ehi6q5DEQZb9Qx4VILqkJgBABv6E4k95M45/5GRjiYlxVFmG2lBlzwdSa4CW/SHWYIpzGSjYQVVD6qTDu8/eeVOHLPf64/eT0JAkv3Il01F/gEH/yaZ08ywgdx007YFCDzQ484cUs8hNQ/hCnA2a+P9C24p/SWk1sFC5zPsHzUXDp4EGAI6JNBF1D/kIAAAAAElFTkSuQmCC')
                        .parent().attr('class', 'gradrent')
                        .siblings('hr').css('border-top', '1px solid #fff')
                }
                // 点赞功能
                var judge = true;
                if (data.Result.ClickType != 0) {
                    judge = false;
                }
                $('.pc_Fabulous img').off('click').on('click', function () {
                    if (judge) {
                        if(M_operation($(this).attr('i'))) {
                            $(this).parent().attr('class', 'gradrent');
                            $(this).parent().siblings('hr').css('border-top', '1px solid #fff');
                            if ($(this).attr('i') == 1) {
                                $(this).attr('src', 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAYAAAGWB6gOAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyNpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTQwIDc5LjE2MDQ1MSwgMjAxNy8wNS8wNi0wMTowODoyMSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIChNYWNpbnRvc2gpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjJCOUVCQTdERDk2MDExRTc5QzFGODRDOERFNzc3ODlFIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjJCOUVCQTdFRDk2MDExRTc5QzFGODRDOERFNzc3ODlFIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MkI5RUJBN0JEOTYwMTFFNzlDMUY4NEM4REU3Nzc4OUUiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MkI5RUJBN0NEOTYwMTFFNzlDMUY4NEM4REU3Nzc4OUUiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7P5q3sAAAEbUlEQVR42mL8//8/AzpgAhGcgfMZoDQXiM0IVQlXDhRkZGLAAlhgssiCAAHEiG4RE8hQqCX/4bZ+X58IU/YfxXCgSkkMgwECCGwozKFQ7SAKJJAI8wQ2N4LsSPj1558OkOaHuxnNAWDAxsJ0+c/ffx+ATEEmJMfoALHavovPGn78/vvi1YfvB3lDFgqB5AACiBFbiOIKInSHrwfiQJRoQAZABy8HUgEYcQUDX9YmJAIdHHHn+acJQG48wr9ANwHxf46AeWz/0QBQjAskj2wSBxY3/8HqJmQA9MQv5PSjDKR+A2nTlx++7//3//93IFsRZ9xhMY0BIIDggYlLITTccAKQPpAaJgYiADRpgWz8D2Rj1UPQoK8//kgAbfwHCXYGUICwYVUIjRMGYPiD6APQuGD49+//pf8EAFAdI1QfhovsoTQvIyODLhG+5iLkNUZCJlx//KEV6OWvWA36++//RyTuP1yGfPnx+4pR3vomjGwCAzzBC0SBFDMQ/wDGDihQOYGYG0qzQi3+DsTPga75jdMgUKqEYrADQZZDMdb0gwwAAoio3E0MYMFlA01SNtTCr9CwYiArZYOj7///M6A0AywS95FtELjmYGTUB7E/fv11l2yDvq1LvANjp0850kIwsHEE9FfkbKAowQdKsB5QB2zDl2n/A8PBF0iJEMqwwEx9D0kfiovAMfLl+29nRkZGIz4uVrxe/vPvHwsrMxPWMGKFVhIsQMPYCYXd/RefF5Ic/diAfs66aVQxCD0PkmXQmduvG4DUT7wG/fz99+Prj9+v4zPIrWb7HFiFhi0d/QWmYiMg/QxalBimeWgoZHprBsiIcFtwsrHIgCq8aVuu5X3/9fcDvgT5DYhvgCIOWjp+m7XjxkMgBuUzdqhakPgb5CIWm0H/oaUfDPyA4veECjVKYw0FAAQY1UpIagEWYrxPoICgyAEw+2DmMFHTd8DqNRnWZgFhYKprJNUMqjnow5dfNsxMjHPQGq3u0Oqavg76/P23pQAP20F08Yv33s4nVCdQ1UHAeNcB5onbvJysx9DN+vjt10mHiq3rofmf/ESNZBkfkIoC4jvAYN+DJM4DpGYAcTQ4m+Jo1fFxsukD1b7EUTiHY9SIBEII1FwDFX/TgXj3x9XxnSC7P62JLwTFEMwxeMsTRqy9EXCLEIg3AD3JSoqDuJBbtJ++/QLxud5//ilMjTT3/svPPdBim2gHMaK2L8B85j9//1MlE8zbdasF2salXbYnIUeerll85gZ6/TwgDgL2N667126PA1WSsP7agDpo2+nHVefvvn0KrbVJKof+H776Iu/bzz/3gAnwcP3Ss1NBYsopKydP2nQ17OGrL+uBLZ6XxDoE2Kb9XD7/lEt838FDoPYIrtDBWttDKztW6MAKN3Tg4CO0bcICFeOGDjKwQosIZqjnGJEwrE3zB9pzeQPKsEDH/MVXueIqGH9DDXiDXn9CE+M7pBCGOYYJyVGwnApz0B+o3v+EWhMsFCaLf/g6zcOigQYAHwBMLjwmBb0AAAAASUVORK5CYII=')
                            } else {
                                M_operation($(this).attr('i'))
                                $(this).attr('src', 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACQAAAAkCAYAAAGWB6gOAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyNpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMTQwIDc5LjE2MDQ1MSwgMjAxNy8wNS8wNi0wMTowODoyMSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIChNYWNpbnRvc2gpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjJCOUVCQTc5RDk2MDExRTc5QzFGODRDOERFNzc3ODlFIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjJCOUVCQTdBRDk2MDExRTc5QzFGODRDOERFNzc3ODlFIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6MkI5RUJBNzdEOTYwMTFFNzlDMUY4NEM4REU3Nzc4OUUiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MkI5RUJBNzhEOTYwMTFFNzlDMUY4NEM4REU3Nzc4OUUiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz47Zb/UAAAEqklEQVR42mL8//8/AzpggdJwGc7A+YxMDFgAC0wWSAkB8Xtk7SDwDsYACCBGdIuYgNpglvzHsOD7+sR/TEiGg41gQXYPiAYIILChUHNg2qKBVCsQnwfiQJAcC5Ikul/l//3//xZICyMbrQakfgMV3wfxm5afjzx89cUzsJORdN4G4gcgJzx7+21L+6oLm3a3eB4CSQAEECO2EMUVbsgOXwOknKDhCA1/oElA/J8jYB7nf0wAlkd2Eysu65jwuQXoDDa4IiBHDkj9OHj5eTOIf+n+u+n7Lz1vgrkZPcQZ788L3yIhyOUFYuOy7r9i0srwN59+HEAWxIg7dAAMEgaAAIIHJi6FIEUEPAdRAwsvYFiC6LfQcAWx//7HAf79+38KTR+G32CxxYcvmBkZGUx//v5bQHSc4APsrMz9OA36/P33aViof//19zYhw4Dhw42RRaGBzQSN079AzAzEYmG2SjoLi+x3QdImwy+uoPmS0ITxA2Q32FJgYLOgWfIPiQ0y7Pmqw/deALECUPEDYNiAkt83IPsHcqyhlyS4oh9UdjwCYmmoi39hjQBC6YgYAPIaQAARlbuJASzofiU3ZTMRcLICtDw8By2NnwKxIV4XgcILVP7++fuvkpGR8TIzE+MWLOoFoYYyIdcU6AmSC0R8+vZb6sOXn+b4XPrhy69MfMUWMzjx/PvP9PP3P7zJQoCHbSpJ5R8xZSPFBr1YGk2d3M/DwZqN16BHr7+cmbzl6jJCBgHDkhFX9P8G+lsH5GpQfpqw4YrB13UJJ5gYGTlgCkwLNjiUBOkZAJMGY2zvgU3IiRXZoO9AfBOa60Fp5DZ30ALD96vidnOwMsuAFBgqizxM6D94DSr/iZRMC6ruQEXILRDn288/87jYWZJJziIg74Kyxdcff25AympGPbLqUZiX9XLW+gObFT/8mnbFEcz9BAq2J8DwAkXES5w1C7UKNiYGKgGAAKNaCUktwIKt5CQ1eCgBMPtg5rDgsSgASIEaXqC2ZyxQ40eouD6QApWPHqBGK5H2vv/x+68hMKM9JKQQV9yDMth6IAalQF9g/qgDlcPA3FsDpC8AcToJjgHXGEDHPABmVFNyHcSBzAEaxA6qYYA1Cycl0cPKwjR7z4Wn3OQ4iBGb2L9/WMWJdxAzk76DruQOYPQzUVISUTcXMTPZHOjwVhs0DgIBPUXhdlyhhM9B/5F6WP+hleEPajgImMB9QE10Uhz037dxl+nxG6861h17kC6fuKId5B61tFUzHCq2mj9/920HIUuBlc1PHOLfDlx+ngNrPREsqaEFFUgxJzS3gZruX6GtamZoe04A1N2pjTTULgzQ7eZkY5bF0pY7KRm7NBkpkzBCQx0Uyh+A+COwTPuNXjDichDBKgeIQc00HlAZMynD0jLVXWMBeoi/+vA9Ehi6q5DEQZb9Qx4VILqkJgBABv6E4k95M45/5GRjiYlxVFmG2lBlzwdSa4CW/SHWYIpzGSjYQVVD6qTDu8/eeVOHLPf64/eT0JAkv3Il01F/gEH/yaZ08ywgdx007YFCDzQ484cUs8hNQ/hCnA2a+P9C24p/SWk1sFC5zPsHzUXDp4EGAI6JNBF1D/kIAAAAAElFTkSuQmCC')
                            }
                        }
                        judge = false;
                    }
                })
            } else {
                alert(data.Message)
            }
        }

    })

    // 物料静态页点赞点踩接口
    function M_operation(i) {
        var that=true;
        $.ajax({
            url:public_url+'/api/ApiNL/SetCustomerInfo',
            type:'post',
            asycn:false,
            dataType: 'json',
            data:{
                MaterieUrl:window.location.href,
                LikeType:i
            },
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,
            success: function (data) {
                if(data.Status==0){

                }else {
                    alert(data.Message);
                    that=false
                }
            }
        });
        return that
    }

    // 选底价
    $(window).scroll(function () {
        var totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
        var documentheight = parseFloat($(document).height());
        if (documentheight - totalheight <= $('#footer').height()) {
            $('.pc_FloorPrice_price').css('bottom', $('#footer').height() + 'px');
        } else {
            $('.pc_FloorPrice_price').css('bottom', 0);
        }
    });
    //    点击关闭
    $('.pc_inquiry img').off('click').on('click', function () {
        $('.pc_FloorPrice_price').hide()
    })
    // 车型联动
    $.ajax({
        url: public_url + '/api/ApiNL/QueryBrand',
        type: 'get',
        async: false,
        dataType: 'json',
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        data: {},
        success: function (data) {
            if (data.Status == 0) {
                var str = '<option MasterId="-2">请选择品牌</option>';
                for (var i = 0; i < data.Result.length; i++) {
                    str += '<option MasterId=' + data.Result[i].MasterId + '>' + data.Result[i].Name + '</option>'
                }
                $('.brand').html(str);

                $('.brand').off('change').on('change', function () {
                    $('.brands').find('.hidden_tip').hide();
                    var MasterBrandId = $(this).find('option:checked').attr('MasterId');
                    if (MasterBrandId == -2) {
                        $('.series').html('<option BrandId="-2">请选择子品牌</option>');
                        $('.Models').html('<option CarSerialId="-2">请选择车型</option>');
                    } else {
                        $.ajax({
                            url: public_url + '/api/ApiNL/QueryBrand',
                            type: 'get',
                            async: false,
                            dataType: 'json',
                            xhrFields: {
                                withCredentials: true
                            },
                            crossDomain: true,
                            data: {
                                MasterBrandId: MasterBrandId
                            },
                            success: function (data) {
                                if (data.Status == 0) {
                                    var str1 = '<option BrandId="-2">请选择子品牌</option>';
                                    for (var j = 0; j < data.Result.length; j++) {
                                        str1 += '<option BrandId=' + data.Result[j].BrandId + '>' + data.Result[j].Name + '</option>'
                                    }
                                    $('.series').html(str1);
                                    $('.Models').html('<option MasterId="-2">请选择车型</option>');
                                    $('.series').off('change').on('change', function () {
                                        $('.brands').find('.hidden_tip').hide();
                                        var BrandId = $(this).find('option:checked').attr('BrandId');
                                        if (BrandId == -2) {
                                            $('.Models').html('<option MasterId="-2">请选择车型</option>');
                                        } else {
                                            $.ajax({
                                                url: public_url + '/api/ApiNL/QuerySerialList',
                                                type: 'get',
                                                async: false,
                                                dataType: 'json',
                                                xhrFields: {
                                                    withCredentials: true
                                                },
                                                crossDomain: true,
                                                data: {
                                                    BrandId: BrandId
                                                },
                                                success: function (data) {
                                                    var str2 = '<option CarSerialId="-2">请选择车型</option>';
                                                    for (var k = 0; k < data.Result.length; k++) {
                                                        str2 += '<option BrandId=' + data.Result[k].BrandId + ' CarSerialId=' + data.Result[k].CarSerialId + '>' + data.Result[k].ShowName + '</option>'
                                                    }
                                                    $('.Models').html(str2);
                                                    $('.Models').off('change').on('change', function () {
                                                        $('.brands').find('.hidden_tip').hide();
                                                    })
                                                }
                                            })
                                        }
                                    })
                                }
                            }
                        });
                    }
                })
                $('.brand').change();
                if ($('.pc_select').attr('brand')) {
                    $('.brand option').each(function () {
                        if ($(this).attr('MasterId') == $('.pc_select').attr('brand')) {
                            $(this).prop('selected', true);
                        }
                    })
                    $('.brand').change();
                    $('.series option').each(function () {
                        if ($(this).attr('BrandId') == $('.pc_select').attr('series')) {
                            $(this).prop('selected', true);
                        }
                    })
                    $('.series').change();
                    $('.Models option').each(function () {
                        if ($(this).attr('carserialid') == $('.pc_select').attr('Models')) {
                            $(this).prop('selected', true);
                        }
                    })
                }
            }
        }
    });

    $('.pc_inquiry button').on('click',function () {
        if($('.Models option:selected').attr('CarSerialId')!=2){
            window.location='http://cheshi.qichedaquan.com/xunjia/1_'+$('.Models option:selected').attr('CarSerialId')
        }else {
            window.location='http://price.qichedaquan.com'
        }
    })

    // 控制pc_content下的标签
    $('.pc_content img').css('max-width','100%');
})