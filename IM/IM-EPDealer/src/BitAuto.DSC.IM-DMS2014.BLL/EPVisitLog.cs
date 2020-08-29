using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���EPVisitLog ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:03 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class EPVisitLog : CommonBll
    {
        public static readonly new EPVisitLog Instance = new EPVisitLog();

        protected EPVisitLog()
        { }

        /// ���ղ�ѯ������ѯ
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetEPVisitLog(QueryEPVisitLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.EPVisitLog.Instance.GetEPVisitLog(query, order, currentPage, pageSize, out totalCount);
        }
        /// ��������б�
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.EPVisitLog.Instance.GetEPVisitLog(new QueryEPVisitLog(), string.Empty, 1, 1000000, out totalCount);
        }

        public DataTable GetDMSMemberByProvince(string provinceid)
        {
            return Dal.EPVisitLog.Instance.GetDMSMemberByProvince(provinceid);
        }
    }
}

