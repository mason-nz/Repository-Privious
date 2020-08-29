/**
 * Created by liushuai on 2017/3/4.
 */

    var index = 1;
window.onload=function () {
    var parameter = window.location.search;
    var arr = parameter.split('&');
    var MediaID = arr[0].split('MediaID=')[1];
    /*控制全选按钮*/
    $('#checkAll').click(function () {
        if($(this).prop('checked')==true){
            $('.table_list input:checkbox').prop('checked',true)
        }else{
            $('.table_list input:checkbox').prop('checked',false)
        }
    });

    setAjax({
        url:'/api/Periodication/GetAppMediaByMediaID',
        type:'get',
        data:{MediaID:MediaID}
    },function (data) {
        var res = data.Result;
        var pubID = data.Result.PubID;
        $('.journal ul li').eq(0).attr('pubID',pubID);
        $('.journal ul li').eq(0).attr('mediaID',MediaID);
        $('.journal ul li').eq(0).find('a').text(res.MediaName.slice(0,10));
        $('.journal ul li').eq(1).text('刊例类型 : '+res.MediaType);
        $('.journal ul li').eq(2).text('刊例执行期：'+res.BeginTime+' 至 '+res.EndTime);
        $('.journal ul li').eq(3).text('销售折扣：'+(res.SaleDiscount*100)+'%');
        $('.journal ul li').eq(4).text('采购折扣：'+(res.PurchaseDiscount*100)+'%');
        $('.journal ul li').eq(5).text('状态：'+res.StatusName);
        controlBtnShow();
        var searchToList = function () {
            searchList({
                PubID:pubID,
                AdPosition:'',
                AdForm:'',
                Style:'',
                PublishStatus:0,
                pagesize:20,
                PageIndex:1
            });
        };
        /*跳转ViewOfAPP详情页*/
        var pubID = $('.journal ul li').eq(0).attr('PubID');
        $('.transViewOfAPP').attr('href','/MediaManager/ViewOfMediaPlateform.html?MediaType=14002&PubID='+pubID);
        if(res.StatusName=='待审核'){
            // CTLogin.RoleIDs ='SYS001RL00003';
            /*媒体主不显示操作*/
            if( CTLogin.RoleIDs=='SYS001RL00003'){
                $('.fr').css('display','none');
            }
        }
        if(res.StatusName == '已通过'){
            $('.assign').css('display','block');
            $('#upDownState').css('display','block');
            searchToList();//已通过状态下列表2
            copyThis();//绑定复制按钮
            modifyTransAdd();//绑定添加广告位
            modifyPublish();//绑定刊例编辑APP
        }else{
            $('.assign').css('display','none');
            searchToList();//查询列表1
            copyThis();
            modifyTransAdd();
            modifyPublish();
        }
    });

};
var parameter = window.location.search;
var arr = parameter.split('&');
var MediaID = arr[0].split('MediaID=')[1];
/*控制按钮显示*/
function controlBtnShow() {
    var state = $('#state').text();
    $('.fr span').css('display','none');
    $('.copy').css('display','none');
    $('.modify').css('display','none');
    $('.putOn').css('display','none');
    $('.lookUp').css('display','none');
    if(state.indexOf('新建')!=-1){
        $('.newBuild').css('display','inline-block');
        $('.copy').css('display','inline-block');
        $('.lookUp').css('display','inline-block');
        $('.modify').css('display','inline-block')
    }else if(state.indexOf('待审核')!=-1){
        $('.audit').css('display','inline-block');
        $('.lookUp').css('display','inline-block')
    }else if(state.indexOf('驳回')!=-1){
        $('.turnDown').css('display','inline-block');
        $('.copy').css('display','inline-block');
        $('.lookUp').css('display','inline-block');
        $('.modify').css('display','inline-block')
    }else if(state.indexOf('已通过')!=-1){
        $('.addADPos').css('display','block');
        $('.auditPass').css('display','inline-block');
        $('.copy').css('display','inline-block');
        $('.lookUp').css('display','inline-block');
        $('.modify').css('display','inline-block');
        $('.putOn').css('display','inline-block')
    }
}

