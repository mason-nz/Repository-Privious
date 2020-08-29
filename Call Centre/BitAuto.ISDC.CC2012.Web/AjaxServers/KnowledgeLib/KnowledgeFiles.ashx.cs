using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Web.Util;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    /// <summary>
    /// KnowledgeFiles 的摘要说明
    /// </summary>
    public class KnowledgeFiles : IHttpHandler, IRequiresSessionState
    {
        // private const string UpladFilesPath = "KnowledgeLib\\KnowledgeFiles";

        /// <summary>
        /// 集中权限系统登录是的cookies的内容
        /// </summary>
        public string LoginCookiesContent
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("LoginCookiesContent");
            }
        }
        private string[] fileFilters = new[] { ".pdf", ".wav", ".mp3" };
        public void ProcessRequest(HttpContext context)
        {
            //BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (UserRightsService.CheckLoginUserIDByCookies(LoginCookiesContent) > 0)
            {
                context.Response.ContentType = "text/plain";
                string retStr = "";

                HttpPostedFile file = context.Request.Files["Filedata"];

                try
                {
                    if (file.ContentLength == 0)
                    {
                        context.Response.Write("[{'result':'noFiles'}]");
                    }
                    else
                    {
                        #region 保存文件

                        var fileExtention = Path.GetExtension(file.FileName.ToLower());
                        if (!fileFilters.Contains(fileExtention))
                        {
                            context.Response.Write("[{'result':'noFiles'}]");
                            return;
                        }

                        string retUrlPath = "";

                        //添加文件路径信息
                        string fullName = GenPath(file.FileName, out retUrlPath);
                        file.SaveAs(fullName);//保存上载文件

                        #endregion

                        #region 获取要返回的信息

                        string filePath = retUrlPath;// retUrlPath + Path.GetFileName(fullName);//文件路径
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName);//文件名（不含扩展名）
                        string fileExt = Path.GetExtension(file.FileName); ;//扩展名
                        FileInfo info = new FileInfo(fullName);
                        long fileSize = info.Length;//文件大小（Byte）

                        #endregion

                        #region 返回Json值

                        retStr += "{";
                        retStr += "'result':'succeed',";
                        retStr += "'RecID':null,";
                        retStr += "'FilePath':'" + HttpUtility.UrlEncode(filePath) + "',";
                        retStr += "'FileName':'" + HttpUtility.UrlEncode(fileName) + "',";
                        retStr += "'ExtendName':'" + HttpUtility.UrlEncode(fileExt) + "',";
                        retStr += "'FileSize':'" + HttpUtility.UrlEncode(fileSize.ToString()) + "'";
                        //retStr += "'FilePath':'" + filePath + "',";
                        //retStr += "'FileName':'" + fileName + "',";
                        //retStr += "'ExtendName':'" + fileExt + "',";
                        //retStr += "'FileSize':'" + fileSize.ToString() + "'";

                        retStr += "}";

                        context.Response.Write(retStr);

                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write("[{'result':'failure'}]");
                }
            }
        }

        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <returns>绝对路径</returns>
        private string GenPath(string fileName, out string UrlPath)
        {
            string strMapPath = BLL.Util.GetUploadWebRoot();
            string strDir = BLL.Util.GetUploadProject(BLL.Util.ProjectTypePath.KnowledgeLib);
            if (!Directory.Exists(strMapPath + strDir))
            {
                Directory.CreateDirectory(strMapPath + strDir);
            }
            UrlPath = (Path.GetFileNameWithoutExtension(fileName) + Guid.NewGuid().ToString() + Path.GetExtension(fileName)).Replace("/", @"\");
            return strMapPath + strDir + UrlPath;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}