using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.YanFa.Crm2009;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class ProjectTask_AuditContrastInfo
    {
           #region Instance
        public static readonly ProjectTask_AuditContrastInfo Instance = new ProjectTask_AuditContrastInfo();
        #endregion

        #region Contructor
        protected ProjectTask_AuditContrastInfo()
        { }
        #endregion

        #region Select
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetProjectTask_AuditContrastInfo(QueryProjectTask_AuditContrastInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfo(query, order, currentPage, pageSize, out totalCount);
        }
        
        //add by qizhiqiang 2012-4-27
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetProjectTask_AuditContrastInfoForChange(QueryProjectTask_AuditContrastInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfoForChange(query, order, currentPage, pageSize, out totalCount);
        }


        public DataTable GetCC_AuditContrastCSTInfo(QueryProjectTask_AuditContrastInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ProjectTask_AuditContrastInfo.Instance.GetCC_AuditContrastCSTInfo(query, order, currentPage, pageSize, out totalCount);
        }
         /// <summary>
        /// 根据会员名称和变更类型获取变更信息
        /// </summary>
        /// <param name="dmsMemberId"></param>
        /// <param name="contrastType"></param>
        /// <returns></returns>
        public DataTable GetProjectTask_AuditContrastInfo(string dmsMemberId, int contrastType,string contrastFiled)
        {
            return Dal.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfo(dmsMemberId, contrastType, contrastFiled);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfo(new QueryProjectTask_AuditContrastInfo(), string.Empty, 1, 2147483647, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ProjectTask_AuditContrastInfo GetProjectTask_AuditContrastInfo(int RecID)
        {
            return Dal.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfo(RecID);
        }


        /// <summary>
        /// 根据RecID，初始化核对的会员信息（仅6个字段和4个关键字）
        /// </summary>
        /// <param name="recID">RecID</param>
        /// <returns></returns>
        public BitAuto.YanFa.Crm2009.Entities.DMSMember GetCC_DMSMemberByRecID(int recID)
        {
            BitAuto.YanFa.Crm2009.Entities.DMSMember DMSMember = new BitAuto.YanFa.Crm2009.Entities.DMSMember();
            Entities.ProjectTask_AuditContrastInfo model = GetProjectTask_AuditContrastInfo(recID);
            if (model != null && model.ContrastType != null &&
                (model.ContrastType.Value == 2 || model.ContrastType.Value == 3) &&
                !string.IsNullOrEmpty(model.ContrastInfoInside))
            {
                return BitAuto.YanFa.Crm2009.BLL.DMSMember.Instance.GetDMSMember(model.DMSMemberID, model.ContrastInfoInside);
            }
            return null;
        }


        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="tid">任务ID</param>
        /// <param name="dd">对照类型ID</param>
        /// <returns></returns>
        public bool IsExistsByTIDAndContrastType(int tid, int contrastTypeID)
        {
            QueryProjectTask_AuditContrastInfo query = new QueryProjectTask_AuditContrastInfo();
            query.PTID = tid;
            query.ContrastType = contrastTypeID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetProjectTask_AuditContrastInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.ProjectTask_AuditContrastInfo model)
        {
            return Dal.ProjectTask_AuditContrastInfo.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.ProjectTask_AuditContrastInfo model)
        {
            return Dal.ProjectTask_AuditContrastInfo.Instance.Update(model);
        }

        /// <summary>
        /// 根据查询条件，批量更新导出状态
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="exportStatus">导出状态（0-未导出，1-已导出）</param>
        public int BatchUpdateExportStatusByWhere(QueryProjectTask_AuditContrastInfo query, int exportStatus)
        {
            return Dal.ProjectTask_AuditContrastInfo.Instance.BatchUpdateExportStatusByWhere(query, exportStatus);
        }
        #endregion

        /// <summary>
        /// 插入到审核对照表，类型为：客户名称变化
        /// </summary>
        /// <param name="cc_Custs">呼叫中心客户实体</param>
        /// <param name="custInfo">CRM系统客户实体</param>
        public int InsertByCustNameChange(Entities.ProjectTask_Cust cc_Custs, BitAuto.YanFa.Crm2009.Entities.CustInfo custInfo)
        {
            Entities.ProjectTask_AuditContrastInfo model = new Entities.ProjectTask_AuditContrastInfo();
            model.PTID = cc_Custs.PTID;
            model.CustID = custInfo.CustID;
            model.ContrastInfoInside = "CustName:('" + BLL.Util.EscapeString(custInfo.CustName) + "','" + BLL.Util.EscapeString(cc_Custs.CustName) + "')";
            model.ContrastInfo = "客户名称由：" + custInfo.CustName + "，改为：" + cc_Custs.CustName;
            model.ExportStatus = 0;//0-未导出，1-已导出
            model.ContrastType = 1;
            model.CreateTime = DateTime.Now;
            model.CreateUserID = BLL.Util.GetLoginUserID();
            model.DisposeStatus = 0;//未处理
            return Insert(model);
        }

        /// <summary>
        /// 插入删除客户申请记录
        /// </summary>
        /// <param name="tid">任务ID</param>
        /// <returns></returns>
        public int InsertByDelCustRelation(string tid, string cc_CustOriginalName, string cc_CustChangeName)
        {
            Entities.ProjectTask_DelCustRelation delModel = BLL.ProjectTask_DelCustRelation.Instance.GetProjectTask_DelCustRelationByTID(tid);
            if (delModel != null)
            {
                Entities.ProjectTask_AuditContrastInfo model = new Entities.ProjectTask_AuditContrastInfo();
                model.PTID = tid;
                model.CustID = delModel.CustID;
                model.ContrastInfoInside = delModel.DelCustIDs;
                string contrastInfo = "客户名称重复，关联客户ID：" + delModel.DelCustIDs;
                if (cc_CustOriginalName != cc_CustChangeName)
                {
                    contrastInfo = "客户名称由：" + cc_CustOriginalName + "，改为：" + cc_CustChangeName + "," + contrastInfo;
                }
                model.ContrastInfo = contrastInfo;//"客户名称由：" + cc_CustOriginalName + "，改为：" + cc_CustChangeName + "，客户名称重复，关联客户ID：" + delModel.DelCustIDs;
                model.ExportStatus = 0;//0-未导出，1-已导出
                model.ContrastType = 4;
                model.CreateTime = DateTime.Now;
                model.CreateUserID = BLL.Util.GetLoginUserID();
                model.DisposeStatus = 0;//未处理
                return Insert(model);
            }
            return -1;
        }

        /// <summary>
        /// 插入到审核对照表，类型为：会员主营品牌变化,则生成记录
        /// </summary>
        /// <param name="cc_DMSMember">呼叫中心会员实体</param>
        /// <param name="DMSMember">CRM系统会员实体</param>
        /// <returns>返回插入记录主键ID</returns>
        public int InsertByDMSMemberBrandChange(Entities.ProjectTask_DMSMember cc_DMSMember, BitAuto.YanFa.Crm2009.Entities.DMSMember DMSMember)
        {
            int recid = -1;
            if (cc_DMSMember != null && DMSMember != null)
            {
                Entities.ProjectTask_AuditContrastInfo model = new Entities.ProjectTask_AuditContrastInfo();
                model.PTID = cc_DMSMember.PTID;
                model.DMSMemberID = DMSMember.ID.ToString();
                model.CustID = DMSMember.CustID;
                model.CreateTime = DateTime.Now;
                model.CreateUserID = BLL.Util.GetLoginUserID();
                model.ExportStatus = 0;//0-未导出，1-已导出
                model.DisposeStatus = 0;//未处理

                string contrastInfoInside = string.Empty;
                string contrastInfo = string.Empty;

                //判断会员类型字段是否有过更改
                if (!BLL.Util.IsEqualsByStringArray(cc_DMSMember.BrandIDs, DMSMember.BrandIds))
                {
                    model.ContrastType = 6;
                    contrastInfoInside += "BrandIDs:('" + Util.EscapeString(DMSMember.BrandIds) + "','" + Util.EscapeString(cc_DMSMember.BrandIDs) + "'),";
                    contrastInfo += "会员主营品牌由：" + DMSMember.BrandNames +
                                    "，改为：" + cc_DMSMember.BrandNames + ",";
                    contrastInfoInside = contrastInfoInside.TrimEnd(',');
                    contrastInfo = contrastInfo.TrimEnd(',');

                    if (contrastInfoInside != string.Empty &&
                        contrastInfo != string.Empty)
                    {
                        model.ContrastInfoInside = contrastInfoInside;
                        model.ContrastInfo = contrastInfo;
                        if (string.IsNullOrEmpty(DMSMember.MemberCode))//是否会员开通成功
                        {
                            model.DisposeStatus = 1;//已处理
                            model.DisposeTime = DateTime.Now;
                        }
                        Insert(model);
                    }
                }
                contrastInfoInside = string.Empty;
                contrastInfo = string.Empty;
            }
            return recid;
        }

        //add by qizhiqiang 2012-4-25
        /// <summary>
        /// 插入到审核对照表，类型为：已开通车易通信息变更,则生成记录
        /// </summary>
        /// <param name="cc_DMSMember">呼叫中心会员实体</param>
        /// <param name="DMSMember">CRM系统会员实体</param>
        /// <returns>返回插入记录主键ID</returns>
        public int InsertByOpenedDMSMemberChange(Entities.ProjectTask_DMSMember cc_DMSMember, BitAuto.YanFa.Crm2009.Entities.DMSMember DMSMember)
        {
            int recid = -1;
            if (cc_DMSMember != null && DMSMember != null)
            {
                Entities.ProjectTask_AuditContrastInfo model = new Entities.ProjectTask_AuditContrastInfo();
                model.PTID = cc_DMSMember.PTID;
                model.DMSMemberID = DMSMember.ID.ToString();
                model.CustID = DMSMember.CustID;
                model.CreateTime = null;
                model.CreateUserID = BLL.Util.GetLoginUserID();
                model.ExportStatus = 0;//0-未导出，1-已导出
                model.DisposeStatus = 0;//未处理

                string contrastInfoInside = string.Empty;
                string contrastInfo = string.Empty;

                //已开通有排期车易通信息变更
                model.ContrastType = 9;

                //判断是否修改地图
                IsEqualsByMapList(cc_DMSMember.Lantitude, cc_DMSMember.Longitude, DMSMember.MapCoordinateList, ref contrastInfoInside, ref contrastInfo);


                if (!BLL.Util.IsEqualsByStringArray(cc_DMSMember.BrandIDs, DMSMember.BrandIds) ||
                    cc_DMSMember.Name != DMSMember.Name ||
                    cc_DMSMember.Abbr != DMSMember.Abbr ||
                    cc_DMSMember.ProvinceID != DMSMember.ProvinceID ||
                    cc_DMSMember.CityID != DMSMember.CityID ||
                    cc_DMSMember.CountyID != DMSMember.CountyID ||
                    cc_DMSMember.MemberType != DMSMember.MemberType ||
                    cc_DMSMember.Phone != DMSMember.Phone ||
                    cc_DMSMember.Fax != DMSMember.Fax ||
                    cc_DMSMember.CompanyWebSite != DMSMember.CompanyWebSite ||
                    cc_DMSMember.Email != DMSMember.Email ||
                    cc_DMSMember.Postcode != DMSMember.Postcode ||
                    cc_DMSMember.ContactAddress != DMSMember.ContactAddress ||
                    cc_DMSMember.TrafficInfo != DMSMember.TrafficInfo ||
                    cc_DMSMember.EnterpriseBrief != DMSMember.EnterpriseBrief ||
                    cc_DMSMember.Remarks != DMSMember.Remarks ||
                    !BLL.Util.IsEqualsByStringArray(cc_DMSMember.SerialIds, DMSMember.SerialIds)
                    )
                {
                    GetDMSMemberContrastByOpenedDMSMember(cc_DMSMember, DMSMember,
                                         ref contrastInfoInside, ref contrastInfo);
                }
                if (contrastInfoInside != string.Empty &&
                        contrastInfo != string.Empty)
                {
                    contrastInfoInside = contrastInfoInside.TrimEnd(',');
                    contrastInfo = contrastInfo.TrimEnd(',');

                    model.ContrastInfoInside = contrastInfoInside;
                    model.ContrastInfo = contrastInfo;
                    //if (string.IsNullOrEmpty(DMSMember.MemberCode))//是否会员开通成功
                    //{
                    //    model.DisposeStatus = 1;//已处理
                    //    model.DisposeTime = DateTime.Now;
                    //}
                    Insert(model);
                }
                
            }
            return recid;
        }
        //add by qizhiqiang 2012-4-25根据已开通易湃会员与cc 中的不用，生成变更信息
        /// <summary>
        /// 根据已开通易湃会员字段信息与CC中的不同，生成变更信息
        /// </summary>
        /// <param name="cc_DMSMember"></param>
        /// <param name="DMSMember"></param>
        /// <param name="contrastInfoInside"></param>
        /// <param name="contrastInfo"></param>
        private void GetDMSMemberContrastByOpenedDMSMember(Entities.ProjectTask_DMSMember cc_DMSMember, BitAuto.YanFa.Crm2009.Entities.DMSMember DMSMember, ref string contrastInfoInside, ref string contrastInfo)
        {
            if (!BLL.Util.IsEqualsByStringArray(cc_DMSMember.BrandIDs, DMSMember.BrandIds))
            {

                contrastInfoInside += "BrandIDs:('" + Util.EscapeString(DMSMember.BrandIds) + "','" + Util.EscapeString(cc_DMSMember.BrandIDs) + "'),";
                contrastInfo += "易湃会员主营品牌由：" + DMSMember.BrandNames +
                                "，改为：" + cc_DMSMember.BrandNames + ",";

            }
            if (cc_DMSMember.Name != DMSMember.Name)
            {
                contrastInfoInside += "Name:('" + Util.EscapeString(DMSMember.Name) + "','" + Util.EscapeString(cc_DMSMember.Name) + "'),";
                contrastInfo += "易湃会员名称由：" + DMSMember.Name + "，改为：" + cc_DMSMember.Name + ",";
            }
            if (cc_DMSMember.Abbr != DMSMember.Abbr)
            {
                contrastInfoInside += "Abbr:('" + Util.EscapeString(DMSMember.Abbr) + "','" + Util.EscapeString(cc_DMSMember.Abbr) + "'),";
                contrastInfo += "易湃会员简称由：" + DMSMember.Abbr + "，改为：" + cc_DMSMember.Abbr + ",";
            }
            if (cc_DMSMember.ProvinceID != DMSMember.ProvinceID)
            {
                contrastInfoInside += "ProvinceID:('" + Util.EscapeString(DMSMember.ProvinceID) + "','" + Util.EscapeString(cc_DMSMember.ProvinceID) + "'),";
                contrastInfo += "易湃会员所属省份由：" + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(DMSMember.ProvinceID) +
                                "，改为：" + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(cc_DMSMember.ProvinceID) + ",";
            }
            if (cc_DMSMember.CityID != DMSMember.CityID)
            {
                contrastInfoInside += "CityID:('" + Util.EscapeString(DMSMember.CityID) + "','" + Util.EscapeString(cc_DMSMember.CityID) + "'),";
                contrastInfo += "易湃会员所属城市由：" + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(DMSMember.CityID) +
                                "，改为：" + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(cc_DMSMember.CityID) + ",";
            }
            if (cc_DMSMember.CountyID != DMSMember.CountyID)
            {
                contrastInfoInside += "CountyID:('" + Util.EscapeString(DMSMember.CountyID) + "','" + Util.EscapeString(cc_DMSMember.CountyID) + "'),";
                contrastInfo += "易湃会员所属区县由：" + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(DMSMember.CountyID) +
                                "，改为：" + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(cc_DMSMember.CountyID) + ",";
            }
            if (cc_DMSMember.MemberType != DMSMember.MemberType)
            {
                contrastInfoInside += "MemberType:('" + Util.EscapeString(DMSMember.MemberType) + "','" + Util.EscapeString(cc_DMSMember.MemberType) + "'),";
                contrastInfo += "易湃会员类型由：" + GetMemberType(DMSMember.MemberType) +
                                "，改为：" + GetMemberType(cc_DMSMember.MemberType) + ",";
            }

            if (cc_DMSMember.Phone != DMSMember.Phone)
            {
                contrastInfoInside += "Phone:('" + Util.EscapeString(DMSMember.Phone) + "','" + Util.EscapeString(cc_DMSMember.Phone) + "'),";
                contrastInfo += "易湃会员电话由：" + DMSMember.Phone + "，改为：" + cc_DMSMember.Phone + ",";
            }
            if (cc_DMSMember.Fax != DMSMember.Fax)
            {
                contrastInfoInside += "Fax:('" + Util.EscapeString(DMSMember.Fax) + "','" + Util.EscapeString(cc_DMSMember.Fax) + "'),";
                contrastInfo += "易湃会员传真由：" + DMSMember.Fax + "，改为：" + cc_DMSMember.Fax + ",";
            }
            if (cc_DMSMember.CompanyWebSite != DMSMember.CompanyWebSite)
            {
                contrastInfoInside += "CompanyWebSite:('" + Util.EscapeString(DMSMember.CompanyWebSite) + "','" + Util.EscapeString(cc_DMSMember.CompanyWebSite) + "'),";
                contrastInfo += "易湃会员公司网址由：" + DMSMember.CompanyWebSite + "，改为：" + cc_DMSMember.CompanyWebSite + ",";
            }
            if (cc_DMSMember.Email != DMSMember.Email)
            {
                contrastInfoInside += "Email:('" + Util.EscapeString(DMSMember.Email) + "','" + Util.EscapeString(cc_DMSMember.Email) + "'),";
                contrastInfo += "易湃会员Email由：" + DMSMember.Email + "，改为：" + cc_DMSMember.Email + ",";
            }
            if (cc_DMSMember.Postcode != DMSMember.Postcode)
            {
                contrastInfoInside += "Postcode:('" + Util.EscapeString(DMSMember.Postcode) + "','" + Util.EscapeString(cc_DMSMember.Postcode) + "'),";
                contrastInfo += "易湃会员邮编由：" + DMSMember.Postcode + "，改为：" + cc_DMSMember.Postcode + ",";
            }
            if (cc_DMSMember.ContactAddress != DMSMember.ContactAddress)
            {
                contrastInfoInside += "ContactAddress:('" + Util.EscapeString(DMSMember.ContactAddress) + "','" + Util.EscapeString(cc_DMSMember.ContactAddress) + "'),";
                contrastInfo += "易湃会员销售地址由：" + DMSMember.ContactAddress + "，改为：" + cc_DMSMember.ContactAddress + ",";
            }
            if (cc_DMSMember.TrafficInfo != DMSMember.TrafficInfo)
            {
                contrastInfoInside += "TrafficInfo:('" + Util.EscapeString(DMSMember.TrafficInfo) + "','" + Util.EscapeString(cc_DMSMember.TrafficInfo) + "'),";
                contrastInfo += "易湃会员交通信息由：" + DMSMember.TrafficInfo + "，改为：" + cc_DMSMember.TrafficInfo + ",";
            }
            if (cc_DMSMember.EnterpriseBrief != DMSMember.EnterpriseBrief)
            {
                contrastInfoInside += "EnterpriseBrief:('" + Util.EscapeString(DMSMember.EnterpriseBrief) + "','" + Util.EscapeString(cc_DMSMember.EnterpriseBrief) + "'),";
                contrastInfo += "易湃会员企业简介由：" + DMSMember.EnterpriseBrief + "，改为：" + cc_DMSMember.EnterpriseBrief + ",";
            }
            if (cc_DMSMember.Remarks != DMSMember.Remarks)
            {
                contrastInfoInside += "Remarks:('" + Util.EscapeString(DMSMember.Remarks) + "','" + Util.EscapeString(cc_DMSMember.Remarks) + "'),";
                contrastInfo += "易湃会员备注由：" + DMSMember.Remarks + "，改为：" + cc_DMSMember.Remarks + ",";
            }

            if (!BLL.Util.IsEqualsByStringArray(cc_DMSMember.SerialIds, DMSMember.SerialIds))
            {

                contrastInfoInside += "SerialIds:('" + Util.EscapeString(DMSMember.SerialIds) + "','" + Util.EscapeString(cc_DMSMember.SerialIds) + "'),";
                contrastInfo += "易湃会员附属子品牌由：" + DMSMember.SerialNames +
                                "，改为：" + cc_DMSMember.SerialNames + ",";

            }

            
        }

        //add by qizhiqiang 2012-4-25判断地图是否修改
        /// <summary>
        /// 判断会员地图是否一样
        /// </summary>
        /// <param name="ccmaplist"></param>
        /// <param name="dmsmaplist"></param>
        /// <returns></returns>
        private void IsEqualsByMapList(string ccmapLatitude,string ccmapLongitude, List<BitAuto.YanFa.Crm2009.Entities.DMSMapCoordinate> dmsmaplist, ref string contrastInfoInside, ref string contrastInfo)
        {


            if (dmsmaplist.Count > 0)
            {
                BitAuto.YanFa.Crm2009.Entities.DMSMapCoordinate map = dmsmaplist[0];

                    if (map.MapProviderName.ToLower() == BitAuto.YanFa.Crm2009.Entities.Constants.Constant.MapProviderName.ToLower())
                    {
                        if (map.Latitude != ccmapLatitude || map.Longitude != ccmapLongitude)
                        {
                            contrastInfoInside += "Latitude:('" + Util.EscapeString(map.Latitude) + "','" + Util.EscapeString(ccmapLatitude) + "'),Longitude:('" + Util.EscapeString(map.Longitude) + "','" + Util.EscapeString(ccmapLongitude) + "'),";
                            contrastInfo += "易湃会员地图经度由：" + map.Latitude + "，改为：" + ccmapLatitude + ",易湃会员地图纬度由：" + map.Longitude + "，改为：" + ccmapLongitude + ",";
                        }
                    }
            }
            else
            {
                contrastInfoInside += "Latitude:('" + Util.EscapeString("") + "','" + Util.EscapeString(ccmapLatitude) + "'),Longitude:('" + Util.EscapeString("") + "','" + Util.EscapeString(ccmapLongitude) + "'),";
                contrastInfo += "易湃会员地图经度由：，改为：" + ccmapLatitude + ",易湃会员地图纬度由：，改为：" + ccmapLongitude + ",";
            }
        }



        /// <summary>
        /// 插入到审核对照表，类型为：会员信息变化,若会员有变化，则生成记录
        /// </summary>
        /// <param name="cC_DMSMember">呼叫中心会员实体</param>
        /// <param name="DMSMember">CRM系统会员实体</param>
        /// <returns>返回插入记录主键ID</returns>
        public int InsertByDMSMemberChange(Entities.ProjectTask_DMSMember cc_DMSMember, BitAuto.YanFa.Crm2009.Entities.DMSMember DMSMember,out bool isChange)
        {
            int recid = -1;
            isChange = false;
            if (cc_DMSMember != null && DMSMember != null)
            {
                Entities.ProjectTask_AuditContrastInfo model = new Entities.ProjectTask_AuditContrastInfo();
                model.PTID = cc_DMSMember.PTID;
                model.DMSMemberID = DMSMember.ID.ToString();
                model.CustID = DMSMember.CustID;
                model.CreateTime = DateTime.Now;
                model.CreateUserID = BLL.Util.GetLoginUserID();
                model.ExportStatus = 0;//0-未导出，1-已导出

                string contrastInfoInside = string.Empty;
                string contrastInfo = string.Empty;

                //判断会员4个字段是否有过更改
                if (cc_DMSMember.Name != DMSMember.Name ||
                    cc_DMSMember.Abbr != DMSMember.Abbr ||
                    cc_DMSMember.ProvinceID != DMSMember.ProvinceID ||
                    cc_DMSMember.CityID != DMSMember.CityID ||
                    cc_DMSMember.CountyID != DMSMember.CountyID ||
                    cc_DMSMember.MemberType != DMSMember.MemberType)
                {
                    model.ContrastType = 2;

                    GetDMSMemberContrastByPart1(cc_DMSMember, DMSMember,
                                         ref contrastInfoInside, ref contrastInfo);
                    if (contrastInfoInside != string.Empty &&
                        contrastInfo != string.Empty)
                    {
                        model.ContrastInfoInside = contrastInfoInside;
                        model.ContrastInfo = contrastInfo;
                        if (string.IsNullOrEmpty(DMSMember.MemberCode))//是否会员开通成功
                        {
                            model.DisposeStatus = 1;//已处理
                            model.DisposeTime = DateTime.Now;
                        }
                        else
                        {
                            model.DisposeStatus = 0;//未处理
                            model.DisposeTime = null;
                        }
                        isChange = true;
                        Insert(model);
                    }
                }
                contrastInfoInside = string.Empty;
                contrastInfo = string.Empty;

                //判断会员除去4个字段，是否有过更改
                if (cc_DMSMember.Phone != DMSMember.Phone ||
                    cc_DMSMember.Fax != DMSMember.Fax ||
                    cc_DMSMember.CompanyWebSite != DMSMember.CompanyWebSite ||
                    cc_DMSMember.Email != DMSMember.Email ||
                    cc_DMSMember.Postcode != DMSMember.Postcode ||
                    cc_DMSMember.ContactAddress != DMSMember.ContactAddress
                    //cc_DMSMember.TrafficInfo != DMSMember.TrafficInfo ||
                    //cc_DMSMember.EnterpriseBrief != DMSMember.EnterpriseBrief ||
                    //cc_DMSMember.Remarks != DMSMember.Remarks
                    )
                {
                    model.ContrastType = 3;

                    GetDMSMemberContrastByPart2(cc_DMSMember, DMSMember,
                                         ref contrastInfoInside, ref contrastInfo);
                    if (contrastInfoInside != string.Empty &&
                        contrastInfo != string.Empty)
                    {
                        model.ContrastInfoInside = contrastInfoInside;
                        model.ContrastInfo = contrastInfo;
                        if (string.IsNullOrEmpty(DMSMember.MemberCode))//是否会员开通成功
                        {
                            model.DisposeStatus = 1;//已处理
                            model.DisposeTime = DateTime.Now;
                        }
                        else
                        {
                            model.DisposeStatus = 0;//未处理
                            model.DisposeTime = null;
                        }
                        recid = Insert(model);
                    }
                }
            }
            return recid;
        }

        /// <summary>
        /// 获得会员4个字段的对照文本信息
        /// </summary>
        /// <param name="cc_DMSMember"></param>
        /// <param name="DMSMember"></param>
        /// <param name="contrastInfoInside"></param>
        /// <param name="contrastInfo"></param>
        private void GetDMSMemberContrastByPart1(Entities.ProjectTask_DMSMember cc_DMSMember, BitAuto.YanFa.Crm2009.Entities.DMSMember DMSMember, ref string contrastInfoInside, ref string contrastInfo)
        {
            if (cc_DMSMember.Name != DMSMember.Name)
            {
                contrastInfoInside += "Name:('" + Util.EscapeString(DMSMember.Name) + "','" + Util.EscapeString(cc_DMSMember.Name) + "'),";
                contrastInfo += "易湃会员名称由：" + DMSMember.Name + "，改为：" + cc_DMSMember.Name + ",";
            }
            if (cc_DMSMember.Abbr != DMSMember.Abbr)
            {
                contrastInfoInside += "Abbr:('" + Util.EscapeString(DMSMember.Abbr) + "','" + Util.EscapeString(cc_DMSMember.Abbr) + "'),";
                contrastInfo += "易湃会员简称由：" + DMSMember.Abbr + "，改为：" + cc_DMSMember.Abbr + ",";
            }
            if (cc_DMSMember.ProvinceID != DMSMember.ProvinceID)
            {
                contrastInfoInside += "ProvinceID:('" + Util.EscapeString(DMSMember.ProvinceID) + "','" + Util.EscapeString(cc_DMSMember.ProvinceID) + "'),";
                contrastInfo += "易湃会员所属省份由：" + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(DMSMember.ProvinceID) +
                                "，改为：" + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(cc_DMSMember.ProvinceID) + ",";
            }
            if (cc_DMSMember.CityID != DMSMember.CityID)
            {
                contrastInfoInside += "CityID:('" + Util.EscapeString(DMSMember.CityID) + "','" + Util.EscapeString(cc_DMSMember.CityID) + "'),";
                contrastInfo += "易湃会员所属城市由：" + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(DMSMember.CityID) +
                                "，改为：" + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(cc_DMSMember.CityID) + ",";
            }
            if (cc_DMSMember.CountyID != DMSMember.CountyID)
            {
                contrastInfoInside += "CountyID:('" + Util.EscapeString(DMSMember.CountyID) + "','" + Util.EscapeString(cc_DMSMember.CountyID) + "'),";
                contrastInfo += "易湃会员所属区县由：" + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(DMSMember.CountyID) +
                                "，改为：" + BitAuto.YanFa.Crm2009.BLL.MainBrand.Instance.GetAreaName(cc_DMSMember.CountyID) + ",";
            }
            if (cc_DMSMember.MemberType != DMSMember.MemberType)
            {
                contrastInfoInside += "MemberType:('" + Util.EscapeString(DMSMember.MemberType) + "','" + Util.EscapeString(cc_DMSMember.MemberType) + "'),";
                contrastInfo += "易湃会员类型由：" + GetMemberType(DMSMember.MemberType) +
                                "，改为：" + GetMemberType(cc_DMSMember.MemberType) + ",";
            }
            contrastInfoInside = contrastInfoInside.TrimEnd(',');
            contrastInfo = contrastInfo.TrimEnd(',');
        }

        /// <summary>
        /// 获得会员除去4个字段的对照文本信息
        /// </summary>
        /// <param name="cc_DMSMember"></param>
        /// <param name="DMSMember"></param>
        /// <param name="contrastInfoInside"></param>
        /// <param name="contrastInfo"></param>
        private void GetDMSMemberContrastByPart2(Entities.ProjectTask_DMSMember cc_DMSMember, BitAuto.YanFa.Crm2009.Entities.DMSMember DMSMember, ref string contrastInfoInside, ref string contrastInfo)
        {
            if (cc_DMSMember.Phone != DMSMember.Phone)
            {
                contrastInfoInside += "Phone:('" + Util.EscapeString(DMSMember.Phone) + "','" + Util.EscapeString(cc_DMSMember.Phone) + "'),";
                contrastInfo += "易湃会员电话由：" + DMSMember.Phone + "，改为：" + cc_DMSMember.Phone + ",";
            }
            if (cc_DMSMember.Fax != DMSMember.Fax)
            {
                contrastInfoInside += "Fax:('" + Util.EscapeString(DMSMember.Fax) + "','" + Util.EscapeString(cc_DMSMember.Fax) + "'),";
                contrastInfo += "易湃会员传真由：" + DMSMember.Fax + "，改为：" + cc_DMSMember.Fax + ",";
            }
            if (cc_DMSMember.CompanyWebSite != DMSMember.CompanyWebSite)
            {
                contrastInfoInside += "CompanyWebSite:('" + Util.EscapeString(DMSMember.CompanyWebSite) + "','" + Util.EscapeString(cc_DMSMember.CompanyWebSite) + "'),";
                contrastInfo += "易湃会员公司网址由：" + DMSMember.CompanyWebSite + "，改为：" + cc_DMSMember.CompanyWebSite + ",";
            }
            if (cc_DMSMember.Email != DMSMember.Email)
            {
                contrastInfoInside += "Email:('" + Util.EscapeString(DMSMember.Email) + "','" + Util.EscapeString(cc_DMSMember.Email) + "'),";
                contrastInfo += "易湃会员Email由：" + DMSMember.Email + "，改为：" + cc_DMSMember.Email + ",";
            }
            if (cc_DMSMember.Postcode != DMSMember.Postcode)
            {
                contrastInfoInside += "Postcode:('" + Util.EscapeString(DMSMember.Postcode) + "','" + Util.EscapeString(cc_DMSMember.Postcode) + "'),";
                contrastInfo += "易湃会员邮编由：" + DMSMember.Postcode + "，改为：" + cc_DMSMember.Postcode + ",";
            }
            if (cc_DMSMember.ContactAddress != DMSMember.ContactAddress)
            {
                contrastInfoInside += "ContactAddress:('" + Util.EscapeString(DMSMember.ContactAddress) + "','" + Util.EscapeString(cc_DMSMember.ContactAddress) + "'),";
                contrastInfo += "易湃会员销售地址由：" + DMSMember.ContactAddress + "，改为：" + cc_DMSMember.ContactAddress + ",";
            }
            contrastInfoInside = contrastInfoInside.TrimEnd(',');
            contrastInfo = contrastInfo.TrimEnd(',');
        }

        /// <summary>
        /// 获得会员类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetMemberType(string type)
        {
            string name = string.Empty;
            switch (type)
            {
                case "1":
                    name = "4S";
                    break;
                case "2":
                    name = "特许经销商";
                    break;
                case "3":
                    name = "综合店";
                    break;
                default:
                    break;
            }
            return name;
        }

        /// <summary>
        /// 删除该记录
        /// </summary>
        /// <param name="tid">任务ID</param>
        /// <param name="dd">对照类型ID</param>
        /// <returns></returns>
        public int DeleteByTIDAndContrastType(int tid, int contrastTypeID)
        {
            return Dal.ProjectTask_AuditContrastInfo.Instance.DeleteByTIDAndContrastType(tid, contrastTypeID);
        }
        //add by qizhiqiang 2012-5-16 根据membercode判断是否有排期
        /// <summary>
        /// 根据memberCode判断会员是否有排期
        /// </summary>
        /// <param name="membercode"></param>
        /// <returns></returns>
        public bool HavePeiQiByDMSMemberCode(string membercode)
        {
            return Dal.ProjectTask_AuditContrastInfo.Instance.HavePeiQiByDMSMemberCode(membercode);

        }
        /// <summary>
        /// 取所有有排期车易通变更记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetnineTypeProjectTask_AuditContrastInfo()
        {
            return Dal.ProjectTask_AuditContrastInfo.Instance.GetnineTypeProjectTask_AuditContrastInfo();
        }
        /// <summary>
        /// 取无排期已开通车易通变更记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetNoPeiQiProjectTask_AuditContrastInfo()
        {
            return Dal.ProjectTask_AuditContrastInfo.Instance.GetNoPeiQiProjectTask_AuditContrastInfo();
            //
        }
    }
}