/**
 * Created by liushuai on 2017/3/4.
 * 查询列表
 * obj:查询的对象条件
 */
function searchList(obj) {
    setAjax({
        url:'/api/Periodication/SelectAppAdvListByPubID',
        type:'get',
        data:obj
    },function (data) {
        var temp = '#tmpl1';
        var stateCon = $('#state').text();
        if(stateCon.indexOf('已通过')!=-1) temp='#tmpl2';
        var arr = data.Result.listDetail;
        var counts = data.Result.TotalCount;
        if(arr.length!=0){
            var str = $(temp).html();
            var html = ejs.render(str, {list: arr});
            $('.table_list').html(html);
            $('#pageContainer').show();
            controlBtnShow();
            picShow();
            rejectAudit();
            searchPublishDetail();
            isCheckAll();
            modifyTrans();
        }else{
            var str = $('#tmpl3').html();
            var html = ejs.render(str, {list: arr});
            $('.table_list').html(html);
            $('#pageContainer').css('display','none')
        }
        // 推荐到首页
        $('.recommend').off('click').on('click', function () {
            var _this = $(this)
            setAjax({
                url: '/api/recommend/add',
                type: 'POST',
                data: {
                    MediaId: MediaID,
                    BusinessType: 14002,
                    ADDetailID:_this.attr('name')
                }
            }, function (data) {
                if (data.Status != 0) {
                    alert(data.Message)
                    return
                }
                _this.hide();
            })
        })

        $('#pageContainer').pagination(
            counts,
            {
                items_per_page:20,
                callback:function (currPage, jg) {
                    index = currPage;
                    obj.PageIndex=index;
                    setAjax({
                            url:'/api/Periodication/SelectAppAdvListByPubID',
                            type:'get',
                            data:obj
                        },
                        function (data) {
                            var temp = '#tmpl1';
                            var stateCon = $('#state').text();
                            if(stateCon.indexOf('已通过')!=-1) temp='#tmpl2';
                            var arr = data.Result.listDetail;
                            if(arr.length!=0){
                                var str = $(temp).html();
                                var html = ejs.render(str, {list: arr});
                                $('.table_list').html(html);
                                $('#pageContainer').show();
                                controlBtnShow();
                                picShow();
                                rejectAudit();
                                searchPublishDetail();
                                isCheckAll();
                                modifyTrans();
                            }else{
                                var str = $('#tmpl3').html();
                                var html = ejs.render(str, {list: arr});
                                $('.table_list').html(html);
                                $('#pageContainer').css('display','none')
                            }
                            // 推荐到首页
                            $('.recommend').off('click').on('click', function () {
                                var _this = $(this)
                                setAjax({
                                    url: '/api/recommend/add',
                                    type: 'POST',
                                    data: {
                                        MediaId: MediaID,
                                        BusinessType: 14002,
                                        ADDetailID:_this.attr('name')
                                    }
                                }, function (data) {
                                    if (data.Status != 0) {
                                        alert(data.Message)
                                        return
                                    }
                                    _this.hide();
                                })
                            })
                        }
                    );
                }
            }
        );
     /*   var NewPage = new PaginationController({
            WrapContainer: "#pageContainer",
            DisabledClassName: ".disabled",//不可点按钮的class名，可选
            CurrentClassName: ".current",//选中状态按钮class名，可选
            EnableClickClassName: ".EnableClick",//可点击按钮class名，可选
            NormalTextClassName: ".NormalText",//普通文本class名，可选，
            MaxPage: counts,
            PageItemCount: 20,
            ControllerCount: 5,
            CallBack: function (currentPageIndex, callback) {
                obj.PageIndex=currentPageIndex;
                setAjax({
                        url:'/api/Periodication/SelectAppAdvListByPubID',
                        type:'get',
                        data:obj
                    },
                    function (data) {
                        var temp = '#tmpl1';
                        var stateCon = $('#state').text();
                        if(stateCon.indexOf('已通过')!=-1) temp='#tmpl2';
                        var arr = data.Result.listDetail;
                        if(arr.length!=0){
                            var str = $(temp).html();
                            var html = ejs.render(str, {list: arr});
                            $('.table_list').html(html);
                            $('#pageContainer').show();
                            controlBtnShow();
                            picShow();
                            rejectAudit();
                            searchPublishDetail();
                            isCheckAll();
                            modifyTrans();
                        }else{
                            var str = $('#tmpl3').html();
                            var html = ejs.render(str, {list: arr});
                            $('.table_list').html(html);
                            $('#pageContainer').css('display','none')
                        }
                    }
                );
                callback(true);
            }
        });
        NewPage.createPageItemFu(1)*/
    })
}

