using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;

namespace XYAuto.ITSC.Chitunion2017.BLL.LETask.Provider.Dto.Request
{
    public class ReqTaskStorageDto
    {
        [Necessary(MtName = "TaskName")]
        public string TaskName { get; set; }

        [Necessary(MtName = "MaterialUrl")]
        public string MaterialUrl { get; set; }

        [Necessary(MtName = "MaterialId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入物料Id")]
        public int MaterialId { get; set; }

        public int TaskType { get; set; } = (int) LeTaskTypeEnum.ContentDistribute;

        [Necessary(MtName = "ImgUrl")]
        public string ImgUrl { get; set; }

        //[Necessary(MtName = "Synopsis")]
        public string Synopsis { get; set; }

        //[Necessary(MtName = "CategoryId", IsValidateThanAt = true, ThanMaxValue = 0, Message = "{0}必须大于{1},请输入CategoryId")]
        public int CategoryId { get; set; }

        /// <summary>
        /// 指定任务的金额
        /// </summary>
        public decimal TaskMoney { get; set; }

    }
}
