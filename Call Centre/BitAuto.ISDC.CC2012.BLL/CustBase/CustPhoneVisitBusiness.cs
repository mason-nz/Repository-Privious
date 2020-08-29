using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CustPhoneVisitBusiness
    {
        public static CustPhoneVisitBusiness Instance = new CustPhoneVisitBusiness();

        /// 维护号码处理任务表数据
        /// <summary>
        /// 维护号码处理任务表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertOrUpdateCustPhoneVisitBusiness(CustPhoneVisitBusinessInfo model, int userid)
        {
            CustPhoneVisitBusinessInfo old_model = GetCustPhoneVisitBusinessInfo(model.PhoneNum_Value, model.TaskID_Value, model.BusinessType_Value);
            if (old_model == null)
            {
                //新增
                model.CreateUserID = model.ModifyUserID = userid;
                model.CreateTime = model.ModifyTime = DateTime.Now;
                return CommonBll.Instance.InsertComAdoInfo(model);
            }
            else
            {
                model.RecID = old_model.RecID;
                model.ModifyUserID = userid;
                model.ModifyTime = DateTime.Now;
                return CommonBll.Instance.UpdateComAdoInfo(model);
            }
        }

        /// 获取实体类
        /// <summary>
        /// 获取实体类
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="taskid"></param>
        /// <param name="businesstype"></param>
        /// <returns></returns>
        public CustPhoneVisitBusinessInfo GetCustPhoneVisitBusinessInfo(string phone, string taskid, int businesstype)
        {
            return Dal.CustPhoneVisitBusiness.Instance.GetCustPhoneVisitBusinessInfo(phone, taskid, businesstype);
        }
    }
}
