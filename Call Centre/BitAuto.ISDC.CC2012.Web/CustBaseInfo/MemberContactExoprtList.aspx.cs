using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;


namespace BitAuto.ISDC.CC2012.Web.CustBaseInfo
{
    public partial class MemberContactExoprtList : PageBase
    {
        #region 定义属性
        //public string RequestShowDMSMemberPart
        //{
        //    get { return BLL.Util.GetCurrentRequestStr("ShowDMSMemberPart"); }
        //}
        public string RequestDMSMemberCode
        {
            get { return BLL.Util.GetCurrentRequestStr("DMSMemberCode"); }
        }
        public string RequestDMSMemberName
        {
            get { return BLL.Util.GetCurrentRequestStr("DMSMemberName"); }
        }
        public string RequestProvinceID
        {
            get { return BLL.Util.GetCurrentRequestStr("ProvinceID"); }
        }
        public string RequestCityID
        {
            get { return BLL.Util.GetCurrentRequestStr("CityID"); }
        }
        public string RequestCountyID
        {
            get { return BLL.Util.GetCurrentRequestStr("CountyID"); }
        }
        public string SelectDeptID
        {
            get { return BLL.Util.GetCurrentRequestStr("selectDeptID"); }
        }

        public string StrDeptS
        {
            get { return BLL.Util.GetCurrentRequestStr("strDeptS"); }
        }

        /// <summary>
        /// 创建会员状态（1：呼叫中心创建的会员，0：区域创建的会员），格式为：1,2,3,4
        /// </summary>
        public string RequestAddMemberFlag
        {
            get { return BLL.Util.GetCurrentRequestStr("AddMemberFlag"); }
        }
        /// <summary>
        /// 负责员工（1：呼叫中心部门的负责员工，0：非呼叫中心部门的负责员工），格式为：1,2,3,4
        /// </summary>
        public string RequestUserMappingFlag
        {
            get { return BLL.Util.GetCurrentRequestStr("UserMappingFlag"); }
        }
        /// <summary>
        /// 回访记录（1：呼叫中心部门的回访记录，0：非呼叫中心部门的回访记录），格式为：1,2,3,4
        /// </summary>
        public string RequestReturnVisitFlag
        {
            get { return BLL.Util.GetCurrentRequestStr("ReturnVisitFlag"); }
        }
        ///// <summary>
        ///// 区域类型为1的值（1:86城）
        ///// </summary>
        //public string RequestAreaType86_Value
        //{
        //    get { return BLL.Util.GetCurrentRequestStr("AreaType86_Value"); }
        //}
        ///// <summary>
        ///// 86城的区域类型值（1:市区，2：郊区）
        ///// </summary>
        //public string RequestAreaValue86
        //{
        //    get { return BLL.Util.GetCurrentRequestStr("AreaValue86"); }
        //}
        ///// <summary>
        ///// 区域类型为1的值（2：246城）
        ///// </summary>
        //public string RequestAreaType246_Value
        //{
        //    get { return BLL.Util.GetCurrentRequestStr("AreaType246_Value"); }
        //}
        ///// <summary>
        ///// 会员销售类型
        ///// </summary>
        //public int RequestMemberCooperateStatus
        //{
        //    get { return BLL.Util.GetCurrentRequestFormInt("MemberCooperateStatus"); }
        //}
        /// <summary>
        /// 会员创建开始时间
        /// </summary>
        public string RequestMemberCreateTimeStart
        {
            get { return BLL.Util.GetCurrentRequestStr("MemberCreateTimeStart"); }
        }
        /// <summary>
        /// 会员创建结束时间
        /// </summary>
        public string RequestMemberCreateTimeEnd
        {
            get { return BLL.Util.GetCurrentRequestStr("MemberCreateTimeEnd"); }
        }
        /// <summary>
        /// 回访记录开始时间
        /// </summary>
        public string RequestReturnVisitTimeStart
        {
            get { return BLL.Util.GetCurrentRequestStr("ReturnVisitTimeStart"); }
        }
        /// <summary>
        /// 回访记录结束时间
        /// </summary>
        public string RequestReturnVisitTimeEnd
        {
            get { return BLL.Util.GetCurrentRequestStr("ReturnVisitTimeEnd"); }
        }
        /// <summary>
        /// 会员行政区划(1-117城区, 2-117郊区, 3-224无人城城区，3-224无人城郊区)
        /// </summary>
        public string RequestAreaTypeIDs
        {
            get { return BLL.Util.GetCurrentRequestStr("AreaTypeIDs"); }
        }
        /// <summary>
        /// 会员主营品牌IDs
        /// </summary>
        public string RequestBrandIDs
        {
            get { return BLL.Util.GetCurrentRequestStr("BrandIDs"); }
        }
        /// <summary>
        /// 曾经合作状态IDs,1--有排期，0--无排期
        /// </summary>
        public string CooperatedStatusIDs
        {
            get { return BLL.Util.GetCurrentRequestStr("CooperatedStatusIDs"); }
        }
        /// <summary>
        /// 有排期开始时间
        /// </summary>
        public string RequestBeginMemberCooperatedTime
        {
            get { return BLL.Util.GetCurrentRequestStr("BeginMemberCooperatedTime"); }
        }
        /// <summary>
        /// 有排期结束时间
        /// </summary>
        public string RequestEndMemberCooperatedTime
        {
            get { return BLL.Util.GetCurrentRequestStr("EndMemberCooperatedTime"); }
        }
        /// <summary>
        /// 无排期开始时间
        /// </summary>
        public string RequestBeginNoMemberCooperatedTime
        {
            get { return BLL.Util.GetCurrentRequestStr("BeginNoMemberCooperatedTime"); }
        }
        /// <summary>
        /// 无排期结束时间
        /// </summary>
        public string RequestEndNoMemberCooperatedTime
        {
            get { return BLL.Util.GetCurrentRequestStr("EndNoMemberCooperatedTime"); }
        }
        /// <summary>
        /// 会员合作状态：销售或试用，如1,2
        /// </summary>
        public string RequestMemberCooperateStatus
        {
            get { return BLL.Util.GetCurrentRequestStr("MemberCooperateStatus"); }
        }
        /// <summary>
        /// 会员类别，如1,2
        /// </summary>
        public string RequestMemberTypeIDs
        {
            get { return BLL.Util.GetCurrentRequestStr("MemberTypeIDs"); }
        }

