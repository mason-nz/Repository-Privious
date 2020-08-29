/********************************************************
*创建人：lixiong
*创建时间：2017/10/25 10:16:26
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.BOP2017.Infrastruction.Verification;
using XYAuto.ChiTu.Excel;

namespace XYAuto.BUOC.BOP2017.BLL.GDT.Dto.Response
{
    public class RespExcelUserOrganizeDto
    {
        [ExcelTitle("UserName")]
        public string UserName { get; set; }

        [ExcelTitle("Mobile")]
        public string Mobile { get; set; }

        [ExcelTitle("ContactsPerson")]
        public string ContactsPerson { get; set; }

        [ExcelTitle("CorporationName")]
        public string CorporationName { get; set; }

        [ExcelTitle("OrganizeID")]
        public int OrganizeId { get; set; }
    }
}