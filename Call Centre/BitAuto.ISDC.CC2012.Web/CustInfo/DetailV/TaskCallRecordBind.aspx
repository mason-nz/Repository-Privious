<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskCallRecordBind.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.CustInfo.DetailV.TaskCallRecordBind" %>

<script type="text/javascript">
       

    //查询客户
    function BindRecordCustSearch() {
        var txtCustName=$.trim($('#txtCustName').val());
        if (txtCustName=='') {
            $.jAlert("客户名称不能为空");return;
        }
        if (GetStringRealLength(txtCustName)>100) {
            $.jAlert("客户名称不能超过100字符");return;
        }
        $('#spanCustID').html('');$('#hidCustID').val('');
        $('#spanMemberCode').html('');
        var strTID='<%=RequestTID %>';
        var postBody = { Action: 'SearchCust',CustName:escapeStr(txtCustName),TID:strTID, DataSource:<%=DataSource %> };
        AjaxPost('/AjaxServers/RecordMonitor/RecordBindManager.ashx', postBody, null, function (data) {
            var jsonData = $.evalJSON(data);
            
            if(jsonData.Search=='NotExistByCustName'||jsonData.Search=='no')
            {
                var ddlMember=$('#ddlMember')[0];
                ddlMember.options.length = 0;
                ddlMember.options.add(new Option('选择会员', '-1'));
                $.jAlert("客户名称在库中没有找到"); return;
            }
            else if(jsonData.Search=='NotExistMember')
            {
                var ddlMember=$('#ddlMember')[0];
                ddlMember.options.length = 0;
                ddlMember.options.add(new Option('选择会员', '-1'));
                $.jAlert("此客户下没有会员！"); return;
            }
            else if(jsonData.Search=='ExistMoreCust')
            {
                var ddlMember=$('#ddlMember')[0];
                ddlMember.options.length = 0;
                ddlMember.options.add(new Option('选择会员', '-1'));
                $.jAlert("客户名称在Excel导入库中存在多条"); return;
            }
            else if(jsonData.Search=='yes')
            {
              if(jsonData.DataSource=='2'){   $('#spanCustID').html(jsonData.CustID);}
                $('#hidCustID').val(jsonData.CustID);
                //$('#hidDataSource').val(jsonData.DataSource);
                var ddlMember=$('#ddlMember')[0];
                ddlMember.options.length = 0;
                ddlMember.options.add(new Option('选择会员', '-1'));
                if(jsonData.member!=null)
                {
                $.each(jsonData.member,function(i,n){
                    ddlMember.options.add(new Option(unescape(n.name), n.id));
                });
                }
            }
        });
    }

    function ddlMemberOnChange() {
        var v = $('#ddlMember').val();
        if (v.split('|').length>1) {
            $('#spanMemberCode').html(v.split('|')[1]);
        }
        else
        { $('#spanMemberCode').html(''); }
    }

    //提交操作
    function BindRecordSubmit() {
        var memberName= $('#ddlMember')[0].options[$('#ddlMember')[0].selectedIndex].text;    
        var m = $('#ddlMember').val().split('|');
        var custID = $.trim($('#hidCustID').val());
        var cc_custID='';//导入Excel客户ID
        if($.trim($('#spanCustID').html())=='')
        {
            cc_custID=custID;
            custID='';
        }
        //var hidDataSource=$('#hidDataSource').val();
        if ($('#ddlMember').val()=='-1') {
            $.jAlert("您必须选择一个会员"); return;
        }
        else {
            $.jConfirm('您确定要将<br/>会员ID：' + ($.trim(m[1])==''?m[0]:m[1]) + '，会员名称：'+memberName+'，<br/>绑定到此录音上吗？', function (result) {
            if (result) {
                var postBody = { Action: 'BindRecord',
                                 CustID:custID, 
                                 CC_CustID:cc_custID,
                                 MemberType:m[2],
                                 MemberID:($.trim(m[0])!=''&&!checkNum(m[0])?m[0]:''),
                                 CC_MemberID:($.trim(m[0])!=''&&checkNum(m[0])?m[0]:''),
                                 CRID:<%=RequestCRID %>,TID:'<%=RequestTID %>',SessionID:'<%=RequestSessionID%>' };
                AjaxPost('/AjaxServers/RecordMonitor/RecordBindManager.ashx', postBody, null, function (data) {
                    var jsonData = $.evalJSON(data);
                    if(jsonData.Bind=='yes')
                    {
                        $.jPopMsgLayer("绑定成功",function(){
                        $.closePopupLayer('BindCallRecordPopup',true);
                        }); 
                    }
                    else 
                    {
                       $.jAlert("绑定失败",function(){
                        $.closePopupLayer('BindCallRecordPopup',false);
                        }); 
                    }
                });
                }
            });
        }
    }
</script>
<div class="pop pb15 openwindow">
    <div class="title bold">
        <h2>
            录音绑定</h2>
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('BindCallRecordPopup',false);">
        </a></span>
    </div>
    <span>提示：绑定当前客户下已有会员直接在会员列表里选择，如要绑定新添加会员，需要先保存核实信 息再选择绑定；更改绑定客户，请在输入客户<span style="color: Red;">全称</span>后点击更改后再选择会员
        如新增客户核实时，客户名称在CRM库中存在，录音只能绑定至CRM库同名客户下
        <br />
        <span style="color: Red;">录音与客户ID为一一对应关系，点击确定后即刻生效，请绑定时确保信息正确</span></span>
    <ul class="clearfix  outTable">
        <li style="width: 360px">
            <label style="width: 60px">
                客户名称：</label><em style="color: Red; float: left;">*</em>
            <input type="text" class="w190" id="txtCustName" name="txtCustName" runat="server" />&nbsp;&nbsp;<input
                type="button" style="height: 22px" onclick="javascript:BindRecordCustSearch();"
                value="查询" />
        </li>
        <li style="width: 190px; height: 25px;">
            <label style="width: 60px">
                客户编号：</label>
            <span id="spanCustID" name="spanCustID" runat="server"></span>
            <input id="hidCustID" runat="server" type="hidden" />
        </li>
        <li style="width: 360px">
            <label style="width: 60px">
                会员名称：</label><em style="color: Red; float: left;">*</em>
            <select id="ddlMember" name="ddlMember" runat="server" style="width: 190px" onchange="javascript:ddlMemberOnChange();">
                <%--<option value="-1">选择会员</option>--%>
            </select>
        </li>
        <li style="width: 190px; height: 25px;">
            <label style="width: 60px">
                会员编号：</label>
            <span id="spanMemberCode" name="spanMemberCode"></span></li>
    </ul>
    <div class="btn" style="margin: 20px 10px 10px 0px;">
        <input type="button" onclick="javascript:BindRecordSubmit();" value="提交" class="btnSave bold" />
        <input type="button" onclick="javascript:$.closePopupLayer('BindCallRecordPopup',false);"
            value="取消" class="btnSave bold" />
    </div>
    <%--<input id="hidDataSource" runat="server" type="hidden" />--%>
</div>
