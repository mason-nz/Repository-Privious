using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class DataImport
    {
        private static string CONNECTIONSTRINGS = ConfigurationManager.AppSettings["ConnectionStrings_ITSC"];

        public int InsertImportData(Import_PCAPPDTO dto, MediaTypeEnum mediaType)
        {
            var dalDataImport = new Dal.DataImport();
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //媒体新增、编辑
                        int mediaId = dalDataImport.MediaOperate(dto, mediaType);
                        if (mediaId <= 0)
                        {
                            var errorMsg = string.Format("媒体编辑、添加失败,类型：{0},参数：{1}", mediaType, GetMediaDtoJson(dto, mediaType));
                            Loger.Log4Net.ErrorFormat(errorMsg);
                            throw new Exception(errorMsg);
                        }
                        //互动参数新增、编辑
                        if (dalDataImport.InteractionOperate(importDto: dto, mediaType: mediaType, mediaId: mediaId) <= 0)
                        {
                            var errorMsg = string.Format(string.Format("互动参数编辑、添加失败,类型：{0},参数：{1}", mediaType, GetInteractionDtoJson(dto, mediaType)));
                            Loger.Log4Net.ErrorFormat(errorMsg);
                            throw new Exception(errorMsg);
                        }
                        //覆盖区域新增、编辑

                        int pubId = dalDataImport.AddPubBasic(mediaType, mediaId, dto.PubBasicInfo, trans);
                        if (pubId <= 0)
                        {
                            var errorMsg = string.Format("刊例base表信息添加失败,类型：{0},参数：{1}", mediaType, JsonConvert.SerializeObject(dto.PubBasicInfo));
                            Loger.Log4Net.ErrorFormat(errorMsg);
                            throw new Exception(errorMsg);
                        }
                        List<int> details = dalDataImport.AddPubDetail(mediaType, mediaId, pubId, dto.PubDetailList, trans);
                        if (details.First() == 0)
                        {
                            var errorMsg = string.Format("刊例详情表信息添加失败,类型：{0},参数：mediaId={1}&pubId={2},{3}", mediaType, mediaId, pubId, JsonConvert.SerializeObject(dto.PubDetailList));
                            Loger.Log4Net.ErrorFormat(errorMsg);
                            throw new Exception(errorMsg);
                        }
                        if (mediaType == MediaTypeEnum.APP)
                        {
                            var recId = dalDataImport.AddPubExtend(mediaId, pubId, details.First(), dto.PubExtend, trans);
                            if (recId <= 0)
                            {
                                var errorMsg = string.Format("app广告表信息添加失败,类型：{0},参数：mediaId={1}&pubId={2},{3}", mediaType, mediaId, pubId, JsonConvert.SerializeObject(dto.PubExtend));
                                Loger.Log4Net.ErrorFormat(errorMsg);
                                throw new Exception(errorMsg);
                            }
                        }
                        trans.Commit();
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        Loger.Log4Net.ErrorFormat("数据导入错误，Exception：{0}{1}{0}{2}", System.Environment.NewLine, ex, ex.StackTrace ?? string.Empty);
                        trans.Rollback();
                        return -1;
                    }
                }
            }
        }

        private string GetMediaDtoJson(Import_PCAPPDTO dto, MediaTypeEnum mediaType)
        {
            switch (mediaType)
            {
                case MediaTypeEnum.微信:
                    return JsonConvert.SerializeObject(dto.MediaWeixin);
                case MediaTypeEnum.APP:
                    return JsonConvert.SerializeObject(dto.MediaPcApp);
                case MediaTypeEnum.直播:
                    return JsonConvert.SerializeObject(dto.MediaBroadcast);
                case MediaTypeEnum.微博:
                    return JsonConvert.SerializeObject(dto.MediaWeibo);
                case MediaTypeEnum.视频:
                    return JsonConvert.SerializeObject(dto.MediaVideo);
                default:
                    return string.Empty;
            }
        }

        private string GetInteractionDtoJson(Import_PCAPPDTO dto, MediaTypeEnum mediaType)
        {
            switch (mediaType)
            {
                case MediaTypeEnum.微信:
                    return JsonConvert.SerializeObject(dto.InteractionWeixin);
                case MediaTypeEnum.直播:
                    return JsonConvert.SerializeObject(dto.InteractionBroadcast);
                case MediaTypeEnum.微博:
                    return JsonConvert.SerializeObject(dto.InteractionWeibo);
                case MediaTypeEnum.视频:
                    return JsonConvert.SerializeObject(dto.InteractionVideo);
                default:
                    return string.Empty;
            }
        }
    }
}
