using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
namespace AutoUpdate
{
    /// <summary>   
    ///    
    /// FTP实用类   
    ///    
    /// Create By YY 2011-9-23   
    ///    
    /// 基于System.Net下的关于FTP的类库   
    /// 应用System.IO下的类库实现流操作   
    ///    
    /// </summary>   
    public class FTP
    {
        //FTP服务器地址   
        private string serverAddr;
        public string ServerAddr
        {
            get { return serverAddr; }
        }
        //FTP服务器端口号   
        private int serverPort;
        public int ServerPort
        {
            get { return serverPort; }
        }
        //登录名   
        private string userID;
        public string UserID
        {
            get { return userID; }
        }
        //登录密码   
        private string password;
        public string Password
        {
            get { return password; }
        }

        /// <summary>   
        /// FTP构造函数   
        /// </summary>   
        /// <param name="serverIP">FTP服务器地址</param>   
        /// <param name="serverPort">FTP服务器端口号，默认：21</param>   
        /// <param name="userID">登录用户名</param>   
        /// <param name="password">登录密码</param>   
        public FTP(string serverAddr, int serverPort, string userID, string password)
        {
            this.serverAddr = serverAddr;
            this.serverPort = serverPort != 0 ? serverPort : 21;
            this.userID = userID;
            this.password = password;
        }

        /// <summary>   
        /// FTP下载文件   
        /// </summary>   
        /// <param name="remoteFilePath">远程目录，以"/"分隔每一级目录。如果非空，以"/"结尾</param>   
        /// <param name="remoteFileName">远程文件名</param>   
        /// <param name="localFilePath">本地目录，以"\"分隔每一级目录。如果为空，下载到当前目录。如果非空，以"\"结尾</param>   
        /// <param name="localFileName">本地文件名，如果为空，则与远程文件同名</param>   
        /// <param name="info"></param>   
        /// <returns></returns>   
        public bool Download(string remoteFilePath, string remoteFileName, string localFilePath, string localFileName, out string info)
        {
            bool bRet = false;
            //如果本地文件名为空，则与远程文件同名   
            localFileName = localFileName != "" ? localFileName : remoteFileName;
            //初始化ftp目标   
            FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri(@"ftp://" + serverAddr + ":" + serverPort + "/" + remoteFilePath +"/"+ remoteFileName));
            //命令方法为下载   
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            //二进制方式传输，即传输文件   
            req.UseBinary = true;
            //登录凭证   
            req.Credentials = new NetworkCredential(this.userID, this.password);
            try
            {
                //发送命令请求至FTP服务器，并得到FtpWebResponse返回结果   
                FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
                //读取FtpWebResponse结果的返回数据流到Stream   
                using (Stream downloadStream = resp.GetResponseStream())
                {
                    //接受数据长度   
                    long length = resp.ContentLength;
                    //缓冲区大小为2048   
                    int bufferSize = 2048;
                    //每次读取到的字节数   
                    int readCount;
                    //缓冲区   
                    byte[] buffer = new byte[bufferSize];
                    //定义本地文件流   

                    if (!Directory.Exists(localFilePath))
                    {
                        Directory.CreateDirectory(localFilePath);
                    }

                    using (FileStream fileStream = new FileStream(localFilePath +"\\"+ localFileName, FileMode.Create))
                    {
                        //第一次读取最大为bufferSize字节的内容，内容装到buffer中，长度返回至readCount   
                        readCount = downloadStream.Read(buffer, 0, bufferSize);
                        //读取到内容   
                        while (readCount > 0)
                        {
                            //将内容写入文件流生成文件内容   
                            fileStream.Write(buffer, 0, readCount);
                            //继续读取保存返回数据的Stream   
                            readCount = downloadStream.Read(buffer, 0, bufferSize);
                        }
                    }
                }
                //操作结束，给附加信息info赋值。也可以用resp.StatusCode判断一下   
                info = "SUCCESS";
                bRet = true;
            }
            catch (Exception e)
            {
                //异常情况，返回异常信息   
                info = e.Message;
            }
            return bRet;
        }

