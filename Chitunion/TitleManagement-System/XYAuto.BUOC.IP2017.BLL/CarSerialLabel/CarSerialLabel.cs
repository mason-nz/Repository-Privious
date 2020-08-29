using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.Business.DTO.ResponseDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.Business.Query.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.CarSerialLabel.DTO.RequestDto.V1_2_4;
using XYAuto.BUOC.IP2017.BLL.CarSerialLabel.DTO.ResponseDto.V1_2_4;
using XYAuto.BUOC.IP2017.Entities.BatchMedia;

namespace XYAuto.BUOC.IP2017.BLL.CarSerialLabel
{
    public class CarSerialLabel
    {
        #region 当前登录人UserID
        private int _currentUserID;
        public int currentUserID
        {
            get
            {
                try
                {
                    _currentUserID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetLoginUserID();
                }
                catch (Exception)
                {
                    _currentUserID = 1225;
                }
                return _currentUserID;
            }
        }
        #endregion        
        public static readonly CarSerialLabel Instance = new CarSerialLabel();
        #region 标签录入列表
        public object InputListCar(ReqInputListCarDto request, ref string errmsg)
        {
            //request.CreateUserID = XYAuto.ITSC.Chitunion2017.Common.UserInfo.GetUserRole().UserID;
            errmsg = string.Empty;
            if (request == null)
            {
                errmsg = "请求参数不能为null";
                return null;
            }
            if (!request.CheckSelfModel(out errmsg))
                return null;

            var result = new InputListCarQuery().GetQueryList(request);
            if (result.List == null)
            {
                errmsg = "没有数据";
                return null;
            }
            result.List.ForEach(x =>
            {
                var operateinfoList = BLL.BatchMedia.BatchMedia.Instance.GetListByCar(x.BrandID, x.SerialID);
                x.OperateInfo = new List<ResInputListCarOperateinfoDto>();
                x.OperateInfo = operateinfoList;
            });
            return result;
        }
        #endregion
        #region 打标签车型渲染接口
        public object RenderBatchCar(DTO.RequestDto.V1_2_4.ReqRenderBatchCarDto req, ref string errmsg)
        {
            errmsg = string.Empty;
            if (!req.CheckSelfModel(out errmsg))
                return null;

            //检查是否有待打批次
            var batchMediaID = ValidBatchCar(req.BrandID, req.SerialID, ref errmsg);
            if (!string.IsNullOrEmpty(errmsg))
                return null;

            ResRenderBatchCarDto resDto = new ResRenderBatchCarDto();
            resDto.IPLabel = new List<MediaLabel.DTO.ResponseDto.V1_2_4.ResBatchMediaIplabelDto>();
            var tp = Dal.CarSerialLabel.CarSerialLabel.Instance.RenderBatchCar(batchMediaID);
            if (tp.Item1 != null)
                resDto = Util.DataTableToEntity<ResRenderBatchCarDto>(tp.Item1);

            if (tp.Item2 != null)
                resDto.IPLabel = Util.DataTableToList<MediaLabel.DTO.ResponseDto.V1_2_4.ResBatchMediaIplabelDto>(tp.Item2);

            resDto.IPLabel.ForEach(x =>
            {
                x.SubIP = new List<MediaLabel.DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>();
                x.SubIP = Util.DataTableToList<MediaLabel.DTO.ResponseDto.V1_2_4.ResBatchMediaCategoryDto>(Dal.IPTitleInfo.IPTitleInfo.Instance.GetSubIPByPID(x.DictId));
            });

