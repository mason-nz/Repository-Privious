function QingNiuCallOut(phoneNum) {
   
    try {
        var onInitiatedTime = new Date();
        $.post("/AjaxServers/CTIHandler.ashx", { Action: "uccalloutlog", PhoneNum: phoneNum }, function () {

        });
        $("#hidonInitiatedTime").val(onInitiatedTime.getTime());
        //  alert("青牛呼出:" + phoneNum);

        AgentUCobj.doCallOut(phoneNum, null);
    } catch (e) {
    
        alert("青牛呼出功能不可用");
    }
}
