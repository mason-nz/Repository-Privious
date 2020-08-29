using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class AppTemplate : DataBase
    {
        public static readonly AppTemplate Instance = new AppTemplate();
        /// <summary>
        /// 2017-06-06 zlb
        /// 删除自己的app广告模板
        /// </summary>
        /// <param name="TemplateID">模板ID</param>
        /// <param name="UserID">操作人ID</param>
        /// <param name="status">审核状态</param>
        /// <returns></returns>
        public int ToDeleteAppTemplate(int TemplateID, int UserID, int status)
        {
            string strSql = "Update App_AdTemplate set Status=-1 where  RecID=" + TemplateID + " and CreateUserID=" + UserID + " and AuditStatus=" + status;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, strSql);
        }
        /// <summary>
        /// 2017-06-22 zlb
        /// AE角色进行删除模板
        /// </summary>
        /// <param name="TemplateID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int ToDeleteAppTemplateAE(int TemplateID, int status)
        {
            string strSql = "Update App_AdTemplate  set Status=-1 where  RecID=" + TemplateID + " and (SELECT top 1 RoleID FROM  dbo.UserRole WHERE UserID=App_AdTemplate.CreateUserID)='SYS001RL00005' and AuditStatus=" + status;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, strSql);
        }

        /// <summary>
        /// 2017-06-06 zlb
        /// 删除模板下的刊例和广告单元
        /// </summary>
        /// <param name="TemplateID">模板ID</param>
        /// <returns></returns>
        public int ToDeletePublish(int TemplateID)
        {
            string strSql = "Update Publish_BasicInfo set IsDel=-1 where TemplateID=" + TemplateID;
            strSql += ";Update AppPriceInfo set Status=-1 where TemplateID=" + TemplateID;
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, System.Data.CommandType.Text, strSql);
        }
        /// <summary>
        ///zlb 2017-06-06
        ///查询已上架的城市
        /// </summary>
        /// <returns></returns>
        public DataTable SelectPublishCitys()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"SELECT  COUNT(1) as Citycount,UPPER(LEFT(AI.AbbrName,1)) AS FirstLetter,SR.CityID,AI.AreaName AS CityName FROM dbo.Publish_BasicInfo PB INNER JOIN AppPriceInfo AP ON PB.PubID=AP.PubID
INNER JOIN SaleAreaRelation SR ON AP.SaleArea = SR.GroupID INNER JOIN dbo.AreaInfo AI
ON SR.CityID = AI.AreaID
WHERE PB.Wx_Status = {0} AND PB.IsDel = 0 AND AP.Status = 0 AND SR.IsPublic=1
GROUP BY SR.CityID, LEFT(AI.AbbrName, 1), AI.AreaName
ORDER BY UPPER(LEFT(AI.AbbrName,1))", (int)AppPublishStatus.已上架);
            return SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, System.Data.CommandType.Text, sb.ToString()).Tables[0];
        }
    }
}
