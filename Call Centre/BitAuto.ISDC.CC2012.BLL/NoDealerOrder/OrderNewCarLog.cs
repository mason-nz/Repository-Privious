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
    /// 业务逻辑类OrderNewCarLog 的摘要说明。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2012-09-21 10:33:31 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OrderNewCarLog
    {
        #region Instance
        public static readonly OrderNewCarLog Instance = new OrderNewCarLog();
        #endregion

        #region Contructor
        protected OrderNewCarLog()
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
        public DataTable GetOrderNewCarLog(QueryOrderNewCarLog query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.OrderNewCarLog.Instance.GetOrderNewCarLog(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.OrderNewCarLog.Instance.GetOrderNewCarLog(new QueryOrderNewCarLog(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// 获取最大易湃新车订单ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxYPOrderID()
        {
            return Dal.OrderNewCarLog.Instance.GetMaxYPOrderID(0);
        }

        /// <summary>
        ///  获取最大易湃试驾订单ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxTestDriveYPOrderID()
        {
            return Dal.OrderNewCarLog.Instance.GetMaxYPOrderID(1);
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.OrderNewCarLog GetOrderNewCarLog(long RecID)
        {
            return Dal.OrderNewCarLog.Instance.GetOrderNewCarLog(RecID);
        }

        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(long RecID)
        {
            QueryOrderNewCarLog query = new QueryOrderNewCarLog();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetOrderNewCarLog(query, string.Empty, 1, 1, out count);
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
        /// 根据易湃订单ID，判断数据是否存在
        /// </summary>
        /// <param name="YPOrderID">易湃订单ID</param>
        /// <returns></returns>
        public bool IsExistsByYPOrderID(int YPOrderID)
        {
            return Dal.OrderNewCarLog.Instance.IsExistsByYPOrderID(YPOrderID);
        }
        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.OrderNewCarLog model)
        {
            return Dal.OrderNewCarLog.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.OrderNewCarLog model)
        {
            return Dal.OrderNewCarLog.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.OrderNewCarLog model)
        {
            return Dal.OrderNewCarLog.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.OrderNewCarLog model)
        {
            return Dal.OrderNewCarLog.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.OrderNewCarLog.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.OrderNewCarLog.Instance.Delete(sqltran, RecID);
        }

        #endregion

        /// <summary>
        /// 生成任务
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="orderType">订单类型（0-新车订单[默认]，1-试驾订单）</param>
        public void GenTask(DataTable dt, int orderType)
        {
            int taskCount = 0;
            int existCount = 0;
            BLL.Loger.Log4Net.Info("生成_易湃_" + (orderType == 0 ? "新车" : "试驾") + "订单_开始:");
            DateTime dtNow = DateTime.Now;
            DataTable dtNew = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int YPOrderID = int.Parse(dt.Rows[i]["OrderBusinessOpportunityID"].ToString());
                int? DealerID = (int?)GetColumnDefaultValue(dt.Rows[i], "DealerID", typeof(int));
                if (BLL.OrderNewCarLog.Instance.IsExistsByYPOrderID(YPOrderID))//存在
                {
                    BLL.Util.InsertUserLogNoUser("（" + (orderType == 0 ? "新车" : "试驾") + "）易湃订单ID：" + YPOrderID + "的记录已经存在，不在生成任务！"); existCount++;
                }
                else if (DealerID != null && DealerID.Value > 0)//Add=Masj,Date=2013-08-26，去掉免费订单的相关数据
                {
                    //BLL.Util.InsertUserLogNoUser("（" + (orderType == 0 ? "新车" : "试驾") + "）易湃订单ID：" + YPOrderID + "的记录经销商为0（免费订单），不生成任务！");
                    BLL.Loger.Log4Net.Info("（" + (orderType == 0 ? "新车" : "试驾") + "）易湃订单ID：" + YPOrderID + "的记录经销商为" + DealerID + "（免费订单），不生成任务！");
                }
                else//不存在
                {
                    dtNew.ImportRow(dt.Rows[i]);
                }
            }
            BLL.Loger.Log4Net.Info("检测拿到的订单数据中有" + existCount + "条数据在呼叫中心库中已经存在了");

            string connectionstrings = ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            SqlConnection connection = new SqlConnection(connectionstrings);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction("OrderNewCarLogTransaction");
            try
            {
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    Entities.OrderNewCarLog model = InitOrderNewCarLog(dtNew.Rows[i], orderType);
                    if (model != null)
                    {
                        long OrderNewCarLogID = Insert(tran, model); //插入 OrderNewCarLog 表
                        BLL.Loger.Log4Net.Info(string.Format((orderType == 0 ? "新车" : "试驾") + "任务生成表OrderNewCarLog成功，主键ID为：{0},无主订单ID为：{1},车款ID：{2}",
                                                  OrderNewCarLogID, model.YPOrderID, model.CarID));

                        if (model.CarID != Constant.INT_INVALID_VALUE && model.CarID != 0)
                        {
                            //Add By Chybin At 2013.2.13  如果Carid和OrderQuantity为空，就只插入OrderNewCarLog 表，不生成任务，不插入OrderNewCar 表
                            //Add By Masj At 2013.7.26 OrderQuantity为空去掉限制了
                            long taskID = BLL.OrderTask.Instance.InsertByOrder(tran, model); //根据接口得到的原始数据生成任务
                            if (BLL.OrderNewCar.Instance.IsExistsByTaskID(taskID))
                            {
                                BLL.Util.InsertUserLogNoUser(tran, "（" + (orderType == 0 ? "新车" : "试驾") + "）任务ID" + taskID + "的记录已经存在，不在生成任务！");
                            }
                            else if (BLL.OrderNewCar.Instance.InsertByTaskID(tran, taskID, model))
                            {
                                BLL.Util.InsertUserLogNoUser(tran, "（" + (orderType == 0 ? "新车" : "试驾") + "）任务ID" + taskID + "的记录生成成功！"); taskCount++;
                            }
                        }
                    }
                }
                //事务提交
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                string msg = "（新车）生成任务失败!失败原因：" + ex.Message;
                BLL.Util.InsertUserLogNoUser(msg);
                BLL.Loger.Log4Net.Error(msg, ex);
            }
            finally
            {
                connection.Close();
            }
            string msgTitle = "一共生成（新车）任务：" + taskCount + "个";
            BLL.Util.InsertUserLogNoUser(msgTitle);
            TimeSpan ts = new TimeSpan();
            ts = DateTime.Now - dtNow;
            BLL.Loger.Log4Net.Info("生成_易湃_新车订单_结束:" + msgTitle + ",用时" + ts.TotalSeconds + "秒。");
        }

        /// <summary>
        /// 初始化新车订单日志信息
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordertype">订单类型（0-新车订单[默认]，1-试驾订单）</param>
        /// <returns></returns>
        private Entities.OrderNewCarLog InitOrderNewCarLog(DataRow dr, int orderType)
        {
            Entities.OrderNewCarLog model = new Entities.OrderNewCarLog();

            try
            {
                model.YPOrderID = int.Parse(dr["OrderBusinessOpportunityID"].ToString());
                model.UserName = dr["UserName"].ToString().Trim();
                model.UserPhone = dr["UserPhone"].ToString().Trim();
                model.UserMobile = dr["UserMobile"].ToString().Trim();
                model.UserMail = dr["UserMail"].ToString().Trim();
                int userGender = -1;
                if (int.TryParse(dr["UserGender"].ToString(), out userGender))
                {
                    model.UserGender = userGender;
                }
                else
                {
                    model.UserGender = null;
                }
                int locationID = -1;
                if (int.TryParse(dr["LocationID"].ToString(), out locationID))
                {
                    model.LocationID = locationID;
                }
                else
                {
                    model.LocationID = null;
                }
                model.LocationName = dr["LocationName"].ToString().Trim();
                model.UserAddress = dr["UserAddress"].ToString().Trim();
                model.OrderCreateTime = DateTime.Parse(dr["OrderBusinessOpportunityCreateTime"].ToString());
                model.OrderPrice = (decimal?)GetColumnDefaultValue(dr, "OrderPrice", typeof(decimal));
                model.OrderQuantity = (int?)GetColumnDefaultValue(dr, "OrderQuantity", typeof(int));
                model.OrderRemark = (string)GetColumnDefaultValue(dr, "OrderRemark", typeof(string));
                model.CarID = (int?)GetColumnDefaultValue(dr, "CarID", typeof(int));
                model.CarFullName = (string)GetColumnDefaultValue(dr, "CarFullName", typeof(string));
                model.CarPrice = (decimal?)GetColumnDefaultValue(dr, "CarPrice", typeof(decimal));
                model.CarColor = (string)GetColumnDefaultValue(dr, "CarColor", typeof(string));
                model.CarPromotions = (string)GetColumnDefaultValue(dr, "CarPromotions", typeof(string));
                model.DealerID = (int?)GetColumnDefaultValue(dr, "DealerID", typeof(int));
                model.OrderType = orderType;
                //if (dr.Table.Columns.Contains("OrderPrice"))
                //{
                //    decimal orderPrice = -1;
                //    if (decimal.TryParse(dr["OrderPrice"].ToString(), out orderPrice))
                //    {
                //        model.OrderPrice = orderPrice;
                //    }
                //    else
                //    {
                //        model.OrderPrice = null;
                //    }
                //}
                //int orderQuantity = -1;
                //if (int.TryParse(dr["OrderQuantity"].ToString(), out orderQuantity))
                //{
                //    model.OrderQuantity = orderQuantity;
                //}
                //else
                //{
                //    model.OrderQuantity = null;
                //}

                //int carID = -1;
                //if (int.TryParse(dr["CarID"].ToString(), out carID))
                //{
                //    model.CarID = carID;
                //}
                //else
                //{
                //    model.CarID = null;
                //}

                //model.CarPrice = null;
                //if (dr.Table.Columns.Contains("CarPrice"))
                //{
                //    decimal carPrice = -1;
                //    if (decimal.TryParse(dr["CarPrice"].ToString(), out carPrice))
                //    {
                //        model.CarPrice = carPrice;
                //    }
                //    else
                //    {
                //        model.CarPrice = null;
                //    }
                //}


                //int dealerID = -1;
                //if (int.TryParse(dr["DealerID"].ToString(), out dealerID))
                //{
                //    model.DealerID = dealerID;
                //}
                //else
                //{
                //    model.DealerID = null;
                //}

            }
            catch (Exception ex)
            {
                BLL.Util.InsertUserLogNoUser("初始化新车订单信息失败!失败原因：" + ex.Message);
                return null;
            }
            return model;
        }

        private static object GetColumnDefaultValue(DataRow dr, string columnName, Type t)
        {
            object ret = null;
            if (dr.Table.Columns.Contains(columnName))
            {
                switch (t.ToString())
                {
                    case "System.String": ret = dr[columnName].ToString().Trim();
                        break;
                    case "System.Decimal": decimal dec = -1;
                        if (decimal.TryParse(dr[columnName].ToString(), out dec))
                            ret = dec;
                        break;
                    case "System.Int32": int i = -1;
                        if (int.TryParse(dr[columnName].ToString(), out i))
                            ret = i;
                        break;
                    case "System.DateTime": DateTime dt = new DateTime();
                        if (DateTime.TryParse(dr[columnName].ToString(), out dt))
                            ret = dt;
                        break;
                    default:
                        break;
                }
            }
            return ret;
        }
    }
}

