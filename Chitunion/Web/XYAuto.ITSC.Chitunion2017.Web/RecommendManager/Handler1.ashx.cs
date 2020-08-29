using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
namespace XYAuto.ITSC.Chitunion2017.Web.RecommendManager
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {
        public string ReqCid
        {
            get { return HttpContext.Current.Request["Cid"] == null ? string.Empty : HttpContext.Current.Request["Cid"].ToString().Trim(); }
        }
        public string ReqMediaTypeID
        {
            get { return HttpContext.Current.Request["MediaTypeID"] == null ? string.Empty : HttpContext.Current.Request["MediaTypeID"].ToString().Trim(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            string msg = string.Empty;
            context.Response.ContentType = "text/plain";
            InsertIntoMessageQueue(out msg);
            context.Response.Write(msg);
        }
        private void InsertIntoMessageQueue(out string msg)
        {
            msg = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(ReqCid) && !string.IsNullOrWhiteSpace(ReqMediaTypeID))
                {
                    string[] strArray = ReqCid.Remove(ReqCid.Length - 1).Split('^');
                    List<HomeCategoryModle> listHomeCategory = new List<HomeCategoryModle>();
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        string[] strArr = strArray[i].Split('#');
                        HomeCategoryModle HomeCategory = new HomeCategoryModle();
                        HomeCategory.MediaType = Convert.ToInt32(ReqMediaTypeID.Trim());
                        HomeCategory.CategoryID = Convert.ToInt32(strArr[0]);
                        HomeCategory.CategoryName = strArr[1];
                        listHomeCategory.Add(HomeCategory);
                    }
                    BLL.HomeCategory.Instance.InsertHomeCategory(listHomeCategory);
                    msg = "添加分类成功";
                }
                else
                {
                    msg = "添加失败：传递参数异常";
                    return;
                }
            }
            catch (Exception ex)
            {
                msg = "添加产品分类出现异常";
                BLL.Loger.Log4Net.Info("添加产品分类出现异常：" + ex.Message);
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