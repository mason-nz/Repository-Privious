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
    /// 业务逻辑类CallRecord_ORIG_Authorizer 的摘要说明。
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
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetCallRecord_ORIG_Authorizer(string query, out int totalCount)
        {
            return Dal.CallRecord_ORIG_Authorizer.Instance.GetCallRecord_ORIG_Authorizer(query, out totalCount);
        }

        ///// <summary>
        ///// 获得数据列表
        ///// </summary>
        //public DataTable GetAllList()
        //{
        //    int totalCount=0;
        //    return Dal.CallRecord_ORIG_Authorizer.Instance.GetCallRecord_ORIG_Authorizer(new QueryCallRecord_ORIG_Authorizer(),string.Empty,1,1000000,out totalCount);
        //}

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CallRecord_ORIG_Authorizer GetCallRecord_ORIG_Authorizer(int RecID)
        {

            return Dal.CallRecord_ORIG_Authorizer.Instance.GetCallRecord_ORIG_Authorizer(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
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
        /// 验证授权逻辑
        /// </summary>
        /// <param name="Verifycode">授权码</param>
        /// <param name="status">状态（默认-0,1-工单接口）</param>
        /// <param name="msg">返回信息</param>
        /// <param name="errorMsg">传入信息</param>
        /// <returns>验证通过为True，否则返回False</returns>
        public bool Verify(string Verifycode, int status, ref string msg, string errorMsg)
        {
            //是否启用话务记录保存
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
        ///// 增加一条数据
        ///// </summary>
        //public int  Insert(Entities.CallRecord_ORIG_Authorizer model)
        //{
        //    return Dal.CallRecord_ORIG_Authorizer.Instance.Insert(model);
        //}

        ///// <summary>
        ///// 增加一条数据
        ///// </summary>
        //public int  Insert(SqlTransaction sqltran, Entities.CallRecord_ORIG_Authorizer model)
        //{
        //    return Dal.CallRecord_ORIG_Authorizer.Instance.Insert(sqltran, model);
        //}

        //#endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.CallRecord_ORIG_Authorizer model)
        {
            return Dal.CallRecord_ORIG_Authorizer.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CallRecord_ORIG_Authorizer model)
        {
            return Dal.CallRecord_ORIG_Authorizer.Instance.Update(sqltran, model);
        }

        #endregion

        //#region Delete
        ///// <summary>
        ///// 删除一条数据
        ///// </summary>
        //public int Delete(int RecID)
        //{

        //    return Dal.CallRecord_ORIG_Authorizer.Instance.Delete(RecID);
        //}

        ///// <summary>
        ///// 删除一条数据
        ///// </summary>
        //public int Delete(SqlTransaction sqltran, int RecID)
        //{

        //    return Dal.CallRecord_ORIG_Authorizer.Instance.Delete(sqltran, RecID);
        //}

        //#endregion

    }
}

