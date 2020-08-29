/********************************
* 项目名称 ：XYAuto.ChiTu2018.BO.LE
* 类 名 称 ：LeFeedbackBo
* 描    述 ：
* 作    者 ：zhangjl
* 创建时间 ：2018/5/23 17:08:19
********************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ChiTu2018.BO.RegisterConfig;
using XYAuto.ChiTu2018.DAO.Chitunion2017.LE;
using XYAuto.ChiTu2018.Entities.Chitunion2017.LE;

namespace XYAuto.ChiTu2018.BO.LE
{
    public class LeFeedbackBo
    {
        private readonly ILeFeedback _leFeedback;
        public LeFeedbackBo()
        {
            _leFeedback = IocMannager.Instance.Resolve<ILeFeedback>();
        }
        public int AddFeedbackInfo(LE_Feedback entity)
        {
            var retEntity = _leFeedback.Add(entity);
            return retEntity?.RecID ?? 0;
        }
    }
}
