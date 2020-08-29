/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 17:15:44
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.GDT
{
    public class GdtAccessToken
    {
        public int Id { get; set; }
        public int RelationType { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ClientId { get; set; }
        public string ClientSecret { get; set; }
        public int AccessTokenExpiresIn { get; set; }

        public int RefreshTokenExpiresIn { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserId { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}