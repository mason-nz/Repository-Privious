using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.OtherTask
{
    public partial class OtherTaskDeal : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public bool IsShowSubmitOrder = false;
        private string _otherTaskID;
        /// <summary>
        /// 其他任务ID
        /// </summary>
        public string OtherTaskID
        {
            get
            {
                if (string.IsNullOrEmpty(_otherTaskID))
                {
                    _otherTaskID = HttpUtility.UrlDecode((Request["OtherTaskID"] + "").Trim());
                }
                return _otherTaskID;
            }
        }

        public string IsAutoCall
        {
            get
            {
                return HttpUtility.UrlDecode((BLL.Util.GetCurrentRequestStr("isautocall") + ""));
            }
        }
        public string AutoCallData
        {
            get
            {
                return HttpUtility.UrlDecode((BLL.Util.GetCurrentRequestStr("autocalldata") + ""));
            }
        }

        public string UserID = "";
        public string BGID = "";
        public string SCID = "";
        public string CustName = "";
        public string TaskTypeID = "";
        public string BlackWhiteList = "-1";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(OtherTaskID))
                {
                    Entities.OtherTaskInfo model = BLL.OtherTaskInfo.Instance.GetOtherTaskInfo(OtherTaskID);
                    BlackWhiteList = Convert.ToInt16(Entities.NotEstablishReason.N05_免打扰屏蔽).ToString();
                    if (model != null)
                    {
                        UserID = BLL.Util.GetLoginUserID().ToString();
                        
                        if (IsAutoCall == "true")
                        {
                            //重新分配坐席
                            BLL.ProjectTask_Employee.Instance.DeleteByIDs("'" + OtherTaskID + "'");
                            //新建坐席数据
                            Entities.ProjectTask_Employee info = new Entities.ProjectTask_Employee();
                            info.PTID = OtherTaskID;
                            info.UserID = int.Parse(UserID);
                            info.Status = 0;
                            info.CreateTime = DateTime.Now;
                            info.CreateUserID = int.Parse(UserID);
                            BLL.ProjectTask_Employee.Instance.Add(info);
                            //修改任务状态=处理中
                            BLL.OtherTaskInfo.Instance.UpdateTaskStatus(info.PTID, Entities.OtheTaskStatus.Processing, Entities.EnumProjectTaskOperationStatus.TaskAllot, "分配", info.UserID.Value);
                            //修改实体类中属性
                            model.TaskStatus = (Int32)Entities.OtheTaskStatus.Processing;
                        }

                        if (model.TaskStatus == (Int32)Entities.OtheTaskStatus.Processed)
                        {
                            Response.Write(@"<script language='javascript'>javascript:alert('该任务已处理完毕！');try {
                                                                 window.external.MethodScript('/browsercontrol/closepage');
                                                            } catch (e) {
                                                                window.opener = null; window.open('', '_self'); window.close();
                                                            };</script>");

                        }
                        else if (model.TaskStatus == (Int32)Entities.OtheTaskStatus.StopTask)
                        {
                            Response.Write(@"<script language='javascript'>javascript:alert('该任务已结束！');try {
                                                                 window.external.MethodScript('/browsercontrol/closepage');
                                                            } catch (e) {
                                                                window.opener = null; window.open('', '_self'); window.close();
                                                            };</script>");
                        }
                        else if (model.TaskStatus == (Int32)Entities.OtheTaskStatus.Unallocated)
                        {
                            Response.Write(@"<script language='javascript'>javascript:alert('该任务未分配处理人！');try {
                                                                 window.external.MethodScript('/browsercontrol/closepage');
                                                            } catch (e) {
                                                                window.opener = null; window.open('', '_self'); window.close();
                                                            };</script>");
                        }
                        else
                        {
                            DataTable dtEmployee = BLL.ProjectTask_Employee.Instance.GetProjectTask_Employee(OtherTaskID);
                            if (dtEmployee != null && dtEmployee.Rows.Count > 0)
                            {
                                if (BLL.Util.GetLoginUserID().ToString() == dtEmployee.Rows[0]["userid"].ToString())
                                {
                                    OtherTaskEdit1.RequestTaskID = OtherTaskID;
                                }
                                else
                                {
                                    Response.Write(@"<script language='javascript'>javascript:alert('您没有该任务的处理权限！');try {
                                                                     window.external.MethodScript('/browsercontrol/closepage');
                                                                } catch (e) {
                                                                    window.opener = null; window.open('', '_self'); window.close();
                                                                };</script>");
                                }
                            }
                            else
                            {
                                Response.Write(@"<script language='javascript'>javascript:alert('该任务未分配处理人！');try {
                                                                     window.external.MethodScript('/browsercontrol/closepage');
                                                                } catch (e) {
                                                                    window.opener = null; window.open('', '_self'); window.close();
                                                                };</script>");
                            }
                        }


                        Entities.ProjectInfo projectModel = BLL.ProjectInfo.Instance.GetProjectInfo((long)model.ProjectID);
                        if (projectModel != null)
                        {
                            BGID = projectModel.BGID.ToString();
                            SCID = projectModel.PCatageID.ToString();
                            Entities.TPage pageModel = BLL.TPage.Instance.GetTPageByTTCode(projectModel.TTCode);
                            if (pageModel.IsShowSubmitOrder.ToString() == "1")
                            {
                                IsShowSubmitOrder = true;
                            }
                        }
                    }
                    else
                    {
                        Response.Write(@"<script language='javascript'>javascript:alert('该任务不存在。');try {
                                                         window.external.MethodScript('/browsercontrol/closepage');
                                                    } catch (e) {
                                                        window.opener = null; window.open('', '_self'); window.close();
                                                    };</script>");

                    }
                }
            }
        }
    }
}