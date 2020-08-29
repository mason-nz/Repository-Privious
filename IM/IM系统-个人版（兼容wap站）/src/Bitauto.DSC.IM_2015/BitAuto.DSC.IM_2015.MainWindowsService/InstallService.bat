@echo======================Install Services Start===========================
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe BitAuto.DSC.IM_2015.MainWindowsService.exe
@echo======================Install Services End===========================

@echo======================Trying Start Services Start===========================
net start BitAuto.DSC.IM_2015.MainWindowsService
@echo======================Services Started===========================
@pause 
