using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard
{
    public partial class IMDisposeFiveLevel : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// 成绩表ID
        /// <summary>
        /// 成绩表ID
        /// </summary>
        public string QS_RID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("QS_RID")); }
        }

        public string Status;
        public string Title;
        public string TableName;
        //控制初审按钮
        public bool FirstTrialButton = false;
        public bool RecheckButton = false;
        public bool AppealButton = false;

        public bool CanSeeMessage = false;
        public int CSID;
        public int userId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userId = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userId, "SYS024BUT600402") || BLL.Util.CheckRight(userId, "SYS024BUT600403") || BLL.Util.CheckRight(userId, "SYS024BUT600404"))
                {
                    VerifyData();
                    BindData();
                }
                else
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
            }
        }

        private void VerifyData()
        {
            string sysId = ConfigurationUtil.GetAppSettingValue("ThisSysID");
            int userId = BLL.Util.GetLoginUserID();
            //初审
            FirstTrialButton = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.CheckRight("SYS024BUT600403", sysId, userId);
            //复审
            RecheckButton = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.CheckRight("SYS024BUT600404", sysId, userId);
            //申诉
            AppealButton = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.CheckRight("SYS024BUT600406", sysId, userId);
        }

        private void BindData()
        {
            if (!string.IsNullOrEmpty(QS_RID))
            {
                QS_IM_ResultInfo info = CommonBll.Instance.GetComAdoInfo<QS_IM_ResultInfo>(CommonFunction.ObjectToInteger(QS_RID));
                if (info != null)
                {
                    if (info.Status == 20004)
                    {
                        CanSeeMessage = true;
                    }
                    int QS_RTID = info.ValueOrDefault_QS_RTID;
                    CSID = (int)info.ValueOrDefault_CSID;

                    DataRow dr = BLL.QS_IM_Result.Instance.GetQS_IM_ResultForCSID(CSID.ToString());
                    Entities.QS_RulesTable tablemodel = BLL.QS_RulesTable.Instance.GetQS_RulesTable(QS_RTID);
                    if (tablemodel != null && dr != null)
                    {
                        //设置标题
                        TableName = tablemodel.Name;
                        if (tablemodel.ScoreType == 2)
                        {
                            TableName += "（致命项数" + tablemodel.DeadItemNum + "，非致命项数" + tablemodel.NoDeadItemNum + "）";
                        }

                        //如果具有申诉权限，并且当前人不是此对话的创建人员，关闭此页面
                        if (AppealButton && CommonFunction.ObjectToInteger(dr["AgentUserID"]) != BLL.Util.GetLoginUserID() & !RecheckButton & !AppealButton)
                        {
                            AlertMessage("您没有权限查看此评分");
                        }
                        //如果不是本人录音，申诉按钮不显示
                        if (AppealButton && CommonFunction.ObjectToInteger(dr["AgentUserID"]) != BLL.Util.GetLoginUserID())
                        {
                            AppealButton = false;
                        }
                        //显示对话
                        this.UCConversationsViewID.CSID = CSID.ToString();
                    }
                    //成绩表状态
                    Status = info.ValueOrDefault_Status.ToString();

                    switch (Status)
                    {
                        case "20003":
                            Title = "对话质检成绩初审";
                            this.UCScoreTableView.QS_RID = CommonFunction.ObjectToInteger(QS_RID);
                            this.UCScoreTableView.QS_RTID = QS_RTID;
                            this.UCScoreTableView.PageFrom = "im";
                            break;
                        case "20004":
                            Title = "对话质检成绩复审";
                            this.ucTableDispose.CallID = -1;
                            this.ucTableDispose.CSID = CSID;
                            this.ucTableDispose.QS_RID = CommonFunction.ObjectToInteger(QS_RID);
                            this.ucTableDispose.QS_RTID = QS_RTID;
                            break;
                        default:
                            Title = "对话质检成绩查看";
                            this.UCScoreTableView.QS_RID = CommonFunction.ObjectToInteger(QS_RID);
                            this.UCScoreTableView.QS_RTID = QS_RTID;
                            this.UCScoreTableView.PageFrom = "cc";
                            break;
                    }
                }
                else
                {
                    AlertMessage("评分成绩不存在");
                }
            }
            else
            {
                AlertMessage("请求参数错误");
            }
        }

        private void AlertMessage(string msg)
        {
            Response.Write(@"<script language='javascript'>javascript:alert('" + msg + "！');try {window.external.MethodScript('/browsercontrol/closepage');} catch (e) {window.opener = null; window.open('', '_self'); window.close();};</script>");
        }

        /// <summary>
        /// 重写LoadControl，带参数。
        /// </summary>
        private UserControl LoadControl(string UserControlPath, params object[] constructorParameters)
        {
            List<Type> constParamTypes = new List<Type>();
            foreach (object constParam in constructorParameters)
            {
                constParamTypes.Add(constParam.GetType());
            }

            UserControl ctl = Page.LoadControl(UserControlPath) as UserControl;

            // Find the relevant constructor
            ConstructorInfo constructor = ctl.GetType().BaseType.GetConstructor(constParamTypes.ToArray());

            //And then call the relevant constructor
            if (constructor == null)
            {
                throw new MemberAccessException("The requested constructor was not found on : " + ctl.GetType().BaseType.ToString());
            }
            else
            {
                constructor.Invoke(ctl, constructorParameters);
            }

            // Finally return the fully initialized UC
            return ctl;
        }

        public string GetIMUrl()
        {
            return ConfigurationUtil.GetAppSettingValue("PersonalIMURL") + "/ConversationHistoryForCCDetail.aspx";
        }
    }
}