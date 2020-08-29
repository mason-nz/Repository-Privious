<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeChatTools.aspx.cs" Inherits="XYAuto.BUOC.Chitunion2017.WebAPIWeChat.WeChatTools" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtMenu" runat="server" Height="23px" Width="472px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnMenu" runat="server" Text="创建菜单" OnClick="btnMenu_Click" />
            <asp:Button ID="btnMenu0" runat="server" Text="清除菜单" OnClick="btnMenu0_Click" />
            <br />
            <br />
            <asp:Literal ID="litlMenu" runat="server" Visible="false"></asp:Literal>
            <br />
            <br />
            <br />
            <br />
            <asp:Button ID="btnUser" runat="server" Text="初始化用户" OnClick="btnUser_Click" />
            <br />
            <br />
            <br />
            <br />
            <asp:Button ID="btnIamge" runat="server" Text="临时图片上传" OnClick="btnIamge_Click" />

            <br />
            <br />
            <br />
            <br />
            <asp:Literal ID="litlsb" runat="server" Visible="false"></asp:Literal>
            <asp:Button ID="btnSB" runat="server" Text="撒币活动二维码" OnClick="btnSB_Click" />

            <br />
            <br />
            <br />
            <br />
            <asp:Literal ID="litlFoot" runat="server" Visible="false"></asp:Literal>
            <asp:Button ID="btnFoot" runat="server" Text="文章详情底部二维码" OnClick="btnFoot_Click" />

            <br />
            <br />
            <br />
            <br />
            <asp:Literal ID="litlArticle" runat="server" Visible="false"></asp:Literal>
            <asp:Button ID="btnArticle" runat="server" Text="文章来源弹窗二维码" OnClick="btnArticle_Click" />


            <br />
            <br />
             <br />
            <asp:Literal ID="Literal1" runat="server" Visible="false"></asp:Literal>
            <asp:Button ID="Button1" runat="server" Text="生成二维码1" OnClick="Button1_Click" />


            <br />

             <br />
            <asp:Literal ID="Literal2" runat="server" Visible="false"></asp:Literal>
            <asp:Button ID="Button2" runat="server" Text="生成二维码2" OnClick="Button2_Click" />


            <br />
            <br />
            <br />
            <asp:Literal ID="litMediaCount" runat="server" Visible="false"></asp:Literal>
            <asp:Button ID="btnGetMediaCount" runat="server" Text="获取永久素材总数" OnClick="btnGetMediaCount_Click" />
            <br />
            <br />
            <asp:Literal ID="litGetOthersMediaList" runat="server" Visible="false"></asp:Literal>
            <select id="ddlGetOthersMediaListMediaType" runat="server">
                <option value="0" selected="selected">image</option>
                <option value="1">voice</option>
                <option value="2">video</option>
                <option value="3">thumb</option>
                <option value="4">news</option>
            </select>
            <asp:Button ID="btnGetOthersMediaList" runat="server" Text="获取图片、视频、语音素材列表" OnClick="btnGetOthersMediaList_Click" />
            <br />
            <br />
            <asp:Literal ID="litAddImageContent" runat="server" Visible="false"></asp:Literal><br />
            商务合作图片地址：<%=LocalWebImage %>hezuo.jpg<br />
            加群交流图片：<%=LocalWebImage %>yaoqing.jpg<br />
            请输入上传图片url：<input type="text" id="txtAddImageUrl" runat="server" /><br />
            <asp:Button ID="btnAddImage" runat="server" Text="上传永久图片" OnClick="btnAddImage_Click" />

            <br />
             <br />
            请输入OpenId起始位置：（为空的话从头开始加载）<input type="text" id="txtNextOpenId" runat="server"/>
            <asp:Button ID="btnGetUserList" runat="server" Text="获取OpenList" OnClick="btnGetUserList_Click" />
            <asp:Literal ID="litGetUserList" runat="server" Visible="false"></asp:Literal>
            <br />
            <br />
            <asp:Literal ID="litlCooperation" runat="server" Visible="false"></asp:Literal>
            <asp:Button ID="btnCooperation" runat="server" Text="商务合作图片" OnClick="btnCooperation_Click" />

            <br />
            <br />
            <br />
            <br />
            <asp:Literal ID="litlCommunication" runat="server" Visible="false"></asp:Literal>
            <asp:Button ID="btnCommunication" runat="server" Text="加群交流图片" OnClick="btnCommunication_Click" />

            <br />
            <br />
            <br />
            <br />
            <asp:Literal ID="litlFootImg" runat="server" Visible="false"></asp:Literal>
            <asp:Button ID="btnFootImg" runat="server" Text="图文消息底部" OnClick="btnFootImg_Click" />
            <br />
            <br />
            <br />
            <br />
            <asp:Literal ID="litlYD" runat="server" Visible="false"></asp:Literal>
            <asp:Button ID="btnYD" runat="server" Text="任务列表引导关注" OnClick="btnYD_Click" />
            <br />
            <br />
            <br />
            <br />
            <asp:Button ID="btnUser2" runat="server" Text="洗微信用户" OnClick="btnUser2_Click" />
            <br />
            <br />
            <br />
            <br />
        </div>


    </form>
</body>
</html>
