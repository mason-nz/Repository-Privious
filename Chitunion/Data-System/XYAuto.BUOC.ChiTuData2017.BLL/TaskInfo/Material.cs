/********************************************************
*创建人：hant
*创建时间：2017/12/19 16:26:06 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;

namespace XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo
{
    public class Material
    {
        public static readonly Material Instance = new Material();


        public ResponseTaskMaterialList GetTaskMaterialList(RequestTaskMaterialList req, ref string code, ref string msg)
        {
            ResponseTaskMaterialList list = new ResponseTaskMaterialList();
            try
            {
                //参数验证
                if (Authentication.Instance.ParaValid<RequestTaskMaterialList>(req, ref code, ref msg))
                {
                    //用户验证
                    if (Authentication.Instance.Access(Convert.ToInt32(req.appId), req.appkey, ref msg, ref code))
                    {
                        string sign = Authentication.Instance.GetSign<RequestTaskMaterialList>(req);
                        //签名验证
                        if (Authentication.Instance.SignValid(req.sign, sign, ref code, ref msg))
                        {
                            //调用次数
                            if (Authentication.Instance.CallNumber(Convert.ToInt32(req.appId), req.appkey, ref msg, ref code))
                            {
                                DataSet ds = GetTaskMaterialListHeadAndFoot(Convert.ToInt32(req.taskid));
                                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    list.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                                    list.MaterielUrl = ds.Tables[0].Rows[0]["MaterialUrl"].ToString();
                                    Head head = new Head();
                                    head.Content = ds.Tables[0].Rows[0]["Content"].ToString();
                                    list.Head = head;
                                    Foot foot = new Foot();
                                    foot.Content = ds.Tables[0].Rows[0]["FootContentUrl"].ToString();
                                    list.Foot = foot;
                                   
                                }
                                List<Entities.Task.TaskMaterialListWaist> listwaist = GetTaskMaterialListWaist(Convert.ToInt32(req.taskid));
                                if (listwaist != null && listwaist.Count > 0)
                                {
                                    List<Waist> wlist = new List<Waist>();
                                    foreach (var item in listwaist)
                                    {
                                        Waist wasit = new Waist();
                                        wasit.Title = item.Title;
                                        wasit.Headimg = item.Headimg;
                                        wasit.Content = item.Content;
                                        wlist.Add(wasit);
                                    }
                                    list.Waist = wlist;
                                }
                                code = "1";
                                msg = "success";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error($"GetTaskMaterialList :{ex.Message}");
                msg = "系统错误";
                code = "-1";
            }
            return list;
        }

        private DataSet GetTaskMaterialListHeadAndFoot(int recid)
        {
            return Dal.TaskInfo.Material.Instance.GetTaskMaterialListHeadAndFoot(recid);
        }

        private List<Entities.Task.TaskMaterialListWaist> GetTaskMaterialListWaist(int recid)
        {
            return Dal.TaskInfo.Material.Instance.GetTaskMaterialListWaist(recid);
        }
    }
}
