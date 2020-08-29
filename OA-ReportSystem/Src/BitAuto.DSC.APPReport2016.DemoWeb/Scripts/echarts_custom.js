
///*-----通用方法----------------------*/
//Object.prototype.APclone = function () {
//    var copy = (this instanceof Array) ? [] : {};
//    for (attr in this) {
//        if (!this.hasOwnProperty(attr)) continue;
//        copy[attr] = (typeof this[attr] == "object") ? this[attr].APclone() : this[attr];
//    }
//    return copy;
//};


/*-----图标默认配置---------(开始)----*/
var defaultEChartsTextStyle = {
    color: 'white',
    fontWeight: 'normal',
    fontFamily: '微软雅黑',
    fontSize: 12
};

//var defaultEChartsTextStyleFontBold = defaultEChartsTextStyle.APclone();
//defaultEChartsTextStyleFontBold.fontWeight = 'bold';



/*
用于绘制Pie图，中心圆形的对象
Add=masj,Date=2016-11-10    
*/
function GetInnerCircleByPieSeries(radiusValue,top,left)
{
    var v = ((radiusValue) ? radiusValue : 4);
    var vTop = ((top) ? top : 35);
    var vLeft = ((left) ? left : 50);
    var seriesObj = {
        name: '',
        type: 'pie',
        selectedOffset: 0,
        hoverAnimation: false,
        silent: true,
        z: 3,
        radius: [0, v+'%'],
        center: [vTop+'%', vLeft+'%'],
        label: {
            show: false
        },
        labelLine: {
            normal: {
                show: false
            }
        },
        data: [
                    { value: 0, name: '', itemStyle: { normal: { color: '#CCCCCC'}} }
                ]
    };
    return seriesObj;
}


/*-----图标默认配置---------(结束)----*/