        /// <summary>   
        /// FTP删除文件   
        /// </summary>   
        /// <param name="remoteFilePath">远程目录，以"/"分隔每一级目录。如果非空，以"/"结尾</param>   
        /// <param name="remoteFileName">远程文件名</param>   
        /// <param name="info"></param>   
        /// <returns></returns>   
        public bool Delete(string remoteFilePath, string remoteFileName, out string info)
        {
            bool bRet = false;
            FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + serverAddr + ":" + serverPort + "/" + remoteFilePath + remoteFileName));//初始化ftp目标   
            req.Method = WebRequestMethods.Ftp.DeleteFile;//下载方式   
            req.UseBinary = true;//二进制方式下载   
            req.Credentials = new NetworkCredential(this.userID, this.password);//登录凭证   
            try
            {
                FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
                //同Download方法，也可以用resp.StatusCode判断一下   
                info = "SUCCESS";
                bRet = true;
            }
            catch (Exception e)
            {
                info = e.Message;
            }
            return bRet;
        }

        /// <summary>   
        /// FTP上传文件   
        /// </summary>   
        /// <param name="remoteFilePath">远程目录，以"/"分隔每一级目录。如果非空，以"/"结尾</param>   
        /// <param name="remoteFileName">远程文件名，如果为空，则与本地文件同名</param>   
        /// <param name="localFilePath">本地目录，以"\"分隔每一级目录。如果为空，下载到当前目录。如果非空，以"\"结尾</param>   
        /// <param name="localFileName">本地文件名</param>   
        /// <param name="info"></param>   
        /// <returns></returns>   
        public bool Upload(string remoteFilePath, string remoteFileName, string localFilePath, string localFileName, out string info)
        {
            bool bRet = false;
            try
            {
                //加载本地文件   
                FileInfo fileInfo = new FileInfo(localFilePath + localFileName);
                //如果远程文件名为空，则与本地文件同名   
                remoteFileName = remoteFileName != "" ? remoteFileName : localFileName;
                FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + serverAddr + ":" + serverPort + "/" + remoteFilePath + remoteFileName));//初始化ftp目标   
                req.Method = WebRequestMethods.Ftp.UploadFile;//上传方式   
                req.UseBinary = true;//二进制方式上传，即上传文件   
                req.Credentials = new NetworkCredential(this.userID, this.password);//登录凭证   
                req.ContentLength = fileInfo.Length;
                int bufferSize = 2048;
                int length;
                byte[] buffer = new byte[bufferSize];
                //将文件流存在FileStream中   
                using (FileStream fs = fileInfo.OpenRead())
                {
                    //上传流指向Stream   
                    using (Stream stream = req.GetRequestStream())
                    {
                        //读取文件流中最大长度为bufferSize字节内容至buffer，读取长度返回至length   
                        length = fs.Read(buffer, 0, bufferSize);
                        //如果读取到了内容   
                        while (length != 0)
                        {
                            //buffer写入上传流   
                            stream.Write(buffer, 0, length);
                            //继续读取   
                            length = fs.Read(buffer, 0, bufferSize);
                        }
                        info = "SUCCESS";
                    }
                }
            }
            catch (Exception e)
            {
                info = e.Message;
            }
            return bRet;
        }

        /// <summary>
        /// 获取FTP项目列表（包括文件和文件夹）
        /// </summary>
        /// <param name="remoteFilePath">FTP上的路径</param>
        /// <returns></returns>
        public List<FtpItem> GetFileList(string remoteFilePath)
        {
             List<FtpItem> downloadFiles = new List<FtpItem>();
            FtpItem item;
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(@"ftp://" + serverAddr + ":" + serverPort + "/" + remoteFilePath));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(UserID, Password);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);

                string line = reader.ReadLine();
                while (line != null)
                {
                    item = new FtpItem();
                    item.ItemName = line.Substring(line.LastIndexOf(' ') + 1);

                    string temp = line.Substring(0, line.LastIndexOf(' ')).Trim();
                    item.ItemType = temp.Substring(temp.LastIndexOf(' ') + 1) == "<DIR>" ? 2 : 1;

                    downloadFiles.Add(item);

                    line = reader.ReadLine();
                }

                reader.Close();
                response.Close();

                return downloadFiles;
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                return downloadFiles;
            }
        }

        public int GetFileSize(string file)
        {
 
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://mysite.se/" + file));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(UserID, Password);
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                int dataLength = (int)request.GetResponse().ContentLength;

                return dataLength;
            }
            catch (Exception ex)
            {
                 return 1337;
            }
        }





    }

    /// <summary>
    /// FTP上的项目（包括文件和文件夹）
    /// </summary>
    public class FtpItem
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 类型  1文件  2文件夹
        /// </summary>
        public int ItemType { get; set; }

        public DateTime CreteTime { get; set; }
    }
}