/**
* jselect
* Autor:Masj Date=2016-07-28
* @description 基于Jquery类库的下拉列表控件
* @version 1.0
* @使用说明：
* 1、$('#divSelect').jSelect({初始化对象}); //初始化控件，可以定义click、change事件等；若指定isProvinceCityCounty=true时，需要指定宽度width，否则默认宽度200px；
* 2、$('#divSelect').jSelect('getName')     //初始化控件，可以定义click、change事件等；
* 3、$('#divSelect').jSelect('settings', 'disabled', true);  //设置控件是否可用；
* 4、$('#divSelect').jSelect('settings', 'disabled');        //获取控件是否可用，true表示不可用，false表示可用；
* 5、$('#divSelect').jSelect('setVal', 1);  //给控件赋值；
* 6、$('#divSelect').jSelect('setVal', 1,2,3);  //给省份城市控件赋值，后面的参数按照级别来赋值；
* 7、$('#divSelect').jSelect('getName');    //获取控件当前选中的名称；
* 8、$('#divSelect').jSelect('getName',1);  //获取控件当前选中的名称；若为省份城市控件，第2个参数为级别，范围为1-3
* 9、$('#divSelect').jSelect('getVal');     //获取控件当前选中的ID；
* 10、$('#divSelect').jSelect('getVal'，1);  //获取控件当前选中的ID；若为省份城市控件，第2个参数为级别，范围为1-3
*/

