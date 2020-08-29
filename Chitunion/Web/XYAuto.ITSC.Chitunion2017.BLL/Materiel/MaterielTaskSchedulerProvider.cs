/********************************************************
*创建人：lixiong
*创建时间：2017/8/30 15:29:57
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Materiel.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Materiel.Dto.Response;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Business.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Publish.Dto;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Materiel;

namespace XYAuto.ITSC.Chitunion2017.BLL.Materiel
{
    public class MaterielTaskSchedulerProvider : CurrentOperateBase
    {
        private readonly ConfigEntity _configEntity;
        private readonly RequestSubmitCleanDto _contSubmitClean;
        private readonly RequestDistributeDto _contextDistribute;

        public MaterielTaskSchedulerProvider(ConfigEntity configEntity, RequestDistributeDto contextDistribute)
        {
            _configEntity = configEntity;
            _contextDistribute = contextDistribute;
        }

        public MaterielTaskSchedulerProvider(ConfigEntity configEntity, RequestSubmitCleanDto contSubmitClean)
        {
            _configEntity = configEntity;
            _contSubmitClean = contSubmitClean;
        }

        #region 分配任务与回收

        public ReturnValue DistributeAndDoRecovery()
        {
            var retValue = VerifyOfNecessaryParameters(_contextDistribute);
            if (retValue.HasError)
                return retValue;
            if (_contextDistribute.OperateType == (int)DistributeOperateType.Distribute)
                return DoDistribute(retValue);
            else if (_contextDistribute.OperateType == (int)DistributeOperateType.Recovery)
                return DoRecovery(retValue);
            else
            {
                return CreateFailMessage(retValue, "40001", $"请输入合法的OperateType");
            }
        }

        /// <summary>
        /// 分配任务到指定用户
        /// </summary>
        /// <returns></returns>
        private ReturnValue DoDistribute(ReturnValue retValue)
        {
            //todo:1.先判断groupId 是否已经存在分配记录

            if (_contextDistribute.UserId <= 0)
                return CreateFailMessage(retValue, "40003", $"请选择分配用户");

            //retValue = VerifyOfRoleModule(retValue, "");

            retValue = VerifyGroupId(retValue);
            if (retValue.HasError)
                return retValue;

            Dal.Materiel.TaskSchedulerUser.Instance.Insert(_contextDistribute.GroupIds.Split(',').ToList()
                , new Entities.Materiel.TaskSchedulerUser()
                {
                    CreateUserId = _configEntity.CreateUserId,
                    UserId = _contextDistribute.UserId
                });

            return retValue;
        }

        private ReturnValue DoRecovery(ReturnValue retValue)
        {
            //todo:删除关联关系,回收，只能操作 已分配状态下的，处理中,已处理的，等等是不能被回收的
            var recoverCount =
                Dal.Materiel.TaskSchedulerUser.Instance.UpdateStatusByRecovery(
                    _contextDistribute.GroupIds.Split(',').ToList());
            if (recoverCount <= 0)
            {
                return CreateFailMessage(retValue, "40002", "收回失败，任务都在处理无法收回");
            }
            return CreateSuccessMessage(retValue, "0", $"成功收回：{recoverCount} 条");
        }

        private ReturnValue VerifyGroupId(ReturnValue retValue)
        {
            if (_contextDistribute.OperateType != (int)DistributeOperateType.Distribute)
            {
                return retValue;
            }
            //todo:添加关联,校验
            var list = Dal.Materiel.TaskSchedulerUser.Instance.GetListByGroupId(0, _contextDistribute.GroupIds.Split(',').ToList());

            if (list.Any())
            {
                return CreateFailMessage(retValue, "40001",
                    $"选择项：{string.Join(",", list.Select(s => s.GroupId))}，已经关联，不允许重复被分配");
            }
            return retValue;
        }

        #endregion 分配任务与回收

        #region 作废

        /// <summary>
        /// 作废操作-只是把头部作废
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public ReturnValue DoAbandoned(int groupId)
        {
            //todo:删除头部关联关系，
            var retValue = new ReturnValue();
            var groupInfo = Dal.Materiel.TaskSchedulerUser.Instance.GeTrGroupInfo(groupId);
            if (groupInfo == null)
                return CreateFailMessage(retValue, "10019", $"当前groupId：{groupId} 信息不存在");

            retValue = VerifySubmitTaskInfo(retValue, groupId);
            if (retValue.HasError)
                return retValue;

            var excuteCount =
                Dal.Materiel.TaskSchedulerUser.Instance.UpdateStatusByRecovery(new List<string>() { groupId.ToString() },
                    TaskStatusEnum.Useless, TaskStatusEnum.Processing);
            if (excuteCount <= 0)
            {
                return CreateFailMessage(retValue, "10020", "作废操作-失败");
            }
            //带出下一条数据
            retValue.ReturnObject = GetNextGroupId(groupId);
            return retValue;
        }

        #endregion 作废

        #region 获取下一条清洗数据

        public int GetNextGroupId(int groupId)
        {
            //todo:1.根据当前用户的角色
            //todo:2.组长，管理员，直接找出下一条数据
            //todo:3.清洗员，则只能找到自己分配的任务对应的GroupId
            //_configEntity.CreateUserId;
            if (_configEntity.RoleTypeEnum == RoleEnum.GroupLeader
                || _configEntity.RoleTypeEnum == RoleEnum.SupperAdmin
                || _configEntity.RoleTypeEnum == RoleEnum.YunYingOperate)
            {
                //权限足够
                return 0;
            }
            else
            {
                var info = Dal.Materiel.TaskSchedulerUser.Instance.GetNextGroupIdByTaskUser(_configEntity.CreateUserId, groupId);
                return info != null ? info.GroupId : 0;
            }
        }

        #endregion 获取下一条清洗数据

        #region 清洗提交

        public ReturnValue SubmitCleanInfo()
        {
            //todo:1.删除的腰部文章也要收集起来，参数集合（文章id）
            //todo:2.保存-先删除需要删除的文章关联（腰部），status=-1，然后查出当前groupId下的腰部
            //todo:3.一个groupId 下面不能有重复的ArticleID（得校验）
            //todo:4.将最新的部分添加（排除提交过来的，与删除剩下的交集之外的）

            var retValue = new ReturnValue();

            retValue = VerifySubmitParams(retValue);
            if (retValue.HasError)
                return retValue;

            retValue = VerifySubmitTaskInfo(retValue, _contSubmitClean.GroupId);
            if (retValue.HasError)
                return retValue;
            var deleteList = _contSubmitClean.DeleteList?.ToList() ?? new List<int>();
            var groupInfo = Dal.Materiel.TaskSchedulerUser.Instance.GeTrGroupInfo(_contSubmitClean.GroupId);
            if (groupInfo == null)
                return CreateFailMessage(retValue, "10016", $"当前groupId：{_contSubmitClean.GroupId} 信息不存在");
            //删除之后的组合关联（包含头部和腰部）
            var groupArticleList = Dal.Materiel.TaskSchedulerUser.Instance.GetGroupArticleList(_contSubmitClean.GroupId,
               deleteList);

            //校验是否存在重复的文章组合
            var bodyArticleIdList = _contSubmitClean.Body.Select(s => s.ArticleId).ToList();
            var intersectList = groupArticleList.Select(s => s.ArticleId).Intersect(bodyArticleIdList).ToList();
            //和参数body里面的集合取差集（剩下来的就是要新增的）
            var insertIntoList = bodyArticleIdList.Except(intersectList);
            //因为剩下的部分，可能其中被替换了（被替换的部分也要删除）
            var sourceDeleteList = groupArticleList.Select(s => s.ArticleId).Except(intersectList);

            //最后再确认一次
            var intoList = insertIntoList as int[] ?? insertIntoList.ToArray();
            var conflictList = groupArticleList.Select(s => s.ArticleId).Intersect(intoList).ToList();
            if (conflictList.Any())
            {
                return CreateFailMessage(retValue, "10015", $"腰部文章与原有的存在冲突:{string.Join(",", conflictList)}");
            }
            //删除和intoList排重
            var lastIntoList = intoList.Except(intoList.Intersect(deleteList)).ToList();

            //添加
            var excuteCount = Dal.Materiel.TaskSchedulerUser.Instance.InsertGroupArticle(_contSubmitClean.GroupId, groupInfo.TaskId,
                lastIntoList, sourceDeleteList.ToList(), (int)XyAttrTypeEnum.Body);
            if (excuteCount <= 0)
                return CreateFailMessage(retValue, "10017", $"添加腰部关联出错，GroupId：{_contSubmitClean.GroupId},TaskId:{groupInfo.TaskId},ArticleId:{string.Join(",", intoList)}");

            return SubmitCleanInfoUpdateArticleInfo(retValue);
        }

        /// <summary>
        /// 修改文章内容
        /// </summary>
        /// <param name="retValue"></param>
        /// <returns></returns>
        private ReturnValue SubmitCleanInfoUpdateArticleInfo(ReturnValue retValue)
        {
            var articleInfo = new List<ArticleInfo>()
            {
                new ArticleInfo()
                {
                    ArticleId = _contSubmitClean.Head.ArticleId,
                    Abstract = _contSubmitClean.Head.Abstract,
                    Content = _contSubmitClean.Head.Content,
                    JsonContent = JsonConvert.SerializeObject( _contSubmitClean.Head.Content),
                    Title = _contSubmitClean.Head.Title
                }
            };
            //头部,腰部文章更新
            _contSubmitClean.Body.ForEach(item =>
            {
                articleInfo.Add(new ArticleInfo()
                {
                    ArticleId = item.ArticleId,
                    Abstract = item.Abstract,
                    Content = item.Content,
                    JsonContent = JsonConvert.SerializeObject(item.Content),
                    Title = item.Title
                });
            });
            Dal.Materiel.TaskSchedulerUser.Instance.UpdateArticleInfo(articleInfo, _contSubmitClean.GroupId);

            //带出下一条数据
            retValue.ReturnObject = GetNextGroupId(_contSubmitClean.GroupId);
            return retValue;
        }

        private ReturnValue VerifySubmitParams(ReturnValue retValue)
        {
            if (_contSubmitClean == null || _contSubmitClean.GroupId <= 0)
                return CreateFailMessage(retValue, "10012", "请输入相关参数信息");
            if (_contSubmitClean.Head == null)
                return CreateFailMessage(retValue, "10013", "请输入头部信息");
            if (_contSubmitClean.Body == null || !_contSubmitClean.Body.Any())
                return CreateFailMessage(retValue, "10014", "请输入腰部信息");
            return retValue;
        }

        private ReturnValue VerifySubmitTaskInfo(ReturnValue retValue, int groupId)
        {
            var taskGroupInfo = Dal.Materiel.TaskSchedulerUser.Instance.GetListByGroupId(groupId, new List<string>());

            if (taskGroupInfo == null || !taskGroupInfo.Any())
            {
                return CreateFailMessage(retValue, "10025", $"当前groupId:{groupId}暂没有被分配任务，不允许操作");
            }
            if (taskGroupInfo.Count > 1)
            {
                return CreateFailMessage(retValue, "10026", $"当前groupId:{groupId}被分配任务多次(大约：{taskGroupInfo.Count})，数据错误。无法进行操作");
            }
            var info = taskGroupInfo.FirstOrDefault();

            if (info != null && info.TaskStatus != (int)TaskStatusEnum.Processing)
            {
                return CreateFailMessage(retValue, "10027", $"当前groupId:{groupId}清洗状态不：是处理中，不允许操作");
            }

            return retValue;
        }

        #endregion 清洗提交

        #region 获取清洗详情

        /// <summary>
        /// 获取清洗详情
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="isSee"></param>
        /// <returns></returns>
        public RespCleanInfoDto GetCleanInfo(int groupId, bool isSee)
        {
            if (groupId <= 0)
                return null;
            var respCleanInfo = new RespCleanInfoDto();
            var list = Dal.Materiel.TaskSchedulerUser.Instance.GetTaskListByGroupId(groupId, isSee);
            if (!list.Any())
            {
                return respCleanInfo;
            }
            respCleanInfo.Head = list.FirstOrDefault(s => s.XyAttr == (int)XyAttrTypeEnum.Head);
            respCleanInfo.Body = list.Where(s => s.XyAttr == (int)XyAttrTypeEnum.Body).ToList();
            return respCleanInfo;
        }

        #endregion 获取清洗详情

        #region 枚举

        public enum DistributeOperateType
        {
            [Description("分配")]
            Distribute = 1,

            [Description("回收")]
            Recovery = 2,
        }

        public enum XyAttrTypeEnum
        {
            [Description("头部")]
            Head = 1,

            [Description("腰部")]
            Body = 2
        }

        #endregion 枚举
    }
}