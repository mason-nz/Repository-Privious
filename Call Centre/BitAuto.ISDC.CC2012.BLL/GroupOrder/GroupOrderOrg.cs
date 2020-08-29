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
    /// 业务逻辑类GroupOrderOrg 的摘要说明。
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
    public class GroupOrderOrg
    {
        #region Instance
        public static readonly GroupOrderOrg Instance = new GroupOrderOrg();
        #endregion

        #region Contructor
        protected GroupOrderOrg()
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
        public DataTable GetGroupOrderOrg(QueryGroupOrderOrg query, string order, int currentPage, int pageSize, out int totalCount)
        {
            return Dal.GroupOrderOrg.Instance.GetGroupOrderOrg(query, order, currentPage, pageSize, out totalCount);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            int totalCount = 0;
            return Dal.GroupOrderOrg.Instance.GetGroupOrderOrg(new QueryGroupOrderOrg(), string.Empty, 1, 1000000, out totalCount);
        }

        /// <summary>
        /// 获取最大易湃订单ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxYPOrderID()
        {
            return Dal.GroupOrderOrg.Instance.GetMaxYPOrderID();
        }
        #endregion

        #region GetModel
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.GroupOrderOrg GetGroupOrderOrg(long RecID)
        {

            return Dal.GroupOrderOrg.Instance.GetGroupOrderOrg(RecID);
        }

        /// <summary>
        /// 根据易湃订单ID，返回实体
        /// </summary>
        /// <param name="OrderID">易湃订单ID</param>
        /// <returns>返回GroupOrderOrg实体</returns>
        public Entities.GroupOrderOrg GetGroupOrderOrgByOrderID(int OrderID)
        {
            return Dal.GroupOrderOrg.Instance.GetGroupOrderOrgByOrderID(OrderID);
        }
        #endregion

        #region IsExists
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsExistsByRecID(long RecID)
        {
            QueryGroupOrderOrg query = new QueryGroupOrderOrg();
            query.RecID = RecID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetGroupOrderOrg(query, string.Empty, 1, 1, out count);
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
            return Dal.GroupOrderOrg.Instance.IsExistsByYPOrderID(YPOrderID);
        }
        #endregion

        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.GroupOrderOrg model)
        {
            return Dal.GroupOrderOrg.Instance.Insert(model);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Insert(SqlTransaction sqltran, Entities.GroupOrderOrg model)
        {
            return Dal.GroupOrderOrg.Instance.Insert(sqltran, model);
        }

        #endregion

        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(Entities.GroupOrderOrg model)
        {
            return Dal.GroupOrderOrg.Instance.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(SqlTransaction sqltran, Entities.GroupOrderOrg model)
        {
            return Dal.GroupOrderOrg.Instance.Update(sqltran, model);
        }

        #endregion

        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long RecID)
        {

            return Dal.GroupOrderOrg.Instance.Delete(RecID);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, long RecID)
        {

            return Dal.GroupOrderOrg.Instance.Delete(sqltran, RecID);
        }

        #endregion

        /// <summary>
        /// 生成任务
        /// </summary>
        /// <param name="dt"></param>
        public void GenTask(DataTable dt)
        {
            int taskCount = 0;
            int existCount = 0;
            BLL.Loger.Log4Net.Info("生成_易湃_团购订单_开始:");
            DateTime dtNow = DateTime.Now;
            DataTable dtNew = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int YPOrderID = int.Parse(dt.Rows[i]["OrderID"].ToString());
                if (BLL.GroupOrderOrg.Instance.IsExistsByYPOrderID(YPOrderID))//存在
                {
                    BLL.Util.InsertUserLogNoUser("易湃团购订单ID：" + YPOrderID + "的记录已经存在，不在生成任务！"); existCount++;
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
            SqlTransaction tran = connection.BeginTransaction("GroupOrderOrgTransaction");
            try
            {
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    Entities.GroupOrderOrg model = InitOrderNewCarLog(dtNew.Rows[i]);
                    if (model != null)
                    {
                        long GroupOrderOrgID = Insert(tran, model); //插入 GroupOrderOrg 表
                        BLL.Loger.Log4Net.Info(string.Format("团购任务生成表GroupOrderOrg成功，主键ID为：{0},易湃订单ID为：{1},车款ID：{2}",
                                                  GroupOrderOrgID, model.OrderID, model.CarID));

                        if (model.CarMasterID != null && model.CarMasterID.Value > 0 &&
                            model.CarSerialID != null && model.CarSerialID.Value > 0 &&
                            model.CarID != null && model.CarID.Value > 0)
                        {
                            //Add By Masj At 2013.12.4 车型为空，就只插入GroupOrderOrg 表，不生成任务，不插入GroupOrder 表
                            long taskID = BLL.GroupOrderTask.Instance.InsertByOrder(tran, model); //根据接口得到的原始数据生成任务
                            if (BLL.GroupOrder.Instance.IsExistsByTaskID(taskID))
                            {
                                BLL.Util.InsertUserLogNoUser(tran, "团购任务ID" + taskID + "的记录已经存在，不在生成任务！");
                            }
                            else if (BLL.GroupOrder.Instance.InsertByTaskID(tran, taskID, model))
                            {
                                BLL.Util.InsertUserLogNoUser(tran, "团购任务ID" + taskID + "的记录生成成功！"); taskCount++;
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
            string msgTitle = "一共生成团购任务：" + taskCount + "个";
            BLL.Util.InsertUserLogNoUser(msgTitle);
            TimeSpan ts = new TimeSpan();
            ts = DateTime.Now - dtNow;
            BLL.Loger.Log4Net.Info("生成_易湃_团购订单_结束:" + msgTitle + ",用时" + ts.TotalSeconds + "秒。");
        }

        /// <summary>
        /// 初始化团购订单日志信息
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Entities.GroupOrderOrg InitOrderNewCarLog(DataRow dr)
        {
            Entities.GroupOrderOrg model = new Entities.GroupOrderOrg();

            try
            {
                model.OrderID = int.Parse(dr["OrderID"].ToString());
                model.OrderCode = int.Parse(dr["OrderCode"].ToString().Trim());
                model.CarMasterID = (int?)GetColumnDefaultValue(dr, "CarMasterID", typeof(int));
                model.CarMasterName = dr["CarMasterName"].ToString().Trim();
                model.CarSerialID = (int?)GetColumnDefaultValue(dr, "CarSerialID", typeof(int));
                model.CarSerialName = dr["CarSerialName"].ToString().Trim();
                model.CarID = (int?)GetColumnDefaultValue(dr, "CarID", typeof(int));
                model.CarName = dr["CarName"].ToString().Trim();
                model.Price = (decimal?)GetColumnDefaultValue(dr, "Price", typeof(decimal));
                model.DealerID = (int?)GetColumnDefaultValue(dr, "DealerID", typeof(int));
                model.DealerName = dr["DealerName"].ToString().Trim();
                model.CustomerName = dr["CustomerName"].ToString().Trim();
                model.CustomerTel = dr["CustomerTel"].ToString().Trim();
                model.OrderCreateTime = DateTime.Parse(dr["CreateDateTime"].ToString());
                model.ProvinceID = (int?)GetColumnDefaultValue(dr, "ProvinceID", typeof(int));
                model.ProvinceName = dr["ProvinceName"].ToString().Trim();
                model.CityID = (int?)GetColumnDefaultValue(dr, "CityID", typeof(int));
                model.CityName = dr["CityName"].ToString().Trim();

            }
            catch (Exception ex)
            {
                BLL.Util.InsertUserLogNoUser("初始化团购订单信息失败!失败原因：" + ex.Message);
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

