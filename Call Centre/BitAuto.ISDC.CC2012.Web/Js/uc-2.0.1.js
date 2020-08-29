 	
	// 获取事件的HttpServer地址
	var httpServerAdress = "http://127.0.0.1:9009/State/query?callback=?";
	//var httpServerAdress = "http://www.huayunw.com/test.xml";
	//var httpServerAdress = "http://127.0.0.1/cca/TestDummy/UCServer.jsp?callback=?&getEvent=bbb";
	// 外呼地址
	var httpServerAdressCallOut = "http://127.0.0.1:9009/CallControl/MakeCall?callback=?";
	//var httpServerAdressCallOut = "http://www.huayunw.com/test.xml";
	//var httpServerAdressCallOut = "http://127.0.0.1/cca/TestDummy/test.xml";
	//var httpServerAdressCallOut = "http://127.0.0.1/cca/TestDummy/UCServer.jsp?callback=?";
	// 置闲地址
	var httpServerAdressReady = "http://127.0.0.1:9009/State/Ready?callback=?";
	//var httpServerAdressReady = "http://127.0.0.1/cca/TestDummy/test.xml";
	//var httpServerAdressReady = "http://127.0.0.1/cca/TestDummy/UCServer.jsp?callback=?&Ready=bbb";
	// 置忙地址
	var httpServerAdressNotReady = "http://127.0.0.1:9009/State/NotReady?callback=?&Reason=";
	//var httpServerAdressNotReady = "http://127.0.0.1/cca/TestDummy/jsonp.txt";
	//var httpServerAdressNotReady = "http://127.0.0.1/cca/TestDummy/UCServer.jsp?callback=?&Reason=bbb";
	// 挂断地址
	var httpServerAdressDisconnect = "Http://127.0.0.1:9009/CallControl/Hangup?callback=?";
	//var httpServerAdressDisconnect = "http://127.0.0.1/cca/TestDummy/UCServer.jsp?callback=?";
	//获取登录ID地址
	var httpServerAdressGetAgent = "http://127.0.0.1:9009/State/AgentInfo?callback=?";

	// 弹屏地址
	var openUrl = "http://www.huayunw.com/";
	// 弹屏方式
	// UC_OPEN: UC直接弹屏
	// CALL_BACK: UC回调弹屏
	var openWindowMode = "CALL_BACK";
	// 刷新事件
	var refreshTime = 1000;

	// 操作类型
	var operation_type = new Array();
	// 外呼
	operation_type[0] = "CallOut";
	// 置闲
	operation_type[1] = "Ready";
	// 置忙
	operation_type[2] = "NotReady";
	// 挂断
	operation_type[3] = "Disconnect";

	// 参数列表
	var call_in_params = new Array();
	// 原始主叫
	call_in_params[0] = "OrgANI";
	// 原始被叫
	call_in_params[1] = "OrgDNIS";
	// 当前主叫
	call_in_params[2] = "ANI";
	// 当前被叫
	call_in_params[3] = "DNIS";
	// 咨询主叫
	call_in_params[4] = "CslANI";
	// 咨询被叫
	call_in_params[5] = "CslDNIS";
	// 服务类型
	call_in_params[6] = "ServiceUnitType";
	// 请求技能组
	call_in_params[7] = "ReqSkill";
	// 响应技能组
	call_in_params[8] = "ResSkill";
	// 呼叫类型
	call_in_params[9] = "CallType";
	// 本次呼叫标识
	call_in_params[10] = "SessionID";
	// IVR数据
	call_in_params[11] = "IVRData";
	
	// http请求方式
	var requestType = "get";
	/**********************返回结果定义************************/
	// 事件类型
	var EventType = "EventType";
	// 原始主叫
	var OrgANI = "OrgANI";
	// 原始被叫
	var OrgDNIS = "OrgDNIS";
	// 当前主叫
	var ANI= "ANI";
	// 当前被叫
	var DNIS= "DNIS";
	// 咨询主叫
	var CslANI= "CslANI";
	// 咨询被叫
	var CslDNIS= "CslDNIS";
	// 服务类型
	var ServiceUnitType= "ServiceUnitType";
	// 请求技能组
	var ReqSkill= "ReqSkill";
	// 响应技能组
	var ResSkill= "ResSkill";
	// 呼叫类型
	var CallType= "CallType";
	// IVR数据
	var IVRData= "IVRData";
	// 本次呼叫表示
	var SessionID = "SessionID";
	/**********************返回结果定义************************/
	
	/**********************事件类型EventType************************/

	// Init状态
	var AgentStatus_Init = "AgentStatus_Init";
	// Login状态
	var AgentStatus_Login = "AgentStatus_Login";
	// Logout状态
	var AgentStatus_Logout = "AgentStatus_Logout";
	// Ready状态
	var AgentStatus_Ready = "AgentStatus_Ready";
	// NotReady状态
	var AgentStatus_NotReady = "AgentStatus_NotReady";
	// Otherwork状态
	var AgentStatus_Otherwork = "AgentStatus_Otherwork";
	// ACW状态
	var AgentStatus_ACW = "AgentStatus_ACW";
	// Locked状态
	var AgentStatus_Locked = "AgentStatus_Locked";
	// Alerting状态
	var AgentStatus_Alerting = "AgentStatus_Alerting";
	// Connecting状态
	var AgentStatus_Connecting = "AgentStatus_Connecting";
	// Connected_Normal状态
	var AgentStatus_Connected_Normal = "AgentStatus_Connected_Normal";
	// Connected_Internal状态
	var AgentStatus_Connected_Internal = "AgentStatus_Connected_Internal";
	// Disconnected状态
	var AgentStatus_Disconnected = "AgentStatus_Disconnected";
	// Suspended状态
	var AgentStatus_Suspended = "AgentStatus_Suspended";
	// Held状态(请求IVR服务时转为此状态)
	var AgentStatus_Held = "AgentStatus_Held";
	// Consulting状态
	var AgentStatus_Consulting = "AgentStatus_Consulting";
	// Consulted状态
	var AgentStatus_Consulted = "AgentStatus_Consulted";
	// Conferenced状态
	var AgentStatus_Conferenced = "AgentStatus_Conferenced";
	// Monitored状态
	var AgentStatus_Monitored = "AgentStatus_Monitored";
	// Inserted状态
	var AgentStatus_Inserted = "AgentStatus_Inserted";
	// Unkown状态
	var AgentStatus_Unkown = "AgentStatus_Unkown";
	/**********************事件类型EventType************************/

	/**********************呼叫类型************************/
	// 呼叫类型：
	// 标识 信息 信息描述
	// 0 INTERNAL 呼入
	// 1 OUTBOUND 外呼
	// 2 INBOUND 内呼
	var INTERNAL = "INTERNAL";
	var OUTBOUND = "OUTBOUND";
	var INBOUND = "INBOUND";
	/**********************呼叫类型************************/

	
	var event_params = new Array();
	// 原始主叫
	event_params[0] = OrgANI;
	// 原始被叫
	event_params[1] = OrgDNIS;
	// 当前主叫
	event_params[2] = ANI;
	// 当前被叫
	event_params[3] = DNIS;
	// 咨询主叫
	event_params[4] = CslANI;
	// 咨询被叫
	event_params[5] = CslDNIS;
	// 服务类型
	event_params[6] = ServiceUnitType;
	// 请求技能组
	event_params[7] = ReqSkill;
	// 响应技能组
	event_params[8] = ResSkill;
	// 呼叫类型
	event_params[9] = CallType;
	// 本次呼叫标识
	event_params[10] = SessionID;
	// IVR数据
	event_params[11] = IVRData;
	var ucwindows;
	var ucdocument;

	/* UCObj version 2.0 pre
	 * creator: tang-xue (tangxue@huayunw.com)
	 * 最新更新：2012-2-1
	 */
	function UCObj(win, doc) {
	
	 	ucwindows = win;
	 	ucdocument = doc;

//	 	this.CallID = "";
//	 	this.recUrl = "";
//	 	this.tSessionID = "";
//	 	this.InitiatedTime = 0;
//	 	this.NetworkReachedTime = 0;
//	 	this.EstablishedTime = 0;

//        //坐席振铃时长=对方振铃时间-外拨开始时间
//	 	this.AgentRingTime = function () {
//	 	    return parseInt((this.NetworkReachedTime - this.InitiatedTime) / 1000);
//	 	}
//	 	//客户振铃时长=电话接通时间-对方振铃时间
//	 	this.CustomerRingTime = function () {
//	 	    return parseInt((this.EstablishedTime - this.NetworkReachedTime) / 1000);
//	 	}

//	 	this.handlerURL = "http://ncc.sys1.bitauto.com/AjaxServers/UCDemoHandler.ashx";
		/*
		 * 开始向UC取来电事件.每搁1000ms取一次
		 */
		this.initUCObj = function(openURL, params, requestTime) { 
			openUrl = openURL;
			call_in_params = params;
			refreshTime = requestTime;
		}
		
		/*
		 * 开始向UC取来电事件.每搁1000ms取一次
		 */
		 this.start = function() { 
			//showDebugMessage("Send Start..................");
			win.setInterval("getEvent()",refreshTime); 
			//showDebugMessage("Send End..................");
		}
		
		/*
		 * 外呼处理
		 */
		this.doCallOut = function(callNum, showNum) {

        
			//showDebugMessage("doCallOut Start..................");

			if (callNum.length == 12 && callNum.indexOf("01") == 0) {
				callNum = callNum.substring(1);
			}

			// 外拨号码前需加Tel:的前缀
			if (callNum.indexOf("Tel:") == -1) {
				callNum = "Tel:" + callNum;
			}

			var callOutUrl = httpServerAdressCallOut + "&TargetDN=" + callNum + "&ShowANI=" + showNum + "&MakeCallType=2";
			var msg = "";

			jQuery.ajax({
	  		type: requestType,
			  url: callOutUrl,
			  dataType: "jsonp",
			  jsonp: "callback",
			  beforeSend: function(XMLHttpRequest){
			   showDebugMessage("beforeSend Start..................");
			  },
			  success: function(data){
			  	//showDebugMessage("success Start..................");
					// 处理结果
					ucwindows.showOpeResultMsg(operation_type[0],data.Code, data.Msg);
			  },
			  complete: function(XMLHttpRequest, textStatus){
			   	//showDebugMessage("complete Start..................");
			  },
			  error: function(XMLHttpRequest, textStatus, errorThrown){
			   //请求出错处理
			   //showDebugMessage("外呼失败 " + XMLHttpRequest.statusText);
				 ucwindows.showOpeResultMsg(operation_type[0],"-2", "请求失败。");
			  }
			})

			//showDebugMessage("doCallOut End..................");
			return msg;
		}

		/*
		 * 置闲处理
		 */
		this.doReady = function() {

			//showDebugMessage("doReady Start..................");

			var msg = "";
		
			jQuery.ajax({
	  		type: requestType,
			  url: httpServerAdressReady,
			  dataType: "jsonp",
			  jsonp: "callback",
			  beforeSend: function(XMLHttpRequest){
			   showDebugMessage("beforeSend Start..................");
			  },
			  success: function(data){
			  	showDebugMessage("success Start..................");
			  	// 处理结果
					ucwindows.showOpeResultMsg(operation_type[1],data.Code, data.Msg);
				
			  },
			  complete: function(XMLHttpRequest, textStatus){
			   	showDebugMessage("complete Start..................");
			  },
			  error: function(XMLHttpRequest, textStatus, errorThrown){
			   //请求出错处理
			   showDebugMessage("置闲失败: " + XMLHttpRequest.statusText);
				 ucwindows.showOpeResultMsg(operation_type[1],"-2", "请求失败。");
			  }
			})
			
			showDebugMessage("doReady End..................");
			return msg;
		}

		/*
		 * 置忙处理
		 */
		this.doNotReady = function() {

			showDebugMessage("doNotReady Start..................");
			var msg = "";
      
			jQuery.ajax({
	  		type: requestType,
			  url: httpServerAdressNotReady,
			  dataType: "jsonp",
			  jsonp: "callback",
			  beforeSend: function(XMLHttpRequest){
			   showDebugMessage("beforeSend Start..................");
			  },
			  success: function(data){
			  	showDebugMessage("success Start..................");
			  	// 处理结果
					ucwindows.showOpeResultMsg(operation_type[2],data.Code, data.Msg);
					
			  },
			  complete: function(XMLHttpRequest, textStatus){
			   	showDebugMessage("complete Start..................");

			  },
			  error: function(XMLHttpRequest, textStatus, errorThrown){
			   //请求出错处理
			   showDebugMessage("置忙失败: " + XMLHttpRequest.statusText);
				 ucwindows.showOpeResultMsg(operation_type[2],"-2", "请求失败。");
			  }
			})

			showDebugMessage("doNotReady End..................");
			return msg;
		}
		
		/*
		 * 挂断处理
		 */
		this.doDisconnect = function() {

			showDebugMessage("doDisconnect Start..................");
			var msg = "";
      
			jQuery.ajax({
	  		type: requestType,
			  url: httpServerAdressDisconnect,
			  dataType: "jsonp",
			  jsonp: "callback",
			  beforeSend: function(XMLHttpRequest){
			   showDebugMessage("beforeSend Start..................");
			  },
			  success: function(data){
			  	showDebugMessage("success Start..................");
			  	// 处理结果
					//ucwindows.showOpeResultMsg(operation_type[3],data.Code, data.Msg);
					
			  },
			  complete: function(XMLHttpRequest, textStatus){
			   	showDebugMessage("complete Start..................");

			  },
			  error: function(XMLHttpRequest, textStatus, errorThrown){
			   //请求出错处理
			   showDebugMessage("挂断失败: " + XMLHttpRequest.statusText);
				 //ucwindows.showOpeResultMsg(operation_type[3],"-2", "请求失败。");
			  }
			})

			showDebugMessage("doDisconnect End..................");
			return msg;
		}


        this.getAgent = function () {

            showDebugMessage("getAgent Start..................");

            var msg = "";


            jQuery.ajax({

                type: requestType,

                url: httpServerAdressGetAgent,

                dataType: "jsonp",

                jsonp: "callback",

                beforeSend: function (XMLHttpRequest) {

                    showDebugMessage("beforeSend Start..................");
                },

                success: function (data) {
                    showDebugMessage("success Start..................");

                    // 处理结果

                    ucwindows.setAgentInfo(data);


                },

                complete: function (XMLHttpRequest, textStatus) {
                    showDebugMessage("complete Start..................");
                },

                error: function (XMLHttpRequest, textStatus, errorThrown) {

                    //请求出错处理
                    showDebugMessage("置忙失败: " + XMLHttpRequest.statusText);
                    ucwindows.showOpeResultMsg(operation_type[2], "-2", "请求失败。");

                }

            })

            showDebugMessage("getAgent End..................");

            return msg;

        }
	}

	
	/*
	 * 解析事件，如果为振铃事件进行弹屏处理
	 */
	function getEvent() {
	    //alert("getEvent begin...");
	    jQuery.ajax({
	        type: requestType,
	        url: httpServerAdress,
	        dataType: "jsonp",
	        jsonp: "callback",
	        beforeSend: function (XMLHttpRequest) {
	            showDebugMessage("beforeSend Start..................");
	        },
	        success: function (data) {
	            //showDebugMessage("success Start..................");

	            // 事件类型
	            var resultCode = data.Result;

	            if (resultCode != "0") {
	                return;
	            };

	            var ani = data.ANI;
	            if (ani.indexOf("tel:") > -1 || ani.indexOf("Tel:") > -1) {
	                ani = ani.substring(4);
	            }
	            data.ANI = ani;

	            var dnis = data.DNIS;
	            if (dnis.indexOf("tel:") > -1 || dnis.indexOf("Tel:") > -1) {
	                dnis = dnis.substring(4);
	            }
	            data.DNIS = dnis;

	            var sessionid = data.SessionID;
	            if (sessionid.indexOf(":") > -1) {
	                var arr = sessionid.split(":");
	                data.SessionID = arr[0];
	            }

	            // 事件类型
	            var EventType = data.EventType;
	            //alert("02EventType:" + data.EventType + ",ANI:" + ani + ",DNIS:" + data.DNIS + ",SessionID:" + data.SessionID + ",Result:" + data.Result);
	            //if (EventType == AgentStatus_Alerting) {
	            if (openWindowMode == "UC_OPEN") {
	                doOpenUrl(data);
	            } else {
	                doCallBackOpen(data);
	            }
	            //	} 

	            if (EventType == AgentStatus_Connecting) {
	                //test01();
	                if (AgentEventTool.AgentStatus_Connecting) {
	                    AgentEventTool.AgentStatus_Connecting(data);
	                }
	            }

	            if (EventType == AgentStatus_Connected_Normal) {
	                //test01();
	                if (AgentEventTool.AgentStatus_Connected_Normal) {
	                    AgentEventTool.AgentStatus_Connected_Normal(data);
	                }
	            }

	            if (EventType == AgentStatus_Disconnected) {
	                //test01();
	                if (AgentEventTool.AgentStatus_Disconnected) {
	                    AgentEventTool.AgentStatus_Disconnected(data);
	                }
	            }
	        },
	        complete: function (XMLHttpRequest, textStatus) {
	            showDebugMessage("complete Start..................");
	        },
	        error: function (XMLHttpRequest, textStatus, errorThrown) {

	            //请求出错处理
	            showErrorMessage("取得事件失败: " + XMLHttpRequest.statusText);
	        }
	    })

	}

	// UC直接弹屏
	function doOpenUrl(events) {
		// 拼接弹屏地址
		var j = 0;
		for (j = 0; j < call_in_params.length; j++) {
			var paramName = call_in_params[j];
			if (paramName != "") {
				// 第一个参数
				if (openUrl.indexOf("?") < 0) {
					var dataValue = "";
					if (event_params[j] == "OrgANI") {
						dataValue = events.OrgANI;
					} else if (event_params[j] == "OrgDNIS") {
						dataValue = events.OrgDNIS;
					} else if (event_params[j] == "ANI") {
						dataValue = events.ANI;
					} else if (event_params[j] == "DNIS") {
						dataValue = events.DNIS;
					} else if (event_params[j] == "CslANI") {
						dataValue = events.CslANI;
					} else if (event_params[j] == "CslDNIS") {
						dataValue = events.CslDNIS;
					} else if (event_params[j] == "ServiceUnitType") {
						dataValue = events.ServiceUnitType;
					} else if (event_params[j] == "ReqSkill") {
						dataValue = events.ReqSkill;
					} else if (event_params[j] == "ResSkill") {
						dataValue = events.ResSkill;
					} else if (event_params[j] == "CallType") {
						dataValue = events.CallType;
					} else if (event_params[j] == "IVRData") {
						dataValue = events.IVRData;
					}
					openUrl = openUrl + "?" + paramName + "=" + dataValue;
					// 其他参数
				} else {
					var dataValue = "";
					if (event_params[j] == "OrgANI") {
						dataValue = events.OrgANI;
					} else if (event_params[j] == "OrgDNIS") {
						dataValue = events.OrgDNIS;
					} else if (event_params[j] == "ANI") {
						dataValue = events.ANI;
					} else if (event_params[j] == "DNIS") {
						dataValue = events.DNIS;
					} else if (event_params[j] == "CslANI") {
						dataValue = events.CslANI;
					} else if (event_params[j] == "CslDNIS") {
						dataValue = events.CslDNIS;
					} else if (event_params[j] == "ServiceUnitType") {
						dataValue = events.ServiceUnitType;
					} else if (event_params[j] == "ReqSkill") {
						dataValue = events.ReqSkill;
					} else if (event_params[j] == "ResSkill") {
						dataValue = events.ResSkill;
					} else if (event_params[j] == "CallType") {
						dataValue = events.CallType;
					} else if (event_params[j] == "IVRData") {
						dataValue = events.IVRData;
					}
					openUrl = openUrl + "&" + paramName + "=" + dataValue;
				}

			}
		}
		showDebugMessage("openUrl: " + openUrl);
		window.open(openUrl);
	}
	
	// 回调业务系统弹屏
	function doCallBackOpen(events) {
		/*
		var i = 0;
		var result = "{";
		for (i = 0; i < event_params.length; i++) {
			
			// JSON属性名称
			result = result + '"' + event_params[i] + '":';
			result = result + '"' + events[event_params[i]] + '",';
		}
		
		result = result.substring(0, result.length - 1);
		result = result + "}"*/
		showDebugMessage("doCallBackOpen开始执行。 ");
		ucwindows.callIn(events);
	}

	var debug_mode = false;
	function showDebugMessage(showMessage) {
		if (debug_mode) {
			alert(showMessage);
		}
	}
	
	var errorlog_mode = false;
	function showErrorMessage(errorMessage) {
		if (errorlog_mode) {
			alert(errorMessage);
		}
	}

	// 暂停执行函数
	function Pause(obj,iMinSecond) {     
    if (window.eventList==null)   
        window.eventList=new Array();     
    var ind=-1;     
    for (var i=0;i<window.eventList.length;i++)  
    {      
        if (window.eventList[i]==null)   
        {       
            window.eventList[i]=obj;   
            ind=i;      
            break;     
        }   
    }       
    if (ind==-1)  
    {      
        ind=window.eventList.length;      
        window.eventList[ind]=obj;     
    }     
    setTimeout("GoOn(" + ind + ")",iMinSecond);    
}   
  
  // 继续函数
 function GoOn(ind) {     
    var obj=window.eventList[ind];     
    window.eventList[ind]=null;     
    if (obj.NextStep)   
        obj.NextStep();     
    else obj();    
 }
    
  // 继续函数
  function sleep(sleeptime) { 
    var start=new   Date().getTime(); 
    while(true)   if(new Date().getTime()-start> sleeptime)
    break; 
  } 
  
	function killErrors(){
		return true;
	}
	window.onerror = killErrors;
	
	
	var data={result:'0000',ani:'13581797617'};
	var adt=window.adt=(function(){
		return{}
	})();
	
	function test01(){
	  if(adt){
		if(adt.test){
			adt.test(data);
		}
	  }
	  
	  if(AgentEventTool.AgentStatus_Connecting){
							AgentEventTool.AgentStatus_Connecting("你好，nihao!");
						}

}

