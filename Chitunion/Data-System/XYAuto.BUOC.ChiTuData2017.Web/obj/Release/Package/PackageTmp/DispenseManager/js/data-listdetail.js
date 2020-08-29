/*
* Written by:     fengb
* function:       趋势分析-数据明细
* Created Date:   2017-11-27
*/

$(function () {

    var Status = GetQueryString('Status')!=null&&GetQueryString('Status')!='undefined'?GetQueryString('Status'):null;
    var elem = ['grab','jxrk','cxpp','cs','rgqx','fz','ff','zf','xs'];//当前type
    var LastYester = getthedate(new Date(),-1);//昨天
    var LastSeven = getthedate(new Date(),-7);//最近7天
    var LastThirty = getthedate(new Date(),-30);//最近30天
    var titArr = ['抓取','机洗入库','车型匹配','初筛','人工清洗','封装','分发','转发统计','线索统计'];

    function DataListDetail() {
        $('.list h2').html('趋势分析-' + titArr[Status]);
        $('.state .date_tit').html(titArr[Status] + '时间：');
        var typeArr = ['74','8001','8002','76','7','73','77'];
        for(var i = 0;i<= typeArr.length - 1;i ++){
            $.ajax({
                url : public_url + '/api/DictInfo/GetDictInfoByTypeId',
                type : 'get',
                data : {
                    typeId : typeArr[i]
                },
                async : false,
                success : function(data){
                    if(data.Status == 0){
                        var str = "<option DictId='-2'>请选择</option>";
                        var Result = data.Result;
                        for(var j = 0;j<= Result.length -1;j ++){
                            str += "<option DictId="+ Result[j].DictId +">"+ Result[j].DictName +"</option>";
                        }
                        $('.SelectOption' + i).html(str);
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                }
            })
        }
        if(Status == 2){
            var str = '<option DictId="-2">请选择</option>';
            str += '<option DictId="1">已匹配</option><option DictId="0">未匹配</option>';
            $('#MatchStatus').html(str);
        }else{
            $.ajax({
                url : public_url + '/api/DictInfo/GetDictInfoByTypeId',
                type : 'get',
                data : {
                    typeId : 77
                },
                async : false,
                success : function(data){
                    if(data.Status == 0){
                        var str = "<option DictId='-2'>请选择</option>";
                        var Result = data.Result;
                        for(var j = 0;j<= Result.length -1;j ++){
                            str += "<option DictId="+ Result[j].DictId +">"+ Result[j].DictName +"</option>";
                        }
                        $('#MatchStatus').html(str);
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                }
            })
        }
        this.queryparameters();
    }

    DataListDetail.prototype = {
        constructor: DataListDetail,
        // 获取查询参数
        queryparameters: function () {
            var _this = this;
            // 切换
            $('.tab_menu li').off('click').on('click', function () {
                var that = $(this);
                var idx = that.index();
                $(this).addClass('selected').siblings('li').removeClass('selected');

                _this.init(idx);
                _this.clickBtn(idx);
            })
            $('.tab_menu li').eq(0).click();
        },
        clickBtn : function(idx){
            var that = this;

            // 搜索
            $('#searchBtn').off('click').on('click', function () {
                var StartDate = $('#StartDate').val();
                var EndDate = $('#EndDate').val();
                var ArticleType = $('#ArticleType option:selected').attr('DictId');//头腰类型
                var ChannelId = '';//渠道
                var SceneId = $('#SceneId option:selected').attr('DictId');//场景
                var AAScoreTypeAccount = $('#AAScoreTypeAccount option:selected').attr('DictId');//分值
                var AccountName = $('#AAScoreTypeAccount option:selected').val();//分值名称
                var DiffChannel = $('#DiffChannel option:selected').attr('DictId');//分发渠道类型
                if(Status < 6){
                    ChannelId = $('#ChannelId option:selected').attr('DictId');
                }else{
                    ChannelId = $('#DiffChannel option:selected').attr('DictId'); 
                }

                if(AccountName == '请选择'){
                    AccountName = '';
                }else{
                    AccountName = $('#AAScoreTypeAccount option:selected').val();
                }
                var MatchStatus = $('#MatchStatus option:selected').attr('DictId');//状态
                var MaterialType = $('#MaterialType option:selected').attr('DictId');//物料类型

                if(StartDate == '' || EndDate == ''){
                    layer.msg('查询日期不能为空',{'time':1000});
                }else{
                    if(Status < 5){
                        //初始化的所有的变量
                        var obj = {
                            TabType : elem[Status],
                            PageSize : 20,
                            PageIndex : 1,
                            StartDate : StartDate,
                            EndDate : EndDate,
                            ArticleType : ArticleType,
                            ChannelId : ChannelId,
                            SceneId : SceneId,
                            AAScoreTypeAccount : AAScoreTypeAccount,
                            MatchStatus : MatchStatus
                        }
                    }else{
                        //初始化的所有的变量
                        var obj = {
                            ListType : elem[Status],
                            PageSize : 20,
                            PageIndex : 1,
                            BeginTime : StartDate,
                            EndTime : EndDate,
                            MaterielTypeID : MaterialType,
                            ChannelID : ChannelId,
                            SceneID : SceneId,
                            AccountName : AccountName,
                            ConditionID : MatchStatus
                        }
                    }
                    that.requestdata(idx,obj);
                }
            })
            $('#searchBtn').click();
        },
        // 请求数据
        requestdata: function (idx,obj) {
            var that = this;

            //前5个tab和后4个tab的接口由于不是一个人写的   结构都不一样
            var url = [];

            //日汇总数据 详情数据
            if(Status < 5){
                url = ['/api/TrendAnalysis/GetDailyList','/api/TrendAnalysis/GetDetailsList'];
            }else{
                if(Status == 5){
                    url = ['/api/Encapsulate/GetEncapsulateStatisticsList','/api/Encapsulate/GetEncapsulateDetailList'];
                }else if(Status == 6){
                    url = ['/api/Distribute/GetDistributeStatisticsList','/api/Distribute/GetDistributeDetailList'];
                }else if(Status == 7){
                    url = ['/api/Forward/GetForwardStatisticsList','/api/Forward/GetForwardDetailList'];
                }else if(Status == 8){
                    url = ['/api/Clue/GetClueStatisticsList','/api/Clue/GetClueDetailList'];
                }
            }

            $.ajax({
                url : public_url + url[idx],
                type : 'get',
                data : obj,
                async : false,
                beforeSend: function(){
                    $('#listLoading').html('<img src="/ImagesNew/icon_loading.gif" style="display: block;margin: 70px auto;">');
                },
                success : function (data) {
                    $('#listLoading').html('');
                    if(data.Status == 0){
                        var Result = data.Result;
                        //渲染数据
                        that.renderdata(that, Result,idx,obj);
                        if(data.Result.TotalCount > 0) {
                            that.createPageController(that, Result,idx,obj);
                        }else{
                            $('#pageContainer').html('<img src="/ImagesNew/no_data.png" style="display: block;margin: 70px auto;">');
                        }
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                }
            })


            //下载数据
            if(Status < 5){
                var download_url = ['/api/ExcelOperation/ExportDaily.aspx','/api/ExcelOperation/ExportDetails.aspx'];
                $('#download').off('click').on('click',function(){
                    var that = $(this);
                    var _url = public_url + download_url[idx] + '?TabType=' + obj.TabType + '&StartDate=' +  obj.StartDate  + '&EndDate=' +  obj.EndDate + '&ArticleType=' +  obj.ArticleType  +  '&ChannelId=' +  obj.ChannelId + '&SceneId=' + obj.SceneId + '&AAScoreTypeAccount=' + obj.AAScoreTypeAccount + '&MatchStatus=' + obj.MatchStatus + '&AccountID=' + obj.AccountID + '&ConditionID=' + obj.ConditionID + '&MaterialID=' + obj.MaterialID;
                    that.attr('href',_url);
                })
            }else{
                var ListType = '';
                var typeArr = ['fz_detail','ff_detail','zf_detail','xs_detail'];
                if(idx == 0){//日汇总
                    ListType = elem[Status];
                }else if(idx == 1){//明细
                    ListType = typeArr[Status-5];
                }
                var download_url = public_url + '/api/ExcelOperation/MaterielExportExcel.aspx';
                $('#download').off('click').on('click',function(){
                    var that = $(this);
                    var _url = download_url  + '?ListType=' + ListType + '&BeginTime=' +  obj.StartDate  + '&EndTime=' +  obj.EndDate + '&ArticleType=' +  obj.ArticleType  +  '&ChannelId=' +  obj.ChannelId + '&SceneId=' + obj.SceneId  + '&MatchStatus=' + obj.MatchStatus + '&AccountID=' + obj.AccountID + '&ConditionID=' + obj.ConditionID + '&MaterialID=' + obj.MaterialID;
                    that.attr('href',_url);
                })
            }

        },
        // 分页
        createPageController : function(that, Result,idx,obj){
            var counts = Result.TotalCount;
            $("#pageContainer").pagination(counts, {
                current_page: (obj.PageIndex ? obj.PageIndex : 1),
                items_per_page: 20, 
                callback: function (currPage) {
                    var obj1 = obj;
                    obj1.PageIndex = currPage;
                    that.requestdata(idx,obj1);
                }
            });
        },
        // 渲染数据
        renderdata: function (that, Result,idx,obj) {
            var element = elem[Status] + idx;

            if(Status < 5){
                $('.table').html(ejs.render($('#'+ element).html(), Result));  
            }else{
                var List = Result.List;
                var TitArr = [];//标题
                var NumArr = [];//值
                var NewList = {};//对象
                for(var i = 0;i<= List.length -1 ;i ++ ){
                    var item = List[i];
                    for(var j in item){
                        TitArr.push(j);
                        NumArr.push(item[j])
                    }
                }
                var TitArr = TitArr.norepeatArray();
                //将数组拆分多个子数组 
                var bigArr = sliceArray(NumArr,TitArr.length);
                //将表头和内容重新赋予新的属性  然后通过模板渲染
                NewList.Tit = TitArr;
                NewList.List = bigArr;

                if(idx == 0){
                    $('.table').html(ejs.render($('#'+ element).html(), NewList));  
                }else if(idx == 1){
                    $('.table').html(ejs.render($('#'+ element).html(), Result));
                }
            }
            $('.table .look_reason').off('click').on('click',function(e){
                e.preventDefault();
                var that = $(this);
                var reason = that.parents('tr').attr('Reason');
                if(reason == 'null' || reason == ''){
                    layer.msg('可用状态下的文章没有作废原因~',{'time':2000});
                }else{
                    layer.msg(reason,{'time':2000});
                }
            })
            //分页
            that.createPageController(that, Result,idx,obj); 
        },
        init : function(idx){//枚举
            if(idx == 0){
                $('#StartDate').val(LastThirty);
                $('#EndDate').val(LastYester);
                $('#StartDate').off('click').on('click', function () {
                    laydate({
                        fixed: false,
                        elem: '#StartDate',
                        choose: function (date) {
                            var now = new Date(date).valueOf();
                            var endTime = new Date($('#EndDate').val()).valueOf();
                            if (date > $('#EndDate').val() && $('#EndDate').val()) {
                                layer.msg('起始时间不能大于结束时间！',{'time':1000});
                                $('#StartDate').val('');
                            }else if( (endTime - now) > 30*24*60*60*1000 ){
                                layer.msg('查询日期不能超过30天！',{'time':1000});
                                $('#StartDate').val('');
                            }
                        }
                    });
                });
                $('#EndDate').off('click').on('click', function () {
                    laydate({
                        fixed: false,
                        elem: '#EndDate',
                        choose: function (date) {
                            var now = new Date(date).valueOf();
                            var beginTime = new Date($('#StartDate').val()).valueOf();
                            if (date < $('#StartDate').val() && $('#StartDate').val()) {
                                layer.msg('结束时间不能小于起始时间！',{'time':1000});
                                $('#EndDate').val('');
                            }else if( (now - beginTime) > 30*24*60*60*1000 ){
                                layer.msg('查询日期不能超过30天！',{'time':1000});
                                $('#EndDate').val('');
                            }
                        }
                    });
                });
            }else if(idx == 1){
                $('#StartDate').val(LastSeven); 
                $('#EndDate').val(LastYester);
                $('#StartDate').off('click').on('click', function () {
                    laydate({
                        fixed: false,
                        elem: '#StartDate',
                        choose: function (date) {
                            var now = new Date(date).valueOf();
                            var endTime = new Date($('#EndDate').val()).valueOf();
                            if (date > $('#EndDate').val() && $('#EndDate').val()) {
                                layer.msg('起始时间不能大于结束时间！',{'time':1000});
                                $('#StartDate').val('');
                            }else if( (endTime - now) > 7*24*60*60*1000 ){
                                layer.msg('查询日期不能超过7天！',{'time':1000});
                                $('#StartDate').val('');
                            }
                        }
                    });
                });
                $('#EndDate').off('click').on('click', function () {
                    laydate({
                        fixed: false,
                        elem: '#EndDate',
                        choose: function (date) {
                            var now = new Date(date).valueOf();
                            var beginTime = new Date($('#StartDate').val()).valueOf();
                            if (date < $('#StartDate').val() && $('#StartDate').val()) {
                                layer.msg('结束时间不能小于起始时间！',{'time':1000});
                                $('#EndDate').val('');
                            }else if( (now - beginTime) > 7*24*60*60*1000 ){
                                layer.msg('查询日期不能超过7天！',{'time':1000});
                                $('#EndDate').val('');
                            }
                        }
                    });
                });
            }
            switch (Status){
                    case "0" :
                    case "1" :
                    $('#status').hide();
                    $('#matype').hide();
                    $('#outtype').hide();
                    $('#score').hide();
                    if(idx == 0){
                        $('#scene').hide();
                    }else if(idx == 1){
                        $('#scene').show();
                    }
                    break;
                    case "2" : 
                    $('#matype').hide();
                    $('#outtype').hide();
                    $('#scene').hide();
                    $('#score').hide();
                    $('#headtype').hide();
                    if(idx == 0){
                        $('#channel').show();
                        $('#status').hide();
                    }else if(idx == 1){
                        $('#channel').show();
                        $('#status').show();
                    }
                    break;
                    case "3" : 
                    $('#matype').hide();
                    $('#outtype').hide();
                    $('#headtype').hide();
                    if(idx == 0){
                        $('#scene').hide();
                        $('#score').hide();
                        $('#status').hide();
                    }else if(idx == 1){
                        $('#scene').show();
                        $('#score').show();
                        $('#status').show();
                    }
                    break;
                    case "4" :
                    $('#matype').hide();
                    $('#outtype').hide();
                    if(idx == 0){
                        $('#headtype').show();
                        $('#scene').hide();
                        $('#score').hide();
                        $('#status').hide();
                    }else if(idx == 1){
                        $('#headtype').show();
                        $('#scene').show();
                        $('#score').show();
                        $('#status').show();
                    }
                    break;
                    case "5" : 
                    $('#headtype').hide();
                    $('#outtype').hide();
                    if(idx == 0){
                        $('#scene').hide();
                        $('#score').hide();
                        $('#status').hide();
                        $('#channel').hide();
                        $('#matype').hide();
                    }else if(idx == 1){
                        $('#scene').show();
                        $('#score').show();
                        $('#status').show();
                        $('#channel').show();
                        $('#matype').show();
                    }
                    break;
                    case "6" :
                    case "7" : 
                    if(idx == 0){
                        $('#headtype').hide();
                        $('#scene').hide();
                        $('#score').hide();
                        $('#status').hide();
                        $('#channel').hide();
                        $('#matype').hide();
                        $('#outtype').hide();
                    }else if(idx == 1){
                        $('#headtype').hide();
                        $('#scene').hide();
                        $('#score').hide();
                        $('#status').hide();
                        $('#channel').hide();
                        $('#matype').show();
                        $('#outtype').show();
                    }
                    break;
                    case "8" : 
                    if(idx == 0){
                        $('#headtype').hide();
                        $('#scene').hide();
                        $('#score').hide();
                        $('#status').hide();
                        $('#channel').show();
                        $('#matype').hide();
                        $('#channel').hide();
                    }else if(idx == 1){
                        $('#headtype').hide();
                        $('#scene').show();
                        $('#score').show();
                        $('#status').hide();
                        $('#channel').hide();
                        $('#matype').show();
                        $('#channel').show();
                    }
                    break;
                    default:break;
            }            
        }
    }
    var DataListDetail = new DataListDetail();

})


//获取url 地址参数方法
function GetQueryString(name) {
    var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if(r!=null)return r[2]; return null;
}

//数组去重
 Array.prototype.norepeatArray=function(){
    var obj={},temp=[];//创建临时对象和数组
    //循环遍历数组
    for(var i=0;i<this.length;i++){
        if(!obj[this[i]]){
            temp.push(this[i]);
            obj[this[i]]=true;
        }
    }
    return temp;
}


/*
 * 将一个数组分成几个同等长度的数组
 * array[分割的原数组]
 * size[每个子数组的长度]
 */function sliceArray(array, size) {
    var result = [];
    for (var x = 0; x < Math.ceil(array.length / size); x++) {
        var start = x * size;
        var end = start + size;
        result.push(array.slice(start, end));
    }
    return result;
}



/* 获取  昨天  最近七天  最近三十天*/
function getthedate(dd,dadd){
    //可以加上错误处理
    var a = new Date(dd)
    a = a.valueOf()
    a = a + dadd * 24 * 60 * 60 * 1000
    a = new Date(a);
    var m = a.getMonth() + 1;
    if(m.toString().length == 1){
        m='0'+m;
    }
    var d = a.getDate();
    if(d.toString().length == 1){
        d='0'+d;
    }
    return a.getFullYear() + "-" + m + "-" + d;
}