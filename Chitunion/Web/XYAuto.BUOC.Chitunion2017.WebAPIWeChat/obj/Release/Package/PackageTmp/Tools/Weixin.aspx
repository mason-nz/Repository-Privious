<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Weixin.aspx.cs" Inherits="XYAuto.BUOC.Chitunion2017.WebAPIWeChat.Tools.Weixin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        请选择一个公众号：<select id="ddlWxNum" runat="server">
            <option value="-1">请选择</option>
            <option selected="selected" value="wxf0eea6fec2756b45|2e35c3d7acb3219b6d77415267d06975">开发环境测试号</option>
            <option value="wx59f2e219a0cbd545|5a5dce76a576e28ccc707603da7ca78f">测试环境测试号</option>
        </select>
        <div>
            生成永久推广二维码：<br />

            请输入推广场景名称：<input runat="server" type="text" id="txtActivityName" /><br />
            <asp:Button ID="btnGenQrCodeUrl" runat="server" Text="生成推广场景二维码" OnClick="btnGenQrCodeUrl_Click" />
            <asp:Literal ID="litActivityQrCodeUrl" runat="server" Visible="false"></asp:Literal><br />
            <img id="imgActivityQrCode" runat="server" visible="false" />

        </div>


        <div>
            生成用户场景二维码：<br />

            请输入用户手机号：<input runat="server" type="text" id="txtMobile" /><br />
            <asp:Button ID="btnGenQrCodeUrl2" runat="server" Text="生成用户场景二维码" OnClick="btnGenQrCodeUrl2_Click" />
            <asp:Literal ID="litActivityQrCodeUr2" runat="server" Visible="false"></asp:Literal><br />
            <img id="imgActivityQrCod2" runat="server" visible="false" />
        </div>
        
        
        <div>
            生成永久MediaID图片：<br />

            请上传图片：<asp:FileUpload ID="FileUpload1" runat="server" /><br />
            <asp:Button ID="btnGenImg" runat="server" Text="生成永久MediaID图片" OnClick="btnGenImg_Click" />
            <asp:Literal ID="litGenImg" runat="server" Visible="false"></asp:Literal><br />
            <img id="imgForver" runat="server" visible="false" />
        </div>
    </form>
</body>
</html>
