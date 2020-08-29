/********************************************************
*创建人：lixiong
*创建时间：2017/6/3 17:26:28
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1
{
    public class RequestAppDto
    {
        public int MediaID { get; set; }
        public int BaseMediaID { get; set; }

        [Necessary(MtName = "名称")]
        public string Name { get; set; }

        [Necessary(MtName = "头像url")]
        public string HeadIconURL { get; set; }

        public int DailyLive { get; set; }//日活
        public string Remark { get; set; }//媒体介绍
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public List<CommonlyClassDto> CommonlyClass { get; set; }//常见分类
        public List<CoverageAreaDto> CoverageArea { get; set; }//覆盖区域
        public List<OrderRemarkDto> OrderRemark { get; set; }//下单备注
        public RequestQualificationDto Qualification { get; set; }//资质信息
        public int CreateUserId { get; set; }
        public int Source { get; set; }
    }

    public class RequestQualificationDto
    {
        public string EnterpriseName { get; set; }

        public string BusinessLicense { get; set; }//     //营业执照
        public string IDCardFrontURL { get; set; }//身份证正面
        public string IDCardBackURL { get; set; }//身份证反面

        //public string BLicenceFile { get; set; }//营业执照附件
        public string AgentContractFrontURL { get; set; }//代理合同正面

        public string AgentContractBackURL { get; set; }//代理合同反面

        [Necessary(MtName = "MediaRelations", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int MediaRelations { get; set; }//媒体关系(50001:代理  50002:自有    50003:自营)

        [Necessary(MtName = "OperatingType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int OperatingType { get; set; }//运营类型
    }
}