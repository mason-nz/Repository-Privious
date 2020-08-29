using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_11;

namespace XYAuto.ITSC.Chitunion2017.BLL.Materiel
{
    public class MaterielChannelData
    {
        public static readonly MaterielChannelData Instance = new MaterielChannelData();

        public bool SaveData(Entities.Materiel.MaterielChannelData req, ref string msg)
        {
            var ur = Common.UserInfo.GetUserRole();
            if (req.ChannelID.Equals(0) && req.RecID.Equals(0))
            {
                msg = "缺少渠道ID!";
                return false;
            }
            if (req.ReadCount < 0)
            {
                msg = "阅读数错误!";
                return false;
            }
            if (req.LikeCount < 0)
            {
                msg = "点赞数错误!";
                return false;
            }
            if (req.CommentCount < 0)
            {
                msg = "评论数错误!";
                return false;
            }
            req.CreateUserID = ur.UserID;
            req.CreateTime = DateTime.Now;
            req.LastUpdateTime = DateTime.Now;
            bool res = false;
            if (req.RecID.Equals(0))
            {
                res = Dal.Materiel.MaterielChannelData.Instance.CheckDataDateRepeat(req.ChannelID, req.DataDate.Date);
                if (res)
                {
                    msg = "有效期重复";
                    return false;
                }
                res = Dal.Materiel.MaterielChannelData.Instance.Add(req);
            }
            else
            {
                res = Dal.Materiel.MaterielChannelData.Instance.CheckDataDateRepeat(req.ChannelID, req.DataDate.Date, req.RecID);
                if (res)
                {
                    msg = "有效期重复";
                    return false;
                }
                res = Dal.Materiel.MaterielChannelData.Instance.Update(req);
            }
            return res;
        }

        public bool Delete(int recID)
        {
            return Dal.Materiel.MaterielChannelData.Instance.Delete(recID);
        }

        public GetDataListResDTO GetDataList(int[] materielIDs)
        {
            GetDataListResDTO res = new GetDataListResDTO();
            //固定排序
            Dictionary<string, int> orderByDict = new Dictionary<string, int>();
            orderByDict.Add("微信", 1);
            orderByDict.Add("微博", 2);
            orderByDict.Add("直播", 3);
            orderByDict.Add("视频", 4);
            var allList = Dal.Materiel.MaterielChannelData.Instance.GetListByMaterielID(materielIDs);
            //按类型
            var typeGroupList = allList.GroupBy(i => i.MediaTypeName).ToList();
            foreach (var typeGroup in typeGroupList)
            {
                TypeItem typeItem = new TypeItem();
                typeItem.MediaTypeName = typeGroup.Key;
                if (orderByDict.Keys.Contains(typeGroup.Key))
                    typeItem.OrderBy = orderByDict[typeGroup.Key];
                else
                    typeItem.OrderBy = orderByDict.Count + 1;
               
                //按媒体
                var mediaGroupList = typeGroup.GroupBy(i => new { i.ChannelID,i.MediaNumber,i.MediaName }).ToList();
                foreach (var mediaGroup in mediaGroupList)
                {
                    MediaItem mediaItem = new MediaItem();
                    mediaItem.ChannelID = mediaGroup.Key.ChannelID;
                    mediaItem.MediaNumber = mediaGroup.Key.MediaNumber;
                    mediaItem.MediaName = mediaGroup.Key.MediaName;
                    mediaItem.DataList = mediaGroup.Where(i=>!i.RecID.Equals(0)) .ToList();
                    typeItem.MediaList.Add(mediaItem);
                }
                res.TypeList.Add(typeItem);
            }
            res.TypeList = res.TypeList.OrderBy(i => i.OrderBy).ToList();

            //合计
            var dateGroupList = allList.Where(i=>!i.RecID.Equals(0)).GroupBy(i => i.DataDate).OrderBy(i=>i.Key).ToList();
            foreach (var dateItem in dateGroupList)
            {
                Entities.Materiel.MaterielChannelData totalItem = new Entities.Materiel.MaterielChannelData();
                totalItem.DataDate = dateItem.Key;
                totalItem.ReadCount = dateItem.Sum(i => i.ReadCount);
                totalItem.LikeCount = dateItem.Sum(i => i.LikeCount);
                totalItem.CommentCount = dateItem.Sum(i => i.CommentCount);
                res.Total.Add(totalItem);
            }
            return res;
        }

