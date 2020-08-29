/********************************************************
*创建人：lixiong
*创建时间：2017/7/24 16:58:05
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Query.Media;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media
{
    public class TwoBarCodeHistory
    {
        #region Instance

        public static readonly TwoBarCodeHistory Instance = new TwoBarCodeHistory();

        #endregion Instance

        #region Contructor

        protected TwoBarCodeHistory()
        { }

        #endregion Contructor

        public List<Entities.Media.TwoBarCodeHistory> GetList(
            TwoBarCodeHistoryQuery<Entities.Media.TwoBarCodeHistory> query)
        {
            return Dal.Media.TwoBarCodeHistory.Instance.GetList(query);
        }

        public List<RespChannelListDto> GetChannelList(ChannelQuery<RespChannelListDto> query)
        {
            return Dal.Media.TwoBarCodeHistory.Instance.GetChannelList(query);
        }
    }
}