/*
* 同步跨域调用--公用方法,需要引用JQuery
* beforeSend没有内容，可以传入null
* Add=lihf,Date=20140520
*/
function AjaxPostAsyncCrossDomain(url, postBody, beforeSend, CallbackName) {
    $.ajax({
        async: false,
        type: "get",
        url: url,
        dataType: "jsonp",
        data: postBody,
        jsonp: "callback",
        scriptCharset: "UTF-8",
        beforeSend: beforeSend,
        success: CallbackName,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中
            // 只有一个会包含信息
            alert(XMLHttpRequest.responseText);
        }
    });
}


//function getRecordUrl(sessionId, CallbackName) {

//    var requestUrl = "http://60.10.131.130/cca/record/RecordFileAction.do?reqCode=queryBySessionID&entID=YCW&sessionId=" + sessionId;
//    //var requestUrl = "http://60.10.131.130/cca/record/RecordFileAction.do?reqCode=queryBySessionID&entID=YCW&sessionId=64537c0a311019e6";
//    jQuery.ajax({
//        async: false,
//        type: "get",
//        url: requestUrl,
//        dataType: "jsonp",
//        jsonp: "callback",
//        scriptCharset: "UTF-8",
//        beforeSend: function (XMLHttpRequest) {

//        },
//        success: CallbackName,
//        complete: function (XMLHttpRequest, textStatus) {

