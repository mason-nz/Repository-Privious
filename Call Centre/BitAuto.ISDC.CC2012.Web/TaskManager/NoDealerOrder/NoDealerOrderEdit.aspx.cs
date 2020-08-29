using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.TaskManager.NoDealerOrder
{
    public partial class NoDealerOrderEdit : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// <summary>
        ///  任务ID
        /// </summary>
        public string TaskID
        {
            get
            {
                if (HttpContext.Current.Request["TaskID"] != null)
                {
                    return HttpContext.Current.Request["TaskID"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string Source = "";

        /// <summary>
        /// 订单状态
        /// </summary>
        public string TaskStatus = "";

        /// <summary>
        /// 是否有处理订单的权限  1,有 0，没有
        /// </summary>
        public string IsEditTask = "";

        /// <summary>
        /// 处理人是否是当前用户
        /// </summary>
        public string IsBelong = "0";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            { 
                //判断是否有处理订单的权限
                if( BLL.Util.CheckButtonRight("SYS024BUT1201"))
                {
                    IsEditTask="1";
                }
                else
                {
                     IsEditTask="0";
                }

                //绑定数据
                GetTaskSource();

            }
        }

        /// <summary>
        /// 根据任务ID，查询订单类型
        /// </summary>
        private void GetTaskSource()
        {
            int intTid = 0;
            if (TaskID != string.Empty && int.TryParse(TaskID, out intTid))
            {
                Entities.OrderTask model = BLL.OrderTask.Instance.GetOrderTask(intTid);
                if (model != null)
                {
                    this.Source = model.Source.ToString();
                    this.TaskStatus = model.TaskStatus.ToString();

                    int userID = BLL.Util.GetLoginUserID();
                    if (model.AssignUserID == userID)
                    {
                        IsBelong = "1";
                    }
                    else
                    {
                        IsBelong = "0";
                    }

                    this.UCOrderConsult1.Source = model.Source.ToString();
                    this.UCOrderConsult1.TID = model.TaskID.ToString();
                }
            }
        }
    }
}