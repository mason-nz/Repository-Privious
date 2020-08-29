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
    /// ҵ���߼���FreProblem ��ժҪ˵����
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
    public class FreProblem : CommonBll
    {
        public static readonly new FreProblem Instance = new FreProblem();

        protected FreProblem()
        { }

        /// ��������б�
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllFreProblem(int top, int SourceType)
        {
            return Dal.FreProblem.Instance.GetAllFreProblem(top, SourceType);
        }
        /// ��������б�
        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllFreProblem(int top)
        {
            return Dal.FreProblem.Instance.GetAllFreProblem(top);
        }
        /// ��ȡ���������
        /// <summary>
        /// ��ȡ���������
        /// </summary>
        /// <returns></returns>
        public int GetMaxSortNum()
        {
            return Dal.FreProblem.Instance.GetMaxSortNum();
        }
        /// �����ƶ�����
        /// <summary>
        /// �����ƶ�����
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="sortnum"></param>
        /// <param name="type">1��-1��</param>
        /// <returns></returns>
        public bool MoveUpOrDown(int recid, int type)
        {
            Entities.FreProblem info = GetComAdoInfo<Entities.FreProblem>(recid);
            if (info != null)
            {
                return Dal.FreProblem.Instance.MoveUpOrDown(recid, info.ValueOrDefault_SortNum, type);
            }
            else
            {
                return false;
            }
        }
        /// ��ȡ����
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        public int GetAllCount()
        {
            return Dal.FreProblem.Instance.GetAllCount();
        }
        public int AddFeeProblem()
        {
            return Dal.FreProblem.Instance.AddFeeProblem();
        }


        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetFreProblem(QueryFreProblem query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.FreProblem.Instance.GetFreProblem(query, order, currentPage, pageSize, out totalCount);

        }
    }
}

