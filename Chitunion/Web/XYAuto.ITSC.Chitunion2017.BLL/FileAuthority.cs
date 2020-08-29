using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Threading;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.Entities.Query;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class FileAuthority : IHttpHandler
    {
        private HttpRequest Request;
        private HttpResponse Response;

        public void ProcessRequest(HttpContext context)
        {
            Chitunion2017.Common.UserInfo.Check();
            Request = context.Request;
            Response = context.Response;
            ApplicationInstance_BeginRequest(null, null);
        }

        public bool IsReusable
        {
            get
            {
                // TODO:  添加 NewsRssSearch.IsReusable getter 实现
                return false;
            }
        }

        private void ApplicationInstance_BeginRequest(object sender, EventArgs e)
        {
            //验证文件数据权限逻辑
            //Response.Write("您无权查看此文件!");
            //Response.End();
            var userId = Chitunion2017.Common.UserInfo.GetLoginUserID();//获取用户id
            var roleEnum = RoleInfoMapping.GetUserRole();
            if (roleEnum == RoleEnum.SupperAdmin || roleEnum == RoleEnum.YunYingOperate)
            {
                ResponseOperate();
            }
            else if (roleEnum == RoleEnum.AE)
            {
                //授权AE的UserID ，包括自己
                var userIds = Common.UserInfo.GetAuthAEUserIDList(userId);
                //查找全部AE的用户相关的文件
                var fileList = BLL.UploadFileInfo.UploadFileInfo.Instance.GetList(
                     new UploadFileQuery<Entities.UploadFileInfo.UploadFileInfo>()
                     {
                         AEUserId = userIds
                     });
                if (fileList.Count == 0)
                {
                    Response.Write("您无权查看此文件!");
                    Response.End();
                }
                ResponseOperate();
            }
            else if (roleEnum == RoleEnum.Advertiser)
            {
                //广告主
                var fileList = BLL.UploadFileInfo.UploadFileInfo.Instance.GetList(
                   new UploadFileQuery<Entities.UploadFileInfo.UploadFileInfo>()
                   {
                       FilePah = Request.RawUrl,
                       RelationTableName = "UserDetailInfo",
                       RelationID = userId, //获取用户id
                   });
                if (fileList.Count == 0)
                {
                    Response.Write("您无权查看此文件!");
                    Response.End();
                }
                ResponseOperate();
            }
            else
            {
                //当前登录人的
                var fileList = BLL.UploadFileInfo.UploadFileInfo.Instance.GetList(
                    new UploadFileQuery<Entities.UploadFileInfo.UploadFileInfo>()
                    {
                        CreaetUserID = userId, //获取用户id
                    });
                if (fileList.Count == 0)
                {
                    Response.Write("您无权查看此文件!");
                    Response.End();
                }
                ResponseOperate();
            }
        }

        private void ResponseOperate()
        {
            string filepath = Request.PhysicalPath;

            Response.Clear();
            bool success = ResponseFile(Request, Response, Path.GetFileName(filepath), filepath, 1024000);
            if (!success)
                Response.Write("下载文件出错！");
            Response.End();
        }

        public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
        {
            try
            {
                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    _Response.Expires = -1;
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;
                    double pack = 10240; //10K bytes
                    //int sleep = 200;    //每秒5次    即5*10K bytes每秒
                    int sleep = (int)Math.Floor(1000 * pack / _speed) + 1;
                    if (_Request.Headers["Range"] != null)
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        //Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength-1, fileLength));             }
                    }
                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));
                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin); int maxCount = (int)Math.Floor((fileLength - startBytes) / pack) + 1;
                    for (int i = 0; i < maxCount; i++)
                    {
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(int.Parse(pack.ToString())));
                            Thread.Sleep(sleep);
                        }
                        else { i = maxCount; }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch (Exception e) { return false; }
            return true;
        }
    }
}