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
    /// ҵ���߼���QS_RulesRange ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-25 09:42:37 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class QS_RulesRange
    {
        public static readonly QS_RulesRange Instance = new QS_RulesRange();

        protected QS_RulesRange()
        { }

        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetQS_RulesRange(QueryQS_RulesRange query, string order, int currentPage, int pageSize, out int totalCount)
        {
            int userid = Util.GetLoginUserID();
            return Dal.QS_RulesRange.Instance.GetQS_RulesRange(query, userid, order, currentPage, pageSize, out totalCount);
        }
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            int userid = Util.GetLoginUserID();
            return Dal.QS_RulesRange.Instance.GetQS_RulesRange(new QueryQS_RulesRange(), userid, string.Empty, 1, 1000000, out totalCount);
        }
        /// �༭Ӧ�÷�Χ
        /// <summary>
        /// �༭Ӧ�÷�Χ
        /// </summary>
        /// <param name="BGID"></param>
        /// <param name="QS_RTID"></param>
        /// <param name="QS_IM_RTID"></param>
        /// <returns></returns>
        public int RangeManage(int BGID, int QS_RTID, int QS_IM_RTID)
        {
            return Dal.QS_RulesRange.Instance.RangeManage(BGID, QS_RTID, QS_IM_RTID, BLL.Util.GetLoginUserID());
        }
        /// �������ȡʵ��
        /// <summary>
        /// �������ȡʵ��
        /// </summary>
        /// <param name="BGID"></param>
        /// <returns></returns>
        public Entities.QS_RulesRange getModelByBGID(int BGID)
        {
            return Dal.QS_RulesRange.Instance.getModelByBGID(BGID);
        }
    }
}

