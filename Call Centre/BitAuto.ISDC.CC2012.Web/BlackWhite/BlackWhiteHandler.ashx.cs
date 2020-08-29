using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.BlackWhite
{
    /// <summary>
    /// BlackWhiteHandler 的摘要说明
    /// </summary>
    public class BlackWhiteHandler : IHttpHandler, IRequiresSessionState
    {
        #region
        public string Action
        {
            get
            {
                return HttpContext.Current.Request["Action"] == null ? string.Empty : HttpContext.Current.Request["Action"].ToString();
            }
        }
        public string RecId
        {
            get
            {
                return HttpContext.Current.Request["RecId"] == null ? string.Empty : HttpContext.Current.Request["RecId"].ToString();
            }
            set
            {

            }
        }
        public string Type
        {
            get
            {
                return HttpContext.Current.Request["Type"] == null ? string.Empty : HttpContext.Current.Request["Type"].ToString();
            }
        }
        public string PhoneNum
        {
            get
            {
                return HttpContext.Current.Request["PhoneNum"] == null ? string.Empty : HttpContext.Current.Request["PhoneNum"].ToString();
            }
        }
        public string CallType
        {
            get
            {
                return HttpContext.Current.Request["CallType"] == null ? string.Empty : HttpContext.Current.Request["CallType"].ToString();
            }
        }
        public string ExpiryDate
        {
            get
            {
                return HttpContext.Current.Request["ExpiryDate"] == null ? string.Empty : HttpContext.Current.Request["ExpiryDate"].ToString();
            }
        }
        public string CDIDS
        {
            get
            {
                return HttpContext.Current.Request["CDIDS"] == null ? string.Empty : HttpContext.Current.Request["CDIDS"].ToString();
            }
        }
        public string CDIDS2
        {
            get
            {
                return HttpContext.Current.Request["CDIDS2"] == null ? string.Empty : HttpContext.Current.Request["CDIDS2"].ToString();
            }
        }
        public string Reason
        {
            get
            {
                return HttpContext.Current.Request["Reason"] == null ? string.Empty : HttpContext.Current.Request["Reason"].ToString();
            }
        }
        public string CallID
        {
            get
            {
                return HttpContext.Current.Request["CallID"] == null ? string.Empty : HttpContext.Current.Request["CallID"].ToString();

            }
        }
        public string NoDisturbReason
        {
            get
            {
                return HttpContext.Current.Request["NoDisturbReason"] == null ? string.Empty : HttpContext.Current.Request["NoDisturbReason"].ToString();

            }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            context.Response.ContentType = "text/plain";
            string msg = string.Empty;
            switch (Action.ToLower())
            {
                case "changeblackwhitestatus":
                    ChangeBlackWhiteStatus(out msg);
                    break;
                case "adddata":
                    AddData(out msg);
                    break;
                case "updatedata":
                    UpdateData(out msg);
                    break;
                case "checkexist":
                    CheckExist(out msg);
                    break;
                case "addnodisturbdata":
                    AddNoDisturbData(out msg);
                    break;
                case "updatenodisturbdata":
                    UpdateNoDisturbData(out msg);
                    break;
                case "checkphonenumisnodisturb":
                    CheckPhoneNumIsNoDisturb(out msg);
                    break;
                case "getaudiourlbycallid":
                    GetAudioURLByCallID(out msg);
                    break;
            }
            context.Response.Write(msg);
        }

        private void GetAudioURLByCallID(out string msg)
        { 
          string strUrl = BLL.CallRecordInfo.Instance.GetAudioURLByCallID(CallID);
          if (strUrl.Length > 0)
          {
              msg = "{result:'yes',msg:'"+strUrl+"'}";
          }
          else
          {
              msg = "{result:'no',msg:'没有找到指定的录音文件'}";
          }
        }

        private void CheckPhoneNumIsNoDisturb(out string msg)
        {
            msg = BLL.BlackWhiteList.Instance.PhoneNumIsNoDisturb(PhoneNum).ToString();
        }
        /// <summary>
        /// 更新免打扰记录信息
        /// </summary>
        /// <param name="msg"></param>
        private void UpdateNoDisturbData(out string msg, int updateRecID = 0)
        {
            msg = string.Empty;
            try
            {
                int recid = 0;
                if (int.TryParse(RecId, out recid) || updateRecID > 0)
                {
                    if (BLL.BlackWhiteList.Instance.PhoneNumIsNoDisturb(PhoneNum) == 2)
                    {
                        //增加免打扰号码
                        //AddNoDisturbData(out msg);
                        msg = "{result:'no',msg:'此号码还不是免打扰号码，请将此号码先添加为免打扰号码'}";
                    }
                    else
                    {                        
                        int userid = BLL.Util.GetLoginUserID();;
                        if (updateRecID > 0)
                        {
                            recid = updateRecID;
                        }
                        Entities.BlackWhiteList model = BLL.BlackWhiteList.Instance.GetModel(recid);
                        
                        model.RecId = recid;
                        model.PhoneNum = PhoneNum;
                        model.CallType = int.Parse(CallType);
                        if (model.Status == -1 || model.Status == 1)
                        {
                            model.CreateDate = DateTime.Now;
                            model.CreateUserId = userid;
                        }
                        DateTime expireydate = GetExpiryDate(ExpiryDate);
                        if (expireydate.ToString("yyyy-MM-dd").CompareTo(DateTime.Now.ToString("yyyy-MM-dd")) >= 0)
                        {
                            model.Status = 0;
                        }
                        else
                        {
                            model.Status = 1;
                        }
                        model.ExpiryDate = expireydate;

                        int cdids;
                        if (int.TryParse(CDIDS, out cdids))
                        {
                            model.CDIDS = cdids;
                        } 

                        model.Reason = Reason;
                        model.UpdateDate = DateTime.Now;
                        model.UpdateUserId = userid;
                        model.SynchrodataStatus = 1;
                        long callid;
                        if (long.TryParse(CallID,out callid))
                        {
                            model.CallID = callid;
                        }
                        else if (updateRecID > 0)
                        {
                            model.CallID = null;
                        }
                        int calloutndtype;
                        if (int.TryParse(NoDisturbReason, out calloutndtype))
                        {
                            model.CallOutNDType = calloutndtype;

                        }

                        Entities.EmployeeAgent employeeagent = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(userid);
                        if (employeeagent != null && employeeagent.BGID.HasValue)
                        {
                            model.BGID = employeeagent.BGID.Value;
                        }
                        bool retVal = BLL.BlackWhiteList.Instance.UpdateNoDisturbData(model);
                        if (retVal)
                        {
                            msg = "{result:'yes',msg:'保存成功'}";
                        }
                        else
                        {
                            msg = "{result:'no',msg:'操作失败，请稍后再试'}";
                        }
                    }
                }
                else
                {
                    msg = "{result:'no',msg:'参数异常'}";
                }
            }
            catch (Exception ex)
            {
                msg = "{result:'no',msg:'" + ex.Message + "'}";
            }
        }

        private void AddNoDisturbData(out string msg)
        {
            msg = string.Empty;
            try
            {
                int valPhoneNumIsNoDisturb = BLL.BlackWhiteList.Instance.PhoneNumIsNoDisturb(PhoneNum);
                if (valPhoneNumIsNoDisturb == 2)
                {
                    int userid = BLL.Util.GetLoginUserID();
                    //增加免打扰号码
                    Entities.BlackWhiteList model = new Entities.BlackWhiteList();
                    model.Type = 0;
                    model.PhoneNum = PhoneNum;
                    model.EffectiveDate = DateTime.Now;
                    model.ExpiryDate = GetExpiryDate(ExpiryDate);
                    model.CallType = int.Parse(CallType);
                    
                    int cdids;
                    if (int.TryParse(CDIDS, out cdids))
                    {
                        model.CDIDS = cdids;
                    } 

                    model.Reason = Reason;
                    model.SynchrodataStatus = 0;
                    model.CreateUserId = userid;
                    model.CreateDate = DateTime.Now;
                    model.Status = 0;
                  
                    long callid;
                    if (long.TryParse(CallID, out callid))
                    {
                        model.CallID = callid;
                    }
                    int calloutndtype;
                    if (int.TryParse(NoDisturbReason, out calloutndtype))
                    {
                        model.CallOutNDType = calloutndtype;

                    }

                    Entities.EmployeeAgent employeeagent = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(userid);
                    if (employeeagent != null && employeeagent.BGID.HasValue)
                    {
                        model.BGID = employeeagent.BGID.Value;
                    } 

                    int retVal = BLL.BlackWhiteList.Instance.AddNoDisturbData(model);
                    if (retVal > 0)
                    {
                        msg = "{result:'yes',msg:'保存成功'}";
                    }
                    else
                    {
                        msg = "{result:'no',msg:'操作失败，请稍后再试'}";
                    }
                }
                else if (valPhoneNumIsNoDisturb == -1)
                {
                    //修改免打扰号码,需要传递RecID
                    int backRecID = BLL.BlackWhiteList.Instance.GetRecIDByPhoneNumberAndType(PhoneNum, 0);
                    if (backRecID > 0)
                    {
                        UpdateNoDisturbData(out msg, backRecID);
                    }
                    else
                    {
                        msg = "{result:'no',msg:'没有找到主键RecID，请稍后再试'}";
                    }
                }
                else if (valPhoneNumIsNoDisturb == 0 || valPhoneNumIsNoDisturb == 1)
                {
                    msg = "{result:'no',msg:'此号码已被设置为免打扰号码，不能重复添加'}";
                }
                else
                {
                    msg = "{result:'no',msg:'对号码的判断失败，请稍后再试'}";
                }
            }
            catch (Exception ex)
            {
                msg = "{result:'no',msg:'" + ex.Message + "'}";
            }
        }

        private void CheckExist(out string msg)
        {
            try
            {
                if (BLL.BlackWhiteList.Instance.IsPhoneNumberCDIDExist(PhoneNum, int.Parse(CDIDS)))
                {
                    msg = "{result:'yes',msg:'此号码对应的业务已存在！'}";
                    return;
                }
                else
                {
                    msg = "{result:'no',msg:'不存在'}";
                }
            }
            catch (Exception ex)
            {
                msg = "{result:'yes',msg:'参数异常'}";
            }
        }

        private void UpdateData(out string msg)
        {
            msg = string.Empty;
            try
            {
                int recid;
                if (int.TryParse(RecId, out recid))
                {
                    string[] cdidsArr = CDIDS2.Split(',');
                    for (int i = 0; i < cdidsArr.Length; i++)
                    {
                        if (BLL.BlackWhiteList.Instance.IsPhoneNumExist(PhoneNum, int.Parse(cdidsArr[i]), recid))
                        {
                            msg = "{result:'no',msg:'电话号码已添加过，请核对数据'}";
                            return;
                        }
                    }

                    Entities.BlackWhiteList model = BLL.BlackWhiteList.Instance.GetModel(recid);
                    model.PhoneNum = PhoneNum;
                    model.CallType = int.Parse(CallType);
                    model.ExpiryDate = GetExpiryDate(ExpiryDate);

                    int cdids;
                    if (int.TryParse(CDIDS, out cdids))
                    {
                        model.CDIDS = cdids;
                    } 

                    model.Reason = Reason;
                    model.UpdateDate = DateTime.Now;
                    model.UpdateUserId = BLL.Util.GetLoginUserID();
                    model.SynchrodataStatus = 1;


                    //此处的判断要除去当前的recid对应的数据行
                    //if (BLL.BlackWhiteList.Instance.IsPhoneNumberCDIDExist(model.PhoneNum, model.CDIDS))
                    //{
                    //    msg = "电话号码已添加过，请核对数据";
                    //    return;
                    //}

                    bool retVal = BLL.BlackWhiteList.Instance.Update(model);
                    if (retVal)
                    {
                        msg = "{result:'yes',msg:'保存成功'}";
                    }
                    else
                    {
                        msg = "{result:'no',msg:'操作失败，请稍后再试'}";
                    }
                }
                else
                {
                    msg = "{result:'no',msg:'参数异常'}";
                }
            }
            catch (Exception ex)
            {
                msg = "{result:'no',msg:'" + ex.Message + "'}";
            }

        }
        private DateTime GetExpiryDate(string ExpiryDate)
        {
            DateTime dt = DateTime.Now;
            int expiredateInt;
            DateTime expiredateDate;
            if (int.TryParse(ExpiryDate,out expiredateInt))
            {
                dt = DateTime.Now.AddMonths(expiredateInt);
            }
            else if (DateTime.TryParse(ExpiryDate,out expiredateDate))
            {
                dt = expiredateDate;
            }
            //switch (ExpiryDate)
            //{
            //    case "3": dt = DateTime.Now.AddMonths(3); break;
            //    case "6": dt = DateTime.Now.AddMonths(6); break;
            //    case "12": dt = DateTime.Now.AddMonths(12); break;
            //    default: dt = Convert.ToDateTime(ExpiryDate); break;
            //}
            return dt;
        }
        private void AddData(out string msg)
        {
            msg = string.Empty;
            try
            {
                string[] cdidsArr = CDIDS2.Split(',');
                for (int i = 0; i < cdidsArr.Length; i++)
                {
                    if (BLL.BlackWhiteList.Instance.IsPhoneNumberCDIDExist(PhoneNum, int.Parse(cdidsArr[i])))
                    {
                        msg = "{result:'no',msg:'电话号码已添加过，请核对数据'}";
                        return;
                    }
                }

                Entities.BlackWhiteList model = new Entities.BlackWhiteList();
                model.Type = int.Parse(Type);
                model.PhoneNum = PhoneNum;
                model.EffectiveDate = DateTime.Now;
                model.ExpiryDate = GetExpiryDate(ExpiryDate);
                model.CallType = int.Parse(CallType);

                int cdids;
                if (int.TryParse(CDIDS, out cdids))
                {
                    model.CDIDS = cdids;
                } 

                model.Reason = Reason;
                model.SynchrodataStatus = 0;
                model.CreateUserId = BLL.Util.GetLoginUserID();
                model.CreateDate = DateTime.Now;
                model.Status = 0;

                int retVal = BLL.BlackWhiteList.Instance.Add(model);
                if (retVal > 0)
                {
                    msg = "{result:'yes',msg:'保存成功'}";
                }
                else
                {
                    msg = "{result:'no',msg:'操作失败，请稍后再试'}";
                }
            }
            catch (Exception ex)
            {
                msg = "{result:'no',msg:'" + ex.Message + "'}";
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 删除黑白名单数据 
        /// </summary>
        /// <param name="msg"></param>
        private void ChangeBlackWhiteStatus(out string msg)
        {
            msg = string.Empty;
            try
            {
                int recid;
                if (!string.IsNullOrEmpty(RecId) && int.TryParse(RecId, out recid))
                {

                    if (BLL.BlackWhiteList.Instance.DeleteByChangeStatus(recid, BLL.Util.GetLoginUserID()))
                    {
                        msg = "{result:'yes',msg:'保存成功'}";
                    }
                    else
                    {
                        msg = "{result:'no',msg:'操作失败，请稍后再试'}";
                    }
                }
                else
                {
                    msg = "{result:'no',msg:'参数异常'}";
                }
            }
            catch (Exception ex)
            {
                msg = "{result:'no',msg:'" + ex.Message + "'}";
            }
        }
    }
}