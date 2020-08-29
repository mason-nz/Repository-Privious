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
		                <a href="/title/getAllBrandWithOutTitleList" ><li class="selected">未打标签</li></a>
		                <a href="/title/carBrandWithTitleList" ><li>已打标签</li></a>
		            </ul>
		</div>
        <div class="mt20">
            <ul class="state">
                <li>品牌信息：
                    <select id="allbrandsNoTitle" style="width:120px;">
                        <option value="0">品牌</option>
                    <c:forEach items="${brands }" var="brand" >  
                    	<option   <c:if test="${param.BrandID==brand.brandID }">selected</c:if>  value="${brand.brandID }"   >${brand.showName }</option>
                    </c:forEach>
                    </select>
                </li>
                <li><a href="javascript:sortByBrandId()" class="but_query" style="width:70px;">查询</a></li>
                <div class="clear"></div>
            </ul>

        </div>
        <div class="clear"></div>
	<script type="text/javascript">
		/*  $(function(){
			var url = "/title/getAllJustBrandNoTitles";
			$.post(url,  function(data){
						var brands = data.brands;
						var brandID = ${brandID}; //js中用el表达式取值，其值不能为null
					     for(var i = 0;i<brands.length;i++){
					    	 $("#allbrandsNoTitle").append("<option value="+brands[i].brandID+">"+brands[i].showName+"</option>");
					     }
					     if(brandID!=null){
					     $("#allbrandsNoTitle option[value="+brandID+"]").attr("selected","selected");
					     }
					   }, "json");
		}); */
		function sortByBrandId(){
			var brandId = $("#allbrandsNoTitle").val();
			window.location.href = "/title/getAllBrandWithOutTitleList?BrandID="+brandId;
		};
	</script>
        <div class="table">
            <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#ddd" >
                <tr>
                    <th width="50%">品牌信息</th>
                     <c:if test="${authoMap.SYS001BUT6000201 ==true }">
                    <th width="50%">操作</th>
                    </c:if>
                </tr>
            
               <c:forEach items="${pagination.list }" var="CarBrand">
                <tr>
                    <td>${CarBrand.showName }</td>
               <c:if test="${authoMap.SYS001BUT6000201 ==true }">
                    <td><a href="${pageContext.request.contextPath}/title/setTitleForBrand/${CarBrand.brandID}/${CarBrand.serialID==null?-1:CarBrand.serialID}/0">设置标签</a></td>
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


</body>
</html>