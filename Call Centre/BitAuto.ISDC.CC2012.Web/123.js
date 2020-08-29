(function(){})();


var $= jqlite = function(selector){
	var selector=selector.slice(1);
	var currentElement=document.getElementById(selector);

	return {
		el:currentElement,
		hide:function(){
			this.el.style['display']='none';
		},
		show:function(){
			this.el.style['display']='block';	
		},
		getStyle:function(name){
			if(this.el.style[name])
				return this.el.style[name];
			if (this.el.currentSytle && this.el.currentSytle[name])
				return this.el.currentSytle[name];
			else

			return null
		},
		reset:function(objects){
			var oldStyle={};
			for (var p in objects) {
				oldStyle[p]=objects[p];
				this.el.style[p]=objects[p];
			};
			return oldStyle;
		},
		getHeight:function(){
			return parseInt(this.getStyle('height'));
		},

		slidedown:function(){
			var old = this.reset({
				display:"block",
				vidibility:'hidden'
			});

			var h=this.getHeight();
			this.reset(old);
			this.reset({
				height:0,
				overflow:'auto'
			});

			var that = this;

			for (var i = 0; i <=100; i++) {
				(function(m){
					setTimeout(function(){
						that.reset({
							height:m/100*h+'px'
						});
					},i*10);
				})(i);			
			};		
		}
	}
}

