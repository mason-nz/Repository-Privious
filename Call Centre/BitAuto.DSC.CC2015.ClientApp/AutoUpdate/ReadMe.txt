自动升级程序说明：

	1、此自动升级程序可以用做任何Winform程序的自动升级
	2、此程序有三个文件：AutoUpdate.exe、ICSharpCode.SharpZipLib.dll、 AutoUpdate.xml，其中UserConfig.xml是配置文件


自动升级程序部署说明：
  HTTP：
	1、配置好 AutoUpdate.xml 文件（配置说明可见下面描述）
	2、将 AutoUpdate.exe 和 AutoUpdate.xml 打包到主程序的根目录下
	3、使得启动 AutoUpdate.exe 为第一个启动的程序，AutoUpdate.exe 完成检测和升级后会自动启动主程序。
	4、HTTP版本自动升级程序需要配备一个HTTP服务器Web站点，要求在服务器down目录下有一个配置文件，名为：config.xml,HTTP服务器上存放要升级文件(路径和客户端配置中的一致)，AutoUpdate.exe会根据配置文件的参数自动连接到HTTP,对比本地和服务器的版本号，
	   如果发现本地的版本和服务上的不一样，则下载最新文件。下载完成后，根据配置文件，启动主程序。


自动升级程序本地配置文件的说明：

  HTTP:
    1、UpdateFilePath：Web站点上存放升级文件的文件夹名称
	2、BaseURL：Web站点的基本目录URL
	3、StartApp：检测和升级后，要启动的主程序路径和名称
	4、Versions：本地程序的版本号


自动升级程序升级操作说明：
  HTTP:
    1、将需要升级的文件打包(命名为update.zip),放到Web站点下的down目录下。
	2、修改服务器端配置文件 config.xml 中的版本号，版本号加1
	3、各个客户端重新打开一次，即可自动更新。