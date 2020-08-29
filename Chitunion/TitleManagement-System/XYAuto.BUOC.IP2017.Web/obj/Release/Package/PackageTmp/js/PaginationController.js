//if (!Function.prototype.bind) {
//    Function.prototype.bind = function (oThis) {
//        if (typeof this !== "function") {
//            throw new TypeError("Function.prototype.bind - what is trying to be bound is not callable");
//        }
//        var aArgs = Array.prototype.slice.call(arguments, 1), fToBind = this, fNOP = function () { },
//                    fBound = function () {
//                        return fToBind.apply(this instanceof fNOP && oThis ? this : oThis, aArgs.concat(Array.prototype.slice.call(arguments)));
//                    };
//        fNOP.prototype = this.prototype;
//        fBound.prototype = new fNOP();
//        return fBound;
//    };
//}

//if (!Array.prototype.forEach) {

//    Array.prototype.forEach = function forEach(callback, thisArg) {

//        var T, k;

//        if (this == null) {
//            throw new TypeError("this is null or not defined");
//        }
//        var O = Object(this);
//        var len = O.length >>> 0;
//        if (typeof callback !== "function") {
//            throw new TypeError(callback + " is not a function");
//        }
//        if (arguments.length > 1) {
//            T = thisArg;
//        }
//        k = 0;

//        while (k < len) {

//            var kValue;
//            if (k in O) {

//                kValue = O[k];
//                callback.call(T, kValue, k, O);
//            }
//            k++;
//        }
//    };
//}

