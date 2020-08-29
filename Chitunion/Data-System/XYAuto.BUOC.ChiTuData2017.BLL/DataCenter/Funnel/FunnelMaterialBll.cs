using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Dto;
using XYAuto.BUOC.ChiTuData2017.Dal.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Funnel;

namespace XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Funnel
{
    public class FunnelMaterialBll
    {
        #region 初始化
        public FunnelMaterialBll()
        {
        }
        static FunnelMaterialBll instance = null;
        static readonly object padlock = new object();

        public static FunnelMaterialBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new FunnelMaterialBll();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region 漏斗分析—头部列表
        /// <summary>
        /// 根据参数获取头部列表
        /// </summary>
        /// <param name="queryArgs">查询参数</param>
        /// <returns></returns>
        public BasicResultDto GetFunnelHeadList(BasicQueryArgs queryArgs)
        {
            var queryTable = FunnelMaterialDa.Instance.GetFunnel_Head_List(queryArgs).Tables[0];

            return new BasicResultDto { List = DataCommon.DataTableToList<FunnelHeadDetailInfo>(queryTable) };
        }
        #endregion

        #region 漏斗分析—腰部列表
        /// <summary>
        /// 根据参数获取腰部列表
        /// </summary>
        /// <param name="queryArgs">查询参数</param>
        /// <returns></returns>
        public BasicResultDto GetFunnelWaistDetailList(BasicQueryArgs queryArgs)
        {
            var queryTable = FunnelMaterialDa.Instance.GetFunnel_Waist_List(queryArgs).Tables[0];

            return new BasicResultDto { List = DataCommon.DataTableToList<FunnelWaistDetailInfo>(queryTable) };

        }
        #endregion

        #region 漏斗分析—物料列表
        /// <summary>
        /// 根据参数获取物料列表
        /// </summary>
        /// <param name="queryArgs">查询参数</param>
        /// <returns></returns>
        public BasicResultDto GetFunnelMaterialList(BasicQueryArgs queryArgs)
        {
            var queryTable = FunnelMaterialDa.Instance.GetFunnel_Material_List(queryArgs).Tables[0];


            return new BasicResultDto { List = DataCommon.DataTableToList<FunnelMaterialDetailInfo>(queryTable) };
        }
        #endregion

        #region 漏斗头部导出

        public ResultExcelInfo FunnelHeadExport(BasicQueryArgs queryArgs)
        {
            string excelUrl = string.Empty;
            DataTable FunnelTable = FunnelMaterialDa.Instance.GetFunnel_Head_List(queryArgs).Tables[0];
            if (FunnelTable.Rows.Count > 0)
            {
                if (FunnelTable.Rows.Count > 100000)
                {
                    return new ResultExcelInfo { Status = -1, Url = null, Message = "数据量过大，请分批导出" };
                }
                string strKey = string.Empty;
                string strValue = string.Empty;
                switch (queryArgs.Operator)
                {
                    case 1:
                        strKey = "SceneName";
                        strValue = "场景";
                        break;
                    case 2:
                        strKey = "ChannelName";
                        strValue = "渠道";
                        break;
                    case 3:
                        strKey = "AAScoreTypeName";
                        strValue = "文章分值";
                        break;
                }
                Dictionary<string, string> dictSieve = new Dictionary<string, string> {
                { strKey,strValue},
                { "SpiderArticleCount","抓取文章"},
                { "SpiderAccountCount","抓取账号"},
                { "AutoArticleCount","机洗入库文章"},
                { "AutoAccountCount","机洗入库账号"},
                { "PrimaryArticleCount","初筛保留文章"},
                { "PrimaryAccountCount","初筛保留账号"},
                { "ArtificialArticleCount","清洗保留文章"},
                { "ArtificialAccountCount","清洗保留账号"},
                { "EncapsulateArticleCount","封装使用文章"},
                { "EncapsulateAccountCount","封装使用账号"} };

                excelUrl = ExcelHelper.ExportEcel(FunnelTable, "漏斗头部文章列表", dictSieve);
            }
            else
            {
                return new ResultExcelInfo { Status = -2, Url = null, Message = "数据为空，不可导出 " };
            }
            return new ResultExcelInfo { Url = new { Url = excelUrl } };

        }
        #endregion

        #region 漏斗腰部导出

        public ResultExcelInfo FunnelWaistExport(BasicQueryArgs queryArgs)
        {
            DataTable WaistTable = FunnelMaterialDa.Instance.GetFunnel_Waist_List(queryArgs).Tables[0];
            string excelUrl = string.Empty;
            if (WaistTable.Rows.Count > 0)
            {
                if (WaistTable.Rows.Count > 100000)
                {
                    return new ResultExcelInfo { Status = -1, Url = null, Message = "数据量过大，请分批导出" };
                }
                string strKey = string.Empty;
                string strValue = string.Empty;
                switch (queryArgs.Operator)
                {
                    case 1:
                        strKey = "Category";
                        strValue = "文章类别";
                        break;
                    case 2:
                        strKey = "ChannelName";
                        strValue = "渠道";
                        break;

                }
                Dictionary<string, string> dictSieve = new Dictionary<string, string> {
                { strKey,strValue},
                { "SpiderCount","抓取"},
                { "AutoCleanCount","机洗"},
                { "MatchedCount","匹配车型"},
                { "ArtificialCount","清洗保留"},
                { "EncapsulateCount","封装使用"}};
                
                excelUrl = ExcelHelper.ExportEcel(WaistTable, "漏斗腰部文章列表", dictSieve);

            }
            else
            {
                return new ResultExcelInfo { Status = -2, Url = null, Message = "数据为空，不可导出 " };
            }
            return new ResultExcelInfo { Url = new { Url = excelUrl } };

        }
        #endregion

        #region 漏斗物料导出

        public ResultExcelInfo FunnelMaterialExport(BasicQueryArgs queryArgs)
        {
            string excelUrl = string.Empty;
            DataTable HeadTable = FunnelMaterialDa.Instance.GetFunnel_Material_List(queryArgs).Tables[0];
            if (HeadTable.Rows.Count > 0)
            {
                if (HeadTable.Rows.Count > 100000)
                {
                    return new ResultExcelInfo { Status = -1, Url = null, Message = "数据量过大，请分批导出" };
                }
                string strKey = string.Empty;
                string strValue = string.Empty;
                switch (queryArgs.Operator)
                {
                    case 1:
                        strKey = "SceneName";
                        strValue = "场景";
                        break;
                    case 2:
                        strKey = "ChannelName";
                        strValue = "渠道";
                        break;
                    case 3:
                        strKey = "AAScoreTypeName";
                        strValue = "头部文章分值";
                        break;

                }
                Dictionary<string, string> dictSieve = new Dictionary<string, string> {
                { strKey,strValue},
                { "Encapsulate","封装物料"},
                { "Distribute","分发物料"},
                { "Forward","转发次数"},
                { "Clue","线索数量"} };
                
                excelUrl = ExcelHelper.ExportEcel(HeadTable, "漏斗物料列表", dictSieve);

            }
            else
            {
                return new ResultExcelInfo { Status = -2, Url = null, Message = "数据为空，不可导出 " };
            }
            return new ResultExcelInfo { Url = new { Url = excelUrl } };
        }
        #endregion
    }
}
