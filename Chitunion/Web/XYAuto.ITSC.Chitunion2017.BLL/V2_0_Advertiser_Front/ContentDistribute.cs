using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V_2_0;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.V2_0_Advertiser_Front
{
    public class ContentDistribute
    {
        public static readonly ContentDistribute Instance = new ContentDistribute();
        public Dictionary<string, object> GetContentDistributeList(string Name, int Status, int PageIndex, int PageSize)
        {
            Common.LoginUser u = Common.UserInfo.GetLoginUser();
            DataTable dt = Dal.V2_0_Advertiser_Front.ContentDistribute.Instance.GetContentDistributeList(u.UserID, Name == null ? "" : Name.Trim(), Status, PageIndex, PageSize);
            List<RespDistributeDto> list = Util.DataTableToList<RespDistributeDto>(dt);
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            dicAll.Add("TotalCount", 0);
            if (list.Count > 0)
            {
                dicAll["TotalCount"] = dt.Columns["TotalCount"].Expression;
            }
            dicAll.Add("List", list);
            return dicAll;
        }
        public RespDistributeDto GetContentDistributeDetailInfo(int RecID)
        {
            Common.LoginUser u = Common.UserInfo.GetLoginUser();
            DataTable dt = Dal.V2_0_Advertiser_Front.ContentDistribute.Instance.GetContentDistributeDetailInfo(RecID, u.UserID);
            return Util.DataTableToEntity<RespDistributeDto>(dt);
        }
        public string AddContentDistributeInfo(ReqDistributeDto Dto)
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
            if (Dal.V2_0_Advertiser_Front.ContentDistribute.Instance.AddContentDistributeInfo(Dto, u.UserID) <= 0)
            {
                return "添加失败，请重试！";
            };
            return "";
        }
        public string Verification(ReqDistributeDto Dto)
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

            if (!Dal.V2_0_Advertiser_Front.ContentDistribute.Instance.IsAvailableByName(Dto.Name))
            {
                return "推广名称不能重复！";
            }
            if (string.IsNullOrWhiteSpace(Dto.Link))
            {
                return "请输入推广链接！";
            }
            if (Util.GetLength(Dto.Link.Trim()) > 255 || !Util.CheckIsFormat(@"(http://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", Dto.Link.Trim()))
            {
                return "推广链接格式不正确！";
            }
            if (string.IsNullOrWhiteSpace(Dto.Synopsis))
            {
                return "请输入推广简介！";
            }
            if (Util.GetLength(Dto.Synopsis.Trim()) < 40)
            {
                return "推广简介最少需输入20个汉字！";
            }
            if (Util.GetLength(Dto.Synopsis.Trim()) > 4000)
            {
                return "推广简介最多可输入2000个汉字！";
            }
            if (string.IsNullOrWhiteSpace(Dto.ImgUrl))
            {
                return "请上传推广图片！";
            }
            if (Util.GetLength(Dto.ImgUrl.Trim()) > 200)
            {
                return "图片路径长度不得超过200";
            }
            if (Dto.ImgUrl.Contains("."))
            {
                string suffixName = Dto.ImgUrl.Substring(Dto.ImgUrl.LastIndexOf('.') + 1).ToUpper();
                if (!(suffixName == "JPG" || suffixName == "JPEG" || suffixName == "PNG"))
                {
                    return "推广图片格式不正确!";
                }
            }
            else
            {
                return "推广图片格式不正确!";
            }
            string errore = Util.ValidateImgSize(Dto.ImgUrl.Trim(), 5 * 1024);
            if (errore != "")
            {
                return errore;
            }
            if (Dto.Platform <= 0)
            {
                return "请选择推广平台！";
            }
            if (!Enum.IsDefined(typeof(PayModeEnum), Dto.BillingMode))
            {
                return "请选择计费模式！";
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
            return "";
        }

    }
}
