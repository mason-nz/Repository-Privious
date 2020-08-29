using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard
{
    public partial class UCQualityStandardFiveLevelEdit : System.Web.UI.UserControl
    {
        //是否有致命项
        public string haveDead = "display:none";
        //是否有质检评价
        public string haveQulity = "display:none";

        //表名后缀
        public string tableEndName
        {
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["tableEndName"]))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["tableEndName"].ToString());
                }
            }
        }

        //致命项和质检评价顺序号
        public int DeadNum = 0;
        public int QulityNum = 0;

        //分类
        public DataTable dtCategory = null;
        //项目
        public DataTable dtItem = null;
        //标准
        public DataTable dtStandard = null;
        //扣分项
        public DataTable dtMarking = null;
        //致命项
        public DataTable dtDead = null;
        //评分规则表
        public DataTable dtRulesTable = null;
        //质检结果明细表
        public DataTable dtResultDetail = null;
        //成绩主表
        public DataTable dtResult = null;
        //致命项数
        public string DeadMIdNum = "0";
        //非致命项数
        public string NoDeadMIdNum = "0";
        public string txtQualityInfo = string.Empty;
        //评分表主键
        private int _qs_rtid = 0;
        public int QS_RTID
        {
            set
            {
                _qs_rtid = value;
            }
            get
            {
                return _qs_rtid;
            }
        }
        //评分成绩表主键
        private int _qs_rid = 0;
        public int QS_RID
        {
            set
            {
                _qs_rid = value;
            }
            get
            {
                return _qs_rid;
            }
        }
        private Int64 _callid = 0;
        public Int64 CallID
        {
            set
            {
                _callid = value;
            }
            get
            {
                return _callid;
            }
        }
        /// 对话ID
        /// <summary>
        /// 对话ID
        /// </summary>
        public int CSID { get; set; }
        public string ScoreType = "1";
        public string ScoreTypeFlag = "";
        #region 取标题
        public string GetNum(string Category)
        {
            if (Category == "1")
            {
                return GetNum(DeadNum, "1");
            }
            else
            {
                return GetNum(QulityNum, "1");
            }
        }
        /// <summary>
        /// category 1是达标题，2是二级标题，3是三级标题
        /// </summary>
        /// <param name="num"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public string GetNum(int num, string category)
        {
            return BLL.Util.GetNum(num, category);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (CallID > 0)
                    {
                        hdrealQs_RID.Value = BLL.QS_Result.Instance.HasScored(CallID.ToString()) == true ? "y" : "n";
                    }

                    //如果Qs_RTID是0说明没有质检表，QS_RID是0说明没有质检过
                    //如果两个都有说明是带答题成绩的预览

                    DataSet ds = null;
                    //如果只有Qs_RTID没有QS_RID，说明是初始加载
                    if (QS_RTID != 0 && QS_RID == 0)
                    {
                        ds = BLL.QS_RulesTable.Instance.GetRulesTableDetailByQS_RTID(QS_RTID);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            dtRulesTable = ds.Tables[0];
                            dtCategory = ds.Tables[1];
                            dtItem = ds.Tables[2];
                            dtStandard = ds.Tables[3];
                            dtMarking = ds.Tables[4];
                            dtDead = ds.Tables[5];
                        }
                        //绑定
                        DataBound();
                    }
                    //质检保存过，二次进入页面
                    else if (QS_RTID != 0 && QS_RID != 0)
                    {
                        //带评分
                        ds = BLL.QS_RulesTable.Instance.GetRulesTableDetailByQS_RTID(QS_RTID, QS_RID);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            dtRulesTable = ds.Tables[0];
                            dtCategory = ds.Tables[1];
                            dtItem = ds.Tables[2];
                            dtStandard = ds.Tables[3];
                            dtMarking = ds.Tables[4];
                            dtDead = ds.Tables[5];
                            dtResultDetail = ds.Tables[6];
                            if (CallID > 0)
                            {
                                dtResult = ds.Tables[7];
                            }
                            else if (CSID > 0)
                            {
                                dtResult = ds.Tables[8];
                            }

                            if (dtResult != null && dtResult.Rows.Count > 0)
                            {
                                txtQualityInfo = dtResult.Rows[0]["QualityAppraisal"].ToString();
                            }

                        }
                        //绑定
                        DataBound();
                    }

                }
                catch (Exception ex)
                {
                    BLL.Loger.Log4Net.Error("五级质检评分表编辑异常,part1：" + ex.StackTrace);
                    BLL.Loger.Log4Net.Error("五级质检评分表编辑异常,part2：" + ex.Message);
                    BLL.Loger.Log4Net.Error("五级质检评分表编辑异常,part3：" + ex.Source); 
                }
            }
        }
        protected void DataBound()
        {
            if (dtCategory != null && dtCategory.Rows.Count > 0)
            {
                this.rp_Category.DataSource = dtCategory;
                this.rp_Category.DataBind();
                DeadNum = dtCategory.Rows.Count + 1;

            }
            else
            {
                DeadNum = 1;
            }
            if (dtDead != null && dtDead.Rows.Count > 0)
            {
                haveDead = "";
                this.rp_Dead.DataSource = dtDead;
                this.rp_Dead.DataBind();
                QulityNum = DeadNum + 1;
            }
            else
            {
                if (dtCategory != null && dtCategory.Rows.Count > 0)
                {
                    QulityNum = dtCategory.Rows.Count + 1;
                }
                else
                {
                    QulityNum = 1;
                }
            }

            if (dtRulesTable != null && dtRulesTable.Rows.Count > 0)
            {
                if (dtRulesTable.Rows[0]["HaveQAppraisal"].ToString() == "1")
                {
                    haveQulity = "";
                }
                ScoreType = dtRulesTable.Rows[0]["ScoreType"].ToString();

                if (!string.IsNullOrEmpty(dtRulesTable.Rows[0]["DeadItemNum"].ToString()))
                {
                    DeadMIdNum = dtRulesTable.Rows[0]["DeadItemNum"].ToString();
                }
                if (!string.IsNullOrEmpty(dtRulesTable.Rows[0]["NoDeadItemNum"].ToString()))
                {
                    NoDeadMIdNum = dtRulesTable.Rows[0]["NoDeadItemNum"].ToString();
                }

                if (ScoreType == "1")
                {
                    ScoreTypeFlag = "";
                }
                else
                {
                    ScoreTypeFlag = "display:none";
                }
            }
        }
        /// <summary>
        /// 根据分类id取项目集合
        /// </summary>
        /// <returns></returns>
        protected DataTable GetItemTableByQS_CID(string QS_CID)
        {
            if (dtItem != null && dtItem.Rows.Count > 0)
            {
                #region 根据条件过滤表
                //只有结构
                DataTable newTable = dtItem.Clone();
                DataColumn col = new DataColumn("QS_SID", typeof(string));
                newTable.Columns.Add(col);

                DataRow[] drs = dtItem.Select(" QS_CID='" + QS_CID + "'");
                foreach (DataRow dr in drs)
                {
                    object[] arr = dr.ItemArray;
                    DataRow newrow = newTable.NewRow();
                    for (int i = 0; i < arr.Length + 1; i++)
                    {
                        if (i == arr.Length)
                        {
                            DataRow[] arrRowResultStandars = dtResultDetail == null ? null : dtResultDetail.Select(" QS_IID='" + arr[0].ToString() + "'");

                            if (arrRowResultStandars != null && arrRowResultStandars.Length > 0)
                            {
                                newrow[i] = arrRowResultStandars[0]["QS_SID"];
                            }
                            else
                            {
                                newrow[i] = "";
                            }
                        }
                        else
                        {
                            newrow[i] = arr[i];
                        }
                    }
                    newTable.Rows.Add(newrow);
                }
                return newTable;
                #endregion
            }
            else
            {
                return null;
            }
        }
        public string GetStandardRemarkBySID(string sid)
        {
            if (string.IsNullOrEmpty(sid))
            {
                return "";
            }
            DataRow[] arrRowResultStandars = dtResultDetail == null ? null : dtResultDetail.Select(" QS_SID='" + sid + "'");
            if (arrRowResultStandars != null && arrRowResultStandars.Length > 0)
            {
                return arrRowResultStandars[0]["Remark"].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 根据项目id取标准集合
        /// </summary>
        /// <returns></returns>
        protected DataTable GetStandardTableByQS_IID(string QS_IID)
        {
            if (dtStandard != null && dtStandard.Rows.Count > 0)
            {
                #region 根据条件过滤表
                //只有结构
                DataTable newTable = dtStandard.Clone();
                DataRow[] drs = dtStandard.Select(" QS_IID='" + QS_IID + "'");
                foreach (DataRow dr in drs)
                {
                    object[] arr = dr.ItemArray;
                    DataRow newrow = newTable.NewRow();
                    for (int i = 0; i < arr.Length; i++)
                        newrow[i] = arr[i];
                    newTable.Rows.Add(newrow);
                }
                return newTable;
                #endregion
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据标准id取扣分项集合
        /// </summary>
        /// <returns></returns>
        protected DataTable GetMarkingTableByQS_SID(string QS_SID)
        {
            if (dtMarking != null && dtMarking.Rows.Count > 0)
            {
                #region 根据条件过滤表
                //只有结构
                DataTable newTable = dtMarking.Clone();
                DataRow[] drs = dtMarking.Select(" QS_SID='" + QS_SID + "'");
                foreach (DataRow dr in drs)
                {
                    object[] arr = dr.ItemArray;
                    DataRow newrow = newTable.NewRow();
                    for (int i = 0; i < arr.Length; i++)
                        newrow[i] = arr[i];
                    newTable.Rows.Add(newrow);
                }
                return newTable;
                #endregion
            }
            else
            {
                return null;
            }
        }


        //分类Bound
        protected void rp_Category_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //取题目
                Repeater rp_Item = e.Item.FindControl("rp_Item") as Repeater;
                Label lblQS_CID = e.Item.FindControl("lblQS_CID") as Label;
                string QS_CID = lblQS_CID.Text.Trim();
                DataTable dtItemByCID = GetItemTableByQS_CID(QS_CID);
                if (dtItemByCID != null)
                {
                    rp_Item.DataSource = dtItemByCID;
                    rp_Item.DataBind();
                }
            }
        }
        //项目Bound
        protected void rp_Item_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //取标准
                Repeater rp_Standard = e.Item.FindControl("rp_Standard") as Repeater;
                Label lblQS_IID = e.Item.FindControl("lblQS_IID") as Label;
                string QS_IID = lblQS_IID.Text.Trim();
                DataTable dtStandardByIID = GetStandardTableByQS_IID(QS_IID);
                if (dtStandardByIID != null)
                {
                    rp_Standard.DataSource = dtStandardByIID;
                    rp_Standard.DataBind();
                }
            }
        }


        protected void rp_Standard_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //取标准
                Repeater rp_Marking = e.Item.FindControl("rp_Marking") as Repeater;
                Label lblQS_SID = e.Item.FindControl("lblQS_SID") as Label;

                string QS_SID = lblQS_SID.Text.Trim();
                DataTable dtMarkingBySID = GetMarkingTableByQS_SID(QS_SID);
                if (dtMarkingBySID != null)
                {
                    rp_Marking.DataSource = dtMarkingBySID;
                    rp_Marking.DataBind();
                }
            }
        }
        /// <summary>
        /// 根据扣分项判断是否是扣分,type=1取扣分项，type=2取致命项
        /// </summary>
        /// <param name="MarkID"></param>
        /// <returns></returns>
        public string GetMarkedRemeak(string MarkID, string type)
        {
            string Remark = "";
            if (dtResultDetail != null && dtResultDetail.Rows.Count > 0)
            {
                DataRow[] drs = null;
                if (type == "1")
                {
                    drs = dtResultDetail.Select(" QS_MID='" + MarkID + "'");
                }
                else
                {
                    drs = dtResultDetail.Select(" ScoreDeadID='" + MarkID + "'");
                }
                if (drs != null && drs.Length > 0)
                {
                    Remark = drs[0]["Remark"].ToString();
                }
            }
            return Remark;
        }
        /// <summary>
        /// 判断是否是复审通过
        /// </summary>
        /// <returns></returns>
        public bool IsSecondApproval()
        {
            bool flag = false;
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                if (dtResult.Rows[0]["StateResult"].ToString() == "1")
                {
                    flag = true;
                }
            }
            return flag;
        }
        /// <summary>
        /// 根据标准iD取，标准信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetQS_StandardByID(string QS_SID)
        {
            if (dtStandard != null && dtStandard.Rows.Count > 0)
            {
                #region 根据条件过滤表
                //只有结构
                DataTable newTable = dtStandard.Clone();
                DataRow[] drs = dtStandard.Select(" QS_SID='" + QS_SID + "'");
                foreach (DataRow dr in drs)
                {
                    object[] arr = dr.ItemArray;
                    DataRow newrow = newTable.NewRow();
                    for (int i = 0; i < arr.Length; i++)
                        newrow[i] = arr[i];
                    newTable.Rows.Add(newrow);
                }
                return newTable;
                #endregion
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 判断扣分项或致命项是否存在于质检明细里，type=1是扣分项，2是致命项
        /// </summary>
        /// <param name="QSID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string IsExistResultDetail(string QSID, string type)
        {
            string str = string.Empty;
            if (dtResultDetail != null && dtResultDetail.Rows.Count > 0)
            {
                DataRow[] drs = null;
                if (type == "1")
                {
                    drs = dtResultDetail.Select(" QS_MID='" + QSID + "'");
                }
                else
                {
                    drs = dtResultDetail.Select(" ScoreDeadID='" + QSID + "'");
                }
                if (drs != null && drs.Length > 0)
                {
                    str = "checked='checked'";
                }
            }
            return str;
        }
        public string GetFiveLevelStandardName(string val)
        {
            int enumval;
            if (int.TryParse(val, out enumval))
            {
                return BLL.Util.GetEnumOptText(typeof(StandardFiveLevel), enumval);
            }
            else
            {
                return "";
            }
        }
    }
}