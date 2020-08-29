using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using XYAuto.Utils;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;
using XYAuto.ITSC.Chitunion2017.BLL.ADOrderInfoDto.Dto;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class CartScheduleInfo
    {
        #region Instance
        public static readonly CartScheduleInfo Instance = new CartScheduleInfo();
        #endregion

        #region Contructor
        protected CartScheduleInfo()
        { }
        #endregion

        #region Select        
        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <param name="order">排序</param>
        /// <param name="currentPage">页号,-1不分页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>集合</returns>
        public DataTable GetCartScheduleInfo(QueryCartScheduleInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.CartScheduleInfo.Instance.GetCartScheduleInfo(query, order, currentPage, pageSize, out totalCount);
        }              
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.CartScheduleInfo.Instance.GetCartScheduleInfo(new QueryCartScheduleInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 根据主键Recid得到一个对象实体
        /// </summary>
        public Entities.CartScheduleInfo GetCartScheduleInfo(int recid)
        {

            return Dal.CartScheduleInfo.Instance.GetCartScheduleInfo(recid);
        }

        #endregion      

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int  Insert(Entities.CartScheduleInfo model)
        {           
            return Dal.CartScheduleInfo.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertCartScheduleInfo(Entities.CartScheduleInfo model)
        {
            return Dal.CartScheduleInfo.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(Entities.CartScheduleInfo model)
        {
            Dal.CartScheduleInfo.Instance.Update(model);
        }      

        #endregion
        
        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int recid)
        {
            return Dal.CartScheduleInfo.Instance.Delete(recid);
        }
        #endregion                

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CartScheduleInfo DataRowToModel(DataRow row)
        {
            return Dal.CartScheduleInfo.Instance.DataRowToModel(row);
        }

        #region 根据CartID查询
        public List<RequestADScheduleInfoDto> QueryByCartID(int cartID)
        {
            return Util.DataTableToList<RequestADScheduleInfoDto>(Dal.CartScheduleInfo.Instance.QueryByCartID(cartID));
        }
        #endregion      
    }
}
