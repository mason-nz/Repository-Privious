<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamScoreList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.AjaxServers.ExamOnline.ExamScoreManage.ExamScoreList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<table cellpadding="0" cellspacing="0" class="tableList mt10 mb15" width="99%" id="tableList">
    <tr class="back" onmouseout="this.className='back'">
        <th style='width: 8%;'>
            考生姓名
        </th>
        <th style='width: 10%;'>
            所属分组
        </th>
        <th style='width: 12%;'>
            考试项目
        </th>
        <th style='width: 12%;'>
            试卷名称
        </th>
        <th style='width: 5%;'>
            单选
        </th>
        <th style='width: 5%;'>
            复选
        </th>
        <th style='width: 5%;'>
            判断
        </th>
        <th style='width: 5%;'>
            主观
        </th>
        <th style='width: 8%;'>
            考试成绩
        </th>
        <th style='width: 10%;'>
            考试时间
        </th>
        <th style='width: 6%;'>
            是否缺考
        </th>
        <th style='width: 7%;'>
            操作
        </th>
    </tr>
    <asp:repeater id="repeaterTableList" runat="server">
                <ItemTemplate>                 
                    <tr class="back" onmouseover="this.className='hover'" onmouseout="this.className='back'">                      
                        <td   style='width: 8%;' title='<%#Eval("Truename")%>'>
                                <%#Eval("Truename")%>&nbsp;
                        </td>
                        <td   style='width: 10%;' title='<%#Eval("BGName")%>'>
                                 <%#Eval("BGName")%>&nbsp;                             
                        </td>
                        <td   style='width: 12%;' title='<%#Eval("ProjectName").ToString()%>'>
                                 <%#Eval("ProjectName").ToString()%>&nbsp;                             
                        </td>
                         <td   style='width: 12%;' title='<%#Eval("Papername")%>'>
                                 <%#Eval("Papername")%>&nbsp;                             
                        </td>
                         <td   style='width: 5%;' title='<%#Eval("Onlyselect")%>'>
                                 <%#Eval("Onlyselect")%>&nbsp;                             
                        </td>
                         <td   style='width: 5%;' title='<%#Eval("moreselect")%>'>
                                 <%#Eval("moreselect")%>&nbsp;
                        </td>
                         <td   style='width: 5%;' title='<%#Eval("panduan")%>'>
                                 <%#Eval("panduan")%>&nbsp;
                        </td>
                         <td   style='width: 5%;' title='<%#Eval("zhuguan")%>'>
                                 <%#Eval("zhuguan")%>&nbsp;
                        </td>
                         <td   style='width: 8%;' title='<%#Eval("sumscore")%>'>
                                 <%#Eval("sumscore")%>&nbsp;
                        </td>                           
                        <td    style='width: 10%;' title='<%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("begintime").ToString(), "yyyy-MM-dd")%>'>
                            <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("begintime").ToString(), "yyyy-MM-dd")%>&nbsp;
                        </td>
                        <td    style='width: 6%;' title='<%#Eval("lack")%>'>
                            <%#Eval("lack")%>&nbsp;
                        </td>
                        <td style='width: 7%;'>
                           <%#Deal(Eval("EIID").ToString(), Eval("IsMaKeUp").ToString(), Eval("ExamPersonID").ToString(), Eval("endtime"), Eval("isMarking").ToString(), Eval("Examepid").ToString())%>&nbsp;                          
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:repeater>
</table>
<br />
<!--分页-->
<div class="pages1" style="text-align: right;">
    <uc:AjaxPager ID="AjaxPager_Custs" runat="server" ContentElementId="ajaxTable" />
</div>
