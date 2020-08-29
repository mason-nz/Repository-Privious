using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.ScoreTableManage
{
    public partial class EditScoreTable :PageBase
    {

        public string QS_RTID
        {
            get
            {
                return HttpContext.Current.Request["QS_RTID"] == null ? string.Empty :
                   HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["QS_RTID"].ToString());
            }
        }

        public string Status = "";
        public string StatusInUse = "";
        public string NoRight = "0";
        protected string region = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!BLL.Util.CheckButtonRight("SYS024BUT600201"))
                {
                    NoRight = "1";
                }
                else
                {
                    BindData();
                }
            }
        }

        private void BindData()
        {
            int rtid = 0;
            if (int.TryParse(QS_RTID, out rtid))
            {
                Entities.QS_RulesTable QsTable = BLL.QS_RulesTable.Instance.GetQS_RulesTable(rtid);
                if (QsTable != null)
                {
                    Status = QsTable.Status.ToString();
                    StatusInUse = QsTable.StatusInUse.ToString();
                    region = QsTable.RegionID;
                    string JsonStr = GetJsonStrByRtid(rtid);
                    this.hidJsonStr.Value = JsonStr;
                }
            }
        }

        /// <summary>
        /// 根据ID，获取整个评分表的JSON格式数据
        /// </summary>
        /// <param name="rtid"></param>
        /// <returns></returns>
        private string GetJsonStrByRtid(int rtid)
        {
            int totalCount = 0;
            Entities.QS_RulesTable QsObject = BLL.QS_RulesTable.Instance.GetQS_RulesTable(rtid);

            Entities.QueryQS_Category queryC = new Entities.QueryQS_Category();
            queryC.QS_RTID = rtid;
            DataTable DtCategory = BLL.QS_Category.Instance.GetQS_Category(queryC, "", 1, 999999, out totalCount);

            Entities.QueryQS_Item queryI = new Entities.QueryQS_Item();
            queryI.QS_RTID = rtid;
            DataTable DtItem = BLL.QS_Item.Instance.GetQS_Item(queryI, "", 1, 999999, out totalCount);

            Entities.QueryQS_Standard queryS = new Entities.QueryQS_Standard();
            queryS.QS_RTID = rtid;
            DataTable DtStandard = BLL.QS_Standard.Instance.GetQS_Standard(queryS, "", 1, 999999, out totalCount);

            Entities.QueryQS_Marking queryM = new Entities.QueryQS_Marking();
            queryM.QS_RTID = rtid;
            DataTable DtMarking = BLL.QS_Marking.Instance.GetQS_Marking(queryM, "", 1, 999999, out totalCount);

            Entities.QueryQS_DeadOrAppraisal queryD = new Entities.QueryQS_DeadOrAppraisal();
            queryD.QS_RTID = rtid;
            DataTable DtDead = BLL.QS_DeadOrAppraisal.Instance.GetQS_DeadOrAppraisal(queryD, "", 1, 999999, out totalCount);

            string jsonStr = GetJsonStrByDataTable(QsObject, DtCategory, DtItem, DtStandard, DtMarking, DtDead);

            return jsonStr;
        }

        private string GetJsonStrByDataTable(Entities.QS_RulesTable QsObject, DataTable DtCategory, DataTable DtItem,
                                                             DataTable DtStandard, DataTable DtMarking, DataTable DtDead)
        {
            string jsonStr = "";

            ScoreTableInfo tableObj = new ScoreTableInfo();
            Catage[] Catage = new Catage[DtCategory.Rows.Count];

            Dead[] Dead = new Dead[DtDead.Rows.Count];

            //分类
            for (int ci = 0; ci < DtCategory.Rows.Count; ci++)
            {
                Catage[ci] = new Catage();
                Catage[ci].CID = DtCategory.Rows[ci]["QS_CID"].ToString();
                Catage[ci].Name = DtCategory.Rows[ci]["Name"].ToString();
                Catage[ci].Score = DtCategory.Rows[ci]["Score"].ToString();
                Catage[ci].Status = DtCategory.Rows[ci]["Status"].ToString();

                //质检项目
                DataRow[] IRows = DtItem.Select("QS_CID=" + DtCategory.Rows[ci]["QS_CID"].ToString());
                Item[] Item = new Item[IRows.Length];
                for (int ii = 0; ii < IRows.Length; ii++)
                {
                    Item[ii] = new Item();
                    Item[ii].IID = IRows[ii]["QS_IID"].ToString();
                    Item[ii].ItemName = IRows[ii]["ItemName"].ToString();
                    Item[ii].Score = IRows[ii]["Score"].ToString();
                    Item[ii].Status = IRows[ii]["Status"].ToString();
                    Item[ii].CID = IRows[ii]["QS_CID"].ToString();

                    //质检标准
                    DataRow[] SRows = DtStandard.Select("QS_IID=" + IRows[ii]["QS_IID"].ToString());
                    Standard[] Standard = new Standard[SRows.Length];
                    for (int si = 0; si < SRows.Length; si++)
                    {
                        Standard[si] = new Standard();
                        Standard[si].SID = SRows[si]["QS_SID"].ToString();
                        Standard[si].IID = SRows[si]["QS_IID"].ToString();
                        Standard[si].CID = SRows[si]["QS_CID"].ToString();
                        Standard[si].SName = SRows[si]["ScoringStandardName"].ToString();
                        Standard[si].Score = SRows[si]["Score"].ToString();
                        Standard[si].IsIsDead = SRows[si]["IsIsDead"].ToString();
                        Standard[si].Status = SRows[si]["Status"].ToString();
                        Standard[si].SExplanation = SRows[si]["SExplanation"].ToString();
                        Standard[si].SkillLevel = SRows[si]["SkillLevel"].ToString();

                        //扣分项
                        DataRow[] MRow = DtMarking.Select("QS_SID=" + SRows[si]["QS_SID"].ToString());
                        Marking[] Marking = new Marking[MRow.Length];
                        for (int mi = 0; mi < MRow.Length; mi++)
                        {
                            Marking[mi] = new Marking();
                            Marking[mi].CID = MRow[mi]["QS_CID"].ToString();
                            Marking[mi].IID = MRow[mi]["QS_IID"].ToString();
                            Marking[mi].SID = MRow[mi]["QS_SID"].ToString();
                            Marking[mi].MID = MRow[mi]["QS_MID"].ToString();
                            Marking[mi].MName = MRow[mi]["MarkingItemName"].ToString();
                            Marking[mi].Score = MRow[mi]["Score"].ToString();
                            Marking[mi].Status = MRow[mi]["Status"].ToString();
                        }
                        Standard[si].Marking = Marking;
                    }
                    Item[ii].Standard = Standard;
                }
                Catage[ci].Item = Item;
            }
            tableObj.Catage = Catage;

            //致命项
            for (int di = 0; di < DtDead.Rows.Count; di++)
            {
                Dead[di] = new Dead();
                Dead[di].DID = DtDead.Rows[di]["QS_DAID"].ToString();
                Dead[di].DName = DtDead.Rows[di]["DeadItemName"].ToString();
                Dead[di].Status = DtDead.Rows[di]["Status"].ToString();
            }
            tableObj.Dead = Dead;

            //评价表的属性
            tableObj.RTID = QsObject.QS_RTID.ToString();
            tableObj.Name = QsObject.Name;
            tableObj.ScoreType = QsObject.ScoreType.ToString();
            tableObj.Description = QsObject.Description;
            tableObj.Status = QsObject.Status.ToString();
            tableObj.Appraisal = QsObject.HaveQAppraisal.ToString();
            tableObj.DeadItemNum = QsObject.DeadItemNum.ToString();
            tableObj.NoDeadItemNum = QsObject.NoDeadItemNum.ToString();
            tableObj.RegionID = QsObject.RegionID;

            jsonStr = Newtonsoft.Json.JavaScriptConvert.SerializeObject(tableObj);

            jsonStr = HttpContext.Current.Server.UrlEncode(jsonStr);
            jsonStr = jsonStr.Replace("+", "%20");

            return jsonStr;
        }
    }
}