using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.UCQualityStandard
{
    public partial class UCCallRecordView : System.Web.UI.UserControl
    {
        public string YPFanXianHBuyCarURL = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBugCar_URL"].ToString();//惠买车URL
        public string EPEmbedCCHBuyCar_APPID = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCCHBuyCar_APPID"];//易湃签入CC页面，惠买车APPID
        /// <summary>
        /// 录音文件路径
        /// </summary>
        public string FileUrl = "";

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
        //有处理页面，编辑页面，查看页面传值
        public string tableEndName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }
        private Random R = new Random();
        private void BindData()
        {
            #region 成绩结果
            if (QS_RID != 0)
            {
                Entities.QS_Result model = BLL.QS_Result.Instance.GetQS_Result(QS_RID);
                if (model != null)
                {
                    if (model.ScoreType == 1 || model.ScoreType == 3)
                    {
                        tdScore.InnerText = model.Score.ToString().Replace(".0", "") + "分";
                    }
                    else
                    {
                        tdScore.InnerText = model.IsQualified == 1 ? "合格" : "不合格";
                    }

                    if (model.Status != (int)QSResultStatus.WaitScore)
                    {
                        this.tdScore.Visible = true;
                    }
                }
            }


            #endregion

            #region 录音
            Entities.CallRecord_ORIG origM = BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGByCallID(CallID, tableEndName);
            Entities.CallRecord_ORIG_Business businessM = BLL.CallRecord_ORIG_Business.Instance.GetByCallID(CallID, tableEndName);
            if (origM != null && businessM != null)
            {
                this.spCallID.InnerText = origM.CallID.ToString();
                this.spLiuShui.InnerHtml = string.Format("<a href='/KnowledgeLib/Personalization/DownLoadFilePage.aspx?theAction=1&theUrl={1}'>{0}</a>", origM.SessionID, HttpUtility.UrlEncode(origM.AudioURL));
                this.spUserName.InnerText = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName((int)businessM.CreateUserID);
                this.spBeginTime.InnerText = origM.EstablishedTime.ToString();
                this.spTimeLong.InnerText = origM.TallTime.ToString();
                this.spCallType.InnerText = BitAuto.ISDC.CC2012.BLL.Util.GetCallStatus(origM.CallStatus.Value.ToString());
                FileUrl = origM.AudioURL;

                this.spTaskID.InnerText = businessM.BusinessID;

                //分类名称
                Entities.SurveyCategory ScMode = BLL.SurveyCategory.Instance.GetSurveyCategory((int)businessM.SCID);
                if (ScMode != null)
                {
                    this.spSCName.InnerText = ScMode.Name;
                }
                //链接
                string url = BLL.CallRecord_ORIG_Business.Instance.GetTaskUrl(businessM.BusinessID, businessM.BGID.ToString(), businessM.SCID.ToString());
                url = BLL.CallRecord_ORIG_Business.Instance.GetViewUrl(businessM.BusinessID, url);
                this.spTaskID.InnerHtml = BLL.Util.GenBusinessURLByBGIDAndSCID(businessM.BGID.ToString(), businessM.SCID.ToString(), url, businessM.BusinessID, YPFanXianHBuyCarURL, EPEmbedCCHBuyCar_APPID);
            }

            //满意度加载
            int ivrscore = BLL.IVRSatisfaction.Instance.GetIVRScoreBYCallID(CallID, tableEndName);
            string strScore = "";
            switch (ivrscore)
            {
                case 0:
                    strScore = "未评价";
                    break;
                case 1:
                    strScore = "满意";
                    break;
                case 2:
                    strScore = "对问题处理结果不满意";
                    break;
                case 3:
                    strScore = "对客服代表服务不满意";
                    break;
                case 10:
                    strScore = "已解决,未评价";
                    break;
                case 11:
                    strScore = "已解决,满意";
                    break;
                case 12:
                    strScore = "已解决,对处理结果不满意";
                    break;
                case 13:
                    strScore = "已解决,对客服代表服务不满意";
                    break;
                case 20:
                    strScore = "未解决,未评价";
                    break;
                case 21:
                    strScore = "未解决,满意";
                    break;
                case 22:
                    strScore = "未解决,对处理结果不满意";
                    break;
                case 23:
                    strScore = "未解决,对客服代表服务不满意";
                    break;
                default:
                    break;
            }
            spIVRScore.InnerText = strScore;
            #endregion

        }
    }
}