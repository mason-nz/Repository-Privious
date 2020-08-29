using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class QS_IM_Result : CommonBll
    {
        public static QS_IM_Result Instance = new QS_IM_Result();

        public DataTable GetQS_IM_Result(QueryQS_IM_Result query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.QS_IM_Result.Instance.GetQS_IM_Result(query, order, currentPage, pageSize, out totalCount);
        }

        public DataRow GetQS_IM_ResultForCSID(string csid)
        {
            QueryQS_IM_Result query = new QueryQS_IM_Result();
            query.CSID = csid;
            int count = 0;
            DataTable dt = GetQS_IM_Result(query, "", 1, 100, out count);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            else return null;
        }

        public bool HasScored(string csid)
        {
            return Dal.QS_IM_Result.Instance.HasScored(csid);
        }

        /// 生成主键
        /// <summary>
        /// 生成主键
        /// </summary>
        /// <returns></returns>
        public int CreateQS_RID()
        {
            return Dal.QS_IM_Result.Instance.CreateQS_RID();
        }

        /// <summary>
        /// 根据RTID和CSID获取RTID
        /// </summary>
        /// <param name="qsRtid"></param>
        /// <param name="csid"></param>
        /// <returns></returns>
        public int GetRidByCsidAndRtid(int qsRtid, Int64 csid)
        {
            return Dal.QS_IM_Result.Instance.GetRidByCsidAndRtid(qsRtid,csid);
        }
    }
}
