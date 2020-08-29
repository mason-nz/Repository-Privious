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
    public class ADDetailInfo : DataBase
    {
        public const string P_ADDetailInfo_SELECT = "p_ADDetailInfo_Select";

        #region Instance
        public static readonly ADDetailInfo Instance = new ADDetailInfo();
        #endregion

        #region Select
        /// <summary>
        /// 根据主键值得到一个对象实体
        /// </summary>
        public Entities.ADDetailInfo GetModel(int recid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT TOP 1 RecID,OrderID,SubOrderID,MediaType,MediaID,PubID,PubDetailID,ADLaunchIDs,ADLaunchStr,OriginalPrice,AdjustPrice,AdjustDiscount,PurchaseDiscount,SaleDiscount,ADLaunchDays,CreateTime,CreateUserID,LastUpdateTime,LastUpdateUserID ");
            strSql.Append(" FROM dbo.ADDetailInfo ");
            strSql.Append(" WHERE RecID=@RecID");
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)
			};
            parameters[0].Value = recid;

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
        public Entities.ADDetailInfo DataRowToModel(DataRow row)
        {
            Entities.ADDetailInfo model = new Entities.ADDetailInfo();
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
                if (row["SubOrderID"] != null && row["SubOrderID"].ToString() != "")
                {
                    model.SubOrderID = row["SubOrderID"].ToString();
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
                if (row["ADLaunchIDs"] != null && row["ADLaunchIDs"].ToString() != "")
                {
                    model.ADLaunchIDs = row["ADLaunchIDs"].ToString();
                }
                if (row["ADLaunchStr"] != null && row["ADLaunchStr"].ToString() != "")
                {
                    model.ADLaunchStr = row["ADLaunchStr"].ToString();
                }
                if (row["OriginalPrice"] != null && row["OriginalPrice"].ToString() != "")
                {
                    model.OriginalPrice = decimal.Parse(row["OriginalPrice"].ToString());
                }
                if (row["AdjustPrice"] != null && row["AdjustPrice"].ToString() != "")
                {
                    model.AdjustPrice = decimal.Parse(row["AdjustPrice"].ToString());
                }
                if (row["AdjustDiscount"] != null && row["AdjustDiscount"].ToString() != "")
                {
                    model.AdjustDiscount = decimal.Parse(row["AdjustDiscount"].ToString());
                }
                if (row["PurchaseDiscount"] != null && row["PurchaseDiscount"].ToString() != "")
                {
                    model.PurchaseDiscount = decimal.Parse(row["PurchaseDiscount"].ToString());
                }
                if (row["SaleDiscount"] != null && row["SaleDiscount"].ToString() != "")
                {
                    model.SaleDiscount = decimal.Parse(row["SaleDiscount"].ToString());
                }
                if (row["ADLaunchDays"] != null && row["ADLaunchDays"].ToString() != "")
                {
                    model.ADLaunchDays = int.Parse(row["ADLaunchDays"].ToString());
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["CreateUserID"] != null && row["CreateUserID"].ToString() != "")
                {
                    model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
                }
                if (row["LastUpdateTime"] != null && row["LastUpdateTime"].ToString() != "")
                {
                    model.LastUpdateTime = DateTime.Parse(row["LastUpdateTime"].ToString());
                }
                if (row["LastUpdateUserID"] != null && row["LastUpdateUserID"].ToString() != "")
                {
                    model.LastUpdateUserID = int.Parse(row["LastUpdateUserID"].ToString());
                }
                
            }
            return model;
        }

        #region 根据订单号查询得到一个对象实体
        


        public Entities.ADDetailInfo GetADDetailInfo(int recid)
        {
            QueryADDetailInfo query = new QueryADDetailInfo();
            query.RecID = recid;
            DataTable dt = new DataTable();
            int count = 0;
            dt = GetADDetailInfo(query, string.Empty, 1, 1, out count);
            if (count > 0)
            {
                return LoadSingleADDetailInfo(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        private Entities.ADDetailInfo LoadSingleADDetailInfo(DataRow row)
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
        public DataTable GetADDetailInfo(QueryADDetailInfo query, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            if (query.OrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.OrderID = '" + query.OrderID + "'";
            }
            if (query.SubOrderID != Constant.STRING_INVALID_VALUE)
            {
                where += " AND a.SubOrderID = '" + query.SubOrderID +"'";
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

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADDetailInfo_Select", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        public DataTable GetADDetailInfoBySubOrderID(string subOrderID)
        {
            string sqlstr = @"SELECT * FROM dbo.ADDetailInfo WHERE SubOrderID=@SubOrderID ORDER BY CreateTime DESC";
            SqlParameter[] parameters = {
                    new SqlParameter("@SubOrderID", SqlDbType.NVarChar, 50)
                    };
            parameters[0].Value = subOrderID;           

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            return ds.Tables[0];
        }
        #endregion
        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Insert(Entities.ADDetailInfo model)
        {
            SqlParameter[] parameters = { 
                    new SqlParameter("@ADSID", SqlDbType.Int,4),                                         
                    new SqlParameter("@OrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@SubOrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@MediaType",SqlDbType.Int,4), 
					new SqlParameter("@MediaID", SqlDbType.Int,4),
                    new SqlParameter("@PubID",SqlDbType.Int,4),
                    new SqlParameter("@ADLaunchIDs",SqlDbType.VarChar,100),
                    new SqlParameter("@ADLaunchStr",SqlDbType.VarChar,500),
                    new SqlParameter("@OriginalPrice",SqlDbType.Decimal,18),
                    new SqlParameter("@AdjustPrice",SqlDbType.Decimal,18),
                    new SqlParameter("@AdjustDiscount",SqlDbType.Decimal,18),
                    new SqlParameter("@PurchaseDiscount",SqlDbType.Decimal,18),
                    new SqlParameter("@SaleDiscount",SqlDbType.Decimal,18),
                    new SqlParameter("@ADLaunchDays",SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@PubDetailID",SqlDbType.Int,4),
                    new SqlParameter("@SaleAreaID",SqlDbType.Int,4),
                    new SqlParameter("@Holidays",SqlDbType.Int,4),
                    new SqlParameter("@HasHoliday",SqlDbType.Int,4),
                    new SqlParameter("@SalePrice_Holiday",SqlDbType.Decimal,18)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.SubOrderID;
            parameters[3].Value = model.MediaType;
            parameters[4].Value = model.MediaID;
            parameters[5].Value = model.PubID;
            parameters[6].Value = model.ADLaunchIDs;
            parameters[7].Value = model.ADLaunchStr;
            parameters[8].Value = model.OriginalPrice;
            parameters[9].Value = model.AdjustPrice;
            parameters[10].Value = model.AdjustDiscount;
            parameters[11].Value = model.PurchaseDiscount;
            parameters[12].Value = model.SaleDiscount;
            parameters[13].Value = model.ADLaunchDays;
            parameters[14].Value = model.CreateUserID;
            parameters[15].Value = model.PubDetailID;
            parameters[16].Value = model.SaleArea;
            parameters[17].Value = model.Holidays;
            parameters[18].Value = model.HasHoliday;
            parameters[19].Value = model.SalePrice_Holiday;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADDetailInfo_Insert", parameters);
            return (int)parameters[0].Value;
        }
        #endregion
        #region Update
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(Entities.ADDetailInfo model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@RecID",SqlDbType.Int,4),
					new SqlParameter("@OrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@SubOrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@MediaType",SqlDbType.Int,4), 
					new SqlParameter("@MediaID", SqlDbType.Int,4),
                    new SqlParameter("@PubID", SqlDbType.Int,4),
                    new SqlParameter("@ADLaunchIDs",SqlDbType.VarChar,100),
                    new SqlParameter("@ADLaunchStr",SqlDbType.VarChar,500),
                    new SqlParameter("@OriginalPrice",SqlDbType.Decimal,18),
                    new SqlParameter("@AdjustPrice",SqlDbType.Decimal,18),
                    new SqlParameter("@AdjustDiscount",SqlDbType.Decimal,18),
                    new SqlParameter("@PurchaseDiscount",SqlDbType.Decimal,18),
                    new SqlParameter("@SaleDiscount",SqlDbType.Decimal,18),
                    new SqlParameter("@ADLaunchDays",SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.SubOrderID;
            parameters[3].Value = model.MediaType;
            parameters[4].Value = model.MediaID;
            parameters[5].Value = model.PubID;
            parameters[6].Value = model.ADLaunchIDs;
            parameters[7].Value = model.ADLaunchStr;
            parameters[8].Value = model.OriginalPrice;
            parameters[9].Value = model.AdjustPrice;
            parameters[10].Value = model.AdjustDiscount;
            parameters[11].Value = model.PurchaseDiscount;
            parameters[12].Value = model.SaleDiscount;
            parameters[13].Value = model.ADLaunchDays; 

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADDetailInfo_Update", parameters);
            return (int)parameters[0].Value;
        }
        #endregion
        #region Delete
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int RecID)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.Int,4)};
            parameters[0].Value = RecID;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADDetailInfo_Delete", parameters);
        }       
        #endregion
      
        #region 根据主订单号删除广告位
        public int DeleteByOrderID(string orderid)
        {
            string sqlstr = string.Format("DELETE dbo.ADDetailInfo WHERE OrderID='{0}'",orderid);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sqlstr);
        }
        #endregion
        #region 根据广告位ID获取广告位信息
        public Entities.ADDetailInfo p_GetPubDetailInfo_SelectV1_1(int mediaType, int pubDetailID)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@MediaType",mediaType),
                new SqlParameter("@PubDetailID",pubDetailID)
            };      

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_GetPubDetailInfo_SelectV1_1", parameters);
            return DataTableToEntity<Entities.ADDetailInfo>(ds.Tables[0]);
        }
        #endregion
        #region 根据订单号查询
        public DataTable QueryADDetailBySubOrderID(int mediaType, string subOrderID)
        {
            string sqlstr = string.Empty;
            if (mediaType == 14001)
            {
                sqlstr = @"SELECT 
                            ADI.RecID ADDetailID,
                            CASE 
	                            WHEN wechat.IsAuth=1 THEN '微信认证' 
	                            ELSE '' 
                            END IsAuth,
                            CASE 
		                            WHEN ui.Source=3001 THEN '赤兔自营' 
		                            WHEN (ui.Source=3002 AND ui.UserName IS NOT NULL) THEN	ui.UserName
		                            WHEN (ui.Source=3002 AND ud.TrueName IS NOT NULL) THEN	ud.TrueName 
		                            ELSE ui.Mobile 
                            END Source,
                            pubdetail.PubID,
                            pub.BeginTime PubBeginTime,
                            pub.EndTime PubEndTime,
                            pubdetail.RecID PublishDetailID,
                            wechat.Number,wechat.Name,
                            wechat.HeadIconURL ADMasterImage,
                            wechat.ADName ADMasterTitle,
                            ADPosition1.DictName ADPosition,
                            ADPosition2.DictName CreateType,
                            ADPosition1.DictId ADPositionID,
                            ADPosition2.DictId CreateTypeID,
                            ADI.OriginalPrice,
                            ADI.AdjustPrice,
                            ADI.AdjustDiscount,
                            ADI.PurchaseDiscount,
                            ADI.SaleDiscount,
                            ADI.ADLaunchDays,
                            ADI.CreateTime,
                            ADI.CreateUserID,
                            CASE 
	                            WHEN wechat.Status = -1 OR pub.IsDel=-1 THEN -1
	                            WHEN GETDATE() BETWEEN pub.BeginTime AND pub.EndTime THEN 0 
	                            ELSE 1 
                            END expired,
                            pub.Wx_Status PublishStatus,
                            wechat.Status MediaStatus,
                            CASE
	                            WHEN pub.Wx_Status=42912
	                            THEN (SELECT COUNT(1) 
				                            FROM dbo.Publish_BasicInfo PB 
				                            WHERE PB.MediaID=wechat.MediaID AND PB.PubID <> pub.PubID
			                            )
	                            ELSE 0
                            END HasOtherPublish,
                            SADI.ChannelID,
                            CHA.ChannelName,
                            ADI.CostReferencePrice
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
                            AND ADI.SubOrderID=@SubOrderID 
                            ORDER BY ADI.CreateTime DESC";
            }
            else if (mediaType == 14002)
            {
                sqlstr = @"--APP广告位
                            SELECT 
                            ADI.RecID ADDetailID,
                            API.SaleType CPDCPM,
                            CASE 
		                            WHEN UI.Source=3001 THEN '赤兔自营' 
		                            WHEN (UI.Source=3002 AND UI.UserName IS NOT NULL) THEN	UI.UserName
		                            WHEN (UI.Source=3002 AND UD.TrueName IS NOT NULL) THEN	UD.TrueName 
		                            ELSE UI.Mobile 
                            END Source,
                            API.PubID,
                            PB.BeginTime PubBeginTime,
                            PB.EndTime PubEndTime,
                            ADI.PubDetailID PublishDetailID,
                            MP.HeadIconURL ADMasterImage,
                            AAT.AdTemplateName ADMasterTitle,
                            ADTS.AdStyle ADStyle,
                            API.CarouselNumber,
                            DICT.DictName SalePlatform,
                            ADI.SaleArea SaleAreaID,
                            CASE WHEN ADI.SaleArea=-1 THEN '其他'
	                             ELSE AI.AreaName
                            END SaleArea,
                            CASE 
	                            WHEN ADI.HasHoliday IS NULL THEN 0
	                            ELSE CAST(ADI.HasHoliday AS INT)
                            END HasHoliday,
                            ADI.SalePrice_Holiday PriceHoliday,
                            ADI.OriginalPrice,
                            ADI.AdjustPrice,
                            ADI.AdjustDiscount,
                            ADI.PurchaseDiscount,
                            ADI.SaleDiscount,
                            ADI.ADLaunchDays,
                            ADI.CreateTime,
                            ADI.CreateUserID,
                            CASE 
	                            WHEN MP.Status = -1 OR PB.IsDel=-1 THEN -1
	                            WHEN DATEDIFF(DAY,GETDATE(),PB.EndTime)>=0 THEN 0
	                            ELSE 1 
                            END expired,
                            MP.AuditStatus MediaStatus,
                            PB.Wx_Status PublishStatus,
                            API.TemplateID,
                            CASE
	                            WHEN PB.Wx_Status=49005 
	                            THEN (SELECT COUNT(1) 
				                            FROM dbo.Publish_BasicInfo PB2
				                            INNER JOIN dbo.AppPriceInfo API2 ON API.PubID=PB2.PubID
				                            WHERE PB2.Status=0 
				                            AND API2.PubID <>API.PubID
				                            AND PB2.Wx_Status=49004
				                            )
	                            ELSE 0
                            END HasOtherPublish		
                            FROM dbo.ADDetailInfo ADI
                            LEFT JOIN dbo.Media_PCAPP MP ON ADI.MediaID=MP.MediaID
                            LEFT JOIN dbo.UserInfo UI ON MP.CreateUserID=UI.UserID
                            LEFT JOIN dbo.UserDetailInfo UD ON UI.UserID=UD.UserID
                            LEFT JOIN dbo.AppPriceInfo API ON API.RecID=ADI.PubDetailID	
                            LEFT JOIN dbo.App_AdTemplate AAT ON AAT.RecID=API.TemplateID		
                            LEFT JOIN dbo.Publish_BasicInfo PB ON PB.PubID=API.PubID
                            LEFT JOIN dbo.AreaInfo AI ON AI.AreaID=ADI.SaleArea
                            LEFT JOIN dbo.App_AdTemplateStyle ADTS ON ADTS.RecID=API.ADStyle
                            LEFT JOIN (SELECT DictId,DictName FROM dbo.DictInfo WHERE DictType=12) DICT ON DICT.DictId=API.SalePlatform
                            WHERE ADI.MediaType=14002
                            AND	 ADI.SubOrderID=@SubOrderID
                            ORDER BY ADI.CreateTime DESC";
            }
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@SubOrderID",subOrderID)
            };
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr,parameters);
            return ds.Tables[0];
        }
        #endregion
        #region 根据广告位ID查询排期
        public DataTable QueryADScheduleInfoByADDetailID(int adDetailID)
        {
            string sqlstr = @"SELECT  
                                        ADSI.BeginData ,
                                        ADSI.EndData ,
                                        CASE WHEN EndData IS NULL THEN 0
                                             WHEN DATEDIFF(DAY, BeginData, EndData) < 0 THEN 0
                                             ELSE DATEDIFF(DAY, BeginData, EndData) + 1
                                        END AS 'AllDays'
                                FROM    dbo.ADScheduleInfo ADSI
                                WHERE   ADSI.ADDetailID = @ADDetailID";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@ADDetailID",adDetailID)
            };

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sqlstr, parameters);
            return ds.Tables[0];
        }
        #endregion
        #region 提交项目V1.1.4
        #region Insert
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertV1_1_4(Entities.ADDetailInfo model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ADSID", SqlDbType.Int,4),
                    new SqlParameter("@OrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@SubOrderID",SqlDbType.VarChar,20),
                    new SqlParameter("@MediaType",SqlDbType.Int,4),
                    new SqlParameter("@MediaID", SqlDbType.Int,4),
                    new SqlParameter("@PubID",SqlDbType.Int,4),
                    new SqlParameter("@ADLaunchIDs",SqlDbType.VarChar,100),
                    new SqlParameter("@ADLaunchStr",SqlDbType.VarChar,500),
                    new SqlParameter("@OriginalPrice",SqlDbType.Decimal,18),
                    new SqlParameter("@AdjustPrice",SqlDbType.Decimal,18),
                    new SqlParameter("@AdjustDiscount",SqlDbType.Decimal,18),
                    new SqlParameter("@PurchaseDiscount",SqlDbType.Decimal,18),
                    new SqlParameter("@SaleDiscount",SqlDbType.Decimal,18),
                    new SqlParameter("@ADLaunchDays",SqlDbType.Int,4),
                    new SqlParameter("@CreateUserID",SqlDbType.Int,4),
                    new SqlParameter("@PubDetailID",SqlDbType.Int,4),
                    new SqlParameter("@SaleAreaID",SqlDbType.Int,4),
                    new SqlParameter("@Holidays",SqlDbType.Int,4),
                    new SqlParameter("@HasHoliday",SqlDbType.Int,4),
                    new SqlParameter("@SalePrice_Holiday",SqlDbType.Decimal,18),
                    new SqlParameter("@CostReferencePrice",SqlDbType.Decimal,18)
                                        };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.OrderID;
            parameters[2].Value = model.SubOrderID;
            parameters[3].Value = model.MediaType;
            parameters[4].Value = model.MediaID;
            parameters[5].Value = model.PubID;
            parameters[6].Value = model.ADLaunchIDs;
            parameters[7].Value = model.ADLaunchStr;
            parameters[8].Value = model.OriginalPrice;
            parameters[9].Value = model.AdjustPrice;
            parameters[10].Value = model.AdjustDiscount;
            parameters[11].Value = model.PurchaseDiscount;
            parameters[12].Value = model.SaleDiscount;
            parameters[13].Value = model.ADLaunchDays;
            parameters[14].Value = model.CreateUserID;
            parameters[15].Value = model.PubDetailID;
            parameters[16].Value = model.SaleArea;
            parameters[17].Value = model.Holidays;
            parameters[18].Value = model.HasHoliday;
            parameters[19].Value = model.SalePrice_Holiday;
            parameters[20].Value = model.CostReferencePrice;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_ADDetailInfo_InsertV1_1_4", parameters);
            return (int)parameters[0].Value;
        }
        #endregion
        #endregion
    }
}