        /// <summary>
        /// 客户联系人职级（枚举）
        /// </summary>
        public int RequestContactOfficeTypeCode
        {
            get { return BLL.Util.GetCurrentRequestInt("ContactOfficeTypeCode"); }
        }
        /// <summary>
        /// 浏览器标识，值为IE或FF
        /// </summary>
        public string RequestBrowser
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("Browser"); }
        }

        public string RequestExecCycle
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("ExecCycle"); }
        }
        public string RequestIsReturnMagazine
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("IsReturnMagazine"); }
        }
        /// <summary>
        /// 会员状态 add by yangyh date=2013-08-26
        /// </summary>
        public string RequestMemberSyncStatus
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("MemberSyncStatus"); }
        }
        public int CountOfRecords = 0;
        //public int PageSize = 20;
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //增加“客户池--易湃客户”联系人导出功能验证逻辑
                int userId = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userId, "SYS024BUG200303"))
                {
                    DataTable dt = GetData();

                    ExportDataToExcel(dt);
                }
                else
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
            }
        }

        private void ExportDataToExcel(DataTable dt)
        {
            dt.Columns.Add("OfficeTypeCodeName", typeof(string));

            //修改数据
            foreach (DataRow dr in dt.Rows)
            {
                dr["OfficeTypeCodeName"] = GetOfficeTypeName(dr["OfficeTypeCode"].ToString());
            }

            //要导出的字段
            Dictionary<string, string> ExportColums = new Dictionary<string, string>();
            ExportColums.Add("custid", "客户id");
            ExportColums.Add("custname", "客户名称");
            ExportColums.Add("membercode", "会员id");
            ExportColums.Add("dmsname", "会员名称");
            ExportColums.Add("cname", "联系人");
            ExportColums.Add("title", "职务");
            ExportColums.Add("officetypecodename", "职级");
            ExportColums.Add("contactaddress", "会员地址");
            ExportColums.Add("postcode", "会员邮编");
            ExportColums.Add("provincename", "省份");
            ExportColums.Add("cityname", "城市");
            ExportColums.Add("countyname", "区县");
            ExportColums.Add("custzipcode", "客户邮编");
            ExportColums.Add("brandnames", "主营品牌");

            //字段排序
            dt.Columns["CustID"].SetOrdinal(0);
            dt.Columns["CustName"].SetOrdinal(1);
            dt.Columns["MemberCode"].SetOrdinal(2);
            dt.Columns["DMSName"].SetOrdinal(3);
            dt.Columns["CName"].SetOrdinal(4);
            dt.Columns["Title"].SetOrdinal(5);
            dt.Columns["OfficeTypeCodeName"].SetOrdinal(6);
            dt.Columns["contactaddress"].SetOrdinal(7);
            dt.Columns["postcode"].SetOrdinal(8);
            dt.Columns["ProvinceName"].SetOrdinal(9);
            dt.Columns["CityName"].SetOrdinal(10);
            dt.Columns["CountyName"].SetOrdinal(11);
            dt.Columns["CustZipCode"].SetOrdinal(12);
            dt.Columns["BrandNames"].SetOrdinal(13);

            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                if (ExportColums.ContainsKey(dt.Columns[i].ColumnName.ToLower()))
                {
                    //字段时要导出的字段，改名
                    dt.Columns[i].ColumnName = ExportColums[dt.Columns[i].ColumnName.ToLower()];
                }
                else
                {
                    //不是要导出的字段，删除
                    dt.Columns.RemoveAt(i);
                }
            }

            BLL.Util.ExportToCSV("易湃会员联系人", dt);
        }

        private DataTable GetData()
        {
            string areaTypeWhereStr = string.Empty;
            Entities.QueryCRMDMSMember query = new Entities.QueryCRMDMSMember();

            if (!string.IsNullOrEmpty(RequestDMSMemberCode))
            {
                query.DMSMemberCode = RequestDMSMemberCode;
            }
            if (!string.IsNullOrEmpty(RequestDMSMemberName))
            {
                query.DMSMemberName = RequestDMSMemberName;
            }
            if (!string.IsNullOrEmpty(RequestProvinceID))
            {
                query.ProvinceID = RequestProvinceID;
            }
            if (!string.IsNullOrEmpty(RequestCityID))
            {
                query.CityID = RequestCityID;
            }
            if (!string.IsNullOrEmpty(RequestCountyID))
            {
                query.CountyID = RequestCountyID;
            }
            if (!string.IsNullOrEmpty(RequestAreaTypeIDs))
            {
                query.AreaTypeIDs = RequestAreaTypeIDs;
            }
            if (RequestAddMemberFlag.Equals("1"))
            {
                query.IsCCCreate = true;
            }
            else if (RequestAddMemberFlag.Equals("0"))
            {
                query.IsCCCreate = false;
            }

            if (RequestUserMappingFlag.Equals("1"))
            {
                query.IsCCUserMapping = true;
            }
            else if (RequestUserMappingFlag.Equals("0"))
            {
                query.IsCCUserMapping = false;
            }

            if (RequestReturnVisitFlag.Equals("1"))
            {
                query.IsCCReturnVisit = true;
            }
            else if (RequestReturnVisitFlag.Equals("0"))
            {
                query.IsCCReturnVisit = false;
            }
            if (!string.IsNullOrEmpty(SelectDeptID))
            {
                query.SelectDeptID = SelectDeptID;
            }
            if (!string.IsNullOrEmpty(StrDeptS))
            {
                if (StrDeptS.Trim() != "1,0" && StrDeptS.Trim() != "1,0")
                {
                    query.StrDeptS = StrDeptS;
                }
            }
            if (!string.IsNullOrEmpty(RequestMemberCreateTimeStart))
            {
                query.MemberCreateTimeStart = RequestMemberCreateTimeStart;
            }
            if (!string.IsNullOrEmpty(RequestMemberCreateTimeEnd))
            {
                query.MemberCreateTimeEnd = RequestMemberCreateTimeEnd;
            }
            if (!string.IsNullOrEmpty(RequestReturnVisitTimeStart))
            {
                query.ReturnVisitTimeStart = RequestReturnVisitTimeStart;
            }
            if (!string.IsNullOrEmpty(RequestReturnVisitTimeEnd))
            {
                query.ReturnVisitTimeEnd = RequestReturnVisitTimeEnd;
            }
            if (!string.IsNullOrEmpty(RequestBrandIDs))
            {
                query.BrandIDs = BLL.Util.GetStringBySplitArray(RequestBrandIDs);
            }

            if (CooperatedStatusIDs != string.Empty)
            {
                query.CooperatedStatusIDs = CooperatedStatusIDs;
            }
            if (!string.IsNullOrEmpty(RequestBeginMemberCooperatedTime))
            {
                query.BeginMemberCooperatedTime = RequestBeginMemberCooperatedTime;
            }
            if (!string.IsNullOrEmpty(RequestEndMemberCooperatedTime))
            {
                query.EndMemberCooperatedTime = RequestEndMemberCooperatedTime;
            }
            if (!string.IsNullOrEmpty(RequestBeginNoMemberCooperatedTime))
            {
                query.BeginNoMemberCooperatedTime = RequestBeginNoMemberCooperatedTime;
            }
            if (!string.IsNullOrEmpty(RequestEndNoMemberCooperatedTime))
            {
                query.EndNoMemberCooperatedTime = RequestEndNoMemberCooperatedTime;
            }
            if (!string.IsNullOrEmpty(RequestMemberCooperateStatus))
            {
                query.MemberCooperateStatus = RequestMemberCooperateStatus;
            }
            if (!string.IsNullOrEmpty(RequestMemberTypeIDs))
            {
                query.MemberType = BLL.Util.GetStringBySplitArray(RequestMemberTypeIDs);
            }
            if (RequestContactOfficeTypeCode > 0)
            {
                query.CustContactOfficeTypeCode = RequestContactOfficeTypeCode;
            }
            if (!string.IsNullOrEmpty(RequestIsReturnMagazine))
            {
                if (RequestIsReturnMagazine == "0")
                {
                    query.IsMagazineReturn = 0;
                }
                else if (RequestIsReturnMagazine == "1")
                {
                    query.IsMagazineReturn = 1;
                    if (!string.IsNullOrEmpty(RequestExecCycle))
                    {
                        query.ExecCycle = RequestExecCycle;
                    }
                }
            }
            if (!string.IsNullOrEmpty(RequestMemberSyncStatus) && RequestMemberSyncStatus != "-1")
            {
                query.MemberSyncStatus = RequestMemberSyncStatus;
            }
            query.Status = 0;
            DataTable dt = BLL.CRMDMSMember.Instance.GetCRMDMSMemberInfo(query, "DMSMember.CustID Desc", 1, 1000000, out CountOfRecords);
            return dt;
        }

        public string GetOfficeTypeName(string OfficeTypeCode)
        {
            string temp = string.Empty;
            switch (OfficeTypeCode.Trim())
            {
                case "160001": temp = "总裁（股东/董事长/董事/总裁…）";
                    break;
                case "160002": temp = "高管（高层/总经理/副总经理/店长…）";
                    break;
                case "160003": temp = "总监（中层/市场总监/销售总监…）";
                    break;
                case "160004": temp = "经理（基层/部门经理/主管…）";
                    break;
                case "160005": temp = "专员（员工/市场/销售/财务/客服/公关…）";
                    break;
                case "160000": temp = "其它";
                    break;
                default: break;
            }
            return temp;
        }

        #region 设置导出格式
        private void SetExportFormat()
        {
            HttpResponse response = Page.Response;
            response.Clear();
            response.ContentType = "application/octet-stream";
            //使用UTF-8对文件名进行编码
            if (RequestBrowser == "IE")
            {
                response.AppendHeader("Content-Disposition", "attachment;filename=\"" + HttpUtility.UrlEncode("导出会员联系人记录.xls", System.Text.Encoding.UTF8) + "\"");
            }
            else if (RequestBrowser == "FF")
            {
                response.AppendHeader("Content-Disposition", "attachment;filename=\"导出会员联系人记录.xls\"");
            }
            else
            {
                response.End();
            }
            response.ContentType = "application/ms-excel;";
        }

        #endregion
    }
}