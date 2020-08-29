
//跳题层相关
$(document).ready(function () {

    //点击选项，打开层
    $(".tipSpan").live('click', function () { showPanel(this); });

    //关闭层
    $("#closePanel").click(function () { panel.style.display = "none"; });

    //单击层中的试题时
    $("#PanelquestList li a").live('click', function (e) { e.preventDefault(); SelectQuestItemInPanel(this) });

    //取消跳题
    $("#cancelSelectQuestion").click(CancelSelect);

});


function showPanel(obj) {
    //显示层
    panel.style.display = "block";

    //定位层
    $("#panel").offset(
        {
            top: $(obj).offset().top - 15,
            left: $(obj).offset().left + $(obj).width()
        });

    //绑定层中试题
    var linkid = $(obj).attr("linkid");
    var sqid = $(obj).attr("sqid");
    GetAllPanelList(sqid, linkid); //生成所有题

    //显示按钮
    if (linkid != "0") {
        $("#cancelSelectQuestion").show();
    }
    else {
        $("#cancelSelectQuestion").hide();
    }

    $("#panel").data("tagObj", obj);
}


//    生成所有题 sqid：不用显示的问题ID。 linkid:已选择的问题
function GetAllPanelList(sqid, linkid) {

    var html = "";
    $("#divQuestionList .qtitle").each(function (i, v) {
        var no = $($(this).find("span[name='no']")[0]).text();
        var QuestionLinkId = $(this).attr("QuestionLinkId");

        if ($(this).attr("sqid") != sqid) {
            html += "<li ";
            if (no == linkid) {
                html += " class='selectLi'";
            }
            html += "><a href='#' QuestionLinkId='" + QuestionLinkId + "' linkid='" + no + "' sqid='" + $(this).attr("sqid") + "'>" + $(this).text() + "</a></li>";
        }
    });
    $("#PanelquestList").html(html);
}


//选择一个题 obj,单击的项
function SelectQuestItemInPanel(obj) {
    var selSqid = $(obj).attr("sqid");
    var QuestionLinkId = $(obj).attr("QuestionLinkId");

    var o = $("#panel").data("tagObj");
    $(o).html("跳转到第<span class='spanlinkIndexText'>" + QuestionLinkId + "</span>题");
    $(o).attr("linkid", QuestionLinkId);

    panel.style.display = "none";

    EditJsonStr(o);
}

//取消跳题
function CancelSelect() {

    var o = $("#panel").data("tagObj");
    $(o).html("无跳题");
    $(o).attr("linkid", "0");

    panel.style.display = "none";

    EditJsonStr(o);
}
 
//编辑问题JSON,修改跳题ID,obj 无跳题Span
function EditJsonStr(obj) {

    //取得当前是第几个选项
    var li = $(obj).parent();
    var lis = $(obj).parent().parent().find("li");
    var index = $(lis).index(li); //当前是第几个选项

    //获取JSON对象
    var QuestItem = $(obj).parents('div.questitem');
    var jsonStr = $($(QuestItem).find("input[name='JsonStr']")[0]).val();
    jsonStr = unescape(jsonStr);
    var objdata = $.evalJSON(jsonStr); //现有的问题JSON对象

    //修改跳题ID
    var linkId = $(obj).attr("linkid");
    $(objdata.option).each(function (i, v) {
        if (i == index) {
            v.linkid = linkId;
        }
    });

    //保存修改后的JSON Str 到页面上
    jsonStr = JSON.stringify(objdata);
    jsonStr = escape(jsonStr);
    $($(QuestItem).find("input[name='JsonStr']")[0]).val(jsonStr);

}

///查询跳题链接到得问题的Index
function GetIndexByLinkID() {
    $(".spanlinkIndexText").each(function (i, v) {

        var linkid = $($(this).parent()[0]).attr("linkid");
        var targetObj = $("#divQuestionList span.qtitle[questionlinkid=" + linkid + "]");
        if (targetObj != null) {
            var curDiv = $(targetObj).parents('div.addst1');
            var divList = $(curDiv).parents().find("#divQuestionList div.addst1");
            
            var index = $(divList).index(curDiv);
       
            $(this).text(index + 1);
        }
    });
}