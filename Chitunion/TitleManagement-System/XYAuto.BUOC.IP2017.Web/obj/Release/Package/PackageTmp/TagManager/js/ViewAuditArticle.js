/*
* Written by:     wangcan
* function:       查看文章--审核人在已审媒体标签列表查看，有原审
* Created Date:   2017-10-23
* Modified Date:  
*/
$(function(){
    var LabelUrl = {
        ViewAuditedMedia:labelApi.tag+'/api/ExamineLabel/QueryAuditMediaLabel',//审核人查看媒体文章
        QueryArticle:labelApi.tag+'/api/MediaLabel/QueryArticle'//查询媒体下的文章
    }
    function GetRequest() {
        var url = location.search; //获取url中"?"符后的字串
        var theRequest = new Object();
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
            }
        }
        return theRequest;
    }
    setAjax({
        url:LabelUrl.ViewAuditedMedia,
        type:'get',
        data:{
            BatchAuditID : GetRequest().BatchAuditID
        }
    },function(data){
        if(data.Status == 0){
            var returnData = data.Result;
            //相等是true；不相等是false;默认相等
            var isEqual = {
                isCategory:true,
                isMarketScene:true,
                isDistributeScene:true,
                isIPLabel:returnData.IpIsSame
            }
            var oldCategory = [],oldScene = [],oldDistributeScene = [],oldIPLabel = [],
                newCategory = [],newScene = [],newDistributeScene = [],newIPLabel = [];
            //分类
            if(returnData.Category.Original){
                returnData.Category.Original.forEach(function(item){
                    oldCategory.push(item.DictName);
                })
            }
            if(returnData.Category.Audit){
                returnData.Category.Audit.forEach(function(item){
                    newCategory.push(item.DictName);
                })
            }
            //原分类在新分类中是否存在，存在返回true，不存在返回false
            oldCategory.forEach(function(item){
                //若不存在，说明审后有新的分类
                if(contains(newCategory,item) == false){
                    isEqual.isCategory = false;
                }
            })
            newCategory.forEach(function(item){
                //若不存在，说明审后有新的分类
                if(contains(oldCategory,item) == false){
                    isEqual.isCategory = false;
                }
            })
            //场景
            if(returnData.MarketScene.Original){
                returnData.MarketScene.Original.forEach(function(item){
                    oldScene.push(item.DictName);
                })
                
            }
            if(returnData.MarketScene.Audit){
                returnData.MarketScene.Audit.forEach(function(item){
                    newScene.push(item.DictName);
                })
                
            }
            //原场景在新场景中是否存在，存在返回true，不存在返回false
            oldScene.forEach(function(item){
                //若不存在，说明审后有新的场景
                if(contains(newScene,item) == false){
                    isEqual.isMarketScene = false;
                }
            })
            newScene.forEach(function(item){
                //若不存在，说明审后有新的场景
                if(contains(oldScene,item) == false){
                    isEqual.isMarketScene = false;
                }
            })
            //分发场景
            if(returnData.DistributeScene.Original){
                returnData.DistributeScene.Original.forEach(function(item){
                    oldDistributeScene.push(item.DictName);
                })
                
            }
            if(returnData.DistributeScene.Audit){
                returnData.DistributeScene.Audit.forEach(function(item){
                    newDistributeScene.push(item.DictName);
                })
                
            }
            //原分发场景在新场景中是否存在，存在返回true，不存在返回false
            oldDistributeScene.forEach(function(item){
                //若不存在，说明审后有新的场景
                if(contains(newDistributeScene,item) == false){
                    isEqual.isDistributeScene = false;
                }
            })
            newDistributeScene.forEach(function(item){
                //若不存在，说明审后有新的场景
                if(contains(oldDistributeScene,item) == false){
                    isEqual.isDistributeScene = false;
                }
            })

            data.Result.isEqual = isEqual;
            $('.container').html(ejs.render($('#temp').html(),{data:data.Result}));
            //设置Marin-left等值
            var ml = 0;
            $('.changeLeft .sonip').each(function(){
                var _this = $(this);
                var count = 0;
                var h = _this.parents('.SonIP').height()-20+'px';
                var l = 0;
                _this.parents('ul').find('img').each(function(){
                    count++;
                    l += $(this).outerWidth()+10;
                })
                if(count){
                    l+=10;
                }
                ml = l;
                _this.css({'marginBottom':h,'marginLeft':l})
            })

            $('.changeLeft .setMarL').each(function(i,item){
                if(i){
                    $(item).css({'marginLeft':ml})
                }
            })

            var articleIDArr = data.Result.ArticleIDs.split(',');
            var num = articleIDArr.length;
            var width=($('#tab_article').parent('div').width()-40-40)/20;
            var tab='';
            for(var i=1;i<=articleIDArr.length;i++){
               tab+='<div articleID="'+articleIDArr[i-1]+'" style="cursor: pointer;text-align:center;border: 1px solid #f7f7f7;float: left;height: 28px;line-height: 28px;width:'+width+'px">'+i+'</div>';
            }
            tab+='<div class="clear"></div>'
            $('#tab_article').html(tab).css('width',(width+2)*num+40+'px').find('div').eq(0).addClass('article_color');
            searchArticle();
            if(num>20){
                if($('#tab_article').width()>$('#tab_article').parent('div').width()){
                    $('.regin_span').addClass('red')
                }
                $('.regin_span').off('click').on('click',function () {
                    $('.left_span').addClass('red');
                    if($('#tab_article').position().left-((width+2)*20)*2<=-$('#tab_article').width()+40){
                        $('.regin_span').removeClass('red');
                    }
                    if($('#tab_article').position().left-((width+2)*20)<=-$('#tab_article').width()+40){
                        $('.left_span').addClass('red');
                        $('.regin_span').removeClass('red');
                        return false;
                    }
                    $('#tab_article').css('left',-((width+2)*20)+ $('#tab_article').position().left+'px');
                })
                $('.left_span').off('click').on('click',function () {
                    $('.regin_span').addClass('red');
                    if($('#tab_article').position().left+((width+2)*20)>=0){
                        $('.left_span').removeClass('red');
                    }
                    if($('#tab_article').position().left>=0){
                        $('.left_span').removeClass('red');
                        return false;
                    }
                    $('#tab_article').css('left',$('#tab_article').position().left+((width+2)*20)+'px');
                })
            }
            $('#tab_article div').off('click').on('click',function () {
                $(this).addClass('article_color').siblings().removeClass('article_color');
                searchArticle();
            })
            //切换文章标题显示和原文显示
            $('.switch').off('click').on('click','span',function(){
                $(this).addClass('current').siblings().removeClass('current');
                var value = $(this).attr('value');
                if(value == '0'){
                    $('.keyWords').show();
                    $('.article').hide();
                }else if(value == '1'){
                    $('.keyWords').hide();
                    $('.article').show();
                }
            })
        }else{
            layer.msg(data.Message);
        }   
    })
    function contains(arr, label) {
        var i = arr && arr.length;
        while (i--) {
            if (arr[i] === label) {
                return true;
            }
        }
        return false;
    }
    function searchArticle(){
        setAjax({
            url:LabelUrl.QueryArticle,
            type:'get',
            data:{
                articleID:$('.article_color').attr('articleid'),
                mediaType:GetRequest().mediaType
            }
        },function(data){
            if(data.Status == 0){
                if(data.Result.Title){
                    $('#articleTitle').html('标题：'+data.Result.Title);
                }
                if(data.Result.Abstract){
                    $('.summary').html(data.Result.Abstract);
                }
                $('.article_main').html(data.Result.Content);
            }
        })
    }

})