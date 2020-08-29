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
    /// ҵ���߼���CarSerial ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-12-11 03:57:11 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CarSerial
    {
        #region Instance
        public static readonly CarSerial Instance = new CarSerial();
        #endregion

        #region Contructor
        protected CarSerial()
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
        public DataTable GetCarSerial(string query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CarSerial.Instance.GetCarSerial(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            DataTable dt = Dal.CarSerial.Instance.GetAllCarSerial();
            return dt;
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.CarSerial GetCarSerial(int CSID)
        {
            return Dal.CarSerial.Instance.GetCarSerial(CSID);
        }
        public List<Entities.CarSerial> GetCarSerial(string scids)
        {
            return Dal.CarSerial.Instance.GetCarSerial(scids);
        }
        #endregion

        //#region IsExists
        ///// <summary>
        ///// �Ƿ���ڸü�¼
        ///// </summary>
        //public bool IsExistsByCSID(int CSID)
        //{
        //    QueryCarSerial query = new QueryCarSerial();
        //    query.CSID = CSID;
        //    DataTable dt = new DataTable();
        //    int count = 0;
        //    dt = GetCarSerial(query, string.Empty, 1, 1, out count);
        //    if (count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //#endregion

        #region Insert
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(Entities.CarSerial model)
        {
            Dal.CarSerial.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.CarSerial model)
        {
            Dal.CarSerial.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.CarSerial model)
        {
            return Dal.CarSerial.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CarSerial model)
        {
            return Dal.CarSerial.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int CSID)
        {

            return Dal.CarSerial.Instance.Delete(CSID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int CSID)
        {

            return Dal.CarSerial.Instance.Delete(sqltran, CSID);
        }

        #endregion


        internal void DeleteTable()
        {
            Dal.CarSerial.Instance.DeleteTable();
        }

        internal DataTable GetAllListFromCrm2009()
        {
            return Dal.CarSerial.Instance.GetAllListFromCrm2009();
        }
    }
}

