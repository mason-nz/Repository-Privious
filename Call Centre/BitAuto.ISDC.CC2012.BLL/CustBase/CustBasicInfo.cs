using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类CustBasicInfo 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-07-27 10:30:12 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CustBasicInfo
    {
        #region Instance
        public static readonly CustBasicInfo Instance = new CustBasicInfo();
        #endregion

        #region Contructor
        protected CustBasicInfo()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 根据电话获取客户信息
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public DataTable GetCustBasicInfosByTel(string tel)
        {
            return Dal.CustBasicInfo.Instance.GetCustBasicInfosByTel(tel);
        }
        /// <summary>
        /// 根据电话和联系人姓名（模糊匹配）获取客户信息
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public DataTable GetCustBasicInfosByTelAndName(string tel, string custName)
        {
            return Dal.CustBasicInfo.Instance.GetCustBasicInfosByTelAndName(tel, custName);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CustBasicInfo GetCustBasicInfo(string CustID)
        {
            return Dal.CustBasicInfo.Instance.GetCustBasicInfo(CustID);
        }
        /// <summary>
        /// 根据客户名称和电话，查询客户信息
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="custName"></param>
        /// <returns></returns>
        public Entities.CustBasicInfo GetCustBasicInfo(string tel, string custName)
        {
            return Dal.CustBasicInfo.Instance.GetCustBasicInfo(tel, custName);
        }
        #endregion

        /// <summary>
        /// 增加一条数据,返回CustID
        /// </summary>
        public string Insert(Entities.CustBasicInfo model)
        {
            return Dal.CustBasicInfo.Instance.Insert(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.CustBasicInfo model)
        {
            return Dal.CustBasicInfo.Instance.Update(model);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string custID)
        {
            return Dal.CustBasicInfo.Instance.Delete(custID);
        }

        /// 保存信息到个人用户库：接口，服务
        /// <summary>
        /// 保存信息到个人用户库：接口，服务
        /// </summary>
        /// <param name="CustName">客户名称</param>
        /// <param name="Tels">电话号码，允许多个</param>
        /// <param name="Sex">姓别 男：1,女：2</param>
        /// <param name="CreateUserID">创建人</param>
        /// <param name="msg">返回调用结果信息</param>
        /// <param name="CustID">客户号返回值</param>
        public bool InsertCustInfo(string CustName, string[] Tels, int Sex, int CreateUserID, int custCategory, int? update_provinceid, int? update_cityid, out string msg, out string CustID)
        {
            CustID = string.Empty;
            msg = string.Empty;

            try
            {
                #region 验证参数
                if (CustName == string.Empty)
                {
                    msg = "'result':'false','errorMsg':'数据格式错误,姓名不能为空！'";
                    return false;
                }
                else if (Tels.Length == 0)
                {
                    msg = "'result':'false','errorMsg':'数据格式错误,电话不能为空！'";
                    return false;
                }
                else if (Tels.Length > 0)
                {
                    foreach (string tel in Tels)
                    {
                        if (string.IsNullOrEmpty(tel))
                        {
                            msg = "'result':'false','errorMsg':'数据格式错误,电话不能为空！'";
                            return false;
                        }
                    }
                }
                //校验客户类型参数，只能传入3，4类型
                if (custCategory == (int)CustTypeEnum.T01_个人)
                {
                    custCategory = (int)CustTypeEnum.T01_个人;
                }
                else
                {
                    custCategory = (int)CustTypeEnum.T02_经销商;
                }
                #endregion

                CustID = GetMaxNewCustBasicInfoByTel(Tels);
                Entities.CustBasicInfo model = null;
                if (!string.IsNullOrEmpty(CustID))
                {
                    #region 修改操作
                    //更新客户
                    model = BLL.CustBasicInfo.Instance.GetCustBasicInfo(CustID);
                    if (update_provinceid.HasValue)
                    {
                        model.ProvinceID = update_provinceid;
                    }
                    if (update_cityid.HasValue)
                    {
                        model.CityID = update_cityid;
                    }
                    model.CustCategoryID = custCategory;
                    model.ModifyTime = DateTime.Now;
                    model.Sex = Sex;
                    BLL.CustBasicInfo.Instance.Update(model);
                    //更新电话
                    foreach (string tel in Tels)
                    {
                        if (!BLL.CustTel.Instance.IsExistsByCustIDAndTel(CustID, tel))
                        {
                            Entities.CustTel modeltel = new Entities.CustTel();
                            modeltel.CreateTime = DateTime.Now;
                            modeltel.CreateUserID = CreateUserID;
                            modeltel.CustID = CustID;
                            modeltel.Tel = tel;
                            try
                            {
                                BLL.CustTel.Instance.Insert(modeltel);
                            }
                            catch (Exception ex)
                            {
                                msg = "'result':'false','errorMsg':'插入【电话】" + modeltel + "失败！'";
                                Loger.Log4Net.Error("【保存信息到个人用户库：接口】", ex);
                                return false;
                            }
                        }
                    }
                    #endregion
                    msg = "'result':'true','CustID':'" + CustID + "'";
                }
                else
                {
                    #region 新增操作
                    model = new Entities.CustBasicInfo();
                    model.CustName = CustName;
                    model.Sex = Sex;
                    model.CustCategoryID = custCategory;//3-经销商；4-个人；
                    int pID = 0, cID = 0;
                    BLL.PhoneNumDataDict.GetAreaId(Tels[0], out pID, out cID);
                    model.ProvinceID = pID == 0 ? -2 : pID;
                    model.CityID = cID == 0 ? -2 : cID;
                    model.CountyID = -1;
                    model.AreaID = "";//新增和更新时自动计算
                    model.CallTime = 0;
                    model.Status = 0;
                    model.CreateUserID = model.ModifyUserID = CreateUserID;
                    model.CreateTime = model.ModifyTime = DateTime.Now;
                    CustID = BLL.CustBasicInfo.Instance.Insert(model);

                    foreach (string tel in Tels)
                    {
                        Entities.CustTel model_Tel = new Entities.CustTel();
                        model_Tel.CustID = CustID;
                        model_Tel.CreateTime = DateTime.Now;
                        model_Tel.CreateUserID = CreateUserID;
                        model_Tel.Tel = tel;
                        try
                        {
                            BLL.CustTel.Instance.Insert(model_Tel);
                        }
                        catch (Exception ex)
                        {
                            msg = "'result':'false','errorMsg':'插入【电话】" + model_Tel + "失败！";
                            Loger.Log4Net.Error("【保存信息到个人用户库：接口】", ex);
                            return false;
                        }
                    }
                    #endregion
                    msg = "'result':'true','CustID':'" + CustID + "'";
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("【保存信息到个人用户库：接口】", ex);
                msg = "'result':'false','errorMsg':'" + ex.Message + "'";
                return false;
            }
            return true;
        }

        /// 根据电话和姓名取值（为IM系统）
        /// <summary>
        /// 根据电话和姓名取值（为IM系统）
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataTable GetCustBasicInfoForIM(string tel)
        {
            return Dal.CustBasicInfo.Instance.GetCustBasicInfoForIM(tel);
        }
        /// 根据 省 城市 区县 重新计算大区信息
        /// <summary>
        /// 根据 省 城市 区县 重新计算大区信息
        /// qiangfei
        /// 2014-12-17
        /// </summary>
        /// <param name="model"></param>
        public void ReCalcAreaDistrict(Entities.CustBasicInfo model)
        {
            BitAuto.YanFa.Crm2009.Entities.AreaInfo info = Util.GetAreaInfoByPCC(
                CommonFunction.ObjectToString(model.ProvinceID),
                CommonFunction.ObjectToString(model.CityID),
                CommonFunction.ObjectToString(model.CountyID));
            if (info != null)
            {
                model.AreaID = info.District;
            }
            else
            {
                model.AreaID = "";
            }
        }
        /// 根据电话号码查询历史客户（新版） 强斐 2016-4-7
        /// <summary>
        /// 根据电话号码查询历史客户（新版） 强斐 2016-4-7
        /// 按照时间排序，取最新的客户
        /// </summary>
        /// <param name="tels"></param>
        /// <param name="taskid"></param>
        /// <param name="retList"></param>
        public void GetCallRecordORGIHistory(string tels, string taskid, out List<string[]> retList)
        {
            Dal.CustBasicInfo.Instance.GetCallRecordORGIHistory_New(tels, taskid, out retList);
        }
        /// 根据手机号码查询最新的客户ID
        /// <summary>
        /// 根据手机号码查询最新的客户ID
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public string GetMaxNewCustBasicInfoByTel(params string[] tels)
        {
            return Dal.CustBasicInfo.Instance.GetMaxNewCustBasicInfoByTel(tels);
        }

        #region 话务分月关联查询
        /// 查询个人用户信息
        /// <summary>
        /// 查询个人用户信息
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryCallInfo"></param>
        /// <param name="queryDealerInfo"></param>
        /// <param name="queryCustHistoryInfo"></param>
        /// <param name="outField"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCustBasicInfo(QueryCustBasicInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CustBasicInfo.Instance.GetCustBasicInfo(query, order, currentPage, pageSize, out totalCount);
        }
        #endregion

        //废弃，未实现
        public void Update(SqlTransaction tran, Entities.CustBasicInfo custmodel)
        {
            throw new NotImplementedException();
        }
        //废弃，未实现
        public string Insert(SqlTransaction sqltran, Entities.CustBasicInfo model)
        {
            throw new NotImplementedException();
        }
        //废弃，未实现
        public bool IsExistsByCustNameAndTel(string UserName, string Tel1)
        {
            throw new NotImplementedException();
        }
    }
}

