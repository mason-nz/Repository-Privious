<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskCallRecordList.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.DetailV.TaskCallRecordList" %>

<!--功能废弃 强斐 2016-8-3-->
<script type="text/javascript">
    function openPlayAudioPopup(audioURL) {
        $.openPopupLayer({
            name: "PlayAudioAjaxPopup",
            parameters: { AudioURL: audioURL },
            popupMethod: 'Post',
            url: "/AjaxServers/AudioManager/PlayAudioLayer.aspx"
        });
    }
    //绑定录音层
    function openBindCallRecordPopup(crid, tid) {
        $.openPopupLayer({
            name: "BindCallRecordPopup",
            parameters: { TID: tid, SessionID: crid },
            popupMethod: 'Post',
            url: "/CustInfo/DetailV/TaskCallRecordBind.aspx",
            afterClose: function (e, data) {
                if (e) {
                    //重新加载通话记录列表
                    $('#divCallRecordList').load('/CustInfo/DetailV/TaskCallRecordList.aspx', {
                        ContentElementId: 'divCustContacts',
                        TID: '<%=TaskID %>',
                        PageSize: 10
                    });
                }
            }
        });
    }
</script>
<table style="width: 100%" border="0" cellspacing="0" cellpadding="0" class="cxjg">
    <tr>
        <th width="29%">
            会员名称
        </th>
        <th width="10%">
            会员ID
        </th>
        <th width="18%">
            通话开始时间
        </th>
        <th width="18%">
            通话结束时间
        </th>
        <th width="10%">
            操作人
        </th>
        <th width="10%">
            播放录音
        </th>
        <th width="5%">
            操作
        </th>
        <%--<th width="20%">播放录音 </th>--%>
    </tr>
    <asp:repeater id="repeater_Contact" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <%#Eval("MemberName")%>
                </td>
                <td>
                    <%#(Eval("MemberCode").ToString()=="0"||Eval("MemberCode").ToString()=="-2")?string.Empty:Eval("MemberCode")%>
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("BeginTime").ToString())%>&nbsp;
                </td>
                <td>
                    <%# BitAuto.ISDC.CC2012.Entities.CommonFunction.GetDateTimeStrForPage(Eval("EndTime").ToString())%>&nbsp;
                </td>
                <td>
                   <%#Eval("trueName").ToString()%>               
                </td>
                <td>
                            <%# Eval("AudioURL").ToString().Trim() == "" ? "" : "<a href='javascript:void(0);' onclick='javascript:ADTTool.PlayRecord(\"" + Eval("AudioURL").ToString() + "\");' title='播放录音' ><img src='../../Images/callTel.png' /></a>"%>&nbsp;
                        </td>
                <td>
                    <% if (IsBind)
                       {%>
                       <a href='javascript:openBindCallRecordPopup("<%#Eval("SessionID") %>","<%=TaskID%>");'>绑定</a>
                    <% }
                       else
                       {%>
                           <span>绑定</span>
                     <%  }%>
                </td>               
            </tr>               
        </ItemTemplate>
    </asp:repeater>
</table>
