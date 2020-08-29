// JavaScript Document

Selection = function(element,panel){
	
		if(element==null || panel==null){
			alert("params error");return ;
		}
		panel.style.display = "none";
		var timer = {};
		var createTimer = function(){
			if(panel.style.display == "block"){
				var id = panel.id;
				timer = setTimeout("document.getElementById('"+id+"').style.display='none'", 100);
			}
		};
		var clearTimer = function(){
			try{window.clearTimeout(timer);}catch(e){alert(e)}
		};

		element.onmouseover = function(){ panel.style.display = "block";clearTimer();}
		element.onmouseout = createTimer;
		
};

