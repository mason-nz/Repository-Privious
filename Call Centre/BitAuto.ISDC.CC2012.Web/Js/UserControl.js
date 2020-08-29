//**************************************************** 下拉控件  ****************************************************
//---根据div+css，实现下拉列表及省份城市列表（省份城市数据源，依赖于JSON【/js/Enum/Area2.js】）
//---add=masj，date=2016-07-25
//---引用此脚本，需要JQuery类库支持
//**************************************************** 下拉控件  ****************************************************
DIVSelect = (function () {
    //初始化
    var Init = function (objID, dataArray, defaultMsg, selectedCallback) {
        $('#' + objID).jSelect({
            defaultMsg: defaultMsg,
            dataJson: dataArray,
            onClick: function (item) {
                if (selectedCallback && typeof selectedCallback == "function") {
                    selectedCallback(item);
                }
            }
        });
    };
    //方法：获取下拉控件当前选中的值
    var GetVal = function (objID, level) {
        return $('#' + objID).jSelect('getVal', level);
    };
    //方法：获取下拉控件当前选中的内容
    var GetName = function (objID, level) {
        return $('#' + objID).jSelect('getName', level);
    };
    //方法：设置下拉控件的值
    var SetVal = function (objID, val) {
        $('#' + objID).jSelect('setVal', val);
    };
    //方法：控件不可用
    var Disabled = function (objID) {
        $('#' + objID).jSelect('settings', 'disabled', true);
    };
    //方法：控件可用
    var Enabled = function (objID) {
        $('#' + objID).jSelect('settings', 'disabled', false);
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
    //初始化省份城市控件
    var InitProvinceCity = function (objID) {
        $('#' + objID).jSelect({
            isProvinceCityCounty: true,
            provinceCityCountyLevel: 3,
            width: 58
        });
    };
    //给省份城市控件，进行复制操作
    var SetValByProvinceCity = function (objID, provinceID, cityID, countyID) {
        $('#' + objID).jSelect('setVal', provinceID, cityID, countyID);
    };
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

//**************************************************** 上传图片控件  ****************************************************
//---上传图片控件封装
//---add=wangth，date=2016-07-26
//--此控件对外提供2个方法，1.初始化控件  2.获取控件中图片数据信息
//-----------------------------------------------------------------------------------------
//UploadifyControl.Init("DivUPloadTest", "上传附件","uploads", 0); //方法：初始化控件
//参数为：第一个参数DivID=控件div的ID,第二个参数btnName=上传按钮的名字,第三个参数cssname=自定义按钮样式 class，第四个参数
// uploadify_CC为存储类型 （ 0 =“其他”, 1="黑白名单"，2="数据模版"，3="知识库"，4="项目管理"，5="易派会员"，6="工单"），请勿传递其他数字
//Uploadify.GetUploadifyArr()  获取控件中图片数据信息方法，此方法无需参数
//---引用此脚本，需要JQuery类库支持，
//CommonJS, 
//<link href="../Css/workorder/wo-uploadify.css" rel="stylesheet" type="text/css" />, 
//../Js/jquery.uploadify.js      
//**************************************************** 上传图片控件  ****************************************************
UploadifyControl = (function () {
    var objUploadifyArr = new Array(); //IMG数据信息数组
    var uploadLimit = 7; //上传文件总数最大为7
    var uploadFileSizeLimit = 1024; //文件最大1M            
    var uploadUrl = '/WOrderV2/Handler/IMGImportHandler.ashx'; //上传URL
    var storagePathType = 0;
    function getsysCookie() {
        var sysCookie = ""; //登录cookie           
        AjaxPostAsync(uploadUrl, {
            Action: "getSysLoginCookieName",
            r: Math.random()
        }, null, function (returnValue) {
            var retValue = $.evalJSON(returnValue);
            sysCookie = GetCookie(retValue.result);
        });
        return sysCookie;
    }

    var Init = function (btnDivID, btnName, cssname, StoragePathType) {
        storagePathType = StoragePathType;
        $("#" + btnDivID).html(' <input type="file" id="uploadify_CC"  name="uploadify_CC" />');
        btnName = '<a id="uploadify_a" href="javascript:void(0)">' + btnName + '</a>';
        $("#uploadify_CC").uploadify({
            'buttonText': btnName,
            'swf': '/js/uploadify.swf?_=' + Math.random(),
            'uploader': uploadUrl,
            'auto': false,
            'multi': true,
            'width': 65,
            'height': 20,
            'FileContentID': 'div_FileContent',
            'formData': {
                Action: 'imgImport', StoragePathType: StoragePathType
            },
            'fileTypeDesc': '支持格式:.png;.jpg;.jpeg;.gif',
            'fileTypeExts': '*.png;*.jpg;*.jpeg;*.gif',
            'queueSizeLimit': uploadLimit,
            'fileSizeLimit': uploadFileSizeLimit,
            'scriptAccess': 'always',
            'removeCompleted': false,
            'onSelect': function (file) {
                var selectFileDiv = $(".uploadify-queue-item");
                if (selectFileDiv.length > uploadLimit) {
                    $("#" + file.id).remove();
                    $.jAlert("上传文件个数不能超过" + uploadLimit + "个！");
                    return;
                }
                $('#uploadify_CC').uploadify('settings', 'uploader', uploadUrl + '?LoginCookiesContent=' + escapeStr(getsysCookie()));
                $("#uploadify_CC").uploadify('upload', '*');
            },
            'onQueueComplete': function (event, data) {
            },
            'onQueueFull': function () {
            },
            'onUploadSuccess': function (file, data, response) {
                if (response == true) {
                    var dataObj = eval("(" + data + ")");
                    if (dataObj.success) {
                        objUploadifyArr[file.id] = dataObj.message;
                        var dataObjMsg = eval("(" + dataObj.message + ")");
                        $("#" + file.id).attr("title", dataObjMsg.FileRealName);
                        //                        $("#" + file.id + "_small").css("background-image", "url(" + dataObjMsg.SmallFileVirtuPath + ")").css("background-size", "100% 100%").css("filter", "progid:DXImageTransform.Microsoft.AlphaImageLoader( src='" + dataObjMsg.SmallFileVirtuPath + "', sizingMethod='scale')");

                        $("#" + file.id + "_small").attr("src", dataObjMsg.SmallFileVirtuPath);
                        $("#" + file.id + "_small").after('<div class="cancel"  onclick="UploadifyControl.deleteImg(&quot;' + file.id + '&quot;)"></div>');
                    }
                    else {
                        $.jAlert(dataObj.result);
                    }
                }
            },
            'onProgress': function (event, queueID, fileObj, data) { },
            'onUploadError': function (event, queueID, fileObj, errorObj) {
            },
            'onSelectError': function (file, errorCode, errorMsg) {
                switch (errorCode) {
                    case -100:
                        $.jAlert("上传的文件数量已经超出系统限制的" + $('#uploadify_CC').uploadify('settings', 'queueSizeLimit') + "个文件！");
                        break;
                    case -110:
                        $.jAlert("文件 [" + file.name + "] 大小超出系统限制的" + $('#uploadify_CC').uploadify('settings', 'fileSizeLimit') + "大小！");
                        break;
                    case -120:
                        $.jAlert("文件 [" + file.name + "] 大小异常！");
                        break;
                    case -130:
                        $.jAlert("文件 [" + file.name + "] 类型不正确！");
                        break;
                }
                return;
            }
        });
        $("#uploadify_CC").css("position", "relative").addClass(cssname);
        $("#uploadify_CC").mouseover(function () {
            $("#uploadify_a").addClass("uploadify-button-text-red");
            $("#uploadify_a").removeClass("uploadify-button-text-black");
        });
        $("#uploadify_CC").mouseout(function () {
            $("#uploadify_a").addClass("uploadify-button-text-black");
            $("#uploadify_a").removeClass("uploadify-button-text-red");
        });
        $("#uploadify_a").addClass("uploadify-button-text-black");
    };

    var deleteImg = function (fileID) {
        //        $('#uploadify_CC').uploadify('cancel', fileID);
        var imgData = eval("(" + objUploadifyArr[fileID] + ")");
        getsysCookie();
        AjaxPost(uploadUrl + '?LoginCookiesContent=' + escapeStr(getsysCookie()), {
            Action: "deleteIMG",
            StoragePathType: storagePathType,
            fileAllPath: imgData.FileAllPath,
            r: Math.random()
        }, null, function (returnValue) {
            var ret = $.evalJSON(returnValue);
            if (ret.success) {
                objUploadifyArr[fileID] = "";
                $("#uploadify_CC").uploadify('cancel', fileID, false);
            }
            else {
                $.jAlert("文件删除失败：" + ret.result);
            }
        });
    };
    //返回地址的数组
    var GetobjUploadifyArr = function () {
        var retArr = new Array();
        for (var key in objUploadifyArr) {
            if (objUploadifyArr[key] != "") {
                retArr.push($.evalJSON(objUploadifyArr[key]));
            }
        }
        return retArr;
    };

    return {
        Init: Init,
        GetUploadifyArr: GetobjUploadifyArr,
        deleteImg: deleteImg
    }
})();

//****************************************************  经销商查询控件  ****************************************************
// bif 2016-7-27
//此控件至少需要初始化四个参数：thisTxtDistributorId（经销商名称显示文本框id）、
//                              thisBtnDistributorId(经销商查询按钮id)
//
//可选初始化参数：thisBtnYaChaoId(亚超登录按钮id)、
//                thisEnableInput (经销商名称显示文本框是否可以手动输入)
//                thisCustId（客户id，用以查询指定客户下的经销商）
//返回的其他信息放在thisTxtDistributorId的三个属性中（attrMemberCode， attrCustName， attrCustID）
//**************************************************************************************************************************
DealerSearchControl = (function () {
    var jsonParams = { "thisTxtDistributorId": "",
        "thisBtnDistributorId": "",
        "thisBtnYaChaoId": "",
        "thisEnableInput": true,
        "thisCustId": "",
        "thisMemberName": "",
        "thisMemberCode": ""
    };
    var InitialEventNoParams = function () {  //按钮绑定事件
        if (CheckParams()) {
            var membername = $.trim($("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).val());
            DealerSearchControl.JsonParams.thisMemberName = membername == "所属经销商" ? "" : membername;
            $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).attr('attrMemberCode', DealerSearchControl.JsonParams.thisMemberCode);
            $("#" + DealerSearchControl.JsonParams.thisBtnDistributorId).click(BtnDistributorClick);
            $("#" + DealerSearchControl.JsonParams.thisBtnYaChaoId).click(BtnYaChaoClick);
            //输入框是否可以手动输入
            if (!DealerSearchControl.JsonParams.thisEnableInput) {
                $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).attr('disabled', true);
                DealerSearchControl.JsonParams.thisEnableInput = "0";
            }
            else {
                $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).attr('disabled', false);
                DealerSearchControl.JsonParams.thisEnableInput = "1";
            }
            $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).keyup(function () {
                ClearAttrValue(this);
            });
        }
    };
    var InitialEvent = function (txtDistributorId, btnDistributorId, btnYaChaoId, enableInput, custId) {  //按钮绑定事件
        var argLength = arguments.length;
        if (argLength != 5) {
            $.jAlert("初始化参数个数不对");
        }
        else if (typeof (enableInput) != "boolean") {
            $.jAlert("enableInput参数类型因为boolean");
        }
        else {
            //公共变量赋值
            DealerSearchControl.JsonParams.thisTxtDistributorId = txtDistributorId;
            DealerSearchControl.JsonParams.thisBtnDistributorId = btnDistributorId;
            if (CheckParams()) {
                DealerSearchControl.JsonParams.thisBtnYaChaoId = btnYaChaoId;
                DealerSearchControl.JsonParams.thisCustId = custId;
                var membername = $.trim($("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).val());
                DealerSearchControl.JsonParams.thisMemberName = membername == "所属经销商" ? "" : membername;
                //按钮绑定事件
                $("#" + btnDistributorId).click(BtnDistributorClick);
                if (btnYaChaoId != "") {
                    $("#" + btnYaChaoId).click(BtnYaChaoClick);
                }
                //输入框是否可以手动输入
                if (!enableInput) {
                    $("#" + txtDistributorId).attr('disabled', true);
                    DealerSearchControl.JsonParams.thisEnableInput = "0";
                }
                else {
                    $("#" + txtDistributorId).attr('disabled', false);
                    DealerSearchControl.JsonParams.thisEnableInput = "1";
                }
                $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).keyup(function () {
                    ClearAttrValue(this);
                });
            }
        }
    };

    var ClearAttrValue = function (obj) {
        var new_value = $.trim($(obj).val());
        var old_value = $.trim($(obj).attr("attrMemberName"));
        if (new_value == old_value) {
            //无操作            
        }
        else {
            //清空四个属性
            $(obj).attr('attrMemberCode', "");
            $(obj).attr('attrMemberName', "");
            $(obj).attr('attrCustName', "");
            $(obj).attr('attrCustID', "");
        }
        return;
    };
    var SetMemberCode = function (memberCode) {  //设置MemberCode属性
        DealerSearchControl.JsonParams.thisMemberCode = memberCode;
        SetDealerInfoByMemberCode();
    };
    var SetMemberName = function (memberName) {  //设置MemberName属性
        DealerSearchControl.JsonParams.thisMemberName = memberName;
        if (DealerSearchControl.JsonParams.thisTxtDistributorId != "") {
            $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).val(memberName);
        }
    };
    var SetDealerInfoByMemberCode = function () {
        if (DealerSearchControl.JsonParams.thisTxtDistributorId != "") {
            AjaxPostAsync("/WOrderV2/Handler/AddWOrderHandler.ashx", {
                Action: "GetDealerInfo",
                MemberCode: DealerSearchControl.JsonParams.thisMemberCode,
                r: Math.random()
            }, null, function (returnValue) {
                if (returnValue) {
                    var backData = $.evalJSON(returnValue);
                    if (backData.result) {
                        var txtDistributorObj = $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId);
                        txtDistributorObj.val(backData.data.name);
                        txtDistributorObj.attr('attrCustName', backData.data.CustName);
                        txtDistributorObj.attr('attrCustID', backData.data.CustID);
                        txtDistributorObj.attr('attrMemberCode', DealerSearchControl.JsonParams.thisMemberCode);
                        txtDistributorObj.attr('attrMemberName', backData.data.name);
                        DealerSearchControl.JsonParams.thisMemberName = backData.data.name;
                    }
                    else {
                        $.jAlert(backData.message);
                    }
                }
            });
        }
    };
    var CheckParams = function () {
        if (DealerSearchControl.JsonParams.thisTxtDistributorId == "" ||
            DealerSearchControl.JsonParams.thisBtnDistributorId == ""
           ) {
            $.jAlert("DealerSearchControl必要的参数未初始化");
            return false;
        }
        else {
            return true;
        }
    };
    var BtnDistributorClick = function () { //经销商查询按钮事件 
        var membername = $.trim($("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).val());
        DealerSearchControl.JsonParams.thisMemberName = membername == "所属经销商" ? "" : membername;
        OpenPopupAndBind('/WOrderV2/PopLayer/DealerSelectLayer.aspx', 'DealerSelectPopup');
    };
    var BtnYaChaoClick = function () {  //亚超登录按钮点击事件
        var attrMemberCode = $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).attr('attrMemberCode');
        if (typeof (attrMemberCode) == "undefined") {  //没有选择过经销商
            $.jAlert("请选择一个经销商");
        }
        else if (attrMemberCode == "") {  //选择了经销商，但没有经销商id值
            if ($.trim(attrMemberCode) == "")  //没有经销商名称
            {
                $.jAlert("所属经销商不能为空");
            }
            else {
                $.jAlert("该经销商不是易湃会员，无法登录");
            }
        }
        else {
            try {
                var pagehost = window.location.host;
                window.external.MethodScript('/browsercontrol/newpage?url=' + escape('/AjaxServers/RYP.aspx?tid=' + attrMemberCode));
            }
            catch (e) {
                window.open("/AjaxServers/RYP.aspx?tid=" + attrMemberCode);
            }
        }
    };
    var OpenPopupAndBind = function (url, PopupName) {//打开弹出层并绑定数据（通用)
        $.openPopupLayer({
            name: PopupName,
            parameters: { CustId: DealerSearchControl.JsonParams.thisCustId,
                MemberName: DealerSearchControl.JsonParams.thisMemberName,
                MemberCode: $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId).attr('attrMemberCode')
            },
            url: url + "?r=" + Math.random(),
            popupMethod: "POST",
            beforeClose: function (e, data) {
                if (e) {
                    BindHTMLbyOjb(data);
                }
            }
        });
    };
    var ClosePopupAndBind = function () {//关闭对话框时回传的数据的处理
        $.closePopupLayer('DealerSelectPopup', true);
    };
    var BindHTMLbyOjb = function (data) {
        if (data != "") {
            var txtDistributorObj = $("#" + DealerSearchControl.JsonParams.thisTxtDistributorId);
            txtDistributorObj.val(data.MemberName);
            txtDistributorObj.bindTextDefaultMsg("所属经销商");
            txtDistributorObj.attr('attrMemberCode', data.MemberCode);
            txtDistributorObj.attr('attrMemberName', data.MemberName);
            txtDistributorObj.attr('attrCustName', data.CustName);
            txtDistributorObj.attr('attrCustID', data.CustID);
        }
    };

    //获取经销商id
    var GetCRMMemberCode = function (txtDistributorId) {
        return $.trim($("#" + txtDistributorId).attr("attrMemberCode"));
    };
    //获取经销商名称
    var GetCRMMemberName = function (txtDistributorId) {
        return $.trim($("#" + txtDistributorId).val());
    };
    //获取客户id
    var GetCRMCustID = function (txtDistributorId) {
        return $.trim($("#" + txtDistributorId).attr("attrCustID"));
    };
    //获取客户名称
    var GetCRMCustName = function (txtDistributorId) {
        return $.trim($("#" + txtDistributorId).attr("attrCustName"));
    };
    return {
        InitialEvent: InitialEvent,
        InitialEventNoParams: InitialEventNoParams,
        //设置初始数据
        SetMemberCode: SetMemberCode,
        SetMemberName: SetMemberName,
        //获取数据接口
        GetCRMMemberCode: GetCRMMemberCode,
        GetCRMMemberName: GetCRMMemberName,
        GetCRMCustID: GetCRMCustID,
        GetCRMCustName: GetCRMCustName,
        JsonParams: jsonParams
    }
})();

