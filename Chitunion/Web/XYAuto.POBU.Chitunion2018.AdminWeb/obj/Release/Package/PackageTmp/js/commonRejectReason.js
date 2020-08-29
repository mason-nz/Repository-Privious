/*
* Written by:     wangcan
* function:       公共作废原因
* Created Date:   2017-11-16
* Modified Date:
* params:BusinessType业务状态（9001初筛 9002清洗 9003封装，9004分发)
		ArticleType文章类型（1头部文章 2腰部文章 3物料）
*/
//显示列表
function  getReason(BusinessType,ArticleType){
	$.ajax({
        url: public_url+'/api/RejectionManage/SelectRejections',
        // url:'/PackageManager/json/A.json',
        type:'get',
        data:{
            BusinessType:BusinessType,
            ArticleType:ArticleType
        },
        dataType: 'json',
        async: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,
        success: function (data) {
            if(data.Status == 0) {
                var str = '';
                for (var n = 0; n < data.Result.RejectionList.length; n++) {
                    str += '<li style="cursor:pointer"><input type="checkbox" name="rejectReason" Rejection='+data.Result.RejectionList[n].RejectionID+' id=one'+n+' class="oneCheck"><label for=one'+n+'>' + data.Result.RejectionList[n].Rejection + '</label>';
	                if(data.Result.RejectionList[n].TwoRejection.length){
	                	var twoReject = '<ol>';
	                	data.Result.RejectionList[n].TwoRejection.forEach(function(item,j){
	                		twoReject += '<li style="cursor:pointer"><input type="checkbox" name="rejectReason2" Rejection='+item.RejectionID+' id=two'+j+'><label for=two'+j+' class="twoCheck">' + item.Rejection + '</label></li>';
	                	})
	                	twoReject += '</ol>';
	                	str += twoReject;
	                }
	                str += '</li>';
                }
                str +='<li><input type="text" id="addReason" style="height: 30px;line-height: 30px;margin-top: 5px;width: 90%;" placeholder="请输入自定义原因"></li>';
                $('.layer #rejectReason').html(str);
                $('#rejectReason input').off('change').on('change',function(){
                	var _this = $(this);
                	if(_this.attr('name') == 'rejectReason'){
                		if(_this.parent('li').find('ol').length){
                			if(_this.prop('checked')){
                				_this.parent('li').find('ol input').prop('checked',true)
                			}else{
                				_this.parent('li').find('ol input').prop('checked',false)
                			}
                		}
                	}else{
                		if(_this.parents('ol').find('input:checked').length){
                			_this.parents('ol').parent('li').find('.oneCheck').prop('checked',true)
                		}else{
                			_this.parents('ol').parent('li').find('.oneCheck').prop('checked',false)
                		}
                	}
                })
            }
        }
    })
}
/*
    获取已选作废原因
    params:ArticleID文章ID
            Type：驳回类型（1文章 2物料）
*/
function getSelectedReason(ArticleID,Type){
    var RejectionIds = [],DefaultId = 0;
    DefaultId = $('#rejectReason').find('input:first').attr('rejection')-0;
    $('#rejectReason').find('input').each(function(i,item){
        if($(item).prop('checked') == true){
            RejectionIds.push($(item).attr('rejection')-0)
        }
    })
    // $('#rejectReason>li').each(function(i,item){
    //     if($(item).find('input:checked').length){
    //         var singleReason = $(item).find('.oneCheck').attr('rejection')-0;
    //         var twoReasons = [];
    //         if($(item).find('ol input:checked').length){
    //             $(item).find('ol input:checked').each(function(j,single){
    //                 var twoReason = $(single).attr('rejection')-0;
    //                 twoReasons.push(twoReason);
    //             })
    //         }
    //         RejectionIds.push({
    //             RejectionId:singleReason,
    //             TwoRejectionId:twoReasons
    //         }) 
    //     }
    // })
    return {
        ArticleID:ArticleID-0,
        Type:Type-0,
        RejectionIds:RejectionIds,
        DefaultId:DefaultId,
        Custom:$.trim($('#addReason').val())
    }

}