using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CC2015_HollyFormsApp.CCWeb.CallRecordService;

namespace CC2015_HollyFormsApp
{
    public class CallRecordHelper
    {
        public static readonly CallRecordHelper Instance = new CallRecordHelper();
        CCWeb.CallRecordService.CallRecordServiceSoapClient crService;
        private string CallRecordAuthorizeCode = System.Configuration.ConfigurationManager.AppSettings["CallRecordAuthorizeCode"];//调用话务记录接口授权码

        protected CallRecordHelper()
        {
            crService = new CCWeb.CallRecordService.CallRecordServiceSoapClient();
        }

        /// 插入或更新话务数据
        /// <summary>
        /// 插入或更新话务数据
        /// </summary>
        /// <param name="response"></param>
        /// <param name="sessionID"></param>
        /// <param name="audioUrl"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool InsertCallRecordNew(bool only_update)
        {
            try
            {
                bool isOK = false;
                string msg = "";
                if (BusinessProcess.CallRecordORIG != null && !string.IsNullOrEmpty(BusinessProcess.CallRecordORIG.SessionID))
                {
                    if (only_update)
                    {
                        //只更新，不插入
                        var model = crService.GetCallRecord_ORIGByCallID(CallRecordAuthorizeCode, BusinessProcess.CallRecordORIG.CallID.Value, ref msg);
                        if (model == null)
                        {
                            //数据备份日志
                            Common.WriteCallDataBackUp(BusinessProcess.CallRecordORIG);
                            //本地备份数据
                            MDBFileHelper.Instance.SaveCallRecordORIG(BusinessProcess.CallRecordORIG);
                            //数据库中不存在该记录
                            return false;
                        }
                    }

                    isOK = crService.InsertCallRecord(CallRecordAuthorizeCode, BusinessProcess.CallRecordORIG, ref msg);

                    if (isOK == false)
                    {
                        Loger.Log4Net.Info("[CallRecordHelper][InsertCallRecordNew] 失败" + msg);
                        Common.SendErrorEmail("西安CC客户端（" + Common.GetVersion() + "）出错", msg, "", "");
                    }
                    else
                    {
                        Loger.Log4Net.Info("[CallRecordHelper][InsertCallRecordNew] 成功");
                        //更新recid
                        BusinessProcess.CallRecordORIG.RecID = crService.GetCallRecord_ORIGByCallID(CallRecordAuthorizeCode, BusinessProcess.CallRecordORIG.CallID.Value, ref msg).RecID;
                        //表示插入成功后，更新主键
                        //如果没有插入数据，之后的更新逻辑不更新
                    }
                }
                else
                {
                    Loger.Log4Net.Info("[CallRecordHelper][InsertCallRecordNew] 数据异常，不进行入库操作！");
                }
                return isOK;
            }
            catch (Exception ex)
            {
                Common.SendErrorEmail("西安CC客户端（" + Common.GetVersion() + "）出错", ex.Message, ex.Source, ex.StackTrace);
                Loger.Log4Net.Error("[CallRecordHelper][InsertCallRecordNew]", ex);
                return false;
            }
        }

        /// 补录数据
        /// <summary>
        /// 补录数据
        /// </summary>
        /// <returns></returns>
        public bool AddCallRecordNew(CallRecord_ORIG model)
        {
            try
            {
                bool isOK = false;
                string msg = "";
                if (model != null)
                {
                    Loger.Log4Net.Info("[CallRecordHelper][AddCallRecordNew] 检查是否存在该数据");
                    var old_model = crService.GetCallRecord_ORIGByCallID(CallRecordAuthorizeCode, model.CallID.Value, ref msg);
                    if (old_model == null)
                    {
                        Loger.Log4Net.Info("[CallRecordHelper][AddCallRecordNew] 不存在：InsertCallRecord");
                        isOK = crService.InsertCallRecord(CallRecordAuthorizeCode, model, ref msg);
                        Loger.Log4Net.Info("[CallRecordHelper][AddCallRecordNew] InsertCallRecord：" + isOK + " " + msg);
                    }
                    else isOK = true;
                }
                return isOK;
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("[CallRecordHelper][AddCallRecordNew]", ex);
                return false;
            }
        }

        /// 是否黑名单
        /// <summary>
        /// 是否黑名单
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool CheckPhoneAndTelIsInBlackList(string phone)
        {
            try
            {
                return crService.CheckPhoneAndTelIsInBlackList(CallRecordAuthorizeCode, BlackListCheckType.BT2_CC, phone);
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("[CallRecordHelper][CheckPhoneAndTelIsInBlackList]", ex);
                return true;
            }
        }
    }
}
