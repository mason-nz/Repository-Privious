using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.Utils.Config;
using System.Text;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace BitAuto.ISDC.CC2012.Web.CallRecordService
{
    /// <summary>
    /// GetCallRecordList 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class GetCallRecordList : System.Web.Services.WebService
    {
        [WebMethod(Description = "获取话务数据，最大不能超过1万条")]
        public DataTable GetCallRecordByMaxID(string Verifycode, int maxID, ref string msg)
        {
            DataTable dt = null;
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "查询话务数据，授权失败。"))
            {
                dt = BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGByMaxID(maxID);
            }
            return dt;
        }

        [WebMethod(Description = "根据CallID，更新业务数据，businessID为业务ID，BGID为分组ID，SCID为分类ID")]
        public int UpdateBusinessDataByCallID(string Verifycode, Int64 callID, string businessID, int BGID, int SCID, int createuserid, ref string msg)
        {
            BLL.Loger.Log4Net.Info("准备验证：");
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "插入话务与任务关联数据，授权失败。"))
            {
                BLL.Loger.Log4Net.Info("验证通过");
                #region 根据UserID获取BGID、SCID
                string sbgid = "";
                string sscid = "";
                string errormsg = "";
                BLL.Util.GetBIGDSCID_UserID(createuserid.ToString(), BGID.ToString(), SCID.ToString(), out sbgid, out sscid, out errormsg);
                int bgid = 0;
                int scid = 0;
                if (string.IsNullOrEmpty(errormsg))
                {
                    if (Int32.TryParse(sbgid, out bgid) && Int32.TryParse(sscid, out scid))
                    {
                        BGID = bgid;
                        SCID = scid;
                    }

                }
                else
                {
                    BLL.Loger.Log4Net.Info("[根据CallID，更新业务数据出错]" + errormsg);
                }
                #endregion
                //查询现在表
                if (BLL.CallRecord_ORIG_Business.Instance.IsExistsByCallID(callID))
                {
                    BLL.Loger.Log4Net.Info("在系统中已经存在业务数据，更新！callID=" + callID);
                    msg = string.Format("CallID:{0},在系统中已经存在业务数据", callID);
                    //外呼时要记录当前坐席所属业务组
                    //初始化是新建记录，但在业务操作页，最后保存时需更新记录
                    BLL.Loger.Log4Net.Info("准备更新数据：");
                    Entities.CallRecord_ORIG_Business model = new CallRecord_ORIG_Business();
                    model.CallID = callID;
                    model.BGID = BGID;
                    model.SCID = SCID;
                    model.BusinessID = businessID;
                    model.CreateUserID = createuserid;
                    model.CreateTime = DateTime.Now;
                    int recID = BLL.CallRecord_ORIG_Business.Instance.Update(model);
                    BLL.Loger.Log4Net.Info("更新完毕：");
                    return recID;
                }
                else
                {
                    BLL.Loger.Log4Net.Info("准备插入数据：");
                    Entities.CallRecord_ORIG_Business model = new CallRecord_ORIG_Business();
                    model.CallID = callID;
                    model.BGID = BGID;
                    model.SCID = SCID;
                    model.BusinessID = businessID;
                    model.CreateUserID = createuserid;
                    model.CreateTime = DateTime.Now;
                    int recID = BLL.CallRecord_ORIG_Business.Instance.Insert(model);

                    model.RecID = recID;

                    string descMsg = string.Empty;
                    BitAuto.ISDC.CC2012.BLL.GetLogDesc.getAddLogInfo(model, out descMsg);
                    BLL.Loger.Log4Net.Info(descMsg);
                    BLL.Loger.Log4Net.Info("插入完毕：");

                    return recID;

                }
            }
            else
            {
                BLL.Loger.Log4Net.Info("验证失败！msg=" + msg);
            }
            return -1;
        }

        [WebMethod(Description = "根据域账号，返回呼叫中心系统当前用户角色信息")]
        public string GetAgentRoleNameByDomainAccount(string Verifycode, string domainAccount)
        {
            string roleNames = "";
            string roleIDs = ""; string msg = string.Empty;
            BLL.Loger.Log4Net.Info("准备验证：");
            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "根根据域账号，返回呼叫中心系统当前用户角色信息，授权失败。"))
            {
                string ss = ConfigurationUtil.GetAppSettingValue("ThisSysID");
                int userid = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetUserIDByNameDomainAccount(domainAccount);
                if (userid <= 0)
                {
                    msg = "根据域账号,找不到有效的UserID"; //return string.Empty;
                }
                DataTable db = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.GetRoleInfoByUserIDAndSysID(userid,
                    ConfigurationUtil.GetAppSettingValue("ThisSysID"));//BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ThisSysID")
                if (db != null && db.Rows.Count > 0)
                {
                    foreach (DataRow dr in db.Rows)
                    {
                        roleNames += dr["RoleName"].ToString() + ",";
                        roleIDs += dr["RoleID"].ToString() + ",";
                    }
                }
                else
                {
                    msg = "根据域账号,在系统" + ConfigurationUtil.GetAppSettingValue("ThisSysID") + "下，没有找到角色信息"; //return string.Empty;
                }
                if (roleNames.Length > 0)
                {
                    roleNames = roleNames.Substring(0, roleNames.Length - 1);
                }
                if (roleIDs.Length > 0)
                {
                    roleIDs = roleIDs.Substring(0, roleIDs.Length - 1);
                }
            }
            else
            {
                BLL.Loger.Log4Net.Info("验证失败！msg=" + msg);
            }
            return "{\"RoleName\":\"" + roleNames + "\",\"RoleID\":\"" + roleIDs + "\",\"Result\":\"" + msg + "\"}";
        }

        [WebMethod(Description = " 查询客服系统人员信息")]
        public DataTable GetAgentInfoByCondition(string Verifycode, AgentInfoCondition AgentInfoCondition, int PageSize, int PageIndex, ref int RecordCount, ref string msg)
        {
            try
            {
                #region 记接口调用日志

                StringBuilder infoSb = new StringBuilder();
                infoSb.Append("调用接口GetAgentInfoByCondition！");
                infoSb.Append(";验证码：" + Verifycode);
                if (AgentInfoCondition == null)
                {
                    infoSb.Append(";条件为空;");
                }
                else
                {
                    infoSb.Append(";条件：{LoginUserID:" + AgentInfoCondition.LoginUserID + ",UserID:" + AgentInfoCondition.UserID + ",TrueName:" + AgentInfoCondition.TrueName + ",ADName:" + AgentInfoCondition.ADName + "},");
                }
                infoSb.Append(";PageSize;" + PageSize);
                infoSb.Append(";PageIndex;" + PageIndex);
                BLL.Loger.Log4Net.Info(infoSb.ToString());
                #endregion

                RecordCount = 0;
                if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "根据当前登录者ID或真实姓名，查询坐席信息，授权失败。"))
                {
                    QueryEmployeeSuper query = new QueryEmployeeSuper();
                    if (AgentInfoCondition != null)
                    {
                        if (AgentInfoCondition.LoginUserID > 0)
                        {
                            Entities.EmployeeAgent model = BLL.EmployeeAgent.Instance.GetEmployeeAgentByUserID(AgentInfoCondition.LoginUserID);
                            if (model != null && model.BGID != null && model.BGID > 0)
                            {
                                query.BGID = model.BGID.Value;
                            }
                        }
                        if (AgentInfoCondition.BGID > 0)
                        {
                            query.BGID = AgentInfoCondition.BGID;
                        }
                        if (!string.IsNullOrEmpty(AgentInfoCondition.TrueName))
                        {
                            query.TrueName = AgentInfoCondition.TrueName.Trim();
                        }
                        if (AgentInfoCondition.UserID > 0)
                        {
                            query.UserID = AgentInfoCondition.UserID;
                        }
                        if (AgentInfoCondition.ADName != "")
                        {
                            query.ADName = AgentInfoCondition.ADName.Trim();
                        }
                        query.OnlyCCDepart = true;
                    }
                    query.PartIDType = "PartID";
                    DataTable dt = BLL.EmployeeSuper.Instance.GetEmployeeSuper(query, "", PageIndex, PageSize, out RecordCount);
                    return dt;
                }
                else
                {
                    BLL.Loger.Log4Net.Info("GetAgentInfoByCondition验证失败！msg=" + msg);
                    return null;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                BLL.Loger.Log4Net.Info("GetAgentInfoByCondition运行报错！msg=" + ex.StackTrace);
                return null;
            }
        }

        [WebMethod(Description = "插入客户信息")]
        public EnumResult InsertCustData(string Verifycode, string jsonDataStr)
        {
            BLL.Loger.Log4Net.Info("准备验证：");
            string msg = string.Empty;

            if (BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref msg, "插入话务与任务关联数据，授权失败。"))
            {
                BLL.Loger.Log4Net.Info("验证通过");
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                CustBussiness info = null;
                try
                {
                    BLL.Loger.Log4Net.Info("Json串：" + jsonDataStr);
                    info = serializer.Deserialize<CustBussiness>(jsonDataStr);
                    BLL.Loger.Log4Net.Info("Json格式转换成功");
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Info("Json格式转换失败，失败原因：" + ex.Message);
                    return EnumResult.JsonPatternError;
                }

                Entities.CallRecord_ORIG orig;
                BitAuto.YanFa.Crm2009.Entities.DMSMember member;
                EnumResult vefiyResult = VefiyCustData(info, out orig, out member);
                if (vefiyResult == EnumResult.Success)
                {
                    try
                    {
                        //0 获取分组分类
                        SetBGIDAndSCID(info);
                        //1 插入个人信息表                  
                        string custId = string.Empty;
                        string errMsg = string.Empty;
                        int custCategory = 4;
                        if (!string.IsNullOrEmpty(info.MemberCode))
                        {
                            //3-经销商；4-个人
                            custCategory = 3;
                        }
                        if (BLL.CustBasicInfo.Instance.InsertCustInfo(info.CustName, info.Tels, CommonFunction.ObjectToInteger(info.Sex), (int)orig.CreateUserID, custCategory, null, null, out errMsg, out custId))
                        {
                            BLL.Loger.Log4Net.Info("插入客户成功");
                            //删除经销商
                            BLL.DealerInfo.Instance.Delete(custId);

                            if (!string.IsNullOrEmpty(info.MemberCode))
                            {
                                //插入
                                Entities.DealerInfo model_Dealer = new Entities.DealerInfo();
                                model_Dealer.CustID = custId;
                                model_Dealer.MemberCode = info.MemberCode;
                                model_Dealer.Name = member.Name;
                                model_Dealer.Status = 0;
                                model_Dealer.CreateTime = DateTime.Now;
                                model_Dealer.CreateUserID = (int)orig.CreateUserID;
                                BLL.DealerInfo.Instance.Insert(model_Dealer);
                                BLL.Loger.Log4Net.Info("插入经销商信息成功");
                            }

                            //2 插入访问记录
                            string tel = BLL.Util.HaoMaProcess(orig.ANI);
                            long callid = CommonFunction.ObjectToLong(info.CallID, -2);
                            int businesstype = (int)VisitBusinessTypeEnum.S0_其他系统;
                            int tasksource = orig.CallStatus == 2 ? 2 : 1;
                            BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo.OperPopCustBasicInfo.OperCustVisitBusiness(custId, info.BusinessID, callid, businesstype, tasksource, (int)orig.CreateUserID, tel);

                            //3 插入话务业务表
                            UpdateBusinessDataByCallID(info);

                            //插入来去电表
                            CallRecordInfoInfo recordInfo = new CallRecordInfoInfo();
                            recordInfo.CallID = long.Parse(info.CallID);
                            recordInfo.SCID = int.Parse(info.SCID);
                            recordInfo.TaskID = info.BusinessID;
                            recordInfo.TaskTypeID = (int)ProjectSource.None;
                            recordInfo.BGID = int.Parse(info.BGID);
                            recordInfo.CustID = custId;
                            recordInfo.CustName = info.CustName;
                            recordInfo.Contact = info.CustName;
                            long recId = 0;
                            BLL.CallRecordInfo.Instance.InsertCallRecordInfoToHuiMaiChe(recordInfo, orig, out recId);
                            return EnumResult.Success;
                        }
                        else
                        {
                            return EnumResult.Fail;
                        }
                    }
                    catch (Exception ex)
                    {
                        BLL.Loger.Log4Net.Error("【插入客户信息+话务】", ex);
                        return EnumResult.Fail;
                    }
                }
                else
                {
                    BLL.Loger.Log4Net.Info("Json验证失败：msg=" + vefiyResult.ToString());
                    return vefiyResult;
                }
            }
            else
            {
                BLL.Loger.Log4Net.Info("验证失败！msg=" + msg);
                return EnumResult.VerifyError;
            }
        }

        private static void SetBGIDAndSCID(CustBussiness info)
        {
            string bgid = "";
            string scid = "";
            string errormsg = "";
            BLL.Util.GetBIGDSCID_UserID(info.CreateUserID, info.BGID, info.SCID, out bgid, out scid, out errormsg);

            if (string.IsNullOrEmpty(errormsg) && (!string.IsNullOrEmpty(bgid) && !string.IsNullOrEmpty(scid)))
            {
                info.BGID = bgid;
                info.SCID = scid;
            }
            else
            {
                BLL.Loger.Log4Net.Info(errormsg);
            }
        }

        [WebMethod(Description = "插入客户信息")]
        public string InsertCustDataReturnStr(string Verifycode, string jsonDataStr)
        {
            return Utils.EnumHelper.GetEnumTextValue(InsertCustData(Verifycode, jsonDataStr));
        }

        private EnumResult VefiyCustData(CustBussiness info, out Entities.CallRecord_ORIG orig, out BitAuto.YanFa.Crm2009.Entities.DMSMember member)
        {
            orig = null;
            member = null;

            if (string.IsNullOrEmpty(info.CustName))
            {
                return EnumResult.CustNameEmpty;
            }
            int sex = 0;
            if (info.Sex != string.Empty && !int.TryParse(info.Sex, out sex))
            {
                return EnumResult.SexPatternError;
            }
            else if (info.Sex == string.Empty)
            {
                info.Sex = "1";
            }
            else if (sex > 2 || sex < 1)
            {
                return EnumResult.SexPatternError;
            }
            if (info.Tels == null)
            {
                return EnumResult.CustTelEmpty;
            }
            else if (info.Tels.Length == 0)
            {
                return EnumResult.CustTelEmpty;
            }
            else if (info.Tels.Length > 2)
            {
                return EnumResult.CustTelPatternError;
            }
            //Regex reTel = new Regex(@"(^0[0-9]{2,3}[0-9]{7,8}$)|(^13[0-9]{9}$)|(^15[0-9]{9}$)|(^18[0-9]{9}$)|(^19[0-9]{9}$)|(^14[0-9]{9}$)|(^400\d{7}$)");
            foreach (string str in info.Tels)
            {
                if (!BLL.Util.IsTelephoneAnd400Tel(str))
                {
                    return EnumResult.CustTelPatternError;
                }
            }

            int temp = 0;
            long templong = 0;
            if (string.IsNullOrEmpty(info.CallID))
            {
                return EnumResult.CallIDEmpty;
            }
            else if (!long.TryParse(info.CallID, out templong))
            {
                return EnumResult.CallIDPatternError;
            }
            else
            {
                orig = BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGByCallID(long.Parse(info.CallID));
                if (orig == null)
                {
                    return EnumResult.CallIDNotExist;
                }
                else
                {
                    info.CreateUserID = orig.CreateUserID.ToString();
                }
            }
            if (string.IsNullOrEmpty(info.BusinessID))
            {
                return EnumResult.BusinessIDEmpty;
            }
            if (string.IsNullOrEmpty(info.BGID))
            {
                return EnumResult.BGIDEmpty;
            }
            if (!int.TryParse(info.BGID, out temp))
            {
                return EnumResult.BGIDPatternError;
            }
            if (string.IsNullOrEmpty(info.SCID))
            {
                return EnumResult.SCIDEmpty;
            }
            if (!int.TryParse(info.SCID, out temp))
            {
                return EnumResult.SCIDPatternError;
            }
            if (!string.IsNullOrEmpty(info.MemberCode))
            {
                member = BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMemberByMemberCode(info.MemberCode);
                if (member == null)
                {
                    return EnumResult.MemberCodeError;
                }
            }
            return EnumResult.Success;
        }

        private void UpdateBusinessDataByCallID(CustBussiness info)
        {
            long callID = long.Parse(info.CallID);
            int BGID = int.Parse(info.BGID);
            int SCID = int.Parse(info.SCID);
            int createuserid = int.Parse(info.CreateUserID);
            //查询现在表
            if (BLL.CallRecord_ORIG_Business.Instance.IsExistsByCallID(callID))
            {
                BLL.Loger.Log4Net.Info("在系统中已经存在业务数据，更新！callID=" + callID);
                //外呼时要记录当前坐席所属业务组
                //初始化是新建记录，但在业务操作页，最后保存时需更新记录
                BLL.Loger.Log4Net.Info("准备更新数据：");
                Entities.CallRecord_ORIG_Business model = new CallRecord_ORIG_Business();
                model.CallID = callID;
                model.BGID = BGID;
                model.SCID = SCID;
                model.BusinessID = info.BusinessID;
                model.CreateUserID = createuserid;
                model.CreateTime = DateTime.Now;
                int recID = BLL.CallRecord_ORIG_Business.Instance.Update(model);
                BLL.Loger.Log4Net.Info("更新完毕：");
            }
            else
            {
                BLL.Loger.Log4Net.Info("准备插入数据：");
                Entities.CallRecord_ORIG_Business model = new CallRecord_ORIG_Business();
                model.CallID = callID;
                model.BGID = BGID;
                model.SCID = SCID;
                model.BusinessID = info.BusinessID;
                model.CreateUserID = createuserid;
                model.CreateTime = DateTime.Now;
                int recID = BLL.CallRecord_ORIG_Business.Instance.Insert(model);

                model.RecID = recID;

                string descMsg = string.Empty;
                BitAuto.ISDC.CC2012.BLL.GetLogDesc.getAddLogInfo(model, out descMsg);
                BLL.Loger.Log4Net.Info(descMsg);
                BLL.Loger.Log4Net.Info("插入完毕：");
            }
        }


        [WebMethod(Description = "获取话务数据接口（易集客调用）")]
        public DataTable GetDemandCallDetail(string Verifycode, CallRecordInfoCondition model, int PageIndex, int PageSize, out int RecordCount, out string Msg)
        {
            RecordCount = 0;
            Msg = string.Empty;
            DataTable dt = new DataTable("callRecordInfo");
            string[] arr = new string[7] { "CallID", "BeginDateTime", "EndDateTime", "CreateUserID", "AudioURL", "EstablishedTime", "Content" };
            for (var i = 0; i < arr.Length; i++)
            {
                dt.Columns.Add(arr[i]);
            }

            try
            {
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref Msg, "易集客调用获取话务数据，授权失败。"))
                {
                    BLL.Loger.Log4Net.Info("【拉取需求单客户通知接口】出错：验证失败，" + Msg);
                    return dt;
                }

                //验证
                if (model.BGID == 0 || model.SCID == 0 || model.BusinessID == string.Empty)
                {
                    Msg = "业务组ID、分类ID和业务ID不能为空！";
                    BLL.Loger.Log4Net.Info("【易集客获取话务数据接口】出错：" + Msg);
                    return dt;
                }

                string where = " And CallID in (select CallID From CallRecord_ORIG_Business cob where cob.BGID=" + model.BGID + " and cob.SCID=" + model.SCID + " and BusinessID='" + model.BusinessID + "')";

                DataTable dt_ORIG = BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGByYiJiKe(where, " InitiatedTime desc ", PageIndex, PageSize, out RecordCount);
                for (int i = 0, len = dt_ORIG.Rows.Count; i < len; i++)
                {
                    string endTime = dt_ORIG.Rows[i]["CustomerReleaseTime"].ToString();
                    if (string.IsNullOrEmpty(endTime))
                    {
                        endTime = dt_ORIG.Rows[i]["AgentReleaseTime"].ToString();
                    }
                    DateTime time1;
                    DateTime time2;
                    if (DateTime.TryParse(dt_ORIG.Rows[i]["CustomerReleaseTime"].ToString(), out time1) && DateTime.TryParse(dt_ORIG.Rows[i]["AgentReleaseTime"].ToString(), out time2))
                    {
                        if (time1 > time2)
                        {
                            endTime = dt_ORIG.Rows[i]["AgentReleaseTime"].ToString();
                        }
                    }
                    DataRow dr = dt.NewRow();
                    dr["CallID"] = dt_ORIG.Rows[i]["CallID"].ToString();
                    dr["BeginDateTime"] = dt_ORIG.Rows[i]["InitiatedTime"].ToString();
                    dr["EndDateTime"] = endTime;
                    dr["CreateUserID"] = dt_ORIG.Rows[i]["CreateUserID"].ToString();
                    dr["AudioURL"] = dt_ORIG.Rows[i]["AudioURL"].ToString();
                    dr["EstablishedTime"] = dt_ORIG.Rows[i]["EstablishedTime"].ToString();
                    dr["Content"] = dt_ORIG.Rows[i]["Content"].ToString();
                    dt.Rows.Add(dr);
                }

            }
            catch (Exception ex)
            {
                Msg = ex.Message;
                BLL.Loger.Log4Net.Info("【易集客获取话务数据接口】出错：" + Msg);
                return dt;
            }

            BLL.Loger.Log4Net.Info("【易集客获取话务数据接口】成功，BGID：" + model.BGID + "，SCID：" + model.SCID + "，BusinessID：" + model.BusinessID + "，总记录数：" + RecordCount);
            return dt;

        }

        [WebMethod(Description = "获取最近一次外呼初始化时间（惠买车调用）")]
        public string GetPhoneLastestInitiatedTime(string Verifycode, string phoneNumber, out string Msg)
        {
            BLL.Loger.Log4Net.Info("【调用获取最近一次外呼初始化时间】");

            BLL.Loger.Log4Net.Info(Verifycode + ".............................................................................................................................................");
            Msg = string.Empty;
            var blResult = BLL.CallRecord_ORIG_Authorizer.Instance.Verify(Verifycode, 0, ref Msg, "惠买车获取最近外呼初始化时间错误，授权失败。");
            BLL.Loger.Log4Net.Info(blResult);
            if (!blResult)
            {
                BLL.Loger.Log4Net.Info("【获取最近一次外呼初始化时间】出错：验证失败，" + Msg);
                return string.Empty;
            }
            try
            {
                BLL.Loger.Log4Net.Info("验证通过");
                return BLL.CallRecord_ORIG.Instance.GetPhoneLastestInitiatedTime(phoneNumber);
            }
            catch (Exception ex)
            {
                Msg = ex.Message;
                BLL.Loger.Log4Net.Info("【获取最近一次外呼初始化时间】出错：" + Msg);
            }
            return "";
        }

    }

    [Serializable]
    public class CustBussiness
    {
        private string _custname = "";
        private string _sex = "";
        private string[] tels = null;
        private string _callid = "";
        private string _businessid = "";
        private string _bgid = "";
        private string _scid = "";
        private string _createuserid = "";
        private string _recordtype = "";
        private string _membercode = "";
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustName
        {
            get { return HttpUtility.UrlDecode(_custname); }
            set { _custname = value; }
        }
        /// <summary>
        /// 客户性别
        /// </summary>
        public string Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }
        /// <summary>
        /// 客户电话
        /// </summary>
        public string[] Tels
        {
            get { return tels; }
            set { tels = value; }
        }
        /// <summary>
        /// 录音ID
        /// </summary>
        public string CallID
        {
            get { return _callid; }
            set { _callid = value; }
        }

        /// <summary>
        /// 分组ID
        /// </summary>
        public string BGID
        {
            get { return _bgid; }
            set { _bgid = value; }
        }
        /// <summary>
        /// 分类ID
        /// </summary>
        public string SCID
        {
            get { return _scid; }
            set { _scid = value; }
        }
        /// <summary>
        /// 创建者ID
        /// </summary>
        public string CreateUserID
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }

        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusinessID
        {
            get { return _businessid; }
            set { _businessid = value; }
        }

        /// <summary>
        /// 呼入类型
        /// </summary>
        public string RecordType
        {
            get { return _recordtype; }
            set { _recordtype = value; }
        }

        /// <summary>
        /// 会员编号
        /// </summary>
        public string MemberCode
        {
            get { return _membercode; }
            set { _membercode = value; }
        }
    }

    /// <summary>
    /// 插入客户结果
    /// </summary>
    [Serializable]
    public enum EnumResult
    {
        [BitAuto.Utils.EnumTextValue("成功")]
        Success = 1,

        [BitAuto.Utils.EnumTextValue("授权失败")]
        VerifyError = 2,

        [BitAuto.Utils.EnumTextValue("json字符串格式不正确")]
        JsonPatternError = 3,

        [BitAuto.Utils.EnumTextValue("客户名称为空")]
        CustNameEmpty = 4,

        [BitAuto.Utils.EnumTextValue("性别格式不正确")]
        SexPatternError = 5,

        [BitAuto.Utils.EnumTextValue("客户电话为空")]
        CustTelEmpty = 6,

        [BitAuto.Utils.EnumTextValue("客户电话格式不正确")]
        CustTelPatternError = 7,

        [BitAuto.Utils.EnumTextValue("录音ID为空")]
        CallIDEmpty = 8,

        [BitAuto.Utils.EnumTextValue("录音ID格式不正确")]
        CallIDPatternError = 9,

        [BitAuto.Utils.EnumTextValue("录音ID不存在")]
        CallIDNotExist = 10,

        [BitAuto.Utils.EnumTextValue("业务ID为空")]
        BusinessIDEmpty = 11,

        [BitAuto.Utils.EnumTextValue("分组ID为空")]
        BGIDEmpty = 12,

        [BitAuto.Utils.EnumTextValue("分组ID必须是整数")]
        BGIDPatternError = 13,

        [BitAuto.Utils.EnumTextValue("分类ID为空")]
        SCIDEmpty = 14,

        [BitAuto.Utils.EnumTextValue("分类ID必须是整数")]
        SCIDPatternError = 15,

        [BitAuto.Utils.EnumTextValue("不存在此会员编号")]
        MemberCodeError = 16,

        [BitAuto.Utils.EnumTextValue("调用接口失败")]
        Fail = 0
    }
    /// <summary>
    /// 业务组
    /// </summary>
    [Serializable]
    public enum EnumBusinessGroup
    {
        [BitAuto.Utils.EnumTextValue("数据清洗组")]
        DataCleanGroup = 2,

        [BitAuto.Utils.EnumTextValue("4S电话营销组")]
        TeleMarking = 6,

        [BitAuto.Utils.EnumTextValue("非4S电话营销组")]
        NotTeleMarking = 7,

        [BitAuto.Utils.EnumTextValue("个人用户服务组")]
        PersonService = 9
    }

    /// <summary>
    /// 业务组对应分类
    /// </summary>
    [Serializable]
    public enum EnumCategory
    {
        [BitAuto.Utils.EnumTextValue("非4S电话营销组-客户回访")]
        TeleCustomerVisit = 62,

        [BitAuto.Utils.EnumTextValue("4S电话营销组-客户回访")]
        NotTeleCustomerVisit = 63,

        [BitAuto.Utils.EnumTextValue("车源审核")]
        CarAudit = 99,

        [BitAuto.Utils.EnumTextValue("数据清洗")]
        DataCleanCategory = 52,

        [BitAuto.Utils.EnumTextValue("个人业务")]
        PersonBusiness = 89,

        [BitAuto.Utils.EnumTextValue("无主订单")]
        NoDealerOrder = 90
    }

    /// <summary>
    /// 查询坐席信息条件类
    /// </summary>
    [XmlInclude(typeof(AgentInfoCondition))]
    public class AgentInfoCondition
    {
        private int _userID = 0;
        private int _loginUserID = 0;
        private int _bgid = 0;
        private string _truename = "";
        private string _adname = "";

        /// <summary>
        /// 按坐席的UserID查询
        /// </summary>
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        /// <summary>
        /// 按指定UserID坐席所在分组查询
        /// </summary>
        public int LoginUserID
        {
            get { return _loginUserID; }
            set { _loginUserID = value; }
        }

        /// <summary>
        /// 按指定业务组ID查询
        /// </summary>
        public int BGID
        {
            get { return _bgid; }
            set { _bgid = value; }
        }

        /// <summary>
        /// 按坐席真实姓名查询
        /// </summary>
        public string TrueName
        {
            get { return _truename; }
            set { _truename = value; }
        }

        /// <summary>
        /// 按坐席域帐号查询
        /// </summary>
        public string ADName
        {
            get { return _adname; }
            set { _adname = value; }
        }

    }

    /// <summary>
    /// 查询话务信息条件类
    /// </summary>
    [XmlInclude(typeof(CallRecordInfoCondition))]
    public class CallRecordInfoCondition
    {
        private int _bgid = 0;
        private int _scid = 0;
        private string _businessid = "";

        /// <summary>
        /// 按业务组查询
        /// </summary>
        public int BGID
        {
            get { return _bgid; }
            set { _bgid = value; }
        }

        /// <summary>
        /// 按分类查询
        /// </summary>
        public int SCID
        {
            get { return _scid; }
            set { _scid = value; }
        }

        /// <summary>
        /// 按业务ID查询
        /// </summary>
        public string BusinessID
        {
            get { return _businessid; }
            set { _businessid = value; }
        }
    }
}