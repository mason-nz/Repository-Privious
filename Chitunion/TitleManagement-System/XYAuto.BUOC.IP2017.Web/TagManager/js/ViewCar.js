/*
* Written by:     wangc
* function:       车型查看
* Created Date:   2017-10-23
* Modified Date:  
*/
$(function(){
    var LabelUrl = {
        ViewBatchCar:labelApi.tag+'/api/CarSerialLabel/ViewBatchCar'
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
    	url:LabelUrl.ViewBatchCar,
    	type:'get',
    	data:{
            BatchMediaID : GetRequest().BatchMediaID
        }
    },function(data){
    	if(data.Status == 0){
            var isEqual = {
                isIPLabel:data.Result.IpIsSame
            }
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
})