using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.Demand.DemandHandle
{
    public partial class Export : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //增加“需求管理--需求处理” 导出功能验证逻辑
                 int userId = BLL.Util.GetLoginUserID();
                 if (BLL.Util.CheckRight(userId, "SYS024BUT101102")) //需求处理
                 {
                     WorkOrderBind();
                 }
                 else
                 {
                     Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                     Response.End();
                 }
            }
        }

        private void WorkOrderBind()
        {
            BitAuto.YanFa.Crm2009.Entities.YJKDemand.YJKDemandQuery query = BLL.Util.BindQuery<BitAuto.YanFa.Crm2009.Entities.YJKDemand.YJKDemandQuery>(this.Context);
            query.Where = " And YJKDemandInfo.Status<>" + (int)BitAuto.YanFa.Crm2009.Entities.YJKDemandStatus.Revoke;
            int total = 0;
            DataTable dt = BitAuto.YanFa.Crm2009.BLL.YJKDemandBLL.Instance.GetYJKDemandInfo(query, "", 1, 10000, out total, BLL.Util.GetLoginUserID());
            DataTable dtNew = new DataTable();

            DataColumn dcDemandID = new DataColumn("需求编号");
            dtNew.Columns.Add(dcDemandID);
            DataColumn dcMemberCode = new DataColumn("会员ID");
            dtNew.Columns.Add(dcMemberCode);
            DataColumn dcMemberName = new DataColumn("经销商名称");
            dtNew.Columns.Add(dcMemberName);
            DataColumn dcPeriod = new DataColumn("服务周期");
            dtNew.Columns.Add(dcPeriod);
            DataColumn dcExpectedNum = new DataColumn("集客数量");
            dtNew.Columns.Add(dcExpectedNum);
            DataColumn dcPracticalNum = new DataColumn("已集客数量");
            dtNew.Columns.Add(dcPracticalNum);
            DataColumn dcCreateTime = new DataColumn("申请日期");
            dtNew.Columns.Add(dcCreateTime);
            DataColumn dcStatus = new DataColumn("需求状态");
            dtNew.Columns.Add(dcStatus);
            DataColumn dcKeFuName = new DataColumn("负责客服");
            dtNew.Columns.Add(dcKeFuName);
            DataColumn dcSaleName = new DataColumn("负责销售");
            dtNew.Columns.Add(dcSaleName);
            DataColumn dcIsOverflow = new DataColumn("是否超量");
            dtNew.Columns.Add(dcIsOverflow);

            foreach (DataRow dr in dt.Rows)
            {
                DataRow drNew = dtNew.NewRow();
                drNew[dcDemandID] = dr["DemandID"].ToString();
                drNew[dcMemberCode] = dr["MemberCode"].ToString();
                drNew[dcMemberName] = dr["MemberName"].ToString();
                drNew[dcPeriod] = DateTime.Parse(dr["BeginTime"].ToString()).ToString("yyyy-MM-dd") + "至" + DateTime.Parse(dr["EndTime"].ToString()).ToString("yyyy-MM-dd");
                drNew[dcExpectedNum] = dr["ExpectedNum"];
                drNew[dcPracticalNum] = dr["PracticalNum"];
                drNew[dcCreateTime] = DateTime.Parse(dr["CreateTime"].ToString()).ToString("yyyy-MM-dd");
                drNew[dcStatus] = BitAuto.YanFa.Crm2009.BLL.DictInfo.Instance.GetDictName(int.Parse(dr["Status"].ToString()));
                drNew[dcKeFuName] = dr["KeFuName"].ToString();
                drNew[dcSaleName] = dr["SaleName"].ToString();
                drNew[dcIsOverflow] = dr["IsOverflow"].ToString() == "2" ? "是" : "否";
                dtNew.Rows.Add(drNew);
            }

            BLL.Util.ExportToCSV("需求处理结果表", dtNew, false);

        }
    }
}