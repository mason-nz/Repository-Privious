@echo======================开始安装服务===========================
%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe BitAtuo.DSC.IM_2015.UserMessageMail.Service.exe
@echo======================安装服务结束===========================

@echo======================开始启动服务===========================
net start BitAuto.ISDC.CC2012.NoDealerOrder.Service
@echo======================启动服务结束===========================
@pause 
