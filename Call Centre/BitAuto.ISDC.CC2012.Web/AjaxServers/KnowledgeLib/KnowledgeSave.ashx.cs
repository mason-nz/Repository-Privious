using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BitAuto.ISDC.CC2012.BLL;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.KnowledgeLib
{
    /// <summary>
    /// KnowledgeSave 的摘要说明
    /// </summary>
    public class KnowledgeSave : IHttpHandler, IRequiresSessionState
    {
        #region 参数

        /// <summary>
        /// 操作类型（save:保存  sub:提交,RecordingSharingSave:录音共享保存）
        /// </summary>
        public string Action
        {
            get
            {
                if (HttpContext.Current.Request["action"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["action"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        ///  知识点ID
        /// </summary>
        public string KID
        {
            get
            {
                if (HttpContext.Current.Request["kid"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["kid"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 知识点相关数据
        /// </summary>
        public string KnowledgeLibData
        {
            get
            {
                if (HttpContext.Current.Request["knowlibdata"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["knowlibdata"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        /// <summary>
        /// 是否是管理员 
        /// </summary>
        public bool IsManager
        {
            get
            {
                if (HttpContext.Current.Request["isManager"] != null)
                {
                    string isM = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["isManager"].ToString());
                    if (isM == "1")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public string RequestKCID
        {
            get
            {
                if (HttpContext.Current.Request["KCID"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["KCID"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string RequestTitle
        {
            get
            {
                if (HttpContext.Current.Request["Title"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Title"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string RequestKAbstract
        {
            get
            {
                if (HttpContext.Current.Request["KAbstract"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["KAbstract"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string RequestFileUrl
        {
            get
            {
                if (HttpContext.Current.Request["FileUrl"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["FileUrl"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string RequestSingleInfo
        {
            get
            {
                if (HttpContext.Current.Request["singleInfo"] != null)
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["singleInfo"].ToString());
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        #endregion


        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";

            string errMsg = "";
            int kid = 0;

            #region 录音共享保存
            if (Action == "RecordingSharingSave")
            {
                try
                {
                    Entities.KnowledgeLib model = new Entities.KnowledgeLib();
                    model.Title = RequestTitle;

                    if (int.TryParse(RequestKCID, out kid))
                    {
                        model.KCID = kid;
                    }
                    model.Abstract = RequestKAbstract;
                    model.FileUrl = RequestFileUrl;
                    model.Status = 1;//待审核
                    model.CreateTime = DateTime.Now;
                    model.CreateUserID = BLL.Util.GetLoginUserID();
                    model.KLNum = "KL" + BLL.KnowledgeLib.Instance.GetCurrMaxID().ToString().PadLeft(7, '0');


                    model.LastModifyTime = DateTime.Now;
                    model.LastModifyUserID = BLL.Util.GetLoginUserID();
                    model.IsHistory = 0;
                    model.RejectReason = "";
                    model.UploadFileCount = 1;

                    model.FAQCount = 0;
                    model.QuestionCount = 0;//试题数量
                    BLL.KnowledgeLib.Instance.Insert(model);
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                }

                if (errMsg != "")
                {
                    context.Response.Write("{'result':'Error','errMsg':'" + errMsg + "'}");
                }
                else
                {
                    context.Response.Write("{'result':'success','errMsg':''}");
                }

                return;

            }
            #endregion

            KnowledgeLibHelper knowledgelibhelper = new KnowledgeLibHelper();

            try
            {
                int userID = BLL.Util.GetLoginUserID();
                knowledgelibhelper.SubmitCheckInfo(userID, out errMsg, Action, out kid, IsManager, RequestSingleInfo);

            }
            catch (Exception ex)
            {
                errMsg = ex.Message.ToString();
            }

            if (errMsg != "")
            {
                context.Response.Write("{'result':'Error','kid':'" + errMsg + "'}");
            }
            else
            {
                context.Response.Write("{'result':'success','kid':'" + kid.ToString() + "'}");
            }

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