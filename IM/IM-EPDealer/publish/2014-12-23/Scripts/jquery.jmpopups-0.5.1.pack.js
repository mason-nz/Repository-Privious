﻿/**
* jmpopups
* Copyright (c) 2009 Otavio Avila (http://otavioavila.com)
* Licensed under GNU Lesser General Public License
* 
* @docs http://jmpopups.googlecode.com/
* @version 0.5.1
* 
*/


(function($) {
    var openedPopups = [];
    var popupLayerScreenLocker = false;
    var focusableElement = [];
    var setupJqueryMPopups = {
        screenLockerBackground: "#000",
        screenLockerOpacity: "0.5"
    };

    var speed = 'fast';

    $.setupJMPopups = function(settings) {
        setupJqueryMPopups = jQuery.extend(setupJqueryMPopups, settings);
        return this;
    }

    $.openPopupLayer = function(settings) {
        if (typeof (settings.name) != "undefined" && !checkIfItExists(settings.name)) {
            settings = jQuery.extend({
                width: "auto",
                height: "auto",
                parameters: {},
                target: "",
                success: function() { },
                error: function() { },
                beforeClose: function() { },
                afterClose: function() { },
                reloadSuccess: null,
                cache: false,
                draggable: true,
                popupMethod: 'Get'
            }, settings);
            loadPopupLayerContent(settings, true);
            return this;
        }
    }

    $.closePopupLayer = function(name, effectiveAction, cData) {
        var ea = false;
        if (effectiveAction && effectiveAction == true) { ea = true; }
        if (name) {
            for (var i = 0; i < openedPopups.length; i++) {
                if (openedPopups[i].name == name) {
                    var thisPopup = openedPopups[i];

                    openedPopups.splice(i, 1)

                    thisPopup.beforeClose(ea, cData);

                    $("#popupLayer_" + name).fadeOut(speed, function() {
                        var data = $("#popupLayer_" + name).children().not('.jmp-link-at-top,.jmp-link-at-bottom'); //获取弹出层中的DIV元素
                        var targetDivID = $("#popupLayer_" + name).data('targetDIVID'); //获取存储在DIV上的targetDIVID值
                        $("#popupLayer_" + name).remove();
                        if ($.trim(targetDivID) != '')
                            $("#" + targetDivID).html(data); //还原DIV中的内容
                        focusableElement.pop();

                        if (focusableElement.length > 0) {
                            $(focusableElement[focusableElement.length - 1]).focus();
                        }

                        thisPopup.afterClose(ea, cData);
                        hideScreenLocker(name);
                    });



                    break;
                }
            }

        } else {
            if (openedPopups.length > 0) {
                $.closePopupLayer(openedPopups[openedPopups.length - 1].name);
            }
        }

        return this;
    }

    $.reloadPopupLayer = function(name, callback) {
        if (name) {
            for (var i = 0; i < openedPopups.length; i++) {
                if (openedPopups[i].name == name) {
                    if (callback) {
                        openedPopups[i].reloadSuccess = callback;
                    }

                    loadPopupLayerContent(openedPopups[i], false);
                    break;
                }
            }
        } else {
            if (openedPopups.length > 0) {
                $.reloadPopupLayer(openedPopups[openedPopups.length - 1].name);
            }
        }

        return this;
    }

    function setScreenLockerSize() {
        if (popupLayerScreenLocker) {
            $('#popupLayerScreenLocker').height($(document).height() * 1 + "px"); //修改遮盖层高度，1.04倍，Modify=Masj，Date=2009-12-24
            $('#popupLayerScreenLocker').width($(document.body).outerWidth(true) + "px");
        }
    }

    function checkIfItExists(name) {
        if (name) {
            for (var i = 0; i < openedPopups.length; i++) {
                if (openedPopups[i].name == name) {
                    return true;
                }
            }
        }
        return false;
    }

    function showScreenLocker() {
        if ($("#popupLayerScreenLocker").length) {
            if (openedPopups.length == 1) {
                popupLayerScreenLocker = true;
                setScreenLockerSize();
                $('#popupLayerScreenLocker').fadeIn(speed);
            }

            if ($.browser.msie && $.browser.version < 7) {
                $("select:not(.hidden-by-jmp)").addClass("hidden-by-jmp hidden-by-" + openedPopups[openedPopups.length - 1].name).css("visibility", "hidden");
            }

            $('#popupLayerScreenLocker').css("z-index", parseInt(openedPopups.length == 1 ? 6999 : $("#popupLayer_" + openedPopups[openedPopups.length - 2].name).css("z-index")) + 1);
        } else {
            $("body").append("<div id='popupLayerScreenLocker'><!-- --></div>");
            $("#popupLayerScreenLocker").css({
                position: "absolute",
                background: setupJqueryMPopups.screenLockerBackground,
                left: "0",
                top: "0",
                opacity: setupJqueryMPopups.screenLockerOpacity,
                display: "none"
            });
            showScreenLocker();

            $("#popupLayerScreenLocker").click(function() {
                //$.closePopupLayer();
            });
        }
    }

    function hideScreenLocker(popupName) {
        if (openedPopups.length == 0) {
            screenlocker = false;
            $('#popupLayerScreenLocker').fadeOut(speed);
        } else {
            $('#popupLayerScreenLocker').css("z-index", parseInt($("#popupLayer_" + openedPopups[openedPopups.length - 1].name).css("z-index")) - 1);
        }

        if ($.browser.msie && $.browser.version < 7) {
            $("select.hidden-by-" + popupName).removeClass("hidden-by-jmp hidden-by-" + popupName).css("visibility", "visible");
        }
    }

    function setPopupLayersPosition(popupElement, animate) {
        if (popupElement) {
            if (popupElement.width() < $(window).width()) {
                var leftPosition = (document.documentElement.offsetWidth - popupElement.width()) / 2;
            } else {
                var leftPosition = document.documentElement.scrollLeft + 5;
            }

            if (popupElement.height() < $(window).height()) {
                var topPosition = document.documentElement.scrollTop + ($(window).height() - popupElement.height()) / 2;
            } else {
                var topPosition = document.documentElement.scrollTop + 5;
            }

            var positions = {
                left: leftPosition + "px",
                top: topPosition + "px"
            };

            if (!animate) {
                popupElement.css(positions);
            } else {
                popupElement.animate(positions, speed);
            }

            setScreenLockerSize();
        } else {
            for (var i = 0; i < openedPopups.length; i++) {
                setPopupLayersPosition($("#popupLayer_" + openedPopups[i].name), true);
            }
        }
    }

    function showPopupLayerContent(popupObject, newElement, data) {
        var idElement = "popupLayer_" + popupObject.name;

        if (newElement) {
            showScreenLocker();

            $("body").append("<div id='" + idElement + "'><!-- --></div>");

            $("#" + idElement).data('targetDIVID', popupObject.target); //存储原始DIV的ID

            var zIndex = parseInt(openedPopups.length == 1 ? 7000 : $("#popupLayer_" + openedPopups[openedPopups.length - 2].name).css("z-index")) + 2;
        } else {
            var zIndex = $("#" + idElement).css("z-index");
        }

        var popupElement = $("#" + idElement);

        popupElement.css({
            visibility: "hidden",
            width: popupObject.width == "auto" ? "" : popupObject.width + "px",
            height: popupObject.height == "auto" ? "" : popupObject.height + "px",
            position: "absolute",
            "z-index": zIndex
        });

        var linkAtTop = "<a href='#' class='jmp-link-at-top' style='position:absolute; left:-9999px; top:-1px;'>&nbsp;</a><input class='jmp-link-at-top' style='position:absolute; left:-9999px; top:-1px;' />";
        var linkAtBottom = "<a href='#' class='jmp-link-at-bottom' style='position:absolute; left:-9999px; bottom:-1px;'>&nbsp;</a><input class='jmp-link-at-bottom' style='position:absolute; left:-9999px; top:-1px;' />";

        popupElement.html(linkAtTop + data + linkAtBottom);

        setPopupLayersPosition(popupElement);

        popupElement.css("display", "none");
        popupElement.css("visibility", "visible");

        if (newElement) {
            if (popupObject.draggable && jQuery.fn.jqDrag) {//draggable
                popupElement.jqDrag('.openwindow h2:first');
            }
            popupElement.fadeIn(speed);
        } else {
            popupElement.show();
        }

        $("#" + idElement + " .jmp-link-at-top, " +
		  "#" + idElement + " .jmp-link-at-bottom").focus(function() {
		      $(focusableElement[focusableElement.length - 1]).focus();
		  });

        var jFocusableElements = $("#" + idElement + " a:visible:not(.jmp-link-at-top, .jmp-link-at-bottom), " +
								   "#" + idElement + " *:input:visible:not(.jmp-link-at-top, .jmp-link-at-bottom)");

        if (jFocusableElements.length == 0) {
            var linkInsidePopup = "<a href='#' class='jmp-link-inside-popup' style='position:absolute; left:-9999px;'>&nbsp;</a>";
            popupElement.find(".jmp-link-at-top").after(linkInsidePopup);
            focusableElement.push($(popupElement).find(".jmp-link-inside-popup")[0]);
        } else {
            jFocusableElements.each(function() {
                if (!$(this).hasClass("jmp-link-at-top") && !$(this).hasClass("jmp-link-at-bottom")) {
                    focusableElement.push(this);
                    return false;
                }
            });
        }

        $(focusableElement[focusableElement.length - 1]).focus();

        popupObject.success();

        if (popupObject.reloadSuccess) {
            popupObject.reloadSuccess();
            popupObject.reloadSuccess = null;
        }
    }

    function loadPopupLayerContent(popupObject, newElement) {
        if (newElement) {
            openedPopups.push(popupObject);
        }

        if (popupObject.target != "") {
            showPopupLayerContent(popupObject, newElement, $("#" + popupObject.target).html());
            $("#" + popupObject.target).html(''); //清空原始层内容
        } else {
            $.ajax({
                url: popupObject.url,
                data: popupObject.parameters,
                cache: popupObject.cache,
                dataType: "html",
                type: popupObject.popupMethod,
                beforeSend: function() {
                    $('body').append('<div style="width: 100%; height: 40px; top: 50%; left: 0px;position:absolute;visibility:visible;z-index:7002;" class="blue-loading"/>');
                },
                success: function(data) {
                    $('.blue-loading').remove();
                    showPopupLayerContent(popupObject, newElement, data);
                },
                error: popupObject.error
            });
        }
    }

    $(window).resize(function() {
        setScreenLockerSize();
        //setPopupLayersPosition();
    });

    $(document).keydown(function(e) {
        if (e.keyCode == 27) {
            $.closePopupLayer();
        }
    });







    /*
    *弹出警告信息窗口
    *需要2个图片，info.gif和title.gif
    *Add=Masj,Date=2010-01-11
    */
    $.jAlert = function(message, afterCloseCallBack) {
        var alertName = 'alertName' + Math.round(Math.random() * 1000000);
        if (!afterCloseCallBack) { afterCloseCallBack = function() { }; }
        var popupObject = {
            width: "auto",
            height: "auto",
            parameters: {},
            target: "",
            success: function() { },
            error: function() { },
            beforeClose: function() { },
            afterClose: afterCloseCallBack,
            reloadSuccess: null,
            cache: false,
            draggable: true,
            name: alertName
        };
        openedPopups[openedPopups.length] = popupObject;

        var divPopup_container = document.createElement("div");
        $(divPopup_container).css('-moz-background-clip', 'border')
               .css('-moz-background-inline-policy', 'continuous')
               .css('-moz-background-origin', 'padding')
               .css('-moz-border-radius-bottomleft', '5px')
               .css('-moz-border-radius-bottomright', '5px')
               .css('-moz-border-radius-topleft', '5px')
               .css('-moz-border-radius-topright', '5px')
               .css('background', '#FFFFFF none repeat scroll 0 0')
               .css('border', '5px solid #999999')
               .css('color', '#000000')
               .css('font-family', 'Arial,sans-serif')
               .css('font-size', '12px')
               .css('width', '400px')
               .addClass('openwindow');

        var h2 = document.createElement("h2");
        $(h2).css('cursor', 'move')
             .css('-moz-background-clip', 'border')
             .css('-moz-background-inline-policy', 'continuous')
             .css('-moz-background-origin', 'padding')
             .css('background', '#CCCCCC url(/images/title.gif) repeat-x scroll center top')
             .css('border-color', '#FFFFFF #FFFFFF #999999')
             .css('border-style', 'solid')
             .css('border-width', '1px')
             .css('color', '#666666')
             .css('font-size', '14px')
             .css('font-weight', 'bold')
             .css('line-height', '1.75em')
             .css('margin', '0')
             .css('padding', '0')
             .css('text-align', 'center');
        $(h2).text('提示对话框');

        var divPopup_content = document.createElement("div");
        $(divPopup_content).css('background-image', 'url(/images/info.gif)')
                           .css('-moz-background-clip', 'border')
                           .css('-moz-background-inline-policy', 'continuous')
                           .css('-moz-background-origin', 'padding')
                           .css('background', 'transparent url(/images/info.gif) no-repeat scroll 16px 16px')
                           .css('margin', '0')
                           .css('padding', '1.75em')

        var divPopup_message = document.createElement("div");
        $(divPopup_message).css('padding-left', '48px')
                           .css('padding-right', '48px')
                           .html(message);

        var divPopup_panel = document.createElement("div");
        $(divPopup_panel).css('margin', '1em 0 0 1em')
                         .css('text-align', 'center');

        var btnPopup_ok = '<input type="button" value=" 确定 " onclick="javascript:$.closePopupLayer(\'' + alertName + '\');" style="width: 50px; height: 22px;"/>';

        $(divPopup_panel).append(btnPopup_ok);
        $(divPopup_content).append($(divPopup_message))
                           .append($(divPopup_panel));

        $(divPopup_container).append($(h2))
	           .append($(divPopup_content));

        var divTemp = document.createElement("div");
        $(divTemp).append($(divPopup_container));

        showPopupLayerContent(popupObject, true, $(divTemp).html());
    }


    /*
    *弹出确认信息窗口
    *需要2个图片，info.gif和title.gif
    *Add=Masj,Date=2010-01-11
    */
    $.jConfirm = function(message, jConfirmAfterClose) {
        var alertName = 'alertName' + Math.round(Math.random() * 1000000);
        if (!jConfirmAfterClose) { jConfirmAfterClose = function() { }; }
        var popupObject = {
            width: "auto",
            height: "auto",
            parameters: {},
            target: "",
            success: function() { },
            error: function() { },
            beforeClose: function() { },
            afterClose: jConfirmAfterClose,
            reloadSuccess: null,
            cache: false,
            draggable: true,
            name: alertName
        };
        openedPopups[openedPopups.length] = popupObject;

        var divPopup_container = document.createElement("div");
        $(divPopup_container).css('-moz-background-clip', 'border')
               .css('-moz-background-inline-policy', 'continuous')
               .css('-moz-background-origin', 'padding')
               .css('-moz-border-radius-bottomleft', '5px')
               .css('-moz-border-radius-bottomright', '5px')
               .css('-moz-border-radius-topleft', '5px')
               .css('-moz-border-radius-topright', '5px')
               .css('background', '#FFFFFF none repeat scroll 0 0')
               .css('border', '5px solid #999999')
               .css('color', '#000000')
               .css('font-family', 'Arial,sans-serif')
               .css('font-size', '12px')
               .css('width', '400px')
               .addClass('openwindow');

        var h2 = document.createElement("h2");
        $(h2).css('cursor', 'move')
             .css('-moz-background-clip', 'border')
             .css('-moz-background-inline-policy', 'continuous')
             .css('-moz-background-origin', 'padding')
             .css('background', '#CCCCCC url(/images/title.gif) repeat-x scroll center top')
             .css('border-color', '#FFFFFF #FFFFFF #999999')
             .css('border-style', 'solid')
             .css('border-width', '1px')
             .css('color', '#666666')
             .css('font-size', '14px')
             .css('font-weight', 'bold')
             .css('line-height', '1.75em')
             .css('margin', '0')
             .css('padding', '0')
             .css('text-align', 'center');
        $(h2).text('确认对话框');

        var divPopup_content = document.createElement("div");
        $(divPopup_content).css('background-image', 'url(/images/important.gif)')
                           .css('-moz-background-clip', 'border')
                           .css('-moz-background-inline-policy', 'continuous')
                           .css('-moz-background-origin', 'padding')
                           .css('background', 'transparent url(/images/important.gif) no-repeat scroll 16px 16px')
                           .css('margin', '0')
                           .css('padding', '1.75em')

        var divPopup_message = document.createElement("div");
        $(divPopup_message).css('padding-left', '48px')
                           .css('padding-right', '48px')
                           .html(message);

        var divPopup_panel = document.createElement("div");
        $(divPopup_panel).css('margin', '1em 0 0 1em')
                         .css('text-align', 'center');

        var btnPopup_ok = '<input type="button" value=" 确定 " onclick="javascript:$.closePopupLayer(\'' + alertName + '\',true);" style="width: 50px; height: 22px;"/>';
        var btnPopup_cancel = '<input type="button" value=" 取消 " onclick="javascript:$.closePopupLayer(\'' + alertName + '\',false);" style="width: 50px; height: 22px;margin-left:15px;"/>';

        $(divPopup_panel).append(btnPopup_ok)
                         .append(btnPopup_cancel);

        $(divPopup_content).append($(divPopup_message))
                           .append($(divPopup_panel));

        $(divPopup_container).append($(h2))
	           .append($(divPopup_content));

        var divTemp = document.createElement("div");
        $(divTemp).append($(divPopup_container));

        showPopupLayerContent(popupObject, true, $(divTemp).html());
    }

    /*
    *弹出确认信息窗口，之后自动关闭
    *Add=Masj,Date=2010-08-25
    */
    $.jPopMsgLayer = function(msg, afterCloseCallBack, second) {
        $('#divPopMsg').remove();
        var objDiv = $('<div/>');
        objDiv.addClass('open_yinyin')
          .attr('id', 'divPopMsg')
          .hide()
          .html('<div class="open_cg">' +
            '<a class="close" onclick="javascript:$.closePopupLayer(\'PopMsgLayer\');$(\'#divPopMsg\').remove();">关闭</a>' +
            '<div class="opensccg">' +
                '<strong>' + msg + '</strong></div>' +
            '</div>').appendTo('body');
        $.openPopupLayer({
            name: 'PopMsgLayer',
            target: 'divPopMsg',
            afterClose: afterCloseCallBack
        });
        setTimeout(function() { $.closePopupLayer('PopMsgLayer'); }, second != undefined ? second * 1000 : 1000);
    }

})(jQuery);