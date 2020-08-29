
//################################################### 注意，此处的文档包括三部分的JS ##############################################
//AspNetComet.js
//common.js
//OnlineService.js


//###################################################    AspNetComet.js     ##############################################
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
    //转接坐席消息
    this.TransferHandLers = new Array();

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
            return new XMLHttpRequest();
        } else {
            if (window.ActiveXObject) {
                // ...otherwise, use the ActiveX control for IE5.x and IE6
                return new ActiveXObject("Microsoft.XMLHTTP");
            }
        }
    };


AspNetComet.prototype.addTransferHandLers =
    function AspNetComet_addTransferHandLers(func) {
        this.TransferHandLers[this.TransferHandLers.length] = func;
    };

AspNetComet.prototype.addQueueSortHandLer =
    function AspNetComet_addQueueSortHandLer(func) {
        this.QueueSortHandLers[this.QueueSortHandLers.length] = func;
    };


AspNetComet.prototype.addSendFileHandLer =
    function AspNetComet_addSendFileHandLer(func) {
        this.SendFileHandLers[this.SendFileHandLers.length] = func;
    };


AspNetComet.prototype.addSatisfactionHandLer =
    function AspNetComet_addSatisfactionHandLer(func) {
        this.SatisfactionHandLers[this.SatisfactionHandLers.length] = func;
    };


AspNetComet.prototype.addAllocAgentForUserHandler =
    function AspNetComet_addAllocAgentForUserHandler(func) {
        this.AllocAgentForUserHandlers[this.AllocAgentForUserHandlers.length] = func;
    };

AspNetComet.prototype.addAgentLeaveHandler =
    function AspNetComet_addAgentLeaveHandler(func) {
        this.AgentLeaveHandLers[this.AgentLeaveHandLers.length] = func;
    };

AspNetComet.prototype.addMAllBussyHandler =
    function AspNetComet_addMAllBussyHandler(func) {
        this.MAllBussyHandlers[this.MAllBussyHandlers.length] = func;
    };


//
//  Add a success handler, called when the comet call succeeds with a message
//
//  func:   The function that will be called
//
AspNetComet.prototype.addSuccessHandler =
    function AspNetComet_addSuccessHandler(func) {
        this.successHandlers[this.successHandlers.length] = func;
    };

//
//  Add a failure handler, called when the comet call fails
//
//  func:   The function that will be called
//
AspNetComet.prototype.addFailureHandler =
    function AspNetComet_addFailureHandler(func) {
        this.failureHandlers[this.failureHandlers.length] = func;
    };

//
//  Add a timeout handler, called when the comet connection returns with no messages
//
//  func:   The function that will be called
//
AspNetComet.prototype.addTimeoutHandler =
    function AspNetComet_addTimeoutHandler(func) {
        this.timeoutHandlers[this.timeoutHandlers.length] = func;
    };


AspNetComet.prototype.callQueueSortHandLers =
    function AspNetComet_callQueueSortHandLers(privateToken, alias, message) {
        for (var i = 0; i < this.QueueSortHandLers.length; i++) {
            this.QueueSortHandLers[i](privateToken, alias, message);
        }
    };

AspNetComet.prototype.callTransferHandLers =
    function AspNetComet_callTransferHandLers(privateToken, alias, message) {
        for (var i = 0; i < this.TransferHandLers.length; i++) {
            this.TransferHandLers[i](privateToken, alias, message);
        }
    };


AspNetComet.prototype.callSendFileHandLers =
    function AspNetComet_callSendFileHandLers(privateToken, alias, message) {
        for (var i = 0; i < this.SendFileHandLers.length; i++) {
            this.SendFileHandLers[i](privateToken, alias, message);
        }
    };

AspNetComet.prototype.callSatisfactionHandLers =
    function AspNetComet_callSatisfactionHandLers(privateToken, alias, message) {
        for (var i = 0; i < this.SatisfactionHandLers.length; i++) {
            this.SatisfactionHandLers[i](privateToken, alias, message);
        }
    };


AspNetComet.prototype.callAllocAgentForUserHandlers =
    function AspNetComet_callAllocAgentForUserHandlers(privateToken, alias, message) {
        for (var i = 0; i < this.AllocAgentForUserHandlers.length; i++) {
            this.AllocAgentForUserHandlers[i](privateToken, alias, message);
        }
    };

AspNetComet.prototype.callMAllBussyHandlers =
    function AspNetComet_callMAllBussyHandlers(privateToken, alias, message) {
        for (var i = 0; i < this.MAllBussyHandlers.length; i++) {
            this.MAllBussyHandlers[i](privateToken, alias, message);
        }
    };

AspNetComet.prototype.callAgentLeaveHandLers =
    function AspNetComet_callAgentLeaveHandLers(privateToken, alias, message) {
        for (var i = 0; i < this.AgentLeaveHandLers.length; i++) {
            this.AgentLeaveHandLers[i](privateToken, alias, message);
        }
    };

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
    };

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
    };

//
//  Call all the timeout handlers
//
//  privateToken:   The private token of the client
//  alias:          The alias of the channel
//
AspNetComet.prototype.callTimeoutHandlers =
    function AspNetComet_callTimeoutHandlers(privateToken, alias, message) {
        for (var i = 0; i < this.timeoutHandlers.length; i++) {
            this.timeoutHandlers[i](privateToken, alias, message);
        }
    };

//
//  unsubscribe from the channel (basically stop the request connecting to the channel after it returns)
//
AspNetComet.prototype.unsubscribe =
    function AspNetComet_unsubscribe() {
        this.enabled = false;
    };

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
            try {
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
                        } else {
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
                                    aspNetComet.callTimeoutHandlers(aspNetComet.privateToken, aspNetComet.alias, message);
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
                                    for (var i = 0; i < result.length; i++) {
                                        //                                for (var i = result.length; i > 0; i--) {

                                        var message = result[i];
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
                                        //发送排队顺序消息
                                        else if (message.n == "MTransfer") {
                                            aspNetComet.callTransferHandLers(aspNetComet.privateToken, aspNetComet.alias, message.c);
                                        } else {
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

                    } else if (waitRequest.status == "0" || waitRequest.status == "12029" || waitRequest.status == "12030" || waitRequest.status == "12031" || waitRequest.status == "12152" || waitRequest.status == "12159") {
                        //网络异常重新订阅
                        aspNetComet.subscribe();
                    }
                }
            } catch (e) {
                //网络异常重新订阅
                //aspNetComet.subscribe();
            }
        };

        //
        //  open the post request to the handler
        waitRequest.open("POST", this.handler, true);
        //  and set the request header indicating we are posting form data
        waitRequest.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        //  setup the private token and last message id, these are needed to identify what state we
        //  are actually interested in
        waitRequest.send("privateToken=" + this.privateToken + "&lastMessageId=" + this.lastMessageId);

    };

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


//###################################################    common.js          ##############################################


var domainCookie = 'im.sys.bitauto.com';
//消息闪烁相关
// 使用message对象封装消息
var messageFlicker = {
    time: 0,
    title: document.title,
    timer: null,

    // 显示新消息提示   
    show: function () {
        clearTimeout(messageFlicker.timer);
        var title = messageFlicker.title.replace("【　　　】", "").replace("【新消息】", "");
        // 定时器，设置消息切换频率闪烁效果就此产生   
        messageFlicker.timer = setTimeout(
                function () {
                    messageFlicker.time++;
                    messageFlicker.show();

                    if (messageFlicker.time % 2 == 0) {
                        document.title = "【新消息】" + title;
                    }
                    else {
                        document.title = "【　　　】" + title;
                    };
                },
                600 // 闪烁时间差  
            );
        return [messageFlicker.timer, messageFlicker.title];
    },

    // 取消新消息提示   
    clear: function (name) {
        clearTimeout(messageFlicker.timer);
        document.title = (name == null ? "在线客服" : name);
    }
};
function validateEdit(ids) {
    var b = false;
    var ar = document.getElementsByName(ids);
    for (var ii = 0; ii < ar.length; ii++) {
        if (ar[ii].type == "checkbox") {
            if (ar[ii].checked) {
                b = true;
            }
        }
    }
    if (b) {
        return b;

    }
    else {
        $.jAlert('请至少选择一项！');

        return false;
    }
}

//全选反选取消
function selectCheckBoxDelAll(objName, showType) {
    var delAllObj = document.getElementsByName(objName);
    if (showType == 1) {
        //全选
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = true;
        }
    }
    else if (showType == 2) {
        //反选
        for (var i = 0; i < delAllObj.length; i++) {
            if (delAllObj[i].checked) {
                delAllObj[i].checked = false;
            }
            else {
                delAllObj[i].checked = true;
            }
        }
    }
    else if (showType == 3) {
        //取消
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = false;
        }
    }
}

function selectCheckBoxDelAllCheck(obj, objName) {
    var delAllObj = document.getElementsByName(objName);
    if (obj.checked == true) {
        //全选
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = true;
            delAllObj[i].enabled = false;
        }
    }
    else {
        //全选
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = false;
        }
    }
}

//验证浮点数
function isFloat(val) {
    var re = /^[0-9]+.?[0-9]*$/;
    if (!re.test(val)) {
        return true;
    }
    else {
        return false;
    }
}

//页面调转
function redirect(url) {
    window.location.href = url;
}

//将时间格式化年-月-日形式
// function getDate(datetime){ 
//    datetime = datetime.toLocaleDateString();
//    datetime = datetime.replace(/年/,"-");
//    datetime = datetime.replace(/月/,"-");
//    datetime = datetime.replace(/日/,"");
//    return datetime;
//}

function getDate(datetime) {
    var year = datetime.getFullYear();
    var month = datetime.getMonth() + 1;
    var date = datetime.getDate();
    if (month < 10) {
        month = "0" + month;
    }
    if (date < 10) {
        date = "0" + date;
    }
    var time = year + "-" + month + "-" + date; //2009-06-12 17:18:05
    return time;
}

//判断日期格式是否合法
String.prototype.isDate = function () {
    var r = this.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
    if (r == null) return false;
    var d = new Date(r[1], r[3] - 1, r[4]);
    return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
};
//验证长日期(2007-06-05 10:57:10)
String.prototype.isDateTime = function () {
    var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
    var r = this.match(reg);
    if (r == null) return false;
    var d = new Date(r[1], r[3] - 1, r[4], r[5], r[6], r[7]);
    return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4] && d.getHours() == r[5] && d.getMinutes() == r[6] && d.getSeconds() == r[7]);
};

//计算字符串长度，汉字算2
function Len(str) {
    var i, sum;
    sum = 0;
    for (i = 0; i < str.length; i++) {
        if ((str.charCodeAt(i) >= 0) && (str.charCodeAt(i) <= 255))
            sum = sum + 1;
        else
            sum = sum + 2;
    }
    return sum;
}

//检查是否含有汉字
function checkChars(s) {
    if (/[^\x00-\xff]/g.test(s)) {
        return true; //含有汉字
    }
    else {
        return false; //全是字符
    }
}


//状态标签点击切换函数
function Show_TabADSMenu(tabadid_num, tabadnum) {
    for (var i = 0; i < 2; i++) { document.getElementById("tabadcontent_" + tabadid_num + i).style.display = "none"; }
    for (var i = 0; i < 2; i++) { document.getElementById("tabadmenu_" + tabadid_num + i).className = ""; }
    document.getElementById("tabadmenu_" + tabadid_num + tabadnum).className = "linknow";
    document.getElementById("tabadcontent_" + tabadid_num + tabadnum).style.display = "block";
}


//设置表格样式
function SetTableStyle(tableid) {
    //$('#'+tableid+' tr:even').addClass('color_hui');//设置列表行样式
    $('#' + tableid + ' tr').removeData('currentcolor');
    $('#' + tableid + ' tr').mouseover(function () {
        if (!($(this).data('currentcolor')))
            $(this).data('currentcolor', $(this).css('backgroundColor'));
        $(this).css('backgroundColor', '#e5edf1').css('fontWeight', '');
    }).mouseout(function () {
        $(this).css('backgroundColor', $(this).data('currentcolor')).css('fontWeight', '');
    });

}
//手机验证
//function isMobile(mobile) {
//    return (/^(?:13\d|15\d|18\d|19\d|14\d)-?\d{5}(\d{3}|\*{3})$/.test(mobile));
//}
////电话验证
//function isTel(tel) {
//    return (/^(([0\+]\d{2,3})?(0\d{2,3}))(\d{7,8})$/.test(tel));
//}
////电话或者手机验证
//function isTelOrMobile(s) {
//    return (isMobile(s) || isTel(s));
//}
//邮件验证
function isEmail(s) {
    return (/^(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)$/.test(s));
}
//营业执照验证
//目前只验证长度和是否都为数字
function isLicense(s) {
    return (!isNaN(s) && s.length == 13);
}

//验证是否为数字
function checkNum(obj) {
    var re = /^-?[1-9]+(\.\d+)?$|^-?0(\.\d+)?$|^-?[1-9]+[0-9]*(\.\d+)?$/;
    if (!re.test(obj)) {
        return false;
    }
    return true;
}


