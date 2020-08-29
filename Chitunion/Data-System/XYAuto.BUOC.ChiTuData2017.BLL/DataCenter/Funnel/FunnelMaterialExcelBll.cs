using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.Dal.DataCenter;
using XYAuto.BUOC.ChiTuData2017.Entities.DataCenter;

namespace XYAuto.BUOC.ChiTuData2017.BLL.DataCenter.Funnel
{
    public class FunnelMaterialExcelBll
    {
        #region 初始化
        public FunnelMaterialExcelBll()
        {
        }
        static FunnelMaterialExcelBll instance = null;
        static readonly object padlock = new object();

        static int ExcelCount = DataCommon.GetAppSettingInt32Value("ExcelQuantity");
        public static FunnelMaterialExcelBll Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new FunnelMaterialExcelBll();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        public Tuple<MemoryStream, string, bool> ReturnExcel(BasicQueryArgs queryArgs)
        {
            switch (queryArgs.ChartType)
            {
                case "ld_tou":
                    return FunnelHeadExport(queryArgs);
                    
                case "ld_yao":
                    return FunnelWaistExport(queryArgs);
                   
                case "ld_wul":
                    return FunnelMaterialExport(queryArgs);
                 
            }
            return null;
        }

        #region 漏斗头部导出

        public Tuple<MemoryStream, string, bool> FunnelHeadExport(BasicQueryArgs queryArgs)
        {
            string excelUrl = string.Empty;
            DataTable FunnelTable = FunnelMaterialDa.Instance.GetFunnel_Head_List(queryArgs).Tables[0];
            if (FunnelTable.Rows.Count > 0)
            {
                if (FunnelTable.Rows.Count > ExcelCount)
                {
                    return new Tuple<MemoryStream, string, bool>(null, string.Empty, false);

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


                return new Tuple<MemoryStream, string, bool>(ExcelHelper.Export(FunnelTable, dictSieve), "漏斗头部列表", true);

            }
            return null;

        }
        #endregion

        #region 漏斗腰部导出

        public Tuple<MemoryStream, string, bool> FunnelWaistExport(BasicQueryArgs queryArgs)
        {
            DataTable WaistTable = FunnelMaterialDa.Instance.GetFunnel_Waist_List(queryArgs).Tables[0];
            string excelUrl = string.Empty;
            if (WaistTable.Rows.Count > 0)
            {
                if (WaistTable.Rows.Count > ExcelCount)
                {
                    return new Tuple<MemoryStream, string, bool>(null, string.Empty, false);

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



                return new Tuple<MemoryStream, string, bool>(ExcelHelper.Export(WaistTable, dictSieve), "漏斗腰部列表", true);

            }
            return null;

        }
        #endregion

        #region 漏斗物料导出

        public Tuple<MemoryStream, string, bool> FunnelMaterialExport(BasicQueryArgs queryArgs)
        {
            string excelUrl = string.Empty;
            DataTable HeadTable = FunnelMaterialDa.Instance.GetFunnel_Material_List(queryArgs).Tables[0];
            if (HeadTable.Rows.Count > 0)
            {
                if (HeadTable.Rows.Count > ExcelCount)
                {
                    return new Tuple<MemoryStream, string, bool>(null, string.Empty, false);

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


                return new Tuple<MemoryStream, string, bool>(ExcelHelper.Export(HeadTable, dictSieve), "漏斗物料列表", true);

            }
            return null;

        }
        #endregion
    }
}
