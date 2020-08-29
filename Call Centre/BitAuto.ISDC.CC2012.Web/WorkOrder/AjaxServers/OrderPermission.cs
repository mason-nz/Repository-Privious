using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder
{
    public class OrderPermission
    {
        #region 判断CC处理工单权限

        public string JudgeOrderPermission(Entities.WorkOrderInfo model, out string msg)
        {
            string permission = string.Empty;

            msg = string.Empty;

            //根据状态判断是否有审核或处理该工单权限
            if (model.WorkOrderStatus == (int)Entities.WorkOrderStatus.Pending)
            {
                //如果是待审核状态，判断是否有审核的权限
                if (!judgeAudit())
                {
                    msg += "{result:'false',type:'audit',msg:'没有审核当前工单权限，操作失败！'}";
                }
                else
                {
                    permission = "audit"; //高级权限，audit
                }
            }
            else if (model.WorkOrderStatus == (int)Entities.WorkOrderStatus.Untreated || model.WorkOrderStatus == (int)Entities.WorkOrderStatus.Processing)
            {
                if (!judgeProcess())
                {
                    msg += "{result:'false',type:'process',msg:'没有处理当前工单权限，操作失败！'}";
                }
                else
                {
                    permission = "process"; //高级权限，process
                }

                if (!judgeIsCreateUserID((int)model.CreateUserID) && !judgeIsProcesser((int)model.ReceiverID))
                {
                    msg += "{result:'false',type:'process',msg:'没有处理当前工单权限，操作失败！'}";
                }
                else
                {
                    permission = "process"; //高级权限，process
                }
            }
            else if (model.WorkOrderStatus == (int)Entities.WorkOrderStatus.Processed || model.WorkOrderStatus == (int)Entities.WorkOrderStatus.Completed || model.WorkOrderStatus == (int)Entities.WorkOrderStatus.Closed)
            {
                permission = "over";
            }
            return permission;
        }

        private bool judgeAudit()
        {
            return BLL.Util.CheckButtonRight("SYS024BUG100601");
        }

        private bool judgeProcess()
        {
            return BLL.Util.CheckButtonRight("SYS024BUG100602");
        }

        private bool loginerIsInProcess(string orderID)
        {
            DataTable dt = BLL.WorkOrderInfo.Instance.GetProcessOrderUserID(orderID);
            if (dt.Select(" createUserID=" + BLL.Util.GetLoginUserID()).Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool judgeIsCreateUserID(int createUserID)
        {
            if (BLL.Util.GetLoginUserID() == createUserID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool judgeIsProcesser(int processerID)
        {
            if (BLL.Util.GetLoginUserID() == processerID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 判断销售处理工单权限

        public string JudgeSalesPermission(Entities.WorkOrderInfo model, out string msg)
        {
            msg = string.Empty;
            string permission = string.Empty;
            int loginerID = BLL.Util.GetLoginUserID();

            //状态为待处理、处理中,并且当前用户是接收人、创建人或有权限处理， 则可处理
            if ((model.WorkOrderStatus == (int)Entities.WorkOrderStatus.Untreated || model.WorkOrderStatus == (int)Entities.WorkOrderStatus.Processing) && (loginerID == model.ReceiverID || loginerID == model.CreateUserID || judgeProcess()))
            {
                return "process";
            }

            DataTable dtOperUserID = BLL.WorkOrderInfo.Instance.GetProcessOrderUserID(model.OrderID);

            //处理人列表为空，无权限
            if (dtOperUserID.Rows.Count == 0)
            {
                return "none";
            }

            DataRow[] drLoginer = dtOperUserID.Select(" createUserID = " + loginerID);
            //处理人列表没有当前登陆人，无权限
            if (drLoginer.Length == 0)
            {
                return "none";
            }
             
            return  "view";
        }

        #endregion
    }
}