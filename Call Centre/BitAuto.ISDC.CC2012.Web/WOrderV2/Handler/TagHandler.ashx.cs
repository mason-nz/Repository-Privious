using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web
{
    /// <summary>
    /// Summary description for TagHandler
    /// </summary>
    public class TagHandler : IHttpHandler, IRequiresSessionState
    {

        #region 定义属性

        public string PId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("pid");
            }
        }
        public string BusiTypeId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("busitypeid");
            }
        }
        public string Status
        {
            get { return BLL.Util.GetCurrentRequestStr("status"); }
        }
        public string TagName
        {
            get { return BLL.Util.GetCurrentRequestStr("tagname"); }
        }
        public int UserId
        {
            get { return BLL.Util.GetLoginUserID(); }
        }
        public string action
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("action");
            }
        }
        /// <summary>
        /// 操作数据的id
        /// </summary>
        public string CurrId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("currid");
            }
        }
        public string CurrSort
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("currsort");
            }
        }
        /// <summary>
        /// 上移或下移 数据的id
        /// </summary>
        public string NextId
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("nextid");
            }
        }
        public string NextSort
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("nextsort");
            }
        }
        #endregion

        public HttpContext currentContext;

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            context.Response.ContentType = "text/plain";
            currentContext = context;

            ReturnMessage message = new ReturnMessage();

            if (!string.IsNullOrEmpty(action))
            {
                string strResult = string.Empty;
                switch (action.ToLower())
                {
                    case "changename":
                        message = ChangeName();
                        break;
                    case "changeorder":
                        message = ChangeOrder();
                        break;
                    case "changestatus":
                        message = ChangeStatus();
                        break;
                    case "deletag":
                        message = DeleTag();
                        break;
                    case "insertnewtag":
                        message = InsertNewTag();
                        break;
                    case "getdata":
                        message = GetData();
                        break;
                }

                string result = Newtonsoft.Json.JavaScriptConvert.SerializeObject(message);
                BLL.Loger.Log4Net.Info("业务类型," + action + ":" + result);

                context.Response.Write(result);
                context.Response.End();
                return;
            }
        }

        private ReturnMessage UpdateStatus(string nStatus = "")
        {
            ReturnMessage message = new ReturnMessage();
            message.Success = true;

            int lCurrId = 0;
            if (!Int32.TryParse(CurrId, out lCurrId))
            {
                message.Success = false;
                message.Message = "参数错误";
            }

            int lstatus = 0;
            if (!int.TryParse(nStatus, out lstatus))
            {
                message.Success = false;
                message.Message = "参数错误";
            }


            if (message.Success)
            {
                Entities.WOrderTagInfo entity = new Entities.WOrderTagInfo();
                entity.RecID = lCurrId;
                entity.Status = lstatus;
                entity.LastUpdateTime = DateTime.Now;
                entity.LastUpdateUserID = UserId;
                bool flag = BLL.CommonBll.Instance.UpdateComAdoInfo<Entities.WOrderTagInfo>(entity);
                message.Success = flag;
            }
            return message;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        private ReturnMessage InsertNewTag()
        {
            ReturnMessage message = new ReturnMessage();
            message.Success = true;
            try
            {
                #region check
                int lCurrSort = 0;
                if (string.IsNullOrEmpty(TagName))
                {
                    message.Success = false;
                    message.Message = "参数错误";
                }
                int lbusitypeid = 0;
                if (!Int32.TryParse(BusiTypeId, out lbusitypeid))
                {
                    message.Success = false;
                    message.Message = "参数错误";
                }

                int? lpid = null;
                if (!string.IsNullOrEmpty(PId))
                {
                    try
                    {
                        lpid = Int32.Parse(PId);
                    }
                    catch
                    {
                        message.Success = false;
                        message.Message = "参数错误";
                    }
                }


                if (message.Success)
                {
                    if (CheckName(TagName, PId, CurrId, BusiTypeId))
                    {
                        message.Success = false;
                        message.Message = "标签名称重复";
                    }
                }
                #endregion

                if (message.Success)
                {
                    lCurrSort = BLL.WOrderTag.Instance.GetMaxSortNum(PId);

                    Entities.WOrderTagInfo entity = new Entities.WOrderTagInfo()
                    {
                        BusiTypeID = lbusitypeid,
                        PID = lpid,
                        TagName = TagName,
                        Status = 1,
                        SortNum = lCurrSort + 1,
                        CreateTime = DateTime.Now,
                        CreateUserID = UserId,
                        LastUpdateTime = DateTime.Now,
                        LastUpdateUserID = UserId
                    };

                    bool flag = BLL.CommonBll.Instance.InsertComAdoInfo<Entities.WOrderTagInfo>(entity);
                    message.Success = flag;
                    message.Data = new { RecID = entity.RecID, SortNum = entity.SortNum };
                }

            }
            catch (Exception ex)
            {
                message.Success = false;
                message.Message = ex.Message.ToString();
            }

            return message;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        private ReturnMessage DeleTag()
        {
            return UpdateStatus("-1");
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <returns></returns>
        private ReturnMessage ChangeStatus()
        {
            return UpdateStatus(Status);
        }

        /// <summary>
        /// 修改名称
        /// </summary>
        /// <returns></returns>
        private ReturnMessage ChangeName()
        {
            ReturnMessage message = new ReturnMessage();
            message.Success = true;

            try
            {
                #region check
                if (string.IsNullOrEmpty(TagName))
                {
                    message.Success = false;
                    message.Message = "名称不能为空";
                }

                int lCurrId = 0;
                if (!Int32.TryParse(CurrId, out lCurrId))
                {
                    message.Success = false;
                    message.Message = "参数错误";
                }

                int? lpid = null;
                if (!string.IsNullOrEmpty(PId))
                {
                    try
                    {
                        lpid = Int32.Parse(PId);
                    }
                    catch
                    {
                        message.Success = false;
                        message.Message = "参数错误";
                    }
                }
                int lCurrSort = 0;
                if (!Int32.TryParse(CurrSort, out lCurrSort))
                {
                    message.Success = false;
                    message.Message = "参数错误";
                }

                if (message.Success)
                {
                    if (CheckName(TagName, PId, CurrId, BusiTypeId))
                    {
                        message.Success = false;
                        message.Message = "标签名称重复";
                    }
                }

                #endregion

                if (lCurrId > 0 && message.Success)
                {
                    Entities.WOrderTagInfo entity = new Entities.WOrderTagInfo();
                    entity.RecID = lCurrId;
                    entity.TagName = TagName;
                    entity.SortNum = lCurrSort;
                    entity.LastUpdateUserID = UserId;
                    entity.LastUpdateTime = DateTime.Now;
                    bool flag = BLL.CommonBll.Instance.UpdateComAdoInfo<Entities.WOrderTagInfo>(entity);
                    message.Success = flag;

                }
            }
            catch (Exception ex)
            {
                message.Success = false;
                message.Message = ex.Message;
            }
            return message;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <returns></returns>
        private ReturnMessage ChangeOrder()
        {
            ReturnMessage message = new ReturnMessage();
            message.Success = true;
            try
            {
                #region check
                int lCurrId = 0;
                if (!Int32.TryParse(CurrId, out lCurrId))
                {
                    message.Success = false;
                    message.Message = "参数错误";
                }

                int lCurrSort = 0;
                if (!Int32.TryParse(CurrSort, out lCurrSort))
                {
                    message.Success = false;
                    message.Message = "参数错误";
                }
                int lNextId = 0;
                if (!Int32.TryParse(NextId, out lNextId))
                {
                    message.Success = false;
                    message.Message = "参数错误";
                }
                int lNextSort = 0;
                if (!Int32.TryParse(NextSort, out lNextSort))
                {
                    message.Success = false;
                    message.Message = "参数错误";
                }

                if (lCurrId <= 0 || lNextId <= 0)
                {
                    message.Success = false;
                    message.Message = "参数错误";
                }
                #endregion

                if (message.Success)
                {
                    bool flag = BLL.WOrderTag.Instance.ChangeOrder(lCurrId, lCurrSort, lNextId, lNextSort);
                    message.Success = flag;
                }

            }
            catch (Exception ex)
            {
                message.Success = false;
                message.Message = ex.Message;
            }
            return message;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取在用数据
        /// </summary>
        /// <returns></returns>
        public ReturnMessage GetData()
        {
            ReturnMessage message = new ReturnMessage();

            try
            {
                #region 查询条件
                Entities.QueryWOrderTag query = new Entities.QueryWOrderTag();
                if (!string.IsNullOrEmpty(CurrId))
                {
                    try
                    { //必须为数字
                        query.RecID = Int32.Parse(CurrId).ToString();
                    }
                    catch { }
                }
                if (!string.IsNullOrEmpty(BusiTypeId))
                {
                    try
                    { //必须为数字
                        query.BusiTypeID = Int32.Parse(BusiTypeId).ToString();
                    }
                    catch { }
                }

                if (!string.IsNullOrEmpty(Status))
                {
                    query.Status = Status.Trim(',');
                }

                if (!string.IsNullOrEmpty(PId))
                {
                    try
                    { //必须为数字
                        query.PID = Int32.Parse(PId).ToString();
                    }
                    catch { }
                }
                #endregion

                DataTable dt = BLL.WOrderTag.Instance.GetAllData(query);
                List<object> list = new List<object>();
                object obj = new object();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    obj = new
                    {
                        RecID = dt.Rows[i]["RecID"].ToString(),
                        TagName = dt.Rows[i]["TagName"].ToString()
                    };
                    list.Add(obj);
                }
                message.Success = true;
                message.Data = list;
            }
            catch (Exception ex)
            {
                message.Success = false;
                message.Message = ex.Message;
            }

            return message;
        }

        /// <summary>
        /// 检查是否有重名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        private bool CheckName(string name, string pid, string id, string busitypeid = "")
        {
            Entities.QueryWOrderTag query = new Entities.QueryWOrderTag();
            query.TagName = name;
            query.PID = pid;
            query.Status = "1,0";
            query.NoRecID = id;
            query.BusiTypeID = busitypeid;
            DataTable dt = BLL.WOrderTag.Instance.GetAllData(query);
            //
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            return false;

        }

        public class ReturnMessage
        {
            public bool Success { get; set; }
            public object Data { get; set; }
            public string Message { get; set; }
        }
    }
}