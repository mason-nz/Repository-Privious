using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.Utils.Config;
using System.Reflection;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard
{
    public partial class Dispose : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 参数
        public string RequestQS_RID
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("QS_RID")); }
        }
        public string tableEndName
        {
            get { return HttpUtility.UrlDecode(BLL.Util.GetCurrentRequestStr("tableEndName")); }
        }
        public string Status;
        public string Title;
        //控制初审按钮
        public bool FirstTrialButton = false;
        public bool RecheckButton = false;
        public bool AppealButton = false;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userId, "SYS024BUT600101") || BLL.Util.CheckRight(userId, "SYS024BUT600102") || BLL.Util.CheckRight(userId, "SYS024BUT600103"))
                {
                    VerifyData();
                }
                else
                {
                     Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End(); 
                }
            }
        }
        public string TableName;
        private void VerifyData()
        {
            string sysId = ConfigurationUtil.GetAppSettingValue("ThisSysID");
            int userId = BLL.Util.GetLoginUserID();
            FirstTrialButton = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.CheckRight("SYS024BUT600102", sysId, userId);
            RecheckButton = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.CheckRight("SYS024BUT600103", sysId, userId);
            AppealButton = BitAuto.YanFa.SysRightManager.Common.UserInfo.Instance.CheckRight("SYS024BUT600106", sysId, userId);

            int rid = 0;
            if (int.TryParse(RequestQS_RID, out rid))
            {
                Entities.QS_Result info = BLL.QS_Result.Instance.GetQS_Result(rid);
                if (info != null)
                {
                    //由于易湃外呼业务签入cc系统，易湃不入库CallRecordInfo表，所以此处改成从CallRecord_ORIG_Business取录音创建坐席 modify by qizq 2014-4-17
                    Entities.CallRecord_ORIG_Business callRecordInfo = BLL.CallRecord_ORIG_Business.Instance.GetByCallID(Convert.ToInt64(info.CallID), tableEndName);
                    //Entities.CallRecordInfo callRecordInfo = BLL.CallRecordInfo.Instance.GetCallRecordInfoByCallID(info.CallID);
                    if (callRecordInfo != null)
                    {
                        //判断单据状态，如果不是待复审，不能复审
                        Entities.QS_RulesTable QS_RulesTableModel = BLL.QS_RulesTable.Instance.GetQS_RulesTable(info.QS_RTID);
                        if (QS_RulesTableModel != null)
                        {
                            TableName = QS_RulesTableModel.Name;
                            if (QS_RulesTableModel.ScoreType == 2)
                            {
                                TableName += "（致命项数" + QS_RulesTableModel.DeadItemNum + ",非致命项数" + QS_RulesTableModel.NoDeadItemNum + "）";
                            }
                        }

                        //如果具有申诉权限，并且当前人不是此录音的创建人员，关闭此页面
                        if (AppealButton && callRecordInfo.CreateUserID != BLL.Util.GetLoginUserID() & !RecheckButton & !AppealButton)
                        {
                            Response.Write(@"<script language='javascript'>alert('您没有权限查看此评分！');try {
                  window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                        }
                        //如果不是本人录音，申诉按钮不显示
                        if (AppealButton && callRecordInfo.CreateUserID != BLL.Util.GetLoginUserID())
                        {
                            AppealButton = false;
                        }
                        this.ucCallRecordView.QS_RID = info.QS_RID;
                        this.ucCallRecordView.CallID = info.CallID;
                        this.ucCallRecordView.tableEndName = tableEndName;
                    }
                    Status = info.Status.ToString();
                    switch (Status)
                    {
                        case "20003":
                            Title = "录音质检成绩初审";
                            this.UCScoreTableView.QS_RID = info.QS_RID;
                            this.UCScoreTableView.QS_RTID = info.QS_RTID;
                            break;
                        case "20004":
                            Title = "录音质检成绩复审";
                            this.ucTableDispose.CallID = info.CallID;
                            this.ucTableDispose.QS_RID = info.QS_RID;
                            this.ucTableDispose.QS_RTID = info.QS_RTID;
                            break;
                        default:
                            Title = "录音质检成绩查看";
                            this.UCScoreTableView.QS_RID = info.QS_RID;
                            this.UCScoreTableView.QS_RTID = info.QS_RTID;
                            break;
                    }
                }
                else
                {
                    Response.Write(@"<script language='javascript'>alert('不存在此评分！');try {
                  window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self',''); window.close();
            };</script>");
                }
            }
            else
            {
                Response.Write(@"<script language='javascript'>alert('参数有误！');try {
                  window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self',''); window.close();
            };</script>");
            }
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
    }
}