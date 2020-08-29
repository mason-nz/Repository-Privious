/********************************************************
*创建人：lixiong
*创建时间：2017/11/20 13:21:15
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;

namespace XYAuto.BUOC.ChiTuData2017.PullStatisticsData.Scheduler.Registry
{
    public class ZhyPullDistributeNoteScheduler : IJob
    {
        public void Execute()
        {
            Loger.Log4Net.Info($" 检测是否有分发明细任务 start ...");
            var queryDate = DateTime.Now.AddDays(-1);
            var count = BLL.Distribute.MaterielDistributeQingNiaoAgent.Instance.IsExist(queryDate);

            if (count <= 0)
            {
                EmailNotes.SendWarin(queryDate.ToString("yyyy-MM-dd"));
                Loger.Log4Net.Info($" 检测是否有分发明细任务 结果：日期：{queryDate.ToString("yyyy-MM-dd")} 没有分发数据产生 ...");
            }
            Loger.Log4Net.Info($" 检测是否有分发明细任务 end ...");
        }
    }
}