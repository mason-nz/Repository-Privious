/*
ASP.NET Comet JavaScript
*/


//
//  A AspNetComet object that represents a client
//  subscribed to a comet channel on the ASP.NET Server
//
//  handler:        The Path to the handler
//  privateToken:   The Client's private token
//  alias:          An alias for the channel
//
function AspNetComet(handler, privateToken, alias) {
    this.handler = handler;
    this.successHandlers = new Array();
    this.failureHandlers = new Array();
    this.timeoutHandlers = new Array();
    this.lastMessageId = 0;
    this.privateToken = privateToken;
    this.alias = alias;
    this.enabled = true;
}

//
//  get an instance of the XML HTTP Request object
//  that is browser specific
//
AspNetComet.prototype.getXMLHttpRequest =
function AspNetComet_getXMLHttpRequest() {
    if (window.XMLHttpRequest) {
        return new XMLHttpRequest()
    }
    else {
        if (window.ActiveXObject) {
            // ...otherwise, use the ActiveX control for IE5.x and IE6
            return new ActiveXObject("Microsoft.XMLHTTP");
        }
    }
}

//
//  Add a success handler, called when the comet call succeeds with a message
//
//  func:   The function that will be called
//
AspNetComet.prototype.addSuccessHandler =
function AspNetComet_addSuccessHandler(func) {
    this.successHandlers[this.successHandlers.length] = func;
}

//
//  Add a failure handler, called when the comet call fails
//
//  func:   The function that will be called
//
AspNetComet.prototype.addFailureHandler =
function AspNetComet_addFailureHandler(func) {
    this.failureHandlers[this.failureHandlers.length] = func;
}

//
//  Add a timeout handler, called when the comet connection returns with no messages
//
//  func:   The function that will be called
//
AspNetComet.prototype.addTimeoutHandler =
function AspNetComet_addTimeoutHandler(func) {
    this.timeoutHandlers[this.timeoutHandlers.length] = func;
}

//
//  Call all the sucess handlers
//
//  privateToken:   The private token of the client
//  alias:          The alias of the channel
//  message:        The message received from the channel
//
AspNetComet.prototype.callSuccessHandlers =
function AspNetComet_callSuccessHandlers(privateToken, alias, message) {
    for (var i = 0; i < this.successHandlers.length; i++) {
        this.successHandlers[i](privateToken, alias, message);
    }
}

//
//  Call all the failure handlers
//
//  privateToken:   The private token of the client
//  alias:          The alias of the channel
//  error:          The error message received from the server
//
AspNetComet.prototype.callFailureHandlers =
function AspNetComet_callFailureHandlers(privateToken, alias, error) {
    for (var i = 0; i < this.failureHandlers.length; i++) {
        this.failureHandlers[i](privateToken, alias, error);
    }
}

//
//  Call all the timeout handlers
//
//  privateToken:   The private token of the client
//  alias:          The alias of the channel
//
AspNetComet.prototype.callTimeoutHandlers =
function AspNetComet_callTimeoutHandlers(privateToken, alias) {
    for (var i = 0; i < this.timeoutHandlers.length; i++) {
        this.timeoutHandlers[i](privateToken, alias);
    }
}

//
//  unsubscribe from the channel (basically stop the request connecting to the channel after it returns)
//
AspNetComet.prototype.unsubscribe =
function AspNetComet_unsubscribe() {
    this.enabled = false;
}

//
//  subscribe to the channel, and start the comet mechanism
//  
AspNetComet.prototype.subscribe =
function AspNetComet_subscribe() {
    var aspNetComet = this;

    //  get our object that is going to perform the request
    var waitRequest = this.getXMLHttpRequest();

    //  indicate we are enabled
    this.enabled = true;

    waitRequest.onreadystatechange = function () {
        //
        //  validate the ready state, we are looking for "4" ready
        if (waitRequest.readyState == "4") {
            //  and a status code of 200 "OK"
            if (waitRequest.status == "200") {
                //  finished, success or not?
                var result;
                if (waitRequest.responseText == "")
                    result = null;
                else
                    result = JSON.parse(waitRequest.responseText);

                if (result == null || result.length == 0) {
                    //  failure
                    aspNetComet.callFailureHandlers(aspNetComet.privateToken, aspNetComet.alias, null);
                }
                else {
                    //  we have a message but we need to inspect
                    //  to see if this is a failure
                    var message = result[0];


                    switch (message.n) {
                        case "aspNetComet.error":
                            //  yes we do this is a failure
                            //  failure
                            aspNetComet.callFailureHandlers(aspNetComet.privateToken, aspNetComet.alias, message.c);
                            break;
                        case "aspNetComet.timeout":
                            //  its a timeout, so lets continue with this
                            aspNetComet.callTimeoutHandlers(aspNetComet.privateToken, aspNetComet.alias);
                            //  and attach back to the handler
                            if (aspNetComet.enabled) {
                                //  continue if we are enabled
                                aspNetComet.subscribe();
                            }
                            break;
                        default:
                            //  else, lets go for it, iterate through the
                            //  returned messages and call the success handlers
                            for (var i = 0; i < result.length; i++) {
                                var message = result[i];
                                //  get the last messageId
                                aspNetComet.lastMessageId = message.mid;
                                //  and now lets call the success handler
                                aspNetComet.callSuccessHandlers(aspNetComet.privateToken, aspNetComet.alias, message);
                            }

                            //  attach back up to the handler
                            if (aspNetComet.enabled) {
                                //  continue!
                                aspNetComet.subscribe();
                            }
                            break;
                    }
                }
            }
        }
    }

    //
    //  open the post request to the handler
    waitRequest.open("POST", this.handler, true);
    //  and set the request header indicating we are posting form data
    waitRequest.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    //  setup the private token and last message id, these are needed to identify what state we
    //  are actually interested in
    waitRequest.send("privateToken=" + this.privateToken + "&lastMessageId=" + this.lastMessageId);
}

/*
-----设置页面关闭及刷新，
参数flag：true，进行关闭或刷新提示操作；false，不进行关闭或刷新提示操作
参数closeFN：若flag=true时，在关闭或刷新之前进行函数closeFN调用操作。
*/
function SetBeforeunload(flag, closeFN) {
    window.onbeforeunload = (flag != null && flag) ? function () {
        if (closeFN && typeof (closeFN) == 'function')
            closeFN();
        return "";
    } : null;
    //window.onunload = (flag != null && flag) ? function () { window.opener = null; window.open('', '_self'); window.close(); } : null;
}