using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.MediaLabel.DTO.ResponseDto.V1_2_4
{
    public class ResViewBatchMediaInfoDto
    {
        public int MediaType { get; set; } = -2;
        public string Name { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string HeadImg { get; set; } = string.Empty;
        public string HomeUrl { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
        public DateTime AuditTime { get; set; } = new DateTime(1900, 1, 1);
        public string OperateInfoUserName { get; set; } = string.Empty;
        public string AuditInfoUserName { get; set; } = string.Empty;
    }
    public class ResViewBatchMediaDto
    {
        public int MediaType { get; set; } = -2;
        public string Name { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public string HeadImg { get; set; } = string.Empty;
        public string HomeUrl { get; set; } = string.Empty;
        public Category Category { get; set; }
        public Category MarketScene { get; set; }
        public Category DistributeScene { get; set; }
        public IPCategory IPLabel { get; set; }
        public Operateinfo OperateInfo { get; set; }
        public Operateinfo AuditInfo { get; set; }
        public string ArticleIDs { get; set; }
        public bool IpIsSame { get; set; } = false;
    }    

    public class Category
    {
        public List<ResViewBatchMediaDictDto> Original { get; set; }
        public List<ResViewBatchMediaDictDto> Audit { get; set; }
    }

    public class ResViewBatchMediaDictDto
    {
        public int DictId { get; set; } = -2;
        public int LabelID { get; set; } = -2;
        public string DictName { get; set; } = string.Empty;
        public int Type { get; set; } = -2;
        public int SubIPID { get; set; } = -2;
    }

    public class IPCategory
    {
        public List<IPLabel> Original { get; set; }
        public List<IPLabel> Audit { get; set; }
    }
    public class IPLabel
    {
        public int DictId { get; set; } = -2;
        public string DictName { get; set; } = string.Empty;
        public int LabelID { get; set; } = -2;
        public List<SubIPLabelDto> SubIP { get; set; }
    }

    public class SubIPLabelDto
    {
        public int DictId { get; set; } = -2;
        public string DictName { get; set; } = string.Empty;
        public int SubIPLabelID { get; set; } = -2;
        public List<LabelDto> Label { get; set; }
    }

    public class LabelDto
    {
        public int DictId { get; set; } = -2;
        public string DictName { get; set; } = string.Empty;
    }   
    public class Operateinfo
    {
        public string UserName { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = new DateTime(1900, 1, 1);
    }    
}