//**************************************************** 选择标签控件  ****************************************************
// 李晓民 2016-7-27
//此控件至少需要初始化1个参数：{tag:},tag（标签控件id）,
//
//new TagSelectControl({}) 初始化， 例如  var tag = new TagSelectControl({ tag: "TagName" });
//Close 关闭,                       例如  tag.Close();
//GetValue 获取选中的值,GetTagId,GetTagName
//Disable 设置控件是否可用          例如 tag.Disable(true);
//Clear 清空id、name
//SetTagNameById 根据id获取名称     例如 tag.SetTagNameById(id);
//SetBusiTypeId 设置业务类型        例如 tag.SetBusiTypeId(2);
//**************************************************************************************************************************
TagSelectControl = function (option) {
    var option = option || {
        busitype: "", //业务类型控件id
        tag: "", //文本框控件id
        trigger: "", //触发器控件id
        afterClose: null //关闭页面事件回调
    };

    //业务类型id
    var vbusitypeid = "";
    //弹层名称
    var layerCode = "SelectTagLayer";
    //返回值
    var jsonData = { tagid: "", tagname: "" };

    //业务类型控件id
    var cbusitypeid = option.busitype;
    //文本框控件id
    var ctagid = option.tag;
    //触发器控件id
    var ctriggerid = option.trigger;
    if (!ctriggerid) {
        ctriggerid = option.tag;
    }

    this.SetBusiTypeId = function (value) {
        if (value) {
            if (value != vbusitypeid) {
                //更换业务类型，清空标签
                clear();
            }
            vbusitypeid = value;
        }
    };
    this.Close = function () {
        $.closePopupLayer(layerCode, false);
    };
    this.SetValue = function (value, text) {
        jsonData.tagid = value;
        jsonData.tagname = text;
    };
    //获取选中的值
    this.GetValue = function () {
        return jsonData = jsonData || {};
    }
    //获取选中的值
    this.GetTagId = function () {
        var val = $("#" + ctagid).attr("tagid");
        return val = val || "";
    };
    //获取选中的值
    this.GetTagName = function () {
        var val = $("#" + ctagid).val();
        return val = val || "";
    };
    //是否可用
    this.Disable = function (flag) {
        disable(flag);
    };
    this.Clear = function () {
        clear();
    };
    //根据id获取名称
    this.SetTagNameById = function (id) {
        if (id && id > 0) {
            AjaxPost("/WOrderV2/Handler/TagHandler.ashx", { action: "GetData", CurrId: id }, null, function (data) {
                if (data) {
                    data = JSON.parse(data);
                    if (data.Success == true) {
                        var rdata = { tagid: data.Data[0].RecID, tagname: data.Data[0].TagName };
                        returnValue(rdata);
                    }
                    else {
                        returnValue({ tagid: "", tagname: "" });
                    }
                }
                else {
                    returnValue({ tagid: "", tagname: "" });
                }
            });
        }
        else {
            returnValue({ tagid: "", tagname: "" });
        }
    };

    var clear = function () {
        $("#" + ctagid).attr("tagid", "");
        $("#" + ctagid).val("");
    };
    var disable = function (flag) {
        controlEvent(flag);
    };
    //控件绑定事件
    var controlEvent = function (flag) {
        if (flag) {
            //标签弹层
            $("#" + ctriggerid).bind("click", function () {
                show();
            });

            //改变业务类型时，清空标签
            if (cbusitypeid) {
                $("#" + cbusitypeid).bind("change", function () {
                    clear();
                });
            }
        }
        else {
            $("#" + ctriggerid).die("click");
        }
    };

    var returnValue = function (data) {
        jsonData = data;
        $("#" + ctagid).attr("tagid", jsonData.tagid);
        $("#" + ctagid).val(jsonData.tagname);
        $("#" + ctagid).bindTextDefaultMsg("请选择标签");
    }

    var show = function () {
        //控件值
        var vtagid = $("#" + ctagid).attr("tagid");
        var vtagname = $("#" + ctagid).val();
        if (!vbusitypeid || vbusitypeid == "" || vbusitypeid == "-1") {
            return false;
        }
        $.openPopupLayer({
            name: layerCode,
            parameters: { busitypeid: vbusitypeid, tagid: vtagid },
            url: "/WOrderV2/PopLayer/SelectTagLayer.aspx",
            beforeClose: function (e, data) {
                if (e) {
                    returnValue(data);
                }
                if (option.afterClose != null) {
                    option.afterClose(e, data)
                }
            }
        });
    };
    var init = function () {
        disable(true);
    };
    init();
};

