using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Dal.APP;
using XYAuto.ITSC.Chitunion2017.Entities.APP;

namespace XYAuto.ITSC.Chitunion2017.BLL.APP
{
    public class FeedbackBll
    {
        #region 单例
        private FeedbackBll() { }

        public static FeedbackBll instance = null;
        public static readonly object padlock = new object();

        public static FeedbackBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new FeedbackBll();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        /// <summary>
        /// 添加反馈信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public int AddFeedbackInfo(FeedbackModel feedBackinfo)
        {
            return FeedbackDa.Instance.AddFeedbackInfo(feedBackinfo);
        }
    }
}
