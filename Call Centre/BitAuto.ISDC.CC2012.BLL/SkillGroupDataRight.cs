using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BitAuto.ISDC.CC2012.BLL
{
    public class SkillGroupDataRight
    {
        #region Instance
        public static readonly SkillGroupDataRight Instance = new SkillGroupDataRight();
        #endregion

        public int UpdateUserSkillDataRight(int UserID, string SGIDAndPriority, int CreateUserID, int RegionID)
        {
            return Dal.SkillGroupDataRight.Instance.UpdateUserSkillDataRight(UserID, SGIDAndPriority, CreateUserID, RegionID);
        }
        public DataTable GetAllHotlineSkillGroup()
        {
            return Dal.SkillGroupDataRight.Instance.GetAllHotlineSkillGroup();
        }
        public string GetUserSkillGroup(int UserID)
        {
            return Dal.SkillGroupDataRight.Instance.GetUserSkillGroup(UserID);
        }
        public DataTable GetUserSkillGroupIdAndPriorty(int UserId)
        {
            return Dal.SkillGroupDataRight.Instance.GetUserSkillGroupIdAndPriorty(UserId);
        }
        public string GetManufactureSkillGroupID(int SGID)
        {
            return Dal.SkillGroupDataRight.Instance.GetManufactureSkillGroupID(SGID);
        }
        /// 根据字母ID获取技能组名称
        /// <summary>
        /// 根据字母ID获取技能组名称
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public string GetSkillNameByMID(string MID)
        {
            return Dal.SkillGroupDataRight.Instance.GetSkillNameByMID(MID);
        }

        public DataTable GetAutoCallSkillGroup()
        {
            return Dal.SkillGroupDataRight.Instance.GetAutoCallSkillGroup();
        }

        /// 根据数字ID获取技能组名称
        /// <summary>
        /// 根据数字ID获取技能组名称
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public string GetSkillNameBySGID(int SGID)
        {
            return Dal.SkillGroupDataRight.Instance.GetSkillNameBySGID(SGID);
        }

        /// 根据热线获取技能组
        /// <summary>
        /// 根据热线获取技能组
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public DataTable GetHotlineSkillGroupByTelMainNum(string tel)
        {
            return Dal.SkillGroupDataRight.Instance.GetHotlineSkillGroupByTelMainNum(tel);
        }
        /// 技能组与热线的对照表
        /// <summary>
        /// 技能组与热线的对照表
        /// </summary>
        /// <returns></returns>
        public DataTable GetHotlineRelationSkillInfo(string linenums)
        {
            return Dal.SkillGroupDataRight.Instance.GetHotlineRelationSkillInfo(linenums);
        }
    }
}
