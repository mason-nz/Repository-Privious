﻿/********************************************************
*创建人：lixiong
*创建时间：2017/11/24 10:02:05
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.BLL.Statistics.Provider.Dto.Response.Grab
{
    /// <summary>
    /// 腰部和头部基本一致，可继承
    /// </summary>
    public class RespGrabBodyDto : RespGrabHeadDto
    {
    }

    /// <summary>
    /// 抓取的腰部文章在渠道上的分布  和 头部基本一致，可继承
    /// </summary>
    public class RespGrabBodyQuDaoDto : RespGrabHeadQuDao
    {
    }

    /// <summary>
    /// 抓取的腰部文章在文章类别上的分布 和 头部基本一致，可继承
    /// </summary>
    public class RespGrabBodyCjDto : RespGrabHeadCjDto
    {
    }
}