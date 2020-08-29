$(function () {

	function OrderUploadData(){

		var searchUrl = window.location.search;
    	var searchid = Array.prototype.slice.call(searchUrl).splice(12).join("");

		this.config = {};
		this.config.suborderid = searchid;
		this.init();
	};


	//初始化
	OrderUploadData.prototype.init = function(){
		this.getData();
	};

	//获取子订单的信息
	OrderUploadData.prototype.getData = function(){
		var _this = this;
		var url = '/api/ADOrderInfo/GetBySubOrderID_ADOrderInfo';
		setAjax({
			url : url,
			type :'get',
			data :_this.config
		},function(data){
			if(data.Status == 0){
				var data = data.Result;
				var ADOrderInfo = data.ADOrderInfo;
				_this.config.MessageTit = ADOrderInfo;//将data存入对象里面
                _this.config.MediaType = data.SubADInfos[0].MediaType;
				_this.getListMeg();//渲染列表数据
			}else{
				layer.msg(data.Message,{'time':1000});
			}
		});
	};


	//读取子订单的反馈数据
	OrderUploadData.prototype.getListMeg = function(){
		var _this = this;

		//动态从接口获取  然后加入url里面
		var mediaType = _this.config.MediaType;
        var url = "http://www.chitunion.com/api/FeedbackData/SelectFeedbackData?suborderid="+_this.config.suborderid+"&MediaType="+mediaType;

		setAjax({
			url : url,
			type : "get"
		},function(data){
			var DetailData = data.Result;
			if(status == 0){
				_this.config.MessageDetail = DetailData;//将data存入对象里面
				_this.getTotal();//总计
				_this.CreateDiffTemplat(mediaType);//渲染模板
			}else{
				layer.msg(data.Message,{'time':1000});
			}
		})
	};

	//根据不同的对象放到不同的模板里面
	OrderUploadData.prototype.CreateDiffTemplat = function(mType){
		var _this = this;
		switch(mType){
			case 14001 :
			_this.showTemplate(_this.config,"#FeedBackDataTemplate-weixin");
			break;
			case 14002 :
			_this.showTemplate(_this.config,"#FeedBackDataTemplate-app");
			break;
			case 14003 :
			_this.showTemplate(_this.config,"#FeedBackDataTemplate-weibo");
			break;
			case 14004 :
			_this.showTemplate(_this.config,"#FeedBackDataTemplate-shipin");
			break;
			case 14005 :
			_this.showTemplate(_this.config,"#FeedBackDataTemplate-zhibo");
			break;
		}

		_this.clickLayer(mType);//加载弹窗
	    _this.deleteData(mType);//删除数据
	};

	//总计
	OrderUploadData.prototype.getTotal = function(){
		var _this = this;
		var data = _this.config.MessageDetail;

		data.forEach(function(item){
			var total = {
				"ClickCount":0,
                "CommentCount":0,
                "DeliveredCount":0,
                "LinkCount":0,
                "OrderCount":0,
                "PVCount":0,
                "ReadCount":0,
                "TransmitCount":0,
                "UVCount":0,
                "Value":0,
                "FeedbackBeginDate":"总计",
                "FeedbackEndDate":"总计"
			}
			item.DataList.forEach(function(items){
				total.ClickCount += items.ClickCount;
				total.CommentCount += items.CommentCount;
				total.DeliveredCount += items.DeliveredCount;
				total.LinkCount += items.LinkCount;
				total.OrderCount += items.OrderCount;
				total.PVCount += items.PVCount;
				total.ReadCount += items.ReadCount;
				total.TransmitCount += items.TransmitCount;
				total.UVCount += items.UVCount;
				total.Value += items.Value;
			})
			total.ClickRate = formatMoney(total.LinkCount/total.PVCount,2,"");
			item.DataList.push(total);
		})

	};


	//渲染模板  传data数据
	OrderUploadData.prototype.showTemplate = function(data,id){

		var _this = this;

        //->首先把页面中模板的innerHTML获取到
        var str=$(id).html();

        //->然后把str和data交给EJS解析处理，得到我们最终想要的字符串
        var result = ejs.render(str, {
        	data: data
        });

        //->最后把获取的HTML放入到MENU
        $(".install_box").html(result);

	};

	//弹窗
	OrderUploadData.prototype.clickLayer = function(type){

		var _this = this;
		var data = _this.config.MessageDetail;

		$(".upload_btn").each(function(){

			var addetailID = $(this).attr("data-id");//广告位ID
			var CreateUserID = $("#creatUser").attr("creat-id");//创建人ID
			$(this).off("click").on("click",function(e){
				e.preventDefault();
				if(type == 14001){//微信
					$.openPopupLayer({
	                    name: "popLayerDemo",
	                    url: "./myWeixin_popup.html",
	                    error: function (dd) { layer.alert(dd.status); },
	                    success:function () {
	                    	uploadFile();
	                    	_this.closeLayer();//关闭弹窗
	                   		$(".layer_data").find("input[name=Username]").each(function(){
	                   			$(this).on("input",function(){
	                   				replaceAndSetPos(this,/[^0-9]/g,'');
		                   		})
	                   		});

	                    	//提交信息
	                        $("#submitMessage").click(function (e) {
	                        	e.preventDefault();
                        		var ReadCount = $.trim($(".readCount").val()),//阅读数
                        			DeliveredCount = $.trim($(".deliverCount").val()),//送达数
                        			ClickCount = $.trim($(".clickCount").val()),//点赞数，
                        			TransmitCount = $.trim($(".transmitCount").val()),//转发数
                        			LinkCount = $.trim($(".linkCount").val()),//原文阅读点击数
                        			PVCount = $.trim($(".pvCount").val()),//PV数
                        			UVCount = $.trim($(".uvCount").val()),//UV数
                        			OrderCount = $.trim($(".orderCount").val()),//订单数
                        			//上传附件
                        			FilePath = $.trim($("#downloadFile").attr("href")),//上传附件  string
                        			FeedbackBeginDate = $.trim($(".feedbackBeginDate").val()),//开始日期 string
                        			FeedbackEndDate = $.trim($(".feedbackEndDate").val());//结束日期 string
                        		//执行时间
                        		//var beiginTime = $(".beiginTime").text().split(" ")[0];

	                        	//提示信息
                        		var str = "";
                    			if(ReadCount == ""){
                    				str += "阅读数不能为空";
                    			}else if(DeliveredCount == ""){
                    				str += "送达数不能为空";
                    			}else if(FilePath == ""){
                    				str += "请上传附件";
                    			}else if(FeedbackBeginDate == "" || FeedbackEndDate == ""){
                    				str += "数据日期不能为空";
                    			}else if(FeedbackBeginDate > FeedbackEndDate ){
                    				str += "开始日期不能大于结束日期";
                    			}/*else if(FeedbackBeginDate < beiginTime){
                    				str += "数据日期必须大于等于执行时间";
                    			}*/else{
                    				var obj ={MediaType:type,SubOrderID:_this.config.suborderid,ADDetailID:addetailID,CreateUserID:CreateUserID,ReadCount:ReadCount,DeliveredCount:DeliveredCount,ClickCount:ClickCount,TransmitCount:TransmitCount,LinkCount:LinkCount,PVCount:PVCount,UVCount:UVCount,OrderCount:OrderCount,FilePath:FilePath,FeedbackBeginDate:FeedbackBeginDate,FeedbackEndDate:FeedbackEndDate};

	                        		_this.getCheckAlreday(obj,addetailID);//判断数据日期是否存在
                    			}
                    			$(".toast").html(str);
	                        });
	                    }
	                });
				}else if(type == 14002){//app
					$.openPopupLayer({
	                    name: "popLayerDemo",
	                    url: "./myApp_popup.html",
	                    error: function (dd) { layer.alert(dd.status); },
	                    success:function () {
	                    	uploadFile();
	                    	_this.closeLayer();//关闭弹窗
	                    	$(".layer_data").find("input[name=Username]").each(function(){
	                   			$(this).on("input",function(){
	                   				replaceAndSetPos(this,/[^0-9]/g,'');
		                   		})
	                   		});
                			//点击率  点击数/pv  保留两位小数
                    		$(".linkCount").on("blur",function(){
                    			var	PVCount = $.trim($(".pvCount").val()),//PV数
                    				LinkCount = $.trim($(".linkCount").val()),//点击数
                					//ClickRate = Number((LinkCount/PVCount)*100).toFixed(2);
                					ClickRate = formatMoney(LinkCount/PVCount*100,2,"");
                    			$(".clickRate").html(ClickRate);
                    		});

                    		//提交信息
	                        $("#submitMessage").click(function (e) {
	                        	e.preventDefault();
                        		var	PVCount = $.trim($(".pvCount").val()),//PV数
	                    			UVCount = $.trim($(".uvCount").val()),//UV数
	                    			LinkCount = $.trim($(".linkCount").val()),//点击数
	                    			//ClickRate = parseFloat(Math.round((parseInt(LinkCount)/PVCount)*100)/100),//点击率
	                    			ClickRate = Number((LinkCount/PVCount)*100).toFixed(2);
	                    			//上传附件
	                    			FilePath = $.trim($("#downloadFile").attr("href")),//上传附件  string
	                    			FeedbackBeginDate = $.trim($(".feedbackBeginDate").val()),//开始日期 string
	                    			FeedbackEndDate = $.trim($(".feedbackEndDate").val());//结束日期 string
	                    		//执行周期
                        		//var beiginTime = $(".beiginTime").text().split(" ")[0];
	                    		//var endTime = $(".endTime").text().split(" ")[0];

	                    		// console.log(beiginTime,endTime);
                        		//提示信息
                        		var str = "";
                    			if(PVCount == ""){
                    				str += "PV数不能为空";
                    			}else if(UVCount == ""){
                    				str += "UV数不能为空";
                    			}else if(FilePath == ""){
                    				str += "请上传附件";
                    			}else if(FeedbackBeginDate == "" || FeedbackEndDate == ""){
                    				str += "数据日期不能为空";
                    			}else if(FeedbackBeginDate > FeedbackEndDate){
                    				str += "开始日期不能大于结束日期";
                    			}/*else if(FeedbackBeginDate < beiginTime || FeedbackEndDate > endTime){
                    				str += "数据日期必须在执行周期内";
                    			}*/else{
                    				var obj ={MediaType:type,SubOrderID:_this.config.suborderid,ADDetailID:addetailID,CreateUserID:CreateUserID,PVCount:PVCount,UVCount:UVCount,LinkCount:LinkCount,ClickRate:ClickRate,FilePath:FilePath,FeedbackBeginDate:FeedbackBeginDate,FeedbackEndDate:FeedbackEndDate};
	                        		_this.getCheckAlreday(obj,addetailID);//判断数据日期是否存在
                    			}
                    			$(".toast").html(str);
	                        });
	                    }
	                });
				}else if(type == 14003){//微博
					$.openPopupLayer({
	                    name: "popLayerDemo",
	                    url: "./myWeibo_popup.html",
	                    error: function (dd) { layer.alert(dd.status); },
	                    success:function () {
	                    	uploadFile();
	                    	_this.closeLayer();//关闭弹窗
	                    	$(".layer_data").find("input[name=Username]").each(function(){
	                   			$(this).on("input",function(){
	                   				replaceAndSetPos(this,/[^0-9]/g,'');
		                   		})
	                   		});
	                    	// console.log(type);
	                    	//提交信息
	                        $("#submitMessage").click(function (e) {
	                        	e.preventDefault();
	                        	var ReadCount = $.trim($(".readCount").val()),//阅读数
                        			TransmitCount = $.trim($(".transmitCount").val()),//转发数
                        			ClickCount = $.trim($(".clickCount").val()),//点赞数
                        			CommentCount = $.trim($(".commentCount").val()),//评论数
                        			LinkCount = $.trim($(".linkCount").val()),//链接点击数
									PVCount = $.trim($(".pvCount").val()),//PV数
			                        UVCount = $.trim($(".uvCount").val()),//UV数
									OrderCount = $.trim($(".orderCount").val()),//订单数
                        			FilePath = $.trim($("#downloadFile").attr("href")),//上传附件  string
                        			FeedbackBeginDate = $.trim($(".feedbackBeginDate").val()),//开始日期 string
                        			FeedbackEndDate = $.trim($(".feedbackEndDate").val());//结束日期 string
                        		var CreateUserID = $("#creatUser").attr("creat-id");//创建人ID
                        		//执行时间
                        		//var beiginTime = $(".beiginTime").text().split(" ")[0];
                        		//提示信息	阅读数 送达数 数据日期，必填写项
                        		var str = "";
                    			if(ReadCount == ""){
                    				str += "阅读数不能为空";
                    			}else if(FilePath == ""){
                    				str += "请上传附件";
                    			}else if(FeedbackBeginDate == "" || FeedbackEndDate == ""){
                    				str += "数据日期不能为空";
                    			}else if(FeedbackBeginDate > FeedbackEndDate ){
                    				str += "开始日期不能大于结束日期";
                    			}/*else if(FeedbackBeginDate < beiginTime){
                    				str += "数据日期必须大于等于执行时间";
                    			}*/else{
                    				var obj ={MediaType:type,SubOrderID:_this.config.suborderid,ADDetailID:addetailID,CreateUserID:CreateUserID,ReadCount:ReadCount,TransmitCount:TransmitCount,ClickCount:ClickCount,CommentCount:CommentCount,LinkCount:LinkCount,PVCount:PVCount,UVCount:UVCount,OrderCount:OrderCount,FilePath:FilePath,FeedbackBeginDate:FeedbackBeginDate,FeedbackEndDate:FeedbackEndDate};

	                        		_this.getCheckAlreday(obj,addetailID);//判断数据日期是否存在
                    			}
                    			$(".toast").html(str);
	                        });
	                    }
	                });
				}else if(type == 14004){//视频
					$.openPopupLayer({
	                    name: "popLayerDemo",
	                    url: "./myShipin_popup.html",
	                    error: function (dd) { layer.alert(dd.status); },
	                    success:function () {
	                    	uploadFile();
	                        _this.closeLayer();//关闭弹窗
	                        $(".layer_data").find("input[name=Username]").each(function(){
	                   			$(this).on("input",function(){
	                   				replaceAndSetPos(this,/[^0-9]/g,'');
		                   		})
	                   		});
	                        $("#submitMessage").click(function (e) {
	                        	e.preventDefault();
	                        	var ReadCount = $.trim($(".readCount").val()),//观看数
                        			TransmitCount = $.trim($(".transmitCount").val()),//曝光数
                        			FilePath = $.trim($("#downloadFile").attr("href")),//上传附件  string
                        			FeedbackBeginDate = $.trim($(".feedbackBeginDate").val()),//开始日期 string
                        			FeedbackEndDate = $.trim($(".feedbackEndDate").val());//结束日期 string
                        		var CreateUserID = $("#creatUser").attr("creat-id");//创建人ID
                        		//执行时间
                        		//var beiginTime = $(".beiginTime").text().split(" ")[0];
                        		//提示信息	阅读数 送达数 数据日期，必填写项
                        		var str = "";
                    			if(ReadCount == ""){
                    				str += "观看数不能为空";
                    			}else if(FilePath == ""){
                    				str += "请上传附件";
                    			}else if(FeedbackBeginDate == "" || FeedbackEndDate == ""){
                    				str += "数据日期不能为空";
                    			}else if(FeedbackBeginDate > FeedbackEndDate){
                    				str += "开始日期不能大于结束日期";
                    			}/*else if(FeedbackBeginDate < beiginTime){
                    				str += "数据日期必须大于等于执行时间";
                    			}*/else{
                    				var obj ={MediaType:type,SubOrderID:_this.config.suborderid,ADDetailID:addetailID,CreateUserID:CreateUserID,ReadCount:ReadCount,TransmitCount:TransmitCount,FilePath:FilePath,FeedbackBeginDate:FeedbackBeginDate,FeedbackEndDate:FeedbackEndDate};
	                        		_this.getCheckAlreday(obj,addetailID);//判断数据日期是否存在
                    			}
                    			$(".toast").html(str);

	                        });
	                    }
	                });
				}else if(type == 14005){//直播
					$.openPopupLayer({
	                    name: "popLayerDemo",
	                    url: "./myZhibo_popup.html",
	                    error: function (dd) { layer.alert(dd.status); },
	                    success:function () {
	                    	uploadFile();
	                    	_this.closeLayer();//关闭弹窗
	                    	$(".layer_data").find("input[name=Username]").each(function(){
	                   			$(this).on("input",function(){
	                   				replaceAndSetPos(this,/[^0-9]/g,'');
		                   		})
	                   		});
	                        $("#submitMessage").click(function (e) {
	                        	e.preventDefault();
	                        	var ReadCount = $.trim($(".readCount").val()),//总观看人数
                        			TransmitCount = $.trim($(".transmitCount").val()),//峰值
                        			Value = $.trim($(".value").val()),//虚拟礼物价值
                        			ClickCount = $.trim($(".clickCount").val()),//题及数
                        			FilePath = $.trim($("#downloadFile").attr("href")),//上传附件  string
                        			FeedbackBeginDate = $.trim($(".feedbackBeginDate").val()),//开始日期 string
                        			FeedbackEndDate = $.trim($(".feedbackEndDate").val());//结束日期 string
                        		var CreateUserID = $("#creatUser").attr("creat-id");//创建人ID
                   				//执行时间
                        		//var beiginTime = $(".beiginTime").text().split(" ")[0];
                        		//提示信息	阅读数 送达数 数据日期，必填写项
                        		var str = "";
                    			if(ReadCount == ""){
                    				str += "总观看人数不能为空";
                    			}else if(FilePath == ""){
                    				str += "请上传附件";
                    			}else if(FeedbackBeginDate == "" || FeedbackEndDate == ""){
                    				str += "数据日期不能为空";
                    			}else if(FeedbackBeginDate > FeedbackEndDate){
                    				str += "开始日期不能大于结束日期";
                    			}/*else if(FeedbackBeginDate < beiginTime){
                    				str += "数据日期必须大于等于执行时间";
                    			}*/else{
                    				var obj ={MediaType:type,SubOrderID:_this.config.suborderid,ADDetailID:addetailID,CreateUserID:CreateUserID,ReadCount:ReadCount,TransmitCount:TransmitCount,Value:Value,ClickCount:ClickCount,FilePath:FilePath,FeedbackBeginDate:FeedbackBeginDate,FeedbackEndDate:FeedbackEndDate};

	                        		_this.getCheckAlreday(obj,addetailID);//判断数据日期是否存在
                    			}
                    			$(".toast").html(str);
	                        })
	                    }
	                });
				}
			})
		})
	};


	//判断数据日期是否存在   在插入之前判断   日期不用转换为时间戳就能直接比较
	OrderUploadData.prototype.getCheckAlreday = function (obj,addetailID) {
		var _this = this;
		var arr = [];//列表里面已经有的日期数据
		/*var FeedbackBeginDate = new Date(obj.FeedbackBeginDate.replace("-","/")).getTime() ;//当前输入的开始日期 c
		var FeedbackEndDate = new Date(obj.FeedbackEndDate.replace("-","/")).getTime();//当前输入的结束日期 d*/
		var FeedbackBeginDate = obj.FeedbackBeginDate;//当前输入的开始日期 c
		var FeedbackEndDate = obj.FeedbackEndDate;//当前输入的结束日期 d

		/*获取列表里面有的日期  然后与现在输入的日期循环判断*/
		var mediaType = _this.config.MediaType;
		var url = "http://www.chitunion.com/api/FeedbackData/SelectFeedbackData?suborderid="+_this.config.suborderid+"&MediaType="+mediaType;

		setAjax({
			url : url,
			type : "get"
		},function(data){
			var DetailData = data.Result;
			if(status == 0){

				DetailData.forEach(function(item){
					if(item.ADDetailID == addetailID){
						item.DataList.forEach(function(items){
							var obj = {};
							obj.beginDate = items.FeedbackBeginDate;
							obj.endDate = items.FeedbackEndDate;
							arr.push(obj);
						})
					}
				})

				/*当列表里面什么都没有的情况下  arr就是空数组*/
				if(arr.length < 1){
					_this.safeData(obj,addetailID);
				}else{
					for(var i = 0; i < arr.length; i++){
						//列表里面已经有的数据日期
				        var bt =  arr[i].beginDate; //a
						var et =  arr[i].endDate; //b

						var btLast = arr[arr.length-1];
						var etLast = arr[arr.length-1];
				        if(FeedbackEndDate<bt || FeedbackBeginDate>et){
				        	if(i==(arr.length-1)){
					        	if(FeedbackEndDate<btLast || FeedbackBeginDate>etLast) {
					        		_this.safeData(obj,addetailID);
					        	}
				        	}
				          	continue;
				        }else{
				            if(FeedbackBeginDate==bt && FeedbackEndDate==et){
					            layer.confirm('你确认要覆盖之前的数据吗', {
									btn: ['确认','取消'] //按钮
								}, function(){
									_this.safeData(obj,addetailID);
									layer.closeAll();
								},function(){
									layer.closeAll();
								});
					            break;
					        }else{
					            _this.safeData(obj,addetailID);//交叉的情况
					            break;
					        }
				        }
				    }

				}
			}else{
				layer.msg("数据加载失败",{'time':1000});
			}
		})
	};


	//弹窗数据保存
	OrderUploadData.prototype.safeData = function(obj,addetailID){
		var _this = this;
		var url = '/api/FeedbackData/InserFeedbackData';

		setAjax({url:url,type:'post',data:obj},
	        function(data){
	        	if(data.Status == 0){
	        		_this.getListMeg();//渲染列表数据
	        		$.closePopupLayer('popLayerDemo');
	        	}else{
	        		layer.msg(data.Message,{'time':1000});
	        	}
	        }
    	);
	};


	//删除数据
	OrderUploadData.prototype.deleteData = function(type){

		var _this = this;

		$(".delete").each(function(){
			var	FeedbackID = $(this).attr("data-id"),//id
				FileUrl = $(this).attr("data-url");//url

			$(this).on("click",function(e){
				e.preventDefault();
				layer.confirm('您是否确认删除此数据吗', {
						btn: ['确认','取消'] //按钮
					}, function(){
						var url = "/api/FeedbackData/DeleteFeedbackData?MediaType="+type+"&FeedbackID="+FeedbackID+"&FileUrl="+FileUrl;
						setAjax({url:url,type:'get'},
						    function(data){
						    	if(data.Status == 0){
						    		_this.getListMeg();//重新渲染接口数据
						    	}else{
						    		layer.msg(data.Message,{'time':1000});
						    	}
						    }
						);
						layer.closeAll();
				});
			})
		})
	};

	//弹窗关闭
	OrderUploadData.prototype.closeLayer = function(){
		//取消信息
		$("#cancleMessage").off("click").on("click",function(e){
			e.preventDefault();
    		$.closePopupLayer('popLayerDemo');
    	});
    	//关闭弹窗
		$("#closebt").off("click").on("click",function(e){
			e.preventDefault();
    		$.closePopupLayer('popLayerDemo');
    	});
	};


	var uploadObj = new OrderUploadData();
});



