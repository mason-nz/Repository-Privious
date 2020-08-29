﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.DTO.V2_3
{
    public class ModifyAttestationReqDto : ModifyMobileReqDto
    {
        public int Type { get; set; }
        public int CheckCode { get; set; }
        public string TrueName { get; set; }
        public string BLicenceURL { get; set; }
        public string IdentityNo { get; set; }
        public string IDCardFrontURL { get; set; }
        public int Sex { get; set; } = -2;


    }
}
