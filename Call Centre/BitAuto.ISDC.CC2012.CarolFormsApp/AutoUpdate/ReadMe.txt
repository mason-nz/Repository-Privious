软件安装说明：
	解压文件之后，双击目录中的“安装此文件.bat”这个文件。程序会自动安装，在过程中，需要用户选择“yes”或选择路径等。
	安装成功后，桌面会创建图表“客户呼叫中心坐席客户端”，之后双击这图标打开应用程序。
	

自动升级程序说明：

	1、此自动升级程序可以用做任何Winform程序的自动升级
	2、此程序有三个文件：AutoUpdate.exe、ICSharpCode.SharpZipLib.dll、 AutoUpdate.xml，其中UserConfig.xml是配置文件
	3、此自动升级文件有两个版本:FTP方式、HTTP方式


自动升级程序部署说明：
  FTP版本：
	 
	1、配置好 AutoUpdate.xml 文件（配置说明可见下面描述）
	2、将 AutoUpdate.exe 和 AutoUpdate.xml 打包到主程序的根目录下
	3、使得启动 AutoUpdate.exe 为第一个启动的程序，AutoUpdate.exe 完成检测和升级后会自动启动主程序。
	4、FTP版本自动升级程序需要配备一个FTP服务器，要求在服务器根目录下有一个配置文件，名为：config.xml,FTP服务器上存放要升级文件，AutoUpdate.exe会根据配置文件的参数自动连接到FTP,对比本地和服务器的版本号，
	   如果发现本地的版本和FTP服务的不一样，则下载最新文件。下载完成后，根据配置文件，启动主程序。
  HTTP：
    和FTP版本类似，只有第4点不同：
	4、HTTP版本自动升级程序需要配备一个HTTP服务器Web站点，要求在服务器down目录下有一个配置文件，名为：config.xml,HTTP服务器上存放要升级文件(路径和客户端配置中的一致)，AutoUpdate.exe会根据配置文件的参数自动连接到HTTP,对比本地和服务器的版本号，
	   如果发现本地的版本和服务上的不一样，则下载最新文件。下载完成后，根据配置文件，启动主程序。


自动升级程序本地配置文件的说明：
  FTP：
	1、RemoteHost: FTP服务器的IP
	2、RemotePort：FTP服务的端口号
	3、RemoteUser：登录FTP服务器的用户名
	4、RemotePass：登录FTP服务器的密码
	5、UpdateFilePath：FTP服务器上存放升级文件的文件夹名称
	6、StartApp ：检测和升级后，要启动的主程序路径和名称
	7、Versions：本地程序的版本号

  HTTP:
    1、UpdateFilePath：Web站点上存放升级文件的文件夹名称
	2、BaseURL：Web站点的基本目录URL
	3、StartApp：检测和升级后，要启动的主程序路径和名称
	4、Versions：本地程序的版本号


自动升级程序升级操作说明：
  FTP:
	1、将需要升级的文件放到服务器的指定文件夹（文件夹名称要与客户端配置文件中的UpdateFilePath值一致）下。
	2、修改服务器端配置文件 config.xml 中的版本号，版本号加1
	3、各个客户端重新打开一次，即可自动更新。
  HTTP:
    1、将需要升级的文件打包(命名为update.zip),放到Web站点下的down目录下。
	2、修改服务器端配置文件 config.xml 中的版本号，版本号加1
	3、各个客户端重新打开一次，即可自动更新。