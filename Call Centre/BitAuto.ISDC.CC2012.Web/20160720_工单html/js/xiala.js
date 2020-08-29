
/*
---根据div+css，实现下拉列表及省份城市列表（省份城市数据源，依赖于JSON【/js/Enum/Area2.js】）
---add=masj，date=2016-07-25
---引用此脚本，需要JQuery类库支持
*/
var DIVSelect = (function () {
    var totalStyle = 'bor_kun'; //div总样式
    var leftStyle = 'borleft2'; //总样式下面嵌套3个div，且3个div之间是平级关系，这是左侧div样式
    var centerStyle = 'borcenter2 iebor'; //总样式下面嵌套3个div，且3个div之间是平级关系，这是中间div样式
    var subCenterStyle = 'cui_selectBox'; //居中样式下，嵌套Div中的样式
    var rightStyle = 'borright2'; //总样式下面嵌套3个div，且3个div之间是平级关系，这是右侧div样式
    var currentSelectStyle = 'cui_selectTitle4'; //当前div中（select控件）的样式colorgray
    var optionStyle = 'cui_selectSub4'; //下拉列表下，option所在Div的样式
    var slideSpeed = 200; //下拉列表滑动动画延迟（单位：毫秒）
    var currentSelectSpanStyle='sf_xz';

    var centerStyleByProvinceCity = 'borcenter2 borcenter21 iebor'; //总样式下面嵌套3个div，且3个div之间是平级关系，这是中间div样式
    var currentSelectStyleByProvinceCity = 'cui_selectTitle'; //当前div中（select控件）的样式
    var optionStyleByProvinceCity = 'cui_selectSub'; //下拉列表下，option所在Div的样式
    var citycountyStyle = 'bor_kun2'; //城市、区县2级下拉特殊样式


    //方法：初始化控件
    //参数为：objID=控件div的ID，defaultMsg=控件的默认值，
    //dataArray=Json类型，如：[{ id: '1', name: '719企业' }, { id: '2', name: '258公司' }, { id: '3', name: '360杀毒软件'}];
    var Init = function (objID, dataArray, defaultMsg, selectedCallback) {
        var divObj = $("div[id='" + objID + "']");

        if (divObj.size() == 1) {
            var divObj_Style = $("div[id='" + objID + "']").attr('style');
            divObj_Style = (divObj_Style ? divObj_Style : ''); //取出div中的样式，准备移动到生成后的div中
            $("div[id='" + objID + "']").removeAttr('style'); //移除页面中当前div的样式style

            if (typeof (defaultMsg) == 'undefined' || defaultMsg == null || defaultMsg == '') {
                var defaultMsg = '请选择';
            }
            divObj.html($('<span>').addClass(currentSelectSpanStyle).html(defaultMsg))
                  .addClass(currentSelectStyle)
                  .attr('rel', -1);
            var optionDiv = $('<div>').addClass(optionStyle).hide();
            //遍历下拉选项
            for (var data in dataArray) {
                var aObj = $('<a>').attr('href', 'javascript:void(0);')
                                   .bind('click', function () {
                                       _optionClick($(this), divObj, selectedCallback);
                                       //                                       var id = $(this).attr('rel');
                                       //                                       var name = $(this).html();
                                       //                                       divObj.attr('rel', id);
                                       //                                       divObj.html(name);
                                       //                                       if (selectedCallback) {
                                       //                                           var item = {};
                                       //                                           item.id = id;
                                       //                                           item.name = name;
                                       //                                           selectedCallback(item);
                                       //                                       }
                                   });
                aObj.html(dataArray[data].name);
                aObj.attr('rel', dataArray[data].id);
                //生成html代码
                optionDiv.append($('<p>').append(aObj));
            }
            //加载默认选项
            var aObjFirst = $('<a>').attr('href', 'javascript:void(0);')
                                   .bind('click', function () {
                                       _optionClick($(this), divObj, selectedCallback);
                                       //                                       divObj.attr('rel', $(this).attr('rel'));
                                       //                                       divObj.html($(this).html());
                                   }); //alert(defaultMsg);
            aObjFirst.html(defaultMsg);
            aObjFirst.attr('rel', -1);
            optionDiv.prepend($('<p>').append(aObjFirst));

            //整理html代码，附加到当前页面中
            divObj.wrap($('<div>').addClass(subCenterStyle))//包裹div样式
                  .parent().wrap($('<div>').addClass(centerStyle))//包裹div.borcenter2
                  .parent().wrap($('<div>').addClass(totalStyle).attr('style', divObj_Style))//包裹div.bor_kun
                  .before($('<div>').addClass(leftStyle))//添加左边圆角样式
                  .after($('<div>').addClass(rightStyle))//添加右边圆角样式
                  .bind('click', function (event) {
                      optionDiv.toggle(slideSpeed); //切换下拉列表显示或隐藏
                  });

            divObj.after(optionDiv); //添加option内容到select中
        }
    };
    //方法：获取下拉控件当前选中的值
    var GetVal = function (objID, level) {
        var obj = _getDivObj(objID, level);
        if (obj && obj != null) {
            return obj.attr('rel');
        }
        return '';
    };
    //方法：获取下拉控件当前选中的内容
    var GetName = function (objID, level) {
        //return $("div[id='" + objID + "']").html();
        var obj = _getDivObj(objID, level);
        if (obj && obj != null) {
            return obj.find('span.sf_xz').html();
        }
        return '';
    };

    var _getDivObj = function (objID, level) {
        var obj = $("div[id='" + objID + "']");
        if (level && level > 0) {
            switch (level) {
                case 1:
                    break;
                case 2:
                    obj = $("div[id='" + objID + "_city']");
                    break;
                case 3:
                    obj = $("div[id='" + objID + "_county']");
                    break;
                default:
                    obj = null;
                    break;
            }
        }
        return obj;
    };
    //方法：设置下拉控件的值
    var SetVal = function (objID, val) {
        var selectObj = $("div[id='" + objID + "']");
        if (selectObj.size() == 1 && val) {
            var findOjb = selectObj.parent().find("p > a[rel='" + val + "']");
            if (findOjb.size() == 1) {
                selectObj.attr('rel', val);
                selectObj.find('span.sf_xz').html(findOjb.html());
                return true;
            }
        }
        return false;
    };

    //方法：控件不可用
    var Disabled = function (objID) {
        var selectObj = $("div[id='" + objID + "'],div[id='" + objID + "_city'],div[id='" + objID + "_county']");
        if (selectObj.size() > 0) {
            selectObj.parent().parent().unbind('click');
            selectObj.find('span.' + currentSelectSpanStyle).addClass('colorgray');
        }
    };
    //方法：控件可用
    var Enabled = function (objID) {
        _enabled($("div[id='" + objID + "']"));
        _enabled($("div[id='" + objID + "_city']"));
        _enabled($("div[id='" + objID + "_county']"));
    };
    //方法：控件可用与否
    var SetEnabledOrDisabled = function (objID, enab) {
        if (enab) {
            Enabled(objID);
        }
        else {
            Disabled(objID);
        }
    }
    var _enabled = function (selectObj) {
        if (selectObj.size() == 1) {
            selectObj.parent().parent().unbind('click').bind('click', function () {
                selectObj.next('div').toggle(slideSpeed); //切换下拉列表显示或隐藏
            });
            selectObj.find('span.' + currentSelectSpanStyle).removeClass('colorgray');
        }
    };

    var _optionClick = function (currentOption, divObj, clickCallBack, level) {
        var id = currentOption.attr('rel');
        var name = currentOption.html();
        //alert(divObj.html());
        var itemOld = {};
        itemOld.id = divObj.attr('rel');
        itemOld.name = divObj.html();

        divObj.attr('rel', id);
        divObj.find('span.sf_xz').html(name);

        var itemNew = {};
        itemNew.id = id;
        itemNew.name = name;
        if (level && level > 0) {
            itemNew.level = level;
            itemOld.level = level;
        }
        if (clickCallBack && itemNew.id != itemOld.id) {
            clickCallBack(itemNew, itemOld);
        }
    };

    //内部方法：初始化省份城市
    var initProvince = function (divObj, dataArray, defaultMsg, clickCallBack, addClassName) {
        if (divObj.size() == 1) {
            var divObj_Style = divObj.attr('style');
            divObj_Style = (divObj_Style ? divObj_Style : ''); //取出div中的样式，准备移动到生成后的div中
            divObj.removeAttr('style'); //移除页面中当前div的样式style

            if (typeof (defaultMsg) == 'undefined' || defaultMsg == null || defaultMsg == '') {
                var defaultMsg = '请选择';
            }
            divObj.html($('<span>').addClass(currentSelectSpanStyle).html(defaultMsg))
                  .addClass(currentSelectStyleByProvinceCity)
                  .attr('rel', -1);
            var optionDiv = $('<div>').addClass(optionStyleByProvinceCity).hide();
            //遍历下拉选项
            for (var data in dataArray) {
                var aObj = $('<a>').attr('href', 'javascript:void(0);')
                                   .bind('click', function () {
                                       _optionClick($(this), divObj, clickCallBack, 1);
                                   });
                aObj.html(dataArray[data].name);
                aObj.attr('rel', dataArray[data].id);
                //生成html代码
                optionDiv.append($('<p>').append(aObj));
            }
            //加载默认选项
            var aObjFirst = $('<a>').attr('href', 'javascript:void(0);')
                                   .bind('click', function () {
                                       _optionClick($(this), divObj, clickCallBack, 1);
                                   }); //alert(defaultMsg);
            aObjFirst.html(defaultMsg);
            aObjFirst.attr('rel', -1);
            optionDiv.prepend($('<p>').append(aObjFirst));

            //整理html代码，附加到当前页面中
            divObj.wrap($('<div>').addClass(subCenterStyle))//包裹div样式
                  .parent().wrap($('<div>').addClass(centerStyleByProvinceCity))//包裹div.borcenter2
                  .parent().wrap($('<div>').addClass(totalStyle + (addClassName && addClassName != '' ? ' ' + addClassName : '')).attr('style', divObj_Style))//包裹div.bor_kun
                  .before($('<div>').addClass(leftStyle))//添加左边圆角样式
                  .after($('<div>').addClass(rightStyle))//添加右边圆角样式
                  .unbind('click').bind('click', function (event) {
                      optionDiv.toggle(slideSpeed); //切换下拉列表显示或隐藏
                  });

            divObj.after(optionDiv); //添加option内容到select中
        }
    };

    var BindSubDataByPID = function (divObj, dataArray, clickCallBack, level) {
        var optionObj = divObj.next('div');
        if (optionObj && optionObj.size() == 1) {
            optionObj.find("p:has(a[rel!='-1'])").remove();
            for (var data in dataArray) {
                var aObj = $('<a>').attr('href', 'javascript:void(0);')
                                   .bind('click', function () {
                                       _optionClick($(this), divObj, clickCallBack, level);
                                       //                                       var id = $(this).attr('rel');
                                       //                                       var name = $(this).html();
                                       //                                       divObj.attr('rel', id);
                                       //                                       divObj.html(name);
                                       //                                       var item = {};
                                       //                                       item.id = id;
                                       //                                       item.name = name;
                                       //                                       item.level=
                                       //                                       if (clickCallBack) {
                                       //                                           clickCallBack($(this).attr('rel'));
                                       //                                       }
                                   });
                aObj.html(dataArray[data].name);
                aObj.attr('rel', dataArray[data].id);
                //生成html代码
                optionObj.append($('<p>').append(aObj));
            };
        }
    };

    var ClearSubDataByPid = function (divObj) {
        var optionObj = divObj.next('div');
        if (optionObj && optionObj.size() == 1) {
            optionObj.find("p:has(a[rel!='-1'])").remove();
            divObj.attr('rel', -1);
            divObj.find('span.sf_xz').html(optionObj.find("p > a[rel='-1']").html());
        }
    };

    //初始化省份城市控件
    var InitProvinceCity = function (objID, provinceID, cityID, countyID, selectedCallback) {
        var divObj = $("div[id='" + objID + "']");
        if (JSonData && JSonData.masterArea.length > 0 && divObj.size() == 1) {
            var divObj2 = $('<div id="' + objID + '_county">').insertAfter('#' + objID);
            var divObj3 = $('<div id="' + objID + '_city">').insertAfter('#' + objID);

            initProvince(divObj, JSonData.masterArea, '省/市', function (itemProvinceNew, itemProvinceOld) {
                ClearSubDataByPid(divObj3);
                ClearSubDataByPid(divObj2);
                if (selectedCallback) {//回传选择后回调函数
                    selectedCallback(itemProvinceNew, itemProvinceOld);
                }
                var cityArray = [];
                for (var i = 0; i < JSonData.masterArea.length; i++) {
                    if (JSonData.masterArea[i].id == itemProvinceNew.id) {
                        for (var j = 0; j < JSonData.masterArea[i].subArea.length; j++) {
                            cityArray.push({ "name": JSonData.masterArea[i].subArea[j].name, "id": JSonData.masterArea[i].subArea[j].id });
                        }
                    }
                }
                BindSubDataByPID(divObj3, cityArray, function (itemCityNew, itemCityOld) {
                    ClearSubDataByPid(divObj2);
                    if (selectedCallback) {//回传选择后回调函数
                        selectedCallback(itemCityNew, itemCityOld);
                    }
                    var countyArray = [];
                    for (var i = 0; i < JSonData.masterArea.length; i++) {
                        if (JSonData.masterArea[i].id == itemProvinceNew.id) {
                            for (var j = 0; j < JSonData.masterArea[i].subArea.length; j++) {
                                if (JSonData.masterArea[i].subArea[j].id == itemCityNew.id) {
                                    for (var m = 0; m < JSonData.masterArea[i].subArea[j].subArea2.length; m++) {
                                        countyArray.push({ "name": JSonData.masterArea[i].subArea[j].subArea2[m].name, "id": JSonData.masterArea[i].subArea[j].subArea2[m].id });
                                    }
                                }
                            }
                        }
                    }
                    BindSubDataByPID(divObj2, countyArray, function (itemCountyNew, itemCountyOld) {
                        if (selectedCallback) {//回传选择后回调函数
                            selectedCallback(itemCountyNew, itemCountyOld);
                        }
                    }, 3);
                }, 2);
            });
            initProvince(divObj3, null, '城市', function (cityID) {
                ClearSubDataByPid(divObj2);
            }, citycountyStyle);
            initProvince(divObj2, null, '区/县', null, citycountyStyle);

            SetValByProvinceCity(objID, provinceID, cityID, countyID);
        }
    };

    //给省份城市控件，进行复制操作
    var SetValByProvinceCity = function (objID, provinceID, cityID, countyID) {
        var divObj = $("div[id='" + objID + "']");
        if (JSonData && JSonData.masterArea.length > 0 && divObj.size() == 1) {
            //选定省份
            if (typeof (provinceID) != 'undefined' && provinceID > 0) {

                divObj.next('div').find("p > a[rel='" + provinceID + "']").triggerHandler('click');
                SetVal(objID, provinceID);
            }
            //选定城市
            if (typeof (cityID) != 'undefined' && cityID > 0) {
                var divObj_City = $("div[id='" + objID + "_city']");

                divObj_City.next('div').find("p > a[rel='" + cityID + "']").triggerHandler('click');
                SetVal(objID + '_city', cityID);
            }
            //选定区县
            if (typeof (countyID) != 'undefined' && countyID > 0) {
                var divObj_County = $("div[id='" + objID + "_county']");
                divObj_County.next('div').find("p > a[rel='" + countyID + "']").triggerHandler('click');
                SetVal(objID + '_county', countyID);
            }
        }
    };

    //解决鼠标在下拉控件之外点击后，自动隐藏option选项层逻辑
    $(document).unbind('click').bind("click", function (event) {
        var e = event || window.event;
        var elem = e.srcElement || e.target;
        var currentSelectDivObj = null;
        while (elem) {
            if (elem.className == subCenterStyle) {
                currentSelectDivObj = $(elem);
                break;
            }
            elem = elem.parentNode;
        }
        var selectObjArray = $('body div.' + subCenterStyle);
        if (currentSelectDivObj && currentSelectDivObj != null) {
            selectObjArray = selectObjArray.not(currentSelectDivObj);
        }
        selectObjArray.children('div.' + optionStyle + ',div.' + optionStyleByProvinceCity).hide();
    });

    return {
        Init: Init,
        GetVal: GetVal,
        GetName: GetName,
        SetVal: SetVal,
        Disabled: Disabled,
        Enabled: Enabled,
        SetEnabledOrDisabled: SetEnabledOrDisabled,
        InitProvinceCity: InitProvinceCity,
        SetValByProvinceCity: SetValByProvinceCity
    }
})();

