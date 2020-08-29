<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCFAQList.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.KnowledgeLib.UCKnowledgeLib.UCFAQList" %>

<div  class="title bold"><a name='faq' ></a>添加FAQ：<span class="add"><a href="javascript:void(0);" onclick="FAQ_Add()"></a></span> <input type="text" name="FAQ_DelIDs" id="FAQ_DelIDs" style=" display:none;"/></div>
<div id="FAQ_Con">
    <asp:Repeater runat="server" ID="Rt_FAQList">
    <ItemTemplate>
    <ul>
        <li style=" height:65px;">
            <label style=" vertical-align:top">Q、</label>
            <span><textarea name="FAQ_Q" cols="" rows="" class="w700"><%#Eval("Question")%></textarea></span>
            <span class='addst'><a href="javascript:void(0);" class="delete" onclick="FAQ_Remove(this)"></a>
            <a href="javascript:void(0);" class="add" onclick="FAQ_Add()"></a></span>
        </li>
        <li class="answer">
            <label style=" vertical-align:top">A、</label>
            <span><textarea name="FAQ_A" cols="" rows="" class="w700"><%#Eval("Ask")%></textarea></span>
            <!--隐藏域-->
            <input type="hidden" name="hidden_FAQID" value='<%#Eval("KLFAQID")%>' />
        </li> 
    </ul>
    </ItemTemplate>
    </asp:Repeater>    
</div>
<ul style=" display:none;" id="FAQ_Template">
    <li style=" height:65px;">
        <label style=" vertical-align:top">Q、</label>
        <span><textarea name="FAQ_Q" cols="" rows="" class="w700"></textarea></span>
        <span class='addst'><a href="javascript:void(0);" class="add" onclick="FAQ_Add()"></a><a href="javascript:void(0);" class="delete" onclick="FAQ_Remove(this)"></a></span>
    </li>
    <li class="answer">
        <label style=" vertical-align:top">A、</label>
        <span><textarea name="FAQ_A" cols="" rows="" class="w700"></textarea></span>

        <!--隐藏域-->
        <input type="hidden" name="hidden_FAQID" value="0" />
    </li> 
</ul>
<script type="text/javascript" language="javascript">
    function FAQ_Add() {
        var FAQ_Temp = "<ul>" + $('#FAQ_Template').html() + "</ul>";
        $('#FAQ_Con').append(FAQ_Temp);
    }
    function FAQ_Remove(oThis) {
        if($('#FAQ_Con ul').length>1){
            $.jConfirm('是否确定删除FAQ？', function (r) {            
                if(r){
                              
                        var thisObject = $(oThis).parents('ul');
                        var FAQ_ID = $.trim(thisObject.find(':input[name=hidden_FAQID]').val());
                        if (FAQ_ID != '0') {
                            $(':input[name=FAQ_DelIDs]').val($(':input[name=FAQ_DelIDs]').val() + FAQ_ID + ',');
                        }
                        thisObject.remove();
                
                }
            });
        }
        else
        {
            $.jAlert("最后一个FAQ不可删除！");
        }
    }

    function GetAllFAQ() {
        var FAQArray = new Array();
        $('#FAQ_Con ul').each(function () {
            var FAQID = $.trim($(this).find(':input[name=hidden_FAQID]').val());
            var FAQ_Q = $.trim($(this).find('[name=FAQ_Q]').val());
            var FAQ_A = $.trim($(this).find('[name=FAQ_A]').val());
            if (FAQ_Q != '' & FAQ_A != '') {
                //写入对象                
                var FAQModel = {
                        faqID: escapeStr(FAQID),
                        faq_Q: escapeStr(FAQ_Q),
                        faq_A: escapeStr(FAQ_A)
                    };
                FAQArray.push(FAQModel);               
            }
        });
        return FAQArray;
    }

    function CheckFAQ() {
        var msg = "";
        var FAQArray = new Array();
        $('#FAQ_Con ul').each(function () {
            var FAQ_Q = $.trim($(this).find('[name=FAQ_Q]').val());
            var FAQ_A = $.trim($(this).find('[name=FAQ_A]').val());
            //至少有一项部位空，否则不能提交
            if (FAQ_Q != '' || FAQ_A != '') {
                if (FAQ_Q == '' || FAQ_A == '') {
                    msg = "有一条FAQ的问题或者答案未填写！";
                }
            }
        });        
        return msg;
    }
    <% if (IsStart())
       { %>
        $(document).ready(function () {
            FAQ_Add();
        });
    <%}
       else { }%>
</script>
  


