<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetExamPaperList.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.ExamOnline.ExamObject.GetExamPaperList" %>
<script type="text/javascript">

    //查询试卷信息
    function SearchEm() {
        LoadingAnimation('EmList');
        var txtQueryEmployeeCnName = $('#txtQueryEmployeeCnName');
        var EmName = "";
        if ($.trim(txtQueryEmployeeCnName.val()) == '请输入试卷名') {
            txtQueryEmployeeCnName.val('');
            EmName = "";
        }
        else {
            EmName = $.trim(txtQueryEmployeeCnName.val());
        }
        //分类
        var Catage = "";
        var list = $(":input[name='paperCatage'][checked=true]");
        $(list).each(function () {
            Catage = Catage + $(this).val() + ",";
        });
        Catage = Catage.substring(0, Catage.lastIndexOf(','));
        var pody = {
            PaperName: encodeURIComponent(EmName),
            Catage: escape(Catage),
            BGID:escape($("#selPaperBG").val()),
            R: Math.random()
        }
        $('#divQueryEmployeeList').load('GetExamPaperList.aspx #divQueryEmployeeList > *', pody);
    }

    function SelectEmployeeAppendToTable(eid, name, number, departname) {
        var htmlStr = "<tr id='tr_" + eid + "'>"
                            + "<td><a href='javascript:delSelectedEmployee(" + eid + ")'>删除</a></td>"
                            + "<td>" + number + "</td>"
                            + "<td><input id='hdn_" + eid + "' type='hidden' value=" + eid + " />" + name + "</td>"
                            + "<td>" + departname + "</td></tr>";
        $("#SelectedEmployees").append(htmlStr);
        
        
    }

    //移除所选
    function delSelectedEmployee(eid) {
        $("#tr_" + eid).remove();
    }

    //提交选择的试卷
    function SubmitSelectEmployee() {
        var selectedEm = $('#tableCustBrandSelect tbody tr');
        var length = selectedEm.length;
        var names = "";
        var marks = "";
        if (length > 1) {
            for (var i = 1; i < length; i++) {
                names += $.trim(selectedEm.eq(i).find("td").eq(1).text()) + ",";
                marks += $.trim(selectedEm.eq(i).find("td").eq(2).find("span").text()) + ",";
            }
            names = names.substring(0, names.length - 1);
            marks = marks.substring(0, marks.length - 1);
            $.closePopupLayer('SelectExamPaper', true, names + ";" + marks);
        }
        else {
            $.jAlert('请至少选择一套试卷！');
        }
    }

    //设置表格行的双击事件
    function SetTableDbClickEvent() {
        $('#tableBrandList tr:gt(0)').dblclick(function () {
            SelectCustBrand($(this).find('a[href][id]').attr('id'), $(this).find('a[href][id]').attr('name'));
        });
    }
    //删除操作
    function DelSelectCustBrand(brandid) {
        $('#tableCustBrandSelect tr:even').removeClass('color_hui');
        $('#tableCustBrandSelect tr').removeAttr('style');
        $("#tableCustBrandSelect a[id='" + brandid + "'][href]").parent().parent().remove();
        $('#tableCustBrandSelect tr:even').addClass('color_hui'); //设置列表行样式
        SetTableStyle('tableCustBrandSelect');
    }
    //选择操作
    function SelectCustBrand(brandID, name) {
        var idsame = $('#tableCustBrandSelect tbody tr').find('a[id="' + brandID + '"]');
        var parentNodes = $("a[id='" + brandID + "']").parent().parent().children().clone();
        var brandName = $.trim(parentNodes.eq(2).html());
        if (idsame.size() > 0) {
            $.jAlert('您已经添加过：' + name + '了！');
            return;
        }

        var tableCustBrandSelect = $('#tableCustBrandSelect tr');
        if (tableCustBrandSelect.length > 1) {
            tableCustBrandSelect.eq(1).remove();
        }

        var trhtml = '<tr class="back" onmouseout="this.className=\'back\'" onmouseover="this.className=\'hover\'\">'
        + '<td><a href="javascript:DelSelectCustBrand(\'' + brandID + '\');"  name=\'' + name + '\' id=\'' 
        + brandID + '\'><img src="/Images/close.png" title="删除"/></a></td>'
               + '<td>' + parentNodes.eq(1).html() + '</td>'
               + '<td>' + parentNodes.eq(2).html() + '</td>'
               + '<td>' + parentNodes.eq(3).html() + '</td>'
               + '<td>' + parentNodes.eq(4).html() + '</tr>';
        $('#tableCustBrandSelect tbody tr:last').after(trhtml);
        SetTableStyle('tableCustBrandSelect');

        SubmitSelectEmployee();
    }
    //清空
    function ClearEmployee() {
        $("input[name=QueryByCnName]").removeAttr('checked');
    }

    $(document).ready(function () {

        enterSearch(SearchEm);
        //SearchEm();
    });
    //分页操作
    function ShowDataByPost1(pody) {
        LoadingAnimation('divQueryEmployeeList');
        $('#divQueryEmployeeList').load('GetExamPaperList.aspx #divQueryEmployeeList > *', pody);
    }
