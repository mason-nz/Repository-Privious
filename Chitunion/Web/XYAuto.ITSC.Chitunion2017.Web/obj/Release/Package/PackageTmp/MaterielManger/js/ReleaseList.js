/*
* 查询列表
*
* */
$(function () {

    var SelectData = {

        init : function () {

            SelectData.selectMotorcycle();//车型查询接口

            // 时间控件
            $('#SatrtDate').off('click').on('click', function () {
                laydate({
                    fixed: false,
                    elem: '#SatrtDate',
                    choose: function (date) {
                        if (date > $('#EndDate').val() && $('#EndDate').val()) {
                            layer.msg('起始时间不能大于结束时间！',{'time':1000});
                            $('#SatrtDate').val('');
                        }
                    }
                });
            });
            $('#EndDate').off('click').on('click', function () {
                laydate({
                    fixed: false,
                    elem: '#EndDate',
                    choose: function (date) {
                        if (date < $('#SatrtDate').val() && $('#SatrtDate').val()) {
                            layer.msg('结束时间不能小于起始时间！',{'time':1000});
                            $('#EndDate').val('');
                        }
                    }
                });
            });

            //查询按钮
            $('#select_data').on('click',function () {
                
                var MaterielName = $.trim($('#material_name').val());//物料名称
                var ContractNumber = $.trim($('#associate_number').val());//关联合同号
                var CarSerialId = $('.state .Models option:checked').attr('CarSerialId')*1;//品牌或车型
                var SatrtDate = $('#SatrtDate').val();
                var EndDate = $('#EndDate').val();

                var obj = {
                    MaterielName : MaterielName,
                    ContractNumber : ContractNumber,
                    CarSerialId : CarSerialId,
                    PageIndex : 1,
                    PageSize : 20,
                    SatrtDate :SatrtDate,
                    EndDate : EndDate
                }
                SelectData.sendRequest(obj,obj.PageIndex);
            })
        },
        selectMotorcycle : function(){//查询车型

            $.ajax({
                url:'http://www.chitunion.com/api/CarSerial/QueryBrand',
                type:'get',
                async: false,
                dataType: 'json',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                data: {},
                success:function(data){
                    if(data.Status == 0){
                        var str = '<option MasterId="-2">请选择品牌</option>';
                        for(var i=0;i<data.Result.length;i++){
                            str += '<option MasterId='+data.Result[i].MasterId+'>'+data.Result[i].Name+'</option>'
                        }
                        $('.brand').html(str);

                        $('.brand').off('change').on('change',function(){
                            $('.brands').find('.hidden_tip').hide();
                            var MasterBrandId = $(this).find('option:checked').attr('MasterId');
                            if(MasterBrandId == -2){
                                $('.series').html('<option BrandId="-2">请选择子品牌</option>');
                                $('.Models').html('<option CarSerialId="-2">请选择车型</option>');
                            }else{
                                $.ajax({
                                    url:'http://www.chitunion.com/api/CarSerial/QueryBrand',
                                    type:'get',
                                    async: false,
                                    dataType: 'json',
                                    xhrFields: {
                                        withCredentials: true
                                    },
                                    crossDomain: true,
                                    data: {
                                        MasterBrandId:MasterBrandId
                                    },
                                    success:function(data){
                                        if(data.Status == 0){
                                            var str1 = '<option BrandId="-2">请选择子品牌</option>';
                                            for(var j=0;j<data.Result.length;j++){
                                                str1 += '<option BrandId='+data.Result[j].BrandId+'>'+data.Result[j].Name+'</option>'
                                            }
                                            $('.series').html(str1);
                                            $('.Models').html('<option MasterId="-2">请选择车型</option>');
                                            $('.series').off('change').on('change',function(){
                                                $('.brands').find('.hidden_tip').hide();
                                                var BrandId = $(this).find('option:checked').attr('BrandId');
                                                if(BrandId == -2){
                                                    $('.Models').html('<option MasterId="-2">请选择车型</option>');
                                                }else{
                                                    $.ajax({
                                                        url:'http://www.chitunion.com/api/CarSerial/QuerySerialList',
                                                        type:'get',
                                                        async: false,
                                                        dataType: 'json',
                                                        xhrFields: {
                                                            withCredentials: true
                                                        },
                                                        crossDomain: true,
                                                        data: {
                                                            BrandId:BrandId
                                                        },
                                                        success:function(data){
                                                            var str2 = '<option CarSerialId="-2">请选择车型</option>';
                                                            for(var k=0;k<data.Result.length;k++){
                                                                str2 += '<option BrandId='+data.Result[k].BrandId+' CarSerialId='+data.Result[k].CarSerialId+'>'+data.Result[k].ShowName+'</option>'
                                                            }
                                                            $('.Models').html(str2);
                                                            $('.Models').off('change').on('change',function(){
                                                                $('.brands').find('.hidden_tip').hide();
                                                            })
                                                        }
                                                    })
                                                }
                                            })
                                        }
                                    }
                                });
                            }
                        })
                    }
                }
            });

        },
        sendRequest : function (obj,PageIndex) {
            var url = 'http://www.chitunion.com/api/Materiel/GetList';
            setAjax({
                url : url,
                type : 'get',
                data : obj
            },function (data) {
                var Result = data.Result;
                var TotalCount = data.Result.TotleCount;
                if(data.Status == 0){
                    if(TotalCount > 0){
                        $('#list_data').show();
                        $('#list_data').html(ejs.render($("#ListTemplate").html(),{Result:Result.List}));
                        $('#No_Data').hide();

                        //分页
                        $("#pageContainer").pagination(TotalCount, {
                            items_per_page: 20, //每页显示多少条记录（默认为10条）
                            callback: function (currPage) {
                                obj.PageIndex = currPage;
                                setAjax({
                                    url  :  url,
                                    type : 'get',
                                    data :  obj
                                },function (data) {
                                    var Result = data.Result;
                                    $('#list_data').html(ejs.render($("#ListTemplate").html(),{Result:Result.List}));
                                })
                            }
                        })
                        //操作
                        SelectData.operation();
                        //关联合同号
                        SelectData.CrmNumADD();
                    }else{
                        $('#No_Data').show();
                        $('#list_data').hide();
                        $('#pageContainer').hide();
                    }
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
        },
        operation : function () {//操作

            //全选
            $('#selectAll').on('change',function () {
                var that = $(this);
                if(that.prop('checked') == true){
                    $('.table input[name=PitchUp]').prop('checked',true);
                }else{
                    $('.table input[name=PitchUp]').prop('checked',false);
                }
            })

            //单选
            $('.onecheck').off('click').on('click',function () {
                if($('.table input[name=PitchUp]').length == $('.onecheck:checked').length){
                    $('#selectAll').prop('checked',true);
                }else{
                    $('#selectAll').prop('checked',false);
                }
            })

            //下载数据
            $('#PackageDownload').off('click').on('click',function () {
                var MaterielIDs = [];
                $('.onebox').each(function () {
                    if($(this).prop('checked')==true){
                        MaterielIDs.push($(this).parents('tr').attr('MaterielID'));
                    }
                })
                if(MaterielIDs.length){
                    setAjax({
                        url : 'http://www.chitunion.com/api/MaterielChannelData/BatchExportToExcel',
                        type : 'post',
                        data : {
                            MaterielIDs : MaterielIDs
                        }
                    },function(data){
                        if(data.Status == 0){
                            window.open(data.Result);
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })
                }else {
                    layer.msg("请选择物料",{'time':1000});
                }
            })


           //发布渠道
           $('.release_channel').off('click').on('click',function(){
                var MaterielID = $(this).parents('tr').attr('MaterielID');
                var ChannelCount = $(this).parents('tr').attr('ChannelCount')*1;
                console.log(ChannelCount);
                if(ChannelCount == 0){//新增
                    //window.open('/MaterielManger/release_channel.html?MaterielID='+MaterielID);
                    window.location = '/MaterielManger/release_channel.html?MaterielID='+MaterielID;
                }else{//编辑
                    window.location = '/MaterielManger/edit_chanel.html?MaterielID='+MaterielID;
                }
           })

           //查看详情
           $('.datail_message').off('click').on('click',function(){
                var MaterielID = $(this).parents('tr').attr('MaterielID');
                window.open('/MaterielManger/release_detail.html?MaterielID='+MaterielID);
           })

           //查看数据
           $('.see_detailData').off('click').on('click',function(){
                var MaterielID = $(this).parents('tr').attr('MaterielID');
                window.open('/MaterielManger/Ad_Data.html?MaterielID='+MaterielID);
           })

        },
        CrmNumADD : function(){//关联合同号       

            var flag = false;

            $('#list_data tr td').on('mouseenter',function(){
                var that = $(this);
                that.find('.CrmNum').removeAttr('disabled');
            }).on('mouseleave',function(){
                var that = $(this);
                that.find('.CrmNum').css({'border':'none'});
            }).on('click',function(){
                var that = $(this);
                var cur = that.find('.CrmNum');
                var MaterielID = that.attr('MaterielID');
                cur.css({'border':'1px solid #ccc'});
            })

            $('#list_data tr td .CrmNum').on('blur',function(){
                var that = $(this);
                var txt = $.trim(that.val());
                var MaterielID = that.parent().attr('MaterielID');
                var len = txt.length;
                var reg = /^([a-z]|[A-Z]|[0-9]|\s)+$/;

                if(txt != ''){
                    if(len > 15){
                        layer.msg('不能超过15个字符哦',{'time':1000});
                        that.css({'border':'1px solid #ccc'});
                    }else if(!reg.test(txt)){
                        layer.msg('请输入字母或者数字',{'time':1000});
                        that.css({'border':'1px solid #ccc'});
                    }else{
                        that.css({'border':'none'});
                        that.attr('disabled',true);
                        flag = true;
                        senToNxt(MaterielID,txt,that);
                    }
                }else{
                    that.css({'border':'none'});
                    that.attr('disabled',true);
                    flag = true;
                    senToNxt(MaterielID,txt,that);
                }
            })

            //新增CRM合同编号 接口
            function senToNxt(MaterielID,CrmNum,ele){
                if(flag == true){
                    setAjax({
                        url : 'http://www.chitunion.com/api/Materiel/UpdateContractNumber',
                        type : 'post',
                        data : {
                            'MaterielID' : MaterielID,
                            'ContractNumber' : CrmNum
                        }
                    },function(data){
                        if(data.Status == 0){
                            $(ele).css({'border':'none'});
                            $(ele).attr('disabled','disabled');
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })
                }
            }

        }
    }

    SelectData.init();//初始化渲染
    $('#select_data').click();
})