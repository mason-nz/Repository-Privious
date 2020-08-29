using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.BatchMediaAudit
{
    public class BatchMediaAudit
    {
        public static readonly BatchMediaAudit Instance = new BatchMediaAudit();
        public int Insert(Entities.BatchMediaAudit.BatchMediaAudit entity)
        {
            return Dal.BatchMediaAudit.BatchMediaAudit.Instance.Insert(entity);
        }
        #region 根据任务类型、媒体类型
        public Entities.BatchMediaAudit.BatchMediaAudit GetModelPending(Entities.BatchMediaAudit.QueryBatchMediaAudit query)
        {
            return Dal.BatchMediaAudit.BatchMediaAudit.Instance.GetModelPending(query.TaskType, query.MediaType, query.MediaNumber, query.MediaName);
        }
        public Entities.BatchMediaAudit.BatchMediaAudit GetModelPending(int brandID, int serialID)
        {
            return Dal.BatchMediaAudit.BatchMediaAudit.Instance.GetModelPending(brandID, serialID);
        }
        #endregion
    }
}
