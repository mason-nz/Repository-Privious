// 统一ajax异步请求方式
/**
* Written by:      lixh
* Created Date:    2017-3-1
* @param setting   ajax请求的参数，可设置url,type(可选，默认get),data（可选，默认为空）
* @param fn1    	异步请求的成功回调函数（参数data为请求回的数据）
* @param fn2    	异步请求的失败回调函数（可选）
* @param fn3    	请求发送之前的回调函数（可选）
*/
function setAjax(setting, fn1, fn2) {
    var r = setting.url.split('&r=')[1] || setting.data ? setting.data.r : '';
    if (!r && setting.data) {
        setting.data.r = Math.random();
    } else {
    }
    $.ajax({
        url: setting.url,
        type: setting.type,
        data: setting.data,
        dataType: 'json',
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: fn1,
        error: fn2,
        beforeSend: function () {
            $(setting.selector).html('<div style="margin:150px auto; width:32px;"><img src="/images/loading.gif"></div>');
        }
    });
}

// 价格数字格式化
/**
* Written by:      liushuai
* function:        将数字转换成货币类型,一般使用传入number和places即可
* Created Date:    2017-2-23
* Modified Date:   2017-2-26
* @param number    传入你想改变的数字
* @param places    保留小数位数,默认保留两位小数位
* @param symbol    以什么符号开头,默认是'¥ '
* @returns{string} 返回值为转换后的货币字符串
*/
function formatMoney(number, places, symbol) {
    number = number || 0;
    places = !isNaN(places = Math.abs(places)) ? places : 2;
    symbol = symbol !== undefined ? symbol : "¥";
    thousand = ",";
    decimal = ".";
    var negative = number < 0 ? "-" : "",
        i = parseInt(number = Math.abs(+number || 0).toFixed(places), 10) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return symbol + negative + (j ? i.substr(0, j) + thousand : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousand) + (places ? decimal + Math.abs(number - i).toFixed(places).slice(2) : "");
}

//判断当前字符串是否以str结束
if (typeof String.prototype.endsWith != 'function') {
    String.prototype.endsWith = function (str) {
        return this.slice(-str.length) == str;
    };
}

