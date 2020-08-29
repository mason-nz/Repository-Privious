/*
    ES5中新增了写数组方法，如下：
    forEach (js v1.6)
    map (js v1.6)
    filter (js v1.6)
    some (js v1.6)
    every (js v1.6)
    indexOf (js v1.6)
    lastIndexOf (js v1.6)
    reduce (js v1.8)
    reduceRight (js v1.8)

    浏览器支持
    Opera 11+
    Firefox 3.6+
    Safari 5+
    Chrome 8+
    Internet Explorer 9+
    
    所以对于IE6-IE8浏览器，Array原型扩展可以实现以上全部功能
 */


if (typeof Array.prototype.forEach != "function") {
    Array.prototype.forEach = function (fn, context) {
        for (var k = 0, length = this.length; k < length; k++) {
            if (typeof fn === "function" && Object.prototype.hasOwnProperty.call(this, k)) {
                fn.call(context, this[k], k, this);
            }
        }
    };
}

if (typeof Array.prototype.map != "function") {
    Array.prototype.map = function (fn, context) {
        var arr = [];
        if (typeof fn === "function") {
            for (var k = 0, length = this.length; k < length; k++) {
                arr.push(fn.call(context, this[k], k, this));
          }
        }
        return arr;
  };
}

if (typeof Array.prototype.filter != "function") {
    Array.prototype.filter = function (fn, context) {
        var arr = [];
        if (typeof fn === "function") {
          for (var k = 0, length = this.length; k < length; k++) {
              fn.call(context, this[k], k, this) && arr.push(this[k]);
          }
        }
        return arr;
  };
}

if (typeof Array.prototype.some != "function") {
    Array.prototype.some = function (fn, context) {
        var passed = false;
        if (typeof fn === "function") {
            for (var k = 0, length = this.length; k < length; k++) {
              if (passed === true) break;
              passed = !!fn.call(context, this[k], k, this);
            }
        }
        return passed;
  };
}

if (typeof Array.prototype.every != "function") {
    Array.prototype.every = function (fn, context) {
        var passed = true;
        if (typeof fn === "function") {
            for (var k = 0, length = this.length; k < length; k++) {
                if (passed === false) break;
                passed = !!fn.call(context, this[k], k, this);
            }
        }
        return passed;
  };
}

if (typeof Array.prototype.indexOf != "function") {
    Array.prototype.indexOf = function (searchElement, fromIndex) {
        var index = -1;
        fromIndex = fromIndex * 1 || 0;

        for (var k = 0, length = this.length; k < length; k++) {
          if (k >= fromIndex && this[k] === searchElement) {
              index = k;
              break;
          }
        }
        return index;
  };
}

if (typeof Array.prototype.lastIndexOf != "function") {
    Array.prototype.lastIndexOf = function (searchElement, fromIndex) {
        var index = -1, length = this.length;
        fromIndex = fromIndex * 1 || length - 1;

        for (var k = length - 1; k > -1; k-=1) {
            if (k <= fromIndex && this[k] === searchElement) {
                index = k;
                break;
            }
        }
        return index;
  };
}

if (typeof Array.prototype.reduce != "function") {
    Array.prototype.reduce = function (callback, initialValue ) {
        var previous = initialValue, k = 0, length = this.length;
        if (typeof initialValue === "undefined") {
            previous = this[0];
            k = 1;
        }
         
        if (typeof callback === "function") {
          for (k; k < length; k++) {
            this.hasOwnProperty(k) && (previous = callback(previous, this[k], k, this));
          }
        }
        return previous;
    };
}

if (typeof Array.prototype.reduceRight != "function") {
    Array.prototype.reduceRight = function (callback, initialValue ) {
        var length = this.length, k = length - 1, previous = initialValue;
        if (typeof initialValue === "undefined") {
            previous = this[length - 1];
            k--;
        }
        if (typeof callback === "function") {
           for (k; k > -1; k-=1) {          
              this.hasOwnProperty(k) && (previous = callback(previous, this[k], k, this));
           }
        }
        return previous;
    };
}


/*---------------------end-----------------*/



//解决IE10以下不支持Function.bind
if (!Function.prototype.bind) {
    Function.prototype.bind = function(oThis) {
        if (typeof this !== "function") {
            throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable");
        }
        var aArgs = Array.prototype.slice.call(arguments, 1),
            fToBind = this,
            fNOP = function() {},
            fBound = function() {
                return fToBind.apply(this instanceof fNOP && oThis ? this : oThis,
                    aArgs.concat(Array.prototype.slice.call(arguments)));
            };
        fNOP.prototype = this.prototype;
        fBound.prototype = new fNOP();
        return fBound;
    };
}



/**
* forEach遍历数组
* @param callback [function] 回调函数；
* @param context [object] 上下文；
*/
Array.prototype.myForEach = function myForEach(callback,context){
    context = context || window;
    if('forEach' in Array.prototye) {
        this.forEach(callback,context);
        return;
    }
    //IE6-8下自己编写回调函数执行的逻辑
    for(var i = 0,len = this.length; i < len;i++) {
        callback && callback.call(context,this[i],i,this);
    }
};



/**
* map遍历数组
* @param callback [function] 回调函数；
* @param context [object] 上下文；
*/
Array.prototype.myMap = function myMap(callback,context){
    context = context || window;
    if('map' in Array.prototye) {
        return this.map(callback,context);
    }
    //IE6-8下自己编写回调函数执行的逻辑
    var newAry = [];
    for(var i = 0,len = this.length; i < len;i++) {
        if(typeof  callback === 'function') {
            var val = callback.call(context,this[i],i,this);
            newAry[newAry.length] = val;
        }
    }
    return newAry;
};








