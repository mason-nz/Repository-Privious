/*
    新增的时候没有参数
    编辑的时候参数为GroupID
 */

$(function(){

 
    $('html,body').animate({scrollTop:-40},50);

	var config = {};
    var GroupID = GetQueryString('GroupID')!=null&&GetQueryString('GroupID')!='undefined'?parseFloat  (GetQueryString('GroupID')):null;


    if(GroupID == null){//新建的时候
        GroupID = 0;
        $('.all_tit').find('.edit').text('新建图文');
    }else{
        $('.all_tit').find('.edit').text('编辑图文');
    }

    var flag;
    
    window.onbeforeunload = function (e) {
        e = e || window.event;
          // 兼容IE8和Firefox 4之前的版本
        if (e) {
           e.returnValue = '关闭提示';
        }
        // Chrome, Safari, Firefox 4+, Opera 12+ , IE 9+
        return '关闭提示';
    };


    var InitImgtxtMessage = {};

    var initEdit = {
        construtor : initEdit,
        pageRendering : function(){//初始化渲染页面
            //默认添加 和编辑时候的渲染
            setAjax({
                url : '/api/WeChatEditor/SelectGroupArticlesByGroupID',
                type : 'get',
                data : {
                    'GroupID' : GroupID
                }
            },function(data){
                var Result = data.Result;
                if(data.Status == 0){
                    $('.newly_build_left .content').html(ejs.render($('#Template-GroupDateArr').html(),Result));
                    InitImgtxtMessage = data.Result.ArticleList[0];
                    initEdit.leftMenuDetail();//左侧菜单
                    initEdit.middleTab();//中间的tab切换
                    initEdit.rightEditBar();//右侧编辑器
                }else{
                    layer.msg(Result,{'time':1000});
                }
            })
        },
        leftMenuDetail : function(){

            if(GroupID != null){//编辑的时候  
                initEdit.mouseoverToShow($('.Each_imgTxt'));//鼠标滑过事件
                //编辑的时候默认把第一个的信息 展示出来  默认点击一次
                $('.cover>img').attr('src',InitImgtxtMessage.CoverPicUrl);//封面图
                var first_border = $('.newly_build_left').find('.Each_imgTxt').eq(0);
                setTimeout(function() {
                    first_border.click();
                },100);
                
                //删除图文
                $('.Each_imgTxt .operation .delete_btn').off('click').on('click',function(){
                    var that = $(this);
                    var idx = that.parents('.Each_imgTxt').index();
                    layer.confirm('你确定要删除该图文吗',{
                        btn : ['删除','不删除']
                    },function(){
                        layer.closeAll();
                        that.parents('.Each_imgTxt').remove();
                        setTimeout(function() {
                            first_border.click();
                        },100);
                    })
                })
            }


            AlwaysFirst();
            initEdit.changeEachImgTxt();//切换并展示信息

            var cur_idx = 0;//获取当前选中的图文的索引
            $('.content .Each_imgTxt').eq(cur_idx).addClass('first_border');//默认单图文加样式
            
            //获取带红色边框的选中的元素   第一个元素
            var first_border = $('.newly_build_left').find('.Each_imgTxt').eq(0);
            first_border.on('mouseover',function(){
                $(this).find('.operation').show();
            }).on('mouseleave',function(){
                $(this).find('.operation').hide();
            })

            var figure_len = $('.content .Each_imgTxt').length;//图文长度
           
            //添加图文
            $('.newly_build_left .add>img').off('click').on('click',function () {
                var that = $(this);
                var cur_content = that.parent().prev('.content');

                if(figure_len < 8){
                    var single_figure = cur_content.find('.Each_imgTxt:eq(0)');
                    figure_len = cur_content.find('.Each_imgTxt').length;//克隆之前长度
                    cur_content.append(single_figure.clone());//复制

                    //当前的具体的元素
                    var cur_txt = cur_content.find('.Each_imgTxt').eq(figure_len);
                    setTimeout(function() {
                        cur_txt.click();
                    },100);
                    cur_txt.addClass('first_border').siblings().removeClass('first_border');
                    cur_txt.find('.fist_txt').hide();
                    cur_txt.find('.no_first').show();
                    cur_txt.find('.operation').show();//鼠标滑过样式
                    cur_txt.find('.headline_txt').text('标题');
                    cur_txt.find('.headline_img').attr('src','/uploadfiles/Article/CoverDefaultPic.png');

                    cur_txt.attr('Title','');
                    cur_txt.attr('Author','');
                    cur_txt.attr('CoverPicUrl','');
                    cur_txt.attr('Abstract','');
                    cur_txt.attr('OriginalUrl','');
                    cur_txt.attr('Content','');
                    cur_txt.attr('ArticleID',0);
                    

                    //新建一个图文  要清空之前填写的内容
                    $('.each_headline').val('');//标题为空
                    $('.each_author').val('');//作者为空
                    $('.cover>img').removeAttr('src');//封面图为空
                    $('.each_abstract').val('');//摘要为空
                    $('.each_link').val('');//原文链接为空
                    current_editor.setContent('');//编辑区域为空


                    initEdit.changeEachImgTxt();//每个图文互相切换的时候

                    first_border = cur_txt;
                    initEdit.mouseoverToShow(first_border);//鼠标滑过事件
                    AlwaysFirst();

                    //删除图文
                    $('.Each_imgTxt .operation .delete_btn').off('click').on('click',function(){
                        var that = $(this);
                        var idx = that.parents('.Each_imgTxt').index();
                        layer.confirm('你确定要删除该图文吗',{
                            btn : ['删除','不删除']
                        },function(){
                            layer.closeAll();
                            that.parents('.Each_imgTxt').remove();
                            setTimeout(function() {
                                $('.newly_build_left').find('.Each_imgTxt').eq(0).click();
                            },100);
                        })
                    })

                    figure_len = cur_content.find('.Each_imgTxt').length;//克隆之后长度
                }else{
                    layer.msg('最多只能填八个图文',{'time':1000});
                    //$('.text_me2').show();
                    //$('.text_me2').find('span').text('最多只能填八个图文');
                }
            })

            //保存文章  和一键同步
            initEdit.pressSafeBtn();
        },
        middleTab : function(){//中间的内容

            //默认显示第一个tab  和对应的内容
            var idx = 0;
            $('.tabContent').find('div.detail').eq(idx).show().siblings().hide();

            //tab切换
            $('.menu li').off('click').on('click',function(){
                var that = $(this);
                var idx = that.index();
                flag = false;
                $('.tabContent').find('div.detail').eq(idx).show().siblings().hide();
                that.addClass('current').siblings().removeClass('current');

                if(idx == 0){//常用
                    $('#edui14_sidebar').show();
                }else if(idx == 1){//图文库

                    $('#edui14_sidebar').hide();
                    //tab 切换
                    $('.imgTxt_tit li').off('click').on('click',function(){
                        var that = $(this);
                        var cur_idx = that.index();
                        that.addClass('current').siblings().removeClass('current');

                        $('.search_input').val('');//清楚搜索条件

                        if(cur_idx == 0){//图文素材
                            flag = false;
                        }else if(cur_idx == 1){//好文推荐
                            flag = true;
                        }

                        $('.search_imgTxt').click();//搜索
                    })

                    $('.search_imgTxt').on('click',function () {
                        initEdit.getFeedBackImgTxtData(flag,1);
                    })
                    $('.search_imgTxt').click();//默认的搜索事件

                }else if(idx == 2){//图库

                    $('#edui14_sidebar').hide();//隐藏编辑器的左侧
                    $('.editor_search').show();//搜索框显示

                    $('.search_photo').on('click',function () {
                        initEdit.getFeedBackImgData(1);
                    })
                    $('.search_photo').click();//默认触发搜索事件
                }
            })
        },
        rightEditBar : function(){//右侧的编辑区域
            //可视区域的宽度和高度
            var _width = document.documentElement.clientWidth;
            var _height = document.documentElement.clientHeight;

            $('.preview_box').css({'width':_width,'height':_height,'position':'fixed','left':0,'top':0,'background':'rgba(0,0,0,0.7)','z-index': '1111'});

            //点击预览的时候
            $('.look_txt').on('click',function () {
                var _html = current_editor.getContent();
                $('html,body').animate({scrollTop:0},100);
                $('.preview_box').show();
                $('.preview_box .content').html(_html);
                $('.exit_look').on('click',function () {
                    $('.preview_box').hide();
                })
            })

            //清空
            $('.empty_editor').on('click',function(){
                current_editor.setContent('');
            })
        },
        getFeedBackImgData : function (pageNum) {//获取图库的数据
            var url = '/api/WeChatEditor/SelectPictrues';
            var PicName = $('.editor_search .PicName').val();
            setAjax({
                url : url,
                type : 'get',
                data : {
                    PageIndex : pageNum,
                    PageSize : 10,
                    PicName : PicName,
                    WxID : -1
                }
            },function (data) {
                if(data.Status == 0){
                    var data = data.Result;
                    if(data.PicList.length > 0){
                        config.Img_TotalCount = data.TotalCount*1;//图库的总条数
                        $('#pageContainer').show();
                        $('#pageContainer').find('.NormalText').hide();
                        $('.MapDepotContent').html(ejs.render($('#Template-MapDepot').html(),data));
                        initEdit.createPageController(pageNum,2);//分页

                        var init_content = current_editor.getContent();
                        current_editor.on('blur',function () {
                            init_content = current_editor.getContent();
                            current_editor.focus();
                            current_editor.blur();
                        })
                        //点击图片将图片追加进编辑器里面
                        $('.MapDepotContent ul>li>img').on('click',function(e){
                            var _src = $(this).attr('src');
                            var img_url ='<img src='+_src+'>';
                            init_content += img_url;
                            console.log(init_content)
                            //current_editor.setContent(init_content);
                            current_editor.execCommand('insertHtml',init_content);
                            current_editor.focus();
                            current_editor.blur();
                            
                            current_editor.on('blur',function () {
                                init_content = current_editor.getContent();
                                current_editor.focus();
                                current_editor.blur();
                            })
                        })
                        return;
                        //鼠标滑过效果
                        $('.MapDepotContent ul>li').on('mouseover',function(){
                            var that = $(this);
                            var _span = that.find('span');
                            _span.show();
                        }).on('mouseleave',function(){
                            var that = $(this);
                            var _span = that.find('span');
                            _span.hide();
                        })
                        //删除图片
                        $('.MapDepotContent ul>li>span').on('click',function(){
                            var that = $(this);
                            var PicIDArr = [];
                            PicIDArr.push($(this).attr('PicID'));
                            setAjax({
                                url : '/api/WeChatEditor/DeletePictruesByPicIDs',
                                type : 'post',
                                data : {
                                    PicIDs : PicIDArr
                                }
                            },function(data){
                                if(data.Status == 0){
                                    that.parents('ul').remove();
                                }else{
                                    layer.msg(data.Message,{'time':1000});
                                }
                            })
                        })
                    }else{
                        $('.MapDepotContent').html(ejs.render($('#Template-MapDepot').html(),data));
                        $('#pageContainer').hide();
                        $('#pageContainer').find('.NormalText').hide();
                        $('.MapDepotContent').find('.no_data').show();
                    }
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
        },
        getFeedBackImgTxtData : function (flag,pageNum) {//图文库获取数据
            var url = '/api/Article/GetWeixinArticleList';
            var Key = $('.search_input').val();
            setAjax({
                url : url,
                type : 'get',
                data : {
                    IsGoodTJ : flag,
                    Key : Key,
                    PageSize : 10,
                    PageIndex : pageNum
                }
            },function (data) {
                if(data.Status == 0){
                    var data = data.Result;
                    if(data.List.length > 0){
                        config.Img_TotalCount = data.TotalCount;
                        $('#pageContainer').show();
                        $('#pageContainer').find('.NormalText').hide();
                        $('.ImageTextDatabaseContent').html(ejs.render($('#Template-ImageTextDatabase').html(),data));
                        initEdit.createPageController(pageNum,1);//分页


                        if(flag == false){//图文素材  根据图文ID

                            $('.GraphicMaterial .text_list>p>a').on('click',function(){
                                var that = $(this);
                                var ArticleID = that.attr('ArticleID');
                                var content = '';//文章内容
                                var OriginalUrl = that.attr('OriginalUrl');
                                var Title = $.trim(that.text());
                                if(OriginalUrl == 'null' || OriginalUrl == ''){
                                    $('.each_link').val('');
                                }else{
                                    $('.each_link').val(OriginalUrl);
                                    $('.content .first_border').attr('OriginalUrl',OriginalUrl);
                                }
                                if(Title != '暂无'){
                                    $('.each_headline').val(Title);
                                    $('.content .first_border').find('.headline_txt').text(Title);
                                    $('.content .first_border').attr('Title',Title);
                                }

                                setAjax({
                                    url : '/api/WeChatEditor/SelectArticleInfoByArticleID',
                                    type : 'get',
                                    data : {
                                        'ArticleID' : ArticleID
                                    }
                                },function(data){
                                    if(data.Status == 0){
                                        content = data.Result.Content;
                                        if(current_editor.getContent() != ''){
                                            layer.confirm('你确定要覆盖编辑区域的内容吗',{
                                                btn : ['覆盖','不覆盖']
                                            },function(){
                                                layer.closeAll();
                                                current_editor.setContent(content);
                                                current_editor.focus();
                                                current_editor.blur();
                                            })
                                        }else{
                                            current_editor.setContent(content);
                                            current_editor.focus();
                                            current_editor.blur();
                                        }
                                    }else{
                                        layer.msg(data.Message);
                                    }
                                })
                            })

                            //查看原文跳转
                            $('.look_message').attr('href','javascript:;');
                            $('.look_message').removeAttr('target');
                            $('.look_message').off('click').on('click',function(){
                                var that = $(this);
                                var ArticleID = that.parents('.text_list').attr('ArticleID');
                                setAjax({
                                    url : '/api/Article/GetArticleView',
                                    type : 'get',
                                    data : {
                                        'OptType' : 1,
                                        'ArticleID' : ArticleID
                                    }
                                },function(data){
                                    if(data.Status == 0){
                                        //that.attr('href',data.Result);
                                        window.open(data.Result);
                                    }else{
                                        layer.msg(data.Message,{'time':1000});
                                    }
                                })
                            })

                        }else{//好文推荐
                            $('.GraphicMaterial .text_list>p>a').on('click',function(){
                                var that = $(this);
                                var OriginalUrl = that.attr('OriginalUrl');
                                var content = '';//文章内容
                                var Title = $.trim(that.text());
                                if(OriginalUrl == 'null' || OriginalUrl == ''){
                                    $('.each_link').val('');
                                }else{
                                    $('.each_link').val(OriginalUrl);
                                    $('.content .first_border').attr('OriginalUrl',OriginalUrl);
                                }
                                if(Title != '暂无'){
                                    $('.each_headline').val(Title);
                                    $('.content .first_border').find('.headline_txt').text(Title);
                                    $('.content .first_border').attr('Title',Title);
                                }
                                setAjax({
                                    url : '/api/WeChatEditor/SelectArticleByUrl',
                                    type : 'get',
                                    data : {
                                        'ImportUrl' : OriginalUrl
                                    }
                                },function(data){
                                    if(data.Status == 0){
                                        content = data.Result;
                                        if(current_editor.getContent() != ''){
                                            layer.confirm('你确定要覆盖编辑区域的内容吗',{
                                                btn : ['覆盖','不覆盖']
                                            },function(){
                                                layer.closeAll();
                                                current_editor.setContent(content);
                                                current_editor.focus();
                                                current_editor.blur();
                                            })
                                        }else{
                                            current_editor.setContent(content);
                                            current_editor.focus();
                                            current_editor.blur();
                                        }
                                    }else{
                                        layer.msg(data.Message);
                                    }
                                })
                            })
                        }
                    }else{//数据为0的时候
                        $('.ImageTextDatabaseContent').html(ejs.render($('#Template-ImageTextDatabase').html(),data));
                        $('.GraphicMaterial').find('.no_data').show();
                        $('#pageContainer').hide();
                        $('#pageContainer').find('.NormalText').hide();
                    }
                }else{
                    layer.msg(data.Message,{'time':1000});
                }
            })
        },
        createPageController : function (pageNum,i) {//分页显示
            if(i == 1){//图文库
                var counts = config.Img_TotalCount;
                $("#pageContainer").pagination(counts, {
                    isShowTotalMsg:false,
                    current_page: (pageNum ? pageNum : 1),
                    items_per_page: 10, //每页显示多少条记录（默认为10条）
                    callback: function (currPage) {
                        initEdit.getFeedBackImgTxtData(flag,currPage);                        
                    }
                })
            }else{//图库
                var counts = config.Img_TotalCount;
                $("#pageContainer").pagination(counts, {
                    isShowTotalMsg:false,
                    current_page: (pageNum ? pageNum : 1),
                    items_per_page: 10, //每页显示多少条记录（默认为10条）
                    callback: function (currPage) {
                        initEdit.getFeedBackImgData(currPage);                                          
                    }
                })
            }
        },
        addFillMessage : function (idx) {//输入内容  标题 摘要 作者
            
            var idx = idx;
            //标题  必填项
            $('.each_headline').on('blur',function () {
                var that = $(this);
                var val = that.val();
                if(val == ''){
                    that.next().show();
                }else{
                    that.next().hide();
                    $('.content .first_border').find('.headline_txt').text(val);
                    $('.content .first_border').attr('Title',val);
                }
            })
            //作者
            $('.each_author').on('blur',function () {
                var that = $(this);
                var val = that.val();
                $('.content .first_border').attr('Author',val);
            })
            
            //封面图
            uploadFile('upload_file',idx);

            //摘要
            $('.each_abstract').on('blur',function () {
                var that = $(this);
                var val = that.val();
                $('.content .first_border').attr('Abstract',val);
            })

            //文本链接
            $('.each_link').on('blur',function () {
                var that = $(this);
                var val = that.val();
                //var reg = /^([hH][tT]{2}[pP]:\/\/|[hH][tT]{2}[pP][sS]:\/\/)(([A-Za-z0-9-~]+)\.)+([A-Za-z0-9-~\/])+$/;
                var reg = /^.*http.*$/;
                if(!reg.test(val)){
                    that.next().show();
                }else{
                    that.next().hide();
                    $('.content .first_border').attr('OriginalUrl',val);
                }
            })

            //编辑器区域的文本内容
            current_editor.on('blur',function () {
                var con = current_editor.getContent();
                $('.content .first_border').attr('Content',con);
            })

        },
        changeEachImgTxt : function () {//每个单图文互相切换的时候

            $('.Each_imgTxt').on('click',function () {
                var that = $(this);
                var idx = that.index();
                that.addClass('first_border').siblings().removeClass('first_border');

                var Title = that.attr('Title');
                var Author = that.attr('Author');
                var CoverPicUrl = that.attr('CoverPicUrl');
                var Abstract = that.attr('Abstract');
                var OriginalUrl = that.attr('OriginalUrl');
                var Content = that.attr('Content');

                $('.each_headline').val(Title);
                $('.each_author').val(Author);
                $('.show_img').attr(CoverPicUrl);
                $('.each_abstract').val(Abstract);
                $('.each_link').val(OriginalUrl);
                current_editor.setContent(Content);

                initEdit.addFillMessage(idx);//切换的时候保存展示信息
            })

        },
        mouseoverToShow : function (cur) {//鼠标滑过滑出效果  同时点击上移下移按钮

            //鼠标滑过
            $(cur).on('mouseover',function () {                
                $(this).find('.operation').show();
            }).on('mouseleave',function () {
                $(this).find('.operation').hide();
            })
            
            cur.find('.imgTxt_up').on('click',function(){
                ToMoveUp($(this));
                AlwaysFirst();
            })
            cur.find('.imgTxt_down').on('click',function(){
                ToMoveDown($(this));
                AlwaysFirst();
            })
            
        },
        pressSafeBtn : function () {//保存 和一键同步  需要验证必填项是否填写

            //保存按钮保存文章  
            $('.save_article').on('click',function(){
                var sendData = {
                    'GroupID' : GroupID,
                    'IsCheck' : false,
                    'ArticleList' : []
                }
                $('.Each_imgTxt').each(function (i,v) {
                    var that = $(this);
                    var Title = that.attr('Title');
                    var Author = that.attr('Author');
                    var CoverPicUrl = that.attr('CoverPicUrl');
                    var Abstract = that.attr('Abstract');
                    var OriginalUrl = that.attr('OriginalUrl');
                    var Content = that.attr('Content');
                    var ArticleID = that.attr('ArticleID')*1;
                    if(CoverPicUrl == ''){
                        CoverPicUrl = '/uploadfiles/Article/CoverDefaultPic.png';
                    }

                    if(GroupID == 0){//新增
                        var obj = {
                            'ArticleID' : 0,
                            'Orderby' : i+1,
                            'Title' : Title,
                            'Author' : Author,
                            'CoverPicUrl' : CoverPicUrl,
                            'Abstract' : Abstract,
                            'OriginalUrl' : OriginalUrl,
                            'Content' : Content
                        }
                    }else{//编辑
                        var obj = {
                            'ArticleID' : ArticleID,
                            'Orderby' : i+1,
                            'Title' : Title,
                            'Author' : Author,
                            'CoverPicUrl' : CoverPicUrl,
                            'Abstract' : Abstract,
                            'OriginalUrl' : OriginalUrl,
                            'Content' : Content
                        }
                    } 
                    sendData.ArticleList.push(obj);
                })
                var url = ' /api/Article/ModifyArticle';
                setAjax({
                    url : url,
                    type : 'post',
                    data : sendData
                },function(data){
                    if(data.Status == 0){
                        GroupID = data.Result.GroupID;
                        layer.msg('保存成功',{'time':1000});
                    }else{
                        layer.msg(data.Message,{'time':1000});
                    }
                })
            })


            //一键同步  保存文章  同时校验必填项  跳转页面
            $('.one_clickSync').on('click',function () {

                var requiredArrs = [];//必填项  文章和内容的对象
                var sendData = {
                    'GroupID' : GroupID,
                    'IsCheck' : true,
                    'ArticleList' : []
                }
                var isflag = false;

                $('.Each_imgTxt').each(function (i,v) {
                    var that = $(this);
                    var Title = that.attr('Title');
                    var Author = that.attr('Author');
                    var CoverPicUrl = that.attr('CoverPicUrl');
                    var Abstract = that.attr('Abstract');
                    var OriginalUrl = that.attr('OriginalUrl');
                    var Content = that.attr('Content');
                    var ArticleID = that.attr('ArticleID')*1;
                    if(CoverPicUrl == ''){
                        CoverPicUrl = '/uploadfiles/Article/CoverDefaultPic.png';
                    }

                    //必填文章标题和内容
                    var requireObj = {
                        'Title' : Title,
                        //'Content' : reStrim(Content)
                        'Content' : Content
                    }
                    requiredArrs.push(requireObj);

                    if(GroupID == 0){//新增
                        var obj = {
                            'ArticleID' : 0,
                            'Orderby' : i+1,
                            'Title' : Title,
                            'Author' : Author,
                            'CoverPicUrl' : CoverPicUrl,
                            'Abstract' : Abstract,
                            'OriginalUrl' : OriginalUrl,
                            'Content' : Content
                        }
                    }else{//编辑
                        var obj = {
                            'ArticleID' : ArticleID,
                            'Orderby' : i+1,
                            'Title' : Title,
                            'Author' : Author,
                            'CoverPicUrl' : CoverPicUrl,
                            'Abstract' : Abstract,
                            'OriginalUrl' : OriginalUrl,
                            'Content' : Content
                        }
                    }
                    sendData.ArticleList.push(obj);
                })                
                //文章内只输入img图片是默认不算输入内容的  中文英文数字
                //var reg = /^([\u4e00-\u9fa5]|[a-z]|[A-Z]|[0-9]|\s)+$/;

                /*for(var i=requiredArrs.length-1;i>=0;i--){
                    if(requiredArrs[i].Title == '' ||  !(reg.test(requiredArrs[i].Content)) ){
                        if(requiredArrs[i].Title == ''){
                            layer.msg('请填写标题',{'time':1000,});
                            $('.content .Each_imgTxt').eq(i).addClass('first_border').siblings().removeClass('first_border');
                            $('.each_headline').val('');
                            $('.each_headline').focus();
                            isflag = false;
                        }else if( !(reg.test(requiredArrs[i].Content)) ){
                            layer.msg('请填写文章',{'time':1000,});
                            $('.content .Each_imgTxt').eq(i).addClass('first_border').siblings().removeClass('first_border');
                            $('.content .Each_imgTxt').eq(i).click();
                            isflag = false;
                        }
                    }
                    if( i == requiredArrs.length-1 && requiredArrs[i].Title != '' &&  (reg.test(requiredArrs[i].Content)) ){
                        isflag = true;
                    }
                }*/
                for(var i=requiredArrs.length-1;i>=0;i--){
                    if(requiredArrs[i].Title == '' ||  requiredArrs[i].Content == ''){
                        if(requiredArrs[i].Title == ''){
                            layer.msg('请填写标题',{'time':1000,});
                            $('.content .Each_imgTxt').eq(i).addClass('first_border').siblings().removeClass('first_border');
                            $('.each_headline').val('');
                            $('.each_headline').focus();
                            isflag = false;
                        }else if( requiredArrs[i].Content == '' ){
                            layer.msg('请填写文章',{'time':1000,});
                            $('.content .Each_imgTxt').eq(i).addClass('first_border').siblings().removeClass('first_border');
                            $('.content .Each_imgTxt').eq(i).click();
                            isflag = false;
                        }
                    }
                    if( i == requiredArrs.length-1 && requiredArrs[i].Title != '' &&  requiredArrs[i].Content != '' ){
                        isflag = true;
                    }
                }

                if(isflag == true){
                    var url = '/api/Article/ModifyArticle';
                    setAjax({
                        url : url,
                        type : 'post',
                        data : sendData
                    },function(data){
                        console.log(data);
                        if(data.Status == 0){
                            var GroupID = data.Result.GroupID;
                            window.onbeforeunload = null;
                            //进入同步历史
                            window.location = '/SouceManager/graphicpublishing.html?GroupID='+GroupID;
                        }else{
                            layer.msg(data.Message,{'time':1000});
                        }
                    })
                }

            })
        }
    }


    initEdit.pageRendering();

    function reStrim(vl){
        var reTag = /<(?:.|\s)*?>/g;
        var reTag1 = /\\pP|\\pS/g;
        var txt = vl.replace(reTag,"");
        var ls = txt.replace(/[\ |\~|\`|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\-|\_|\+|\=|\||\\|\[|\]|\{|\}|\;|\:|\"|\'|\,|\<|\.|\>|\/|\?|\，|\。|\？|\！|\、|\：|\ |\（|\）|\；|\”|\''|\‘|\’|\“|\；|\【|\】|\{|\}|\……|\──|\《|\》|\<|\>|\---]/g,"");
        var delete_tirm = ls.replace(/\s/g,"");
        return delete_tirm;
    }


    //上移
    function ToMoveUp(ele){
        var cur_parents = $(ele).parents('.Each_imgTxt');
        var prev_parents = cur_parents.prev();
        if(prev_parents.length == 0){
            layer.msg("第一行,想移啥？",{'time':1000});
            return;
        }else{
            prev_parents.before(cur_parents);
        }        
    };
    //下移
    function ToMoveDown(ele){
        var cur_parents = $(ele).parents('.Each_imgTxt');
        var next_parents = cur_parents.next();
        if(next_parents.length == 0){
            layer.msg("最后一行,想移啥？",{'time':1000});
            return;
        }else{
            next_parents.after(cur_parents);
        }
    };

    //第一个的默认显示
    function  AlwaysFirst(){
        var idx = $('.newly_build_left').find('.Each_imgTxt').length-1;
        var cur_txt = $('.newly_build_left').find('.Each_imgTxt');
        cur_txt.eq(0).find('.fist_txt').show();
        cur_txt.eq(0).find('.no_first').hide();
        cur_txt.eq(0).find('.delete_btn').hide();
        
        cur_txt.not(":eq("+0+")").find('.fist_txt').hide();
        cur_txt.not(":eq("+0+")").find('.no_first').show();
        cur_txt.not(":eq("+0+")").find('.delete_btn').show();

        cur_txt.eq(0).find('.imgTxt_up').hide();//第一个没有上移的按钮
        cur_txt.not(":eq("+0+")").find('.imgTxt_up').show();

        cur_txt.eq(idx).find('.imgTxt_down').hide();//最后一个没有上移的按钮
        cur_txt.not(":eq("+idx+")").find('.imgTxt_down').show();
    };


    //上传文件
    function uploadFile(id,idx) {
        jQuery.extend({
            evalJSON: function (strJson) {
                if ($.trim(strJson) == ''){
                    return '';
                }else{
                    return eval("(" + strJson + ")");
                }
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

        $('#'+id).uploadify({
            'swf': '/Js/uploadify.swf?_=' + Math.random(),
            'uploader': '/AjaxServers/UploadFile.ashx',
            'auto': true,
            'multi': false,
            'width': 25,
            'height': 25,
            'buttonImage':'/ImagesNew/plus_2.png',
            /*'formData': { Action: 'BatchImport', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')), IsGenSmallImage: 1 }, 存在缩略图大图的格式 */
            'formData': { Action: 'BatchImport', CarType: '', LoginCookiesContent: escapeStr(getCookie('ct-uinfo')) },
            'fileTypeDesc': '支持格式:xls,xlsx,jpg,jpeg,png.gif,zip,pdf,ppt,pptx,mp4',
            'fileTypeExts': '*.xls;*.xlsx;*.jpg;*.jpeg;*.png;*.gif;*.zip;*.pdf;*.ppt;*.pptx;*.mp4',
            'fileSizeLimit':'5MB',
            'queueSizeLimit': 1,
            'scriptAccess': 'always',
            'onQueueComplete': function (event, data) {},
            'fileCount':1,
            queueID:'imgShow',
            'scriptAccess': 'always',
            'overrideEvents' : [ 'onDialogClose'],
            'onQueueComplete': function (event, data) {},
            'onQueueFull': function () {
                layer.alert('您最多只能上传1个文件！');
                return false;
            },
            'onUploadSuccess': function (file, data, response) {
                if (response == true) {
                    var json = $.evalJSON(data);
                    $('.newly_build_left .Each_imgTxt').eq(idx).find('.headline_img').attr('src',json.Msg);
                    $('.newly_build_left .Each_imgTxt').eq(idx).attr('CoverPicUrl',json.Msg);
                }
            },
            'onProgress': function (event, queueID, fileObj, data) {},
            'onUploadError': function (event, queueID, fileObj, errorObj) {},
            'onSelectError':function(file, errorCode, errorMsg){}
        });
    };

    //获取url 地址参数方法
    function GetQueryString(name) {
        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if(r!=null)return unescape(r[2]); return null;
    };

	/*window.onbeforeunload = function(event){
	    //return '真的要离开吗';
	   //return (e || window.event).returnValue = '有信息未保存，确认离开？！';
	   event.returnValue = "确定离开当前页面吗？";
	}*/

	//原理很简单，就是在body的onbeforeunload事件绑定函数，代码如下：        
	/*document.body.onbeforeunload = function (event){
        var c = event || window.event;
        if (/webkit/.test(navigator.userAgent.toLowerCase())) {
            return "离开页面将导致数据丢失！";
        }else{
            c.returnValue = "离开页面将导致数据丢失！";
        }
    }*/
})