function searchListSecond(obj) {
    setAjax({
        url:'/api/Periodication/SelectAppAdvListByPubID',
        type:'get',
        data:obj
    },function (data) {
        console.log(data.Result.listDetail.length);
        if(data.Result.listDetail.length > 0){
            var temp = '#tmpl1';
            var stateCon = $('#state').text();
            if(stateCon.indexOf('已通过')!=-1) temp='#tmpl2';
            var arr = data.Result.listDetail;
            var counts = data.Result.TotalCount;
            if(arr.length!=0){
                var str = $(temp).html();
                var html = ejs.render(str, {list: arr});
                $('.table_list').html(html);
                $('#pageContainer').show();
                controlBtnShow();
                picShow();
                rejectAudit();
                searchPublishDetail();
                isCheckAll();
                modifyTrans();
            }else{
                var str = $('#tmpl3').html();
                var html = ejs.render(str, {list: arr});
                $('.table_list').html(html);
                $('#pageContainer').css('display','none')
            }
            $('#pageContainer').pagination(
                    counts,
                {   current_page:index,
                    items_per_page:20,
                    callback:function (currPage, jg) {
                        index = currPage;
                        obj.PageIndex=index;
                        setAjax({
                                url:'/api/Periodication/SelectAppAdvListByPubID',
                                type:'get',
                                data:obj
                            },
                            function (data) {
                                var temp = '#tmpl1';
                                var stateCon = $('#state').text();
                                if(stateCon.indexOf('已通过')!=-1) temp='#tmpl2';
                                var arr = data.Result.listDetail;
                                if(arr.length!=0){
                                    var str = $(temp).html();
                                    var html = ejs.render(str, {list: arr});
                                    $('.table_list').html(html);
                                    $('#pageContainer').show();
                                    controlBtnShow();
                                    picShow();
                                    rejectAudit();
                                    searchPublishDetail();
                                    isCheckAll();
                                    modifyTrans();
                                }else{
                                    var str = $('#tmpl3').html();
                                    var html = ejs.render(str, {list: arr});
                                    $('.table_list').html(html);
                                    $('#pageContainer').css('display','none')
                                }
                            }
                        );
                    }
                }
            );
        }else{
            console.log("$$$$$");
            lookUpList();
        }
    })
}
/*上架下架状态切换*/
function putOn(event) {
    // var status=15002;//申请创建
    if($(event).text()=='上架'){
        status=15005;//申请上架
    }else if($(event).text()=='下架'){
        status=15006;//申请下架
    }
    var publishID = Number($('.journal ul li').eq(0).attr('PubID'));
    var adID = $(event).parent().attr('addID');
    var obj={
        RecID:adID,
        PubID:publishID,
        Status:status
    };
    setAjax({
        url:'/api/Publish/ModifyPublishStatus',
        type:'post',
        data:obj
    },function (data) {

        var flag = data.Result;
        if(flag){
            if($(event).text()=='上架'){
                $(event).text('下架');
                console.log(index);
                lookUpListSecond();
            }else if($(event).text()=='下架'){
                $(event).text('上架');
                console.log(index);
                lookUpListSecond();
            }
        }else{
            alert('上/下架操作失败')
        }

    });
}

