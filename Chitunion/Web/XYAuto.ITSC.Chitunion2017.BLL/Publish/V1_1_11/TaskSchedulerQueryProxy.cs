/********************************************************
*创建人：lixiong
*创建时间：2017/8/30 11:24:29
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto.V1_1_11;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;

namespace XYAuto.ITSC.Chitunion2017.BLL.Publish.V1_1_11
{
    public class TaskSchedulerQueryProxy
    {
        private readonly ConfigEntity _configEntity;
        private readonly RequestTaskSchedulerDto _requestTaskSchedulerDto;

        public TaskSchedulerQueryProxy(ConfigEntity configEntity, RequestTaskSchedulerDto request)
        {
            _configEntity = configEntity;
            _requestTaskSchedulerDto = request;
        }

        public dynamic GetQuery()
        {
            //todo:如果是管理员-->查看全部的
            //todo:当前人-->查询的是自己的

            //组长-管理员查询

            if (_configEntity.RoleTypeEnum == RoleEnum.GroupLeader
                || _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin
                || _configEntity.RoleTypeEnum == RoleEnum.YunYingOperate)
            {
                return new MaterielTaskByAdminQuery(_configEntity).GetQueryList(_requestTaskSchedulerDto);
            }
            else
            {
                return new MaterielTaskQuery(_configEntity).GetQueryList(_requestTaskSchedulerDto);
            }
        }
    }
}