$(function(){
    var CheckMoudle = new ComponentSection({});
    var WSearch = window.location.search.substr(1);
    var SearchArray = WSearch.split("&");
    var SearchObject = {};
    SearchArray.forEach(function(item){
        var temp = item.split("=");
        SearchObject[temp[0]] = temp[1];
    });
    var MediaID = SearchObject.MediaID ? SearchObject.MediaID : "";
    var PubID = SearchObject.PubID=='null'  ? 0:SearchObject.PubID;
    var MediaType = SearchObject.MediaType ? SearchObject.MediaType : "";
    // 获取媒体信息方法
    CheckMoudle.getMediaData = function(){
        var url = "/api/Media/GetMediaDetail";
        setAjax({
            url:url,
            type:"get",
            data:{
             "MediaType":  CheckMoudle.State.MediaType,
             "MediaID" :CheckMoudle.State.MediaID
            }
        },function(data){
            CheckMoudle.setState({
                "MediaInfo":data.Result.MediaInfo,
                "InteractionInfo":data.Result.InteractionInfo,
                "UserInfo":data.Result.UserInfo,
                "OverlayIDs":data.Result.OverlayIDs
            },function(){
                CheckMoudle.getKanliInfo();
            });
        });
    };
    // 获取刊例信息
    CheckMoudle.getKanliInfo = function(){
        var url = "/api/Periodication/GetPublishInfoBymediaTypeAndPubID";
        setAjax({
            url:url,
            type:"get",
            data:{
               "PubID": CheckMoudle.State.PubID,
               "MediaType":CheckMoudle.State.MediaType
            },
            selector:"#CheckView"
        },function(data){
            CheckMoudle.setState({
                "Kanli":data.Result
            });
            CheckMoudle.ReturnCanRenderData(data.Result.Detail);
        });
    };
    // 根据刊例信息，得到可渲染数据。
    CheckMoudle.ReturnCanRenderData = function(data){
        var hearder = [];
        data.forEach(function(item){
            hearder.push(item.First);
        });
        var BodyData = [];
        data.forEach(function(item){
            BodyData.push(item.SecondDescrit);
        });
        CheckMoudle.setState({
            "Hearder":hearder,
            "BodyData":BodyData
        },function(){
            CheckMoudle.createAllList();
        });
    };
    // 审核刊例接口
    CheckMoudle.ShenHeFunction = function(){
        setAjax({
            url:"/api/ADOrderInfo/Review_Publish",
            type:"get",
            data:{
                "rejectMsg":CheckMoudle.State.rejectMsg,
                "mediaType":CheckMoudle.State.MediaType,
                "publishID":CheckMoudle.State.PubID,
                "optType":CheckMoudle.State.IsAgreen
            }
            
        },function(data){
            if(data.Status == 0){
                layer.msg(data.Message,{time:1000},function(){
                    switch(CheckMoudle.State.MediaType){
                        case "14005":
                            window.location.href='/PublishManager/pricelist-zhibo.html';
                        break;
                        case "14004":
                            window.location.href = '/PublishManager/pricelist-video.html';
                        break;
                        case "14003":
                            window.location.href = '/PublishManager/pricelist-sina.html'; 
                        break;
                        case "14001":
                            window.location.href = '/PublishManager/pricelist-wechat.html';  
                        break;
                        default:break;
                    }
                });
                
            }else{
                layer.alert("审核未成功，请稍后重试~")
            }
        });
    };

     // 渲染刊例审核列表
    CheckMoudle.createAllList = function(){
        switch(CheckMoudle.State.MediaType){
            case "14005":
                if(CheckMoudle.State.MediaInfo){
                    CheckMoudle.Render("#CheckTemplate","#CheckView",function(){
                        $("#submit").off("click").on("click",function(event){
                            event.preventDefault();
                            CheckMoudle.GetShenheInfo(function(){
                                if(CheckMoudle.State.IsAgreen =='27002'){
                                    if(CheckMoudle.State.rejectMsg){
                                        CheckMoudle.ShenHeFunction();
                                    }else{
                                        layer.msg('驳回原因不能为空',function(){
                                            return ;
                                        });
                                    } 
                               }else{
                                    CheckMoudle.ShenHeFunction();
                               }
                            });
                        });
                        layerImg();
                        $('title').text(CheckMoudle.State.MediaInfo.Name+'-媒体管理-我的赤兔');
                    });
                }else{
                    layer.alert('暂无刊例信息',function(){
                         window.location.href = '/PublishManager/pricelist-zhibo.html';
                    })
                }
            break;
            case "14004":
                if(CheckMoudle.State.MediaInfo){
                    CheckMoudle.Render("#CheckTemplate","#CheckView",function(){
                        
                            $("#submit").off("click").on("click",function(event){
                                event.preventDefault();
                                CheckMoudle.GetShenheInfo(function(){
                                    if(CheckMoudle.State.IsAgreen =='27002'){
                                        if(CheckMoudle.State.rejectMsg){
                                            CheckMoudle.ShenHeFunction();
                                        }else{
                                            layer.msg('驳回原因不能为空',function(){
                                                return ;
                                            });
                                        } 
                                   }else{
                                        CheckMoudle.ShenHeFunction();
                                   }
                                });
                            });
                            layerImg();
                            $('title').text(CheckMoudle.State.MediaInfo.Name+'-媒体管理-我的赤兔');
                    });
                 }else{
                    layer.alert('暂无刊例信息',function(){
                        window.location.href = '/PublishManager/pricelist-video.html';
                    })
                }
            break;
            case "14003":
                if(CheckMoudle.State.MediaInfo){
                    CheckMoudle.Render("#CheckTemplate","#CheckView",function(){
                        
                            $("#submit").off("click").on("click",function(event){
                                event.preventDefault();
                                CheckMoudle.GetShenheInfo(function(){
                                    if(CheckMoudle.State.IsAgreen =='27002'){
                                        if(CheckMoudle.State.rejectMsg){
                                            CheckMoudle.ShenHeFunction();
                                        }else{
                                            layer.msg('驳回原因不能为空',function(){
                                                return ;
                                            });
                                        } 
                                   }else{
                                        CheckMoudle.ShenHeFunction();
                                   }
                                });
                            });
                            layerImg();
                            $('title').text(CheckMoudle.State.MediaInfo.Name+'-媒体管理-我的赤兔');
                    });
                }else{
                    layer.alert('暂无刊例信息',function(){
                        window.location.href = '/PublishManager/pricelist-sina.html';
                    })
                }  
            break;
            case "14001":
                if(CheckMoudle.State.MediaInfo){
                    CheckMoudle.Render("#CheckTemplate","#CheckView",function(){
                        $("#submit").off("click").on("click",function(event){
                            event.preventDefault();
                            CheckMoudle.GetShenheInfo(function(){
                                if(CheckMoudle.State.IsAgreen =='27002'){
                                    if(CheckMoudle.State.rejectMsg){
                                        CheckMoudle.ShenHeFunction();
                                    }else{
                                        layer.msg('驳回原因不能为空',function(){
                                            return ;
                                        });
                                    } 
                               }else{
                                    CheckMoudle.ShenHeFunction();
                               }
                            });
                        });
                        layerImg();
                        $('title').text(CheckMoudle.State.MediaInfo.Name+'-媒体管理-我的赤兔');
                    });
                }else{
                    layer.alert('暂无刊例信息',function(){
                        window.location.href = '/PublishManager/pricelist-wechat.html';
                    })
                }    
            break;
            default:break;
        }
    };
    // 获取审核按钮切换结果以及审核原因填写内容
    CheckMoudle.GetShenheInfo = function(fn){
        var IsAgreen = "27001";//通过为默认
        if($("#shenhe").find("input[name=shenhe]")[0].checked){
            IsAgreen = "27001";
        }else{
            IsAgreen = "27002";
        }
        CheckMoudle.setState({
            IsAgreen:IsAgreen,
            rejectMsg:$("#CheckView #shenhe .reason textarea").val()
        });
        if(fn){
            fn();
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

    $('#CheckView').on('change',':radio',function(){
        if($(this).attr('class') == 'shenhe1'){
            $('.parameter .reason').css('display','none');
        }else{
           $('.parameter .reason').css('display','block'); 
        }
    });

    $("#CheckView").on('click','input[name=shenhe]',function(){
        if($(this).attr('class')=='shenhe1'){
            CheckMoudle.setState.IsAgreen = 27001;
        }else{
            CheckMoudle.setState.IsAgreen = 27002;
        }
    });
});