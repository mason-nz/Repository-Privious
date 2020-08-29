using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Workload;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;
using XYAuto.BUOC.ChiTuData2017.WebAPI.Common;

namespace XYAuto.BUOC.ChiTuData2017.WebAPI.ExcelOperation
{
    public partial class MaterielExportExcel : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string beginTime = Request["BeginTime"] + string.Empty; //开始时间
                string endTime = Request["EndTime"] + string.Empty;//结束时间
                string materialID = Request["MaterialID"] + string.Empty;//物料类型
                string channelID = Request["ChannelID"] + string.Empty;//渠道
                string sceneID = Request["SceneID"] + string.Empty;//场景
                string accountID = Request["AccountID"] + string.Empty;//账号分值
                string conditionID = Request["ConditionID"] + string.Empty;//状态
                string listType = Request["ListType"] + string.Empty;//	列表类型
                var Tuple = DataStreamExportExceBll.Instance.DetailExport(new ListQueryArgs
                {
                    BeginTime = beginTime,
                    EndTime = endTime,
                    MaterialID = !string.IsNullOrEmpty(materialID) ? Convert.ToInt32(materialID) : 0,
                    ChannelID = !string.IsNullOrEmpty(materialID) ? Convert.ToInt32(materialID) : 0,
                    SceneID = !string.IsNullOrEmpty(materialID) ? Convert.ToInt32(materialID) : 0,
                    AccountID = !string.IsNullOrEmpty(accountID) ? Convert.ToInt32(materialID) : 0,
                    ConditionID = !string.IsNullOrEmpty(conditionID) ? Convert.ToInt32(materialID) : 0,
                    ListType = listType
                });
                if (Tuple == null)
                {
                    ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>MessageBox('导出失败');</script>");
                }
                else
                {
                    if (!Tuple.Item3)
                    {
                        ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>MessageBox('数据量过大');</script>");

                    }
                    else
                    {
                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", HttpUtility.UrlEncode(Tuple.Item2.ToString(), System.Text.Encoding.UTF8)));
                        Response.BinaryWrite(Tuple.Item1.ToArray());
                        Response.End();
                        Tuple.Item1.Close();
                        Tuple.Item1.Dispose();
                    }
                }
            }
            catch (Exception x)
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>MessageBox('导出异常');</script>");
                Loger.Log4Net.Error($"导出物料列表出错", x);
            }
        }
    }
}