/**
* Written by:     liushuai
* function:       左侧导航栏渲染
* Created Date:   2017-02-23
* Modified Date:  2017-03-13
*/
function leftNav() {
    var changeImg = function () {
        var arr = $('.vtitle');
        for (var i = 0; i < arr.length; i++) {
            if ($(arr[i]).prop('class') != 'v_current') {
                var srcUrl = $(arr[i]).find('img').attr('src').slice(0, 14) + '.png';
                $(arr[i]).find('img').attr('src', srcUrl)
            }
        }
    };
    setAjax({
        url: 'http://j.chitunion.com/api/Authorize/GetMenuInfo',
        type: 'get'
    }, function (data) {
        var status = data.Status;
        /*判断图片类型*/
        if (status == 0) {
            var arr = data.Result;
            var str = '';
            var levelOne = ''; //一级标题
            for (var i = 0; i < arr.length; i++) {
                str += '<div class="vtitle" data-num="' + i + '" ModuleID="' + arr[i].ModuleID + '" level=1>';
                if (arr[i].SubModule.length != 0) {
                    str += '<em class="v"></em>';
                }
                var imageUrl = '';
                switch (arr[i].ModuleID) {
                    case 'SYS001MOD0010':
                        imageUrl = '/images/ico_06.png';
                        break;
                    case 'SYS001MOD0011':
                        imageUrl = '/images/ico_05.png';
                        break;
                    case 'SYS001MOD0002':
                        imageUrl = '/images/ico_01.png';
                        break;
                    // case '媒体刊例列表':        
                    /*    case '媒体刊例列表':
                    imageUrl = '/images/ico_02.png';
                    break;*/ 
                    case 'SYS001MOD0003':
                        imageUrl = '/images/ico_12.png';
                        break;
                    /*  case '用户中心':
                    imageUrl = '/images/ico_03.png';
                    break;*/ 
                    case 'SYS001MOD0006':
                        imageUrl = '/images/ico_07.png';
                        break;
                    case 'SYS001MOD0008':
                        imageUrl = '/images/ico_08.png';
                        break;
                    case 'SYS001MOD0012':
                        imageUrl = '/images/ico_09.png';
                        break;
                    case 'SYS001MOD0004':
                        imageUrl = '/images/ico_10.png';
                        break;
                    case 'SYS001MOD0005':
                        imageUrl = '/images/ico_11.png';
                        break;
                    default:
                        imageUrl = '/images/ico_01.png';
                }
                if (arr[i].SubModule == '') {
                    levelOne = arr[i].Url;
                } else {
                    levelOne = 'javascript:;'
                }
                str += '<img src=' + imageUrl + '>&nbsp;<a href="' + levelOne + '">' + arr[i].ModuleName + '</a></div>';
                /*添加二级菜单*/
                if (arr[i].SubModule) {
                    var arrTwo = arr[i].SubModule;
                    str += '<div class="vcon" style="display:none;">';
                    str += '<ul class="vconlist clearfix">';
                    for (var j = 0; j < arrTwo.length; j++) {
                        url = arrTwo[j].Url;
                        links = arrTwo[j].Links;
                        str += '<li data-link="' + links + '" ModuleID="' + arrTwo[j].ModuleID + '" level=2 ><a href="' + url + '">' + arrTwo[j].ModuleName + '</a></li>';
                    }
                    str += '</ul>';
                    str += '</div>'
                }
            }
        }
        $('.order_l').html(str);
        var tabs_i = 0;
        var curUrl = window.location.href;
        var arrUrl = curUrl.split('?');
        $('.vtitle').on('click', function () {
            tabs_i = parseInt($(this).attr('data-num'));
            $(this).siblings('.v_current').attr('class', 'vtitle');
            $(this).attr('class', 'v_current');
            $('.vcon').css('display', 'none');
            $(this).next('.vcon ').css('display', 'block');
            $('.vcon').eq(tabs_i).slideDown().siblings('.vcon').slideUp();
            var currentSrc = $(this).find('img').attr('src');
            var newSrc = currentSrc.slice(0, 14) + '_h.png';
            $(this).find('img').attr('src', newSrc); //图片显示高亮
            changeImg(); //不是当前的标题 图片为灰色
            $('.vtitle').each(function () {
                if ($(this).find('a').attr('href') == curUrl) {
                    $(this).prop('class', 'v_current');
                } else {
                    $(this).prop('class', 'vtitle')
                }
            })
        });
        var vtitleArr = $('.vtitle');
        var array = data.Result;

        //判断一级标题隐藏页
        for (var m = 0; m < array.length; m++) {
            var arrLinks = array[m].Links.split(',');
            if (arrLinks == '') {
                continue;
            }
            $.each(arrLinks, function (index, n) {
                if (n && n != '' && arrUrl[0].toUpperCase().endsWith(n.toUpperCase())) {
                    //$(vtitleDiv).prop('class', 'v_current');
                    //alert(arrUrl[0]);
                    $('#divMenu [moduleid="' + array[m].ModuleID + '"]').prop('class', 'v_current').click();
                    //$(vtitleDiv).click();
                    return false;
                }
            });
        }


        //        //判断一级标题
        //        vtitleArr.each(function (index, vtitleDiv) {
        //            var oneHref = $(this).find('a').attr('href');
        //            if (oneHref) {
        //                if (oneHref.toUpperCase() == arrUrl[0].toUpperCase()) {
        //                    changeImg();
        //                    $(this).prop('class', 'v_current');
        //                    $(this).click();
        //                }
        //                //}else{
        //                //判断一级标题隐藏页
        //                for (var i = 0; i < array.length; i++) {
        //                    var arrLinks = array[i].Links.split(',');
        //                    if (arrLinks == '') {
        //                        continue;
        //                    }
        //                    $.each(arrLinks, function (i, n) {
        //                        if (n && n != '' && arrUrl[0].toUpperCase().endsWith(n.toUpperCase())) {
        //                            $(vtitleDiv).prop('class', 'v_current');
        //                            $(vtitleDiv).click();
        //                            return false;
        //                        }
        //                    });
        //                    //                    if (arrUrl[0].toUpperCase() == arrLinks[i].toUpperCase()) {
        //                    //                        $(this).click();
        //                    //                        return;
        //                    //                    }
        //                }
        //            }
        //        });
        /*判断二级标题*/
        var vconArr = $('.vcon ul li');
        vconArr.each(function () {
            if ($(this).find('a').attr('href').toUpperCase() == arrUrl[0].toUpperCase()) {//arrUrl[0]当前浏览器的URL
                $(this).addClass('vcon_select').siblings().removeClass('vcon_select');
                $(this).parent().parent().prev().click()
            } else {
                var arrLinks = $(this).attr('data-link').split(',');
                for (var i = 0; i < arrLinks.length; i++) {
                    for (var j = 0; j < arrLinks[i].length; j++) {
                        if (arrUrl[0].toUpperCase().indexOf(arrLinks[i].toUpperCase()) != -1) {
                            $(this).addClass('vcon_select').siblings().removeClass('vcon_select');
                            $(this).parent().parent().prev().click();
                            return;
                        }
                    }
                }
            }
        });
        $('.vcon li').on('click', function () {
            $('.vcon ul li').removeClass('vcon_select');
            $(this).addClass('vcon_select').siblings().removeClass('vcon_select');
            $(this).find('a').css('color', '#FF9100').siblings().css('color', '#333');
        });
        /*鼠标移入移出时候的图片效果*/
        $('.vtitle a').mouseover(function () {
            var str = $(this).siblings('img').attr('src').slice(0, 14) + '_h.png';
            $(this).siblings('img').attr('src', str);
        });
        $('.vtitle a').mouseout(function () {
            if ($(this).parent('div').prop('class') != 'v_current') {
                var str = $(this).siblings('img').attr('src').slice(0, 14) + '.png';
                $(this).siblings('img').attr('src', str);
            }
        });
    })
}



//判断日期格式是否合法
String.prototype.isDate = function () {
    var r = this.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
    if (r == null) return false; var d = new Date(r[1], r[3] - 1, r[4]);
    return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
}

//日期类型，添加天数函数
Date.prototype.addDate = function (days) {
    var a = this;
    a = a.valueOf();
    a = a + days * 24 * 60 * 60 * 1000
    a = new Date(a);
    return a;
}

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}