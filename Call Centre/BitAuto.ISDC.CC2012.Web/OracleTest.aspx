<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OracleTest.aspx.cs" Inherits="BitAuto.ISDC.CC2012.Web.OracleTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Js/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        document.writeln("<s" + "cript type='text/javascript' src='/Js/config.js?r=" + Math.random() + "'></scr" + "ipt>");
    </script>
    <script src="../Js/jquery.jmpopups-0.5.1.pack.js" charset="utf-8" type="text/javascript"></script>
    <script type="text/javascript">
        loadJS("common");
        loadJS("CTITool");
        loadJS("ucCommon");
        loadJS("controlParams");
        loadJS("bitdropdownlist");
        loadJS("UserControl");
    </script>
    <script src="../Js/json2.js" type="text/javascript"></script>
    <script src="../Js/Enum/Area2.js" type="text/javascript"></script>
    <script src="../Js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="/Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        function aaa() {
            //var str = "UserEvent=Transferred&UserName=80182&CalledNum=81412&CallerNum=18682902973&CallID=801821436231815296&UserChoice=&CallType=33&MediaType=MediaVoice&CallState=0&CurrentDate=%u0032%u0030%u0031%u0035%u002D%u0030%u0037%u002D%u0030%u0037%u0020%u0030%u0039%u003A%u0031%u0036%u003A%u0035%u0035&SYS_DNIS=02968211101&outBoundType=0";
            var str = "UserEvent=Transferred&UserName=80182&CalledNum=81412&CallerNum=18686960679&CallID=801821436236269303&UserChoice=&CallType=33&MediaType=MediaVoice&CallState=0&CurrentDate=%u0032%u0030%u0031%u0035%u002D%u0030%u0037%u002D%u0030%u0037%u0020%u0031%u0030%u003A%u0033%u0031%u003A%u0030%u0039&SYS_DNIS=02968211101&outBoundType=0";
            MethodScript(str);
        }
        function chekno(data) {
            var pos = data.CallerNum.indexOf("0");
            var len = data.CallerNum.length;
            //alert("phone: " + data.CallerNum + "len: " + len + "pos: " + pos);

            //手机号判断规则：大于等于11位，倒数第11位是1，倒数第10位不是0
            if (len >= 11 && data.CallerNum.charAt(len - 11) == '1' && data.CallerNum.charAt(len - 11 + 1) != '0') {
                data.CallerNum = data.CallerNum.substr(len - 11, 11);
                //alert("手机号：" + data.CallerNum);
            }
            //区号座机判断规则：存在0，且第一个0之后（含0）的长度大于等于10
            else if (pos >= 0 && len - pos >= 10) {
                data.CallerNum = data.CallerNum.substr(pos);
                //alert("区号座机：" + data.CallerNum);
            }
            //非区号座机：不是手机号，不带区号的情况下，大于8位
            else if (len > 8) {
                data.CallerNum = data.CallerNum.substr(len - 8, 8);
                //alert("座机：" + data.CallerNum);
            }
            else {
                //alert("无处理：" + data.CallerNum);
            }

            if (data.CallerNum.substr(0, 2) == "00") {
                //00开头 去掉一个0
                data.CallerNum = data.CallerNum.substr(1);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="button" onclick="aaa()" name="aaa" value="1111" />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查询分页" />
        <br />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="查询全部" />
        <br />
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="查询空" />
        <br />
        <asp:Button ID="Button5" runat="server" OnClick="Button5_Click" Text="数据入库hour" />
        <asp:Button ID="Button7" runat="server" OnClick="Button7_Click" Text="查询时间hour" />
        <br />
        <br />
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <asp:Button ID="Button8" runat="server" OnClick="Button8_Click" Text="开始项目" />
        <asp:Button ID="Button9" runat="server" OnClick="Button9_Click" Text="结束项目" />
        <br />
        <asp:Button ID="Button10" runat="server" OnClick="Button10_Click" Text="开始项目" />
        <asp:Button ID="Button11" runat="server" OnClick="Button11_Click" Text="结束项目" />
        <br />
        <asp:TextBox ID="TextBox3" runat="server" TextMode="MultiLine" Width="208px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="Button12" runat="server" OnClick="Button12_Click" Text="新建活动" />
        <asp:Button ID="Button14" runat="server" OnClick="Button14_Click" Text="更新" />
        <asp:Button ID="Button13" runat="server" OnClick="Button13_Click" Text="结束易团购活动" />
        <br />
        <asp:Button ID="Button15" runat="server" OnClick="Button15_Click" Text="重置缓存数据" />
        <br />
        <asp:Button ID="Button16" runat="server" OnClick="Button16_Click" Text="IM获取用户" />
        <asp:Button ID="Button17" runat="server" OnClick="Button17_Click" Text="IM获取客户" />
        <br />
        <asp:Button ID="Button18" runat="server" OnClick="Button18_Click" Text="同步合力数据" />
        <asp:Button ID="Button34" runat="server" Text="获取热线表数据" OnClick="Button34_Click" />
        <br />
        <asp:Button ID="Button19" runat="server" OnClick="Button19_Click" Text="查询北京自动外呼项目表" />
        <asp:Button ID="Button20" runat="server" OnClick="Button20_Click" Text="查询北京自动外呼任务表" />
        <asp:Button ID="Button21" runat="server" OnClick="Button21_Click" Text="获取统计表最大时间戳" />
        <asp:Button ID="Button22" runat="server" OnClick="Button22_Click" Text="获取明细表最大时间戳" />
        <br />
        <asp:Button ID="Button23" runat="server" OnClick="Button23_Click" Text="查询管辖分组" />
        <asp:Button ID="Button24" runat="server" OnClick="Button24_Click" Text="查询全部的坐席信息" />
        <br />
        <br />
        <asp:Button ID="Button25" runat="server" Text="读取写入文件" OnClick="Button25_Click" />
        &nbsp;<asp:Button ID="Button26" runat="server" Text="上传坐席日志" OnClick="Button26_Click" />
        &nbsp;<asp:Button ID="Button27" runat="server" Text="下载坐席日志" OnClick="Button27_Click" />
        &nbsp;<asp:Button ID="Button28" runat="server" Text="获取服务器版本号" OnClick="Button28_Click" />
        &nbsp;<asp:Button ID="Button29" runat="server" Text="获取坐席信息" OnClick="Button29_Click" />
        &nbsp;<asp:Button ID="Button30" runat="server" Text="获取外呼黑名单" OnClick="Button30_Click" />
        <br />
        <asp:Button ID="Button31" runat="server" Text="测试个人用户接口" OnClick="Button31_Click" />
        <br />
        <br />
        <asp:Button ID="Button32" runat="server" Text="测试专席坐席服务" OnClick="Button32_Click" />
        <br />
        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
        <asp:Button ID="Button33" runat="server" Text="手机号归属" OnClick="Button33_Click" />
        <br />
        <asp:TextBox ID="TextBox5" runat="server" Width="436px"></asp:TextBox>
        <asp:Button ID="Button37" runat="server" OnClick="Button37_Click" Text="加密" />
        <asp:TextBox ID="TextBox6" runat="server" Width="615px"></asp:TextBox>
        <asp:Button ID="Button44" runat="server" OnClick="Button44_Click" 
            Text="插入话务的接口" />
        <br />
        <asp:Button ID="Button35" runat="server" Text="有电话号码" OnClick="Button35_Click" />
        <asp:Button ID="Button36" runat="server" Text="没有电话" OnClick="Button36_Click" />
        <asp:Button ID="Button38" runat="server" Text="呼入弹屏" OnClick="Button38_Click1" />
        <asp:Button ID="Button39" runat="server" Text="呼出弹屏" OnClick="Button39_Click" />
        <asp:Button ID="Button40" runat="server" Text="回访工单" OnClick="Button40_Click" />
        <asp:Button ID="Button41" runat="server" Text="IM个人" OnClick="Button41_Click" />
        <asp:Button ID="Button42" runat="server" Text="IM经销商" OnClick="Button42_Click" />
        <br />
        <asp:HyperLink ID="HyperLink1" runat="server" Target="_blank">HyperLink</asp:HyperLink>
        <br />
        <asp:Button ID="Button43" runat="server" Text="工单处理" OnClick="Button43_Click" />
        <asp:TextBox ID="TextBox7" runat="server">WO160730122912128</asp:TextBox>
        <br />
        <asp:HyperLink ID="HyperLink2" runat="server" Target="_blank">HyperLink</asp:HyperLink>
        <br />
    </div>
    </form>
</body>
</html>
