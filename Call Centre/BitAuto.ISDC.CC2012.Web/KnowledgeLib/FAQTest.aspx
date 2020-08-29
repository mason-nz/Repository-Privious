<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FAQTest.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.FAQTest" %>

<%@ Register Src="UCKnowledgeLib/UCFAQList.ascx" TagName="UCFAQList" TagPrefix="UC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加知识点</title>
    <link href="../css/base.css" type="text/css" rel="stylesheet" />
    <link href="../css/style.css" type="text/css" rel="stylesheet" />
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
    <div class="taskT">添加知识点</div>
    	<div class="addzs">
        	<ul>
            	<li><label>标题：</label><span><input name="" type="text"  class="w260"/></span>&nbsp;<span class="redColor">*</span></li>
                <li><label>所属分类：</label><span><select name="" class="w80"><option>公司简介</option><option>部门流程</option><option>风站功能</option><option>行业介绍</option><option>产品功能</option></select></span>
                <span><select name="" class="w160"><option>集团简介</option><option>一级智能部门</option><option>大区组织架构</option></select></span>&nbsp;<span class="redColor">*</span>
                </li>
                <li><label style=" vertical-align:top">内容：</label><span><textarea name="" cols="" rows=""></textarea></span></li>
                <li><label>附件：</label>
                    <span class=""><input type="button" name="" value="上 传"  class="btnattach"></span>
                    <span>每次最多上传10份文档，每份文档不超过5M; 支持类型 doc,ppt,xls,pps,pdf,txt</span> 
                </li> 
            </ul>           
            
            <UC:UCFAQList ID="UCFAQList" runat="server" />
            
            <div class="title bold">添加试题：<span><input name="" type="button" value="添加单选题" class="newBtn2 addbtn"/></span>&nbsp;
                <span><input name="" type="button" value="添加复选题" class="newBtn2 addbtn"/></span>&nbsp;<span><input name="" type="button" value="添加主观题" class="newBtn2 addbtn"/></span>&nbsp;
                <span><input name="" type="button" value="添加判断题" class="newBtn2 addbtn"/></span>
            </div>
            <!--添加单选题开始-->
            <ul>
            	<li class="xzt"><label>1、</label><span><input name="" type="text"  class="w600"/></span>&nbsp;
                <span><select name="" class="w100"><option>单选题</option><option>复选题</option><option>主观题</option><option>客观题</option><option>判断题</option></select></span>
                <span class="delete"><a href="#"></a></span>&nbsp;<span class="add"><a href="#"></a></span>
                	<ul>
                    	<li><input name="" type="radio" value="" /><em class="dx">A、</em><span><input name="" type="text"  class="w550"/></span></li>
                        <li><input name="" type="radio" value="" /><em class="dx">B、</em><span><input name="" type="text"  class="w550"/></span></li>
                        <li><input name="" type="radio" value="" /><em class="dx">C、</em><span><input name="" type="text"  class="w550"/></span></li>
                        <li><input name="" type="radio" value="" /><em class="dx">D、</em><span><input name="" type="text"  class="w550"/></span>
                        <span class="delete"><a href="#"></a></span>&nbsp;<span class="add"><a href="#"></a></span>
                        </li>
                    </ul>
                    <div class="title bold conrect">正确答案：<span><input name="" type="text"  class="w90"/></span></div>
                </li>
            </ul>
             <!--添加单选题结束-->
             <!--添加复选题开始-->
            <ul>
            	<li class="xzt"><label>1、</label><span><input name="" type="text"  class="w600"/></span>&nbsp;
                <span><select name="" class="w100"><option>复选题</option><option>单选题</option><option>主观题</option><option>客观题</option><option>判断题</option></select></span>
                <span class="delete"><a href="#"></a></span>&nbsp;<span class="add"><a href="#"></a></span>
                	<ul>
                    	<li><input name="" type="checkbox" value="" /><em class="dx">A、</em><span><input name="" type="text"  class="w550"/></span></li>
                        <li><input name="" type="checkbox" value="" /><em class="dx">B、</em><span><input name="" type="text"  class="w550"/></span></li>
                        <li><input name="" type="checkbox" value="" /><em class="dx">C、</em><span><input name="" type="text"  class="w550"/></span></li>
                        <li><input name="" type="checkbox" value="" /><em class="dx">D、</em><span><input name="" type="text"  class="w550"/></span>
                        <span class="delete"><a href="#"></a></span>&nbsp;<span class="add"><a href="#"></a></span>
                        </li>
                    </ul>
                    <div class="title bold conrect">正确答案：<span><input name="" type="text"  class="w90"/></span></div>
                </li>
            </ul>
             <!--添加复选题结束-->
             <!--添加复选题开始-->
            <ul>
            	<li class="xzt"><label>1、</label><span><input name="" type="text"  class="w600"/></span>&nbsp;
                <span><select name="" class="w100"><option>主观题</option><option>单选题</option><option>复选题</option><option>客观题</option><option>判断题</option></select></span>
                <span class="delete"><a href="#"></a></span>&nbsp;<span class="add"><a href="#"></a></span>
                    <div class="title bold conrect">正确答案：<span><textarea name="" cols="" rows=""  class="w500"></textarea></span></div>
                </li>
            </ul>
             <!--添加复选题结束-->
             <!--添加判断题开始-->
            <ul>
            	<li class="xzt"><label>1、</label><span><input name="" type="text"  class="w600"/></span>&nbsp;
                <span><select name="" class="w100"><option>判断题</option><option>单选题</option><option>复选题</option><option>客观题</option><option>主观题</option></select></span>
                <span class="delete"><a href="#"></a></span>&nbsp;<span class="add"><a href="#"></a></span>
                	<ul>
                    	<li><input name="" type="radio" value="" /><em class="dx">错</em></li>
                        <li><input name="" type="radio" value="" /><em class="dx">对</em></li>
                    </ul>
                    <div class="title bold conrect">正确答案：<span><input name="" type="text"  class="w90"/></span></div>
                </li>
            </ul>
             <!--添加判断题结束-->
             <div class="btn zsdbtn">
                    <input type="button" name="" value="保 存"  class="btnSave bold" onclick="save()">&nbsp;&nbsp;
                    <input type="button" name="" value="取 消"  class="btnSave bold">&nbsp;&nbsp;
                    <input type="button" name="" value="提 交" class="btnCannel bold">
   			</div>
        </div>
    </div>
    </form>
</body>
</html>
