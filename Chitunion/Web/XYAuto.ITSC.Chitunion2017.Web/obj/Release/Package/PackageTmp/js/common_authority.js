/*
	功能：判断系统角色，给予对应操作权限
	当页面渲染完毕后，调用方法即可
 *	num传参：
        媒体管理：
            微信公众号：传1
            新浪微博：传2
            视频：传3
            直播：传4
            平台：传5
        刊例管理：
            微信公众号：传1
            新浪微博账号：传2
            视频账号：传3
            直播账号：传4
            APP资源：传5
 *	若要设置权限，需要添加对应的class
 *	媒体管理页面：
 *		添加：media_add_btn
 		创建刊例：media_create_btn
 		编辑：media_edit_btn 
 *	刊例管理页面：
 		查看刊例：pricelist_see
 		添加刊例：pricelist_add
 		编辑刊例：pricelist_edit
 		上架、下架和申请上架：pricelist_shelves
		审核刊例：pricelist_examine
		查看驳回原因：pricelise_reject
 *	项目管理页面：
 		查看：project_look_btn	
 		查看原因：project_lookReason_btn
 		编辑：project_edit_btn
 		删除：project_delete_btn
 		审核：project_examine_btn
 *	订单管理页面：
 		查看：order_look_btn
 		查看数据：order_lookData_btn
 		查看原因:order_lookReason_btn
 		删除:order_delete_btn
		执行:order_execute_btn
		取消订单:order_cancel_btn
		上传数据:order_uploadData_btn
		执行完毕:order_executeAll_btn
		订单确认:order_confirm_btn
*/
function judgeAuthority(num){
    if(num){
        /*媒体管理*/    
        //添加  
        if(CTLogin.BUTIDs.indexOf('SYS001BUT400'+num+'01') == -1){
            $('.media_add_btn').remove();
        }
        //编辑  
        if(CTLogin.BUTIDs.indexOf('SYS001BUT400'+num+'02') == -1){
            $('.media_edit_btn').remove();
        }
        //创建刊例  
        if(CTLogin.BUTIDs.indexOf('SYS001BUT400'+num+'03') == -1){
            $('.media_create_btn').remove();
        }

        /*刊例管理*/    
        //查看刊例
        if(CTLogin.BUTIDs.indexOf('SYS001BUT500'+num+'01') == -1){
            $('.pricelist_see').remove();
        }
        //添加刊例
        if(CTLogin.BUTIDs.indexOf('SYS001BUT500'+num+'02') == -1){
            $('.pricelist_add').remove();
        }
        //编辑刊例
        if(CTLogin.BUTIDs.indexOf('SYS001BUT500'+num+'03') == -1){
            $('.pricelist_edit').remove();
        }
        //上架、下架
        if(CTLogin.BUTIDs.indexOf('SYS001BUT500'+num+'04') == -1){
            $('.pricelist_shelves').remove();
        }
        //审核刊例
        if(CTLogin.BUTIDs.indexOf('SYS001BUT500'+num+'05') == -1){
            $('.pricelist_examine').remove();
        }
        //查看驳回原因
        if(CTLogin.BUTIDs.indexOf('SYS001BUT500'+num+'06') == -1){
            $('.pricelise_reject').remove();
        }
    }else{
        /*项目管理*/
        //查看
        if(CTLogin.BUTIDs.indexOf('SYS001BUT10001') == -1){
            $('.project_look_btn').remove();
        }
        //查看原因
        if(CTLogin.BUTIDs.indexOf('SYS001BUT10002') == -1){
            $('.project_lookReason_btn').remove();
        }
        //编辑
        if(CTLogin.BUTIDs.indexOf('SYS001BUT10003') == -1){
            $('.project_edit_btn').remove();
        }
        //删除
        if(CTLogin.BUTIDs.indexOf('SYS001BUT10004') == -1){
            $('.project_delete_btn').remove();
        }
        //审核
        if(CTLogin.BUTIDs.indexOf('SYS001BUT10005') == -1){
            $('.project_examine_btn').remove();
        }

        /*订单管理*/
        //查看
        if(CTLogin.BUTIDs.indexOf('SYS001BUT20001') == -1){
            $('.order_look_btn').remove();
        }
        //查看数据
        if(CTLogin.BUTIDs.indexOf('SYS001BUT20002') == -1){
            $('.order_lookData_btn').remove();
        }
        //查看原因
        if(CTLogin.BUTIDs.indexOf('SYS001BUT20003') == -1){
            $('.order_lookReason_btn').remove();
        }
        //删除
        if(CTLogin.BUTIDs.indexOf('SYS001BUT20004') == -1){
            $('.order_delete_btn').remove();
        }
        //执行
        if(CTLogin.BUTIDs.indexOf('SYS001BUT20005') == -1){
            $('.order_execute_btn').remove();
        }
        //取消订单
        if(CTLogin.BUTIDs.indexOf('SYS001BUT20006') == -1){
            $('.order_cancel_btn').remove();
        }
        //上传数据
        if(CTLogin.BUTIDs.indexOf('SYS001BUT20007') == -1){
            $('.order_uploadData_btn').remove();
        }
        //执行完毕
        if(CTLogin.BUTIDs.indexOf('SYS001BUT20008') == -1){
            $('.order_executeAll_btn').remove();
        }
        //订单确认
        if(CTLogin.BUTIDs.indexOf('SYS001BUT20009') == -1){
            $('.order_confirm_btn').remove();
        }
    }
}