//**************************************************** 电话呼入呼出控件  ****************************************************
// 电话控件
//需要在初始化页面时调用Init方法
//外呼实现CallOut方法
//需要调用SetInfoFunc设置，指定如何获取个人用户信息
//如果，获取方式固定，则需要在初始化时，设置一次即可
//如果获取方式实时变化，需要在调用CallOut前，重新调用SetInfoFunc并设置
//可以调用SetEstablishedEvent注册接通事件
//可以调用SetReleaseEvent注册挂断事件
//***************************************************************************************************************************
HollyPhoneControl = (function () {
    var url = "/AjaxServers/CommonCallHandler.ashx";
    //基础数据存储 NoSave:代表不存储
    var data = {
        ADTTool: ADTTool, //电话控CTI件
        TaskType: "", //任务类型：客户核实，其他任务，厂家集客，工单
        TaskID: "", //选填，如何不填，需要任务页面进行绑定
        BGID: "当前分组", //不是数字，默认为当前分组
        SCID: "当前分类", //不是数字，默认为当前分类

        //=============外呼时，输入==============//
        CustTypeFunc: null, //客户类别：个人=4，经销商=3 【挂断时调用该方法获取】
        CBNameFunc: null, //选填 姓名 【挂断时调用该方法获取】
        CBSexFunc: null, //选填 性别：先生=1，女士=2 【挂断时调用该方法获取】
        CBMemberCodeFunc: null, //选填：经销商id 【挂断时调用该方法获取】
        CBMemberNameFunc: null, //选填：经销商名称 【挂断时调用该方法获取】

        //=============事件==============//
        EstablishedEvent: null, //呼入接通事件
        ReleaseEvent: null, //挂断事件
        SendMsgToWindowsEvent: null //转发消息事件
    };
    //初始化函数（任务类型，任务ID，分组，分类）
    //任务ID不为空，绑定话务和任务关系
    //如果任务ID为空，需要调用方在生成任务的时候，主动绑定话务和任务关系
    var Init = function (tasktype, taskid, bgid, scid, crmcustid) {
        data.TaskType = $.trim(tasktype);
        data.TaskID = $.trim(taskid);
        data.CRMCustID = $.trim(crmcustid);
        if (bgid && bgid > 0) {
            data.BGID = $.trim(bgid);
        }
        if (scid && scid > 0) {
            data.SCID = $.trim(scid);
        }

        //内部处理：接通事件实现
        ADTTool.EstablishedForCallComing = function (calldata) {
            CallSave(calldata, data.EstablishedEvent);
        };
        ADTTool.Established = function (calldata) {
            CallSave(calldata, data.EstablishedEvent);
        };
        //内部处理：挂断事件实现
        ADTTool.onDisconnected = function (calldata) {
            CallSave(calldata, data.ReleaseEvent);
        };
        //内部处理：转发消息事件实现
        ADTTool.SendMsgToWindows = function (msg) {
            if (data.SendMsgToWindowsEvent && typeof data.SendMsgToWindowsEvent == "function") {
                data.SendMsgToWindowsEvent(msg);
            }
        };
    };
    //设置备用信息获取方法（和短信控件的逻辑保持一致）
    var SetInfoFunc = function (custtype_func, cbname_func, cbsex_func, membercode_func, membername_func, crmcustid) {
        //选填
        data.CustTypeFunc = custtype_func;
        data.CBNameFunc = cbname_func;
        data.CBSexFunc = cbsex_func;
        data.CBMemberCodeFunc = membercode_func;
        data.CBMemberNameFunc = membername_func;
        //不为空就赋值
        if ($.trim(crmcustid) != "") {
            data.CRMCustID = $.trim(crmcustid);
        }
    };
    //外呼操作
    var CallOut = function (phone) {
        phone = phone.replace(/\-/g, "");
        if (phone == "") {
            $.jAlert('电话号码为空！');
            return;
        }
        else if (!isTelOrMobile(phone)) {
            $.jAlert('电话号码格式不正确！');
            return;
        }
        if (Validate()) {
            //外呼
            try {
                ADTTool.MethodScript('/CallControl/MakeCall?targetdn=' + phone + '&OutShowTag=' + 0);
            }
            catch (e) {
                $.jAlert('外呼功能不可用！');
            }
        }
    };

    //对外事件-接通保存成功事件
    var SetEstablishedEvent = function (event) {
        if (event && typeof event == "function") {
            data.EstablishedEvent = event;
        }
    };
    //对外事件-挂断保存成功事件
    var SetReleaseEvent = function (event) {
        if (event && typeof event == "function") {
            data.ReleaseEvent = event;
        }
    };
    //对外事件-接收其他页面消息事件
    var SetSendMsgToWindowsEvent = function (event) {
        if (event && typeof event == "function") {
            data.SendMsgToWindowsEvent = event;
        }
    };

    //内部处理：参数校验（电话控件对象，任务类型，手机号码，客户类别必填）
    var Validate = function () {
        var msg = "";
        var result = true;
        if (data.ADTTool == null) {
            msg = "电话CTI控件加载失败！";
            result = false;
        }
        //任务类型：客户核实，其他任务，厂家集客，工单
        else if (data.TaskType != "客户核实" && data.TaskType != "其他任务" && data.TaskType != "厂家集客" && data.TaskType != "工单") {
            msg = "任务类型数据不正确！";
            result = false;
        }
        else if (GetCustType() == "") {
            msg = "客户类型必填！";
            result = false;
        }
        if (result == false) {
            $.jAlert(msg);
        }
        return result;
    };

    //数据访问： //更新CustBasicInfo 3表 //新增或者更新CallRecord_ORIG_Business表//如果接通：新增或者更新CallRecordInfo表
    var CallSave = function (calldata, callback) {
        var jsondata = GetJsonData(calldata);
        //Log(jsondata);
        //构造数据
        var pody = {
            Action: "CallSaveEvent",
            JsonData: escape(JSON.stringify(jsondata)), //json数据
            R: Math.random()
        };
        //异步保存
        AjaxPost(url, pody, null, function (data) {
            var jsonData = $.evalJSON(data);
            if (jsonData.result) {
                //触发接通保存成功事件
                if (callback && typeof callback == "function") {
                    callback(jsondata);
                }
            }
            else {
                $.jAlert(jsonData.message, function () { });
            }
        });
    };

    //内部处理：构建数据
    var GetJsonData = function (calldata) {
        var pody = {
            //页面数据
            PageData: {
                TaskType: data.TaskType,
                TaskID: data.TaskID,
                BGID: data.BGID,
                SCID: data.SCID,
                CRMCustID: data.CRMCustID,
                //备用信息
                CustType: GetCustType(),
                CBName: GetCBName(),
                CBSex: GetCBSex(),
                CBMemberCode: GetCBMemberCode(),
                CBMemberName: GetCBMemberName()
            },
            //话务数据
            CallData: {
                UserEvent: $.trim(calldata.UserEvent),
                ExtensionNum: $.trim(calldata.UserName),
                Beijiao: $.trim(calldata.CalledNum),
                Zhujiao: $.trim(calldata.CallerNum),
                CallID: $.trim(calldata.CallID),
                SkillGroup: $.trim(calldata.UserChoice),
                CallType: $.trim(calldata.CallType),
                CurrentDate: $.trim(unescape(calldata.CurrentDate)),
                LuoDiHao: $.trim(calldata.SYS_DNIS),
                SessionID: $.trim(calldata.RecordID),
                AudioURL: $.trim(calldata.RecordIDURL),
                IsEstablished: $.trim(calldata.IsEstablished),
                EstablishedStartTime: $.trim(unescape(calldata.EstablishedStartTime)),
                TaskID: $.trim(calldata.TaskID),
                TaskType: $.trim(calldata.TaskType)
            }
        };
        return pody;
    };
    //内部处理：获取数据
    var GetCustType = function () { if (data.CustTypeFunc && typeof data.CustTypeFunc == "function") { return data.CustTypeFunc(); } else { return ""; } };
    var GetCBName = function () { if (data.CBNameFunc && typeof data.CBNameFunc == "function") { return data.CBNameFunc(); } else { return ""; } };
    var GetCBSex = function () { if (data.CBSexFunc && typeof data.CBSexFunc == "function") { return data.CBSexFunc(); } else { return ""; } };
    var GetCBMemberCode = function () { if (data.CBMemberCodeFunc && typeof data.CBMemberCodeFunc == "function") { return data.CBMemberCodeFunc(); } else { return ""; } };
    var GetCBMemberName = function () { if (data.CBMemberNameFunc && typeof data.CBMemberNameFunc == "function") { return data.CBMemberNameFunc(); } else { return ""; } };
    //log方法
    var Log = function (jsondata) {
        var logs = "PageData\r\n";
        $.each(jsondata.PageData, function (i, n) {
            logs += (i + "=" + n) + "\r\n";
        });
        logs += "CallData\r\n";
        $.each(jsondata.CallData, function (i, n) {
            logs += (i + "=" + n) + "\r\n";
        });
        alert(logs);
    };
    //测试方法
    var Test = function () {
        data.ADTTool.onDisconnected({});
    };

    return {
        Init: Init,
        CallOut: CallOut,
        SetInfoFunc: SetInfoFunc,
        SetEstablishedEvent: SetEstablishedEvent,
        SetReleaseEvent: SetReleaseEvent,
        SetSendMsgToWindowsEvent: SetSendMsgToWindowsEvent,
        Test: Test
    }
})();

