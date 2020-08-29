/********************************************************
*创建人：hant
*创建时间：2017/12/19 16:06:55 
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
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.TaskInfo
{
    public class Material :DataBase
    {
        public static readonly Material Instance = new Material();



        /// <summary>
        /// 获取任务素材列表
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        public DataSet GetTaskMaterialListHeadAndFoot(int recid)
        {
            var sql = @"
                    SELECT [TaskName] AS Title,[MaterialUrl],ME.Content,MEF.FootContentUrl   
                    FROM [Chitunion2017].[dbo].[LE_TaskInfo] T
                    INNER JOIN Chitunion_OP2017..MaterielExtend ME ON T.MaterialID = ME.MaterielID
                    LEFT JOIN Chitunion_OP2017..MaterielExtendFoot MEF ON ME.MaterielID = MEF.MaterielID
                    WHERE T.RecID = @recid";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@recid",recid)
            };
            var data = SqlHelper.ExecuteDataset(ConnectChitunion2017, CommandType.Text, sql, parameters.ToArray());
            return data;
        }

        public List<Entities.Task.TaskMaterialListWaist> GetTaskMaterialListWaist(int recid)
        {
            var sql = @"
                    SELECT MEB.Title,MEB.HeadImg,MEB.Content
                    FROM [Chitunion2017].[dbo].[LE_TaskInfo] T
                    INNER JOIN Chitunion_OP2017..MaterielExtendBody MEB ON T.MaterialID = MEB.MaterielID
                    WHERE T.RecID = @recid";
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter("@recid",recid)
            };
            var data = SqlHelper.ExecuteDataset(ConnectChitunion2017, CommandType.Text, sql, parameters.ToArray());
            return DataTableToList<Entities.Task.TaskMaterialListWaist>(data.Tables[0]);
        }
    }
}
