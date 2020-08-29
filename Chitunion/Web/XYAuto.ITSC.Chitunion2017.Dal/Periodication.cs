using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.Utils;
using System.Data;
using System.Data.SqlClient;
using XYAuto.Utils.Data;
namespace XYAuto.ITSC.Chitunion2017.Dal
{
    /// <summary>
    /// 2017-02-21 张立彬
    /// 刊例Dal
    /// </summary>
    public class Periodication : DataBase
    {
        public static readonly Periodication Instance = new Periodication();
        #region V1.0
        /// <summary>
        /// 2017-02-23 张立彬
        /// 根据刊例ID和媒体类型查询不同的媒体信息
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="pubID">刊例ID</param>
        /// <returns></returns>
        public DataSet GetPublishInfoBymediaTypeAndPubID(int mediaType, int pubID, string Where)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@mediaType", SqlDbType.Int),
                    new SqlParameter("@pubID", SqlDbType.Int),
                    new SqlParameter("@Where", SqlDbType.VarChar,100)
                    };
            parameters[0].Value = mediaType;
            parameters[1].Value = pubID;
            parameters[2].Value = Where;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectPublishInfo", parameters);
            return ds;
        }
        /// <summary>
        /// 2017-02-24 张立彬
        /// 根据广告位ID查询APP媒体广告位信息
        /// </summary>
        /// <param name="ADDetailID">广告位ID</param>
        /// <returns></returns>
        public DataTable GetAppPublishInfoByAdvID(int ADDetailID, string where)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ADDetailID", SqlDbType.Int),
                    new SqlParameter("@where", SqlDbType.VarChar,100)
                    };
            parameters[0].Value = ADDetailID;
            parameters[1].Value = where;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectAppAdvInfoByAdvID", parameters);
            return ds.Tables[0];
        }
        /// <summary>
        /// 2017-03-01 张立彬
        /// 根据媒体ID或刊例ID查询刊例详情
        /// </summary>
        /// <param name="pubIDOrMediaID"></param>
        /// <returns></returns>
        public DataTable GetPublishBasicInfoByID(int pubIDOrMediaID, int mediaType)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@PubIDOrMediaID", SqlDbType.Int),
                    new SqlParameter("@MediaType", SqlDbType.Int)

                    };
            parameters[0].Value = pubIDOrMediaID;
            parameters[1].Value = mediaType;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectPublishDetailInfo", parameters);
            return ds.Tables[0];
        }
        /// <summary>
        /// 2017-03-02 张立彬
        ///根据刊例ID和其他条件查询APP刊例下广告位的信息列表
        /// </summary>
        /// <param name="pubID">刊例ID</param>
        /// <param name="publishStatus">发布状态</param>
        /// <param name="adPosition">广告位置</param>
        /// <param name="adForm">广告形式</param>
        /// <param name="style">广告样式</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="PageIndex">第几页</param>
        /// <returns></returns>
        public DataTable SelectAppAdvListByPubID(int pubID, int publishStatus, string adPosition, string adForm, string style, int pagesize, int PageIndex, string Where)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@PubID", SqlDbType.Int),
                    new SqlParameter("@PublishStatus", SqlDbType.Int),
                    new SqlParameter("@AdPosition", SqlDbType.NVarChar,100),
                    new SqlParameter("@AdForm", SqlDbType.NVarChar,100),
                       new SqlParameter("@Style", SqlDbType.NVarChar,100),
                    new SqlParameter("@pagesize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int),
                    new SqlParameter("@Where", SqlDbType.VarChar,100)
                    };
            parameters[0].Value = pubID;
            parameters[1].Value = publishStatus;
            parameters[2].Value = adPosition;
            parameters[3].Value = adForm;
            parameters[4].Value = style;
            parameters[5].Value = pagesize;
            parameters[6].Value = PageIndex;
            parameters[7].Direction = ParameterDirection.Output;
            parameters[8].Value = Where;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectAppAdvListByPubID", parameters);
            int totalCount = (int)(parameters[7].Value);
            ds.Tables[0].Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            return ds.Tables[0];


        }
        /// <summary>
        /// 2017-03-04 张立彬
        /// 根据广告位ID查询APP媒体广告位信息
        /// </summary>
        /// <param name="ADDetailID">广告位ID</param>
        /// <returns></returns>
        public DataTable GetAppPublishAdvInfoByAdvID(int ADDetailID, string Where)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ADDetailID", SqlDbType.Int),
                    new SqlParameter("@where", SqlDbType.VarChar,100)
                    };
            parameters[0].Value = ADDetailID;
            parameters[1].Value = Where;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectAppPulishInfoByAdvID", parameters);
            return ds.Tables[0];
        }
        /// <summary>
        /// 2017-03-07 张立彬
        /// 根据媒体ID查询APP媒体刊例信息
        /// </summary>
        /// <param name="MediaID">媒体ID</param>
        /// <returns></returns>
        public DataTable GetAppMediaByMediaID(int MediaID, string Where)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaID", SqlDbType.Int),
                    new SqlParameter("@Where", SqlDbType.VarChar,100)
                    };
            parameters[0].Value = MediaID;
            parameters[1].Value = Where;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectAppMediaByMediaID", parameters);
            return ds.Tables[0];
        }
        #endregion

        #region V1.1
        /// <summary>
        /// 根据刊例ID和媒体类型查询刊例信息 2017-04-21 张立彬
        /// </summary>
        /// <param name="PubID">刊例ID</param>
        /// <param name="MediaType">媒体类型</param>
        /// <returns></returns>
        public DataSet SelectWXPublishByIDAndType(int PubID, int MediaType, string where)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@PubID", SqlDbType.Int),
                    new SqlParameter("@MediaType", SqlDbType.Int),
                    new SqlParameter("@Where", SqlDbType.VarChar,100)
                    };
            parameters[0].Value = PubID;
            parameters[1].Value = MediaType;
            parameters[2].Value = where;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectWXPublishByIDAndType", parameters);
            return ds;
        }
        /// <summary>
        /// 查询购物车刊例信息 2017-04-27 张立彬
        /// </summary>
        /// <param name="MediaType"></param>
        /// <param name="MediaID"></param>
        /// <returns></returns>
        public DataSet SelectShoppingPublish(int MediaType, int MediaID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType", SqlDbType.Int),
                    new SqlParameter("@MediaID", SqlDbType.Int)
                    };
            parameters[0].Value = MediaType;
            parameters[1].Value = MediaID;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectShoppingPublish", parameters);
            return ds;
        }

        #endregion
        #region V1.1.1
        /// <summary>
        ///查询广告下的政策列表及对应的广告位信息集合 张立彬 2017-05-15
        /// </summary>
        /// <param name="MediaType">媒体类型</param>
        /// <param name="MediaID">媒体ID</param>
        /// <returns>政策列表集合</returns>
        public DataSet SelectPublishesByMediaID(int MediaType, int MediaID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@MediaType", SqlDbType.Int),
                    new SqlParameter("@MediaID", SqlDbType.Int)
                    };
            parameters[0].Value = MediaType;
            parameters[1].Value = MediaID;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectPublishesByMediaID", parameters);
            return ds;
        }

        #endregion

        #region V1.1.4
        /// <summary>
        /// 2017-06-08 zlb
        /// 查询APP广告详情
        /// </summary>
        /// <param name="TemplateID">模板ID</param>
        /// <param name="MediaID">订单ID</param>
        /// <returns></returns>
        public DataSet SelectShoppingAppPublish(int TemplateID, int MediaID, int UserID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TemplateID", SqlDbType.Int),
                    new SqlParameter("@MediaID", SqlDbType.Int),
                     new SqlParameter("@UserID", SqlDbType.Int)
                    };
            parameters[0].Value = TemplateID;
            parameters[1].Value = MediaID;
            parameters[2].Value = UserID;
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectAppShoppingPublish", parameters);
            return ds;
        }
        #endregion
        #region 1.1.8
        /// <summary>
        /// zlb 2017-07-26
        /// 查询媒体下对应渠道信息
        /// </summary>
        /// <param name="MediaID">附表媒体ID</param>
        /// <param name="BeginDate">刊例开始时间</param>
        /// <param name="EndDate">刊例结束时间</param>
        /// <returns></returns>
        public DataSet SelectChannelInfoByMediaID(int MediaID, string BeginDate, string EndDate)
        {
            String strSql = string.Format(@"SELECT CC.CostID,CI.ChannelID,CI.ChannelName,CONVERT(varchar(100), CI.CooperateBeginDate, 23) CooperateBeginDate,CONVERT(varchar(100), CI.CooperateEndDate, 23)  CooperateEndDate FROM  ChannelCost CC INNER JOIN ChannelInfo CI ON CC.ChannelID=CI.ChannelID
WHERE CC.MediaID = {0} AND CC.SaleStatus = 44001 AND CC.Status = 0 AND CI.Status = 0 and '{1}'<=CONVERT(varchar(100), CI.CooperateEndDate, 23) and CONVERT(varchar(100), CI.CooperateBeginDate, 23)<='{2}'  ORDER BY CooperateBeginDate", MediaID, BeginDate, EndDate);

            strSql += string.Format(@" SELECT CostID, ADPosition1, ADPosition2, ADPosition3, CostPrice, SalePrice FROM ChannelCostDetail WHERE CostID IN(SELECT CC.CostID FROM  ChannelCost CC INNER JOIN ChannelInfo CI ON CC.ChannelID = CI.ChannelID
WHERE CC.MediaID = {0} AND CC.SaleStatus = 44001 AND CC.Status = 0 AND CI.Status = 0 and '{1}'<=CONVERT(varchar(100), CI.CooperateEndDate, 23) and CONVERT(varchar(100), CI.CooperateBeginDate, 23)<='{2}') AND Status = 0", MediaID, BeginDate, EndDate);
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        #endregion

    }
}
