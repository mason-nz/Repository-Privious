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
    public class SubADInfo
    {
        #region Instance
        public static readonly SubADInfo Instance = new SubADInfo();
        #endregion

        #region Contructor
        protected SubADInfo()
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
        public DataTable GetSubADInfo(QuerySubADInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.SubADInfo.Instance.GetSubADInfo(query, order, currentPage, pageSize, out totalCount);
        }              
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.SubADInfo.Instance.GetSubADInfo(new QuerySubADInfo(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SubADInfo GetSubADInfo(string subOrderID)
        {

            return Dal.SubADInfo.Instance.GetSubADInfo(subOrderID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByOrderID(string OrderID)
        {
            QuerySubADInfo query = new QuerySubADInfo();
            query.OrderID = OrderID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetSubADInfo(query, string.Empty, 1, 1, out count);
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
        /// 增加一条数据
        /// </summary>
        public string Insert(Entities.SubADInfo model)
        {           
            return Dal.SubADInfo.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertSubADInfo(Entities.SubADInfo model)
        {
            return Dal.SubADInfo.Instance.Insert(model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.SubADInfo model)
        {
            return Dal.SubADInfo.Instance.Update(model);
        }      

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string OrderID)
        {

            return Dal.SubADInfo.Instance.Delete(OrderID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string OrderID)
        {

            return Dal.SubADInfo.Instance.Delete(sqltran, OrderID);
        }

        #endregion

        #region 更新子工单金额
        /// <summary>
        /// 更新子工单金额
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="suborderid"></param>
        /// <returns></returns>
        public int UpdateTotalAmmount_SubADInfo(decimal amount, string suborderid)
        {
            return Dal.SubADInfo.Instance.UpdateTotalAmmount_SubADInfo(amount, suborderid);
        }
        #endregion

        #region 更改主订单状态
        /// <summary>
        /// 根据订单号更改主订单状态
        /// </summary>
        /// <param name="orderid">主订单号</param>
        /// <param name="status">状态，具体值看订单状态枚举</param>
        /// <returns></returns>
        public int UpdateStatus_SubADInfo(string orderid, int status)
        {
            return Dal.SubADInfo.Instance.UpdateStatus_SubADInfo(orderid, status);
        }
        #endregion

        #region 根据主订单号更新子订单状态
        /// <summary>
        /// 根据主订单号更新子订单状态
        /// </summary>
        /// <param name="orderid">主订单</param>
        /// <param name="status">状态，具体值看订单状态枚举</param>
        /// <returns></returns>
        public int UpdateStatusByOrderID_SubADInfo(string orderid, int status)
        {            
            return Dal.SubADInfo.Instance.UpdateStatusByOrderID_SubADInfo(orderid,status);
        }
        #endregion

        #region 根据主订单删除子订单
        /// <summary>
        /// 根据主订单删除子订单
        /// </summary>
        /// <param name="orderid">主订单号</param>
        /// <returns></returns>
        public int DeleteByOrerID(string orderid)
        {
            return Dal.SubADInfo.Instance.DeleteByOrerID(orderid);
        }
        #endregion        

        #region 将DataRow转成实体
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.SubADInfo DataRowToModel(DataRow row)
        {
            return Dal.SubADInfo.Instance.DataRowToModel(row);
        }
        #endregion

        #region 根据子订单状态修改项目状态(执行完毕、订单完成)
        /// <summary>
        /// 根据子订单状态修改项目状态(执行完毕、订单完成)
        /// </summary>
        /// <param name="orderid">主订单号</param>
        /// <param name="status">执行完毕、订单完成</param>
        public int UpdateStatusADOrder_OrderID(string orderid, int status)
        {
            return Dal.SubADInfo.Instance.UpdateStatusADOrder_OrderID(orderid, status);
        }
        #endregion

        #region 针对订单取消状态，所有子订单都取消后更改项目为取消状态
        /// <summary>
        /// 针对订单取消状态，所有子订单都取消后更改项目为取消状态
        /// </summary>
        /// <param name="orderid">主订单号</param>
        public int UpdateStatusADOrder_OrderID(string orderid)
        {
            return Dal.SubADInfo.Instance.UpdateStatusADOrder_OrderID(orderid);
        }
        #endregion

        #region AE 订单执行、取消、执行完毕、已完成 权限验证
        /// <summary>
        /// AE 订单执行、取消、执行完毕、已完成 权限验证
        /// </summary>
        /// <param name="UserID">AE的UserID</param>
        /// <param name="SubOrderID">订单号</param>
        /// <param name="Status">要设置的状态</param>
        /// <returns></returns>
        public string p_SubADInfoStatus_UpdatePrivilege(int UserID, string SubOrderID, int Status)
        {
            return Dal.SubADInfo.Instance.p_SubADInfoStatus_UpdatePrivilege(UserID, SubOrderID, Status);
        }
        #endregion

        #region 子订单在设置完成状态时 判断是否有上传数据
        public string p_SubADInfoOrderFeedbackData_Select(string suborderid)
        {
            return Dal.SubADInfo.Instance.p_SubADInfoOrderFeedbackData_Select(suborderid);
        }
        #endregion

        public DataTable GetSubADInfoByOrderID(string orderID)
        {
            return Dal.SubADInfo.Instance.GetSubADInfoByOrderID(orderID);
        }

        #region 根据订单号查询
        public void QuerySubADInfoBySubOrderID(string subOrderID, out ResponseADOrderDto resADOrder, out ResponseMediaOrderInfoDto resMOI, 
            out ResponseSubADInfoDto resSubAInfo,out List<ResponseSelfDetailDto> resSelfDetailList, 
            out List<ResponseAPPDetailDto> resAPPDetailList, out List<ResponseOperateInfoDto> resOperateList)
        {
            DataTable dtADOrderInfo = null;
            DataTable dtMOI = null;
            DataTable dtSubADInfo = null;
            DataTable dtADDetail = null;
            DataTable dtOperateInfo = null;
            int mediaType = -2;
            resSelfDetailList = null;
            resAPPDetailList = null;
            Dal.SubADInfo.Instance.QuerySubADInfoBySubOrderID(subOrderID, out mediaType, out dtADOrderInfo, out dtMOI, out dtSubADInfo, out dtADDetail, out dtOperateInfo);
            resADOrder = Util.DataTableToEntity<ResponseADOrderDto>(dtADOrderInfo);
            resMOI = Util.DataTableToEntity<ResponseMediaOrderInfoDto>(dtMOI);
            resSubAInfo = Util.DataTableToEntity<ResponseSubADInfoDto>(dtSubADInfo);
            if (mediaType == 14001)
                resSelfDetailList = Util.DataTableToList<ResponseSelfDetailDto>(dtADDetail);
            else
                resAPPDetailList = Util.DataTableToList<ResponseAPPDetailDto>(dtADDetail);

            resOperateList = Util.DataTableToList<ResponseOperateInfoDto>(dtOperateInfo);
        }
        #endregion
    }
}
