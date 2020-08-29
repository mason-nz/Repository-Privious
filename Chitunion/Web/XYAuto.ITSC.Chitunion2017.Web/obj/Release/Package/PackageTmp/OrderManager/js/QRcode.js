//获取Url上的参数
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {

            theRequest[strs[i].split("=")[0]] = strs[i].split("=")[1];
        }
    }
    return theRequest;
}
$(function () {
    function implement() {
        setAjax({
            url:'http://www.chitunion.com/api/ADOrderInfo/GetTwoBarCodeHistory?v=1_1',
            type:'get',
            data:{
                OrderID:GetRequest().OrderID
            }
        },function (data) {
            if(data.Status==0){
                var arrid=[];
                for(var i=0;i<data.Result.TwoBarCodeHistory.length;i++){
                    arrid.push(data.Result.TwoBarCodeHistory[i].MediaID);
                }
                $(".button").attr("arrid",arrid);
                $("#Projectnumber").html(data.Result.OrderID);
                $("#ProjectName").html(data.Result.OrderName);
                $('table').html(ejs.render($('#QR').html(), data));
                operation();
            }else {
                layer.msg(data.Message);
            }
        })
    }
    implement();
    
    function operation() {
        $('.button').off('click').on('click',function () {
            var OrderId=$('#Projectnumber').html();
            var url=$('#urlAddress').val();
            if(!(/^https?:\/\/(([a-zA-Z0-9_-])+(\.)?)*(:\d+)?(\/((\.)?(\?)?=?&?[a-zA-Z0-9_-](\?)?)*)*$/i.test(url))){
               layer.msg('请输入正确的url');
               return false;
            }
            var item=[];
            for(var i=0;i<$(this).attr('arrid').split(',').length;i++){
                item.push({
                    "OrderId": OrderId,
                    "MediaType": 14001,
                    "MediaId": $(this).attr('arrid').split(',')[i]-0,
                    "Url": url
                })
            }
            setAjax({
                url:'http://www.chitunion.com/api/ADOrderInfo/Generate?v=1_1',
                type:'post',
                data: {
                    "Item":item
                }
            },function (data) {
                if(data.Status==0){
                    implement();
                }else {
                    layer.msg(data.Message)
                }
            })
        });
        $('.Regenerate').off('click').on('click',function () {
            var OrderId=$('#Projectnumber').html();
            var url=$(this).parents('td').prev().prev().find('input').val();
            if(!(/^https?:\/\/(([a-zA-Z0-9_-])+(\.)?)*(:\d+)?(\/((\.)?(\?)?=?&?[a-zA-Z0-9_-](\?)?)*)*$/i.test(url))){
                layer.msg('请输入正确的url');
                return false;
            }
            setAjax({
                url:'http://www.chitunion.com/api/ADOrderInfo/Generate?v=1_1',
                type:'post',
                data: {
                    "Item":[
                        {
                            "OrderId": OrderId,
                            "MediaType": 14001,
                            "MediaId": $(this).attr('arrid')-0,
                            "Url": url
                        }
                    ]
                }
            },function (data) {
                if(data.Status==0){
                    implement();
                    layer.msg('生成成功');
                    $('#allbox').prop('checked', false);
                }else {
                    layer.msg(data.Message)
                }
            })
        })
        $('.download').off('click').on('click',function () {
            var _this=$(this);
            $.ajax({
                url: "http://www.chitunion.com/api/ADOrderInfo/DownloadZip?v=1_1",
                type: 'get',
                data: {
                    ids:$(this).attr('arrid')
                },
                dataType: 'text',
                xhrFields: {
                    withCredentials: true
                },
                crossDomain: true,
                success: function (data) {
                    // console.log(data);
                    // window.location.href=data;
                    // alert(data);
                    if(data!=null){
                        window.location='http://www.chitunion.com/api/ADOrderInfo/DownloadZip?v=1_1&ids='+_this.attr('arrid');
                    }
                }
            });

        })

        /*全选 全不选按钮*/
        $('.box').on('change', '#allbox', function () {
            if ($(this).prop('checked')) {
                $('.onebox').prop('checked', true);
            } else {
                $('.onebox').prop('checked', false);
            }
        });
        /*单选按钮*/
        $('.box').on('change', '.onebox', function () {
            if ($('.onebox').length == $("input[name='checkbox']:checked").length) {
                $('#allbox').prop('checked', true);
            } else {
                $('#allbox').prop('checked', false);
            }
        });
        $('#PackageDownload').off('click').on('click',function () {
            var arrid=[];
            $('.onebox').each(function () {
                if($(this).prop('checked')==true){
                    arrid.push($(this).attr('arrid'));
                }
            })
            if(arrid.length){
                $.ajax({
                    url: "http://www.chitunion.com/api/ADOrderInfo/DownloadZip?v=1_1",
                    type: 'get',
                    data: {
                        ids:arrid.join(',')
                    },
                    dataType: 'text',
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,
                    success: function (data) {
                        // console.log(data);
                        // window.location.href=data;
                        // alert(data);
                        if(data!=null){
                            window.location='http://www.chitunion.com/api/ADOrderInfo/DownloadZip?v=1_1&ids='+arrid.join(',')
                        }
                    }
                });
            }else {
                layer.msg("请选择媒体");
            }
        })
    }

})