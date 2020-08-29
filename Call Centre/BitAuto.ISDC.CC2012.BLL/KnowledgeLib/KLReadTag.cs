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
    /// ҵ���߼���KLReadTag ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-08-21 10:19:08 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class KLReadTag
    {
        #region Instance
        public static readonly KLReadTag Instance = new KLReadTag();
        #endregion

        #region Contructor
        protected KLReadTag()
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
        public DataTable GetKLReadTag(QueryKLReadTag query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.KLReadTag.Instance.GetKLReadTag(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.KLReadTag.Instance.GetKLReadTag(new QueryKLReadTag(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.KLReadTag GetKLReadTag(long RecID)
        {

            return Dal.KLReadTag.Instance.GetKLReadTag(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByRecID(long RecID)
        {
            QueryKLReadTag query = new QueryKLReadTag();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetKLReadTag(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.KLReadTag model)
        {
            return Dal.KLReadTag.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.KLReadTag model)
        {
            return Dal.KLReadTag.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.KLReadTag model)
        {
            return Dal.KLReadTag.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.KLReadTag model)
        {
            return Dal.KLReadTag.Instance.Update(sqltran, model);
        }

        /// <summary>
        /// ����֪ʶ��ID�������Ķ����Ϊ"δ��"
        /// </summary>
        /// <param name="sqltran"></param>
        /// <param name="KLID"></param>
        /// <param name="Tag">1 �Ѷ�  0δ��</param>
        /// <returns></returns>
        public int UpdateTagByKLID(SqlTransaction sqltran, long KLID, int Tag)
        {
            return Dal.KLReadTag.Instance.UpdateTagByKLID(sqltran, KLID, Tag);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.KLReadTag.Instance.Delete(RecID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.KLReadTag.Instance.Delete(sqltran, RecID);
        }

        public int DeleteByUserID(SqlTransaction sqltran, int UserID, long klid)
        {
            return Dal.KLReadTag.Instance.DeleteByUserID(sqltran, UserID, klid);
        }
        #endregion

        /// <summary>
        /// ���Ϊ�Ѷ�
        /// </summary>
        /// <param name="kid">֪ʶ��ID</param>
        /// <param name="userID">�û�ID</param>
        /// <param name="tag">1 �Ѷ�  0δ��</param>
        public int SetReadTag(int kid, int userID, int tag)
        {
            Entities.QueryKLReadTag query = new QueryKLReadTag();

            query.KLID = kid;
            query.UserID = userID;
            int totalCount = 0;
            DataTable dt = BLL.KLReadTag.Instance.GetKLReadTag(query, "", 1, 999, out totalCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                //�У��͸��ı�־
                return Dal.KLReadTag.Instance.ModifyReadTag(kid, userID, tag);
            }
            else
            {
                //û�У��Ͳ���
                Entities.KLReadTag newModel = new Entities.KLReadTag();
                newModel.KLID = kid;
                newModel.UserID = userID;
                newModel.ReadTag = tag;
                newModel.CreateTime = DateTime.Now;
                newModel.CreateUserID = userID;

                return Dal.KLReadTag.Instance.Insert(newModel);
            }

        }
    }
}

