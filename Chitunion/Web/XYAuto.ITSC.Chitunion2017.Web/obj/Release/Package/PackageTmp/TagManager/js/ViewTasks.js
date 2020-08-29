/*
* Written by:     wangcan
* function:       查看任务
* Created Date:   2017-08-04
* Modified Date:
*/
$(function(){
	// 获取url？后面的参数
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
	var url = 'http://www.chitunion.com/api/LabelTask/ProjectQuery';
	setAjax({
		url:url,
		type:'get',
		data:{
			projectID : GetRequest().projectID
		}
	},function(data){
		if(data.Status == 0){
			$('.main_info').html(ejs.render($('#detailFforArticle').html(),{list:data.Result}))
		}else{
			layer.msg(data.Message);
		}
	})	
})