/*驳回按钮*/
function rejectAudit() {
    $(".order_r").on("click","#reject",function () {
        $.openPopupLayer({
            name:'popLayerDemo111',
            url:'./reject.html',
            error:function (dd) {
                alert(dd.status)
            },
            success:function () {
                $('#allowBtn').click(function () {
                    var rejCon = $('#rejCon').val();
                    var mediaType= 14002;

                    var publishID = Number($('.journal ul li').eq(0).attr('PubID'));

                    setAjax({
                        url:'/api/ADOrderInfo/Review_Publish',
                        type:'get',
                        data: {mediaType:mediaType,publishID:publishID,optType: 27002,rejectMsg: rejCon}
                    },function (data) {

                        if(data.Message=='操作成功'){
                            $('#state').text('驳回');
                            window.location.href='/PublishManager/pricelist-app.html'
                        }else{
                            window.location.href='/PublishManager/pricelist-app.html'
                        }
                    });
                    $.closePopupLayer('popLayerDemo111');
                });
                $('#cancleBtn').click(function () {
                    $('#rejCon').text('');
                    $.closePopupLayer('popLayerDemo111')
                })
                $('#closebt').click(function () {
                    $('#rejCon').text('');
                    $.closePopupLayer('popLayerDemo111')
                })
            }
        })
    })
}
/*图例弹层*/
function picShow(){
    $(".order_r").on("click",".picDemo",function () {
        var url  = $(this).attr('picurl');
        $.openPopupLayer({
            name:'popLayerDemo',
            url:'./layer.html',
            error:function (dd) {
                alert(dd.status)
            },
            success:function () {
                $('.layer_con2 img').attr('src',url);
                $('.layer_con2').html('<img src="'+url+'" width="350" height="420">');

                $('#popupLayerScreenLocker').click(function () {
                    $.closePopupLayer('popLayerDemo')
                })
            }
        })
    });
}

/*审批通过*/
function auditPass(event) {
    var rejCon = $('#rejCon').val();
    var publishID = Number($('.journal ul li').eq(0).attr('PubID'));
    setAjax({
        url:'/api/ADOrderInfo/Review_Publish',
        type:'get',
        data: {mediaType:14002,publishID:publishID,optType: 27001,rejectMsg:''}
    },function (data) {
        if(data.Message=='操作成功'){
            $('#state').text('状态：审核通过');
            controlBtnShow();
            lookUpList();
            window.location.href='/PublishManager/pricelist-app.html'
        }else{
            window.location.href='/PublishManager/pricelist-app.html'
        }
    });
}

/*复制功能*/
function copyThis() {
    $(".order_r").on("click", ".copy", function () {
        var AdID = $(this).parent().attr('addID');
        var publishID = Number($('.journal ul li').eq(0).attr('PubID'));
        setAjax({
            url: "/api/Publish/CopyADPosition",
            type: 'post',
            data: {
                ADDetailID: AdID,
                PubID: publishID
            }
        }, function (data) {
            var pubID = $('.journal ul li').eq(0).attr('pubid');
            if (data.Result) {
                searchList({ PubID: pubID, PublishStatus: 0, pagesize: 20, PageIndex: 1 })
            } else {
                alert('复制失败')
            }
        })
    })
}


/*查看功能*/
function searchPublishDetail() {
    $(".order_r").on("click",".lookUp",function () {
        var pubid = $('.journal ul li').eq(0).attr('pubid');
        var adID = $(this).parent().attr('addid');
        $(this).attr('href','/MediaManager/ViewOfApp.html?MediaType='+14002+'&PubID='+pubid+'&ADDetailID='+adID+'&MediaID='+MediaID)
    })
}

