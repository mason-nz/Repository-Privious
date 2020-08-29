/********************************************************
*创建人：hant
*创建时间：2018/1/12 17:07:37 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.Utils.Data;

namespace XYAuto.ITSC.Chitunion2017.Dal
{
    public class WXUserScene:DataBase
    {
        public static readonly WXUserScene Instance = new WXUserScene();

        public List<Entities.DTO.V2_3.WXUserSceneRspDTO> GetUserSceneByUserId(int userid)
        {
            var sql = $@" SELECT SI.[SceneID]
                          ,SI.[SceneName],COUNT(B.CategoryID) AS Counts
	                       ,CASE WHEN (SELECT COUNT(US.[SceneID]) FROM [Chitunion2017].[dbo].[LE_WXUserScene] US WITH(NOLOCK) 
	                        WHERE SI.[SceneID] = US.[SceneID] AND [UserID]= @UserID AND [Status]=0)>0 THEN 1 ELSE 0 END AS IsSelected
                        FROM  [Chitunion_OP2017].[dbo].[DictScene] SI WITH(NOLOCK)
                         LEFT JOIN (SELECT CategoryID FROM Chitunion2017..LE_TaskInfo TI WITH(NOLOCK) WHERE TI.Status=194001)
						B ON SI.SceneID = B.CategoryID
                        WHERE SI.ParenID IS NOT NULL AND  SI.ParenID>0
                        GROUP BY  SI.[SceneID],SI.[SceneName]
                        ORDER BY COUNT(B.CategoryID) DESC";
           
            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID",userid),
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, sqlParams);
            return DataTableToList<Entities.DTO.V2_3.WXUserSceneRspDTO>(data.Tables[0]);
        }


        /// <summary>
        /// 事务更新用户场景
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public bool UpdateWeiXinUserScene(int userid,Entities.DTO.V2_3.WXUserSceneResDTO res)
        {
            bool result = false;
            using (SqlConnection cnn = new SqlConnection(CONNECTIONSTRINGS))
            {
                cnn.Open();
                SqlTransaction trans = cnn.BeginTransaction();
                SqlCommand cm = cnn.CreateCommand();
                cm.Transaction = trans;
                try
                {
                    #region 删除选择跳过
                    var model = Dal.WeChat.LE_WXUserScene.Instance.GetModel(new Entities.WeChat.LE_WXUserScene() { UserID = userid, SceneID = -3 });
                    if (model != null)
                    {
                        Dal.WeChat.LE_WXUserScene.Instance.Delete(model, trans);
                    }
                    
                    #endregion
                    #region 删除原有场景
                    var sql = @" UPDATE [Chitunion2017].[dbo].[LE_WXUserScene]
                                SET [Status]=-1 WHERE [Status]=0 AND [UserID]=@UserID";
                    var parameters = new List<SqlParameter> {
                new SqlParameter("@UserID", userid),
               
                };

                    #endregion

                    cm.CommandText = sql;
                    cm.Parameters.AddRange(parameters.ToArray());
                    cm.ExecuteScalar();

                    #region 插入场景
                    if (res.SceneInfo != null && res.SceneInfo.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("UserID", typeof(int));
                        dt.Columns.Add("SceneID", typeof(int));
                        dt.Columns.Add("SceneName", typeof(string));
                        dt.Columns.Add("CreateTime", typeof(DateTime));
                        dt.Columns.Add("Status", typeof(int));
                        foreach (var item in res.SceneInfo)
                        {
                            dt.Rows.Add(userid, item.SceneID, item.SceneName,DateTime.Now,0);
                        }
                        SqlBulkCopyByDataTable(cnn, trans, "LE_WXUserScene", dt);
                    }
                    
                    #endregion

                    trans.Commit();
                    result = true;

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
                finally
                {
                    cnn.Close();
                    trans.Dispose();
                    cnn.Dispose();
                }
                return result;

            }
        }

        #region V2.5
        public DataTable GetUserSceneByUserIdV2_5(int userid)
        {
            var sql = $@"SELECT SI.[SceneID] ,
                                SI.[SceneName] ,
                                COUNT(B.CategoryID) AS Counts ,
                                CASE WHEN ( SELECT  COUNT(US.[SceneID])
                                            FROM    [Chitunion2017].[dbo].[LE_WXUserScene] US WITH ( NOLOCK )
                                            WHERE   SI.[SceneID] = US.[SceneID]
                                                    AND [UserID] = @UserID
                                                    AND [Status] = 0
                                          ) > 0 THEN 1
                                     ELSE 0
                                END AS IsSelected
                        FROM    [Chitunion_OP2017].[dbo].[DictScene] SI WITH ( NOLOCK )
                                LEFT JOIN ( SELECT  CategoryID
                                            FROM    Chitunion2017..LE_TaskInfo TI WITH ( NOLOCK )
                                            WHERE   TI.Status = 194001
                                          ) B ON SI.SceneID = B.CategoryID
                        WHERE   SI.ParenID>0 
                        GROUP BY SI.[SceneID] ,
                                SI.[SceneName]
                        ORDER BY COUNT(B.CategoryID) DESC";

            var sqlParams = new SqlParameter[]{
                new SqlParameter("@UserID",userid),
            };
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql, sqlParams);
            return data.Tables[0];
        }
        #endregion
        #region V2.8
        /// <summary>
        /// 根据场景ID获取场景名称
        /// </summary>
        /// <param name="sceneID"></param>
        /// <returns></returns>
        public Entities.UserScene GetSceneByID(int sceneID)
        {
            var sql = $@"SELECT SceneID,SceneName FROM Chitunion_OP2017.dbo.DictScene WHERE SceneID={sceneID}";           
            var data = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            return DataTableToEntity<Entities.UserScene>(data.Tables[0]);
        }
        #endregion
    }
}