//----------------只能输入数字 Start
	/*调用    '/限制的正则表达式/g'   必须以/开头，/g结尾
	 *  onkeyup="replaceAndSetPos(this,/[^0-9]/g,'')" oninput ="replaceAndSetPos(this,/[^0-9]/g,'')"
	*/
//获取光标位置
function getCursorPos(obj) {
    var CaretPos = 0;
    // IE Support
    if (document.selection) {
        obj.focus (); //获取光标位置函数
        var Sel = document.selection.createRange ();
        Sel.moveStart ('character', -obj.value.length);
        CaretPos = Sel.text.length;
    }
    // Firefox/Safari/Chrome/Opera support
    else if (obj.selectionStart || obj.selectionStart == '0')
        CaretPos = obj.selectionEnd;
    return (CaretPos);
};
//定位光标
function setCursorPos(obj,pos){

    if (obj.setSelectionRange) { //Firefox/Safari/Chrome/Opera
        obj.focus(); //
        obj.setSelectionRange(pos,pos);
    } else if (obj.createTextRange) { // IE
        var range = obj.createTextRange();
        range.collapse(true);
        range.moveEnd('character', pos);
        range.moveStart('character', pos);
        range.select();
    }
};
//替换后定位光标在原处,可以这样调用onkeyup=replaceAndSetPos(this,/[^/d]/g,'');
function replaceAndSetPos(obj,pattern,text){
    if ($(obj).val() == "" || $(obj).val() == null) {
        return;
    }
    var pos=getCursorPos(obj);//保存原始光标位置
    var temp=$(obj).val(); //保存原始值
    obj.value=temp.replace(pattern,text);//替换掉非法值
    //截掉超过长度限制的字串（此方法要求已设定元素的maxlength属性值）
    var max_length = obj.getAttribute? parseInt(obj.getAttribute("maxlength")) : "";
    if( obj.value.length > max_length){
            var str1 = obj.value.substring( 0,pos-1 );
        var str2 = obj.value.substring( pos,max_length+1 );
        obj.value = str1 + str2;
    }
    pos=pos-(temp.length-obj.value.length);//当前光标位置
    setCursorPos(obj,pos);//设置光标
    //el.onkeydown = null;
};
//-----------------只能输入数字 end

