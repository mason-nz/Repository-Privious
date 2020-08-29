$(function () {

    LoadRoles();
    var UserID = getQueryString("UserID");
    if (UserID != null) {
        LoadUserInfo(UserID);
    }
    var employeeNumber = "";
    $('#employeeNumber').on('blur', function () {
        //查询 click  
    })
    $('#selectMessage').on('click', function () {
        employeeNumber = $('#employeeNumber').val();
        if (employeeNumber == "") {
            alert("编号不能为空");
            return;
        }
        ClearTxt();
        GetEmployee(employeeNumber);

    });
    $('#commit').on('click', function () {
        var result = VaildInsert(employeeNumber)
        if (result != "Success") {
            alert(result);
        }
        else {
            AddUserInfo(employeeNumber);
        }
    })
    $('#returnBtn').on('click', function () {
        $(window).attr('location', 'http://j.chitunion.com/userinfo/touseroperatelist');
    })
})
function getQueryString(name) {
    console.log(name);
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
function GetEmployee(employeeNumber) {
    $.ajax({
        type: "POST",
        url: "Handler1.ashx",
        //我们用text格式接收
        dataType: "json",
        data: { "Method": "SelectEmployee", "EmployeeNumber": employeeNumber },
        success: function (result) {
            if (result != null) {
                $('#cnName').val(result.CnName);
                $('#phone').val(result.Mobile);
                $('#email').val(result.Email);
                $('#employeeName').val(result.DomainAccount);
                $('#cnName').attr("SysUserID", result.EmployeeID);
            } else {
                alert("该员工编号不存在");
            }
        }
    });

}
function LoadRoles() {
    $.ajax({
        type: "POST",
        url: "Handler1.ashx",
        //我们用text格式接收
        dataType: "json",
        data: { "Method": "SelectRole" },
        success: function (result) {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    $("#userRole").append("<option SysID=" + result[i].SysID + " value=" + result[i].RoleID + ">" + result[i].RoleName + "</option>");
                }
            }
        }
    });
}
function LoadUserInfo(UserID) {
    $.ajax({
        type: "POST",
        url: "Handler1.ashx",
        //我们用text格式接收
        dataType: "json",
        data: { "Method": "SelectUserInfo", "UserID": UserID },
        success: function (result) {
            if (result != null) {
                $('#employeeNumber').attr("UserID", result[0].UserID)
                $('#employeeNumber').val(result[0].EmployeeNumber);
                $('#cnName').val(result[0].TrueName);
                $('#phone').val(result[0].Mobile);
                $('#email').val(result[0].Email);
                $('#employeeName').val(result[0].UserName);
                $('#userRole').val(result[0].RoleID);
            }
        }
    });
}


function ClearTxt() {
    $('#cnName').val("");
    $('#phone').val("");
    $('#email').val("");
    $('#employeeName').val("");
    $('#cnName').attr("SysUserID", "");
}
function VaildInsert(employeeNumber) {
    var checkValue = $("#userRole").val();
    if (checkValue == -1) {
        return "请选择角色";
    }
    if ($('#employeeNumber').attr("UserID") <= 0)
    {
        if (employeeNumber == "") {
            return "请先获取用户信息";
        }
        if (($('#cnName').val() == "" && $('#phone').val() == "" && $('#email').val() == "")) {
            return "请先获取用户信息";
        }
        if (($('#cnName').val() == "" || $('#phone').val() == "" || $('#email').val() == "")) {
            return "该用户不可用";
        }
    }
    return "Success"
}
//function D() {
//   return { "Method": "Add", "EmployeeNumber": employeeNumber, "Mobile": $('#phone').val(), "Email": $('#email').val(), "UserName": $('#employeeName').val(""), "SysID": $("#userRole option:Selected").attr("SysID"), "RoleID": $("#userRole").val(), "SysUserID": "Add", "TrueName": $('#cnName').val() }
//}
function AddUserInfo(employeeNumber) {
    $.ajax({
        type: "POST",
        url: "Handler1.ashx",
        //我们用text格式接收
        dataType: "text",
        data: { "Method": "Add", "UserID": $('#employeeNumber').attr("UserID"), "EmployeeNumber": employeeNumber, "Mobile": $('#phone').val(), "Email": $('#email').val(), "UserName": $('#employeeName').val(), "SysID": $("#userRole option:Selected").attr("SysID"), "RoleID": $("#userRole").val(), "SysUserID": $('#cnName').attr("SysUserID"), "TrueName": $('#cnName').val() },
        success: function (result) {
            alert(result);
            if (result == "保存成功") {
                ClearTxt();
                $('#employeeNumber').val("");
                $("#userRole").val(-1);
                $('#employeeNumber').focus();

            }
           
        }
    });
}