/*上架所有*/
function putOnAllOfAD(event) {
    var arrList = $('.table_list table').find('tr');
    if(arrList.length == 1){
        alert("广告位数量为0时，不能上架!");
        return;
    }
    var PubID = Number($('.journal ul li').eq(0).attr('PubID'));
    var obj={
        PubID:PubID,
        Status:15002
    };
    setAjax({
        url:'/api/Publish/ModifyPublishStatus',
        type:'post',
        data:obj
    },function (data) {
        if(data.Result){
            alert("上架已经提交,请耐心等待!");
            lookUpList();
            window.location.href='/PublishManager/pricelist-app.html'
        }else{
            window.location.href='/PublishManager/pricelist-app.html';
        }
    });
}


/*批量上下架*/
function putOnAll(event) {
    var stateCon = 15002;
    if($(event).text()=='上架') stateCon=15005;
    if($(event).text()=='下架') stateCon=15006;
    var arr = [];
    var PubID = Number($('.journal ul li').eq(0).attr('pubid'));
    var arrList = $('.table_list').find('input');
    $(arrList).each(function () {
        if($(this).prop('checked')==true){
            var id = $(this).parent().attr('addID');
            arr.push(id);
        }
    });
    if(arr.length==0){
        alert('请选择广告位!');
        return
    }
    arr = arr.join(',');

    var obj = {
        RecID:arr,
        PubID:PubID,
        Status:stateCon
    };

    setAjax({
        url:'/api/Publish/ModifyPublishStatus',
        type:'post',
        data:obj
    },function (data) {
        if(data.Result){
            lookUpList();
        }else{
            alert('操作失败,'+data.Message)
            $('.table_list tr input:checkbox').prop('checked',false)
        }
    });
    $('#checkAll').prop('checked',false)
}


/*判断是否全选*/
function isCheckAll() {
    $('.table_list input:checkbox').click(function () {
        var totalCount = $.makeArray($('.table_list input:checkbox')).length;
        var total = 0;
        $('.table_list input:checkbox').each(function () {
            if($(this).prop('checked')==true){
                total++;
            }
        });
        if(total==totalCount){
            $('#checkAll').prop('checked',true)
        }else{
            $('#checkAll').prop('checked',false)
        }
    })
}

/*跳转刊例APP广告位编辑页面*/
function modifyTrans() {
    $(".order_r").on("click",".modify",function () {
        var pubID = $('.journal ul li').eq(0).attr('pubID');
        var adDetailID = $(this).parent().attr('addid');
        $(this).attr('href','addEditAdSpace.html?PubID='+pubID+'&ADDetailID='+adDetailID);
    })
}

/*跳转刊例APP编辑添加页面*/
function modifyTransAdd() {
    $(".order_r").on("click",".addModify",function () {
        var pubID = $('.journal ul li').eq(0).attr('pubID');
        $(this).attr('href','addEditAdSpace.html?PubID='+pubID);
    })
}

/*跳转刊例编辑添加页面*/
function modifyPublish() {
    $(".order_r").on("click",".addPubMod",function () {
        var pubID = $('.journal ul li').eq(0).attr('pubID');
        $(this).attr('href','addEditPublish-app.html?isAdd=1&PubID='+pubID);
    })
}

/*查询按钮*/
function lookUpList() {
    var pubID = $('.journal ul li').eq(0).attr('PubID');
    var adPosition = $('#adPos').val();
    var adForm = $('#adForm').val();
    var adStyle = $('#adStyle').val();
    var stateID = $('#selectState option:selected').val();
    var obj={
        PubID:pubID,
        AdPosition:adPosition,
        AdForm:adForm,
        Style:adStyle,
        PublishStatus:stateID,
        pagesize:20,
        PageIndex:1
    };
    searchList(obj);
    index = 1;
}
// 查询按钮2
function lookUpListSecond() {
    var pubID = $('.journal ul li').eq(0).attr('PubID');
    var adPosition = $('#adPos').val();
    var adForm = $('#adForm').val();
    var adStyle = $('#adStyle').val();
    var stateID = $('#selectState option:selected').val();
    var obj={
        PubID:pubID,
        AdPosition:adPosition,
        AdForm:adForm,
        Style:adStyle,
        PublishStatus:stateID,
        pagesize:20,
        PageIndex:index
    };
    searchListSecond(obj);
}
