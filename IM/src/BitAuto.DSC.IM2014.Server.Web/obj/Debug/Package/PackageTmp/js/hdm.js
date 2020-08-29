// JavaScript Document
/*第一种形式 第二种形式 更换显示样式*/
function setTab(name,cursel,n){
 for(i=1;i<=n;i++){
  var menu=document.getElementById(name+i);
  var con=document.getElementById("con_"+name+"_"+i);
  menu.className=i==cursel?"hover":"";
  con.style.display=i==cursel?"block":"none";
 }
}
/*
for循环知道吧 
循环操作menu菜单对象咯 
我们知道每一个HTML元素都是一个DOM对象 
通过getElementById（对象ID）方法就可以获得这个DOM对象 

menu.className=i==cursel?"hover":""; 
意思是：设置MENU的样式(class="样式名"，CSS中定义)，条件是 

如果i==cursel则样式为"hover"，否则不加任何样式

con.style.display=i==cursel?"block":"none"; 
如果i==cursel则显示 加粗显示，否则不显示，即不可见
"block"是不是加粗显示，忘记了，自己看下CSS把
*/
