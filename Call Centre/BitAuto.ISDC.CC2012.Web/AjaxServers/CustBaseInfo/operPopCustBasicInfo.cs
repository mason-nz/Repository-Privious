using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustBaseInfo
{
    public class OperPopCustBasicInfo
    {
        public string ccCustID = string.Empty;

        /// 保存个人信息：弹屏，老版工单
        /// <summary>
        /// 保存个人信息：弹屏，老版工单
        /// </summary>
        /// <param name="info"></param>
        /// <param name="msg"></param>
        public void InsertCustInfo(CustBasicInfo info, out string msg)
        {
            msg = string.Empty;
            try
            {
                if (info.CustName == string.Empty || info.Tel == string.Empty)
                {
                    msg = "'result':'false','errorMsg':'数据格式错误！'";
                    return;
                }
                validateData(info.CustName, info.Sex, info.Tel, info.CustCategoryID, out msg);
                if (msg != "")
                {
                    msg = "'result':'false','errorMsg':'" + msg + "'";
                    return;
                }
                //获取个人用户ID
                ccCustID = BLL.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(info.Tel);
                if (!string.IsNullOrEmpty(ccCustID))
                {
                    # region 更新操作
                    Entities.CustBasicInfo model = BLL.CustBasicInfo.Instance.GetCustBasicInfo(ccCustID);
                    model.CustName = info.CustName;

                    int _sex = CommonFunction.ObjectToInteger(info.Sex, -1); //1男 2女
                    int _category = CommonFunction.ObjectToInteger(info.CustCategoryID, -1); //3经销商 4个人
                    if (_sex > 0)
                    {
                        model.Sex = _sex;
                    }
                    if (_category > 0)
                    {
                        model.CustCategoryID = _category;//3-经销商；4-个人；
                    }
                    int pID = 0, cID = 0;
                    BLL.PhoneNumDataDict.GetAreaId(info.Tel, out pID, out cID);
                    model.ProvinceID = pID == 0 ? -2 : pID;
                    model.CityID = cID == 0 ? -2 : cID;
                    model.CountyID = -1;
                    model.AreaID = "";//新增 更新时 自动计算
                    model.CallTime = 0;
                    model.Status = 0;
                    model.ModifyUserID = info.OperID;
                    model.ModifyTime = info.OperTime;
                    BLL.CustBasicInfo.Instance.Update(model);
                    #endregion
                    BLL.Util.InsertUserLog("个人用户表【修改记录】【修改电话信息】成功【电话】" + info.Tel + "【姓名】" + info.CustName + "【客户分类】" + info.CustCategoryID + "【省份ID】" + pID + "【城市ID】" + cID + "【大区】" + model.AreaID + "【操作人】" + info.OperID + "【操作时间】" + info.OperTime);
                    msg = "'result':'true','CustID':'" + ccCustID + "'";
                    return;
                }
                else
                {
                    int _sex = CommonFunction.ObjectToInteger(info.Sex, 1); //1男 2女
                    int _category = CommonFunction.ObjectToInteger(info.CustCategoryID, 4); //3经销商 4个人

                    if (_sex <= 0 || _category <= 0)
                    {
                        msg = "'result':'false','errorMsg':'Sex:" + info.Sex + ",CustCategoryID:" + info.CustCategoryID + ",数据格式出现错误，无法操作！'";
                        return;
                    }

                    #region 插入个人信息表CustBasicInfo
                    Entities.CustBasicInfo model = new Entities.CustBasicInfo();
                    model.CustName = info.CustName;
                    model.Sex = _sex;
                    model.CustCategoryID = _category;//3-经销商；4-个人；
                    int pID = 0, cID = 0;
                    BLL.PhoneNumDataDict.GetAreaId(info.Tel, out pID, out cID);
                    model.ProvinceID = pID == 0 ? -2 : pID;
                    model.CityID = cID == 0 ? -2 : cID;
                    model.CountyID = -1;
                    model.AreaID = "";//新增 更新时 自动计算
                    model.CallTime = 0;
                    model.Status = 0;
                    model.CreateUserID = model.ModifyUserID = info.OperID;
                    model.CreateTime = model.ModifyTime = info.OperTime;
                    //插入个人用户库
                    ccCustID = BLL.CustBasicInfo.Instance.Insert(model);
                    #endregion

                    #region 插入电话信息CustTel
                    Entities.CustTel model_Tel = new Entities.CustTel();
                    model_Tel.CustID = ccCustID;
                    model_Tel.CreateTime = info.OperTime;
                    model_Tel.CreateUserID = info.OperID;
                    model_Tel.Tel = info.Tel;
                    BLL.CustTel.Instance.Insert(model_Tel);
                    #endregion

                    #region 如果客户分类为3-经销商，且经销商ID存在，则插入客户与经销商关联表
                    if (info.CustCategoryID == "3" && !string.IsNullOrEmpty(info.MemberID) && !string.IsNullOrEmpty(info.MemberName))
                    {
                        Entities.DealerInfo model_Dealer = new Entities.DealerInfo();
                        model_Dealer.CustID = ccCustID;
                        model_Dealer.MemberCode = info.MemberID;
                        model_Dealer.Name = info.MemberName;
                        model_Dealer.Status = 0;
                        model_Dealer.CreateTime = info.OperTime;
                        model_Dealer.CreateUserID = info.OperID;
                        BLL.DealerInfo.Instance.Insert(model_Dealer);
                    }
                    #endregion

                    msg = "'result':'true','CustID':'" + ccCustID + "'";
                    BLL.Util.InsertUserLog("个人用户表【新增记录】【插入电话信息】成功【电话】" + info.Tel + "【姓名】" + info.CustName + "【客户分类】" + info.CustCategoryID + "【省份ID】" + pID + "【城市ID】" + cID + "【大区】" + model.AreaID + "【操作人】" + info.OperID + "【操作时间】" + info.OperTime);
                }
            }
            catch (Exception ex)
            {
                BLL.Util.InsertUserLog("个人用户表【维护记录】出错【电话】" + info.Tel + "【姓名】" + info.CustName + "错误信息：" + ex.Message);
                msg = "'result':'false','errorMsg':'" + ex.Message + "'";
            }
        }
        //验证数据格式
        private void validateData(string CustName, string Sex, string Tel, string CustCategoryID, out string errorMsg)
        {
            errorMsg = "";
            if (CustName == "")
            {
                errorMsg += "姓名不能为空！<br/>";
            }
            if (Tel == "")
            {
                errorMsg += "电话不能为空！<br/>";
            }
        }

        /// 验证电话号码在个人用户库是否存在
        /// <summary>
        /// 验证电话号码在个人用户库是否存在
        /// </summary>
        /// <param name="Tel"></param>
        /// <param name="msg"></param>
        public void telIsExists(string Tel, out string msg)
        {
            msg = string.Empty;
            if (Tel == string.Empty)
            {
                msg = "'result':'false'";//数据格式错误
                return;
            }
            string ccCustID = BLL.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(Tel);
            if (!string.IsNullOrEmpty(ccCustID))
            {
                msg = "'result':'true','isExists':'yes','CustID':'" + ccCustID + "'";//存在
            }
            else
            {
                msg = "'result':'true','isExists':'no'";//不存在
            }
        }
        /// 更新个人用户业务访问记录：入口：老版工单，接口
        /// <summary>
        /// 更新个人用户业务访问记录：入口：老版工单，接口
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="taskid"></param>
        /// <param name="callrecid"></param>
        /// <param name="callid"></param>
        /// <param name="businesstype"></param>
        /// <param name="tasksource"></param>
        /// <param name="userid"></param>
        /// <param name="te1"></param>
        /// <param name="tel2"></param>
        public static void OperCustVisitBusiness(string custid, string taskid, long callid, int businesstype, int tasksource, int userid, string te1, string tel2 = null)
        {
            CustPhoneVisitBusinessInfo custphonevisitbusinessinfo = new CustPhoneVisitBusinessInfo();
            custphonevisitbusinessinfo.TaskID = taskid;
            custphonevisitbusinessinfo.BusinessType = businesstype;
            custphonevisitbusinessinfo.TaskSource = tasksource;
            custphonevisitbusinessinfo.CallID = callid < 0 ? null : (long?)callid;
            //保存电话1
            custphonevisitbusinessinfo.PhoneNum = te1;
            BLL.CustPhoneVisitBusiness.Instance.InsertOrUpdateCustPhoneVisitBusiness(custphonevisitbusinessinfo, userid);
            //保存电话2
            if (!string.IsNullOrEmpty(tel2) && callid <= 0)
            {
                custphonevisitbusinessinfo.PhoneNum = tel2;
                BLL.CustPhoneVisitBusiness.Instance.InsertOrUpdateCustPhoneVisitBusiness(custphonevisitbusinessinfo, userid);
            }
        }
    }
}