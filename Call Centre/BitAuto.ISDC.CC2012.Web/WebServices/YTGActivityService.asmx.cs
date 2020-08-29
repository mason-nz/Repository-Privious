using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.WebServices
{
    /// <summary>
    /// YTGActivityService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class YTGActivityService : System.Web.Services.WebService
    {
        [WebMethod(Description = "新建或者修改易团购活动")]
        public Result InsertOrUpdateYTGActivity(string key, YTGActivityInfo obj)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            BLL.Util.LogForWeb("info", "新建或者修改易团购活动：InsertOrUpdateYTGActivity");
            BLL.Util.LogForWeb("info", obj.ToString());
            Result result = new Result();
            string msg = "";
            try
            {
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(key, 0, ref msg, "权限错误"))
                {
                    result.Success = false;
                    result.Message = msg;
                }
                else
                {
                    //逻辑处理
                    if (CheckYTGActivityInfo(obj, out msg))
                    {
                        //修改报名结束时间+18小时
                        obj.SignEndTime = obj.ValueOrDefault_SignEndTime.Date.AddHours(18);

                        //查询是否存在
                        YTGActivityInfo info = BLL.YTGActivity.Instance.GetComAdoInfo<YTGActivityInfo>(obj.ActivityID);
                        if (info == null)
                        {
                            BLL.Util.LogForWeb("info", "新增操作");
                            obj.CreateTime = DateTime.Now;
                            obj.LastUpdateTime = null;
                            BLL.YTGActivity.Instance.InsertComAdoInfo<YTGActivityInfo>(obj);
                        }
                        else
                        {
                            BLL.Util.LogForWeb("info", "修改操作");
                            //忽略状态和创建时间
                            obj.IsModify_Status = false;
                            obj.IsModify_CreateTime = false;
                            obj.LastUpdateTime = DateTime.Now;
                            BLL.YTGActivity.Instance.UpdateComAdoInfo<YTGActivityInfo>(obj);
                        }

                        //入库成功
                        sw.Stop();
                        result.Success = true;
                        result.Message = "耗时（ms）：" + sw.Elapsed.TotalMilliseconds;
                    }
                    else
                    {
                        //校验失败
                        result.Success = false;
                        result.Message = msg;
                    }
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                result.Success = false;
                result.Message = ex.Message;
            }
            BLL.Util.LogForWeb("info", "新建或者修改易团购活动：处理结果：" + result.ToString() + "\r\n\r\n");
            return result;
        }
        /// 校验数据
        /// <summary>
        /// 校验数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool CheckYTGActivityInfo(YTGActivityInfo obj, out string msg)
        {
            msg = "";
            if (obj == null)
            {
                msg = "传入数据对象(obj)为空";
                return false;
            }
            else if (string.IsNullOrEmpty(obj.ActivityID))
            {
                msg = "活动ID(ActivityID)为空";
                return false;
            }
            else if (obj.SignBeginTime == null)
            {
                msg = "报名时间(SignBeginTime)为空";
                return false;
            }
            else if (obj.SignEndTime == null)
            {
                msg = "报名时间(SignEndTime)为空";
                return false;
            }
            else if (obj.SignBeginTime.Value > obj.SignEndTime.Value)
            {
                msg = "报名时间(SignBeginTime)大于报名时间(SignEndTime)";
                return false;
            }
            else if (string.IsNullOrEmpty(obj.CarSerials))
            {
                msg = "活动车型(CarSerials)为空";
                return false;
            }
            else if (obj.Status == null)
            {
                msg = "活动状态(Status)为空";
                return false;
            }
            else if (obj.Status.Value != 1)
            {
                msg = "只接受活动进行中的数据";
                return false;
            }
            List<string> ids = BLL.YTGActivity.Instance.GetCarSerialIDsByIds(obj.CarSerials);
            if (ids.Count == 0)
            {
                msg = "查不到任何车型(CarSerials)数据:" + obj.CarSerials + ".";
                return false;
            }
            else
            {
                obj.CarSerials = string.Join(",", ids.ToArray());
            }

            return true;
        }

        [WebMethod(Description = "结束易团购活动")]
        public Result EndYTGActivity(string key, string activityid)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            BLL.Util.LogForWeb("info", "结束易团购活动：EndYTGActivity");
            Result result = new Result();
            string msg = "";
            try
            {
                if (!BLL.CallRecord_ORIG_Authorizer.Instance.Verify(key, 0, ref msg, "权限错误"))
                {
                    result.Success = false;
                    result.Message = msg;
                }
                else
                {
                    //逻辑处理                   
                    YTGActivityInfo info = BLL.YTGActivity.Instance.GetComAdoInfo<YTGActivityInfo>(activityid);
                    if (info == null)
                    {
                        result.Success = false;
                        result.Message = "活动（" + activityid + "）不存在";
                    }
                    else
                    {
                        //1 更新活动
                        info.Status = 3;//已终止
                        info.LastUpdateTime = DateTime.Now;
                        BLL.YTGActivity.Instance.UpdateComAdoInfo<YTGActivityInfo>(info);
                        BLL.Util.LogForWeb("info", "活动（" + activityid + "）设置为终止（3）状态");
                        //2 结束项目
                        int a = BLL.YTGActivity.Instance.EndProjectForYTGActivity(activityid);
                        BLL.Util.LogForWeb("info", "关闭相应的项目" + a + "条");
                        //3 结束任务
                        int b = BLL.YTGActivity.Instance.EndTaskForYTGActivity(activityid, "活动终止");
                        BLL.Util.LogForWeb("info", "撤销相应的任务" + b + "条");
                        //结果
                        sw.Stop();
                        result.Success = true;
                        result.Message = "耗时（ms）：" + sw.Elapsed.TotalMilliseconds;
                    }
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                result.Success = false;
                result.Message = ex.Message;
            }
            BLL.Util.LogForWeb("info", "结束易团购活动：处理结果：" + result.ToString() + "\r\n\r\n");
            return result;
        }
    }
}
