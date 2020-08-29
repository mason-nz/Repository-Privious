using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Enums;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Verify
{
    public class VerifyOfMediaOperateRole
    {
        /// <summary>
        /// 媒体新增或编辑的模块角色校验
        /// </summary>
        /// <param name="entity">需要验证的实体</param>
        /// <param name="isDefaultRule">是否是默认的全部规则</param>
        /// <param name="funcDic">传入需要验证的规则</param>
        /// <param name="isOutParam">是否需要返回值</param>
        /// <returns></returns>
        public ReturnValue Verify(VerifyModuleRight entity, bool isDefaultRule, Dictionary<int, Func<VerifyModuleRight, ReturnValue>> funcDic,
             bool isOutParam = false)
        {
            //自定义规则
            if (!isDefaultRule) new Verifiction<VerifyModuleRight>(funcDic).Verify(entity, isOutParam);
            //默认规则
            funcDic = new Dictionary<int, Func<VerifyModuleRight, ReturnValue>>
            {
                {(int)  VerifyDataTypeEnum.VerifyOfMediaOperateRoleCheckRights, CheckModuleRight },
            };
            return new Verifiction<VerifyModuleRight>(funcDic).Verify(entity, isOutParam);
        }

        public static ReturnValue CheckInit(VerifyModuleRight entity)
        {
            var retValue = new ReturnValue() { HasError = true, Message = "请输入校验的实体参数" };
            if (entity == null || entity.PublicParam == null)
            {
                return retValue;
            }
            retValue.HasError = false;
            return retValue;
        }

        public static ReturnValue CheckModuleRight(VerifyModuleRight entity)
        {
            if (entity.PublicParam.OperateType == (int) OperateType.Insert)
                return CheckInsertRights(entity);
            else
            {
                return CheckEditRights(entity);
            }
        }

        /// <summary>
        /// 媒体添加的角色权限校验
        /// AE.媒体主.运营 角色能添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ReturnValue CheckInsertRights(VerifyModuleRight entity)
        {
            var retValue = new ReturnValue() { HasError = true, Message = "当前角色不允许添加媒体操作" };
            if (!Chitunion2017.Common.UserInfo.CheckRight(entity.ModuleId, "SYS001"))
                return retValue;
            //var role = RoleInfoMapping.GetUserRole();
            //if (role == RoleEnum.AE || role == RoleEnum.MediaOwner || role == RoleEnum.YunYingOperate)
            //{
            //    return retValue;
            //}
          
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        /// <summary>
        /// 媒体编辑的角色权限校验
        /// AE.媒体主 角色能修改编辑，但只能修改自己创建的
        /// 运营角色能修改全部的
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static ReturnValue CheckEditRights(VerifyModuleRight entity)
        {
            var retValue = new ReturnValue() { HasError = true, Message = "当前角色不允许编辑媒体操作" };
            if (!Chitunion2017.Common.UserInfo.CheckRight(entity.ModuleId, "SYS001"))
                return retValue;

            var role = RoleInfoMapping.GetUserRole();
            if (role == RoleEnum.SupperAdmin || role == RoleEnum.YunYingOperate)
            {
                retValue.HasError = false;
                retValue.Message = string.Empty;
                return retValue;
            }

            if (entity.UserId == entity.PublicParam.LastUpdateUserID)
            {
                retValue.HasError = false;
                retValue.Message = string.Empty;
                return retValue;
            }
            return retValue;
        }
    }


    public class VerifyModuleRight
    {
        public string ModuleId { get; set; }
        public int UserId { get; set; }//媒体创建时的用户Id

        public RequestMediaPublicParam PublicParam { get; set; }
    }
}