//****************************************************  发送短信控件  ****************************************************
//发送短信控件，需要在初始化页面时调用Init方法，默认采用新版样式，如果要采用老板样式，需要单独设置url值
//url值标示弹层位置
//BtnSendMessageClick 发送短信具体逻辑
//发送短信前，需要调用SetInfoFunc，指定如何获取个人用户信息
//如果，获取方式固定，则需要在初始化时，设置一次即可
//如果获取方式实时变化，需要在调用BtnSendMessageClick前，重新调用SetInfoFunc并设置
//如果pagetype=1，发送短信之前，需要调用SetTemplateFunc设置模板信息，动态生成短信内容
//如果，获取方式固定，则需要在初始化时，设置一次即可
//如果获取方式实时变化，需要在调用SetInfoFunc前，重新调用SetTemplateFunc并设置
//可以设置SendMessageCompleteEvent，注册发送成功事件
//************************************************************************************************************************
SendMessageControl = (function () {
    //基础数据存储 NoSave:代表不存储
    var data = {
        TaskType: "", //任务类型：客户核实，其他任务，厂家集客，工单
        TaskID: "", //选填，如何不填，需要任务页面进行绑定
        BGID: "当前分组", //不是数字，默认为当前分组
        SCID: "当前分类", //不是数字，默认为当前分类

        CustTypeFunc: null, //客户类别：个人=4，经销商=3 【挂断时调用该方法获取】
        CBNameFunc: null, //选填 姓名 【挂断时调用该方法获取】
        CBSexFunc: null, //选填 性别：先生=1，女士=2 【挂断时调用该方法获取】
        CBMemberCodeFunc: null, //选填：经销商id 【挂断时调用该方法获取】
        CBMemberNameFunc: null, //选填：经销商名称 【挂断时调用该方法获取】

        SendMessageCompleteEvent: null, //发送成功回调事件
        r: Math.random()
    };
    //初始化 任务类型：客户核实，其他任务，厂家集客，工单
    var Init = function (tasktype, taskid, bgid, scid, crmcustid, url) {
        data.TaskType = $.trim(tasktype);
        data.TaskID = $.trim(taskid);
        data.CRMCustID = $.trim(crmcustid);
        if (bgid && bgid > 0) {
            data.BGID = $.trim(bgid);
        }
        if (scid && scid > 0) {
            data.SCID = $.trim(scid);
        }
        //弹层url
        if ($.trim(url) == "") {
            data.url = "/WOrderV2/PopLayer/SendMessageLayer.aspx";
        }
        else {
            data.url = url;
        }
    };
    //内部处理：构建数据
    var GetJsonData = function (phone, pagetype) {
        var pody = {
            //页面数据
            PageData: {
                TaskType: data.TaskType,
                TaskID: data.TaskID,
                BGID: data.BGID,
                SCID: data.SCID,
                CRMCustID: data.CRMCustID,
                //备用信息
                CustType: GetCustType(),
                CBName: GetCBName(),
                CBSex: GetCBSex(),
                CBMemberCode: GetCBMemberCode(),
                CBMemberName: GetCBMemberName()
            },
            //短信数据
            SMSData: {
                Phone: phone,
                PageType: pagetype,
                MemberName: GetMemberName(),
                MemberAddress: GetMemberAddress(),
                MemberTel: GetMemberTel()
            }
        };
        return pody;
    };
    //pageType 1 有模板 2 无模板
    var BtnSendMessageClick = function (phone, pagetype) {
        if (phone == "") {
            $.jAlert('电话号码为空！');
            return;
        }
        else if (!isTelOrMobile(phone)) {
            $.jAlert('电话号码格式不正确！');
            return;
        }
        else if (pagetype != 1 && pagetype != 2) {
            $.jAlert('发送类型参数不正确！');
            return;
        }
        if (Validate()) {
            var jsondata = GetJsonData(phone, pagetype);
            $.openPopupLayer({
                name: "SendSMSPopup",
                parameters: { JsonData: escape(JSON.stringify(jsondata)), R: Math.random() },
                url: data.url,
                beforeClose: function (e, result) {
                    if (e) {
                        if (data.SendMessageCompleteEvent && typeof data.SendMessageCompleteEvent == "function") {
                            data.SendMessageCompleteEvent(result.RecID, jsondata);
                        }
                    }
                }
            });
        }
    };
    //内部处理：参数校验（电话控件对象，任务类型，手机号码，客户类别必填）
    var Validate = function () {
        var msg = "";
        var result = true;
        //任务类型：客户核实，其他任务，厂家集客，工单
        if (data.TaskType != "客户核实" && data.TaskType != "其他任务" && data.TaskType != "厂家集客" && data.TaskType != "工单" && data.TaskType != "无任务") {
            msg = "任务类型数据不正确！";
            result = false;
        }
        else if (GetCustType() == "") {
            msg = "客户类型必填！";
            result = false;
        }
        if (result == false) {
            $.jAlert(msg);
        }
        return result;
    };

    //设置备用信息获取方法(和电话控件的逻辑保持一致)
    var SetInfoFunc = function (custtype_func, cbname_func, cbsex_func, membercode_func, membername_func, crmcustid) {
        //选填
        data.CustTypeFunc = custtype_func;
        data.CBNameFunc = cbname_func;
        data.CBSexFunc = cbsex_func;
        data.CBMemberCodeFunc = membercode_func;
        data.CBMemberNameFunc = membername_func;
        //不为空就赋值
        if ($.trim(crmcustid) != "") {
            data.CRMCustID = $.trim(crmcustid);
        }
    };
    //内部处理：获取数据
    var GetCustType = function () { if (data.CustTypeFunc && typeof data.CustTypeFunc == "function") { return data.CustTypeFunc(); } else { return ""; } };
    var GetCBName = function () { if (data.CBNameFunc && typeof data.CBNameFunc == "function") { return data.CBNameFunc(); } else { return ""; } };
    var GetCBSex = function () { if (data.CBSexFunc && typeof data.CBSexFunc == "function") { return data.CBSexFunc(); } else { return ""; } };
    var GetCBMemberCode = function () { if (data.CBMemberCodeFunc && typeof data.CBMemberCodeFunc == "function") { return data.CBMemberCodeFunc(); } else { return ""; } };
    var GetCBMemberName = function () { if (data.CBMemberNameFunc && typeof data.CBMemberNameFunc == "function") { return data.CBMemberNameFunc(); } else { return ""; } };
    //设置模板发送短信时的必须值
    var SetTemplateFunc = function (membername_func, memberaddress_func, membertel_func) {
        data.MemberNameFunc = membername_func;
        data.MemberAddressFunc = memberaddress_func;
        data.MemberTelFunc = membertel_func;
    }
    //内部处理：获取数据
    var GetMemberName = function () { if (data.MemberNameFunc && typeof data.MemberNameFunc == "function") { return data.MemberNameFunc(); } else { return ""; } };
    var GetMemberAddress = function () { if (data.MemberAddressFunc && typeof data.MemberAddressFunc == "function") { return data.MemberAddressFunc(); } else { return ""; } };
    var GetMemberTel = function () { if (data.MemberTelFunc && typeof data.MemberTelFunc == "function") { return data.MemberTelFunc(); } else { return ""; } };

    //设置完成事件
    var SetSendMessageCompleteEvent = function (event) {
        if (event && typeof event == "function") {
            data.SendMessageCompleteEvent = event;
        }
    };
    return {
        Init: Init,
        BtnSendMessageClick: BtnSendMessageClick,
        SetSendMessageCompleteEvent: SetSendMessageCompleteEvent,
        SetInfoFunc: SetInfoFunc,
        SetTemplateFunc: SetTemplateFunc
    }
})();

