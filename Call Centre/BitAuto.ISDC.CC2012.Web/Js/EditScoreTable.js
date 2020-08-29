
var EditScoreT = (function () {

    var scoreObj = {
        "RTID": "-1",
        "Name": "",
        "ScoreType": "1",
        "Description": "",
        "Status": "10001",
        "Appraisal": "0",
        "DeadItemNum": "0",
        "NoDeadItemNum": "0",
        "Catage": [],
        "Dead": []
    },

    categoryhtml = "", //评分分类页面的HTML
    itemhtml = "", //质检项目页面的HTML
    standardhtml = "", //评分质检标准页面的HTML
    standardhtmlHead = "", //头部
    standardhtmlFoot = "", //尾部

    standardfivelevelhtml = "", //五级质检质检标准页面的HTML

    markhtml = "", //评分扣分项HTML
    deadhtml = "", //评分致命项页面的HTML

    Dict = ["一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十",
                "二十一", "二十二", "二十三", "二十四", "二十五", "二十六", "二十七", "二十八", "二十九", "三十"],

   MaxCID = 0,
   MaxIID = 0,
   MaxSID = 0,
   MaxMID = 0,
   MaxDID = 0,

    init = function () {//初始化

        //评分分类页面的HTML
        categoryhtml = "<ul class='categoryRow'>";
        categoryhtml += "<li><span class='count' style='color:White;'>0、</span>";
        categoryhtml += "<input name='CID' type='hidden' value=''>";
        categoryhtml += "<input name='cStatus' type='hidden' value='0'>";
        categoryhtml += "<span><input name='cName' type='text' value='' class='w280' style='width: 380px;' /></span>";
        categoryhtml += "<em  class='insr insr2 typeFlog2'><input name='cScore' type='text' value='0'  class='w50'/></em>";
        categoryhtml += "<span class='add'><a  href='#'></a></span>";
        categoryhtml += "<span class='delete'><a href='#'></a></span>";
        categoryhtml += "</li>";
        categoryhtml += "</ul>";


        //质检项目页面的HTML
        itemhtml = "<ul class='itemRow' CID='' style='display:none;'>";
        itemhtml += "<li><span class='count' style='color:White;'>0、</span>";
        itemhtml += "<input name='IID' type='hidden' value=''>";
        itemhtml += "<input name='CID' type='hidden' value=''>";
        itemhtml += "<input name='IStatus' type='hidden' value=''>";
        itemhtml += "<span><input name='IName' type='text' value='' class='w280' style='width: 380px;' /></span>";
        itemhtml += "<em  class='insr insr2 typeFlog2'><input name='IScore' type='text' value='0'  class='w50'/></em>";
        itemhtml += "<span class='add'><a  href='#'></a></span>";
        itemhtml += "<span class='delete'><a href='#'></a></span>";
        itemhtml += "</li>";
        itemhtml += "</ul>";

        //质检标准页面的HTML
        standardhtmlHead = "<ul class='standardRow' CID='' IID='' style='display:none;'>";

        standardhtml = "<li  class='SLi'><span class='count' style='color:White;'>0、</span>";
        standardhtml += "<input name='SID' type='hidden' value=''/>";
        standardhtml += "<input name='CID' type='hidden' value=''/>";
        standardhtml += "<input name='IID' type='hidden' value=''/>";
        standardhtml += "<input name='SStatus' type='hidden' value='0'/>";

        standardhtml += "<span><input name='SName' type='text' value='' class='w280' style='width: 380px;' /></span>";
        standardhtml += "<em  class='insr insr2 typeFlog2'><input name='SScore' type='text' value='0' class='w50'/></em>";
        standardhtml += "<em  class='insr insr2 typeFlog1'><input type='checkbox' value='' name='IsIsDead'/></em>";
        standardhtml += "<span class='add'><a  href='#'></a></span>";
        standardhtml += "<span class='delete'><a href='#'></a></span>";
        standardhtml += "</li>";

        standardhtmlFoot = "</ul>";

        //五级质检质检标准页面的HTML
        standardfivelevelhtml = "<li class='fivestandardRow' CID='' IID=''  >" +
                                " <label style='position: relative; top: 10px;'>技能水平：</label>" +
                                " <ul class='pfbz_choose'>" +
                                "     <li><a href='javascript:void(0)' onclick='javascript:changestatus(this,1);' class='current'>优秀</a></li>" +
                                "     <li><a href='javascript:void(0)' onclick='javascript:changestatus(this,2);' >良好</a></li>" +
                                "     <li><a href='javascript:void(0)' onclick='javascript:changestatus(this,3);' >合格</a></li>" +
                                "     <li><a href='javascript:void(0)' onclick='javascript:changestatus(this,4);' >较差</a></li>" +
                                "     <li><a href='javascript:void(0)' onclick='javascript:changestatus(this,5);' class='current'>很差</a></li>" +
                                " </ul>" +
                                " <input name='CID' type='hidden' value=''/>" +
                                " <input name='IID' type='hidden' value=''/>" +
                                " <input name='SID' type='hidden' value=''/>" +
                                " <input name='SStatus' type='hidden' value='0'/>" +
                                " <ul class=' clearfix zjpf_df'>" +
                                "     <li class='zjpf_zd'><span>技能水平</span><span style='margin-right: 21px;'>对应分值</span></li>" +
                                "     <li class='standardli standard1' ><span class='count'>质检标准1：</span> <span>" +
                                "         <input name='SName' type='text' class='w460' /></span> <em>" +
                                "             <input type='text' disabled='disabled' value='优秀' class='w50' />" +
                                "         </em><em>" +
                                "             <input type='text' disabled='disabled' name='Sore' value='' class='w50' />" +
                                "         </em>" +
                                "         <ul style='margin-top: 30px;'>" +
                                "             <li><span class='kfsm'>说明：</span> <span>" +
                                "                 <input name='SExplanation' type='text' value='' class='w393' /></span> </li>" +
                                "         </ul>" +
                                "     </li>" +
                                "     <li class='standardli standard2' style='display:none;'><span class='count'>质检标准2：</span> <span>" +
                                "         <input name='SName' type='text' class='w460' /></span> <em>" +
                                "             <input type='text' disabled='disabled' value='良好' class='w50' />" +
                                "         </em><em>" +
                                "             <input type='text' disabled='disabled' name='Sore' value='' class='w50' />" +
                                "         </em>" +
                                "         <ul style='margin-top: 30px;'>" +
                                "             <li><span class='kfsm'>说明：</span> <span>" +
                                "                 <input name='SExplanation' type='text' value='' class='w393' /></span> </li>" +
                                "         </ul>" +
                                "     </li>" +
                                "     <li class='standardli standard3' style='display:none;'><span class='count'>质检标准3：</span> <span>" +
                                "         <input name='SName' type='text' class='w460' /></span> <em>" +
                                "             <input type='text' disabled='disabled' value='合格' class='w50' />" +
                                "         </em><em>" +
                                "             <input type='text' disabled='disabled' name='Sore' value='' class='w50' />" +
                                "         </em>" +
                                "         <ul style='margin-top: 30px;'>" +
                                "             <li><span class='kfsm'>说明：</span> <span>" +
                                "                 <input name='SExplanation' type='text' value='' class='w393' /></span> </li>" +
                                "         </ul>" +
                                "     </li>" +
                                "     <li class='standardli standard4' style='display:none;'><span class='count'>质检标准4：</span> <span>" +
                                "         <input name='SName' type='text' class='w460' /></span> <em>" +
                                "             <input type='text' disabled='disabled' value='较差' class='w50' />" +
                                "         </em><em>" +
                                "             <input type='text' disabled='disabled' name='Sore' value='' class='w50' />" +
                                "         </em>" +
                                "         <ul style='margin-top: 30px;'>" +
                                "             <li><span class='kfsm'>说明：</span> <span>" +
                                "                 <input name='SExplanation' type='text' value='' class='w393' /></span> </li>" +
                                "         </ul>" +
                                "     </li>" +
                                "     <li class='standardli standard5' ><span class='count'>质检标准5：</span> <span>" +
                                "         <input name='SName' type='text' class='w460' /></span> <em>" +
                                "             <input type='text' disabled='disabled' value='很差' class='w50' />" +
                                "         </em><em>" +
                                "             <input type='text' disabled='disabled' name='Sore' value='0' class='w50' />" +
                                "         </em>" +
                                "         <ul style='margin-top: 30px;'>" +
                                "             <li><span class='kfsm'>说明：</span> <span>" +
                                "                 <input name='SExplanation' type='text' value='' class='w393' /></span> </li>" +
                                "         </ul>" +
                                "     </li>" +
                                " </ul>" +
                                "</li>";


        //扣分项
        markhtml = "<li class='MLi' CID='' IID='' SID='' ><span class='kfsm typeFlog2'>扣分说明</span><span class='kfsm typeFlog1'>特殊说明</span><span>";
        markhtml += "<input name='MCID' type='hidden' value=''/>";
        markhtml += "<input name='MIID' type='hidden' value=''/>";
        markhtml += "<input name='MSID' type='hidden' value=''/>";
        markhtml += "<input name='MID' type='hidden' value=''/>";
        markhtml += "<input name='MStatus' type='hidden' value='0'/>";
        markhtml += "<input type='text' style='width: 330px;' class='w280' value='' name='MName'/>";
        markhtml += "<em class='insr insr2 typeFlog2'><input name='MScore'  type='text' class='w50' value='0' style='margin-right: 0px;'></em>";
        markhtml += "<span class='add typeFlog2'><a href='#'></a></span>";
        markhtml += "<span class='delete typeFlog2'><a href='#'></a></span></span></li>";

        //致命项
        deadhtml = "<ul class='deadRow'>";
        deadhtml += "<li><span class='count' style=' color:White;'>0、</span>";
        deadhtml += "<input name='DID' type='hidden' value=''>";
        deadhtml += "<input name='dStatus' type='hidden' value='0'>";
        deadhtml += "<span><input name='DName' type='text' value='' class='w280' style='width: 380px;' /></span>";
        deadhtml += "<span class='add'><a  href='#'></a></span>";
        deadhtml += "<span class='delete'><a href='#'></a></span>";
        deadhtml += "</li>";
        deadhtml += "</ul>";

        BindHTMLbyOjb();

    },

      OpenCategory = function () {//打开质检类别
          if (EditScoreT.scoreObj.ScoreType == "3") {
              OpenPopupAndBind('/AjaxServers/QualityStandard/ScoreTableManage/SetCategoryFiveLevel.aspx', 'CategoryPopup');
          }
          else {
              OpenPopupAndBind('/AjaxServers/QualityStandard/ScoreTableManage/SetCategory.aspx', 'CategoryPopup');
          }
      },

   OpenItem = function () {        //打开质检类别
       if (EditScoreT.scoreObj.ScoreType == "3") {
           OpenPopupAndBind('/AjaxServers/QualityStandard/ScoreTableManage/SetItemFiveLevel.aspx', 'ItemPopup');
       }
       else {
           OpenPopupAndBind('/AjaxServers/QualityStandard/ScoreTableManage/SetItem.aspx', 'ItemPopup');
       }
   },

    OpenStandard = function () {        //打开质检标准
        if (EditScoreT.scoreObj.ScoreType == "3") {
            OpenPopupAndBind('/AjaxServers/QualityStandard/ScoreTableManage/SetStandardFiveLevel.aspx', 'StandardPopup');
        }
        else {
            OpenPopupAndBind('/AjaxServers/QualityStandard/ScoreTableManage/SetStandard.aspx', 'StandardPopup');
        }
    },

    OpenDead = function () {        //打开评分致命项
        //        if (EditScoreT.scoreObj.ScoreType == "3") {
        //            OpenPopupAndBind('/AjaxServers/QualityStandard/ScoreTableManage/SetDeadFiveLevel.aspx', 'DeadPopup');
        //        }
        //        else {
        OpenPopupAndBind('/AjaxServers/QualityStandard/ScoreTableManage/SetDead.aspx', 'DeadPopup');
        // }
    },

     AddAppraisal = function () {        //添加质检
         EditScoreT.scoreObj.Appraisal = "1";
         BindHTMLbyOjb(EditScoreT.scoreObj);
     },

   OpenPopupAndBind = function (url, PopupName) {//打开弹出层并绑定数据（通用)

       $.openPopupLayer({
           name: PopupName,
           parameters: EditScoreT.scoreObj,
           url: url + "?r=" + Math.random(),
           popupMethod: "POST",
           beforeClose: function (e, scoreObj) {
               if (e) {
                   //绑定HTML
                   BindHTMLbyOjb(scoreObj);
               }
           }
       });
   },
   BindHTMLbyOjb = function () { //根据Object,绑定HTML
       BindBaseInfo();
       BindAppraisalHTML();
       BindDealHTML();
       BindContentHTML();
   },
   BindBaseInfo = function () {

       $("#txtName").val(EditScoreT.scoreObj.Name);
       $("#txtDesc").val(EditScoreT.scoreObj.Description);
       $("#txtDeadItemNum").val(EditScoreT.scoreObj.DeadItemNum);
       $("#txtNoDeadItemNum").val(EditScoreT.scoreObj.NoDeadItemNum);
       $("[name='ScoreType'][value='" + EditScoreT.scoreObj.ScoreType + "']").attr("checked", true);

       if (EditScoreT.scoreObj.ScoreType == "1" || EditScoreT.scoreObj.ScoreType == "3") {
           $("#btnDead").show();
           $(".DeadItemDiv").hide();
       }
       else {
           $("#btnDead").hide();
           $(".DeadItemDiv").show();
       }
       //适用区域已经注释，所以此处逻辑无用
       //       var chkRegion = $(":checkbox[name='chkRegionID']");
       //       if (EditScoreT.scoreObj.RegionID) {
       //           var arr_region = EditScoreT.scoreObj.RegionID.split(',');
       //           for (var i = 0; i < arr_region.length; i++) {
       //               for (var k = 0; k < chkRegion.length; k++) {
       //                   if (chkRegion[k].value === arr_region[i]) {
       //                       chkRegion[k].checked = true;
       //                   }
       //               }
       //           }
       //       }

   },
    BindContentHTML = function () { //绑定分类、项目、标准内容

        $("#div1").html("");
        var html = "";
        $(EditScoreT.scoreObj.Catage).each(function (ci, cv) { //遍历分类
            if (cv.Status == "0") {
                var optionText = cv.Name;
                if (EditScoreT.scoreObj.ScoreType == "1" || EditScoreT.scoreObj.ScoreType == "3") {
                    optionText += " (" + cv.Score + "分)"
                }
                html += "<div class='lybase fwgf'><div class='title'>" + optionText + "</div>";
                if (EditScoreT.scoreObj.ScoreType == "3") {
                    $(cv.Item).each(function (ii, iv) { //遍历质检项目
                        if (iv.Status == "0") {
                            var optionText = iv.ItemName;
                            if (iv.Score.indexOf(".0") >= 0) {
                                iv.Score = iv.Score.substring(0, iv.Score.indexOf(".0"));
                            }
                            optionText += " (" + iv.Score + "分)";
                            html += "<p class='itemTitle'>" + optionText + "</p>";
                            html += "<table width='100%' cellspacing='0' cellpadding='0' border='1' class='tdStantard'><tbody>";

                            $(iv.Standard.sort(function (a, b) { return a.SkillLevel < b.SkillLevel ? 1 : -1 })).each(function (si, sv) { //遍历质检标准
                                if ($.trim(sv.SName) == "" || sv.Status == "-1") {
                                    return true;
                                }
                                var optionText = sv.SName;
                                if (sv.Score.toString().indexOf(".0") >= 0) {
                                    sv.Score = sv.Score.toString().substring(0, sv.Score.toString().indexOf(".0"));
                                }
                                optionText += " (" + sv.Score + "分)"

                                html += "<tr>";
                                html += "<td width='36%' class='bdlnone zdq standardTitle'>" + optionText + "</td>";
                                html += "<td width='64%' class='qrb'>" + sv.SExplanation + "</td>";
                                html += "</tr>";
                            });

                            html += "</tbody></table>";
                        }
                    });
                }
                else {
                    $(cv.Item).each(function (ii, iv) { //遍历质检项目
                        if (iv.Status == "0") {
                            var optionText = iv.ItemName;
                            if (EditScoreT.scoreObj.ScoreType == "1" || EditScoreT.scoreObj.ScoreType == "3") {
                                optionText += " (" + iv.Score + "分)"
                            }
                            html += "<p class='itemTitle'>" + optionText + "</p>";
                            html += "<table width='100%' cellspacing='0' cellpadding='0' border='1' class='tdStantard'><tbody>";

                            $(iv.Standard).each(function (si, sv) { //遍历质检标准

                                if (sv.Status != "0") {
                                    return true;
                                }
                                var optionText = sv.SName;
                                if (EditScoreT.scoreObj.ScoreType == "1" || EditScoreT.scoreObj.ScoreType == "3") {
                                    optionText += " (" + sv.Score + "分)"
                                }
                                else {
                                    if (sv.IsIsDead == "1") {
                                        optionText += " (致命)"
                                    }
                                    else {
                                        optionText += " (非致命)"
                                    }
                                }

                                html += "<tr>";
                                html += "<td width='36%' class='bdlnone zdq standardTitle'>" + optionText + "</td>";
                                if (EditScoreT.scoreObj.ScoreType == "3") {
                                    html += "<td width='64%' class='qrb'>" + sv.SExplanation + "</td>";
                                }
                                else {
                                    html += "<td width='64%' class='qrb'>";
                                }
                                html += "<table width='100%' cellspacing='0' cellpadding='0' border='0'><tbody>";

                                $(sv.Marking).each(function (mi, mv) { //遍历扣分项

                                    if (mv.Status != "0") {
                                        return true;
                                    }

                                    var optionText = mv.MName;
                                    if (EditScoreT.scoreObj.ScoreType == "1" || EditScoreT.scoreObj.ScoreType == "3") {
                                        optionText += " ( " + mv.Score + "分 )"
                                    }

                                    html += "<tr><td width='58%' class='zdq'>";
                                    html += optionText;
                                    html += "</td><td width='12%'><label>";
                                    html += "<input type='checkbox' class='dx' value='' name='' ";

                                    if (EditScoreT.scoreObj.ScoreType == "2") {
                                        if (sv.IsIsDead == "1") {
                                            html += "checked='checked'";
                                        }
                                    }

                                    html += " >";
                                    html += "</label></td><td width='30%' class='borderR'><input type='text' class='wsr' value</td>";
                                    html += "</tr>";

                                });

                                html += "</tbody></table>";
                                html += "</td>";
                                html += "</tr>";
                            });

                            html += "</tbody></table>";
                        }
                    });
                }

                html += "</div>";
            }
        });

        $("#div1").html(html);

        //给分类编号
        $(".pfb .title").each(function (i, v) {
            $(this).text(Dict[i] + "、" + $(this).text());


        });

        //给项目编号
        $(".lybase").each(function (i, v) {
            $(this).find(".itemTitle").each(function (ii, iv) {
                $(this).text((ii + 1) + "、" + $(this).text());
            });

        });


        //给质检标准编号
        $(".tdStantard").each(function (i, v) {
            $(this).find(".standardTitle").each(function (si, sv) {
                $(this).text("(" + (si + 1) + ") " + $(this).text());
            });
        });

    },
          BindDealHTML = function () { //绑定致命项
              $("#div2").html("");
              if (EditScoreT.scoreObj.ScoreType == "2") {
                  return;
              }
              if (EditScoreT.scoreObj.Dead != null && EditScoreT.scoreObj.Dead != undefined && $(EditScoreT.scoreObj.Dead).length > 0) {
                  var html = "<div class='lybase fwgf'><div class='title'>致命项</div>";
                  html += "<table width='100%' cellspacing='0' cellpadding='0' border='1' style='margin-top: 10px;' id='tableDead'><tbody>";
                  $(EditScoreT.scoreObj.Dead).each(function (i, v) {//遍历致命项
                      if (v.Status == "0") {
                          html += "<tr><td class='bdlnone zdq bd itemTitle'>";
                          html += v.DName;
                          html += "</td>";
                          html += "<td width='8%'><label><input type='checkbox' value='' name=''></label></td>";
                          html += "<td width='20%'><input type='text' class='wsr' value=''></td></tr>";
                      }
                  });
                  html += "</tbody></table>";
                  $("#div2").html(html);
              }
          },
            BindAppraisalHTML = function () { //绑定质检
                $("#div3").html("");
                if (EditScoreT.scoreObj.Appraisal == "1" || EditScoreT.scoreObj.Appraisal == "3") {
                    var html = "<div class='lybase fwgf'>";
                    html += "<div class='title'>质检评价</div>";
                    html += "<div class='pj'><textarea rows='' cols='' name=''></textarea></div>";
                    html += "</div>";
                    $("#div3").html(html);
                }
            },

    //评分分类对话框相关的代码 1
   BindCategoryHTML = function () { //根据Object,绑定类别对话框内的HTML

       var CatageList = jF("> Catage > *[Status=0]", EditScoreT.scoreObj).get(); //从JSON中查询数据，引用了js文件jfunk-0.0.1.js
       if (CatageList != null && $(CatageList).length > 0) {
           //显示HTML
           var html = "";
           $(CatageList).each(function (i, v) {
               html += categoryhtml;
           });

           $("#divCategoryPopup .Categorytitle").last().after(html);

           //绑定数据
           $(CatageList).each(function (i, v) {
               $($("#divCategoryPopup ul.categoryRow [name='cName']")[i]).val(v.Name);
               $($("#divCategoryPopup ul.categoryRow [name='CID']")[i]).val(v.CID);
               $($("#divCategoryPopup ul.categoryRow [name='cScore']")[i]).val(v.Score);
               $($("#divCategoryPopup ul.categoryRow [name='cStatus']")[i]).val(v.Status);
           });

           //判断类型
           if (EditScoreT.scoreObj.ScoreType == "2") {
               //合格型
               $("#divCategoryPopup .typeFlog2").hide();
           }
       }
       else {
           AddCategory(null);
       }
   },
    AddCategory = function (obj) { //添加一个评分分类
        var curOne;
        if (obj == null) {
            curOne = $("#divCategoryPopup .Categorytitle").last();
        }
        else {
            curOne = $(obj).parent().parent().parent();
        }

        $(curOne).after(categoryhtml);
        $(curOne).next().addClass("newObject");
        $($(curOne).next().find("[name='CID']")[0]).val(--MaxCID);
        $(curOne).next().show();
        //判断类型
        if (EditScoreT.scoreObj.ScoreType == "2") {
            //合格型
            $("#divCategoryPopup .typeFlog2").hide();
        }
    },
     DeleteCategory = function (obj) { //删除一个评分分类
         var curOne = $(obj).parent().parent().parent();
         var txtName = $.trim($($(curOne).find("[name='cName']")[0]).val()); //文本框内容
         if (txtName != "") {//如果有内容，提示
             $.jConfirm('确定要删除【' + txtName + '】类别吗', function (r) {
                 if (r) {
                     $(curOne).hide();
                     $($(curOne).find("[name='cStatus']")[0]).val("-1");
                     $(curOne).removeClass("newObject").addClass("deletObject"); //做一个删除标记

                     if ($("#divCategoryPopup ul.categoryRow:visible").length == 0) {
                         AddCategory(obj);
                     }
                     if (document.getElementById("spanTotalScore")) {
                         GetCanUseScore();
                     }
                 }
             });
         }
         else {
             $(curOne).hide();
             $($(curOne).find("[name='cStatus']")[0]).val("-1");
             $(curOne).removeClass("newObject").addClass("deletObject"); //做一个删除标记

             if ($("#divCategoryPopup ul.categoryRow:visible").length == 0) {
                 AddCategory(obj);
             }
         }
         if (EditScoreT.scoreObj.ScoreType == "3") {
             GetCanUseScore();
         }
     },
     CheckCategory = function () { //验证分类

         //检查
         var msg = "";

         var hideObject = $("#divCategoryPopup .categoryRow [name='cStatus'][value='-1']"); //隐藏的个数
         var showObject = $("#divCategoryPopup .categoryRow [name='cStatus'][value='0']"); //显示的个数

         var showText = "";
         var showScore = "0";
         if ($(showObject).length == 1) {
             showText = $.trim($($(showObject).find("[name='cName']")[0]).val());
             showScore = $.trim($($(showObject).find("[name='cScore']")[0]).val());
         }

         if ($(hideObject).length > 0 && $(showObject).length == 1 && showScore == "0" && showText == "") {
             //有被删除的，并且就显示一个，就是全部删除,不用验证
         }
         else {

             $("#divCategoryPopup .categoryRow").each(function () {
                 var Status = $.trim($($(this).find("[name='cStatus']")[0]).val());
                 var cName = $.trim($($(this).find("[name='cName']")[0]).val());
                 var cScore = $.trim($($(this).find("[name='cScore']")[0]).val());

                 if (Status == "0") {  //没有被删除的才检查

                     //评分型，要判断是否填写分数
                     if (EditScoreT.scoreObj.ScoreType == "1" || EditScoreT.scoreObj.ScoreType == "3") {
                         if (cScore == "") {
                             msg += "请输入分数值<br/>";
                         }
                         else if (isNaN(cScore)) {
                             msg += "分数值应该是数字<br/>";
                         }
                     }

                     //判断是否输入了类别文本
                     if (cName == "") {
                         msg += "分类名称不能为空<br/>";
                     }
                 }
             });
         }

         return msg;
     },
    SaveCategory = function () { //保存分类

        $("#divCategoryPopup .categoryRow").each(function () {
            var CID = $.trim($($(this).find("[name='CID']")[0]).val());
            var Status = $.trim($($(this).find("[name='cStatus']")[0]).val());
            var cName = $.trim($($(this).find("[name='cName']")[0]).val());
            var cScore = $.trim($($(this).find("[name='cScore']")[0]).val());

            if (cName == "" && cScore == "0") {
                return true; //退出当次循环
            }

            //有newObject标记，是新加的，添加到全局JSon中
            if ($(this).hasClass('newObject')) {
                var newC = {
                    "CID": CID,
                    "Name": cName,
                    "Score": cScore,
                    "Status": Status,
                    "Item": []
                };
                EditScoreT.scoreObj.Catage.push(newC);
            }
            else {
                //没有newObject标记，更新到全局JSon中

                $(EditScoreT.scoreObj.Catage).each(function (ci, cv) {
                    if (cv.CID == CID) {
                        cv.Name = cName;
                        cv.Score = cScore;
                        cv.Status = Status;
                    }
                });
            }
        });

        //关闭对话框
        $.closePopupLayer('CategoryPopup', true);

    },
    //质检项目对话框相关的代码 2
    BindItemHTML = function () { //根据Object,绑定项目对话框内的HTML

        //绑定分类下拉列表
        var CatageList = jF("> Catage > *[Status=0]", EditScoreT.scoreObj).get(); //从JSON中查询数据，引用了js文件jfunk-0.0.1.js
        if (CatageList != null && $(CatageList).length > 0) {
            $(CatageList).each(function (i, v) {
                var optionText = v.Name;
                if (EditScoreT.scoreObj.ScoreType == "1" || EditScoreT.scoreObj.ScoreType == "3") {
                    optionText += " (" + v.Score + "分)"
                }
                $("#itemSelCatagory").append("<option  value='" + v.CID + "'  score='" + v.Score + "'>" + optionText + "</option>");
            });
        }

        BindAllItemHtml(); //绑定所有质检项目

        //显示第一个分类下的质检项目
        var firstCid = $("#itemSelCatagory").val();
        ShowItemHtmlByCID(firstCid);

        //判断类型
        if (EditScoreT.scoreObj.ScoreType == "2") {
            //合格型
            $("#divItemPopup .typeFlog2").hide();
        }

    },
            BindAllItemHtml = function () { //绑定所有项目

                //取所有质检项目
                var ItemList = jF("> Catage > *[Status=0] > Item > *[Status=0]", EditScoreT.scoreObj).get(); //从JSON中查询数据，引用了js文件jfunk-0.0.1.js

                if (ItemList != null && $(ItemList).length > 0) {

                    //绑定所用分类下的所有项目HTML
                    var html = "";
                    $(ItemList).each(function (i, v) {
                        html += itemhtml;
                    });
                    $("#divItemPopup .ItemTitle").last().after(html);

                    //绑定数据
                    $(ItemList).each(function (i, v) {
                        var curUL = $("#divItemPopup ul.itemRow")[i]; //当前UL

                        $(curUL).attr("CID", v.CID);
                        $(curUL).find("[name='IID']").val(v.IID);
                        $(curUL).find("[name='CID']").val(v.CID);
                        $(curUL).find("[name='IName']").val(v.ItemName);
                        $(curUL).find("[name='IScore']").val(v.Score);
                        $(curUL).find("[name='IStatus']").val(v.Status);

                    });
                }
            },

           ShowItemHtmlByCID = function (CID) { //根据分类，绑定评分项

               if (CID == null || CID == "") {
                   return;
               }
               $("#divItemPopup ul.itemRow").hide();

               $("#divItemPopup ul.itemRow").each(function () {
                   var status = $($(this).find("[name='IStatus']")[0]).val();
                   var Cid = $(this).attr("CID");
                   if (status == "0" && Cid == CID) {
                       //如果不是删除的，就显示
                       $(this).show();
                   }
               });
               if ($("#divItemPopup ul.itemRow:visible").length == 0) {
                   //没有，添加一个空的
                   AddItem(null);
               }
           },

            AddItem = function (obj) { //添加一个质检项目
                if ($("#itemSelCatagory").val() == null || $("#itemSelCatagory").val() == "") {
                    return;
                }
                var curOne;
                if (obj == null) {
                    curOne = $("#divItemPopup .ItemTitle").last();
                }
                else {
                    curOne = $(obj).parent().parent().parent();
                }
                $(curOne).after(itemhtml);

                var newRow = $(curOne).next();
                $(newRow).addClass("newObject");
                $(newRow).attr("CID", $("#itemSelCatagory").val());
                $($(newRow).find("[name='CID']")[0]).val($("#itemSelCatagory").val());
                $($(newRow).find("[name='IID']")[0]).val(--MaxIID);
                $($(newRow).find("[name='IStatus']")[0]).val("0");
                $(newRow).show();

                //判断类型
                if (EditScoreT.scoreObj.ScoreType == "2") {
                    //合格型
                    $("#divItemPopup .typeFlog2").hide();
                }
            },

            DeleteItem = function (obj) { //删除一个质检项目
                var curOne = $(obj).parent().parent().parent();
                var txtName = $.trim($($(curOne).find("[name='IName']")[0]).val()); //文本框内容
                if (txtName != "") {//如果有内容，提示
                    $.jConfirm('确定要删除【' + txtName + '】项目吗', function (r) {
                        if (r) {
                            $(curOne).hide();
                            $($(curOne).find("[name='IStatus']")[0]).val("-1");
                            $(curOne).removeClass("newObject").addClass("deletObject"); //做一个删除标记
                            //如果删除的是最后一个，就添加一个空项
                            if ($("#divItemPopup ul.itemRow:visible").length == 0) {
                                AddItem(obj);
                            }
                            if (document.getElementById("spanCanUseScore")) {
                                GetCanUseScore();
                            }
                        }
                    });
                }
                else {
                    $(curOne).hide();
                    $($(curOne).find("[name='IStatus']")[0]).val("-1");
                    $(curOne).removeClass("newObject").addClass("deletObject"); //做一个删除标记

                    //如果删除的是最后一个，就添加一个空项
                    if ($("#divItemPopup ul.itemRow:visible").length == 0) {
                        AddItem(obj);
                    }
                }
                if (EditScoreT.scoreObj.ScoreType == "3") {
                    GetCanUseScore();
                }
            },
            CheckItem = function () {
                //检查
                var msg = "";

                if ($("#itemSelCatagory option").length == 0) {
                    msg += "没有质检类别，请先添加类别";
                }
                else {
                    //遍历分类
                    $("#itemSelCatagory option").each(function (ci, cv) {

                        var cid = $(cv).attr("value");
                        var hideObject = $("#divItemPopup .itemRow[cid='" + cid + "'] [name='IStatus'][value='-1']"); //隐藏的个数
                        var showObject = $("#divItemPopup .itemRow[cid='" + cid + "'] [name='IStatus'][value='0']"); //显示的个数

                        var showText = "";
                        var showScore = "0";
                        if ($(showObject).length == 1) {
                            showText = $.trim($($(showObject).parent().parent().find("[name='IName']")[0]).val());
                            showScore = $.trim($($(showObject).parent().parent().find("[name='IScore']")[0]).val());
                        }
                        if ($(hideObject).length > 0 && $(showObject).length == 1 && showScore == "0" && showText == "") {
                            //有被删除的，并且就显示一个，就是全部删除,不用验证
                        }
                        else {
                            var scoreNum = 0; //项目分数和

                            $("#divItemPopup .itemRow[cid='" + cid + "']").each(function () {
                                var Status = $.trim($($(this).find("[name='IStatus']")[0]).val());
                                var iName = $.trim($($(this).find("[name='IName']")[0]).val());
                                var iScore = $.trim($($(this).find("[name='IScore']")[0]).val());

                                if (Status == "0") {  //没有被删除的才检查

                                    //评分型，要判断是否填写分数
                                    if (EditScoreT.scoreObj.ScoreType == "1") {
                                        if (iScore == "") {
                                            msg += "请输入分数值<br/>";
                                        }
                                        else if (isNaN(iScore)) {
                                            msg += "分数值应该是数字<br/>";
                                        }
                                        else {
                                            scoreNum += Number(iScore);
                                        }
                                    }

                                    //判断是否输入了类别文本
                                    if (iName == "") {
                                        msg += "质检项目名称不能为空<br/>";
                                    }
                                }
                            });
                            if (EditScoreT.scoreObj.ScoreType == "1") {
                                var scoreSum = $(cv).attr("score");
                                if (scoreNum != scoreSum) {
                                    msg += "【" + $(cv).text() + "】分类下项目分数的和应该等于当前分类的分数";
                                }
                            }
                        }

                    });
                }
                return msg;
            },
           SaveItem = function () { //保存质检项目

               //遍历分类
               $("#itemSelCatagory option").each(function (ci, cv) {
                   var cid = $(cv).attr("value");

                   $("#divItemPopup .itemRow[cid='" + cid + "']").each(function () {
                       var CID = $.trim($($(this).find("[name='CID']")[0]).val());
                       var IID = $.trim($($(this).find("[name='IID']")[0]).val());
                       var Status = $.trim($($(this).find("[name='IStatus']")[0]).val());
                       var iName = $.trim($($(this).find("[name='IName']")[0]).val());
                       var iScore = $.trim($($(this).find("[name='IScore']")[0]).val());

                       if (iName == "" && iScore == "0") {
                           return true; //退出当次循环
                       }

                       //有newObject标记，是新加的，添加到全局JSon中
                       if ($(this).hasClass('newObject')) {
                           var newI = {
                               "IID": IID,
                               "ItemName": iName,
                               "Score": iScore,
                               "Status": Status,
                               "CID": CID,
                               "Standard": []
                           };
                           EditScoreT.scoreObj.Catage[ci].Item.push(newI);
                       }
                       else {
                           //没有newObject标记，更新到全局JSon中

                           $(EditScoreT.scoreObj.Catage[ci].Item).each(function (ii, iv) {
                               if (iv.IID == IID) {
                                   iv.ItemName = iName;
                                   iv.Score = iScore;
                                   iv.Status = Status;
                                   iv.CID = CID;
                               }
                           });
                       }
                   });
               });

               //关闭对话框
               $.closePopupLayer('ItemPopup', true);
           },

    //  3
    BindStandardHTML = function () { //根据Object,绑定标准对话框内的HTML

        //绑定分类下拉列表
        var CatageList = jF("> Catage > *[Status=0]", EditScoreT.scoreObj).get(); //从JSON中查询数据，引用了js文件jfunk-0.0.1.js
        if (CatageList != null && $(CatageList).length > 0) {
            $(CatageList).each(function (i, v) {
                var optionText = v.Name;
                if (EditScoreT.scoreObj.ScoreType == "1") {
                    optionText += " (" + v.Score + "分)"
                }
                $("#standardSelCatagory").append("<option  value='" + v.CID + "'>" + optionText + "</option>");
            });

            BindItemByCID($("#standardSelCatagory").val()); //根据第一分类绑定项目
        }

        //绑定所有质检标准
        BindAllStandardHtml();

        //显示第一个分类和第一个项目下的质检项目
        var firstCid = $("#standardSelCatagory").val();
        var firstIID = $("#standardSelItem").val();
        ShowStandardHtmlByCIDIID(firstCid, firstIID);

        //判断类型
        if (EditScoreT.scoreObj.ScoreType == "2") {
            //合格型
            $("#divstandardPopup .typeFlog2").hide();
            $("#divstandardPopup .typeFlog1").show();
        }
        else {
            $("#divstandardPopup .typeFlog1").hide();
            $("#divstandardPopup .typeFlog2").show();
        }
    },
    //  3 绑定五级质检项
    BindStandardHTMLFiveLevel = function () { //根据Object,绑定标准对话框内的HTML

        //绑定分类下拉列表
        var CatageList = jF("> Catage > *[Status=0]", EditScoreT.scoreObj).get(); //从JSON中查询数据，引用了js文件jfunk-0.0.1.js
        if (CatageList != null && $(CatageList).length > 0) {
            $(CatageList).each(function (i, v) {
                var optionText = v.Name;
                if (EditScoreT.scoreObj.ScoreType == "3") {
                    optionText += " (" + v.Score + "分)"
                }
                $("#standardSelCatagory").append("<option  value='" + v.CID + "'>" + optionText + "</option>");
            });

            BindItemByCID($("#standardSelCatagory").val()); //根据第一分类绑定项目
        }

        //绑定所有质检标准
        BindAllStandardFiveLevelHtml();

        //显示第一个分类和第一个项目下的质检项目
        var firstCid = $("#standardSelCatagory").val();
        var firstIID = $("#standardSelItem").val();
        ShowStandardHtmlByCIDIID(firstCid, firstIID);

    },
  BindAllStandardHtml = function () { //绑定所用质检标准

      //取所有质检标准
      var StandardList = jF("> Catage > *[Status=0] > Item > *[Status=0] > Standard > *[Status=0]", EditScoreT.scoreObj).get(); //从JSON中查询数据，引用了js文件jfunk-0.0.1.js

      if (StandardList != null && $(StandardList).length > 0) {

          //绑定所有标准HTML
          var html = "";
          $(StandardList).each(function (i, v) {
              html += standardhtmlHead;
              html += standardhtml;
              if (v.Marking != null && $(v.Marking).length > 0) {
                  $(v.Marking).each(function () { //扣分项
                      html += markhtml;
                  });
              }
              else {
                  html += markhtml;
              }
              html += standardhtmlFoot;
          });

          $("#divstandardPopup .StandardTitle").last().after(html);

          //绑定数据
          $(StandardList).each(function (i, v) {

              var curUL = $("#divstandardPopup ul.standardRow")[i];

              $(curUL).attr("CID", v.CID);
              $(curUL).attr("IID", v.IID);
              $(curUL).find("[name='SID']").val(v.SID);
              $(curUL).find("[name='SName']").val(v.SName);
              $(curUL).find("[name='SID']").val(v.SID);
              $(curUL).find("[name='SScore']").val(v.Score);
              if (v.IsIsDead == "1") {
                  $(curUL).find("[name='IsIsDead']").attr("checked", true);
              }
              else {
                  $(curUL).find("[name='IsIsDead']").attr("checked", false);
              }
              $(curUL).find("[name='SStatus']").val(v.Status);

              //绑定扣分项
              if (v.Marking != undefined && v.Marking.length > 0) {
                  $(v.Marking).each(function (j, v2) { //扣分项

                      var curM = $(curUL).find(".MLi")[j]; //当前扣分项
                      $(curM).attr("CID", v2.CID);
                      $(curM).attr("IID", v2.IID);
                      $(curM).attr("SID", v2.SID);
                      $(curM).find("[name='MID']").val(v2.MID);
                      $(curM).find("[name='MName']").val(v2.MName);
                      $(curM).find("[name='MStatus']").val(v2.Status);
                      $(curM).find("[name='MScore']").val(v2.Score);

                      if (v2.Status == -1) {
                          $(curM).css("display", "none");
                      }

                  });
              }
              else {
                  $(curUL).find("li.MLi").addClass("newObjectM");
                  $(curUL).find("li.MLi").attr("CID", v.CID);
                  $(curUL).find("li.MLi").attr("IID", v.IID);
                  $(curUL).find("li.MLi").attr("sid", v.SID);
                  $($(curUL).find("li.MLi [name='CID']")[0]).val(v.CID);
                  $($(curUL).find("li.MLi [name='IID']")[0]).val(v.IID);
                  $($(curUL).find("li.MLi [name='SID']")[0]).val(v.SID);
                  $($(curUL).find("li.MLi [name='MID']")[0]).val(--MaxMID);
                  $($(curUL).find("li.MLi [name='MStatus']")[0]).val("0");
              }
          });
      }
  },
   BindAllStandardFiveLevelHtml = function () { //绑定所用质检标准
       //取所有质检项目
       var StandardList = jF("> Catage > *[Status=0] > Item > *[Status=0]", EditScoreT.scoreObj).get(); //从JSON中查询数据，引用了js文件jfunk-0.0.1.js
       //给每个项目绑定指定数量五级质检模板HTML
       for (var i = 0; i < StandardList.length; i++) {
           //此项目下如果存在质检标准，则增加一个五级质检模板HTML
           if (StandardList[i].Standard.length > 0) {
               $("#divstandardPopup #selectorsli").after(standardfivelevelhtml);
           }
       }
       var rowindex = 0;
       //绑定指定数量五级质检模板HTML数据
       for (var i = 0; i < StandardList.length; i++) {
           if (StandardList[i].Standard.length > 0) {
               var curUL = $("#divstandardPopup li.fivestandardRow")[rowindex];
               $(curUL).attr("CID", StandardList[i].CID);
               $(curUL).attr("IID", StandardList[i].IID);
               rowindex = rowindex + 1;
               //循环遍历项目下的质检项目
               for (var j = 0; j < StandardList[i].Standard.length; j++) {
                   //遍历指定模板中的质检项目
                   $(curUL).find(" .standardli").each(function () {
                       var sSkillLevel = 0;
                       var SID = 0;
                       var strclassname = $(this).attr("class");
                       if (strclassname.indexOf("standard1") >= 0) {
                           sSkillLevel = 5;
                           SID = 5;
                       }
                       else if (strclassname.indexOf("standard2") >= 0) {
                           sSkillLevel = 4;
                           SID = 4;
                       }
                       else if (strclassname.indexOf("standard3") >= 0) {
                           sSkillLevel = 3;
                           SID = 3;
                       }
                       else if (strclassname.indexOf("standard4") >= 0) {
                           sSkillLevel = 2;
                           SID = 2;
                       }
                       else if (strclassname.indexOf("standard5") >= 0) {
                           sSkillLevel = 1;
                           SID = 1;
                       }
                       //在模板中找到了对应的质检标准，则将此json中的数据绑定到对应模板的质检标准上
                       if (StandardList[i].Standard[j].SkillLevel == SID) {
                           $(this).find("input:[name='SName']").val(StandardList[i].Standard[j].SName);
                           $(this).find("input:[name='Score']").val(StandardList[i].Standard[j].Score);
                           $(this).find("input:[name='SExplanation']").val(StandardList[i].Standard[j].SExplanation);
                           $(this).find("input:[name='SStatus']").val(StandardList[i].Standard[j].SStatus);
                           //质检标准名称不为空，并且质检标准状态正常，则绑定之前标准数据
                           if ($.trim(StandardList[i].Standard[j].SName) != "" && StandardList[i].Standard[j].Status == "0") {
                               switch (SID) {
                                   case 5:
                                       //修改选择按钮的状态为选中状态（current）
                                       $(curUL).find(" .pfbz_choose a").each(function () {
                                           if ($(this).text() == "优秀") {
                                               if (!$(this).hasClass("current")) {
                                                   $(this).addClass("current");
                                               }
                                           }
                                       });
                                       //显示按钮对应的选项
                                       $(curUL).find(" .standard1").show();
                                       break;
                                   case 4:
                                       $(curUL).find(" .pfbz_choose a").each(function () {
                                           if ($(this).text() == "良好") {
                                               if (!$(this).hasClass("current")) {
                                                   $(this).addClass("current");
                                               }
                                           }
                                       });
                                       $(curUL).find(" .standard2").show();
                                       break;
                                   case 3:
                                       $(curUL).find(" .pfbz_choose a").each(function () {
                                           if ($(this).text() == "合格") {
                                               if (!$(this).hasClass("current")) {
                                                   $(this).addClass("current");
                                               }
                                           }
                                       });
                                       $(curUL).find(" .standard3").show();
                                       break;
                                   case 2:
                                       $(curUL).find(" .pfbz_choose a").each(function () {
                                           if ($(this).text() == "较差") {
                                               if (!$(this).hasClass("current")) {
                                                   $(this).addClass("current");
                                               }
                                           }
                                       });
                                       $(curUL).find(" .standard4").show();
                                       break;
                                   case 1:
                                       $(curUL).find(" .pfbz_choose a").each(function () {
                                           if ($(this).text() == "很差") {
                                               if (!$(this).hasClass("current")) {
                                                   $(this).addClass("current");
                                               }
                                           }
                                       });
                                       $(curUL).find(" .standard5").show();
                                       break;
                                   default: break;
                               }
                           }
                           return false;
                       }
                   });
               }
           }
       }

       //       if (StandardList != null && $(StandardList).length > 0) {
       //           //绑定指定个数的五级质检模板HTML
       //           var html = "";
       //           $(StandardList).each(function (i, v) {
       //               html += standardfivelevelhtml;
       //           });
       //           $("#divstandardPopup #selectorsli").after(html);

       //           //绑定数据
       //           $(StandardList).each(function (i, v) {
       //               var curUL = $("#divstandardPopup li.fivestandardRow")[i];
       //               $(curUL).attr("CID", v.CID);
       //               $(curUL).attr("IID", v.IID);
       //               //先使此质检项目下的五级质检按钮变灰 
       //               $(curUL).find(" .pfbz_choose a").each(function () {
       //                   if ($(this).hasClass("current")) {
       //                       $(this).removeClass("current");
       //                   }
       //               });
       //               //先隐藏五级质检质检标准
       //               $(curUL).find(" .standardli").each(function () {
       //                   $(this).hide();
       //               });

       //               $(curUL).find(" .standardli").each(function () {
       //                   var sSkillLevel = 0;
       //                   var SID = 0;
       //                   var strclassname = $(this).attr("class");
       //                   if (strclassname.indexOf("standard1") >= 0) {
       //                       sSkillLevel = 5;
       //                       SID = 5;
       //                   }
       //                   else if (strclassname.indexOf("standard2") >= 0) {
       //                       sSkillLevel = 4;
       //                       SID = 4;
       //                   }
       //                   else if (strclassname.indexOf("standard3") >= 0) {
       //                       sSkillLevel = 3;
       //                       SID = 3;
       //                   }
       //                   else if (strclassname.indexOf("standard4") >= 0) {
       //                       sSkillLevel = 2;
       //                       SID = 2;
       //                   }
       //                   else if (strclassname.indexOf("standard5") >= 0) {
       //                       sSkillLevel = 1;
       //                       SID = 1;
       //                   }

       //                   if (v.SID == SID) {
       //                       $(this).find("input:[name='SName']").val(v.SName);
       //                       $(this).find("input:[name='Score']").val(v.Score);
       //                       $(this).find("input:[name='SExplanation']").val(v.SExplanation);
       //                       if (v.Status == 0) {
       //                           switch (SID) {
       //                               case 5:
       //                                   $("#divstandardPopup .pfbz_choose a").each(function () {
       //                                       if ($(this).text() == "优秀") {
       //                                           if (!$(this).hasClass("current")) {
       //                                               $(this).addClass("current");
       //                                           }
       //                                       }
       //                                   });
       //                                   $(curUL).find(" .standard1").show();
       //                                   break;
       //                               case 4:
       //                                   $("#divstandardPopup .pfbz_choose a").each(function () {
       //                                       if ($(this).text() == "良好") {
       //                                           if (!$(this).hasClass("current")) {
       //                                               $(this).addClass("current");
       //                                           }
       //                                       }
       //                                   });
       //                                   $(curUL).find(" .standard2").show();
       //                                   break;
       //                               case 3:
       //                                   $("#divstandardPopup .pfbz_choose a").each(function () {
       //                                       if ($(this).text() == "合格") {
       //                                           if (!$(this).hasClass("current")) {
       //                                               $(this).addClass("current");
       //                                           }
       //                                       }
       //                                   });
       //                                   $(curUL).find(" .standard3").show();
       //                                   break;
       //                               case 2:
       //                                   $("#divstandardPopup .pfbz_choose a").each(function () {
       //                                       if ($(this).text() == "较差") {
       //                                           if (!$(this).hasClass("current")) {
       //                                               $(this).addClass("current");
       //                                           }
       //                                       }
       //                                   });
       //                                   $(curUL).find(" .standard4").show();
       //                                   break;
       //                               case 1:
       //                                   $("#divstandardPopup .pfbz_choose a").each(function () {
       //                                       if ($(this).text() == "很差") {
       //                                           if (!$(this).hasClass("current")) {
       //                                               $(this).addClass("current");
       //                                           }
       //                                       }
       //                                   });
       //                                   $(curUL).find(" .standard5").show();
       //                                   break;
       //                           }
       //                       }
       //                   }
       //               });
       //           });
       //       }
   },

           ShowStandardHtmlByCIDIID = function (CID, IID) { //根据分类，显示评分项
               if (EditScoreT.scoreObj.ScoreType == "3") {
                   //                   //隐藏所有质检标准模板
                   //                   $("#divstandardPopup li.fivestandardRow").hide();
                   if (CID == null || CID == "" || IID == null || IID == "") {
                       return;
                   }
                   $("#divstandardPopup li.fivestandardRow").each(function () {
                       var Cid = $(this).attr("CID");
                       var IId = $(this).attr("IID");
                       if (Cid == CID && IId == IID) {
                           //如果是当前分类项目所属的模板，则显示
                           $(this).show();
                           //排序质检标标准名称前的汉字，计算对应的分数
                           changestandardtileindexscore($("#divstandardPopup .fivestandardRow:visible"));
                       }
                       else {
                           $(this).hide();
                       }
                   });
                   //若果在已绑定的模板中没有涨到当前分类项目所属的模板，则说明此分类项目下未添加过质检标准，需要新增模板。
                   if ($("#divstandardPopup li.fivestandardRow:visible").length == 0) {
                       AddFiveStandard(null);
                   }
               }
               else {
                   $("#divstandardPopup ul.standardRow").hide();

                   if (CID == null || CID == "" || IID == null || IID == "") {
                       return;
                   }

                   $("#divstandardPopup ul.standardRow").each(function () {

                       var status = $($(this).find("[name='SStatus']")[0]).val();
                       var Cid = $(this).attr("CID");
                       var IId = $(this).attr("IID");
                       if (status == "0" && Cid == CID && IId == IID) {
                           //如果不是删除的，就显示
                           $(this).show();

                           //显示扣分项
                           var Mlist = $(this).find("li.MLi");
                           $(Mlist).each(function () {
                               status = $($(this).find("[name='MStatus']")[0]).val();
                               if (status == "0") {
                                   $(this).show();
                               }
                           });

                       }
                   });
                   if ($("#divstandardPopup ul.standardRow:visible").length == 0) {
                       //没有，添加一个空的
                       AddStandard(null);
                   }
               }
           },
           BindItemByCID = function (CID) { // 质检标准页面，根据分类，绑定项目

               //绑定项目下拉列表
               var ItemList = jF("> Catage > *[Status=0][CID=" + CID + "] > Item > *[Status=0]", EditScoreT.scoreObj).get(); //从JSON中查询数据，引用了js文件jfunk-0.0.1.js
               $("#standardSelItem").html("");

               if (ItemList != null && $(ItemList).length > 0) {

                   $(ItemList).each(function (i, v) {
                       var optionText = v.ItemName;
                       if (EditScoreT.scoreObj.ScoreType == "1" || EditScoreT.scoreObj.ScoreType == "3") {
                           optionText += " (" + v.Score + "分)"
                       }
                       $("#standardSelItem").append("<option  value='" + v.IID + "' score='" + v.Score + "'>" + optionText + "</option>");
                   });
               }
           },

            AddStandard = function (obj) { //添加一个质检标准

                var CID = $("#standardSelCatagory").val();
                var IID = $("#standardSelItem").val();
                if (CID == null || CID == "" || IID == null || IID == "") {
                    return;
                }

                var html = standardhtmlHead + standardhtml + markhtml + standardhtmlFoot;
                var curOne;

                if (obj == null) {
                    //如果null,则
                    curOne = $("#divstandardPopup .StandardTitle").last();
                }
                else {
                    curOne = $(obj).parent().parent().parent();
                }

                //生成空HTML

                $(curOne).after(html);

                //绑定数据
                var newRow = $(curOne).next();
                $(newRow).addClass("newObject");
                $(newRow).attr("CID", $("#standardSelCatagory").val());
                $(newRow).attr("IID", $("#standardSelItem").val());
                $($(newRow).find("[name='CID']")[0]).val($("#standardSelCatagory").val());
                $($(newRow).find("[name='IID']")[0]).val($("#standardSelItem").val());
                $($(newRow).find("[name='CID']")[0]).val($("#standardSelCatagory").val());
                $($(newRow).find("[name='SID']")[0]).val(--MaxSID);
                $(newRow).show();

                //显示扣分项
                var Mlist = $(newRow).find("li.MLi");
                $(Mlist).each(function () {

                    $(this).addClass("newObjectM");
                    $(this).attr("CID", $("#standardSelCatagory").val());
                    $(this).attr("IID", $("#standardSelItem").val());
                    $(this).attr("SID", $($(newRow).find("[name='SID']")[0]).val());

                    $($(this).find("[name='CID']")[0]).val($("#standardSelCatagory").val());
                    $($(this).find("[name='IID']")[0]).val($("#standardSelItem").val());
                    $($(this).find("[name='SID']")[0]).val($($(newRow).find("[name='SID']")[0]).val());
                    $($(this).find("[name='MID']")[0]).val(--MaxMID);
                    $(this).show();
                });

                //判断类型
                if (EditScoreT.scoreObj.ScoreType == "2") {
                    //合格型
                    $("#divstandardPopup .typeFlog2").hide();
                    $("#divstandardPopup .typeFlog1").show();
                }
                else {
                    $("#divstandardPopup .typeFlog1").hide();
                    $("#divstandardPopup .typeFlog2").show();
                }
            },
             AddFiveStandard = function () { //添加一个质检标准

                 var CID = $("#standardSelCatagory").val();
                 var IID = $("#standardSelItem").val();
                 if (CID == null || CID == "" || IID == null || IID == "") {
                     return;
                 };
                 var hasrow = false;
                 $("#divstandardPopup .fivestandardRow").each(function () {
                     if ($(this).attr("CID") == CID && $(this).attr("IID") == IID) {
                         hasrow = true;
                         $(this).show();
                         return false;
                     }
                 });
                 //没有CID,IID对应的模板信息，则添加新模板
                 if (!hasrow) {
                     var curOne = $("#divstandardPopup #selectorsli").last()
                     $(curOne).after(standardfivelevelhtml);

                     //绑定数据
                     var newRow = $(curOne).next();
                     $(newRow).addClass("newObject");
                     $(newRow).attr("CID", $("#standardSelCatagory").val());
                     $(newRow).attr("IID", $("#standardSelItem").val());
                     $($(newRow).find("[name='CID']")[0]).val($("#standardSelCatagory").val());
                     $($(newRow).find("[name='IID']")[0]).val($("#standardSelItem").val());
                     $($(newRow).find("[name='SID']")[0]).val(--MaxSID);  //此处能否不要SID？
                     $(newRow).show();
                     $(newRow).find(" .pfbz_choose a").each(function (i) {
                         switch ($(this).text()) {
                             case "良好": changestatus($(this)[0], 2); break;
                             case "合格": changestatus($(this)[0], 3); break;
                             case "较差": changestatus($(this)[0], 4); break;
                             default: break;
                         }
                     });
                     changestandardtileindexscore($(newRow));
                 }
                 //判断类型
                 if (EditScoreT.scoreObj.ScoreType == "2") {
                     //合格型
                     $("#divstandardPopup .typeFlog2").hide();
                     $("#divstandardPopup .typeFlog1").show();
                 }
                 else {
                     $("#divstandardPopup .typeFlog1").hide();
                     $("#divstandardPopup .typeFlog2").show();
                 }
             },

            DeleteStandard = function (obj) { //删除一个质检标准
                var curOne = $(obj).parent().parent().parent();
                var txtName = $.trim($($(curOne).find("[name='SName']")[0]).val()); //文本框内容
                if (txtName != "") {//如果有内容，提示
                    $.jConfirm('确定要删除【' + txtName + '】标准吗', function (r) {
                        if (r) {
                            $(curOne).hide();
                            $(curOne).removeClass("newObject").addClass("deletObject"); //做一个删除标记
                            $($(curOne).find("[name='SStatus']")[0]).val("-1");

                            //如果删除的是最后一个，就添加一个空项
                            if ($("#divstandardPopup ul.standardRow:visible").length == 0) {
                                AddStandard(obj);
                            }
                        }
                    });
                }
                else {
                    $(curOne).hide();
                    $($(curOne).find("[name='SStatus']")[0]).val("-1");
                    $(curOne).removeClass("newObject").addClass("deletObject"); //做一个删除标记
                    //如果删除的是最后一个，就添加一个空项
                    if ($("#divstandardPopup ul.standardRow:visible").length == 0) {
                        AddStandard(obj);
                    }
                }
            },
            AddMarking = function (obj) { //添加一个扣分项

                var CID = $("#standardSelCatagory").val();
                var IID = $("#standardSelItem").val();
                if (CID == null || CID == "" || IID == null || IID == "") {
                    return;
                }

                //  markhtml
                var curStandardOne = $(obj).parent().parent().parent().parent(); //当前质检标准
                var curMarkingOne = $(obj).parent().parent().parent(); //当前扣分项
                var SID = $($(curStandardOne).find("[name='SID']")[0]).val();

                $(curMarkingOne).after(markhtml);

                var newRow = $(curMarkingOne).next();
                $(newRow).addClass("newObjectM");
                $(newRow).attr("CID", $("#standardSelCatagory").val());
                $(newRow).attr("IID", $("#standardSelItem").val());
                $(newRow).attr("SID", SID);
                $($(newRow).find("[name='CID']")[0]).val($("#standardSelCatagory").val());
                $($(newRow).find("[name='IID']")[0]).val($("#standardSelItem").val());
                $($(newRow).find("[name='MID']")[0]).val(--MaxMID);
                $($(newRow).find("[name='SID']")[0]).val(SID);
                $($(newRow).find("[name='MStatus']")[0]).val("0");
                $(newRow).show();

                //判断类型
                if (EditScoreT.scoreObj.ScoreType == "2") {
                    //合格型
                    $("#divstandardPopup .typeFlog2").hide();
                }
                else {
                    $("#divstandardPopup .typeFlog1").hide();
                }
            },
             DeleteMarking = function (obj) { //添加一个扣分项
                 var curStandardOne = $(obj).parent().parent().parent().parent(); //当前质检标准
                 var curOne = $(obj).parent().parent().parent(); //当前扣分项
                 var txtName = $.trim($($(curOne).find("[name='MName']")[0]).val()); //文本框内容
                 if (txtName != "") {//如果有内容，提示
                     $.jConfirm('确定要删除【' + txtName + '】吗', function (r) {
                         if (r) {
                             $(curOne).hide();
                             $(curOne).removeClass("newObjectM").addClass("deletObjectM");
                             $($(curOne).find("[name='MStatus']")[0]).val("-1");

                             //如果删除的是最后一个，就添加一个空项
                             if ($(curStandardOne).find("li.MLi:visible").length == 0) {
                                 AddMarking(obj);
                             }
                         }
                     });
                 }
                 else {
                     $(curOne).hide();
                     $(curOne).removeClass("newObjectM").addClass("deletObjectM");
                     $($(curOne).find("[name='MStatus']")[0]).val("-1");

                     //如果删除的是最后一个，就添加一个空项
                     if ($(curStandardOne).find("li.MLi:visible").length == 0) {
                         AddMarking(obj);
                     }
                 }

             },
             CheckStandard = function () {//验证标准

                 var msg = "";

                 //遍历分类
                 $(EditScoreT.scoreObj.Catage).each(function (ci, cv) {

                     //遍历质检项目
                     $(EditScoreT.scoreObj.Catage[ci].Item).each(function (ii, iv) {

                         var hideObject = $("#divstandardPopup .standardRow[cid='" + cv.CID + "'][iid='" + iv.IID + "'] [name='SStatus'][value='-1']"); //删除的个数
                         var showObject = $("#divstandardPopup .standardRow[cid='" + cv.CID + "'][iid='" + iv.IID + "'] [name='SStatus'][value='0']"); //正常的个数

                         var showText = "";
                         var showScore = "0";
                         if ($(showObject).length == 1) {
                             showText = $.trim($($(showObject).parent().parent().find("[name='SName']")[0]).val());
                             showScore = $.trim($($(showObject).parent().parent().find("[name='SScore']")[0]).val());
                         }
                         if ($(hideObject).length > 0 && $(showObject).length == 1 && showScore == "0" && showText == "") {
                             //有被删除的，并且就显示一个，就是全部删除,不用验证
                         }
                         else {
                             var scoreNum = 0; //质检标准分数和

                             //循环同一分类、同一项目的质检标准
                             $("#divstandardPopup .standardRow[cid='" + cv.CID + "'][iid='" + iv.IID + "']").each(function () {
                                 var Status = $.trim($($(this).find("[name='SStatus']")[0]).val());
                                 var sName = $.trim($($(this).find("[name='SName']")[0]).val());
                                 var sScore = $.trim($($(this).find("[name='SScore']")[0]).val());

                                 if (Status == "0") {  //没有被删除的才检查

                                     //评分型，要判断是否填写分数
                                     if (EditScoreT.scoreObj.ScoreType == "1") {
                                         if (sScore == "") {
                                             msg += "请输入分数值<br/>";
                                         }
                                         else if (isNaN(sScore)) {
                                             msg += "分数值应该是数字<br/>";
                                         }
                                         else {
                                             scoreNum += Number(sScore);
                                         }
                                     }

                                     //判断是否输入了类别文本
                                     if (sName == "") {
                                         msg += "质检标准名称不能为空<br/>";
                                     }


                                     //检查扣分项
                                     $(this).find(".MLi").each(function (pmi, pmv) { // //循环页面上的评分扣分项

                                         var MName = $.trim($($(this).find("[name='MName']")[0]).val());
                                         var Score = $.trim($($(this).find("[name='MScore']")[0]).val());
                                         var Status = $.trim($($(this).find("[name='MStatus']")[0]).val());

                                         if (Status == "0") {

                                             //评分型，要判断是否填写分数
                                             if (EditScoreT.scoreObj.ScoreType == "1") {
                                                 if (Score == "") {
                                                     msg += "请输入扣分项分数值<br/>";
                                                 }
                                                 else if (isNaN(Score)) {
                                                     msg += "扣分项分数值应该是数字<br/>";
                                                 }
                                                 else if (Score >= 0) {
                                                     msg += "扣分项必须是负数<br/>";
                                                 }
                                                 else {
                                                     if (Number(Score) * -1 > sScore) {
                                                         msg += "扣分项的分数不能超过扣分标准的分数值<br/>";
                                                     }
                                                 }
                                             }

                                             //判断是否输入了类别文本
                                             //                                             if (MName == "") {
                                             //                                                 if (EditScoreT.scoreObj.ScoreType == "1") {
                                             //                                                     msg += "扣分项名称不能为空<br/>";
                                             //                                                 }
                                             //                                                 else {
                                             //                                                     msg += "特殊说明不能为空<br/>";
                                             //                                                 }
                                             //                                             }
                                         }

                                     });
                                 }
                             });

                             if (EditScoreT.scoreObj.ScoreType == "1") {
                                 if (scoreNum != iv.Score) {
                                     msg += "【" + iv.ItemName + "】项目下质检标准分数的和应该等于它所在项目的分数<br/>";
                                 }
                             }
                         }

                     });
                 });

                 return msg;
             },
             SaveStandardInfo = function () {
                 //遍历分类
                 $(EditScoreT.scoreObj.Catage).each(function (ci, cv) {

                     //遍历质检项目
                     $(EditScoreT.scoreObj.Catage[ci].Item).each(function (ii, iv) {

                         //循环页面上的质检标准
                         $("#divstandardPopup .standardRow[cid='" + cv.CID + "'][iid='" + iv.IID + "']").each(function (psi, psv) {
                             var CID = $.trim($($(this).find("[name='CID']")[0]).val());
                             var IID = $.trim($($(this).find("[name='IID']")[0]).val());
                             var SID = $.trim($($(this).find("[name='SID']")[0]).val());
                             var Status = $.trim($($(this).find("[name='SStatus']")[0]).val());
                             var sName = $.trim($($(this).find("[name='SName']")[0]).val());
                             var sScore = $.trim($($(this).find("[name='SScore']")[0]).val());
                             var IsIsDead = $($(this).find("[name='IsIsDead']")[0]).attr("checked") ? "1" : "0";

                             if (sName == "" && sScore == "0") {
                                 return true; //退出当次循环
                             }

                             //有newObject标记，是新加的，添加到全局JSon中
                             if ($(this).hasClass('newObject')) {
                                 var newS = {
                                     "SID": SID,
                                     "IID": iv.IID,
                                     "CID": cv.CID,
                                     "SName": sName,
                                     "Score": sScore,
                                     "IsIsDead": IsIsDead,
                                     "Status": Status,
                                     "Marking": []
                                 };
                                 EditScoreT.scoreObj.Catage[ci].Item[ii].Standard.push(newS);

                                 //新增标准下的所有扣分项，都是新增的
                                 $(psv).find(".MLi").each(function (pmi, pmv) { // //循环页面上的评分扣分项
                                     var MID = $.trim($($(this).find("[name='MID']")[0]).val());
                                     var MName = $.trim($($(this).find("[name='MName']")[0]).val());
                                     var Score = $.trim($($(this).find("[name='MScore']")[0]).val());
                                     var Status = $.trim($($(this).find("[name='MStatus']")[0]).val());

                                     var newM = {
                                         "CID": cv.CID,
                                         "IID": iv.IID,
                                         "SID": SID,
                                         "MID": MID,
                                         "MName": MName,
                                         "Score": Score,
                                         "Status": Status
                                     };

                                     EditScoreT.scoreObj.Catage[ci].Item[ii].Standard[EditScoreT.scoreObj.Catage[ci].Item[ii].Standard.length - 1].Marking.push(newM);

                                 });

                             }
                             else {
                                 //没有newObject标记，更新到全局JSon中
                                 var Exists = false;
                                 $(EditScoreT.scoreObj.Catage[ci].Item[ii].Standard).each(function (si, sv) {
                                     if (sv.SID == SID) {
                                         sv.SName = sName;
                                         sv.Score = sScore;
                                         sv.Status = Status,
                                             sv.IsIsDead = IsIsDead;

                                         Exists = true;
                                     }
                                 });
                                 //判断全局json对象里是否有这个质检标准，没有
                                 if (Exists) {

                                     //编辑标准，扣分项有新增的，有编辑的

                                     $(psv).find(".MLi").each(function (pmi, pmv) { // //循环页面上的评分扣分项
                                         var MID = $.trim($($(this).find("[name='MID']")[0]).val());
                                         var MName = $.trim($($(this).find("[name='MName']")[0]).val());
                                         var Score = $.trim($($(this).find("[name='MScore']")[0]).val());
                                         var Status = $.trim($($(this).find("[name='MStatus']")[0]).val());

                                         if ($(this).hasClass('newObjectM')) {

                                             var newM = {
                                                 "CID": cv.CID,
                                                 "IID": iv.IID,
                                                 "SID": SID,
                                                 "MID": MID,
                                                 "MName": MName,
                                                 "Score": Score,
                                                 "Status": Status
                                             };

                                             $(EditScoreT.scoreObj.Catage[ci].Item[ii].Standard).each(function (i, s) {
                                                 if (s.SID == SID) {
                                                     EditScoreT.scoreObj.Catage[ci].Item[ii].Standard[i].Marking.push(newM);
                                                 }
                                             });
                                         }
                                         else {

                                             $(EditScoreT.scoreObj.Catage[ci].Item[ii].Standard).each(function (i, s) {
                                                 if (s.SID == SID) {
                                                     //遍历扣分项
                                                     $(EditScoreT.scoreObj.Catage[ci].Item[ii].Standard[i].Marking).each(function (mi, mv) {

                                                         //编辑的扣分项
                                                         if (mv.MID == MID) {
                                                             mv.MName = MName;
                                                             mv.Score = Score;
                                                             mv.Status = Status
                                                         }
                                                     });
                                                 }

                                             });
                                         }

                                     });
                                 }
                             }
                         });
                     });
                 });

             },
              SaveStandardFiveLevelInfo = function () {
                  ////测试代码
                  //                  $("#divstandardPopup .fivestandardRow").each(function (i, v) {
                  //                      var fullscore = GetSelectOptionScore($("#divstandardPopup #standardSelItem option[value='" + $(this).attr("IID") + "']").text());
                  //                      console.log(i + ": CID=" + $(this).attr("CID") + " ; IID=" + $(this).attr("IID") + " ; FullScore=" + fullscore);
                  //                      $(this).find(" .standardli").each(function (i) {
                  //                          var sName = $.trim($($(this).find("input:[name='SName']")[0]).val());
                  //                          var sScore = $.trim($($(this).find(" input:[name='Sore']")[0]).val());
                  //                          var sSExplanation = $.trim($($(this).find("[name='SExplanation']")[0]).val());
                  //                          var strclassname = $(this).attr("class");
                  //                          console.log("       Name=" + sName + "; Score=" + sScore + " ; SExplanation=" + sSExplanation + " ; classname=" + strclassname);
                  //                      });
                  //                  });

                  //遍历分类
                  $(EditScoreT.scoreObj.Catage).each(function (ci, cv) {
                      //遍历质检项目
                      $(EditScoreT.scoreObj.Catage[ci].Item).each(function (ii, iv) {
                          //循环页面上的质检标准
                          $("#divstandardPopup .fivestandardRow[CID='" + cv.CID + "'][IID='" + iv.IID + "']").each(function (psi, psv) {
                              //有newObject标记，是新加的模板，则将其添加到全局JSon中
                              var cId = $(this).attr("CID");
                              var iId = $(this).attr("IID");

                              $(this).find(" .standardli").each(function (i) {
                                  var isNewObj = $(this).hasClass('newObject');
                                  var sScore = 0;  /// $.trim($($(this).find(" input:[name='Sore']")[0]).val());
                                  var sName = $.trim($($(this).find("input:[name='SName']")[0]).val());
                                  var sSExplanation = $.trim($($(this).find("[name='SExplanation']")[0]).val());
                                  var isDisplay = $(this).css("display");
                                  //console.log("sName = " + sName + " ;isDisplay = " + isDisplay);
                                  var sSkillLevel = 0;
                                  var SID = 0;
                                  var strclassname = $(this).attr("class");
                                  if (strclassname.indexOf("standard1") >= 0) {
                                      sSkillLevel = 5;
                                      SID = 5;
                                      sScore = iv.Score;
                                  }
                                  else if (strclassname.indexOf("standard2") >= 0) {
                                      sSkillLevel = 4;
                                      SID = 4;
                                      sScore = iv.Score * 100 * 0.8 / 100
                                  }
                                  else if (strclassname.indexOf("standard3") >= 0) {
                                      sSkillLevel = 3;
                                      SID = 3;
                                      sScore = iv.Score * 100 * 0.6 / 100
                                  }
                                  else if (strclassname.indexOf("standard4") >= 0) {
                                      sSkillLevel = 2;
                                      SID = 2;
                                      sScore = iv.Score * 100 * 0.4 / 100
                                  }
                                  else if (strclassname.indexOf("standard5") >= 0) {
                                      sSkillLevel = 1;
                                      SID = 1;
                                      sScore = 0;
                                  }
                                  if (isNewObj && isDisplay != "none") {
                                      var newS = {
                                          "CID": cv.CID,
                                          "IID": iv.IID,
                                          "SID": SID,
                                          "SName": sName,
                                          "Score": sScore,
                                          "Status": 0,
                                          "SExplanation": sSExplanation,
                                          "SkillLevel": sSkillLevel
                                      };
                                      EditScoreT.scoreObj.Catage[ci].Item[ii].Standard.push(newS);
                                  }
                                  else {
                                      //此标准组已经加入全局json
                                      //检查此次循环的标准项是否经加入到该项目中：如果没有，则需要新增标准项；如果有，则需要更新标准项内容
                                      var isThisStandardExist = false;
                                      $(EditScoreT.scoreObj.Catage[ci].Item[ii].Standard).each(function (si, sv) {
                                          if (SID == sv.SkillLevel) {
                                              isThisStandardExist = true;
                                              sv.SName = sName;
                                              sv.Score = sScore;
                                              sv.SExplanation = sSExplanation;
                                              sv.Status = isDisplay == "none" ? -1 : 0;
                                              return false;
                                          }
                                      });
                                      if (!isThisStandardExist) {
                                          var newS = {
                                              "CID": cv.CID,
                                              "IID": iv.IID,
                                              "SID": SID,
                                              "SName": sName,
                                              "Score": sScore,
                                              "Status": isDisplay == "none" ? -1 : 0,
                                              "SExplanation": sSExplanation,
                                              "SkillLevel": sSkillLevel
                                          };
                                          EditScoreT.scoreObj.Catage[ci].Item[ii].Standard.push(newS);
                                      }
                                  }
                              });
                          });
                      });
                  });
              }, checkFiveLevelStandardInfo = function () {
                  //遍历分类
                  var msg = "";
                  var hasNotMatchInfo = false;
                  $(EditScoreT.scoreObj.Catage).each(function (ci, cv) {
                      //遍历质检项目
                      if (EditScoreT.scoreObj.Catage[ci].Status == "0") {
                          $(EditScoreT.scoreObj.Catage[ci].Item).each(function (ii, iv) {
                              if (EditScoreT.scoreObj.Catage[ci].Item[ii].Status == "0") {
                                  var standardlength = EditScoreT.scoreObj.Catage[ci].Item[ii].Standard.length;
                                  if (standardlength < 1) {
                                      msg += "【" + EditScoreT.scoreObj.Catage[ci].Name + "】质检类别下的【" + EditScoreT.scoreObj.Catage[ci].Item[ii].ItemName + "】质检项目下没有创建质检标准，因此无法保存！<br/>";
                                      hasNotMatchInfo = true;
                                      //return false;
                                  }
                                  var hasNoNameStandard = false;
                                  $(EditScoreT.scoreObj.Catage[ci].Item[ii]).each(function (si, sv) {
                                      for (var i = 0; i < sv.Standard.length; i++) {
                                          if ($.trim(sv.Standard[i].SName) == "" && $.trim(sv.Standard[i].Status == "0")) {
                                              msg += "【" + EditScoreT.scoreObj.Catage[ci].Name + "】质检类别下的【" + EditScoreT.scoreObj.Catage[ci].Item[ii].ItemName + "】质检项目下的质检标准内容为空，因此无法保存！<br/>";
                                              hasNoNameStandard = true;
                                              //return false;
                                          }
                                      }
                                  });
                                  if (hasNoNameStandard) {
                                      return false;
                                  }
                              }
                          });
                      }
                  });
                  if (msg != "") {
                      $.jAlert(msg);
                  }
                  return hasNotMatchInfo;
              },
    SaveStandard = function () {  //保存标准（只是保存，不验证）

        $.blockUI({ message: '正在处理，请等待...' });
        if (EditScoreT.scoreObj.ScoreType == "3") {
            SaveStandardFiveLevelInfo();
        }
        else {
            SaveStandardInfo();
        }
        $.unblockUI();

        //关闭对话框
        $.closePopupLayer('StandardPopup', true);
    },

     SubmitStandard = function () {  //提交标准（验证后保存）

         $.blockUI({ message: '正在处理，请等待...' });

         var msg = CheckStandard();

         if (msg == "") {
             SaveStandardInfo();
             //关闭对话框
             $.closePopupLayer('StandardPopup', true);
         }
         else {
             $.jAlert(msg);
         }
         $.unblockUI();

     },
    //致命项相关的代码 4
    BindDeadHTML = function () { //根据Object,绑定类别对话框内的HTML

        var DeadList = jF("> Dead > *[Status=0]", EditScoreT.scoreObj).get(); //从JSON中查询数据，引用了js文件jfunk-0.0.1.js
        if (DeadList != null && $(DeadList).length > 0) {
            //显示HTML
            var html = deadhtml;
            $(DeadList).each(function (i, v) {
                if (i > 0) {
                    html += deadhtml;
                }
            });

            $("#divDeadPopup .Deadtitle").last().after(html);

            //绑定数据
            $(DeadList).each(function (i, v) {
                $($("#divDeadPopup ul.deadRow [name='DName']")[i]).val(v.DName);
                $($("#divDeadPopup ul.deadRow [name='DID']")[i]).val(v.DID);
                $($("#divDeadPopup ul.deadRow [name='dStatus']")[i]).val(v.Status);
            });
        }
        else {
            AddDead(null);
        }
    },
            AddDead = function (obj) { //添加一个致命项
                var curOne;
                if (obj == null) {
                    curOne = $("#divDeadPopup .Deadtitle").last();
                }
                else {
                    curOne = $(obj).parent().parent().parent();
                }
                $(curOne).after(deadhtml);
                $(curOne).next().addClass("newObject");
                $($(curOne).next().find("[name='DID']")[0]).val(--MaxDID);
                $($(curOne).next().find("[name='dStatus']")[0]).val("0");
                $(curOne).next().show();
            },
            DeleteDead = function (obj) { //删除一个致命项
                var curOne = $(obj).parent().parent().parent();
                var txtName = $.trim($($(curOne).find("[name='DName']")[0]).val()); //文本框内容
                if (txtName != "") {//如果有内容，提示
                    $.jConfirm('确定要删除【' + txtName + '】致命项吗', function (r) {
                        if (r) {
                            $(curOne).hide();
                            $($(curOne).find("[name='dStatus']")[0]).val("-1");

                            if ($("#divDeadPopup ul.deadRow:visible").length == 0) {
                                AddDead(obj);
                            }
                        }
                    });
                }
                else {
                    $(curOne).hide();
                    $($(curOne).find("[name='dStatus']")[0]).val("-1");
                    if ($("#divDeadPopup ul.deadRow:visible").length == 0) {
                        AddDead(obj);
                    }
                }
            },
            SaveDead = function () {

                //检查
                var msg = "";

                var hideObject = $("#divDeadPopup .deadRow [name='dStatus'][value='-1']"); //隐藏的个数
                var showObject = $("#divDeadPopup .deadRow [name='dStatus'][value='0']"); //显示的个数

                var showText = "";
                if ($(showObject).length == 1) {
                    showText = $.trim($($(showObject).find("[name='dName']")[0]).val());
                }

                if ($(hideObject).length > 0 && $(showObject).length == 1 && showText == "") {
                    //有被删除的，并且就显示一个，就是全部删除,不用验证
                }
                else {

                    $("#divDeadPopup .deadRow").each(function () {
                        var Status = $.trim($($(this).find("[name='dStatus']")[0]).val());
                        var dName = $.trim($($(this).find("[name='DName']")[0]).val());

                        if (Status == "0") {  //没有被删除的才检查

                            //判断是否输入了文本
                            if (dName == "") {
                                msg += "致命项不能为空<br/>";
                            }
                        }
                    });
                }

                //保存
                if (msg != "") {
                    $.jAlert(msg);
                }
                else {

                    $("#divDeadPopup .deadRow").each(function () {
                        var DID = $.trim($($(this).find("[name='DID']")[0]).val());
                        var Status = $.trim($($(this).find("[name='dStatus']")[0]).val());
                        var dName = $.trim($($(this).find("[name='DName']")[0]).val());

                        if (dName == "") {
                            return true; //退出本次循环
                        }

                        //有newObject标记，是新加的，添加到全局JSon中
                        if ($(this).hasClass('newObject')) {
                            var newD = {
                                "DID": DID,
                                "DName": dName,
                                "Status": Status
                            };
                            EditScoreT.scoreObj.Dead.push(newD);
                        }
                        else {
                            //没有newObject标记，更新到全局JSon中

                            $(EditScoreT.scoreObj.Dead).each(function (di, dv) {
                                if (dv.DID == DID) {
                                    dv.DName = dName;
                                    dv.Status = Status;
                                }
                            });
                        }
                    });

                    //关闭对话框
                    $.closePopupLayer('DeadPopup', true);
                }
            },
       SaveCheck = function () { //保存验证



       },
    SaveInfo = function (ActionType) { //保存
        if (EditScoreT.scoreObj.ScoreType == "3" && ActionType == "Submit") {
            if (checkFiveLevelStandardInfo()) {
                return false;
            }
        }
        // var jsonStr = JSON.stringify(EditScoreT.scoreObj);
        var jsonStr = jQuery.toJSONstring(EditScoreT.scoreObj);
        AjaxPost('/AjaxServers/QualityStandard/ScoreTableManage/ScoreTableSave.ashx', { "DataStr": escape(jsonStr), "ActionType": ActionType },
            function () {
                $.blockUI({ message: '正在处理，请等待...' });
            }
            ,
             function (data) {
                 $.unblockUI();
                 var jsonData = $.evalJSON(data);
                 if (jsonData.result == "success") {
                     var QS_RTID = jsonData.QS_RTID;


                     if (ActionType == "Save") {
                         $.jPopMsgLayer("保存成功！", function () {
                             window.location = "EditScoreTable.aspx?QS_RTID=" + QS_RTID;
                         });
                     }
                     if (ActionType == "PreView") {
                         try {
                             var pagehost = window.location.host;
                             //window.external.MethodScript('/browsercontrol/newpage?url=' + escape('<%=BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ExitAddress")%>/QualityStandard/ScoreTableManage/ScoreTableView.aspx?QS_RTID=' + QS_RTID));
                             window.external.MethodScript('/browsercontrol/newpage?url=' + escape('/QualityStandard/ScoreTableManage/ScoreTableView.aspx?QS_RTID=' + QS_RTID));
                         }
                         catch (e) {
                             window.open("/QualityStandard/ScoreTableManage/ScoreTableView.aspx?QS_RTID=" + QS_RTID); //,"_blank"
                         }

                         //当前页面转向编辑页面
                         window.location = "EditScoreTable.aspx?QS_RTID=" + QS_RTID;
                     }
                     else if (ActionType == "Submit") {
                         $.jPopMsgLayer("提交成功！", function () {
                             closePage();
                         });
                     }
                 }
                 else {
                     $.jAlert("操作出错！<br/>" + jsonData.msg);
                 }

             });

    }
           ;
    return {
        //全局
        init: init,
        Dict: Dict,
        scoreObj: scoreObj,
        OpenCategory: OpenCategory,
        OpenItem: OpenItem,
        OpenStandard: OpenStandard,
        OpenDead: OpenDead,
        AddAppraisal: AddAppraisal,
        BindHTMLbyOjb: BindHTMLbyOjb,
        MaxCID: MaxCID,
        MaxIID: MaxIID,
        MaxSID: MaxSID,
        MaxMID: MaxMID,
        MaxDID: MaxDID,

        //评分分类
        BindCategoryHTML: BindCategoryHTML,
        AddCategory: AddCategory,
        DeleteCategory: DeleteCategory,
        SaveCategory: SaveCategory,

        //质检项目
        BindItemHTML: BindItemHTML,
        ShowItemHtmlByCID: ShowItemHtmlByCID,
        BindAllItemHtml: BindAllItemHtml,
        AddItem: AddItem,
        DeleteItem: DeleteItem,
        SaveItem: SaveItem,

        //质检标准
        BindStandardHTML: BindStandardHTML,
        BindStandardHTMLFiveLevel: BindStandardHTMLFiveLevel,
        BindAllStandardHtml: BindAllStandardHtml,
        BindAllStandardFiveLevelHtml: BindAllStandardFiveLevelHtml,
        ShowStandardHtmlByCIDIID: ShowStandardHtmlByCIDIID,
        AddStandard: AddStandard,
        DeleteStandard: DeleteStandard,
        BindItemByCID: BindItemByCID,
        AddMarking: AddMarking,
        DeleteMarking: DeleteMarking,
        SaveStandard: SaveStandard,
        SubmitStandard: SubmitStandard,

        //致命项
        BindDeadHTML: BindDeadHTML,
        AddDead: AddDead,
        DeleteDead: DeleteDead,
        SaveDead: SaveDead,

        //提交
        SaveCheck: SaveCheck,
        SaveInfo: SaveInfo
    }

})();



 