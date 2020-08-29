/**
 * Created by luq on 2017/2/27.
 */
function ComponentSection(Data){
    this.State = {};
    /*获取出事数据源，一般为从后台返回的数据对象。*/
    this.getInitialState = function(){
        for(var i in Data){
            this.State[i] = Data[i];
        }
    }.bind(this)();
}
ComponentSection.prototype.addSelfState = function(object){
    for(var i in object){
        this.State[i] = object[i];
    }
};
ComponentSection.prototype.setState = function(object,fn){
    for(var i in object){
        this.State[i] = object[i];
    }
    if(fn){
        fn();
    }
};
 // 创建排期弹出框
ComponentSection.prototype.createADScheduleInfos = function(data,RangeDate,fn){
    var HasSelected = data;
    function isLeapYear(year) {
        var cond1 = year % 4 == 0;  //条件1：年份必须要能被4整除
        var cond2 = year % 100 != 0;  //条件2：年份不能是整百数
        var cond3 = year % 400 ==0;  //条件3：年份是400的倍数
        //当条件1和条件2同时成立时，就肯定是闰年，所以条件1和条件2之间为“与”的关系。
        //如果条件1和条件2不能同时成立，但如果条件3能成立，则仍然是闰年。所以条件3与前2项为“或”的关系。
        //所以得出判断闰年的表达式：
        var cond = cond1 && cond2 || cond3;
        if(cond) {
            // alert(year + "是闰年");
            return true;
        } else {
            // alert(year + "不是闰年");
            return false;
        }
    }
    var BeginYear = RangeDate[0].substr(0,4);
    var EndYear = RangeDate[1].substr(0,4);
    // var EndYear = 2018;
    var BeginMonth = RangeDate[0].substr(5,2);
    var EndMonth = RangeDate[1].substr(5,2);
    // var EndMonth = "09";
    var BeginDay = RangeDate[0].substr(8,2);
    var EndDay = RangeDate[1].substr(8,2);
    // var EndDay = "10";
    var DispalyArray = {};
    if(BeginYear == EndYear){
        DispalyArray[BeginYear] = [];
    }else{
        DispalyArray[BeginYear] = [];
        DispalyArray[EndYear] = [];
    }
    if(parseInt(BeginYear) != parseInt(EndYear) && parseInt(BeginYear) < parseInt(EndYear)){
        for(var i=parseInt(BeginMonth); i <= 12; i++){
            if(i<10){
                DispalyArray[BeginYear].push("0"+i);
            }else{
                DispalyArray[BeginYear].push(i.toString());
            }
        }
        for(var i = 1; i<=parseInt(EndMonth); i++){
            if(i<10){
                DispalyArray[EndYear].push("0"+i);
            }else{
                DispalyArray[EndYear].push(i.toString());
            }
        }
    }else if(parseInt(BeginYear) == parseInt(EndYear)){
        for(var i = parseInt(BeginMonth); i <= parseInt(EndMonth); i++){
            if(i<10){
                DispalyArray[BeginYear].push("0"+i);
            }else{
                DispalyArray[BeginYear].push(i.toString());
            }
        }
    }
    for(var i = parseInt(BeginYear)+1; i<parseInt(EndYear); i++){
        DispalyArray[i] = ["01","02","03","04",'05','06','07','08','09','10','11','12'];
    }
    // 根据月份判定这个月有多少天，
    var TempDispalyArray = {};
    if(parseInt(BeginYear) != parseInt(EndYear) && parseInt(BeginYear) < parseInt(EndYear)){
        for(var i in DispalyArray){
            if(i == BeginYear){
                if(isLeapYear(i)){
                    TempDispalyArray[i] = DispalyArray[i].map(function(item){
                        if(Number(item) == 2){
                            if(Number(item) == BeginMonth){
                                return{
                                    Month:item,
                                    BeginDay:parseInt(BeginDay),
                                    EndDay:29
                                }
                            }else{
                                return {
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:29
                                }
                            }
                        }else if(Number(item)%2 == 0 && Number(item) != 2 ){
                            if(Number(item) == BeginMonth){
                                return{
                                    Month:item,
                                    BeginDay:parseInt(BeginDay),
                                    EndDay:30
                                }
                            }else{
                                return {
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:30
                                }
                            }
                        }else{
                            if(Number(item) == BeginMonth){
                                return{
                                    Month:item,
                                    BeginDay:parseInt(BeginDay),
                                    EndDay:31
                                }
                            }else{
                                return {
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:31
                                }
                            }
                        }
                    });
                }else{
                    TempDispalyArray[i] = DispalyArray[i].map(function(item){
                        if(Number(item) == 2){
                            if(Number(item) == BeginMonth){
                                return{
                                    Month:item,
                                    BeginDay:parseInt(BeginDay),
                                    EndDay:28
                                }
                            }else{
                                return {
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:28
                                }
                            }
                        }else if(Number(item)%2 == 0 && Number(item) != 2 ){
                            if(Number(item) == BeginMonth){
                                return{
                                    Month:item,
                                    BeginDay:parseInt(BeginDay),
                                    EndDay:30
                                }
                            }else{
                                return {
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:30
                                }
                            }
                        }else{
                            if(Number(item) == BeginMonth){
                                return{
                                    Month:item,
                                    BeginDay:parseInt(BeginDay),
                                    EndDay:31
                                }
                            }else{
                                return {
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:31
                                }
                            }
                        }
                    });
                }
            }else if(i == EndYear){
                if(isLeapYear(i)){
                    TempDispalyArray[i] = DispalyArray[i].map(function(item){
                        if(Number(item) == 2){
                            if(Number(item) == EndMonth){
                                return{
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:parseInt(EndMonth)
                                }
                            }else{
                                return {
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:29
                                }
                            }
                        }else if(Number(item)%2 == 0 && Number(item) != 2 ){
                            if(Number(item) == EndMonth){
                                return{
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:parseInt(EndMonth)
                                }
                            }else{
                                return {
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:30
                                }
                            }
                        }else{
                            if(Number(item) == EndMonth){
                                return{
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:parseInt(EndMonth)
                                }
                            }else{
                                return {
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:31
                                }
                            }
                        }
                    });
                }else{
                    TempDispalyArray[i] = DispalyArray[i].map(function(item){
                        if(Number(item) == 2){
                            if(Number(item) == EndMonth){
                                return{
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:parseInt(EndDay)
                                }
                            }else{
                                return {
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:28
                                }
                            }
                        }else if(Number(item)%2 == 0 && Number(item) != 2 ){
                            if(Number(item) == EndMonth){
                                return{
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:parseInt(EndDay)
                                }
                            }else{
                                return {
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:30
                                }
                            }
                        }else{
                            if(Number(item) == EndMonth){
                                return{
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:parseInt(EndDay)
                                }
                            }else{
                                return {
                                    Month:item,
                                    BeginDay:1,
                                    EndDay:31
                                }
                            }
                        }
                    });
                }
            }else{
                if(isLeapYear(i)){
                    TempDispalyArray[i] = DispalyArray[i].map(function(item){
                        if(Number(item) == 2){
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:29
                            }
                        }else if(Number(item)%2 == 0 && Number(item) != 2 ){
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:30
                            }
                        }else{
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:31
                            }
                        }
                    });
                }else{
                    TempDispalyArray[i] = DispalyArray[i].map(function(item){
                        if(Number(item) == 2){
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:28
                            }
                        }else if(Number(item)%2 == 0 && Number(item) != 2 ){
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:30
                            }
                        }else{
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:31
                            }
                        }
                    });
                }
            }
        }
    }else if(parseInt(BeginYear) == parseInt(EndYear)){
        if(isLeapYear(BeginYear)){
            if(parseInt(BeginMonth) == parseInt(EndMonth)){
                TempDispalyArray[BeginYear] = DispalyArray[BeginYear].map(function(item){
                    return{
                        Month:item,
                        BeginDay:parseInt(BeginDay),
                        EndDay:parseInt(EndDay)
                    }
                });
            }else{
                TempDispalyArray[BeginYear] = DispalyArray[BeginYear].map(function(item,index){
                    if(index == 0){
                        if(Number(item) == 2){
                            return {
                                Month:item,
                                BeginDay:parseInt(BeginDay),
                                EndDay:29
                            }
                        }else if(Number(item)%2 == 0 && Number(item) != 2 ){
                            return {
                                Month:item,
                                BeginDay:parseInt(BeginDay),
                                EndDay:30
                            }
                        }else{
                            return {
                                Month:item,
                                BeginDay:parseInt(BeginDay),
                                EndDay:31
                            }
                        }
                    }else if(index == DispalyArray[BeginYear].length-1){
                        if(Number(item) == 2){
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:parseInt(EndDay)
                            }
                        }else if(Number(item)%2 == 0 && Number(item) != 2 ){
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:parseInt(EndDay)
                            }
                        }else{
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:parseInt(EndDay)
                            }
                        }
                    }else{
                        if(Number(item) == 2){
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:29
                            }
                        }else if(Number(item)%2 == 0 && Number(item) != 2 ){
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:30
                            }
                        }else{
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:31
                            }
                        }
                    }
                });
            }
        }else{
            if(parseInt(BeginMonth) == parseInt(EndMonth)){
               TempDispalyArray[BeginYear] = DispalyArray[BeginYear].map(function(item){
                    return{
                        Month:item,
                        BeginDay:parseInt(BeginDay),
                        EndDay:parseInt(EndDay)
                    }
                });
            }else{
                TempDispalyArray[BeginYear] = DispalyArray[BeginYear].map(function(item,index){
                    if(index == 0){
                        if(Number(item) == 2){
                            return {
                                Month:item,
                                BeginDay:parseInt(BeginDay),
                                EndDay:28
                            }
                        }else if(Number(item)%2 == 0 && Number(item) != 2 ){
                            return {
                                Month:item,
                                BeginDay:parseInt(BeginDay),
                                EndDay:30
                            }
                        }else{
                            return {
                                Month:item,
                                BeginDay:parseInt(BeginDay),
                                EndDay:31
                            }
                        }
                    }else if(index == DispalyArray[BeginYear].length-1){
                        if(Number(item) == 2){
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:parseInt(EndDay)
                            }
                        }else if(Number(item)%2 == 0 && Number(item) != 2 ){
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:parseInt(EndDay)
                            }
                        }else{
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:parseInt(EndDay)
                            }
                        }
                    }else{
                        if(Number(item) == 2){
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:28
                            }
                        }else if(Number(item)%2 == 0 && Number(item) != 2 ){
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:30
                            }
                        }else{
                            return {
                                Month:item,
                                BeginDay:1,
                                EndDay:31
                            }
                        }
                    }
                });
            }
        }
    }
    DispalyArray = TempDispalyArray;
    TempDispalyArray = null;
    // 根据已选择的数据来修改说明范围内那一天是选中的
    (function(){
        // console.log(DispalyArray,"dis");
        // console.log(HasSelected,"hs")
        HasSelected.forEach(function(item){
            var BeginTime = item.BeginDay;
            var EndTime = item.EndDay;
            for(var y = parseInt(BeginTime[0]); y<=parseInt(EndTime[0]); y++){
                for(var m = parseInt(BeginTime[1]); m <= parseInt(EndTime[1]); m++){
                    for(var d = parseInt(BeginTime[2]); d<=parseInt(EndTime[2]); d++){
                        DispalyArray[y].forEach(function(item){
                            if(item.Selected){

                            }else{
                                item.Selected = [];
                            }
                            if(parseInt(item.Month) == m){
                                item.Selected.push(d);
                            }
                        })
                    }
                }
            }
        });
    })();
    $("body").append("<div id='shadow'></div>");
    var Str = "<div id='shadow_bg' style='position:fixed;top:0;left:0;z-index:11;height:100%;width:100%;background:rgba(0,0,0,0.3);'></div>";
    var Result = ejs.render(Str,{});
    var target = "";
    $.ajax({
        url:"./ScheduleInfo.html",
        type:"get",
        dataType:"html",
        cache:false,
        success:function(data){
            target = data;
            // var result = ejs.render(target,{});
            $("#shadow_bg").html(ejs.render(target,{
                data:DispalyArray,
                HasSelected:HasSelected
            }));
            $("#closebt").off("click").on("click",function(){
                $("#shadow").remove();
            });
            $("#closeBtn").off("click").on("click",function(event){
                event.preventDefault();
                 $("#shadow").remove();
            });
            $("#EnsureBtn").off("click").on("click",function(event){
                event.preventDefault();
                GetAllSelectedDay(fn);
                $("#shadow").remove();
            });
            $(".ENClick").off("click").on("click",function(){
                if($(this).find("span.select").length == 0){
                    var itext = $(this).text();
                    $(this).text("");
                    $(this).append("<span class='select'>"+itext+"</span>");
                }else{
                    var text = $(this).find("span.select").text();
                    $(this).find(".select").remove();
                    $(this).text(text);
                }
            })
        }
    });
    function GetAllSelectedDay(fn){
        var array = [];
        $("#ScheduleBox").find("span.select").parent().each(function(){
            var attr = ($(this).attr("data-id"));
            array.push(attr);
        });
        var Barray = [];
        $("#ScheduleBox").find("li.ENClick").each(function(){
            var attr = $(this).attr("data-id");
            Barray.push(attr);
        });
        var Mainarray = [];
        var BEarray = [];
        var temp = 0;
        for(var j = 0; j < array.length; j++){
            for(var i = 0; i<Barray.length; i++){
                if(array[j] == Barray[i]){
                    if(BEarray.length > 0){
                        if(i-temp > 1){
                            BEarray.push(i);
                            temp = i;
                            break;
                        }
                        if(Barray[i+1] != array[j+1] ){
                            BEarray.push(i);
                            temp = i;
                            Mainarray.push(BEarray);
                            BEarray = [];
                            break;
                        }
                    }
                    if(BEarray.length == 0 && j == 0){
                        BEarray.push(i);
                        temp = i;
                        break;
                    }
                    if(i == Barray.length-1 && j == array.length-1){
                        BEarray.push(i);
                        Mainarray.push(BEarray);
                        BEarray = [];
                        break;
                    }
                    if(BEarray.length == 0 && j !=0){
                        BEarray.push(i);
                        temp = i;
                        if(Barray[i+1] != array[j+1]){
                            Mainarray.push(BEarray);
                            BEarray = [];
                        }
                        break;
                    }
                    temp = i;
                }
            }
        }
        // console.log(Mainarray,"main")
        var TempObjectSection = {
            BeginDay:[],
            EndDay:[]
        }
        var HasSelected = [];
        for(var i = 0; i< Mainarray.length; i++){
            if(Mainarray[i].length == 2){
                TempObjectSection.BeginDay = Barray[Mainarray[i][0]].substr(1,10).split(",");
                TempObjectSection.EndDay = Barray[Mainarray[i][1]].substr(1,10).split(",");
            }else if(Mainarray[i].length == 1){
                TempObjectSection.BeginDay = Barray[Mainarray[i][0]].substr(1,10).split(",");
                TempObjectSection.EndDay = Barray[Mainarray[i][0]].substr(1,10).split(",");
            }
            HasSelected.push({
                BeginDay:TempObjectSection.BeginDay,
                EndDay:TempObjectSection.EndDay
            });
        }
        if(fn){
            // console.log("HasSelected",HasSelected)
            fn(HasSelected,array.length);
        }
    }

    $("#shadow").html(Result);
};
/**
 *ejs初始化渲染函数，需要三个参数，
 * source:是指template模板的id(需要带#号)
 * target:是指渲染地的id(需要带#号)
 * Data:数据源（一般为对象）
 * */
ComponentSection.prototype.Render = function(source,target,fn){
    var Source = $(source).html();
    var Result = ejs.render(Source,this.State);
    $(target).html(Result);
    if(fn){
        fn();
    }
    Source = null;
    Result = null;
};
