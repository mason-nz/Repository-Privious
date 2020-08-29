
/*
* 绑定省份，需要引用Area.js文件
* Area.js文件是生成的
* Add=Masj, Date: 2009-12-07 
* Modify=Masj,Date:2017-03-16
*/
function BindProvince(SelectID, isShowQuanGuo) {
    if (JSonData && JSonData.masterArea.length > 0) {
        var masterObj = document.getElementById(SelectID);
        if (masterObj && masterObj.options) {
            masterObj.options.length = 0;
            masterObj.options[0] = new Option("覆盖区域 -- 省", -1);
            if (isShowQuanGuo && isShowQuanGuo == true) {
                masterObj.options[1] = new Option("全国", 0);
            }
            for (var i = 0; i < JSonData.masterArea.length; i++) {
                masterObj.options[masterObj.options.length] = new Option(JSonData.masterArea[i].name, JSonData.masterArea[i].id);
            }
        }
    }
}

/*
* 绑定城市，需要引用Area.js文件
* Area.js文件是生成的
* 参数provinceSelectID为省份ID，citySelectID为城市ID
* Add=Masj, Date: 2009-12-07 
*/
function BindCity(provinceSelectID, citySelectID) {
    var temp = document.getElementById(provinceSelectID); if (!temp) { return; }
    temp = temp.options[document.getElementById(provinceSelectID).selectedIndex]; if (!temp) { return; }
    var masterObjid = temp.value;
    if (masterObjid && masterObjid >= 0) {
        var subAreaObj = document.getElementById(citySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("覆盖区域 -- 市", -1);
        for (var i = 0; i < JSonData.masterArea.length; i++) {
            if (JSonData.masterArea[i].id == masterObjid) {
                for (var j = 0; j < JSonData.masterArea[i].subArea.length; j++) {
                    subAreaObj.options[subAreaObj.options.length] = new Option(JSonData.masterArea[i].subArea[j].name, JSonData.masterArea[i].subArea[j].id);
                }
            }
        }
    }
    else if (masterObjid && masterObjid == -1) {
        var subAreaObj = document.getElementById(citySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("覆盖区域 -- 市", -1);
    }
}

/*
* 绑定区县，需要引用Area2.js文件
* Area2.js文件是生成的
* 参数provinceSelectID为省份ID，citySelectID为城市ID, countyID为区县ID
*/
function BindCounty(provinceSelectID, citySelectID, countySelectID) {
    var temp = document.getElementById(provinceSelectID); if (!temp) { return; }
    temp = temp.options[document.getElementById(provinceSelectID).selectedIndex]; if (!temp) { return; }
    var provinceId = temp.value;

    var temp = document.getElementById(citySelectID); if (!temp) { return; }
    temp = temp.options[document.getElementById(citySelectID).selectedIndex]; if (!temp) { return; }
    var cityId = temp.value;
    if (provinceId && provinceId > 0 && cityId && cityId > 0) {
        var subAreaObj = document.getElementById(countySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("区/县", -1);
        for (var i = 0; i < JSonData.masterArea.length; i++) {
            if (JSonData.masterArea[i].id == provinceId) {
                var t1 = JSonData.masterArea[i];
                for (var j = 0; j < t1.subArea.length; j++) {
                    if (t1.subArea[j].id == cityId) {
                        var t2 = t1.subArea[j];
                        for (var k = 0; k < t2.subArea2.length; k++) {
                            subAreaObj.options[subAreaObj.options.length] =
                                new Option(t2.subArea2[k].name, t2.subArea2[k].id);
                        }
                        return;
                    }
                }
            }
        }
    }
    else if ((provinceId && provinceId == -1) || (cityId && cityId == -1)) {
        var subAreaObj = document.getElementById(countySelectID);
        if (subAreaObj != null) {
            subAreaObj.options.length = 0;
            subAreaObj.options[subAreaObj.options.length] = new Option("区/县", -1);
        }
    }
}