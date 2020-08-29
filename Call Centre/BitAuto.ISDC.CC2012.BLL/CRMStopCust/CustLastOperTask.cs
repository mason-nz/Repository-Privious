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
    /// 业务逻辑类CustLastOperTask 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-07-23 11:01:34 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CustLastOperTask
    {
        public static readonly CustLastOperTask Instance = new CustLastOperTask();

        protected CustLastOperTask()
        { }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CustLastOperTask GetCustLastOperTask(string CustID)
        {
            return Dal.CustLastOperTask.Instance.GetCustLastOperTask(CustID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(Entities.CustLastOperTask model)
        {
            Dal.CustLastOperTask.Instance.Insert(model);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(SqlTransaction sqltran, Entities.CustLastOperTask model)
        {
            Dal.CustLastOperTask.Instance.Insert(sqltran, model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.CustLastOperTask model)
        {
            return Dal.CustLastOperTask.Instance.Update(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.CustLastOperTask model)
        {
            return Dal.CustLastOperTask.Instance.Update(sqltran, model);
        }
    }
}

