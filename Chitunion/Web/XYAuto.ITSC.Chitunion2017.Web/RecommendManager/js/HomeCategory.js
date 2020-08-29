$(function () {

    $('#Upload,#again').click(function () {
        $.openPopupLayer(
         {
             name: "popLayerDemo",
             url: "./HomeCategory.aspx?" + "CategoryID=" + $("div").data("CategoryID") + "&MediaTypeID=" + $("div").data("MediaTypeID"),
             error: function (dd) { alert("添加失败"); },
             success: function () {
                 $(".button").click(
                     function () {
                         if (null == $('#u306_input option').val()) {
                             alert('请选择分类！');
                             return false;
                         }
                         if ($('#u306_input option').length < 8) {
                             var IsOK = window.confirm('确定在首页只显示' + $('#u306_input option').length + '个分类吗？');
                             if (!IsOK) {
                                 return;
                             }
                         }
                         if ($('#u306_input option').length >8) {
                             alert("最多选择8个分类，请修改已选分类！")
                             return;
                         }
                         var Cid = "";
                         $("#u306_input option").each(
                             function myfunction(i, o) {
                                 Cid = Cid + o.value + "#" + o.text + "^";
                             }),
                             AjaxPost("Handler1.ashx", { "Cid": Cid, "MediaTypeID": $("div").data("MediaTypeID") }, function (result) {
                                 if (result != "添加分类成功") {
                                     alert(result);
                                 }
                                 $.closePopupLayer('popLayerDemo');
                                 window.location.reload();
                             });


                     });
                 $('.but_keep').click(
                     function () {
                         $.closePopupLayer('popLayerDemo');
                     }
                     );
                 $('#closebt').click(
                   function () {
                       $.closePopupLayer('popLayerDemo');
                   }
                   );

             }

         });
    });
});
function AjaxPost(url, postBody, CallbackName) {
    $.ajax({
        type: "POST",
        url: url,
        data: postBody,
        success: CallbackName,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中
            // 只有一个会包含信息
            alert(XMLHttpRequest.responseText);
        }
    });
}