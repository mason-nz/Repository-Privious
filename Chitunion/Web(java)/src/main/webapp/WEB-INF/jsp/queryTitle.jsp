<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
     <%@ page import="org.apache.jasper.tagplugins.jstl.core.Import"%>
<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/functions" prefix="fn"  %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head > 
<meta charset="utf-8">
<meta name="description" content="描述文字，字数200内？">
<meta name="keywords" content="网站相关关键字，字数200内？">
<title >赤兔联盟平台</title>
<link rel="icon" href="${pageContext.request.contextPath }/images/favicon.ico" type="image/x-icon">
</head>
<body>
<li id="come"/>
<!--顶部logo 导航-->
<jsp:include page="header.jsp" />
<script type="text/javascript"
	src="/js/jquery.pagination.js"></script>
<div class="list_main">
<!--中间内容-->
<div class="order">
<jsp:include page="Menu.jsp"></jsp:include>
    <div class="order_r">

            <ul class="state">
                <li>类型：
                    <span><input name="queryTypeId"  type="radio" value="0"  checked="checked" />品牌</span>
                    <span><input name="queryTypeId" type="radio" value="1"  />媒体</span>
                </li>
                <li>标签名称：
                    <input name="titleName" type="text"  style="width:200px;">
                </li>
                <li><a href="javascript:query();" class="but_query" style="width:70px;">查询</a></li>
                <div class="clear"></div>
            </ul>



        <div class="table">
            <table id="resultList" width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#ddd" >
                <tr id="headtr">
                    <th id="tableHead">
                    </th>
                </tr>
               
            </table>
				<div class="no_data"></div>
            <!--分页-->
             <div class="green-black" id="pageContainer"></div>
        </div>
    </div>
    <div class="clear"></div>
</div>

</div>


<!--底部-->
<jsp:include page="footer.jsp"></jsp:include>
<script type="text/javascript">
	function page(count, pageRows, url, queryByTitleNamePojo) {
		$("#resultList").show();
		var counts = count;
		//分页----begin----
		$("#pageContainer").pagination(
		                        counts,
		                            {
		                                items_per_page: pageRows, //每页显示多少条记录（默认为20条）
		                                callback:  function (curPage,jg) {
		                                	 var queryByTitleNamePojo = {};
			                      				var selectR = $('input:radio:checked').val();
			                      				var titleName = $("input[name='titleName']").val();
			                      				queryByTitleNamePojo.curPage = curPage;
			                      				queryByTitleNamePojo.queryTypeId = selectR;
			                      				queryByTitleNamePojo.titleName = titleName;
		                    				$.ajax({
		                    					url : "/title/" + url,
		                    					type : "post",
		                    					dataType : "json",
		                    					data : JSON.stringify(queryByTitleNamePojo),
		                    					success : function(data) {
		                    						$(".no_data").html("");
		                    						initTitleList(data);
		                    					},
		                    					contentType : 'application/json'
		                    	
		                    				});
		                    			/* 	callback(true); */
		                            } 
		             });
         if(counts==0){
				$(".no_data").html("<img src='${pageContext.request.contextPath }/images/no_data.png'>");
				$("#pageContainer").html("");
				$("#resultList").hide();
				$("#resultList").html("<tr id='headtr'></tr>");
		//分页----end----
		}
	};
	function initTitleList(data) {
		$(".tr").remove();
		var brandResult = data.resultTitleName;
		var mediaResult = data.mediaQueryResult;
		if(null!=brandResult){
			for ( var i in brandResult) {
				$('#headtr').empty();
				$('#headtr').html("<th id='tableHead'>品牌</th>");
				$("#resultList").append(
								"<tr class='tr'>"+
									"<td>"+brandResult[i]+"</td>"
								+"</tr>");
			}
		}
		if(null!=mediaResult){
			for(var i in mediaResult){
				$('#headtr').empty();
				$('#headtr').append("<th id='type'>媒体</th><th>类型</th>");
				$("#resultList").append("<tr class='tr'><td><div class='query_media'><div class='portrait'><img src='"+mediaResult[i].headIconURL+"' /></div><div class='public'><h2>"+mediaResult[i].mediaName+"</h2><p>"+mediaResult[i].platNumber+"</p></div></div></td><td>"+mediaResult[i].mediaTypeName+"</td></tr>");
			}
		}
		window.location.href ="#come";  
	};
	
	function query(){
		var queryByTitleNamePojo = {};
		var selectR = $('input:radio:checked').val();
		var titleName = $("input[name='titleName']").val();
		//window.location.href = "${pageContext.request.contextPath}/title/queryByTypeAndTitleName?queryType="+v+"&titleName="+titleName;
		
		queryByTitleNamePojo.curPage = 1;
		queryByTitleNamePojo.queryTypeId = selectR;
		queryByTitleNamePojo.titleName = titleName;
		
		
		$.ajax({
			url : "/title/queryByTypeAndTitleName",
			type : "post",
			dataType : "json",
			data : JSON.stringify(queryByTitleNamePojo),
			success : function(data) {
//					console.info(data);
				//var datas = [];
				if(data!=null){
					titleNames = data.resultTitleName;
					mediaResult = data.mediaQueryResult;
					if (titleNames != null || mediaResult!=null) {
						count = data.count;
						pageRows = data.pageRows;
					} else {
						count = 0;
						pageRows = 5;
					}
					page(count, pageRows, 'queryByTypeAndTitleName', queryByTitleNamePojo);
					initTitleList(data);
				}
			},
			contentType : 'application/json'
		});
	}
	$(function(){
		$("#resultList").hide();
	});
	


</script>


</body>
</html>