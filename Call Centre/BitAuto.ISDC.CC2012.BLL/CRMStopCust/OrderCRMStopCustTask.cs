using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.BLL
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// ҵ���߼���OrderCRMStopCustTask ��ժҪ˵����
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-07-01 12:21:55 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class OrderCRMStopCustTask
    {
        public static readonly OrderCRMStopCustTask Instance = new OrderCRMStopCustTask();
        Random random = new Random();

        protected OrderCRMStopCustTask() { }

        /// �õ�һ������ʵ�� ǿ� 2016-8-16
        /// <summary>
        /// �õ�һ������ʵ�� ǿ� 2016-8-16
        /// </summary>
        public OrderCRMStopCustTaskInfo GetOrderCRMStopCustTask(string taskID)
        {
            //�ñ���������Ϣ�����Զ�������/�����ֶ�
            return Dal.OrderCRMStopCustTask.Instance.GetOrderCRMStopCustTask(taskID);
        }

        public string Insert(OrderCRMStopCustTaskInfo model)
        {
            //3+12+4
            string taskid = CreatTaskID();
            model.TaskID = taskid;
            CommonBll.Instance.InsertComAdoInfo(model);
            return taskid;
        }
        public string CreatTaskID()
        {
            string taskid = "CKU" + DateTime.Now.ToString("yyMMddHHmmss") + random.Next(1000, 9999);
            if (GetOrderCRMStopCustTask(taskid) == null)
                return taskid;
            else return CreatTaskID();
        }
        public bool Update(OrderCRMStopCustTaskInfo model)
        {
            return CommonBll.Instance.UpdateComAdoInfo(model);
        }

        /// ��ѯ���� ǿ� 2016-8-16
        /// <summary>
        /// ��ѯ���� ǿ� 2016-8-16
        /// </summary>
        /// <param name="taskIDs"></param>
        /// <returns></returns>
        public DataTable GetListByTaskIDs(string taskIDs)
        {
            return Dal.OrderCRMStopCustTask.Instance.GetListByTaskIDs(taskIDs);
        }
        /// ��ѯ���� ǿ� 2016-8-16 
        /// <summary>
        /// ��ѯ���� ǿ� 2016-8-16 
        /// </summary>
        /// <param name="relationID"></param>
        /// <returns></returns>
        public OrderCRMStopCustTaskInfo GetEntityByRelationID(int relationID)
        {
            return Dal.OrderCRMStopCustTask.Instance.GetEntityByRelationID(relationID);
        }
    }
}

