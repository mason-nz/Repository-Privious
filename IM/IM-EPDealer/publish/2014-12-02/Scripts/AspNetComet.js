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

    //接收文本消息
    this.successHandlers = new Array();
    //失败消息
    this.failureHandlers = new Array();
    //超时消息
    this.timeoutHandlers = new Array();
    //为网友分配坐席
    this.AllocAgentForUserHandlers = new Array();
    //通知网友坐席全忙
    this.MAllBussyHandlers = new Array();
    //通知坐席离开
    this.AgentLeaveHandLers = new Array();
    //坐席给网友发满意度消息
    this.SatisfactionHandLers = new Array();
    //发送文件消息
    this.SendFileHandLers = new Array();
    //发送排队顺序消息
    this.QueueSortHandLers = new Array();

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

AspNetComet.prototype.addQueueSortHandLer =
function AspNetComet_addQueueSortHandLer(func) {
    this.QueueSortHandLers[this.QueueSortHandLers.length] = func;
}


AspNetComet.prototype.addSendFileHandLer =
function AspNetComet_addSendFileHandLer(func) {
    this.SendFileHandLers[this.SendFileHandLers.length] = func;
}


AspNetComet.prototype.addSatisfactionHandLer =
function AspNetComet_addSatisfactionHandLer(func) {
    this.SatisfactionHandLers[this.SatisfactionHandLers.length] = func;
}


AspNetComet.prototype.addAllocAgentForUserHandler =
function AspNetComet_addAllocAgentForUserHandler(func) {
    this.AllocAgentForUserHandlers[this.AllocAgentForUserHandlers.length] = func;
}

AspNetComet.prototype.addAgentLeaveHandler =
function AspNetComet_addAgentLeaveHandler(func) {
    this.AgentLeaveHandLers[this.AgentLeaveHandLers.length] = func;
}

