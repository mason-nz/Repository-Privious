using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���ConverSationDetail ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2014-10-29 10:21:01 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class ConverSationDetail
    {
        #region Instance
        public static readonly ConverSationDetail Instance = new ConverSationDetail();
        #endregion

        #region Contructor
        protected ConverSationDetail()
        { }
        #endregion

        #region Select
        /// <summary>
        /// ��ȡ�Ự��ϸ�ı���
        /// </summary>
        /// <returns>���ر���</returns>
        public string GetSationDetailName()
        {
            return Dal.ConverSationDetail.Instance.GetSationDetailName();
        }

        /// <summary>
        ///  ���ݻỰID��ѯ��ϸ
        /// </summary>
        /// <param name="CSID">��ԱID</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��</param>
        /// <param name="pageSize">ҳ��</param>
        /// <param name="totalCount">����</param>
        /// <returns></returns>
        public DataTable GetDetailByCSID(int CSID, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ConverSationDetail.Instance.GetDetailByCSID(CSID, order, currentPage, pageSize, out totalCount);
        }


        /// <summary>
        /// ���ݻỰID��ѯ������ϸ(���Ƽ�ʹ��)
        /// </summary>
        /// <param name="CSID">�ỰID</param>
        /// <returns></returns>
        public DataTable GetDetailByCSID(int CSID)
        {
            int totalCount = 0;
            return Dal.ConverSationDetail.Instance.GetDetailByCSID(CSID, "", 1, 100000, out totalCount);
        }


        /// <summary>
        ///  ��ѯ��ǰ����ϸ
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��</param>
        /// <param name="pageSize">ҳ��</param>
        /// <param name="totalCount">����</param>
        /// <returns></returns>
        public DataTable GetConverSationDetail(QueryConverSationDetail CSID, string order, int currentPage, int pageSize, out int totalCount, string TableName = "")
        {
            return Dal.ConverSationDetail.Instance.GetConverSationDetail(CSID, order, currentPage, pageSize, out totalCount, TableName);
        }

        /// <summary>
        ///  ģ����ѯ�Ự��ʷ��¼
        /// </summary>
        /// <param name="date">ʱ��</param>
        /// <param name="content">����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��</param>
        /// <param name="pageSize">ҳ��</param>
        /// <param name="totalCount">����</param>
        /// <returns></returns>
        public DataTable GetDetailByContent(DateTime date, string content, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.ConverSationDetail.Instance.GetDetailByContent(date, content, order, currentPage, pageSize, out totalCount);
        }

        #endregion

        public object GetSourceTypeValue(string VisitID)
        {
            return Dal.ConverSationDetail.Instance.GetSourceTypeValue(VisitID);
        }

    }
}

