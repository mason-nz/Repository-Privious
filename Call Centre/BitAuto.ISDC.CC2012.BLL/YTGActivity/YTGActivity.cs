using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class YTGActivity : CommonBll
    {
        private YTGActivity() { }
        private static YTGActivity _instance = null;
        public static new YTGActivity Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new YTGActivity();
                }
                return _instance;
            }
        }

        /// 查询车型列表
        /// <summary>
        /// 查询车型列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<Entities.CarSerial> GetCarSerialsByIds(string ids)
        {
            return BLL.CarSerial.Instance.GetCarSerial(ids);
        }
        /// 查询车型的ID
        /// <summary>
        /// 查询车型的ID
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<string> GetCarSerialIDsByIds(string ids)
        {
            List<Entities.CarSerial> list = BLL.CarSerial.Instance.GetCarSerial(ids);
            List<string> result = new List<string>();
            foreach (Entities.CarSerial cars in list)
            {
                result.Add(cars.CSID.ToString());
            }
            return result;
        }
        /// 终止项目
        /// <summary>
        /// 终止项目
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns></returns>
        public int EndProjectForYTGActivity(string activityid)
        {
            DataTable dt = Dal.YTGActivity.Instance.GetProjectForYTGActivity(activityid);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BLL.ProjectLog.Instance.InsertProjectLog(CommonFunction.ObjectToLong(dr["ProjectID"]), ProjectLogOper.L4_结束项目, "结束项目-" + CommonFunction.ObjectToString(dr["Name"]), null, -1);
                }
            }
            return Dal.YTGActivity.Instance.EndProjectForYTGActivity(activityid);
        }
        /// 终止任务
        /// <summary>
        /// 终止任务
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns></returns>
        public int EndTaskForYTGActivity(string activityid, string remark)
        {
            return Dal.YTGActivity.Instance.EndTaskForYTGActivity(activityid, remark);
        }
        /// 获取到期未关闭的活动
        /// <summary>
        /// 获取到期未关闭的活动
        /// </summary>
        /// <returns></returns>
        public DataTable GetMaturityNoCloseActivity(DateTime d)
        {
            return Dal.YTGActivity.Instance.GetMaturityNoCloseActivity(d);
        }
        /// 获取最大报名人ID
        /// <summary>
        /// 获取最大报名人ID
        /// </summary>
        /// <returns></returns>
        public int GetMaxSingUserID()
        {
            return Dal.YTGActivity.Instance.GetMaxSingUserID();
        }
        /// 根据城市获取省
        /// <summary>
        /// 根据城市获取省
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public int GetAreaParentID(int aid)
        {
            return Dal.YTGActivity.Instance.GetAreaParentID(aid);
        }
        /// 根据车款获取品牌和车型
        /// <summary>
        /// 根据车款获取品牌和车型
        /// </summary>
        /// <param name="carid"></param>
        /// <returns></returns>
        public string[] GetCarInfo(int carid)
        {
            return Dal.YTGActivity.Instance.GetCarInfo(carid);
        }
        /// 获取活动数据
        /// <summary>
        /// 获取活动数据
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable GetYTGActivityByStauts(int status)
        {
            return Dal.YTGActivity.Instance.GetYTGActivityByStauts(status);
        }
    }
}
