using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using XYAuto.Utils;
using XYAuto.Utils.Data;
using XYAuto.ITSC.Chitunion2017.Entities;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class CartInfo : DataBase
    {
        public const string P_CartInfo_SELECT = "p_CartInfo_Select";

        #region Instance
        public static readonly CartInfo Instance = new CartInfo();
        #endregion

        #region Select
        /// <summary>
        /// 根据主键值得到一个对象实体
        /// </summary>
        public Entities.CartInfo GetModel(int cartid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 CartID,MediaType,MediaID,PubID,PubDetailID,IsSelected,OrderID,IsSelected,OrderID,CreateTime,CreateUserID ");
            strSql.Append(" FROM dbo.CartInfo ");
            strSql.Append(" WHERE CartID=@CartID");
            SqlParameter[] parameters = {
                    new SqlParameter("@CartID", SqlDbType.Int,4)
            };
            parameters[0].Value = cartid;

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
        public Entities.CartInfo DataRowToModel(DataRow row)
        {
            Entities.CartInfo model = new Entities.CartInfo();
            if (row != null)
            {
                if (row["CartID"] != null && row["CartID"].ToString() != "")
                {
                    model.CartID = int.Parse(row["CartID"].ToString());
                }
                if (row["OrderID"] != null && row["OrderID"].ToString() != "")
                {
                    model.OrderID = row["OrderID"].ToString();
                }

                if (row["MediaType"] != null && row["MediaType"].ToString() != "")
                {
                    model.MediaType = int.Parse(row["MediaType"].ToString());
                }

                if (row["MediaID"] != null && row["MediaID"].ToString() != "")
                {
                    model.MediaID = int.Parse(row["MediaID"].ToString());
                }
                if (row["PubID"] != null && row["PubID"].ToString() != "")
                {
                    model.PubID = int.Parse(row["PubID"].ToString());
                }
                if (row["PubDetailID"] != null && row["PubDetailID"].ToString() != "")
                {
                    model.PubDetailID = int.Parse(row["PubDetailID"].ToString());
                }

                if (row["IsSelected"] != null && row["IsSelected"].ToString() != "")
                {
                    model.IsSelected = byte.Parse(row["IsSelected"].ToString());
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["CreateUserID"] != null && row["CreateUserID"].ToString() != "")
                {
                    model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
                }


            }
            return model;
        }

        #region 得到一个对象实体
        public Entities.CartInfo GetCartInfo(int cartid)
        {
            QueryCartInfo query = new QueryCartInfo();
            query.CartID = cartid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetCartInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleCartInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.CartInfo LoadSingleCartInfo(DataRow row)
        {
            return DataRowToModel(row);
        }
        #endregion

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
        public DataTable GetCartInfo(QueryCartInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.OrderID = '" + query.OrderID + "'";
            }
            if (query.MediaType != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.MediaType = " + query.MediaType;
            }
            if (query.MediaID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.MediaID = " + query.MediaID;
            }
            if (query.PubID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.PubID = " + query.PubID;
            }
            if (query.PubDetailID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.PubDetailID = " + query.PubDetailID;
            }
            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " AND a.CreateUserID = " + query.CreateUserID;
            }

            //DateTime beginTime;
            //if (query.BeginCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.BeginCreateTime, out beginTime))
            //{
            //    where += " and a.CreateTime>='" + beginTime + "'";
            //}
            //DateTime endTime;
            //if (query.EndCreateTime != Constant.STRING_INVALID_VALUE && DateTime.TryParse(query.EndCreateTime, out endTime))
            //{
            //    where += " and a.CreateTime<'" + endTime.AddDays(1) + "'";
            //}

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

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartInfo_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }

        #endregion
        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Entities.CartInfo model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@CartID", SqlDbType.Int,4),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@MediaID", SqlDbType.Int,4),
                    new SqlParameter("@PubID",SqlDbType.Int,4),
                    new SqlParameter("@PubDetailID",SqlDbType.Int,4),
                    new SqlParameter("@IsSelected",SqlDbType.Bit,2),
                    new SqlParameter("@OrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.MediaType;
            parameters[2].Value = model.MediaID;
            parameters[3].Value = model.PubID;
            parameters[4].Value = model.PubDetailID;
            parameters[5].Value = model.IsSelected;
            parameters[6].Value = model.OrderID;
            parameters[7].Value = model.CreateUserID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartInfo_Insert", parameters);
            return (int)parameters[0].Value;
        }
        #endregion
        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Entities.CartInfo model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@CartID", SqlDbType.Int,4),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@MediaID", SqlDbType.Int,4),
                    new SqlParameter("@PubID",SqlDbType.Int,4),
                    new SqlParameter("@PubDetailID",SqlDbType.Int,4),
                    new SqlParameter("@IsSelected",SqlDbType.Bit,2),
                    new SqlParameter("@OrderID",SqlDbType.VarChar,20)
                                        };
            parameters[0].Value = model.CartID;
            parameters[1].Value = model.MediaType;
            parameters[2].Value = model.MediaID;
            parameters[3].Value = model.PubID;
            parameters[4].Value = model.PubDetailID;
            parameters[5].Value = model.IsSelected;
            parameters[6].Value = model.OrderID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartInfo_Update", parameters);
            return (int)parameters[0].Value;
        }
        #endregion
        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int CartID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@CartID", SqlDbType.Int,4)};
            parameters[0].Value = CartID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartInfo_Delete", parameters);
        }
        #endregion

        #region 根据媒体类型、创建人、广告位查看记录是否存在
        public bool IsExistsDetailID(int mediaType, int userID, int detailID)
        {
            bool isOK = false;
            string sqlstr = string.Format("SELECT COUNT(1) FROM dbo.CartInfo WHERE MediaType={0} AND CreateUserID={1} AND PubDetailID={2}", mediaType, userID, detailID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                {
                    isOK = true;
                }
            }

            return isOK;
        }
        #endregion

        #region 根据媒体类型、创建人、媒体ID查看记录是否存在
        public bool IsExistsMediaID(int mediaType, int userID, int mediaID)
        {
            bool isOK = false;
            string sqlstr = string.Format("SELECT COUNT(1) FROM dbo.CartInfo WHERE MediaType={0} AND CreateUserID={1} AND MediaID={2}", mediaType, userID, mediaID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                {
                    isOK = true;
                }
            }

            return isOK;
        }
        #endregion

        #region 根据媒体类型、创建人、广告位ID删除记录
        /// <summary>
        /// 根据媒体类型、创建人、广告位ID删除记录
        /// </summary>
        /// <param name="mediatype">媒体类型</param>
        /// <param name="userid">当前登录人ID</param>
        /// <param name="ids">广告位ID拼接字符串</param>
        /// <returns></returns>
        public int Delete_APPMedia(int mediatype, int userid, string ids)
        {
            string sqlstr = string.Format("DELETE FROM dbo.CartInfo WHERE CreateUserID={0} AND MediaType={1} AND PubDetailID IN({2})", userid, mediatype, ids);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
        }
        #endregion

        #region 根据媒体类型、创建人、媒体ID删除记录
        /// <summary>
        /// 根据媒体类型、创建人、媒体ID删除记录
        /// </summary>
        /// <param name="mediatype">媒体类型</param>
        /// <param name="userid">当前登录人ID</param>
        /// <param name="ids">媒体ID拼接字符串</param>
        /// <returns></returns>
        public int Delete_SelfMedia(int mediatype, int userid, string ids)
        {
            string sqlstr = string.Format("DELETE FROM dbo.CartInfo WHERE CreateUserID={0} AND MediaType={1} AND MediaID IN({2})", userid, mediatype, ids);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
        }
        #endregion

        #region 根据媒体类型、创建人清空购物车
        /// <summary>
        /// 根据媒体类型、创建人清空购物车
        /// </summary>
        /// <param name="mediatype">媒体类型</param>
        /// <param name="userid">创建人</param>
        /// <returns></returns>
        public int ClearAll_CartInfo(int mediatype, int userid)
        {
            string sqlstr = string.Format("DELETE FROM dbo.CartInfo WHERE CreateUserID={0} AND MediaType={1}", userid, mediatype);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
        }
        #endregion

        #region 自媒体更新广告位
        /// <summary>
        /// 自媒体更新广告位
        /// </summary>
        /// <param name="pubdetailid">广告位ID</param>
        /// <param name="mediaid">媒体ID</param>
        /// <param name="userid">创建人</param>
        /// <returns></returns>
        public int UpdatePubDetailID_CartInfo(int pubdetailid, int mediaid, int userid)
        {
            string sqlstr = string.Format("UPDATE dbo.CartInfo SET PubDetailID={0} WHERE MediaID={1} AND CreateUserID={2}", pubdetailid, mediaid, userid);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
        }
        #endregion

        #region 根据媒体类型、创建人清空购物车(未选择的不清)
        /// <summary>
        /// 根据媒体类型、创建人清空购物车(未选择的不清)
        /// </summary>
        /// <param name="mediatype">媒体类型</param>
        /// <param name="userid">创建人</param>
        /// <param name="isAll">是否全清</param>
        /// <returns></returns>
        public int ClearAll_CartInfo(int mediatype, int userid, string ids)
        {
            string sqlstr = "";
            if (mediatype == 14002)//APP
            {
                sqlstr = string.Format("DELETE FROM dbo.CartInfo WHERE CreateUserID={0} AND MediaType={1} AND PubDetailID IN({2})", userid, mediatype, ids);
            }
            else
            {
                sqlstr = string.Format("DELETE FROM dbo.CartInfo WHERE CreateUserID={0} AND MediaType={1} AND MediaID IN({2})", userid, mediatype, ids);
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
        }
        #endregion

        #region 根据媒体类型、创建人获取购物车信息
        /// <summary>
        /// 根据媒体类型、创建人获取购物车信息
        /// </summary>
        /// <param name="mediatype">媒体类型</param>
        /// <param name="userid">创建人UserID</param>
        /// <returns></returns>
        public DataTable GetCartInfo_MediaTypeUserID(int mediatype, int userid)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType", SqlDbType.Int, 4),
                    new SqlParameter("@CreateUserID", SqlDbType.Int, 4)
                    };
            parameters[0].Value = mediatype;
            parameters[1].Value = userid;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADCartInfoDetail_Select", parameters);
            return ds.Tables[0];
        }
        #endregion

        #region 添加购物V1.1 Insert                
        public string InsertV1_1(Entities.CartInfo model, out int CartID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Msg", SqlDbType.VarChar,200),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@MediaID", SqlDbType.Int,4),
                    new SqlParameter("@PubID",SqlDbType.Int,4),
                    new SqlParameter("@PubDetailID",SqlDbType.Int,4),
                    new SqlParameter("@IsSelected",SqlDbType.Bit,2),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@CartID",SqlDbType.Int,4),
                    new SqlParameter("@ADSchedule",SqlDbType.DateTime)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.MediaType;
            parameters[2].Value = model.MediaID;
            parameters[3].Value = model.PubID;
            parameters[4].Value = model.PubDetailID;
            parameters[5].Value = model.IsSelected;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Direction = ParameterDirection.Output;
            parameters[8].Value = model.ADSchedule;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartInfo_InsertV1_1", parameters);
            CartID = -2;
            string retval = (string)parameters[0].Value;
            if (string.IsNullOrEmpty(retval))
            {
                CartID = (int)parameters[7].Value;
            }
            return retval;
        }
        #endregion

        #region 删除购物V1.1 Delete                
        public string DeleteV1_1(int currentUserID, string CartIDS)
        {
            string sqlstr = string.Format(@"DELETE  dbo.CartInfo WHERE CartID IN({0})
                       DELETE dbo.CartScheduleInfo WHERE CartID IN({0})", CartIDS);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr).ToString();
        }
        #endregion

        #region 购物车排期(新增修改删除)              
        public string CartScheduleInfo_Oper(int OptType, int CartID, int RecID, DateTime BeginTime, int CreateUserID, out int RecIDNew)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Msg", SqlDbType.VarChar,200),
                    new SqlParameter("@OptType",SqlDbType.Int,4),
                    new SqlParameter("@CartID", SqlDbType.Int,4),
                    new SqlParameter("@RecID",SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@BeginTime",SqlDbType.DateTime),
                    new SqlParameter("@RecIDNew",SqlDbType.Int,4)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = OptType;
            parameters[2].Value = CartID;
            parameters[3].Value = RecID;
            parameters[4].Value = CreateUserID;
            parameters[5].Value = BeginTime;
            parameters[6].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartScheduleInfo_Oper", parameters);

            string retval = (string)parameters[0].Value;
            RecIDNew = -2;
            if (string.IsNullOrEmpty(retval))
            {
                RecIDNew = (int)parameters[6].Value;
            }
            return retval;
        }
        #endregion

        #region WeChat购物车查询V1.1              
        public DataTable ADCartInfoDetail_SelectV1_1(int MediaType, int CreateUserID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4)
                                        };
            parameters[0].Value = MediaType;
            parameters[1].Value = CreateUserID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADCartInfoDetail_SelectV1_1", parameters);
            return ds.Tables[0];

        }
        #endregion

        #region 购物车排期查询V1.1              
        public DataTable ADCartScheduleInfo_Select(int CartID)
        {
            string sqlstr = @"SELECT  RecID ,
                                        BeginTime AS 'BeginData' ,
                                        EndTime AS 'EndData' ,
                                        CASE WHEN EndTime IS NULL THEN 0
                                             WHEN DATEDIFF(DAY, BeginTime, EndTime) < 0 THEN 0
                                             ELSE DATEDIFF(DAY, BeginTime, EndTime) + 1
                                        END AS 'AllDays'
                                FROM    dbo.CartScheduleInfo
                                WHERE   CartID = @CartID";
            SqlParameter[] parameters = {
                    new SqlParameter("@CartID",SqlDbType.Int,4)
                                        };
            parameters[0].Value = CartID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            return ds.Tables[0];

        }
        #endregion

        #region 购物车AE待审项目查询V1.1              
        public DataTable OrderIDName_Select(int MediaType, int AEUserID, out string Msg)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@AEUserID",SqlDbType.Int,4),
                    new SqlParameter("@Msg",SqlDbType.VarChar,200)
                                        };
            parameters[0].Value = MediaType;
            parameters[1].Value = AEUserID;
            parameters[2].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_OrderIDName_Select", parameters);
            Msg = (string)parameters[2].Value;
            if (!string.IsNullOrEmpty(Msg))
                return null;

            return ds.Tables[0];

        }
        #endregion

        #region 购物车中当前选择的广告位在项目中是否已存在V1.1                
        public string PubDetailVertify_ADOrderOrCart(string OrderID, string PublishDetailIDs)
        {
            string sqlstr = "SELECT PubDetailID FROM dbo.ADDetailInfo WHERE OrderID='" + OrderID + "' AND PubDetailID IN(" + PublishDetailIDs + ")";
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                PublishDetailIDs = "";
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    PublishDetailIDs += row["PubDetailID"].ToString() + ",";
                }

                if (PublishDetailIDs.EndsWith(","))
                    PublishDetailIDs = PublishDetailIDs.Substring(0, PublishDetailIDs.Length - 1);

                return "当前购物车中的广告位：" + PublishDetailIDs + ",在项目:" + OrderID + "已存在";
            }

            return "";
        }
        #endregion

        #region 根据媒体类型、创建人清空购物车(未选择的不清)V1.1       
        public int ClearAll_CartInfoV1_1(int mediatype, int userid, string ids)
        {
            string sqlstr = string.Empty;
            if (mediatype == 14002 || mediatype == 14001)//APP、微信
            {
                sqlstr = string.Format("DELETE FROM dbo.CartInfo WHERE IsSelected=1 AND CreateUserID={0} AND MediaType={1} AND PubDetailID IN({2})", userid, mediatype, ids);
            }
            else
            {
                sqlstr = string.Format("DELETE FROM dbo.CartInfo WHERE CreateUserID={0} AND MediaType={1} AND MediaID IN({2})", userid, mediatype, ids);
            }
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
        }
        #endregion
        #region UpdateV1.1.4
        public void UpdateV1_4(Entities.CartInfo model)
        {
            string sqlstr = @"UPDATE dbo.CartInfo SET ADLaunchDays=@ADLaunchDays,IsSelected=@IsSelected WHERE CartID=@CartID";
            SqlParameter[] parameters = {
                    new SqlParameter("@CartID",model.CartID),
                    new SqlParameter("@ADLaunchDays",model.ADLaunchDays),
                    new SqlParameter("@IsSelected",model.IsSelected)
                                        };

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
        }
        #endregion
        #region 购物车AE待审项目查询V1.4
        public void OrderIDName_SelectV1_4(int AEUserID, out string Msg, out List<Dictionary<string, object>> dicList)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@AEUserID",SqlDbType.Int,4),
                    new SqlParameter("@Msg",SqlDbType.VarChar,200)
                                        };
            parameters[0].Value = AEUserID;
            parameters[1].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_OrderIDName_Select", parameters);
            Msg = (string)parameters[1].Value;            

            dicList = new List<Dictionary<string, object>>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("OrderID", row["OrderID"]);
                    dic.Add("OrderName", row["OrderName"]);

                    dicList.Add(dic);
                }

            }
        }
        #endregion
        #region APP添加购物V1.1.4 Insert                
        public string InsertAPP(Entities.CartInfo model, out int CartID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Msg", SqlDbType.VarChar,200),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@MediaID", SqlDbType.Int,4),
                    new SqlParameter("@SaleAreaID",SqlDbType.Int,4),
                    new SqlParameter("@PubDetailID",SqlDbType.Int,4),
                    new SqlParameter("@IsSelected",SqlDbType.Bit,2),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@CartID",SqlDbType.Int,4),
                    new SqlParameter("@BeginData",SqlDbType.DateTime),
                    new SqlParameter("@EndData",SqlDbType.DateTime),
                    new SqlParameter("@ADLaunchDays",SqlDbType.Int,4)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.MediaType;
            parameters[2].Value = model.MediaID;
            parameters[3].Value = model.SaleArea;
            parameters[4].Value = model.PubDetailID;
            parameters[5].Value = model.IsSelected;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Direction = ParameterDirection.Output;
            parameters[8].Value = model.BeginData;
            parameters[9].Value = model.EndData;
            parameters[10].Value = model.ADLaunchDays;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartInfoAPP_Insert", parameters);
            CartID = -2;
            string retval = (string)parameters[0].Value;
            if (string.IsNullOrEmpty(retval))
            {
                CartID = (int)parameters[7].Value;
            }
            return retval;
        }
        #endregion
        #region APP购物车排期(新增修改删除)              
        public string CartScheduleInfoAPP_Oper(int OptType, int CartID, int RecID, DateTime BeginTime, DateTime EndTime,int CreateUserID, out int RecIDNew)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Msg", SqlDbType.VarChar,200),
                    new SqlParameter("@OptType",SqlDbType.Int,4),
                    new SqlParameter("@CartID", SqlDbType.Int,4),
                    new SqlParameter("@RecID",SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@BeginData",SqlDbType.DateTime),
                    new SqlParameter("@RecIDNew",SqlDbType.Int,4),
                    new SqlParameter("@EndData",SqlDbType.DateTime)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = OptType;
            parameters[2].Value = CartID;
            parameters[3].Value = RecID;
            parameters[4].Value = CreateUserID;
            parameters[5].Value = BeginTime;
            parameters[6].Direction = ParameterDirection.Output;
            parameters[7].Value = EndTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartScheduleInfoAPP_Oper", parameters);

            string retval = (string)parameters[0].Value;
            RecIDNew = -2;
            if (string.IsNullOrEmpty(retval))
            {
                RecIDNew = (int)parameters[6].Value;
            }
            return retval;
        }
        #endregion
        #region 购物车中当前选择的广告位在项目中是否已存在V1.1.4               
        public string p_PubDetailVertify_ADOrderOrCart(string orderID, int cartID,int mediaType,int pubDetailID,int saleAreaID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Msg", SqlDbType.VarChar,200),
                    new SqlParameter("@OrderID",SqlDbType.VarChar,50),
                    new SqlParameter("@CartID", SqlDbType.Int,4),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@PubDetailID",SqlDbType.Int,4),
                    new SqlParameter("@SaleAreaID",SqlDbType.Int,4)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = orderID;
            parameters[2].Value = cartID;
            parameters[3].Value = mediaType;
            parameters[4].Value = pubDetailID;
            parameters[5].Value = saleAreaID;         

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_PubDetailVertify_ADOrderOrCart", parameters);            
            return (string)parameters[0].Value;
        }
        #endregion
        #region APP购物车查询V1.1.4              
        public DataTable p_ADCartInfoDetailAPP_Select(int MediaType, int CreateUserID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4)
                                        };
            parameters[0].Value = MediaType;
            parameters[1].Value = CreateUserID;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADCartInfoDetailAPP_Select", parameters);
            return ds.Tables[0];

        }
        #endregion
        #region 查询一段时间范围内的节假是天数及详情V1.4
        public int p_CALHolidays(DateTime beginDate,DateTime endDate,out DataTable dtHoliday)
        {
            dtHoliday = null; 
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@StartDate",beginDate),
                new SqlParameter("@EndDate",endDate)
            };

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CALHolidays", parameters);
            if (ds.Tables[1]?.Rows.Count > 0)
                dtHoliday = ds.Tables[1];

            if (ds.Tables[0]?.Rows.Count > 0)
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            return 0;       
        }
        #endregion
        #region 购物车投放V1.1.4                
        public string Delivery(Entities.CartInfo model)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@MediaType",model.MediaType),
                new SqlParameter("@CartID",model.CartID),
                new SqlParameter("@MediaID",model.MediaID),
                new SqlParameter("@PubDetailID",model.PubDetailID),
                new SqlParameter("@SaleAreaID",model.SaleArea),
                new SqlParameter("@ADLaunchDays",model.ADLaunchDays),
                new SqlParameter("@IsSelected",model.IsSelected),
                new SqlParameter("@Msg",SqlDbType.VarChar,200)
            };
            parameters[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartInfoDelivery", parameters);
           
            return (string)parameters[7].Value;
        }
        #endregion

        #region V1.1.8
        #region 购物车中当前选择的广告位在项目中是否已存在V1.1.4               
        public string p_PubDetailVertify_ADOrderOrCartV1_1_8(string orderID, int cartID, int mediaType, int pubDetailID, int saleAreaID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Msg", SqlDbType.VarChar,200),
                    new SqlParameter("@OrderID",SqlDbType.VarChar,50),
                    new SqlParameter("@CartID", SqlDbType.Int,4),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@PubDetailID",SqlDbType.Int,4),
                    new SqlParameter("@SaleAreaID",SqlDbType.Int,4)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = orderID;
            parameters[2].Value = cartID;
            parameters[3].Value = mediaType;
            parameters[4].Value = pubDetailID;
            parameters[5].Value = saleAreaID;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_PubDetailVertify_ADOrderOrCartV1_1_8", parameters);
            return (string)parameters[0].Value;
        }
        #endregion
        public string InsertAPPV1_1_8(Entities.CartInfo model, out int CartID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Msg", SqlDbType.VarChar,200),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@MediaID", SqlDbType.Int,4),
                    new SqlParameter("@SaleAreaID",SqlDbType.Int,4),
                    new SqlParameter("@PubDetailID",SqlDbType.Int,4),
                    new SqlParameter("@IsSelected",SqlDbType.Bit,2),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@CartID",SqlDbType.Int,4),
                    new SqlParameter("@BeginData",SqlDbType.DateTime),
                    new SqlParameter("@EndData",SqlDbType.DateTime),
                    new SqlParameter("@ADLaunchDays",SqlDbType.Int,4)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.MediaType;
            parameters[2].Value = model.MediaID;
            parameters[3].Value = model.SaleArea;
            parameters[4].Value = model.PubDetailID;
            parameters[5].Value = model.IsSelected;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Direction = ParameterDirection.Output;
            parameters[8].Value = model.BeginData;
            parameters[9].Value = model.EndData;
            parameters[10].Value = model.ADLaunchDays;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartInfoAPP_InsertV1_1_8", parameters);
            CartID = -2;
            string retval = (string)parameters[0].Value;
            if (string.IsNullOrEmpty(retval))
            {
                CartID = (int)parameters[7].Value;
            }
            return retval;
        }
        public string InsertV1_1_8(Entities.CartInfo model, out int CartID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Msg", SqlDbType.VarChar,200),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@MediaID", SqlDbType.Int,4),
                    new SqlParameter("@PubID",SqlDbType.Int,4),
                    new SqlParameter("@PubDetailID",SqlDbType.Int,4),
                    new SqlParameter("@IsSelected",SqlDbType.Bit,2),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@CartID",SqlDbType.Int,4),
                    new SqlParameter("@ADSchedule",SqlDbType.DateTime)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.MediaType;
            parameters[2].Value = model.MediaID;
            parameters[3].Value = model.PubID;
            parameters[4].Value = model.PubDetailID;
            parameters[5].Value = model.IsSelected;
            parameters[6].Value = model.CreateUserID;
            parameters[7].Direction = ParameterDirection.Output;
            parameters[8].Value = model.ADSchedule;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartInfo_InsertV1_1_8", parameters);
            CartID = -2;
            string retval = (string)parameters[0].Value;
            if (string.IsNullOrEmpty(retval))
            {
                CartID = (int)parameters[7].Value;
            }
            return retval;
        }
        public string CartScheduleInfo_OperV1_1_8(int OptType, int CartID, int RecID, DateTime BeginTime, int CreateUserID, out int RecIDNew)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Msg", SqlDbType.VarChar,200),
                    new SqlParameter("@OptType",SqlDbType.Int,4),
                    new SqlParameter("@CartID", SqlDbType.Int,4),
                    new SqlParameter("@RecID",SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@BeginTime",SqlDbType.DateTime),
                    new SqlParameter("@RecIDNew",SqlDbType.Int,4)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = OptType;
            parameters[2].Value = CartID;
            parameters[3].Value = RecID;
            parameters[4].Value = CreateUserID;
            parameters[5].Value = BeginTime;
            parameters[6].Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartScheduleInfo_OperV1_1_8", parameters);

            string retval = (string)parameters[0].Value;
            RecIDNew = -2;
            if (string.IsNullOrEmpty(retval))
            {
                RecIDNew = (int)parameters[6].Value;
            }
            return retval;
        }
        public string CartScheduleInfoAPP_OperV1_1_8(int OptType, int CartID, int RecID, DateTime BeginTime, DateTime EndTime, int CreateUserID, out int RecIDNew)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@Msg", SqlDbType.VarChar,200),
                    new SqlParameter("@OptType",SqlDbType.Int,4),
                    new SqlParameter("@CartID", SqlDbType.Int,4),
                    new SqlParameter("@RecID",SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@BeginData",SqlDbType.DateTime),
                    new SqlParameter("@RecIDNew",SqlDbType.Int,4),
                    new SqlParameter("@EndData",SqlDbType.DateTime)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = OptType;
            parameters[2].Value = CartID;
            parameters[3].Value = RecID;
            parameters[4].Value = CreateUserID;
            parameters[5].Value = BeginTime;
            parameters[6].Direction = ParameterDirection.Output;
            parameters[7].Value = EndTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CartScheduleInfoAPP_OperV1_1_8", parameters);

            string retval = (string)parameters[0].Value;
            RecIDNew = -2;
            if (string.IsNullOrEmpty(retval))
            {
                RecIDNew = (int)parameters[6].Value;
            }
            return retval;
        }

        #region 购物车AE待审项目查询
        public void OrderIDName_SelectV1_1_8(int AEUserID, out string Msg, out List<Dictionary<string, object>> dicList)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@AEUserID",SqlDbType.Int,4),
                    new SqlParameter("@Msg",SqlDbType.VarChar,200)
                                        };
            parameters[0].Value = AEUserID;
            parameters[1].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_OrderIDName_SelectV1_1_8", parameters);
            Msg = (string)parameters[1].Value;

            dicList = new List<Dictionary<string, object>>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("OrderID", row["OrderID"]);
                    dic.Add("OrderName", row["OrderName"]);

                    dicList.Add(dic);
                }

            }
        }
        #endregion
        #endregion
    }
}
