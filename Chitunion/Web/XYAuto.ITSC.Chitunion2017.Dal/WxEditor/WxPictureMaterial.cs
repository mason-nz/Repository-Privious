using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_6;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.WxEditor
{
    /// <summary>
    /// 2017-06-22 zlb
    /// 微信图片类 Dal
    /// </summary>
    public class WxPictureMaterial : DataBase
    {
        public static readonly WxPictureMaterial Instance = new WxPictureMaterial();

        /// <summary>
        /// zlb 2017-06-27
        /// 查询用户下的微信图片列表
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="PicName">图片名称</param>
        /// <param name="WxID">0:查询本地上传 大于0：微信的 小于0：全部的</param>>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public DataTable SelectPictrues(int PageIndex, int PageSize, string PicName, int WxID, int UserID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PicName", SqlDbType.VarChar,200),
                    new SqlParameter("@WxID", SqlDbType.Int),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                    };
            parameters[0].Value = UserID;
            parameters[1].Value = PageIndex;
            parameters[2].Value = PageSize;
            parameters[3].Value = PicName;
            parameters[4].Value = WxID;
            parameters[5].Direction = ParameterDirection.Output;

            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SelectPictrues", parameters).Tables[0];
            int totalCount = (int)(parameters[5].Value);
            dt.Columns.Add("TotalCount", typeof(int), totalCount.ToString());
            return dt;
        }
        /// <summary>
        /// zlb 2017-06-28
        /// 批量删除微信图片
        /// </summary>
        /// <param name="PicIDs">图片ID集合</param>
        /// <param name="UserID">用户ID</param>
        public int DeletePictruesByPicIDs(string PicIDs, int UserID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("DELETE FROM dbo.WxPictureMaterial WHERE PicID IN ({0}) AND CreateUserID={1}", PicIDs, UserID);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sb.ToString());
        }
        /// <summary>
        /// zlb 2117-06-28
        /// 批量添加微信图片
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PicInfos"></param>
        /// <returns></returns>
        public int InsertPictrues(int UserID, Dictionary<string, string> PicInfos)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT  INTO dbo.WxPictureMaterial (WxID,WxMaterialID,PicName,PicUrl,Status,CreateUserID,CreateTime) VALUES ");
            DateTime dtNow = DateTime.Now;
            foreach (var item in PicInfos)
            {
                sb.AppendFormat("(0,0,'{0}','{1}',0,{2},'{3}'),", item.Value, item.Key, UserID, dtNow);
            }
            string strSql = sb.ToString();
            strSql = strSql.Remove(strSql.LastIndexOf(","), 1);
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql);
        }
        /// <summary>
        /// zlb 2017-06-28
        /// 修改图片名称
        /// </summary>
        /// <param name="req">图片信息类</param>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        public int UpdatePictruesByPicID(UpdatePictrueNameReqDTO req, int UserID)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int),
                    new SqlParameter("@PicName", SqlDbType.VarChar,200),
                    new SqlParameter("@PicID", SqlDbType.Int),
                    };
            parameters[0].Value = UserID;
            parameters[1].Value = req.PicName.Trim();
            parameters[2].Value = req.PicID;
            string strSql = "update WxPictureMaterial SET PicName=@PicName where PicID=@PicID and CreateUserID=@UserID";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, strSql, parameters);
        }
    }
}
