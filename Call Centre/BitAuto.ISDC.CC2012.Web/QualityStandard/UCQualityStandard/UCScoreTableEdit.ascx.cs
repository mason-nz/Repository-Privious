using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard
{
    public partial class UCScoreTableEdit : System.Web.UI.UserControl
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
        //评分表主键
        private int _qs_rtid;
        public int Qs_RTID
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
            string str = string.Empty;
            switch (num)
            {
                case 1:
                    if (category == "1")
                    {
                        str = "一、";
                    }
                    else if (category == "2")
                    {
                        str = "1、";
                    }
                    else
                    {
                        str = "(1)";
                    }
                    break;
                case 2:
                    if (category == "1")
                    {
                        str = "二、";
                    }
                    else if (category == "2")
                    {
                        str = "2、";
                    }
                    else
                    {
                        str = "(2)";
                    }
                    break;
                case 3:
                    if (category == "1")
                    {
                        str = "三、";
                    }
                    else if (category == "2")
                    {
                        str = "3、";
                    }
                    else
                    {
                        str = "(3)";
                    }
                    break;
                case 4:
                    if (category == "1")
                    {
                        str = "四、";
                    }
                    else if (category == "2")
                    {
                        str = "4、";
                    }
                    else
                    {
                        str = "(4)";
                    }
                    break;
                case 5:
                    if (category == "1")
                    {
                        str = "五、";
                    }
                    else if (category == "2")
                    {
                        str = "5、";
                    }
                    else
                    {
                        str = "(5)";
                    }
                    break;
                case 6:
                    if (category == "1")
                    {
                        str = "六、";
                    }
                    else if (category == "2")
                    {
                        str = "6、";
                    }
                    else
                    {
                        str = "(6)";
                    }
                    break;
                case 7:
                    if (category == "1")
                    {
                        str = "七、";
                    }
                    else if (category == "2")
                    {
                        str = "7、";
                    }
                    else
                    {
                        str = "(7)";
                    }
                    break;
                case 8:
                    if (category == "1")
                    {
                        str = "八、";
                    }
                    else if (category == "2")
                    {
                        str = "8、";
                    }
                    else
                    {
                        str = "(8)";
                    }
                    break;
                case 9:
                    if (category == "1")
                    {
                        str = "九、";
                    }
                    else if (category == "2")
                    {
                        str = "9、";
                    }
                    else
                    {
                        str = "(9)";
                    }
                    break;
                case 10:
                    if (category == "1")
                    {
                        str = "十、";
                    }
                    else if (category == "2")
                    {
                        str = "10、";
                    }
                    else
                    {
                        str = "(10)";
                    }
                    break;
                case 11:
                    if (category == "1")
                    {
                        str = "十一、";
                    }
                    else if (category == "2")
                    {
                        str = "11、";
                    }
                    else
                    {
                        str = "(11)";
                    }
                    break;
                case 12:
                    if (category == "1")
                    {
                        str = "十二、";
                    }
                    else if (category == "2")
                    {
                        str = "12、";
                    }
                    else
                    {
                        str = "(12)";
                    }
                    break;
                case 13:
                    if (category == "1")
                    {
                        str = "十三、";
                    }
                    else if (category == "2")
                    {
                        str = "13、";
                    }
                    else
                    {
                        str = "(13)";
                    }
                    break;
                case 14:
                    if (category == "1")
                    {
                        str = "十四、";
                    }
                    else if (category == "2")
                    {
                        str = "14、";
                    }
                    else
                    {
                        str = "(14)";
                    }
                    break;
                case 15:
                    if (category == "1")
                    {
                        str = "十五、";
                    }
                    else if (category == "2")
                    {
                        str = "15、";
                    }
                    else
                    {
                        str = "(15)";
                    }
                    break;
                case 16:
                    if (category == "1")
                    {
                        str = "十六、";
                    }
                    else if (category == "2")
                    {
                        str = "16、";
                    }
                    else
                    {
                        str = "(16)";
                    }
                    break;
                case 17:
                    if (category == "1")
                    {
                        str = "十七、";
                    }
                    else if (category == "2")
                    {
                        str = "17、";
                    }
                    else
                    {
                        str = "(17)";
                    }
                    break;
                case 18:
                    if (category == "1")
                    {
                        str = "十八、";
                    }
                    else if (category == "2")
                    {
                        str = "18、";
                    }
                    else
                    {
                        str = "(18)";
                    }
                    break;
                case 19:
                    if (category == "1")
                    {
                        str = "十九、";
                    }
                    else if (category == "2")
                    {
                        str = "19、";
                    }
                    else
                    {
                        str = "(19)";
                    }
                    break;
                case 20:
                    if (category == "1")
                    {
                        str = "二十、";
                    }
                    else if (category == "2")
                    {
                        str = "20、";
                    }
                    else
                    {
                        str = "(20)";
                    }
                    break;
                default:
                    break;
            }
            return "";
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataSet ds = null;
                ds = BLL.QS_RulesTable.Instance.GetRulesTableDetailByQS_RTID(Qs_RTID);
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
        protected System.Data.DataTable GetMarkingTableByQS_SID(string QS_SID)
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
                System.Data.DataTable dtMarkingBySID = GetMarkingTableByQS_SID(QS_SID);
                if (dtMarkingBySID != null)
                {
                    rp_Marking.DataSource = dtMarkingBySID;
                    rp_Marking.DataBind();
                }
            }
        }
    }
}