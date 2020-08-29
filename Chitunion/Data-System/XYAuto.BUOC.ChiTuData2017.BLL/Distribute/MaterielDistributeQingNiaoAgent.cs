/********************************************************
*创建人：lixiong
*创建时间：2017/11/20 13:30:52
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Distribute
{
    public class MaterielDistributeQingNiaoAgent
    {
        #region Instance

        public static readonly MaterielDistributeQingNiaoAgent Instance = new MaterielDistributeQingNiaoAgent();

        #endregion Instance

        public int IsExist(DateTime queryDate)
        {
            return Dal.Distribute.MaterielDistributeQingNiaoAgent.Instance.IsExist(queryDate);
        }
    }
}