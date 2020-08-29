using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using BitAuto.ISDC.CC2012.Entities;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Web.ProjectManage.PopPanel
{
    /// <summary>
    /// AutoCallProjectManager 的摘要说明
    /// </summary>
    public class AutoCallProjectManager : IHttpHandler, IRequiresSessionState
    {

        #region 属性


        public string Action
        {
            get { return BLL.Util.GetCurrentRequestFormStr("action"); }
        }

        public string Projectids
        {
            get { return BLL.Util.GetCurrentRequestFormStr("projectids"); }
        }
        public string Cdid
        {
            get { return BLL.Util.GetCurrentRequestFormStr("cdid"); }
        }

        public string SkillGid
        {
            get { return BLL.Util.GetCurrentRequestFormStr("skid"); }
        }

        public string ProjectId
        {
            get { return BLL.Util.GetCurrentRequestFormStr("pid"); }
        }

        public string AcStatus
        {
            get { return BLL.Util.GetCurrentRequestFormStr("acstatus"); }
        }
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();

            string strResult = string.Empty;
            switch (Action)
            {
                case "AddAutoCallProjcts":
                    AddAutoCallProjcts(out strResult);
                    break;
                case "editP":
                    UpdateAutoCallProject(out strResult);
                    break;
                case "startP":
                    StartAutoProject(out strResult);
                    break;
                case "pauseP":
                    PauseProjects(out strResult);
                    break;
                case "endP":
                    EndProjects(out strResult);
                    break;
                default:
                    break; ;
            }
            //context.Response.ContentType = "application/json";
            context.Response.Write(strResult);
        }

        private void AddAutoCallProjcts(out string msg)
        {
            if (CheckParameters(out msg)) return;

            int nUserId = -1;

            try
            {
                nUserId = BLL.Util.GetLoginUserID();
                BLL.AutoCall_ProjectInfo.Instance.InsertAutoProBatch(Projectids, SkillGid, Cdid, nUserId, out msg);
                int nProjectid = 0;

                foreach (string s in Projectids.Split(','))
                {
                    if (!string.IsNullOrEmpty(s) && int.TryParse(s, out nProjectid))
                    {
                        BLL.ProjectLog.Instance.InsertProjectLog(nProjectid, ProjectLogOper.L1_新建项目, "批量生成自动外呼项目: " + s);
                    }
                }

            }
            catch (Exception ex)
            {
                msg = "{\"result\":\"-1\",\"msg\":\"" + ex.Message + "\"}";
                return;
            }

            if (string.IsNullOrEmpty(msg))
            {
                msg = "{\"result\":\"0\",\"msg\":\"\"}";
            }
            else
            {
                msg = "{\"result\":\"0\",\"msg\":\"" + msg + "\"}";
            }

        }


        private void UpdateAutoCallProject(out string msg)
        {
            if (CheckParameters(out msg)) return;


            try
            {
                BLL.AutoCall_ProjectInfo.Instance.Update(Projectids, SkillGid, Cdid);
                var nProjectid = -1;
                if (int.TryParse(Projectids, out nProjectid))
                {
                    BLL.ProjectLog.Instance.InsertProjectLog(nProjectid, ProjectLogOper.L2_编辑项目, "编辑项目:" + Projectids);
                }
            }
            catch (Exception ex)
            {
                msg = "{\"result\":\"-1\",\"msg\":\"" + ex.Message + "\"}";
                return;
            }

            if (string.IsNullOrEmpty(msg))
            {
                msg = "{\"result\":\"0\",\"msg\":\"\"}";
            }
            else
            {
                msg = "{\"result\":\"0\",\"msg\":\"" + msg + "\"}";
            }
        }

        private void StartAutoProject(out string msg)
        {
            msg = string.Empty;

            if (string.IsNullOrEmpty(Projectids))
            {
                msg = "{\"result\":\"-1\",\"msg\":\"项目ID不能为空\"}";
                return;
            }
            msg = string.Empty;


            try
            {
                BLL.AutoCall_ProjectInfo.Instance.UpdateAutoProjectStatus(Projectids, ProjectACStatus.P01_进行中);
                var nProjectid = -1;
                if (int.TryParse(Projectids, out nProjectid))
                {
                    BLL.ProjectLog.Instance.InsertProjectLog(nProjectid, ProjectLogOper.L7_启动项目, (string.IsNullOrEmpty(AcStatus) ? "未开始 => 进行中" : "暂停 => 进行中") + "项目：" + Projectids);
                }
                ThreadPool.QueueUserWorkItem(obj =>
                {
                    try
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        BLL.Loger.Log4Net.Info("异步-根据项目导入任务到自动外呼任务表中" + obj.ToString());
                        var sMsg = BLL.AutoCall_ProjectInfo.Instance.InportAutoCallTask(obj.ToString());
                        BLL.ProjectLog.Instance.InsertProjectLog(nProjectid, ProjectLogOper.L3_导入数据, string.Format("开始导入项目【{0}】 下所有外呼任务.", obj.ToString()));
                        sw.Stop();
                        BLL.Loger.Log4Net.Info("异步-根据项目导入任务到自动外呼任务表中 耗时（ms）：" + sw.ElapsedMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        BLL.Loger.Log4Net.Error("异步-根据项目导入任务到自动外呼任务表中" + obj.ToString(), ex);
                    }
                }, Projectids);
            }
            catch (Exception ex)
            {
                msg = "{\"result\":\"-1\",\"msg\":\"" + ex.Message + "\"}";
                return;
            }

            if (string.IsNullOrEmpty(msg))
            {
                msg = "{\"result\":\"0\",\"msg\":\"\"}";
            }
            else
            {
                msg = "{\"result\":\"0\",\"msg\":\"" + msg + "\"}";
            }
        }

        private void PauseProjects(out string msg)
        {
            msg = string.Empty;

            if (string.IsNullOrEmpty(Projectids))
            {
                msg = "{\"result\":\"-1\",\"msg\":\"项目ID不能为空\"}";
                return;
            }
            msg = string.Empty;


            try
            {
                var nProjectid = -1;
                if (int.TryParse(Projectids, out nProjectid))
                {
                    BLL.ProjectLog.Instance.InsertProjectLog(nProjectid, ProjectLogOper.Z4_暂停自动外呼, string.Format("暂停自动外呼项目:{0}", Projectids));
                }
                BLL.AutoCall_ProjectInfo.Instance.UpdateAutoProjectStatus(Projectids, ProjectACStatus.P02_暂停中);
            }
            catch (Exception ex)
            {
                msg = "{\"result\":\"-1\",\"msg\":\"" + ex.Message + "\"}";
                return;
            }

            if (string.IsNullOrEmpty(msg))
            {
                msg = "{\"result\":\"0\",\"msg\":\"\"}";
            }
            else
            {
                msg = "{\"result\":\"0\",\"msg\":\"" + msg + "\"}";
            }
        }

        private void EndProjects(out string msg)
        {
            msg = string.Empty;

            if (string.IsNullOrEmpty(Projectids))
            {
                msg = "{\"result\":\"-1\",\"msg\":\"项目ID不能为空\"}";
                return;
            }
            msg = string.Empty;


            try
            {
                BLL.AutoCall_ProjectInfo.Instance.UpdateAutoProjectStatus(Projectids, ProjectACStatus.P03_已结束);
                var nProjectid = -1;
                if (int.TryParse(Projectids, out nProjectid))
                {
                    BLL.ProjectLog.Instance.InsertProjectLog(nProjectid, ProjectLogOper.L4_结束项目, string.Format("结束自动外呼项目:{0}", Projectids));
                }
            }
            catch (Exception ex)
            {
                msg = "{\"result\":\"-1\",\"msg\":\"" + ex.Message + "\"}";
                return;
            }

            if (string.IsNullOrEmpty(msg))
            {
                msg = "{\"result\":\"0\",\"msg\":\"\"}";
            }
            else
            {
                msg = "{\"result\":\"0\",\"msg\":\"" + msg + "\"}";
            }
        }




        private bool CheckParameters(out string msg)
        {
            msg = string.Empty;

            if (string.IsNullOrEmpty(Projectids))
            {
                msg = "{\"result\":\"-1\",\"msg\":\"项目ID不能为空\"}";
                return true;
            }
            if (string.IsNullOrEmpty(Cdid))
            {
                msg = "{\"result\":\"-1\",\"msg\":\"外拨电话不能为空\"}";
                return true;
            }
            if (string.IsNullOrEmpty(SkillGid))
            {
                msg = "{\"result\":\"-1\",\"msg\":\"技能组不能为空\"}";
                return true;
            }
            return false;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}