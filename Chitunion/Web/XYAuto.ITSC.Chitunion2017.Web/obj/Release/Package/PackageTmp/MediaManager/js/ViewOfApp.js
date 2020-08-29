$(function(){
    var CheckMoudle = new ComponentSection({});
    var WSearch = window.location.search.substr(1);
    var SearchArray = WSearch.split("&");
    var SearchObject = {};
    SearchArray.forEach(function(item){
        var temp = item.split("=");
        SearchObject[temp[0]] = temp[1];
    });
    var PubID = SearchObject.PubID ? SearchObject.PubID : "";
    var MediaType = SearchObject.MediaType ? SearchObject.MediaType : "";
    var ADDetailID = SearchObject.ADDetailID ? SearchObject.ADDetailID :"";
    var MediaID = SearchObject.MediaID ? SearchObject.MediaID : "";
    

    // 获取媒体信息方法
    CheckMoudle.getMediaData = function(){
        var url = "/api/Media/GetMediaDetail?MediaType="+CheckMoudle.State.MediaType+"&MediaID="+CheckMoudle.State.MediaID
        setAjax({
            url:url,
            type:"get"
        },function(data){
            CheckMoudle.setState({
                "MediaInfo":data.Result.MediaInfo,
                "InteractionInfo":data.Result.InteractionInfo,
                "UserInfo":data.Result.UserInfo,
                "OverlayIDs":data.Result.OverlayIDs
            },function(){
                CheckMoudle.getKanliTime();
            });
        });
    };
    // 获取广告位信息
    CheckMoudle.getKanliInfo = function(){
        var url = "/api/Periodication/GetAppPublishInfoByAdvID?ADDetailID="+CheckMoudle.State.ADDetailID;
        setAjax({
            url:url,
            type:"get"
        },function(data){
            CheckMoudle.setState({
                "Kanli":data.Result
            },function(){
                CheckMoudle.createAllList();
            });
        });
    };
    // 获取刊例信息
    CheckMoudle.getKanliTime = function(){
        var url = "/api/Periodication/GetPublishInfoBymediaTypeAndPubID?MediaType="+CheckMoudle.State.MediaType+'&PubID='+CheckMoudle.State.PubID;
        setAjax({
            url:url,
            type:"get"
        },function(data){
            CheckMoudle.setState({
                "time":data.Result
            },function(){
               CheckMoudle.getKanliInfo();
            });
        });
    };
     // 渲染刊例审核列表
    CheckMoudle.createAllList = function(){
        switch(CheckMoudle.State.MediaType){
            case "14002":
                if(CheckMoudle.State.Kanli){
                    CheckMoudle.Render("#CheckTemplate","#CheckView");
                    layerImg();
                }else{
                    // layer.alert('刊例不存在',function(){
                    //     window.close();
                    // });
                }
                
            break;
            default:
                
            break;
        }
    };
    
    CheckMoudle.setState({
        "PubID":PubID,
        "MediaType":MediaType,
        "ADDetailID":ADDetailID,
        "MediaID":MediaID
    },function(){
        CheckMoudle.getMediaData();
    });

    function layerImg(){
        /*图例弹层*/
        $("#CheckView").on("click",".picDemo",function () {
            var url  = $(this).attr('picurl');
            $.openPopupLayer({
                name:'popLayerDemo',
                url:'./layer.html',
                error:function (dd) {
                    alert(dd.status)
                },
                success:function () {
                    $('.layer_con2 img').attr('src',url);
                    $('.layer_con2').html('<img src="'+url+'" width="350" height="420">');

                    $('#popupLayerScreenLocker').click(function () {
                        $.closePopupLayer('popLayerDemo')
                    })

                }
            });
        });
    }
});