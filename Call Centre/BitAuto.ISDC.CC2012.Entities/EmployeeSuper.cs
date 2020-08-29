using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    class EmployeeSuper
    {
        public EmployeeSuper()
		{
		 _userID = Constant.INT_INVALID_VALUE;
		 _trueName = Constant.STRING_INVALID_VALUE;
		 _agentNum = Constant.INT_INVALID_VALUE;
		 _rightType = Constant.INT_INVALID_VALUE;		 
		}
		#region Model
        private int _userID;
        private string _trueName;
        private int _agentNum;
        private int _rightType;
        #endregion

        /// <summary>
        /// 用户ID（EmployeeID）
        /// </summary>
        public int UserID
        {
            set { _userID = value; }
            get { return _userID; }
        }

        public int AgentNum
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
    }
}
