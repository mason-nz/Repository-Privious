using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.DTO.V1_1_11;

namespace XYAuto.ITSC.Chitunion2017.BLL.Materiel
{
    public class MaterielExtend
    {
        public static readonly MaterielExtend Instance = new MaterielExtend();

        public bool UpdateContractNumber(UpdateContractNumberReqDTO req, ref string msg)
        {
            if (!string.IsNullOrEmpty(req.ContractNumber) && req.ContractNumber.Length > 15)
            {
                msg = "合同号不能大于15位";
                return false;
            }
            return Dal.Materiel.MaterielExtend.Instance.UpdateContractNumber(req.MaterielID, req.ContractNumber);
        }

        public Entities.Materiel.DTO.ResGetDivideUserDTO GetDivideUser()
        {         
            return new Entities.Materiel.DTO.ResGetDivideUserDTO()
            {
                List = Dal.Materiel.MaterielExtend.Instance.GetDivideUser()
            }; 
        }
        public Entities.Materiel.DTO.ResGetArticleInfoDTO GetArticleInfo(int ArticleId)
        {
            return Dal.Materiel.MaterielExtend.Instance.GetArticleInfo(ArticleId);
        }
    }
}
