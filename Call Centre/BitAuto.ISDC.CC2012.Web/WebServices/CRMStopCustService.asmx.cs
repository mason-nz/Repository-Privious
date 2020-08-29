using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WebServices
{
    /// <summary>
    /// CRMStopCustService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class CRMStopCustService : System.Web.Services.WebService
    {
        [WebMethod(Description = "CRM申请停用，启用成功后，调用此接口，插入到客户呼叫中心系统，并生产任务，进行后续的处理。参数为：CRM停用，启用客户申请ID、客户ID、申请者ID、大区名称、申请时间、停用处理状态枚举值、备注信息、删除会员信息（格式：易湃会员1,易湃会员2;车商通会员1,车商通会员2）,2014-8-6新增启用申请，加参数ApplyType 1停用，2启用，新加申请原因ApplyReason，申请说明ApplyRemark")]
        public int GenOrderCRMCustTask(string Verifycode, int CRMStopCustApplyID, string CustID, int ApplyerID, string AreaName, DateTime ApplyTime, int StopStatus, string Remark, string DeleteMemberID, int ApplyType, int ApplyReason, int ApplyRemark, ref string msg)
        {
            BLL.Loger.Log4Net.Info("调用了【CRM停用/启用核实】接口，CRMStopCustApplyID:[" + CRMStopCustApplyID + "],CustID:[" + CustID + "],ApplyerID:[" + ApplyerID + "],AreaName:[" + AreaName + "],ApplyTime:[" + ApplyTime.ToString() + "],StopStatus:[" + StopStatus + "],Remark:[" + Remark + "],DeleteMemberID:[" + DeleteMemberID + "],ApplyType:[" + ApplyType + "],ApplyReason:[" + ApplyReason + "],ApplyRemark:[" + ApplyRemark + "]");
            int flag = 0;
            try
            {
                if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "CRM申请停用调用，授权失败"))
                {
                    flag = 1;

                    CRMStopCustWebService.Insert(CRMStopCustApplyID, CustID, ApplyerID, AreaName, ApplyTime, StopStatus, Remark, DeleteMemberID, ApplyType, ApplyReason, ApplyRemark);
                    return flag;
                }
                else
                {
                    BLL.Loger.Log4Net.Info("调用了【CRM停用/启用核实】接口,验证失败。Verifycode：【" + Verifycode + "】,IP：【" + System.Web.HttpContext.Current.Request.UserHostAddress + "】");
                }
                return flag;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("调用了【CRM停用/启用核实】接口出现异常：" + ex);
                return flag;
            }
        }

        [WebMethod(Description = "CRM审核停用,启用成功后，调用此接口，根据ID，更新停用状态和插入操作记录信息。参数为：CRM停用启用客户申请ID、停用启用处理状态枚举值、备注信息")]
        public int UpdateCRMCustStatusByIDAndRemark(string Verifycode, int CRMStopCustApplyID, int StopStatus, string Remark, int operID, ref string msg)
        {
            BLL.Loger.Log4Net.Info("调用了【CRM停用/启用核实更改状态】接口，传入参数为：" + string.Format("CRMStopCustApplyID:{0},StopStatus:{1},Remark:{2},operID:{3}", CRMStopCustApplyID, StopStatus, Remark, operID));
            int flag = 0;
            try
            {
                if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "CRM申请停用调用，授权失败。"))
                {
                    flag = 1;
                    Entities.StopCustApply oldModel = BLL.StopCustApply.Instance.GetStopCustApplyByCrmStopCustApplyID(CRMStopCustApplyID);
                    Entities.StopCustApply model = BLL.StopCustApply.Instance.GetStopCustApplyByCrmStopCustApplyID(CRMStopCustApplyID);
                    if (model != null)
                    {
                        DateTime dtime = DateTime.Now;
                        model.StopStatus = StopStatus;
                        model.StopTime = dtime;
                        if (StopStatus == (int)StopCustStopStatus.Disabled)
                        {
                            model.Remark = Remark;
                        }
                        else if (StopStatus == (int)StopCustStopStatus.Reject)
                        {
                            model.RejectReason = Remark;
                        }
                        //更新主表
                        BLL.StopCustApply.Instance.Update(model);
                        //插入日志
                        Log.InsertLogStopCustApply(oldModel, model, operID);
                        //获取任务
                        OrderCRMStopCustTaskInfo model_task = BLL.OrderCRMStopCustTask.Instance.GetEntityByRelationID(model.RecID);
                        if (model_task != null)
                        {
                            BLL.Loger.Log4Net.Info("调用了【CRM停用/启用核实更改状态】接口当前任务状态=" + model_task.TaskStatus_Value + " 任务ID=" + model_task.TaskID_Value);
                            StopCustTaskOperStatus operstatus = StopCustTaskOperStatus.Disabled;
                            //停用申请，3是已停用
                            if (StopStatus == (int)StopCustStopStatus.Disabled && model.ApplyType == 1)
                            {
                                //操作状态是停用
                                operstatus = StopCustTaskOperStatus.Disabled;
                            }
                            //启用申请，3是已启用
                            else if (StopStatus == (int)StopCustStopStatus.Disabled && model.ApplyType == 2)
                            {
                                //操作状态是启用
                                operstatus = StopCustTaskOperStatus.Enable;
                            }
                            //剩余为驳回
                            else
                            {
                                operstatus = StopCustTaskOperStatus.Reject;
                            }
                            //记录操作日志
                            CRMStopCustWebService.InsertOperationLog(model_task.TaskID_Value, operID, Remark, (StopCustTaskStatus)model_task.TaskStatus_Value, operstatus);
                        }
                    }
                    return flag;
                }
                else
                {
                    BLL.Loger.Log4Net.Info("调用了【CRM停用/启用核实更改状态】接口,验证失败。Verifycode：【" + Verifycode + "】,IP：【" + System.Web.HttpContext.Current.Request.UserHostAddress + "】");
                }
                return flag;
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Error("调用了【CRM停用/启用核实更改状态】接口出现异常：" + ex);
                return flag;
            }
        }
    }
}
