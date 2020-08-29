using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto.RequestDto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media
{
    public class MediaAreaMapping
    {
        #region Instance

        public static readonly MediaAreaMapping Instance = new MediaAreaMapping();

        #endregion Instance

        #region Contructor

        protected MediaAreaMapping()
        { }

        #endregion Contructor

        public int Insert(Entities.Media.MediaAreaMapping entity)
        {
            return Dal.Media.MediaAreaMapping.Instance.Insert(entity);
        }

        public int Delete(int mediaId, int mediaType)
        {
            return Dal.Media.MediaAreaMapping.Instance.Delete(mediaId, mediaType);
        }

        [Obsolete("此方法已过期，用到请通知我：lixiong")]
        /// <summary>
        /// 添加媒体覆盖区域ForTask
        /// </summary>
        /// <param name="requestMediaPublicParam"></param>
        /// <returns></returns>
        public void BusinessCureForTask(RequestMediaPublicParam requestMediaPublicParam)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    BusinessCure(requestMediaPublicParam);
                }
                catch (Exception exception)
                {
                    Loger.Log4Net.ErrorFormat("{3}BusinessCure添加媒体覆盖区域操作失败！参数：MediaID={0}&BusinessType={1}&CoverageArea={2}{3}异常：{4}",
                        requestMediaPublicParam.MediaID, requestMediaPublicParam.BusinessType, requestMediaPublicParam.CoverageArea,
                        System.Environment.NewLine, exception.Message + (exception.StackTrace ?? string.Empty));
                }
            });
        }

        [Obsolete("此方法已过期，用到请通知我：lixiong")]
        /// <summary>
        /// 添加媒体覆盖区域
        /// </summary>
        /// <param name="requestMediaPublicParam"></param>
        /// <returns></returns>
        public ReturnValue BusinessCure(RequestMediaPublicParam requestMediaPublicParam)
        {
            var retValue = new ReturnValue() { HasError = true, Message = "媒体覆盖区域为空" };
            var coverageAreaList = requestMediaPublicParam.CoverageArea.Split(',');//覆盖区域是多选 12-1236,8-810,4-422
            if (coverageAreaList.Length == 0) return retValue;
            var userid = requestMediaPublicParam.LastUpdateUserID;//Common.UserInfo.GetLoginUserID();//获取用户id
            Delete(requestMediaPublicParam.MediaID, requestMediaPublicParam.BusinessType);//删除数据
            Loger.Log4Net.Info(string.Format("删除覆盖区域成功,参数：MediaID={0}&BusinessType={1}&CoverageArea={2}", requestMediaPublicParam.MediaID, requestMediaPublicParam.BusinessType, requestMediaPublicParam.CoverageArea));

            foreach (var item in coverageAreaList)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;//一个12-1236内容
                var spProvinceCityId = item.Split('-');
                if (spProvinceCityId.Length == 1) continue;
                Insert(new Entities.Media.MediaAreaMapping()
                {
                    CityID = spProvinceCityId[1].ToInt(),
                    ProvinceID = spProvinceCityId[0].ToInt(),
                    MediaID = requestMediaPublicParam.MediaID,
                    MediaType = requestMediaPublicParam.BusinessType,
                    CreateTime = DateTime.Now,
                    CreateUserID = userid
                });
            }
            Loger.Log4Net.Info(string.Format("新增/编辑覆盖区域成功,参数：MediaID={0}&BusinessType={1}&CoverageArea={2}", requestMediaPublicParam.MediaID, requestMediaPublicParam.BusinessType, requestMediaPublicParam.CoverageArea));
            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        /// <summary>
        /// 覆盖区域批量操作
        /// </summary>
        /// <param name="areaMapping">区域相关实体</param>
        /// <param name="mediaRelationType">基表 or 附表 类型</param>
        /// <returns></returns>
        public ReturnValue InsertByBulk(AreaMapping areaMapping, MediaRelationType mediaRelationType)
        {
            var retValue = new ReturnValue();//{ HasError = true, Message = "媒体覆盖区域为空" };

            if (areaMapping.CoverageArea == null || areaMapping.CoverageArea.Count == 0)
            {
                Delete(areaMapping, mediaRelationType);
                return retValue;
            }

            Dal.Media.MediaAreaMapping.Instance.Insert_BulkCopyToDB(GetDtData(areaMapping, mediaRelationType),
                mediaRelationType);

            retValue.HasError = false;
            retValue.Message = string.Empty;
            return retValue;
        }

        private DataTable GetDtData(AreaMapping areaMapping, MediaRelationType mediaRelationType)
        {
            if (mediaRelationType == MediaRelationType.Attached)
            {
                return areaMapping.CoverageArea.Select(s => new Entities.Media.MediaAreaMappingTable()
                {
                    ProvinceID = s.ProvinceId,
                    CityID = s.CityId,
                    CreateUserID = areaMapping.CreateUserId,
                    CreateTime = DateTime.Now,
                    MediaType = (int)areaMapping.MediaType,
                    RelateType = (int)areaMapping.AreaMappingType,
                    MediaID = areaMapping.MediaId
                }).ToDataTable();
            }
            return areaMapping.CoverageArea.Select(s => new Entities.Media.MediaAreaMappingBaseTable()
            {
                ProvinceID = s.ProvinceId,
                CityID = s.CityId,
                CreateUserID = areaMapping.CreateUserId,
                CreateTime = DateTime.Now,
                MediaType = (int)areaMapping.MediaType,
                RelateType = (int)areaMapping.AreaMappingType,
                BaseMediaID = areaMapping.MediaId
            }).ToDataTable();
        }

        private void Delete(AreaMapping areaMapping, MediaRelationType mediaRelationType)
        {
            if (mediaRelationType == MediaRelationType.Attached)
            {
                Dal.Media.MediaAreaMapping.Instance.Delete(areaMapping.MediaId, (int)areaMapping.MediaType);
            }
            else
            {
                Dal.Media.MediaAreaMapping.Instance.DeleteBase(areaMapping.MediaId, (int)areaMapping.MediaType);
            }
        }
    }

    public class AreaMapping
    {
        public AreaMapping()
        {
            this.CreateTime = DateTime.Now;
        }

        public List<CoverageAreaDto> CoverageArea { get; set; }
        public int CreateUserId { get; set; }
        public MediaType MediaType { get; set; }
        public MediaAreaMappingType AreaMappingType { get; set; } = MediaAreaMappingType.CoverageArea;
        public DateTime CreateTime { get; set; }
        public int MediaId { get; set; }
    }
}