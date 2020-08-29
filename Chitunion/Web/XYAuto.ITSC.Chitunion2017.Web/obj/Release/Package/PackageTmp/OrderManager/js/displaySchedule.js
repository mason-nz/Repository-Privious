/*传入开始日期和结束日期，返回区间日期数组，闭区间*/
function displaySchedule(event){
	var curTr = event.parents('tr');
    var curAdscheduleinfos = curTr.attr('adscheduleinfos');

    /*存放已选日期*/
    var checkedDate = [];
    if(curAdscheduleinfos && curAdscheduleinfos.length != 0){
        var jsonCurAdscheduleinfos = JSON.parse(curAdscheduleinfos);

        /*
        判断日期数组中的开始日期和结束日期是否相同，若相同，将其中一个放进已选日期数组中
        若不相同，将开始日期和结束日期的区间日期都存放到已选日期数组中
		*/
        for(var i=0;i<jsonCurAdscheduleinfos.length;i++){
            var beginDate = jsonCurAdscheduleinfos[i].BeginData.substr(0,10),
                endDate = jsonCurAdscheduleinfos[i].EndData.substr(0,10);
                
            if(beginDate == endDate){
                checkedDate.push(endDate);
            }else{
                var date = getAll(beginDate,endDate);
                for(var j=0;j<date.length;j++){
                    checkedDate.push(date[j]);
                }
                checkedDate.push(endDate);
                checkedDate.unshift(beginDate);
            }
        }

    }
    /*判断排期中每个日期是否已选，若已选，则显示为已选样式*/
    var orderList = $(".choseLi");
    if(checkedDate.length != 0){
        var dateStr = JSON.stringify(checkedDate);
        for (var i = 0; i < orderList.length; i++) {
            //获取日期
            var curDate = $(orderList[i]).attr('data');

            if(dateStr.indexOf(curDate) != -1){
                $(orderList[i]).html("<span>" + $(orderList[i]).text() + "</span>")
                $(orderList[i]).children().addClass("select");
            }
        }
    }
}
/*数组去重*/
function arrRepeat(arr) {
    var result = []
    for (var i = 0; i < arr.length; i++) {
        if (result.indexOf(arr[i]) == -1) {
            result.push(arr[i])
        }
    }
    return result;
}
 /*获取两个时间之间的日期,调用函数getAll,注：不包括开始日期和结束日期*/
