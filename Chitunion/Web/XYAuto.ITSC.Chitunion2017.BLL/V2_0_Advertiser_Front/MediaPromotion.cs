using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V_2_0;

namespace XYAuto.ITSC.Chitunion2017.BLL.V2_0_Advertiser_Front
{
    public class MediaPromotion
    {
        public static readonly MediaPromotion Instance = new MediaPromotion();
        public Dictionary<string, object> GetMediaPromotionList(string Name, int Status, int PageIndex, int PageSize)
        {
            Common.LoginUser u = Common.UserInfo.GetLoginUser();
            DataTable dt = Dal.V2_0_Advertiser_Front.MediaPromotion.Instance.GetMediaPromotionList(u.UserID, Name == null ? "" : Name.Trim(), Status, PageIndex, PageSize);
            List<RespPromotionDto> list = Util.DataTableToList<RespPromotionDto>(dt);
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            dicAll.Add("TotalCount", 0);
            if (list.Count > 0)
            {
                dicAll["TotalCount"] = dt.Columns["TotalCount"].Expression;
            }
            dicAll.Add("List", list);
            return dicAll;
        }
        public RespPromotionDto GetMediaPromotionInfo(int RecID)
        {
            Common.LoginUser u = Common.UserInfo.GetLoginUser();
            DataSet ds = Dal.V2_0_Advertiser_Front.MediaPromotion.Instance.GetMediaPromotionInfo(RecID, u.UserID);
            RespPromotionDto resp = Util.DataTableToEntity<RespPromotionDto>(ds.Tables[0]);
            if (resp != null)
            {

                if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                {
                    DataRow drCar;
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        drCar = ds.Tables[1].Rows[i];
                        resp.CarStyleText += drCar["CarStyleText"].ToString() + "、";
                    }
                    if (!string.IsNullOrEmpty(resp.CarStyleText))
                    {
                        resp.CarStyleText = resp.CarStyleText.Remove(resp.CarStyleText.Length - 1, 1);
                    }
                }
                if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                {
                    DataRow drArea;
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        drArea = ds.Tables[2].Rows[i];
                        resp.AreaText += drArea["AreaText"].ToString() + "、";
                    }
                    if (!string.IsNullOrEmpty(resp.AreaText))
                    {
                        resp.AreaText = resp.AreaText.Remove(resp.AreaText.Length - 1, 1);
                    }
                }
            }
            return resp;
        }
        public string AddMediaPromotionInfo(ReqPromotionDto Dto)
        {
            string error = Verification(Dto);
            if (error != "")
            {
                return error;
            }
            Common.LoginUser u = Common.UserInfo.GetLoginUser();
            if (!u.RoleIDs.Contains("SYS007RL00025"))
            {
                return "对不起，您没有权限添加";
            }
            int mediaID = Dal.V2_0_Advertiser_Front.MediaPromotion.Instance.AddMediaPromotionInfo(Dto, u.UserID);
            if (mediaID <= 0)
            {
                return "添加失败，请重试！";
            };
            if (Dal.V2_0_Advertiser_Front.MediaPromotion.Instance.AddCarPromotion(Dto.CarList, mediaID) <= 0)
            {
                Loger.Log4Net.Error("添加媒体智投推广计划->mediaID:" + mediaID + "广联车型失败");
            }
            if (Dal.V2_0_Advertiser_Front.MediaPromotion.Instance.AddAreaPromotion(Dto.AreaList, mediaID) <= 0)
            {
                Loger.Log4Net.Error("添加媒体智投推广计划->mediaID:" + mediaID + "广联地区失败");
            }
            return "";
        }
        public string Verification(ReqPromotionDto Dto)
        {
            if (Dto == null)
            {
                return "参数错误";
            }
            if (string.IsNullOrWhiteSpace(Dto.Name))
            {
                return "请输入推广名称！";
            }
            if (Util.GetLength(Dto.Name.Trim()) > 40)
            {
                return "推广名称最多可输入20个汉字！";
            }

            if (!Dal.V2_0_Advertiser_Front.MediaPromotion.Instance.IsAvailableByName(Dto.Name))
            {
                return "推广名称不能重复！";
            }
            if (Dto.CarList == null || Dto.CarList.Count <= 0)
            {
                return "请选择推广车型！";
            }
            if (Dto.CarList.Count > 20)
            {
                return "最多可选择20个车型！";
            }
            if (Dto.AreaList == null || Dto.AreaList.Count <= 0)
            {
                return "请选择推广省市！";
            }
            if (Dto.AreaList.Count > 20)
            {
                return "最多可选择20个城市！";
            }
            if (!string.IsNullOrWhiteSpace(Dto.MaterialUrl))
            {
                if (Util.GetLength(Dto.MaterialUrl.Trim()) > 200)
                {
                    return "物料路径长度不得超过200";
                }
                if (Dto.MaterialUrl.Contains("."))
                {
                    string suffixName = Dto.MaterialUrl.Substring(Dto.MaterialUrl.LastIndexOf('.') + 1).ToUpper();
                    if (!(suffixName == "ZIP" || suffixName == "RAR"))
                    {
                        return "推广物料格式不正确!";
                    }
                }
                else
                {
                    return "推广物料格式不正确!";
                }
                string errore = Util.ValidateImgSize(Dto.MaterialUrl.Trim(), 5 * 1024);
                if (errore != "")
                {
                    return errore;
                }
            }

            if (Dto.BudgetPrice < 3000)
            {
                return "推广预算最低为3000元！";
            }
            if (string.IsNullOrWhiteSpace(Dto.BeginTime) || string.IsNullOrWhiteSpace(Dto.EndTime))
            {
                return "请选择推广时间！";
            }
            if (Convert.ToDateTime(Dto.BeginTime).Date < DateTime.Now.Date.AddDays(3))
            {
                return $"推广日期不能早于{ DateTime.Now.Date.AddDays(3)}的日期";
            }
            if (Convert.ToDateTime(Dto.BeginTime).Date > Convert.ToDateTime(Dto.EndTime))
            {
                return "结束日期必须大于开始日期！";
            }
            if (!string.IsNullOrWhiteSpace(Dto.Remark))
            {
                if (Util.GetLength(Dto.Remark.Trim()) > 4000)
                {
                    return "推广简介最多可输入2000个汉字！";
                }
            }
            return "";
        }

    }
}
