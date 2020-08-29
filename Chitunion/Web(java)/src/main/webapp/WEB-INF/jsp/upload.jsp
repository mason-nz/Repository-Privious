<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<title>Insert title here</title>
<script type="text/javascript"
	src="${pageContext.request.contextPath }/js/jquery.1.11.3.min.js"></script>
<script type="text/javascript">
	$(function(){
		var token = Math.random();
		$('#token').val(token);
	});
</script>
</head>
<body>
	<form method="post"  enctype="multipart/form-data" action="${pageContext.request.contextPath}/uploadXls/importToDatabase" >
		<input name="titleFile" type="file" />
		<input id="token" name="importTitleToken" type="hidden" />
		<input type="submit" id="button-import" value="提交">
	</form>
</body>
</html>