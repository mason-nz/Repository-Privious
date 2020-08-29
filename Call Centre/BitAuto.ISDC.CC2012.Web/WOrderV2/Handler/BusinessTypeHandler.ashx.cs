using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using Newtonsoft;
using System.Web.SessionState;

namespace BitAuto.ISDC.CC2012.Web
{
    /// <summary>
    /// Summary description for BusinessTypeHandler
    /// </summary>
    public class BusinessTypeHandler : IHttpHandler, IRequiresSessionState
    {

        #region 定义属性

        public string Status
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("status");

            }
        }

        public string TagName
        {
            get { return BLL.Util.GetCurrentRequestStr("tagname"); }
        }

        public int UserID
        {
            get { return BLL.Util.GetLoginUserID(); }
        }

        public string Action
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

            if (!string.IsNullOrEmpty(Action))
            {
                string strResult = string.Empty;
                switch (Action.ToLower())
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
                    case "getselectdata":
                        message = GetData("1");
                        break;
                }

                string result = Newtonsoft.Json.JavaScriptConvert.SerializeObject(message);
                BLL.Loger.Log4Net.Info("业务类型," + Action + ":" + result);

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

            if (lstatus == -1)//删除
            {
                //判断是否使用
                Entities.QueryWOrderTag querytag = new Entities.QueryWOrderTag();
                querytag.BusiTypeID = CurrId;
                DataTable dt = BLL.WOrderTag.Instance.GetAllData(querytag);
                if (dt != null && dt.Rows.Count > 0)
                {
                    message.Success = false;
                    message.Message = "此业务线已使用，不可以删除";
                }
            }


            if (message.Success)
            {
                Entities.WOrderBusiTypeInfo entity = new Entities.WOrderBusiTypeInfo();
                entity.RecID = lCurrId;
                entity.Status = lstatus;
                entity.LastUpdateTime = DateTime.Now;
                entity.LastUpdateUserID = UserID;
                bool flag = BLL.CommonBll.Instance.UpdateComAdoInfo<Entities.WOrderBusiTypeInfo>(entity);
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
                if (string.IsNullOrEmpty(TagName))
                {
                    message.Success = false;
                    message.Message = "参数错误";
                }
                if (string.IsNullOrEmpty(TagName))
                {
                    message.Success = false;
                    message.Message = "参数错误";
                }
                if (message.Success)
                {
                    if (CheckName(TagName,CurrId))
                    {
                        message.Success = false;
                        message.Message = "业务类型重复";
                    }
                }

                int sort = BLL.WOrderBusiType.Instance.GetMaxSortNum();

                if (message.Success)
                {
                    Entities.WOrderBusiTypeInfo entity = new Entities.WOrderBusiTypeInfo()
                    {
                        BusiTypeName = TagName,
                        Status = 1,
                        SortNum = sort + 1,
                        CreateTime = DateTime.Now,
                        CreateUserID = UserID,
                        LastUpdateTime = DateTime.Now,
                        LastUpdateUserID = UserID
                    };

                    bool flag = BLL.CommonBll.Instance.InsertComAdoInfo<Entities.WOrderBusiTypeInfo>(entity);
                    message.Success = flag;
                    message.Data = entity.RecID;
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

            if (message.Success)
            {
                if (CheckName(TagName, CurrId))
                {
                    message.Success = false;
                    message.Message = "业务类型重复";
                }
            }


            try
            {

                if (lCurrId > 0 && message.Success)
                {
                    Entities.WOrderBusiTypeInfo entity = new Entities.WOrderBusiTypeInfo();
                    entity.RecID = lCurrId;
                    entity.BusiTypeName = TagName;
                    entity.LastUpdateUserID = UserID;
                    entity.LastUpdateTime = DateTime.Now;
                    bool flag = BLL.CommonBll.Instance.UpdateComAdoInfo<Entities.WOrderBusiTypeInfo>(entity);
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
                if (string.IsNullOrEmpty(CurrId) || string.IsNullOrEmpty(CurrSort) || string.IsNullOrEmpty(NextId) || string.IsNullOrEmpty(NextSort))
                {
                    message.Success = false;
                    message.Message = "参数错误";
                }
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

                if (message.Success)
                {
                    bool flag = BLL.WOrderBusiType.Instance.ChangeOrder(lCurrId, lCurrSort, lNextId, lNextSort);
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
        public ReturnMessage GetData(string status = "1")
        {
            ReturnMessage message = new ReturnMessage();
            try
            {


                //获取标签最多的业务类型-------------------
                Entities.QueryWOrderTag query = new Entities.QueryWOrderTag() { PID = "0", Status = "1" };
                int selbusid = BLL.WOrderTag.Instance.GetMaxBusiTagCount(query);


                DataTable dt = BLL.WOrderBusiType.Instance.GetAllData(new Entities.QueryWOrderBusiTypeInfo() { Status = status });

                List<object> list = new List<object>();
                object obj = new object();
                bool isSelected = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    isSelected = false;
                    if (dt.Rows[i]["RecID"].ToString() == selbusid.ToString())
                    {
                        isSelected = true;
                    }
                    obj = new
                    {
                        Selected = isSelected,
                        Id = dt.Rows[i]["RecID"].ToString(),
                        Name = dt.Rows[i]["BusiTypeName"].ToString()
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
        private bool CheckName(string name, string id)
        {
            try
            {
                Entities.QueryWOrderBusiTypeInfo query = new Entities.QueryWOrderBusiTypeInfo();
                query.BusiTypeName = name;
                query.Status = "1,0";
                query.NoRecID = id;
                DataTable dt = BLL.WOrderBusiType.Instance.GetAllData(query);
                //
                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch {
                return false;
            }

        }

        public class ReturnMessage
        {
            public bool Success { get; set; }
            public object Data { get; set; }
            public string Message { get; set; }
        }
    }
}