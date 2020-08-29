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
    /// 业务逻辑类OrderRelpaceCar 的摘要说明。
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
    public class OrderRelpaceCar
    {
        #region Instance
        public static readonly OrderRelpaceCar Instance = new OrderRelpaceCar();
        #endregion

        #region Contructor
        protected OrderRelpaceCar()
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
        public DataTable GetOrderRelpaceCar(QueryOrderRelpaceCar query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.OrderRelpaceCar.Instance.GetOrderRelpaceCar(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.OrderRelpaceCar.Instance.GetOrderRelpaceCar(new QueryOrderRelpaceCar(), string.Empty, 1, 1000000, out totalCount);
        }

        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.OrderRelpaceCar GetOrderRelpaceCar(long TaskID)
        {

            return Dal.OrderRelpaceCar.Instance.GetOrderRelpaceCar(TaskID);
        }

        public List<Entities.OrderRelpaceCar> GetOrderRelpaceCarList(Entities.QueryOrderRelpaceCar query)
        {
            return Dal.OrderRelpaceCar.Instance.GetOrderRelpaceCarList(query);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByTaskID(long TaskID)
        {
            return Dal.OrderRelpaceCar.Instance.IsExistsByTaskID(TaskID);
            //QueryOrderRelpaceCar query = new QueryOrderRelpaceCar();
            //query.TaskID = TaskID;
            //DataTable dt = new DataTable();
            //int count = 0;
            //dt = GetOrderRelpaceCar(query, string.Empty, 1, 1, out count);
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
        public void Insert(Entities.OrderRelpaceCar model)
        {
            Dal.OrderRelpaceCar.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(SqlTransaction sqltran, Entities.OrderRelpaceCar model)
        {
            return Dal.OrderRelpaceCar.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.OrderRelpaceCar model)
        {
            return Dal.OrderRelpaceCar.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderRelpaceCar model)
        {
            return Dal.OrderRelpaceCar.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long TaskID)
        {

            return Dal.OrderRelpaceCar.Instance.Delete(TaskID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long TaskID)
        {

            return Dal.OrderRelpaceCar.Instance.Delete(sqltran, TaskID);
        }

        #endregion

        /// <summary>
        /// 根据易湃置换订单实体信息，插入表OrderRelpaceCar信息
        /// </summary>
        /// <param name="tran"></param>
        /// <param name="taskID">任务ID</param>
        /// <param name="model">易湃置换订单实体信息</param>
        /// <returns></returns>
        public bool InsertByTaskID(SqlTransaction tran, long taskID, Entities.OrderRelpaceCarLog model)
        {
            Entities.OrderRelpaceCar orModel = new Entities.OrderRelpaceCar();
            orModel.TaskID = taskID;
            orModel.YPOrderID = model.YPOrderID;
            orModel.UserName = model.UserName;
            orModel.UserPhone = model.UserPhone;
            orModel.UserMobile = model.UserMobile;
            orModel.UserMail = model.UserMail;
            if (model.UserGender != null && model.UserGender.Value == 0)
            {
                orModel.UserGender = 2;//性别“女”  
            }
            else
            {
                orModel.UserGender = model.UserGender;
            }
            orModel.ProvinceID = null;
            orModel.CityID = null;
            orModel.CountyID = null;
            if (model.LocationID != null)
            {
                string locationName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaStrByAreaID(model.LocationID.Value.ToString());
                switch (locationName.Split(',').Length)
                {
                    case 1: orModel.ProvinceID = int.Parse(locationName.Split(',')[0]);
                        break;
                    case 2:
                        orModel.ProvinceID = int.Parse(locationName.Split(',')[0]);
                        orModel.CityID = int.Parse(locationName.Split(',')[1]);
                        break;
                    case 3:
                        orModel.ProvinceID = int.Parse(locationName.Split(',')[0]);
                        orModel.CityID = int.Parse(locationName.Split(',')[1]);
                        orModel.CountyID = int.Parse(locationName.Split(',')[2]);
                        break;
                    default:
                        break;
                }
            }
            orModel.AreaID = null;
            orModel.UserAddress = model.UserAddress;
            orModel.OrderCreateTime = model.OrderCreateTime;
            orModel.OrderRemark = model.OrderRemark;
            orModel.CarMasterID = null;
            orModel.CarSerialID = null;
            orModel.CarTypeID = null;
            if (model.CarID != null)
            {
                orModel.CarTypeID = model.CarID;
                int CarMasterID = 0;
                int CarSerialID = 0;
                int brandID = 0;
                BLL.CarTypeAPI.Instance.GetSerialIDAndMasterBrandIDByCarTypeID(model.CarID.Value, out CarSerialID, out CarMasterID,out brandID);
                //orModel.CarMasterID = CarMasterID;
                //orModel.CarSerialID = CarSerialID;
                if (CarMasterID > 0)
                {
                    orModel.CarMasterID = CarMasterID;
                }
                if (CarSerialID > 0)
                {
                    orModel.CarSerialID = CarSerialID;
                }
            }
            orModel.CarPrice = model.CarPrice;
            orModel.CarColor = model.CarColor;
            orModel.RepCarMasterID = null;
            orModel.RepCarSerialID = null;
            orModel.RepCarTypeId = null;
            if (model.ReplacementCarId != null)
            {
                orModel.RepCarTypeId = model.ReplacementCarId;
                int CarMasterID = 0;
                int CarSerialID = 0;
                int brandID = 0;
                BLL.CarTypeAPI.Instance.GetSerialIDAndMasterBrandIDByCarTypeID(model.ReplacementCarId.Value, out CarSerialID, out CarMasterID,out brandID);
                //orModel.RepCarMasterID = CarMasterID;
                //orModel.RepCarSerialID = CarSerialID;
                if (CarMasterID > 0)
                {
                    orModel.RepCarMasterID = CarMasterID;
                }
                if (CarSerialID > 0)
                {
                    orModel.RepCarSerialID = CarSerialID;
                }
            }
            orModel.ReplacementCarBuyYear = model.ReplacementCarBuyYear;
            orModel.ReplacementCarBuyMonth = model.ReplacementCarBuyMonth;
            orModel.ReplacementCarColor = model.ReplacementCarColor;
            orModel.ReplacementCarUsedMiles = model.ReplacementCarUsedMiles;
            
            orModel.SalePrice = model.SalePrice;
            orModel.RepCarProvinceID = null;
            orModel.RepCarCityID = null;
            orModel.RepCarCountyID = null;
            if (model.ReplacementCarLocationID != null)
            {
                string locationName = BitAuto.YanFa.Crm2009.BLL.AreaInfo.Instance.GetAreaStrByAreaID(model.ReplacementCarLocationID.Value.ToString());
                switch (locationName.Split(',').Length)
                {
                    case 1: orModel.RepCarProvinceID = int.Parse(locationName.Split(',')[0]);
                        break;
                    case 2:
                        orModel.RepCarProvinceID = int.Parse(locationName.Split(',')[0]);
                        orModel.RepCarCityID = int.Parse(locationName.Split(',')[1]);
                        break;
                    case 3:
                        orModel.RepCarProvinceID = int.Parse(locationName.Split(',')[0]);
                        orModel.RepCarCityID = int.Parse(locationName.Split(',')[1]);
                        orModel.RepCarCountyID = int.Parse(locationName.Split(',')[2]);
                        break;
                    default:
                        break;
                }
            }
            orModel.DMSMemberCode = string.Empty;
            orModel.DMSMemberName = string.Empty;
            orModel.CallRecord = string.Empty;
            orModel.Status = 0;
            orModel.CreateTime = DateTime.Now;
            orModel.DealerID = model.DealerID;

            int recid = Insert(tran, orModel);
            if (recid > 0)
            {
                return true;
            }
            return false;
        }
    }
}

