/**
 * Written by:     zhengxh
 * Created Date:   2018/2/7
 */
/*
 *
 * 作者：hansen Wu
 * 描述：自定义消息组件
 * 邮箱：840399345@qq.com
 *
 * ## 如有感兴趣的同学优化了本组件，希望这位同学可以把优化后的作品发邮件给我，让我也学习学习。
 *
 */

var Wu = function() {

    //判断是否可以删除组件
    this.canRemove = true;
    //loading 节点
    this.iosLoading = '<i class="wu-icon wu-loading"></i>';

    return;
}

/*
 * 把生成的组件添加到dom里
 *
 * ## 在vue里添加到实例vue的dom元素里
 * ## 其他根据自己业务添加到对应的元素里
 */
Wu.prototype.addDom = function(el) {
    //vue
    // document.querySelector('#app').appendChild(el);
    //普通页面
    document.body.appendChild(el);
}

/*
 *  按钮触碰状态
 */
Wu.prototype.hoverButton = function() {

    var i, len, btns = document.querySelectorAll('.wu-btn');

    for(i = 0, len = btns.length; i < len; i++) {

        btns[i].addEventListener('touchstart', function() {
            this.classList.add('wu-btn-hover')
        })
        btns[i].addEventListener('touchend', function() {
            this.classList.remove('wu-btn-hover')
        })
    }
}

/*
 * 显示loading
 * @param {string} title 要显示的说明文字，可不添加
 *
 */
Wu.prototype.showLoading = function(title) {

    var load = document.querySelector(".wu-toast");
    if(load) {
        load.parentNode.removeChild(load);
    };

    var $this = this;
    var n = title ? title : '';
    var el = document.createElement("div");
    var span = '<span class="wu-toast-content">' + n + '</span>';
    var h = '<div class="wu-mask-transparent"></div>' +
        '<div class="wu-toast-box wu-toast-loading">' +

        $this.iosLoading +
        (title ? span : '') +

        '</div>';

    el.innerHTML = h;
    el.className = 'wu-toast';

    wu.addDom(el);

    setTimeout(function() {
        el.classList.add('wu-animate-in')
    }, 0)

};

/*
 * 显示loading 背景和 logo
 */
Wu.prototype.showLoadingBg = function() {

    wu.hideToast();

    var $this = this;
    var el = document.createElement("div");
    var h = '<div class="wu-mask-transparent bg-white"></div>' +
        '<div class="wu-toast-box wu-toast-box-transparent">' +

        $this.iosLoading +

        '</div>';
    el.innerHTML = h;
    el.className = 'wu-toast';

    wu.addDom(el);

    setTimeout(function() {
        el.classList.add('wu-animate-in')
    }, 0)

    return false
};

/*
 *  隐藏 toast
 */
Wu.prototype.hideToast = function() {

    var el = document.querySelector('.wu-toast');
    var $this = this;

    if(el) {
        if($this.canRemove) {
            $this.canRemove = false;
            el.classList.add('wu-animate-out');
            el.addEventListener('webkitTransitionEnd', function() {
                el.remove();
                $this.canRemove = true;
            })
        }
    };
    return false
};

/*
 *  wu.showError(); 显示连接失败错误提示
 *  @param {string} title 要显示的说明文字，可不添加
 */
Wu.prototype.showError = function(title) {

    wu.hideToast();
    var t = title || '网络请求超时，轻触屏幕刷新';
    var el = document.createElement("div");
    el.innerHTML = '<div class="start-box-refresh"><div class="start-box-icon">' +
        '<i class="wu-icon icon-refresh"></i></div>' +
        '<p class="refresh-text">' + t + '<p>' +
        '</div>';
    el.className = 'wu-toast';

    wu.addDom(el);

    setTimeout(function() {
        el.classList.add('wu-animate-in')
    }, 0)

    document.querySelector(".start-box-refresh").addEventListener("click", function() {
        location.reload();
    })
    return false
};

/*
 *  显示toast
 *
 *  @param {object} obj 相关配置
 *
 *  wu.showToast({
		title:'操作成功',
		mask:false,
		icon:'icon-success',    // icon-success | icon-error | icon-info
		duration:3000
	});
 */