//****************************************************个人用户保存控件****************************************************
//王同海 2016-8-18
//*************************************************************************************************************************
CustBaseInfoPopControl = (function () {
    var url = "/AjaxServers/CustBasicInfoPop.aspx";
    var SaveCompleteEvent = null;
    var SetSaveCompleteEvent = function (event) {
        if (event && typeof event == "function") {
            SaveCompleteEvent = event;
        }
    };
    var Open = function (phone, custtype, crmcustid, cbname, cbsex) {
        phone = $.trim(phone);
        custtype = $.trim(custtype);
        crmcustid = $.trim(crmcustid);
        cbname = $.trim(cbname);
        cbsex = $.trim(cbsex);

        if (phone == "") {
            $.jAlert("电话号码不能为空！");
            return;
        }
        if (!isTelOrMobile(phone)) {
            $.jAlert("电话号码格式不正确！");
            return;
        }
        if (custtype != "经销商" && custtype != "个人" && custtype != "3" && custtype != "4") {
            $.jAlert("客户类型必须是经销商或者个人！");
            return;
        }
        if (custtype == "经销商" || custtype == "3") {
            if (crmcustid == "") {
                $.jAlert("经销商类型下客户ID不能为空！");
                return;
            }
        }
        $.openPopupLayer({
            name: "OperCustInfoPop",
            parameters: { CRMCustID: crmcustid, CustName: cbname, Tel: phone, Sex: cbsex, CustType: custtype, r: Math.random() },
            url: url,
            beforeClose: function (e) {
                if (e) {
                    var custid = $('#popupLayer_' + 'OperCustInfoPop').data('popCustID');
                    if (SaveCompleteEvent && typeof SaveCompleteEvent == "function") {
                        SaveCompleteEvent(custid);
                    }
                }
            }
        });
    };

    return {
        SetSaveCompleteEvent: SetSaveCompleteEvent,
        Open: Open
    };
})();

