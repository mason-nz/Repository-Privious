<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InsideAddUser.aspx.cs" Inherits="XYAuto.ITSC.Chitunion2017.Web.UserInfoManage.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="/css/resetNew_editor.css" />
    <link rel="stylesheet" type="text/css" href="/css/layoutNew_editor.css" />
    <script type="text/javascript" src="../js/jquery.1.11.3.min.js"></script>
    <script language="javascript" src="/api/check.ashx?NotCheckModule=true"
        type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.browser.js"></script>
    <script type="text/javascript" src="../js/tabNew.js"></script>
    <script type="text/javascript" src="../js/ejs.min.js"></script>
    <script type="text/javascript" src="../js/Common_chitu.js"></script>
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../js/laydate.js"></script>
    <script type="text/javascript" src="../js/layer/layer.js"></script>
    <script type="text/javascript" src="../js/common_authority.js"></script>
    <script src="../js/jquery.jmpopups-0.5.1.pack.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.pagination.js"></script>
    <script type="text/javascript" src="../js/jquery.jmpopups-0.5.1.pack.js"></script>
    <script src="InsideUser.js"></script>
    <title></title>
</head>
<body>
    <!--顶部logo 导航-->
    <!--#include file="../Base/header.html" -->

    <form id="form1" runat="server">
        <div class="order">
            <!--左侧菜单-->
            <!--#include file="../Base/Menu.html" -->
            <div class="order_r" style="margin-left: 140px;">
                <div class="install">
                    <input id="employeeID" type="hidden" />
                    <input id="isNew" type="hidden" value="true" />
                    <input id="userID" type="hidden" value="" />
                    <input id="oldRoleID" type="hidden" value="" />
                    <ul>
                        <li class="ins_a"><span class="red f12">*</span> 员工编号：</li>
                        <li>
                            <input id="employeeNumber" UserID="0" type="text" value="" maxlength="6" onkeyup="value=value.replace(/[^\d]/g,'')" style="width: 315px;" /><input id="checkNumber" type="hidden" /></li>

                        <li><a class="blue3" id="selectMessage">查询</a></li>

                        <div class="clear"></div>
                    </ul>
                    <ul>
                        <li class="ins_a"><span class="red f12">*</span> 真实姓名：</li>
                        <li>
                            <input id="cnName" SysUserID="" type="text" value="" disabled="" style="width: 315px;"></li>
                        <li class="red">
                            <!-- <img src="/images/icon21.png"> 请输入真实姓名-->
                        </li>
                        <div class="clear"></div>
                    </ul>
                    <ul>
                        <li class="ins_a"><span class="red f12">*</span> 手机号：</li>
                        <li>
                            <input id="phone" type="text" disabled="" style="width: 315px;">
                            <%--<input id="mobile" type="hidden" value="">--%>
                        </li>
                        <li class="red">
                            <!--<img src="/images/icon21.png"> 请输入手机号-->
                        </li>
                        <div class="clear"></div>
                    </ul>
                    <ul>
                        <li class="ins_a"><span class="red f12">*</span> 邮箱：</li>
                        <li>
                            <input id="email" type="text" value="" disabled="" style="width: 315px;"></li>
                        <li class="red">
                            <!--<img src="/images/icon21.png"> 请输入邮箱-->
                        </li>
                        <div class="clear"></div>
                    </ul>
                    <ul>
                        <li class="ins_a"><span class="red f12">*</span> 用户名：</li>
                        <li>
                            <input id="employeeName" type="text" value="" disabled="" style="width: 315px;"></li>
                        <li class="red">
                            <!--<img src="/images/icon21.png"> 请输入邮箱-->
                        </li>
                        <div class="clear"></div>
                    </ul>
                    <ul>
                        <li class="ins_a"><span class="red f12">*</span> 角色：</li>
                        <li>
                            <select id="userRole"  style="width: 327px; line-height: 30px">
                                <option value="-1">请选择角色</option>
                            </select>
                        </li>
                        <li id="checkRole" class="red" style="display: none;">
                            <img src="http://www.chitunion.com/imagesNew/icon21.png" />
                            请选择角色</li>
                        <div class="clear"></div>
                    </ul>
                    <ul>
                        <li class="ins_a">&nbsp;</li>

                        <li><a class="button" id="commit" style="width: 150px">提交</a></li>

                        <li><a class="but_keep" id="returnBtn" style="width: 150px; margin-top: 15px">返回</a></li>
                        <div class="clear"></div>
                    </ul>



                    <div class="clear"></div>
                </div>
            </div>
            <div class="clear"></div>
        </div>
    </form>
    <!--底部-->
    <!--#include file="../Base/footer.html" -->
</body>
</html>
