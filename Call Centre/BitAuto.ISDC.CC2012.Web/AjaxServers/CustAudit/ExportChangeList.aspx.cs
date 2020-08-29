using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustAudit
{
    public partial class ExportChangeList : PageBase
    {
        #region 定义属性
        public string RequestCreateBeginTime
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CreateBeginTime"); }
        }
        public string RequestCreateEndTime
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CreateEndTime"); }
        }
        ////查询类型：0 车易通；1 车商通
        //public string RequestType
        //{
        //    get { return BLL.Util.GetCurrentRequestStr("Type"); }
        //}
        /// <summary>
        /// 导出状态：1-已导出，0-未导出
        /// </summary>
        public int RequestExportStatus
        {
            get { return BLL.Util.GetCurrentRequestFormInt("ExportStatus"); }
        }
        /// <summary>
        /// 导出类型：
        /// 1----------客户名称修改
        /// 2----------客户下，会员4个字段（会员全称、会员简称、会员地区、会员类型）修改
        /// 3----------客户下，会员除去4个字段修改
        /// </summary>
        public int RequestContrastType
        {
            get { return BLL.Util.GetCurrentRequestFormInt("ContrastType"); }
        }
        /// <summary>
        /// 处理状态：0-未处理，1-已处理,2-未修改
        /// </summary>
        public int RequestDisposeStatus
        {
            get { return BLL.Util.GetCurrentRequestFormInt("DisposeStatus"); }
        }
        /// <summary>
        /// 客户或会员编号
        /// </summary>
        public string RequestCustIDORMemberID
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CustIDORMemberID"); }
        }
        /// <summary>
        /// 客户或会员名称
        /// </summary>
        public string RequestCustNameORMemberName
        {
            get { return BLL.Util.GetCurrentRequestFormStr("CustNameORMemberName"); }
        }
        /// <summary>
        /// 坐席名称
        /// </summary>
        public string RequestSeatTrueName
        {
            get { return BLL.Util.GetCurrentRequestFormStr("SeatTrueName"); }
        }

        /// <summary>
        /// 所属轮次：-1全部，1、2、3、4代表1、2、3、4轮
        /// </summary>
        public int RequestTaskBatch
        {
            get { return BLL.Util.GetCurrentRequestFormInt("TaskBatch"); }
        }

        ///// <summary>
        ///// 经营范围
        ///// </summary>
        //public string RequestCarType
        //{
        //    get { return BLL.Util.GetCurrentRequestFormStr("CarType"); }
        //}

        //add by qizhiqiang 2012-5-21取会员地区
        public int RequestMemberProvinceID
        {
            get { return BLL.Util.GetCurrentRequestFormInt("MemberProvince"); }
        }
        public int RequestMemberCityID
        {
            get { return BLL.Util.GetCurrentRequestFormInt("MemberCity"); }
        }
        public int RequestMemberCountyID
        {
            get { return BLL.Util.GetCurrentRequestFormInt("MemberCounty"); }
        }

        public int CountOfRecords = 0;
     
        #endregion
    
        public int GroupLength = 8;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                 BindDMSData();
            }
        }

        private void BindDMSData()
        {
            string areaTypeWhereStr = string.Empty;
            Entities.QueryProjectTask_AuditContrastInfo query = new Entities.QueryProjectTask_AuditContrastInfo();

            if (!string.IsNullOrEmpty(RequestCustIDORMemberID))
            {
                query.CustIDORMemberID = RequestCustIDORMemberID;
            }
            if (!string.IsNullOrEmpty(RequestCustNameORMemberName))
            {
                query.CustNameORMemberName = RequestCustNameORMemberName;
            }

            if (!string.IsNullOrEmpty(RequestCreateBeginTime))
            {
                query.CreateStartDate = RequestCreateBeginTime;
            }
            if (!string.IsNullOrEmpty(RequestCreateEndTime))
            {
                query.CreateEndDate = RequestCreateEndTime;
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

            //if (!string.IsNullOrEmpty(RequestCarType))
            //{
            //    query.CarType = RequestCarType;
            //}
            query.TaskBatch = RequestTaskBatch;
            DataTable dt = BLL.ProjectTask_AuditContrastInfo.Instance.GetProjectTask_AuditContrastInfo(query, "ac.CreateTime Desc", PageCommon.Instance.PageIndex, BLL.PageCommon.Instance.PageSize, out CountOfRecords);
            //DataTable dt = BLL.CC_AuditContrastInfo.Instance.GetCC_AuditContrastInfoForChange(query, "ac.CreateTime Desc", PageCommon.Instance.PageIndex, PageSize, out CountOfRecords);
            if (dt != null && dt.Rows.Count > 0)
            {
                //add by qizhiqiang 过滤StatID!='' and StatID!='0' AND StatID!='-2'
                DataView dv = new DataView(dt);
                dv.RowFilter = "StatID<>'' and StatID<>'0' AND StatID<>'-2'";
                DataTable dt_New = dv.ToTable();
                this.Repeater_ExportChanged.DataSource = dt_New;

                //this.Repeater_ExportChanged.DataSource = dt;
            }
            //绑定列表数据
            this.Repeater_ExportChanged.DataBind();
            //分页控件(要明确页面容器ID，这里没有写是因为请求页面在Request参数中声明了)
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, CountOfRecords, BLL.PageCommon.Instance.PageSize, BLL.PageCommon.Instance.PageIndex, 1);
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
                    case "9": //name = "已开通车易通信息变更";
                        name = "有排期车易通信息变更";
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
    }
}