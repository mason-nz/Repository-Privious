$(function(){
    var CheckMoudle = new ComponentSection({});
    var WSearch = window.location.search.substr(1);
    var SearchArray = WSearch.split("&");
    var SearchObject = {};
    SearchArray.forEach(function(item){
        var temp = item.split("=");
        SearchObject[temp[0]] = temp[1];
    });
    var PubID = SearchObject.PubID=='null'  ? 0:SearchObject.PubID;
    var MediaType = SearchObject.MediaType ? SearchObject.MediaType : "";
    var MediaID = SearchObject.MediaID ? SearchObject.MediaID : "";
    
    

    // 获取媒体信息方法
    CheckMoudle.getMediaData = function(){
        var url = "/api/Media/GetMediaDetail";
        setAjax({
            url:url,
            type:"get",
            data:{
                "MediaType":CheckMoudle.State.MediaType,
                "MediaID":CheckMoudle.State.MediaID
            }
        },function(data){
            CheckMoudle.setState({
                "MediaInfo":data.Result.MediaInfo,
                "InteractionInfo":data.Result.InteractionInfo,
                "UserInfo":data.Result.UserInfo,
                "OverlayIDs":data.Result.OverlayIDs
            },function(){
             if (CheckMoudle.State.MediaType!=14002)
              { 
                CheckMoudle.getKanliInfo();
               }
               else{
                console.log(14002)
                if(CheckMoudle.State.MediaInfo){
                   CheckMoudle.Render("#CheckTemplate","#CheckView");
                   layerImg();
                   $('title').text(CheckMoudle.State.MediaInfo.Name+'-媒体管理-我的赤兔');
                }else{
                    layer.alert('刊例不存在',function(){
                        window.close();
                    });
                }
               }
            });
        });
    };
    // 获取刊例信息
        CheckMoudle.getKanliInfo = function(){
            // console.log(CheckMoudle.State.PubID)
            if (CheckMoudle.State.PubID=="") {CheckMoudle.State.PubID=-1}
            var url = "/api/Periodication/GetPublishInfoBymediaTypeAndPubID";  
            setAjax({
                url:url,
                type:"get",
                data:{
                  "PubID": CheckMoudle.State.PubID,
                  "MediaType" :CheckMoudle.State.MediaType
                },
                selector:"#CheckView"
            },function(data){
                CheckMoudle.setState({
                    "Kanli":data.Result
                },function(){
                    CheckMoudle.ReturnCanRenderData(data.Result.Detail);
                    CheckMoudle.createAllList();
                });
            });
        };
    // 根据刊例信息，得到可渲染数据。
    CheckMoudle.ReturnCanRenderData = function(data){
        var hearder = ['单图文','多图文头条','多图文第二条','多图文3-N条'];
        var BodyData = [{"Price":""},{"Price":""},{"Price":""},{"Price":""}];
        var BodyDataThird = [{"Third":""},{"Third":""},{"Third":""},{"Third":""}];
        var BodyDataThirdSecond = [{"Second":""},{"Second":""},{"Second":""},{"Second":""}];
        var BodyDataSecond = [{"Price":""},{"Price":""},{"Price":""}];
        // console.log(data);
        if(data.length&&MediaType=="14001"){
            for(var i = 0;i < data.length;i++){
                if(data[i].SecondDescrit[0].Combdimension == "6001-7002-8001"||data[i].SecondDescrit[0].Combdimension == "6001-7001-8001" || data[i].SecondDescrit[0].Combdimension == "6001-7001-8002"||data[i].SecondDescrit[0].Combdimension == "6001-7002-8002"){
                   BodyData[0].Price =  data[i].SecondDescrit[0].Price;
                   BodyDataThird[0].Third =  data[i].SecondDescrit[0].Third;
                   BodyDataThirdSecond[0].Second =  data[i].SecondDescrit[0].Second;
                }
                if(data[i].SecondDescrit[0].Combdimension == "6002-7002-8001"||data[i].SecondDescrit[0].Combdimension == "6002-7001-8001" || data[i].SecondDescrit[0].Combdimension == "6002-7001-8002"||data[i].SecondDescrit[0].Combdimension == "6002-7002-8002"){
                   BodyData[1].Price =  data[i].SecondDescrit[0].Price;
                   BodyDataThird[1].Third =  data[i].SecondDescrit[0].Third;
                   BodyDataThirdSecond[1].Second =  data[i].SecondDescrit[0].Second;
                }
                if(data[i].SecondDescrit[0].Combdimension == "6003-7002-8001"||data[i].SecondDescrit[0].Combdimension == "6003-7001-8001" || data[i].SecondDescrit[0].Combdimension == "6003-7001-8002"||data[i].SecondDescrit[0].Combdimension == "6003-7002-8002"){
                   BodyData[2].Price =  data[i].SecondDescrit[0].Price;
                   BodyDataThird[2].Third =  data[i].SecondDescrit[0].Third;
                   BodyDataThirdSecond[2].Second =  data[i].SecondDescrit[0].Second;
                } 
                if(data[i].SecondDescrit[0].Combdimension == "6004-7002-8001"||data[i].SecondDescrit[0].Combdimension == "6004-7001-8001" || data[i].SecondDescrit[0].Combdimension == "6004-7001-8002"||data[i].SecondDescrit[0].Combdimension == "6004-7002-8002"){
                   BodyData[3].Price =  data[i].SecondDescrit[0].Price
                   BodyDataThird[3].Third =  data[i].SecondDescrit[0].Third;
                   BodyDataThirdSecond[3].Second =  data[i].SecondDescrit[0].Second;
                }
            }
        }else if(data.length&&MediaType=="14003"){
            if(data.length>1){
                for(var i = 0; i<data[0].SecondDescrit.length;i++){
                    if(data[0].SecondDescrit[i].Combdimension == "7001-9001"){
                       BodyData[0].Price =  data[0].SecondDescrit[i].Price;
                    }
                    if(data[0].SecondDescrit[i].Combdimension[i] == "7001-9002"){
                       BodyData[1].Price =  data[0].SecondDescrit[i].Price;
                    }
                    if(data[0].SecondDescrit[i].Combdimension == "7001-9003"){
                       BodyData[2].Price =  data[0].SecondDescrit[i].Price
                    }
                }
                for(var j = 0; j<data[1].SecondDescrit.length;j++){
                    if(data[1].SecondDescrit[j].Combdimension == "7002-9001"){
                       BodyDataSecond[0].Price =  data[1].SecondDescrit[j].Price;
                    }
                    if(data[1].SecondDescrit[j].Combdimension == "7002-9002"){
                       BodyDataSecond[1].Price =  data[1].SecondDescrit[j].Price;
                    }
                    if(data[1].SecondDescrit[j].Combdimension == "7002-9003"){
                       BodyDataSecond[2].Price =  data[1].SecondDescrit[j].Price;
                    }
                }
            }else{
                 for(var i = 0; i<data[0].SecondDescrit.length;i++){
                    if(data.First=="硬广"){
                         if(data[0].SecondDescrit[i].Combdimension == "7001-9001"||data[0].SecondDescrit[i].Combdimension == "7002-9001"){
                           BodyData[0].Price =  data[0].SecondDescrit[i].Price;
                        }
                        if(data[0].SecondDescrit[i].Combdimension[i] == "7001-9002"||data[0].SecondDescrit[i].Combdimension == "7002-9002"){
                           BodyData[1].Price =  data[0].SecondDescrit[i].Price;
                        }
                        if(data[0].SecondDescrit[i].Combdimension == "7001-9003"||data[0].SecondDescrit[i].Combdimension == "7002-9003"){
                           BodyData[2].Price =  data[0].SecondDescrit[i].Price
                        }
                    }else{
                        if(data[0].SecondDescrit[i].Combdimension == "7001-9001"||data[0].SecondDescrit[i].Combdimension == "7002-9001"){
                           BodyDataSecond[0].Price =  data[0].SecondDescrit[i].Price;
                        }
                        if(data[0].SecondDescrit[i].Combdimension[i] == "7001-9002"||data[0].SecondDescrit[i].Combdimension == "7002-9002"){
                           BodyDataSecond[1].Price =  data[0].SecondDescrit[i].Price;
                        }
                        if(data[0].SecondDescrit[i].Combdimension == "7001-9003"||data[0].SecondDescrit[i].Combdimension == "7002-9003"){
                           BodyDataSecond[2].Price =  data[0].SecondDescrit[i].Price
                        }
                    }
                    
                }
            }
        }else if(data.length&&MediaType=="14004"){
             for(var i = 0; i<data.length;i++){
                if(data[i].SecondDescrit[0].Combdimension == "9001"){
                   BodyData[0].Price =  data[i].SecondDescrit[0].Price;
                }
                if(data[i].SecondDescrit[0].Combdimension == "9002"){
                   BodyData[1].Price =  data[i].SecondDescrit[0].Price;
                }
            }
        }else if(data.length&&MediaType=="14005"){
            for(var i = 0; i<data.length;i++){
                if(data[i].SecondDescrit[0].Combdimension == "10001"){
                   BodyData[0].Price =  data[i].SecondDescrit[0].Price;
                }
                if(data[i].SecondDescrit[0].Combdimension == "10002"){
                   BodyData[1].Price =  data[i].SecondDescrit[0].Price;
                }
            }
        }else{
            BodyData = [];
            BodyDataSecond = [];
        }
        // console.log(BodyData);
        CheckMoudle.setState({
            "Hearder":hearder,
            "BodyData":BodyData,
            'BodyDataSecond':BodyDataSecond,
            'BodyDataThird':BodyDataThird,
            'BodyDataThirdSecond':BodyDataThirdSecond
        },function(){
            CheckMoudle.createAllList();
        });
    };
     // 渲染刊例审核列表
    CheckMoudle.createAllList = function(){
        switch(CheckMoudle.State.MediaType){
            case "14005":
                if(CheckMoudle.State.MediaInfo){
                    CheckMoudle.Render("#CheckTemplate","#CheckView");
                    $('title').text(CheckMoudle.State.MediaInfo.Name+'-媒体管理-我的赤兔');
                    layerImg()
                }else{
                    layer.alert('刊例不存在',function(){
                        window.close();
                    });
                }
            break;
            case "14004":
                if(CheckMoudle.State.MediaInfo){
                    CheckMoudle.Render("#CheckTemplate","#CheckView");
                    layerImg();
                    $('title').text(CheckMoudle.State.MediaInfo.Name+'-媒体管理-我的赤兔');
                }else{
                    layer.alert('刊例不存在',function(){
                        window.close();
                    });
                }
            break;
            case "14003":
                if(CheckMoudle.State.MediaInfo){
                    CheckMoudle.Render("#CheckTemplate","#CheckView");
                    layerImg();
                    $('title').text(CheckMoudle.State.MediaInfo.Name+'-媒体管理-我的赤兔');
                }else{
                    layer.alert('刊例不存在',function(){
                        window.close();
                    });
                }
                
            break;
            case "14001":
                if(CheckMoudle.State.MediaInfo){
                   CheckMoudle.Render("#CheckTemplate","#CheckView");
                   layerImg();
                   $('title').text(CheckMoudle.State.MediaInfo.Name+'-媒体管理-我的赤兔');
                }else{
                    layer.alert('刊例不存在',function(){
                        window.close();
                    });
                }
                
            break;
             case "14002":
                if(CheckMoudle.State.MediaInfo){
                   CheckMoudle.Render("#CheckTemplate","#CheckView");
                   layerImg();
                   $('title').text(CheckMoudle.State.MediaInfo.Name+'-媒体管理-我的赤兔');
                }else{
                    layer.alert('刊例不存在',function(){
                        window.close();
                    });
                }
                 
            break;
            default:
                
            break;
        }
    };
    
    CheckMoudle.setState({
        "PubID":PubID,
        "MediaType":MediaType,
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