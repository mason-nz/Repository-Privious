<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TagEdit.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.TagEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>编辑标签</title>
    <style type="text/css">
        .Order
        {
            margin-left: 5px;
            width: 20px;
        }
    </style>
    <script type="text/javascript">
    
    function delTag(obj){
       
        //删除分两类，
        //1未提交数据前,删除界面上新添加的文本框元素
        //2数据已交，需要实际从库里删除
        //是：未提交数据，否：已提交数据
        var isNewTag = false;
        var tagInput = $(obj).siblings("input[type='text']");
        var tagid = tagInput.attr("TagID");
        if(tagid == ""){        
            isNewTag = true;
        }
        
        
        if(isNewTag){
          //删除前先判断是否有标题“标签”
          if($(obj).parent().siblings("label").html()!="&nbsp;"){
            $(obj).parent().parent().next().children("label").html("标签：");
          }
          //未提交数据，直接删除元素即可
          $(obj).parent().parent().remove();
          
        }
        else{
            //判断是否已使用
            var pody = "";
            pody = { TagsIsUsedSelect:'yes',TagID:tagid };
            AjaxPost('/AjaxServers/TemplateManagement/TagEdit.ashx', pody, null, function(data){
                //alert("DATA="+ data);                
                if(data == "isUsed"){
                     $.jAlert("标签已被使用，不能删除!", function (r) { 
                            //alert("r="+ r);
                            //$.closePopupLayer('AuditTaskPopup', true);
                       });
                }
                else{
                    //删除前先判断是否有标题“标签”
                    if($(obj).parent().siblings("label").html()!="&nbsp;"){
                      $(obj).parent().parent().next().children("label").html("标签：");
                    }
                    $(obj).parent().parent().hide();
                    tagInput.attr("isDeleted","true");
                }
            });
            
        }
    }


    function delTag2(obj){
       
        //删除分两类，
        //1未提交数据前,删除界面上新添加的文本框元素
        //2数据已交，需要实际从库里删除
        //是：未提交数据，否：已提交数据
        var isNewTag = false;
        var tagInput = $(this).siblings("input[type='text']");
        var tagid = tagInput.attr("TagID");
        if(tagid == ""){        
            isNewTag = true;
        }
        
        
        if(isNewTag){
          //删除前先判断是否有标题“标签”
          alert("ok="+$(this).html());
          if($(this).parent().siblings("label").html()!="&nbsp;"){
            $(this).parent().parent().next().children("label").html("标签：");
          }
          //未提交数据，直接删除元素即可
          $(this).parent().parent().remove();
          
        }
        else{
            //判断是否已使用
            var pody = "";
            pody = { TagsIsUsedSelect:'yes',TagID:tagid };
            AjaxPost('/AjaxServers/TemplateManagement/TagEdit.ashx', pody, null, function(data){
                //alert("DATA="+ data);                
                if(data == "isUsed"){
                     $.jAlert("标签已被使用，不能删除!", function (r) { 
                            alert("r="+ r);
                            //$.closePopupLayer('AuditTaskPopup', true);
                       });
                }
                else{
                    alert("ok");
                    //删除前先判断是否有标题“标签”
                    if($(this).parent().siblings("label").html()!="&nbsp;"){
                      $(this).parent().parent().next().children("label").html("标签：");
                    }
                    alert("ok2="+$(this).html());
                    $(this).parent().parent().hide();
                    tagInput.attr("isDeleted","true");
                }
            });
            
        }
    }

    function AddTagRow(){
        var html = "";
        html = "<li class='item'><label>&nbsp;</label><span>" +
               "<input type='checkbox' checked='checked' value='0' class='fxk'/>"+
               "<input type='text' value='' TagID='' class='w250'/><input type='text' value='0' class='Order' />"+
               "<a href='javascript:void(0)' onclick='delTag(this)' ><img border='0' src='../Css/img/del.png' style='position:relative; top:5px; left:2px;'></a>"+
               "</span></li>";
                      
        $(this).parent().parent().before(html);
        
        //第一个li标签，要加标题“标签”
        if($("#tagEditUL>li:nth-child(2)>label").html()!="标签："){
            $("#tagEditUL>li:nth-child(2)>label").html("标签：");

            //去掉添加文本框前的标签
            $(this).parent().siblings("label").html("&nbsp;");
        };
        
        $(this).parent().parent().prev().find("span>input").eq(1).focus();
        //$("#tagEditUL>li>span>a").unbind("click",delTag);
        //$("#tagEditUL>li>span>a").bind("click",delTag);
    }

       
    function CreateULTag(i, n,ilength) {
        var html = "";
        if (i == 0) {//添加组名
            
            $("#tagEditUL>li").eq(0).find("span").eq(0).text(n.GroupName);
            $("#tagEditUL>li").eq(0).attr("BGID",n.BGID);
            if(n.TagName != ""){//该组有标签                
                if(n.IsUsed == "true"){//标签正常
                   html = "<li class='item'><label>&nbsp;</label><span>" +
                          "<input type='checkbox' checked='checked' value='0' class='fxk'/>"+
                          "<input type='text' value='"+ n.TagName +"' TagID='"+ n.TagID +"' class='w250'/><input type='text' value='"+ n.order +"' class='Order' />"+
                          "<a href='javascript:void(0)' onclick='delTag(this)'><img border='0' src='../Css/img/del.png' style='position:relative; top:5px; left:2px;'></a>"+
                          "</span></li>";
                }
                else{//标签停用                  
                   html = "<li class='item'><label>&nbsp;</label><span>" +
                          "<input type='checkbox' value='1' class='fxk'/>"+
                          "<input type='text' value='"+ n.TagName +"' TagID='"+ n.TagID +"' class='w250'/><input type='text'  value='"+ n.order +"'  class='Order' />"+
                          "<a href='javascript:void(0)' onclick='delTag(this)'><img border='0' src='../Css/img/del.png' style='position:relative; top:5px; left:2px;'></a>"+
                          "</span></li>";
                }
                $("#tagEditUL").append(html);
            }
            else{//该组无标签
                //为了格式好看，无标签情况，给加一行空的
                html = "<li class='item'><label>&nbsp;</label><span>" +
                          "<input type='checkbox' checked='checked' value='0' class='fxk'/>"+
                          "<input type='text' value='"+ n.TagName +"' TagID='"+ n.TagID +"' class='w250'/><input type='text'  value='"+ n.order +"'  class='Order' />"+
                          "<a href='javascript:void(0)' onclick='delTag(this)'><img border='0' src='../Css/img/del.png' style='position:relative; top:5px; left:2px;'></a>"+
                          "</span></li>";
                $("#tagEditUL").append(html);
            }

            
        }
        else {            
               if(n.IsUsed == "true"){//标签正常
                   html = "<li class='item'><label>&nbsp;</label><span>" +
                          "<input type='checkbox' checked='checked' value='0' class='fxk'/>"+
                          "<input type='text' value='"+ n.TagName +"' TagID='"+ n.TagID +"' class='w250'/><input type='text' value='"+ n.order +"'   class='Order' />"+
                          "<a href='javascript:void(0)' onclick='delTag(this)'><img border='0' src='../Css/img/del.png' style='position:relative; top:5px; left:2px;'></a>"+
                          "</span></li>";
                }
                else{//标签停用                  
                   html = "<li class='item'><label>&nbsp;</label><span>" +
                          "<input type='checkbox' value='1' class='fxk'/>"+
                          "<input type='text' value='"+ n.TagName +"' TagID='"+ n.TagID +"' class='w250'/><input type='text'  value='"+ n.order +"'  class='Order' />"+
                          "<a href='javascript:void(0)' onclick='delTag(this)'><img border='0' src='../Css/img/del.png' style='position:relative; top:5px; left:2px;'></a>"+
                          "</span></li>";
                }
                $("#tagEditUL").append(html);
        }

        if(i==(ilength-1)){
            html = "<li><label>&nbsp;</label><span>" +
                   "<input id='txtBoxAdd' type='text' style='margin-left:22px;cursor:pointer;' value='点击添加一条' class='w250' />"+
                   "</span></li>";  
            $("#tagEditUL").append(html);
            $("#tagEditUL>li:nth-child(2)>label").html("标签：");
            $("#txtBoxAdd").bind('click',AddTagRow);
        }
        
        //html = html.replace(/标签：/,"&nbsp;");
             
           
    }

        function LoadULData() {
        var bgid = <%=BGID%>;
        var pody = "";
        pody = { TagsSelectAll:'yes',BGID:bgid };
            //加载组下的标签
            AjaxPost('/AjaxServers/TemplateManagement/TagEdit.ashx', pody, null, success);
            function success(data) {
                var mbi = $.evalJSON(data);
                $.each(mbi, function (i, n) {                    
                    CreateULTag(i, n,mbi.length);
                });
                //$("#tagEditUL>li>span>a").unbind("click",delTag);
                //$("#tagEditUL>li>span>a").bind("click",delTag);
            }
        }

        function Submit() {
        $.openPopupLayer({
            name: "WaittingPoper",
            parameters: { FunctionName: "Save2DB" },
            url: "/AjaxServers/RequestWaittingPoper.aspx",
            beforeClose: function (e, data) {
                    if (e) {
                    }
                }
            });
        }

        function InsertTag2DB(){

           //返回值说明：
           //没有新增数据：0，新增成功：1，新增失败:2
            var tagList = new Array();
            //检查order是否为有效数字
            
            //遍历要新增的数据
            var TagCount = 0;
            var orderT = 0;
            $("#tagEditUL>li.item").each(function(i){
               if($(this).find("span>input[type='text']").attr("TagID")==""){
                  TagCount++;
                  var isUsed = "0";
                  if ($(this).find("span>input:checkbox").attr("checked") == true) {
                    isUsed = "0";
                  }
                  else{
                    isUsed = "1";
                  }
                   orderT = $(this).find("span>input.Order").val();
                   orderT = (orderT == null || orderT == "") ? 0 : orderT;
                  var tag ={
                      TagName: encodeURIComponent($(this).find("span>input[type='text']").val()),
                      Status: isUsed,
                      OrderNum:orderT
                  };

                  tagList.push(tag);
               }
            });

            if(TagCount>0){
            }
            else{
                return 0;
            }
            //alert("ok1");
            var str = JSON.stringify(tagList);
            //alert("ok2");
            var bgid = <%=BGID%>;
            var pody = "";
            pody = { InsertTag2DB:'yes',BGID:bgid,tagsJSON:str };
                //加载组下的标签
                AjaxPost('/AjaxServers/TemplateManagement/TagEdit.ashx', pody, null, success);
                function success(data) {
                    if(data == "操作成功"){
                          return 1;
                    }
                    else{
                          return 2;
                    }
                }
        }

        function DeleteTag2DB(){

           //返回值说明：
           //没有删除数据：0，删除成功：1，删除失败:2
            var tagList = new Array();

            //遍历要删除的数据
            var TagCount = 0;
            $("#tagEditUL>li").each(function(i){
               if($(this).find("span>input[type='text']").attr("isDeleted")=="true"){
                  TagCount++;
                  
                  var tag ={
                      TagID: $(this).find("span>input[type='text']").attr("TagID")
                  };

                  tagList.push(tag);
               }
            });

            if(TagCount>0){
            }
            else{
                return 0;
            }
            var str = JSON.stringify(tagList);
            var pody = "";
            pody = { DeleteTag2DB:'yes',tagsJSON:str };
                //加载组下的标签
                AjaxPost('/AjaxServers/TemplateManagement/TagEdit.ashx', pody, null, success);
                function success(data) {
                    if(data == "操作成功"){
                          return 1;
                    }
                    else{
                          return 2;
                    }
                }
        }

        function UpdateTag2DB(){
           //返回值说明：
           //没有更新数据：0，更新成功：1，更新失败:2
            var tagList = new Array();
             var orderT = 0;
            //遍历要更新的数据
            var TagCount = 0;
            $("#tagEditUL>li.item").each(function(i){
               if($(this).find("span>input[type='text']").attr("TagID")!=""){
                  TagCount++;
                  var isUsed = "0";
                  if ($(this).find("span>input:checkbox").attr("checked") == true) {
                    isUsed = "0";
                  }
                  else{
                    isUsed = "1";
                  }
                     orderT = $(this).find("span>input.Order").val();
                   orderT = (orderT == null || orderT == "") ? 0 : orderT;
                  var tag ={
                      TagID: $(this).find("span>input[type='text']").attr("TagID"),
                      TagName: encodeURIComponent($(this).find("span>input[type='text']").val()),
                      Status: isUsed,
                      OrderNum:orderT
                  };

                  tagList.push(tag);
               }
            });

            if(TagCount>0){
            }
            else{
                return 0;
            }

            var str = JSON.stringify(tagList);
            var bgid = <%=BGID%>;
            var pody = "";
            pody = { UpdateTag2DB:'yes',BGID:bgid,tagsJSON:str };
                //加载组下的标签
                AjaxPost('/AjaxServers/TemplateManagement/TagEdit.ashx', pody, null, success);
                function success(data) {
                    if(data == "操作成功"){
                          return 1;
                    }
                    else{
                          return 2;
                    }
                }
        }

        function Save2DB(){
            //验证数据
            var isValid = false;
            $("#tagEditUL>li").each(function(i){
               if($(this).find("span>input[type='text']").attr("TagID") != null){
                  var tag = "";
                  tag = $(this).find("span>input[type='text']").val();             
                  if(tag == "" || tag.length>16){                    
                    isValid = false;
                  }
                  else{
                    isValid = true;
                  }
               }
            });

            if(!isValid){
                $.jAlert("标签名称不能为空且不能超过16个字符，请修改！");
                $.closePopupLayer("WaittingPoper");
                return;
            }

            var tVal;
            $("#tagEditUL>li input.Order").each(function(i,dom) {
                tVal = $(dom).val();
                if (tVal==""||!isNum(tVal)) {
                    isValid = false;                    
                    return false;
                }
            });
         
            if(!isValid){
                $.jAlert("排序号最多为4位有效数字,且不能为空！");
                $.closePopupLayer("WaittingPoper");
                return;
            }
               
            var retval=0;
            retval = InsertTag2DB();
            if(retval == 1){
                 isValid = true;
            }
            else if(retval == 2){
                $.jAlert('新增标签失败！', function () {
                   $.closePopupLayer('updateTagGroup',false);
                   $.closePopupLayer("WaittingPoper");
                 });
                 return;
            }

            retval = UpdateTag2DB();
            if(retval == 1){
                 isValid = true;
            }
            else if(retval == 2){
                $.jAlert('更新标签失败！', function () {
                   $.closePopupLayer('updateTagGroup',false);
                   $.closePopupLayer("WaittingPoper");
                 });
                 return;
            }

            retval = DeleteTag2DB();
            if(retval == 1){
                 isValid = true;
            }
            else if(retval == 2){
                $.jAlert('删除标签失败！', function () {
                   $.closePopupLayer('updateTagGroup',false);
                   $.closePopupLayer("WaittingPoper");
                 });
                 return;
            }

            if(isValid){
                $.jAlert('保存成功！', function () {
                   $.closePopupLayer('updateTagGroup',false);
                   $.closePopupLayer("WaittingPoper");
                   LoadData();//更新标签管理页面
                 });
            }
        }

        $(document).ready(function () {
            LoadULData();
            //LoadData();
        });

        //验证是否为数字