        public string ExportToExcel(int[] materielIDs, bool IsBatch = false)
        {
            string pagePath = string.Format("{0}\\UploadFiles\\MaterielData\\{1}\\{2}\\{3}\\{4}",
                WebConfigurationManager.AppSettings["UploadFilePath"],
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                DateTime.Now.Hour);
            if (!Directory.Exists(pagePath))
                Directory.CreateDirectory(pagePath);
            string fileName = string.Empty;
            if (IsBatch)
            {
                fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".xls";
            }
            else
            {
                var materiel = Dal.Materiel.MaterielExtend.Instance.GetEntity(materielIDs[0]);
                fileName = materiel.Name + ".xls";
            }
            string backFilePath = pagePath + "\\" + fileName;
            string frontFilePath = string.Format("/uploadfiles/MaterielData/{0}/{1}", DateTime.Now.ToString("yyyy/M/d/H"), fileName);
            var res = this.GetDataList(materielIDs);
            if (res.TypeList != null && res.TypeList.Count > 0)
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                string headerStr = "物料ID|物料名称|落地页地址|关联车型|媒体类型|媒体名称|渠道类型|费用类型|付费模式|单位成本|数据日期|阅读数|点赞数|评论数";
                //填充表头
                IRow dataRow = sheet.CreateRow(0);
                string[] headers = headerStr.Split('|');
                for (int i = 0; i < headers.Length; i++)
                    dataRow.CreateCell(i).SetCellValue(headers[i]);
                #region 逐行渲染 类型-媒体-数据
                int rownumber = 1;
                foreach (var typeitem in res.TypeList)
                {
                    foreach (var mediaitem in typeitem.MediaList)
                    {
                        foreach (var item in mediaitem.DataList)
                        {
                            dataRow = sheet.CreateRow(rownumber);
                            dataRow.CreateCell(0).SetCellValue(item.MaterielID);
                            dataRow.CreateCell(1).SetCellValue(item.MaterialName);
                            dataRow.CreateCell(2).SetCellValue(item.FootContentURL);
                            dataRow.CreateCell(3).SetCellValue((string.IsNullOrEmpty(item.BrandName)?"":(item.BrandName + "—")) +  item.CarSerialName);

                            dataRow.CreateCell(4).SetCellValue(typeitem.MediaTypeName);
                            dataRow.CreateCell(5).SetCellValue(mediaitem.MediaNumber + (string.IsNullOrWhiteSpace(mediaitem.MediaName) ? "" : ("|" + mediaitem.MediaName)));
                            dataRow.CreateCell(6).SetCellValue(item.ChannelTypeName);
                            dataRow.CreateCell(7).SetCellValue(item.PayTypeName);
                            if (item.PayTypeName != "免费")
                            {
                                dataRow.CreateCell(8).SetCellValue(item.PayModeName);
                                dataRow.CreateCell(9).SetCellValue((double)item.UnitCost);
                            }
                            dataRow.CreateCell(10).SetCellValue(item.DataDate.ToString("yyyy-MM-dd"));
                            dataRow.CreateCell(11).SetCellValue(item.ReadCount);
                            dataRow.CreateCell(12).SetCellValue(item.LikeCount);
                            dataRow.CreateCell(13).SetCellValue(item.CommentCount);
                            rownumber++;
                        }
                    }
                }
                #endregion
                using (FileStream fs = new FileStream(backFilePath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }
            }
            return frontFilePath;
        }
    }
}
