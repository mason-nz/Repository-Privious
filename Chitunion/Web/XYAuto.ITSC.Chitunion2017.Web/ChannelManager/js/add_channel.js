$( function () {
  var PolicyList = [],
      Quota,
      QuotaIncludingEqual,
      SingleAccountSum,
      SingleAccountSumType,
      PurchaseDiscount,
      RebateType1,
      RebateType2,
      RebateValue,
      RebateDateType,
      ChannelName,
      IncludingTax,
      CooperateBeginDate = '',
      CooperateEndDate = '',
      Remark;
  var flag = true,
      flag2 = true;
  //渠道名称部分

  //政策弹窗部分
  //点击添加显示弹窗
  $(".but_add").off("click").on("click", function () {
    $("#zhezhao").show();
    $(".layer").show();
    $("#add_z").show();
    $("#add_w").hide();
    //置空操作
    empty_info ();
    //验证输入信息
    validation ();

  })
  //弹窗中的部分交互
  //页面首次加载默认为无
  if($("#type_one input[type=radio]:checked").val() == 56003){
    $("#type_two").hide();
    $("#type_three").hide();
    $("#type_five").hide();
    $("#type_six").hide();
  }
  //当单选框选中的变化时
  $("input[type=radio]").on("change", function () {
    change_one();
    change_two();
    hide_info ();
  })
  //点击关闭隐藏弹窗
  $(".close").on("click", function () {
    $("#zhezhao").hide();
    $(".layer").hide();
  })
  //返点类型部分的维度切换

  //点击弹窗的添加生成表格数据
  $("#add_z").off("click").on("click", function () {
    //验证取值
    verify_value();
    //验证通过之后进行
    if(flag && flag2){
      //去除重复的选项
      for(var i=0;i<PolicyList.length;i++){
          if(Quota == PolicyList[i].Quota && QuotaIncludingEqual == PolicyList[i].QuotaIncludingEqual && ((SingleAccountSum > PolicyList[i].SingleAccountSum && SingleAccountSumType == 55002 && PolicyList[i].SingleAccountSumType == 55001) || (SingleAccountSum < PolicyList[i].SingleAccountSum && SingleAccountSumType == 55001 && PolicyList[i].SingleAccountSumType == 55002))){
            layer.msg("满额与单个账号金额的组合不能重复，相同的满额和单个账号金额只能对应一个政策", {time:2000});
            return false;
          }
          if(Quota == PolicyList[i].Quota && QuotaIncludingEqual == PolicyList[i].QuotaIncludingEqual && SingleAccountSum == PolicyList[i].SingleAccountSum && PolicyList[i].SingleAccountSumType == SingleAccountSumType){
          layer.msg("满额与单个账号金额的组合不能重复，相同的满额和单个账号金额只能对应一个政策", {time:2000});
          return false;
          }
      }
      //获取页面上的数据传入集合中
      PolicyList.push({
        "PolicyID": 0,
        "Quota": Quota,
        "QuotaIncludingEqual": QuotaIncludingEqual,
        "SingleAccountSum": SingleAccountSum,
        "SingleAccountSumType": SingleAccountSumType,
        "PurchaseDiscount": PurchaseDiscount,
        "RebateType1": RebateType1,
        "RebateType2": RebateType2,
        "RebateValue": RebateValue,
        "RebateDateType": RebateDateType
      })
      console.log(PolicyList);
      //对PolicyList数组进行正序排列
      PolicyList.sort(compare("Quota","SingleAccountSum"));
      console.log(PolicyList);
      //渲染表格信息出现表格
      $(".table").html(ejs.render($("#table").html(),{PolicyList:PolicyList}));
      //对表格的操作（编辑跟删除）
      table_opration ();
      $(".layer").hide();
      $("#policy").show();
      //提示信息隐藏
      $("#add_channel_info").hide();
      //隐藏遮罩层
      $("#zhezhao").hide();
    }

  })
  //定义一个排序函数(正序,包含两个优先级条件)
  function compare (prop1,prop2) {
    return function (obj1, obj2) {
       var val1 = obj1[prop1];
       var val2 = obj2[prop1];
       if(val1 == val2){
         val1 = obj1[prop2];
         val2 = obj2[prop2];
       }
       if (!isNaN(Number(val1)) && !isNaN(Number(val2))) {
           val1 = Number(val1);
           val2 = Number(val2);
       }
       if (val1 < val2) {
           return -1;
       } else if (val1 > val2) {
           return 1;
       } else {
           return 0;
       }
   }
  }

  //验证弹窗部分的必填项跟取值
  function verify_value() {
    flag = true;
    //首先验证必填项不能空
    //满额部分验证取值
    if($("#top_up").val() == "" ){
      $("#mane").show();
      flag = false;
      return false;
    }else{
      $("#mane").hide();
      //取页面上的值并查看是否包含等于
      Quota = parseInt($("#top_up").val());
      if($("#equal").prop("checked")){
        QuotaIncludingEqual = true
      }else{
        QuotaIncludingEqual = false
      }
    }
    //单个账号金额（不是必填项）
    if($("#single").val() == ""){
      SingleAccountSumType = -2;
      SingleAccountSum = 0;
    }else{
      SingleAccountSum = parseInt($("#single").val());
      SingleAccountSumType = parseInt($("#select select").val());

    }
    console.log(SingleAccountSumType);
    //采购折扣
    if($("#procurement").val() == "" ){
      $("#discount").show();
      flag = false;
      return false;
    }else if($("#procurement").val() > 100){
      $("#discount").hide();
      $("#discount_two").show();
      flag = false;
      return false;
    }else{
      $("#discount").hide();
      PurchaseDiscount = parseFloat($("#procurement").val());
    }
    //选中了返点类型的必填验证
    //返现部分验证取值
    if($("#type_one input[type=radio]:checked").val() == 56001){
      RebateType1 = 56001;
      if($("#type_two input[type=radio]:checked").val() == 57001 && $("#type_three input").val() == ""){
        $("#proportion").show();
        flag = false;
        return false;
      }else if ($("#type_two input[type=radio]:checked").val() == 57001 && $("#type_three input").val() > 100){
        $("#proportion").hide();
        $('#proportion_two').show();
        flag = false;
        return false;
      }else if($("#type_two input[type=radio]:checked").val() == 57001 && $("#type_three input").val() != ""){
        $("#proportion").hide();
        RebateType2 = 57001;
        RebateValue = parseFloat(($("#proportion_val").val())/100);
        console.log($("#proportion_val").val());
        console.log(RebateValue);
      }
      if($("#type_two input[type=radio]:checked").val() == 57002 && $("#type_four input").val() == ""){
        $("#amount").show();
        flag = false;
        return false;
      }else if($("#type_two input[type=radio]:checked").val() == 57002 && $("#type_four input").val() != ""){
        $("#amount").hide();
        RebateType2 = 57002;
        RebateValue = parseFloat($("#amount_val").val());
        console.log(RebateValue);
      }
      if($("#type_five input[type=radio]:checked").val() == 58001){
        RebateDateType = 58001;
      }else if($("#type_five input[type=radio]:checked").val() == 58002){
        RebateDateType = 58002;
      }
    }
    //返货部分验证取值
    if($("#type_one input[type=radio]:checked").val() == 56002){
      if($("#type_six input").val() == ""){
        $("#proportion_goods").show();
        flag = false;
        return false;
      }else if($("#type_six input").val() > 100 ){
        $("#proportion_goods").hide();
        $("#proportion_goods_two").show();
        flag = false;
        return false;
      }else{
        $("#proportion_goods").hide();
        RebateType1 = 56002;
        RebateType2 = -2;
        RebateValue = parseFloat(($("#proportion_val_goods").val())/100);
        console.log(RebateValue);
      }
      if($("#type_five input[type=radio]:checked").val() == 58001){
        RebateDateType = 58001;
      }else if($("#type_five input[type=radio]:checked").val() == 58002){
        RebateDateType = 58002;
      }
    }
    //无的情况
    if($("#type_one input[type=radio]:checked").val() == 56003){
        RebateType1 = 56003;
        RebateType2 = -2;
        RebateValue = 0;
        RebateDateType = -2;
    }
    console.log(RebateValue);
  }
  //每次点击添加按钮让所有条件置为初始条件
  function empty_info () {
    //把验证的提示信息全部隐藏（从上往下）
    $("#mane").hide();
    $("#discount").hide();
    $("#discount_two").hide();
    $("#proportion").hide();
    $("#proportion_two").hide();
    $("#proportion_goods").hide();
    $("#proportion_goods_two").hide();
    $("#amount").hide();

    //满额部分清空条件
    $("#top_up").val("");
    // $("#equal").prop("checked",false);
    //单个账号金额部分清空条件
    $("#single").val("");
    //采购折扣部分清空条件
    $("#procurement").val("");
    //返点类型部分(含以下的部分)清空条件
    for(var i=0;i<$("#type_one input").length;i++){
      if($("#type_one input")[i].value == 56003){
        $("#type_one input")[i].checked = true;
        change_one ();
      }
    }
    for(var i=0;i<$("#type_two input").length;i++){
      if($("#type_two input")[i].value == 57001){
        $("#type_two input")[i].checked = true;
        change_two ();
      }
    }
    $("#type_three input").val("");
    $("#type_four input").val("");
    $("#type_six input").val("");
    for(var i=0;i<$("#type_five input").length;i++){
      if($("#type_five input")[i].value == 58001){
        $("#type_five input")[i].checked = true;
      }
    }
    $("#type_two").hide();
    $("#type_three").hide();
    $("#type_five").hide();
    $("#type_six").hide();
  }

  //关于表格的操作
  function table_opration () {
    $(".edit_btn").off("click").on("click", function () {
      var that = $(this);
      console.log(that);
      console.log($(".edit_btn").index(that));
      //取得操作数据的下标
      var del_index = $(".edit_btn").index(that);
      console.log(del_index);
      $("#zhezhao").show();
      $(".layer").show();
      $("#add_w").show();
      $("#add_z").hide();
      //置空输入框
      empty_info ();
      //获取表格信息并进行赋值的操作
      var tds = that.parents("tr").children('td');
      //满额部分
      $("#top_up").val(tds.eq(0).children('span').html());
      if(tds.eq(0).attr("judge") == "true"){
        $("#equal").prop("checked",true);
      }
      //单个账号金额部分
      //大于小于号部分
      for(var i=0;i<$("#select select option").length;i++){
        if($("#select select option")[i].value == tds.eq(1).attr("judge")){
          $("#select select option")[i].selected = true;
        }
      }
      //赋值部分
      $("#single").val(tds.eq(1).children('span').html());
      //采购折扣部分
      $('#procurement').val(tds.eq(2).children('span').html());
      //返点类型部分
      for(var i=0;i<$("#type_one input").length;i++){
        if($("#type_one input")[i].value == tds.eq(3).attr("difference")){
          $("#type_one input")[i].checked = true;
        }
      }

      //返现的操作
      if(tds.eq(5).attr("difference") == 56001){
        for(var i=0;i<$("#type_two input").length;i++){
          if($("#type_two input")[i].value == tds.eq(4).attr("difference")){
            $("#type_two input")[i].checked = true;
          }
        }
        if(tds.eq(4).attr("difference") == 57001){
          $('#proportion_val').val(tds.eq(5).children('span').html());
          console.log($('#proportion_val').val());
        }else if(tds.eq(4).attr("difference") == 57002){
          $('#amount_val').val(tds.eq(4).children('span').html());
        }
      }
      //返货的操作
      if(tds.eq(5).attr("difference") == 56002){
        $('#proportion_val_goods').val(tds.eq(5).children('span').html());
      }
      //返点时间
      for(var i=0;i<$("#type_five input").length;i++){
        if($("#type_five input")[i].value == tds.eq(6).attr("difference")){
          $("#type_five input")[i].checked = true;
        }
      }
      change_one();
      change_two();

      //点击保存按钮存数据
      $("#add_w").off("click").on("click", function () {
        console.log(del_index);
          //验证取值
          verify_value();
          if(flag){
            //去除重复的选项
            for(var i=0;i<PolicyList.length;i++){
              if(i!= del_index){
                // for(var i=0;i<PolicyList.length;i++){
                    if(Quota == PolicyList[i].Quota && QuotaIncludingEqual == PolicyList[i].QuotaIncludingEqual && ((SingleAccountSum > PolicyList[i].SingleAccountSum && SingleAccountSumType == 55002 && PolicyList[i].SingleAccountSumType == 55001) || (SingleAccountSum < PolicyList[i].SingleAccountSum && SingleAccountSumType == 55001 && PolicyList[i].SingleAccountSumType == 55002))){
                      layer.msg("满额与单个账号金额的组合不能重复，相同的满额和单个账号金额只能对应一个政策", {time:2000});
                      return false;
                    }
                    if(Quota == PolicyList[i].Quota && QuotaIncludingEqual == PolicyList[i].QuotaIncludingEqual && SingleAccountSum == PolicyList[i].SingleAccountSum && PolicyList[i].SingleAccountSumType == SingleAccountSumType){
                    layer.msg("满额与单个账号金额的组合不能重复，相同的满额和单个账号金额只能对应一个政策", {time:2000});
                    return false;
                    }
                // }
              }
            }
            //删除旧数据保存新数据
            PolicyList.splice(del_index,1);
            //获取页面上的数据传入集合中
            PolicyList.push({
              "PolicyID": 0,
              "Quota": Quota,
              "QuotaIncludingEqual": QuotaIncludingEqual,
              "SingleAccountSum": SingleAccountSum,
              "SingleAccountSumType": SingleAccountSumType,
              "PurchaseDiscount": PurchaseDiscount,
              "RebateType1": RebateType1,
              "RebateType2": RebateType2,
              "RebateValue": RebateValue,
              "RebateDateType": RebateDateType
            })
            console.log(PolicyList);
            //对PolicyList数组进行正序排列
            PolicyList.sort(compare("Quota","SingleAccountSum"));
            console.log(PolicyList);
            //渲染表格信息出现表格
            $(".table").html(ejs.render($("#table").html(),{PolicyList:PolicyList}));
            //对表格的操作（编辑跟删除）
            table_opration ();
            $(".layer").hide();
            $("#policy").show();
            $("#zhezhao").hide();
          }
        })
    })
    //点击删除的操作
    $(".delete_btn").off("click").on("click", function () {
      var that = $(this);
      console.log(that);
      console.log($(".delete_btn").index(that));
      //取得操作数据的下标
      var del_index = $(".delete_btn").index(that);
      //删除旧数据保存新数据
      PolicyList.splice(del_index,1);
      //对PolicyList数组进行正序排列
      PolicyList.sort(compare("Quota","SingleAccountSum"));
      console.log(PolicyList);
      //渲染表格信息出现表格
      $(".table").html(ejs.render($("#table").html(),{PolicyList:PolicyList}));
      //对表格的操作（编辑跟删除）
      table_opration ();
      if(PolicyList.length == 0){
        $("#policy").hide();
      }else{
        $("#policy").show();
      }
    })
  }

  //在弹窗出现的时候的输入验证
  function validation () {

    //添加数据的时候的限制条件
    var reg_mane = /^[0-9]*$/;
    //满额限制
    $("#top_up").on("input", function () {
      if(!reg_mane.test($("#top_up").val())){
        $("#top_up").val("");
      }else{
        $("#mane").hide();
      }
    })
    //单个账号金额输入限制
    $("#single").on("input", function () {
      if(!reg_mane.test($("#single").val())){
        $("#single").val("");
      }
    })
    //采购折扣输入限制
    var reg_discount = /^-?(?!0+(\.0*)?$)\d+(\.\d{1,2})?$/;
    $("#procurement").on("blur", function () {
      if($("#procurement").val() != ""){
        if(!reg_discount.test($("#procurement").val())){
          // $("#procurement").val("");
          $("#discount").hide();
          $("#discount_two").show();
          flag2 = false;
        }else{
          flag2 = true;
          $("#discount_two").hide();
          $("#discount").hide();

        }
      }
    })
    //选中了返点类型的必填验证
    if($("#type_one input[type=radio]:checked").val() == 56001){
      //返现比例格式验证
      if($("#type_two input[type=radio]:checked").val() == 57001){
        $("#proportion_val").on("blur", function () {
          if($("#proportion_val").val() != ""){
            if(!reg_discount.test($("#proportion_val").val())){
              // $("#proportion_val").val("");
              $("#proportion").hide()
              $("#proportion_two").show();
              flag2 = false;
            }else{
              flag2 = true;
              $("#proportion_two").hide();
              $("#proportion").hide();
            }
          }
        })
      }
      //返现金额输入验证
      if($("#type_two input[type=radio]:checked").val() == 57002){
        $("#amount_val").on("input", function () {
          if(!reg_mane.test($("#amount_val").val())){
            $("#amount_val").val("");
          }else{
            $("#amount").hide();
          }
        })
      }
    }
    //返货时候的验证
    if($("#type_one input[type=radio]:checked").val() == 56002){
      $("#proportion_val_goods").on("blur", function () {
        if($("#proportion_val_goods").val() != ""){
          if(!reg_discount.test($("#proportion_val_goods").val())){
            // $("#proportion_val_goods").val("");
            $("#proportion_goods").hide();
            $("#proportion_goods_two").show();
            flag2 = false;
          }else{
            flag2 = true;
            $("#proportion_goods_two").hide();
            $("#proportion_goods").hide();
          }
        }
      })
    }
  }
  //返点类型一级维度变化
  function change_one () {
    if($("#type_one input[type=radio]:checked").val() == 56001){
      $("#type_two").show();
      //每次点击恢复默认值
      // for(var i=0;i<$("#type_two input").length;i++){
      //   if($($("#type_two input")[i]).val() == 57001){
      //     $($("#type_two input")[i]).prop("checked","checked");
      //   }
      // }
      //每次切换清空上次的操作的数据
      // $("#proportion_val").val("");
      // $("#amount_val").val("");

      $("#type_three").show();
      $("#type_four").hide();
      $("#type_five").show();
      $("#type_six").hide();
    }
    if($("#type_one input[type=radio]:checked").val() == 56002){
      $("#type_two").hide();
      // $("#proportion_val").val("");
      $("#type_three").hide();
      $("#type_six").show();
      $("#type_four").hide();
      $("#type_five").show();

    }
    if($("#type_one input[type=radio]:checked").val() == 56003){
      $("#type_two").hide();
      $("#type_three").hide();
      $("#type_four").hide();
      $("#type_five").hide();
      $("#type_six").hide();
    }
    validation ();
  }
  //返点类型二级维度变化
  function change_two () {
    if($("#type_two input[type=radio]:checked").val() == 57001 && $("#type_two")[0].style.display != "none"){
      //每次切换清空上次的操作的数据
      // $("#proportion_val").val("");
      $("#type_three").show();
      $("#type_four").hide();
      $("#type_six").hide();
    }
    if($("#type_two input[type=radio]:checked").val() == 57002 && $("#type_two")[0].style.display != "none"){
      //每次切换清空上次的操作的数据
      $("#type_three").hide();
      // $("#amount_val").val("");
      $("#type_four").show();
      $("#type_six").hide();
    }
    validation ();
  }
  //当返点类型进行切换的时候应该把那些提示信息隐藏
  function hide_info () {
    $("#proportion").hide();
    $("#proportion_two").hide();
    $("#proportion_goods").hide();
    $("#proportion_goods_two").hide();
    $("#amount").hide();
  }

  //渠道名称部分
  $("#channel_name").on("blur", function () {
    var reg1 = /^[\u4e00-\u9fa5a-zA-Z0-9]{1,20}$/;
    if($("#channel_name").val() != ""){
      if(!reg1.test($("#channel_name").val())){
        $("#channel_info").hide();
        $("#channel_info_two").show();
        $("#channel_name").val($("#channel_name").val().substring(0,20));
        return false;
      }else{
        $("#channel_info_two").hide();
        $("#channel_info").hide();
      }
    }
  })
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
      // $("#startTime").val("");
      if(CooperateEndDate != ''){
        if(date > CooperateEndDate){
          $("#cycle").hide();
          $("#cycle_two").show();
          $("#startTime").val("");
          CooperateBeginDate = '';
          return false;
        }else{
          $("#cycle_two").hide();
          $("#cycle").hide();
          CooperateBeginDate = date;
        }
      }else{
        CooperateBeginDate = date;
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
      if(CooperateBeginDate != ''){
        if(date < CooperateBeginDate || date < laydate.now()){
          $("#cycle").hide();
          $("#cycle_two").show();
          $("#endTime").val("");
          CooperateEndDate = '';
          return false;
        }else{
          $("#cycle_two").hide();
          $("#cycle").hide();
          CooperateEndDate = date;
        }
      }else{
        CooperateEndDate = date;
      }
    }
  }
  //给开始时间跟结束时间绑定点击的事件
  $("#startTime").off("click").on("click", function () {
    laydate(start);
    console.log(CooperateBeginDate);
  })
  $("#endTime").off("click").on("click", function () {
    laydate(end);
    console.log(CooperateEndDate);
  })
  //备注部分
  $("#show_logic").on("blur", function () {
    var reg = /^[\u4e00-\u9fa5a-zA-Z0-9\W]{0,200}$/;
    if(!reg.test($("#show_logic").val())){
      $("#remark").show();
      $("#show_logic").val($("#show_logic").val().substring(0,200));
    }else{
      $("#remark").hide();
    }
  })
  //点击总的添加的部分
  $("#add_button").off("click").on("click", function () {
    //验证从上往下各个维度
    //渠道名称部分
    if($("#channel_name").val() == ""){
      $("#channel_info").show();
      return false;
    }else{
      $("#channel_info").hide();
      ChannelName = $("#channel_name").val();
    }

    //政策部分
    if(PolicyList.length == 0){
      $("#add_channel_info").show();
      return false;
    }else{
      $("#add_channel_info").hide();
    }
    //是否含税
    if($("#tax input[type=radio]:checked").val() == "是"){
      IncludingTax = true;
    }else{
      IncludingTax = false;
    }
    //合作周期
    if(CooperateBeginDate == "" || CooperateBeginDate == ""){
      $("#cycle").show();
      return false;
    }else{
      $("#cycle").hide();
    }
    //备注部分
    Remark = $("#show_logic").val();
    var param = {
      "ChannelID": 0,
      "ChannelName": ChannelName,
      "IncludingTax": IncludingTax,
      "CooperateBeginDate": CooperateBeginDate,
      "CooperateEndDate": CooperateEndDate,
      "Remark": Remark,
      "PolicyList": PolicyList
    }
    console.log(param);
    $.ajax({
      url: "http://www.chitunion.com/api/Channel/ModifyChannel",
      type: "post",
      dataType: "json",
      data: param,
      xhrFields: {
        withCredentials: true
      },
      crossDomain:true,
      success: function (data) {
        console.log(data);
        if(data.Status == 0){
          layer.msg("成功",{time:2000});
          window.location = "/ChannelManager/channelList.html";
        }else{
          layer.msg(data.Message,{time:2000});
        }
      }
    })
  })
  //点击返回返回渠道管理页面
  $("#re_button").off("click").on("click", function () {
    window.location = "/ChannelManager/channelList.html";
  })

  //单个账号金额部分的比较符号
  setAjax({
    url: "http://www.chitunion.com/api/DictInfo/GetDictInfoByTypeID",
    type: "get",
    data: {
      typeID: 55
    }
  },function (data) {
    if(data.Status == 0){
      var options = "";
      for(var i=0;i<data.Result.length;i++){
        options += '<option value="'+data.Result[i].DictId+'">'+data.Result[i].DictName+'</option>';
      }
      $("#select").html('<select>'+options+'</select>');
    }
  })

})
// //将时间戳转换为日期格式2017-05-06
Date.prototype.format = function (fmt) {
      var o = {
          "M+": this.getMonth() + 1,                 //月份
          "d+": this.getDate(),                    //日
          "h+": this.getHours(),                   //小时
          "m+": this.getMinutes(),                 //分
          "s+": this.getSeconds(),                 //秒
          "q+": Math.floor((this.getMonth() + 3) / 3), //季度
          "S": this.getMilliseconds()             //毫秒
      };
      if (/(y+)/.test(fmt)) {
          fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
      }
      for (var k in o) {
          if (new RegExp("(" + k + ")").test(fmt)) {
              fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
          }
      }
      return fmt;
  }
//   //时间加一天
function add_date(time) {
      time=time.split(' ')[0]
      var date1 = new Date(time);
      var s1 = date1.getTime();
      var day = parseInt(s1 + (24 * 60 * 60 * 1000));//计算整数天数
      var str1 = new Date(day).format("yyyy-MM-dd");
      return str1;
  }
