/*
* Written by:     zhengxh
* function:       需求审核详情
* Created Date:   2017-08-18
* Modified Date:
*/
/*

后期需要修改部分：1、审核接口调用时，需要使用post
	2、没有下一条审核信息时，需要跳转到对应的列表页

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
	var url = 'http://www.chitunion.com/api/Demand/GetDemandDetail';
	setAjax({
		url:url,
		type:'get',
		data:{
            DemandBillNo : GetRequest().DemandBillNo
		}
	},function(data){
		if(data.Status == 0){
			$('.main_info').html(ejs.render($('#detailForDemand').html(),{list:data.Result.Demand}))
			// 切换审核结果
	        $('input[name=shenhe]').off('change').on('change',function(){
	            if($(this).attr('class') == 'shenhe1'){
	                $('#parameter').hide();
	            }else{
	                $('#parameter').show();
	            }
	        });
	        // 点击审核
	        $('#Submit1').off('click').on('click',function () {
	        	if($('#Submit1').css('background-color') != 'rgb(255, 79, 79)'){
	        		return
	        	}
	            var upData={
                    DemandBillNo: data.Result.Demand.DemandBillNo,
                    AuditStatus: 89002,
                    Reason: ''
	            };
	            upData.AuditStatus=$('input[name=shenhe]:checked').attr('i')-0;
	            if(upData.AuditStatus == 89002){
	                upData.Reason = $('textarea').val();
	                if(!upData.Reason){
	                    layer.msg('驳回信息不能为空')
	                    return
	                }
	            }
	            if(upData.AuditStatus == 89003){
	                upData.Reason = '';
	            }
	            console.log(upData);
	            var url1 = 'http://www.chitunion.com/api/Demand/AuditDemand';
	            setAjax({
	                url:url1,
	                type:'post',
	                data:upData
	            },function (data) {
					if(data.Status==0){
						$('#Submit1').css('background-color','#eee');
						layer.msg('审核成功',{time: 1000},function () {
							if(data.Result){
								window.location='/JointManager/DemandAudit.html?DemandBillNo='+data.Result;
							}else{
								layer.msg('没有下一条审核信息了',{time: 2000},function () {
			                        window.location='/jointmanager/putin_list.html'//跳转到列表页
			                    })
							}
	                    })
					}else {
						layer.msg(data.Message);
					}
	                
	            })
	        });
		}else{
			layer.msg(data.Message);
		}
	})	
})