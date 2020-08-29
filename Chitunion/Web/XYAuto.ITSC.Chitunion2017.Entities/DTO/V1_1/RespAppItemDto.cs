/********************************************************
*创建人：lixiong
*创建时间：2017/6/3 10:32:51
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1
{
    public class RespAppItemDto : RespGetWeiXinBaseDto
    {
        public int BaseMediaID { get; set; }
        public List<CoverageAreaDto> CoverageArea { get; set; }//覆盖区域
        public List<CommonlyClassDto> CommonlyClass { get; set; }//常见分类
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int DailyLive { get; set; }//日活
        public string Remark { get; set; }//媒体介绍
        public int AdTemplateId { get; set; }//来验证是否已存在模板
        public int AuditStatus { get; set; }
        public List<OrderRemarkDto> OrderRemark { get; set; }//下单备注
    }

    public class CoverageAreaDto
    {
        public CoverageAreaDto()
        {
            ProvinceId = Entities.Constants.Constant.INT_INVALID_VALUE;
            CityId = Entities.Constants.Constant.INT_INVALID_VALUE;
        }

        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
    }

    public class CommonlyClassDto
    {
        public CommonlyClassDto()
        {
            CategoryId = Entities.Constants.Constant.INT_INVALID_VALUE;
        }

        public int CategoryId { get; set; }
        public int SortNumber { get; set; }
        public string CategoryName { get; set; }
    }

    public class OrderRemarkDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descript { get; set; }
    }
}