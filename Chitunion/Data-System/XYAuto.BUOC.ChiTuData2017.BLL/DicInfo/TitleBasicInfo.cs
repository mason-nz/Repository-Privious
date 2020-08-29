/********************************************************
*创建人：lixiong
*创建时间：2017/9/12 19:56:53
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Entities.Enum;

namespace XYAuto.BUOC.ChiTuData2017.BLL.DicInfo
{
    public class TitleBasicInfo
    {
        public static readonly TitleBasicInfo Instance = new TitleBasicInfo();

        /// <summary>
        /// 获取父类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Entities.Lable.TitleBasicInfo> GeTitleBasicInfos(LableTypeEnum type)
        {
            return Dal.Lable.TitleBasicInfo.Instance.GeTitleBasicInfos(type);
        }

        /// <summary>
        /// 获取子IP
        /// </summary>
        /// <param name="ipId"></param>
        /// <returns></returns>
        public List<Entities.Lable.TitleBasicInfo> GetChildrenLable(int ipId)
        {
            return Dal.Lable.TitleBasicInfo.Instance.GetChildrenLable(ipId);
        }
    }
}