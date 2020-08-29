/*
* Written by:     wangcan
* function:       车型查看--最终
* Created Date:   2017-10-20
* Modified Date:  
*/
$(function(){
    var LabelUrl = {
        QueryResultBrandLabel:labelApi.tag+'/api/ResultLabel/QueryResultBrandLabel'//查看车型最终结果
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
    	url:LabelUrl.QueryResultBrandLabel,
    	type:'get',
    	data:{
    		MediaResultID : GetRequest().MediaResultID//审核结果ID
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