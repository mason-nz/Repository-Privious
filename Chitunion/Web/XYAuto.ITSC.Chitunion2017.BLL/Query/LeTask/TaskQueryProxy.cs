using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto.ResponseDto;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Request.AdOrder;
using XYAuto.ITSC.Chitunion2017.BLL.Query.Dto.Response.Task;
using XYAuto.ITSC.Chitunion2017.Entities.Enum.LeTask;
using XYAuto.ITSC.Chitunion2017.Entities.Query;

namespace XYAuto.ITSC.Chitunion2017.BLL.Query.LeTask
{
    public class TaskQueryProxy
    {
        private readonly ReqOrderCoverImageDto _reqTaskQueryDto;

        public TaskQueryProxy(ReqOrderCoverImageDto reqTaskQueryDto)
        {
            _reqTaskQueryDto = reqTaskQueryDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BaseResponseEntity<RespReceiveTaskInfoDto> GetCoverImageList()
        {
            _reqTaskQueryDto.TaskType = LeTaskTypeEnum.CoverImage;
            return new TaskRecCoverImageQuery(new ConfigEntity()).GetQueryList(_reqTaskQueryDto);
        }
    }
}
