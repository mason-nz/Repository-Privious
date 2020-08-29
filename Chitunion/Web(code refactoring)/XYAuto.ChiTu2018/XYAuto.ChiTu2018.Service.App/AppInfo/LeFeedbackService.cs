/********************************
* 项目名称 ：XYAuto.ChiTu2018.Service.App.AppInfo
* 类 名 称 ：LeFeedbackService
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/23 17:13:40
********************************/
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;
using XYAuto.ChiTu2018.Infrastructure.AutoMapper;
using XYAuto.ChiTu2018.Service.App.AppInfo.Dto;

namespace XYAuto.ChiTu2018.Service.App.AppInfo
{
    public class LeFeedbackService
    {
        #region 单例

        private LeFeedbackService() { }
        private static readonly Lazy<LeFeedbackService> Linstance = new Lazy<LeFeedbackService>(() => { return new LeFeedbackService(); });

        public static LeFeedbackService Instance => Linstance.Value;

        #endregion

        public int AddFeedbackInfo(LeFeedbackDto feedBackinfo)
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<string, DateTime?>().ConvertUsing(s => string.IsNullOrEmpty(s) ? DateTime.Now : Convert.ToDateTime(s));
            });
            return new LeFeedbackBo().AddFeedbackInfo(Mapper.Map<LeFeedbackDto, LE_Feedback>(feedBackinfo, new LE_Feedback { CreateTime = DateTime.Now, ReplyTime = DateTime.Now }));
        }
    }
}