            return resDto;
        }
        public int ValidBatchCar(int brandID, int serialID, ref string errmsg)
        {
            errmsg = string.Empty;

            //检查是否有待打批次
            Entities.BatchMedia.QueryBatchMedia query = new Entities.BatchMedia.QueryBatchMedia()
            {
                BrandID = brandID,
                SerialID = serialID,
                CurrentUserID = currentUserID,
                Status = (int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打
            };
            Entities.BatchMedia.BatchMedia bmModel = BLL.BatchMedia.BatchMedia.Instance.GetModelByCar(query);
            if (bmModel == null)//没有待打批次
            {
                query.BatchMediaID = BLL.BatchMedia.BatchMedia.Instance.Insert(new Entities.BatchMedia.BatchMedia()
                {
                    BrandID = brandID,
                    SerialID = serialID,
                    CreateUserID = currentUserID,
                    Status = query.Status,
                    TaskType = serialID == -2 ? (int)Entities.ENUM.ENUM.EnumTaskType.子品牌 : (int)Entities.ENUM.ENUM.EnumTaskType.车型,
                });
            }
            else
                query.BatchMediaID = bmModel.BatchMediaID;

            return query.BatchMediaID;
        }
        #endregion
        #region 查看文章或媒体已审批次详情
        public object ViewBatchCar(int BatchMediaID, ref string errmsg)
        {
            errmsg = string.Empty;
            var batchMediaModel = BLL.BatchMedia.BatchMedia.Instance.GetModelByRecID(BatchMediaID);
            if (batchMediaModel == null)
            {
                errmsg = $"参数错误，批次:{BatchMediaID}不存在";
                return null;

            }

            var result = new ResViewBatchCarDto();
            var tp = Dal.CarSerialLabel.CarSerialLabel.Instance.ViewBatchCar(BatchMediaID, batchMediaModel.BatchAuditID);

            //提交人、审核人
            if (tp.Item1 != null)
            {
                result = Util.DataTableToEntity<ResViewBatchCarDto>(tp.Item1);
                if (result != null)
                {
                    result.OperateInfo = new MediaLabel.DTO.ResponseDto.V1_2_4.Operateinfo()
                    {
                        UserName = result.OperateUser,
                        CreateTime = result.CreateTime
                    };

                    result.AuditInfo = new MediaLabel.DTO.ResponseDto.V1_2_4.Operateinfo()
                    {
                        UserName = result.AuditUser,
                        CreateTime = result.AuditTime
                    };
                }
            }

            result.IPLabel = new MediaLabel.DTO.ResponseDto.V1_2_4.IPCategory();
            //打标签项
            if (tp.Item2 != null)
            {
                var batchOriginal = Util.DataTableToList<MediaLabel.DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>(tp.Item2);

                //IP标签                
                result.IPLabel.Original = new List<MediaLabel.DTO.ResponseDto.V1_2_4.IPLabel>();
                foreach (var item in batchOriginal.Where(t => t.Type == (int)Entities.ENUM.ENUM.EnumLabelType.IP).ToList())
                {
                    result.IPLabel.Original.Add(new MediaLabel.DTO.ResponseDto.V1_2_4.IPLabel()
                    {
                        DictId = item.DictId,
                        DictName = item.DictName,
                        LabelID = item.LabelID
                    });
                }

                //子IP项
                if (tp.Item4 != null)
                {
                    var ipSUB = Util.DataTableToList<MediaLabel.DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>(tp.Item4);
                    var ipSON = new List<MediaLabel.DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>();
                    if (tp.Item5 != null)
                    {
                        ipSON = Util.DataTableToList<MediaLabel.DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>(tp.Item5);
                    }

                    //遍历IP标签
                    result.IPLabel.Original.ForEach(x =>
                    {
                        x.SubIP = new List<MediaLabel.DTO.ResponseDto.V1_2_4.SubIPLabelDto>();
                        foreach (var item in ipSUB.Where(t => t.LabelID == x.LabelID).ToList())
                        {
                            x.SubIP.Add(new MediaLabel.DTO.ResponseDto.V1_2_4.SubIPLabelDto()
                            {
                                DictId = item.DictId,
                                DictName = item.DictName,
                                SubIPLabelID = item.SubIPID
                            });
                        }


                        //遍历子IP
                        x.SubIP.ForEach(x1 =>
                        {
                            x1.Label = new List<MediaLabel.DTO.ResponseDto.V1_2_4.LabelDto>();
                            foreach (var item in ipSON.Where(t => t.SubIPID == x1.SubIPLabelID).ToList())
                            {
                                x1.Label.Add(new MediaLabel.DTO.ResponseDto.V1_2_4.LabelDto()
                                {
                                    DictId = item.DictId,
                                    DictName = item.DictName
                                });
                            }
                        });
                    });

                }

            }
            //审标签项
            if (tp.Item3 != null)
            {
                var batchAudit = Util.DataTableToList<MediaLabel.DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>(tp.Item3);


                //IP标签 （审核）               
                result.IPLabel.Audit = new List<MediaLabel.DTO.ResponseDto.V1_2_4.IPLabel>();
                foreach (var item in batchAudit.Where(t => t.Type == (int)Entities.ENUM.ENUM.EnumLabelType.IP).ToList())
                {
                    result.IPLabel.Audit.Add(new MediaLabel.DTO.ResponseDto.V1_2_4.IPLabel()
                    {
                        DictId = item.DictId,
                        DictName = item.DictName,
                        LabelID = item.LabelID
                    });
                }


                //子IP项（审核）item6
                if (tp.Item6 != null)
                {
                    var ipSUB = Util.DataTableToList<MediaLabel.DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>(tp.Item6);
                    //标签项（审核）item7
                    var ipSON = new List<MediaLabel.DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>();
                    if (tp.Item7 != null)
                    {
                        ipSON = Util.DataTableToList<MediaLabel.DTO.ResponseDto.V1_2_4.ResViewBatchMediaDictDto>(tp.Item7);
                    }

                    //遍历IP标签
                    result.IPLabel.Audit.ForEach(x =>
                    {
                        x.SubIP = new List<MediaLabel.DTO.ResponseDto.V1_2_4.SubIPLabelDto>();
                        foreach (var item in ipSUB.Where(t => t.LabelID == x.LabelID).ToList())
                        {
                            x.SubIP.Add(new MediaLabel.DTO.ResponseDto.V1_2_4.SubIPLabelDto()
                            {
                                DictId = item.DictId,
                                DictName = item.DictName,
                                SubIPLabelID = item.SubIPID
                            });
                        }


                        //遍历子IP
                        x.SubIP.ForEach(x1 =>
                        {
                            x1.Label = new List<MediaLabel.DTO.ResponseDto.V1_2_4.LabelDto>();
                            foreach (var item in ipSON.Where(t => t.SubIPID == x1.SubIPLabelID).ToList())
                            {
                                x1.Label.Add(new MediaLabel.DTO.ResponseDto.V1_2_4.LabelDto()
                                {
                                    DictId = item.DictId,
                                    DictName = item.DictName
                                });
                            }
                        });
                    });

                }
            }
            //验证IP标签，原跟审核是否完全一样
            result.IpIsSame = BLL.MediaLabel.MediaLabel.Instance.StatisticsLabel(BatchMediaID);
            return result;
        }
        #endregion
        #region 打标签车型提交
        public int BatchCarSubmit(ReqBatchCarSubmitDto req, ref string errmsg)
        {
            errmsg = string.Empty;
            if (!req.CheckSelfModel(out errmsg))
                return -2;

            //获取待打批次
            Entities.BatchMedia.QueryBatchMedia query = new Entities.BatchMedia.QueryBatchMedia()
            {
                BrandID = req.BrandID,
                SerialID = req.SerialID,
                CurrentUserID = currentUserID,
                Status = (int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待打
            };
            Entities.BatchMedia.BatchMedia bmModel = BLL.BatchMedia.BatchMedia.Instance.GetModelByCar(query);
            if (bmModel == null)//没有待打批次
            {
                errmsg = "待打批次不存在";
                return -2;
            }

            //记录标签
            BLL.BatchLabelHistory.BatchLabelHistory.Instance.InsertBatch(req, bmModel.BatchMediaID, currentUserID);

            //检查待审记录
            var batchAuditMode = BLL.BatchMediaAudit.BatchMediaAudit.Instance.GetModelPending(req.BrandID, req.SerialID);
            if (batchAuditMode == null)
            {
                //插入待审记录
                int batchAuditID = BLL.BatchMediaAudit.BatchMediaAudit.Instance.Insert(new Entities.BatchMediaAudit.BatchMediaAudit()
                {
                    TaskType = req.SerialID == -2 ? (int)Entities.ENUM.ENUM.EnumTaskType.子品牌 : (int)Entities.ENUM.ENUM.EnumTaskType.车型,
                    BrandID = req.BrandID,
                    SerialID = req.SerialID,
                    Status = (int)Entities.ENUM.ENUM.EnumBatchMediaStatus.待审,
                    CreateUserID = currentUserID
                });
                batchAuditMode = new Entities.BatchMediaAudit.BatchMediaAudit()
                {
                    BatchAuditID = batchAuditID
                };
            }

            //更新打标签批次为待审
            BLL.BatchMedia.BatchMedia.Instance.UpdatePendingSubmitTime(bmModel.BatchMediaID, batchAuditMode.BatchAuditID);
            return bmModel.BatchMediaID;
        }
        #endregion
    }
}
