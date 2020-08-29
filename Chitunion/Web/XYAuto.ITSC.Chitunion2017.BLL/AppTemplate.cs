using System.Data;
using XYAuto.ITSC.Chitunion2017.Entities.DTO;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    /// <summary>
    /// APP模板
    /// </summary>
    public class AppTemplate
    {
        public static readonly AppTemplate Instance = new AppTemplate();
        /// <summary>
        /// 2017-06-06 zlb
        /// 删除app广告模板
        /// </summary>
        /// <param name="TemplateID">模板ID</param>
        /// <param name="ErrorMsg">错误消息</param>
        /// <returns></returns>
        public int ToDeleteAppTemplate(int TemplateID, out string ErrorMsg)
        {
            ErrorMsg = "";
            var ur = Common.UserInfo.GetUserRole();
            if (!(ur.IsAE || ur.IsMedia))
            {
                ErrorMsg = "您没有删除模板权限";
                return -1;
            }
            int result = 0;
            if (ur.IsAE)
            {
                result = Dal.AppTemplate.Instance.ToDeleteAppTemplateAE(TemplateID, (int)AppTemplateEnum.已驳回);
            }
            else
            {
                result = Dal.AppTemplate.Instance.ToDeleteAppTemplate(TemplateID, ur.UserID, (int)AppTemplateEnum.已驳回);
            }
            if (result > 0)
            {
                Dal.AppTemplate.Instance.ToDeletePublish(TemplateID);
                return 1;
            }
            else
            {
                ErrorMsg = "删除失败";
                return -1;
            }
        }
        #region 获取DataTable前几条数据 
        /// <summary> 
        /// 获取DataTable前几条数据 
        /// </summary> 
        /// <param name="TopItem">前N条数据</param> 
        /// <param name="oDT">源DataTable</param> 
        /// <returns></returns> 
        public static DataTable DtSelectTop(int TopItem, DataTable oDT)
        {
            if (oDT.Rows.Count < TopItem) return oDT;

            DataTable NewTable = oDT.Clone();
            DataRow[] rows = oDT.Select("1=1");
            for (int i = 0; i < TopItem; i++)
            {
                NewTable.ImportRow((DataRow)rows[i]);
            }
            return NewTable;
        }
        #endregion
        /// <summary>
        /// 2017-06-09 zlb
        /// 查询上架广告最多的前十的城市
        /// </summary>
        /// <returns></returns>
        public DataTable SelectTopTenCitys()
        {

            DataTable dt = Dal.AppTemplate.Instance.SelectPublishCitys();

            DataTable dtCopy = dt.Clone();
            DataView dv = dt.DefaultView;
            dv.Sort = "Citycount Desc";
            dtCopy = dv.ToTable();
            DataTable dtRemoveCol = DtSelectTop(10, dtCopy);
            dtRemoveCol.Columns.Remove("Citycount");
            return dtRemoveCol;

        }
        /// <summary>
        /// 2017-06-09 zlb
        /// 查询上架的城市
        /// </summary>
        /// <returns></returns>
        public DataTable SelectPublishCitys()
        {
            DataTable dt = Dal.AppTemplate.Instance.SelectPublishCitys();
            dt.Columns.Remove("Citycount");
            return dt;
        }


        public AuditTemplateDTORes AuditTemplate(int templateID, int optType, string rejectReason, ref string msg)
        {
            if (!optType.Equals((int)AppTemplateEnum.已通过) && !optType.Equals((int)AppTemplateEnum.已驳回))
            {
                msg = "操作类型错误";
                return null;
            }
            if (optType.Equals((int)AppTemplateEnum.已驳回) && string.IsNullOrWhiteSpace(rejectReason))
            {
                msg = "缺少驳回原因";
                return null;
            }
            var ur = Common.UserInfo.GetUserRole();
            if (!ur.IsYY && !ur.IsAdministrator)
            {
                msg = "无权限";
                return null;
            }
            string notice = "审核失败";
            var res = Dal.AdTemplate.AppAdTemplate.Instance.AuditTemplate(templateID, optType, rejectReason, ur.UserID, ref notice);
            msg = res != null ? "审核成功" : notice;
            return res;
        }

        public bool CheckCanAddModifyTemplate(int baseTemplateID)
        {
            var ur = Common.UserInfo.GetUserRole();
            string rightSql = string.Empty;
            if (ur.IsAdministrator || ur.IsYY)
                rightSql = BLL.Util.CreateRightSql(Util.CreateSqlDependOnEnum.系统, ur);
            else if (ur.IsAE)
                rightSql = BLL.Util.CreateRightSql(Util.CreateSqlDependOnEnum.角色, ur);
            else
                rightSql = " and 1 = 2";
            return Dal.AdTemplate.AppAdTemplate.Instance.CheckCanAddModifyTemplate(baseTemplateID, rightSql);
        }


    }
}