Date.prototype.format = function () {
    var s = '';
    s += this.getFullYear() + '-';// 获取年份。
    if (this.getMonth() + 1 < 10) {
        s += "0" + (this.getMonth() + 1) + "-";// 获取月份。
    } else {
        s += (this.getMonth() + 1) + "-";
    }
    if (this.getDate() < 10) {               // 获取日。
        s += "0" + this.getDate();
    } else {
        s += this.getDate();
    }
    return (s);                          // 返回日期。
};
function getAll(begin, end) {
    var arr = [];
    var ab = begin.split("-");
    var ae = end.split("-");
    var db = new Date();
    db.setUTCFullYear(ab[0], ab[1] - 1, ab[2]);
    var de = new Date();
    de.setUTCFullYear(ae[0], ae[1] - 1, ae[2]);
    var unixDb = db.getTime();
    var unixDe = de.getTime();
    for (var k = unixDb + 24 * 60 * 60 * 1000; k < unixDe;) {
        arr.push((new Date(parseInt(k))).format());
        k = k + 24 * 60 * 60 * 1000;
    }
    return arr;
}
/*传入开始日期和结束日期，在页面上显示所有日期*/
function setSchedule(beginDay,endDay){
    /*由开始日期和结束日期，得到日期数组*/
    var dataArr = getAll(beginDay, endDay);
    if(beginDay != endDay){
        dataArr.unshift(beginDay);
        dataArr.push(endDay);
    }else{
        dataArr.unshift(beginDay);
    }
    /*转换成只含有月份的数组 begin*/
    var tempmonthArr = [];
    for (var i = 0; i < dataArr.length; i++) {
        tempmonthArr.push(dataArr[i].substr(0, 7))
    }
    var monthArr = arrRepeat(tempmonthArr);
    //根据月份创建相应的日期li
    for(var i=0;i<monthArr.length;i++){
        var month = monthArr[i].substr(5,2)-0;
        switch(month){
            case 01:
            case 03:
            case 05:
            case 07:
            case 08:
            case 10:
            case 12:
                var dataMouth = "<div class='blue dataMouth'>" + monthArr[i] + "</div>";
                var dataDay = " <div class='order_date orderList' ><ul> " + "<li data='" + monthArr[i] + "-01" + "'>01</li>" + "<li data='" + monthArr[i] + "-02" + "'>02</li>" + "<li data='" + monthArr[i] + "-03" + "'>03</li>" + "<li data='" + monthArr[i] + "-04" + "'>04</li>" + "<li data='" + monthArr[i] + "-05" + "'>05</li>" + "<li data='" + monthArr[i] + "-06" + "'>06</li>" + "<li data='" + monthArr[i] + "-07" + "'>07</li>" + "<li data='" + monthArr[i] + "-08" + "'>08</li>" + "<li data='" + monthArr[i] + "-09" + "'>09</li>" + "<li data='" + monthArr[i] + "-10" + "'>10</li>" + "<li data='" + monthArr[i] + "-11" + "'>11</li>" + "<li data='" + monthArr[i] + "-12" + "'>12</li>" + "<li data='" + monthArr[i] + "-13" + "'>13</li>" + "<li data='" + monthArr[i] + "-14" + "'>14</li>" + "<li data='" + monthArr[i] + "-15" + "'>15</li>" + "<li data='" + monthArr[i] + "-16" + "'>16</li>" + "<li data='" + monthArr[i] + "-17" + "'>17</li>" + "<li data='" + monthArr[i] + "-18" + "'>18</li>" + "<li data='" + monthArr[i] + "-19" + "'>19</li>" + "<li data='" + monthArr[i] + "-20" + "'>20</li>" + "<li data='" + monthArr[i] + "-21" + "'>21</li>" + "<li data='" + monthArr[i] + "-22" + "'>22</li>" + "<li data='" + monthArr[i] + "-23" + "'>23</li>" + "<li data='" + monthArr[i] + "-24" + "'>24</li>" + "<li data='" + monthArr[i] + "-25" + "'>25</li>" + "<li data='" + monthArr[i] + "-26" + "'>26</li>" + "<li data='" + monthArr[i] + "-27" + "'>27</li>" + "<li data='" + monthArr[i] + "-28" + "'>28</li>" + "<li data='" + monthArr[i] + "-29" + "'>29</li>" + "<li data='" + monthArr[i] + "-30" + "'>30</li>" + "<li data='" + monthArr[i] + "-31" + "'>31</li>" + "<div class='clear'></div> </ul></div>";
                $("#dataContet").append(dataMouth);
                $("#dataContet").append(dataDay);
                break;
            case 04:
            case 06:
            case 09:
            case 11:
                var dataMouth = "<div class='blue dataMouth'>" + monthArr[i] + "</div>"
                var dataDay = " <div class='order_date orderList'><ul> " + "<li data='" + monthArr[i] + "-01" + "'>01</li>" + "<li data='" + monthArr[i] + "-02" + "'>02</li>" + "<li data='" + monthArr[i] + "-03" + "'>03</li>" + "<li data='" + monthArr[i] + "-04" + "'>04</li>" + "<li data='" + monthArr[i] + "-05" + "'>05</li>" + "<li data='" + monthArr[i] + "-06" + "'>06</li>" + "<li data='" + monthArr[i] + "-07" + "'>07</li>" + "<li data='" + monthArr[i] + "-08" + "'>08</li>" + "<li data='" + monthArr[i] + "-09" + "'>09</li>" + "<li data='" + monthArr[i] + "-10" + "'>10</li>" + "<li data='" + monthArr[i] + "-11" + "'>11</li>" + "<li data='" + monthArr[i] + "-12" + "'>12</li>" + "<li data='" + monthArr[i] + "-13" + "'>13</li>" + "<li data='" + monthArr[i] + "-14" + "'>14</li>" + "<li data='" + monthArr[i] + "-15" + "'>15</li>" + "<li data='" + monthArr[i] + "-16" + "'>16</li>" + "<li data='" + monthArr[i] + "-17" + "'>17</li>" + "<li data='" + monthArr[i] + "-18" + "'>18</li>" + "<li data='" + monthArr[i] + "-19" + "'>19</li>" + "<li data='" + monthArr[i] + "-20" + "'>20</li>" + "<li data='" + monthArr[i] + "-21" + "'>21</li>" + "<li data='" + monthArr[i] + "-22" + "'>22</li>" + "<li data='" + monthArr[i] + "-23" + "'>23</li>" + "<li data='" + monthArr[i] + "-24" + "'>24</li>" + "<li data='" + monthArr[i] + "-25" + "'>25</li>" + "<li data='" + monthArr[i] + "-26" + "'>26</li>" + "<li data='" + monthArr[i] + "-27" + "'>27</li>" + "<li data='" + monthArr[i] + "-28" + "'>28</li>" + "<li data='" + monthArr[i] + "-29" + "'>29</li>" + "<li data='" + monthArr[i] + "-30" + "'>30</li>" + "<div class='clear'></div> </ul></div>"
                $("#dataContet").append(dataMouth);
                $("#dataContet").append(dataDay);
                break;
            case 02:
                var year = monthArr[i].substr(0, 4) - 0;
                if (year % 400 == 0 || (year % 4 == 0 && year % 100 != 0)) {
                    var dataMouth = "<div class='blue dataMouth'>" + monthArr[i] + "</div>"
                    var dataDay = " <div class='order_date orderList'><ul> " + "<li data='" + monthArr[i] + "-01" + "'>01</li>" + "<li data='" + monthArr[i] + "-02" + "'>02</li>" + "<li data='" + monthArr[i] + "-03" + "'>03</li>" + "<li data='" + monthArr[i] + "-04" + "'>04</li>" + "<li data='" + monthArr[i] + "-05" + "'>05</li>" + "<li data='" + monthArr[i] + "-06" + "'>06</li>" + "<li data='" + monthArr[i] + "-07" + "'>07</li>" + "<li data='" + monthArr[i] + "-08" + "'>08</li>" + "<li data='" + monthArr[i] + "-09" + "'>09</li>" + "<li data='" + monthArr[i] + "-10" + "'>10</li>" + "<li data='" + monthArr[i] + "-11" + "'>11</li>" + "<li data='" + monthArr[i] + "-12" + "'>12</li>" + "<li data='" + monthArr[i] + "-13" + "'>13</li>" + "<li data='" + monthArr[i] + "-14" + "'>14</li>" + "<li data='" + monthArr[i] + "-15" + "'>15</li>" + "<li data='" + monthArr[i] + "-16" + "'>16</li>" + "<li data='" + monthArr[i] + "-17" + "'>17</li>" + "<li data='" + monthArr[i] + "-18" + "'>18</li>" + "<li data='" + monthArr[i] + "-19" + "'>19</li>" + "<li data='" + monthArr[i] + "-20" + "'>20</li>" + "<li data='" + monthArr[i] + "-21" + "'>21</li>" + "<li data='" + monthArr[i] + "-22" + "'>22</li>" + "<li data='" + monthArr[i] + "-23" + "'>23</li>" + "<li data='" + monthArr[i] + "-24" + "'>24</li>" + "<li data='" + monthArr[i] + "-25" + "'>25</li>" + "<li data='" + monthArr[i] + "-26" + "'>26</li>" + "<li data='" + monthArr[i] + "-27" + "'>27</li>" + "<li  data='" + monthArr[i] + "-28" + "'>28</li>" + "<li data='" + monthArr[i] + "-29" + "'>29</li>" + "<div class='clear'></div> </ul></div>"
                    $("#dataContet").append(dataMouth);
                    $("#dataContet").append(dataDay);
                    break;
                } else {
                    var dataMouth = "<div class='blue dataMouth'>" + monthArr[i] + "</div>"
                    var dataDay = " <div class='order_date orderList'><ul> " + "<li data='" + monthArr[i] + "-01" + "'>01</li>" + "<li data='" + monthArr[i] + "-02" + "'>02</li>" + "<li data='" + monthArr[i] + "-03" + "'>03</li>" + "<li data='" + monthArr[i] + "-04" + "'>04</li>" + "<li data='" + monthArr[i] + "-05" + "'>05</li>" + "<li data='" + monthArr[i] + "-06" + "'>06</li>" + "<li data='" + monthArr[i] + "-07" + "'>07</li>" + "<li data='" + monthArr[i] + "-08" + "'>08</li>" + "<li data='" + monthArr[i] + "-09" + "'>09</li>" + "<li data='" + monthArr[i] + "-10" + "'>10</li>" + "<li data='" + monthArr[i] + "-11" + "'>11</li>" + "<li data='" + monthArr[i] + "-12" + "'>12</li>" + "<li data='" + monthArr[i] + "-13" + "'>13</li>" + "<li data='" + monthArr[i] + "-14" + "'>14</li>" + "<li data='" + monthArr[i] + "-15" + "'>15</li>" + "<li data='" + monthArr[i] + "-16" + "'>16</li>" + "<li data='" + monthArr[i] + "-17" + "'>17</li>" + "<li data='" + monthArr[i] + "-18" + "'>18</li>" + "<li data='" + monthArr[i] + "-19" + "'>19</li>" + "<li data='" + monthArr[i] + "-20" + "'>20</li>" + "<li data='" + monthArr[i] + "-21" + "'>21</li>" + "<li data='" + monthArr[i] + "-22" + "'>22</li>" + "<li data='" + monthArr[i] + "-23" + "'>23</li>" + "<li data='" + monthArr[i] + "-24" + "'>24</li>" + "<li data='" + monthArr[i] + "-25" + "'>25</li>" + "<li data='" + monthArr[i] + "-26" + "'>26</li>" + "<li data='" + monthArr[i] + "-27" + "'>27</li>" + "<li data='" + monthArr[i] + "-28" + "'>28</li>" + "<div class='clear'></div> </ul></div>"
                    $("#dataContet").append(dataMouth);
                    $("#dataContet").append(dataDay);
                    break;
                }
        }
    }
    /*根据选择的选择的日期间隔确定可选日期*/
    var beginday = beginDay.substr(8, 2) - 0;
    var endMonth = endDay.substr(5, 2) - 0;
    var endday = endDay.substr(8, 2) - 0;
    var dataList = $("li[data]");
    for (var i = 0; i < beginday - 1; i++) {
        var thisLi = dataList[i];
        $(thisLi).attr("class", "invalid");
    }
    for (var i = beginday - 1; i < dataList.length; i++) {
        var thisLi = dataList[i];
        $(thisLi).attr("class", "choseLi");
    }
    if (endMonth == 1 || endMonth == 3 || endMonth == 5 || endMonth == 7 || endMonth == 8 || endMonth == 10 || endMonth == 12) {
        for (var i = dataList.length - (31 - endday); i < dataList.length; i++) {
            var thisLi = dataList[i];
            $(thisLi).attr("class", "invalid");
        }
    } else if (endMonth == 4 || endMonth == 6 || endMonth == 9 || endMonth == 11) {
        for (var i = dataList.length - (30 - endday); i < dataList.length; i++) {
            var thisLi = dataList[i];
            $(thisLi).attr("class", "invalid");
        }
    } else {
        for (var i = dataList.length - (29 - endday); i < dataList.length; i++) {
            var thisLi = dataList[i];
            $(thisLi).attr("class", "invalid");
        }
    }
}
/*获取所有已选择的排期，存在数组中*/
function getDataArr() {
    var dataArr = [];
    var selectList = $(".select");
    for (var i = 0; i < selectList.length; i++) {
        dataArr.push($(selectList[i]).parent().attr("data"));
    }
    return dataArr;
}