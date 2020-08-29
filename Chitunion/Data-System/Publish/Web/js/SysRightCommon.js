//全选反选取消
function selectCheckBoxDelAll(objName, showType) {
    var delAllObj = document.getElementsByName(objName);
    if (showType == 1) {
        //全选
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = true;
        }
    }
    else if (showType == 2) {
        //反选
        for (var i = 0; i < delAllObj.length; i++) {
            if (delAllObj[i].checked) {
                delAllObj[i].checked = false;
            }
            else {
                delAllObj[i].checked = true;
            }
        }
    }
    else if (showType == 3) {
        //取消
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = false;
        }
    }
}
function selectCheckBoxDelAllCheck(obj, objName) {
    var delAllObj = document.getElementsByName(objName);
    if (obj.checked == true) {
        //全选
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = true;
        }
    }
    else {
        //全选
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = false;
        }
    }
}

/*节选自jQueryString v2.0.2*/
(function ($) {
    $.unserialise = function (Data) {
        var Data = Data.split("&");
        var Serialised = new Array();
        $.each(Data, function () {
            var Properties = this.split("=");
            Serialised[Properties[0]] = Properties[1];
        });
        return Serialised;
    };
})(jQuery);


/*
* 异步调用--公用方法,需要引用JQuery
* beforeSend没有内容，可以传入null
* Add=Masj,Date=20091207
*/
function AjaxPost(url, postBody, beforeSend, CallbackName) {
    $.ajax({
        type: "POST",
        url: url,
        data: postBody,
        beforeSend: beforeSend,
        success: CallbackName,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中
            // 只有一个会包含信息
            alert(XMLHttpRequest.responseText);
        }
    });
}

/*
* 异步调用--公用方法,需要引用JQuery
* beforeSend没有内容，可以传入null
* Add=Masj,Date=20091215
*/
function AjaxGet(url, postBody, beforeSend, CallbackName) {
    $.ajax({
        type: "GET",
        url: url,
        data: postBody,
        beforeSend: beforeSend,
        success: CallbackName,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中
            // 只有一个会包含信息
            alert(XMLHttpRequest.responseText);
        }
    });
}

//关闭弹出层
/**
@name 弹出层的名字
@, isCancel 是否是取消之类的操作，默认为true
*/
function Close(name, effectiveAction) {
    $.closePopupLayer(name, effectiveAction);
}

//trim
function trim(s) {
    return s.replace(/(^[\s\u3000]*)|([\s\u3000]*$)/g, "");
}

/**
* @desc  JQuery扩展，将json字符串转换为对象，需要引用类库JQuery
* @param   json字符串
* @return 返回object,array,string等对象
* @Add=Masj, Date: 2009-12-07
*/
jQuery.extend(
 {
     evalJSON: function (strJson) {
         if ($.trim(strJson) == '')
             return '';
         else
             return eval("(" + strJson + ")");
     }
 });

 function validateEdit(ids) {
     var b = false;
     var ar = document.getElementsByName(ids);
     for (var ii = 0; ii < ar.length; ii++) {
         if (ar[ii].type == "checkbox") {
             if (ar[ii].checked) {
                 b = true;
             }
         }
     }
     if (b) {
         return b;

     }
     else {
         alert("请至少选择一项!");
         return false;
     }
 }

 function validateDel(ids) {
     var b = false;
     var ar = document.getElementsByName(ids);
     for (var ii = 0; ii < ar.length; ii++) {
         if (ar[ii].type == "checkbox") {
             if (ar[ii].checked) {
                 b = true;
             }
         }
     }
     if (b) {
         return confirm("确定要删除吗?");

     }
     else {
         alert("请至少选择一项!");
         return false;
     }
 }