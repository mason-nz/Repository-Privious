using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 业务逻辑类FreProblem 的摘要说明。
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

        /// 获得数据列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllFreProblem(int top)
        {
            return Dal.FreProblem.Instance.GetAllFreProblem(top);
        }
        /// 获取最大的排序号
        /// <summary>
        /// 获取最大的排序号
        /// </summary>
        /// <returns></returns>
        public int GetMaxSortNum()
        {
            return Dal.FreProblem.Instance.GetMaxSortNum();
        }
        /// 上下移动数据
        /// <summary>
        /// 上下移动数据
        /// </summary>
        /// <param name="recid"></param>
        /// <param name="sortnum"></param>
        /// <param name="type">1上-1下</param>
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
        /// 获取总数
        /// <summary>
        /// 获取总数
        /// </summary>
        /// <returns></returns>
        public int GetAllCount()
        {
            return Dal.FreProblem.Instance.GetAllCount();
        }
    }
}

