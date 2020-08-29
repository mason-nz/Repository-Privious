using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using XYAuto.Utils;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_8;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class ADOrderInfo : DataBase
    {
        public const string P_ADOrderInfo_SELECT = "p_ADOrderInfo_Select";

        #region Instance

        public static readonly ADOrderInfo Instance = new ADOrderInfo();

        #endregion Instance

        #region Select

        /// <summary>
        /// 根据主键值得到一个对象实体
        /// </summary>
        public Entities.ADOrderInfo GetModel(int RecId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 RecID,OrderID,OrderName,BeginTime,EndTime,Note,UploadFileURL,MediaType,TotalAmount,Status,CreateTime,CreaetUserID");
            strSql.Append(" FROM ADOrderInfo ");
            strSql.Append(" WHERE RecId=@RecId");
            SqlParameter[] parameters = {
                    new SqlParameter("@RecId", SqlDbType.Int,4)
            };
            parameters[0].Value = RecId;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.ADOrderInfo DataRowToModel(DataRow row)
        {
            Entities.ADOrderInfo model = new Entities.ADOrderInfo();
            if (row != null)
            {
                if (row["RecId"] != null && row["RecId"].ToString() != "")
                {
                    model.RecID = int.Parse(row["RecId"].ToString());
                }
                if (row["OrderID"] != null && row["OrderID"].ToString() != "")
                {
                    model.OrderID = row["OrderID"].ToString();
                }
                if (row["OrderName"] != null)
                {
                    model.OrderName = row["OrderName"].ToString();
                }
                if (row["BeginTime"] != null && row["BeginTime"].ToString() != "")
                {
                    model.BeginTime = DateTime.Parse(row["BeginTime"].ToString());
                }
                if (row["EndTime"] != null && row["EndTime"].ToString() != "")
                {
                    model.EndTime = DateTime.Parse(row["EndTime"].ToString());
                }
                if (row["Note"] != null && row["Note"].ToString() != "")
                {
                    model.Note = row["Note"].ToString();
                }
                if (row["UploadFileURL"] != null && row["UploadFileURL"].ToString() != "")
                {
                    model.UploadFileURL = row["UploadFileURL"].ToString();
                }
                if (row["MediaType"] != null && row["MediaType"].ToString() != "")
                {
                    model.MediaType = int.Parse(row["MediaType"].ToString());
                }
                if (row["TotalAmount"] != null && row["TotalAmount"].ToString() != "")
                {
                    model.TotalAmount = decimal.Parse(row["TotalAmount"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["CreaetUserID"] != null && row["CreaetUserID"].ToString() != "")
                {
                    model.CreateUserID = int.Parse(row["CreaetUserID"].ToString());
                }
            }
            return model;
        }

        #region 根据订单号查询得到一个对象实体

        public Entities.ADOrderInfo GetADOrderInfo(string OrderID)
        {
            QueryADOrderInfo query = new QueryADOrderInfo();
            query.OrderID = OrderID;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetADOrderInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleADOrderInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        private Entities.ADOrderInfo LoadSingleADOrderInfo(DataRow row)
        {
            Entities.ADOrderInfo model = new Entities.ADOrderInfo();
            model.RecID = int.Parse(row["RecID"].ToString());
            model.OrderID = row["OrderID"].ToString();
            if (row["CRMCustomerID"].ToString() != "")
            {
                model.CRMCustomerID = row["CRMCustomerID"].ToString();
            }
            if (row["CustomerText"].ToString() != "")
            {
                model.CustomerText = row["CustomerText"].ToString();
            }
            if (row["CreatorName"].ToString() != "")
            {
                model.CreatorName = row["CreatorName"].ToString();
            }
            if (row["CreatorUserName"].ToString() != "")
            {
                model.CreatorUserName = row["CreatorUserName"].ToString();
            }
            if (row["CustomerName"].ToString() != "")
            {
                model.CustomerName = row["CustomerName"].ToString();
            }
            if (row["CustomerUserName"].ToString() != "")
            {
                model.CustomerUserName = row["CustomerUserName"].ToString();
            }

            if (row["OrderName"].ToString() != "")
            {
                model.OrderName = row["OrderName"].ToString();
            }
            if (row["BeginTime"].ToString() != "")
            {
                model.BeginTime = DateTime.Parse(row["BeginTime"].ToString());
            }
            if (row["EndTime"].ToString() != "")
            {
                model.EndTime = DateTime.Parse(row["EndTime"].ToString());
            }
            if (row["Note"].ToString() != "")
            {
                model.Note = row["Note"].ToString();
            }
            if (row["UploadFileURL"].ToString() != "")
            {
                model.UploadFileURL = row["UploadFileURL"].ToString();
            }
            if (row["MediaType"].ToString() != "")
            {
                model.MediaType = int.Parse(row["MediaType"].ToString());
            }
            if (row["TotalAmount"].ToString() != "")
            {
                model.TotalAmount = decimal.Parse(row["TotalAmount"].ToString());
            }
            if (row["Status"].ToString() != "")
            {
                model.Status = int.Parse(row["Status"].ToString());
            }
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["CustomerID"].ToString() != "")
            {
                model.CustomerID = int.Parse(row["CustomerID"].ToString());
            }

            return model;
        }

        #endregion 根据订单号查询得到一个对象实体

        /// 分页查询
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetADOrderInfo(QueryADOrderInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.OrderID = '" + query.OrderID + "'";
            }
            if (query.OrderName != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.OrderName = '" + query.OrderName + "'";
            }

            DateTime beginTime;
            if (query.BeginCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.BeginCreateTime, out beginTime))
            {
                where += " and a.CreateTime>='" + beginTime + "'";
            }
            DateTime endTime;
            if (query.EndCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.EndCreateTime, out endTime))
            {
                where += " and a.CreateTime<'" + endTime.AddDays(1) + "'";
            }

            SqlParameter[] parameters = {
                    new SqlParameter("@where", SqlDbType.NVarChar, 4000),
                    new SqlParameter("@order", SqlDbType.NVarChar, 200),
                    new SqlParameter("@pagesize", SqlDbType.Int, 4),
                    new SqlParameter("@indexpage", SqlDbType.Int, 4),
                    new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
                    };
            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderInfo_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #region 张立彬

        /// <summary>
        /// 2017-02-24 张立彬
        /// 根据条件查询订单信息
        /// </summary>
        /// <param name="orderType">订单类型（0:主订单；1：子订单）</param>
        /// <param name="orderNum">	订单编号（精确查找）</param>
        /// <param name="demandDescribe">需求名称（模糊查询</param>
        /// <param name="mediaType">资源类型 （全部为0）</param>
        /// <param name="creater">创建人(模糊查询）</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="PageIndex">第几页</param>
        /// <param name="CustomerId">  广告主ID（精确查找）</param>
        /// <param name="OrderSource">  项目类型(0全部 1代客下单 2自助下单) </param>
        /// <param name="IsCRM"> 是否关联CRM(0全部 1关联CRM 2否 不关联CRM) </param>
        /// <param name="SubOrderNum">子订单号</param>
        /// <returns></returns>
        public DataTable SelectOrderInfo(int orderType, string orderNum, string demandDescribe, int mediaType, string creater, int pagesize, int PageIndex, int orderState, string strWhere, int userId, int CustomerId, int OrderSource, int IsCRM, string SubOrderNum)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderType", SqlDbType.Int),
                    new SqlParameter("@OrderNum", SqlDbType.VarChar,20),
                    new SqlParameter("@DemandDescribe", SqlDbType.VarChar,50),
                    new SqlParameter("@MediaType", SqlDbType.Int),
                    new SqlParameter("@Creater", SqlDbType.VarChar,20),
                    new SqlParameter("@pagesize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int),
                    new SqlParameter("@OrderState", SqlDbType.Int),
                    new SqlParameter("@strWhere", SqlDbType.NVarChar,700),
                    new SqlParameter("@userId", SqlDbType.Int),
                    new SqlParameter("@CustomerId", SqlDbType.Int),
                    new SqlParameter("@OrderSource", SqlDbType.Int),
                    new SqlParameter("@IsCRM", SqlDbType.Int),
                    new SqlParameter("@SubOrderNum", SqlDbType.VarChar,20)
                    };
            parameters[0].Value = orderType;
            parameters[1].Value = orderNum;
            parameters[2].Value = demandDescribe;
            parameters[3].Value = mediaType;
            parameters[4].Value = creater;
            parameters[5].Value = pagesize;
            parameters[6].Value = PageIndex;
            parameters[7].Direction = ParameterDirection.Output;
            parameters[8].Value = orderState;
            parameters[9].Value = strWhere;
            parameters[10].Value = userId;
            parameters[11].Value = CustomerId;
            parameters[12].Value = OrderSource;
            parameters[13].Value = IsCRM;
            parameters[14].Value = SubOrderNum;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectOrderInfo", parameters);
            int totalCount = (int)(parameters[7].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /// <summary>
        /// 2017-03-21 张立彬
        /// 根据项目ID查询订单信息
        /// </summary>
        /// <param name="orderID">项目ID</param>
        /// <returns></returns>
        public DataTable SelectSubOrderByOrderID(string orderID, string strWhere)
        {
            SqlParameter[] parameters = {
            new SqlParameter("@OrderId",SqlDbType.VarChar,20),
            new SqlParameter("@strWhere",SqlDbType.VarChar,700),
        };
            parameters[0].Value = orderID;
            parameters[1].Value = strWhere;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectSubOrderByOrderId", parameters);
            return ds.Tables[0];
        }

        #endregion 张立彬

        #endregion Select

        #region Insert

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Insert(Entities.ADOrderInfo model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.VarChar,20),
                    new SqlParameter("@OrderName",SqlDbType.VarChar,50),
                    new SqlParameter("@BeginTime",SqlDbType.DateTime),
                    new SqlParameter("@EndTime",SqlDbType.DateTime),
                    new SqlParameter("@Note",SqlDbType.VarChar,1000),
                    new SqlParameter("@UploadFileURL",SqlDbType.VarChar,200),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@TotalAmount",SqlDbType.Decimal,18),
                    new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@CustomerID",SqlDbType.Int,4),
                    new SqlParameter("@CRMCustomerID",SqlDbType.VarChar,100),
                    new SqlParameter("@CustomerText",SqlDbType.VarChar,200)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            //0001/1/1 0:00:00

            DateTime tmpDT = new DateTime(1, 1, 1);
            //if (tmpDT == model.BeginTime)
            //{
            //    model.BeginTime = new DateTime(1990, 1, 1);
            //}
            //if (tmpDT == model.EndTime)
            //{
            //    model.EndTime = new DateTime(1990, 1, 1);
            //}
            parameters[1].Value = model.OrderName;
            if (tmpDT == model.BeginTime)
            {
                parameters[2].Value = null;
            }
            else
            {
                parameters[2].Value = model.BeginTime;
            }
            if (tmpDT == model.EndTime)
            {
                parameters[3].Value = null;
            }
            else
            {
                parameters[3].Value = model.EndTime;
            }
            parameters[4].Value = model.Note;
            parameters[5].Value = model.UploadFileURL;
            parameters[6].Value = model.MediaType;
            parameters[7].Value = model.TotalAmount;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.CreateUserID;
            parameters[10].Value = model.CustomerID;
            parameters[11].Value = model.CRMCustomerID;
            parameters[12].Value = model.CustomerText;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderInfo_Insert", parameters);
            return (string)parameters[0].Value;
        }

        #endregion Insert

        #region Update

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Entities.ADOrderInfo model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.VarChar,20),
                    new SqlParameter("@OrderName", SqlDbType.VarChar,50),
                    new SqlParameter("@BeginTime",SqlDbType.DateTime),
                    new SqlParameter("@EndTime",SqlDbType.DateTime),
                    new SqlParameter("@Note",SqlDbType.VarChar,1000),
                    new SqlParameter("@UploadFileURL",SqlDbType.VarChar,200),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@TotalAmount",SqlDbType.Decimal,18),
                    new SqlParameter("@Status", SqlDbType.Int,4),
                    new SqlParameter("@CreateTime",SqlDbType.DateTime),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@CustomerID",SqlDbType.Int,4)
                                        };

            DateTime tmpDT = new DateTime(1, 1, 1);
            parameters[0].Value = model.OrderID;
            parameters[1].Value = model.OrderName;
            if (tmpDT == model.BeginTime)
            {
                parameters[2].Value = null;
            }
            else
            {
                parameters[2].Value = model.BeginTime;
            }
            if (tmpDT == model.EndTime)
            {
                parameters[3].Value = null;
            }
            else
            {
                parameters[3].Value = model.EndTime;
            }
            parameters[4].Value = model.Note;
            parameters[5].Value = model.UploadFileURL;
            parameters[6].Value = model.MediaType;
            parameters[7].Value = model.TotalAmount;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.CreateTime;
            parameters[10].Value = model.CreateUserID;
            parameters[11].Value = model.CustomerID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderInfo_Update", parameters); ;
        }

        #endregion Update

        #region Delete

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string OrderID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.VarChar,50)};
            parameters[0].Value = OrderID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderInfo_Delete", parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(SqlTransaction sqltran, string OrderID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.VarChar,50)};
            parameters[0].Value = OrderID;

            return SqlHelper.ExecuteNonQuery(sqltran, CommandType.StoredProcedure, "p_ADOrderInfo_Delete", parameters);
        }

        #endregion Delete

        #region 根据项目号删除项目订单广告位排期信息

        /// <summary>
        /// 根据项目号删除项目订单广告位排期信息
        /// </summary>
        /// <param name="OrderID">项目号</param>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="ISDeleteADOrder">是否删除项目</param>
        /// <returns></returns>
        public string p_ADOrderAllInfo_Delete(string OrderID, int MediaType, bool ISDeleteADOrder)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.VarChar,20),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@ISDeleteADOrder",SqlDbType.Bit),
                    new SqlParameter("@Msg",SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = OrderID;
            parameters[1].Value = MediaType;
            if (ISDeleteADOrder)
            {
                parameters[2].Value = 1;
            }
            else
            {
                parameters[2].Value = 0;
            }
            parameters[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderAllInfo_Delete", parameters);

            return (string)parameters[3].Value;
        }

        #endregion 根据项目号删除项目订单广告位排期信息

        #region 更改主订单状态

        /// <summary>
        /// 根据订单号更改主订单状态
        /// </summary>
        /// <param name="orderid">主订单号</param>
        /// <param name="status">状态，具体值看订单状态枚举</param>
        /// <returns></returns>
        public int UpdateStatus_ADOrder(string orderid, int status)
        {
            string sqlstr = "UPDATE dbo.ADOrderInfo SET Status=" + status + " WHERE OrderID='" + orderid + "'";
            int retval = 0;

            retval = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
            return retval;
        }

        #endregion 更改主订单状态

        #region 更改主订单价格

        /// <summary>
        /// 根据订单号更改主订单状态
        /// </summary>
        /// <param name="orderid"></param>
        /// <param name="pirce"></param>
        /// <returns></returns>
        public int UpdateTotalAmount_ADOrder(string orderid, decimal total)
        {
            string sqlstr = "UPDATE dbo.ADOrderInfo SET TotalAmount=" + total + " WHERE OrderID='" + orderid + "'";
            int retval = 0;

            retval = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
            return retval;
        }

        #endregion 更改主订单价格

        #region 自媒体-根据媒体类型、刊例广告位ID获取广告位信息

        /// <summary>
        /// 自媒体-根据媒体类型、刊例广告位ID获取广告位信息
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="pubDetailID"></param>
        /// <returns></returns>
        public DataTable GetSelfMediaDetail_MediaTypePubDetailID(int mediaType, int pubDetailID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType", SqlDbType.Int),
                    new SqlParameter("@PubDetailID", SqlDbType.Int)
                    };
            parameters[0].Value = mediaType;
            parameters[1].Value = pubDetailID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelfMediaDetail_Select", parameters);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        #endregion 自媒体-根据媒体类型、刊例广告位ID获取广告位信息

        #region 自媒体-根据媒体类型、媒体ID获取媒体信息

        /// <summary>
        /// 自媒体-根据媒体类型、媒体ID获取媒体信息
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="mediaID">嫖体ID</param>
        /// <returns></returns>
        public DataTable GetSelfMediaDetail_MediaTypeMediaID(int mediaType, int mediaID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType", SqlDbType.Int),
                    new SqlParameter("@MediaID", SqlDbType.Int)
                    };
            parameters[0].Value = mediaType;
            parameters[1].Value = mediaID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelfMediaDetail_SelectByMediaID", parameters);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        #endregion 自媒体-根据媒体类型、媒体ID获取媒体信息

        #region APP-根据广告位ID获取媒体名称广告位信息

        public DataTable GetAPPMediaDetail_PubDetailID(int pubDetailID)
        {
            string sqlstr = string.Format("SELECT  app.Name,app.HeadIconURL,t1.AdPosition ,t1.AdForm ,t1.Style ,t1.CarouselCount ,t1.PlayPosition ,t1.SysPlatform FROM    dbo.Publish_ExtendInfoPCAPP t1 LEFT JOIN dbo.Media_PCAPP app ON t1.MediaID = app.MediaID WHERE t1.ADDetailID={0}", pubDetailID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        #endregion APP-根据广告位ID获取媒体名称广告位信息

        #region 根据媒体类型、广告位ID获取刊例ID、成本价、采购、销售折扣

        /// <summary>
        /// #region 根据媒体类型、广告位ID获取刊例ID、成本价、采购、销售折扣
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="pubDetailID">媒体ID</param>
        /// <returns></returns>
        public DataTable p_GetPubDetailInfo_Select(int mediaType, int pubDetailID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType", SqlDbType.Int, 4),
                    new SqlParameter("@PubDetailID", SqlDbType.Int, 4)
                    };
            parameters[0].Value = mediaType;
            parameters[1].Value = pubDetailID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetPubDetailInfo_Select", parameters);
            return ds.Tables[0];
        }

        #endregion 根据媒体类型、广告位ID获取刊例ID、成本价、采购、销售折扣

        #region 根据个人姓名、公司名称或手机号模糊查询广告主

        /// <summary>
        /// 根据个人姓名、公司名称或手机号模糊查询广告主
        /// </summary>
        /// <param name="AEUserID">AE的userid</param>
        /// <param name="NameOrMobile">模糊查询的姓名、公司名称或手机号</param>
        /// <returns></returns>
        public DataTable p_ADMaster_Select(int AEUserID, string NameOrMobile, int IsAEAuth)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@AEUserID", SqlDbType.Int, 4),
                    new SqlParameter("@NameOrMobile", SqlDbType.NVarChar, 200),
                    new SqlParameter("@IsAEAuth", SqlDbType.Bit, 2)
                    };
            parameters[0].Value = AEUserID;
            parameters[1].Value = NameOrMobile;
            parameters[2].Value = IsAEAuth;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADMaster_Select", parameters);
            return ds.Tables[0];
        }

        #endregion 根据个人姓名、公司名称或手机号模糊查询广告主

        #region AE提交修改订单，验证广告主是否存在

        /// <summary>
        /// AE提交修改订单，验证广告主是否存在
        /// </summary>
        /// <param name="AEUserID">AE的userid</param>
        /// <param name="CustomerID">广告主UserID</param>
        /// <returns></returns>
        public string p_ADMaster_IsExist(int AEUserID, int CustomerID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@AEUserID", SqlDbType.Int, 4),
                    new SqlParameter("@CustomerID", SqlDbType.Int, 4),
                    new SqlParameter("@Msg",SqlDbType.VarChar,50)
                    };
            parameters[0].Value = AEUserID;
            parameters[1].Value = CustomerID;
            parameters[2].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADMaster_IsExistV1_1_8", parameters);
            return (string)parameters[2].Value;
        }

        #endregion AE提交修改订单，验证广告主是否存在

        #region 根据媒体类型广告位ID获取刊例ID、成本价、采购、销售折扣V1.1

        /// <summary>
        /// #region 根据媒体类型、广告位ID获取刊例ID、成本价、采购、销售折扣
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="pubDetailID">媒体ID</param>
        /// <returns></returns>
        public DataTable p_GetPubDetailInfo_SelectV1_1(int mediaType, int pubDetailID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType", SqlDbType.Int, 4),
                    new SqlParameter("@PubDetailID", SqlDbType.Int, 4)
                    };
            parameters[0].Value = mediaType;
            parameters[1].Value = pubDetailID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetPubDetailInfo_SelectV1_1", parameters);
            return ds.Tables[0];
        }

        #endregion 根据媒体类型广告位ID获取刊例ID、成本价、采购、销售折扣V1.1

        #region 根据项目号删除项目订单广告位排期信息V1.1

        /// <summary>
        /// 根据项目号删除项目订单广告位排期信息
        /// </summary>
        /// <param name="OrderID">项目号</param>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="ISDeleteADOrder">是否删除项目</param>
        /// <returns></returns>
        public string p_ADOrderAllInfo_DeleteV1_1(string OrderID, int MediaType, bool ISDeleteADOrder)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.VarChar,20),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@ISDeleteADOrder",SqlDbType.Bit),
                    new SqlParameter("@Msg",SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = OrderID;
            parameters[1].Value = MediaType;
            if (ISDeleteADOrder)
            {
                parameters[2].Value = 1;
            }
            else
            {
                parameters[2].Value = 0;
            }
            parameters[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderAllInfo_DeleteV1_1", parameters);

            return (string)parameters[3].Value;
        }

        #endregion 根据项目号删除项目订单广告位排期信息V1.1

        #region 自媒体-根据媒体类型、刊例广告位ID获取广告位信息V1.1

        /// <summary>
        /// 自媒体-根据媒体类型、刊例广告位ID获取广告位信息
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="pubDetailID"></param>
        /// <returns></returns>
        public DataTable GetSelfMediaDetail_MediaTypePubDetailIDV1_1(int mediaType, int pubDetailID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType", SqlDbType.Int),
                    new SqlParameter("@PubDetailID", SqlDbType.Int)
                    };
            parameters[0].Value = mediaType;
            parameters[1].Value = pubDetailID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelfMediaDetail_SelectV1_1", parameters);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        #endregion 自媒体-根据媒体类型、刊例广告位ID获取广告位信息V1.1

        #region 根据项目号订单号查询订单操作日志V1.1

        public DataTable GeADOrderOperateInfoV1_1(string orderid, string suborderid)
        {
            string sqlstr = "SELECT CASE WHEN ud.TrueName IS NULL OR ud.TrueName='' THEN ui.UserName ELSE ud.TrueName END AS 'Creator',";
            sqlstr += "t.CurrentStatus AS 'LastOrderStatus',t.OrderStatus,t.CreateTime FROM ";
            sqlstr += "(SELECT * FROM dbo.ADOrderOperateInfo WHERE OptType IN(27001,27002) AND OrderID=@OrderID ";
            sqlstr += "UNION ALL ";
            sqlstr += "SELECT * FROM dbo.ADOrderOperateInfo WHERE SubOrderID=@SubOrderID ";
            sqlstr += ")t LEFT JOIN dbo.UserInfo ui ON t.CreateUserID=ui.UserID LEFT JOIN dbo.UserDetailInfo ud ON ud.UserID = ui.UserID ";
            sqlstr += "ORDER BY t.CreateTime DESC";
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.VarChar),
                    new SqlParameter("@SubOrderID", SqlDbType.VarChar)
                    };
            parameters[0].Value = orderid;
            parameters[1].Value = suborderid;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        #endregion 根据项目号订单号查询订单操作日志V1.1

        #region 根据微信号或名称模类查询V1.1

        public string QueryWeChat_NumerOrNameV1_1(int UserID, string NumberORName, int AuditStatus, out List<Dictionary<string, object>> dicList)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@NumberORName", SqlDbType.VarChar,20),
                    new SqlParameter("@AuditStatus", SqlDbType.Int,4),
                    new SqlParameter("@Msg", SqlDbType.VarChar,200)
                    };
            parameters[0].Value = UserID;
            parameters[1].Value = NumberORName;
            parameters[2].Value = AuditStatus;
            parameters[3].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_WeChat_SelectV1_1", parameters);

            dicList = new List<Dictionary<string, object>>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Name", row["Name"]);
                    dic.Add("Number", row["Number"]);
                    dic.Add("MediaID", row["MediaID"]);

                    dicList.Add(dic);
                }
            }
            return (string)parameters[3].Value;
        }

        #endregion 根据微信号或名称模类查询V1.1

        #region 根据APP名称模类查询V1.1.4

        public string p_APPSelectByName(int UserID, string Name, int AuditStatus, out List<Dictionary<string, object>> dicList)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@Name", SqlDbType.VarChar,20),
                    new SqlParameter("@AuditStatus", SqlDbType.Int,4),
                    new SqlParameter("@Msg", SqlDbType.VarChar,200)
                    };
            parameters[0].Value = UserID;
            parameters[1].Value = Name;
            parameters[2].Value = AuditStatus;
            parameters[3].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_APPSelectByName", parameters);

            dicList = new List<Dictionary<string, object>>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Name", row["Name"]);
                    dic.Add("MediaID", row["MediaID"]);

                    dicList.Add(dic);
                }
            }
            return (string)parameters[3].Value;
        }

        #endregion 根据APP名称模类查询V1.1.4

        #region 根据项目号查询

        public void QueryADOrderInfoByOrderID(string orderID, out DataTable dtADOrderInfo, out DataTable dtMOI, out DataTable dtSubADInfo)
        {
            dtADOrderInfo = null;
            dtMOI = null;
            dtSubADInfo = null;
            string sqlstr = @"--项目信息
                                SELECT  ADI.OrderID ,
                                        ADI.OrderName,
		                                ADI.Status,
		                                ADI.TotalAmount,
		                                ADI.CreateTime,
		                                ADI.CreateUserID,
		                                CASE
			                                WHEN UD1.TrueName IS NULL THEN US1.Mobile
			                                ELSE UD1.TrueName
		                                END CreatorName,
		                                US1.UserName CreatorUserName,
		                                ADI.CustomerID CustomerIDINT,
		                                UD2.TrueName CustomerName,
		                                US2.UserName CustomerUserName,
		                                (SELECT TOP 1 RejectMsg FROM dbo.ADOrderOperateInfo
		                                WHERE OptType=27002 AND OrderID=ADI.OrderID
		                                ORDER BY CreateTime DESC) RejectMsg
                                FROM    dbo.ADOrderInfo ADI
                                LEFT JOIN dbo.UserDetailInfo UD1 ON ADI.CreateUserID=UD1.UserID
                                LEFT JOIN dbo.UserInfo US1 ON ADI.CreateUserID=US1.UserID
                                LEFT JOIN dbo.UserInfo US2 ON ADI.CustomerID=US2.UserID
                                LEFT JOIN dbo.UserDetailInfo UD2 ON ADI.CustomerID=UD2.UserID
                                WHERE ADI.OrderID=@OrderID

                                --媒体项目
                                --UploadFileName在程序中处理
                                SELECT MOI.MediaType,
                                MOI.Note,
                                MOI.UploadFileURL
                                FROM dbo.MediaOrderInfo MOI
                                WHERE MOI.OrderID=@OrderID

                                --订单
                                SELECT SADI.SubOrderID,
                                SADI.MediaType,
                                SADI.MediaID,
                                CASE SADI.MediaType
	                                WHEN 14001 THEN WECHAT.Name
	                                WHEN 14002 THEN PCAPP.Name
	                                ELSE ''
                                END Name,
                                CASE SADI.MediaType
	                                WHEN 14001 THEN
				                                 (CASE
					                                WHEN UIWECHAT.Source=3001 THEN '赤兔自营'
					                                WHEN (UIWECHAT.Source=3002 AND UIWECHAT.UserName IS NOT NULL) THEN	UIWECHAT.UserName
					                                WHEN (UIWECHAT.Source=3002 AND UDWECHAT.TrueName IS NOT NULL) THEN	UDWECHAT.TrueName
					                                ELSE UIWECHAT.Mobile
				                                  END)
	                                WHEN 14002 THEN
				                                (CASE
					                                WHEN UIPCAPP.Source=3001 THEN '赤兔自营'
					                                WHEN (UIPCAPP.Source=3002 AND UIPCAPP.UserName IS NOT NULL) THEN	UIPCAPP.UserName
					                                WHEN (UIPCAPP.Source=3002 AND UDPCAPP.TrueName IS NOT NULL) THEN	UDPCAPP.TrueName
					                                ELSE UIPCAPP.Mobile
				                                 END)
	                                ELSE ''
                                END MediaOwner,
                                SADI.TotalAmount,
                                SADI.Status,
                                SADI.CreateTime,
                                SADI.CreateUserID
                                FROM dbo.SubADInfo SADI
                                LEFT JOIN dbo.Media_Weixin WECHAT ON WECHAT.MediaID=SADI.MediaID
                                LEFT JOIN dbo.Media_PCAPP PCAPP ON PCAPP.MediaID=SADI.MediaID
                                LEFT JOIN dbo.UserInfo UIWECHAT ON WECHAT.CreateUserID=UIWECHAT.UserID
                                LEFT JOIN dbo.UserInfo UIPCAPP ON PCAPP.CreateUserID=UIPCAPP.UserID
                                LEFT JOIN dbo.UserDetailInfo UDWECHAT ON UIWECHAT.UserID=UDWECHAT.UserID
                                LEFT JOIN dbo.UserDetailInfo UDPCAPP ON UDPCAPP.UserID=UIPCAPP.UserID
                                WHERE SADI.OrderID=@OrderID";

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@OrderID",orderID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            dtADOrderInfo = ds.Tables[0];
            dtMOI = ds.Tables[1];
            dtSubADInfo = ds.Tables[2];
        }

        #endregion 根据项目号查询

        #region 购物车添加到待审项目查询

        public void QueryAEOrderInfo(string orderID, out DataTable dtADOrderInfo, out DataTable dtMOI, out DataTable dtADDetails)
        {
            dtADOrderInfo = null;
            dtMOI = null;
            dtADDetails = null;
            string sqlstr = @"--项目信息
                            SELECT  ADI.OrderID ,
                                    ADI.OrderName,
		                            ADI.Status,
		                            ADI.CustomerID CustomerIDINT
                            FROM    dbo.ADOrderInfo ADI
                            LEFT JOIN dbo.UserDetailInfo UD1 ON ADI.CreateUserID=UD1.UserID
                            LEFT JOIN dbo.UserInfo US1 ON ADI.CreateUserID=US1.UserID
                            LEFT JOIN dbo.UserInfo US2 ON ADI.CustomerID=US2.UserID
                            LEFT JOIN dbo.UserDetailInfo UD2 ON ADI.CustomerID=UD2.UserID
                            WHERE ADI.OrderID=@OrderID

                            --媒体项目
                            --UploadFileName在程序中处理
                            SELECT
                            MOI.MediaType,
                            MOI.Note,
                            MOI.UploadFileURL
                            FROM dbo.MediaOrderInfo MOI
                            WHERE MOI.OrderID=@OrderID

                            --广告位
                            SELECT
                            RecID,
                            MediaType,
                            MediaID,
                            PubDetailID,
                            AdjustPrice,
                            AdjustDiscount,
                            ADLaunchDays,
                            Holidays
                            FROM dbo.ADDetailInfo
                            WHERE OrderID=@OrderID";

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@OrderID",orderID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            dtADOrderInfo = ds.Tables[0];
            dtMOI = ds.Tables[1];
            dtADDetails = ds.Tables[2];
        }

        #endregion 购物车添加到待审项目查询

        #region V1.1.8

        #region 根据个人姓名、公司名称或手机号模糊查询广告主

        /// <summary>
        /// 根据个人姓名、公司名称或手机号模糊查询广告主
        /// </summary>
        /// <param name="AEUserID">AE的userid</param>
        /// <param name="NameOrMobile">模糊查询的姓名、公司名称或手机号</param>
        /// <returns></returns>
        public DataTable p_ADMaster_SelectV1_1_8(int AEUserID, string NameOrMobile, int IsAEAuth)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@AEUserID", SqlDbType.Int, 4),
                    new SqlParameter("@NameOrMobile", SqlDbType.NVarChar, 200),
                    new SqlParameter("@IsAEAuth", SqlDbType.Bit, 2)
                    };
            parameters[0].Value = AEUserID;
            parameters[1].Value = NameOrMobile;
            parameters[2].Value = IsAEAuth;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADMaster_SelectV1_1_8", parameters);
            return ds.Tables[0];
        }

        #endregion 根据个人姓名、公司名称或手机号模糊查询广告主

        #region 根据项目号查询

        public void QueryADOrderInfoByOrderIDV1_1_8(string orderID, out DataTable dtADOrderInfo, out DataTable dtMOI, out DataTable dtSubADInfo)
        {
            dtADOrderInfo = null;
            dtMOI = null;
            dtSubADInfo = null;
            string sqlstr = @"--项目信息
                                SELECT  ADI.OrderID ,
                                        ADI.OrderName,
		                                ADI.Status,
		                                ADI.TotalAmount,
		                                ADI.CostTotal,
		                                ADI.CreateTime,
		                                ADI.CreateUserID,
		                                CASE
			                                WHEN UD1.TrueName IS NULL THEN US1.Mobile
			                                ELSE UD1.TrueName
		                                END CreatorName,
		                                US1.UserName CreatorUserName,
		                                ADI.CustomerID CustomerIDINT,
		                                UD2.TrueName CustomerName,
		                                US2.UserName CustomerUserName,
		                                (SELECT TOP 1 RejectMsg FROM dbo.ADOrderOperateInfo
		                                WHERE OptType=27002 AND OrderID=ADI.OrderID
		                                ORDER BY CreateTime DESC) RejectMsg,
		                                CASE
			                                WHEN ADI.RoleID='SYS001RL00002' THEN 2
			                                ELSE 1
		                                END OrderType
                                FROM    dbo.ADOrderInfo ADI
                                LEFT JOIN dbo.UserDetailInfo UD1 ON ADI.CreateUserID=UD1.UserID
                                LEFT JOIN dbo.UserInfo US1 ON ADI.CreateUserID=US1.UserID
                                LEFT JOIN dbo.UserInfo US2 ON ADI.CustomerID=US2.UserID
                                LEFT JOIN dbo.UserDetailInfo UD2 ON ADI.CustomerID=UD2.UserID
                                WHERE ADI.OrderID=@OrderID

                                --媒体项目
                                --UploadFileName在程序中处理
                                SELECT MOI.MediaType,
                                MOI.Note,
                                MOI.UploadFileURL
                                FROM dbo.MediaOrderInfo MOI
                                WHERE MOI.OrderID=@OrderID

                                --订单
                                SELECT SADI.SubOrderID,
                                SADI.MediaType,
                                SADI.MediaID,
                                CASE SADI.MediaType
	                                WHEN 14001 THEN WECHAT.Name
	                                WHEN 14002 THEN PCAPP.Name
	                                ELSE ''
                                END Name,
                                CASE SADI.MediaType
	                                WHEN 14001 THEN
				                                 (CASE
					                                WHEN UIWECHAT.Source=3001 THEN '赤兔自营'
					                                WHEN (UIWECHAT.Source=3002 AND UIWECHAT.UserName IS NOT NULL) THEN	UIWECHAT.UserName
					                                WHEN (UIWECHAT.Source=3002 AND UDWECHAT.TrueName IS NOT NULL) THEN	UDWECHAT.TrueName
					                                ELSE UIWECHAT.Mobile
				                                  END)
	                                WHEN 14002 THEN
				                                (CASE
					                                WHEN UIPCAPP.Source=3001 THEN '赤兔自营'
					                                WHEN (UIPCAPP.Source=3002 AND UIPCAPP.UserName IS NOT NULL) THEN	UIPCAPP.UserName
					                                WHEN (UIPCAPP.Source=3002 AND UDPCAPP.TrueName IS NOT NULL) THEN	UDPCAPP.TrueName
					                                ELSE UIPCAPP.Mobile
				                                 END)
	                                ELSE ''
                                END MediaOwner,
                                SADI.TotalAmount,
                                SADI.Status,
                                SADI.CreateTime,
                                SADI.CreateUserID
                                FROM dbo.SubADInfo SADI
                                LEFT JOIN dbo.Media_Weixin WECHAT ON WECHAT.MediaID=SADI.MediaID
                                LEFT JOIN dbo.Media_PCAPP PCAPP ON PCAPP.MediaID=SADI.MediaID
                                LEFT JOIN dbo.UserInfo UIWECHAT ON WECHAT.CreateUserID=UIWECHAT.UserID
                                LEFT JOIN dbo.UserInfo UIPCAPP ON PCAPP.CreateUserID=UIPCAPP.UserID
                                LEFT JOIN dbo.UserDetailInfo UDWECHAT ON UIWECHAT.UserID=UDWECHAT.UserID
                                LEFT JOIN dbo.UserDetailInfo UDPCAPP ON UDPCAPP.UserID=UIPCAPP.UserID
                                WHERE SADI.OrderID=@OrderID";

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@OrderID",orderID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            dtADOrderInfo = ds.Tables[0];
            dtMOI = ds.Tables[1];
            dtSubADInfo = ds.Tables[2];
        }

        #endregion 根据项目号查询

        #endregion V1.1.8

        #region V1.1.8第二部分

        #region 根据项目号查询生成二维码

        public List<TwoBarCodeDetailDto> GetTwoBarCodeHistory(string orderID, out GetTwoBarCodeHistoryDto resDto)
        {
            resDto = null;
            string sqlstr = @"--查询二维码生成记录
                            SELECT  TmpADDetail.MediaType ,
                                    TmpADDetail.MediaID ,
                                    WECHAT.Number MediaNumber ,
                                    WECHAT.Name MediaName ,
                                    WECHAT.HeadIconURL ,
                                    TBCH.RecID,
                                    TBCH.URL ,
                                    TBCH.TwoBarUrl
                            FROM    ( SELECT DISTINCT
                                                MediaType ,
                                                MediaID
                                      FROM      dbo.ADDetailInfo
                                      WHERE     OrderID = @OrderID
                                                AND MediaType = 14001
                                    ) TmpADDetail
                                    JOIN dbo.Media_Weixin WECHAT ON WECHAT.MediaID = TmpADDetail.MediaID
                                    LEFT JOIN dbo.TwoBarCodeHistory TBCH ON TBCH.MediaID = TmpADDetail.MediaID
                                                                            AND TBCH.OrderID = @OrderID;
                            SELECT OrderID,OrderName FROM dbo.ADOrderInfo WHERE OrderID=@OrderID";

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@OrderID",orderID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            resDto = DataTableToEntity<GetTwoBarCodeHistoryDto>(ds.Tables[1]);
            return DataTableToList<TwoBarCodeDetailDto>(ds.Tables[0]);
        }

        #endregion 根据项目号查询生成二维码
        #region 直辖市字典
        public Dictionary<int, string> DictDirectCity()
        {
            return new Dictionary<int, string>(){
                {2,"北京" },
                { 24,"上海"},
                {26,"天津" },
                {31,"重庆" }
            };
        }
        #endregion
        #region 根据投放城市、投放日期、已投放媒体帐号查询广告位

        public List<GetPublishDetailForRecommendAddPDDto> GetPublishDetailForRecommendAddPD(string MeidiaNumbers, DateTime launchTime, int provinceID = -2, int cityID = -2)
        {
            string tmp = string.Empty;
            if (MeidiaNumbers == ",")
                MeidiaNumbers = string.Empty;
            if (!string.IsNullOrEmpty(MeidiaNumbers))
            {
                if (MeidiaNumbers.EndsWith(","))
                    MeidiaNumbers = MeidiaNumbers.Substring(0, MeidiaNumbers.Length - 1);

                if (MeidiaNumbers.Contains(","))
                {
                    foreach (var item in MeidiaNumbers.Split(','))
                    {
                        if (!string.IsNullOrEmpty(item))
                            tmp += $"'{item}',";
                    }
                }
                else
                    tmp = $"'{MeidiaNumbers}'";
            }
            if (!string.IsNullOrEmpty(tmp))
            {
                if (tmp.EndsWith(","))
                    tmp = tmp.Substring(0, tmp.Length - 1);
                if (!string.IsNullOrEmpty(tmp))
                    tmp = $"AND WOA.WxNumber NOT IN ({tmp})";
            }

            string tmpCity = $"AND MAMB.CityID = {cityID}";
            if (DictDirectCity().ContainsKey(provinceID))
                tmpCity = string.Empty;
            string sqlstr = string.Format(@"--2>查询广告位
                                                SELECT  PD.RecID PublishDetailID ,
                                                        PD.MediaType ,
                                                        PD.MediaID ,
                                                        WOA_MAMB_MW.MediaName ,
                                                        WOA_MAMB_MW.MediaNumber ,
                                                        WOA_MAMB_MW.HeadIconURL ,
                                                        WOA_MAMB_MW.FansCount ,
                                                        ADPosition1.DictName ADPosition ,
                                                        ADPosition2.DictName CreateType ,
		                                                ADPosition1.DictId ADPositionID ,
                                                        ADPosition2.DictId CreateTypeID ,
                                                        PD.SalePrice ,
                                                        PD.CostReferencePrice ,
                                                        PB.OriginalReferencePrice,
		                                                CI.ChannelID,
		                                                CI.ChannelName
                                                FROM    dbo.Publish_DetailInfo PD
		                                                LEFT JOIN dbo.ChannelCostDetail CCD ON PD.CostDetailID=CCD.DetailID
		                                                LEFT JOIN dbo.ChannelInfo CI ON CI.ChannelID=CCD.ChannelID
                                                        JOIN ( SELECT   WOA.WxNumber MediaNumber ,
                                                                        WOA.FansCount ,
                                                                        MW.MediaID ,
                                                                        MW.Name MediaName ,
                                                                        MW.HeadIconURL
                                                                FROM     dbo.Weixin_OAuth WOA
                                                                        JOIN Media_Area_Mapping_Basic MAMB ON MAMB.BaseMediaID = WOA.RecID
                                                                        JOIN dbo.Media_Weixin MW ON MW.Number = WOA.WxNumber
                                                                                                    AND MW.Status = 0
                                                                                                    AND MW.AuditStatus = 43002
                                                                                                    AND WOA.Status = 0
                                                                        JOIN dbo.UserInfo UIWECHAT ON MW.CreateUserID = UIWECHAT.UserID
                                                                              AND UIWECHAT.Source = 3001
                                                                        LEFT JOIN dbo.Interaction_Weixin IW ON WOA.RecID = IW.WxID
                                                                WHERE    MAMB.ProvinceID = @ProvinceID
                                                                        {1}
                                                                        {0}
                                                                ) WOA_MAMB_MW ON WOA_MAMB_MW.MediaID = PD.MediaID
                                                                                AND PD.MediaType = 14001
                                                        JOIN dbo.Publish_BasicInfo PB ON PB.PubID = PD.PubID
                                                                                            AND PB.Wx_Status = 42011
                                                                                            AND PB.IsDel = 0
                                                        LEFT JOIN ( SELECT  DictId ,
                                                                            DictName
                                                                    FROM    dbo.DictInfo
                                                                    WHERE   DictType = 6
                                                                    ) ADPosition1 ON ADPosition1.DictId = PD.ADPosition1
                                                        LEFT JOIN ( SELECT  DictId ,
                                                                            DictName
                                                                    FROM    dbo.DictInfo
                                                                    WHERE   DictType = 8
                                                                    ) ADPosition2 ON ADPosition2.DictId = PD.ADPosition2
                                                WHERE   @LaunchTime BETWEEN PB.BeginTime AND PB.EndTime
                                                ORDER BY WOA_MAMB_MW.FansCount DESC ,
                                                        PD.SalePrice ASC", tmp, tmpCity);

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@LaunchTime",launchTime.Date),
                new SqlParameter("@ProvinceID",provinceID),
                new SqlParameter("@CityID",cityID)
                //,
                //new SqlParameter("@MeidiaNumbers",tmp)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            return DataTableToList<GetPublishDetailForRecommendAddPDDto>(ds.Tables[0]);
        }

        #endregion 根据投放城市、投放日期、已投放媒体帐号查询广告位

        #region 智投推荐相关

        /// <summary>
        /// 查询智投推荐广告位ID、成本价格
        /// </summary>
        /// <param name="launchTime">投放日期</param>
        /// <param name="provinceID">省份ID</param>
        /// <param name="cityID">城市ID</param>
        /// <param name="mediaCount">投放媒体数</param>
        /// <returns></returns>
        public List<PubDetailCostPriceDto> GetPubDetailCostPrice(DateTime launchTime, string orderRemark, int provinceID = -2, int cityID = -2, int mediaCount = 3, bool originContain = true)
        {
            string tmp = $"AND MAMB.CityID = {cityID}";
            if (DictDirectCity().ContainsKey(provinceID))
                tmp = string.Empty;
            string sqlOriginContain = string.Empty;
            if (!originContain)//不含仅原创
                sqlOriginContain = "AND PD.ADPosition2 <> 8002";
            string sqlstr = string.Format(@"--查询智投推荐广告位ID、成本价格
                            SELECT  PD.RecID PublishDetailID ,
                                    --WOA_MAMB.MediaNumber ,
                                    WOA_MAMB.RecID BaseMediaID ,
                                    --MW.MediaID ,
                                    PD.CostReferencePrice
                            FROM    dbo.Publish_DetailInfo PD
                                    JOIN dbo.Publish_BasicInfo PB ON PB.PubID = PD.PubID
                                                                     AND PB.Wx_Status = 42011
                                                                     AND PB.IsDel = 0
                                    JOIN dbo.Media_Weixin MW ON MW.MediaID = PD.MediaID
                                                                AND PD.MediaType = 14001
                                                                AND MW.Status = 0
                                                                AND MW.AuditStatus = 43002
                                    JOIN dbo.UserInfo UIWECHAT ON MW.CreateUserID = UIWECHAT.UserID
                                          AND UIWECHAT.Source = 3001
                                    JOIN ( SELECT --TOP {0}
                                                    WOA.RecID ,
                                                    WOA.WxNumber MediaNumber ,
                                                    WOA.FansCount
                                           FROM     dbo.Weixin_OAuth WOA
                                                    JOIN Media_Area_Mapping_Basic MAMB ON MAMB.BaseMediaID = WOA.RecID
                                                                                          AND WOA.Status = 0
                                                    LEFT JOIN dbo.Interaction_Weixin IW ON WOA.RecID = IW.WxID
                                           WHERE    MAMB.ProvinceID = @ProvinceID
                                                    {2}
                                                    AND EXISTS ( SELECT 1
                                                                 FROM   dbo.Media_Remark_Basic
                                                                 WHERE  EnumType = 45002
                                                                        AND RelationID = WOA.RecID
                                                                        AND RemarkID IN ({1}) )
                                           --ORDER BY WOA.FansCount DESC
                                         ) WOA_MAMB ON MW.Number = WOA_MAMB.MediaNumber
                            WHERE 1=1 
                                  {3}
                                  AND  @LaunchTime BETWEEN PB.BeginTime AND PB.EndTime", mediaCount, orderRemark, tmp, sqlOriginContain);

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@LaunchTime",launchTime.Date),
                new SqlParameter("@ProvinceID",provinceID),
                new SqlParameter("@CityID",cityID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            return DataTableToList<PubDetailCostPriceDto>(ds.Tables[0]);
        }

        public List<ResponseIntelligenceRecommendDetailDto> GetRecommendDetail(string PublishDetailIDs, int provinceID = -2, int cityID = -2, bool originContain = true)
        {
            if (!string.IsNullOrEmpty(PublishDetailIDs))
                PublishDetailIDs = $"AND PD.RecID IN ({PublishDetailIDs})";
            string tmp = $"AND MAMB.CityID = {cityID}";
            if (DictDirectCity().ContainsKey(provinceID))
                tmp = string.Empty;
            string sqlOriginContain = string.Empty;
            if (!originContain)//不含仅原创
                sqlOriginContain = "AND PD.ADPosition2 <> 8002";
            string sqlstr = string.Format(@"--智投推荐查询结果
                            SELECT  PD.RecID PublishDetailID ,
                                    PD.MediaType ,
                                    PD.MediaID ,
                                    WOA_MAMB_MW.MediaName ,
                                    WOA_MAMB_MW.MediaNumber ,
                                    WOA_MAMB_MW.HeadIconURL ,
                                    WOA_MAMB_MW.FansCount ,
                                    ADPosition1.DictName ADPosition ,
                                    ADPosition2.DictName CreateType ,
                                    PD.SalePrice ,
                                    PD.CostReferencePrice ,
                                    PB.OriginalReferencePrice ,
                                    WOA_MAMB_MW.ProvinceID ,
                                    WOA_MAMB_MW.CityID ,
                                    WOA_MAMB_MW.ProvinceName ,
                                    WOA_MAMB_MW.CityName ,
                                    PB.BeginTime ,
                                    PB.EndTime
                            FROM    dbo.Publish_DetailInfo PD
                                    JOIN ( SELECT   WOA.WxNumber MediaNumber ,
                                                    WOA.FansCount ,
                                                    MW.MediaID ,
                                                    MW.Name MediaName ,
                                                    MW.HeadIconURL ,
                                                    MAMB.ProvinceID ,
                                                    MAMB.CityID ,
                                                    ( SELECT TOP 1
                                                                AreaName
                                                      FROM      dbo.AreaInfo
                                                      WHERE     AreaID = MAMB.ProvinceID
                                                    ) ProvinceName ,
                                                    ( SELECT TOP 1
                                                                AreaName
                                                      FROM      dbo.AreaInfo
                                                      WHERE     AreaID = MAMB.CityID
                                                    ) CityName
                                           FROM     dbo.Weixin_OAuth WOA
                                                    JOIN Media_Area_Mapping_Basic MAMB ON MAMB.BaseMediaID = WOA.RecID
                                                    JOIN dbo.Media_Weixin MW ON MW.Number = WOA.WxNumber
                                           WHERE    1=1 AND MAMB.ProvinceID = @ProvinceID
                                                    {1}
                                         ) WOA_MAMB_MW ON WOA_MAMB_MW.MediaID = PD.MediaID
                                                          AND PD.MediaType = 14001
                                    JOIN dbo.Publish_BasicInfo PB ON PB.PubID = PD.PubID
                                                                     AND PB.Wx_Status = 42011
                                                                     AND PB.IsDel = 0
                                    LEFT JOIN ( SELECT  DictId ,
                                                        DictName
                                                FROM    dbo.DictInfo
                                                WHERE   DictType = 6
                                              ) ADPosition1 ON ADPosition1.DictId = PD.ADPosition1
                                    LEFT JOIN ( SELECT  DictId ,
                                                        DictName
                                                FROM    dbo.DictInfo
                                                WHERE   DictType = 8
                                              ) ADPosition2 ON ADPosition2.DictId = PD.ADPosition2
                            WHERE   1 = 1
                                    {0}
                                    {2}
                            ORDER BY WOA_MAMB_MW.FansCount DESC ,
                                    PD.SalePrice ASC", PublishDetailIDs, tmp, sqlOriginContain);
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@ProvinceID",provinceID),
                new SqlParameter("@CityID",cityID)
                //,
                //new SqlParameter("@MediaCount",mediaCount)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            return DataTableToList<ResponseIntelligenceRecommendDetailDto>(ds.Tables[0]);
        }

        #endregion

        #region 提交项目相关

        public void UpdateRecommend_ADDADOrderNote(RequestIntelligenceADOrderDto reqDto)
        {
            string sqlstr = string.Format(@"--智能审核添写需求更新项目
                            UPDATE  dbo.ADOrderInfo
                            SET     CustomerID = @CustomerID ,
                                    CRMCustomerID = @CRMCustomerID ,
                                    CustomerText = @CustomerText ,
                                    OrderName = @OrderName ,
                                    Note = @Note ,
                                    Status = @Status ,
                                    UploadFileURL = @UploadFileURL
                            WHERE   OrderID = @OrderID
                            UPDATE  dbo.ADOrderInfoExtend
                            SET     MarketingPolices = @MarketingPolices ,
                                    MarketingUrl = @MarketingUrl
                            WHERE   OrderID = @OrderID
                            UPDATE  dbo.MediaOrderInfo
                            SET     Note = @Note
                            WHERE   OrderID = @OrderID");
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@CustomerID",reqDto.CustomerIDINT),
                new SqlParameter("@CRMCustomerID",reqDto.CRMCustomerID),
                new SqlParameter("@CustomerText",reqDto.CustomerText),
                new SqlParameter("@OrderName",reqDto.OrderName),
                new SqlParameter("@UploadFileURL",reqDto.UploadFileURL),
                new SqlParameter("@MarketingPolices",reqDto.MarketingPolices),
                new SqlParameter("@MarketingUrl",reqDto.MarketingUrl),
                new SqlParameter("@OrderID",reqDto.OrderID),
                new SqlParameter("@Status",reqDto.Status),
                new SqlParameter("@Note",reqDto.Note)
            };
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
        }

        #region 根据广告位ID查询广告位置渠道ID
        public GetADPostionChannelIDDto GetADPositionChannelIDByPubDetailID(int publishDetailID)
        {
            string sqlstr = @"SELECT  PB.ADPosition1 ,
                                    PB.ADPosition2 ,
                                    PB.ADPosition3 ,
                                    CCD.ADPosition3 ,
                                    CCD.ChannelID
                            FROM    dbo.Publish_DetailInfo PB
                                    JOIN dbo.ChannelCostDetail CCD ON PB.CostDetailID = CCD.DetailID
                            WHERE   PB.MediaType = 14001
                                    AND PB.RecID = @PublishDetailID";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("PublishDetailID",publishDetailID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            return DataTableToEntity<GetADPostionChannelIDDto>(ds.Tables[0]);
        }
        #endregion

        #region 修改项目

        public void p_ADOrderInfo_UpdateV1_1_8(Entities.ADOrderInfo reqDto)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@OrderID",reqDto.OrderID),
                new SqlParameter("@OrderName",reqDto.OrderName),
                new SqlParameter("@Status",reqDto.Status),
                new SqlParameter("@CustomerID",reqDto.CustomerID),
                new SqlParameter("@UploadFileURL",reqDto.UploadFileURL),
                new SqlParameter("@CRMCustomerID",reqDto.CRMCustomerID),
                new SqlParameter("@CustomerText",reqDto.CustomerText),
                new SqlParameter("@MarketingPolices",reqDto.MarketingPolices),
                new SqlParameter("@LaunchTime",reqDto.LaunchTime),
                new SqlParameter("@OrderRemark",reqDto.OrderRemark),
                new SqlParameter("@MasterID",reqDto.MasterID),
                new SqlParameter("@BrandID",reqDto.BrandID),
                new SqlParameter("@SerialID",reqDto.SerialID),
                new SqlParameter("@MasterName",reqDto.MasterName),
                new SqlParameter("@BrandName",reqDto.BrandName),
                new SqlParameter("@SerialName",reqDto.SerialName),
                new SqlParameter("@JKEntrance",reqDto.JKEntrance),
                new SqlParameter("@Note",reqDto.Note)
            };
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderInfo_UpdateV1_1_8", parameters);
        }

        #endregion 修改项目

        #region 根据项目号返回model

        public RequestIntelligenceADOrderDto p_ADOrderInfo_SelectByOrderIDV1_1_8(string orderID)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@OrderID",orderID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderInfo_SelectByOrderIDV1_1_8", parameters);
            return DataTableToEntity<RequestIntelligenceADOrderDto>(ds.Tables[0]);
        }

        #endregion 根据项目号返回model

        #region 根据项目号删除项目

        /// <summary>
        /// 根据项目号删除项目订单广告位排期信息
        /// </summary>
        /// <param name="OrderID">项目号</param>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="ISDeleteADOrder">是否删除项目</param>
        /// <returns></returns>
        public string p_ADOrderAllInfo_DeleteV1_1_8(string OrderID, int MediaType, bool ISDeleteADOrder)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.VarChar,20),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@ISDeleteADOrder",SqlDbType.Bit),
                    new SqlParameter("@Msg",SqlDbType.VarChar,50)
                                        };
            parameters[0].Value = OrderID;
            parameters[1].Value = MediaType;
            if (ISDeleteADOrder)
            {
                parameters[2].Value = 1;
            }
            else
            {
                parameters[2].Value = 0;
            }
            parameters[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderAllInfo_DeleteV1_1_8", parameters);

            return (string)parameters[3].Value;
        }

        #endregion 根据项目号删除项目

        #region 更新项目金额

        public void UpdateADOrderAmmount(string orderID, decimal totalAmmount, decimal costTotal, decimal budgetTotal)
        {
            string sqlstr = @"--更新项目金额
                            UPDATE  dbo.ADOrderInfo
                            SET     TotalAmount = @TotalAmount ,
                                    CostTotal = @CostTotal
                            WHERE   OrderID = @OrderID
                            UPDATE  dbo.ADOrderInfoExtend
                            SET     BudgetTotal = @BudgetTotal
                            WHERE   OrderID = @OrderID";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@TotalAmount",totalAmmount),
                new SqlParameter("@CostTotal",costTotal),
                new SqlParameter("@BudgetTotal",budgetTotal),
                new SqlParameter("@OrderID",orderID)
            };
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
        }

        #endregion 更新项目金额

        #region 项目城市表插入

        public int p_ADOrderCityExtend_InsertV1_1_8(Entities.ADOrderCityExtend model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@CityExtendID",SqlDbType.Int,4),
                    new SqlParameter("@OrderID", model.OrderID),
                    new SqlParameter("@ProvinceID",model.ProvinceID),
                    new SqlParameter("@CityID",model.CityID),
                    new SqlParameter("@Budget",model.Budget),
                    new SqlParameter("@MediaCount",model.MediaCount),
                    new SqlParameter("@OriginContain",model.OriginContain),
                    new SqlParameter("@CreateUserID", model.CreateUserID)
                                        };
            parameters[0].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderCityExtend_InsertV1_1_8", parameters);
            return (int)parameters[0].Value;
        }

        #endregion 项目城市表插入

        #region 订单广告位插入

        public string p_ADDetailInfoInsert_IntelligenceV1_1_8(Entities.ADDetailInfo model, out decimal adjustPrice, out decimal finalCostPrice)
        {
            adjustPrice = 0;
            finalCostPrice = 0;
            SqlParameter[] parameters = {
                    new SqlParameter("@SubOrderID", SqlDbType.VarChar,20),
                    new SqlParameter("@AdjustPriceOut",SqlDbType.Decimal),
                    new SqlParameter("@FinalCostPriceOut",SqlDbType.Decimal),
                    new SqlParameter("@FinalCostPrice",model.FinalCostPrice),
                    new SqlParameter("@CostPrice",model.CostPrice),
                    new SqlParameter("@OrderID", model.OrderID),
                    new SqlParameter("@CityExtendID",model.CityExtendID),
                    new SqlParameter("@PublishDetailID",model.PubDetailID),
                    new SqlParameter("@MediaType",model.MediaType),
                    new SqlParameter("@MediaID",model.MediaID),
                    new SqlParameter("@EnableOriginPrice", model.EnableOriginPrice),
                    new SqlParameter("@ChannelID",model.ChannelID),
                    new SqlParameter("@LaunchTime",model.LaunchTime),
                    new SqlParameter("@CreateUserID",model.CreateUserID),
                    new SqlParameter("@Status", model.Status),
                    new SqlParameter("@AdjustPrice",model.AdjustPrice),
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Direction = ParameterDirection.Output;
            parameters[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADDetailInfoInsert_IntelligenceV1_1_8", parameters);
            adjustPrice = (decimal)parameters[1].Value;
            finalCostPrice = (decimal)parameters[2].Value;
            return (string)parameters[0].Value;
        }

        #endregion 订单广告位插入

        #region Insert

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string InsertV1_1_8(Entities.ADOrderInfo model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID", SqlDbType.VarChar,20),
                    new SqlParameter("@OrderName",model.OrderName),
                    new SqlParameter("@Status",model.Status),
                    new SqlParameter("@CustomerID",model.CustomerID),
                    new SqlParameter("@MarketingPolices",model.MarketingPolices),
                    new SqlParameter("@UploadFileURL",model.UploadFileURL),
                    new SqlParameter("@LaunchTime", model.LaunchTime),
                    new SqlParameter("@CRMCustomerID",model.CRMCustomerID),
                    new SqlParameter("@CustomerText",model.CustomerText),
                    new SqlParameter("@BudgetTotal",model.BudgetTotal),
                    new SqlParameter("@OrderRemark",model.OrderRemark),
                    new SqlParameter("@MasterID", model.MasterID),
                    new SqlParameter("@BrandID",model.BrandID),
                    new SqlParameter("@SerialID",model.SerialID),
                    new SqlParameter("@MasterName",model.MasterName),
                    new SqlParameter("@BrandName",model.BrandName),
                    new SqlParameter("@SerialName",model.SerialName),
                    new SqlParameter("@JKEntrance", model.JKEntrance),
                    new SqlParameter("@Note",model.Note),
                    new SqlParameter("@MediaType",model.MediaType),
                    new SqlParameter("@TotalAmount",model.TotalAmount),
                    new SqlParameter("@CreateUserID",model.CreateUserID),
                    new SqlParameter("@OrderType", model.OrderType),
                    new SqlParameter("@CrmNum",model.CrmNum),
                    new SqlParameter("@FinalReportURL",model.FinalReportURL),
                    new SqlParameter("@CostTotal",model.CostTotal),
                    new SqlParameter("@MarketingUrl",model.MarketingUrl)
                                        };
            parameters[0].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADOrderInfo_InsertV1_1_8", parameters);
            return (string)parameters[0].Value;
        }

        #endregion Insert

        #endregion

        #region 查看智投项目相关

        #region 根据项目号查项目信息

        public void IntelligenceADOrderInfoQuery_ByOrderID(string orderID, out ResponseIntelligenceADOrderDto resADOrderInfo, out List<ResponseIntelligenceADOrderAreaInfoDto> listAreainfo)
        {
            resADOrderInfo = null;
            listAreainfo = null;
            string sqlstr = @"--项目信息
                            SELECT  ADI.OrderID ,
                                    ADI.OrderName,
                                    ADI.Note,
		                            ADI.Status,
		                            ADI.TotalAmount,
		                            ADI.CreateTime,
		                            ADI.CreateUserID,
		                            CASE
			                            WHEN UD1.TrueName IS NULL THEN US1.Mobile
			                            ELSE UD1.TrueName
		                            END CreatorName,
		                            US1.UserName CreatorUserName,
		                            ADI.CRMCustomerID,
		                            ADI.CustomerText,
		                            ADI.CustomerID CustomerIDINT,
		                            --UD2.TrueName CustomerName,
		                            VUS.SysName CustomerName,
		                            US2.UserName CustomerUserName,
		                            (SELECT TOP 1 RejectMsg FROM dbo.ADOrderOperateInfo
		                            WHERE OptType=27002 AND OrderID=ADI.OrderID
		                            ORDER BY CreateTime DESC) RejectMsg,
		                            ADI.OrderType,
                                    ADI.UploadFileURL,
		                            ADE.MarketingPolices,
		                            ADE.MarketingUrl,
		                            ADE.OrderRemark,
		                            ADE.MasterID,
		                            ADE.BrandID,
		                            ADE.SerialID,
		                            ADE.MasterBrand MasterName,
		                            ADE.CarBrand BrandName,
		                            ADE.CarSerial SerialName,
		                            ADE.LaunchTime,
		                            ADE.JKEntrance,
		                            ADE.BudgetTotal,
		                            ADI.CostTotal
                            FROM    dbo.ADOrderInfo ADI
                            INNER JOIN dbo.ADOrderInfoExtend ADE ON ADE.OrderID = ADI.OrderID
                            LEFT JOIN dbo.UserDetailInfo UD1 ON ADI.CreateUserID=UD1.UserID
                            LEFT JOIN dbo.UserInfo US1 ON ADI.CreateUserID=US1.UserID
                            LEFT JOIN dbo.UserInfo US2 ON ADI.CustomerID=US2.UserID
                            LEFT JOIN dbo.UserDetailInfo UD2 ON ADI.CustomerID=UD2.UserID
                            LEFT JOIN dbo.v_UserInfo VUS ON VUS.UserID=ADI.CustomerID
                            WHERE ADI.OrderID=@OrderID

                            --智投项目城市表
                            SELECT  CITY.RecID CityExtendID,
                                    CITY.ProvinceID ,
                                    AI1.AreaName ProvinceName,
		                            CITY.CityID,
		                            AI2.AreaName CityName,
		                            CITY.Budget,
		                            CITY.MediaCount,
		                            CITY.OriginContain
                            FROM    dbo.ADOrderCityExtend CITY
                                    LEFT JOIN dbo.AreaInfo AI1 ON CITY.ProvinceID = AI1.AreaID
                                    LEFT JOIN dbo.AreaInfo AI2 ON CITY.CityID = AI2.AreaID
		                            WHERE CITY.OrderID=@OrderID";

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@OrderID",orderID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            resADOrderInfo = DataTableToEntity<ResponseIntelligenceADOrderDto>(ds.Tables[0]);
            listAreainfo = DataTableToList<ResponseIntelligenceADOrderAreaInfoDto>(ds.Tables[1]);
        }

        #endregion

        #region 根据项目城市ID查广告位信息

        public void IntelligenceADOrderInfoQuery_ByCityExtendID(int cityExtendID, out List<ResponseIntelligencePublishDetailDto> listPublishDetail)
        {
            listPublishDetail = null;
            string sqlstr = @"--查询城市下广告位
                            SELECT
                            ADI.MediaType,
                            ADI.PubDetailID PublishDetailID,
                            ADI.MediaID,
                            wechat.Name MediaName,
                            wechat.Number MediaNumber,
                            wechat.HeadIconURL,
                            wechat.FansCount,
                            0 AverageReadCount,
                            ADPosition1.DictName ADPosition,
                            ADPosition2.DictName CreateType,
                            ADPosition1.DictId ADPositionID,
                            ADPosition2.DictId CreateTypeID,
                            ADI.OriginalPrice SalePrice,
                            ADI.CostReferencePrice,
                            ADI.CostPrice,
                            ADI.FinalCostPrice,
                            ADI.OriginalReferencePrice,
                            ADI.EnableOriginPrice,
                            ADI.ADLaunchDays,
                            CASE
	                            WHEN wechat.Status = -1 OR pub.IsDel=-1 THEN -1
	                            WHEN GETDATE() BETWEEN pub.BeginTime AND pub.EndTime THEN 0
	                            ELSE 1
                            END expired,
                            pub.Wx_Status PublishStatus,
                            wechat.Status MediaStatus,
                            ADI.AdjustPrice,
                            SADI.ChannelID,
                            CHA.ChannelName,
                            (SELECT TOP 1 BeginData FROM dbo.ADScheduleInfo WHERE ADDetailID=ADI.RecID) AS LaunchTime,
                            CASE
	                            WHEN pub.Wx_Status=42912
	                            THEN (SELECT COUNT(1)
				                            FROM dbo.Publish_BasicInfo PB
				                            WHERE PB.MediaID=wechat.MediaID AND PB.PubID <> pub.PubID
			                            )
	                            ELSE 0
                            END HasOtherPublish,
                            ADI.CreateTime,
                            ADI.CreateUserID
                            FROM dbo.ADDetailInfo ADI
                            INNER JOIN dbo.SubADInfo SADI ON ADI.SubOrderID=SADI.SubOrderID
                            LEFT JOIN dbo.ChannelInfo CHA ON CHA.ChannelID=SADI.ChannelID
                            LEFT JOIN dbo.Publish_DetailInfo pubdetail ON ADI.PubDetailID=pubdetail.RecID
                            LEFT JOIN dbo.Media_Weixin wechat ON pubdetail.MediaID=wechat.MediaID
                            LEFT JOIN dbo.UserInfo ui ON wechat.CreateUserID=ui.UserID
                            LEFT JOIN dbo.UserDetailInfo ud ON ui.UserID=ud.UserID
                            LEFT JOIN dbo.Publish_BasicInfo pub ON pub.PubID=pubdetail.PubID
                            LEFT JOIN (SELECT DictId,DictName FROM dbo.DictInfo WHERE DictType=6) ADPosition1 ON ADPosition1.DictId=pubdetail.ADPosition1
                            LEFT JOIN (SELECT DictId,DictName FROM dbo.DictInfo WHERE DictType=8) ADPosition2 ON ADPosition2.DictId=pubdetail.ADPosition2
                            WHERE ADI.MediaType=14001
                            AND SADI.CityExtendID=@CityExtendID
                            ORDER BY ADI.CreateTime DESC";

            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@CityExtendID",cityExtendID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            listPublishDetail = DataTableToList<ResponseIntelligencePublishDetailDto>(ds.Tables[0]);
        }

        #endregion

        #endregion

        #endregion

        #region 1.1.8

        /// <summary>
        ///  修改CRM编号
        /// </summary>
        /// <param name="OrderNum">订单号</param>
        /// <param name="CrmNum">CRM编号</param>
        /// <returns></returns>
        public int UpdateCrmNum(string OrderNum, string CrmNum)
        {
            string strSql = "UPDATE dbo.ADOrderInfo SET CrmNum=@CrmNum  WHERE OrderID=@OrderNum";
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderNum",SqlDbType.VarChar,20),
                    new SqlParameter("@CrmNum", SqlDbType.VarChar,20)
                    };
            parameters[0].Value = OrderNum;
            parameters[1].Value = CrmNum;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
        }

        /// <summary>
        /// zlb 2017-07-25
        /// 上传发文地址
        /// </summary>
        /// <param name="SubOrderID">子订单号</param>
        /// <param name="PostingAddress">发文地址</param>
        /// <returns></returns>
        public int UpdatePostingAddressBySubID(string SubOrderID, string PostingAddress)
        {
            string strSql = "UPDATE dbo.SubADInfo SET PostingAddress=@PostingAddress  WHERE SubOrderID=@SubOrderID";
            SqlParameter[] parameters = {
                    new SqlParameter("@SubOrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@PostingAddress", SqlDbType.VarChar,200)
                    };
            parameters[0].Value = SubOrderID;
            parameters[1].Value = PostingAddress;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
        }

        /// <summary>
        /// zlb 2017-07-26
        /// 查询订单的反馈图片路径集合
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <returns></returns>
        public DataSet SelectFeedBackFileInfoByOrderID(string OrderID)
        {
            string strSql = string.Format("SELECT top 1 OrderName,FinalReportURL FROM  dbo.ADOrderInfo WHERE OrderID='{0}' AND Status={1};", SqlFilter(OrderID), (int)EnumOrderStatus.OrderFinished);

            strSql += string.Format(@"SELECT MW.Name,CONVERT(varchar(100), AI.BeginData, 23) as BeginData,OFD.ReadCount,OFD.GoodPointCount FROM  ADOrderInfo  AD INNER JOIN dbo.SubADInfo SA  ON AD.OrderID=SA.OrderID
                                    INNER JOIN  ADScheduleInfo AI ON SA.SubOrderID=AI.SubOrderID
                                    INNER JOIN dbo.Media_Weixin MW ON SA.MediaID=MW.MediaID
                                    INNER JOIN OrderFeedbackData_Weixin OFD  ON OFD.SubOrderCode=SA.SubOrderID
                                    WHERE AD.OrderID='{0}' AND AD.Status={1} AND  SA.Status={1} AND SA.MediaType={2} ", SqlFilter(OrderID), (int)EnumOrderStatus.OrderFinished, (int)EnumMediaType.WeChat);

            strSql += string.Format(@"SELECT  OFDF.UploadFileURL FROM dbo.ADOrderInfo  AD INNER JOIN dbo.SubADInfo SA ON AD.OrderID=SA.OrderID INNER JOIN OrderFeedbackData_Weixin OFD ON OFD.SubOrderCode=SA.SubOrderID INNER JOIN  OrderFeedbackData_File  OFDF ON OFD.RecID = OFDF.FeedBackID
                                       WHERE AD.OrderID= '{0}' AND AD.Status = {1} AND  SA.Status = {1} AND SA.MediaType = {2}  AND OFDF.MediaType = {2}", SqlFilter(OrderID), (int)EnumOrderStatus.OrderFinished, (int)EnumMediaType.WeChat);
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2017-07-28
        ///  增加结案报告
        /// </summary>
        /// <param name="OrderNum">订单号</param>
        /// <param name="CrmNum">CRM编号</param>
        /// <returns></returns>
        public int UpdateFinalReportURL(string OrderID, string ReportURL)
        {
            string strSql = "UPDATE dbo.ADOrderInfo SET FinalReportURL=@ReportURL  WHERE OrderID=@OrderID";
            SqlParameter[] parameters = {
                    new SqlParameter("@OrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@ReportURL", SqlDbType.VarChar,200)
                    };
            parameters[0].Value = OrderID;
            parameters[1].Value = ReportURL;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
        }
        /// <summary>
        /// zlb 2017-07-31
        /// 查看发文地址
        /// </summary>
        /// <param name="SubOrderID"></param>
        /// <returns></returns>
        public string SelectPostingAddress(string SubOrderID)
        {
            string strSql = "SELECT PostingAddress FROM  dbo.SubADInfo WHERE SubOrderID=@SubOrderID";
            SqlParameter[] parameters = {
                    new SqlParameter("@SubOrderID",SqlDbType.VarChar,20),
                    };
            parameters[0].Value = SubOrderID;
            object obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
            return obj == null ? "" : obj.ToString();
        }
        #endregion 1.1.8
    }
}