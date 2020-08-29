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
    /// ҵ���߼���TPage ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-02-20 10:39:35 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class TPage
    {
        #region Instance
        public static readonly TPage Instance = new TPage();
        #endregion

        #region Contructor
        protected TPage()
        { }
        #endregion

        #region Select
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="order">����</param>
        /// <param name="currentPage">ҳ��,-1����ҳ</param>
        /// <param name="pageSize">ÿҳ��¼��</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetTPage(QueryTPage query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.TPage.Instance.GetTPage(query, order, currentPage, pageSize, out totalCount);
        }

        //��ȡ״̬   add lxw 13.3.22
        //δ���-���浫δ����ģ��-status=0�������-�ѱ���������ģ�嵫�½����±�û����-status=1����ʹ�ã�������ģ�岢�½����±��������-status=1
        //��Ϊ�����ʱ����Ҫ����TTable���ѯ��TTcode�µ�TTIsData�Ƿ�Ϊ1�����Ϊ1��Ϊ��ʹ�ã�status��ֵΪ2
        public int getStatus(string recID, string status)
        {
            int _status;
            int _recID;
            if (int.TryParse(status, out _status) && int.TryParse(recID, out _recID))
            {
                if (_status == 1)
                {
                    //�жϸ����ɵ�ģ����Ƿ��������
                    int count = BLL.TPage.Instance.isHasDataByTTName(_recID);
                    if (count > 0)//������0����ʾģ��������ݣ�״̬�޸�Ϊ��ʹ��
                    {
                        _status = 2;
                    }
                }

            }
            return _status;
        }

        //��ȡ״̬   add lxw 13.3.22
        //δ���-���浫δ����ģ��-status=0�������-�ѱ���������ģ�嵫�½����±�û����-status=1����ʹ�ã�������ģ�岢�½����±��������-status=1
        //��Ϊ�����ʱ����Ҫ����TTable���ѯ��TTcode�µ�TTIsData�Ƿ�Ϊ1�����Ϊ1��Ϊ��ʹ�ã�status��ֵΪ2
        public int getStatusByTTCode(string status, string ttCode)
        {
            int _status;
            if (int.TryParse(status, out _status) && ttCode != string.Empty)
            {
                if (_status == 1)
                {
                    //�жϸ����ɵ�ģ����Ƿ��������
                    Entities.TTable model = BLL.TTable.Instance.GetTTableByTTCode(ttCode);
                    if (model != null && model.TTIsData == 1)//����1����ʾģ��������ݣ�״̬�޸�Ϊ��ʹ��
                    {
                        _status = 2;
                    }
                }

            }
            return _status;
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.TPage.Instance.GetTPage(new QueryTPage(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.TPage GetTPage(int RecID)
        {

            return Dal.TPage.Instance.GetTPage(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(int RecID)
        {
            QueryTPage query = new QueryTPage();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetTPage(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Insert
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(Entities.TPage model)
        {
            return Dal.TPage.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.TPage model)
        {
            return Dal.TPage.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.TPage model)
        {
            return Dal.TPage.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.TPage model)
        {
            return Dal.TPage.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int RecID)
        {

            return Dal.TPage.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int RecID)
        {

            return Dal.TPage.Instance.Delete(sqltran, RecID);
        }

        #endregion

        public DataTable GetAllCreateUserID()
        {
            return Dal.TPage.Instance.GetAllCreateUserID();
        }

        /// <summary>
        /// �ж�recID������ģ����Ƿ��������
        /// </summary>
        /// <param name="recID">TPage����</param>
        /// <returns></returns>
        public int isHasDataByTTName(int recID)
        {
            return Dal.TPage.Instance.isHasDataByTTName(recID);
        }

        /// <summary>
        /// ����TTCode��ȡʵ��
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Entities.TPage GetTPageByTTCode(string ttcode)
        {
            Entities.TPage tpageModel = new Entities.TPage();
            Entities.QueryTPage query = new QueryTPage();
            query.TTCode = ttcode;
            int totalCount = 0;
            DataTable dt = BLL.TPage.Instance.GetTPage(query, "", 1, 999, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                tpageModel = Dal.TPage.Instance.GetTPage(int.Parse(dt.Rows[0]["RecID"].ToString()));
            }
            return tpageModel;
        }
    }
}

