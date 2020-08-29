using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CallRecordService
{
    /// <summary>
    /// CallRecordService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class CallRecordService : System.Web.Services.WebService
    {
        [WebMethod(Description = "新增话务数据")]
        public bool InsertCallRecord(string Verifycode, Entities.CallRecord_ORIG model, ref string msg)
        {
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "存储话务数据，授权失败。"))
            {
                CallRecord_ORIG model_ORIG = null;
                model_ORIG = (model != null && model.CallID != null ? BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGByCallID(Int64.Parse(model.CallID.ToString())) : null);

                if ((model == null || (model != null && model.CallID == null)) && model_ORIG == null)
                {
                    msg = "参数CallRecord_ORIG的Model为空";
                    BLL.Loger.Log4Net.Info(msg);
                    return false;
                }

                if (model != null && string.IsNullOrEmpty(model.SwitchINNum) && !string.IsNullOrEmpty(model.SkillGroup))
                {
                    //落地号码不存在，技能组存在，反查落地号码
                    DataTable dt = BitAuto.ISDC.CC2012.BLL.CallDisplay.Instance.GetCallDisplayByManufacturerSGID(model.SkillGroup);
                    if (dt.Rows.Count > 0)
                    {
                        model.SwitchINNum = CommonFunction.ObjectToString(dt.Rows[0]["AreaCode"]) + CommonFunction.ObjectToString(dt.Rows[0]["TelMainNum"]);
                    }
                }

                /*根据model.CallID，插入记录到表CallIDMapping中，若CallID和CurrentTime内容存在，则不做插入操作*/
                if (model != null && model_ORIG == null)
                {
                    BLL.Loger.Log4Net.Info("[CallRecordService.asmx]InsertCallRecord ...InsertCallRecord_ORIG...CallID:" + model.CallID);
                    InsertCallRecord_ORIG(model);
                }
                else if (model != null && model_ORIG != null)
                {
                    BLL.Loger.Log4Net.Info("[CallRecordService.asmx]InsertCallRecord ...UpdateCallRecord_ORIG...CallID:" + model.CallID);
                    UpdateCallRecord_ORIG(model);
                }
                else
                {
                    BLL.Loger.Log4Net.Info("条件不满足，无法入库！");
                }
                return true;
            }
            else
            {
                BLL.Loger.Log4Net.Info("存储话务数据，授权失败：" + msg);
            }
            return false;
        }

        [WebMethod(Description = "新增话务数据WB")]
        public bool InsertCallRecordWB(string Verifycode, Entities.CallRecord_ORIG model, ref string msg)
        {
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "存储话务数据，授权失败。"))
            {
                CallRecord_ORIG model_ORIG = null;
                model_ORIG = (model != null && model.CallID != null ? BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGByCallID(Int64.Parse(model.CallID.ToString())) : null);

                if ((model == null || (model != null && model.CallID == null))
                    && model_ORIG == null)
                {
                    msg = "参数CallRecord_ORIG的Model为空"; return false;
                }
                /*根据model.CallID，插入记录到表CallIDMapping中，若CallID和CurrentTime内容存在，则不做插入操作*/
                if (model != null && model_ORIG == null)
                {
                    InsertCallRecord_ORIGWB(model);
                    //if (model.CallStatus == 2)
                    //    InsertCallRecord_ORIG_Business(model);
                }
                else if (model != null && model_ORIG != null)
                {
                    UpdateCallRecord_ORIG(model);
                }
                return true;
            }
            return false;
        }

        [WebMethod(Description = "根据CALLID返回话务实体")]
        public Entities.CallRecord_ORIG GetCallRecord_ORIGByCallID(string Verifycode, Int64 callid, ref string msg)
        {
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "根据CALLID返回话务实体，授权失败。"))
            {
                Entities.CallRecord_ORIG model = BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGByCallID(callid);
                return model;
            }
            return null;
        }

        [WebMethod(Description = "校验电话号码是否黑名单")]
        public bool CheckPhoneAndTelIsInBlackList(string Verifycode, BlackListCheckType blackListCheckType, string phone)
        {
            string msg = "";
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "校验电话号码是否黑名单，授权失败。"))
            {
                return BLL.BlackWhiteList.Instance.CheckPhoneAndTelIsInBlackList(blackListCheckType, phone);
            }
            //没有权限，返回是黑名单
            return true;
        }

        #region 插入

        private int InsertCallRecord_ORIG(CallRecord_ORIG model)
        {
            //int userID;

            //if (int.TryParse(model.CreateUserID.ToString(), out userID))
            //{
            //    try
            //    {
            //        BLL.Loger.Log4Net.Info("[CallRecordService.asmx]GetEmployeeDomainAccountByEid" + userID);
            //        string domainAccount = BLL.Util.GetEmployeeDomainAccountByEid(userID);
            //        //根据域账号找到权限系统该人员的主键
            //        BLL.Loger.Log4Net.Info("[CallRecordService.asmx]GetUserIDByNameDomainAccount" + domainAccount);
            //        model.CreateUserID = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetUserIDByNameDomainAccount(domainAccount);
            //    }
            //    catch
            //    {
            //    }
            //}

            BLL.Loger.Log4Net.Info("[CallRecordService.asmx]InsertCallRecord_ORIG" + model.CallID);
            int retval = 0;
            retval = BitAuto.ISDC.CC2012.BLL.CallRecord_ORIG.Instance.Insert(model);

            if (retval > 0)
            {
                BLL.Loger.Log4Net.Info("[CallRecordService.asmx]InsertCallRecord_ORIG ...话务中间表保存...CallID:" + model.CallID);
                InsertCallRecord_ORIG_Business(model);
            }
            else
            {
                BLL.Loger.Log4Net.Info("[CallRecordService.asmx]InsertCallRecord_ORIG ...失败...");
            }

            return retval;
        }

        private int InsertCallRecord_ORIGWB(CallRecord_ORIG model)
        {
            int retval = 0;
            retval = BitAuto.ISDC.CC2012.BLL.CallRecord_ORIG.Instance.Insert(model);

            if (retval > 0)
            {
                //if (model.OutBoundType == 2)//客户端外呼
                //{
                //    BLL.Loger.Log4Net.Info("[CallRecordService.asmx]InsertCallRecord_ORIG WB ...客户端外呼话务保存...");
                //    InsertCallRecord_ORIG_Business(model);
                //}

                BLL.Loger.Log4Net.Info("[CallRecordService.asmx]InsertCallRecord_ORIG WB ...话务中间表保存...");
                InsertCallRecord_ORIG_Business(model);
            }
            else
            {
                BLL.Loger.Log4Net.Info("[CallRecordService.asmx]InsertCallRecord_ORIG WB...失败...");
            }

            return retval;
        }

        #endregion

        #region 插入
        private int InsertCallRecord_ORIG_Business(CallRecord_ORIG model)
        {
            BLL.Loger.Log4Net.Info("[CallRecordService.asmx]InsertCallRecord_ORIG_Business BEGIN...");

            int retval = 0;
            string str = "";
            str = BitAuto.ISDC.CC2012.BLL.AgentTimeState.Instance.GetBGNameAndOutNum(Convert.ToInt32(model.CreateUserID));
            int bgid = 0;
            if (!string.IsNullOrEmpty(str))
            {
                string[] strArray = str.Split(',');
                bgid = Convert.ToInt32(strArray[0]);

                CallRecord_ORIG_Business bmodel = new CallRecord_ORIG_Business();
                bmodel.CreateUserID = model.CreateUserID;
                bmodel.CreateTime = model.CreateTime;
                bmodel.CallID = Convert.ToInt64(model.CallID);
                bmodel.BGID = bgid;
                //查询现在表
                if (!BLL.CallRecord_ORIG_Business.Instance.IsExistsByCallID(Convert.ToInt64(model.CallID)))
                {
                    BLL.Loger.Log4Net.Info("[CallRecordService.asmx]InsertCallRecord_ORIG_Business ...中间表数据不存在...CallID:" + model.CallID);
                    retval = BitAuto.ISDC.CC2012.BLL.CallRecord_ORIG_Business.Instance.Insert(bmodel);
                }
                else
                {
                    BLL.Loger.Log4Net.Info("[CallRecordService.asmx]InsertCallRecord_ORIG_Business ...中间表数据存在...CallID:" + model.CallID);
                }
            }
            else
            {
                BLL.Loger.Log4Net.Info("[CallRecordService.asmx]InsertCallRecord_ORIG_Business ...当前用户所属业务组为空");
            }

            return retval;
        }
        //此方法是统计外呼失败坐席所属业务组添加的，经分析后觉得不合理弃用
        /*
        private int InsertCallRecord_ORIG_Business(CallRecord_ORIG model)
        {
            BLL.Loger.Log4Net.Info("InsertCallRecord_ORIG_Business BEGIN...");
            int userID;

            if (int.TryParse(model.CreateUserID.ToString(), out userID))
            {
                string domainAccount = BLL.Util.GetEmployeeDomainAccountByEid(userID);
                //根据域账号找到权限系统该人员的主键
                model.CreateUserID = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetUserIDByNameDomainAccount(domainAccount);
            }

            string bgids = "";
            bgids = BitAuto.ISDC.CC2012.BLL.AgentTimeState.Instance.GetUserGroupIDsStr(userID);
            BLL.Loger.Log4Net.Info("InsertCallRecord_ORIG_Business bgids...IS:" + bgids);
            string[] array = bgids.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                CallRecord_ORIG_Business bmodel = new CallRecord_ORIG_Business();
                bmodel.CreateUserID = model.CreateUserID;
                bmodel.CreateTime = model.CreateTime;
                bmodel.CallID = Convert.ToInt64(model.CallID);
                bmodel.BGID = Convert.ToInt32(array[i]);
                BitAuto.ISDC.CC2012.BLL.CallRecord_ORIG_Business.Instance.Insert(bmodel);
            }
            return 1;
        }
        */
        #endregion

        #region 修改

        private int UpdateCallRecord_ORIG(CallRecord_ORIG model)
        {
            BLL.Loger.Log4Net.Info("[CallRecordService.asmx]UpdateCallRecord_ORIG" + model.CallID);
            return BitAuto.ISDC.CC2012.BLL.CallRecord_ORIG.Instance.Update(model);
        }

        #endregion


    }
}
