/********************************************************
*创建人：hantao
*创建时间：2017/10/11
*说明：广点通同步用户信息
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Data;

namespace XYAuto.BUOC.BOP2017.BLL.GDT
{
    public class GDTDeriveUserInfo
    {
        #region Instance
        public static readonly GDTDeriveUserInfo Instance = new GDTDeriveUserInfo();
        #endregion

        public int Insert(Entities.GDT.GDTDeriveUserInfo entity)
        {
            return Dal.GDT.GDTDeriveUserInfo.Instance.Insert(entity);
        }

        /// <summary>
        /// 获取最大ID，用户同步用户信息
        /// </summary>
        /// <returns></returns>
        public int SelectMaxId()
        {
            return Dal.GDT.GDTDeriveUserInfo.Instance.SelectMaxId();
        }

        /// <summary>
        /// 往智慧云推送
        /// </summary>
        /// <param name="deriveUserInfoId"></param>
        /// <param name="status"></param>
        public void UpdaetDeriveUserInfoByKeyId(int deriveUserInfoId, int status)
        {
            Dal.GDT.GDTDeriveUserInfo.Instance.UpdaetDeriveUserInfoByKeyId(deriveUserInfoId, status);
        }

        public void UpdaetDeriveUserInfoByClueId(int clueId, int status)
        {
            Dal.GDT.GDTDeriveUserInfo.Instance.UpdaetDeriveUserInfoByClueId(clueId, status);
        }

        /// <summary>
        /// 获取小去重推送线索
        /// </summary>
        /// <param name="status"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public DataSet GetClueIds(int status, DateTime time)
        {
            return Dal.GDT.GDTDeriveUserInfo.Instance.GetClueIds(status,time);
        }
    }
}