Wu.prototype.showToast = function(obj) {

    wu.hideToast();

    obj.title = obj.title || '';
    obj.mask = obj.mask ? true : false;
    obj.icon = obj.icon || '';
    obj.duration = obj.duration || 3000;
    var el = document.createElement("div")

    var mask = '<div class="wu-mask-transparent"></div>';
    var iconLoad = '<i class="wu-icon wu-icon-toast ' + obj.icon + '"></i>';
    var span = '<span class="wu-toast-content">' + obj.title + '</span>';

    var h = (obj.mask ? mask : '') +

        '<div class="wu-toast-box">' +

        (obj.icon ? iconLoad : '') +
        (obj.title ? span : '') +

        '</div>';

    el.innerHTML = h;
    el.className = 'wu-toast';

    wu.addDom(el);

    setTimeout(function() {
        el.classList.add('wu-animate-in')
    }, 0)

    //定时关掉
    setTimeout(function() {
        el.classList.add('wu-animate-out');
        el.addEventListener('webkitTransitionEnd', function() {
            el.remove()
        })
    }, obj.duration)

    return false
};

/*
 *  显示头部滑出的消息提示
 *
 *  @param {object} obj 相关配置
 *
 *  wu.showMessage({
		title: "输入错误",
		backgroundColor: 'rgba(255, 0, 0, 0.5)',  // #fff | rgb/rgba
		duration: 3000
	});
 */
Wu.prototype.showMessage = function(obj) {

    wu.hideToast();

    var is_el = document.querySelector('.wu-message');
    if(!is_el) {
        var el = document.createElement("div")
        var title = obj.title || '正在加载',
            top = obj.top || '0',
            duration = obj.duration || 3000,
            bgColor = obj.backgroundColor || 'rgba(17, 17, 17, 0.9)';

        var h = '<span class="wu-toast-content">' + title + '</span>';

        el.className = 'wu-message';
        el.style.backgroundColor = bgColor;
        el.innerHTML = h;

        wu.addDom(el);

        setTimeout(function() {
            el.style.top = top + "px"
        }, 0)

        setTimeout(function() {
            el.style.top = "-44px";
            el.addEventListener('webkitTransitionEnd', function() {
                this.remove()
            })
        }, duration)
    }

    return false
};

/*
 *  显示操作窗口
 *
 *  @param {object} ops 相关配置
 *
 *  wu.showDialog({
		title: 'Hello Wu-ui',
		content: '欢迎使用Wu-ui',
		showCancel: true,
		showInput: false,
		success: function (res) {              //res.val == "confirm ||cancel"  // res.inputVal == "输入框的值"
			if(res.val == "confirm") {
				wu.showToast({
					title: '点击了确定',
					duration:2500
				})
			}
			if(res.val == "cancel") {
				wu.showToast({
					title: '点击了取消'
				})
			}
		}
	})
 */
Wu.prototype.showDialog = function(ops) {

    var title = ops.title || '提示',
        desc = ops.content || '',
        showCancel = ops.showCancel ? true : false,
        showInput = ops.showInput ? true : false;

    var el = document.createElement("div");
    var content_text = '<div class="dialog-text">' + desc + '</div>';
    var content_input = '<div class="dialog-input"><input type="text" class="dialog-prompt" autofocus /></div>';

    var h = '<div class="wu-mask-transparent wu-mask-black"></div>' +
        '<div class="dialog-content">' +
        '<div class="dialog-title">' + title + '</div>' +
        '<div class="dialog-body">' +

        (showInput ? content_input : content_text) +

        '</div>' +
        '<div class="dialog-foot">' +
        '<a class="wu-btn dialog-btn-cancel" style="' + (showCancel ? '' : 'display: none') + '">取消</a>' +
        '<a class="wu-btn dialog-btn-confirm">确认</a>' +
        '</div></div>';

    el.className = 'wu-dialog';
    el.innerHTML = h;

    wu.addDom(el);
    wu.hoverButton();

    setTimeout(function() {
        el.classList.add('wu-animate-in')
    }, 0)

    document.body.addEventListener('click', dialogHandlerr, false) //监听按钮点击

    function dialogHandlerr(e) {

        var e = e || window.event;
        var target = e.target || e.srcElement;
        var cls = target.className;

        if(cls.indexOf('dialog-btn-confirm') >= 0) {

            //确认
            if(showInput) { //如果显示输入框

                var input_val = document.getElementsByClassName('dialog-prompt')[0].value;

                if(typeof ops.success == 'function') {
                    ops.success({
                        value: 'confirm',
                        inputValue: input_val
                    })
                }

            } else {

                if(typeof ops.success == 'function') {
                    ops.success({
                        value: 'confirm'
                    })
                }
            }
            removeDialog()

            return false
        }
        if(cls.indexOf('dialog-btn-cancel') >= 0) {

            //取消
            if(typeof ops.success == 'function') {
                ops.success({
                    value: 'cancel'
                })
            }
            removeDialog()

            return false
        }
    }

    function removeDialog() {

        var dialogView = document.querySelector('.wu-dialog');

        dialogView.classList.add('wu-animate-out');
        dialogView.addEventListener('webkitTransitionEnd', function() {
            this.remove()
        })
        document.body.removeEventListener('click', actionHandlerr, false)
    }

    return
}