</script>
<div class="pop pb15 openwindow" style=" background:#FFF; width:650px;">
    <div class="title bold">
        <h2>试卷选择</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SelectExamPaper',false);"></a></span>
    </div>
    <div class="more" id="closebox" style="float:right;" onclick="javascript:$.closePopupLayer('SelectExamPaper',false);">
    </div>    
        <div id='divQueryByEmployee'>
            <div class="infor_li renyuan_cx">
                <ul class="clearfix  outTable">                    
                    <li style=" margin-right:0; width:370px;">
                        <label style=" width:70px;">分类：</label>
                        <span>
                        <asp:Repeater ID="Rpt" runat="server">
                            <ItemTemplate>
                                <input name="paperCatage" type="checkbox" value='<%#Eval("ECID") %>' /><em class="dx"><%#Eval("Name") %></em>&nbsp; 
                            </ItemTemplate>
                        </asp:Repeater>             
                        </span>
                    </li>
                    <li style=" width:200px;" >
                        <b>试卷：</b>
                        <input type="text" value="请输入试卷名" onfocus="javascript:var name=document.getElementById('txtQueryEmployeeCnName');if(name.value=='请输入试卷名')name.value='';"
                            name="txtQueryEmployeeCnName" id="txtQueryEmployeeCnName" class="w190" style=" width:120px;"/>
                    </li> 
                    <li style=" width:250px;" >
                        <label style=" width:70px;">所属分组：</label>
                        <span>
                         <select id="selPaperBG"  style=" margin:5px 0px;*margin:5px 0px; max-width:160px;*max-width:160px;*width:160px;">
                             <asp:Repeater id="rp_BGData" runat="server">
                                      <ItemTemplate>
                                        <option value='<%#Eval("BGID")%>'><%#Eval("Name")%></option>
                                      </ItemTemplate>
                             </asp:Repeater> 
                         </select>
                         </span>
                    </li>                   
                    <li class="btn">
                        <input type="button" style=" margin:5px 0px;*margin:5px 0px; " value="查询" class="btnSave bold"  onclick="javascript:SearchEm();"/>
                    </li>
                </ul>
            </div>
            
            <div id="divQueryEmployeeList">
                <div class="Table2" id="EmList">
                    <table width="100%" cellspacing="0" cellpadding="0" id="tableQueryEmployee" class="tableList mt10 mb15">
                        <tr>
                            <th style="width: 10%;">
                                选择
                            </th>
                            <th style="width: 40%;">
                                试卷名称
                            </th>
                            <th style="width: 15%;">
                                所属分类
                            </th>                                
                            <th style="width: 10%;">
                                创建人                                
                            </th>
                            <th style="width: 25%;">
                                创建时间                                
                            </th>
                        </tr>
                        <tbody>
                    <asp:repeater id="Rpt_ExamPaper" runat="server">
                    <ItemTemplate>
                    <tr>
                        <td>
                            <a class="linkBlue" onclick="SelectCustBrand('<%# Eval("EPID") %>','<%# Eval("Name").ToString()%>');" name='<%# Eval("Name").ToString()%>' id='<%# Eval("EPID") %>' style=" cursor:pointer;">选择</a>
                        </td>
                        <td>
                            <%#Eval("Name").ToString()%>
                        </td>
                        <td>
                            <span style=" display:none;"><%# Eval("EPID").ToString()%></span> <label><%# GetCatageName(Eval("ECID").ToString()) %></label>
                        </td>                        
                        <td >
                            <%#getCreateUserName(Eval("CreaetUserID").ToString())%>
                        </td>
                        <td>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("CreateTime").ToString(), "yyyy-MM-dd")%>&nbsp;
                        </td>
                    </tr>                   
                    </ItemTemplate>
                    </asp:repeater>
                    </tbody>
                    </table>
                </div>
                <div class="it_page" style=" text-align:right;">
                    <asp:literal runat="server" id="litPagerDown"></asp:literal>&nbsp;&nbsp;&nbsp;
                </div>
            </div>
        
        </div>
        
        <br/><div style=" clear:both;"></div>
        
        <!--已选人员列表-->
        <div class="Table2List mt10 mb15" id="divCustBrandSelect" style=" display:block;">
            <table cellspacing="0" cellpadding="0" border="0" class="Table2List mt10 mb15" id="tableCustBrandSelect" style=" margin-left:0px; width:100%;">
                <tbody>
                    <tr>
                        <th style="width: 10%;">
                                选择
                            </th>
                            <th style="width: 40%;">
                                试卷名称
                            </th>
                            <th style="width: 15%;">
                                所属分类
                            </th>                                
                            <th style="width: 10%;">
                                创建人                                
                            </th>
                            <th style="width: 25%;">
                                创建时间                                
                            </th>
                    </tr>
                    <%=LoadSelectedEmployees()%>
                </tbody>
            </table>
            <div style=" clear:both;"></div>          
        </div>
        <div class="btn" style=" margin:10px 0 10px 0">
            <input type="button" onclick="javascript:SubmitSelectEmployee();" class="btnSave bold" value="确定">            
        </div>
</div>