<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="BitAuto.ISDC.CC2012.WebAPI.Test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="Radio1" name="business" type="radio" runat="server" />
        <asp:Label ID="Label1" runat="server" Text="惠买车"></asp:Label>
        <input id="Radio2" name="business" type="radio" runat="server" /><asp:Label ID="Label2"
            runat="server" Text="汽车金融"></asp:Label>
        <input id="Radio3" name="business" type="radio" runat="server" /><asp:Label ID="Label3"
            runat="server" Text="企业"></asp:Label>
        <input id="Radio4" name="business" type="radio" runat="server" /><asp:Label ID="Label4"
            runat="server" Text="易车商城"></asp:Label>
        <br />
        <asp:Label ID="Label5" runat="server" Text="主叫"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server" Style="margin-bottom: 0px">18629257531</asp:TextBox>
        <asp:Label ID="Label6" runat="server" Text="被叫"></asp:Label>
        <asp:TextBox ID="TextBox2" runat="server" Style="margin-bottom: 0px">02987237676</asp:TextBox>
        <br />
        <asp:Label ID="Label7" runat="server" Text="开始时间"></asp:Label>
        <asp:TextBox ID="TextBox3" runat="server" Style="margin-bottom: 0px">2015-6-15 09:31:31</asp:TextBox>
        <asp:Label ID="Label8" runat="server" Text="结束时间"></asp:Label>
        <asp:TextBox ID="TextBox4" runat="server" Style="margin-bottom: 0px">2015-6-15 09:38:18</asp:TextBox>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="3.1	验证黑白名单接口" OnClick="Button1_Click" />
        <br />
        <br />
        <asp:Label ID="Label9" runat="server" Text="路径"></asp:Label>
        <asp:TextBox ID="TextBox5" runat="server" Width="493px">/var/spool/hollyRecord/2015/06/3001/</asp:TextBox>
        <br />
        <asp:Label ID="Label10" runat="server" Text="名称"></asp:Label>
        <asp:TextBox ID="TextBox6" runat="server" Width="491px">3001_20150611161443831.mp3</asp:TextBox>
        <br />
        <asp:Label ID="Label15" runat="server" Text="厂家id"></asp:Label>
        <asp:TextBox ID="TextBox11" runat="server" Width="227px">20150611111301189_1_047_UIP-1</asp:TextBox>
        <br />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="3.2	语音留言接口" />
        <br />
        <br />
        <asp:Label ID="Label11" runat="server" Text="callid"></asp:Label>
        <asp:TextBox ID="TextBox7" runat="server">30021433992425459</asp:TextBox>
        <br />
        <asp:Label ID="Label14" runat="server" Text="满意度"></asp:Label>
        <asp:TextBox ID="TextBox10" runat="server">13</asp:TextBox>
        <br />
        <asp:Button ID="Button3" runat="server" Text="3.3	满意度调查接口" OnClick="Button3_Click" />
        <br />
        <br />
        <asp:Label ID="Label12" runat="server" Text="厂家id"></asp:Label>
        <asp:TextBox ID="TextBox8" runat="server" Width="227px">20150611111301189_1_047_UIP-1</asp:TextBox>
        <br />
        <asp:Label ID="Label13" runat="server" Text="按键顺序"></asp:Label>
        <asp:TextBox ID="TextBox9" runat="server">1*23*0</asp:TextBox>
        <br />
        <asp:Button ID="Button4" runat="server" Text="3.4	业务信息推送接口" OnClick="Button4_Click" />
        <br />
        <br />
        <asp:Button ID="Button7" runat="server" Text="3.5 专属坐席" 
            OnClick="Button7_Click" />
        <br />
        链接： <a id="link" runat="server" href="" target="_blank"></a>
        <br />
        <br />
    </div>
    <div>
        <asp:RadioButton ID="RadioButton1" GroupName="QueryType" runat="server" Checked="True"
            Text="主被叫" />
        <asp:RadioButton ID="RadioButton2" GroupName="QueryType" runat="server" Text="呼入主叫" />
        <asp:RadioButton ID="RadioButton3" GroupName="QueryType" runat="server" Text="呼出被叫" />
        <asp:RadioButton ID="RadioButton4" GroupName="QueryType" runat="server" Text="话务" />
        <br />
        &nbsp;主叫：<asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>
        <br />
        &nbsp;被叫：<asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
        <br />
        &nbsp;话务ID:<asp:TextBox ID="TextBox14" runat="server"></asp:TextBox>
        <br />
        &nbsp;TOP:<asp:TextBox ID="TextBox15" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="Button5" runat="server" OnClick="Button5_Click" Text="查询话务大数据" />
        <br />
        <br />
        链接： <a id="link2" runat="server" href="" target="_blank"></a>
        <br />
        <br />
        <asp:Button ID="Button6" runat="server" OnClick="Button6_Click" Text="查询任务ID" />
        <br />
        链接： <a id="link3" runat="server" href="" target="_blank"></a>
    </div>
    </form>
</body>
</html>