//crm外呼统一实现方法（需要页面注册HollyPhoneControl控件）
function CallOutForCRM(phone, membercode, membername, crmcustid, cbname, cbsex) {
    phone = $.trim(phone);
    membercode = $.trim(membercode);
    membername = $.trim(membername);
    crmcustid = $.trim(crmcustid);
    cbname = $.trim(cbname);
    cbsex = $.trim(cbsex);
    if (phone == "") {
        $.jAlert("电话号码为空，不能外呼！");
        return;
    }
    if (crmcustid == "") {
        $.jAlert("CRM客户信息为空，不能外呼！");
    }
    //转换性别 crm 的性别是 0男1女 cc的性别是 1男2女
    if (cbsex == 0) {
        cbsex = 1;
    }
    else if (cbsex == 1) {
        cbsex = 2;
    }
    else {
        cbsex = -1;
    }
    //alert(phone + " " + membercode + " " + membername + " " + crmcustid + " " + cbname + " " + cbsex);
    //注册个人用户信息获取方法
    HollyPhoneControl.SetInfoFunc(
    //客户类型
            function () {
                return "3";
            },
    //客户姓名
            function () {
                return cbname;
            },
    //客户性别
            function () {
                return cbsex;
            },
    //经销商id
            function () {
                return membercode;
            },
    //经销商名称
            function () {
                return membername;
            },
    //crm客户id
            crmcustid
            );
    //外呼
    HollyPhoneControl.CallOut(phone);
}
//crm联系人短信统一实现方法（需要页面注册SendMessageControl控件）
function SendSmSForCRM(phone, membercode, membername, crmcustid, cbname, cbsex) {
    phone = $.trim(phone);
    membercode = $.trim(membercode);
    membername = $.trim(membername);
    crmcustid = $.trim(crmcustid);
    cbname = $.trim(cbname);
    cbsex = $.trim(cbsex);
    if (phone == "") {
        $.jAlert("电话号码为空，不能外呼！");
        return;
    }
    if (crmcustid == "") {
        $.jAlert("CRM客户信息为空，不能外呼！");
    }
    //转换性别 crm 的性别是 0男1女 cc的性别是 1男2女
    if (cbsex == 0) {
        cbsex = 1;
    }
    else if (cbsex == 1) {
        cbsex = 2;
    }
    else {
        cbsex = -1;
    }
    //alert(phone + " " + membercode + " " + membername + " " + crmcustid + " " + cbname + " " + cbsex);
    //注册个人用户信息获取方法
    SendMessageControl.SetInfoFunc(
    //客户类型
            function () {
                return "3";
            },
    //客户姓名
            function () {
                return cbname;
            },
    //客户性别
            function () {
                return cbsex;
            },
    //经销商id
            function () {
                return membercode;
            },
    //经销商名称
            function () {
                return membername;
            },
    //crm客户id
            crmcustid
            );
    //短信
    SendMessageControl.BtnSendMessageClick(phone, 2);
}
//保存个人信息弹层统一处理
function SaveCustBasicInfoPoP(jsondata) {
    //姓名为空 或者 经销商：code为空
    //经销商类型=3
    if (jsondata.PageData.CBName == "" || (jsondata.PageData.CBSex != 1 && jsondata.PageData.CBSex != 2) || (jsondata.PageData.CustType == "3" && jsondata.PageData.CBMemberCode == "")) {
        //弹层
        //获取电话
        var phone = "";
        if (jsondata.CallData) {
            phone = jsondata.CallData.Beijiao == jsondata.CallData.ExtensionNum ? jsondata.CallData.Zhujiao : jsondata.CallData.Beijiao;
        }
        else if (jsondata.SMSData) {
            phone = jsondata.SMSData.Phone;
        }
        CustBaseInfoPopControl.Open(phone, jsondata.PageData.CustType, jsondata.PageData.CRMCustID, jsondata.PageData.CBName, jsondata.PageData.CBSex);
    }
}

