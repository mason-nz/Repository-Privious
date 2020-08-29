using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class MediaOrderInfo
    {
        public static readonly MediaOrderInfo Instance = new MediaOrderInfo();
        #region 新增媒体项目中间表
        public int Insert(Entities.MediaOrderInfo model)
        {
            return Dal.MediaOrderInfo.Instance.Insert(model);
        }
        #endregion
        #region 根据项目号删除
        public void DeleteByOrderID(string orderid)
        {
            Dal.MediaOrderInfo.Instance.DeleteByOrderID(orderid);
        }
        #endregion
    }
}