AspNetComet.prototype.addMAllBussyHandler =
function AspNetComet_addMAllBussyHandler(func) {
    this.MAllBussyHandlers[this.MAllBussyHandlers.length] = func;
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




AspNetComet.prototype.callQueueSortHandLers =
function AspNetComet_callQueueSortHandLers(privateToken, alias, message) {
    for (var i = 0; i < this.QueueSortHandLers.length; i++) {
        this.QueueSortHandLers[i](privateToken, alias, message);
    }
}



AspNetComet.prototype.callSendFileHandLers =
function AspNetComet_callSendFileHandLers(privateToken, alias, message) {
    for (var i = 0; i < this.SendFileHandLers.length; i++) {
        this.SendFileHandLers[i](privateToken, alias, message);
    }
}

AspNetComet.prototype.callSatisfactionHandLers =
function AspNetComet_callSatisfactionHandLers(privateToken, alias, message) {
    for (var i = 0; i < this.SatisfactionHandLers.length; i++) {
        this.SatisfactionHandLers[i](privateToken, alias, message);
    }
}


AspNetComet.prototype.callAllocAgentForUserHandlers =
function AspNetComet_callAllocAgentForUserHandlers(privateToken, alias, message) {
    for (var i = 0; i < this.AllocAgentForUserHandlers.length; i++) {
        this.AllocAgentForUserHandlers[i](privateToken, alias, message);
    }
}

AspNetComet.prototype.callMAllBussyHandlers =
function AspNetComet_callMAllBussyHandlers(privateToken, alias, message) {
    for (var i = 0; i < this.MAllBussyHandlers.length; i++) {
        this.MAllBussyHandlers[i](privateToken, alias, message);
    }
}

AspNetComet.prototype.callAgentLeaveHandLers =
function AspNetComet_callAgentLeaveHandLers(privateToken, alias, message) {
    for (var i = 0; i < this.AgentLeaveHandLers.length; i++) {
        this.AgentLeaveHandLers[i](privateToken, alias, message);
    }
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
                        //分配坐席                           

                        //告诉网友坐席全忙                           

                        default:
                            //  else, lets go for it, iterate through the
                            //  returned messages and call the success handlers
                            //for (var i = 0; i < result.length; i++) {
                            for (var i = result.length; i > 0; i--) {

                                var message = result[i - 1];
                                //  get the last messageId
                                if (aspNetComet.lastMessageId < message.mid) {
                                    aspNetComet.lastMessageId = message.mid;
                                }
                                //坐席全忙
                                if (message.n == "MAllBussy") {
                                    //  its a timeout, so lets continue with this
                                    aspNetComet.callMAllBussyHandlers(aspNetComet.privateToken, aspNetComet.alias, message.c);
                                }
                                //分配坐席
                                else if (message.n == "MAllocAgent") {
                                    aspNetComet.callAllocAgentForUserHandlers(aspNetComet.privateToken, aspNetComet.alias, message.c);
                                }
                                //坐席离线
                                else if (message.n == "MLline") {
                                    aspNetComet.callAgentLeaveHandLers(aspNetComet.privateToken, aspNetComet.alias, message.c);
                                }
                                //坐席向网友发起满意度评价                                }
                                else if (message.n == "MSatisfaction") {
                                    aspNetComet.callSatisfactionHandLers(aspNetComet.privateToken, aspNetComet.alias, message.c);
                                }
                                //上传文件
                                else if (message.n == "MSendFile") {
                                    aspNetComet.callSendFileHandLers(aspNetComet.privateToken, aspNetComet.alias, message.c);
                                }
                                //发送排队顺序消息
                                else if (message.n == "MQueueSort") {
                                    aspNetComet.callQueueSortHandLers(aspNetComet.privateToken, aspNetComet.alias, message.c);
                                }
                                else {
                                    //  and now lets call the success handler
                                    aspNetComet.callSuccessHandlers(aspNetComet.privateToken, aspNetComet.alias, message);
                                }
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
            else if (waitRequest.status == "0") {
                //网络异常重新订阅
                aspNetComet.subscribe();
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

/* -------------------------------------------------------------------------------------------------------------------

http://www.JSON.org/json2.js
2008-03-24

Public Domain.

NO WARRANTY EXPRESSED OR IMPLIED. USE AT YOUR OWN RISK.

See http://www.JSON.org/js.html

This file creates a global JSON object containing three methods: stringify,
parse, and quote.


JSON.stringify(value, replacer, space)
value       any JavaScript value, usually an object or array.

replacer    an optional parameter that determines how object
values are stringified for objects without a toJSON
method. It can be a function or an array.

space       an optional parameter that specifies the indentation
of nested structures. If it is omitted, the text will
be packed without extra whitespace. If it is a number,
it will specify the number of spaces to indent at each
level. If it is a string (such as '\t'), it contains the
characters used to indent at each level.

This method produces a JSON text from a JavaScript value.

When an object value is found, if the object contains a toJSON
method, its toJSON method will be called and the result will be
stringified. A toJSON method does not serialize: it returns the
value represented by the name/value pair that should be serialized,
or undefined if nothing should be serialized. The toJSON method will
be passed the key associated with the value, and this will be bound
to the object holding the key.

This is the toJSON method added to Dates:

function toJSON(key) {
return this.getUTCFullYear()   + '-' +
f(this.getUTCMonth() + 1) + '-' +
f(this.getUTCDate())      + 'T' +
f(this.getUTCHours())     + ':' +
f(this.getUTCMinutes())   + ':' +
f(this.getUTCSeconds())   + 'Z';
}

You can provide an optional replacer method. It will be passed the
key and value of each member, with this bound to the containing
object. The value that is returned from your method will be
serialized. If your method returns undefined, then the member will
be excluded from the serialization.

If no replacer parameter is provided, then a default replacer
will be used:

function replacer(key, value) {
return Object.hasOwnProperty.call(this, key) ?
value : undefined;
}

The default replacer is passed the key and value for each item in
the structure. It excludes inherited members.

If the replacer parameter is an array, then it will be used to
select the members to be serialized. It filters the results such
that only members with keys listed in the replacer array are
stringified.

Values that do not have JSON representaions, such as undefined or
functions, will not be serialized. Such values in objects will be
dropped; in arrays they will be replaced with null. You can use
a replacer function to replace those with JSON values.
JSON.stringify(undefined) returns undefined.

The optional space parameter produces a stringification of the value
that is filled with line breaks and indentation to make it easier to
read.

If the space parameter is a non-empty string, then that string will
be used for indentation. If the space parameter is a number, then
then indentation will be that many spaces.

Example:

text = JSON.stringify(['e', {pluribus: 'unum'}]);
// text is '["e",{"pluribus":"unum"}]'


text = JSON.stringify(['e', {pluribus: 'unum'}], null, '\t');
// text is '[\n\t"e",\n\t{\n\t\t"pluribus": "unum"\n\t}\n]'


JSON.parse(text, reviver)
This method parses a JSON text to produce an object or array.
It can throw a SyntaxError exception.

The optional reviver parameter is a function that can filter and
transform the results. It receives each of the keys and values,
and its return value is used instead of the original value.
If it returns what it received, then the structure is not modified.
If it returns undefined then the member is deleted.

Example:

// Parse the text. Values that look like ISO date strings will
// be converted to Date objects.

myData = JSON.parse(text, function (key, value) {
var a;
if (typeof value === 'string') {
a =
/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/.exec(value);
if (a) {
return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4],
+a[5], +a[6]));
}
}
return value;
});


JSON.quote(text)
This method wraps a string in quotes, escaping some characters
as needed.


This is a reference implementation. You are free to copy, modify, or
redistribute.

USE YOUR OWN COPY. IT IS EXTREMELY UNWISE TO LOAD THIRD PARTY
CODE INTO YOUR PAGES.
*/

/*jslint regexp: true, forin: true, evil: true */

/*global JSON */

/*members "", "\b", "\t", "\n", "\f", "\r", "\"", JSON, "\\", apply,
call, charCodeAt, floor, getUTCDate, getUTCFullYear, getUTCHours,
getUTCMinutes, getUTCMonth, getUTCSeconds, hasOwnProperty, join, length,
parse, propertyIsEnumerable, prototype, push, quote, replace, stringify,
test, toJSON, toString
*/

if (!this.JSON) {

    // Create a JSON object only if one does not already exist. We create the
    // object in a closure to avoid global variables.

    JSON = function () {

        function f(n) {    // Format integers to have at least two digits.
            return n < 10 ? '0' + n : n;
        }

        Date.prototype.toJSON = function () {

            // Eventually, this method will be based on the date.toISOString method.

            return this.getUTCFullYear() + '-' +
                 f(this.getUTCMonth() + 1) + '-' +
                 f(this.getUTCDate()) + 'T' +
                 f(this.getUTCHours()) + ':' +
                 f(this.getUTCMinutes()) + ':' +
                 f(this.getUTCSeconds()) + 'Z';
        };


        var escapeable = /["\\\x00-\x1f\x7f-\x9f]/g,
            gap,
            indent,
            meta = {    // table of character substitutions
                '\b': '\\b',
                '\t': '\\t',
                '\n': '\\n',
                '\f': '\\f',
                '\r': '\\r',
                '"': '\\"',
                '\\': '\\\\'
            },
            rep;


        function quote(string) {

            // If the string contains no control characters, no quote characters, and no
            // backslash characters, then we can safely slap some quotes around it.
            // Otherwise we must also replace the offending characters with safe escape
            // sequences.

            return escapeable.test(string) ?
                '"' + string.replace(escapeable, function (a) {
                    var c = meta[a];
                    if (typeof c === 'string') {
                        return c;
                    }
                    c = a.charCodeAt();
                    return '\\u00' + Math.floor(c / 16).toString(16) +
                                               (c % 16).toString(16);
                }) + '"' :
                '"' + string + '"';
        }


        function str(key, holder) {

            // Produce a string from holder[key].

            var i,          // The loop counter.
                k,          // The member key.
                v,          // The member value.
                length,
                mind = gap,
                partial,
                value = holder[key];

            // If the value has a toJSON method, call it to obtain a replacement value.

            if (value && typeof value === 'object' &&
                    typeof value.toJSON === 'function') {
                value = value.toJSON(key);
            }

            // If we were called with a replacer function, then call the replacer to
            // obtain a replacement value.

            if (typeof rep === 'function') {
                value = rep.call(holder, key, value);
            }

            // What happens next depends on the value's type.

            switch (typeof value) {
                case 'string':
                    return quote(value);

                case 'number':

                    // JSON numbers must be finite. Encode non-finite numbers as null.

                    return isFinite(value) ? String(value) : 'null';

                case 'boolean':
                case 'null':

                    // If the value is a boolean or null, convert it to a string. Note:
                    // typeof null does not produce 'null'. The case is included here in
                    // the remote chance that this gets fixed someday.

                    return String(value);

                    // If the type is 'object', we might be dealing with an object or an array or
                    // null.

                case 'object':

                    // Due to a specification blunder in ECMAScript, typeof null is 'object',
                    // so watch out for that case.

                    if (!value) {
                        return 'null';
                    }

                    // Make an array to hold the partial results of stringifying this object value.

                    gap += indent;
                    partial = [];

                    // If the object has a dontEnum length property, we'll treat it as an array.

                    if (typeof value.length === 'number' &&
                        !(value.propertyIsEnumerable('length'))) {

                        // The object is an array. Stringify every element. Use null as a placeholder
                        // for non-JSON values.

                        length = value.length;
                        for (i = 0; i < length; i += 1) {
                            partial[i] = str(i, value) || 'null';
                        }

                        // Join all of the elements together, separated with commas, and wrap them in
                        // brackets.

                        v = partial.length === 0 ? '[]' :
                        gap ? '[\n' + gap + partial.join(',\n' + gap) +
                                  '\n' + mind + ']' :
                              '[' + partial.join(',') + ']';
                        gap = mind;
                        return v;
                    }

                    // If the replacer is an array, use it to select the members to be stringified.

                    if (typeof rep === 'object') {
                        length = rep.length;
                        for (i = 0; i < length; i += 1) {
                            k = rep[i];
                            if (typeof k === 'string') {
                                v = str(k, value, rep);
                                if (v) {
                                    partial.push(quote(k) + (gap ? ': ' : ':') + v);
                                }
                            }
                        }
                    } else {

                        // Otherwise, iterate through all of the keys in the object.

                        for (k in value) {
                            v = str(k, value, rep);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }

                    // Join all of the member texts together, separated with commas,
                    // and wrap them in braces.

                    v = partial.length === 0 ? '{}' :
                    gap ? '{\n' + gap + partial.join(',\n' + gap) +
                              '\n' + mind + '}' :
                          '{' + partial.join(',') + '}';
                    gap = mind;
                    return v;
            }
        }


        // Return the JSON object containing the stringify, parse, and quote methods.

        return {
            stringify: function (value, replacer, space) {

                // The stringify method takes a value and an optional replacer, and an optional
                // space parameter, and returns a JSON text. The replacer can be a function
                // that can replace values, or an array of strings that will select the keys.
                // A default replacer method can be provided. Use of the space parameter can
                // produce text that is more easily readable.

                var i;
                gap = '';
                indent = '';
                if (space) {

                    // If the space parameter is a number, make an indent string containing that
                    // many spaces.

                    if (typeof space === 'number') {
                        for (i = 0; i < space; i += 1) {
                            indent += ' ';
                        }

                        // If the space parameter is a string, it will be used as the indent string.

                    } else if (typeof space === 'string') {
                        indent = space;
                    }
                }

                // If there is no replacer parameter, use the default replacer.

                if (!replacer) {
                    rep = function (key, value) {
                        if (!Object.hasOwnProperty.call(this, key)) {
                            return undefined;
                        }
                        return value;
                    };

                    // The replacer can be a function or an array. Otherwise, throw an error.

                } else if (typeof replacer === 'function' ||
                        (typeof replacer === 'object' &&
                         typeof replacer.length === 'number')) {
                    rep = replacer;
                } else {
                    throw new Error('JSON.stringify');
                }

                // Make a fake root object containing our value under the key of ''.
                // Return the result of stringifying the value.

                return str('', { '': value });
            },


            parse: function (text, reviver) {

                // The parse method takes a text and an optional reviver function, and returns
                // a JavaScript value if the text is a valid JSON text.

                var j;

                function walk(holder, key) {

                    // The walk method is used to recursively walk the resulting structure so
                    // that modifications can be made.

                    var k, v, value = holder[key];
                    if (value && typeof value === 'object') {
                        for (k in value) {
                            if (Object.hasOwnProperty.call(value, k)) {
                                v = walk(value, k);
                                if (v !== undefined) {
                                    value[k] = v;
                                } else {
                                    delete value[k];
                                }
                            }
                        }
                    }
                    return reviver.call(holder, key, value);
                }


                // Parsing happens in three stages. In the first stage, we run the text against
                // regular expressions that look for non-JSON patterns. We are especially
                // concerned with '()' and 'new' because they can cause invocation, and '='
                // because it can cause mutation. But just to be safe, we want to reject all
                // unexpected forms.

                // We split the first stage into 4 regexp operations in order to work around
                // crippling inefficiencies in IE's and Safari's regexp engines. First we
                // replace all backslash pairs with '@' (a non-JSON character). Second, we
                // replace all simple value tokens with ']' characters. Third, we delete all
                // open brackets that follow a colon or comma or that begin the text. Finally,
                // we look to see that the remaining characters are only whitespace or ']' or
                // ',' or ':' or '{' or '}'. If that is so, then the text is safe for eval.

                if (/^[\],:{}\s]*$/.test(text.replace(/\\["\\\/bfnrtu]/g, '@').
replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']').
replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {

                    // In the second stage we use the eval function to compile the text into a
                    // JavaScript structure. The '{' operator is subject to a syntactic ambiguity
                    // in JavaScript: it can begin a block or an object literal. We wrap the text
                    // in parens to eliminate the ambiguity.

                    j = eval('(' + text + ')');

                    // In the optional third stage, we recursively walk the new structure, passing
                    // each name/value pair to a reviver function for possible transformation.

                    return typeof reviver === 'function' ?
                        walk({ '': j }, '') : j;
                }

                // If the text is not JSON parseable, then a SyntaxError is thrown.

                throw new SyntaxError('JSON.parse');
            },

            quote: quote
        };
    } ();
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