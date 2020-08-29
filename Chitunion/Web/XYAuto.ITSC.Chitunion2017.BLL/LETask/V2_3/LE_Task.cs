/********************************************************
*创建人：hant
*创建时间：2018/1/12 14:28:05 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Activity;
using XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.Withdrawals;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.V2_3
{
    public class LE_Task
    {
        public static readonly LE_Task Instance = new LE_Task();

        /// <summary>
        /// M领取任务列表
        /// </summary>
        /// <param name="res"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public Entities.DTO.V2_3.TaskRspDTO GetDataByPage(Entities.DTO.V2_3.TaskResDTO res, out int totalCount)
        {
            Entities.DTO.V2_3.TaskRspDTO entity = Dal.LETask.LeTaskInfo.Instance.GetDataByPage(res, out totalCount);
            //if (entity != null && entity.TaskInfo.Count > 0)
            //{
            //    foreach (var item in entity.TaskInfo)
            //    {
            //        Uri url = new Uri(item.MaterialUrl);
            //        item.MaterialUrl = item.MaterialUrl.Replace("http://" + url.Host, XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("Domin"));
            //    }
            //}
            return entity;
        }

        public int GetTaskIdByMaterialID(int MaterialID)
        {
            return Dal.LETask.LeTaskInfo.Instance.GetTaskIdByMaterialID(MaterialID);
        }
        #region V2.5
        public Entities.DTO.V2_3.TaskRspDTO GetDataByPageV2_5(Entities.DTO.V2_3.TaskResDTO res)
        {
            if (res.IsNewUser)
            {
                var config = ActivityConfigManage.GetActivityConfig();
                res.BeginTime = config.BeginTime.ToString("yyyy-MM-dd");
            }
            var randomNumber = Utils.Config.ConfigurationUtil.GetAppSettingValue("GetRandomNumber", false) ?? "21,155.22";
            Entities.DTO.V2_3.TaskRspDTO entity = Dal.LETask.LeTaskInfo.Instance.GetDataByPageV2_5(res, randomNumber);
            foreach (var task in entity.TaskInfo)
            {
                task.MaterialUrl = ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.GetDomainByRandom_ShareArticle(task.MaterialUrl);
            }
            //foreach (var task in entity.TaskInfo)
            //{
            //    task.MaterialUrl = ITSC.Chitunion2017.BLL.ShareOrderInfo.Instance.GetDomainByRandom_ShareArticle(task.MaterialUrl);                
            //}
            return entity;
        }
        #endregion
    }
}
