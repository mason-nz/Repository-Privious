using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.TaskManager
{
    public partial class NoDealerOrderView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        public string RequestTaskID
        {
            get { return HttpContext.Current.Request["TaskID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TaskID"].ToString()); }
        }

        /// <summary>
        /// 是否有查看处理订单的权限  1,有 0，没有
        /// </summary>
        public string IsEditTask = "0";

        /// <summary>
        /// 处理人是否是当前用户
        /// </summary>
        public string IsBelong = "0";

        /// <summary>
        /// 判断该任务状态是否已处理  1,有 0，没有
        /// </summary>
        public string IsProcessed = "0";

        public string Source = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //判断是否有查看该处理订单的权限
                if (BLL.Util.CheckButtonRight("SYS024BUT1201"))
                {
                    IsEditTask = "1";
                } 

                //查询数据来源属于新车还是置换订单
                int _userID=BLL.Util.GetLoginUserID();
                int _taskID;
                if (int.TryParse(RequestTaskID, out _taskID))
                {
                    Entities.OrderTask model = BLL.OrderTask.Instance.GetOrderTask(_taskID);
                    if (model != null)
                    {
                        if (model.TaskStatus == (int)Entities.TaskStatus.Processed)
                        {
                            IsProcessed = "1";
                        }
                        if (model.AssignUserID == _userID)
                        {
                            IsBelong = "1";
                        }
                        Source = model.Source.ToString();
                        ContactRecordView1.TaskID = RequestTaskID;
                        ContactRecordView1.Source = Source;
                    }
                }

            }
        } 
    }
}