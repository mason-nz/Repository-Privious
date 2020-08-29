using System;
using System.Collections.Generic;
using BitAuto.ISDC.CC2012.Entities.Constants;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{

    [Serializable]
    public class QueryEmployeeSuper
    {
        public QueryEmployeeSuper()
        {
            _userID = Constant.INT_INVALID_VALUE;
            _trueName = Constant.STRING_INVALID_VALUE;
            _agentNum = Constant.STRING_INVALID_VALUE;
            _rightType = Constant.INT_INVALID_VALUE;
            _role = Constant.STRING_INVALID_VALUE;
            _bgId = Constant.INT_INVALID_VALUE;
            _bgIds = Constant.STRING_INVALID_VALUE;
            _adname = Constant.STRING_INVALID_VALUE;
            _onlyccdepart = true;
            _regionid = Constant.INT_INVALID_VALUE;
            filterDepartIDStr = Constant.STRING_INVALID_VALUE;
            _usercodeNotEmpty = false;
        }
        #region Model
        private int _userID;
        private string _trueName;
        private string _agentNum;
        private int _rightType;
        private string _role;
        private int _bgId;
        private string _bgIds;
        private string _adname;
        private bool _onlyccdepart;
        private int _regionid;
        private string filterDepartIDStr;
        private bool _usercodeNotEmpty;
        #endregion

        /// <summary>
        /// 用户ID（EmployeeID）
        /// </summary>
        public int UserID
        {
            set { _userID = value; }
            get { return _userID; }
        }

        public string AgentNum
        {
            set { _agentNum = value; }
            get { return _agentNum; }
        }

        public string TrueName
        {
            set { _trueName = value; }
            get { return _trueName; }
        }

        public int RightType
        {
            set { _rightType = value; }
            get { return _rightType; }
        }

        /// <summary>
        /// 角色编号，以逗号间隔
        /// </summary>
        public string Role
        {
            set { _role = value; }
            get { return _role; }
        }

        public int BGID
        {
            set { _bgId = value; }
            get { return _bgId; }
        }

        public string BGIDs
        {
            set { _bgIds = value; }
            get { return _bgIds; }
        }

        public string ADName
        {
            set { _adname = value; }
            get { return _adname; }
        }

        /// <summary>
        /// 是否只有呼叫中心部门
        /// </summary>
        public bool OnlyCCDepart
        {
            set { _onlyccdepart = value; }
            get { return _onlyccdepart; }
        }

        /// <summary>
        /// 归属区域
        /// </summary>
        public int RegionID
        {
            set { _regionid = value; }
            get { return _regionid; }
        }

        public string FilterDepartIDStr
        {
            set { filterDepartIDStr = value; }
            get { return filterDepartIDStr; }
        }

        /// <summary>
        /// UserId的特殊查询语句
        /// </summary>
        public string SelectUserIdSql { get; set; }

        //查询分组时，包含本人
        public int ContainLoginUserID { get; set; }

        //区别PartID和YichePartID
        public string PartIDType { get; set; }

        /// <summary>
        /// UserCode是否可空
        /// </summary>
        public bool UsercodeNotEmpty
        {
            set { _usercodeNotEmpty = value; }
            get { return _usercodeNotEmpty; }
        }
    }
}
