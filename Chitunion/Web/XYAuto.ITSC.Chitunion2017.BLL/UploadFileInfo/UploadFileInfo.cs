using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Dto;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base;
using XYAuto.ITSC.Chitunion2017.BLL.Publish;
using XYAuto.ITSC.Chitunion2017.BLL.Recommend.Extend;
using XYAuto.ITSC.Chitunion2017.Common;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;
using XYAuto.ITSC.Chitunion2017.Entities.Query;

namespace XYAuto.ITSC.Chitunion2017.BLL.UploadFileInfo
{
    public class UploadFileInfo
    {
        #region Instance

        public static readonly UploadFileInfo Instance = new UploadFileInfo();

        #endregion Instance

        #region Contructor

        protected UploadFileInfo()
        { }

        #endregion Contructor

        public List<Entities.UploadFileInfo.UploadFileInfo> GetList(
            UploadFileQuery<Entities.UploadFileInfo.UploadFileInfo> query)
        {
            return Dal.UploadFileInfo.UploadFileInfo.Instance.GetList(query);
        }

        public int Insert(Entities.UploadFileInfo.UploadFileInfo entity)
        {
            return Dal.UploadFileInfo.UploadFileInfo.Instance.Insert(entity);
        }

        public int Update(Entities.UploadFileInfo.UploadFileInfo entity)
        {
            return Dal.UploadFileInfo.UploadFileInfo.Instance.Update(entity);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="urlList">文件路径</param>
        /// <param name="createUserId">当前用户Id</param>
        /// <param name="uploadFileEnum">文件上传类型</param>
        /// <param name="relationId">相关Id</param>
        /// <param name="relationTableName">表名</param>
        /// <returns></returns>
        public List<Entities.UploadFileInfo.UploadFileInfo> GetUploadFileInfo(List<string> urlList, int createUserId,
            UploadFileEnum uploadFileEnum, int relationId, string relationTableName)
        {
            var operateList = new List<Entities.UploadFileInfo.UploadFileInfo>();
            foreach (var url in urlList)
            {
                if (string.IsNullOrWhiteSpace(url)) continue;
                if (url.Equals("-1", StringComparison.OrdinalIgnoreCase)) continue;
                var fileTuple = BLL.Util.GetFileNameAndExtension(url);//获取文件及文件扩展名
                var fileLength = BLL.Util.GetFileSize(BLL.Util.Urlconvertorlocal(url));
                var entity = new Entities.UploadFileInfo.UploadFileInfo
                {
                    CreateTime = DateTime.Now,
                    CreaetUserID = createUserId,
                    ExtendName = fileTuple.Item2,
                    FileName = fileTuple.Item1,
                    FilePah = url.ToAbsolutePath(UserInfo.WebDomain),
                    FileSize = Convert.ToInt32(fileLength),
                    RelationID = relationId,
                    RelationTableName = relationTableName,
                    Type = (int)uploadFileEnum
                };
                operateList.Add(entity);
            }
            return operateList;
        }

        /// <summary>
        /// 上传文件，调用此方法
        /// </summary>
        /// <param name="urlList">文件路径</param>
        /// <param name="createUserId">当前用户Id</param>
        /// <param name="uploadFileEnum">文件上传类型</param>
        /// <param name="relationId">相关Id</param>
        /// <param name="relationTableName">表名</param>
        /// <returns></returns>
        public ReturnValue Excute(List<string> urlList, int createUserId,
            UploadFileEnum uploadFileEnum, int relationId, string relationTableName)
        {
            var list = GetUploadFileInfo(urlList, createUserId, uploadFileEnum, relationId, relationTableName);
            if (list.Count == 0)
            {
                return new ReturnValue()
                {
                    HasError = true,
                    Message = string.Format("GetUploadFileInfo 为null，参数：urlList={0}&createUserId={1}&uploadFileEnum={2}&relationId={3}&relationTableName={4}",
                    urlList, createUserId, uploadFileEnum, relationId, relationTableName)
                };
            }
            return DoOperate(list);
        }

        /// <summary>
        /// 文件上传相关信息维护
        /// </summary>
        /// <param name="operateList"></param>
        /// <returns></returns>
        public ReturnValue DoOperate(List<Entities.UploadFileInfo.UploadFileInfo> operateList)
        {
            var retValue = new ReturnValue() { HasError = true, Message = "请传入UploadFileInfo参数" };
            var currentEntity = operateList.FirstOrDefault();
            if (currentEntity == null)
                return retValue;

            //if (operateType == OperateType.Edit)
            //{
            //    retValue = VerifyUpdateUploadFile(currentEntity, retValue);
            //    if (retValue.HasError)
            //        return retValue;
            //}

            //删除
            Dal.UploadFileInfo.UploadFileInfo.Instance.Delete(currentEntity);
            //添加
            try
            {
                operateList.ForEach(item => { Insert(item); });
            }
            catch (Exception ex)
            {
                retValue.Message = string.Format("UploadFileInfo 添加失败。参数：RelationID={0}&Type={1}&CreaetUserID={2},异常:{3}",
               currentEntity.RelationID, currentEntity.Type, currentEntity.CreaetUserID, ex.Message);
                Loger.Log4Net.Error(retValue.Message);
                return retValue;
            }
            retValue.HasError = false;
            retValue.Message = "success";
            return retValue;
        }

        private ReturnValue VerifyUpdateUploadFile(Entities.UploadFileInfo.UploadFileInfo entity, ReturnValue retValue)
        {
            retValue = retValue ?? new ReturnValue();
            var uploadList = GetList(new UploadFileQuery<Entities.UploadFileInfo.UploadFileInfo>()
            {
                CreaetUserID = entity.CreaetUserID,
                Type = entity.Type,
                //PageSize = 5,
                RelationID = entity.RelationID
            });
            if (uploadList.Count == 0)
            {
                retValue.HasError = true;
                retValue.Message = string.Format("当前用户:{0},类型:{1}没有查到上传文件信息", entity.CreaetUserID, entity.Type);
                return retValue;
            }
            retValue.HasError = false;
            return retValue;
        }

        public ReturnValue VerifyUploadFile(ReturnValue retValue)
        {
            retValue = retValue ?? new ReturnValue();
            var roleEnum = RoleInfoMapping.GetUserRole();
            if (roleEnum == RoleEnum.SupperAdmin || roleEnum == RoleEnum.YunYingOperate)
            {
                retValue.HasError = false;
                return retValue;
            }
            return retValue;
        }

        /// <summary>
        /// 2017-03-24 张立彬 删除反馈数据对应的图片信息
        /// </summary>
        /// <param name="TalbeID">反馈表ID</param>
        /// <param name="TableName">反馈表名称</param>
        /// <param name="UploadType">上传类型</param>
        /// <returns></returns>
        public int Delete(int TalbeID, string TableName, int UploadType)
        {
            return Dal.UploadFileInfo.UploadFileInfo.Instance.Delete(TalbeID, TableName, UploadType);
        }
    }
}