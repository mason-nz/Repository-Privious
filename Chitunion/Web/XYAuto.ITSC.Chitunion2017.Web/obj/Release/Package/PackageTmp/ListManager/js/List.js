$(function () {
    // 获取当前时间
    Date.prototype.format = function(fmt) {
        var o = {
            "M+" : this.getMonth()+1,                 //月份
            "d+" : this.getDate(),                    //日
            "h+" : this.getHours(),                   //小时
            "m+" : this.getMinutes(),                 //分
            "s+" : this.getSeconds(),                 //秒
            "q+" : Math.floor((this.getMonth()+3)/3), //季度
            "S"  : this.getMilliseconds()             //毫秒
        };
        if(/(y+)/.test(fmt)) {
            fmt=fmt.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));
        }
        for(var k in o) {
            if(new RegExp("("+ k +")").test(fmt)){
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));
            }
        }
        return fmt;
    }
    function List() {
        this.fenlei();
    }
    List.prototype={
        constructor: List,
        // 分类
        fenlei: function () {
            var _this=this;
            setAjax({
                url:'http://www.chitunion.com/api/Ranking/CategoryList',
                type:'get',
                data:{
                    businesstype:14001
                }
            },function (data) {
                $('#fenlei2').html(ejs.render($('#fenlei1').html(), data));
                _this.liebiao();
            })
        },
        // 列表
        liebiao: function () {
            var _this=this;
            // 点击查询
            $('#List_button').off('click').on('click',function () {
                $('#fenlei2 li').each(function () {
                    $(this).siblings().find('a').attr('class','');
                });
                $('#fenlei2 li').eq(0).find('a').attr('class',"active")
                setAjax({
                    url:'http://www.chitunion.com/api/Ranking/Query',
                    type:'get',
                    data:_this.canshu()
                },function (data) {
                    $('#liebiao2').html(ejs.render($('#liebiao1').html(), {Result:data.Result.List}));
                    $('#riqi').html('更新日期：'+new Date((data.Result.Info.LastModifyTime).split(' ')[0]).format("yyyy年MM月dd日"))
                    _this.caozuo();
                    $('#table').hide();
                    if(data.Result.List.length<=0){
                        $('#table').show().html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                    }
                })
            })
            $('#List_button').click();
            // 点击分类
            $('#fenlei2 li').each(function () {
                $(this).off('click').on('click',function () {
                    $(this).siblings().find('a').attr('class','');
                    $(this).find('a').attr('class',"active");
                    setAjax({
                        url:'http://www.chitunion.com/api/Ranking/Query',
                        type:'get',
                        data:_this.canshu()
                    },function (data) {
                        $('#liebiao2').html(ejs.render($('#liebiao1').html(), {Result:data.Result.List}));
                        $('#riqi').html('更新日期：'+new Date((data.Result.Info.LastModifyTime).split(' ')[0]).format("yyyy年MM月dd日"))
                        _this.caozuo();
                        $('#table').hide();
                        if(data.Result.List.length<=0){
                            $('#table').show().html('<img src="/images/no_data.png" style="display: block;margin: 70px auto;">')
                        }
                    })
                })
            })
        },
        // 参数
        canshu: function () {
            // 获取昵称
            var List_name=$('#List_name').val();
            var fenlei;
            $('#fenlei2 li').each(function () {
                if($(this).find('a').attr('class')=="active"){
                    fenlei=$(this).find('a').attr('CategoryId')-0
                }
            });
            return {
                Keyword:List_name,
                CategoryId:fenlei,
                businesstype:14001
            }
        },
        // 操作
        caozuo: function () {
            // 跳转到详情页
            $('tbody tr').off('click').on('click',function () {
                window.open('/OrderManager/wx_detail.html?MediaID='+$(this).attr('MediaID')+'&MediaType=14001&BaseMediaID='+$(this).attr('BaseMediaID'))
            })
            // 固定导航
            $("#example1").posfixed({
                distance:0,
                pos:"top",
                type:"while",
                hide:false
            });
            $("#example2").posfixed({
                distance:0,
                pos:"top",
                type:"while",
                hide:false
            });
            $("#example3").posfixed({
                distance:40,
                pos:"top",
                type:"while",
                hide:false
            });
            // 点击数据说明
            $('#Explain').off('click').on('click',function () {
                $.openPopupLayer({
                    name: "clickApply",
                    url: "Elasticlayer.html",
                    error: function (dd) {
                        alert(dd.status);
                    },
                    success: function () {
                        $('#button').off('click').on('click',function () {
                            $.closePopupLayer('clickApply')
                        })

                    }})
            });
            // 返回顶部
            $(window).scroll(function(event){
                if($(document).scrollTop()>200){
                    $('#fanhuidingbu').show().off('click').on('click',function () {
                        $(document).scrollTop(0)
                    })
                }else {
                    $('#fanhuidingbu').hide();
                }
            });

        }
    }
    new List();

})
function quantum(data) {
    data-=0;
    if(data<10000){

        return data;
    }else if(10000<data&&data<100000){

        data=data/100000;
        data=parseFloat(data).toFixed(1)-0;
        if(!/^\d+$/.test(data)){
            data=parseFloat(data).toFixed(1)-0
        }else {
            data=parseInt(data)-0;
        }
        return data+'万';
    }else if(100000<=data){
        data=data/10000;
        data=parseFloat(data).toFixed(1)-0;
        console.log(data);
        if(!/^\d+$/.test(data)){
            data=parseFloat(data).toFixed(1)-0
        }else {
            console.log(1);
            data=parseInt(data)-0;
        }
        return data+'万+'
    }
}