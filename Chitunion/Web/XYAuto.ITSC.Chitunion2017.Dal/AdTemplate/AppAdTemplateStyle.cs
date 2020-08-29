/********************************************************
*创建人：lixiong
*创建时间：2017/6/8 17:33:05
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.AdTemplate;
using XYAuto.ITSC.Chitunion2017.Entities.Query.AdTemplate;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.AdTemplate
{
    public class AppAdTemplateStyle : DataBase
    {
        #region Instance

        public static readonly AppAdTemplateStyle Instance = new AppAdTemplateStyle();

        #endregion Instance

        public void Insert_BulkCopyToDB(DataTable dt)
        {
            SqlBulkCopyByDataTable(CONNECTIONSTRINGS, "App_AdTemplateStyle", dt);
        }

        public void Insert_BulkCopyToDB(List<AppAdTemplateStyleTable> list)
        {
            if (list.Count == 0)
                return;
            var strSql = new StringBuilder("insert into App_AdTemplateStyle(");
            strSql.Append("BaseMediaID,AdTemplateID,AdStyle,CreateUserID,IsPublic,CreateTime ) values ");

            list.ForEach(item =>
            {
                strSql.Append("(");
                strSql.AppendFormat("{0},{1},'{2}',{3},{4},GETDATE()", item.BaseMediaID, item.AdTemplateID,
                    item.AdStyle, item.CreateUserID, item.IsPublic);
                strSql.Append("),");
            });
            var sql = strSql.ToString().TrimEnd(',');
            var paras = new List<SqlParameter>();
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
        }

        public int Delete(int adTemplateId)
        {
            var sql = @"DELETE FROM DBO.App_AdTemplateStyle WHERE AdTemplateID = @AdTemplateID AND IsPublic = 0 ";
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("@AdTemplateID",adTemplateId)
            };
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
        }

        public List<Entities.AdTemplate.AppAdTemplateStyle> GetList(AdTemplateQuery<Entities.AdTemplate.AppAdTemplateStyle> query)
        {
            string sql = @" SELECT TOP ({0})
                                   ATS.RecID ,
                                   ATS.BaseMediaID ,
                                   ATS.AdTemplateID ,
                                   ATS.AdStyle ,
                                   ATS.IsPublic ,
                                   ATS.CreateUserID ,
                                   ATS.CreateTime
                            FROM  dbo.App_AdTemplateStyle AS ATS WITH ( NOLOCK )
                            WHERE 1 = 1 ";

            sql = string.Format(sql, query.PageSize);
            var paras = new List<SqlParameter>();
            if (query.BaseMediaId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND ATS.BaseMediaID = @BaseMediaID";
                paras.Add(new SqlParameter("@BaseMediaID", query.BaseMediaId));
            }
            if (query.TemplateId != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND ATS.AdTemplateID = @AdTemplateID";
                paras.Add(new SqlParameter("@AdTemplateID", query.TemplateId));
            }
            if (query.IsPublic != Entities.Constants.Constant.INT_INVALID_VALUE)
            {
                sql += " AND ATS.IsPublic = @IsPublic ";
                paras.Add(new SqlParameter("@IsPublic", query.IsPublic));
            }
            if (!string.IsNullOrWhiteSpace(query.OrderBy))
            {
                sql = string.Format(" ORDER BY {0}", query.OrderBy);
            }
            else
            {
                sql += " ORDER BY ATS.RecID ASC ";
            }

            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, paras.ToArray());
            return DataTableToList<Entities.AdTemplate.AppAdTemplateStyle>(data.Tables[0]);
        }
    }
}