//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            alert(XMLHttpRequest.responseText);
//        }
//    })
//}



/*
/*业务页面事件对象，封装外呼客户振铃、通话、挂断断事件
*/
var AgentEventTool = window.AgentEventTool = (function () {
    this.CallID = "";
    this.recUrl = "";
    this.SessionID = "";
    this.InitiatedTime = 0;
    this.NetworkReachedTime = 0;
    this.EstablishedTime = 0;

    //坐席振铃时长=对方振铃时间-外拨开始时间
    this.AgentRingTime = function () {
        return parseInt((this.NetworkReachedTime - this.InitiatedTime) / 1000);
    }
    //客户振铃时长=电话接通时间-对方振铃时间
    this.CustomerRingTime = function () {
        return parseInt((this.EstablishedTime - this.NetworkReachedTime) / 1000);
    }

    this.handlerURL = "http://ncc.sys1.bitauto.com/AjaxServers/UCDemoHandler.ashx";

    this.getRecordUrl = function getRecordUrl(sessionId, CallbackName) {

        var requestUrl = "http://60.10.131.130/cca/record/RecordFileAction.do?reqCode=queryBySessionID&entID=YCW&sessionId=" + sessionId;
        jQuery.ajax({
            async: false,
            type: "get",
            url: requestUrl,
            dataType: "jsonp",
            jsonp: "callback",
            scriptCharset: "UTF-8",
            beforeSend: function (XMLHttpRequest) {

            },
            success: CallbackName,
            complete: function (XMLHttpRequest, textStatus) {

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest.responseText);
            }
        })
    }


    return {
        CallID: this.CallID,
        SessionID: this.SessionID,
        InitiatedTime: this.InitiatedTime,
        NetworkReachedTime: this.NetworkReachedTime,
        EstablishedTime: this.EstablishedTime,
        AgentRingTime: this.AgentRingTime,
        CustomerRingTime: this.CustomerRingTime,
        handlerURL: this.handlerURL,
        getRecordUrl:this.getRecordUrl
    }
})();