//验证是否为数字
function isNum(s) {
    var pattern = /^[0-9]*$/;
    if (pattern.test(s)) {
        return true;
    }
    return false;
}
//身份证验证
function checkIdcard(idcard) {
    idcard = idcard.replace('x', 'X');
    var area = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }
    var idcard, Y, JYM;
    var S, M;
    var idcard_array = new Array();
    idcard_array = idcard.split("");
    //地区检验 
    if (area[parseInt(idcard.substr(0, 2))] == null) return false;
    //身份号码位数及格式检验 
    switch (idcard.length) {
        case 15:
            if ((parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0 || ((parseInt(idcard.substr(6, 2)) + 1900) % 100 == 0 && (parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$/; //测试出生日期的合法性 
            }
            else {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$/; //测试出生日期的合法性 
            }
            if (ereg.test(idcard)) return true;
            else return false;
            break;
        case 18:
            //18位身份号码检测 
            //出生日期的合法性检查  
            //闰年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9])) 
            //平年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8])) 
            if (parseInt(idcard.substr(6, 4)) % 4 == 0 || (parseInt(idcard.substr(6, 4)) % 100 == 0 && parseInt(idcard.substr(6, 4)) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$/; //闰年出生日期的合法性正则表达式 
            } else {
                ereg = /^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$/; //平年出生日期的合法性正则表达式 
            }
            if (ereg.test(idcard)) {//测试出生日期的合法性 
                //计算校验位 
                S = (parseInt(idcard_array[0]) + parseInt(idcard_array[10])) * 7
    + (parseInt(idcard_array[1]) + parseInt(idcard_array[11])) * 9
    + (parseInt(idcard_array[2]) + parseInt(idcard_array[12])) * 10
    + (parseInt(idcard_array[3]) + parseInt(idcard_array[13])) * 5
    + (parseInt(idcard_array[4]) + parseInt(idcard_array[14])) * 8
    + (parseInt(idcard_array[5]) + parseInt(idcard_array[15])) * 4
    + (parseInt(idcard_array[6]) + parseInt(idcard_array[16])) * 2
    + parseInt(idcard_array[7]) * 1
    + parseInt(idcard_array[8]) * 6
    + parseInt(idcard_array[9]) * 3;
                Y = S % 11;
                M = "F";
                JYM = "10X98765432";
                M = JYM.substr(Y, 1); //判断校验位 
                if (M == idcard_array[17]) return true; //检测ID的校验位 
                else return false;
            }
            else return false;
            break;
        default:
            return false;
            break;
    }
}
//兼容性的日历控件
function L_calendar() { }

L_calendar.prototype = {
    _VersionInfo: "Version:1.0&#13",
    Moveable: true,
    NewName: "",
    insertId: "",
    ClickObject: null,
    InputObject: null,
    InputDate: null,
    IsOpen: false,
    MouseX: 0,
    MouseY: 0,
    GetDateLayer: function () {
        if (window.parent) {
            return window.parent.L_DateLayer;
        } else {
            return window.L_DateLayer;
        }
    },
    L_TheYear: new Date().getFullYear(), //定义年的变量的初始值
    L_TheMonth: new Date().getMonth() + 1, //定义月的变量的初始值
    L_WDay: new Array(39), //定义写日期的数组
    MonHead: new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31),    		   //定义阳历中每个月的最大天数
    GetY: function () {
        var obj;
        if (arguments.length > 0) {
            obj == arguments[0];
        } else {
            obj = this.ClickObject;
        }
        if (obj != null) {
            var y = obj.offsetTop;
            while (obj = obj.offsetParent) y += obj.offsetTop;
            return y;
        } else {
            return 0;
        }
    },
    GetX: function () {
        var obj;
        if (arguments.length > 0) {
            obj == arguments[0];

        } else {
            obj = this.ClickObject;
        }
        if (obj != null) {
            var y = obj.offsetLeft;
            while (obj = obj.offsetParent) y += obj.offsetLeft;
            return y;
        } else {
            return 0;
        }
    },
    CreateHTML: function () {
        var htmlstr = "";
        htmlstr += "<div id=\"L_calendar\">\r\n";
        htmlstr += "<span id=\"SelectYearLayer\" style=\"z-index: 100100;position: absolute;top: 3; left: 19;display: none\"></span>\r\n";
        htmlstr += "<span id=\"SelectMonthLayer\" style=\"z-index: 100100;position: absolute;top: 3; left: 78;display: none\"></span>\r\n";
        htmlstr += "<div id=\"L_calendar-year-month\"><div id=\"L_calendar-PrevM\" onclick=\"parent." + this.NewName + ".PrevM()\" title=\"前一月\"><b>&lt;</b></div><div id=\"L_calendar-year\" onclick=\"parent." + this.NewName + ".SelectYearInnerHTML('" + this.L_TheYear + "')\"></div><div id=\"L_calendar-month\"   onclick=\"parent." + this.NewName + ".SelectMonthInnerHTML('" + this.L_TheMonth + "')\"></div><div id=\"L_calendar-NextM\" onclick=\"parent." + this.NewName + ".NextM()\" title=\"后一月\"><b>&gt;</b></div></div>\r\n";
        htmlstr += "<div id=\"L_calendar-week\"><ul  onmouseup=\"StopMove()\"><li>日</li><li>一</li><li>二</li><li>三</li><li>四</li><li>五</li><li>六</li></ul></div>\r\n";
        htmlstr += "<div id=\"L_calendar-day\">\r\n";
        htmlstr += "<ul>\r\n";
        for (var i = 0; i < this.L_WDay.length; i++) {
            htmlstr += "<li id=\"L_calendar-day_" + i + "\" style=\"background:#EFEFEF\" onmouseover=\"this.style.background='#ffffff'\"  onmouseout=\"this.style.background='#e0e0e0'\"></li>\r\n";
        }
        htmlstr += "</ul>\r\n";
        //htmlstr+="<span id=\"L_calendar-today\" onclick=\"parent."+this.NewName+".Today()\"><b>Today</b></span>\r\n";
        htmlstr += "</div>\r\n";
        //htmlstr+="<div id=\"L_calendar-control\"></div>\r\n";
        htmlstr += "</div>\r\n";
        htmlstr += "<scr" + "ipt type=\"text/javas" + "cript\">\r\n";
        htmlstr += "var MouseX,MouseY;";
        htmlstr += "var Moveable=" + this.Moveable + ";\r\n";
        htmlstr += "var MoveaStart=false;\r\n";
        htmlstr += "document.onmousemove=function(e)\r\n";
        htmlstr += "{\r\n";
        htmlstr += "var DateLayer=parent.document.getElementById(\"L_DateLayer\");\r\n";
        htmlstr += "	e = window.event || e;\r\n";
        htmlstr += "var DateLayerLeft=DateLayer.style.posLeft || parseInt(DateLayer.style.left.replace(\"px\",\"\"));\r\n";
        htmlstr += "var DateLayerTop=DateLayer.style.posTop || parseInt(DateLayer.style.top.replace(\"px\",\"\"));\r\n";
        htmlstr += "if(MoveaStart){DateLayer.style.left=(DateLayerLeft+e.clientX-MouseX)+\"px\";DateLayer.style.top=(DateLayerTop+e.clientY-MouseY)+\"px\"}\r\n";
        htmlstr += ";\r\n";
        htmlstr += "}\r\n";

        htmlstr += "document.getElementById(\"L_calendar-week\").onmousedown=function(e){\r\n";
        htmlstr += "if(Moveable){MoveaStart=true;}\r\n";
        htmlstr += "	e = window.event || e;\r\n";
        htmlstr += "  MouseX = e.clientX;\r\n";
        htmlstr += "  MouseY = e.clientY;\r\n";
        htmlstr += "	}\r\n";

        htmlstr += "function StopMove(){\r\n";
        htmlstr += "MoveaStart=false;\r\n";
        htmlstr += "	}\r\n";
        htmlstr += "</scr" + "ipt>\r\n";
        var stylestr = "";
        stylestr += "<style type=\"text/css\">";
        stylestr += "body{background:transparent;font-size:12px;margin:0px;padding:0px;text-align:left;font-family:宋体,Arial, Helvetica, sans-serif;line-height:1.6em;color:#666666;}\r\n";
        stylestr += "#L_calendar{background:#fff;border:1px solid #7AB8DF;width:158px;padding:1px;height:180px;z-index:100099;text-align:center}\r\n";
        stylestr += "#L_calendar-year-month{height:23px;line-height:23px;z-index:100099;background-color:#2670C1;color:#fff}\r\n";
        stylestr += "#L_calendar-year{line-height:23px;width:60px;float:left;z-index:100099;position: absolute;top: 3; left: 19;cursor:default}\r\n";
        stylestr += "#L_calendar-month{line-height:23px;width:48px;float:left;z-index:100099;position: absolute;top: 3; left: 78;cursor:default}\r\n";
        stylestr += "#L_calendar-PrevM{position: absolute;top: 3; left: 5;cursor:pointer}"
        stylestr += "#L_calendar-NextM{position: absolute;top: 3; left: 145;cursor:pointer}"
        stylestr += "#L_calendar-week{height:23px;line-height:23px;z-index:100099;}\r\n";
        stylestr += "#L_calendar-day{height:136px;z-index:100099;}\r\n";
        stylestr += "#L_calendar-week ul{cursor:move;list-style:none;margin:0px;padding:0px;}\r\n";
        stylestr += "#L_calendar-week li{width:20px;height:20px;float:left;;margin:1px;padding:0px;text-align:center;}\r\n";
        stylestr += "#L_calendar-day ul{list-style:none;margin:0px;padding:0px;}\r\n";
        stylestr += "#L_calendar-day li{cursor:pointer;width:20px;height:20px;float:left;;margin:1px;padding:0px;}\r\n";
        stylestr += "#L_calendar-control{height:25px;z-index:100099;}\r\n";
        stylestr += "#L_calendar-today{cursor:pointer;float:left;width:63px;height:20px;line-height:20px;margin:1px;text-align:center;background:#3B80CB}"
        stylestr += "</style>";
        var TempLateContent = "<html>\r\n";
        TempLateContent += "<head>\r\n";
        TempLateContent += "<title></title>\r\n";
        TempLateContent += stylestr;
        TempLateContent += "</head>\r\n";
        TempLateContent += "<body>\r\n";
        TempLateContent += htmlstr;
        TempLateContent += "</body>\r\n";
        TempLateContent += "</html>\r\n";
        this.GetDateLayer().document.writeln(TempLateContent);
        this.GetDateLayer().document.close();
    },
    InsertHTML: function (id, htmlstr) {
        var L_DateLayer = this.GetDateLayer();
        if (L_DateLayer) {
            L_DateLayer.document.getElementById(id).innerHTML = htmlstr;
        }
    },
    WriteHead: function (yy, mm)  //往 head 中写入当前的年与月
    {
        this.InsertHTML("L_calendar-year", yy + " 年");
        this.InsertHTML("L_calendar-month", mm + " 月");
    },
    IsPinYear: function (year)            //判断是否闰平年
    {
        if (0 == year % 4 && ((year % 100 != 0) || (year % 400 == 0))) return true;
        else return false;
    },
    GetMonthCount: function (year, month)  //闰年二月为29天
    {
        var c = this.MonHead[month - 1];
        if ((month == 2) && this.IsPinYear(year)) c++;
        return c;
    },
    GetDOW: function (day, month, year)     //求某天的星期几
    {
        var dt = new Date(year, month - 1, day).getDay() / 7;
        return dt;
    },
    GetText: function (obj) {
        if (obj.innerText) {
            return obj.innerText;
        } else {
            return obj.textContent;
        }
    },
    PrevM: function ()  //往前翻月份
    {
        if (this.L_TheMonth > 1) {
            this.L_TheMonth--;
        } else {
            this.L_TheYear--;
            this.L_TheMonth = 12;
        }
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    NextM: function ()  //往后翻月份
    {
        if (this.L_TheMonth == 12) {
            this.L_TheYear++;
            this.L_TheMonth = 1
        } else {
            this.L_TheMonth++
        }
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    Today: function ()  //Today Button
    {
        var today;
        this.L_TheYear = new Date().getFullYear();
        this.L_TheMonth = new Date().getMonth() + 1;
        today = new Date().getDate();
        if (this.InputObject) {
            this.InputObject.value = this.L_TheYear + "-" + this.L_TheMonth + "-" + today;
        }
        this.CloseLayer();
    },
    SetDay: function (yy, mm)   //主要的写程序**********
    {
        this.WriteHead(yy, mm);
        //设置当前年月的公共变量为传入值
        this.L_TheYear = yy;
        this.L_TheMonth = mm;
        for (var i = 0; i < 39; i++) {
            this.L_WDay[i] = "";
        }
        ; //将显示框的内容全部清空
        var day1 = 1, day2 = 1, firstday = new Date(yy, mm - 1, 1).getDay(); //某月第一天的星期几

        for (i = 0; i < firstday; i++) this.L_WDay[i] = this.GetMonthCount(mm == 1 ? yy - 1 : yy, mm == 1 ? 12 : mm - 1) - firstday + i + 1 //上个月的最后几天
        for (i = firstday; day1 < this.GetMonthCount(yy, mm) + 1; i++) {
            this.L_WDay[i] = day1;
            day1++;
        }
        for (i = firstday + this.GetMonthCount(yy, mm); i < 39; i++) {
            this.L_WDay[i] = day2;
            day2++;
        }
        for (i = 0; i < 39; i++) {
            var da = this.GetDateLayer().document.getElementById("L_calendar-day_" + i + "");
            var month, day;
            if (this.L_WDay[i] != "") {
                if (i < firstday) {
                    //da.innerHTML="<b style=\"color:gray\">" + this.L_WDay[i] + "</b>";
                    da.innerHTML = "";
                    //month=(mm==1?12:mm-1);
                    //day=this.L_WDay[i];
                    if (document.all) {
                        da.onclick = null;
                    } else {
                        da.setAttribute("onclick", "null");
                    }
                } else if (i >= firstday + this.GetMonthCount(yy, mm)) {
                    //da.innerHTML="<b style=\"color:gray\">" + this.L_WDay[i] + "</b>";
                    da.innerHTML = "";
                    //month=(mm==1?12:mm+1);
                    //day=this.L_WDay[i];
                    if (document.all) {
                        da.onclick = null;
                    } else {
                        da.setAttribute("onclick", "null");
                    }
                } else {
                    da.innerHTML = "<b style=\"color:#000\">" + this.L_WDay[i] + "</b>";
                    //month=(mm==1?12:mm);
                    month = mm;
                    day = this.L_WDay[i];
                    if (document.all) {
                        da.onclick = Function("parent." + this.NewName + ".DayClick(" + month + "," + day + ")");
                    } else {
                        da.setAttribute("onclick", "parent." + this.NewName + ".DayClick(" + month + "," + day + ")");
                    }
                    da.title = month + "月" + day + "日";
                    da.style.background = (yy == new Date().getFullYear() && month == new Date().getMonth() + 1 && day == new Date().getDate()) ? "#3B80CB" : "#EFEFEF";
                    if (this.InputDate != null) {
                        if (yy == this.InputDate.getFullYear() && month == this.InputDate.getMonth() + 1 && day == this.InputDate.getDate()) {
                            da.style.background = "#FF0000";
                        }
                    }
                }


            }
        }
    },
    SelectYearInnerHTML: function (strYear) //年份的下拉框
    {
        if (strYear.match(/\D/) != null) {
            $.jAlert("年份输入参数不是数字！");
            return;
        }
        var m = (strYear) ? strYear : new Date().getFullYear();
        if (m < 1000 || m > 9999) {
            $.jAlert("年份值不在 1000 到 9999 之间！");
            return;
        }
        var n = m - 50;
        if (n < 1000) n = 1000;
        if (n + 56 > 9999) n = 9974;
        var s = "<select name=\"L_SelectYear\" id=\"L_SelectYear\" style='font-size: 12px' "
        s += "onblur='document.getElementById(\"SelectYearLayer\").style.display=\"none\"' "
        s += "onchange='document.getElementById(\"SelectYearLayer\").style.display=\"none\";"
        s += "parent." + this.NewName + ".L_TheYear = this.value; parent." + this.NewName + ".SetDay(parent." + this.NewName + ".L_TheYear,parent." + this.NewName + ".L_TheMonth)'>\r\n";
        var selectInnerHTML = s;
        for (var i = n; i < n + 56; i++) {
            if (i == m) {
                selectInnerHTML += "<option value='" + i + "' selected>" + i + "年" + "</option>\r\n";
            } else {
                selectInnerHTML += "<option value='" + i + "'>" + i + "年" + "</option>\r\n";
            }
        }
        selectInnerHTML += "</select>";
        var DateLayer = this.GetDateLayer();
        DateLayer.document.getElementById("SelectYearLayer").style.display = "";
        DateLayer.document.getElementById("SelectYearLayer").innerHTML = selectInnerHTML;
        DateLayer.document.getElementById("L_SelectYear").focus();
    },
    SelectMonthInnerHTML: function (strMonth) //月份的下拉框
    {
        if (strMonth.match(/\D/) != null) {
            $.jAlert("月份输入参数不是数字！");
            return;
        }
        var m = (strMonth) ? strMonth : new Date().getMonth() + 1;
        var s = "<select name=\"L_SelectYear\" id=\"L_SelectMonth\" style='font-size: 12px' "
        s += "onblur='document.getElementById(\"SelectMonthLayer\").style.display=\"none\"' "
        s += "onchange='document.getElementById(\"SelectMonthLayer\").style.display=\"none\";"
        s += "parent." + this.NewName + ".L_TheMonth = this.value; parent." + this.NewName + ".SetDay(parent." + this.NewName + ".L_TheYear,parent." + this.NewName + ".L_TheMonth)'>\r\n";
        var selectInnerHTML = s;
        for (var i = 1; i < 13; i++) {
            if (i == m) {
                selectInnerHTML += "<option value='" + i + "' selected>" + i + "月" + "</option>\r\n";
            } else {
                selectInnerHTML += "<option value='" + i + "'>" + i + "月" + "</option>\r\n";
            }
        }
        selectInnerHTML += "</select>";
        var DateLayer = this.GetDateLayer();
        DateLayer.document.getElementById("SelectMonthLayer").style.display = "";
        DateLayer.document.getElementById("SelectMonthLayer").innerHTML = selectInnerHTML;
        DateLayer.document.getElementById("L_SelectMonth").focus();
    },
    DayClick: function (mm, dd)  //点击显示框选取日期，主输入函数*************
    {
        var yy = this.L_TheYear;
        //判断月份，并进行对应的处理
        if (mm < 1) {
            yy--;
            mm = 12 + mm;
        } else if (mm > 12) {
            yy++;
            mm = mm - 12;
        }
        if (mm < 10) {
            mm = "0" + mm;
        }
        if (this.ClickObject) {
            if (!dd) {
                return;
            }
            if (dd < 10) {
                dd = "0" + dd;
            }
            this.InputObject.value = yy + "-" + mm + "-" + dd; //注：在这里你可以输出改成你想要的格式
            this.CloseLayer();
        } else {
            this.CloseLayer();
            alert("您所要输出的控件对象并不存在！");
        }
    },
    SetDate: function () {
        if (arguments.length < 1) {
            alert("对不起！传入参数太少！");
            return;
        } else if (arguments.length > 3) {
            alert("对不起！传入参数太多！");
            return;
        }
        this.InputObject = (arguments.length == 1) ? arguments[0] : arguments[1];
        this.ClickObject = arguments[0];
        if (typeof (arguments[arguments.length - 1]) == 'function') { //如果最后一个参数是函数的话，为关闭时的响应方法
            this.OnClose = arguments[arguments.length - 1];
        }
        var reg = /^(\d+)-(\d{1,2})-(\d{1,2})$/;
        var r = this.InputObject.value.match(reg);
        if (r != null) {
            r[2] = r[2] - 1;
            var d = new Date(r[1], r[2], r[3]);
            if (d.getFullYear() == r[1] && d.getMonth() == r[2] && d.getDate() == r[3]) {
                this.InputDate = d; //保存外部传入的日期
            } else this.InputDate = "";
            this.L_TheYear = r[1];
            this.L_TheMonth = r[2] + 1;
        } else {
            this.L_TheYear = new Date().getFullYear();
            this.L_TheMonth = new Date().getMonth() + 1;
        }
        this.CreateHTML();
        var top = this.GetY();
        var left = this.GetX();
        var DateLayer = document.getElementById("L_DateLayer");

        //判断如果浏览器是ie 7.0，且是查询列表中的时间控件 则将离左边宽度-240px;高度加上3px
        if ($.browser.msie) {
            if ($.browser.version == "7.0" && $("#sidebar").length > 0) {
                left = left - 240;
                top = top + 3;
            }
        }

        DateLayer.style.top = top + this.ClickObject.clientHeight + 5 + "px";
        DateLayer.style.left = left + "px";

        DateLayer.style.display = "block";
        if (document.all) {
            this.GetDateLayer().document.getElementById("L_calendar").style.width = "160px";
            this.GetDateLayer().document.getElementById("L_calendar").style.height = "180px";
        } else {
            this.GetDateLayer().document.getElementById("L_calendar").style.width = "154px";
            this.GetDateLayer().document.getElementById("L_calendar").style.height = "180px";
            DateLayer.style.width = "158px";
            DateLayer.style.height = "250px";
        }
        //alert(DateLayer.style.display)
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    CloseLayer: function () {
        try {
            var DateLayer = document.getElementById("L_DateLayer");
            if ((DateLayer.style.display == "" || DateLayer.style.display == "block") && arguments[0] != this.ClickObject && arguments[0] != this.InputObject) {
                DateLayer.style.display = "none";
                if (this.OnClose) {
                    this.OnClose(this.InputObject);
                }
            }
        } catch (e) {
        }
    }
};

document.writeln('<iframe id="L_DateLayer" name="L_DateLayer" frameborder="0" style="position:absolute;width:160px; height:190px;overflow:hidden;z-index:100099;display:none;backgorund-color:transparent;"></iframe>');
var MyCalendar = new L_calendar();
MyCalendar.NewName = "MyCalendar";
document.onclick = function (e) {
    e = window.event || e;
    var srcElement = e.srcElement || e.target;
    MyCalendar.CloseLayer(srcElement);
};




//编辑数据
function toggles(obj_id) {
    var target = document.getElementById(obj_id);

    var bgObj = document.getElementById("bgDiv");
    bgObj.style.width = document.body.offsetWidth + "px";
    bgObj.style.height = screen.height + "px";

    var bgOifm = document.getElementById("iframe_top");
    bgOifm.style.width = document.body.offsetWidth + "px";
    bgOifm.style.height = screen.height + "px";

    if (target.style.display == "none") {
        target.style.display = "block";
        target.style.top = (document.documentElement.scrollTop + document.documentElement.clientHeight / 2);
        bgObj.style.display = "block";
        bgOifm.style.display = "block";
    }
    else {
        target.style.display = "none";
        bgObj.style.display = "none";
        bgOifm.style.display = "none";
    }
}

/*
* 异步调用--公用方法,需要引用JQuery
* beforeSend没有内容，可以传入null
* Add=Masj,Date=20091207
*/
function AjaxPost(url, postBody, beforeSend, CallbackName) {
    $.ajax({
        type: "POST",
        url: url,
        data: postBody,
        beforeSend: beforeSend,
        success: CallbackName,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中
            // 只有一个会包含信息
            alert(XMLHttpRequest.responseText);
        }
    });
}

/*
* 同步调用--公用方法,需要引用JQuery
* beforeSend没有内容，可以传入null
* Add=chybin,Date=20120801
*/
function AjaxPostAsync(url, postBody, beforeSend, CallbackName) {
    $.ajax({
        type: "POST",
        url: url,
        data: postBody,
        async: false,
        beforeSend: beforeSend,
        success: CallbackName,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中
            // 只有一个会包含信息
            //alert(XMLHttpRequest.responseText);
        }
    });
}


/*
* 异步调用--公用方法,需要引用JQuery
* beforeSend没有内容，可以传入null
* Add=Masj,Date=20091215
*/
function AjaxGet(url, postBody, beforeSend, CallbackName) {
    $.ajax({
        type: "GET",
        url: url,
        data: postBody,
        beforeSend: beforeSend,
        success: CallbackName,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中
            // 只有一个会包含信息
            alert(XMLHttpRequest.responseText);
        }
    });
}


/*
* 绑定省份，需要引用Area.js文件
* Area.js文件是生成的
* Add=Masj, Date: 2009-12-07 
*/
function BindProvince(SelectID) {
    if (JSonData && JSonData.masterArea.length > 0) {
        var masterObj = document.getElementById(SelectID);
        if (masterObj && masterObj.options) {
            masterObj.options.length = 0;
            masterObj.options[0] = new Option("省/直辖市", -1);
            for (var i = 0; i < JSonData.masterArea.length; i++) {
                masterObj.options[masterObj.options.length] = new Option(JSonData.masterArea[i].name, JSonData.masterArea[i].id);
            }
        }
    }
}

/*
* 绑定城市，需要引用Area.js文件
* Area.js文件是生成的
* 参数provinceSelectID为省份ID，citySelectID为城市ID
* Add=Masj, Date: 2009-12-07 
*/
function BindCity(provinceSelectID, citySelectID) {
    var temp = document.getElementById(provinceSelectID); if (!temp) { return; }
    temp = temp.options[document.getElementById(provinceSelectID).selectedIndex]; if (!temp) { return; }
    var masterObjid = temp.value;
    if (masterObjid && masterObjid > 0) {
        var subAreaObj = document.getElementById(citySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("城市", -1);
        for (var i = 0; i < JSonData.masterArea.length; i++) {
            if (JSonData.masterArea[i].id == masterObjid) {
                for (var j = 0; j < JSonData.masterArea[i].subArea.length; j++) {
                    subAreaObj.options[subAreaObj.options.length] = new Option(JSonData.masterArea[i].subArea[j].name, JSonData.masterArea[i].subArea[j].id);
                }
            }
        }
    }
    else if (masterObjid && masterObjid == -1) {
        var subAreaObj = document.getElementById(citySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("城市", -1);
    }
}
//根据城市id，取省份id
function GetProvinceIDByCityID(cityvalue) {
    var provinceid = 0;
    for (var i = 0; i < JSonData.masterArea.length; i++) {
        for (var j = 0; j < JSonData.masterArea[i].subArea.length; j++) {
            if (JSonData.masterArea[i].subArea[j].id == cityvalue) {
                provinceid = JSonData.masterArea[i].id;
                break;
            }
        }
        if (provinceid != 0) {
            break;
        }
    }
    return provinceid;
}

/*
* 绑定区县，需要引用Area2.js文件
* Area2.js文件是生成的
* 参数provinceSelectID为省份ID，citySelectID为城市ID, countyID为区县ID
*/
function BindCounty(provinceSelectID, citySelectID, countySelectID) {
    var temp = document.getElementById(provinceSelectID); if (!temp) { return; }
    temp = temp.options[document.getElementById(provinceSelectID).selectedIndex]; if (!temp) { return; }
    var provinceId = temp.value;

    var temp = document.getElementById(citySelectID); if (!temp) { return; }
    temp = temp.options[document.getElementById(citySelectID).selectedIndex]; if (!temp) { return; }
    var cityId = temp.value;
    if (provinceId && provinceId > 0 && cityId && cityId > 0) {
        var subAreaObj = document.getElementById(countySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("区/县", -1);
        for (var i = 0; i < JSonData.masterArea.length; i++) {
            if (JSonData.masterArea[i].id == provinceId) {
                var t1 = JSonData.masterArea[i];
                for (var j = 0; j < t1.subArea.length; j++) {
                    if (t1.subArea[j].id == cityId) {
                        var t2 = t1.subArea[j];
                        for (var k = 0; k < t2.subArea2.length; k++) {
                            subAreaObj.options[subAreaObj.options.length] =
                                new Option(t2.subArea2[k].name, t2.subArea2[k].id);
                        }
                        return;
                    }
                }
            }
        }
    }
    else if ((provinceId && provinceId == -1) || (cityId && cityId == -1)) {
        var subAreaObj = document.getElementById(countySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("区/县", -1);
    }
}


/*
* 绑定枚举列表，需要引用ShowEnum.js文件和类库JQuery
* ShowEnum.js文件是生成的
* 参数arrayObject为数组对象
* Add=Masj, Date: 2009-12-07 
*/
function BindArrayToSelect(arrayObject, selectID, str) {
    var selectObject = document.getElementById(selectID);
    selectObject.options.length = 0;
    if (str) {
        selectObject.options[0] = new Option(str, -1);
    }
    else {
        selectObject.options[0] = new Option("请选择", -1);
    }
    $.each(arrayObject, function (name, value) {
        selectObject.options[selectObject.options.length] = new Option(value[0], value[1]);
    });
}

/**
* @desc   escape字符串,escape不编码字符有69个：*，-，.，@，_，0-9，a-z，A-Z
* @param  字符串
* @return 返回string等对象
* @Add=Masj, Date: 2009-12-16
*/
function escapeStr(str) {
    return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
}

/**
* @desc  JQuery扩展，将json字符串转换为对象，需要引用类库JQuery
* @param   json字符串
* @return 返回object,array,string等对象
* @Add=Masj, Date: 2009-12-07
*/
jQuery.extend(
 {
     evalJSON: function (strJson) {
         if ($.trim(strJson) == '')
             return '';
         else
             return eval("(" + strJson + ")");
     }
 });
/**
* @desc  JQuery扩展，将javascript数据类型转换为json字符串，需要引用类库JQuery
* @param 待转换对象,支持object,array,string,function,number,boolean,regexp
* @return 返回json字符串
* @Add=Masj, Date: 2009-12-07
*/
jQuery.extend(
{
    toJSONstring: function (object) {
        var type = typeof object;
        if ('object' == type) {
            if (Array == object.constructor)
                type = 'array';
            else if (RegExp == object.constructor)
                type = 'regexp';
            else
                type = 'object';
        }
        switch (type) {
            case 'undefined':
            case 'unknown':
                return;
                break;
            case 'function':
            case 'boolean':
            case 'regexp':
                return object.toString();
                break;
            case 'number':
                return isFinite(object) ? object.toString() : 'null';
                break;
            case 'string':
                return '"' + object.replace(/(\\|\")/g, "\\$1").replace(/\n|\r|\t/g,
                      function () {
                          var a = arguments[0];
                          return (a == '\n') ? '\\n' :
                              (a == '\r') ? '\\r' :
                                  (a == '\t') ? '\\t' : "";
                      }) + '"';
                break;
            case 'object':
                if (object === null) return 'null';
                var results = [];
                for (var property in object) {
                    var value = jQuery.toJSONstring(object[property]);
                    if (typeof (value) != "undefined")
                        results.push(jQuery.toJSONstring(property) + ':' + value);
                }
                return '{' + results.join(',') + '}';
                break;
            case 'array':
                var results = [];
                for (var i = 0; i < object.length; i++) {
                    var value = jQuery.toJSONstring(object[i]);
                    if (typeof (value) != "undefined") results.push(value);
                }
                return '[' + results.join(',') + ']';
                break;
        }
    }
});

//获得客户浏览器类型
function GetBrowserName() {
    var Sys = {};
    var ua = navigator.userAgent.toLowerCase();
    var s;
    (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
        (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
        (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
        (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
        (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;

    //以下进行测试
    if (Sys.ie) {
        //alert('IE: ' + Sys.ie);
        return 'IE';
    }
    else if (Sys.firefox) {
        //alert('Firefox: ' + Sys.firefox);
        return 'FF';
    }
    else
        return '';
    //        if (Sys.chrome) document.write('Chrome: ' + Sys.chrome);
    //        if (Sys.opera) document.write('Opera: ' + Sys.opera);
    //        if (Sys.safari) document.write('Safari: ' + Sys.safari);
}

//重置
function resetForm(id) {
    jQuery('#' + id).each(function () {
        this.reset();
    });
}

//关闭弹出层
/**
@name 弹出层的名字
@, isCancel 是否是取消之类的操作，默认为true
*/
function Close(name, effectiveAction) {
    $.closePopupLayer(name, effectiveAction);
}

/**
* @desc  返回字符串长度
* @param 字符串
* @return 返回字符串长度
* @Add=Masj, Date: 2009-12-11
*/
function GetStringRealLength(str) {
    var bytesCount = 0;
    for (var i = 0; i < str.length; i++) {
        var c = str.charAt(i);
        if (/^[\u0000-\u00ff]$/.test(c))   //匹配双字节
        {
            bytesCount += 1;
        }
        else {
            bytesCount += 2;
        }
    }
    return bytesCount;
}

/*头部鼠标划过出现下拉层star*/
//function addLoadEvent(func) {
//    var oldonload = window.onload;
//    if (typeof window.onload != 'function') {
//        window.onload = func;
//    } else {
//        window.onload = function () {
//            oldonload();
//            func();
//        }
//    }
//}

function addClass(element, value) {
    if (!element.className) {
        element.className = value;
    } else {
        newClassName = element.className;
        newClassName += " ";
        newClassName += value;
        element.className = newClassName;
    }
}

function removeClass(element, value) {
    var removedClass = element.className;
    var pattern = new RegExp("(^| )" + value + "( |$)");
    removedClass = removedClass.replace(pattern, "$1");
    removedClass = removedClass.replace(/ $/, "");
    element.className = removedClass;
    return true;
}

function bt_login_more(overID, boxID, add_Class) {
    if (!document.getElementById(overID)) return false;
    var btli = document.getElementById(overID);
    var btpop = document.getElementById(boxID);
    btli.onmouseover = function () {
        addClass(btpop, add_Class);
    };
    btli.onmouseout = function () {
        removeClass(btpop, add_Class);
    };
}
function all_login_box() {
    bt_login_more('goOther', 'goOtherContent', 'pop_block');
}
//addLoadEvent(all_login_box);
/*头部鼠标划过出现下拉层end*/





/*节选自jQueryString v2.0.2*/
(function ($) {
    $.unserialise = function (Data) {
        var Data = Data.split("&");
        var Serialised = new Array();
        $.each(Data, function () {
            var Properties = this.split("=");
            Serialised[Properties[0]] = Properties[1];
        });
        return Serialised;
    };
})(jQuery);


/*设置透明度，兼容IE和FF*/
; (function ($) {
    $.freeOpacity = {
        main: function (opacity) {
            this.each(function (i) {
                var _this = $(this);
                if ($.browser.msie) { _this.css('filter', 'alpha(opacity=' + opacity * 100 + ')'); }
                else { _this.css('opacity', opacity); this.style.Opacity = 0.5; }
            });
            return this;
        }
    }; $.fn.opacity = $.freeOpacity.main;
})(jQuery);

/*
* jqDnR - Minimalistic Drag'n'Resize for jQuery.
*
* Copyright (c) 2007 Brice Burgess <bhb@iceburg.net>, http://www.iceburg.net
* Licensed under the MIT License:
* http://www.opensource.org/licenses/mit-license.php
* 
* $Version: 2007.08.19 +r2
* Drag and Resize, 我觉得比jQuery.ui中写的还要好...
* 我将opacity注释掉，因为IE中会有BUG；添加了setCapture，解决IE中在浏览器外不响应mouseup事件的问题。
*/
(function ($) {
    $.fn.jqDrag = function (h) { return i(this, h, 'd'); };
    $.fn.jqResize = function (h) { return i(this, h, 'r'); };
    $.jqDnR = { dnr: {}, e: 0,
        drag: function (v) {
            if (M.k == 'd') E.css({ left: M.X + v.pageX - M.pX, top: M.Y + v.pageY - M.pY });
            else E.css({ width: Math.max(v.pageX - M.pX + M.W, 0), height: Math.max(v.pageY - M.pY + M.H, 0) });
            return false;
        },
        stop: function (h) {/*E.opacity(M.o);*/
            if (h[0].releaseCapture) { h[0].releaseCapture(); } //取消捕获范围
            else if (window.captureEvents) { window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP); }
            $(document).unbind('mousemove', J.drag).unbind('mouseup', J.stop);
        }
    };
    var J = $.jqDnR, M = J.dnr, E = J.e,
i = function (e, h, k) {
    return e.each(function () {
        h = (h) ? $(h, e) : e;
        h.bind('mouseover', function () { $(this).css('cursor', 'move'); })
 .bind('mouseout', function () { $(this).css('cursor', 'auto'); });
        h.bind('mousedown', { e: e, k: k }, function (v) {
            var d = v.data, p = {}; E = d.e;
            // attempt utilization of dimensions plugin to fix IE issues
            if (E.css('position') != 'relative') { try { E.position(p); } catch (e) { } }
            M = { X: p.left || f('left') || 0, Y: p.top || f('top') || 0, W: f('width') || E[0].scrollWidth || 0, H: f('height') || E[0].scrollHeight || 0, pX: v.pageX, pY: v.pageY, k: d.k, o: E.css('opacity') };
            /*E.opacity(0.8);*/
            //设置捕获范围
            if (h[0].setCapture) { h[0].setCapture(); }
            else if (window.captureEvents) { window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP); }
            $(document).mousemove($.jqDnR.drag).mouseup(function () {
                $.jqDnR.stop(h);
            });
            return false;
        });
    });
},
f = function (k) { return parseInt(E.css(k)) || false; };
})(jQuery);


//载入时的动画. eleId为容器ID
function LoadingAnimation(eleId) {
    jQuery('#' + eleId).html('<div style="width:100%; height:40px;padding-top:15px;"><div class="blue-loading" style="width:50%;float:left;background-position:right;"></div><div style="float:left;padding:20px 0px 0px 10px;">正在加载中...</div></div>');
}


function fullChar2halfChar(str) {
    var result = '';
    for (i = 0; i < str.length; i++) {
        code = str.charCodeAt(i);             //获取当前字符的unicode编码
        if (code >= 65281 && code <= 65373)   //unicode编码范围是所有的英文字母以及各种字符
        {
            result += String.fromCharCode(str.charCodeAt(i) - 65248);    //把全角字符的unicode编码转换为对应半角字符的unicode码
        }
        else if (code == 12288)                                      //空格
        {
            result += String.fromCharCode(str.charCodeAt(i) - 12288 + 32); //半角空格
        } else {
            result += str.charAt(i);                                     //原字符返回
        }
    }
    return result;
}

/*! Copyright (c) 2010 Brandon Aaron (http://brandonaaron.net)
* Licensed under the MIT License (LICENSE.txt).
* Version 2.1.3-pre
*/
(function ($) {
    $.fn.bgiframe = ($.browser.msie && /msie 6\.0/i.test(navigator.userAgent) ? function (s) {
        s = $.extend({
            top: 'auto', // auto == .currentStyle.borderTopWidth
            left: 'auto', // auto == .currentStyle.borderLeftWidth
            width: 'auto', // auto == offsetWidth
            height: 'auto', // auto == offsetHeight
            opacity: true,
            src: 'javascript:false;'
        }, s);
        var html = '<iframe class="bgiframe"frameborder="0"tabindex="-1"src="' + s.src + '"' +
                   'style="display:block;position:absolute;z-index:-1;' +
                       (s.opacity !== false ? 'filter:Alpha(Opacity=\'0\');' : '') +
                       'top:' + (s.top == 'auto' ? 'expression(((parseInt(this.parentNode.currentStyle.borderTopWidth)||0)*-1)+\'px\')' : prop(s.top)) + ';' +
                       'left:' + (s.left == 'auto' ? 'expression(((parseInt(this.parentNode.currentStyle.borderLeftWidth)||0)*-1)+\'px\')' : prop(s.left)) + ';' +
                       'width:' + (s.width == 'auto' ? 'expression(this.parentNode.offsetWidth+\'px\')' : prop(s.width)) + ';' +
                       'height:' + (s.height == 'auto' ? 'expression(this.parentNode.offsetHeight+\'px\')' : prop(s.height)) + ';' +
                '"/>';
        return this.each(function () {
            if ($(this).children('iframe.bgiframe').length === 0)
                this.insertBefore(document.createElement(html), this.firstChild);
        });
    } : function () { return this; });
    // old alias
    $.fn.bgIframe = $.fn.bgiframe;
    function prop(n) {
        return n && n.constructor === Number ? n + 'px' : n;
    }
})(jQuery);

(function ($) {
    $.fn.centerScreen = function (ratioW, ratioH) {
        if (!ratioW) {
            ratioW = 0.5;
        }
        if (!ratioH) {
            ratioH = 0.5;
        }
        var top = ($(window).height() - this.height()) * ratioH;
        var left = ($(window).width() - this.width()) * ratioW;
        var scrollTop = $(document).scrollTop();
        var scrollLeft = $(document).scrollLeft();
        return this.css({ position: 'absolute', 'top': top + scrollTop, left: left + scrollLeft }).show();
    };
})(jQuery);

//将一块区域覆盖
function CoverArea(ele, coverId, txt) {
    var source = jQuery(ele);
    if (source.size() <= 0) { return; }
    if (!coverId) {
        coverId = '_coverAreaID';
    }
    if (!txt) { txt = ''; }
    var z = source.css('z-index'); //"z-index"
    if (z == 'auto') { z = 90000; }
    var c = jQuery("<div/>").attr('id', coverId).appendTo(source).css({
        'z-index': z + 3,
        background: 'gray',
        position: 'absolute',
        left: source.offset().left,
        top: source.offset().top,
        height: source.height(),
        width: source.width()
    }).opacity(0.5);
    var textDiv = $('<div class="locking" style="">' + txt + '</div>');
    textDiv.centerScreen(0.2);
    var closeSpan = $('<span onclick="javascript:$(\'#' + coverId + '\').remove();">close</span>');
    c.append(textDiv).append(closeSpan).bgiframe();
    return c;
}

function OpenWaitting() {
    $.openPopupLayer({
        name: "WaittingPopup",
        parameters: {},
        url: "../AjaxServers/RequestWaittingPoper.aspx",
        beforeClose: function (e) {
        }
    });
}
function CloseWaitting() {
    $.closePopupLayer('WaittingPopup', false);
}
//日期增加函数
function dateAdd(strInterval, NumDay, dtDate) {
    var dtTmp = new Date(dtDate);
    if (isNaN(dtTmp)) dtTmp = new Date();
    switch (strInterval) {
        case "s": return new Date(Date.parse(dtTmp) + (1000 * NumDay));
        case "n": return new Date(Date.parse(dtTmp) + (60000 * NumDay));
        case "h": return new Date(Date.parse(dtTmp) + (3600000 * NumDay));
        case "d": return new Date(Date.parse(dtTmp) + (86400000 * NumDay));
        case "w": return new Date(Date.parse(dtTmp) + ((86400000 * 7) * NumDay));
        case "m": return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + NumDay, dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
        case "y": return new Date((dtTmp.getFullYear() + NumDay), dtTmp.getMonth(), dtTmp.getDate(), dtTmp.getHours(), dtTmp.getMinutes(), dtTmp.getSeconds());
    }
}


//写cookies函数
function SetCookie(name, value)//两个参数，一个是cookie的名子，一个是值
{
    var Days = 1;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    // document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ';domain=oa.bitauto.com;path=/;';
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ';domain=' + domainCookie + ';path=/;';
    //document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ';domain=;path=/;';
}
function GetCookie(name)//取cookies函数        
{
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) return unescape(arr[2]); return null;

}
function DelCookie(name)//删除cookie
{
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = GetCookie(name);
    if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString() + ";domain=" + domainCookie + ";path=/;";
    //if (cval != null) document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString() + ";domain=;path=/;";
}

String.prototype.rExp = function (a1, a2) {
    var reg = new RegExp(a1, "g");
    return this.replace(reg, a2);
};

String.prototype.returnRegExp = function () {
    var str = this;

    str = str.rExp("\\+", "%2B");
    str = str.rExp("%F7", escape("&divide;"));
    str = str.rExp("%B1", escape("&plusmn;"));
    str = str.rExp("%D7", escape("&times;"));
    str = str.rExp("%A9", escape("&copy;"));
    str = str.rExp("%AE", escape("&reg;"));
    str = str.rExp("%B7", escape("&middot;"));
    str = str.rExp("%A3", escape("&pound;"));
    str = str.rExp("%u2122", escape("&#8482;"));
    str = str.rExp("%u2022", escape("&bull;"));


    return str;
};
//得到radio的value值
function getRadioVal(name) {
    if ($("input[name='isMakeUp']:checked").length > 0) {
        return $("input[name='isMakeUp']:checked").val();
    }
    else {
        return "";
    }
}
//得到checkbox的value值
function getCheckBoxVal(name) {
    var val = "";
    var d = $(":checkbox[name='" + name + "'][checked=true]");
    if (d.length != 0) {
        for (var i = 0; i < d.length; i++) {
            val += d.eq(i).val() + ",";
        }
        val = val.substring(0, val.length - 1);
    }
    return val;
}

//点击文字，选中复选框、单选框
function emChkIsChoose(othis) {
    var $ChkRdo = $(othis).prev();
    //控件未被禁用时点击生效
    if (!$ChkRdo.is(":disabled")) {
        //判断 单选OR复选
        if ($ChkRdo.attr("type") == "checkbox") {
            if ($ChkRdo.is(":checked")) {
                $ChkRdo.removeAttr("checked");
            }
            else {
                $ChkRdo.attr("checked", "checked");
            }
        }
        else if ($ChkRdo.attr("type") == "radio") {
            if (!$ChkRdo.is(":checked")) {
                $ChkRdo.attr("checked", "checked");
            }
        }
    }
}

//把Json对象转换成 & 分割的串形式
function JsonObjToParStr(json) {
    var tmps = [];
    for (var key in json) {
        tmps.push(key + '=' + escape(json[key]));
    }
    return tmps.join('&');
}

///根据枚举绑定下拉列表
///id  下拉列表的ID
/// enumName 枚举名称
function BindByEnum(id, enumName) {
    AjaxPostAsync('/AjaxServers/Common/GetFromEnum.ashx', { Action: 'GetListByEnum', EnumName: enumName }, null, function (data) {
        var jsonData = $.evalJSON(data);
        $("[id$='" + id + "']").html("");
        $("[id$='" + id + "']").append("<option value='-1'>请选择</option>");
        $(jsonData.root).each(function (i, v) {
            $("[id$='" + id + "']").append("<option value=" + v.value + ">" + v.name + "</option>");
        });
    });
}

//页面敲回车键，执行的方法，funName-方法名
//注：如果列表页面有弹出层也需要使用该方法，则需要在列表页面调用完弹出层方法后的回调函数重新绑定该方法，否则列表页因为document的keydown事件被解除而不可用
enterSearch = function (funName) {
    $(document).unbind("keydown");
    $(document).keydown(function (event) {
        if (event.keyCode == 13) {
            funName();
        }
    });
};

//初始化时间
//type=1：只有一个时间输入框用以下脚本初始化；
//type=2:有两个时间输入框，前面的日期不能大于后面的日期
//type=3:有两个时间输入框，且需要精确到时分秒
//arryTimeID:时间控件ID数组。例：InitWdatePicker(2, ["tfBeginTime", "tfEndTime"]);
function InitWdatePicker(type, arryTimeID) {

    switch (type) {
        case 1: $('#' + arryTimeID[0]).bind('click focus', function () { WdatePicker(); });
            break;
        case 2: $('#' + arryTimeID[0]).bind('click focus', function () { WdatePicker({ maxDate: "#F{$dp.$D(" + arryTimeID[1] + ")}", onpicked: function () { document.getElementById(arryTimeID[1]).focus(); } }); });
            $('#' + arryTimeID[1]).bind('click focus', function () { WdatePicker({ minDate: "#F{$dp.$D(" + arryTimeID[0] + ")}" }); });
            break;
        case 3: $('#' + arryTimeID[0]).bind('click focus', function () { WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', startDate: "%y-%M-%d 00:00:00", maxDate: "#F{$dp.$D(" + arryTimeID[1] + ")}", onpicked: function () { document.getElementById(arryTimeID[1]).focus(); } }); });
            $('#' + arryTimeID[1]).bind('click focus', function () { WdatePicker({ dateFmt: 'yyyy-MM-dd HH:mm:ss', startDate: "%y-%M-%d 23:59:59", minDate: "#F{$dp.$D(" + arryTimeID[0] + ")}" }); });
            break;
    }
}


//搜索-智能提示
function getContactTips(id, telID, showWidth) {
    if ($.trim($("#" + telID).val()) == "") {
        return false;
    }
    $("#" + id).autocomplete("/AjaxServers/ContactTips.ashx", {
        minChars: 1,
        width: showWidth,
        scrollHeight: 300,
        autoFill: false,
        delay: 100,
        matchContains: "word",
        extraParams: { keyWord: function () { return $("#" + id).val(); }, Tel: function () { return $.trim($("#" + telID).val()) }, r: Math.random() },
        parse: function (data) {
            if (data != "") {
                return $.map(eval(data), function (row) {
                    return {
                        data: row,
                        value: row.CustID,    //此处无需把全部列列出来，只是两个关键列
                        result: data.CustName
                    };
                });
            }
        },
        formatItem: function (data, i, n, value) {
            return data.CustName;
        },
        formatResult: function (data, value) {
            return data.CustID;
        }
    });

    //选中时填写到输入框中
    $("#" + id).result(function (event, data, formatted) {
        if (data && data != "")
            $(this).val(data.CustName);
    });

}

//选择推荐活动弹出层 add lxw 14.1.14
function fnSelectActivityPop(opts, afterFn) {
    var popObj = { ids: "", values: "", msg: "" };
    if (typeof opts != "object") {
        popObj.msg = "传入的参数格式不正确！";
        return popObj;
    }
    var params = opts;
    //验证
    if (!params.pid || !params.cid) {
        popObj.msg = "省份城市传入参数有错误！";
        return popObj;
    }
    if (params.pid == "-1" && params.cid == "-1") {
        popObj.msg = "请选择省份或城市！";
        return popObj;
    }
    var brandID = "";
    if (!params.carid || !params.bid) {
        popObj.msg = "品牌车型传入参数有错误！";
        return popObj;
    }
    if ((params.bid == "0" || params.bid == "-1") && (params.carid == "0" || params.carid == "-1")) {
        popObj.msg = "请选择品牌车型！";
        return popObj;
    }
    if (params.carid != "0" && params.carid != "-1") {
        brandID = params.carid;
    }
    else {
        brandID = params.bid;
    }

    $.openPopupLayer({
        name: "selectActivityAjaxPopup",
        parameters: {},
        url: "/TemplateManagement/SelectActivity.aspx?ActivityIDs=" + params.selectids + "&ProvinceID=" + params.pid + "&CityID=" + params.cid + "&BrandID=" + brandID,
        beforeClose: function (b, cData) {
            if (b) {
                popObj.ids = cData.ActivityIDs;
                popObj.values = cData.ActivityNames;
                if (typeof afterFn == "function") {
                    afterFn(popObj);
                }
            }
        },
        afterClose: function () {
            //敲回车键执行方法
            //enterSearch(search);
        }
    });

    return popObj;
}

//添加多个绑定事件 add lxw 14.1.14
//调用：AttachEvent("id", "change", function () { alert(0); });
function AttachEvent(id, eventName, fn) {
    if (window.attachEvent)//IE
        document.getElementById(id).attachEvent("on" + eventName, fn);
    else//FF
        document.getElementById(id).addEventListener(eventName, fn, false);
}

function getHoursMinute() {
    var date = "";
    var jsDate = new Date();
    if (jsDate.getHours() < 10) {
        date = date + "0" + jsDate.getHours() + ":";
    } else {
        date = date + jsDate.getHours() + ":";
    }
    if (jsDate.getMinutes() < 10) {
        date = date + "0" + jsDate.getMinutes() + ":";
    } else {
        date = date + jsDate.getMinutes() + ":";
    }
    if (jsDate.getSeconds() < 10) {
        date = date + "0" + jsDate.getSeconds() + "";
    } else {
        date = date + jsDate.getSeconds() + "";
    } //
    return date;
}

function getHoursMinute2(ttDate) {


    var date = "";
    var jsDate = null;

    if ((typeof ttDate) == "object") {
        jsDate = ttDate;
    } else if ((typeof ttDate) == "string") {
        jsDate = new Date(ttDate.replace(/-/g, "/"));
    }

    if (jsDate.getHours() < 10) {
        date = date + "0" + jsDate.getHours() + ":";
    } else {
        date = date + jsDate.getHours() + ":";
    }
    if (jsDate.getMinutes() < 10) {
        date = date + "0" + jsDate.getMinutes() + ":";
    } else {
        date = date + jsDate.getMinutes() + ":";
    }
    if (jsDate.getSeconds() < 10) {
        date = date + "0" + jsDate.getSeconds() + "";
    } else {
        date = date + jsDate.getSeconds() + "";
    } //
    return date;
}
//比较后以取到时间
//返回值毫秒：ttDate2-ttDate1
function CompareDateTime(ttDate1, ttDate2) {
    ttDate1 = ttDate1.replace(/-/g, "/");
    ttDate2 = ttDate2.replace(/-/g, "/");
    var jsDate1 = new Date(ttDate1);
    var jsDate2 = new Date(ttDate2);
    var millsecs = 0;
    millsecs = Date.parse(jsDate2) - Date.parse(jsDate1);

    return millsecs;
}

//格式化html中的特殊字符
function FormatSpecialCharacters(msgstr) {
    msgstr = msgstr.replace(/</g, "&lt;");
    msgstr = msgstr.replace(/>/g, "&gt;");
    msgstr = msgstr.replace(/\r/g, "<br>");
    msgstr = msgstr.replace(/\n/g, "<br>");
    //msgstr = msgstr.replace(/ /g, "&nbsp;");
    //msgstr = msgstr.replace(/\&/g, "&amp;");
    msgstr = msgstr.replace(/\+/g, "&#43;");

    return msgstr;
}

//正则表达式替换文本中的url为链接
function replaceRegUrl(str) {
    var reg = /http:\/\/[\w-]*(\.[\w-]*)+/ig;
    return str.replace(reg, function (m) { return '<a href="' + m + '" target="blank">' + m + '</a>'; })
}

//分页控件中跳转使用方法

//校验文本域页码是否正确
function CheckPageNum(txt, max) {
    var v = parseInt($.trim(txt.value));
    if (isNaN(v)) {
        txt.value = "";
    }
    else {
        if (v < 1)
            v = 1;
        if (v > max)
            v = max;
        txt.value = v;
    }
}
//跳转
function GoToPageForInput(txtid, gofun, para) {
    var txt = document.getElementById(txtid);
    var v = parseInt($.trim(txt.value));
    if (isNaN(v)) {
        return;
    }
    else {
        gofun(para + "page=" + v);
    }
}
//回车跳转
function EnterPressGoTo(txtid, gofun, para, max) {
    var e = window.event;
    if (e.keyCode == 13) {
        CheckPageNum(document.getElementById(txtid), max);
        GoToPageForInput(txtid, gofun, para);
    }
}

//去掉html标签，保留img，a 标签
function HtmlReplacehaveImgA(description) {
    if (description.substring(0, 4) == "<br>") {
        description = description.substring(4);
    }
    description = description.replace(/(\n)/g, "");
    description = description.replace(/(\t)/g, "");
    description = description.replace(/<(?!\/?a|\/?IMG|\/?br|\/?p|\/?div)[^<>]*>/ig, "");
    description = description.replace(/&nbsp;/ig, '');
    return description;
}

/*--统计代码(开始)
var cnzz_protocol = (("https:" == document.location.protocol) ? " https://" : " http://");
document.write(unescape("%3Cspan id='cnzz_stat_icon_1253677917' style='display:none;' %3E%3Cscript  src='" + cnzz_protocol + "s11.cnzz.com/z_stat.php%3Fid%3D1253677917' type='text/javascript'%3E%3C/script%3E  %3C/span%3E"));
--统计代码(结束)*/



function MaskPage() {
    jQuery('body').append('<div name="divMaskPage" style="position: fixed; left: 0; top: 0; width: 100%; height: 100%; z-index: 20000;background-color: #666; opacity: 0.3; filter: alpha(opacity=30);"><div class="blue-loading" style="width:50%;float:left;background-position:right;"></div><div style="float:left;padding:20px 0px 0px 10px;">正在加载中...</div></div>');
}
function UnMaskPage() {
    $('div[name=divMaskPage]').remove();
}
/*节选自jQueryString v2.0.2*/
(function ($) {

    $.fn.Mask = function () {
        return this.each(function () {
            var this$ = $(this);
            this$.data("pos__", this$.css("position")).css("position", "relative");
            this$.append('<div name="divMaskPage__" style="position: absolute; left: 0; top: 0; right: 0;bottom: 0; z-index: 20000; background-color: #666; opacity: 0.3; filter: alpha(opacity=30);"><div class="blue-loading" style="width: 50%; float: left; background-position: right;"></div><div style="float: left; padding: 20px 0px 0px 10px;">正在加载中...</div></div>');
        });
    };
    $.fn.UnMask = function () {
        return this.each(function () {
            var this$ = $(this);
            this$.find('div[name=divMaskPage__]').remove().end().css("position", this$.data("pos__"));
        });
    };
})(jQuery);


//手机验证
function isMobile(mobile) {
    //alert("isMobile000003:"+ mobile);
    return (/^(?:13\d|15\d|17\d|18\d|19\d|14\d)-?\d{5}(\d{3}|\*{3})$/.test(mobile));
}
//电话验证
function isTel(tel) {
    return (/^(([0\+]\d{2,3})?(0\d{2,3}))(\d{7,8})$/.test(tel));
}
//电话或者手机验证
function isTelOrMobile(s) {
    //alert("isTelOrMobile000002:"+ s);
    return (isMobile(s) || isTel(s));
}


//质检列表，个人客户业务记录，话务记录，话务总表，去掉记录 易湃惠买车页面查看
function GoToEpURL(acontrl, YPFanXianHBuyCarURL, EPEmbedCCHBuyCar_APPID) {
    var OrgUrl = $(acontrl).attr("urlstr");
    AjaxPostAsync("/AjaxServers/EPEmbedCC.ashx", { YPFanXianURL: YPFanXianHBuyCarURL, GoToEPURL: OrgUrl, EPEmbedCC_APPID: EPEmbedCCHBuyCar_APPID }, null, function (data) {
        if (data) {
            if (data != "" && data != "Error") {
                try {
                    var dd = window.external.MethodScript('/browsercontrol/newpage?url=' + data);
                }
                catch (e) {
                    window.open(decodeURIComponent(data));
                }
            }
        }
    });
}
///表情弹窗


(function ($) {

    $.fn.BQPop = function (settings) {

        if ($('#BQMain').length > 0) {
            return false;
        }
        settings = jQuery.extend({
            left: 0,
            top: 0,
            isAgent: true,
            el: document,
            click: function (url, e) { }
        }, settings);

        this.each(function () {
            var this$ = $(this);
            this$.append(initHtml(settings).join(''));
            afterInit();
        });

        $(document).click(function () { $('#BQMain').hide(); });
        $('#BQMain').click(function (eve) {
            eve.stopPropagation();
            eve.preventDefault();
        });
        return $('#BQMain');
        //       
        function initHtml(settings) {


            //初始化基本属性
            var yxcTitle = ['微笑', '贫嘴', '呲牙', '调皮', '鸣笛', '高兴', '喜欢', '送花', '酷', '地宝', '侠客', '住嘴', '拍砖', '伤心', '大哭', '强', '呕吐', '眩晕', '洗车', '发怒', '大惊', '赌气', '犯错', '感动', '感冒', '害羞', '汗颜', '活波'];
            var yxmTitle = ["哀伤", "鄙视", "不屑", "大哭", "大笑", "赌气", "愤怒", "尴尬", "感冒", "乖乖", "鬼脸", "害羞", "酣睡", "好奇", "惊恐", "惊讶", "开心", "瞌睡", "亲亲", "生气", "失落", "使坏", "微笑", "委屈", "羡慕", "心动", "眩晕", "翻眼"];
            var othTitle = ["微笑", "难过", "憨笑", "大哭", "发怒", "惊讶", "调皮", "害羞", "偷笑", "流汗", "抓狂", "呲牙", "拥抱", "胜利", "时间", "吻", "握手", "电话", "可爱", "惊恐", "OK", "大牙", "瞌睡", "衰", "坏笑", "心", "大便", "强"];
            var othName = ["smile", "sad", "biggrin", "cry", "huffy", "shocked", "tongue", "shy", "titter", "sweat", "mad", "lol", "hug", "victory", "time", "kiss", "handshake", "call", "loveliness", "funk", "ok", "proud", "sleep", "failure", "badsmile", "heart", "shit", "strong"];
            var yxc = [], yxm = [], oth = [];
            var tS;
            for (var i = 0; i < 28; i++) {
                if (i % 7 == 0) {
                    tS = ' style="border-left: 1px solid #ccc;" ';
                } else {
                    tS = '';
                }
                yxc.push('<li ' + tS + ' onmouseover="showitem(\'http://img1.baa.bitautotech.com/webpic/smilies/baabig/' + convertName(i) + '.gif\',this,' + i + ')" onclick="BQClick(\'http://img1.baa.bitautotech.com/webpic/smilies/baabig/' + convertName(i) + '.gif\',this)"><span title="' + yxcTitle[i] + '" style="background-position: -' + (i * 24) + 'px -174px;" ></span></li>');
                yxm.push('<li ' + tS + '  onmouseover="showitem(\'http://img1.baa.bitautotech.com/webpic/smilies/yxm/yxm' + convert2Name(i) + '.gif\',this,' + i + ')" onclick="BQClick(\'http://img1.baa.bitautotech.com/webpic/smilies/yxm/yxm' + convert2Name(i) + '.gif\',this)"><span title="' + yxmTitle[i] + '"  style="background-position: -' + (i * 24) + 'px -222px;"></span></li>');
                oth.push('<li ' + tS + '  onmouseover="showitem(\'http://img1.baa.bitautotech.com/webpic/smilies/default/' + othName[i] + '.gif\',this,' + i + ')" onclick="BQClick(\'http://img1.baa.bitautotech.com/webpic/smilies/default/' + othName[i] + '.gif\',this)"><span title="' + othTitle[i] + '"  style="background-position: -' + (i * 24) + 'px -30px;"></span></li>');
            }
            var tLR = "";
            if (settings.right) {
                tLR += ";right:" + settings.right + "px;";
            } else {
                tLR += ";left:" + settings.left + "px;";
            }
            if (settings.bottom) {
                tLR += ";bottom:" + settings.bottom + "px;";
            } else {
                tLR += ";top:" + settings.top + "px;";
            }

            var sHtml = [];
            sHtml.push('');
            sHtml.push('<div class="yc_Emotiondiv" style="position: absolute;display:none;' + tLR + '" id="BQMain">');
            sHtml.push('<div class="imgContentbox" style="position: relative;" id="divBQContain">');
            sHtml.push('<div id="divFlagBQImg" class="imgDiv">');
            sHtml.push('<img width="30px" height="30px" src="" id="imgFlagBQ" style="margin: 14px;" />');
            sHtml.push('</div>');
            sHtml.push('<div id="con_BQ_2" style="" did="1">');
            sHtml.push('<ul class="ulBQ" id="yxc">' + yxc.join('') + '</ul></div>');
            sHtml.push('<div id="con_BQ_3" style="display: none;" did="2">');
            sHtml.push('<ul class="ulBQ" id="yxm">' + yxm.join('') + '</ul></div>');
            sHtml.push('<div id="con_BQ_1" style="display: none;" did="3">');
            sHtml.push('<ul class="ulBQ" id="ulOth">' + oth.join('') + '</ul></div>');
            sHtml.push('</div>');
            sHtml.push('<div class="menuContentbox">');
            sHtml.push('<ul><li id="BQ2" onclick="setEmotionTab(\'BQ\',2,3)" class="hover">易小车</li>');
            sHtml.push('<li id="BQ3" onclick="setEmotionTab(\'BQ\',3,3)" class="">易小妹</li>');
            sHtml.push('<li id="BQ1" onclick="setEmotionTab(\'BQ\',1,3)" class="">其它</li></ul>');
            sHtml.push('</div></div>');
            return sHtml;
        }

        //内部函数
        function convertName(n) {
            n++;
            if (n < 10) {
                return '00' + n;
            } else if (n < 99) {
                return '0' + n;
            } else {
                return n;
            }
        }
        function convert2Name(n) {
            n++;
            if (n < 10) {
                return '0' + n;
            } else {
                return n;
            }
        }
        function afterInit() {
            window.divImg$ = $('#divFlagBQImg');
            window.imgFlagBQ$ = $('#imgFlagBQ');

            $('#divBQContain').mouseout(function () {
                divImg$.hide();
            });

            if (!window.setEmotionTab) {
                window.setEmotionTab = function (name, cursel, n) {
                    for (i = 1; i <= n; i++) {
                        var menu = document.getElementById(name + i);
                        var con = document.getElementById("con_" + name + "_" + i);
                        menu.className = i == cursel ? "hover" : "";
                        con.style.display = i == cursel ? "block" : "none";
                    }
                    //                    event.stopPropagation();
                    //                    event.preventDefault();
                    return false;
                };
            }
            if (!window.showitem) {
                window.showitem = function (url, e, i) {
                    imgFlagBQ$.attr('src', url);
                    var pos$ = $(e).position();
                    if (i < 21 || settings.isAgent) {
                        divImg$.css({ 'top': pos$.top + 45, 'left': pos$.left + 45 }).show();
                    } else {
                        divImg$.css({ 'top': pos$.top - 62, 'left': pos$.left + 45 }).show();
                    }
                };
            }
        }

    };

})(jQuery);

function GetServerTime(dt) {
    if (dt && typeof (dt) == "object") {
        window.BTTimeNow = dt;
        return false;
    }
    if (!window.BTTimeNow) {
        $.post("../AjaxServers/Handler.ashx", { action: "getservertime" }, function (data) {
            data = $.parseJSON(data);
            if (!data) {
                alert("获取服务器时间失败");
                return false;
            }
            window.BTTimeNow = new Date(data.result);
            setInterval(function () {
                window.BTTimeNow.setSeconds(window.BTTimeNow.getSeconds() + 1);
            }, 1000);
            return window.BTTimeNow;
        });
    } else {
        return window.BTTimeNow;
    }
}

//跳转查看会话历史记录
function GotoConversation(a, url, visitid, startime, endtime) {
    url = decodeURIComponent(url);
    var paras = "{'VisitID':'" + $.trim(visitid) + "','QueryStartTime':'" + $.trim(startime) + "','QueryEndTime':'" + $.trim(endtime) + "','TimeStamp':'" + new Date().getTime() + "'}";
    AjaxPostAsync("/AjaxServers/CommonHandler.ashx",
             { Action: "EncryptString", EncryptInfo: paras, r: Math.random() }, null, function (data) {
                 var href = url + "?data=" + data;
                 $(a).attr("href", href);
             });
}

function moveEnd(obj) {
    var html = "";
    if (obj instanceof jQuery) {
        html = $(obj).html();
        obj.focus();
    }
    else {
        html = document.getElementById(obj).innerHTML;
    }


    var count = html.split('').length; // lyTXT1.innerHTML.split('').length;

    var sel;
    var browser = checkBrowser().split(":");
    var IEbrowser = checkBrowser().split(":")[0];
    var IEverson = Number(checkBrowser().split(":")[1]);

    if (IEbrowser == "IE") {
        //判断浏览器是ie，但不是ie9以上
        if (IEbrowser == "IE" && IEverson < 9) {
            if (obj instanceof jQuery) {
                $(obj).html('');
            }
            else {
                $("#" + obj).html('');
            }
            var range = document.selection.createRange();
            range.pasteHTML(html);
        } else {
            sel = window.getSelection();
            sel.collapseToEnd();
        }
    }
    else if (IEbrowser == "Firefox" || IEbrowser == "Chrome" || IEbrowser == "Opera" || IEbrowser == "Safari") {
        if (window.getSelection) {
            sel = window.getSelection();
            for (var i = 0; i < count; i++) {
                sel.modify('move', 'right', 'line');
            }
        }
        else if (document.getSelection) {
            sel = document.getSelection();
            for (var i = 0; i < count; i++) {
                sel.modify('move', 'right', 'line');
            }
        }
    }
    else {
        if (window.getSelection) {
            sel = window.getSelection();
            try {
                sel.collapseToEnd();
            } catch (e) {
                for (var i = 0; i < count; i++) {
                    sel.modify('move', 'right', 'line');
                }
            }
        }
        else if (document.getSelection) {
            sel = document.getSelection();
            try {
                sel.collapseToEnd();
            } catch (e) {
                for (var i = 0; i < count; i++) {
                    sel.modify('move', 'right', 'line');
                }
            }

        }
    }



}


function checkBrowser() {
    var browserName = navigator.userAgent.toLowerCase();
    //var ua = navigator.userAgent.toLowerCase();
    var Sys = {};
    var rtn = false;

    if (/msie/i.test(browserName) && !/opera/.test(browserName)) {
        strBrowser = "IE: " + browserName.match(/msie ([\d.]+)/)[1];
        rtn = true;
        //return true;
    } else if (/firefox/i.test(browserName)) {
        strBrowser = "Firefox: " + browserName.match(/firefox\/([\d.]+)/)[1]; ;
        //return false;
    } else if (/chrome/i.test(browserName) && /webkit/i.test(browserName) && /mozilla/i.test(browserName)) {
        strBrowser = "Chrome: " + browserName.match(/chrome\/([\d.]+)/)[1];
        //return false;
    } else if (/opera/i.test(browserName)) {
        strBrowser = "Opera: " + browserName.match(/opera.([\d.]+)/)[1];
        //return false;
    } else if (/webkit/i.test(browserName) && !(/chrome/i.test(browserName) && /webkit/i.test(browserName) && /mozilla/i.test(browserName))) {
        strBrowser = ": ";
        //return false;
    } else {
        strBrowser = "unKnow,未知浏览器 ";
        //return false;
    }
    strBrowser = strBrowser;
    //alert(strBrowser)
    return strBrowser;
}



//###################################################    OnlineService.js   ##############################################

//请求进入队列
function CominQuene(LoginID, SourceType) {
    var pody = { action: 'cominquene', FromPrivateToken: escape(LoginID), SourceType: escape(SourceType) };
    AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
             function (msg) {
                 var r1 = JSON.parse(msg);
                 //alert(r1.result);
                 if (r1 != null && r1.result == '0') {//登录成功之后
                     //建立长连接
                     defaultChannel = null;
                     Connect();
                 }
                 else if (r1 != null && r1.result == '1') {
                     //0:成功;1:参数错误，未找到业务线，2：等待队列已满
                     $("#divagentAllocat").html("<div class='an_info'><img width='16' height='16' src='images/an_jr.png'>您好，欢迎使用在线客服系统。参数错误，未找到业务线，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a></div>");
                     $("#divagentAllocat").css("display", "");
                     $("#Rmessages").css("height", "85%");
                 }
                 else if (r1 != null && r1.result == '2') {
                     $("#divagentAllocat").html("<div class='an_info'><img width='16' height='16' src='images/an_jr.png'>您好，欢迎使用在线客服系统。目前没有空闲客服，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a></div>");
                     $("#divagentAllocat").css("display", "");
                     $("#Rmessages").css("height", "85%");
                 }
             });
}
function InitUploadify() {
    var uploadSuccess = true;
    $("#uploadify").uploadify({
        'buttonText': '选择',
        'buttonImg': 'css/img/shangchuan.png',
        'hideButton': false,
        'queueID': 'fileQueue',
        'auto': true,
        'swf': 'Scripts/uploadify.swf',
        'uploader': '/AjaxServers/FileLoad.ashx?v=' + Math.random(),
        'multi': false,
        'fileSizeLimit': '5MB',
        'queueSizeLimit': 1,
        'method': 'post',
        'removeTimeout': 1,
        'fileTypeDesc': '*.doc;*.docx;*.ppt;*.pptx;*.xls;*.xlsx;*.pps;*.pdf;*.txt;*.jpg;*.gif;*.png',
        'fileTypeExts': '*.doc;*.docx;*.ppt;*.pptx;*.xls;*.xlsx;*.pps;*.pdf;*.txt;*.jpg;*.gif;*.png',
        'width': 10,
        'height': 26,
        'onUploadSuccess': function (file, data, response) {
            if (response == false) {
                uploadSuccess = false;
                $.jAlert("上传失败!");
            }
            else {
                //    alert('The file ' + file.name + ' was successfully uploaded with a response of ' + response + ':' + data);
                var jsonData = $.evalJSON(data);
                if (jsonData.result == "noFiles") {
                    uploadSuccess = false;
                    $.jAlert("请选择文件!");
                }
                else if (jsonData.result == "failure") {
                    uploadSuccess = false;
                    $.jAlert("上传文件出错!");
                }
                else if (jsonData.result != "succeed") {
                    uploadSuccess = false;
                }
                else {
                    //上传成功
                    uploadSuccess = true;
                    var filepath = jsonData.FilePath;
                    filepath = filepath.replace(new RegExp(/(--)/g), '/');
                    //var filename = unescape(jsonData.FileName);
                    var filename = jsonData.FileName;
                    var hostT = window.location.href.substr(0, window.location.href.indexOf('/', 8)) + filepath;
                    $('#Smessage').html("<a href=\"" + hostT + "\" class=\"upfile\" target=\"_blank\">下载文件：" + filename + "</a>");
                    SendMessage("7", filename, jsonData.ExtendName, jsonData.FileSize, filepath);
                }
            }
        },
        'onQueueComplete': function (queueData) {
            // alert(queueData.uploadsSuccessful + ' files were successfully uploaded.');
            if (uploadSuccess) { //上传都成功
            }
        }
    });
}
function GetQuick() {
    var quickvalue = GetCookie("quickvalue");
    //快捷键从cookie读取
    if (quickvalue != null && quickvalue != "") {
        if (quickvalue == "1") {
            $("#radenter").attr("checked", true);
        }
        else {
            $("#radctrlenter").attr("checked", true);
        }
    }
    else {
        $("#radctrlenter").attr("checked", true);
    }
}
function setbigsmall() {
    //    $('#bodyDIV').css('height', (($(window).height() - 20) + 'px'));
    $('#divcontent').css('height', (($(window).height() - 20 - 33 - 20) + 'px'));
    //    //            //指定左边高度
    //   $('#divleft').css('height', (($(window).height() - 20 - 33 - 30) + 'px'));
    //    //            //指定右面高度
    //    $('#divright').css('height', (($(window).height() - 20 - 33 - 30) + 'px'));
}
//给cookie设置网友所选快捷键，以便下次访问不要再重新设置快捷键
function SetQuick(quickvalue) {
    SetCookie("quickvalue", quickvalue);
}
//页面大小关闭时触发，让页面始终适合页面大小
function ChangeBigSmall() {
    setbigsmall();
}
//刷新关闭执行方法
function onbeforeunload_handler() {
    var sendto = $('#hidto').val();
    var pody = { action: 'userclosechat', FromPrivateToken: escape(privatetaken), SendToPublicToken: escape(sendto) };
    AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
             function (msg) {
                 var r = JSON.parse(msg);
                 if (r != null && r.result == 'sendok') {//登录成功之后
                 }
             });
}


//发送文件消息
function SendFileHandLer(privateToken, alias, message) {
    messageFlicker.clear();
    var rectime = parseLongDate(message.t);
    rectime = getHoursMinute2(rectime);
    //message.c.m = replaceRegUrl(message.c.m);
    document.getElementById("Rmessages").innerHTML += zts(number, rectime, message.m);
    //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_name'>客服" + number + "<span>【" + rectime + "】</span></div><div class='user_con'>" + message.m + "</div></div></div><div class='clearfix'></div>";

    document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
    messageFlicker.show();
}

//客服发起满意度评价消息
function SatisfactionHandLer(privateToken, alias, message) {
    document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/xiaoxin.png'>非常感谢您的使用，请<a style='cursor:pointer' href='#'  onclick='addSatisfaction()'>点击这里</a>对我的服务做出评价！")
    //"<p class='hs'>非常感谢您的使用，请<a style='cursor:pointer' href='#'  onclick='addSatisfaction()'>点击这里</a>对我的服务做出评价！</p>";
}

////发送排队信息消息
function QueueOrderHandLer(privateToken, alias, message) {
    Chatdisabled(0);
    $("#divagentAllocat").html("");
    document.getElementById("divagentAllocat").innerHTML += "<div class='an_tip' style='margin-bottom:10px;'><div class='an_info'><img width='16' height='16' src='images/an_dd.png'>您好，欢迎使用易车在线客服系统。</div></div>";
    document.getElementById("divagentAllocat").innerHTML += "<div class='an_tip' style='margin-bottom:10px;'><div class='an_info'><img width='16' height='16' src='images/an_dd.png'>" + message.m + "如您不想继续等待，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a>！</div></div>"
    $("#divagentAllocat").css("display", "");
    $("#Rmessages").css("height", "85%");
}

////转接消息
function TransferHandLer(privateToken, alias, message) {
    Chatdisabled(1);
    messageFlicker.clear();
    //坐席标识
    $('#hidto').val(message.a);
    number = message.anum;
    //坐席分配标识
    $('#hidAllocID').val(message.cs);
    isManyi = false;
    var AllocID = $('#hidAllocID').val();
    document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>对话被中断或已结束！");
    document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/an_dd.png'>请稍候，正在为您转接指定客服。");
    document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/an_jr.png'>您好，欢迎使用在线客服系统，客服已加入会话。");
    var rectime = parseTime(message.cst);
    document.getElementById("Rmessages").innerHTML += zts(number, rectime, "您好，易车客服" + number + "号很高兴为您服务！");
    //zs("您好，易车客服" + message.anum + "号很高兴为您服务！");

    messageFlicker.show();
}

//继续排队
function ContinueQueue() {

    $.jConfirm("您确定要继续排队吗？", function (r) {
        if (r) {
            $("#divagentAllocat").html("");
            setTimeout(function () {
                var pody = { action: 'resetagent', FromPrivateToken: escape(privatetaken) };
                AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
                                             function (msg) {
                                                 if (msg != "") {
                                                     var m = JSON.parse(msg);
                                                     if (m != null && m.result == 'Initializeok') {//登录成功之后
                                                         //StartChat();
                                                     }
                                                     else {

                                                     }
                                                 }
                                             });
            }, 1000);
        }
    });
}
//分配坐席
function AllocAgentForUserHandler(privateToken, alias, message) {
    //把坐席标识放在隐藏域里
    setTimeout(function () {

        messageFlicker.clear();
        messageFlicker.show();
        Chatdisabled(1);
        $("#hidto").val(message.a);
        number = message.anum;
        //$("#divagentNo").html("易车网服务顾问 " + message.anum);
        //把分配坐席标识
        $("#hidAllocID").val(message.cs);
        GetHMessageByAgent(privateToken, message.a);
        //分配后就是可以重新做满意度评价
        isManyi = false;
        $("#Rmessages").css("height", "100%");
        $("#divagentAllocat").html("");
        $("#divagentAllocat").hide();
        document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/an_jr.png'>您好，欢迎使用在线客服系统，客服已加入对话。");
        //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'> 您好，易车网运营服务中心竭诚为您服务，已为您接通专属服务顾问" + message.anum + "，请问有什么可以帮您？</div></div></div><div class='clearfix'></div>";
        //让信息框和发送信息按钮可用
        var rectime = parseTime(message.cst);
        document.getElementById("Rmessages").innerHTML += zts(number, rectime, "您好，易车客服" + number + "号很高兴为您服务！");

    }, 500);


}
function alloc() {

}
//坐席全忙，去留言
function addMessage() {
    $.openPopupLayer({
        name: "AddOnlineMessageAjaxPopup",
        parameters: { VisitID: visitid },
        url: "/OnLineMessageForm.aspx?r=" + Math.random()
    });

}
//坐席全忙
function MAllBussyHandler(privateToken, alias, message) {
    Chatdisabled(0);
    $("#divagentAllocat").html("<img width='16' height='16' src='images/an_dd.png'>您好，欢迎使用易车在线客服。目前没有空闲客服，请<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);addMessage()'>点击留言</a>");
    $("#divagentAllocat").css("display", "");
    $("#Rmessages").css("height", "85%");
}
//成功接收到聊天信息事件
function SuccessHandler(privateToken, alias, message) {
    if ($("#divagentAllocat").html() != "") {
        $("#divagentAllocat").html("");
        $("#divagentAllocat").css("display", "none");
        $("#Rmessages").css("height", "100%");
    }
    messageFlicker.clear();
    var rectime = getHoursMinute2(message.ct);
    //message.c.m = replaceRegUrl(message.c.m);
    document.getElementById("Rmessages").innerHTML += zts(number, rectime, message.c.m);
    //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_name'>客服" + number + "<span>【" + rectime + "】</span></div><div class='user_con'>" + message.c.m + "</div></div></div><div class='clearfix'></div>";


    document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
    messageFlicker.show();
}
//错误通知
function FailureHandler(privateToken, alias, error) {
    messageFlicker.clear();
    Chatdisabled(0);
    messageFlicker.show();
    $("#divagentAllocat").html("");
    $("#divagentAllocat").hide();
    //长连接错误，但坐席没有离开
    if ($('#hidto').val() != "" && isleave == "0") {
        document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>")
        // "<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></div></div></div><div class='clearfix'></div>";
    }
    //长连接错误，但是没有分配坐席
    else if ($('#hidto').val() == "" && isleave == "0") {
        document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>排队中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(1)'>继续排队</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束排队</a>")
    }
    document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
}
//坐席离开通知
function AgentLeaveHandler(privateToken, alias, message) {
    $('#hidto').val("");
    isleave = "1";
    messageFlicker.clear();
    document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(0)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>")
    //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='RestAllocAgent(0)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></div></div></div><div class='clearfix'></div>";


    document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
    Chatdisabled(0);
    messageFlicker.show();
}
//长连接超时通知
function TimeoutHandler(privateToken, alias, message) {
    GetServerTime(new Date(parseFloat(message.ct)));
}


//发送消息，sendtype=7是文件,不传是文本
function SendMessage(MessageType, FileName, FileType, FileSize, FilePath) {
    //消息接收者
    var sendto = $('#hidto').val();
    //坐席分配标识
    var AllocID = $('#hidAllocID').val();
    //var issuccess=0;
    var messagetxt = $('#Smessage').html();
    messagetxt = $.trim(messagetxt);

    //替换html,保留换行，img,a标签
    messagetxt = HtmlReplacehaveImgA(messagetxt);
    messagetxt = $.trim(messagetxt);
    if (sendto == "") {
        document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>");
        //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>对话被中断或已结束！如果您需要继续咨询，请点击<a style='cursor:pointer'  onclick='SetBeforeunload(false, onbeforeunload_handler);RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></div></div></div><div class='clearfix'></div>";
    }
    else {
        if (MessageType == "6") {
            messagetxt = "网友已做满意度评价！";
        }
        //当通知网友已做满意度评价消息，消息信息为空
        if (messagetxt == "" || messagetxt == "<br>") {
            $.jAlert("消息不能为空！");

        }
        else if (messagetxt == "请用一句话简要、准确地描述您的问题，如易车惠购车返现问题") {
            $.jAlert("消息不能为空！");

        }
        else if (Len(messagetxt) > 500) {
            $.jAlert("文本内容太长，请整理后重新发送！");
        }
        else {
            var sendmessage = messagetxt;
            $('#Smessage').html("");
            //发送文件
            var pody;
            if (MessageType == "7") {
                pody = { action: 'sendmessage', FromPrivateToken: escape(privatetaken), usertype: escape('2'), message: escape(sendmessage), SendToPrivateToken: escape(sendto), AllocID: escape(AllocID), MessageType: escape(MessageType), FileName: escape(FileName), FileType: escape(FileType), FileSize: escape(FileSize), FilePath: escape(FilePath) };
            }
            else {
                pody = { action: 'sendmessage', FromPrivateToken: escape(privatetaken), usertype: escape('2'), message: escape(sendmessage), SendToPrivateToken: escape(sendto), AllocID: escape(AllocID), MessageType: escape(MessageType) };
            }
            //var idNow = Date.now();
            var idNow = (new Date()).valueOf();
            if (MessageType != "6") {
                document.getElementById("Rmessages").innerHTML += wts("您", GetDateNow(), sendmessage);
            }

            //"<div class='dh2' id='" + idNow + "'><div class='title title2'>您 <span>：" + GetDateNow() + "</span><em class='embg'></em></div><div class='dhc dhc2'>" + sendmessage + "</div></div>";
            document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
            AjaxPost('AjaxServers/Handler.ashx', pody, null,
             function (msg) {
                 if (msg != "") {
                     var r = JSON.parse(msg);
                     //消息发送成功
                     if (r != null && r.result != "SendToLeave" && r.result != "ClientNotExists") {//登录成功之后
                         var r2 = $.evalJSON(r.result);
                         if (r2.result == 'sendok') {
                             //var dtime = getHoursMinute2(r2.rectime);
                             //sendmessage = replaceRegUrl(sendmessage);
                             // document.getElementById("Rmessages").innerHTML += "<div class='dh2'><div class='title title2'>您 <span>：" + dtime + "</span><em class='embg'></em></div><div class='dhc dhc2'>" + sendmessage + "</div></div>";                             
                             $('#' + idNow + ' .title2 em').removeClass('embg').html("");
                         }
                         //else {
                         //    $('#' + idNow + ' .title2 em').removeClass('embg').html("发送消息失败...");
                         //}
                     }
                     //坐席离线
                     else if (r != null && r.result == 'SendToLeave') {
                         document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>消息: " + sendmessage + " 发送失败，对话已结束！如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(0)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>")
                         //"<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>消息: " + sendmessage + " 发送失败，对话已结束！如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(0)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></div></div></div><div class='clearfix'></div>";

                     }
                     //网友客户端实体不存在
                     else if (r != null && r.result == 'ClientNotExists') {
                         document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>消息: " + sendmessage + " 发送失败，对话已结束！</p><p class='hs'>如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a>")
                         //"<p class='hs'>消息: " + sendmessage + " 发送失败，对话已结束！</p><p class='hs'>如果您需要继续咨询，请点击<a style='cursor:pointer' onclick='RestAllocAgent(1)'>继续对话</a>，如果您的问题已解决，请点击<a style='cursor:pointer'  onclick='CloseWindow(0)'>结束对话</a></p>";
                     }

                 }
                 else {
                     document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/jieshu.png'>消息: " + sendmessage + " 发送失败，您的网络出现异常！")
                     //"<p class='hs'>消息: " + sendmessage + " 发送失败，您的网络出现异常！</p>";
                     $.jAlert("您的网络出现异常，请检查网络后重试！");
                 }
                 document.getElementById('Rmessages').scrollTop = document.getElementById('Rmessages').scrollHeight;
             });
        }
    }
}
//继续等待，分两种是1是需要初始化网友实体，并且重新建立长连接，0是长连接存在，只是重新排队
function RestAllocAgent(Reset) {
    //$("#divagentNo").html("易车网服务顾问");
    number = "";
    isleave = "0";
    document.getElementById("Rmessages").innerHTML = "";
    if (Reset == '0') {
        var pody = { action: 'resetagent', FromPrivateToken: escape(privatetaken) };
        AjaxPostAsync('AjaxServers/Handler.ashx', pody, null,
                         function (msg) {
                             if (msg != "") {
                                 var r = JSON.parse(msg);
                                 if (r != null && r.result == 'Initializeok') {//登录成功之后
                                     StartChat();
                                 }
                                 else {

                                     CominQuene(privatetaken, SourceType);
                                 }
                             }
                         });
    } else {
        StartChat();
    }
}

//绑定发送消息快捷键
function onKeyPress(e) {
    var keyCode = null;
    if (e.which)
        keyCode = e.which;
    else if (e.keyCode)
        keyCode = e.keyCode;
    var quickvalue = "";
    $("input[name='RadioQuick']").each(function () {
        if ($(this).attr("checked")) {
            quickvalue = $(this).val();
        }
    });

    if (quickvalue == "1") {
        if (keyCode == 13) {
            SendMessage();
            return false;
        }
    }
    else if (quickvalue == "2") {
        //如果是火狐浏览器
        if (navigator.userAgent.indexOf('Firefox') >= 0) {
            if (e.ctrlKey && keyCode == 13) {
                SendMessage();
                return false;
            }
        }
        else {
            if (event.ctrlKey && keyCode == 10) {
                SendMessage();
                return false;
            }
        }
    }

    return true;
}
//文本框提示语
function ClearTitle() {
    if ($("#Smessage").html() == "请用一句话简要、准确地描述您的问题，如易车惠购车返现问题") {
        $("#Smessage").html("");
    }
}
//窗口关闭按钮事件
function CloseWindow(ConfirmValue) {
    if (ConfirmValue == 0) {
        $.jConfirm("您确定要结束对话么？", function (r) {
            if (r) {
                SetBeforeunload(false, onbeforeunload_handler);
                onbeforeunload_handler();
                window.opener = null; window.open('', '_self'); window.close();
            }

        });
    } else {
        SetBeforeunload(false, onbeforeunload_handler);
        onbeforeunload_handler();
        window.opener = null; window.open('', '_self'); window.close();
    }
}
function shangchuan() {
    var uploadify = $('#uploadify');
    uploadify.uploadify('upload', '*');

}

//根据客户ID与坐席ID，取近六条聊天记录
function GetHMessageByAgent(privatetaken, agentid) {
    //    AjaxPostAsync("AjaxServers/Handler.ashx", {
    //        action: escape("getfristhistroy"),
    //        FromPrivateToken: escape(privatetaken),
    //        SendToPrivateToken: escape(agentid)
    //    }, null, function (data) {
    //        if (data != "") {
    //            var jsonData = JSON.parse(data);
    //            //不成功提示错误，成功把录音主键保存在隐藏域里面

    //            document.getElementById("Rmessages").innerHTML += "<p class='his_more'> ---查看更多，请点击 <a href=\"ConversationHistory.aspx?UserID=" + agentid + "&LoginID=" + privatetaken + "\" target=\"_blank\">历史记录</a>---</p>";
    //            document.getElementById("Rmessages").innerHTML += "<div class='fix_gd'>";
    //            for (var i = 0; i < jsonData.length; i++) {
    //                if (jsonData[i].Sender == "1") {
    //                    document.getElementById("Rmessages").innerHTML += zts(jsonData[i].AgentNum, jsonData[i].CreateTime, jsonData[i].Content);
    //                    //"<div class='dh1'><div class='title'>客服" + jsonData[i].AgentNum + "说（" + jsonData[i].CreateTime + "）：</div><div class='dhc'>" +
    //                    //jsonData[i].Content + "</div></div>";
    //                }
    //                else {
    //                    document.getElementById("Rmessages").innerHTML += wts("您", jsonData[i].CreateTime, jsonData[i].Content);
    //                    //"<div class='dh1 dh2'><div class='title'>您说（" + jsonData[i].CreateTime + "）：</div><div class='dhc'>" +
    //                    //jsonData[i].Content + "</div></div>";
    //                }

    //            }
    //        }
    //        var nowdate = new Date();
    //        var datestr = nowdate.getFullYear() + "年" + (nowdate.getMonth() + 1) + "月" + nowdate.getDate() + "日";
    //        document.getElementById("Rmessages").innerHTML += "</div><p class='his_more'>---" + datestr + "---</p>"
    //    });
}
function jieping() {
    //scpMgr.Capture();
    f_capture();
}
//事件-传输完毕
function ScreenCapture_Complete(obj) {
    obj.Progress.text("100%");
    obj.Message.text("上传完成");
    obj.State = obj.StateType.Complete;
    obj.CloseInfPanel(); //隐藏信息层
    //显示截取的屏幕图片
    var hostT = window.location.href.substr(0, window.location.href.indexOf('/', 8)) + obj.ATL.Response;
    var imgstr = "<img src='" + hostT + "'/>";

    $("#Smessage").html(oldText + imgstr);
    //设置为截屏
    $("#hidIsJiePing").val(imgstr);
}
function f_onUploadCompleted(responseText) {
    f_log("图片上传完成.");
    var jsonData = $.evalJSON(responseText);
    if (jsonData.result == "noFiles") {
        $.jAlert("截屏出错!");
    }
    else if (jsonData.result == "failure") {
        $.jAlert("截屏出错!");
    }
    else if (jsonData.result != "succeed") {
        $.jAlert("截屏出错!");
    }
    else {
        //上传成功
        var filepath = jsonData.FilePath;
        filepath = filepath.replace(new RegExp(/(--)/g), '/');

        var hostT = window.location.href.substr(0, window.location.href.indexOf('/', 8)) + filepath;

        var imgstr = "<img src='" + hostT + "'/>";
        var oldText = $.trim($("#Smessage").html());
        $("#Smessage").html(oldText + imgstr);
        //设置为截屏
        $("#hidIsJiePing").val(imgstr);
    }
}
//加载常用问题
function f_getFreProblemlist(SourceType) {
    LoadingAnimation('divquestion');
    $('#divquestion').load('SeniorManage/FreProblemForClient.aspx?SourceType=' + SourceType + '&r=' + Math.random(), null);

}
//取历史聊天记录
function HistroyMore() {
    var userid = $("#hidto").val();
    window.open("ConversationHistory.aspx?LoginID=" + escape(loginid) + "&r=" + Math.random());
}
//满意度弹层
function addSatisfaction() {
    if (isManyi) {
        $.jAlert("您已对客服进行过评价，请勿重复提交~");
    }
    else {
        var csid = $("#hidAllocID").val();
        $.openPopupLayer({
            name: "AddSatisfactionAjaxPopup",
            parameters: { CSID: csid },
            url: "/SatisfactionForm.aspx?r=" + Math.random(),
            beforeClose: function (n) {
                if (n == true) {
                    isManyi = true;
                    //$("#EMYiChe").unbind("click");
                    //发送满意度消息给客服
                    SendMessage("6", "", "", "", "");

                    document.getElementById("Rmessages").innerHTML += js("<img width='16' height='16' src='images/taiyang.png'>非常感谢您对我的服务做出评价，祝您生活愉快。^_^");
                }

            }
        });
    }
}
//设置按钮是否可用
function Chatdisabled(type) {
    if (type == 0) {
        $("#bq_listSH").css("display", "none");
        $("#HistroyList").css("display", "none");
        $("#btnCapture").css("display", "none");
        $("#EMYiChe").css("display", "none");
        $("#fileUpload").css("display", "none");


        $("#btnSend").attr("disabled", "disabled");
        //$("#Smessage").attr("contenteditable", false);
    }
    else {


        $("#bq_listSH").css("display", "");
        $("#HistroyList").css("display", "");
        $("#btnCapture").css("display", "");
        $("#EMYiChe").css("display", "");
        $("#fileUpload").css("display", "");

        $("#btnSend").removeAttr("disabled");
        $("#Smessage").attr("contenteditable", true);
    }
}


function parseTime(strDate) {
    if (strDate == "" || strDate == null) {
        return "";
    }
    else {
        var myDate = new Date(parseInt(strDate.substring(strDate.indexOf("(") + 1, strDate.indexOf(")"))));
        var h = myDate.getHours();
        if (h < 10) {
            h = "0" + h;
        }
        var m = myDate.getMinutes();
        if (m < 10) {
            m = "0" + m;
        }
        var s = myDate.getSeconds("ss");
        if (s < 10) {
            s = "0" + s;
        }
        return h + ":" + m + ":" + s;
    }
}


///针对类型///Date(-2209017600000+0800)/
function parseLongDate(strDate) {
    if (strDate == "" || strDate == null) {
        return "";
    }
    else {
        var myDate = new Date(parseInt(strDate.substring(strDate.indexOf("(") + 1, strDate.indexOf(")"))));
        var year = myDate.getFullYear();
        var month = myDate.getMonth() + 1;
        if (month < 10) {
            month = "0" + month;
        }
        var day = myDate.getDate();
        if (day < 10) {
            day = "0" + day;
        }
        var h = myDate.getHours();
        if (h < 10) {
            h = "0" + h;
        }
        var m = myDate.getMinutes();
        if (m < 10) {
            m = "0" + m;
        }
        var s = myDate.getSeconds("ss");
        if (s < 10) {
            s = "0" + s;
        }
        return year + "/" + month + "/" + day + " " + h + ":" + m + ":" + s;
    }
}

function GetDateNow() {

    var myDate = GetServerTime();

    var h = myDate.getHours();
    if (h < 10) {
        h = "0" + h;
    }
    var m = myDate.getMinutes();
    if (m < 10) {
        m = "0" + m;
    }
    var s = myDate.getSeconds("ss");
    if (s < 10) {
        s = "0" + s;
    }
    return h + ":" + m + ":" + s;

}

function zts(z, t, s) {
    return "<div class='dh_user1' style='padding: 0px 10px;margin: 3px 0px;'><table  border='0'><tr><td rowspan='2' style='width:40px;' class='top_dq'><div class='user_img'><img src='" + AgentHeadURL + "'></div></td><td><div class='user_name'><span>客服" + z + "</span>" + t + "</div></td></tr><tr><td><div class='im-con'><div class='arrow'></div><div class='user_con' >" + s + "</div></div></td></tr></table></div><div class='clearfix'></div>";
}
function zs(msg) {
    return "<div class='dh_user1' style='padding: 0px 10px;margin: 3px 0px;'><table  border='0'><tr><td rowspan='2' style='width:40px;' class='top_dq'><div class='user_img'><img src='" + AgentHeadURL + "'></div></td></tr><tr><td><div class='im-con'><div class='arrow'></div><div class='user_con' >" + msg + "</div></div></td></tr></table></div><div class='clearfix'></div>";
    //return "<div class='dh_user1'><div class='user_img'><img width='70' height='70' src='" + AgentHeadURL + "'></div><div class='user_content'><div class='arrow'></div><div class='user_con'>" + msg + "</div></div></div><div class='clearfix'></div>";
}
function wts(w, t, s) {

    return "<div class='dh_user1 dh_user2' style='padding: 0px 10px;margin: -1px 0px; float:right;'><table  border='0'><tr><td><div class='user_name'><span>" + w + "</span>" + t + "</div></td><td rowspan='2' style='width:40px;' class='top_dq'><div class='user_img'><img src='images/user-im.png' /></div></td> </tr><tr><td><div class='im-con'><div class='user_con' >" + s + "<div class='arrow2'></div></div></div></td></tr></table></div><div class='clearfix'></div>"

    //return "<div class='dh_user1 dh_user2' style='padding: 0px 10px; float:right;'><div class='user_img' style='float:right;'><img width='70' height='70' src='images/user-im.png'></div><div class='user_content' style='float:right;'><div class='user_name'>" + w + "<span>【" + t + "】</span></div><div class='user_con'>" + s + "</div><div class='arrow2' style='float:right;'></div></div></div><div class='clearfix'></div>";
}
function js(msg) {
    return "<div class='an_tip' style='margin-bottom:10px;'><div class='an_info'>" + msg + "</div></div><div class='clearfix'></div>";
}


function BQIMGClick(eve) {
    eve.stopPropagation();
    eve.preventDefault();
    if (BQPop == null) {
        $('#divAnswer').BQPop({ left: 10, bottom: 8, isAgent: false });
        BQPop = $('#BQMain');
    }
    BQPop.toggle();
    return false;
}
//表情按钮：
function BQClick(url, e) {

    $('#Smessage').append('<img src="' + url + '" />').focus();
    BQPop.hide();
    $("#Smessage").focus();
    moveEnd("Smessage");
}