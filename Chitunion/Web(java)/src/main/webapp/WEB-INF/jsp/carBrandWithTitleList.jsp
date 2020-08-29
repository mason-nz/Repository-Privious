<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<%@ page import="org.apache.jasper.tagplugins.jstl.core.Import"%>
<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/functions" prefix="fn"  %>
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
        <div id="tab_list">
		            <ul class="tab_menu" style="border-bottom: 1px solid #EDEDED;margin-bottom:20px">
		                <a href="/title/getAllBrandWithOutTitleList" ><li>未打标签</li></a>
		                <a href="/title/carBrandWithTitleList" ><li class="selected">已打标签</li></a>
		            </ul>
		</div>
        <div class="mt20">
        <form id="queryForm" action="${pageContext.request.contextPath}/title/carBrandWithTitleList" method="post">
            <ul class="state">
                <li onload>品牌信息：
                    <select id="brandSelect" name="BrandID" style="width:85px">
                        <option value="0">品牌</option>
                        <c:forEach items="${brands }" var="brand">
                        	<option <c:if test="${param.BrandID  == brand.brandID}">selected</c:if> value="${brand.brandID }">${brand.showName }</option>
                        </c:forEach>
                    </select>
                </li>
                <li>标签：<input name="titleName" type="text" value="${param.titleName }"  style="width:85px;"></li>
                <li>操作人：<input name="createUserName" value="${param.createUserName }" type="text"  style="width:85px;"></li>
                <li>操作时间：<input id="lastUpdateTime" value="${param.lastUpdateTime }" name="lastUpdateTime" type="text"  style="width:90px;" class="laydate-icon" onClick="laydate({fixed:false,elem:'#lastUpdateTime'})"> 至 <input id="createTime1" name="createTime1" value="${param.createTime1 }" type="text"  style="width:90px;" class="laydate-icon" onClick="laydate({fixed:false,elem:'#createTime1'})"></li>
                <li><a href="javascript:QuerySubmit();" class="but_query" style="width:70px;">查询</a></li>
                <div class="clear"></div>
            </ul>
             </form>
        </div>
        <div class="clear"></div>

        <div class="table">
            <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#ddd" >
                <tr>
                    <th width="20%">品牌信息</th>
                    <th width="30%">标签</th>
                    <th width="15%">操作人</th>
                    <th width="20%">操作时间</th>
                    <c:if test="${authoMap.SYS001BUT6000202==true }">
                    <th width="15%">操作</th>
                    </c:if>
                </tr>
                <c:forEach items="${pagination.list }" var="CarBrand">
				 		<tr>
	                    <td>${CarBrand.showName }</td>
	                    <td>${CarBrand.titles }</td>
	                    <td>${CarBrand.createUserName }</td>
	                    <td>${CarBrand.showTime }</td>
	                    <c:if test="${authoMap.SYS001BUT6000202==true }">
	                    <td><a href="${pageContext.request.contextPath}/title/setTitleForBrand/${CarBrand.brandID}/${CarBrand.serialID==null?-1:CarBrand.serialID}/1">标签维护</a></td>
               			</c:if>
               			</tr>
				 </c:forEach>
            </table>
			<c:choose>
			<c:when test="${fn:length(pagination.list)==0 }">
				<div class="no_data"><img src='${pageContext.request.contextPath }/images/no_data.png'></div>
			</c:when>
			</c:choose>

            <!--分页-->
            <c:choose>
            <c:when test="${fn:length(pagination.list)!=0 }">
            <div class="green-black">
                 <c:forEach items="${pagination.pageView }" var="page">
					${page }
				</c:forEach>
            </div>
			</c:when>
			</c:choose>
        </div>
    </div>
    <div class="clear"></div>
</div>

</div>


<!--底部-->
<jsp:include page="footer.jsp"></jsp:include>
<script type="text/javascript">
		/* var url = "/title/getAllBrandWithTitle";
		$.post(url,function(data){
			 var brands = data.brands;
			for(var i = 0;i<brands.length;i++){
				var brandId = brands[i].brandID;
				if('${param.BrandID}'==brandId){
				$('#brandSelect').append('<option selected value="'+brands[i].brandID+'">'+brands[i].showName+'</option>');
					
				}else{
				$('#brandSelect').append('<option value="'+brands[i].brandID+'">'+brands[i].showName+'</option>');
				}
			}
		},"json"); */
		function QuerySubmit(){
			$("#queryForm").submit();
		}
		
	</script> 
</body>
</html>