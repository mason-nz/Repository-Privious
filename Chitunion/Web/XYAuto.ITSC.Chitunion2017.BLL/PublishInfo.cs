using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Constants;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    /// <summary>
    /// 刊例相关业务逻辑类
    /// ls
    /// </summary>
    public class PublishInfo
    {
        public static readonly PublishInfo Instance = new PublishInfo();

        /// <summary>
        /// 增加刊例
        /// ls
        /// </summary>
        /// <param name="model">PublishBasicInfo对象</param>
        /// <param name="prices">维度价格串数组 维度1-维度2-维度3-价格</param>
        /// <returns>刊例ID 大于0正常</returns>
        public int AddPublishBasicInfo(PublishInfoDTO dto)
        {
            var ur = Common.UserInfo.GetUserRole();
            int userID = ur.UserID;
            string msg = string.Empty;
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.添加刊例, string.Empty, "CreateUserID", userID, out msg);
            Entities.Publish.PublishBasicInfo model = dto.Publish;
            List<string> prices = dto.Prices;
            model.PubCode = "AP" + (int)model.MediaType + DateTime.Now.ToString("yyyyMMdd") + new Random().Next(10000, 99999);
            if (ur.IsAdministrator || ur.IsAE || ur.IsYY)
            {//管理员 运营 AE创建的审核通过并上架
                model.Status = AuditStatusEnum.已通过;
                model.PublishStatus = PublishStatusEnum.已上架;
            }
            else
            {
                model.Status = AuditStatusEnum.新建;
                model.PublishStatus = PublishStatusEnum.新建;
            }
            model.CreateUserID = userID;//
            model.LastUpdateUserID = userID;
            model.CreateTime = DateTime.Now;
            model.LastUpdateTime = DateTime.Now;

            List<Entities.Publish.PublishDetailInfo> detailList = new List<Entities.Publish.PublishDetailInfo>();
            if (model.MediaType != MediaTypeEnum.APP)
            {
                #region PublishDetailInfo-List-Fill
                foreach (var priceStr in prices)
                {
                    int count = priceStr.Split('-').Length;
                    int p1 = int.Parse(priceStr.Split('-')[0]);//6001
                    int p2 = count > 2 ? int.Parse(priceStr.Split('-')[1]) : Constant.INT_INVALID_VALUE;//7001
                    int p3 = count > 3 ? int.Parse(priceStr.Split('-')[2]) : Constant.INT_INVALID_VALUE;//8001
                    decimal price = decimal.Parse(priceStr.Split('-')[count - 1]);
                    detailList.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = p1,
                        ADPosition2 = p2,
                        ADPosition3 = p3,
                        Price = price
                    });
                    if (ur.IsAdministrator || ur.IsAE || ur.IsYY)//管理员 运营 AE创建的审核通过并上架
                    {
                        detailList.ForEach(d => d.PublishStatus = PublishStatusEnum.已上架);
                    }
                    else
                    {
                        detailList.ForEach(d => d.PublishStatus = PublishStatusEnum.新建);
                    }
                }
                #endregion
            }
            return Dal.PublishInfo.Instance.AddPublishBasicInfo(model, detailList, rightSql);
        }

        public bool AddPublishBasicInfoV1_1(ModifyPublishReqDTO dto, ref string msg, ref int pubID)
        {
            var ur = Common.UserInfo.GetUserRole();
            string rightSql = string.Empty;
            if (ur.IsAE)
                rightSql = BLL.Util.CreateRightSql(Util.CreateSqlDependOnEnum.角色, ur);
            else if (ur.IsMedia)
                rightSql = BLL.Util.CreateRightSql(Util.CreateSqlDependOnEnum.用户, ur);
            else
                return false;
            if (dto.Publish.MediaType.Equals((int)MediaTypeEnum.微信))
            {
                #region 微信
                if (dto.Publish.MediaID.Equals(0) && !string.IsNullOrWhiteSpace(dto.Publish.MediaName))
                {
                    int mediaID = Dal.Media.MediaWeixin.Instance.GetWeixinMediaIDByMediaName(dto.Publish.MediaName, rightSql);
                    if (mediaID.Equals(0))
                    {
                        msg = "媒体不存在或不可用";
                        return false;
                    }
                    dto.Publish.MediaID = mediaID;
                }
                if (ur.IsMedia)
                {
                    dto.Publish.Wx_Status = (int)PublishBasicStatusEnum.上架;
                }
                else
                {
                    dto.Publish.Wx_Status = (int)PublishBasicStatusEnum.待审核;
                }
                #endregion
            }
            else if (dto.Publish.MediaType.Equals((int)MediaTypeEnum.APP))
            {
                #region APP
                msg = string.Empty;
                if (!ur.IsAE && !ur.IsMedia) {
                    return false;
                }
                var tStatus = Dal.PublishInfo.Instance.GetTemplateStatus(dto.Publish.TemplateID, true, ur.IsAE ? 0 : ur.UserID);
                int pStatus = 0;
                if (tStatus.Equals(0))
                {
                    msg = "模板不存在或不可用";
                    return false;
                }
                if (CheckIsConflict((int)MediaTypeEnum.APP, dto.Publish.BeginTime, dto.Publish.EndTime, dto.Publish.MediaID, dto.Publish.TemplateID, dto.Publish.PubID))
                {
                    msg = "当前价格有效期与现有有效期冲突，请重新设置";
                    return false;
                }
                if (tStatus == (int)AppTemplateEnum.已通过)
                {//共有
                    pStatus = ur.IsMedia ? (int)AppPublishStatus.已上架 : (int)AppPublishStatus.待审核; 
                }
                else {//私有
                    msg = "不能使用私有模板";
                    return false;
                    //pStatus = ur.IsMedia ? (int)AppPublishStatus.未上架 : (int)AppPublishStatus.待提交;
                }
                if (dto.Pwd != null && dto.Pwd.Equals("xingyuanimportdata"))
                    pStatus = (int)AppPublishStatus.已上架;
                dto.Publish.Wx_Status = pStatus;
                #endregion
            }
            dto.Publish.PubCode = "AP" + (int)dto.Publish.MediaType + DateTime.Now.ToString("yyyyMMdd") + new Random().Next(10000, 99999);
            dto.Publish.CreateUserID = ur.UserID;
            dto.Publish.LastUpdateUserID = ur.UserID;
            dto.Publish.CreateTime = DateTime.Now;
            dto.Publish.LastUpdateTime = DateTime.Now;
            pubID = Dal.PublishInfo.Instance.AddPublishBasicInfo_V1_1(dto.Publish, rightSql, dto.Details, dto.PriceList);
            if (pubID.Equals(0))
                msg = "媒体不存在";
            if (pubID.Equals(-1))
                msg = "系统异常";
            return pubID>0;
        }

        /// <summary>
        /// 修改刊例 
        /// ls
        /// </summary>
        /// <param name="model">PublishBasicInfo对象</param>
        /// <param name="prices">维度价格串数组 维度1-维度2-维度3-价格</param>
        /// <returns></returns>
        public bool UpdataPublishBasicInfo(PublishInfoDTO dto)
        {
            int userID = Chitunion2017.Common.UserInfo.GetLoginUserID();
            string msg = string.Empty;
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.修改刊例, string.Empty, "CreateUserID", userID, out msg, dto.Publish.PubID);
            Entities.Publish.PublishBasicInfo model = dto.Publish;
            List<string> prices = dto.Prices;
            model.LastUpdateUserID = userID;//
            model.LastUpdateTime = DateTime.Now;
            List<Entities.Publish.PublishDetailInfo> detailList = new List<Entities.Publish.PublishDetailInfo>();
            if (model.MediaType != MediaTypeEnum.APP)
            {
                #region PublishDetailInfo-List-Fill
                foreach (var priceStr in prices)
                {
                    int count = priceStr.Split('-').Length;
                    int p1 = int.Parse(priceStr.Split('-')[0]);//6001
                    int p2 = count > 2 ? int.Parse(priceStr.Split('-')[1]) : Constant.INT_INVALID_VALUE;//7001
                    int p3 = count > 3 ? int.Parse(priceStr.Split('-')[2]) : Constant.INT_INVALID_VALUE;//8001
                    decimal price = decimal.Parse(priceStr.Split('-')[count - 1]);
                    detailList.Add(new Entities.Publish.PublishDetailInfo()
                    {
                        ADPosition1 = p1,
                        ADPosition2 = p2,
                        ADPosition3 = p3,
                        Price = price,
                        CreateTime = model.CreateTime,
                        CreateUserID = model.CreateUserID
                    });
                }
                #endregion
            }
            return Dal.PublishInfo.Instance.UpdataPublishBasicInfo(model, detailList, rightSql) > 0;
        }

        public bool UpdatePublishBasicInfo_V1_1(ModifyPublishReqDTO dto, ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            string rightSql = string.Empty;
            var one = PublishInfo.Instance.GetDetail(dto.Publish.PubID);
            if (CheckIsConflict((int)MediaTypeEnum.APP, dto.Publish.BeginTime, dto.Publish.EndTime, one.MediaID, one.TemplateID, dto.Publish.PubID))
            {
                msg = "当前价格有效期与现有有效期冲突，请重新设置";
                return false;
            }
            if (ur.IsAE)
                rightSql = BLL.Util.CreateRightSql(Util.CreateSqlDependOnEnum.角色, ur);
            else if (ur.IsMedia)
                rightSql = BLL.Util.CreateRightSql(Util.CreateSqlDependOnEnum.用户, ur);
            else
                return false;
            if (dto.Publish.MediaType.Equals(14001))
            {
                if (!ur.IsMedia)
                {
                    dto.Publish.Wx_Status = (int)PublishBasicStatusEnum.待审核;
                }
                else
                {
                    dto.Publish.Wx_Status = (int)PublishBasicStatusEnum.上架;
                }
            }
            else if (dto.Publish.MediaType.Equals(14002)) {
                var tStatus = Dal.PublishInfo.Instance.GetTemplateStatus(dto.Publish.PubID, false, ur.IsAE ? 0 : ur.UserID);
                int pStatus = 0;
                if (tStatus.Equals(0))
                    return false ;
                if (tStatus == (int)AppTemplateEnum.已通过)
                {//共有 媒体主:下架——下架  AE:驳回、下架——待审核
                    pStatus = ur.IsMedia ? (int)AppPublishStatus.已下架 : (int)AppPublishStatus.待审核;
                }
                else
                {//私有 媒体主:未上架——未上架  AE:待提交——待提交
                    msg = "不能使用私有模板";
                    return false;
                    //pStatus = ur.IsMedia ? (int)AppPublishStatus.未上架 : (int)AppPublishStatus.待提交;
                }
                dto.Publish.Wx_Status = pStatus;
            }
            dto.Publish.LastUpdateUserID = ur.UserID;
            dto.Publish.LastUpdateTime = DateTime.Now;
            return Dal.PublishInfo.Instance.UpdataPublishBasicInfo_V1_1(dto.Publish, rightSql, dto.Details, dto.PriceList) > 0;
        }

        /// <summary>
        /// 修改刊例状态
        /// ls
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool ModifyPublishStatus(ModifyPublishStatusDTO dto)
        {
            int userID = Chitunion2017.Common.UserInfo.GetLoginUserID();
            string msg = string.Empty;
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.修改刊例, string.Empty, "CreateUserID", userID, out msg, dto.PubID);
            if (string.IsNullOrEmpty(dto.RecID))
            {
                return Dal.PublishInfo.Instance.ModifyPublishStatus(dto.PubID, (int)dto.Status, userID, rightSql);
            }
            else
            {
                return Dal.PublishInfo.Instance.ModifyADPositionStatus(dto.RecID, dto.PubID, (int)dto.Status, userID, rightSql);
            }
        }

        /// <summary>
        /// 检查APP的媒体名称是否存在 以及返回存在的MediaID、刊例数量、广告位数量
        /// ls
        /// </summary>
        /// <param name="mediaName">媒体名称</param>
        /// <returns>
        /// Item1 : 0-可以添加 1-有刊例无广告位 2-有刊例有广告位 3-无对应名称媒体 4-参数错  
        /// Item2 : 实体包含MediaID、刊例数量、广告位数量
        /// </returns>
        public Tuple<int, MediaExistsDTO> CheckAppExists(string mediaName)
        {

            int status = 0;
            MediaExistsDTO dto = null;
            if (!string.IsNullOrEmpty(mediaName))
            {
                dto = BLL.MediaInfo.Instance.MediaIsExists(MediaTypeEnum.APP, 0, mediaName, string.Empty, true);
                if (dto.IsExists)//存在媒体名称
                {
                    if (dto.PubID > 0)
                    {
                        status = dto.ADCount == 0 ? 1 : 2;
                    }
                }
                else
                { //媒体名称不存在
                    status = 3;
                }
            }
            else
            {//名称为空错误
                status = 4;
            }
            return new Tuple<int, MediaExistsDTO>(status, dto);
        }

        /// <summary>
        /// 增加广告位
        /// ls
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool AddPublishExtendInfo(ADPositionDTO dto)
        {

            int userID = Chitunion2017.Common.UserInfo.GetLoginUserID();
            string msg = string.Empty;
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.修改刊例, string.Empty, "CreateUserID", userID, out msg, dto.PubID);
            var ur = Common.UserInfo.GetUserRole();
            if (ur.IsAdministrator || ur.IsAE || ur.IsYY)
            {//管理员 运营 AE创建的审核通过并上架
                dto.PublishStatus = PublishStatusEnum.已上架;
            }
            else
            {
                dto.PublishStatus = PublishStatusEnum.新建;
            }
            dto.CreateTime = DateTime.Now;
            dto.LastUpdateTime = DateTime.Now;
            dto.CreateUserID = userID;
            dto.LastUpdateUserID = userID;
            return Dal.PublishInfo.Instance.AddPublishExtendInfo(dto, rightSql) > 0;
        }

        /// <summary>
        /// 更新广告位
        /// ls
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool UpdatePublishExtendInfo(ADPositionDTO dto)
        {

            int userID = Chitunion2017.Common.UserInfo.GetLoginUserID();
            string msg = string.Empty;
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.修改刊例, string.Empty, "CreateUserID", userID, out msg, dto.PubID);
            dto.CreateTime = DateTime.Now;
            dto.LastUpdateTime = DateTime.Now;
            dto.LastUpdateUserID = userID;
            return Dal.PublishInfo.Instance.UpdatePublishExtendInfo(dto, rightSql) > 0;
        }

        public bool CopyPublishExtendInfo(ADCopyDTO dto)
        {

            int userID = Chitunion2017.Common.UserInfo.GetLoginUserID();
            string msg = string.Empty;
            string rightSql = Util.GetSqlRightStr(Entities.EnumResourceType.修改刊例, string.Empty, "CreateUserID", userID, out msg, dto.PubID);
            return Dal.PublishInfo.Instance.CopyPublishExtendInfo(dto.ADDetailID, userID, rightSql) > 0;
        }

        public bool GetMediaTypeByPubID(int pubID, out MediaTypeEnum mediaType)
        {
            int type = Dal.PublishInfo.Instance.GetMediaTypeByPubID(pubID);
            return System.Enum.TryParse<MediaTypeEnum>(type.ToString(), out mediaType);
        }

        public int AuditPublish(AuditPublishReqDTO req, out int nextPubID)
        {
            var ur = Common.UserInfo.GetUserRole();
            string rightSql = string.Empty;
            rightSql = " and 1=1 ";
            int rowcount = Dal.PublishInfo.Instance.AuditPublish(req.PubID, req.OpType, ur.UserID, rightSql, out nextPubID);
            if ((req.OpType.Equals(42002) || req.OpType.Equals(42003)) && rowcount > 0)
            {
                Entities.PublishAuditInfo info = new Entities.PublishAuditInfo()
                {
                    PublishID = req.PubID,
                    OptType = req.OpType.Equals(42002) ? 27001 : 27002,
                    PubStatus = req.OpType.Equals(42002) ? 42011 : req.OpType,
                    MediaType = 14001,
                    CreateUserID = ur.UserID,
                    RejectMsg = req.RejectReason,
                    CreateTime = DateTime.Now
                };
                if (req.OpType.Equals(42002))
                {
                    var obj = Dal.PublishInfo.Instance.GetWeixinOldPublish(req.PubID);
                    info.RejectMsg = JsonConvert.SerializeObject(obj);
                }
                Dal.PublishAuditInfo.Instance.Insert(info);
            }

            return rowcount;
        }

        /// <summary>
        /// 批量审核、上下架刊例
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public bool BatchAuditPublish(AuditPublishReqDTO req,ref string msg, ref int nextPubID) {
            
            var ur = Common.UserInfo.GetUserRole();
            string rightSql = string.Empty;
            if (req.OpType.Equals(1) || req.OpType.Equals(2))
            {//通过 驳回需运营
                if (!ur.IsYY && !ur.IsAdministrator)
                    return false;
                rightSql = " and 1=1 ";
            }
            else
            {//上下架
                if (ur.IsAE)
                    rightSql = BLL.Util.CreateRightSql(Util.CreateSqlDependOnEnum.角色, ur);
                else if (ur.IsMedia)
                    rightSql = BLL.Util.CreateRightSql(Util.CreateSqlDependOnEnum.用户, ur);
                else if (ur.IsAdministrator || ur.IsYY)
                    rightSql = " and 1=1 ";
                else
                    return false;
            }
            bool flag = true;
            StringBuilder sb = new StringBuilder();
            if (req.PubIDs != null && req.PubIDs.Count > 0) {
                foreach (var pubID in req.PubIDs) {
                    #region 逐条审核
                    if (req.OpType.Equals(3))
                    {
                        //上架
                        var one = PublishInfo.Instance.GetDetail(pubID);
                        if (CheckIsConflict((int)MediaTypeEnum.APP, one.BeginTime, one.EndTime, one.MediaID, one.TemplateID, pubID))
                        {
                            sb.Append(string.Format("PubID:{0} {1} \r\n", pubID, "当前价格有效期与现有有效期冲突，请重新设置"));
                            flag = false;
                            continue;
                        }
                    }
                    int rowcount = Dal.PublishInfo.Instance.AuditPublish(pubID, req.OpType, ur.UserID, rightSql, out nextPubID);
                    if (rowcount <= 0)
                    {
                        flag = false;
                        sb.Append(string.Format("PubID:{0} {1} \r\n", pubID, "审核或上下架失败"));
                    }
                    else if ((req.OpType.Equals(1) || req.OpType.Equals(2)))
                    {
                        Entities.PublishAuditInfo info = new Entities.PublishAuditInfo()
                        {
                            PublishID = pubID,
                            OptType = req.OpType.Equals(1) ? (int)AppPublishStatus.已上架 : (int)AppPublishStatus.已驳回,
                            PubStatus = req.OpType.Equals(1) ? (int)AppPublishStatus.已上架 : (int)AppPublishStatus.已驳回,
                            MediaType = 14002,
                            CreateUserID = ur.UserID,
                            RejectMsg = req.RejectReason,
                            CreateTime = DateTime.Now
                        };
                        if (req.OpType.Equals(1))
                        {
                            var obj = Dal.PublishInfo.Instance.GetAppOldPublish(pubID);
                            info.RejectMsg = JsonConvert.SerializeObject(obj);
                        }
                        Dal.PublishAuditInfo.Instance.Insert(info);
                    }
                    #endregion
                }
            }
            msg = sb.ToString();
            return flag;
        }

        public int DeletePublishBasicInfo(int pubID)
        {
            return Dal.PublishInfo.Instance.DeletePublishBasicInfo(pubID);
        }

        /// <summary>
        /// 检查刊例是否有排期冲突
        /// 注意一定要在CheckSelfModel之后
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool CheckIsConflict(int mediaType, DateTime beginTime, DateTime endTime, int mediaID, int templateID, int pubID) {

            return Dal.PublishInfo.Instance.CheckIsConflict(mediaType, beginTime.ToString("yyyy-MM-dd"), endTime.ToString("yyyy-MM-dd"), mediaID, templateID, pubID);
        }

        public GetPublishListBResDTO GetAppPublishList(GetADListBReqDTO req)
        {
            string rightSql = string.Empty;
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsAdministrator && !ur.IsYY && !ur.IsADAudit)
                return null;
            if (!req.IsAE && (ur.IsAdministrator || ur.IsYY))//运营列表页
                rightSql = " and 1=1";
            else//AE领导页
                rightSql = " and v_AppADList.CreateUserID in (select UserID from UserRole where RoleID = 'SYS001RL00005')";
            return Dal.PublishInfo.Instance.GetAppPublishList(req.MediaID, req.MediaName, req.ADName, req.UserName, req.BeginTime, req.EndTime, req.PubStatus, rightSql, null, req.PageIndex, req.PageSize);
        }

        public Entities.Publish.PublishBasicInfo GetDetail(int pubID)
        {
            return Dal.PublishInfo.Instance.GetDetail(pubID);
        }

        public List<PubDateItem> GetPubDateList(int mediaID, int templateID)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsAE && !ur.IsMedia)
                return null;
            return Dal.PublishInfo.Instance.GetPubDateList(mediaID, templateID);
        }


        }
}
