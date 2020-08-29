/*
* Written by:     wangcan
* function:       媒体查看
* Created Date:   2017-08-07
* Modified Date:
*/
$(function(){
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
    	url:'http://www.chitunion.com/api/LabelTask/SelectMediaOrArticleLable',//查媒体或文章信息http://www.chitunion.com/api/LabelTask/SelectMediaOrArticleLable
    	type:'get',
    	data:{
    		TaskID : GetRequest().TaskID,
            SelectType : GetRequest().SelectType
    	}
    },function(data){
    	if(data.Status == 0){
    		$('.container').html(ejs.render($('#temp').html(),{data:data.Result}));
            $('.sonip').each(function(){
                var h = $(this).parents('.SonIP').height()-20+'px';
                $(this).css('marginBottom',h)
            })
    	}else{
    		layer.msg(data.Message);
    	}	
    })
})