using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_11;
using XYAuto.ITSC.Chitunion2017.Entities.Materiel;
using XYAuto.ITSC.Chitunion2017.WebAPI.App_Start;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;
using XYAuto.ITSC.Chitunion2017.WebAPI.Filter;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    [CrossSite]
    [LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
    public class MaterielChannelDataController : ApiController
    {
        [HttpPost]
        public JsonResult SaveData([FromBody]MaterielChannelData req)
        {
            string msg = "保存失败";
            bool res = BLL.Materiel.MaterielChannelData.Instance.SaveData(req, ref msg);
            return Util.GetJsonDataByResult(res, res ? "保存成功" : msg, res ? 0 : 1);
        }

        [HttpPost]
        public JsonResult DeleteData([FromBody]MaterielChannelData req)
        {
            string msg = "删除失败";
            bool res = BLL.Materiel.MaterielChannelData.Instance.Delete(req.RecID);
            return Util.GetJsonDataByResult(res, res ? "删除成功" : msg, res ? 0 : 1);
        }

        [HttpGet]
        public JsonResult GetDataList([FromUri]MaterielChannelData req)
        {
            string msg = "获取失败";
            GetDataListResDTO res = BLL.Materiel.MaterielChannelData.Instance.GetDataList(new int[] { req.MaterielID });
            return Util.GetJsonDataByResult(res, res != null ? "获取成功" : msg, res == null ? 1 : 0);
        }

        [HttpPost]
        public JsonResult ExportToExcel([FromBody]MaterielChannelData req)
        {
            string msg = "导出失败";
            string filePath = BLL.Materiel.MaterielChannelData.Instance.ExportToExcel(new int[] { req.MaterielID });
            return Util.GetJsonDataByResult(filePath, !string.IsNullOrWhiteSpace(filePath) ? "导出成功" : msg, !string.IsNullOrWhiteSpace(filePath) ? 0 : 1);
        }

        [HttpPost]
        public JsonResult BatchExportToExcel([FromBody]MaterielChannelData req)
        {
            if (req.MaterielIDs == null || req.MaterielIDs.Length == 0)
            {
                return Util.GetJsonDataByResult(null, "没有选择任何物料", 1);
            }
            string msg = "导出失败";
            string filePath = BLL.Materiel.MaterielChannelData.Instance.ExportToExcel(req.MaterielIDs,true);
            return Util.GetJsonDataByResult(filePath, !string.IsNullOrWhiteSpace(filePath) ? "导出成功" : msg, !string.IsNullOrWhiteSpace(filePath) ? 0 : 1);
        }

    }
}
