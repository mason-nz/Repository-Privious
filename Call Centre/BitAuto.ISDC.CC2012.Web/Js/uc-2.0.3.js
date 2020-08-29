﻿ 	
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
	// 设置页面标识
	var httpServerAdressSetPageIndex = "http://127.0.0.1:9009/State/SetPageIndex?callback=?";
	
	// 弹屏地址
	var httpServerAdressGetAgent = "http://127.0.0.1:9009/State/AgentInfo?callback=?";
	//咨询
	var httpServerAdressConsulting = "http://127.0.0.1:9009/CallControl/Consult?callback=?";
	//转移
	var httpServerAdressShift = "http://127.0.0.1:9009/CallControl/Transfer?callback=?";
	
	var openUrl = "http://www.huayunworld.com/";
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
	var m_PageIndex = "";
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
	
	var handle;

	//企业号变量
	var enterpriseCode = "INYCW";

	/* UCObj version 2.0 pre
	 * creator: tang-xue (tangxue@huayunw.com)
	 * 最新更新：2012-2-1
	 */
	function UCObj(win, doc) {
	
	 	ucwindows = win;
	 	ucdocument = doc;
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
			showDebugMessage("Send Start..................");
			handle = win.setInterval("getEvent()",refreshTime); 
			showDebugMessage("Send End..................");
		}
		
		this.stopObj = function() { 
			showDebugMessage("Send Start..................");
			clearInterval(handle);
			showDebugMessage("Send End..................");
		}
		 
		/*
		 * 外呼处理
		 */
		this.doCallOut = function (callNum, showNum) {
		    this.setPageIndex(AgentEventTool.Guid);
		    showDebugMessage("doCallOut Start..................");

		    if (callNum.length == 12 && callNum.indexOf("01") == 0) {
		        callNum = callNum.substring(1);
		    }

		    // 外拨号码前需加Tel:的前缀
		    if (callNum.indexOf("Tel:") == -1) {
		        callNum = "Tel:" + callNum;
		    }

		    var makeCallType = "&MakeCallType=5";
		    if (showNum == "") {
		        makeCallType = "&MakeCallType=2";
		    }

		    var callOutUrl = httpServerAdressCallOut + "&TargetDN=" + callNum + "&ShowANI=" + showNum + "&MakeCallType=2";
		    var msg = "";

		    jQuery.ajax({
		        type: requestType,
		        url: callOutUrl,
		        dataType: "jsonp",
		        jsonp: "callback",
		        beforeSend: function (XMLHttpRequest) {
		            showDebugMessage("beforeSend Start..................");
		        },
		        success: function (data) {
		            showDebugMessage("success Start..................");
		            // 处理结果
		            ucwindows.showOpeResultMsg(operation_type[0], data.Code, data.Msg);
		        },
		        complete: function (XMLHttpRequest, textStatus) {
		            showDebugMessage("complete Start..................");
		        },
		        error: function (XMLHttpRequest, textStatus, errorThrown) {
		            //请求出错处理
		            showDebugMessage("外呼失败 " + XMLHttpRequest.statusText);
		            ucwindows.showOpeResultMsg(operation_type[0], "-2", "请求失败。");
		        }
		    })

		    showDebugMessage("doCallOut End..................");
		    return msg;
		}

		/*
		 * 置闲处理
		 */
		this.doReady = function() {

			showDebugMessage("doReady Start..................");

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
		
			/*
			 * 设置页面标识
			 */
			this.setPageIndex = function(pageIndex) {

			showDebugMessage("setPageIndex Start..................");
			
			var setPageIndextUrl = httpServerAdressSetPageIndex + "&index=" + pageIndex;
			
			var msg = "";
      
      // 设置UC-JS的页面标识
      m_PageIndex = pageIndex;
      
			jQuery.ajax({
	  		type: requestType,
			  url: setPageIndextUrl,
			  dataType: "jsonp",
			  jsonp: "callback",
			  beforeSend: function(XMLHttpRequest){
			   showDebugMessage("beforeSend Start..................");
			  },
			  success: function(data){
			  	showDebugMessage("success Start..................");
			  	// 处理结果
					
			  },
			  complete: function(XMLHttpRequest, textStatus){
			   	showDebugMessage("complete Start..................");

			  },
			  error: function(XMLHttpRequest, textStatus, errorThrown){
			   //请求出错处理
			   showDebugMessage("挂断失败: " + XMLHttpRequest.statusText);
			  }
			})

			showDebugMessage("setPageIndex End..................");
			return msg;
		}
	
		this.getAgent = function() {
			showDebugMessage("getAgent Start..................");
			var msg = "";

			jQuery.ajax({
	  		    type: requestType,
			    url: httpServerAdressGetAgent,
			    dataType: "jsonp",
			    jsonp: "callback",
			    beforeSend: function(XMLHttpRequest){
			    showDebugMessage("beforeSend Start..................");
			  },
			  success: function(data){
				 
			  	showDebugMessage("success Start..................");
			  	// 处理结果
					ucwindows.setAgentInfo(data);
					
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

			showDebugMessage("getAgent End..................");
			return msg;
		}
		
		
		this.consulting = function(targetdn,consulttype) {
			showDebugMessage("consulting Start..................");
			var msg = "";
			// 外拨号码前需加Tel:的前缀
			if (targetdn.indexOf("Tel:") == -1) {
				targetdn = "Tel:" + targetdn;
			}

			var consultingURL = httpServerAdressConsulting + "&TARGETDN="+targetdn+"&CONSULTTYPE="+consulttype;
			
			jQuery.ajax({
	  		type: requestType,
			  url:consultingURL,
			  dataType: "jsonp",
			  jsonp: "callback",
			  beforeSend: function(XMLHttpRequest){
			   showDebugMessage("beforeSend Start..................");
			  },
			  success: function(data){

			  	showDebugMessage("success Start..................");
			  	// 处理结果
					
			  },
			  complete: function(XMLHttpRequest, textStatus){
				  
			   	showDebugMessage("complete Start..................");

			  },
			  error: function(XMLHttpRequest, textStatus, errorThrown){
			   //请求出错处理
			   alert("error");
			  }
			})

			showDebugMessage("consulting End..................");
			return msg;
		}
		
		this.shift = function() {
			showDebugMessage("shift Start..................");
			var msg = "";
			jQuery.ajax({
	  		type: requestType,
			  url:httpServerAdressShift ,
			  dataType: "jsonp",
			  jsonp: "callback",
			  beforeSend: function(XMLHttpRequest){
			   showDebugMessage("beforeSend Start..................");
			  },
			  success: function(data){
			  	showDebugMessage("success Start..................");
			  	// 处理结果
			  },
			  complete: function(XMLHttpRequest, textStatus){
				  
			   	showDebugMessage("complete Start..................");

			  },
			  error: function(XMLHttpRequest, textStatus, errorThrown){
			   //请求出错处理

			  }
			})

			showDebugMessage("shift End..................");
			return msg;
		}
	}

	/*
	 * 解析事件，如果为振铃事件进行弹屏处理
	 */
	function getEvent() {


			var eventUrl = httpServerAdress  + "&index=" + m_PageIndex;

			jQuery.ajax({
			    type: requestType,
			    url: eventUrl,
			    dataType: "jsonp",
			    jsonp: "callback",
			    scriptCharset: "UTF-8",
			    beforeSend: function (XMLHttpRequest) {
			        showDebugMessage("beforeSend Start..................");
			    },
			    success: function (data) {
			        showDebugMessage("success Start..................");

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

			        var csldnis = data.CslDNIS;
			        if (csldnis.indexOf("tel:") > -1 || csldnis.indexOf("Tel:") > -1) {
			            csldnis = csldnis.substring(4);
			        }
			        data.CslDNIS = csldnis;

			        // 事件类型
			        var EventType = data.EventType;
			        if (EventType == AgentStatus_Alerting && data.CallDetailEvent == "CA_CALL_EVENT_ALERTING") {
			            if (openWindowMode == "UC_OPEN") {
			                doOpenUrl(data);
			            } else {
			                doCallBackOpen(data);
			            }
			        }


			        //alert("02EventType:" + data.EventType + ",ANI:" + ani + ",DNIS:" + data.DNIS + ",SessionID:" + data.SessionID + ",Result:" + data.Result);
			        var dnis = data.DNIS;
			        if (dnis.indexOf("tel:") > -1 || dnis.indexOf("Tel:") > -1) {
			            dnis = dnis.substring(4);
			        }
			        data.DNIS = dnis;

			        var sessionid = data.SessionID;
			        if (sessionid.indexOf(":") > -1) {
			            var arr = sessionid.split(":");
			            data.SessionID = arr[0];
			            //记录企业号
			            enterpriseCode = arr[1];
			        }

			        //页面唯一标识
			        var PageIndex = data.ActivePageIndex;
			        if (EventType == AgentStatus_Connecting && PageIndex == AgentEventTool.Guid) {
			            //alert("AgentStatus_Connecting:" + AgentEventTool.Guid);
			            if (AgentEventTool.AgentStatus_Connecting) {
			                AgentEventTool.AgentStatus_Connecting(data);
			            }
			        }

			        // 事件详细类型
			        var CallDetailEvent = data.CallDetailEvent;

			        if (EventType == AgentStatus_Connected_Normal && CallDetailEvent == "CA_CALL_EVENT_OUTBOUND_CONNECTED_OP" && PageIndex == AgentEventTool.Guid) {
			            //alert("AgentStatus_Connected_Normal:" + AgentEventTool.Guid);
			            //客户端版本34
			            if (AgentEventTool.AgentStatus_Connected_Normal) {
			                AgentEventTool.AgentStatus_Connected_Normal(data);
			            }
			        }
			        else if (EventType == AgentStatus_Connected_Normal && CallDetailEvent == "CA_CALL_EVENT_OUTBOUND_CONNECTED_TP") {
			            //客户端版本34
			            //TP:本方接通，坐席绑定电话接通
			        }

			        if (EventType == AgentStatus_Disconnected && PageIndex == AgentEventTool.Guid) {
			            //alert("AgentStatus_Disconnected:" + AgentEventTool.Guid);
			            //呼入电话挂断事件
			            if (data.CallType == "INTERNAL") {
			                if (AgentEventTool.AgentStatus_Disconnected_Inbound) {
			                    AgentEventTool.AgentStatus_Disconnected_Inbound(data);
			                }
			            }
			            else {
			                if (AgentEventTool.AgentStatus_Disconnected) {
			                    AgentEventTool.AgentStatus_Disconnected(data);
			                }
			            }

			        }

			        //呼入接通
			        if (EventType == AgentStatus_Connected_Normal && CallDetailEvent == "CA_CALL_EVENT_CONNECTED" && PageIndex == AgentEventTool.Guid) {
			            //alert("AgentStatus_Connected_Normal:" + AgentEventTool.Guid);
			            //客户端版本34
			            //是否呼入接通赋值，呼入接通时间戳赋值
			            AgentEventTool.EstablishedStartTime = data.TimeStamp;
			            AgentEventTool.IsEstablished = "true";
			            if (AgentEventTool.AgentStatus_Connected_Inbound) {
			                AgentEventTool.AgentStatus_Connected_Inbound(data);
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
					} else if (event_params[j] == "SessionID") {
						dataValue = events.SessionID;
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
					} else if (event_params[j] == "SessionID") {
						dataValue = events.SessionID;
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

	/*
	* 置忙处理结果
	* opeType(操作类型):  CallOut: 外呼, Ready: 置闲, NotReady: 置忙
	* opeResultCode(操作结果): -1: UC内部处理失败 -2: Ajax请求UC失败
	* opeResultMsg(操作结果描述)
	*/
	function showOpeResultMsg(opeType, opeResultCode, opeResultMsg) {
	    //alert(opeResultMsg);
	    if (opeType == "CallOut") {
	        msg = "外呼结果: ";
	        if (opeResultCode == "0") {
	            msg = msg + "成功！";
	        } else {
	            msg = msg + opeResultMsg;
	        }
	    } else if (opeType == "Ready") {
	        msg = "置闲结果:";
	        if (opeResultCode == "0") {
	            msg = msg + "成功！";
	        } else {
	            msg = msg + opeResultMsg;
	        }
	    } else if (opeType == "NotReady") {
	        msg = "置忙结果:";
	        if (opeResultCode == "0") {
	            msg = msg + "成功！";
	        } else {
	            msg = msg + opeResultMsg;
	        }
	    }

	    //document.getElementById("CallOut").value = msg;
	}


	/*
	* 来电（放置自己的弹屏代码）
	*/
	var ringTime = 0;
	function callIn(result) {
	    AgentUCobj.setPageIndex(AgentEventTool.Guid);
	    //alert("callIn.Guid:" + AgentEventTool.Guid);
	    // 事件类型
	    var EventType = result.EventType;
	    // 原始主叫
	    var OrgANI = result.OrgANI;
	    // 原始被叫
	    var OrgDNIS = result.OrgDNIS;
	    // 当前主叫
	    var ANI = result.ANI;
	    // 当前被叫
	    var DNIS = result.DNIS;
	    // 咨询主叫
	    var CslANI = result.CslANI;
	    // 咨询被叫
	    var CslDNIS = result.CslDNIS;
	    // 服务类型
	    var ServiceUnitType = result.ServiceUnitType;
	    // 请求技能组
	    var ReqSkill = result.ReqSkill;
	    // 响应技能组
	    var ResSkill = result.ResSkill;
	    // 呼叫类型
	    var CallType = result.CallType;
	    // IVR数据
	    var IVRData = result.IVRData;
	    // SessionID
	    var SessionID = result.SessionID;

	    ringTime = ringTime + 1;
	    var msg = "当来电次数:" + ringTime + "\n";
	    if (EventType == "AgentStatus_Connecting") {
	        msg = msg + "事件类型： " + "电话接通" + "\n";
	    } else if (EventType == "AgentStatus_Disconnected") {
	        msg = msg + "事件类型：  " + "挂断事件" + "\n";
	    } else if (EventType == "AgentStatus_Connected_Normal") {
	        msg = msg + "事件类型： " + "客户接通" + "\n";
	    } else {
	        msg = msg + "事件类型： " + EventType + "\n";
	    }

	    if (DNIS.indexOf("tel:") > -1) {
	        DNIS = DNIS.substring(4);
	    }

	    msg = msg + "当前主叫: " + ANI + "\n";
	    msg = msg + "当前被叫: " + DNIS + "\n";
	    msg = msg + "请求技能组: " + ReqSkill + "\n";
	    msg = msg + "响应技能组: " + ResSkill + "\n";
	    msg = msg + "原始主叫: " + OrgANI + "\n";
	    msg = msg + "原始被叫: " + OrgDNIS + "\n";
	    msg = msg + "咨询主叫: " + CslANI + "\n";
	    msg = msg + "咨询被叫: " + CslDNIS + "\n";
	    msg = msg + "呼叫类型: " + CallType + "\n";
	    msg = msg + "IVR数据: " + IVRData + "\n";
	    msg = msg + "呼叫标识: " + SessionID + "\n";

	    //document.getElementById("Result").value = msg;
	    //alert("callin:" + msg);
	    CallArrive(result);
	}

	/*
	/*业务页面事件对象，封装外呼客户振铃、通话、挂断事件
    /*add by lihf 20140520
	*/

    //声明UC客户端全局对象
	var AgentUCobj = null;
	var AgentEventTool = window.AgentEventTool = (function () {
	    //window.AgentEventTool = (function () {
	    this.CallID = "";//呼入电话CallID是在
	    this.AgentID = "1000";
	    this.recUrl = "";
	    this.SessionID = "";
	    this.InitiatedTime = 0;
	    this.NetworkReachedTime = 0;
	    this.EstablishedTime = 0;
	    this.Guid = "";
	    this.IsEstablished = "false"; //呼入电话是否接通
	    this.EstablishedStartTime = 0;//呼入接通时间戳


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

	        var requestUrl = "http://60.10.131.130/cca/record/RecordFileAction.do?reqCode=queryBySessionID&entID=" + enterpriseCode + "&sessionId=" + sessionId;
	        //var requestUrl = "http://60.10.131.130/cca/record/RecordFileAction.do?reqCode=queryBySessionID&entID=YCW1&sessionId=" + sessionId;
	        //var requestUrl = "http://60.10.131.130/cca/record/RecordFileAction.do?reqCode=queryBySessionID&sessionId=" + sessionId;
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

	    this.UCInit = function UCInit() {
	        AgentUCobj = new UCObj(window, document);
	        //AgentUCobj.stopObj();
	        AgentUCobj.start();
	        AgentUCobj.getAgent();
	        this.Guid = newGuid();
	        AgentUCobj.setPageIndex(AgentEventTool.Guid);
	        //alert("this.Guid:" + this.Guid);
	    }

	    this.UCSleep = function UCSleep(numberMillis) {
	        var now = new Date();
	        var exitTime = now.getTime() + numberMillis;
	        while (true) {
	            now = new Date();
	            if (now.getTime() > exitTime)
	                return;
	        }
	    }

	    return {
	        CallID: this.CallID,
	        AgentID: this.AgentID,
	        SessionID: this.SessionID,
	        InitiatedTime: this.InitiatedTime,
	        NetworkReachedTime: this.NetworkReachedTime,
	        EstablishedTime: this.EstablishedTime,
	        AgentRingTime: this.AgentRingTime,
	        CustomerRingTime: this.CustomerRingTime,
	        handlerURL: this.handlerURL,
	        getRecordUrl: this.getRecordUrl,
	        UCInit: this.UCInit,
	        UCSleep: this.UCSleep,
	        Guid: this.Guid,
	        IsEstablished: this.IsEstablished,
	        EstablishedStartTime: this.EstablishedStartTime
	    }
	})();

	//初始化对象
	AgentEventTool.UCInit();
    function newGuid() {
        var guid = "";
        for (var i = 1; i <= 32; i++) {
            var n = Math.floor(Math.random() * 16.0).toString(16);
            guid += n;
            if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
                guid += "-";
        }
        return guid;
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

    //获取登录用户，密码信息回调函数
	function setAgentInfo(result) {
	    //            var entID = "HYCC";
	    //            var userName = result.AgentID;
	    //            var password = result.AgentPWD;
	    //alert("result.AgentID1：" + result.AgentID);
	    AgentEventTool.AgentID = result.AgentID;
	    //alert("result.AgentID2：" + AgentEventTool.AgentID);
	}

	//呼入振铃
	function CallArrive(data) {
	    //alert("callarrive");
	    var callType = 1;
	    //	    var callNum = data.OrgANI;
	    var callNum = data.ANI;

	    var dataSource = "";

	    if (data.DNIS == 2446) {
	        dataSource = 168;
	    }
	    else {
	        dataSource = 168;
	    }

	    var beginRingTime = "";
	    beginRingTime = data.TimeStamp;
	    var url = "";
	}