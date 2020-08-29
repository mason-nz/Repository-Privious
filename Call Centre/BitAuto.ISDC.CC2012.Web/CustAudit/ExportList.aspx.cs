using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.CustAudit
{
    public partial class ExportList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 定义属性
        public string RequestCreateBeginTime
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CreateBeginTime"); }
        }
        public string RequestCreateEndTime
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CreateEndTime"); }
        }
        ///// <summary>
        ///// 导出状态：1-已导出，0-未导出
        ///// </summary>
        //public int RequestExportStatus
        //{
        //    get { return BLL.Util.GetCurrentRequestQueryInt("ExportStatus"); }
        //}
        /// <summary>
        /// 处理状态：1-已处理，0-未处理,2-未修改
        /// </summary>
        public int RequestDisposeStatus
        {
            get { return BLL.Util.GetCurrentRequestQueryInt("DisposeStatus"); }
        }
        /// <summary>
        /// 导出类型：
        /// 1----------客户名称修改
        /// 2----------客户下，会员4个字段（会员全称、会员简称、会员地区、会员类型）修改
        /// 3----------客户下，会员除去4个字段修改
        /// </summary>
        public int RequestContrastType
        {
            get { return BLL.Util.GetCurrentRequestQueryInt("ContrastType"); }
        }
        /// <summary>
        /// 浏览器标识，值为IE或FF
        /// </summary>
        public string RequestBrowser
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("Browser"); }
        }
        public string RequestExport
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("Export"); }
        }
        /// <summary>
        /// 客户或会员编号
        /// </summary>
        public string RequestCustIDORMemberID
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CustIDORMemberID"); }
        }
        /// <summary>
        /// 客户或会员名称
        /// </summary>
        public string RequestCustNameORMemberName
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("CustNameORMemberName"); }
        }
        /// <summary>
        /// 所属轮次：-1全部，1、2、3、4代表1、2、3、4轮
        /// </summary>
        public int RequestTaskBatch
        {
            get { return BLL.Util.GetCurrentRequestQueryInt("TaskBatch"); }
        }

        /// <summary>
        /// 坐席姓名
        /// </summary>
        public string RequestSeatTrueName
        {
            get { return BLL.Util.GetCurrentRequestQueryStr("SeatTrueName"); }
        }

        //add by qizhiqiang 2012-5-21取会员地区
        public int RequestMemberProvinceID
        {
            get { return Request.QueryString["MemberProvince"] == null ? -1 : int.Parse(Request.QueryString["MemberProvince"]); }
        }
        public int RequestMemberCityID
        {
            get { return Request.QueryString["MemberCity"] == null ? -1 : int.Parse(Request.QueryString["MemberCity"]); }
        }
        public int RequestMemberCountyID
        {
            get { return Request.QueryString["MemberCounty"] == null ? -1 : int.Parse(Request.QueryString["MemberCounty"]); }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (RequestExport.Equals("yes"))
            {
                DataTable dt = BindData();
                //    SetExportFormat();

                ExprotExcel(dt);
            }
        }

        private void ExprotExcel(DataTable dt)
        {

            dt.Columns.Add("CreateUserName");
            dt.Columns.Add("ContrastTypeName");
            dt.Columns.Add("DisposeStatusName");

            foreach (DataRow dr in dt.Rows)
            {
                dr["StatID"] = (dr["StatID"].ToString() == "0" || dr["StatID"].ToString() == "-2") ? String.Empty : dr["StatID"].ToString();
                dr["ContrastTypeName"] = GetContrastTypeName(dr["ContrastType"]);
                dr["DisposeStatusName"] = GetDisposeStatusName(dr["DisposeStatus"].ToString());
                dr["CreateUserName"] = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(int.Parse(dr["CreateUserID"].ToString()));

            }

            //要导出的字段
            Dictionary<string, string> ExportColums = new Dictionary<string, string>();
            ExportColums.Add("statid", "id");
            ExportColums.Add("statname", "名称");
            ExportColums.Add("contrastinfo", "变更详情");
            ExportColums.Add("contrasttypename", "变更类型");
            ExportColums.Add("disposestatusname", "处理状态");
            ExportColums.Add("disposetime", "处理时间");
            ExportColums.Add("remark", "备注");
            ExportColums.Add("createtime", "创建日期");
            ExportColums.Add("createusername", "创建人");         

            //字段排序
            dt.Columns["StatID"].SetOrdinal(0);
            dt.Columns["StatName"].SetOrdinal(1);
            dt.Columns["ContrastInfo"].SetOrdinal(2);
            dt.Columns["ContrastTypeName"].SetOrdinal(3);
            dt.Columns["DisposeStatusName"].SetOrdinal(4);
            dt.Columns["DisposeTime"].SetOrdinal(5);
            dt.Columns["Remark"].SetOrdinal(6);
            dt.Columns["CreateTime"].SetOrdinal(7);
            dt.Columns["CreateUserName"].SetOrdinal(8);

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

            BLL.Util.ExportToCSV("导出变更信息详细记录", dt);

        }
        private void SetExportFormat()
        {
            string typeName = "变更信息";
            //switch (exportType.ToLower().Trim())
            //{
            //    case "employee":
            //        typeName = "员工";
            //        break;
            //    case "departstat":
            //        typeName = "部门统计";
            //        break;
            //    default:
            //        break;
            //}
            HttpResponse response = Page.Response;
            response.Clear();
            response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");//指定
            response.Charset = "utf-8";//指定
            //使用UTF-8对文件名进行编码
            if (RequestBrowser == "IE")
            {
                response.AppendHeader("Content-Disposition", "attachment;filename=\"" + HttpUtility.UrlEncode("导出" + typeName + "详细记录.xls", System.Text.Encoding.UTF8) + "\"");
            }
            else if (RequestBrowser == "FF")
            {
                response.AppendHeader("Content-Disposition", "attachment;filename=\"导出" + typeName + "详细记录.xls\"");
            }
            else
            {
                response.End();
            }
            response.ContentType = "application/ms-excel;";
        }

        private DataTable BindData()
        {
            string areaTypeWhereStr = string.Empty;
            Entities.QueryProjectTask_AuditContrastInfo query = new Entities.QueryProjectTask_AuditContrastInfo();

            if (!string.IsNullOrEmpty(RequestCreateBeginTime))
            {
                query.CreateStartDate = RequestCreateBeginTime;
            }
            if (!string.IsNullOrEmpty(RequestCreateEndTime))
            {
                query.CreateEndDate = RequestCreateEndTime;
            }
            if (!string.IsNullOrEmpty(RequestCustIDORMemberID))
            {
                query.CustIDORMemberID = RequestCustIDORMemberID;
            }
            if (!string.IsNullOrEmpty(RequestCustNameORMemberName))
            {
                query.CustNameORMemberName = RequestCustNameORMemberName;
            }
            //if (RequestExportStatus != -1)
            //{
            //    query.ExportStatus = RequestExportStatus;
            //}
            if (RequestContrastType != -1)
            {
                query.ContrastType = RequestContrastType;
            }
            if (RequestDisposeStatus != -1)
            {
                query.DisposeStatus = RequestDisposeStatus;
            }
            if (!string.IsNullOrEmpty(RequestSeatTrueName))
            {
                query.SeatTrueName = RequestSeatTrueName;
            }
            if (RequestTaskBatch != -1)
            {
                query.TaskBatch = RequestTaskBatch;
            }

            //**add by qizhiqiang 2012-5-21
            if (RequestMemberProvinceID > 0)
            {

                query.MemberProvinceID = RequestMemberProvinceID.ToString();

            }

            if (RequestMemberCityID > 0)
            {

                query.MemberCityID = RequestMemberCityID.ToString();


            }
            if (RequestMemberCountyID > 0)
            {

                query.MemberCountyID = RequestMemberCountyID.ToString();

            }
            //**

            //BLL.CC_AuditContrastInfo.Instance.BatchUpdateExportStatusByWhere(query, 1);
            int CountOfRecords = 0;
            DataTable dt = BLL.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfo(query, "ac.CreateTime Desc", 1, 1000000, out CountOfRecords);
            if (dt != null && dt.Rows.Count > 0)
            {
                //add by qizhiqiang 过滤StatID!='' and StatID!='0' AND StatID!='-2'
                DataView dv = new DataView(dt);
                dv.RowFilter = "StatID<>'' and StatID<>'0' AND StatID<>'-2'";
                DataTable dt_New = dv.ToTable();

                return dt_New;
                //this.repterExportList.DataSource = dt_New;
            }
            else
            {
                return null;
            }
            ////绑定列表数据
            //this.repterExportList.DataBind();
        }


        public string GetExportStatusName(object str)
        {
            string name = string.Empty;
            if (!string.IsNullOrEmpty(Convert.ToString(str)))
            {
                switch (Convert.ToString(str).Trim())
                {
                    case "0": name = "未导出";
                        break;
                    case "1": name = "已导出";
                        break;
                    default:
                        break;
                }
            }
            return name;
        }
        public string GetDisposeStatusName(object str)
        {
            string name = string.Empty;
            if (!string.IsNullOrEmpty(Convert.ToString(str)))
            {
                switch (Convert.ToString(str).Trim())
                {
                    case "0": name = "未处理";
                        break;
                    case "1": name = "已处理";
                        break;
                    case "2": name = "未修改";
                        break;
                    default:
                        break;
                }
            }
            return name;
        }
        public string GetContrastTypeName(object str)
        {
            string name = string.Empty;
            if (!string.IsNullOrEmpty(Convert.ToString(str)))
            {
                switch (Convert.ToString(str).Trim())
                {
                    case "1": name = "客户名称变更";
                        break;
                    case "2": name = "易湃会员4个关键项变更";
                        break;
                    case "3": name = "易湃会员其他信息变更";
                        break;
                    case "4": name = "客户名称变更（有重名）";
                        break;
                    case "5": name = "客户停用申请";
                        break;
                    case "6": name = "易湃会员主营品牌变更";
                        break;
                    case "7": name = "车商通会员4个关键项变更";
                        break;
                    case "8": name = "车商通会员3个关键项变更";
                        break;
                    case "9": name = "有排期车易通信息变更";
                        break;
                    default:
                        break;
                }
            }
            return name;
        }
    }
}