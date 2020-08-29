/*
* Copyright (c) 2010 Mason
*/
(function($, undefined) {
    var fc = $.fullCalendar = {};
    var top = 0,
        cellWidth = 0, //每个单元格宽度
        cellSide = 0, //每个单元格边的距离
    mousedownStartDate = null, //鼠标按下时，计算X轴坐标所在开始日期（添加备注时用）
    mouseupStartX = 0;

    /* Defaults
    -----------------------------------------------------------------------------*/
    var defaults = {
        // display
        view: 'day',
        dateStart: '',
        dateCount: 31,
        weekCount: 8,
        monthCount: 4,
        rowName: 'tr1'
    };
    // function for adding/overriding defaults
    var setDefaults = fc.setDefaults = function(d) {
        $.extend(true, defaults, d);
    };
    /* .fullCalendar jQuery function
    -----------------------------------------------------------------------------*/
    $.fn.fullCalendar = function(options) {
        // initialize options
        options = $.extend(true, {},
		defaults, options);
        this.each(function() {
            /* Instance Initialization
            -----------------------------------------------------------------------------*/
            var _element = this, element = $(_element);
            element.css('position', 'relative'); //设置DIV样式
            initPara(options.view); //初始化参数
            renderGrid(element, options); //绘制表格及初始化备注信息
            element.data('startDate', parseDate(options.dateStart)); //保存当前元素开始日期
            /* Public Methods
            -----------------------------------------------------------------------------*/

            var publicMethods = {
                prev: function() {
                    changeView(-1, element, options);
                },
                next: function() {
                    changeView(1, element, options);
                },
                today: function() {
                    changeView(0, element, options);
                },
                changeMode: function(mode) {
                    initPara(mode); //初始化单元格宽度参数
                    changeMode(element, options, mode);
                },
                getHeaderTitle: function() {
                    return GetHeaderTitle(element, options);
                },
                getElement: function() {
                    return element;
                },
                getOptions: function() {
                    return options;
                }
            };
            $.data(this, 'publicMethods', publicMethods);
            bindEvent(element, options); //绑定事件
        });
        return this;
    };
    //上一页
    $.fn.fullCalendar.prev = function(obj) {
        $.each(obj, function(i, n) {
            var publicMethods = $(n).data('publicMethods');
            if (publicMethods != undefined || publicMethods != null) {
                publicMethods['prev']();
            }
        });
    }
    //下一页
    $.fn.fullCalendar.next = function(obj) {
        $.each(obj, function(i, n) {
            var publicMethods = $(n).data('publicMethods');
            if (publicMethods != undefined || publicMethods != null) {
                publicMethods['next']();
            }
        });
    }
    //今天
    $.fn.fullCalendar.today = function(obj) {
        $.each(obj, function(i, n) {
            var publicMethods = $(n).data('publicMethods');
            if (publicMethods != undefined || publicMethods != null) {
                publicMethods['today']();
            }
        });
    }
    //更改视图显示方式，天、周、月
    $.fn.fullCalendar.changeMode = function(obj, mode) {
        $.each(obj, function(i, n) {
            var publicMethods = $(n).data('publicMethods');
            if (publicMethods != undefined || publicMethods != null) {
                publicMethods['changeMode'](mode);
            }
        });
    }
    //获取时间信息
    $.fn.fullCalendar.GetHeaderTitle = function(obj) {
        var currentDateTitle = new Object();
        $.each(obj, function(i, n) {
            var publicMethods = $(n).data('publicMethods');
            if (publicMethods != undefined || publicMethods != null) {
                currentDateTitle = publicMethods['getHeaderTitle']();
                return false;
            }
        });
        return currentDateTitle;
    }
    //新建备注事件
    $.fn.fullCalendar.newEvent = function(obj, event) {
        var ele = new Object();
        var opt = new Object();
        var publicMethods = $(obj[0]).data('publicMethods');
        if (publicMethods != undefined || publicMethods != null) {
            ele = publicMethods['getElement']();
            opt = publicMethods['getOptions']();
        }
        appendEvent(obj.find('div[name="eventArea"]')[0], event, ele, opt);
        ///var obj = $(e.data.ele).find('div[name="eventArea"]');
        //appendEvent(obj, event, e.data.ele, e.data.opt);
    }
    //删除一个备注事件
    $.fn.fullCalendar.delEvent = function(obj, tmCode) {
        var ele = new Object();
        var opt = new Object();
        var publicMethods = $(obj[0]).data('publicMethods');
        if (publicMethods != undefined || publicMethods != null) {
            ele = publicMethods['getElement']();
            opt = publicMethods['getOptions']();
        }
        var delObj = obj.find('div[name="eventArea"] span[tmcode="' + tmCode + '"]');
        if (delObj.size() > 0) {
            delObj.parent().parent().remove();
        }
        saveDelEventCache($(obj[0]), opt, tmCode);
    }
    //获取时间信息
    function GetHeaderTitle(element, options) {
        var d = element.data('startDate'), dEnd, dText = '';
        if (d == undefined) {
            d = options.dateStart;
        }
        switch (options.view) {
            case 'day':
                dEnd = addDays(cloneDate(d), options.dateCount - 1);
                var startYear = d.getFullYear();
                var endYear = dEnd.getFullYear();
                var startMonth = d.getMonth() + 1;
                var endMonth = dEnd.getMonth() + 1;
                if (startYear == endYear && startMonth == endMonth) {
                    var startDate = d.getDate();
                    dText += startYear + '年' + startMonth + '月' + startDate + '日' + '——' + (startDate + options.dateCount - 1) + '日';
                }
                else {
                    var startDate = d.getDate();
                    var endDate = dEnd.getDate();
                    dText += startYear + '年' + startMonth + '月' + startDate + '日' + '——' + endYear + '年' + endMonth + '月' + endDate + '日';
                }
                break;
            case 'week':
                while (d.getDay() != 1) {
                    d = addDays(cloneDate(d), -1);
                }
                dEnd = addDays(cloneDate(d), options.weekCount * 7 - 1);
                var startYear = d.getFullYear();
                var endYear = dEnd.getFullYear();
                var startMonth = d.getMonth() + 1;
                var endMonth = dEnd.getMonth() + 1;
                if (startYear == endYear && startMonth == endMonth) {
                    var startDate = d.getDate();
                    dText += startYear + '年' + startMonth + '月' + startDate + '日' + '——' + (startDate + options.weekCount * 7 - 1) + '日';
                }
                else {
                    var startDate = d.getDate();
                    var endDate = dEnd.getDate();
                    dText += startYear + '年' + startMonth + '月' + startDate + '日' + '——' + endYear + '年' + endMonth + '月' + endDate + '日';
                }
                break;
            case 'month':
                dEnd = addDays(cloneDate(addMonths(cloneDate(d), options.monthCount)), -1);
                var startYear = d.getFullYear();
                var endYear = dEnd.getFullYear();
                if (startYear == endYear) {
                    var startMonth = d.getMonth() + 1;
                    dText += startYear + '年' + startMonth + '月——' + (startMonth + options.monthCount - 1) + '月';
                }
                else {
                    var startMonth = d.getMonth() + 1;
                    var endMonth = dEnd.getMonth() + 1;
                    dText += startYear + '年' + startMonth + '月——' + endYear + '年' + endMonth + '月';
                }
                break;
            default: break;
        }

        return { 'CurrentView': options.view, 'dStart': d, 'dEnd': dEnd, 'dText': dText };
    }
    //更改视图显示方式
    function changeMode(element, options, mode) {
        element.children().remove();
        options.view = mode;
        renderGrid(element, options); //初始化表格
        bindEvent(element, options); //绑定事件
    }
    //更改视图
    function changeView(v, element, options) {
        var days, html = '';
        var objDiv = element.find('div[name="eventArea"]');
        objDiv.children().remove();
        if (v == 0) {
            switch (options.view) {
                case 'day':
                    options.dateStart = addDays(new Date(), -Math.round(options.dateCount / 2));
                    break;
                case 'week':
                    var dTemp = new Date();
                    dTemp = addDays(dTemp, 1 - (dTemp.getDay() == 0 ? 7 : dTemp.getDay()));
                    options.dateStart = addDays(dTemp, -Math.round(options.weekCount / 2) * 7);
                    break;
                case 'month':
                    options.dateStart = addMonths(new Date(), -Math.round(options.monthCount / 2));
                    options.dateStart.setDate(1);
                    break;
                default: break;
            }
            element.data('startDate', options.dateStart);
        }
        else {
            var date = element.data('startDate');
            switch (options.view) {
                case 'day':
                    days = v * options.dateCount;
                    if (date == undefined) {
                        options.dateStart = addDays(parseDate(options.dateStart), days);
                        element.data('startDate', options.dateStart);
                    }
                    else {
                        options.dateStart = addDays(date, days);
                    }
                    break;
                case 'week':
                    days = v * options.weekCount * 7;
                    if (date == undefined) {
                        options.dateStart = addMonths(parseDate(options.dateStart), days);
                        element.data('startDate', options.dateStart);
                    }
                    else {
                        options.dateStart = addDays(date, days);
                    }
                    break;
                case 'month':
                    days = v * options.monthCount;
                    if (date == undefined) {
                        options.dateStart = addMonths(parseDate(options.dateStart), days);
                        element.data('startDate', options.dateStart);
                    }
                    else {
                        options.dateStart = addMonths(date, days);
                    }
                    break;
                default: break;
            }
        }
        html = renderEvent(element, options);
        if (html != '') {
            $(html).appendTo(objDiv)
               .bind("click", { ele: element, opt: options }, editEvent) //绑定编辑事件;
               .bind("mouseover", { ele: element, opt: options }, mouseoverEvent) //绑定鼠标进入事件
               .bind("mouseout", { ele: element, opt: options }, mouseoutEvent);
        }
    }
    //保存新添或修改的事件
    function saveEditEventCache(element, event) {
        var eventArray = element.data('EditEventCache');
        if (eventArray != undefined) {
            var flag = false;
            $.each(eventArray, function(i, n) {
                if (n.tmCode == event.tmCode) {
                    flag = true;
                    return false;
                }
            });
            if (!flag) { eventArray.push(event); }
        }
        else {
            var array = new Array();
            array.push(event);
            element.data('EditEventCache', array);
        }
    }
    //保存删除的事件
    function saveDelEventCache(element, opt, tmCode) {
        var eventArray = element.data('EditEventCache');
        if (eventArray != undefined) {
            var array = new Array();
            $.each(eventArray, function(i, n) {
                if (n.tmCode != tmCode) {
                    array.push(n);
                }
            });
            element.data('EditEventCache', array);
        }
        var delFalg = new Array();
        if (opt.events != undefined) {
            $.each(opt.events, function(i, n) {
                if (n.tmCode == tmCode) {
                    delFalg[delFalg.length] = i;
                }
            });
            for (var k = delFalg.length - 1; k >= 0; k--) {
                opt.events.splice(delFalg[k], 1);
            }
        }
    }
    //绑定事件
    function bindEvent(element, options) {
        element.find('div[class^="fcTable-"]').bind("mousedown", { ele: element, opt: options }, calcStartDate)//绑定鼠标按下事件
        .bind("mousemove", { ele: element, opt: options }, caleGlideLayerWidth)//绑定鼠标移动事件，计算滑块宽度
        .bind("mouseup", { ele: element, opt: options }, addEvent); //绑定添加事件

        element.find('div.fc-event').bind("click", { ele: element, opt: options }, editEvent)//绑定编辑事件
        .bind("mouseover", { ele: element, opt: options }, mouseoverEvent) //绑定鼠标进入事件
        .bind("mouseout", { ele: element, opt: options }, mouseoutEvent); //绑定鼠标移出事件
    }
    //初始化参数
    function initPara(view) {
        switch (view) {
            case 'day':
                cellWidth = 18;
                cellSide = 0;
                break;
            case 'week':
                cellWidth = 72;
                cellSide = 0;
                break;
            case 'month':
                cellWidth = 144;
                cellSide = 0;
                break;
            default: break;
        }
    }
    //生成一个滑动层
    function createGlideLayer(element, options, startDate) {
        var html = '', dataStart = parseDate(element.data('startDate')),
        left = 0;
        switch (options.view) {
            case 'day':
                left = dayDiff(startDate, dataStart) * cellWidth;
                break;
            case 'week':
                while (dataStart.getDay() != 1) {
                    dataStart = addDays(cloneDate(dataStart), -1);
                }
                left = dayDiff(startDate, dataStart) / 7 * cellWidth;
                break;
            case 'month':
                left = dayDiff(startDate, dataStart) / 30 * cellWidth;
                break;
            default: break;
        }
        html += "<div class='fc-event fc-glidelayer' style='display:none;position:absolute;z-index:8;top:" + top + "px;left:" + left + "px;width:" + cellWidth + "px;' " + " >" +
			"</div>";
        if (html != '') {
            $(html).appendTo(element.find('div[name="eventArea"]')[0])
            //.bind('mouseup', function() { })
            .bind("mousemove", { ele: element, opt: options }, caleGlideLayerWidth) //绑定鼠标移动事件，计算滑块宽度
            .bind("mouseup", { ele: element, opt: options }, addEvent); //绑定添加事件
        }
    }
    //追加一个备注事件
    function appendEvent(objDiv, event, element, options) {
        var html = '',
        left = 0,
        width = 0, modeCount;
        switch (options.view) {
            case 'day':
                modeCount = options.dateCount;
                break;
            case 'week':
                modeCount = options.weekCount * 7;
                while (options.dateStart.getDay() != 1) {
                    options.dateStart = addDays(cloneDate(options.dateStart), -1);
                }
                break;
            case 'month':
                modeCount = options.monthCount * 30;
                options.dateStart = parseDate(options.dateStart);
                parseDate(options.dateStart).setDate(1);
                //options.dateStart = options.dateStart.substring(0, options.dateStart.length - 2) + '01';
                break;
            default: false;
        }
        var eventStartDate = parseDate(event.start);
        var eventEndDate = eventStartDate;
        if (event.end != undefined) {
            eventEndDate = parseDate(event.end);
        }
        var startIndex = endIndex = -1;
        var usedays = 1; //经过天数
        if (eventEndDate != undefined && eventEndDate != '') {
            eventEndDate = parseDate(eventEndDate);
            endIndex = dayDiff(eventEndDate, parseDate(options.dateStart));
        }
        startIndex = dayDiff(eventStartDate, parseDate(options.dateStart));
        if (startIndex < 0 && endIndex >= 0) {//时间间隔在开始日期当中
            startIndex = 0;
        }
        if (endIndex >= modeCount) {//时间间隔在结束日期当中
            endIndex = modeCount - 1;
        }
        if (startIndex >= 0 && startIndex < modeCount) {
            if (endIndex >= 0) {
                usedays = endIndex - startIndex + 1;
            }
            switch (options.view) {
                case 'day':
                    width = cellWidth * usedays - cellSide * 2;
                    left = cellWidth * startIndex + cellSide;
                    break;
                case 'week':
                    width = cellWidth * (parseInt(usedays / 7) + (usedays % 7 > 0 ? 1 : 0)) - cellSide * 2;
                    left = cellWidth * ((startIndex + (7 - startIndex % 7)) / 7 - 1) + cellSide;
                    break;
                case 'month':
                    width = cellWidth * (parseInt(usedays / 30) + (usedays % 30 > 0 ? 1 : 0)) - cellSide * 2;
                    left = cellWidth * (parseInt(startIndex / 30) + (startIndex % 30 > 0 ? 1 : 0) - 1) + cellSide;
                    left = (left < 0 ? cellSide : left);
                    break;
                default: false;
            }
            if (options.view == 'day') {
                html +=
			"<div class='fc-event fc-event-hori' style='position:absolute;z-index:" + (event.isMilestone != undefined && event.isMilestone ? '7;height:31px;' : '8') + ";top:" + top + "px;left:" + left + "px;width:" + width + "px;' " + (options.view == 'day' ? "title='" + htmlEscape(event.title) + "'" : "") + " >" +
				"<a " + (event.stylestr != undefined ? ("class='" + event.stylestr + "'") : "") + ">" +
					"<span class='fc-event-title' startDate='" + event.start + "' endDate='" + (event.end != undefined ? event.end : event.start) + "' tmcode='" + event.tmCode + "' " + (event.fontColor != undefined ? "style='color:" + event.fontColor + ";'" : "") + ">" + htmlEscape(event.title) + "</span>" +
				"</a>" +
                //					"<div class='ui-resizable-handle ui-resizable-e''></div>"
                //					+
			"</div>";
            }
            else if (options.view == 'week' || options.view == 'month') {
                var splitWeekCount = width / cellWidth - 1;
                for (var w = 0; w <= splitWeekCount; w++) {
                    left += (w == 0 ? 0 : cellWidth);
                    if (left < (options.view == 'week' ? options.weekCount : options.monthCount) * cellWidth) {
                        html +=
			"<div class='fc-event fc-event-hori' style='position:absolute;z-index:" + (event.isMilestone != undefined && event.isMilestone ? '7;height:31px;' : '8') + ";top:" + top + "px;left:" + left + "px;width:" + cellWidth + "px;' " + (options.view == 'day' ? "title='" + htmlEscape(event.title) + "'" : "") + " >" +
				"<a " + (event.stylestr != undefined ? ("class='" + event.stylestr + "'") : "") + ">" +
					"<span class='fc-event-title' startDate='" + event.start + "' endDate='" + (event.end != undefined ? event.end : event.start) + "' tmcode='" + event.tmCode + "' " + (event.fontColor != undefined ? "style='color:" + event.fontColor + ";'" : "") + ">" + htmlEscape(event.title) + "</span>" +
				"</a>" +
                        //					"<div class='ui-resizable-handle ui-resizable-e''></div>"
                        //					+
			"</div>";
                    }
                }
            }
        }
        //alert(html);
        if (html != '') {
            $(html).appendTo(objDiv)
               .bind("click", { ele: element, opt: options }, editEvent); //绑定编辑事件;
        }
        var newEvent = { title: htmlEscape(event.title), start: event.start, end: (event.end != undefined ? event.end : event.start), tmCode: event.tmCode, stylestr: (event.stylestr != undefined ? event.stylestr : ''), isMilestone: event.isMilestone };
        saveEditEventCache(element, newEvent);
    }
    //计算鼠标按下时，选择的日期，以及显示滑动层
    function calcStartDate(e) {
        initPara(e.data.opt.view); //初始化参数
        layerX = e.originalEvent.layerX;
        if (e.originalEvent.layerX == undefined) {
            layerX = e.originalEvent.x;
        }
        mouseupStartX = layerX;
        var selectDate = GetClickDate(layerX, e.data.opt, e.data.ele);
        switch (e.data.opt.view) {
            case 'day':
                $.each(e.data.ele.find('div.fc-event span.fc-event-title'), function(i, n) {
                    var startDate = parseDate($(n).attr('startdate'));
                    var enddate = parseDate($(n).attr('enddate'));
                    if (dayDiff(selectDate, startDate) >= 0 && dayDiff(enddate, selectDate) >= 0 && $(n).parent().parent().css('z-index') != 7) {
                        alert('当前日期已有事件，不能添加！');
                        return flag = false;
                    }
                });
                break;
            case 'week':
                selectDate = addDays(selectDate, 1 - (selectDate.getDay() == 0 ? 7 : selectDate.getDay()));
                break;
            case 'month':
                selectDate.setDate(1);
                break;
            default: break;
        }
        mousedownStartDate = selectDate;
        createGlideLayer(e.data.ele, e.data.opt, selectDate);
    }
    function caleGlideLayerWidth(e) {
        var objDiv = e.data.ele.find('div.fc-glidelayer');
        if (mousedownStartDate != null) {
            objDiv.show();
            layerX = e.originalEvent.layerX;
            if (e.originalEvent.layerX == undefined) {
                layerX = e.originalEvent.x;
            }
            if (!$.browser.msie) {
                layerX += mouseupStartX;
            }
            var selectDate = GetClickDate(layerX, e.data.opt, e.data.ele);
            var w = 0;
            switch (e.data.opt.view) {
                case 'day':
                    w = (dayDiff(selectDate, mousedownStartDate) + 1) * cellWidth;
                    break;
                case 'week':
                    w = (parseInt(dayDiff(selectDate, mousedownStartDate) / 7) + 1) * cellWidth;
                    break;
                case 'month':
                    w = (parseInt(dayDiff(selectDate, mousedownStartDate) / 30) + 1) * cellWidth;
                    break;
                default: break;
            }
            if (layerX < objDiv.parent().parent().width())
            { objDiv.width(w); }
        }
    }
    //鼠标添加备注事件
    function addEvent(e) {
        var flag = true, layerX;
        initPara(e.data.opt.view); //初始化参数
        layerX = e.originalEvent.layerX;
        if (e.originalEvent.layerX == undefined) {
            layerX = e.originalEvent.x;
        }
        if (e.data.ele.find('div.fc-glidelayer:visible').size() > 0 &&
            e.originalEvent.layerX != undefined) { //显示滑动层时,仅FF浏览器
            var dataStart = parseDate(e.data.ele.data('startDate'));
            switch (e.data.opt.view) {
                case 'day':
                    layerX += dayDiff(mousedownStartDate, dataStart) * cellWidth;
                    break;
                case 'week':
                    while (dataStart.getDay() != 1) {
                        dataStart = addDays(cloneDate(dataStart), -1);
                    }
                    layerX += dayDiff(mousedownStartDate, dataStart) / 7 * cellWidth;
                    break;
                case 'month':
                    layerX += dayDiff(mousedownStartDate, dataStart) / 30 * cellWidth;
                    break;
                default: break;
            }
        }
        var selectDate = GetClickDate(layerX, e.data.opt, e.data.ele);
        switch (e.data.opt.view) {
            case 'day':
                $.each(e.data.ele.find('div.fc-event span.fc-event-title'), function(i, n) {
                    var startDate = parseDate($(n).attr('startdate'));
                    var enddate = parseDate($(n).attr('enddate'));
                    if (dayDiff(selectDate, startDate) >= 0 && dayDiff(enddate, selectDate) >= 0 && $(n).parent().parent().css('z-index') != 7) {
                        alert('当前日期已有事件，不能添加！');
                        return flag = false;
                    }
                });
                break;
            case 'week':
                selectDate = addDays(selectDate, 1 - (selectDate.getDay() == 0 ? 7 : selectDate.getDay()));
                break;
            case 'month':
                selectDate.setDate(1);
                break;
            default: break;
        }
        if (flag && mousedownStartDate != null) {
            e.data.ele.find('div[class^="fcTable-"]').unbind("mouseup")//.unbind("mousedown")
        .bind("mouseup", { sDate: mousedownStartDate, eDate: selectDate, ele: e.data.ele, opt: e.data.opt }, e.data.opt.addEvent).trigger("mouseup").unbind("mouseup")
        .bind("mouseup", { ele: e.data.ele, opt: e.data.opt }, addEvent);
        }
        mousedownStartDate = null;
        e.data.ele.find('div.fc-glidelayer').remove();
    }
    //鼠标编辑备注事件
    function editEvent(e) {
        var objDiv = $(e.currentTarget);
        var eventList = new Array();

        var objDivLeft = objDiv.css('left');
        var objDivWidth = objDiv.width();
        $.each(objDiv.parent().find('div.fc-event'), function(i, n) {
            if ($(n).css('left') == objDivLeft && $(n).width() == objDivWidth) {
                var tmcode = $(n).find('span').attr('tmcode');
                var startDate = $(n).find('span').attr('startdate');
                var enddate = $(n).find('span').attr('enddate');
                var titleContent = $.trim($(n).find('span').html());
                var ismilestone = $(n).css('z-index') == 7 ? true : false;
                eventList.push({ sDate: startDate, eDate: enddate, title: titleContent, tmCode: tmcode, isMilestone: ismilestone });
            }
        });

        objDiv.unbind("click")
        .bind("click", { eventArray: eventList, opt: e.data.opt, pageX: e.pageX, pageY: e.pageY }, e.data.opt.editEvent).trigger("click").unbind("click")
        .bind("click", { ele: e.data.ele, opt: e.data.opt }, editEvent);
    }
    //鼠标进入备注事件
    function mouseoverEvent(e) {
        var objDiv = $(e.currentTarget);
        var eventList = new Array();

        var objDivLeft = objDiv.css('left');
        //var objDivWidth = objDiv.width();
        $.each(objDiv.parent().find('div.fc-event'), function(i, n) {
            if ($(n).css('left') == objDivLeft && $(n).width() >= cellWidth) {
                var tmcode = $(n).find('span').attr('tmcode');
                var startDate = $(n).find('span').attr('startdate');
                var enddate = $(n).find('span').attr('enddate');
                var titleContent = $.trim($(n).find('span').html());
                eventList.push({ sDate: startDate, eDate: enddate, title: titleContent, tmCode: tmcode });
            }
        });

        objDiv.unbind("mouseover")
          .bind("mouseover", { eventArray: eventList, opt: e.data.opt, pageX: e.pageX, pageY: e.pageY }, e.data.opt.mouseoverEvent)
          .trigger("mouseover").unbind("mouseout")
          .bind("mouseout", { ele: e.data.ele, opt: e.data.opt }, e.data.opt.mouseoutEvent);
    }
    //鼠标移出备注事件
    function mouseoutEvent(e) {
        var objDiv = $(e.currentTarget);
        objDiv.unbind("mouseout")
          .bind("mouseout", e.data.opt.mouseoutEvent);
    }
    //计算鼠标单击时，所选当前日期
    function GetClickDate(xposition, options, ele) {
        var usedays;
        usedays = parseInt((xposition - cellSide) / cellWidth); //当前
        //alert(usedays);
        //alert(addDays(parseDate(options.dateStart), usedays));
        var d = parseDate(options.dateStart);
        if (ele.data('startDate') != undefined) {
            d = parseDate(ele.data('startDate'));
            options.dateStart = cloneDate(d);
        }
        switch (options.view) {
            case 'day':
                return addDays(cloneDate(d), usedays, false);
                break;
            case 'week':
                return addDays(cloneDate(d), usedays * 7, false);
                break;
            case 'month':
                return addMonths(cloneDate(d), usedays, false);
                break;
            default:
                return null;
                break;
        }
    }
    //初始化已"周"为单位的，备注信息
    function GetWeeksEventsHtml(element, options, editEventCache) {
        var events = null, html = '', dateStart, dateEnd, // dateMonthStart, dateMonthEnd
        left = 0,
        width = 0, genEvents = new Array(),
        d = element.data('startDate');
        if (d != undefined) {
            dateStart = cloneDate(parseDate(d));
            if (dateStart.getDay() != 1) {
                dateStart = addDays(dateStart, 1 - (dateStart.getDay() == 0 ? 7 : dateStart.getDay()));
            }
        }
        else {
            var newdate = cloneDate(parseDate(options.dateStart));
            dateStart = addDays(cloneDate(newdate), 1 - (newdate.getDay() == 0 ? 7 : newdate.getDay()));
        }

        dateEnd = addDays(cloneDate(dateStart), options.weekCount * 7 - 1);
        //        dateMonthStart = dateStart.getMonth() + 1;
        //        dateMonthEnd = dateEnd.getMonth() + 1;
        if (editEventCache != undefined) {
            events = element.data('EditEventCache');
        }
        else if (options.events != undefined && options.events.length > 0) {
            events = options.events;
        }
        if (events != null && events != undefined && events.length > 0) {
            for (var i = 0; i < events.length; i++) {
                var eventStartDate = parseDate(events[i].start); //事件开始日期
                var eventEndDate = parseDate(events[i].end); //事件结束日期
                if (dayDiff(eventEndDate, dateStart) >= 0 &&
                    dayDiff(dateEnd, eventStartDate) >= 0) {//在月份范围之内的事件

                    var startIndex = endIndex = -1;
                    var usedays = 1; //经过天数
                    if (eventEndDate != undefined && eventEndDate != '') {
                        eventEndDate = parseDate(eventEndDate);
                        endIndex = dayDiff(eventEndDate, dateStart);
                    }
                    startIndex = dayDiff(eventStartDate, dateStart);
                    if (startIndex < 0 && endIndex >= 0) {//时间间隔在开始日期当中
                        startIndex = 0;
                    }
                    if (endIndex >= options.weekCount * 7) {//时间间隔在结束日期当中
                        endIndex = options.weekCount * 7 - 1;
                    }
                    if (startIndex >= 0 && startIndex < options.weekCount * 7) {
                        if (endIndex >= 0) {
                            usedays = endIndex - startIndex + 1;
                        }
                        if (startIndex > 0) {
                            switch (eventStartDate.getDay()) {
                                case 0:
                                    usedays += 6;
                                    break;
                                default:
                                    usedays += (eventStartDate.getDay() - 1);
                                    break;
                            }
                        }
                        width = cellWidth * (parseInt(usedays / 7) + (usedays % 7 > 0 ? 1 : 0)) - cellSide * 2;
                        left = cellWidth * ((startIndex + (7 - startIndex % 7)) / 7 - 1) + cellSide;

                        var splitWeekCount = width / cellWidth - 1;
                        for (var w = 0; w <= splitWeekCount; w++) {
                            left += (w == 0 ? 0 : cellWidth);
                            if (left < options.weekCount * cellWidth) {
                                html +=
			"<div class='fc-event fc-event-hori' style='position:absolute;z-index:" + ((events[i].isMilestone + '').toLowerCase() == 'true' ? '7;height:31px;' : '8') + ";top:" + top + "px;left:" + left + "px;width:" + cellWidth + "px;'>" +
				"<a " + (events[i].stylestr != undefined ? ("class='" + events[i].stylestr + "'") : "") + ">" +
					"<span class='fc-event-title' startDate='" + events[i].start + "' endDate='" + (events[i].end != undefined ? events[i].end : events[i].start) + "' tmcode='" + events[i].tmCode + "' " + (events[i].stylestr != undefined ? "style='" + events[i].stylestr + "'" : "") + ">" + unescape(events[i].title) + "</span>" +
				"</a>" +
                                //					"<div class='ui-resizable-handle ui-resizable-e''></div>"
                                //					+
			"</div>";
                            }
                        }
                    }



                }
            }
            //            $.each(genEvents, function(i, n) {
            //                alert(n);
            //            });
        }
        return html;
    }
    //初始化已"月"为单位的，备注信息
    function GetMonthsEventsHtml(element, options, editEventCache) {
        var events = null, html = '', dateStart, dateEnd, // dateMonthStart, dateMonthEnd
        left = 0,
        width = 0, genEvents = new Array(),
        d = element.data('startDate');
        if (d != undefined) {
            dateStart = cloneDate(parseDate(d));
        }
        else {
            dateStart = cloneDate(parseDate(options.dateStart));
        }
        dateEnd = addDays(addMonths(cloneDate(dateStart), options.monthCount), -1);
        //        dateMonthStart = dateStart.getMonth() + 1;
        //        dateMonthEnd = dateEnd.getMonth() + 1;
        if (editEventCache != undefined) {
            events = element.data('EditEventCache');
        }
        else if (options.events != undefined && options.events.length > 0) {
            events = options.events;
        }
        if (events != null && events != undefined && events.length > 0) {
            for (var i = 0; i < events.length; i++) {
                var eventStartDate = parseDate(events[i].start); //事件开始日期
                var eventEndDate = parseDate(events[i].end); //事件结束日期
                if (dayDiff(eventEndDate, dateStart) >= 0 &&
                    dayDiff(dateEnd, eventStartDate) >= 0) {//在月份范围之内的事件

                    var startIndex = endIndex = -1;
                    var usedays = 1; //经过天数
                    var usemonth = 1; //经过月数
                    if (eventEndDate != undefined && eventEndDate != '') {
                        eventEndDate = parseDate(eventEndDate);
                        endIndex = dayDiff(eventEndDate, dateStart);
                    }
                    startIndex = dayDiff(eventStartDate, dateStart);
                    if (startIndex < 0 && endIndex >= 0) {//时间间隔在开始日期当中
                        startIndex = 0;
                    }

                    if (endIndex >= dayDiff(dateEnd, dateStart)) {//时间间隔在结束日期当中
                        endIndex = dayDiff(dateEnd, dateStart) - 1;
                    }
                    if (startIndex >= 0 && startIndex < dayDiff(dateEnd, dateStart)) {
                        if (endIndex >= 0) {
                            usedays = endIndex - startIndex + 1;
                        }
                        //if (startIndex > 0)
                        //{ usedays += eventStartDate.getDate() - 1; }
                        //width = cellWidth * (parseInt(usedays / 30) + (usedays % 30 > 0 ? 1 : 0)) - cellSide * 2;
                        //usemonth=
                        var sMonth = (startIndex == 0 ? dateStart : eventStartDate);
                        var eMonth = addDays(cloneDate(sMonth), usedays - 1);
                        width = cellWidth * (getMonthCha(sMonth, eMonth) + 1);
                        left = cellWidth * (parseInt((startIndex + 1) / 30) + ((startIndex + 1) % 30 > 0 ? 1 : 0) - 1) + cellSide;
                        left = (left < 0 ? cellSide : left);
                        var splitWeekCount = width / cellWidth - 1;
                        for (var w = 0; w <= splitWeekCount; w++) {
                            left += (w == 0 ? 0 : cellWidth);
                            if (left < options.monthCount * cellWidth) {
                                html +=
			"<div class='fc-event fc-event-hori' style='position:absolute;z-index:" + ((events[i].isMilestone + '').toLowerCase() == 'true' ? '7;height:31px;' : '8') + ";top:" + top + "px;left:" + left + "px;width:" + cellWidth + "px;'>" +
				"<a " + (events[i].stylestr != undefined ? ("class='" + events[i].stylestr + "'") : "") + ">" +
					"<span class='fc-event-title' startDate='" + events[i].start + "' endDate='" + (events[i].end != undefined ? events[i].end : events[i].start) + "' tmcode='" + events[i].tmCode + "' " + (events[i].stylestr != undefined ? "style='" + events[i].stylestr + "'" : "") + "'>" + unescape(events[i].title) + "</span>" +
				"</a>" +
                                //					"<div class='ui-resizable-handle ui-resizable-e''></div>"
                                //					+
			"</div>";
                            }
                        }
                    }
                    //                    var eventStartMonth = eventStartDate.getMonth() + 1; //事件开始月份
                    //                    var eventEndMonth = eventEndDate.getMonth() + 1; //事件结束月份
                    //                    if (eventStartMonth == eventEndMonth) { //事件都在一个月内
                    //                        genEvents.push(events[i]);
                    //                        continue;
                    //                    }
                    //                    else {//事件在几个月内
                    //                        var arrayStartDate = arrayEndDate = null;
                    //                        while (eventEndDate != arrayEndDate) {
                    //                            if (arrayEndDate == null) {
                    //                                arrayStartDate = eventStartDate;
                    //                            }
                    //                            else {
                    //                                arrayStartDate = addDays(cloneDate(arrayEndDate), 1);
                    //                            }
                    //                            var temp = cloneDate(arrayStartDate);
                    //                            temp.setDate(1);
                    //                            arrayEndDate = addMonths(temp, 1);
                    //                            addDays(arrayEndDate, -1);
                    //                            if (dayDiff(eventEndDate, arrayEndDate) <= 0) {
                    //                                arrayEndDate = eventEndDate;
                    //                            }

                    //                            if (dayDiff(arrayStartDate, dateStart) >= 0 &&
                    //                               dayDiff(dateEnd, arrayStartDate) >= 0) {//满足当前指定月试图范围内，添加到数组genEvents中
                    //                                //alert(arrayStartDate + '|' + arrayEndDate);
                    //                                var genEventsStart = arrayStartDate.getFullYear() + '-' + zeroPad(arrayStartDate.getMonth() + 1) + '-' + zeroPad(arrayStartDate.getDate());
                    //                                var genEventsEnd = arrayEndDate.getFullYear() + '-' + zeroPad(arrayEndDate.getMonth() + 1) + '-' + zeroPad(arrayEndDate.getDate());
                    //                                genEvents.push({ title: htmlEscape(events[i].title), start: genEventsStart, end: genEventsEnd });
                    //                            }
                    //                        }
                    //                    }
                }
            }
            //            $.each(genEvents, function(i, n) {
            //                alert(n);
            //            });
        }
        return html;
    }
    //初始化已"天"为单位的，备注信息
    function GetDaysEventsHtml(element, options, editEventCache) {
        var events = null, html = '',
        left = 0,
        width = 0;
        if (editEventCache != undefined) {
            events = element.data('EditEventCache');
        }
        else if (options.events != undefined && options.events.length > 0) {
            events = options.events;
        }
        if (events != null && events != undefined && events.length > 0) {
            for (var i = 0; i < events.length; i++) {
                var eventStartDate = parseDate(events[i].start);
                var eventEndDate = events[i].end;
                var startIndex = endIndex = -1;
                var usedays = 1; //经过天数
                var dataStart = parseDate(options.dateStart);
                if (element.data('startDate') != undefined) {
                    dataStart = parseDate(element.data('startDate'));
                }
                if (eventEndDate != undefined && eventEndDate != '') {
                    eventEndDate = parseDate(eventEndDate);
                    endIndex = dayDiff(eventEndDate, dataStart);
                }
                startIndex = dayDiff(eventStartDate, dataStart);
                if (startIndex < 0 && endIndex >= 0) {//时间间隔在开始日期当中
                    startIndex = 0;
                }
                if (endIndex >= options.dateCount) {//时间间隔在结束日期当中
                    endIndex = options.dateCount - 1;
                }
                if (startIndex >= 0 && startIndex < options.dateCount) {
                    if (endIndex >= 0) {
                        usedays = endIndex - startIndex + 1;
                    }
                    width = cellWidth * usedays - cellSide * 2;
                    left = cellWidth * startIndex + cellSide;
                    html +=
			"<div class='fc-event fc-event-hori' style='position:absolute;z-index:" + ((events[i].isMilestone + '').toLowerCase() == 'true' ? '7;height:31px;' : '8') + ";top:" + top + "px;left:" + left + "px;width:" + width + "px;' title='" + unescape(events[i].title) + "'>" +
				"<a " + (events[i].stylestr != undefined ? ("class='" + events[i].stylestr + "'") : "") + ">" +
					"<span class='fc-event-title' startDate='" + events[i].start + "' endDate='" + (events[i].end != undefined ? events[i].end : events[i].start) + "' tmcode='" + events[i].tmCode + "' " + (events[i].stylestr != undefined ? "style='" + events[i].stylestr + "'" : "") + "'>" + unescape(events[i].title) + "</span>" +
				"</a>" +
                    //					"<div class='ui-resizable-handle ui-resizable-e''></div>"
                    //					+
			"</div>";
                }
            }
        }
        return html;
    }
    //初始化,备注信息
    function renderEvent(element, options) {
        var html = '';
        var editEventCache = element.data('EditEventCache');
        switch (options.view) {
            case 'day':
                html += GetDaysEventsHtml(element, options);
                if (editEventCache != undefined && editEventCache.length > 0) {
                    html += GetDaysEventsHtml(element, options, editEventCache);
                }
                break;
            case 'week':
                html += GetWeeksEventsHtml(element, options);
                if (editEventCache != undefined && editEventCache.length > 0) {
                    html += GetWeeksEventsHtml(element, options, editEventCache);
                }
                break;
            case 'month':
                html += GetMonthsEventsHtml(element, options);
                if (editEventCache != undefined && editEventCache.length > 0) {
                    html += GetMonthsEventsHtml(element, options, editEventCache);
                }
                break;
            default: break;
        }
        return html;
    }
    //初始化表格
    function renderGrid(element, options) {
        var divObj = $("<div class='fcTable-" + options.view + "' />").appendTo(element);
        //var table = $("<table class='fcTable-" + options.view + "' unselectable='on'/>").appendTo(element);
        //var tr = $("<tr id='tr" + options.rowName + "' class=''/>").appendTo(table);
        //element.attr('class', 'fcTable-' + options.view);
        var p = $("<div name='eventArea' style='position:absolute;z-index:8;top:0;left:0'/>").appendTo(element);
        switch (options.view) {
            case 'day':
                element.width(cellWidth * options.dateCount);
                divObj.width(cellWidth * options.dateCount);
                break;
            case 'week':
                if (element.data('startDate') != undefined) {
                    var d = parseDate(element.data('startDate'));
                    while (d.getDay() != 1) {
                        d = addDays(cloneDate(d), -1);
                    }
                    element.data('startDate', setYMD(d, d.getFullYear(), d.getMonth(), 1));
                }
                else {
                    var d = parseDate(options.dateStart);
                    while (d.getDay() != 1) {
                        d = addDays(cloneDate(d), -1);
                    }
                    element.data('startDate', setYMD(d, d.getFullYear(), getMonth(), 1));
                }
                element.width(cellWidth * options.weekCount);
                divObj.width(cellWidth * options.weekCount);
                break;
            case 'month':
                if (element.data('startDate') != undefined) {
                    var d = parseDate(element.data('startDate'));
                    element.data('startDate', setYMD(d, d.getFullYear(), d.getMonth(), 1));
                }
                else {
                    var d = parseDate(options.dateStart);
                    element.data('startDate', setYMD(d, d.getFullYear(), getMonth(), 1));
                }
                element.width(cellWidth * options.monthCount);
                divObj.width(cellWidth * options.monthCount);
                break;
            default: break;
        }
        p.append(renderEvent(element, options));
    }



    /* Date Math
    -----------------------------------------------------------------------------*/

    var DAY_MS = 86400000,
	HOUR_MS = 3600000,
	MINUTE_MS = 60000;

    function addYears(d, n, keepTime) {
        d.setFullYear(d.getFullYear() + n);
        if (!keepTime) {
            clearTime(d);
        }
        return d;
    }

    function addMonths(d, n, keepTime) { // prevents day overflow/underflow
        if (+d) { // prevent infinite looping on invalid dates
            var m = d.getMonth() + n,
			check = cloneDate(d);
            check.setDate(1);
            check.setMonth(m);
            d.setMonth(m);
            if (!keepTime) {
                clearTime(d);
            }
            while (d.getMonth() != check.getMonth()) {
                d.setDate(d.getDate() + (d < check ? 1 : -1));
            }
        }
        return d;
    }

    function addDays(d, n, keepTime) { // deals with daylight savings
        if (+d) {
            var dd = d.getDate() + n,
			check = cloneDate(d);
            check.setHours(9); // set to middle of day
            check.setDate(dd);
            d.setDate(dd);
            if (!keepTime) {
                clearTime(d);
            }
            fixDate(d, check);
        }
        return d;
    }
    fc.addDays = addDays;

    function fixDate(d, check) { // force d to be on check's YMD, for daylight savings purposes
        if (+d) { // prevent infinite looping on invalid dates
            while (d.getDate() != check.getDate()) {
                d.setTime(+d + (d < check ? 1 : -1) * HOUR_MS);
            }
        }
    }

    function addMinutes(d, n) {
        d.setMinutes(d.getMinutes() + n);
        return d;
    }

    function clearTime(d) {
        d.setHours(0);
        d.setMinutes(0);
        d.setSeconds(0);
        d.setMilliseconds(0);
        return d;
    }

    function cloneDate(d, dontKeepTime) {
        if (dontKeepTime) {
            return clearTime(new Date(+d));
        }
        return new Date(+d);
    }
    fc.cloneDate = cloneDate;

    function zeroDate() { // returns a Date with time 00:00:00 and dateOfMonth=1
        var i = 0, d;
        do {
            d = new Date(1970, i++, 1);
        } while (d.getHours()); // != 0
        return d;
    }

    function skipWeekend(date, inc, excl) {
        inc = inc || 1;
        while (!date.getDay() || (excl && date.getDay() == 1 || !excl && date.getDay() == 6)) {
            addDays(date, inc);
        }
        return date;
    }

    function dayDiff(d1, d2) { // d1 - d2
        return Math.round((cloneDate(d1, true) - cloneDate(d2, true)) / DAY_MS);
    }

    function setYMD(date, y, m, d) {
        if (y !== undefined && y != date.getFullYear()) {
            date.setDate(1);
            date.setMonth(0);
            date.setFullYear(y);
        }
        if (m !== undefined && m != date.getMonth()) {
            date.setDate(1);
            date.setMonth(m);
        }
        if (d !== undefined) {
            date.setDate(d);
        }
    }
    /* Date Parsing
    -----------------------------------------------------------------------------*/

    var parseDate = fc.parseDate = function(s) {
        if (typeof s == 'object') { // already a Date object
            return s;
        }
        if (typeof s == 'number') { // a UNIX timestamp
            return new Date(s * 1000);
        }
        if (typeof s == 'string') {
            if (s.match(/^\d+$/)) { // a UNIX timestamp
                return new Date(parseInt(s) * 1000);
            }
            return parseISO8601(s, true) || (s ? new Date(s) : null);
        }
        // TODO: never return invalid dates (like from new Date(<string>)), return null instead
        return null;
    };

    var parseISO8601 = fc.parseISO8601 = function(s, ignoreTimezone) {
        // derived from http://delete.me.uk/2005/03/iso8601.html
        // TODO: for a know glitch/feature, read tests/issue_206_parseDate_dst.html
        var m = s.match(/^([0-9]{4})(-([0-9]{2})(-([0-9]{2})([T ]([0-9]{2}):([0-9]{2})(:([0-9]{2})(\.([0-9]+))?)?(Z|(([-+])([0-9]{2}):([0-9]{2})))?)?)?)?$/);
        if (!m) {
            return null;
        }
        var date = new Date(m[1], 0, 1),
		check = new Date(m[1], 0, 1, 9, 0),
		offset = 0;
        if (m[3]) {
            date.setMonth(m[3] - 1);
            check.setMonth(m[3] - 1);
        }
        if (m[5]) {
            date.setDate(m[5]);
            check.setDate(m[5]);
        }
        fixDate(date, check);
        if (m[7]) {
            date.setHours(m[7]);
        }
        if (m[8]) {
            date.setMinutes(m[8]);
        }
        if (m[10]) {
            date.setSeconds(m[10]);
        }
        if (m[12]) {
            date.setMilliseconds(Number("0." + m[12]) * 1000);
        }
        fixDate(date, check);
        if (!ignoreTimezone) {
            if (m[14]) {
                offset = Number(m[16]) * 60 + Number(m[17]);
                offset *= m[15] == '-' ? 1 : -1;
            }
            offset -= date.getTimezoneOffset();
        }
        return new Date(+date + (offset * 60 * 1000));
    };
    /* Misc Utils
    -----------------------------------------------------------------------------*/

    var dayIDs = ['sun', 'mon', 'tue', 'wed', 'thu', 'fri', 'sat'];

    function zeroPad(n) {
        return (n < 10 ? '0' : '') + n;
    }

    function smartProperty(obj, name) { // get a camel-cased/namespaced property of an object
        if (obj[name] !== undefined) {
            return obj[name];
        }
        var parts = name.split(/(?=[A-Z])/),
		i = parts.length - 1, res;
        for (; i >= 0; i--) {
            res = obj[parts[i].toLowerCase()];
            if (res !== undefined) {
                return res;
            }
        }
        return obj[''];
    }

    function htmlEscape(s) {
        return s.replace(/&/g, '&amp;')
		.replace(/</g, '&lt;')
		.replace(/>/g, '&gt;')
		.replace(/'/g, '&#039;')
		.replace(/"/g, '&quot;')
		.replace(/\n/g, '<br />');
    }



    function HorizontalPositionCache(getElement) {

        var t = this,
		elements = {},
		lefts = {},
		rights = {};

        function e(i) {
            return elements[i] = elements[i] || getElement(i);
        }

        t.left = function(i) {
            return lefts[i] = lefts[i] === undefined ? e(i).position().left : lefts[i];
        };

        t.right = function(i) {
            return rights[i] = rights[i] === undefined ? t.left(i) + e(i).width() : rights[i];
        };

        t.clear = function() {
            elements = {};
            lefts = {};
            rights = {};
        };

    }
})(jQuery);
function zeroPad(n) {
    return (n < 10 ? '0' : '') + n;
}
//+---------------------------------------------------  
//| 日期计算  
//+---------------------------------------------------  
Date.prototype.DateAdd = function(strInterval, Number) {
    var dtTmp = this;
    switch (strInterval) {
        case 's': return new Date(Date.parse(dtTmp) + (1000 * Number));
        case 'n': return new Date(Date.parse(dtTmp) + (60000 * Number));
        case 'h': return new Date(Date.parse(dtTmp) + (3600000 * Number));
        case 'd': return new Date(Date.parse(dtTmp) + (86400000 * Number));
        case 'w': return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));
        case 'q': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'm': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case 'y': return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
    }
}
//+---------------------------------------------------  
//| 比较日期差 dtEnd 格式为日期型或者 有效日期格式字符串  
//+---------------------------------------------------  
Date.prototype.DateDiff = function(strInterval, dtEnd) {
    var dtStart = this;
    if (typeof dtEnd == 'string')//如果是字符串转换为日期型  
    {
        dtEnd = StringToDate(dtEnd);
    }
    switch (strInterval) {
        case 's': return parseInt((dtEnd - dtStart) / 1000);
        case 'n': return parseInt((dtEnd - dtStart) / 60000);
        case 'h': return parseInt((dtEnd - dtStart) / 3600000);
        case 'd': return parseInt((dtEnd - dtStart) / 86400000);
        case 'w': return parseInt((dtEnd - dtStart) / (86400000 * 7));
        case 'm': return (dtEnd.getMonth() + 1) + ((dtEnd.getFullYear() - dtStart.getFullYear()) * 12) - (dtStart.getMonth() + 1);
        case 'y': return dtEnd.getFullYear() - dtStart.getFullYear();
    }
}
//+---------------------------------------------------  
//| 取得当前日期所在周是一年中的第几周
//+---------------------------------------------------
function GetTotalDays(y, m, d, ds) {
    var totalDays = 0;
    var lastyear = new Date(y, m, d - totalDays * 7 + (7 - ds)).getFullYear();
    while (lastyear == y) {
        totalDays++;
        lastyear = new Date(y, m, d - totalDays * 7 + (7 - ds)).getFullYear();
    }
    return totalDays;
}
//计算月份差
function getMonthCha(dt1, dt2) {
    return (dt2.getYear() * 12 + dt2.getMonth()) -
                  (dt1.getYear() * 12 + dt1.getMonth());
} 
