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
<script type="text/javascript">
	function queryByMediaType(){
		var mediaTypeId=$("#mediaTypeSelect").find("option:selected").val(); //获取Select选择的Text 
		var titleName = $("input[name='titleName']").val();
		var username = $("input[name='username']").val();
		var createTime = $("input[name='createTime']").val();
		var createTime1 = $("input[name='createTime1']").val();
		window.location.href = "${pageContext.request.contextPath}/title/mediaWithTitleList?mediaTypeId="+mediaTypeId+"&createTime="+createTime+"&username="+username+"&titleName="+titleName+"&createTime1="+createTime1; 
	}
		
</script>
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
		                <a href="/title/mediaNoTitleList" ><li>未打标签</li></a>
		                <a href="/title/mediaWithTitleList" ><li class="selected">已打标签</li></a>
		            </ul>
		</div>
        <div class="mt20">
            <ul class="state">
                <li>媒体类型：
                    <select id="mediaTypeSelect" style="width:85px">
						<option value="14001"  <c:if test="${mediaTypeId==14001 }">selected</c:if> >微信</option>
						<option value="14002" <c:if test="${mediaTypeId==14002 }">selected</c:if> >APP</option>
						<option value="14003" <c:if test="${mediaTypeId==14003 }">selected</c:if> >微博</option>
						<option value="14004" <c:if test="${mediaTypeId==14004 }">selected</c:if> >视频</option>
						<option value="14005" <c:if test="${mediaTypeId==14005 }">selected</c:if> >直播</option>
                    </select>
                </li>
                <li>标签：<input name="titleName" value="${param.titleName }" type="text"  style="width:85px;"></li>
                <li>操作人：<input name="username" value="${param.username }" type="text"  style="width:85px;"></li>
               <li>操作时间：<input id="createTime" value="${param.createTime }" name="createTime" type="text"  style="width:90px;" class="laydate-icon" onClick="laydate({fixed:false,elem:'#createTime'})"> 至 <input id="createTime1" value="${param.createTime1 }" name="createTime1" type="text"  style="width:90px;" class="laydate-icon" onClick="laydate({fixed:false,elem:'#createTime1'})"></li>
                <li><a href="javascript:queryByMediaType();" class="but_query" style="width:70px;">查询</a></li>
                <div class="clear"></div>
            </ul>
        </div>
        <div class="clear"></div>

        <div class="table">
            <table width="100%" border="0" cellspacing="1" cellpadding="0" bgcolor="#ddd" >
                <tr>
                    <th width="33%">媒体</th>
                    <th width="23%">标签</th>
                    <th width="15%">操作人</th>
                    <th width="20%">操作时间</th>
                  <c:if test="${authoMap.SYS001BUT6000102 ==true }">
                    <th width="10%">操作</th>
                  </c:if>
                </tr>
                 <c:forEach items="${pagination.list }" var="media">
                <tr>
                    <td>
                        <div class="portrait"><img src="${media.headIconURL }"/></div>
                        <div class="public">
                            <h2>${media.mediaName }</h2>
                            <p>${media.platNumber }</p>
                        </div>
                        <div class="clear"></div>
                    </td>
                    <td>${media.titles }</td>
                    <td>${media.createUserName }</td>
                    <td>${media.showTime }</td>
                  <c:if test="${authoMap.SYS001BUT6000102 ==true }">
                   <td><a href="/title/setTitleForMedia/${media.mediaTypeId}/${media.mediaId}/1">标签维护</a></td>
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