/**
	*  上传附件
	* @desc  JQuery扩展，将json字符串转换为对象，需要引用类库JQuery
	* @param   json字符串
	* @return 返回object,array,string等对象
	* @Add=Masj, Date: 2009-12-07
	*/

function uploadFile() {
    jQuery.extend({
        evalJSON: function (strJson) {
            if ($.trim(strJson) == '')
                return '';
            else
                return eval("(" + strJson + ")");
        }
    });
    function getCookie(name) {
        var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
        if (arr = document.cookie.match(reg))
            return unescape(arr[2]);
        else
            return null;
    };
    function escapeStr(str) {
        return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
    };

 
    $('#UploadifyDoc').uploadify({
        'buttonText': '+ 上传附件',
        'buttonClass': 'but_upload',
        'swf': '/Js/uploadify.swf?_=' + Math.random(),
        'uploader': '/AjaxServers/UploadFile.ashx',
        'auto': true,
        'multi': false,
        'width': 200,
        'height': 35,
        'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
        'fileTypeDesc': '支持格式:jpg,jpeg,png,zip',
        'fileTypeExts': '*.jpg;*.jpeg;*.png;*.zip;',
        'fileSizeLimit':'5MB',
        'queueSizeLimit': 1,
        'scriptAccess': 'always',
        'onQueueComplete': function (event, data) {
            //enableConfirmBtn();
        },
        'onQueueFull': function () {
            layer.alert('您最多只能上传1个文件！');
            return false;
        },
        'onUploadSuccess': function (file, data, response) {
            if (response == true) {
                var json = $.evalJSON(data);
                $(".fileBox").show();
                $("#FileName").text(json.FileName);
                $("#downloadFile").attr("href",json.Msg);
            }
        },
        'onProgress': function (event, queueID, fileObj, data) {},
        'onUploadError': function (event, queueID, fileObj, errorObj) {
            //enableConfirmBtn();
        },
        'onSelectError':function(file, errorCode, errorMsg){
            console.log(errorCode);
            /*if (errorCode == SWFUpload.UPLOAD_ERROR.FILE_CANCELLED
                || errorCode == SWFUpload.UPLOAD_ERROR.UPLOAD_STOPPED) {
                return;
            }
            switch(errorCode) {
                case -100:
                    $('#'+imgerr).html('上传图片数量超过1个');
                    break;
                case -110:
                    $('#'+imgerr).html('上传图片大小应小于2MB');
                    break;
                case -120:

                    break;
                case -130:
                    $('#'+imgerr).html('上传图片类型不正确');
                    break;
            }*/
        }
    });
	    
};



