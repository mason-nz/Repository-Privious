using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using System.Data.OleDb;
using System.Collections;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Web.Util;
using System.Web.SessionState;
using System.Text.RegularExpressions;

namespace BitAuto.ISDC.CC2012.Web.Statistics.MemberIDImport
{
    public partial class MemberInfoExport : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        //public string MemberStr
        //{
        //    get { return Request.QueryString["MemberStr"] == null ? string.Empty : Request.QueryString["MemberStr"].Trim(); }
        //}
        public string MemberStr
        {
            get { return BLL.Util.GetCurrentRequestStr("hidData"); }
        }

        /// <summary>
        /// 导出类型（Member:会员信息   CstMember:车商通信息 ）
        /// </summary>
        public string ExportTyle
        {
            get { return BLL.Util.GetCurrentRequestStr("hidType"); }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                switch (ExportTyle)
                {
                    case "Member": ExportMember(); break;
                    case "CustIDYP": ExportCustIDByMemberCodeYP(); break;

                    case "CstMember": ExportCstMember(); break;
                    case "CustIDCST": ExportCustIDByMemberCodeCST(); break;
                }
            }
        }

        /// <summary>
        /// 车商通，根据会员号导出客户ID
        /// </summary>
        private void ExportCustIDByMemberCodeCST()
        {
            BitAuto.ISDC.CC2012.BLL.CRMCSTMember crmcstmeberbll = new CRMCSTMember();
            DataTable dt = null;
            string username = Session["truename"].ToString();
            dt = crmcstmeberbll.GetCustIDByMemberCode(username, MemberStr);
            //导出数据
            if (dt != null)
            {
                ExprotExcelCustIDByMemberCodeYP(dt);
            }
        }

        /// <summary>
        /// 易湃，根据会员号导出客户ID
        /// </summary>
        private void ExportCustIDByMemberCodeYP()
        {
            BitAuto.ISDC.CC2012.BLL.CRMDMSMember crmdmsmeberbll = new CRMDMSMember();
            DataTable dt = null;
            string username = Session["truename"].ToString();
            dt = crmdmsmeberbll.GetCustIDByMemberCode(username, MemberStr);
            //导出数据
            if (dt != null)
            {
                ExprotExcelCustIDByMemberCodeYP(dt);
            }
        }

        #region 导出方法
        /// <summary>
        /// 车商通、易湃共用此方法
        /// </summary>
        /// <param name="dtCost"></param>
        private void ExprotExcelCustIDByMemberCodeYP(DataTable dtCost)
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("会员ID\t,客户ID");
            foreach (DataRow dr in dtCost.Rows)
            {
                sw.WriteLine(dr["MemberCode"] + "\t," + dr["CustID"]);
            }
            sw.Close();
            Response.AddHeader("Content-Disposition", "attachment; filename=memberinfo.csv");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.Write(sw);
            Response.End();
        }

        #endregion

        /// <summary>
        /// 导出车商通
        /// </summary>
        private void ExportCstMember()
        {
            //daochu 

            DataTable dt = BitAuto.ISDC.CC2012.BLL.CC_CSTMember.Instance.GetOrderInfo(MemberStr);

            if (dt != null)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    dr["排期创建时间"] = (Convert.ToDateTime(dr["排期创建时间"].ToString())).ToString("yyyy-MM-dd");
                }

                CreateEXCEL(dt);
            }

        }

        /// <summary>
        /// 导出会员
        /// </summary>
        private void ExportMember()
        {
            string[] memberCodes = MemberStr.Split(',');
            WebService.CRM.CRMCooperationInfo service = new WebService.CRM.CRMCooperationInfo();
            DataTable dt = service.GetMemberCoorperation(memberCodes);
            //导出数据
            if (dt != null)
            {
                ExprotExcel(dt);
            }
        }

        #region 导出方法
        private void ExprotExcel(DataTable dtCost)
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("省份\t,城市\t,区县\t,经销商名称\t,品牌\t,类别\t,会员ID号\t,类型\t,合作名称\t,销售类型\t,执行周期\t,创建时间\t,确认时间\t,备注");
            foreach (DataRow dr in dtCost.Rows)
            {
                string confirmtime = "";
                if (dr["确认时间"] != DBNull.Value && dr["确认时间"].ToString() != "")
                {
                    confirmtime = Convert.ToDateTime(dr["确认时间"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                }
                sw.WriteLine(dr["会员省份"] + "\t," + dr["会员城市"] + "\t," + dr["会员区县"].ToString() + "\t," + dr["经销商名称"] + "\t,\"" + dr["经销商品牌"].ToString() + "\"\t," + dr["会员类别"] + "\t," + dr["经销商编号"].ToString() + "\t," + dr["订单类型"] + "\t," + dr["合作名称"] + "\t," + dr["销售类型"] + "\t," + dr["执行周期"].ToString() + "\t," + dr["创建时间"] + "\t," + confirmtime + "\t," + dr["备注"]);
            }
            sw.Close();
            Response.AddHeader("Content-Disposition", "attachment; filename=memberinfo.csv");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.Write(sw);
            Response.End();
        }

        /// <summary>
        /// 导出EXCEL（HTML格式的）
        /// </summary>
        /// <param name="ds">要导出的DataSet</param>
        /// <param name="fileName"></param>
        public void CreateEXCEL(DataTable dtCost)
        {
            //会员ID	会员名称	会员省份	会员城市	会员区县	合作名称	销售类型	执行周期	排期创建时间
            StringWriter sw = new StringWriter();
            sw.WriteLine("会员ID\t,会员名称\t,会员省份\t,会员城市\t,会员区县\t,合作名称\t,销售类型\t,执行周期\t,排期创建时间");
            foreach (DataRow dr in dtCost.Rows)
            {
                sw.WriteLine(dr["会员ID"] + "\t," + dr["会员名称"] + "\t," + dr["会员省份"].ToString() + "\t," + dr["会员城市"] + "\t," + dr["会员区县"] + "\t," + dr["合作名称"] + "\t," + dr["销售类型"].ToString() + "\t," + dr["执行周期"] + "\t," + dr["排期创建时间"]);
            }
            sw.Close();
            Response.AddHeader("Content-Disposition", "attachment; filename=cstmemberinfo.csv");
            Response.ContentType = "application/ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.Write(sw);
            Response.End();

        }

        #endregion
    }
}