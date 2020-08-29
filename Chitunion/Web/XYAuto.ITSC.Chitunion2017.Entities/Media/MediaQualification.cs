/********************************************************
*创建人：lixiong
*创建时间：2017/5/12 18:24:37
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    public class MediaQualification
    {
        public MediaQualification()
        {
            CreateTime = DateTime.Now;
        }

        //自增ID
        public int RecID { get; set; }

        //媒体Id
        public int MediaID { get; set; }

        //企业名称
        public string EnterpriseName { get; set; }

        //资质1
        public string QualificationOne { get; set; }

        //资质2
        public string QualificationTwo { get; set; }

        //营业执照
        public string BusinessLicense { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; }

        //创建人
        public int CreateUserID { get; set; }

        //状态
        public int Status { get; set; }

        public string IDCardFrontURL { get; set; }
        public string IDCardBackURL { get; set; }
        public string AgentContractFrontURL { get; set; }
        public string AgentContractBackURL { get; set; }
        public int MediaRelations { get; set; }
        public int OperatingType { get; set; }
        public int MediaType { get; set; }
        public string MediaRelationsName { get; set; }
        public string OperatingTypeName { get; set; }
        public int AuditStatus { get; set; }
    }
}