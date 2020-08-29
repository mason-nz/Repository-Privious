using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard
{
    public partial class UCQualityStandardView : System.Web.UI.UserControl
    {
        //是否有致命项
        public string haveDead = "display:none";
        //是否有质检评价
        public string haveQulity = "display:none";

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

        public string ScoreType = "1";
        public string ScoreTypeFlag = "";
        /// <summary>
        /// category 1是大标题，2是二级标题，3是三级标题
        /// </summary>
        /// <param name="num"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public string GetNum(int num, string category)
        {
            return BLL.Util.GetNum(num, category);
        }
        #endregion
        //质检对象类型：cc*（录音）/im（对话）
        private string _qs_pagefrom = "";
        public string PageFrom
        {
            set
            {
                _qs_pagefrom = value;
            }
            get
            {
                return _qs_pagefrom;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //如果Qs_RTID是0说明没有质检表，QS_RID是0说明没有质检过
                //如果两个都有说明是带答题成绩的预览

                DataSet ds = null;
                //如果只有Qs_RTID没有QS_RID，说明是预览
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
                //质检答题情况查看
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
                        //dtResult = ds.Tables[7];
                        if (PageFrom.ToLower().Trim() == "cc")
                        {
                            dtResult = ds.Tables[7];
                        }
                        else if (PageFrom.ToLower().Trim() == "im")
                        {
                            dtResult = ds.Tables[8];
                        } 
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            txtQualityInfo.Text = dtResult.Rows[0]["QualityAppraisal"].ToString();
                        }

                    }
                    //绑定
                    DataBound();
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
                DataRow[] drs = dtItem.Select(" QS_CID='" + QS_CID + "'");
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
        /// 根据扣分项判断是否是扣分,type=1取初始评分字段，type=2取最终字段
        /// </summary>
        /// <param name="MarkID"></param>
        /// <returns></returns>
        public bool IsMarked(string MarkID, out string Remark, string type)
        {
            bool flag = false;
            Remark = "";
            if (dtResultDetail != null && dtResultDetail.Rows.Count > 0)
            {
                DataRow[] drs = null;
                if (type == "1")
                {
                    drs = dtResultDetail.Select(" QS_MID='" + MarkID + "'");
                }
                else
                {
                    drs = dtResultDetail.Select(" QS_MID_End='" + MarkID + "'");
                }
                if (drs != null && drs.Length > 0)
                {
                    flag = true;
                    Remark = drs[0]["Remark"].ToString();
                }
            }
            return flag;
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
                if (dtResult.Rows[0]["StateResult"].ToString() == "1" && dtResult.Rows[0]["Status"].ToString() == "20006")
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
                    str = "tssm";
                }
            }


            return str;
        }


        /// <summary>
        /// 根据致命项判断是否是致命,type=1取初始评分字段，type=2取最终字段
        /// </summary>
        /// <param name="MarkID"></param>
        /// <returns></returns>
        public bool IsDeaded(string DeadID, out string Remark, string type)
        {
            bool flag = false;
            Remark = "";
            if (dtResultDetail != null && dtResultDetail.Rows.Count > 0)
            {
                DataRow[] drs = null;
                if (type == "1")
                {
                    drs = dtResultDetail.Select(" ScoreDeadID='" + DeadID + "'");
                }
                else
                {
                    drs = dtResultDetail.Select(" ScoreDeadID_End='" + DeadID + "'");
                }
                if (drs != null && drs.Length > 0)
                {
                    flag = true;
                    Remark = drs[0]["Remark"].ToString();
                }
            }
            return flag;
        }

        protected void rp_Dead_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //取扣分项ID
                Label lblDeadID = (Label)e.Item.FindControl("lblDeadID");
                //取扣分分数
                Label lblDeadInfo = (Label)e.Item.FindControl("lblDeadInfo");
                Label lblDeadInfo_End = (Label)e.Item.FindControl("lblDeadInfo_End");
                //扣分备注
                Label lblDeadRemark = (Label)e.Item.FindControl("lblDeadRemark");



                if (!string.IsNullOrEmpty(lblDeadID.Text.Trim()))
                {
                    //被扣分
                    string Remark = string.Empty;
                    if (IsDeaded(lblDeadID.Text.Trim(), out Remark, "1"))
                    {
                        lblDeadInfo.Visible = true;
                        lblDeadRemark.Visible = true;
                        lblDeadRemark.Text = Remark;

                    }
                    //复审通过
                    if (IsSecondApproval())
                    {
                        lblDeadInfo_End.Visible = true;

                        //判断是否依然扣分，或去掉扣分
                        if (IsMarked(lblDeadID.Text.Trim(), out Remark, "2"))
                        {
                            lblDeadInfo_End.Visible = true;
                            lblDeadInfo_End.Text = "致命";
                        }

                    }
                }
            }
        }
        //是否申诉成功
        public string StatsSuccess = "display:none";
        protected void rp_Marking_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //取扣分项ID
                Label lblMarkID = e.Item.FindControl("lblMarkID") as Label;
                //取扣分分数
                Label lblMarkingScore = e.Item.FindControl("lblMarkingScore") as Label;
                //扣分备注
                Label lblMarkingRemark = e.Item.FindControl("lblMarkingRemark") as Label;
                //标准ID
                Label lblStandarID = e.Item.FindControl("lblStandarID") as Label;
                //复审通过控件
                Label lblMarkingScore_End = e.Item.FindControl("lblMarkingScore_End") as Label;
                //判断是合格型还是评分型
                Label lblScoreType = e.Item.FindControl("lblScoreType") as Label;

                //根据标准取标准信息
                DataTable dt = GetQS_StandardByID(lblStandarID.Text);
                //是否致命
                string IsDead = string.Empty;
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["IsIsDead"].ToString() == "1")
                    {
                        IsDead = "致命";
                    }
                    else if (dt.Rows[0]["IsIsDead"].ToString() == "0")
                    {
                        IsDead = "非致命";
                    }

                }


                if (!string.IsNullOrEmpty(lblMarkID.Text.Trim()))
                {
                    //被扣分
                    string Remark = string.Empty;
                    if (IsMarked(lblMarkID.Text.Trim(), out Remark, "1"))
                    {
                        lblMarkingScore.Visible = true;
                        lblMarkingRemark.Visible = true;
                        lblMarkingRemark.Text = Remark;
                        //如果是合格型的
                        if (lblScoreType.Text == "2")
                        {
                            lblMarkingScore.Text = IsDead;
                        }
                    }
                    //复审通过
                    if (IsSecondApproval())
                    {
                        StatsSuccess = "";
                        lblMarkingScore_End.Visible = true;

                        //判断是否依然扣分，或去掉扣分
                        if (IsMarked(lblMarkID.Text.Trim(), out Remark, "2"))
                        {
                            lblMarkingScore_End.Visible = true;
                        }
                        else
                        {
                            lblMarkingScore_End.Visible = false;
                        }
                        if (lblScoreType.Text == "2")
                        {
                            lblMarkingScore_End.Text = IsDead;
                        }
                    }
                    else
                    {
                        StatsSuccess = "display:none";
                        lblMarkingScore_End.Visible = false;
                    }

                }
            }
        }
    }
}