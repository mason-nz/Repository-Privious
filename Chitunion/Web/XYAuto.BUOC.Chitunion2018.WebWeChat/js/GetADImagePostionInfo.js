/**
 * Written by:     zhengxh
 * Created Date:   2018/2/8
 */
// 图片
var img='';
// 插入广告
function insert() {
    // 可视区域高度
    var visibleAreaHeight=$(window).height();
    // 标题+作者信息高度
    var TitleOrInfo=$('.M_h2').outerHeight()+$('.M_authors').outerHeight();
    // 判断M_body_content下有多少标签
    var num=$('.M_body_content>*').length;
    // 获取内容高度
    var ContentHeight=TitleOrInfo;
    if(num<=1){
        // .M_body_content只有一个子标签
        $('.M_body_content>*>*').each(function () {
            //计算内容高度
            ContentHeight+=$(this).outerHeight();
            // 当内容高度的和大于可视区域时，在当前标签后插入图片
            if(ContentHeight>=visibleAreaHeight){
                $(this).after(img);
                return false
            }
        })
    }else {
        // .M_body_content有多个子标签
        $('.M_body_content>*').each(function () {
            //计算内容高度
            ContentHeight+=$(this).outerHeight();
            // 当内容高度的和大于可视区域时，在当前标签后插入图片
            if(ContentHeight>=visibleAreaHeight){
                $(this).after(img);
                return false
            }
        })
    }
    // 当内容高度的和小于可视区域时，在最后插入图片
    if(ContentHeight<visibleAreaHeight){
        $('.M_body_content').after(img)
    }
}
// 获取文章id
var MaterialID = $('#M_ArticleID').attr('articleID');
// 获取物料id
var MaterialID1 = (window.location+'').split('/')[(window.location+'').split('/').length-1].split('.')[0];
// 根据物料ID获取广告图片信息接口
$.ajax({
    url:'http://op1.chitunion.com/api/ApiNL/GetADImagePostionInfo',
    data:{
        MaterielID:MaterialID,
        TerminalType:11002
    },
    dataType:'json',
    async:false,
    success:function (data) {
        if(data.Status==0){
            // 百度统计
            // baidu_track_name='a' （标签名称）
            // baidu_track_action='click'（触发事件，如：click）
            // baidu_track_label='一辆安全的保姆车' （标签内容）
            // baidu_track_val='53805'（页面文章ID）
            if(data.Result.Middle){
                img='<a style="margin: 8px 0;display: block;" baidu_track_name=\'a\' baidu_track_action=\'click\' baidu_track_label="'+data.Result.Middle.ImageUrl+'" baidu_track_val="'+MaterialID1+'" href='+data.Result.Middle.Link+'><img src='+data.Result.Middle.ImageUrl+' alt=""></a>';
                insert()
            }
        }else {
            // alert(data.Message)
        }
    }
})