/*
 *  显示底部弹出操作窗口
 *
 *  @param {object} ops 相关配置
 *
 *  wu.showAction({
		title :'示例标题',
		menuArray :[
			{ title: '示例菜单一', value: 'menu1', color: '' },
			{ title: '示例菜单二', value: 'menu2', color: 'blue'},
			{ title: 'Delete', value: 'delete', color: 'red'}      //如果是删除  value 必须是 "delete"
		],
		success: function (res) {}
	})
 */
Wu.prototype.showAction = function(ops) {

    var i, len, h = '';
    var title = ops.title || '',
        deleteText = ops.deleteText,
        menuArr = ops.menuArray || [];
    var el = document.createElement("div");

    h += '<div class="wu-mask-transparent wu-mask-black wu-mask-hide"></div>' +
        '<div class="wu-actionsheet-main">' +
        '<div class="wu-actionsheet-title">' + title + '</div>' +
        '<div class="wu-actionsheet-menu">';

    //遍历菜单
    if(menuArr.length) {
        for(i = 0, len = menuArr.length; i < len; i++) {
            h += '<div class="wu-btn wu-actionsheet-cell" data-value="' + menuArr[i].value + '" style="color: ' + menuArr[i].color + '">' + menuArr[i].title + '</div>'
        }
    }

    h += '</div><div class="wu-btn wu-actionsheet-cancel">取消</div></div>';

    el.className = "wu-actionsheet";
    el.innerHTML = h;

    setTimeout(function() {
        el.classList.add('wu-animate-in')
    }, 0)

    wu.addDom(el);
    wu.hoverButton();

    document.body.addEventListener('click', actionHandler, false);

    function actionHandler(e) {

        var e = e || window.event;
        var target = e.target || e.srcElement;
        var cls = target.className;

        if(cls.indexOf('wu-mask-hide') >= 0) {
            hideAction()
        }

        if(cls.indexOf('wu-actionsheet-cancel') >= 0) {
            hideAction()
        }

        if(cls.indexOf('wu-actionsheet-cell') >= 0) {

            var val = target.dataset.value,    //菜单显示的文本
                val_text = target.innerHTML;   //菜单data值

            if(val == "delete") {

                wu.showDialog({
                    title: '提示',
                    content: deleteText ? deleteText : '确定删除吗？',
                    showCancel: true,
                    success: function(res) {

                        if(res.value == "confirm") {
                            ops.success({
                                value: val,
                                title: val_text
                            })
                            hideAction()
                        }
                    }
                })
            } else {
                //传入参数
                if(typeof ops.success == "function") {

                    ops.success({
                        value: val,
                        title: val_text
                    })
                    hideAction()
                }
            }
        }

    }

    //隐藏弹出层
    function hideAction() {

        document.querySelector('.wu-actionsheet').classList.add('wu-animate-out');
        document.querySelector('.wu-actionsheet-main').addEventListener('webkitTransitionEnd', function() {
            document.querySelector('.wu-actionsheet').remove()
        })
        document.body.removeEventListener('click', actionHandler, false)
    }
    return
}