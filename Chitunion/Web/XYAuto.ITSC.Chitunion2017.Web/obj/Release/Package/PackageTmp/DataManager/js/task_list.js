$(function () {



    var obj = {
        PageSize:20,
        PageIndex:1,
        StartDate:laydate.now(),
        EndDate: laydate.now()
    };
    $("#startTime").val(laydate.now());
    $("#endTime").val(laydate.now());
    if(CTLogin.RoleIDs=="SYS001RL00012" || CTLogin.RoleIDs=="SYS001RL00001"){
        obj.TaskStatus = -2;
    }
    if(CTLogin.RoleIDs=="SYS001RL00013"){
        obj.TaskStatus = 95002;

    }
    function Task() {

        this.GetParam();
        this.operat();

    }

    Task.prototype = {
        constructor: Task,
        //权限操作
        limits: function () {
            var _this = this;
            //除了未分配的都没有全选单选操作
            if(obj.TaskStatus == -2 || obj.TaskStatus == 95002 ){
                $("#all").show();
                $(".single").show();
            }else{

                $("#all").hide();
                $(".single").hide();
            }

            //翻页条数变化时
            $(".pagesize").on("change",function(){
                obj.PageSize = parseInt($(".pagesize option:selected").val());
                _this.search();
            })

            if(CTLogin.RoleIDs=="SYS001RL00012" || CTLogin.RoleIDs=="SYS001RL00001"){
                $(".fr").show();
                //查看清洗的详情页面
                $(".pp").css("cursor","pointer");
                $(".pp").off("click").on("click",function(){
                     var GroupId = $(this).attr("GroupId");
                     window.open("/DataManager/material_clean.html?GroupId="+GroupId+"&isSee=1");
                })
               //  obj.TaskStatus = 95001;
                if(obj.TaskStatus != 95002){
                    $(".take_back").hide();
                }else{
                    $(".take_back").show();
                }
                if(obj.TaskStatus != -2){
                    $(".distribution").hide();
                }else{
                    $(".distribution").show();
                }
            }else{
                $(".fr").hide();
            }
            if(CTLogin.RoleIDs=="SYS001RL00013"){
                $("#status option").eq(0).hide();
                $("#all").hide();
                $(".single").hide();
                $('.operation_people').hide();//清洗员  隐藏操作人选项
                if(obj.TaskStatus == 95002 || obj.TaskStatus == 95003){
                    $("#opration").show();
                    $(".qingxi").show();
                }else{
                    $("#opration").hide();
                    $(".qingxi").hide();
                }
                //点击清洗进入清洗页面
                $(".qingxi").off("click").on("click",function(){
                    var GroupId = $(this).attr("GroupId");
                    window.open("/DataManager/material_clean.html?GroupId="+GroupId);
                })
                //点击标题进入查看页面(作废不可以跳)
                if(obj.TaskStatus != 95005){
                    $(".pp").css("cursor","pointer");
                    $(".pp").off("click").on("click",function(){
                         var GroupId = $(this).attr("GroupId");
                         window.open("/DataManager/material_clean.html?GroupId="+GroupId+"&isSee=1");
                    })
                }

            }else{
                $("#opration").hide();
                $(".qingxi").hide();
            }
        },

        //点击搜索
        search: function () {
            var _this = this;
                 obj.PageIndex = 1;
                 $.ajax({
                     url:"http://www.chitunion.com/api/Materiel/GetTaskList",
                     type:"get",
                     xhrFields:{
                         withCredentials:true
                     },
                     crossDomain:true,
                     data:obj,
                     success: function(data){
                         console.log(data);
                         _this.limits();
                         if(data.Result.TotleCount != 0 ){
                             $(".fen").show();
                             $(".fl span").html(data.Result.TotleCount);
                             $(".ad_table").html(ejs.render($("#info_tmp").html(),{data:data.Result.List}));
                             _this.operat();

                             _this.limits();
                             //翻页操作
                             //分页部分
                             $("#pageContainer").pagination(
                                 data.Result.TotleCount,
                                 {
                                     // items_per_page: 2, //每页显示多少条记录（默认为20条）
                                     callback: function (currPage, jg) {
                                         obj.PageIndex = currPage;
                                         $.ajax({
                                             url:"http://www.chitunion.com/api/Materiel/GetTaskList",
                                             type:"get",
                                             xhrFields:{
                                                 withCredentials:true
                                             },
                                             crossDomain:true,
                                             data:obj,
                                             success: function(data){
                                                 console.log(data);
                                                 if(data.Status == 0){
                                                     $(".ad_table").html(ejs.render($("#info_tmp").html(),{data:data.Result.List}));
                                                    _this.limits();
                                                    _this.operat();
                                                 }
                                             }
                                         })
                                     }
                                 });
                         }else{
                             $(".fr").hide();
                             $(".fl span").html(data.Result.TotleCount);
                             $(".fen").hide();
                              $(".ad_table").html('<img src="../images/no_data.png" style="display:block;margin:20px auto;">');
                         }



                     }
                 })

        },
        distribution: function() { //分配操作
            var _this = this;
            //点击分配

                $.ajax({
                    url:"http://www.chitunion.com/api/Materiel/GetDivideUser",
                    type:"get",
                    xhrFields:{
                        withCredentials:true
                    },
                    crossDomain:true,
                    success: function(data) {
                        console.log(data);
                        if(data.Status == 0){
                            $(".add_ad").html(ejs.render($("#layer_tmp").html(),{data:data.Result.List}));
                        }
                    }

                })
                //点击确认分配
                $("#add_z").off("click").on("click",function(){
                    var UserId = parseInt($(".add_ad input:checked").val());
                    var GroupId = "";
                    var select_input = $(".select_single:checked");
                    for(var i=0;i<select_input.length;i++){
                        GroupId = GroupId + select_input[i].value + ",";
                    }
                    GroupId = GroupId.substring(0,GroupId.length-1);
                    console.log(GroupId);
                    console.log(UserId);
                    if(GroupId){
                      $.ajax({
                          url:"http://www.chitunion.com/api/Materiel/TaskDivide",
                          type:"post",
                          xhrFields:{
                              withCredentials:true
                          },
                          crossDomain:true,
                          data:{
                              OperateType:1,
                              GroupIds:GroupId,
                              UserId: UserId
                          },
                          success: function(data){
                              console.log(data);
                              if(data.Status == 0){
                                  $(".layer").hide();
                                  layer.msg("分配成功",{time:1000});
                                  _this.search();
                              }else{
                                  if(data.msg == "当前物料已分配，请重新选择"){
                                      layer.msg("当前物料已分配，请重新选择",{time:1000});
                                      $(".layer").hide();
                                  }else{
                                      layer.msg("分配失败请重试",{time:1000});
                                  }
                              }

                          }
                      })
                  }else{
                      layer.msg("请选择分配内容",{time:2000});
                  }

                })

        },
        takeBack: function(){ //收回
            var _this = this;
            // var UserId = $(".add_ad input:checked").val();
            // console.log(UserId);
            var GroupId = "";
            var select_input = $(".select_single:checked");
            for(var i=0;i<select_input.length;i++){
                GroupId = GroupId + select_input[i].value + ",";
            }
            GroupId = GroupId.substring(0,GroupId.length-1);
            $.ajax({
                url:"http://www.chitunion.com/api/Materiel/TaskDivide",
                type:"post",
                xhrFields:{
                    withCredentials:true
                },
                crossDomain:true,
                data:{
                    OperateType:2,
                    GroupIds:GroupId,
                    // UserId: UserId
                },
                success: function(data){
                    if(data.Status == 0){
                        layer.msg("收回成功",{time:1000});
                        _this.search();
                    }else{
                        layer.msg(data.Message,{time:2000});
                    }
                }
            })
        },
        //操作
        operat: function () {
                var _this = this;
                //全选操作

                //_this.search();

                $("#select_all").on("change",function() {
                  if($(this).prop("checked")){
                      $(".select_single").prop("checked",true);
                  }else{
                      $(".select_single").prop("checked",false);
                  }
                })
                //单个按钮取消选中
                $(".select_single").on("change",function(){
                    var len1 = $(".select_single").length;
                    var len2 = $(".select_single:checked").length;
                    if(len1 == len2){
                        $("#select_all").prop("checked",true);
                    }else{
                        $("#select_all").prop("checked",false);
                    }
                })

                //点击分配出现弹层
                $(".distribution").off("click").on("click",function () {
                    $(".layer").show();
                    _this.distribution();
                })
                //关闭按钮
                $(".closePopup").on("click",function() {
                    $(".layer").hide();
                })
                //确认分配
                // $("#add_z").off("click").on("click",function () {
                //     $(".layer").hide();
                // })
                //暂不分配
                $("#add_w").off("click").on("click",function () {
                    $(".layer").hide();
                })

                //点击收回
                $(".take_back").off("click").on("click",function(){
                    _this.takeBack();
                })

                //搜索操作
                $("#seach").off("click").on("click",function(){
                    _this.search();
                })

                //翻页条数变化时
                $(".pagesize").on("change",function(){
                    obj.PageSize = parseInt($(".pagesize option:selected").val());
                    _this.search();
                })

        },
        GetParam : function(){
            var _this = this;
            //渠道
            $("#channel").on("change",function() {
                console.log($(this).children('option:selected').val());
                obj.ChannelId = $(this).children('option:selected').val();
            })
            //分类
            // $.ajax({
            //     url:"http://www.chitunion.com/api/DictInfo/GetDictInfoByTypeID?typeID=95",
            //     type:"get",
            //     xhrFields:{
            //         withCredentials:true
            //     },
            //     crossDomain:true,
            //     success: function(data){
            //         console.log(data);
            //         if(data.Status == 0){
                        var  data = [
                                    "辟谣"
                                    ,"财经"
                                    ,"财经视频"
                                    ,"彩票"
                                    ,"宠物"
                                    ,"传媒"
                                    ,"电影"
                                    ,"动漫"
                                    ,"法制"
                                    ,"房产"
                                    ,"风水"
                                    ,"佛学"
                                    ,"改装/赛事"
                                    ,"搞笑"
                                    ,"公益"
                                    ,"故事"
                                    ,"国际"
                                    ,"婚嫁"
                                    ,"技术"
                                    ,"技术设计"
                                    ,"家居"
                                    ,"健康"
                                    ,"健康视频"
                                    ,"教育"
                                    ,"警法"
                                    ,"军事"
                                    ,"军事视频"
                                    ,"科技"
                                    ,"科普"
                                    ,"科学"
                                    ,"理财"
                                    ,"历史"
                                    ,"亮相下线"
                                    ,"旅游"
                                    ,"美食"
                                    ,"美文"
                                    ,"母婴"
                                    ,"其它"
                                    ,"汽车"
                                    ,"汽车文化"
                                    ,"情感"
                                    ,"三农"
                                    ,"上市报道"
                                    ,"上市消息"
                                    ,"设计"
                                    ,"社会"
                                    ,"摄影"
                                    ,"时尚"
                                    ,"时政"
                                    ,"市场推广"
                                    ,"视频"
                                    ,"试驾评测"
                                    ,"收藏"
                                    ,"数码"
                                    ,"数码视频"
                                    ,"体育"
                                    ,"文化"
                                    ,"小说"
                                    ,"心理"
                                    ,"新车谍照"
                                    ,"新车信息"
                                    ,"新闻中心"
                                    ,"星座"
                                    ,"选车导购"
                                    ,"移民"
                                    ,"艺术"
                                    ,"音乐"
                                    ,"用车养车"
                                    ,"游戏"
                                    ,"娱乐"
                                    ,"育儿"
                                    ,"政务"
                                    ,"职场"
                                ];
                        var str = '<option value="">不限</option>';
                        for(var i=0;i<data.length;i++){
                            str += '<option value="'+data[i]+'">'+data[i]+'</option>';
                        }
                        $("#CategoryId").html(str);

                        $("#CategoryId").on("change",function() {
                            console.log($(this).children('option:selected').val());
                            obj.Category = $(this).children('option:selected').val();
                        })
            //         }
            //     }
            // })


            //查询车型
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
                                obj.CarSerialId = -2;
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
                                                    obj.CarSerialId = -2;
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

                                                            $(".Models").on("change",function() {
                                                                console.log($(this).children('option:selected').attr("CarSerialId"));
                                                                obj.CarSerialId = $(this).children('option:selected').attr("CarSerialId");
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

            //操作人
            $.ajax({
                url:"http://www.chitunion.com/api/Materiel/GetDivideUser",
                type:"get",
                xhrFields:{
                    withCredentials:true
                },
                crossDomain:true,
                success: function (data) {
                    console.log(data);
                    var str = '<option value="-2">请选择</option>';
                    if(data.Result.List.length != 0){
                        for(var i=0;i<data.Result.List.length;i++){
                            str += '<option value="'+data.Result.List[i].UserId+'">'+data.Result.List[i].UserName+'</option>'
                        }
                        $("#User").html(str);
                        $("#User").on("change",function() {
                                obj.UserId = $(this).children('option:selected').val();
                        })
                    }

                }
            });

            //发布时间

            //合作周期部分
            var start = {
              elem: "#startTime",
              fixed: false,
              // min: add_date(laydate.now()),
              // min: laydate.now(),
              // istime: true,
              // issure: true,
              istoday:false,
              // isNeedConfirm: true,
              format: 'YYYY-MM-DD',
              choose: function (date) {
                //   _this.GetParam();
                if($("#startTime").val()){
                    obj.StartDate = $("#startTime").val();
                }else{
                    obj.StartDate = "";
                }
              }
            }
            var end = {
              elem: "#endTime",
              fixed: false,
              // min: add_date(laydate.now()),
              // min: laydate.now(),
              // istime: true,
              // issure: true,
              istoday:false,
              // isNeedConfirm: true,
              format: 'YYYY-MM-DD',
              choose: function (date) {
                //    _this.GetParam();
                if($("#endTime").val()){
                    obj.EndDate = $("#endTime").val();
                }else{
                    obj.EndDate = "";
                }
              }
            }
            //给开始时间跟结束时间绑定点击的事件
            $("#startTime").off("click").on("click", function () {
              laydate(start);


            })
            $("#endTime").off("click").on("click", function () {
              laydate(end);

            })
            //状态

            $.ajax({
                url:"http://www.chitunion.com/api/DictInfo/GetDictInfoByTypeID?typeID=95",
                type:"get",
                xhrFields:{
                    withCredentials:true
                },
                crossDomain:true,
                success: function(data){
                    if(data.Status == 0){
                        var str = '<option value="-2">待分配</option>';
                        // var str = '';
                        for(var i=0;i<data.Result.length;i++){
                            str += '<option value="'+data.Result[i].DictId+'">'+data.Result[i].DictName+'</option>';
                        }
                        $("#status").html(str);

                        $("#status").on("change",function(){
                            obj.TaskStatus = $(this).children('option:selected').val();

                        })
                        //清洗员默认选中已分配
                        if(CTLogin.RoleIDs=="SYS001RL00013"){
                            var op = $("#status").children('option');
                            for(var i=0;i<op.length;i++){
                                if(op[i].value == 95002){
                                    op[i].selected = "selected";
                                }
                            }
                        }
                        //分配员默认选中待分配
                        if(CTLogin.RoleIDs=="SYS001RL00012"){
                            var op = $("#status").children('option');
                            for(var i=0;i<op.length;i++){
                                if(op[i].value == -2){
                                    op[i].selected = "selected";
                                }
                            }
                        }


                    }
                }
            })



          //触发第一次
          _this.search();
        },

    }

    var task = new Task();


})
