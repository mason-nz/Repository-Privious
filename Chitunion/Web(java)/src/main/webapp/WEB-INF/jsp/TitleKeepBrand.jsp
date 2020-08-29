<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
    <%@page import="org.apache.jasper.tagplugins.jstl.core.Import"%>
	<%@ taglib uri="http://java.sun.com/jsp/jstl/functions" prefix="fn"  %>
	<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<meta charset="utf-8">
<meta name="description" content="描述文字，字数200内？">
<meta name="keywords" content="网站相关关键字，字数200内？">
<title>赤兔联盟平台</title>
<link rel="icon" href="${pageContext.request.contextPath }/images/favicon.ico" type="image/x-icon">
</head>
<body>
<!--顶部logo 导航-->
<jsp:include page="header.jsp"></jsp:include>
<div class="list_main">
<!--中间内容-->
<div class="order">
<jsp:include page="Menu.jsp"></jsp:include>
    <div class="order_r">
        <div class="mb15 f16">${aboutName }</div>
        <div class="tenance">
        <!-- class="st_tree" -->
            <div style="position:absolute; height:400px; overflow:auto" class="tenance_l">
                <div >
                    <ul>
                         <ul id="titleTree" class="ztree"></ul>
                    </ul>
                </div>
            </div>
            <div class="tenance_r">
                <h2>已选标签</h2>
                <div class="line2"></div>
                <ul id="selectedUl">
                   <!--  <li>宋小宝 <a href="#" class="blue3">删除</a></li>
                    <li>可爱 <a href="#" class="blue3">删除</a></li>  -->
                    <div class="clear"></div>
                </ul>
            </div>
            <div class="clear"></div>
            <div class="mt20">找不到标签？ <a href="#" class="red">请联系策划添加</a></div>
            <div><a href="javascript:#" onclick="submit()" class="button">提交</a></div>
        </div>
    </div>
    <div class="clear"></div>
</div>
</div>
<!--底部-->
<jsp:include page="footer.jsp"></jsp:include>
 <script type="text/javascript">
    var selectedId = "";
    var setOrUpdate = ${setOrUpdate};
    function submit(){
    	if(selectedId!=""){
    	var params = {"selectedId":selectedId};
    	var url = "/title/updateCarBrandTitle";
    	$.post(url,params,function(data){
    		if(data.status=='ok'){
    			if(data.setOrUpdate==0){
    				window.location.href = "${pageContext.request.contextPath}/title/getAllBrandWithOutTitleList";
    			}
    			if(data.setOrUpdate==1){
    				window.location.href = "${pageContext.request.contextPath}/title/carBrandWithTitleList";
    			}
    		}
    	},"json");
    	
	    }else{
	    	if(setOrUpdate == 0){
				alert("请至少设置一个标签");
	    	}
	    	if(setOrUpdate == 1){
	    		if(confirm("确定清空标签吗？")){
	    			var params = {"selectedId":selectedId};
	    	    	var url = "/title/updateCarBrandTitle";
	    	    	$.post(url,params,function(data){
	    	    		if(data.status=='ok'){
	    	    			if(data.setOrUpdate==0){
	    	    				window.location.href = "${pageContext.request.contextPath}/title/getAllBrandWithOutTitleList";
	    	    			}
	    	    			if(data.setOrUpdate==1){
	    	    				window.location.href = "${pageContext.request.contextPath}/title/carBrandWithTitleList";
	    	    			}
	    	    		}
	    	    	},"json"); 
	    		}
	    	}
		}
    }
	function removeTitleOfMedia(titleId){
		selectedId = selectedId.replace(titleId+"-", "");
		$("#n"+titleId).remove();
	};
	  function mouseOver(nodeId){
		  $('#ul'+nodeId).empty();
	    	var url = "/title/getRelationByTitleId";
	    	var params = {"TitleId":nodeId};
	    	$.post(url,params,function(data){
	    		/* alert(JSON.stringify(data)); */
	    		for(var i = 0;i<data.length;i++){
	    		$('#ul'+nodeId).append('<li>'+data[i].showStr+'</li></br>');
	    		} 
	    	},"json"); 
	    }
	    function mouseOut(nodeId){
	    	$('#ul'+nodeId).empty();
	    }
	$(function(){
		var url = "/title/getTitleByBrandIdAndSerialID";
		$.post(url,function(data){
				var arr = data.titles;
				/* alert(JSON.stringify(arr)); */
				for(var i = 0;i<arr.length;i++){
					var titleId = arr[i].id;
					selectedId += titleId+"-"
					 $('#selectedUl').append("<li id=n"+titleId+" class='title_relation'><div id=divL"+titleId+"><span onmouseleave='mouseOut("+titleId+")' onmouseenter='mouseOver("+titleId+")' >"+arr[i].name+"</span>  <a href='javascript:removeTitleOfMedia("+titleId+");' class='blue3'>删除</a></br><ul id=ul"+titleId+"  class='title_name'></ul></div></li>");
				}
		},"json"); 
    var setting = {
           data : {
                  key : {
                        title : "t"
                  },
                  simpleData : {
                        enable : true
                  }
           },
           check: {
        	   chkboxType :{ "Y" : "", "N" : "" }, //Y:勾选（参数：p:影响父节点），N：不勾（参数s：影响子节点）[p 和 s 为参数]
           },
           callback: {
       		onClick: zTreeOnClick
       	   },
       	view: {
    		showIcon: false,
    		showLine: false,
    		showTitle: false,
    		fontCss : {'font-size':'14px'}
    	}
    };
    var treeObject;
    function zTreeOnClick(event, treeId, treeNode) {
    	treeObject.expandNode(treeNode, true, false, true);
    	var pId = treeNode.pId;
    	var nodeId = treeNode.id;
    	var type = treeNode.type; //0，1，2分别为母ip,子ip,普通标签
    	var rp = treeNode.rp;
    	if(type==1){
			nodeId = nodeId/rp;
		}
    	if(pId!=null){   /* 父节点id为null说明是根节点，不为null则为一级后的节点 */
    	   	if(selectedId.indexOf(nodeId)==-1){
    	   		$('#selectedUl').append("<li id=n"+nodeId+" class='title_relation'><div id=divL"+nodeId+"><span onmouseleave='mouseOut("+nodeId+")' onmouseenter='mouseOver("+nodeId+")' >"+treeNode.name+"</span>  <a href='javascript:removeTitleOfMedia("+nodeId+");' class='blue3'>删除</a></br><ul id=ul"+nodeId+"  class='title_name'></ul></div></li>");
				selectedId +=nodeId+"-"
    	   	}
    	}
    };
    $.ajax({
           url : '${pageContext.request.contextPath}/title/allTitle',
           type : 'POST',
           /* dataType : 'text', */
           dataType:'json',
           success : function(data) {
                 treeObject =  $.fn.zTree.init($("#titleTree"), setting, data);
           },
           error : function(msg) {
                  alert('树加载异常!');
           }
    });
});

</script>
</body>
</html>
 