function isNum(s) {
    if (s.toString().length > 4) return false;
    var pattern = /^[0-9]*$/;
    if (pattern.test(s)) {
        return true;
    }
    return false;
}
    </script>
</head>
<body>
    <div class="pop pb15 editbq">
        <div class="title bold">
            编辑标签<span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('updateTagGroup',false);">
            </a></span>
        </div>
        <ul id="tagEditUL" class="clearfix ">
            <li>
                <label>
                    所属分组：</label><span>客户回访组</span></li>
            <%--<li><label>标签：</label><span><input checked="checked" type="checkbox" class="fxk" value="" /><input type="text" class="w250" value="" /><a href="#"><img border="0" style="position:relative; top:5px; left:2px;" src="../Css/img/del.png"></a></span></li>
        <li><label>&nbsp;</label><span><input type="checkbox" class="fxk" value="" /><input type="text" class="w250" value="" /><a href="#"><img border="0" style="position:relative; top:5px; left:2px;" src="../Css/img/del.png"></a></span></li>
        <li><label>&nbsp;</label><span><input id="txtBoxAdd" type="text" style="margin-left:22px;" value="点击添加一条" class="w250" /></span></li>  --%>
        </ul>
        <div class="clearfix">
        </div>
        <div class="btn">
            <input type="button" onclick="Submit()" class="btnSave bold" value="保 存" name="" />
            <input type="button" class="btnCannel bold" onclick="javascript:$.closePopupLayer('updateTagGroup',false);"
                value="取 消" name="" /></div>
    </div>
</body>
</html>
