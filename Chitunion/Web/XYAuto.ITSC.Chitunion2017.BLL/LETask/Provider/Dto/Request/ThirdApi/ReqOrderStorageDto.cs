using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request.ThirdApi
{
    public class ReqOrderStorageDto
    {
        [Necessary(MtName = "TaskId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入TaskId")]
        public int TaskId { get; set; }

        //[Necessary(MtName = "UserIdentity")]
        public string UserIdentity { get; set; }

        [Necessary(MtName = "ChannelId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入ChannelId")]
        public int ChannelId { get; set; }

        //[Necessary(MtName = "UserId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入UserId")]
        public int UserId { get; set; }

        public string OrderUrl { get; set; }
    }
}
