using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1
{
    public class RequestBusinessTypeDto
    {
        [Necessary(MtName = "BusinessType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入合法的BusinessType")]
        public int BusinessType { get; set; }

        [Necessary(MtName = "OperateType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int OperateType { get; set; }//1：添加（是从授权之后调整到页面、查询是基础表信息） 2：编辑（从后台媒体列表跳转到页面、查询是副表信息）
    }

    public class RequestMediaDto : RequestBusinessTypeDto
    {
        public RequestWeiXinDto WeiXin { get; set; }
        public RequestAppDto App { get; set; }
        public RequestTemplateDto Temp { get; set; }
    }

    public class RequestGetMeidaInfoDto
    {
        [Necessary(MtName = "BusinessType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int BusinessType { get; set; }

        [Necessary(MtName = "OperateType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int OperateType { get; set; }//1：添加（是从授权之后调整到页面、查询是基础表信息） 2：编辑（从后台媒体列表跳转到页面、查询是副表信息）

        [Necessary(MtName = "RecId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1}")]
        public int RecId { get; set; }//微信基础表id，微信媒体表id

        public int CreateUserId { get; set; }

        public bool IsAuditPass { get; set; }//是否审核通过

        public int MediaId { get; set; }
        public int BaseMediaId { get; set; }
        public int AdTempId { get; set; }
        public int AdBaseTempId { get; set; }

        public int Version { get; set; }

        public string MediaName { get; set; }

        public string Number { get; set; }

        public int PageSize { get; set; } = 10;
    }

    public class RequestMeidaVerifyOfAdd
    {
        //[Necessary(MtName = "BusinessType", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入合法的BusinessType")]
        public int BusinessType { get; set; }

        [Necessary(MtName = "MediaName媒体名称")]
        public string MediaName { get; set; }//媒体名称

        public int MediaId { get; set; }
    }
}