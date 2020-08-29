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
    /// 业务逻辑类GroupOrder 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-11-04 09:34:14 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class GroupOrder
    {
        #region Instance
        public static readonly GroupOrder Instance = new GroupOrder();
        #endregion

        #region Contructor
        protected GroupOrder()
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
        public DataTable GetGroupOrder(QueryGroupOrder query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.GroupOrder.Instance.GetGroupOrder(query, order, currentPage, pageSize, out totalCount);
        }

        public DataTable GetGroupOrder(string wheresql)
        {

            return Dal.GroupOrder.Instance.GetGroupOrder(wheresql);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.GroupOrder.Instance.GetGroupOrder(new QueryGroupOrder(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.GroupOrder GetGroupOrder(long TaskID)
        {

            return Dal.GroupOrder.Instance.GetGroupOrder(TaskID);
        }

        public List<Entities.GroupOrder> GetGroupOrderList(Entities.QueryGroupOrder query)
        {
            return Dal.GroupOrder.Instance.GetGroupOrderList(query);
        }
        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByTaskID(long TaskID)
        {
            QueryGroupOrder query = new QueryGroupOrder();
            query.TaskID = TaskID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetGroupOrder(query, string.Empty, 1, 1, out count);
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
        public void Insert(Entities.GroupOrder model)
        {
            Dal.GroupOrder.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.GroupOrder model)
        {
            return Dal.GroupOrder.Instance.Insert(sqltran, model);
        }

        /// <summary>
        /// 根据易湃团购订单实体信息，插入表GroupOrder信息
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="taskID">任务ID</param>
        /// <param name="model">易湃团购订单实体信息</param>
        /// <returns></returns>
        public bool InsertByTaskID(SqlTransaction tran, long taskID, Entities.GroupOrderOrg model)
        {
            Entities.GroupOrder onModel = new Entities.GroupOrder();
            onModel.TaskID = taskID;
            onModel.OrderID = model.OrderID;
            onModel.OrderCode = model.OrderCode;
            onModel.CustomerName = model.CustomerName.Trim();
            onModel.CustomerTel = model.CustomerTel.Trim();
            onModel.ProvinceID = model.ProvinceID;
            onModel.ProvinceName = model.ProvinceName;
            onModel.CityID = model.CityID;
            onModel.CityName = model.CityName;

            //修改大区 强斐 2014-12-17
            BitAuto.YanFa.Crm2009.Entities.AreaInfo info = Util.GetAreaInfoByPCC(
                   CommonFunction.ObjectToString(model.ProvinceID),
                   CommonFunction.ObjectToString(model.CityID),
                   null);
            onModel.AreaID = info == null ? "" : info.District;
            
            onModel.OrderCreateTime = model.OrderCreateTime;
            onModel.CarMasterID = model.CarMasterID;
            onModel.CarMasterName = model.CarMasterName;
            onModel.CarSerialID = model.CarSerialID;
            onModel.CarSerialName = model.CarSerialName;
            onModel.CarID = model.CarID;
            onModel.CarName = model.CarName;
            onModel.DealerID = model.DealerID;
            onModel.DealerName = model.DealerName;
            onModel.OrderPrice = model.Price;
            onModel.CreateTime = DateTime.Now;
            onModel.CreateUserID = null;
            onModel.LastUpdateTime = null;
            onModel.LastUpdateUserID = null;
            onModel.UserName = null;


            long recid = Insert(tran, onModel);
            if (recid > 0)
            {
                BLL.Loger.Log4Net.Info(string.Format("团购任务生成表GroupOrder成功，主键ID为：{0},无主订单ID为：{1},车款ID：{2},子品牌ID：{3}，主品牌ID为：{4}",
                    recid, onModel.OrderID,
                    onModel.CarID == null ? 0 : onModel.CarID.Value,
                    onModel.CarSerialID == null ? 0 : onModel.CarSerialID.Value,
                    onModel.CarMasterID == null ? 0 : onModel.CarMasterID.Value
                    ));
                return true;
            }
            return false;
        }
        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.GroupOrder model)
        {
            return Dal.GroupOrder.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.GroupOrder model)
        {
            return Dal.GroupOrder.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long TaskID)
        {

            return Dal.GroupOrder.Instance.Delete(TaskID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long TaskID)
        {

            return Dal.GroupOrder.Instance.Delete(sqltran, TaskID);
        }

        #endregion

        #region 获取订单查看页客户基本信息
        /// <summary>
        /// 获取订单查看页客户基本信息
        /// </summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        public Entities.GroupOrder GetGroupOrderCustInfo(long TaskID)
        {
            return Dal.GroupOrder.Instance.GetGroupOrderCustInfo(TaskID);
        }
        #endregion
    }
}

