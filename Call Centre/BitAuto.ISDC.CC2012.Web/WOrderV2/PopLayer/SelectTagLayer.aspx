<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectTagLayer.aspx.cs"
    Inherits="BitAuto.ISDC.CC2012.Web.WOrderV2.PopLayer.SelectTagLayer" %>

<script type="text/javascript">
    $(function () {
        //默认选中第一个
//        $(".Menubox_left ul li").first().attr("class", "hover");
//        $(".Contentbox_right div").first().attr("class", "hover").css("display", "block");

        $(".Contentbox_right div li").click(function () {
            //样式
            $(this).siblings().attr("class", ""); //其他的取消选中
            $(this).attr("class", "current"); //选中

            //数据
            var tagid = $(this).attr("tagid");
            var tagname = $(this).attr("tagname");
            var data = { tagid: tagid, tagname: tagname };

            $.closePopupLayer('SelectTagLayer', true, data);
        });
    });

    function setTab(name, cursel, n) {

        for (i = 1; i <= n; i++) {
            var menu = document.getElementById(name + i);
            var con = document.getElementById("con_" + name + "_" + i);
            menu.className = i == cursel ? "hover" : "";
            con.style.display = i == cursel ? "block" : "none";
        }
    }

    function SelectClear() {
        $(".Contentbox_right div li").attr("class", "");
        $.closePopupLayer('SelectTagLayer', true, { tagid: "", tagname: "" });
    }

</script>
<form id="form1" runat="server">
<div class="pop_new w900 openwindow">
    <div class="title">
        <h2>
            标签选择
        <span><a href="javascript:void(0)" onclick="javascript:$.closePopupLayer('SelectTagLayer',false);">
        </a></span></h2>
    </div>
    <div class="biaoqian_xz">
        <div class="Menubox_left">
            <asp:literal id="Lit_left" runat="server"></asp:literal>
        </div>
        <div class="Contentbox_right">
            <asp:literal id="Lit_right" runat="server"></asp:literal>
        </div>
    </div>
    <div class="clearfix">
    </div>
    <div class="option_button option_button2 btn">
        <input name="" type="button" onclick="javascript:$.closePopupLayer('SelectTagLayer',false);"
            value="取消" /><span style="position: relative; *top: -4px;"><a href="javascript:void(0)" onclick="SelectClear()">清空已选项</a></span>
    </div>
    <div class="clearfix">
    </div>
</div>
</form>