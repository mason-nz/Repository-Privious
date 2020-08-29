using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Threading;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���AutoCall_TaskInfo ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2015-09-14 09:57:59 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class AutoCall_TaskInfo
    {
        #region Instance
        public static readonly AutoCall_TaskInfo Instance = new AutoCall_TaskInfo();
        #endregion

        #region Contructor
        protected AutoCall_TaskInfo()
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
        public DataTable GetAutoCall_TaskInfo(QueryAutoCall_TaskInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.AutoCall_TaskInfo.Instance.GetAutoCall_TaskInfo(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.AutoCall_TaskInfo.Instance.GetAutoCall_TaskInfo(new QueryAutoCall_TaskInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.AutoCall_TaskInfo GetAutoCall_TaskInfo(int ACTID)
        {

            return Dal.AutoCall_TaskInfo.Instance.GetAutoCall_TaskInfo(ACTID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByACTID(int ACTID)
        {
            QueryAutoCall_TaskInfo query = new QueryAutoCall_TaskInfo();
            query.ACTID = ACTID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetAutoCall_TaskInfo(query, string.Empty, 1, 1, out count);
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
        public int Insert(Entities.AutoCall_TaskInfo model)
        {
            return Dal.AutoCall_TaskInfo.Instance.Insert(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.AutoCall_TaskInfo model)
        {
            return Dal.AutoCall_TaskInfo.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(int ACTID)
        {

            return Dal.AutoCall_TaskInfo.Instance.Delete(ACTID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public int Delete(SqlTransaction sqltran, int ACTID)
        {

            return Dal.AutoCall_TaskInfo.Instance.Delete(sqltran, ACTID);
        }

        #endregion


        /// ������Ŀ���������Զ�����������
        /// <summary>
        /// ������Ŀ���������Զ�����������
        /// </summary>
        public void AutoCallTaskInfoUpdate(long projectid)
        {
            int userid = CommonFunction.ObjectToInteger(BLL.Util.GetLoginUserIDNotCheck(), -1);
            Dal.AutoCall_TaskInfo.Instance.AutoCallTaskInfoUpdate(projectid, userid);
        }
        /// ������Ŀ���������Զ�����������
        /// <summary>
        /// ������Ŀ���������Զ�����������
        /// </summary>
        public void AutoCallTaskInfoUpdate_Async(long projectid)
        {
            ThreadPool.QueueUserWorkItem(obj =>
            {
                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    BLL.Loger.Log4Net.Info("�첽-������Ŀ���������Զ�����������" + obj.ToString());
                    int userid = CommonFunction.ObjectToInteger(BLL.Util.GetLoginUserIDNotCheck(), -1);
                    Dal.AutoCall_TaskInfo.Instance.AutoCallTaskInfoUpdate(CommonFunction.ObjectToLong(obj), userid);
                    sw.Stop();
                    BLL.Loger.Log4Net.Info("�첽-������Ŀ���������Զ����������� ��ʱ��ms����" + sw.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("�첽-������Ŀ���������Զ�����������" + obj.ToString(), ex);
                }
            }, projectid);
        }
        /// �����ֻ������ѯ��ǰʱ������ͨ��������ID
        /// <summary>
        /// �����ֻ������ѯ��ǰʱ������ͨ��������ID
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public string GetCurrentTaskIDByPhone(string phone)
        {
            return Dal.AutoCall_TaskInfo.Instance.GetCurrentTaskIDByPhone(phone);
        }
    }
}

