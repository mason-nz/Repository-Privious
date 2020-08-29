using System.Collections.Generic;
using System.Data;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.DicInfo
{
    public class DictInfo : DataBase
    {
        public static readonly DictInfo Instance = new DictInfo();

        public DataTable GetDictInfoByTypeId(int TypeId)
        {
            string SQL = string.Empty;
            switch (TypeId)
            {
                case 8001://封装渠道
                    SQL = "SELECT ChannelID AS DictId,ChannelName AS DictName FROM Chitunion_OP2017..[CollectionChannel] ORDER BY ChannelID";
                    break;
                case 8002://场景
                    SQL = "SELECT SceneID AS DictId, SceneName AS DictName FROM  Chitunion_OP2017..SceneInfo ORDER BY SceneValue";
                    break;
                default://其他
                    SQL = $@"
					SELECT  DictId ,
							DictName
					FROM    ( SELECT    DictId ,
										DictName ,
										DictType ,
										OrderNum
							  FROM      dbo.DictInfo
							  WHERE     Status = 0
										AND DictType > 0
										AND DictType IN ( 74, 75, 76, 77 )
							  UNION ALL
							  SELECT    DictId ,
										DictName ,
										DictType ,
										OrderNum
							  FROM      Chitunion_OP2017..DictInfo
							  WHERE     Status = 0
										AND DictType > 0
										AND DictType IN ( 7, 73 )
							) AS DictInfo
					WHERE   DictInfo.DictType = {TypeId}";
                    break;
            }

            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, SQL);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        public List<Entities.DictInfo.DictInfo> GetDictInfo()
        {
            string sql = string.Format("SELECT DictId,DictType,DictName FROM DictInfo WHERE Status=0 And DictType>0 ");
            //if (typeId > 0)
            //{
            //    sql += $" AND DictType={typeId} ";
            //}

            sql += " ORDER BY OrderNum";
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToList<Entities.DictInfo.DictInfo>(data.Tables[0]);
        }
    }
}