;(function ($) {
    var totalStyle = 'bor_kun'; //div总样式
    var leftStyle = 'borleft2'; //总样式下面嵌套3个div，且3个div之间是平级关系，这是左侧div样式
    var centerStyle = 'borcenter2 iebor'; //总样式下面嵌套3个div，且3个div之间是平级关系，这是中间div样式
    var subCenterStyle = 'cui_selectBox'; //居中样式下，嵌套Div中的样式
    var rightStyle = 'borright2'; //总样式下面嵌套3个div，且3个div之间是平级关系，这是右侧div样式
    var currentSelectStyle = 'cui_selectTitle'; //当前div中（select控件）的样式colorgray
    var optionStyle = 'cui_selectSub4'; //下拉列表下，option所在Div的样式
    var slideSpeed = 200; //下拉列表滑动动画延迟（单位：毫秒）
    var currentSelectSpanStyle='sf_xz';//当前选中选项span标签样式
    var spanWidthFactor=0.82;//下拉列表中，span宽度占上级div宽度的百分比

    //var centerStyleByProvinceCity = 'borcenter2 borcenter21 iebor'; //总样式下面嵌套3个div，且3个div之间是平级关系，这是中间div样式
    //var currentSelectStyleByProvinceCity = 'cui_selectTitle'; //当前div中（select控件）的样式
    var optionStyleByProvinceCity = 'cui_selectSub'; //下拉列表下，option所在Div的样式
    var citycountyStyle = 'bor_kun2'; //城市、区县2级下拉特殊样式

    //控件提供的方法
    var methods = {

        //初始化控件
        init :function (options){
           return this.each(function () {
                
                var $this = $(this);
                //var $clone = $this.clone();

                var settings = $.extend({
                    //必须配置的项目
                    id: $this.attr('id'),
                    
                    //选项 
                    defaultMsg:'请选择',        //提示文本信息
                    defaultMsgId:-1,            //提示文本的值
                    width:200,                  //下拉列表的宽度，默认为200px，省份城市控件为58px
                    dataUrl:null,               //请求数据url(目前还没有实现)
                    formData:{},                //请求数据url，携带的参数(目前还没有实现)
                    dataJson:[],                //数据源，Json格式
                    dataBindField_ID:'id',      //数据源，Json格式，标识ID的字段名称
                    dataBindField_Name:'name',  //数据源，Json格式，标识Name的字段名称
                    isProvinceCityCounty:false, //是否绑定省份城市控件
                    provinceCityCountyLevel:3,  //若isProvinceCityCounty==true时，省份城市控件，需要设定几级（从1-3级），默认为3级
                    disabled:false,             //控件是否可用，默认为可用

                    onChanged:null,             //下拉列表内容变化事件
                    onClick:null                //控件选择某个选项后的点击事件
                }, options);
                
                $this.data('jSelect_Settings',settings);
                if (settings.isProvinceCityCounty) {
                    
                    settings.dataJson=JSonData.masterArea;
                    settings.defaultMsg="省/市";
                    InitControl($this,settings,1);//绑定一级

                    if(settings.provinceCityCountyLevel&&settings.provinceCityCountyLevel>1)
                    {
                        settings.dataJson=[];
                        settings.defaultMsg="城市";
                        var divObj2 = $('<div id="' + settings.id + '_city">').insertAfter($('#' + settings.id).parent().parent().parent());
                        InitControl(divObj2,settings,2);//绑定二级
                    }
                    if(settings.provinceCityCountyLevel&&settings.provinceCityCountyLevel>2)
                    {
                        settings.dataJson=[];
                        settings.defaultMsg="区/县";
                        var divObj3 = $('<div id="' + settings.id + '_county">').insertAfter(divObj2.parent().parent().parent());
                        InitControl(divObj3,settings,3);//绑定三级
                    }
                }
                else{
                    InitControl($this,settings);//初始化普通下拉列表（1级）
                }
           }); 
                
        },

        //获取或设置参数
        settings: function (name, value, resetObjects) {
            var args = arguments;
            var returnValue = value;
            
            this.each(function () {
                // Create a reference to the jQuery DOM object
                var $this = $(this),
			    settings = $this.data('jSelect_Settings');

                if (typeof (args[0]) == 'object') {
                    for (var n in value) {
                        setData(n, value[n]);
                    }
                }
                if (args.length === 1) {
                    returnValue = settings[name];
                } else {
                    switch(name)
                    {
                        case 'disabled':
                        var selectObj = $("div[id='" + settings.id + "'],div[id='" + settings.id + "_city'],div[id='" + settings.id + "_county']");
                        handlers.setEnabledOrDisabled(selectObj,value);
                        break;
                    }
                    settings[name] = value;
                    $this.data('jSelect_Settings',settings);
                }
            });
            if (args.length === 1) {
                return returnValue;
            }
        },

        //获取下拉列表的值
        getVal : function(level){
                if(level){
                   var obj = handlers.getDivObj($(this),level);
                }
                else{ 
                   var obj = handlers.getDivObj($(this));
                }
                if (obj && obj != null) {
                    return obj.attr('rel');
                }
                return '';
            },

        //获取下拉列表的值
        getName : function(level){
                if(level){
                   var obj = handlers.getDivObj($(this),level);
                }
                else{ 
                   var obj = handlers.getDivObj($(this));
                }
                if (obj && obj != null) {
                    return obj.find('span.' + currentSelectSpanStyle ).html();
                }
                return '';
        },

        //设置下拉列表的值
        setVal : function (val,val2,val3) {
            var $this = $(this),
			    settings = $this.data('jSelect_Settings');
            $this.next('div').find("p > a[rel='" + settings.defaultMsgId + "']").triggerHandler('click',{'isInnerCalled':true});
            var obj = handlers.getDivObj($this);
            handlers.setVal(obj,val);
            
            if(settings.provinceCityCountyLevel && settings.provinceCityCountyLevel>1)
            {
                obj.next('div').find("p > a[rel='" + val + "']").triggerHandler('click',{'isInnerCalled':true});
                if(val2 && val2 >0)
                {
                   handlers.setVal($("div[id='" + settings.id + "_city']"), val2);
                }
            }
            if(settings.provinceCityCountyLevel && settings.provinceCityCountyLevel>2)
            {
                $("div[id='" + settings.id + "_city']").next('div').find("p > a[rel='" + val2 + "']").triggerHandler('click',{'isInnerCalled':true});
                if(val3 && val3 >0)
                {
                    handlers.setVal($("div[id='" + settings.id + "_county']"), val3);
                }
            }
        }

    };

    //控件的事件处理
    var handlers = {
        //选项点击事件
        optionClick : function (currentOption, divObj, settings,level,isInnerCalled) {
            var id = currentOption.attr('rel');
            var name = currentOption.html();
            
            var itemOld = {};
            itemOld[settings.dataBindField_ID] = divObj.attr('rel');
            itemOld[settings.dataBindField_Name] = divObj.find('span.' + currentSelectSpanStyle ).html();

            divObj.attr('rel', id);
            divObj.find('span.' + currentSelectSpanStyle ).html(name);

            var itemNew = {};
            itemNew[settings.dataBindField_ID] = id;
            itemNew[settings.dataBindField_Name] = name;
            if (level && level > 0) {
                itemNew.level = level;
                itemOld.level = level;
            }
            if(settings.isProvinceCityCounty&&
                (level==1||level==2))
            {
               var data = [];
               if(level==1)
               { 
                    data = handlers.getProvinceCityData(itemNew.id);
               }
               else if(level==2&&itemNew.id>0)
               {
                    data = handlers.getProvinceCityData($('#'+settings.id).attr('rel'),itemNew.id);
               }
               handlers.bindProvinceCityData(divObj,data,settings,level);
            }
            if(!isInnerCalled){
                if (settings.onClick) {
                    settings.onClick.call(this, itemNew);
                }
                if (settings.onChanged && itemNew[settings.dataBindField_ID] != itemOld[settings.dataBindField_ID]) {
                    settings.onChanged(itemNew, itemOld);
                }
            }
        },

        //获得当前控件的对象（若为省份城市控件，需要制定level层级，1-3）
        getDivObj : function(divObj,level){
                var settings = divObj.data('jSelect_Settings');
                if (settings.isProvinceCityCounty && 
                    settings.provinceCityCountyLevel && 
                    settings.provinceCityCountyLevel > 0) {
                    
                    if(level && level>=1 && level<=3)
                    {
                        switch(level)
                        {
                            case 1://一级
                                break;
                            case 2://二级
                                divObj = $("div[id='" + settings.id + "_city']");
                                break;
                            case 3://三级
                                divObj = $("div[id='" + settings.id + "_county']");
                                break;
                            default:
                                divObj = null;
                                break;
                        }
                    }
                    else
                    {
                        var divObj2_Temp = $("div[id='" + settings.id + "_city']");
                        var divObj3_Temp = $("div[id='" + settings.id + "_county']");
                        switch (settings.provinceCityCountyLevel) {
                            case 1:
                                break;
                            case 2:
                                if(divObj2_Temp.attr('rel') && divObj2_Temp.attr('rel')>0)
                                {
                                   divObj=divObj2_Temp;
                                }
                                break;
                            case 3:
                                if(divObj3_Temp.attr('rel') && divObj3_Temp.attr('rel')>0)
                                {
                                   divObj=divObj3_Temp;
                                }
                                else if(divObj2_Temp.attr('rel') && divObj2_Temp.attr('rel')>0)
                                {
                                   divObj=divObj2_Temp;
                                }
                                break;
                            default:
                                divObj = null;
                                break;
                        }

                    }                    
                }
                return divObj;
        },

        //设置控件是否可用
        setEnabledOrDisabled:function(divObj,flag){
            var settings = divObj.data('jSelect_Settings');
            if(flag)//禁用
            {
                divObj.each(function(){
                    $(this).parent().parent().unbind('click');
                    $(this).css('cursor','default').find('span.' + currentSelectSpanStyle).addClass('colorgray');
                });
            }
            else{//启用
                divObj.each(function(){
                    $(this).parent().parent().unbind('click').bind('click', function () {
                        $(this).find('div.'+optionStyleByProvinceCity+',div.'+optionStyle).toggle(slideSpeed); //切换下拉列表显示或隐藏
                    });
                    $(this).css('cursor','pointer').find('span.' + currentSelectSpanStyle).removeClass('colorgray');
                });
            }
        },
        //把引用元素中的样式，移动到生成后的div中
        getOriDivStyle:function(divObj){
            if (divObj.size() == 1) {
                var divObj_Style = divObj.attr('style');
                divObj_Style = (divObj_Style ? divObj_Style : ''); //取出div中的样式，准备移动到生成后的div中
                divObj.removeAttr('style'); //移除页面中当前div的样式style
                return divObj_Style;
            }
            return '';
        },
        //根据省份城市ID，获取Json数据
        getProvinceCityData:function(provinceId,cityId){
            var dataArray = [];
            if(provinceId&&provinceId>0)
            {
                for (var i = 0; i < JSonData.masterArea.length; i++) {
                    if (JSonData.masterArea[i].id == provinceId) {
                        for (var j = 0; j < JSonData.masterArea[i].subArea.length; j++) {
                            if(cityId&&cityId>0)//有城市ID时
                            {
                                if(JSonData.masterArea[i].subArea[j].id == cityId){
                                    for (var m = 0; m < JSonData.masterArea[i].subArea[j].subArea2.length; m++) {
                                            dataArray.push({ "name": JSonData.masterArea[i].subArea[j].subArea2[m].name, "id": JSonData.masterArea[i].subArea[j].subArea2[m].id });
                                    }
                                }
                            }
                            else{
                                dataArray.push({ "name": JSonData.masterArea[i].subArea[j].name, "id": JSonData.masterArea[i].subArea[j].id });
                            }
                        }
                    }
                }
            }
            return dataArray;
        },
        //根据div元素，清理下面option内容
        clearSubDataByPid:function(divObj,settings){
            var optionObj = divObj.next('div');
            if (optionObj && optionObj.size() == 1) {
                optionObj.find("p:has(a[rel!='"+settings.defaultMsgId+"'])").remove();
                divObj.attr('rel', settings.defaultMsgId);
                divObj.find('span.sf_xz').html(optionObj.find("p > a[rel='"+settings.defaultMsgId+"']").html());
            }
        },
        //根据省份城市Json数据，进行初始化下拉列表
        bindProvinceCityData:function(divObj,dataJson,settings,level){
            if(level&&level==1)
            {
                divObj=$('#'+settings.id+'_city');
                handlers.clearSubDataByPid($('#'+settings.id+'_city'),settings);
                handlers.clearSubDataByPid($('#'+settings.id+'_county'),settings);
            }
            else if(level&&level==2)
            {
                divObj=$('#'+settings.id+'_county');
                handlers.clearSubDataByPid(divObj,settings);
            }
            
            //遍历下拉选项
            for (var data in dataJson) {
                var aObj = $('<a>').attr('href', 'javascript:void(0);')
                                   .unbind('click').bind('click', function (event,para) {
                                       handlers.optionClick($(this), divObj, settings,(level?level+1:null),(para && para.isInnerCalled ? true:false));
                                   });
                aObj.html(dataJson[data]['name']);
                aObj.attr('rel', dataJson[data]['id']);
                //生成html代码
                divObj.next('div').append($('<p>').append(aObj));
            }
        },
        //设置下拉列表的值
        setVal:function (obj,val){
            if (obj && obj != null) {
                var findOjb = obj.parent().find("p > a[rel='" + val + "']");
                if (findOjb.size() == 1) {
                    obj.attr('rel', val);
                    obj.find('span.' + currentSelectSpanStyle ).html(findOjb.html());
                    return true;
                }
            }
            return false;
        }
    };

    //初始化普通下拉列表（1级）
    InitControl = function (divObj,settings,level){
        if (divObj.size() == 1) {
            var divObj_Style = handlers.getOriDivStyle(divObj);
            var defaultMsg=settings.defaultMsg;
            if (typeof (settings.defaultMsg) == 'undefined' || settings.defaultMsg == null || settings.defaultMsg == '') {
                defaultMsg = '请选择';
            }
            divObj.html($('<span>').addClass(currentSelectSpanStyle).width(settings.width * spanWidthFactor).html(defaultMsg))
                  .addClass(currentSelectStyle)
                  .width(settings.width)
                  .attr('rel', settings.defaultMsgId);
            if(settings.width){
                divObj.width(settings.width);
            }
            var genOptionStyle = optionStyle;
            var gentotalStyle = totalStyle;
            if(level)//省份城市控件中的，二级或三级，需要添加citycountyStyle样式
            {
               if(level>0){
                    genOptionStyle = optionStyleByProvinceCity;
               }
               if(level>1){
                    gentotalStyle = totalStyle + ' ' + citycountyStyle;
               }
            }
            var optionDiv = $('<div>').addClass(genOptionStyle).hide();

            var dataJson=settings.dataJson;
            var defaultJson={};
            defaultJson[settings.dataBindField_ID]=settings.defaultMsgId;
            defaultJson[settings.dataBindField_Name]=settings.defaultMsg;
            defaultJson['top']=true;
            dataJson.push(defaultJson);

            //遍历下拉选项
            for (var data in dataJson) {
                var aObj = $('<a>').attr('href', 'javascript:void(0);')
                                   .unbind('click').bind('click', function (event,para) {
                                       handlers.optionClick($(this), divObj, settings,level,(para && para.isInnerCalled ? true:false));
                                   });
                aObj.html(dataJson[data][settings.dataBindField_Name]);
                aObj.attr('rel', dataJson[data][settings.dataBindField_ID]);
                //生成html代码
                if(dataJson[data]['top']&&dataJson[data]['top']==true)
                {
                   optionDiv.prepend($('<p>').append(aObj));
                }
                else
                {
                   optionDiv.append($('<p>').append(aObj));
                }
            }
            var divCenterStyle = $('<div>').addClass(centerStyle);
            
            if(settings.width){
               divCenterStyle.width(settings.width);
            }
            
            //整理html代码，附加到当前页面中
            divObj.wrap($('<div>').addClass(subCenterStyle))//包裹div样式cui_selectBox
                  .parent().wrap(divCenterStyle)//包裹div.borcenter2，并根据控件中的参数宽度进行赋值操作
                  .parent().wrap($('<div>').addClass(gentotalStyle).attr('style', divObj_Style))//包裹div.bor_kun
                  .before($('<div>').addClass(leftStyle))//添加左边圆角样式
                  .after($('<div>').addClass(rightStyle))//添加右边圆角样式
                  .unbind('click').bind('click', function (event) {
                      optionDiv.toggle(slideSpeed); //切换下拉列表显示或隐藏
                  });
            
            divObj.after(optionDiv); //添加option内容到select中
            handlers.setEnabledOrDisabled(divObj,settings.disabled);
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
    
    $.fn.jSelect = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('The method ' + method + ' does not exist in $.jSelect');
        }

    }
})($);