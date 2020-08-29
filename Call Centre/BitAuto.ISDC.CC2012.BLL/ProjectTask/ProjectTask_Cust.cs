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
    /// 业务逻辑类ProjectTask_Cust 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:30 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ProjectTask_Cust
    {
        #region Instance
        public static readonly ProjectTask_Cust Instance = new ProjectTask_Cust();
        #endregion

        #region Contructor
        protected ProjectTask_Cust()
        { }
        #endregion

        public Entities.ProjectTask_Cust GetProjectTask_Cust(string tid)
        {
            Entities.ProjectTask_Cust c = Dal.ProjectTask_Cust.Instance.GetProjectTask_Cust(tid);
            if (c == null) { return null; }

            //品牌
            int tc;
            QueryProjectTask_Cust_Brand query = new QueryProjectTask_Cust_Brand();
            query.PTID = tid;
            DataTable dt = BLL.ProjectTask_Cust_Brand.Instance.GetProjectTask_Cust_Brand(query, "", 1, 10000, out tc);
            foreach (DataRow dr in dt.Rows)
            {
                c.BrandIDs.Add(int.Parse(dr["BrandID"].ToString()));
                c.BrandNames.Add(dr["Name"].ToString());
            }

            return c;
        }

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        public DataTable GetProjectTask_Cust(QueryProjectTask_Cust queryProjectTask_Cust, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_Cust.Instance.GetProjectTask_Cust(queryProjectTask_Cust, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 这条记录是否存在
        /// </summary>
        public bool Exists(string tid)
        {
            return Dal.ProjectTask_Cust.Instance.Exists(tid);
        }

        /// <summary>
        /// 插入新的记录
        /// </summary>
        private void Insert(Entities.ProjectTask_Cust c)
        {
            c.CreateTime = c.LastUpdateTime = DateTime.Now;
            c.CreateUserID = c.LastUpdateUserID = BLL.Util.GetLoginUserID();
            Dal.ProjectTask_Cust.Instance.Insert(c);
            //主营品牌
            Dal.ProjectTask_Cust_Brand.Instance.UpdataBrandInfoByTID(c.PTID, c.BrandIDs);
        }

        public BitAuto.YanFa.Crm2009.Entities.EnumCustomType? GetCustTypeByName(string typeName)
        {
            switch (typeName)
            {
                case "厂商":
                    return BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Company;
                case "集团":
                    return BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Bloc;
                case "4s":
                    return BitAuto.YanFa.Crm2009.Entities.EnumCustomType.FourS;
                case "特许经销商":
                    return BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Licence;
                case "综合店":
                    return BitAuto.YanFa.Crm2009.Entities.EnumCustomType.SynthesizedShop;
                case "汽车服务商":
                    return BitAuto.YanFa.Crm2009.Entities.EnumCustomType.SP;
                case "其他":
                    return BitAuto.YanFa.Crm2009.Entities.EnumCustomType.Other;
                default:
                    return null;
            }
        }

        private BitAuto.YanFa.Crm2009.Entities.EnumCustomIndustry? GetCustIndustryByName(string name)
        {
            switch (name)
            {
                case "汽车销售":
                    return BitAuto.YanFa.Crm2009.Entities.EnumCustomIndustry.AutoSales;
                case "汽车生产":
                    return BitAuto.YanFa.Crm2009.Entities.EnumCustomIndustry.AutoManufacture;
                case "汽车服务":
                    return BitAuto.YanFa.Crm2009.Entities.EnumCustomIndustry.AutoService;
                default:
                    return null;
            }
        }



        /// <summary>
        /// 将excel中信息同步到ProjectTask_Cust
        /// </summary>
        public void SyncInfoFromExcel(Entities.ProjectTaskInfo task)
        {
            if (task.Source == 1 && BLL.ProjectTask_Cust.Instance.Exists(task.PTID) == false)
            {
                int i = -1;
                if (int.TryParse(task.RelationID, out i) == false) { return; }
                Entities.ExcelCustInfo excelInfo = BLL.ExcelCustInfo.Instance.GetExcelCustInfo(i);
                if (excelInfo == null) { return; }
                //插入基本信息
                Entities.ProjectTask_Cust c = new Entities.ProjectTask_Cust();
                c.PTID = task.PTID;
                c.CustName = excelInfo.CustName;

                BitAuto.YanFa.Crm2009.Entities.EnumCustomType? ct = this.GetCustTypeByName(excelInfo.TypeName);
                c.TypeID = ct.HasValue ? ((int)ct.Value).ToString() : "";

                //Crm2009.Entities.EnumCustomIndustry? ci = this.GetCustIndustryByName(excelInfo.IndustryName);
                //c.IndustryID = ci.HasValue ? ((int)ci.Value).ToString() : "";

                c.ProvinceID = BLL.AreaHelper.Instance.GetAreaIDByName(excelInfo.ProvinceName);
                c.CityID = BLL.AreaHelper.Instance.GetAreaIDByName(excelInfo.CityName);
                c.CountyID = BLL.AreaHelper.Instance.GetAreaIDByName(excelInfo.CountyName);
                c.BrandIDs = BLL.CarBrandHelper.Instance.GetCarBrandIDsByNames(excelInfo.BrandName);
                c.Address = excelInfo.Address;
                c.OfficeTel = excelInfo.OfficeTel;
                c.Fax = excelInfo.Fax;
                c.Zipcode = excelInfo.Zipcode;

                c.CreateTime = c.LastUpdateTime = DateTime.Now;
                c.CreateUserID = c.LastUpdateUserID = BLL.Util.GetLoginUserID();
                c.CarType = excelInfo.CarType;
                c.ContactName = excelInfo.ContactName;
                if (!string.IsNullOrEmpty(excelInfo.ContactName))
                {
                    c.ContactName = excelInfo.ContactName;
                }
                if (excelInfo.TradeMarketID > 0)
                {
                    c.TradeMarketID = excelInfo.TradeMarketID.ToString();
                }
                if (excelInfo.CarType > 0)
                {
                    c.CarType = excelInfo.CarType;
                }
                if (!string.IsNullOrEmpty(excelInfo.TypeName))
                {
                    foreach (BitAuto.YanFa.Crm2009.Entities.EnumCustomType type in (BitAuto.YanFa.Crm2009.Entities.EnumCustomType[])Enum.GetValues(typeof(BitAuto.YanFa.Crm2009.Entities.EnumCustomType)))
                    {
                        if (Utils.EnumHelper.GetEnumTextValue(type) == excelInfo.TypeName)
                        {
                            c.TypeID = ((int)type).ToString();
                            break;
                        }
                    }
                }
                if (excelInfo.MonthSales > 0 || excelInfo.MonthStock > 0 || excelInfo.MonthTrade > 0)
                {
                    Entities.ProjectTask_BusinessScale scaleInfo = new Entities.ProjectTask_BusinessScale();
                    scaleInfo.CreateTime = DateTime.Now;
                    scaleInfo.CreateUserID = Util.GetLoginUserID();
                    scaleInfo.MonthSales = excelInfo.MonthSales;
                    scaleInfo.MonthStock = excelInfo.MonthStock;
                    scaleInfo.MonthTrade = excelInfo.MonthTrade;
                    scaleInfo.Status = 0;
                    scaleInfo.PTID = task.PTID;
                    BLL.ProjectTask_BusinessScale.Instance.Insert(scaleInfo);
                }
                BLL.ProjectTask_Cust.Instance.Insert(c);
            }
        }

        /// <summary>
        /// 将CRM中信息同步到ProjectTask_Cust
        /// </summary>
        public void SyncInfoFromCrm(Entities.ProjectTaskInfo task)
        {
            if (task.Source == 2 && BLL.ProjectTask_Cust.Instance.Exists(task.PTID) == false)
            {
                Dal.ProjectTask_Cust.Instance.SyncInfoFromCrm(task, Util.GetLoginUserID(), DateTime.Now);
            }
        }

        /// <summary>
        /// 更新客户核实信息
        /// </summary>
        protected void Update(Entities.ProjectTask_Cust cust)
        {
            cust.LastUpdateTime = DateTime.Now;
            cust.LastUpdateUserID = BLL.Util.GetLoginUserID();

            Dal.ProjectTask_Cust.Instance.Update(cust);
            //主营品牌
            Dal.ProjectTask_Cust_Brand.Instance.UpdataBrandInfoByTID(cust.PTID, cust.BrandIDs);
        }

        /// <summary>
        /// 保存核实的信息(客户与会员)
        /// </summary>
        public void SaveCheckedInfo(Entities.ProjectTask_Cust cust, List<Entities.ProjectTask_DMSMember> members, List<Entities.ProjectTask_CSTMember> cstMembers, List<Entities.ProjectTask_CSTLinkMan> linkMans, List<string> cstMemberBrands, DateTime dtime)
        {
            Entities.ProjectTask_Cust oldCustInfo = BLL.ProjectTask_Cust.Instance.GetProjectTask_Cust(cust.PTID);

            //如果更改经营范围
            if (oldCustInfo.CarType != 1 && cust.CarType == 1)
            {
                //删除客户二手车相关信息
                cust.CstMemberID = string.Empty;
                cust.TradeMarketID = string.Empty;
                cust.UsedCarBusinessType = string.Empty;

                //删除二手车相关
                IList<Entities.ProjectTask_BusinessScale> businessScaleList = BLL.ProjectTask_BusinessScale.Instance.GetAllProjectTask_BusinessScaleByTID(cust.PTID);
                foreach (Entities.ProjectTask_BusinessScale busInfo in businessScaleList)
                {
                    BLL.ProjectTask_BusinessScale.Instance.Delete(busInfo.RecID);
                }

                //删除车商通会员
                IList<Entities.ProjectTask_CSTMember> memberList = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(cust.PTID.ToString());
                foreach (Entities.ProjectTask_CSTMember cstMemberInfo in memberList)
                {
                    BLL.ProjectTask_CSTMember.Instance.Delete(cstMemberInfo.ID);
                }
            }

            this.Update(cust);
            int i = 0;
            #region 插入会员
            foreach (Entities.ProjectTask_DMSMember member in members)
            {
                member.PTID = cust.PTID;
                BLL.ProjectTask_DMSMember.Instance.InsertOrUpdate(member);
                i++;
            }
            #endregion



            #region 插入车商通会员信息
            if (cstMembers != null)
            {
                int count = 0;
                #region 会员基本信息
                foreach (Entities.ProjectTask_CSTMember cstMemberInfo in cstMembers)
                {
                    int recId = -1;
                    if (cstMemberInfo.ID > 0)
                    {
                        recId = cstMemberInfo.ID;
                        linkMans[count].CSTMemberID = recId;
                        linkMans[count].PTID = cust.PTID;
                        cstMemberInfo.PTID = cust.PTID;
                        BLL.ProjectTask_CSTMember.Instance.Update(cstMemberInfo);
                        //插入车商通会员品牌
                        InsertCstMemberBrand(cstMemberBrands, count, cstMemberInfo.ID);
                    }
                    else
                    {
                        cstMemberInfo.PTID = cust.PTID;
                        //cstMemberInfo
                        recId = BLL.ProjectTask_CSTMember.Instance.Insert(cstMemberInfo);
                        //插入车商通会员品牌
                        InsertCstMemberBrand(cstMemberBrands, count, recId);

                        linkMans[count].CSTMemberID = recId;
                        linkMans[count].PTID = cust.PTID;
                    }
                    count++;
                }
                #endregion

                #region 会员联系人信息
                foreach (Entities.ProjectTask_CSTLinkMan linkMan in linkMans)
                {
                    //如果是开通的会员，不进行任何修改
                    if (!BLL.ProjectTask_CSTMember.Instance.IsOpenSuccessMember((int)linkMan.CSTMemberID))
                    {
                        Entities.QueryProjectTask_CSTLinkMan linkManQuery = new QueryProjectTask_CSTLinkMan();
                        linkManQuery.CSTMemberID = linkMan.CSTMemberID;
                        int total = 0;
                        DataTable dt = BLL.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkMan(linkManQuery, "", 1, 1, out total);
                        if (total > 0)
                        {
                            Entities.ProjectTask_CSTLinkMan linkManInfo = BLL.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkMan(int.Parse(dt.Rows[0]["RecID"].ToString()));
                            linkManInfo.Department = linkMan.Department;
                            linkManInfo.Email = linkMan.Email;
                            linkManInfo.Mobile = linkMan.Mobile;
                            linkManInfo.Name = linkMan.Name;
                            linkManInfo.Position = linkMan.Position;
                            BLL.ProjectTask_CSTLinkMan.Instance.Update(linkManInfo);
                        }
                        else
                        {
                            BLL.ProjectTask_CSTLinkMan.Instance.Insert(linkMan);
                        }
                    }
                }
                #endregion
            }
            #endregion

            //任务状态
            BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(cust.PTID, Entities.EnumProjectTaskStatus.Assigning, Entities.EnumProjectTaskOperationStatus.TaskSave, "", dtime);
        }

        /// <summary>
        /// 插入车商通主营品牌
        /// </summary>
        /// <param name="cstMemberBrands"></param>
        /// <param name="count"></param>
        /// <param name="memberId"></param>
        private void InsertCstMemberBrand(List<string> cstMemberBrands, int count, int memberId)
        {
            BLL.ProjectTask_CSTMember_Brand.Instance.Delete(memberId);
            string brands = cstMemberBrands[count];
            if (brands != string.Empty)
            {
                string[] brandArry = brands.Split(',');
                foreach (string brandId in brandArry)
                {
                    int id = -1;
                    if (int.TryParse(brandId, out id))
                    {
                        Entities.ProjectTask_CSTMember_Brand brandInfo = new Entities.ProjectTask_CSTMember_Brand();
                        brandInfo.BrandID = id;
                        brandInfo.CreateUserID = BLL.Util.GetLoginUserID();
                        brandInfo.CSTMemberID = memberId;
                        BLL.ProjectTask_CSTMember_Brand.Instance.Insert(brandInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 提交核实的信息(客户与会员)
        /// </summary>
        public void SubmitCheckedInfo(Entities.ProjectTask_Cust cust, List<Entities.ProjectTask_DMSMember> members, List<Entities.ProjectTask_CSTMember> cstMembers, List<Entities.ProjectTask_CSTLinkMan> linkMans, List<string> cstMemberBrands, DateTime dtime)
        {
            Entities.ProjectTask_Cust oldCustInfo = BLL.ProjectTask_Cust.Instance.GetProjectTask_Cust(cust.PTID);
            //如果更改经营范围
            if (oldCustInfo.CarType != 1 && cust.CarType == 1)
            {
                //删除客户二手车相关信息
                cust.CstMemberID = string.Empty;
                cust.TradeMarketID = string.Empty;
                cust.UsedCarBusinessType = string.Empty;

                //删除二手车相关
                IList<Entities.ProjectTask_BusinessScale> businessScaleList = BLL.ProjectTask_BusinessScale.Instance.GetAllProjectTask_BusinessScaleByTID(cust.PTID);
                foreach (Entities.ProjectTask_BusinessScale busInfo in businessScaleList)
                {
                    BLL.ProjectTask_BusinessScale.Instance.Delete(busInfo.RecID);
                }

                //删除车商通会员
                IList<Entities.ProjectTask_CSTMember> memberList = BLL.ProjectTask_CSTMember.Instance.GetProjectTask_CSTMemberByTID(cust.PTID.ToString());
                foreach (Entities.ProjectTask_CSTMember cstMemberInfo in memberList)
                {
                    BLL.ProjectTask_CSTMember.Instance.Delete(cstMemberInfo.ID);
                }
            }
            this.Update(cust);
            int i = 0;
            foreach (Entities.ProjectTask_DMSMember member in members)
            {
                member.PTID = cust.PTID;
                BLL.ProjectTask_DMSMember.Instance.InsertOrUpdate(member);
                //删除会员保存数据

                i++;
            }
            #region 车商通信息插入
            if (cust.TypeID != "1")
            {
                if (cstMembers != null)
                {
                    int count = 0;
                    foreach (Entities.ProjectTask_CSTMember cstMemberInfo in cstMembers)
                    {
                        int recId = -1;
                        if (cstMemberInfo.ID > 0)
                        {
                            recId = cstMemberInfo.ID;
                            linkMans[count].CSTMemberID = recId;
                            linkMans[count].PTID = cust.PTID;
                            cstMemberInfo.PTID = cust.PTID;
                            BLL.ProjectTask_CSTMember.Instance.Update(cstMemberInfo);
                            //插入车商通会员品牌
                            InsertCstMemberBrand(cstMemberBrands, count, cstMemberInfo.ID);
                        }
                        else
                        {
                            cstMemberInfo.PTID = cust.PTID;
                            recId = BLL.ProjectTask_CSTMember.Instance.Insert(cstMemberInfo);
                            //插入车商通会员品牌
                            InsertCstMemberBrand(cstMemberBrands, count, recId);

                            linkMans[count].CSTMemberID = recId;
                            linkMans[count].PTID = cust.PTID;
                        }
                        count++;
                    }
                    foreach (Entities.ProjectTask_CSTLinkMan linkMan in linkMans)
                    {
                        //如果是开通的会员，不进行任何修改
                        if (!BLL.ProjectTask_CSTMember.Instance.IsOpenSuccessMember((int)linkMan.CSTMemberID))
                        {
                            Entities.QueryProjectTask_CSTLinkMan linkManQuery = new QueryProjectTask_CSTLinkMan();
                            linkManQuery.CSTMemberID = linkMan.CSTMemberID;
                            int total = 0;
                            DataTable dt = BLL.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkMan(linkManQuery, "", 1, 1, out total);
                            if (total > 0)
                            {
                                Entities.ProjectTask_CSTLinkMan linkManInfo = BLL.ProjectTask_CSTLinkMan.Instance.GetProjectTask_CSTLinkMan(int.Parse(dt.Rows[0]["RecID"].ToString()));
                                linkManInfo.Department = linkMan.Department;
                                linkManInfo.Email = linkMan.Email;
                                linkManInfo.Mobile = linkMan.Mobile;
                                linkManInfo.Name = linkMan.Name;
                                linkManInfo.Position = linkMan.Position;
                                BLL.ProjectTask_CSTLinkMan.Instance.Update(linkManInfo);
                            }
                            else
                            {
                                BLL.ProjectTask_CSTLinkMan.Instance.Insert(linkMan);
                            }
                        }
                    }
                }
            }
            #endregion

            //任务状态,处理完成后改为待审核
            BLL.ProjectTaskInfo.Instance.UpdateTaskStatus(cust.PTID, Entities.EnumProjectTaskStatus.SubmitFinsih, EnumProjectTaskOperationStatus.TaskSubmit, "", dtime);

        }

        /// <summary>
        /// 根据任务ID，删除客户信息
        /// </summary>
        /// <param name="tid">任务ID</param>
        public void DeleteByTID(string tid)
        {
            Dal.ProjectTask_Cust.Instance.DeleteByTID(tid);
        }

        /// <summary>
        /// 根据CRM客户名称，查询数据来源为CRM库的，客户核实信息
        /// </summary>
        /// <param name="custName">CRM客户名称</param>
        /// <returns></returns>
        public DataTable GetProjectTask_CustByExcelCustName(string custName)
        {
            return Dal.ProjectTask_Cust.Instance.GetProjectTask_CustByCRMCustName(custName);
        }

        /// <summary>
        /// 根据任务ID得到客户信息实体 刘学文
        /// </summary>
        /// <param name="ptid">任务ID</param>
        /// <returns></returns>
        public Entities.ProjectTask_Cust GetCustInfoModelByPTID(string ptid)
        {
            return Dal.ProjectTask_Cust.Instance.GetCustInfoModelByPTID(ptid);
        }
        /// <summary>
        /// 通过任务ID得到客户信息和会员信息 刘学文 13.2.25
        /// </summary>
        /// <param name="ptid">任务ID</param>
        /// <returns>Table</returns>
        public DataTable GetCustInfoByPTID(string ptid)
        {
            return Dal.ProjectTask_Cust.Instance.GetCustInfoByPTID(ptid);
        }

        /// <summary>
        /// 通过项目ID得到客户信息和会员信息串 刘学文 13.3.14
        /// </summary>
        /// <param name="ProjectID">ProjectID</param>
        /// <returns>Table</returns>
        public DataTable GetCustInfoByReturnProjectID(string ProjectID)
        {
            return Dal.ProjectTask_Cust.Instance.GetCustInfoByReturnProjectID(ProjectID);
        }
        /// <summary>
        /// 通过项目ID得到客户信息和会员信息串 刘学文 13.4.7
        /// </summary>
        /// <param name="ProjectID">项目ID</param>
        /// <param name="siid">问卷ID</param>
        /// <returns>Table</returns>
        public DataTable GetCustInfoByProjectID(int ProjectID, int siid)
        {
            return Dal.ProjectTask_Cust.Instance.GetCustInfoByProjectID(ProjectID, siid);
        }
    }
}

