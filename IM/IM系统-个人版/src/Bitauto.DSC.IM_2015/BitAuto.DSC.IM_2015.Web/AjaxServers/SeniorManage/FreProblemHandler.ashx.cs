using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.SeniorManage
{
    /// <summary>
    /// FreProblemHandler 的摘要说明
    /// </summary>
    public class FreProblemHandler : IHttpHandler, IRequiresSessionState
    {
        #region 属性
        /// 操作方式
        /// <summary>
        /// 操作方式
        /// </summary>
        public string Action { get { return BLL.Util.GetCurrentRequestFormStr("Action"); } }
        /// 标题
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestFormStr("Title")); } }

        /// 链接
        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get { return BLL.Util.GetCurrentRequestFormStr("Url"); } }
        /// ID
        /// <summary>
        /// ID
        /// </summary>
        public int RecID { get { return BLL.Util.GetCurrentRequestFormInt("RecID"); } }
        /// 方向
        /// <summary>
        /// 方向
        /// </summary>
        public string Direct { get { return BLL.Util.GetCurrentRequestFormStr("Direct"); } }


        //备注
        public string Remark { get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestFormStr("Remark")); } }
        //业务线
        public string SourceTyps { get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestFormStr("SourceTyps")); } }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            bool success = false;
            string msg = "成功";

            switch (Action.ToLower())
            {
                case "add":
                    success = InsertFreProblem(out msg);
                    break;
                case "mod":
                    success = UpdateFreProblem(out msg);
                    break;
                case "del":
                    success = DeleteFreProblem(out msg);
                    break;
                case "move":
                    success = MoveUpOrDown(out msg);
                    break;
                case "check":
                    success = CheckMaxCount(out msg);
                    break;
                case "getsourcetype":
                    GetSourceType(context);
                    break;
            }

            if (success)
            {
                context.Response.Write("{'result':'success','msg':'" + msg + "'}");
            }
            else
            {
                context.Response.Write("{'result':'error','msg':'" + msg + "'}");
            }
        }

        /// <summary>
        /// 获取业务线
        /// </summary>
        /// <param name="context"></param>
        private void GetSourceType(HttpContext context)
        {
            var list = BLL.Util.GetAllSourceType(false);
            var json = JsonConvert.SerializeObject(list);
            context.Response.ContentType = "application/json";
            context.Response.Write("{\"result\":true,\"json\":" + json + "}");
            context.Response.End();
        }

        /// 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool InsertFreProblem(out string msg)
        {
            msg = "";
            try
            {
                Entities.FreProblem info = new Entities.FreProblem();
                info.Title = Title;
                info.Url = Url;
                info.SortNum = 1; //BLL.FreProblem.Instance.GetMaxSortNum() + 1;
                info.Status = 0;
                info.CreateTime = DateTime.Now;
                info.Remark = Remark;
                info.SourceType = SourceTyps;
                info.CreateUserID = BLL.Util.GetLoginUserID();
                BLL.FreProblem.Instance.AddFeeProblem();
                return BLL.FreProblem.Instance.InsertComAdoInfo<Entities.FreProblem>(info);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
        /// 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool UpdateFreProblem(out string msg)
        {
            msg = "";
            try
            {
                Entities.FreProblem info = BLL.FreProblem.Instance.GetComAdoInfo<Entities.FreProblem>(RecID);
                info.Title = Title;
                info.Remark = Remark;
                info.SourceType = SourceTyps;
                info.Url = Url;
                info.LastUpdateTime = DateTime.Now;
                info.LastUpdateUserID = BLL.Util.GetLoginUserID();
                return BLL.FreProblem.Instance.UpdateComAdoInfo<Entities.FreProblem>(info);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
        /// 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool DeleteFreProblem(out string msg)
        {
            msg = "";
            try
            {
                return BLL.FreProblem.Instance.DeleteComAdoInfo<Entities.FreProblem>(RecID);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
        /// 移动
        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool MoveUpOrDown(out string msg)
        {
            int type = Direct == "up" ? 1 : -1;
            msg = "";
            try
            {
                return BLL.FreProblem.Instance.MoveUpOrDown(RecID, type);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }
        /// 校验数据库数据量是否超标
        /// <summary>
        /// 校验数据库数据量是否超标
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool CheckMaxCount(out string msg)
        {
            msg = "";
            try
            {
                //int count = BLL.FreProblem.Instance.GetAllCount();
                //if (count >= 20)
                //{
                //    return false;
                //}
                //else
                //{
                return true;
                //}
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
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