//****************************************************  免打扰控件  ****************************************************
//此控件至少需要初始化四个参数：thisTxtUserPhoneId（电话号码所在的控件id）、
//                              thissBtnOpenLayerId(触发打开免打扰层事件的控件ID)、
//                              thisCallIdFunc(打完电话的CallID)
//************************************************************************************************************************
NoDisturbLayerControl = (function () {
    var jsonParams = {
        "thisTxtUserPhoneId": "",
        "thissBtnOpenLayerId": "",
        "thisCallIdFunc": ""
    },
    InitialEventNoParams = function () {  //按钮绑定事件 
        CheckParamsAndBindEvent();
    },
    InitialEvent = function (txtUserPhoneId, btnOpenLayerId, callIdFunc) {
        //按钮绑定事件
        NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId = txtUserPhoneId;
        NoDisturbLayerControl.JsonParams.thissBtnOpenLayerId = btnOpenLayerId;
        NoDisturbLayerControl.JsonParams.thisCallIdFunc = callIdFunc;
        CheckParamsAndBindEvent();
    },
    CheckParamsAndBindEvent = function () {
        //检验必填项
        if (NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId == "" ||
            NoDisturbLayerControl.JsonParams.thissBtnOpenLayerId == "" ||
            NoDisturbLayerControl.JsonParams.thisCallIdFunc == null ||
            typeof NoDisturbLayerControl.JsonParams.thisCallIdFunc != "function") {
            $.jAlert("NoDisturbLayerControl必要的参数未初始化");
        }
        else {
            $("#" + NoDisturbLayerControl.JsonParams.thissBtnOpenLayerId).click(BtnOpenLayerClick);
        }
    },
    BtnOpenLayerClick = function () {
        var phone = $.trim($("#" + NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId).val());
        if (phone == "") {
            $.jAlert("电话号码为空！", function () { $("#" + NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId).focus(); });
            return;
        }
        else if (!isTelOrMobile(phone)) {
            $.jAlert("电话号码格式不正确！", function () { $("#" + NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId).focus(); });
            return;
        }
        if (NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId != "") {
            AjaxPostAsync("/WOrderV2/Handler/AddWOrderHandler.ashx", { Action: "phonenumisnodisturb", Phone: $.trim($("#" + NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId).val()) }, null, function (returnValue) {
                /// 判断此号码是否为免打扰号码：-1：是已删除的免打扰号码；0：是免打扰号码；1：是已过期的免打扰号码；2：不是免打扰号码
                switch (returnValue) {
                    case "0":
                        $.jAlert("当前号码已被设置为免打扰，请勿重复添加！");
                        break;
                    case "1":
                        $.jConfirm("当前号码已被设置为免打扰，且有效期已失效,点击“确定”按钮，更新号码信息？", function (r) {
                            if (r) {
                                OpenPopupAndBind();
                            }
                        });
                        break;
                    default:
                        OpenPopupAndBind();
                        break;
                }
            });
        }
    },
    OpenPopupAndBind = function () {
        var callid = $.trim(NoDisturbLayerControl.JsonParams.thisCallIdFunc());
        if (callid == "") {
            $.jAlert("通话中无法添加，请通话结束后进行操作！"); // 获取不到话务数据，无法添加免打扰！
            return false;
        }
        if (NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId != "") {
            $.openPopupLayer({
                name: "UpdateBlackDataAjaxPopup",
                parameters: {
                    CallId: callid,
                    PhoneNumber: $.trim($("#" + NoDisturbLayerControl.JsonParams.thisTxtUserPhoneId).val()),
                    ResponseFrom: "plugin",
                    r: Math.random()
                },
                url: "/WOrderV2/PopLayer/NoDisturbLayer.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        //if (NoDisturbLayerControl.JsonParams.thissBtnOpenLayerId != "") {
                        //  $("#" + NoDisturbLayerControl.JsonParams.thissBtnOpenLayerId).attr("disabled", true);
                        //}
                    }
                }
            });
        }
        else {
            $.jAlert("NoDisturbLayerControl必要的参数thisTxtUserPhoneId未初始化");
        }
    };
    return {
        InitialEvent: InitialEvent,
        InitialEventNoParams: InitialEventNoParams,
        JsonParams: jsonParams
    }
})();

