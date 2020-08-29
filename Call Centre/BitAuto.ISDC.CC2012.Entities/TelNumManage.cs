using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    /// 热线号码信息
    /// <summary>
    /// 热线号码信息
    /// </summary>
    public class TelNum
    {
        /// 热线号码
        /// <summary>
        /// 热线号码
        /// </summary>
        public string Tel = "";
        /// 出局号码
        /// <summary>
        /// 出局号码
        /// </summary>
        public string Out = "";
        /// 热线名称
        /// <summary>
        /// 热线名称
        /// </summary>
        public string Name = "";
        /// 大于0表示呼入
        /// <summary>
        /// 大于0表示呼入
        /// </summary>
        public int HotlineID = 0;
        /// 技能组
        /// <summary>
        /// 技能组
        /// </summary>
        public List<TelSkill> Skills = null;
        /// 热线类型
        /// <summary>
        /// 热线类型
        /// </summary>
        public WorkOrderDataSource DataSource;
        /// 长名称
        /// <summary>
        /// 长名称
        /// </summary>
        public string LongName = "";
        /// 多选ID
        /// <summary>
        /// 多选ID
        /// </summary>
        public int MutilID = -1;
        /// 区号
        /// <summary>
        /// 区号
        /// </summary>
        public string AreaCode = "";
    }

    /// 技能
    /// <summary>
    /// 技能
    /// </summary>
    public class TelSkill
    {
        public string ManufacturerSGID = "";
        public string Name = "";

        public TelSkill(string id, string name)
        {
            ManufacturerSGID = id;
            Name = name;
        }
    }

    /// 技能组管理类
    /// <summary>
    /// 技能组管理类
    /// </summary>
    public class TelSkillGroup
    {
        /// <summary>
        /// 技能集合，int=热线id，List<TelSkill>对应技能组
        /// </summary>
        public Dictionary<int, List<TelSkill>> SkillDic = new Dictionary<int, List<TelSkill>>();

        public TelSkillGroup()
        {
        }

        public List<TelSkill> TotalSkill
        {
            get
            {
                List<TelSkill> total = new List<TelSkill>();
                foreach (int key in SkillDic.Keys)
                {
                    List<TelSkill> list = SkillDic[key];
                    total.AddRange(list);
                }
                return total;
            }
        }

        /// 呼入技能组
        /// <summary>
        /// 呼入技能组
        /// </summary>
        /// <param name="skcode"></param>
        /// <returns></returns>
        private string GetSkillGroupText(string skcode)
        {
            TelSkill item = TotalSkill.FirstOrDefault(x => x.ManufacturerSGID == skcode);
            if (item != null)
            {
                return item.Name;
            }
            else return "";
        }
    }

    /// 热线号码管理类
    /// <summary>
    /// 热线号码管理类
    /// </summary>
    public class TelNumManage
    {
        public List<TelNum> TelNumList = new List<TelNum>();

        public TelNumManage(TelSkillGroup TelSkillGroup)
        {
        }

        /// 检查是否是西安的热线
        /// <summary>
        /// 检查是否是西安的热线
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public bool CheckTelIsInXANos(string tel)
        {
            TelNum telnum = TelNumList.FirstOrDefault(x => tel.EndsWith(x.Tel));
            if (telnum != null && telnum.HotlineID > 0)
            {
                return true;
            }
            return false;
        }
    }
}
