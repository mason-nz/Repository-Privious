using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities.BatchMedia;

namespace XYAuto.BUOC.IP2017.BLL.BatchMedia
{
    public class BatchMedia
    {
        public static readonly BatchMedia Instance = new BatchMedia();
        public List<Business.DTO.ResponseDto.V1_2_4.ResOperateinfo> GetListByMedia(int MediaType, string NumberOrName)
        {
            if (Dal.MediaLabelResult.MediaLabelResult.Instance.GetCountByMedia(MediaType, NumberOrName) == 0)
            {
                return Util.DataTableToList<Business.DTO.ResponseDto.V1_2_4.ResOperateinfo>(Dal.MediaLabelResult.MediaLabelResult.Instance.GetListByMedia(MediaType, NumberOrName));
            }
            else
                return Util.DataTableToList<Business.DTO.ResponseDto.V1_2_4.ResOperateinfo>(Dal.BatchMedia.BatchMedia.Instance.GetListByMedia(MediaType, NumberOrName));
        }

        public List<Business.DTO.ResponseDto.V1_2_4.ResInputListCarOperateinfoDto> GetListByCar(int brandID, int serialID)
        {
            if (Dal.MediaLabelResult.MediaLabelResult.Instance.GetCountByCar(brandID, serialID) == 0)
            {
                return Util.DataTableToList<Business.DTO.ResponseDto.V1_2_4.ResInputListCarOperateinfoDto>(Dal.MediaLabelResult.MediaLabelResult.Instance.GetListByCar(brandID, serialID));
            }
            return Util.DataTableToList<Business.DTO.ResponseDto.V1_2_4.ResInputListCarOperateinfoDto>(Dal.BatchMedia.BatchMedia.Instance.GetListByCar(brandID, serialID));
        }

        public Entities.BatchMedia.BatchMedia GetModelByMedia(Entities.BatchMedia.QueryBatchMedia query)
        {
            return Util.DataTableToEntity<Entities.BatchMedia.BatchMedia>(Dal.BatchMedia.BatchMedia.Instance.GetModelByMedia(query));
        }
        public Entities.BatchMedia.BatchMedia GetModelByCar(Entities.BatchMedia.QueryBatchMedia query)
        {
            return Util.DataTableToEntity<Entities.BatchMedia.BatchMedia>(Dal.BatchMedia.BatchMedia.Instance.GetModelByCar(query));
        }
        public Entities.BatchMedia.BatchMedia GetModelByRecID(int batchMediaID)
        {
            return Dal.BatchMedia.BatchMedia.Instance.GetModelByRecID(batchMediaID);
        }
        public int Insert(Entities.BatchMedia.BatchMedia entity)
        {
            return Dal.BatchMedia.BatchMedia.Instance.Insert(entity);
        }

        #region 获取媒体信息
        public MediaModel GetMediaModel(int MediaType, string NumberOrName)
        {
            return Dal.BatchMedia.BatchMedia.Instance.GetMediaModel(MediaType, NumberOrName);
        }
        #endregion        
        #region 更新待审状态，提交人
        public int UpdatePendingSubmitTime(int batchMediaID, int batchAuditID)
        {
            return Dal.BatchMedia.BatchMedia.Instance.UpdatePendingSubmitTime(batchMediaID, batchAuditID);
        }
        #endregion     
    }
}
