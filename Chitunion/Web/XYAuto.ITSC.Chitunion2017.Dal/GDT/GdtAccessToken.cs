/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 17:17:33
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal.GDT
{
    public class GdtAccessToken : DataBase
    {
        #region Instance

        public static readonly GdtAccessToken Instance = new GdtAccessToken();

        #endregion Instance

        public Entities.GDT.GdtAccessToken GetInfo(int relationType, int clientId)
        {
            var sql =
                $@"SELECT * FROM DBO.GDT_AccessToken WITH(NOLOCK) WHERE RelationType = {relationType} AND ClientId = {
                    clientId}";
            var obj = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.GDT.GdtAccessToken>(obj.Tables[0]);
        }

        public int InsertByGdtServer(Entities.GDT.GdtAccessToken entity)
        {
            var sql = string.Format(@"
                    IF EXISTS(SELECT 1 FROM DBO.GDT_AccessToken WITH(NOLOCK) WHERE RelationType = {0} AND ClientId = {1})
                    BEGIN
	                    UPDATE DBO.GDT_AccessToken SET AccessToken = '{2}',RefreshToken='{3}',UpdateTime = GETDATE() WHERE RelationType = {0} AND ClientId = {1}
                    END
                    ", entity.RelationType, entity.ClientId, entity.AccessToken, entity.RefreshToken);
            var obj = SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
            return Convert.ToInt32(obj);
        }
    }
}