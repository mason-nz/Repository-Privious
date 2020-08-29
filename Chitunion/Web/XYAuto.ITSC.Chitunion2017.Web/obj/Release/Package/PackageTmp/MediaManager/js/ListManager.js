/**
 * Created by luq on 2017/2/27.
 */
function ComponentSection(Data){
    this.State = {};
    /*获取出事数据源，一般为从后台返回的数据对象。*/
    this.getInitialState = function(){
        for(var i in Data){
            this.State[i] = Data[i];
        }
    }.bind(this)();
}
ComponentSection.prototype.addSelfState = function(object){
    for(var i in object){
        this.State[i] = object[i];
    }
};
ComponentSection.prototype.setState = function(object,fn){
    for(var i in object){
        this.State[i] = object[i];
    }
    if(fn){
        fn();
    }
};
/**
 *ejs初始化渲染函数，需要三个参数，
 * source:是指template模板的id(需要带#号)
 * target:是指渲染地的id(需要带#号)
 * Data:数据源（一般为对象）
 * */
ComponentSection.prototype.Render = function(source,target,fn){
    var Source = $(source).html();
    var Result = ejs.render(Source,this.State);
    $(target).html(Result);
    if(fn){
        fn();
    }
    Source = null;
    Result = null;
};