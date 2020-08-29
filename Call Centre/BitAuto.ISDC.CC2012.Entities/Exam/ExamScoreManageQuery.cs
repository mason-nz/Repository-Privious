using System;
using System.Collections.Generic;
using System.Text;
using BitAuto.ISDC.CC2012.Entities.Constants;

namespace BitAuto.ISDC.CC2012.Entities
{
    [Serializable]
    public class ExamScoreManageQuery
    {
        public ExamScoreManageQuery()
        {
            _projectname = Constant.STRING_INVALID_VALUE;
            _papername = Constant.STRING_INVALID_VALUE;
            _begintime = Constant.DATE_INVALID_VALUE;
            _endtime = Constant.DATE_INVALID_VALUE;
            _examcategory = Constant.STRING_INVALID_VALUE; //分类串
            _truename = Constant.STRING_INVALID_VALUE; //  考试人员ID
            _rolename = Constant.STRING_INVALID_VALUE;
            _ismarking = Constant.STRING_INVALID_VALUE;
            _bgids = Constant.STRING_INVALID_VALUE;
        }
        #region Model
        private string _projectname;
        private string _papername;
        private DateTime? _begintime;
        private DateTime? _endtime;
        private string _examcategory;
        private string _truename;
        private string _rolename;
        private string _ismarking;
        private string _bgids;

        /// <summary>
        /// 
        /// </summary>
        public string ProjectName
        {
            set { _projectname = value; }
            get { return _projectname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PaperName
        {
            set { _papername = value; }
            get { return _papername; }
        }
        /// <summary>
        /// 考试开始时间
        /// </summary>
        public DateTime? BeginTime
        {
            set { _begintime = value; }
            get { return _begintime; }
        }
        /// <summary>
        /// 考试结束时间
        /// </summary>
        public DateTime? EndTime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        public string ExamCategory
        {
            set { _examcategory = value; }
            get { return _examcategory; }
        }
        public string TrueName
        {
            set { _truename = value; }
            get { return _truename; }
        }
        /// <summary>
        /// 角色用，隔开
        /// </summary>
        public string RoleName
        {
            set { _rolename = value; }
            get { return _rolename; }
        }
        public string IsMarking
        {
            set { _ismarking = value; }
            get { return _ismarking; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BGIDS
        {
            get { return _bgids; }
            set { _bgids = value; }
        }
        #endregion Model
    }
}