using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class CRMCSTMember
    {
        #region Instance
        public static readonly CRMCSTMember Instance = new CRMCSTMember();
        #endregion

        //add by lihf 根据会员号查询客户ID2013-7-22
        /// <summary>
        /// 根据会员号查询客户ID
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetCustIDByMemberCode(string name, string where)
        {
            DataTable dt = null;
            dt = Dal.CRMCSTMember.Instance.GetCustIDByMemberCode(where);
            if (dt != null && dt.Rows.Count > 0)
            {
                Loger.Log4Net.Info(name + " 在 " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm").Replace("-", "/") + " 进行了导出客户ID操作，导出" + dt.Rows.Count + "条记录");
            }
            return dt;
        }

        /// <summary>
        /// 获取CRM系统，车商通会员信息
        /// </summary>
        /// <param name="query">查询条件类</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>返回ds</returns>
        public DataTable GetCC_CRMCSTMemberInfo(QueryCRMCSTMember query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CRMCSTMember.Instance.GetCC_CRMCSTMemberInfo(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获取CRM系统，会员信息（包含客户联系人信息）
        /// </summary>
        /// <param name="query">查询条件类</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>返回ds</returns>
        public DataTable GetCRMDMSMemberInfo(QueryCRMCSTMember query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CRMCSTMember.Instance.GetCRMCSTMemberInfo(query, order, currentPage, pageSize, out totalCount);
        }

        //add by qizhiqiang 批量查询会员签约结果2012-4-11
        /// <summary>
        /// 会员签约结果
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        //public DataTable GetMemberSignResult(string name, string where)
        //{
        //    DataTable dt = null;
        //    dt = Dal.CRMDMSMember.Instance.GetMemberSignResult(where);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        Loger.Log4Net.Info(name + " 在 " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm").Replace("-", "/") + " 进行了导出会员操作，导出" + dt.Rows.Count + "条记录");
        //    }
        //    return dt;
        //}
        //add by qizhiqiang 2012-4-20
        /// <summary>
        /// 根据客户id取车商通信息
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public DataTable SelectByCustID(string CustID)
        {
            return Dal.CRMCSTMember.Instance.SelectByCustID(CustID);
        }
    }
}
