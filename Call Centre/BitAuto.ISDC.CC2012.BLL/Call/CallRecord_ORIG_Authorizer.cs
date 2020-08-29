using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���CallRecord_ORIG_Authorizer ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-17 10:17:59 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CallRecord_ORIG_Authorizer
    {
        #region Instance
        public static readonly CallRecord_ORIG_Authorizer Instance = new CallRecord_ORIG_Authorizer();
        #endregion

        #region Contructor
        protected CallRecord_ORIG_Authorizer()
        { }
        #endregion

        #region Select
        /// <summary>
        /// ���ղ�ѯ������ѯ
        /// </summary>
        /// <param name="query">��ѯ����</param>
        /// <param name="totalCount">������</param>
        /// <returns>����</returns>
        public DataTable GetCallRecord_ORIG_Authorizer(string query, out int totalCount)
        {
            return Dal.CallRecord_ORIG_Authorizer.Instance.GetCallRecord_ORIG_Authorizer(query, out totalCount);
        }

        ///// <summary>
        ///// ��������б�
        ///// </summary>
        //public DataTable GetAllList()
        //{
        //    int totalCount=0;
        //    return Dal.CallRecord_ORIG_Authorizer.Instance.GetCallRecord_ORIG_Authorizer(new QueryCallRecord_ORIG_Authorizer(),string.Empty,1,1000000,out totalCount);
        //}

        #endregion

        #region GetModel
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Entities.CallRecord_ORIG_Authorizer GetCallRecord_ORIG_Authorizer(int RecID)
        {

            return Dal.CallRecord_ORIG_Authorizer.Instance.GetCallRecord_ORIG_Authorizer(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool IsExistsByIPAndCode(string IP, string code, int status)
        {
            string sql = string.Format(" And IP='{0}' And AuthorizeCode='{1}' And Status={2}", StringHelper.SqlFilter(IP), StringHelper.SqlFilter(code), status);
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCallRecord_ORIG_Authorizer(sql, out count);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ��֤��Ȩ�߼�
        /// </summary>
        /// <param name="Verifycode">��Ȩ��</param>
        /// <param name="status">״̬��Ĭ��-0,1-�����ӿڣ�</param>
        /// <param name="msg">������Ϣ</param>
        /// <param name="errorMsg">������Ϣ</param>
        /// <returns>��֤ͨ��ΪTrue�����򷵻�False</returns>
        public bool Verify(string Verifycode, int status, ref string msg, string errorMsg)
        {
            //�Ƿ����û����¼����
            string sVerify = "";
            sVerify = ConfigurationUtil.GetAppSettingValue("CallRecordAuthorizeVerify");
            if (sVerify == "false")
                return true;

            string userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            if (BLL.CallRecord_ORIG_Authorizer.Instance.IsExistsByIPAndCode(userHostAddress, Verifycode, status))
            {
                return true;
            }
            else
            {
                msg = errorMsg + "userHostAddress=" + userHostAddress + ",Verifycode=" + Verifycode;
                return false;
            }
        }

        #endregion

        //#region Insert
        ///// <summary>
        ///// ����һ������
        ///// </summary>
        //public int  Insert(Entities.CallRecord_ORIG_Authorizer model)
        //{
        //    return Dal.CallRecord_ORIG_Authorizer.Instance.Insert(model);
        //}

        ///// <summary>
        ///// ����һ������
        ///// </summary>
        //public int  Insert(SqlTransaction sqltran, Entities.CallRecord_ORIG_Authorizer model)
        //{
        //    return Dal.CallRecord_ORIG_Authorizer.Instance.Insert(sqltran, model);
        //}

        //#endregion

        #region Update
        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(Entities.CallRecord_ORIG_Authorizer model)
        {
            return Dal.CallRecord_ORIG_Authorizer.Instance.Update(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CallRecord_ORIG_Authorizer model)
        {
            return Dal.CallRecord_ORIG_Authorizer.Instance.Update(sqltran, model);
        }

        #endregion

        //#region Delete
        ///// <summary>
        ///// ɾ��һ������
        ///// </summary>
        //public int Delete(int RecID)
        //{

        //    return Dal.CallRecord_ORIG_Authorizer.Instance.Delete(RecID);
        //}

        ///// <summary>
        ///// ɾ��һ������
        ///// </summary>
        //public int Delete(SqlTransaction sqltran, int RecID)
        //{

        //    return Dal.CallRecord_ORIG_Authorizer.Instance.Delete(sqltran, RecID);
        //}

        //#endregion

    }
}

