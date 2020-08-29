<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditMemberDetail.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustCheck.EditMemberDetail" %>

<!--功能废弃 强斐 2016-8-3-->
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>会员二次核实</title>
    <link href="../Css/base.css" rel="stylesheet" type="text/css" />
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <link href="../Css/cc_list.css" rel="stylesheet" type="text/css" />
    <script src="../Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="../Js/common.js" type="text/javascript"></script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script src="../Js/jquery.uploadify.v2.1.4.min.js" type="text/javascript"></script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="/CTI/CTITool.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.3"></script>
    <script type="text/javascript">
        //展开收缩
        function divShowHideEvent(divId, obj) {
            if ($("#" + divId).css("display") == "none") {
                $("#" + divId).show("slow");
                $(obj).attr("class", "toggle");
            }
            else {
                $("#" + divId).hide("slow");
                $(obj).attr("class", "toggle hide");
            }
        }
         
    </script>
    <script type="text/javascript">
        var uCCustHelper = (function () {
            var IsHaveAuthForMember = function () {
                $.post("MemberCheckHandler.ashx", { Action: "Checkauth", OriginalDMSMemberID: '<%= this.OriginalDMSMemberID %>', OriginalCSTRecID: '<%= this.OriginalCSTRecID %>' }, function (data) {
                    if (data != "success") {
                        $.jAlert(data, function () {
                            location.href = "MemberCheckList.aspx";
                        })
                    }
                });
            },
              loadContacts = function () {//显示客户联系人
                  $('#divCustContacts').load('../CustInfo/MoreInfo/CC_Contact/List.aspx', {
                      ContentElementId: 'divCustContacts',
                      TID: '<%= this.TID%>'
                  });
              },
            loadTaskLog = function () {
                $('#divTaskLog').load('../CustInfo/MoreInfo/CrmMemberLog.aspx', {
                    ContentElementId: 'divCustContacts',
                    OriginalDMSMemberID: '<%= this.OriginalDMSMemberID %>'
                });
            },
       loadCallRecord = function () {
           $('#divCallRecordList').load('../CustInfo/DetailV/TaskCallRecordList.aspx', {
               ContentElementId: 'divCustContacts',
               TID: '<%= this.TID %>',
               PageSize: 10
           });
       },
            loadCooperationProjects = function () {//显示客户合作项列表
                $('#divCooperationProjectList').load('../CustInfo/MoreInfo/CooperationProjectList.aspx', {
                    ContentElementId: 'divCooperationProjectList',
                    CustID: '<%=Task.CrmCustID %>',
                    IsPartShow: true
                });
            },
            loadBusinessLicense = function () {//显示客户年检列表
                $('#divBusinessLicense').load('/CustInfo/MoreInfo/BusinessLicenseList.aspx', {
                    ContentElementId: 'divBusinessLicense',
                    CustID: '<%=Task.CrmCustID %>'
                });
            },

        init = function () {
            //判断用户是否有权限访问此会员
            IsHaveAuthForMember();
            //得到当前会员区域
            loadContacts();
            loadTaskLog();
            loadCallRecord();
            if ('<%=Task.CrmCustID %>' != '') {
                $('#liCustCooperationProject').show();
                $('#liCustBusinessLicense').show();
                //载入客户合作项列表
                loadCooperationProjects();
                //载入客户年检列表
                loadBusinessLicense();
            }
        };

            return {
                init: init
            }
        })();

        function EditMemberInfo(action) {
            //$('#divMembers').find('.MemberInfoArea').size() > 0 
            if ($('#divMembers').find('.MemberInfoArea').size() > 0 || $('#divCSTMembers').find('.MemberInfoArea').size() > 0) {
                var divMap = $('div[id$="divMap"]');
                var lat = $('input[id$="hdnMap"]').data('marker_lat');
                var lng = $('input[id$="hdnMap"]').data('marker_lng');
                var pody = {
                    Action: encodeURIComponent(action),
                    //车易通编辑信息
                    MemberID: encodeURIComponent($.trim($('input[id$="tfMemberID"]').val())),
                    MemberName: encodeURIComponent($.trim($('input[id$="tfMemberName"]').val())),
                    MemberAbbr: encodeURIComponent($.trim($('input[id$="tfMemberAbbr"]').val())),
                    MemberType: encodeURIComponent($.trim($('select[id$="selMemberType"]').val())),
                    //Phone: encodeURIComponent($.trim($('input[id$="tfPhone"]').val())),
                    Phone: encodeURIComponent($(":text[id*='tfPhone'][value!='']").map(function () { return $.trim($(this).val()); }).get().join(",")), //改为3个输入框，存放联系电话
                    Brand: encodeURIComponent($.trim($('input[id$="tfBrand"]').val())),
                    ProvinceID: encodeURIComponent($.trim($('select[id$="selProvince"]').val())),
                    CityID: encodeURIComponent($.trim($('select[id$="selCity"]').val())),
                    CountyID: encodeURIComponent($.trim($('select[id$="selCounty"]').val())),
                    IsHasCounty: ($('select[id$="selCounty"]').attr('noCounty') == undefined ? 0 : $('select[id$="selCounty"]').attr('noCounty')),
                    Address: encodeURIComponent($.trim($('input[id$="tfAddress"]').val())),
                    Longitude: lng ? lng : '',
                    Lantitude: lat ? lat : '',
                    //Fax: encodeURIComponent($.trim($('input[id$="tfFax"]').val())),
                    Fax: encodeURIComponent($(":text[id*='tfFax'][value!='']").map(function () { return $.trim($(this).val()); }).get().join(",")), //改为3个输入框，存放传真
                    CompanyWebSite: encodeURIComponent($.trim($('input[id$="tfCompanyWebSite"]').val())),
                    Email: encodeURIComponent($.trim($('input[id$="tfEmail"]').val())),
                    Postcode: encodeURIComponent($.trim($('input[id$="tfZipcode"]').val())),
                    SerialIds: encodeURIComponent($.trim($('input[id$="txtSerialIds"]').val())),
                    TrafficInfo: encodeURIComponent($.trim($('input[id$="tfTrafficInfo"]').val())),
                    EnterpriseBrief: encodeURIComponent($.trim($('textarea[id$="tfEnterpriseBrief"]').val())),
                    Notes: encodeURIComponent($.trim($('textarea[id$="tfNotes"]').val())),
                    SpanMemberName: encodeURIComponent($.trim($('span[id$="spanMemberName_"]').html())),

                    //车商通编辑信息
                    CstMemberID: encodeURIComponent($.trim($('input[id$="tfCstMemberID"]').val())),
                    CstMemberFullName: encodeURIComponent($.trim($('input[id$="tfCstMemberFullName"]').val())),
                    CstMemberShortName: encodeURIComponent($.trim($('input[id$="tfCstMemberShortName"]').val())),
                    CstMemberType: encodeURIComponent($.trim($('select[id$="selCstMemberType"]').val())),
                    //CstVendorCode: encodeURIComponent($.trim($('input[id$="tfVendorCode"]').val())),
                    CstMemberProvince: encodeURIComponent($.trim($('select[id$="selCstMemberProvince"]').val())),
                    CstMemberCity: encodeURIComponent($.trim($('select[id$="selCstMemberCity"]').val())),
                    CstMemberCounty: encodeURIComponent($.trim($('select[id$="selCstMemberCounty"]').val())),
                    CstSuperId: encodeURIComponent($.trim($('select[id$="sltSuperId"]').val())),
                    CstMemberAddress: encodeURIComponent($.trim($('input[id$="tfCstMemberAddress"]').val())),
                    CstMemberPostCode: encodeURIComponent($.trim($('input[id$="tfCstMemberPostCode"]').val())),
                    //                    CstTrafficInfo: encodeURIComponent($.trim($('input[id$="tfTrafficInfo"]').val())),
                    //                    CstMemberBrand: encodeURIComponent($.trim($('input[id$="tfCstMemberBrand"]').val())),
                    OriginalCSTRecID: encodeURIComponent($.trim($('input[id$="tfRecID"]').val())),

                    CstLinkMan: encodeURIComponent($.trim($('input[id$="sltCstLinkMan"]').val())),
                    CstLinkManName: encodeURIComponent($.trim($('input[id$="tfLinkManName"]').val())),
                    //                    CstLinkManDepartment: encodeURIComponent($.trim($('input[id$="tfLinkManDepartment"]').val())),
                    //                    CstLinkManPosition: encodeURIComponent($.trim($('input[id$="tfLinkManPosition"]').val())),
                    CstLinkManMobile: encodeURIComponent($.trim($('input[id$="tfLinkManMobile"]').val())),
                    CstLinkManEmail: encodeURIComponent($.trim($('input[id$="tfLinkManEmail"]').val()))
                };
                $.ajax({
                    type: "POST",
                    url: "MemberCheckHandler.ashx?callback=?",
                    data: pody,
                    beforeSend: function () {
                        $("#divButton").css("display", "none");
                    },
                    success: function (data) {
                        if (data == 'success') {
                            if (action == "saveCheckInfo") {
                                $.jAlert('保存成功！ ');
                                $("#divButton").css("display", "block");
                            }
                            else {
                                $.jAlert('提交成功！ ', function () {
                                    closePageExecOpenerSearch();
                                });
                            }
                        }
                        else {
                            $.jAlert('错误: ' + data);
                            $("#divButton").css("display", "block");
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $.jAlert('错误: ' + XMLHttpRequest.responseText);
                        $("#divButton").css("display", "block");
                    }
                });
            }
        }
        if ('<%=MemberType %>' != '0') {
            function deleteMemberInfo() {
                if (confirm("确定要删除吗？")) {
                    $.post('MemberCheckHandler.ashx?callback=?', { Action: "deleteCheckInfo", OriginalCSTRecID: encodeURIComponent($.trim($('input[id$="tfRecID"]').val())), CstMemberID: encodeURIComponent($.trim($('input[id$="tfCstMemberID"]').val())) }, function (data) {
                        if (data == 'success') {
                            $.jPopMsgLayer("删除成功", function () {
                                closePageExecOpenerSearch();
                            });
                        }
                        else {
                            $.jAlert('错误: ' + data);
                        }
                    });
                }
            }
        }
        else if ('<%=MemberType %>' != '1') {
            function deleteMemberInfo() {
                if (confirm("确定要删除吗？")) {
                    $.post('MemberCheckHandler.ashx?callback=?', { Action: "deleteCheckInfo", MemberID: encodeURIComponent($.trim($('input[id$="tfMemberID"]').val())), CstMemberID: encodeURIComponent($.trim($('input[id$="tfCstMemberID"]').val())) }, function (data) {
                        if (data == 'success') {
                            $.jAlert("删除成功", function () {
                                location.href = "MemberCheckList.aspx";
                            });
                        }
                        else {
                            $.jAlert('错误: ' + data);
                        }
                    });
                }
            }
        }

        $(function () {
            uCCustHelper.init();
            //            GMapService.loadMapJs(); //加载google地图相关脚本，完成后回调相应方法队列
            //控制展开和收起
            $('img.shouqizhankai').toggle(
          function () {
              $(this).attr({ src: '/images/zhankai.gif', title: '展开' })
              .next().hide("normal");
          },
          function () {
              $(this).attr({ src: '/images/shouqi.gif', title: '收起' })
            .next().show("normal");

          }
        );
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="w980">
        <div class="taskT">
            会员二次核实查看<span></span></div>
        <div class="baseInfo">
            <div class="title">
                基本信息<a href="javascript:void(0)" onclick="divShowHideEvent('divBaseInfo',this)" class="toggle"></a>
            </div>
            <div id="divBaseInfo">
                <ul class="clearfix">
                    <li>
                        <label>
                            客户ID：</label>
                        <span id="spanCustName" name="CustName" runat="server" /></li>
                    <li>
                        <label>
                            客户简称：</label>
                        <span id="spanCustAbbr" name="CustAbbr" runat="server" /></li>
                    <li>
                        <label>
                            客户类别：</label>
                        <span id="spanCustType" name="CustType" runat="server" />
                        <input runat="server" id="hdnCustType" style="display: none;" />
                        <span id="custid" name="custid" runat="server" style="display: none;" /></li>
                    <li>
                        <label>
                            客户行业：</label>
                        <span id="spanCustIndustry" name="CustIndustry" runat="server" /></li>
                    <li>
                        <label>
                            客户地区：</label>
                        <span id="spanArea" name="CustIndustry" runat="server" /></li>
                    <li>
                        <label>
                            注册地址：</label>
                        <span id="spanAddress" name="Address" runat="server" /></li>
                    <li>
                        <label>
                            邮政编码：</label>
                        <span id="spanZipcode" name="Zipcode" runat="server" /></li>
                    <li id="liCustStatus" runat="server" style="display: none;">
                        <label>
                            客户状态：</label>
                        <%if (imgStatusLock == 1) %>
                        <%{ %>
                        <img style="margin-left: 5px;" id="imgCustStatus" runat="server" />
                        <%} %>
                    </li>
                    <li id="liCustLock" runat="server" style="display: none;">
                        <label>
                            锁定：</label>
                        <%if (imgLock == 1) %>
                        <%{ %>
                        <img style="margin-left: 5px;" id="imgCustStatusLock" runat="server" />
                        <%} %>
                    </li>
                    <li>
                        <label>
                            客户级别：</label>
                        <span id="spanCustLevel" name="CustLevel" runat="server" /></li>
                    <%if (CarType != 2)
                      { %>
                    <li id="liCustPid" runat="server">
                        <label>
                            所属厂商：</label>
                        <span id="spanCustPidName" name="CustPidName" runat="server" /></li>
                    <li id="liPid" runat="server">
                        <label>
                            所属集团：</label>
                        <span id="spanPidName" name="PidName" runat="server" /></li>
                    <li>
                        <label>
                            经营店级别：</label>
                        <span id="spanShopLevel" name="ShopLevel" runat="server" /></li>
                    <li>
                        <label>
                            主营品牌：</label>
                        <span id="spanBrandName" name="BrandName" runat="server" /></li>
                    <%} %>
                    <li>
                        <label>
                            电话：</label>
                        <span id="spanOfficeTel" name="OfficeTel" runat="server" /></li>
                    <li>
                        <label>
                            传真：</label>
                        <span id="spanFax" name="Fax" runat="server" /></li>
                    <li>
                        <label>
                            联系人：</label>
                        <span id="spanContactName" name="ContactName" runat="server" /></li>
                    <li class="singleRow" style="width: 700px">
                        <label>
                            备注：</label>
                        <span id="spanNotes" name="Notes" runat="server" class="exceed" style="width: 560px;" />
                    </li>
                </ul>
            </div>
            <div class="title contact">
                客户联系人<a href="javascript:void(0)" onclick="divShowHideEvent('divCustContacts',this)"
                    class="toggle"></a></div>
            <div id="divCustContacts" class="fullRow  cont_cxjg" style="margin: 0 18px;">
            </div>
            <div class="title contact">
                合作项列表<a href="javascript:void(0)" onclick="divShowHideEvent('divCooperationProjectList',this)"
                    class="toggle"></a></div>
            <div id="divCooperationProjectList" class="fullRow  cont_cxjg" style="margin: 0 8px;">
            </div>
            <div class="title contact">
                年检列表<a href="javascript:void(0)" onclick="divShowHideEvent('divBusinessLicense',this)"
                    class="toggle"></a></div>
            <div id="divBusinessLicense" class="fullRow  cont_cxjg" style="margin: 0 8px;">
            </div>
            <div id="divMembers">
                <asp:PlaceHolder ID="PlaceHolder" runat="server"></asp:PlaceHolder>
            </div>
            <div id="divCSTMembers">
                <asp:PlaceHolder ID="CSTPlaceHolder" runat="server"></asp:PlaceHolder>
            </div>
            <div class="title contact" style="clear: both;">
                操作记录<a href="javascript:void(0)" onclick="divShowHideEvent('divTaskLog',this)" class="toggle"></a></div>
            <div id="divTaskLog" class="fullRow  cont_cxjg" style="margin: 0 18px;">
            </div>
            <div class="title contact">
                通话记录<a href="javascript:void(0)" onclick="divShowHideEvent('divCallRecordList',this)"
                    class="toggle"></a></div>
            <div id="divCallRecordList" class="fullRow  cont_cxjg" style="margin: 0 18px;">
            </div>
            <div class="btn" id="divButton" <%=btnDisplay %>>
                <input type="button" id="BtnDel" onclick="deleteMemberInfo()" class="button" style="margin-right: 50px;"
                    value="删除" />
                <input type="button" id="btnConfirm" onclick="EditMemberInfo('saveCheckInfo');" class="button"
                    value="保存" />
                <input type="button" id="Button2" onclick="EditMemberInfo('submitCheckInfo');" class="button"
                    value="提交" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
