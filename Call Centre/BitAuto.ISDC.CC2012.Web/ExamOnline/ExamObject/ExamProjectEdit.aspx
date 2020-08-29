<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamProjectEdit.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.ExamObject.ExamProjectEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>考试项目管理</title>
    <link href="../../css/base.css" type="text/css" rel="stylesheet" />
    <link href="../../css/style.css" type="text/css" rel="stylesheet" />
    <script src="../../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../../Js/common.js" type="text/javascript"></script>
    <script src="../../Js/json2.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../../css/GooCalendar.css" />
    <script type="text/javascript" src="../../js/GooCalendar.js"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script src="../../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        function fnMakeChange(MCvalue) {
            if(MCvalue=="0"){
                $("input[name='IsMakeUp']").eq(1).attr("checked","checked");
                $("#MakeUpUL").css("display","none");
            }
            else {
                $("input[name='IsMakeUp']").eq(0).attr("checked","checked");
                $("#MakeUpUL").css("display","block");
            }
        }

        //选择参考人员
        function openSelectBrandPopup(isMakeUp) {
            $.openPopupLayer({
                name: "SelectEmployeePopup",
                parameters: { UserIDs: $.trim($("#EmIDs"+isMakeUp).val()) },
                url: "GetEmployeeList.aspx",
                beforeClose: function (e, data) {
                    if(e){
                        $("#EmNames"+isMakeUp).val(data.split(";")[0]);
                        $("#EmIDs"+isMakeUp).val(data.split(";")[1]);
                    }                    
                }
            }
         );
        }

        //选择试卷
        function openSelectExamPaper(isMakeUp) {
            $.openPopupLayer({
                name: "SelectExamPaper",
                parameters: { ExamPaperID: $.trim($("#ExamPaperID"+isMakeUp).val()) },
                url: "GetExamPaperList.aspx",
                beforeClose: function (e, data) {
                    if(e){
                        $("#ExamPaperName"+isMakeUp).val(data.split(";")[0]);
                        $("#ExamPaperID"+isMakeUp).val(data.split(";")[1]);
                    }
                }
            }
         );
        }

        function fnSubExamObject(){          
            var isSuccess = false;

            var action = $("#inputSaveOrSub").val(); //（save:保存  complate:完成）
            var exObjID = $("#ExObjID").val();
            var title = $("#ExObjTitle").val();
            var ecid = '';
            if ($("input[name='ExObjCate']:checked").val() != undefined) {
                ecid = $("input[name='ExObjCate']:checked").val();
            }
            var description = $("#ExObjDescription").val();
            var group = $("#ExObjGroup").val();
            var examPersonIDs = $("#EmIDs1").val();//考生IDs
            var epid = $("#ExamPaperID1").val();//试卷ID
            var examStartTime = $("#calenStar1").val();
            var examEndTime = $("#calenEnd1").val();
            var isMakeUp = $("input[name='IsMakeUp']:checked").val(); 
            //补考相关
            var makeUpEcid = $("#ExamPaperID2").val();
            var makeStartTime = $("#calenStar2").val();
            var makeEndTime = $("#calenEnd2").val();
            var makeExamPersonIDs = $("#EmIDs2").val();
            
            //数据验证
            if(title == ''){
                $.jAlert("考试名称不能为空！");
                return ;
            }
            if(ecid == ''){
                $.jAlert("未选择考试分类！");
                return ;
            }
            if (description == '') {
                $.jAlert("考试说明不可为空！");
                return;
            }
            if (epid == '') {
                $.jAlert("未选择考试试卷！");
                return;
            }
            if (examPersonIDs == '') {
                $.jAlert("未选择考生！");
                return;
            }
            if (examStartTime == '') {
                $.jAlert("未选择开始时间！");
                return;
            }
            if (examEndTime == '') {
                $.jAlert("未选择结束时间！");
                return;
            }
            if(isMakeUp == '1'){
                if (makeUpEcid == '') {
                    $.jAlert("未选择补考试卷！");
                    return;
                }
                if (makeExamPersonIDs == '') {
                    $.jAlert("未选择补考考生！");
                    return;
                }
                if (makeStartTime == '') {
                    $.jAlert("未选择补考开始时间！");
                    return;
                }
                if (makeEndTime == '') {
                    $.jAlert("未选择补考结束时间！");
                    return;
                }
            }
            //数据提交到数据库
            var pody = {Action: action, ExObjID: exObjID, Title: escape(title), Ecid: ecid, 
            Description:escape(description), Group: escape(group), ExamPersonIDs: examPersonIDs, Epid: epid, 
            ExamStartTime: examStartTime, ExamEndTime: examEndTime, IsMakeUp: isMakeUp, 
            MakeUpEcid: makeUpEcid, MakeStartTime: makeStartTime, MakeEndTime: makeEndTime, makeExamPersonIDs: makeExamPersonIDs
            ,BGID:$("#selExamBG").val()
            };
            
            AjaxPostAsync('SubExamProjec.ashx', pody,
            function(){},
            function (data) {
                 var jsonData = $.evalJSON(data);
                 
                 if (jsonData.result == "success") {
                     $.jAlert("操作成功,点击关闭！");
                     closePage();
                 }
                 else {
                     $.jAlert("添加失败。错误信息：" + jsonData.msg);
                 }
             } );
        }

        $(document).ready(function () {
            //InitWdatePicker(3, ["calenStar1", "calenEnd1"]);
            //保存事件
            $("#btnSave").click(function () {
                $("#inputSaveOrSub").val("save");
                fnSubExamObject();
            });
            //完成事件
            $("#btnSubmit").click(function () {
                $("#inputSaveOrSub").val("complate");
                fnSubExamObject();
            });
            <% if(Action()){ %>
                //编辑时初始化参数
                $("#ExObjID").val('<% =examInfo.EIID %>');
                $("#ExObjTitle").val('<% =examInfo.Name %>');
                
                $(":radio[name='ExObjCate'][value="+'<% =examInfo.ECID %>'+"]").attr("checked","checked");//分类
                
                $("#ExObjDescription").val('<% =examInfo.Description %>');
                
                $("#ExObjGroup").val('<%=examInfo.BusinessGroup%>');//组
                $("#ExamPaperName1").val('<% =examPaper.Name %>');//试卷名
                $("#ExamPaperID1").val('<% =examInfo.EPID %>');//试卷ID
                $("#EmIDs1").val('<% =ExamPersionsIDs %>');//考生ID
                $("#EmNames1").val('<% =ExamPersionsNames %>');//考生姓名
                $("#calenStar1").val('<% =examInfo.ExamStartTime %>');
                $("#calenEnd1").val('<% =examInfo.ExamEndTime %>');
               
                $("#selExamBG option[value='<%=examInfo.BGID%>']").attr("selected", true);
  
                //进行中的不可编辑
                <%if(DateTime.Now > examInfo.ExamStartTime){ %>
                $("#ExObjTitle").attr("disabled","disabled");
                $(":radio[name='ExObjCate']").attr("disabled","disabled");
                $("#ExObjDescription").attr("disabled","disabled");
                $("#ExObjGroup").attr("disabled","disabled");
                $("#button_EmNames1").css("display","none");
                $("#button_ExamPaperName1").css("display","none");
                $("#calenStar1").attr("disabled","disabled");
                $("#calenEnd1").attr("disabled","disabled");
                <%} %>

                var calenStar1 = $.createGooCalendar("calenStar1", property2);
                var calenStar2 = $.createGooCalendar("calenStar2", property2);
                var calenEnd1 = $.createGooCalendar("calenEnd1", property2);
                var calenEnd2 = $.createGooCalendar("calenEnd2", property2);

                calenStar1.setDate({ year: <%=examInfo.ExamStartTime.Year %>, 
                                    month: <%=examInfo.ExamStartTime.Month%>, 
                                      day: <%=examInfo.ExamStartTime.Day %>, 
                                     hour: <%=examInfo.ExamStartTime.Hour %>, 
                                   minute: <%=examInfo.ExamStartTime.Minute %>, 
                                   second: <%=examInfo.ExamStartTime.Second %> });
                calenEnd1.setDate({ year: <%=examInfo.ExamEndTime.Year %>, 
                                    month: <%=examInfo.ExamEndTime.Month%>, 
                                      day: <%=examInfo.ExamEndTime.Day %>, 
                                     hour: <%=examInfo.ExamEndTime.Hour %>, 
                                   minute: <%=examInfo.ExamEndTime.Minute %>, 
                                   second: <%=examInfo.ExamEndTime.Second %> });
                fnMakeChange(<%=examInfo.IsMakeUp %>)
                                
                
                <%if(examInfo.IsMakeUp ==1){%>
                $("#ExamPaperName2").val('<% =makeExamPaper.Name %>');//补考试卷名
                $("#ExamPaperID2").val('<% =makeExamPaper.EPID %>');//补考试卷ID
                $("#EmIDs2").val('<% =MakeExamPersionsIDs %>');//考生ID
                $("#EmNames2").val('<% =MakeExamPersionsNames %>');//考生姓名
                 $("#calenStar2").val('<% =makeExamInfo.MakeUpExamStartTime %>');
                $("#calenEnd2").val('<% =makeExamInfo.MakeupExamEndTime %>');
                calenStar2.setDate({ year: <%=makeExamInfo.MakeUpExamStartTime.Year %>, 
                                    month: <%=makeExamInfo.MakeUpExamStartTime.Month%>, 
                                      day: <%=makeExamInfo.MakeUpExamStartTime.Day %>, 
                                     hour: <%=makeExamInfo.MakeUpExamStartTime.Hour %>, 
                                   minute: <%=makeExamInfo.MakeUpExamStartTime.Minute %>, 
                                   second: <%=makeExamInfo.MakeUpExamStartTime.Second %> });

                calenEnd2.setDate({  year: <%=makeExamInfo.MakeupExamEndTime.Year %>, 
                                    month: <%=makeExamInfo.MakeupExamEndTime.Month%>, 
                                      day: <%=makeExamInfo.MakeupExamEndTime.Day %>, 
                                     hour: <%=makeExamInfo.MakeupExamEndTime.Hour %>, 
                                   minute: <%=makeExamInfo.MakeupExamEndTime.Minute %>, 
                                   second: <%=makeExamInfo.MakeupExamEndTime.Second %> });

                    //补考进行中不可编辑
                    <%if(DateTime.Now > makeExamInfo.MakeUpExamStartTime){ %>
                    $("input[name='IsMakeUp']").attr("disabled","disabled");
                    $("#button_EmNames2").css("display","none");
                    $("#button_ExamPaperName2").css("display","none");

                    $("#calenStar2").attr("disabled","disabled");
                    $("#calenEnd2").attr("disabled","disabled");
                    <%} %>

                    
                <%}%>

            <%}else{ %>
                fnMakeChange("0");
                $("#btnSubmit").css("display","none");
                calenStar1 = $.createGooCalendar("calenStar1", property2);
                calenStar2 = $.createGooCalendar("calenStar2", property2);
                calenEnd1 = $.createGooCalendar("calenEnd1", property2);
                calenEnd2 = $.createGooCalendar("calenEnd2", property2);
            <%} %>
        });
    </script>
    <script type="text/javascript">
        var property2 = {
            divId: "calen1", //日历控件最外层DIV的ID
            needTime: true, //是否需要显示精确到秒的时间选择器，即输出时间中是否需要精确到小时：分：秒 默认为FALSE可不填
            yearRange: [1970, 2030], //可选年份的范围,数组第一个为开始年份，第二个为结束年份,如[1970,2030],可不填
            //week: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'], //数组，设定了周日至周六的显示格式,可不填
            //month: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'], //数组，设定了12个月份的显示格式,可不填
            format: "yyyy-MM-dd hh:mm:ss"
            /*设定日期的输出格式,可不填*/
        };
        var property3 = {
            divId: "calen2", //日历控件最外层DIV的ID
            needTime: false, //是否需要显示精确到秒的时间选择器，即输出时间中是否需要精确到小时：分：秒 默认为FALSE可不填
            //yearRange: [1970, 2030], //可选年份的范围,数组第一个为开始年份，第二个为结束年份,如[1970,2030],可不填
            week: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'], //数组，设定了周日至周六的显示格式,可不填
            month: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'], //数组，设定了12个月份的显示格式,可不填
            format: "yyyy-MM-dd"
            /*设定日期的输出格式,可不填*/
        };
        var property = {
            divId: "demo",
            needTime: true,
            fixid: "fff"
            /*决定了日历的显示方式，默认不填时为点击INPUT后出现，但如果填了此项，日历控件将始终显示在一个id为其值（如"fff"）的DIV中（且此DIV必须存在）*/
        };
        $(document).ready(function () {
            //            calenStar1 = $.createGooCalendar("calenStar1", property2);
            //            $.createGooCalendar("calenStar2", property2);
            //            $.createGooCalendar("calenEnd1", property2);
            //            $.createGooCalendar("calenEnd2", property2);
            //            calenStar1.setDate({ year: 2008, month: 11, day: 22, hour: 14, minute: 52, second: 45 });
            //calenStar1.setDate({ year: 2008, month: 11, day: 22, hour: 14, minute: 52, second: 45 });
            //canva2 = $.createGooCalendar("calen2", property2);
            //canva2.setDate({ year: 2008, month: 11, day: 22, hour: 14, minute: 52, second: 45 });
            $("#selExamBG").css({ "border": "#CCC 1px solid" });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            添加考试项目</div>
        <div class="addexam clearfix">
            <ul>
                <li>
                    <label>
                        <span class="redColor">*</span>考试名称：</label>
                    <span>
                        <input type="text" value="" id="ExObjTitle" class="w260" /></span>
                    <input type="hidden" id="ExObjID" value="0" />
                </li>
                <li>
                    <label>
                        <span class="redColor">*</span>所属分组：
                    </label>
                    <span style="width:265px; margin:0px;padding:0px;">
                        <select id="selExamBG" class="select" style="width:265px;" runat="server">
                        </select>
                    </span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>考试分类：</label>
                    <span>
                        <asp:Repeater ID="Rpt" runat="server">
                            <ItemTemplate>
                                <label style="float: none; cursor: pointer; font-weight: normal;">
                                    <input name="ExObjCate" type="radio" value='<%#Eval("ECID") %>' /><em class="dx"><%#Eval("Name") %></em>&nbsp;
                                </label>
                            </ItemTemplate>
                        </asp:Repeater>
                    </span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>考试说明：</label>
                    <span>
                        <textarea name="" cols="" rows="" id="ExObjDescription"></textarea></span>
                </li>
                <li>
                    <label>
                        考试范围：</label>
                    <span>
                        <input type="text" value="" id="ExObjGroup" class="w260" /></span> </li>
                <li>
                    <label>
                        <span class="redColor">*</span>考试试卷：</label>
                    <span>
                        <input type="text" id="ExamPaperName1" disabled="disabled" class="w550" style="width: 570px;
                            *width: 568px;" /></span>&nbsp;
                    <input type="hidden" id="ExamPaperID1" class="w550" />
                    <span class="btnOption" style="display: inline-block; vertical-align: top;">
                        <input name="" id="button_ExamPaperName1" type="button" value="选择" onclick="openSelectExamPaper(1)" /></span>
                </li>
                <li>
                    <label>
                        <span class="redColor">*</span>考生姓名：</label>
                    <span>
                        <textarea name="" id="EmNames1" cols="" rows="" disabled="disabled"></textarea></span>&nbsp;
                    <input type="hidden" id="EmIDs1" />
                    <span class="btnOption" style="vertical-align: top; display: inline-block;">
                        <input name="" id="button_EmNames1" type="button" value="选择" onclick="javascript:openSelectBrandPopup(1);" /></span>
                </li>
                <li>
                    <label>
                        <span class="redColor">*</span>考试时间：</label>
                    <span>
                        <input type="text" id="calenStar1" class="w90" style="width: 121px;" /></span>-<span>
                            <input type="text" id="calenEnd1" class="w90" style="width: 122px;" /></span>
                </li>
                <li>
                    <label>
                        <span class="redColor">*</span>是否补考：</label><span>
                            <label style="float: none; cursor: pointer; font-weight: normal;">
                                <input name="IsMakeUp" type="radio" value="1" onclick="fnMakeChange('1')" /><em class="dx">是</em></label></span>&nbsp;&nbsp;&nbsp;<span>
                                    <label style="float: none; cursor: pointer; font-weight: normal;">
                                        <input name="IsMakeUp" type="radio" value="0" onclick="fnMakeChange('0')" /><em class="dx">否</em></label></span>
                </li>
            </ul>
            <ul id="MakeUpUL">
                <li>
                    <label>
                        <span class="redColor">*</span>补考试卷：</label>
                    <span>
                        <input type="text" id="ExamPaperName2" value="" disabled="disabled" class="w550"
                            style="width: 570px; *width: 568px;" /></span>&nbsp;
                    <input type="hidden" id="ExamPaperID2" class="w550" />
                    <span class="btnOption" style="vertical-align: top;">
                        <input name="" id="button_ExamPaperName2" type="button" value="选择" onclick="openSelectExamPaper(2)" /></span>
                </li>
                <li>
                    <label>
                        <span class="redColor">*</span>考生姓名：</label><span>
                            <textarea name="" id="EmNames2" cols="" disabled="disabled"></textarea></span>&nbsp;
                    <input type="hidden" id="EmIDs2" />
                    <span class="btnOption" style="display: inline-block; vertical-align: top;">
                        <input name="" id="button_EmNames2" type="button" value="选择" onclick="javascript:openSelectBrandPopup(2);" />
                    </span></li>
                <li>
                    <label>
                        <span class="redColor">*</span>考试时间：</label>
                    <span>
                        <input type="text" id="calenStar2" class="w90" style="width: 121px;" /></span>-<span>
                            <input type="text" id="calenEnd2" class="w90" style="width: 122px;" /></span>
                </li>
            </ul>
        </div>
        <div class="btn" style="margin: 20px auto;">
            <input type="button" name="" value="保 存" id="btnSave" />&nbsp;&nbsp;
            <input type="button" name="" value="完 成" id="btnSubmit" />&nbsp;&nbsp;
            <input type="button" value="取 消" onclick='closePage()' />
            <input type="hidden" id="inputSaveOrSub" /><!--状态save保存，complate完成-->
        </div>
    </div>
    </form>
</body>
</html>