//**************************************************** 接收/抄送人控件 ****************************************************
//此控件需要初始化1个参数：thisTxtReceiverCopyUserId（选择接收/抄送人的textbox的Id） 
//返回信息存放在Id为thisTxtReceiverCopyUserId的textbox的userids属性中
//                  thisTxtReceiverCopyUserId的textbox的UserIDuserCodeJson属性中存放了UserID和UserCode的json串
//*************************************************************************************************************************
ReceiverCopyUserSelectControl = function () {
    var jsonParams = { "thisTxtReceiverCopyUserId": "", "thisLimtSelectCount": "" },
    InitialEventNoParams = function () {  //按钮绑定事件 
        CheckParamsAndBindEvent();
    },
    InitialEvent = function (txtReceiverCopyUserId, limitSelectCount) {  //按钮绑定事件
        jsonParams.thisTxtReceiverCopyUserId = txtReceiverCopyUserId;
        jsonParams.thisLimtSelectCount = limitSelectCount;
        CheckParamsAndBindEvent();
    },
    CheckParamsAndBindEvent = function () {  //检验必填项
        if (jsonParams.thisTxtReceiverCopyUserId == "") {
            $.jAlert("ReceiverCopyUserSelectControl必要的参数未初始化");
        }
        else {
            $("#" + jsonParams.thisTxtReceiverCopyUserId).click(BtnOpenLayerClick);
        }
    },
    BtnOpenLayerClick = function () {
        if (jsonParams.thisTxtReceiverCopyUserId != "") {
            var thisObj = $("#" + jsonParams.thisTxtReceiverCopyUserId);
            var userids = "";
            var attruserids = thisObj.attr("userids")
            if (typeof (attruserids) == "undefined") {  //判断属性是否存在
                thisObj.attr("userids", "");  //新增属性
            }
            userids = thisObj.attr("userids");
            $.openPopupLayer({
                name: "SelReceiveCopyUserAjaxPopup",
                parameters: { UserIDs: userids, LimitSelectCount: jsonParams.thisLimtSelectCount },
                url: "/WOrderV2/PopLayer/ReceiveCopyLayer.aspx",
                beforeClose: function (e, data) {
                    if (e) {
                        if (data != "") {
                            thisObj.attr("userids", data.Ids);
                            thisObj.attr("UserIDuserCodeJson", data.UserIDuserCodeJson);
                            thisObj.val(data.Names);
                        }
                    }
                }
            });
        }
        else {
            $.jAlert("ReceiverCopyUserSelectControl必要的参数thisTxtReceiverCopyUserId未初始化");
        }
    };
    return {
        InitialEvent: InitialEvent,
        JsonParams: jsonParams
    }
};
