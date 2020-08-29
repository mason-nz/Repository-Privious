using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BitAuto.Utils.Data;

namespace BitAuto.ISDC.CC2012.BLL.BigScreen
{
    public class SkillGropLiveData4Daping
    {
        private SkillGropLiveData4Daping()
        {
        }


        public static SkillGropLiveData4Daping Instance = new SkillGropLiveData4Daping();

        /// 明细表数据入库
        /// <summary>
        /// 明细表数据入库
        /// </summary>
        public void BulkCopyToDB_Detail(DataTable dt)
        {
            //清空临时表
            Dal.AutoCallSyncData.Instance.ClearTemp_Detail();

            List<SqlBulkCopyColumnMapping> list = new List<SqlBulkCopyColumnMapping>();
            list.Add(new SqlBulkCopyColumnMapping("SkillID", "SkillID"));
            list.Add(new SqlBulkCopyColumnMapping("AgentTotal", "AgentTotal"));
            list.Add(new SqlBulkCopyColumnMapping("AgentReady", "AgentReady"));
            list.Add(new SqlBulkCopyColumnMapping("AgentNotReady", "AgentNotReady"));
            list.Add(new SqlBulkCopyColumnMapping("AgentOtherWork", "AgentOtherWork"));

            list.Add(new SqlBulkCopyColumnMapping("AgentTalking", "AgentTalking"));
            list.Add(new SqlBulkCopyColumnMapping("AgentAfterCallWork", "AgentAfterCallWork"));
            list.Add(new SqlBulkCopyColumnMapping("ContactInbound", "ContactInbound"));
            list.Add(new SqlBulkCopyColumnMapping("ContactOutbound", "ContactOutbound"));

            list.Add(new SqlBulkCopyColumnMapping("InboundConnect", "InboundConnect"));
            list.Add(new SqlBulkCopyColumnMapping("OutboundConnect", "OutboundConnect"));
            list.Add(new SqlBulkCopyColumnMapping("AgentOtherstatus", "AgentOtherstatus"));
            list.Add(new SqlBulkCopyColumnMapping("ContactInQueue", "ContactInQueue"));

            list.Add(new SqlBulkCopyColumnMapping("MaxRequestLevel", "MaxRequestLevel"));
            //入库
            string msg = "";
            Util.BulkCopyToDB(dt, Dal.AutoCallSyncData.Instance.CC, "SkillGropLiveData4Daping", 10000, list, out msg);
        }

        public void CleanSkillGropLiveDataOldData()
        {           
            SqlHelper.ExecuteNonQuery(Dal.AutoCallSyncData.Instance.CC, CommandType.StoredProcedure,
                "CleanSkillGropLiveDataOldData");
        }


    }
}
