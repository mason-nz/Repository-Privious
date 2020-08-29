
Area = function (Arguments) {
    //Global variables.
    // Constructor function

    var BaseId = Arguments ? Arguments.BaseId : Arguments; // necessary paramter

    // 下面是可选的参数 PWidth,CityWidth,CountryWidth,PClass,CityClass,CountryClass
    var PWidth, CityWidth, CountryWidth;
    var PClass, CityClass, CountryClass;
    // 缺省事件是change
    var Event, ProvinceListener, CityListener, CountryListener, type;

    var SelectElement = null;
    var parent;
    var idProvince, idCity, idCountry;
    var bExistingCountry = true;

    // start函数
    Initialize = function () {
        bExistingCountry = IsExistCountry();
        if (BaseId != undefined) {
            InitializingField();
            CreateSelectHTML();
            InitializingEvent();
        }
    }
    // 初始化全部global varibles
    InitializingField = function () {
        // 下面是可选的参数 PWidth,CityWidth,CountryWidth,PClass,CityClass,CountryClass
        PWidth = Arguments.PWidth ? Arguments.PWidth : "80px";
        CityWidth = Arguments.CityWidth ? Arguments.CityWidth : "80px";
        CountryWidth = Arguments.CountryWidth ? Arguments.CountryWidth : "90px";

        PClass = Arguments.PClass ? Arguments.PClass : "kProvince";
        CityClass = Arguments.CityClass ? Arguments.CityClass : "kArea";
        CountryClass = Arguments.CountryClass ? Arguments.CountryClass : "kArea";
        // 缺省事件是change
        Event = Arguments.Event ? Arguments.Event : "change";
        // 缺省listener
        ProvinceListener = Arguments.ProvinceListener ? Arguments.ProvinceListener : function () { ; };
        CityListener = Arguments.CityListener ? Arguments.CityListener : function () { ; };
        CountryListener = Arguments.CountryListener ? Arguments.CountryListener : function () { ; };
        // 该参数若使用，type=3，type=2；如果不使用该参数也可以使用IsExistCountry遍历数据源来获得此信息。
        type = Arguments.type ? Arguments.type : -1;

        parent = BaseId ? $("#" + BaseId) : BaseId;
        idProvince = BaseId ? BaseId + "_Province" : "_Province";
        idCity = BaseId ? BaseId + "_City" : "_City";
        idCountry = BaseId ? BaseId + "_Country" : "_Country";

    }
    // 创建需要的select group元素
    CreateSelectHTML = function () {
        // 省/直辖市
        SelectElement = CreateSelectElement(idProvince, idProvince, PClass, PWidth);
        if (type != 1) {
            SelectElement.bind("change", function () { BindCity(idProvince, idCity); });
        }
        if (type != 2 && type != 1) {
            if (bExistingCountry) {
                SelectElement.bind("change", function () { BindCounty(idProvince, idCity, idCountry); });
            }
        }
        parent.before(SelectElement);
        BindProvince(idProvince);

        if (type != 1) {
            //市
            SelectElement = CreateSelectElement(idCity, idCity, CityClass, CityWidth);
            if (type != 2) {
                if (bExistingCountry) {
                    SelectElement.bind("change", function () { BindCounty(idProvince, idCity, idCountry); });
                }
            }
            parent.before(SelectElement);
            BindCity(idProvince, idCity);
            oCity().val('-1');
            //区
            if (type != 2) {
                if (type == 3 || (type == -1 && bExistingCountry)) {
                    SelectElement = CreateSelectElement(idCountry, idCountry, CountryClass, CountryWidth);
                    parent.before(SelectElement);
                    BindCounty(idProvince, idCity, idCountry);
                    oCountry().val('-1');
                }
            }
        }
        parent.remove();
    }

    // 创建一个select元素
    CreateSelectElement = function (id, name, classString, nwidth) {
        var objSelect = $("<select></select>"); //
        objSelect.attr("id", id);
        objSelect.attr("name", name);
        
        if (classString == "kProvince" || classString == "kArea") {
            objSelect.width(nwidth);
        }
        else {
            objSelect.addClass(classString);
        }
        return objSelect;
    }
    // 初始化event，bind function
    InitializingEvent = function () {
        if (oProvince() && typeof (ProvinceListener) == 'function') {
            oProvince().bind(Event, function () { ProvinceListener(); });
        }
        if (oCity() && typeof (CityListener) == 'function') {
            oCity().bind(Event, CityListener);
        }
        if (oCountry() && typeof (CountryListener) == 'function') {
            oCountry().bind(Event, CountryListener);
        }
    }
    // 判断数据源是三级还是二级，三级：province/city/country，返回值是true；二级：province/city，返回值是false
    IsExistCountry = function () {
        var bIsExisting = false;
        for (var i = 0; i < JSonData.masterArea.length; i++) {
            var province = JSonData.masterArea[i];
            if (bIsExisting) break;
            for (var j = 0; j < province.subArea.length; j++) {
                var city = province.subArea[j];
                if (city.subArea2 == undefined) {
                    break;
                }
                if (city.subArea2 != undefined && city.subArea2.length > 0) {
                    bIsExisting = true;
                    break;
                }
            }
        }
        return bIsExisting;
    }
    // retrieve an instance of province
    var oProvince = function () {
        return $("#" + idProvince);
    }
    // retrieve an instance of city
    var oCity = function () {
        return $("#" + idCity);
    }
    // retrieve an instance of country
    var oCountry = function () {
        return $("#" + idCountry);
    }
    //获取province的value，也就是id，如北京：2
    getProvinceValue = function (Value) {
        var provinceValue, provinces = JSonData.masterArea;
        for (var i = 0; i < provinces.length; i++) {
            if (provinces[i].id == Value) {
                provinceValue = Value + "," + provinces[i].name;
                break;
            }
        }
        return provinceValue;
    }
    // 获取city的value，id，如：北京市：201
    getCityValue = function (Value) {
        var bIsContinue = false;
        var cityValue, provinces = JSonData.masterArea;
        for (var i = 0; i < provinces.length; i++) {
            var province = provinces[i];
            if (bIsContinue) break;
            for (var j = 0; j < province.subArea.length; j++) {
                var city = province.subArea[j];
                if (bIsContinue) break;
                if (city.id == Value) {
                    bIsContinue = true;
                    cityValue = province.id + "," + province.name + "/" + Value + "," + city.name;
                    break;
                }
            }
        }
        return cityValue;
    }
    // 获取country的value，id，如：朝阳区：110105
    getCountryValue = function (Value) {
        var bIsContinue = false;
        var countryValue, provinces = JSonData.masterArea;
        for (var i = 0; i < provinces.length; i++) {
            var province = provinces[i];
            if (bIsContinue) break;
            for (var j = 0; j < province.subArea.length; j++) {
                var city = province.subArea[j];
                if (bIsContinue) break;
                for (var k = 0; k < city.subArea2.length; k++) {
                    var country = city.subArea2[k];
                    if (country.id == Value) {
                        countryValue = province.id + "," + province.name + "/" + city.id + "," + city.name + "/" + country.id + "," + country.name;
                        bIsContinue = true;
                        break;
                    }
                }
            }
        }
        return countryValue;
    }
    // 根据查询的value，设置各组select的value，text。兼容二级和三级
    setValue = function (matchedValue) {
        var arrValues = matchedValue.split('/');
        var arrTexts;
        arrTexts = arrValues[0].split(',');
        oProvince().val(arrTexts[0]);
        if (type != 1) {
            BindCity(idProvince, idCity);
            clearCountry(idCountry);
            if (arrValues.length == 2 || arrValues.length == 3) {
                arrTexts = arrValues[1].split(',');
                oCity().val(arrTexts[0]);
                if (bExistingCountry) {
                    if (type != 2) {
                        BindCounty(idProvince, idCity, idCountry);
                        if (arrValues.length == 3) {
                            arrTexts = arrValues[2].split(',');
                            oCountry().val(arrTexts[0]);
                        }
                    }
                }
            }
        }
    }
    clearCountry = function (idCountry) {
        if (oCountry()) {
            oCountry().empty();
            oCountry().append("<option value='-1'>区/县</option>");
        }
    }
    // The function can be used when SELECT group is not crated.
    // 根据查询的value，格式化text，兼容二级和三级
    setTextByFormat = function (Value, format) {
        var arrValues = Value.split('/');
        var arrTexts;
        var formatedText;
        if (arrValues.length == 1) {
            arrTexts = arrValues[0].split(',');
            formatedText = arrTexts[1];
        }
        else if (arrValues.length == 2 || arrValues.length == 3) {
            arrTexts = arrValues[0].split(',');
            formatedText = arrTexts[1] + format;
            arrTexts = arrValues[1].split(',');
            formatedText += arrTexts[1];
            if (arrValues.length == 3) {
                arrTexts = arrValues[2].split(',');
                formatedText += format + arrTexts[1];
            }
        }
        return formatedText;
    }
    // 下面为省市区数据初始化过程，各select的id分别为 BaseId_Province,BaseId_City, BaseId_Country, BaseId根据不同的div的id而改变    
    Initialize();


    // The following functions are correlated with created SELECT group.
    // The followings are interface of the Area object
    // 获取省直辖市的text
    this.ProvinceText = function () {
        return oProvince().find("option:selected").text();
    }
    //获取市的text
    this.CityText = function () {
        return oCity().find("option:selected").text();
    }
    //获取地区的text
    this.CountryText = function () {
        return oCountry().find("option:selected").text();
    }
    // 获取省直辖市的value
    this.ProvinceVal = function () {
        return oProvince().val();
    }
    // 获取市的value
    this.CityVal = function () {
        return oCity().val();
    }
    // 获取地区的value
    this.CountryVal = function () {
        return oCountry().val();
    }
    // retrieve the instance of Province object
    this.Province = function () {
        return oProvince();
    }
    // retrieve an instance of City object
    this.City = function () {
        return oCity();
    }
    // retrieve an instance of Country object
    this.Country = function () {
        return oCountry();
    }
    // set proper text with province/city/country value
    this.SetProvinceCityCountry = function (province, city, country) {
        if (!BaseId) return;
        oProvince().val(province);
        if (type != 1) {
            BindCity(idProvince, idCity);
            oCity().val(city);
            if (type != 2) {
                if (bExistingCountry) {
                    BindCounty(idProvince, idCity, idCountry);
                    oCountry().val(country);
                }
            }
        }
    }
    // 根据一个value，设置二级或三级的text
    this.SetByOneValue = function (Value) {
        var bIsContinue = false;
        var provinceValue, cityValue, countryValue;
        var arrValues;
        if (!BaseId) return;
        provinceValue = getProvinceValue(Value);
        if (provinceValue) {
            setValue(provinceValue);
        }
        else {
            if (type != 1) {
                cityValue = getCityValue(Value);
                if (cityValue != undefined) {
                    setValue(cityValue);
                }
                else {
                    if (type != 2) {
                        if ((type == 3 && bExistingCountry) || (type == -1 && bExistingCountry)) {
                            countryValue = getCountryValue(Value);
                            if (countryValue != undefined) {
                                setValue(countryValue);
                            }
                        }
                    }
                }
            }
        }
    }
    //验证是否选择，如：北京/北京市/区县，返回false；北京/北京市/朝阳区,返回true;没有区县的数据,追溯上一级,如:甘肃省/嘉峪关市/区县,返回true;
    // 北京/市区/区县,返回false;北京/北京市/区县,返回false;而香港/市区/区县,返回true,此时没有二级和三级数据.
    this.Validate = function () {
        var bExists = true;
        if (!BaseId) return;
        if (oCountry() && (oCountry().val() == '-1' || oCountry().val() == undefined)) {
            bExists = false;
            if ($("#" + idCountry + " option").length == 1 || oCountry().val() == undefined) {
                if (oCity() && oCity().val() != '-1') {
                    bExists = true;
                }
                else if ($("#" + idCity + " option").length == 1) {
                    bExists = false;
                }
            }
        }
        return bExists;
    }
    // 返回最后一组被选择的数据
    this.LastVal = function () {
        var Value = "";
        if (oCountry() && (oCountry().val() != undefined) && (oCountry().val() != '-1'))
            Value = oCountry().val();
        else if (oCity() && (oCity().val() != undefined) && oCity().val() != '-1')
            Value = oCity().val();
        else
            Value = oProvince().val();
        return Value;
    }
    // The following function is used when SELECT group is not crated.
    // 根据value，获得二级或三级的text，并根据format格式来格式化字符串，如：format：'-'，北京-北京市-朝阳区
    this.GetText = function (Value, format) {
        var matchedText = "";
        var bIsContinue = false;
        var provinceValue, cityValue, countryValue;
        var arrValues;
        provinceValue = getProvinceValue(Value);
        if (provinceValue) {
            matchedText = setTextByFormat(provinceValue, format);
        }
        else {
            if (type != 1) {
                cityValue = getCityValue(Value);
                if (cityValue != undefined) {
                    matchedText = setTextByFormat(cityValue, format);
                }
                else {
                    if (type != 2) {
                        if (bExistingCountry) {
                            countryValue = getCountryValue(Value);
                            if (countryValue != undefined) {
                                matchedText = setTextByFormat(countryValue, format);
                            }
                        }
                    }
                }
            }
        }
        return matchedText;
    }
}