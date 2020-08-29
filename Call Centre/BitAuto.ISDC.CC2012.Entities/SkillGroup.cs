using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class SkillGroup
    {
        public SkillGroup()
        {
            _sgid = Constant.INT_INVALID_VALUE;
            _manufacturersgid = Constant.STRING_INVALID_VALUE;
            _name = Constant.STRING_INVALID_VALUE;
            _status = Constant.INT_INVALID_VALUE;
            _cdid = Constant.INT_INVALID_VALUE;
            _regionid = Constant.INT_INVALID_VALUE;
            _createtime = Constant.DATE_INVALID_VALUE;
            _updatetime = Constant.DATE_INVALID_VALUE;
            _updatedetail = Constant.STRING_EMPTY_VALUE;
        }
        #region Model
        private int _sgid;
        private string _manufacturersgid;
        private string _name;
        private int? _status;
        private int? _cdid;
        private int? _regionid;
        private DateTime? _createtime;
        private DateTime? _updatetime;
        private string _updatedetail;
        /// <summary>
        /// 技能组ID
        /// </summary>
        public int SGID
        {
            set { _sgid = value; }
            get { return _sgid; }
        }
        /// <summary>
        /// 技能组所属业务（1：热线；2在线）
        /// </summary>
        public string ManufacturerSGID
        {
            set { _manufacturersgid = value; }
            get { return _manufacturersgid; }
        }
        /// <summary>
        /// 技能组名称
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 技能组状态：0为使用状态；1为停用；-1为删除
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 热线ID
        /// </summary>
        public int? CDID
        {
            set { _cdid = value; }
            get { return _cdid; }
        }
        /// <summary>
        /// 区域ID（1:北京；2：西安）
        /// </summary>
        public int? RegionID
        {
            set { _regionid = value; }
            get { return _regionid; }
        }
        /// <summary>
        /// 技能组创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 业务线数据更新时间
        /// </summary>
        public DateTime? UpdateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 业务线数据更新内容
        /// </summary>
        public string UpdateDetail
        {
            set { _updatedetail = value; }
            get { return _updatedetail; }
        }
        #endregion Model

    }
}
