using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.IP2017.Dal.Import
{
    public class ImportAPPLabel : DataBase
    {
        public readonly static ImportAPPLabel Instance = new ImportAPPLabel();
        #region 获取APP基表数据
        public DataTable GetMediaList()
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT  MBPC.Name ,
                                    MBPC.HeadIconURL HeadImg ,
                                    MBPC.CreateTime ,
                                    MBPC.DailyLive
                            FROM    Chitunion2017.dbo.Media_BasePCAPP MBPC
                            WHERE   MBPC.Status = 0 ");

            sbSql.Append($" ORDER BY MBPC.CreateTime DESC ");
            //var parameters = new SqlParameter[]{
            //                new SqlParameter("@MasterId",masterBrandId)
            //            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return data.Tables[0];
        }
        #endregion
        #region 获取APP标签最终结果数据
        public DataTable GetMediaLabelResultList(int mediaType)
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT  *
                            FROM    dbo.MediaLabelResult MLR
                            WHERE   MLR.MediaType = {mediaType} ");
            
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return data.Tables[0];
        }
        #endregion
        #region 获取标签数据
        public DataTable GetLabelList()
        {
            var sbSql = new StringBuilder();
            sbSql.Append($@"
                            SELECT  TBI.TitleID DictId,
                                    TBI.Name DictName,
                                    TBI.Type
                            FROM    dbo.TitleBasicInfo TBI");

            //var parameters = new SqlParameter[]{
            //                new SqlParameter("@MasterId",masterBrandId)
            //            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sbSql.ToString());
            return data.Tables[0];
        }
        #endregion
    }
}
