using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Common.Dal
{
    public class DictInfo : DataBase
    {
        public static readonly DictInfo Instance = new DictInfo();

        public DataTable GetDictInfoByTypeID(int typeID)
        {
            string sql = string.Format("SELECT DictId,DictName FROM DictInfo WHERE Status=0 And DictType>0 AND DictType={0} ORDER BY OrderNum", typeID);
            DataSet ds = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        internal DataTable GetDictInfoByAPP()
        {
//            string sql = string.Format(@"SELECT  di.DictId ,
//        di.DictName
//FROM    DictInfo AS di
//        JOIN ( SELECT   mapp.CategoryID ,
//                        COUNT(DISTINCT mapp.MediaID) AS C
//               FROM     dbo.Media_PCAPP AS mapp
//                        JOIN dbo.Publish_BasicInfo AS pbi ON mapp.MediaID = pbi.MediaID
//               WHERE    mapp.Status = 0 AND pbi.MediaType=14002
//                        AND mapp.MediaID IN (
//                        SELECT  pdi.MediaID
//                        FROM    dbo.Publish_DetailInfo AS pdi
//                        WHERE   pdi.MediaID = mapp.MediaID
//                                AND pdi.MediaType=14002
//                                AND pdi.PublishStatus = 15005 )
//               GROUP BY mapp.CategoryID
//             ) AS b ON di.DictId = b.CategoryID
//WHERE   di.DictType > 0
//        AND di.DictType = 22 AND di.Status=0
//ORDER BY b.C DESC;");
            string sql = string.Format(@"SELECT  di.DictId ,
        di.DictName
FROM    DictInfo AS di
        JOIN ( SELECT   mc.CategoryID,
                        COUNT(DISTINCT mbapp.RecID) AS C
               FROM     dbo.Media_PCAPP AS mapp
                        JOIN dbo.Media_BasePCAPP AS mbapp ON mapp.BaseMediaID = mbapp.RecID
                        JOIN MediaCategory AS mc ON mc.MediaType = 14002
                                                    AND mc.WxID = mbapp.RecID
                        JOIN dbo.Publish_BasicInfo AS pbi ON pbi.MediaType = 14002
                                                             AND mapp.MediaID = pbi.MediaID
															 AND pbi.Wx_Status = 49004
															 AND pbi.IsDel = 0
															 AND EXISTS (SELECT 1 FROM dbo.App_AdTemplate WHERE AuditStatus = 48002 AND Status = 0 AND pbi.TemplateID = dbo.App_AdTemplate.RecID)
					    								 
               WHERE    mapp.Status = 0
                        AND mbapp.Status = 0
						AND not exists 
						(
							select 1 from Media_CollectionBlacklist 
							where Media_CollectionBlacklist.MediaID = mapp.MediaID 
							and Media_CollectionBlacklist.Status = 0 
							and Media_CollectionBlacklist.MediaType = 14002 
							and Media_CollectionBlacklist.RelationType = 2 
							and Media_CollectionBlacklist.CreateUserID = {0}
						)
               GROUP BY mc.CategoryID
             ) AS b ON di.DictId = b.CategoryID
WHERE   di.DictType > 0
        AND di.DictType = 52
        AND di.Status = 0
ORDER BY b.C DESC;",Common.UserInfo.GetLoginUserID());

            DataSet ds = SqlHelper.ExecuteDataset(SYSCONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }
    }
}