/**
* Created by luq on 2017/2/24.
*
* 使用说明：
* 1，使用时需要进行对象的实例化，里边可以传入一个对象参数，
* 例如：{
*      WrapContainer:"#pageContainer",//页码父包裹层的id值。
*      DisabledClassName:".disabled",//不可点按钮的class名，可选
*      CurrentClassName:".current",//选中状态按钮class名，可选
*      EnableClickClassName:".EnableClick",//可点击按钮class名，可选
*      NormalTextClassName:".NormalText",//普通文本class名，可选，
*      MaxPage:150,//最大数据条数，
*      PageItemCount:10,//定义每一页展现的数据条数，
*      ControllerCount:7,//定义可点击的页码按钮数量，
*      CallBack:function(currentPageIndex,callback){
*          console.log(currentPageIndex);//获取当前页码
*          callback(true);//或者callback(false);
*
*          //此部分中的callback为控件传入的回调函数，此部分内可以进行ajax
*          请求，成功之后执行callback(true);失败执行callback(false);
*
*      }
* }
* 2，实例化之后，要执行一下createPageItemFu(num)函数，传值num为数字，定义了默认渲染第几页。例如：

* var NewPage = new PaginationController({
WrapContainer:"#pageContainer",
MaxPage:150,
PageItemCount:10,
ControllerCount:7,
CallBack:function(currentPageIndex,callback){
console.log(currentPageIndex);
callback(true);
}
});
NewPage.createPageItemFu(1)
*
*
*
*
*
*/
function PaginationController() {
    this.options = arguments[0];
    /*不可点击按钮*/
    this.disabledBtn = $(this.options.WrapContainer).find(this.options.DisabledClassName)[0] ? $(this.options.WrapContainer).find(this.options.DisabledClassName)[0] : '<span class="disabled">&lt</span>';
    /*当前选中按钮*/
    this.currentBtn = $(this.options.WrapContainer).find(this.options.CurrentClassName)[0] ? $(this.options.WrapContainer).find(this.options.CurrentClassName)[0] : '<span class="current">1</span>';
    /*可点击按钮*/
    this.EnableClickBtn = $(this.options.WrapContainer).find(this.options.EnableClickClassName)[0] ? $(this.options.WrapContainer).find(this.options.EnableClickClassName)[0] : '<a href="" class="EnableClick">2</a>';
    /*普通文本*/
    this.NormalTextBtn = $(this.options.WrapContainer).find(this.options.NormalTextClassName)[0] ? $(this.options.WrapContainer).find(this.options.NormalTextClassName)[0] : '<span class="NormalText">...</span>';
    /*************************************/
    /*计算一共有多少页*/
    this.pageCount = Math.ceil(this.options.MaxPage / this.options.PageItemCount);
    /*当前选定状态页码*/
    this.CurrentPageCount = 1;

    /*默认展示页码数组*/
    this.displayArray = function () {
        var array = [];
        for (var i = 0; i < this.pageCount; i++) {
            array.push(i + 1);
        }
        if (array.length > parseInt(this.options.ControllerCount)) {
            return array.slice(0, parseInt(this.options.ControllerCount));
        } else {
            return array;
        }
    } .bind(this)();

    /*向前跳临界数值*/
    this.jumpUp = this.displayArray[0];
    /*向后跳临界数值*/
    this.jumpNext = this.displayArray[this.displayArray.length - 1];

    /*需要展现的页码的dom结构*/
    this.dispalyPageItem = [];
    /*修改默认展示页码数组*/
    this.ChangeDisplayArray2 = function (Tag) {
        switch (Tag) {
            case "next":
                this.CurrentPageCount++;
                this.options.CallBack(this.CurrentPageCount, function (ifSuccess) {
                    if (ifSuccess) {
                        if (this.jumpNext + 1 <= this.pageCount) {
                            var tempArray = this.displayArray;
                            tempArray.push(this.jumpNext + 1);
                            this.displayArray = [];
                            tempArray.forEach(function (item) {
                                if (item > this.jumpNext - (parseInt(this.options.ControllerCount) - 1)) {
                                    this.displayArray.push(item)
                                }
                            } .bind(this));
                        }
                        this.jumpUp = this.displayArray[0];
                        this.jumpNext = this.displayArray[this.displayArray.length - 1];
                        this.createPageItemFu(this.CurrentPageCount);
                    } else {
                        this.CurrentPageCount--;
                        alert("数据请求失败，请稍后重试。")
                    }
                } .bind(this));
                break;
            case "up":
                this.CurrentPageCount--;
                this.options.CallBack(this.CurrentPageCount, function (ifSuccess) {
                    if (ifSuccess) {
                        if (this.jumpUp > 1) {
                            var tempArray = this.displayArray;
                            tempArray.unshift(this.jumpUp - 1);
                            this.displayArray = [];
                            tempArray.forEach(function (item) {
                                if (item < this.jumpUp + parseInt(this.options.ControllerCount) - 1) {
                                    this.displayArray.push(item)
                                }
                            } .bind(this));
                        }

                        this.jumpUp = this.displayArray[0];
                        this.jumpNext = this.displayArray[this.displayArray.length - 1];
                        this.createPageItemFu(this.CurrentPageCount);
                    } else {
                        this.CurrentPageCount++;
                        alert("数据加载失败，请稍后重试。")
                    }
                } .bind(this));

                break;
            default:
                break;
        }
    };
    this.ChangeDisplayArray = function (Tag, clickCount) {
        this.CurrentPageCount = clickCount;
        /**/
        this.options.CallBack(this.CurrentPageCount, function (ifSuccess) {
            if (ifSuccess) {
                if (Tag == "next") {
                    if (parseInt(clickCount) + 1 <= this.pageCount) {
                        var tempArray = this.displayArray;
                        tempArray.push(parseInt(clickCount) + 1);
                        this.displayArray = [];
                        tempArray.forEach(function (item) {
                            if (item > parseInt(clickCount) - (parseInt(this.options.ControllerCount) - 1)) {
                                this.displayArray.push(item)
                            }
                        } .bind(this));
                    }
                } else if (Tag == "up") {
                    if (parseInt(clickCount) > 1) {
                        var tempArray = this.displayArray;
                        tempArray.unshift(parseInt(clickCount) - 1);
                        this.displayArray = [];
                        tempArray.forEach(function (item) {
                            if (item < parseInt(clickCount) + parseInt(this.options.ControllerCount) - 1) {
                                this.displayArray.push(item)
                            }
                        } .bind(this));
                    }
                } else if (Tag == "jumpTop") {
                    var array = [];
                    for (var i = 0; i < parseInt(this.options.ControllerCount); i++) {
                        array.push(i + 1);
                    }
                    this.displayArray = array;
                } else if (Tag == "jumpBottom") {
                    this.displayArray = [];
                    for (var i = this.pageCount; i > this.pageCount - parseInt(this.options.ControllerCount); i--) {
                        this.displayArray.unshift(i);
                    }
                } else {

                }
                this.jumpUp = this.displayArray[0];
                this.jumpNext = this.displayArray[this.displayArray.length - 1];
                this.createPageItemFu(this.CurrentPageCount);
            } else {
                alert("数据请求失败，请稍后重试。")
            }
        } .bind(this));
        /**/

    };
    /*创造当前选中页码*/
    this.createCurrentPageItem = function (count) {
        return $(this.currentBtn).clone().text(count).off("click").on("click", function () {
            alert("这是当前页。")
        });
    };
    /*创造可点选页码*/
    this.createEnableClickItem = function (count) {
        return $(this.EnableClickBtn).clone().text(count).off("click").on("click", function (event) {
            event.preventDefault();
            if (event.target.textContent == this.jumpNext) {
                this.ChangeDisplayArray("next", event.target.textContent);
            } else if (event.target.textContent == this.jumpUp) {
                this.ChangeDisplayArray("up", event.target.textContent);
            } else if (event.target.textContent != 1 && event.target.textContent != this.pageCount) {
                this.ChangeDisplayArray("normal", event.target.textContent);
            }

            if (event.target.textContent < this.jumpUp) {
                this.ChangeDisplayArray("jumpTop", event.target.textContent);
            } else if (event.target.textContent > this.jumpNext) {
                this.ChangeDisplayArray("jumpBottom", event.target.textContent);
            }
        } .bind(this));
    };
    /*创造普通文本显示*/
    this.createNormalText = function (text) {
        return $(this.NormalTextBtn).clone().text(text);
    };
    /*创造下一页*/
    this.createNextBtn = function (ifDisable) {
        if (ifDisable) {
            return $(this.disabledBtn).clone().text(">").off("click").on("click", function () {
                alert("已经是最后一页了。");
            });
        }
        return $(this.EnableClickBtn).clone().text(">").off("click").on("click", function (event) {
            event.preventDefault();
            this.ChangeDisplayArray2("next");
        } .bind(this));
    };
    /*创造上一页*/
    this.createPreBtn = function (ifDisable) {
        if (ifDisable) {
            return $(this.disabledBtn).clone().text("<").off("click").on("click", function () {
                alert("已经是第一页了。");
            });
        }
        return $(this.EnableClickBtn).clone().text("<").off("click").on("click", function (event) {
            event.preventDefault();
            this.ChangeDisplayArray2("up");
        } .bind(this));
    };
    /*创造最后的提示文字*/
    this.createTipText = function (count) {
        if (this.options.MaxPage == 0) {
            return $(this.NormalTextBtn).clone().text("0" + "/" + this.pageCount + "，共有" + this.options.MaxPage + "条数据。");
        }
        return $(this.NormalTextBtn).clone().text(count + "/" + this.pageCount + "，共有" + this.options.MaxPage + "条数据。");
    };
    /*创造可见页码*/
    this.createPageItemFu = function (currentCount) {
        this.CurrentPageCount = currentCount;
        this.dispalyPageItem = [];
        this.displayArray.forEach(function (item, index) {
            if (index == 0 && item != 1 && this.pageCount > parseInt(this.options.ControllerCount)) {
                this.dispalyPageItem.push(this.createEnableClickItem(1));
                this.dispalyPageItem.push(this.createNormalText("..."));
            }
            if (item == currentCount) {
                this.dispalyPageItem.push(this.createCurrentPageItem(currentCount));
            } else {
                this.dispalyPageItem.push(this.createEnableClickItem(item));
            }
            if (index == parseInt(this.options.ControllerCount) - 1 && item < this.pageCount) {
                this.dispalyPageItem.push(this.createNormalText("..."));
                this.dispalyPageItem.push(this.createEnableClickItem(this.pageCount));
            }
        } .bind(this));
        if (this.CurrentPageCount == 1) {
            this.dispalyPageItem.unshift(this.createPreBtn(true));
        } else {
            this.dispalyPageItem.unshift(this.createPreBtn(false));
        }
        if (this.CurrentPageCount == this.pageCount || this.options.MaxPage == 0) {
            this.dispalyPageItem.push(this.createNextBtn(true));
        } else {
            this.dispalyPageItem.push(this.createNextBtn(false));
        }
        this.dispalyPageItem.push(this.createTipText(this.CurrentPageCount));
        $(this.options.WrapContainer).html("").html(this.dispalyPageItem);
    } .bind(this);
}