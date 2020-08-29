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
    /// 业务逻辑类OrderNewCar 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 05:58:24 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OrderNewCar
    {
        #region Instance
        public static readonly OrderNewCar Instance = new OrderNewCar();
        #endregion

        #region Contructor
        protected OrderNewCar()
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
        public DataTable GetOrderNewCar(QueryOrderNewCar query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.OrderNewCar.Instance.GetOrderNewCar(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.OrderNewCar.Instance.GetOrderNewCar(new QueryOrderNewCar(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.OrderNewCar GetOrderNewCar(long TaskID)
        {

            return Dal.OrderNewCar.Instance.GetOrderNewCar(TaskID);
        }

        public List<Entities.OrderNewCar> GetOrderNewCarList(Entities.QueryOrderNewCar query, int OrderType = 0)
        {
            return Dal.OrderNewCar.Instance.GetOrderNewCarList(query, OrderType);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByTaskID(long TaskID)
        {
            return Dal.OrderNewCar.Instance.IsExistsByTaskID(TaskID);
            //QueryOrderNewCar query = new QueryOrderNewCar();
            //query.TaskID = TaskID;
            //DataTable dt = new DataTable();
            //int count = 0;
            //dt = GetOrderNewCar(query, string.Empty, 1, 1, out count);
            //if (count > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Insert(Entities.OrderNewCar model)
        {
            Dal.OrderNewCar.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.OrderNewCar model)
        {
            return Dal.OrderNewCar.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.OrderNewCar model)
        {
            return Dal.OrderNewCar.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderNewCar model)
        {
            return Dal.OrderNewCar.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long TaskID)
        {

            return Dal.OrderNewCar.Instance.Delete(TaskID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long TaskID)
        {

            return Dal.OrderNewCar.Instance.Delete(sqltran, TaskID);
        }

        #endregion

        /// <summary>
        /// 根据易湃新车订单实体信息，插入表OrderNewCar信息
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="taskID">任务ID</param>
        /// <param name="model">易湃新车订单实体信息</param>
        /// <returns></returns>
        public bool InsertByTaskID(SqlTransaction tran, long taskID, Entities.OrderNewCarLog model)
        {
            Entities.OrderNewCar onModel = new Entities.OrderNewCar();
            onModel.TaskID = taskID;
            onModel.YPOrderID = model.YPOrderID;
            onModel.UserName = model.UserName;
            onModel.UserPhone = model.UserPhone;
            onModel.UserMobile = model.UserMobile;
            onModel.UserMail = model.UserMail;
            if (model.UserGender != null && model.UserGender.Value == 0)
            {
                onModel.UserGender = 2;//性别“女”  
            }
            else
            {
                onModel.UserGender = model.UserGender;
            }
            onModel.ProvinceID = null;
            onModel.CityID = null;
            onModel.CountyID = null;
            if (model.LocationID != null)
            {
                string locationName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaStrByAreaID(model.LocationID.Value.ToString());
                switch (locationName.Split(',').Length)
                {
                    case 1: onModel.ProvinceID = int.Parse(locationName.Split(',')[0]);
                        break;
                    case 2:
                        onModel.ProvinceID = int.Parse(locationName.Split(',')[0]);
                        onModel.CityID = int.Parse(locationName.Split(',')[1]);
                        break;
                    case 3:
                        onModel.ProvinceID = int.Parse(locationName.Split(',')[0]);
                        onModel.CityID = int.Parse(locationName.Split(',')[1]);
                        onModel.CountyID = int.Parse(locationName.Split(',')[2]);
                        break;
                    default:
                        break;
                }
            }
            onModel.AreaID = null;
            onModel.UserAddress = model.UserAddress;
            onModel.OrderCreateTime = model.OrderCreateTime;
            onModel.OrderRemark = model.OrderRemark;
            onModel.CarMasterID = null;
            onModel.CarSerialID = null;
            onModel.CarTypeID = null;
            if (model.CarID != null)
            {
                onModel.CarTypeID = model.CarID;
                int CarMasterID = 0;
                int CarSerialID = 0;
                int brandID = 0;
                BLL.CarTypeAPI.Instance.GetSerialIDAndMasterBrandIDByCarTypeID(model.CarID.Value, out CarSerialID, out CarMasterID,out brandID);
                //onModel.CarMasterID = CarMasterID;
                //onModel.CarSerialID = CarSerialID;
                if (CarMasterID > 0)
                {
                    onModel.CarMasterID = CarMasterID;
                }
                if (CarSerialID > 0)
                {
                    onModel.CarSerialID = CarSerialID;
                }
            }
            onModel.CarColor = model.CarColor;
            onModel.DMSMemberCode = string.Empty;
            onModel.DMSMemberName = string.Empty;
            onModel.CallRecord = string.Empty;
            onModel.Status = 0;
            onModel.CreateTime = DateTime.Now;
            onModel.DealerID = model.DealerID;
            onModel.OrderType = model.OrderType;

            int recid = Insert(tran, onModel);
            if (recid > 0)
            {
                BLL.Loger.Log4Net.Info(string.Format("新车任务生成表OrderNewCar成功，主键ID为：{0},无主订单ID为：{1},车款ID：{2},子品牌ID：{3}，主品牌ID为：{4}",
                    recid, onModel.YPOrderID,
                    onModel.CarTypeID == null ? 0 : onModel.CarTypeID.Value,
                    onModel.CarSerialID == null ? 0 : onModel.CarSerialID.Value,
                    onModel.CarMasterID == null ? 0 : onModel.CarMasterID.Value
                    ));
                return true;
            }
            return false;
        }


        public DataTable GetModifyCar()
        {
            return Dal.OrderNewCar.GetModifyCar();
        }
    }
}

