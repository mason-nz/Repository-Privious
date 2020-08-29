/*
* Written by:     wangcan
* function:       媒体查看--标签录入列表（审核中、已审查看），无原审
* Created Date:   2017-10-20
* Modified Date:  
*/
$(function(){
    var LabelUrl = {
        ViewBatchMedia:labelApi.tag+'/api/MediaLabel/ViewBatchMedia'//查看媒体
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
    	url:LabelUrl.ViewBatchMedia,
    	type:'get',
    	data:{
           BatchMediaID: GetRequest().BatchMediaID
        }
    },function(data){
    	if(data.Status == 0){
            var returnData = data.Result;
            //相等是true；不相等是false;默认相等
            var isEqual = {
                isCategory:true,
                isMarketScene:true,
                isDistributeScene:true,
                isIPLabel:data.Result.IpIsSame
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
                var i = 0;
                var _this = $(this);
                var h = _this.parents('.SonIP').height()-20+'px';
                var l = 0;
                _this.parents('ul').find('img').each(function(){
                    i++;
                    l += $(this).outerWidth()+10;
                })
                if(i){
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
})